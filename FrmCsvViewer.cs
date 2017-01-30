using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
            DialogResult result = loadDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPath.Text = loadDialog.FileName;
            }
        }

        private string _loadedFile = string.Empty;
        private long _currentReg = 0;
        private long _totalRegs = 0;
        private List<List<string>> _data = null;

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtPath.Text) == false)
            {
                RenderRegClean();
                _loadedFile = null;
                _totalRegs = 0;
                _data = null;
                MessageBoxEx.Show(this, "FileNotFound");
                return;
            }

            _loadedFile = txtPath.Text;

            var csvParser = new CsvParser();
            csvParser.ParseFile(_loadedFile);

            _totalRegs = csvParser.Data.Count;
            _data = csvParser.Data;

            RenderReg(0);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SetRedraw = 0XB;

        private void RenderReg(long currentReg)
        {
            if (_data == null || _data.Count <= 0)
            {
                RenderRegClean();
            }
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

            _currentReg = currentReg;
            txtCurrentReg.Text = Convert.ToString(currentReg);
            txtTotalRegs.Text = Convert.ToString(_totalRegs);

            btnFirstReg.Enabled = (first == false);
            btnPrevReg.Enabled = (first == false);
            btnLastReg.Enabled = (last == false);
            btnNextReg.Enabled = (last == false);

            List<string> currentData = _data[(int)currentReg];
            
            pnlData.Visible = false;
            pnlData.Controls.Clear();
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
            pnlData.Visible = true;
        }

        private TextBox RenderValue(string value, int y, int TexboxPadding, int Padding, int LineHeight)
        {
            string[] valueLines = value.Split('\n');
            CTextBox txtValue = new CTextBox()
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right),
                Width = pnlData.Width - Padding,
                Height = (valueLines.Length * LineHeight) + TexboxPadding,
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
            pnlReg.Enabled = false;
            txtCurrentReg.Text = string.Empty;
            txtTotalRegs.Text = string.Empty;
            pnlData.Controls.Clear();
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
