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

namespace PWRSet
{
    public partial class Compare2XMLfiles1 : Form
    {
        public Compare2XMLfiles1()
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


        private void btnCompare_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
        }
         void Start()
         {
             int TotalUnfound = 0;
             int TotalSubsumptions = 0;

            string line;
            // First File Lists
            line = "";
            bool isFirst = true;
            ArrayList SubSet = new ArrayList();
            ArrayList FirstFileDs = new ArrayList();
            ArrayList FirstFileFs = new ArrayList();
            ArrayList FirstFileAsSubsets = new ArrayList();
            ArrayList FirstFileAsSubsetsClasses = new ArrayList();
            using (StreamReader reader = new StreamReader(txtBrowse1.Text ))
            {
                while ((line = reader.ReadLine()) != null )
                {
                   if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line)) 
                    {
                        string s = line.Replace("_", " ").Replace(" =", "=").Replace("'","\"");
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
            SubSet.Clear();
            // Second File Lists
            line = "";
            ArrayList SecondFileDs = new ArrayList();
            ArrayList SecondFileFs = new ArrayList();
            ArrayList SecondFileAsSubsets = new ArrayList();
            ArrayList SecondFileAsSubsetsClasses = new ArrayList();
            isFirst = true;
            using (StreamReader reader = new StreamReader(txtBrowse2.Text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line))
                    {
                        string s = line.Replace("_", " ").Replace(" =", "=").Replace("'", "\"");
                        line = s;
                        if (line.IndexOf("<Rule ") != -1)
                        {
                            SecondFileAsSubsetsClasses.Add(line);
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                SecondFileAsSubsets.Add((ArrayList)SubSet.Clone());
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
                            SecondFileAsSubsets.Add((ArrayList)SubSet.Clone());
                            SubSet.Clear();
                        }
                    }
                }
            }
          

            ArrayList SubsumtionsFromSecondFileOnItself = new ArrayList();
           
            int Mx1 = 0;
            foreach (ArrayList  item in FirstFileAsSubsets)
            {
                if (Mx1 < item.Count)
                {
                    Mx1 = item.Count;
                }
            }


            SetTxtText(Environment.NewLine + "Total Rules count In The First File =" + FirstFileAsSubsets.Count.ToString());
            SetTxtText(Environment.NewLine + "Max Rules Length In The First File =" + Mx1.ToString());
            Mx1 = 0;
            foreach (ArrayList item in SecondFileAsSubsets)
            {
                if (Mx1 < item.Count)
                {
                    Mx1 = item.Count;
                }
            }
            SetTxtText(Environment.NewLine + "Total Rules count In The Second File =" + SecondFileAsSubsets.Count.ToString());
            SetTxtText(Environment.NewLine + "Max Rules Length In The Second File =" + Mx1.ToString());
            if (!ckSwap.Checked)
            {
                TotalUnfound = 0;
                TotalSubsumptions = 0;
                for (int i = 0; i < FirstFileAsSubsets.Count; i++)
                {

                    SetText((i+1).ToString());
                    if (HasSubsumtions_Traditional((ArrayList)FirstFileAsSubsets[i], SecondFileAsSubsets))
                    {
                        TotalSubsumptions++;
                        SetTotSubsumptionsText(TotalSubsumptions.ToString());
                        SetTxtText(Environment.NewLine + FirstFileAsSubsetsClasses[i].ToString());
                        foreach (string item in (ArrayList)FirstFileAsSubsets[i])
                        {
                            SetTxtText(Environment.NewLine + item);
                        }
                        SetTxtText(Environment.NewLine + "</Rule>");
                    }
                    else
                    {
                        TotalUnfound++;
                        SetTotUnfoundText(TotalUnfound.ToString());
                        SetTxtText(Environment.NewLine + "Not Found Rule");
                        SetTxtText(Environment.NewLine + FirstFileAsSubsetsClasses[i].ToString());
                        foreach (string item in (ArrayList)FirstFileAsSubsets[i])
                        {
                            SetTxtText(Environment.NewLine + item);
                        }
                        SetTxtText(Environment.NewLine + "</Rule>");
                    }
                }
            }
            else
            {
                  TotalUnfound = 0;
                  TotalSubsumptions = 0;
                for (int i = 0; i < SecondFileAsSubsets.Count; i++)
                {
                    if (HasSubsumtions_Traditional((ArrayList)SecondFileAsSubsets[i], FirstFileAsSubsets))
                    {
                        TotalSubsumptions++;
                        SetTotSubsumptionsText(TotalSubsumptions.ToString());
                        SetTxtText(Environment.NewLine + "Subsumed Rule");
                        SetTxtText(Environment.NewLine + SecondFileAsSubsetsClasses[i].ToString());
                        foreach (string item in (ArrayList)SecondFileAsSubsets[i])
                        {
                            SetTxtText(Environment.NewLine + item);
                        }
                        SetTxtText(Environment.NewLine + "</Rule>");
                    }
                    else
                    {
                        TotalUnfound++;
                        SetTotUnfoundText(TotalUnfound.ToString());
                        SetTxtText(Environment.NewLine +"Not Found Rule");
                        SetTxtText(Environment.NewLine + SecondFileAsSubsetsClasses[i].ToString());
                        foreach (string item in (ArrayList)SecondFileAsSubsets[i])
                        {
                            SetTxtText(Environment.NewLine + item);
                        }
                        SetTxtText(Environment.NewLine + "</Rule>");
                    }
                }
            }

            SetTxtText(Environment.NewLine + ".........Finished..........");
        }
        private bool HasSubsumtions_Traditional(ArrayList pSubSet, ArrayList FullSets)
        {
            foreach (ArrayList lstUniqueSets in FullSets)
            {
                 ArrayList tmplst = new ArrayList();
                 foreach (string x in lstUniqueSets)
                {
                     
                        if (pSubSet.Contains(x))
                        {
                            tmplst.Add(x);
                        }


                        if (tmplst.Count == pSubSet.Count)
                    {
                        return true;
                    }


                }
            }
            return false;
        }
       
        //private BackgroundWorker backgroundWorker1;
       
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);


        }
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            txtBrowse1.Text = path;
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = string.Empty;
            openFileDialog2.ShowDialog();
            string path = openFileDialog2.FileName;
            txtBrowse2.Text = path;
        }

        private void Compare2XMLfiles_Load(object sender, EventArgs e)
        {

        }

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

        private void Compare2XMLfiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
            this.Dispose();

        }
    }
}
