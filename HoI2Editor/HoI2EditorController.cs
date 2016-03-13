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
        internal static string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    AssemblyTitleAttribute attr =
                        (AssemblyTitleAttribute) Attribute.GetCustomAttribute(assembly, typeof (AssemblyTitleAttribute));
                    _name = attr.Title;
                }
                return _name;
            }
        }

        /// <summary>
        ///     アプリケーション名
        /// </summary>
        private static string _name;

        /// <summary>
        ///     エディタのバージョン
        /// </summary>
        internal static string Version
        {
            get
            {
                if (string.IsNullOrEmpty(_version))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                    _version = $"{Name} Ver {info.FileMajorPart}.{info.FileMinorPart}{info.FileBuildPart}";
                    if (info.FilePrivatePart > 0 && info.FilePrivatePart <= 26)
                    {
                        _version += $"{'`' + info.FilePrivatePart}";
                    }
                }
                return _version;
            }
        }

        /// <summary>
        ///     エディタのバージョン
        /// </summary>
        private static string _version;

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
}