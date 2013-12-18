using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションクラス
    /// </summary>
    public static class HoI2EditorApplication
    {
        #region 公開プロパティ

        /// <summary>
        ///     エディターのバージョン
        /// </summary>
        public static string Version
        {
            get { return _version; }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     エディターのバージョン
        /// </summary>
        private static string _version;

        /// <summary>
        ///     ログファイル書き込み用
        /// </summary>
        private static StreamWriter _writer;

        /// <summary>
        ///     指揮官エディターのフォーム
        /// </summary>
        private static LeaderEditorForm _leaderEditorForm;

        /// <summary>
        ///     閣僚エディターのフォーム
        /// </summary>
        private static MinisterEditorForm _ministerEditorForm;

        /// <summary>
        ///     研究機関エディターのフォーム
        /// </summary>
        private static TeamEditorForm _teamEditorForm;

        /// <summary>
        ///     プロヴィンスエディターのフォーム
        /// </summary>
        private static ProvinceEditorForm _provinceEditorForm;

        /// <summary>
        ///     技術ツリーエディターのフォーム
        /// </summary>
        private static TechEditorForm _techEditorForm;

        /// <summary>
        ///     ユニットモデルエディターのフォーム
        /// </summary>
        private static UnitEditorForm _unitEditorForm;

        /// <summary>
        ///     ゲーム設定エディターのフォーム
        /// </summary>
        private static MiscEditorForm _miscEditorForm;

        /// <summary>
        ///     ユニット名エディターのフォーム
        /// </summary>
        private static UnitNameEditorForm _unitNameEditorForm;

        /// <summary>
        ///     師団名エディターのフォーム
        /// </summary>
        private static DivisionNameEditorForm _divisionNameEditorForm;

        /// <summary>
        ///     ランダム指揮官エディターのフォーム
        /// </summary>
        private static RandomLeaderEditorForm _randomLeaderEditorForm;

        #endregion

        #region 内部定数

        /// <summary>
        ///     ログファイル名
        /// </summary>
        private const string LogFileName = "editorlog.txt";

        /// <summary>
        ///     ログファイル識別子
        /// </summary>
        private const string LogFileIdentifier = "LogFile";

        #endregion

        #region 初期化

        /// <summary>
        ///     アプリケーションのエントリーポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            InitLogFile();

            try
            {
                Debug.WriteLine("");
                Debug.WriteLine(string.Format("[{0}]", DateTime.Now));

                InitVersion();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
            finally
            {
                TermLogFile();
            }
        }

        /// <summary>
        ///     エディターのバージョンを初期化する
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

        /// <summary>
        ///     ログファイルを初期化する
        /// </summary>
        [Conditional("DEBUG")]
        private static void InitLogFile()
        {
            _writer = new StreamWriter(LogFileName, true, Encoding.UTF8) {AutoFlush = true};
            Debug.Listeners.Add(new TextWriterTraceListener(_writer, LogFileIdentifier));
        }

        /// <summary>
        ///     ログファイルを終了する
        /// </summary>
        [Conditional("DEBUG")]
        private static void TermLogFile()
        {
            _writer.Close();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     編集したデータを保存する
        /// </summary>
        public static void Save()
        {
            // 文字列の一時キーを保存形式に変更する
            Techs.RenameTempKeys();

            // 編集したデータを保存する
            Misc.Save();
            Config.Save();
            Leaders.Save();
            Ministers.Save();
            Teams.Save();
            Provinces.Save();
            Techs.Save();
            Units.Save();
            UnitNames.Save();
            DivisionNames.Save();
            RandomLeaders.Load();

            // データ保存後の処理呼び出し
            OnSaved();
        }

        #endregion

        #region エディターフォーム管理

        /// <summary>
        ///     指揮官エディターフォームを起動する
        /// </summary>
        public static void LaunchLeaderEditorForm()
        {
            if (_leaderEditorForm == null)
            {
                _leaderEditorForm = new LeaderEditorForm();
                _leaderEditorForm.Show();
            }
            else
            {
                _leaderEditorForm.Activate();
            }
        }

        /// <summary>
        ///     閣僚エディターフォームを起動する
        /// </summary>
        public static void LaunchMinisterEditorForm()
        {
            if (_ministerEditorForm == null)
            {
                _ministerEditorForm = new MinisterEditorForm();
                _ministerEditorForm.Show();
            }
            else
            {
                _ministerEditorForm.Activate();
            }
        }

        /// <summary>
        ///     研究機関エディターフォームを起動する
        /// </summary>
        public static void LaunchTeamEditorForm()
        {
            if (_teamEditorForm == null)
            {
                _teamEditorForm = new TeamEditorForm();
                _teamEditorForm.Show();
            }
            else
            {
                _teamEditorForm.Activate();
            }
        }

        /// <summary>
        ///     プロヴィンスエディターフォームを起動する
        /// </summary>
        public static void LaunchProvinceEditorForm()
        {
            if (_provinceEditorForm == null)
            {
                _provinceEditorForm = new ProvinceEditorForm();
                _provinceEditorForm.Show();
            }
            else
            {
                _provinceEditorForm.Activate();
            }
        }

        /// <summary>
        ///     技術ツリーエディターフォームを起動する
        /// </summary>
        public static void LaunchTechEditorForm()
        {
            if (_techEditorForm == null)
            {
                _techEditorForm = new TechEditorForm();
                _techEditorForm.Show();
            }
            else
            {
                _techEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ユニットモデルエディターフォームを起動する
        /// </summary>
        public static void LaunchUnitEditorForm()
        {
            if (_unitEditorForm == null)
            {
                _unitEditorForm = new UnitEditorForm();
                _unitEditorForm.Show();
            }
            else
            {
                _unitEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ゲーム設定エディターフォームを起動する
        /// </summary>
        public static void LaunchMiscEditorForm()
        {
            if (_miscEditorForm == null)
            {
                _miscEditorForm = new MiscEditorForm();
                _miscEditorForm.Show();
            }
            else
            {
                _miscEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ユニット名エディターフォームを起動する
        /// </summary>
        public static void LaunchUnitNameEditorForm()
        {
            if (_unitNameEditorForm == null)
            {
                _unitNameEditorForm = new UnitNameEditorForm();
                _unitNameEditorForm.Show();
            }
            else
            {
                _unitNameEditorForm.Activate();
            }
        }

        /// <summary>
        ///     師団名エディターフォームを起動する
        /// </summary>
        public static void LaunchDivisionNameEditorForm()
        {
            if (_divisionNameEditorForm == null)
            {
                _divisionNameEditorForm = new DivisionNameEditorForm();
                _divisionNameEditorForm.Show();
            }
            else
            {
                _divisionNameEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ランダム指揮官エディターフォームを起動する
        /// </summary>
        public static void LaunchRandomLeaderEditorForm()
        {
            if (_randomLeaderEditorForm == null)
            {
                _randomLeaderEditorForm = new RandomLeaderEditorForm();
                _randomLeaderEditorForm.Show();
            }
            else
            {
                _randomLeaderEditorForm.Activate();
            }
        }

        /// <summary>
        ///     指揮官エディターフォームクローズ時の処理
        /// </summary>
        public static void OnLeaderEditorFormClosed()
        {
            _leaderEditorForm = null;
        }

        /// <summary>
        ///     閣僚エディターフォームクローズ時の処理
        /// </summary>
        public static void OnMinisterEditorFormClosed()
        {
            _ministerEditorForm = null;
        }

        /// <summary>
        ///     研究機関エディターフォームクローズ時の処理
        /// </summary>
        public static void OnTeamEditorFormClosed()
        {
            _teamEditorForm = null;
        }

        /// <summary>
        ///     プロヴィンスエディターフォームクローズ時の処理
        /// </summary>
        public static void OnProvinceEditorFormClosed()
        {
            _provinceEditorForm = null;
        }

        /// <summary>
        ///     技術ツリーエディターフォームクローズ時の処理
        /// </summary>
        public static void OnTechEditorFormClosed()
        {
            _techEditorForm = null;
        }

        /// <summary>
        ///     ユニットモデルエディターフォームクローズ時の処理
        /// </summary>
        public static void OnUnitEditorFormClosed()
        {
            _unitEditorForm = null;
        }

        /// <summary>
        ///     ゲーム設定エディターフォームクローズ時の処理
        /// </summary>
        public static void OnMiscEditorFormClosed()
        {
            _miscEditorForm = null;
        }

        /// <summary>
        ///     ユニット名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnUnitNameEditorFormClosed()
        {
            _unitNameEditorForm = null;
        }

        /// <summary>
        ///     師団名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnDivisionNameEditorFormClosed()
        {
            _divisionNameEditorForm = null;
        }

        /// <summary>
        ///     ランダム指揮官エディターフォームクローズ時の処理
        /// </summary>
        public static void OnRandomLeaderEditorFormClosed()
        {
            _randomLeaderEditorForm = null;
        }

        /// <summary>
        ///     データ保存後の処理呼び出し
        /// </summary>
        private static void OnSaved()
        {
            if (_leaderEditorForm != null)
            {
                _leaderEditorForm.OnLeadersSaved();
            }
            if (_ministerEditorForm != null)
            {
                _ministerEditorForm.OnMinistersSaved();
            }
            if (_teamEditorForm != null)
            {
                _teamEditorForm.OnTeamsSaved();
            }
            if (_provinceEditorForm != null)
            {
                _provinceEditorForm.OnProvincesSaved();
            }
            if (_techEditorForm != null)
            {
                _techEditorForm.OnTechsSaved();
            }
            if (_unitEditorForm != null)
            {
                _unitEditorForm.OnUnitsSaved();
            }
            if (_miscEditorForm != null)
            {
                _miscEditorForm.OnMiscSaved();
            }
            if (_unitNameEditorForm != null)
            {
                _unitNameEditorForm.OnUnitNamesSaved();
            }
            if (_divisionNameEditorForm != null)
            {
                _divisionNameEditorForm.OnDivisionNamesSaved();
            }
            if (_randomLeaderEditorForm != null)
            {
                _randomLeaderEditorForm.OnRandomLeadersSaved();
            }
        }

        #endregion
    }
}