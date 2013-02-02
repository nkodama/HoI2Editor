using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     ユニットモデルエディタフォーム
    /// </summary>
    public partial class UnitEditorForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public UnitEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitModelEditorFormLoad(object sender, EventArgs e)
        {
            // 各種データを初期化する
            InitData();

            // 各種ファイルを読み込む
            LoadFiles();

            // 編集可能な項目を初期化する
            InitEditableItems();

            // ゲームの種類により編集項目を有効化/無効化する
            ActivateEditableItems();

            // ユニットリストボックスを更新する
            UpdateUnitListBox();
        }

        /// <summary>
        ///     各種データを初期化する
        /// </summary>
        private static void InitData()
        {
            // 国家データを初期化する
            Country.Init();

            // ユニットデータを初期化する
            Units.Init();
        }

        /// <summary>
        ///     編集可能な項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国家リストボックス
            foreach (string s in Country.CountryTextTable.Where(country => !string.IsNullOrEmpty(country)))
            {
                countryListView.Items.Add(s);
            }

            // 兵科コンボボックス
            branchComboBox.Items.Add(Resources.BranchArmy);
            branchComboBox.Items.Add(Resources.BranchNavy);
            branchComboBox.Items.Add(Resources.BranchAirforce);

            // 付属可能旅団チェックリストボックス
            allowedBrigadesCheckedListBox.Items.Clear();
            foreach (UnitType type in Units.BrigadeTypes)
            {
                allowedBrigadesCheckedListBox.Items.Add(Config.GetText(Units.List[(int) type].Name));
            }

            // 生産ユニット種類コンボボックス
            realUnitTypeComboBox.Items.Clear();
            foreach (RealUnitType type in Enum.GetValues(typeof (RealUnitType)))
            {
                realUnitTypeComboBox.Items.Add(Units.RealStringTable[(int) type]);
            }

            // スプライト種類コンボボックス
            spriteTypeComboBox.Items.Clear();
            foreach (SpriteType type in Enum.GetValues(typeof (SpriteType)))
            {
                spriteTypeComboBox.Items.Add(Units.SpriteStringTable[(int) type]);
            }

            // 代替ユニット種類コンボボックス
            transmuteComboBox.Items.Clear();
            foreach (UnitType type in Units.Types)
            {
                transmuteComboBox.Items.Add(Config.GetText(Units.List[(int) type].Name));
            }

            // 更新ユニット種類コンボボックス
            upgradeTypeComboBox.Items.Clear();
            foreach (UnitType type in Units.Types)
            {
                upgradeTypeComboBox.Items.Add(Config.GetText(Units.List[(int) type].Name));
            }

            // ユニットモデルの編集項目を無効化する
            DisableModelEditableItems();
        }

        /// <summary>
        ///     ゲームの種類により編集項目を有効化/無効化する
        /// </summary>
        private void ActivateEditableItems()
        {
            if (Game.Type == GameType.ArsenalOfDemocracy || (Game.Type == GameType.DarkestHour && Game.Version >= 103))
            {
                branchComboBox.Enabled = true;
            }
            else
            {
                branchComboBox.Enabled = false;
            }
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                productableCheckBox.Enabled = true;
                detachableCheckBox.Enabled = true;
                gfxPrioLabel.Enabled = true;
                gfxPrioNumericUpDown.Enabled = true;
                listPrioLabel.Enabled = true;
                listPrioNumericUpDown.Enabled = true;
                realUnitTypeLabel.Enabled = true;
                realUnitTypeComboBox.Enabled = true;
                spriteTypeLabel.Enabled = true;
                spriteTypeComboBox.Enabled = true;
                transmuteLabel.Enabled = true;
                transmuteComboBox.Enabled = true;
                militaryValueLabel.Enabled = true;
                militaryValueTextBox.Enabled = true;

                speedCapAllLabel.Enabled = true;
                speedCapAllTextBox.Enabled = true;
                equipmentGroupBox.Enabled = true;
            }
            else
            {
                productableCheckBox.Enabled = false;
                detachableCheckBox.Enabled = false;
                gfxPrioLabel.Enabled = false;
                gfxPrioNumericUpDown.Enabled = false;
                listPrioLabel.Enabled = false;
                listPrioNumericUpDown.Enabled = false;
                realUnitTypeLabel.Enabled = false;
                realUnitTypeComboBox.Enabled = false;
                spriteTypeLabel.Enabled = false;
                spriteTypeComboBox.Enabled = false;
                transmuteLabel.Enabled = false;
                transmuteComboBox.Enabled = false;
                militaryValueLabel.Enabled = false;
                militaryValueTextBox.Enabled = false;

                speedCapAllLabel.Enabled = false;
                speedCapAllTextBox.Enabled = false;
                equipmentGroupBox.Enabled = false;
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                maxSupplyStockLabel.Enabled = true;
                maxSupplyStockTextBox.Enabled = true;
                maxOilStockLabel.Enabled = true;
                maxOilStockTextBox.Enabled = true;
                artilleryBombardmentLabel.Enabled = true;
                artilleryBombardmentTextBox.Enabled = true;
            }
            else
            {
                maxSupplyStockLabel.Enabled = false;
                maxSupplyStockTextBox.Enabled = false;
                maxOilStockLabel.Enabled = false;
                maxOilStockTextBox.Enabled = false;
                artilleryBombardmentLabel.Enabled = false;
                artilleryBombardmentTextBox.Enabled = false;
            }
            if (Game.Type == GameType.DarkestHour)
            {
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                noFuelCombatModLabel.Enabled = true;
                noFuelCombatModTextBox.Enabled = true;

                upgradeGroupBox.Enabled = true;
            }
            else
            {
                reinforceCostLabel.Enabled = false;
                reinforceCostTextBox.Enabled = false;
                reinforceTimeLabel.Enabled = false;
                reinforceTimeTextBox.Enabled = false;
                noFuelCombatModLabel.Enabled = false;
                noFuelCombatModTextBox.Enabled = false;

                upgradeGroupBox.Enabled = false;
            }
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     各種ファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 文字列定義ファイルを読み込む
            Config.LoadConfigFiles();

            // ユニットデータを読み込む
            Units.Load();
        }

        /// <summary>
        ///     各種ファイルを再読み込みする
        /// </summary>
        private void ReloadFiles()
        {
            // 文字列定義ファイルを読み込む
            Config.LoadConfigFiles();

            // ユニットデータを読み込む
            Units.Reload();
        }

        /// <summary>
        ///     各種ファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 文字列定義ファイルを保存する
            Config.SaveConfigFiles();

            // ユニットデータを保存する
            Units.Save();
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        private void SetDirty()
        {
            int index = unitListBox.SelectedIndex;
            UnitType type = Units.Types[index];
            Units.SetDirty(type, true);
        }

        /// <summary>
        ///     ユニットモデルを挿入する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="model">挿入対象のユニットモデル</param>
        /// <param name="index">挿入する位置</param>
        /// <param name="name">ユニットモデル名</param>
        private void InsertModel(Unit unit, UnitModel model, int index, string name)
        {
            // ユニットクラスにユニットモデルを挿入する
            unit.InsertModel(model, index);

            // 挿入位置以降のユニットモデル名を変更する
            string key = UnitModel.GetName(unit, unit.Models.Count - 1, CountryTag.None);
            if (!Config.ExistsKey(key))
            {
                string temp = Config.GetTempKey(Game.UnitTextFileName);
                Config.RenameTempKey(temp, key, Game.UnitTextFileName);
            }
            for (int i = unit.Models.Count - 1; i >= index; i--)
            {
                Config.SetText(UnitModel.GetName(unit, i, CountryTag.None),
                               Config.GetText(UnitModel.GetName(unit, i - 1, CountryTag.None)));
            }

            // 挿入位置のユニットモデル名を変更する
            Config.SetText(UnitModel.GetName(unit, index, CountryTag.None), name);

            // ユニットモデルリストビューの表示を更新する
            UpdateModelListView(unit);

            // 挿入した項目を選択する
            modelListView.Items[index].Focused = true;
            modelListView.Items[index].Selected = true;

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     ユニットモデルを削除する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="index">削除する位置</param>
        private void RemoveModel(Unit unit, int index)
        {
            // ユニットクラスからユニットモデルを削除する
            unit.RemoveModel(index);

            // 削除位置以降のユニットモデル名を変更する
            if (index < unit.Models.Count)
            {
                for (int i = index; i < unit.Models.Count; i++)
                {
                    Config.SetText(UnitModel.GetName(unit, i, CountryTag.None),
                                   Config.GetText(UnitModel.GetName(unit, i + 1, CountryTag.None)));
                }
            }
            Config.SetText(UnitModel.GetName(unit, unit.Models.Count, CountryTag.None), String.Empty);

            // ユニットモデルリストビューの表示を更新する
            UpdateModelListView(unit);

            // 次の項目を選択する
            if (index < modelListView.Items.Count - 1)
            {
                modelListView.Items[index].Focused = true;
                modelListView.Items[index].Selected = true;
            }
            else if (modelListView.Items.Count > 0)
            {
                modelListView.Items[index - 1].Focused = true;
                modelListView.Items[index - 1].Selected = true;
            }

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     ユニットモデルを移動する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveModel(Unit unit, int src, int dest)
        {
            // ユニットクラスのユニットモデルを移動する
            unit.MoveModel(src, dest);

            // 移動元と移動先の間のユニットモデル名を変更する
            string name = Config.GetText(UnitModel.GetName(unit, src, CountryTag.None));
            if (src > dest)
            {
                // 上へ移動する場合
                for (int i = src; i > dest; i--)
                {
                    Config.SetText(UnitModel.GetName(unit, i, CountryTag.None),
                                   Config.GetText(UnitModel.GetName(unit, i - 1, CountryTag.None)));
                }
            }
            else
            {
                // 下へ移動する場合
                for (int i = src; i < dest; i++)
                {
                    Config.SetText(UnitModel.GetName(unit, i, CountryTag.None),
                                   Config.GetText(UnitModel.GetName(unit, i + 1, CountryTag.None)));
                }
            }
            Config.SetText(UnitModel.GetName(unit, dest, CountryTag.None), name);

            // ユニットモデルリストビューの表示を更新する
            UpdateModelListView(unit);

            // 移動先の項目を選択する
            modelListView.Items[dest].Focused = true;
            modelListView.Items[dest].Selected = true;

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        #endregion

        #region フォーム直下のボタン

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            ReloadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFiles();
        }

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region ユニットクラスリストボックス

        /// <summary>
        ///     ユニットクラスリストボックスの表示を更新する
        /// </summary>
        private void UpdateUnitListBox()
        {
            // リストボックスに項目を登録する
            unitListBox.BeginUpdate();
            unitListBox.Items.Clear();
            foreach (UnitType type in Units.Types)
            {
                Unit unit = Units.List[(int) type];
                unitListBox.Items.Add(Config.GetText(unit.Name));
            }
            unitListBox.EndUpdate();

            // 先頭の項目を選択する
            if (unitListBox.Items.Count > 0)
            {
                unitListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     ユニットクラスリストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 選択項目がない場合は何もしない
            if (e.Index == -1)
            {
                return;
            }

            UnitType type = Units.Types[e.Index];
            Unit unit = Units.List[(int) type];

            // 背景を描画する
            e.DrawBackground();
            if ((e.State & DrawItemState.Selected) == 0)
            {
                if (unit.Models.Count > 0)
                {
                    e.Graphics.FillRectangle(
                        unit.Organization == UnitOrganization.Division ? Brushes.AliceBlue : Brushes.Honeydew,
                        new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                }
            }

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                brush = Units.DirtyFlags[(int) type]
                            ? new SolidBrush(Color.Red)
                            : new SolidBrush(unitListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            var listbox = sender as ListBox;
            if (listbox != null)
            {
                string s = listbox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            }
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ユニットクラスリストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの表示を更新する
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            UpdateModelListView(unit);

            // ユニットクラスタブの表示を更新する
            UpdateClassEditableItems();

            if (editTabControl.SelectedIndex == 1)
            {
                // ユニットモデル編集中ならば先頭の項目を選択する
                if (modelListView.Items.Count > 0)
                {
                    modelListView.Items[0].Focused = true;
                    modelListView.Items[0].Selected = true;
                }
                else
                {
                    // ユニットクラスタブを選択する
                    editTabControl.SelectedIndex = 0;

                    // ユニットモデルの編集項目を無効化する
                    DisableModelEditableItems();
                }
            }
        }

        #endregion

        #region ユニットモデルリストビュー

        /// <summary>
        ///     ユニットモデルリストビューの表示を更新する
        /// </summary>
        /// <param name="unit">選択中のユニットクラス</param>
        private void UpdateModelListView(Unit unit)
        {
            // リストビューに項目を登録する
            modelListView.BeginUpdate();
            modelListView.Items.Clear();
            for (int no = 0; no < unit.Models.Count; no++)
            {
                ListViewItem item = CreateModelListItem(unit, no);
                modelListView.Items.Add(item);
            }
            modelListView.EndUpdate();
        }

        /// <summary>
        ///     ユニットモデルリストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 未選択の場合
            if (modelListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableModelEditableItems();
                return;
            }

            // ユニットモデルタブを選択する
            editTabControl.SelectedIndex = 1;

            // 編集項目を有効化する
            EnableModelEditableItems();

            // 編集項目の値を更新する
            UpdateModelEditableItems();
        }

        /// <summary>
        ///     ユニットモデルリストビューの項目を作成する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="no">ユニットモデル番号</param>
        /// <returns>ユニットモデルリストビューの項目</returns>
        private ListViewItem CreateModelListItem(Unit unit, int no)
        {
            UnitModel model = unit.Models[no];

            var item = new ListViewItem {Text = Config.GetText(no.ToString(CultureInfo.InvariantCulture))};
            item.SubItems.Add(Config.GetText(UnitModel.GetName(unit, no, CountryTag.None)));
            item.SubItems.Add(Config.GetText(unit.Name));
            switch (unit.Branch)
            {
                case UnitBranch.Army:
                    item.SubItems.Add(Resources.BranchArmy);
                    break;

                case UnitBranch.Navy:
                    item.SubItems.Add(Resources.BranchNavy);
                    break;

                case UnitBranch.AirForce:
                    item.SubItems.Add(Resources.BranchAirforce);
                    break;
            }
            item.SubItems.Add(model.Cost.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.BuildTime.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.ManPower.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.SupplyConsumption.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.FuelConsumption.ToString(CultureInfo.InvariantCulture));

            return item;
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // ユニットモデルを挿入する
            var model = new UnitModel();
            if (modelListView.SelectedIndices.Count > 0)
            {
                int no = modelListView.SelectedIndices[0];
                InsertModel(unit, model, no + 1, "");
            }
            else
            {
                InsertModel(unit, model, 0, "");
            }
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];

            // ユニットモデルを挿入する
            UnitModel model = unit.Models[no].Clone();
            InsertModel(unit, model, no + 1, Config.GetText(UnitModel.GetName(unit, no, CountryTag.None)));
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];

            // ユニットモデルを削除する
            RemoveModel(unit, no);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // リストの先頭ならば何もしない
            int no = modelListView.SelectedIndices[0];
            if (no == 0)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, no, 0);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // リストの先頭ならば何もしない
            int no = modelListView.SelectedIndices[0];
            if (no == 0)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, no, no - 1);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // リストの末尾ならば何もしない
            int no = modelListView.SelectedIndices[0];
            if (no == unit.Models.Count - 1)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, no, no + 1);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottonButtonClick(object sender, EventArgs e)
        {
            // ユニットモデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // リストの末尾ならば何もしない
            int no = modelListView.SelectedIndices[0];
            if (no == unit.Models.Count - 1)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, no, unit.Models.Count - 1);
        }

        #endregion

        #region 国家リストビュー

        /// <summary>
        ///     国家リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // モデルリストビューの選択項目がなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            CountryTag country = countryListView.SelectedIndices.Count == 0
                                     ? CountryTag.None
                                     : (CountryTag) (countryListView.SelectedIndices[0] + 1);

            // ユニットモデル画像名を更新する
            modelImagePictureBox.ImageLocation = GetModelImageFileName(unit, no, country);

            // ユニットモデル名を更新する
            modelNameTextBox.Text = Config.GetText(UnitModel.GetName(unit, no, country));
        }

        #endregion

        #region ユニットクラスタブ

        /// <summary>
        ///     ユニットクラスタブの編集項目の値を更新する
        /// </summary>
        private void UpdateClassEditableItems()
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            classNameTextBox.Text = Config.GetText(unit.Name);
            classShortNameTextBox.Text = Config.GetText(unit.ShortName);
            classDescTextBox.Text = Config.GetText(unit.Desc);
            classShortDescTextBox.Text = Config.GetText(unit.ShortDesc);
            branchComboBox.SelectedIndex = (int) unit.Branch;
            for (int i = 0; i < Units.BrigadeTypes.Count(); i++)
            {
                UnitType brigade = Units.BrigadeTypes[i];
                allowedBrigadesCheckedListBox.SetItemChecked(i, unit.AllowedBrigades.Contains(brigade));
            }
            if (Game.Type == GameType.DarkestHour)
            {
                UpdateUpgradeList(unit);
                if (Game.Version >= 103)
                {
                    productableCheckBox.Checked = unit.Productable;
                    detachableCheckBox.Checked = unit.Detachable;
                    gfxPrioNumericUpDown.Value = unit.GfxPrio;
                    listPrioNumericUpDown.Value = unit.ListPrio;
                    realUnitTypeComboBox.SelectedIndex = (int) unit.RealType;
                    spriteTypeComboBox.SelectedIndex = (int) unit.Sprite;
                    int index = -1;
                    for (int i = 0; i < Units.Types.Length; i++)
                    {
                        if (unit.Transmute == Units.Types[i])
                        {
                            index = i;
                        }
                    }
                    transmuteComboBox.SelectedIndex = index;
                    militaryValueTextBox.Text = unit.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        ///     ユニットクラス名テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassNameTextBoxValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (classNameTextBox.Text.Equals(Config.GetText(unit.Name)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.Name, classNameTextBox.Text);

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     ユニットクラス短縮名テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassShortNameTextBoxValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (classShortNameTextBox.Text.Equals(Config.GetText(unit.ShortName)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.ShortName, classShortNameTextBox.Text);

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     ユニットクラス説明テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassDescTextBoxValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (classDescTextBox.Text.Equals(Config.GetText(unit.Desc)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.Desc, classDescTextBox.Text);

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     ユニットクラス短縮説明テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassShortDescTextBox(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (classShortDescTextBox.Text.Equals(Config.GetText(unit.ShortDesc)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.ShortDesc, classShortDescTextBox.Text);

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(Game.UnitTextFileName);
        }

        /// <summary>
        ///     兵科コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (UnitBranch) branchComboBox.SelectedIndex;
            if (val == unit.Branch)
            {
                return;
            }

            // 値を更新する
            unit.Branch = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     生産可能チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProductableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (productableCheckBox.Checked == unit.Productable)
            {
                return;
            }

            // 値を更新する
            unit.Productable = productableCheckBox.Checked;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     着脱可能チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDetachableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            if (detachableCheckBox.Checked == unit.Detachable)
            {
                return;
            }

            // 値を更新する
            unit.Detachable = detachableCheckBox.Checked;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     画像優先度数値アップダウンフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGraphicsPriorityNumericUpDownValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (int) gfxPrioNumericUpDown.Value;
            if (val == unit.GfxPrio)
            {
                return;
            }

            // 値を更新する
            unit.GfxPrio = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     リスト優先度数値アップダウンフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListPrioNumericUpDownValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (int) listPrioNumericUpDown.Value;
            if (val == unit.ListPrio)
            {
                return;
            }

            // 値を更新する
            unit.ListPrio = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     生産ユニット種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRealUnitTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (RealUnitType) realUnitTypeComboBox.SelectedIndex;
            if (val == unit.RealType)
            {
                return;
            }

            // 値を更新する
            unit.RealType = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     スプライト種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpriteTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (SpriteType) spriteTypeComboBox.SelectedIndex;
            if (val == unit.Sprite)
            {
                return;
            }

            // 値を更新する
            unit.Sprite = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     代替ユニットコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransmuteComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 値に変化がなければ何もしない
            var val = (UnitType) transmuteComboBox.SelectedIndex;
            if (val == unit.Transmute)
            {
                return;
            }

            // 値を更新する
            unit.Transmute = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     軍事力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMilitaryValueTextBoxValidated(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(militaryValueTextBox.Text, out val))
            {
                militaryValueTextBox.Text = unit.Value.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - unit.Value) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            unit.Value = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良リストビューの項目を更新する
        /// </summary>
        /// <param name="unit"></param>
        private void UpdateUpgradeList(Unit unit)
        {
            // 項目を順に登録する
            upgradeListView.BeginUpdate();
            upgradeListView.Items.Clear();
            foreach (UnitUpgrade upgrade in unit.Upgrades)
            {
                upgradeListView.Items.Add(CreateUpgradeListItem(upgrade));
            }
            upgradeListView.EndUpdate();

            // 改良情報が登録されていなければ編集項目を無効化して戻る
            if (unit.Upgrades.Count == 0)
            {
                DisableUpgradeItems();
                return;
            }

            upgradeListView.Items[0].Focused = true;
            upgradeListView.Items[0].Selected = true;

            // 編集項目を有効化する
            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良の編集項目を有効化する
        /// </summary>
        private void EnableUpgradeItems()
        {
            upgradeTypeComboBox.Enabled = true;
            upgradeCostTextBox.Enabled = true;
            upgradeTimeTextBox.Enabled = true;

            upgradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     改良の編集項目を無効化する
        /// </summary>
        private void DisableUpgradeItems()
        {
            upgradeTypeComboBox.Text = "";
            upgradeCostTextBox.Text = "";
            upgradeTimeTextBox.Text = "";

            upgradeTypeComboBox.Enabled = false;
            upgradeCostTextBox.Enabled = false;
            upgradeTimeTextBox.Enabled = false;

            upgradeRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     改良リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集を禁止する
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                DisableUpgradeItems();
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            UnitUpgrade upgrade = unit.Upgrades[upgradeListView.SelectedIndices[0]];

            upgradeTypeComboBox.SelectedIndex = (int) upgrade.Type;
            upgradeCostTextBox.Text = upgrade.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
            upgradeTimeTextBox.Text = upgrade.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);

            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良ユニット種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 値に変化がなければ何もしない
            var val = (UnitType) upgradeTypeComboBox.SelectedIndex;
            if (val == upgrade.Type)
            {
                return;
            }

            // 値を更新する
            upgrade.Type = val;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].Text = Config.GetText(Units.List[(int) val].Name);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeCostTextBox.Text, out val))
            {
                upgradeCostTextBox.Text = upgrade.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - upgrade.UpgradeCostFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            upgrade.UpgradeCostFactor = val;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].SubItems[1].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeTimeTextBox.Text, out val))
            {
                upgradeTimeTextBox.Text = upgrade.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - upgrade.UpgradeTimeFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            upgrade.UpgradeTimeFactor = val;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].SubItems[2].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良設定の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeAddButtonClick(object sender, EventArgs e)
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];

            var upgrade = new UnitUpgrade();
            unit.Upgrades.Add(upgrade);

            AddUpgradeListItem(upgrade);

            SetDirty();
        }

        /// <summary>
        ///     改良設定の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中の項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int index = upgradeListView.SelectedIndices[0];

            RemoveUpgradeListItem(index);

            unit.Upgrades.RemoveAt(index);

            SetDirty();
        }

        /// <summary>
        ///     改良リストの項目を作成する
        /// </summary>
        /// <param name="upgrade">改良設定</param>
        /// <returns>改良リストの項目</returns>
        private static ListViewItem CreateUpgradeListItem(UnitUpgrade upgrade)
        {
            var item = new ListViewItem {Text = Config.GetText(Units.List[(int) upgrade.Type].Name)};
            item.SubItems.Add(upgrade.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(upgrade.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture));
            return item;
        }

        /// <summary>
        ///     改良リストの項目を追加する
        /// </summary>
        /// <param name="upgrade">追加対象の改良設定</param>
        private void AddUpgradeListItem(UnitUpgrade upgrade)
        {
            // 改良リストビューの項目を追加する
            upgradeListView.Items.Add(CreateUpgradeListItem(upgrade));

            // 追加した項目を選択する
            int index = upgradeListView.Items.Count - 1;
            upgradeListView.Items[index].Focused = true;
            upgradeListView.Items[index].Selected = true;
            upgradeListView.EnsureVisible(index);

            // 改良の編集項目を有効化する
            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveUpgradeListItem(int index)
        {
            // 改良リストビューの項目を削除する
            upgradeListView.Items.RemoveAt(index);

            if (index < upgradeListView.Items.Count)
            {
                // 追加した項目の次を選択する
                upgradeListView.Items[index].Focused = true;
                upgradeListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // 末尾の項目を選択する
                upgradeListView.Items[upgradeListView.Items.Count - 1].Focused = true;
                upgradeListView.Items[upgradeListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 改良の編集項目を無効化する
                DisableUpgradeItems();
            }
        }

        #endregion

        #region ユニットモデルタブ

        #region 編集項目操作

        /// <summary>
        ///     ユニットモデルタブの編集項目の値を更新する
        /// </summary>
        private void UpdateModelEditableItems()
        {
            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];
            CountryTag country = countryListView.SelectedIndices.Count == 0
                                     ? CountryTag.None
                                     : (CountryTag) (countryListView.SelectedIndices[0] + 1);

            modelImagePictureBox.ImageLocation = GetModelImageFileName(unit, no, country);
            modelIconPictureBox.ImageLocation = GetModelIconFileName(unit, no);
            modelNameTextBox.Text = Config.GetText(UnitModel.GetName(unit, no, country));
            defaultOrganisationTextBox.Text = model.DefaultOrganization.ToString(CultureInfo.InvariantCulture);
            moraleTextBox.Text = model.Morale.ToString(CultureInfo.InvariantCulture);
            rangeTextBox.Text = model.Range.ToString(CultureInfo.InvariantCulture);
            transportWeightTextBox.Text = model.TransportWeight.ToString(CultureInfo.InvariantCulture);
            transportCapabilityTextBox.Text = model.TransportCapability.ToString(CultureInfo.InvariantCulture);
            suppressionTextBox.Text = model.Suppression.ToString(CultureInfo.InvariantCulture);
            supplyConsumptionTextBox.Text = model.SupplyConsumption.ToString(CultureInfo.InvariantCulture);
            fuelConsumptionTextBox.Text = model.FuelConsumption.ToString(CultureInfo.InvariantCulture);
            costTextBox.Text = model.Cost.ToString(CultureInfo.InvariantCulture);
            buildTimeTextBox.Text = model.BuildTime.ToString(CultureInfo.InvariantCulture);
            manPowerTextBox.Text = model.ManPower.ToString(CultureInfo.InvariantCulture);
            upgradeCostFactorTextBox.Text = model.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
            upgradeTimeFactorTextBox.Text = model.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);
            maxSpeedTextBox.Text = model.MaxSpeed.ToString(CultureInfo.InvariantCulture);
            speedCapArtTextBox.Text = model.SpeedCapArt.ToString(CultureInfo.InvariantCulture);
            speedCapEngTextBox.Text = model.SpeedCapEng.ToString(CultureInfo.InvariantCulture);
            speedCapAtTextBox.Text = model.SpeedCapAt.ToString(CultureInfo.InvariantCulture);
            speedCapAaTextBox.Text = model.SpeedCapAa.ToString(CultureInfo.InvariantCulture);
            defensivenessTextBox.Text = model.Defensiveness.ToString(CultureInfo.InvariantCulture);
            seaDefenceTextBox.Text = model.SeaDefense.ToString(CultureInfo.InvariantCulture);
            airDefenceTextBox.Text = model.AirDefense.ToString(CultureInfo.InvariantCulture);
            surfaceDefenceTextBox.Text = model.SurfaceDefense.ToString(CultureInfo.InvariantCulture);
            toughnessTextBox.Text = model.Toughness.ToString(CultureInfo.InvariantCulture);
            softnessTextBox.Text = model.Softness.ToString(CultureInfo.InvariantCulture);
            softAttackTextBox.Text = model.SoftAttack.ToString(CultureInfo.InvariantCulture);
            hardAttackTextBox.Text = model.HardAttack.ToString(CultureInfo.InvariantCulture);
            seaAttackTextBox.Text = model.SeaAttack.ToString(CultureInfo.InvariantCulture);
            subAttackTextBox.Text = model.SubAttack.ToString(CultureInfo.InvariantCulture);
            convoyAttackTextBox.Text = model.ConvoyAttack.ToString(CultureInfo.InvariantCulture);
            shoreBombardmentTextBox.Text = model.ShoreBombardment.ToString(CultureInfo.InvariantCulture);
            airAttackTextBox.Text = model.AirAttack.ToString(CultureInfo.InvariantCulture);
            navalAttackTextBox.Text = model.NavalAttack.ToString(CultureInfo.InvariantCulture);
            strategicAttackTextBox.Text = model.StrategicAttack.ToString(CultureInfo.InvariantCulture);
            distanceTextBox.Text = model.Distance.ToString(CultureInfo.InvariantCulture);
            visibilityTextBox.Text = model.Visibility.ToString(CultureInfo.InvariantCulture);
            surfaceDetectionCapabilityTextBox.Text =
                model.SurfaceDetectionCapability.ToString(CultureInfo.InvariantCulture);
            subDetectionCapabilityTextBox.Text = model.SubDetectionCapability.ToString(CultureInfo.InvariantCulture);
            airDetectionCapabilityTextBox.Text = model.AirDetectionCapability.ToString(CultureInfo.InvariantCulture);

            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                maxSupplyStockTextBox.Text = model.MaxSupplyStock.ToString(CultureInfo.InvariantCulture);
                maxOilStockTextBox.Text = model.MaxOilStock.ToString(CultureInfo.InvariantCulture);
                artilleryBombardmentTextBox.Text = model.ArtilleryBombardment.ToString(CultureInfo.InvariantCulture);
            }

            if (Game.Type == GameType.DarkestHour)
            {
                reinforceCostTextBox.Text = model.ReinforceCostFactor.ToString(CultureInfo.InvariantCulture);
                reinforceTimeTextBox.Text = model.ReinforceTimeFactor.ToString(CultureInfo.InvariantCulture);
                noFuelCombatModTextBox.Text = model.NoFuelCombatMod.ToString(CultureInfo.InvariantCulture);
                if (Game.Version >= 103)
                {
                    speedCapAllTextBox.Text = model.SpeedCap.ToString(CultureInfo.InvariantCulture);
                    UpdateEquipmentList(model);
                }
            }
        }

        /// <summary>
        ///     ユニットモデルタブの編集項目を有効化する
        /// </summary>
        private void EnableModelEditableItems()
        {
            modelNameTextBox.Enabled = true;
            basicGroupBox.Enabled = true;
            productionGroupBox.Enabled = true;
            speedGroupBox.Enabled = true;
            battleGroupBox.Enabled = true;

            // ゲームの種類依存の項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                maxSupplyStockLabel.Enabled = true;
                maxSupplyStockTextBox.Enabled = true;
                maxOilStockLabel.Enabled = true;
                maxOilStockTextBox.Enabled = true;
                artilleryBombardmentLabel.Enabled = true;
                artilleryBombardmentTextBox.Enabled = true;
            }
            else if (Game.Type == GameType.DarkestHour)
            {
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                noFuelCombatModLabel.Enabled = true;
                noFuelCombatModTextBox.Enabled = true;

                if (Game.Version >= 103)
                {
                    speedCapAllLabel.Enabled = true;
                    speedCapAllTextBox.Enabled = true;
                    equipmentGroupBox.Enabled = true;
                }
            }
        }

        /// <summary>
        ///     ユニットモデルタブの編集項目を無効化する
        /// </summary>
        private void DisableModelEditableItems()
        {
            modelImagePictureBox.ImageLocation = "";
            modelIconPictureBox.ImageLocation = "";
            modelNameTextBox.Text = "";
            defaultOrganisationTextBox.Text = "";
            moraleTextBox.Text = "";
            rangeTextBox.Text = "";
            transportWeightTextBox.Text = "";
            transportCapabilityTextBox.Text = "";
            suppressionTextBox.Text = "";
            supplyConsumptionTextBox.Text = "";
            fuelConsumptionTextBox.Text = "";
            maxSupplyStockTextBox.Text = "";
            maxOilStockTextBox.Text = "";
            costTextBox.Text = "";
            buildTimeTextBox.Text = "";
            manPowerTextBox.Text = "";
            upgradeCostFactorTextBox.Text = "";
            upgradeTimeFactorTextBox.Text = "";
            reinforceCostTextBox.Text = "";
            reinforceTimeTextBox.Text = "";
            maxSpeedTextBox.Text = "";
            speedCapAllTextBox.Text = "";
            speedCapArtTextBox.Text = "";
            speedCapEngTextBox.Text = "";
            speedCapAtTextBox.Text = "";
            speedCapAaTextBox.Text = "";
            defensivenessTextBox.Text = "";
            seaDefenceTextBox.Text = "";
            airDefenceTextBox.Text = "";
            surfaceDefenceTextBox.Text = "";
            toughnessTextBox.Text = "";
            softnessTextBox.Text = "";
            softAttackTextBox.Text = "";
            hardAttackTextBox.Text = "";
            seaAttackTextBox.Text = "";
            subAttackTextBox.Text = "";
            convoyAttackTextBox.Text = "";
            shoreBombardmentTextBox.Text = "";
            airAttackTextBox.Text = "";
            navalAttackTextBox.Text = "";
            strategicAttackTextBox.Text = "";
            artilleryBombardmentTextBox.Text = "";
            distanceTextBox.Text = "";
            visibilityTextBox.Text = "";
            surfaceDetectionCapabilityTextBox.Text = "";
            subDetectionCapabilityTextBox.Text = "";
            airDetectionCapabilityTextBox.Text = "";
            noFuelCombatModTextBox.Text = "";
            equipmentListView.Items.Clear();
            resourceComboBox.Text = "";
            quantityTextBox.Text = "";

            modelNameTextBox.Enabled = false;
            basicGroupBox.Enabled = false;
            productionGroupBox.Enabled = false;
            speedGroupBox.Enabled = false;
            battleGroupBox.Enabled = false;
            equipmentGroupBox.Enabled = false;
        }

        #endregion

        #region ユニットモデル名

        /// <summary>
        ///     ユニットモデル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];

            // 値に変化がなければ何もしない
            string val = modelNameTextBox.Text;
            string name = UnitModel.GetName(unit, no,
                                            countryListView.SelectedIndices.Count == 0
                                                ? CountryTag.None
                                                : (CountryTag) (countryListView.SelectedIndices[0] + 1));
            if (val.Equals(Config.GetText(name)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(name, val);

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[1].Text = val;

            // 編集済みフラグを設定する
            SetDirty();
            Config.SetDirtyFlag(countryListView.SelectedIndices.Count == 0
                                    ? Game.UnitTextFileName
                                    : Game.ModelTextFileName);
        }

        #endregion

        #region ユニットモデルの画像

        /// <summary>
        ///     ユニットモデル画像のファイル名を取得する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="no">ユニットモデル番号</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル画像のファイル名</returns>
        private static string GetModelImageFileName(Unit unit, int no, CountryTag country)
        {
            string name;
            string fileName;
            if (country != CountryTag.None)
            {
                // 国タグ指定/モデル番号指定
                name = string.Format(
                    unit.Organization == UnitOrganization.Division
                        ? "ill_div_{0}_{1}_{2}.bmp"
                        : "ill_bri_{0}_{1}_{2}.bmp",
                    Country.CountryTextTable[(int) country],
                    Units.UnitNumbers[(int) unit.Type],
                    no);
                fileName = Game.GetReadFileName(Path.Combine(Game.ModelPicturePathName, name));
                if (File.Exists(fileName))
                {
                    return fileName;
                }

                // 国タグ指定/モデル番号は0指定
                name = string.Format(
                    unit.Organization == UnitOrganization.Division
                        ? "ill_div_{0}_{1}_0.bmp"
                        : "ill_bri_{0}_{1}_0.bmp",
                    Country.CountryTextTable[(int) country],
                    Units.UnitNumbers[(int) unit.Type]);
                fileName = Game.GetReadFileName(Path.Combine(Game.ModelPicturePathName, name));
                if (File.Exists(fileName))
                {
                    return fileName;
                }
            }

            // モデル番号指定
            name = string.Format(
                unit.Organization == UnitOrganization.Division
                    ? "ill_div_{0}_{1}.bmp"
                    : "ill_bri_{0}_{1}.bmp",
                Units.UnitNumbers[(int) unit.Type],
                no);
            fileName = Game.GetReadFileName(Path.Combine(Game.ModelPicturePathName, name));
            if (File.Exists(fileName))
            {
                return fileName;
            }

            // モデル番号は0指定
            name = string.Format(
                unit.Organization == UnitOrganization.Division
                    ? "ill_div_{0}_0.bmp"
                    : "ill_bri_{0}_0.bmp",
                Units.UnitNumbers[(int) unit.Type]);
            fileName = Game.GetReadFileName(Path.Combine(Game.ModelPicturePathName, name));
            return File.Exists(fileName) ? fileName : string.Empty;
        }

        /// <summary>
        ///     ユニットモデルアイコンのファイル名を取得する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="no">ユニットモデル番号</param>
        /// <returns>ユニットモデルアイコンのファイル名</returns>
        private static string GetModelIconFileName(Unit unit, int no)
        {
            // 旅団にはアイコンが存在しないので戻る
            if (unit.Organization == UnitOrganization.Brigade)
            {
                return string.Empty;
            }

            string name = string.Format("model_{0}_{1}.bmp", Units.UnitNumbers[(int) unit.Type], no);
            string fileName = Game.GetReadFileName(Path.Combine(Game.ModelPicturePathName, name));
            return File.Exists(fileName) ? fileName : string.Empty;
        }

        #endregion

        #region 基本ステータス

        /// <summary>
        ///     組織率テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultOrganizationTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(defaultOrganisationTextBox.Text, out val))
            {
                defaultOrganisationTextBox.Text = model.DefaultOrganization.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.DefaultOrganization) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.DefaultOrganization = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     士気テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoraleTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(moraleTextBox.Text, out val))
            {
                moraleTextBox.Text = model.Morale.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Morale) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Morale = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     航続距離テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRangeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(rangeTextBox.Text, out val))
            {
                rangeTextBox.Text = model.Range.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Range) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Range = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     輸送負荷テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransportWeightTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(transportWeightTextBox.Text, out val))
            {
                transportWeightTextBox.Text = model.TransportWeight.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.TransportWeight) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.TransportWeight = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     輸送能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransportCapabilityTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(transportCapabilityTextBox.Text, out val))
            {
                transportCapabilityTextBox.Text = model.TransportCapability.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.TransportCapability) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.TransportCapability = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     制圧力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuppressionTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(suppressionTextBox.Text, out val))
            {
                suppressionTextBox.Text = model.Suppression.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Suppression) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Suppression = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     消費物資テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyConsumptionTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(supplyConsumptionTextBox.Text, out val))
            {
                supplyConsumptionTextBox.Text = model.SupplyConsumption.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SupplyConsumption) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SupplyConsumption = val;

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[7].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     消費燃料テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelConsumptionTextBox(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(fuelConsumptionTextBox.Text, out val))
            {
                fuelConsumptionTextBox.Text = model.FuelConsumption.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.FuelConsumption) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.FuelConsumption = val;

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[8].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     最大物資テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSupplyStockTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxSupplyStockTextBox.Text, out val))
            {
                maxSupplyStockTextBox.Text = model.MaxSupplyStock.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.MaxSupplyStock) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.MaxSupplyStock = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     最大燃料テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxOilStockTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxOilStockTextBox.Text, out val))
            {
                maxOilStockTextBox.Text = model.MaxOilStock.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.MaxOilStock) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.MaxOilStock = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        #endregion

        #region 生産ステータス

        /// <summary>
        ///     必要ICテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(costTextBox.Text, out val))
            {
                costTextBox.Text = model.Cost.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Cost) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Cost = val;

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[4].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     必要時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(buildTimeTextBox.Text, out val))
            {
                costTextBox.Text = model.BuildTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.BuildTime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.BuildTime = val;

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[5].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     労働力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManPowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(manPowerTextBox.Text, out val))
            {
                manPowerTextBox.Text = model.ManPower.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ManPower) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ManPower = val;

            // モデルリストビューの項目を変更する
            modelListView.Items[no].SubItems[6].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostFactorTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeCostFactorTextBox.Text, out val))
            {
                upgradeCostFactorTextBox.Text = model.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.UpgradeCostFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.UpgradeCostFactor = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     改良時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeFactorTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(upgradeTimeFactorTextBox.Text, out val))
            {
                upgradeTimeFactorTextBox.Text = model.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.UpgradeTimeFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.UpgradeTimeFactor = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     補充コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceCostTextBox.Text, out val))
            {
                reinforceCostTextBox.Text = model.ReinforceCostFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ReinforceCostFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ReinforceCostFactor = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     補充時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(reinforceTimeTextBox.Text, out val))
            {
                reinforceTimeTextBox.Text = model.ReinforceTimeFactor.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ReinforceTimeFactor) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ReinforceTimeFactor = val;

            SetDirty();
            Units.SetDirty(unit.Type, true);
        }

        #endregion

        #region 速度ステータス

        /// <summary>
        ///     最大速度テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSpeedTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(maxSpeedTextBox.Text, out val))
            {
                maxSpeedTextBox.Text = model.MaxSpeed.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.MaxSpeed) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.MaxSpeed = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(speedCapAllTextBox.Text, out val))
            {
                speedCapAllTextBox.Text = model.SpeedCap.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SpeedCap) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SpeedCap = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     砲兵旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapArtTextBox(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(speedCapArtTextBox.Text, out val))
            {
                speedCapArtTextBox.Text = model.SpeedCapArt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SpeedCapArt) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SpeedCapArt = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     工兵旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapEngTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(speedCapEngTextBox.Text, out val))
            {
                speedCapEngTextBox.Text = model.SpeedCapEng.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SpeedCapEng) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SpeedCapEng = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対戦車旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapAtTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(speedCapAtTextBox.Text, out val))
            {
                speedCapAtTextBox.Text = model.SpeedCapAt.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SpeedCapAt) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SpeedCapAt = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対空旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapAaTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(speedCapAaTextBox.Text, out val))
            {
                speedCapAaTextBox.Text = model.SpeedCapAa.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SpeedCapAa) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SpeedCapAa = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        #endregion

        #region 戦闘ステータス

        /// <summary>
        ///     防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefensivenessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(defensivenessTextBox.Text, out val))
            {
                defensivenessTextBox.Text = model.Defensiveness.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Defensiveness) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Defensiveness = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対艦防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(seaDefenceTextBox.Text, out val))
            {
                seaDefenceTextBox.Text = model.SeaDefense.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SeaDefense) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SeaDefense = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対空防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(airDefenceTextBox.Text, out val))
            {
                airDefenceTextBox.Text = model.AirDefense.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.AirDefense) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.AirDefense = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対地防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSurfaceDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(surfaceDefenceTextBox.Text, out val))
            {
                surfaceDefenceTextBox.Text = model.SurfaceDefense.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SurfaceDefense) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SurfaceDefense = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     耐久力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToughnessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(toughnessTextBox.Text, out val))
            {
                toughnessTextBox.Text = model.Toughness.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Toughness) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Toughness = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     脆弱性テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSoftnessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(softnessTextBox.Text, out val))
            {
                softnessTextBox.Text = model.Softness.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Softness) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Softness = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対人攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSoftAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(softAttackTextBox.Text, out val))
            {
                softAttackTextBox.Text = model.SoftAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SoftAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SoftAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対甲攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHardAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(hardAttackTextBox.Text, out val))
            {
                hardAttackTextBox.Text = model.HardAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.HardAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.HardAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対艦攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(seaAttackTextBox.Text, out val))
            {
                seaAttackTextBox.Text = model.SeaAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SeaAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SeaAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対潜攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(subAttackTextBox.Text, out val))
            {
                subAttackTextBox.Text = model.SubAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SubAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SubAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     沿岸砲撃能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShoreBombardmentTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(shoreBombardmentTextBox.Text, out val))
            {
                shoreBombardmentTextBox.Text = model.ShoreBombardment.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ShoreBombardment) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ShoreBombardment = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対空攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(airAttackTextBox.Text, out val))
            {
                airAttackTextBox.Text = model.AirAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.AirAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.AirAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     空対艦攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavalAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(navalAttackTextBox.Text, out val))
            {
                navalAttackTextBox.Text = model.NavalAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.NavalAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.NavalAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     戦略爆撃攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStrategicAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(strategicAttackTextBox.Text, out val))
            {
                strategicAttackTextBox.Text = model.StrategicAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.StrategicAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.StrategicAttack = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     砲撃能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArtilleryBombardmentTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(artilleryBombardmentTextBox.Text, out val))
            {
                artilleryBombardmentTextBox.Text = model.ArtilleryBombardment.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ArtilleryBombardment) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ArtilleryBombardment = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     射程テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDistanceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(distanceTextBox.Text, out val))
            {
                distanceTextBox.Text = model.Distance.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Distance) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Distance = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     視認性テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVisibilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(visibilityTextBox.Text, out val))
            {
                visibilityTextBox.Text = model.Visibility.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.Visibility) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.Visibility = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対地索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSurfaceDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(surfaceDetectionCapabilityTextBox.Text, out val))
            {
                surfaceDetectionCapabilityTextBox.Text =
                    model.SurfaceDetectionCapability.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SurfaceDetectionCapability) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SurfaceDetectionCapability = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対潜索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(subDetectionCapabilityTextBox.Text, out val))
            {
                subDetectionCapabilityTextBox.Text = model.SubDetectionCapability.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.SubDetectionCapability) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.SubDetectionCapability = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     対空索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(airDetectionCapabilityTextBox.Text, out val))
            {
                airDetectionCapabilityTextBox.Text = model.AirDetectionCapability.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.AirDetectionCapability) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.AirDetectionCapability = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     燃料切れ補正テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoFuelCombatModTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(noFuelCombatModTextBox.Text, out val))
            {
                noFuelCombatModTextBox.Text = model.NoFuelCombatMod.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.NoFuelCombatMod) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.NoFuelCombatMod = val;

            // 編集済みフラグを設定する
            SetDirty();
        }

        #endregion

        #region 装備

        /// <summary>
        ///     装備リストビューの項目を更新する
        /// </summary>
        /// <param name="model"></param>
        private void UpdateEquipmentList(UnitModel model)
        {
            if (model.Equipments.Count == 0)
            {
                DisableEquipmentItems();
                return;
            }

            equipmentListView.BeginUpdate();
            equipmentListView.Items.Clear();
            foreach (UnitEquipment equipment in model.Equipments)
            {
                equipmentListView.Items.Add(CreateEquipmentListItem(equipment));
            }
            equipmentListView.EndUpdate();

            equipmentListView.Items[0].Focused = true;
            equipmentListView.Items[0].Selected = true;

            EnableEquipmentItems();
        }

        /// <summary>
        ///     装備の編集項目を有効化する
        /// </summary>
        private void EnableEquipmentItems()
        {
            resourceComboBox.Enabled = true;
            quantityTextBox.Enabled = true;

            equipmentRemoveButton.Enabled = true;
            equipmentUpButton.Enabled = true;
            equipmentDownButton.Enabled = true;
        }

        /// <summary>
        ///     装備の編集項目を無効化する
        /// </summary>
        private void DisableEquipmentItems()
        {
            resourceComboBox.Enabled = false;
            quantityTextBox.Enabled = false;

            equipmentRemoveButton.Enabled = false;
            equipmentUpButton.Enabled = false;
            equipmentDownButton.Enabled = false;
        }

        /// <summary>
        ///     装備リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集を禁止する
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                DisableEquipmentItems();
                return;
            }

            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            UnitEquipment equipment = model.Equipments[equipmentListView.SelectedIndices[0]];

            resourceComboBox.Text = equipment.Resource;
            quantityTextBox.Text = equipment.Quantity.ToString(CultureInfo.InvariantCulture);

            EnableEquipmentItems();
        }

        /// <summary>
        ///     資源コンボボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceComboBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];
            int index = equipmentListView.SelectedIndices[0];
            UnitEquipment equipment = model.Equipments[index];

            // 値に変化がなければ何もしない
            if (resourceComboBox.Text.Equals(equipment.Resource))
            {
                return;
            }

            // 値を更新する
            equipment.Resource = resourceComboBox.Text;

            // 装備リストビューの項目を更新する
            equipmentListView.Items[index].Text = resourceComboBox.Text;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuantityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];
            int index = modelListView.SelectedIndices[0];
            UnitEquipment equipment = model.Equipments[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(quantityTextBox.Text, out val))
            {
                quantityTextBox.Text = equipment.Quantity.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - equipment.Quantity) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            equipment.Quantity = val;

            // 装備リストビューの項目を更新する
            equipmentListView.Items[index].SubItems[1].Text = quantityTextBox.Text;

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     装備の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentAddButtonClick(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            var equipment = new UnitEquipment();
            model.Equipments.Add(equipment);

            AddEquipmentListItem(equipment);

            SetDirty();
        }

        /// <summary>
        ///     装備の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // 選択中の項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];

            RemoveEquipmentListItem(index);

            model.Equipments.RemoveAt(index);

            SetDirty();
        }

        /// <summary>
        ///     装備の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentUpButtonClick(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // リストの先頭ならば何もしない
            int index = equipmentListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            model.MoveEquipment(index, index - 1);

            MoveEquipmentListItem(index, index - 1);

            SetDirty();
        }

        /// <summary>
        ///     装備の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentDownButtonClick(object sender, EventArgs e)
        {
            // 選択中のモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }

            UnitType type = Units.Types[unitListBox.SelectedIndex];
            Unit unit = Units.List[(int) type];
            int no = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[no];

            // リストの先頭ならば何もしない
            int index = equipmentListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            model.MoveEquipment(index, index + 1);

            MoveEquipmentListItem(index, index + 1);

            SetDirty();
        }

        /// <summary>
        ///     装備リストの項目を作成する
        /// </summary>
        /// <param name="equipment">装備</param>
        /// <returns>装備リストの項目</returns>
        private static ListViewItem CreateEquipmentListItem(UnitEquipment equipment)
        {
            var item = new ListViewItem {Text = equipment.Resource};
            item.SubItems.Add(equipment.Quantity.ToString(CultureInfo.InvariantCulture));
            return item;
        }

        /// <summary>
        ///     装備リストの項目を追加する
        /// </summary>
        /// <param name="equipment">追加対象の装備</param>
        private void AddEquipmentListItem(UnitEquipment equipment)
        {
            ListViewItem listItem = CreateEquipmentListItem(equipment);

            equipmentListView.Items.Add(listItem);

            int index = equipmentListView.Items.Count - 1;
            equipmentListView.Items[index].Focused = true;
            equipmentListView.Items[index].Selected = true;
            equipmentListView.EnsureVisible(index);

            EnableEquipmentItems();
        }

        /// <summary>
        ///     装備リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveEquipmentListItem(int src, int dest)
        {
            var listItem = equipmentListView.Items[src].Clone() as ListViewItem;
            if (listItem == null)
            {
                return;
            }

            if (src > dest)
            {
                // 上へ移動する場合
                equipmentListView.Items.Insert(dest, listItem);
                equipmentListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                equipmentListView.Items.Insert(dest + 1, listItem);
                equipmentListView.Items.RemoveAt(src);
            }

            equipmentListView.Items[dest].Focused = true;
            equipmentListView.Items[dest].Selected = true;
            equipmentListView.EnsureVisible(dest);
        }

        /// <summary>
        ///     装備リストから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveEquipmentListItem(int index)
        {
            equipmentListView.Items.RemoveAt(index);

            if (index < equipmentListView.Items.Count)
            {
                equipmentListView.Items[index].Focused = true;
                equipmentListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                equipmentListView.Items[equipmentListView.Items.Count - 1].Focused = true;
                equipmentListView.Items[equipmentListView.Items.Count - 1].Selected = true;
            }
            else
            {
                DisableEquipmentItems();
            }
        }

        #endregion

        #endregion
    }
}