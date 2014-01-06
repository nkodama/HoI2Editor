using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

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

            // 自動スケーリングを考慮した初期化
            InitScaling();
        }

        /// <summary>
        ///     自動スケーリングを考慮した初期化
        /// </summary>
        private void InitScaling()
        {
            // ユニットモデルリストビュー
            noColumnHeader.Width = DeviceCaps.GetScaledWidth(noColumnHeader.Width);
            nameColumnHeader.Width = DeviceCaps.GetScaledWidth(nameColumnHeader.Width);
            buildCostColumnHeader.Width = DeviceCaps.GetScaledWidth(buildCostColumnHeader.Width);
            buildTimeColumnHeader.Width = DeviceCaps.GetScaledWidth(buildTimeColumnHeader.Width);
            manpowerColumnHeader.Width = DeviceCaps.GetScaledWidth(manpowerColumnHeader.Width);
            supplyColumnHeader.Width = DeviceCaps.GetScaledWidth(supplyColumnHeader.Width);
            fuelColumnHeader.Width = DeviceCaps.GetScaledWidth(fuelColumnHeader.Width);
            organisationColumnHeader.Width = DeviceCaps.GetScaledWidth(organisationColumnHeader.Width);
            moraleColumnHeader.Width = DeviceCaps.GetScaledWidth(moraleColumnHeader.Width);
            maxSpeedColumnHeader.Width = DeviceCaps.GetScaledWidth(maxSpeedColumnHeader.Width);

            // ユニットクラスリストボックス
            classListBox.ItemHeight = DeviceCaps.GetScaledHeight(classListBox.ItemHeight);

            // 国家リストボックス
            countryDummyColumnHeader.Width = DeviceCaps.GetScaledWidth(countryDummyColumnHeader.Width);

            // 改良リストビュー
            upgradeTypeColumnHeader.Width = DeviceCaps.GetScaledWidth(upgradeTypeColumnHeader.Width);
            upgradeCostColumnHeader.Width = DeviceCaps.GetScaledWidth(upgradeCostColumnHeader.Width);
            upgradeTimeColumnHeader.Width = DeviceCaps.GetScaledWidth(upgradeTimeColumnHeader.Width);

            // 装備リストビュー
            resourceColumnHeader.Width = DeviceCaps.GetScaledWidth(resourceColumnHeader.Width);
            quantityColumnHeader.Width = DeviceCaps.GetScaledWidth(quantityColumnHeader.Width);

            // 画面解像度が十分に広い場合は指揮官リストビューが広く表示できるようにする
            int longHeight = DeviceCaps.GetScaledHeight(720);
            if (Screen.GetWorkingArea(this).Height >= longHeight)
            {
                Height = longHeight;
            }
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitModelEditorFormLoad(object sender, EventArgs e)
        {
            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 国家データを初期化する
            Countries.Init();

            // ユニットデータを初期化する
            Units.Init();

            // Miscファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // ユニットデータを読み込む
            Units.Load();

            // データ読み込み後の処理
            OnUnitsLoaded();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 国家リストボックス
            foreach (string s in Countries.Strings.Where(country => !string.IsNullOrEmpty(country)))
            {
                countryListView.Items.Add(s);
            }

            // 兵科コンボボックス
            branchComboBox.Items.Add(Config.GetText("EYR_ARMY"));
            branchComboBox.Items.Add(Config.GetText("EYR_NAVY"));
            branchComboBox.Items.Add(Config.GetText("EYR_AIRFORCE"));

            // 付属可能旅団リストビュー
            allowedBrigadesListView.Items.Clear();
            int maxWidth = 60;
            foreach (UnitType type in Units.BrigadeTypes)
            {
                string s = Config.GetText(Units.Items[(int) type].Name);
                allowedBrigadesListView.Items.Add(s);
                // +16はチェックボックスの分
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, allowedBrigadesListView.Font).Width + DeviceCaps.GetScaledWidth(16));
            }
            allowedBrigadesDummyColumnHeader.Width = maxWidth;

            // 実ユニット種類コンボボックス
            realUnitTypeComboBox.Items.Clear();
            maxWidth = realUnitTypeComboBox.DropDownWidth;
            foreach (RealUnitType type in Enum.GetValues(typeof (RealUnitType)))
            {
                string s = Config.GetText(Units.RealNames[(int) type]);
                realUnitTypeComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, realUnitTypeComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            realUnitTypeComboBox.DropDownWidth = maxWidth;

            // スプライト種類コンボボックス
            spriteTypeComboBox.Items.Clear();
            maxWidth = spriteTypeComboBox.DropDownWidth;
            foreach (SpriteType type in Enum.GetValues(typeof (SpriteType)))
            {
                string s = Config.GetText(Units.SpriteNames[(int) type]);
                spriteTypeComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, spriteTypeComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            spriteTypeComboBox.DropDownWidth = maxWidth;

            // 代替ユニット種類コンボボックス
            transmuteComboBox.Items.Clear();
            maxWidth = transmuteComboBox.DropDownWidth;
            foreach (UnitType type in Units.DivisionTypes)
            {
                string s = Config.GetText(Units.Items[(int) type].Name);
                transmuteComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, transmuteComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            transmuteComboBox.DropDownWidth = maxWidth;

            // 更新ユニット種類コンボボックス
            upgradeTypeComboBox.Items.Clear();
            maxWidth = upgradeTypeComboBox.DropDownWidth;
            foreach (UnitType type in Units.DivisionTypes)
            {
                string s = Config.GetText(Units.Items[(int) type].Name);
                upgradeTypeComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, upgradeTypeComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            upgradeTypeComboBox.DropDownWidth = maxWidth;

            // 資源コンボボックス
            resourceComboBox.Items.Clear();
            maxWidth = resourceComboBox.DropDownWidth;
            foreach (EquipmentType type in Enum.GetValues(typeof (EquipmentType)))
            {
                string s = Config.GetText(Units.EquipmentNames[(int) type]);
                resourceComboBox.Items.Add(s);
                maxWidth = Math.Max(maxWidth,
                    (int) g.MeasureString(s, resourceComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
            }
            resourceComboBox.DropDownWidth = maxWidth;

            // チェックボックスの文字列
            cagCheckBox.Text = Config.GetText("NAME_CAG");
            escortCheckBox.Text = Config.GetText("NAME_ESCORT");
            engineerCheckBox.Text = Config.GetText("NAME_ENGINEER");
        }

        /// <summary>
        ///     ゲームの種類により編集項目を制限する
        /// </summary>
        private void RestrictEditableItems()
        {
            // AoD
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                maxSpeedStepLabel.Enabled = true;
                maxSpeedStepComboBox.Enabled = true;
                maxSupplyStockLabel.Enabled = true;
                maxSupplyStockTextBox.Enabled = true;
                maxOilStockLabel.Enabled = true;
                maxOilStockTextBox.Enabled = true;
                artilleryBombardmentLabel.Enabled = true;
                artilleryBombardmentTextBox.Enabled = true;
            }
            else
            {
                maxSpeedStepLabel.Enabled = false;
                maxSpeedStepComboBox.Enabled = false;
                maxSupplyStockLabel.Enabled = false;
                maxSupplyStockTextBox.Enabled = false;
                maxOilStockLabel.Enabled = false;
                maxOilStockTextBox.Enabled = false;
                artilleryBombardmentLabel.Enabled = false;
                artilleryBombardmentTextBox.Enabled = false;

                maxSpeedStepComboBox.ResetText();
                maxSupplyStockTextBox.ResetText();
                maxOilStockTextBox.ResetText();
                artilleryBombardmentTextBox.ResetText();
            }

            // DH
            if (Game.Type == GameType.DarkestHour)
            {
                maxAllowedBrigadesLabel.Enabled = true;
                maxAllowedBrigadesNumericUpDown.Enabled = true;
                upgradeGroupBox.Enabled = true;
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                noFuelCombatModLabel.Enabled = true;
                noFuelCombatModTextBox.Enabled = true;
            }
            else
            {
                maxAllowedBrigadesLabel.Enabled = false;
                maxAllowedBrigadesNumericUpDown.Enabled = false;
                reinforceCostLabel.Enabled = false;
                reinforceCostTextBox.Enabled = false;
                reinforceTimeLabel.Enabled = false;
                reinforceTimeTextBox.Enabled = false;
                noFuelCombatModLabel.Enabled = false;
                noFuelCombatModTextBox.Enabled = false;
                upgradeGroupBox.Enabled = false;

                maxAllowedBrigadesNumericUpDown.ResetText();
                reinforceCostTextBox.ResetText();
                reinforceTimeTextBox.ResetText();
                noFuelCombatModTextBox.ResetText();
                upgradeListView.Items.Clear();
                upgradeTypeComboBox.SelectedIndex = -1;
                upgradeTypeComboBox.ResetText();
                upgradeCostTextBox.ResetText();
                upgradeTimeTextBox.ResetText();
            }

            // DH1.03以降
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                productableCheckBox.Enabled = true;
                cagCheckBox.Enabled = true;
                escortCheckBox.Enabled = true;
                engineerCheckBox.Enabled = true;
                eyrLabel.Enabled = true;
                eyrNumericUpDown.Enabled = true;
                gfxPrioLabel.Enabled = true;
                gfxPrioNumericUpDown.Enabled = true;
                listPrioLabel.Enabled = true;
                listPrioNumericUpDown.Enabled = true;
                uiPrioLabel.Enabled = true;
                uiPrioNumericUpDown.Enabled = true;
                realUnitTypeLabel.Enabled = true;
                realUnitTypeComboBox.Enabled = true;
                defaultTypeCheckBox.Enabled = true;
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
                cagCheckBox.Enabled = false;
                escortCheckBox.Enabled = false;
                engineerCheckBox.Enabled = false;
                eyrLabel.Enabled = false;
                eyrNumericUpDown.Enabled = false;
                gfxPrioLabel.Enabled = false;
                gfxPrioNumericUpDown.Enabled = false;
                listPrioLabel.Enabled = false;
                listPrioNumericUpDown.Enabled = false;
                uiPrioLabel.Enabled = false;
                uiPrioNumericUpDown.Enabled = false;
                realUnitTypeLabel.Enabled = false;
                realUnitTypeComboBox.Enabled = false;
                defaultTypeCheckBox.Enabled = false;
                spriteTypeLabel.Enabled = false;
                spriteTypeComboBox.Enabled = false;
                transmuteLabel.Enabled = false;
                transmuteComboBox.Enabled = false;
                militaryValueLabel.Enabled = false;
                militaryValueTextBox.Enabled = false;
                speedCapAllLabel.Enabled = false;
                speedCapAllTextBox.Enabled = false;
                equipmentGroupBox.Enabled = false;

                productableCheckBox.Checked = false;
                detachableCheckBox.Checked = false;
                cagCheckBox.Checked = false;
                escortCheckBox.Checked = false;
                engineerCheckBox.Checked = false;
                eyrNumericUpDown.ResetText();
                gfxPrioNumericUpDown.ResetText();
                listPrioNumericUpDown.ResetText();
                uiPrioNumericUpDown.ResetText();
                realUnitTypeComboBox.SelectedIndex = -1;
                realUnitTypeComboBox.ResetText();
                spriteTypeComboBox.SelectedIndex = -1;
                spriteTypeComboBox.ResetText();
                transmuteComboBox.SelectedIndex = -1;
                transmuteComboBox.ResetText();
                militaryValueTextBox.ResetText();
                speedCapAllTextBox.ResetText();
                equipmentListView.Items.Clear();
                resourceComboBox.SelectedIndex = -1;
                resourceComboBox.ResetText();
                quantityTextBox.ResetText();
            }

            // AoDまたはDH1.03以降
            if (Game.Type == GameType.ArsenalOfDemocracy || (Game.Type == GameType.DarkestHour && Game.Version >= 103))
            {
                branchComboBox.Enabled = true;
                detachableCheckBox.Enabled = true;
            }
            else
            {
                branchComboBox.Enabled = false;
                detachableCheckBox.Enabled = true;
            }
        }

        /// <summary>
        ///     付属可能旅団数を初期化する
        /// </summary>
        private static void InitMaxAllowedBrigades()
        {
            // AoD1.07以降のみ
            if (Game.Type != GameType.ArsenalOfDemocracy || Game.Version < 107)
            {
                return;
            }

            Units.Items[(int) UnitType.Transport].MaxAllowedBrigades = Misc.TpMaxAttach;
            Units.Items[(int) UnitType.Submarine].MaxAllowedBrigades = Misc.SsMaxAttach;
            Units.Items[(int) UnitType.NuclearSubmarine].MaxAllowedBrigades = Misc.SsnMaxAttach;
            Units.Items[(int) UnitType.Destroyer].MaxAllowedBrigades = Misc.DdMaxAttach;
            Units.Items[(int) UnitType.LightCruiser].MaxAllowedBrigades = Misc.ClMaxAttach;
            Units.Items[(int) UnitType.HeavyCruiser].MaxAllowedBrigades = Misc.CaMaxAttach;
            Units.Items[(int) UnitType.BattleCruiser].MaxAllowedBrigades = Misc.BcMaxAttach;
            Units.Items[(int) UnitType.BattleShip].MaxAllowedBrigades = Misc.BbMaxAttach;
            Units.Items[(int) UnitType.LightCarrier].MaxAllowedBrigades = Misc.CvlMaxAttach;
            Units.Items[(int) UnitType.Carrier].MaxAllowedBrigades = Misc.CvMaxAttach;
        }

        #endregion

        #region 終了処理

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2Editor.IsDirty())
            {
                return;
            }

            // 保存するかを問い合わせる
            DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    HoI2Editor.SaveFiles();
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnUnitEditorFormClosed();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.SaveFiles();
                        break;
                }
            }

            HoI2Editor.ReloadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.SaveFiles();
        }

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnUnitsLoaded()
        {
            // 付属可能旅団の値が変化してしまうので一旦選択を解除する
            classListBox.SelectedIndex = -1;

            // Miscの値に応じて付属可能旅団数を初期化する
            InitMaxAllowedBrigades();

            // 編集項目を初期化する
            InitEditableItems();

            // ゲームの種類により編集項目を制限する
            RestrictEditableItems();

            // ユニットリストを更新する
            UpdateUnitList();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnUnitsSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            classListBox.Refresh();
            modelListView.Refresh();
            UpdateClassEditableItems();
            UpdateModelEditableItems();
        }

        #endregion

        #region ユニットクラスリスト

        /// <summary>
        ///     ユニットクラスリストを更新する
        /// </summary>
        private void UpdateUnitList()
        {
            // リストボックスに項目を登録する
            classListBox.BeginUpdate();
            classListBox.Items.Clear();
            foreach (UnitType type in Units.UnitTypes)
            {
                Unit unit = Units.Items[(int) type];
                classListBox.Items.Add(Config.GetText(unit.Name));
            }
            classListBox.EndUpdate();

            // 先頭の項目を選択する
            if (classListBox.Items.Count > 0)
            {
                classListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     ユニットクラスリストの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            Unit unit = Units.Items[(int) Units.UnitTypes[e.Index]];

            // 背景を描画する
            e.DrawBackground();
            if (((e.State & DrawItemState.Selected) == 0) && (unit.Models.Count > 0))
            {
                e.Graphics.FillRectangle(
                    unit.Organization == UnitOrganization.Division ? Brushes.AliceBlue : Brushes.Honeydew,
                    new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                brush = unit.IsDirty() ? new SolidBrush(Color.Red) : new SolidBrush(classListBox.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = classListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ユニットクラスリストの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // ユニットモデルリストを更新する
            UpdateModelList();

            // ユニットクラスの編集項目を更新する
            UpdateClassEditableItems();

            // ユニットモデル編集中の場合
            if (editTabControl.SelectedIndex == (int) UnitEditorTab.Model)
            {
                if (modelListView.Items.Count > 0)
                {
                    // 先頭の項目を選択する
                    modelListView.Items[0].Focused = true;
                    modelListView.Items[0].Selected = true;
                }
                else
                {
                    // ユニットクラスタブを選択する
                    editTabControl.SelectedIndex = (int) UnitEditorTab.Class;

                    // ユニットモデルの編集項目を無効化する
                    DisableModelEditableItems();
                }
            }
        }

        #endregion

        #region ユニットモデルリスト

        /// <summary>
        ///     ユニットモデルリストの表示を更新する
        /// </summary>
        private void UpdateModelList()
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // リストビューに項目を登録する
            modelListView.BeginUpdate();
            modelListView.Items.Clear();
            for (int i = 0; i < unit.Models.Count; i++)
            {
                ListViewItem item = CreateModelListItem(unit, i);
                modelListView.Items.Add(item);
            }
            modelListView.EndUpdate();
        }

        /// <summary>
        ///     ユニットモデルリストのモデル名を更新する
        /// </summary>
        private void UpdateModelListName()
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // リストビューの項目を更新する
            Country country = (countryListView.SelectedIndices.Count == 0)
                ? Country.None
                : (Country) (countryListView.SelectedIndices[0] + 1);
            modelListView.BeginUpdate();
            for (int i = 0; i < unit.Models.Count; i++)
            {
                modelListView.Items[i].SubItems[1].Text = Config.GetText(UnitModel.GetName(unit, i, country));
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
            // 選択中のユニットモデルがない場合
            if (modelListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableModelEditableItems();
                return;
            }

            // ユニットモデルの編集項目を有効化する
            EnableModelEditableItems();

            // ユニットモデルの編集項目の値を更新する
            UpdateModelEditableItems();

            // ユニットモデルタブを選択する
            editTabControl.SelectedIndex = (int) UnitEditorTab.Model;

            // 項目移動ボタンの状態更新
            int index = modelListView.SelectedIndices[0];
            topButton.Enabled = (index != 0);
            upButton.Enabled = (index != 0);
            downButton.Enabled = (index != modelListView.Items.Count - 1);
            bottomButton.Enabled = (index != modelListView.Items.Count - 1);
        }

        /// <summary>
        ///     ユニットモデルリストビューの項目を作成する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデルリストビューの項目</returns>
        private ListViewItem CreateModelListItem(Unit unit, int index)
        {
            UnitModel model = unit.Models[index];

            var item = new ListViewItem {Text = Config.GetText(index.ToString(CultureInfo.InvariantCulture))};
            item.SubItems.Add(Config.GetText(UnitModel.GetName(
                unit, index, countryListView.SelectedIndices.Count == 0
                    ? Country.None
                    : (Country) (countryListView.SelectedIndices[0] + 1))));
            item.SubItems.Add(model.Cost.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.BuildTime.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.ManPower.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.SupplyConsumption.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.FuelConsumption.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.DefaultOrganization.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.Morale.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(model.MaxSpeed.ToString(CultureInfo.InvariantCulture));

            return item;
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // ユニットモデルを挿入する
            var model = new UnitModel();
            if (modelListView.SelectedIndices.Count > 0)
            {
                int index = modelListView.SelectedIndices[0];
                InsertModel(unit, model, index + 1, "");
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
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // ユニットモデルを挿入する
            var model = new UnitModel(unit.Models[index]);
            InsertModel(unit, model, index + 1, Config.GetText(UnitModel.GetName(unit, index, Country.None)));
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // ユニットモデルを削除する
            RemoveModel(unit, index);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // リストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, index, 0);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // リストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, index, index - 1);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // リストの末尾ならば何もしない
            if (index == unit.Models.Count - 1)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, index, index + 1);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottonButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // リストの末尾ならば何もしない
            if (index == unit.Models.Count - 1)
            {
                return;
            }

            // ユニットモデルを移動する
            MoveModel(unit, index, unit.Models.Count - 1);
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
            for (int i = unit.Models.Count - 1; i > index; i--)
            {
                Config.SetText(UnitModel.GetName(unit, i, Country.None),
                    Config.GetText(UnitModel.GetName(unit, i - 1, Country.None)), Game.UnitTextFileName);
            }

            // 挿入位置のユニットモデル名を変更する
            Config.SetText(UnitModel.GetName(unit, index, Country.None), name, Game.UnitTextFileName);

            // 編集済みフラグを設定する
            model.SetDirtyAll();
            unit.SetDirty();

            // ユニットモデルリストの表示を更新する
            UpdateModelList();

            // 挿入した項目を選択する
            modelListView.Items[index].Focused = true;
            modelListView.Items[index].Selected = true;
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
                    Config.SetText(UnitModel.GetName(unit, i, Country.None),
                        Config.GetText(UnitModel.GetName(unit, i + 1, Country.None)),
                        Game.UnitTextFileName);
                }
            }

            // 末尾のユニットモデル名を削除する
            Config.RemoveText(UnitModel.GetName(unit, unit.Models.Count, Country.None), Game.UnitTextFileName);

            // 編集済みフラグを設定する
            unit.SetDirty();

            // ユニットモデルリストの表示を更新する
            UpdateModelList();

            // 削除した項目の次の項目を選択する
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
            string name = Config.GetText(UnitModel.GetName(unit, src, Country.None));
            if (src > dest)
            {
                // 上へ移動する場合
                for (int i = src; i > dest; i--)
                {
                    Config.SetText(UnitModel.GetName(unit, i, Country.None),
                        Config.GetText(UnitModel.GetName(unit, i - 1, Country.None)),
                        Game.UnitTextFileName);
                }
            }
            else
            {
                // 下へ移動する場合
                for (int i = src; i < dest; i++)
                {
                    Config.SetText(UnitModel.GetName(unit, i, Country.None),
                        Config.GetText(UnitModel.GetName(unit, i + 1, Country.None)),
                        Game.UnitTextFileName);
                }
            }

            // 移動先のユニットモデル名を変更する
            Config.SetText(UnitModel.GetName(unit, dest, Country.None), name, Game.UnitTextFileName);

            // 編集済みフラグを設定する
            unit.SetDirty();

            // ユニットモデルリストの表示を更新する
            UpdateModelList();

            // 移動先の項目を選択する
            modelListView.Items[dest].Focused = true;
            modelListView.Items[dest].Selected = true;
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
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // ユニットモデルリストのモデル名を更新する
            UpdateModelListName();

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            Country country = (countryListView.SelectedIndices.Count == 0
                ? Country.None
                : (Country) (countryListView.SelectedIndices[0] + 1));

            // ユニットモデル画像名を更新する
            modelImagePictureBox.ImageLocation = GetModelImageFileName(unit, index, country);

            // ユニットモデル名を更新する
            modelNameTextBox.Text = Config.GetText(UnitModel.GetName(unit, index, country));

            // ユニットモデル名の表示色を更新する
            modelNameTextBox.ForeColor = unit.Models[index].IsDirtyName(country) ? Color.Red : SystemColors.WindowText;
        }

        #endregion

        #region ユニットクラスタブ

        /// <summary>
        ///     ユニットクラスタブの編集項目を更新する
        /// </summary>
        private void UpdateClassEditableItems()
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            classNameTextBox.Text = Config.GetText(unit.Name);
            classShortNameTextBox.Text = Config.GetText(unit.ShortName);
            classDescTextBox.Text = Config.GetText(unit.Desc);
            classShortDescTextBox.Text = Config.GetText(unit.ShortDesc);

            // 兵科
            branchComboBox.SelectedIndex = (int) unit.Branch - 1;
            if (Game.Type == GameType.ArsenalOfDemocracy ||
                (Game.Type == GameType.DarkestHour && Game.Version >= 103))
            {
                branchComboBox.Enabled = true;
            }
            else
            {
                branchComboBox.Enabled = false;
            }

            // 付属旅団
            if (unit.Organization == UnitOrganization.Division)
            {
                if (Game.Type == GameType.DarkestHour)
                {
                    maxAllowedBrigadesLabel.Enabled = true;
                    maxAllowedBrigadesNumericUpDown.Enabled = true;
                }
                else
                {
                    maxAllowedBrigadesLabel.Enabled = false;
                    maxAllowedBrigadesNumericUpDown.Enabled = false;
                }
                maxAllowedBrigadesNumericUpDown.Value = unit.MaxAllowedBrigades;
                maxAllowedBrigadesNumericUpDown.Text =
                    maxAllowedBrigadesNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);

                allowedBrigadesListView.Enabled = true;
                for (int i = 0; i < Units.BrigadeTypes.Count(); i++)
                {
                    UnitType type = Units.BrigadeTypes[i];
                    allowedBrigadesListView.Items[i].Checked = unit.AllowedBrigades.Contains(type);
                }
            }
            else
            {
                maxAllowedBrigadesLabel.Enabled = false;
                maxAllowedBrigadesNumericUpDown.Enabled = false;
                maxAllowedBrigadesNumericUpDown.ResetText();

                allowedBrigadesListView.Enabled = false;
                for (int i = 0; i < Units.BrigadeTypes.Count(); i++)
                {
                    allowedBrigadesListView.Items[i].Checked = false;
                }
            }

            // DH1.03以降のユニット設定
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                listPrioLabel.Enabled = true;
                listPrioNumericUpDown.Enabled = true;

                // 師団
                if (unit.Organization == UnitOrganization.Division)
                {
                    eyrLabel.Enabled = true;
                    eyrNumericUpDown.Enabled = true;
                    gfxPrioLabel.Enabled = true;
                    gfxPrioNumericUpDown.Enabled = true;
                    uiPrioLabel.Enabled = true;
                    uiPrioNumericUpDown.Enabled = true;
                    realUnitTypeLabel.Enabled = true;
                    realUnitTypeComboBox.Enabled = true;
                    spriteTypeLabel.Enabled = true;
                    spriteTypeComboBox.Enabled = true;
                    transmuteLabel.Enabled = true;
                    transmuteComboBox.Enabled = true;
                    militaryValueLabel.Enabled = true;
                    militaryValueTextBox.Enabled = true;
                    defaultTypeCheckBox.Enabled = true;
                    productableCheckBox.Enabled = true;

                    eyrNumericUpDown.Value = unit.Eyr;
                    eyrNumericUpDown.Text = eyrNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
                    gfxPrioNumericUpDown.Value = unit.GfxPrio;
                    gfxPrioNumericUpDown.Text = gfxPrioNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
                    uiPrioNumericUpDown.Value = unit.UiPrio;
                    uiPrioNumericUpDown.Text = uiPrioNumericUpDown.Value.ToString(CultureInfo.InvariantCulture);
                    realUnitTypeComboBox.SelectedIndex = (int) unit.RealType;
                    spriteTypeComboBox.SelectedIndex = (int) unit.Sprite;
                    transmuteComboBox.SelectedIndex = Units.UnitTypes.IndexOf(unit.Transmute);
                    militaryValueTextBox.Text = unit.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    eyrLabel.Enabled = false;
                    eyrNumericUpDown.Enabled = false;
                    gfxPrioLabel.Enabled = false;
                    gfxPrioNumericUpDown.Enabled = false;
                    uiPrioLabel.Enabled = false;
                    uiPrioNumericUpDown.Enabled = false;
                    realUnitTypeLabel.Enabled = false;
                    realUnitTypeComboBox.Enabled = false;
                    spriteTypeLabel.Enabled = false;
                    spriteTypeComboBox.Enabled = false;
                    transmuteLabel.Enabled = false;
                    transmuteComboBox.Enabled = false;
                    militaryValueLabel.Enabled = false;
                    militaryValueTextBox.Enabled = false;
                    defaultTypeCheckBox.Enabled = false;
                    productableCheckBox.Enabled = false;

                    eyrNumericUpDown.ResetText();
                    gfxPrioNumericUpDown.ResetText();
                    uiPrioNumericUpDown.ResetText();
                    realUnitTypeComboBox.SelectedIndex = -1;
                    realUnitTypeComboBox.ResetText();
                    spriteTypeComboBox.SelectedIndex = -1;
                    spriteTypeComboBox.ResetText();
                    transmuteComboBox.SelectedIndex = -1;
                    transmuteComboBox.ResetText();
                    militaryValueTextBox.ResetText();
                }

                // 陸軍旅団
                if (unit.Branch == Branch.Army && unit.Organization == UnitOrganization.Brigade)
                {
                    engineerCheckBox.Enabled = true;
                }
                else
                {
                    engineerCheckBox.Enabled = false;
                }

                // 海軍旅団
                if (unit.Branch == Branch.Navy && unit.Organization == UnitOrganization.Brigade)
                {
                    cagCheckBox.Enabled = true;
                }
                else
                {
                    cagCheckBox.Enabled = false;
                }

                // 空軍旅団
                if (unit.Branch == Branch.Airforce && unit.Organization == UnitOrganization.Brigade)
                {
                    escortCheckBox.Enabled = true;
                }
                else
                {
                    escortCheckBox.Enabled = false;
                }
            }
            else
            {
                eyrLabel.Enabled = false;
                eyrNumericUpDown.Enabled = false;
                gfxPrioLabel.Enabled = false;
                gfxPrioNumericUpDown.Enabled = false;
                listPrioLabel.Enabled = false;
                listPrioNumericUpDown.Enabled = false;
                uiPrioLabel.Enabled = false;
                uiPrioNumericUpDown.Enabled = false;
                realUnitTypeLabel.Enabled = false;
                realUnitTypeComboBox.Enabled = false;
                spriteTypeLabel.Enabled = false;
                spriteTypeComboBox.Enabled = false;
                transmuteLabel.Enabled = false;
                transmuteComboBox.Enabled = false;
                militaryValueLabel.Enabled = false;
                militaryValueTextBox.Enabled = false;
                cagCheckBox.Enabled = false;
                escortCheckBox.Enabled = false;
                engineerCheckBox.Enabled = false;
                defaultTypeCheckBox.Enabled = false;
                productableCheckBox.Enabled = false;

                eyrNumericUpDown.ResetText();
                gfxPrioNumericUpDown.ResetText();
                listPrioNumericUpDown.ResetText();
                uiPrioNumericUpDown.ResetText();
                realUnitTypeComboBox.SelectedIndex = -1;
                realUnitTypeComboBox.ResetText();
                spriteTypeComboBox.SelectedIndex = -1;
                spriteTypeComboBox.ResetText();
                transmuteComboBox.SelectedIndex = -1;
                transmuteComboBox.ResetText();
                militaryValueTextBox.ResetText();
            }

            // 最大生産速度
            if ((Game.Type == GameType.ArsenalOfDemocracy) && (unit.Organization == UnitOrganization.Division))
            {
                maxSpeedStepLabel.Enabled = true;
                maxSpeedStepComboBox.Enabled = true;
                maxSpeedStepComboBox.SelectedIndex = unit.MaxSpeedStep;
            }
            else
            {
                maxSpeedStepLabel.Enabled = false;
                maxSpeedStepComboBox.Enabled = false;
                maxSpeedStepComboBox.SelectedIndex = -1;
                maxSpeedStepComboBox.ResetText();
            }

            // 着脱可能
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                detachableCheckBox.Enabled = (unit.Organization == UnitOrganization.Brigade);
            }
            else if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                detachableCheckBox.Enabled = ((unit.Branch == Branch.Navy) &&
                                              (unit.Organization == UnitOrganization.Brigade));
            }
            else
            {
                detachableCheckBox.Enabled = false;
            }

            detachableCheckBox.Checked = unit.Detachable;
            cagCheckBox.Checked = unit.Cag;
            escortCheckBox.Checked = unit.Escort;
            engineerCheckBox.Checked = unit.Engineer;
            defaultTypeCheckBox.Checked = unit.DefaultType;
            productableCheckBox.Checked = unit.Productable;

            // 改良
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                upgradeGroupBox.Enabled = unit.Organization == UnitOrganization.Division;
                UpdateUpgradeList(unit);
            }
            else
            {
                upgradeGroupBox.Enabled = false;
            }

            // 編集項目の色を設定する
            classNameTextBox.ForeColor = unit.IsDirty(UnitClassItemId.Name) ? Color.Red : SystemColors.WindowText;
            classShortNameTextBox.ForeColor = unit.IsDirty(UnitClassItemId.ShortName)
                ? Color.Red
                : SystemColors.WindowText;
            classDescTextBox.ForeColor = unit.IsDirty(UnitClassItemId.Desc) ? Color.Red : SystemColors.WindowText;
            classShortDescTextBox.ForeColor = unit.IsDirty(UnitClassItemId.ShortDesc)
                ? Color.Red
                : SystemColors.WindowText;
            eyrNumericUpDown.ForeColor = unit.IsDirty(UnitClassItemId.Eyr) ? Color.Red : SystemColors.WindowText;
            gfxPrioNumericUpDown.ForeColor = unit.IsDirty(UnitClassItemId.GfxPrio) ? Color.Red : SystemColors.WindowText;
            listPrioNumericUpDown.ForeColor = unit.IsDirty(UnitClassItemId.ListPrio)
                ? Color.Red
                : SystemColors.WindowText;
            uiPrioNumericUpDown.ForeColor = unit.IsDirty(UnitClassItemId.UiPrio) ? Color.Red : SystemColors.WindowText;
            militaryValueTextBox.ForeColor = unit.IsDirty(UnitClassItemId.Vaule) ? Color.Red : SystemColors.WindowText;
            detachableCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.Detachable)
                ? Color.Red
                : SystemColors.WindowText;
            cagCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.Cag) ? Color.Red : SystemColors.WindowText;
            escortCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.Escort) ? Color.Red : SystemColors.WindowText;
            engineerCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.Engineer) ? Color.Red : SystemColors.WindowText;
            defaultTypeCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.DefaultType)
                ? Color.Red
                : SystemColors.WindowText;
            productableCheckBox.ForeColor = unit.IsDirty(UnitClassItemId.Productable)
                ? Color.Red
                : SystemColors.WindowText;
            maxAllowedBrigadesNumericUpDown.ForeColor =
                unit.IsDirty(UnitClassItemId.MaxAllowedBrigades) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     兵科コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) unit.Branch - 1) && unit.IsDirty(UnitClassItemId.Branch))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = branchComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     実ユニット種類コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRealUnitTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) unit.RealType) && unit.IsDirty(UnitClassItemId.RealType))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = realUnitTypeComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     スプライト種類コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpriteTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) unit.Sprite) && unit.IsDirty(UnitClassItemId.Sprite))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = spriteTypeComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     代替ユニットコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransmuteComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) unit.Transmute) && unit.IsDirty(UnitClassItemId.Transmute))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = transmuteComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     最大生産速度コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSpeedStepComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == unit.MaxSpeedStep) && unit.IsDirty(UnitClassItemId.MaxSpeedStep))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = maxSpeedStepComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ユニットクラス名テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (classNameTextBox.Text.Equals(Config.GetText(unit.Name)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.Name, classNameTextBox.Text, Game.UnitTextFileName);

            // ユニットクラスリストボックスの項目を再設定することで表示更新している
            // この時再選択によりフォーカスが外れるので、イベントハンドラを一時的に無効化する
            classListBox.SelectedIndexChanged -= OnClassListBoxSelectedIndexChanged;
            classListBox.Items[classListBox.SelectedIndex] = classNameTextBox.Text;
            classListBox.SelectedIndexChanged += OnClassListBoxSelectedIndexChanged;


            if (unit.Organization == UnitOrganization.Division)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                int margin = DeviceCaps.GetScaledWidth(2) + 1;

                // 実ユニットコンボボックスの項目を更新する
                int index = Array.IndexOf(Units.RealNames, unit.Name);
                if (index >= 0)
                {
                    realUnitTypeComboBox.Items[index] = classNameTextBox.Text;
                    // ドロップダウン幅を更新する
                    realUnitTypeComboBox.DropDownWidth =
                        Math.Max(realUnitTypeComboBox.DropDownWidth,
                            (int) g.MeasureString(classNameTextBox.Text, realUnitTypeComboBox.Font).Width +
                            SystemInformation.VerticalScrollBarWidth + margin);
                }

                // スプライトコンボボックスの項目を更新する
                index = Array.IndexOf(Units.SpriteNames, unit.Name);
                if (index >= 0)
                {
                    spriteTypeComboBox.Items[index] = classNameTextBox.Text;
                    // ドロップダウン幅を更新する
                    spriteTypeComboBox.DropDownWidth =
                        Math.Max(spriteTypeComboBox.DropDownWidth,
                            (int) g.MeasureString(classNameTextBox.Text, spriteTypeComboBox.Font).Width +
                            SystemInformation.VerticalScrollBarWidth + margin);
                }

                // 代替ユニットコンボボックスの項目を更新する
                transmuteComboBox.Items[classListBox.SelectedIndex] = classNameTextBox.Text;
                // ドロップダウン幅を更新する
                transmuteComboBox.DropDownWidth =
                    Math.Max(transmuteComboBox.DropDownWidth,
                        (int) g.MeasureString(classNameTextBox.Text, transmuteComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);

                // 更新ユニットコンボボックスの項目を更新する
                upgradeTypeComboBox.Items[classListBox.SelectedIndex] = classNameTextBox.Text;
                // ドロップダウン幅を更新する
                upgradeTypeComboBox.DropDownWidth =
                    Math.Max(upgradeTypeComboBox.DropDownWidth,
                        (int) g.MeasureString(classNameTextBox.Text, upgradeTypeComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
            }
            else
            {
                // 付属旅団リストビューの項目を更新する
                int index = Array.IndexOf(Units.BrigadeTypes, unit.Type);
                if (index >= 0)
                {
                    allowedBrigadesListView.Items[index].Text = classNameTextBox.Text;
                }
            }


            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Name);

            // 文字色を変更する
            classNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ユニットクラス短縮名テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassShortNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (classShortNameTextBox.Text.Equals(Config.GetText(unit.ShortName)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.ShortName, classShortNameTextBox.Text, Game.UnitTextFileName);

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.ShortName);

            // 文字色を変更する
            classShortNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ユニットクラス説明テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassDescTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (classDescTextBox.Text.Equals(Config.GetText(unit.Desc)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.Desc, classDescTextBox.Text, Game.UnitTextFileName);

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Desc);

            // 文字色を変更する
            classDescTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ユニットクラス短縮説明テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassShortDescTextBox(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (classShortDescTextBox.Text.Equals(Config.GetText(unit.ShortDesc)))
            {
                return;
            }

            // 値を更新する
            Config.SetText(unit.ShortDesc, classShortDescTextBox.Text, Game.UnitTextFileName);

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.ShortDesc);

            // 文字色を変更する
            classShortDescTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     兵科コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var branch = (Branch) (branchComboBox.SelectedIndex + 1);
            if (branch == unit.Branch)
            {
                return;
            }

            // 値を更新する
            unit.Branch = branch;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Branch);
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                unit.SetDirty();
            }
            else if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                if (unit.Organization == UnitOrganization.Division)
                {
                    Units.SetDirtyDivisionTypes();
                }
                else
                {
                    Units.SetDirtyBrigadeTypes();
                }
            }

            // 兵科コンボボックスの項目色を変更するために描画更新する
            branchComboBox.Refresh();
        }

        /// <summary>
        ///     統計グループ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEyrNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var eyr = (int) eyrNumericUpDown.Value;
            if (eyr == unit.Eyr)
            {
                return;
            }

            // 値を更新する
            unit.Eyr = eyr;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Eyr);
            Units.SetDirtyDivisionTypes();

            // 文字色を変更する
            eyrNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     画像優先度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGraphicsPriorityNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var prio = (int) gfxPrioNumericUpDown.Value;
            if (prio == unit.GfxPrio)
            {
                return;
            }

            // 値を更新する
            unit.GfxPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.GfxPrio);
            Units.SetDirtyDivisionTypes();

            // 文字色を変更する
            gfxPrioNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     リスト優先度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListPrioNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var prio = (int) listPrioNumericUpDown.Value;
            if (prio == unit.ListPrio)
            {
                return;
            }

            // 値を更新する
            unit.ListPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.ListPrio);
            if (unit.Organization == UnitOrganization.Division)
            {
                Units.SetDirtyDivisionTypes();
            }
            else
            {
                Units.SetDirtyBrigadeTypes();
            }

            // 文字色を変更する
            listPrioNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     UI優先度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUiPrioNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var prio = (int) uiPrioNumericUpDown.Value;
            if (prio == unit.UiPrio)
            {
                return;
            }

            // 値を更新する
            unit.UiPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.UiPrio);
            Units.SetDirtyDivisionTypes();

            // 文字色を変更する
            uiPrioNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     実ユニット種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRealUnitTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var type = (RealUnitType) realUnitTypeComboBox.SelectedIndex;
            if (type == unit.RealType)
            {
                return;
            }

            // 値を更新する
            unit.RealType = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.RealType);
            Units.SetDirtyDivisionTypes();

            // 実ユニット種類コンボボックスの項目色を変更するために描画更新する
            realUnitTypeComboBox.Refresh();
        }

        /// <summary>
        ///     標準の生産タイプチェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultTypeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (defaultTypeCheckBox.Checked == unit.DefaultType)
            {
                return;
            }

            // 値を更新する
            unit.DefaultType = defaultTypeCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.DefaultType);
            Units.SetDirtyDivisionTypes();

            // 文字色を変更する
            defaultTypeCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     スプライト種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpriteTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var type = (SpriteType) spriteTypeComboBox.SelectedIndex;
            if (type == unit.Sprite)
            {
                return;
            }

            // 値を更新する
            unit.Sprite = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Sprite);
            Units.SetDirtyDivisionTypes();

            // スプライト種類コンボボックスの項目色を変更するために描画更新する
            spriteTypeComboBox.Refresh();
        }

        /// <summary>
        ///     代替ユニットコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransmuteComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            var type = (UnitType) transmuteComboBox.SelectedIndex;
            if (type == unit.Transmute)
            {
                return;
            }

            // 値を更新する
            unit.Transmute = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Transmute);
            Units.SetDirtyDivisionTypes();

            // 代替ユニットコンボボックスの項目色を変更するために描画更新する
            transmuteComboBox.Refresh();
        }

        /// <summary>
        ///     軍事力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMilitaryValueTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

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
            unit.SetDirty(UnitClassItemId.Vaule);
            if (unit.Organization == UnitOrganization.Division)
            {
                Units.SetDirtyDivisionTypes();
            }
            else
            {
                Units.SetDirtyBrigadeTypes();
            }

            // 文字色を変更する
            militaryValueTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     最大生産速度コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSpeedStepComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 非選択になった時には何もしない
            if (maxSpeedStepComboBox.SelectedIndex == -1)
            {
                return;
            }

            // 値に変化がなければ何もしない
            int val = maxSpeedStepComboBox.SelectedIndex;
            if (val == unit.MaxSpeedStep)
            {
                return;
            }

            // 値を更新する
            unit.MaxSpeedStep = val;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.MaxSpeedStep);
            unit.SetDirty();

            // 最大生産速度コンボボックスの項目色を変更するために描画更新する
            maxSpeedStepComboBox.Refresh();
        }

        /// <summary>
        ///     生産可能チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProductableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (productableCheckBox.Checked == unit.Productable)
            {
                return;
            }

            // 値を更新する
            unit.Productable = productableCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Productable);
            Units.SetDirtyDivisionTypes();

            // 文字色を変更する
            productableCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     着脱可能チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDetachableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (detachableCheckBox.Checked == unit.Detachable)
            {
                return;
            }

            // 値を更新する
            unit.Detachable = detachableCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Detachable);
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                unit.SetDirty();
            }
            else if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                Units.SetDirtyBrigadeTypes();
            }

            // 文字色を変更する
            detachableCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     空母航空隊チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCagCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (cagCheckBox.Checked == unit.Cag)
            {
                return;
            }

            // 値を更新する
            unit.Cag = cagCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Cag);
            Units.SetDirtyBrigadeTypes();

            // 文字色を変更する
            cagCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     護衛戦闘機チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEscortCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (escortCheckBox.Checked == unit.Escort)
            {
                return;
            }

            // 値を更新する
            unit.Escort = escortCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Escort);
            Units.SetDirtyBrigadeTypes();

            // 文字色を変更する
            escortCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     工兵チェックボックスの状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngineerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (engineerCheckBox.Checked == unit.Engineer)
            {
                return;
            }

            // 値を更新する
            unit.Engineer = engineerCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Engineer);
            Units.SetDirtyBrigadeTypes();

            // 文字色を変更する
            engineerCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     最大付属旅団数変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxAllowedBrigadesNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 値に変化がなければ何もしない
            if (maxAllowedBrigadesNumericUpDown.Value == unit.MaxAllowedBrigades)
            {
                return;
            }

            // 値を更新する
            unit.MaxAllowedBrigades = (int) maxAllowedBrigadesNumericUpDown.Value;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.MaxAllowedBrigades);
            unit.SetDirty();

            // 文字色を変更する
            maxAllowedBrigadesNumericUpDown.ForeColor = Color.Red;
        }

        /// <summary>
        ///     付属旅団リストビューののチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowedBrigadesListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];
            UnitType type = Units.BrigadeTypes[e.Item.Index];

            if (e.Item.Checked)
            {
                // 値に変化がなければ何もしない
                if (unit.AllowedBrigades.Contains(type))
                {
                    return;
                }

                // 値を更新する
                unit.AllowedBrigades.Add(type);
            }
            else
            {
                // 値に変化がなければ何もしない
                if (!unit.AllowedBrigades.Contains(type))
                {
                    return;
                }

                // 値を更新する
                unit.AllowedBrigades.Remove(type);
            }

            // 編集済みフラグを設定する
            unit.SetDirtyAllowedBrigades(type);
            unit.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットクラスタブ - 改良

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

            // 改良情報が登録されていなければ編集項目を無効化する
            if (unit.Upgrades.Count == 0)
            {
                DisableUpgradeItems();
                return;
            }

            // 先頭の項目を選択する
            upgradeListView.Items[0].Focused = true;
            upgradeListView.Items[0].Selected = true;

            // 編集項目を有効化する
            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良情報の編集項目を有効化する
        /// </summary>
        private void EnableUpgradeItems()
        {
            upgradeTypeComboBox.Enabled = true;
            upgradeCostTextBox.Enabled = true;
            upgradeTimeTextBox.Enabled = true;

            upgradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     改良情報の編集項目を無効化する
        /// </summary>
        private void DisableUpgradeItems()
        {
            upgradeTypeComboBox.Enabled = false;
            upgradeCostTextBox.Enabled = false;
            upgradeTimeTextBox.Enabled = false;

            upgradeTypeComboBox.SelectedIndex = -1;
            upgradeTypeComboBox.ResetText();
            upgradeCostTextBox.ResetText();
            upgradeTimeTextBox.ResetText();

            upgradeRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     改良ユニット種類コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中の項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            UnitUpgrade upgrade = unit.Upgrades[upgradeListView.SelectedIndices[0]];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) upgrade.Type) && upgrade.IsDirty(UnitUpgradeItemId.Type))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = upgradeTypeComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     改良リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択項目がなければ編集を禁止する
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                DisableUpgradeItems();
                return;
            }
            UnitUpgrade upgrade = unit.Upgrades[upgradeListView.SelectedIndices[0]];

            // 編集項目の値を更新する
            upgradeTypeComboBox.SelectedIndex = (int) upgrade.Type;
            upgradeCostTextBox.Text = upgrade.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
            upgradeTimeTextBox.Text = upgrade.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);

            // 編集項目の色を更新する
            upgradeCostTextBox.ForeColor = upgrade.IsDirty(UnitUpgradeItemId.UpgradeCostFactor)
                ? Color.Red
                : SystemColors.WindowText;
            upgradeTimeTextBox.ForeColor = upgrade.IsDirty(UnitUpgradeItemId.UpgradeTimeFactor)
                ? Color.Red
                : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良ユニット種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 値に変化がなければ何もしない
            var type = (UnitType) upgradeTypeComboBox.SelectedIndex;
            if (type == upgrade.Type)
            {
                return;
            }

            // 値を更新する
            upgrade.Type = type;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].Text = Config.GetText(Units.Items[(int) type].Name);

            // 編集済みフラグを設定する
            upgrade.SetDirty(UnitUpgradeItemId.Type);
            upgrade.SetDirty();
            unit.SetDirty();

            // 改良ユニット種類コンボボックスの項目色を変更するために描画更新する
            upgradeTypeComboBox.Refresh();
        }

        /// <summary>
        ///     改良コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
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
            upgrade.SetDirty(UnitUpgradeItemId.UpgradeCostFactor);
            upgrade.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            upgradeCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     改良時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
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
            upgrade.SetDirty(UnitUpgradeItemId.UpgradeTimeFactor);
            upgrade.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            upgradeTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     改良情報の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeAddButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 改良情報を追加する
            var upgrade = new UnitUpgrade();
            unit.Upgrades.Add(upgrade);

            // 編集済みフラグを設定する
            upgrade.SetDirtyAll();
            unit.SetDirty();

            // 改良リストビューに項目を追加する
            AddUpgradeListItem(upgrade);
        }

        /// <summary>
        ///     改良情報の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];

            // 改良情報を削除する
            unit.Upgrades.RemoveAt(index);

            // 編集済みフラグを設定する
            unit.SetDirty();

            // 改良リストビューから項目を削除する
            RemoveUpgradeListItem(index);
        }

        /// <summary>
        ///     改良リストの項目を作成する
        /// </summary>
        /// <param name="upgrade">改良設定</param>
        /// <returns>改良リストの項目</returns>
        private static ListViewItem CreateUpgradeListItem(UnitUpgrade upgrade)
        {
            var item = new ListViewItem {Text = Config.GetText(Units.Items[(int) upgrade.Type].Name)};
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

        /// <summary>
        ///     ユニットモデルタブの編集項目の値を更新する
        /// </summary>
        private void UpdateModelEditableItems()
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            Country country = (countryListView.SelectedIndices.Count == 0
                ? Country.None
                : (Country) (countryListView.SelectedIndices[0] + 1));

            // モデル画像
            modelImagePictureBox.ImageLocation = GetModelImageFileName(unit, index, country);
            // モデルアイコン
            modelIconPictureBox.ImageLocation = GetModelIconFileName(unit, index);
            // モデル名
            modelNameTextBox.Text = Config.GetText(UnitModel.GetName(unit, index, country));
            modelNameTextBox.ForeColor = model.IsDirtyName(country) ? Color.Red : SystemColors.WindowText;

            // 組織率
            defaultOrganisationTextBox.Text = model.DefaultOrganization.ToString(CultureInfo.InvariantCulture);
            defaultOrganisationTextBox.ForeColor = model.IsDirty(UnitModelItemId.DefaultOrganization)
                ? Color.Red
                : SystemColors.WindowText;
            // 士気
            moraleTextBox.Text = model.Morale.ToString(CultureInfo.InvariantCulture);
            moraleTextBox.ForeColor = model.IsDirty(UnitModelItemId.Morale) ? Color.Red : SystemColors.WindowText;
            // 消費物資
            supplyConsumptionTextBox.Text = model.SupplyConsumption.ToString(CultureInfo.InvariantCulture);
            supplyConsumptionTextBox.ForeColor = model.IsDirty(UnitModelItemId.SupplyConsumption)
                ? Color.Red
                : SystemColors.WindowText;
            // 消費燃料
            fuelConsumptionTextBox.Text = model.FuelConsumption.ToString(CultureInfo.InvariantCulture);
            fuelConsumptionTextBox.ForeColor = model.IsDirty(UnitModelItemId.FuelConsumption)
                ? Color.Red
                : SystemColors.WindowText;
            // 必要IC
            costTextBox.Text = model.Cost.ToString(CultureInfo.InvariantCulture);
            costTextBox.ForeColor = model.IsDirty(UnitModelItemId.Cost) ? Color.Red : SystemColors.WindowText;
            // 必要時間
            buildTimeTextBox.Text = model.BuildTime.ToString(CultureInfo.InvariantCulture);
            buildTimeTextBox.ForeColor = model.IsDirty(UnitModelItemId.BuildTime) ? Color.Red : SystemColors.WindowText;
            // 労働力
            manPowerTextBox.Text = model.ManPower.ToString(CultureInfo.InvariantCulture);
            manPowerTextBox.ForeColor = model.IsDirty(UnitModelItemId.ManPower) ? Color.Red : SystemColors.WindowText;
            // 最大速度
            maxSpeedTextBox.Text = model.MaxSpeed.ToString(CultureInfo.InvariantCulture);
            maxSpeedTextBox.ForeColor = model.IsDirty(UnitModelItemId.MaxSpeed) ? Color.Red : SystemColors.WindowText;
            // 対空防御力
            airDefenceTextBox.Text = model.AirDefense.ToString(CultureInfo.InvariantCulture);
            airDefenceTextBox.ForeColor = model.IsDirty(UnitModelItemId.AirDefense)
                ? Color.Red
                : SystemColors.WindowText;
            // 対空攻撃力
            airAttackTextBox.Text = model.AirAttack.ToString(CultureInfo.InvariantCulture);
            airAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.AirAttack) ? Color.Red : SystemColors.WindowText;

            // 陸軍
            if (unit.Branch == Branch.Army)
            {
                // 航続距離
                rangeLabel.Enabled = false;
                rangeTextBox.Enabled = false;
                rangeTextBox.ResetText();
                // 輸送負荷
                transportWeightLabel.Enabled = true;
                transportWeightTextBox.Enabled = true;
                transportWeightTextBox.Text = model.TransportWeight.ToString(CultureInfo.InvariantCulture);
                transportWeightTextBox.ForeColor = model.IsDirty(UnitModelItemId.TransportWeight)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 輸送能力
                transportCapabilityLabel.Enabled = false;
                transportCapabilityTextBox.Enabled = false;
                transportCapabilityTextBox.ResetText();
                // 制圧力
                suppressionLabel.Enabled = true;
                suppressionTextBox.Enabled = true;
                suppressionTextBox.Text = model.Suppression.ToString(CultureInfo.InvariantCulture);
                suppressionTextBox.ForeColor = model.IsDirty(UnitModelItemId.Suppression)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 防御力
                defensivenessLabel.Enabled = true;
                defensivenessTextBox.Enabled = true;
                defensivenessTextBox.Text = model.Defensiveness.ToString(CultureInfo.InvariantCulture);
                defensivenessTextBox.ForeColor = model.IsDirty(UnitModelItemId.Defensiveness)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 耐久力
                toughnessLabel.Enabled = true;
                toughnessTextBox.Enabled = true;
                toughnessTextBox.Text = model.Toughness.ToString(CultureInfo.InvariantCulture);
                toughnessTextBox.ForeColor = model.IsDirty(UnitModelItemId.Toughness)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 脆弱性
                softnessLabel.Enabled = true;
                softnessTextBox.Enabled = true;
                softnessTextBox.Text = model.Softness.ToString(CultureInfo.InvariantCulture);
                softnessTextBox.ForeColor = model.IsDirty(UnitModelItemId.Softness)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対地索敵力
                surfaceDetectionCapabilityLabel.Enabled = false;
                surfaceDetectionCapabilityTextBox.Enabled = false;
                surfaceDetectionCapabilityTextBox.ResetText();
                // 対空索敵力
                airDetectionCapabilityLabel.Enabled = false;
                airDetectionCapabilityTextBox.Enabled = false;
                airDetectionCapabilityTextBox.ResetText();
            }
            else
            {
                // 航続距離
                rangeLabel.Enabled = true;
                rangeTextBox.Enabled = true;
                rangeTextBox.Text = model.Range.ToString(CultureInfo.InvariantCulture);
                rangeTextBox.ForeColor = model.IsDirty(UnitModelItemId.Range) ? Color.Red : SystemColors.WindowText;
                // 輸送負荷
                transportWeightLabel.Enabled = false;
                transportWeightTextBox.Enabled = false;
                transportWeightTextBox.ResetText();
                // 輸送能力
                transportCapabilityLabel.Enabled = true;
                transportCapabilityTextBox.Enabled = true;
                transportCapabilityTextBox.Text = model.TransportCapability.ToString(CultureInfo.InvariantCulture);
                transportCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.TransportCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 制圧力
                suppressionLabel.Enabled = false;
                suppressionTextBox.Enabled = false;
                suppressionTextBox.ResetText();
                // 防御力
                defensivenessLabel.Enabled = false;
                defensivenessTextBox.Enabled = false;
                defensivenessTextBox.ResetText();
                // 耐久力
                toughnessLabel.Enabled = false;
                toughnessTextBox.Enabled = false;
                toughnessTextBox.ResetText();
                // 脆弱性
                softnessLabel.Enabled = false;
                softnessTextBox.Enabled = false;
                softnessTextBox.ResetText();
                // 対地索敵力
                surfaceDetectionCapabilityLabel.Enabled = true;
                surfaceDetectionCapabilityTextBox.Enabled = true;
                surfaceDetectionCapabilityTextBox.Text =
                    model.SurfaceDetectionCapability.ToString(CultureInfo.InvariantCulture);
                surfaceDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.SurfaceDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対空索敵力
                airDetectionCapabilityLabel.Enabled = true;
                airDetectionCapabilityTextBox.Enabled = true;
                airDetectionCapabilityTextBox.Text = model.AirDetectionCapability.ToString(CultureInfo.InvariantCulture);
                airDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.AirDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
            }

            // 陸軍師団
            if (unit.Branch == Branch.Army && unit.Organization == UnitOrganization.Division)
            {
                // 速度キャップ(砲兵)
                speedCapArtLabel.Enabled = true;
                speedCapArtTextBox.Enabled = true;
                speedCapArtTextBox.Text = model.SpeedCapArt.ToString(CultureInfo.InvariantCulture);
                speedCapArtTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapArt)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(工兵)
                speedCapEngLabel.Enabled = true;
                speedCapEngTextBox.Enabled = true;
                speedCapEngTextBox.Text = model.SpeedCapEng.ToString(CultureInfo.InvariantCulture);
                speedCapEngTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapEng)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(対戦車)
                speedCapAtLabel.Enabled = true;
                speedCapAtTextBox.Enabled = true;
                speedCapAtTextBox.Text = model.SpeedCapAt.ToString(CultureInfo.InvariantCulture);
                speedCapAtTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapAt)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(対空)
                speedCapAaLabel.Enabled = true;
                speedCapAaTextBox.Enabled = true;
                speedCapAaTextBox.Text = model.SpeedCapAa.ToString(CultureInfo.InvariantCulture);
                speedCapAaTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapAa)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 速度キャップ(砲兵)
                speedCapArtLabel.Enabled = false;
                speedCapArtTextBox.Enabled = false;
                speedCapArtTextBox.ResetText();
                // 速度キャップ(工兵)
                speedCapEngLabel.Enabled = false;
                speedCapEngTextBox.Enabled = false;
                speedCapEngTextBox.ResetText();
                // 速度キャップ(対戦車)
                speedCapAtTextBox.Enabled = false;
                speedCapAaLabel.Enabled = false;
                speedCapAtTextBox.ResetText();
                // 速度キャップ(対空)
                speedCapAtLabel.Enabled = false;
                speedCapAaTextBox.Enabled = false;
                speedCapAaTextBox.ResetText();
            }

            // 海軍
            if (unit.Branch == Branch.Navy)
            {
                // 改良コスト
                upgradeCostFactorLabel.Enabled = false;
                upgradeCostFactorTextBox.Enabled = false;
                upgradeCostFactorTextBox.ResetText();
                // 改良時間
                upgradeTimeFactorLabel.Enabled = false;
                upgradeTimeFactorTextBox.Enabled = false;
                upgradeTimeFactorTextBox.ResetText();
                // 対艦防御力
                seaDefenceLabel.Enabled = true;
                seaDefenceTextBox.Enabled = true;
                seaDefenceTextBox.Text = model.SeaDefense.ToString(CultureInfo.InvariantCulture);
                seaDefenceTextBox.ForeColor = model.IsDirty(UnitModelItemId.SeaDefense)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対人攻撃力
                softAttackLabel.Enabled = false;
                softAttackTextBox.Enabled = false;
                softAttackTextBox.ResetText();
                // 対甲攻撃力
                hardAttackLabel.Enabled = false;
                hardAttackTextBox.Enabled = false;
                hardAttackTextBox.ResetText();
                // 艦対艦攻撃力
                seaAttackLabel.Enabled = true;
                seaAttackTextBox.Enabled = true;
                seaAttackTextBox.Text = model.SeaAttack.ToString(CultureInfo.InvariantCulture);
                seaAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SeaAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対潜攻撃力
                subAttackLabel.Enabled = true;
                subAttackTextBox.Enabled = true;
                subAttackTextBox.Text = model.SubAttack.ToString(CultureInfo.InvariantCulture);
                subAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SubAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 船団攻撃力
                convoyAttackLabel.Enabled = true;
                convoyAttackTextBox.Enabled = true;
                convoyAttackTextBox.Text = model.ConvoyAttack.ToString(CultureInfo.InvariantCulture);
                convoyAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.ConvoyAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 沿岸砲撃能力
                shoreBombardmentLabel.Enabled = true;
                shoreBombardmentTextBox.Enabled = true;
                shoreBombardmentTextBox.Text = model.ShoreBombardment.ToString(CultureInfo.InvariantCulture);
                shoreBombardmentTextBox.ForeColor = model.IsDirty(UnitModelItemId.ShoreBombardment)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 射程
                distanceLabel.Enabled = true;
                distanceTextBox.Enabled = true;
                distanceTextBox.Text = model.Distance.ToString(CultureInfo.InvariantCulture);
                distanceTextBox.ForeColor = model.IsDirty(UnitModelItemId.Distance)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 視認性
                visibilityLabel.Enabled = true;
                visibilityTextBox.Enabled = true;
                visibilityTextBox.Text = model.Visibility.ToString(CultureInfo.InvariantCulture);
                visibilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.Visibility)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対艦索敵力
                subDetectionCapabilityLabel.Enabled = true;
                subDetectionCapabilityTextBox.Enabled = true;
                subDetectionCapabilityTextBox.Text = model.SubDetectionCapability.ToString(CultureInfo.InvariantCulture);
                subDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.SubDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 改良コスト
                upgradeCostFactorLabel.Enabled = true;
                upgradeCostFactorTextBox.Enabled = true;
                upgradeCostFactorTextBox.Text = model.UpgradeCostFactor.ToString(CultureInfo.InvariantCulture);
                upgradeCostFactorTextBox.ForeColor = model.IsDirty(UnitModelItemId.UpgradeCostFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 改良時間
                upgradeTimeFactorLabel.Enabled = true;
                upgradeTimeFactorTextBox.Enabled = true;
                upgradeTimeFactorTextBox.Text = model.UpgradeTimeFactor.ToString(CultureInfo.InvariantCulture);
                upgradeTimeFactorTextBox.ForeColor = model.IsDirty(UnitModelItemId.UpgradeTimeFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対艦防御力
                seaDefenceLabel.Enabled = false;
                seaDefenceTextBox.Enabled = false;
                seaDefenceTextBox.ResetText();
                // 対人攻撃力
                softAttackLabel.Enabled = true;
                softAttackTextBox.Enabled = true;
                softAttackTextBox.Text = model.SoftAttack.ToString(CultureInfo.InvariantCulture);
                softAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SoftAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対甲攻撃力
                hardAttackLabel.Enabled = true;
                hardAttackTextBox.Enabled = true;
                hardAttackTextBox.Text = model.HardAttack.ToString(CultureInfo.InvariantCulture);
                hardAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.HardAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 艦対艦攻撃力
                seaAttackLabel.Enabled = false;
                seaAttackTextBox.Enabled = false;
                seaAttackTextBox.ResetText();
                // 対潜攻撃力
                subAttackLabel.Enabled = false;
                subAttackTextBox.Enabled = false;
                subAttackTextBox.ResetText();
                // 船団攻撃力
                convoyAttackLabel.Enabled = false;
                convoyAttackTextBox.Enabled = false;
                convoyAttackTextBox.ResetText();
                // 沿岸砲撃能力
                shoreBombardmentLabel.Enabled = false;
                shoreBombardmentTextBox.Enabled = false;
                shoreBombardmentTextBox.ResetText();
                // 射程
                distanceLabel.Enabled = false;
                distanceTextBox.Enabled = false;
                distanceTextBox.ResetText();
                // 視認性
                visibilityLabel.Enabled = false;
                visibilityTextBox.Enabled = false;
                visibilityTextBox.ResetText();
                // 対艦索敵力
                subDetectionCapabilityLabel.Enabled = false;
                subDetectionCapabilityTextBox.Enabled = false;
                subDetectionCapabilityTextBox.ResetText();
            }

            // 空軍
            if (unit.Branch == Branch.Airforce)
            {
                // 対地防御力
                surfaceDefenceLabel.Enabled = true;
                surfaceDefenceTextBox.Enabled = true;
                surfaceDefenceTextBox.Text = model.SurfaceDefense.ToString(CultureInfo.InvariantCulture);
                surfaceDefenceTextBox.ForeColor = model.IsDirty(UnitModelItemId.SurfaceDefense)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 空対艦攻撃力
                navalAttackLabel.Enabled = true;
                navalAttackTextBox.Enabled = true;
                navalAttackTextBox.Text = model.NavalAttack.ToString(CultureInfo.InvariantCulture);
                navalAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.NavalAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 戦略爆撃攻撃力
                strategicAttackLabel.Enabled = true;
                strategicAttackTextBox.Enabled = true;
                strategicAttackTextBox.Text = model.StrategicAttack.ToString(CultureInfo.InvariantCulture);
                strategicAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.StrategicAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 対地防御力
                surfaceDefenceLabel.Enabled = false;
                surfaceDefenceTextBox.Enabled = false;
                surfaceDefenceTextBox.ResetText();
                // 空対艦攻撃力
                navalAttackLabel.Enabled = false;
                navalAttackTextBox.Enabled = false;
                navalAttackTextBox.ResetText();
                // 戦略爆撃攻撃力
                strategicAttackLabel.Enabled = false;
                strategicAttackTextBox.Enabled = false;
                strategicAttackTextBox.ResetText();
            }

            // AoD/陸軍
            if (Game.Type == GameType.ArsenalOfDemocracy && unit.Branch == Branch.Army)
            {
                // 最大物資
                maxSupplyStockLabel.Enabled = true;
                maxSupplyStockTextBox.Enabled = true;
                maxSupplyStockTextBox.Text = model.MaxSupplyStock.ToString(CultureInfo.InvariantCulture);
                maxSupplyStockTextBox.ForeColor = model.IsDirty(UnitModelItemId.MaxSupplyStock)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 最大燃料
                maxOilStockLabel.Enabled = true;
                maxOilStockTextBox.Enabled = true;
                maxOilStockTextBox.Text = model.MaxOilStock.ToString(CultureInfo.InvariantCulture);
                maxOilStockTextBox.ForeColor = model.IsDirty(UnitModelItemId.MaxOilStock)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 最大物資
                maxSupplyStockLabel.Enabled = false;
                maxSupplyStockTextBox.Enabled = false;
                maxSupplyStockTextBox.ResetText();
                // 最大燃料
                maxOilStockLabel.Enabled = false;
                maxOilStockTextBox.Enabled = false;
                maxOilStockTextBox.ResetText();
            }

            // AoD/陸軍旅団
            if (Game.Type == GameType.ArsenalOfDemocracy &&
                unit.Branch == Branch.Army &&
                unit.Organization == UnitOrganization.Brigade)
            {
                // 砲撃能力
                artilleryBombardmentLabel.Enabled = true;
                artilleryBombardmentTextBox.Enabled = true;
                artilleryBombardmentTextBox.Text = model.ArtilleryBombardment.ToString(CultureInfo.InvariantCulture);
                artilleryBombardmentTextBox.ForeColor = model.IsDirty(UnitModelItemId.ArtilleryBombardment)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 砲撃能力
                artilleryBombardmentLabel.Enabled = false;
                artilleryBombardmentTextBox.Enabled = false;
                artilleryBombardmentTextBox.ResetText();
            }

            // DH/師団
            if (Game.Type == GameType.DarkestHour && unit.Organization == UnitOrganization.Division)
            {
                // 補充コスト
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceCostTextBox.Text = model.ReinforceCostFactor.ToString(CultureInfo.InvariantCulture);
                reinforceCostTextBox.ForeColor = model.IsDirty(UnitModelItemId.ReinforceCostFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 補充時間
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                reinforceTimeTextBox.Text = model.ReinforceTimeFactor.ToString(CultureInfo.InvariantCulture);
                reinforceTimeTextBox.ForeColor = model.IsDirty(UnitModelItemId.ReinforceTimeFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 補充コスト
                reinforceCostLabel.Enabled = false;
                reinforceCostTextBox.Enabled = false;
                reinforceCostTextBox.ResetText();
                // 補充時間
                reinforceTimeLabel.Enabled = false;
                reinforceTimeTextBox.Enabled = false;
                reinforceTimeTextBox.ResetText();
            }

            // DH/陸軍師団
            if (Game.Type == GameType.DarkestHour &&
                unit.Branch == Branch.Army &&
                unit.Organization == UnitOrganization.Division)
            {
                // 燃料切れ補正
                noFuelCombatModLabel.Enabled = true;
                noFuelCombatModTextBox.Enabled = true;
                noFuelCombatModTextBox.Text = model.NoFuelCombatMod.ToString(CultureInfo.InvariantCulture);
                noFuelCombatModTextBox.ForeColor = model.IsDirty(UnitModelItemId.NoFuelCombatMod)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 燃料切れ補正
                noFuelCombatModLabel.Enabled = false;
                noFuelCombatModTextBox.Enabled = false;
                noFuelCombatModTextBox.ResetText();
            }

            // DH1.03以降
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                // 装備リストを更新する
                UpdateEquipmentList(model);
            }

            // DH1.03以降/陸軍旅団
            if (Game.Type == GameType.DarkestHour &&
                Game.Version >= 103 &&
                unit.Branch == Branch.Army &&
                unit.Organization == UnitOrganization.Brigade)
            {
                // 速度キャップ
                speedCapAllLabel.Enabled = true;
                speedCapAllTextBox.Enabled = true;
                speedCapAllTextBox.Text = model.SpeedCap.ToString(CultureInfo.InvariantCulture);
                speedCapAllTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCap)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 速度キャップ
                speedCapAllLabel.Enabled = false;
                speedCapAllTextBox.Enabled = false;
                speedCapAllTextBox.ResetText();
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

            cloneButton.Enabled = true;
            removeButton.Enabled = true;

            // DH1.03以降
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                equipmentGroupBox.Enabled = true;
            }
            else
            {
                equipmentGroupBox.Enabled = false;
            }
        }

        /// <summary>
        ///     ユニットモデルタブの編集項目を無効化する
        /// </summary>
        private void DisableModelEditableItems()
        {
            modelNameTextBox.Enabled = false;
            basicGroupBox.Enabled = false;
            productionGroupBox.Enabled = false;
            speedGroupBox.Enabled = false;
            battleGroupBox.Enabled = false;
            equipmentGroupBox.Enabled = false;

            modelImagePictureBox.ImageLocation = "";
            modelIconPictureBox.ImageLocation = "";
            modelNameTextBox.ResetText();

            defaultOrganisationTextBox.ResetText();
            moraleTextBox.ResetText();
            rangeTextBox.ResetText();
            transportWeightTextBox.ResetText();
            transportCapabilityTextBox.ResetText();
            suppressionTextBox.ResetText();
            supplyConsumptionTextBox.ResetText();
            fuelConsumptionTextBox.ResetText();
            maxSupplyStockTextBox.ResetText();
            maxOilStockTextBox.ResetText();

            costTextBox.ResetText();
            buildTimeTextBox.ResetText();
            manPowerTextBox.ResetText();
            upgradeCostFactorTextBox.ResetText();
            upgradeTimeFactorTextBox.ResetText();
            reinforceCostTextBox.ResetText();
            reinforceTimeTextBox.ResetText();

            maxSpeedTextBox.ResetText();
            speedCapAllTextBox.ResetText();
            speedCapArtTextBox.ResetText();
            speedCapEngTextBox.ResetText();
            speedCapAtTextBox.ResetText();
            speedCapAaTextBox.ResetText();

            defensivenessTextBox.ResetText();
            seaDefenceTextBox.ResetText();
            airDefenceTextBox.ResetText();
            surfaceDefenceTextBox.ResetText();
            toughnessTextBox.ResetText();
            softnessTextBox.ResetText();
            softAttackTextBox.ResetText();
            hardAttackTextBox.ResetText();
            seaAttackTextBox.ResetText();
            subAttackTextBox.ResetText();
            convoyAttackTextBox.ResetText();
            shoreBombardmentTextBox.ResetText();
            airAttackTextBox.ResetText();
            navalAttackTextBox.ResetText();
            strategicAttackTextBox.ResetText();
            artilleryBombardmentTextBox.ResetText();
            distanceTextBox.ResetText();
            visibilityTextBox.ResetText();
            surfaceDetectionCapabilityTextBox.ResetText();
            subDetectionCapabilityTextBox.ResetText();
            airDetectionCapabilityTextBox.ResetText();
            noFuelCombatModTextBox.ResetText();

            equipmentListView.Items.Clear();
            resourceComboBox.ResetText();
            quantityTextBox.ResetText();

            cloneButton.Enabled = false;
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     ユニットモデル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            Country country = countryListView.SelectedIndices.Count == 0
                ? Country.None
                : (Country) (countryListView.SelectedIndices[0] + 1);
            string name = UnitModel.GetName(unit, index, country);
            if (modelNameTextBox.Text.Equals(Config.GetText(name)))
            {
                return;
            }

            // 値を更新する
            UnitModel.SetName(unit, index, country, modelNameTextBox.Text);

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[1].Text = modelNameTextBox.Text;

            // 編集済みフラグを設定する
            model.SetDirtyName(country);

            // 文字色を変更する
            modelNameTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     ユニットモデル画像のファイル名を取得する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル画像のファイル名</returns>
        private static string GetModelImageFileName(Unit unit, int index, Country country)
        {
            string name;
            string fileName;
            if (country != Country.None)
            {
                // 国タグ指定/モデル番号指定
                name = string.Format(
                    unit.Organization == UnitOrganization.Division
                        ? "ill_div_{0}_{1}_{2}.bmp"
                        : "ill_bri_{0}_{1}_{2}.bmp",
                    Countries.Strings[(int) country],
                    Units.UnitNumbers[(int) unit.Type],
                    index);
                fileName = Game.GetReadFileName(Game.ModelPicturePathName, name);
                if (File.Exists(fileName))
                {
                    return fileName;
                }

                // 国タグ指定/モデル番号は0指定
                name = string.Format(
                    unit.Organization == UnitOrganization.Division
                        ? "ill_div_{0}_{1}_0.bmp"
                        : "ill_bri_{0}_{1}_0.bmp",
                    Countries.Strings[(int) country],
                    Units.UnitNumbers[(int) unit.Type]);
                fileName = Game.GetReadFileName(Game.ModelPicturePathName, name);
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
                index);
            fileName = Game.GetReadFileName(Game.ModelPicturePathName, name);
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
            fileName = Game.GetReadFileName(Game.ModelPicturePathName, name);
            return File.Exists(fileName) ? fileName : string.Empty;
        }

        /// <summary>
        ///     ユニットモデルアイコンのファイル名を取得する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデルアイコンのファイル名</returns>
        private static string GetModelIconFileName(Unit unit, int index)
        {
            // 旅団にはアイコンが存在しないので空文字列を返す
            if (unit.Organization == UnitOrganization.Brigade)
            {
                return string.Empty;
            }

            string name = string.Format("model_{0}_{1}.bmp", Units.UnitNumbers[(int) unit.Type], index);
            string fileName = Game.GetReadFileName(Game.ModelPicturePathName, name);
            return File.Exists(fileName) ? fileName : string.Empty;
        }

        #endregion

        #region ユニットモデルタブ - 基本ステータス

        /// <summary>
        ///     組織率テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultOrganizationTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[7].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.DefaultOrganization);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            defaultOrganisationTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     士気テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoraleTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[8].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Morale);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            moraleTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     航続距離テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRangeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Range);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            rangeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     輸送負荷テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransportWeightTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.TransportWeight);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            transportWeightTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     輸送能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTransportCapabilityTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.DefaultOrganization);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            transportCapabilityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     制圧力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuppressionTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Suppression);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            suppressionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     消費物資テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSupplyConsumptionTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[5].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SupplyConsumption);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            supplyConsumptionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     消費燃料テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuelConsumptionTextBox(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[6].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.FuelConsumption);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            fuelConsumptionTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     最大物資テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSupplyStockTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.MaxSupplyStock);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            maxSupplyStockTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     最大燃料テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxOilStockTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.MaxOilStock);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            maxOilStockTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットモデルタブ - 生産ステータス

        /// <summary>
        ///     必要ICテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[2].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Cost);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            costTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     必要時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(buildTimeTextBox.Text, out val))
            {
                buildTimeTextBox.Text = model.BuildTime.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.BuildTime) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.BuildTime = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[3].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.BuildTime);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            buildTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     労働力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnManPowerTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[4].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ManPower);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            manPowerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     改良コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostFactorTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.UpgradeCostFactor);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            upgradeCostFactorTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     改良時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeFactorTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.UpgradeTimeFactor);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            upgradeTimeFactorTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     補充コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.ReinforceCostFactor);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            reinforceCostTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     補充時間テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReinforceTimeTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ReinforceTimeFactor);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            reinforceTimeTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットモデルタブ - 速度ステータス

        /// <summary>
        ///     最大速度テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSpeedTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[9].Text = val.ToString(CultureInfo.InvariantCulture);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.MaxSpeed);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            maxSpeedTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SpeedCap);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            speedCapAllTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砲兵旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapArtTextBox(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SpeedCapArt);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            speedCapArtTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     工兵旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapEngTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SpeedCapEng);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            speedCapEngTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対戦車旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapAtTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SpeedCapAt);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            speedCapAtTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空旅団速度キャップテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSpeedCapAaTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SpeedCapAa);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            speedCapAaTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットモデルタブ - 戦闘ステータス

        /// <summary>
        ///     防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefensivenessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Defensiveness);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            defensivenessTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対艦防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SeaDefense);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            seaDefenceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.AirDefense);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            airDefenceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対地防御力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSurfaceDefenceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SurfaceDefense);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            surfaceDefenceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     耐久力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToughnessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Toughness);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            toughnessTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     脆弱性テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSoftnessTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Softness);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            softnessTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対人攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSoftAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SoftAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            softAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対甲攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHardAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.HardAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            hardAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対艦攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SeaAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            seaAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対潜攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SubAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            subAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     船団攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvoyAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!double.TryParse(convoyAttackTextBox.Text, out val))
            {
                convoyAttackTextBox.Text = model.ConvoyAttack.ToString(CultureInfo.InvariantCulture);
                return;
            }

            // 値に変化がなければ何もしない
            if (Math.Abs(val - model.ConvoyAttack) <= 0.00005)
            {
                return;
            }

            // 値を更新する
            model.ConvoyAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ConvoyAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            convoyAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     沿岸砲撃能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShoreBombardmentTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.ShoreBombardment);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            shoreBombardmentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.AirAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            airAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     空対艦攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavalAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.NavalAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            navalAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     戦略爆撃攻撃力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStrategicAttackTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.StrategicAttack);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            strategicAttackTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     砲撃能力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArtilleryBombardmentTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.ArtilleryBombardment);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            artilleryBombardmentTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     射程テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDistanceTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Distance);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            distanceTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     視認性テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVisibilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.Visibility);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            visibilityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対地索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSurfaceDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SurfaceDetectionCapability);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            surfaceDetectionCapabilityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対潜索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.SubDetectionCapability);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            subDetectionCapabilityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     対空索敵力テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAirDetectionCapabilityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.AirDetectionCapability);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            airDetectionCapabilityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     燃料切れ補正テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNoFuelCombatModTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

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
            model.SetDirty(UnitModelItemId.NoFuelCombatMod);
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            noFuelCombatModTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットモデルタブ - 装備

        /// <summary>
        ///     装備リストビューの項目を更新する
        /// </summary>
        /// <param name="model"></param>
        private void UpdateEquipmentList(UnitModel model)
        {
            // 項目を順に登録する
            equipmentListView.BeginUpdate();
            equipmentListView.Items.Clear();
            foreach (UnitEquipment equipment in model.Equipments)
            {
                equipmentListView.Items.Add(CreateEquipmentListItem(equipment));
            }
            equipmentListView.EndUpdate();

            // 項目がなければ編集項目を無効化する
            if (model.Equipments.Count == 0)
            {
                DisableEquipmentItems();
                return;
            }

            // 先頭の項目を選択する
            equipmentListView.Items[0].Focused = true;
            equipmentListView.Items[0].Selected = true;

            // 編集項目を有効化する
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

            resourceComboBox.SelectedIndex = -1;
            resourceComboBox.ResetText();
            quantityTextBox.ResetText();

            equipmentRemoveButton.Enabled = false;
            equipmentUpButton.Enabled = false;
            equipmentDownButton.Enabled = false;
        }

        /// <summary>
        ///     資源コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];
            UnitEquipment equipment = model.Equipments[index];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == (int) equipment.Resource) && equipment.IsDirty(UnitEquipmentItemId.Resource))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = resourceComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     装備リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];
            UnitEquipment equipment = model.Equipments[index];

            // 編集項目の値を更新する
            resourceComboBox.SelectedIndex = (int) equipment.Resource;
            quantityTextBox.Text = equipment.Quantity.ToString(CultureInfo.InvariantCulture);

            // 編集項目の色を更新する
            quantityTextBox.ForeColor = equipment.IsDirty(UnitEquipmentItemId.Quantity)
                ? Color.Red
                : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableEquipmentItems();
        }

        /// <summary>
        ///     資源コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];
            UnitEquipment equipment = model.Equipments[index];

            // 値に変化がなければ何もしない
            var type = (EquipmentType) resourceComboBox.SelectedIndex;
            if (type == equipment.Resource)
            {
                return;
            }

            // 値を更新する
            equipment.Resource = type;

            // 装備リストビューの項目を更新する
            equipmentListView.Items[index].Text = Config.GetText(Units.EquipmentNames[(int) type]);

            // 編集済みフラグを設定する
            equipment.SetDirty(UnitEquipmentItemId.Resource);
            equipment.SetDirty();
            model.SetDirty();
            unit.SetDirty();

            // 資源コンボボックスの項目色を変更するために描画更新する
            resourceComboBox.Refresh();
        }

        /// <summary>
        ///     量テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuantityTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];
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
            equipment.SetDirty(UnitEquipmentItemId.Quantity);
            equipment.SetDirty();
            model.SetDirty();
            unit.SetDirty();

            // 文字色を変更する
            quantityTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     装備の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentAddButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 装備リストに項目を追加する
            var equipment = new UnitEquipment();
            model.Equipments.Add(equipment);

            // 編集済みフラグを設定する
            equipment.SetDirtyAll();
            model.SetDirty();
            unit.SetDirty();

            // 装備リストビューに項目を追加する
            AddEquipmentListItem(equipment);
        }

        /// <summary>
        ///     装備の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];

            // 装備リストから項目を削除する
            model.Equipments.RemoveAt(index);

            // 編集済みフラグを設定する
            model.SetDirty();
            unit.SetDirty();

            // 装備リストビューから項目を削除する
            RemoveEquipmentListItem(index);
        }

        /// <summary>
        ///     装備の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentUpButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];

            // リストの先頭ならば何もしない
            if (index == 0)
            {
                return;
            }

            // 装備リストの項目を移動する
            model.MoveEquipment(index, index - 1);

            // 編集済みフラグを設定する
            model.SetDirty();
            unit.SetDirty();

            // 装備リストビューの項目を移動する
            MoveEquipmentListItem(index, index - 1);
        }

        /// <summary>
        ///     装備の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentDownButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            if (classListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[classListBox.SelectedIndex]];

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 選択項目がなければ何もしない
            if (equipmentListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = equipmentListView.SelectedIndices[0];

            // リストの末尾ならば何もしない
            if (index == equipmentListView.Items.Count - 1)
            {
                return;
            }

            // 装備リストの項目を移動する
            model.MoveEquipment(index, index + 1);

            // 編集済みフラグを設定する
            model.SetDirty();
            unit.SetDirty();

            // 装備リストビューの項目を移動する
            MoveEquipmentListItem(index, index + 1);
        }

        /// <summary>
        ///     装備リストの項目を作成する
        /// </summary>
        /// <param name="equipment">装備</param>
        /// <returns>装備リストの項目</returns>
        private static ListViewItem CreateEquipmentListItem(UnitEquipment equipment)
        {
            var item = new ListViewItem {Text = Config.GetText(Units.EquipmentNames[(int) equipment.Resource])};
            item.SubItems.Add(equipment.Quantity.ToString(CultureInfo.InvariantCulture));

            return item;
        }

        /// <summary>
        ///     装備リストビューの項目を追加する
        /// </summary>
        /// <param name="equipment">追加対象の装備</param>
        private void AddEquipmentListItem(UnitEquipment equipment)
        {
            // 装備リストビューに項目を追加する
            ListViewItem item = CreateEquipmentListItem(equipment);
            equipmentListView.Items.Add(item);

            // 追加した項目を選択する
            int index = equipmentListView.Items.Count - 1;
            equipmentListView.Items[index].Focused = true;
            equipmentListView.Items[index].Selected = true;
            equipmentListView.EnsureVisible(index);

            // 編集項目を有効化する
            EnableEquipmentItems();
        }

        /// <summary>
        ///     装備リストビューから項目を削除する
        /// </summary>
        /// <param name="index">削除する項目の位置</param>
        private void RemoveEquipmentListItem(int index)
        {
            // 装備リストビューから項目を削除する
            equipmentListView.Items.RemoveAt(index);

            if (index < equipmentListView.Items.Count)
            {
                // 削除した項目の次を選択する
                equipmentListView.Items[index].Focused = true;
                equipmentListView.Items[index].Selected = true;
            }
            else if (index > 0)
            {
                // 末尾の項目を選択する
                equipmentListView.Items[equipmentListView.Items.Count - 1].Focused = true;
                equipmentListView.Items[equipmentListView.Items.Count - 1].Selected = true;
            }
            else
            {
                // 項目がなくなれば編集項目を無効化する
                DisableEquipmentItems();
            }
        }

        /// <summary>
        ///     装備リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        private void MoveEquipmentListItem(int src, int dest)
        {
            var item = equipmentListView.Items[src].Clone() as ListViewItem;
            if (item == null)
            {
                return;
            }

            if (src > dest)
            {
                // 上へ移動する場合
                equipmentListView.Items.Insert(dest, item);
                equipmentListView.Items.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                equipmentListView.Items.Insert(dest + 1, item);
                equipmentListView.Items.RemoveAt(src);
            }

            // 移動先の項目を選択する
            equipmentListView.Items[dest].Focused = true;
            equipmentListView.Items[dest].Selected = true;
            equipmentListView.EnsureVisible(dest);
        }

        #endregion
    }

    /// <summary>
    ///     ユニットエディタのタブ番号
    /// </summary>
    public enum UnitEditorTab
    {
        Class, // ユニットクラス
        Model, // ユニットモデル
    }
}