using System;
using System.Threading;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

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
                Application.ThreadException += OnThreadException;
                Thread.GetDomain().UnhandledException += OnUnhandledException;

                HoI2EditorController.InitVersion();
                HoI2EditorController.LoadSettings();

                Log.Error("");
                Log.Error("[{0}]", DateTime.Now);
                Log.Error(HoI2EditorController.Version);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                HoI2EditorController.LaunchMainForm();
            }
            finally
            {
                Log.Terminate();
                HoI2EditorController.SaveSettings();
                HoI2EditorController.UnlockMutex();
            }
        }

        /// <summary>
        ///     メインスレッドの未処理例外ハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledExceptionHandler(e.Exception);
        }

        /// <summary>
        ///     メインスレッド以外の未処理例外ハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                UnhandledExceptionHandler(exception);
            }
        }

        /// <summary>
        ///     未処理例外ハンドラ
        /// </summary>
        /// <param name="e">例外</param>
        private static void UnhandledExceptionHandler(Exception e)
        {
            Log.Error($"===== {Resources.CriticalError} =====");
            Log.Error(e.Message);
            Log.Error(e.StackTrace);

            MessageBox.Show($"{e.Message}\n{e.StackTrace}",
                $"{HoI2EditorController.Name} - {Resources.CriticalError}", MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            Application.Exit();
        }
    }
}