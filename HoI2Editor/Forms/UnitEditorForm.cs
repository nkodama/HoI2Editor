using System;
using System.Drawing;
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
        #region 公開定数

        /// <summary>
        ///     ユニットモデルリストビューの列の数
        /// </summary>
        public const int ModelListColumnCount = 10;

        /// <summary>
        ///     改良リストビューの列の数
        /// </summary>
        public const int UpgradeListColumnCount = 3;

        /// <summary>
        ///     装備リストビューの列の数
        /// </summary>
        public const int EquipmentListColumnCount = 2;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public UnitEditorForm()
        {
            InitializeComponent();

            // フォームの初期化
            InitForm();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 国家リストビュー
            countryListView.BeginUpdate();
            countryListView.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListView.Items.Add(Countries.Strings[(int) country]);
            }
            countryListView.EndUpdate();

            // 兵科コンボボックス
            branchComboBox.BeginUpdate();
            branchComboBox.Items.Clear();
            foreach (string s in Branches.GetNames())
            {
                branchComboBox.Items.Add(s);
            }
            branchComboBox.EndUpdate();

            // 付属可能旅団リストビュー
            allowedBrigadesListView.BeginUpdate();
            allowedBrigadesListView.Items.Clear();
            int width = 60;
            foreach (UnitType type in Units.BrigadeTypes)
            {
                string s = Units.Items[(int) type].ToString();
                allowedBrigadesListView.Items.Add(s);
                // +16はチェックボックスの分
                width = Math.Max(width,
                    (int) g.MeasureString(s, allowedBrigadesListView.Font).Width + DeviceCaps.GetScaledWidth(16));
            }
            allowedBrigadesDummyColumnHeader.Width = width;
            allowedBrigadesListView.EndUpdate();

            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                // 実ユニット種類コンボボックス
                realUnitTypeComboBox.BeginUpdate();
                realUnitTypeComboBox.Items.Clear();
                width = realUnitTypeComboBox.Width;
                foreach (RealUnitType type in Enum.GetValues(typeof (RealUnitType)))
                {
                    string s = Units.Items[(int) Units.RealTypeTable[(int) type]].ToString();
                    realUnitTypeComboBox.Items.Add(s);
                    width = Math.Max(width,
                        (int) g.MeasureString(s, realUnitTypeComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                }
                realUnitTypeComboBox.DropDownWidth = width;
                realUnitTypeComboBox.EndUpdate();

                // スプライト種類コンボボックス
                spriteTypeComboBox.BeginUpdate();
                spriteTypeComboBox.Items.Clear();
                width = spriteTypeComboBox.Width;
                foreach (SpriteType type in Enum.GetValues(typeof (SpriteType)))
                {
                    string s = Units.Items[(int) Units.SpriteTypeTable[(int) type]].ToString();
                    spriteTypeComboBox.Items.Add(s);
                    width = Math.Max(width,
                        (int) g.MeasureString(s, spriteTypeComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                }
                spriteTypeComboBox.DropDownWidth = width;
                spriteTypeComboBox.EndUpdate();

                // 代替ユニット種類コンボボックス
                transmuteComboBox.BeginUpdate();
                transmuteComboBox.Items.Clear();
                width = transmuteComboBox.Width;
                foreach (UnitType type in Units.DivisionTypes)
                {
                    string s = Units.Items[(int) type].ToString();
                    transmuteComboBox.Items.Add(s);
                    width = Math.Max(width,
                        (int) g.MeasureString(s, transmuteComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                }
                transmuteComboBox.DropDownWidth = width;
                transmuteComboBox.EndUpdate();

                // 資源コンボボックス
                resourceComboBox.BeginUpdate();
                resourceComboBox.Items.Clear();
                width = resourceComboBox.Width;
                foreach (EquipmentType type in Enum.GetValues(typeof (EquipmentType)))
                {
                    string s = Config.GetText(Units.EquipmentNames[(int) type]);
                    resourceComboBox.Items.Add(s);
                    width = Math.Max(width,
                        (int) g.MeasureString(s, resourceComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                }
                resourceComboBox.DropDownWidth = width;
                resourceComboBox.EndUpdate();
            }

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
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                upgradeTimeBoostCheckBox.Enabled = true;
                autoUpgradeCheckBox.Enabled = true;
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
                upgradeTimeBoostCheckBox.Enabled = false;
                autoUpgradeCheckBox.Enabled = false;
                autoUpgradeClassComboBox.Enabled = false;
                autoUpgradeModelComboBox.Enabled = false;
                noFuelCombatModLabel.Enabled = false;
                noFuelCombatModTextBox.Enabled = false;
                upgradeGroupBox.Enabled = false;

                maxAllowedBrigadesNumericUpDown.ResetText();
                reinforceCostTextBox.ResetText();
                reinforceTimeTextBox.ResetText();
                autoUpgradeClassComboBox.SelectedIndex = -1;
                autoUpgradeClassComboBox.ResetText();
                autoUpgradeModelComboBox.SelectedIndex = -1;
                autoUpgradeModelComboBox.ResetText();
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

            // AoD1.07以降またはDH
            if (((Game.Type == GameType.ArsenalOfDemocracy) && (Game.Version >= 107)) ||
                (Game.Type == GameType.DarkestHour))
            {
                maxAllowedBrigadesLabel.Enabled = true;
                maxAllowedBrigadesNumericUpDown.Enabled = true;
            }
            else
            {
                maxAllowedBrigadesLabel.Enabled = false;
                maxAllowedBrigadesNumericUpDown.Enabled = false;
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
                detachableCheckBox.Enabled = false;
            }
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // 付属可能旅団の値が変化してしまうので一旦選択を解除する
            classListBox.SelectedIndex = -1;

            // ユニットモデルの編集項目の文字列を初期化する
            InitModelItemText();

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
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            classListBox.Refresh();
            modelListView.Refresh();
            UpdateClassEditableItems();
            UpdateModelEditableItems();
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            switch (id)
            {
                case EditorItemId.MaxAllowedBrigades:
                    Log.Verbose("[Unit] Notified max allowed brigades");
                    // 最大付属旅団数の表示を更新する
                    UpdateMaxAllowedBrigades();
                    break;

                case EditorItemId.CommonModelName:
                    Log.Verbose("[Unit] Notified common model name");
                    // ユニットモデルリストのモデル名を更新する
                    UpdateModelListName();
                    // ユニットモデル名の表示を更新する
                    UpdateModelNameTextBox();
                    break;

                case EditorItemId.CountryModelName:
                    Log.Verbose("[Unit] Notified country model name");
                    // ユニットモデルリストのモデル名を更新する
                    UpdateModelListName();
                    // ユニットモデル名の表示を更新する
                    UpdateModelNameTextBox();
                    break;
            }
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // ユニットモデルリストビュー
            noColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[0];
            nameColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[1];
            buildCostColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[2];
            buildTimeColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[3];
            manpowerColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[4];
            supplyColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[5];
            fuelColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[6];
            organisationColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[7];
            moraleColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[8];
            maxSpeedColumnHeader.Width = HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[9];

            // ユニットクラスリストボックス
            classListBox.ItemHeight = DeviceCaps.GetScaledHeight(classListBox.ItemHeight);

            // 国家リストビュー
            countryDummyColumnHeader.Width = DeviceCaps.GetScaledWidth(countryDummyColumnHeader.Width);

            // 改良リストビュー
            upgradeTypeColumnHeader.Width = HoI2Editor.Settings.UnitEditor.UpgradeListColumnWidth[0];
            upgradeCostColumnHeader.Width = HoI2Editor.Settings.UnitEditor.UpgradeListColumnWidth[1];
            upgradeTimeColumnHeader.Width = HoI2Editor.Settings.UnitEditor.UpgradeListColumnWidth[2];

            // 装備リストビュー
            resourceColumnHeader.Width = HoI2Editor.Settings.UnitEditor.EquipmentListColumnWidth[0];
            quantityColumnHeader.Width = HoI2Editor.Settings.UnitEditor.EquipmentListColumnWidth[1];


            // ウィンドウの位置
            Location = HoI2Editor.Settings.UnitEditor.Location;
            Size = HoI2Editor.Settings.UnitEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
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
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
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
                    HoI2Editor.Save();
                    break;
                case DialogResult.No:
                    HoI2Editor.SaveCanceled = true;
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnUnitEditorFormClosed();
        }

        /// <summary>
        ///     フォーム移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormMove(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.UnitEditor.Location = Location;
            }
        }

        /// <summary>
        ///     フォームリサイズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2Editor.Settings.UnitEditor.Size = Size;
            }
        }

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
                        HoI2Editor.Save();
                        break;
                }
            }

            HoI2Editor.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.Save();
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
                classListBox.Items.Add(unit);
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
            else
            {
                // 編集項目を無効化する
                DisableModelEditableItems();
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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // リストビューの項目を更新する
            Country country = GetSelectedCountry();
            modelListView.BeginUpdate();
            for (int i = 0; i < unit.Models.Count; i++)
            {
                modelListView.Items[i].SubItems[1].Text = unit.GetModelName(i, country);
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
        ///     ユニットモデルリストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < ModelListColumnCount))
            {
                HoI2Editor.Settings.UnitEditor.ModelListColumnWidth[e.ColumnIndex] =
                    modelListView.Columns[e.ColumnIndex].Width;
            }
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

            var item = new ListViewItem { Text = IntHelper.ToString(index) };
            item.SubItems.Add(unit.GetModelName(index, GetSelectedCountry()));
            item.SubItems.Add(DoubleHelper.ToString(model.Cost));
            item.SubItems.Add(DoubleHelper.ToString(model.BuildTime));
            item.SubItems.Add(DoubleHelper.ToString(model.ManPower));
            item.SubItems.Add(DoubleHelper.ToString(model.SupplyConsumption));
            item.SubItems.Add(DoubleHelper.ToString(model.FuelConsumption));
            item.SubItems.Add(DoubleHelper.ToString(model.DefaultOrganization));
            item.SubItems.Add(DoubleHelper.ToString(model.Morale));
            item.SubItems.Add(DoubleHelper.ToString(model.MaxSpeed));

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // ユニットモデルを挿入する
            var model = new UnitModel();
            int index = (modelListView.SelectedIndices.Count > 0) ? (modelListView.SelectedIndices[0] + 1) : 0;
            InsertModel(unit, model, index, "");

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // ユニットモデルを挿入する
            var model = new UnitModel(unit.Models[index]);
            InsertModel(unit, model, index + 1, unit.GetModelName(index));

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // ユニットモデルを削除する
            RemoveModel(unit, index);

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottonButtonClick(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            // ユニットモデルリストの更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.ModelList, this);
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
            unit.InsertModel(model, index, name);

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

            // ユニットモデルリストの表示を更新する
            UpdateModelList();

            // 移動先の項目を選択する
            modelListView.Items[dest].Focused = true;
            modelListView.Items[dest].Selected = true;
        }

        #endregion

        #region 国家リストビュー

        /// <summary>
        ///     選択中の国タグを取得する
        /// </summary>
        /// <returns></returns>
        private Country GetSelectedCountry()
        {
            return (countryListView.SelectedIndices.Count > 0)
                ? Countries.Tags[countryListView.SelectedIndices[0]]
                : Country.None;
        }

        /// <summary>
        ///     国家リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // ユニットモデルリストのモデル名を更新する
            UpdateModelListName();

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            // ユニットモデル画像名を更新する
            Image oldImage = modelImagePictureBox.Image;
            string fileName = GetModelImageFileName(unit, index, GetSelectedCountry());
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                var bitmap = new Bitmap(fileName);
                bitmap.MakeTransparent(Color.Lime);
                modelImagePictureBox.Image = bitmap;
            }
            else
            {
                modelImagePictureBox.Image = null;
            }
            if (oldImage != null)
            {
                oldImage.Dispose();
            }

            // ユニットモデル名を更新する
            UpdateModelNameTextBox();
        }

        #endregion

        #region ユニットクラスタブ

        /// <summary>
        ///     ユニットクラスタブの編集項目を更新する
        /// </summary>
        private void UpdateClassEditableItems()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            classNameTextBox.Text = Config.ExistsKey(unit.Name) ? Config.GetText(unit.Name) : "";
            classShortNameTextBox.Text = unit.GetShortName();
            classDescTextBox.Text = unit.GetDesc();
            classShortDescTextBox.Text = unit.GetShortDesc();

            // 兵科
            branchComboBox.SelectedIndex = (int) unit.Branch - 1;

            // 付属旅団
            if (unit.Organization == UnitOrganization.Division)
            {
                if (unit.CanModifyMaxAllowedBrigades())
                {
                    maxAllowedBrigadesLabel.Enabled = true;
                    maxAllowedBrigadesNumericUpDown.Enabled = true;
                }
                else
                {
                    maxAllowedBrigadesLabel.Enabled = false;
                    maxAllowedBrigadesNumericUpDown.Enabled = false;
                }
                maxAllowedBrigadesNumericUpDown.Value = unit.GetMaxAllowedBrigades();
                maxAllowedBrigadesNumericUpDown.Text = IntHelper.ToString((int) maxAllowedBrigadesNumericUpDown.Value);

                Graphics g = Graphics.FromHwnd(allowedBrigadesListView.Handle);
                int width = DeviceCaps.GetScaledWidth(60);
                allowedBrigadesListView.ItemChecked -= OnAllowedBrigadesListViewItemChecked;
                allowedBrigadesListView.Enabled = true;
                allowedBrigadesListView.BeginUpdate();
                allowedBrigadesListView.Items.Clear();
                foreach (Unit brigade in Units.BrigadeTypes
                    .Select(type => Units.Items[(int) type])
                    .Where(brigade => (brigade.Branch == unit.Branch) && (brigade.Models.Count > 0)))
                {
                    string s = brigade.ToString();
                    // +16はチェックボックスの分
                    width = Math.Max(width,
                        (int) g.MeasureString(s, allowedBrigadesListView.Font).Width + DeviceCaps.GetScaledWidth(16));
                    var item = new ListViewItem
                    {
                        Text = s,
                        Checked = unit.AllowedBrigades.Contains(brigade.Type),
                        ForeColor = unit.IsDirtyAllowedBrigades(brigade.Type) ? Color.Red : SystemColors.WindowText,
                        Tag = brigade
                    };
                    allowedBrigadesListView.Items.Add(item);
                }
                allowedBrigadesDummyColumnHeader.Width = width;
                allowedBrigadesListView.EndUpdate();
                allowedBrigadesListView.ItemChecked += OnAllowedBrigadesListViewItemChecked;
            }
            else
            {
                maxAllowedBrigadesLabel.Enabled = false;
                maxAllowedBrigadesNumericUpDown.Enabled = false;
                maxAllowedBrigadesNumericUpDown.ResetText();

                allowedBrigadesListView.Enabled = false;
                allowedBrigadesListView.BeginUpdate();
                allowedBrigadesListView.Items.Clear();
                allowedBrigadesListView.EndUpdate();
            }

            // DH1.03以降のユニット設定
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                listPrioLabel.Enabled = true;
                listPrioNumericUpDown.Enabled = true;
                listPrioNumericUpDown.Value = unit.ListPrio;
                listPrioNumericUpDown.Text = IntHelper.ToString(unit.ListPrio);

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
                    eyrNumericUpDown.Text = IntHelper.ToString((int) eyrNumericUpDown.Value);
                    gfxPrioNumericUpDown.Value = unit.GfxPrio;
                    gfxPrioNumericUpDown.Text = IntHelper.ToString((int) gfxPrioNumericUpDown.Value);
                    uiPrioNumericUpDown.Value = unit.UiPrio;
                    uiPrioNumericUpDown.Text = IntHelper.ToString((int) uiPrioNumericUpDown.Value);
                    realUnitTypeComboBox.SelectedIndex = (int) unit.RealType;
                    spriteTypeComboBox.SelectedIndex = (int) unit.Sprite;
                    transmuteComboBox.SelectedIndex = Units.UnitTypes.IndexOf(unit.Transmute);
                    militaryValueTextBox.Text = DoubleHelper.ToString(unit.Value);
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
                if ((unit.Branch == Branch.Army) && (unit.Organization == UnitOrganization.Brigade))
                {
                    engineerCheckBox.Enabled = true;
                }
                else
                {
                    engineerCheckBox.Enabled = false;
                }

                // 海軍旅団
                if ((unit.Branch == Branch.Navy) && (unit.Organization == UnitOrganization.Brigade))
                {
                    cagCheckBox.Enabled = true;
                }
                else
                {
                    cagCheckBox.Enabled = false;
                }

                // 空軍旅団
                if ((unit.Branch == Branch.Airforce) && (unit.Organization == UnitOrganization.Brigade))
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

            detachableCheckBox.Checked = detachableCheckBox.Enabled && unit.Detachable;
            cagCheckBox.Checked = cagCheckBox.Enabled && unit.Cag;
            escortCheckBox.Checked = escortCheckBox.Enabled && unit.Escort;
            engineerCheckBox.Checked = engineerCheckBox.Enabled && unit.Engineer;
            defaultTypeCheckBox.Checked = unit.DefaultType;
            productableCheckBox.Checked = unit.Productable;

            // 改良
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103) &&
                (unit.Organization == UnitOrganization.Division) && (unit.Branch != Branch.Navy))
            {
                upgradeGroupBox.Enabled = true;
                UpdateUpgradeList(unit);
                UpdateUpgradeTypeComboBox();
                const string def = "0";
                upgradeCostTextBox.Text = def;
                upgradeTimeTextBox.Text = def;
            }
            else
            {
                upgradeGroupBox.Enabled = false;
                // 編集項目の値をクリアする
                upgradeListView.BeginUpdate();
                upgradeListView.Items.Clear();
                upgradeListView.EndUpdate();
                upgradeTypeComboBox.BeginUpdate();
                upgradeTypeComboBox.Items.Clear();
                upgradeTypeComboBox.EndUpdate();
                upgradeCostTextBox.ResetText();
                upgradeTimeTextBox.ResetText();
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

            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                upgradeCostTextBox.ForeColor = SystemColors.WindowText;
                upgradeTimeTextBox.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     最大付属旅団数を更新する
        /// </summary>
        private void UpdateMaxAllowedBrigades()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットクラスが旅団ならば何もしない
            if (unit.Organization == UnitOrganization.Brigade)
            {
                return;
            }

            maxAllowedBrigadesNumericUpDown.Value = unit.GetMaxAllowedBrigades();
            maxAllowedBrigadesNumericUpDown.Text = IntHelper.ToString((int) maxAllowedBrigadesNumericUpDown.Value);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (Config.ExistsKey(unit.Name))
            {
                if (classNameTextBox.Text.Equals(Config.GetText(unit.Name)))
                {
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(classNameTextBox.Text))
                {
                    return;
                }
            }

            Log.Info("[Unit] unit name: {0} -> {1}", Config.GetText(unit.Name), classNameTextBox.Text);

            // 値を更新する
            Config.SetText(unit.Name, classNameTextBox.Text, Game.UnitTextFileName);

            // ユニットクラスリストボックスの表示を更新する
            classListBox.Refresh();

            if (unit.Organization == UnitOrganization.Division)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                int margin = DeviceCaps.GetScaledWidth(2) + 1;

                if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
                {
                    // 実ユニットコンボボックスの項目を更新する
                    int index = Array.IndexOf(Units.RealTypeTable, unit.Type);
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
                    index = Array.IndexOf(Units.SpriteTypeTable, unit.Type);
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
                }
            }

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Name);

            // 文字色を変更する
            classNameTextBox.ForeColor = Color.Red;

            // ユニットクラス名の更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.UnitName, this);
        }

        /// <summary>
        ///     ユニットクラス短縮名テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClassShortNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (classShortNameTextBox.Text.Equals(unit.GetShortName()))
            {
                return;
            }

            Log.Info("[Unit] unit short name: {0} -> {1} ({2})", unit.GetShortName(), classShortNameTextBox.Text, unit);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (classDescTextBox.Text.Equals(Config.GetText(unit.Desc)))
            {
                return;
            }

            Log.Info("[Unit] unit desc: {0} -> {1} ({2})", unit.GetDesc(), classDescTextBox.Text, unit);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (classShortDescTextBox.Text.Equals(Config.GetText(unit.ShortDesc)))
            {
                return;
            }

            Log.Info("[Unit] unit short desc: {0} -> {1} ({2})", unit.GetShortDesc(), classShortDescTextBox.Text, unit);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var branch = (Branch) (branchComboBox.SelectedIndex + 1);
            if (branch == unit.Branch)
            {
                return;
            }

            Log.Info("[Unit] branch: {0} -> {1} ({2})", Branches.GetName(unit.Branch), Branches.GetName(branch), unit);

            // 値を更新する
            unit.Branch = branch;

            // DH1.03以降で師団の兵科を変更する場合、実ユニット種類も連動する
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103) &&
                (unit.Organization == UnitOrganization.Division))
            {
                RealUnitType type;
                switch (branch)
                {
                    case Branch.Army:
                        type = RealUnitType.Infantry;
                        break;

                    case Branch.Navy:
                        type = RealUnitType.Destroyer;
                        break;

                    case Branch.Airforce:
                        type = RealUnitType.Interceptor;
                        break;

                    default:
                        type = RealUnitType.Infantry;
                        break;
                }

                Log.Info("[Unit] Switched real unit type: {0} -> {1} ({2})",
                    Units.Items[(int) Units.RealTypeTable[(int) unit.RealType]],
                    Units.Items[(int) Units.RealTypeTable[(int) type]], unit);

                unit.RealType = type;

                // 編集済みフラグを設定する
                unit.SetDirty(UnitClassItemId.RealType);
            }

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Branch);

            // ユニットクラスタブ/ユニットモデルタブの表示を更新する
            UpdateClassEditableItems();
            UpdateModelEditableItems();
        }

        /// <summary>
        ///     統計グループ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEyrNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var eyr = (int) eyrNumericUpDown.Value;
            if (eyr == unit.Eyr)
            {
                return;
            }

            Log.Info("[Unit] eyr: {0} -> {1} ({2})", unit.Eyr, eyr, unit);

            // 値を更新する
            unit.Eyr = eyr;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Eyr);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var prio = (int) gfxPrioNumericUpDown.Value;
            if (prio == unit.GfxPrio)
            {
                return;
            }

            Log.Info("[Unit] gfx prio: {0} -> {1} ({2})", unit.GfxPrio, prio, unit);

            // 値を更新する
            unit.GfxPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.GfxPrio);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var prio = (int) listPrioNumericUpDown.Value;
            if (prio == unit.ListPrio)
            {
                return;
            }

            Log.Info("[Unit] list prio: {0} -> {1} ({2})", unit.ListPrio, prio, unit);

            // 値を更新する
            unit.ListPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.ListPrio);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var prio = (int) uiPrioNumericUpDown.Value;
            if (prio == unit.UiPrio)
            {
                return;
            }

            Log.Info("[Unit] ui prio: {0} -> {1} ({2})", unit.UiPrio, prio, unit);

            // 値を更新する
            unit.UiPrio = prio;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.UiPrio);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 非選択になった時には何もしない
            if (realUnitTypeComboBox.SelectedIndex == -1)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var type = (RealUnitType) realUnitTypeComboBox.SelectedIndex;
            if (type == unit.RealType)
            {
                return;
            }

            Log.Info("[Unit] real unit type: {0} -> {1} ({2})",
                Units.Items[(int) Units.RealTypeTable[(int) unit.RealType]],
                Units.Items[(int) Units.RealTypeTable[(int) type]], unit);

            // 値を更新する
            unit.RealType = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.RealType);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (defaultTypeCheckBox.Checked == unit.DefaultType)
            {
                return;
            }

            Log.Info("[Unit] default production type: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.DefaultType),
                BoolHelper.ToYesNo(defaultTypeCheckBox.Checked), unit);

            // 値を更新する
            unit.DefaultType = defaultTypeCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.DefaultType);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 非選択になった時には何もしない
            if (spriteTypeComboBox.SelectedIndex == -1)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var type = (SpriteType) spriteTypeComboBox.SelectedIndex;
            if (type == unit.Sprite)
            {
                return;
            }

            Log.Info("[Unit] sprite type: {0} -> {1} ({2})",
                Units.Items[(int) Units.SpriteTypeTable[(int) unit.Sprite]],
                Units.Items[(int) Units.SpriteTypeTable[(int) type]], unit);

            // 値を更新する
            unit.Sprite = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Sprite);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 非選択になった時には何もしない
            if (transmuteComboBox.SelectedIndex == -1)
            {
                return;
            }

            // 値に変化がなければ何もしない
            var type = (UnitType) transmuteComboBox.SelectedIndex;
            if (type == unit.Transmute)
            {
                return;
            }

            Log.Info("[Unit] transmute type: {0} -> {1} ({2})", Units.Items[(int) unit.Transmute],
                Units.Items[(int) type], unit);

            // 値を更新する
            unit.Transmute = type;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Transmute);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(militaryValueTextBox.Text, out val))
            {
                militaryValueTextBox.Text = DoubleHelper.ToString(unit.Value);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, unit.Value))
            {
                return;
            }

            Log.Info("[Unit] military value: {0} -> {1} ({2})", DoubleHelper.ToString(unit.Value),
                DoubleHelper.ToString(val), unit);

            // 値を更新する
            unit.Value = val;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Vaule);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            Log.Info("[Unit] max speed step: {0} -> {1} ({2})", unit.MaxSpeedStep, val, unit);

            // 値を更新する
            unit.MaxSpeedStep = val;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.MaxSpeedStep);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }
            // 値に変化がなければ何もしない
            if (productableCheckBox.Checked == unit.Productable)
            {
                return;
            }

            Log.Info("[Unit] productable: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.Productable),
                BoolHelper.ToYesNo(productableCheckBox.Checked), unit);

            // 値を更新する
            unit.Productable = productableCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Productable);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (detachableCheckBox.Checked == unit.Detachable)
            {
                return;
            }

            Log.Info("[Unit] detachable: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.Detachable),
                BoolHelper.ToYesNo(detachableCheckBox.Checked), unit);

            // 値を更新する
            unit.Detachable = detachableCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Detachable);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (cagCheckBox.Checked == unit.Cag)
            {
                return;
            }

            Log.Info("[Unit] cag: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.Cag),
                BoolHelper.ToYesNo(cagCheckBox.Checked), unit);

            // 値を更新する
            unit.Cag = cagCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Cag);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (escortCheckBox.Checked == unit.Escort)
            {
                return;
            }

            Log.Info("[Unit] escort: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.Escort),
                BoolHelper.ToYesNo(escortCheckBox.Checked), unit);

            // 値を更新する
            unit.Escort = escortCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Escort);

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (engineerCheckBox.Checked == unit.Engineer)
            {
                return;
            }

            Log.Info("[Unit] engineer: {0} -> {1} ({2})", BoolHelper.ToYesNo(unit.Engineer),
                BoolHelper.ToYesNo(engineerCheckBox.Checked), unit);

            // 値を更新する
            unit.Engineer = engineerCheckBox.Checked;

            // 編集済みフラグを設定する
            unit.SetDirty(UnitClassItemId.Engineer);

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
            // HoI2またはAoD1.07より前の場合は何もしない
            if ((Game.Type == GameType.HeartsOfIron2) ||
                ((Game.Type == GameType.ArsenalOfDemocracy) && (Game.Version < 107)))
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (maxAllowedBrigadesNumericUpDown.Value == unit.GetMaxAllowedBrigades())
            {
                return;
            }

            Log.Info("[Unit] Max allowed brigades: {0} -> {1} ({2})", unit.GetMaxAllowedBrigades(),
                maxAllowedBrigadesNumericUpDown.Value, unit);

            // 値を更新する
            unit.SetMaxAllowedBrigades((int) maxAllowedBrigadesNumericUpDown.Value);

            // 文字色を変更する
            maxAllowedBrigadesNumericUpDown.ForeColor = Color.Red;

            // 最大付属旅団数の更新を通知する
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                HoI2Editor.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
            }
        }

        /// <summary>
        ///     付属旅団リストビューののチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowedBrigadesListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }
            var brigade = e.Item.Tag as Unit;
            if (brigade == null)
            {
                return;
            }

            if (e.Item.Checked)
            {
                // 値に変化がなければ何もしない
                if (unit.AllowedBrigades.Contains(brigade.Type))
                {
                    return;
                }

                Log.Info("[Unit] Added allowed brigades: {0} ({1})", brigade, unit);

                // 値を更新する
                unit.AllowedBrigades.Add(brigade.Type);
            }
            else
            {
                // 値に変化がなければ何もしない
                if (!unit.AllowedBrigades.Contains(brigade.Type))
                {
                    return;
                }

                Log.Info("[Unit] Removed allowed brigades: {0} ({1})", brigade, unit);

                // 値を更新する
                unit.AllowedBrigades.Remove(brigade.Type);
            }

            // 編集済みフラグを設定する
            unit.SetDirtyAllowedBrigades(brigade.Type);

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

            // 編集項目を有効化する
            EnableUpgradeItems();
        }

        /// <summary>
        ///     改良情報の編集項目を有効化する
        /// </summary>
        private void EnableUpgradeItems()
        {
            upgradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     改良情報の編集項目を無効化する
        /// </summary>
        private void DisableUpgradeItems()
        {
            upgradeRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     改良ユニット種類コンボボックスの項目を更新する
        /// </summary>
        private void UpdateUpgradeTypeComboBox()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            Graphics g = Graphics.FromHwnd(autoUpgradeClassComboBox.Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;
            upgradeTypeComboBox.BeginUpdate();
            upgradeTypeComboBox.Items.Clear();
            int width = upgradeTypeComboBox.Width;
            // 現在の改良先クラスと兵科がマッチしない場合、ワンショットで候補に登録する
            Unit current = null;
            if (upgradeListView.SelectedIndices.Count > 0)
            {
                current = Units.Items[(int) unit.Upgrades[upgradeListView.SelectedIndices[0]].Type];
                if ((current.Branch != unit.Branch) || (current.Models.Count == 0))
                {
                    width = Math.Max(width,
                        (int) g.MeasureString(current.ToString(), upgradeTypeComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                    upgradeTypeComboBox.Items.Add(current);
                }
            }
            foreach (Unit u in Units.DivisionTypes
                .Select(type => Units.Items[(int) type])
                .Where(u => (u.Branch == unit.Branch) &&
                            (u.Models.Count > 0)))
            {
                width = Math.Max(width,
                    (int) g.MeasureString(u.ToString(), upgradeTypeComboBox.Font).Width +
                    SystemInformation.VerticalScrollBarWidth + margin);
                upgradeTypeComboBox.Items.Add(u);
            }
            upgradeTypeComboBox.DropDownWidth = width;
            if (current != null)
            {
                upgradeTypeComboBox.SelectedItem = current;
            }
            else
            {
                if (upgradeTypeComboBox.Items.Count > 0)
                {
                    upgradeTypeComboBox.SelectedIndex = 0;
                }
            }
            upgradeTypeComboBox.EndUpdate();
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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var u = upgradeTypeComboBox.Items[e.Index] as Unit;
            if (u != null)
            {
                Brush brush;
                if (upgradeListView.SelectedIndices.Count > 0)
                {
                    UnitUpgrade upgrade = unit.Upgrades[upgradeListView.SelectedIndices[0]];
                    if ((u.Type == upgrade.Type) && upgrade.IsDirty(UnitUpgradeItemId.Type))
                    {
                        brush = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        brush = new SolidBrush(SystemColors.WindowText);
                    }
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                e.Graphics.DrawString(u.ToString(), e.Font, brush, e.Bounds);
                brush.Dispose();
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択項目がなければ編集を禁止する
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                DisableUpgradeItems();
                return;
            }
            UnitUpgrade upgrade = unit.Upgrades[upgradeListView.SelectedIndices[0]];

            // 編集項目の値を更新する
            UpdateUpgradeTypeComboBox();
            upgradeCostTextBox.Text = DoubleHelper.ToString(upgrade.UpgradeCostFactor);
            upgradeTimeTextBox.Text = DoubleHelper.ToString(upgrade.UpgradeTimeFactor);

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
        ///     改良リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < UpgradeListColumnCount))
            {
                HoI2Editor.Settings.UnitEditor.UpgradeListColumnWidth[e.ColumnIndex] =
                    upgradeListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     改良ユニット種類コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 値に変化がなければ何もしない
            var selected = upgradeTypeComboBox.SelectedItem as Unit;
            if (selected == null)
            {
                return;
            }
            if (selected.Type == upgrade.Type)
            {
                return;
            }

            Unit old = Units.Items[(int) upgrade.Type];

            Log.Info("[Unit] upgrade type: {0} -> {1} ({2})", old, selected, unit);

            // 値を更新する
            upgrade.Type = selected.Type;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].Text = selected.ToString();

            // 編集済みフラグを設定する
            upgrade.SetDirty(UnitUpgradeItemId.Type);
            unit.SetDirtyFile();

            if ((old.Branch != unit.Branch) || (old.Models.Count == 0))
            {
                // 改良先クラスと兵科がマッチしていなかった場合は、項目を更新する
                UpdateUpgradeTypeComboBox();
            }
            else
            {
                // 改良ユニット種類コンボボックスの項目色を変更するために描画更新する
                upgradeTypeComboBox.Refresh();
            }
        }

        /// <summary>
        ///     改良コストテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(upgradeCostTextBox.Text, out val))
            {
                upgradeCostTextBox.Text = DoubleHelper.ToString(upgrade.UpgradeCostFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, upgrade.UpgradeCostFactor))
            {
                return;
            }

            Log.Info("[Unit] upgrade cost: {0} -> {1} ({2})", DoubleHelper.ToString(upgrade.UpgradeCostFactor),
                DoubleHelper.ToString(val), unit);

            // 値を更新する
            upgrade.UpgradeCostFactor = val;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].SubItems[1].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            upgrade.SetDirty(UnitUpgradeItemId.UpgradeCostFactor);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];
            UnitUpgrade upgrade = unit.Upgrades[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(upgradeTimeTextBox.Text, out val))
            {
                upgradeTimeTextBox.Text = DoubleHelper.ToString(upgrade.UpgradeTimeFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, upgrade.UpgradeTimeFactor))
            {
                return;
            }

            Log.Info("[Unit] upgrade time: {0} -> {1} ({2})", DoubleHelper.ToString(upgrade.UpgradeTimeFactor),
                DoubleHelper.ToString(val), unit);

            // 値を更新する
            upgrade.UpgradeTimeFactor = val;

            // 改良リストビューの項目を更新する
            upgradeListView.Items[index].SubItems[2].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            upgrade.SetDirty(UnitUpgradeItemId.UpgradeTimeFactor);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            var selected = upgradeTypeComboBox.SelectedItem as Unit;
            var upgrade = new UnitUpgrade { Type = (selected != null) ? selected.Type : unit.Type };
            double val;
            if (DoubleHelper.TryParse(upgradeCostTextBox.Text, out val))
            {
                upgrade.UpgradeCostFactor = val;
            }
            if (DoubleHelper.TryParse(upgradeTimeTextBox.Text, out val))
            {
                upgrade.UpgradeTimeFactor = val;
            }

            Log.Info("[Unit] Added upgrade info: {0} {1} {2} ({3})", Units.Items[(int) upgrade.Type],
                DoubleHelper.ToString(upgrade.UpgradeCostFactor), DoubleHelper.ToString(upgrade.UpgradeTimeFactor), unit);

            // 改良情報を追加する
            unit.Upgrades.Add(upgrade);

            // 編集済みフラグを設定する
            upgrade.SetDirtyAll();
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (upgradeListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = upgradeListView.SelectedIndices[0];

            Log.Info("[Unit] Removed upgrade info: {0} ({1})", Units.Items[(int) unit.Upgrades[index].Type], unit);

            // 改良情報を削除する
            unit.Upgrades.RemoveAt(index);

            // 編集済みフラグを設定する
            unit.SetDirtyFile();

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
            var item = new ListViewItem { Text = Units.Items[(int) upgrade.Type].ToString() };
            item.SubItems.Add(DoubleHelper.ToString(upgrade.UpgradeCostFactor));
            item.SubItems.Add(DoubleHelper.ToString(upgrade.UpgradeTimeFactor));

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // モデル画像
            Image oldImage = modelImagePictureBox.Image;
            string fileName = GetModelImageFileName(unit, index, GetSelectedCountry());
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                var bitmap = new Bitmap(fileName);
                bitmap.MakeTransparent(Color.Lime);
                modelImagePictureBox.Image = bitmap;
            }
            else
            {
                modelImagePictureBox.Image = null;
            }
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
            // モデルアイコン
            oldImage = modelIconPictureBox.Image;
            fileName = GetModelIconFileName(unit, index);
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                var bitmap = new Bitmap(GetModelIconFileName(unit, index));
                bitmap.MakeTransparent(Color.Lime);
                modelIconPictureBox.Image = bitmap;
            }
            else
            {
                modelIconPictureBox.Image = null;
            }
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
            // モデル名
            UpdateModelNameTextBox();

            // 組織率
            defaultOrganisationTextBox.Text = DoubleHelper.ToString(model.DefaultOrganization);
            defaultOrganisationTextBox.ForeColor = model.IsDirty(UnitModelItemId.DefaultOrganization)
                ? Color.Red
                : SystemColors.WindowText;
            // 士気
            moraleTextBox.Text = DoubleHelper.ToString(model.Morale);
            moraleTextBox.ForeColor = model.IsDirty(UnitModelItemId.Morale) ? Color.Red : SystemColors.WindowText;
            // 消費物資
            supplyConsumptionTextBox.Text = DoubleHelper.ToString(model.SupplyConsumption);
            supplyConsumptionTextBox.ForeColor = model.IsDirty(UnitModelItemId.SupplyConsumption)
                ? Color.Red
                : SystemColors.WindowText;
            // 消費燃料
            fuelConsumptionTextBox.Text = DoubleHelper.ToString(model.FuelConsumption);
            fuelConsumptionTextBox.ForeColor = model.IsDirty(UnitModelItemId.FuelConsumption)
                ? Color.Red
                : SystemColors.WindowText;
            // 必要IC
            costTextBox.Text = DoubleHelper.ToString(model.Cost);
            costTextBox.ForeColor = model.IsDirty(UnitModelItemId.Cost) ? Color.Red : SystemColors.WindowText;
            // 必要時間
            buildTimeTextBox.Text = DoubleHelper.ToString(model.BuildTime);
            buildTimeTextBox.ForeColor = model.IsDirty(UnitModelItemId.BuildTime) ? Color.Red : SystemColors.WindowText;
            // 労働力
            manPowerTextBox.Text = DoubleHelper.ToString(model.ManPower);
            manPowerTextBox.ForeColor = model.IsDirty(UnitModelItemId.ManPower) ? Color.Red : SystemColors.WindowText;
            // 最大速度
            maxSpeedTextBox.Text = DoubleHelper.ToString(model.MaxSpeed);
            maxSpeedTextBox.ForeColor = model.IsDirty(UnitModelItemId.MaxSpeed) ? Color.Red : SystemColors.WindowText;
            // 対空防御力
            airDefenceTextBox.Text = DoubleHelper.ToString(model.AirDefence);
            airDefenceTextBox.ForeColor = model.IsDirty(UnitModelItemId.AirDefense)
                ? Color.Red
                : SystemColors.WindowText;
            // 対空攻撃力
            airAttackTextBox.Text = DoubleHelper.ToString(model.AirAttack);
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
                transportWeightTextBox.Text = DoubleHelper.ToString(model.TransportWeight);
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
                suppressionTextBox.Text = DoubleHelper.ToString(model.Suppression);
                suppressionTextBox.ForeColor = model.IsDirty(UnitModelItemId.Suppression)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 防御力
                defensivenessLabel.Enabled = true;
                defensivenessTextBox.Enabled = true;
                defensivenessTextBox.Text = DoubleHelper.ToString(model.Defensiveness);
                defensivenessTextBox.ForeColor = model.IsDirty(UnitModelItemId.Defensiveness)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 耐久力
                toughnessLabel.Enabled = true;
                toughnessTextBox.Enabled = true;
                toughnessTextBox.Text = DoubleHelper.ToString(model.Toughness);
                toughnessTextBox.ForeColor = model.IsDirty(UnitModelItemId.Toughness)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 脆弱性
                softnessLabel.Enabled = true;
                softnessTextBox.Enabled = true;
                softnessTextBox.Text = DoubleHelper.ToString(model.Softness);
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
                rangeTextBox.Text = DoubleHelper.ToString(model.Range);
                rangeTextBox.ForeColor = model.IsDirty(UnitModelItemId.Range) ? Color.Red : SystemColors.WindowText;
                // 輸送負荷
                transportWeightLabel.Enabled = false;
                transportWeightTextBox.Enabled = false;
                transportWeightTextBox.ResetText();
                // 輸送能力
                transportCapabilityLabel.Enabled = true;
                transportCapabilityTextBox.Enabled = true;
                transportCapabilityTextBox.Text = DoubleHelper.ToString(model.TransportCapability);
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
                surfaceDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.SurfaceDetectionCapability);
                surfaceDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.SurfaceDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対空索敵力
                airDetectionCapabilityLabel.Enabled = true;
                airDetectionCapabilityTextBox.Enabled = true;
                airDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.AirDetectionCapability);
                airDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.AirDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
            }

            // 陸軍師団
            if ((unit.Branch == Branch.Army) && (unit.Organization == UnitOrganization.Division))
            {
                // 速度キャップ(砲兵)
                speedCapArtLabel.Enabled = true;
                speedCapArtTextBox.Enabled = true;
                speedCapArtTextBox.Text = DoubleHelper.ToString(model.SpeedCapArt);
                speedCapArtTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapArt)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(工兵)
                speedCapEngLabel.Enabled = true;
                speedCapEngTextBox.Enabled = true;
                speedCapEngTextBox.Text = DoubleHelper.ToString(model.SpeedCapEng);
                speedCapEngTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapEng)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(対戦車)
                speedCapAtLabel.Enabled = true;
                speedCapAtTextBox.Enabled = true;
                speedCapAtTextBox.Text = DoubleHelper.ToString(model.SpeedCapAt);
                speedCapAtTextBox.ForeColor = model.IsDirty(UnitModelItemId.SpeedCapAt)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 速度キャップ(対空)
                speedCapAaLabel.Enabled = true;
                speedCapAaTextBox.Enabled = true;
                speedCapAaTextBox.Text = DoubleHelper.ToString(model.SpeedCapAa);
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
                seaDefenceTextBox.Text = DoubleHelper.ToString(model.SeaDefense);
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
                seaAttackTextBox.Text = DoubleHelper.ToString(model.SeaAttack);
                seaAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SeaAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対潜攻撃力
                subAttackLabel.Enabled = true;
                subAttackTextBox.Enabled = true;
                subAttackTextBox.Text = DoubleHelper.ToString(model.SubAttack);
                subAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SubAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 船団攻撃力
                convoyAttackLabel.Enabled = true;
                convoyAttackTextBox.Enabled = true;
                convoyAttackTextBox.Text = DoubleHelper.ToString(model.ConvoyAttack);
                convoyAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.ConvoyAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 沿岸砲撃能力
                shoreBombardmentLabel.Enabled = true;
                shoreBombardmentTextBox.Enabled = true;
                shoreBombardmentTextBox.Text = DoubleHelper.ToString(model.ShoreBombardment);
                shoreBombardmentTextBox.ForeColor = model.IsDirty(UnitModelItemId.ShoreBombardment)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 射程
                distanceLabel.Enabled = true;
                distanceTextBox.Enabled = true;
                distanceTextBox.Text = DoubleHelper.ToString(model.Distance);
                distanceTextBox.ForeColor = model.IsDirty(UnitModelItemId.Distance)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 視認性
                visibilityLabel.Enabled = true;
                visibilityTextBox.Enabled = true;
                visibilityTextBox.Text = DoubleHelper.ToString(model.Visibility);
                visibilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.Visibility)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対艦索敵力
                subDetectionCapabilityLabel.Enabled = true;
                subDetectionCapabilityTextBox.Enabled = true;
                subDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.SubDetectionCapability);
                subDetectionCapabilityTextBox.ForeColor = model.IsDirty(UnitModelItemId.SubDetectionCapability)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                // 改良コスト
                upgradeCostFactorLabel.Enabled = true;
                upgradeCostFactorTextBox.Enabled = true;
                upgradeCostFactorTextBox.Text = DoubleHelper.ToString(model.UpgradeCostFactor);
                upgradeCostFactorTextBox.ForeColor = model.IsDirty(UnitModelItemId.UpgradeCostFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 改良時間
                upgradeTimeFactorLabel.Enabled = true;
                upgradeTimeFactorTextBox.Enabled = true;
                upgradeTimeFactorTextBox.Text = DoubleHelper.ToString(model.UpgradeTimeFactor);
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
                softAttackTextBox.Text = DoubleHelper.ToString(model.SoftAttack);
                softAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.SoftAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 対甲攻撃力
                hardAttackLabel.Enabled = true;
                hardAttackTextBox.Enabled = true;
                hardAttackTextBox.Text = DoubleHelper.ToString(model.HardAttack);
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
                surfaceDefenceTextBox.Text = DoubleHelper.ToString(model.SurfaceDefence);
                surfaceDefenceTextBox.ForeColor = model.IsDirty(UnitModelItemId.SurfaceDefense)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 空対艦攻撃力
                navalAttackLabel.Enabled = true;
                navalAttackTextBox.Enabled = true;
                navalAttackTextBox.Text = DoubleHelper.ToString(model.NavalAttack);
                navalAttackTextBox.ForeColor = model.IsDirty(UnitModelItemId.NavalAttack)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 戦略爆撃攻撃力
                strategicAttackLabel.Enabled = true;
                strategicAttackTextBox.Enabled = true;
                strategicAttackTextBox.Text = DoubleHelper.ToString(model.StrategicAttack);
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

            // 海軍師団以外
            if ((unit.Branch != Branch.Navy) || (unit.Organization != UnitOrganization.Division))
            {
                // 2段階改良
                upgradeTimeBoostCheckBox.Enabled = (Game.Type == GameType.DarkestHour);
                upgradeTimeBoostCheckBox.Checked = model.UpgradeTimeBoost;
                upgradeTimeBoostCheckBox.ForeColor = model.IsDirty(UnitModelItemId.UpgradeTimeBoost)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 自動改良先
                autoUpgradeCheckBox.Enabled = (Game.Type == GameType.DarkestHour);
                autoUpgradeCheckBox.Checked = model.AutoUpgrade;
                autoUpgradeCheckBox.ForeColor = model.IsDirty(UnitModelItemId.AutoUpgrade)
                    ? Color.Red
                    : SystemColors.WindowText;
                UpdateAutoUpgradeClassList();
                UpdateAutoUpgradeModelList();
            }
            else
            {
                // 2段階改良
                upgradeTimeBoostCheckBox.Enabled = false;
                upgradeTimeBoostCheckBox.Checked = false;
                autoUpgradeCheckBox.Enabled = false;
                autoUpgradeCheckBox.Checked = false;
                // 自動改良先
                autoUpgradeClassComboBox.BeginUpdate();
                autoUpgradeClassComboBox.Items.Clear();
                autoUpgradeClassComboBox.EndUpdate();
                autoUpgradeModelComboBox.BeginUpdate();
                autoUpgradeModelComboBox.Items.Clear();
                autoUpgradeModelComboBox.EndUpdate();
            }

            // AoD/陸軍
            if ((Game.Type == GameType.ArsenalOfDemocracy) && (unit.Branch == Branch.Army))
            {
                // 最大物資
                maxSupplyStockLabel.Enabled = true;
                maxSupplyStockTextBox.Enabled = true;
                maxSupplyStockTextBox.Text = DoubleHelper.ToString(model.MaxSupplyStock);
                maxSupplyStockTextBox.ForeColor = model.IsDirty(UnitModelItemId.MaxSupplyStock)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 最大燃料
                maxOilStockLabel.Enabled = true;
                maxOilStockTextBox.Enabled = true;
                maxOilStockTextBox.Text = DoubleHelper.ToString(model.MaxOilStock);
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
            if ((Game.Type == GameType.ArsenalOfDemocracy) &&
                (unit.Branch == Branch.Army) &&
                (unit.Organization == UnitOrganization.Brigade))
            {
                // 砲撃能力
                artilleryBombardmentLabel.Enabled = true;
                artilleryBombardmentTextBox.Enabled = true;
                artilleryBombardmentTextBox.Text = DoubleHelper.ToString(model.ArtilleryBombardment);
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
            if ((Game.Type == GameType.DarkestHour) && (unit.Organization == UnitOrganization.Division))
            {
                // 補充コスト
                reinforceCostLabel.Enabled = true;
                reinforceCostTextBox.Enabled = true;
                reinforceCostTextBox.Text = DoubleHelper.ToString(model.ReinforceCostFactor);
                reinforceCostTextBox.ForeColor = model.IsDirty(UnitModelItemId.ReinforceCostFactor)
                    ? Color.Red
                    : SystemColors.WindowText;
                // 補充時間
                reinforceTimeLabel.Enabled = true;
                reinforceTimeTextBox.Enabled = true;
                reinforceTimeTextBox.Text = DoubleHelper.ToString(model.ReinforceTimeFactor);
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
            if ((Game.Type == GameType.DarkestHour) &&
                (unit.Branch == Branch.Army) &&
                (unit.Organization == UnitOrganization.Division))
            {
                // 燃料切れ補正
                noFuelCombatModLabel.Enabled = true;
                noFuelCombatModTextBox.Enabled = true;
                noFuelCombatModTextBox.Text = DoubleHelper.ToString(model.NoFuelCombatMod);
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
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                // 装備リストを更新する
                UpdateEquipmentList(model);
            }

            // DH1.03以降/陸軍旅団
            if ((Game.Type == GameType.DarkestHour) &&
                (Game.Version >= 103) &&
                (unit.Branch == Branch.Army) &&
                (unit.Organization == UnitOrganization.Brigade))
            {
                // 速度キャップ
                speedCapAllLabel.Enabled = true;
                speedCapAllTextBox.Enabled = true;
                speedCapAllTextBox.Text = DoubleHelper.ToString(model.SpeedCap);
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
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
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

            Image oldImage = modelImagePictureBox.Image;
            modelImagePictureBox.Image = null;
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
            oldImage = modelIconPictureBox.Image;
            modelIconPictureBox.Image = null;
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
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
        ///     ユニットモデルの編集項目の文字列を初期化する
        /// </summary>
        private void InitModelItemText()
        {
            if (Misc.CombatMode)
            {
                defensivenessLabel.Text = Resources.UnitModelDefensiveVulnerablity;
                toughnessLabel.Text = Resources.UnitModelOffensiveVulnerability;
                seaDefenceLabel.Text = Resources.UnitModelNavalVulnerability;
                airDefenceLabel.Text = Resources.UnitModelAirVulnerability;
                surfaceDefenceLabel.Text = Resources.UnitModelGroundVulnerability;
            }
            else
            {
                defensivenessLabel.Text = Resources.UnitModelDefensiveness;
                toughnessLabel.Text = Resources.UnitModelToughness;
                seaDefenceLabel.Text = Resources.UnitModelSeaDefence;
                airDefenceLabel.Text = Resources.UnitModelAirDefence;
                surfaceDefenceLabel.Text = Resources.UnitModelSurfaceDefence;
            }
        }

        /// <summary>
        ///     ユニットモデル名の表示を更新する
        /// </summary>
        private void UpdateModelNameTextBox()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];

            Country country = GetSelectedCountry();

            // ユニットモデル名を更新する
            modelNameTextBox.Text = unit.GetModelName(index, country);

            // ユニットモデル名の表示色を更新する
            UnitModel model = unit.Models[index];
            if (country == Country.None)
            {
                modelNameTextBox.ForeColor = model.IsDirty(UnitModelItemId.Name)
                    ? Color.Red
                    : SystemColors.WindowText;
            }
            else
            {
                if (unit.ExistsModelName(index, country))
                {
                    modelNameTextBox.ForeColor = model.IsDirtyName(country)
                        ? Color.Red
                        : SystemColors.WindowText;
                }
                else
                {
                    modelNameTextBox.ForeColor = model.IsDirty(UnitModelItemId.Name)
                        ? Color.Salmon
                        : Color.Gray;
                }
            }
        }

        /// <summary>
        ///     ユニットモデル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            Country country = GetSelectedCountry();
            string name = unit.GetModelName(index, country);
            if (modelNameTextBox.Text.Equals(name))
            {
                return;
            }

            if ((country != Country.None) && string.IsNullOrEmpty(modelNameTextBox.Text))
            {
                // 国別のモデル名を削除する
                unit.RemoveModelName(index, country);
                // 共通のモデル名を設定する
                modelNameTextBox.Text = unit.GetModelName(index);
                // 文字色を変更する
                modelNameTextBox.ForeColor = model.IsDirty(UnitModelItemId.Name) ? Color.Salmon : Color.Gray;
            }
            else
            {
                // 値を更新する
                unit.SetModelName(index, country, modelNameTextBox.Text);
                // 文字色を変更する
                modelNameTextBox.ForeColor = Color.Red;
            }

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[1].Text = modelNameTextBox.Text;

            // 編集済みフラグを設定する
            model.SetDirtyName(country);
            unit.SetDirty();

            // ユニットモデル名の更新を通知する
            HoI2Editor.OnItemChanged(
                (country == Country.None) ? EditorItemId.CommonModelName : EditorItemId.CountryModelName, this);
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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(defaultOrganisationTextBox.Text, out val))
            {
                defaultOrganisationTextBox.Text = DoubleHelper.ToString(model.DefaultOrganization);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.DefaultOrganization))
            {
                return;
            }

            Log.Info("[Unit] default organization: {0} -> {1} ({2})", DoubleHelper.ToString(model.DefaultOrganization),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.DefaultOrganization = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[7].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.DefaultOrganization);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(moraleTextBox.Text, out val))
            {
                moraleTextBox.Text = DoubleHelper.ToString(model.Morale);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Morale))
            {
                return;
            }

            Log.Info("[Unit] morale: {0} -> {1} ({2})", DoubleHelper.ToString(model.Morale), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.Morale = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[8].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Morale);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(rangeTextBox.Text, out val))
            {
                rangeTextBox.Text = DoubleHelper.ToString(model.Range);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Range))
            {
                return;
            }

            Log.Info("[Unit] range: {0} -> {1} ({2})", DoubleHelper.ToString(model.Range), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.Range = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Range);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(transportWeightTextBox.Text, out val))
            {
                transportWeightTextBox.Text = DoubleHelper.ToString(model.TransportWeight);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.TransportWeight))
            {
                return;
            }

            Log.Info("[Unit] transport weight: {0} -> {1} ({2})", DoubleHelper.ToString(model.TransportWeight),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.TransportWeight = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.TransportWeight);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(transportCapabilityTextBox.Text, out val))
            {
                transportCapabilityTextBox.Text = DoubleHelper.ToString(model.TransportCapability);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.TransportCapability))
            {
                return;
            }

            Log.Info("[Unit] transport capacity: {0} -> {1} ({2})", DoubleHelper.ToString(model.TransportCapability),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.TransportCapability = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.DefaultOrganization);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(suppressionTextBox.Text, out val))
            {
                suppressionTextBox.Text = DoubleHelper.ToString(model.Suppression);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Suppression))
            {
                return;
            }

            Log.Info("[Unit] suppression: {0} -> {1} ({2})", DoubleHelper.ToString(model.Suppression),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Suppression = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Suppression);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(supplyConsumptionTextBox.Text, out val))
            {
                supplyConsumptionTextBox.Text = DoubleHelper.ToString(model.SupplyConsumption);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SupplyConsumption))
            {
                return;
            }

            Log.Info("[Unit] supply consumption: {0} -> {1} ({2})", DoubleHelper.ToString(model.SupplyConsumption),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SupplyConsumption = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[5].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SupplyConsumption);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(fuelConsumptionTextBox.Text, out val))
            {
                fuelConsumptionTextBox.Text = DoubleHelper.ToString(model.FuelConsumption);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.FuelConsumption))
            {
                return;
            }

            Log.Info("[Unit] fuel consumption: {0} -> {1} ({2})", DoubleHelper.ToString(model.FuelConsumption),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.FuelConsumption = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[6].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.FuelConsumption);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(maxSupplyStockTextBox.Text, out val))
            {
                maxSupplyStockTextBox.Text = DoubleHelper.ToString(model.MaxSupplyStock);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.MaxSupplyStock))
            {
                return;
            }

            Log.Info("[Unit] max supply stock: {0} -> {1} ({2})", DoubleHelper.ToString(model.MaxSupplyStock),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.MaxSupplyStock = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.MaxSupplyStock);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(maxOilStockTextBox.Text, out val))
            {
                maxOilStockTextBox.Text = DoubleHelper.ToString(model.MaxOilStock);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.MaxOilStock))
            {
                return;
            }

            Log.Info("[Unit] max oil stock: {0} -> {1} ({2})", DoubleHelper.ToString(model.MaxOilStock),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.MaxOilStock = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.MaxOilStock);
            unit.SetDirtyFile();

            // 文字色を変更する
            maxOilStockTextBox.ForeColor = Color.Red;
        }

        #endregion

        #region ユニットモデルタブ - 生産ステータス

        /// <summary>
        ///     自動改良先リストを更新する
        /// </summary>
        private void UpdateAutoUpgradeClassList()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            Graphics g = Graphics.FromHwnd(autoUpgradeClassComboBox.Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;
            autoUpgradeClassComboBox.BeginUpdate();
            autoUpgradeClassComboBox.Items.Clear();
            if (model.AutoUpgrade)
            {
                int width = autoUpgradeClassComboBox.Width;
                // 現在の自動改良先クラスと兵科がマッチしない場合、ワンショットで候補に登録する
                Unit current = Units.Items[(int) model.UpgradeClass];
                if (current.Branch != unit.Branch)
                {
                    width = Math.Max(width,
                        (int) g.MeasureString(current.ToString(), autoUpgradeClassComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                    autoUpgradeClassComboBox.Items.Add(current);
                }
                foreach (Unit u in Units.UnitTypes
                    .Select(type => Units.Items[(int) type])
                    .Where(u => (u.Branch == unit.Branch) &&
                                (u.Organization == unit.Organization) &&
                                (u.Models.Count > 0)))
                {
                    width = Math.Max(width,
                        (int) g.MeasureString(u.ToString(), autoUpgradeClassComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                    autoUpgradeClassComboBox.Items.Add(u);
                }
                autoUpgradeClassComboBox.DropDownWidth = width;
                autoUpgradeClassComboBox.SelectedItem = Units.Items[(int) model.UpgradeClass];
                autoUpgradeClassComboBox.Enabled = true;
            }
            else
            {
                autoUpgradeClassComboBox.Enabled = false;
                autoUpgradeClassComboBox.SelectedIndex = -1;
                autoUpgradeClassComboBox.ResetText();
            }
            autoUpgradeClassComboBox.EndUpdate();
        }

        /// <summary>
        ///     自動改良先モデルの表示を更新する
        /// </summary>
        private void UpdateAutoUpgradeModelList()
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            Graphics g = Graphics.FromHwnd(autoUpgradeModelComboBox.Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;
            autoUpgradeModelComboBox.BeginUpdate();
            autoUpgradeModelComboBox.Items.Clear();
            if (model.AutoUpgrade)
            {
                Unit upgrade = Units.Items[(int) model.UpgradeClass];
                int width = autoUpgradeModelComboBox.Width;
                for (int i = 0; i < upgrade.Models.Count; i++)
                {
                    string s = upgrade.GetModelName(i);
                    width = Math.Max(width,
                        (int) g.MeasureString(s, autoUpgradeModelComboBox.Font).Width +
                        SystemInformation.VerticalScrollBarWidth + margin);
                    autoUpgradeModelComboBox.Items.Add(s);
                }
                autoUpgradeModelComboBox.DropDownWidth = width;
                if ((model.UpgradeModel >= 0) && (model.UpgradeModel < upgrade.Models.Count))
                {
                    autoUpgradeModelComboBox.SelectedIndex = model.UpgradeModel;
                }
                else
                {
                    autoUpgradeModelComboBox.SelectedIndex = -1;
                    autoUpgradeModelComboBox.Text = DoubleHelper.ToString(model.UpgradeModel);
                }
                autoUpgradeModelComboBox.Enabled = true;
            }
            else
            {
                autoUpgradeModelComboBox.Enabled = false;
                autoUpgradeModelComboBox.SelectedIndex = -1;
                autoUpgradeModelComboBox.ResetText();
            }
            autoUpgradeModelComboBox.ForeColor = model.IsDirty(UnitModelItemId.UpgradeModel)
                ? Color.Red
                : SystemColors.WindowText;
            autoUpgradeModelComboBox.EndUpdate();
        }

        /// <summary>
        ///     必要ICテキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCostTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(costTextBox.Text, out val))
            {
                costTextBox.Text = DoubleHelper.ToString(model.Cost);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Cost))
            {
                return;
            }

            Log.Info("[Unit] cost: {0} -> {1} ({2})", DoubleHelper.ToString(model.Cost), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.Cost = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[2].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Cost);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(buildTimeTextBox.Text, out val))
            {
                buildTimeTextBox.Text = DoubleHelper.ToString(model.BuildTime);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.BuildTime))
            {
                return;
            }

            Log.Info("[Unit] build time: {0} -> {1} ({2})", DoubleHelper.ToString(model.BuildTime),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.BuildTime = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[3].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.BuildTime);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(manPowerTextBox.Text, out val))
            {
                manPowerTextBox.Text = DoubleHelper.ToString(model.ManPower);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ManPower))
            {
                return;
            }

            Log.Info("[Unit] manpower: {0} -> {1} ({2})", DoubleHelper.ToString(model.ManPower),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ManPower = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[4].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ManPower);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(upgradeCostFactorTextBox.Text, out val))
            {
                upgradeCostFactorTextBox.Text = DoubleHelper.ToString(model.UpgradeCostFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.UpgradeCostFactor))
            {
                return;
            }

            Log.Info("[Unit] upgrade cost factor: {0} -> {1} ({2})", DoubleHelper.ToString(model.UpgradeCostFactor),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.UpgradeCostFactor = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeCostFactor);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(upgradeTimeFactorTextBox.Text, out val))
            {
                upgradeTimeFactorTextBox.Text = DoubleHelper.ToString(model.UpgradeTimeFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.UpgradeTimeFactor))
            {
                return;
            }

            Log.Info("[Unit] upgrade time factor: {0} -> {1} ({2})", DoubleHelper.ToString(model.UpgradeTimeFactor),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.UpgradeTimeFactor = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeTimeFactor);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(reinforceCostTextBox.Text, out val))
            {
                reinforceCostTextBox.Text = DoubleHelper.ToString(model.ReinforceCostFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ReinforceCostFactor))
            {
                return;
            }

            Log.Info("[Unit] reinforce cost: {0} -> {1} ({2})", DoubleHelper.ToString(model.ReinforceCostFactor),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ReinforceCostFactor = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ReinforceCostFactor);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(reinforceTimeTextBox.Text, out val))
            {
                reinforceTimeTextBox.Text = DoubleHelper.ToString(model.ReinforceTimeFactor);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ReinforceTimeFactor))
            {
                return;
            }

            Log.Info("[Unit] reinforce time: {0} -> {1} ({2})", DoubleHelper.ToString(model.ReinforceTimeFactor),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ReinforceTimeFactor = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ReinforceTimeFactor);
            unit.SetDirtyFile();

            // 文字色を変更する
            reinforceTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     2段階改良チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpgradeTimeBoostCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットクラスが海軍師団ならば何もしない
            if ((unit.Branch == Branch.Navy) && (unit.Organization == UnitOrganization.Division))
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            if (upgradeTimeBoostCheckBox.Checked == model.UpgradeTimeBoost)
            {
                return;
            }

            Log.Info("[Unit] upgrade time boost: {0} -> {1} ({2})", BoolHelper.ToYesNo(model.UpgradeTimeBoost),
                BoolHelper.ToYesNo(upgradeTimeBoostCheckBox.Checked), unit.GetModelName(index));

            // 値を更新する
            model.UpgradeTimeBoost = upgradeTimeBoostCheckBox.Checked;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeTimeBoost);
            unit.SetDirtyFile();

            // 文字色を変更する
            upgradeTimeBoostCheckBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     自動改良チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットクラスが海軍師団ならば何もしない
            if ((unit.Branch == Branch.Navy) && (unit.Organization == UnitOrganization.Division))
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            if (autoUpgradeCheckBox.Checked == model.AutoUpgrade)
            {
                return;
            }

            Log.Info("[Unit] auto upgrade: {0} -> {1} ({2})", BoolHelper.ToYesNo(model.AutoUpgrade),
                BoolHelper.ToYesNo(autoUpgradeCheckBox.Checked), unit.GetModelName(index));

            // 値を更新する
            model.AutoUpgrade = autoUpgradeCheckBox.Checked;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.AutoUpgrade);
            unit.SetDirtyFile();

            // 文字色を変更する
            autoUpgradeCheckBox.ForeColor = Color.Red;

            // 自動改良先リストを更新する
            UpdateAutoUpgradeClassList();
            UpdateAutoUpgradeModelList();
        }

        /// <summary>
        ///     自動改良先クラスコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeClassComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            var u = autoUpgradeClassComboBox.Items[e.Index] as Unit;
            if (u != null)
            {
                Brush brush;
                if ((u.Type == model.UpgradeClass) && model.IsDirty(UnitModelItemId.UpgradeClass))
                {
                    brush = new SolidBrush(Color.Red);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.WindowText);
                }
                string s = autoUpgradeClassComboBox.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     自動改良先モデルコンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeModelComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.Index == model.UpgradeModel) && model.IsDirty(UnitModelItemId.UpgradeModel))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = autoUpgradeModelComboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     自動改良先クラスコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeClassComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中の項目がなければ何もしない
            if (autoUpgradeClassComboBox.SelectedIndex < 0)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            var upgrade = autoUpgradeClassComboBox.SelectedItem as Unit;
            if (upgrade == null)
            {
                return;
            }
            if (upgrade.Type == model.UpgradeClass)
            {
                return;
            }

            Log.Info("[Unit] auto upgrade class: {0} -> {1} ({2})", Units.Items[(int) model.UpgradeClass],
                Units.Items[(int) upgrade.Type], unit.GetModelName(index));

            // 値を更新する
            Unit old = Units.Items[(int) model.UpgradeClass];
            model.UpgradeClass = upgrade.Type;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeClass);
            unit.SetDirtyFile();

            if (old.Branch != unit.Branch)
            {
                // 自動改良先クラスと兵科がマッチしていなかった場合は、項目を更新する
                UpdateAutoUpgradeClassList();
            }
            else
            {
                // 自動改良先クラスコンボボックスの項目色を変更するために描画更新する
                autoUpgradeClassComboBox.Refresh();
            }

            // 自動改良先モデルコンボボックスの表示を更新する
            UpdateAutoUpgradeModelList();
        }

        /// <summary>
        ///     自動改良先モデルコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeModelComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中の項目がなければ何もしない
            if (autoUpgradeModelComboBox.SelectedIndex < 0)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 値に変化がなければ何もしない
            if (autoUpgradeModelComboBox.SelectedIndex == model.UpgradeModel)
            {
                return;
            }

            Log.Info("[Unit] auto upgrade model: {0} -> {1} ({2})",
                Units.Items[(int) model.UpgradeClass].GetModelName(model.UpgradeModel),
                Units.Items[(int) model.UpgradeClass].GetModelName(autoUpgradeModelComboBox.SelectedIndex),
                unit.GetModelName(index));

            // 値を更新する
            model.UpgradeModel = autoUpgradeModelComboBox.SelectedIndex;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeModel);
            unit.SetDirtyFile();

            // 文字色を変更する
            autoUpgradeModelComboBox.ForeColor = Color.Red;

            // 自動改良先モデルコンボボックスの項目色を変更するために描画更新する
            autoUpgradeModelComboBox.Refresh();
        }

        /// <summary>
        ///     自動改良先クラスコンボボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoUpgradeModelComboBoxValidated(object sender, EventArgs e)
        {
            // 選択中の項目があれば何もしない
            if (autoUpgradeModelComboBox.SelectedIndex >= 0)
            {
                return;
            }

            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            Unit upgrade = Units.Items[(int) model.UpgradeClass];

            // 変更後の文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(autoUpgradeModelComboBox.Text, out val))
            {
                if ((model.UpgradeModel >= 0) && (model.UpgradeModel < upgrade.Models.Count))
                {
                    autoUpgradeModelComboBox.SelectedIndex = model.UpgradeModel;
                }
                else
                {
                    autoUpgradeModelComboBox.SelectedIndex = -1;
                    autoUpgradeModelComboBox.Text = DoubleHelper.ToString(model.UpgradeModel);
                }
                return;
            }

            // 値に変化がなければ更新しない
            if (val == model.UpgradeModel)
            {
                // 選択項目が存在する範囲ならば数字を選択項目へ戻す
                if ((val >= 0) && (val < upgrade.Models.Count))
                {
                    autoUpgradeModelComboBox.SelectedIndex = model.UpgradeModel;
                }
                return;
            }

            Log.Info("[Unit] auto upgrade model: {0} -> {1} ({2})",
                Units.Items[(int) model.UpgradeClass].GetModelName(model.UpgradeModel),
                Units.Items[(int) model.UpgradeClass].GetModelName(val), unit.GetModelName(index));

            // 値を更新する
            model.UpgradeModel = val;

            // 選択項目が存在する範囲ならば数字を選択項目へ戻す
            {
                if ((val >= 0) && (val < upgrade.Models.Count))
                {
                    autoUpgradeModelComboBox.SelectedIndex = model.UpgradeModel;
                }
            }

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.UpgradeModel);
            unit.SetDirtyFile();

            // 文字色を変更する
            autoUpgradeModelComboBox.ForeColor = Color.Red;

            // 自動改良先モデルコンボボックスの項目色を変更するために描画更新する
            autoUpgradeModelComboBox.Refresh();
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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(maxSpeedTextBox.Text, out val))
            {
                maxSpeedTextBox.Text = DoubleHelper.ToString(model.MaxSpeed);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.MaxSpeed))
            {
                return;
            }

            Log.Info("[Unit] max speed: {0} -> {1} ({2})", DoubleHelper.ToString(model.MaxSpeed),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.MaxSpeed = val;

            // ユニットモデルリストの項目を更新する
            modelListView.Items[index].SubItems[9].Text = DoubleHelper.ToString(val);

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.MaxSpeed);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(speedCapAllTextBox.Text, out val))
            {
                speedCapAllTextBox.Text = DoubleHelper.ToString(model.SpeedCap);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SpeedCap))
            {
                return;
            }

            Log.Info("[Unit] speed cap: {0} -> {1} ({2})", DoubleHelper.ToString(model.SpeedCap),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SpeedCap = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SpeedCap);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(speedCapArtTextBox.Text, out val))
            {
                speedCapArtTextBox.Text = DoubleHelper.ToString(model.SpeedCapArt);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SpeedCapArt))
            {
                return;
            }

            Log.Info("[Unit] speed cap art: {0} -> {1} ({2})", DoubleHelper.ToString(model.SpeedCapArt),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SpeedCapArt = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SpeedCapArt);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(speedCapEngTextBox.Text, out val))
            {
                speedCapEngTextBox.Text = DoubleHelper.ToString(model.SpeedCapEng);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SpeedCapEng))
            {
                return;
            }

            Log.Info("[Unit] speed cap eng: {0} -> {1} ({2})", DoubleHelper.ToString(model.SpeedCapEng),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SpeedCapEng = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SpeedCapEng);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(speedCapAtTextBox.Text, out val))
            {
                speedCapAtTextBox.Text = DoubleHelper.ToString(model.SpeedCapAt);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SpeedCapAt))
            {
                return;
            }

            Log.Info("[Unit] speed cap at: {0} -> {1} ({2})", DoubleHelper.ToString(model.SpeedCapAt),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SpeedCapAt = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SpeedCapAt);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(speedCapAaTextBox.Text, out val))
            {
                speedCapAaTextBox.Text = DoubleHelper.ToString(model.SpeedCapAa);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SpeedCapAa))
            {
                return;
            }

            Log.Info("[Unit] speed cap aa: {0} -> {1} ({2})", DoubleHelper.ToString(model.SpeedCapAa),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SpeedCapAa = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SpeedCapAa);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(defensivenessTextBox.Text, out val))
            {
                defensivenessTextBox.Text = DoubleHelper.ToString(model.Defensiveness);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Defensiveness))
            {
                return;
            }

            Log.Info("[Unit] defensiveness: {0} -> {1} ({2})", DoubleHelper.ToString(model.Defensiveness),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Defensiveness = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Defensiveness);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(seaDefenceTextBox.Text, out val))
            {
                seaDefenceTextBox.Text = DoubleHelper.ToString(model.SeaDefense);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SeaDefense))
            {
                return;
            }

            Log.Info("[Unit] sea defence: {0} -> {1} ({2})", DoubleHelper.ToString(model.SeaDefense),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SeaDefense = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SeaDefense);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(airDefenceTextBox.Text, out val))
            {
                airDefenceTextBox.Text = DoubleHelper.ToString(model.AirDefence);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.AirDefence))
            {
                return;
            }

            Log.Info("[Unit] air defence: {0} -> {1} ({2})", DoubleHelper.ToString(model.AirDefence),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.AirDefence = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.AirDefense);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(surfaceDefenceTextBox.Text, out val))
            {
                surfaceDefenceTextBox.Text = DoubleHelper.ToString(model.SurfaceDefence);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SurfaceDefence))
            {
                return;
            }

            Log.Info("[Unit] surface defence: {0} -> {1} ({2})", DoubleHelper.ToString(model.SurfaceDefence),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SurfaceDefence = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SurfaceDefense);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(toughnessTextBox.Text, out val))
            {
                toughnessTextBox.Text = DoubleHelper.ToString(model.Toughness);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Toughness))
            {
                return;
            }

            Log.Info("[Unit] toughness: {0} -> {1} ({2})", DoubleHelper.ToString(model.Toughness),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Toughness = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Toughness);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(softnessTextBox.Text, out val))
            {
                softnessTextBox.Text = DoubleHelper.ToString(model.Softness);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Softness))
            {
                return;
            }

            Log.Info("[Unit] softness: {0} -> {1} ({2})", DoubleHelper.ToString(model.Softness),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Softness = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Softness);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(softAttackTextBox.Text, out val))
            {
                softAttackTextBox.Text = DoubleHelper.ToString(model.SoftAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SoftAttack))
            {
                return;
            }

            Log.Info("[Unit] soft attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.SoftAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SoftAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SoftAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(hardAttackTextBox.Text, out val))
            {
                hardAttackTextBox.Text = DoubleHelper.ToString(model.HardAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.HardAttack))
            {
                return;
            }

            Log.Info("[Unit] hard attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.HardAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.HardAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.HardAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(seaAttackTextBox.Text, out val))
            {
                seaAttackTextBox.Text = DoubleHelper.ToString(model.SeaAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SeaAttack))
            {
                return;
            }

            Log.Info("[Unit] sea attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.SeaAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SeaAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SeaAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(subAttackTextBox.Text, out val))
            {
                subAttackTextBox.Text = DoubleHelper.ToString(model.SubAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SubAttack))
            {
                return;
            }

            Log.Info("[Unit] sub attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.SubAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.SubAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SubAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(convoyAttackTextBox.Text, out val))
            {
                convoyAttackTextBox.Text = DoubleHelper.ToString(model.ConvoyAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ConvoyAttack))
            {
                return;
            }

            Log.Info("[Unit] convoy attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.ConvoyAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ConvoyAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ConvoyAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (
                !DoubleHelper.TryParse(shoreBombardmentTextBox.Text, out val))
            {
                shoreBombardmentTextBox.Text = DoubleHelper.ToString(model.ShoreBombardment);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ShoreBombardment))
            {
                return;
            }

            Log.Info("[Unit] shore bombardment: {0} -> {1} ({2})", DoubleHelper.ToString(model.ShoreBombardment),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ShoreBombardment = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ShoreBombardment);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(airAttackTextBox.Text, out val))
            {
                airAttackTextBox.Text = DoubleHelper.ToString(model.AirAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.AirAttack))
            {
                return;
            }

            Log.Info("[Unit] air attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.AirAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.AirAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.AirAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(navalAttackTextBox.Text, out val))
            {
                navalAttackTextBox.Text = DoubleHelper.ToString(model.NavalAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.NavalAttack))
            {
                return;
            }

            Log.Info("[Unit] naval attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.NavalAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.NavalAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.NavalAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(strategicAttackTextBox.Text, out val))
            {
                strategicAttackTextBox.Text = DoubleHelper.ToString(model.StrategicAttack);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.StrategicAttack))
            {
                return;
            }

            Log.Info("[Unit] strategic attack: {0} -> {1} ({2})", DoubleHelper.ToString(model.StrategicAttack),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.StrategicAttack = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.StrategicAttack);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(artilleryBombardmentTextBox.Text, out val))
            {
                artilleryBombardmentTextBox.Text = DoubleHelper.ToString(model.ArtilleryBombardment);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.ArtilleryBombardment))
            {
                return;
            }

            Log.Info("[Unit] artillery bombardment: {0} -> {1} ({2})", DoubleHelper.ToString(model.ArtilleryBombardment),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.ArtilleryBombardment = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.ArtilleryBombardment);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(distanceTextBox.Text, out val))
            {
                distanceTextBox.Text = DoubleHelper.ToString(model.Distance);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Distance))
            {
                return;
            }

            Log.Info("[Unit] distance: {0} -> {1} ({2})", DoubleHelper.ToString(model.Distance),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Distance = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Distance);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(visibilityTextBox.Text, out val))
            {
                visibilityTextBox.Text = DoubleHelper.ToString(model.Visibility);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.Visibility))
            {
                return;
            }

            Log.Info("[Unit] visibility: {0} -> {1} ({2})", DoubleHelper.ToString(model.Visibility),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.Visibility = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.Visibility);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(surfaceDetectionCapabilityTextBox.Text, out val))
            {
                surfaceDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.SurfaceDetectionCapability);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SurfaceDetectionCapability))
            {
                return;
            }

            Log.Info("[Unit] surface detection capability: {0} -> {1} ({2})",
                DoubleHelper.ToString(model.SurfaceDetectionCapability), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.SurfaceDetectionCapability = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SurfaceDetectionCapability);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(subDetectionCapabilityTextBox.Text, out val))
            {
                subDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.SubDetectionCapability);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.SubDetectionCapability))
            {
                return;
            }

            Log.Info("[Unit] sub detection capability: {0} -> {1} ({2})",
                DoubleHelper.ToString(model.SubDetectionCapability), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.SubDetectionCapability = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.SubDetectionCapability);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(airDetectionCapabilityTextBox.Text, out val))
            {
                airDetectionCapabilityTextBox.Text = DoubleHelper.ToString(model.AirDetectionCapability);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.AirDetectionCapability))
            {
                return;
            }

            Log.Info("[Unit] air detection capablity: {0} -> {1} ({2})",
                DoubleHelper.ToString(model.AirDetectionCapability), DoubleHelper.ToString(val),
                unit.GetModelName(index));

            // 値を更新する
            model.AirDetectionCapability = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.AirDetectionCapability);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[index];

            // 変更後の文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(noFuelCombatModTextBox.Text, out val))
            {
                noFuelCombatModTextBox.Text = DoubleHelper.ToString(model.NoFuelCombatMod);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, model.NoFuelCombatMod))
            {
                return;
            }

            Log.Info("[Unit] no fuel combat mod: {0} -> {1} ({2})", DoubleHelper.ToString(model.NoFuelCombatMod),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            model.NoFuelCombatMod = val;

            // 編集済みフラグを設定する
            model.SetDirty(UnitModelItemId.NoFuelCombatMod);
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            quantityTextBox.Text = DoubleHelper.ToString(equipment.Quantity);

            // 編集項目の色を更新する
            quantityTextBox.ForeColor = equipment.IsDirty(UnitEquipmentItemId.Quantity)
                ? Color.Red
                : SystemColors.WindowText;

            // 編集項目を有効化する
            EnableEquipmentItems();
        }

        /// <summary>
        ///     装備リストビューの列の幅変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEquipmentListViewColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if ((e.ColumnIndex >= 0) && (e.ColumnIndex < EquipmentListColumnCount))
            {
                HoI2Editor.Settings.UnitEditor.EquipmentListColumnWidth[e.ColumnIndex] =
                    equipmentListView.Columns[e.ColumnIndex].Width;
            }
        }

        /// <summary>
        ///     資源コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中のユニットクラスがなければ何もしない
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            Log.Info("[Unit] equipment resource: {0} -> {1} ({2})",
                Config.GetText(Units.EquipmentNames[(int) equipment.Resource]),
                Config.GetText(Units.EquipmentNames[(int) type]), unit.GetModelName(index));

            // 値を更新する
            equipment.Resource = type;

            // 装備リストビューの項目を更新する
            equipmentListView.Items[index].Text = Config.GetText(Units.EquipmentNames[(int) type]);

            // 編集済みフラグを設定する
            equipment.SetDirty(UnitEquipmentItemId.Resource);
            model.SetDirty();
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            if (!DoubleHelper.TryParse(quantityTextBox.Text, out val))
            {
                quantityTextBox.Text = DoubleHelper.ToString(equipment.Quantity);
                return;
            }

            // 値に変化がなければ何もしない
            if (DoubleHelper.IsEqual(val, equipment.Quantity))
            {
                return;
            }

            Log.Info("[Unit] equipment quantity: {0} -> {1} ({2})", DoubleHelper.ToString(equipment.Quantity),
                DoubleHelper.ToString(val), unit.GetModelName(index));

            // 値を更新する
            equipment.Quantity = val;

            // 装備リストビューの項目を更新する
            equipmentListView.Items[index].SubItems[1].Text = quantityTextBox.Text;

            // 編集済みフラグを設定する
            equipment.SetDirty(UnitEquipmentItemId.Quantity);
            model.SetDirty();
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

            // 選択中のユニットモデルがなければ何もしない
            if (modelListView.SelectedIndices.Count == 0)
            {
                return;
            }
            int i = modelListView.SelectedIndices[0];
            UnitModel model = unit.Models[i];

            Log.Info("[Unit] Added new equipment: ({0})", unit.GetModelName(i));

            // 装備リストに項目を追加する
            var equipment = new UnitEquipment();
            model.Equipments.Add(equipment);

            // 編集済みフラグを設定する
            equipment.SetDirtyAll();
            model.SetDirty();
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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

            Log.Info("[Unit] Removed equipment: {0} ({1})",
                Config.GetText(Units.EquipmentNames[(int) model.Equipments[index].Resource]), unit.GetModelName(i));

            // 装備リストから項目を削除する
            model.Equipments.RemoveAt(index);

            // 編集済みフラグを設定する
            model.SetDirty();
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            unit.SetDirtyFile();

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
            var unit = classListBox.SelectedItem as Unit;
            if (unit == null)
            {
                return;
            }

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
            unit.SetDirtyFile();

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
            var item = new ListViewItem { Text = Config.GetText(Units.EquipmentNames[(int) equipment.Resource]) };
            item.SubItems.Add(DoubleHelper.ToString(equipment.Quantity));

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