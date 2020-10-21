namespace PWRSet
{
    partial class PNU
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
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtres = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesOperationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.multiTextSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortingTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nCrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dClassifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLDatasetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSV2XMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChkSkipSubsumptionCheck = new System.Windows.Forms.CheckBox();
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
            this.lblRemainingTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblnrOfObjects = new System.Windows.Forms.Label();
            this.lblRt = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalUnique = new System.Windows.Forms.Label();
            this.outputAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
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
            this.btnStart.Text = "pw";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Start_Click);
            // 
            // txtres
            // 
            this.txtres.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtres.Location = new System.Drawing.Point(13, 194);
            this.txtres.Multiline = true;
            this.txtres.Name = "txtres";
            this.txtres.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtres.Size = new System.Drawing.Size(771, 312);
            this.txtres.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(141, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Validate No repeatation";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(298, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 23);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse (CSV)";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(13, 39);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(278, 20);
            this.txtFilePath.TabIndex = 15;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesOperationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(796, 24);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesOperationsToolStripMenuItem
            // 
            this.filesOperationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareToolStripMenuItem1,
            this.multiTextSearchToolStripMenuItem,
            this.classifierToolStripMenuItem,
            this.sortingTreeToolStripMenuItem,
            this.nCrToolStripMenuItem,
            this.dClassifierToolStripMenuItem,
            this.xMLDatasetsToolStripMenuItem,
            this.cSV2XMLToolStripMenuItem,
            this.outputAnalysisToolStripMenuItem});
            this.filesOperationsToolStripMenuItem.Name = "filesOperationsToolStripMenuItem";
            this.filesOperationsToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.filesOperationsToolStripMenuItem.Text = "Files Operations";
            // 
            // compareToolStripMenuItem1
            // 
            this.compareToolStripMenuItem1.Name = "compareToolStripMenuItem1";
            this.compareToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.compareToolStripMenuItem1.Text = "Compare";
            this.compareToolStripMenuItem1.Click += new System.EventHandler(this.compareToolStripMenuItem1_Click);
            // 
            // multiTextSearchToolStripMenuItem
            // 
            this.multiTextSearchToolStripMenuItem.Name = "multiTextSearchToolStripMenuItem";
            this.multiTextSearchToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.multiTextSearchToolStripMenuItem.Text = "MultiTextSearch";
            this.multiTextSearchToolStripMenuItem.Click += new System.EventHandler(this.multiTextSearchToolStripMenuItem_Click);
            // 
            // classifierToolStripMenuItem
            // 
            this.classifierToolStripMenuItem.Name = "classifierToolStripMenuItem";
            this.classifierToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.classifierToolStripMenuItem.Text = "Classifier";
            this.classifierToolStripMenuItem.Click += new System.EventHandler(this.classifierToolStripMenuItem_Click);
            // 
            // sortingTreeToolStripMenuItem
            // 
            this.sortingTreeToolStripMenuItem.Name = "sortingTreeToolStripMenuItem";
            this.sortingTreeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sortingTreeToolStripMenuItem.Text = "Sorting Tree";
            this.sortingTreeToolStripMenuItem.Click += new System.EventHandler(this.sortingTreeToolStripMenuItem_Click);
            // 
            // nCrToolStripMenuItem
            // 
            this.nCrToolStripMenuItem.Name = "nCrToolStripMenuItem";
            this.nCrToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.nCrToolStripMenuItem.Text = "nCr";
            this.nCrToolStripMenuItem.Click += new System.EventHandler(this.nCrToolStripMenuItem_Click);
            // 
            // dClassifierToolStripMenuItem
            // 
            this.dClassifierToolStripMenuItem.Name = "dClassifierToolStripMenuItem";
            this.dClassifierToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.dClassifierToolStripMenuItem.Text = "DClassifier";
            this.dClassifierToolStripMenuItem.Click += new System.EventHandler(this.dClassifierToolStripMenuItem_Click);
            // 
            // xMLDatasetsToolStripMenuItem
            // 
            this.xMLDatasetsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.xMLDatasetsToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.xMLDatasetsToolStripMenuItem.Name = "xMLDatasetsToolStripMenuItem";
            this.xMLDatasetsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.xMLDatasetsToolStripMenuItem.Text = "XML Datasets";
            this.xMLDatasetsToolStripMenuItem.Click += new System.EventHandler(this.xMLDatasetsToolStripMenuItem_Click);
            // 
            // cSV2XMLToolStripMenuItem
            // 
            this.cSV2XMLToolStripMenuItem.Name = "cSV2XMLToolStripMenuItem";
            this.cSV2XMLToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cSV2XMLToolStripMenuItem.Text = "CSV2XML";
            this.cSV2XMLToolStripMenuItem.Click += new System.EventHandler(this.cSV2XMLToolStripMenuItem_Click);
            // 
            // ChkSkipSubsumptionCheck
            // 
            this.ChkSkipSubsumptionCheck.AutoSize = true;
            this.ChkSkipSubsumptionCheck.Location = new System.Drawing.Point(394, 41);
            this.ChkSkipSubsumptionCheck.Name = "ChkSkipSubsumptionCheck";
            this.ChkSkipSubsumptionCheck.Size = new System.Drawing.Size(145, 17);
            this.ChkSkipSubsumptionCheck.TabIndex = 21;
            this.ChkSkipSubsumptionCheck.Text = "Skip Subsumption Check";
            this.ChkSkipSubsumptionCheck.UseVisualStyleBackColor = true;
            // 
            // chkShowCurrentLevel
            // 
            this.chkShowCurrentLevel.AutoSize = true;
            this.chkShowCurrentLevel.Checked = true;
            this.chkShowCurrentLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCurrentLevel.Location = new System.Drawing.Point(545, 42);
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
            this.lblTime.Location = new System.Drawing.Point(109, 140);
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
            this.label1.Location = new System.Drawing.Point(14, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Elapsed Time:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(512, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Nonesense:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(501, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "PartialReduct:";
            // 
            // lblnoneSense
            // 
            this.lblnoneSense.AutoSize = true;
            this.lblnoneSense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnoneSense.ForeColor = System.Drawing.Color.Red;
            this.lblnoneSense.Location = new System.Drawing.Point(598, 90);
            this.lblnoneSense.Name = "lblnoneSense";
            this.lblnoneSense.Size = new System.Drawing.Size(0, 20);
            this.lblnoneSense.TabIndex = 32;
            // 
            // lblPartialreduct
            // 
            this.lblPartialreduct.AutoSize = true;
            this.lblPartialreduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartialreduct.ForeColor = System.Drawing.Color.Green;
            this.lblPartialreduct.Location = new System.Drawing.Point(598, 114);
            this.lblPartialreduct.Name = "lblPartialreduct";
            this.lblPartialreduct.Size = new System.Drawing.Size(0, 20);
            this.lblPartialreduct.TabIndex = 33;
            // 
            // lblTotalNotUnique
            // 
            this.lblTotalNotUnique.AutoSize = true;
            this.lblTotalNotUnique.Location = new System.Drawing.Point(485, 161);
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
            this.lblTotalNotUniqueOutput.Location = new System.Drawing.Point(598, 161);
            this.lblTotalNotUniqueOutput.Name = "lblTotalNotUniqueOutput";
            this.lblTotalNotUniqueOutput.Size = new System.Drawing.Size(0, 17);
            this.lblTotalNotUniqueOutput.TabIndex = 35;
            // 
            // lbltotalNotuniqueWillBeCheckedOutPut
            // 
            this.lbltotalNotuniqueWillBeCheckedOutPut.AutoSize = true;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbltotalNotuniqueWillBeCheckedOutPut.ForeColor = System.Drawing.Color.Purple;
            this.lbltotalNotuniqueWillBeCheckedOutPut.Location = new System.Drawing.Point(598, 139);
            this.lbltotalNotuniqueWillBeCheckedOutPut.Name = "lbltotalNotuniqueWillBeCheckedOutPut";
            this.lbltotalNotuniqueWillBeCheckedOutPut.Size = new System.Drawing.Size(0, 17);
            this.lbltotalNotuniqueWillBeCheckedOutPut.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(411, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Total Not Unique will be checked:";
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.AutoSize = true;
            this.lblRemainingTime.Location = new System.Drawing.Point(2, 163);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(86, 13);
            this.lblRemainingTime.TabIndex = 38;
            this.lblRemainingTime.Text = "Remaining Time:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(482, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Searched Objects:";
            // 
            // lblnrOfObjects
            // 
            this.lblnrOfObjects.AutoSize = true;
            this.lblnrOfObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnrOfObjects.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblnrOfObjects.Location = new System.Drawing.Point(598, 68);
            this.lblnrOfObjects.Name = "lblnrOfObjects";
            this.lblnrOfObjects.Size = new System.Drawing.Size(0, 18);
            this.lblnrOfObjects.TabIndex = 41;
            this.lblnrOfObjects.Tag = "0";
            // 
            // lblRt
            // 
            this.lblRt.AutoSize = true;
            this.lblRt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRt.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblRt.Location = new System.Drawing.Point(110, 166);
            this.lblRt.Name = "lblRt";
            this.lblRt.Size = new System.Drawing.Size(16, 18);
            this.lblRt.TabIndex = 42;
            this.lblRt.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(505, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Total Unique:";
            // 
            // lblTotalUnique
            // 
            this.lblTotalUnique.AutoSize = true;
            this.lblTotalUnique.Location = new System.Drawing.Point(571, 178);
            this.lblTotalUnique.Name = "lblTotalUnique";
            this.lblTotalUnique.Size = new System.Drawing.Size(0, 13);
            this.lblTotalUnique.TabIndex = 44;
            // 
            // outputAnalysisToolStripMenuItem
            // 
            this.outputAnalysisToolStripMenuItem.Name = "outputAnalysisToolStripMenuItem";
            this.outputAnalysisToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.outputAnalysisToolStripMenuItem.Text = "Output-Analysis";
            this.outputAnalysisToolStripMenuItem.Click += new System.EventHandler(this.outputAnalysisToolStripMenuItem_Click);
            // 
            // PNU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 509);
            this.Controls.Add(this.lblTotalUnique);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblRt);
            this.Controls.Add(this.lblnrOfObjects);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblRemainingTime);
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
            this.Controls.Add(this.ChkSkipSubsumptionCheck);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtres);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PNU";
            this.Text = "PNU";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtres;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem filesOperationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem1;
        private System.Windows.Forms.CheckBox ChkSkipSubsumptionCheck;
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
        private System.Windows.Forms.Label lblRemainingTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblnrOfObjects;
        private System.Windows.Forms.Label lblRt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTotalUnique;
        private System.Windows.Forms.ToolStripMenuItem multiTextSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortingTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nCrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dClassifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLDatasetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cSV2XMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputAnalysisToolStripMenuItem;
    }
}

