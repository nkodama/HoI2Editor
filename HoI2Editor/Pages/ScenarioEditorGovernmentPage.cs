using System;
using System.Drawing;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタの政府タブ
    /// </summary>
    internal partial class ScenarioEditorGovernmentPage : UserControl
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

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタコントローラ</param>
        /// <param name="form">シナリオエディタのフォーム</param>
        internal ScenarioEditorGovernmentPage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitPoliticalSliderItems();
            InitCabinetItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 国家リストボックスを更新する
            UpdateCountryListBox();

            // 国家リストボックスを有効化する
            EnableGovernmentCountryListBox();

            // 編集項目を無効化する
            DisablePoliticalSliderItems();
            DisableCabinetItems();

            // 編集項目をクリアする
            ClearPoliticalSliderItems();
            ClearCabinetItems();
        }

        #endregion

        #region 政府タブ - 国家リスト

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableGovernmentCountryListBox()
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
                DisablePoliticalSliderItems();
                DisableCabinetItems();

                // 編集項目をクリアする
                ClearPoliticalSliderItems();
                ClearCabinetItems();
                return;
            }

            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            ScenarioHeader header = Scenarios.Data.Header;
            int year = header.StartDate?.Year ?? header.StartYear;

            // 編集項目を更新する
            UpdatePoliticalSliderItems(settings);
            UpdateCabinetItems(country, settings, year);

            // 編集項目を有効化する
            EnablePoliticalSliderItems();
            EnableCabinetItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedGovernmentCountry()
        {
            if (countryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }
            return Countries.Tags[countryListBox.SelectedIndex];
        }

        #endregion

        #region 政府タブ - 政策スライダー

        /// <summary>
        ///     政策スライダーの編集項目を初期化する
        /// </summary>
        private void InitPoliticalSliderItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.SliderYear, sliderYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.SliderMonth, sliderMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.SliderDay, sliderDayTextBox);

            _form._itemControls.Add(ScenarioEditorItemId.SliderDemocratic, democraticTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderPoliticalLeft, politicalLeftTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderFreedom, freedomTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderFreeMarket, freeMarketTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderProfessionalArmy, professionalArmyTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderDefenseLobby, defenseLobbyTrackBar);
            _form._itemControls.Add(ScenarioEditorItemId.SliderInterventionism, interventionismTrackBar);

            sliderYearTextBox.Tag = ScenarioEditorItemId.SliderYear;
            sliderMonthTextBox.Tag = ScenarioEditorItemId.SliderMonth;
            sliderDayTextBox.Tag = ScenarioEditorItemId.SliderDay;

            democraticTrackBar.Tag = ScenarioEditorItemId.SliderDemocratic;
            politicalLeftTrackBar.Tag = ScenarioEditorItemId.SliderPoliticalLeft;
            freedomTrackBar.Tag = ScenarioEditorItemId.SliderFreedom;
            freeMarketTrackBar.Tag = ScenarioEditorItemId.SliderFreeMarket;
            professionalArmyTrackBar.Tag = ScenarioEditorItemId.SliderProfessionalArmy;
            defenseLobbyTrackBar.Tag = ScenarioEditorItemId.SliderDefenseLobby;
            interventionismTrackBar.Tag = ScenarioEditorItemId.SliderInterventionism;

            democraticLabel.Text = Config.GetText(TextId.SliderDemocratic);
            authoritarianLabel.Text = Config.GetText(TextId.SliderAuthoritarian);
            politicalLeftLabel.Text = Config.GetText(TextId.SliderPoliticalLeft);
            politicalRightLabel.Text = Config.GetText(TextId.SliderPoliticalRight);
            openSocietyLabel.Text = Config.GetText(TextId.SliderOpenSociety);
            closedSocietyLabel.Text = Config.GetText(TextId.SliderClosedSociety);
            freeMarketLabel.Text = Config.GetText(TextId.SliderFreeMarket);
            centralPlanningLabel.Text = Config.GetText(TextId.SliderCentralPlanning);
            standingArmyLabel.Text = Config.GetText(TextId.SliderStandingArmy);
            draftedArmyLabel.Text = Config.GetText(TextId.SliderDraftedArmy);
            hawkLobbyLabel.Text = Config.GetText(TextId.SliderHawkLobby);
            doveLobbyLabel.Text = Config.GetText(TextId.SliderDoveLobby);
            interventionismLabel.Text = Config.GetText(TextId.SliderInterventionism);
            isolationismLabel.Text = Config.GetText(TextId.SlidlaIsolationism);

            authoritarianLabel.Left = democraticTrackBar.Left + democraticTrackBar.Width - authoritarianLabel.Width;
            politicalRightLabel.Left = politicalLeftTrackBar.Left + politicalLeftTrackBar.Width -
                                       politicalRightLabel.Width;
            closedSocietyLabel.Left = freedomTrackBar.Left + freedomTrackBar.Width - closedSocietyLabel.Width;
            centralPlanningLabel.Left = freeMarketTrackBar.Left + freeMarketTrackBar.Width - centralPlanningLabel.Width;
            draftedArmyLabel.Left = professionalArmyTrackBar.Left + professionalArmyTrackBar.Width -
                                    draftedArmyLabel.Width;
            doveLobbyLabel.Left = defenseLobbyTrackBar.Left + defenseLobbyTrackBar.Width - doveLobbyLabel.Width;
            isolationismLabel.Left = interventionismTrackBar.Left + interventionismTrackBar.Width -
                                     isolationismLabel.Width;
        }

        /// <summary>
        ///     政策スライダーの編集項目を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdatePoliticalSliderItems(CountrySettings settings)
        {
            _controller.UpdateItemValue(sliderYearTextBox, settings);
            _controller.UpdateItemValue(sliderMonthTextBox, settings);
            _controller.UpdateItemValue(sliderDayTextBox, settings);

            _controller.UpdateItemColor(sliderYearTextBox, settings);
            _controller.UpdateItemColor(sliderMonthTextBox, settings);
            _controller.UpdateItemColor(sliderDayTextBox, settings);

            _controller.UpdateItemValue(democraticTrackBar, settings);
            _controller.UpdateItemValue(politicalLeftTrackBar, settings);
            _controller.UpdateItemValue(freedomTrackBar, settings);
            _controller.UpdateItemValue(freeMarketTrackBar, settings);
            _controller.UpdateItemValue(professionalArmyTrackBar, settings);
            _controller.UpdateItemValue(defenseLobbyTrackBar, settings);
            _controller.UpdateItemValue(interventionismTrackBar, settings);
        }

        /// <summary>
        ///     政策スライダーの編集項目の表示をクリアする
        /// </summary>
        private void ClearPoliticalSliderItems()
        {
            sliderYearTextBox.Text = "";
            sliderMonthTextBox.Text = "";
            sliderDayTextBox.Text = "";

            democraticTrackBar.Value = 6;
            politicalLeftTrackBar.Value = 6;
            freedomTrackBar.Value = 6;
            freeMarketTrackBar.Value = 6;
            professionalArmyTrackBar.Value = 6;
            defenseLobbyTrackBar.Value = 6;
            interventionismTrackBar.Value = 6;
        }

        /// <summary>
        ///     政策スライダーの編集項目を有効化する
        /// </summary>
        private void EnablePoliticalSliderItems()
        {
            politicalSliderGroupBox.Enabled = true;
        }

        /// <summary>
        ///     政策スライダーの編集項目を無効化する
        /// </summary>
        private void DisablePoliticalSliderItems()
        {
            politicalSliderGroupBox.Enabled = false;
        }

        #endregion

        #region 政府タブ - 閣僚

        /// <summary>
        ///     閣僚の編集項目を初期化する
        /// </summary>
        private void InitCabinetItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfState, headOfStateComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfStateType, headOfStateTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfStateId, headOfStateIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernment, headOfGovernmentComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernmentType, headOfGovernmentTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetHeadOfGovernmentId, headOfGovernmentIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetForeignMinister, foreignMinisterComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetForeignMinisterType, foreignMinisterTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetForeignMinisterId, foreignMinisterIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinister, armamentMinisterComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinisterType, armamentMinisterTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetArmamentMinisterId, armamentMinisterIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurity, ministerOfSecurityComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurityType, ministerOfSecurityTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfSecurityId, ministerOfSecurityIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligence, ministerOfIntelligenceComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligenceType,
                ministerOfIntelligenceTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetMinisterOfIntelligenceId,
                ministerOfIntelligenceIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaff, chiefOfStaffComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaffType, chiefOfStaffTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfStaffId, chiefOfStaffIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmy, chiefOfArmyComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmyType, chiefOfArmyTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfArmyId, chiefOfArmyIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavy, chiefOfNavyComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavyType, chiefOfNavyTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfNavyId, chiefOfNavyIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAir, chiefOfAirComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAirType, chiefOfAirTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CabinetChiefOfAirId, chiefOfAirIdTextBox);

            headOfStateComboBox.Tag = ScenarioEditorItemId.CabinetHeadOfState;
            headOfStateTypeTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfStateType;
            headOfStateIdTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfStateId;
            headOfGovernmentComboBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernment;
            headOfGovernmentTypeTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernmentType;
            headOfGovernmentIdTextBox.Tag = ScenarioEditorItemId.CabinetHeadOfGovernmentId;
            foreignMinisterComboBox.Tag = ScenarioEditorItemId.CabinetForeignMinister;
            foreignMinisterTypeTextBox.Tag = ScenarioEditorItemId.CabinetForeignMinisterType;
            foreignMinisterIdTextBox.Tag = ScenarioEditorItemId.CabinetForeignMinisterId;
            armamentMinisterComboBox.Tag = ScenarioEditorItemId.CabinetArmamentMinister;
            armamentMinisterTypeTextBox.Tag = ScenarioEditorItemId.CabinetArmamentMinisterType;
            armamentMinisterIdTextBox.Tag = ScenarioEditorItemId.CabinetArmamentMinisterId;
            ministerOfSecurityComboBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurity;
            ministerOfSecurityTypeTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurityType;
            ministerOfSecurityIdTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfSecurityId;
            ministerOfIntelligenceComboBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligence;
            ministerOfIntelligenceTypeTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligenceType;
            ministerOfIntelligenceIdTextBox.Tag = ScenarioEditorItemId.CabinetMinisterOfIntelligenceId;
            chiefOfStaffComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaff;
            chiefOfStaffTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaffType;
            chiefOfStaffIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfStaffId;
            chiefOfArmyComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmy;
            chiefOfArmyTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmyType;
            chiefOfArmyIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfArmyId;
            chiefOfNavyComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavy;
            chiefOfNavyTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavyType;
            chiefOfNavyIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfNavyId;
            chiefOfAirComboBox.Tag = ScenarioEditorItemId.CabinetChiefOfAir;
            chiefOfAirTypeTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfAirType;
            chiefOfAirIdTextBox.Tag = ScenarioEditorItemId.CabinetChiefOfAirId;

            headOfStateLabel.Text = Config.GetText(TextId.MinisterHeadOfState);
            headOfGovernmentLabel.Text = Config.GetText(TextId.MinisterHeadOfGovernment);
            foreignMinisterlabel.Text = Config.GetText(TextId.MinisterForeignMinister);
            armamentMinisterLabel.Text = Config.GetText(TextId.MinisterArmamentMinister);
            ministerOfSecurityLabel.Text = Config.GetText(TextId.MinisterMinisterOfSecurity);
            ministerOfIntelligenceLabel.Text = Config.GetText(TextId.MinisterMinisterOfIntelligence);
            chiefOfStaffLabel.Text = Config.GetText(TextId.MinisterChiefOfStaff);
            chiefOfArmyLabel.Text = Config.GetText(TextId.MinisterChiefOfArmy);
            chiefOfNavyLabel.Text = Config.GetText(TextId.MinisterChiefOfNavy);
            chiefOfAirLabel.Text = Config.GetText(TextId.MinisterChiefOfAir);
        }

        /// <summary>
        ///     閣僚の編集項目を更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        /// <param name="year">対象年次</param>
        private void UpdateCabinetItems(Country country, CountrySettings settings, int year)
        {
            // 閣僚候補リストを更新する
            _controller.UpdateMinisterList(country, year);

            // 閣僚コンボボックスの表示を更新する
            _controller.UpdateItemValue(headOfStateComboBox, settings);
            _controller.UpdateItemValue(headOfGovernmentComboBox, settings);
            _controller.UpdateItemValue(foreignMinisterComboBox, settings);
            _controller.UpdateItemValue(armamentMinisterComboBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityComboBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceComboBox, settings);
            _controller.UpdateItemValue(chiefOfStaffComboBox, settings);
            _controller.UpdateItemValue(chiefOfArmyComboBox, settings);
            _controller.UpdateItemValue(chiefOfNavyComboBox, settings);
            _controller.UpdateItemValue(chiefOfAirComboBox, settings);

            // 閣僚type/idテキストボックスの表示を更新する
            _controller.UpdateItemValue(headOfStateTypeTextBox, settings);
            _controller.UpdateItemValue(headOfStateIdTextBox, settings);
            _controller.UpdateItemValue(headOfGovernmentTypeTextBox, settings);
            _controller.UpdateItemValue(headOfGovernmentIdTextBox, settings);
            _controller.UpdateItemValue(foreignMinisterTypeTextBox, settings);
            _controller.UpdateItemValue(foreignMinisterIdTextBox, settings);
            _controller.UpdateItemValue(armamentMinisterTypeTextBox, settings);
            _controller.UpdateItemValue(armamentMinisterIdTextBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityTypeTextBox, settings);
            _controller.UpdateItemValue(ministerOfSecurityIdTextBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceTypeTextBox, settings);
            _controller.UpdateItemValue(ministerOfIntelligenceIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfStaffTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfStaffIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfArmyTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfArmyIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfNavyTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfNavyIdTextBox, settings);
            _controller.UpdateItemValue(chiefOfAirTypeTextBox, settings);
            _controller.UpdateItemValue(chiefOfAirIdTextBox, settings);

            // 閣僚type/idテキストボックスの色を更新する
            _controller.UpdateItemColor(headOfStateTypeTextBox, settings);
            _controller.UpdateItemColor(headOfStateIdTextBox, settings);
            _controller.UpdateItemColor(headOfGovernmentTypeTextBox, settings);
            _controller.UpdateItemColor(headOfGovernmentIdTextBox, settings);
            _controller.UpdateItemColor(foreignMinisterTypeTextBox, settings);
            _controller.UpdateItemColor(foreignMinisterIdTextBox, settings);
            _controller.UpdateItemColor(armamentMinisterTypeTextBox, settings);
            _controller.UpdateItemColor(armamentMinisterIdTextBox, settings);
            _controller.UpdateItemColor(ministerOfSecurityTypeTextBox, settings);
            _controller.UpdateItemColor(ministerOfSecurityIdTextBox, settings);
            _controller.UpdateItemColor(ministerOfIntelligenceTypeTextBox, settings);
            _controller.UpdateItemColor(ministerOfIntelligenceIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfStaffTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfStaffIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfArmyTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfArmyIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfNavyTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfNavyIdTextBox, settings);
            _controller.UpdateItemColor(chiefOfAirTypeTextBox, settings);
            _controller.UpdateItemColor(chiefOfAirIdTextBox, settings);
        }

        /// <summary>
        ///     閣僚の編集項目の表示をクリアする
        /// </summary>
        private void ClearCabinetItems()
        {
            headOfStateComboBox.SelectedIndex = -1;
            headOfGovernmentComboBox.SelectedIndex = -1;
            foreignMinisterComboBox.SelectedIndex = -1;
            armamentMinisterComboBox.SelectedIndex = -1;
            ministerOfSecurityComboBox.SelectedIndex = -1;
            ministerOfIntelligenceComboBox.SelectedIndex = -1;
            chiefOfStaffComboBox.SelectedIndex = -1;
            chiefOfArmyComboBox.SelectedIndex = -1;
            chiefOfNavyComboBox.SelectedIndex = -1;
            chiefOfAirComboBox.SelectedIndex = -1;

            headOfStateTypeTextBox.Text = "";
            headOfStateIdTextBox.Text = "";
            headOfGovernmentTypeTextBox.Text = "";
            headOfGovernmentIdTextBox.Text = "";
            foreignMinisterTypeTextBox.Text = "";
            foreignMinisterIdTextBox.Text = "";
            armamentMinisterTypeTextBox.Text = "";
            armamentMinisterIdTextBox.Text = "";
            ministerOfSecurityTypeTextBox.Text = "";
            ministerOfSecurityIdTextBox.Text = "";
            ministerOfIntelligenceTypeTextBox.Text = "";
            ministerOfIntelligenceIdTextBox.Text = "";
            chiefOfStaffTypeTextBox.Text = "";
            chiefOfStaffIdTextBox.Text = "";
            chiefOfArmyTypeTextBox.Text = "";
            chiefOfArmyIdTextBox.Text = "";
            chiefOfNavyTypeTextBox.Text = "";
            chiefOfNavyIdTextBox.Text = "";
            chiefOfAirTypeTextBox.Text = "";
            chiefOfAirIdTextBox.Text = "";
        }

        /// <summary>
        ///     閣僚の編集項目を有効化する
        /// </summary>
        private void EnableCabinetItems()
        {
            cabinetGroupBox.Enabled = true;
        }

        /// <summary>
        ///     閣僚の編集項目を無効化する
        /// </summary>
        private void DisableCabinetItems()
        {
            cabinetGroupBox.Enabled = false;
        }

        #endregion

        #region 政府タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGovernmentIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, settings))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        /// <summary>
        ///     政策スライダースライドバーの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPoliticalSliderTrackBarScroll(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            TrackBar control = (TrackBar) sender;
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 初期値から変更されていなければ何もしない
            int val = 11 - control.Value;
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && (val == 5))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);
        }

        /// <summary>
        ///     閣僚コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCabinetComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
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
            Country country = GetSelectedGovernmentCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = _controller.GetItemValue(itemId, settings);
            object sel = _controller.GetListItemValue(itemId, e.Index);
            Brush brush = (val != null) && (sel != null) && ((int) val == (int) sel) &&
                          _controller.IsItemDirty(itemId, settings)
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     閣僚コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCabinetComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            ComboBox control = (ComboBox) sender;
            int index = control.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            // 選択中の国家がなければ何もしない
            Country country = GetSelectedGovernmentCountry();
            if (country == Country.None)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 初期値から変更されていなければ何もしない
            object val = _controller.GetListItemValue(itemId, index);
            if (val == null)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && ((int) val == (int) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, settings);
        }

        #endregion
    }
}