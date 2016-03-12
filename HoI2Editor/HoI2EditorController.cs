using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

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

        #region データ処理

        /// <summary>
        ///     保存がキャンセルされたかどうか
        /// </summary>
        internal static bool SaveCanceled { get; set; }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal static bool IsDirty()
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
                   RandomLeaders.IsDirty() ||
                   Scenarios.IsDirty();
        }

        /// <summary>
        ///     ファイルの再読み込みを要求する
        /// </summary>
        internal static void RequestReload()
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
            Scenarios.RequestReload();
            Maps.RequestReload();

            SaveCanceled = false;

            Log.Verbose("Request to reload");
        }

        /// <summary>
        ///     データを再読み込みする
        /// </summary>
        internal static void Reload()
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
            Scenarios.Reload();

            // データ読み込み後の更新処理呼び出し
            OnFileLoaded();

            SaveCanceled = false;
        }

        /// <summary>
        ///     データを保存する
        /// </summary>
        internal static void Save()
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
            if (!RandomLeaders.Save())
            {
                return;
            }
            Scenarios.Save();
        }

        /// <summary>
        ///     データ読み込み後の更新処理呼び出し
        /// </summary>
        private static void OnFileLoaded()
        {
            _leaderEditor?.OnFileLoaded();
            _ministerEditor?.OnFileLoaded();
            _teamEditor?.OnFileLoaded();
            _provinceEditor?.OnFileLoaded();
            _techEditor?.OnFileLoaded();
            _unitEditor?.OnFileLoaded();
            _miscEditor?.OnFileLoaded();
            _corpsNameEditor?.OnFileLoaded();
            _unitNameEditor?.OnFileLoaded();
            _modelNameEditor?.OnFileLoaded();
            _randomLeaderEditor?.OnFileLoaded();
            _researchViewer?.OnFileLoaded();
            _scenarioEditor?.OnFileLoaded();
        }

        /// <summary>
        ///     データ保存後の更新処理呼び出し
        /// </summary>
        private static void OnFileSaved()
        {
            _leaderEditor?.OnFileSaved();
            _ministerEditor?.OnFileSaved();
            _teamEditor?.OnFileSaved();
            _provinceEditor?.OnFileSaved();
            _techEditor?.OnFileSaved();
            _unitEditor?.OnFileSaved();
            _miscEditor?.OnFileSaved();
            _corpsNameEditor?.OnFileSaved();
            _unitNameEditor?.OnFileSaved();
            _modelNameEditor?.OnFileSaved();
            _randomLeaderEditor?.OnFileSaved();
            _scenarioEditor?.OnFileSaved();
        }

        /// <summary>
        ///     編集項目変更後の更新処理呼び出し
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal static void OnItemChanged(EditorItemId id)
        {
            _leaderEditor?.OnItemChanged(id);
            _ministerEditor?.OnItemChanged(id);
            _teamEditor?.OnItemChanged(id);
            _provinceEditor?.OnItemChanged(id);
            _techEditor?.OnItemChanged(id);
            _unitEditor?.OnItemChanged(id);
            _miscEditor?.OnItemChanged(id);
            _corpsNameEditor?.OnItemChanged(id);
            _unitNameEditor?.OnItemChanged(id);
            _modelNameEditor?.OnItemChanged(id);
            _randomLeaderEditor?.OnItemChanged(id);
            _researchViewer?.OnItemChanged(id);
            _scenarioEditor?.OnItemChanged(id);
        }

        /// <summary>
        ///     遅延読み込み完了後の更新処理呼び出し
        /// </summary>
        internal static void OnLoadingCompleted()
        {
            if (!ExistsEditorForms() && !IsLoadingData())
            {
                _mainForm.EnableFolderChange();
            }
            else
            {
                _mainForm.DisableFolderChange();
            }
        }

        /// <summary>
        ///     データを遅延読み込み中かどうかを判定する
        /// </summary>
        /// <returns>データを遅延読み込み中ならばtrueを返す</returns>
        private static bool IsLoadingData()
        {
            return Leaders.IsLoading() ||
                   Ministers.IsLoading() ||
                   Teams.IsLoading() ||
                   Provinces.IsLoading() ||
                   Techs.IsLoading() ||
                   Units.IsLoading() ||
                   Maps.IsLoading();
        }

        #endregion

        #region エディタフォーム管理

        /// <summary>
        ///     メインフォーム
        /// </summary>
        private static MainForm _mainForm;

        /// <summary>
        ///     指揮官エディタコントローラ
        /// </summary>
        private static LeaderEditorController _leaderEditor;

        /// <summary>
        ///     閣僚エディタコントローラ
        /// </summary>
        private static MinisterEditorController _ministerEditor;

        /// <summary>
        ///     研究機関エディタコントローラ
        /// </summary>
        private static TeamEditorController _teamEditor;

        /// <summary>
        ///     プロヴィンスエディタコントローラ
        /// </summary>
        private static ProvinceEditorController _provinceEditor;

        /// <summary>
        ///     技術ツリーエディタコントローラ
        /// </summary>
        private static TechEditorController _techEditor;

        /// <summary>
        ///     ユニットモデルエディタコントローラ
        /// </summary>
        private static UnitEditorController _unitEditor;

        /// <summary>
        ///     ゲーム設定エディタコントローラ
        /// </summary>
        private static MiscEditorController _miscEditor;

        /// <summary>
        ///     軍団名エディタコントローラ
        /// </summary>
        private static CorpsNameEditorController _corpsNameEditor;

        /// <summary>
        ///     ユニット名エディタコントローラ
        /// </summary>
        private static UnitNameEditorController _unitNameEditor;

        /// <summary>
        ///     モデル名エディタコントローラ
        /// </summary>
        private static ModelNameEditorController _modelNameEditor;

        /// <summary>
        ///     ランダム指揮官エディタコントローラ
        /// </summary>
        private static RandomLeaderEditorController _randomLeaderEditor;

        /// <summary>
        ///     研究速度ビューアコントローラ
        /// </summary>
        private static ResearchViewerController _researchViewer;

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private static ScenarioEditorController _scenarioEditor;

        /// <summary>
        ///     メインフォームを起動する
        /// </summary>
        internal static void LaunchMainForm()
        {
            _mainForm = new MainForm();
            Application.Run(_mainForm);
        }

        /// <summary>
        ///     指揮官エディタフォームを起動する
        /// </summary>
        internal static void LaunchLeaderEditorForm()
        {
            if (_leaderEditor == null)
            {
                _leaderEditor = new LeaderEditorController();
            }
            _leaderEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     閣僚エディタフォームを起動する
        /// </summary>
        internal static void LaunchMinisterEditorForm()
        {
            if (_ministerEditor == null)
            {
                _ministerEditor = new MinisterEditorController();
            }
            _ministerEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     研究機関エディタフォームを起動する
        /// </summary>
        internal static void LaunchTeamEditorForm()
        {
            if (_teamEditor == null)
            {
                _teamEditor = new TeamEditorController();
            }
            _teamEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     プロヴィンスエディタフォームを起動する
        /// </summary>
        internal static void LaunchProvinceEditorForm()
        {
            if (_provinceEditor == null)
            {
                _provinceEditor = new ProvinceEditorController();
            }
            _provinceEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     技術ツリーエディタフォームを起動する
        /// </summary>
        internal static void LaunchTechEditorForm()
        {
            if (_techEditor == null)
            {
                _techEditor = new TechEditorController();
            }
            _techEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ユニットモデルエディタフォームを起動する
        /// </summary>
        internal static void LaunchUnitEditorForm()
        {
            if (_unitNameEditor == null)
            {
                _unitEditor = new UnitEditorController();
            }
            _unitEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ゲーム設定エディタフォームを起動する
        /// </summary>
        internal static void LaunchMiscEditorForm()
        {
            if (_miscEditor == null)
            {
                _miscEditor = new MiscEditorController();
            }
            _miscEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     軍団名エディタフォームを起動する
        /// </summary>
        internal static void LaunchCorpsNameEditorForm()
        {
            if (_corpsNameEditor == null)
            {
                _corpsNameEditor = new CorpsNameEditorController();
            }
            _corpsNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ユニット名エディタフォームを起動する
        /// </summary>
        internal static void LaunchUnitNameEditorForm()
        {
            if (_unitNameEditor == null)
            {
                _unitNameEditor = new UnitNameEditorController();
            }
            _unitNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     モデル名エディタフォームを起動する
        /// </summary>
        internal static void LaunchModelNameEditorForm()
        {
            if (_modelNameEditor == null)
            {
                _modelNameEditor = new ModelNameEditorController();
            }
            _modelNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ランダム指揮官エディタフォームを起動する
        /// </summary>
        internal static void LaunchRandomLeaderEditorForm()
        {
            if (_randomLeaderEditor == null)
            {
                _randomLeaderEditor = new RandomLeaderEditorController();
            }
            _randomLeaderEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     研究速度ビューアフォームを起動する
        /// </summary>
        internal static void LaunchResearchViewerForm()
        {
            if (_researchViewer == null)
            {
                _researchViewer = new ResearchViewerController();
            }
            _researchViewer.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     シナリオエディタフォームを起動する
        /// </summary>
        internal static void LaunchScenarioEditorForm()
        {
            if (_scenarioEditor == null)
            {
                _scenarioEditor = new ScenarioEditorController();
            }
            _scenarioEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     エディタフォームの状態更新時の処理
        /// </summary>
        internal static void OnEditorStatusUpdate()
        {
            if (!ExistsEditorForms() && !IsLoadingData())
            {
                _mainForm.EnableFolderChange();
            }
            else
            {
                _mainForm.DisableFolderChange();
            }
        }

        /// <summary>
        ///     エディタのフォームが存在するかどうかを判定する
        /// </summary>
        /// <returns>エディタのフォームが存在すればtrueを返す</returns>
        private static bool ExistsEditorForms()
        {
            return (_leaderEditor != null && _leaderEditor.ExistsForm()) ||
                   (_ministerEditor != null && _ministerEditor.ExistsForm()) ||
                   (_teamEditor != null && _teamEditor.ExistsForm()) ||
                   (_provinceEditor != null && _provinceEditor.ExistsForm()) ||
                   (_techEditor != null && _techEditor.ExistsForm()) ||
                   (_unitNameEditor != null && _unitNameEditor.ExistsForm()) ||
                   (_miscEditor != null && _miscEditor.ExistsForm()) ||
                   (_corpsNameEditor != null && _corpsNameEditor.ExistsForm()) ||
                   (_unitNameEditor != null && _unitNameEditor.ExistsForm()) ||
                   (_modelNameEditor != null && _modelNameEditor.ExistsForm()) ||
                   (_randomLeaderEditor != null && _randomLeaderEditor.ExistsForm()) ||
                   (_researchViewer != null && _researchViewer.ExistsForm()) ||
                   (_scenarioEditor != null && _scenarioEditor.ExistsForm());
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