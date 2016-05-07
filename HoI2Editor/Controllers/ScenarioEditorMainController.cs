using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Pages;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     シナリオエディタのメインタブのコントローラクラス
    /// </summary>
    internal class ScenarioEditorMainController
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private readonly ScenarioEditorController _parent;

        /// <summary>
        ///     メインタブページ
        /// </summary>
        private ScenarioEditorMainPage _page;

        /// <summary>
        ///     編集項目更新済みフラグ
        /// </summary>
        private bool _updated;

        /// <summary>
        ///     主要国リスト
        /// </summary>
        private List<Country> _majorCountries;

        /// <summary>
        ///     主要国以外の選択可能国リスト
        /// </summary>
        private List<Country> _selectableCountries;

        /// <summary>
        ///     選択可能国以外の国家リスト
        /// </summary>
        private List<Country> _unselectableCountries;

        #endregion

        #region 公開定数

        /// <summary>
        ///     メインタブの項目ID
        /// </summary>
        internal enum ItemId
        {
            ScenarioName,
            ScenarioPanelName,
            ScenarioStartYear,
            ScenarioStartMonth,
            ScenarioStartDay,
            ScenarioEndYear,
            ScenarioEndMonth,
            ScenarioEndDay,
            ScenarioIncludeFolder,
            ScenarioBattleScenario,
            ScenarioFreeSelection,
            ScenarioAllowDiplomacy,
            ScenarioAllowProduction,
            ScenarioAllowTechnology,
            ScenarioAiAggressive,
            ScenarioDifficulty,
            ScenarioGameSpeed,
            MajorCountryNameKey,
            MajorCountryNameString,
            MajorFlagExt,
            MajorCountryDescKey,
            MajorCountryDescString,
            MajorPropaganada,
            MajorCountries,
            SelectableCountries,
            UnselectableCountries
        }

        /// <summary>
        ///     シナリオフォルダの種類
        /// </summary>
        internal enum FileType
        {
            Scenario, // シナリオフォルダ
            SaveGames // 保存ゲーム
        }

        /// <summary>
        ///     ゲームフォルダの種類
        /// </summary>
        internal enum FolderType
        {
            Vanilla, // バニラフォルダ
            Mod, // MODフォルダ
            Export // 保存フォルダ
        }

        #endregion

        #region 内部定数

        /// <summary>
        ///     編集項目の編集済みフラグ
        /// </summary>
        private static readonly object[] ItemDirtyFlags =
        {
            Scenario.ItemId.Name,
            Scenario.ItemId.PanelName,
            Scenario.ItemId.StartYear,
            Scenario.ItemId.StartMonth,
            Scenario.ItemId.StartDay,
            Scenario.ItemId.EndYear,
            Scenario.ItemId.EndMonth,
            Scenario.ItemId.EndDay,
            Scenario.ItemId.IncludeFolder,
            Scenario.ItemId.BattleScenario,
            Scenario.ItemId.FreeSelection,
            Scenario.ItemId.AllowDiplomacy,
            Scenario.ItemId.AllowProduction,
            Scenario.ItemId.AllowTechnology,
            Scenario.ItemId.AiAggressive,
            Scenario.ItemId.Difficulty,
            Scenario.ItemId.GameSpeed,
            MajorCountrySettings.ItemId.NameKey,
            MajorCountrySettings.ItemId.NameString,
            MajorCountrySettings.ItemId.FlagExt,
            MajorCountrySettings.ItemId.DescKey,
            MajorCountrySettings.ItemId.DescString,
            MajorCountrySettings.ItemId.PictureName,
            null,
            null,
            null
        };

        /// <summary>
        ///     編集項目の文字列
        /// </summary>
        private static readonly string[] ItemStrings =
        {
            "scenario name",
            "panel name",
            "scenario start year",
            "scenario start month",
            "scenario start day",
            "scenario end year",
            "scenario end month",
            "scenario end day",
            "include folder",
            "battle scenario",
            "free selection",
            "allow diplomacy",
            "allow production",
            "allow technology",
            "ai aggressive",
            "difficulty",
            "game speed",
            "major country name key",
            "major country name string",
            "major flag ext",
            "country desc key",
            "country desc string",
            "propaganda image",
            "",
            "",
            ""
        };

        /// <summary>
        ///     AIの攻撃性の文字列名
        /// </summary>
        private readonly TextId[] _aiAggressiveNames =
        {
            TextId.OptionAiAggressiveness1,
            TextId.OptionAiAggressiveness2,
            TextId.OptionAiAggressiveness3,
            TextId.OptionAiAggressiveness4,
            TextId.OptionAiAggressiveness5
        };

        /// <summary>
        ///     難易度の文字列名
        /// </summary>
        private readonly TextId[] _difficultyNames =
        {
            TextId.OptionDifficulty1,
            TextId.OptionDifficulty2,
            TextId.OptionDifficulty3,
            TextId.OptionDifficulty4,
            TextId.OptionDifficulty5
        };

        /// <summary>
        ///     ゲームスピードの文字列名
        /// </summary>
        private readonly TextId[] _gameSpeedNames =
        {
            TextId.OptionGameSpeed0,
            TextId.OptionGameSpeed1,
            TextId.OptionGameSpeed2,
            TextId.OptionGameSpeed3,
            TextId.OptionGameSpeed4,
            TextId.OptionGameSpeed5,
            TextId.OptionGameSpeed6,
            TextId.OptionGameSpeed7
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="parent">シナリオエディタコントローラ</param>
        internal ScenarioEditorMainController(ScenarioEditorController parent)
        {
            _parent = parent;

            InitTabPage();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     読み込みボタン押下時の処理
        /// </summary>
        /// <param name="fileName">シナリオファイル名</param>
        /// <param name="fileType">シナリオフォルダの種類</param>
        /// <param name="folderType">ゲームフォルダの種類</param>
        internal void OnLoadButtonClick(string fileName, FileType fileType, FolderType folderType)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            // 編集済みならば保存するかを問い合わせる
            if (Scenarios.IsLoaded() && _parent.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Resources.EditorScenario,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        _parent.Save();
                        break;
                }
            }

            string folderName = fileType == FileType.SaveGames
                ? Game.SaveGamesPathName
                : Game.ScenarioPathName;
            string pathName = folderType == FolderType.Export
                ? Game.GetExportFileName(folderName, fileName)
                : folderType == FolderType.Mod
                    ? Game.GetModFileName(folderName, fileName)
                    : Game.GetVanillaFileName(folderName, fileName);

            // シナリオファイルを読み込む
            if (File.Exists(pathName))
            {
                Scenarios.Load(pathName);
            }

            // データ読み込み後の処理
            _parent.OnFileLoaded();
        }

        #endregion

        #region タブページ管理

        /// <summary>
        ///     タブページを初期化する
        /// </summary>
        private void InitTabPage()
        {
            _page = new ScenarioEditorMainPage(this);
            _parent.SetTabPage(_page, (int) ScenarioEditorForm.TabPageNo.Main);

            _page.UpdateScenarioListBox();
        }

        /// <summary>
        ///     タブページの更新を予約する
        /// </summary>
        internal void ReserveUpdate()
        {
            _updated = false;
        }

        /// <summary>
        ///     タブページを更新する
        /// </summary>
        internal void UpdateTabPage()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 編集項目更新済みならば何もしない
            if (_updated)
            {
                return;
            }

            // 編集項目を更新する
            UpdateItems();

            // 編集項目更新済みフラグをセットする
            _updated = true;
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateItems()
        {
            // 編集項目を更新する
            UpdateScenarioInfoItems();
            UpdateScenarioOptionItems();

            // 選択可能国リストを更新する
            UpdateSelectableList();

            // メインタブの編集項目を有効化する
            _page.EnableItems();
        }

        /// <summary>
        ///     シナリオ情報の編集項目を更新する
        /// </summary>
        private void UpdateScenarioInfoItems()
        {
            UpdateItemValue(ItemId.ScenarioName);
            UpdateItemValue(ItemId.ScenarioPanelName);
            UpdateItemValue(ItemId.ScenarioStartYear);
            UpdateItemValue(ItemId.ScenarioStartMonth);
            UpdateItemValue(ItemId.ScenarioStartDay);
            UpdateItemValue(ItemId.ScenarioEndYear);
            UpdateItemValue(ItemId.ScenarioEndMonth);
            UpdateItemValue(ItemId.ScenarioEndDay);
            UpdateItemValue(ItemId.ScenarioIncludeFolder);

            UpdateItemColor(ItemId.ScenarioName);
            UpdateItemColor(ItemId.ScenarioPanelName);
            UpdateItemColor(ItemId.ScenarioStartYear);
            UpdateItemColor(ItemId.ScenarioStartMonth);
            UpdateItemColor(ItemId.ScenarioStartDay);
            UpdateItemColor(ItemId.ScenarioEndYear);
            UpdateItemColor(ItemId.ScenarioEndMonth);
            UpdateItemColor(ItemId.ScenarioEndDay);
            UpdateItemColor(ItemId.ScenarioIncludeFolder);
        }

        /// <summary>
        ///     シナリオオプションの編集項目を更新する
        /// </summary>
        private void UpdateScenarioOptionItems()
        {
            UpdateItemValueList(ItemId.ScenarioAiAggressive);
            UpdateItemValueList(ItemId.ScenarioDifficulty);
            UpdateItemValueList(ItemId.ScenarioGameSpeed);

            UpdateItemValue(ItemId.ScenarioBattleScenario);
            UpdateItemValue(ItemId.ScenarioFreeSelection);
            UpdateItemValue(ItemId.ScenarioAllowDiplomacy);
            UpdateItemValue(ItemId.ScenarioAllowProduction);
            UpdateItemValue(ItemId.ScenarioAllowTechnology);
            UpdateItemValue(ItemId.ScenarioAiAggressive);
            UpdateItemValue(ItemId.ScenarioDifficulty);
            UpdateItemValue(ItemId.ScenarioGameSpeed);

            UpdateItemColor(ItemId.ScenarioBattleScenario);
            UpdateItemColor(ItemId.ScenarioFreeSelection);
            UpdateItemColor(ItemId.ScenarioAllowDiplomacy);
            UpdateItemColor(ItemId.ScenarioAllowProduction);
            UpdateItemColor(ItemId.ScenarioAllowTechnology);
            UpdateItemColor(ItemId.ScenarioAiAggressive);
            UpdateItemColor(ItemId.ScenarioDifficulty);
            UpdateItemColor(ItemId.ScenarioGameSpeed);
        }

        /// <summary>
        ///     編集項目の値更新時の処理
        /// </summary>
        /// <param name="control">対象コントロール</param>
        internal void OnItemValueChanged(Control control)
        {
            ItemId itemId = (ItemId) control.Tag;

            switch (itemId)
            {
                case ItemId.ScenarioStartYear:
                case ItemId.ScenarioStartMonth:
                case ItemId.ScenarioStartDay:
                case ItemId.ScenarioEndYear:
                case ItemId.ScenarioEndMonth:
                case ItemId.ScenarioEndDay:
                    OnScenarioIntItemValueChanged((TextBox) control, itemId);
                    break;

                case ItemId.ScenarioName:
                case ItemId.ScenarioPanelName:
                case ItemId.ScenarioIncludeFolder:
                    OnScenarioStringItemValueChanged((TextBox) control, itemId);
                    break;

                case ItemId.ScenarioAiAggressive:
                case ItemId.ScenarioDifficulty:
                case ItemId.ScenarioGameSpeed:
                    OnScenarioSelectionItemValueChanged((ComboBox) control, itemId);
                    break;

                case ItemId.ScenarioBattleScenario:
                case ItemId.ScenarioFreeSelection:
                case ItemId.ScenarioAllowDiplomacy:
                case ItemId.ScenarioAllowProduction:
                case ItemId.ScenarioAllowTechnology:
                    OnScenarioBoolItemValueChanged((CheckBox) control, itemId);
                    break;

                case ItemId.MajorCountryNameKey:
                case ItemId.MajorCountryNameString:
                case ItemId.MajorFlagExt:
                case ItemId.MajorCountryDescKey:
                case ItemId.MajorCountryDescString:
                case ItemId.MajorPropaganada:
                    OnMajorStringItemValueChanged((TextBox) control, itemId);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値更新時の処理 - シナリオ情報/整数値
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void OnScenarioIntItemValueChanged(TextBox control, ItemId itemId)
        {
            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                UpdateItemValue(itemId);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = GetItemValue(itemId);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!IsItemValueValid(itemId, val))
            {
                UpdateItemValue(itemId);
                return;
            }

            // ログ出力
            OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            PreItemChanged(itemId, val);

            // 値を更新する
            SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            SetItemDirty(itemId);

            // 文字色を更新する
            UpdateItemColor(itemId);

            // 項目値変更後の処理
            PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     編集項目の値更新時の処理 - シナリオ情報/文字列値
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void OnScenarioStringItemValueChanged(TextBox control, ItemId itemId)
        {
            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) GetItemValue(itemId)))
            {
                return;
            }

            // ログ出力
            OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            PreItemChanged(itemId, val);

            // 値を更新する
            SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            SetItemDirty(itemId);

            // 文字色を更新する
            UpdateItemColor(itemId);

            // 項目値変更後の処理
            PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     編集項目の値更新時の処理 - シナリオ情報/選択値
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void OnScenarioSelectionItemValueChanged(ComboBox control, ItemId itemId)
        {
            // 選択項目がなければ何もしない
            if (control.SelectedIndex < 0)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int val = control.SelectedIndex;
            if (val == (int) GetItemValue(itemId))
            {
                return;
            }

            // ログ出力
            OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            PreItemChanged(itemId, val);

            // 値を更新する
            SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            SetItemDirty(itemId);

            // 文字色を更新する
            UpdateItemColor(itemId);

            // 項目値変更後の処理
            PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     編集項目の値更新時の処理 - シナリオ情報/真偽値
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void OnScenarioBoolItemValueChanged(CheckBox control, ItemId itemId)
        {
            // 値に変化がなければ何もしない
            bool val = control.Checked;
            object prev = GetItemValue(itemId);
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            // ログ出力
            OutputItemValueChangedLog(itemId, val);

            // 項目値変更前の処理
            PreItemChanged(itemId, val);

            // 値を更新する
            SetItemValue(itemId, val);

            // 編集済みフラグを設定する
            SetItemDirty(itemId);

            // 文字色を更新する
            UpdateItemColor(itemId);

            // 項目値変更後の処理
            PostItemChanged(itemId, val);
        }

        /// <summary>
        ///     編集項目の値更新時の処理 - 主要国設定/文字列値
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void OnMajorStringItemValueChanged(TextBox control, ItemId itemId)
        {
            // 選択項目がなければ何もしない
            int index = _page.GetSelectedMajorCountryIndex();
            if (index < 0)
            {
                return;
            }
            MajorCountrySettings major = Scenarios.Data.Header.MajorCountries[index];

            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) GetItemValue(itemId, major)))
            {
                return;
            }

            // ログ出力
            OutputItemValueChangedLog(itemId, val, major);

            // 項目値変更前の処理
            PreItemChanged(itemId, val, major);

            // 値を更新する
            SetItemValue(itemId, val, major);

            // 編集済みフラグを設定する
            SetItemDirty(itemId, major);

            // 文字色を更新する
            UpdateItemColor(itemId, major);

            // 項目値変更後の処理
            PostItemChanged(itemId, val, major);
        }

        #endregion

        #region 主要国情報

        /// <summary>
        ///     選択可能国リストを更新する
        /// </summary>
        private void UpdateSelectableList()
        {
            // 主要国リスト
            ScenarioHeader header = Scenarios.Data.Header;
            _majorCountries = header.MajorCountries.Select(major => major.Country).ToList();

            // 選択可能国リスト
            _selectableCountries =
                header.SelectableCountries.Where(country => !_majorCountries.Contains(country)).ToList();

            // 非選択可能国リスト
            _unselectableCountries =
                Countries.Tags.Where(country => !header.SelectableCountries.Contains(country)).ToList();

            // リストボックスの項目を更新する
            UpdateItemValueList(ItemId.MajorCountries);
            UpdateItemValueList(ItemId.SelectableCountries);
            UpdateItemValueList(ItemId.UnselectableCountries);

            // 主要国の操作ボタンを無効化する
            _page.DisableMajorButtons();

            // 編集項目を無効化する
            _page.DisableSelectableItems();

            // 編集項目をクリアする
            _page.ClearSelectableItems();
        }

        /// <summary>
        ///     選択可能国の編集項目を更新する
        /// </summary>
        /// <param name="major">主要国設定</param>
        private void UpdateSelectableItems(MajorCountrySettings major)
        {
            UpdateItemValue(ItemId.MajorCountryNameKey, major);
            UpdateItemValue(ItemId.MajorCountryNameString, major);
            UpdateItemValue(ItemId.MajorFlagExt, major);
            UpdateItemValue(ItemId.MajorCountryDescKey, major);
            UpdateItemValue(ItemId.MajorCountryDescString, major);
            UpdateItemValue(ItemId.MajorPropaganada, major);

            UpdateItemColor(ItemId.MajorCountryNameKey, major);
            UpdateItemColor(ItemId.MajorCountryNameString, major);
            UpdateItemColor(ItemId.MajorFlagExt, major);
            UpdateItemColor(ItemId.MajorCountryDescKey, major);
            UpdateItemColor(ItemId.MajorCountryDescString, major);
            UpdateItemColor(ItemId.MajorPropaganada, major);
        }

        /// <summary>
        ///     選択中の主要国変更時の処理
        /// </summary>
        /// <param name="index">主要国リストのインデックス</param>
        internal void OnSelectedMajorCountryChanged(int index)
        {
            // 選択項目がなければ編集項目を無効化する
            if (index < 0)
            {
                // 主要国の操作ボタンを無効化する
                _page.DisableMajorButtons();

                // 選択可能国の編集項目を無効化する
                _page.DisableSelectableItems();

                // 選択可能国の編集項目をクリアする
                _page.ClearSelectableItems();
                return;
            }

            MajorCountrySettings major = Scenarios.Data.Header.MajorCountries[index];

            // 編集項目を更新する
            UpdateSelectableItems(major);

            // 選択可能国の編集項目を有効化する
            _page.EnableSelectableItems();

            // 主要国の操作ボタンを有効化する
            _page.EnableMajorButtons();
        }

        /// <summary>
        ///     選択中の主要国を1つ上へ移動する
        /// </summary>
        /// <param name="index">主要国リストのインデックス</param>
        internal void MoveUpMajorCountry(int index)
        {
            // 選択項目がなければ何もしない
            if (index < 0)
            {
                return;
            }

            // 主要国リストの項目を移動する
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;
            MajorCountrySettings major = majors[index];
            majors.RemoveAt(index);
            majors.Insert(index - 1, major);

            _majorCountries.RemoveAt(index);
            _majorCountries.Insert(index - 1, major.Country);

            // 主要国リストボックスの項目を移動する
            _page.MoveMajorListItem(index, index - 1);

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(major.Country);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     選択中の主要国を1つ下へ移動する
        /// </summary>
        /// <param name="index">主要国リストのインデックス</param>
        internal void MoveDownMajorCountry(int index)
        {
            // 選択項目がなければ何もしない
            if (index < 0)
            {
                return;
            }

            // 主要国リストの項目を移動する
            Scenario scenario = Scenarios.Data;
            List<MajorCountrySettings> majors = scenario.Header.MajorCountries;
            MajorCountrySettings major = majors[index];
            majors.RemoveAt(index);
            majors.Insert(index + 1, major);

            _majorCountries.RemoveAt(index);
            _majorCountries.Insert(index + 1, major.Country);

            // 主要国リストボックスの項目を移動する
            _page.MoveMajorListItem(index, index + 1);

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(major.Country);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     主要国リストに項目を追加する
        /// </summary>
        /// <param name="indices">選択国リストのインデックス</param>
        internal void AddMajorCountries(List<int> indices)
        {
            // 選択項目がなければ何もしない
            if (indices == null || indices.Any())
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries = (from int index in indices select _selectableCountries[index]).ToList();
            foreach (Country country in countries)
            {
                // 主要国リストボックスに追加する
                _page.AddMajorListItem(country);

                // 主要国リストに追加する
                MajorCountrySettings major = new MajorCountrySettings { Country = country };
                header.MajorCountries.Add(major);
                _majorCountries.Add(country);

                // 選択可能国リストボックスから削除する
                int index = _selectableCountries.IndexOf(country);
                _page.RemoveSelectableListItem(index);

                // 選択可能国リストから削除する
                _selectableCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] major country: +{0}", Countries.Strings[(int) country]);
            }

            // 主要国リストボックスに追加した項目を選択する
            _page.SelectMajorListItem(_majorCountries.Count - 1);
        }

        /// <summary>
        ///     主要国リストから項目を削除する
        /// </summary>
        /// <param name="index">主要国リストのインデックス</param>
        internal void RemoveMajorCountry(int index)
        {
            // 選択項目がなければ何もしない
            if (index < 0)
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;
            Country country = header.MajorCountries[index].Country;

            // 編集済みフラグを設定する
            scenario.SetDirtySelectableCountry(country);
            Scenarios.SetDirty();

            // 主要国リストボックスから削除する
            _page.RemoveMajorListItem(index);

            // 主要国リストから削除する
            header.MajorCountries.RemoveAt(index);

            // 選択可能国リストボックスに追加する
            index = _selectableCountries.FindIndex(c => c > country);
            if (index < 0)
            {
                index = _selectableCountries.Count;
            }
            _page.InsertSelectableListItem(index, country);
            _selectableCountries.Insert(index, country);

            Log.Info("[Scenario] major country: -{0}", Countries.Strings[(int) country]);

            // 主要国リストボックスの次の項目を選択する
            _page.SelectMajorListItem(index < _majorCountries.Count ? index : index - 1);
        }

        /// <summary>
        ///     選択国リストに項目を追加する
        /// </summary>
        /// <param name="indices">非選択国リストのインデックス</param>
        internal void AddSelectableCountries(List<int> indices)
        {
            // 選択項目がなければ何もしない
            if (indices == null || indices.Any())
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries = indices.Select(index => _unselectableCountries[index]).ToList();
            foreach (Country country in countries)
            {
                // 選択可能国リストボックスに追加する
                int index = _selectableCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _selectableCountries.Count;
                }
                _page.InsertSelectableListItem(index, country);
                _selectableCountries.Insert(index, country);

                // 選択可能国リストに追加する
                index = header.SelectableCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = header.SelectableCountries.Count;
                }
                header.SelectableCountries.Insert(index, country);

                // 非選択国リストボックスから削除する
                index = _unselectableCountries.IndexOf(country);
                _page.RemoveUnselectableListItem(index);
                _unselectableCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] selectable country: +{0}", Countries.Strings[(int) country]);
            }
        }

        /// <summary>
        ///     選択国リストから項目を削除する
        /// </summary>
        /// <param name="indices">選択国リストのインデックス</param>
        internal void RemoveSelectableCountries(List<int> indices)
        {
            // 選択項目がなければ何もしない
            if (indices == null || indices.Any())
            {
                return;
            }

            Scenario scenario = Scenarios.Data;
            ScenarioHeader header = scenario.Header;

            List<Country> countries = indices.Select(index => _selectableCountries[index]).ToList();
            foreach (Country country in countries)
            {
                // 非選択国リストボックスに追加する
                int index = _unselectableCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _unselectableCountries.Count;
                }
                _page.InsertUnselectableListItem(index, country);
                _unselectableCountries.Insert(index, country);

                // 選択可能国リストボックスから削除する
                index = _selectableCountries.IndexOf(country);
                _page.RemoveSelectableListItem(index);
                _selectableCountries.RemoveAt(index);

                // 選択可能国リストから削除する
                header.SelectableCountries.Remove(country);

                // 編集済みフラグを設定する
                scenario.SetDirtySelectableCountry(country);
                Scenarios.SetDirty();

                Log.Info("[Scenario] selectable country: -{0}", Countries.Strings[(int) country]);
            }
        }

        /// <summary>
        ///     主要国/選択国/非選択国リストの項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="index">リストのインデックス</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirtyCountry(ItemId itemId, int index)
        {
            switch (itemId)
            {
                case ItemId.MajorCountries:
                    return Scenarios.Data.IsDirtySelectableCountry(_majorCountries[index]);

                case ItemId.SelectableCountries:
                    return Scenarios.Data.IsDirtySelectableCountry(_selectableCountries[index]);

                case ItemId.UnselectableCountries:
                    return Scenarios.Data.IsDirtySelectableCountry(_unselectableCountries[index]);
            }
            return false;
        }

        #endregion

        #region 項目値更新

        /// <summary>
        ///     編集項目の値を更新する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        private void UpdateItemValue(ItemId itemId)
        {
            Control control = _page.GetItemControl(itemId);
            if (control is TextBox)
            {
                control.Text = ObjectHelper.ToString(GetItemValue(itemId));
            }
            else if (control is ComboBox)
            {
                ((ComboBox) control).SelectedIndex = (int) GetItemValue(itemId);
            }
            else if (control is CheckBox)
            {
                ((CheckBox) control).Checked = (bool) GetItemValue(itemId);
            }

            // パネル画像を更新する
            if (itemId == ItemId.ScenarioPanelName)
            {
                _page.UpdatePanelImage((string) GetItemValue(itemId));
            }
        }

        /// <summary>
        ///     編集項目の値を更新する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        private void UpdateItemValue(ItemId itemId, MajorCountrySettings major)
        {
            Control control = _page.GetItemControl(itemId);
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, major));

            switch (itemId)
            {
                case ItemId.MajorCountryNameString:
                    // 主要国設定の国名定義がなければ編集不可
                    ((TextBox) control).ReadOnly = string.IsNullOrEmpty(major.Name);
                    break;

                case ItemId.MajorCountryDescString:
                    // 主要国設定の説明文定義がなければ編集不可
                    ((TextBox) control).ReadOnly = string.IsNullOrEmpty(major.Desc);
                    break;

                case ItemId.MajorPropaganada:
                    // プロパガンダ画像を更新する
                    _page.UpdatePropagandaImage(major.Country, major.PictureName);
                    break;
            }
        }

        #endregion

        #region 項目色更新

        /// <summary>
        ///     編集項目の色を更新する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        private void UpdateItemColor(ItemId itemId)
        {
            Control control = _page.GetItemControl(itemId);
            control.ForeColor = IsItemDirty(itemId) ? Color.Red : SystemColors.WindowText;

            // 項目色を変更するため描画更新する
            switch (itemId)
            {
                case ItemId.ScenarioAiAggressive:
                case ItemId.ScenarioDifficulty:
                case ItemId.ScenarioGameSpeed:
                    control.Refresh();
                    break;
            }
        }

        /// <summary>
        ///     編集項目の色を更新する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        private void UpdateItemColor(ItemId itemId, MajorCountrySettings major)
        {
            Control control = _page.GetItemControl(itemId);
            control.ForeColor = IsItemDirty(itemId, major) ? Color.Red : SystemColors.WindowText;
        }

        #endregion

        #region 項目値取得

        /// <summary>
        ///     編集項目の値を取得する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>編集項目の値</returns>
        internal object GetItemValue(ItemId itemId)
        {
            Scenario scenario = Scenarios.Data;

            switch (itemId)
            {
                case ItemId.ScenarioName:
                    return Config.ExistsKey(scenario.Name) ? Config.GetText(scenario.Name) : scenario.Name;

                case ItemId.ScenarioPanelName:
                    return scenario.PanelName;

                case ItemId.ScenarioStartYear:
                    return scenario.GlobalData.StartDate?.Year;

                case ItemId.ScenarioStartMonth:
                    return scenario.GlobalData.StartDate?.Month;

                case ItemId.ScenarioStartDay:
                    return scenario.GlobalData.StartDate?.Day;

                case ItemId.ScenarioEndYear:
                    return scenario.GlobalData.EndDate?.Year;

                case ItemId.ScenarioEndMonth:
                    return scenario.GlobalData.EndDate?.Month;

                case ItemId.ScenarioEndDay:
                    return scenario.GlobalData.EndDate?.Day;

                case ItemId.ScenarioIncludeFolder:
                    return scenario.IncludeFolder;

                case ItemId.ScenarioBattleScenario:
                    return scenario.Header.IsBattleScenario;

                case ItemId.ScenarioFreeSelection:
                    return scenario.Header.IsFreeSelection;

                case ItemId.ScenarioAllowDiplomacy:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowDiplomacy;

                case ItemId.ScenarioAllowProduction:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowProduction;

                case ItemId.ScenarioAllowTechnology:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowTechnology;

                case ItemId.ScenarioAiAggressive:
                    return scenario.Header.AiAggressive;

                case ItemId.ScenarioDifficulty:
                    return scenario.Header.Difficulty;

                case ItemId.ScenarioGameSpeed:
                    return scenario.Header.GameSpeed;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <returns>編集項目の値</returns>
        private object GetItemValue(ItemId itemId, MajorCountrySettings major)
        {
            switch (itemId)
            {
                case ItemId.MajorCountryNameKey:
                    return major.Name;

                case ItemId.MajorCountryNameString:
                    return Scenarios.GetCountryName(major.Country);

                case ItemId.MajorFlagExt:
                    return major.FlagExt;

                case ItemId.MajorCountryDescKey:
                    return major.Desc;

                case ItemId.MajorCountryDescString:
                    if (!string.IsNullOrEmpty(major.Desc))
                    {
                        return Config.GetText(major.Desc);
                    }
                    int year = Scenarios.Data.GlobalData.StartDate != null
                        ? Scenarios.Data.GlobalData.StartDate.Year
                        : Scenarios.Data.Header.StartDate != null
                            ? Scenarios.Data.Header.StartDate.Year
                            : Scenarios.Data.Header.StartYear;
                    // 年数の下2桁のみ使用する
                    year = year % 100;
                    // 年数別の説明があれば使用する
                    string key = $"{major.Country}_{year}_DESC";
                    if (Config.ExistsKey(key))
                    {
                        return Config.GetText(key);
                    }
                    key = $"{major.Country}_DESC";
                    return Config.GetText(key);

                case ItemId.MajorPropaganada:
                    return major.PictureName;
            }

            return null;
        }

        #endregion

        #region 項目値設定

        /// <summary>
        ///     編集項目の値を設定する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        private void SetItemValue(ItemId itemId, object val)
        {
            Scenario scenario = Scenarios.Data;

            switch (itemId)
            {
                case ItemId.ScenarioName:
                    if (Config.ExistsKey(scenario.Name))
                    {
                        Config.SetText(scenario.Name, (string) val, Game.ScenarioTextFileName);
                    }
                    else
                    {
                        scenario.Name = (string) val;
                    }
                    break;

                case ItemId.ScenarioPanelName:
                    scenario.PanelName = (string) val;
                    break;

                case ItemId.ScenarioStartYear:
                    scenario.GlobalData.StartDate.Year = (int) val;
                    break;

                case ItemId.ScenarioStartMonth:
                    scenario.GlobalData.StartDate.Month = (int) val;
                    break;

                case ItemId.ScenarioStartDay:
                    scenario.GlobalData.StartDate.Day = (int) val;
                    break;

                case ItemId.ScenarioEndYear:
                    scenario.GlobalData.EndDate.Year = (int) val;
                    break;

                case ItemId.ScenarioEndMonth:
                    scenario.GlobalData.EndDate.Month = (int) val;
                    break;

                case ItemId.ScenarioEndDay:
                    scenario.GlobalData.EndDate.Day = (int) val;
                    break;

                case ItemId.ScenarioIncludeFolder:
                    scenario.IncludeFolder = (string) val;
                    break;

                case ItemId.ScenarioBattleScenario:
                    scenario.Header.IsBattleScenario = (bool) val;
                    break;

                case ItemId.ScenarioFreeSelection:
                    scenario.Header.IsFreeSelection = (bool) val;
                    break;

                case ItemId.ScenarioAllowDiplomacy:
                    scenario.GlobalData.Rules.AllowDiplomacy = (bool) val;
                    break;

                case ItemId.ScenarioAllowProduction:
                    scenario.GlobalData.Rules.AllowProduction = (bool) val;
                    break;

                case ItemId.ScenarioAllowTechnology:
                    scenario.GlobalData.Rules.AllowTechnology = (bool) val;
                    break;

                case ItemId.ScenarioAiAggressive:
                    scenario.Header.AiAggressive = (int) val;
                    break;

                case ItemId.ScenarioDifficulty:
                    scenario.Header.Difficulty = (int) val;
                    break;

                case ItemId.ScenarioGameSpeed:
                    scenario.Header.GameSpeed = (int) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <param name="val">編集項目の値</param>
        private void SetItemValue(ItemId itemId, object val, MajorCountrySettings major)
        {
            switch (itemId)
            {
                case ItemId.MajorCountryNameKey:
                    major.Name = (string) val;
                    break;

                case ItemId.MajorCountryNameString:
                    // 主要国設定の国名
                    if (!string.IsNullOrEmpty(major.Name))
                    {
                        Config.SetText(major.Name, (string) val, Game.WorldTextFileName);
                        break;
                    }
                    // 国家設定の国名
                    CountrySettings settings = Scenarios.GetCountrySettings(major.Country);
                    if (!string.IsNullOrEmpty(settings?.Name))
                    {
                        Config.SetText(settings.Name, (string) val, Game.WorldTextFileName);
                        break;
                    }
                    // 標準の国名
                    Config.SetText(Countries.Strings[(int) major.Country], (string) val, Game.WorldTextFileName);
                    break;

                case ItemId.MajorFlagExt:
                    major.FlagExt = (string) val;
                    break;

                case ItemId.MajorCountryDescKey:
                    major.Desc = (string) val;
                    break;

                case ItemId.MajorCountryDescString:
                    // 主要国設定の説明文
                    if (!string.IsNullOrEmpty(major.Desc))
                    {
                        Config.SetText(major.Desc, (string) val, Game.ScenarioTextFileName);
                        break;
                    }
                    int year = Scenarios.Data.GlobalData.StartDate != null
                        ? Scenarios.Data.GlobalData.StartDate.Year
                        : Scenarios.Data.Header.StartDate != null
                            ? Scenarios.Data.Header.StartDate.Year
                            : Scenarios.Data.Header.StartYear;
                    // 年数の下2桁のみ使用する
                    year = year % 100;
                    // 年数別の説明文
                    string key = $"{major.Country}_{year}_DESC";
                    if (Config.ExistsKey(key))
                    {
                        Config.SetText(key, (string) val, Game.ScenarioTextFileName);
                        break;
                    }
                    // 標準の説明文
                    key = $"{major.Country}_DESC";
                    Config.SetText(key, (string) val, Game.ScenarioTextFileName);
                    break;

                case ItemId.MajorPropaganada:
                    major.PictureName = (string) val;
                    break;
            }
        }

        #endregion

        #region 有効値判定

        /// <summary>
        ///     編集項目の値が有効かどうかを判定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        private bool IsItemValueValid(ItemId itemId, object val)
        {
            switch (itemId)
            {
                case ItemId.ScenarioStartYear:
                case ItemId.ScenarioEndYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ItemId.ScenarioStartMonth:
                case ItemId.ScenarioEndMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ItemId.ScenarioStartDay:
                case ItemId.ScenarioEndDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        #endregion

        #region リスト項目更新

        /// <summary>
        ///     リスト項目の値を更新する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        private void UpdateItemValueList(ItemId itemId)
        {
            Control control = _page.GetItemControl(itemId);

            switch (itemId)
            {
                case ItemId.ScenarioAiAggressive:
                case ItemId.ScenarioDifficulty:
                case ItemId.ScenarioGameSpeed:
                    UpdateItemValueList((ComboBox) control, itemId);
                    break;

                case ItemId.MajorCountries:
                case ItemId.SelectableCountries:
                case ItemId.UnselectableCountries:
                    UpdateItemValueList((ListBox) control, itemId);
                    break;
            }
        }

        /// <summary>
        ///     リスト項目の値を更新する - コンボボックス
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void UpdateItemValueList(ComboBox control, ItemId itemId)
        {
            control.BeginUpdate();
            control.Items.Clear();
            foreach (string s in GetItemValueList(itemId))
            {
                control.Items.Add(s);
            }
            control.EndUpdate();
        }

        /// <summary>
        ///     リスト項目の値を更新する - リストボックス
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="itemId">項目ID</param>
        private void UpdateItemValueList(ListBox control, ItemId itemId)
        {
            control.BeginUpdate();
            control.Items.Clear();
            foreach (string s in GetItemValueList(itemId))
            {
                control.Items.Add(s);
            }
            control.EndUpdate();
        }

        #endregion

        #region リスト項目取得

        /// <summary>
        ///     リスト項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>リスト項目の値</returns>
        private IEnumerable<string> GetItemValueList(ItemId itemId)
        {
            switch (itemId)
            {
                case ItemId.ScenarioAiAggressive:
                    return _aiAggressiveNames.Select(Config.GetText);

                case ItemId.ScenarioDifficulty:
                    return _difficultyNames.Select(Config.GetText);

                case ItemId.ScenarioGameSpeed:
                    return _gameSpeedNames.Select(Config.GetText);

                case ItemId.MajorCountries:
                    return Scenarios.Data.Header.MajorCountries.Select(major => Countries.GetTagName(major.Country));

                case ItemId.SelectableCountries:
                    IEnumerable<Country> majors = Scenarios.Data.Header.MajorCountries.Select(major => major.Country);
                    return Scenarios.Data.Header.SelectableCountries.Where(country => !majors.Contains(country))
                        .Select(Countries.GetTagName);

                case ItemId.UnselectableCountries:
                    return Countries.Tags.Where(country => !Scenarios.Data.Header.SelectableCountries.Contains(country))
                        .Select(Countries.GetTagName);
            }
            return null;
        }

        #endregion

        #region 編集済みフラグ取得

        /// <summary>
        ///     編集項目の編集済みフラグを取得する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>編集済みフラグ</returns>
        internal bool IsItemDirty(ItemId itemId)
        {
            return Scenarios.Data.IsDirty((Scenario.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <returns>編集済みフラグ</returns>
        private bool IsItemDirty(ItemId itemId, MajorCountrySettings major)
        {
            return major.IsDirty((MajorCountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        #endregion

        #region 編集済みフラグ設定

        /// <summary>
        ///     編集項目の編集済みフラグを設定する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        private void SetItemDirty(ItemId itemId)
        {
            Scenarios.Data.SetDirty((Scenario.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        private void SetItemDirty(ItemId itemId, MajorCountrySettings major)
        {
            major.SetDirty((MajorCountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        #endregion

        #region 変更前処理

        /// <summary>
        ///     項目値変更前の処理 - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        private void PreItemChanged(ItemId itemId, object val)
        {
            switch (itemId)
            {
                case ItemId.ScenarioStartYear:
                    PreItemChangedScenarioStartDate(ItemId.ScenarioStartMonth, ItemId.ScenarioStartDay);
                    break;

                case ItemId.ScenarioStartMonth:
                    PreItemChangedScenarioStartDate(ItemId.ScenarioStartYear, ItemId.ScenarioStartDay);
                    break;

                case ItemId.ScenarioStartDay:
                    PreItemChangedScenarioStartDate(ItemId.ScenarioStartYear, ItemId.ScenarioStartMonth);
                    break;

                case ItemId.ScenarioEndYear:
                    PreItemChangedScenarioEndDate(ItemId.ScenarioEndMonth, ItemId.ScenarioEndDay);
                    break;

                case ItemId.ScenarioEndMonth:
                    PreItemChangedScenarioEndDate(ItemId.ScenarioEndYear, ItemId.ScenarioEndDay);
                    break;

                case ItemId.ScenarioEndDay:
                    PreItemChangedScenarioEndDate(ItemId.ScenarioEndYear, ItemId.ScenarioEndMonth);
                    break;

                case ItemId.ScenarioAllowDiplomacy:
                case ItemId.ScenarioAllowProduction:
                case ItemId.ScenarioAllowTechnology:
                    if (Scenarios.Data.GlobalData.Rules == null)
                    {
                        Scenarios.Data.GlobalData.Rules = new ScenarioRules();
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理 - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        private void PreItemChanged(ItemId itemId, object val, MajorCountrySettings major)
        {
            // 何もしない
        }

        /// <summary>
        ///     項目変更前の処理 - シナリオ開始日時
        /// </summary>
        /// <param name="itemId1">連動する項目ID1</param>
        /// <param name="itemId2">連動する項目ID2</param>
        private void PreItemChangedScenarioStartDate(ItemId itemId1, ItemId itemId2)
        {
            if (Scenarios.Data.GlobalData.StartDate != null)
            {
                return;
            }

            Scenarios.Data.GlobalData.StartDate = new GameDate();

            // 編集済みフラグを設定する
            SetItemDirty(itemId1);
            SetItemDirty(itemId2);

            // 編集項目の値を更新する
            UpdateItemValue(itemId1);
            UpdateItemValue(itemId2);

            // 編集項目の色を更新する
            UpdateItemColor(itemId1);
            UpdateItemColor(itemId2);
        }

        /// <summary>
        ///     項目変更前の処理 - シナリオ終了日時
        /// </summary>
        /// <param name="itemId1">連動する項目ID1</param>
        /// <param name="itemId2">連動する項目ID2</param>
        private void PreItemChangedScenarioEndDate(ItemId itemId1, ItemId itemId2)
        {
            if (Scenarios.Data.GlobalData.EndDate != null)
            {
                return;
            }

            Scenarios.Data.GlobalData.EndDate = new GameDate();

            // 編集済みフラグを設定する
            SetItemDirty(itemId1);
            SetItemDirty(itemId2);

            // 編集項目の値を更新する
            UpdateItemValue(itemId1);
            UpdateItemValue(itemId2);

            // 編集項目の色を更新する
            UpdateItemColor(itemId1);
            UpdateItemColor(itemId2);
        }

        #endregion

        #region 変更後処理

        /// <summary>
        ///     項目値変更後の処理 - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        private void PostItemChanged(ItemId itemId, object val)
        {
            switch (itemId)
            {
                case ItemId.ScenarioPanelName:
                    _page?.UpdatePanelImage((string) val);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        private void PostItemChanged(ItemId itemId, object val, MajorCountrySettings major)
        {
            switch (itemId)
            {
                case ItemId.MajorCountryNameKey:
                    UpdateItemValue(ItemId.MajorCountryNameString, major);
                    UpdateItemColor(ItemId.MajorCountryNameString, major);
                    break;

                case ItemId.MajorCountryDescKey:
                    UpdateItemValue(ItemId.MajorCountryDescString, major);
                    UpdateItemColor(ItemId.MajorCountryDescString, major);
                    break;

                case ItemId.MajorPropaganada:
                    _page?.UpdatePropagandaImage(major.Country, (string) val);
                    break;
            }
        }

        #endregion

        #region ログ出力

        /// <summary>
        ///     編集項目の値変更時のログを出力する - シナリオ情報
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        private void OutputItemValueChangedLog(ItemId itemId, object val)
        {
            Log.Info("[Scenario] {0}: {1} -> {2}", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId)), ObjectHelper.ToString(val));
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する - 主要国設定
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        private void OutputItemValueChangedLog(ItemId itemId, object val, MajorCountrySettings major)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, major)), ObjectHelper.ToString(val),
                Countries.Strings[(int) major.Country]);
        }

        #endregion
    }
}