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
            this.btnFirstReg = new System.Windows.Forms.Button();
            this.btnPrevReg = new System.Windows.Forms.Button();
            this.btnNextReg = new System.Windows.Forms.Button();
            this.btnLastReg = new System.Windows.Forms.Button();
            this.txtCurrentReg = new System.Windows.Forms.TextBox();
            this.txtTotalRegs = new System.Windows.Forms.TextBox();
            this.tblRegNumbers = new System.Windows.Forms.TableLayoutPanel();
            this.pnlReg = new CsvView.DoubleBufferPanel();
            this.pnlData = new CsvView.DoubleBufferPanel();
            this.tblRegNumbers.SuspendLayout();
            this.pnlReg.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(12, 14);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(457, 20);
            this.txtPath.TabIndex = 0;
            this.txtPath.DoubleClick += new System.EventHandler(this.txtPath_DoubleClick);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(475, 11);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(45, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnFirstReg
            // 
            this.btnFirstReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstReg.Location = new System.Drawing.Point(0, 0);
            this.btnFirstReg.Name = "btnFirstReg";
            this.btnFirstReg.Size = new System.Drawing.Size(36, 30);
            this.btnFirstReg.TabIndex = 4;
            this.btnFirstReg.Text = "|<";
            this.btnFirstReg.UseVisualStyleBackColor = true;
            this.btnFirstReg.Click += new System.EventHandler(this.btnFirstReg_Click);
            // 
            // btnPrevReg
            // 
            this.btnPrevReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevReg.Location = new System.Drawing.Point(42, 0);
            this.btnPrevReg.Name = "btnPrevReg";
            this.btnPrevReg.Size = new System.Drawing.Size(36, 30);
            this.btnPrevReg.TabIndex = 5;
            this.btnPrevReg.Text = "<";
            this.btnPrevReg.UseVisualStyleBackColor = true;
            this.btnPrevReg.Click += new System.EventHandler(this.btnPrevReg_Click);
            // 
            // btnNextReg
            // 
            this.btnNextReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextReg.Location = new System.Drawing.Point(430, 0);
            this.btnNextReg.Name = "btnNextReg";
            this.btnNextReg.Size = new System.Drawing.Size(36, 30);
            this.btnNextReg.TabIndex = 6;
            this.btnNextReg.Text = ">";
            this.btnNextReg.UseVisualStyleBackColor = true;
            this.btnNextReg.Click += new System.EventHandler(this.btnNextReg_Click);
            // 
            // btnLastReg
            // 
            this.btnLastReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLastReg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastReg.Location = new System.Drawing.Point(472, 0);
            this.btnLastReg.Name = "btnLastReg";
            this.btnLastReg.Size = new System.Drawing.Size(36, 30);
            this.btnLastReg.TabIndex = 7;
            this.btnLastReg.Text = ">|";
            this.btnLastReg.UseVisualStyleBackColor = true;
            this.btnLastReg.Click += new System.EventHandler(this.btnLastReg_Click);
            // 
            // txtCurrentReg
            // 
            this.txtCurrentReg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentReg.Location = new System.Drawing.Point(3, 3);
            this.txtCurrentReg.Name = "txtCurrentReg";
            this.txtCurrentReg.Size = new System.Drawing.Size(164, 20);
            this.txtCurrentReg.TabIndex = 8;
            this.txtCurrentReg.TextChanged += new System.EventHandler(this.txtCurrentReg_TextChanged);
            // 
            // txtTotalRegs
            // 
            this.txtTotalRegs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalRegs.Location = new System.Drawing.Point(173, 3);
            this.txtTotalRegs.Name = "txtTotalRegs";
            this.txtTotalRegs.ReadOnly = true;
            this.txtTotalRegs.Size = new System.Drawing.Size(164, 20);
            this.txtTotalRegs.TabIndex = 9;
            // 
            // tblRegNumbers
            // 
            this.tblRegNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblRegNumbers.ColumnCount = 2;
            this.tblRegNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.Controls.Add(this.txtTotalRegs, 1, 0);
            this.tblRegNumbers.Controls.Add(this.txtCurrentReg, 0, 0);
            this.tblRegNumbers.Location = new System.Drawing.Point(84, 0);
            this.tblRegNumbers.Name = "tblRegNumbers";
            this.tblRegNumbers.RowCount = 1;
            this.tblRegNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRegNumbers.Size = new System.Drawing.Size(340, 30);
            this.tblRegNumbers.TabIndex = 10;
            // 
            // pnlReg
            // 
            this.pnlReg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReg.Controls.Add(this.pnlData);
            this.pnlReg.Controls.Add(this.btnPrevReg);
            this.pnlReg.Controls.Add(this.tblRegNumbers);
            this.pnlReg.Controls.Add(this.btnFirstReg);
            this.pnlReg.Controls.Add(this.btnLastReg);
            this.pnlReg.Controls.Add(this.btnNextReg);
            this.pnlReg.Enabled = false;
            this.pnlReg.Location = new System.Drawing.Point(12, 40);
            this.pnlReg.Name = "pnlReg";
            this.pnlReg.Size = new System.Drawing.Size(508, 469);
            this.pnlReg.TabIndex = 11;
            // 
            // pnlData
            // 
            this.pnlData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlData.AutoScroll = true;
            this.pnlData.Location = new System.Drawing.Point(0, 37);
            this.pnlData.DisableAutoScroll = true;
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(508, 432);
            this.pnlData.TabIndex = 11;
            // 
            // FrmCsvViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 521);
            this.Controls.Add(this.pnlReg);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtPath);
            this.Name = "FrmCsvViewer";
            this.Text = "CsvViewer";
            this.tblRegNumbers.ResumeLayout(false);
            this.tblRegNumbers.PerformLayout();
            this.pnlReg.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

