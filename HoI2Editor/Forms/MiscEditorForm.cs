using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     基礎データエディタのフォーム
    /// </summary>
    public partial class MiscEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     項目IDと編集コントロールとの対応付け
        /// </summary>
        private readonly Control[] _edits = new Control[Enum.GetValues(typeof (MiscItemId)).Length];

        /// <summary>
        ///     項目IDと編集ラベルとの対応付け
        /// </summary>
        private readonly Label[] _labels = new Label[Enum.GetValues(typeof (MiscItemId)).Length];

        #endregion

        #region 内部定数

        /// <summary>
        ///     編集項目のID
        /// </summary>
        private static readonly MiscItemId[][][] EditableItems =
        {
            // 経済1
            new[]
            {
                new[]
                {
                    MiscItemId.IcToTcRatio,
                    MiscItemId.IcToSuppliesRatio,
                    MiscItemId.IcToConsumerGoodsRatio,
                    MiscItemId.IcToMoneyRatio,
                    MiscItemId.MaxGearingBonus,
                    MiscItemId.GearingBonusIncrement,
                    MiscItemId.IcMultiplierNonNational,
                    MiscItemId.IcMultiplierNonOwned,
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
                    MiscItemId.RequirementAffectSlider,
                    MiscItemId.TrickleBackFactorManpower,
                    MiscItemId.ReinforceManpower,
                    MiscItemId.ReinforceCost
                },
                new[]
                {
                    MiscItemId.ReinforceTime,
                    MiscItemId.UpgradeCost,
                    MiscItemId.UpgradeTime,
                    MiscItemId.NationalismStartingValue,
                    MiscItemId.MonthlyNationalismReduction,
                    MiscItemId.SendDivisionDays,
                    MiscItemId.TcLoadUndeployedBrigade,
                    MiscItemId.CanUnitSendNonAllied,
                    MiscItemId.Separator,
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
                    MiscItemId.AiInfluenceModifier
                },
                new[]
                {
                    MiscItemId.CoreProvinceEfficiencyRiseTime,
                    MiscItemId.RestockSpeedLand,
                    MiscItemId.RestockSpeedAir,
                    MiscItemId.RestockSpeedNaval,
                    MiscItemId.SpyCoupDissentModifier,
                    MiscItemId.ConvoyDutyConversion,
                    MiscItemId.EscortDutyConversion,
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
                    MiscItemId.CanChangeIdeas
                }
            },
            // 経済2
            new[]
            {
                new[]
                {
                    MiscItemId.DissentChangeSpeed,
                    MiscItemId.GearingResourceIncrement,
                    MiscItemId.GearingLossNoIc,
                    MiscItemId.CostRepairBuildings,
                    MiscItemId.TimeRepairBuilding,
                    MiscItemId.ProvinceEfficiencyRiseTime,
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
                    MiscItemId.SupplyLandBombing
                },
                new[]
                {
                    MiscItemId.SupplyStockLand,
                    MiscItemId.SupplyStockAir,
                    MiscItemId.SupplyStockNaval,
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
                    MiscItemId.InfraEfficiencyModifier,
                    MiscItemId.ManpowerToConsumerGoods,
                    MiscItemId.TimeBetweenSliderChangesAoD,
                    MiscItemId.MinimalPlacementIc
                },
                new[]
                {
                    MiscItemId.NuclearPower,
                    MiscItemId.FreeInfraRepair,
                    MiscItemId.MaxSliderDissent,
                    MiscItemId.MinSliderDissent,
                    MiscItemId.MaxDissentSliderMove,
                    MiscItemId.IcConcentrationBonus,
                    MiscItemId.TransportConversion,
                    MiscItemId.MinisterChangeDelay,
                    MiscItemId.MinisterChangeEventDelay,
                    MiscItemId.IdeaChangeDelay,
                    MiscItemId.IdeaChangeEventDelay,
                    MiscItemId.LeaderChangeDelay,
                    MiscItemId.ChangeIdeaDissent,
                    MiscItemId.ChangeMinisterDissent,
                    MiscItemId.MinDissentRevolt,
                    MiscItemId.DissentRevoltMultiplier
                }
            },
            // 経済3
            new[]
            {
                new[]
                {
                    MiscItemId.MinAvailableIc,
                    MiscItemId.MinFinalIc,
                    MiscItemId.DissentReduction,
                    MiscItemId.IcMultiplierPuppet,
                    MiscItemId.ResourceMultiplierNonNational,
                    MiscItemId.ResourceMultiplierNonOwned,
                    MiscItemId.ResourceMultiplierNonNationalAi,
                    MiscItemId.ResourceMultiplierPuppet,
                    MiscItemId.ManpowerMultiplierPuppet,
                    MiscItemId.ManpowerMultiplierWartimeOversea,
                    MiscItemId.ManpowerMultiplierPeacetime,
                    MiscItemId.ManpowerMultiplierWartime,
                    MiscItemId.DailyRetiredManpower,
                    MiscItemId.ReinforceToUpdateModifier,
                    MiscItemId.NationalismPerManpowerDh,
                    MiscItemId.MaxNationalism,
                    MiscItemId.MaxRevoltRisk,
                    MiscItemId.CanUnitSendNonAlliedDh,
                    MiscItemId.BluePrintsCanSoldNonAllied,
                    MiscItemId.ProvinceCanSoldNonAllied,
                    MiscItemId.TransferAlliedCoreProvinces,
                    MiscItemId.ProvinceBuildingsRepairModifier,
                    MiscItemId.ProvinceResourceRepairModifier,
                    MiscItemId.StockpileLimitMultiplierResource
                },
                new[]
                {
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
                    MiscItemId.ProductionLineEdit
                },
                new[]
                {
                    MiscItemId.GearingBonusLossUpgradeUnit,
                    MiscItemId.GearingBonusLossUpgradeBrigade,
                    MiscItemId.DissentNukes,
                    MiscItemId.MaxDailyDissent,
                    MiscItemId.Separator,
                    MiscItemId.NukesProductionModifier,
                    MiscItemId.ConvoySystemOptionsAllied,
                    MiscItemId.ResourceConvoysBackUnneeded
                }
            },
            // 諜報
            new[]
            {
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
                }
            },
            // 外交
            new[]
            {
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
                }
            },
            // 戦闘1
            new[]
            {
                new[]
                {
                    MiscItemId.LandXpGainFactor,
                    MiscItemId.NavalXpGainFactor,
                    MiscItemId.AirXpGainFactor,
                    MiscItemId.DivisionXpGainFactor,
                    MiscItemId.LeaderXpGainFactor,
                    MiscItemId.AttritionSeverityModifier,
                    MiscItemId.BaseProximity,
                    MiscItemId.ShoreBombardmentModifier,
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
                    MiscItemId.DissentMultiplier,
                    MiscItemId.SupplyProblemsModifier,
                    MiscItemId.RaderStationMultiplier,
                    MiscItemId.InterceptorBomberModifier
                },
                new[]
                {
                    MiscItemId.AirOverstackingModifier,
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
                    MiscItemId.DelayAfterCombatEnds,
                    MiscItemId.MaximumSizesAirStacks,
                    MiscItemId.EffectExperienceCombat,
                    MiscItemId.DamageNavalBasesBombing,
                    MiscItemId.DamageAirBaseBombing,
                    MiscItemId.DamageAaBombing,
                    MiscItemId.DamageRocketBombing,
                    MiscItemId.DamageNukeBombing
                },
                new[]
                {
                    MiscItemId.DamageRadarBombing,
                    MiscItemId.DamageInfraBombing,
                    MiscItemId.DamageIcBombing,
                    MiscItemId.DamageResourcesBombing,
                    MiscItemId.HowEffectiveGroundDef,
                    MiscItemId.ChanceAvoidDefencesLeft,
                    MiscItemId.ChanceAvoidNoDefences,
                    MiscItemId.ChanceGetTerrainTrait,
                    MiscItemId.ChanceGetEventTrait,
                    MiscItemId.BonusTerrainTrait,
                    MiscItemId.BonusEventTrait,
                    MiscItemId.ChanceLeaderDying,
                    MiscItemId.AirOrgDamage,
                    MiscItemId.AirStrDamageOrg,
                    MiscItemId.AirStrDamage,
                    MiscItemId.Separator,
                    MiscItemId.SubsOrgDamage,
                    MiscItemId.SubsStrDamage,
                    MiscItemId.SubStacksDetectionModifier
                }
            },
            // 戦闘2
            new[]
            {
                new[]
                {
                    MiscItemId.NoSupplyAttritionSeverity,
                    MiscItemId.NoSupplyMinimunAttrition,
                    MiscItemId.RaderStationAaMultiplier,
                    MiscItemId.AirOverstackingModifierAoD,
                    MiscItemId.LandDelayBeforeOrders,
                    MiscItemId.NavalDelayBeforeOrders,
                    MiscItemId.AirDelayBeforeOrders,
                    MiscItemId.DamageSyntheticOilBombing,
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
                    MiscItemId.AirStrDamageAirAoD
                },
                new[]
                {
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
                    MiscItemId.ScorchDamage
                },
                new[]
                {
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
                    MiscItemId.PortAttackSurpriseChanceNight
                }
            },
            // 戦闘3
            new[]
            {
                new[]
                {
                    MiscItemId.PortAttackSurpriseModifier,
                    MiscItemId.RadarAntiSurpriseChance,
                    MiscItemId.RadarAntiSurpriseModifier
                },
                new[]
                {
                    MiscItemId.ShoreBombardmentCap,
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
                    MiscItemId.TacticalWithdrawStrAttackerAoD
                },
                new[]
                {
                    MiscItemId.TacticalWithdrawOrgAttackerAoD,
                    MiscItemId.BreakthroughStrDefenderAoD,
                    MiscItemId.BreakthroughOrgDefenderAoD,
                    MiscItemId.BreakthroughStrAttackerAoD,
                    MiscItemId.BreakthroughOrgAttackerAoD
                }
            },
            // 戦闘4
            new[]
            {
                new[]
                {
                    MiscItemId.AirDogfightXpGainFactor,
                    MiscItemId.HardUnitsAttackingUrbanPenalty,
                    MiscItemId.SupplyProblemsModifierLand,
                    MiscItemId.SupplyProblemsModifierAir,
                    MiscItemId.SupplyProblemsModifierNaval,
                    MiscItemId.FuelProblemsModifierLand,
                    MiscItemId.FuelProblemsModifierAir,
                    MiscItemId.FuelProblemsModifierNaval,
                    MiscItemId.ConvoyEscortsModel,
                    MiscItemId.DurationAirToAirBattles,
                    MiscItemId.DurationNavalPortBombing,
                    MiscItemId.DurationStrategicBombing,
                    MiscItemId.DurationGroundAttackBombing,
                    MiscItemId.BonusSimilarTerrainTrait,
                    MiscItemId.AirStrDamageLandOrg,
                    MiscItemId.AirOrgDamageLandDh,
                    MiscItemId.AirStrDamageLandDh,
                    MiscItemId.LandOrgDamageLandOrg,
                    MiscItemId.LandStrDamageLandDh,
                    MiscItemId.AirOrgDamageAirDh,
                    MiscItemId.AirStrDamageAirDh,
                    MiscItemId.LandOrgDamageAirDh,
                    MiscItemId.LandStrDamageAirDh,
                    MiscItemId.NavalOrgDamageAirDh
                },
                new[]
                {
                    MiscItemId.NavalStrDamageAirDh,
                    MiscItemId.SubsOrgDamageAir,
                    MiscItemId.SubsStrDamageAir,
                    MiscItemId.AirOrgDamageNavyDh,
                    MiscItemId.AirStrDamageNavyDh,
                    MiscItemId.NavalOrgDamageNavyDh,
                    MiscItemId.NavalStrDamageNavyDh,
                    MiscItemId.SubsOrgDamageNavy,
                    MiscItemId.SubsStrDamageNavy,
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
                    MiscItemId.AutoReturnTransportFleets
                },
                new[]
                {
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
                    MiscItemId.ScreenCapitalShipsTargeting
                }
            },
            // 戦闘5
            new[]
            {
                new[]
                {
                    MiscItemId.LandChanceAvoidDefencesLeft,
                    MiscItemId.AirChanceAvoidDefencesLeft,
                    MiscItemId.NavalChanceAvoidDefencesLeft,
                    MiscItemId.LandChanceAvoidNoDefences,
                    MiscItemId.AirChanceAvoidNoDefences,
                    MiscItemId.NavalChanceAvoidNoDefences,
                    MiscItemId.BonusLeaderSkillPointLand,
                    MiscItemId.BonusLeaderSkillPointAir,
                    MiscItemId.BonusLeaderSkillPointNaval,
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
                    MiscItemId.NavalAdditionalStrDamage
                },
                new[]
                {
                    MiscItemId.LandOrgDamageLandUrban,
                    MiscItemId.LandOrgDamageLandFort,
                    MiscItemId.RequiredLandFortSize,
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
                    MiscItemId.AssaultStrAttackerDh
                },
                new[]
                {
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
            },
            // 任務1
            new[]
            {
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
                    MiscItemId.PlannedDefenseStartingEfficiency
                },
                new[]
                {
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
                    MiscItemId.PortStrikeStartingEfficiency
                },
                new[]
                {
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
                    MiscItemId.ShoreBombardmentModifierDh
                }
            },
            // 任務2
            new[]
            {
                new[]
                {
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
                    MiscItemId.NavalScrambleSpeedBonus
                },
                new[]
                {
                    MiscItemId.UseAttackEfficiencyCombatModifier
                }
            },
            // 国家
            new[]
            {
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
                    MiscItemId.SupplyProductionEfficiency
                },
                new[]
                {
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
                }
            },
            // 研究
            new[]
            {
                new[]
                {
                    MiscItemId.BlueprintBonus,
                    MiscItemId.PreHistoricalDateModifier,
                    MiscItemId.CostSkillLevel,
                    MiscItemId.MeanNumberInventionEventsYear
                },
                new[]
                {
                    MiscItemId.PostHistoricalDateModifierAoD,
                    MiscItemId.TechSpeedModifier,
                    MiscItemId.PreHistoricalPenaltyLimit,
                    MiscItemId.PostHistoricalBonusLimit,
                    MiscItemId.Separator,
                    MiscItemId.MaxActiveTechTeamsAoD,
                    MiscItemId.RequiredIcEachTechTeamAoD,
                    MiscItemId.Separator,
                    MiscItemId.MaximumRandomModifier
                },
                new[]
                {
                    MiscItemId.PostHistoricalDateModifierDh,
                    MiscItemId.UseNewTechnologyPageLayout,
                    MiscItemId.MaxActiveTechTeamsDh,
                    MiscItemId.MinActiveTechTeams,
                    MiscItemId.RequiredIcEachTechTeamDh,
                    MiscItemId.Separator,
                    MiscItemId.TechOverviewPanelStyle,
                    MiscItemId.NewCountryRocketryComponent,
                    MiscItemId.NewCountryNuclearPhysicsComponent,
                    MiscItemId.NewCountryNuclearEngineeringComponent,
                    MiscItemId.NewCountrySecretTechs,
                    MiscItemId.MaxTechTeamSkill
                }
            },
            // 貿易
            new[]
            {
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
                    MiscItemId.CancelTradeDealsEffectiveness
                },
                new[]
                {
                    MiscItemId.AutoTradeAiTradeDeals
                }
            },
            // AI
            new[]
            {
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
                    MiscItemId.ForcePuppetsJoinMastersAllianceNeutrality,
                    MiscItemId.NewAiReleaseRules,
                    MiscItemId.AiEventsActionSelectionRules,
                    MiscItemId.ForceStrategicRedeploymentHour
                },
                new[]
                {
                    MiscItemId.MaxRedeploymentDaysAi,
                    MiscItemId.UseQuickAreaCheckGarrisonAi,
                    MiscItemId.AiMastersGetProvincesConquredPuppets,
                    MiscItemId.MinDaysRequiredAiReleaseCountry,
                    MiscItemId.MinDaysRequiredAiAllied,
                    MiscItemId.MinDaysRequiredAiAlliedSupplyBase,
                    MiscItemId.MinRequiredRelationsAlliedClaimed,
                    MiscItemId.Separator,
                    MiscItemId.NewDowRules2
                }
            },
            // MOD
            new[]
            {
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
                    MiscItemId.ExtraRebelBonusMountain
                },
                new[]
                {
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
                    MiscItemId.LoadSpritesModdirOnly,
                    MiscItemId.LoadUnitIconsModdirOnly,
                    MiscItemId.LoadUnitPicturesModdirOnly,
                    MiscItemId.LoadAiFilesModdirOnly,
                    MiscItemId.UseSpeedSetGarrisonStatus,
                    MiscItemId.UseOldSaveGameFormat
                },
                new[]
                {
                    MiscItemId.InGameLossLogging2,
                    MiscItemId.EnableRetirementYearMinisters,
                    MiscItemId.EnableRetirementYearLeaders,
                    MiscItemId.ProductionPanelUiStyle,
                    MiscItemId.UnitPicturesSize,
                    MiscItemId.EnablePicturesNavalBrigades,
                    MiscItemId.BuildingsBuildableOnlyProvinces,
                    MiscItemId.UnitModifiersStatisticsPages
                }
            },
            // マップ
            new[]
            {
                new[]
                {
                    MiscItemId.MapNumber,
                    MiscItemId.TotalProvinces,
                    MiscItemId.DistanceCalculationModel,
                    MiscItemId.MapWidth,
                    MiscItemId.MapHeight
                }
            }
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MiscEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscEditorFormLoad(object sender, EventArgs e)
        {
            // 基本データファイルを読み込む
            Misc.Load();

            // データ読み込み後の処理
            OnMiscLoaded();

            // 先頭ページを初期化する
            InitTabPage(0);
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        /// <param name="index">タブページのインデックスう</param>
        private void UpdateEditableItems(int index)
        {
            MiscGameType gameType = Misc.GetGameType();
            foreach (Control control in miscTabControl.TabPages[index].Controls)
            {
                // タグの設定されていないラベル/共通ボタンをスキップする
                if (control.Tag == null)
                {
                    continue;
                }

                var id = (MiscItemId) control.Tag;
                if (Misc.ItemTable[(int) id, (int) gameType])
                {
                    _labels[(int) id].Enabled = true;
                    _edits[(int) id].Enabled = true;
                    ComboBox comboBox;
                    switch (Misc.ItemTypes[(int) id])
                    {
                        case MiscItemType.None:
                            break;

                        case MiscItemType.Bool:
                            comboBox = _edits[(int) id] as ComboBox;
                            if (comboBox != null)
                            {
                                comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                            }
                            break;

                        case MiscItemType.Enum:
                            comboBox = _edits[(int) id] as ComboBox;
                            if (comboBox != null)
                            {
                                comboBox.SelectedIndex = (int) Misc.GetItem(id) - Misc.IntMinValues[id];
                            }
                            break;

                        default:
                            var textBox = _edits[(int) id] as TextBox;
                            if (textBox != null)
                            {
                                textBox.Text = Misc.GetString(id);
                            }
                            break;
                    }
                }
                else
                {
                    _labels[(int) id].Enabled = false;
                    _edits[(int) id].Enabled = false;
                }
            }
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="index">タブページのインデックスう</param>
        private void UpdateItemColor(int index)
        {
            MiscGameType gameType = Misc.GetGameType();
            foreach (Control control in miscTabControl.TabPages[index].Controls)
            {
                // タグの設定されていないラベル/共通ボタンをスキップする
                if (control.Tag == null)
                {
                    continue;
                }

                var id = (MiscItemId) control.Tag;
                if (Misc.ItemTable[(int) id, (int) gameType])
                {
                    switch (Misc.ItemTypes[(int) id])
                    {
                        case MiscItemType.None:
                        case MiscItemType.Bool:
                        case MiscItemType.Enum:
                            break;

                        default:
                            var textBox = _edits[(int) id] as TextBox;
                            if (textBox != null)
                            {
                                textBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                            }
                            break;
                    }
                }
            }
        }

        #endregion

        #region 終了処理

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2Editor.IsDirty())
            {
                return;
            }

            // 保存するかを問い合わせる
            DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    HoI2Editor.SaveFiles();
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnMiscEditorFormClosed();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.SaveFiles();
                        break;
                }
            }

            HoI2Editor.ReloadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.SaveFiles();
        }

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnMiscLoaded()
        {
            for (int index = 0; index < miscTabControl.TabPages.Count; index++)
            {
                // 編集項目を更新する
                UpdateEditableItems(index);
                // 編集項目の色を更新する
                UpdateItemColor(index);
            }
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnMiscSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            for (int index = 0; index < miscTabControl.TabPages.Count; index++)
            {
                UpdateItemColor(index);
            }
        }

        #endregion

        #region タブページ処理

        /// <summary>
        ///     タブページ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = miscTabControl.SelectedIndex;
            if (miscTabControl.TabPages[index].Controls.Count == 0)
            {
                InitTabPage(index);
            }
        }

        /// <summary>
        ///     タブページを初期化する
        /// </summary>
        /// <param name="index">タブページのインデックス</param>
        private void InitTabPage(int index)
        {
            // 編集項目を作成する
            CreateEditableItems(index);

            // 共通ボタンを作成する
            CreateCommonButtons(index);

            // 編集項目を更新する
            UpdateEditableItems(index);

            // 編集項目の色を更新する
            UpdateItemColor(index);
        }

        /// <summary>
        ///     編集項目を作成する
        /// </summary>
        /// <param name="index">タブページのインデックス</param>
        private void CreateEditableItems(int index)
        {
            var rm = new ResourceManager("HoI2Editor.Properties.Resources", typeof (Resources).Assembly);

            int labelX = 10;
            const int margin = 8;
            foreach (var ids in EditableItems[index])
            {
                int labelY = 13;
                int editX = labelX;
                foreach (MiscItemId id in ids)
                {
                    // セパレータ
                    if (id == MiscItemId.Separator)
                    {
                        labelY += 25;
                        continue;
                    }

                    // ラベルを作成する
                    var label = new Label
                    {
                        AutoSize = true,
                        Text = rm.GetString("MiscLabel" + Misc.ItemNames[(int) id]),
                        Location = new Point(labelX, labelY)
                    };
                    string t = rm.GetString("MiscToolTip" + Misc.ItemNames[(int) id]);
                    if (!string.IsNullOrEmpty(t))
                    {
                        miscToolTip.SetToolTip(label, t);
                    }
                    miscTabControl.TabPages[index].Controls.Add(label);
                    _labels[(int) id] = label;

                    // 編集コントロールの幅のみ求める
                    int x = labelX + label.Width + 8;
                    MiscItemType type = Misc.ItemTypes[(int) id];
                    switch (type)
                    {
                        case MiscItemType.Bool:
                        case MiscItemType.Enum:
                            int maxWidth = 50;
                            for (int i = Misc.IntMinValues[id]; i <= Misc.IntMaxValues[id]; i++)
                            {
                                string s = i.ToString(CultureInfo.InvariantCulture) + ": " +
                                           rm.GetString("MiscEnum" + Misc.ItemNames[(int) id] +
                                                        i.ToString(CultureInfo.InvariantCulture));
                                if (!string.IsNullOrEmpty(s))
                                {
                                    maxWidth = Math.Max(maxWidth,
                                        TextRenderer.MeasureText(s, Font).Width +
                                        SystemInformation.VerticalScrollBarWidth + margin);
                                    maxWidth = 50 + ((maxWidth - 50 + 14)/15)*15;
                                }
                            }
                            x += maxWidth;
                            break;

                        default:
                            // テキストボックスの項目は50px固定
                            x += 50;
                            break;
                    }
                    if (x > editX)
                    {
                        editX = x;
                    }
                    labelY += 25;
                }
                int editY = 10;
                foreach (MiscItemId id in ids)
                {
                    // セパレータ
                    if (id == MiscItemId.Separator)
                    {
                        editY += 25;
                        continue;
                    }
                    // 編集コントロールを作成する
                    MiscItemType type = Misc.ItemTypes[(int) id];
                    switch (type)
                    {
                        case MiscItemType.Bool:
                        case MiscItemType.Enum:
                            var comboBox = new ComboBox
                            {
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                DrawMode = DrawMode.OwnerDrawFixed,
                                Tag = id
                            };
                            // コンボボックスの選択項目を登録し、最大幅を求める
                            int maxWidth = 50;
                            for (int i = Misc.IntMinValues[id]; i <= Misc.IntMaxValues[id]; i++)
                            {
                                string s = i.ToString(CultureInfo.InvariantCulture) + ": " +
                                           rm.GetString("MiscEnum" + Misc.ItemNames[(int) id] +
                                                        i.ToString(CultureInfo.InvariantCulture));
                                if (!string.IsNullOrEmpty(s))
                                {
                                    comboBox.Items.Add(s);
                                    maxWidth = Math.Max(maxWidth,
                                        TextRenderer.MeasureText(s, Font).Width +
                                        SystemInformation.VerticalScrollBarWidth + margin);
                                    maxWidth = 50 + ((maxWidth - 50 + 14)/15)*15;
                                }
                            }
                            comboBox.Size = new Size(maxWidth, 20);
                            comboBox.Location = new Point(editX - maxWidth, editY);
                            comboBox.DrawItem += OnItemComboBoxDrawItem;
                            comboBox.SelectedIndexChanged += OnItemComboBoxSelectedIndexChanged;
                            miscTabControl.TabPages[index].Controls.Add(comboBox);
                            _edits[(int) id] = comboBox;
                            break;

                        default:
                            var textBox = new TextBox
                            {
                                Size = new Size(50, 19),
                                Location = new Point(editX - 50, editY),
                                TextAlign = HorizontalAlignment.Right,
                                Tag = id
                            };
                            textBox.Validated += OnItemTextBoxValidated;
                            miscTabControl.TabPages[index].Controls.Add(textBox);
                            _edits[(int) id] = textBox;
                            break;
                    }
                    editY += 25;
                }
                // 次の列との間を10px空ける
                labelX = editX + 10;
            }
        }

        /// <summary>
        ///     タブページ間共通ボタンを作成する
        /// </summary>
        /// <param name="index">タブページのインデックス</param>
        private void CreateCommonButtons(int index)
        {
            // 再読み込み
            var button = new Button
            {
                Text = Resources.Reload,
                Location = new Point(731, 625),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };
            button.Click += OnReloadButtonClick;
            miscTabControl.TabPages[index].Controls.Add(button);

            // 保存
            button = new Button
            {
                Text = Resources.Save,
                Location = new Point(812, 625),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };
            button.Click += OnSaveButtonClick;
            miscTabControl.TabPages[index].Controls.Add(button);

            // 閉じる
            button = new Button
            {
                Text = Resources.Close,
                Location = new Point(893, 625),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };
            button.Click += OnCloseButtonClick;
            miscTabControl.TabPages[index].Controls.Add(button);
        }

        #endregion

        #region 編集項目操作

        /// <summary>
        ///     編集項目テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTextBoxValidated(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var id = (MiscItemId) textBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            double d = 0;
            int i = 0;

            // 変更後の文字列を数値に変換できなければ値を戻す
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    if (!int.TryParse(textBox.Text, out i))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    if (!double.TryParse(textBox.Text, out d))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.None:
                case MiscItemType.Bool:
                case MiscItemType.Enum:
                    break;
            }

            // 設定範囲外の値ならば戻す
            switch (type)
            {
                case MiscItemType.PosInt:
                    if (i <= 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegInt:
                case MiscItemType.NonNegInt1:
                    if (i < 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonPosInt:
                    if (i > 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegIntMinusOne:
                    if (i < 0 && i != -1)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedInt:
                    if (i < Misc.IntMinValues[id] || i > Misc.IntMaxValues[id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedPosInt:
                    if (i < Misc.IntMinValues[id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusOne:
                    if ((i < Misc.IntMinValues[id] || i > Misc.IntMaxValues[id]) && i != -1)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusThree:
                    if ((i < Misc.IntMinValues[id] || i > Misc.IntMaxValues[id]) && i != -1 && i != -2 &&
                        i != -3)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.PosDbl:
                    if (d <= 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                    if (d < 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonPosDbl:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                    if (d > 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                    if (d < 0 && Math.Abs(d - (-1)) > 0.00005)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDbl0:
                    if (d < Misc.DblMinValues[id] || d > Misc.DblMaxValues[id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                    if ((d < Misc.DblMinValues[id] || d > Misc.DblMaxValues[id]) &&
                        Math.Abs(d - (-1)) > 0.00005)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;
            }

            // 値に変化がなければ何もしない
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    if (i == (int) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    if (Math.Abs(d - (double) Misc.GetItem(id)) <= 0.00005)
                    {
                        return;
                    }
                    break;
            }

            // 値を更新する
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    Misc.SetItem(id, i);
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    Misc.SetItem(id, d);
                    break;
            }

            // 編集済みフラグを設定する
            Misc.SetDirty(id);
            Misc.SetDirty();

            // 文字色を変更する
            textBox.ForeColor = Color.Red;
        }

        /// <summary>
        ///     編集項目コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            var id = (MiscItemId) comboBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int index = 0;
            switch (type)
            {
                case MiscItemType.Bool:
                    index = (bool) Misc.GetItem(id) ? 1 : 0;
                    break;

                case MiscItemType.Enum:
                    index = (int) Misc.GetItem(id);
                    break;
            }
            if ((e.Index + Misc.IntMinValues[id] == index) && Misc.IsDirty(id))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = comboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     編集項目コンボボックスの選択インデックス変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            var id = (MiscItemId) comboBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            if (comboBox.SelectedIndex == -1)
            {
                return;
            }

            bool b = false;
            int i = 0;

            // 値に変化がなければ何もしない
            switch (type)
            {
                case MiscItemType.Bool:
                    b = (comboBox.SelectedIndex == 1);
                    if (b == (bool) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;

                case MiscItemType.Enum:
                    i = comboBox.SelectedIndex + Misc.IntMinValues[id];
                    if (i == (int) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;
            }

            // 値を更新する
            switch (type)
            {
                case MiscItemType.Bool:
                    Misc.SetItem(id, b);
                    break;

                case MiscItemType.Enum:
                    Misc.SetItem(id, i);
                    break;
            }

            // 編集済みフラグを設定する
            Misc.SetDirty(id);
            Misc.SetDirty();

            // 項目色を変更するために描画更新する
            comboBox.Refresh();
        }

        #endregion
    }
}