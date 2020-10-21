using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using PWRSet;

namespace Pursuit_not_Super_Reduct_only_Algorithm
{
    public partial class MultiTextSearch : Form
    {
        public MultiTextSearch()
        {
            InitializeComponent();
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            txtBrowse1.Text = path;
            //if (!String.IsNullOrWhiteSpace(path) && !String.IsNullOrEmpty(path))
            //{ }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            txtRes.Text = "";
            string line;
            // First File Lists
            line = "";
            ArrayList strTags = new ArrayList();
            strTags.AddRange(txtTags.Text.Split(','));
            ArrayList strReqRule = new ArrayList();
            ArrayList SubSet = new ArrayList();
            ArrayList FirstFileDs = new ArrayList();
            ArrayList FirstFileFs = new ArrayList();
            ArrayList FirstFileAsSubsets = new ArrayList();
            ArrayList FirstFileAsSubsetsClasses = new ArrayList();
            
            using (StreamReader reader = new StreamReader(txtBrowse1.Text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line))
                    {
                        if (line.IndexOf("<Rule") != -1)
                        {
                            strReqRule.Add(line);
                        }
                        if (line.IndexOf("Tuple") > -1)
                        {
                            strReqRule.Add(line);

                        }
                        if (line.IndexOf("/Rule") > -1)
                        {
                            strReqRule.Add(line);
                            ArrayList t = new ArrayList();
                            t = (ArrayList)strTags.Clone();
                            foreach (string myTag in t)
                            {
                                foreach (string tuple in strReqRule)
                                {
                                    if (tuple.Contains(myTag))
                                    {
                                        strTags.Remove(myTag);
                                    }
                                }
                            }


                            if (strTags.Count == 0)
                            {

                                foreach (string tuple in strReqRule)
                                {
                                    txtRes.AppendText(Environment.NewLine + tuple);
                                }
                                txtRes.AppendText(Environment.NewLine + "</Rule>");
                            }
                            strReqRule.Clear();
                            strTags.Clear();
                            strTags.AddRange(txtTags.Text.Split(','));
                        }
                    }
                }
            }
            MessageBox.Show("Finished");
        }

        private void MultiTextSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
            this.Dispose();
        }

    }
}
