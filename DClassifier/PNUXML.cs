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
using Pursuit_not_Super_Reduct_only_Algorithm.CS;
using DClassifier.CS;
namespace PWRSet
{
    public partial class PNUXML : Form
    {
        int RowNr = 0;
       
        ArrayList InitSetlist = new ArrayList();
        ArrayList SkipperListComplement = new ArrayList();
        static int CurrentLevel = 1;
        static public int NrFeatures;
        ArrayList dr;
        ArrayList CurrentFeatureStopList = new ArrayList();
        ArrayList  FS= new  ArrayList ();
        ArrayList Ds = new ArrayList();
        ArrayList UnFoundLists = new ArrayList();
        ArrayList NotUniqueSetsPrevLevel = new ArrayList();
        ArrayList tmpNUList = new ArrayList();
        ArrayList tmpUList = new ArrayList();
        List<List<NotUniqueSet>> PublicNotUniq = new List<List<NotUniqueSet>>();
        List<ArrayList> PublicUniq = new List<ArrayList>();
        List<ArrayList> LstOfUniqueSets = new List<ArrayList>();
        List<List<ArrayList>> SkipChildsOfThusParents = new List<List<ArrayList>>();
        private Label label4;
        private Label lblTotalUnique;
        private Label lblRt;
        private Label lblRemainingTime;
        List<ArrayList> LstOfRowsFeaturesSetsIDs = new List<ArrayList>();
        private void CheckStopLevel()
        {
            if (chkShowCurrentLevel.Checked)
            {
                SetText("Current Level=" + CurrentLevel.ToString() + "  With only " + NotUniqueSetsPrevLevel.Count + " Not Unique Starting sets  were found in level" + (CurrentLevel - 1).ToString() + " and nonesense sets=" + NrOfNonesenseSets.ToString() + " with percentage=" + (100 * (double)NrOfNonesenseSets / (double)NotUniqueSetsPrevLevel.Count).ToString() + "  At Time:" + DateTime.Now.ToString() + Environment.NewLine);
                System.Diagnostics.Debug.Write("Current Level=" + CurrentLevel.ToString() + "  With only " + NotUniqueSetsPrevLevel.Count + " Not Unique Starting sets  were found in level" + (CurrentLevel - 1).ToString() + " and nonesense sets=" + NrOfNonesenseSets.ToString() + " with percentage=" + (100 * (double)NrOfNonesenseSets / (double)NotUniqueSetsPrevLevel.Count).ToString() + "  At Time:" + DateTime.Now.ToString() + Environment.NewLine);
                NrOfNonesenseSets = 0;
            }
            PublicNotUniq.Clear();
            tmpNUList = (ArrayList)NotUniqueSetsPrevLevel.Clone();
            NrOfNotUniqueThatWillBeChecked = 0;
            NotUniqueSetsPrevLevel.Clear();
            for (int k = 0; k < tmpNUList.Count; k++)
            {
                List<NotUniqueSet> temp = new List<NotUniqueSet>();
                PublicNotUniq.Add(temp);
                ArrayList tt = new ArrayList();
                PublicUniq.Add(tt);
            }
            Parallel.For(0, tmpNUList.Count, new ParallelOptions { MaxDegreeOfParallelism = 32 }, i =>
            {
            //for (int i = 0; i < tmpNUList.Count; i++)
            //{
                NotUniqueSet pl1 = (NotUniqueSet)tmpNUList[i];
                ArrayList OldSetIndices = (ArrayList)pl1._NotUniqueSetIndexes.Clone();
                for (int j = SkipperListComplement.IndexOf(pl1._NotUniqueSet[pl1._NotUniqueSet.Count - 1]) + 1; j < SkipperListComplement.Count; j++)
                {
                    pl1._NotUniqueSet.Add(SkipperListComplement[j]);
                    pl1._NotUniqueSetIndexes = (ArrayList)OldSetIndices.Clone();
                    IsUnique(pl1, i);                     
                    pl1._NotUniqueSet.RemoveAt(pl1._NotUniqueSet.Count - 1);
                }
                tmpNUList[i] = null;
            } );
        }
        
