using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml.Serialization;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションコントローラクラス
    /// </summary>
    internal static class HoI2EditorController
    {
        #region エディタのバージョン

        /// <summary>
        ///     アプリケーション名
        /// </summary>
        internal const string Name = "Alternative HoI2 Editor";

        /// <summary>
        ///     エディタのバージョン
        /// </summary>
        internal static string Version { get; private set; }

        /// <summary>
        ///     エディタのバージョンを初期化する
        /// </summary>
        internal static void InitVersion()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            if (info.FilePrivatePart > 0 && info.FilePrivatePart <= 26)
            {
                Version =
                    $"{Name} Ver {info.FileMajorPart}.{info.FileMinorPart}{info.FileBuildPart}{'`' + info.FilePrivatePart}";
            }
            else
            {
                Version = $"{Name} Ver {info.FileMajorPart}.{info.FileMinorPart}{info.FileBuildPart}";
            }
        }

        #endregion

        #region リソース管理

        /// <summary>
        ///     リソースマネージャ
        /// </summary>
        private static readonly ResourceManager ResourceManager
            = new ResourceManager("HoI2Editor.Properties.Resources", typeof (Resources).Assembly);

        /// <summary>
        ///     リソース文字列を取得する
        /// </summary>
        /// <param name="name">リソース名</param>
        internal static string GetResourceString(string name)
        {
            return ResourceManager.GetString(name);
        }

        #endregion

        #region 多重編集禁止

        /// <summary>
        ///     多重編集禁止用のミューテックス
        /// </summary>
        private const string MutextName = "Alternative HoI2 Editor";

        private static Mutex _mutex;

        /// <summary>
        ///     ミューテックスをロックする
        /// </summary>
        /// <param name="key">キー文字列</param>
        /// <returns>ロックに成功したらtrueを返す</returns>
        internal static bool LockMutex(string key)
        {
            // 既にミューテックスをロックしていれば解放する
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex = null;
            }

            _mutex = new Mutex(false, $"{MutextName}: {key.GetHashCode()}");

            if (!_mutex.WaitOne(0, false))
            {
                _mutex = null;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     ミューテックスをアンロックする
        /// </summary>
        internal static void UnlockMutex()
        {
            if (_mutex == null)
            {
                return;
            }

            _mutex.ReleaseMutex();
            _mutex = null;
        }

        #endregion

        #region 設定値管理

        /// <summary>
        ///     設定ファイル名
        /// </summary>
        private const string SettingsFileName = "HoI2Editor.settings";

        /// <summary>
        ///     設定値
        /// </summary>
        internal static HoI2EditorSettings Settings { get; private set; }

        /// <summary>
        ///     設定値を読み込む
        /// </summary>
        internal static void LoadSettings()
        {
            if (!File.Exists(SettingsFileName))
            {
                Settings = new HoI2EditorSettings();
                // ログ出力レベルを[情報]に設定する
                Log.Level = 3;
            }
            else
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (HoI2EditorSettings));
                    using (FileStream fs = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read))
                    {
                        Settings = serializer.Deserialize(fs) as HoI2EditorSettings;
                        if (Settings == null)
                        {
                            return;
                        }
                    }
                }
                catch (Exception)
                {
                    Log.Error("[Settings] Read error");
                    Settings = new HoI2EditorSettings();
                }
            }
            Settings.Round();
        }

        /// <summary>
        ///     設定値を保存する
        /// </summary>
        internal static void SaveSettings()
        {
            if (Settings == null)
            {
                return;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof (HoI2EditorSettings));
                using (FileStream fs = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, Settings);
                }
            }
            catch (Exception)
            {
                Log.Error("[Settings] Write error");
            }
        }

        #endregion
    }

    /// <summary>
    ///     エディタの編集項目ID
    /// </summary>
    internal enum EditorItemId
    {
        LeaderRetirementYear, // 指揮官の引退年設定
        MinisterEndYear, // 閣僚の終了年設定
        MinisterRetirementYear, // 閣僚の引退年設定
        TeamList, // 研究機関リスト
        TeamCountry, // 研究機関の所属国
        TeamName, // 研究機関名
        TeamId, // 研究機関ID
        TeamSkill, // 研究機関のスキル
        TeamSpeciality, // 研究機関の特性
        TechItemList, // 技術項目リスト
        TechItemName, // 技術項目名
        TechItemId, // 技術項目ID
        TechItemYear, // 技術項目の史実年度
        TechComponentList, // 小研究リスト
        TechComponentSpeciality, // 小研究の特性
        TechComponentDifficulty, // 小研究の難易度
        TechComponentDoubleTime, // 小研究の2倍時間設定
        UnitName, // ユニットクラス名
        MaxAllowedBrigades, // 最大付属可能旅団数
        ModelList, // ユニットモデルリスト
        CommonModelName, // 共通のユニットモデル名
        CountryModelName // 国別のユニットモデル名
    }
}