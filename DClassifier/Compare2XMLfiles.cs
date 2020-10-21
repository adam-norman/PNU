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
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using Pursuit_not_Super_Reduct_only_Algorithm;

namespace PWRSet
{
    public partial class Compare2XMLfiles : Form
    {
        public Compare2XMLfiles()
        {
            InitializeComponent();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Start();
        }
        private void btnCompare_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
        }
        void Start()
        {
            string line = "";
            int fc = 0;
            int catIndex = 0;
            ArrayList Categories = new ArrayList();
            Hashtable htCategories = new Hashtable();
            int maxLength = 0;
            using (StreamReader reader = new StreamReader(txtBrowse1.Text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line))
                    {
                        if (line.IndexOf("<Rule ") != -1)
                        {
                            if (Categories.Contains(GetAttrVal(line)))
                            {
                                catIndex = Categories.IndexOf(GetAttrVal(line));
                            }
                            else
                            {
                                Categories.Add(GetAttrVal(line));
                                catIndex = Categories.Count - 1;
                            }
                        }
                        if (line.IndexOf("<Tuple") > -1)
                        {
                            fc++;
                        }
                        if (line.IndexOf("</Rule>") > -1)
                        {
                            if (htCategories.ContainsKey(catIndex.ToString()))
                            {
                                string OuterKey = "";
                                string InnerKey = "";
                                int InnerValue = 0;
                                Hashtable htTemp = new Hashtable();
                                foreach (DictionaryEntry category in htCategories)
                                {
                                    if (category.Key.ToString() == catIndex.ToString())
                                    {
                                        OuterKey = catIndex.ToString();
                                        htTemp = (Hashtable)category.Value;
                                        Hashtable htLengths = (Hashtable)category.Value;
                                        if (htLengths.ContainsKey(fc.ToString()))
                                        {
                                            foreach (DictionaryEntry ind in (Hashtable)category.Value)
                                            {
                                                if (ind.Key.ToString() == fc.ToString())
                                                {
                                                    InnerValue = Int32.Parse(ind.Value.ToString()) + 1;
                                                    InnerKey = ind.Key.ToString();
                                                    htLengths = (Hashtable)category.Value;
                                                    break;
                                                }
                                            }
                                            htLengths[InnerKey.ToString()] = InnerValue;
                                            htCategories[OuterKey.ToString()] = htLengths;
                                            break;
                                        }
                                        else
                                        {
                                            htLengths.Add(fc.ToString(), 1);
                                            htCategories[OuterKey.ToString()] = htLengths;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Hashtable htLengths = new Hashtable();
                                htLengths.Add(fc.ToString(), 1);
                                htCategories.Add(catIndex.ToString(), htLengths);
                            }
                            if (maxLength < fc)
                            {
                                maxLength = fc;
                            }
                            fc = 0;
                            catIndex = 0;
                        }
                    }
                }
            }
            StringBuilder sbHtml = new StringBuilder();
            //header
            sbHtml.Append("<table><tr><td style=\"border-style: dotted; border-width: thin; \">Category/length</td>");
            for (int counter = 1; counter <= maxLength; counter++)
            {
                sbHtml.Append("<td style=\"border-style: dotted; border-width: thin; \">" + counter.ToString() + "</td>");
            }
            sbHtml.Append("</tr>");
            foreach (DictionaryEntry category in htCategories)
            {
                string Cat = GetAttrVal(Categories[Int32.Parse(category.Key.ToString())].ToString());
                sbHtml.Append("<tr><td style=\"border-style: dotted; border-width: thin; \">" + Cat + "</td>");

                for (int counter = 1; counter <= maxLength; counter++)
                {
                    if (((Hashtable)category.Value).ContainsKey(counter.ToString()))
                    {
                        foreach (DictionaryEntry ind in (Hashtable)category.Value)
                        {
                            if (ind.Key.ToString() == counter.ToString())
                            {
                                sbHtml.Append("<td style=\"border-style: dotted; border-width: thin; \">" + ind.Value.ToString() + "</td>");
                            }
                        }
                    }
                    else
                    {
                        sbHtml.Append("<td style=\"border-style: dotted; border-width: thin; \">0</td>");
                    }
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");
            webBrowser1.DocumentText = sbHtml.ToString();
        }
        public string GetAttrVal(string param)
        {
            string line = "<root>" + param.ToLower().Replace(">", "/>").Replace("//>", "/>") + "</root>";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(line); // suppose that myXmlString contains "<Names>...</Names>"
            string val = "";
            XmlNodeList xnList = xml.SelectNodes("/root/rule");
            bool x = false;
            foreach (XmlNode xn in xnList)
            {
                val = xn.Attributes["category"].Value;
                x = true;
                break;
            }
            if (x)
            {
                return val;
            }
            return param;
        }
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
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }

        
    }
   

}