        ArrayList AllUniqueLists = new ArrayList();
        ArrayList StatisticsList = new ArrayList();
        ArrayList AllUniqueListsEqClass = new ArrayList();
        string AppPath ="";
        List<ArrayList> ValCatsCounts = new List<ArrayList>();
        
        int NrOfNonesenseSets = 0;
        
        
        private void Start_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        int TotalNumberOfObjects = 0;
        public int[] ClassesCounter;
        public int[] ClassesCounterTmp;
        Stopwatch stopwatch = new Stopwatch();
        private void Start()
        {
            
          TotalNumberOfObjects=  Dataset.Rules.Count;
          
            AppPath =txtFilePath.Text.Substring(0, txtFilePath.Text.LastIndexOf("\\")).ToString(); //System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            
            SetText(DateTime.Now + Environment.NewLine);
            
            Stopwatch RestartableStopWatch = new Stopwatch();
            RestartableStopWatch.Start();
            stopwatch.Start();
            ClassesCounter = new int[Ds.Count];
            ClassesCounterTmp = new int[Ds.Count];


            for (int x = 0; x < Dataset.Rules.Count; x++)
            {
                 
                    ArrayList t2 = new ArrayList();
                  
                
                LstOfRowsFeaturesSetsIDs.Add(t2);
            }
           // LstOfRowsFeaturesSetsIDs =(List<List<ArrayList>> )SkipChildsOfThusParents;
            foreach (ArrayList drow in Dataset.Rules)
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
                PowerSet();
                RowNr++;
            }
            stopwatch.Stop();
            timer1.Enabled = false;
            string Elapsedtime = stopwatch.Elapsed.ToString();
            SetText(Environment.NewLine + "NoneSense:" + TotalNoneSense.ToString() + Environment.NewLine + 
                "Total Subsumptions:" + TotalPartialReduct.ToString() + Environment.NewLine +
                "Total Unique:" + TotalUnique.ToString() + Environment.NewLine + 
                "Total Not Unique:" + TotalNotUnique.ToString());
            SetText(Environment.NewLine + "Total Number Of Searched Objects:" + Dataset.Rules.Count.ToString() + Environment.NewLine + 
                "Elapsedtime Time:" + Elapsedtime);
            GC.Collect();
            SaveNow(AppPath);
            MessageBox.Show("done");
        }
        long TotalNoneSense = 0;
        long TotalPartialReduct = 0;
        int MaxSubsetLength = 0;
        long TotalNotUnique = 0;
        long TotalUnique = 0;
        long NrOfNotUniqueThatWillBeChecked = 0;
        private void SaveNow(string AppPath)
        {
            AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<Root>" + Environment.NewLine + "<Inference>" + Environment.NewLine + "<Cluster>" + Environment.NewLine);
            for (int i = 0; i < LstOfRowsFeaturesSetsIDs.Count; i++)//rows
            {
                
                    ArrayList IDs = LstOfRowsFeaturesSetsIDs[i];
                    foreach (int id in IDs)
                    {
                        ArrayList tmpSet = LstOfUniqueSets[id];
                        LstOfUniqueSets[id]=null;
                            if (tmpSet != null)
                            {
                                sbXML.Append(Ds[(int)Dataset.Categories[i]].ToString().Replace(">", " Oc=\"" + htOc[id].ToString() + "\" Fc=\"" + tmpSet.Count.ToString() + "\" >") + Environment.NewLine);
                                if (MaxSubsetLength < tmpSet.Count)
                                {
                                    MaxSubsetLength = tmpSet.Count;
                                }
                                foreach (int Feature in tmpSet)
                                {
                                    
                                        sbXML.Append(FS[Feature] + Environment.NewLine);
                                    
                                }
                                sbXML.Append("</Rule>" + Environment.NewLine);
                            }
                    }
                
            }
            sbXML.Append("</Cluster>" + Environment.NewLine + "</Inference>" + Environment.NewLine + "</Root>");
            Random r=new Random();
            string p = r.Next(11111, 999999).ToString();
            StreamWriter s1 = new StreamWriter(AppPath + "\\"+p+"_Refined.XML");
            s1.Write(sbXML.ToString());
            s1.Close();
            SetText(Environment.NewLine + "Max Subset Length found=" + MaxSubsetLength.ToString());
            
           //xxxxxxxxxxxxxx
            StreamWriter s2  = new StreamWriter(AppPath + "\\" + p + "_outputscreen.txt");
            s2.Write(txtres.Text);
            s2.Close();
            sbXML.Clear();
        }
        Hashtable htOc = new Hashtable();
        public void PowerSet()
        {
            for (int k = 0; k <  SkipperListComplement.Count; k++)
            {
                List<NotUniqueSet> temp = new List<NotUniqueSet>();
                PublicNotUniq.Add(temp);
                ArrayList tt = new ArrayList();
                PublicUniq.Add(tt);
            }
            TotalNotUnique = TotalNotUnique + (PublicUniq.Count - 1)-TotalUnique;
            CurrentLevel = 1;
            ArrayList AllFeaturesFirstLevel = (ArrayList)SkipperListComplement.Clone();
            foreach (int f in AllFeaturesFirstLevel)
            {
                InitSetlist.Clear();
                InitSetlist.Add(f);
                NotUniqueSet nus = new NotUniqueSet();
                nus._NotUniqueSet =(ArrayList) InitSetlist.Clone();
                 IsUnique(nus, 0);
            }

            for (int thread = 0; thread < PublicUniq.Count; thread++)
            {
                ArrayList Threadlst = PublicUniq[thread];
                foreach (FeatureValues fv in Threadlst)
                {
                    LstOfUniqueSets.Add(fv.UnkSet);
                    int index = LstOfUniqueSets.Count - 1;
                    htOc.Add(index, fv.RowsIndices.Count);
                    foreach (int r in fv.RowsIndices)
                    {
                        LstOfRowsFeaturesSetsIDs[r].Add(index);
                    }
                }
            }
            PublicUniq.Clear();
            NotUniqueSetsPrevLevel.Clear();
            for (int i = 0; i < PublicNotUniq.Count; i++)
            {
                for (int j = 0; j < PublicNotUniq[i].Count; j++)
                {
                    NotUniqueSetsPrevLevel.Add((NotUniqueSet)PublicNotUniq[i][j]);
                }
            }

             
            CurrentLevel++;
            while (WorkMood())//SkipperListComplement.Count && NotUniqueSetsPrevLevel.Count > 0  )
            {

                CheckStopLevel();
                CurrentLevel++;

                for (int thread = 0; thread < PublicUniq.Count; thread++)
                {
                    ArrayList Threadlst = PublicUniq[thread];
                    foreach (FeatureValues fv in Threadlst)
                    {
                        LstOfUniqueSets.Add(fv.UnkSet);
                        int index = LstOfUniqueSets.Count - 1;
                        htOc.Add(index, fv.RowsIndices.Count);
                        foreach (int r in fv.RowsIndices)
                        {
                            LstOfRowsFeaturesSetsIDs[r].Add(index);
                        }
                    }
                }
                PublicUniq.Clear();
                NotUniqueSetsPrevLevel.Clear();
                for (int i = 0; i < PublicNotUniq.Count; i++)
                {
                    for (int j = 0; j < PublicNotUniq[i].Count; j++)
                    {
                        NotUniqueSetsPrevLevel.Add((NotUniqueSet)PublicNotUniq[i][j]);
                    }
                }
            }

        }

