using System;
using System.Drawing;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタの関係タブ
    /// </summary>
    internal partial class ScenarioEditorRelationPage : UserControl
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
        internal ScenarioEditorRelationPage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitRelationItems();
            InitGuaranteedItems();
            InitNonAggressionItems();
            InitPeaceItems();
            InitIntelligenceItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 選択国リストを更新する
            UpdateCountryListBox();

            // 選択国リストを有効化する
            EnableRelationCountryList();

            // 国家関係リストをクリアする
            ClearRelationList();

            // 編集項目を無効化する
            DisableRelationItems();
            DisableGuaranteedItems();
            DisableNonAggressionItems();
            DisablePeaceItems();
            DisableIntelligenceItems();

            // 編集項目をクリアする
            ClearRelationItems();
            ClearGuaranteedItems();
            ClearNonAggressionItems();
            ClearPeaceItems();
            ClearIntelligenceItems();

            // 国家関係リストを有効化する
            EnableRelationList();
        }

        #endregion

        #region 国家リスト

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableRelationCountryList()
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
            // 選択項目がなければ国家関係リストをクリアする
            if (countryListBox.SelectedIndex < 0)
            {
                // 国家関係リストをクリアする
                ClearRelationList();
                return;
            }

            // 国家関係リストを更新する
            UpdateRelationList();
        }

        /// <summary>
        ///     選択国を取得する
        /// </summary>
        /// <returns>選択国</returns>
        private Country GetSelectedCountry()
        {
            return countryListBox.SelectedIndex >= 0
                ? Countries.Tags[countryListBox.SelectedIndex]
                : Country.None;
        }

        #endregion

        #region 国家関係リスト

        /// <summary>
        ///     国家関係リストを更新する
        /// </summary>
        private void UpdateRelationList()
        {
            Country selected = GetSelectedCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(selected);

            relationListView.BeginUpdate();
            relationListView.Items.Clear();
            foreach (Country target in Countries.Tags)
            {
                relationListView.Items.Add(CreateRelationListItem(selected, target, settings));
            }
            relationListView.EndUpdate();
        }

        /// <summary>
        ///     国家関係リストをクリアする
        /// </summary>
        private void ClearRelationList()
        {
            relationListView.BeginUpdate();
            relationListView.Items.Clear();
            relationListView.EndUpdate();
        }

        /// <summary>
        ///     国家関係リストを有効化する
        /// </summary>
        private void EnableRelationList()
        {
            relationListView.Enabled = true;
        }

        /// <summary>
        ///     国家関係リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (relationListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableRelationItems();
                DisableGuaranteedItems();
                DisableNonAggressionItems();
                DisablePeaceItems();
                DisableIntelligenceItems();

                // 編集項目をクリアする
                ClearRelationItems();
                ClearGuaranteedItems();
                ClearNonAggressionItems();
                ClearPeaceItems();
                ClearIntelligenceItems();
                return;
            }

            Country selected = GetSelectedCountry();
            Country target = GetTargetRelationCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);
            Treaty nonAggression = Scenarios.GetNonAggression(selected, target);
            Treaty peace = Scenarios.GetPeace(selected, target);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            // 編集項目を更新する
            UpdateRelationItems(relation, target, settings);
            UpdateGuaranteedItems(relation);
            UpdateNonAggressionItems(nonAggression);
            UpdatePeaceItems(peace);
            UpdateIntelligenceItems(spy);

            // 編集項目を有効化する
            EnableRelationItems();
            EnableGuaranteedItems();
            EnableNonAggressionItems();
            EnablePeaceItems();
            EnableIntelligenceItems();
        }

        /// <summary>
        ///     貿易リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">項目のインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetRelationListItemText(int index, int no, string s)
        {
            relationListView.Items[index].SubItems[no].Text = s;
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetRelationListItemText(int no, string s)
        {
            relationListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     国家関係リストビューの項目を作成する
        /// </summary>
        /// <param name="selected">選択国</param>
        /// <param name="target">対象国</param>
        /// <param name="settings">国家設定</param>
        /// <returns>国家関係リストビューの項目</returns>
        private ListViewItem CreateRelationListItem(Country selected, Country target, CountrySettings settings)
        {
            ListViewItem item = new ListViewItem(Countries.GetTagName(target));
            Relation relation = Scenarios.GetCountryRelation(selected, target);
            Treaty nonAggression = Scenarios.GetNonAggression(selected, target);
            Treaty peace = Scenarios.GetPeace(selected, target);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            item.SubItems.Add(
                ObjectHelper.ToString(_controller.GetItemValue(ScenarioEditorItemId.DiplomacyRelationValue, relation)));
            item.SubItems.Add(
                (Country) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMaster, settings) == target
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (Country) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMilitaryControl, settings) == target
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyMilitaryAccess, relation)
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyGuaranteed, relation) ? Resources.Yes : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyNonAggression, nonAggression)
                    ? Resources.Yes
                    : "");
            item.SubItems.Add(
                (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyPeace, peace) ? Resources.Yes : "");
            item.SubItems.Add(
                ObjectHelper.ToString(_controller.GetItemValue(ScenarioEditorItemId.IntelligenceSpies, spy)));

            return item;
        }

        /// <summary>
        ///     国家関係の対象国を取得する
        /// </summary>
        /// <returns>国家関係の対象国</returns>
        private Country GetTargetRelationCountry()
        {
            return relationListView.SelectedItems.Count > 0
                ? Countries.Tags[relationListView.SelectedIndices[0]]
                : Country.None;
        }

        #endregion

        #region 国家関係

        /// <summary>
        ///     国家関係の編集項目を初期化する
        /// </summary>
        private void InitRelationItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyRelationValue, relationValueTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyMaster, masterCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyMilitaryControl, controlCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyMilitaryAccess, accessCheckBox);

            relationValueTextBox.Tag = ScenarioEditorItemId.DiplomacyRelationValue;
            masterCheckBox.Tag = ScenarioEditorItemId.DiplomacyMaster;
            controlCheckBox.Tag = ScenarioEditorItemId.DiplomacyMilitaryControl;
            accessCheckBox.Tag = ScenarioEditorItemId.DiplomacyMilitaryAccess;
        }

        /// <summary>
        ///     国家関係の編集項目を更新する
        /// </summary>
        /// <param name="relation">国家関係</param>
        /// <param name="target">相手国</param>
        /// <param name="settings">国家設定</param>
        private void UpdateRelationItems(Relation relation, Country target, CountrySettings settings)
        {
            _controller.UpdateItemValue(relationValueTextBox, relation);
            _controller.UpdateItemValue(masterCheckBox, target, settings);
            _controller.UpdateItemValue(controlCheckBox, target, settings);
            _controller.UpdateItemValue(accessCheckBox, relation);

            _controller.UpdateItemColor(relationValueTextBox, relation);
            _controller.UpdateItemColor(masterCheckBox, settings);
            _controller.UpdateItemColor(controlCheckBox, settings);
            _controller.UpdateItemColor(accessCheckBox, relation);
        }

        /// <summary>
        ///     国家関係の編集項目をクリアする
        /// </summary>
        private void ClearRelationItems()
        {
            relationValueTextBox.Text = "";
            masterCheckBox.Checked = false;
            controlCheckBox.Checked = false;
            accessCheckBox.Checked = false;
        }

        /// <summary>
        ///     国家関係の編集項目を有効化する
        /// </summary>
        private void EnableRelationItems()
        {
            relationGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家関係の編集項目を無効化する
        /// </summary>
        private void DisableRelationItems()
        {
            relationGroupBox.Enabled = false;
        }

        #endregion

        #region 独立保障

        /// <summary>
        ///     独立保障の編集項目を初期化する
        /// </summary>
        private void InitGuaranteedItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteed, guaranteedCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndYear, guaranteedYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndMonth, guaranteedMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyGuaranteedEndDay, guaranteedDayTextBox);

            guaranteedCheckBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteed;
            guaranteedYearTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndYear;
            guaranteedMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndMonth;
            guaranteedDayTextBox.Tag = ScenarioEditorItemId.DiplomacyGuaranteedEndDay;
        }

        /// <summary>
        ///     独立保障の編集項目を更新する
        /// </summary>
        /// <param name="relation">国家関係</param>
        private void UpdateGuaranteedItems(Relation relation)
        {
            _controller.UpdateItemValue(guaranteedCheckBox, relation);
            _controller.UpdateItemValue(guaranteedYearTextBox, relation);
            _controller.UpdateItemValue(guaranteedMonthTextBox, relation);
            _controller.UpdateItemValue(guaranteedDayTextBox, relation);

            _controller.UpdateItemColor(guaranteedCheckBox, relation);
            _controller.UpdateItemColor(guaranteedYearTextBox, relation);
            _controller.UpdateItemColor(guaranteedMonthTextBox, relation);
            _controller.UpdateItemColor(guaranteedDayTextBox, relation);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyGuaranteed, relation);
            guaranteedYearTextBox.Enabled = flag;
            guaranteedMonthTextBox.Enabled = flag;
            guaranteedDayTextBox.Enabled = flag;
        }

        /// <summary>
        ///     独立保障の編集項目をクリアする
        /// </summary>
        private void ClearGuaranteedItems()
        {
            guaranteedCheckBox.Checked = false;
            guaranteedYearTextBox.Text = "";
            guaranteedMonthTextBox.Text = "";
            guaranteedDayTextBox.Text = "";

            guaranteedYearTextBox.Enabled = false;
            guaranteedMonthTextBox.Enabled = false;
            guaranteedDayTextBox.Enabled = false;
        }

        /// <summary>
        ///     独立保障の編集項目を有効化する
        /// </summary>
        private void EnableGuaranteedItems()
        {
            guaranteedGroupBox.Enabled = true;
        }

        /// <summary>
        ///     独立保障の編集項目を無効化する
        /// </summary>
        private void DisableGuaranteedItems()
        {
            guaranteedGroupBox.Enabled = false;
        }

        #endregion

        #region 不可侵条約

        /// <summary>
        ///     不可侵条約の編集項目を初期化する
        /// </summary>
        private void InitNonAggressionItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggression, nonAggressionCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartYear, nonAggressionStartYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartMonth,
                nonAggressionStartMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionStartDay, nonAggressionStartDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndYear, nonAggressionEndYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndMonth, nonAggressionEndMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionEndDay, nonAggressionEndDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionType, nonAggressionTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyNonAggressionId, nonAggressionIdTextBox);

            nonAggressionCheckBox.Tag = ScenarioEditorItemId.DiplomacyNonAggression;
            nonAggressionStartYearTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartYear;
            nonAggressionStartMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartMonth;
            nonAggressionStartDayTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionStartDay;
            nonAggressionEndYearTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndYear;
            nonAggressionEndMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndMonth;
            nonAggressionEndDayTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionEndDay;
            nonAggressionTypeTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionType;
            nonAggressionIdTextBox.Tag = ScenarioEditorItemId.DiplomacyNonAggressionId;
        }

        /// <summary>
        ///     不可侵条約の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateNonAggressionItems(Treaty treaty)
        {
            _controller.UpdateItemValue(nonAggressionCheckBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartYearTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartMonthTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionStartDayTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndYearTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndMonthTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionEndDayTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionTypeTextBox, treaty);
            _controller.UpdateItemValue(nonAggressionIdTextBox, treaty);

            _controller.UpdateItemColor(nonAggressionCheckBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartYearTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartMonthTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionStartDayTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndYearTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndMonthTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionEndDayTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionTypeTextBox, treaty);
            _controller.UpdateItemColor(nonAggressionIdTextBox, treaty);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyNonAggression, treaty);
            nonAggressionStartLabel.Enabled = flag;
            nonAggressionStartYearTextBox.Enabled = flag;
            nonAggressionStartMonthTextBox.Enabled = flag;
            nonAggressionStartDayTextBox.Enabled = flag;
            nonAggressionEndLabel.Enabled = flag;
            nonAggressionEndYearTextBox.Enabled = flag;
            nonAggressionEndMonthTextBox.Enabled = flag;
            nonAggressionEndDayTextBox.Enabled = flag;
            nonAggressionIdLabel.Enabled = flag;
            nonAggressionTypeTextBox.Enabled = flag;
            nonAggressionIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     不可侵条約の編集項目をクリアする
        /// </summary>
        private void ClearNonAggressionItems()
        {
            nonAggressionCheckBox.Checked = false;
            nonAggressionStartYearTextBox.Text = "";
            nonAggressionStartMonthTextBox.Text = "";
            nonAggressionStartDayTextBox.Text = "";
            nonAggressionEndYearTextBox.Text = "";
            nonAggressionEndMonthTextBox.Text = "";
            nonAggressionEndDayTextBox.Text = "";
            nonAggressionTypeTextBox.Text = "";
            nonAggressionIdTextBox.Text = "";

            nonAggressionStartLabel.Enabled = false;
            nonAggressionStartYearTextBox.Enabled = false;
            nonAggressionStartMonthTextBox.Enabled = false;
            nonAggressionStartDayTextBox.Enabled = false;
            nonAggressionEndLabel.Enabled = false;
            nonAggressionEndYearTextBox.Enabled = false;
            nonAggressionEndMonthTextBox.Enabled = false;
            nonAggressionEndDayTextBox.Enabled = false;
            nonAggressionIdLabel.Enabled = false;
            nonAggressionTypeTextBox.Enabled = false;
            nonAggressionIdTextBox.Enabled = false;
        }

        /// <summary>
        ///     不可侵条約の編集項目を有効化する
        /// </summary>
        private void EnableNonAggressionItems()
        {
            nonAggressionGroupBox.Enabled = true;
        }

        /// <summary>
        ///     不可侵条約の編集項目を無効化する
        /// </summary>
        private void DisableNonAggressionItems()
        {
            nonAggressionGroupBox.Enabled = false;
        }

        #endregion

        #region 講和条約

        /// <summary>
        ///     講和条約の編集項目を初期化する
        /// </summary>
        private void InitPeaceItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeace, peaceCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartYear, peaceStartYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartMonth, peaceStartMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceStartDay, peaceStartDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndYear, peaceEndYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndMonth, peaceEndMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceEndDay, peaceEndDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceType, peaceTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.DiplomacyPeaceId, peaceIdTextBox);

            peaceCheckBox.Tag = ScenarioEditorItemId.DiplomacyPeace;
            peaceStartYearTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartYear;
            peaceStartMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartMonth;
            peaceStartDayTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceStartDay;
            peaceEndYearTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndYear;
            peaceEndMonthTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndMonth;
            peaceEndDayTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceEndDay;
            peaceTypeTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceType;
            peaceIdTextBox.Tag = ScenarioEditorItemId.DiplomacyPeaceId;
        }

        /// <summary>
        ///     講和条約の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdatePeaceItems(Treaty treaty)
        {
            _controller.UpdateItemValue(peaceCheckBox, treaty);
            _controller.UpdateItemValue(peaceStartYearTextBox, treaty);
            _controller.UpdateItemValue(peaceStartMonthTextBox, treaty);
            _controller.UpdateItemValue(peaceStartDayTextBox, treaty);
            _controller.UpdateItemValue(peaceEndYearTextBox, treaty);
            _controller.UpdateItemValue(peaceEndMonthTextBox, treaty);
            _controller.UpdateItemValue(peaceEndDayTextBox, treaty);
            _controller.UpdateItemValue(peaceTypeTextBox, treaty);
            _controller.UpdateItemValue(peaceIdTextBox, treaty);

            _controller.UpdateItemColor(peaceCheckBox, treaty);
            _controller.UpdateItemColor(peaceStartYearTextBox, treaty);
            _controller.UpdateItemColor(peaceStartMonthTextBox, treaty);
            _controller.UpdateItemColor(peaceStartDayTextBox, treaty);
            _controller.UpdateItemColor(peaceEndYearTextBox, treaty);
            _controller.UpdateItemColor(peaceEndMonthTextBox, treaty);
            _controller.UpdateItemColor(peaceEndDayTextBox, treaty);
            _controller.UpdateItemColor(peaceTypeTextBox, treaty);
            _controller.UpdateItemColor(peaceIdTextBox, treaty);

            bool flag = (bool) _controller.GetItemValue(ScenarioEditorItemId.DiplomacyPeace, treaty);
            peaceStartLabel.Enabled = flag;
            peaceStartYearTextBox.Enabled = flag;
            peaceStartMonthTextBox.Enabled = flag;
            peaceStartDayTextBox.Enabled = flag;
            peaceEndLabel.Enabled = flag;
            peaceEndYearTextBox.Enabled = flag;
            peaceEndMonthTextBox.Enabled = flag;
            peaceEndDayTextBox.Enabled = flag;
            peaceIdLabel.Enabled = flag;
            peaceTypeTextBox.Enabled = flag;
            peaceIdTextBox.Enabled = flag;
        }

        /// <summary>
        ///     講和条約の編集項目をクリアする
        /// </summary>
        private void ClearPeaceItems()
        {
            peaceCheckBox.Checked = false;
            peaceStartYearTextBox.Text = "";
            peaceStartMonthTextBox.Text = "";
            peaceStartDayTextBox.Text = "";
            peaceEndYearTextBox.Text = "";
            peaceEndMonthTextBox.Text = "";
            peaceEndDayTextBox.Text = "";
            peaceTypeTextBox.Text = "";
            peaceIdTextBox.Text = "";

            peaceStartLabel.Enabled = false;
            peaceStartYearTextBox.Enabled = false;
            peaceStartMonthTextBox.Enabled = false;
            peaceStartDayTextBox.Enabled = false;
            peaceEndLabel.Enabled = false;
            peaceEndYearTextBox.Enabled = false;
            peaceEndMonthTextBox.Enabled = false;
            peaceEndDayTextBox.Enabled = false;
            peaceIdLabel.Enabled = false;
            peaceTypeTextBox.Enabled = false;
            peaceIdTextBox.Enabled = false;
        }

        /// <summary>
        ///     講和条約の編集項目を有効化する
        /// </summary>
        private void EnablePeaceItems()
        {
            peaceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     講和条約の編集項目を無効化する
        /// </summary>
        private void DisablePeaceItems()
        {
            peaceGroupBox.Enabled = false;
        }

        #endregion

        #region 諜報

        /// <summary>
        ///     諜報情報の編集項目を更新する
        /// </summary>
        private void InitIntelligenceItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.IntelligenceSpies, spyNumNumericUpDown);

            spyNumNumericUpDown.Tag = ScenarioEditorItemId.IntelligenceSpies;
        }

        /// <summary>
        ///     諜報情報の編集項目を更新する
        /// </summary>
        /// <param name="spy">諜報設定</param>
        private void UpdateIntelligenceItems(SpySettings spy)
        {
            _controller.UpdateItemValue(spyNumNumericUpDown, spy);

            _controller.UpdateItemColor(spyNumNumericUpDown, spy);
        }

        /// <summary>
        ///     諜報情報の編集項目をクリアする
        /// </summary>
        private void ClearIntelligenceItems()
        {
            spyNumNumericUpDown.Value = 0;
        }

        /// <summary>
        ///     諜報情報の編集項目を有効化する
        /// </summary>
        private void EnableIntelligenceItems()
        {
            intelligenceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     諜報情報の編集項目を無効化する
        /// </summary>
        private void DisableIntelligenceItems()
        {
            intelligenceGroupBox.Enabled = false;
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, relation);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, relation);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, relation);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            Relation relation = Scenarios.GetCountryRelation(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, relation);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (relation == null)
            {
                relation = new Relation { Country = target };
                settings.Relations.Add(relation);
                Scenarios.SetCountryRelation(selected, relation);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, relation);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, relation);

            // 値を更新する
            _controller.SetItemValue(itemId, val, relation);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, relation, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, relation);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationCountryItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);

            // 初期値から変更されていなければ何もしない
            Country val = control.Checked ? target : Country.None;
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev == null) && (val == Country.None))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (Country) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
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
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationNonAggressionItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetNonAggression(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationNonAggressionItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetNonAggression(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            if (val)
            {
                treaty = new Treaty
                {
                    Type = TreatyType.NonAggression,
                    Country1 = selected,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                Scenarios.Data.GlobalData.NonAggressions.Add(treaty);
                Scenarios.SetNonAggression(treaty);
            }
            else
            {
                Scenarios.Data.GlobalData.NonAggressions.Remove(treaty);
                Scenarios.RemoveNonAggression(treaty);
            }

            _controller.OutputItemValueChangedLog(itemId, val, !val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, val ? treaty : null);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationPeaceItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetPeace(selected, target);

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationPeaceItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            Treaty treaty = Scenarios.GetPeace(selected, target);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            if (val)
            {
                treaty = new Treaty
                {
                    Type = TreatyType.Peace,
                    Country1 = selected,
                    Country2 = target,
                    StartDate = new GameDate(),
                    EndDate = new GameDate(),
                    Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
                };
                Scenarios.Data.GlobalData.Peaces.Add(treaty);
                Scenarios.SetPeace(treaty);
            }
            else
            {
                Scenarios.Data.GlobalData.Peaces.Remove(treaty);
                Scenarios.RemovePeace(treaty);
            }

            _controller.OutputItemValueChangedLog(itemId, val, !val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, val ? treaty : null);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelationIntelligenceItemNumericUpDownValueChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country selected = GetSelectedCountry();
            if (selected == Country.None)
            {
                return;
            }
            Country target = GetTargetRelationCountry();
            if (target == Country.None)
            {
                return;
            }

            NumericUpDown control = sender as NumericUpDown;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(selected);
            SpySettings spy = Scenarios.GetCountryIntelligence(selected, target);

            // 初期値から変更されていなければ何もしない
            int val = (int) control.Value;
            object prev = _controller.GetItemValue(itemId, spy);
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
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, spy);
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(selected);
            }

            if (spy == null)
            {
                spy = new SpySettings { Country = target };
                Scenarios.SetCountryIntelligence(selected, spy);
            }

            _controller.OutputItemValueChangedLog(itemId, val, selected, spy);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, spy);

            // 値を更新する
            _controller.SetItemValue(itemId, val, spy);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, spy, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, spy);
        }

        #endregion
    }
}