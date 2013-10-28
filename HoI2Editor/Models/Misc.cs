using System;
using HoI2Editor.Parsers;
using HoI2Editor.Utilities;
using HoI2Editor.Writers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     miscファイルの設定項目
    /// </summary>
    public static class Misc
    {
        #region 公開プロパティ

        /// <summary>
        ///     輸送艦最大付属装備数
        /// </summary>
        public static int TpMaxAttach
        {
            get { return (_items[(int) MiscItemId.TpMaxAttach] != null) ? (int) _items[(int) MiscItemId.TpMaxAttach] : 0; }
        }

        /// <summary>
        ///     潜水艦最大付属装備数
        /// </summary>
        public static int SsMaxAttach
        {
            get { return (_items[(int) MiscItemId.SsMaxAttach] != null) ? (int) _items[(int) MiscItemId.SsMaxAttach] : 0; }
        }

        /// <summary>
        ///     原子力潜水艦最大付属装備数
        /// </summary>
        public static int SsnMaxAttach
        {
            get { return (_items[(int) MiscItemId.SsnMaxAttach] != null) ? (int) _items[(int) MiscItemId.SsnMaxAttach] : 0; }
        }

        /// <summary>
        ///     駆逐艦最大付属装備数
        /// </summary>
        public static int DdMaxAttach
        {
            get { return (_items[(int) MiscItemId.DdMaxAttach] != null) ? (int) _items[(int) MiscItemId.DdMaxAttach] : 1; }
        }

        /// <summary>
        ///     軽巡洋艦最大付属装備数
        /// </summary>
        public static int ClMaxAttach
        {
            get { return (_items[(int) MiscItemId.ClMaxAttach] != null) ? (int) _items[(int) MiscItemId.ClMaxAttach] : 2; }
        }

        /// <summary>
        ///     重巡洋艦最大付属装備数
        /// </summary>
        public static int CaMaxAttach
        {
            get { return (_items[(int) MiscItemId.CaMaxAttach] != null) ? (int) _items[(int) MiscItemId.CaMaxAttach] : 3; }
        }

        /// <summary>
        ///     巡洋戦艦最大付属装備数
        /// </summary>
        public static int BcMaxAttach
        {
            get { return (_items[(int) MiscItemId.BcMaxAttach] != null) ? (int) _items[(int) MiscItemId.BcMaxAttach] : 4; }
        }

        /// <summary>
        ///     戦艦最大付属装備数
        /// </summary>
        public static int BbMaxAttach
        {
            get { return (_items[(int) MiscItemId.BbMaxAttach] != null) ? (int) _items[(int) MiscItemId.BbMaxAttach] : 5; }
        }

        /// <summary>
        ///     軽空母最大付属装備数
        /// </summary>
        public static int CvlMaxAttach
        {
            get { return (_items[(int) MiscItemId.CvlMaxAttach] != null) ? (int) _items[(int) MiscItemId.CvlMaxAttach] : 1; }
        }

        /// <summary>
        ///     空母最大付属装備数
        /// </summary>
        public static int CvMaxAttach
        {
            get { return (_items[(int) MiscItemId.CvMaxAttach] != null) ? (int) _items[(int) MiscItemId.CvMaxAttach] : 1; }
        }

        /// <summary>
        ///     新形式閣僚ファイルフォーマット
        /// </summary>
        public static bool UseNewMinisterFilesFormat
        {
            get
            {
                return (_items[(int) MiscItemId.UseNewMinisterFilesFormat] != null) &&
                       (bool) _items[(int) MiscItemId.UseNewMinisterFilesFormat];
            }
        }

        /// <summary>
        ///     閣僚引退年を使用
        /// </summary>
        public static bool EnableRetirementYearMinisters
        {
            get
            {
                return (_items[(int) MiscItemId.EnableRetirementYearMinisters] != null) &&
                       (bool) _items[(int) MiscItemId.EnableRetirementYearMinisters];
            }
        }

        /// <summary>
        ///     指揮官引退年を使用
        /// </summary>
        public static bool EnableRetirementYearLeaders
        {
            get
            {
                return (_items[(int) MiscItemId.EnableRetirementYearLeaders] != null) &&
                       (bool) _items[(int) MiscItemId.EnableRetirementYearLeaders];
            }
        }

        /// <summary>
        ///     マップ番号
        /// </summary>
        public static int MapNumber
        {
            get { return (_items[(int) MiscItemId.MapNumber] != null) ? (int) _items[(int) MiscItemId.MapNumber] : 0; }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の値
        /// </summary>
        private static object[] _items = new object[Enum.GetValues(typeof (MiscItemId)).Length];

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (MiscItemId)).Length];

        /// <summary>
        ///     項目のコメント
        /// </summary>
        private static string[] _comments = new string[Enum.GetValues(typeof (MiscItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     セクション名
        /// </summary>
        public static readonly string[] SectionNames =
            {
                "economy",
                "intelligence",
                "diplomacy",
                "combat",
                "mission",
                "country",
                "research",
                "trade",
                "ai",
                "mod",
                "map"
            };

        /// <summary>
        ///     セクションごとの項目
        /// </summary>
        public static MiscItemId[][] SectionItems =
            {
                new[]
                    {
                        MiscItemId.IcToTcRatio,
                        MiscItemId.IcToSuppliesRatio,
                        MiscItemId.IcToConsumerGoodsRatio,
                        MiscItemId.IcToMoneyRatio,
                        MiscItemId.DissentChangeSpeed,
                        MiscItemId.MinAvailableIc,
                        MiscItemId.MinFinalIc,
                        MiscItemId.DissentReduction,
                        MiscItemId.MaxGearingBonus,
                        MiscItemId.GearingBonusIncrement,
                        MiscItemId.GearingResourceIncrement,
                        MiscItemId.GearingLossNoIc,
                        MiscItemId.IcMultiplierNonNational,
                        MiscItemId.IcMultiplierNonOwned,
                        MiscItemId.IcMultiplierPuppet,
                        MiscItemId.ResourceMultiplierNonNational,
                        MiscItemId.ResourceMultiplierNonOwned,
                        MiscItemId.ResourceMultiplierNonNationalAi,
                        MiscItemId.ResourceMultiplierPuppet,
                        MiscItemId.TcLoadUndeployedDivision,
                        MiscItemId.TcLoadOccupied,
                        MiscItemId.TcLoadMultiplierLand,
                        MiscItemId.TcLoadMultiplierAir,
                        MiscItemId.TcLoadMultiplierNaval,
                        MiscItemId.TcLoadPartisan,
                        MiscItemId.TcLoadFactorOffensive,
                        MiscItemId.TcLoadProvinceDevelopment,
                        MiscItemId.TcLoadBase,
                        MiscItemId.ManpowerMultiplierNational,
                        MiscItemId.ManpowerMultiplierNonNational,
                        MiscItemId.ManpowerMultiplierColony,
                        MiscItemId.ManpowerMultiplierPuppet,
                        MiscItemId.ManpowerMultiplierWartimeOversea,
                        MiscItemId.ManpowerMultiplierPeacetime,
                        MiscItemId.ManpowerMultiplierWartime,
                        MiscItemId.DailyRetiredManpower,
                        MiscItemId.RequirementAffectSlider,
                        MiscItemId.TrickleBackFactorManpower,
                        MiscItemId.ReinforceManpower,
                        MiscItemId.ReinforceCost,
                        MiscItemId.ReinforceTime,
                        MiscItemId.UpgradeCost,
                        MiscItemId.UpgradeTime,
                        MiscItemId.ReinforceToUpdateModifier,
                        MiscItemId.NationalismStartingValue,
                        MiscItemId.NationalismPerManpowerAoD,
                        MiscItemId.NationalismPerManpowerDh,
                        MiscItemId.MaxNationalism,
                        MiscItemId.MaxRevoltRisk,
                        MiscItemId.MonthlyNationalismReduction,
                        MiscItemId.SendDivisionDays,
                        MiscItemId.TcLoadUndeployedBrigade,
                        MiscItemId.CanUnitSendNonAllied,
                        MiscItemId.SpyMissionDays,
                        MiscItemId.IncreateIntelligenceLevelDays,
                        MiscItemId.ChanceDetectSpyMission,
                        MiscItemId.RelationshipsHitDetectedMissions,
                        MiscItemId.ShowThirdCountrySpyReports,
                        MiscItemId.DistanceModifierNeighbours,
                        MiscItemId.SpyInformationAccuracyModifier,
                        MiscItemId.AiPeacetimeSpyMissions,
                        MiscItemId.MaxIcCostModifier,
                        MiscItemId.AiSpyMissionsCostModifier,
                        MiscItemId.AiDiplomacyCostModifier,
                        MiscItemId.AiInfluenceModifier,
                        MiscItemId.CostRepairBuildings,
                        MiscItemId.TimeRepairBuilding,
                        MiscItemId.ProvinceEfficiencyRiseTime,
                        MiscItemId.CoreProvinceEfficiencyRiseTime,
                        MiscItemId.LineUpkeep,
                        MiscItemId.LineStartupTime,
                        MiscItemId.LineUpgradeTime,
                        MiscItemId.RetoolingCost,
                        MiscItemId.RetoolingResource,
                        MiscItemId.DailyAgingManpower,
                        MiscItemId.SupplyConvoyHunt,
                        MiscItemId.SupplyNavalStaticAoD,
                        MiscItemId.SupplyNavalMoving,
                        MiscItemId.SupplyNavalBattleAoD,
                        MiscItemId.SupplyAirStaticAoD,
                        MiscItemId.SupplyAirMoving,
                        MiscItemId.SupplyAirBattleAoD,
                        MiscItemId.SupplyAirBombing,
                        MiscItemId.SupplyLandStaticAoD,
                        MiscItemId.SupplyLandMoving,
                        MiscItemId.SupplyLandBattleAoD,
                        MiscItemId.SupplyLandBombing,
                        MiscItemId.SupplyStockLand,
                        MiscItemId.SupplyStockAir,
                        MiscItemId.SupplyStockNaval,
                        MiscItemId.RestockSpeedLand,
                        MiscItemId.RestockSpeedAir,
                        MiscItemId.RestockSpeedNaval,
                        MiscItemId.SyntheticOilConversionMultiplier,
                        MiscItemId.SyntheticRaresConversionMultiplier,
                        MiscItemId.MilitarySalary,
                        MiscItemId.MaxIntelligenceExpenditure,
                        MiscItemId.MaxResearchExpenditure,
                        MiscItemId.MilitarySalaryAttrictionModifier,
                        MiscItemId.MilitarySalaryDissentModifier,
                        MiscItemId.NuclearSiteUpkeepCost,
                        MiscItemId.NuclearPowerUpkeepCost,
                        MiscItemId.SyntheticOilSiteUpkeepCost,
                        MiscItemId.SyntheticRaresSiteUpkeepCost,
                        MiscItemId.DurationDetection,
                        MiscItemId.ConvoyProvinceHostileTime,
                        MiscItemId.ConvoyProvinceBlockedTime,
                        MiscItemId.AutoTradeConvoy,
                        MiscItemId.SpyUpkeepCost,
                        MiscItemId.SpyDetectionChance,
                        MiscItemId.SpyCoupDissentModifier,
                        MiscItemId.InfraEfficiencyModifier,
                        MiscItemId.ManpowerToConsumerGoods,
                        MiscItemId.TimeBetweenSliderChangesAoD,
                        MiscItemId.MinimalPlacementIc,
                        MiscItemId.NuclearPower,
                        MiscItemId.FreeInfraRepair,
                        MiscItemId.MaxSliderDissent,
                        MiscItemId.MinSliderDissent,
                        MiscItemId.MaxDissentSliderMove,
                        MiscItemId.IcConcentrationBonus,
                        MiscItemId.TransportConversion,
                        MiscItemId.ConvoyDutyConversion,
                        MiscItemId.EscortDutyConversion,
                        MiscItemId.MinisterChangeDelay,
                        MiscItemId.MinisterChangeEventDelay,
                        MiscItemId.IdeaChangeDelay,
                        MiscItemId.IdeaChangeEventDelay,
                        MiscItemId.LeaderChangeDelay,
                        MiscItemId.ChangeIdeaDissent,
                        MiscItemId.ChangeMinisterDissent,
                        MiscItemId.MinDissentRevolt,
                        MiscItemId.DissentRevoltMultiplier,
                        MiscItemId.TpMaxAttach,
                        MiscItemId.SsMaxAttach,
                        MiscItemId.SsnMaxAttach,
                        MiscItemId.DdMaxAttach,
                        MiscItemId.ClMaxAttach,
                        MiscItemId.CaMaxAttach,
                        MiscItemId.BcMaxAttach,
                        MiscItemId.BbMaxAttach,
                        MiscItemId.CvlMaxAttach,
                        MiscItemId.CvMaxAttach,
                        MiscItemId.CanChangeIdeas,
                        MiscItemId.CanUnitSendNonAlliedDh,
                        MiscItemId.BluePrintsCanSoldNonAllied,
                        MiscItemId.ProvinceCanSoldNonAllied,
                        MiscItemId.TransferAlliedCoreProvinces,
                        MiscItemId.ProvinceBuildingsRepairModifier,
                        MiscItemId.ProvinceResourceRepairModifier,
                        MiscItemId.StockpileLimitMultiplierResource,
                        MiscItemId.StockpileLimitMultiplierSuppliesOil,
                        MiscItemId.OverStockpileLimitDailyLoss,
                        MiscItemId.MaxResourceDepotSize,
                        MiscItemId.MaxSuppliesOilDepotSize,
                        MiscItemId.DesiredStockPilesSuppliesOil,
                        MiscItemId.MaxManpower,
                        MiscItemId.ConvoyTransportsCapacity,
                        MiscItemId.SuppyLandStaticDh,
                        MiscItemId.SupplyLandBattleDh,
                        MiscItemId.FuelLandStatic,
                        MiscItemId.FuelLandBattle,
                        MiscItemId.SupplyAirStaticDh,
                        MiscItemId.SupplyAirBattleDh,
                        MiscItemId.FuelAirNavalStatic,
                        MiscItemId.FuelAirBattle,
                        MiscItemId.SupplyNavalStaticDh,
                        MiscItemId.SupplyNavalBattleDh,
                        MiscItemId.FuelNavalNotMoving,
                        MiscItemId.FuelNavalBattle,
                        MiscItemId.TpTransportsConversionRatio,
                        MiscItemId.DdEscortsConversionRatio,
                        MiscItemId.ClEscortsConversionRatio,
                        MiscItemId.CvlEscortsConversionRatio,
                        MiscItemId.ProductionLineEdit,
                        MiscItemId.GearingBonusLossUpgradeUnit,
                        MiscItemId.GearingBonusLossUpgradeBrigade,
                        MiscItemId.DissentNukes,
                        MiscItemId.MaxDailyDissent,
                        MiscItemId.NukesProductionModifier,
                        MiscItemId.ConvoySystemOptionsAllied,
                        MiscItemId.ResourceConvoysBackUnneeded,
                        MiscItemId.EconomyEnd
                    },
                new[]
                    {
                        MiscItemId.SpyMissionDaysDh,
                        MiscItemId.IncreateIntelligenceLevelDaysDh,
                        MiscItemId.ChanceDetectSpyMissionDh,
                        MiscItemId.RelationshipsHitDetectedMissionsDh,
                        MiscItemId.DistanceModifier,
                        MiscItemId.DistanceModifierNeighboursDh,
                        MiscItemId.SpyLevelBonusDistanceModifier,
                        MiscItemId.SpyLevelBonusDistanceModifierAboveTen,
                        MiscItemId.SpyInformationAccuracyModifierDh,
                        MiscItemId.IcModifierCost,
                        MiscItemId.MinIcCostModifier,
                        MiscItemId.MaxIcCostModifierDh,
                        MiscItemId.ExtraMaintenanceCostAboveTen,
                        MiscItemId.ExtraCostIncreasingAboveTen,
                        MiscItemId.ShowThirdCountrySpyReportsDh,
                        MiscItemId.SpiesMoneyModifier,
                        MiscItemId.IntelligenceEnd
                    },
                new[]
                    {
                        MiscItemId.DaysBetweenDiplomaticMissions,
                        MiscItemId.TimeBetweenSliderChangesDh,
                        MiscItemId.RequirementAffectSliderDh,
                        MiscItemId.UseMinisterPersonalityReplacing,
                        MiscItemId.RelationshipHitCancelTrade,
                        MiscItemId.RelationshipHitCancelPermanentTrade,
                        MiscItemId.PuppetsJoinMastersAlliance,
                        MiscItemId.MastersBecomePuppetsPuppets,
                        MiscItemId.AllowManualClaimsChange,
                        MiscItemId.BelligerenceClaimedProvince,
                        MiscItemId.BelligerenceClaimsRemoval,
                        MiscItemId.JoinAutomaticallyAllesAxis,
                        MiscItemId.AllowChangeHosHog,
                        MiscItemId.ChangeTagCoup,
                        MiscItemId.FilterReleaseCountries,
                        MiscItemId.DiplomacyEnd
                    },
                new[]
                    {
                        MiscItemId.LandXpGainFactor,
                        MiscItemId.NavalXpGainFactor,
                        MiscItemId.AirXpGainFactor,
                        MiscItemId.AirDogfightXpGainFactor,
                        MiscItemId.DivisionXpGainFactor,
                        MiscItemId.LeaderXpGainFactor,
                        MiscItemId.AttritionSeverityModifier,
                        MiscItemId.NoSupplyAttritionSeverity,
                        MiscItemId.NoSupplyMinimunAttrition,
                        MiscItemId.BaseProximity,
                        MiscItemId.ShoreBombardmentModifier,
                        MiscItemId.ShoreBombardmentCap,
                        MiscItemId.InvasionModifier,
                        MiscItemId.MultipleCombatModifier,
                        MiscItemId.OffensiveCombinedArmsBonus,
                        MiscItemId.DefensiveCombinedArmsBonus,
                        MiscItemId.SurpriseModifier,
                        MiscItemId.LandCommandLimitModifier,
                        MiscItemId.AirCommandLimitModifier,
                        MiscItemId.NavalCommandLimitModifier,
                        MiscItemId.EnvelopmentModifier,
                        MiscItemId.EncircledModifier,
                        MiscItemId.LandFortMultiplier,
                        MiscItemId.CoastalFortMultiplier,
                        MiscItemId.HardUnitsAttackingUrbanPenalty,
                        MiscItemId.DissentMultiplier,
                        MiscItemId.SupplyProblemsModifier,
                        MiscItemId.SupplyProblemsModifierLand,
                        MiscItemId.SupplyProblemsModifierAir,
                        MiscItemId.SupplyProblemsModifierNaval,
                        MiscItemId.FuelProblemsModifierLand,
                        MiscItemId.FuelProblemsModifierAir,
                        MiscItemId.FuelProblemsModifierNaval,
                        MiscItemId.RaderStationMultiplier,
                        MiscItemId.RaderStationAaMultiplier,
                        MiscItemId.InterceptorBomberModifier,
                        MiscItemId.AirOverstackingModifier,
                        MiscItemId.AirOverstackingModifierAoD,
                        MiscItemId.NavalOverstackingModifier,
                        MiscItemId.LandLeaderCommandLimitRank0,
                        MiscItemId.LandLeaderCommandLimitRank1,
                        MiscItemId.LandLeaderCommandLimitRank2,
                        MiscItemId.LandLeaderCommandLimitRank3,
                        MiscItemId.AirLeaderCommandLimitRank0,
                        MiscItemId.AirLeaderCommandLimitRank1,
                        MiscItemId.AirLeaderCommandLimitRank2,
                        MiscItemId.AirLeaderCommandLimitRank3,
                        MiscItemId.NavalLeaderCommandLimitRank0,
                        MiscItemId.NavalLeaderCommandLimitRank1,
                        MiscItemId.NavalLeaderCommandLimitRank2,
                        MiscItemId.NavalLeaderCommandLimitRank3,
                        MiscItemId.HqCommandLimitFactor,
                        MiscItemId.ConvoyProtectionFactor,
                        MiscItemId.ConvoyEscortsModel,
                        MiscItemId.DelayAfterCombatEnds,
                        MiscItemId.LandDelayBeforeOrders,
                        MiscItemId.NavalDelayBeforeOrders,
                        MiscItemId.AirDelayBeforeOrders,
                        MiscItemId.MaximumSizesAirStacks,
                        MiscItemId.DurationAirToAirBattles,
                        MiscItemId.DurationNavalPortBombing,
                        MiscItemId.DurationStrategicBombing,
                        MiscItemId.DurationGroundAttackBombing,
                        MiscItemId.EffectExperienceCombat,
                        MiscItemId.DamageNavalBasesBombing,
                        MiscItemId.DamageAirBaseBombing,
                        MiscItemId.DamageAaBombing,
                        MiscItemId.DamageRocketBombing,
                        MiscItemId.DamageNukeBombing,
                        MiscItemId.DamageRadarBombing,
                        MiscItemId.DamageInfraBombing,
                        MiscItemId.DamageIcBombing,
                        MiscItemId.DamageResourcesBombing,
                        MiscItemId.DamageSyntheticOilBombing,
                        MiscItemId.HowEffectiveGroundDef,
                        MiscItemId.ChanceAvoidDefencesLeft,
                        MiscItemId.ChanceAvoidNoDefences,
                        MiscItemId.LandChanceAvoidDefencesLeft,
                        MiscItemId.AirChanceAvoidDefencesLeft,
                        MiscItemId.NavalChanceAvoidDefencesLeft,
                        MiscItemId.LandChanceAvoidNoDefences,
                        MiscItemId.AirChanceAvoidNoDefences,
                        MiscItemId.NavalChanceAvoidNoDefences,
                        MiscItemId.ChanceGetTerrainTrait,
                        MiscItemId.ChanceGetEventTrait,
                        MiscItemId.BonusTerrainTrait,
                        MiscItemId.BonusSimilarTerrainTrait,
                        MiscItemId.BonusEventTrait,
                        MiscItemId.BonusLeaderSkillPointLand,
                        MiscItemId.BonusLeaderSkillPointAir,
                        MiscItemId.BonusLeaderSkillPointNaval,
                        MiscItemId.ChanceLeaderDying,
                        MiscItemId.AirOrgDamage,
                        MiscItemId.AirStrDamageOrg,
                        MiscItemId.AirStrDamage,
                        MiscItemId.LandMinOrgDamage,
                        MiscItemId.LandOrgDamageHardSoftEach,
                        MiscItemId.LandOrgDamageHardVsSoft,
                        MiscItemId.LandMinStrDamage,
                        MiscItemId.LandStrDamageHardSoftEach,
                        MiscItemId.LandStrDamageHardVsSoft,
                        MiscItemId.AirMinOrgDamage,
                        MiscItemId.AirAdditionalOrgDamage,
                        MiscItemId.AirMinStrDamage,
                        MiscItemId.AirAdditionalStrDamage,
                        MiscItemId.AirStrDamageEntrenced,
                        MiscItemId.NavalMinOrgDamage,
                        MiscItemId.NavalAdditionalOrgDamage,
                        MiscItemId.NavalMinStrDamage,
                        MiscItemId.NavalAdditionalStrDamage,
                        MiscItemId.AirStrDamageLandOrg,
                        MiscItemId.AirOrgDamageLandDh,
                        MiscItemId.AirStrDamageLandDh,
                        MiscItemId.LandOrgDamageLandOrg,
                        MiscItemId.LandOrgDamageLandUrban,
                        MiscItemId.LandOrgDamageLandFort,
                        MiscItemId.RequiredLandFortSize,
                        MiscItemId.LandStrDamageLandDh,
                        MiscItemId.AirOrgDamageAirDh,
                        MiscItemId.AirStrDamageAirDh,
                        MiscItemId.LandOrgDamageAirDh,
                        MiscItemId.LandStrDamageAirDh,
                        MiscItemId.NavalOrgDamageAirDh,
                        MiscItemId.NavalStrDamageAirDh,
                        MiscItemId.SubsOrgDamageAir,
                        MiscItemId.SubsStrDamageAir,
                        MiscItemId.AirOrgDamageNavyDh,
                        MiscItemId.AirStrDamageNavyDh,
                        MiscItemId.NavalOrgDamageNavyDh,
                        MiscItemId.NavalStrDamageNavyDh,
                        MiscItemId.SubsOrgDamageNavy,
                        MiscItemId.SubsStrDamageNavy,
                        MiscItemId.SubsOrgDamage,
                        MiscItemId.SubsStrDamage,
                        MiscItemId.SubStacksDetectionModifier,
                        MiscItemId.AirOrgDamageLandAoD,
                        MiscItemId.AirStrDamageLandAoD,
                        MiscItemId.LandDamageArtilleryBombardment,
                        MiscItemId.InfraDamageArtilleryBombardment,
                        MiscItemId.IcDamageArtilleryBombardment,
                        MiscItemId.ResourcesDamageArtilleryBombardment,
                        MiscItemId.PenaltyArtilleryBombardment,
                        MiscItemId.ArtilleryStrDamage,
                        MiscItemId.ArtilleryOrgDamage,
                        MiscItemId.LandStrDamageLandAoD,
                        MiscItemId.LandOrgDamageLand,
                        MiscItemId.LandStrDamageAirAoD,
                        MiscItemId.LandOrgDamageAirAoD,
                        MiscItemId.NavalStrDamageAirAoD,
                        MiscItemId.NavalOrgDamageAirAoD,
                        MiscItemId.AirStrDamageAirAoD,
                        MiscItemId.AirOrgDamageAirAoD,
                        MiscItemId.NavalStrDamageNavyAoD,
                        MiscItemId.NavalOrgDamageNavyAoD,
                        MiscItemId.AirStrDamageNavyAoD,
                        MiscItemId.AirOrgDamageNavyAoD,
                        MiscItemId.MilitaryExpenseAttritionModifier,
                        MiscItemId.NavalMinCombatTime,
                        MiscItemId.LandMinCombatTime,
                        MiscItemId.AirMinCombatTime,
                        MiscItemId.LandOverstackingModifier,
                        MiscItemId.LandOrgLossMoving,
                        MiscItemId.AirOrgLossMoving,
                        MiscItemId.NavalOrgLossMoving,
                        MiscItemId.SupplyDistanceSeverity,
                        MiscItemId.SupplyBase,
                        MiscItemId.LandOrgGain,
                        MiscItemId.AirOrgGain,
                        MiscItemId.NavalOrgGain,
                        MiscItemId.NukeManpowerDissent,
                        MiscItemId.NukeIcDissent,
                        MiscItemId.NukeTotalDissent,
                        MiscItemId.LandFriendlyOrgGain,
                        MiscItemId.AirLandStockModifier,
                        MiscItemId.ScorchDamage,
                        MiscItemId.StandGroundDissent,
                        MiscItemId.ScorchGroundBelligerence,
                        MiscItemId.DefaultLandStack,
                        MiscItemId.DefaultNavalStack,
                        MiscItemId.DefaultAirStack,
                        MiscItemId.DefaultRocketStack,
                        MiscItemId.FortDamageArtilleryBombardment,
                        MiscItemId.ArtilleryBombardmentOrgCost,
                        MiscItemId.LandDamageFort,
                        MiscItemId.AirRebaseFactor,
                        MiscItemId.AirMaxDisorganized,
                        MiscItemId.AaInflictedStrDamage,
                        MiscItemId.AaInflictedOrgDamage,
                        MiscItemId.AaInflictedFlyingDamage,
                        MiscItemId.AaInflictedBombingDamage,
                        MiscItemId.HardAttackStrDamage,
                        MiscItemId.HardAttackOrgDamage,
                        MiscItemId.ArmorSoftBreakthroughMin,
                        MiscItemId.ArmorSoftBreakthroughMax,
                        MiscItemId.NavalCriticalHitChance,
                        MiscItemId.NavalCriticalHitEffect,
                        MiscItemId.LandFortDamage,
                        MiscItemId.PortAttackSurpriseChanceDay,
                        MiscItemId.PortAttackSurpriseChanceNight,
                        MiscItemId.PortAttackSurpriseModifier,
                        MiscItemId.RadarAntiSurpriseChance,
                        MiscItemId.RadarAntiSurpriseModifier,
                        MiscItemId.CounterAttackStrDefenderAoD,
                        MiscItemId.CounterAttackOrgDefenderAoD,
                        MiscItemId.CounterAttackStrAttackerAoD,
                        MiscItemId.CounterAttackOrgAttackerAoD,
                        MiscItemId.AssaultStrDefenderAoD,
                        MiscItemId.AssaultOrgDefenderAoD,
                        MiscItemId.AssaultStrAttackerAoD,
                        MiscItemId.AssaultOrgAttackerAoD,
                        MiscItemId.EncirclementStrDefenderAoD,
                        MiscItemId.EncirclementOrgDefenderAoD,
                        MiscItemId.EncirclementStrAttackerAoD,
                        MiscItemId.EncirclementOrgAttackerAoD,
                        MiscItemId.AmbushStrDefenderAoD,
                        MiscItemId.AmbushOrgDefenderAoD,
                        MiscItemId.AmbushStrAttackerAoD,
                        MiscItemId.AmbushOrgAttackerAoD,
                        MiscItemId.DelayStrDefenderAoD,
                        MiscItemId.DelayOrgDefenderAoD,
                        MiscItemId.DelayStrAttackerAoD,
                        MiscItemId.DelayOrgAttackerAoD,
                        MiscItemId.TacticalWithdrawStrDefenderAoD,
                        MiscItemId.TacticalWithdrawOrgDefenderAoD,
                        MiscItemId.TacticalWithdrawStrAttackerAoD,
                        MiscItemId.TacticalWithdrawOrgAttackerAoD,
                        MiscItemId.BreakthroughStrDefenderAoD,
                        MiscItemId.BreakthroughOrgDefenderAoD,
                        MiscItemId.BreakthroughStrAttackerAoD,
                        MiscItemId.BreakthroughOrgAttackerAoD,
                        MiscItemId.NavalOrgDamageAa,
                        MiscItemId.AirOrgDamageAa,
                        MiscItemId.AirStrDamageAa,
                        MiscItemId.AaAirFiringRules,
                        MiscItemId.AaAirNightModifier,
                        MiscItemId.AaAirBonusRadars,
                        MiscItemId.MovementBonusTerrainTrait,
                        MiscItemId.MovementBonusSimilarTerrainTrait,
                        MiscItemId.LogisticsWizardEseBonus,
                        MiscItemId.DaysOffensiveSupply,
                        MiscItemId.MinisterBonuses,
                        MiscItemId.OrgRegainBonusFriendly,
                        MiscItemId.OrgRegainBonusFriendlyCap,
                        MiscItemId.ConvoyInterceptionMissions,
                        MiscItemId.AutoReturnTransportFleets,
                        MiscItemId.AllowProvinceRegionTargeting,
                        MiscItemId.NightHoursWinter,
                        MiscItemId.NightHoursSpringFall,
                        MiscItemId.NightHoursSummer,
                        MiscItemId.RecalculateLandArrivalTimes,
                        MiscItemId.SynchronizeArrivalTimePlayer,
                        MiscItemId.SynchronizeArrivalTimeAi,
                        MiscItemId.RecalculateArrivalTimesCombat,
                        MiscItemId.LandSpeedModifierCombat,
                        MiscItemId.LandSpeedModifierBombardment,
                        MiscItemId.LandSpeedModifierSupply,
                        MiscItemId.LandSpeedModifierOrg,
                        MiscItemId.LandAirSpeedModifierFuel,
                        MiscItemId.DefaultSpeedFuel,
                        MiscItemId.FleetSizeRangePenaltyRatio,
                        MiscItemId.FleetSizeRangePenaltyThrethold,
                        MiscItemId.FleetSizeRangePenaltyMax,
                        MiscItemId.ApplyRangeLimitsAreasRegions,
                        MiscItemId.RadarBonusDetection,
                        MiscItemId.BonusDetectionFriendly,
                        MiscItemId.ScreensCapitalRatioModifier,
                        MiscItemId.ChanceTargetNoOrgLand,
                        MiscItemId.ScreenCapitalShipsTargeting,
                        MiscItemId.FleetPositioningDaytime,
                        MiscItemId.FleetPositioningLeaderSkill,
                        MiscItemId.FleetPositioningFleetSize,
                        MiscItemId.FleetPositioningFleetComposition,
                        MiscItemId.LandCoastalFortsDamage,
                        MiscItemId.LandCoastalFortsMaxDamage,
                        MiscItemId.MinSoftnessBrigades,
                        MiscItemId.AutoRetreatOrg,
                        MiscItemId.LandOrgNavalTransportation,
                        MiscItemId.MaxLandDig,
                        MiscItemId.DigIncreaseDay,
                        MiscItemId.BreakthroughEncirclementMinSpeed,
                        MiscItemId.BreakthroughEncirclementMaxChance,
                        MiscItemId.BreakthroughEncirclementChanceModifier,
                        MiscItemId.CombatEventDuration,
                        MiscItemId.CounterAttackOrgAttackerDh,
                        MiscItemId.CounterAttackStrAttackerDh,
                        MiscItemId.CounterAttackOrgDefenderDh,
                        MiscItemId.CounterAttackStrDefenderDh,
                        MiscItemId.AssaultOrgAttackerDh,
                        MiscItemId.AssaultStrAttackerDh,
                        MiscItemId.AssaultOrgDefenderDh,
                        MiscItemId.AssaultStrDefenderDh,
                        MiscItemId.EncirclementOrgAttackerDh,
                        MiscItemId.EncirclementStrAttackerDh,
                        MiscItemId.EncirclementOrgDefenderDh,
                        MiscItemId.EncirclementStrDefenderDh,
                        MiscItemId.AmbushOrgAttackerDh,
                        MiscItemId.AmbushStrAttackerDh,
                        MiscItemId.AmbushOrgDefenderDh,
                        MiscItemId.AmbushStrDefenderDh,
                        MiscItemId.DelayOrgAttackerDh,
                        MiscItemId.DelayStrAttackerDh,
                        MiscItemId.DelayOrgDefenderDh,
                        MiscItemId.DelayStrDefenderDh,
                        MiscItemId.TacticalWithdrawOrgAttackerDh,
                        MiscItemId.TacticalWithdrawStrAttackerDh,
                        MiscItemId.TacticalWithdrawOrgDefenderDh,
                        MiscItemId.TacticalWithdrawStrDefenderDh,
                        MiscItemId.BreakthroughOrgAttackerDh,
                        MiscItemId.BreakthroughStrAttackerDh,
                        MiscItemId.BreakthroughOrgDefenderDh,
                        MiscItemId.BreakthroughStrDefenderDh,
                        MiscItemId.HqStrDamageBreakthrough,
                        MiscItemId.CombatMode,
                        MiscItemId.CombatEnd
                    }
                ,
                new[]
                    {
                        MiscItemId.AttackMission,
                        MiscItemId.AttackStartingEfficiency,
                        MiscItemId.AttackSpeedBonus,
                        MiscItemId.RebaseMission,
                        MiscItemId.RebaseStartingEfficiency,
                        MiscItemId.RebaseChanceDetected,
                        MiscItemId.StratRedeployMission,
                        MiscItemId.StratRedeployStartingEfficiency,
                        MiscItemId.StratRedeployAddedValue,
                        MiscItemId.StratRedeployDistanceMultiplier,
                        MiscItemId.SupportAttackMission,
                        MiscItemId.SupportAttackStartingEfficiency,
                        MiscItemId.SupportAttackSpeedBonus,
                        MiscItemId.SupportDefenseMission,
                        MiscItemId.SupportDefenseStartingEfficiency,
                        MiscItemId.SupportDefenseSpeedBonus,
                        MiscItemId.ReservesMission,
                        MiscItemId.ReservesStartingEfficiency,
                        MiscItemId.ReservesSpeedBonus,
                        MiscItemId.AntiPartisanDutyMission,
                        MiscItemId.AntiPartisanDutyStartingEfficiency,
                        MiscItemId.AntiPartisanDutySuppression,
                        MiscItemId.PlannedDefenseMission,
                        MiscItemId.PlannedDefenseStartingEfficiency,
                        MiscItemId.AirSuperiorityMission,
                        MiscItemId.AirSuperiorityStartingEfficiency,
                        MiscItemId.AirSuperiorityDetection,
                        MiscItemId.AirSuperiorityMinRequired,
                        MiscItemId.GroundAttackMission,
                        MiscItemId.GroundAttackStartingEfficiency,
                        MiscItemId.GroundAttackOrgDamage,
                        MiscItemId.GroundAttackStrDamage,
                        MiscItemId.InterdictionMission,
                        MiscItemId.InterdictionStartingEfficiency,
                        MiscItemId.InterdictionOrgDamage,
                        MiscItemId.InterdictionStrDamage,
                        MiscItemId.StrategicBombardmentMission,
                        MiscItemId.StrategicBombardmentStartingEfficiency,
                        MiscItemId.LogisticalStrikeMission,
                        MiscItemId.LogisticalStrikeStartingEfficiency,
                        MiscItemId.RunwayCrateringMission,
                        MiscItemId.RunwayCrateringStartingEfficiency,
                        MiscItemId.InstallationStrikeMission,
                        MiscItemId.InstallationStrikeStartingEfficiency,
                        MiscItemId.NavalStrikeMission,
                        MiscItemId.NavalStrikeStartingEfficiency,
                        MiscItemId.PortStrikeMission,
                        MiscItemId.PortStrikeStartingEfficiency,
                        MiscItemId.ConvoyAirRaidingMission,
                        MiscItemId.ConvoyAirRaidingStartingEfficiency,
                        MiscItemId.AirSupplyMission,
                        MiscItemId.AirSupplyStartingEfficiency,
                        MiscItemId.AirborneAssaultMission,
                        MiscItemId.AirborneAssaultStartingEfficiency,
                        MiscItemId.NukeMission,
                        MiscItemId.NukeStartingEfficiency,
                        MiscItemId.AirScrambleMission,
                        MiscItemId.AirScrambleStartingEfficiency,
                        MiscItemId.AirScrambleDetection,
                        MiscItemId.AirScrambleMinRequired,
                        MiscItemId.ConvoyRadingMission,
                        MiscItemId.ConvoyRadingStartingEfficiency,
                        MiscItemId.ConvoyRadingRangeModifier,
                        MiscItemId.ConvoyRadingChanceDetected,
                        MiscItemId.AswMission,
                        MiscItemId.AswStartingEfficiency,
                        MiscItemId.NavalInterdictionMission,
                        MiscItemId.NavalInterdictionStartingEfficiency,
                        MiscItemId.ShoreBombardmentMission,
                        MiscItemId.ShoreBombardmentStartingEfficiency,
                        MiscItemId.ShoreBombardmentModifierDh,
                        MiscItemId.AmphibousAssaultMission,
                        MiscItemId.AmphibousAssaultStartingEfficiency,
                        MiscItemId.SeaTransportMission,
                        MiscItemId.SeaTransportStartingEfficiency,
                        MiscItemId.SeaTransportRangeModifier,
                        MiscItemId.SeaTransportChanceDetected,
                        MiscItemId.NavalCombatPatrolMission,
                        MiscItemId.NavalCombatPatrolStartingEfficiency,
                        MiscItemId.NavalPortStrikeMission,
                        MiscItemId.NavalPortStrikeStartingEfficiency,
                        MiscItemId.NavalAirbaseStrikeMission,
                        MiscItemId.NavalAirbaseStrikeStartingEfficiency,
                        MiscItemId.SneakMoveMission,
                        MiscItemId.SneakMoveStartingEfficiency,
                        MiscItemId.SneakMoveRangeModifier,
                        MiscItemId.SneakMoveChanceDetected,
                        MiscItemId.NavalScrambleMission,
                        MiscItemId.NavalScrambleStartingEfficiency,
                        MiscItemId.NavalScrambleSpeedBonus,
                        MiscItemId.UseAttackEfficiencyCombatModifier,
                        MiscItemId.MissionEnd
                    },
                new[]
                    {
                        MiscItemId.LandFortEfficiency,
                        MiscItemId.CoastalFortEfficiency,
                        MiscItemId.GroundDefenseEfficiency,
                        MiscItemId.ConvoyDefenseEfficiency,
                        MiscItemId.ManpowerBoost,
                        MiscItemId.TransportCapacityModifier,
                        MiscItemId.OccupiedTransportCapacityModifier,
                        MiscItemId.AttritionModifier,
                        MiscItemId.ManpowerTrickleBackModifier,
                        MiscItemId.SupplyDistanceModifier,
                        MiscItemId.RepairModifier,
                        MiscItemId.ResearchModifier,
                        MiscItemId.RadarEfficiency,
                        MiscItemId.HqSupplyEfficiencyBonus,
                        MiscItemId.HqCombatEventsBonus,
                        MiscItemId.CombatEventChances,
                        MiscItemId.FriendlyArmyDetectionChance,
                        MiscItemId.EnemyArmyDetectionChance,
                        MiscItemId.FriendlyIntelligenceChance,
                        MiscItemId.EnemyIntelligenceChance,
                        MiscItemId.MaxAmphibiousArmySize,
                        MiscItemId.EnergyToOil,
                        MiscItemId.TotalProductionEfficiency,
                        MiscItemId.SupplyProductionEfficiency,
                        MiscItemId.AaPower,
                        MiscItemId.AirSurpriseChance,
                        MiscItemId.LandSurpriseChance,
                        MiscItemId.NavalSurpriseChance,
                        MiscItemId.PeacetimeIcModifier,
                        MiscItemId.WartimeIcModifier,
                        MiscItemId.BuildingsProductionModifier,
                        MiscItemId.ConvoysProductionModifier,
                        MiscItemId.MinShipsPositioningBattle,
                        MiscItemId.MaxShipsPositioningBattle,
                        MiscItemId.PeacetimeStockpilesResources,
                        MiscItemId.WartimeStockpilesResources,
                        MiscItemId.PeacetimeStockpilesOilSupplies,
                        MiscItemId.WartimeStockpilesOilSupplies,
                        MiscItemId.CountryEnd
                    },
                new[]
                    {
                        MiscItemId.BlueprintBonus,
                        MiscItemId.PreHistoricalDateModifier,
                        MiscItemId.PostHistoricalDateModifierDh,
                        MiscItemId.CostSkillLevel,
                        MiscItemId.MeanNumberInventionEventsYear,
                        MiscItemId.PostHistoricalDateModifierAoD,
                        MiscItemId.TechSpeedModifier,
                        MiscItemId.PreHistoricalPenaltyLimit,
                        MiscItemId.PostHistoricalBonusLimit,
                        MiscItemId.MaxActiveTechTeamsAoD,
                        MiscItemId.RequiredIcEachTechTeamAoD,
                        MiscItemId.MaximumRandomModifier,
                        MiscItemId.UseNewTechnologyPageLayout,
                        MiscItemId.TechOverviewPanelStyle,
                        MiscItemId.MaxActiveTechTeamsDh,
                        MiscItemId.MinActiveTechTeams,
                        MiscItemId.RequiredIcEachTechTeamDh,
                        MiscItemId.NewCountryRocketryComponent,
                        MiscItemId.NewCountryNuclearPhysicsComponent,
                        MiscItemId.NewCountryNuclearEngineeringComponent,
                        MiscItemId.NewCountrySecretTechs,
                        MiscItemId.MaxTechTeamSkill,
                        MiscItemId.ResearchEnd
                    },
                new[]
                    {
                        MiscItemId.DaysTradeOffers,
                        MiscItemId.DelayGameStartNewTrades,
                        MiscItemId.LimitAiNewTradesGameStart,
                        MiscItemId.DesiredOilStockpile,
                        MiscItemId.CriticalOilStockpile,
                        MiscItemId.DesiredSuppliesStockpile,
                        MiscItemId.CriticalSuppliesStockpile,
                        MiscItemId.DesiredResourcesStockpile,
                        MiscItemId.CriticalResourceStockpile,
                        MiscItemId.WartimeDesiredStockpileMultiplier,
                        MiscItemId.PeacetimeExtraOilImport,
                        MiscItemId.WartimeExtraOilImport,
                        MiscItemId.ExtraImportBelowDesired,
                        MiscItemId.PercentageProducedSupplies,
                        MiscItemId.PercentageProducedMoney,
                        MiscItemId.ExtraImportStockpileSelected,
                        MiscItemId.DaysDeliverResourcesTrades,
                        MiscItemId.MergeTradeDeals,
                        MiscItemId.ManualTradeDeals,
                        MiscItemId.PuppetsSendSuppliesMoney,
                        MiscItemId.PuppetsCriticalSupplyStockpile,
                        MiscItemId.PuppetsMaxPoolResources,
                        MiscItemId.NewTradeDealsMinEffectiveness,
                        MiscItemId.CancelTradeDealsEffectiveness,
                        MiscItemId.AutoTradeAiTradeDeals,
                        MiscItemId.TradeEnd
                    },
                new[]
                    {
                        MiscItemId.OverproduceSuppliesBelowDesired,
                        MiscItemId.MultiplierOverproduceSuppliesWar,
                        MiscItemId.NotProduceSuppliesStockpileOver,
                        MiscItemId.MaxSerialLineProductionGarrisonMilitia,
                        MiscItemId.MinIcSerialProductionNavalAir,
                        MiscItemId.NotProduceNewUnitsManpowerRatio,
                        MiscItemId.NotProduceNewUnitsManpowerValue,
                        MiscItemId.NotProduceNewUnitsSupply,
                        MiscItemId.MilitaryStrengthTotalIcRatioPeacetime,
                        MiscItemId.MilitaryStrengthTotalIcRatioWartime,
                        MiscItemId.MilitaryStrengthTotalIcRatioMajor,
                        MiscItemId.NotUseOffensiveSupplyStockpile,
                        MiscItemId.NotUseOffensiveOilStockpile,
                        MiscItemId.NotUseOffensiveEse,
                        MiscItemId.NotUseOffensiveOrgStrDamage,
                        MiscItemId.AiPeacetimeSpyMissionsDh,
                        MiscItemId.AiSpyMissionsCostModifierDh,
                        MiscItemId.AiDiplomacyCostModifierDh,
                        MiscItemId.AiInfluenceModifierDh,
                        MiscItemId.NewDowRules,
                        MiscItemId.NewDowRules2,
                        MiscItemId.ForcePuppetsJoinMastersAllianceNeutrality,
                        MiscItemId.NewAiReleaseRules,
                        MiscItemId.AiEventsActionSelectionRules,
                        MiscItemId.ForceStrategicRedeploymentHour,
                        MiscItemId.MaxRedeploymentDaysAi,
                        MiscItemId.UseQuickAreaCheckGarrisonAi,
                        MiscItemId.AiMastersGetProvincesConquredPuppets,
                        MiscItemId.MinDaysRequiredAiReleaseCountry,
                        MiscItemId.MinDaysRequiredAiAllied,
                        MiscItemId.MinDaysRequiredAiAlliedSupplyBase,
                        MiscItemId.MinRequiredRelationsAlliedClaimed,
                        MiscItemId.AiEnd
                    },
                new[]
                    {
                        MiscItemId.AiSpyDiplomaticMissionLogger,
                        MiscItemId.CountryLogger,
                        MiscItemId.SwitchedAiFilesLogger,
                        MiscItemId.UseNewAutoSaveFileFormat,
                        MiscItemId.LoadNewAiSwitchingAllClients,
                        MiscItemId.TradeEfficiencyCalculationSystem,
                        MiscItemId.MergeRelocateProvincialDepots,
                        MiscItemId.InGameLossesLogging,
                        MiscItemId.InGameLossLogging2,
                        MiscItemId.AllowBrigadeAttachingInSupply,
                        MiscItemId.MultipleDeploymentSizeArmies,
                        MiscItemId.MultipleDeploymentSizeFleets,
                        MiscItemId.MultipleDeploymentSizeAir,
                        MiscItemId.AllowUniquePicturesAllLandProvinces,
                        MiscItemId.AutoReplyEvents,
                        MiscItemId.ForceActionsShow,
                        MiscItemId.EnableDicisionsPlayers,
                        MiscItemId.RebelsArmyComposition,
                        MiscItemId.RebelsArmyTechLevel,
                        MiscItemId.RebelsArmyMinStr,
                        MiscItemId.RebelsArmyMaxStr,
                        MiscItemId.RebelsOrgRegain,
                        MiscItemId.ExtraRebelBonusNeighboringProvince,
                        MiscItemId.ExtraRebelBonusOccupied,
                        MiscItemId.ExtraRebelBonusMountain,
                        MiscItemId.ExtraRebelBonusHill,
                        MiscItemId.ExtraRebelBonusForest,
                        MiscItemId.ExtraRebelBonusJungle,
                        MiscItemId.ExtraRebelBonusSwamp,
                        MiscItemId.ExtraRebelBonusDesert,
                        MiscItemId.ExtraRebelBonusPlain,
                        MiscItemId.ExtraRebelBonusUrban,
                        MiscItemId.ExtraRebelBonusAirNavalBases,
                        MiscItemId.ReturnRebelliousProvince,
                        MiscItemId.UseNewMinisterFilesFormat,
                        MiscItemId.EnableRetirementYearMinisters,
                        MiscItemId.EnableRetirementYearLeaders,
                        MiscItemId.LoadSpritesModdirOnly,
                        MiscItemId.LoadUnitIconsModdirOnly,
                        MiscItemId.LoadUnitPicturesModdirOnly,
                        MiscItemId.LoadAiFilesModdirOnly,
                        MiscItemId.UseSpeedSetGarrisonStatus,
                        MiscItemId.UseOldSaveGameFormat,
                        MiscItemId.ProductionPanelUiStyle,
                        MiscItemId.UnitPicturesSize,
                        MiscItemId.EnablePicturesNavalBrigades,
                        MiscItemId.BuildingsBuildableOnlyProvinces,
                        MiscItemId.UnitModifiersStatisticsPages,
                        MiscItemId.ModEnd
                    },
                new[]
                    {
                        MiscItemId.MapNumber,
                        MiscItemId.TotalProvinces,
                        MiscItemId.DistanceCalculationModel,
                        MiscItemId.MapWidth,
                        MiscItemId.MapHeight,
                        MiscItemId.MapEnd
                    }
            };

        /// <summary>
        ///     ゲームごとのセクションの有無
        /// </summary>
        public static bool[,] SectionTable =
            {
                {true, true, true, true, true, true, true}, // economy
                {false, false, false, false, false, true, true}, // intelligence
                {false, false, false, false, false, true, true}, // diplomacy
                {true, true, true, true, true, true, true}, // combat
                {false, false, false, false, false, true, true}, // mission
                {false, false, false, false, false, true, true}, // country
                {true, true, true, true, true, true, true}, // research
                {false, false, false, false, false, true, true}, // trade
                {false, false, false, false, false, true, true}, // ai
                {false, false, false, false, false, true, true}, // mod
                {false, false, false, false, false, true, true} // map
            };

        /// <summary>
        ///     ゲームごとの項目の有無
        /// </summary>
        public static bool[,] ItemTable =
            {
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, true, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, false, false, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, false, false},
                {false, false, false, false, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, false, false, false, true, true},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {true, true, true, true, true, true, false},
                {true, true, true, true, true, true, false},
                {true, true, true, true, true, true, false},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, false, false},
                {true, true, false, false, false, false, false},
                {true, true, false, false, false, false, false},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, false, false},
                {false, true, false, false, false, true, true},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {true, true, true, true, true, true, true},
                {true, true, true, true, true, true, true},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, true, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, true, true, false, false},
                {false, false, false, false, true, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {true, true, true, true, true, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, false},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, false},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, false, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true},
                {false, false, false, false, false, true, true}
            };

        /// <summary>
        ///     項目の型
        /// </summary>
        public static MiscItemType[] ItemTypes =
            {
                MiscItemType.NonNegDbl2AoD,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDblMinusOne,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2Dh103Full2,
                MiscItemType.NonNegDbl2Dh103Full2,
                MiscItemType.NonNegDbl2Dh103Full2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.RangedInt,
                MiscItemType.NonNegDbl4Dda13,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.RangedInt,
                MiscItemType.RangedInt,
                MiscItemType.Enum,
                MiscItemType.RangedDbl,
                MiscItemType.RangedInt,
                MiscItemType.Enum,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDblMinusOne,
                MiscItemType.NonNegDblMinusOne,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Enum,
                MiscItemType.NonNegDblMinusOne1,
                MiscItemType.None,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.RangedInt,
                MiscItemType.RangedInt,
                MiscItemType.Int,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.RangedInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.Enum,
                MiscItemType.NonNegDbl,
                MiscItemType.None,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedInt,
                MiscItemType.RangedIntMinusOne,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.None,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonNegDbl2AoD,
                MiscItemType.NonNegDbl2AoD,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl2Dh103Full,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.Dbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonPosDbl,
                MiscItemType.NonPosDbl2,
                MiscItemType.NonPosDbl,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.PosInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2Dh103Full,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDblMinusOne,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosInt,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Enum,
                MiscItemType.RangedDbl0,
                MiscItemType.NonNegDbl0,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.Enum,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.RangedDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Enum,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.RangedDbl,
                MiscItemType.Enum,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedInt,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt1,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.None,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Bool,
                MiscItemType.None,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.None,
                MiscItemType.NonNegDbl,
                MiscItemType.NonPosDbl5AoD,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl5,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedInt,
                MiscItemType.PosInt,
                MiscItemType.NonNegDbl0,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.PosInt,
                MiscItemType.None,
                MiscItemType.PosInt,
                MiscItemType.RangedPosInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl2,
                MiscItemType.NonNegDbl2Dh103Full1,
                MiscItemType.NonNegDbl2Dh103Full1,
                MiscItemType.NonNegDbl2,
                MiscItemType.PosInt,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.NonNegInt,
                MiscItemType.PosDbl,
                MiscItemType.NonNegIntNegDbl,
                MiscItemType.RangedInt,
                MiscItemType.RangedInt,
                MiscItemType.RangedInt,
                MiscItemType.None,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedDbl,
                MiscItemType.RangedDblMinusOne1,
                MiscItemType.RangedDblMinusOne1,
                MiscItemType.PosDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.RangedDbl,
                MiscItemType.Enum,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegDbl,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.NonNegInt,
                MiscItemType.Bool,
                MiscItemType.RangedIntMinusThree,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.NonNegInt,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.RangedDbl,
                MiscItemType.None,
                MiscItemType.Bool,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Int,
                MiscItemType.NonNegInt,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.RangedInt,
                MiscItemType.NonNegIntMinusOne,
                MiscItemType.RangedInt,
                MiscItemType.RangedInt,
                MiscItemType.NonNegDbl,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.NonNegInt,
                MiscItemType.PosInt,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.Enum,
                MiscItemType.Bool,
                MiscItemType.Enum,
                MiscItemType.PosInt,
                MiscItemType.None,
                MiscItemType.NonNegInt,
                MiscItemType.RangedInt,
                MiscItemType.Enum,
                MiscItemType.PosInt,
                MiscItemType.PosInt,
                MiscItemType.None
            };

        /// <summary>
        ///     編集項目の最小値
        /// </summary>
        public static double[] ItemMinValues =
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                -10,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                -10,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0,
                0.05,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0.05,
                0,
                0,
                0,
                0.05,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                1,
                0,
                1,
                2,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                -200,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                1,
                0,
                1,
                1,
                0
            };

        /// <summary>
        ///     編集項目の最大値
        /// </summary>
        public static double[] ItemMaxValues =
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                100,
                0,
                0,
                0,
                1,
                0,
                0,
                100,
                400,
                3,
                1,
                10,
                2,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                2,
                2,
                2,
                1,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                0,
                1,
                0,
                2,
                0,
                0,
                0,
                0,
                100,
                400,
                0,
                1,
                0,
                0,
                10,
                0,
                0,
                0,
                0,
                0,
                3,
                0,
                0,
                0,
                0,
                0,
                1,
                400,
                100,
                1,
                1,
                1,
                0,
                0,
                1,
                3,
                1,
                3,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                5,
                0,
                0,
                2,
                2,
                1,
                24,
                24,
                24,
                0,
                0,
                0,
                2,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                1,
                2,
                0,
                0,
                0,
                100,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                1,
                10,
                0,
                1,
                10,
                0,
                1,
                10,
                0,
                0,
                1,
                10,
                0,
                1,
                10,
                0,
                1,
                10,
                0,
                1,
                10,
                0,
                1,
                10,
                1,
                10,
                0,
                0,
                1,
                10,
                0,
                0,
                1,
                10,
                0,
                0,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                0,
                0,
                1,
                10,
                0,
                0,
                1,
                10,
                1,
                10,
                1,
                10,
                0,
                1,
                10,
                1,
                10,
                0,
                0,
                1,
                10,
                1,
                10,
                1,
                10,
                1,
                10,
                0,
                0,
                1,
                10,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                20,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                100,
                100,
                100,
                0,
                0,
                0,
                0,
                99,
                0,
                0,
                0,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                1,
                2,
                0,
                0,
                0,
                1,
                2,
                0,
                1,
                100,
                0,
                0,
                1,
                1,
                0,
                0,
                0,
                200,
                0,
                1,
                0,
                1,
                1,
                1,
                0,
                0,
                1,
                4,
                1,
                0,
                0,
                0,
                1,
                1,
                2,
                1,
                100,
                0,
                100,
                100,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                2,
                0,
                0,
                0,
                10000,
                1,
                0,
                0,
                0
            };

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     miscファイルの再読み込みを要求する
        /// </summary>
        /// <remarks>
        ///     ゲームフォルダ、MOD名、ゲーム種類の変更があった場合に呼び出す
        /// </remarks>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     miscファイルを読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // 設定値を初期化する
            _items = new object[Enum.GetValues(typeof (MiscItemId)).Length];

            // コメントを初期化する
            _comments = new string[Enum.GetValues(typeof (MiscItemId)).Length];

            // miscファイルを解釈する
            if (!MiscParser.Parse(Game.GetReadFileName(Game.MiscPathName)))
            {
                return;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     miscファイルを保存する
        /// </summary>
        public static void Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return;
            }

            // miscファイルを保存する
            MiscWriter.Write(Game.GetWriteFileName(Game.MiscPathName));

            // 編集済みフラグを全て解除する
            ResetDirtyAll();
        }

        #endregion

        #region 設定項目操作

        /// <summary>
        ///     項目の値を取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>項目の値</returns>
        public static object GetItem(MiscItemId id)
        {
            return _items[(int) id];
        }

        /// <summary>
        ///     項目の値を設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <param name="o">項目の値</param>
        public static void SetItem(MiscItemId id, object o)
        {
            _items[(int) id] = o;
        }

        public static string GetComment(MiscItemId id)
        {
            return _comments[(int) id];
        }

        /// <summary>
        ///     項目のコメントを追加する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <param name="s">追加する文字列</param>
        public static void AppendComment(MiscItemId id, string s)
        {
            _comments[(int) id] += s;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(MiscItemId id)
        {
            return DirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public static void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public static void SetDirty(MiscItemId id)
        {
            DirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public static void ResetDirtyAll()
        {
            foreach (MiscItemId id in Enum.GetValues(typeof (MiscItemId)))
            {
                DirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     項目の文字列を取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>文字列</returns>
        /// <remarks>Bool/Enumの項目は整数で表現する</remarks>
        public static string GetString(MiscItemId id)
        {
            switch (ItemTypes[(int) id])
            {
                case MiscItemType.Bool:
                    return (bool) GetItem(id) ? "1" : "0";

                case MiscItemType.Enum:
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    return IntHelper.ToString0((int) GetItem(id));

                case MiscItemType.NonNegInt1:
                    return IntHelper.ToString1((int) GetItem(id));

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne1:
                    return DoubleHelper.ToString1((double) GetItem(id));

                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.RangedDbl0:
                    return DoubleHelper.ToString0((double) GetItem(id));

                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonPosDbl2:
                    return DoubleHelper.ToString2((double) GetItem(id));

                case MiscItemType.NonNegDbl5:
                    return DoubleHelper.ToString5((double) GetItem(id));

                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.RangedDblMinusOne:
                    return GetDbl1MinusOneString((double) GetItem(id));

                case MiscItemType.NonNegDbl2AoD:
                    return GetDbl1AoD2String((double) GetItem(id));

                case MiscItemType.NonNegDbl4Dda13:
                    return GetDbl1Dda134String((double) GetItem(id));

                case MiscItemType.NonNegDbl2Dh103Full:
                    return GetDbl1Range2String((double) GetItem(id), 0, 0.1000005);

                case MiscItemType.NonNegDbl2Dh103Full1:
                    return GetDbl2Range1String((double) GetItem(id), 0, 0.2000005);

                case MiscItemType.NonNegDbl2Dh103Full2:
                    return GetDbl1Range2String((double) GetItem(id), 0, 1);

                case MiscItemType.NonPosDbl5AoD:
                    return GetDbl1AoD5String((double) GetItem(id));

                case MiscItemType.NonPosDbl2Dh103Full:
                    return GetDbl1Range2String((double) GetItem(id), -0.1000005, 0);

                case MiscItemType.NonNegIntNegDbl:
                    return GetNonNegIntNegDblString((double) GetItem(id));
            }

            return string.Empty;
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下1桁 or -1)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        private static string GetDbl1MinusOneString(double val)
        {
            return Math.Abs(val - (-1)) < 0.0000005 ? "-1" : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下1桁/DDA1.3 or DHのみ小数点以下4桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        private static string GetDbl1Dda134String(double val)
        {
            return ((Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130) || Game.Type == GameType.DarkestHour)
                       ? DoubleHelper.ToString4(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下1桁/AoDのみ小数点以下2桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        private static string GetDbl1AoD2String(double val)
        {
            return (Game.Type == GameType.ArsenalOfDemocracy)
                       ? DoubleHelper.ToString2(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下1桁/AoDのみ小数点以下5桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        private static string GetDbl1AoD5String(double val)
        {
            return (Game.Type == GameType.ArsenalOfDemocracy)
                       ? DoubleHelper.ToString5(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下1桁/指定範囲内ならば小数点以下2桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <param name="min">範囲内の最小値</param>
        /// <param name="max">範囲内の最大値</param>
        /// <returns>文字列</returns>
        private static string GetDbl1Range2String(double val, double min, double max)
        {
            return (val > min && val < max) ? DoubleHelper.ToString2(val) : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (実数/小数点以下2桁/指定範囲外ならば小数点以下1桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        /// <param name="min">範囲内の最小値</param>
        /// <param name="max">範囲内の最大値</param>
        private static string GetDbl2Range1String(double val, double min, double max)
        {
            return (val > min && val < max) ? DoubleHelper.ToString2(val) : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     文字列を取得する (非負の整数 or 負の実数)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>文字列</returns>
        private static string GetNonNegIntNegDblString(double val)
        {
            return (val < 0) ? DoubleHelper.ToString1(val) : IntHelper.ToString0((int) val);
        }

        #endregion

        #region ゲームバージョン

        /// <summary>
        ///     miscファイルの種類を取得する
        /// </summary>
        /// <returns></returns>
        public static MiscGameType GetGameType()
        {
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    return (Game.Version >= 130) ? MiscGameType.Dda13 : MiscGameType.Dda12;

                case GameType.ArsenalOfDemocracy:
                    return (Game.Version >= 108)
                               ? MiscGameType.Aod108
                               : ((Game.Version <= 104) ? MiscGameType.Aod104 : MiscGameType.Aod107);

                case GameType.DarkestHour:
                    return (Game.Version >= 103) ? MiscGameType.Dh103 : MiscGameType.Dh102;
            }
            return MiscGameType.Dda12;
        }

        #endregion
    }

    /// <summary>
    ///     misc項目ID
    /// </summary>
    public enum MiscItemId
    {
        // economy
        IcToTcRatio, // ICからTCへの変換効率
        IcToSuppliesRatio, // ICから物資への変換効率
        IcToConsumerGoodsRatio, // ICから消費財への変換効率
        IcToMoneyRatio, // ICから資金への変換効率
        DissentChangeSpeed, // 不満度変化速度
        MinAvailableIc, // 最小実効ICの比率
        MinFinalIc, // 最小実効IC
        DissentReduction, // 不満度低下補正
        MaxGearingBonus, // 最大ギアリングボーナス
        GearingBonusIncrement, // ギアリングボーナスの増加値
        GearingResourceIncrement, // 連続生産時の資源消費増加
        GearingLossNoIc, // IC不足時のギアリングボーナス減少値
        IcMultiplierNonNational, // 非中核州のIC補正
        IcMultiplierNonOwned, // 占領地のIC補正
        IcMultiplierPuppet, // 属国のIC補正
        ResourceMultiplierNonNational, // 非中核州の資源補正
        ResourceMultiplierNonOwned, // 占領地の資源補正
        ResourceMultiplierNonNationalAi, // 非中核州の資源補正(AI)
        ResourceMultiplierPuppet, // 属国の資源補正
        TcLoadUndeployedDivision, // 未配備師団のTC負荷
        TcLoadOccupied, // 占領地のTC負荷
        TcLoadMultiplierLand, // 陸軍師団のTC負荷補正
        TcLoadMultiplierAir, // 空軍師団のTC負荷補正
        TcLoadMultiplierNaval, // 海軍師団のTC負荷補正
        TcLoadPartisan, // パルチザンのTC負荷
        TcLoadFactorOffensive, // 攻勢時のTC負荷係数
        TcLoadProvinceDevelopment, // プロヴィンス開発のTC負荷
        TcLoadBase, // 未配備の基地のTC負荷
        ManpowerMultiplierNational, // 中核州の人的資源補正
        ManpowerMultiplierNonNational, // 非中核州の人的資源補正
        ManpowerMultiplierColony, // 海外州の人的資源補正
        ManpowerMultiplierPuppet, // 属国の人的資源補正
        ManpowerMultiplierWartimeOversea, // 戦時の海外州の人的資源補正
        ManpowerMultiplierPeacetime, // 平時の人的資源補正
        ManpowerMultiplierWartime, // 戦時の人的資源補正
        DailyRetiredManpower, // 人的資源の老化率
        RequirementAffectSlider, // 政策スライダーに影響を与えるためのIC比率
        TrickleBackFactorManpower, // 戦闘による損失からの復帰係数
        ReinforceManpower, // 補充に必要な人的資源の比率
        ReinforceCost, // 補充に必要なICの比率
        ReinforceTime, // 補充に必要な時間の比率
        UpgradeCost, // 改良に必要なICの比率
        UpgradeTime, // 改良に必要な時間の比率
        ReinforceToUpdateModifier, // 改良のための補充係数
        NationalismStartingValue, // ナショナリズムの初期値
        NationalismPerManpowerAoD, // 人的資源によるナショナリズムの補正値
        NationalismPerManpowerDh, // 人的資源によるナショナリズムの補正値
        MaxNationalism, // ナショナリズム最大値
        MaxRevoltRisk, // 最大反乱率
        MonthlyNationalismReduction, // 月ごとのナショナリズムの減少値
        SendDivisionDays, // 師団譲渡後配備可能になるまでの時間
        TcLoadUndeployedBrigade, // 未配備旅団のTC負荷
        CanUnitSendNonAllied, // 非同盟国に師団を売却
        SpyMissionDays, // 諜報任務の間隔
        IncreateIntelligenceLevelDays, // 諜報レベルの増加間隔
        ChanceDetectSpyMission, // 国内の諜報活動を発見する確率
        RelationshipsHitDetectedMissions, // 諜報任務発覚時の友好度低下量
        ShowThirdCountrySpyReports, // 第三国の諜報活動を報告するか
        DistanceModifierNeighbours, // 諜報任務の近隣国補正
        SpyInformationAccuracyModifier, // 情報の正確さ補正
        AiPeacetimeSpyMissions, // AIの平時の攻撃的諜報活動
        MaxIcCostModifier, // 諜報コスト補正の最大IC
        AiSpyMissionsCostModifier, // AIの諜報コスト補正
        AiDiplomacyCostModifier, // AIの外交コスト補正
        AiInfluenceModifier, // AIの外交影響度補正
        CostRepairBuildings, // 建物修復コスト補正
        TimeRepairBuilding, // 建物修復時間補正
        ProvinceEfficiencyRiseTime, // プロヴィンス効率上昇時間
        CoreProvinceEfficiencyRiseTime, // 中核プロヴィンス効率上昇時間
        LineUpkeep, // ライン維持コスト補正
        LineStartupTime, // ライン開始時間
        LineUpgradeTime, // ライン改良時間
        RetoolingCost, // ライン調整コスト補正
        RetoolingResource, // ライン調整資源補正
        DailyAgingManpower, // 人的資源老化補正
        SupplyConvoyHunt, // 船団襲撃時物資使用量補正
        SupplyNavalStaticAoD, // 海軍の待機時物資使用量補正
        SupplyNavalMoving, // 海軍の移動時物資使用量補正
        SupplyNavalBattleAoD, // 海軍の戦闘時物資使用量補正
        SupplyAirStaticAoD, // 空軍の待機時物資使用量補正
        SupplyAirMoving, // 空軍の移動時物資使用量補正
        SupplyAirBattleAoD, // 空軍の戦闘時物資使用量補正
        SupplyAirBombing, // 空軍の爆撃時物資使用量補正
        SupplyLandStaticAoD, // 陸軍の待機時物資使用量補正
        SupplyLandMoving, // 陸軍の移動時物資使用量補正
        SupplyLandBattleAoD, // 陸軍の戦闘時物資使用量補正
        SupplyLandBombing, // 陸軍の砲撃時物資使用量補正
        SupplyStockLand, // 陸軍の物資備蓄量
        SupplyStockAir, // 空軍の物資備蓄量
        SupplyStockNaval, // 海軍の物資備蓄量
        RestockSpeedLand, // 陸軍の物資再備蓄速度
        RestockSpeedAir, // 空軍の物資再備蓄速度
        RestockSpeedNaval, // 海軍の物資再備蓄速度
        SyntheticOilConversionMultiplier, // 合成石油変換係数
        SyntheticRaresConversionMultiplier, // 合成希少資源変換係数
        MilitarySalary, // 軍隊の給料
        MaxIntelligenceExpenditure, // 最大諜報費比率
        MaxResearchExpenditure, // 最大研究費比率
        MilitarySalaryAttrictionModifier, // 軍隊の給料不足時の消耗補正
        MilitarySalaryDissentModifier, // 軍隊の給料不足時の不満度補正
        NuclearSiteUpkeepCost, // 原子炉維持コスト
        NuclearPowerUpkeepCost, // 原子力発電所維持コスト
        SyntheticOilSiteUpkeepCost, // 合成石油工場維持コスト
        SyntheticRaresSiteUpkeepCost, // 合成希少資源工場維持コスト
        DurationDetection, // 海軍情報の存続期間
        ConvoyProvinceHostileTime, // 船団攻撃回避時間
        ConvoyProvinceBlockedTime, // 船団攻撃妨害時間
        AutoTradeConvoy, // 自動貿易に必要な輸送船団割合
        SpyUpkeepCost, // 諜報維持コスト
        SpyDetectionChance, // スパイ発見確率
        SpyCoupDissentModifier, // 不満度によるクーデター成功率修正
        InfraEfficiencyModifier, // インフラによるプロヴィンス効率補正
        ManpowerToConsumerGoods, // 人的資源の消費財生産補正
        TimeBetweenSliderChangesAoD, // スライダー移動の間隔
        MinimalPlacementIc, // 海外プロヴィンスへの配置の必要IC
        NuclearPower, // 原子力発電量
        FreeInfraRepair, // インフラの自然回復率
        MaxSliderDissent, // スライダー移動時の最大不満度
        MinSliderDissent, // スライダー移動時の最小不満度
        MaxDissentSliderMove, // スライダー移動可能な最大不満度
        IcConcentrationBonus, // 工場集中ボーナス
        TransportConversion, // 輸送艦変換係数
        ConvoyDutyConversion, // 輸送船団変換係数
        EscortDutyConversion, // 護衛船団変換係数
        MinisterChangeDelay, // 閣僚変更遅延日数
        MinisterChangeEventDelay, // 閣僚変更遅延日数(イベント)
        IdeaChangeDelay, // 国策変更遅延日数
        IdeaChangeEventDelay, // 国策変更遅延日数(イベント)
        LeaderChangeDelay, // 指揮官変更遅延日数
        ChangeIdeaDissent, // 国策変更時の不満度上昇量
        ChangeMinisterDissent, // 閣僚変更時の不満度上昇量
        MinDissentRevolt, // 反乱が発生する最低不満度
        DissentRevoltMultiplier, // 不満度による反乱軍発生率係数
        TpMaxAttach, // 輸送艦最大付属装備数
        SsMaxAttach, // 潜水艦最大付属装備数
        SsnMaxAttach, // 原子力潜水艦最大付属装備数
        DdMaxAttach, // 駆逐艦最大付属装備数
        ClMaxAttach, // 軽巡洋艦最大付属装備数
        CaMaxAttach, // 重巡洋艦最大付属装備数
        BcMaxAttach, // 巡洋戦艦最大付属装備数
        BbMaxAttach, // 戦艦最大付属装備数
        CvlMaxAttach, // 軽空母最大付属装備数
        CvMaxAttach, // 空母最大付属装備数
        CanChangeIdeas, // プレイヤーの国策変更を許可
        CanUnitSendNonAlliedDh, // 非同盟国に師団を売却
        BluePrintsCanSoldNonAllied, // 非同盟国に青写真を売却
        ProvinceCanSoldNonAllied, // 非同盟国にプロヴィンスを売却
        TransferAlliedCoreProvinces, // 占領中の同盟国の中核州返還
        ProvinceBuildingsRepairModifier, // 建物修復速度補正
        ProvinceResourceRepairModifier, // 資源回復速度補正
        StockpileLimitMultiplierResource, // 資源備蓄上限補正
        StockpileLimitMultiplierSuppliesOil, // 物資/燃料備蓄上限補正
        OverStockpileLimitDailyLoss, // 超過備蓄損失割合
        MaxResourceDepotSize, // 資源備蓄上限値
        MaxSuppliesOilDepotSize, // 物資/燃料備蓄上限値
        DesiredStockPilesSuppliesOil, // 理想物資/燃料備蓄比率
        MaxManpower, // 最大人的資源
        ConvoyTransportsCapacity, // 船団輸送能力
        SuppyLandStaticDh, // 陸軍の待機時物資使用量補正
        SupplyLandBattleDh, // 陸軍の戦闘時物資使用量補正
        FuelLandStatic, // 陸軍の待機時燃料使用量補正
        FuelLandBattle, // 陸軍の戦闘時燃料使用量補正
        SupplyAirStaticDh, // 空軍の待機時物資使用量補正
        SupplyAirBattleDh, // 空軍の戦闘時物資使用量補正
        FuelAirNavalStatic, // 空軍/海軍の待機時燃料使用量補正
        FuelAirBattle, // 空軍の戦闘時燃料使用量補正
        SupplyNavalStaticDh, // 海軍の待機時物資使用量補正
        SupplyNavalBattleDh, // 海軍の戦闘時物資使用量補正
        FuelNavalNotMoving, // 海軍の非移動時燃料使用量補正
        FuelNavalBattle, // 海軍の戦闘時燃料使用量補正
        TpTransportsConversionRatio, // 輸送艦の輸送船団への変換比率
        DdEscortsConversionRatio, // 駆逐艦の護衛船団への変換比率
        ClEscortsConversionRatio, // 軽巡洋艦の護衛船団への変換比率
        CvlEscortsConversionRatio, // 軽空母の護衛船団への変換比率
        ProductionLineEdit, // 生産ラインの編集
        GearingBonusLossUpgradeUnit, // ユニット改良時のギアリングボーナス減少比率
        GearingBonusLossUpgradeBrigade, // 旅団改良時のギアリングボーナス減少比率
        DissentNukes, // 中核州核攻撃時の不満度上昇係数
        MaxDailyDissent, // 物資/消費財不足時の最大不満度上昇値
        NukesProductionModifier, // 核兵器生産補正
        ConvoySystemOptionsAllied, // 同盟国に対する船団システム
        ResourceConvoysBackUnneeded, // 不要な資源/燃料の回収比率
        EconomyEnd, // 経済項目の末尾

        // intelligence
        SpyMissionDaysDh, // 諜報任務の間隔
        IncreateIntelligenceLevelDaysDh, // 諜報レベルの増加間隔
        ChanceDetectSpyMissionDh, // 国内の諜報活動を発見する確率
        RelationshipsHitDetectedMissionsDh, // 諜報任務発覚時の友好度低下量
        DistanceModifier, // 諜報任務の距離補正
        DistanceModifierNeighboursDh, // 諜報任務の近隣国補正
        SpyLevelBonusDistanceModifier, // 諜報レベルの距離補正
        SpyLevelBonusDistanceModifierAboveTen, // 諜報レベル10超過時の距離補正
        SpyInformationAccuracyModifierDh, // 情報の正確さ補正
        IcModifierCost, // 諜報コストのIC補正
        MinIcCostModifier, // 諜報コスト補正の最小IC
        MaxIcCostModifierDh, // 諜報コスト補正の最大IC
        ExtraMaintenanceCostAboveTen, // 諜報レベル10超過時追加維持コスト
        ExtraCostIncreasingAboveTen, // 諜報レベル10超過時増加コスト
        ShowThirdCountrySpyReportsDh, // 第三国の諜報活動を報告するか
        SpiesMoneyModifier, // 諜報資金割り当て補正
        IntelligenceEnd, // 諜報項目の末尾

        // diplomacy
        DaysBetweenDiplomaticMissions, // 外交官派遣間隔
        TimeBetweenSliderChangesDh, // スライダー移動の間隔
        RequirementAffectSliderDh, // 政策スライダーに影響を与えるためのIC比率
        UseMinisterPersonalityReplacing, // 閣僚交代時に閣僚特性を適用する
        RelationshipHitCancelTrade, // 貿易キャンセル時の友好度低下
        RelationshipHitCancelPermanentTrade, // 永久貿易キャンセル次の友好度低下
        PuppetsJoinMastersAlliance, // 属国が宗主国の同盟に強制参加する
        MastersBecomePuppetsPuppets, // 属国の属国が設立できるか
        AllowManualClaimsChange, // 領有権主張の変更
        BelligerenceClaimedProvince, // 領有権主張時の好戦性上昇値
        BelligerenceClaimsRemoval, // 領有権撤回時の好戦性減少値
        JoinAutomaticallyAllesAxis, // 宣戦布告された時に対抗陣営へ自動加盟
        AllowChangeHosHog, // 国家元首/政府首班の交代
        ChangeTagCoup, // クーデター発生時に兄弟国へ変更
        FilterReleaseCountries, // 独立可能国設定
        DiplomacyEnd, // 経済項目の末尾

        // combat
        LandXpGainFactor, // 陸軍経験値入手係数
        NavalXpGainFactor, // 海軍経験値入手係数
        AirXpGainFactor, // 空軍経験値入手係数
        AirDogfightXpGainFactor, // 空軍空戦時経験値入手係数
        DivisionXpGainFactor, // 師団経験値入手係数
        LeaderXpGainFactor, // 指揮官経験値入手係数
        AttritionSeverityModifier, // 消耗係数
        NoSupplyAttritionSeverity, // 無補給時の自然条件消耗係数
        NoSupplyMinimunAttrition, // 無補給時の消耗係数
        BaseProximity, // 基地戦闘補正
        ShoreBombardmentModifier, // 艦砲射撃戦闘補正
        ShoreBombardmentCap, // 艦砲射撃戦闘効率上限
        InvasionModifier, // 強襲上陸ペナルティ
        MultipleCombatModifier, // 側面攻撃ペナルティ
        OffensiveCombinedArmsBonus, // 攻撃側諸兵科連合ボーナス
        DefensiveCombinedArmsBonus, // 防御側諸兵科連合ボーナス
        SurpriseModifier, // 奇襲攻撃ペナルティ
        LandCommandLimitModifier, // 陸軍指揮上限ペナルティ
        AirCommandLimitModifier, // 空軍指揮上限ペナルティ
        NavalCommandLimitModifier, // 海軍指揮上限ペナルティ
        EnvelopmentModifier, // 多方面攻撃補正
        EncircledModifier, // 包囲攻撃ペナルティ
        LandFortMultiplier, // 要塞攻撃ペナルティ
        CoastalFortMultiplier, // 沿岸要塞攻撃ペナルティ
        HardUnitsAttackingUrbanPenalty, // 装甲ユニットの都市攻撃ペナルティ
        DissentMultiplier, // 国民不満度ペナルティ
        SupplyProblemsModifier, // 補給不足ペナルティ
        SupplyProblemsModifierLand, // 陸軍物資不足ペナルティ
        SupplyProblemsModifierAir, // 空軍物資不足ペナルティ
        SupplyProblemsModifierNaval, // 海軍物資不足ペナルティ
        FuelProblemsModifierLand, // 陸軍燃料不足ペナルティ
        FuelProblemsModifierAir, // 空軍燃料不足ペナルティ
        FuelProblemsModifierNaval, // 海軍燃料不足ペナルティ
        RaderStationMultiplier, // レーダー補正
        RaderStationAaMultiplier, // レーダー/対空砲複合補正
        InterceptorBomberModifier, // 爆撃機迎撃ボーナス
        AirOverstackingModifier, // 空軍スタックペナルティ
        AirOverstackingModifierAoD, // 空軍スタックペナルティ
        NavalOverstackingModifier, // 海軍スタックペナルティ
        LandLeaderCommandLimitRank0, // 陸軍元帥指揮上限
        LandLeaderCommandLimitRank1, // 陸軍大将指揮上限
        LandLeaderCommandLimitRank2, // 陸軍中将指揮上限
        LandLeaderCommandLimitRank3, // 陸軍少将指揮上限
        AirLeaderCommandLimitRank0, // 空軍元帥指揮上限
        AirLeaderCommandLimitRank1, // 空軍大将指揮上限
        AirLeaderCommandLimitRank2, // 空軍中将指揮上限
        AirLeaderCommandLimitRank3, // 空軍少将指揮上限
        NavalLeaderCommandLimitRank0, // 海軍元帥指揮上限
        NavalLeaderCommandLimitRank1, // 海軍大将指揮上限
        NavalLeaderCommandLimitRank2, // 海軍中将指揮上限
        NavalLeaderCommandLimitRank3, // 海軍少将指揮上限
        HqCommandLimitFactor, // 司令部指揮上限係数
        ConvoyProtectionFactor, // 輸送船団護衛係数
        ConvoyEscortsModel, // 輸送船団護衛モデル
        DelayAfterCombatEnds, // 戦闘後命令遅延時間
        LandDelayBeforeOrders, // 陸軍命令遅延時間
        NavalDelayBeforeOrders, // 海軍命令遅延時間
        AirDelayBeforeOrders, // 空軍命令遅延時間
        MaximumSizesAirStacks, // 空軍最大スタックサイズ
        DurationAirToAirBattles, // 空戦最小戦闘時間
        DurationNavalPortBombing, // 港湾攻撃最小戦闘時間
        DurationStrategicBombing, // 戦略爆撃最小戦闘時間
        DurationGroundAttackBombing, // 地上爆撃最小戦闘時間
        EffectExperienceCombat, // 経験値補正
        DamageNavalBasesBombing, // 海軍基地戦略爆撃係数
        DamageAirBaseBombing, // 空軍基地戦略爆撃係数
        DamageAaBombing, // 対空砲戦略爆撃係数
        DamageRocketBombing, // ロケット試験場戦略爆撃係数
        DamageNukeBombing, // 原子炉戦略爆撃係数
        DamageRadarBombing, // レーダー戦略爆撃係数
        DamageInfraBombing, // インフラ戦略爆撃係数
        DamageIcBombing, // IC戦略爆撃係数
        DamageResourcesBombing, // 資源戦略爆撃係数
        DamageSyntheticOilBombing, // 合成石油工場戦略爆撃係数
        HowEffectiveGroundDef, // 対地防御効率補正
        ChanceAvoidDefencesLeft, // 基本回避率(防御回数あり)
        ChanceAvoidNoDefences, // 基本回避率(防御回数なし)
        LandChanceAvoidDefencesLeft, // 陸軍基本回避率(防御回数あり)
        AirChanceAvoidDefencesLeft, // 空軍基本回避率(防御回数あり)
        NavalChanceAvoidDefencesLeft, // 海軍基本回避率(防御回数あり)
        LandChanceAvoidNoDefences, // 陸軍基本回避率(防御回数あり)
        AirChanceAvoidNoDefences, // 空軍基本回避率(防御回数あり)
        NavalChanceAvoidNoDefences, // 海軍基本回避率(防御回数あり)
        ChanceGetTerrainTrait, // 地形特性獲得可能性
        ChanceGetEventTrait, // 戦闘特性獲得可能性
        BonusTerrainTrait, // 地形特性補正
        BonusSimilarTerrainTrait, // 類似地形特性補正
        BonusEventTrait, // 戦闘特性補正
        BonusLeaderSkillPointLand, // 陸軍指揮官スキル補正
        BonusLeaderSkillPointAir, // 空軍指揮官スキル補正
        BonusLeaderSkillPointNaval, // 海軍指揮官スキル補正
        ChanceLeaderDying, // 指揮官死亡確率
        AirOrgDamage, // 空軍組織率被ダメージ
        AirStrDamageOrg, // 空軍戦力被ダメージ(組織力)
        AirStrDamage, // 空軍戦力被ダメージ
        LandMinOrgDamage, // 陸軍最小組織率被ダメージ
        LandOrgDamageHardSoftEach, // 陸軍組織率被ダメージ(装甲/非装甲同士)
        LandOrgDamageHardVsSoft, // 陸軍組織率被ダメージ(装甲対非装甲)
        LandMinStrDamage, // 陸軍最小戦力被ダメージ
        LandStrDamageHardSoftEach, // 陸軍戦力被ダメージ(装甲/非装甲同士)
        LandStrDamageHardVsSoft, // 陸軍戦力被ダメージ(装甲対非装甲)
        AirMinOrgDamage, // 空軍最小組織率被ダメージ
        AirAdditionalOrgDamage, // 空軍追加組織率被ダメージ
        AirMinStrDamage, // 空軍最小戦力被ダメージ
        AirAdditionalStrDamage, // 空軍追加戦力被ダメージ
        AirStrDamageEntrenced, // 空軍戦力被ダメージ(対塹壕)
        NavalMinOrgDamage, // 海軍最小組織率被ダメージ
        NavalAdditionalOrgDamage, // 海軍追加組織率被ダメージ
        NavalMinStrDamage, // 海軍最小戦力被ダメージ
        NavalAdditionalStrDamage, // 海軍追加戦力被ダメージ
        AirStrDamageLandOrg, // 空軍対陸軍戦力被ダメージ(組織率)
        AirOrgDamageLandDh, // 空軍対陸軍組織率被ダメージ
        AirStrDamageLandDh, // 空軍対陸軍戦力被ダメージ
        LandOrgDamageLandOrg, // 陸軍対陸軍組織率被ダメージ(組織率)
        LandOrgDamageLandUrban, // 陸軍対陸軍組織率被ダメージ(都市)
        LandOrgDamageLandFort, // 陸軍対陸軍組織率被ダメージ(要塞)
        RequiredLandFortSize, // 必要要塞規模
        LandStrDamageLandDh, // 陸軍対陸軍戦力被ダメージ
        AirOrgDamageAirDh, // 空軍対空軍組織率被ダメージ
        AirStrDamageAirDh, // 空軍対空軍戦力被ダメージ
        LandOrgDamageAirDh, // 陸軍対空軍組織率被ダメージ
        LandStrDamageAirDh, // 陸軍対空軍戦力被ダメージ
        NavalOrgDamageAirDh, // 海軍対空軍組織率被ダメージ
        NavalStrDamageAirDh, // 海軍対空軍戦力被ダメージ
        SubsOrgDamageAir, // 潜水艦対空軍組織率被ダメージ
        SubsStrDamageAir, // 潜水艦対空軍戦力被ダメージ
        AirOrgDamageNavyDh, // 空軍対海軍組織率被ダメージ
        AirStrDamageNavyDh, // 空軍対海軍戦力被ダメージ
        NavalOrgDamageNavyDh, // 海軍対海軍組織率被ダメージ
        NavalStrDamageNavyDh, // 海軍対海軍戦力被ダメージ
        SubsOrgDamageNavy, // 潜水艦対海軍組織率被ダメージ
        SubsStrDamageNavy, // 潜水艦対海軍戦力被ダメージ
        SubsOrgDamage, // 潜水艦組織率被ダメージ
        SubsStrDamage, // 潜水艦戦力被ダメージ
        SubStacksDetectionModifier, // 潜水艦発見補正
        AirOrgDamageLandAoD, // 空軍対陸軍組織率被ダメージ
        AirStrDamageLandAoD, // 空軍対陸軍戦力被ダメージ
        LandDamageArtilleryBombardment, // 砲撃ダメージ補正(陸上部隊)
        InfraDamageArtilleryBombardment, // 砲撃ダメージ補正(インフラ)
        IcDamageArtilleryBombardment, // 砲撃ダメージ補正(IC)
        ResourcesDamageArtilleryBombardment, // 砲撃ダメージ補正(資源)
        PenaltyArtilleryBombardment, // 砲撃中の被攻撃ペナルティ
        ArtilleryStrDamage, // 砲撃戦力ダメージ
        ArtilleryOrgDamage, // 砲撃組織率ダメージ
        LandStrDamageLandAoD, // 陸軍対陸軍戦力被ダメージ
        LandOrgDamageLand, // 陸軍対陸軍組織率被ダメージ
        LandStrDamageAirAoD, // 陸軍対空軍戦力被ダメージ
        LandOrgDamageAirAoD, // 陸軍対空軍組織率被ダメージ
        NavalStrDamageAirAoD, // 海軍対空軍戦力被ダメージ
        NavalOrgDamageAirAoD, // 海軍対空軍組織率被ダメージ
        AirStrDamageAirAoD, // 空軍対空軍戦力被ダメージ
        AirOrgDamageAirAoD, // 空軍対空軍組織率被ダメージ
        NavalStrDamageNavyAoD, // 海軍対海軍戦力被ダメージ
        NavalOrgDamageNavyAoD, // 海軍対海軍組織率被ダメージ
        AirStrDamageNavyAoD, // 空軍対海軍戦力被ダメージ
        AirOrgDamageNavyAoD, // 空軍対海軍組織率被ダメージ
        MilitaryExpenseAttritionModifier, // 給料不足時の戦闘補正
        NavalMinCombatTime, // 海軍最小戦闘時間
        LandMinCombatTime, // 陸軍最小戦闘時間
        AirMinCombatTime, // 空軍最小戦闘時間
        LandOverstackingModifier, // 陸軍スタックペナルティ
        LandOrgLossMoving, // 陸軍移動時組織率減少係数
        AirOrgLossMoving, // 空軍移動時組織率減少係数
        NavalOrgLossMoving, // 海軍移動時組織率減少係数
        SupplyDistanceSeverity, // 遠隔地補給係数
        SupplyBase, // 基礎補給効率
        LandOrgGain, // 陸軍組織率補正
        AirOrgGain, // 空軍組織率補正
        NavalOrgGain, // 海軍組織率補正
        NukeManpowerDissent, // 核攻撃不満度係数(人的資源)
        NukeIcDissent, // 核攻撃不満度係数(IC)
        NukeTotalDissent, // 核攻撃不満度係数(トータル)
        LandFriendlyOrgGain, // 陸軍友好地組織率補正
        AirLandStockModifier, // 阻止攻撃備蓄補正
        ScorchDamage, // 焦土命令ダメージ
        StandGroundDissent, // 死守命令不満度上昇
        ScorchGroundBelligerence, // 焦土命令好戦性上昇
        DefaultLandStack, // 陸軍デフォルトスタック数
        DefaultNavalStack, // 海軍デフォルトスタック数
        DefaultAirStack, // 空軍デフォルトスタック数
        DefaultRocketStack, // ロケットデフォルトスタック数
        FortDamageArtilleryBombardment, // 要塞砲撃ダメージ補正
        ArtilleryBombardmentOrgCost, // 砲撃組織率減少
        LandDamageFort, // 陸軍対要塞ダメージ係数
        AirRebaseFactor, // 空軍基地移動組織率減少係数
        AirMaxDisorganized, // 空港占領時ペナルティ
        AaInflictedStrDamage, // 対空砲戦力ダメージ補正
        AaInflictedOrgDamage, // 対空砲組織率ダメージ補正
        AaInflictedFlyingDamage, // 対空砲上空通過ダメージ補正
        AaInflictedBombingDamage, // 対空砲爆撃中ダメージ補正
        HardAttackStrDamage, // 装甲ユニット戦力ダメージ補正
        HardAttackOrgDamage, // 装甲ユニット組織率ダメージ補正
        ArmorSoftBreakthroughMin, // 戦車対人最小突破係数
        ArmorSoftBreakthroughMax, // 戦車対人最大突破係数
        NavalCriticalHitChance, // 海軍クリティカルヒット確率
        NavalCriticalHitEffect, // 海軍クリティカルヒット効果
        LandFortDamage, // 要塞ダメージ補正
        PortAttackSurpriseChanceDay, // 日中港湾攻撃奇襲確率
        PortAttackSurpriseChanceNight, // 夜間港湾攻撃奇襲確率
        PortAttackSurpriseModifier, // 港湾攻撃奇襲補正
        RadarAntiSurpriseChance, // レーダー奇襲確率減少値
        RadarAntiSurpriseModifier, // レーダー奇襲効果減少値
        CounterAttackStrDefenderAoD, // 反撃イベント防御側戦力補正
        CounterAttackOrgDefenderAoD, // 反撃イベント防御側組織率補正
        CounterAttackStrAttackerAoD, // 反撃イベント攻撃側戦力補正
        CounterAttackOrgAttackerAoD, // 反撃イベント攻撃側組織率補正
        AssaultStrDefenderAoD, // 強襲イベント防御側戦力補正
        AssaultOrgDefenderAoD, // 強襲イベント防御側組織率補正
        AssaultStrAttackerAoD, // 強襲イベント攻撃側戦力補正
        AssaultOrgAttackerAoD, // 強襲イベント攻撃側組織率補正
        EncirclementStrDefenderAoD, // 包囲イベント防御側戦力補正
        EncirclementOrgDefenderAoD, // 包囲イベント防御側組織率補正
        EncirclementStrAttackerAoD, // 包囲イベント攻撃側戦力補正
        EncirclementOrgAttackerAoD, // 包囲イベント攻撃側組織率補正
        AmbushStrDefenderAoD, // 待伏イベント防御側戦力補正
        AmbushOrgDefenderAoD, // 待伏イベント防御側組織率補正
        AmbushStrAttackerAoD, // 待伏イベント攻撃側戦力補正
        AmbushOrgAttackerAoD, // 待伏イベント攻撃側組織率補正
        DelayStrDefenderAoD, // 遅延イベント防御側戦力補正
        DelayOrgDefenderAoD, // 遅延イベント防御側組織率補正
        DelayStrAttackerAoD, // 遅延イベント攻撃側戦力補正
        DelayOrgAttackerAoD, // 遅延イベント攻撃側組織率補正
        TacticalWithdrawStrDefenderAoD, // 後退イベント防御側戦力補正
        TacticalWithdrawOrgDefenderAoD, // 後退イベント防御側組織率補正
        TacticalWithdrawStrAttackerAoD, // 後退イベント攻撃側戦力補正
        TacticalWithdrawOrgAttackerAoD, // 後退イベント攻撃側組織率補正
        BreakthroughStrDefenderAoD, // 突破イベント防御側戦力補正
        BreakthroughOrgDefenderAoD, // 突破イベント防御側組織率補正
        BreakthroughStrAttackerAoD, // 突破イベント攻撃側戦力補正
        BreakthroughOrgAttackerAoD, // 突破イベント攻撃側組織率補正
        NavalOrgDamageAa, // 海軍対空砲組織率被ダメージ
        AirOrgDamageAa, // 空軍対空砲組織率被ダメージ
        AirStrDamageAa, // 空軍対空砲戦力被ダメージ
        AaAirFiringRules, // 対空砲攻撃ルール
        AaAirNightModifier, // 対空砲夜間攻撃補正
        AaAirBonusRadars, // 対空砲攻撃レーダーボーナス
        MovementBonusTerrainTrait, // 地形適正移動ボーナス
        MovementBonusSimilarTerrainTrait, // 類似地形適正移動ボーナス
        LogisticsWizardEseBonus, // 兵站管理の補給効率ボーナス
        DaysOffensiveSupply, // 攻勢継続日数
        MinisterBonuses, // 閣僚ボーナス適用方法
        OrgRegainBonusFriendly, // 友好地組織率回復ボーナス
        OrgRegainBonusFriendlyCap, // 友好地組織率回復ボーナス上限
        ConvoyInterceptionMissions, // 海上任務中の船団妨害
        AutoReturnTransportFleets, // 輸送艦隊の自動帰還
        AllowProvinceRegionTargeting, // 単一プロヴィンス/地域指定任務
        NightHoursWinter, // 冬季夜間時間
        NightHoursSpringFall, // 春季/秋季夜間時間
        NightHoursSummer, // 夏季夜間時間
        RecalculateLandArrivalTimes, // 陸上部隊到着時刻再計算間隔
        SynchronizeArrivalTimePlayer, // 同時到着補正(プレイヤー)
        SynchronizeArrivalTimeAi, // 同時到着補正(AI)
        RecalculateArrivalTimesCombat, // 戦闘後到着時刻再計算
        LandSpeedModifierCombat, // 戦闘時陸軍移動速度補正
        LandSpeedModifierBombardment, // 沿岸砲撃時陸軍移動速度補正
        LandSpeedModifierSupply, // 物資切れ時陸軍移動速度補正
        LandSpeedModifierOrg, // 組織率低下時陸軍移動速度補正
        LandAirSpeedModifierFuel, // 燃料切れ時陸軍/空軍移動速度補正
        DefaultSpeedFuel, // 燃料切れ時デフォルト移動速度
        FleetSizeRangePenaltyRatio, // 艦隊規模航続距離ペナルティ割合
        FleetSizeRangePenaltyThrethold, // 艦隊規模航続距離ペナルティ閾値
        FleetSizeRangePenaltyMax, // 艦隊規模航続距離ペナルティ上限
        ApplyRangeLimitsAreasRegions, // 地方/地域内での距離制限適用
        RadarBonusDetection, // レーダー航空機発見ボーナス
        BonusDetectionFriendly, // 友好地航空機発見ボーナス
        ScreensCapitalRatioModifier, // 主力艦/補助艦割合修正
        ChanceTargetNoOrgLand, // 陸軍組織率不足ユニット標的確率
        ScreenCapitalShipsTargeting, // 主力艦/補助艦標的ポジション値
        FleetPositioningDaytime, // 海戦ポジション値日中ボーナス
        FleetPositioningLeaderSkill, // 海戦ポジション値スキル補正
        FleetPositioningFleetSize, // 海戦ポジション値艦隊規模補正
        FleetPositioningFleetComposition, // 海戦ポジション値艦隊構成補正
        LandCoastalFortsDamage, // 要塞被ダメージ補正
        LandCoastalFortsMaxDamage, // 要塞最大被ダメージ
        MinSoftnessBrigades, // 付属旅団による最小脆弱性
        AutoRetreatOrg, // 自動撤退組織率
        LandOrgNavalTransportation, // 陸軍海上輸送後組織率補正
        MaxLandDig, // 最大塹壕値
        DigIncreaseDay, // 1日の塹壕増加量
        BreakthroughEncirclementMinSpeed, // 突破/包囲最小速度
        BreakthroughEncirclementMaxChance, // 突破/包囲最大確率
        BreakthroughEncirclementChanceModifier, // 突破/包囲確率補正
        CombatEventDuration, // コンバットイベント継続時間
        CounterAttackOrgAttackerDh, // 反撃イベント攻撃側組織率補正
        CounterAttackStrAttackerDh, // 反撃イベント攻撃側戦力補正
        CounterAttackOrgDefenderDh, // 反撃イベント防御側組織率補正
        CounterAttackStrDefenderDh, // 反撃イベント防御側戦力補正
        AssaultOrgAttackerDh, // 強襲イベント攻撃側組織率補正
        AssaultStrAttackerDh, // 強襲イベント攻撃側戦力補正
        AssaultOrgDefenderDh, // 強襲イベント防御側組織率補正
        AssaultStrDefenderDh, // 強襲イベント防御側戦力補正
        EncirclementOrgAttackerDh, // 包囲イベント攻撃側組織率補正
        EncirclementStrAttackerDh, // 包囲イベント攻撃側戦力補正
        EncirclementOrgDefenderDh, // 包囲イベント防御側組織率補正
        EncirclementStrDefenderDh, // 包囲イベント防御側戦力補正
        AmbushOrgAttackerDh, // 待伏イベント攻撃側組織率補正
        AmbushStrAttackerDh, // 待伏イベント攻撃側戦力補正
        AmbushOrgDefenderDh, // 待伏イベント防御側組織率補正
        AmbushStrDefenderDh, // 待伏イベント防御側戦力補正
        DelayOrgAttackerDh, // 遅延イベント攻撃側組織率補正
        DelayStrAttackerDh, // 遅延イベント攻撃側戦力補正
        DelayOrgDefenderDh, // 遅延イベント防御側組織率補正
        DelayStrDefenderDh, // 遅延イベント防御側戦力補正
        TacticalWithdrawOrgAttackerDh, // 後退イベント攻撃側組織率補正
        TacticalWithdrawStrAttackerDh, // 後退イベント攻撃側戦力補正
        TacticalWithdrawOrgDefenderDh, // 後退イベント防御側組織率補正
        TacticalWithdrawStrDefenderDh, // 後退イベント防御側戦力補正
        BreakthroughOrgAttackerDh, // 突破イベント攻撃側組織率補正
        BreakthroughStrAttackerDh, // 突破イベント攻撃側戦力補正
        BreakthroughOrgDefenderDh, // 突破イベント防御側組織率補正
        BreakthroughStrDefenderDh, // 突破イベント防御側戦力補正
        HqStrDamageBreakthrough, // 司令部は突破イベント時のみ戦力ダメージ
        CombatMode, // 戦闘モード
        CombatEnd, // 戦闘項目の末尾

        // mission
        AttackMission, // 攻撃任務
        AttackStartingEfficiency, // 攻撃初期効率
        AttackSpeedBonus, // 攻撃速度ボーナス
        RebaseMission, // 基地移動任務
        RebaseStartingEfficiency, // 基地移動初期効率
        RebaseChanceDetected, // 基地移動被発見確率
        StratRedeployMission, // 戦略的再配置任務
        StratRedeployStartingEfficiency, // 戦略的再配置初期効率
        StratRedeployAddedValue, // 戦略的再配置加算値
        StratRedeployDistanceMultiplier, // 戦略的再配置距離補正
        SupportAttackMission, // 支援攻撃任務
        SupportAttackStartingEfficiency, // 支援攻撃初期効率
        SupportAttackSpeedBonus, // 支援攻撃速度ボーナス
        SupportDefenseMission, // 防衛支援任務
        SupportDefenseStartingEfficiency, // 防衛支援初期効率
        SupportDefenseSpeedBonus, // 防衛支援速度ボーナス
        ReservesMission, // 待機任務
        ReservesStartingEfficiency, // 待機初期効率
        ReservesSpeedBonus, // 待機速度ボーナス
        AntiPartisanDutyMission, // パルチザン掃討任務
        AntiPartisanDutyStartingEfficiency, // パルチザン掃討初期効率
        AntiPartisanDutySuppression, // パルチザン掃討制圧力補正
        PlannedDefenseMission, // 防衛計画任務
        PlannedDefenseStartingEfficiency, // 防衛計画初期効率
        AirSuperiorityMission, // 制空権任務
        AirSuperiorityStartingEfficiency, // 制空権初期効率
        AirSuperiorityDetection, // 制空権敵機発見補正
        AirSuperiorityMinRequired, // 制空権最小ユニット数
        GroundAttackMission, // 地上攻撃任務
        GroundAttackStartingEfficiency, // 地上攻撃初期効率
        GroundAttackOrgDamage, // 地上攻撃組織率ダメージ補正
        GroundAttackStrDamage, // 地上攻撃戦力ダメージ補正
        InterdictionMission, // 阻止攻撃任務
        InterdictionStartingEfficiency, // 阻止攻撃初期効率
        InterdictionOrgDamage, // 阻止攻撃組織率ダメージ補正
        InterdictionStrDamage, // 阻止攻撃戦力ダメージ補正
        StrategicBombardmentMission, // 戦略爆撃任務
        StrategicBombardmentStartingEfficiency, // 戦略爆撃初期効率
        LogisticalStrikeMission, // 兵站攻撃任務
        LogisticalStrikeStartingEfficiency, // 兵站攻撃初期効率
        RunwayCrateringMission, // 空港空爆任務
        RunwayCrateringStartingEfficiency, // 空港空爆初期効率
        InstallationStrikeMission, // 軍事施設攻撃任務
        InstallationStrikeStartingEfficiency, // 軍事施設攻撃初期効率
        NavalStrikeMission, // 艦船攻撃任務
        NavalStrikeStartingEfficiency, // 艦船攻撃初期効率
        PortStrikeMission, // 港湾攻撃任務
        PortStrikeStartingEfficiency, // 港湾攻撃初期効率
        ConvoyAirRaidingMission, // 航空船団爆撃任務
        ConvoyAirRaidingStartingEfficiency, // 航空船団爆撃初期効率
        AirSupplyMission, // 空輸補給任務
        AirSupplyStartingEfficiency, // 空輸補給初期効率
        AirborneAssaultMission, // 空挺強襲任務
        AirborneAssaultStartingEfficiency, // 空挺強襲初期効率
        NukeMission, // 核攻撃任務
        NukeStartingEfficiency, // 核攻撃初期効率
        AirScrambleMission, // 航空緊急出撃任務
        AirScrambleStartingEfficiency, // 航空緊急出撃初期効率
        AirScrambleDetection, // 航空緊急出撃敵機発見補正
        AirScrambleMinRequired, // 航空緊急出撃最小ユニット数
        ConvoyRadingMission, // 船団襲撃任務
        ConvoyRadingStartingEfficiency, // 船団襲撃初期効率
        ConvoyRadingRangeModifier, // 船団襲撃航続距離補正
        ConvoyRadingChanceDetected, // 船団襲撃被発見確率
        AswMission, // 対潜作戦任務
        AswStartingEfficiency, // 対潜作戦初期効率
        NavalInterdictionMission, // 海上阻止任務
        NavalInterdictionStartingEfficiency, // 海上阻止初期効率
        ShoreBombardmentMission, // 沿岸砲撃任務
        ShoreBombardmentStartingEfficiency, // 沿岸砲撃初期効率
        ShoreBombardmentModifierDh, // 沿岸砲撃補正
        AmphibousAssaultMission, // 強襲上陸任務
        AmphibousAssaultStartingEfficiency, // 強襲上陸初期効率
        SeaTransportMission, // 海上輸送任務
        SeaTransportStartingEfficiency, // 海上輸送初期効率
        SeaTransportRangeModifier, // 海上輸送航続距離補正
        SeaTransportChanceDetected, // 海上輸送被発見確率
        NavalCombatPatrolMission, // 海上戦闘哨戒任務
        NavalCombatPatrolStartingEfficiency, // 海上戦闘哨戒初期効率
        NavalPortStrikeMission, // 空母による港湾攻撃任務
        NavalPortStrikeStartingEfficiency, // 空母による港湾攻撃初期効率
        NavalAirbaseStrikeMission, // 空母による航空基地攻撃任務
        NavalAirbaseStrikeStartingEfficiency, // 空母による航空基地攻撃初期効率
        SneakMoveMission, // 隠密移動任務
        SneakMoveStartingEfficiency, // 隠密移動初期効率
        SneakMoveRangeModifier, // 隠密移動航続距離補正
        SneakMoveChanceDetected, // 隠密移動被発見確率
        NavalScrambleMission, // 海上緊急出撃任務
        NavalScrambleStartingEfficiency, // 海上緊急出撃初期効率
        NavalScrambleSpeedBonus, // 海上緊急出撃速度ボーナス
        UseAttackEfficiencyCombatModifier, // 攻撃/支援攻撃効率を戦闘補正として使用
        MissionEnd, // 任務項目の末尾

        // country
        LandFortEfficiency, // 陸上要塞効率
        CoastalFortEfficiency, // 沿岸要塞効率
        GroundDefenseEfficiency, // 対地防御効率
        ConvoyDefenseEfficiency, // 船団防衛効率
        ManpowerBoost, // 人的資源増加
        TransportCapacityModifier, // TC補正
        OccupiedTransportCapacityModifier, // 占領地TC補正
        AttritionModifier, // 消耗補正
        ManpowerTrickleBackModifier, // 人的資源復帰補正
        SupplyDistanceModifier, // 遠隔地補給補正
        RepairModifier, // 修理補正
        ResearchModifier, // 研究補正
        RadarEfficiency, // レーダー効率
        HqSupplyEfficiencyBonus, // 司令部補給効率ボーナス
        HqCombatEventsBonus, // 司令部コンバットイベントボーナス
        CombatEventChances, // コンバットイベント発生確率
        FriendlyArmyDetectionChance, // 友軍発見確率
        EnemyArmyDetectionChance, // 敵軍発見確率
        FriendlyIntelligenceChance, // 友好国諜報確率
        EnemyIntelligenceChance, // 敵国諜報確率
        MaxAmphibiousArmySize, // 最大強襲上陸規模
        EnergyToOil, // エネルギー/石油変換効率
        TotalProductionEfficiency, // 総合生産効率
        SupplyProductionEfficiency, // 物資生産効率
        AaPower, // 対空砲攻撃力
        AirSurpriseChance, // 空軍奇襲確率
        LandSurpriseChance, // 陸軍奇襲確率
        NavalSurpriseChance, // 海軍奇襲確率
        PeacetimeIcModifier, // 平時IC補正
        WartimeIcModifier, // 戦時IC補正
        BuildingsProductionModifier, // 建造物生産補正
        ConvoysProductionModifier, // 輸送船団生産補正
        MinShipsPositioningBattle, // 最小艦船ポジション値
        MaxShipsPositioningBattle, // 最大艦船ポジション値
        PeacetimeStockpilesResources, // 平時資源備蓄補正
        WartimeStockpilesResources, // 戦時資源備蓄補正
        PeacetimeStockpilesOilSupplies, // 平時物資/燃料備蓄補正
        WartimeStockpilesOilSupplies, // 戦時物資/燃料備蓄補正
        CountryEnd, // 国家項目の末尾

        // research
        BlueprintBonus, // 青写真ボーナス
        PreHistoricalDateModifier, // 史実年度以前研究ペナルティ
        PostHistoricalDateModifierDh, // 史実年度以降研究ボーナス
        CostSkillLevel, // 研究機関レベル毎のコスト
        MeanNumberInventionEventsYear, // 1年毎の発明イベント平均回数
        PostHistoricalDateModifierAoD, // 史実年度以降研究ボーナス
        TechSpeedModifier, // 研究速度補正
        PreHistoricalPenaltyLimit, // 史実年度以前研究ペナルティ上限
        PostHistoricalBonusLimit, // 史実年度以降研究ボーナス上限
        MaxActiveTechTeamsAoD, // 研究スロット上限
        RequiredIcEachTechTeamAoD, // 研究スロット毎の必要IC
        MaximumRandomModifier, // 最大ランダム補正
        UseNewTechnologyPageLayout, // 研究ページのレイアウト
        TechOverviewPanelStyle, // 研究概要パネルのスタイル
        MaxActiveTechTeamsDh, // 研究スロット上限
        MinActiveTechTeams, // 研究スロット下限
        RequiredIcEachTechTeamDh, // 研究スロット毎の必要IC
        NewCountryRocketryComponent, // 新規国家でロケット技術を継承
        NewCountryNuclearPhysicsComponent, // 新規国家で核物理学技術を継承
        NewCountryNuclearEngineeringComponent, // 新規国家で核工学技術を継承
        NewCountrySecretTechs, // 新規国家で秘密兵器技術を継承
        MaxTechTeamSkill, // 最大研究機関スキル
        ResearchEnd, // 研究項目の末尾

        // trade
        DaysTradeOffers, // 貿易交渉間隔
        DelayGameStartNewTrades, // ゲーム開始直後の貿易交渉遅延日数
        LimitAiNewTradesGameStart, // ゲーム開始直後のAI友好国貿易交渉遅延日数
        DesiredOilStockpile, // 理想石油備蓄
        CriticalOilStockpile, // 危険水準石油備蓄
        DesiredSuppliesStockpile, // 理想物資備蓄
        CriticalSuppliesStockpile, // 危険水準物資備蓄
        DesiredResourcesStockpile, // 理想資源備蓄
        CriticalResourceStockpile, // 危険水準資源備蓄
        WartimeDesiredStockpileMultiplier, // 戦時理想備蓄係数
        PeacetimeExtraOilImport, // 平時石油臨時輸入割合
        WartimeExtraOilImport, // 戦時石油臨時輸入割合
        ExtraImportBelowDesired, // 理想備蓄未到達時の臨時輸入割合
        PercentageProducedSupplies, // 物資生産割合
        PercentageProducedMoney, // 資金生産割合
        ExtraImportStockpileSelected, // 備蓄選択時の臨時輸入割合
        DaysDeliverResourcesTrades, // 貿易協定資源輸送日数
        MergeTradeDeals, // 貿易協定の統合
        ManualTradeDeals, // 手動貿易協定
        PuppetsSendSuppliesMoney, // 傀儡国上納物資/資金
        PuppetsCriticalSupplyStockpile, // 傀儡国物資危険水準
        PuppetsMaxPoolResources, // 傀儡国最大資源備蓄
        NewTradeDealsMinEffectiveness, // 新規貿易協定最小効率
        CancelTradeDealsEffectiveness, // 貿易協定破棄効率
        AutoTradeAiTradeDeals, // 自動/AIの貿易協定最小効率
        TradeEnd, // 貿易項目の末尾

        // ai
        OverproduceSuppliesBelowDesired, // 理想備蓄未達時の余剰物資生産比率
        MultiplierOverproduceSuppliesWar, // 戦時余剰物資生産係数
        NotProduceSuppliesStockpileOver, // 備蓄余裕時の物資生産禁止係数
        MaxSerialLineProductionGarrisonMilitia, // 守備隊/民兵の最大連続生産数
        MinIcSerialProductionNavalAir, // 海軍/空軍連続生産最小IC
        NotProduceNewUnitsManpowerRatio, // 新規生産禁止人的資源比率
        NotProduceNewUnitsManpowerValue, // 新規生産禁止人的資源値
        NotProduceNewUnitsSupply, // 新規生産禁止物資比率
        MilitaryStrengthTotalIcRatioPeacetime, // 総ICに対する軍事力比率(平時)
        MilitaryStrengthTotalIcRatioWartime, // 総ICに対する軍事力比率(戦時)
        MilitaryStrengthTotalIcRatioMajor, // 総ICに対する軍事力比率(主要国)
        NotUseOffensiveSupplyStockpile, // 攻勢中止物資備蓄
        NotUseOffensiveOilStockpile, // 攻勢中止燃料備蓄
        NotUseOffensiveEse, // 攻勢中止補給効率
        NotUseOffensiveOrgStrDamage, // 攻勢中止組織率/戦力ダメージ
        AiPeacetimeSpyMissionsDh, // AIの平時の攻撃的諜報活動
        AiSpyMissionsCostModifierDh, // AIの諜報コスト補正
        AiDiplomacyCostModifierDh, // AIの外交コスト補正
        AiInfluenceModifierDh, // AIの外交影響度補正
        NewDowRules, // AIの新宣戦布告ルール
        NewDowRules2, // AIの新宣戦布告ルール
        ForcePuppetsJoinMastersAllianceNeutrality, // 傀儡国が宗主国の同盟に強制加入する中立性
        NewAiReleaseRules, // 新AI占領地解放ルール
        AiEventsActionSelectionRules, // AIイベント選択ルール
        ForceStrategicRedeploymentHour, // 強制戦略的再配置時間
        MaxRedeploymentDaysAi, // AI最大再配置日数
        UseQuickAreaCheckGarrisonAi, // 守備隊AIの簡易チェック
        AiMastersGetProvincesConquredPuppets, // AI宗主国が傀儡国の占領地を支配
        MinDaysRequiredAiReleaseCountry, // AI占領地解放最小日数
        MinDaysRequiredAiAllied, // AI占領地返還最小日数
        MinDaysRequiredAiAlliedSupplyBase, // AI占領地返還最小日数(物資補給基地)
        MinRequiredRelationsAlliedClaimed, // 被領有権主張時連合加盟最小友好度
        AiEnd, // AI項目の末尾

        // mod
        AiSpyDiplomaticMissionLogger, // AIの諜報/外交をログに記録
        CountryLogger, // 国家情報をログに記録
        SwitchedAiFilesLogger, // AI切り替えをログに記録
        UseNewAutoSaveFileFormat, // 新自動セーブファイル名
        LoadNewAiSwitchingAllClients, // マルチプレイでAI切替時に新しい設定を読み込む
        TradeEfficiencyCalculationSystem, // 貿易効率算出間隔
        MergeRelocateProvincialDepots, // 備蓄庫の再計算間隔
        InGameLossesLogging, // 損失を記録
        InGameLossLogging2, // 損失を記録
        AllowBrigadeAttachingInSupply, // 占領地で旅団付属を許可
        MultipleDeploymentSizeArmies, // 陸軍の一括配置数
        MultipleDeploymentSizeFleets, // 海軍の一括配置数
        MultipleDeploymentSizeAir, // 空軍の一括配置数
        AllowUniquePicturesAllLandProvinces, // すべての陸地プロヴィンスに固有画像を許可
        AutoReplyEvents, // イベント選択肢を委任
        ForceActionsShow, // イベント選択肢強制表示
        EnableDicisionsPlayers, // ディシジョンを使用する
        RebelsArmyComposition, // パルチザンの歩兵構成割合
        RebelsArmyTechLevel, // パルチザンの技術レベル
        RebelsArmyMinStr, // パルチザン最小戦力
        RebelsArmyMaxStr, // パルチザン最大戦力
        RebelsOrgRegain, // パルチザン組織率回復速度
        ExtraRebelBonusNeighboringProvince, // パルチザンボーナス(隣接地占領)
        ExtraRebelBonusOccupied, // パルチザンボーナス(占領地)
        ExtraRebelBonusMountain, // パルチザンボーナス(山岳)
        ExtraRebelBonusHill, // パルチザンボーナス(丘陵)
        ExtraRebelBonusForest, // パルチザンボーナス(森林)
        ExtraRebelBonusJungle, // パルチザンボーナス(密林)
        ExtraRebelBonusSwamp, // パルチザンボーナス(湿地)
        ExtraRebelBonusDesert, // パルチザンボーナス(砂漠)
        ExtraRebelBonusPlain, // パルチザンボーナス(平地)
        ExtraRebelBonusUrban, // パルチザンボーナス(都市)
        ExtraRebelBonusAirNavalBases, // パルチザンボーナス(航空/海軍基地)
        ReturnRebelliousProvince, // パルチザン占領プロヴィンス返却時間
        UseNewMinisterFilesFormat, // 新形式閣僚ファイルフォーマット
        EnableRetirementYearMinisters, // 閣僚引退年を使用
        EnableRetirementYearLeaders, // 指揮官引退年を使用
        LoadSpritesModdirOnly, // スプライトをMODDIRのみから読み込む
        LoadUnitIconsModdirOnly, // ユニットアイコンをMODDIRのみから読み込む
        LoadUnitPicturesModdirOnly, // ユニット画像をMODDIRのみから読み込む
        LoadAiFilesModdirOnly, // AIファイルをMODDIRのみから読み込む
        UseSpeedSetGarrisonStatus, // 守備隊判定ルール
        UseOldSaveGameFormat, // 旧セーブフォーマットを使用
        ProductionPanelUiStyle, // 生産パネルのUIスタイル
        UnitPicturesSize, // ユニット画像のサイズ
        EnablePicturesNavalBrigades, // 艦艇付属装備に画像を使用
        BuildingsBuildableOnlyProvinces, // 建物をプロヴィンスでのみ建造
        UnitModifiersStatisticsPages, // ユニット補正ページの新スタイル移行閾値
        ModEnd, // MOD項目の末尾

        // map
        MapNumber, // マップ番号
        TotalProvinces, // 総プロヴィンス数
        DistanceCalculationModel, // 距離算出方法
        MapWidth, // マップの幅
        MapHeight, // マップの高さ
        MapEnd, // マップ項目の末尾
    }

    /// <summary>
    ///     miscセクションID
    /// </summary>
    public enum MiscSectionId
    {
        Economy,
        Intelligence,
        Diplomacy,
        Combat,
        Mission,
        Country,
        Research,
        Trade,
        Ai,
        Mod,
        Map,
    }

    /// <summary>
    ///     miscファイルの種類
    /// </summary>
    public enum MiscGameType
    {
        Dda12, // DDA1.2
        Dda13, // DDA1.3
        Aod104, // AoD1.04
        Aod107, // AoD1.07
        Aod108, // AoD1.08-
        Dh102, // DH1.02
        Dh103, // DH1.03-
    }

    /// <summary>
    ///     misc項目の型
    /// </summary>
    public enum MiscItemType
    {
        None, // 終端項目
        Bool, // ブール値
        Enum, // 選択肢
        Int, // 整数
        PosInt, // 正の整数
        NonNegInt, // 非負の整数
        NonPosInt, // 非正の整数
        NonNegIntMinusOne, // 非負の整数 or -1
        NonNegInt1, // 非負の整数 (小数点以下1桁)
        RangedInt, // 範囲あり整数
        RangedPosInt, // 範囲あり正の整数
        RangedIntMinusOne, // 範囲あり整数 or -1
        RangedIntMinusThree, // 範囲あり実数 or -1 or -2 or -3
        Dbl, // 実数
        PosDbl, // 正の実数
        NonNegDbl, // 非負の実数
        NonPosDbl, // 非正の実数
        NonNegDbl0, // 非負の実数 (小数点以下なし)
        NonNegDbl2, // 非負の実数 (小数点以下2桁)
        NonNegDbl5, // 非負の実数 (小数点以下5桁)
        NonPosDbl0, // 非正の実数 (小数点以下なし)
        NonPosDbl2, // 非正の実数 (小数点以下2桁)
        NonNegDblMinusOne, // 非負の実数 or -1
        NonNegDblMinusOne1, // 非負の実数 or -1.0
        NonNegDbl2AoD, // 非負の実数 (AoDのみ小数点以下2桁)
        NonNegDbl4Dda13, // 非負の実数 (DDA1.3/DHのみ小数点以下4桁)
        NonNegDbl2Dh103Full, // 非負の実数 (0より大きく0.10以下の場合のみ小数点以下2桁)
        NonNegDbl2Dh103Full1, // 非負の実数 (小数点以下2桁/0より大きく0.20以下の場合のみ小数点以下1桁)
        NonNegDbl2Dh103Full2, // 非負の実数 (0より大きく1未満の場合のみ小数点以下2桁)
        NonPosDbl5AoD, // 非正の実数 (AoDのみ小数点以下5桁)
        NonPosDbl2Dh103Full, // 非正の実数 (-0.10以上0未満の場合のみ小数点以下2桁)
        RangedDbl, // 範囲あり実数
        RangedDblMinusOne, // 範囲あり実数 or -1
        RangedDblMinusOne1, // 範囲あり実数 or -1.0
        RangedDbl0, // 範囲あり実数 (小数点以下なし)
        NonNegIntNegDbl, // 非負の整数 or 負の実数
    }
}