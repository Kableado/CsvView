using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CsvView.UI
{
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                          , true);
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SetRedraw = 0XB;

        public void SuspendDrawing()
        {
            SuspendLayout();
            SendMessage(Handle, WM_SetRedraw, false, 0);
        }

        public void ResumeDrawing()
        {
            ResumeLayout(true);
            SendMessage(Handle, WM_SetRedraw, true, 0);
            Refresh();
        }

        [DefaultValue(false)]
        [Localizable(false)]
        [Category("CatLayout")]
        [Description("DisableAutoScrollDescr")]
        public bool DisableAutoScroll { get; set; }

        protected override Point ScrollToControl(Control activeControl)
        {
            if (DisableAutoScroll)
            {
                return DisplayRectangle.Location;
            }
            return base.ScrollToControl(activeControl);
        }
    }
}
