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
using Pursuit_not_Super_Reduct_only_Algorithm.CS;
using DClassifier.CS;
using System.Text.RegularExpressions;

namespace PWRSet
{
    public partial class MyDClassifier : Form
    {
        public MyDClassifier()
        {
            InitializeComponent();
        }
        
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Start();
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            if (this.lblRes.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblRes.Text=text;
            }
        }
        private void SetTotSubsumptionsText(string text)
        {
            if (this.lblSubsumptions.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTotSubsumptionsText);
                this.Invoke(d, new object[] { text });
            }
            else
            {

                this.lblSubsumptions.Text = text;
            }
        }
        private void SetTotUnfoundText(string text)
        {
            if (this.lblUnfound.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTotUnfoundText);
                this.Invoke(d, new object[] { text });
            }
            else
            {

                this.lblUnfound.Text = text;
            }
        }
        private void SetTxtText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtRes.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtRes.AppendText(text);
            }
        }

       public   string  ChoosenCriteria = "";
        private void btnCompare_Click(object sender, EventArgs e)
        {
            
            this.backgroundWorker1.RunWorkerAsync();
            btnPredict.Enabled = false;
            btnApply.Enabled = true;
        }
        ArrayList SubSet = new ArrayList();
        ArrayList FirstFileDs = new ArrayList();
        ArrayList FirstFileFs = new ArrayList();
        ArrayList FirstFileAsSubsets = new ArrayList();
        ArrayList FirstFileAsSubsetsClasses = new ArrayList();
        public static bool IsNumber(string s)
        {
            return s.All(char.IsDigit);
        }
        public void LoadTrainingFile( string path)
        {
            try
            {
            string line;
            // First File Lists
            line = "";
            bool isFirst = true;
            string[] strCn= new string[5];
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line))
                    {
                        string s = line.Replace(" =", "=").Replace("'", "\"");
                        line = s;
                        if (line.IndexOf("<Rule ") != -1)
                        {
                            string RuleClass = StringOperations.GetAttrVal("Category", line);
                            if (!DistinctClasses.ContainsKey(RuleClass))
                            {
                                PredictedClassLabelAnalysis p = new PredictedClassLabelAnalysis();
                                p.className = RuleClass;
                                p.Rules_ClassCount++;
                                DistinctClasses.Add(RuleClass, p);
                            }
                            else {
                                PredictedClassLabelAnalysis p = (PredictedClassLabelAnalysis)DistinctClasses[RuleClass];
                                //p.className = RuleClass;
                                p.Rules_ClassCount++;
                                DistinctClasses[RuleClass]= p;
                            }
                            FirstFileAsSubsetsClasses.Add(line);
                           
                        }
                        if (line.IndexOf("</Rule>") > -1)
                        {
                            FirstFileAsSubsets.Add((ArrayList)SubSet.Clone());
                            SubSet.Clear();
                        }
                        if (line.IndexOf("<Tuple") > -1)
                        {
                            if (!SubSet.Contains(line))
                            {
                                SubSet.Add(line);
                            }
                        }
                        if (line.IndexOf("<Root") > -1)
                        {
                            NrOfInstances = Int32.Parse(StringOperations.GetAttrVal("M", line));
                            string z = StringOperations.GetAttrVal("CategoriesN", line);// In this section Get each category with nr of cases in training set
                              strCn = z.Split(',');
                            if (strCn.Length > 0)
                            {
                                for (int ii = 0; ii < strCn.Length-1; ii+=2)
                                {
                                    CategoriesN.Add(strCn[ii], strCn[ii + 1]);
                                }
                            }
                        }
                        
                    }
                }
            }
           for(int g=0;g<strCn.Length-1;g+=2)
           {
               PredictedClassLabelAnalysis p =(PredictedClassLabelAnalysis) DistinctClasses[strCn[g]];
               p.ds_ClassCount =Int32.Parse( strCn[g + 1]);
               DistinctClasses[strCn[g]] = p;
           }
            int NumberOfRules=FirstFileAsSubsetsClasses.Count;
            for (int i = 0; i <NumberOfRules ; i++)
            {
                //if (!FirstFileDs.Contains(FirstFileAsSubsetsClasses[i].ToString()))
                //{
                //    FirstFileDs.Add(FirstFileAsSubsetsClasses[i].ToString());
                //}
                for(int j=0;j< ((ArrayList)FirstFileAsSubsets[i]).Count;j++)
                {
                    if (!FirstFileFs.Contains(((ArrayList)FirstFileAsSubsets[i])[j].ToString()))
                    {
                        FirstFileFs.Add(((ArrayList)FirstFileAsSubsets[i])[j].ToString());
                    }
                }

            }
            ArrayList tmpFs = new ArrayList();
            foreach (string x in FirstFileFs)// need to extract all features
            {
                string y = StringOperations.GetAttrVal("Prop", x);// x.Substring(x.IndexOf("<Rule ") + ("<Rule ").Length + 2, x.IndexOf("=")-(x.IndexOf("<Rule ") + ("<Rule ").Length)-2);
                string v=StringOperations.GetAttrVal("Val", x);
                     
                if (!tmpFs.Contains(y))
                {
                    tmpFs.Add(y);
                    FeaturesCharacteristics fc = new FeaturesCharacteristics();
                    fc.FeatureName = y;
                    fc.FeatureValues.Add(v);
                    TrainingSetMetaData.Add(y, fc);
                }
                else
                { 
                  ( (FeaturesCharacteristics) TrainingSetMetaData[y]).FeatureValues.Add(v);
                }
                if (((FeaturesCharacteristics)TrainingSetMetaData[y]).isContinuous != false)
                {
                    if (IsNumber(v))
                    {
                        ((FeaturesCharacteristics)TrainingSetMetaData[y]).isContinuous = true;
                    }
                }
            }

            foreach (DictionaryEntry de in TrainingSetMetaData)
            {
                TrainingSetMetaDataList.Add((FeaturesCharacteristics)de.Value);
            }


                for (int i = 0; i < FirstFileAsSubsets.Count; i++)
            {
                ArrayList tmp = new ArrayList();
                ArrayList itemset = new ArrayList();
                tmp = (ArrayList)(FirstFileAsSubsets[i]);
                foreach(string tuple in tmp)
                {
                    int featureIndex=0;
                foreach (FeaturesCharacteristics fc in TrainingSetMetaDataList)
                {

                    if (StringOperations.GetAttrVal("Prop", tuple) == fc.FeatureName) // recoginze feature value
                    {
                        int ValueIndex=0;
                        foreach (string val in fc.FeatureValues)
                        {

                            if (StringOperations.GetAttrVal("Val", tuple) == val)
                            {
                                itemset.Add(featureIndex.ToString()+ ","  + ValueIndex.ToString());
                                break;
                            }
                            ValueIndex++;
                        }
                    }
                    featureIndex++;
                }
                }
                TrainingSubsets.Add((ArrayList)itemset.Clone());
            }
            }
            catch
            { }
        }
        ArrayList TrainingSubsets = new ArrayList();
        Hashtable TrainingSetMetaData = new Hashtable();
        ArrayList TrainingSetMetaDataList = new ArrayList();
        Hashtable htRes = new Hashtable();
         void Start()
         {
             
             int TestCounter = 0;
             for ( int i=0;i< TestItemsets.Count;i++)
             {
                 FoundItemsetsProperties itemSetProperties = new FoundItemsetsProperties();
                 itemSetProperties.testIndex = i;
                 ArrayList testItemset =(ArrayList) TestItemsets[i];
              for ( int j=0;j< TrainingSubsets.Count;j++)
             {   
                  ArrayList trainingItemset =(ArrayList) TrainingSubsets[j];
                  if (HasSubsumtions_Traditional(trainingItemset, testItemset))
                 {
                     //اولا هنا المفروض هسجل رقم الصف اللى بختبره 
                     itemSetProperties.itemsetIndexInTrainingSet.Add(j);
                     //ثانيا هسجل كل كلاس مع خصائص الروول بمعنى هذه الروول اتكررت كام مرة وبها كام فيتشر
                      itemSetProperties.itemsetIndexInTrainingSetClassLabel.Add(FirstFileAsSubsetsClasses[j]);
                     //string x = FirstFileAsSubsetsClasses[j].ToString();

                 }
                 TestCounter++;
             }
              TestResults.Add(itemSetProperties);
             }
             Results_Analysis();
            SetTxtText(Environment.NewLine + ".........Finished..........");
      
         }
         Hashtable DistinctClasses = new Hashtable();
         ArrayList TestResults = new ArrayList();
         int NrOfInstances = 0;
         Hashtable CategoriesN = new Hashtable();
         private void Results_Analysis()
         {
             this.Invoke((MethodInvoker)delegate()
             {
                 ChoosenCriteria = ddlCriteris.Text;
             });
             
             for (int i = 0; i < TestResults.Count; i++)
             {
                 Hashtable htclass = new Hashtable();
                 foreach ( DictionaryEntry de in DistinctClasses)
                 {
                     PredictedClassLabelAnalysis p = new PredictedClassLabelAnalysis();
                     htclass.Add(de.Key, p);
                 }
                 
               
                 FoundItemsetsProperties t = (FoundItemsetsProperties)TestResults[i];
                 foreach (string rule in t.itemsetIndexInTrainingSetClassLabel)
                 {
                     string Category = StringOperations.GetAttrVal("Category", rule);
                     string Oc = StringOperations.GetAttrVal("Oc", rule);
                     string Fc = StringOperations.GetAttrVal("Fc", rule);
                     string Rids = StringOperations.GetAttrVal("Rid", rule);

                     //1- Get DistinctClasses and iterate over them
                     //2- Get All mutual Rows for each class
                     //3- Get Max(Oc/M)
                     //4- Get Max(Oc/c) where c = number of rows for this class 
                     foreach (  DictionaryEntry str in DistinctClasses)
                     {
                         if (Category == str.Key.ToString())
                         {
                             PredictedClassLabelAnalysis p = (PredictedClassLabelAnalysis)htclass[Category];
                             p.rowIDs += Rids;
                             p.Ocs.Add(Oc);
                             p.className = Category;
                             p.Supp_c_to_i.Add(double.Parse(CategoriesN[Category].ToString()) / double.Parse(NrOfInstances.ToString()));
                             p.Supp_i_to_c.Add(double.Parse(Oc.ToString()) / double.Parse(NrOfInstances.ToString()));
                             p.Conf_c_to_i.Add(double.Parse(Oc.ToString()) / double.Parse(CategoriesN[Category].ToString()));
                             htclass[Category] = p;
                             break;
                         }
                     }
                 }
                     if (ChoosenCriteria == "MaxFcXOc" || true )
                     {
                        StringBuilder sb2 =new StringBuilder();
                        PredictedClassLabelAnalysis f = new PredictedClassLabelAnalysis();
                         foreach (DictionaryEntry de in htclass)
                         {
                             //if (f.GetMutualRowsIds().Count < ((PredictedClassLabelAnalysis)de.Value).GetMutualRowsIds().Count) // swap to get max
                             //{
                                 f.rowIDs = ((PredictedClassLabelAnalysis)de.Value).rowIDs;
                                 f.className = ((PredictedClassLabelAnalysis)de.Value).className;
                                 f.Ocs = ((PredictedClassLabelAnalysis)de.Value).Ocs;
                                 f.Conf_c_to_i = ((PredictedClassLabelAnalysis)de.Value).Conf_c_to_i;
                                 f.Supp_c_to_i = ((PredictedClassLabelAnalysis)de.Value).Supp_c_to_i;
                                 f.Supp_i_to_c = ((PredictedClassLabelAnalysis)de.Value).Supp_i_to_c;
                                 f.DistinctRowsIDs = ((PredictedClassLabelAnalysis)de.Value).GetMutualRowsIds();
                                 f.rowIDsCount = f.DistinctRowsIDs.Count;
                            // }

                                 sb2.Append(f.className + "," + f.rowIDsCount.ToString() + "," + f.GetMax_Conf_c_to_i().ToString() + "," + f.GetMax_Supp_c_to_i().ToString() + "," + f.GetMax_Supp_i_to_c().ToString()+" | ");
                         }
                         
                         SetTxtText(Environment.NewLine + i.ToString() + "," + sb2.ToString());
                     }
 
                 
                  
//            SetTxtText(Environment.NewLine + i.ToString()+"," + f.className);
            //f = new PredictedClassLabelAnalysis();
            // FinalObjectsClassLabels = new ArrayList();
           
             }
             
         }
        
        
         int faults = 0;
         ArrayList FinalObjectsClassLabels = new ArrayList();
         struct OutputClassLabels {
             public string classLabel  ;
             public int OcxFc  ;
           public  int  counter;
          // public float VarsMean;
           public int OcSum;
           public int FcSum;
           public int W;
           public double W2;
         }

        
        private bool HasSubsumtions_Traditional(ArrayList Trainingset, ArrayList TestSet)
        {
            
                 ArrayList tmplst = new ArrayList();
                 foreach (string x in Trainingset)
                {

                    if (TestSet.Contains(x))
                    {
                        tmplst.Add(x);
                    }
                    else
                    {
                        return false;
                    }

                    if (tmplst.Count == Trainingset.Count)
                    {
                        return true;
                    }


                }
            return false;
        }
       
       
       
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);


        }
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            //string xxx = "<Tuple Age_=\"35\" />";
            //string v = xxx.Substring(xxx.IndexOf("\""), xxx.LastIndexOf("\"") - xxx.IndexOf("\""));
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            txtBrowse1.Text = path;
            LoadTrainingFile(path);
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = string.Empty;
            openFileDialog2.ShowDialog();
            string path = openFileDialog2.FileName;
            txtBrowse2.Text = path;
            LoadDataFromCSV(path);
        }
        DataTable dt = new DataTable();
        //string CategoryColumnName = "";
        ArrayList SecondFileDs = new ArrayList();
        List<ArrayList> SecondFileFs = new List<ArrayList>();
        ArrayList SecondFileAsSubsets = new ArrayList();
        ArrayList SecondFileAsSubsetsClasses = new ArrayList();
        private void LoadDataFromCSV(string path)
        {
           
            DataColumn dc;
            dc = new DataColumn();
            string header;
            try
            {
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
                                //string[] cls = line.Trim().Replace(";", ",").Replace("\t", ",").Split(',');
                                //CategoryColumnName = cls[cls.Length - 1];
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
            }
            catch
            {
                MessageBox.Show("you did't choose a test file!");
                return;
            }
            SecondFileFs.Clear();
            for (int b = 0; b < dt.Columns.Count - 1; b++)
            {
                ArrayList tmpa = new ArrayList();
                SecondFileFs.Add((ArrayList)tmpa.Clone());
            }


            for (int r = 0; r < dt.Rows.Count; r++)
            {
                List<ArrayList> LstOfFeatures = new List<ArrayList>();
                for (int c = 0; c < dt.Columns.Count - 1; c++)
                {
                    ArrayList tmp = new ArrayList();
                    LstOfFeatures.Add(tmp);
                    string CurVal = "";
                    CurVal = dt.Rows[r][c].ToString().Trim();
                        dt.Rows[r][c] = CurVal;

                }
            }
            dataGridView1.DataSource = dt.DefaultView;
            DataColumn dcol = new DataColumn();
            DataRow drow;
            for(int i =0;i<dt.Rows.Count;i++)
            {
                ArrayList tmp = new ArrayList();
                drow = dt.Rows[i];
                 
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dcol=  dt.Columns[j];
                    string featureName = dcol.ColumnName;
                    string featureVal = drow[j].ToString();
                    int featureIndex = 0;
                    foreach (FeaturesCharacteristics fc in TrainingSetMetaDataList)
                    {
                        if (featureName == fc.FeatureName)
                        {
                            int ValueIndex = 0;
                                foreach (string val in fc.FeatureValues)
                                {
                                    if (featureVal.Contains(val))
                                    {
                                        tmp.Add(featureIndex.ToString()+"," + ValueIndex.ToString());
                                        break;
                                    }
                                    ValueIndex++;
                                }
                            
                            break;
                        }
                        featureIndex++;
                    }
                   
                }
                TestItemsets.Add((ArrayList)tmp.Clone());
            }             

        }

        ArrayList TestItemsets = new ArrayList();
         
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MultiTextSearch ss = new MultiTextSearch();
            ss.Activate();
            ss.Show();
        }

        private void MyClassifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            nCr frm = new nCr();
            frm.Show();
            this.Dispose();
        }

        private void MyClassifier_Load(object sender, EventArgs e)
        {
            
            ddlCriteris.SelectedIndex = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Results_Analysis();
            SetTxtText(Environment.NewLine + ".........Finished..........");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-2;i++ )
            {
                for (int j = i+1; j < dataGridView1.Rows.Count-1; j++)
                {

                    for (int k = 0; k < dataGridView1.Columns.Count-1; k++)
                    {

                        if (dataGridView1.Rows[i].Cells[k].Value.ToString() != dataGridView1.Rows[j].Cells[k].Value.ToString())
                        {
                            break;
                        }
                        if (k == dataGridView1.Columns.Count - 2)
                        {
                            for (int l = 0; l < dataGridView1.Columns.Count - 1; l++)
                            {
                                dataGridView1.Rows[i].Cells[l].Value = "";
                                dataGridView1.Rows[j].Cells[l].Value = "";
                            }
                            SetTxtText(Environment.NewLine + "Current index=" + i.ToString() +" , ("+i.ToString()+","+j.ToString()+ ") Ereased.");
                        }
                    }
                }
                SetTxtText(Environment.NewLine + "Current index=" + i.ToString());
            }
            MessageBox.Show("Finished");
        }

        private void txtBrowse1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MyDClassifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
            this.Dispose();
        }
    }
}