        private bool WorkMood()
        {
          return ((CurrentLevel <= SkipperListComplement.Count && NotUniqueSetsPrevLevel.Count > 0));
        }
        ArrayList InitiatorFeatures = new ArrayList();
        public void IsUnique(NotUniqueSet CurrentSetPositions, int threadID)
        {
            if (HasSubsumtions_Traditional(CurrentSetPositions._NotUniqueSet))
            {
                TotalPartialReduct++;
                return;
            }
            ArrayList PassedSetRowIndices = new ArrayList();
            ArrayList allListInterSection = new ArrayList();
            if (CurrentLevel > 1)
            {
                    int NewAddedFeature =(int) CurrentSetPositions._NotUniqueSet[CurrentSetPositions._NotUniqueSet.Count - 1];
                    FeatureValues fv = DsMeta[NewAddedFeature];
                    PassedSetRowIndices.Add((ArrayList)fv.RowsIndices.Clone());
                    PassedSetRowIndices.Add((ArrayList)CurrentSetPositions._NotUniqueSetIndexes.Clone());
                ArrayList OldSetIndices = (ArrayList)CurrentSetPositions._NotUniqueSetIndexes.Clone(); 
                allListInterSection=fnGetIntersection1(PassedSetRowIndices);
                CurrentSetPositions._NotUniqueSetIndexes = (ArrayList)allListInterSection.Clone();
                if (allListInterSection.Count == OldSetIndices.Count)
                {
                    NrOfNonesenseSets++;
                    TotalNoneSense++;
                    return;
                }
            }
            else
            {
                FeatureValues fv = DsMeta[(int)CurrentSetPositions._NotUniqueSet[0]];
                allListInterSection.AddRange(fv.RowsIndices);
                CurrentSetPositions._NotUniqueSetIndexes = allListInterSection;
            }
            int c = 1;
            string t = Dataset.Categories[Int32.Parse(allListInterSection[0].ToString())].ToString();
            for (int k=1 ;k< allListInterSection.Count;k++)
            {
                if (t!= Dataset.Categories[Int32.Parse(allListInterSection[k].ToString())].ToString())
                {
                    c ++;
                    break;
                }
            }
            double MaxClassCount = ClassesCounterTmp.Max();
            if ( c==1) 
            {
                TotalUnique++;
                FeatureValues fv = new FeatureValues();
                fv.UnkSet = (ArrayList)CurrentSetPositions._NotUniqueSet.Clone();
                fv.RowsIndices = (ArrayList)allListInterSection.Clone();
                PublicUniq[threadID].Add(fv);
                if (CurrentLevel == 1)
                {
                    SkipperListComplement.Remove(CurrentSetPositions._NotUniqueSet[0]);
                    if (SkipperListComplement.Count > 0)
                    {
                        LastFeature = (int)SkipperListComplement[SkipperListComplement.Count - 1];
                    }
                }
                return;
            }
            
            // not unique Handling
            TotalNotUnique++;
            if (!CurrentSetPositions._NotUniqueSet.Contains(LastFeature))
            {
                NotUniqueSet nus = new NotUniqueSet();
                nus._NotUniqueSet = (ArrayList)CurrentSetPositions._NotUniqueSet.Clone();
                nus._NotUniqueSetIndexes = (ArrayList)allListInterSection.Clone();
                PublicNotUniq[threadID].Add(nus);
                NrOfNotUniqueThatWillBeChecked++;
            }
            //end
        }

         
        private ArrayList fnGetIntersection1(ArrayList PassedSetRowIndices)
        {
            ArrayList c1 = (ArrayList)PassedSetRowIndices[0];
            ArrayList c2 = (ArrayList)PassedSetRowIndices[1];
            ArrayList intersection = new ArrayList();
            if (c1.Count < c2.Count)
            {
                foreach (int i in c1)
                {
                    if (c2.Contains(i))
                    {
                        intersection.Add(i);
                    }
                }
            }
            else
            {
                foreach (int i in c2)
                {
                    if (c1.Contains(i))
                    {
                        intersection.Add(i);
                    }
                }
            }


            return intersection;
        }
       
