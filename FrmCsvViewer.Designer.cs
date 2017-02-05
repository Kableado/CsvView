namespace CsvView
{
    partial class FrmCsvViewer
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
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.grpFile = new System.Windows.Forms.GroupBox();
            this.pnlScrollData = new CsvView.DoubleBufferPanel();
            this.pnlData = new CsvView.DoubleBufferPanel();
            this.pnlReg = new CsvView.DoubleBufferPanel();
            this.btnPrevReg = new System.Windows.Forms.Button();
            this.tblRegNumbers = new System.Windows.Forms.TableLayoutPanel();
            this.txtCurrentReg = new System.Windows.Forms.TextBox();
            this.txtTotalRegs = new System.Windows.Forms.TextBox();
            this.btnFirstReg = new System.Windows.Forms.Button();
            this.btnLastReg = new System.Windows.Forms.Button();
            this.btnNextReg = new System.Windows.Forms.Button();
            this.grpFile.SuspendLayout();
            this.pnlScrollData.SuspendLayout();
            this.pnlReg.SuspendLayout();
            this.tblRegNumbers.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(7, 27);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(676, 26);
            this.txtPath.TabIndex = 0;
            this.txtPath.DoubleClick += new System.EventHandler(this.txtPath_DoubleClick);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(691, 27);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(68, 26);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // grpFile
            // 
            this.grpFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFile.Controls.Add(this.btnLoad);
            this.grpFile.Controls.Add(this.txtPath);
            this.grpFile.Location = new System.Drawing.Point(20, 12);
            this.grpFile.Name = "grpFile";
            this.grpFile.Size = new System.Drawing.Size(766, 67);
            this.grpFile.TabIndex = 12;
            this.grpFile.TabStop = false;
            this.grpFile.Text = "File";
            // 
            // pnlScrollData
            // 
            this.pnlScrollData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlScrollData.AutoScroll = true;
            this.pnlScrollData.Controls.Add(this.pnlData);
            this.pnlScrollData.DisableAutoScroll = true;
            this.pnlScrollData.Location = new System.Drawing.Point(20, 126);
            this.pnlScrollData.Name = "pnlScrollData";
            this.pnlScrollData.Size = new System.Drawing.Size(766, 664);
            this.pnlScrollData.TabIndex = 12;
            // 
            // pnlData
            // 
            this.pnlData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlData.BackColor = System.Drawing.SystemColors.Control;
            this.pnlData.DisableAutoScroll = true;
            this.pnlData.Location = new System.Drawing.Point(0, 0);
            this.pnlData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(766, 80);
            this.pnlData.TabIndex = 11;
            // 
            // pnlReg
            // 
            this.pnlReg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReg.BackColor = System.Drawing.SystemColors.Control;
            this.pnlReg.Controls.Add(this.btnPrevReg);
            this.pnlReg.Controls.Add(this.tblRegNumbers);
            this.pnlReg.Controls.Add(this.btnFirstReg);
            this.pnlReg.Controls.Add(this.btnLastReg);
            this.pnlReg.Controls.Add(this.btnNextReg);
            this.pnlReg.Enabled = false;
            this.pnlReg.Location = new System.Drawing.Point(21, 87);
            this.pnlReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlReg.Name = "pnlReg";
            this.pnlReg.Size = new System.Drawing.Size(765, 701);
            this.pnlReg.TabIndex = 11;
            // 
            // btnPrevReg
            // 
            this.btnPrevReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevReg.Location = new System.Drawing.Point(62, 0);
            this.btnPrevReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrevReg.Name = "btnPrevReg";
            this.btnPrevReg.Size = new System.Drawing.Size(54, 31);
            this.btnPrevReg.TabIndex = 5;
            this.btnPrevReg.Text = "<";
            this.btnPrevReg.UseVisualStyleBackColor = true;
            this.btnPrevReg.Click += new System.EventHandler(this.btnPrevReg_Click);
            // 
            // tblRegNumbers
            // 
            this.tblRegNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblRegNumbers.ColumnCount = 2;
            this.tblRegNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.Controls.Add(this.txtCurrentReg, 0, 0);
            this.tblRegNumbers.Controls.Add(this.txtTotalRegs, 1, 0);
            this.tblRegNumbers.Location = new System.Drawing.Point(124, 0);
            this.tblRegNumbers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tblRegNumbers.Name = "tblRegNumbers";
            this.tblRegNumbers.RowCount = 1;
            this.tblRegNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.Size = new System.Drawing.Size(517, 31);
            this.tblRegNumbers.TabIndex = 10;
            // 
            // txtCurrentReg
            // 
            this.txtCurrentReg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentReg.Location = new System.Drawing.Point(4, 5);
            this.txtCurrentReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCurrentReg.Name = "txtCurrentReg";
            this.txtCurrentReg.Size = new System.Drawing.Size(250, 26);
            this.txtCurrentReg.TabIndex = 8;
            this.txtCurrentReg.TextChanged += new System.EventHandler(this.txtCurrentReg_TextChanged);
            // 
            // txtTotalRegs
            // 
            this.txtTotalRegs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalRegs.Location = new System.Drawing.Point(262, 5);
            this.txtTotalRegs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalRegs.Name = "txtTotalRegs";
            this.txtTotalRegs.ReadOnly = true;
            this.txtTotalRegs.Size = new System.Drawing.Size(251, 26);
            this.txtTotalRegs.TabIndex = 9;
            // 
            // btnFirstReg
            // 
            this.btnFirstReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstReg.Location = new System.Drawing.Point(0, 0);
            this.btnFirstReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFirstReg.Name = "btnFirstReg";
            this.btnFirstReg.Size = new System.Drawing.Size(54, 31);
            this.btnFirstReg.TabIndex = 4;
            this.btnFirstReg.Text = "|<";
            this.btnFirstReg.UseVisualStyleBackColor = true;
            this.btnFirstReg.Click += new System.EventHandler(this.btnFirstReg_Click);
            // 
            // btnLastReg
            // 
            this.btnLastReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLastReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastReg.Location = new System.Drawing.Point(711, 0);
            this.btnLastReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLastReg.Name = "btnLastReg";
            this.btnLastReg.Size = new System.Drawing.Size(54, 31);
            this.btnLastReg.TabIndex = 7;
            this.btnLastReg.Text = ">|";
            this.btnLastReg.UseVisualStyleBackColor = true;
            this.btnLastReg.Click += new System.EventHandler(this.btnLastReg_Click);
            // 
            // btnNextReg
            // 
            this.btnNextReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextReg.Location = new System.Drawing.Point(649, 0);
            this.btnNextReg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNextReg.Name = "btnNextReg";
            this.btnNextReg.Size = new System.Drawing.Size(54, 31);
            this.btnNextReg.TabIndex = 6;
            this.btnNextReg.Text = ">";
            this.btnNextReg.UseVisualStyleBackColor = true;
            this.btnNextReg.Click += new System.EventHandler(this.btnNextReg_Click);
            // 
            // FrmCsvViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 802);
            this.Controls.Add(this.pnlScrollData);
            this.Controls.Add(this.grpFile);
            this.Controls.Add(this.pnlReg);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmCsvViewer";
            this.Text = "CsvViewer";
            this.grpFile.ResumeLayout(false);
            this.grpFile.PerformLayout();
            this.pnlScrollData.ResumeLayout(false);
            this.pnlReg.ResumeLayout(false);
            this.tblRegNumbers.ResumeLayout(false);
            this.tblRegNumbers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnFirstReg;
        private System.Windows.Forms.Button btnPrevReg;
        private System.Windows.Forms.Button btnNextReg;
        private System.Windows.Forms.Button btnLastReg;
        private System.Windows.Forms.TextBox txtCurrentReg;
        private System.Windows.Forms.TextBox txtTotalRegs;
        private System.Windows.Forms.TableLayoutPanel tblRegNumbers;
        private CsvView.DoubleBufferPanel pnlReg;
        private CsvView.DoubleBufferPanel pnlData;
        private System.Windows.Forms.GroupBox grpFile;
        private DoubleBufferPanel pnlScrollData;
    }
}

