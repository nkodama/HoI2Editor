using System;
using System.Windows.Forms;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションクラス
    /// </summary>
    public static class HoI2EditorApplication
    {
        /// <summary>
        ///     アプリケーションのエントリーポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
                HoI2Editor.InitLogFile();
                HoI2Editor.InitVersion();
                HoI2Editor.LoadSettings();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                HoI2Editor.LaunchMainForm();
            }
            finally
            {
                HoI2Editor.SaveSettings();
                HoI2Editor.TermLogFile();
                HoI2Editor.UnlockMutex();
            }
        }
    }
}