using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Controllers
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

        /// <summary>
        ///     ユニットツリーのコントローラ
        /// </summary>
        private readonly UnitTreeController _unitTreeController;

        #endregion

        #region 指揮官候補リスト

        /// <summary>
        ///     陸軍指揮官リスト
        /// </summary>
        private List<Leader> _landLeaders;

        /// <summary>
        ///     海軍指揮官リスト
        /// </summary>
        private List<Leader> _navalLeaders;

        /// <summary>
        ///     空軍指揮官リスト
        /// </summary>
        private List<Leader> _airLeaders;

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

        #region プロヴィンスリスト

        /// <summary>
        ///     全プロヴィンスリスト
        /// </summary>
        private List<Province> _provinces = new List<Province>();

        /// <summary>
        ///     陸地プロヴィンスリスト
        /// </summary>
        private readonly List<Province> _landProvinces = new List<Province>();

        /// <summary>
        ///     海洋/海軍基地プロヴィンスリスト
        /// </summary>
        private readonly List<Province> _seaBaseProvinces = new List<Province>();

        /// <summary>
        ///     海軍基地プロヴィンスリスト
        /// </summary>
        private readonly List<Province> _navalBaseProvinces = new List<Province>();

        /// <summary>
        ///     空軍基地プロヴィンスリスト
        /// </summary>
        private readonly List<Province> _airBaseProvinces = new List<Province>();

        /// <summary>
        ///     プロヴィンスリストの初期化済みフラグ
        /// </summary>
        private bool _provincesInitialized;

        #endregion

        #region ユニット種類リスト

        /// <summary>
        ///     陸軍師団のユニット種類
        /// </summary>
        private List<UnitType> _landDivisionTypes;

        /// <summary>
        ///     海軍師団のユニット種類
        /// </summary>
        private List<UnitType> _navalDivisionTypes;

        /// <summary>
        ///     空軍師団のユニット種類
        /// </summary>
        private List<UnitType> _airDivisionTypes;

        /// <summary>
        ///     陸軍旅団のユニット種類
        /// </summary>
        private List<UnitType> _landBrigadeTypes;

        /// <summary>
        ///     海軍旅団のユニット種類
        /// </summary>
        private List<UnitType> _navalBrigadeTypes;

        /// <summary>
        ///     空軍旅団のユニット種類
        /// </summary>
        private List<UnitType> _airBrigadeTypes;

        #endregion

        #endregion

        #region 内部定数

        /// <summary>
        ///     編集項目の編集済みフラグ
        /// </summary>
        private static readonly object[] ItemDirtyFlags =
        {
            Scenario.ItemId.Name,
            Scenario.ItemId.PanelName,
            Scenario.ItemId.StartYear,
            Scenario.ItemId.StartMonth,
            Scenario.ItemId.StartDay,
            Scenario.ItemId.EndYear,
            Scenario.ItemId.EndMonth,
            Scenario.ItemId.EndDay,
            Scenario.ItemId.IncludeFolder,
            Scenario.ItemId.BattleScenario,
            Scenario.ItemId.FreeSelection,
            Scenario.ItemId.AllowDiplomacy,
            Scenario.ItemId.AllowProduction,
            Scenario.ItemId.AllowTechnology,
            Scenario.ItemId.AiAggressive,
            Scenario.ItemId.Difficulty,
            Scenario.ItemId.GameSpeed,
            MajorCountrySettings.ItemId.NameKey,
            MajorCountrySettings.ItemId.NameString,
            MajorCountrySettings.ItemId.FlagExt,
            MajorCountrySettings.ItemId.DescKey,
            MajorCountrySettings.ItemId.DescString,
            MajorCountrySettings.ItemId.PictureName,
            Alliance.ItemId.Name,
            Alliance.ItemId.Type,
            Alliance.ItemId.Id,
            War.ItemId.StartYear,
            War.ItemId.StartMonth,
            War.ItemId.StartDay,
            War.ItemId.EndYear,
            War.ItemId.EndMonth,
            War.ItemId.EndDay,
            War.ItemId.Type,
            War.ItemId.Id,
            War.ItemId.AttackerType,
            War.ItemId.AttackerId,
            War.ItemId.DefenderType,
            War.ItemId.DefenderId,
            Relation.ItemId.Value,
            CountrySettings.ItemId.Master,
            CountrySettings.ItemId.Control,
            Relation.ItemId.Access,
            Relation.ItemId.Guaranteed,
            Relation.ItemId.GuaranteedYear,
            Relation.ItemId.GuaranteedMonth,
            Relation.ItemId.GuaranteedDay,
            null,
            Treaty.ItemId.StartYear,
            Treaty.ItemId.StartMonth,
            Treaty.ItemId.StartDay,
            Treaty.ItemId.EndYear,
            Treaty.ItemId.EndMonth,
            Treaty.ItemId.EndDay,
            Treaty.ItemId.Type,
            Treaty.ItemId.Id,
            null,
            Treaty.ItemId.StartYear,
            Treaty.ItemId.StartMonth,
            Treaty.ItemId.StartDay,
            Treaty.ItemId.EndYear,
            Treaty.ItemId.EndMonth,
            Treaty.ItemId.EndDay,
            Treaty.ItemId.Type,
            Treaty.ItemId.Id,
            SpySettings.ItemId.Spies,
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
            CountrySettings.ItemId.NameKey,
            CountrySettings.ItemId.NameString,
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
            ProvinceSettings.ItemId.NameKey,
            ProvinceSettings.ItemId.NameString,
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
            ProvinceSettings.ItemId.RelativeNuclearPower,
            Unit.ItemId.Type,
            Unit.ItemId.Id,
            Unit.ItemId.Name,
            Unit.ItemId.Location,
            Unit.ItemId.Location,
            Unit.ItemId.Base,
            Unit.ItemId.Base,
            Unit.ItemId.Leader,
            Unit.ItemId.Leader,
            Unit.ItemId.Morale,
            Unit.ItemId.DigIn,
            Division.ItemId.Type,
            Division.ItemId.Id,
            Division.ItemId.Name,
            Division.ItemId.UnitType,
            Division.ItemId.Model,
            Division.ItemId.BrigadeType1,
            Division.ItemId.BrigadeType2,
            Division.ItemId.BrigadeType3,
            Division.ItemId.BrigadeType4,
            Division.ItemId.BrigadeType5,
            Division.ItemId.BirgadeModel1,
            Division.ItemId.BirgadeModel2,
            Division.ItemId.BirgadeModel3,
            Division.ItemId.BirgadeModel4,
            Division.ItemId.BirgadeModel5,
            Division.ItemId.Strength,
            Division.ItemId.MaxStrength,
            Division.ItemId.Organisation,
            Division.ItemId.MaxOrganisation,
            Division.ItemId.Morale,
            Division.ItemId.Experience,
            Division.ItemId.Locked,
            Division.ItemId.Dormant
        };

        /// <summary>
        ///     編集項目の文字列
        /// </summary>
        private static readonly string[] ItemStrings =
        {
            "scenario name",
            "panel name",
            "scenario start year",
            "scenario start month",
            "scenario start day",
            "scenario end year",
            "scenario end month",
            "scenario end day",
            "include folder",
            "battle scenario",
            "free selection",
            "allow diplomacy",
            "allow production",
            "allow technology",
            "ai aggressive",
            "difficulty",
            "game speed",
            "major country name key",
            "major country name string",
            "major flag ext",
            "country desc key",
            "country desc string",
            "propaganda image",
            "alliance name",
            "alliance type",
            "alliance id",
            "war start year",
            "war start month",
            "war start day",
            "war end year",
            "war end month",
            "war end day",
            "war type",
            "war id",
            "war attacker type",
            "war attacker id",
            "war defender type",
            "war defender id",
            "relation value",
            "country master",
            "country control",
            "access",
            "guaranteed",
            "guaranteed year",
            "guaranteed month",
            "guaranteed day",
            "non aggression",
            "non aggression start year",
            "non aggression start month",
            "non aggression start day",
            "non aggression end year",
            "non aggression end month",
            "non aggression end day",
            "non aggression type",
            "non aggression id",
            "peace",
            "peace start year",
            "peace start month",
            "peace start day",
            "peace end year",
            "peace end month",
            "peace end day",
            "peace type",
            "peace id",
            "num of spies",
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
            "country name key",
            "country name string",
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
            "province name key",
            "province name string",
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
            "province relative nuclear power",
            "unit type",
            "unit id",
            "unit name",
            "unit location",
            "unit location",
            "unit base",
            "unit base",
            "unit leader",
            "unit leader",
            "unit morale",
            "unit digin",
            "division type",
            "division id",
            "division name",
            "division unit type",
            "division model",
            "division brigade type1",
            "division brigade type2",
            "division brigade type3",
            "division brigade type4",
            "division brigade type5",
            "division brigade model1",
            "division brigade model2",
            "division brigade model3",
            "division brigade model4",
            "division brigade model5",
            "division strength",
            "division max strength",
            "division organisation",
            "division max organisation",
            "division morale",
            "division experience",
            "division locked",
            "division dormant"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="form">シナリオエディタのフォーム</param>
        /// <param name="mapPanelController">マップパネルのコントローラ</param>
        /// <param name="unitTreeController">ユニットツリーのコントローラ</param>
        public ScenarioEditorController(ScenarioEditorForm form, MapPanelController mapPanelController,
            UnitTreeController unitTreeController)
        {
            _form = form;
            _mapPanelController = mapPanelController;
            _unitTreeController = unitTreeController;
        }

        #endregion

        #region 指揮官候補リスト

        /// <summary>
        ///     指揮官候補リストを更新する
        /// </summary>
        /// <param name="country">選択国</param>
        /// <param name="year">現在年</param>
        public void UpdateLeaderList(Country country, int year)
        {
            List<Leader> leaders = Misc.EnableRetirementYearLeaders
                ? Leaders.Items.Where(
                    leader => (leader.Country == country) &&
                              (year >= leader.StartYear) &&
                              (year < leader.EndYear) &&
                              (year < leader.RetirementYear)).ToList()
                : Leaders.Items.Where(
                    leader => (leader.Country == country) &&
                              (year >= leader.StartYear) &&
                              (year < leader.EndYear)).ToList();
            _landLeaders = leaders.Where(leader => leader.Branch == Branch.Army).ToList();
            _navalLeaders = leaders.Where(leader => leader.Branch == Branch.Navy).ToList();
            _airLeaders = leaders.Where(leader => leader.Branch == Branch.Airforce).ToList();
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

        #region プロヴィンスリスト

        /// <summary>
        ///     プロヴィンスリストを初期化する
        /// </summary>
        public void InitProvinceList()
        {
            // 既に初期化済みならば戻る
            if (_provincesInitialized)
            {
                return;
            }

            _provinces = Provinces.Items.Where(province => province.Id > 0).ToList();
            if (Scenarios.Data.Map != null)
            {
                _provinces = Scenarios.Data.Map.All
                    ? _provinces.Where(province => !Scenarios.Data.Map.No.Contains(province.Id)).ToList()
                    : _provinces.Where(province => Scenarios.Data.Map.Yes.Contains(province.Id)).ToList();
            }
            _landProvinces.Clear();
            _seaBaseProvinces.Clear();
            _navalBaseProvinces.Clear();
            _airBaseProvinces.Clear();
            foreach (Province province in _provinces)
            {
                if (province.IsSea)
                {
                    _seaBaseProvinces.Add(province);
                    continue;
                }
                if (province.IsLand)
                {
                    _landProvinces.Add(province);
                }
                ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);
                if (settings == null)
                {
                    continue;
                }
                if (settings.NavalBase != null)
                {
                    _seaBaseProvinces.Add(province);
                    _navalBaseProvinces.Add(province);
                }
                if (settings.AirBase != null)
                {
                    _airBaseProvinces.Add(province);
                }
            }

            _provincesInitialized = true;
        }

        /// <summary>
        ///     陸地プロヴィンスリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<Province> GetLandProvinces()
        {
            return _landProvinces;
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

        #region ユニット種類リスト

        /// <summary>
        ///     ユニット種類リストを初期化する
        /// </summary>
        public void InitUnitTypeList()
        {
            _landDivisionTypes =
                Units.DivisionTypes.Where(type => Units.Items[(int) type].Branch == Branch.Army).ToList();
            _navalDivisionTypes =
                Units.DivisionTypes.Where(type => Units.Items[(int) type].Branch == Branch.Navy).ToList();
            _airDivisionTypes =
                Units.DivisionTypes.Where(type => Units.Items[(int) type].Branch == Branch.Airforce).ToList();
            _landBrigadeTypes =
                Units.BrigadeTypes.Where(type => Units.Items[(int) type].Branch == Branch.Army).ToList();
            _navalBrigadeTypes =
                Units.BrigadeTypes.Where(type => Units.Items[(int) type].Branch == Branch.Navy).ToList();
            _airBrigadeTypes =
                Units.BrigadeTypes.Where(type => Units.Items[(int) type].Branch == Branch.Airforce).ToList();
        }

        #endregion

        #region 編集項目

        #region 編集項目 - 項目値更新

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        public void UpdateItemValue(TextBox control)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        public void UpdateItemValue(ComboBox control)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.SelectedIndex = (int) GetItemValue(itemId);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        public void UpdateItemValue(CheckBox control)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Checked = (bool) GetItemValue(itemId);
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="major">主要国設定</param>
        public void UpdateItemValue(TextBox control, MajorCountrySettings major)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, major));
            switch (itemId)
            {
                case ScenarioEditorItemId.MajorCountryNameString:
                    // 主要国設定の国名定義がなければ編集不可
                    control.ReadOnly = string.IsNullOrEmpty(major.Name);
                    break;

                case ScenarioEditorItemId.MajorCountryDescString:
                    // 主要国設定の説明文定義がなければ編集不可
                    control.ReadOnly = string.IsNullOrEmpty(major.Desc);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="alliance">同盟</param>
        public void UpdateItemValue(TextBox control, Alliance alliance)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, alliance));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="war">戦争</param>
        public void UpdateItemValue(TextBox control, War war)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, war));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="relation">国家関係</param>
        public void UpdateItemValue(TextBox control, Relation relation)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, relation));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="relation">国家関係</param>
        public void UpdateItemValue(CheckBox control, Relation relation)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Checked = (bool) GetItemValue(itemId, relation);
        }

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
        /// <param name="spy">諜報設定</param>
        public void UpdateItemValue(NumericUpDown control, SpySettings spy)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Value = (int) GetItemValue(itemId, spy);
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
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryNameString:
                    // 国家設定の国名定義がないか、文字列直接埋め込み形式ならば編集不可
                    control.ReadOnly = string.IsNullOrEmpty(settings?.Name) || !Config.ExistsKey(settings.Name);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="country">選択国</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemValue(CheckBox control, Country country, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Checked = (Country) GetItemValue(itemId, settings) == country;
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
                    control.Checked = settings.Capital == province.Id;
                    control.Enabled = !control.Checked;
                    break;

                case ScenarioEditorItemId.CountryCoreProvinces:
                    control.Checked = settings.NationalProvinces.Contains(province.Id);
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    control.Checked = settings.OwnedProvinces.Contains(province.Id);
                    control.Enabled = !control.Checked;
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    control.Checked = settings.ControlledProvinces.Contains(province.Id);
                    control.Enabled = !control.Checked;
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    control.Checked = settings.ClaimedProvinces.Contains(province.Id);
                    control.Enabled = Game.Type == GameType.DarkestHour;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        public void UpdateItemValue(Control control, Province province)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, province));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemValue(Control control, ProvinceSettings settings)
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
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceNameString:
                    // プロヴィンス設定のプロヴィンス名定義がなければ編集不可
                    control.ReadOnly = string.IsNullOrEmpty(settings?.Name);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="unit">ユニット</param>
        public void UpdateItemValue(TextBox control, Unit unit)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, unit));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="unit">ユニット</param>
        public void UpdateItemValue(ComboBox control, Unit unit)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = GetItemValue(itemId, unit);
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitLocation:
                case ScenarioEditorItemId.UnitBase:
                    List<Province> provinces = (List<Province>) GetListItems(itemId, unit);
                    control.SelectedIndex = provinces?.FindIndex(province => province.Id == (int) val) ?? -1;
                    break;

                case ScenarioEditorItemId.UnitLeader:
                    List<Leader> leaders = (List<Leader>) GetListItems(itemId, unit);
                    control.SelectedIndex = leaders.FindIndex(leader => leader.Id == (int) val);
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="division">師団</param>
        public void UpdateItemValue(TextBox control, Division division)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Text = ObjectHelper.ToString(GetItemValue(itemId, division));
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="division">師団</param>
        public void UpdateItemValue(ComboBox control, Division division)
        {
            List<UnitType> types;
            int max;

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            object val = GetItemValue(itemId, division);
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionUnitType:
                    types = (List<UnitType>) GetListItems(itemId, division);
                    control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType1:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 0)
                    {
                        types = (List<UnitType>) GetListItems(itemId, division);
                        control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType2:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 1)
                    {
                        types = (List<UnitType>) GetListItems(itemId, division);
                        control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType3:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 2)
                    {
                        types = (List<UnitType>) GetListItems(itemId, division);
                        control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType4:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 3)
                    {
                        types = (List<UnitType>) GetListItems(itemId, division);
                        control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType5:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 4)
                    {
                        types = (List<UnitType>) GetListItems(itemId, division);
                        control.SelectedIndex = types.FindIndex(type => type == (UnitType) val);
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionModel:
                    if ((int) val < 0)
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                    }
                    else if ((int) val < Units.Items[(int) division.Type].Models.Count)
                    {
                        control.SelectedIndex = (int) val;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = ObjectHelper.ToString(val);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel1:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 0)
                    {
                        if ((int) val < 0)
                        {
                            control.SelectedIndex = -1;
                            control.Text = "";
                        }
                        else if ((int) val < Units.Items[(int) division.Extra1].Models.Count)
                        {
                            control.SelectedIndex = (int) val;
                        }
                        else
                        {
                            control.SelectedIndex = -1;
                            control.Text = ObjectHelper.ToString(val);
                        }
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel2:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 1)
                    {
                        if ((int) val < 0)
                        {
                            control.SelectedIndex = -1;
                            control.Text = "";
                        }
                        else if ((int) val < Units.Items[(int) division.Extra2].Models.Count)
                        {
                            control.SelectedIndex = (int) val;
                        }
                        else
                        {
                            control.SelectedIndex = -1;
                            control.Text = ObjectHelper.ToString(val);
                        }
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel3:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 2)
                    {
                        if ((int) val < 0)
                        {
                            control.SelectedIndex = -1;
                            control.Text = "";
                        }
                        else if ((int) val >= 0 && (int) val < Units.Items[(int) division.Extra3].Models.Count)
                        {
                            control.SelectedIndex = (int) val;
                        }
                        else
                        {
                            control.SelectedIndex = -1;
                            control.Text = ObjectHelper.ToString(val);
                        }
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel4:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 3)
                    {
                        if ((int) val < 0)
                        {
                            control.SelectedIndex = -1;
                            control.Text = "";
                        }
                        else if ((int) val < Units.Items[(int) division.Extra4].Models.Count)
                        {
                            control.SelectedIndex = (int) val;
                        }
                        else
                        {
                            control.SelectedIndex = -1;
                            control.Text = ObjectHelper.ToString(val);
                        }
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                        control.Enabled = false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel5:
                    max = Units.Items[(int) division.Type].GetMaxAllowedBrigades();
                    if (max > 4)
                    {
                        if ((int) val < 0)
                        {
                            control.SelectedIndex = -1;
                            control.Text = "";
                        }
                        else if ((int) val < Units.Items[(int) division.Extra5].Models.Count)
                        {
                            control.SelectedIndex = (int) val;
                        }
                        else
                        {
                            control.SelectedIndex = -1;
                            control.Text = ObjectHelper.ToString(val);
                        }
                        control.Enabled = true;
                    }
                    else
                    {
                        control.SelectedIndex = -1;
                        control.Text = "";
                        control.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="division">師団</param>
        public void UpdateItemValue(CheckBox control, Division division)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.Checked = (bool) GetItemValue(itemId, division);
        }

        #endregion

        #region 編集項目 - リスト項目更新

        /// <summary>
        ///     リスト項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="unit">ユニット</param>
        public void UpdateListItems(ComboBox control, Unit unit)
        {
            control.BeginUpdate();
            control.Items.Clear();
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitLocation:
                case ScenarioEditorItemId.UnitBase:
                    List<Province> provinces = (List<Province>) GetListItems(itemId, unit);
                    if (provinces != null)
                    {
                        foreach (Province province in provinces)
                        {
                            ProvinceSettings settings = Scenarios.GetProvinceSettings(province.Id);
                            control.Items.Add(Scenarios.GetProvinceName(province, settings));
                        }
                    }
                    break;

                case ScenarioEditorItemId.UnitLeader:
                    List<Leader> leaders = (List<Leader>) GetListItems(itemId, unit);
                    foreach (Leader leader in leaders)
                    {
                        control.Items.Add(leader.Name);
                    }
                    break;
            }
            control.EndUpdate();
        }

        /// <summary>
        ///     リスト項目の値を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        public void UpdateListItems(ComboBox control, Division division, CountrySettings settings)
        {
            control.BeginUpdate();
            control.Items.Clear();
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            UnitClass uc;
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionUnitType:
                case ScenarioEditorItemId.DivisionBrigadeType1:
                case ScenarioEditorItemId.DivisionBrigadeType2:
                case ScenarioEditorItemId.DivisionBrigadeType3:
                case ScenarioEditorItemId.DivisionBrigadeType4:
                case ScenarioEditorItemId.DivisionBrigadeType5:
                    List<UnitType> types = (List<UnitType>) GetListItems(itemId, division);
                    foreach (UnitType type in types)
                    {
                        control.Items.Add(Units.Items[(int) type]);
                    }
                    break;

                case ScenarioEditorItemId.DivisionModel:
                    uc = Units.Items[(int) division.Type];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel1:
                    uc = Units.Items[(int) division.Extra1];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel2:
                    uc = Units.Items[(int) division.Extra2];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel3:
                    uc = Units.Items[(int) division.Extra3];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel4:
                    uc = Units.Items[(int) division.Extra4];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel5:
                    uc = Units.Items[(int) division.Extra5];
                    for (int i = 0; i < uc.Models.Count; i++)
                    {
                        string name = uc.GetCountryModelName(i, settings.Country);
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uc.GetModelName(i);
                        }
                        control.Items.Add(name);
                    }
                    break;
            }
            control.EndUpdate();
        }

        #endregion

        #region 編集項目 - 項目色更新

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        public void UpdateItemColor(Control control)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="major">主要国設定</param>
        public void UpdateItemColor(Control control, MajorCountrySettings major)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, major) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="alliance">同盟</param>
        public void UpdateItemColor(Control control, Alliance alliance)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, alliance) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="war">戦争</param>
        public void UpdateItemColor(Control control, War war)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, war) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="relation">国家関係</param>
        public void UpdateItemColor(Control control, Relation relation)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, relation) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="treaty">協定</param>
        public void UpdateItemColor(Control control, Treaty treaty)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, treaty) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="spy">諜報設定</param>
        public void UpdateItemColor(Control control, SpySettings spy)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, spy) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemColor(Control control, CountrySettings settings)
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
        public void UpdateItemColor(Control control, Country country, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        public void UpdateItemColor(Control control, Province province, CountrySettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, province, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void UpdateItemColor(Control control, ProvinceSettings settings)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, settings) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="unit">ユニット</param>
        public void UpdateItemColor(Control control, Unit unit)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, unit) ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="division">師団</param>
        public void UpdateItemColor(Control control, Division division)
        {
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            control.ForeColor = IsItemDirty(itemId, division) ? Color.Red : SystemColors.WindowText;
        }

        #endregion

        #region 編集項目 - 項目値取得

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId)
        {
            Scenario scenario = Scenarios.Data;

            switch (itemId)
            {
                case ScenarioEditorItemId.ScenarioName:
                    return Config.ExistsKey(scenario.Name) ? Config.GetText(scenario.Name) : scenario.Name;

                case ScenarioEditorItemId.ScenarioPanelName:
                    return scenario.PanelName;

                case ScenarioEditorItemId.ScenarioStartYear:
                    return scenario.GlobalData.StartDate?.Year;

                case ScenarioEditorItemId.ScenarioStartMonth:
                    return scenario.GlobalData.StartDate?.Month;

                case ScenarioEditorItemId.ScenarioStartDay:
                    return scenario.GlobalData.StartDate?.Day;

                case ScenarioEditorItemId.ScenarioEndYear:
                    return scenario.GlobalData.EndDate?.Year;

                case ScenarioEditorItemId.ScenarioEndMonth:
                    return scenario.GlobalData.EndDate?.Month;

                case ScenarioEditorItemId.ScenarioEndDay:
                    return scenario.GlobalData.EndDate?.Day;

                case ScenarioEditorItemId.ScenarioIncludeFolder:
                    return scenario.IncludeFolder;

                case ScenarioEditorItemId.ScenarioBattleScenario:
                    return scenario.Header.IsBattleScenario;

                case ScenarioEditorItemId.ScenarioFreeSelection:
                    return scenario.Header.IsFreeSelection;

                case ScenarioEditorItemId.ScenarioAllowDiplomacy:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowDiplomacy;

                case ScenarioEditorItemId.ScenarioAllowProduction:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowProduction;

                case ScenarioEditorItemId.ScenarioAllowTechnology:
                    return (scenario.GlobalData.Rules == null) || scenario.GlobalData.Rules.AllowTechnology;

                case ScenarioEditorItemId.ScenarioAiAggressive:
                    return scenario.Header.AiAggressive;

                case ScenarioEditorItemId.ScenarioDifficulty:
                    return scenario.Header.Difficulty;

                case ScenarioEditorItemId.ScenarioGameSpeed:
                    return scenario.Header.GameSpeed;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, MajorCountrySettings major)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.MajorCountryNameKey:
                    return major.Name;

                case ScenarioEditorItemId.MajorCountryNameString:
                    return Scenarios.GetCountryName(major.Country);

                case ScenarioEditorItemId.MajorFlagExt:
                    return major.FlagExt;

                case ScenarioEditorItemId.MajorCountryDescKey:
                    return major.Desc;

                case ScenarioEditorItemId.MajorCountryDescString:
                    if (!string.IsNullOrEmpty(major.Desc))
                    {
                        return Config.GetText(major.Desc);
                    }
                    int year = Scenarios.Data.GlobalData.StartDate != null
                        ? Scenarios.Data.GlobalData.StartDate.Year
                        : Scenarios.Data.Header.StartDate != null
                            ? Scenarios.Data.Header.StartDate.Year
                            : Scenarios.Data.Header.StartYear;
                    // 年数の下2桁のみ使用する
                    year = year % 100;
                    // 年数別の説明があれば使用する
                    string key = $"{major.Country}_{year}_DESC";
                    if (Config.ExistsKey(key))
                    {
                        return Config.GetText(key);
                    }
                    key = $"{major.Country}_DESC";
                    return Config.GetText(key);

                case ScenarioEditorItemId.MajorPropaganada:
                    return major.PictureName;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="alliance">同盟</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Alliance alliance)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.AllianceName:
                    if (!string.IsNullOrEmpty(alliance.Name))
                    {
                        return Config.GetText("ALLIANCE_" + alliance.Name);
                    }
                    ScenarioGlobalData data = Scenarios.Data.GlobalData;
                    if (alliance == data.Axis)
                    {
                        return Config.GetText(TextId.AllianceAxis);
                    }
                    if (alliance == data.Allies)
                    {
                        return Config.GetText(TextId.AllianceAllies);
                    }
                    if (alliance == data.Comintern)
                    {
                        return Config.GetText(TextId.AllianceComintern);
                    }
                    return "";

                case ScenarioEditorItemId.AllianceType:
                    return alliance.Id?.Type;

                case ScenarioEditorItemId.AllianceId:
                    return alliance.Id?.Id;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="war">戦争</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, War war)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.WarStartYear:
                    return war.StartDate?.Year;

                case ScenarioEditorItemId.WarStartMonth:
                    return war.StartDate?.Month;

                case ScenarioEditorItemId.WarStartDay:
                    return war.StartDate?.Day;

                case ScenarioEditorItemId.WarEndYear:
                    return war.EndDate?.Year;

                case ScenarioEditorItemId.WarEndMonth:
                    return war.EndDate?.Month;

                case ScenarioEditorItemId.WarEndDay:
                    return war.EndDate?.Day;

                case ScenarioEditorItemId.WarType:
                    return war.Id?.Type;

                case ScenarioEditorItemId.WarId:
                    return war.Id?.Id;

                case ScenarioEditorItemId.WarAttackerType:
                    return war.Attackers?.Id?.Type;

                case ScenarioEditorItemId.WarAttackerId:
                    if (war.Id == null)
                    {
                        return null;
                    }
                    return war.Attackers.Id.Id;

                case ScenarioEditorItemId.WarDefenderType:
                    return war.Defenders?.Id?.Type;

                case ScenarioEditorItemId.WarDefenderId:
                    if (war.Id == null)
                    {
                        return null;
                    }
                    return war.Defenders.Id.Id;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="relation">国家関係</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Relation relation)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyRelationValue:
                    if (relation == null)
                    {
                        return (double) 0;
                    }
                    return relation.Value;

                case ScenarioEditorItemId.DiplomacyMilitaryAccess:
                    return (relation != null) && relation.Access;

                case ScenarioEditorItemId.DiplomacyGuaranteed:
                    return relation?.Guaranteed != null;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndYear:
                    return relation?.Guaranteed?.Year;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndMonth:
                    return relation?.Guaranteed?.Month;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndDay:
                    return relation?.Guaranteed?.Day;
            }

            return null;
        }

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
                case ScenarioEditorItemId.DiplomacyNonAggression:
                case ScenarioEditorItemId.DiplomacyPeace:
                    return treaty != null;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartYear:
                case ScenarioEditorItemId.DiplomacyPeaceStartYear:
                case ScenarioEditorItemId.TradeStartYear:
                    return treaty?.StartDate?.Year;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartMonth:
                case ScenarioEditorItemId.DiplomacyPeaceStartMonth:
                case ScenarioEditorItemId.TradeStartMonth:
                    return treaty?.StartDate?.Month;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartDay:
                case ScenarioEditorItemId.DiplomacyPeaceStartDay:
                case ScenarioEditorItemId.TradeStartDay:
                    return treaty?.StartDate?.Day;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndYear:
                case ScenarioEditorItemId.DiplomacyPeaceEndYear:
                case ScenarioEditorItemId.TradeEndYear:
                    return treaty?.EndDate?.Year;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndMonth:
                case ScenarioEditorItemId.DiplomacyPeaceEndMonth:
                case ScenarioEditorItemId.TradeEndMonth:
                    return treaty?.EndDate?.Month;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndDay:
                case ScenarioEditorItemId.DiplomacyPeaceEndDay:
                case ScenarioEditorItemId.TradeEndDay:
                    return treaty?.EndDate?.Day;

                case ScenarioEditorItemId.DiplomacyNonAggressionType:
                case ScenarioEditorItemId.DiplomacyPeaceType:
                case ScenarioEditorItemId.TradeType:
                    return treaty?.Id?.Type;

                case ScenarioEditorItemId.DiplomacyNonAggressionId:
                case ScenarioEditorItemId.DiplomacyPeaceId:
                case ScenarioEditorItemId.TradeId:
                    return treaty?.Id?.Id;

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
        /// <param name="spy">諜報設定</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, SpySettings spy)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.IntelligenceSpies:
                    return spy?.Spies ?? 0;
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
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyMaster:
                    return settings?.Master ?? Country.None;

                case ScenarioEditorItemId.DiplomacyMilitaryControl:
                    return settings?.Control ?? Country.None;

                case ScenarioEditorItemId.CountryNameKey:
                    return settings != null ? settings.Name : "";

                case ScenarioEditorItemId.CountryFlagExt:
                    return settings != null ? settings.FlagExt : "";

                case ScenarioEditorItemId.CountryRegularId:
                    return settings?.RegularId ?? Country.None;

                case ScenarioEditorItemId.CountryBelligerence:
                    return settings?.Belligerence ?? 0;

                case ScenarioEditorItemId.CountryDissent:
                    return settings?.Dissent ?? 0;

                case ScenarioEditorItemId.CountryExtraTc:
                    return settings?.ExtraTc ?? 0;

                case ScenarioEditorItemId.CountryNuke:
                    return settings?.Nuke ?? 0;

                case ScenarioEditorItemId.CountryNukeYear:
                    return settings?.NukeDate?.Year;

                case ScenarioEditorItemId.CountryNukeMonth:
                    return settings?.NukeDate?.Month;

                case ScenarioEditorItemId.CountryNukeDay:
                    return settings?.NukeDate?.Day;

                case ScenarioEditorItemId.CountryGroundDefEff:
                    return settings?.GroundDefEff ?? 0;

                case ScenarioEditorItemId.CountryPeacetimeIcModifier:
                    return settings?.PeacetimeIcModifier ?? 0;

                case ScenarioEditorItemId.CountryWartimeIcModifier:
                    return settings?.WartimeIcModifier ?? 0;

                case ScenarioEditorItemId.CountryIndustrialModifier:
                    return settings?.IndustrialModifier ?? 0;

                case ScenarioEditorItemId.CountryRelativeManpower:
                    return settings?.RelativeManpower ?? 0;

                case ScenarioEditorItemId.CountryEnergy:
                    return settings?.Energy ?? 0;

                case ScenarioEditorItemId.CountryMetal:
                    return settings?.Metal ?? 0;

                case ScenarioEditorItemId.CountryRareMaterials:
                    return settings?.RareMaterials ?? 0;

                case ScenarioEditorItemId.CountryOil:
                    return settings?.Oil ?? 0;

                case ScenarioEditorItemId.CountrySupplies:
                    return settings?.Supplies ?? 0;

                case ScenarioEditorItemId.CountryMoney:
                    return settings?.Money ?? 0;

                case ScenarioEditorItemId.CountryTransports:
                    return settings?.Transports ?? 0;

                case ScenarioEditorItemId.CountryEscorts:
                    return settings?.Escorts ?? 0;

                case ScenarioEditorItemId.CountryManpower:
                    return settings?.Manpower ?? 0;

                case ScenarioEditorItemId.CountryOffmapEnergy:
                    return settings?.Offmap?.Energy ?? 0;

                case ScenarioEditorItemId.CountryOffmapMetal:
                    return settings?.Offmap?.Metal ?? 0;

                case ScenarioEditorItemId.CountryOffmapRareMaterials:
                    return settings?.Offmap?.RareMaterials ?? 0;

                case ScenarioEditorItemId.CountryOffmapOil:
                    return settings?.Offmap?.Oil ?? 0;

                case ScenarioEditorItemId.CountryOffmapSupplies:
                    return settings?.Offmap?.Supplies ?? 0;

                case ScenarioEditorItemId.CountryOffmapMoney:
                    return settings?.Offmap?.Money ?? 0;

                case ScenarioEditorItemId.CountryOffmapTransports:
                    return settings?.Offmap?.Transports ?? 0;

                case ScenarioEditorItemId.CountryOffmapEscorts:
                    return settings?.Offmap?.Escorts ?? 0;

                case ScenarioEditorItemId.CountryOffmapManpower:
                    return settings?.Offmap?.Manpower ?? 0;

                case ScenarioEditorItemId.CountryOffmapIc:
                    return settings?.Offmap?.Ic ?? 0;

                case ScenarioEditorItemId.CountryAiFileName:
                    return settings != null ? settings.AiFileName : "";

                case ScenarioEditorItemId.SliderYear:
                    return settings?.Policy?.Date?.Year;

                case ScenarioEditorItemId.SliderMonth:
                    return settings?.Policy?.Date?.Month;

                case ScenarioEditorItemId.SliderDay:
                    return settings?.Policy?.Date?.Day;

                case ScenarioEditorItemId.SliderDemocratic:
                    return settings?.Policy?.Democratic ?? 5;

                case ScenarioEditorItemId.SliderPoliticalLeft:
                    return settings?.Policy?.PoliticalLeft ?? 5;

                case ScenarioEditorItemId.SliderFreedom:
                    return settings?.Policy?.Freedom ?? 5;

                case ScenarioEditorItemId.SliderFreeMarket:
                    return settings?.Policy?.FreeMarket ?? 5;

                case ScenarioEditorItemId.SliderProfessionalArmy:
                    return settings?.Policy?.ProfessionalArmy ?? 5;

                case ScenarioEditorItemId.SliderDefenseLobby:
                    return settings?.Policy?.DefenseLobby ?? 5;

                case ScenarioEditorItemId.SliderInterventionism:
                    return settings?.Policy?.Interventionism ?? 5;

                case ScenarioEditorItemId.CabinetHeadOfState:
                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    return settings?.HeadOfState?.Id;

                case ScenarioEditorItemId.CabinetHeadOfStateType:
                    return settings?.HeadOfState?.Type;

                case ScenarioEditorItemId.CabinetHeadOfGovernment:
                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    return settings?.HeadOfGovernment?.Id;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    return settings?.HeadOfGovernment?.Type;

                case ScenarioEditorItemId.CabinetForeignMinister:
                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    return settings?.ForeignMinister?.Id;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    return settings?.ForeignMinister?.Type;

                case ScenarioEditorItemId.CabinetArmamentMinister:
                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    return settings?.ArmamentMinister?.Id;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    return settings?.ArmamentMinister?.Type;

                case ScenarioEditorItemId.CabinetMinisterOfSecurity:
                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    return settings?.MinisterOfSecurity?.Id;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    return settings?.MinisterOfSecurity?.Type;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligence:
                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    return settings?.MinisterOfIntelligence?.Id;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    return settings?.MinisterOfIntelligence?.Type;

                case ScenarioEditorItemId.CabinetChiefOfStaff:
                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    return settings?.ChiefOfStaff?.Id;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    return settings?.ChiefOfStaff?.Type;

                case ScenarioEditorItemId.CabinetChiefOfArmy:
                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    return settings?.ChiefOfArmy?.Id;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    return settings?.ChiefOfArmy?.Type;

                case ScenarioEditorItemId.CabinetChiefOfNavy:
                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    return settings?.ChiefOfNavy?.Id;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    return settings?.ChiefOfNavy?.Type;

                case ScenarioEditorItemId.CabinetChiefOfAir:
                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    return settings?.ChiefOfAir?.Id;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    return settings?.ChiefOfAir?.Type;
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
                case ScenarioEditorItemId.CountryNameString:
                    if (!string.IsNullOrEmpty(settings?.Name))
                    {
                        return Config.ExistsKey(settings.Name) ? Config.GetText(settings.Name) : "";
                    }
                    return Scenarios.GetCountryName(country);
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
                return false;
            }

            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCapital:
                    return settings.Capital == province.Id;

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
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceVp:
                    return settings?.Vp ?? 0;

                case ScenarioEditorItemId.ProvinceRevoltRisk:
                    return settings?.RevoltRisk ?? 0;

                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                    return settings?.Manpower ?? 0;

                case ScenarioEditorItemId.ProvinceManpowerMax:
                    return settings?.MaxManpower ?? 0;

                case ScenarioEditorItemId.ProvinceEnergyPool:
                    return settings?.EnergyPool ?? 0;

                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                    return settings?.Energy ?? 0;

                case ScenarioEditorItemId.ProvinceEnergyMax:
                    return settings?.MaxEnergy ?? 0;

                case ScenarioEditorItemId.ProvinceMetalPool:
                    return settings?.MetalPool ?? 0;

                case ScenarioEditorItemId.ProvinceMetalCurrent:
                    return settings?.Metal ?? 0;

                case ScenarioEditorItemId.ProvinceMetalMax:
                    return settings?.MaxMetal ?? 0;

                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                    return settings?.RareMaterialsPool ?? 0;

                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                    return settings?.RareMaterials ?? 0;

                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                    return settings?.MaxRareMaterials ?? 0;

                case ScenarioEditorItemId.ProvinceOilPool:
                    return settings?.OilPool ?? 0;

                case ScenarioEditorItemId.ProvinceOilCurrent:
                    return settings?.Oil ?? 0;

                case ScenarioEditorItemId.ProvinceOilMax:
                    return settings?.MaxOil ?? 0;

                case ScenarioEditorItemId.ProvinceSupplyPool:
                    return settings?.SupplyPool ?? 0;

                case ScenarioEditorItemId.ProvinceIcCurrent:
                    return settings?.Ic?.CurrentSize;

                case ScenarioEditorItemId.ProvinceIcMax:
                    return settings?.Ic?.MaxSize;

                case ScenarioEditorItemId.ProvinceIcRelative:
                    return settings?.Ic?.Size;

                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                    return settings?.Infrastructure?.CurrentSize;

                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                    return settings?.Infrastructure?.MaxSize;

                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                    return settings?.Infrastructure?.Size;

                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                    return settings?.LandFort?.CurrentSize;

                case ScenarioEditorItemId.ProvinceLandFortMax:
                    return settings?.LandFort?.MaxSize;

                case ScenarioEditorItemId.ProvinceLandFortRelative:
                    return settings?.LandFort?.Size;

                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                    return settings?.CoastalFort?.CurrentSize;

                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                    return settings?.CoastalFort?.MaxSize;

                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                    return settings?.CoastalFort?.Size;

                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                    return settings?.AntiAir?.CurrentSize;

                case ScenarioEditorItemId.ProvinceAntiAirMax:
                    return settings?.AntiAir?.MaxSize;

                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                    return settings?.AntiAir?.Size;

                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                    return settings?.AirBase?.CurrentSize;

                case ScenarioEditorItemId.ProvinceAirBaseMax:
                    return settings?.AirBase?.MaxSize;

                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                    return settings?.AirBase?.Size;

                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                    return settings?.NavalBase?.CurrentSize;

                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                    return settings?.NavalBase?.MaxSize;

                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                    return settings?.NavalBase?.Size;

                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                    return settings?.RadarStation?.CurrentSize;

                case ScenarioEditorItemId.ProvinceRadarStationMax:
                    return settings?.RadarStation?.MaxSize;

                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                    return settings?.RadarStation?.Size;

                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                    return settings?.NuclearReactor?.CurrentSize;

                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                    return settings?.NuclearReactor?.MaxSize;

                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                    return settings?.NuclearReactor?.Size;

                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                    return settings?.RocketTest?.CurrentSize;

                case ScenarioEditorItemId.ProvinceRocketTestMax:
                    return settings?.RocketTest?.MaxSize;

                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                    return settings?.RocketTest?.Size;

                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                    return settings?.SyntheticOil?.CurrentSize;

                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                    return settings?.SyntheticOil?.MaxSize;

                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                    return settings?.SyntheticOil?.Size;

                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                    return settings?.SyntheticRares?.CurrentSize;

                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                    return settings?.SyntheticRares?.MaxSize;

                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                    return settings?.SyntheticRares?.Size;

                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                    return settings?.NuclearPower?.CurrentSize;

                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                    return settings?.NuclearPower?.MaxSize;

                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                    return settings?.NuclearPower?.Size;
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
                case ScenarioEditorItemId.ProvinceNameKey:
                    return !string.IsNullOrEmpty(settings?.Name) ? settings.Name : "";

                case ScenarioEditorItemId.ProvinceNameString:
                    return Scenarios.GetProvinceName(province, settings);
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="unit">ユニット</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitType:
                    return unit.Id?.Type;

                case ScenarioEditorItemId.UnitId:
                    return unit.Id?.Id;

                case ScenarioEditorItemId.UnitName:
                    return unit.Name;

                case ScenarioEditorItemId.UnitLocationId:
                case ScenarioEditorItemId.UnitLocation:
                    return unit.Location;

                case ScenarioEditorItemId.UnitBaseId:
                case ScenarioEditorItemId.UnitBase:
                    if (unit.Branch == Branch.Army)
                    {
                        return null;
                    }
                    return unit.Base;

                case ScenarioEditorItemId.UnitLeaderId:
                case ScenarioEditorItemId.UnitLeader:
                    return unit.Leader;

                case ScenarioEditorItemId.UnitMorale:
                    return unit.Morale;

                case ScenarioEditorItemId.UnitDigIn:
                    if (unit.Branch == Branch.Navy || unit.Branch == Branch.Airforce)
                    {
                        return null;
                    }
                    return unit.DigIn;
            }

            return null;
        }

        /// <summary>
        ///     編集項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="division">師団</param>
        /// <returns>編集項目の値</returns>
        public object GetItemValue(ScenarioEditorItemId itemId, Division division)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionType:
                    return division.Id?.Type;

                case ScenarioEditorItemId.DivisionId:
                    return division.Id?.Id;

                case ScenarioEditorItemId.DivisionName:
                    return division.Name;

                case ScenarioEditorItemId.DivisionUnitType:
                    return division.Type;

                case ScenarioEditorItemId.DivisionModel:
                    return division.Model;

                case ScenarioEditorItemId.DivisionBrigadeType1:
                    return division.Extra1;

                case ScenarioEditorItemId.DivisionBrigadeType2:
                    return division.Extra2;

                case ScenarioEditorItemId.DivisionBrigadeType3:
                    return division.Extra3;

                case ScenarioEditorItemId.DivisionBrigadeType4:
                    return division.Extra4;

                case ScenarioEditorItemId.DivisionBrigadeType5:
                    return division.Extra5;

                case ScenarioEditorItemId.DivisionBrigadeModel1:
                    return division.BrigadeModel1;

                case ScenarioEditorItemId.DivisionBrigadeModel2:
                    return division.BrigadeModel2;

                case ScenarioEditorItemId.DivisionBrigadeModel3:
                    return division.BrigadeModel3;

                case ScenarioEditorItemId.DivisionBrigadeModel4:
                    return division.BrigadeModel4;

                case ScenarioEditorItemId.DivisionBrigadeModel5:
                    return division.BrigadeModel5;

                case ScenarioEditorItemId.DivisionStrength:
                    return division.Strength;

                case ScenarioEditorItemId.DivisionMaxStrength:
                    return division.MaxStrength;

                case ScenarioEditorItemId.DivisionOrganisation:
                    return division.Organisation;

                case ScenarioEditorItemId.DivisionMaxOrganisation:
                    return division.MaxOrganisation;

                case ScenarioEditorItemId.DivisionMorale:
                    return division.Morale;

                case ScenarioEditorItemId.DivisionExperience:
                    return division.Experience;

                case ScenarioEditorItemId.DivisionLocked:
                    return division.Locked;

                case ScenarioEditorItemId.DivisionDormant:
                    return division.Dormant;
            }

            return null;
        }

        #endregion

        #region 編集項目 - リスト項目取得

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
        ///     項目値のリストを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="unit">ユニット</param>
        /// <returns>項目値のリスト</returns>
        public object GetListItems(ScenarioEditorItemId itemId, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitLocation:
                    switch (unit.Branch)
                    {
                        case Branch.Army:
                            return _landProvinces;

                        case Branch.Navy:
                            return _seaBaseProvinces;

                        case Branch.Airforce:
                            return _provinces;
                    }
                    break;

                case ScenarioEditorItemId.UnitBase:
                    switch (unit.Branch)
                    {
                        case Branch.Navy:
                            return _navalBaseProvinces;

                        case Branch.Airforce:
                            return _airBaseProvinces;
                    }
                    break;

                case ScenarioEditorItemId.UnitLeader:
                    switch (unit.Branch)
                    {
                        case Branch.Army:
                            return _landLeaders;

                        case Branch.Navy:
                            return _navalLeaders;

                        case Branch.Airforce:
                            return _airLeaders;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        ///     項目値のリストを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="division">師団</param>
        /// <returns>項目値のリスト</returns>
        public object GetListItems(ScenarioEditorItemId itemId, Division division)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionUnitType:
                    switch (division.Branch)
                    {
                        case Branch.Army:
                            return _landDivisionTypes;

                        case Branch.Navy:
                            return _navalDivisionTypes;

                        case Branch.Airforce:
                            return _airDivisionTypes;
                    }
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType1:
                case ScenarioEditorItemId.DivisionBrigadeType2:
                case ScenarioEditorItemId.DivisionBrigadeType3:
                case ScenarioEditorItemId.DivisionBrigadeType4:
                case ScenarioEditorItemId.DivisionBrigadeType5:
                    switch (division.Branch)
                    {
                        case Branch.Army:
                            return _landBrigadeTypes;

                        case Branch.Navy:
                            return _navalBrigadeTypes;

                        case Branch.Airforce:
                            return _airBrigadeTypes;
                    }
                    break;
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
            switch (itemId)
            {
                case ScenarioEditorItemId.TradeCountry1:
                case ScenarioEditorItemId.TradeCountry2:
                    return index >= 0 ? Countries.Tags[index] : Country.None;

                case ScenarioEditorItemId.CountryRegularId:
                    return index > 0 ? Countries.Tags[index - 1] : Country.None;

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
                    if (index < 0)
                    {
                        return null;
                    }
                    return ((List<Minister>) GetListItems(itemId))[index].Id;
            }

            return null;
        }

        /// <summary>
        ///     リスト項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="index">リストのインデックス</param>
        /// <param name="unit">ユニット</param>
        /// <returns>リスト項目の値</returns>
        public object GetListItemValue(ScenarioEditorItemId itemId, int index, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitLocation:
                case ScenarioEditorItemId.UnitBase:
                    return ((List<Province>) GetListItems(itemId, unit))[index].Id;

                case ScenarioEditorItemId.UnitLeader:
                    return ((List<Leader>) GetListItems(itemId, unit))[index].Id;
            }

            return null;
        }

        /// <summary>
        ///     リスト項目の値を取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="index">リストのインデックス</param>
        /// <param name="division">師団</param>
        /// <returns>リスト項目の値</returns>
        public object GetListItemValue(ScenarioEditorItemId itemId, int index, Division division)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionUnitType:
                case ScenarioEditorItemId.DivisionBrigadeType1:
                case ScenarioEditorItemId.DivisionBrigadeType2:
                case ScenarioEditorItemId.DivisionBrigadeType3:
                case ScenarioEditorItemId.DivisionBrigadeType4:
                case ScenarioEditorItemId.DivisionBrigadeType5:
                    return ((List<UnitType>) GetListItems(itemId, division))[index];

                case ScenarioEditorItemId.DivisionModel:
                case ScenarioEditorItemId.DivisionBrigadeModel1:
                case ScenarioEditorItemId.DivisionBrigadeModel2:
                case ScenarioEditorItemId.DivisionBrigadeModel3:
                case ScenarioEditorItemId.DivisionBrigadeModel4:
                case ScenarioEditorItemId.DivisionBrigadeModel5:
                    return index;
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
        public void SetItemValue(ScenarioEditorItemId itemId, object val)
        {
            Scenario scenario = Scenarios.Data;

            switch (itemId)
            {
                case ScenarioEditorItemId.ScenarioName:
                    if (Config.ExistsKey(scenario.Name))
                    {
                        Config.SetText(scenario.Name, (string) val, Game.ScenarioTextFileName);
                    }
                    else
                    {
                        scenario.Name = (string) val;
                    }
                    break;

                case ScenarioEditorItemId.ScenarioPanelName:
                    scenario.PanelName = (string) val;
                    break;

                case ScenarioEditorItemId.ScenarioStartYear:
                    scenario.GlobalData.StartDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioStartMonth:
                    scenario.GlobalData.StartDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioStartDay:
                    scenario.GlobalData.StartDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioEndYear:
                    scenario.GlobalData.EndDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioEndMonth:
                    scenario.GlobalData.EndDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioEndDay:
                    scenario.GlobalData.EndDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioIncludeFolder:
                    scenario.IncludeFolder = (string) val;
                    break;

                case ScenarioEditorItemId.ScenarioBattleScenario:
                    scenario.Header.IsBattleScenario = (bool) val;
                    break;

                case ScenarioEditorItemId.ScenarioFreeSelection:
                    scenario.Header.IsFreeSelection = (bool) val;
                    break;

                case ScenarioEditorItemId.ScenarioAllowDiplomacy:
                    scenario.GlobalData.Rules.AllowDiplomacy = (bool) val;
                    break;

                case ScenarioEditorItemId.ScenarioAllowProduction:
                    scenario.GlobalData.Rules.AllowProduction = (bool) val;
                    break;

                case ScenarioEditorItemId.ScenarioAllowTechnology:
                    scenario.GlobalData.Rules.AllowTechnology = (bool) val;
                    break;

                case ScenarioEditorItemId.ScenarioAiAggressive:
                    scenario.Header.AiAggressive = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioDifficulty:
                    scenario.Header.Difficulty = (int) val;
                    break;

                case ScenarioEditorItemId.ScenarioGameSpeed:
                    scenario.Header.GameSpeed = (int) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <param name="val">編集項目の値</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, MajorCountrySettings major)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.MajorCountryNameKey:
                    major.Name = (string) val;
                    break;

                case ScenarioEditorItemId.MajorCountryNameString:
                    // 主要国設定の国名
                    if (!string.IsNullOrEmpty(major.Name))
                    {
                        Config.SetText(major.Name, (string) val, Game.WorldTextFileName);
                        break;
                    }
                    // 国家設定の国名
                    CountrySettings settings = Scenarios.GetCountrySettings(major.Country);
                    if (!string.IsNullOrEmpty(settings?.Name))
                    {
                        Config.SetText(settings.Name, (string) val, Game.WorldTextFileName);
                        break;
                    }
                    // 標準の国名
                    Config.SetText(Countries.Strings[(int) major.Country], (string) val, Game.WorldTextFileName);
                    break;

                case ScenarioEditorItemId.MajorFlagExt:
                    major.FlagExt = (string) val;
                    break;

                case ScenarioEditorItemId.MajorCountryDescKey:
                    major.Desc = (string) val;
                    break;

                case ScenarioEditorItemId.MajorCountryDescString:
                    // 主要国設定の説明文
                    if (!string.IsNullOrEmpty(major.Desc))
                    {
                        Config.SetText(major.Desc, (string) val, Game.ScenarioTextFileName);
                        break;
                    }
                    int year = Scenarios.Data.GlobalData.StartDate != null
                        ? Scenarios.Data.GlobalData.StartDate.Year
                        : Scenarios.Data.Header.StartDate != null
                            ? Scenarios.Data.Header.StartDate.Year
                            : Scenarios.Data.Header.StartYear;
                    // 年数の下2桁のみ使用する
                    year = year % 100;
                    // 年数別の説明文
                    string key = $"{major.Country}_{year}_DESC";
                    if (Config.ExistsKey(key))
                    {
                        Config.SetText(key, (string) val, Game.ScenarioTextFileName);
                        break;
                    }
                    // 標準の説明文
                    key = $"{major.Country}_DESC";
                    Config.SetText(key, (string) val, Game.ScenarioTextFileName);
                    break;

                case ScenarioEditorItemId.MajorPropaganada:
                    major.PictureName = (string) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Alliance alliance)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.AllianceName:
                    if (!string.IsNullOrEmpty(alliance.Name))
                    {
                        Config.SetText("ALLIANCE_" + alliance.Name, (string) val, Game.ScenarioTextFileName);
                    }
                    else
                    {
                        ScenarioGlobalData data = Scenarios.Data.GlobalData;
                        if (alliance == data.Axis)
                        {
                            Config.SetText(TextId.AllianceAxis, (string) val, Game.ScenarioTextFileName);
                        }
                        else if (alliance == data.Allies)
                        {
                            Config.SetText(TextId.AllianceAllies, (string) val, Game.ScenarioTextFileName);
                        }
                        else if (alliance == data.Comintern)
                        {
                            Config.SetText(TextId.AllianceComintern, (string) val, Game.ScenarioTextFileName);
                        }
                    }
                    break;

                case ScenarioEditorItemId.AllianceType:
                    alliance.Id.Type = (int) val;
                    break;

                case ScenarioEditorItemId.AllianceId:
                    alliance.Id.Id = (int) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, War war)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.WarStartYear:
                    war.StartDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.WarStartMonth:
                    war.StartDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.WarStartDay:
                    war.StartDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.WarEndYear:
                    war.EndDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.WarEndMonth:
                    war.EndDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.WarEndDay:
                    war.EndDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.WarType:
                    war.Id.Type = (int) val;
                    break;

                case ScenarioEditorItemId.WarId:
                    war.Id.Id = (int) val;
                    break;

                case ScenarioEditorItemId.WarAttackerType:
                    war.Attackers.Id.Type = (int) val;
                    break;

                case ScenarioEditorItemId.WarAttackerId:
                    war.Attackers.Id.Id = (int) val;
                    break;

                case ScenarioEditorItemId.WarDefenderType:
                    war.Defenders.Id.Type = (int) val;
                    break;

                case ScenarioEditorItemId.WarDefenderId:
                    war.Defenders.Id.Id = (int) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="relation">国家関係</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Relation relation)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyRelationValue:
                    relation.Value = (double) val;
                    break;

                case ScenarioEditorItemId.DiplomacyMilitaryAccess:
                    relation.Access = (bool) val;
                    break;

                case ScenarioEditorItemId.DiplomacyGuaranteed:
                    relation.Guaranteed = (bool) val ? new GameDate() : null;
                    break;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndYear:
                    relation.Guaranteed.Year = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndMonth:
                    relation.Guaranteed.Month = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyGuaranteedEndDay:
                    relation.Guaranteed.Day = (int) val;
                    break;
            }
        }

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
                case ScenarioEditorItemId.DiplomacyNonAggressionStartYear:
                case ScenarioEditorItemId.DiplomacyPeaceStartYear:
                case ScenarioEditorItemId.TradeStartYear:
                    treaty.StartDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartMonth:
                case ScenarioEditorItemId.DiplomacyPeaceStartMonth:
                case ScenarioEditorItemId.TradeStartMonth:
                    treaty.StartDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartDay:
                case ScenarioEditorItemId.DiplomacyPeaceStartDay:
                case ScenarioEditorItemId.TradeStartDay:
                    treaty.StartDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndYear:
                case ScenarioEditorItemId.DiplomacyPeaceEndYear:
                case ScenarioEditorItemId.TradeEndYear:
                    treaty.EndDate.Year = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndMonth:
                case ScenarioEditorItemId.DiplomacyPeaceEndMonth:
                case ScenarioEditorItemId.TradeEndMonth:
                    treaty.EndDate.Month = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndDay:
                case ScenarioEditorItemId.DiplomacyPeaceEndDay:
                case ScenarioEditorItemId.TradeEndDay:
                    treaty.EndDate.Day = (int) val;
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionType:
                case ScenarioEditorItemId.DiplomacyPeaceType:
                case ScenarioEditorItemId.TradeType:
                    Scenarios.SetType(treaty.Id, (int) val);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionId:
                case ScenarioEditorItemId.DiplomacyPeaceId:
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
        /// <param name="spy">諜報設定</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, SpySettings spy)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.IntelligenceSpies:
                    spy.Spies = (int) val;
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
                case ScenarioEditorItemId.DiplomacyMaster:
                    settings.Master = (Country) val;
                    break;

                case ScenarioEditorItemId.DiplomacyMilitaryControl:
                    settings.Control = (Country) val;
                    break;

                case ScenarioEditorItemId.CountryNameKey:
                    settings.Name = (string) val;
                    break;

                case ScenarioEditorItemId.CountryNameString:
                    Config.SetText(settings.Name, (string) val, Game.WorldTextFileName);
                    break;

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
                case ScenarioEditorItemId.ProvinceNameKey:
                    settings.Name = (string) val;
                    break;

                case ScenarioEditorItemId.ProvinceNameString:
                    Config.SetText(settings.Name, (string) val, Game.ProvinceTextFileName);
                    break;

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
        /// <param name="unit">ユニット</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitType:
                    Scenarios.SetType(unit.Id, (int) val);
                    break;

                case ScenarioEditorItemId.UnitId:
                    Scenarios.SetId(unit.Id, (int) val);
                    break;

                case ScenarioEditorItemId.UnitName:
                    unit.Name = (string) val;
                    break;

                case ScenarioEditorItemId.UnitLocationId:
                case ScenarioEditorItemId.UnitLocation:
                    unit.Location = (int) val;
                    break;

                case ScenarioEditorItemId.UnitBaseId:
                case ScenarioEditorItemId.UnitBase:
                    unit.Base = (int) val;
                    break;

                case ScenarioEditorItemId.UnitLeaderId:
                case ScenarioEditorItemId.UnitLeader:
                    unit.Leader = (int) val;
                    break;

                case ScenarioEditorItemId.UnitMorale:
                    unit.Morale = (double) val;
                    break;

                case ScenarioEditorItemId.UnitDigIn:
                    unit.DigIn = (double) val;
                    break;
            }
        }

        /// <summary>
        ///     編集項目の値を設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="division">師団</param>
        public void SetItemValue(ScenarioEditorItemId itemId, object val, Division division)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionType:
                    Scenarios.SetType(division.Id, (int) val);
                    break;

                case ScenarioEditorItemId.DivisionId:
                    Scenarios.SetId(division.Id, (int) val);
                    break;

                case ScenarioEditorItemId.DivisionName:
                    division.Name = (string) val;
                    break;

                case ScenarioEditorItemId.DivisionUnitType:
                    division.Type = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionModel:
                    division.Model = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType1:
                    division.Extra1 = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType2:
                    division.Extra2 = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType3:
                    division.Extra3 = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType4:
                    division.Extra4 = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType5:
                    division.Extra5 = (UnitType) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel1:
                    division.BrigadeModel1 = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel2:
                    division.BrigadeModel2 = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel3:
                    division.BrigadeModel3 = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel4:
                    division.BrigadeModel4 = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionBrigadeModel5:
                    division.BrigadeModel5 = (int) val;
                    break;

                case ScenarioEditorItemId.DivisionStrength:
                    division.Strength = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionMaxStrength:
                    division.MaxStrength = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionOrganisation:
                    division.Organisation = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionMaxOrganisation:
                    division.MaxOrganisation = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionMorale:
                    division.Morale = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionExperience:
                    division.Experience = (double) val;
                    break;

                case ScenarioEditorItemId.DivisionLocked:
                    division.Locked = (bool) val;
                    break;

                case ScenarioEditorItemId.DivisionDormant:
                    division.Dormant = (bool) val;
                    break;
            }
        }

        #endregion

        #region 編集項目 - 有効値判定

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
                case ScenarioEditorItemId.IntelligenceSpies:
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

                case ScenarioEditorItemId.DiplomacyRelationValue:
                    if (((double) val < -200) || ((double) val > 200))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.ScenarioStartYear:
                case ScenarioEditorItemId.ScenarioEndYear:
                case ScenarioEditorItemId.DiplomacyGuaranteedEndYear:
                case ScenarioEditorItemId.CountryNukeYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.ScenarioStartMonth:
                case ScenarioEditorItemId.ScenarioEndMonth:
                case ScenarioEditorItemId.DiplomacyGuaranteedEndMonth:
                case ScenarioEditorItemId.CountryNukeMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.ScenarioStartDay:
                case ScenarioEditorItemId.ScenarioEndDay:
                case ScenarioEditorItemId.DiplomacyGuaranteedEndDay:
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
        /// <param name="alliance">同盟</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, Alliance alliance)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.AllianceType:
                    if ((alliance.Id != null) && Scenarios.ExistsTypeId((int) val, alliance.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.AllianceId:
                    if ((alliance.Id != null) && Scenarios.ExistsTypeId(alliance.Id.Type, (int) val))
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
        /// <param name="war">戦争</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, War war)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.WarStartYear:
                case ScenarioEditorItemId.WarEndYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarStartMonth:
                case ScenarioEditorItemId.WarEndMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarStartDay:
                case ScenarioEditorItemId.WarEndDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;
                case ScenarioEditorItemId.WarType:
                    if ((war.Id != null) && Scenarios.ExistsTypeId((int) val, war.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarId:
                    if ((war.Id != null) && Scenarios.ExistsTypeId(war.Id.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarAttackerType:
                    if (war.Attackers?.Id != null && Scenarios.ExistsTypeId((int) val, war.Attackers.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarAttackerId:
                    if (war.Attackers?.Id != null && Scenarios.ExistsTypeId(war.Attackers.Id.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarDefenderType:
                    if (war.Defenders?.Id != null && Scenarios.ExistsTypeId((int) val, war.Defenders.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.WarDefenderId:
                    if (war.Defenders?.Id != null && Scenarios.ExistsTypeId(war.Defenders.Id.Type, (int) val))
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
                case ScenarioEditorItemId.DiplomacyNonAggressionStartYear:
                case ScenarioEditorItemId.DiplomacyNonAggressionEndYear:
                case ScenarioEditorItemId.DiplomacyPeaceStartYear:
                case ScenarioEditorItemId.DiplomacyPeaceEndYear:
                case ScenarioEditorItemId.TradeStartYear:
                case ScenarioEditorItemId.TradeEndYear:
                    if (((int) val < GameDate.MinYear) || ((int) val > GameDate.MaxYear))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartMonth:
                case ScenarioEditorItemId.DiplomacyNonAggressionEndMonth:
                case ScenarioEditorItemId.DiplomacyPeaceStartMonth:
                case ScenarioEditorItemId.DiplomacyPeaceEndMonth:
                case ScenarioEditorItemId.TradeStartMonth:
                case ScenarioEditorItemId.TradeEndMonth:
                    if (((int) val < GameDate.MinMonth) || ((int) val > GameDate.MaxMonth))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartDay:
                case ScenarioEditorItemId.DiplomacyNonAggressionEndDay:
                case ScenarioEditorItemId.DiplomacyPeaceStartDay:
                case ScenarioEditorItemId.DiplomacyPeaceEndDay:
                case ScenarioEditorItemId.TradeStartDay:
                case ScenarioEditorItemId.TradeEndDay:
                    if (((int) val < GameDate.MinDay) || ((int) val > GameDate.MaxDay))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionType:
                case ScenarioEditorItemId.DiplomacyPeaceType:
                case ScenarioEditorItemId.TradeType:
                    if ((treaty.Id != null) && Scenarios.ExistsTypeId((int) val, treaty.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionId:
                case ScenarioEditorItemId.DiplomacyPeaceId:
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

                case ScenarioEditorItemId.ScenarioEndDay:
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
                    if (settings?.HeadOfState != null && Scenarios.ExistsTypeId((int) val, settings.HeadOfState.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfStateId:
                    if (settings?.HeadOfState != null && Scenarios.ExistsTypeId(settings.HeadOfState.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentType:
                    if (settings?.HeadOfGovernment != null &&
                        Scenarios.ExistsTypeId((int) val, settings.HeadOfGovernment.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetHeadOfGovernmentId:
                    if (settings?.HeadOfGovernment != null &&
                        Scenarios.ExistsTypeId(settings.HeadOfGovernment.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterType:
                    if (settings?.ForeignMinister != null &&
                        Scenarios.ExistsTypeId((int) val, settings.ForeignMinister.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetForeignMinisterId:
                    if (settings?.ForeignMinister != null &&
                        Scenarios.ExistsTypeId(settings.ForeignMinister.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterType:
                    if (settings?.ArmamentMinister != null &&
                        Scenarios.ExistsTypeId((int) val, settings.ArmamentMinister.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetArmamentMinisterId:
                    if (settings?.ArmamentMinister != null &&
                        Scenarios.ExistsTypeId(settings.ArmamentMinister.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityType:
                    if (settings?.MinisterOfSecurity != null &&
                        Scenarios.ExistsTypeId((int) val, settings.MinisterOfSecurity.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfSecurityId:
                    if (settings?.MinisterOfSecurity != null &&
                        Scenarios.ExistsTypeId(settings.MinisterOfSecurity.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceType:
                    if (settings?.MinisterOfIntelligence != null &&
                        Scenarios.ExistsTypeId((int) val, settings.MinisterOfIntelligence.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetMinisterOfIntelligenceId:
                    if (settings?.MinisterOfIntelligence != null &&
                        Scenarios.ExistsTypeId(settings.MinisterOfIntelligence.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffType:
                    if (settings?.ChiefOfStaff != null && Scenarios.ExistsTypeId((int) val, settings.ChiefOfStaff.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfStaffId:
                    if (settings?.ChiefOfStaff != null && Scenarios.ExistsTypeId(settings.ChiefOfStaff.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyType:
                    if (settings?.ChiefOfArmy != null && Scenarios.ExistsTypeId((int) val, settings.ChiefOfArmy.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfArmyId:
                    if (settings?.ChiefOfArmy != null && Scenarios.ExistsTypeId(settings.ChiefOfArmy.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyType:
                    if (settings?.ChiefOfNavy != null && Scenarios.ExistsTypeId((int) val, settings.ChiefOfNavy.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfNavyId:
                    if (settings?.ChiefOfNavy != null && Scenarios.ExistsTypeId(settings.ChiefOfNavy.Type, (int) val))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirType:
                    if (settings?.ChiefOfAir != null && Scenarios.ExistsTypeId((int) val, settings.ChiefOfAir.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.CabinetChiefOfAirId:
                    if (settings?.ChiefOfAir != null && Scenarios.ExistsTypeId(settings.ChiefOfAir.Type, (int) val))
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
        /// <param name="unit">ユニット</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitType:
                    if ((unit.Id != null) && Scenarios.ExistsTypeId((int) val, unit.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.UnitId:
                    if ((unit.Id != null) && Scenarios.ExistsTypeId(unit.Id.Type, (int) val))
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
        /// <param name="division">師団</param>
        /// <returns>編集項目の値が有効でなければfalseを返す</returns>
        public bool IsItemValueValid(ScenarioEditorItemId itemId, object val, Division division)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionType:
                    if ((division.Id != null) && Scenarios.ExistsTypeId((int) val, division.Id.Id))
                    {
                        return false;
                    }
                    break;

                case ScenarioEditorItemId.DivisionId:
                    if ((division.Id != null) && Scenarios.ExistsTypeId(division.Id.Type, (int) val))
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
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId)
        {
            return Scenarios.Data.IsDirty((Scenario.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, MajorCountrySettings major)
        {
            return major.IsDirty((MajorCountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="alliance">同盟</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Alliance alliance)
        {
            return alliance.IsDirty((Alliance.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="war">戦争</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, War war)
        {
            return war.IsDirty((War.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="relation">国家関係</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Relation relation)
        {
            return (relation != null) && relation.IsDirty((Relation.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="treaty">協定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyNonAggression:
                case ScenarioEditorItemId.DiplomacyPeace:
                    return (treaty != null) && treaty.IsDirty();

                default:
                    return (treaty != null) && treaty.IsDirty((Treaty.ItemId) ItemDirtyFlags[(int) itemId]);
            }
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="spy">諜報設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, SpySettings spy)
        {
            return (spy != null) && spy.IsDirty((SpySettings.ItemId) ItemDirtyFlags[(int) itemId]);
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
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, ProvinceSettings settings)
        {
            return (settings != null) && settings.IsDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="unit">ユニット</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Unit unit)
        {
            return unit.IsDirty((Unit.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        /// <summary>
        ///     編集項目の編集済みフラグを取得する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="division">師団</param>
        /// <returns>編集済みフラグ</returns>
        public bool IsItemDirty(ScenarioEditorItemId itemId, Division division)
        {
            return division.IsDirty((Division.ItemId) ItemDirtyFlags[(int) itemId]);
        }

        #endregion

        #region 編集項目 - 編集済みフラグ設定

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        public void SetItemDirty(ScenarioEditorItemId itemId)
        {
            Scenarios.Data.SetDirty((Scenario.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="major">主要国設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, MajorCountrySettings major)
        {
            major.SetDirty((MajorCountrySettings.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="alliance">同盟</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Alliance alliance)
        {
            alliance.SetDirty((Alliance.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="war">戦争</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, War war)
        {
            war.SetDirty((War.ItemId) ItemDirtyFlags[(int) itemId]);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="relation">国家関係</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Relation relation, CountrySettings settings)
        {
            relation.SetDirty((Relation.ItemId) ItemDirtyFlags[(int) itemId]);
            settings.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="treaty">協定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Treaty treaty)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyNonAggression:
                case ScenarioEditorItemId.DiplomacyPeace:
                    if (treaty != null)
                    {
                        treaty.SetDirty(Treaty.ItemId.StartYear);
                        treaty.SetDirty(Treaty.ItemId.StartMonth);
                        treaty.SetDirty(Treaty.ItemId.StartDay);
                        treaty.SetDirty(Treaty.ItemId.EndYear);
                        treaty.SetDirty(Treaty.ItemId.EndMonth);
                        treaty.SetDirty(Treaty.ItemId.EndDay);
                        treaty.SetDirty(Treaty.ItemId.Type);
                        treaty.SetDirty(Treaty.ItemId.Id);
                        treaty.SetDirty();
                    }
                    break;

                default:
                    treaty.SetDirty((Treaty.ItemId) ItemDirtyFlags[(int) itemId]);
                    break;
            }
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="spy">諜報設定</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, SpySettings spy, CountrySettings settings)
        {
            spy.SetDirty((SpySettings.ItemId) ItemDirtyFlags[(int) itemId]);
            settings.SetDirty();
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
                    Scenarios.Data.SetDirtyProvinces();
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
            settings.SetDirty((ProvinceSettings.ItemId) ItemDirtyFlags[(int) itemId]);
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceNameKey:
                case ScenarioEditorItemId.ProvinceManpowerCurrent:
                case ScenarioEditorItemId.ProvinceManpowerMax:
                case ScenarioEditorItemId.ProvinceEnergyCurrent:
                case ScenarioEditorItemId.ProvinceEnergyMax:
                case ScenarioEditorItemId.ProvinceMetalCurrent:
                case ScenarioEditorItemId.ProvinceMetalMax:
                case ScenarioEditorItemId.ProvinceRareMaterialsCurrent:
                case ScenarioEditorItemId.ProvinceRareMaterialsMax:
                case ScenarioEditorItemId.ProvinceOilCurrent:
                case ScenarioEditorItemId.ProvinceOilMax:
                    if (Scenarios.Data.IsBaseProvinceSettings || Scenarios.Data.IsCountryProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyProvinces();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.ProvinceEnergyPool:
                case ScenarioEditorItemId.ProvinceMetalPool:
                case ScenarioEditorItemId.ProvinceRareMaterialsPool:
                case ScenarioEditorItemId.ProvinceOilPool:
                case ScenarioEditorItemId.ProvinceSupplyPool:
                    if (Scenarios.Data.IsDepotsProvinceSettings || Scenarios.Data.IsBaseProvinceSettings ||
                        Scenarios.Data.IsCountryProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyProvinces();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.ProvinceIcCurrent:
                case ScenarioEditorItemId.ProvinceIcMax:
                case ScenarioEditorItemId.ProvinceIcRelative:
                case ScenarioEditorItemId.ProvinceInfrastructureCurrent:
                case ScenarioEditorItemId.ProvinceInfrastructureMax:
                case ScenarioEditorItemId.ProvinceInfrastructureRelative:
                case ScenarioEditorItemId.ProvinceLandFortCurrent:
                case ScenarioEditorItemId.ProvinceLandFortMax:
                case ScenarioEditorItemId.ProvinceLandFortRelative:
                case ScenarioEditorItemId.ProvinceCoastalFortCurrent:
                case ScenarioEditorItemId.ProvinceCoastalFortMax:
                case ScenarioEditorItemId.ProvinceCoastalFortRelative:
                case ScenarioEditorItemId.ProvinceAntiAirCurrent:
                case ScenarioEditorItemId.ProvinceAntiAirMax:
                case ScenarioEditorItemId.ProvinceAntiAirRelative:
                case ScenarioEditorItemId.ProvinceAirBaseCurrent:
                case ScenarioEditorItemId.ProvinceAirBaseMax:
                case ScenarioEditorItemId.ProvinceAirBaseRelative:
                case ScenarioEditorItemId.ProvinceNavalBaseCurrent:
                case ScenarioEditorItemId.ProvinceNavalBaseMax:
                case ScenarioEditorItemId.ProvinceNavalBaseRelative:
                case ScenarioEditorItemId.ProvinceRadarStationCurrent:
                case ScenarioEditorItemId.ProvinceRadarStationMax:
                case ScenarioEditorItemId.ProvinceRadarStationRelative:
                case ScenarioEditorItemId.ProvinceNuclearReactorCurrent:
                case ScenarioEditorItemId.ProvinceNuclearReactorMax:
                case ScenarioEditorItemId.ProvinceNuclearReactorRelative:
                case ScenarioEditorItemId.ProvinceRocketTestCurrent:
                case ScenarioEditorItemId.ProvinceRocketTestMax:
                case ScenarioEditorItemId.ProvinceRocketTestRelative:
                case ScenarioEditorItemId.ProvinceSyntheticOilCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticOilMax:
                case ScenarioEditorItemId.ProvinceSyntheticOilRelative:
                case ScenarioEditorItemId.ProvinceSyntheticRaresCurrent:
                case ScenarioEditorItemId.ProvinceSyntheticRaresMax:
                case ScenarioEditorItemId.ProvinceSyntheticRaresRelative:
                case ScenarioEditorItemId.ProvinceNuclearPowerCurrent:
                case ScenarioEditorItemId.ProvinceNuclearPowerMax:
                case ScenarioEditorItemId.ProvinceNuclearPowerRelative:
                case ScenarioEditorItemId.ProvinceRevoltRisk:
                    if (Scenarios.Data.IsBaseDodProvinceSettings || Scenarios.Data.IsBaseProvinceSettings ||
                        Scenarios.Data.IsCountryProvinceSettings)
                    {
                        Scenarios.Data.SetDirtyProvinces();
                    }
                    else
                    {
                        Scenarios.Data.SetDirty();
                    }
                    Scenarios.SetDirty();
                    break;

                case ScenarioEditorItemId.ProvinceVp:
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
            }
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="unit">ユニット</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Unit unit, CountrySettings settings)
        {
            unit.SetDirty((Unit.ItemId) ItemDirtyFlags[(int) itemId]);
            settings.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     編集項目の編集済みフラグを設定する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        public void SetItemDirty(ScenarioEditorItemId itemId, Division division, CountrySettings settings)
        {
            division.SetDirty((Division.ItemId) ItemDirtyFlags[(int) itemId]);
            settings.SetDirty();
            Scenarios.SetDirty();
        }

        #endregion

        #region 編集項目 - 変更前処理

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ScenarioStartYear:
                    PreItemChangedScenarioStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartDay));
                    break;

                case ScenarioEditorItemId.ScenarioStartMonth:
                    PreItemChangedScenarioStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartDay));
                    break;

                case ScenarioEditorItemId.ScenarioStartDay:
                    PreItemChangedScenarioStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioStartMonth));
                    break;

                case ScenarioEditorItemId.ScenarioEndYear:
                    PreItemChangedScenarioEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndDay));
                    break;

                case ScenarioEditorItemId.ScenarioEndMonth:
                    PreItemChangedScenarioEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndDay));
                    break;

                case ScenarioEditorItemId.ScenarioEndDay:
                    PreItemChangedScenarioEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.ScenarioEndMonth));
                    break;

                case ScenarioEditorItemId.ScenarioAllowDiplomacy:
                case ScenarioEditorItemId.ScenarioAllowProduction:
                case ScenarioEditorItemId.ScenarioAllowTechnology:
                    if (Scenarios.Data.GlobalData.Rules == null)
                    {
                        Scenarios.Data.GlobalData.Rules = new ScenarioRules();
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, MajorCountrySettings major)
        {
            // 何もしない
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Alliance alliance)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.AllianceType:
                    if (alliance.Id == null)
                    {
                        alliance.Id = new TypeId();
                        PreItemChangedAllianceType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.AllianceId), val, alliance);
                    }
                    break;

                case ScenarioEditorItemId.AllianceId:
                    if (alliance.Id == null)
                    {
                        alliance.Id = new TypeId();
                        PreItemChangedAllianceId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.AllianceType), val, alliance);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">同盟</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, War war)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.WarStartYear:
                    PreItemChangedWarStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartDay), war);
                    break;

                case ScenarioEditorItemId.WarStartMonth:
                    PreItemChangedWarStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartDay), war);
                    break;

                case ScenarioEditorItemId.WarStartDay:
                    PreItemChangedWarStartDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarStartMonth), war);
                    break;

                case ScenarioEditorItemId.WarEndYear:
                    PreItemChangedWarEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndDay), war);
                    break;

                case ScenarioEditorItemId.WarEndMonth:
                    PreItemChangedWarEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndDay), war);
                    break;

                case ScenarioEditorItemId.WarEndDay:
                    PreItemChangedWarEndDate((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarEndMonth), war);
                    break;

                case ScenarioEditorItemId.WarType:
                    if (war.Id == null)
                    {
                        war.Id = new TypeId();
                        PreItemChangedWarType((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarId), val, war);
                    }
                    break;

                case ScenarioEditorItemId.WarId:
                    if (war.Id == null)
                    {
                        war.Id = new TypeId();
                        PreItemChangedWarId((TextBox) _form.GetItemControl(ScenarioEditorItemId.WarType), val, war);
                    }
                    break;

                case ScenarioEditorItemId.WarAttackerType:
                    if (war.Attackers.Id == null)
                    {
                        war.Attackers.Id = new TypeId();
                        PreItemChangedWarAttackerType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarAttackerId), val, war);
                    }
                    break;

                case ScenarioEditorItemId.WarAttackerId:
                    if (war.Attackers.Id == null)
                    {
                        war.Attackers.Id = new TypeId();
                        PreItemChangedWarAttackerId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarAttackerType), val, war);
                    }
                    break;

                case ScenarioEditorItemId.WarDefenderType:
                    if (war.Defenders.Id == null)
                    {
                        war.Defenders.Id = new TypeId();
                        PreItemChangedWarDefenderType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarDefenderId), val, war);
                    }
                    break;

                case ScenarioEditorItemId.WarDefenderId:
                    if (war.Defenders.Id == null)
                    {
                        war.Defenders.Id = new TypeId();
                        PreItemChangedWarDefenderId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.WarDefenderType), val, war);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="relation">国家関係</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Relation relation)
        {
            // 何もしない
        }

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
                case ScenarioEditorItemId.DiplomacyNonAggressionStartYear:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartMonth:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionStartDay:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartMonth), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndYear:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndMonth:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionEndDay:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndMonth), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionType:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyType(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionId), val, treaty);
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyNonAggressionId:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyId(
                            (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionType), val, treaty);
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceStartYear:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceStartMonth:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceStartDay:
                    PreItemChangedTreatyStartDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartMonth), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceEndYear:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceEndMonth:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndDay), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceEndDay:
                    PreItemChangedTreatyEndDate(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndMonth), treaty);
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceType:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyType((TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceId),
                            val, treaty);
                    }
                    break;

                case ScenarioEditorItemId.DiplomacyPeaceId:
                    if (treaty.Id == null)
                    {
                        treaty.Id = new TypeId();
                        PreItemChangedTreatyId((TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceType),
                            val, treaty);
                    }
                    break;

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
        /// <param name="spy">諜報設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, SpySettings spy)
        {
            // 何もしない
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
                case ScenarioEditorItemId.DiplomacyMaster:
                    PreItemChangedRelation((Country) val, (Country) GetItemValue(itemId, settings), 2);
                    break;

                case ScenarioEditorItemId.DiplomacyMilitaryControl:
                    PreItemChangedRelation((Country) val, (Country) GetItemValue(itemId, settings), 3);
                    break;

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
                    PreItemChangedCapital(_form.GetItemControl(itemId), val, province, settings);
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    PreItemChangedOwnedProvinces(_form.GetItemControl(itemId), val, province);
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    PreItemChangedControlledProvinces(_form.GetItemControl(itemId), val, province);
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
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="unit">ユニット</param>
        /// <param name="settings">国家設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Unit unit, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitType:
                    if (unit.Id == null)
                    {
                        unit.Id = new TypeId();
                        PreItemChangedUnitType((TextBox) _form.GetItemControl(ScenarioEditorItemId.UnitId), val, unit,
                            settings);
                    }
                    break;

                case ScenarioEditorItemId.UnitId:
                    if (unit.Id == null)
                    {
                        unit.Id = new TypeId();
                        PreItemChangedUnitId((TextBox) _form.GetItemControl(ScenarioEditorItemId.UnitType), val, unit,
                            settings);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目値変更前の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        public void PreItemChanged(ScenarioEditorItemId itemId, object val, Division division, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionType:
                    if (division.Id == null)
                    {
                        division.Id = new TypeId();
                        PreItemChangedDivisionType((TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionId), val,
                            division,
                            settings);
                    }
                    break;

                case ScenarioEditorItemId.DivisionId:
                    if (division.Id == null)
                    {
                        division.Id = new TypeId();
                        PreItemChangedDivisionId((TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionType), val,
                            division,
                            settings);
                    }
                    break;
            }
        }

        /// <summary>
        ///     項目変更前の処理 - シナリオ開始日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        private void PreItemChangedScenarioStartDate(TextBox control1, TextBox control2)
        {
            if (Scenarios.Data.GlobalData.StartDate == null)
            {
                Scenarios.Data.GlobalData.StartDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2);

                // 編集項目の値を更新する
                UpdateItemValue(control1);
                UpdateItemValue(control2);

                // 編集項目の色を更新する
                UpdateItemColor(control1);
                UpdateItemColor(control2);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - シナリオ終了日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        private void PreItemChangedScenarioEndDate(TextBox control1, TextBox control2)
        {
            if (Scenarios.Data.GlobalData.EndDate == null)
            {
                Scenarios.Data.GlobalData.EndDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2);

                // 編集項目の値を更新する
                UpdateItemValue(control1);
                UpdateItemValue(control2);

                // 編集項目の色を更新する
                UpdateItemColor(control1);
                UpdateItemColor(control2);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 同盟type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        private void PreItemChangedAllianceType(TextBox control, object val, Alliance alliance)
        {
            // 新規idを設定する
            alliance.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, alliance);

            // 編集項目の値を更新する
            UpdateItemValue(control, alliance);

            // 編集項目の色を更新する
            UpdateItemColor(control, alliance);
        }

        /// <summary>
        ///     項目変更前の処理 - 同盟id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        private void PreItemChangedAllianceId(TextBox control, object val, Alliance alliance)
        {
            alliance.Id.Type = Scenarios.GetNewType(Scenarios.DefaultAllianceType, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, alliance);

            // 編集項目の値を更新する
            UpdateItemValue(control, alliance);

            // 編集項目の色を更新する
            UpdateItemColor(control, alliance);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争開始日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarStartDate(TextBox control1, TextBox control2, War war)
        {
            if (war.StartDate == null)
            {
                war.StartDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, war);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, war);

                // 編集項目の値を更新する
                UpdateItemValue(control1, war);
                UpdateItemValue(control2, war);

                // 編集項目の色を更新する
                UpdateItemColor(control1, war);
                UpdateItemColor(control2, war);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争終了日時
        /// </summary>
        /// <param name="control1">連動するコントロール1</param>
        /// <param name="control2">連動するコントロール2</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarEndDate(TextBox control1, TextBox control2, War war)
        {
            if (war.EndDate == null)
            {
                war.EndDate = new GameDate();

                // 編集済みフラグを設定する
                ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
                SetItemDirty(itemId1, war);
                ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
                SetItemDirty(itemId2, war);

                // 編集項目の値を更新する
                UpdateItemValue(control1, war);
                UpdateItemValue(control2, war);

                // 編集項目の色を更新する
                UpdateItemColor(control1, war);
                UpdateItemColor(control2, war);
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarType(TextBox control, object val, War war)
        {
            // 新規idを設定する
            war.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarId(TextBox control, object val, War war)
        {
            war.Id.Type = Scenarios.GetNewType(Scenarios.DefaultWarType, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争攻撃側type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarAttackerType(TextBox control, object val, War war)
        {
            // 新規idを設定する
            war.Attackers.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争攻撃側id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarAttackerId(TextBox control, object val, War war)
        {
            war.Attackers.Id.Type = Scenarios.GetNewType(Scenarios.DefaultWarType, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争防御側type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarDefenderType(TextBox control, object val, War war)
        {
            // 新規idを設定する
            war.Defenders.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 戦争防御側id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        private void PreItemChangedWarDefenderId(TextBox control, object val, War war)
        {
            war.Defenders.Id.Type = Scenarios.GetNewType(Scenarios.DefaultWarType, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, war);

            // 編集項目の値を更新する
            UpdateItemValue(control, war);

            // 編集項目の色を更新する
            UpdateItemColor(control, war);
        }

        /// <summary>
        ///     項目変更前の処理 - 宗主国/統帥権取得国
        /// </summary>
        /// <param name="val">編集項目の値</param>
        /// <param name="prev">変更前の値</param>
        /// <param name="no">関係リストビューの項目番号</param>
        private void PreItemChangedRelation(Country val, Country prev, int no)
        {
            // 関係リストビューの表示を更新する
            if (prev != Country.None)
            {
                int index = Array.IndexOf(Countries.Tags, prev);
                if (index >= 0)
                {
                    _form.SetRelationListItemText(index, no, "");
                }
            }
            _form.SetRelationListItemText(no, val != Country.None ? Resources.Yes : "");
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
            typeId.Type = Scenarios.GetNewType(settings.HeadOfState?.Type ?? 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, settings);

            // 編集項目の色を更新する
            UpdateItemColor(control, settings);
        }

        /// <summary>
        ///     項目変更前の処理 - 首都
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedCapital(Control control, object val, Province province, CountrySettings settings)
        {
            // チェックを入れた後はチェックボックスを無効化する
            if ((bool) val)
            {
                control.Enabled = false;
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
        }

        /// <summary>
        ///     項目変更前の処理 - 保有プロヴィンス
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        private static void PreItemChangedOwnedProvinces(Control control, object val, Province province)
        {
            // チェックを入れた後はチェックボックスを無効化する
            if ((bool) val)
            {
                control.Enabled = false;
            }

            // 元の保有国を解除する
            foreach (Country country in Countries.Tags)
            {
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                if (settings == null)
                {
                    continue;
                }
                if (settings.OwnedProvinces.Contains(province.Id))
                {
                    settings.OwnedProvinces.Remove(province.Id);
                    settings.SetDirtyOwnedProvinces(province.Id);
                }
            }
        }

        /// <summary>
        ///     項目変更前の処理 - 支配プロヴィンス
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        private static void PreItemChangedControlledProvinces(Control control, object val, Province province)
        {
            // チェックを入れた後はチェックボックスを無効化する
            if ((bool) val)
            {
                control.Enabled = false;
            }

            // 元の支配国を解除する
            foreach (Country country in Countries.Tags)
            {
                CountrySettings settings = Scenarios.GetCountrySettings(country);
                if (settings == null)
                {
                    continue;
                }
                if (settings.ControlledProvinces.Contains(province.Id))
                {
                    settings.ControlledProvinces.Remove(province.Id);
                    settings.SetDirtyControlledProvinces(province.Id);
                }
            }
        }

        /// <summary>
        ///     項目変更前の処理 - ユニットtype
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="unit">ユニット</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedUnitType(TextBox control, object val, Unit unit, CountrySettings settings)
        {
            // 新規idを設定する
            unit.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, unit, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, unit);

            // 編集項目の色を更新する
            UpdateItemColor(control, unit);
        }

        /// <summary>
        ///     項目変更前の処理 - ユニットid
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="unit">ユニット</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedUnitId(TextBox control, object val, Unit unit, CountrySettings settings)
        {
            unit.Id.Type = Scenarios.GetNewType(1, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, unit, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, unit);

            // 編集項目の色を更新する
            UpdateItemColor(control, unit);
        }

        /// <summary>
        ///     項目変更前の処理 - 師団type
        /// </summary>
        /// <param name="control">idのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedDivisionType(TextBox control, object val, Division division, CountrySettings settings)
        {
            // 新規idを設定する
            division.Id.Id = Scenarios.GetNewId((int) val, 1);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, division, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, division);

            // 編集項目の色を更新する
            UpdateItemColor(control, division);
        }

        /// <summary>
        ///     項目変更前の処理 - 師団id
        /// </summary>
        /// <param name="control">typeのコントロール</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PreItemChangedDivisionId(TextBox control, object val, Division division, CountrySettings settings)
        {
            division.Id.Type = Scenarios.GetNewType(1, (int) val);

            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            SetItemDirty(itemId, division, settings);

            // 編集項目の値を更新する
            UpdateItemValue(control, division);

            // 編集項目の色を更新する
            UpdateItemColor(control, division);
        }

        #endregion

        #region 編集項目 - 変更後処理

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ScenarioPanelName:
                    _form.UpdatePanelImage((string) val);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, MajorCountrySettings major)
        {
            TextBox control;
            switch (itemId)
            {
                case ScenarioEditorItemId.MajorCountryNameKey:
                    control = (TextBox) _form.GetItemControl(ScenarioEditorItemId.MajorCountryNameString);
                    UpdateItemValue(control, major);
                    UpdateItemColor(control, major);
                    break;

                case ScenarioEditorItemId.MajorCountryDescKey:
                    control = (TextBox) _form.GetItemControl(ScenarioEditorItemId.MajorCountryDescString);
                    UpdateItemValue(control, major);
                    UpdateItemColor(control, major);
                    break;

                case ScenarioEditorItemId.MajorPropaganada:
                    _form.UpdatePropagandaImage(major.Country, (string) val);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Alliance alliance)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.AllianceName:
                    _form.SetAllianceListItemText(0, (string) val);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, War war)
        {
            // 何もしない
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="relation">国家関係</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Relation relation)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyRelationValue:
                    _form.SetRelationListItemText(1, ObjectHelper.ToString(val));
                    break;

                case ScenarioEditorItemId.DiplomacyMilitaryAccess:
                    _form.SetRelationListItemText(4, (bool) val ? Resources.Yes : "");
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="relation">国家関係</param>
        /// <param name="settings">国家設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Relation relation, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DiplomacyGuaranteed:
                    PostItemChangedGuaranteed(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyGuaranteedEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyGuaranteedEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyGuaranteedEndDay), val, relation,
                        settings);
                    break;
            }
        }

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
                case ScenarioEditorItemId.DiplomacyNonAggression:
                    PostItemChangedTreaty(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionStartDay),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionEndDay),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionType),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyNonAggressionId), val, treaty, 6);
                    break;

                case ScenarioEditorItemId.DiplomacyPeace:
                    PostItemChangedTreaty((TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceStartDay),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndYear),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndMonth),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceEndDay),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceType),
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DiplomacyPeaceId), val, treaty, 7);
                    break;

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
        /// <param name="spy">諜報設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, SpySettings spy)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.IntelligenceSpies:
                    _form.SetRelationListItemText(8, ObjectHelper.ToString(val));
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
                case ScenarioEditorItemId.CountryNameKey:
                    TextBox control = (TextBox) _form.GetItemControl(ScenarioEditorItemId.CountryNameString);
                    UpdateItemValue(control, settings.Country, settings);
                    UpdateItemColor(control, settings);
                    break;

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
        /// <param name="settings">国家設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Province province, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.CountryCoreProvinces:
                    PostItemChangedCoreProvinces(val, province);
                    break;

                case ScenarioEditorItemId.CountryOwnedProvinces:
                    PostItemChangedOwnedProvinces(val, province, settings);
                    break;

                case ScenarioEditorItemId.CountryControlledProvinces:
                    PostItemChangedControlledProvinces(val, province, settings);
                    break;

                case ScenarioEditorItemId.CountryClaimedProvinces:
                    PostItemChangedClaimedProvinces(val, province);
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
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, object val, Province province,
            ProvinceSettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.ProvinceNameKey:
                    TextBox control = (TextBox) _form.GetItemControl(ScenarioEditorItemId.ProvinceNameString);
                    UpdateItemValue(control, province, settings);
                    UpdateItemColor(control, settings);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="unit">ユニット</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, Unit unit)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.UnitName:
                    _unitTreeController.UpdateUnitNodeLabel(unit.Name);
                    break;

                case ScenarioEditorItemId.UnitLocationId:
                    PostItemChangedProvinceId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.UnitLocation), unit);
                    break;

                case ScenarioEditorItemId.UnitLocation:
                    PostItemChangedProvince((TextBox) _form.GetItemControl(ScenarioEditorItemId.UnitLocationId), unit);
                    break;

                case ScenarioEditorItemId.UnitBaseId:
                    PostItemChangedProvinceId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.UnitBase), unit);
                    break;

                case ScenarioEditorItemId.UnitBase:
                    PostItemChangedProvince((TextBox) _form.GetItemControl(ScenarioEditorItemId.UnitBaseId), unit);
                    break;

                case ScenarioEditorItemId.UnitLeaderId:
                    PostItemChangedLeaderId((ComboBox) _form.GetItemControl(ScenarioEditorItemId.UnitLeader), unit);
                    break;

                case ScenarioEditorItemId.UnitLeader:
                    PostItemChangedLeader((TextBox) _form.GetItemControl(ScenarioEditorItemId.UnitLeaderId), unit);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        public void PostItemChanged(ScenarioEditorItemId itemId, Division division, CountrySettings settings)
        {
            switch (itemId)
            {
                case ScenarioEditorItemId.DivisionName:
                    _unitTreeController.UpdateDivisionNodeLabel(division.Name);
                    break;

                case ScenarioEditorItemId.DivisionUnitType:
                    PostItemChangedDivisionType(division, settings);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType1:
                    PostItemChangedBrigadeType(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel1), division, settings);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType2:
                    PostItemChangedBrigadeType(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel2), division, settings);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType3:
                    PostItemChangedBrigadeType(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel3), division, settings);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType4:
                    PostItemChangedBrigadeType(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel4), division, settings);
                    break;

                case ScenarioEditorItemId.DivisionBrigadeType5:
                    PostItemChangedBrigadeType(
                        (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel5), division, settings);
                    break;

                case ScenarioEditorItemId.DivisionStrength:
                    PostItemChangedStrength((TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionMaxStrength),
                        division, settings);
                    break;

                case ScenarioEditorItemId.DivisionMaxStrength:
                    PostItemChangedMaxStrength((TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionStrength),
                        division, settings);
                    break;

                case ScenarioEditorItemId.DivisionOrganisation:
                    PostItemChangedOrganisation(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionMaxOrganisation),
                        division, settings);
                    break;

                case ScenarioEditorItemId.DivisionMaxOrganisation:
                    PostItemChangedMaxOrganisation(
                        (TextBox) _form.GetItemControl(ScenarioEditorItemId.DivisionOrganisation),
                        division, settings);
                    break;
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 独立保障
        /// </summary>
        /// <param name="control1">コントロール1</param>
        /// <param name="control2">コントロール2</param>
        /// <param name="control3">コントロール3</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="relation">国家関係</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedGuaranteed(TextBox control1, TextBox control2, TextBox control3, object val,
            Relation relation, CountrySettings settings)
        {
            // 編集済みフラグを設定する
            ScenarioEditorItemId itemId1 = (ScenarioEditorItemId) control1.Tag;
            SetItemDirty(itemId1, relation, settings);
            ScenarioEditorItemId itemId2 = (ScenarioEditorItemId) control2.Tag;
            SetItemDirty(itemId2, relation, settings);
            ScenarioEditorItemId itemId3 = (ScenarioEditorItemId) control3.Tag;
            SetItemDirty(itemId3, relation, settings);

            // 項目の値を更新する
            UpdateItemValue(control1, relation);
            UpdateItemValue(control2, relation);
            UpdateItemValue(control3, relation);

            // 項目の色を更新する
            UpdateItemColor(control1, relation);
            UpdateItemColor(control2, relation);
            UpdateItemColor(control3, relation);

            // 項目を有効化/無効化する
            control1.Enabled = (bool) val;
            control2.Enabled = (bool) val;
            control3.Enabled = (bool) val;

            // 関係リストビューの項目を更新する
            _form.SetRelationListItemText(5, (bool) val ? Resources.Yes : "");
        }

        /// <summary>
        ///     項目値変更後の処理 - 不可侵条約/講和条約
        /// </summary>
        /// <param name="control1">コントロール1</param>
        /// <param name="control2">コントロール2</param>
        /// <param name="control3">コントロール3</param>
        /// <param name="control4">コントロール4</param>
        /// <param name="control5">コントロール5</param>
        /// <param name="control6">コントロール6</param>
        /// <param name="control7">コントロール7</param>
        /// <param name="control8">コントロール8</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        /// <param name="no">関係リストビューの項目番号</param>
        private void PostItemChangedTreaty(TextBox control1, TextBox control2, TextBox control3, TextBox control4,
            TextBox control5, TextBox control6, TextBox control7, TextBox control8, object val, Treaty treaty, int no)
        {
            // 項目の値を更新する
            UpdateItemValue(control1, treaty);
            UpdateItemValue(control2, treaty);
            UpdateItemValue(control3, treaty);
            UpdateItemValue(control4, treaty);
            UpdateItemValue(control5, treaty);
            UpdateItemValue(control6, treaty);
            UpdateItemValue(control7, treaty);
            UpdateItemValue(control8, treaty);

            // 項目の色を更新する
            UpdateItemColor(control1, treaty);
            UpdateItemColor(control2, treaty);
            UpdateItemColor(control3, treaty);
            UpdateItemColor(control4, treaty);
            UpdateItemColor(control5, treaty);
            UpdateItemColor(control6, treaty);
            UpdateItemColor(control7, treaty);
            UpdateItemColor(control8, treaty);

            // 項目を有効化/無効化する
            control1.Enabled = (bool) val;
            control2.Enabled = (bool) val;
            control3.Enabled = (bool) val;
            control4.Enabled = (bool) val;
            control5.Enabled = (bool) val;
            control6.Enabled = (bool) val;
            control7.Enabled = (bool) val;
            control8.Enabled = (bool) val;

            // 関係リストビューの項目を更新する
            _form.SetRelationListItemText(no, (bool) val ? Resources.Yes : "");
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
        ///     項目値変更後の処理 - 閣僚id
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
        ///     項目値変更後の処理 - 中核プロヴィンス
        /// </summary>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        private void PostItemChangedCoreProvinces(object val, Province province)
        {
            // プロヴィンスリストビューの表示を更新する
            int index = GetLandProvinceIndex(province.Id);
            if (index >= 0)
            {
                _form.SetProvinceListItemText(index, 3, (bool) val ? Resources.Yes : "");
            }

            // プロヴィンスの強調表示を更新する
            if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Core)
            {
                _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 保有プロヴィンス
        /// </summary>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedOwnedProvinces(object val, Province province, CountrySettings settings)
        {
            // 変更前の保有国を解除
            foreach (Country country in Countries.Tags)
            {
                CountrySettings cs = Scenarios.GetCountrySettings(country);
                if (cs == null || cs.Country == settings.Country ||
                    !cs.OwnedProvinces.Contains(province.Id))
                {
                    continue;
                }
                cs.OwnedProvinces.Remove(province.Id);
                cs.SetDirtyOwnedProvinces(province.Id);
            }

            // プロヴィンスリストビューの表示を更新する
            int index = GetLandProvinceIndex(province.Id);
            if (index >= 0)
            {
                _form.SetProvinceListItemText(index, 4, (bool) val ? Resources.Yes : "");
            }

            // プロヴィンスの強調表示を更新する
            if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Owned)
            {
                _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 支配プロヴィンス
        /// </summary>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedControlledProvinces(object val, Province province, CountrySettings settings)
        {
            // 変更前の支配国を解除
            foreach (Country country in Countries.Tags)
            {
                CountrySettings cs = Scenarios.GetCountrySettings(country);
                if (cs == null || cs.Country == settings.Country ||
                    !cs.ControlledProvinces.Contains(province.Id))
                {
                    continue;
                }
                cs.ControlledProvinces.Remove(province.Id);
                cs.SetDirtyControlledProvinces(province.Id);
            }

            // プロヴィンスリストビューの表示を更新する
            int index = GetLandProvinceIndex(province.Id);
            if (index >= 0)
            {
                _form.SetProvinceListItemText(index, 5, (bool) val ? Resources.Yes : "");
            }

            // プロヴィンスの強調表示を更新する
            if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Controlled)
            {
                _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
            }
        }

        /// <summary>
        ///     項目値変更後の処理 - 領有権主張プロヴィンス
        /// </summary>
        /// <param name="val">編集項目の値</param>
        /// <param name="province">プロヴィンス</param>
        private void PostItemChangedClaimedProvinces(object val, Province province)
        {
            // プロヴィンスリストビューの表示を更新する
            int index = GetLandProvinceIndex(province.Id);
            if (index >= 0)
            {
                _form.SetProvinceListItemText(index, 6, (bool) val ? Resources.Yes : "");
            }

            // プロヴィンスの強調表示を更新する
            if (_mapPanelController.FilterMode == MapPanelController.MapFilterMode.Claimed)
            {
                _mapPanelController.UpdateProvince((ushort) province.Id, (bool) val);
            }
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

        /// <summary>
        ///     項目値変更後の処理 - プロヴィンス
        /// </summary>
        /// <param name="control">プロヴィンスIDのコントロール</param>
        /// <param name="unit">ユニット</param>
        private void PostItemChangedProvince(TextBox control, Unit unit)
        {
            // 項目の値を更新する
            UpdateItemValue(control, unit);

            // 項目の色を更新する
            UpdateItemColor(control, unit);
        }

        /// <summary>
        ///     項目値変更後の処理 - プロヴィンスID
        /// </summary>
        /// <param name="control">プロヴィンスコンボボックス</param>
        /// <param name="unit">ユニット</param>
        private void PostItemChangedProvinceId(ComboBox control, Unit unit)
        {
            // 項目の値を更新する
            UpdateItemValue(control, unit);

            // 項目の色を更新する
            control.Refresh();
        }

        /// <summary>
        ///     項目値変更後の処理 - 指揮官
        /// </summary>
        /// <param name="control">指揮官IDのコントロール</param>
        /// <param name="unit">ユニット</param>
        private void PostItemChangedLeader(TextBox control, Unit unit)
        {
            // 項目の値を更新する
            UpdateItemValue(control, unit);

            // 項目の色を更新する
            UpdateItemColor(control, unit);
        }

        /// <summary>
        ///     項目値変更後の処理 - 指揮官ID
        /// </summary>
        /// <param name="control">指揮官コンボボックス</param>
        /// <param name="unit">ユニット</param>
        private void PostItemChangedLeaderId(ComboBox control, Unit unit)
        {
            // 項目の値を更新する
            UpdateItemValue(control, unit);

            // 項目の色を更新する
            control.Refresh();
        }

        /// <summary>
        ///     項目値変更後の処理 - 師団のユニット種類
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedDivisionType(Division division, CountrySettings settings)
        {
            // ユニットモデル項目の候補を更新する
            ComboBox control = (ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionModel);
            UpdateListItems(control, division, settings);

            // ユニットモデル項目の値を更新する
            UpdateItemValue(control, division);

            // 付属旅団の項目の値を更新する
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeType1), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel1), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeType2), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel2), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeType3), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel3), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeType4), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel4), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeType5), division);
            UpdateItemValue((ComboBox) _form.GetItemControl(ScenarioEditorItemId.DivisionBrigadeModel5), division);
        }

        /// <summary>
        ///     項目値変更後の処理 - 旅団のユニット種類
        /// </summary>
        /// <param name="control">ユニットモデルコンボボックス</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedBrigadeType(ComboBox control, Division division, CountrySettings settings)
        {
            // 項目の候補を更新する
            UpdateListItems(control, division, settings);

            // 項目の値を更新する
            UpdateItemValue(control, division);
        }

        /// <summary>
        ///     項目値変更後の処理 - 師団の戦力
        /// </summary>
        /// <param name="control">最大値のコントロール</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedStrength(TextBox control, Division division, CountrySettings settings)
        {
            if (DoubleHelper.IsZero(division.MaxStrength) ||
                DoubleHelper.IsLessOrEqual(division.Strength, division.MaxStrength))
            {
                return;
            }

            // 最大値を現在値に合わせる
            division.MaxStrength = division.Strength;

            // 編集済みフラグを設定する
            division.SetDirty(Division.ItemId.MaxStrength);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 項目の値を更新する
            UpdateItemValue(control, division);

            // 項目の色を更新する
            UpdateItemColor(control, division);
        }

        /// <summary>
        ///     項目値変更後の処理 - 師団の最大戦力
        /// </summary>
        /// <param name="control">現在値のコントロール</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedMaxStrength(TextBox control, Division division, CountrySettings settings)
        {
            if (DoubleHelper.IsZero(division.Strength) ||
                DoubleHelper.IsLessOrEqual(division.Strength, division.MaxStrength))
            {
                return;
            }

            // 現在値を最大値に合わせる
            division.Strength = division.MaxStrength;

            // 編集済みフラグを設定する
            division.SetDirty(Division.ItemId.Strength);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 項目の値を更新する
            UpdateItemValue(control, division);

            // 項目の色を更新する
            UpdateItemColor(control, division);
        }

        /// <summary>
        ///     項目値変更後の処理 - 師団の組織率
        /// </summary>
        /// <param name="control">最大値のコントロール</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedOrganisation(TextBox control, Division division, CountrySettings settings)
        {
            if (DoubleHelper.IsZero(division.MaxOrganisation) ||
                DoubleHelper.IsLessOrEqual(division.Organisation, division.MaxOrganisation))
            {
                return;
            }

            // 最大値を現在値に合わせる
            division.MaxOrganisation = division.Organisation;

            // 編集済みフラグを設定する
            division.SetDirty(Division.ItemId.MaxOrganisation);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 項目の値を更新する
            UpdateItemValue(control, division);

            // 項目の色を更新する
            UpdateItemColor(control, division);
        }

        /// <summary>
        ///     項目値変更後の処理 - 師団の最大組織率
        /// </summary>
        /// <param name="control">現在値のコントロール</param>
        /// <param name="division">師団</param>
        /// <param name="settings">国家設定</param>
        private void PostItemChangedMaxOrganisation(TextBox control, Division division, CountrySettings settings)
        {
            if (DoubleHelper.IsZero(division.Organisation) ||
                DoubleHelper.IsLessOrEqual(division.Organisation, division.MaxOrganisation))
            {
                return;
            }

            // 現在値を最大値に合わせる
            division.Organisation = division.MaxOrganisation;

            // 編集済みフラグを設定する
            division.SetDirty(Division.ItemId.Organisation);
            settings.SetDirty();
            Scenarios.SetDirty();

            // 項目の値を更新する
            UpdateItemValue(control, division);

            // 項目の色を更新する
            UpdateItemColor(control, division);
        }

        #endregion

        #region 編集項目 - ログ出力

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val)
        {
            Log.Info("[Scenario] {0}: {1} -> {2}", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId)), ObjectHelper.ToString(val));
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="major">主要国設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, MajorCountrySettings major)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, major)), ObjectHelper.ToString(val),
                Countries.Strings[(int) major.Country]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="alliance">同盟</param>
        /// <param name="index">同盟リストのインデックス</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Alliance alliance, int index)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, alliance)), ObjectHelper.ToString(val), index);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="war">戦争</param>
        /// <param name="index">戦争リストのインデックス</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, War war, int index)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, war)), ObjectHelper.ToString(val), index);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="country">選択国</param>
        /// <param name="relation">国家関係</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Country country,
            Relation relation)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3}=>{4})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, relation)), ObjectHelper.ToString(val),
                Countries.Strings[(int) country], Countries.Strings[(int) relation.Country]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="treaty">協定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Treaty treaty)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3}:{4})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, treaty)), ObjectHelper.ToString(val),
                Countries.Strings[(int) treaty.Country1], Countries.Strings[(int) treaty.Country2]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="prev">変更前の値</param>
        /// <param name="treaty">協定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, object prev, Treaty treaty)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3}:{4})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(prev), ObjectHelper.ToString(val), Countries.Strings[(int) treaty.Country1],
                Countries.Strings[(int) treaty.Country2]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="country">選択国</param>
        /// <param name="spy">諜報設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Country country, SpySettings spy)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3}=>{4})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, spy)), ObjectHelper.ToString(val),
                Countries.Strings[(int) country], Countries.Strings[(int) spy.Country]);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="settings">国家設定</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, CountrySettings settings)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, settings)), ObjectHelper.ToString(val),
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
                        province.Id, Countries.Strings[(int) settings.Country]);
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
                case ScenarioEditorItemId.ProvinceNameKey:
                case ScenarioEditorItemId.ProvinceNameString:
                    Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                        GetItemValue(itemId, province, settings), val, province.Id);
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

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="unit">ユニット</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Unit unit)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, unit)), ObjectHelper.ToString(val), unit.Name);
        }

        /// <summary>
        ///     編集項目の値変更時のログを出力する
        /// </summary>
        /// <param name="itemId">項目ID</param>
        /// <param name="val">編集項目の値</param>
        /// <param name="division">師団</param>
        public void OutputItemValueChangedLog(ScenarioEditorItemId itemId, object val, Division division)
        {
            Log.Info("[Scenario] {0}: {1} -> {2} ({3})", ItemStrings[(int) itemId],
                ObjectHelper.ToString(GetItemValue(itemId, division)), ObjectHelper.ToString(val), division.Name);
        }

        #endregion

        #endregion
    }

    /// <summary>
    ///     シナリオエディタの項目ID
    /// </summary>
    public enum ScenarioEditorItemId
    {
        ScenarioName,
        ScenarioPanelName,
        ScenarioStartYear,
        ScenarioStartMonth,
        ScenarioStartDay,
        ScenarioEndYear,
        ScenarioEndMonth,
        ScenarioEndDay,
        ScenarioIncludeFolder,
        ScenarioBattleScenario,
        ScenarioFreeSelection,
        ScenarioAllowDiplomacy,
        ScenarioAllowProduction,
        ScenarioAllowTechnology,
        ScenarioAiAggressive,
        ScenarioDifficulty,
        ScenarioGameSpeed,
        MajorCountryNameKey,
        MajorCountryNameString,
        MajorFlagExt,
        MajorCountryDescKey,
        MajorCountryDescString,
        MajorPropaganada,
        AllianceName,
        AllianceType,
        AllianceId,
        WarStartYear,
        WarStartMonth,
        WarStartDay,
        WarEndYear,
        WarEndMonth,
        WarEndDay,
        WarType,
        WarId,
        WarAttackerType,
        WarAttackerId,
        WarDefenderType,
        WarDefenderId,
        DiplomacyRelationValue,
        DiplomacyMaster,
        DiplomacyMilitaryControl,
        DiplomacyMilitaryAccess,
        DiplomacyGuaranteed,
        DiplomacyGuaranteedEndYear,
        DiplomacyGuaranteedEndMonth,
        DiplomacyGuaranteedEndDay,
        DiplomacyNonAggression,
        DiplomacyNonAggressionStartYear,
        DiplomacyNonAggressionStartMonth,
        DiplomacyNonAggressionStartDay,
        DiplomacyNonAggressionEndYear,
        DiplomacyNonAggressionEndMonth,
        DiplomacyNonAggressionEndDay,
        DiplomacyNonAggressionType,
        DiplomacyNonAggressionId,
        DiplomacyPeace,
        DiplomacyPeaceStartYear,
        DiplomacyPeaceStartMonth,
        DiplomacyPeaceStartDay,
        DiplomacyPeaceEndYear,
        DiplomacyPeaceEndMonth,
        DiplomacyPeaceEndDay,
        DiplomacyPeaceType,
        DiplomacyPeaceId,
        IntelligenceSpies,
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
        CountryNameKey,
        CountryNameString,
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
        ProvinceNameKey,
        ProvinceNameString,
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
        ProvinceNuclearPowerRelative,
        UnitType,
        UnitId,
        UnitName,
        UnitLocationId,
        UnitLocation,
        UnitBaseId,
        UnitBase,
        UnitLeaderId,
        UnitLeader,
        UnitMorale,
        UnitDigIn,
        DivisionType,
        DivisionId,
        DivisionName,
        DivisionUnitType,
        DivisionModel,
        DivisionBrigadeType1,
        DivisionBrigadeType2,
        DivisionBrigadeType3,
        DivisionBrigadeType4,
        DivisionBrigadeType5,
        DivisionBrigadeModel1,
        DivisionBrigadeModel2,
        DivisionBrigadeModel3,
        DivisionBrigadeModel4,
        DivisionBrigadeModel5,
        DivisionStrength,
        DivisionMaxStrength,
        DivisionOrganisation,
        DivisionMaxOrganisation,
        DivisionMorale,
        DivisionExperience,
        DivisionLocked,
        DivisionDormant
    }
}