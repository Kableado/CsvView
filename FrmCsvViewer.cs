using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CsvView
{
    public partial class FrmCsvViewer : Form
    {
        public FrmCsvViewer()
        {
            InitializeComponent();
        }

        private void txtPath_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            loadDialog.DefaultExt = "csv";
            loadDialog.Filter = "CSV Files|*.csv|Any File|*";
            DialogResult result = loadDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPath.Text = loadDialog.FileName;
            }
        }

        private string _loadedFile = string.Empty;
        private long _currentReg = 0;
        private long _totalRegs = 0;
        private List<long> _index = null;

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPath.Text) == false)
            {
                RenderRegClean();
                _loadedFile = null;
                _totalRegs = 0;
                _index = null;
                MessageBoxEx.Show(this, "FileNotFound");
                return;
            }

            LoadFile(txtPath.Text);
        }

        public void LoadFile(string fileName)
        {
            _loadedFile = fileName;
            txtPath.Text = fileName;

            var csvIndexer = new CsvIndexer();
            csvIndexer.LoadIndexOfFile(_loadedFile);
            _index = csvIndexer.Index;
            _totalRegs = _index.Count - 1;

            RenderReg(0);
        }

        private List<string> Index_LoadReg(int idx)
        {
            var csvParser = new CsvParser();
            csvParser.ParseFile(_loadedFile, _index[idx], 1);
            return csvParser.Data[0];
        }

        bool _rendering = false;
        private void RenderReg(long currentReg)
        {
            if (_index == null || _index.Count <= 0)
            {
                RenderRegClean();
            }

            if (_rendering) { return; }
            _rendering = true;

            pnlReg.Enabled = true;
            bool first = false;
            bool last = false;
            if (currentReg <= 0)
            {
                currentReg = 0;
                first = true;
            }
            if (currentReg >= (_totalRegs - 1))
            {
                currentReg = _totalRegs - 1;
                last = true;
            }

            pnlScrollData.SuspendDrawing();
            pnlScrollData.VerticalScroll.Value = 0;
            pnlData.Height = 10;

            pnlData.Controls.Clear();
            _currentReg = currentReg;
            txtCurrentReg.Text = Convert.ToString(currentReg);
            txtTotalRegs.Text = Convert.ToString(_totalRegs);

            btnFirstReg.Enabled = (first == false);
            btnPrevReg.Enabled = (first == false);
            btnLastReg.Enabled = (last == false);
            btnNextReg.Enabled = (last == false);

            List<string> currentData = Index_LoadReg((int)currentReg);

            int y = 0;
            const int TexboxPadding = 5;
            const int PaddingLeft = 0;
            const int PaddingRight = 0;
            const int PaddingBetween = 10;
            const int LineHeight = 15;
            for (int i = 0; i < currentData.Count; i++)
            {
                TextBox txtValue = RenderValue(currentData[i], y, TexboxPadding, PaddingLeft, PaddingRight, LineHeight);
                pnlData.Controls.Add(txtValue);
                y += txtValue.Height + PaddingBetween;
            }
            pnlData.Height = y;

            pnlScrollData.ResumeDrawing();
            _rendering = false;
        }

        private TextBox RenderValue(string value, int y, int TexboxPadding, int PaddingLeft, int PaddingRight, int LineHeight)
        {
            string[] valueLines = value.Split('\n');
            CTextBox txtValue = new CTextBox()
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right),
                Width = pnlData.Width - (PaddingLeft + PaddingRight),
                Height = (valueLines.Length * LineHeight) + TexboxPadding,
                Multiline = (valueLines.Length > 1),
                Top = y,
                Left = PaddingLeft,
                ReadOnly = true,
            };
            for (int j = 0; j < valueLines.Length; j++)
            {
                if (j > 0)
                {
                    txtValue.AppendText("\n");
                }
                txtValue.AppendText(valueLines[j]);
            }
            return txtValue;
        }

        private void RenderRegClean()
        {
            if (_rendering) { return; }
            _rendering = true;

            pnlScrollData.SuspendDrawing();
            pnlScrollData.VerticalScroll.Value = 0;
            pnlData.Height = 10;

            pnlData.Controls.Clear();
            pnlReg.Enabled = false;
            txtCurrentReg.Text = string.Empty;
            txtTotalRegs.Text = string.Empty;

            pnlScrollData.ResumeDrawing();
            _rendering = false;
        }

        private void btnFirstReg_Click(object sender, EventArgs e)
        {
            RenderReg(0);
        }

        private void btnPrevReg_Click(object sender, EventArgs e)
        {
            RenderReg(_currentReg - 1);
        }

        private void btnNextReg_Click(object sender, EventArgs e)
        {
            RenderReg(_currentReg + 1);
        }

        private void btnLastReg_Click(object sender, EventArgs e)
        {
            RenderReg(_totalRegs - 1);
        }

        private void txtCurrentReg_TextChanged(object sender, EventArgs e)
        {
            int newReg = 0;
            if (int.TryParse(txtCurrentReg.Text, out newReg))
            {
                RenderReg(newReg);
            }
            else
            {
                RenderReg(_currentReg);
            }
        }
    }
}
