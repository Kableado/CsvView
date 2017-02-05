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
            DateTime dtFile = File.GetCreationTime(_loadedFile);
            string indexFile = _loadedFile + ".idx";
            if (File.Exists(indexFile) && File.GetCreationTime(indexFile) > dtFile)
            {
                List<long> tempIndex = Index_LoadFile(indexFile);

                _index = tempIndex;
                _totalRegs = _index.Count - 1;
            }
            else
            {
                // Generate index
                DateTime dtNow = DateTime.UtcNow;
                var csvParser = new CsvParser();
                csvParser.GenerateIndex(_loadedFile);
                TimeSpan tsGenIndex = DateTime.UtcNow - dtNow;

                _index = csvParser.Index;
                _totalRegs = _index.Count - 1;

                // Save Index if expensive generation
                if (tsGenIndex.TotalSeconds > 2)
                {
                    Index_SaveFile(indexFile);
                }

            }
            RenderReg(0);
        }

        private void Index_SaveFile(string indexFile)
        {
            if (File.Exists(indexFile))
            {
                File.Delete(indexFile);
            }
            Stream streamOut = File.Open(indexFile, FileMode.Create);
            using (BinaryWriter binWriter = new BinaryWriter(streamOut))
            {
                binWriter.Write(_index.Count);
                for (int i = 0; i < _index.Count; i++)
                {
                    binWriter.Write(_index[i]);
                }
            }
            streamOut.Close();
        }

        private static List<long> Index_LoadFile(string indexFile)
        {
            var tempIndex = new List<long>();

            Stream streamIn = File.Open(indexFile, FileMode.Open);
            using (BinaryReader binReader = new BinaryReader(streamIn))
            {
                int numRegs = binReader.ReadInt32();
                for (int i = 0; i < numRegs; i++)
                {
                    long value = binReader.ReadInt64();
                    tempIndex.Add(value);
                }
            }
            streamIn.Close();
            return tempIndex;
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

            pnlData.SuspendDrawing();

            pnlData.VerticalScroll.Value = 0;
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
            const int Padding = 9;
            const int LineHeight = 15;
            for (int i = 0; i < currentData.Count; i++)
            {
                TextBox txtValue = RenderValue(currentData[i], y, TexboxPadding, Padding, LineHeight);
                pnlData.Controls.Add(txtValue);
                y += txtValue.Height + Padding;
            }

            pnlData.ResumeDrawing();
            _rendering = false;
        }

        private TextBox RenderValue(string value, int y, int TexboxPadding, int Padding, int LineHeight)
        {
            string[] valueLines = value.Split('\n');
            CTextBox txtValue = new CTextBox()
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right),
                Width = pnlData.Width - Padding,
                Height = (valueLines.Length * LineHeight) + TexboxPadding,
                Multiline = (valueLines.Length > 1),
                Top = y,
                Left = 0,
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

            pnlData.SuspendDrawing();

            pnlData.VerticalScroll.Value = 0;
            pnlData.Controls.Clear();
            pnlReg.Enabled = false;
            txtCurrentReg.Text = string.Empty;
            txtTotalRegs.Text = string.Empty;

            pnlData.ResumeDrawing();
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
