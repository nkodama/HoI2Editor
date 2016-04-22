using System;
using System.ComponentModel;
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
    ///     シナリオエディタの国家タブ
    /// </summary>
    [ToolboxItem(false)]
    internal partial class ScenarioEditorCountryPage : UserControl
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
        internal ScenarioEditorCountryPage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitCountryInfoItems();
            InitCountryModifierItems();
            InitCountryResourceItems();
            InitCountryAiItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 兄弟国コンボボックスを更新する
            UpdateRegularIdComboBox();

            // 国家リストボックスを更新する
            UpdateCountryListBox();

            // 国家リストボックスを有効化する
            EnableCountryListBox();

            // 編集項目を無効化する
            DisableCountryInfoItems();
            DisableCountryModifierItems();
            DisableCountryResourceItems();
            DisableCountryAiItems();

            // 編集項目をクリアする
            ClearCountryInfoItems();
            ClearCountryModifierItems();
            ClearCountryResourceItems();
            ClearCountryAiItems();
        }

        #endregion

        #region 国家リスト

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableCountryListBox()
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
                DisableCountryInfoItems();
                DisableCountryModifierItems();
                DisableCountryResourceItems();
                DisableCountryAiItems();

                // 編集項目をクリアする
                ClearCountryInfoItems();
                ClearCountryModifierItems();
                ClearCountryResourceItems();
                ClearCountryAiItems();
                return;
            }

            Country country = GetSelectedCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 編集項目を更新する
            UpdateCountryInfoItems(country, settings);
            UpdateCountryModifierItems(settings);
            UpdateCountryResourceItems(settings);
            UpdateCountryAiItems(settings);

            // 編集項目を有効化する
            EnableCountryInfoItems();
            EnableCountryModifierItems();
            EnableCountryResourceItems();
            EnableCountryAiItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedCountry()
        {
            return countryListBox.SelectedIndex >= 0 ? Countries.Tags[countryListBox.SelectedIndex] : Country.None;
        }

        #endregion

        #region 国家情報

        /// <summary>
        ///     国家情報の編集項目を初期化する
        /// </summary>
        private void InitCountryInfoItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CountryNameKey, countryNameKeyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryNameString, countryNameStringTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryFlagExt, flagExtTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryRegularId, regularIdComboBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryBelligerence, belligerenceTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryDissent, dissentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryExtraTc, extraTcTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryNuke, nukeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryNukeYear, nukeYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryNukeMonth, nukeMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryNukeDay, nukeDayTextBox);

            countryNameKeyTextBox.Tag = ScenarioEditorItemId.CountryNameKey;
            countryNameStringTextBox.Tag = ScenarioEditorItemId.CountryNameString;
            flagExtTextBox.Tag = ScenarioEditorItemId.CountryFlagExt;
            regularIdComboBox.Tag = ScenarioEditorItemId.CountryRegularId;
            belligerenceTextBox.Tag = ScenarioEditorItemId.CountryBelligerence;
            dissentTextBox.Tag = ScenarioEditorItemId.CountryDissent;
            extraTcTextBox.Tag = ScenarioEditorItemId.CountryExtraTc;
            nukeTextBox.Tag = ScenarioEditorItemId.CountryNuke;
            nukeYearTextBox.Tag = ScenarioEditorItemId.CountryNukeYear;
            nukeMonthTextBox.Tag = ScenarioEditorItemId.CountryNukeMonth;
            nukeDayTextBox.Tag = ScenarioEditorItemId.CountryNukeDay;
        }

        /// <summary>
        ///     国家情報の編集項目を更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryInfoItems(Country country, CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(countryNameKeyTextBox, settings);
            _controller.UpdateItemValue(countryNameStringTextBox, country, settings);
            _controller.UpdateItemValue(flagExtTextBox, settings);
            _controller.UpdateItemValue(regularIdComboBox, settings);
            _controller.UpdateItemValue(belligerenceTextBox, settings);
            _controller.UpdateItemValue(dissentTextBox, settings);
            _controller.UpdateItemValue(extraTcTextBox, settings);
            _controller.UpdateItemValue(nukeTextBox, settings);
            _controller.UpdateItemValue(nukeYearTextBox, settings);
            _controller.UpdateItemValue(nukeMonthTextBox, settings);
            _controller.UpdateItemValue(nukeDayTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(countryNameKeyTextBox, settings);
            _controller.UpdateItemColor(countryNameStringTextBox, country, settings);
            _controller.UpdateItemColor(flagExtTextBox, settings);
            _controller.UpdateItemColor(belligerenceTextBox, settings);
            _controller.UpdateItemColor(dissentTextBox, settings);
            _controller.UpdateItemColor(extraTcTextBox, settings);
            _controller.UpdateItemColor(nukeTextBox, settings);
            _controller.UpdateItemColor(nukeYearTextBox, settings);
            _controller.UpdateItemColor(nukeMonthTextBox, settings);
            _controller.UpdateItemColor(nukeDayTextBox, settings);
        }

        /// <summary>
        ///     国家情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryInfoItems()
        {
            countryNameKeyTextBox.Text = "";
            countryNameStringTextBox.Text = "";
            flagExtTextBox.Text = "";
            regularIdComboBox.SelectedIndex = -1;
            belligerenceTextBox.Text = "";
            dissentTextBox.Text = "";
            extraTcTextBox.Text = "";
            nukeTextBox.Text = "";
            nukeYearTextBox.Text = "";
            nukeMonthTextBox.Text = "";
            nukeDayTextBox.Text = "";
        }

        /// <summary>
        ///     国家情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryInfoItems()
        {
            countryInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryInfoItems()
        {
            countryInfoGroupBox.Enabled = false;
        }

        /// <summary>
        ///     兄弟国コンボボックスを更新する
        /// </summary>
        private void UpdateRegularIdComboBox()
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            int width = regularIdComboBox.Width;
            regularIdComboBox.BeginUpdate();
            regularIdComboBox.Items.Clear();
            regularIdComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                regularIdComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, regularIdComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            regularIdComboBox.DropDownWidth = width;
            regularIdComboBox.EndUpdate();
        }

        #endregion

        #region 補正値

        /// <summary>
        ///     国家補正値の編集項目を初期化する
        /// </summary>
        private void InitCountryModifierItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CountryGroundDefEff, groundDefEffTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryPeacetimeIcModifier, peacetimeIcModifierTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryWartimeIcModifier, wartimeIcModifierTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryIndustrialModifier, industrialModifierTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryRelativeManpower, relativeManpowerTextBox);

            groundDefEffTextBox.Tag = ScenarioEditorItemId.CountryGroundDefEff;
            peacetimeIcModifierTextBox.Tag = ScenarioEditorItemId.CountryPeacetimeIcModifier;
            wartimeIcModifierTextBox.Tag = ScenarioEditorItemId.CountryWartimeIcModifier;
            industrialModifierTextBox.Tag = ScenarioEditorItemId.CountryIndustrialModifier;
            relativeManpowerTextBox.Tag = ScenarioEditorItemId.CountryRelativeManpower;
        }

        /// <summary>
        ///     国家補正値の編集項目を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryModifierItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(groundDefEffTextBox, settings);
            _controller.UpdateItemValue(peacetimeIcModifierTextBox, settings);
            _controller.UpdateItemValue(wartimeIcModifierTextBox, settings);
            _controller.UpdateItemValue(industrialModifierTextBox, settings);
            _controller.UpdateItemValue(relativeManpowerTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(groundDefEffTextBox, settings);
            _controller.UpdateItemColor(peacetimeIcModifierTextBox, settings);
            _controller.UpdateItemColor(wartimeIcModifierTextBox, settings);
            _controller.UpdateItemColor(industrialModifierTextBox, settings);
            _controller.UpdateItemColor(relativeManpowerTextBox, settings);
        }

        /// <summary>
        ///     国家補正値の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryModifierItems()
        {
            groundDefEffTextBox.Text = "";
            peacetimeIcModifierTextBox.Text = "";
            wartimeIcModifierTextBox.Text = "";
            industrialModifierTextBox.Text = "";
            relativeManpowerTextBox.Text = "";
        }

        /// <summary>
        ///     国家補正値の編集項目を有効化する
        /// </summary>
        private void EnableCountryModifierItems()
        {
            countryModifierGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家補正値の編集項目を無効化する
        /// </summary>
        private void DisableCountryModifierItems()
        {
            countryModifierGroupBox.Enabled = false;
        }

        #endregion

        #region 資源情報

        /// <summary>
        ///     国家資源情報の編集項目を初期化する
        /// </summary>
        private void InitCountryResourceItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CountryEnergy, countryEnergyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryMetal, countryMetalTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryRareMaterials, countryRareMaterialsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOil, countryOilTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountrySupplies, countrySuppliesTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryMoney, countryMoneyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryTransports, countryTransportsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryEscorts, countryEscortsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryManpower, countryManpowerTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapEnergy, offmapEnergyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapMetal, offmapMetalTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapRareMaterials, offmapRareMaterialsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapOil, offmapOilTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapSupplies, offmapSuppliesTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapMoney, offmapMoneyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapTransports, offmapTransportsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapEscorts, offmapEscortsTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapManpower, offmapManpowerTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOffmapIc, offmapIcTextBox);

            countryEnergyTextBox.Tag = ScenarioEditorItemId.CountryEnergy;
            countryMetalTextBox.Tag = ScenarioEditorItemId.CountryMetal;
            countryRareMaterialsTextBox.Tag = ScenarioEditorItemId.CountryRareMaterials;
            countryOilTextBox.Tag = ScenarioEditorItemId.CountryOil;
            countrySuppliesTextBox.Tag = ScenarioEditorItemId.CountrySupplies;
            countryMoneyTextBox.Tag = ScenarioEditorItemId.CountryMoney;
            countryTransportsTextBox.Tag = ScenarioEditorItemId.CountryTransports;
            countryEscortsTextBox.Tag = ScenarioEditorItemId.CountryEscorts;
            countryManpowerTextBox.Tag = ScenarioEditorItemId.CountryManpower;
            offmapEnergyTextBox.Tag = ScenarioEditorItemId.CountryOffmapEnergy;
            offmapMetalTextBox.Tag = ScenarioEditorItemId.CountryOffmapMetal;
            offmapRareMaterialsTextBox.Tag = ScenarioEditorItemId.CountryOffmapRareMaterials;
            offmapOilTextBox.Tag = ScenarioEditorItemId.CountryOffmapOil;
            offmapSuppliesTextBox.Tag = ScenarioEditorItemId.CountryOffmapSupplies;
            offmapMoneyTextBox.Tag = ScenarioEditorItemId.CountryOffmapMoney;
            offmapTransportsTextBox.Tag = ScenarioEditorItemId.CountryOffmapTransports;
            offmapEscortsTextBox.Tag = ScenarioEditorItemId.CountryOffmapEscorts;
            offmapManpowerTextBox.Tag = ScenarioEditorItemId.CountryOffmapManpower;
            offmapIcTextBox.Tag = ScenarioEditorItemId.CountryOffmapIc;

            // 国家資源ラベル
            countryEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            countryMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            countryRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            countryOilLabel.Text = Config.GetText(TextId.ResourceOil);
            countrySuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            countryMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
            countryTransportsLabel.Text = Config.GetText(TextId.ResourceTransports);
            countryEscortsLabel.Text = Config.GetText(TextId.ResourceEscorts);
            countryManpowerLabel.Text = Config.GetText(TextId.ResourceManpower);
            countryIcLabel.Text = Config.GetText(TextId.ResourceIc);
        }

        /// <summary>
        ///     国家資源情報の編集項目を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryResourceItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(countryEnergyTextBox, settings);
            _controller.UpdateItemValue(countryMetalTextBox, settings);
            _controller.UpdateItemValue(countryRareMaterialsTextBox, settings);
            _controller.UpdateItemValue(countryOilTextBox, settings);
            _controller.UpdateItemValue(countrySuppliesTextBox, settings);
            _controller.UpdateItemValue(countryMoneyTextBox, settings);
            _controller.UpdateItemValue(countryTransportsTextBox, settings);
            _controller.UpdateItemValue(countryEscortsTextBox, settings);
            _controller.UpdateItemValue(countryManpowerTextBox, settings);
            _controller.UpdateItemValue(offmapEnergyTextBox, settings);
            _controller.UpdateItemValue(offmapMetalTextBox, settings);
            _controller.UpdateItemValue(offmapRareMaterialsTextBox, settings);
            _controller.UpdateItemValue(offmapOilTextBox, settings);
            _controller.UpdateItemValue(offmapSuppliesTextBox, settings);
            _controller.UpdateItemValue(offmapMoneyTextBox, settings);
            _controller.UpdateItemValue(offmapTransportsTextBox, settings);
            _controller.UpdateItemValue(offmapEscortsTextBox, settings);
            _controller.UpdateItemValue(offmapManpowerTextBox, settings);
            _controller.UpdateItemValue(offmapIcTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(countryEnergyTextBox, settings);
            _controller.UpdateItemColor(countryMetalTextBox, settings);
            _controller.UpdateItemColor(countryRareMaterialsTextBox, settings);
            _controller.UpdateItemColor(countryOilTextBox, settings);
            _controller.UpdateItemColor(countrySuppliesTextBox, settings);
            _controller.UpdateItemColor(countryMoneyTextBox, settings);
            _controller.UpdateItemColor(countryTransportsTextBox, settings);
            _controller.UpdateItemColor(countryEscortsTextBox, settings);
            _controller.UpdateItemColor(countryManpowerTextBox, settings);
            _controller.UpdateItemColor(offmapEnergyTextBox, settings);
            _controller.UpdateItemColor(offmapMetalTextBox, settings);
            _controller.UpdateItemColor(offmapRareMaterialsTextBox, settings);
            _controller.UpdateItemColor(offmapOilTextBox, settings);
            _controller.UpdateItemColor(offmapSuppliesTextBox, settings);
            _controller.UpdateItemColor(offmapMoneyTextBox, settings);
            _controller.UpdateItemColor(offmapTransportsTextBox, settings);
            _controller.UpdateItemColor(offmapEscortsTextBox, settings);
            _controller.UpdateItemColor(offmapManpowerTextBox, settings);
            _controller.UpdateItemColor(offmapIcTextBox, settings);
        }

        /// <summary>
        ///     国家資源情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryResourceItems()
        {
            countryEnergyTextBox.Text = "";
            countryMetalTextBox.Text = "";
            countryRareMaterialsTextBox.Text = "";
            countryOilTextBox.Text = "";
            countrySuppliesTextBox.Text = "";
            countryMoneyTextBox.Text = "";
            countryTransportsTextBox.Text = "";
            countryEscortsTextBox.Text = "";
            countryManpowerTextBox.Text = "";
            offmapEnergyTextBox.Text = "";
            offmapMetalTextBox.Text = "";
            offmapRareMaterialsTextBox.Text = "";
            offmapOilTextBox.Text = "";
            offmapSuppliesTextBox.Text = "";
            offmapMoneyTextBox.Text = "";
            offmapTransportsTextBox.Text = "";
            offmapEscortsTextBox.Text = "";
            offmapManpowerTextBox.Text = "";
            offmapIcTextBox.Text = "";
        }

        /// <summary>
        ///     国家資源情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryResourceItems()
        {
            countryResourceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家資源情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryResourceItems()
        {
            countryResourceGroupBox.Enabled = false;
        }

        #endregion

        #region AI情報

        /// <summary>
        ///     国家AI情報の編集項目を初期化する
        /// </summary>
        private void InitCountryAiItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CountryAiFileName, aiFileNameTextBox);

            aiFileNameTextBox.Tag = ScenarioEditorItemId.CountryAiFileName;
        }

        /// <summary>
        ///     国家AI情報の編集項目を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateCountryAiItems(CountrySettings settings)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(aiFileNameTextBox, settings);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(aiFileNameTextBox, settings);
        }

        /// <summary>
        ///     国家AI情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearCountryAiItems()
        {
            aiFileNameTextBox.Text = "";
        }

        /// <summary>
        ///     AIファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAiFileNameBrowseButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Game.GetReadFileName(Game.AiPathName),
                FileName = settings.AiFileName,
                Filter = Resources.OpenAiFileDialogFilter
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                if (Game.IsExportFolderActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ExportFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        aiFileNameTextBox.Text = fileName;
                        return;
                    }
                }
                if (Game.IsModActive)
                {
                    fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.ModFolderName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        aiFileNameTextBox.Text = fileName;
                        return;
                    }
                }
                fileName = PathHelper.GetRelativePathName(dialog.FileName, Game.FolderName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    aiFileNameTextBox.Text = fileName;
                    return;
                }
                aiFileNameTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        ///     国家AI情報の編集項目を有効化する
        /// </summary>
        private void EnableCountryAiItems()
        {
            aiGroupBox.Enabled = true;
        }

        /// <summary>
        ///     国家AI情報の編集項目を無効化する
        /// </summary>
        private void DisableCountryAiItems()
        {
            aiGroupBox.Enabled = false;
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
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
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
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
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, settings);
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, settings);
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
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
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
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

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, country, settings);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, settings)))
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

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryNameItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedCountry();
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

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, country, settings);
            string val = control.Text;
            if ((prev == null) && string.IsNullOrEmpty(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if (val.Equals(_controller.GetItemValue(itemId, country, settings)))
            {
                return;
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
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
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

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            if (e.Index > 0)
            {
                Country country = GetSelectedCountry();
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
                object val = _controller.GetItemValue(itemId, settings);
                object sel = _controller.GetListItemValue(itemId, e.Index);
                Brush brush = (val != null) && ((Country) val == (Country) sel) &&
                              _controller.IsItemDirty(itemId, settings)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
                string s = control.Items[e.Index].ToString();
                e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                brush.Dispose();
            }

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Country country = GetSelectedCountry();
            if (country == Country.None)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            if ((settings == null) && (control.SelectedIndex == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            Country val = control.SelectedIndex > 0 ? Countries.Tags[control.SelectedIndex - 1] : Country.None;
            if ((settings != null) && (val == (Country) _controller.GetItemValue(itemId, settings)))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // コンボボックスの項目色を変更するために描画更新する
            control.Refresh();
        }

        #endregion
    }
}