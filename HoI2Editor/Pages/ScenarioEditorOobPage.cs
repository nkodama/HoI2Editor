using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタの初期部隊タブ
    /// </summary>
    [ToolboxItem(false)]
    internal partial class ScenarioEditorOobPage : UserControl
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private readonly ScenarioEditorController _controller;

        /// <summary>
        ///     シナリオエディタのフォーム
        /// </summary>
        private readonly ScenarioEditorForm _form;

        /// <summary>
        ///     ユニットツリーコントローラ
        /// </summary>
        private readonly UnitTreeController _unitTreeController;

        /// <summary>
        ///     選択中の国家
        /// </summary>
        private Country _selectedCountry;

        /// <summary>
        ///     最終のユニットの兵科
        /// </summary>
        private Branch _lastUnitBranch;

        /// <summary>
        ///     最終の師団の兵科
        /// </summary>
        private Branch _lastDivisionBranch;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタコントローラ</param>
        /// <param name="form">シナリオエディタのフォーム</param>
        internal ScenarioEditorOobPage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // ユニットツリーコントローラを初期化する
            _unitTreeController = new UnitTreeController(unitTreeView);
            _controller.AttachUnitTree(_unitTreeController);

            // 編集項目を初期化する
            InitUnitTree();
            InitOobUnitItems();
            InitOobDivisionItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // プロヴィンスリストを初期化する
            _controller.InitProvinceList();

            // ユニット種類リストを初期化する
            _controller.InitUnitTypeList();

            // ユニットツリーコントローラの選択国を解除する
            _unitTreeController.Country = Country.None;

            // 国家リストボックスを更新する
            UpdateCountryListBox();

            // 国家リストボックスを有効化する
            EnableOobCountryListBox();

            // ユニットツリーを有効化する
            EnableUnitTree();

            // 編集項目を無効化する
            DisableOobUnitItems();
            DisableOobDivisionItems();

            // 編集項目をクリアする
            ClearOobUnitItems();
            ClearOobDivisionItems();
        }

        #endregion

        #region 国家リスト

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableOobCountryListBox()
        {
            countryListBox.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスを更新する
        /// </summary>
        private void UpdateCountryListBox()
        {
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                countryListBox.Items.Add(Scenarios.GetCountryTagName(country));
            }
            countryListBox.EndUpdate();
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                CountrySettings settings = Scenarios.GetCountrySettings(Countries.Tags[e.Index]);
                brush = new SolidBrush(settings != null
                    ? (settings.IsDirty() ? Color.Red : countryListBox.ForeColor)
                    : Color.LightGray);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = countryListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (countryListBox.SelectedIndex < 0)
            {
                // 編集項目を無効化する
                DisableOobUnitItems();
                DisableOobDivisionItems();

                // 編集項目をクリアする
                ClearOobUnitItems();
                ClearOobDivisionItems();
                return;
            }

            _selectedCountry = Countries.Tags[countryListBox.SelectedIndex];

            // 指揮官リストを初期化する
            ScenarioHeader header = Scenarios.Data.Header;
            int year = header.StartDate?.Year ?? header.StartYear;
            _controller.UpdateLeaderList(_selectedCountry, year);

            // ユニットツリーを更新する
            _unitTreeController.Country = _selectedCountry;
        }

        #endregion

        #region ユニットツリー

        /// <summary>
        ///     ユニットツリーを初期化する
        /// </summary>
        private void InitUnitTree()
        {
            _unitTreeController.AfterSelect += OnUnitTreeAfterSelect;
        }

        /// <summary>
        ///     ユニットツリーを有効化する
        /// </summary>
        private void EnableUnitTree()
        {
            unitTreeView.Enabled = true;

            // ツリー操作ボタンを無効化する
            oobAddUnitButton.Enabled = false;
            oobAddDivisionButton.Enabled = false;
            oobCloneButton.Enabled = false;
            oobRemoveButton.Enabled = false;
            oobTopButton.Enabled = false;
            oobUpButton.Enabled = false;
            oobDownButton.Enabled = false;
            oobBottomButton.Enabled = false;
        }

        /// <summary>
        ///     ユニットツリーのノード選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitTreeAfterSelect(object sender, UnitTreeController.UnitTreeViewEventArgs e)
        {
            // ボタンの状態を更新する
            oobAddUnitButton.Enabled = e.CanAddUnit;
            oobAddDivisionButton.Enabled = e.CanAddDivision;
            bool selected = (e.Unit != null) || (e.Division != null);
            oobCloneButton.Enabled = selected;
            oobRemoveButton.Enabled = selected;
            TreeNode parent = e.Node.Parent;
            if (selected && (parent != null))
            {
                int index = parent.Nodes.IndexOf(e.Node);
                int bottom = parent.Nodes.Count - 1;
                oobTopButton.Enabled = index > 0;
                oobUpButton.Enabled = index > 0;
                oobDownButton.Enabled = index < bottom;
                oobBottomButton.Enabled = index < bottom;
            }
            else
            {
                oobTopButton.Enabled = false;
                oobUpButton.Enabled = false;
                oobDownButton.Enabled = false;
                oobBottomButton.Enabled = false;
            }

            if (e.Unit != null)
            {
                UpdateOobUnitItems(e.Unit);
                EnableOobUnitItems();
            }
            else
            {
                DisableOobUnitItems();
                ClearOobUnitItems();
            }

            if (e.Division != null)
            {
                UpdateOobDivisionItems(e.Division);
                EnableOobDivisionItems();
            }
            else
            {
                DisableOobDivisionItems();
                ClearOobDivisionItems();
            }
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobTopButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveTop();
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobUpButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveUp();
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobDownButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveDown();
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobBottomButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.MoveBottom();
        }

        /// <summary>
        ///     新規ユニットボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobAddUnitButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.AddUnit();
        }

        /// <summary>
        ///     新規師団ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobAddDivisionButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.AddDivision();
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobCloneButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.Clone();
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOobRemoveButtonClick(object sender, EventArgs e)
        {
            _unitTreeController.Remove();
        }

        #endregion

        #region ユニット情報

        /// <summary>
        ///     ユニット情報の編集項目を初期化する
        /// </summary>
        private void InitOobUnitItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.UnitType, unitTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitId, unitIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitName, unitNameTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitLocationId, locationTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitLocation, locationComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitBaseId, baseTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitBase, baseComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitLeaderId, leaderTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitLeader, leaderComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitMorale, unitMoraleTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.UnitDigIn, digInTextBox);

            unitTypeTextBox.Tag = ScenarioEditorItemId.UnitType;
            unitIdTextBox.Tag = ScenarioEditorItemId.UnitId;
            unitNameTextBox.Tag = ScenarioEditorItemId.UnitName;
            locationTextBox.Tag = ScenarioEditorItemId.UnitLocationId;
            locationComboBox.Tag = ScenarioEditorItemId.UnitLocation;
            baseTextBox.Tag = ScenarioEditorItemId.UnitBaseId;
            baseComboBox.Tag = ScenarioEditorItemId.UnitBase;
            leaderTextBox.Tag = ScenarioEditorItemId.UnitLeaderId;
            leaderComboBox.Tag = ScenarioEditorItemId.UnitLeader;
            unitMoraleTextBox.Tag = ScenarioEditorItemId.UnitMorale;
            digInTextBox.Tag = ScenarioEditorItemId.UnitDigIn;
        }

        /// <summary>
        ///     ユニット情報の編集項目を更新する
        /// </summary>
        /// <param name="unit">ユニット</param>
        private void UpdateOobUnitItems(Unit unit)
        {
            _controller.UpdateItemValue(unitTypeTextBox, unit);
            _controller.UpdateItemValue(unitIdTextBox, unit);
            _controller.UpdateItemValue(unitNameTextBox, unit);
            _controller.UpdateItemValue(locationTextBox, unit);
            _controller.UpdateItemValue(baseTextBox, unit);
            _controller.UpdateItemValue(leaderTextBox, unit);
            _controller.UpdateItemValue(unitMoraleTextBox, unit);
            _controller.UpdateItemValue(digInTextBox, unit);

            _controller.UpdateItemColor(unitTypeTextBox, unit);
            _controller.UpdateItemColor(unitIdTextBox, unit);
            _controller.UpdateItemColor(unitNameTextBox, unit);
            _controller.UpdateItemColor(locationTextBox, unit);
            _controller.UpdateItemColor(baseTextBox, unit);
            _controller.UpdateItemColor(leaderTextBox, unit);
            _controller.UpdateItemColor(unitMoraleTextBox, unit);
            _controller.UpdateItemColor(digInTextBox, unit);

            // ユニットの兵科が変更された場合
            if (unit.Branch != _lastUnitBranch)
            {
                _lastUnitBranch = unit.Branch;

                // リストの選択肢を変更する
                _controller.UpdateListItems(locationComboBox, unit);
                _controller.UpdateListItems(baseComboBox, unit);
                _controller.UpdateListItems(leaderComboBox, unit);

                // 兵科による編集制限
                switch (unit.Branch)
                {
                    case Branch.Army:
                        baseLabel.Enabled = false;
                        baseTextBox.Enabled = false;
                        baseComboBox.Enabled = false;
                        digInLabel.Enabled = true;
                        digInTextBox.Enabled = true;
                        break;

                    case Branch.Navy:
                    case Branch.Airforce:
                        baseLabel.Enabled = true;
                        baseTextBox.Enabled = true;
                        baseComboBox.Enabled = true;
                        digInLabel.Enabled = false;
                        digInTextBox.Enabled = false;
                        break;
                }
            }

            _controller.UpdateItemValue(locationComboBox, unit);
            _controller.UpdateItemValue(baseComboBox, unit);
            _controller.UpdateItemValue(leaderComboBox, unit);

            _controller.UpdateItemColor(locationComboBox, unit);
            _controller.UpdateItemColor(baseComboBox, unit);
            _controller.UpdateItemColor(leaderComboBox, unit);
        }

        /// <summary>
        ///     ユニット情報の編集項目をクリアする
        /// </summary>
        private void ClearOobUnitItems()
        {
            unitTypeTextBox.Text = "";
            unitIdTextBox.Text = "";
            unitNameTextBox.Text = "";
            locationTextBox.Text = "";
            locationComboBox.SelectedIndex = -1;
            baseTextBox.Text = "";
            baseComboBox.SelectedIndex = -1;
            leaderTextBox.Text = "";
            leaderComboBox.SelectedIndex = -1;
            unitMoraleTextBox.Text = "";
            digInTextBox.Text = "";
        }

        /// <summary>
        ///     ユニット情報の編集項目を有効化する
        /// </summary>
        private void EnableOobUnitItems()
        {
            unitGroupBox.Enabled = true;
        }

        /// <summary>
        ///     ユニット情報の編集項目を無効化する
        /// </summary>
        private void DisableOobUnitItems()
        {
            unitGroupBox.Enabled = false;
        }

        #endregion

        #region 師団情報

        /// <summary>
        ///     師団情報の編集項目を初期化する
        /// </summary>
        private void InitOobDivisionItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.DivisionType, divisionTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionId, divisionIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionName, divisionNameTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionUnitType, unitTypeComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionModel, unitModelComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType1, brigadeTypeComboBox1);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType2, brigadeTypeComboBox2);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType3, brigadeTypeComboBox3);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType4, brigadeTypeComboBox4);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeType5, brigadeTypeComboBox5);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel1, brigadeModelComboBox1);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel2, brigadeModelComboBox2);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel3, brigadeModelComboBox3);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel4, brigadeModelComboBox4);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionBrigadeModel5, brigadeModelComboBox5);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionStrength, strengthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionMaxStrength, maxStrengthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionOrganisation, organisationTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionMaxOrganisation, maxOrganisationTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionMorale, divisionMoraleTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionExperience, experienceTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionLocked, lockedCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DivisionDormant, dormantCheckBox);

            divisionTypeTextBox.Tag = ScenarioEditorItemId.DivisionType;
            divisionIdTextBox.Tag = ScenarioEditorItemId.DivisionId;
            divisionNameTextBox.Tag = ScenarioEditorItemId.DivisionName;
            unitTypeComboBox.Tag = ScenarioEditorItemId.DivisionUnitType;
            unitModelComboBox.Tag = ScenarioEditorItemId.DivisionModel;
            brigadeTypeComboBox1.Tag = ScenarioEditorItemId.DivisionBrigadeType1;
            brigadeTypeComboBox2.Tag = ScenarioEditorItemId.DivisionBrigadeType2;
            brigadeTypeComboBox3.Tag = ScenarioEditorItemId.DivisionBrigadeType3;
            brigadeTypeComboBox4.Tag = ScenarioEditorItemId.DivisionBrigadeType4;
            brigadeTypeComboBox5.Tag = ScenarioEditorItemId.DivisionBrigadeType5;
            brigadeModelComboBox1.Tag = ScenarioEditorItemId.DivisionBrigadeModel1;
            brigadeModelComboBox2.Tag = ScenarioEditorItemId.DivisionBrigadeModel2;
            brigadeModelComboBox3.Tag = ScenarioEditorItemId.DivisionBrigadeModel3;
            brigadeModelComboBox4.Tag = ScenarioEditorItemId.DivisionBrigadeModel4;
            brigadeModelComboBox5.Tag = ScenarioEditorItemId.DivisionBrigadeModel5;
            strengthTextBox.Tag = ScenarioEditorItemId.DivisionStrength;
            maxStrengthTextBox.Tag = ScenarioEditorItemId.DivisionMaxStrength;
            organisationTextBox.Tag = ScenarioEditorItemId.DivisionOrganisation;
            maxOrganisationTextBox.Tag = ScenarioEditorItemId.DivisionMaxOrganisation;
            divisionMoraleTextBox.Tag = ScenarioEditorItemId.DivisionMorale;
            experienceTextBox.Tag = ScenarioEditorItemId.DivisionExperience;
            lockedCheckBox.Tag = ScenarioEditorItemId.DivisionLocked;
            dormantCheckBox.Tag = ScenarioEditorItemId.DivisionDormant;
        }

        /// <summary>
        ///     師団情報の編集項目を更新する
        /// </summary>
        /// <param name="division">師団</param>
        private void UpdateOobDivisionItems(Division division)
        {
            _controller.UpdateItemValue(divisionTypeTextBox, division);
            _controller.UpdateItemValue(divisionIdTextBox, division);
            _controller.UpdateItemValue(divisionNameTextBox, division);
            _controller.UpdateItemValue(strengthTextBox, division);
            _controller.UpdateItemValue(maxStrengthTextBox, division);
            _controller.UpdateItemValue(organisationTextBox, division);
            _controller.UpdateItemValue(maxOrganisationTextBox, division);
            _controller.UpdateItemValue(divisionMoraleTextBox, division);
            _controller.UpdateItemValue(experienceTextBox, division);
            _controller.UpdateItemValue(lockedCheckBox, division);
            _controller.UpdateItemValue(dormantCheckBox, division);

            _controller.UpdateItemColor(divisionTypeTextBox, division);
            _controller.UpdateItemColor(divisionIdTextBox, division);
            _controller.UpdateItemColor(divisionNameTextBox, division);
            _controller.UpdateItemColor(strengthTextBox, division);
            _controller.UpdateItemColor(maxStrengthTextBox, division);
            _controller.UpdateItemColor(organisationTextBox, division);
            _controller.UpdateItemColor(maxOrganisationTextBox, division);
            _controller.UpdateItemColor(divisionMoraleTextBox, division);
            _controller.UpdateItemColor(experienceTextBox, division);
            _controller.UpdateItemColor(lockedCheckBox, division);
            _controller.UpdateItemColor(dormantCheckBox, division);

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 師団の兵科が変更された場合、リストの選択肢も変更する
            if (division.Branch != _lastDivisionBranch)
            {
                _lastDivisionBranch = division.Branch;
                _controller.UpdateListItems(unitTypeComboBox, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox1, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox2, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox3, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox4, division, settings);
                _controller.UpdateListItems(brigadeTypeComboBox5, division, settings);
            }

            _controller.UpdateListItems(unitModelComboBox, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox1, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox2, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox3, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox4, division, settings);
            _controller.UpdateListItems(brigadeModelComboBox5, division, settings);

            _controller.UpdateItemValue(unitTypeComboBox, division);
            _controller.UpdateItemValue(brigadeTypeComboBox1, division);
            _controller.UpdateItemValue(brigadeTypeComboBox2, division);
            _controller.UpdateItemValue(brigadeTypeComboBox3, division);
            _controller.UpdateItemValue(brigadeTypeComboBox4, division);
            _controller.UpdateItemValue(brigadeTypeComboBox5, division);
            _controller.UpdateItemValue(unitModelComboBox, division);
            _controller.UpdateItemValue(brigadeModelComboBox1, division);
            _controller.UpdateItemValue(brigadeModelComboBox2, division);
            _controller.UpdateItemValue(brigadeModelComboBox3, division);
            _controller.UpdateItemValue(brigadeModelComboBox4, division);
            _controller.UpdateItemValue(brigadeModelComboBox5, division);

            _controller.UpdateItemColor(unitTypeComboBox, division);
            _controller.UpdateItemColor(brigadeTypeComboBox1, division);
            _controller.UpdateItemColor(brigadeTypeComboBox2, division);
            _controller.UpdateItemColor(brigadeTypeComboBox3, division);
            _controller.UpdateItemColor(brigadeTypeComboBox4, division);
            _controller.UpdateItemColor(brigadeTypeComboBox5, division);
            _controller.UpdateItemColor(unitModelComboBox, division);
            _controller.UpdateItemColor(brigadeModelComboBox1, division);
            _controller.UpdateItemColor(brigadeModelComboBox2, division);
            _controller.UpdateItemColor(brigadeModelComboBox3, division);
            _controller.UpdateItemColor(brigadeModelComboBox4, division);
            _controller.UpdateItemColor(brigadeModelComboBox5, division);
        }

        /// <summary>
        ///     師団情報の編集項目をクリアする
        /// </summary>
        private void ClearOobDivisionItems()
        {
            divisionTypeTextBox.Text = "";
            divisionIdTextBox.Text = "";
            divisionNameTextBox.Text = "";
            unitTypeComboBox.SelectedIndex = -1;
            unitModelComboBox.SelectedIndex = -1;
            brigadeTypeComboBox1.SelectedIndex = -1;
            brigadeTypeComboBox2.SelectedIndex = -1;
            brigadeTypeComboBox3.SelectedIndex = -1;
            brigadeTypeComboBox4.SelectedIndex = -1;
            brigadeTypeComboBox5.SelectedIndex = -1;
            brigadeModelComboBox1.SelectedIndex = -1;
            brigadeModelComboBox2.SelectedIndex = -1;
            brigadeModelComboBox3.SelectedIndex = -1;
            brigadeModelComboBox4.SelectedIndex = -1;
            brigadeModelComboBox5.SelectedIndex = -1;
            strengthTextBox.Text = "";
            maxStrengthTextBox.Text = "";
            organisationTextBox.Text = "";
            maxOrganisationTextBox.Text = "";
            divisionMoraleTextBox.Text = "";
            experienceTextBox.Text = "";
            lockedCheckBox.Checked = false;
            dormantCheckBox.Checked = false;
        }

        /// <summary>
        ///     師団情報の編集項目を有効化する
        /// </summary>
        private void EnableOobDivisionItems()
        {
            divisionGroupBox.Enabled = true;
        }

        /// <summary>
        ///     師団情報の編集項目を無効化する
        /// </summary>
        private void DisableOobDivisionItems()
        {
            divisionGroupBox.Enabled = false;
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, unit);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, unit, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, unit);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, unit, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, unit, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, unit);
            object sel = _controller.GetListItemValue(itemId, e.Index, unit);
            Brush brush = ((int) val == (int) sel) && _controller.IsItemDirty(itemId, unit)
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            Unit unit = _unitTreeController.GetSelectedUnit();
            if (unit == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index, unit);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, unit);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, unit);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, unit, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, unit);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, unit, settings);

            // 文字色変更のため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, unit);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, division);
            object sel = _controller.GetListItemValue(itemId, e.Index, division);
            Brush brush = ((int) val == (int) sel) && _controller.IsItemDirty(itemId, division)
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index, division);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色変更のため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionComboBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, division);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Division division = _unitTreeController.GetSelectedDivision();
            if (division == null)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(_selectedCountry);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, division);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, division);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, division, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, division);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, division, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, division, settings);
        }

        #endregion
    }
}