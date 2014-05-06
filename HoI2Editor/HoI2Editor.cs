using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションコントローラクラス
    /// </summary>
    public static class HoI2Editor
    {
        #region エディターのバージョン

        /// <summary>
        ///     アプリケーション名
        /// </summary>
        public const string Name = "Alternative HoI2 Editor";

        /// <summary>
        ///     エディターのバージョン
        /// </summary>
        public static string Version { get; private set; }

        /// <summary>
        ///     エディターのバージョンを初期化する
        /// </summary>
        public static void InitVersion()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            if (info.FilePrivatePart > 0 && info.FilePrivatePart <= 26)
            {
                Version = string.Format("{0} Ver {1}.{2}{3}{4}", Name, info.FileMajorPart,
                    info.FileMinorPart, info.FileBuildPart, (char) ('`' + info.FilePrivatePart));
            }
            else
            {
                Version = string.Format("{0} Ver {1}.{2}{3}", Name, info.FileMajorPart,
                    info.FileMinorPart, info.FileBuildPart);
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
        public static string GetResourceString(string name)
        {
            return ResourceManager.GetString(name);
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     保存がキャンセルされたかどうか
        /// </summary>
        public static bool SaveCanceled { get; set; }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty()
        {
            return Misc.IsDirty() ||
                   Config.IsDirty() ||
                   Leaders.IsDirty() ||
                   Ministers.IsDirty() ||
                   Teams.IsDirty() ||
                   Provinces.IsDirty() ||
                   Techs.IsDirty() ||
                   Units.IsDirty() ||
                   CorpsNames.IsDirty() ||
                   UnitNames.IsDirty() ||
                   RandomLeaders.IsDirty();
        }

        /// <summary>
        ///     ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            Misc.RequestReload();
            Config.RequestReload();
            Leaders.RequestReload();
            Ministers.RequestReload();
            Teams.RequestReload();
            Techs.RequestReload();
            Units.RequestReload();
            Provinces.RequestReload();
            CorpsNames.RequestReload();
            UnitNames.RequestReload();
            RandomLeaders.RequestReload();

            SaveCanceled = false;

            Log.Verbose("Request to reload");
        }

        /// <summary>
        ///     データを再読み込みする
        /// </summary>
        public static void Reload()
        {
            Log.Info("Reload");

            // データを再読み込みする
            Misc.Reload();
            Config.Reload();
            Leaders.Reload();
            Ministers.Reload();
            Teams.Reload();
            Provinces.Reload();
            Techs.Reload();
            Units.Reload();
            CorpsNames.Reload();
            UnitNames.Reload();
            RandomLeaders.Reload();

            // データ読み込み後の更新処理呼び出し
            OnFileLoaded();

            SaveCanceled = false;
        }

        /// <summary>
        ///     データを保存する
        /// </summary>
        public static void Save()
        {
            Log.Info("Save");

            // 文字列の一時キーを保存形式に変更する
            Techs.RenameKeys();

            // 編集したデータを保存する
            SaveFiles();

            // データ保存後の更新処理呼び出し
            OnFileSaved();

            SaveCanceled = false;
        }

        /// <summary>
        ///     データを保存する
        /// </summary>
        private static void SaveFiles()
        {
            if (!Misc.Save())
            {
                return;
            }
            if (!Config.Save())
            {
                return;
            }
            if (!Leaders.Save())
            {
                return;
            }
            if (!Ministers.Save())
            {
                return;
            }
            if (!Teams.Save())
            {
                return;
            }
            if (!Provinces.Save())
            {
                return;
            }
            if (!Techs.Save())
            {
                return;
            }
            if (!Units.Save())
            {
                return;
            }
            if (!CorpsNames.Save())
            {
                return;
            }
            if (!UnitNames.Save())
            {
                return;
            }
            RandomLeaders.Save();
        }

        /// <summary>
        ///     データ読み込み後の更新処理呼び出し
        /// </summary>
        private static void OnFileLoaded()
        {
            if (_leaderEditorForm != null)
            {
                _leaderEditorForm.OnFileLoaded();
            }
            if (_ministerEditorForm != null)
            {
                _ministerEditorForm.OnFileLoaded();
            }
            if (_teamEditorForm != null)
            {
                _teamEditorForm.OnFileLoaded();
            }
            if (_provinceEditorForm != null)
            {
                _provinceEditorForm.OnFileLoaded();
            }
            if (_techEditorForm != null)
            {
                _techEditorForm.OnFileLoaded();
            }
            if (_unitEditorForm != null)
            {
                _unitEditorForm.OnFileLoaded();
            }
            if (_miscEditorForm != null)
            {
                _miscEditorForm.OnFileLoaded();
            }
            if (_corpsNameEditorForm != null)
            {
                _corpsNameEditorForm.OnFileLoaded();
            }
            if (_unitNameEditorForm != null)
            {
                _unitNameEditorForm.OnFileLoaded();
            }
            if (_modelNameEditorForm != null)
            {
                _modelNameEditorForm.OnFileLoaded();
            }
            if (_randomLeaderEditorForm != null)
            {
                _randomLeaderEditorForm.OnFileLoaded();
            }
            if (_researchViewerForm != null)
            {
                _researchViewerForm.OnFileLoaded();
            }
        }

        /// <summary>
        ///     データ保存後の更新処理呼び出し
        /// </summary>
        private static void OnFileSaved()
        {
            if (_leaderEditorForm != null)
            {
                _leaderEditorForm.OnFileSaved();
            }
            if (_ministerEditorForm != null)
            {
                _ministerEditorForm.OnFileSaved();
            }
            if (_teamEditorForm != null)
            {
                _teamEditorForm.OnFileSaved();
            }
            if (_provinceEditorForm != null)
            {
                _provinceEditorForm.OnFileSaved();
            }
            if (_techEditorForm != null)
            {
                _techEditorForm.OnFileSaved();
            }
            if (_unitEditorForm != null)
            {
                _unitEditorForm.OnFileSaved();
            }
            if (_miscEditorForm != null)
            {
                _miscEditorForm.OnFileSaved();
            }
            if (_corpsNameEditorForm != null)
            {
                _corpsNameEditorForm.OnFileSaved();
            }
            if (_unitNameEditorForm != null)
            {
                _unitNameEditorForm.OnFileSaved();
            }
            if (_modelNameEditorForm != null)
            {
                _modelNameEditorForm.OnFileSaved();
            }
            if (_randomLeaderEditorForm != null)
            {
                _randomLeaderEditorForm.OnFileSaved();
            }
        }

        /// <summary>
        ///     編集項目変更後の更新処理呼び出し
        /// </summary>
        /// <param name="id">編集項目ID</param>
        /// <param name="form">呼び出し元のフォーム</param>
        public static void OnItemChanged(EditorItemId id, Form form)
        {
            if ((_leaderEditorForm != null) && (form != _leaderEditorForm))
            {
                _leaderEditorForm.OnItemChanged(id);
            }
            if ((_ministerEditorForm != null) && (form != _ministerEditorForm))
            {
                _ministerEditorForm.OnItemChanged(id);
            }
            if ((_teamEditorForm != null) && (form != _teamEditorForm))
            {
                _teamEditorForm.OnItemChanged(id);
            }
            if ((_provinceEditorForm != null) && (form != _provinceEditorForm))
            {
                _provinceEditorForm.OnItemChanged(id);
            }
            if ((_techEditorForm != null) && (form != _techEditorForm))
            {
                _techEditorForm.OnItemChanged(id);
            }
            if ((_unitEditorForm != null) && (form != _unitEditorForm))
            {
                _unitEditorForm.OnItemChanged(id);
            }
            if ((_miscEditorForm != null) && (form != _miscEditorForm))
            {
                _miscEditorForm.OnItemChanged(id);
            }
            if ((_corpsNameEditorForm != null) && (form != _corpsNameEditorForm))
            {
                _corpsNameEditorForm.OnItemChanged(id);
            }
            if ((_unitNameEditorForm != null) && (form != _unitNameEditorForm))
            {
                _unitNameEditorForm.OnItemChanged(id);
            }
            if ((_modelNameEditorForm != null) && (form != _modelNameEditorForm))
            {
                _modelNameEditorForm.OnItemChanged(id);
            }
            if ((_randomLeaderEditorForm != null) && (form != _randomLeaderEditorForm))
            {
                _randomLeaderEditorForm.OnItemChanged(id);
            }
            if (_researchViewerForm != null)
            {
                _researchViewerForm.OnItemChanged(id);
            }
        }

        #endregion

        #region エディターフォーム管理

        /// <summary>
        ///     メインフォーム
        /// </summary>
        private static MainForm _mainForm;

        /// <summary>
        ///     指揮官エディタのフォーム
        /// </summary>
        private static LeaderEditorForm _leaderEditorForm;

        /// <summary>
        ///     閣僚エディタのフォーム
        /// </summary>
        private static MinisterEditorForm _ministerEditorForm;

        /// <summary>
        ///     研究機関エディタのフォーム
        /// </summary>
        private static TeamEditorForm _teamEditorForm;

        /// <summary>
        ///     プロヴィンスエディタのフォーム
        /// </summary>
        private static ProvinceEditorForm _provinceEditorForm;

        /// <summary>
        ///     技術ツリーエディタのフォーム
        /// </summary>
        private static TechEditorForm _techEditorForm;

        /// <summary>
        ///     ユニットモデルエディタのフォーム
        /// </summary>
        private static UnitEditorForm _unitEditorForm;

        /// <summary>
        ///     ゲーム設定エディタのフォーム
        /// </summary>
        private static MiscEditorForm _miscEditorForm;

        /// <summary>
        ///     軍団名エディタのフォーム
        /// </summary>
        private static CorpsNameEditorForm _corpsNameEditorForm;

        /// <summary>
        ///     ユニット名エディタのフォーム
        /// </summary>
        private static UnitNameEditorForm _unitNameEditorForm;

        /// <summary>
        ///     モデル名エディタのフォーム
        /// </summary>
        private static ModelNameEditorForm _modelNameEditorForm;

        /// <summary>
        ///     ランダム指揮官エディタのフォーム
        /// </summary>
        private static RandomLeaderEditorForm _randomLeaderEditorForm;

        /// <summary>
        ///     研究速度ビューアのフォーム
        /// </summary>
        private static ResearchViewerForm _researchViewerForm;

        /// <summary>
        ///     メインフォームを起動する
        /// </summary>
        public static void LaunchMainForm()
        {
            _mainForm = new MainForm();
            Application.Run(_mainForm);
        }

        /// <summary>
        ///     指揮官エディタフォームを起動する
        /// </summary>
        public static void LaunchLeaderEditorForm()
        {
            if (_leaderEditorForm == null)
            {
                _leaderEditorForm = new LeaderEditorForm();
                _leaderEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _leaderEditorForm.Activate();
            }
        }

        /// <summary>
        ///     閣僚エディタフォームを起動する
        /// </summary>
        public static void LaunchMinisterEditorForm()
        {
            if (_ministerEditorForm == null)
            {
                _ministerEditorForm = new MinisterEditorForm();
                _ministerEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _ministerEditorForm.Activate();
            }
        }

        /// <summary>
        ///     研究機関エディタフォームを起動する
        /// </summary>
        public static void LaunchTeamEditorForm()
        {
            if (_teamEditorForm == null)
            {
                _teamEditorForm = new TeamEditorForm();
                _teamEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _teamEditorForm.Activate();
            }
        }

        /// <summary>
        ///     プロヴィンスエディタフォームを起動する
        /// </summary>
        public static void LaunchProvinceEditorForm()
        {
            if (_provinceEditorForm == null)
            {
                _provinceEditorForm = new ProvinceEditorForm();
                _provinceEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _provinceEditorForm.Activate();
            }
        }

        /// <summary>
        ///     技術ツリーエディタフォームを起動する
        /// </summary>
        public static void LaunchTechEditorForm()
        {
            if (_techEditorForm == null)
            {
                _techEditorForm = new TechEditorForm();
                _techEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _techEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ユニットモデルエディタフォームを起動する
        /// </summary>
        public static void LaunchUnitEditorForm()
        {
            if (_unitEditorForm == null)
            {
                _unitEditorForm = new UnitEditorForm();
                _unitEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _unitEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ゲーム設定エディタフォームを起動する
        /// </summary>
        public static void LaunchMiscEditorForm()
        {
            if (_miscEditorForm == null)
            {
                _miscEditorForm = new MiscEditorForm();
                _miscEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _miscEditorForm.Activate();
            }
        }

        /// <summary>
        ///     軍団名エディタフォームを起動する
        /// </summary>
        public static void LaunchCorpsNameEditorForm()
        {
            if (_corpsNameEditorForm == null)
            {
                _corpsNameEditorForm = new CorpsNameEditorForm();
                _corpsNameEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _corpsNameEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ユニット名エディタフォームを起動する
        /// </summary>
        public static void LaunchUnitNameEditorForm()
        {
            if (_unitNameEditorForm == null)
            {
                _unitNameEditorForm = new UnitNameEditorForm();
                _unitNameEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _unitNameEditorForm.Activate();
            }
        }

        /// <summary>
        ///     モデル名エディタフォームを起動する
        /// </summary>
        public static void LaunchModelNameEditorForm()
        {
            if (_modelNameEditorForm == null)
            {
                _modelNameEditorForm = new ModelNameEditorForm();
                _modelNameEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _modelNameEditorForm.Activate();
            }
        }

        /// <summary>
        ///     ランダム指揮官エディタフォームを起動する
        /// </summary>
        public static void LaunchRandomLeaderEditorForm()
        {
            if (_randomLeaderEditorForm == null)
            {
                _randomLeaderEditorForm = new RandomLeaderEditorForm();
                _randomLeaderEditorForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _randomLeaderEditorForm.Activate();
            }
        }

        /// <summary>
        ///     研究速度ビューアフォームを起動する
        /// </summary>
        public static void LaunchResearchViewerForm()
        {
            if (_researchViewerForm == null)
            {
                _researchViewerForm = new ResearchViewerForm();
                _researchViewerForm.Show();

                OnEditorStatusUpdete();
            }
            else
            {
                _researchViewerForm.Activate();
            }
        }

        /// <summary>
        ///     指揮官エディターフォームクローズ時の処理
        /// </summary>
        public static void OnLeaderEditorFormClosed()
        {
            _leaderEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     閣僚エディターフォームクローズ時の処理
        /// </summary>
        public static void OnMinisterEditorFormClosed()
        {
            _ministerEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     研究機関エディターフォームクローズ時の処理
        /// </summary>
        public static void OnTeamEditorFormClosed()
        {
            _teamEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     プロヴィンスエディターフォームクローズ時の処理
        /// </summary>
        public static void OnProvinceEditorFormClosed()
        {
            _provinceEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     技術ツリーエディターフォームクローズ時の処理
        /// </summary>
        public static void OnTechEditorFormClosed()
        {
            _techEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     ユニットモデルエディターフォームクローズ時の処理
        /// </summary>
        public static void OnUnitEditorFormClosed()
        {
            _unitEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     ゲーム設定エディターフォームクローズ時の処理
        /// </summary>
        public static void OnMiscEditorFormClosed()
        {
            _miscEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     軍団名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnCorpsNameEditorFormClosed()
        {
            _corpsNameEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     ユニット名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnUnitNameEditorFormClosed()
        {
            _unitNameEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     モデル名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnModelNameEditorFormClosed()
        {
            _modelNameEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     ランダム指揮官エディターフォームクローズ時の処理
        /// </summary>
        public static void OnRandomLeaderEditorFormClosed()
        {
            _randomLeaderEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     研究速度ビューアフォームクローズ時の処理
        /// </summary>
        public static void OnResearchViewerFormClosed()
        {
            _researchViewerForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     エディターの状態更新時の処理
        /// </summary>
        private static void OnEditorStatusUpdete()
        {
            if (_leaderEditorForm == null &&
                _ministerEditorForm == null &&
                _teamEditorForm == null &&
                _provinceEditorForm == null &&
                _techEditorForm == null &&
                _unitEditorForm == null &&
                _miscEditorForm == null &&
                _corpsNameEditorForm == null &&
                _unitNameEditorForm == null &&
                _modelNameEditorForm == null &&
                _randomLeaderEditorForm == null &&
                _researchViewerForm == null)
            {
                _mainForm.EnableFolderChange();
            }
            else
            {
                _mainForm.DisableFolderChange();
            }
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
        public static bool LockMutex(string key)
        {
            // 既にミューテックスをロックしていれば解放する
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex = null;
            }

            _mutex = new Mutex(false, string.Format("{0}: {1}", MutextName, key.GetHashCode()));

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
        public static void UnlockMutex()
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
        public static HoI2EditorSettings Settings { get; private set; }

        /// <summary>
        ///     設定値を読み込む
        /// </summary>
        public static void LoadSettings()
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
                    var serializer = new XmlSerializer(typeof (HoI2EditorSettings));
                    using (var fs = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read))
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
        public static void SaveSettings()
        {
            if (Settings == null)
            {
                return;
            }
            try
            {
                var serializer = new XmlSerializer(typeof (HoI2EditorSettings));
                using (var fs = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write))
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
    public enum EditorItemId
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
        CountryModelName, // 国別のユニットモデル名
    }
}