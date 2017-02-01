using System;
using System.Windows.Forms;

namespace CsvView.Net
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frmCsvViewer = new FrmCsvViewer();
            if (args.Length > 0)
            {
                if (System.IO.File.Exists(args[0]))
                {
                    frmCsvViewer.LoadFile(args[0]);
                }
            }
            Application.Run(frmCsvViewer);
        }
    }
}
