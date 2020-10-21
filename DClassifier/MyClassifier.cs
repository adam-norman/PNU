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

namespace PWRSet
{
    public partial class MyClassifier : Form
    {
        public MyClassifier()
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

        public void LoadTrainingFile( string path)
        {
            try
            {
            string line;
            // First File Lists
            line = "";
            bool isFirst = true;

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
                            FirstFileAsSubsetsClasses.Add(line);
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                FirstFileAsSubsets.Add((ArrayList)SubSet.Clone());
                                SubSet.Clear();
                            }
                        }

                        if (line.IndexOf("<Tuple") > -1)
                        {
                            if (!SubSet.Contains(line))
                            {
                                SubSet.Add(line);
                            }
                        }
                        // finished
                        if (line.IndexOf("</Root>") > -1)
                        {
                            FirstFileAsSubsets.Add((ArrayList)SubSet.Clone());
                            SubSet.Clear();
                        }
                    }
                }
            }
           
            int NumberOfRules=FirstFileAsSubsetsClasses.Count;
            for (int i = 0; i <NumberOfRules ; i++)
            {
                if (!FirstFileDs.Contains(FirstFileAsSubsetsClasses[i].ToString()))
                {
                    FirstFileDs.Add(FirstFileAsSubsetsClasses[i].ToString());
                }
                for(int j=0;j< ((ArrayList)FirstFileAsSubsets[i]).Count;j++)
                {
                    if (!FirstFileFs.Contains(((ArrayList)FirstFileAsSubsets[i])[j].ToString()))
                    {
                        FirstFileFs.Add(((ArrayList)FirstFileAsSubsets[i])[j].ToString());
                    }
                }

            }
            ArrayList tmpFs = new ArrayList();
            foreach (string x in FirstFileFs)
            {
                string y = x.Substring(x.IndexOf("<Rule ") + ("<Rule ").Length + 2, x.IndexOf("=")-(x.IndexOf("<Rule ") + ("<Rule ").Length)-2);
                if (!tmpFs.Contains(y))
                {
                    tmpFs.Add(y);
                }
            }
            foreach (string s in tmpFs)
            {
                FeaturesCharacteristics fc = new FeaturesCharacteristics();
                fc.FeatureName = s;
                for (int i = 0; i < NumberOfRules; i++)
                {
                    for (int j = 0; j < ((ArrayList)FirstFileAsSubsets[i]).Count; j++)
                    {
                        string cc=((ArrayList)FirstFileAsSubsets[i])[j].ToString();
                        if (cc.Contains(" " + s + "="))
                        {
                            string h = cc.Substring(cc.IndexOf("=") + 2, cc.LastIndexOf("\"") - cc.IndexOf("=") - 2);
                            if (!fc.FeatureValues.Contains(h))
                            {
                                fc.FeatureValues.Add(h.Trim());
                            }
                        }
                    }
                }
                foreach (string str in fc.FeatureValues)
                {
                    if (str.Contains("=") || str.Contains("<") || str.Contains(">"))
                    {
                        fc.isContinuous = true;
                        break;
                    }
                }
                TrainingSetMetaData.Add(fc);
            }



            for (int i = 0; i < FirstFileAsSubsets.Count; i++)
            {
                ArrayList tmp = new ArrayList();
                ArrayList itemset = new ArrayList();
                tmp = (ArrayList)(FirstFileAsSubsets[i]);
                foreach(string tuple in tmp)
                {
                    int featureIndex=0;
                foreach (FeaturesCharacteristics fc in TrainingSetMetaData)
                {
                    
                    if (tuple.Contains(" " + fc.FeatureName + "="))
                    {
                        int ValueIndex=0;
                        foreach (string val in fc.FeatureValues)
                        {
                            
                            if (tuple.Contains(val))
                            {
                                itemset.Add(featureIndex.ToString()+ ","  + ValueIndex.ToString());
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
        ArrayList TrainingSetMetaData = new ArrayList();
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
                     //FirstFileAsSubsetsClasses
                     itemSetProperties.itemsetIndexInTrainingSet.Add(j);
                     itemSetProperties.itemsetIndexInTrainingSetClassLabel.Add(FirstFileAsSubsetsClasses[j]);
                     string x = FirstFileAsSubsetsClasses[j].ToString();

                     //اولا هنا المفروض هسجل رقم الصف اللى بختبره 
                     //ثانيا هسجل كل كلاس مع خصائص الروول بمعنى هذه الروول اتكررت كام مرة وبها كام فيتشر

                 }
                 TestCounter++;
             }
              TestResults.Add(itemSetProperties);
             }
             Results_Analysis();
            SetTxtText(Environment.NewLine + ".........Finished..........");
      
         }
         ArrayList TestResults = new ArrayList();
         private void Results_Analysis()
         {
             Hashtable htclass = new Hashtable();
             for (int i = 0; i < TestResults.Count; i++)
             {
                 FoundItemsetsProperties r = (FoundItemsetsProperties)TestResults[i];
                
                 
                 
                 foreach (string xn in r.itemsetIndexInTrainingSetClassLabel)
                 {
                     string Category = GetAttrVal("Category", xn);
                     if (!htclass.ContainsKey(Category))
                     {
                         htclass.Add(Category, 1);
                     }
                     else 
                     {
                         htclass[Category] =Int32.Parse( htclass[Category].ToString()) + 1;
                     }
                     string Oc = GetAttrVal("Oc", xn);// xn.Attributes["Oc"].Value;
                     string Fc = GetAttrVal("Fc", xn);// xn.Attributes["Fc"].Value;
                     string _w = GetAttrVal("W", xn);// xn.Attributes["W"].Value;
                     
                     int counter = FinalObjectsClassLabels.Count ;
                     int Exists=0;
                     for (int x = 0; x < counter;x++ )
                     {
                         OutputClassLabels cl =(OutputClassLabels)  FinalObjectsClassLabels[x];
                         if (cl.classLabel == Category)
                         {
                              cl.OcxFc += (Int32.Parse(Oc) * Int32.Parse(Fc));//
                              cl.FcSum += Int32.Parse(Fc);
                              cl.OcSum += Int32.Parse(Oc); 
                              cl.counter += 1;
                              cl.W += Int32.Parse(_w);
                              Exists++;
                              FinalObjectsClassLabels[x] = cl;
                         }
                     }
                     if (Exists == 0)
                     {
                         OutputClassLabels lbl = new OutputClassLabels();
                         lbl.classLabel = Category;
                         lbl.OcxFc = (Int32.Parse(Oc) * Int32.Parse(Fc));
                         lbl.FcSum += Int32.Parse(Fc);
                         lbl.OcSum += Int32.Parse(Oc);
                         lbl.counter = 1;
                         FinalObjectsClassLabels.Add(lbl);
                         
                     }
                      
                 }
                 
                 
                 this.Invoke((MethodInvoker)delegate()
                 {
                       ChoosenCriteria = ddlCriteris.Text;
                 });


                 double res = 0;
                 string finalClassLabel = "";
                 string MyValues = "";
                 foreach (OutputClassLabels foc in FinalObjectsClassLabels)
                 {
                     MyValues += foc.classLabel;
                     MyValues += ",";
                     MyValues += foc.counter.ToString();
                     MyValues += ",";
                     MyValues += foc.FcSum.ToString();
                     MyValues += ",";
                     MyValues += foc.OcSum.ToString();
                     MyValues += ",";
                     MyValues += (((Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.OcSum.ToString())) / Double.Parse(foc.counter.ToString()))).ToString();
                     if (ChoosenCriteria == "fo/f+o/c")
                     {
                         if (res < ((((Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.OcSum.ToString())) / (Double.Parse(foc.FcSum.ToString()) + Double.Parse(foc.OcSum.ToString())))/Double.Parse(foc.counter.ToString()))))
                         {
                             res = ((((Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.OcSum.ToString())) / (Double.Parse(foc.FcSum.ToString()) + Double.Parse(foc.OcSum.ToString())))/Double.Parse(foc.counter.ToString())));
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "featuresXcounter")
                     {
                         if (res < Double.Parse(foc.FcSum.ToString())* Double.Parse(foc.counter.ToString()))
                         {
                             res = Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.counter.ToString());
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "MaxFcXOc")
                     {
                         if (res < Double.Parse(foc.OcxFc.ToString()))
                         {
                             res = foc.OcxFc;
                             finalClassLabel = foc.classLabel;
                         }
                     }

                     if (ChoosenCriteria == "MaxFeatures")
                     {
                         if (res < Double.Parse(foc.FcSum.ToString())) 
                         {
                             res = foc.FcSum;
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "MaxObjects")
                     {
                         if (res < Double.Parse(foc.OcSum.ToString())) 
                         {
                             res = foc.OcSum;
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "MaxCount")
                     {
                         if (res < Double.Parse(foc.counter.ToString())) 
                         {
                             res = foc.OcSum;
                             finalClassLabel = foc.classLabel;
                         }
                     }

                     if (ChoosenCriteria == "MeanFcXOc")
                     {
                         if (res < ((Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.OcSum.ToString())) / Double.Parse(foc.counter.ToString())))
                         {
                             res = ((Double.Parse(foc.FcSum.ToString()) * Double.Parse(foc.OcSum.ToString())) / Double.Parse(foc.counter.ToString()));
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "FeaturesMean")
                     {
                         if (res < Double.Parse(foc.FcSum.ToString())/ Double.Parse(foc.counter.ToString()))
                         {
                             res = Double.Parse(foc.FcSum.ToString()) / Double.Parse(foc.counter.ToString());
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "ObjectsMean")
                     {
                         if (res < Double.Parse(foc.OcSum.ToString()) / Double.Parse(foc.counter.ToString()))
                         {
                             res = Double.Parse(foc.OcSum.ToString()) / Double.Parse(foc.counter.ToString());
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "Weight")
                     {
                         if (res < Double.Parse(foc.W.ToString()) )
                         {
                             res = Double.Parse(foc.W.ToString());
                             finalClassLabel = foc.classLabel;
                         }
                     }
                     if (ChoosenCriteria == "MaxNumberOfRules")
                     {
                         int cur=0;
                          
                         foreach (DictionaryEntry dic in htclass)
                         {
                             if (Int32.Parse(dic.Value.ToString()) > cur)
                             {
                                 cur = Int32.Parse(dic.Value.ToString());
                                 finalClassLabel = dic.Key.ToString();
                             }
                         }
                     }
                     MyValues += ",";
                 }
                 //if (finalClassLabel.Trim()!= dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count-1].Value.ToString().Trim())
                 //{
                 //    faults++;
                 //}

                 SetTxtText(Environment.NewLine + i.ToString() + "," + finalClassLabel+","+MyValues);
                 FinalObjectsClassLabels = new ArrayList();
                 finalClassLabel = "";
             }
              

         }
         public string GetAttrVal(string Attr, string myxmlStr)
         {
             string str1 = myxmlStr.Substring(myxmlStr.IndexOf(Attr) + Attr.Length + 2);
             string val = str1.Substring(0, str1.IndexOf("\""));
             return val;
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
                    string it=featureName+"="+featureVal;
                    int featureIndex = 0;
                    foreach (FeaturesCharacteristics fc in TrainingSetMetaData)
                    {
                        if (featureName.Replace("_", "") == fc.FeatureName.Replace("_", ""))
                        {
                            int ValueIndex = 0;
                            if (fc.isContinuous == true)
                            //if (1==2)
                            {
                                foreach (string val in fc.FeatureValues)
                                {
                                    
                                    
                                    
                                    
                                    ////////////////////////////////////////////
                                    // Descritization standard  ie. each period must be written with two values as the following ranges for Age 
                                    //<=30
                                    //>30 && <=35
                                    //>35 && <=40
                                    //>40 && <=45
                                    //>45 && <=50
                                    //>50 && <=55
                                    //>55
                                    string[] ValIntervals = val.Replace("&&","&").Split('&');
                                    if (ValIntervals.Length > 1)// if our value betwwen two values
                                    {
                                        if (decimal.Parse(featureVal) >= decimal.Parse(ValIntervals[0].Replace("=", "").Replace(">", "")) && decimal.Parse(featureVal) < decimal.Parse(ValIntervals[1].Replace("=", "").Replace("<", "")))
                                        {
                                            tmp.Add(featureIndex.ToString() + "," + ValueIndex.ToString());
                                           
                                            break;
                                        }
                                    }
                                    else // if our value 
                                    {
                                        bool isLth = ValIntervals[0].Contains("<");
                                        if (isLth)
                                        {
                                           string interValValue=  ValIntervals[0].Replace("<", "");
                                           if (decimal.Parse(featureVal) < decimal.Parse(interValValue))
                                           {
                                               tmp.Add(featureIndex.ToString() + "," + ValueIndex.ToString());
                                              
                                               break;
                                           }
                                        }
                                        else
                                        {
                                            string interValValue = ValIntervals[0].Replace(">", "").Replace("=","");
                                            if (decimal.Parse(featureVal) >= decimal.Parse(interValValue))
                                            {
                                                tmp.Add(featureIndex.ToString() + "," + ValueIndex.ToString());
                                                
                                                break;
                                            }
                                        }
                                    }
                                    ///////////////////////////
                                    ValueIndex++;
                                    
                                }
                            }
                            else
                            {
                                foreach (string val in fc.FeatureValues)
                                {
                                    if (featureVal.Contains(val))
                                    {
                                        tmp.Add(featureIndex.ToString()+"," + ValueIndex.ToString());
                                        
                                        break;
                                    }
                                    ValueIndex++;
                                }
                            }
                            break;
                        }
                        featureIndex++;

                        //if (tmp.Count == j)
                        //{
                        //    int lkhkl = 0;
                        //}
                        //else {
                        //    int lkhkl = 0;
                        //}

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

        private void MyClassifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
            this.Dispose();
        }
    }
}
