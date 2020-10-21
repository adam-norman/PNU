using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DClassifier
{
    public partial class ConvertDatasetsWithUnknownValuesToXML : Form
    {
        public ConvertDatasetsWithUnknownValuesToXML()
        {
            InitializeComponent();
        }
        string path = "";
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
              path = openFileDialog1.FileName;
            txtFilePath.Text = path;
            LoadDataFromCSV(path);
            MessageBox.Show("done");
            
        }
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
            string[] Features = new string[1]; ;
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<Root M=\"_Nr_Of_Cases_\" N=\"_Nr_Of_Features_\">" + Environment.NewLine + "<Inference>" + Environment.NewLine + "<Cluster>" + Environment.NewLine);
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        if (isHeader == true)
                        {
                            isHeader = false;
                              Features = line.Replace(" ", "_").Replace(";", ",").Replace("\t", ",").Split(',');
                        }
                        else
                        {
                            string[] strRow = line.ToLower().Replace(";", ",").Replace("\t", ",").Split(',');
                           // اضرب لوب على القيم
                            sbXML.Append("<Rule Category=\"" + strRow[Features.Length - 1] + "\">" + Environment.NewLine);
                            for (int i = 0; i <Features.Length - 1 ; i++)
                            {
                                sbXML.Append("<Tuple Cpt=\"Dummy\" Prop=\"" + Features[i] + "\" Val=\"" + strRow[i] + "\" />" + Environment.NewLine);
                            }
                            sbXML.Append("</Rule>" + Environment.NewLine);
                        }
                    }

                }
            }
            sbXML.Append("</Cluster>" + Environment.NewLine + "</Inference>" + Environment.NewLine + "</Root>");
            string rand = rn.Next(11111, 99999).ToString();
            path = path.Substring(0, path.LastIndexOf("\\")  );
            if (!File.Exists(path + "\\DatasetFile" + rand + ".XML"))
            {
                
                var myFile = File.Create(path + "\\DatasetFile" + rand + ".XML");
                myFile.Close();
            }

            StreamWriter asdf = new StreamWriter(path + "\\DatasetFile" + rand + ".XML");
            asdf.Write(sbXML.ToString());
            asdf.Close();
            sbXML.Clear();  
        }
        Random rn = new Random();
    }
}
