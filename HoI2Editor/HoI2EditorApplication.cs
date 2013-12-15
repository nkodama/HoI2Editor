using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Forms;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションクラス
    /// </summary>
    public static class HoI2EditorApplication
    {
        /// <summary>
        /// エディターのバージョン
        /// </summary>
        public static string Version { get { return _version; } }

        /// <summary>
        /// エディターのバージョン
        /// </summary>
        private static string _version;

        /// <summary>
        ///     アプリケーションのエントリーポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var writer = new StreamWriter("editorlog.txt", true, Encoding.UTF8))
            {
                writer.AutoFlush = true;
                Debug.Listeners.Add(new TextWriterTraceListener(writer, "LogFile"));

                Debug.WriteLine("");
                Debug.WriteLine(string.Format("[{0}]", DateTime.Now));
                InitVersion();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
        }

        /// <summary>
        /// エディターのバージョンを初期化する
        /// </summary>
        private static void InitVersion()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            if (info.FilePrivatePart > 0 && info.FilePrivatePart <= 26)
            {
                _version = string.Format("Alternative HoI2 Editor Ver {0}.{1}{2}{3}", info.FileMajorPart,
                                         info.FileMinorPart, info.FileBuildPart, (char) ('`' + info.FilePrivatePart));
            }
            else
            {
                _version = string.Format("Alternative HoI2 Editor Ver {0}.{1}{2}", info.FileMajorPart,
                                         info.FileMinorPart, info.FileBuildPart);
            }
            Debug.WriteLine(_version);
        }
    }
}