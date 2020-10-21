using System;
using System.Numerics;
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
namespace PWRSet
{
    public partial class nCr : Form
    {
        public int _NU = 0;
        public int _U = 0;
        public int _SS = 0;
        public int _NS = 0;
        public int _T = 0;
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
        ArrayList AllCuttedUniqueSetsPrevLevel = new ArrayList();
        ArrayList tmpNUList = new ArrayList();
        ArrayList tmpAllCuttedSets = new ArrayList();
        ArrayList tmpUList = new ArrayList();
        List<List<FeatureValues>> PublicNotUniq = new List<List<FeatureValues>>();
        List<ArrayList> PublicUniq = new List<ArrayList>();
        // 
        List<ArrayList> LstOfUniqueSets = new List<ArrayList>();
        List<int> LstOfUniqueSetsNrOfObjects = new List<int>();
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



            for (int k = 0; k < tmpNUList.Count; k++)
            {

                List<FeatureValues> temp = new List<FeatureValues>();
                PublicNotUniq.Add(temp);
                ArrayList tt = new ArrayList();
                PublicUniq.Add(tt);

            }
            //XXXXXX

            //Parallel.For(0, tmpNUList.Count, new ParallelOptions { MaxDegreeOfParallelism = 32 }, i =>
            //{
            for (int i = 0; i < tmpNUList.Count; i++)
            {
                ArrayList pl1 = ((FeatureValues)tmpNUList[i]).UnkSet;
                ArrayList pl = (ArrayList)pl1.Clone();
                for (int j = SkipperListComplement.IndexOf(pl[pl.Count - 1]) + 1; j < SkipperListComplement.Count; j++)
                {
                    pl.Add(SkipperListComplement[j]);
                    IsUnique(pl, i, ((FeatureValues)tmpNUList[i]).RowsIndices);
                    pl.RemoveAt(pl.Count - 1);
                }
                tmpNUList[i] = null;
            }//);



