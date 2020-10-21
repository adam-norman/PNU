using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PWRSet.CS;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using Pursuit_not_Super_Reduct_only_Algorithm;
using DClassifier.CS;
using System.Web;
using DClassifier;
namespace PWRSet
{
    public partial class PNU : Form
    {
        int RowNr = 0;
        List<List<ArrayList>> SkipChildsOfThusParents = new List<List<ArrayList>>();
        ArrayList InitSetlist = new ArrayList();
        ArrayList MainList = new ArrayList();
        ArrayList SkipperListComplement = new ArrayList();
        static int CurrentLevel = 1;
        static public int NrFeatures;
        DataRow dr;

        ArrayList CurrentFeatureStopList = new ArrayList();

        DataTable dt = new DataTable();
        DataTable dtTest = new DataTable();
        List<ArrayList> Fs = new List<ArrayList>();
        List<ArrayList> FsTest = new List<ArrayList>();
        ArrayList FS = new ArrayList();
        ArrayList Ds = new ArrayList();
        string category = "";
        ArrayList RowCuts = new ArrayList();
        ArrayList UnFoundLists = new ArrayList();
        ArrayList NotUniqueSetsPrevLevel = new ArrayList();
        ArrayList tmpNUList = new ArrayList();
        ArrayList tmpUList = new ArrayList();
        List<List<FeatureValues>> PublicNotUniq = new List<List<FeatureValues>>();
        List<ArrayList> PublicUniq = new List<ArrayList>();
        //
        List<FeatureValues> LstOfUniqueSets = new List<FeatureValues>();
        //List<int> LstOfUniqueSetsNrOfObjects = new List<int>();
        List<List<ArrayList>> LstOfRowsFeaturesSetsIDs = new List<List<ArrayList>>();
        // List<ArrayList> LstOfRowsFeaturesSetsIDsTmp = new List<ArrayList>();
        //
        string StartTime = DateTime.Now.ToString();
        int TotalUniqueSets = 0;
        private void CheckStopLevel()
        {
            // backgroundWorker1.RunWorkerAsync();

            if (chkShowCurrentLevel.Checked)
            {
                SetText("Current Level=" + CurrentLevel.ToString() + "  With only " + NotUniqueSetsPrevLevel.Count + " Not Unique Starting sets  were found in level" + (CurrentLevel - 1).ToString() + " and nonesense sets=" + NrOfNonesenseSets.ToString() + " with percentage=" + (100 * (double)NrOfNonesenseSets / (double)NotUniqueSetsPrevLevel.Count).ToString() + "  At Time:" + DateTime.Now.ToString() + Environment.NewLine);
                //txtres.AppendText("Current Level=" + CurrentLevel.ToString() + "  With only " + NotUniqueSetsPrevLevel.Count + " Not Unique Starting sets  were found in level" + (CurrentLevel - 1).ToString() + " and nonesense sets=" + NrOfNonesenseSets.ToString() + " with percentage=" + (100 * (double)NrOfNonesenseSets / (double)NotUniqueSetsPrevLevel.Count).ToString() + "  At Time:" + DateTime.Now.ToString() + Environment.NewLine);
                System.Diagnostics.Debug.Write("Current Level=" + CurrentLevel.ToString() + "  With only " + NotUniqueSetsPrevLevel.Count + " Not Unique Starting sets  were found in level" + (CurrentLevel - 1).ToString() + " and nonesense sets=" + NrOfNonesenseSets.ToString() + " with percentage=" + (100 * (double)NrOfNonesenseSets / (double)NotUniqueSetsPrevLevel.Count).ToString() + "  At Time:" + DateTime.Now.ToString() + Environment.NewLine);
                NrOfNonesenseSets = 0;
            }


            PublicNotUniq.Clear();
            tmpNUList = (ArrayList)NotUniqueSetsPrevLevel.Clone();
            NrOfNotUniqueThatWillBeChecked = 0;
            NotUniqueSetsPrevLevel.Clear();

            //ArrayList SharedRows = new ArrayList();
            //foreach (int f in InitiatorFeatures)
            //{
            //    foreach (FeatureValues fv in DsMeta[f])
            //    {
            //        if (dr[f].ToString() == fv.value.ToString())
            //        {
            //            foreach (int index in fv.RowsIndices)
            //            {
            //                if (!SharedRows.Contains(index))
            //                {
            //                    SharedRows.Add(index);
            //                }
            //            }
            //        }
            //    }
            //}

            for (int k = 0; k < tmpNUList.Count; k++)
            {

                List<FeatureValues> temp = new List<FeatureValues>();
                PublicNotUniq.Add(temp);
                ArrayList tt = new ArrayList();
                PublicUniq.Add(tt);

            }
            //XXXXXX
            Parallel.For(0, tmpNUList.Count, new ParallelOptions { MaxDegreeOfParallelism = 48 }, i =>
            {
                  //  for(int i=0;i< tmpNUList.Count; i++)
               // {
                ArrayList pl1 = ((FeatureValues)tmpNUList[i]).UnkSet;
                ArrayList pl = (ArrayList)pl1.Clone();
                for (int j = SkipperListComplement.IndexOf(pl[pl.Count - 1]) + 1; j < SkipperListComplement.Count; j++)
                {
                    pl.Add(SkipperListComplement[j]);
                    IsUnique(pl, i, ((FeatureValues)tmpNUList[i]).RowsIndices);
                    pl.RemoveAt(pl.Count - 1);
                }
                tmpNUList[i] = null;
            });
        }