        int LastFeature = 0;
        private bool HasSubsumtions_Traditional(ArrayList pSubSet)
        {
            foreach (int j in pSubSet)
            {
               ArrayList lstUniqueSetsOfRow = LstOfRowsFeaturesSetsIDs[RowNr];
                
                    foreach (int id in lstUniqueSetsOfRow)
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
        
        ArrayList GeneralList = new ArrayList();
        ArrayList GeneralListClass = new ArrayList();
       
        private void FillSkipperListWithEmptyFeatures(ArrayList   drow)
        {
            SkipperListComplement.Clear();

            reOrderFeaturesList((ArrayList)drow.Clone());

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
                    if (((FeatureValues)(DsMeta[(int)UnOrderedFeaturesSet[x]])).count > ((FeatureValues)(DsMeta[(int)UnOrderedFeaturesSet[x+1]])).count)
                    {
                        int tmp = (int)UnOrderedFeaturesSet[x];
                        UnOrderedFeaturesSet[x] = UnOrderedFeaturesSet[x + 1];
                        UnOrderedFeaturesSet[x + 1] = tmp;
                    }
                }
            }
            SkipperListComplement = UnOrderedFeaturesSet;
        }
        
        ArrayList XMLRules_RowsXCategories = new ArrayList();
        Hashtable RowsXCategories = new Hashtable();
        private void LoadDataSetFromXML(string Path)
        {
           // int NumberOfRules = 0;
            string line;
            
            int CategoryIndex = 0;
            ArrayList tmpFeaturesList = new ArrayList();
            using (StreamReader reader = new StreamReader(Path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {

                        if (line.ToLower().IndexOf("<rule") != -1)
                        {
                           
                                
                            string s = line;
                            s = s.Replace("'", "");
                            if (!Ds.Contains(s))
                            {
                                Ds.Add(s);
                            }
                            CategoryIndex = Ds.IndexOf(s);
                               // NumberOfRules++;
                        }
                        if (line.ToLower().IndexOf("</rule") != -1)
                        {
                            Dataset.Rules.Add((ArrayList)tmpFeaturesList.Clone());
                            Dataset.Categories.Add(CategoryIndex);
                            tmpFeaturesList.Clear();
                        }
                         
                            if ( line.ToLower().Contains("<tuple"))
                            {
                               string DecodedVal = StringOperations.GetAttrVal("Val",line);
                               string NewLine = line.Replace(DecodedVal, System.Web.HttpUtility.HtmlEncode(DecodedVal));
                                
                                if (line.ToLower().Contains("tuple") && !FS.Contains(line))
                                {
                                    FS.Add(NewLine);
                                }
                                int fIndex = FS.IndexOf(NewLine);
                                if (!tmpFeaturesList.Contains(fIndex))
                                {
                                    tmpFeaturesList.Add(fIndex);
                                }
                            }
                        
                    }

                }
            }

            for (int v = 0; v < FS.Count;v++ )
            {
                 FeatureValues fv = new FeatureValues();
                for (int i = 0; i < Dataset.Rules.Count; i++)
                {
                    ArrayList tuple = (ArrayList)Dataset.Rules[i];
                  
                    if (tuple.Contains(v))
                    {
                        fv.count++;
                        fv.RowsIndices.Add(i);
                    }
                   
                }
                DsMeta.Add(fv);
            }
               
             


        }
        
