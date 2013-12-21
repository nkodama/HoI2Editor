using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HoI2Editor.Forms;
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
            // 多重起動防止
            const string mutexName = "Alternative HoI2 Editor";
            var mutex = new Mutex(false, mutexName);
            if (!mutex.WaitOne(0, false))
            {
                return;
            }

            try
            {
                InitLogFile();

                Debug.WriteLine("");
                Debug.WriteLine(String.Format("[{0}]", DateTime.Now));

                HoI2Editor.InitVersion();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
            finally
            {
                TermLogFile();

                mutex.ReleaseMutex();
            }
        }

        #region ログファイル

        /// <summary>
        ///     ログファイル名
        /// </summary>
        private const string LogFileName = "editorlog.txt";

        /// <summary>
        ///     ログファイル識別子
        /// </summary>
        private const string LogFileIdentifier = "LogFile";

        /// <summary>
        ///     ログファイル書き込み用
        /// </summary>
        private static StreamWriter _writer;

        /// <summary>
        ///     ログファイル書き込み用
        /// </summary>
        private static TextWriterTraceListener _listener;

        /// <summary>
        ///     ログファイルを初期化する
        /// </summary>
        [Conditional("DEBUG")]
        private static void InitLogFile()
        {
            try
            {
                _writer = new StreamWriter(LogFileName, true, Encoding.UTF8) {AutoFlush = true};
                _listener = new TextWriterTraceListener(_writer, LogFileIdentifier);
                Debug.Listeners.Add(_listener);
            }
            catch (Exception)
            {
                const string appName = "Alternative HoI2 Editor";
                MessageBox.Show(Resources.LogFileOpenError, appName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TermLogFile();
            }
        }

        /// <summary>
        ///     ログファイルを終了する
        /// </summary>
        [Conditional("DEBUG")]
        private static void TermLogFile()
        {
            if (_listener != null)
            {
                Debug.Listeners.Remove(_listener);
            }
            if (_writer != null)
            {
                _writer.Close();
            }
        }

        #endregion
    }
}