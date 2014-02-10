using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
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
            get
            {
                return (_items[(int) MiscItemId.TpMaxAttach] != null) ? (int) _items[(int) MiscItemId.TpMaxAttach] : 0;
            }
        }

        /// <summary>
        ///     潜水艦最大付属装備数
        /// </summary>
        public static int SsMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.SsMaxAttach] != null) ? (int) _items[(int) MiscItemId.SsMaxAttach] : 0;
            }
        }

        /// <summary>
        ///     原子力潜水艦最大付属装備数
        /// </summary>
        public static int SsnMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.SsnMaxAttach] != null) ? (int) _items[(int) MiscItemId.SsnMaxAttach] : 0;
            }
        }

        /// <summary>
        ///     駆逐艦最大付属装備数
        /// </summary>
        public static int DdMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.DdMaxAttach] != null) ? (int) _items[(int) MiscItemId.DdMaxAttach] : 1;
            }
        }

        /// <summary>
        ///     軽巡洋艦最大付属装備数
        /// </summary>
        public static int ClMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.ClMaxAttach] != null) ? (int) _items[(int) MiscItemId.ClMaxAttach] : 2;
            }
        }

        /// <summary>
        ///     重巡洋艦最大付属装備数
        /// </summary>
        public static int CaMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.CaMaxAttach] != null) ? (int) _items[(int) MiscItemId.CaMaxAttach] : 3;
            }
        }

        /// <summary>
        ///     巡洋戦艦最大付属装備数
        /// </summary>
        public static int BcMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.BcMaxAttach] != null) ? (int) _items[(int) MiscItemId.BcMaxAttach] : 4;
            }
        }

        /// <summary>
        ///     戦艦最大付属装備数
        /// </summary>
        public static int BbMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.BbMaxAttach] != null) ? (int) _items[(int) MiscItemId.BbMaxAttach] : 5;
            }
        }

        /// <summary>
        ///     軽空母最大付属装備数
        /// </summary>
        public static int CvlMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.CvlMaxAttach] != null) ? (int) _items[(int) MiscItemId.CvlMaxAttach] : 1;
            }
        }

        /// <summary>
        ///     空母最大付属装備数
        /// </summary>
        public static int CvMaxAttach
        {
            get
            {
                return (_items[(int) MiscItemId.CvMaxAttach] != null) ? (int) _items[(int) MiscItemId.CvMaxAttach] : 1;
            }
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
        private static string[] _comments;

        /// <summary>
        ///     セクション末尾の文字列
        /// </summary>
        private static string[] _suffixes;

        #endregion

        #region 公開定数

        /// <summary>
        ///     セクション文字列
        /// </summary>
        public static readonly string[] SectionStrings =
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
        ///     ゲームごとのセクションの有無
        /// </summary>
        public static bool[,] SectionTable =
        {
            {true, true, true, true, true, true, true}, // 経済
            {false, false, false, false, false, true, true}, // 諜報
            {false, false, false, false, false, true, true}, // 外交
            {true, true, true, true, true, true, true}, // 戦闘
            {false, false, false, false, false, true, true}, // 任務
            {false, false, false, false, false, true, true}, // 国家
            {true, true, true, true, true, true, true}, // 研究
            {false, false, false, false, false, true, true}, // 貿易
            {false, false, false, false, false, true, true}, // AI
            {false, false, false, false, false, true, true}, // MOD
            {false, false, false, false, false, true, true} // マップ
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
                MiscItemId.ResourceConvoysBackUnneeded
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
                MiscItemId.SpiesMoneyModifier
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
                MiscItemId.FilterReleaseCountries
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
                MiscItemId.CombatMode
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
                MiscItemId.UseAttackEfficiencyCombatModifier
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
                MiscItemId.WartimeStockpilesOilSupplies
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
                MiscItemId.MaxTechTeamSkill
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
                MiscItemId.AutoTradeAiTradeDeals
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
                MiscItemId.MinRequiredRelationsAlliedClaimed
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
                MiscItemId.UnitModifiersStatisticsPages
            },
            new[]
            {
                MiscItemId.MapNumber,
                MiscItemId.TotalProvinces,
                MiscItemId.DistanceCalculationModel,
                MiscItemId.MapWidth,
                MiscItemId.MapHeight
            }
        };

        /// <summary>
        ///     ゲームごとの項目の有無
        /// </summary>
        public static bool[,] ItemTable =
        {
            {true, true, true, true, true, true, true}, // ICからTCへの変換効率
            {true, true, true, true, true, true, true}, // ICから物資への変換効率
            {true, true, true, true, true, true, true}, // ICから消費財への変換効率
            {true, true, true, true, true, true, true}, // ICから資金への変換効率
            {false, false, true, true, true, false, false}, // 不満度減少速度
            {false, false, false, false, false, true, true}, // 最小実効ICの比率
            {false, false, false, false, false, true, true}, // 最小実効IC
            {false, false, false, false, false, true, true}, // 不満度低下補正
            {true, true, true, true, true, true, true}, // 最大ギアリングボーナス
            {true, true, true, true, true, true, true}, // ギアリングボーナスの増加値
            {false, false, true, true, true, false, false}, // 連続生産時の資源消費増加
            {false, false, true, true, true, false, false}, // IC不足時のギアリングボーナス減少値
            {true, true, true, true, true, true, true}, // 非中核州のIC補正
            {true, true, true, true, true, true, true}, // 占領地のIC補正
            {false, false, false, false, false, true, true}, // 属国のIC補正
            {false, false, false, false, false, true, true}, // 非中核州の資源補正
            {false, false, false, false, false, true, true}, // 占領地の資源補正
            {false, false, false, false, false, true, true}, // 非中核州の資源補正(AI)
            {false, false, false, false, false, true, true}, // 属国の資源補正
            {true, true, true, true, true, true, true}, // 未配備師団のTC負荷
            {true, true, true, true, true, true, true}, // 占領地のTC負荷
            {true, true, true, true, true, true, true}, // 陸軍師団のTC負荷補正
            {true, true, true, true, true, true, true}, // 空軍師団のTC負荷補正
            {true, true, true, true, true, true, true}, // 海軍師団のTC負荷補正
            {true, true, true, true, true, true, true}, // パルチザンのTC負荷
            {true, true, true, true, true, true, true}, // 攻勢時のTC負荷係数
            {true, true, true, true, true, true, true}, // プロヴィンス開発のTC負荷
            {true, true, true, true, true, true, true}, // 未配備の基地のTC負荷
            {true, true, true, true, true, true, true}, // 中核州の人的資源補正
            {true, true, true, true, true, true, true}, // 非中核州の人的資源補正
            {true, true, true, true, true, true, true}, // 海外州の人的資源補正
            {false, false, false, false, false, true, true}, // 属国の人的資源補正
            {false, false, false, false, false, true, true}, // 戦時の海外州の人的資源補正
            {false, false, false, false, false, true, true}, // 平時の人的資源補正
            {false, false, false, false, false, true, true}, // 戦時の人的資源補正
            {false, false, false, false, false, true, true}, // 人的資源の老化率
            {true, true, true, true, true, false, false}, // 政策スライダーに影響を与えるためのIC比率
            {true, true, true, true, true, true, true}, // 戦闘による損失からの復帰係数
            {true, true, true, true, true, true, true}, // 補充に必要な人的資源の比率
            {true, true, true, true, true, true, true}, // 補充に必要なICの比率
            {true, true, true, true, true, true, true}, // 補充に必要な時間の比率
            {true, true, true, true, true, true, true}, // 改良に必要なICの比率
            {true, true, true, true, true, true, true}, // 改良に必要な時間の比率
            {false, false, false, false, false, true, true}, // 改良のための補充係数
            {true, true, true, true, true, true, true}, // ナショナリズムの初期値
            {false, false, false, true, true, false, false}, // 人的資源によるナショナリズムの補正値
            {false, false, false, false, false, true, true}, // 人的資源によるナショナリズムの補正値
            {false, false, false, false, false, true, true}, // ナショナリズム最大値
            {false, false, false, false, false, true, true}, // 最大反乱率
            {true, true, true, true, true, true, true}, // 月ごとのナショナリズムの減少値
            {true, true, true, true, true, true, true}, // 師団譲渡後配備可能になるまでの時間
            {true, true, true, true, true, true, true}, // 未配備旅団のTC負荷
            {true, true, true, true, true, false, false}, // 非同盟国に師団を売却/譲渡
            {false, true, false, false, false, false, false}, // 諜報任務の間隔
            {false, true, false, false, false, false, false}, // 諜報レベルの増加間隔
            {false, true, false, false, false, false, false}, // 国内の諜報活動を発見する確率
            {false, true, false, false, false, false, false}, // 諜報任務発覚時の友好度低下量
            {false, true, false, false, false, false, false}, // 第三国の諜報活動を報告
            {false, true, false, false, false, false, false}, // 諜報任務の隣国補正
            {false, true, false, false, false, false, false}, // 情報の正確さ補正
            {false, true, false, false, false, false, false}, // 平時のAIの攻撃的諜報活動
            {false, true, false, false, false, false, false}, // 諜報コスト補正の最大IC
            {false, true, false, false, false, false, false}, // AIの諜報コスト補正
            {false, true, false, false, false, false, false}, // AIの外交コスト補正
            {false, true, false, false, false, false, false}, // AIの外交干渉頻度補正
            {false, false, true, true, true, false, false}, // 建物修復コスト補正
            {false, false, true, true, true, false, false}, // 建物修復時間補正
            {false, false, true, true, true, false, false}, // プロヴィンス効率上昇時間
            {false, false, false, true, true, false, false}, // 中核プロヴィンス効率上昇時間
            {false, false, true, true, true, false, false}, // ライン維持コスト補正
            {false, false, true, true, true, false, false}, // ライン開始時間
            {false, false, true, true, true, false, false}, // ライン改良時間
            {false, false, true, true, true, false, false}, // ライン調整コスト補正
            {false, false, true, true, true, false, false}, // ライン調整資源補正
            {false, false, true, true, true, false, false}, // 人的資源老化補正
            {false, false, true, true, true, false, false}, // 船団襲撃時物資使用量補正
            {false, false, true, true, true, false, false}, // 海軍の待機時物資使用量補正
            {false, false, true, true, true, false, false}, // 海軍の移動時物資使用量補正
            {false, false, true, true, true, false, false}, // 海軍の戦闘時物資使用量補正
            {false, false, true, true, true, false, false}, // 空軍の待機時物資使用量補正
            {false, false, true, true, true, false, false}, // 空軍の移動時物資使用量補正
            {false, false, true, true, true, false, false}, // 空軍の戦闘時物資使用量補正
            {false, false, true, true, true, false, false}, // 空軍の爆撃時物資使用量補正
            {false, false, true, true, true, false, false}, // 陸軍の待機時物資使用量補正
            {false, false, true, true, true, false, false}, // 陸軍の移動時物資使用量補正
            {false, false, true, true, true, false, false}, // 陸軍の戦闘時物資使用量補正
            {false, false, true, true, true, false, false}, // 陸軍の砲撃時物資使用量補正
            {false, false, true, true, true, false, false}, // 陸軍の物資備蓄量
            {false, false, true, true, true, false, false}, // 空軍の物資備蓄量
            {false, false, true, true, true, false, false}, // 海軍の物資備蓄量
            {false, false, false, true, true, false, false}, // 陸軍の物資再備蓄速度
            {false, false, false, true, true, false, false}, // 空軍の物資再備蓄速度
            {false, false, false, true, true, false, false}, // 海軍の物資再備蓄速度
            {false, false, true, true, true, false, false}, // 合成石油変換係数
            {false, false, true, true, true, false, false}, // 合成希少資源変換係数
            {false, false, true, true, true, false, false}, // 軍隊の給料
            {false, false, true, true, true, false, false}, // 最大諜報費比率
            {false, false, true, true, true, false, false}, // 最大研究費比率
            {false, false, true, true, true, false, false}, // 軍隊の給料不足時の消耗補正
            {false, false, true, true, true, false, false}, // 軍隊の給料不足時の不満度補正
            {false, false, true, true, true, false, false}, // 原子炉維持コスト
            {false, false, true, true, true, false, false}, // 原子力発電所維持コスト
            {false, false, true, true, true, false, false}, // 合成石油工場維持コスト
            {false, false, true, true, true, false, false}, // 合成希少資源工場維持コスト
            {false, false, true, true, true, false, false}, // 海軍情報の存続期間
            {false, false, true, true, true, false, false}, // 船団攻撃回避時間
            {false, false, true, true, true, false, false}, // 船団攻撃妨害時間
            {false, false, true, true, true, false, false}, // 自動貿易に必要な輸送船団割合
            {false, false, true, true, true, false, false}, // 諜報維持コスト
            {false, false, true, true, true, false, false}, // スパイ発見確率
            {false, false, false, true, true, false, false}, // 不満度によるクーデター成功率修正
            {false, false, true, true, true, false, false}, // インフラによるプロヴィンス効率補正
            {false, false, true, true, true, false, false}, // 人的資源の消費財生産補正
            {false, false, true, true, true, false, false}, // スライダー移動の間隔
            {false, false, true, true, true, false, false}, // 海外プロヴィンスへの配置の必要IC
            {false, false, true, true, true, false, false}, // 原子力発電量
            {false, false, true, true, true, false, false}, // インフラの自然回復率
            {false, false, true, true, true, false, false}, // スライダー移動時の最大不満度
            {false, false, true, true, true, false, false}, // スライダー移動時の最小不満度
            {false, false, true, true, true, false, false}, // スライダー移動可能な最大不満度
            {false, false, true, true, true, false, false}, // 工場集中ボーナス
            {false, false, true, false, false, false, false}, // 輸送艦変換係数
            {false, false, false, true, true, false, false}, // 輸送船団変換係数
            {false, false, false, true, true, false, false}, // 護衛船団変換係数
            {false, false, true, true, true, false, false}, // 閣僚変更遅延日数
            {false, false, true, true, true, false, false}, // 閣僚変更遅延日数(イベント)
            {false, false, true, true, true, false, false}, // 国策変更遅延日数
            {false, false, true, true, true, false, false}, // 国策変更遅延日数(イベント)
            {false, false, true, true, true, false, false}, // 指揮官変更遅延日数
            {false, false, true, true, true, false, false}, // 国策変更時の不満度上昇量
            {false, false, true, true, true, false, false}, // 閣僚変更時の不満度上昇量
            {false, false, true, true, true, false, false}, // 反乱が発生する最低不満度
            {false, false, true, true, true, false, false}, // 不満度による反乱軍発生率係数
            {false, false, false, true, true, false, false}, // 輸送艦最大付属装備数
            {false, false, false, true, true, false, false}, // 潜水艦最大付属装備数
            {false, false, false, true, true, false, false}, // 原子力潜水艦最大付属装備数
            {false, false, false, true, true, false, false}, // 駆逐艦最大付属装備数
            {false, false, false, true, true, false, false}, // 軽巡洋艦最大付属装備数
            {false, false, false, true, true, false, false}, // 重巡洋艦最大付属装備数
            {false, false, false, true, true, false, false}, // 巡洋戦艦最大付属装備数
            {false, false, false, true, true, false, false}, // 戦艦最大付属装備数
            {false, false, false, true, true, false, false}, // 軽空母最大付属装備数
            {false, false, false, true, true, false, false}, // 空母最大付属装備数
            {false, false, false, true, true, false, false}, // プレイヤーの国策変更を許可
            {false, false, false, false, false, true, true}, // 非同盟国に師団を売却
            {false, false, false, false, false, true, true}, // 非同盟国に青写真を売却
            {false, false, false, false, false, true, true}, // 非同盟国にプロヴィンスを売却
            {false, false, false, false, false, true, true}, // 占領中の同盟国の中核州返還
            {false, false, false, false, false, true, true}, // 建物修復速度補正
            {false, false, false, false, false, true, true}, // 資源回復速度補正
            {false, false, false, false, false, true, true}, // 資源備蓄上限補正
            {false, false, false, false, false, true, true}, // 物資/燃料備蓄上限補正
            {false, false, false, false, false, true, true}, // 超過備蓄損失割合
            {false, false, false, false, false, true, true}, // 資源備蓄上限値
            {false, false, false, false, false, true, true}, // 物資/燃料備蓄上限値
            {false, false, false, false, false, true, true}, // 理想物資/燃料備蓄比率
            {false, false, false, false, false, true, true}, // 最大人的資源
            {false, false, false, false, false, true, true}, // 船団輸送能力
            {false, false, false, false, false, true, true}, // 陸軍の待機時物資使用量補正
            {false, false, false, false, false, true, true}, // 陸軍の戦闘時物資使用量補正
            {false, false, false, false, false, true, true}, // 陸軍の待機時燃料使用量補正
            {false, false, false, false, false, true, true}, // 陸軍の戦闘時燃料使用量補正
            {false, false, false, false, false, true, true}, // 空軍の待機時物資使用量補正
            {false, false, false, false, false, true, true}, // 空軍の戦闘時物資使用量補正
            {false, false, false, false, false, true, true}, // 空軍/海軍の待機時燃料使用量補正
            {false, false, false, false, false, true, true}, // 空軍の戦闘時燃料使用量補正
            {false, false, false, false, false, true, true}, // 海軍の待機時物資使用量補正
            {false, false, false, false, false, true, true}, // 海軍の戦闘時物資使用量補正
            {false, false, false, false, false, true, true}, // 海軍の非移動時燃料使用量補正
            {false, false, false, false, false, true, true}, // 海軍の戦闘時燃料使用量補正
            {false, false, false, false, false, true, true}, // 輸送艦の輸送船団への変換比率
            {false, false, false, false, false, true, true}, // 駆逐艦の護衛船団への変換比率
            {false, false, false, false, false, true, true}, // 軽巡洋艦の護衛船団への変換比率
            {false, false, false, false, false, true, true}, // 軽空母の護衛船団への変換比率
            {false, false, false, false, false, true, true}, // 生産ラインの編集
            {false, false, false, false, false, true, true}, // ユニット改良時のギアリングボーナス減少比率
            {false, false, false, false, false, true, true}, // 旅団改良時のギアリングボーナス減少比率
            {false, false, false, false, false, true, true}, // 中核州核攻撃時の不満度上昇係数
            {false, false, false, false, false, true, true}, // 物資/消費財不足時の最大不満度上昇値
            {false, false, false, false, false, false, true}, // 核兵器生産補正
            {false, false, false, false, false, false, true}, // 同盟国に対する船団システム
            {false, false, false, false, false, false, true}, // 不要な資源/燃料の回収比率
            {false, false, false, false, false, true, true}, // 諜報任務の間隔
            {false, false, false, false, false, true, true}, // 諜報レベルの増加間隔
            {false, false, false, false, false, true, true}, // 国内の諜報活動を発見する確率
            {false, false, false, false, false, true, true}, // 諜報任務発覚時の友好度低下量
            {false, false, false, false, false, true, true}, // 諜報任務の距離補正
            {false, false, false, false, false, true, true}, // 諜報任務の隣国補正
            {false, false, false, false, false, true, true}, // 諜報レベルの距離補正
            {false, false, false, false, false, true, true}, // 諜報レベル10超過時の距離補正
            {false, false, false, false, false, true, true}, // 情報の正確さ補正
            {false, false, false, false, false, true, true}, // 諜報コストのIC補正
            {false, false, false, false, false, true, true}, // 諜報コスト補正の最小IC
            {false, false, false, false, false, true, true}, // 諜報コスト補正の最大IC
            {false, false, false, false, false, true, true}, // 諜報レベル10超過時追加維持コスト
            {false, false, false, false, false, true, true}, // 諜報レベル10超過時増加コスト
            {false, false, false, false, false, true, true}, // 第三国の諜報活動を報告
            {false, false, false, false, false, true, true}, // 諜報資金割り当て補正
            {false, false, false, false, false, true, true}, // 外交官派遣間隔
            {false, false, false, false, false, true, true}, // スライダー移動の間隔
            {false, false, false, false, false, true, true}, // 政策スライダーに影響を与えるためのIC比率
            {false, false, false, false, false, true, true}, // 閣僚交代時に閣僚特性を適用する
            {false, false, false, false, false, true, true}, // 貿易キャンセル時の友好度低下
            {false, false, false, false, false, true, true}, // 永久貿易キャンセル時の友好度低下
            {false, false, false, false, false, true, true}, // 属国が宗主国の同盟に強制参加する
            {false, false, false, false, false, true, true}, // 属国の属国が設立できるか
            {false, false, false, false, false, true, true}, // 領有権主張の変更
            {false, false, false, false, false, true, true}, // 領有権主張時の好戦性上昇値
            {false, false, false, false, false, true, true}, // 領有権撤回時の好戦性減少値
            {false, false, false, false, false, true, true}, // 宣戦布告された時に対抗陣営へ自動加盟
            {false, false, false, false, false, true, true}, // 国家元首/政府首班の交代
            {false, false, false, false, false, true, true}, // クーデター発生時に兄弟国へ変更
            {false, false, false, false, false, true, true}, // 独立可能国設定
            {true, true, true, true, true, true, true}, // 陸軍経験値入手係数
            {true, true, true, true, true, true, true}, // 海軍経験値入手係数
            {true, true, true, true, true, true, true}, // 空軍経験値入手係数
            {false, false, false, false, false, true, true}, // 空軍空戦時経験値入手係数
            {true, true, true, true, true, true, true}, // 師団経験値入手係数
            {true, true, true, true, true, true, true}, // 指揮官経験値入手係数
            {true, true, true, true, true, true, true}, // 消耗係数
            {false, false, true, true, true, false, false}, // 無補給時の自然条件消耗係数
            {false, false, true, true, true, false, false}, // 無補給時の消耗係数
            {true, true, true, true, true, true, true}, // 基地戦闘補正
            {true, true, true, true, true, false, false}, // 艦砲射撃戦闘補正
            {false, false, false, false, true, false, false}, // 艦砲射撃戦闘効率上限
            {true, true, true, true, true, true, true}, // 強襲上陸ペナルティ
            {true, true, true, true, true, true, true}, // 側面攻撃ペナルティ
            {true, true, true, true, true, true, true}, // 攻撃側諸兵科連合ボーナス
            {true, true, true, true, true, true, true}, // 防御側諸兵科連合ボーナス
            {true, true, true, true, true, true, true}, // 奇襲攻撃ペナルティ
            {true, true, true, true, true, true, true}, // 陸軍指揮上限ペナルティ
            {true, true, true, true, true, true, true}, // 空軍指揮上限ペナルティ
            {true, true, true, true, true, true, true}, // 海軍指揮上限ペナルティ
            {true, true, true, true, true, true, true}, // 多方面攻撃補正
            {true, true, true, true, true, true, true}, // 包囲攻撃ペナルティ
            {true, true, true, true, true, true, true}, // 要塞攻撃ペナルティ
            {true, true, true, true, true, true, true}, // 沿岸要塞攻撃ペナルティ
            {false, false, false, false, false, true, true}, // 装甲ユニットの都市攻撃ペナルティ
            {true, true, true, true, true, true, true}, // 国民不満度ペナルティ
            {true, true, true, true, true, false, false}, // 補給不足ペナルティ
            {false, false, false, false, false, true, true}, // 陸軍物資不足ペナルティ
            {false, false, false, false, false, true, true}, // 空軍物資不足ペナルティ
            {false, false, false, false, false, true, true}, // 海軍物資不足ペナルティ
            {false, false, false, false, false, true, true}, // 陸軍燃料不足ペナルティ
            {false, false, false, false, false, true, true}, // 空軍燃料不足ペナルティ
            {false, false, false, false, false, true, true}, // 海軍燃料不足ペナルティ
            {true, true, true, true, true, true, true}, // レーダー補正
            {false, false, true, true, true, false, false}, // レーダー/対空砲複合補正
            {true, true, true, true, true, true, true}, // 爆撃機迎撃ボーナス
            {true, true, false, false, false, true, true}, // 空軍スタックペナルティ
            {false, false, true, true, true, false, false}, // 空軍スタックペナルティ
            {true, true, true, true, true, true, true}, // 海軍スタックペナルティ
            {true, true, true, true, true, true, true}, // 陸軍元帥指揮上限
            {true, true, true, true, true, true, true}, // 陸軍大将指揮上限
            {true, true, true, true, true, true, true}, // 陸軍中将指揮上限
            {true, true, true, true, true, true, true}, // 陸軍少将指揮上限
            {true, true, true, true, true, true, true}, // 空軍元帥指揮上限
            {true, true, true, true, true, true, true}, // 空軍大将指揮上限
            {true, true, true, true, true, true, true}, // 空軍中将指揮上限
            {true, true, true, true, true, true, true}, // 空軍少将指揮上限
            {true, true, true, true, true, true, true}, // 海軍元帥指揮上限
            {true, true, true, true, true, true, true}, // 海軍大将指揮上限
            {true, true, true, true, true, true, true}, // 海軍中将指揮上限
            {true, true, true, true, true, true, true}, // 海軍少将指揮上限
            {true, true, true, true, true, true, true}, // 司令部指揮上限係数
            {true, true, true, true, true, true, true}, // 輸送船団護衛係数
            {false, false, false, false, false, true, true}, // 輸送船団護衛モデル
            {true, true, true, true, true, true, true}, // 戦闘後命令遅延時間
            {false, false, true, true, true, false, false}, // 陸軍命令遅延時間
            {false, false, true, true, true, false, false}, // 海軍命令遅延時間
            {false, false, true, true, true, false, false}, // 空軍命令遅延時間
            {true, true, true, true, true, true, true}, // 空軍最大スタックサイズ
            {false, false, false, false, false, true, true}, // 空戦最小戦闘時間
            {false, false, false, false, false, true, true}, // 港湾攻撃最小戦闘時間
            {false, false, false, false, false, true, true}, // 戦略爆撃最小戦闘時間
            {false, false, false, false, false, true, true}, // 地上爆撃最小戦闘時間
            {true, true, true, true, true, true, true}, // 経験値補正
            {true, true, true, true, true, true, true}, // 海軍基地戦略爆撃係数
            {true, true, true, true, true, true, true}, // 空軍基地戦略爆撃係数
            {true, true, true, true, true, true, true}, // 対空砲戦略爆撃係数
            {true, true, true, true, true, true, true}, // ロケット試験場戦略爆撃係数
            {true, true, true, true, true, true, true}, // 原子炉戦略爆撃係数
            {true, true, true, true, true, true, true}, // レーダー戦略爆撃係数
            {true, true, true, true, true, true, true}, // インフラ戦略爆撃係数
            {true, true, true, true, true, true, true}, // IC戦略爆撃係数
            {true, true, true, true, true, true, true}, // 資源戦略爆撃係数
            {false, false, true, true, true, false, false}, // 合成石油工場戦略爆撃係数
            {true, true, true, true, true, true, false}, // 対地防御効率補正
            {true, true, true, true, true, true, false}, // 基本回避率(防御回数あり)
            {true, true, true, true, true, true, false}, // 基本回避率(防御回数なし)
            {false, false, false, false, false, false, true}, // 陸軍基本回避率(防御回数あり)
            {false, false, false, false, false, false, true}, // 空軍基本回避率(防御回数あり)
            {false, false, false, false, false, false, true}, // 海軍基本回避率(防御回数あり)
            {false, false, false, false, false, false, true}, // 陸軍基本回避率(防御回数なし)
            {false, false, false, false, false, false, true}, // 空軍基本回避率(防御回数なし)
            {false, false, false, false, false, false, true}, // 海軍基本回避率(防御回数なし)
            {true, true, true, true, true, true, true}, // 地形特性獲得可能性
            {true, true, true, true, true, true, true}, // 戦闘特性獲得可能性
            {true, true, true, true, true, true, true}, // 地形特性補正
            {false, false, false, false, false, true, true}, // 類似地形特性補正
            {true, true, true, true, true, true, true}, // 戦闘特性補正
            {false, false, false, false, false, false, true}, // 陸軍指揮官スキル補正
            {false, false, false, false, false, false, true}, // 空軍指揮官スキル補正
            {false, false, false, false, false, false, true}, // 海軍指揮官スキル補正
            {true, true, true, true, true, true, true}, // 指揮官死亡確率
            {true, true, true, true, true, false, false}, // 空軍組織率被ダメージ
            {true, true, false, false, false, false, false}, // 空軍戦力被ダメージ(組織力)
            {true, true, false, false, false, false, false}, // 空軍戦力被ダメージ
            {false, false, false, false, false, false, true}, // 陸軍最小組織率被ダメージ
            {false, false, false, false, false, false, true}, // 陸軍組織率被ダメージ(装甲/非装甲同士)
            {false, false, false, false, false, false, true}, // 陸軍組織率被ダメージ(装甲対非装甲)
            {false, false, false, false, false, false, true}, // 陸軍最小戦力被ダメージ
            {false, false, false, false, false, false, true}, // 陸軍戦力被ダメージ(装甲/非装甲同士)
            {false, false, false, false, false, false, true}, // 陸軍戦力被ダメージ(装甲対非装甲)
            {false, false, false, false, false, false, true}, // 空軍最小組織率被ダメージ
            {false, false, false, false, false, false, true}, // 空軍追加組織率被ダメージ
            {false, false, false, false, false, false, true}, // 空軍最小戦力被ダメージ
            {false, false, false, false, false, false, true}, // 空軍追加戦力被ダメージ
            {false, false, false, false, false, false, true}, // 空軍戦力被ダメージ(対塹壕)
            {false, false, false, false, false, false, true}, // 海軍最小組織率被ダメージ
            {false, false, false, false, false, false, true}, // 海軍追加組織率被ダメージ
            {false, false, false, false, false, false, true}, // 海軍最小戦力被ダメージ
            {false, false, false, false, false, false, true}, // 海軍追加戦力被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対陸軍戦力被ダメージ(組織率)
            {false, false, false, false, false, true, true}, // 空軍対陸軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対陸軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 陸軍対陸軍組織率被ダメージ(組織率)
            {false, false, false, false, false, false, true}, // 陸軍対陸軍組織率被ダメージ(都市)
            {false, false, false, false, false, false, true}, // 陸軍対陸軍組織率被ダメージ(要塞)
            {false, false, false, false, false, false, true}, // 必要要塞規模
            {false, false, false, false, false, true, true}, // 陸軍対陸軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対空軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対空軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 陸軍対空軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 陸軍対空軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 海軍対空軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 海軍対空軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 潜水艦対空軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 潜水艦対空軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対海軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対海軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 海軍対海軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 海軍対海軍戦力被ダメージ
            {false, false, false, false, false, true, true}, // 潜水艦対海軍組織率被ダメージ
            {false, false, false, false, false, true, true}, // 潜水艦対海軍戦力被ダメージ
            {false, true, false, false, false, false, false}, // 潜水艦組織率被ダメージ
            {false, true, false, false, false, false, false}, // 潜水艦戦力被ダメージ
            {false, true, false, false, false, true, true}, // 潜水艦発見補正
            {false, false, true, true, true, false, false}, // 空軍対陸軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 空軍対陸軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 砲撃ダメージ補正(陸上部隊)
            {false, false, true, true, true, false, false}, // 砲撃ダメージ補正(インフラ)
            {false, false, true, true, true, false, false}, // 砲撃ダメージ補正(IC)
            {false, false, true, true, true, false, false}, // 砲撃ダメージ補正(資源)
            {false, false, true, true, true, false, false}, // 砲撃中の被攻撃ペナルティ
            {false, false, true, true, true, false, false}, // 砲撃戦力ダメージ
            {false, false, true, true, true, false, false}, // 砲撃組織率ダメージ
            {false, false, true, true, true, false, false}, // 陸軍対陸軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 陸軍対陸軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 陸軍対空軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 陸軍対空軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 海軍対空軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 海軍対空軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 空軍対空軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 空軍対空軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 海軍対海軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 海軍対海軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 空軍対海軍戦力被ダメージ
            {false, false, true, true, true, false, false}, // 空軍対海軍組織率被ダメージ
            {false, false, true, true, true, false, false}, // 給料不足時の戦闘補正
            {false, false, true, true, true, false, false}, // 海軍最小戦闘時間
            {false, false, true, true, true, false, false}, // 陸軍最小戦闘時間
            {false, false, true, true, true, false, false}, // 空軍最小戦闘時間
            {false, false, true, true, true, false, false}, // 陸軍スタックペナルティ
            {false, false, true, true, true, false, false}, // 陸軍移動時組織率減少係数
            {false, false, true, true, true, false, false}, // 空軍移動時組織率減少係数
            {false, false, true, true, true, false, false}, // 海軍移動時組織率減少係数
            {false, false, true, true, true, false, false}, // 遠隔地補給係数
            {false, false, true, true, true, false, false}, // 基礎補給効率
            {false, false, true, true, true, false, false}, // 陸軍組織率補正
            {false, false, true, true, true, false, false}, // 空軍組織率補正
            {false, false, true, true, true, false, false}, // 海軍組織率補正
            {false, false, true, true, true, false, false}, // 核攻撃不満度係数(人的資源)
            {false, false, true, true, true, false, false}, // 核攻撃不満度係数(IC)
            {false, false, true, true, true, false, false}, // 核攻撃不満度係数(トータル)
            {false, false, true, true, true, false, false}, // 陸軍友好地組織率補正
            {false, false, true, true, true, false, false}, // 阻止攻撃備蓄補正
            {false, false, true, true, true, false, false}, // 焦土命令ダメージ
            {false, false, true, true, true, false, false}, // 死守命令不満度上昇
            {false, false, true, true, true, false, false}, // 焦土命令好戦性上昇
            {false, false, true, true, true, false, false}, // 陸軍デフォルトスタック数
            {false, false, true, true, true, false, false}, // 海軍デフォルトスタック数
            {false, false, true, true, true, false, false}, // 空軍デフォルトスタック数
            {false, false, true, true, true, false, false}, // ロケットデフォルトスタック数
            {false, false, true, true, true, false, false}, // 要塞砲撃ダメージ補正
            {false, false, true, true, true, false, false}, // 砲撃組織率減少
            {false, false, true, true, true, false, false}, // 陸軍対要塞ダメージ係数
            {false, false, true, true, true, false, false}, // 空軍基地移動組織率減少係数
            {false, false, true, true, true, false, false}, // 空港占領時ペナルティ
            {false, false, true, true, true, false, false}, // 対空砲戦力ダメージ補正
            {false, false, true, true, true, false, false}, // 対空砲組織率ダメージ補正
            {false, false, true, true, true, false, false}, // 対空砲上空通過ダメージ補正
            {false, false, true, true, true, false, false}, // 対空砲爆撃中ダメージ補正
            {false, false, true, true, true, false, false}, // 装甲ユニット戦力ダメージ補正
            {false, false, true, true, true, false, false}, // 装甲ユニット組織率ダメージ補正
            {false, false, true, true, true, false, false}, // 戦車対人最小突破係数
            {false, false, true, true, true, false, false}, // 戦車対人最大突破係数
            {false, false, true, true, true, false, false}, // 海軍クリティカルヒット確率
            {false, false, true, true, true, false, false}, // 海軍クリティカルヒット効果
            {false, false, true, true, true, false, false}, // 要塞ダメージ補正
            {false, false, true, true, true, false, false}, // 日中港湾攻撃奇襲確率
            {false, false, true, true, true, false, false}, // 夜間港湾攻撃奇襲確率
            {false, false, true, true, true, false, false}, // 港湾攻撃奇襲補正
            {false, false, true, true, true, false, false}, // レーダー奇襲確率減少値
            {false, false, true, true, true, false, false}, // レーダー奇襲効果減少値
            {false, false, false, false, true, false, false}, // 反撃イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 反撃イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 反撃イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 反撃イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 強襲イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 強襲イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 強襲イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 強襲イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 包囲イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 包囲イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 包囲イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 包囲イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 待伏イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 待伏イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 待伏イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 待伏イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 遅延イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 遅延イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 遅延イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 遅延イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 後退イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 後退イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 後退イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 後退イベント攻撃側組織率補正
            {false, false, false, false, true, false, false}, // 突破イベント防御側戦力補正
            {false, false, false, false, true, false, false}, // 突破イベント防御側組織率補正
            {false, false, false, false, true, false, false}, // 突破イベント攻撃側戦力補正
            {false, false, false, false, true, false, false}, // 突破イベント攻撃側組織率補正
            {false, false, false, false, false, true, true}, // 海軍対空砲組織率被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対空砲組織率被ダメージ
            {false, false, false, false, false, true, true}, // 空軍対空砲戦力被ダメージ
            {false, false, false, false, false, true, true}, // 対空砲攻撃ルール
            {false, false, false, false, false, true, true}, // 対空砲夜間攻撃補正
            {false, false, false, false, false, true, true}, // 対空砲攻撃レーダーボーナス
            {false, false, false, false, false, true, true}, // 地形適正移動ボーナス
            {false, false, false, false, false, true, true}, // 類似地形適正移動ボーナス
            {false, false, false, false, false, true, true}, // 兵站管理の補給効率ボーナス
            {false, false, false, false, false, true, true}, // 攻勢継続日数
            {false, false, false, false, false, true, true}, // 閣僚ボーナス適用方法
            {false, false, false, false, false, true, true}, // 友好地組織率回復ボーナス
            {false, false, false, false, false, true, true}, // 友好地組織率回復ボーナス上限
            {false, false, false, false, false, true, true}, // 海上任務中の船団妨害
            {false, false, false, false, false, true, true}, // 輸送艦隊の自動帰還
            {false, false, false, false, false, true, true}, // 単一プロヴィンス/地域指定任務
            {false, false, false, false, false, true, true}, // 冬季夜間時間
            {false, false, false, false, false, true, true}, // 春季/秋季夜間時間
            {false, false, false, false, false, true, true}, // 夏季夜間時間
            {false, false, false, false, false, true, true}, // 陸上部隊到着時刻再計算間隔
            {false, false, false, false, false, true, true}, // 同時到着補正(プレイヤー)
            {false, false, false, false, false, true, true}, // 同時到着補正(AI)
            {false, false, false, false, false, true, true}, // 戦闘後到着時刻再計算
            {false, false, false, false, false, true, true}, // 戦闘時陸軍移動速度補正
            {false, false, false, false, false, true, true}, // 沿岸砲撃時陸軍移動速度補正
            {false, false, false, false, false, true, true}, // 物資切れ時陸軍移動速度補正
            {false, false, false, false, false, true, true}, // 組織率低下時陸軍移動速度補正
            {false, false, false, false, false, true, true}, // 燃料切れ時陸軍/空軍移動速度補正
            {false, false, false, false, false, true, true}, // 燃料切れ時デフォルト移動速度
            {false, false, false, false, false, true, true}, // 艦隊規模航続距離ペナルティ割合
            {false, false, false, false, false, true, true}, // 艦隊規模航続距離ペナルティ閾値
            {false, false, false, false, false, true, true}, // 艦隊規模航続距離ペナルティ上限
            {false, false, false, false, false, true, true}, // 地方/地域内での距離制限適用
            {false, false, false, false, false, true, true}, // レーダー航空機発見ボーナス
            {false, false, false, false, false, true, true}, // 友好地航空機発見ボーナス
            {false, false, false, false, false, true, true}, // 主力艦/補助艦割合修正
            {false, false, false, false, false, true, true}, // 陸軍組織率不足ユニット標的確率
            {false, false, false, false, false, true, true}, // 主力艦/補助艦標的ポジション値
            {false, false, false, false, false, false, true}, // 海戦ポジション値日中ボーナス
            {false, false, false, false, false, false, true}, // 海戦ポジション値スキル補正
            {false, false, false, false, false, false, true}, // 海戦ポジション値艦隊規模補正
            {false, false, false, false, false, false, true}, // 海戦ポジション値艦隊構成補正
            {false, false, false, false, false, false, true}, // 要塞被ダメージ補正
            {false, false, false, false, false, false, true}, // 要塞最大被ダメージ
            {false, false, false, false, false, false, true}, // 付属旅団による最小脆弱性
            {false, false, false, false, false, false, true}, // 自動撤退組織率
            {false, false, false, false, false, false, true}, // 陸軍海上輸送後組織率補正
            {false, false, false, false, false, false, true}, // 最大塹壕値
            {false, false, false, false, false, false, true}, // 1日の塹壕増加量
            {false, false, false, false, false, false, true}, // 突破/包囲最小速度
            {false, false, false, false, false, false, true}, // 突破/包囲最大確率
            {false, false, false, false, false, false, true}, // 突破/包囲確率補正
            {false, false, false, false, false, false, true}, // コンバットイベント継続時間
            {false, false, false, false, false, false, true}, // 反撃イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 反撃イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 反撃イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 反撃イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 強襲イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 強襲イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 強襲イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 強襲イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 包囲イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 包囲イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 包囲イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 包囲イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 待伏イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 待伏イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 待伏イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 待伏イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 遅延イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 遅延イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 遅延イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 遅延イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 後退イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 後退イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 後退イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 後退イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 突破イベント攻撃側組織率補正
            {false, false, false, false, false, false, true}, // 突破イベント攻撃側戦力補正
            {false, false, false, false, false, false, true}, // 突破イベント防御側組織率補正
            {false, false, false, false, false, false, true}, // 突破イベント防御側戦力補正
            {false, false, false, false, false, false, true}, // 司令部は突破イベント時のみ戦力ダメージ
            {false, false, false, false, false, false, true}, // 戦闘モード
            {false, false, false, false, false, true, true}, // 攻撃任務
            {false, false, false, false, false, true, true}, // 攻撃初期効率
            {false, false, false, false, false, true, true}, // 攻撃速度ボーナス
            {false, false, false, false, false, true, true}, // 基地移動任務
            {false, false, false, false, false, true, true}, // 基地移動初期効率
            {false, false, false, false, false, true, true}, // 基地移動被発見確率
            {false, false, false, false, false, true, true}, // 戦略的再配置任務
            {false, false, false, false, false, true, true}, // 戦略的再配置初期効率
            {false, false, false, false, false, true, true}, // 戦略的再配置加算値
            {false, false, false, false, false, true, true}, // 戦略的再配置距離補正
            {false, false, false, false, false, true, true}, // 支援攻撃任務
            {false, false, false, false, false, true, true}, // 支援攻撃初期効率
            {false, false, false, false, false, true, true}, // 支援攻撃速度ボーナス
            {false, false, false, false, false, true, true}, // 防衛支援任務
            {false, false, false, false, false, true, true}, // 防衛支援初期効率
            {false, false, false, false, false, true, true}, // 防衛支援速度ボーナス
            {false, false, false, false, false, true, true}, // 待機任務
            {false, false, false, false, false, true, true}, // 待機初期効率
            {false, false, false, false, false, true, true}, // 待機速度ボーナス
            {false, false, false, false, false, true, true}, // パルチザン掃討任務
            {false, false, false, false, false, true, true}, // パルチザン掃討初期効率
            {false, false, false, false, false, true, true}, // パルチザン掃討制圧力補正
            {false, false, false, false, false, true, true}, // 防衛計画任務
            {false, false, false, false, false, true, true}, // 防衛計画初期効率
            {false, false, false, false, false, true, true}, // 制空権任務
            {false, false, false, false, false, true, true}, // 制空権初期効率
            {false, false, false, false, false, true, true}, // 制空権敵機発見補正
            {false, false, false, false, false, true, true}, // 制空権最小ユニット数
            {false, false, false, false, false, true, true}, // 地上攻撃任務
            {false, false, false, false, false, true, true}, // 地上攻撃初期効率
            {false, false, false, false, false, true, true}, // 地上攻撃組織率ダメージ補正
            {false, false, false, false, false, true, true}, // 地上攻撃戦力ダメージ補正
            {false, false, false, false, false, true, true}, // 阻止攻撃任務
            {false, false, false, false, false, true, true}, // 阻止攻撃初期効率
            {false, false, false, false, false, true, true}, // 阻止攻撃組織率ダメージ補正
            {false, false, false, false, false, true, true}, // 阻止攻撃戦力ダメージ補正
            {false, false, false, false, false, true, true}, // 戦略爆撃任務
            {false, false, false, false, false, true, true}, // 戦略爆撃初期効率
            {false, false, false, false, false, true, true}, // 兵站攻撃任務
            {false, false, false, false, false, true, true}, // 兵站攻撃初期効率
            {false, false, false, false, false, true, true}, // 空港空爆任務
            {false, false, false, false, false, true, true}, // 空港空爆初期効率
            {false, false, false, false, false, true, true}, // 軍事施設攻撃任務
            {false, false, false, false, false, true, true}, // 軍事施設攻撃初期効率
            {false, false, false, false, false, true, true}, // 艦船攻撃任務
            {false, false, false, false, false, true, true}, // 艦船攻撃初期効率
            {false, false, false, false, false, true, true}, // 港湾攻撃任務
            {false, false, false, false, false, true, true}, // 港湾攻撃初期効率
            {false, false, false, false, false, true, true}, // 航空船団爆撃任務
            {false, false, false, false, false, true, true}, // 航空船団爆撃初期効率
            {false, false, false, false, false, true, true}, // 空輸補給任務
            {false, false, false, false, false, true, true}, // 空輸補給初期効率
            {false, false, false, false, false, true, true}, // 空挺強襲任務
            {false, false, false, false, false, true, true}, // 空挺強襲初期効率
            {false, false, false, false, false, true, true}, // 核攻撃任務
            {false, false, false, false, false, true, true}, // 核攻撃初期効率
            {false, false, false, false, false, true, true}, // 航空緊急出撃任務
            {false, false, false, false, false, true, true}, // 航空緊急出撃初期効率
            {false, false, false, false, false, true, true}, // 航空緊急出撃敵機発見補正
            {false, false, false, false, false, true, true}, // 航空緊急出撃最小ユニット数
            {false, false, false, false, false, true, true}, // 船団襲撃任務
            {false, false, false, false, false, true, true}, // 船団襲撃初期効率
            {false, false, false, false, false, true, true}, // 船団襲撃航続距離補正
            {false, false, false, false, false, true, true}, // 船団襲撃被発見確率
            {false, false, false, false, false, true, true}, // 対潜作戦任務
            {false, false, false, false, false, true, true}, // 対潜作戦初期効率
            {false, false, false, false, false, true, true}, // 海上阻止任務
            {false, false, false, false, false, true, true}, // 海上阻止初期効率
            {false, false, false, false, false, true, true}, // 沿岸砲撃任務
            {false, false, false, false, false, true, true}, // 沿岸砲撃初期効率
            {false, false, false, false, false, true, true}, // 沿岸砲撃補正
            {false, false, false, false, false, true, true}, // 強襲上陸任務
            {false, false, false, false, false, true, true}, // 強襲上陸初期効率
            {false, false, false, false, false, true, true}, // 海上輸送任務
            {false, false, false, false, false, true, true}, // 海上輸送初期効率
            {false, false, false, false, false, true, true}, // 海上輸送航続距離補正
            {false, false, false, false, false, true, true}, // 海上輸送被発見確率
            {false, false, false, false, false, true, true}, // 海上戦闘哨戒任務
            {false, false, false, false, false, true, true}, // 海上戦闘哨戒初期効率
            {false, false, false, false, false, true, true}, // 空母による港湾攻撃任務
            {false, false, false, false, false, true, true}, // 空母による港湾攻撃初期効率
            {false, false, false, false, false, true, true}, // 空母による航空基地攻撃任務
            {false, false, false, false, false, true, true}, // 空母による航空基地攻撃初期効率
            {false, false, false, false, false, true, true}, // 隠密移動任務
            {false, false, false, false, false, true, true}, // 隠密移動初期効率
            {false, false, false, false, false, true, true}, // 隠密移動航続距離補正
            {false, false, false, false, false, true, true}, // 隠密移動被発見確率
            {false, false, false, false, false, true, true}, // 海上緊急出撃任務
            {false, false, false, false, false, true, true}, // 海上緊急出撃初期効率
            {false, false, false, false, false, true, true}, // 海上緊急出撃速度ボーナス
            {false, false, false, false, false, false, true}, // 攻撃/支援攻撃効率を戦闘補正として使用
            {false, false, false, false, false, true, true}, // 陸上要塞効率
            {false, false, false, false, false, true, true}, // 沿岸要塞効率
            {false, false, false, false, false, true, true}, // 対地防御効率
            {false, false, false, false, false, true, true}, // 船団防衛効率
            {false, false, false, false, false, true, true}, // 人的資源増加
            {false, false, false, false, false, true, true}, // TC補正
            {false, false, false, false, false, true, true}, // 占領地TC補正
            {false, false, false, false, false, true, true}, // 消耗補正
            {false, false, false, false, false, true, true}, // 人的資源復帰補正
            {false, false, false, false, false, true, true}, // 遠隔地補給補正
            {false, false, false, false, false, true, true}, // 修理補正
            {false, false, false, false, false, true, true}, // 研究補正
            {false, false, false, false, false, true, true}, // レーダー効率
            {false, false, false, false, false, true, true}, // 司令部補給効率ボーナス
            {false, false, false, false, false, true, true}, // 司令部コンバットイベントボーナス
            {false, false, false, false, false, true, true}, // コンバットイベント発生確率
            {false, false, false, false, false, true, true}, // 友軍発見確率
            {false, false, false, false, false, true, true}, // 敵軍発見確率
            {false, false, false, false, false, true, true}, // 友好国諜報確率
            {false, false, false, false, false, true, true}, // 敵国諜報確率
            {false, false, false, false, false, true, true}, // 最大強襲上陸規模
            {false, false, false, false, false, true, true}, // エネルギー/石油変換効率
            {false, false, false, false, false, true, true}, // 総合生産効率
            {false, false, false, false, false, true, true}, // 物資生産効率
            {false, false, false, false, false, true, true}, // 対空砲攻撃力
            {false, false, false, false, false, true, true}, // 空軍奇襲確率
            {false, false, false, false, false, true, true}, // 陸軍奇襲確率
            {false, false, false, false, false, true, true}, // 海軍奇襲確率
            {false, false, false, false, false, true, true}, // 平時IC補正
            {false, false, false, false, false, true, true}, // 戦時IC補正
            {false, false, false, false, false, true, true}, // 建造物生産補正
            {false, false, false, false, false, true, true}, // 輸送船団生産補正
            {false, false, false, false, false, true, true}, // 最小艦船ポジション値
            {false, false, false, false, false, true, true}, // 最大艦船ポジション値
            {false, false, false, false, false, true, true}, // 平時資源備蓄補正
            {false, false, false, false, false, true, true}, // 戦時資源備蓄補正
            {false, false, false, false, false, true, true}, // 平時物資/燃料備蓄補正
            {false, false, false, false, false, true, true}, // 戦時物資/燃料備蓄補正
            {true, true, true, true, true, true, true}, // 青写真ボーナス
            {true, true, true, true, true, true, true}, // 史実年度以前研究ペナルティ
            {false, false, false, false, false, true, true}, // 史実年度以降研究ボーナス
            {true, true, true, true, true, true, true}, // 研究機関レベル毎のコスト
            {true, true, true, true, true, true, true}, // 1年毎の発明イベント平均回数
            {false, false, true, true, true, false, false}, // 史実年度以降研究ボーナス
            {false, false, true, true, true, false, false}, // 研究速度補正
            {false, false, true, true, true, false, false}, // 史実年度以前研究ペナルティ上限
            {false, false, true, true, true, false, false}, // 史実年度以降研究ボーナス上限
            {false, false, false, true, true, false, false}, // 研究スロット上限
            {false, false, false, true, true, false, false}, // 研究スロット毎の必要IC
            {false, false, false, false, true, false, false}, // 最大ランダム補正
            {false, false, false, false, false, true, true}, // 研究ページのレイアウト
            {false, false, false, false, false, false, true}, // 研究概要パネルのスタイル
            {false, false, false, false, false, true, true}, // 研究スロット上限
            {false, false, false, false, false, true, true}, // 研究スロット下限
            {false, false, false, false, false, true, true}, // 研究スロット毎の必要IC
            {false, false, false, false, false, false, true}, // 新規国家でロケット技術を継承
            {false, false, false, false, false, false, true}, // 新規国家で核物理学技術を継承
            {false, false, false, false, false, false, true}, // 新規国家で核工学技術を継承
            {false, false, false, false, false, false, true}, // 新規国家で秘密兵器技術を継承
            {false, false, false, false, false, false, true}, // 最大研究機関スキル
            {false, false, false, false, false, true, true}, // 貿易交渉間隔
            {false, false, false, false, false, true, true}, // ゲーム開始直後の貿易交渉遅延日数
            {false, false, false, false, false, true, true}, // ゲーム開始直後のAI友好国貿易交渉遅延日数
            {false, false, false, false, false, true, true}, // 理想石油備蓄
            {false, false, false, false, false, true, true}, // 危険水準石油備蓄
            {false, false, false, false, false, true, true}, // 理想物資備蓄
            {false, false, false, false, false, true, true}, // 危険水準物資備蓄
            {false, false, false, false, false, true, true}, // 理想資源備蓄
            {false, false, false, false, false, true, true}, // 危険水準資源備蓄
            {false, false, false, false, false, true, true}, // 戦時理想備蓄係数
            {false, false, false, false, false, true, true}, // 平時石油臨時輸入割合
            {false, false, false, false, false, true, true}, // 戦時石油臨時輸入割合
            {false, false, false, false, false, true, true}, // 理想備蓄未到達時の臨時輸入割合
            {false, false, false, false, false, true, true}, // 物資生産割合
            {false, false, false, false, false, true, true}, // 資金生産割合
            {false, false, false, false, false, true, true}, // 備蓄選択時の臨時輸入割合
            {false, false, false, false, false, true, true}, // 貿易協定資源輸送日数
            {false, false, false, false, false, true, true}, // 貿易協定の統合
            {false, false, false, false, false, true, true}, // 手動貿易協定
            {false, false, false, false, false, true, true}, // 傀儡国上納物資/資金
            {false, false, false, false, false, true, true}, // 傀儡国物資危険水準
            {false, false, false, false, false, true, true}, // 傀儡国最大資源備蓄
            {false, false, false, false, false, true, true}, // 新規貿易協定最小効率
            {false, false, false, false, false, true, true}, // 貿易協定破棄効率
            {false, false, false, false, false, true, true}, // 自動/AIの貿易協定最小効率
            {false, false, false, false, false, true, true}, // 理想備蓄未達時の余剰物資生産比率
            {false, false, false, false, false, true, true}, // 戦時余剰物資生産係数
            {false, false, false, false, false, true, true}, // 備蓄余裕時の物資生産禁止係数
            {false, false, false, false, false, true, true}, // 守備隊/民兵の最大連続生産数
            {false, false, false, false, false, true, true}, // 海軍/空軍連続生産最小IC
            {false, false, false, false, false, true, true}, // 新規生産禁止人的資源比率
            {false, false, false, false, false, true, true}, // 新規生産禁止人的資源値
            {false, false, false, false, false, true, true}, // 新規生産禁止物資比率
            {false, false, false, false, false, true, true}, // 総ICに対する軍事力比率(平時)
            {false, false, false, false, false, true, true}, // 総ICに対する軍事力比率(戦時)
            {false, false, false, false, false, true, true}, // 総ICに対する軍事力比率(主要国)
            {false, false, false, false, false, true, true}, // 攻勢中止物資備蓄
            {false, false, false, false, false, true, true}, // 攻勢中止燃料備蓄
            {false, false, false, false, false, true, true}, // 攻勢中止補給効率
            {false, false, false, false, false, true, true}, // 攻勢中止組織率/戦力ダメージ
            {false, false, false, false, false, true, true}, // AIの平時の攻撃的諜報活動
            {false, false, false, false, false, true, true}, // AIの諜報コスト補正
            {false, false, false, false, false, true, true}, // AIの外交コスト補正
            {false, false, false, false, false, true, true}, // AIの外交干渉頻度補正
            {false, false, false, false, false, true, false}, // AIの新宣戦布告ルール
            {false, false, false, false, false, false, true}, // AIの新宣戦布告ルール
            {false, false, false, false, false, true, true}, // 傀儡国が宗主国の同盟に強制加入する中立性
            {false, false, false, false, false, true, true}, // 新AI占領地解放ルール
            {false, false, false, false, false, true, true}, // AIイベント選択ルール
            {false, false, false, false, false, true, true}, // 強制戦略的再配置時間
            {false, false, false, false, false, true, true}, // AI最大再配置日数
            {false, false, false, false, false, true, true}, // 守備隊AIの簡易チェック
            {false, false, false, false, false, true, true}, // AI宗主国が傀儡国の占領地を支配
            {false, false, false, false, false, true, true}, // AI占領地解放最小日数
            {false, false, false, false, false, true, true}, // AI占領地返還最小日数
            {false, false, false, false, false, true, true}, // AI占領地返還最小日数(物資補給基地)
            {false, false, false, false, false, true, true}, // 被領有権主張時連合加盟最小友好度
            {false, false, false, false, false, true, true}, // AIの諜報/外交をログに記録
            {false, false, false, false, false, true, true}, // 国家情報をログに記録
            {false, false, false, false, false, true, true}, // AI切り替えをログに記録
            {false, false, false, false, false, true, true}, // 新自動セーブファイル名
            {false, false, false, false, false, true, true}, // マルチプレイでAI切替時に新しい設定を読み込む
            {false, false, false, false, false, true, true}, // 貿易効率算出間隔
            {false, false, false, false, false, true, true}, // 備蓄庫の再計算間隔
            {false, false, false, false, false, true, false}, // 損失を記録
            {false, false, false, false, false, false, true}, // 損失を記録
            {false, false, false, false, false, true, true}, // 占領地で旅団付属を許可
            {false, false, false, false, false, true, true}, // 陸軍の一括配置数
            {false, false, false, false, false, true, true}, // 海軍の一括配置数
            {false, false, false, false, false, true, true}, // 空軍の一括配置数
            {false, false, false, false, false, true, true}, // すべての陸地プロヴィンスに固有画像を許可
            {false, false, false, false, false, true, true}, // イベント選択肢を委任
            {false, false, false, false, false, true, true}, // イベント選択肢強制表示
            {false, false, false, false, false, true, true}, // ディシジョンを使用する
            {false, false, false, false, false, true, true}, // パルチザンの歩兵構成割合
            {false, false, false, false, false, true, true}, // パルチザンの技術レベル
            {false, false, false, false, false, true, true}, // パルチザン最小戦力
            {false, false, false, false, false, true, true}, // パルチザン最大戦力
            {false, false, false, false, false, true, true}, // パルチザン組織率回復速度
            {false, false, false, false, false, true, true}, // パルチザンボーナス(隣接地占領)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(占領地)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(山岳)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(丘陵)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(森林)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(密林)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(湿地)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(砂漠)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(平地)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(都市)
            {false, false, false, false, false, true, true}, // パルチザンボーナス(航空/海軍基地)
            {false, false, false, false, false, true, true}, // パルチザン占領プロヴィンス返却時間
            {false, false, false, false, false, true, true}, // 新形式閣僚ファイルフォーマット
            {false, false, false, false, false, false, true}, // 閣僚引退年を使用
            {false, false, false, false, false, false, true}, // 指揮官引退年を使用
            {false, false, false, false, false, true, true}, // スプライトをMODDIRのみから読み込む
            {false, false, false, false, false, true, true}, // ユニットアイコンをMODDIRのみから読み込む
            {false, false, false, false, false, true, true}, // ユニット画像をMODDIRのみから読み込む
            {false, false, false, false, false, true, true}, // AIファイルをMODDIRのみから読み込む
            {false, false, false, false, false, true, true}, // 守備隊判定ルール
            {false, false, false, false, false, true, true}, // 旧セーブフォーマットを使用
            {false, false, false, false, false, false, true}, // 生産パネルのUIスタイル
            {false, false, false, false, false, false, true}, // ユニット画像のサイズ
            {false, false, false, false, false, false, true}, // 艦艇付属装備に画像を使用
            {false, false, false, false, false, false, true}, // 建物をプロヴィンスでのみ建造
            {false, false, false, false, false, false, true}, // ユニット補正ページの新スタイル移行閾値
            {false, false, false, false, false, true, true}, // マップ番号
            {false, false, false, false, false, true, true}, // 総プロヴィンス数
            {false, false, false, false, false, true, true}, // 距離算出方法
            {false, false, false, false, false, true, true}, // マップの幅
            {false, false, false, false, false, true, true} // マップの高さ
        };

        /// <summary>
        ///     項目の型
        /// </summary>
        public static MiscItemType[] ItemTypes =
        {
            MiscItemType.NonNegDbl2AoD, // ICからTCへの変換効率
            MiscItemType.NonNegDbl, // ICから物資への変換効率
            MiscItemType.NonNegDbl, // ICから消費財への変換効率
            MiscItemType.NonNegDbl, // ICから資金への変換効率
            MiscItemType.NonNegDbl, // 不満度減少速度
            MiscItemType.NonNegDbl, // 最小実効ICの比率
            MiscItemType.NonNegInt, // 最小実効IC
            MiscItemType.NonNegDbl, // 不満度低下補正
            MiscItemType.NonNegDbl2, // 最大ギアリングボーナス
            MiscItemType.NonNegDbl2, // ギアリングボーナスの増加値
            MiscItemType.NonNegDbl, // 連続生産時の資源消費増加
            MiscItemType.NonNegDbl, // IC不足時のギアリングボーナス減少値
            MiscItemType.NonNegDbl, // 非中核州のIC補正
            MiscItemType.NonNegDbl, // 占領地のIC補正
            MiscItemType.NonNegDbl, // 属国のIC補正
            MiscItemType.NonNegDbl, // 非中核州の資源補正
            MiscItemType.NonNegDbl, // 占領地の資源補正
            MiscItemType.NonNegDblMinusOne, // 非中核州の資源補正(AI)
            MiscItemType.NonNegDbl, // 属国の資源補正
            MiscItemType.NonNegDbl, // 未配備師団のTC負荷
            MiscItemType.NonNegDbl, // 占領地のTC負荷
            MiscItemType.NonNegDbl, // 陸軍師団のTC負荷補正
            MiscItemType.NonNegDbl, // 空軍師団のTC負荷補正
            MiscItemType.NonNegDbl, // 海軍師団のTC負荷補正
            MiscItemType.NonNegDbl, // パルチザンのTC負荷
            MiscItemType.NonNegDbl, // 攻勢時のTC負荷係数
            MiscItemType.NonNegDbl, // プロヴィンス開発のTC負荷
            MiscItemType.NonNegDbl, // 未配備の基地のTC負荷
            MiscItemType.NonNegDbl, // 中核州の人的資源補正
            MiscItemType.NonNegDbl, // 非中核州の人的資源補正
            MiscItemType.NonNegDbl, // 海外州の人的資源補正
            MiscItemType.NonNegDbl, // 属国の人的資源補正
            MiscItemType.NonNegDbl2Dh103Full2, // 戦時の海外州の人的資源補正
            MiscItemType.NonNegDbl2Dh103Full2, // 平時の人的資源補正
            MiscItemType.NonNegDbl2Dh103Full2, // 戦時の人的資源補正
            MiscItemType.NonNegDbl, // 人的資源の老化率
            MiscItemType.NonNegDbl, // 政策スライダーに影響を与えるためのIC比率
            MiscItemType.NonNegDbl, // 戦闘による損失からの復帰係数
            MiscItemType.NonNegDbl, // 補充に必要な人的資源の比率
            MiscItemType.NonNegDbl0, // 補充に必要なICの比率
            MiscItemType.NonNegDbl0, // 補充に必要な時間の比率
            MiscItemType.NonNegDbl0, // 改良に必要なICの比率
            MiscItemType.NonNegDbl0, // 改良に必要な時間の比率
            MiscItemType.NonNegDbl, // 改良のための補充係数
            MiscItemType.NonNegDbl0, // ナショナリズムの初期値
            MiscItemType.NonNegDbl, // 人的資源によるナショナリズムの補正値
            MiscItemType.NonNegDbl, // 人的資源によるナショナリズムの補正値
            MiscItemType.NonNegInt, // ナショナリズム最大値
            MiscItemType.RangedInt, // 最大反乱率
            MiscItemType.NonNegDbl4Dda13, // 月ごとのナショナリズムの減少値
            MiscItemType.NonNegInt, // 師団譲渡後配備可能になるまでの時間
            MiscItemType.NonNegDbl, // 未配備旅団のTC負荷
            MiscItemType.Bool, // 非同盟国に師団を売却/譲渡
            MiscItemType.NonNegInt, // 諜報任務の間隔
            MiscItemType.NonNegInt, // 諜報レベルの増加間隔
            MiscItemType.RangedInt, // 国内の諜報活動を発見する確率
            MiscItemType.RangedInt, // 諜報任務発覚時の友好度低下量
            MiscItemType.Enum, // 第三国の諜報活動を報告
            MiscItemType.RangedDbl, // 諜報任務の隣国補正
            MiscItemType.RangedInt, // 情報の正確さ補正
            MiscItemType.Enum, // 平時のAIの攻撃的諜報活動
            MiscItemType.NonNegInt, // 諜報コスト補正の最大IC
            MiscItemType.NonNegDbl, // AIの諜報コスト補正
            MiscItemType.NonNegDbl, // AIの外交コスト補正
            MiscItemType.NonNegDbl, // AIの外交干渉頻度補正
            MiscItemType.NonNegDbl2, // 建物修復コスト補正
            MiscItemType.NonNegDbl, // 建物修復時間補正
            MiscItemType.NonNegInt, // プロヴィンス効率上昇時間
            MiscItemType.NonNegInt, // 中核プロヴィンス効率上昇時間
            MiscItemType.NonNegDbl, // ライン維持コスト補正
            MiscItemType.NonNegInt, // ライン開始時間
            MiscItemType.NonNegInt, // ライン改良時間
            MiscItemType.NonNegDbl, // ライン調整コスト補正
            MiscItemType.NonNegDbl, // ライン調整資源補正
            MiscItemType.NonNegDbl, // 人的資源老化補正
            MiscItemType.NonNegDbl, // 船団襲撃時物資使用量補正
            MiscItemType.NonNegDbl, // 海軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 海軍の移動時物資使用量補正
            MiscItemType.NonNegDbl, // 海軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍の移動時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍の爆撃時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の移動時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の砲撃時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の物資備蓄量
            MiscItemType.NonNegDbl, // 空軍の物資備蓄量
            MiscItemType.NonNegDbl, // 海軍の物資備蓄量
            MiscItemType.NonNegDbl, // 陸軍の物資再備蓄速度
            MiscItemType.NonNegDbl, // 空軍の物資再備蓄速度
            MiscItemType.NonNegDbl, // 海軍の物資再備蓄速度
            MiscItemType.NonNegDbl, // 合成石油変換係数
            MiscItemType.NonNegDbl, // 合成希少資源変換係数
            MiscItemType.NonNegDbl, // 軍隊の給料
            MiscItemType.NonNegDbl2, // 最大諜報費比率
            MiscItemType.NonNegDbl, // 最大研究費比率
            MiscItemType.NonNegDbl, // 軍隊の給料不足時の消耗補正
            MiscItemType.NonNegDbl, // 軍隊の給料不足時の不満度補正
            MiscItemType.NonNegDbl, // 原子炉維持コスト
            MiscItemType.NonNegDbl, // 原子力発電所維持コスト
            MiscItemType.NonNegDbl, // 合成石油工場維持コスト
            MiscItemType.NonNegDbl, // 合成希少資源工場維持コスト
            MiscItemType.NonNegDbl2, // 海軍情報の存続期間
            MiscItemType.NonNegDbl2, // 船団攻撃回避時間
            MiscItemType.NonNegDbl2, // 船団攻撃妨害時間
            MiscItemType.NonNegInt, // 自動貿易に必要な輸送船団割合
            MiscItemType.NonNegDbl, // 諜報維持コスト
            MiscItemType.NonNegDbl, // スパイ発見確率
            MiscItemType.NonNegDbl, // 不満度によるクーデター成功率修正
            MiscItemType.NonNegDbl, // インフラによるプロヴィンス効率補正
            MiscItemType.NonNegDbl, // 人的資源の消費財生産補正
            MiscItemType.NonNegInt, // スライダー移動の間隔
            MiscItemType.NonNegInt, // 海外プロヴィンスへの配置の必要IC
            MiscItemType.NonNegDbl, // 原子力発電量
            MiscItemType.NonNegDbl, // インフラの自然回復率
            MiscItemType.NonNegInt, // スライダー移動時の最大不満度
            MiscItemType.NonNegInt, // スライダー移動時の最小不満度
            MiscItemType.NonNegInt, // スライダー移動可能な最大不満度
            MiscItemType.NonNegDbl, // 工場集中ボーナス
            MiscItemType.NonNegInt, // 輸送艦変換係数
            MiscItemType.NonNegInt, // 輸送船団変換係数
            MiscItemType.NonNegInt, // 護衛船団変換係数
            MiscItemType.NonNegInt, // 閣僚変更遅延日数
            MiscItemType.NonNegInt, // 閣僚変更遅延日数(イベント)
            MiscItemType.NonNegInt, // 国策変更遅延日数
            MiscItemType.NonNegInt, // 国策変更遅延日数(イベント)
            MiscItemType.NonNegInt, // 指揮官変更遅延日数
            MiscItemType.NonNegInt, // 国策変更時の不満度上昇量
            MiscItemType.NonNegInt, // 閣僚変更時の不満度上昇量
            MiscItemType.NonNegInt, // 反乱が発生する最低不満度
            MiscItemType.NonNegDbl, // 不満度による反乱軍発生率係数
            MiscItemType.NonNegInt, // 輸送艦最大付属装備数
            MiscItemType.NonNegInt, // 潜水艦最大付属装備数
            MiscItemType.NonNegInt, // 原子力潜水艦最大付属装備数
            MiscItemType.NonNegInt, // 駆逐艦最大付属装備数
            MiscItemType.NonNegInt, // 軽巡洋艦最大付属装備数
            MiscItemType.NonNegInt, // 重巡洋艦最大付属装備数
            MiscItemType.NonNegInt, // 巡洋戦艦最大付属装備数
            MiscItemType.NonNegInt, // 戦艦最大付属装備数
            MiscItemType.NonNegInt, // 軽空母最大付属装備数
            MiscItemType.NonNegInt, // 空母最大付属装備数
            MiscItemType.Bool, // プレイヤーの国策変更を許可
            MiscItemType.Enum, // 非同盟国に師団を売却
            MiscItemType.Enum, // 非同盟国に青写真を売却
            MiscItemType.Enum, // 非同盟国にプロヴィンスを売却
            MiscItemType.Bool, // 占領中の同盟国の中核州返還
            MiscItemType.NonNegDbl, // 建物修復速度補正
            MiscItemType.NonNegDbl, // 資源回復速度補正
            MiscItemType.NonNegDblMinusOne, // 資源備蓄上限補正
            MiscItemType.NonNegDblMinusOne, // 物資/燃料備蓄上限補正
            MiscItemType.RangedDbl, // 超過備蓄損失割合
            MiscItemType.NonNegInt, // 資源備蓄上限値
            MiscItemType.NonNegInt, // 物資/燃料備蓄上限値
            MiscItemType.NonNegInt, // 理想物資/燃料備蓄比率
            MiscItemType.NonNegInt, // 最大人的資源
            MiscItemType.NonNegDbl, // 船団輸送能力
            MiscItemType.NonNegDbl, // 陸軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 陸軍の待機時燃料使用量補正
            MiscItemType.NonNegDbl, // 陸軍の戦闘時燃料使用量補正
            MiscItemType.NonNegDbl, // 空軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 空軍/海軍の待機時燃料使用量補正
            MiscItemType.NonNegDbl, // 空軍の戦闘時燃料使用量補正
            MiscItemType.NonNegDbl, // 海軍の待機時物資使用量補正
            MiscItemType.NonNegDbl, // 海軍の戦闘時物資使用量補正
            MiscItemType.NonNegDbl, // 海軍の非移動時燃料使用量補正
            MiscItemType.NonNegDbl, // 海軍の戦闘時燃料使用量補正
            MiscItemType.NonNegInt, // 輸送艦の輸送船団への変換比率
            MiscItemType.NonNegInt, // 駆逐艦の護衛船団への変換比率
            MiscItemType.NonNegInt, // 軽巡洋艦の護衛船団への変換比率
            MiscItemType.NonNegInt, // 軽空母の護衛船団への変換比率
            MiscItemType.Bool, // 生産ラインの編集
            MiscItemType.RangedDbl, // ユニット改良時のギアリングボーナス減少比率
            MiscItemType.RangedDbl, // 旅団改良時のギアリングボーナス減少比率
            MiscItemType.NonNegDbl, // 中核州核攻撃時の不満度上昇係数
            MiscItemType.RangedDbl, // 物資/消費財不足時の最大不満度上昇値
            MiscItemType.NonNegDbl, // 核兵器生産補正
            MiscItemType.Enum, // 同盟国に対する船団システム
            MiscItemType.NonNegDblMinusOne1, // 不要な資源/燃料の回収比率
            MiscItemType.NonNegInt, // 諜報任務の間隔
            MiscItemType.NonNegInt, // 諜報レベルの増加間隔
            MiscItemType.RangedInt, // 国内の諜報活動を発見する確率
            MiscItemType.RangedInt, // 諜報任務発覚時の友好度低下量
            MiscItemType.Int, // 諜報任務の距離補正
            MiscItemType.RangedDbl, // 諜報任務の隣国補正
            MiscItemType.NonNegInt, // 諜報レベルの距離補正
            MiscItemType.NonNegInt, // 諜報レベル10超過時の距離補正
            MiscItemType.RangedInt, // 情報の正確さ補正
            MiscItemType.NonNegInt, // 諜報コストのIC補正
            MiscItemType.NonNegInt, // 諜報コスト補正の最小IC
            MiscItemType.NonNegInt, // 諜報コスト補正の最大IC
            MiscItemType.NonNegDbl, // 諜報レベル10超過時追加維持コスト
            MiscItemType.NonNegInt, // 諜報レベル10超過時増加コスト
            MiscItemType.Enum, // 第三国の諜報活動を報告
            MiscItemType.NonNegDbl, // 諜報資金割り当て補正
            MiscItemType.NonNegInt, // 外交官派遣間隔
            MiscItemType.NonNegInt, // スライダー移動の間隔
            MiscItemType.NonNegDbl, // 政策スライダーに影響を与えるためのIC比率
            MiscItemType.Bool, // 閣僚交代時に閣僚特性を適用する
            MiscItemType.RangedInt, // 貿易キャンセル時の友好度低下
            MiscItemType.RangedIntMinusOne, // 永久貿易キャンセル時の友好度低下
            MiscItemType.Bool, // 属国が宗主国の同盟に強制参加する
            MiscItemType.Bool, // 属国の属国が設立できるか
            MiscItemType.Bool, // 領有権主張の変更
            MiscItemType.NonNegDbl, // 領有権主張時の好戦性上昇値
            MiscItemType.NonPosDbl, // 領有権撤回時の好戦性減少値
            MiscItemType.Bool, // 宣戦布告された時に対抗陣営へ自動加盟
            MiscItemType.Enum, // 国家元首/政府首班の交代
            MiscItemType.Bool, // クーデター発生時に兄弟国へ変更
            MiscItemType.Enum, // 独立可能国設定
            MiscItemType.NonNegDbl, // 陸軍経験値入手係数
            MiscItemType.NonNegDbl, // 海軍経験値入手係数
            MiscItemType.NonNegDbl, // 空軍経験値入手係数
            MiscItemType.NonNegDbl, // 空軍空戦時経験値入手係数
            MiscItemType.NonNegDbl, // 師団経験値入手係数
            MiscItemType.NonNegDbl, // 指揮官経験値入手係数
            MiscItemType.NonNegDbl, // 消耗係数
            MiscItemType.NonNegInt, // 無補給時の自然条件消耗係数
            MiscItemType.NonNegInt, // 無補給時の消耗係数
            MiscItemType.NonNegDbl, // 基地戦闘補正
            MiscItemType.NonNegDbl, // 艦砲射撃戦闘補正
            MiscItemType.NonNegDbl, // 艦砲射撃戦闘効率上限
            MiscItemType.NonPosDbl, // 強襲上陸ペナルティ
            MiscItemType.NonPosDbl, // 側面攻撃ペナルティ
            MiscItemType.NonNegDbl2AoD, // 攻撃側諸兵科連合ボーナス
            MiscItemType.NonNegDbl2AoD, // 防御側諸兵科連合ボーナス
            MiscItemType.NonPosDbl, // 奇襲攻撃ペナルティ
            MiscItemType.NonPosDbl, // 陸軍指揮上限ペナルティ
            MiscItemType.NonPosDbl, // 空軍指揮上限ペナルティ
            MiscItemType.NonPosDbl, // 海軍指揮上限ペナルティ
            MiscItemType.NonPosDbl, // 多方面攻撃補正
            MiscItemType.NonPosDbl, // 包囲攻撃ペナルティ
            MiscItemType.NonPosDbl, // 要塞攻撃ペナルティ
            MiscItemType.NonPosDbl, // 沿岸要塞攻撃ペナルティ
            MiscItemType.NonPosDbl2Dh103Full, // 装甲ユニットの都市攻撃ペナルティ
            MiscItemType.NonPosDbl, // 国民不満度ペナルティ
            MiscItemType.NonPosDbl, // 補給不足ペナルティ
            MiscItemType.NonPosDbl, // 陸軍物資不足ペナルティ
            MiscItemType.NonPosDbl, // 空軍物資不足ペナルティ
            MiscItemType.NonPosDbl, // 海軍物資不足ペナルティ
            MiscItemType.NonPosDbl, // 陸軍燃料不足ペナルティ
            MiscItemType.NonPosDbl, // 空軍燃料不足ペナルティ
            MiscItemType.NonPosDbl, // 海軍燃料不足ペナルティ
            MiscItemType.Dbl, // レーダー補正
            MiscItemType.NonNegDbl, // レーダー/対空砲複合補正
            MiscItemType.NonNegDbl2, // 爆撃機迎撃ボーナス
            MiscItemType.NonPosDbl, // 空軍スタックペナルティ
            MiscItemType.NonPosDbl2, // 空軍スタックペナルティ
            MiscItemType.NonPosDbl, // 海軍スタックペナルティ
            MiscItemType.PosInt, // 陸軍元帥指揮上限
            MiscItemType.PosInt, // 陸軍大将指揮上限
            MiscItemType.PosInt, // 陸軍中将指揮上限
            MiscItemType.PosInt, // 陸軍少将指揮上限
            MiscItemType.PosInt, // 空軍元帥指揮上限
            MiscItemType.PosInt, // 空軍大将指揮上限
            MiscItemType.PosInt, // 空軍中将指揮上限
            MiscItemType.PosInt, // 空軍少将指揮上限
            MiscItemType.PosInt, // 海軍元帥指揮上限
            MiscItemType.PosInt, // 海軍大将指揮上限
            MiscItemType.PosInt, // 海軍中将指揮上限
            MiscItemType.PosInt, // 海軍少将指揮上限
            MiscItemType.PosInt, // 司令部指揮上限係数
            MiscItemType.NonNegDbl, // 輸送船団護衛係数
            MiscItemType.NonPosInt, // 輸送船団護衛モデル
            MiscItemType.NonNegInt, // 戦闘後命令遅延時間
            MiscItemType.NonNegInt, // 陸軍命令遅延時間
            MiscItemType.NonNegInt, // 海軍命令遅延時間
            MiscItemType.NonNegInt, // 空軍命令遅延時間
            MiscItemType.PosInt, // 空軍最大スタックサイズ
            MiscItemType.NonNegInt, // 空戦最小戦闘時間
            MiscItemType.NonNegInt, // 港湾攻撃最小戦闘時間
            MiscItemType.NonNegInt, // 戦略爆撃最小戦闘時間
            MiscItemType.NonNegInt, // 地上爆撃最小戦闘時間
            MiscItemType.NonNegDbl, // 経験値補正
            MiscItemType.NonNegDbl, // 海軍基地戦略爆撃係数
            MiscItemType.NonNegDbl, // 空軍基地戦略爆撃係数
            MiscItemType.NonNegDbl, // 対空砲戦略爆撃係数
            MiscItemType.NonNegDbl, // ロケット試験場戦略爆撃係数
            MiscItemType.NonNegDbl, // 原子炉戦略爆撃係数
            MiscItemType.NonNegDbl, // レーダー戦略爆撃係数
            MiscItemType.NonNegDbl, // インフラ戦略爆撃係数
            MiscItemType.NonNegDbl, // IC戦略爆撃係数
            MiscItemType.NonNegDbl, // 資源戦略爆撃係数
            MiscItemType.NonNegDbl, // 合成石油工場戦略爆撃係数
            MiscItemType.NonNegDbl, // 対地防御効率補正
            MiscItemType.NonNegDbl, // 基本回避率(防御回数あり)
            MiscItemType.NonNegDbl, // 基本回避率(防御回数なし)
            MiscItemType.NonNegDbl, // 陸軍基本回避率(防御回数あり)
            MiscItemType.NonNegDbl, // 空軍基本回避率(防御回数あり)
            MiscItemType.NonNegDbl, // 海軍基本回避率(防御回数あり)
            MiscItemType.NonNegDbl, // 陸軍基本回避率(防御回数なし)
            MiscItemType.NonNegDbl, // 空軍基本回避率(防御回数なし)
            MiscItemType.NonNegDbl, // 海軍基本回避率(防御回数なし)
            MiscItemType.NonNegInt, // 地形特性獲得可能性
            MiscItemType.NonNegInt, // 戦闘特性獲得可能性
            MiscItemType.NonNegDbl, // 地形特性補正
            MiscItemType.NonNegDbl, // 類似地形特性補正
            MiscItemType.NonNegDbl, // 戦闘特性補正
            MiscItemType.NonNegDbl, // 陸軍指揮官スキル補正
            MiscItemType.NonNegDbl, // 空軍指揮官スキル補正
            MiscItemType.NonNegDbl, // 海軍指揮官スキル補正
            MiscItemType.NonNegDbl, // 指揮官死亡確率
            MiscItemType.NonNegDbl, // 空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍戦力被ダメージ(組織力)
            MiscItemType.NonNegDbl, // 空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 陸軍最小組織率被ダメージ
            MiscItemType.NonNegInt, // 陸軍組織率被ダメージ(装甲/非装甲同士)
            MiscItemType.NonNegInt, // 陸軍組織率被ダメージ(装甲対非装甲)
            MiscItemType.NonNegDbl, // 陸軍最小戦力被ダメージ
            MiscItemType.NonNegInt, // 陸軍戦力被ダメージ(装甲/非装甲同士)
            MiscItemType.NonNegInt, // 陸軍戦力被ダメージ(装甲対非装甲)
            MiscItemType.NonNegDbl, // 空軍最小組織率被ダメージ
            MiscItemType.NonNegInt, // 空軍追加組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍最小戦力被ダメージ
            MiscItemType.NonNegInt, // 空軍追加戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍戦力被ダメージ(対塹壕)
            MiscItemType.NonNegDbl, // 海軍最小組織率被ダメージ
            MiscItemType.NonNegInt, // 海軍追加組織率被ダメージ
            MiscItemType.NonNegDbl, // 海軍最小戦力被ダメージ
            MiscItemType.NonNegInt, // 海軍追加戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍対陸軍戦力被ダメージ(組織率)
            MiscItemType.NonNegDbl, // 空軍対陸軍組織率被ダメージ
            MiscItemType.NonNegDbl2Dh103Full, // 空軍対陸軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 陸軍対陸軍組織率被ダメージ(組織率)
            MiscItemType.RangedDbl, // 陸軍対陸軍組織率被ダメージ(都市)
            MiscItemType.RangedDbl, // 陸軍対陸軍組織率被ダメージ(要塞)
            MiscItemType.NonNegDbl, // 必要要塞規模
            MiscItemType.NonNegDbl, // 陸軍対陸軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl2, // 陸軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 陸軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 海軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 海軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 潜水艦対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 潜水艦対空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍対海軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対海軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 海軍対海軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 海軍対海軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 潜水艦対海軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 潜水艦対海軍戦力被ダメージ
            MiscItemType.NonNegInt, // 潜水艦組織率被ダメージ
            MiscItemType.NonNegInt, // 潜水艦戦力被ダメージ
            MiscItemType.RangedDbl, // 潜水艦発見補正
            MiscItemType.NonNegDbl2, // 空軍対陸軍組織率被ダメージ
            MiscItemType.NonNegDbl2, // 空軍対陸軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 砲撃ダメージ補正(陸上部隊)
            MiscItemType.NonNegDbl, // 砲撃ダメージ補正(インフラ)
            MiscItemType.NonNegDbl, // 砲撃ダメージ補正(IC)
            MiscItemType.NonNegDbl, // 砲撃ダメージ補正(資源)
            MiscItemType.NonPosDbl, // 砲撃中の被攻撃ペナルティ
            MiscItemType.NonNegDbl2, // 砲撃戦力ダメージ
            MiscItemType.NonNegDbl2, // 砲撃組織率ダメージ
            MiscItemType.NonNegDbl2, // 陸軍対陸軍戦力被ダメージ
            MiscItemType.NonNegDbl2, // 陸軍対陸軍組織率被ダメージ
            MiscItemType.NonNegDbl2, // 陸軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl2, // 陸軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 海軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 海軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 海軍対海軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 海軍対海軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対海軍戦力被ダメージ
            MiscItemType.NonNegDbl, // 空軍対海軍組織率被ダメージ
            MiscItemType.NonNegDbl, // 給料不足時の戦闘補正
            MiscItemType.NonNegDbl, // 海軍最小戦闘時間
            MiscItemType.NonNegDbl, // 陸軍最小戦闘時間
            MiscItemType.NonNegDbl, // 空軍最小戦闘時間
            MiscItemType.NonNegDblMinusOne, // 陸軍スタックペナルティ
            MiscItemType.NonNegDbl, // 陸軍移動時組織率減少係数
            MiscItemType.NonNegDbl, // 空軍移動時組織率減少係数
            MiscItemType.NonNegDbl, // 海軍移動時組織率減少係数
            MiscItemType.NonNegDbl, // 遠隔地補給係数
            MiscItemType.NonNegDbl, // 基礎補給効率
            MiscItemType.NonNegDbl, // 陸軍組織率補正
            MiscItemType.NonNegDbl, // 空軍組織率補正
            MiscItemType.NonNegDbl, // 海軍組織率補正
            MiscItemType.NonNegDbl, // 核攻撃不満度係数(人的資源)
            MiscItemType.NonNegDbl, // 核攻撃不満度係数(IC)
            MiscItemType.NonNegDbl, // 核攻撃不満度係数(トータル)
            MiscItemType.NonNegDbl, // 陸軍友好地組織率補正
            MiscItemType.NonNegDbl, // 阻止攻撃備蓄補正
            MiscItemType.NonNegDbl, // 焦土命令ダメージ
            MiscItemType.NonNegDbl, // 死守命令不満度上昇
            MiscItemType.NonNegDbl, // 焦土命令好戦性上昇
            MiscItemType.PosInt, // 陸軍デフォルトスタック数
            MiscItemType.PosInt, // 海軍デフォルトスタック数
            MiscItemType.PosInt, // 空軍デフォルトスタック数
            MiscItemType.PosInt, // ロケットデフォルトスタック数
            MiscItemType.NonNegDbl, // 要塞砲撃ダメージ補正
            MiscItemType.NonNegDbl, // 砲撃組織率減少
            MiscItemType.NonNegDbl, // 陸軍対要塞ダメージ係数
            MiscItemType.NonNegDbl, // 空軍基地移動組織率減少係数
            MiscItemType.NonPosInt, // 空港占領時ペナルティ
            MiscItemType.NonNegDbl0, // 対空砲戦力ダメージ補正
            MiscItemType.NonNegDbl0, // 対空砲組織率ダメージ補正
            MiscItemType.NonNegDbl0, // 対空砲上空通過ダメージ補正
            MiscItemType.NonNegDbl0, // 対空砲爆撃中ダメージ補正
            MiscItemType.NonNegDbl0, // 装甲ユニット戦力ダメージ補正
            MiscItemType.NonNegDbl0, // 装甲ユニット組織率ダメージ補正
            MiscItemType.NonNegDbl, // 戦車対人最小突破係数
            MiscItemType.NonNegDbl, // 戦車対人最大突破係数
            MiscItemType.NonNegInt, // 海軍クリティカルヒット確率
            MiscItemType.NonNegInt, // 海軍クリティカルヒット効果
            MiscItemType.NonNegDbl, // 要塞ダメージ補正
            MiscItemType.NonNegInt, // 日中港湾攻撃奇襲確率
            MiscItemType.NonNegInt, // 夜間港湾攻撃奇襲確率
            MiscItemType.NonNegDbl, // 港湾攻撃奇襲補正
            MiscItemType.NonNegDbl, // レーダー奇襲確率減少値
            MiscItemType.NonNegDbl, // レーダー奇襲効果減少値
            MiscItemType.NonNegDbl, // 反撃イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 反撃イベント防御側組織率補正
            MiscItemType.NonNegDbl2, // 反撃イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 反撃イベント攻撃側組織率補正
            MiscItemType.NonNegDbl2, // 強襲イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 強襲イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 強襲イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 強襲イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 包囲イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 包囲イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 包囲イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 包囲イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 待伏イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 待伏イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 待伏イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 待伏イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 遅延イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 遅延イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 遅延イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 遅延イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 後退イベント防御側戦力補正
            MiscItemType.NonNegDbl2, // 後退イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 後退イベント攻撃側戦力補正
            MiscItemType.NonNegDbl2, // 後退イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 突破イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 突破イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 突破イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 突破イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 海軍対空砲組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空砲組織率被ダメージ
            MiscItemType.NonNegDbl, // 空軍対空砲戦力被ダメージ
            MiscItemType.Enum, // 対空砲攻撃ルール
            MiscItemType.RangedDbl0, // 対空砲夜間攻撃補正
            MiscItemType.NonNegDbl0, // 対空砲攻撃レーダーボーナス
            MiscItemType.NonNegDbl, // 地形適正移動ボーナス
            MiscItemType.NonNegDbl, // 類似地形適正移動ボーナス
            MiscItemType.NonNegDbl, // 兵站管理の補給効率ボーナス
            MiscItemType.NonNegInt, // 攻勢継続日数
            MiscItemType.Enum, // 閣僚ボーナス適用方法
            MiscItemType.NonNegDbl, // 友好地組織率回復ボーナス
            MiscItemType.NonNegDbl, // 友好地組織率回復ボーナス上限
            MiscItemType.Enum, // 海上任務中の船団妨害
            MiscItemType.Enum, // 輸送艦隊の自動帰還
            MiscItemType.Bool, // 単一プロヴィンス/地域指定任務
            MiscItemType.RangedDbl, // 冬季夜間時間
            MiscItemType.RangedDbl, // 春季/秋季夜間時間
            MiscItemType.RangedDbl, // 夏季夜間時間
            MiscItemType.NonNegInt, // 陸上部隊到着時刻再計算間隔
            MiscItemType.NonNegDbl, // 同時到着補正(プレイヤー)
            MiscItemType.NonNegDbl, // 同時到着補正(AI)
            MiscItemType.Enum, // 戦闘後到着時刻再計算
            MiscItemType.NonNegDbl, // 戦闘時陸軍移動速度補正
            MiscItemType.NonNegDbl, // 沿岸砲撃時陸軍移動速度補正
            MiscItemType.NonNegDbl, // 物資切れ時陸軍移動速度補正
            MiscItemType.NonNegDbl, // 組織率低下時陸軍移動速度補正
            MiscItemType.NonNegDbl, // 燃料切れ時陸軍/空軍移動速度補正
            MiscItemType.NonNegIntMinusOne, // 燃料切れ時デフォルト移動速度
            MiscItemType.RangedDbl, // 艦隊規模航続距離ペナルティ割合
            MiscItemType.NonNegIntMinusOne, // 艦隊規模航続距離ペナルティ閾値
            MiscItemType.RangedDbl, // 艦隊規模航続距離ペナルティ上限
            MiscItemType.Enum, // 地方/地域内での距離制限適用
            MiscItemType.NonNegDbl, // レーダー航空機発見ボーナス
            MiscItemType.NonNegInt, // 友好地航空機発見ボーナス
            MiscItemType.NonNegDbl, // 主力艦/補助艦割合修正
            MiscItemType.RangedInt, // 陸軍組織率不足ユニット標的確率
            MiscItemType.NonNegIntMinusOne, // 主力艦/補助艦標的ポジション値
            MiscItemType.NonNegDbl, // 海戦ポジション値日中ボーナス
            MiscItemType.NonNegDbl, // 海戦ポジション値スキル補正
            MiscItemType.NonPosDbl, // 海戦ポジション値艦隊規模補正
            MiscItemType.RangedDbl, // 海戦ポジション値艦隊構成補正
            MiscItemType.NonNegDbl, // 要塞被ダメージ補正
            MiscItemType.NonNegDbl, // 要塞最大被ダメージ
            MiscItemType.NonNegDbl, // 付属旅団による最小脆弱性
            MiscItemType.NonNegDbl, // 自動撤退組織率
            MiscItemType.NonNegDbl, // 陸軍海上輸送後組織率補正
            MiscItemType.NonNegInt1, // 最大塹壕値
            MiscItemType.NonNegDbl, // 1日の塹壕増加量
            MiscItemType.NonNegDbl, // 突破/包囲最小速度
            MiscItemType.NonNegDbl, // 突破/包囲最大確率
            MiscItemType.NonNegDbl, // 突破/包囲確率補正
            MiscItemType.NonNegInt, // コンバットイベント継続時間
            MiscItemType.NonNegDbl, // 反撃イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 反撃イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 反撃イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 反撃イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 強襲イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 強襲イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 強襲イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 強襲イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 包囲イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 包囲イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 包囲イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 包囲イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 待伏イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 待伏イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 待伏イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 待伏イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 遅延イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 遅延イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 遅延イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 遅延イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 後退イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 後退イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 後退イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 後退イベント防御側戦力補正
            MiscItemType.NonNegDbl, // 突破イベント攻撃側組織率補正
            MiscItemType.NonNegDbl, // 突破イベント攻撃側戦力補正
            MiscItemType.NonNegDbl, // 突破イベント防御側組織率補正
            MiscItemType.NonNegDbl, // 突破イベント防御側戦力補正
            MiscItemType.Bool, // 司令部は突破イベント時のみ戦力ダメージ
            MiscItemType.Enum, // 戦闘モード
            MiscItemType.Bool, // 攻撃任務
            MiscItemType.RangedDbl, // 攻撃初期効率
            MiscItemType.NonNegDbl, // 攻撃速度ボーナス
            MiscItemType.Bool, // 基地移動任務
            MiscItemType.RangedDbl, // 基地移動初期効率
            MiscItemType.NonNegDbl, // 基地移動被発見確率
            MiscItemType.Bool, // 戦略的再配置任務
            MiscItemType.RangedDbl, // 戦略的再配置初期効率
            MiscItemType.NonNegDbl, // 戦略的再配置加算値
            MiscItemType.NonNegDbl, // 戦略的再配置距離補正
            MiscItemType.Bool, // 支援攻撃任務
            MiscItemType.RangedDbl, // 支援攻撃初期効率
            MiscItemType.NonNegDbl, // 支援攻撃速度ボーナス
            MiscItemType.Bool, // 防衛支援任務
            MiscItemType.RangedDbl, // 防衛支援初期効率
            MiscItemType.NonNegDbl, // 防衛支援速度ボーナス
            MiscItemType.Bool, // 待機任務
            MiscItemType.RangedDbl, // 待機初期効率
            MiscItemType.NonNegDbl, // 待機速度ボーナス
            MiscItemType.Bool, // パルチザン掃討任務
            MiscItemType.RangedDbl, // パルチザン掃討初期効率
            MiscItemType.NonNegDbl, // パルチザン掃討制圧力補正
            MiscItemType.Bool, // 防衛計画任務
            MiscItemType.RangedDbl, // 防衛計画初期効率
            MiscItemType.Bool, // 制空権任務
            MiscItemType.RangedDbl, // 制空権初期効率
            MiscItemType.NonNegDbl, // 制空権敵機発見補正
            MiscItemType.NonNegDbl, // 制空権最小ユニット数
            MiscItemType.Bool, // 地上攻撃任務
            MiscItemType.RangedDbl, // 地上攻撃初期効率
            MiscItemType.NonNegDbl, // 地上攻撃組織率ダメージ補正
            MiscItemType.NonNegDbl, // 地上攻撃戦力ダメージ補正
            MiscItemType.Bool, // 阻止攻撃任務
            MiscItemType.RangedDbl, // 阻止攻撃初期効率
            MiscItemType.NonNegDbl, // 阻止攻撃組織率ダメージ補正
            MiscItemType.NonNegDbl, // 阻止攻撃戦力ダメージ補正
            MiscItemType.Bool, // 戦略爆撃任務
            MiscItemType.RangedDbl, // 戦略爆撃初期効率
            MiscItemType.Bool, // 兵站攻撃任務
            MiscItemType.RangedDbl, // 兵站攻撃初期効率
            MiscItemType.Bool, // 空港空爆任務
            MiscItemType.RangedDbl, // 空港空爆初期効率
            MiscItemType.Bool, // 軍事施設攻撃任務
            MiscItemType.RangedDbl, // 軍事施設攻撃初期効率
            MiscItemType.Bool, // 艦船攻撃任務
            MiscItemType.RangedDbl, // 艦船攻撃初期効率
            MiscItemType.Bool, // 港湾攻撃任務
            MiscItemType.RangedDbl, // 港湾攻撃初期効率
            MiscItemType.Bool, // 航空船団爆撃任務
            MiscItemType.RangedDbl, // 航空船団爆撃初期効率
            MiscItemType.Bool, // 空輸補給任務
            MiscItemType.RangedDbl, // 空輸補給初期効率
            MiscItemType.Bool, // 空挺強襲任務
            MiscItemType.RangedDbl, // 空挺強襲初期効率
            MiscItemType.Bool, // 核攻撃任務
            MiscItemType.RangedDbl, // 核攻撃初期効率
            MiscItemType.Bool, // 航空緊急出撃任務
            MiscItemType.RangedDbl, // 航空緊急出撃初期効率
            MiscItemType.NonNegDbl, // 航空緊急出撃敵機発見補正
            MiscItemType.NonNegDbl, // 航空緊急出撃最小ユニット数
            MiscItemType.Bool, // 船団襲撃任務
            MiscItemType.RangedDbl, // 船団襲撃初期効率
            MiscItemType.NonNegDbl, // 船団襲撃航続距離補正
            MiscItemType.NonNegDbl, // 船団襲撃被発見確率
            MiscItemType.Bool, // 対潜作戦任務
            MiscItemType.RangedDbl, // 対潜作戦初期効率
            MiscItemType.Bool, // 海上阻止任務
            MiscItemType.RangedDbl, // 海上阻止初期効率
            MiscItemType.Bool, // 沿岸砲撃任務
            MiscItemType.RangedDbl, // 沿岸砲撃初期効率
            MiscItemType.NonNegDbl, // 沿岸砲撃補正
            MiscItemType.Bool, // 強襲上陸任務
            MiscItemType.RangedDbl, // 強襲上陸初期効率
            MiscItemType.Bool, // 海上輸送任務
            MiscItemType.RangedDbl, // 海上輸送初期効率
            MiscItemType.NonNegDbl, // 海上輸送航続距離補正
            MiscItemType.NonNegDbl, // 海上輸送被発見確率
            MiscItemType.Bool, // 海上戦闘哨戒任務
            MiscItemType.RangedDbl, // 海上戦闘哨戒初期効率
            MiscItemType.Bool, // 空母による港湾攻撃任務
            MiscItemType.RangedDbl, // 空母による港湾攻撃初期効率
            MiscItemType.Bool, // 空母による航空基地攻撃任務
            MiscItemType.RangedDbl, // 空母による航空基地攻撃初期効率
            MiscItemType.Bool, // 隠密移動任務
            MiscItemType.RangedDbl, // 隠密移動初期効率
            MiscItemType.NonNegDbl, // 隠密移動航続距離補正
            MiscItemType.NonNegDbl, // 隠密移動被発見確率
            MiscItemType.Bool, // 海上緊急出撃任務
            MiscItemType.RangedDbl, // 海上緊急出撃初期効率
            MiscItemType.NonNegDbl, // 海上緊急出撃速度ボーナス
            MiscItemType.Bool, // 攻撃/支援攻撃効率を戦闘補正として使用
            MiscItemType.NonNegDbl, // 陸上要塞効率
            MiscItemType.NonNegDbl, // 沿岸要塞効率
            MiscItemType.NonNegDbl, // 対地防御効率
            MiscItemType.NonNegDbl, // 船団防衛効率
            MiscItemType.NonNegDbl, // 人的資源増加
            MiscItemType.NonNegDbl, // TC補正
            MiscItemType.NonNegDbl, // 占領地TC補正
            MiscItemType.NonNegDbl, // 消耗補正
            MiscItemType.NonNegDbl, // 人的資源復帰補正
            MiscItemType.NonNegDbl, // 遠隔地補給補正
            MiscItemType.NonNegDbl, // 修理補正
            MiscItemType.NonNegDbl, // 研究補正
            MiscItemType.NonNegDbl, // レーダー効率
            MiscItemType.NonNegDbl, // 司令部補給効率ボーナス
            MiscItemType.NonNegDbl, // 司令部コンバットイベントボーナス
            MiscItemType.RangedDbl, // コンバットイベント発生確率
            MiscItemType.NonNegInt, // 友軍発見確率
            MiscItemType.NonNegInt, // 敵軍発見確率
            MiscItemType.NonNegInt, // 友好国諜報確率
            MiscItemType.NonNegInt, // 敵国諜報確率
            MiscItemType.NonNegInt, // 最大強襲上陸規模
            MiscItemType.NonNegDbl, // エネルギー/石油変換効率
            MiscItemType.NonNegDbl, // 総合生産効率
            MiscItemType.NonNegDbl, // 物資生産効率
            MiscItemType.NonNegDbl, // 対空砲攻撃力
            MiscItemType.NonNegDbl, // 空軍奇襲確率
            MiscItemType.NonNegDbl, // 陸軍奇襲確率
            MiscItemType.NonNegDbl, // 海軍奇襲確率
            MiscItemType.NonNegDbl, // 平時IC補正
            MiscItemType.NonNegDbl, // 戦時IC補正
            MiscItemType.NonNegDbl, // 建造物生産補正
            MiscItemType.NonNegDbl, // 輸送船団生産補正
            MiscItemType.NonNegDbl, // 最小艦船ポジション値
            MiscItemType.NonNegDbl, // 最大艦船ポジション値
            MiscItemType.NonNegDbl, // 平時資源備蓄補正
            MiscItemType.NonNegDbl, // 戦時資源備蓄補正
            MiscItemType.NonNegDbl, // 平時物資/燃料備蓄補正
            MiscItemType.NonNegDbl, // 戦時物資/燃料備蓄補正
            MiscItemType.NonNegDbl, // 青写真ボーナス
            MiscItemType.NonPosDbl5AoD, // 史実年度以前研究ペナルティ
            MiscItemType.NonNegDbl, // 史実年度以降研究ボーナス
            MiscItemType.NonNegDbl, // 研究機関レベル毎のコスト
            MiscItemType.NonNegInt, // 1年毎の発明イベント平均回数
            MiscItemType.NonNegDbl5, // 史実年度以降研究ボーナス
            MiscItemType.NonNegDbl, // 研究速度補正
            MiscItemType.NonNegDbl, // 史実年度以前研究ペナルティ上限
            MiscItemType.NonNegDbl, // 史実年度以降研究ボーナス上限
            MiscItemType.RangedInt, // 研究スロット上限
            MiscItemType.PosInt, // 研究スロット毎の必要IC
            MiscItemType.NonNegDbl0, // 最大ランダム補正
            MiscItemType.Enum, // 研究ページのレイアウト
            MiscItemType.Enum, // 研究概要パネルのスタイル
            MiscItemType.PosInt, // 研究スロット上限
            MiscItemType.PosInt, // 研究スロット下限
            MiscItemType.PosInt, // 研究スロット毎の必要IC
            MiscItemType.Bool, // 新規国家でロケット技術を継承
            MiscItemType.Bool, // 新規国家で核物理学技術を継承
            MiscItemType.Bool, // 新規国家で核工学技術を継承
            MiscItemType.Bool, // 新規国家で秘密兵器技術を継承
            MiscItemType.PosInt, // 最大研究機関スキル
            MiscItemType.PosInt, // 貿易交渉間隔
            MiscItemType.RangedPosInt, // ゲーム開始直後の貿易交渉遅延日数
            MiscItemType.NonNegInt, // ゲーム開始直後のAI友好国貿易交渉遅延日数
            MiscItemType.NonNegInt, // 理想石油備蓄
            MiscItemType.NonNegInt, // 危険水準石油備蓄
            MiscItemType.NonNegInt, // 理想物資備蓄
            MiscItemType.NonNegInt, // 危険水準物資備蓄
            MiscItemType.NonNegInt, // 理想資源備蓄
            MiscItemType.NonNegInt, // 危険水準資源備蓄
            MiscItemType.NonNegDbl, // 戦時理想備蓄係数
            MiscItemType.NonNegDbl, // 平時石油臨時輸入割合
            MiscItemType.NonNegDbl, // 戦時石油臨時輸入割合
            MiscItemType.NonNegDbl2, // 理想備蓄未到達時の臨時輸入割合
            MiscItemType.NonNegDbl2Dh103Full1, // 物資生産割合
            MiscItemType.NonNegDbl2Dh103Full1, // 資金生産割合
            MiscItemType.NonNegDbl2, // 備蓄選択時の臨時輸入割合
            MiscItemType.PosInt, // 貿易協定資源輸送日数
            MiscItemType.Bool, // 貿易協定の統合
            MiscItemType.RangedIntMinusOne, // 手動貿易協定
            MiscItemType.NonNegInt, // 傀儡国上納物資/資金
            MiscItemType.PosDbl, // 傀儡国物資危険水準
            MiscItemType.NonNegIntNegDbl, // 傀儡国最大資源備蓄
            MiscItemType.RangedInt, // 新規貿易協定最小効率
            MiscItemType.RangedInt, // 貿易協定破棄効率
            MiscItemType.RangedInt, // 自動/AIの貿易協定最小効率
            MiscItemType.NonNegDbl, // 理想備蓄未達時の余剰物資生産比率
            MiscItemType.NonNegDbl, // 戦時余剰物資生産係数
            MiscItemType.NonNegDbl, // 備蓄余裕時の物資生産禁止係数
            MiscItemType.RangedInt, // 守備隊/民兵の最大連続生産数
            MiscItemType.NonNegInt, // 海軍/空軍連続生産最小IC
            MiscItemType.NonNegDbl, // 新規生産禁止人的資源比率
            MiscItemType.NonNegDbl, // 新規生産禁止人的資源値
            MiscItemType.RangedDbl, // 新規生産禁止物資比率
            MiscItemType.RangedDblMinusOne1, // 総ICに対する軍事力比率(平時)
            MiscItemType.RangedDblMinusOne1, // 総ICに対する軍事力比率(戦時)
            MiscItemType.PosDbl, // 総ICに対する軍事力比率(主要国)
            MiscItemType.NonNegDbl, // 攻勢中止物資備蓄
            MiscItemType.NonNegDbl, // 攻勢中止燃料備蓄
            MiscItemType.NonNegDbl, // 攻勢中止補給効率
            MiscItemType.RangedDbl, // 攻勢中止組織率/戦力ダメージ
            MiscItemType.Enum, // AIの平時の攻撃的諜報活動
            MiscItemType.NonNegDbl, // AIの諜報コスト補正
            MiscItemType.NonNegDbl, // AIの外交コスト補正
            MiscItemType.NonNegDbl, // AIの外交干渉頻度補正
            MiscItemType.Enum, // AIの新宣戦布告ルール
            MiscItemType.Enum, // AIの新宣戦布告ルール
            MiscItemType.NonNegInt, // 傀儡国が宗主国の同盟に強制加入する中立性
            MiscItemType.Bool, // 新AI占領地解放ルール
            MiscItemType.RangedIntMinusThree, // AIイベント選択ルール
            MiscItemType.NonNegIntMinusOne, // 強制戦略的再配置時間
            MiscItemType.NonNegInt, // AI最大再配置日数
            MiscItemType.Bool, // 守備隊AIの簡易チェック
            MiscItemType.Bool, // AI宗主国が傀儡国の占領地を支配
            MiscItemType.NonNegInt, // AI占領地解放最小日数
            MiscItemType.NonNegInt, // AI占領地返還最小日数
            MiscItemType.NonNegInt, // AI占領地返還最小日数(物資補給基地)
            MiscItemType.RangedDbl, // 被領有権主張時連合加盟最小友好度
            MiscItemType.Bool, // AIの諜報/外交をログに記録
            MiscItemType.NonNegIntMinusOne, // 国家情報をログに記録
            MiscItemType.Bool, // AI切り替えをログに記録
            MiscItemType.Bool, // 新自動セーブファイル名
            MiscItemType.Bool, // マルチプレイでAI切替時に新しい設定を読み込む
            MiscItemType.Int, // 貿易効率算出間隔
            MiscItemType.NonNegInt, // 備蓄庫の再計算間隔
            MiscItemType.Bool, // 損失を記録
            MiscItemType.Enum, // 損失を記録
            MiscItemType.Bool, // 占領地で旅団付属を許可
            MiscItemType.PosInt, // 陸軍の一括配置数
            MiscItemType.PosInt, // 海軍の一括配置数
            MiscItemType.PosInt, // 空軍の一括配置数
            MiscItemType.Bool, // すべての陸地プロヴィンスに固有画像を許可
            MiscItemType.Bool, // イベント選択肢を委任
            MiscItemType.Enum, // イベント選択肢強制表示
            MiscItemType.Bool, // ディシジョンを使用する
            MiscItemType.RangedInt, // パルチザンの歩兵構成割合
            MiscItemType.NonNegIntMinusOne, // パルチザンの技術レベル
            MiscItemType.RangedInt, // パルチザン最小戦力
            MiscItemType.RangedInt, // パルチザン最大戦力
            MiscItemType.NonNegDbl, // パルチザン組織率回復速度
            MiscItemType.NonNegInt, // パルチザンボーナス(隣接地占領)
            MiscItemType.NonNegInt, // パルチザンボーナス(占領地)
            MiscItemType.NonNegInt, // パルチザンボーナス(山岳)
            MiscItemType.NonNegInt, // パルチザンボーナス(丘陵)
            MiscItemType.NonNegInt, // パルチザンボーナス(森林)
            MiscItemType.NonNegInt, // パルチザンボーナス(密林)
            MiscItemType.NonNegInt, // パルチザンボーナス(湿地)
            MiscItemType.NonNegInt, // パルチザンボーナス(砂漠)
            MiscItemType.NonNegInt, // パルチザンボーナス(平地)
            MiscItemType.NonNegInt, // パルチザンボーナス(都市)
            MiscItemType.NonNegInt, // パルチザンボーナス(航空/海軍基地)
            MiscItemType.PosInt, // パルチザン占領プロヴィンス返却時間
            MiscItemType.Bool, // 新形式閣僚ファイルフォーマット
            MiscItemType.Bool, // 閣僚引退年を使用
            MiscItemType.Bool, // 指揮官引退年を使用
            MiscItemType.Bool, // スプライトをMODDIRのみから読み込む
            MiscItemType.Bool, // ユニットアイコンをMODDIRのみから読み込む
            MiscItemType.Bool, // ユニット画像をMODDIRのみから読み込む
            MiscItemType.Bool, // AIファイルをMODDIRのみから読み込む
            MiscItemType.Enum, // 守備隊判定ルール
            MiscItemType.Bool, // 旧セーブフォーマットを使用
            MiscItemType.Enum, // 生産パネルのUIスタイル
            MiscItemType.Enum, // ユニット画像のサイズ
            MiscItemType.Bool, // 艦艇付属装備に画像を使用
            MiscItemType.Enum, // 建物をプロヴィンスでのみ建造
            MiscItemType.PosInt, // ユニット補正ページの新スタイル移行閾値
            MiscItemType.NonNegInt, // マップ番号
            MiscItemType.RangedInt, // 総プロヴィンス数
            MiscItemType.Enum, // 距離算出方法
            MiscItemType.PosInt, // マップの幅
            MiscItemType.PosInt // マップの高さ
        };

        /// <summary>
        ///     項目の最小値 (整数型/列挙型)
        /// </summary>
        public static readonly Dictionary<MiscItemId, int> IntMinValues = new Dictionary<MiscItemId, int>
        {
            {MiscItemId.MaxRevoltRisk, 0},
            {MiscItemId.CanUnitSendNonAllied, 0},
            {MiscItemId.ChanceDetectSpyMission, 0},
            {MiscItemId.RelationshipsHitDetectedMissions, 0},
            {MiscItemId.ShowThirdCountrySpyReports, 0},
            {MiscItemId.SpyInformationAccuracyModifier, -10},
            {MiscItemId.AiPeacetimeSpyMissions, 0},
            {MiscItemId.CanChangeIdeas, 0},
            {MiscItemId.CanUnitSendNonAlliedDh, 0},
            {MiscItemId.BluePrintsCanSoldNonAllied, 0},
            {MiscItemId.ProvinceCanSoldNonAllied, 0},
            {MiscItemId.TransferAlliedCoreProvinces, 0},
            {MiscItemId.ProductionLineEdit, 0},
            {MiscItemId.ConvoySystemOptionsAllied, 0},
            {MiscItemId.ChanceDetectSpyMissionDh, 0},
            {MiscItemId.RelationshipsHitDetectedMissionsDh, 0},
            {MiscItemId.SpyInformationAccuracyModifierDh, -10},
            {MiscItemId.ShowThirdCountrySpyReportsDh, 0},
            {MiscItemId.UseMinisterPersonalityReplacing, 0},
            {MiscItemId.RelationshipHitCancelTrade, 0},
            {MiscItemId.RelationshipHitCancelPermanentTrade, 0},
            {MiscItemId.PuppetsJoinMastersAlliance, 0},
            {MiscItemId.MastersBecomePuppetsPuppets, 0},
            {MiscItemId.AllowManualClaimsChange, 0},
            {MiscItemId.JoinAutomaticallyAllesAxis, 0},
            {MiscItemId.AllowChangeHosHog, 0},
            {MiscItemId.ChangeTagCoup, 0},
            {MiscItemId.FilterReleaseCountries, 0},
            {MiscItemId.AaAirFiringRules, 0},
            {MiscItemId.MinisterBonuses, 1},
            {MiscItemId.ConvoyInterceptionMissions, 0},
            {MiscItemId.AutoReturnTransportFleets, 0},
            {MiscItemId.AllowProvinceRegionTargeting, 0},
            {MiscItemId.RecalculateArrivalTimesCombat, 0},
            {MiscItemId.ApplyRangeLimitsAreasRegions, 0},
            {MiscItemId.ChanceTargetNoOrgLand, 0},
            {MiscItemId.HqStrDamageBreakthrough, 0},
            {MiscItemId.CombatMode, 0},
            {MiscItemId.AttackMission, 0},
            {MiscItemId.RebaseMission, 0},
            {MiscItemId.StratRedeployMission, 0},
            {MiscItemId.SupportAttackMission, 0},
            {MiscItemId.SupportDefenseMission, 0},
            {MiscItemId.ReservesMission, 0},
            {MiscItemId.AntiPartisanDutyMission, 0},
            {MiscItemId.PlannedDefenseMission, 0},
            {MiscItemId.AirSuperiorityMission, 0},
            {MiscItemId.GroundAttackMission, 0},
            {MiscItemId.InterdictionMission, 0},
            {MiscItemId.StrategicBombardmentMission, 0},
            {MiscItemId.LogisticalStrikeMission, 0},
            {MiscItemId.RunwayCrateringMission, 0},
            {MiscItemId.InstallationStrikeMission, 0},
            {MiscItemId.NavalStrikeMission, 0},
            {MiscItemId.PortStrikeMission, 0},
            {MiscItemId.ConvoyAirRaidingMission, 0},
            {MiscItemId.AirSupplyMission, 0},
            {MiscItemId.AirborneAssaultMission, 0},
            {MiscItemId.NukeMission, 0},
            {MiscItemId.AirScrambleMission, 0},
            {MiscItemId.ConvoyRadingMission, 0},
            {MiscItemId.AswMission, 0},
            {MiscItemId.NavalInterdictionMission, 0},
            {MiscItemId.ShoreBombardmentMission, 0},
            {MiscItemId.AmphibousAssaultMission, 0},
            {MiscItemId.SeaTransportMission, 0},
            {MiscItemId.NavalCombatPatrolMission, 0},
            {MiscItemId.NavalPortStrikeMission, 0},
            {MiscItemId.NavalAirbaseStrikeMission, 0},
            {MiscItemId.SneakMoveMission, 0},
            {MiscItemId.NavalScrambleMission, 0},
            {MiscItemId.UseAttackEfficiencyCombatModifier, 0},
            {MiscItemId.MaxActiveTechTeamsAoD, 1},
            {MiscItemId.UseNewTechnologyPageLayout, 0},
            {MiscItemId.TechOverviewPanelStyle, 0},
            {MiscItemId.NewCountryRocketryComponent, 0},
            {MiscItemId.NewCountryNuclearPhysicsComponent, 0},
            {MiscItemId.NewCountryNuclearEngineeringComponent, 0},
            {MiscItemId.NewCountrySecretTechs, 0},
            {MiscItemId.DelayGameStartNewTrades, 2},
            {MiscItemId.MergeTradeDeals, 0},
            {MiscItemId.ManualTradeDeals, 0},
            {MiscItemId.NewTradeDealsMinEffectiveness, 0},
            {MiscItemId.CancelTradeDealsEffectiveness, 0},
            {MiscItemId.AutoTradeAiTradeDeals, 0},
            {MiscItemId.MaxSerialLineProductionGarrisonMilitia, 1},
            {MiscItemId.AiPeacetimeSpyMissionsDh, 0},
            {MiscItemId.NewDowRules, 0},
            {MiscItemId.NewDowRules2, 0},
            {MiscItemId.NewAiReleaseRules, 0},
            {MiscItemId.AiEventsActionSelectionRules, 0},
            {MiscItemId.UseQuickAreaCheckGarrisonAi, 0},
            {MiscItemId.AiMastersGetProvincesConquredPuppets, 0},
            {MiscItemId.AiSpyDiplomaticMissionLogger, 0},
            {MiscItemId.SwitchedAiFilesLogger, 0},
            {MiscItemId.UseNewAutoSaveFileFormat, 0},
            {MiscItemId.LoadNewAiSwitchingAllClients, 0},
            {MiscItemId.InGameLossesLogging, 0},
            {MiscItemId.InGameLossLogging2, 0},
            {MiscItemId.AllowBrigadeAttachingInSupply, 0},
            {MiscItemId.AllowUniquePicturesAllLandProvinces, 0},
            {MiscItemId.AutoReplyEvents, 0},
            {MiscItemId.ForceActionsShow, 0},
            {MiscItemId.EnableDicisionsPlayers, 0},
            {MiscItemId.RebelsArmyComposition, 0},
            {MiscItemId.RebelsArmyMinStr, 1},
            {MiscItemId.RebelsArmyMaxStr, 1},
            {MiscItemId.UseNewMinisterFilesFormat, 0},
            {MiscItemId.EnableRetirementYearMinisters, 0},
            {MiscItemId.EnableRetirementYearLeaders, 0},
            {MiscItemId.LoadSpritesModdirOnly, 0},
            {MiscItemId.LoadUnitIconsModdirOnly, 0},
            {MiscItemId.LoadUnitPicturesModdirOnly, 0},
            {MiscItemId.LoadAiFilesModdirOnly, 0},
            {MiscItemId.UseSpeedSetGarrisonStatus, 0},
            {MiscItemId.UseOldSaveGameFormat, 0},
            {MiscItemId.ProductionPanelUiStyle, 0},
            {MiscItemId.UnitPicturesSize, 0},
            {MiscItemId.EnablePicturesNavalBrigades, 0},
            {MiscItemId.BuildingsBuildableOnlyProvinces, 0},
            {MiscItemId.TotalProvinces, 1},
            {MiscItemId.DistanceCalculationModel, 0},
        };

        /// <summary>
        ///     項目の最大値 (整数型/列挙型)
        /// </summary>
        public static Dictionary<MiscItemId, int> IntMaxValues = new Dictionary<MiscItemId, int>
        {
            {MiscItemId.MaxRevoltRisk, 100},
            {MiscItemId.CanUnitSendNonAllied, 1},
            {MiscItemId.ChanceDetectSpyMission, 100},
            {MiscItemId.RelationshipsHitDetectedMissions, 400},
            {MiscItemId.ShowThirdCountrySpyReports, 3},
            {MiscItemId.SpyInformationAccuracyModifier, 10},
            {MiscItemId.AiPeacetimeSpyMissions, 2},
            {MiscItemId.CanChangeIdeas, 1},
            {MiscItemId.CanUnitSendNonAlliedDh, 2},
            {MiscItemId.BluePrintsCanSoldNonAllied, 2},
            {MiscItemId.ProvinceCanSoldNonAllied, 2},
            {MiscItemId.TransferAlliedCoreProvinces, 1},
            {MiscItemId.ProductionLineEdit, 1},
            {MiscItemId.ConvoySystemOptionsAllied, 2},
            {MiscItemId.ChanceDetectSpyMissionDh, 100},
            {MiscItemId.RelationshipsHitDetectedMissionsDh, 400},
            {MiscItemId.SpyInformationAccuracyModifierDh, 10},
            {MiscItemId.ShowThirdCountrySpyReportsDh, 3},
            {MiscItemId.UseMinisterPersonalityReplacing, 1},
            {MiscItemId.RelationshipHitCancelTrade, 400},
            {MiscItemId.RelationshipHitCancelPermanentTrade, 100},
            {MiscItemId.PuppetsJoinMastersAlliance, 1},
            {MiscItemId.MastersBecomePuppetsPuppets, 1},
            {MiscItemId.AllowManualClaimsChange, 1},
            {MiscItemId.JoinAutomaticallyAllesAxis, 1},
            {MiscItemId.AllowChangeHosHog, 3},
            {MiscItemId.ChangeTagCoup, 1},
            {MiscItemId.FilterReleaseCountries, 3},
            {MiscItemId.AaAirFiringRules, 1},
            {MiscItemId.MinisterBonuses, 5},
            {MiscItemId.ConvoyInterceptionMissions, 2},
            {MiscItemId.AutoReturnTransportFleets, 2},
            {MiscItemId.AllowProvinceRegionTargeting, 1},
            {MiscItemId.RecalculateArrivalTimesCombat, 2},
            {MiscItemId.ApplyRangeLimitsAreasRegions, 2},
            {MiscItemId.ChanceTargetNoOrgLand, 100},
            {MiscItemId.HqStrDamageBreakthrough, 1},
            {MiscItemId.CombatMode, 1},
            {MiscItemId.AttackMission, 1},
            {MiscItemId.RebaseMission, 1},
            {MiscItemId.StratRedeployMission, 1},
            {MiscItemId.SupportAttackMission, 1},
            {MiscItemId.SupportDefenseMission, 1},
            {MiscItemId.ReservesMission, 1},
            {MiscItemId.AntiPartisanDutyMission, 1},
            {MiscItemId.PlannedDefenseMission, 1},
            {MiscItemId.AirSuperiorityMission, 1},
            {MiscItemId.GroundAttackMission, 1},
            {MiscItemId.InterdictionMission, 1},
            {MiscItemId.StrategicBombardmentMission, 1},
            {MiscItemId.LogisticalStrikeMission, 1},
            {MiscItemId.RunwayCrateringMission, 1},
            {MiscItemId.InstallationStrikeMission, 1},
            {MiscItemId.NavalStrikeMission, 1},
            {MiscItemId.PortStrikeMission, 1},
            {MiscItemId.ConvoyAirRaidingMission, 1},
            {MiscItemId.AirSupplyMission, 1},
            {MiscItemId.AirborneAssaultMission, 1},
            {MiscItemId.NukeMission, 1},
            {MiscItemId.AirScrambleMission, 1},
            {MiscItemId.ConvoyRadingMission, 1},
            {MiscItemId.AswMission, 1},
            {MiscItemId.NavalInterdictionMission, 1},
            {MiscItemId.ShoreBombardmentMission, 1},
            {MiscItemId.AmphibousAssaultMission, 1},
            {MiscItemId.SeaTransportMission, 1},
            {MiscItemId.NavalCombatPatrolMission, 1},
            {MiscItemId.NavalPortStrikeMission, 1},
            {MiscItemId.NavalAirbaseStrikeMission, 1},
            {MiscItemId.SneakMoveMission, 1},
            {MiscItemId.NavalScrambleMission, 1},
            {MiscItemId.UseAttackEfficiencyCombatModifier, 1},
            {MiscItemId.MaxActiveTechTeamsAoD, 20},
            {MiscItemId.UseNewTechnologyPageLayout, 1},
            {MiscItemId.TechOverviewPanelStyle, 1},
            {MiscItemId.NewCountryRocketryComponent, 1},
            {MiscItemId.NewCountryNuclearPhysicsComponent, 1},
            {MiscItemId.NewCountryNuclearEngineeringComponent, 1},
            {MiscItemId.NewCountrySecretTechs, 1},
            {MiscItemId.DelayGameStartNewTrades, 0},
            {MiscItemId.MergeTradeDeals, 1},
            {MiscItemId.ManualTradeDeals, 100},
            {MiscItemId.NewTradeDealsMinEffectiveness, 100},
            {MiscItemId.CancelTradeDealsEffectiveness, 100},
            {MiscItemId.AutoTradeAiTradeDeals, 100},
            {MiscItemId.MaxSerialLineProductionGarrisonMilitia, 99},
            {MiscItemId.AiPeacetimeSpyMissionsDh, 2},
            {MiscItemId.NewDowRules, 1},
            {MiscItemId.NewDowRules2, 2},
            {MiscItemId.NewAiReleaseRules, 1},
            {MiscItemId.AiEventsActionSelectionRules, 100},
            {MiscItemId.UseQuickAreaCheckGarrisonAi, 1},
            {MiscItemId.AiMastersGetProvincesConquredPuppets, 1},
            {MiscItemId.AiSpyDiplomaticMissionLogger, 1},
            {MiscItemId.SwitchedAiFilesLogger, 1},
            {MiscItemId.UseNewAutoSaveFileFormat, 1},
            {MiscItemId.LoadNewAiSwitchingAllClients, 1},
            {MiscItemId.InGameLossesLogging, 1},
            {MiscItemId.InGameLossLogging2, 4},
            {MiscItemId.AllowBrigadeAttachingInSupply, 1},
            {MiscItemId.AllowUniquePicturesAllLandProvinces, 1},
            {MiscItemId.AutoReplyEvents, 1},
            {MiscItemId.ForceActionsShow, 2},
            {MiscItemId.EnableDicisionsPlayers, 1},
            {MiscItemId.RebelsArmyComposition, 100},
            {MiscItemId.RebelsArmyMinStr, 100},
            {MiscItemId.RebelsArmyMaxStr, 100},
            {MiscItemId.UseNewMinisterFilesFormat, 1},
            {MiscItemId.EnableRetirementYearMinisters, 1},
            {MiscItemId.EnableRetirementYearLeaders, 1},
            {MiscItemId.LoadSpritesModdirOnly, 1},
            {MiscItemId.LoadUnitIconsModdirOnly, 1},
            {MiscItemId.LoadUnitPicturesModdirOnly, 1},
            {MiscItemId.LoadAiFilesModdirOnly, 1},
            {MiscItemId.UseSpeedSetGarrisonStatus, 1},
            {MiscItemId.UseOldSaveGameFormat, 1},
            {MiscItemId.ProductionPanelUiStyle, 1},
            {MiscItemId.UnitPicturesSize, 1},
            {MiscItemId.EnablePicturesNavalBrigades, 1},
            {MiscItemId.BuildingsBuildableOnlyProvinces, 2},
            {MiscItemId.TotalProvinces, 10000},
            {MiscItemId.DistanceCalculationModel, 1},
        };

        /// <summary>
        ///     項目の最小値 (実数型)
        /// </summary>
        public static Dictionary<MiscItemId, double> DblMinValues = new Dictionary<MiscItemId, double>
        {
            {MiscItemId.DistanceModifierNeighbours, 0},
            {MiscItemId.OverStockpileLimitDailyLoss, 0},
            {MiscItemId.GearingBonusLossUpgradeUnit, 0},
            {MiscItemId.GearingBonusLossUpgradeBrigade, 0},
            {MiscItemId.MaxDailyDissent, 0},
            {MiscItemId.DistanceModifierNeighboursDh, 0},
            {MiscItemId.LandOrgDamageLandUrban, 0},
            {MiscItemId.LandOrgDamageLandFort, 0},
            {MiscItemId.SubStacksDetectionModifier, 0},
            {MiscItemId.AaAirNightModifier, 0},
            {MiscItemId.NightHoursWinter, 0},
            {MiscItemId.NightHoursSpringFall, 0},
            {MiscItemId.NightHoursSummer, 0},
            {MiscItemId.FleetSizeRangePenaltyRatio, 0},
            {MiscItemId.FleetSizeRangePenaltyMax, 0},
            {MiscItemId.FleetPositioningFleetComposition, 0},
            {MiscItemId.AttackStartingEfficiency, 0.05},
            {MiscItemId.RebaseStartingEfficiency, 0.05},
            {MiscItemId.StratRedeployStartingEfficiency, 0.05},
            {MiscItemId.SupportAttackStartingEfficiency, 0.05},
            {MiscItemId.SupportDefenseStartingEfficiency, 0.05},
            {MiscItemId.ReservesStartingEfficiency, 0.05},
            {MiscItemId.AntiPartisanDutyStartingEfficiency, 0.05},
            {MiscItemId.PlannedDefenseStartingEfficiency, 0.05},
            {MiscItemId.AirSuperiorityStartingEfficiency, 0.05},
            {MiscItemId.GroundAttackStartingEfficiency, 0.05},
            {MiscItemId.InterdictionStartingEfficiency, 0.05},
            {MiscItemId.StrategicBombardmentStartingEfficiency, 0.05},
            {MiscItemId.LogisticalStrikeStartingEfficiency, 0.05},
            {MiscItemId.RunwayCrateringStartingEfficiency, 0.05},
            {MiscItemId.InstallationStrikeStartingEfficiency, 0.05},
            {MiscItemId.NavalStrikeStartingEfficiency, 0.05},
            {MiscItemId.PortStrikeStartingEfficiency, 0.05},
            {MiscItemId.ConvoyAirRaidingStartingEfficiency, 0.05},
            {MiscItemId.AirSupplyStartingEfficiency, 0.05},
            {MiscItemId.AirborneAssaultStartingEfficiency, 0.05},
            {MiscItemId.NukeStartingEfficiency, 0.05},
            {MiscItemId.AirScrambleStartingEfficiency, 0.05},
            {MiscItemId.ConvoyRadingStartingEfficiency, 0.05},
            {MiscItemId.AswStartingEfficiency, 0.05},
            {MiscItemId.NavalInterdictionStartingEfficiency, 0.05},
            {MiscItemId.ShoreBombardmentStartingEfficiency, 0.05},
            {MiscItemId.AmphibousAssaultStartingEfficiency, 0.05},
            {MiscItemId.SeaTransportStartingEfficiency, 0.05},
            {MiscItemId.NavalCombatPatrolStartingEfficiency, 0.05},
            {MiscItemId.NavalPortStrikeStartingEfficiency, 0.05},
            {MiscItemId.NavalAirbaseStrikeStartingEfficiency, 0.05},
            {MiscItemId.SneakMoveStartingEfficiency, 0.05},
            {MiscItemId.NavalScrambleStartingEfficiency, 0.05},
            {MiscItemId.CombatEventChances, 0},
            {MiscItemId.NotProduceNewUnitsSupply, 0},
            {MiscItemId.MilitaryStrengthTotalIcRatioPeacetime, 0},
            {MiscItemId.MilitaryStrengthTotalIcRatioWartime, 0},
            {MiscItemId.NotUseOffensiveOrgStrDamage, 0},
            {MiscItemId.MinRequiredRelationsAlliedClaimed, -200},
        };

        /// <summary>
        ///     項目の最大値 (実数型)
        /// </summary>
        public static Dictionary<MiscItemId, double> DblMaxValues = new Dictionary<MiscItemId, double>
        {
            {MiscItemId.DistanceModifierNeighbours, 1},
            {MiscItemId.OverStockpileLimitDailyLoss, 1},
            {MiscItemId.GearingBonusLossUpgradeUnit, 1},
            {MiscItemId.GearingBonusLossUpgradeBrigade, 1},
            {MiscItemId.MaxDailyDissent, 1},
            {MiscItemId.DistanceModifierNeighboursDh, 1},
            {MiscItemId.LandOrgDamageLandUrban, 1},
            {MiscItemId.LandOrgDamageLandFort, 1},
            {MiscItemId.SubStacksDetectionModifier, 1},
            {MiscItemId.AaAirNightModifier, 1},
            {MiscItemId.NightHoursWinter, 24},
            {MiscItemId.NightHoursSpringFall, 24},
            {MiscItemId.NightHoursSummer, 24},
            {MiscItemId.FleetSizeRangePenaltyRatio, 1},
            {MiscItemId.FleetSizeRangePenaltyMax, 1},
            {MiscItemId.FleetPositioningFleetComposition, 1},
            {MiscItemId.AttackStartingEfficiency, 10},
            {MiscItemId.RebaseStartingEfficiency, 10},
            {MiscItemId.StratRedeployStartingEfficiency, 10},
            {MiscItemId.SupportAttackStartingEfficiency, 10},
            {MiscItemId.SupportDefenseStartingEfficiency, 10},
            {MiscItemId.ReservesStartingEfficiency, 10},
            {MiscItemId.AntiPartisanDutyStartingEfficiency, 10},
            {MiscItemId.PlannedDefenseStartingEfficiency, 10},
            {MiscItemId.AirSuperiorityStartingEfficiency, 10},
            {MiscItemId.GroundAttackStartingEfficiency, 10},
            {MiscItemId.InterdictionStartingEfficiency, 10},
            {MiscItemId.StrategicBombardmentStartingEfficiency, 10},
            {MiscItemId.LogisticalStrikeStartingEfficiency, 10},
            {MiscItemId.RunwayCrateringStartingEfficiency, 10},
            {MiscItemId.InstallationStrikeStartingEfficiency, 10},
            {MiscItemId.NavalStrikeStartingEfficiency, 10},
            {MiscItemId.PortStrikeStartingEfficiency, 10},
            {MiscItemId.ConvoyAirRaidingStartingEfficiency, 10},
            {MiscItemId.AirSupplyStartingEfficiency, 10},
            {MiscItemId.AirborneAssaultStartingEfficiency, 10},
            {MiscItemId.NukeStartingEfficiency, 10},
            {MiscItemId.AirScrambleStartingEfficiency, 10},
            {MiscItemId.ConvoyRadingStartingEfficiency, 10},
            {MiscItemId.AswStartingEfficiency, 10},
            {MiscItemId.NavalInterdictionStartingEfficiency, 10},
            {MiscItemId.ShoreBombardmentStartingEfficiency, 10},
            {MiscItemId.AmphibousAssaultStartingEfficiency, 10},
            {MiscItemId.SeaTransportStartingEfficiency, 10},
            {MiscItemId.NavalCombatPatrolStartingEfficiency, 10},
            {MiscItemId.NavalPortStrikeStartingEfficiency, 10},
            {MiscItemId.NavalAirbaseStrikeStartingEfficiency, 10},
            {MiscItemId.SneakMoveStartingEfficiency, 10},
            {MiscItemId.NavalScrambleStartingEfficiency, 10},
            {MiscItemId.CombatEventChances, 1},
            {MiscItemId.NotProduceNewUnitsSupply, 1},
            {MiscItemId.MilitaryStrengthTotalIcRatioPeacetime, 1},
            {MiscItemId.MilitaryStrengthTotalIcRatioWartime, 1},
            {MiscItemId.NotUseOffensiveOrgStrDamage, 1},
            {MiscItemId.MinRequiredRelationsAlliedClaimed, 200},
        };

        /// <summary>
        ///     項目名
        /// </summary>
        public static readonly string[] ItemNames =
        {
            "IcToTcRatio",
            "IcToSuppliesRatio",
            "IcToConsumerGoodsRatio",
            "IcToMoneyRatio",
            "DissentChangeSpeed",
            "MinAvailableIc",
            "MinFinalIc",
            "DissentReduction",
            "MaxGearingBonus",
            "GearingBonusIncrement",
            "GearingResourceIncrement",
            "GearingLossNoIc",
            "IcMultiplierNonNational",
            "IcMultiplierNonOwned",
            "IcMultiplierPuppet",
            "ResourceMultiplierNonNational",
            "ResourceMultiplierNonOwned",
            "ResourceMultiplierNonNationalAi",
            "ResourceMultiplierPuppet",
            "TcLoadUndeployedDivision",
            "TcLoadOccupied",
            "TcLoadMultiplierLand",
            "TcLoadMultiplierAir",
            "TcLoadMultiplierNaval",
            "TcLoadPartisan",
            "TcLoadFactorOffensive",
            "TcLoadProvinceDevelopment",
            "TcLoadBase",
            "ManpowerMultiplierNational",
            "ManpowerMultiplierNonNational",
            "ManpowerMultiplierColony",
            "ManpowerMultiplierPuppet",
            "ManpowerMultiplierWartimeOversea",
            "ManpowerMultiplierPeacetime",
            "ManpowerMultiplierWartime",
            "DailyRetiredManpower",
            "RequirementAffectSlider",
            "TrickleBackFactorManpower",
            "ReinforceManpower",
            "ReinforceCost",
            "ReinforceTime",
            "UpgradeCost",
            "UpgradeTime",
            "ReinforceToUpdateModifier",
            "NationalismStartingValue",
            "NationalismPerManpowerAoD",
            "NationalismPerManpowerDh",
            "MaxNationalism",
            "MaxRevoltRisk",
            "MonthlyNationalismReduction",
            "SendDivisionDays",
            "TcLoadUndeployedBrigade",
            "CanUnitSendNonAllied",
            "SpyMissionDays",
            "IncreateIntelligenceLevelDays",
            "ChanceDetectSpyMission",
            "RelationshipsHitDetectedMissions",
            "ShowThirdCountrySpyReports",
            "DistanceModifierNeighbours",
            "SpyInformationAccuracyModifier",
            "AiPeacetimeSpyMissions",
            "MaxIcCostModifier",
            "AiSpyMissionsCostModifier",
            "AiDiplomacyCostModifier",
            "AiInfluenceModifier",
            "CostRepairBuildings",
            "TimeRepairBuilding",
            "ProvinceEfficiencyRiseTime",
            "CoreProvinceEfficiencyRiseTime",
            "LineUpkeep",
            "LineStartupTime",
            "LineUpgradeTime",
            "RetoolingCost",
            "RetoolingResource",
            "DailyAgingManpower",
            "SupplyConvoyHunt",
            "SupplyNavalStaticAoD",
            "SupplyNavalMoving",
            "SupplyNavalBattleAoD",
            "SupplyAirStaticAoD",
            "SupplyAirMoving",
            "SupplyAirBattleAoD",
            "SupplyAirBombing",
            "SupplyLandStaticAoD",
            "SupplyLandMoving",
            "SupplyLandBattleAoD",
            "SupplyLandBombing",
            "SupplyStockLand",
            "SupplyStockAir",
            "SupplyStockNaval",
            "RestockSpeedLand",
            "RestockSpeedAir",
            "RestockSpeedNaval",
            "SyntheticOilConversionMultiplier",
            "SyntheticRaresConversionMultiplier",
            "MilitarySalary",
            "MaxIntelligenceExpenditure",
            "MaxResearchExpenditure",
            "MilitarySalaryAttrictionModifier",
            "MilitarySalaryDissentModifier",
            "NuclearSiteUpkeepCost",
            "NuclearPowerUpkeepCost",
            "SyntheticOilSiteUpkeepCost",
            "SyntheticRaresSiteUpkeepCost",
            "DurationDetection",
            "ConvoyProvinceHostileTime",
            "ConvoyProvinceBlockedTime",
            "AutoTradeConvoy",
            "SpyUpkeepCost",
            "SpyDetectionChance",
            "SpyCoupDissentModifier",
            "InfraEfficiencyModifier",
            "ManpowerToConsumerGoods",
            "TimeBetweenSliderChangesAoD",
            "MinimalPlacementIc",
            "NuclearPower",
            "FreeInfraRepair",
            "MaxSliderDissent",
            "MinSliderDissent",
            "MaxDissentSliderMove",
            "IcConcentrationBonus",
            "TransportConversion",
            "ConvoyDutyConversion",
            "EscortDutyConversion",
            "MinisterChangeDelay",
            "MinisterChangeEventDelay",
            "IdeaChangeDelay",
            "IdeaChangeEventDelay",
            "LeaderChangeDelay",
            "ChangeIdeaDissent",
            "ChangeMinisterDissent",
            "MinDissentRevolt",
            "DissentRevoltMultiplier",
            "TpMaxAttach",
            "SsMaxAttach",
            "SsnMaxAttach",
            "DdMaxAttach",
            "ClMaxAttach",
            "CaMaxAttach",
            "BcMaxAttach",
            "BbMaxAttach",
            "CvlMaxAttach",
            "CvMaxAttach",
            "CanChangeIdeas",
            "CanUnitSendNonAlliedDh",
            "BluePrintsCanSoldNonAllied",
            "ProvinceCanSoldNonAllied",
            "TransferAlliedCoreProvinces",
            "ProvinceBuildingsRepairModifier",
            "ProvinceResourceRepairModifier",
            "StockpileLimitMultiplierResource",
            "StockpileLimitMultiplierSuppliesOil",
            "OverStockpileLimitDailyLoss",
            "MaxResourceDepotSize",
            "MaxSuppliesOilDepotSize",
            "DesiredStockPilesSuppliesOil",
            "MaxManpower",
            "ConvoyTransportsCapacity",
            "SuppyLandStaticDh",
            "SupplyLandBattleDh",
            "FuelLandStatic",
            "FuelLandBattle",
            "SupplyAirStaticDh",
            "SupplyAirBattleDh",
            "FuelAirNavalStatic",
            "FuelAirBattle",
            "SupplyNavalStaticDh",
            "SupplyNavalBattleDh",
            "FuelNavalNotMoving",
            "FuelNavalBattle",
            "TpTransportsConversionRatio",
            "DdEscortsConversionRatio",
            "ClEscortsConversionRatio",
            "CvlEscortsConversionRatio",
            "ProductionLineEdit",
            "GearingBonusLossUpgradeUnit",
            "GearingBonusLossUpgradeBrigade",
            "DissentNukes",
            "MaxDailyDissent",
            "NukesProductionModifier",
            "ConvoySystemOptionsAllied",
            "ResourceConvoysBackUnneeded",
            "SpyMissionDaysDh",
            "IncreateIntelligenceLevelDaysDh",
            "ChanceDetectSpyMissionDh",
            "RelationshipsHitDetectedMissionsDh",
            "DistanceModifier",
            "DistanceModifierNeighboursDh",
            "SpyLevelBonusDistanceModifier",
            "SpyLevelBonusDistanceModifierAboveTen",
            "SpyInformationAccuracyModifierDh",
            "IcModifierCost",
            "MinIcCostModifier",
            "MaxIcCostModifierDh",
            "ExtraMaintenanceCostAboveTen",
            "ExtraCostIncreasingAboveTen",
            "ShowThirdCountrySpyReportsDh",
            "SpiesMoneyModifier",
            "DaysBetweenDiplomaticMissions",
            "TimeBetweenSliderChangesDh",
            "RequirementAffectSliderDh",
            "UseMinisterPersonalityReplacing",
            "RelationshipHitCancelTrade",
            "RelationshipHitCancelPermanentTrade",
            "PuppetsJoinMastersAlliance",
            "MastersBecomePuppetsPuppets",
            "AllowManualClaimsChange",
            "BelligerenceClaimedProvince",
            "BelligerenceClaimsRemoval",
            "JoinAutomaticallyAllesAxis",
            "AllowChangeHosHog",
            "ChangeTagCoup",
            "FilterReleaseCountries",
            "LandXpGainFactor",
            "NavalXpGainFactor",
            "AirXpGainFactor",
            "AirDogfightXpGainFactor",
            "DivisionXpGainFactor",
            "LeaderXpGainFactor",
            "AttritionSeverityModifier",
            "NoSupplyAttritionSeverity",
            "NoSupplyMinimunAttrition",
            "BaseProximity",
            "ShoreBombardmentModifier",
            "ShoreBombardmentCap",
            "InvasionModifier",
            "MultipleCombatModifier",
            "OffensiveCombinedArmsBonus",
            "DefensiveCombinedArmsBonus",
            "SurpriseModifier",
            "LandCommandLimitModifier",
            "AirCommandLimitModifier",
            "NavalCommandLimitModifier",
            "EnvelopmentModifier",
            "EncircledModifier",
            "LandFortMultiplier",
            "CoastalFortMultiplier",
            "HardUnitsAttackingUrbanPenalty",
            "DissentMultiplier",
            "SupplyProblemsModifier",
            "SupplyProblemsModifierLand",
            "SupplyProblemsModifierAir",
            "SupplyProblemsModifierNaval",
            "FuelProblemsModifierLand",
            "FuelProblemsModifierAir",
            "FuelProblemsModifierNaval",
            "RaderStationMultiplier",
            "RaderStationAaMultiplier",
            "InterceptorBomberModifier",
            "AirOverstackingModifier",
            "AirOverstackingModifierAoD",
            "NavalOverstackingModifier",
            "LandLeaderCommandLimitRank0",
            "LandLeaderCommandLimitRank1",
            "LandLeaderCommandLimitRank2",
            "LandLeaderCommandLimitRank3",
            "AirLeaderCommandLimitRank0",
            "AirLeaderCommandLimitRank1",
            "AirLeaderCommandLimitRank2",
            "AirLeaderCommandLimitRank3",
            "NavalLeaderCommandLimitRank0",
            "NavalLeaderCommandLimitRank1",
            "NavalLeaderCommandLimitRank2",
            "NavalLeaderCommandLimitRank3",
            "HqCommandLimitFactor",
            "ConvoyProtectionFactor",
            "ConvoyEscortsModel",
            "DelayAfterCombatEnds",
            "LandDelayBeforeOrders",
            "NavalDelayBeforeOrders",
            "AirDelayBeforeOrders",
            "MaximumSizesAirStacks",
            "DurationAirToAirBattles",
            "DurationNavalPortBombing",
            "DurationStrategicBombing",
            "DurationGroundAttackBombing",
            "EffectExperienceCombat",
            "DamageNavalBasesBombing",
            "DamageAirBaseBombing",
            "DamageAaBombing",
            "DamageRocketBombing",
            "DamageNukeBombing",
            "DamageRadarBombing",
            "DamageInfraBombing",
            "DamageIcBombing",
            "DamageResourcesBombing",
            "DamageSyntheticOilBombing",
            "HowEffectiveGroundDef",
            "ChanceAvoidDefencesLeft",
            "ChanceAvoidNoDefences",
            "LandChanceAvoidDefencesLeft",
            "AirChanceAvoidDefencesLeft",
            "NavalChanceAvoidDefencesLeft",
            "LandChanceAvoidNoDefences",
            "AirChanceAvoidNoDefences",
            "NavalChanceAvoidNoDefences",
            "ChanceGetTerrainTrait",
            "ChanceGetEventTrait",
            "BonusTerrainTrait",
            "BonusSimilarTerrainTrait",
            "BonusEventTrait",
            "BonusLeaderSkillPointLand",
            "BonusLeaderSkillPointAir",
            "BonusLeaderSkillPointNaval",
            "ChanceLeaderDying",
            "AirOrgDamage",
            "AirStrDamageOrg",
            "AirStrDamage",
            "LandMinOrgDamage",
            "LandOrgDamageHardSoftEach",
            "LandOrgDamageHardVsSoft",
            "LandMinStrDamage",
            "LandStrDamageHardSoftEach",
            "LandStrDamageHardVsSoft",
            "AirMinOrgDamage",
            "AirAdditionalOrgDamage",
            "AirMinStrDamage",
            "AirAdditionalStrDamage",
            "AirStrDamageEntrenced",
            "NavalMinOrgDamage",
            "NavalAdditionalOrgDamage",
            "NavalMinStrDamage",
            "NavalAdditionalStrDamage",
            "AirStrDamageLandOrg",
            "AirOrgDamageLandDh",
            "AirStrDamageLandDh",
            "LandOrgDamageLandOrg",
            "LandOrgDamageLandUrban",
            "LandOrgDamageLandFort",
            "RequiredLandFortSize",
            "LandStrDamageLandDh",
            "AirOrgDamageAirDh",
            "AirStrDamageAirDh",
            "LandOrgDamageAirDh",
            "LandStrDamageAirDh",
            "NavalOrgDamageAirDh",
            "NavalStrDamageAirDh",
            "SubsOrgDamageAir",
            "SubsStrDamageAir",
            "AirOrgDamageNavyDh",
            "AirStrDamageNavyDh",
            "NavalOrgDamageNavyDh",
            "NavalStrDamageNavyDh",
            "SubsOrgDamageNavy",
            "SubsStrDamageNavy",
            "SubsOrgDamage",
            "SubsStrDamage",
            "SubStacksDetectionModifier",
            "AirOrgDamageLandAoD",
            "AirStrDamageLandAoD",
            "LandDamageArtilleryBombardment",
            "InfraDamageArtilleryBombardment",
            "IcDamageArtilleryBombardment",
            "ResourcesDamageArtilleryBombardment",
            "PenaltyArtilleryBombardment",
            "ArtilleryStrDamage",
            "ArtilleryOrgDamage",
            "LandStrDamageLandAoD",
            "LandOrgDamageLand",
            "LandStrDamageAirAoD",
            "LandOrgDamageAirAoD",
            "NavalStrDamageAirAoD",
            "NavalOrgDamageAirAoD",
            "AirStrDamageAirAoD",
            "AirOrgDamageAirAoD",
            "NavalStrDamageNavyAoD",
            "NavalOrgDamageNavyAoD",
            "AirStrDamageNavyAoD",
            "AirOrgDamageNavyAoD",
            "MilitaryExpenseAttritionModifier",
            "NavalMinCombatTime",
            "LandMinCombatTime",
            "AirMinCombatTime",
            "LandOverstackingModifier",
            "LandOrgLossMoving",
            "AirOrgLossMoving",
            "NavalOrgLossMoving",
            "SupplyDistanceSeverity",
            "SupplyBase",
            "LandOrgGain",
            "AirOrgGain",
            "NavalOrgGain",
            "NukeManpowerDissent",
            "NukeIcDissent",
            "NukeTotalDissent",
            "LandFriendlyOrgGain",
            "AirLandStockModifier",
            "ScorchDamage",
            "StandGroundDissent",
            "ScorchGroundBelligerence",
            "DefaultLandStack",
            "DefaultNavalStack",
            "DefaultAirStack",
            "DefaultRocketStack",
            "FortDamageArtilleryBombardment",
            "ArtilleryBombardmentOrgCost",
            "LandDamageFort",
            "AirRebaseFactor",
            "AirMaxDisorganized",
            "AaInflictedStrDamage",
            "AaInflictedOrgDamage",
            "AaInflictedFlyingDamage",
            "AaInflictedBombingDamage",
            "HardAttackStrDamage",
            "HardAttackOrgDamage",
            "ArmorSoftBreakthroughMin",
            "ArmorSoftBreakthroughMax",
            "NavalCriticalHitChance",
            "NavalCriticalHitEffect",
            "LandFortDamage",
            "PortAttackSurpriseChanceDay",
            "PortAttackSurpriseChanceNight",
            "PortAttackSurpriseModifier",
            "RadarAntiSurpriseChance",
            "RadarAntiSurpriseModifier",
            "CounterAttackStrDefenderAoD",
            "CounterAttackOrgDefenderAoD",
            "CounterAttackStrAttackerAoD",
            "CounterAttackOrgAttackerAoD",
            "AssaultStrDefenderAoD",
            "AssaultOrgDefenderAoD",
            "AssaultStrAttackerAoD",
            "AssaultOrgAttackerAoD",
            "EncirclementStrDefenderAoD",
            "EncirclementOrgDefenderAoD",
            "EncirclementStrAttackerAoD",
            "EncirclementOrgAttackerAoD",
            "AmbushStrDefenderAoD",
            "AmbushOrgDefenderAoD",
            "AmbushStrAttackerAoD",
            "AmbushOrgAttackerAoD",
            "DelayStrDefenderAoD",
            "DelayOrgDefenderAoD",
            "DelayStrAttackerAoD",
            "DelayOrgAttackerAoD",
            "TacticalWithdrawStrDefenderAoD",
            "TacticalWithdrawOrgDefenderAoD",
            "TacticalWithdrawStrAttackerAoD",
            "TacticalWithdrawOrgAttackerAoD",
            "BreakthroughStrDefenderAoD",
            "BreakthroughOrgDefenderAoD",
            "BreakthroughStrAttackerAoD",
            "BreakthroughOrgAttackerAoD",
            "NavalOrgDamageAa",
            "AirOrgDamageAa",
            "AirStrDamageAa",
            "AaAirFiringRules",
            "AaAirNightModifier",
            "AaAirBonusRadars",
            "MovementBonusTerrainTrait",
            "MovementBonusSimilarTerrainTrait",
            "LogisticsWizardEseBonus",
            "DaysOffensiveSupply",
            "MinisterBonuses",
            "OrgRegainBonusFriendly",
            "OrgRegainBonusFriendlyCap",
            "ConvoyInterceptionMissions",
            "AutoReturnTransportFleets",
            "AllowProvinceRegionTargeting",
            "NightHoursWinter",
            "NightHoursSpringFall",
            "NightHoursSummer",
            "RecalculateLandArrivalTimes",
            "SynchronizeArrivalTimePlayer",
            "SynchronizeArrivalTimeAi",
            "RecalculateArrivalTimesCombat",
            "LandSpeedModifierCombat",
            "LandSpeedModifierBombardment",
            "LandSpeedModifierSupply",
            "LandSpeedModifierOrg",
            "LandAirSpeedModifierFuel",
            "DefaultSpeedFuel",
            "FleetSizeRangePenaltyRatio",
            "FleetSizeRangePenaltyThrethold",
            "FleetSizeRangePenaltyMax",
            "ApplyRangeLimitsAreasRegions",
            "RadarBonusDetection",
            "BonusDetectionFriendly",
            "ScreensCapitalRatioModifier",
            "ChanceTargetNoOrgLand",
            "ScreenCapitalShipsTargeting",
            "FleetPositioningDaytime",
            "FleetPositioningLeaderSkill",
            "FleetPositioningFleetSize",
            "FleetPositioningFleetComposition",
            "LandCoastalFortsDamage",
            "LandCoastalFortsMaxDamage",
            "MinSoftnessBrigades",
            "AutoRetreatOrg",
            "LandOrgNavalTransportation",
            "MaxLandDig",
            "DigIncreaseDay",
            "BreakthroughEncirclementMinSpeed",
            "BreakthroughEncirclementMaxChance",
            "BreakthroughEncirclementChanceModifier",
            "CombatEventDuration",
            "CounterAttackOrgAttackerDh",
            "CounterAttackStrAttackerDh",
            "CounterAttackOrgDefenderDh",
            "CounterAttackStrDefenderDh",
            "AssaultOrgAttackerDh",
            "AssaultStrAttackerDh",
            "AssaultOrgDefenderDh",
            "AssaultStrDefenderDh",
            "EncirclementOrgAttackerDh",
            "EncirclementStrAttackerDh",
            "EncirclementOrgDefenderDh",
            "EncirclementStrDefenderDh",
            "AmbushOrgAttackerDh",
            "AmbushStrAttackerDh",
            "AmbushOrgDefenderDh",
            "AmbushStrDefenderDh",
            "DelayOrgAttackerDh",
            "DelayStrAttackerDh",
            "DelayOrgDefenderDh",
            "DelayStrDefenderDh",
            "TacticalWithdrawOrgAttackerDh",
            "TacticalWithdrawStrAttackerDh",
            "TacticalWithdrawOrgDefenderDh",
            "TacticalWithdrawStrDefenderDh",
            "BreakthroughOrgAttackerDh",
            "BreakthroughStrAttackerDh",
            "BreakthroughOrgDefenderDh",
            "BreakthroughStrDefenderDh",
            "HqStrDamageBreakthrough",
            "CombatMode",
            "AttackMission",
            "AttackStartingEfficiency",
            "AttackSpeedBonus",
            "RebaseMission",
            "RebaseStartingEfficiency",
            "RebaseChanceDetected",
            "StratRedeployMission",
            "StratRedeployStartingEfficiency",
            "StratRedeployAddedValue",
            "StratRedeployDistanceMultiplier",
            "SupportAttackMission",
            "SupportAttackStartingEfficiency",
            "SupportAttackSpeedBonus",
            "SupportDefenseMission",
            "SupportDefenseStartingEfficiency",
            "SupportDefenseSpeedBonus",
            "ReservesMission",
            "ReservesStartingEfficiency",
            "ReservesSpeedBonus",
            "AntiPartisanDutyMission",
            "AntiPartisanDutyStartingEfficiency",
            "AntiPartisanDutySuppression",
            "PlannedDefenseMission",
            "PlannedDefenseStartingEfficiency",
            "AirSuperiorityMission",
            "AirSuperiorityStartingEfficiency",
            "AirSuperiorityDetection",
            "AirSuperiorityMinRequired",
            "GroundAttackMission",
            "GroundAttackStartingEfficiency",
            "GroundAttackOrgDamage",
            "GroundAttackStrDamage",
            "InterdictionMission",
            "InterdictionStartingEfficiency",
            "InterdictionOrgDamage",
            "InterdictionStrDamage",
            "StrategicBombardmentMission",
            "StrategicBombardmentStartingEfficiency",
            "LogisticalStrikeMission",
            "LogisticalStrikeStartingEfficiency",
            "RunwayCrateringMission",
            "RunwayCrateringStartingEfficiency",
            "InstallationStrikeMission",
            "InstallationStrikeStartingEfficiency",
            "NavalStrikeMission",
            "NavalStrikeStartingEfficiency",
            "PortStrikeMission",
            "PortStrikeStartingEfficiency",
            "ConvoyAirRaidingMission",
            "ConvoyAirRaidingStartingEfficiency",
            "AirSupplyMission",
            "AirSupplyStartingEfficiency",
            "AirborneAssaultMission",
            "AirborneAssaultStartingEfficiency",
            "NukeMission",
            "NukeStartingEfficiency",
            "AirScrambleMission",
            "AirScrambleStartingEfficiency",
            "AirScrambleDetection",
            "AirScrambleMinRequired",
            "ConvoyRadingMission",
            "ConvoyRadingStartingEfficiency",
            "ConvoyRadingRangeModifier",
            "ConvoyRadingChanceDetected",
            "AswMission",
            "AswStartingEfficiency",
            "NavalInterdictionMission",
            "NavalInterdictionStartingEfficiency",
            "ShoreBombardmentMission",
            "ShoreBombardmentStartingEfficiency",
            "ShoreBombardmentModifierDh",
            "AmphibousAssaultMission",
            "AmphibousAssaultStartingEfficiency",
            "SeaTransportMission",
            "SeaTransportStartingEfficiency",
            "SeaTransportRangeModifier",
            "SeaTransportChanceDetected",
            "NavalCombatPatrolMission",
            "NavalCombatPatrolStartingEfficiency",
            "NavalPortStrikeMission",
            "NavalPortStrikeStartingEfficiency",
            "NavalAirbaseStrikeMission",
            "NavalAirbaseStrikeStartingEfficiency",
            "SneakMoveMission",
            "SneakMoveStartingEfficiency",
            "SneakMoveRangeModifier",
            "SneakMoveChanceDetected",
            "NavalScrambleMission",
            "NavalScrambleStartingEfficiency",
            "NavalScrambleSpeedBonus",
            "UseAttackEfficiencyCombatModifier",
            "LandFortEfficiency",
            "CoastalFortEfficiency",
            "GroundDefenseEfficiency",
            "ConvoyDefenseEfficiency",
            "ManpowerBoost",
            "TransportCapacityModifier",
            "OccupiedTransportCapacityModifier",
            "AttritionModifier",
            "ManpowerTrickleBackModifier",
            "SupplyDistanceModifier",
            "RepairModifier",
            "ResearchModifier",
            "RadarEfficiency",
            "HqSupplyEfficiencyBonus",
            "HqCombatEventsBonus",
            "CombatEventChances",
            "FriendlyArmyDetectionChance",
            "EnemyArmyDetectionChance",
            "FriendlyIntelligenceChance",
            "EnemyIntelligenceChance",
            "MaxAmphibiousArmySize",
            "EnergyToOil",
            "TotalProductionEfficiency",
            "SupplyProductionEfficiency",
            "AaPower",
            "AirSurpriseChance",
            "LandSurpriseChance",
            "NavalSurpriseChance",
            "PeacetimeIcModifier",
            "WartimeIcModifier",
            "BuildingsProductionModifier",
            "ConvoysProductionModifier",
            "MinShipsPositioningBattle",
            "MaxShipsPositioningBattle",
            "PeacetimeStockpilesResources",
            "WartimeStockpilesResources",
            "PeacetimeStockpilesOilSupplies",
            "WartimeStockpilesOilSupplies",
            "BlueprintBonus",
            "PreHistoricalDateModifier",
            "PostHistoricalDateModifierDh",
            "CostSkillLevel",
            "MeanNumberInventionEventsYear",
            "PostHistoricalDateModifierAoD",
            "TechSpeedModifier",
            "PreHistoricalPenaltyLimit",
            "PostHistoricalBonusLimit",
            "MaxActiveTechTeamsAoD",
            "RequiredIcEachTechTeamAoD",
            "MaximumRandomModifier",
            "UseNewTechnologyPageLayout",
            "TechOverviewPanelStyle",
            "MaxActiveTechTeamsDh",
            "MinActiveTechTeams",
            "RequiredIcEachTechTeamDh",
            "NewCountryRocketryComponent",
            "NewCountryNuclearPhysicsComponent",
            "NewCountryNuclearEngineeringComponent",
            "NewCountrySecretTechs",
            "MaxTechTeamSkill",
            "DaysTradeOffers",
            "DelayGameStartNewTrades",
            "LimitAiNewTradesGameStart",
            "DesiredOilStockpile",
            "CriticalOilStockpile",
            "DesiredSuppliesStockpile",
            "CriticalSuppliesStockpile",
            "DesiredResourcesStockpile",
            "CriticalResourceStockpile",
            "WartimeDesiredStockpileMultiplier",
            "PeacetimeExtraOilImport",
            "WartimeExtraOilImport",
            "ExtraImportBelowDesired",
            "PercentageProducedSupplies",
            "PercentageProducedMoney",
            "ExtraImportStockpileSelected",
            "DaysDeliverResourcesTrades",
            "MergeTradeDeals",
            "ManualTradeDeals",
            "PuppetsSendSuppliesMoney",
            "PuppetsCriticalSupplyStockpile",
            "PuppetsMaxPoolResources",
            "NewTradeDealsMinEffectiveness",
            "CancelTradeDealsEffectiveness",
            "AutoTradeAiTradeDeals",
            "OverproduceSuppliesBelowDesired",
            "MultiplierOverproduceSuppliesWar",
            "NotProduceSuppliesStockpileOver",
            "MaxSerialLineProductionGarrisonMilitia",
            "MinIcSerialProductionNavalAir",
            "NotProduceNewUnitsManpowerRatio",
            "NotProduceNewUnitsManpowerValue",
            "NotProduceNewUnitsSupply",
            "MilitaryStrengthTotalIcRatioPeacetime",
            "MilitaryStrengthTotalIcRatioWartime",
            "MilitaryStrengthTotalIcRatioMajor",
            "NotUseOffensiveSupplyStockpile",
            "NotUseOffensiveOilStockpile",
            "NotUseOffensiveEse",
            "NotUseOffensiveOrgStrDamage",
            "AiPeacetimeSpyMissionsDh",
            "AiSpyMissionsCostModifierDh",
            "AiDiplomacyCostModifierDh",
            "AiInfluenceModifierDh",
            "NewDowRules",
            "NewDowRules2",
            "ForcePuppetsJoinMastersAllianceNeutrality",
            "NewAiReleaseRules",
            "AiEventsActionSelectionRules",
            "ForceStrategicRedeploymentHour",
            "MaxRedeploymentDaysAi",
            "UseQuickAreaCheckGarrisonAi",
            "AiMastersGetProvincesConquredPuppets",
            "MinDaysRequiredAiReleaseCountry",
            "MinDaysRequiredAiAllied",
            "MinDaysRequiredAiAlliedSupplyBase",
            "MinRequiredRelationsAlliedClaimed",
            "AiSpyDiplomaticMissionLogger",
            "CountryLogger",
            "SwitchedAiFilesLogger",
            "UseNewAutoSaveFileFormat",
            "LoadNewAiSwitchingAllClients",
            "TradeEfficiencyCalculationSystem",
            "MergeRelocateProvincialDepots",
            "InGameLossesLogging",
            "InGameLossLogging2",
            "AllowBrigadeAttachingInSupply",
            "MultipleDeploymentSizeArmies",
            "MultipleDeploymentSizeFleets",
            "MultipleDeploymentSizeAir",
            "AllowUniquePicturesAllLandProvinces",
            "AutoReplyEvents",
            "ForceActionsShow",
            "EnableDicisionsPlayers",
            "RebelsArmyComposition",
            "RebelsArmyTechLevel",
            "RebelsArmyMinStr",
            "RebelsArmyMaxStr",
            "RebelsOrgRegain",
            "ExtraRebelBonusNeighboringProvince",
            "ExtraRebelBonusOccupied",
            "ExtraRebelBonusMountain",
            "ExtraRebelBonusHill",
            "ExtraRebelBonusForest",
            "ExtraRebelBonusJungle",
            "ExtraRebelBonusSwamp",
            "ExtraRebelBonusDesert",
            "ExtraRebelBonusPlain",
            "ExtraRebelBonusUrban",
            "ExtraRebelBonusAirNavalBases",
            "ReturnRebelliousProvince",
            "UseNewMinisterFilesFormat",
            "EnableRetirementYearMinisters",
            "EnableRetirementYearLeaders",
            "LoadSpritesModdirOnly",
            "LoadUnitIconsModdirOnly",
            "LoadUnitPicturesModdirOnly",
            "LoadAiFilesModdirOnly",
            "UseSpeedSetGarrisonStatus",
            "UseOldSaveGameFormat",
            "ProductionPanelUiStyle",
            "UnitPicturesSize",
            "EnablePicturesNavalBrigades",
            "BuildingsBuildableOnlyProvinces",
            "UnitModifiersStatisticsPages",
            "MapNumber",
            "TotalProvinces",
            "DistanceCalculationModel",
            "MapWidth",
            "MapHeight"
        };

        #endregion

        #region 内部定数

        /// <summary>
        ///     セクション名
        /// </summary>
        private static readonly string[] SectionNames =
        {
            "Economy",
            "Intelligence",
            "Diplomacy",
            "Combat",
            "Mission",
            "Country",
            "Research",
            "Trade",
            "Ai",
            "Mod",
            "Map"
        };

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     miscファイルの再読み込みを要求する
        /// </summary>
        /// <remarks>
        ///     ゲームフォルダ、MOD名、ゲーム種類の変更があった場合に呼び出す
        /// </remarks>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     miscファイルを再読み込みする
        /// </summary>
        public static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            Load();
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

            // セクション末尾の文字列を初期化する
            _suffixes = new string[Enum.GetValues(typeof (MiscSectionId)).Length];

            // miscファイルを解釈する
            string fileName = Game.GetReadFileName(Game.MiscPathName);
            Debug.WriteLine(string.Format("[Misc] Load: {0}", Path.GetFileName(fileName)));
            try
            {
                MiscParser.Parse(fileName);
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("[Misc] Read error: {0}", fileName));
                Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                    Resources.EditorMisc, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return true;
            }

            string fileName = Game.GetWriteFileName(Game.MiscPathName);
            try
            {
                // dbフォルダがなければ作成する
                string folderName = Game.GetWriteFileName(Game.DatabasePathName);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                // miscファイルを保存する
                Debug.WriteLine(string.Format("[Misc] Save: {0}", Path.GetFileName(fileName)));
                MiscWriter.Write(fileName);
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("[Misc] Write error: {0}", fileName));
                Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                    Resources.EditorMisc, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            return true;
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

        /// <summary>
        ///     項目の空白文字/コメントを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>項目の空白文字/コメント</returns>
        public static string GetComment(MiscItemId id)
        {
            return _comments[(int) id];
        }

        /// <summary>
        ///     項目の空白文字/コメントを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <param name="s">設定する文字列</param>
        public static void SetComment(MiscItemId id, string s)
        {
            _comments[(int) id] = s;
        }

        /// <summary>
        ///     セクション末尾の空白文字/コメントを取得する
        /// </summary>
        /// <param name="section">セクションID</param>
        /// <returns>セクション末尾の空白文字/コメント</returns>
        public static string GetSuffix(MiscSectionId section)
        {
            return _suffixes[(int) section];
        }

        /// <summary>
        ///     セクション末尾の空白文字/コメントを設定する
        /// </summary>
        /// <param name="section">セクションID</param>
        /// <param name="s">設定する文字列</param>
        public static void SetSuffix(MiscSectionId section, string s)
        {
            _suffixes[(int) section] = s;
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
        ///     セクション名を取得する
        /// </summary>
        /// <param name="section">セクション</param>
        /// <returns>セクション名</returns>
        public static string GetSectionName(MiscSectionId section)
        {
            return HoI2Editor.GetResourceString("MiscSection" + SectionNames[(int) section]);
        }

        /// <summary>
        ///     項目名を取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>項目名</returns>
        public static string GetItemName(MiscItemId id)
        {
            return HoI2Editor.GetResourceString("MiscLabel" + ItemNames[(int) id]);
        }

        /// <summary>
        ///     項目のツールチップ文字列を取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>ツールチップ文字列</returns>
        public static string GetItemToolTip(MiscItemId id)
        {
            return HoI2Editor.GetResourceString("MiscToolTip" + ItemNames[(int) id]);
        }

        /// <summary>
        ///     項目の選択肢の文字列を取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <param name="index">選択肢のインデックス</param>
        /// <returns>選択肢の文字列</returns>
        public static string GetItemChoice(MiscItemId id, int index)
        {
            return index.ToString(CultureInfo.InvariantCulture) + ": "
                   + HoI2Editor.GetResourceString("MiscEnum" + ItemNames[(int) id]
                                                  + index.ToString(CultureInfo.InvariantCulture));
        }

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

        // map
        MapNumber, // マップ番号
        TotalProvinces, // 総プロヴィンス数
        DistanceCalculationModel, // 距離算出方法
        MapWidth, // マップの幅
        MapHeight, // マップの高さ
    }

    /// <summary>
    ///     miscセクションID
    /// </summary>
    public enum MiscSectionId
    {
        Economy, // 経済
        Intelligence, // 諜報
        Diplomacy, // 外交
        Combat, // 戦闘
        Mission, // 任務
        Country, // 国家
        Research, // 研究
        Trade, // 貿易
        Ai, // AI
        Mod, // MOD
        Map, // マップ
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