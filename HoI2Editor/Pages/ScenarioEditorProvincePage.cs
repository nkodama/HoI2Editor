using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタのプロヴィンスタブ
    /// </summary>
    internal partial class ScenarioEditorProvincePage : UserControl
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
        ///     マップパネルのコントローラ
        /// </summary>
        private readonly MapPanelController _mapPanelController;

        /// <summary>
        ///     マップパネルの初期化フラグ
        /// </summary>
        private bool _mapPanelInitialized;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタコントローラ</param>
        /// <param name="form">シナリオエディタのフォーム</param>
        internal ScenarioEditorProvincePage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // マップパネル
            _mapPanelController = new MapPanelController(provinceMapPanel, provinceMapPictureBox);
            _controller.AttachMapPanel(_mapPanelController);

            // 編集項目を初期化する
            InitMapFilter();
            InitProvinceIdTextBox();
            InitProvinceCountryItems();
            InitProvinceInfoItems();
            InitProvinceResourceItems();
            InitProvinceBuildingItems();
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        internal void UpdateItems()
        {
            // 陸地プロヴィンスリストを初期化する
            _controller.InitProvinceList();

            // プロヴィンスリストを初期化する
            InitProvinceList();

            // 国家フィルターを更新する
            UpdateProvinceCountryFilter();

            // プロヴィンスリストを有効化する
            EnableProvinceList();

            // 国家フィルターを有効化する
            EnableProvinceCountryFilter();

            // IDテキストボックスを有効化する
            EnableProvinceIdTextBox();

            // 編集項目を無効化する
            DisableProvinceCountryItems();
            DisableProvinceInfoItems();
            DisableProvinceResourceItems();
            DisableProvinceBuildingItems();

            // 編集項目の表示をクリアする
            ClearProvinceCountryItems();
            ClearProvinceInfoItems();
            ClearProvinceResourceItems();
            ClearProvinceBuildingItems();

            // 読み込み済みで未初期化ならばマップパネルを更新する
            UpdateMapPanel();
        }

        #endregion

        #region プロヴィンスタブ - マップ

        /// <summary>
        ///     マップパネルを更新する
        /// </summary>
        internal void UpdateMapPanel()
        {
            // マップ読み込み前ならば何もしない
            if (!Maps.IsLoaded[(int) MapLevel.Level2])
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_mapPanelInitialized)
            {
                return;
            }

            // 初期化済みフラグを設定する
            _mapPanelInitialized = true;

            // マップパネルを有効化する
            _mapPanelController.ProvinceMouseClick += OnMapPanelMouseClick;
            _mapPanelController.Show();

            // マップフィルターを有効化する
            EnableMapFilter();

            // 選択プロヴィンスが表示されるようにスクロールする
            Province province = GetSelectedProvince();
            if (province != null)
            {
                _mapPanelController.ScrollToProvince(province.Id);
            }
        }

        /// <summary>
        ///     プロヴィンス単位でマップ画像を更新する
        /// </summary>
        /// <param name="id">更新対象のプロヴィンスID</param>
        /// <param name="highlighted">強調表示の有無</param>
        /// <param name="mode">フィルターモード</param>
        internal void UpdateProvince(ushort id, bool highlighted, MapPanelController.MapFilterMode mode)
        {
            _mapPanelController.UpdateProvince(id, highlighted, mode);
        }

        /// <summary>
        ///     マップパネルのマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapPanelMouseClick(object sender, MapPanelController.ProvinceEventArgs e)
        {
            // 左クリック以外では何もしない
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // 陸地プロヴィンスでなければ何もしない
            if (_controller.GetLandProvinceIndex(e.Id) < 0)
            {
                return;
            }

            // 選択中のプロヴィンスIDを更新する
            provinceIdTextBox.Text = IntHelper.ToString(e.Id);

            // プロヴィンスを選択する
            SelectProvince(e.Id);

            Country country = GetSelectedProvinceCountry();
            switch (_mapPanelController.FilterMode)
            {
                case MapPanelController.MapFilterMode.None:
                    Country target = (from settings in Scenarios.Data.Countries
                        where settings.ControlledProvinces.Contains(e.Id)
                        select settings.Country).FirstOrDefault();
                    provinceCountryFilterComboBox.SelectedIndex = Array.IndexOf(Countries.Tags, target) + 1;
                    break;

                case MapPanelController.MapFilterMode.Core:
                    if (country != Country.None)
                    {
                        coreProvinceCheckBox.Checked = !coreProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Owned:
                    if (country != Country.None && ownedProvinceCheckBox.Enabled)
                    {
                        ownedProvinceCheckBox.Checked = !ownedProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Controlled:
                    if (country != Country.None && controlledProvinceCheckBox.Enabled)
                    {
                        controlledProvinceCheckBox.Checked = !controlledProvinceCheckBox.Checked;
                    }
                    break;

                case MapPanelController.MapFilterMode.Claimed:
                    if (country != Country.None)
                    {
                        claimedProvinceCheckBox.Checked = !claimedProvinceCheckBox.Checked;
                    }
                    break;
            }
        }

        #endregion

        #region プロヴィンスタブ - マップフィルター

        /// <summary>
        ///     マップフィルターを初期化する
        /// </summary>
        private void InitMapFilter()
        {
            mapFilterNoneRadioButton.Tag = MapPanelController.MapFilterMode.None;
            mapFilterCoreRadioButton.Tag = MapPanelController.MapFilterMode.Core;
            mapFilterOwnedRadioButton.Tag = MapPanelController.MapFilterMode.Owned;
            mapFilterControlledRadioButton.Tag = MapPanelController.MapFilterMode.Controlled;
            mapFilterClaimedRadioButton.Tag = MapPanelController.MapFilterMode.Claimed;
        }

        /// <summary>
        ///     マップフィルターを有効化する
        /// </summary>
        private void EnableMapFilter()
        {
            mapFilterGroupBox.Enabled = true;
        }

        /// <summary>
        ///     マップフィルターラジオボタンのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapFilterRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null)
            {
                return;
            }

            // チェックなしの時には他の項目にチェックがついているので処理しない
            if (!radioButton.Checked)
            {
                return;
            }

            // フィルターモードを更新する
            _mapPanelController.FilterMode = (MapPanelController.MapFilterMode) radioButton.Tag;
        }

        #endregion

        #region プロヴィンスタブ - 国家フィルター

        /// <summary>
        ///     国家フィルターを更新する
        /// </summary>
        private void UpdateProvinceCountryFilter()
        {
            provinceCountryFilterComboBox.BeginUpdate();
            provinceCountryFilterComboBox.Items.Clear();
            provinceCountryFilterComboBox.Items.Add("");
            foreach (Country country in Countries.Tags)
            {
                provinceCountryFilterComboBox.Items.Add(Scenarios.GetCountryTagName(country));
            }
            provinceCountryFilterComboBox.EndUpdate();
        }

        /// <summary>
        ///     国家フィルターを有効化する
        /// </summary>
        private void EnableProvinceCountryFilter()
        {
            provinceCountryFilterLabel.Enabled = true;
            provinceCountryFilterComboBox.Enabled = true;
        }

        /// <summary>
        ///     国家フィルターの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCountryFilterComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Country country = GetSelectedProvinceCountry();

            // プロヴィンスリストを更新する
            UpdateProvinceList(country);

            // マップフィルターを更新する
            _mapPanelController.SelectedCountry = country;

            // プロヴィンス国家グループボックスの編集項目を更新する
            Province province = GetSelectedProvince();
            if ((country != Country.None) && (province != null))
            {
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                UpdateProvinceCountryItems(province, settings);
                EnableProvinceCountryItems();
            }
            else
            {
                DisableProvinceCountryItems();
                ClearProvinceCountryItems();
            }
        }

        /// <summary>
        ///     選択国を取得する
        /// </summary>
        /// <returns>選択国</returns>
        private Country GetSelectedProvinceCountry()
        {
            if (provinceCountryFilterComboBox.SelectedIndex <= 0)
            {
                return Country.None;
            }
            return Countries.Tags[provinceCountryFilterComboBox.SelectedIndex - 1];
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンスID

        /// <summary>
        ///     プロヴィンスIDテキストボックスを初期化する
        /// </summary>
        private void InitProvinceIdTextBox()
        {
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceId, provinceIdTextBox);

            provinceIdTextBox.Tag = ScenarioEditorItemId.ProvinceId;
        }

        /// <summary>
        ///     プロヴィンスIDテキストボックスを有効化する
        /// </summary>
        private void EnableProvinceIdTextBox()
        {
            provinceIdLabel.Enabled = true;
            provinceIdTextBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンスIDテキストボックスのID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceIdTextBoxValidated(object sender, EventArgs e)
        {
            Province province = GetSelectedProvince();

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(provinceIdTextBox.Text, out val))
            {
                if (province != null)
                {
                    provinceIdTextBox.Text = IntHelper.ToString(province.Id);
                }
                return;
            }

            // 初期値から変更されていなければ何もしない
            if ((province == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((province != null) && (val == province.Id))
            {
                return;
            }

            // プロヴィンスを選択する
            SelectProvince(val);
        }

        /// <summary>
        ///     プロヴィンスを選択する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        private void SelectProvince(int id)
        {
            // プロヴィンスリストビューの選択項目を変更する
            int index = _controller.GetLandProvinceIndex(id);
            if (index >= 0)
            {
                ListViewItem item = provinceListView.Items[index];
                item.Focused = true;
                item.Selected = true;
                item.EnsureVisible();
            }
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンスリスト

        /// <summary>
        ///     陸地プロヴィンスリストを初期化する
        /// </summary>
        private void InitProvinceList()
        {
            provinceListView.BeginUpdate();
            provinceListView.Items.Clear();
            foreach (Province province in _controller.GetLandProvinces())
            {
                ListViewItem item = CreateProvinceListItem(province);
                provinceListView.Items.Add(item);
            }
            provinceListView.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを更新する
        /// </summary>
        /// <param name="country">選択国</param>
        private void UpdateProvinceList(Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            provinceListView.BeginUpdate();
            if (settings != null)
            {
                foreach (ListViewItem item in provinceListView.Items)
                {
                    Province province = (Province) item.Tag;
                    item.SubItems[2].Text = province.Id == settings.Capital ? Resources.Yes : "";
                    item.SubItems[3].Text = settings.NationalProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[4].Text = settings.OwnedProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[5].Text = settings.ControlledProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[6].Text = settings.ClaimedProvinces.Contains(province.Id) ? Resources.Yes : "";
                }
            }
            else
            {
                foreach (ListViewItem item in provinceListView.Items)
                {
                    item.SubItems[2].Text = "";
                    item.SubItems[3].Text = "";
                    item.SubItems[4].Text = "";
                    item.SubItems[5].Text = "";
                    item.SubItems[6].Text = "";
                }
            }
            provinceListView.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを有効化する
        /// </summary>
        private void EnableProvinceList()
        {
            provinceListView.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">プロヴィンスリストビューのインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetProvinceListItemText(int index, int no, string s)
        {
            provinceListView.Items[index].SubItems[no].Text = s;
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目を作成する
        /// </summary>
        /// <param name="province">プロヴィンスデータ</param>
        /// <returns>プロヴィンスリストビューの項目</returns>
        private static ListViewItem CreateProvinceListItem(Province province)
        {
            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            ListViewItem item = new ListViewItem { Text = IntHelper.ToString(province.Id), Tag = province };
            item.SubItems.Add(Scenarios.GetProvinceName(province, settings));
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");

            return item;
        }

        /// <summary>
        ///     選択中のプロヴィンスを取得する
        /// </summary>
        /// <returns></returns>
        private Province GetSelectedProvince()
        {
            if (provinceListView.SelectedIndices.Count == 0)
            {
                return null;
            }
            return provinceListView.SelectedItems[0].Tag as Province;
        }

        /// <summary>
        ///     プロヴィンスリストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            Province province = GetSelectedProvince();
            if (province == null)
            {
                // 編集項目を無効化する
                DisableProvinceCountryItems();
                DisableProvinceInfoItems();
                DisableProvinceResourceItems();
                DisableProvinceBuildingItems();

                // 編集項目の表示をクリアする
                ClearProvinceCountryItems();
                ClearProvinceInfoItems();
                ClearProvinceResourceItems();
                ClearProvinceBuildingItems();
                return;
            }

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);
            Country country = GetSelectedProvinceCountry();
            CountrySettings countrySettings = Scenarios.GetCountrySettings(country);

            // 編集項目の表示を更新する
            UpdateProvinceCountryItems(province, countrySettings);
            UpdateProvinceInfoItems(province, settings);
            UpdateProvinceResourceItems(settings);
            UpdateProvinceBuildingItems(settings);

            // 編集項目を有効化する
            if (country != Country.None)
            {
                EnableProvinceCountryItems();
            }
            EnableProvinceInfoItems();
            EnableProvinceResourceItems();
            EnableProvinceBuildingItems();

            // マップをスクロールさせる
            _mapPanelController.ScrollToProvince(province.Id);
        }

        #endregion

        #region プロヴィンスタブ - 国家情報

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceCountryItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.CountryCapital, capitalCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryCoreProvinces, coreProvinceCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryOwnedProvinces, ownedProvinceCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryControlledProvinces, controlledProvinceCheckBox);
            _form._itemControls.Add(ScenarioEditorItemId.CountryClaimedProvinces, claimedProvinceCheckBox);

            capitalCheckBox.Tag = ScenarioEditorItemId.CountryCapital;
            coreProvinceCheckBox.Tag = ScenarioEditorItemId.CountryCoreProvinces;
            ownedProvinceCheckBox.Tag = ScenarioEditorItemId.CountryOwnedProvinces;
            controlledProvinceCheckBox.Tag = ScenarioEditorItemId.CountryControlledProvinces;
            claimedProvinceCheckBox.Tag = ScenarioEditorItemId.CountryClaimedProvinces;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        private void UpdateProvinceCountryItems(Province province, CountrySettings settings)
        {
            _controller.UpdateItemValue(capitalCheckBox, province, settings);
            _controller.UpdateItemValue(coreProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(ownedProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(controlledProvinceCheckBox, province, settings);
            _controller.UpdateItemValue(claimedProvinceCheckBox, province, settings);

            _controller.UpdateItemColor(capitalCheckBox, province, settings);
            _controller.UpdateItemColor(coreProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(ownedProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(controlledProvinceCheckBox, province, settings);
            _controller.UpdateItemColor(claimedProvinceCheckBox, province, settings);
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceCountryItems()
        {
            capitalCheckBox.Checked = false;
            coreProvinceCheckBox.Checked = false;
            ownedProvinceCheckBox.Checked = false;
            controlledProvinceCheckBox.Checked = false;
            claimedProvinceCheckBox.Checked = false;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceCountryItems()
        {
            provinceCountryGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス国家情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceCountryItems()
        {
            provinceCountryGroupBox.Enabled = false;
        }

        #endregion

        #region プロヴィンスタブ - プロヴィンス情報

        /// <summary>
        ///     プロヴィンス情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceInfoItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNameKey, provinceNameKeyTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNameString, provinceNameStringTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceVp, vpTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRevoltRisk, revoltRiskTextBox);

            provinceNameKeyTextBox.Tag = ScenarioEditorItemId.ProvinceNameKey;
            provinceNameStringTextBox.Tag = ScenarioEditorItemId.ProvinceNameString;
            vpTextBox.Tag = ScenarioEditorItemId.ProvinceVp;
            revoltRiskTextBox.Tag = ScenarioEditorItemId.ProvinceRevoltRisk;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を更新する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceInfoItems(Province province, ProvinceSettings settings)
        {
            _controller.UpdateItemValue(provinceIdTextBox, province);
            _controller.UpdateItemValue(provinceNameKeyTextBox, province, settings);
            _controller.UpdateItemValue(provinceNameStringTextBox, province, settings);
            _controller.UpdateItemValue(vpTextBox, settings);
            _controller.UpdateItemValue(revoltRiskTextBox, settings);

            _controller.UpdateItemColor(provinceNameKeyTextBox, settings);
            _controller.UpdateItemColor(provinceNameStringTextBox, settings);
            _controller.UpdateItemColor(vpTextBox, settings);
            _controller.UpdateItemColor(revoltRiskTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceInfoItems()
        {
            provinceIdTextBox.Text = "";
            provinceNameKeyTextBox.Text = "";
            provinceNameStringTextBox.Text = "";
            vpTextBox.Text = "";
            revoltRiskTextBox.Text = "";
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = true;
            provinceNameKeyTextBox.Enabled = Game.Type == GameType.DarkestHour;
        }

        /// <summary>
        ///     プロヴィンス情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceInfoItems()
        {
            provinceInfoGroupBox.Enabled = false;
        }

        #endregion

        #region プロヴィンスタブ - 資源情報

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceResourceItems()
        {
            // 資源名ラベル
            provinceManpowerLabel.Text = Config.GetText(TextId.ResourceManpower);
            provinceEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            provinceMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            provinceRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            provinceOilLabel.Text = Config.GetText(TextId.ResourceOil);
            provinceSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);

            // 編集項目
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceManpowerCurrent, manpowerCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceManpowerMax, manpowerMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceEnergyPool, energyPoolTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceEnergyCurrent, energyCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceEnergyMax, energyMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceMetalPool, metalPoolTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceMetalCurrent, metalCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceMetalMax, metalMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsPool, rareMaterialsPoolTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsCurrent, rareMaterialsCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRareMaterialsMax, rareMaterialsMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceOilPool, oilPoolTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceOilCurrent, oilCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceOilMax, oilMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSupplyPool, suppliesPoolTextBox);

            manpowerCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceManpowerCurrent;
            manpowerMaxTextBox.Tag = ScenarioEditorItemId.ProvinceManpowerMax;
            energyPoolTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyPool;
            energyCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyCurrent;
            energyMaxTextBox.Tag = ScenarioEditorItemId.ProvinceEnergyMax;
            metalPoolTextBox.Tag = ScenarioEditorItemId.ProvinceMetalPool;
            metalCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceMetalCurrent;
            metalMaxTextBox.Tag = ScenarioEditorItemId.ProvinceMetalMax;
            rareMaterialsPoolTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsPool;
            rareMaterialsCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsCurrent;
            rareMaterialsMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRareMaterialsMax;
            oilPoolTextBox.Tag = ScenarioEditorItemId.ProvinceOilPool;
            oilCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceOilCurrent;
            oilMaxTextBox.Tag = ScenarioEditorItemId.ProvinceOilMax;
            suppliesPoolTextBox.Tag = ScenarioEditorItemId.ProvinceSupplyPool;
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を更新する
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceResourceItems(ProvinceSettings settings)
        {
            _controller.UpdateItemValue(manpowerCurrentTextBox, settings);
            _controller.UpdateItemValue(manpowerMaxTextBox, settings);
            _controller.UpdateItemValue(energyPoolTextBox, settings);
            _controller.UpdateItemValue(energyCurrentTextBox, settings);
            _controller.UpdateItemValue(energyMaxTextBox, settings);
            _controller.UpdateItemValue(metalPoolTextBox, settings);
            _controller.UpdateItemValue(metalCurrentTextBox, settings);
            _controller.UpdateItemValue(metalMaxTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsPoolTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsCurrentTextBox, settings);
            _controller.UpdateItemValue(rareMaterialsMaxTextBox, settings);
            _controller.UpdateItemValue(oilPoolTextBox, settings);
            _controller.UpdateItemValue(oilCurrentTextBox, settings);
            _controller.UpdateItemValue(oilMaxTextBox, settings);
            _controller.UpdateItemValue(suppliesPoolTextBox, settings);

            _controller.UpdateItemColor(manpowerCurrentTextBox, settings);
            _controller.UpdateItemColor(manpowerMaxTextBox, settings);
            _controller.UpdateItemColor(energyPoolTextBox, settings);
            _controller.UpdateItemColor(energyCurrentTextBox, settings);
            _controller.UpdateItemColor(energyMaxTextBox, settings);
            _controller.UpdateItemColor(metalPoolTextBox, settings);
            _controller.UpdateItemColor(metalCurrentTextBox, settings);
            _controller.UpdateItemColor(metalMaxTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsPoolTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsCurrentTextBox, settings);
            _controller.UpdateItemColor(rareMaterialsMaxTextBox, settings);
            _controller.UpdateItemColor(oilPoolTextBox, settings);
            _controller.UpdateItemColor(oilCurrentTextBox, settings);
            _controller.UpdateItemColor(oilMaxTextBox, settings);
            _controller.UpdateItemColor(suppliesPoolTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceResourceItems()
        {
            manpowerCurrentTextBox.Text = "";
            manpowerMaxTextBox.Text = "";
            energyPoolTextBox.Text = "";
            energyCurrentTextBox.Text = "";
            energyMaxTextBox.Text = "";
            metalPoolTextBox.Text = "";
            metalCurrentTextBox.Text = "";
            metalMaxTextBox.Text = "";
            rareMaterialsPoolTextBox.Text = "";
            rareMaterialsCurrentTextBox.Text = "";
            rareMaterialsMaxTextBox.Text = "";
            oilPoolTextBox.Text = "";
            oilCurrentTextBox.Text = "";
            oilMaxTextBox.Text = "";
            suppliesPoolTextBox.Text = "";
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceResourceItems()
        {
            provinceResourceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     プロヴィンス資源情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceResourceItems()
        {
            provinceResourceGroupBox.Enabled = false;
        }

        #endregion

        #region プロヴィンスタブ - 建物情報

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を初期化する
        /// </summary>
        private void InitProvinceBuildingItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceIcCurrent, icCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceIcMax, icMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceIcRelative, icRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureCurrent, infrastructureCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureMax, infrastructureMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceInfrastructureRelative, infrastructureRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceLandFortCurrent, landFortCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceLandFortMax, landFortMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceLandFortRelative, landFortRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortCurrent, coastalFortCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortMax, coastalFortMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceCoastalFortRelative, coastalFortRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirCurrent, antiAirCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirMax, antiAirMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAntiAirRelative, antiAirRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseCurrent, airBaseCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseMax, airBaseMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceAirBaseRelative, airBaseRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseCurrent, navalBaseCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseMax, navalBaseMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNavalBaseRelative, navalBaseRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationCurrent, radarStationCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationMax, radarStationMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRadarStationRelative, radarStationRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorCurrent, nuclearReactorCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorMax, nuclearReactorMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearReactorRelative, nuclearReactorRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestCurrent, rocketTestCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestMax, rocketTestMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceRocketTestRelative, rocketTestRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilCurrent, syntheticOilCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilMax, syntheticOilMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticOilRelative, syntheticOilRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresCurrent, syntheticRaresCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresMax, syntheticRaresMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceSyntheticRaresRelative, syntheticRaresRelativeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerCurrent, nuclearPowerCurrentTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerMax, nuclearPowerMaxTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.ProvinceNuclearPowerRelative, nuclearPowerRelativeTextBox);

            icCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceIcCurrent;
            icMaxTextBox.Tag = ScenarioEditorItemId.ProvinceIcMax;
            icRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceIcRelative;
            infrastructureCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureCurrent;
            infrastructureMaxTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureMax;
            infrastructureRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceInfrastructureRelative;
            landFortCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortCurrent;
            landFortMaxTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortMax;
            landFortRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceLandFortRelative;
            coastalFortCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortCurrent;
            coastalFortMaxTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortMax;
            coastalFortRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceCoastalFortRelative;
            antiAirCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirCurrent;
            antiAirMaxTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirMax;
            antiAirRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceAntiAirRelative;
            airBaseCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseCurrent;
            airBaseMaxTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseMax;
            airBaseRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceAirBaseRelative;
            navalBaseCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseCurrent;
            navalBaseMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseMax;
            navalBaseRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNavalBaseRelative;
            radarStationCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationCurrent;
            radarStationMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationMax;
            radarStationRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceRadarStationRelative;
            nuclearReactorCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorCurrent;
            nuclearReactorMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorMax;
            nuclearReactorRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearReactorRelative;
            rocketTestCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestCurrent;
            rocketTestMaxTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestMax;
            rocketTestRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceRocketTestRelative;
            syntheticOilCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilCurrent;
            syntheticOilMaxTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilMax;
            syntheticOilRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticOilRelative;
            syntheticRaresCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresCurrent;
            syntheticRaresMaxTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresMax;
            syntheticRaresRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceSyntheticRaresRelative;
            nuclearPowerCurrentTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerCurrent;
            nuclearPowerMaxTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerMax;
            nuclearPowerRelativeTextBox.Tag = ScenarioEditorItemId.ProvinceNuclearPowerRelative;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を更新する
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        private void UpdateProvinceBuildingItems(ProvinceSettings settings)
        {
            _controller.UpdateItemValue(icCurrentTextBox, settings);
            _controller.UpdateItemValue(icMaxTextBox, settings);
            _controller.UpdateItemValue(icRelativeTextBox, settings);
            _controller.UpdateItemValue(infrastructureCurrentTextBox, settings);
            _controller.UpdateItemValue(infrastructureMaxTextBox, settings);
            _controller.UpdateItemValue(infrastructureRelativeTextBox, settings);
            _controller.UpdateItemValue(landFortCurrentTextBox, settings);
            _controller.UpdateItemValue(landFortMaxTextBox, settings);
            _controller.UpdateItemValue(landFortRelativeTextBox, settings);
            _controller.UpdateItemValue(coastalFortCurrentTextBox, settings);
            _controller.UpdateItemValue(coastalFortMaxTextBox, settings);
            _controller.UpdateItemValue(coastalFortRelativeTextBox, settings);
            _controller.UpdateItemValue(antiAirCurrentTextBox, settings);
            _controller.UpdateItemValue(antiAirMaxTextBox, settings);
            _controller.UpdateItemValue(antiAirRelativeTextBox, settings);
            _controller.UpdateItemValue(airBaseCurrentTextBox, settings);
            _controller.UpdateItemValue(airBaseMaxTextBox, settings);
            _controller.UpdateItemValue(airBaseRelativeTextBox, settings);
            _controller.UpdateItemValue(navalBaseCurrentTextBox, settings);
            _controller.UpdateItemValue(navalBaseMaxTextBox, settings);
            _controller.UpdateItemValue(navalBaseRelativeTextBox, settings);
            _controller.UpdateItemValue(radarStationCurrentTextBox, settings);
            _controller.UpdateItemValue(radarStationMaxTextBox, settings);
            _controller.UpdateItemValue(radarStationRelativeTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorCurrentTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorMaxTextBox, settings);
            _controller.UpdateItemValue(nuclearReactorRelativeTextBox, settings);
            _controller.UpdateItemValue(rocketTestCurrentTextBox, settings);
            _controller.UpdateItemValue(rocketTestMaxTextBox, settings);
            _controller.UpdateItemValue(rocketTestRelativeTextBox, settings);
            _controller.UpdateItemValue(syntheticOilCurrentTextBox, settings);
            _controller.UpdateItemValue(syntheticOilMaxTextBox, settings);
            _controller.UpdateItemValue(syntheticOilRelativeTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresCurrentTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresMaxTextBox, settings);
            _controller.UpdateItemValue(syntheticRaresRelativeTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerCurrentTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerMaxTextBox, settings);
            _controller.UpdateItemValue(nuclearPowerRelativeTextBox, settings);

            _controller.UpdateItemColor(icCurrentTextBox, settings);
            _controller.UpdateItemColor(icMaxTextBox, settings);
            _controller.UpdateItemColor(icRelativeTextBox, settings);
            _controller.UpdateItemColor(infrastructureCurrentTextBox, settings);
            _controller.UpdateItemColor(infrastructureMaxTextBox, settings);
            _controller.UpdateItemColor(infrastructureRelativeTextBox, settings);
            _controller.UpdateItemColor(landFortCurrentTextBox, settings);
            _controller.UpdateItemColor(landFortMaxTextBox, settings);
            _controller.UpdateItemColor(landFortRelativeTextBox, settings);
            _controller.UpdateItemColor(coastalFortCurrentTextBox, settings);
            _controller.UpdateItemColor(coastalFortMaxTextBox, settings);
            _controller.UpdateItemColor(coastalFortRelativeTextBox, settings);
            _controller.UpdateItemColor(antiAirCurrentTextBox, settings);
            _controller.UpdateItemColor(antiAirMaxTextBox, settings);
            _controller.UpdateItemColor(antiAirRelativeTextBox, settings);
            _controller.UpdateItemColor(airBaseCurrentTextBox, settings);
            _controller.UpdateItemColor(airBaseMaxTextBox, settings);
            _controller.UpdateItemColor(airBaseRelativeTextBox, settings);
            _controller.UpdateItemColor(navalBaseCurrentTextBox, settings);
            _controller.UpdateItemColor(navalBaseMaxTextBox, settings);
            _controller.UpdateItemColor(navalBaseRelativeTextBox, settings);
            _controller.UpdateItemColor(radarStationCurrentTextBox, settings);
            _controller.UpdateItemColor(radarStationMaxTextBox, settings);
            _controller.UpdateItemColor(radarStationRelativeTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorCurrentTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorMaxTextBox, settings);
            _controller.UpdateItemColor(nuclearReactorRelativeTextBox, settings);
            _controller.UpdateItemColor(rocketTestCurrentTextBox, settings);
            _controller.UpdateItemColor(rocketTestMaxTextBox, settings);
            _controller.UpdateItemColor(rocketTestRelativeTextBox, settings);
            _controller.UpdateItemColor(syntheticOilCurrentTextBox, settings);
            _controller.UpdateItemColor(syntheticOilMaxTextBox, settings);
            _controller.UpdateItemColor(syntheticOilRelativeTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresCurrentTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresMaxTextBox, settings);
            _controller.UpdateItemColor(syntheticRaresRelativeTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerCurrentTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerMaxTextBox, settings);
            _controller.UpdateItemColor(nuclearPowerRelativeTextBox, settings);
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearProvinceBuildingItems()
        {
            icCurrentTextBox.Text = "";
            icMaxTextBox.Text = "";
            icRelativeTextBox.Text = "";
            infrastructureCurrentTextBox.Text = "";
            infrastructureMaxTextBox.Text = "";
            infrastructureRelativeTextBox.Text = "";
            landFortCurrentTextBox.Text = "";
            landFortMaxTextBox.Text = "";
            landFortRelativeTextBox.Text = "";
            coastalFortCurrentTextBox.Text = "";
            coastalFortMaxTextBox.Text = "";
            coastalFortRelativeTextBox.Text = "";
            antiAirCurrentTextBox.Text = "";
            antiAirMaxTextBox.Text = "";
            antiAirRelativeTextBox.Text = "";
            airBaseCurrentTextBox.Text = "";
            airBaseMaxTextBox.Text = "";
            airBaseRelativeTextBox.Text = "";
            navalBaseCurrentTextBox.Text = "";
            navalBaseMaxTextBox.Text = "";
            navalBaseRelativeTextBox.Text = "";
            radarStationCurrentTextBox.Text = "";
            radarStationMaxTextBox.Text = "";
            radarStationRelativeTextBox.Text = "";
            nuclearReactorCurrentTextBox.Text = "";
            nuclearReactorMaxTextBox.Text = "";
            nuclearReactorRelativeTextBox.Text = "";
            rocketTestCurrentTextBox.Text = "";
            rocketTestMaxTextBox.Text = "";
            rocketTestRelativeTextBox.Text = "";
            syntheticOilCurrentTextBox.Text = "";
            syntheticOilMaxTextBox.Text = "";
            syntheticOilRelativeTextBox.Text = "";
            syntheticRaresCurrentTextBox.Text = "";
            syntheticRaresMaxTextBox.Text = "";
            syntheticRaresRelativeTextBox.Text = "";
            nuclearPowerCurrentTextBox.Text = "";
            nuclearPowerMaxTextBox.Text = "";
            nuclearPowerRelativeTextBox.Text = "";
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を有効化する
        /// </summary>
        private void EnableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = true;

            bool flag = Game.Type == GameType.ArsenalOfDemocracy;
            provinceSyntheticOilLabel.Enabled = flag;
            syntheticOilCurrentTextBox.Enabled = flag;
            syntheticOilMaxTextBox.Enabled = flag;
            syntheticOilRelativeTextBox.Enabled = flag;
            provinceSyntheticRaresLabel.Enabled = flag;
            syntheticRaresCurrentTextBox.Enabled = flag;
            syntheticRaresMaxTextBox.Enabled = flag;
            syntheticRaresRelativeTextBox.Enabled = flag;
            provinceNuclearPowerLabel.Enabled = flag;
            nuclearPowerCurrentTextBox.Enabled = flag;
            nuclearPowerMaxTextBox.Enabled = flag;
            nuclearPowerRelativeTextBox.Enabled = flag;
        }

        /// <summary>
        ///     プロヴィンス建物情報の編集項目を無効化する
        /// </summary>
        private void DisableProvinceBuildingItems()
        {
            provinceBuildingGroupBox.Enabled = false;
        }

        #endregion

        #region プロヴィンスタブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

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

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

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
        private void OnProvinceDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

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

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, settings);

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
        private void OnProvinceStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, province, settings);
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

            if (settings == null)
            {
                settings = new ProvinceSettings { Id = province.Id };
                Scenarios.AddProvinceSettings(settings);
            }

            _controller.OutputItemValueChangedLog(itemId, val, province, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, province, settings);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProvinceCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Province province = GetSelectedProvince();
            if (province == null)
            {
                return;
            }
            Country country = GetSelectedProvinceCountry();
            if (country == Country.None)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 初期値から変更されていなければ何もしない
            bool val = control.Checked;
            if ((settings == null) && !val)
            {
                return;
            }

            // 値に変化がなければ何もしない
            object prev = _controller.GetItemValue(itemId, province, settings);
            if ((prev != null) && (val == (bool) prev))
            {
                return;
            }

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            _controller.OutputItemValueChangedLog(itemId, val, province, settings);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, province, settings);

            // 値を更新する
            _controller.SetItemValue(itemId, val, province, settings);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, province, settings);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, province, settings);
        }

        #endregion
    }
}