            lstDataAnalysis.Add((NrOfRows + 1).ToString() + "," + SkipperListComplement.Count.ToString() + "," + CurrentLevel.ToString() + "," + _NU.ToString() + "," + _NS.ToString() + "," + _U.ToString() + "," + _SS.ToString());
            _NS = 0;
            _NU = 0;
            _SS = 0;
            _T = 0;
            _U = 0;
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
        List<ulong> AllCuttedCombinationsForAllRows = new List<ulong>();
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
            lstDataAnalysis.Add("RowNr,nrFeatures,Level,NU,NS,UN,SS");
            LevelsUN = new int[dt.Columns.Count];

           
            foreach (DataRow drow in dt.Rows)
            {
                NU = 0;
                NS = 0;
                UN = 0;
                SS = 0;
                dr = drow;
                FillSkipperListWithEmptyFeatures(drow);
                string curStrSet = "";
                foreach (int f in SkipperListComplement)
                {
                    curStrSet += "," + f.ToString();
                }

                nrFeatures = SkipperListComplement.Count;
                SetText("row:" + (RowNr + 1).ToString() + "  started at " + DateTime.Now.ToString().Replace("م", "PM ").Replace("ص", "AM ") + "___(" + nrFeatures.ToString() + ")  [" + curStrSet.Substring(1) + "]" + Environment.NewLine);

                NotUniqueSetsPrevLevel.Clear();
                TimeSpan ts = RestartableStopWatch.Elapsed;
                SetLblText((RowNr + 1).ToString() + " of " + TotalNumberOfObjects.ToString());
                PossibleCombinationsNextLevel_Un_NS_SS = 0;
                PossibleCombinationsNextLevel_NU = 0;
                AllCuttedUniqueSetsPrevLevel = new ArrayList();
                PowerSet();
                //AddToDataAnalysisList();
               // break;
                //int x = 0;
                foreach (string xx in lstDataAnalysis)
                {
                    
                        SetText(Environment.NewLine + xx);
                     
                }


                //  SaveNow(AppPath);
                RowNr++;
                break;
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
        int PossibleCombinationsNextLevel_Un_NS_SS = 0;
        int PossibleCombinationsNextLevel_NU = 0;
        ArrayList CombinationsTypesCounters = new ArrayList();
        int nrFeatures = 0;
        long TotalNoneSense = 0;
        long TotalPartialReduct = 0;
        int MaxSubsetLength = 0;
        long TotalNotUnique = 0;
        long NrOfNotUniqueThatWillBeChecked = 0;
        int MaxRecurrsiveRuleCount = 0;
        private void SaveNow(string AppPath)
        {
          

           

            for (int i = 1; i < AllCuttedCombinationsForAllRows.Count; i++)
           {
               SetText(Environment.NewLine + "L" + (i).ToString() + "\t" + AllCuttedCombinationsForAllRows[i].ToString());
           }

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
        ArrayList lstDataAnalysis = new ArrayList();
        int[] LevelsUN;
        ulong CombinatorialsOfBranch(int FeatureOrder, int NrOfFearures, int ReqLevel, int StopLevel, ArrayList s)
        {

            int N_Dash = NrOfFearures - FeatureOrder;//2
            int r = ReqLevel - StopLevel;//0



            ulong res = 0;
            if (N_Dash >= r)
            {
                res =   Combinatorial(N_Dash, r)  ;
            }

            return res;


        }

        private ulong Combinatorial(int N_Dash, int r)
        {

            int x = N_Dash;
            int y = r;
            int z = N_Dash - r;
            BigInteger bigInt1 = 1;
            BigInteger bigInt2 = 1;
            BigInteger bigInt3 = 1;

            while (x > 1)
            {
                bigInt1 = BigInteger.Multiply(x--, bigInt1);
            }

            while (y > 1)
            {
                bigInt3 = BigInteger.Multiply(y--, bigInt3);
            }

            while (z > 1)
            {
                bigInt2 = BigInteger.Multiply(z--, bigInt2);
            }

            return ulong.Parse((bigInt1 / (bigInt2 * bigInt3)).ToString());


        }
        public void PowerSet()
        {

            _NS = 0;
            _NU = 0;
            _SS = 0;
            _T = 0;
            _U = 0;

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

            lstDataAnalysis.Add((NrOfRows + 1).ToString() + "," + SkipperListComplement.Count.ToString()+ ","+CurrentLevel.ToString()+","+_NU.ToString()+","+_NS.ToString()+","+_U.ToString()+","+_SS.ToString());
            PossibleCombinationsNextLevel_Un_NS_SS = 0;
            PossibleCombinationsNextLevel_NU = 0;
            CurrentLevel++;

            _NS = 0;
            _NU = 0;
            _SS = 0;
            _T = 0;
            _U = 0;
            NotUniqueSetsPrevLevel.Clear();
            for (int iThread = 0; iThread < PublicNotUniq.Count; iThread++)
            {
                for (int j = 0; j < PublicNotUniq[iThread].Count; j++)
                {
                    NotUniqueSetsPrevLevel.Add(PublicNotUniq[iThread][j]);
                }
            }
            while (WorkMood())
            {

                CheckStopLevel();
                //if (chkShowCurrentLevel.Checked)
                //{
                //    SetText("All PossibleCombinations of (UN,NS,SS) For Level  L+1 ie.L" + (CurrentLevel + 1).ToString() + "=" + PossibleCombinationsNextLevel_Un_NS_SS.ToString() + Environment.NewLine);
                //    SetText("All PossibleCombinations of (NU) For Level  L+1 ie.L" + (CurrentLevel + 1).ToString() + "=" + PossibleCombinationsNextLevel_NU.ToString() + Environment.NewLine);
                //}
                PossibleCombinationsNextLevel_Un_NS_SS = 0;
                PossibleCombinationsNextLevel_NU = 0;
                CurrentLevel++;

                for (int thread = 0; thread < PublicUniq.Count; thread++)
                {
                    ArrayList Threadlst = PublicUniq[thread];
                    foreach (FeatureValues fv in Threadlst)
                    {
                        LstOfUniqueSets.Add(fv.UnkSet);
                        int index = LstOfUniqueSets.Count - 1;
                        LstOfUniqueSetsNrOfObjects.Add(fv.RowsIndices.Count);
                        foreach (int r in fv.RowsIndices)
                        {
                            LstOfRowsFeaturesSetsIDs[r][(int)fv.UnkSet[0]].Add(index);
                        }
                    }
                }
                PublicUniq.Clear();

                for (int iThread = 0; iThread < PublicNotUniq.Count; iThread++)
                {
                    for (int j = 0; j < PublicNotUniq[iThread].Count; j++)
                    {
                        NotUniqueSetsPrevLevel.Add(PublicNotUniq[iThread][j]);
                    }
                }
            }


        }

        bool Go = false;
        private void AddToDataAnalysisList()
        {

            for (int reqLevel = 1; reqLevel <= SkipperListComplement.Count; reqLevel++)
            {

                foreach (Itemset_StopLevel_U_SS_NS xx in AllCuttedUniqueSetsPrevLevel)
                {

                    int FeatureOrder = 0;
                    FeatureOrder = SkipperListComplement.IndexOf(xx.itemset[xx.itemset.Count - 1]) + 1;
                    if ((SkipperListComplement.Count - FeatureOrder + xx.stopLevel) >= reqLevel && reqLevel > xx.stopLevel)
                    {
                        //ulong xBefore = AllCuttedCombinationsForAllRows[reqLevel];
                        //ulong xAfter = 0;
                        AllCuttedCombinationsForAllRows[reqLevel] += CombinatorialsOfBranch(FeatureOrder, SkipperListComplement.Count, reqLevel, xx.stopLevel, xx.itemset);
                        //xAfter = AllCuttedCombinationsForAllRows[reqLevel];
                        //if (xAfter < xBefore)
                        //{
                        //    SetText(Environment.NewLine + "RequiredLevel" + "\t" + reqLevel.ToString() + "\t" + "\t" + "CurrentLevel" + "\t" + CurrentLevel.ToString() + "\t" + "RowNr" + "\t" + RowNr.ToString() + Environment.NewLine);
                        //}
                    }
                    else
                    {

                    }
                }


            }

            //for (int b = 0; b < AllCuttedCombinationsForAllRows.Count; b++)
            //{
            //    if (AllCuttedCombinationsForAllRows[b] < 0)
            //    {
            //        SetText( Environment.NewLine+ b.ToString() + "\t" + "L" + CurrentLevel.ToString() + "\t" + "RowNr" + RowNr.ToString()+Environment.NewLine);
            //    }
            //}
        }
        private bool WorkMood()
        {

            return ((CurrentLevel < SkipperListComplement.Count));//&& NotUniqueSetsPrevLevel.Count > 0));


        }
        ArrayList InitiatorFeatures = new ArrayList();
        public int IsUnique(ArrayList CurrentSetPositions, int threadID)
        {
            if (HasSubsumtions_Traditional(CurrentSetPositions))
            {
                TotalPartialReduct++;
                SS++;
                _SS++;
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
                
                UN++;
                _U++;
                PublicUniq[threadID].Add(FVcurrSet);
                TotalUniqueSets++;

                Itemset_StopLevel_U_SS_NS ob = new Itemset_StopLevel_U_SS_NS();
                ob.itemset = (ArrayList)CurrentSetPositions.Clone();
                ob.stopLevel = CurrentLevel;
                ob.SkippedSetType = "u";
                AllCuttedUniqueSetsPrevLevel.Add(ob);
                return 1;
            }
            else
            { // is not unique set
                NU++;
                _NU++;
                TotalNotUnique++;
                if (!CurrentSetPositions.Contains(LastFeature))
                {
                    
                    PublicNotUniq[threadID].Add(FVcurrSet);
                    
                    NrOfNotUniqueThatWillBeChecked++;
                    
                }
                 

            }
            return 0;
        }
        public void IsUnique(ArrayList CurrentSetPositions, int threadID, ArrayList PrevSetRowsLst)
        {

            // Nonesense Check
            ArrayList PassedSetRowIndices = new ArrayList();
            ArrayList allListInterSection = new ArrayList();

            int f = (int)CurrentSetPositions[CurrentSetPositions.Count - 1];
            foreach (FeatureValues fv in DsMeta[f])
            {
                if (fv.value.ToString() == dr[f].ToString())
                {
                    PassedSetRowIndices.Add((ArrayList)fv.RowsIndices.Clone());
                    break;
                }
            }

            ArrayList AlstInterSection1 = PrevSetRowsLst;
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
                NS++;
                _NS++;
                Itemset_StopLevel_U_SS_NS ob = new Itemset_StopLevel_U_SS_NS();
                ob.itemset = (ArrayList)CurrentSetPositions.Clone();
                ob.stopLevel = CurrentLevel;
                ob.SkippedSetType = "ns";
                AllCuttedUniqueSetsPrevLevel.Add(ob);

                return;
            }

            // subsumption check

            if (HasSubsumtions_Traditional(CurrentSetPositions))
            {
                TotalPartialReduct++;
                SS++;
                _SS++;
                Itemset_StopLevel_U_SS_NS ob = new Itemset_StopLevel_U_SS_NS();
                ob.itemset = (ArrayList)CurrentSetPositions.Clone();
                ob.stopLevel = CurrentLevel;
                ob.SkippedSetType = "ss";
                AllCuttedUniqueSetsPrevLevel.Add(ob);
                return;
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
                UN++;
                _U++;
                TotalUniqueSets++;
                PublicUniq[threadID].Add(FVcurrSet);
                
                Itemset_StopLevel_U_SS_NS ob = new Itemset_StopLevel_U_SS_NS();
                ob.itemset = (ArrayList)CurrentSetPositions.Clone();
                ob.stopLevel = CurrentLevel;
                ob.SkippedSetType = "u";
                AllCuttedUniqueSetsPrevLevel.Add(ob);
                return;
            }
            else
            { // is not unique set
   NU++;
   _NU++;
   TotalNotUnique++;
                if (!CurrentSetPositions.Contains(LastFeature))
                {
                 
                    PublicNotUniq[threadID].Add(FVcurrSet);
                    
                    NrOfNotUniqueThatWillBeChecked++;
                    
                }
                
            }
            
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
                    ArrayList set = LstOfUniqueSets[id];
                    ArrayList tmplst = new ArrayList();
                    if (set == null)
                    {
                        return false;
                    }
                    foreach (int x in set)
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
                    if (tmplst.Count == set.Count)
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
        public nCr()
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
                            string[] cls = line.Replace(" ", "_").Replace(";", ",").Replace("\t", ",").Split(',');
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
                    CurVal = "<Tuple " + dt.Columns[c].ColumnName + "=\"" + dt.Rows[r][c].ToString() + "\" />";
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
                //CurCat = Ds[Int32.Parse(dt.Rows[r][CategoryColumnName].ToString())].ToString();
                CurCat = "<Rule " + CategoryColumnName + "=\"" + dt.Rows[r][CategoryColumnName] + "\" >";
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
            Compare2XMLfiles ss = new Compare2XMLfiles();
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
                this.lblnrOfObjects.Text = text;
            }
        }
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }

