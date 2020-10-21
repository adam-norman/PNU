namespace PWRSet
{
    partial class Compare2XMLfiles1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.txtBrowse1 = new System.Windows.Forms.TextBox();
            this.txtBrowse2 = new System.Windows.Forms.TextBox();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.txtRes = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.ckSwap = new System.Windows.Forms.CheckBox();
            this.lblRes = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSubsumptions = new System.Windows.Forms.Label();
            this.lblUnfound = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
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
            // txtBrowse2
            // 
            this.txtBrowse2.Location = new System.Drawing.Point(12, 41);
            this.txtBrowse2.Name = "txtBrowse2";
            this.txtBrowse2.Size = new System.Drawing.Size(416, 20);
            this.txtBrowse2.TabIndex = 3;
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(434, 41);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(123, 23);
            this.btnBrowse2.TabIndex = 2;
            this.btnBrowse2.Text = "Browse Second File..";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // txtRes
            // 
            this.txtRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRes.Location = new System.Drawing.Point(12, 106);
            this.txtRes.Multiline = true;
            this.txtRes.Name = "txtRes";
            this.txtRes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRes.Size = new System.Drawing.Size(873, 418);
            this.txtRes.TabIndex = 4;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(561, 12);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(77, 23);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "Compare";
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
            // ckSwap
            // 
            this.ckSwap.AutoSize = true;
            this.ckSwap.Location = new System.Drawing.Point(434, 74);
            this.ckSwap.Name = "ckSwap";
            this.ckSwap.Size = new System.Drawing.Size(53, 17);
            this.ckSwap.TabIndex = 6;
            this.ckSwap.Text = "Swap";
            this.ckSwap.UseVisualStyleBackColor = true;
            // 
            // lblRes
            // 
            this.lblRes.AutoSize = true;
            this.lblRes.Location = new System.Drawing.Point(715, 41);
            this.lblRes.Name = "lblRes";
            this.lblRes.Size = new System.Drawing.Size(10, 13);
            this.lblRes.TabIndex = 7;
            this.lblRes.Text = "-";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(635, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Total Searched";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(572, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "T. Subsumptions or Matched";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(640, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Total Unfound";
            // 
            // lblSubsumptions
            // 
            this.lblSubsumptions.AutoSize = true;
            this.lblSubsumptions.Location = new System.Drawing.Point(715, 65);
            this.lblSubsumptions.Name = "lblSubsumptions";
            this.lblSubsumptions.Size = new System.Drawing.Size(10, 13);
            this.lblSubsumptions.TabIndex = 11;
            this.lblSubsumptions.Text = "-";
            // 
            // lblUnfound
            // 
            this.lblUnfound.AutoSize = true;
            this.lblUnfound.Location = new System.Drawing.Point(715, 89);
            this.lblUnfound.Name = "lblUnfound";
            this.lblUnfound.Size = new System.Drawing.Size(10, 13);
            this.lblUnfound.TabIndex = 12;
            this.lblUnfound.Text = "-";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(645, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Search Multi-Text";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Compare2XMLfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 536);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblUnfound);
            this.Controls.Add(this.lblSubsumptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblRes);
            this.Controls.Add(this.ckSwap);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtRes);
            this.Controls.Add(this.txtBrowse2);
            this.Controls.Add(this.btnBrowse2);
            this.Controls.Add(this.txtBrowse1);
            this.Controls.Add(this.btnBrowse1);
            this.Name = "Compare2XMLfiles";
            this.Text = "Compare2XMLfiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Compare2XMLfiles_FormClosing);
            this.Load += new System.EventHandler(this.Compare2XMLfiles_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TextBox txtBrowse1;
        private System.Windows.Forms.TextBox txtBrowse2;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.TextBox txtRes;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.CheckBox ckSwap;
        private System.Windows.Forms.Label lblRes;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSubsumptions;
        private System.Windows.Forms.Label lblUnfound;
        private System.Windows.Forms.Button button1;
    }
}