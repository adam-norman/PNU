using PWRSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DClassifier
{
    public partial class Functions : Form
    {
        public Functions()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PNU f = new PNU();
            this.Hide();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PNUXML f = new PNUXML();
            this.Hide();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sort f = new Sort();
            this.Hide();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            nCr f = new nCr();
            this.Hide();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Compare2XMLfiles1 f = new Compare2XMLfiles1();
            this.Hide();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Compare2XMLfiles f = new Compare2XMLfiles();
            this.Hide();
            f.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ConvertDatasetsWithUnknownValuesToXML f = new ConvertDatasetsWithUnknownValuesToXML();
            this.Hide();
            f.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MyDClassifier f = new MyDClassifier();
            this.Hide();
            f.Show();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            MyClassifier f = new MyClassifier();
            this.Hide();
            f.Show();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            Application.Exit();

        }
    }
}
