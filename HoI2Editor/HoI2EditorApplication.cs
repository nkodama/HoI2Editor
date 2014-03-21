using System;
using System.Windows.Forms;
using HoI2Editor.Models;

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
                HoI2Editor.InitVersion();
                HoI2Editor.LoadSettings();

                Log.Error("");
                Log.Error("[{0}]", DateTime.Now);
                Log.Error(HoI2Editor.Version);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                HoI2Editor.LaunchMainForm();
            }
            finally
            {
                HoI2Editor.SaveSettings();
                HoI2Editor.UnlockMutex();
            }
        }
    }
}