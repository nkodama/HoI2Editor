using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;

namespace HoI2Editor
{
    /// <summary>
    ///     アプリケーションコントローラクラス
    /// </summary>
    public static class HoI2Editor
    {
        #region 内部フィールド

        #endregion

        #region エディターのバージョン

        /// <summary>
        ///     エディターのバージョン
        /// </summary>
        private static string _version;

        /// <summary>
        ///     エディターのバージョン
        /// </summary>
        public static string Version
        {
            get { return _version; }
        }

        /// <summary>
        ///     エディターのバージョンを初期化する
        /// </summary>
        public static void InitVersion()
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

        #endregion

        #region データ処理

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
                   UnitNames.IsDirty() ||
                   DivisionNames.IsDirty() ||
                   RandomLeaders.IsDirty();
        }

        /// <summary>
        ///     データを再読み込みする
        /// </summary>
        public static void ReloadFiles()
        {
            // データを再読み込みする
            Misc.Reload();
            Config.Reload();
            Leaders.Reload();
            Ministers.Reload();
            Teams.Reload();
            Provinces.Reload();
            Techs.Reload();
            Units.Reload();
            UnitNames.Reload();
            DivisionNames.Reload();
            RandomLeaders.Reload();

            // データ読み込み後の更新処理呼び出し
            OnFileLoaded();
        }

        /// <summary>
        ///     データを保存する
        /// </summary>
        public static void SaveFiles()
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
            RandomLeaders.Save();

            // データ保存後の更新処理呼び出し
            OnFileSaved();
        }

        /// <summary>
        ///     データ読み込み後の更新処理呼び出し
        /// </summary>
        private static void OnFileLoaded()
        {
            if (_leaderEditorForm != null)
            {
                _leaderEditorForm.OnLeadersLoaded();
            }
            if (_ministerEditorForm != null)
            {
                _ministerEditorForm.OnMinistersLoaded();
            }
            if (_teamEditorForm != null)
            {
                _teamEditorForm.OnTeamsLoaded();
            }
            if (_provinceEditorForm != null)
            {
                _provinceEditorForm.OnProvincesLoaded();
            }
            if (_techEditorForm != null)
            {
                _techEditorForm.OnTechsLoaded();
            }
            if (_unitEditorForm != null)
            {
                _unitEditorForm.OnUnitsLoaded();
            }
            if (_miscEditorForm != null)
            {
                _miscEditorForm.OnMiscLoaded();
            }
            if (_unitNameEditorForm != null)
            {
                _unitNameEditorForm.OnUnitNamesLoaded();
            }
            if (_divisionNameEditorForm != null)
            {
                _divisionNameEditorForm.OnDivisionNamesLoaded();
            }
            if (_randomLeaderEditorForm != null)
            {
                _randomLeaderEditorForm.OnRandomLeadersLoaded();
            }
        }

        /// <summary>
        ///     データ保存後の更新処理呼び出し
        /// </summary>
        private static void OnFileSaved()
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

        #region エディターフォーム管理

        /// <summary>
        /// メインフォーム
        /// </summary>
        private static MainForm _mainForm;

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

        /// <summary>
        /// メインフォームを起動する
        /// </summary>
        public static void LaunchMainForm()
        {
            _mainForm = new MainForm();
            Application.Run(_mainForm);
        }

        /// <summary>
        ///     指揮官エディターフォームを起動する
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
        ///     閣僚エディターフォームを起動する
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
        ///     研究機関エディターフォームを起動する
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
        ///     プロヴィンスエディターフォームを起動する
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
        ///     技術ツリーエディターフォームを起動する
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
        ///     ユニットモデルエディターフォームを起動する
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
        ///     ゲーム設定エディターフォームを起動する
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
        ///     ユニット名エディターフォームを起動する
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
        ///     師団名エディターフォームを起動する
        /// </summary>
        public static void LaunchDivisionNameEditorForm()
        {
            if (_divisionNameEditorForm == null)
            {
                _divisionNameEditorForm = new DivisionNameEditorForm();
                _divisionNameEditorForm.Show();

                OnEditorStatusUpdete();
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

                OnEditorStatusUpdete();
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
        ///     ユニット名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnUnitNameEditorFormClosed()
        {
            _unitNameEditorForm = null;

            OnEditorStatusUpdete();
        }

        /// <summary>
        ///     師団名エディターフォームクローズ時の処理
        /// </summary>
        public static void OnDivisionNameEditorFormClosed()
        {
            _divisionNameEditorForm = null;

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
        /// エディターの状態更新時の処理
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
                _unitNameEditorForm == null &&
                _divisionNameEditorForm == null &&
                _randomLeaderEditorForm == null)
            {
                _mainForm.EnableFolderChange();
            }
            else
            {
                _mainForm.DisableFolderChange();
            }
        }

        #endregion
    }
}