        int NU = 0;
        int NS = 0;
        int UN = 0;
        int SS = 0;

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
                    CurVal = "<Tuple " + dtTest.Columns[c].ColumnName + "='" + dtTest.Rows[r][c].ToString() + "' />";
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

            TimeSpan t = TimeSpan.FromSeconds((stopwatch.Elapsed.TotalSeconds * TotalNumberOfObjects) / (RowNr + 1));

            lblRt.Text = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s", t.Days, t.Hours,
                            t.Minutes,
                            t.Seconds);

        }

        private void nCr_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit or no?",
                       "Info.",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                PNU p = new PNU();
                p.Activate();
                p.Show();
                this.Dispose();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (Go)
            {
                AddToDataAnalysisList();
            }
        }

        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtres = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesOperationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.multiTextSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkShowCurrentLevel = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblnoneSense = new System.Windows.Forms.Label();
            this.lblPartialreduct = new System.Windows.Forms.Label();
            this.lblTotalNotUnique = new System.Windows.Forms.Label();
            this.lblTotalNotUniqueOutput = new System.Windows.Forms.Label();
            this.lbltotalNotuniqueWillBeCheckedOutPut = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblRemainingTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblnrOfObjects = new System.Windows.Forms.Label();
            this.lblRt = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalUnique = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.Red;
            this.btnStart.Location = new System.Drawing.Point(13, 106);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "pw";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Start_Click);
            // 
            // txtres
            // 
            this.txtres.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtres.Location = new System.Drawing.Point(13, 194);
            this.txtres.Multiline = true;
            this.txtres.Name = "txtres";
            this.txtres.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtres.Size = new System.Drawing.Size(771, 312);
            this.txtres.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(298, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(13, 39);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(278, 20);
            this.txtFilePath.TabIndex = 15;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesOperationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(796, 24);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesOperationsToolStripMenuItem
            // 
            this.filesOperationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareToolStripMenuItem1,
            this.multiTextSearchToolStripMenuItem,
            this.classifierToolStripMenuItem});
            this.filesOperationsToolStripMenuItem.Name = "filesOperationsToolStripMenuItem";
            this.filesOperationsToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.filesOperationsToolStripMenuItem.Text = "Files Operations";
            // 
            // compareToolStripMenuItem1
            // 
            this.compareToolStripMenuItem1.Name = "compareToolStripMenuItem1";
            this.compareToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.compareToolStripMenuItem1.Text = "Compare";
            this.compareToolStripMenuItem1.Click += new System.EventHandler(this.compareToolStripMenuItem1_Click);
            // 
            // multiTextSearchToolStripMenuItem
            // 
            this.multiTextSearchToolStripMenuItem.Name = "multiTextSearchToolStripMenuItem";
            this.multiTextSearchToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.multiTextSearchToolStripMenuItem.Text = "MultiTextSearch";
            this.multiTextSearchToolStripMenuItem.Click += new System.EventHandler(this.multiTextSearchToolStripMenuItem_Click);
            // 
            // classifierToolStripMenuItem
            // 
            this.classifierToolStripMenuItem.Name = "classifierToolStripMenuItem";
            this.classifierToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.classifierToolStripMenuItem.Text = "Classifier";
            this.classifierToolStripMenuItem.Click += new System.EventHandler(this.classifierToolStripMenuItem_Click);
            // 
            // chkShowCurrentLevel
            // 
            this.chkShowCurrentLevel.AutoSize = true;
            this.chkShowCurrentLevel.Location = new System.Drawing.Point(545, 42);
            this.chkShowCurrentLevel.Name = "chkShowCurrentLevel";
            this.chkShowCurrentLevel.Size = new System.Drawing.Size(119, 17);
            this.chkShowCurrentLevel.TabIndex = 22;
            this.chkShowCurrentLevel.Text = "Show Current Level";
            this.chkShowCurrentLevel.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Blue;
            this.lblTime.Location = new System.Drawing.Point(109, 140);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 24);
            this.lblTime.TabIndex = 28;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Elapsed Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(512, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Nonesense:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(501, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "PartialReduct:";
            // 
            // lblnoneSense
            // 
            this.lblnoneSense.AutoSize = true;
            this.lblnoneSense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnoneSense.ForeColor = System.Drawing.Color.Red;
            this.lblnoneSense.Location = new System.Drawing.Point(598, 90);
            this.lblnoneSense.Name = "lblnoneSense";
            this.lblnoneSense.Size = new System.Drawing.Size(0, 20);
            this.lblnoneSense.TabIndex = 32;
            // 
            // lblPartialreduct
            // 
            this.lblPartialreduct.AutoSize = true;
            this.lblPartialreduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartialreduct.ForeColor = System.Drawing.Color.Green;
            this.lblPartialreduct.Location = new System.Drawing.Point(598, 114);
            this.lblPartialreduct.Name = "lblPartialreduct";
            this.lblPartialreduct.Size = new System.Drawing.Size(0, 20);
            this.lblPartialreduct.TabIndex = 33;
            // 
            // lblTotalNotUnique
            // 
            this.lblTotalNotUnique.AutoSize = true;
            this.lblTotalNotUnique.Location = new System.Drawing.Point(485, 161);
            this.lblTotalNotUnique.Name = "lblTotalNotUnique";
            this.lblTotalNotUnique.Size = new System.Drawing.Size(91, 13);
            this.lblTotalNotUnique.TabIndex = 34;
            this.lblTotalNotUnique.Text = "Total Not Unique:";
            // 
            // lblTotalNotUniqueOutput
            // 
            this.lblTotalNotUniqueOutput.AutoSize = true;
            this.lblTotalNotUniqueOutput.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblTotalNotUniqueOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblTotalNotUniqueOutput.Location = new System.Drawing.Point(598, 161);
            this.lblTotalNotUniqueOutput.Name = "lblTotalNotUniqueOutput";
            this.lblTotalNotUniqueOutput.Size = new System.Drawing.Size(0, 17);
            this.lblTotalNotUniqueOutput.TabIndex = 35;
            // 
            // lbltotalNotuniqueWillBeCheckedOutPut
            // 
            this.lbltotalNotuniqueWillBeCheckedOutPut.AutoSize = true;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbltotalNotuniqueWillBeCheckedOutPut.ForeColor = System.Drawing.Color.Purple;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Location = new System.Drawing.Point(598, 139);
            this.lbltotalNotuniqueWillBeCheckedOutPut.Name = "lbltotalNotuniqueWillBeCheckedOutPut";
            this.lbltotalNotuniqueWillBeCheckedOutPut.Size = new System.Drawing.Size(0, 17);
            this.lbltotalNotuniqueWillBeCheckedOutPut.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(411, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Total Not Unique will be checked:";
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.AutoSize = true;
            this.lblRemainingTime.Location = new System.Drawing.Point(2, 163);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(86, 13);
            this.lblRemainingTime.TabIndex = 38;
            this.lblRemainingTime.Text = "Remaining Time:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(482, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Searched Objects:";
            // 
            // lblnrOfObjects
            // 
            this.lblnrOfObjects.AutoSize = true;
            this.lblnrOfObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnrOfObjects.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblnrOfObjects.Location = new System.Drawing.Point(598, 68);
            this.lblnrOfObjects.Name = "lblnrOfObjects";
            this.lblnrOfObjects.Size = new System.Drawing.Size(0, 18);
            this.lblnrOfObjects.TabIndex = 41;
            this.lblnrOfObjects.Tag = "0";
            // 
            // lblRt
            // 
            this.lblRt.AutoSize = true;
            this.lblRt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRt.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblRt.Location = new System.Drawing.Point(110, 166);
            this.lblRt.Name = "lblRt";
            this.lblRt.Size = new System.Drawing.Size(16, 18);
            this.lblRt.TabIndex = 42;
            this.lblRt.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(505, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Total Unique:";
            // 
            // lblTotalUnique
            // 
            this.lblTotalUnique.AutoSize = true;
            this.lblTotalUnique.Location = new System.Drawing.Point(571, 178);
            this.lblTotalUnique.Name = "lblTotalUnique";
            this.lblTotalUnique.Size = new System.Drawing.Size(0, 13);
            this.lblTotalUnique.TabIndex = 44;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 45;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 509);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblTotalUnique);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblRt);
            this.Controls.Add(this.lblnrOfObjects);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblRemainingTime);
            this.Controls.Add(this.lbltotalNotuniqueWillBeCheckedOutPut);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTotalNotUniqueOutput);
            this.Controls.Add(this.lblTotalNotUnique);
            this.Controls.Add(this.lblPartialreduct);
            this.Controls.Add(this.lblnoneSense);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.chkShowCurrentLevel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtres);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "nCr";
            this.Text = "Pursuit_not_Super_Reduct_only_Algorithm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.nCr_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtres;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem filesOperationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem1;
        private System.Windows.Forms.CheckBox chkShowCurrentLevel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblnoneSense;
        private System.Windows.Forms.Label lblPartialreduct;
        private System.Windows.Forms.Label lblTotalNotUnique;
        private System.Windows.Forms.Label lblTotalNotUniqueOutput;
        private System.Windows.Forms.Label lbltotalNotuniqueWillBeCheckedOutPut;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblRemainingTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblnrOfObjects;
        private System.Windows.Forms.Label lblRt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTotalUnique;
        private System.Windows.Forms.ToolStripMenuItem multiTextSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classifierToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
    public class NU_U_SS_NS
    {
        public int NU = 0;
        public int U = 0;
        public int SS = 0;
        public int NS = 0;
        public int T = 0;

    }
    public class Itemset_StopLevel_U_SS_NS
    {
        public ArrayList itemset = new ArrayList();
        public int stopLevel = 0;
        public string SkippedSetType = "";
    }
}


