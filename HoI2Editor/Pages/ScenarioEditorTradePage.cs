using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタの貿易タブ
    /// </summary>
    internal partial class ScenarioEditorTradePage : UserControl
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
        internal ScenarioEditorTradePage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitTradeInfoItems();
            InitTradeDealsItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 貿易国コンボボックスを更新する
            UpdateCountryComboBox(tradeCountryComboBox1, false);
            UpdateCountryComboBox(tradeCountryComboBox2, false);

            // 貿易リストを更新する
            UpdateTradeList();

            // 貿易リストを有効化する
            EnableTradeList();

            // 新規ボタンを有効化する
            EnableTradeNewButton();
        }

        #endregion

        #region 貿易タブ

        #region 貿易タブ - 貿易リスト

        /// <summary>
        ///     貿易リストの表示を更新する
        /// </summary>
        private void UpdateTradeList()
        {
            List<Treaty> trades = Scenarios.Data.GlobalData.Trades;
            tradeListView.BeginUpdate();
            tradeListView.Items.Clear();
            foreach (Treaty treaty in trades)
            {
                tradeListView.Items.Add(CreateTradeListViewItem(treaty));
            }
            tradeListView.EndUpdate();

            // 編集項目を無効化する
            DisableTradeInfoItems();
            DisableTradeDealsItems();
            DisableTradeButtons();

            // 編集項目をクリアする
            ClearTradeInfoItems();
            ClearTradeDealsItems();
        }

        /// <summary>
        ///     貿易リストを有効化する
        /// </summary>
        private void EnableTradeList()
        {
            tradeListView.Enabled = true;
        }

        /// <summary>
        ///     新規ボタンを有効化する
        /// </summary>
        private void EnableTradeNewButton()
        {
            tradeNewButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを有効化する
        /// </summary>
        private void EnableTradeButtons()
        {
            int index = tradeListView.SelectedIndices[0];
            int count = tradeListView.Items.Count;
            tradeUpButton.Enabled = index > 0;
            tradeDownButton.Enabled = index < count - 1;
            tradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを無効化する
        /// </summary>
        private void DisableTradeButtons()
        {
            tradeUpButton.Enabled = false;
            tradeDownButton.Enabled = false;
            tradeRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     貿易リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (tradeListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableTradeInfoItems();
                DisableTradeDealsItems();
                DisableTradeButtons();

                // 編集項目をクリアする
                ClearTradeInfoItems();
                ClearTradeDealsItems();
                return;
            }

            Treaty treaty = GetSelectedTrade();

            // 編集項目を更新する
            UpdateTradeInfoItems(treaty);
            UpdateTradeDealsItems(treaty);

            // 編集項目を有効化する
            EnableTradeInfoItems();
            EnableTradeDealsItems();
            EnableTradeButtons();
        }

        /// <summary>
        ///     貿易の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index - 1, item);
            tradeListView.Items[index - 1].Focused = true;
            tradeListView.Items[index - 1].Selected = true;
            tradeListView.EnsureVisible(index - 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index - 1, trade);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index + 1, item);
            tradeListView.Items[index + 1].Focused = true;
            tradeListView.Items[index + 1].Selected = true;
            tradeListView.EnsureVisible(index + 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index + 1, trade);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストに項目を追加する
            Treaty trade = new Treaty
            {
                StartDate = new GameDate(),
                EndDate = new GameDate(),
                Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
            };
            trades.Add(trade);

            // 貿易リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Tag = trade };
            item.SubItems.Add("");
            item.SubItems.Add("");
            tradeListView.Items.Add(item);

            // 編集済みフラグを設定する
            trade.SetDirty(Treaty.ItemId.StartYear);
            trade.SetDirty(Treaty.ItemId.StartMonth);
            trade.SetDirty(Treaty.ItemId.StartDay);
            trade.SetDirty(Treaty.ItemId.EndYear);
            trade.SetDirty(Treaty.ItemId.EndMonth);
            trade.SetDirty(Treaty.ItemId.EndDay);
            trade.SetDirty(Treaty.ItemId.Type);
            trade.SetDirty(Treaty.ItemId.Id);
            trade.SetDirty(Treaty.ItemId.Cancel);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (tradeListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = tradeListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     貿易の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            int index = tradeListView.SelectedIndices[0];
            Treaty trade = trades[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(trade.Id);

            // 貿易リストから項目を削除する
            trades.RemoveAt(index);

            // 貿易リストビューから項目を削除する
            tradeListView.Items.RemoveAt(index);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            if (index == trades.Count)
            {
                index--;
            }
            if (index >= 0)
            {
                tradeListView.Items[index].Focused = true;
                tradeListView.Items[index].Selected = true;
            }
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetTradeListItemText(int no, string s)
        {
            tradeListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     貿易リストの項目を作成する
        /// </summary>
        /// <param name="treaty">貿易情報</param>
        /// <returns>貿易リストの項目</returns>
        private static ListViewItem CreateTradeListViewItem(Treaty treaty)
        {
            ListViewItem item = new ListViewItem
            {
                Text = Countries.GetName(treaty.Country1),
                Tag = treaty
            };
            item.SubItems.Add(Countries.GetName(treaty.Country2));
            item.SubItems.Add(treaty.GetTradeString());

            return item;
        }

        /// <summary>
        ///     選択中の貿易情報を取得する
        /// </summary>
        /// <returns>選択中の貿易情報</returns>
        private Treaty GetSelectedTrade()
        {
            return tradeListView.SelectedItems.Count > 0 ? tradeListView.SelectedItems[0].Tag as Treaty : null;
        }

        #endregion

        #region 貿易タブ - 貿易情報

        /// <summary>
        ///     貿易情報の編集項目を初期化する
        /// </summary>
        private void InitTradeInfoItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.TradeStartYear, tradeStartYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeStartMonth, tradeStartMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeStartDay, tradeStartDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeEndYear, tradeEndYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeEndMonth, tradeEndMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeEndDay, tradeEndDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeType, tradeTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeId, tradeIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.TradeCancel, tradeCancelCheckBox);

            tradeStartYearTextBox.Tag = ScenarioEditorItemId.TradeStartYear;
            tradeStartMonthTextBox.Tag = ScenarioEditorItemId.TradeStartMonth;
            tradeStartDayTextBox.Tag = ScenarioEditorItemId.TradeStartDay;
            tradeEndYearTextBox.Tag = ScenarioEditorItemId.TradeEndYear;
            tradeEndMonthTextBox.Tag = ScenarioEditorItemId.TradeEndMonth;
            tradeEndDayTextBox.Tag = ScenarioEditorItemId.TradeEndDay;
            tradeTypeTextBox.Tag = ScenarioEditorItemId.TradeType;
            tradeIdTextBox.Tag = ScenarioEditorItemId.TradeId;
            tradeCancelCheckBox.Tag = ScenarioEditorItemId.TradeCancel;
        }

        /// <summary>
        ///     貿易情報の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeInfoItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeStartYearTextBox, treaty);
            _controller.UpdateItemValue(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeStartDayTextBox, treaty);
            _controller.UpdateItemValue(tradeEndYearTextBox, treaty);
            _controller.UpdateItemValue(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeEndDayTextBox, treaty);
            _controller.UpdateItemValue(tradeTypeTextBox, treaty);
            _controller.UpdateItemValue(tradeIdTextBox, treaty);
            _controller.UpdateItemValue(tradeCancelCheckBox, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeStartYearTextBox, treaty);
            _controller.UpdateItemColor(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeStartDayTextBox, treaty);
            _controller.UpdateItemColor(tradeEndYearTextBox, treaty);
            _controller.UpdateItemColor(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeEndDayTextBox, treaty);
            _controller.UpdateItemColor(tradeTypeTextBox, treaty);
            _controller.UpdateItemColor(tradeIdTextBox, treaty);
            _controller.UpdateItemColor(tradeCancelCheckBox, treaty);
        }

        /// <summary>
        ///     貿易情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeInfoItems()
        {
            tradeStartYearTextBox.Text = "";
            tradeStartMonthTextBox.Text = "";
            tradeStartDayTextBox.Text = "";
            tradeEndYearTextBox.Text = "";
            tradeEndMonthTextBox.Text = "";
            tradeEndDayTextBox.Text = "";
            tradeTypeTextBox.Text = "";
            tradeIdTextBox.Text = "";
            tradeCancelCheckBox.Checked = false;
        }

        /// <summary>
        ///     貿易情報の編集項目を有効化する
        /// </summary>
        private void EnableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易情報の編集項目を無効化する
        /// </summary>
        private void DisableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = false;
        }

        #endregion

        #region 貿易タブ - 貿易内容

        /// <summary>
        ///     貿易内容の編集項目を初期化する
        /// </summary>
        private void InitTradeDealsItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.TradeCountry1, tradeCountryComboBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeCountry2, tradeCountryComboBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeEnergy1, tradeEnergyTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeEnergy2, tradeEnergyTextBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeMetal1, tradeMetalTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeMetal2, tradeMetalTextBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeRareMaterials1, tradeRareMaterialsTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeRareMaterials2, tradeRareMaterialsTextBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeOil1, tradeOilTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeOil2, tradeOilTextBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeSupplies1, tradeSuppliesTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeSupplies2, tradeSuppliesTextBox2);
            _form._itemControls.Add(ScenarioEditorItemId.TradeMoney1, tradeMoneyTextBox1);
            _form._itemControls.Add(ScenarioEditorItemId.TradeMoney2, tradeMoneyTextBox2);

            tradeCountryComboBox1.Tag = ScenarioEditorItemId.TradeCountry1;
            tradeCountryComboBox2.Tag = ScenarioEditorItemId.TradeCountry2;
            tradeEnergyTextBox1.Tag = ScenarioEditorItemId.TradeEnergy1;
            tradeEnergyTextBox2.Tag = ScenarioEditorItemId.TradeEnergy2;
            tradeMetalTextBox1.Tag = ScenarioEditorItemId.TradeMetal1;
            tradeMetalTextBox2.Tag = ScenarioEditorItemId.TradeMetal2;
            tradeRareMaterialsTextBox1.Tag = ScenarioEditorItemId.TradeRareMaterials1;
            tradeRareMaterialsTextBox2.Tag = ScenarioEditorItemId.TradeRareMaterials2;
            tradeOilTextBox1.Tag = ScenarioEditorItemId.TradeOil1;
            tradeOilTextBox2.Tag = ScenarioEditorItemId.TradeOil2;
            tradeSuppliesTextBox1.Tag = ScenarioEditorItemId.TradeSupplies1;
            tradeSuppliesTextBox2.Tag = ScenarioEditorItemId.TradeSupplies2;
            tradeMoneyTextBox1.Tag = ScenarioEditorItemId.TradeMoney1;
            tradeMoneyTextBox2.Tag = ScenarioEditorItemId.TradeMoney2;

            // 貿易資源ラベル
            tradeEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            tradeMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            tradeRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            tradeOilLabel.Text = Config.GetText(TextId.ResourceOil);
            tradeSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            tradeMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
        }

        /// <summary>
        ///     貿易内容の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeDealsItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeCountryComboBox1, treaty);
            _controller.UpdateItemValue(tradeCountryComboBox2, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox1, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox2, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemValue(tradeOilTextBox1, treaty);
            _controller.UpdateItemValue(tradeOilTextBox2, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox2, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemColor(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox1, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox2, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemColor(tradeOilTextBox1, treaty);
            _controller.UpdateItemColor(tradeOilTextBox2, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox2, treaty);
        }

        /// <summary>
        ///     貿易内容の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeDealsItems()
        {
            tradeCountryComboBox1.SelectedIndex = -1;
            tradeCountryComboBox2.SelectedIndex = -1;
            tradeEnergyTextBox1.Text = "";
            tradeEnergyTextBox2.Text = "";
            tradeMetalTextBox1.Text = "";
            tradeMetalTextBox2.Text = "";
            tradeRareMaterialsTextBox1.Text = "";
            tradeRareMaterialsTextBox2.Text = "";
            tradeOilTextBox1.Text = "";
            tradeOilTextBox2.Text = "";
            tradeSuppliesTextBox1.Text = "";
            tradeSuppliesTextBox2.Text = "";
            tradeMoneyTextBox1.Text = "";
            tradeMoneyTextBox2.Text = "";
        }

        /// <summary>
        ///     貿易内容の編集項目を有効化する
        /// </summary>
        private void EnableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易内容の編集項目を無効化する
        /// </summary>
        private void DisableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = false;
        }

        /// <summary>
        ///     貿易国入れ替えボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSwapButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 値を入れ替える
            Country country = treaty.Country1;
            treaty.Country1 = treaty.Country2;
            treaty.Country2 = country;

            treaty.Energy = -treaty.Energy;
            treaty.Metal = -treaty.Metal;
            treaty.RareMaterials = -treaty.RareMaterials;
            treaty.Oil = -treaty.Oil;
            treaty.Supplies = -treaty.Supplies;
            treaty.Money = -treaty.Money;

            // 編集済みフラグを設定する
            treaty.SetDirty(Treaty.ItemId.Country1);
            treaty.SetDirty(Treaty.ItemId.Country2);
            treaty.SetDirty(Treaty.ItemId.Energy);
            treaty.SetDirty(Treaty.ItemId.Metal);
            treaty.SetDirty(Treaty.ItemId.RareMaterials);
            treaty.SetDirty(Treaty.ItemId.Oil);
            treaty.SetDirty(Treaty.ItemId.Supplies);
            treaty.SetDirty(Treaty.ItemId.Money);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            ListViewItem item = tradeListView.SelectedItems[0];
            item.Text = Countries.GetName(treaty.Country1);
            item.SubItems[1].Text = Countries.GetName(treaty.Country2);
            item.SubItems[2].Text = treaty.GetTradeString();

            // 編集項目を更新する
            UpdateTradeDealsItems(treaty);
        }

        #endregion

        #region 貿易タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

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
            if (!_controller.IsItemValueValid(itemId, val, treaty))
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
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
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
            if (!_controller.IsItemValueValid(itemId, val, treaty))
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
        private void OnTradeItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            bool val = control.Checked;
            if (val == (bool) _controller.GetItemValue(itemId, treaty))
            {
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
        ///     国家コンボボックスを更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="allowEmpty">空項目を許可するかどうか</param>
        private void UpdateCountryComboBox(ComboBox control, bool allowEmpty)
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            int width = control.Width;
            control.BeginUpdate();
            control.Items.Clear();
            if (allowEmpty)
            {
                control.Items.Add("");
            }
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                control.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, control.Font).Width + SystemInformation.VerticalScrollBarWidth + margin);
            }
            control.DropDownWidth = width;
            control.EndUpdate();
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            Country val = (Country) _controller.GetItemValue(itemId, treaty);
            Country sel = (Country) _controller.GetListItemValue(itemId, e.Index);
            Brush brush = (val == sel) && _controller.IsItemDirty(itemId, treaty)
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
        private void OnTradeCountryItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (control.SelectedIndex < 0)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            Country val = Countries.Tags[control.SelectedIndex];
            if (val == (Country) _controller.GetItemValue(itemId, treaty))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 項目色を変更するため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        #endregion

        #endregion
    }
}