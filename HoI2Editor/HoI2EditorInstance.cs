using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor
{
    /// <summary>
    ///     エディタインスタンス
    /// </summary>
    internal class HoI2EditorInstance
    {
        #region 内部フィールド

        /// <summary>
        ///     メインフォーム
        /// </summary>
        private MainForm _mainForm;

        /// <summary>
        ///     指揮官エディタコントローラ
        /// </summary>
        private LeaderEditorController _leaderEditor;

        /// <summary>
        ///     閣僚エディタコントローラ
        /// </summary>
        private MinisterEditorController _ministerEditor;

        /// <summary>
        ///     研究機関エディタコントローラ
        /// </summary>
        private TeamEditorController _teamEditor;

        /// <summary>
        ///     プロヴィンスエディタコントローラ
        /// </summary>
        private ProvinceEditorController _provinceEditor;

        /// <summary>
        ///     技術ツリーエディタコントローラ
        /// </summary>
        private TechEditorController _techEditor;

        /// <summary>
        ///     ユニットモデルエディタコントローラ
        /// </summary>
        private UnitEditorController _unitEditor;

        /// <summary>
        ///     ゲーム設定エディタコントローラ
        /// </summary>
        private MiscEditorController _miscEditor;

        /// <summary>
        ///     軍団名エディタコントローラ
        /// </summary>
        private CorpsNameEditorController _corpsNameEditor;

        /// <summary>
        ///     ユニット名エディタコントローラ
        /// </summary>
        private UnitNameEditorController _unitNameEditor;

        /// <summary>
        ///     モデル名エディタコントローラ
        /// </summary>
        private ModelNameEditorController _modelNameEditor;

        /// <summary>
        ///     ランダム指揮官エディタコントローラ
        /// </summary>
        private RandomLeaderEditorController _randomLeaderEditor;

        /// <summary>
        ///     研究速度ビューアコントローラ
        /// </summary>
        private ResearchViewerController _researchViewer;

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private ScenarioEditorController _scenarioEditor;

        #endregion

        #region データ処理

        /// <summary>
        ///     保存がキャンセルされたかどうか
        /// </summary>
        internal bool SaveCanceled { get; private set; }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
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
        internal void RequestReload()
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
        private void Reload()
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
        internal void Save()
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
        private void SaveFiles()
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
        ///     問い合わせてからデータを再読み込みする
        /// </summary>
        internal void QueryReload()
        {
            // 編集済みならば保存するかを問い合わせる
            if (IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, HoI2EditorController.Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        Save();
                        break;
                }
            }

            Reload();
        }

        /// <summary>
        ///     問い合わせてからデータを保存する
        /// </summary>
        /// <returns>キャンセルした場合はtrueを返す</returns>
        internal bool QuerySave()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return false;
            }

            // 保存するかを問い合わせる
            DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, HoI2EditorController.Name,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    return true;

                case DialogResult.Yes:
                    Save();
                    break;

                case DialogResult.No:
                    SaveCanceled = true;
                    break;
            }
            return false;
        }

        /// <summary>
        ///     データ読み込み後の更新処理呼び出し
        /// </summary>
        private void OnFileLoaded()
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
        private void OnFileSaved()
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
        ///     遅延読み込み完了後の更新処理呼び出し
        /// </summary>
        internal void OnLoadingCompleted()
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
        private bool IsLoadingData()
        {
            return Leaders.IsLoading() ||
                   Ministers.IsLoading() ||
                   Teams.IsLoading() ||
                   Provinces.IsLoading() ||
                   Techs.IsLoading() ||
                   Units.IsLoading() ||
                   Maps.IsLoading();
        }

        /// <summary>
        ///     編集項目の変更を通知する
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal void NotifyItemChange(EditorItemId id)
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

        #endregion

        #region フォーム管理

        /// <summary>
        ///     メインフォームを起動する
        /// </summary>
        internal void LaunchMainForm()
        {
            _mainForm = new MainForm(this);
            Application.Run(_mainForm);
        }

        /// <summary>
        ///     指揮官エディタフォームを起動する
        /// </summary>
        internal void LaunchLeaderEditorForm()
        {
            if (_leaderEditor == null)
            {
                _leaderEditor = new LeaderEditorController(this);
            }
            _leaderEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     閣僚エディタフォームを起動する
        /// </summary>
        internal void LaunchMinisterEditorForm()
        {
            if (_ministerEditor == null)
            {
                _ministerEditor = new MinisterEditorController(this);
            }
            _ministerEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     研究機関エディタフォームを起動する
        /// </summary>
        internal void LaunchTeamEditorForm()
        {
            if (_teamEditor == null)
            {
                _teamEditor = new TeamEditorController(this);
            }
            _teamEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     プロヴィンスエディタフォームを起動する
        /// </summary>
        internal void LaunchProvinceEditorForm()
        {
            if (_provinceEditor == null)
            {
                _provinceEditor = new ProvinceEditorController(this);
            }
            _provinceEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     技術ツリーエディタフォームを起動する
        /// </summary>
        internal void LaunchTechEditorForm()
        {
            if (_techEditor == null)
            {
                _techEditor = new TechEditorController(this);
            }
            _techEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ユニットモデルエディタフォームを起動する
        /// </summary>
        internal void LaunchUnitEditorForm()
        {
            if (_unitNameEditor == null)
            {
                _unitEditor = new UnitEditorController(this);
            }
            _unitEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ゲーム設定エディタフォームを起動する
        /// </summary>
        internal void LaunchMiscEditorForm()
        {
            if (_miscEditor == null)
            {
                _miscEditor = new MiscEditorController(this);
            }
            _miscEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     軍団名エディタフォームを起動する
        /// </summary>
        internal void LaunchCorpsNameEditorForm()
        {
            if (_corpsNameEditor == null)
            {
                _corpsNameEditor = new CorpsNameEditorController(this);
            }
            _corpsNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ユニット名エディタフォームを起動する
        /// </summary>
        internal void LaunchUnitNameEditorForm()
        {
            if (_unitNameEditor == null)
            {
                _unitNameEditor = new UnitNameEditorController(this);
            }
            _unitNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     モデル名エディタフォームを起動する
        /// </summary>
        internal void LaunchModelNameEditorForm()
        {
            if (_modelNameEditor == null)
            {
                _modelNameEditor = new ModelNameEditorController(this);
            }
            _modelNameEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     ランダム指揮官エディタフォームを起動する
        /// </summary>
        internal void LaunchRandomLeaderEditorForm()
        {
            if (_randomLeaderEditor == null)
            {
                _randomLeaderEditor = new RandomLeaderEditorController(this);
            }
            _randomLeaderEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     研究速度ビューアフォームを起動する
        /// </summary>
        internal void LaunchResearchViewerForm()
        {
            if (_researchViewer == null)
            {
                _researchViewer = new ResearchViewerController(this);
            }
            _researchViewer.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     シナリオエディタフォームを起動する
        /// </summary>
        internal void LaunchScenarioEditorForm()
        {
            if (_scenarioEditor == null)
            {
                _scenarioEditor = new ScenarioEditorController(this);
            }
            _scenarioEditor.OpenForm();

            OnEditorStatusUpdate();
        }

        /// <summary>
        ///     エディタフォームの状態更新時の処理
        /// </summary>
        internal void OnEditorStatusUpdate()
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
        private bool ExistsEditorForms()
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
    }
}