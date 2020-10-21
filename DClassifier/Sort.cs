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
//using PWRSet.CS;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
//using Pursuit_not_Super_Reduct_only_Algorithm;
using System.Windows.Forms.DataVisualization.Charting;
namespace PWRSet
{
    public partial class Sort : Form
    {
        public Sort()
        {
            InitializeComponent();
        }
        
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Start();
        }
         


        private void btnCompare_Click(object sender, EventArgs e)
        {
           // this.backgroundWorker1.RunWorkerAsync();
            //Start();
            LoadTreeView(tabControl1.SelectedIndex);
        }

        private TextBox txtStatisticalData;
        private Chart chart1;
        List<XMLApplicableFormat> MyApplicableXMLformat = new List<XMLApplicableFormat>();
      
            void Start()
         {
            
            
        }
            List<string> AnalyticalData = new List<string>();
            private void LoadXml()
            {
                //System.Net.WebUtility.HtmlEncode(Strvalue);
                //System.Net.WebUtility.HtmlDecode(Strvalue);
                string line;
                line = "";
                //bool isFirst = true;
                XMLApplicableFormat r = new XMLApplicableFormat();
                using (StreamReader reader = new StreamReader(txtBrowse1.Text))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!String.IsNullOrWhiteSpace(line) && !String.IsNullOrEmpty(line))
                        {
                            if (line.Contains("<Rule "))
                            {
                                string AttrFilter = "Fc";
                                string cd = line.Substring(line.IndexOf(AttrFilter) + AttrFilter.Length + 2);
                                string val1 = cd.Substring(0, cd.IndexOf("\"")  );
                                r.Fc = Int32.Parse(val1);

                                AttrFilter = "Oc";
                                cd = line.Substring(line.IndexOf(AttrFilter) + AttrFilter.Length + 2);
                                val1 = cd.Substring(0, cd.IndexOf("\"") );
                                r.Oc = Int32.Parse(val1);

                                //AttrFilter = "W";
                                //cd = line.Substring(line.IndexOf(AttrFilter) + AttrFilter.Length + 2);
                                //val1 = cd.Substring(0, cd.IndexOf("\"")  );
                                //r.W = Int32.Parse(val1);

                                r.Rule.Add(line);
                                AnalyticalData.Add(r.Fc.ToString() + "," + r.Oc.ToString() );
                            }
                            if (line.Contains("<Tuple "))
                            {
                                r.Rule.Add(line);
                            }
                            if (line.Contains("</Rule>"))
                            {
                                MyApplicableXMLformat.Add(r);
                                r = new XMLApplicableFormat();
                            }
                        }
                    }
                }
                foreach (string str in AnalyticalData)
                {
                    string[] arr = str.Split(',');

                    if (htFc.ContainsKey(Int32.Parse(arr[0])))
                    {
                        htFc[Int32.Parse(arr[0])] = (Int32.Parse(htFc[Int32.Parse(arr[0])].ToString()) + 1);
                    }
                    else
                    {
                        htFc.Add(Int32.Parse(arr[0]), 1);
                    }

                    if (htOc.ContainsKey(Int32.Parse(arr[1])))
                    {
                        htOc[Int32.Parse(arr[1])] = (Int32.Parse(htOc[Int32.Parse(arr[1])].ToString()) + 1);
                    }
                    else
                    {
                        htOc.Add(Int32.Parse(arr[1]), 1);
                    }



                    //if (htW.ContainsKey(Int32.Parse(arr[2])))
                    //{
                    //    htW[Int32.Parse(arr[2])] = (Int32.Parse(htW[Int32.Parse(arr[2])].ToString()) + 1);
                    //}
                    //else
                    //{
                    //    htW.Add(Int32.Parse(arr[2]), 1);
                    //}

                }
                
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("---FC---" + Environment.NewLine);
                foreach (var d in htFc)
                {
                    sb2.Append(d.Key.ToString() + "->" + d.Value.ToString() + Environment.NewLine);
                }
                //sb2.Append("---OC---" + Environment.NewLine);
                //foreach (DictionaryEntry d in htOc)
                //{
                //    sb2.Append(d.Key.ToString() + ":" + d.Value.ToString() + Environment.NewLine);
                //}
                //sb2.Append("---W---" + Environment.NewLine);
                //foreach (DictionaryEntry d in htW)
                //{
                //    sb2.Append(d.Key.ToString() + ":" + d.Value.ToString() + Environment.NewLine);
                //}
                txtStatisticalData.Text = sb2.ToString();

                //Draw it.

                chart1.Series.Clear();
                var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "TuplesCount",
                    Color = System.Drawing.Color.Green,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Line
                };

                this.chart1.Series.Add(series1);

               


                foreach (var d in htFc)
                {
                   series1.Points.AddXY(d.Key, d.Value);
                }


                chart1.Invalidate();
            }
            SortedDictionary<int, int> htFc = new SortedDictionary<int, int>();
            SortedDictionary<int, int> htOc = new SortedDictionary<int, int>();
            SortedDictionary<int, int> htW = new SortedDictionary<int, int>();
            
            
       
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);


        }
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
             openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();


            if (!String.IsNullOrWhiteSpace(openFileDialog1.FileName) && !String.IsNullOrEmpty(openFileDialog1.FileName))
            {
                string path = openFileDialog1.FileName;
                txtBrowse1.Text = path;
                LoadXml();
            }
        }

      
 

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            Start();
        }

        private void LoadTreeView(int tabIndex)
        {
            if (tabIndex == 0)
            {
                treeview_Fc.Nodes.Clear();
                TreeNode root = new TreeNode("Root");
                treeview_Fc.Nodes.Add(root);
                List<XMLApplicableFormat> SortedList;
                if (rbAsc.Checked)
                {
                    SortedList = MyApplicableXMLformat.OrderBy(o => o.Fc).ToList();
                }
                else
                {
                    SortedList = MyApplicableXMLformat.OrderByDescending(o => o.Fc).ToList();
                }
                int m = 0;
                for (int x = 0; x <SortedList.Count ;  x++)
                {
                    XMLApplicableFormat item = SortedList[x];
                    TreeNode _Rule = new TreeNode();
                    //TreeNode Tuples = new TreeNode();
                    for (int i = 0; i < item.Rule.Count; i++)
                    {
                        if (i == 0)
                        {
                            _Rule = new TreeNode(item.Fc.ToString() + ")-" + item.Rule[i]);
                        }
                        else
                        {
                            _Rule.Nodes.Add(item.Rule[i]);
                        }
                    }
                   // _Rule.Nodes.Add(Tuples);
                    m++;
                    // treeviewCasesNr.Nodes.Add(_Rule);
                    root.Nodes.Add(_Rule);
                    if (m > Int32.Parse(txtTop.Text))
                    {
                        break;
                    }
                }
            }
            if (tabIndex == 1)
            {
                treeview_Oc.Nodes.Clear();
                TreeNode root = new TreeNode("Root");
                treeview_Oc.Nodes.Add(root);
                List<XMLApplicableFormat> SortedList;
                if (rbAsc.Checked)
                {
                    SortedList = MyApplicableXMLformat.OrderBy(o => o.Oc).ToList();
                }
                else
                {
                    SortedList = MyApplicableXMLformat.OrderByDescending(o => o.Oc).ToList();
                }
                int m = 0;
                for (int x = 0; x < SortedList.Count; x++)
                {
                    XMLApplicableFormat item = SortedList[x];
                    TreeNode _Rule = new TreeNode();
                    TreeNode Tuples = new TreeNode();
                    for (int i = 0; i < item.Rule.Count; i++)
                    {
                        if (i == 0)
                        {
                            _Rule = new TreeNode(item.Oc.ToString() + ")-" + item.Rule[i]);
                        }
                        else
                        {
                            _Rule.Nodes.Add(item.Rule[i]);
                        }
                    }
                   // _Rule.Nodes.Add(Tuples);
                    m++;
                    // treeviewCasesNr.Nodes.Add(_Rule);
                    root.Nodes.Add(_Rule);
                    if (m > Int32.Parse(txtTop.Text))
                    {
                        break;
                    }
                }
            }
            if (tabIndex == 2)
            {
                treeview_W.Nodes.Clear();
                TreeNode root = new TreeNode("Root");
                treeview_W.Nodes.Add(root);
                List<XMLApplicableFormat> SortedList;
                if (rbAsc.Checked)
                {
                    SortedList = MyApplicableXMLformat.OrderBy(o => o.Oc).ToList();
                }
                else
                {
                    SortedList = MyApplicableXMLformat.OrderByDescending(o => o.Fc).ToList();
                }
                int m = 0;
                for (int x = 0; x < SortedList.Count; x++)
                {
                    XMLApplicableFormat item = SortedList[x];
                    TreeNode _Rule = new TreeNode();
                    //TreeNode Tuples = new TreeNode();
                    for (int i = 0; i < item.Rule.Count; i++)
                    {
                        if (i == 0)
                        {
                            _Rule = new TreeNode(item.Oc.ToString() + ")-" + item.Rule[i]);
                        }
                        else
                        {
                            _Rule.Nodes.Add(item.Rule[i]);
                        }
                    }
                   // _Rule.Nodes.Add(Tuples);
                    m++;
                    // treeviewCasesNr.Nodes.Add(_Rule);
                    root.Nodes.Add(_Rule);
                    if (m > Int32.Parse(txtTop.Text))
                    {
                        break;
                    }
                }
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
           
        }
        
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.txtBrowse1 = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeview_Fc = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeview_Oc = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeview_W = new System.Windows.Forms.TreeView();
            this.rbAsc = new System.Windows.Forms.RadioButton();
            this.rbDesc = new System.Windows.Forms.RadioButton();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatisticalData = new System.Windows.Forms.TextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(434, 12);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(123, 23);
            this.btnBrowse1.TabIndex = 0;
            this.btnBrowse1.Text = "Browse First File..";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // txtBrowse1
            // 
            this.txtBrowse1.Location = new System.Drawing.Point(12, 12);
            this.txtBrowse1.Name = "txtBrowse1";
            this.txtBrowse1.Size = new System.Drawing.Size(416, 20);
            this.txtBrowse1.TabIndex = 1;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(561, 12);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(77, 23);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "LoadTree";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(635, 367);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeview_Fc);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(627, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Nr Of Tuples";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeview_Fc
            // 
            this.treeview_Fc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeview_Fc.Location = new System.Drawing.Point(3, 3);
            this.treeview_Fc.Name = "treeview_Fc";
            this.treeview_Fc.Size = new System.Drawing.Size(621, 335);
            this.treeview_Fc.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeview_Oc);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(627, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nr Of Cases";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeview_Oc
            // 
            this.treeview_Oc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeview_Oc.Location = new System.Drawing.Point(3, 3);
            this.treeview_Oc.Name = "treeview_Oc";
            this.treeview_Oc.Size = new System.Drawing.Size(621, 335);
            this.treeview_Oc.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeview_W);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(627, 341);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Case Weight";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeview_W
            // 
            this.treeview_W.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeview_W.Location = new System.Drawing.Point(0, 0);
            this.treeview_W.Name = "treeview_W";
            this.treeview_W.Size = new System.Drawing.Size(627, 341);
            this.treeview_W.TabIndex = 0;
            // 
            // rbAsc
            // 
            this.rbAsc.AutoSize = true;
            this.rbAsc.Checked = true;
            this.rbAsc.Location = new System.Drawing.Point(645, 17);
            this.rbAsc.Name = "rbAsc";
            this.rbAsc.Size = new System.Drawing.Size(43, 17);
            this.rbAsc.TabIndex = 7;
            this.rbAsc.TabStop = true;
            this.rbAsc.Text = "Asc";
            this.rbAsc.UseVisualStyleBackColor = true;
            // 
            // rbDesc
            // 
            this.rbDesc.AutoSize = true;
            this.rbDesc.Location = new System.Drawing.Point(694, 18);
            this.rbDesc.Name = "rbDesc";
            this.rbDesc.Size = new System.Drawing.Size(50, 17);
            this.rbDesc.TabIndex = 8;
            this.rbDesc.TabStop = true;
            this.rbDesc.Text = "Desc";
            this.rbDesc.UseVisualStyleBackColor = true;
            // 
            // txtTop
            // 
            this.txtTop.Location = new System.Drawing.Point(782, 15);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(47, 20);
            this.txtTop.TabIndex = 9;
            this.txtTop.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(753, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Top";
            // 
            // txtStatisticalData
            // 
            this.txtStatisticalData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatisticalData.Location = new System.Drawing.Point(649, 63);
            this.txtStatisticalData.Multiline = true;
            this.txtStatisticalData.Name = "txtStatisticalData";
            this.txtStatisticalData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatisticalData.Size = new System.Drawing.Size(114, 338);
            this.txtStatisticalData.TabIndex = 11;
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(769, 66);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(411, 335);
            this.chart1.TabIndex = 12;
            this.chart1.Text = "chart1";
            // 
            // Sort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 413);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.txtStatisticalData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTop);
            this.Controls.Add(this.rbDesc);
            this.Controls.Add(this.rbAsc);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtBrowse1);
            this.Controls.Add(this.btnBrowse1);
            this.Name = "Sort";
            this.Text = "Compare2XMLfiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Sort_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TextBox txtBrowse1;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView treeview_Fc;
        private System.Windows.Forms.TreeView treeview_Oc;
        private System.Windows.Forms.TreeView treeview_W;
        private System.Windows.Forms.RadioButton rbAsc;
        private System.Windows.Forms.RadioButton rbDesc;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.Label label1;

        private void Sort_FormClosing(object sender, FormClosingEventArgs e)
        {
            PNU p = new PNU();
            p.Activate();
            p.Show();
            this.Dispose();
        }

        
        
       
        
    }
    public class XMLApplicableFormat
    { 
        //public double  W=0;
        public int Oc;
        public int Fc = 0;
        public  List<string> Rule = new List<string>();
    }
}
