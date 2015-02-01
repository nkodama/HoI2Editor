using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Controller
{
    /// <summary>
    ///     シナリオエディタのコントローラクラス
    /// </summary>
    public class ScenarioEditorController
    {
        #region 内部フィールド

        #region 共通

        /// <summary>
        ///     シナリオエディタのフォーム
        /// </summary>
        private readonly ScenarioEditorForm _form;

        /// <summary>
        ///     マップパネルのコントローラ
        /// </summary>
        private readonly MapPanelController _mapPanelController;

        #endregion

        #region 閣僚候補リスト

        /// <summary>
        ///     国家元首リスト
        /// </summary>
        private List<Minister> _headOfStateList;

        /// <summary>
        ///     政府首班リスト
        /// </summary>
        private List<Minister> _headOfGovernmentList;

        /// <summary>
        ///     外務大臣リスト
        /// </summary>
        private List<Minister> _foreignMinisterList;

        /// <summary>
        ///     軍需大臣リスト
        /// </summary>
        private List<Minister> _armamentMinisterList;

        /// <summary>
        ///     内務大臣リスト
        /// </summary>
        private List<Minister> _ministerOfSecurityList;

        /// <summary>
        ///     情報大臣リスト
        /// </summary>
        private List<Minister> _ministerOfIntelligenceList;

        /// <summary>
        ///     統合参謀総長リスト
        /// </summary>
        private List<Minister> _chiefOfStaffList;

        /// <summary>
        ///     陸軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfArmyList;

        /// <summary>
        ///     海軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfNavyList;

        /// <summary>
        ///     空軍総司令官リスト
        /// </summary>
        private List<Minister> _chiefOfAirList;

        #endregion

        #region 陸地プロヴィンスリスト

        /// <summary>
        ///     陸地プロヴィンスリスト
        /// </summary>
        private readonly List<Province> _landProvinces = new List<Province>();

        #endregion

        #endregion

        #region 内部定数

        /// <summary>
        ///     編集項目の編集済みフラグ
        /// </summary>
        private static readonly object[] ItemDirtyFlags =
        {
            Treaty.ItemId.StartYear,
            Treaty.ItemId.StartMonth,
            Treaty.ItemId.StartDay,
            Treaty.ItemId.EndYear,
            Treaty.ItemId.EndMonth,
            Treaty.ItemId.EndDay,
            Treaty.ItemId.Type,
            Treaty.ItemId.Id,
            Treaty.ItemId.Cancel,
            Treaty.ItemId.Country1,
            Treaty.ItemId.Country2,
            Treaty.ItemId.Energy,
            Treaty.ItemId.Energy,
            Treaty.ItemId.Metal,
            Treaty.ItemId.Metal,
            Treaty.ItemId.RareMaterials,
            Treaty.ItemId.RareMaterials,
            Treaty.ItemId.Oil,
            Treaty.ItemId.Oil,
            Treaty.ItemId.Supplies,
            Treaty.ItemId.Supplies,
            Treaty.ItemId.Money,
            Treaty.ItemId.Money,
            CountrySettings.ItemId.Name,
            CountrySettings.ItemId.FlagExt,
            CountrySettings.ItemId.RegularId,
            CountrySettings.ItemId.Belligerence,
            CountrySettings.ItemId.Dissent,
            CountrySettings.ItemId.ExtraTc,
            CountrySettings.ItemId.Nuke,
            CountrySettings.ItemId.NukeYear,
            CountrySettings.ItemId.NukeMonth,
            CountrySettings.ItemId.NukeDay,
            CountrySettings.ItemId.GroundDefEff,
            CountrySettings.ItemId.PeacetimeIcModifier,
            CountrySettings.ItemId.WartimeIcModifier,
            CountrySettings.ItemId.IndustrialModifier,
            CountrySettings.ItemId.RelativeManpower,
            CountrySettings.ItemId.Energy,
            CountrySettings.ItemId.Metal,
            CountrySettings.ItemId.RareMaterials,
            CountrySettings.ItemId.Oil,
            CountrySettings.ItemId.Supplies,
            CountrySettings.ItemId.Money,
            CountrySettings.ItemId.Transports,
            CountrySettings.ItemId.Escorts,
            CountrySettings.ItemId.Manpower,
            CountrySettings.ItemId.OffmapEnergy,
            CountrySettings.ItemId.OffmapMetal,
            CountrySettings.ItemId.OffmapRareMaterials,
            CountrySettings.ItemId.OffmapOil,
            CountrySettings.ItemId.OffmapSupplies,
            CountrySettings.ItemId.OffmapMoney,
            CountrySettings.ItemId.OffmapTransports,
            CountrySettings.ItemId.OffmapEscorts,
            CountrySettings.ItemId.OffmapManpower,
            CountrySettings.ItemId.OffmapIc,
            CountrySettings.ItemId.AiFileName,
            CountrySettings.ItemId.SliderYear,
            CountrySettings.ItemId.SliderMonth,
            CountrySettings.ItemId.SliderDay,
            CountrySettings.ItemId.Democratic,
            CountrySettings.ItemId.PoliticalLeft,
            CountrySettings.ItemId.Freedom,
            CountrySettings.ItemId.FreeMarket,
            CountrySettings.ItemId.ProfessionalArmy,
            CountrySettings.ItemId.DefenseLobby,
            CountrySettings.ItemId.Interventionism,
            CountrySettings.ItemId.HeadOfStateId,
            CountrySettings.ItemId.HeadOfStateType,
            CountrySettings.ItemId.HeadOfStateId,
            CountrySettings.ItemId.HeadOfGovernmentId,
            CountrySettings.ItemId.HeadOfGovernmentType,
            CountrySettings.ItemId.HeadOfGovernmentId,
            CountrySettings.ItemId.ForeignMinisterId,
            CountrySettings.ItemId.ForeignMinisterType,
            CountrySettings.ItemId.ForeignMinisterId,
            CountrySettings.ItemId.ArmamentMinisterId,
            CountrySettings.ItemId.ArmamentMinisterType,
            CountrySettings.ItemId.ArmamentMinisterId,
            CountrySettings.ItemId.MinisterOfSecurityId,
            CountrySettings.ItemId.MinisterOfSecurityType,
            CountrySettings.ItemId.MinisterOfSecurityId,
            CountrySettings.ItemId.MinisterOfIntelligenceId,
            CountrySettings.ItemId.MinisterOfIntelligenceType,
            CountrySettings.ItemId.MinisterOfIntelligenceId,
            CountrySettings.ItemId.ChiefOfStaffId,
            CountrySettings.ItemId.ChiefOfStaffType,
            CountrySettings.ItemId.ChiefOfStaffId,
            CountrySettings.ItemId.ChiefOfArmyId,
            CountrySettings.ItemId.ChiefOfArmyType,
            CountrySettings.ItemId.ChiefOfArmyId,
            CountrySettings.ItemId.ChiefOfNavyId,
            CountrySettings.ItemId.ChiefOfNavyType,
            CountrySettings.ItemId.ChiefOfNavyId,
            CountrySettings.ItemId.ChiefOfAirId,
            CountrySettings.ItemId.ChiefOfAirType,
            CountrySettings.ItemId.ChiefOfAirId,
            CountrySettings.ItemId.Capital,
            null,
            null,
            null,
            null,
            null,
            null,
            ProvinceSettings.ItemId.Vp,
            ProvinceSettings.ItemId.RevoltRisk,
            ProvinceSettings.ItemId.Manpower,
            ProvinceSettings.ItemId.MaxManpower,
            ProvinceSettings.ItemId.EnergyPool,
            ProvinceSettings.ItemId.Energy,
            ProvinceSettings.ItemId.MaxEnergy,
            ProvinceSettings.ItemId.MetalPool,
            ProvinceSettings.ItemId.Metal,
            ProvinceSettings.ItemId.MaxMetal,
            ProvinceSettings.ItemId.RareMaterialsPool,
            ProvinceSettings.ItemId.RareMaterials,
            ProvinceSettings.ItemId.MaxRareMaterials,
            ProvinceSettings.ItemId.OilPool,
            ProvinceSettings.ItemId.Oil,
            ProvinceSettings.ItemId.MaxOil,
            ProvinceSettings.ItemId.SupplyPool,
            ProvinceSettings.ItemId.Ic,
            ProvinceSettings.ItemId.MaxIc,
            ProvinceSettings.ItemId.RelativeIc,
            ProvinceSettings.ItemId.Infrastructure,
            ProvinceSettings.ItemId.MaxInfrastructure,
            ProvinceSettings.ItemId.RelativeInfrastructure,
            ProvinceSettings.ItemId.LandFort,
            ProvinceSettings.ItemId.MaxLandFort,
            ProvinceSettings.ItemId.RelativeLandFort,
            ProvinceSettings.ItemId.CoastalFort,
            ProvinceSettings.ItemId.MaxCoastalFort,
            ProvinceSettings.ItemId.RelativeCoastalFort,
            ProvinceSettings.ItemId.AntiAir,
            ProvinceSettings.ItemId.MaxAntiAir,
            ProvinceSettings.ItemId.RelativeAntiAir,
            ProvinceSettings.ItemId.AirBase,
            ProvinceSettings.ItemId.MaxAirBase,
            ProvinceSettings.ItemId.RelativeAirBase,
            ProvinceSettings.ItemId.NavalBase,
            ProvinceSettings.ItemId.MaxNavalBase,
            ProvinceSettings.ItemId.RelativeNavalBase,
            ProvinceSettings.ItemId.RadarStation,
            ProvinceSettings.ItemId.MaxRadarStation,
            ProvinceSettings.ItemId.RelativeRadarStation,
            ProvinceSettings.ItemId.NuclearReactor,
            ProvinceSettings.ItemId.MaxNuclearReactor,
            ProvinceSettings.ItemId.RelativeNuclearReactor,
            ProvinceSettings.ItemId.RocketTest,
            ProvinceSettings.ItemId.MaxRocketTest,
            ProvinceSettings.ItemId.RelativeRocketTest,
            ProvinceSettings.ItemId.SyntheticOil,
            ProvinceSettings.ItemId.MaxSyntheticOil,
            ProvinceSettings.ItemId.RelativeSyntheticOil,
            ProvinceSettings.ItemId.SyntheticRares,
            ProvinceSettings.ItemId.MaxSyntheticRares,
            ProvinceSettings.ItemId.RelativeSyntheticRares,
            ProvinceSettings.ItemId.NuclearPower,
            ProvinceSettings.ItemId.MaxNuclearPower,
            ProvinceSettings.ItemId.RelativeNuclearPower
        };

        /// <summary>
        ///     編集項目の文字列
        /// </summary>
        private static readonly string[] ItemStrings =
        {
            "trade start year",
            "trade start month",
            "trade start day",
            "trade end year",
            "trade end month",
            "trade end day",
            "trade type",
            "trade id",
            "trade cancel",
            "trade country1",
            "trade country2",
            "trade energy",
            "trade energy",
            "trade metal",
            "trade metal",
            "trade rare materials",
            "trade rare materials",
            "trade oil",
            "trade oil",
            "trade supplies",
            "trade supplies",
            "trade money",
            "trade money",
            "country name",
            "country flag ext",
            "country regular id",
            "country belligerence",
            "country dissent",
            "country extra tc",
            "country nuke",
            "country nuke year",
            "country nuke month",
            "country nuke day",
            "country ground def eff",
            "country peacetime ic modifier",
            "country wartime ic modifier",
            "country industrial modifier",
            "country relative manpower",
            "country energy",
            "country metal",
            "country rare materials",
            "country oil",
            "country supplies",
            "country money",
            "country transports",
            "country escorts",
            "country manpower",
            "country offmap energy",
            "country offmap metal",
            "country offmap rare materials",
            "country offmap oil",
            "country offmap supplies",
            "country offmap money",
            "country offmap transports",
            "country offmap escorts",
            "country offmap manpower",
            "country offmap ic",
            "country ai file",
            "slider year",
            "slider month",
            "slider day",
            "democratic",
            "political left",
            "freedom",
            "free market",
            "professional army",
            "defense lobby",
            "interventionism",
            "head of state id",
            "head of state type",
            "head of state id",
            "head of government id",
            "head of government type",
            "head of government id",
            "foreign minister id",
            "foreign minister type",
            "foreign minister id",
            "armament minister id",
            "armament minister type",
            "armament minister id",
            "minister of security id",
            "minister of security type",
            "minister of security id",
            "minister of intelligence id",
            "minister of intelligence type",
            "minister of intelligence id",
            "chief of staff id",
            "chief of staff type",
            "chief of staff id",
            "chief of army id",
            "chief of army type",
            "chief of army id",
            "chief of navy id",
            "chief of navy type",
            "chief of navy id",
            "chief of air id",
            "chief of air type",
            "chief of air id",
            "capital",
            "core provinces",
            "owned provinces",
            "controlled provinces",
            "claimed provinces",
            "province id",
            "province name",
            "province vp",
            "province revolt risk",
            "province manpower",
            "province max manpower",
            "province energy pool",
            "province energy",
            "province max energy",
            "province metal pool",
            "province metal",
            "province max metal",
            "province rare materials pool",
            "province rare materials",
            "province max rare materials",
            "province oil pool",
            "province oil",
            "province max oil",
            "province supply pool",
            "province ic",
            "province max ic",
            "province relative ic",
            "province infrastructure",
            "province max infrastructure",
            "province relative infrastructure",
            "province land fort",
            "province max land fort",
            "province relative land fort",
            "province coastal fort",
            "province max coastal fort",
            "province relative coastal fort",
            "province anti air",
            "province max anti air",
            "province relative anti air",
            "province air base",
            "province max air base",
            "province relative air base",
            "province naval base",
            "province max naval base",
            "province relative naval base",
            "province radar station",
            "province max radar station",
            "province relative radar station",
            "province nuclear reactor",
            "province max nuclear reactor",
            "province relative nuclear reactor",
            "province rocket test",
            "province max rocket test",
            "province relative rocket test",
            "province synthetic oil",
            "province max synthetic oil",
            "province relative synthetic oil",
            "province synthetic rares",
            "province max synthetic rares",
            "province relative synthetic rares",
            "province nuclear power",
            "province max nuclear power",
            "province relative nuclear power"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="form">シナリオエディタのフォーム</param>
        /// <param name="mapPanelController">マップパネルのコントローラ</param>
        public ScenarioEditorController(ScenarioEditorForm form, MapPanelController mapPanelController)
        {
            _form = form;
            _mapPanelController = mapPanelController;
        }

        #endregion

        #region 閣僚候補リスト

        /// <summary>
        ///     閣僚候補リストを更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="year">現在年</param>
        public void UpdateMinisterList(Country country, int year)
        {
            List<Minister> ministers = Misc.EnableRetirementYearMinisters
                ? Ministers.Items.Where(
                    minister => (minister.Country == country) &&
                                (year >= minister.StartYear) &&
                                (year < minister.EndYear) &&
                                (year < minister.RetirementYear)).ToList()
                : Ministers.Items.Where(
                    minister => (minister.Country == country) &&
                                (year >= minister.StartYear) &&
                                (year < minister.EndYear)).ToList();

            _headOfStateList = ministers.Where(minister => minister.Position == MinisterPosition.HeadOfState).ToList();
            _headOfGovernmentList =
                ministers.Where(minister => minister.Position == MinisterPosition.HeadOfGovernment).ToList();
            _foreignMinisterList =
                ministers.Where(minister => minister.Position == MinisterPosition.ForeignMinister).ToList();
            _armamentMinisterList =
                ministers.Where(minister => minister.Position == MinisterPosition.MinisterOfArmament).ToList();
            _ministerOfSecurityList =
                ministers.Where(minister => minister.Position == MinisterPosition.MinisterOfSecurity).ToList();
            _ministerOfIntelligenceList =
                ministers.Where(minister => minister.Position == MinisterPosition.HeadOfMilitaryIntelligence).ToList();
            _chiefOfStaffList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfStaff).ToList();
            _chiefOfArmyList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfArmy).ToList();
            _chiefOfNavyList = ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfNavy).ToList();
            _chiefOfAirList =
                ministers.Where(minister => minister.Position == MinisterPosition.ChiefOfAirForce).ToList();
        }

        #endregion

        #region 陸地プロヴィンスリスト

        /// <summary>
        ///     陸地プロヴィンスリストを初期化する
        /// </summary>
        /// <param name="control">プロヴィンスリストビュー</param>
        public void InitProvinceList(ListView control)
        {
            control.BeginUpdate();
            control.Items.Clear();
            _landProvinces.Clear();
            foreach (Province province in Provinces.Items.Where(province => province.IsLand && (province.Id > 0)))
            {
                _landProvinces.Add(province);
                ListViewItem item = CreateProvinceListItem(province);
                _form.AddProvinceListItem(item);
            }
            control.EndUpdate();
        }

        /// <summary>
        ///     プロヴィンスリストを更新する
        /// </summary>
        public void UpdateProvinceList(ListView control, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // プロヴィンスリストビューを更新する
            control.BeginUpdate();
            if (settings != null)
            {
                foreach (ListViewItem item in control.Items)
                {
                    Province province = (Province) item.Tag;
                    item.SubItems[2].Text = (province.Id == settings.Capital) ? Resources.Yes : "";
                    item.SubItems[3].Text = settings.NationalProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[4].Text = settings.OwnedProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[5].Text = settings.ControlledProvinces.Contains(province.Id) ? Resources.Yes : "";
                    item.SubItems[6].Text = settings.ClaimedProvinces.Contains(province.Id) ? Resources.Yes : "";
                }
            }
            else
            {
                foreach (ListViewItem item in control.Items)
                {
                    item.SubItems[2].Text = "";
                    item.SubItems[3].Text = "";
                    item.SubItems[4].Text = "";
                    item.SubItems[5].Text = "";
                    item.SubItems[6].Text = "";
                }
            }
            control.EndUpdate();
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
        ///     陸地プロヴィンスリストのインデックスを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>陸地プロヴィンスリストのインデックス</returns>
        public int GetLandProvinceIndex(int id)
        {
            return _landProvinces.FindIndex(province => province.Id == id);
        }

        #endregion

        #region 編集項目

        #region 編集項目 - 項目値更新

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemValue(TextBox control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = GetItemValue(itemId, treaty);
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeEnergy1:
                case ScenarioEditorItemId.TradeEnergy2:
                case ScenarioEditorItemId.TradeMetal1:
                case ScenarioEditorItemId.TradeMetal2:
                case ScenarioEditorItemId.TradeRareMaterials1:
                case ScenarioEditorItemId.TradeRareMaterials2:
                case ScenarioEditorItemId.TradeOil1:
                case ScenarioEditorItemId.TradeOil2:
                case ScenarioEditorItemId.TradeSupplies1:
                case ScenarioEditorItemId.TradeSupplies2:
                case ScenarioEditorItemId.TradeMoney1:
                case ScenarioEditorItemId.TradeMoney2:
                    control.Text = DoubleHelper.IsPositive((double) val) ? DoubleHelper.ToString((double) val) : "";
                    break;

                default:
                    control.Text = ObjectHelper.ToString(val);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemValue(ComboBox control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.SelectedIndex = Array.IndexOf(Countries.Tags, (Country) GetItemValue(itemId, treaty));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemValue(CheckBox control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Checked = (bool) GetItemValue(itemId, treaty);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(TextBox control, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, settings));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(ComboBox control, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryRegularId:
                    Country country = (Country) (GetItemValue(itemId, settings) ?? Country.None);
                    control.SelectedIndex = Array.IndexOf(Countries.Tags, country) + 1;
                    break;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfAir:
                    List<Minister> ministers = (List<Minister>) GetListItems(itemId);
                    control.BeginUpdate();
                    control.Items.Clear();
                    foreach (Minister minister in ministers)
                    {
                        control.Items.Add(minister.Name);
                    }
                    control.EndUpdate();
                    object val = GetItemValue(itemId, settings);
                    if (val != null)
                    {
                        control.SelectedIndex = ministers.FindIndex(minister => minister.Id == (int) val);
                    }
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(TrackBar control, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = GetItemValue(itemId, settings);
            if (val == null)
            {
                control.Value = 6;
                return;
            }

            if ((int) val < 1)
            {
                val = 1;
            }
            else if ((int) val > 10)
            {
                val = 10;
            }
            control.Value = 11 - (int) val;
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(TextBox control, Country country, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = (string) GetItemValue(itemId, country, settings);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(CheckBox control, Province province, CountrySettings settings)
        {
            if (settings == null)
            {
                control.Checked = false;
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    control.Checked = (settings.Capital == province.Id);
                    control.Enabled = !control.Checked;
                    break;

                case ScenarioEditorItemId.CountryCoreProvinces:
                    control.Checked = settings.NationalProvinces.Contains(province.Id);
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    control.Checked = settings.OwnedProvinces.Contains(province.Id);
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    control.Checked = settings.ControlledProvinces.Contains(province.Id);
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    control.Checked = settings.ClaimedProvinces.Contains(province.Id);
                    control.Enabled = (Game.Type == GameType.DarkestHour);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        public void UpdateItemValue(TextBox control, Province province)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, province));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemValue(TextBox control, ProvinceSettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, settings));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemValue(TextBox control, Province province, ProvinceSettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, province, settings));
        }

        #endregion

        #region 編集項目 - 項目色更新

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemColor(TextBox control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, treaty) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemColor(CheckBox control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, treaty) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemColor(TextBox control, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemColor(TextBox control, Country country, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, country, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemColor(CheckBox control, Province province, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, province, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemColor(TextBox control, ProvinceSettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemColor(TextBox control, Province province, ProvinceSettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, province, settings) ? Color.Red : SystemColors.WindowText;
        }

        #endregion

        #region 編集項目 - 項目値取得

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="treaty">協定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeStartYear:
                    if (treaty.StartDate == null)
                    {
                        return null;
                    }
                    return treaty.StartDate.Year;

                case ScenarioEditorItemId.TradeStartMonth:
                    if (treaty.StartDate == null)
                    {
                        return null;
                    }
                    return treaty.StartDate.Month;

                case ScenarioEditorItemId.TradeStartDay:
                    if (treaty.StartDate == null)
                    {
                        return null;
                    }
                    return treaty.StartDate.Day;

                case ScenarioEditorItemId.TradeEndYear:
                    if (treaty.EndDate == null)
                    {
                        return null;
                    }
                    return treaty.EndDate.Year;

                case ScenarioEditorItemId.TradeEndMonth:
                    if (treaty.EndDate == null)
                    {
                        return null;
                    }
                    return treaty.EndDate.Month;

                case ScenarioEditorItemId.TradeEndDay:
                    if (treaty.EndDate == null)
                    {
                        return null;
                    }
                    return treaty.EndDate.Day;

                case ScenarioEditorItemId.TradeType:
                    if (treaty.Id == null)
                    {
                        return null;
                    }
                    return treaty.Id.Type;

                case ScenarioEditorItemId.TradeId:
                    if (treaty.Id == null)
                    {
                        return null;
                    }
                    return treaty.Id.Id;

                case ScenarioEditorItemId.TradeCancel:
                    return treaty.Cancel;

                case ScenarioEditorItemId.TradeCountry1:
                    return treaty.Country1;

                case ScenarioEditorItemId.TradeCountry2:
                    return treaty.Country2;

                case ScenarioEditorItemId.TradeEnergy1:
                    return -treaty.Energy;

                case ScenarioEditorItemId.TradeEnergy2:
                    return treaty.Energy;

                case ScenarioEditorItemId.TradeMetal1:
                    return -treaty.Metal;

                case ScenarioEditorItemId.TradeMetal2:
                    return treaty.Metal;

                case ScenarioEditorItemId.TradeRareMaterials1:
                    return -treaty.RareMaterials;

                case ScenarioEditorItemId.TradeRareMaterials2:
                    return treaty.RareMaterials;

                case ScenarioEditorItemId.TradeOil1:
                    return -treaty.Oil;

                case ScenarioEditorItemId.TradeOil2:
                    return treaty.Oil;

                case ScenarioEditorItemId.TradeSupplies1:
                    return -treaty.Supplies;

                case ScenarioEditorItemId.TradeSupplies2:
                    return treaty.Supplies;

                case ScenarioEditorItemId.TradeMoney1:
                    return -treaty.Money;

                case ScenarioEditorItemId.TradeMoney2:
                    return treaty.Money;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, CountrySettings settings)
        {
            if (settings == null)
            {
                return null;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.CountryFlagExt:
                    return settings.FlagExt;

                case ScenarioEditorItemId.CountryRegularId:
                    return settings.RegularId;

                case ScenarioEditorItemId.CountryBelligerence:
                    return settings.Belligerence;

                case ScenarioEditorItemId.CountryDissent:
                    return settings.Dissent;

                case ScenarioEditorItemId.CountryExtraTc:
                    return settings.ExtraTc;

                case ScenarioEditorItemId.CountryNuke:
                    return settings.Nuke;

                case ScenarioEditorItemId.CountryNukeYear:
                    if (settings.NukeDate == null)
                    {
                        return null;
                    }
                    return settings.NukeDate.Year;

                case ScenarioEditorItemId.CountryNukeMonth:
                    if (settings.NukeDate == null)
                    {
                        return null;
                    }
                    return settings.NukeDate.Month;

                case ScenarioEditorItemId.CountryNukeDay:
                    if (settings.NukeDate == null)
                    {
                        return null;
                    }
                    return settings.NukeDate.Day;

                case ScenarioEditorItemId.CountryGroundDefEff:
                    return settings.GroundDefEff;

                case ScenarioEditorItemId.CountryPeacetimeIcModifier:
                    return settings.PeacetimeIcModifier;

                case ScenarioEditorItemId.CountryWartimeIcModifier:
                    return settings.WartimeIcModifier;

                case ScenarioEditorItemId.CountryIndustrialModifier:
                    return settings.IndustrialModifier;

                case ScenarioEditorItemId.CountryRelativeManpower:
                    return settings.RelativeManpower;

                case ScenarioEditorItemId.CountryEnergy:
                    return settings.Energy;

                case ScenarioEditorItemId.CountryMetal:
                    return settings.Metal;

                case ScenarioEditorItemId.CountryRareMaterials:
                    return settings.RareMaterials;

                case ScenarioEditorItemId.CountryOil:
                    return settings.Oil;

                case ScenarioEditorItemId.CountrySupplies:
                    return settings.Supplies;

                case ScenarioEditorItemId.CountryMoney:
                    return settings.Money;

                case ScenarioEditorItemId.CountryTransports:
                    return settings.Transports;

                case ScenarioEditorItemId.CountryEscorts:
                    return settings.Escorts;

                case ScenarioEditorItemId.CountryManpower:
                    return settings.Manpower;

                case ScenarioEditorItemId.CountryOffmapEnergy:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Energy;

                case ScenarioEditorItemId.CountryOffmapMetal:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Metal;

                case ScenarioEditorItemId.CountryOffmapRareMaterials:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.RareMaterials;

                case ScenarioEditorItemId.CountryOffmapOil:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Oil;

                case ScenarioEditorItemId.CountryOffmapSupplies:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Supplies;

                case ScenarioEditorItemId.CountryOffmapMoney:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Money;

                case ScenarioEditorItemId.CountryOffmapTransports:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Transports;

                case ScenarioEditorItemId.CountryOffmapEscorts:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Escorts;

                case ScenarioEditorItemId.CountryOffmapManpower:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Manpower;

                case ScenarioEditorItemId.CountryOffmapIc:
                    if (settings.Offmap == null)
                    {
                        return null;
                    }
                    return settings.Offmap.Ic;

                case ScenarioEditorItemId.CountryAiFileName:
                    return settings.AiFileName;

                case ScenarioEditorItemId.SliderYear:
                    if ((settings.Policy == null) || (settings.Policy.Date == null))
                    {
                        return null;
                    }
                    return settings.Policy.Date.Year;

                case ScenarioEditorItemId.SliderMonth:
                    if ((settings.Policy == null) || (settings.Policy.Date == null))
                    {
                        return null;
                    }
                    return settings.Policy.Date.Month;

                case ScenarioEditorItemId.SliderDay:
                    if ((settings.Policy == null) || (settings.Policy.Date == null))
                    {
                        return null;
                    }
                    return settings.Policy.Date.Day;

                case ScenarioEditorItemId.SliderDemocratic:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.Democratic;

                case ScenarioEditorItemId.SliderPoliticalLeft:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.PoliticalLeft;

                case ScenarioEditorItemId.SliderFreedom:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.Freedom;

                case ScenarioEditorItemId.SliderFreeMarket:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.FreeMarket;

                case ScenarioEditorItemId.SliderProfessionalArmy:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.ProfessionalArmy;

                case ScenarioEditorItemId.SliderDefenseLobby:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.DefenseLobby;

                case ScenarioEditorItemId.SliderInterventionism:
                    if (settings.Policy == null)
                    {
                        return null;
                    }
                    return settings.Policy.Interventionism;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    if (settings.HeadOfState == null)
                    {
                        return null;
                    }
                    return settings.HeadOfState.Id;

                case ScenarioEditorItemId.CabinetHeadOfStateType:
                    if (settings.HeadOfState == null)
                    {
                        return null;
                    }
                    return settings.HeadOfState.Type;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    if (settings.HeadOfGovernment == null)
                    {
                        return null;
                    }
                    return settings.HeadOfGovernment.Id;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    if (settings.HeadOfGovernment == null)
                    {
                        return null;
                    }
                    return settings.HeadOfGovernment.Type;

                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    if (settings.ForeignMinister == null)
                    {
                        return null;
                    }
                    return settings.ForeignMinister.Id;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    if (settings.ForeignMinister == null)
                    {
                        return null;
                    }
                    return settings.ForeignMinister.Type;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    if (settings.ArmamentMinister == null)
                    {
                        return null;
                    }
                    return settings.ArmamentMinister.Id;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    if (settings.ArmamentMinister == null)
                    {
                        return null;
                    }
                    return settings.ArmamentMinister.Type;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    if (settings.MinisterOfSecurity == null)
                    {
                        return null;
                    }
                    return settings.MinisterOfSecurity.Id;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    if (settings.MinisterOfSecurity == null)
                    {
                        return null;
                    }
                    return settings.MinisterOfSecurity.Type;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    if (settings.MinisterOfIntelligence == null)
                    {
                        return null;
                    }
                    return settings.MinisterOfIntelligence.Id;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    if (settings.MinisterOfIntelligence == null)
                    {
                        return null;
                    }
                    return settings.MinisterOfIntelligence.Type;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    if (settings.ChiefOfStaff == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfStaff.Id;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    if (settings.ChiefOfStaff == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfStaff.Type;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    if (settings.ChiefOfArmy == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfArmy.Id;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    if (settings.ChiefOfArmy == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfArmy.Type;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    if (settings.ChiefOfNavy == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfNavy.Id;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    if (settings.ChiefOfNavy == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfNavy.Type;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    if (settings.ChiefOfAir == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfAir.Id;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    if (settings.ChiefOfAir == null)
                    {
                        return null;
                    }
                    return settings.ChiefOfAir.Type;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Country country, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryName:
                    return ((settings != null) && !string.IsNullOrEmpty(settings.Name))
                        ? Config.GetText(settings.Name)
                        : Countries.GetName(country);
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Province province, CountrySettings settings)
        {
            if (settings == null)
            {
                return null;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    return (settings.Capital == province.Id);

                case ScenarioEditorItemId.CountryCoreProvinces:
                    return settings.NationalProvinces.Contains(province.Id);

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    return settings.OwnedProvinces.Contains(province.Id);

                case ScenarioEditorItemId.CountryControlledProvinces:
                    return settings.ControlledProvinces.Contains(province.Id);

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    return settings.ClaimedProvinces.Contains(province.Id);
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Province province)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceId:
                    return province.Id;
            }
            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, ProvinceSettings settings)
        {
            if (settings == null)
            {
                return null;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceVp:
                    return settings.Vp;

                case ScenarioEditorItemId.ProvinceRevoltRisk:
                    return settings.RevoltRisk;

                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                    return settings.Manpower;

                case ScenarioEditorItemId.ProvinceManpowerMax:
                    return settings.MaxManpower;

                case ScenarioEditorItemId.ProvinceEnergyPool:
                    return settings.EnergyPool;

                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                    return settings.Energy;

                case ScenarioEditorItemId.ProvinceEnergyMax:
                    return settings.MaxEnergy;

                case ScenarioEditorItemId.ProvinceMetalPool:
                    return settings.MetalPool;

                case ScenarioEditorItemId.ProvinceMetalCurrent:
                    return settings.Metal;

                case ScenarioEditorItemId.ProvinceMetalMax:
                    return settings.MaxMetal;

                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                    return settings.RareMaterialsPool;

                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                    return settings.RareMaterials;

                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                    return settings.MaxRareMaterials;

                case ScenarioEditorItemId.ProvinceOilPool:
                    return settings.OilPool;

                case ScenarioEditorItemId.ProvinceOilCurrent:
                    return settings.Oil;

                case ScenarioEditorItemId.ProvinceOilMax:
                    return settings.MaxOil;

                case ScenarioEditorItemId.ProvinceSupplyPool:
                    return settings.SupplyPool;

                case ScenarioEditorItemId.ProvinceIcCurrent:
                    if (settings.Ic == null)
                    {
                        return null;
                    }
                    return settings.Ic.CurrentSize;

                case ScenarioEditorItemId.ProvinceIcMax:
                    if (settings.Ic == null)
                    {
                        return null;
                    }
                    return settings.Ic.MaxSize;

                case ScenarioEditorItemId.ProvinceIcRelative:
                    if (settings.Ic == null)
                    {
                        return null;
                    }
                    return settings.Ic.Size;

                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                    if (settings.Infrastructure == null)
                    {
                        return null;
                    }
                    return settings.Infrastructure.CurrentSize;

                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                    if (settings.Infrastructure == null)
                    {
                        return null;
                    }
                    return settings.Infrastructure.MaxSize;

                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                    if (settings.Infrastructure == null)
                    {
                        return null;
                    }
                    return settings.Infrastructure.Size;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                    if (settings.LandFort == null)
                    {
                        return null;
                    }
                    return settings.LandFort.CurrentSize;

                case ScenarioEditorItemId.ProvinceLandFortMax:
                    if (settings.LandFort == null)
                    {
                        return null;
                    }
                    return settings.LandFort.MaxSize;

                case ScenarioEditorItemId.ProvinceLandFortRelative:
                    if (settings.LandFort == null)
                    {
                        return null;
                    }
                    return settings.LandFort.Size;

                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                    if (settings.CoastalFort == null)
                    {
                        return null;
                    }
                    return settings.CoastalFort.CurrentSize;

                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                    if (settings.CoastalFort == null)
                    {
                        return null;
                    }
                    return settings.CoastalFort.MaxSize;

                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                    if (settings.CoastalFort == null)
                    {
                        return null;
                    }
                    return settings.CoastalFort.Size;

                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                    if (settings.AntiAir == null)
                    {
                        return null;
                    }
                    return settings.AntiAir.CurrentSize;

                case ScenarioEditorItemId.ProvinceAntiAirMax:
                    if (settings.AntiAir == null)
                    {
                        return null;
                    }
                    return settings.AntiAir.MaxSize;

                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                    if (settings.AntiAir == null)
                    {
                        return null;
                    }
                    return settings.AntiAir.Size;

                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                    if (settings.AirBase == null)
                    {
                        return null;
                    }
                    return settings.AirBase.CurrentSize;

                case ScenarioEditorItemId.ProvinceAirBaseMax:
                    if (settings.AirBase == null)
                    {
                        return null;
                    }
                    return settings.AirBase.MaxSize;

                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                    if (settings.AirBase == null)
                    {
                        return null;
                    }
                    return settings.AirBase.Size;

                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                    if (settings.NavalBase == null)
                    {
                        return null;
                    }
                    return settings.NavalBase.CurrentSize;

                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                    if (settings.NavalBase == null)
                    {
                        return null;
                    }
                    return settings.NavalBase.MaxSize;

                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                    if (settings.NavalBase == null)
                    {
                        return null;
                    }
                    return settings.NavalBase.Size;

                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                    if (settings.RadarStation == null)
                    {
                        return null;
                    }
                    return settings.RadarStation.CurrentSize;

                case ScenarioEditorItemId.ProvinceRadarStationMax:
                    if (settings.RadarStation == null)
                    {
                        return null;
                    }
                    return settings.RadarStation.MaxSize;

                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                    if (settings.RadarStation == null)
                    {
                        return null;
                    }
                    return settings.RadarStation.Size;

                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                    if (settings.NuclearReactor == null)
                    {
                        return null;
                    }
                    return settings.NuclearReactor.CurrentSize;

                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                    if (settings.NuclearReactor == null)
                    {
                        return null;
                    }
                    return settings.NuclearReactor.MaxSize;

                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                    if (settings.NuclearReactor == null)
                    {
                        return null;
                    }
                    return settings.NuclearReactor.Size;

                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                    if (settings.RocketTest == null)
                    {
                        return null;
                    }
                    return settings.RocketTest.CurrentSize;

                case ScenarioEditorItemId.ProvinceRocketTestMax:
                    if (settings.RocketTest == null)
                    {
                        return null;
                    }
                    return settings.RocketTest.MaxSize;

                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                    if (settings.RocketTest == null)
                    {
                        return null;
                    }
                    return settings.RocketTest.Size;

                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                    if (settings.SyntheticOil == null)
                    {
                        return null;
                    }
                    return settings.SyntheticOil.CurrentSize;

                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                    if (settings.SyntheticOil == null)
                    {
                        return null;
                    }
                    return settings.SyntheticOil.MaxSize;

                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                    if (settings.SyntheticOil == null)
                    {
                        return null;
                    }
                    return settings.SyntheticOil.Size;

                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                    if (settings.SyntheticRares == null)
                    {
                        return null;
                    }
                    return settings.SyntheticRares.CurrentSize;

                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                    if (settings.SyntheticRares == null)
                    {
                        return null;
                    }
                    return settings.SyntheticRares.MaxSize;

                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                    if (settings.SyntheticRares == null)
                    {
                        return null;
                    }
                    return settings.SyntheticRares.Size;

                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                    if (settings.NuclearPower == null)
                    {
                        return null;
                    }
                    return settings.NuclearPower.CurrentSize;

                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    if (settings.NuclearPower == null)
                    {
                        return null;
                    }
                    return settings.NuclearPower.MaxSize;

                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    if (settings.NuclearPower == null)
                    {
                        return null;
                    }
                    return settings.NuclearPower.Size;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Province province, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceName:
                    return Scenarios.GetProvinceName(province, settings);
            }

            return null;
        }

        /// <summary>
        ///     項目値のリストを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>項目値のリスト</returns>
        public object GetListItems(ScenarioEditorItemId itemId)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CabinetHeadOfState:
                    return _headOfStateList;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                    return _headOfGovernmentList;

                case ScenarioEditorItemId.CabinetForeignMinister:
                    return _foreignMinisterList;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                    return _armamentMinisterList;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                    return _ministerOfSecurityList;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                    return _ministerOfIntelligenceList;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                    return _chiefOfStaffList;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                    return _chiefOfArmyList;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                    return _chiefOfNavyList;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                    return _chiefOfAirList;
            }

            return null;
        }

        /// <summary>
        ///     リスト項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="index">リストのインデックス</param>
        /// <returns>リスト項目の値</returns>
        public object GetListItemValue(ScenarioEditorItemId itemId, int index)
        {
            if (index < 0)
            {
                return null;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.TradeCountry1:
                case ScenarioEditorItemId.TradeCountry2:
                    return Countries.Tags[index];

                case ScenarioEditorItemId.CountryRegularId:
                    return (index > 0) ? Countries.Tags[index - 1] : Country.None;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfAir:
                    return ((List<Minister>) GetListItems(itemId))[index].Id;
            }

            return null;
        }

        #endregion

        #region 編集項目 - 項目値設定

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeStartYear:
                    treaty.StartDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.TradeStartMonth:
                    treaty.StartDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.TradeStartDay:
                    treaty.StartDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.TradeEndYear:
                    treaty.EndDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.TradeEndMonth:
                    treaty.EndDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.TradeEndDay:
                    treaty.EndDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.TradeType:
                    Scenarios.SetType(treaty.Id, (int) val);
                    break;

                case ScenarioEditorItemId.TradeId:
                    Scenarios.SetId(treaty.Id, (int) val);
                    break;

                case ScenarioEditorItemId.TradeCancel:
                    treaty.Cancel = (bool) val;
                    break;

                case ScenarioEditorItemId.TradeCountry1:
                    treaty.Country1 = (Country) val;
                    break;

                case ScenarioEditorItemId.TradeCountry2:
                    treaty.Country2 = (Country) val;
                    break;

                case ScenarioEditorItemId.TradeEnergy1:
                    treaty.Energy = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeEnergy2:
                    treaty.Energy = (double) val;
                    break;

                case ScenarioEditorItemId.TradeMetal1:
                    treaty.Metal = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeMetal2:
                    treaty.Metal = (double) val;
                    break;

                case ScenarioEditorItemId.TradeRareMaterials1:
                    treaty.RareMaterials = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeRareMaterials2:
                    treaty.RareMaterials = (double) val;
                    break;

                case ScenarioEditorItemId.TradeOil1:
                    treaty.Oil = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeOil2:
                    treaty.Oil = (double) val;
                    break;

                case ScenarioEditorItemId.TradeSupplies1:
                    treaty.Supplies = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeSupplies2:
                    treaty.Supplies = (double) val;
                    break;

                case ScenarioEditorItemId.TradeMoney1:
                    treaty.Money = -(double) val;
                    break;

                case ScenarioEditorItemId.TradeMoney2:
                    treaty.Money = (double) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryFlagExt:
                    settings.FlagExt = (string) val;
                    break;

                case ScenarioEditorItemId.CountryRegularId:
                    settings.RegularId = (Country) val;
                    break;

                case ScenarioEditorItemId.CountryBelligerence:
                    settings.Belligerence = (int) val;
                    break;

                case ScenarioEditorItemId.CountryDissent:
                    settings.Dissent = (double) val;
                    break;

                case ScenarioEditorItemId.CountryExtraTc:
                    settings.ExtraTc = (double) val;
                    break;

                case ScenarioEditorItemId.CountryNuke:
                    settings.Nuke = (int) val;
                    break;

                case ScenarioEditorItemId.CountryNukeYear:
                    settings.NukeDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.CountryNukeMonth:
                    settings.NukeDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.CountryNukeDay:
                    settings.NukeDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.CountryGroundDefEff:
                    settings.GroundDefEff = (double) val;
                    break;

                case ScenarioEditorItemId.CountryPeacetimeIcModifier:
                    settings.PeacetimeIcModifier = (double) val;
                    break;

                case ScenarioEditorItemId.CountryWartimeIcModifier:
                    settings.WartimeIcModifier = (double) val;
                    break;

                case ScenarioEditorItemId.CountryIndustrialModifier:
                    settings.IndustrialModifier = (double) val;
                    break;

                case ScenarioEditorItemId.CountryRelativeManpower:
                    settings.RelativeManpower = (double) val;
                    break;

                case ScenarioEditorItemId.CountryEnergy:
                    settings.Energy = (double) val;
                    break;

                case ScenarioEditorItemId.CountryMetal:
                    settings.Metal = (double) val;
                    break;

                case ScenarioEditorItemId.CountryRareMaterials:
                    settings.RareMaterials = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOil:
                    settings.Oil = (double) val;
                    break;

                case ScenarioEditorItemId.CountrySupplies:
                    settings.Supplies = (double) val;
                    break;

                case ScenarioEditorItemId.CountryMoney:
                    settings.Money = (double) val;
                    break;

                case ScenarioEditorItemId.CountryTransports:
                    settings.Transports = (int) val;
                    break;

                case ScenarioEditorItemId.CountryEscorts:
                    settings.Escorts = (int) val;
                    break;

                case ScenarioEditorItemId.CountryManpower:
                    settings.Manpower = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapEnergy:
                    settings.Offmap.Energy = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapMetal:
                    settings.Offmap.Metal = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapRareMaterials:
                    settings.Offmap.RareMaterials = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapOil:
                    settings.Offmap.Oil = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapSupplies:
                    settings.Offmap.Supplies = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapMoney:
                    settings.Offmap.Money = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapTransports:
                    settings.Offmap.Transports = (int) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapEscorts:
                    settings.Offmap.Escorts = (int) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapManpower:
                    settings.Offmap.Manpower = (double) val;
                    break;

                case ScenarioEditorItemId.CountryOffmapIc:
                    settings.Offmap.Ic = (double) val;
                    break;

                case ScenarioEditorItemId.CountryAiFileName:
                    settings.AiFileName = (string) val;
                    break;

                case ScenarioEditorItemId.SliderYear:
                    settings.Policy.Date.Year = (int) val;
                    break;

                case ScenarioEditorItemId.SliderMonth:
                    settings.Policy.Date.Month = (int) val;
                    break;

                case ScenarioEditorItemId.SliderDay:
                    settings.Policy.Date.Day = (int) val;
                    break;

                case ScenarioEditorItemId.SliderDemocratic:
                    settings.Policy.Democratic = (int) val;
                    break;

                case ScenarioEditorItemId.SliderPoliticalLeft:
                    settings.Policy.PoliticalLeft = (int) val;
                    break;

                case ScenarioEditorItemId.SliderFreedom:
                    settings.Policy.Freedom = (int) val;
                    break;

                case ScenarioEditorItemId.SliderFreeMarket:
                    settings.Policy.FreeMarket = (int) val;
                    break;

                case ScenarioEditorItemId.SliderProfessionalArmy:
                    settings.Policy.ProfessionalArmy = (int) val;
                    break;

                case ScenarioEditorItemId.SliderDefenseLobby:
                    settings.Policy.DefenseLobby = (int) val;
                    break;

                case ScenarioEditorItemId.SliderInterventionism:
                    settings.Policy.Interventionism = (int) val;
                    break;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    Scenarios.SetId(settings.HeadOfState, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateType:
                    Scenarios.SetType(settings.HeadOfState, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    Scenarios.SetId(settings.HeadOfGovernment, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    Scenarios.SetType(settings.HeadOfGovernment, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    Scenarios.SetId(settings.ForeignMinister, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    Scenarios.SetType(settings.ForeignMinister, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    Scenarios.SetId(settings.ArmamentMinister, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    Scenarios.SetType(settings.ArmamentMinister, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    Scenarios.SetId(settings.MinisterOfSecurity, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    Scenarios.SetType(settings.MinisterOfSecurity, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    Scenarios.SetId(settings.MinisterOfIntelligence, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    Scenarios.SetType(settings.MinisterOfIntelligence, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    Scenarios.SetId(settings.ChiefOfStaff, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    Scenarios.SetType(settings.ChiefOfStaff, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    Scenarios.SetId(settings.ChiefOfArmy, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    Scenarios.SetType(settings.ChiefOfArmy, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    Scenarios.SetId(settings.ChiefOfNavy, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    Scenarios.SetType(settings.ChiefOfNavy, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    Scenarios.SetId(settings.ChiefOfAir, (int) val);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    Scenarios.SetType(settings.ChiefOfAir, (int) val);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Country country, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryName:
                    Config.SetText(((settings != null) && !string.IsNullOrEmpty(settings.Name))
                        ? settings.Name
                        : Countries.Strings[(int) country], (string) val, Game.WorldTextFileName);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Province province, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    settings.Capital = province.Id;
                    break;

                case ScenarioEditorItemId.CountryCoreProvinces:
                    if ((bool) val)
                    {
                        Scenarios.AddCoreProvince(province.Id, settings);
                    }
                    else
                    {
                        Scenarios.RemoveCoreProvince(province.Id, settings);
                    }
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    if ((bool) val)
                    {
                        Scenarios.AddOwnedProvince(province.Id, settings);
                    }
                    else
                    {
                        Scenarios.RemoveOwnedProvince(province.Id, settings);
                    }
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    if ((bool) val)
                    {
                        Scenarios.AddControlledProvince(province.Id, settings);
                    }
                    else
                    {
                        Scenarios.RemoveControlledProvince(province.Id, settings);
                    }
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    if ((bool) val)
                    {
                        Scenarios.AddClaimedProvince(province.Id, settings);
                    }
                    else
                    {
                        Scenarios.RemoveClaimedProvince(province.Id, settings);
                    }
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceVp:
                    settings.Vp = (int) val;
                    break;

                case ScenarioEditorItemId.ProvinceRevoltRisk:
                    settings.RevoltRisk = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                    settings.Manpower = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceManpowerMax:
                    settings.MaxManpower = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceEnergyPool:
                    settings.EnergyPool = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                    settings.Energy = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceEnergyMax:
                    settings.MaxEnergy = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceMetalPool:
                    settings.MetalPool = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceMetalCurrent:
                    settings.Metal = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceMetalMax:
                    settings.MaxMetal = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                    settings.RareMaterialsPool = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                    settings.RareMaterials = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                    settings.MaxRareMaterials = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceOilPool:
                    settings.OilPool = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceOilCurrent:
                    settings.Oil = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceOilMax:
                    settings.MaxOil = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSupplyPool:
                    settings.SupplyPool = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceIcCurrent:
                    settings.Ic.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceIcMax:
                    settings.Ic.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceIcRelative:
                    settings.Ic.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                    settings.Infrastructure.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                    settings.Infrastructure.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                    settings.Infrastructure.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                    settings.LandFort.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceLandFortMax:
                    settings.LandFort.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceLandFortRelative:
                    settings.LandFort.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                    settings.CoastalFort.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                    settings.CoastalFort.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                    settings.CoastalFort.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                    settings.AntiAir.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirMax:
                    settings.AntiAir.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                    settings.AntiAir.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                    settings.AirBase.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseMax:
                    settings.AirBase.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                    settings.AirBase.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                    settings.NavalBase.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                    settings.NavalBase.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                    settings.NavalBase.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                    settings.RadarStation.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationMax:
                    settings.RadarStation.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                    settings.RadarStation.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                    settings.NuclearReactor.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                    settings.NuclearReactor.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                    settings.NuclearReactor.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                    settings.RocketTest.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestMax:
                    settings.RocketTest.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                    settings.RocketTest.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                    settings.SyntheticOil.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                    settings.SyntheticOil.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                    settings.SyntheticOil.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                    settings.SyntheticRares.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                    settings.SyntheticRares.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                    settings.SyntheticRares.Size = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                    settings.NuclearPower.CurrentSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    settings.NuclearPower.MaxSize = (double) val;
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    settings.NuclearPower.Size = (double) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Province province, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceName:
                    Scenarios.SetProvinceName(province, settings, (string) val);
                    break;
            }
        }

        #endregion

        #region 編集項目 - 有効判定

        /// <summary>
        ///     編集項目の値が有効かどうかを判定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryBelligerence:
                case ScenarioEditorItemId.CountryNuke:
                case ScenarioEditorItemId.CountryTransports:
                case ScenarioEditorItemId.CountryEscorts:
                case ScenarioEditorItemId.ProvinceVp:
                    if ((int) val < 0)
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CountryDissent:
                case ScenarioEditorItemId.CountryPeacetimeIcModifier:
                case ScenarioEditorItemId.CountryWartimeIcModifier:
                case ScenarioEditorItemId.CountryIndustrialModifier:
                case ScenarioEditorItemId.CountryEnergy:
                case ScenarioEditorItemId.CountryMetal:
                case ScenarioEditorItemId.CountryRareMaterials:
                case ScenarioEditorItemId.CountryOil:
                case ScenarioEditorItemId.CountrySupplies:
                case ScenarioEditorItemId.CountryMoney:
                case ScenarioEditorItemId.CountryManpower:
                case ScenarioEditorItemId.ProvinceRevoltRisk:
                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                case ScenarioEditorItemId.ProvinceManpowerMax:
                case ScenarioEditorItemId.ProvinceEnergyPool:
                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                case ScenarioEditorItemId.ProvinceEnergyMax:
                case ScenarioEditorItemId.ProvinceMetalPool:
                case ScenarioEditorItemId.ProvinceMetalCurrent:
                case ScenarioEditorItemId.ProvinceMetalMax:
                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                case ScenarioEditorItemId.ProvinceOilPool:
                case ScenarioEditorItemId.ProvinceOilCurrent:
                case ScenarioEditorItemId.ProvinceOilMax:
                case ScenarioEditorItemId.ProvinceSupplyPool:
                case ScenarioEditorItemId.ProvinceIcCurrent:
                case ScenarioEditorItemId.ProvinceIcMax:
                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                    if (DoubleHelper.IsNegative((double) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CountryNukeYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CountryNukeMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CountryNukeDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                case ScenarioEditorItemId.ProvinceLandFortMax:
                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                case ScenarioEditorItemId.ProvinceAntiAirMax:
                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                case ScenarioEditorItemId.ProvinceAirBaseMax:
                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                case ScenarioEditorItemId.ProvinceRadarStationMax:
                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                case ScenarioEditorItemId.ProvinceRocketTestMax:
                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    if (DoubleHelper.IsNegative((double) val) || DoubleHelper.IsGreaterOrEqual((double) val, 10))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        ///     編集項目の値が有効かどうかを判定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeStartYear:
                case ScenarioEditorItemId.TradeEndYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.TradeStartMonth:
                case ScenarioEditorItemId.TradeEndMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.TradeStartDay:
                case ScenarioEditorItemId.TradeEndDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.TradeType:
                    if ((treaty.Id != null) && Scenarios.ExistsTypeId((int) val, treaty.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.TradeId:
                    if ((treaty.Id != null) && Scenarios.ExistsTypeId(treaty.Id.Type, (int) val))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        ///     編集項目の値が有効かどうかを判定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.SliderYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.SliderMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.SliderDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.SliderDemocratic:
                case ScenarioEditorItemId.SliderPoliticalLeft:
                case ScenarioEditorItemId.SliderFreedom:
                case ScenarioEditorItemId.SliderFreeMarket:
                case ScenarioEditorItemId.SliderProfessionalArmy:
                case ScenarioEditorItemId.SliderDefenseLobby:
                case ScenarioEditorItemId.SliderInterventionism:
                    if (((int) val < 1) || ((int) val > 10))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateType:
                    if ((settings != null) && (settings.HeadOfState != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.HeadOfState.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    if ((settings != null) && (settings.HeadOfState != null) &&
                        Scenarios.ExistsTypeId(settings.HeadOfState.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    if ((settings != null) && (settings.HeadOfGovernment != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.HeadOfGovernment.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    if ((settings != null) && (settings.HeadOfGovernment != null) &&
                        Scenarios.ExistsTypeId(settings.HeadOfGovernment.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    if ((settings != null) && (settings.ForeignMinister != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ForeignMinister.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    if ((settings != null) && (settings.ForeignMinister != null) &&
                        Scenarios.ExistsTypeId(settings.ForeignMinister.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    if ((settings != null) && (settings.ArmamentMinister != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ArmamentMinister.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    if ((settings != null) && (settings.ArmamentMinister != null) &&
                        Scenarios.ExistsTypeId(settings.ArmamentMinister.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    if ((settings != null) && (settings.MinisterOfSecurity != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.MinisterOfSecurity.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    if ((settings != null) && (settings.MinisterOfSecurity != null) &&
                        Scenarios.ExistsTypeId(settings.MinisterOfSecurity.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.MinisterOfIntelligence.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    if ((settings != null) && (settings.MinisterOfIntelligence != null) &&
                        Scenarios.ExistsTypeId(settings.MinisterOfIntelligence.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    if ((settings != null) && (settings.ChiefOfStaff != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ChiefOfStaff.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    if ((settings != null) && (settings.ChiefOfStaff != null) &&
                        Scenarios.ExistsTypeId(settings.ChiefOfStaff.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    if ((settings != null) && (settings.ChiefOfArmy != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ChiefOfArmy.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    if ((settings != null) && (settings.ChiefOfArmy != null) &&
                        Scenarios.ExistsTypeId(settings.ChiefOfArmy.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    if ((settings != null) && (settings.ChiefOfNavy != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ChiefOfNavy.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    if ((settings != null) && (settings.ChiefOfNavy != null) &&
                        Scenarios.ExistsTypeId(settings.ChiefOfNavy.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    if ((settings != null) && (settings.ChiefOfAir != null) &&
                        Scenarios.ExistsTypeId((int) val, settings.ChiefOfAir.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    if ((settings != null) && (settings.ChiefOfAir != null) &&
                        Scenarios.ExistsTypeId(settings.ChiefOfAir.Type, (int) val))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        #endregion

        #region 編集項目 - 編集済みフラグ取得

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="treaty">協定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Treaty treaty)
        {
            return treaty.IsDirty((Treaty.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, CountrySettings settings)
        {
            return (settings != null) && settings.IsDirty((CountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Country country, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryName:
                    return ((settings != null) && !String.IsNullOrEmpty(settings.Name)) &&
                           settings.IsDirty(CountrySettings.ItemId.Name);
            }

            return false;
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Province province, CountrySettings settings)
        {
            if (settings == null)
            {
                return false;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    return settings.IsDirty((CountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);

                case ScenarioEditorItemId.CountryCoreProvinces:
                    return settings.IsDirtyCoreProvinces(province.Id);

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    return settings.IsDirtyOwnedProvinces(province.Id);

                case ScenarioEditorItemId.CountryControlledProvinces:
                    return settings.IsDirtyControlledProvinces(province.Id);

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    return settings.IsDirtyClaimedProvinces(province.Id);
            }

            return false;
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Province province, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceName:
                    return ((settings != null) && !String.IsNullOrEmpty(settings.Name))
                        ? settings.IsDirty(ProvinceSettings.ItemId.Name)
                        : province.IsDirty(ProvinceItemId.Name);
            }

            return false;
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, ProvinceSettings settings)
        {
            return (settings != null) && settings.IsDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        #endregion

        #region 編集項目 - 編集済みフラグ設定

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="treaty">協定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Treaty treaty)
        {
            treaty.SetDirty((Treaty.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, CountrySettings settings)
        {
            settings.SetDirty((CountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Country country, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryName:
                    if ((settings != null) && !string.IsNullOrEmpty(settings.Name))
                    {
                        settings.SetDirty((CountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
                    }
                    break;
            }
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Province province, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    settings.SetDirty((CountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.CountryCoreProvinces:
                    settings.SetDirtyCoreProvinces(province.Id);
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    settings.SetDirtyOwnedProvinces(province.Id);
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    settings.SetDirtyControlledProvinces(province.Id);
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    settings.SetDirtyClaimedProvinces(province.Id);
                    Scenarios.SetDirty();
                    break;
            }
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceVp:
                    settings.SetDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
                    if (Scenarios.Data.IsVpProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyVpInc();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.ProvinceRevoltRisk:
                case ScenarioEditorItemId.ProvinceIcRelative:
                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    settings.SetDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
                    if (Scenarios.Data.IsBaseDodProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyBasesDodInc();
                    }
                    else if (Scenarios.Data.IsBaseProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyBasesInc();
                    }
                    else if (Scenarios.Data.IsCountryProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyCountryInc();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                case ScenarioEditorItemId.ProvinceManpowerMax:
                case ScenarioEditorItemId.ProvinceEnergyPool:
                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                case ScenarioEditorItemId.ProvinceEnergyMax:
                case ScenarioEditorItemId.ProvinceMetalPool:
                case ScenarioEditorItemId.ProvinceMetalCurrent:
                case ScenarioEditorItemId.ProvinceMetalMax:
                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                case ScenarioEditorItemId.ProvinceOilPool:
                case ScenarioEditorItemId.ProvinceOilCurrent:
                case ScenarioEditorItemId.ProvinceOilMax:
                case ScenarioEditorItemId.ProvinceSupplyPool:
                case ScenarioEditorItemId.ProvinceIcCurrent:
                case ScenarioEditorItemId.ProvinceIcMax:
                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                case ScenarioEditorItemId.ProvinceLandFortMax:
                case ScenarioEditorItemId.ProvinceLandFortRelative:
                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                case ScenarioEditorItemId.ProvinceAntiAirMax:
                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                case ScenarioEditorItemId.ProvinceAirBaseMax:
                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                case ScenarioEditorItemId.ProvinceRadarStationMax:
                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                case ScenarioEditorItemId.ProvinceRocketTestMax:
                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    settings.SetDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
                    if (Scenarios.Data.IsBaseProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyBasesInc();
                    }
                    else if (Scenarios.Data.IsCountryProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyCountryInc();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;
            }
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Province province, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceName:
                    if ((settings != null) && !String.IsNullOrEmpty(settings.Name))
                    {
                        settings.SetDirty(ProvinceSettings.ItemId.Name);
                        if (Scenarios.Data.IsBaseProvinceSettings)
                        {
                            Scenarios.Data.SetDirtyBasesInc();
                        }
                        else if (Scenarios.Data.IsCountryProvinceSettings)
                        {
                            Scenarios.Data.SetDirtyCountryInc();
                        }
                        else
                        {
                            Scenarios.Data.SetDirty();
                        }
                    }
                    else
                    {
                        province.SetDirty(ProvinceItemId.Name);
                    }
                    break;
            }
        }

        #endregion

        #region 編集項目 - 変更前処理

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeStartYear:
                    PreItemChangedTreatyStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartDay), treaty);
                    break;

                case ScenarioEditorItemId.TradeStartMonth:
                    PreItemChangedTreatyStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartDay), treaty);
                    break;

                case ScenarioEditorItemId.TradeStartDay:
                    PreItemChangedTreatyStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeStartMonth), treaty);
                    break;

                case ScenarioEditorItemId.TradeEndYear:
                    PreItemChangedTreatyEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndDay), treaty);
                    break;

                case ScenarioEditorItemId.TradeEndMonth:
                    PreItemChangedTreatyEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndDay), treaty);
                    break;

                case ScenarioEditorItemId.TradeEndDay:
                    PreItemChangedTreatyEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEndMonth), treaty);
                    break;

                case ScenarioEditorItemId.TradeType:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyType((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeId), val,
                            treaty);
                    }
                    break;

                case ScenarioEditorItemId.TradeId:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyId((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeType), val,
                            treaty);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryNukeYear:
                    PreItemChangedNukeDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeDay), settings);
                    break;

                case ScenarioEditorItemId.CountryNukeMonth:
                    PreItemChangedNukeDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeDay), settings);
                    break;

                case ScenarioEditorItemId.CountryNukeDay:
                    PreItemChangedNukeDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNukeMonth), settings);
                    break;

                case ScenarioEditorItemId.CountryOffmapEnergy:
                case ScenarioEditorItemId.CountryOffmapMetal:
                case ScenarioEditorItemId.CountryOffmapRareMaterials:
                case ScenarioEditorItemId.CountryOffmapOil:
                case ScenarioEditorItemId.CountryOffmapSupplies:
                case ScenarioEditorItemId.CountryOffmapMoney:
                case ScenarioEditorItemId.CountryOffmapTransports:
                case ScenarioEditorItemId.CountryOffmapEscorts:
                case ScenarioEditorItemId.CountryOffmapManpower:
                case ScenarioEditorItemId.CountryOffmapIc:
                    if (settings.Offmap == null)
                    {
                        settings.Offmap = new ResourceSettings();
                    }
                    break;

                case ScenarioEditorItemId.SliderYear:
                    PreItemChangedSliderDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderDay), settings);
                    break;

                case ScenarioEditorItemId.SliderMonth:
                    PreItemChangedSliderDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderDay), settings);
                    break;

                case ScenarioEditorItemId.SliderDay:
                    PreItemChangedSliderDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.SliderMonth), settings);
                    break;

                case ScenarioEditorItemId.SliderDemocratic:
                case ScenarioEditorItemId.SliderPoliticalLeft:
                case ScenarioEditorItemId.SliderFreedom:
                case ScenarioEditorItemId.SliderFreeMarket:
                case ScenarioEditorItemId.SliderProfessionalArmy:
                case ScenarioEditorItemId.SliderDefenseLobby:
                case ScenarioEditorItemId.SliderInterventionism:
                    if (settings.Policy == null)
                    {
                        settings.Policy = new CountryPolicy();
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    if (settings.HeadOfState == null)
                    {
                        settings.HeadOfState = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfStateType), settings,
                            settings.HeadOfState);
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateType:
                    if (settings.HeadOfState == null)
                    {
                        settings.HeadOfState = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfStateId), val, settings,
                            settings.HeadOfState);
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    if (settings.HeadOfGovernment == null)
                    {
                        settings.HeadOfGovernment = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfGovernmentType), settings,
                            settings.HeadOfGovernment);
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    if (settings.HeadOfGovernment == null)
                    {
                        settings.HeadOfGovernment = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfGovernmentId), val,
                            settings, settings.HeadOfGovernment);
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    if (settings.ForeignMinister == null)
                    {
                        settings.ForeignMinister = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetForeignMinisterType), settings,
                            settings.ForeignMinister);
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    if (settings.ForeignMinister == null)
                    {
                        settings.ForeignMinister = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetForeignMinisterId), val, settings,
                            settings.ForeignMinister);
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    if (settings.ArmamentMinister == null)
                    {
                        settings.ArmamentMinister = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetArmamentMinisterType), settings,
                            settings.ArmamentMinister);
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    if (settings.ArmamentMinister == null)
                    {
                        settings.ArmamentMinister = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetArmamentMinisterId), val,
                            settings, settings.ArmamentMinister);
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    if (settings.MinisterOfSecurity == null)
                    {
                        settings.MinisterOfSecurity = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfSecurityType), settings,
                            settings.MinisterOfSecurity);
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    if (settings.MinisterOfSecurity == null)
                    {
                        settings.MinisterOfSecurity = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfSecurityId), val,
                            settings, settings.MinisterOfSecurity);
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    if (settings.MinisterOfIntelligence == null)
                    {
                        settings.MinisterOfIntelligence = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfIntelligenceType),
                            settings, settings.MinisterOfIntelligence);
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    if (settings.MinisterOfIntelligence == null)
                    {
                        settings.MinisterOfIntelligence = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfIntelligenceId), val,
                            settings, settings.MinisterOfIntelligence);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    if (settings.ChiefOfStaff == null)
                    {
                        settings.ChiefOfStaff = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfStaffType), settings,
                            settings.ChiefOfStaff);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    if (settings.ChiefOfStaff == null)
                    {
                        settings.ChiefOfStaff = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfStaffId), val, settings,
                            settings.ChiefOfStaff);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    if (settings.ChiefOfArmy == null)
                    {
                        settings.ChiefOfArmy = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfArmyType), settings,
                            settings.ChiefOfArmy);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    if (settings.ChiefOfArmy == null)
                    {
                        settings.ChiefOfArmy = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfArmyId), val, settings,
                            settings.ChiefOfArmy);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    if (settings.ChiefOfNavy == null)
                    {
                        settings.ChiefOfNavy = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfNavyType), settings,
                            settings.ChiefOfNavy);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    if (settings.ChiefOfNavy == null)
                    {
                        settings.ChiefOfNavy = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfNavyId), val, settings,
                            settings.ChiefOfNavy);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    if (settings.ChiefOfAir == null)
                    {
                        settings.ChiefOfAir = new TypeId();
                        PreItemChangedCabinetId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfAirType), settings,
                            settings.ChiefOfAir);
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    if (settings.ChiefOfAir == null)
                    {
                        settings.ChiefOfAir = new TypeId();
                        PreItemChangedCabinetType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfAirId), val, settings,
                            settings.ChiefOfAir);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Province province, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    // チェックを入れた後はチェックボックスを無効化する
                    if ((bool) val)
                    {
                        _form.GetItemControl(itemId).Enabled = false;
                    }
                    // プロヴィンスリストビューの表示を更新する
                    int index = GetLandProvinceIndex(settings.Capital);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 2, "");
                    }
                    index = _landProvinces.IndexOf(province);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 2, Resources.Yes);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceIcCurrent:
                case ScenarioEditorItemId.ProvinceIcMax:
                case ScenarioEditorItemId.ProvinceIcRelative:
                    if (settings.Ic == null)
                    {
                        settings.Ic = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                    if (settings.Infrastructure == null)
                    {
                        settings.Infrastructure = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                case ScenarioEditorItemId.ProvinceLandFortMax:
                case ScenarioEditorItemId.ProvinceLandFortRelative:
                    if (settings.LandFort == null)
                    {
                        settings.LandFort = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                    if (settings.CoastalFort == null)
                    {
                        settings.CoastalFort = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                case ScenarioEditorItemId.ProvinceAntiAirMax:
                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                    if (settings.AntiAir == null)
                    {
                        settings.AntiAir = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                case ScenarioEditorItemId.ProvinceAirBaseMax:
                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                    if (settings.AirBase == null)
                    {
                        settings.AirBase = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                    if (settings.NavalBase == null)
                    {
                        settings.NavalBase = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                case ScenarioEditorItemId.ProvinceRadarStationMax:
                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                    if (settings.RadarStation == null)
                    {
                        settings.RadarStation = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                    if (settings.NuclearReactor == null)
                    {
                        settings.NuclearReactor = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                case ScenarioEditorItemId.ProvinceRocketTestMax:
                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                    if (settings.RocketTest == null)
                    {
                        settings.RocketTest = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                    if (settings.SyntheticOil == null)
                    {
                        settings.SyntheticOil = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                    if (settings.SyntheticRares == null)
                    {
                        settings.SyntheticRares = new BuildingSize();
                    }
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    if (settings.NuclearPower == null)
                    {
                        settings.NuclearPower = new BuildingSize();
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 協定開始日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="treaty">協定</param>
        private void PreItemChangedTreatyStartDate(TextBox control1, TextBox control2, Treaty treaty)
        {
            if (treaty.StartDate == null)
            {
                treaty.StartDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, treaty);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, treaty);

                // 編集項目の値を更新する
                UpdateItemValue(control1, treaty);
                UpdateItemValue(control2, treaty);

                // 編集項目の色を更新する
                UpdateItemColor(control1, treaty);
                UpdateItemColor(control2, treaty);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 協定終了日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="treaty">協定</param>
        private void PreItemChangedTreatyEndDate(TextBox control1, TextBox control2, Treaty treaty)
        {
            if (treaty.EndDate == null)
            {
                treaty.EndDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, treaty);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, treaty);

                // 編集項目の値を更新する
                UpdateItemValue(control1, treaty);
                UpdateItemValue(control2, treaty);

                // 編集項目の色を更新する
                UpdateItemColor(control1, treaty);
                UpdateItemColor(control2, treaty);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 協定type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        private void PreItemChangedTreatyType(TextBox control, object val, Treaty treaty)
        {
            // 新規idを設定する
            treaty.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, treaty);

            // 編集項目の値を更新する
            UpdateItemValue(control, treaty);

            // 編集項目の色を更新する
            UpdateItemColor(control, treaty);
        }

        /// <summary>
        ///     項目変更前の処理 - 協定id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        private void PreItemChangedTreatyId(TextBox control, object val, Treaty treaty)
        {
            treaty.Id.Type = Scenarios.GetNewType(Scenarios.DefaultTreatyType, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, treaty);

            // 編集項目の値を更新する
            UpdateItemValue(control, treaty);

            // 編集項目の色を更新する
            UpdateItemColor(control, treaty);
        }

        /// <summary>
        ///     項目変更前の処理 - 核兵器生産日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedNukeDate(TextBox control1, TextBox control2, CountrySettings settings)
        {
            if (settings.NukeDate == null)
            {
                settings.NukeDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, settings);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, settings);

                // 編集項目の値を更新する
                UpdateItemValue(control1, settings);
                UpdateItemValue(control2, settings);

                // 編集項目の色を更新する
                UpdateItemColor(control1, settings);
                UpdateItemColor(control2, settings);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - スライダー移動日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedSliderDate(TextBox control1, TextBox control2, CountrySettings settings)
        {
            if (settings.Policy == null)
            {
                settings.Policy = new CountryPolicy();
            }
            if (settings.Policy.Date == null)
            {
                settings.Policy.Date = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, settings);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, settings);

                // 編集項目の値を更新する
                UpdateItemValue(control1, settings);
                UpdateItemValue(control2, settings);

                // 編集項目の色を更新する
                UpdateItemColor(control1, settings);
                UpdateItemColor(control2, settings);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 閣僚type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        /// <param name="typeId">typeとidの組</param>
        private void PreItemChangedCabinetType(TextBox control, object val, CountrySettings settings, TypeId typeId)
        {
            // 新規idを設定する
            typeId.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, settings);

            // 編集項目の色を更新する
            UpdateItemColor(control, settings);
        }

        /// <summary>
        ///     項目変更前の処理 - 閣僚id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="settings">国家設定</param>
        /// <param name="typeId">typeとidの組</param>
        private void PreItemChangedCabinetId(TextBox control, CountrySettings settings, TypeId typeId)
        {
            typeId.Type = Scenarios.GetNewType((settings.HeadOfState != null) ? settings.HeadOfState.Type : 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, settings);

            // 編集項目の色を更新する
            UpdateItemColor(control, settings);
        }

        #endregion

        #region 編集項目 - 変更後処理

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeCountry1:
                    _form.SetTradeListItemText(0, Countries.GetName((Country) val));
                    break;

                case ScenarioEditorItemId.TradeCountry2:
                    _form.SetTradeListItemText(1, Countries.GetName((Country) val));
                    break;

                case ScenarioEditorItemId.TradeEnergy1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEnergy1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEnergy2), treaty);
                    break;

                case ScenarioEditorItemId.TradeEnergy2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEnergy2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeEnergy1), treaty);
                    break;

                case ScenarioEditorItemId.TradeMetal1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMetal1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMetal2), treaty);
                    break;

                case ScenarioEditorItemId.TradeMetal2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMetal2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMetal1), treaty);
                    break;

                case ScenarioEditorItemId.TradeRareMaterials1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeRareMaterials1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeRareMaterials2), treaty);
                    break;

                case ScenarioEditorItemId.TradeRareMaterials2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeRareMaterials2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeRareMaterials1), treaty);
                    break;

                case ScenarioEditorItemId.TradeOil1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeOil1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeOil2), treaty);
                    break;

                case ScenarioEditorItemId.TradeOil2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeOil2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeOil1), treaty);
                    break;

                case ScenarioEditorItemId.TradeSupplies1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeSupplies1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeSupplies2), treaty);
                    break;

                case ScenarioEditorItemId.TradeSupplies2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeSupplies2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeSupplies1), treaty);
                    break;

                case ScenarioEditorItemId.TradeMoney1:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMoney1),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMoney2), treaty);
                    break;

                case ScenarioEditorItemId.TradeMoney2:
                    PostItemChangedTradeDeals((TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMoney2),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.TradeMoney1), treaty);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CabinetHeadOfState:
                    PostItemChangedCabinet((TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfStateId),
                        settings);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    PostItemChangedCabinetId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfState),
                        val, _headOfStateList);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                    PostItemChangedCabinet(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfGovernmentId), settings);
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    PostItemChangedCabinetId(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetHeadOfGovernment), val,
                        _headOfGovernmentList);
                    break;

                case ScenarioEditorItemId.CabinetForeignMinister:
                    PostItemChangedCabinet(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetForeignMinisterId), settings);
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    PostItemChangedCabinetId(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetForeignMinister), val,
                        _foreignMinisterList);
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                    PostItemChangedCabinet(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetArmamentMinisterId), settings);
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    PostItemChangedCabinetId(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetArmamentMinister), val,
                        _armamentMinisterList);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                    PostItemChangedCabinet(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfSecurityId), settings);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    PostItemChangedCabinetId(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfSecurity), val,
                        _ministerOfSecurityList);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                    PostItemChangedCabinet(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfIntelligenceId), settings);
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    PostItemChangedCabinetId(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetMinisterOfIntelligence), val,
                        _ministerOfIntelligenceList);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                    PostItemChangedCabinet((TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfStaffId),
                        settings);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    PostItemChangedCabinetId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfStaff),
                        val, _chiefOfStaffList);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                    PostItemChangedCabinet((TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfArmyId),
                        settings);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    PostItemChangedCabinetId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfArmy),
                        val, _chiefOfArmyList);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                    PostItemChangedCabinet((TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfNavyId),
                        settings);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    PostItemChangedCabinetId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfNavy),
                        val, _chiefOfNavyList);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                    PostItemChangedCabinet((TextBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfAirId),
                        settings);
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    PostItemChangedCabinetId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.CabinetChiefOfAir),
                        val, _chiefOfAirList);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Province province)
        {
            int index;
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCoreProvinces:
                    // プロヴィンスリストビューの表示を更新する
                    index = GetLandProvinceIndex(province.Id);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 3, (bool) val ? Resources.Yes : "");
                    }

                    // プロヴィンスの強調表示を更新する
                    if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Core)
                    {
                        _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
                    }
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    // プロヴィンスリストビューの表示を更新する
                    index = GetLandProvinceIndex(province.Id);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 4, (bool) val ? Resources.Yes : "");
                    }

                    // プロヴィンスの強調表示を更新する
                    if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Owned)
                    {
                        _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
                    }
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    // プロヴィンスリストビューの表示を更新する
                    index = GetLandProvinceIndex(province.Id);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 5, (bool) val ? Resources.Yes : "");
                    }

                    // プロヴィンスの強調表示を更新する
                    if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Controlled)
                    {
                        _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
                    }
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    // プロヴィンスリストビューの表示を更新する
                    index = GetLandProvinceIndex(province.Id);
                    if (index >= 0)
                    {
                        _form.SetProvinceListItemText(index, 6, (bool) val ? Resources.Yes : "");
                    }

                    // プロヴィンスの強調表示を更新する
                    if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Claimed)
                    {
                        _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                    PostItemChangedResourceCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceManpowerMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceManpowerMax:
                    PostItemChangedResourceMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceManpowerCurrent), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                    PostItemChangedResourceCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceEnergyMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceEnergyMax:
                    PostItemChangedResourceMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceEnergyCurrent), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceMetalCurrent:
                    PostItemChangedResourceCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceMetalMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceMetalMax:
                    PostItemChangedResourceMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceMetalCurrent), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                    PostItemChangedResourceCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRareMaterialsMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                    PostItemChangedResourceMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRareMaterialsCurrent), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceOilCurrent:
                    PostItemChangedResourceCurrent((TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceOilMax),
                        val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceOilMax:
                    PostItemChangedResourceMax((TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceOilCurrent),
                        val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceIcCurrent:
                    PostItemChangedBuildingCurrent((TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceIcMax:
                    PostItemChangedBuildingMax((TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceIcRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceIcMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceInfrastructureMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceLandFortMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceLandFortRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceLandFortMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceCoastalFortMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAntiAirMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceAirBaseMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNavalBaseMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRadarStationMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearReactorMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceRocketTestMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticOilMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresRelative), val,
                        settings);
                    break;

                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceSyntheticRaresMax), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                    PostItemChangedBuildingCurrent(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerMax),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    PostItemChangedBuildingMax(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerRelative), val, settings);
                    break;

                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    PostItemChangedBuildingRelative(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerCurrent),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNuclearPowerMax), val, settings);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 貿易量
        /// </summary>
        /// <param name="control1">変更対象のコントロール</param>
        /// <param name="control2">連動するコントロール</param>
        /// <param name="treaty">協定</param>
        private void PostItemChangedTradeDeals(TextBox control1, TextBox control2, Treaty treaty)
        {
            // 項目の値を更新する
            UpdateItemValue(control1, treaty);
            UpdateItemValue(control2, treaty);

            // 項目の色を更新する
            UpdateItemColor(control2, treaty);

            // 貿易リストビューの項目を更新する
            _form.SetTradeListItemText(2, treaty.GetTradeString());
        }

        /// <summary>
        ///     項目値変更後の処理 - 閣僚
        /// </summary>
        /// <param name="control">閣僚idのコントロール</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedCabinet(TextBox control, CountrySettings settings)
        {
            // 項目の値を更新する
            UpdateItemValue(control, settings);

            // 項目の色を更新する
            UpdateItemColor(control, settings);
        }

        /// <summary>
        ///     項目変更後の処理 - 閣僚id
        /// </summary>
        /// <param name="control">閣僚コンボボックス</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="ministers">閣僚候補リスト</param>
        private static void PostItemChangedCabinetId(ComboBox control, object val, List<Minister> ministers)
        {
            // コンボボックスの選択項目を変更する
            control.SelectedIndex = ministers.FindIndex(minister => minister.Id == (int) val);
        }

        /// <summary>
        ///     項目値変更後の処理 - 資源現在値
        /// </summary>
        /// <param name="control">最大値のコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void PostItemChangedResourceCurrent(Control control, object val, ProvinceSettings settings)
        {
            // 現在値が最大値以下ならば何もしない
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            double max = (double) GetItemValue(itemId, settings);
            if (DoubleHelper.IsLessOrEqual((double) val, max))
            {
                return;
            }

            OutputItemValueChangedLog(itemId, val, settings);

            // 最大値を現在値で更新する
            SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            SetItemDirty(itemId, settings);

            // 編集項目の値を更新する
            control.Text = DoubleHelper.ToString((double) val);

            // 編集項目の色を更新する
            control.ForeColor = Color.Red;
        }

        /// <summary>
        ///     項目値変更後の処理 - 資源最大値
        /// </summary>
        /// <param name="control">現在値のコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void PostItemChangedResourceMax(Control control, object val, ProvinceSettings settings)
        {
            // 最大値が現在値以上ならば何もしない
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            double current = (double) GetItemValue(itemId, settings);
            if (DoubleHelper.IsGreaterOrEqual((double) val, current))
            {
                return;
            }

            OutputItemValueChangedLog(itemId, val, settings);

            // 現在値を最大値で更新する
            SetItemValue(itemId, val, settings);

            // 編集済みフラグを設定する
            SetItemDirty(itemId, settings);

            // 編集項目の値を更新する
            control.Text = DoubleHelper.ToString((double) val);

            // 編集項目の色を更新する
            control.ForeColor = Color.Red;
        }

        /// <summary>
        ///     項目値変更後の処理 - 建物現在値
        /// </summary>
        /// <param name="control1">最大値のコントロール1</param>
        /// <param name="control2">相対値のコントロール2</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void PostItemChangedBuildingCurrent(Control control1, Control control2, object val,
            ProvinceSettings settings)
        {
            // 相対値が0でなければクリアする
            ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
            double relative = (double) GetItemValue(itemId2, settings);
            if (!DoubleHelper.IsZero(relative))
            {
                OutputItemValueChangedLog(itemId2, val, settings);

                // 相対値を0に設定する
                SetItemValue(itemId2, (double) 0, settings);

                // 編集済みフラグを設定する
                SetItemDirty(itemId2, settings);

                // 編集項目の値をクリアする
                control2.Text = "";
            }

            // 現在値が最大値以下ならば何もしない
            ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
            double max = (double) GetItemValue(itemId1, settings);
            if (DoubleHelper.IsLessOrEqual((double) val, max))
            {
                return;
            }

            OutputItemValueChangedLog(itemId1, val, settings);

            // 最大値を現在値で更新する
            SetItemValue(itemId1, val, settings);

            // 編集済みフラグを設定する
            SetItemDirty(itemId1, settings);

            // 編集項目の値を更新する
            control1.Text = DoubleHelper.ToString((double) val);

            // 編集項目の色を更新する
            control1.ForeColor = Color.Red;
        }

        /// <summary>
        ///     項目値変更後の処理 - 建物最大値
        /// </summary>
        /// <param name="control1">現在値のコントロール1</param>
        /// <param name="control2">相対値のコントロール2</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void PostItemChangedBuildingMax(Control control1, Control control2, object val,
            ProvinceSettings settings)
        {
            // 相対値が0でなければクリアする
            ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
            double relative = (double) GetItemValue(itemId2, settings);
            if (!DoubleHelper.IsZero(relative))
            {
                OutputItemValueChangedLog(itemId2, val, settings);

                // 相対値を0に設定する
                SetItemValue(itemId2, (double) 0, settings);

                // 編集済みフラグを設定する
                SetItemDirty(itemId2, settings);

                // 編集項目の値をクリアする
                control2.Text = "";
            }

            // 最大値が現在値以上ならば何もしない
            ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
            double current = (double) GetItemValue(itemId1, settings);
            if (DoubleHelper.IsGreaterOrEqual((double) val, current))
            {
                return;
            }

            OutputItemValueChangedLog(itemId1, val, settings);

            // 現在値を最大値で更新する
            SetItemValue(itemId1, val, settings);

            // 編集済みフラグを設定する
            SetItemDirty(itemId1, settings);

            // 編集項目の値を更新する
            control1.Text = DoubleHelper.ToString((double) val);

            // 編集項目の色を更新する
            control1.ForeColor = Color.Red;
        }

        /// <summary>
        ///     項目値変更後の処理 - 建物相対値
        /// </summary>
        /// <param name="control1">現在値のコントロール1</param>
        /// <param name="control2">最大値のコントロール2</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        private void PostItemChangedBuildingRelative(Control control1, Control control2, object val,
            ProvinceSettings settings)
        {
            // 現在値が0でなければクリアする
            ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
            double current = (double) GetItemValue(itemId1, settings);
            if (!DoubleHelper.IsZero(current))
            {
                OutputItemValueChangedLog(itemId1, val, settings);

                // 現在値を0に設定する
                SetItemValue(itemId1, (double) 0, settings);

                // 編集済みフラグを設定する
                SetItemDirty(itemId1, settings);

                // 編集項目の値をクリアする
                control1.Text = "";
            }

            // 最大値が0でなければクリアする
            ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
            double max = (double) GetItemValue(itemId2, settings);
            if (!DoubleHelper.IsZero(max))
            {
                OutputItemValueChangedLog(itemId2, val, settings);

                // 最大値を0に設定する
                SetItemValue(itemId2, (double) 0, settings);

                // 編集済みフラグを設定する
                SetItemDirty(itemId2, settings);

                // 編集項目の値をクリアする
                control2.Text = "";
            }
        }

        #endregion

        #region 編集項目 - ログ出力

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeCountry1:
                case ScenarioEditorItemId.TradeCountry2:
                    Log.Info("[Scenario] {0}: {1} -> {2}", ItemStrings[(int) itemId],
                        Countries.Strings[(int) GetItemValue(itemId, treaty)], Countries.Strings[(int) val]);
                    break;

                case ScenarioEditorItemId.TradeCancel:
                    Log.Info("[Scenario] {0}: {1} -> {2}", ItemStrings[(int) itemId], BoolHelper.ToYesNo(treaty.Cancel),
                        BoolHelper.ToYesNo((bool) val));
                    break;

                default:
                    Log.Info("[Scenario] {0}: {1} -> {2}", ItemStrings[(int) itemId],
                        ObjectHelper.ToString(GetItemValue(itemId, treaty)), ObjectHelper.ToString(val));
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryRegularId:
                    Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                        Countries.Strings[(int) GetItemValue(itemId, settings)], Countries.Strings[(int) val],
                        Countries.Strings[(int) settings.Country]);
                    break;

                default:
                    Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                        ObjectHelper.ToString(GetItemValue(itemId, settings)), ObjectHelper.ToString(val),
                        Countries.Strings[(int) settings.Country]);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Country country,
            CountrySettings settings)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, country, settings)), ObjectHelper.ToString(val),
                Countries.Strings[(int) settings.Country]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Province province,
            CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId], settings.Capital,
                        province.Id, Countries.Strings[(int) settings.Country]);
                    break;

                case ScenarioEditorItemId.CountryCoreProvinces:
                case ScenarioEditorItemId.CountryOwnedProvinces:
                case ScenarioEditorItemId.CountryControlledProvinces:
                case ScenarioEditorItemId.CountryClaimedProvinces:
                    Log.Info("[Scenario] {0}: {1}{2} ({3})", ItemStrings[(int) itemId], (bool) val ? '+' : '-',
                        ObjectHelper.ToString(province.Id), Countries.Strings[(int) settings.Country]);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Province province,
            ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceName:
                    Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                        Scenarios.GetProvinceName(province, settings), val, province.Id);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, ProvinceSettings settings)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, settings)), ObjectHelper.ToString(val), settings.Id);
        }

        #endregion

        #endregion
    }

    /// <summary>
    ///     シナリオエディタの項目ID
    /// </summary>
    public enum ScenarioEditorItemId
    {
        TradeStartYear,
        TradeStartMonth,
        TradeStartDay,
        TradeEndYear,
        TradeEndMonth,
        TradeEndDay,
        TradeType,
        TradeId,
        TradeCancel,
        TradeCountry1,
        TradeCountry2,
        TradeEnergy1,
        TradeEnergy2,
        TradeMetal1,
        TradeMetal2,
        TradeRareMaterials1,
        TradeRareMaterials2,
        TradeOil1,
        TradeOil2,
        TradeSupplies1,
        TradeSupplies2,
        TradeMoney1,
        TradeMoney2,
        CountryName,
        CountryFlagExt,
        CountryRegularId,
        CountryBelligerence,
        CountryDissent,
        CountryExtraTc,
        CountryNuke,
        CountryNukeYear,
        CountryNukeMonth,
        CountryNukeDay,
        CountryGroundDefEff,
        CountryPeacetimeIcModifier,
        CountryWartimeIcModifier,
        CountryIndustrialModifier,
        CountryRelativeManpower,
        CountryEnergy,
        CountryMetal,
        CountryRareMaterials,
        CountryOil,
        CountrySupplies,
        CountryMoney,
        CountryTransports,
        CountryEscorts,
        CountryManpower,
        CountryOffmapEnergy,
        CountryOffmapMetal,
        CountryOffmapRareMaterials,
        CountryOffmapOil,
        CountryOffmapSupplies,
        CountryOffmapMoney,
        CountryOffmapTransports,
        CountryOffmapEscorts,
        CountryOffmapManpower,
        CountryOffmapIc,
        CountryAiFileName,
        SliderYear,
        SliderMonth,
        SliderDay,
        SliderDemocratic,
        SliderPoliticalLeft,
        SliderFreedom,
        SliderFreeMarket,
        SliderProfessionalArmy,
        SliderDefenseLobby,
        SliderInterventionism,
        CabinetHeadOfState,
        CabinetHeadOfStateType,
        CabinetHeadOfStateId,
        CabinetHeadOfGovernment,
        CabinetHeadOfGovernmentType,
        CabinetHeadOfGovernmentId,
        CabinetForeignMinister,
        CabinetForeignMinisterType,
        CabinetForeignMinisterId,
        CabinetArmamentMinister,
        CabinetArmamentMinisterType,
        CabinetArmamentMinisterId,
        CabinetMinisterOfSecurity,
        CabinetMinisterOfSecurityType,
        CabinetMinisterOfSecurityId,
        CabinetMinisterOfIntelligence,
        CabinetMinisterOfIntelligenceType,
        CabinetMinisterOfIntelligenceId,
        CabinetChiefOfStaff,
        CabinetChiefOfStaffType,
        CabinetChiefOfStaffId,
        CabinetChiefOfArmy,
        CabinetChiefOfArmyType,
        CabinetChiefOfArmyId,
        CabinetChiefOfNavy,
        CabinetChiefOfNavyType,
        CabinetChiefOfNavyId,
        CabinetChiefOfAir,
        CabinetChiefOfAirType,
        CabinetChiefOfAirId,
        CountryCapital,
        CountryCoreProvinces,
        CountryOwnedProvinces,
        CountryControlledProvinces,
        CountryClaimedProvinces,
        ProvinceId,
        ProvinceName,
        ProvinceVp,
        ProvinceRevoltRisk,
        ProvinceManpowerCurrent,
        ProvinceManpowerMax,
        ProvinceEnergyPool,
        ProvinceEnergyCurrent,
        ProvinceEnergyMax,
        ProvinceMetalPool,
        ProvinceMetalCurrent,
        ProvinceMetalMax,
        ProvinceRareMaterialsPool,
        ProvinceRareMaterialsCurrent,
        ProvinceRareMaterialsMax,
        ProvinceOilPool,
        ProvinceOilCurrent,
        ProvinceOilMax,
        ProvinceSupplyPool,
        ProvinceIcCurrent,
        ProvinceIcMax,
        ProvinceIcRelative,
        ProvinceInfrastructureCurrent,
        ProvinceInfrastructureMax,
        ProvinceInfrastructureRelative,
        ProvinceLandFortCurrent,
        ProvinceLandFortMax,
        ProvinceLandFortRelative,
        ProvinceCoastalFortCurrent,
        ProvinceCoastalFortMax,
        ProvinceCoastalFortRelative,
        ProvinceAntiAirCurrent,
        ProvinceAntiAirMax,
        ProvinceAntiAirRelative,
        ProvinceAirBaseCurrent,
        ProvinceAirBaseMax,
        ProvinceAirBaseRelative,
        ProvinceNavalBaseCurrent,
        ProvinceNavalBaseMax,
        ProvinceNavalBaseRelative,
        ProvinceRadarStationCurrent,
        ProvinceRadarStationMax,
        ProvinceRadarStationRelative,
        ProvinceNuclearReactorCurrent,
        ProvinceNuclearReactorMax,
        ProvinceNuclearReactorRelative,
        ProvinceRocketTestCurrent,
        ProvinceRocketTestMax,
        ProvinceRocketTestRelative,
        ProvinceSyntheticOilCurrent,
        ProvinceSyntheticOilMax,
        ProvinceSyntheticOilRelative,
        ProvinceSyntheticRaresCurrent,
        ProvinceSyntheticRaresMax,
        ProvinceSyntheticRaresRelative,
        ProvinceNuclearPowerCurrent,
        ProvinceNuclearPowerMax,
        ProvinceNuclearPowerRelative
    }
}