        public PNUXML()
        {
            InitializeComponent();
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
                
                    LoadDataSetFromXML(path);
                

              
                btnStart.Enabled = true;
            }
            else
            { btnStart.Enabled = false; }
        }
        //ArrayList ValuesCounter =  new ArrayList();
        
        List<ArrayList> CategoriesMeta = new List<ArrayList>();
        Hashtable htFeatureIndicesForCurrentObject = new Hashtable();
        List<FeatureValues> DsMeta = new List<FeatureValues>();// FeatureIndex|Value Index|Value rows indices + its existence count
       

 
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

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }
            
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = stopwatch.Elapsed.ToString();
            lblnoneSense.Text= TotalNoneSense.ToString();
            lblPartialreduct.Text = TotalPartialReduct.ToString();
            lbltotalNotuniqueWillBeCheckedOutPut.Text = NrOfNotUniqueThatWillBeChecked.ToString();
            lblTotalNotUniqueOutput.Text = TotalNotUnique.ToString();
            lblTotalUnique.Text = TotalUnique.ToString();
            TimeSpan t = TimeSpan.FromSeconds((stopwatch.Elapsed.TotalSeconds * TotalNumberOfObjects) / (RowNr + 1));

            lblRt.Text = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s", t.Days, t.Hours,
                            t.Minutes,
                            t.Seconds);
        }
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtres = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
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
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotalUnique = new System.Windows.Forms.Label();
            this.lblRt = new System.Windows.Forms.Label();
            this.lblRemainingTime = new System.Windows.Forms.Label();
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
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Start_Click);
            // 
            // txtres
            // 
            this.txtres.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtres.Location = new System.Drawing.Point(13, 184);
            this.txtres.Multiline = true;
            this.txtres.Name = "txtres";
            this.txtres.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtres.Size = new System.Drawing.Size(860, 322);
            this.txtres.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(279, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(94, 23);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse(XML)";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(13, 39);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(260, 20);
            this.txtFilePath.TabIndex = 15;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkShowCurrentLevel
            // 
            this.chkShowCurrentLevel.AutoSize = true;
            this.chkShowCurrentLevel.Checked = true;
            this.chkShowCurrentLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCurrentLevel.Location = new System.Drawing.Point(392, 42);
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
            this.lblTime.Location = new System.Drawing.Point(127, 133);
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
            this.label1.Location = new System.Drawing.Point(32, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Elapsed Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(540, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Nonesense:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(528, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Subsumptions:";
            // 
            // lblnoneSense
            // 
            this.lblnoneSense.AutoSize = true;
            this.lblnoneSense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnoneSense.ForeColor = System.Drawing.Color.Red;
            this.lblnoneSense.Location = new System.Drawing.Point(626, 39);
            this.lblnoneSense.Name = "lblnoneSense";
            this.lblnoneSense.Size = new System.Drawing.Size(0, 20);
            this.lblnoneSense.TabIndex = 32;
            // 
            // lblPartialreduct
            // 
            this.lblPartialreduct.AutoSize = true;
            this.lblPartialreduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartialreduct.ForeColor = System.Drawing.Color.Green;
            this.lblPartialreduct.Location = new System.Drawing.Point(626, 63);
            this.lblPartialreduct.Name = "lblPartialreduct";
            this.lblPartialreduct.Size = new System.Drawing.Size(0, 20);
            this.lblPartialreduct.TabIndex = 33;
            // 
            // lblTotalNotUnique
            // 
            this.lblTotalNotUnique.AutoSize = true;
            this.lblTotalNotUnique.Location = new System.Drawing.Point(513, 110);
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
            this.lblTotalNotUniqueOutput.Location = new System.Drawing.Point(626, 110);
            this.lblTotalNotUniqueOutput.Name = "lblTotalNotUniqueOutput";
            this.lblTotalNotUniqueOutput.Size = new System.Drawing.Size(0, 17);
            this.lblTotalNotUniqueOutput.TabIndex = 35;
            // 
            // lbltotalNotuniqueWillBeCheckedOutPut
            // 
            this.lbltotalNotuniqueWillBeCheckedOutPut.AutoSize = true;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbltotalNotuniqueWillBeCheckedOutPut.ForeColor = System.Drawing.Color.Purple;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Location = new System.Drawing.Point(626, 88);
            this.lbltotalNotuniqueWillBeCheckedOutPut.Name = "lbltotalNotuniqueWillBeCheckedOutPut";
            this.lbltotalNotuniqueWillBeCheckedOutPut.Size = new System.Drawing.Size(0, 17);
            this.lbltotalNotuniqueWillBeCheckedOutPut.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(439, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Total Not Unique will be checked:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(534, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Total Unique:";
            // 
            // lblTotalUnique
            // 
            this.lblTotalUnique.AutoSize = true;
            this.lblTotalUnique.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblTotalUnique.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblTotalUnique.Location = new System.Drawing.Point(616, 133);
            this.lblTotalUnique.Name = "lblTotalUnique";
            this.lblTotalUnique.Size = new System.Drawing.Size(16, 17);
            this.lblTotalUnique.TabIndex = 39;
            this.lblTotalUnique.Text = "u";
            // 
            // lblRt
            // 
            this.lblRt.AutoSize = true;
            this.lblRt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRt.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblRt.Location = new System.Drawing.Point(128, 160);
            this.lblRt.Name = "lblRt";
            this.lblRt.Size = new System.Drawing.Size(16, 18);
            this.lblRt.TabIndex = 46;
            this.lblRt.Text = "0";
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.AutoSize = true;
            this.lblRemainingTime.Location = new System.Drawing.Point(20, 157);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(86, 13);
            this.lblRemainingTime.TabIndex = 45;
            this.lblRemainingTime.Text = "Remaining Time:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 509);
            this.Controls.Add(this.lblRt);
            this.Controls.Add(this.lblRemainingTime);
            this.Controls.Add(this.lblTotalUnique);
            this.Controls.Add(this.label4);
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
            this.Name = "Form1";
            this.Text = "Pursuit_not_Unique_Algorithm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtres;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
        }
        }

       
        
    }
public static class  Dataset
{
    public static ArrayList Rows = new ArrayList();
    public static ArrayList Categories = new ArrayList();
    public static ArrayList Rules = new ArrayList();
}
  