        ArrayList AllUniqueLists = new ArrayList();
        ArrayList StatisticsList = new ArrayList();
        ArrayList AllUniqueListsEqClass = new ArrayList();
        string AppPath = "";
        List<ArrayList> ValCatsCounts = new List<ArrayList>();
        int[] ClearNrOfCategories(int c)
        {
            int[] v = new int[c];
            for (int i = 0; i < c; i++)
            {
                v[i] = -1;
            }
            return v;
        }
        public void GetNegationRules()
        {
            for (int CurFeature = 0; CurFeature < dt.Columns.Count - 1; CurFeature++)
            {
                for (int distinctValues = 0; distinctValues < DsMeta[CurFeature].Count; distinctValues++)
                {
                    FeatureValues _fv = DsMeta[CurFeature][distinctValues];
                    ArrayList FeatureRowsIndices = _fv.RowsIndices;
                    for (int c = 0; c < CategoriesMeta.Count; c++)
                    {
                        bool IsExist = false;
                        foreach (int i in FeatureRowsIndices)
                        {
                            if (CategoriesMeta[c].Contains(i))
                            {
                                IsExist = true;
                            }

                            if (IsExist == true)
                            {
                                break;
                            }
                        }
                        if (IsExist == false)
                        {
                            _fv.NegationCatIndices.Add(c);
             DsMeta[CurFeature][distinctValues] = _fv;
                        }
                    }
                }
            }
        }
        int NrOfRows = 0;
        int NrOfNonesenseSets = 0;

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);


        }

        private void Start_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        Stopwatch stopwatch = new Stopwatch();
        int TotalNumberOfObjects = 0;
        private void Start()
        {
            // timer1.Enabled = true;
            NrOfRows = dt.Rows.Count;
            GetNegationRules();
            GC.Collect();
            int CurrentFileRowNumber = 0;
            AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(AppPath + "\\StartObject.txt"))
            {
                using (StreamWriter sw = File.CreateText(AppPath + "\\StartObject.txt"))
                {
                    sw.WriteLine("0");
                }
            }
            else
            {
                CurrentFileRowNumber = GetCurrentRowNumberFromRowNumberFile(AppPath + "\\StartObject.txt");
            }
            if (!File.Exists(AppPath + "\\Refined.XML"))
            {
                using (StreamWriter sw = File.CreateText(AppPath + "\\Refined.XML"))
                {
                    sw.WriteLine("");
                }
            }
            SetText(DateTime.Now + Environment.NewLine);

            Stopwatch RestartableStopWatch = new Stopwatch();
            RestartableStopWatch.Start();
            stopwatch.Start();
             

            for (int x = 0; x < dt.Rows.Count; x++)
            {
                List<ArrayList> t1 = new List<ArrayList>();
                for (int y = 0; y < dt.Columns.Count - 1; y++)
                {
                    ArrayList t2 = new ArrayList();
                    t1.Add(t2);
                }
                SkipChildsOfThusParents.Add(t1);
            }
            TotalNumberOfObjects = dt.Rows.Count;
            foreach (DataRow drow in dt.Rows)
            {
                dr = drow;
                FillSkipperListWithEmptyFeatures(drow);
                string curStrSet = "";
                foreach (int f in SkipperListComplement)
                {
                    curStrSet += "," + f.ToString();
                }
                SetText("row:" + (RowNr + 1).ToString() + "  started at " + DateTime.Now.ToString().Replace("م", "PM ").Replace("ص", "AM ") + "___(" + SkipperListComplement.Count.ToString() + ")  [" + curStrSet.Substring(1) + "]" + Environment.NewLine);
                NotUniqueSetsPrevLevel.Clear();
                TimeSpan ts = RestartableStopWatch.Elapsed;
                SetLblText((RowNr + 1).ToString() + " of " + TotalNumberOfObjects.ToString());
                
                PowerSet();

              //  SaveNow(AppPath);
                RowNr++;
                //if (RowNr == 1)
                //{
                //    break;
                //}
            }
            stopwatch.Stop();
            timer1.Enabled = false;
            string Elapsedtime = stopwatch.Elapsed.ToString();
            //SetText(Environment.NewLine + "NoneSense:" + TotalNoneSense.ToString() + "   Total Subsumptions:" + TotalPartialReduct.ToString() + "  Total Not Unique:" + lblTotalNotUniqueOutput.Text);

            SetText(Environment.NewLine + "Total Number Of Searched Objects:" + dt.Rows.Count + Environment.NewLine + "Elapsedtime Time:" + Elapsedtime);
            GC.Collect();
            SaveNow(AppPath);
            MessageBox.Show("done");
        }
        long TotalNoneSense = 0;
        long TotalPartialReduct = 0;
        int MaxSubsetLength = 0;
        long TotalNotUnique = 0;
        long NrOfNotUniqueThatWillBeChecked = 0;
        int MaxRecurrsiveRuleCount = 0;
        private void SaveNow(string AppPath)
        {
            //TotalNotUnique = 0;
            StringBuilder sbXML = new StringBuilder();
            string ClassMeta = "";
            int p = 0;
            foreach (ArrayList l in CategoriesMeta)
            {
                ClassMeta +=Ds[p].ToString()+","+ l.Count.ToString()+",";
                p++;
            }
            p = 0;
            sbXML.Append("<Root M=\"" + dt.Rows.Count.ToString() + "\" N=\"" + (dt.Columns.Count - 1).ToString() + "\">" + Environment.NewLine + "<Inference>" + Environment.NewLine + "<Cluster>"+Environment.NewLine);
            for (int i = 0; i < LstOfRowsFeaturesSetsIDs.Count; i++)//rows
            {
                for (int j = 0; j < LstOfRowsFeaturesSetsIDs[i].Count; j++)//features
                {
                    ArrayList IDs = LstOfRowsFeaturesSetsIDs[i][j];
                    foreach (int id in IDs)
                    {
                        FeatureValues tmpSet = LstOfUniqueSets[id];
                        LstOfUniqueSets[id] = null;
                        if (tmpSet != null)
                        {
              //              TotalNotUnique++;
                            if (MaxRecurrsiveRuleCount < tmpSet.RowsIndices.Count)
                            {
                                MaxRecurrsiveRuleCount = tmpSet.RowsIndices.Count;
                            }
                            sbXML.Append("<Rule Category=\"" + Ds[Int32.Parse(dt.Rows[i][CategoryColumnName].ToString())].ToString() + "\" Oc=\"" + tmpSet.RowsIndices.Count.ToString() + "\" Fc=\"" + tmpSet.UnkSet.Count.ToString() + "\"  OXF=\"" + (tmpSet.UnkSet.Count * tmpSet.RowsIndices.Count).ToString() + "\" >" + Environment.NewLine);
                            if (MaxSubsetLength < tmpSet.UnkSet.Count)
                            {
                                MaxSubsetLength = tmpSet.UnkSet.Count;
                            }
                            foreach (int Feature in tmpSet.UnkSet)
                            {
                                if (txtFilePath.Text.ToLower().IndexOf(".csv") >= 0)
                                {
                                    sbXML.Append(Fs[Feature][Int32.Parse(dt.Rows[i][Feature].ToString())] + Environment.NewLine);
                                }
                                else
                                {
                                    sbXML.Append(Fs[Feature][0] + Environment.NewLine);
                                }
                            }
                            sbXML.Append("</Rule>" + Environment.NewLine);
                        }
                    }
                }
            }
            sbXML.Append(Environment.NewLine + "</Cluster>" + Environment.NewLine + "</Inference>" + Environment.NewLine);
            sbXML.Append("</Root>");
            string rand = rn.Next(11111, 99999).ToString();
            StreamWriter asdf = new StreamWriter(AppPath + "\\Refined" + rand + ".XML");
            asdf.Write(sbXML.ToString());
            asdf.Close();
            sbXML.Clear();
            SetText(Environment.NewLine +"Total Unique Rules"+  TotalUniqueSets.ToString());
            SetText(Environment.NewLine + "Maximum recursive rule R=" + MaxRecurrsiveRuleCount.ToString());
            SetText(Environment.NewLine + "Total Nonesense=" + TotalNoneSense.ToString());
            SetText(Environment.NewLine + "Total Subsumptions=" + TotalPartialReduct.ToString());
            SetText(Environment.NewLine + "Total Not Unique =" + TotalNotUnique.ToString());
            
            
            

            
            StreamWriter asdf2 = new StreamWriter(AppPath + "\\Refined.XML");
            SetText(Environment.NewLine + "Max Subset Length found=" + MaxSubsetLength.ToString());
            asdf2 = new StreamWriter(AppPath + "\\outputscreen" + rand + ".txt");
            asdf2.Write(txtres.Text);
            asdf2.Close();

        }

        Random rn = new Random();
        private ArrayList GetCurrentUniqueSetsEqClassesFromCurrentUnqueSetsEqClassesFile()
        {
            ArrayList c = new ArrayList();
            string line;
            using (StreamReader reader = new StreamReader(@"C:\DataSets\" + "\\CurrentUbniqueSetsEqClasses.txt"))
            {

                while ((line = reader.ReadLine()) != null)
                {
                    c.Add(line);
                }
                reader.Close();
                reader.Dispose();
            }
            return c;
        }
        private ArrayList GetCurrentUniqueSetsFromCurrentUnqueSetsFile()
        {
            string line;
            ArrayList c = new ArrayList();
            using (StreamReader reader = new StreamReader(@"C:\DataSets\" + "\\CurrentUbniqueSets.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    c.Add(line);
                }
                reader.Close();
                reader.Dispose();
            }
            return c;
        }
        private int GetCurrentRowNumberFromRowNumberFile(string Path)
        {
            StreamReader reader = new StreamReader(Path);
            string input = reader.ReadToEnd();
            int c = Int32.Parse(input);
            reader.Close();
            reader.Dispose();
            return c;
        }
        private void UpdateCurrentRowFile(string AppPath)
        {
            if (File.Exists(AppPath + "\\StartObject.txt"))
            {
                File.Delete(AppPath + "\\StartObject.txt");
            }
            using (StreamWriter writer = new StreamWriter(AppPath + "\\StartObject.txt", true))
            {
                {
                    writer.Write(RowNr.ToString());
                }
                writer.Close();
                writer.Dispose();
            }
        }
        public void PowerSet()
        {

            for (int k = 0; k < 1; k++)
            {
                ArrayList temp = new ArrayList();
                PublicUniq.Add(temp);
            }
            
            List<FeatureValues> lfv = new List<FeatureValues>();
            PublicNotUniq.Add(lfv);
            CurrentLevel = 1;
            ArrayList AllFeaturesFirstLevel = (ArrayList)SkipperListComplement.Clone();
            foreach (int f in AllFeaturesFirstLevel)
            {
                InitSetlist.Clear();
                InitSetlist.Add(f);
                IsUnique(InitSetlist, 0);
            }
            CurrentLevel++;
            NotUniqueSetsPrevLevel.Clear();
            for (int iThread = 0; iThread < PublicNotUniq.Count; iThread++)
            {
                for (int j = 0; j < PublicNotUniq[iThread].Count; j++)
                {
                    NotUniqueSetsPrevLevel.Add(PublicNotUniq[iThread][j]);
                }
            }
            while (WorkMood())//SkipperListComplement.Count && NotUniqueSetsPrevLevel.Count > 0  )
            {

                CheckStopLevel();
                CurrentLevel++;

                for (int thread = 0; thread < PublicUniq.Count; thread++)
                {
                    ArrayList Threadlst = PublicUniq[thread];
                    foreach (FeatureValues fv in Threadlst)
                    {
                        // adding unique feature-value combinations to onle store
                        LstOfUniqueSets.Add(fv);
                        // after adding the unique feature-value combination it takes incremental index to referernce it any where
                        int index = LstOfUniqueSets.Count - 1;
                       // LstOfUniqueSetsNrOfObjects.Add(fv.RowsIndices.Count);
                        foreach (int r in fv.RowsIndices)
                        {
                            LstOfRowsFeaturesSetsIDs[r][(int)fv.UnkSet[0]].Add(index);
                        }
                    }
                }
                PublicUniq.Clear();
                 
                 for (int iThread=0;iThread<PublicNotUniq.Count;iThread++ )
                 {
                     for (int j = 0; j < PublicNotUniq[iThread].Count; j++)
                     {
                         NotUniqueSetsPrevLevel.Add(PublicNotUniq[iThread][j]);
                     }
                 }
            }

        }

        private bool WorkMood()
        {
             
                return ((CurrentLevel <= SkipperListComplement.Count && NotUniqueSetsPrevLevel.Count > 0));
            

        }
        ArrayList InitiatorFeatures = new ArrayList();
        public int  IsUnique(ArrayList CurrentSetPositions, int threadID)
        {
            if (HasSubsumtions_Traditional(CurrentSetPositions))
            {
                TotalPartialReduct++;
                return -1;
            }
            
            ArrayList PassedSetRowIndices = new ArrayList();
            ArrayList allListInterSection = new ArrayList();
            if (CurrentLevel > 1)
            {

                foreach (int f in CurrentSetPositions)
                {
                    foreach (FeatureValues fv in DsMeta[f])
                    {
                        if (fv.value.ToString() == dr[f].ToString())
                        {
                            PassedSetRowIndices.Add((ArrayList)fv.RowsIndices.Clone());
                        }
                    }
                }

                ArrayList AlstInterSection1 = fnGetIntersection1(PassedSetRowIndices);
                ArrayList MaxSec = (ArrayList)PassedSetRowIndices[PassedSetRowIndices.Count - 1];

                int IsNonSenseCount = 0;
                foreach (int g in AlstInterSection1)
                {
                    if (!MaxSec.Contains(g))
                    {
                        break;
                    }
                    else
                    {
                        IsNonSenseCount++;
                    }

                }
                if (IsNonSenseCount == AlstInterSection1.Count)
                {
                    NrOfNonesenseSets++;
                    TotalNoneSense++;

                    return -1;
                }
                allListInterSection.AddRange(AlstInterSection1);
                foreach (int u in AlstInterSection1)
                {
                    if (!MaxSec.Contains(u))
                    {
                        allListInterSection.Remove(u);
                    }
                }

            }
            else
            {
                foreach (FeatureValues fv in DsMeta[(int)CurrentSetPositions[0]])
                {
                    if (fv.value.ToString() == dr[(int)CurrentSetPositions[0]].ToString())
                    {
                        allListInterSection.AddRange(fv.RowsIndices);
                        break;
                    }
                }
            }
            int c = 0;

            ArrayList NrOfCategories = new ArrayList();

            foreach (int k in allListInterSection)
            {
                DataRow d = dt.Rows[k];
                if (!NrOfCategories.Contains(Int32.Parse(d[CategoryColumnName].ToString())))
                {
                    NrOfCategories.Add(Int32.Parse(d[CategoryColumnName].ToString()));
                    c++;
                }
            }
            FeatureValues FVcurrSet = new FeatureValues();
            FVcurrSet.UnkSet = (ArrayList)CurrentSetPositions.Clone();
            FVcurrSet.RowsIndices = (ArrayList)allListInterSection.Clone();
            // trusted End
            if (c == 1)
            { // is unique set
                 
                PublicUniq[threadID].Add(FVcurrSet);
                TotalUniqueSets++;
                if (CurrentLevel == 1)
                {
                    SkipperListComplement.Remove(CurrentSetPositions[0]);
                    if (SkipperListComplement.Count > 0)
                    {
                        LastFeature = (int)SkipperListComplement[SkipperListComplement.Count - 1];
                    }
                }

                return 1;
            }
            else
            { // is not unique set
                if (!CurrentSetPositions.Contains(LastFeature))
                {
                    PublicNotUniq[threadID].Add(FVcurrSet);
                    TotalNotUnique++;
                    NrOfNotUniqueThatWillBeChecked++;
                }
            }
            return 0;
        }
        public void   IsUnique(ArrayList CurrentSetPositions, int threadID,ArrayList PrevSetRowsLst)
        {

            // Nonesense Check
            ArrayList PassedSetRowIndices = new ArrayList();
            ArrayList allListInterSection = new ArrayList();
           
                 int f =(int)CurrentSetPositions[CurrentSetPositions.Count-1];
                    foreach (FeatureValues fv in DsMeta[f])
                    {
                        if (fv.value.ToString() == dr[f].ToString())
                        {
                            PassedSetRowIndices.Add((ArrayList)fv.RowsIndices.Clone());
                            break;
                        }
                    }
                    
                    ArrayList AlstInterSection1 =  PrevSetRowsLst;
                    ArrayList MaxSec = (ArrayList)PassedSetRowIndices[0];

                int IsNonSenseCount = 0;
                foreach (int g in AlstInterSection1)
                {
                    if (!MaxSec.Contains(g))
                    {
                        break;
                    }
                    else
                    {
                        IsNonSenseCount++;
                    }

                }
                if (IsNonSenseCount == AlstInterSection1.Count)
                {
                    NrOfNonesenseSets++;
                    TotalNoneSense++;

                    return ;
                }

            // subsumption check
                if (ChkSkipSubsumptionCheck.Checked == false)
                {
                    if (HasSubsumtions_Traditional(CurrentSetPositions))
                    {
                        TotalPartialReduct++;
                        return;
                    }
                }
                allListInterSection.AddRange(AlstInterSection1);
                foreach (int u in AlstInterSection1)
                {
                    if (!MaxSec.Contains(u))
                    {
                        allListInterSection.Remove(u);
                    }
                }

            
            
            int c = 0;

            ArrayList NrOfCategories = new ArrayList();
            // check uniqueness
            foreach (int k in allListInterSection)
            {
                DataRow d = dt.Rows[k];
                if (!NrOfCategories.Contains(Int32.Parse(d[CategoryColumnName].ToString())))
                {
                    // can be replaced with list of integers represents categories counters
                    NrOfCategories.Add(Int32.Parse(d[CategoryColumnName].ToString()));
                    c++;
                }
            }
            FeatureValues FVcurrSet = new FeatureValues();
            FVcurrSet.UnkSet = (ArrayList)CurrentSetPositions.Clone();
            FVcurrSet.RowsIndices = (ArrayList)allListInterSection.Clone();
            // trusted End
            if (c == 1)
            { // is unique set
                TotalUniqueSets++;
                
                PublicUniq[threadID].Add(FVcurrSet);
                return ;
            }
            else
            { // is not unique set
                if (!CurrentSetPositions.Contains(LastFeature))
                {
                    PublicNotUniq[threadID].Add(FVcurrSet);
                    TotalNotUnique++;
                    NrOfNotUniqueThatWillBeChecked++;
                }
            }
            return ;
        }
        string CategoryColumnName = "";
        private ArrayList fnGetIntersection1(ArrayList PassedSetRowIndices)
        {
            ArrayList ALst = (ArrayList)PassedSetRowIndices[0];
            if (PassedSetRowIndices.Count < 2)
            {
                return ALst;
            }
            else
            {
                ArrayList tmp2 = (ArrayList)ALst.Clone();
                for (int i = 1; i < PassedSetRowIndices.Count - 1; i++)
                {
                    foreach (int y in tmp2)
                    {
                        if (!((ArrayList)PassedSetRowIndices[i]).Contains(y))
                        {
                            ALst.Remove(y);
                        }
                    }
                }
            }
            return ALst;
        }
        public bool IsUnique(ArrayList CurrentSetPositionsInDataRow)
        {

            if (CurrentSetPositionsInDataRow.Count > 0)
            {
                string[] NrOfCategories = new string[dt.Rows.Count];
                int RowIndex = 0;
                foreach (DataRow d in dt.Rows)
                {
                    foreach (int f in CurrentSetPositionsInDataRow)
                    {
                        if (d[f].ToString() == dr[f].ToString())
                        {
                            NrOfCategories[RowIndex] = (d[CategoryColumnName].ToString());
                        }
                        else
                        {
                            NrOfCategories[RowIndex] = "";
                            break;
                        }
                    }
                    RowIndex++;
                }
                ArrayList curCat = new ArrayList();
                foreach (string str in NrOfCategories)
                {
                    if (str != "")
                    {
                        if (!curCat.Contains(dt.Rows[Int32.Parse(str)][CategoryColumnName]))
                        {
                            curCat.Add(dt.Rows[Int32.Parse(str)][CategoryColumnName]);
                            if (curCat.Count > 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;


        }
        //XXXXXX
        int LastFeature = 0;
        private bool HasSubsumtions_Traditional(ArrayList pSubSet)
        {
            foreach (int j in pSubSet)
            {
                ArrayList lstUniqueSets = LstOfRowsFeaturesSetsIDs[RowNr][j];
                foreach (int id in lstUniqueSets)
                {
                    FeatureValues set = LstOfUniqueSets[id];
                    ArrayList tmplst = new ArrayList();
                    if (set.UnkSet == null)
                    {
                        return false;
                    }
                    foreach (int x in set.UnkSet)
                    {
                        if (!pSubSet.Contains(x))
                        {
                            break;
                        }
                        else
                        {
                            tmplst.Add(x);
                        }
                    }
                    if (tmplst.Count == set.UnkSet.Count)
                    {
                        return true;
                    }


                }
            }
            return false;
        }
        private int MatchedRowIndex_Traditional(ArrayList pSubSet)
        {
            for (int rowIndex = 0; rowIndex < SkipChildsOfThusParents.Count; rowIndex++)
            {
                List<ArrayList> TrainingDataUniq = SkipChildsOfThusParents[rowIndex];

                for (int j = 0; j < TrainingDataUniq.Count; j++)
                {
                    if (TrainingDataUniq[j].Count > 0)
                    {
                        foreach (ArrayList uSet in TrainingDataUniq[j])
                        {
                            if (uSet != null)
                            {

                                if (pSubSet.Count < uSet.Count)
                                {
                                    break;
                                }

                                ArrayList tmplst = new ArrayList();
                                if (uSet != null)
                                {
                                    break;
                                }
                                foreach (int x in uSet)
                                {
                                    if (!pSubSet.Contains(x))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        tmplst.Add(x);
                                    }
                                }
                                if (tmplst.Count == uSet.Count)
                                {
                                    return rowIndex;
                                }

                            }
                        }
                    }
                }
            }
            return -1;
        }
        ArrayList GeneralList = new ArrayList();
        ArrayList GeneralListClass = new ArrayList();
        private void HasSubsumtions_Traditional(ArrayList OuterSet, ArrayList SkipChildsOfThusParents, int OuterIndex)
        {
            ArrayList RepeatedSetsIndex = new ArrayList();
            int NMax = OuterIndex;
            for (int innerIndex = OuterIndex + 1; innerIndex < SkipChildsOfThusParents.Count; innerIndex++)
            {


                ArrayList minLst = new ArrayList();
                ArrayList MaxLst = new ArrayList();
                ArrayList InnerSet = new ArrayList();
                InnerSet = (ArrayList)SkipChildsOfThusParents[innerIndex];
                if (InnerSet.Count >= OuterSet.Count)
                {
                    minLst = OuterSet;
                    MaxLst = InnerSet;
                    NMax = innerIndex;
                }
                else
                {
                    NMax = OuterIndex;
                    MaxLst = OuterSet;
                    minLst = InnerSet;
                }
                ArrayList tmplst = new ArrayList();
                foreach (int x in minLst)
                {
                    if (!MaxLst.Contains(x))
                    {
                        break;
                    }
                    else
                    {
                        tmplst.Add(x);
                    }
                }
                if (tmplst.Count == minLst.Count)
                {

                    GeneralListClass[NMax] = null;
                    GeneralList[NMax] = null;
                    // genindex++;
                }

                //  curIndex++;
            }
            return;
        }
        private void FillSkipperListWithEmptyFeatures(DataRow drow)
        {
            SkipperListComplement.Clear();
            for (int x = 0; x < dt.Columns.Count - 1; x++)
            {
                if (!(String.IsNullOrWhiteSpace(drow[x].ToString()) && String.IsNullOrEmpty(drow[x].ToString())) && drow[x].ToString() != "-1")
                {
                    SkipperListComplement.Add(x);
                }
            }
            reOrderFeaturesList(SkipperListComplement);

            if (SkipperListComplement.Count > 0)
            {
                LastFeature = (int)SkipperListComplement[SkipperListComplement.Count - 1];
            }
        }
        private void reOrderFeaturesList(ArrayList passedList)
        {
            ArrayList UnOrderedFeaturesSet = (ArrayList)passedList.Clone();
            for (int y = 0; y < UnOrderedFeaturesSet.Count - 1; y++)
            {
                for (int x = 0; x < UnOrderedFeaturesSet.Count - 1; x++)
                {
                    int f1 = 0;
                    int f2 = 0;
                    foreach (FeatureValues k in DsMeta[(int)UnOrderedFeaturesSet[x]])
                    {
                        if (k.value == Int32.Parse(dr[(int)UnOrderedFeaturesSet[x]].ToString()))
                        {
                            f1 = k.count;
                            break;
                        }
                    }

                    foreach (FeatureValues k in DsMeta[(int)UnOrderedFeaturesSet[x + 1]])
                    {
                        if (k.value == Int32.Parse(dr[(int)UnOrderedFeaturesSet[x + 1]].ToString()))
                        {
                            f2 = k.count;
                            break;
                        }
                    }
                    if (f1 > f2)
                    {
                        int tmp = (int)UnOrderedFeaturesSet[x];
                        UnOrderedFeaturesSet[x] = UnOrderedFeaturesSet[x + 1];
                        UnOrderedFeaturesSet[x + 1] = tmp;
                    }
                }
            }
            SkipperListComplement = UnOrderedFeaturesSet;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int NrRows = 0;
            string contents = "";
            string line;
            bool newRow = false;
            using (StreamReader reader = new StreamReader(@"C:\DataSets\" + "\\Wheat_VM3.xml"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        string cx = line;
                        if (line.IndexOf("Category=") != -1)
                        {
                            category = line.Substring(line.IndexOf("Category="), line.IndexOf(">") - line.IndexOf("Disorder"));
                            newRow = true;
                            NrRows++;
                        }
                        else
                        {
                            if (newRow == true)
                            {

                                contents += "<Rule>";
                                contents += line.Replace("/>", " " + category + "  nrRow=\"" + NrRows.ToString() + "\"  />");
                                newRow = false;

                            }
                            else
                            {
                                contents += line.Replace("/>", " " + category + "  nrRow=\"" + NrRows.ToString() + "\"  />");

                            }
                        }
                    }
                }
            }
            StreamWriter asdf = new StreamWriter(@"C:\DataSets\" + "\\Wheat_VM4.xml");
            asdf.Write(contents);
            asdf.Close();
            MessageBox.Show("done");
        }
        private void LoadDataSetFromXML(string Path)
        {
            string line;
            bool IsFirst = true;
            using (StreamReader reader = new StreamReader(Path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {

                        if (line.IndexOf("Category=") != -1)
                        {
                            string s = line;//.Remove(line.IndexOf("Name="), line.IndexOf("Disorder") - line.IndexOf("Name="));
                            s = s.Replace("'", "");
                            if (!Ds.Contains(s))
                            {
                                Ds.Add(s);
                            }
                        }
                        else
                        {
                            if (!line.Contains("suspected") && line.Contains("Tuple"))
                            {
                                line = line.Replace("'", "");
                                if (line.Contains("Tuple") && !FS.Contains(line))
                                {
                                    FS.Add(line);
                                }
                            }
                        }
                    }

                }
            }



            string[] CurrObject = new string[FS.Count + 1];
            GetTableHeaders();
            dt.Rows.Clear();
            using (StreamReader reader = new StreamReader(Path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {

                        if (line.IndexOf("Category=") != -1)
                        {
                            string s = line;//.Remove(line.IndexOf("Name="), line.IndexOf("Disorder"));
                            s = s.Replace("'", "");
                            if (!IsFirst)
                            {
                                for (int item = 0; item < CurrObject.Length; item++)
                                {
                                    if (CurrObject[item] == null)
                                    {
                                        CurrObject[item] = "-1";
                                    }
                                }
                                dt.Rows.Add(CurrObject);
                                CurrObject = new string[FS.Count + 1];
                                CurrObject[FS.Count] = Ds.IndexOf(s).ToString();
                            }
                            else
                            {

                                CurrObject[FS.Count] = Ds.IndexOf(s).ToString();
                            }
                        }
                        else
                        {
                            if (!line.Contains("suspected") && line.Contains("Tuple"))
                            {
                                line = line.Replace("'", "");
                                int cIndex = FS.IndexOf(line);
                                CurrObject[cIndex] = cIndex.ToString();
                                IsFirst = false;
                            }

                        }

                    }

                }
            }
            CategoryColumnName = dt.Columns[dt.Columns.Count - 1].ColumnName;
            for (int b = 0; b < dt.Columns.Count - 1; b++)
            {
                ArrayList tmpa = new ArrayList();
                tmpa.Add(FS[b].ToString());
                Fs.Add((ArrayList)tmpa.Clone());
            }
            LstOfRowsFeaturesSetsIDs = new List<List<ArrayList>>();
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                List<ArrayList> tmp1 = new List<ArrayList>();
                for (int c = 0; c < dt.Columns.Count - 1; c++)
                {
                    ArrayList tmp2 = new ArrayList();
                    tmp1.Add(tmp2);
                    string CurVal = "";
                    if (dt.Rows[r][c].ToString() != "-1")
                    {
                        CurVal = FS[Int32.Parse(dt.Rows[r][c].ToString())].ToString();
                        if (!FS.Contains(CurVal))
                        {

                            FS.Add(CurVal);
                            dt.Rows[r][c] = FS.IndexOf(CurVal);
                            // ValuesCounter.Add(1);
                        }
                        else
                        {
                            int h = FS.IndexOf(CurVal);
                            dt.Rows[r][c] = h;
                            // ValuesCounter[h] =(int) ValuesCounter[h] + 1;
                        }
                    }

                }
                LstOfRowsFeaturesSetsIDs.Add(tmp1);
                string CurCat = "";
                CurCat = Ds[Int32.Parse(dt.Rows[r][CategoryColumnName].ToString())].ToString();
                if (!Ds.Contains(CurCat))
                {
                    Ds.Add(CurCat);
                    dt.Rows[r][CategoryColumnName] = Ds.IndexOf(CurCat);
                }
                else
                {
                    dt.Rows[r][CategoryColumnName] = Ds.IndexOf(CurCat);
                }
            }
            CategoriesMeta = new List<ArrayList>(Ds.Count);

            for (int cat = 0; cat < Ds.Count; cat++)
            {
                ArrayList tmp = new ArrayList();
                CategoriesMeta.Add(tmp);
            }
            for (int cat = 0; cat < Ds.Count; cat++)
            {


                for (int c = 0; c < dt.Rows.Count; c++)
                {

                    if (dt.Rows[c][CategoryColumnName].ToString() == cat.ToString())
                    {
                        CategoriesMeta[cat].Add(c);
                    }

                }

            }

            for (int c = 0; c < dt.Columns.Count - 1; c++)
            {

                ArrayList DistinctFeatureValues = new ArrayList();
                for (int p = 0; p < dt.Rows.Count; p++)
                {
                    if (!DistinctFeatureValues.Contains(Int32.Parse(dt.Rows[p][c].ToString())) && dt.Rows[p][c].ToString() != "-1")
                    {
                        DistinctFeatureValues.Add(Int32.Parse(dt.Rows[p][c].ToString()));
                    }
                }
                List<FeatureValues> TmpFv = new List<FeatureValues>();
                foreach (int v in DistinctFeatureValues)
                {
                    FeatureValues fv = new FeatureValues();
                    fv.value = v;
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {

                        if (fv.value == Int32.Parse(dt.Rows[r][c].ToString()))
                        {
                            fv.count++;
                            fv.RowsIndices.Add(r);
                        }
                    }
                    TmpFv.Add(fv);
                }
                DsMeta.Add(TmpFv);
            }


        }
        string[] Featues;
        private void LoadDataObjects()
        {
            Featues = new string[dt.Columns.Count - 1];
            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                Featues[i] = dt.Columns[i].ColumnName;
                MainList.Add(i);
            }

            SkipperListComplement.AddRange(MainList);
            NrFeatures = Featues.Length - 1;
        }
        public void GetTableHeaders()
        {
            DataColumn dc = new DataColumn();
            int colsNr = 0;
            if (txtFilePath.Text.ToLower().IndexOf("xml") != -1)
            {
                colsNr = FS.Count;
            }
            else
            {
                colsNr = Fs.Count;
            }
            for (int i = 0; i < colsNr; i++)
            {
                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = i.ToString();
                dt.Columns.Add(dc);
            }

            dc = new DataColumn();
            dc.ColumnName = "Category";
            dc.DataType = Type.GetType("System.String");
            dt.Columns.Add(dc);
        }
        public PNU ()
        {
            InitializeComponent();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            string disorder = "";
            string line;
            bool AtTheSameRule = true;
            ArrayList MyList = new ArrayList();
            using (StreamReader reader = new StreamReader(AppPath + "\\Refined.XML"))
            {
                int Id = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        Id++;

                        string StrNode = line.ToString();
                        if (StrNode.IndexOf("Category=\"") != -1)
                        {
                            disorder = StrNode;
                        }
                        if (StrNode.IndexOf("<Tuple") != -1)
                        {
                            AtTheSameRule = true;
                        }
                        if (StrNode.IndexOf("</Rule>") != -1)
                        {
                            AtTheSameRule = false;
                            MyList.Clear();
                        }

                        if (AtTheSameRule)
                        {
                            if (!MyList.Contains(StrNode) && !String.IsNullOrWhiteSpace(StrNode) && !String.IsNullOrEmpty(StrNode) && StrNode.IndexOf("root") == -1)
                            {
                                MyList.Add(StrNode);
                            }
                            else
                            {
                                if (!String.IsNullOrWhiteSpace(StrNode) && !String.IsNullOrEmpty(StrNode))
                                {
                                    MessageBox.Show("Repeatation Found  " + Id.ToString() + "  disorder:" + disorder);
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("done");
            }
        }
        ArrayList UniqueSets_A = new ArrayList();
        ArrayList UniqueSetsEqClass_A = new ArrayList();
        ArrayList UniqueSets_B = new ArrayList();
        ArrayList UniqueSetsEqClass_B = new ArrayList();
        ArrayList File_A_SetsNrByLevel = new ArrayList();
        ArrayList File_B_SetsNrByLevel = new ArrayList();
        private bool HasUniqueSubSet(ArrayList A_Set, ArrayList UniqueSets_B)
        {
            foreach (ArrayList Bs in UniqueSets_B)
            {
                ArrayList tt = new ArrayList();
                if (Bs.Count < A_Set.Count)
                {
                    foreach (int i in Bs)
                    {
                        if (A_Set.Contains(i))
                        {
                            tt.Add(i);
                        }
                    }
                    if (tt.Count == Bs.Count)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        private bool IsSubsumtion(ArrayList LowerLengthList, ArrayList HigherLengthList)
        {

            ArrayList tt = new ArrayList();
            foreach (int i in LowerLengthList)
            {
                if (!HigherLengthList.Contains(i))
                {
                    return false;
                }
            }


            return true;
        }
        private bool HasSubsumtionsOrEqual(ArrayList pSubSet, ArrayList ListofLists, bool IgnoreSubsumtions)
        {
            if (IgnoreSubsumtions)
            {
                int i = 0;
                foreach (ArrayList UniqueSets in ListofLists)
                {
                    i++;
                    ArrayList minLst = new ArrayList();
                    ArrayList MaxLst = new ArrayList();
                    if (UniqueSets.Count >= pSubSet.Count)
                    {
                        minLst = pSubSet;
                        MaxLst = UniqueSets;
                    }
                    else
                    {
                        MaxLst = pSubSet;
                        minLst = UniqueSets;
                    }
                    ArrayList tmplst = new ArrayList();
                    foreach (int x in minLst)
                    {
                        if (!MaxLst.Contains(x))
                        {
                            break;
                        }
                        else
                        {
                            tmplst.Add(x);
                        }
                    }
                    if (tmplst.Count == minLst.Count)
                    {
                        return true;
                    }
                }
            }
            else
            {
                int i = 0;
                foreach (ArrayList UniqueSets in ListofLists)
                {
                    i++;
                    ArrayList minLst = new ArrayList();
                    ArrayList MaxLst = new ArrayList();
                    if (UniqueSets.Count == pSubSet.Count)
                    {
                        minLst = pSubSet;
                        MaxLst = UniqueSets;
                    }
                    else
                    {
                        break;
                    }
                    ArrayList tmplst = new ArrayList();
                    foreach (int x in minLst)
                    {
                        if (!MaxLst.Contains(x))
                        {
                            break;
                        }
                        else
                        {
                            tmplst.Add(x);
                        }
                    }
                    if (tmplst.Count == minLst.Count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 10;
        }
        string path = "";
        string OutputFilePath = "";
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            path = openFileDialog1.FileName;
            txtFilePath.Text = path;
            if (!String.IsNullOrWhiteSpace(path) && !String.IsNullOrEmpty(path))
            {
                OutputFilePath = path.Substring(0, path.LastIndexOf("\\")) + "\\Refined.xml";
                if (!File.Exists(path.Substring(0, path.LastIndexOf("\\")) + "\\Refined.xml"))
                {
                    File.Create(OutputFilePath);
                }
                if (path.ToLower().IndexOf(".csv") > -1)
                {
                    LoadDataFromCSV(path);
                }
                else
                {
                    LoadDataSetFromXML(path);
                }

                LoadDataObjects();
                btnStart.Enabled = true;
            }
            else
            { btnStart.Enabled = false; }
        }
        //ArrayList ValuesCounter =  new ArrayList();
        Hashtable htClassesCounts = new Hashtable();
        private void LoadDataFromCSV(string path)
        {
            DataColumn dc;
            dc = new DataColumn();
            string header;
            StreamReader re = File.OpenText(path);
            string input = null;
            input = re.ReadToEnd();
            header = input;
            string line = "";
            bool isHeader = true;

            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        if (isHeader == true)
                        {
                            isHeader = false;
                            string[] cls = line.Replace(";", ",").Replace("\t", ",").Split(',');
                            CategoryColumnName = cls[cls.Length - 1];
                            for (int i = 0; i < cls.Length; i++)
                            {
                                dc = new DataColumn();
                                dc.ColumnName = cls[i];
                                dc.DataType = Type.GetType("System.String");
                                dt.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            string[] strRow = line.ToLower().Replace(";", ",").Replace("\t", ",").Split(',');
                            string[] strRowt = strRow;
                            for (int i = 0; i < strRowt.Length; i++)
                            {
                                strRow[i] =System.Web.HttpUtility.HtmlEncode(strRowt[i]);
                            }
                            dt.Rows.Add(strRow);
                        }
                    }

                }
            }
            Fs.Clear();
            for (int b = 0; b < dt.Columns.Count - 1; b++)
            {
                ArrayList tmpa = new ArrayList();
                Fs.Add((ArrayList)tmpa.Clone());
            }

            LstOfRowsFeaturesSetsIDs = new List<List<ArrayList>>();
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                List<ArrayList> LstOfFeatures = new List<ArrayList>();
                for (int c = 0; c < dt.Columns.Count - 1; c++)
                {
                    ArrayList tmp = new ArrayList();
                    LstOfFeatures.Add(tmp);
                    string CurVal = "";
                   
                    CurVal = "<Tuple  Cpt=\"Dummy\" Prop=\"" + dt.Columns[c].ColumnName + "\" Val=\"" + dt.Rows[r][c].ToString() + "\" />";
                    if (!Fs[c].Contains(CurVal))
                    {

                        Fs[c].Add(CurVal);
                        dt.Rows[r][c] = Fs[c].IndexOf(CurVal);
                        //  ValuesCounter.Add(1);
                    }
                    else
                    {
                        int h = Fs[c].IndexOf(CurVal);
                        dt.Rows[r][c] = h;
                        //ValuesCounter[h] =(int) ValuesCounter[h] + 1;
                    }
                }
                LstOfRowsFeaturesSetsIDs.Add(LstOfFeatures);
                string CurCat = "";
                CurCat = dt.Rows[r][CategoryColumnName].ToString();
               // CurCat = "<Rule " + CategoryColumnName + "=\"" + dt.Rows[r][CategoryColumnName] + "\" >";
                if (!Ds.Contains(  CurCat))
                {
                    Ds.Add( CurCat);
                    dt.Rows[r][CategoryColumnName] = Ds.IndexOf(CurCat);
                }
                else
                {
                    dt.Rows[r][CategoryColumnName] = Ds.IndexOf(CurCat);
                }
            }
            CategoriesMeta = new List<ArrayList>(Ds.Count);

            for (int cat = 0; cat < Ds.Count; cat++)
            {
                ArrayList tmp = new ArrayList();
                CategoriesMeta.Add(tmp);
            }
            for (int cat = 0; cat < Ds.Count; cat++)
            {


                for (int c = 0; c < dt.Rows.Count; c++)
                {

                    if (dt.Rows[c][CategoryColumnName].ToString() == cat.ToString())
                    {
                        CategoriesMeta[cat].Add(c);
                    }

                }

            }

            for (int c = 0; c < dt.Columns.Count - 1; c++)
            {

                ArrayList DistinctFeatureValues = new ArrayList();
                for (int p = 0; p < dt.Rows.Count; p++)
                {
                    if (!DistinctFeatureValues.Contains(Int32.Parse(dt.Rows[p][c].ToString())))
                    {
                        DistinctFeatureValues.Add(Int32.Parse(dt.Rows[p][c].ToString()));
                    }
                }
                List<FeatureValues> TmpFv = new List<FeatureValues>();
                foreach (int v in DistinctFeatureValues)
                {
                    FeatureValues fv = new FeatureValues();
                    fv.value = v;
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {

                        if (fv.value == Int32.Parse(dt.Rows[r][c].ToString()))
                        {
                            fv.count++;
                            fv.RowsIndices.Add(r);
                        }
                    }
                    TmpFv.Add(fv);
                }
                DsMeta.Add(TmpFv);
            }

        }
        List<ArrayList> CategoriesMeta = new List<ArrayList>();
        Hashtable htFeatureIndicesForCurrentObject = new Hashtable();
        List<List<FeatureValues>> DsMeta = new List<List<FeatureValues>>();// FeatureIndex|Value Index|Value rows indices + its existence count
        private void compareToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Compare2XMLfiles1 ss = new Compare2XMLfiles1();
            ss.Activate();
            ss.Show();
            this.Hide();
        }
        
        

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtres.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtres.AppendText(text);
            }
        }
        delegate void SetLblTextCallback(string text);
        private void SetLblText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblnrOfObjects.InvokeRequired)
            {
                SetLblTextCallback d = new SetLblTextCallback(SetLblText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblnrOfObjects.Text=text;
            }
        }
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }

         

        private void LoadDataSetFromXMLTest(string path)
        {
            throw new NotImplementedException();
        }

        private void LoadDataFromCSVTest(string path)
        {
            DataColumn dc;
            dc = new DataColumn();
            string header;
            StreamReader re = File.OpenText(path);
            string input = null;
            input = re.ReadToEnd();
            header = input;
            string line = "";
            bool isHeader = true;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        if (isHeader == true)
                        {
                            isHeader = false;
                            string[] cls = line.Replace(" ", "_").Split(',');
                            for (int i = 0; i < cls.Length; i++)
                            {
                                dc = new DataColumn();
                                dc.ColumnName = cls[i];
                                dc.DataType = Type.GetType("System.String");
                                dtTest.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            string[] strRow = line.Split(',');
                            dtTest.Rows.Add(strRow);
                        }
                    }
                }
            }
            for (int b = 0; b < dtTest.Columns.Count - 1; b++)
            {
                ArrayList tmpa = new ArrayList();
                FsTest.Add((ArrayList)tmpa.Clone());
            }
            for (int r = 0; r < dtTest.Rows.Count; r++)
            {
                for (int c = 0; c < dtTest.Columns.Count - 1; c++)
                {
                    string CurVal = "";
                    CurVal = "<Tuple  Cpt=\"Dummy\" Prop=\"" + dtTest.Columns[c].ColumnName + "\" Val=\"" + dtTest.Rows[r][c].ToString() + "\" />";
                    if (!FsTest[c].Contains(CurVal))
                    {
                        FsTest[c].Add(CurVal);
                        dtTest.Rows[r][c] = FsTest[c].IndexOf(CurVal);
                    }
                    else
                    {
                        int h = FsTest[c].IndexOf(CurVal);
                        dtTest.Rows[r][c] = h;
                    }
                }
            }

        }
        //  List<List<ArrayList>> SkipChildsOfThusParents = new List<List<ArrayList>>();
        private void btnPredict_Click(object sender, EventArgs e)
        {
            List<int> Rs = new List<int>(dtTest.Rows.Count);
            SkipperListComplement.Clear();
            for (int x = 0; x < dtTest.Columns.Count - 1; x++)
            {
                SkipperListComplement.Add(x);
            }
            foreach (DataRow drow in dtTest.Rows)
            {
                Rs.Add(Predict(drow));
            }
        }

        private int Predict(DataRow drow)
        {
            int x = MatchedRowIndex_Traditional(SkipperListComplement);
            return -1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s", stopwatch.Elapsed.Days, stopwatch.Elapsed.Hours,
                            stopwatch.Elapsed.Minutes,
                            stopwatch.Elapsed.Seconds);
                
            lblnoneSense.Text = TotalNoneSense.ToString();
            lblPartialreduct.Text = TotalPartialReduct.ToString();
            lbltotalNotuniqueWillBeCheckedOutPut.Text = NrOfNotUniqueThatWillBeChecked.ToString();
            lblTotalNotUniqueOutput.Text = TotalNotUnique.ToString();
            lblTotalUnique.Text = TotalUniqueSets.ToString();
            //

            TimeSpan t = TimeSpan.FromSeconds((stopwatch.Elapsed.TotalSeconds * TotalNumberOfObjects) / (RowNr+1));

            lblRt.Text =   string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s",t.Days,t.Hours,
                            t.Minutes,
                            t.Seconds);

             }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit or no?",
                       "Info.",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else {
                Application.Exit();
            }
        }

        private void multiTextSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiTextSearch ss = new MultiTextSearch();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void classifierToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MyClassifier ss = new MyClassifier();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void sortingTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sort ss = new Sort();
            ss.Activate();
            ss.Show();
            this.Hide();
            
        }

        private void nCrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nCr ss = new nCr();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void dClassifierToolStripMenuItem_Click(object sender, EventArgs e)
        {


            MyDClassifier ss = new MyDClassifier();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void xMLDatasetsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            PNUXML ss = new PNUXML();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void cSV2XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConvertDatasetsWithUnknownValuesToXML ss = new ConvertDatasetsWithUnknownValuesToXML();
            ss.Activate();
            ss.Show();
            this.Hide();
        }

        private void outputAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compare2XMLfiles ss = new Compare2XMLfiles();
            ss.Activate();
            ss.Show();
            this.Hide();
        }


    }

}


