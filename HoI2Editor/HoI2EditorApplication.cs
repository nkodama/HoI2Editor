using System;
using System.Windows.Forms;
using HoI2Editor.Forms;

namespace HoI2Editor
{
    internal class HoI2EditorApplication
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MinisterEditorForm());
        }
    }
}