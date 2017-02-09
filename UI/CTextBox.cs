using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CsvView.UI
{
    public class CTextBox : TextBox
    {
        public CTextBox()
        {
            WordWrap = false;
            ScrollBars = ScrollBars.Horizontal;
            Font = new Font("Courier New", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
        }
        
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private const int WM_MOUSEWHEEL = 0x20a;
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                SendMessage(Parent.Handle, m.Msg, m.WParam, m.LParam);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
