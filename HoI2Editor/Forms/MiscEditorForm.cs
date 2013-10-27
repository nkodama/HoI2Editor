using System;
using System.Drawing;
using System.Globalization;
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
            // 編集項目のタグを初期化する
            InitItemTags();

            // miscファイルを読み込む
            LoadFiles();

            // フォームを前面に表示する
            Activate();
        }

        /// <summary>
        ///     編集項目のタグを初期化する
        /// </summary>
        private void InitItemTags()
        {
            InitEconomy1ItemTags();
            InitEconomy2ItemTags();
            InitEconomy3ItemTags();
            InitIntelligenceItemTags();
            InitDiplomacyItemTags();
            InitCombat1ItemTags();
            InitCombat2ItemTags();
            InitCombat3ItemTags();
            InitCombat4ItemTags();
            InitCombat5ItemTags();
            InitMission1ItemTags();
            InitMission2ItemTags();
            InitCountryItemTags();
            InitResearchItemTags();
            InitTradeItemTags();
            InitAiItemTags();
            InitModItemTags();
            InitMapItemTags();
        }

        /// <summary>
        ///     経済1タブの編集項目のタグを初期化する
        /// </summary>
        private void InitEconomy1ItemTags()
        {
            icToTcRatioTextBox.Tag = MiscItemId.IcToTcRatio;
            icToSuppliesRatioTextBox.Tag = MiscItemId.IcToSuppliesRatio;
            icToConsumerGoodsRatioTextBox.Tag = MiscItemId.IcToConsumerGoodsRatio;
            icToMoneyRatioTextBox.Tag = MiscItemId.IcToMoneyRatio;
            maxGearingBonusTextBox.Tag = MiscItemId.MaxGearingBonus;
            gearingBonusIncrementTextBox.Tag = MiscItemId.GearingBonusIncrement;
            icMultiplierNonNationalTextBox.Tag = MiscItemId.IcMultiplierNonNational;
            icMultiplierNonOwnedTextBox.Tag = MiscItemId.IcMultiplierNonOwned;
            tcLoadUndeployedDivisionTextBox.Tag = MiscItemId.TcLoadUndeployedDivision;
            tcLoadOccupiedTextBox.Tag = MiscItemId.TcLoadOccupied;
            tcLoadMultiplierLandTextBox.Tag = MiscItemId.TcLoadMultiplierLand;
            tcLoadMultiplierAirTextBox.Tag = MiscItemId.TcLoadMultiplierAir;
            tcLoadMultiplierNavalTextBox.Tag = MiscItemId.TcLoadMultiplierNaval;
            tcLoadPartisanTextBox.Tag = MiscItemId.TcLoadPartisan;
            tcLoadFactorOffensiveTextBox.Tag = MiscItemId.TcLoadFactorOffensive;
            tcLoadProvinceDevelopmentTextBox.Tag = MiscItemId.TcLoadProvinceDevelopment;
            tcLoadBaseTextBox.Tag = MiscItemId.TcLoadBase;
            manpowerMultiplierNationalTextBox.Tag = MiscItemId.ManpowerMultiplierNational;
            manpowerMultiplierNonNationalTextBox.Tag = MiscItemId.ManpowerMultiplierNonNational;
            manpowerMultiplierColonyTextBox.Tag = MiscItemId.ManpowerMultiplierColony;
            requirementAffectSliderTextBox.Tag = MiscItemId.RequirementAffectSlider;
            trickleBackFactorManpowerTextBox.Tag = MiscItemId.TrickleBackFactorManpower;
            reinforceManpowerTextBox.Tag = MiscItemId.ReinforceManpower;
            reinforceCostTextBox.Tag = MiscItemId.ReinforceCost;
            reinforceTimeTextBox.Tag = MiscItemId.ReinforceTime;
            upgradeCostTextBox.Tag = MiscItemId.UpgradeCost;
            upgradeTimeTextBox.Tag = MiscItemId.UpgradeTime;
            nationalismStartingValueTextBox.Tag = MiscItemId.NationalismStartingValue;
            monthlyNationalismReductionTextBox.Tag = MiscItemId.MonthlyNationalismReduction;
            sendDivisionDaysTextBox.Tag = MiscItemId.SendDivisionDays;
            tcLoadUndeployedBrigadeTextBox.Tag = MiscItemId.TcLoadUndeployedBrigade;
            canUnitSendNonAlliedComboBox.Tag = MiscItemId.CanUnitSendNonAllied;
            spyMissionDaysTextBox.Tag = MiscItemId.SpyMissionDays;
            increateIntelligenceLevelDaysTextBox.Tag = MiscItemId.IncreateIntelligenceLevelDays;
            chanceDetectSpyMissionTextBox.Tag = MiscItemId.ChanceDetectSpyMission;
            relationshipsHitDetectedMissionsTextBox.Tag = MiscItemId.RelationshipsHitDetectedMissions;
            showThirdCountrySpyReportsComboBox.Tag = MiscItemId.ShowThirdCountrySpyReports;
            distanceModifierNeighboursTextBox.Tag = MiscItemId.DistanceModifierNeighbours;
            spyInformationAccuracyModifierTextBox.Tag = MiscItemId.SpyInformationAccuracyModifier;
            aiPeacetimeSpyMissionsComboBox.Tag = MiscItemId.AiPeacetimeSpyMissions;
            maxIcCostModifierTextBox.Tag = MiscItemId.MaxIcCostModifier;
            aiSpyMissionsCostModifierTextBox.Tag = MiscItemId.AiSpyMissionsCostModifier;
            aiDiplomacyCostModifierTextBox.Tag = MiscItemId.AiDiplomacyCostModifier;
            aiInfluenceModifierTextBox.Tag = MiscItemId.AiInfluenceModifier;
            nationalismPerManpowerAoDTextBox.Tag = MiscItemId.NationalismPerManpowerAoD;
            coreProvinceEfficiencyRiseTimeTextBox.Tag = MiscItemId.CoreProvinceEfficiencyRiseTime;
            restockSpeedLandTextBox.Tag = MiscItemId.RestockSpeedLand;
            restockSpeedAirTextBox.Tag = MiscItemId.RestockSpeedAir;
            restockSpeedNavalTextBox.Tag = MiscItemId.RestockSpeedNaval;
            spyCoupDissentModifierTextBox.Tag = MiscItemId.SpyCoupDissentModifier;
            convoyDutyConversionTextBox.Tag = MiscItemId.ConvoyDutyConversion;
            escortDutyConversionTextBox.Tag = MiscItemId.EscortDutyConversion;
            tpMaxAttachTextBox.Tag = MiscItemId.TpMaxAttach;
            ssMaxAttachTextBox.Tag = MiscItemId.SsMaxAttach;
            ssnMaxAttachTextBox.Tag = MiscItemId.SsnMaxAttach;
            ddMaxAttachTextBox.Tag = MiscItemId.DdMaxAttach;
            clMaxAttachTextBox.Tag = MiscItemId.ClMaxAttach;
            caMaxAttachTextBox.Tag = MiscItemId.CaMaxAttach;
            bcMaxAttachTextBox.Tag = MiscItemId.BcMaxAttach;
            bbMaxAttachTextBox.Tag = MiscItemId.BbMaxAttach;
            cvlMaxAttachTextBox.Tag = MiscItemId.CvlMaxAttach;
            cvMaxAttachTextBox.Tag = MiscItemId.CvMaxAttach;
            canChangeIdeasComboBox.Tag = MiscItemId.CanChangeIdeas;
        }

        /// <summary>
        ///     経済2タブの編集項目のタグを初期化する
        /// </summary>
        private void InitEconomy2ItemTags()
        {
            dissentChangeSpeedTextBox.Tag = MiscItemId.DissentChangeSpeed;
            gearingResourceIncrementTextBox.Tag = MiscItemId.GearingResourceIncrement;
            gearingLossNoIcTextBox.Tag = MiscItemId.GearingLossNoIc;
            costRepairBuildingsTextBox.Tag = MiscItemId.CostRepairBuildings;
            timeRepairBuildingTextBox.Tag = MiscItemId.TimeRepairBuilding;
            provinceEfficiencyRiseTimeTextBox.Tag = MiscItemId.ProvinceEfficiencyRiseTime;
            lineUpkeepTextBox.Tag = MiscItemId.LineUpkeep;
            lineStartupTimeTextBox.Tag = MiscItemId.LineStartupTime;
            lineUpgradeTimeTextBox.Tag = MiscItemId.LineUpgradeTime;
            retoolingCostTextBox.Tag = MiscItemId.RetoolingCost;
            retoolingResourceTextBox.Tag = MiscItemId.RetoolingResource;
            dailyAgingManpowerTextBox.Tag = MiscItemId.DailyAgingManpower;
            supplyConvoyHuntTextBox.Tag = MiscItemId.SupplyConvoyHunt;
            supplyNavalStaticAoDTextBox.Tag = MiscItemId.SupplyNavalStaticAoD;
            supplyNavalMovingTextBox.Tag = MiscItemId.SupplyNavalMoving;
            supplyNavalBattleAoDTextBox.Tag = MiscItemId.SupplyNavalBattleAoD;
            supplyAirStaticAoDTextBox.Tag = MiscItemId.SupplyAirStaticAoD;
            supplyAirMovingTextBox.Tag = MiscItemId.SupplyAirMoving;
            supplyAirBattleAoDTextBox.Tag = MiscItemId.SupplyAirBattleAoD;
            supplyAirBombingTextBox.Tag = MiscItemId.SupplyAirBombing;
            supplyLandStaticAoDTextBox.Tag = MiscItemId.SupplyLandStaticAoD;
            supplyLandMovingTextBox.Tag = MiscItemId.SupplyLandMoving;
            supplyLandBattleAoDTextBox.Tag = MiscItemId.SupplyLandBattleAoD;
            supplyLandBombingTextBox.Tag = MiscItemId.SupplyLandBombing;
            supplyStockLandTextBox.Tag = MiscItemId.SupplyStockLand;
            supplyStockAirTextBox.Tag = MiscItemId.SupplyStockAir;
            supplyStockNavalTextBox.Tag = MiscItemId.SupplyStockNaval;
            syntheticOilConversionMultiplierTextBox.Tag = MiscItemId.SyntheticOilConversionMultiplier;
            syntheticRaresConversionMultiplierTextBox.Tag = MiscItemId.SyntheticRaresConversionMultiplier;
            militarySalaryTextBox.Tag = MiscItemId.MilitarySalary;
            maxIntelligenceExpenditureTextBox.Tag = MiscItemId.MaxIntelligenceExpenditure;
            maxResearchExpenditureTextBox.Tag = MiscItemId.MaxResearchExpenditure;
            militarySalaryAttrictionModifierTextBox.Tag = MiscItemId.MilitarySalaryAttrictionModifier;
            militarySalaryDissentModifierTextBox.Tag = MiscItemId.MilitarySalaryDissentModifier;
            nuclearSiteUpkeepCostTextBox.Tag = MiscItemId.NuclearSiteUpkeepCost;
            nuclearPowerUpkeepCostTextBox.Tag = MiscItemId.NuclearPowerUpkeepCost;
            syntheticOilSiteUpkeepCostTextBox.Tag = MiscItemId.SyntheticOilSiteUpkeepCost;
            syntheticRaresSiteUpkeepCostTextBox.Tag = MiscItemId.SyntheticRaresSiteUpkeepCost;
            durationDetectionTextBox.Tag = MiscItemId.DurationDetection;
            convoyProvinceHostileTimeTextBox.Tag = MiscItemId.ConvoyProvinceHostileTime;
            convoyProvinceBlockedTimeTextBox.Tag = MiscItemId.ConvoyProvinceBlockedTime;
            autoTradeConvoyTextBox.Tag = MiscItemId.AutoTradeConvoy;
            spyUpkeepCostTextBox.Tag = MiscItemId.SpyUpkeepCost;
            spyDetectionChanceTextBox.Tag = MiscItemId.SpyDetectionChance;
            infraEfficiencyModifierTextBox.Tag = MiscItemId.InfraEfficiencyModifier;
            manpowerToConsumerGoodsTextBox.Tag = MiscItemId.ManpowerToConsumerGoods;
            timeBetweenSliderChangesAoDTextBox.Tag = MiscItemId.TimeBetweenSliderChangesAoD;
            minimalPlacementIcTextBox.Tag = MiscItemId.MinimalPlacementIc;
            nuclearPowerTextBox.Tag = MiscItemId.NuclearPower;
            freeInfraRepairTextBox.Tag = MiscItemId.FreeInfraRepair;
            maxSliderDissentTextBox.Tag = MiscItemId.MaxSliderDissent;
            minSliderDissentTextBox.Tag = MiscItemId.MinSliderDissent;
            maxDissentSliderMoveTextBox.Tag = MiscItemId.MaxDissentSliderMove;
            icConcentrationBonusTextBox.Tag = MiscItemId.IcConcentrationBonus;
            transportConversionTextBox.Tag = MiscItemId.TransportConversion;
            ministerChangeDelayTextBox.Tag = MiscItemId.MinisterChangeDelay;
            ministerChangeEventDelayTextBox.Tag = MiscItemId.MinisterChangeEventDelay;
            ideaChangeDelayTextBox.Tag = MiscItemId.IdeaChangeDelay;
            ideaChangeEventDelayTextBox.Tag = MiscItemId.IdeaChangeEventDelay;
            leaderChangeDelayTextBox.Tag = MiscItemId.LeaderChangeDelay;
            changeIdeaDissentTextBox.Tag = MiscItemId.ChangeIdeaDissent;
            changeMinisterDissentTextBox.Tag = MiscItemId.ChangeMinisterDissent;
            minDissentRevoltTextBox.Tag = MiscItemId.MinDissentRevolt;
            dissentRevoltMultiplierTextBox.Tag = MiscItemId.DissentRevoltMultiplier;
        }

        /// <summary>
        ///     経済3タブの編集項目のタグを初期化する
        /// </summary>
        private void InitEconomy3ItemTags()
        {
            minAvailableIcTextBox.Tag = MiscItemId.MinAvailableIc;
            minFinalIcTextBox.Tag = MiscItemId.MinFinalIc;
            dissentReductionTextBox.Tag = MiscItemId.DissentReduction;
            icMultiplierPuppetTextBox.Tag = MiscItemId.IcMultiplierPuppet;
            resourceMultiplierNonNationalTextBox.Tag = MiscItemId.ResourceMultiplierNonNational;
            resourceMultiplierNonOwnedTextBox.Tag = MiscItemId.ResourceMultiplierNonOwned;
            resourceMultiplierNonNationalAiTextBox.Tag = MiscItemId.ResourceMultiplierNonNationalAi;
            resourceMultiplierPuppetTextBox.Tag = MiscItemId.ResourceMultiplierPuppet;
            manpowerMultiplierPuppetTextBox.Tag = MiscItemId.ManpowerMultiplierPuppet;
            manpowerMultiplierWartimeOverseaTextBox.Tag = MiscItemId.ManpowerMultiplierWartimeOversea;
            manpowerMultiplierPeacetimeTextBox.Tag = MiscItemId.ManpowerMultiplierPeacetime;
            manpowerMultiplierWartimeTextBox.Tag = MiscItemId.ManpowerMultiplierWartime;
            dailyRetiredManpowerTextBox.Tag = MiscItemId.DailyRetiredManpower;
            reinforceToUpdateModifierTextBox.Tag = MiscItemId.ReinforceToUpdateModifier;
            nationalismPerManpowerDhTextBox.Tag = MiscItemId.NationalismPerManpowerDh;
            maxNationalismTextBox.Tag = MiscItemId.MaxNationalism;
            maxRevoltRiskTextBox.Tag = MiscItemId.MaxRevoltRisk;
            canUnitSendNonAlliedDhComboBox.Tag = MiscItemId.CanUnitSendNonAlliedDh;
            bluePrintsCanSoldNonAlliedComboBox.Tag = MiscItemId.BluePrintsCanSoldNonAllied;
            provinceCanSoldNonAlliedComboBox.Tag = MiscItemId.ProvinceCanSoldNonAllied;
            transferAlliedCoreProvincesComboBox.Tag = MiscItemId.TransferAlliedCoreProvinces;
            provinceBuildingsRepairModifierTextBox.Tag = MiscItemId.ProvinceBuildingsRepairModifier;
            provinceResourceRepairModifierTextBox.Tag = MiscItemId.ProvinceResourceRepairModifier;
            stockpileLimitMultiplierResourceTextBox.Tag = MiscItemId.StockpileLimitMultiplierResource;
            stockpileLimitMultiplierSuppliesOilTextBox.Tag = MiscItemId.StockpileLimitMultiplierSuppliesOil;
            overStockpileLimitDailyLossTextBox.Tag = MiscItemId.OverStockpileLimitDailyLoss;
            maxResourceDepotSizeTextBox.Tag = MiscItemId.MaxResourceDepotSize;
            maxSuppliesOilDepotSizeTextBox.Tag = MiscItemId.MaxSuppliesOilDepotSize;
            desiredStockPilesSuppliesOilTextBox.Tag = MiscItemId.DesiredStockPilesSuppliesOil;
            maxManpowerTextBox.Tag = MiscItemId.MaxManpower;
            convoyTransportsCapacityTextBox.Tag = MiscItemId.ConvoyTransportsCapacity;
            suppyLandStaticDhTextBox.Tag = MiscItemId.SuppyLandStaticDh;
            supplyLandBattleDhTextBox.Tag = MiscItemId.SupplyLandBattleDh;
            fuelLandStaticTextBox.Tag = MiscItemId.FuelLandStatic;
            fuelLandBattleTextBox.Tag = MiscItemId.FuelLandBattle;
            supplyAirStaticDhTextBox.Tag = MiscItemId.SupplyAirStaticDh;
            supplyAirBattleDhTextBox.Tag = MiscItemId.SupplyAirBattleDh;
            fuelAirNavalStaticTextBox.Tag = MiscItemId.FuelAirNavalStatic;
            fuelAirBattleTextBox.Tag = MiscItemId.FuelAirBattle;
            supplyNavalStaticDhTextBox.Tag = MiscItemId.SupplyNavalStaticDh;
            supplyNavalBattleDhTextBox.Tag = MiscItemId.SupplyNavalBattleDh;
            fuelNavalNotMovingTextBox.Tag = MiscItemId.FuelNavalNotMoving;
            fuelNavalBattleTextBox.Tag = MiscItemId.FuelNavalBattle;
            tpTransportsConversionRatioTextBox.Tag = MiscItemId.TpTransportsConversionRatio;
            ddEscortsConversionRatioTextBox.Tag = MiscItemId.DdEscortsConversionRatio;
            clEscortsConversionRatioTextBox.Tag = MiscItemId.ClEscortsConversionRatio;
            cvlEscortsConversionRatioTextBox.Tag = MiscItemId.CvlEscortsConversionRatio;
            productionLineEditComboBox.Tag = MiscItemId.ProductionLineEdit;
            gearingBonusLossUpgradeUnitTextBox.Tag = MiscItemId.GearingBonusLossUpgradeUnit;
            gearingBonusLossUpgradeBrigadeTextBox.Tag = MiscItemId.GearingBonusLossUpgradeBrigade;
            dissentNukesTextBox.Tag = MiscItemId.DissentNukes;
            maxDailyDissentTextBox.Tag = MiscItemId.MaxDailyDissent;
            nukesProductionModifierTextBox.Tag = MiscItemId.NukesProductionModifier;
            convoySystemOptionsAlliedComboBox.Tag = MiscItemId.ConvoySystemOptionsAllied;
            resourceConvoysBackUnneededTextBox.Tag = MiscItemId.ResourceConvoysBackUnneeded;
        }

        /// <summary>
        ///     諜報タブの編集項目のタグを初期化する
        /// </summary>
        private void InitIntelligenceItemTags()
        {
            spyMissionDaysDhTextBox.Tag = MiscItemId.SpyMissionDaysDh;
            increateIntelligenceLevelDaysDhTextBox.Tag = MiscItemId.IncreateIntelligenceLevelDaysDh;
            chanceDetectSpyMissionDhTextBox.Tag = MiscItemId.ChanceDetectSpyMissionDh;
            relationshipsHitDetectedMissionsDhTextBox.Tag = MiscItemId.RelationshipsHitDetectedMissionsDh;
            distanceModifierTextBox.Tag = MiscItemId.DistanceModifier;
            distanceModifierNeighboursDhTextBox.Tag = MiscItemId.DistanceModifierNeighboursDh;
            spyLevelBonusDistanceModifierTextBox.Tag = MiscItemId.SpyLevelBonusDistanceModifier;
            spyLevelBonusDistanceModifierAboveTenTextBox.Tag = MiscItemId.SpyLevelBonusDistanceModifierAboveTen;
            spyInformationAccuracyModifierDhTextBox.Tag = MiscItemId.SpyInformationAccuracyModifierDh;
            icModifierCostTextBox.Tag = MiscItemId.IcModifierCost;
            minIcCostModifierTextBox.Tag = MiscItemId.MinIcCostModifier;
            maxIcCostModifierDhTextBox.Tag = MiscItemId.MaxIcCostModifierDh;
            extraMaintenanceCostAboveTenTextBox.Tag = MiscItemId.ExtraMaintenanceCostAboveTen;
            extraCostIncreasingAboveTenTextBox.Tag = MiscItemId.ExtraCostIncreasingAboveTen;
            showThirdCountrySpyReportsDhComboBox.Tag = MiscItemId.ShowThirdCountrySpyReportsDh;
            spiesMoneyModifierTextBox.Tag = MiscItemId.SpiesMoneyModifier;
        }

        /// <summary>
        ///     外交タブの編集項目のタグを初期化する
        /// </summary>
        private void InitDiplomacyItemTags()
        {
            daysBetweenDiplomaticMissionsTextBox.Tag = MiscItemId.DaysBetweenDiplomaticMissions;
            timeBetweenSliderChangesDhTextBox.Tag = MiscItemId.TimeBetweenSliderChangesDh;
            requirementAffectSliderDhTextBox.Tag = MiscItemId.RequirementAffectSliderDh;
            useMinisterPersonalityReplacingComboBox.Tag = MiscItemId.UseMinisterPersonalityReplacing;
            relationshipHitCancelTradeTextBox.Tag = MiscItemId.RelationshipHitCancelTrade;
            relationshipHitCancelPermanentTradeTextBox.Tag = MiscItemId.RelationshipHitCancelPermanentTrade;
            puppetsJoinMastersAllianceComboBox.Tag = MiscItemId.PuppetsJoinMastersAlliance;
            mastersBecomePuppetsPuppetsComboBox.Tag = MiscItemId.MastersBecomePuppetsPuppets;
            allowManualClaimsChangeComboBox.Tag = MiscItemId.AllowManualClaimsChange;
            belligerenceClaimedProvinceTextBox.Tag = MiscItemId.BelligerenceClaimedProvince;
            belligerenceClaimsRemovalTextBox.Tag = MiscItemId.BelligerenceClaimsRemoval;
            joinAutomaticallyAllesAxisComboBox.Tag = MiscItemId.JoinAutomaticallyAllesAxis;
            allowChangeHosHogComboBox.Tag = MiscItemId.AllowChangeHosHog;
            changeTagCoupComboBox.Tag = MiscItemId.ChangeTagCoup;
            filterReleaseCountriesComboBox.Tag = MiscItemId.FilterReleaseCountries;
        }

        /// <summary>
        ///     戦闘1タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCombat1ItemTags()
        {
            landXpGainFactorTextBox.Tag = MiscItemId.LandXpGainFactor;
            navalXpGainFactorTextBox.Tag = MiscItemId.NavalXpGainFactor;
            airXpGainFactorTextBox.Tag = MiscItemId.AirXpGainFactor;
            divisionXpGainFactorTextBox.Tag = MiscItemId.DivisionXpGainFactor;
            leaderXpGainFactorTextBox.Tag = MiscItemId.LeaderXpGainFactor;
            attritionSeverityModifierTextBox.Tag = MiscItemId.AttritionSeverityModifier;
            baseProximityTextBox.Tag = MiscItemId.BaseProximity;
            shoreBombardmentModifierTextBox.Tag = MiscItemId.ShoreBombardmentModifier;
            invasionModifierTextBox.Tag = MiscItemId.InvasionModifier;
            multipleCombatModifierTextBox.Tag = MiscItemId.MultipleCombatModifier;
            offensiveCombinedArmsBonusTextBox.Tag = MiscItemId.OffensiveCombinedArmsBonus;
            defensiveCombinedArmsBonusTextBox.Tag = MiscItemId.DefensiveCombinedArmsBonus;
            surpriseModifierTextBox.Tag = MiscItemId.SurpriseModifier;
            landCommandLimitModifierTextBox.Tag = MiscItemId.LandCommandLimitModifier;
            airCommandLimitModifierTextBox.Tag = MiscItemId.AirCommandLimitModifier;
            navalCommandLimitModifierTextBox.Tag = MiscItemId.NavalCommandLimitModifier;
            envelopmentModifierTextBox.Tag = MiscItemId.EnvelopmentModifier;
            encircledModifierTextBox.Tag = MiscItemId.EncircledModifier;
            landFortMultiplierTextBox.Tag = MiscItemId.LandFortMultiplier;
            coastalFortMultiplierTextBox.Tag = MiscItemId.CoastalFortMultiplier;
            dissentMultiplierTextBox.Tag = MiscItemId.DissentMultiplier;
            supplyProblemsModifierTextBox.Tag = MiscItemId.SupplyProblemsModifier;
            raderStationMultiplierTextBox.Tag = MiscItemId.RaderStationMultiplier;
            interceptorBomberModifierTextBox.Tag = MiscItemId.InterceptorBomberModifier;
            airOverstackingModifierTextBox.Tag = MiscItemId.AirOverstackingModifier;
            navalOverstackingModifierTextBox.Tag = MiscItemId.NavalOverstackingModifier;
            landLeaderCommandLimitRank0TextBox.Tag = MiscItemId.LandLeaderCommandLimitRank0;
            landLeaderCommandLimitRank1TextBox.Tag = MiscItemId.LandLeaderCommandLimitRank1;
            landLeaderCommandLimitRank2TextBox.Tag = MiscItemId.LandLeaderCommandLimitRank2;
            landLeaderCommandLimitRank3TextBox.Tag = MiscItemId.LandLeaderCommandLimitRank3;
            airLeaderCommandLimitRank0TextBox.Tag = MiscItemId.AirLeaderCommandLimitRank0;
            airLeaderCommandLimitRank1TextBox.Tag = MiscItemId.AirLeaderCommandLimitRank1;
            airLeaderCommandLimitRank2TextBox.Tag = MiscItemId.AirLeaderCommandLimitRank2;
            airLeaderCommandLimitRank3TextBox.Tag = MiscItemId.AirLeaderCommandLimitRank3;
            navalLeaderCommandLimitRank0TextBox.Tag = MiscItemId.NavalLeaderCommandLimitRank0;
            navalLeaderCommandLimitRank1TextBox.Tag = MiscItemId.NavalLeaderCommandLimitRank1;
            navalLeaderCommandLimitRank2TextBox.Tag = MiscItemId.NavalLeaderCommandLimitRank2;
            navalLeaderCommandLimitRank3TextBox.Tag = MiscItemId.NavalLeaderCommandLimitRank3;
            hqCommandLimitFactorTextBox.Tag = MiscItemId.HqCommandLimitFactor;
            convoyProtectionFactorTextBox.Tag = MiscItemId.ConvoyProtectionFactor;
            delayAfterCombatEndsTextBox.Tag = MiscItemId.DelayAfterCombatEnds;
            maximumSizesAirStacksTextBox.Tag = MiscItemId.MaximumSizesAirStacks;
            effectExperienceCombatTextBox.Tag = MiscItemId.EffectExperienceCombat;
            damageNavalBasesBombingTextBox.Tag = MiscItemId.DamageNavalBasesBombing;
            damageAirBaseBombingTextBox.Tag = MiscItemId.DamageAirBaseBombing;
            damageAaBombingTextBox.Tag = MiscItemId.DamageAaBombing;
            damageRocketBombingTextBox.Tag = MiscItemId.DamageRocketBombing;
            damageNukeBombingTextBox.Tag = MiscItemId.DamageNukeBombing;
            damageRadarBombingTextBox.Tag = MiscItemId.DamageRadarBombing;
            damageInfraBombingTextBox.Tag = MiscItemId.DamageInfraBombing;
            damageIcBombingTextBox.Tag = MiscItemId.DamageIcBombing;
            damageResourcesBombingTextBox.Tag = MiscItemId.DamageResourcesBombing;
            howEffectiveGroundDefTextBox.Tag = MiscItemId.HowEffectiveGroundDef;
            chanceAvoidDefencesLeftTextBox.Tag = MiscItemId.ChanceAvoidDefencesLeft;
            chanceAvoidNoDefencesTextBox.Tag = MiscItemId.ChanceAvoidNoDefences;
            chanceGetTerrainTraitTextBox.Tag = MiscItemId.ChanceGetTerrainTrait;
            chanceGetEventTraitTextBox.Tag = MiscItemId.ChanceGetEventTrait;
            bonusTerrainTraitTextBox.Tag = MiscItemId.BonusTerrainTrait;
            bonusEventTraitTextBox.Tag = MiscItemId.BonusEventTrait;
            chanceLeaderDyingTextBox.Tag = MiscItemId.ChanceLeaderDying;
            airOrgDamageTextBox.Tag = MiscItemId.AirOrgDamage;
            airStrDamageOrgTextBox.Tag = MiscItemId.AirStrDamageOrg;
            airStrDamageTextBox.Tag = MiscItemId.AirStrDamage;
            subsOrgDamageTextBox.Tag = MiscItemId.SubsOrgDamage;
            subsStrDamageTextBox.Tag = MiscItemId.SubsStrDamage;
            subStacksDetectionModifierTextBox.Tag = MiscItemId.SubStacksDetectionModifier;
        }

        /// <summary>
        ///     戦闘2タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCombat2ItemTags()
        {
            noSupplyAttritionSeverityTextBox.Tag = MiscItemId.NoSupplyAttritionSeverity;
            noSupplyMinimunAttritionTextBox.Tag = MiscItemId.NoSupplyMinimunAttrition;
            raderStationAaMultiplierTextBox.Tag = MiscItemId.RaderStationAaMultiplier;
            airOverstackingModifierAoDTextBox.Tag = MiscItemId.AirOverstackingModifierAoD;
            landDelayBeforeOrdersTextBox.Tag = MiscItemId.LandDelayBeforeOrders;
            navalDelayBeforeOrdersTextBox.Tag = MiscItemId.NavalDelayBeforeOrders;
            airDelayBeforeOrdersTextBox.Tag = MiscItemId.AirDelayBeforeOrders;
            damageSyntheticOilBombingTextBox.Tag = MiscItemId.DamageSyntheticOilBombing;
            airOrgDamageLandAoDTextBox.Tag = MiscItemId.AirOrgDamageLandAoD;
            airStrDamageLandAoDTextBox.Tag = MiscItemId.AirStrDamageLandAoD;
            landDamageArtilleryBombardmentTextBox.Tag = MiscItemId.LandDamageArtilleryBombardment;
            infraDamageArtilleryBombardmentTextBox.Tag = MiscItemId.InfraDamageArtilleryBombardment;
            icDamageArtilleryBombardmentTextBox.Tag = MiscItemId.IcDamageArtilleryBombardment;
            resourcesDamageArtilleryBombardmentTextBox.Tag = MiscItemId.ResourcesDamageArtilleryBombardment;
            penaltyArtilleryBombardmentTextBox.Tag = MiscItemId.PenaltyArtilleryBombardment;
            artilleryStrDamageTextBox.Tag = MiscItemId.ArtilleryStrDamage;
            artilleryOrgDamageTextBox.Tag = MiscItemId.ArtilleryOrgDamage;
            landStrDamageLandAoDTextBox.Tag = MiscItemId.LandStrDamageLandAoD;
            landOrgDamageLandTextBox.Tag = MiscItemId.LandOrgDamageLand;
            landStrDamageAirAoDTextBox.Tag = MiscItemId.LandStrDamageAirAoD;
            landOrgDamageAirAoDTextBox.Tag = MiscItemId.LandOrgDamageAirAoD;
            navalStrDamageAirAoDTextBox.Tag = MiscItemId.NavalStrDamageAirAoD;
            navalOrgDamageAirAoDTextBox.Tag = MiscItemId.NavalOrgDamageAirAoD;
            airStrDamageAirAoDTextBox.Tag = MiscItemId.AirStrDamageAirAoD;
            airOrgDamageAirAoDTextBox.Tag = MiscItemId.AirOrgDamageAirAoD;
            navalStrDamageNavyAoDTextBox.Tag = MiscItemId.NavalStrDamageNavyAoD;
            navalOrgDamageNavyAoDTextBox.Tag = MiscItemId.NavalOrgDamageNavyAoD;
            airStrDamageNavyAoDTextBox.Tag = MiscItemId.AirStrDamageNavyAoD;
            airOrgDamageNavyAoDTextBox.Tag = MiscItemId.AirOrgDamageNavyAoD;
            militaryExpenseAttritionModifierTextBox.Tag = MiscItemId.MilitaryExpenseAttritionModifier;
            navalMinCombatTimeTextBox.Tag = MiscItemId.NavalMinCombatTime;
            landMinCombatTimeTextBox.Tag = MiscItemId.LandMinCombatTime;
            airMinCombatTimeTextBox.Tag = MiscItemId.AirMinCombatTime;
            landOverstackingModifierTextBox.Tag = MiscItemId.LandOverstackingModifier;
            landOrgLossMovingTextBox.Tag = MiscItemId.LandOrgLossMoving;
            airOrgLossMovingTextBox.Tag = MiscItemId.AirOrgLossMoving;
            navalOrgLossMovingTextBox.Tag = MiscItemId.NavalOrgLossMoving;
            supplyDistanceSeverityTextBox.Tag = MiscItemId.SupplyDistanceSeverity;
            supplyBaseTextBox.Tag = MiscItemId.SupplyBase;
            landOrgGainTextBox.Tag = MiscItemId.LandOrgGain;
            airOrgGainTextBox.Tag = MiscItemId.AirOrgGain;
            navalOrgGainTextBox.Tag = MiscItemId.NavalOrgGain;
            nukeManpowerDissentTextBox.Tag = MiscItemId.NukeManpowerDissent;
            nukeIcDissentTextBox.Tag = MiscItemId.NukeIcDissent;
            nukeTotalDissentTextBox.Tag = MiscItemId.NukeTotalDissent;
            landFriendlyOrgGainTextBox.Tag = MiscItemId.LandFriendlyOrgGain;
            airLandStockModifierTextBox.Tag = MiscItemId.AirLandStockModifier;
            scorchDamageTextBox.Tag = MiscItemId.ScorchDamage;
            standGroundDissentTextBox.Tag = MiscItemId.StandGroundDissent;
            scorchGroundBelligerenceTextBox.Tag = MiscItemId.ScorchGroundBelligerence;
            defaultLandStackTextBox.Tag = MiscItemId.DefaultLandStack;
            defaultNavalStackTextBox.Tag = MiscItemId.DefaultNavalStack;
            defaultAirStackTextBox.Tag = MiscItemId.DefaultAirStack;
            defaultRocketStackTextBox.Tag = MiscItemId.DefaultRocketStack;
            fortDamageArtilleryBombardmentTextBox.Tag = MiscItemId.FortDamageArtilleryBombardment;
            artilleryBombardmentOrgCostTextBox.Tag = MiscItemId.ArtilleryBombardmentOrgCost;
            landDamageFortTextBox.Tag = MiscItemId.LandDamageFort;
            airRebaseFactorTextBox.Tag = MiscItemId.AirRebaseFactor;
            airMaxDisorganizedTextBox.Tag = MiscItemId.AirMaxDisorganized;
            aaInflictedStrDamageTextBox.Tag = MiscItemId.AaInflictedStrDamage;
            aaInflictedOrgDamageTextBox.Tag = MiscItemId.AaInflictedOrgDamage;
            aaInflictedFlyingDamageTextBox.Tag = MiscItemId.AaInflictedFlyingDamage;
            aaInflictedBombingDamageTextBox.Tag = MiscItemId.AaInflictedBombingDamage;
            hardAttackStrDamageTextBox.Tag = MiscItemId.HardAttackStrDamage;
            hardAttackOrgDamageTextBox.Tag = MiscItemId.HardAttackOrgDamage;
            armorSoftBreakthroughMinTextBox.Tag = MiscItemId.ArmorSoftBreakthroughMin;
            armorSoftBreakthroughMaxTextBox.Tag = MiscItemId.ArmorSoftBreakthroughMax;
            navalCriticalHitChanceTextBox.Tag = MiscItemId.NavalCriticalHitChance;
            navalCriticalHitEffectTextBox.Tag = MiscItemId.NavalCriticalHitEffect;
            landFortDamageTextBox.Tag = MiscItemId.LandFortDamage;
            portAttackSurpriseChanceDayTextBox.Tag = MiscItemId.PortAttackSurpriseChanceDay;
            portAttackSurpriseChanceNightTextBox.Tag = MiscItemId.PortAttackSurpriseChanceNight;
        }

        /// <summary>
        ///     戦闘3タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCombat3ItemTags()
        {
            portAttackSurpriseModifierTextBox.Tag = MiscItemId.PortAttackSurpriseModifier;
            radarAntiSurpriseChanceTextBox.Tag = MiscItemId.RadarAntiSurpriseChance;
            radarAntiSurpriseModifierTextBox.Tag = MiscItemId.RadarAntiSurpriseModifier;
            shoreBombardmentCapTextBox.Tag = MiscItemId.ShoreBombardmentCap;
            counterAttackStrDefenderAoDTextBox.Tag = MiscItemId.CounterAttackStrDefenderAoD;
            counterAttackOrgDefenderAoDTextBox.Tag = MiscItemId.CounterAttackOrgDefenderAoD;
            counterAttackStrAttackerAoDTextBox.Tag = MiscItemId.CounterAttackStrAttackerAoD;
            counterAttackOrgAttackerAoDTextBox.Tag = MiscItemId.CounterAttackOrgAttackerAoD;
            assaultStrDefenderAoDTextBox.Tag = MiscItemId.AssaultStrDefenderAoD;
            assaultOrgDefenderAoDTextBox.Tag = MiscItemId.AssaultOrgDefenderAoD;
            assaultStrAttackerAoDTextBox.Tag = MiscItemId.AssaultStrAttackerAoD;
            assaultOrgAttackerAoDTextBox.Tag = MiscItemId.AssaultOrgAttackerAoD;
            encirclementStrDefenderAoDTextBox.Tag = MiscItemId.EncirclementStrDefenderAoD;
            encirclementOrgDefenderAoDTextBox.Tag = MiscItemId.EncirclementOrgDefenderAoD;
            encirclementStrAttackerAoDTextBox.Tag = MiscItemId.EncirclementStrAttackerAoD;
            encirclementOrgAttackerAoDTextBox.Tag = MiscItemId.EncirclementOrgAttackerAoD;
            ambushStrDefenderAoDTextBox.Tag = MiscItemId.AmbushStrDefenderAoD;
            ambushOrgDefenderAoDTextBox.Tag = MiscItemId.AmbushOrgDefenderAoD;
            ambushStrAttackerAoDTextBox.Tag = MiscItemId.AmbushStrAttackerAoD;
            ambushOrgAttackerAoDTextBox.Tag = MiscItemId.AmbushOrgAttackerAoD;
            delayStrDefenderAoDTextBox.Tag = MiscItemId.DelayStrDefenderAoD;
            delayOrgDefenderAoDTextBox.Tag = MiscItemId.DelayOrgDefenderAoD;
            delayStrAttackerAoDTextBox.Tag = MiscItemId.DelayStrAttackerAoD;
            delayOrgAttackerAoDTextBox.Tag = MiscItemId.DelayOrgAttackerAoD;
            tacticalWithdrawStrDefenderAoDTextBox.Tag = MiscItemId.TacticalWithdrawStrDefenderAoD;
            tacticalWithdrawOrgDefenderAoDTextBox.Tag = MiscItemId.TacticalWithdrawOrgDefenderAoD;
            tacticalWithdrawStrAttackerAoDTextBox.Tag = MiscItemId.TacticalWithdrawStrAttackerAoD;
            tacticalWithdrawOrgAttackerAoDTextBox.Tag = MiscItemId.TacticalWithdrawOrgAttackerAoD;
            breakthroughStrDefenderAoDTextBox.Tag = MiscItemId.BreakthroughStrDefenderAoD;
            breakthroughOrgDefenderAoDTextBox.Tag = MiscItemId.BreakthroughOrgDefenderAoD;
            breakthroughStrAttackerAoDTextBox.Tag = MiscItemId.BreakthroughStrAttackerAoD;
            breakthroughOrgAttackerAoDTextBox.Tag = MiscItemId.BreakthroughOrgAttackerAoD;
        }

        /// <summary>
        ///     戦闘4タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCombat4ItemTags()
        {
            airDogfightXpGainFactorTextBox.Tag = MiscItemId.AirDogfightXpGainFactor;
            hardUnitsAttackingUrbanPenaltyTextBox.Tag = MiscItemId.HardUnitsAttackingUrbanPenalty;
            supplyProblemsModifierLandTextBox.Tag = MiscItemId.SupplyProblemsModifierLand;
            supplyProblemsModifierAirTextBox.Tag = MiscItemId.SupplyProblemsModifierAir;
            supplyProblemsModifierNavalTextBox.Tag = MiscItemId.SupplyProblemsModifierNaval;
            fuelProblemsModifierLandTextBox.Tag = MiscItemId.FuelProblemsModifierLand;
            fuelProblemsModifierAirTextBox.Tag = MiscItemId.FuelProblemsModifierAir;
            fuelProblemsModifierNavalTextBox.Tag = MiscItemId.FuelProblemsModifierNaval;
            convoyEscortsModelTextBox.Tag = MiscItemId.ConvoyEscortsModel;
            durationAirToAirBattlesTextBox.Tag = MiscItemId.DurationAirToAirBattles;
            durationNavalPortBombingTextBox.Tag = MiscItemId.DurationNavalPortBombing;
            durationStrategicBombingTextBox.Tag = MiscItemId.DurationStrategicBombing;
            durationGroundAttackBombingTextBox.Tag = MiscItemId.DurationGroundAttackBombing;
            bonusSimilarTerrainTraitTextBox.Tag = MiscItemId.BonusSimilarTerrainTrait;
            airStrDamageLandOrgTextBox.Tag = MiscItemId.AirStrDamageLandOrg;
            airOrgDamageLandDhTextBox.Tag = MiscItemId.AirOrgDamageLandDh;
            airStrDamageLandDhTextBox.Tag = MiscItemId.AirStrDamageLandDh;
            landOrgDamageLandOrgTextBox.Tag = MiscItemId.LandOrgDamageLandOrg;
            landStrDamageLandDhTextBox.Tag = MiscItemId.LandStrDamageLandDh;
            airOrgDamageAirDhTextBox.Tag = MiscItemId.AirOrgDamageAirDh;
            airStrDamageAirDhTextBox.Tag = MiscItemId.AirStrDamageAirDh;
            landOrgDamageAirDhTextBox.Tag = MiscItemId.LandOrgDamageAirDh;
            landStrDamageAirDhTextBox.Tag = MiscItemId.LandStrDamageAirDh;
            navalOrgDamageAirDhTextBox.Tag = MiscItemId.NavalOrgDamageAirDh;
            navalStrDamageAirDhTextBox.Tag = MiscItemId.NavalStrDamageAirDh;
            subsOrgDamageAirTextBox.Tag = MiscItemId.SubsOrgDamageAir;
            subsStrDamageAirTextBox.Tag = MiscItemId.SubsStrDamageAir;
            airOrgDamageNavyDhTextBox.Tag = MiscItemId.AirOrgDamageNavyDh;
            airStrDamageNavyDhTextBox.Tag = MiscItemId.AirStrDamageNavyDh;
            navalOrgDamageNavyDhTextBox.Tag = MiscItemId.NavalOrgDamageNavyDh;
            navalStrDamageNavyDhTextBox.Tag = MiscItemId.NavalStrDamageNavyDh;
            subsOrgDamageNavyTextBox.Tag = MiscItemId.SubsOrgDamageNavy;
            subsStrDamageNavyTextBox.Tag = MiscItemId.SubsStrDamageNavy;
            navalOrgDamageAaTextBox.Tag = MiscItemId.NavalOrgDamageAa;
            airOrgDamageAaTextBox.Tag = MiscItemId.AirOrgDamageAa;
            airStrDamageAaTextBox.Tag = MiscItemId.AirStrDamageAa;
            aaAirFiringRulesComboBox.Tag = MiscItemId.AaAirFiringRules;
            aaAirNightModifierTextBox.Tag = MiscItemId.AaAirNightModifier;
            aaAirBonusRadarsTextBox.Tag = MiscItemId.AaAirBonusRadars;
            movementBonusTerrainTraitTextBox.Tag = MiscItemId.MovementBonusTerrainTrait;
            movementBonusSimilarTerrainTraitTextBox.Tag = MiscItemId.MovementBonusSimilarTerrainTrait;
            logisticsWizardEseBonusTextBox.Tag = MiscItemId.LogisticsWizardEseBonus;
            daysOffensiveSupplyTextBox.Tag = MiscItemId.DaysOffensiveSupply;
            ministerBonusesComboBox.Tag = MiscItemId.MinisterBonuses;
            orgRegainBonusFriendlyTextBox.Tag = MiscItemId.OrgRegainBonusFriendly;
            orgRegainBonusFriendlyCapTextBox.Tag = MiscItemId.OrgRegainBonusFriendlyCap;
            convoyInterceptionMissionsComboBox.Tag = MiscItemId.ConvoyInterceptionMissions;
            autoReturnTransportFleetsComboBox.Tag = MiscItemId.AutoReturnTransportFleets;
            allowProvinceRegionTargetingComboBox.Tag = MiscItemId.AllowProvinceRegionTargeting;
            nightHoursWinterTextBox.Tag = MiscItemId.NightHoursWinter;
            nightHoursSpringFallTextBox.Tag = MiscItemId.NightHoursSpringFall;
            nightHoursSummerTextBox.Tag = MiscItemId.NightHoursSummer;
            recalculateLandArrivalTimesTextBox.Tag = MiscItemId.RecalculateLandArrivalTimes;
            synchronizeArrivalTimePlayerTextBox.Tag = MiscItemId.SynchronizeArrivalTimePlayer;
            synchronizeArrivalTimeAiTextBox.Tag = MiscItemId.SynchronizeArrivalTimeAi;
            recalculateArrivalTimesCombatComboBox.Tag = MiscItemId.RecalculateArrivalTimesCombat;
            landSpeedModifierCombatTextBox.Tag = MiscItemId.LandSpeedModifierCombat;
            landSpeedModifierBombardmentTextBox.Tag = MiscItemId.LandSpeedModifierBombardment;
            landSpeedModifierSupplyTextBox.Tag = MiscItemId.LandSpeedModifierSupply;
            landSpeedModifierOrgTextBox.Tag = MiscItemId.LandSpeedModifierOrg;
            landAirSpeedModifierFuelTextBox.Tag = MiscItemId.LandAirSpeedModifierFuel;
            defaultSpeedFuelTextBox.Tag = MiscItemId.DefaultSpeedFuel;
            fleetSizeRangePenaltyRatioTextBox.Tag = MiscItemId.FleetSizeRangePenaltyRatio;
            fleetSizeRangePenaltyThretholdTextBox.Tag = MiscItemId.FleetSizeRangePenaltyThrethold;
            fleetSizeRangePenaltyMaxTextBox.Tag = MiscItemId.FleetSizeRangePenaltyMax;
            applyRangeLimitsAreasRegionsComboBox.Tag = MiscItemId.ApplyRangeLimitsAreasRegions;
            radarBonusDetectionTextBox.Tag = MiscItemId.RadarBonusDetection;
            bonusDetectionFriendlyTextBox.Tag = MiscItemId.BonusDetectionFriendly;
            screensCapitalRatioModifierTextBox.Tag = MiscItemId.ScreensCapitalRatioModifier;
            chanceTargetNoOrgLandTextBox.Tag = MiscItemId.ChanceTargetNoOrgLand;
            screenCapitalShipsTargetingTextBox.Tag = MiscItemId.ScreenCapitalShipsTargeting;
        }

        /// <summary>
        ///     戦闘5タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCombat5ItemTags()
        {
            landChanceAvoidDefencesLeftTextBox.Tag = MiscItemId.LandChanceAvoidDefencesLeft;
            airChanceAvoidDefencesLeftTextBox.Tag = MiscItemId.AirChanceAvoidDefencesLeft;
            navalChanceAvoidDefencesLeftTextBox.Tag = MiscItemId.NavalChanceAvoidDefencesLeft;
            landChanceAvoidNoDefencesTextBox.Tag = MiscItemId.LandChanceAvoidNoDefences;
            airChanceAvoidNoDefencesTextBox.Tag = MiscItemId.AirChanceAvoidNoDefences;
            navalChanceAvoidNoDefencesTextBox.Tag = MiscItemId.NavalChanceAvoidNoDefences;
            bonusLeaderSkillPointLandTextBox.Tag = MiscItemId.BonusLeaderSkillPointLand;
            bonusLeaderSkillPointAirTextBox.Tag = MiscItemId.BonusLeaderSkillPointAir;
            bonusLeaderSkillPointNavalTextBox.Tag = MiscItemId.BonusLeaderSkillPointNaval;
            landMinOrgDamageTextBox.Tag = MiscItemId.LandMinOrgDamage;
            landOrgDamageHardSoftEachTextBox.Tag = MiscItemId.LandOrgDamageHardSoftEach;
            landOrgDamageHardVsSoftTextBox.Tag = MiscItemId.LandOrgDamageHardVsSoft;
            landMinStrDamageTextBox.Tag = MiscItemId.LandMinStrDamage;
            landStrDamageHardSoftEachTextBox.Tag = MiscItemId.LandStrDamageHardSoftEach;
            landStrDamageHardVsSoftTextBox.Tag = MiscItemId.LandStrDamageHardVsSoft;
            airMinOrgDamageTextBox.Tag = MiscItemId.AirMinOrgDamage;
            airAdditionalOrgDamageTextBox.Tag = MiscItemId.AirAdditionalOrgDamage;
            airMinStrDamageTextBox.Tag = MiscItemId.AirMinStrDamage;
            airAdditionalStrDamageTextBox.Tag = MiscItemId.AirAdditionalStrDamage;
            airStrDamageEntrencedTextBox.Tag = MiscItemId.AirStrDamageEntrenced;
            navalMinOrgDamageTextBox.Tag = MiscItemId.NavalMinOrgDamage;
            navalAdditionalOrgDamageTextBox.Tag = MiscItemId.NavalAdditionalOrgDamage;
            navalMinStrDamageTextBox.Tag = MiscItemId.NavalMinStrDamage;
            navalAdditionalStrDamageTextBox.Tag = MiscItemId.NavalAdditionalStrDamage;
            landOrgDamageLandUrbanTextBox.Tag = MiscItemId.LandOrgDamageLandUrban;
            landOrgDamageLandFortTextBox.Tag = MiscItemId.LandOrgDamageLandFort;
            requiredLandFortSizeTextBox.Tag = MiscItemId.RequiredLandFortSize;
            fleetPositioningDaytimeTextBox.Tag = MiscItemId.FleetPositioningDaytime;
            fleetPositioningLeaderSkillTextBox.Tag = MiscItemId.FleetPositioningLeaderSkill;
            fleetPositioningFleetSizeTextBox.Tag = MiscItemId.FleetPositioningFleetSize;
            fleetPositioningFleetCompositionTextBox.Tag = MiscItemId.FleetPositioningFleetComposition;
            landCoastalFortsDamageTextBox.Tag = MiscItemId.LandCoastalFortsDamage;
            landCoastalFortsMaxDamageTextBox.Tag = MiscItemId.LandCoastalFortsMaxDamage;
            minSoftnessBrigadesTextBox.Tag = MiscItemId.MinSoftnessBrigades;
            autoRetreatOrgTextBox.Tag = MiscItemId.AutoRetreatOrg;
            landOrgNavalTransportationTextBox.Tag = MiscItemId.LandOrgNavalTransportation;
            maxLandDigTextBox.Tag = MiscItemId.MaxLandDig;
            digIncreaseDayTextBox.Tag = MiscItemId.DigIncreaseDay;
            breakthroughEncirclementMinSpeedTextBox.Tag = MiscItemId.BreakthroughEncirclementMinSpeed;
            breakthroughEncirclementMaxChanceTextBox.Tag = MiscItemId.BreakthroughEncirclementMaxChance;
            breakthroughEncirclementChanceModifierTextBox.Tag = MiscItemId.BreakthroughEncirclementChanceModifier;
            combatEventDurationTextBox.Tag = MiscItemId.CombatEventDuration;
            counterAttackOrgAttackerDhTextBox.Tag = MiscItemId.CounterAttackOrgAttackerDh;
            counterAttackStrAttackerDhTextBox.Tag = MiscItemId.CounterAttackStrAttackerDh;
            counterAttackOrgDefenderDhTextBox.Tag = MiscItemId.CounterAttackOrgDefenderDh;
            counterAttackStrDefenderDhTextBox.Tag = MiscItemId.CounterAttackStrDefenderDh;
            assaultOrgAttackerDhTextBox.Tag = MiscItemId.AssaultOrgAttackerDh;
            assaultStrAttackerDhTextBox.Tag = MiscItemId.AssaultStrAttackerDh;
            assaultOrgDefenderDhTextBox.Tag = MiscItemId.AssaultOrgDefenderDh;
            assaultStrDefenderDhTextBox.Tag = MiscItemId.AssaultStrDefenderDh;
            encirclementOrgAttackerDhTextBox.Tag = MiscItemId.EncirclementOrgAttackerDh;
            encirclementStrAttackerDhTextBox.Tag = MiscItemId.EncirclementStrAttackerDh;
            encirclementOrgDefenderDhTextBox.Tag = MiscItemId.EncirclementOrgDefenderDh;
            encirclementStrDefenderDhTextBox.Tag = MiscItemId.EncirclementStrDefenderDh;
            ambushOrgAttackerDhTextBox.Tag = MiscItemId.AmbushOrgAttackerDh;
            ambushStrAttackerDhTextBox.Tag = MiscItemId.AmbushStrAttackerDh;
            ambushOrgDefenderDhTextBox.Tag = MiscItemId.AmbushOrgDefenderDh;
            ambushStrDefenderDhTextBox.Tag = MiscItemId.AmbushStrDefenderDh;
            delayOrgAttackerDhTextBox.Tag = MiscItemId.DelayOrgAttackerDh;
            delayStrAttackerDhTextBox.Tag = MiscItemId.DelayStrAttackerDh;
            delayOrgDefenderDhTextBox.Tag = MiscItemId.DelayOrgDefenderDh;
            delayStrDefenderDhTextBox.Tag = MiscItemId.DelayStrDefenderDh;
            tacticalWithdrawOrgAttackerDhTextBox.Tag = MiscItemId.TacticalWithdrawOrgAttackerDh;
            tacticalWithdrawStrAttackerDhTextBox.Tag = MiscItemId.TacticalWithdrawStrAttackerDh;
            tacticalWithdrawOrgDefenderDhTextBox.Tag = MiscItemId.TacticalWithdrawOrgDefenderDh;
            tacticalWithdrawStrDefenderDhTextBox.Tag = MiscItemId.TacticalWithdrawStrDefenderDh;
            breakthroughOrgAttackerDhTextBox.Tag = MiscItemId.BreakthroughOrgAttackerDh;
            breakthroughStrAttackerDhTextBox.Tag = MiscItemId.BreakthroughStrAttackerDh;
            breakthroughOrgDefenderDhTextBox.Tag = MiscItemId.BreakthroughOrgDefenderDh;
            breakthroughStrDefenderDhTextBox.Tag = MiscItemId.BreakthroughStrDefenderDh;
            hqStrDamageBreakthroughComboBox.Tag = MiscItemId.HqStrDamageBreakthrough;
            combatModeComboBox.Tag = MiscItemId.CombatMode;
        }

        /// <summary>
        ///     任務1タブの編集項目のタグを初期化する
        /// </summary>
        private void InitMission1ItemTags()
        {
            attackMissionComboBox.Tag = MiscItemId.AttackMission;
            attackStartingEfficiencyTextBox.Tag = MiscItemId.AttackStartingEfficiency;
            attackSpeedBonusTextBox.Tag = MiscItemId.AttackSpeedBonus;
            rebaseMissionComboBox.Tag = MiscItemId.RebaseMission;
            rebaseStartingEfficiencyTextBox.Tag = MiscItemId.RebaseStartingEfficiency;
            rebaseChanceDetectedTextBox.Tag = MiscItemId.RebaseChanceDetected;
            stratRedeployMissionComboBox.Tag = MiscItemId.StratRedeployMission;
            stratRedeployStartingEfficiencyTextBox.Tag = MiscItemId.StratRedeployStartingEfficiency;
            stratRedeployAddedValueTextBox.Tag = MiscItemId.StratRedeployAddedValue;
            stratRedeployDistanceMultiplierTextBox.Tag = MiscItemId.StratRedeployDistanceMultiplier;
            supportAttackMissionComboBox.Tag = MiscItemId.SupportAttackMission;
            supportAttackStartingEfficiencyTextBox.Tag = MiscItemId.SupportAttackStartingEfficiency;
            supportAttackSpeedBonusTextBox.Tag = MiscItemId.SupportAttackSpeedBonus;
            supportDefenseMissionComboBox.Tag = MiscItemId.SupportDefenseMission;
            supportDefenseStartingEfficiencyTextBox.Tag = MiscItemId.SupportDefenseStartingEfficiency;
            supportDefenseSpeedBonusTextBox.Tag = MiscItemId.SupportDefenseSpeedBonus;
            reservesMissionComboBox.Tag = MiscItemId.ReservesMission;
            reservesStartingEfficiencyTextBox.Tag = MiscItemId.ReservesStartingEfficiency;
            reservesSpeedBonusTextBox.Tag = MiscItemId.ReservesSpeedBonus;
            antiPartisanDutyMissionComboBox.Tag = MiscItemId.AntiPartisanDutyMission;
            antiPartisanDutyStartingEfficiencyTextBox.Tag = MiscItemId.AntiPartisanDutyStartingEfficiency;
            antiPartisanDutySuppressionTextBox.Tag = MiscItemId.AntiPartisanDutySuppression;
            plannedDefenseMissionComboBox.Tag = MiscItemId.PlannedDefenseMission;
            plannedDefenseStartingEfficiencyTextBox.Tag = MiscItemId.PlannedDefenseStartingEfficiency;
            airSuperiorityMissionComboBox.Tag = MiscItemId.AirSuperiorityMission;
            airSuperiorityStartingEfficiencyTextBox.Tag = MiscItemId.AirSuperiorityStartingEfficiency;
            airSuperiorityDetectionTextBox.Tag = MiscItemId.AirSuperiorityDetection;
            airSuperiorityMinRequiredTextBox.Tag = MiscItemId.AirSuperiorityMinRequired;
            groundAttackMissionComboBox.Tag = MiscItemId.GroundAttackMission;
            groundAttackStartingEfficiencyTextBox.Tag = MiscItemId.GroundAttackStartingEfficiency;
            groundAttackOrgDamageTextBox.Tag = MiscItemId.GroundAttackOrgDamage;
            groundAttackStrDamageTextBox.Tag = MiscItemId.GroundAttackStrDamage;
            interdictionMissionComboBox.Tag = MiscItemId.InterdictionMission;
            interdictionStartingEfficiencyTextBox.Tag = MiscItemId.InterdictionStartingEfficiency;
            interdictionOrgDamageTextBox.Tag = MiscItemId.InterdictionOrgDamage;
            interdictionStrDamageTextBox.Tag = MiscItemId.InterdictionStrDamage;
            strategicBombardmentMissionComboBox.Tag = MiscItemId.StrategicBombardmentMission;
            strategicBombardmentStartingEfficiencyTextBox.Tag = MiscItemId.StrategicBombardmentStartingEfficiency;
            logisticalStrikeMissionComboBox.Tag = MiscItemId.LogisticalStrikeMission;
            logisticalStrikeStartingEfficiencyTextBox.Tag = MiscItemId.LogisticalStrikeStartingEfficiency;
            runwayCrateringMissionComboBox.Tag = MiscItemId.RunwayCrateringMission;
            runwayCrateringStartingEfficiencyTextBox.Tag = MiscItemId.RunwayCrateringStartingEfficiency;
            installationStrikeMissionComboBox.Tag = MiscItemId.InstallationStrikeMission;
            installationStrikeStartingEfficiencyTextBox.Tag = MiscItemId.InstallationStrikeStartingEfficiency;
            navalStrikeMissionComboBox.Tag = MiscItemId.NavalStrikeMission;
            navalStrikeStartingEfficiencyTextBox.Tag = MiscItemId.NavalStrikeStartingEfficiency;
            portStrikeMissionComboBox.Tag = MiscItemId.PortStrikeMission;
            portStrikeStartingEfficiencyTextBox.Tag = MiscItemId.PortStrikeStartingEfficiency;
            convoyAirRaidingMissionComboBox.Tag = MiscItemId.ConvoyAirRaidingMission;
            convoyAirRaidingStartingEfficiencyTextBox.Tag = MiscItemId.ConvoyAirRaidingStartingEfficiency;
            airSupplyMissionComboBox.Tag = MiscItemId.AirSupplyMission;
            airSupplyStartingEfficiencyTextBox.Tag = MiscItemId.AirSupplyStartingEfficiency;
            airborneAssaultMissionComboBox.Tag = MiscItemId.AirborneAssaultMission;
            airborneAssaultStartingEfficiencyTextBox.Tag = MiscItemId.AirborneAssaultStartingEfficiency;
            nukeMissionComboBox.Tag = MiscItemId.NukeMission;
            nukeStartingEfficiencyTextBox.Tag = MiscItemId.NukeStartingEfficiency;
            airScrambleMissionComboBox.Tag = MiscItemId.AirScrambleMission;
            airScrambleStartingEfficiencyTextBox.Tag = MiscItemId.AirScrambleStartingEfficiency;
            airScrambleDetectionTextBox.Tag = MiscItemId.AirScrambleDetection;
            airScrambleMinRequiredTextBox.Tag = MiscItemId.AirScrambleMinRequired;
            convoyRadingMissionComboBox.Tag = MiscItemId.ConvoyRadingMission;
            convoyRadingStartingEfficiencyTextBox.Tag = MiscItemId.ConvoyRadingStartingEfficiency;
            convoyRadingRangeModifierTextBox.Tag = MiscItemId.ConvoyRadingRangeModifier;
            convoyRadingChanceDetectedTextBox.Tag = MiscItemId.ConvoyRadingChanceDetected;
            aswMissionComboBox.Tag = MiscItemId.AswMission;
            aswStartingEfficiencyTextBox.Tag = MiscItemId.AswStartingEfficiency;
            navalInterdictionMissionComboBox.Tag = MiscItemId.NavalInterdictionMission;
            navalInterdictionStartingEfficiencyTextBox.Tag = MiscItemId.NavalInterdictionStartingEfficiency;
            shoreBombardmentMissionComboBox.Tag = MiscItemId.ShoreBombardmentMission;
            shoreBombardmentStartingEfficiencyTextBox.Tag = MiscItemId.ShoreBombardmentStartingEfficiency;
            shoreBombardmentModifierDhTextBox.Tag = MiscItemId.ShoreBombardmentModifierDh;
        }

        /// <summary>
        ///     任務2タブの編集項目のタグを初期化する
        /// </summary>
        private void InitMission2ItemTags()
        {
            amphibousAssaultMissionComboBox.Tag = MiscItemId.AmphibousAssaultMission;
            amphibousAssaultStartingEfficiencyTextBox.Tag = MiscItemId.AmphibousAssaultStartingEfficiency;
            seaTransportMissionComboBox.Tag = MiscItemId.SeaTransportMission;
            seaTransportStartingEfficiencyTextBox.Tag = MiscItemId.SeaTransportStartingEfficiency;
            seaTransportRangeModifierTextBox.Tag = MiscItemId.SeaTransportRangeModifier;
            seaTransportChanceDetectedTextBox.Tag = MiscItemId.SeaTransportChanceDetected;
            NavalCombatPatrolMissionComboBox.Tag = MiscItemId.NavalCombatPatrolMission;
            NavalCombatPatrolStartingEfficiencyTextBox.Tag = MiscItemId.NavalCombatPatrolStartingEfficiency;
            navalPortStrikeMissionComboBox.Tag = MiscItemId.NavalPortStrikeMission;
            navalPortStrikeStartingEfficiencyTextBox.Tag = MiscItemId.NavalPortStrikeStartingEfficiency;
            navalAirbaseStrikeMissionComboBox.Tag = MiscItemId.NavalAirbaseStrikeMission;
            navalAirbaseStrikeStartingEfficiencyTextBox.Tag = MiscItemId.NavalAirbaseStrikeStartingEfficiency;
            sneakMoveMissionComboBox.Tag = MiscItemId.SneakMoveMission;
            sneakMoveStartingEfficiencyTextBox.Tag = MiscItemId.SneakMoveStartingEfficiency;
            sneakMoveRangeModifierTextBox.Tag = MiscItemId.SneakMoveRangeModifier;
            sneakMoveChanceDetectedTextBox.Tag = MiscItemId.SneakMoveChanceDetected;
            navalScrambleMissionComboBox.Tag = MiscItemId.NavalScrambleMission;
            navalScrambleStartingEfficiencyTextBox.Tag = MiscItemId.NavalScrambleStartingEfficiency;
            navalScrambleSpeedBonusTextBox.Tag = MiscItemId.NavalScrambleSpeedBonus;
            useAttackEfficiencyCombatModifierComboBox.Tag = MiscItemId.UseAttackEfficiencyCombatModifier;
        }

        /// <summary>
        ///     国家タブの編集項目のタグを初期化する
        /// </summary>
        private void InitCountryItemTags()
        {
            landFortEfficiencyTextBox.Tag = MiscItemId.LandFortEfficiency;
            coastalFortEfficiencyTextBox.Tag = MiscItemId.CoastalFortEfficiency;
            groundDefenseEfficiencyTextBox.Tag = MiscItemId.GroundDefenseEfficiency;
            convoyDefenseEfficiencyTextBox.Tag = MiscItemId.ConvoyDefenseEfficiency;
            manpowerBoostTextBox.Tag = MiscItemId.ManpowerBoost;
            transportCapacityModifierTextBox.Tag = MiscItemId.TransportCapacityModifier;
            occupiedTransportCapacityModifierTextBox.Tag = MiscItemId.OccupiedTransportCapacityModifier;
            attritionModifierTextBox.Tag = MiscItemId.AttritionModifier;
            manpowerTrickleBackModifierTextBox.Tag = MiscItemId.ManpowerTrickleBackModifier;
            supplyDistanceModifierTextBox.Tag = MiscItemId.SupplyDistanceModifier;
            repairModifierTextBox.Tag = MiscItemId.RepairModifier;
            researchModifierTextBox.Tag = MiscItemId.ResearchModifier;
            radarEfficiencyTextBox.Tag = MiscItemId.RadarEfficiency;
            hqSupplyEfficiencyBonusTextBox.Tag = MiscItemId.HqSupplyEfficiencyBonus;
            hqCombatEventsBonusTextBox.Tag = MiscItemId.HqCombatEventsBonus;
            combatEventChancesTextBox.Tag = MiscItemId.CombatEventChances;
            friendlyArmyDetectionChanceTextBox.Tag = MiscItemId.FriendlyArmyDetectionChance;
            enemyArmyDetectionChanceTextBox.Tag = MiscItemId.EnemyArmyDetectionChance;
            friendlyIntelligenceChanceTextBox.Tag = MiscItemId.FriendlyIntelligenceChance;
            enemyIntelligenceChanceTextBox.Tag = MiscItemId.EnemyIntelligenceChance;
            maxAmphibiousArmySizeTextBox.Tag = MiscItemId.MaxAmphibiousArmySize;
            energyToOilTextBox.Tag = MiscItemId.EnergyToOil;
            totalProductionEfficiencyTextBox.Tag = MiscItemId.TotalProductionEfficiency;
            supplyProductionEfficiencyTextBox.Tag = MiscItemId.SupplyProductionEfficiency;
            aaPowerTextBox.Tag = MiscItemId.AaPower;
            airSurpriseChanceTextBox.Tag = MiscItemId.AirSurpriseChance;
            landSurpriseChanceTextBox.Tag = MiscItemId.LandSurpriseChance;
            navalSurpriseChanceTextBox.Tag = MiscItemId.NavalSurpriseChance;
            peacetimeIcModifierTextBox.Tag = MiscItemId.PeacetimeIcModifier;
            wartimeIcModifierTextBox.Tag = MiscItemId.WartimeIcModifier;
            buildingsProductionModifierTextBox.Tag = MiscItemId.BuildingsProductionModifier;
            convoysProductionModifierTextBox.Tag = MiscItemId.ConvoysProductionModifier;
            minShipsPositioningBattleTextBox.Tag = MiscItemId.MinShipsPositioningBattle;
            maxShipsPositioningBattleTextBox.Tag = MiscItemId.MaxShipsPositioningBattle;
            peacetimeStockpilesResourcesTextBox.Tag = MiscItemId.PeacetimeStockpilesResources;
            wartimeStockpilesResourcesTextBox.Tag = MiscItemId.WartimeStockpilesResources;
            peacetimeStockpilesOilSuppliesTextBox.Tag = MiscItemId.PeacetimeStockpilesOilSupplies;
            wartimeStockpilesOilSuppliesTextBox.Tag = MiscItemId.WartimeStockpilesOilSupplies;
        }

        /// <summary>
        ///     研究タブの編集項目のタグを初期化する
        /// </summary>
        private void InitResearchItemTags()
        {
            blueprintBonusTextBox.Tag = MiscItemId.BlueprintBonus;
            preHistoricalDateModifierTextBox.Tag = MiscItemId.PreHistoricalDateModifier;
            costSkillLevelTextBox.Tag = MiscItemId.CostSkillLevel;
            meanNumberInventionEventsYearTextBox.Tag = MiscItemId.MeanNumberInventionEventsYear;
            postHistoricalDateModifierAoDTextBox.Tag = MiscItemId.PostHistoricalDateModifierAoD;
            techSpeedModifierTextBox.Tag = MiscItemId.TechSpeedModifier;
            preHistoricalPenaltyLimitTextBox.Tag = MiscItemId.PreHistoricalPenaltyLimit;
            postHistoricalBonusLimitTextBox.Tag = MiscItemId.PostHistoricalBonusLimit;
            maxActiveTechTeamsAoDTextBox.Tag = MiscItemId.MaxActiveTechTeamsAoD;
            requiredIcEachTechTeamAoDTextBox.Tag = MiscItemId.RequiredIcEachTechTeamAoD;
            maximumRandomModifierTextBox.Tag = MiscItemId.MaximumRandomModifier;
            postHistoricalDateModifierDhTextBox.Tag = MiscItemId.PostHistoricalDateModifierDh;
            useNewTechnologyPageLayoutComboBox.Tag = MiscItemId.UseNewTechnologyPageLayout;
            maxActiveTechTeamsDhTextBox.Tag = MiscItemId.MaxActiveTechTeamsDh;
            minActiveTechTeamsTextBox.Tag = MiscItemId.MinActiveTechTeams;
            requiredIcEachTechTeamDhTextBox.Tag = MiscItemId.RequiredIcEachTechTeamDh;
            techOverviewPanelStyleComboBox.Tag = MiscItemId.TechOverviewPanelStyle;
            newCountryRocketryComponentComboBox.Tag = MiscItemId.NewCountryRocketryComponent;
            newCountryNuclearPhysicsComponentComboBox.Tag = MiscItemId.NewCountryNuclearPhysicsComponent;
            newCountryNuclearEngineeringComponentComboBox.Tag = MiscItemId.NewCountryNuclearEngineeringComponent;
            newCountrySecretTechsComboBox.Tag = MiscItemId.NewCountrySecretTechs;
            maxTechTeamSkillTextBox.Tag = MiscItemId.MaxTechTeamSkill;
        }

        /// <summary>
        ///     貿易タブの編集項目のタグを初期化する
        /// </summary>
        private void InitTradeItemTags()
        {
            daysTradeOffersTextBox.Tag = MiscItemId.DaysTradeOffers;
            delayGameStartNewTradesTextBox.Tag = MiscItemId.DelayGameStartNewTrades;
            limitAiNewTradesGameStartTextBox.Tag = MiscItemId.LimitAiNewTradesGameStart;
            desiredOilStockpileTextBox.Tag = MiscItemId.DesiredOilStockpile;
            criticalOilStockpileTextBox.Tag = MiscItemId.CriticalOilStockpile;
            desiredSuppliesStockpileTextBox.Tag = MiscItemId.DesiredSuppliesStockpile;
            criticalSuppliesStockpileTextBox.Tag = MiscItemId.CriticalSuppliesStockpile;
            desiredResourcesStockpileTextBox.Tag = MiscItemId.DesiredResourcesStockpile;
            criticalResourceStockpileTextBox.Tag = MiscItemId.CriticalResourceStockpile;
            wartimeDesiredStockpileMultiplierTextBox.Tag = MiscItemId.WartimeDesiredStockpileMultiplier;
            peacetimeExtraOilImportTextBox.Tag = MiscItemId.PeacetimeExtraOilImport;
            wartimeExtraOilImportTextBox.Tag = MiscItemId.WartimeExtraOilImport;
            extraImportBelowDesiredTextBox.Tag = MiscItemId.ExtraImportBelowDesired;
            percentageProducedSuppliesTextBox.Tag = MiscItemId.PercentageProducedSupplies;
            percentageProducedMoneyTextBox.Tag = MiscItemId.PercentageProducedMoney;
            extraImportStockpileSelectedTextBox.Tag = MiscItemId.ExtraImportStockpileSelected;
            daysDeliverResourcesTradesTextBox.Tag = MiscItemId.DaysDeliverResourcesTrades;
            mergeTradeDealsComboBox.Tag = MiscItemId.MergeTradeDeals;
            manualTradeDealsComboBox.Tag = MiscItemId.ManualTradeDeals;
            puppetsSendSuppliesMoneyTextBox.Tag = MiscItemId.PuppetsSendSuppliesMoney;
            puppetsCriticalSupplyStockpileTextBox.Tag = MiscItemId.PuppetsCriticalSupplyStockpile;
            puppetsMaxPoolResourcesTextBox.Tag = MiscItemId.PuppetsMaxPoolResources;
            newTradeDealsMinEffectivenessTextBox.Tag = MiscItemId.NewTradeDealsMinEffectiveness;
            cancelTradeDealsEffectivenessTextBox.Tag = MiscItemId.CancelTradeDealsEffectiveness;
            autoTradeAiTradeDealsTextBox.Tag = MiscItemId.AutoTradeAiTradeDeals;
        }

        /// <summary>
        ///     AIタブの編集項目のタグを初期化する
        /// </summary>
        private void InitAiItemTags()
        {
            overproduceSuppliesBelowDesiredTextBox.Tag = MiscItemId.OverproduceSuppliesBelowDesired;
            multiplierOverproduceSuppliesWarTextBox.Tag = MiscItemId.MultiplierOverproduceSuppliesWar;
            notProduceSuppliesStockpileOverTextBox.Tag = MiscItemId.NotProduceSuppliesStockpileOver;
            maxSerialLineProductionGarrisonMilitiaTextBox.Tag = MiscItemId.MaxSerialLineProductionGarrisonMilitia;
            minIcSerialProductionNavalAirTextBox.Tag = MiscItemId.MinIcSerialProductionNavalAir;
            notProduceNewUnitsManpowerRatioTextBox.Tag = MiscItemId.NotProduceNewUnitsManpowerRatio;
            notProduceNewUnitsManpowerValueTextBox.Tag = MiscItemId.NotProduceNewUnitsManpowerValue;
            notProduceNewUnitsSupplyTextBox.Tag = MiscItemId.NotProduceNewUnitsSupply;
            militaryStrengthTotalIcRatioPeacetimeTextBox.Tag = MiscItemId.MilitaryStrengthTotalIcRatioPeacetime;
            militaryStrengthTotalIcRatioWartimeTextBox.Tag = MiscItemId.MilitaryStrengthTotalIcRatioWartime;
            militaryStrengthTotalIcRatioMajorTextBox.Tag = MiscItemId.MilitaryStrengthTotalIcRatioMajor;
            notUseOffensiveSupplyStockpileTextBox.Tag = MiscItemId.NotUseOffensiveSupplyStockpile;
            notUseOffensiveOilStockpileTextBox.Tag = MiscItemId.NotUseOffensiveOilStockpile;
            notUseOffensiveEseTextBox.Tag = MiscItemId.NotUseOffensiveEse;
            notUseOffensiveOrgStrDamageTextBox.Tag = MiscItemId.NotUseOffensiveOrgStrDamage;
            aiPeacetimeSpyMissionsDhComboBox.Tag = MiscItemId.AiPeacetimeSpyMissionsDh;
            aiSpyMissionsCostModifierDhTextBox.Tag = MiscItemId.AiSpyMissionsCostModifierDh;
            aiDiplomacyCostModifierDhTextBox.Tag = MiscItemId.AiDiplomacyCostModifierDh;
            aiInfluenceModifierDhTextBox.Tag = MiscItemId.AiInfluenceModifierDh;
            newDowRulesComboBox.Tag = MiscItemId.NewDowRules;
            forcePuppetsJoinMastersAllianceNeutralityTextBox.Tag = MiscItemId.ForcePuppetsJoinMastersAllianceNeutrality;
            newAiReleaseRulesComboBox.Tag = MiscItemId.NewAiReleaseRules;
            aiEventsActionSelectionRulesTextBox.Tag = MiscItemId.AiEventsActionSelectionRules;
            forceStrategicRedeploymentHourTextBox.Tag = MiscItemId.ForceStrategicRedeploymentHour;
            maxRedeploymentDaysAiTextBox.Tag = MiscItemId.MaxRedeploymentDaysAi;
            useQuickAreaCheckGarrisonAiComboBox.Tag = MiscItemId.UseQuickAreaCheckGarrisonAi;
            aiMastersGetProvincesConquredPuppetsComboBox.Tag = MiscItemId.AiMastersGetProvincesConquredPuppets;
            minDaysRequiredAiReleaseCountryTextBox.Tag = MiscItemId.MinDaysRequiredAiReleaseCountry;
            minDaysRequiredAiAlliedTextBox.Tag = MiscItemId.MinDaysRequiredAiAllied;
            minDaysRequiredAiAlliedSupplyBaseTextBox.Tag = MiscItemId.MinDaysRequiredAiAlliedSupplyBase;
            minRequiredRelationsAlliedClaimedTextBox.Tag = MiscItemId.MinRequiredRelationsAlliedClaimed;
            newDowRules2ComboBox.Tag = MiscItemId.NewDowRules2;
        }

        /// <summary>
        ///     MODタブの編集項目のタグを初期化する
        /// </summary>
        private void InitModItemTags()
        {
            aiSpyDiplomaticMissionLoggerComboBox.Tag = MiscItemId.AiSpyDiplomaticMissionLogger;
            countryLoggerTextBox.Tag = MiscItemId.CountryLogger;
            switchedAiFilesLoggerComboBox.Tag = MiscItemId.SwitchedAiFilesLogger;
            useNewAutoSaveFileFormatComboBox.Tag = MiscItemId.UseNewAutoSaveFileFormat;
            loadNewAiSwitchingAllClientsComboBox.Tag = MiscItemId.LoadNewAiSwitchingAllClients;
            tradeEfficiencyCalculationSystemTextBox.Tag = MiscItemId.TradeEfficiencyCalculationSystem;
            mergeRelocateProvincialDepotsTextBox.Tag = MiscItemId.MergeRelocateProvincialDepots;
            inGameLossesLoggingComboBox.Tag = MiscItemId.InGameLossesLogging;
            inGameLossLogging2ComboBox.Tag = MiscItemId.InGameLossLogging2;
            allowBrigadeAttachingInSupplyComboBox.Tag = MiscItemId.AllowBrigadeAttachingInSupply;
            multipleDeploymentSizeArmiesTextBox.Tag = MiscItemId.MultipleDeploymentSizeArmies;
            multipleDeploymentSizeFleetsTextBox.Tag = MiscItemId.MultipleDeploymentSizeFleets;
            multipleDeploymentSizeAirTextBox.Tag = MiscItemId.MultipleDeploymentSizeAir;
            allowUniquePicturesAllLandProvincesComboBox.Tag = MiscItemId.AllowUniquePicturesAllLandProvinces;
            autoReplyEventsComboBox.Tag = MiscItemId.AutoReplyEvents;
            forceActionsShowComboBox.Tag = MiscItemId.ForceActionsShow;
            enableDicisionsPlayersComboBox.Tag = MiscItemId.EnableDicisionsPlayers;
            rebelsArmyCompositionTextBox.Tag = MiscItemId.RebelsArmyComposition;
            rebelsArmyTechLevelTextBox.Tag = MiscItemId.RebelsArmyTechLevel;
            rebelsArmyMinStrTextBox.Tag = MiscItemId.RebelsArmyMinStr;
            rebelsArmyMaxStrTextBox.Tag = MiscItemId.RebelsArmyMaxStr;
            rebelsOrgRegainTextBox.Tag = MiscItemId.RebelsOrgRegain;
            extraRebelBonusNeighboringProvinceTextBox.Tag = MiscItemId.ExtraRebelBonusNeighboringProvince;
            extraRebelBonusOccupiedTextBox.Tag = MiscItemId.ExtraRebelBonusOccupied;
            extraRebelBonusMountainTextBox.Tag = MiscItemId.ExtraRebelBonusMountain;
            extraRebelBonusHillTextBox.Tag = MiscItemId.ExtraRebelBonusHill;
            extraRebelBonusForestTextBox.Tag = MiscItemId.ExtraRebelBonusForest;
            extraRebelBonusJungleTextBox.Tag = MiscItemId.ExtraRebelBonusJungle;
            extraRebelBonusSwampTextBox.Tag = MiscItemId.ExtraRebelBonusSwamp;
            extraRebelBonusDesertTextBox.Tag = MiscItemId.ExtraRebelBonusDesert;
            extraRebelBonusPlainTextBox.Tag = MiscItemId.ExtraRebelBonusPlain;
            extraRebelBonusUrbanTextBox.Tag = MiscItemId.ExtraRebelBonusUrban;
            extraRebelBonusAirNavalBasesTextBox.Tag = MiscItemId.ExtraRebelBonusAirNavalBases;
            returnRebelliousProvinceTextBox.Tag = MiscItemId.ReturnRebelliousProvince;
            useNewMinisterFilesFormatComboBox.Tag = MiscItemId.UseNewMinisterFilesFormat;
            enableRetirementYearMinistersComboBox.Tag = MiscItemId.EnableRetirementYearMinisters;
            enableRetirementYearLeadersComboBox.Tag = MiscItemId.EnableRetirementYearLeaders;
            loadSpritesModdirOnlyComboBox.Tag = MiscItemId.LoadSpritesModdirOnly;
            loadUnitIconsModdirOnlyComboBox.Tag = MiscItemId.LoadUnitIconsModdirOnly;
            loadUnitPicturesModdirOnlyComboBox.Tag = MiscItemId.LoadUnitPicturesModdirOnly;
            loadAiFilesModdirOnlyComboBox.Tag = MiscItemId.LoadAiFilesModdirOnly;
            useSpeedSetGarrisonStatusComboBox.Tag = MiscItemId.UseSpeedSetGarrisonStatus;
            useOldSaveGameFormatComboBox.Tag = MiscItemId.UseOldSaveGameFormat;
            productionPanelUiStyleComboBox.Tag = MiscItemId.ProductionPanelUiStyle;
            unitPicturesSizeComboBox.Tag = MiscItemId.UnitPicturesSize;
            enablePicturesNavalBrigadesComboBox.Tag = MiscItemId.EnablePicturesNavalBrigades;
            buildingsBuildableOnlyProvincesComboBox.Tag = MiscItemId.BuildingsBuildableOnlyProvinces;
            unitModifiersStatisticsPagesTextBox.Tag = MiscItemId.UnitModifiersStatisticsPages;
        }

        /// <summary>
        ///     マップタブの編集項目のタグを初期化する
        /// </summary>
        private void InitMapItemTags()
        {
            mapNumberTextBox.Tag = MiscItemId.MapNumber;
            totalProvincesTextBox.Tag = MiscItemId.TotalProvinces;
            distanceCalculationModelComboBox.Tag = MiscItemId.DistanceCalculationModel;
            MapWidthTextBox.Tag = MiscItemId.MapWidth;
            MapHeightTextBox.Tag = MiscItemId.MapHeight;
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            InitEconomy1EditableItems();
            InitEconomy2EditableItems();
            InitEconomy3EditableItems();
            InitIntelligenceEditableItems();
            InitDiplomacyEditableItems();
            InitCombat1EditableItems();
            InitCombat2EditableItems();
            InitCombat3EditableItems();
            InitCombat4EditableItems();
            InitCombat5EditableItems();
            InitMission1EditableItems();
            InitMission2EditableItems();
            InitCountryEditableItems();
            InitResearchEditableItems();
            InitTradeEditableItems();
            InitAiEditableItems();
            InitModEditableItems();
            InitMapEditableItems();
        }

        /// <summary>
        ///     経済1タブの編集項目を初期化する
        /// </summary>
        private void InitEconomy1EditableItems()
        {
            // DDA1.3以降固有項目
            bool flag = (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130);

            spyMissionDaysLabel.Enabled = flag;
            increateIntelligenceLevelDaysLabel.Enabled = flag;
            chanceDetectSpyMissionLabel.Enabled = flag;
            relationshipsHitDetectedMissionsLabel.Enabled = flag;
            showThirdCountrySpyReportsLabel.Enabled = flag;
            distanceModifierNeighboursLabel.Enabled = flag;
            spyInformationAccuracyModifierLabel.Enabled = flag;
            aiPeacetimeSpyMissionsLabel.Enabled = flag;
            maxIcCostModifierLabel.Enabled = flag;
            aiSpyMissionsCostModifierLabel.Enabled = flag;
            aiDiplomacyCostModifierLabel.Enabled = flag;
            aiInfluenceModifierLabel.Enabled = flag;

            spyMissionDaysTextBox.Enabled = flag;
            increateIntelligenceLevelDaysTextBox.Enabled = flag;
            chanceDetectSpyMissionTextBox.Enabled = flag;
            relationshipsHitDetectedMissionsTextBox.Enabled = flag;
            showThirdCountrySpyReportsComboBox.Enabled = flag;
            distanceModifierNeighboursTextBox.Enabled = flag;
            spyInformationAccuracyModifierTextBox.Enabled = flag;
            aiPeacetimeSpyMissionsComboBox.Enabled = flag;
            maxIcCostModifierTextBox.Enabled = flag;
            aiSpyMissionsCostModifierTextBox.Enabled = flag;
            aiDiplomacyCostModifierTextBox.Enabled = flag;
            aiInfluenceModifierTextBox.Enabled = flag;

            // AoD1.07以降固有項目
            flag = (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107);

            nationalismPerManpowerAoDLabel.Enabled = flag;
            coreProvinceEfficiencyRiseTimeLabel.Enabled = flag;
            restockSpeedLandLabel.Enabled = flag;
            restockSpeedAirLabel.Enabled = flag;
            restockSpeedNavalLabel.Enabled = flag;
            spyCoupDissentModifierLabel.Enabled = flag;
            convoyDutyConversionLabel.Enabled = flag;
            escortDutyConversionLabel.Enabled = flag;
            tpMaxAttachLabel.Enabled = flag;
            ssMaxAttachLabel.Enabled = flag;
            ssnMaxAttachLabel.Enabled = flag;
            ddMaxAttachLabel.Enabled = flag;
            clMaxAttachLabel.Enabled = flag;
            caMaxAttachLabel.Enabled = flag;
            bcMaxAttachLabel.Enabled = flag;
            bbMaxAttachLabel.Enabled = flag;
            cvlMaxAttachLabel.Enabled = flag;
            cvMaxAttachLabel.Enabled = flag;
            canChangeIdeasLabel.Enabled = flag;

            nationalismPerManpowerAoDTextBox.Enabled = flag;
            coreProvinceEfficiencyRiseTimeTextBox.Enabled = flag;
            restockSpeedLandTextBox.Enabled = flag;
            restockSpeedAirTextBox.Enabled = flag;
            restockSpeedNavalTextBox.Enabled = flag;
            spyCoupDissentModifierTextBox.Enabled = flag;
            convoyDutyConversionTextBox.Enabled = flag;
            escortDutyConversionTextBox.Enabled = flag;
            tpMaxAttachTextBox.Enabled = flag;
            ssMaxAttachTextBox.Enabled = flag;
            ssnMaxAttachTextBox.Enabled = flag;
            ddMaxAttachTextBox.Enabled = flag;
            clMaxAttachTextBox.Enabled = flag;
            caMaxAttachTextBox.Enabled = flag;
            bcMaxAttachTextBox.Enabled = flag;
            bbMaxAttachTextBox.Enabled = flag;
            cvlMaxAttachTextBox.Enabled = flag;
            cvMaxAttachTextBox.Enabled = flag;
            canChangeIdeasComboBox.Enabled = flag;

            // DDA/AoD固有項目
            flag = (Game.Type == GameType.HeartsOfIron2 || Game.Type == GameType.ArsenalOfDemocracy);

            requirementAffectSliderLabel.Enabled = flag;
            canUnitSendNonAlliedLabel.Enabled = flag;

            requirementAffectSliderTextBox.Enabled = flag;
            canUnitSendNonAlliedComboBox.Enabled = flag;
        }

        /// <summary>
        ///     経済2タブの編集項目を初期化する
        /// </summary>
        private void InitEconomy2EditableItems()
        {
            // AoD固有項目
            bool flag = (Game.Type == GameType.ArsenalOfDemocracy);

            dissentChangeSpeedLabel.Enabled = flag;
            gearingResourceIncrementLabel.Enabled = flag;
            gearingLossNoIcLabel.Enabled = flag;
            costRepairBuildingsLabel.Enabled = flag;
            timeRepairBuildingLabel.Enabled = flag;
            provinceEfficiencyRiseTimeLabel.Enabled = flag;
            lineUpkeepLabel.Enabled = flag;
            lineStartupTimeLabel.Enabled = flag;
            lineUpgradeTimeLabel.Enabled = flag;
            retoolingCostLabel.Enabled = flag;
            retoolingResourceLabel.Enabled = flag;
            dailyAgingManpowerLabel.Enabled = flag;
            supplyConvoyHuntLabel.Enabled = flag;
            supplyNavalStaticAoDLabel.Enabled = flag;
            supplyNavalMovingLabel.Enabled = flag;
            supplyNavalBattleAoDLabel.Enabled = flag;
            supplyAirStaticAoDLabel.Enabled = flag;
            supplyAirMovingLabel.Enabled = flag;
            supplyAirBattleAoDLabel.Enabled = flag;
            supplyAirBombingLabel.Enabled = flag;
            supplyLandStaticAoDLabel.Enabled = flag;
            supplyLandMovingLabel.Enabled = flag;
            supplyLandBattleAoDLabel.Enabled = flag;
            supplyLandBombingLabel.Enabled = flag;
            supplyStockLandLabel.Enabled = flag;
            supplyStockAirLabel.Enabled = flag;
            supplyStockNavalLabel.Enabled = flag;
            syntheticOilConversionMultiplierLabel.Enabled = flag;
            syntheticRaresConversionMultiplierLabel.Enabled = flag;
            militarySalaryLabel.Enabled = flag;
            maxIntelligenceExpenditureLabel.Enabled = flag;
            maxResearchExpenditureLabel.Enabled = flag;
            militarySalaryAttrictionModifierLabel.Enabled = flag;
            militarySalaryDissentModifierLabel.Enabled = flag;
            nuclearSiteUpkeepCostLabel.Enabled = flag;
            nuclearPowerUpkeepCostLabel.Enabled = flag;
            syntheticOilSiteUpkeepCostLabel.Enabled = flag;
            syntheticRaresSiteUpkeepCostLabel.Enabled = flag;
            durationDetectionLabel.Enabled = flag;
            convoyProvinceHostileTimeLabel.Enabled = flag;
            convoyProvinceBlockedTimeLabel.Enabled = flag;
            autoTradeConvoyLabel.Enabled = flag;
            spyUpkeepCostLabel.Enabled = flag;
            spyDetectionChanceLabel.Enabled = flag;
            infraEfficiencyModifierLabel.Enabled = flag;
            manpowerToConsumerGoodsLabel.Enabled = flag;
            timeBetweenSliderChangesAoDLabel.Enabled = flag;
            minimalPlacementIcLabel.Enabled = flag;
            nuclearPowerLabel.Enabled = flag;
            freeInfraRepairLabel.Enabled = flag;
            maxSliderDissentLabel.Enabled = flag;
            minSliderDissentLabel.Enabled = flag;
            maxDissentSliderMoveLabel.Enabled = flag;
            icConcentrationBonusLabel.Enabled = flag;
            ministerChangeDelayLabel.Enabled = flag;
            ministerChangeEventDelayLabel.Enabled = flag;
            ideaChangeDelayLabel.Enabled = flag;
            ideaChangeEventDelayLabel.Enabled = flag;
            leaderChangeDelayLabel.Enabled = flag;
            changeIdeaDissentLabel.Enabled = flag;
            changeMinisterDissentLabel.Enabled = flag;
            minDissentRevoltLabel.Enabled = flag;
            dissentRevoltMultiplierLabel.Enabled = flag;

            dissentChangeSpeedTextBox.Enabled = flag;
            gearingResourceIncrementTextBox.Enabled = flag;
            gearingLossNoIcTextBox.Enabled = flag;
            costRepairBuildingsTextBox.Enabled = flag;
            timeRepairBuildingTextBox.Enabled = flag;
            provinceEfficiencyRiseTimeTextBox.Enabled = flag;
            lineUpkeepTextBox.Enabled = flag;
            lineStartupTimeTextBox.Enabled = flag;
            lineUpgradeTimeTextBox.Enabled = flag;
            retoolingCostTextBox.Enabled = flag;
            retoolingResourceTextBox.Enabled = flag;
            dailyAgingManpowerTextBox.Enabled = flag;
            supplyConvoyHuntTextBox.Enabled = flag;
            supplyNavalStaticAoDTextBox.Enabled = flag;
            supplyNavalMovingTextBox.Enabled = flag;
            supplyNavalBattleAoDTextBox.Enabled = flag;
            supplyAirStaticAoDTextBox.Enabled = flag;
            supplyAirMovingTextBox.Enabled = flag;
            supplyAirBattleAoDTextBox.Enabled = flag;
            supplyAirBombingTextBox.Enabled = flag;
            supplyLandStaticAoDTextBox.Enabled = flag;
            supplyLandMovingTextBox.Enabled = flag;
            supplyLandBattleAoDTextBox.Enabled = flag;
            supplyLandBombingTextBox.Enabled = flag;
            supplyStockLandTextBox.Enabled = flag;
            supplyStockAirTextBox.Enabled = flag;
            supplyStockNavalTextBox.Enabled = flag;
            syntheticOilConversionMultiplierTextBox.Enabled = flag;
            syntheticRaresConversionMultiplierTextBox.Enabled = flag;
            militarySalaryTextBox.Enabled = flag;
            maxIntelligenceExpenditureTextBox.Enabled = flag;
            maxResearchExpenditureTextBox.Enabled = flag;
            militarySalaryAttrictionModifierTextBox.Enabled = flag;
            militarySalaryDissentModifierTextBox.Enabled = flag;
            nuclearSiteUpkeepCostTextBox.Enabled = flag;
            nuclearPowerUpkeepCostTextBox.Enabled = flag;
            syntheticOilSiteUpkeepCostTextBox.Enabled = flag;
            syntheticRaresSiteUpkeepCostTextBox.Enabled = flag;
            durationDetectionTextBox.Enabled = flag;
            convoyProvinceHostileTimeTextBox.Enabled = flag;
            convoyProvinceBlockedTimeTextBox.Enabled = flag;
            autoTradeConvoyTextBox.Enabled = flag;
            spyUpkeepCostTextBox.Enabled = flag;
            spyDetectionChanceTextBox.Enabled = flag;
            infraEfficiencyModifierTextBox.Enabled = flag;
            manpowerToConsumerGoodsTextBox.Enabled = flag;
            timeBetweenSliderChangesAoDTextBox.Enabled = flag;
            minimalPlacementIcTextBox.Enabled = flag;
            nuclearPowerTextBox.Enabled = flag;
            freeInfraRepairTextBox.Enabled = flag;
            maxSliderDissentTextBox.Enabled = flag;
            minSliderDissentTextBox.Enabled = flag;
            maxDissentSliderMoveTextBox.Enabled = flag;
            icConcentrationBonusTextBox.Enabled = flag;
            ministerChangeDelayTextBox.Enabled = flag;
            ministerChangeEventDelayTextBox.Enabled = flag;
            ideaChangeDelayTextBox.Enabled = flag;
            ideaChangeEventDelayTextBox.Enabled = flag;
            leaderChangeDelayTextBox.Enabled = flag;
            changeIdeaDissentTextBox.Enabled = flag;
            changeMinisterDissentTextBox.Enabled = flag;
            minDissentRevoltTextBox.Enabled = flag;
            dissentRevoltMultiplierTextBox.Enabled = flag;

            // AoD1.04以前固有項目
            flag = (Game.Type == GameType.ArsenalOfDemocracy && Game.Version <= 104);

            transportConversionLabel.Enabled = flag;

            transportConversionTextBox.Enabled = flag;
        }

        /// <summary>
        ///     経済3タブの編集項目を初期化する
        /// </summary>
        private void InitEconomy3EditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            minAvailableIcLabel.Enabled = flag;
            minFinalIcLabel.Enabled = flag;
            dissentReductionLabel.Enabled = flag;
            icMultiplierPuppetLabel.Enabled = flag;
            resourceMultiplierNonNationalLabel.Enabled = flag;
            resourceMultiplierNonOwnedLabel.Enabled = flag;
            resourceMultiplierNonNationalAiLabel.Enabled = flag;
            resourceMultiplierPuppetLabel.Enabled = flag;
            manpowerMultiplierPuppetLabel.Enabled = flag;
            manpowerMultiplierWartimeOverseaLabel.Enabled = flag;
            manpowerMultiplierPeacetimeLabel.Enabled = flag;
            manpowerMultiplierWartimeLabel.Enabled = flag;
            dailyRetiredManpowerLabel.Enabled = flag;
            reinforceToUpdateModifierLabel.Enabled = flag;
            nationalismPerManpowerDhLabel.Enabled = flag;
            maxNationalismLabel.Enabled = flag;
            maxRevoltRiskLabel.Enabled = flag;
            canUnitSendNonAlliedDhLabel.Enabled = flag;
            bluePrintsCanSoldNonAlliedLabel.Enabled = flag;
            provinceCanSoldNonAlliedLabel.Enabled = flag;
            transferAlliedCoreProvincesLabel.Enabled = flag;
            provinceBuildingsRepairModifierLabel.Enabled = flag;
            provinceResourceRepairModifierLabel.Enabled = flag;
            stockpileLimitMultiplierResourceLabel.Enabled = flag;
            stockpileLimitMultiplierSuppliesOilLabel.Enabled = flag;
            overStockpileLimitDailyLossLabel.Enabled = flag;
            maxResourceDepotSizeLabel.Enabled = flag;
            maxSuppliesOilDepotSizeLabel.Enabled = flag;
            desiredStockPilesSuppliesOilLabel.Enabled = flag;
            maxManpowerLabel.Enabled = flag;
            convoyTransportsCapacityLabel.Enabled = flag;
            suppyLandStaticDhLabel.Enabled = flag;
            supplyLandBattleDhLabel.Enabled = flag;
            fuelLandStaticLabel.Enabled = flag;
            fuelLandBattleLabel.Enabled = flag;
            supplyAirStaticDhLabel.Enabled = flag;
            supplyAirBattleDhLabel.Enabled = flag;
            fuelAirNavalStaticLabel.Enabled = flag;
            fuelAirBattleLabel.Enabled = flag;
            supplyNavalStaticDhLabel.Enabled = flag;
            supplyNavalBattleDhLabel.Enabled = flag;
            fuelNavalNotMovingLabel.Enabled = flag;
            fuelNavalBattleLabel.Enabled = flag;
            tpTransportsConversionRatioLabel.Enabled = flag;
            ddEscortsConversionRatioLabel.Enabled = flag;
            clEscortsConversionRatioLabel.Enabled = flag;
            cvlEscortsConversionRatioLabel.Enabled = flag;
            productionLineEditLabel.Enabled = flag;
            gearingBonusLossUpgradeUnitLabel.Enabled = flag;
            gearingBonusLossUpgradeBrigadeLabel.Enabled = flag;
            dissentNukesLabel.Enabled = flag;
            maxDailyDissentLabel.Enabled = flag;

            minAvailableIcTextBox.Enabled = flag;
            minFinalIcTextBox.Enabled = flag;
            dissentReductionTextBox.Enabled = flag;
            icMultiplierPuppetTextBox.Enabled = flag;
            resourceMultiplierNonNationalTextBox.Enabled = flag;
            resourceMultiplierNonOwnedTextBox.Enabled = flag;
            resourceMultiplierNonNationalAiTextBox.Enabled = flag;
            resourceMultiplierPuppetTextBox.Enabled = flag;
            manpowerMultiplierPuppetTextBox.Enabled = flag;
            manpowerMultiplierWartimeOverseaTextBox.Enabled = flag;
            manpowerMultiplierPeacetimeTextBox.Enabled = flag;
            manpowerMultiplierWartimeTextBox.Enabled = flag;
            dailyRetiredManpowerTextBox.Enabled = flag;
            reinforceToUpdateModifierTextBox.Enabled = flag;
            nationalismPerManpowerDhTextBox.Enabled = flag;
            maxNationalismTextBox.Enabled = flag;
            maxRevoltRiskTextBox.Enabled = flag;
            canUnitSendNonAlliedDhComboBox.Enabled = flag;
            bluePrintsCanSoldNonAlliedComboBox.Enabled = flag;
            provinceCanSoldNonAlliedComboBox.Enabled = flag;
            transferAlliedCoreProvincesComboBox.Enabled = flag;
            provinceBuildingsRepairModifierTextBox.Enabled = flag;
            provinceResourceRepairModifierTextBox.Enabled = flag;
            stockpileLimitMultiplierResourceTextBox.Enabled = flag;
            stockpileLimitMultiplierSuppliesOilTextBox.Enabled = flag;
            overStockpileLimitDailyLossTextBox.Enabled = flag;
            maxResourceDepotSizeTextBox.Enabled = flag;
            maxSuppliesOilDepotSizeTextBox.Enabled = flag;
            desiredStockPilesSuppliesOilTextBox.Enabled = flag;
            maxManpowerTextBox.Enabled = flag;
            convoyTransportsCapacityTextBox.Enabled = flag;
            suppyLandStaticDhTextBox.Enabled = flag;
            supplyLandBattleDhTextBox.Enabled = flag;
            fuelLandStaticTextBox.Enabled = flag;
            fuelLandBattleTextBox.Enabled = flag;
            supplyAirStaticDhTextBox.Enabled = flag;
            supplyAirBattleDhTextBox.Enabled = flag;
            fuelAirNavalStaticTextBox.Enabled = flag;
            fuelAirBattleTextBox.Enabled = flag;
            supplyNavalStaticDhTextBox.Enabled = flag;
            supplyNavalBattleDhTextBox.Enabled = flag;
            fuelNavalNotMovingTextBox.Enabled = flag;
            fuelNavalBattleTextBox.Enabled = flag;
            tpTransportsConversionRatioTextBox.Enabled = flag;
            ddEscortsConversionRatioTextBox.Enabled = flag;
            clEscortsConversionRatioTextBox.Enabled = flag;
            cvlEscortsConversionRatioTextBox.Enabled = flag;
            productionLineEditComboBox.Enabled = flag;
            gearingBonusLossUpgradeUnitTextBox.Enabled = flag;
            gearingBonusLossUpgradeBrigadeTextBox.Enabled = flag;
            dissentNukesTextBox.Enabled = flag;
            maxDailyDissentTextBox.Enabled = flag;

            // DH1.03以降固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            nukesProductionModifierLabel.Enabled = flag;
            convoySystemOptionsAlliedLabel.Enabled = flag;
            resourceConvoysBackUnneededLabel.Enabled = flag;

            nukesProductionModifierTextBox.Enabled = flag;
            convoySystemOptionsAlliedComboBox.Enabled = flag;
            resourceConvoysBackUnneededTextBox.Enabled = flag;
        }

        /// <summary>
        ///     諜報タブの編集項目を初期化する
        /// </summary>
        private void InitIntelligenceEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            spyMissionDaysDhLabel.Enabled = flag;
            increateIntelligenceLevelDaysDhLabel.Enabled = flag;
            chanceDetectSpyMissionDhLabel.Enabled = flag;
            relationshipsHitDetectedMissionsDhLabel.Enabled = flag;
            distanceModifierLabel.Enabled = flag;
            distanceModifierNeighboursDhLabel.Enabled = flag;
            spyLevelBonusDistanceModifierLabel.Enabled = flag;
            spyLevelBonusDistanceModifierAboveTenLabel.Enabled = flag;
            spyInformationAccuracyModifierDhLabel.Enabled = flag;
            icModifierCostLabel.Enabled = flag;
            minIcCostModifierLabel.Enabled = flag;
            maxIcCostModifierDhLabel.Enabled = flag;
            extraMaintenanceCostAboveTenLabel.Enabled = flag;
            extraCostIncreasingAboveTenLabel.Enabled = flag;
            showThirdCountrySpyReportsDhLabel.Enabled = flag;
            spiesMoneyModifierLabel.Enabled = flag;

            spyMissionDaysDhTextBox.Enabled = flag;
            increateIntelligenceLevelDaysDhTextBox.Enabled = flag;
            chanceDetectSpyMissionDhTextBox.Enabled = flag;
            relationshipsHitDetectedMissionsDhTextBox.Enabled = flag;
            distanceModifierTextBox.Enabled = flag;
            distanceModifierNeighboursDhTextBox.Enabled = flag;
            spyLevelBonusDistanceModifierTextBox.Enabled = flag;
            spyLevelBonusDistanceModifierAboveTenTextBox.Enabled = flag;
            spyInformationAccuracyModifierDhTextBox.Enabled = flag;
            icModifierCostTextBox.Enabled = flag;
            minIcCostModifierTextBox.Enabled = flag;
            maxIcCostModifierDhTextBox.Enabled = flag;
            extraMaintenanceCostAboveTenTextBox.Enabled = flag;
            extraCostIncreasingAboveTenTextBox.Enabled = flag;
            showThirdCountrySpyReportsDhComboBox.Enabled = flag;
            spiesMoneyModifierTextBox.Enabled = flag;
        }

        /// <summary>
        ///     外交タブの編集項目を初期化する
        /// </summary>
        private void InitDiplomacyEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            daysBetweenDiplomaticMissionsLabel.Enabled = flag;
            timeBetweenSliderChangesDhLabel.Enabled = flag;
            requirementAffectSliderDhLabel.Enabled = flag;
            useMinisterPersonalityReplacingLabel.Enabled = flag;
            relationshipHitCancelTradeLabel.Enabled = flag;
            relationshipHitCancelPermanentTradeLabel.Enabled = flag;
            puppetsJoinMastersAllianceLabel.Enabled = flag;
            mastersBecomePuppetsPuppetsLabel.Enabled = flag;
            allowManualClaimsChangeLabel.Enabled = flag;
            belligerenceClaimedProvinceLabel.Enabled = flag;
            belligerenceClaimsRemovalLabel.Enabled = flag;
            joinAutomaticallyAllesAxisLabel.Enabled = flag;
            allowChangeHosHogLabel.Enabled = flag;
            changeTagCoupLabel.Enabled = flag;
            filterReleaseCountriesLabel.Enabled = flag;

            daysBetweenDiplomaticMissionsTextBox.Enabled = flag;
            timeBetweenSliderChangesDhTextBox.Enabled = flag;
            requirementAffectSliderDhTextBox.Enabled = flag;
            useMinisterPersonalityReplacingComboBox.Enabled = flag;
            relationshipHitCancelTradeTextBox.Enabled = flag;
            relationshipHitCancelPermanentTradeTextBox.Enabled = flag;
            puppetsJoinMastersAllianceComboBox.Enabled = flag;
            mastersBecomePuppetsPuppetsComboBox.Enabled = flag;
            allowManualClaimsChangeComboBox.Enabled = flag;
            belligerenceClaimedProvinceTextBox.Enabled = flag;
            belligerenceClaimsRemovalTextBox.Enabled = flag;
            joinAutomaticallyAllesAxisComboBox.Enabled = flag;
            allowChangeHosHogComboBox.Enabled = flag;
            changeTagCoupComboBox.Enabled = flag;
            filterReleaseCountriesComboBox.Enabled = flag;
        }

        /// <summary>
        ///     戦闘1タブの編集項目を初期化する
        /// </summary>
        private void InitCombat1EditableItems()
        {
            // AoDに存在しない項目
            bool flag = (Game.Type != GameType.ArsenalOfDemocracy);
            airOverstackingModifierLabel.Enabled = flag;

            airOverstackingModifierTextBox.Enabled = flag;

            // DHに存在しない項目
            flag = (Game.Type != GameType.DarkestHour);

            shoreBombardmentModifierLabel.Enabled = flag;
            supplyProblemsModifierLabel.Enabled = flag;
            airOrgDamageLabel.Enabled = flag;

            shoreBombardmentModifierTextBox.Enabled = flag;
            supplyProblemsModifierTextBox.Enabled = flag;
            airOrgDamageTextBox.Enabled = flag;

            // DH1.03以降に存在しない項目
            flag = (Game.Type != GameType.DarkestHour || Game.Version < 103);

            howEffectiveGroundDefLabel.Enabled = flag;
            chanceAvoidDefencesLeftLabel.Enabled = flag;
            chanceAvoidNoDefencesLabel.Enabled = flag;

            howEffectiveGroundDefTextBox.Enabled = flag;
            chanceAvoidDefencesLeftTextBox.Enabled = flag;
            chanceAvoidNoDefencesTextBox.Enabled = flag;

            // AODにもDHにも存在しない項目
            flag = (Game.Type != GameType.ArsenalOfDemocracy && Game.Type != GameType.DarkestHour);

            airStrDamageOrgLabel.Enabled = flag;
            airStrDamageLabel.Enabled = flag;

            airStrDamageOrgTextBox.Enabled = flag;
            airStrDamageTextBox.Enabled = flag;

            // DDA1.3以降固有項目
            flag = (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130);

            subsOrgDamageLabel.Enabled = flag;
            subsStrDamageLabel.Enabled = flag;

            subsOrgDamageTextBox.Enabled = flag;
            subsStrDamageTextBox.Enabled = flag;

            // DDA1.3以降またはDH固有項目
            flag = ((Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130) || Game.Type == GameType.DarkestHour);

            subStacksDetectionModifierLabel.Enabled = flag;

            subStacksDetectionModifierTextBox.Enabled = flag;
        }

        /// <summary>
        ///     戦闘2タブの編集項目を初期化する
        /// </summary>
        private void InitCombat2EditableItems()
        {
            // AoD固有項目
            bool flag = (Game.Type == GameType.ArsenalOfDemocracy);

            noSupplyAttritionSeverityLabel.Enabled = flag;
            noSupplyMinimunAttritionLabel.Enabled = flag;
            raderStationAaMultiplierLabel.Enabled = flag;
            airOverstackingModifierAoDLabel.Enabled = flag;
            landDelayBeforeOrdersLabel.Enabled = flag;
            navalDelayBeforeOrdersLabel.Enabled = flag;
            airDelayBeforeOrdersLabel.Enabled = flag;
            damageSyntheticOilBombingLabel.Enabled = flag;
            airOrgDamageLandAoDLabel.Enabled = flag;
            airStrDamageLandAoDLabel.Enabled = flag;
            landDamageArtilleryBombardmentLabel.Enabled = flag;
            infraDamageArtilleryBombardmentLabel.Enabled = flag;
            icDamageArtilleryBombardmentLabel.Enabled = flag;
            resourcesDamageArtilleryBombardmentLabel.Enabled = flag;
            penaltyArtilleryBombardmentLabel.Enabled = flag;
            artilleryStrDamageLabel.Enabled = flag;
            artilleryOrgDamageLabel.Enabled = flag;
            landStrDamageLandAoDLabel.Enabled = flag;
            landOrgDamageLandLabel.Enabled = flag;
            landStrDamageAirAoDLabel.Enabled = flag;
            landOrgDamageAirAoDLabel.Enabled = flag;
            navalStrDamageAirAoDLabel.Enabled = flag;
            navalOrgDamageAirAoDLabel.Enabled = flag;
            airStrDamageAirAoDLabel.Enabled = flag;
            airOrgDamageAirAoDLabel.Enabled = flag;
            navalStrDamageNavyAoDLabel.Enabled = flag;
            navalOrgDamageNavyAoDLabel.Enabled = flag;
            airStrDamageNavyAoDLabel.Enabled = flag;
            airOrgDamageNavyAoDLabel.Enabled = flag;
            militaryExpenseAttritionModifierLabel.Enabled = flag;
            navalMinCombatTimeLabel.Enabled = flag;
            landMinCombatTimeLabel.Enabled = flag;
            airMinCombatTimeLabel.Enabled = flag;
            landOverstackingModifierLabel.Enabled = flag;
            landOrgLossMovingLabel.Enabled = flag;
            airOrgLossMovingLabel.Enabled = flag;
            navalOrgLossMovingLabel.Enabled = flag;
            supplyDistanceSeverityLabel.Enabled = flag;
            supplyBaseLabel.Enabled = flag;
            landOrgGainLabel.Enabled = flag;
            airOrgGainLabel.Enabled = flag;
            navalOrgGainLabel.Enabled = flag;
            nukeManpowerDissentLabel.Enabled = flag;
            nukeIcDissentLabel.Enabled = flag;
            nukeTotalDissentLabel.Enabled = flag;
            landFriendlyOrgGainLabel.Enabled = flag;
            airLandStockModifierLabel.Enabled = flag;
            scorchDamageLabel.Enabled = flag;
            standGroundDissentLabel.Enabled = flag;
            scorchGroundBelligerenceLabel.Enabled = flag;
            defaultLandStackLabel.Enabled = flag;
            defaultNavalStackLabel.Enabled = flag;
            defaultAirStackLabel.Enabled = flag;
            defaultRocketStackLabel.Enabled = flag;
            fortDamageArtilleryBombardmentLabel.Enabled = flag;
            artilleryBombardmentOrgCostLabel.Enabled = flag;
            landDamageFortLabel.Enabled = flag;
            airRebaseFactorLabel.Enabled = flag;
            airMaxDisorganizedLabel.Enabled = flag;
            aaInflictedStrDamageLabel.Enabled = flag;
            aaInflictedOrgDamageLabel.Enabled = flag;
            aaInflictedFlyingDamageLabel.Enabled = flag;
            aaInflictedBombingDamageLabel.Enabled = flag;
            hardAttackStrDamageLabel.Enabled = flag;
            hardAttackOrgDamageLabel.Enabled = flag;
            armorSoftBreakthroughMinLabel.Enabled = flag;
            armorSoftBreakthroughMaxLabel.Enabled = flag;
            navalCriticalHitChanceLabel.Enabled = flag;
            navalCriticalHitEffectLabel.Enabled = flag;
            landFortDamageLabel.Enabled = flag;
            portAttackSurpriseChanceDayLabel.Enabled = flag;
            portAttackSurpriseChanceNightLabel.Enabled = flag;

            noSupplyAttritionSeverityTextBox.Enabled = flag;
            noSupplyMinimunAttritionTextBox.Enabled = flag;
            raderStationAaMultiplierTextBox.Enabled = flag;
            airOverstackingModifierAoDTextBox.Enabled = flag;
            landDelayBeforeOrdersTextBox.Enabled = flag;
            navalDelayBeforeOrdersTextBox.Enabled = flag;
            airDelayBeforeOrdersTextBox.Enabled = flag;
            damageSyntheticOilBombingTextBox.Enabled = flag;
            airOrgDamageLandAoDTextBox.Enabled = flag;
            airStrDamageLandAoDTextBox.Enabled = flag;
            landDamageArtilleryBombardmentTextBox.Enabled = flag;
            infraDamageArtilleryBombardmentTextBox.Enabled = flag;
            icDamageArtilleryBombardmentTextBox.Enabled = flag;
            resourcesDamageArtilleryBombardmentTextBox.Enabled = flag;
            penaltyArtilleryBombardmentTextBox.Enabled = flag;
            artilleryStrDamageTextBox.Enabled = flag;
            artilleryOrgDamageTextBox.Enabled = flag;
            landStrDamageLandAoDTextBox.Enabled = flag;
            landOrgDamageLandTextBox.Enabled = flag;
            landStrDamageAirAoDTextBox.Enabled = flag;
            landOrgDamageAirAoDTextBox.Enabled = flag;
            navalStrDamageAirAoDTextBox.Enabled = flag;
            navalOrgDamageAirAoDTextBox.Enabled = flag;
            airStrDamageAirAoDTextBox.Enabled = flag;
            airOrgDamageAirAoDTextBox.Enabled = flag;
            navalStrDamageNavyAoDTextBox.Enabled = flag;
            navalOrgDamageNavyAoDTextBox.Enabled = flag;
            airStrDamageNavyAoDTextBox.Enabled = flag;
            airOrgDamageNavyAoDTextBox.Enabled = flag;
            militaryExpenseAttritionModifierTextBox.Enabled = flag;
            navalMinCombatTimeTextBox.Enabled = flag;
            landMinCombatTimeTextBox.Enabled = flag;
            airMinCombatTimeTextBox.Enabled = flag;
            landOverstackingModifierTextBox.Enabled = flag;
            landOrgLossMovingTextBox.Enabled = flag;
            airOrgLossMovingTextBox.Enabled = flag;
            navalOrgLossMovingTextBox.Enabled = flag;
            supplyDistanceSeverityTextBox.Enabled = flag;
            supplyBaseTextBox.Enabled = flag;
            landOrgGainTextBox.Enabled = flag;
            airOrgGainTextBox.Enabled = flag;
            navalOrgGainTextBox.Enabled = flag;
            nukeManpowerDissentTextBox.Enabled = flag;
            nukeIcDissentTextBox.Enabled = flag;
            nukeTotalDissentTextBox.Enabled = flag;
            landFriendlyOrgGainTextBox.Enabled = flag;
            airLandStockModifierTextBox.Enabled = flag;
            scorchDamageTextBox.Enabled = flag;
            standGroundDissentTextBox.Enabled = flag;
            scorchGroundBelligerenceTextBox.Enabled = flag;
            defaultLandStackTextBox.Enabled = flag;
            defaultNavalStackTextBox.Enabled = flag;
            defaultAirStackTextBox.Enabled = flag;
            defaultRocketStackTextBox.Enabled = flag;
            fortDamageArtilleryBombardmentTextBox.Enabled = flag;
            artilleryBombardmentOrgCostTextBox.Enabled = flag;
            landDamageFortTextBox.Enabled = flag;
            airRebaseFactorTextBox.Enabled = flag;
            airMaxDisorganizedTextBox.Enabled = flag;
            aaInflictedStrDamageTextBox.Enabled = flag;
            aaInflictedOrgDamageTextBox.Enabled = flag;
            aaInflictedFlyingDamageTextBox.Enabled = flag;
            aaInflictedBombingDamageTextBox.Enabled = flag;
            hardAttackStrDamageTextBox.Enabled = flag;
            hardAttackOrgDamageTextBox.Enabled = flag;
            armorSoftBreakthroughMinTextBox.Enabled = flag;
            armorSoftBreakthroughMaxTextBox.Enabled = flag;
            navalCriticalHitChanceTextBox.Enabled = flag;
            navalCriticalHitEffectTextBox.Enabled = flag;
            landFortDamageTextBox.Enabled = flag;
            portAttackSurpriseChanceDayTextBox.Enabled = flag;
            portAttackSurpriseChanceNightTextBox.Enabled = flag;
        }

        /// <summary>
        ///     戦闘3タブの編集項目を初期化する
        /// </summary>
        private void InitCombat3EditableItems()
        {
            // AoD固有項目
            bool flag = (Game.Type == GameType.ArsenalOfDemocracy);

            portAttackSurpriseModifierLabel.Enabled = flag;
            radarAntiSurpriseChanceLabel.Enabled = flag;
            radarAntiSurpriseModifierLabel.Enabled = flag;

            portAttackSurpriseModifierTextBox.Enabled = flag;
            radarAntiSurpriseChanceTextBox.Enabled = flag;
            radarAntiSurpriseModifierTextBox.Enabled = flag;

            // AoD1.08以降固有項目
            flag = (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108);

            shoreBombardmentCapLabel.Enabled = flag;
            counterAttackStrDefenderAoDLabel.Enabled = flag;
            counterAttackOrgDefenderAoDLabel.Enabled = flag;
            counterAttackStrAttackerAoDLabel.Enabled = flag;
            counterAttackOrgAttackerAoDLabel.Enabled = flag;
            assaultStrDefenderAoDLabel.Enabled = flag;
            assaultOrgDefenderAoDLabel.Enabled = flag;
            assaultStrAttackerAoDLabel.Enabled = flag;
            assaultOrgAttackerAoDLabel.Enabled = flag;
            encirclementStrDefenderAoDLabel.Enabled = flag;
            encirclementOrgDefenderAoDLabel.Enabled = flag;
            encirclementStrAttackerAoDLabel.Enabled = flag;
            encirclementOrgAttackerAoDLabel.Enabled = flag;
            ambushStrDefenderAoDLabel.Enabled = flag;
            ambushOrgDefenderAoDLabel.Enabled = flag;
            ambushStrAttackerAoDLabel.Enabled = flag;
            ambushOrgAttackerAoDLabel.Enabled = flag;
            delayStrDefenderAoDLabel.Enabled = flag;
            delayOrgDefenderAoDLabel.Enabled = flag;
            delayStrAttackerAoDLabel.Enabled = flag;
            delayOrgAttackerAoDLabel.Enabled = flag;
            tacticalWithdrawStrDefenderAoDLabel.Enabled = flag;
            tacticalWithdrawOrgDefenderAoDLabel.Enabled = flag;
            tacticalWithdrawStrAttackerAoDLabel.Enabled = flag;
            tacticalWithdrawOrgAttackerAoDLabel.Enabled = flag;
            breakthroughStrDefenderAoDLabel.Enabled = flag;
            breakthroughOrgDefenderAoDLabel.Enabled = flag;
            breakthroughStrAttackerAoDLabel.Enabled = flag;
            breakthroughOrgAttackerAoDLabel.Enabled = flag;

            shoreBombardmentCapTextBox.Enabled = flag;
            counterAttackStrDefenderAoDTextBox.Enabled = flag;
            counterAttackOrgDefenderAoDTextBox.Enabled = flag;
            counterAttackStrAttackerAoDTextBox.Enabled = flag;
            counterAttackOrgAttackerAoDTextBox.Enabled = flag;
            assaultStrDefenderAoDTextBox.Enabled = flag;
            assaultOrgDefenderAoDTextBox.Enabled = flag;
            assaultStrAttackerAoDTextBox.Enabled = flag;
            assaultOrgAttackerAoDTextBox.Enabled = flag;
            encirclementStrDefenderAoDTextBox.Enabled = flag;
            encirclementOrgDefenderAoDTextBox.Enabled = flag;
            encirclementStrAttackerAoDTextBox.Enabled = flag;
            encirclementOrgAttackerAoDTextBox.Enabled = flag;
            ambushStrDefenderAoDTextBox.Enabled = flag;
            ambushOrgDefenderAoDTextBox.Enabled = flag;
            ambushStrAttackerAoDTextBox.Enabled = flag;
            ambushOrgAttackerAoDTextBox.Enabled = flag;
            delayStrDefenderAoDTextBox.Enabled = flag;
            delayOrgDefenderAoDTextBox.Enabled = flag;
            delayStrAttackerAoDTextBox.Enabled = flag;
            delayOrgAttackerAoDTextBox.Enabled = flag;
            tacticalWithdrawStrDefenderAoDTextBox.Enabled = flag;
            tacticalWithdrawOrgDefenderAoDTextBox.Enabled = flag;
            tacticalWithdrawStrAttackerAoDTextBox.Enabled = flag;
            tacticalWithdrawOrgAttackerAoDTextBox.Enabled = flag;
            breakthroughStrDefenderAoDTextBox.Enabled = flag;
            breakthroughOrgDefenderAoDTextBox.Enabled = flag;
            breakthroughStrAttackerAoDTextBox.Enabled = flag;
            breakthroughOrgAttackerAoDTextBox.Enabled = flag;
        }

        /// <summary>
        ///     戦闘4タブの編集項目を初期化する
        /// </summary>
        private void InitCombat4EditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            airDogfightXpGainFactorLabel.Enabled = flag;
            hardUnitsAttackingUrbanPenaltyLabel.Enabled = flag;
            supplyProblemsModifierLandLabel.Enabled = flag;
            supplyProblemsModifierAirLabel.Enabled = flag;
            supplyProblemsModifierNavalLabel.Enabled = flag;
            fuelProblemsModifierLandLabel.Enabled = flag;
            fuelProblemsModifierAirLabel.Enabled = flag;
            fuelProblemsModifierNavalLabel.Enabled = flag;
            convoyEscortsModelLabel.Enabled = flag;
            durationAirToAirBattlesLabel.Enabled = flag;
            durationNavalPortBombingLabel.Enabled = flag;
            durationStrategicBombingLabel.Enabled = flag;
            durationGroundAttackBombingLabel.Enabled = flag;
            bonusSimilarTerrainTraitLabel.Enabled = flag;
            airStrDamageLandOrgLabel.Enabled = flag;
            airOrgDamageLandDhLabel.Enabled = flag;
            airStrDamageLandDhLabel.Enabled = flag;
            landOrgDamageLandOrgLabel.Enabled = flag;
            landStrDamageLandDhLabel.Enabled = flag;
            airOrgDamageAirDhLabel.Enabled = flag;
            airStrDamageAirDhLabel.Enabled = flag;
            landOrgDamageAirDhLabel.Enabled = flag;
            landStrDamageAirDhLabel.Enabled = flag;
            navalOrgDamageAirDhLabel.Enabled = flag;
            navalStrDamageAirDhLabel.Enabled = flag;
            subsOrgDamageAirLabel.Enabled = flag;
            subsStrDamageAirLabel.Enabled = flag;
            airOrgDamageNavyDhLabel.Enabled = flag;
            airStrDamageNavyDhLabel.Enabled = flag;
            navalOrgDamageNavyDhLabel.Enabled = flag;
            navalStrDamageNavyDhLabel.Enabled = flag;
            subsOrgDamageNavyLabel.Enabled = flag;
            subsStrDamageNavyLabel.Enabled = flag;
            navalOrgDamageAaLabel.Enabled = flag;
            airOrgDamageAaLabel.Enabled = flag;
            airStrDamageAaLabel.Enabled = flag;
            aaAirFiringRulesLabel.Enabled = flag;
            aaAirNightModifierLabel.Enabled = flag;
            aaAirBonusRadarsLabel.Enabled = flag;
            movementBonusTerrainTraitLabel.Enabled = flag;
            movementBonusSimilarTerrainTraitLabel.Enabled = flag;
            logisticsWizardEseBonusLabel.Enabled = flag;
            daysOffensiveSupplyLabel.Enabled = flag;
            ministerBonusesLabel.Enabled = flag;
            orgRegainBonusFriendlyLabel.Enabled = flag;
            orgRegainBonusFriendlyCapLabel.Enabled = flag;
            convoyInterceptionMissionsLabel.Enabled = flag;
            autoReturnTransportFleetsLabel.Enabled = flag;
            allowProvinceRegionTargetingLabel.Enabled = flag;
            nightHoursWinterLabel.Enabled = flag;
            nightHoursSpringFallLabel.Enabled = flag;
            nightHoursSummerLabel.Enabled = flag;
            recalculateLandArrivalTimesLabel.Enabled = flag;
            synchronizeArrivalTimePlayerLabel.Enabled = flag;
            synchronizeArrivalTimeAiLabel.Enabled = flag;
            recalculateArrivalTimesCombatLabel.Enabled = flag;
            landSpeedModifierCombatLabel.Enabled = flag;
            landSpeedModifierBombardmentLabel.Enabled = flag;
            landSpeedModifierSupplyLabel.Enabled = flag;
            landSpeedModifierOrgLabel.Enabled = flag;
            landAirSpeedModifierFuelLabel.Enabled = flag;
            defaultSpeedFuelLabel.Enabled = flag;
            fleetSizeRangePenaltyRatioLabel.Enabled = flag;
            fleetSizeRangePenaltyThretholdLabel.Enabled = flag;
            fleetSizeRangePenaltyMaxLabel.Enabled = flag;
            applyRangeLimitsAreasRegionsLabel.Enabled = flag;
            radarBonusDetectionLabel.Enabled = flag;
            bonusDetectionFriendlyLabel.Enabled = flag;
            screensCapitalRatioModifierLabel.Enabled = flag;
            chanceTargetNoOrgLandLabel.Enabled = flag;
            screenCapitalShipsTargetingLabel.Enabled = flag;

            airDogfightXpGainFactorTextBox.Enabled = flag;
            hardUnitsAttackingUrbanPenaltyTextBox.Enabled = flag;
            supplyProblemsModifierLandTextBox.Enabled = flag;
            supplyProblemsModifierAirTextBox.Enabled = flag;
            supplyProblemsModifierNavalTextBox.Enabled = flag;
            fuelProblemsModifierLandTextBox.Enabled = flag;
            fuelProblemsModifierAirTextBox.Enabled = flag;
            fuelProblemsModifierNavalTextBox.Enabled = flag;
            convoyEscortsModelTextBox.Enabled = flag;
            durationAirToAirBattlesTextBox.Enabled = flag;
            durationNavalPortBombingTextBox.Enabled = flag;
            durationStrategicBombingTextBox.Enabled = flag;
            durationGroundAttackBombingTextBox.Enabled = flag;
            bonusSimilarTerrainTraitTextBox.Enabled = flag;
            airStrDamageLandOrgTextBox.Enabled = flag;
            airOrgDamageLandDhTextBox.Enabled = flag;
            airStrDamageLandDhTextBox.Enabled = flag;
            landOrgDamageLandOrgTextBox.Enabled = flag;
            landStrDamageLandDhTextBox.Enabled = flag;
            airOrgDamageAirDhTextBox.Enabled = flag;
            airStrDamageAirDhTextBox.Enabled = flag;
            landOrgDamageAirDhTextBox.Enabled = flag;
            landStrDamageAirDhTextBox.Enabled = flag;
            navalOrgDamageAirDhTextBox.Enabled = flag;
            navalStrDamageAirDhTextBox.Enabled = flag;
            subsOrgDamageAirTextBox.Enabled = flag;
            subsStrDamageAirTextBox.Enabled = flag;
            airOrgDamageNavyDhTextBox.Enabled = flag;
            airStrDamageNavyDhTextBox.Enabled = flag;
            navalOrgDamageNavyDhTextBox.Enabled = flag;
            navalStrDamageNavyDhTextBox.Enabled = flag;
            subsOrgDamageNavyTextBox.Enabled = flag;
            subsStrDamageNavyTextBox.Enabled = flag;
            navalOrgDamageAaTextBox.Enabled = flag;
            airOrgDamageAaTextBox.Enabled = flag;
            airStrDamageAaTextBox.Enabled = flag;
            aaAirFiringRulesComboBox.Enabled = flag;
            aaAirNightModifierTextBox.Enabled = flag;
            aaAirBonusRadarsTextBox.Enabled = flag;
            movementBonusTerrainTraitTextBox.Enabled = flag;
            movementBonusSimilarTerrainTraitTextBox.Enabled = flag;
            logisticsWizardEseBonusTextBox.Enabled = flag;
            daysOffensiveSupplyTextBox.Enabled = flag;
            ministerBonusesComboBox.Enabled = flag;
            orgRegainBonusFriendlyTextBox.Enabled = flag;
            orgRegainBonusFriendlyCapTextBox.Enabled = flag;
            convoyInterceptionMissionsComboBox.Enabled = flag;
            autoReturnTransportFleetsComboBox.Enabled = flag;
            allowProvinceRegionTargetingComboBox.Enabled = flag;
            nightHoursWinterTextBox.Enabled = flag;
            nightHoursSpringFallTextBox.Enabled = flag;
            nightHoursSummerTextBox.Enabled = flag;
            recalculateLandArrivalTimesTextBox.Enabled = flag;
            synchronizeArrivalTimePlayerTextBox.Enabled = flag;
            synchronizeArrivalTimeAiTextBox.Enabled = flag;
            recalculateArrivalTimesCombatComboBox.Enabled = flag;
            landSpeedModifierCombatTextBox.Enabled = flag;
            landSpeedModifierBombardmentTextBox.Enabled = flag;
            landSpeedModifierSupplyTextBox.Enabled = flag;
            landSpeedModifierOrgTextBox.Enabled = flag;
            landAirSpeedModifierFuelTextBox.Enabled = flag;
            defaultSpeedFuelTextBox.Enabled = flag;
            fleetSizeRangePenaltyRatioTextBox.Enabled = flag;
            fleetSizeRangePenaltyThretholdTextBox.Enabled = flag;
            fleetSizeRangePenaltyMaxTextBox.Enabled = flag;
            applyRangeLimitsAreasRegionsComboBox.Enabled = flag;
            radarBonusDetectionTextBox.Enabled = flag;
            bonusDetectionFriendlyTextBox.Enabled = flag;
            screensCapitalRatioModifierTextBox.Enabled = flag;
            chanceTargetNoOrgLandTextBox.Enabled = flag;
            screenCapitalShipsTargetingTextBox.Enabled = flag;
        }

        /// <summary>
        ///     戦闘5タブの編集項目を初期化する
        /// </summary>
        private void InitCombat5EditableItems()
        {
            // DH1.03以降固有項目
            bool flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            landChanceAvoidDefencesLeftLabel.Enabled = flag;
            airChanceAvoidDefencesLeftLabel.Enabled = flag;
            navalChanceAvoidDefencesLeftLabel.Enabled = flag;
            landChanceAvoidNoDefencesLabel.Enabled = flag;
            airChanceAvoidNoDefencesLabel.Enabled = flag;
            navalChanceAvoidNoDefencesLabel.Enabled = flag;
            bonusLeaderSkillPointLandLabel.Enabled = flag;
            bonusLeaderSkillPointAirLabel.Enabled = flag;
            bonusLeaderSkillPointNavalLabel.Enabled = flag;
            landMinOrgDamageLabel.Enabled = flag;
            landOrgDamageHardSoftEachLabel.Enabled = flag;
            landOrgDamageHardVsSoftLabel.Enabled = flag;
            landMinStrDamageLabel.Enabled = flag;
            landStrDamageHardSoftEachLabel.Enabled = flag;
            landStrDamageHardVsSoftLabel.Enabled = flag;
            airMinOrgDamageLabel.Enabled = flag;
            airAdditionalOrgDamageLabel.Enabled = flag;
            airMinStrDamageLabel.Enabled = flag;
            airAdditionalStrDamageLabel.Enabled = flag;
            airStrDamageEntrencedLabel.Enabled = flag;
            navalMinOrgDamageLabel.Enabled = flag;
            navalAdditionalOrgDamageLabel.Enabled = flag;
            navalMinStrDamageLabel.Enabled = flag;
            navalAdditionalStrDamageLabel.Enabled = flag;
            landOrgDamageLandUrbanLabel.Enabled = flag;
            landOrgDamageLandFortLabel.Enabled = flag;
            requiredLandFortSizeLabel.Enabled = flag;
            fleetPositioningDaytimeLabel.Enabled = flag;
            fleetPositioningLeaderSkillLabel.Enabled = flag;
            fleetPositioningFleetSizeLabel.Enabled = flag;
            fleetPositioningFleetCompositionLabel.Enabled = flag;
            landCoastalFortsDamageLabel.Enabled = flag;
            landCoastalFortsMaxDamageLabel.Enabled = flag;
            minSoftnessBrigadesLabel.Enabled = flag;
            autoRetreatOrgLabel.Enabled = flag;
            landOrgNavalTransportationLabel.Enabled = flag;
            maxLandDigLabel.Enabled = flag;
            digIncreaseDayLabel.Enabled = flag;
            breakthroughEncirclementMinSpeedLabel.Enabled = flag;
            breakthroughEncirclementMaxChanceLabel.Enabled = flag;
            breakthroughEncirclementChanceModifierLabel.Enabled = flag;
            combatEventDurationLabel.Enabled = flag;
            counterAttackOrgAttackerDhLabel.Enabled = flag;
            counterAttackStrAttackerDhLabel.Enabled = flag;
            counterAttackOrgDefenderDhLabel.Enabled = flag;
            counterAttackStrDefenderDhLabel.Enabled = flag;
            assaultOrgAttackerDhLabel.Enabled = flag;
            assaultStrAttackerDhLabel.Enabled = flag;
            assaultOrgDefenderDhLabel.Enabled = flag;
            assaultStrDefenderDhLabel.Enabled = flag;
            encirclementOrgAttackerDhLabel.Enabled = flag;
            encirclementStrAttackerDhLabel.Enabled = flag;
            encirclementOrgDefenderDhLabel.Enabled = flag;
            encirclementStrDefenderDhLabel.Enabled = flag;
            ambushOrgAttackerDhLabel.Enabled = flag;
            ambushStrAttackerDhLabel.Enabled = flag;
            ambushOrgDefenderDhLabel.Enabled = flag;
            ambushStrDefenderDhLabel.Enabled = flag;
            delayOrgAttackerDhLabel.Enabled = flag;
            delayStrAttackerDhLabel.Enabled = flag;
            delayOrgDefenderDhLabel.Enabled = flag;
            delayStrDefenderDhLabel.Enabled = flag;
            tacticalWithdrawOrgAttackerDhLabel.Enabled = flag;
            tacticalWithdrawStrAttackerDhLabel.Enabled = flag;
            tacticalWithdrawOrgDefenderDhLabel.Enabled = flag;
            tacticalWithdrawStrDefenderDhLabel.Enabled = flag;
            breakthroughOrgAttackerDhLabel.Enabled = flag;
            breakthroughStrAttackerDhLabel.Enabled = flag;
            breakthroughOrgDefenderDhLabel.Enabled = flag;
            breakthroughStrDefenderDhLabel.Enabled = flag;
            hqStrDamageBreakthroughLabel.Enabled = flag;
            combatModeLabel.Enabled = flag;

            landChanceAvoidDefencesLeftTextBox.Enabled = flag;
            airChanceAvoidDefencesLeftTextBox.Enabled = flag;
            navalChanceAvoidDefencesLeftTextBox.Enabled = flag;
            landChanceAvoidNoDefencesTextBox.Enabled = flag;
            airChanceAvoidNoDefencesTextBox.Enabled = flag;
            navalChanceAvoidNoDefencesTextBox.Enabled = flag;
            bonusLeaderSkillPointLandTextBox.Enabled = flag;
            bonusLeaderSkillPointAirTextBox.Enabled = flag;
            bonusLeaderSkillPointNavalTextBox.Enabled = flag;
            landMinOrgDamageTextBox.Enabled = flag;
            landOrgDamageHardSoftEachTextBox.Enabled = flag;
            landOrgDamageHardVsSoftTextBox.Enabled = flag;
            landMinStrDamageTextBox.Enabled = flag;
            landStrDamageHardSoftEachTextBox.Enabled = flag;
            landStrDamageHardVsSoftTextBox.Enabled = flag;
            airMinOrgDamageTextBox.Enabled = flag;
            airAdditionalOrgDamageTextBox.Enabled = flag;
            airMinStrDamageTextBox.Enabled = flag;
            airAdditionalStrDamageTextBox.Enabled = flag;
            airStrDamageEntrencedTextBox.Enabled = flag;
            navalMinOrgDamageTextBox.Enabled = flag;
            navalAdditionalOrgDamageTextBox.Enabled = flag;
            navalMinStrDamageTextBox.Enabled = flag;
            navalAdditionalStrDamageTextBox.Enabled = flag;
            landOrgDamageLandUrbanTextBox.Enabled = flag;
            landOrgDamageLandFortTextBox.Enabled = flag;
            requiredLandFortSizeTextBox.Enabled = flag;
            fleetPositioningDaytimeTextBox.Enabled = flag;
            fleetPositioningLeaderSkillTextBox.Enabled = flag;
            fleetPositioningFleetSizeTextBox.Enabled = flag;
            fleetPositioningFleetCompositionTextBox.Enabled = flag;
            landCoastalFortsDamageTextBox.Enabled = flag;
            landCoastalFortsMaxDamageTextBox.Enabled = flag;
            minSoftnessBrigadesTextBox.Enabled = flag;
            autoRetreatOrgTextBox.Enabled = flag;
            landOrgNavalTransportationTextBox.Enabled = flag;
            maxLandDigTextBox.Enabled = flag;
            digIncreaseDayTextBox.Enabled = flag;
            breakthroughEncirclementMinSpeedTextBox.Enabled = flag;
            breakthroughEncirclementMaxChanceTextBox.Enabled = flag;
            breakthroughEncirclementChanceModifierTextBox.Enabled = flag;
            combatEventDurationTextBox.Enabled = flag;
            counterAttackOrgAttackerDhTextBox.Enabled = flag;
            counterAttackStrAttackerDhTextBox.Enabled = flag;
            counterAttackOrgDefenderDhTextBox.Enabled = flag;
            counterAttackStrDefenderDhTextBox.Enabled = flag;
            assaultOrgAttackerDhTextBox.Enabled = flag;
            assaultStrAttackerDhTextBox.Enabled = flag;
            assaultOrgDefenderDhTextBox.Enabled = flag;
            assaultStrDefenderDhTextBox.Enabled = flag;
            encirclementOrgAttackerDhTextBox.Enabled = flag;
            encirclementStrAttackerDhTextBox.Enabled = flag;
            encirclementOrgDefenderDhTextBox.Enabled = flag;
            encirclementStrDefenderDhTextBox.Enabled = flag;
            ambushOrgAttackerDhTextBox.Enabled = flag;
            ambushStrAttackerDhTextBox.Enabled = flag;
            ambushOrgDefenderDhTextBox.Enabled = flag;
            ambushStrDefenderDhTextBox.Enabled = flag;
            delayOrgAttackerDhTextBox.Enabled = flag;
            delayStrAttackerDhTextBox.Enabled = flag;
            delayOrgDefenderDhTextBox.Enabled = flag;
            delayStrDefenderDhTextBox.Enabled = flag;
            tacticalWithdrawOrgAttackerDhTextBox.Enabled = flag;
            tacticalWithdrawStrAttackerDhTextBox.Enabled = flag;
            tacticalWithdrawOrgDefenderDhTextBox.Enabled = flag;
            tacticalWithdrawStrDefenderDhTextBox.Enabled = flag;
            breakthroughOrgAttackerDhTextBox.Enabled = flag;
            breakthroughStrAttackerDhTextBox.Enabled = flag;
            breakthroughOrgDefenderDhTextBox.Enabled = flag;
            breakthroughStrDefenderDhTextBox.Enabled = flag;
            hqStrDamageBreakthroughComboBox.Enabled = flag;
            combatModeComboBox.Enabled = flag;
        }

        /// <summary>
        ///     任務1タブの編集項目を初期化する
        /// </summary>
        private void InitMission1EditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            attackMissionLabel.Enabled = flag;
            attackStartingEfficiencyLabel.Enabled = flag;
            attackSpeedBonusLabel.Enabled = flag;
            rebaseMissionLabel.Enabled = flag;
            rebaseStartingEfficiencyLabel.Enabled = flag;
            rebaseChanceDetectedLabel.Enabled = flag;
            stratRedeployMissionLabel.Enabled = flag;
            stratRedeployStartingEfficiencyLabel.Enabled = flag;
            stratRedeployAddedValueLabel.Enabled = flag;
            stratRedeployDistanceMultiplierLabel.Enabled = flag;
            supportAttackMissionLabel.Enabled = flag;
            supportAttackStartingEfficiencyLabel.Enabled = flag;
            supportAttackSpeedBonusLabel.Enabled = flag;
            supportDefenseMissionLabel.Enabled = flag;
            supportDefenseStartingEfficiencyLabel.Enabled = flag;
            supportDefenseSpeedBonusLabel.Enabled = flag;
            reservesMissionLabel.Enabled = flag;
            reservesStartingEfficiencyLabel.Enabled = flag;
            reservesSpeedBonusLabel.Enabled = flag;
            antiPartisanDutyMissionLabel.Enabled = flag;
            antiPartisanDutyStartingEfficiencyLabel.Enabled = flag;
            antiPartisanDutySuppressionLabel.Enabled = flag;
            plannedDefenseMissionLabel.Enabled = flag;
            plannedDefenseStartingEfficiencyLabel.Enabled = flag;
            airSuperiorityMissionLabel.Enabled = flag;
            airSuperiorityStartingEfficiencyLabel.Enabled = flag;
            airSuperiorityDetectionLabel.Enabled = flag;
            airSuperiorityMinRequiredLabel.Enabled = flag;
            groundAttackMissionLabel.Enabled = flag;
            groundAttackStartingEfficiencyLabel.Enabled = flag;
            groundAttackOrgDamageLabel.Enabled = flag;
            groundAttackStrDamageLabel.Enabled = flag;
            interdictionMissionLabel.Enabled = flag;
            interdictionStartingEfficiencyLabel.Enabled = flag;
            interdictionOrgDamageLabel.Enabled = flag;
            interdictionStrDamageLabel.Enabled = flag;
            strategicBombardmentMissionLabel.Enabled = flag;
            strategicBombardmentStartingEfficiencyLabel.Enabled = flag;
            logisticalStrikeMissionLabel.Enabled = flag;
            logisticalStrikeStartingEfficiencyLabel.Enabled = flag;
            runwayCrateringMissionLabel.Enabled = flag;
            runwayCrateringStartingEfficiencyLabel.Enabled = flag;
            installationStrikeMissionLabel.Enabled = flag;
            installationStrikeStartingEfficiencyLabel.Enabled = flag;
            navalStrikeMissionLabel.Enabled = flag;
            navalStrikeStartingEfficiencyLabel.Enabled = flag;
            portStrikeMissionLabel.Enabled = flag;
            portStrikeStartingEfficiencyLabel.Enabled = flag;
            convoyAirRaidingMissionLabel.Enabled = flag;
            convoyAirRaidingStartingEfficiencyLabel.Enabled = flag;
            airSupplyMissionLabel.Enabled = flag;
            airSupplyStartingEfficiencyLabel.Enabled = flag;
            airborneAssaultMissionLabel.Enabled = flag;
            airborneAssaultStartingEfficiencyLabel.Enabled = flag;
            nukeMissionLabel.Enabled = flag;
            nukeStartingEfficiencyLabel.Enabled = flag;
            airScrambleMissionLabel.Enabled = flag;
            airScrambleStartingEfficiencyLabel.Enabled = flag;
            airScrambleDetectionLabel.Enabled = flag;
            airScrambleMinRequiredLabel.Enabled = flag;
            convoyRadingMissionLabel.Enabled = flag;
            convoyRadingStartingEfficiencyLabel.Enabled = flag;
            convoyRadingRangeModifierLabel.Enabled = flag;
            convoyRadingChanceDetectedLabel.Enabled = flag;
            aswMissionLabel.Enabled = flag;
            aswStartingEfficiencyLabel.Enabled = flag;
            navalInterdictionMissionLabel.Enabled = flag;
            navalInterdictionStartingEfficiencyLabel.Enabled = flag;
            shoreBombardmentMissionLabel.Enabled = flag;
            shoreBombardmentStartingEfficiencyLabel.Enabled = flag;
            shoreBombardmentModifierDhLabel.Enabled = flag;

            attackMissionComboBox.Enabled = flag;
            attackStartingEfficiencyTextBox.Enabled = flag;
            attackSpeedBonusTextBox.Enabled = flag;
            rebaseMissionComboBox.Enabled = flag;
            rebaseStartingEfficiencyTextBox.Enabled = flag;
            rebaseChanceDetectedTextBox.Enabled = flag;
            stratRedeployMissionComboBox.Enabled = flag;
            stratRedeployStartingEfficiencyTextBox.Enabled = flag;
            stratRedeployAddedValueTextBox.Enabled = flag;
            stratRedeployDistanceMultiplierTextBox.Enabled = flag;
            supportAttackMissionComboBox.Enabled = flag;
            supportAttackStartingEfficiencyTextBox.Enabled = flag;
            supportAttackSpeedBonusTextBox.Enabled = flag;
            supportDefenseMissionComboBox.Enabled = flag;
            supportDefenseStartingEfficiencyTextBox.Enabled = flag;
            supportDefenseSpeedBonusTextBox.Enabled = flag;
            reservesMissionComboBox.Enabled = flag;
            reservesStartingEfficiencyTextBox.Enabled = flag;
            reservesSpeedBonusTextBox.Enabled = flag;
            antiPartisanDutyMissionComboBox.Enabled = flag;
            antiPartisanDutyStartingEfficiencyTextBox.Enabled = flag;
            antiPartisanDutySuppressionTextBox.Enabled = flag;
            plannedDefenseMissionComboBox.Enabled = flag;
            plannedDefenseStartingEfficiencyTextBox.Enabled = flag;
            airSuperiorityMissionComboBox.Enabled = flag;
            airSuperiorityStartingEfficiencyTextBox.Enabled = flag;
            airSuperiorityDetectionTextBox.Enabled = flag;
            airSuperiorityMinRequiredTextBox.Enabled = flag;
            groundAttackMissionComboBox.Enabled = flag;
            groundAttackStartingEfficiencyTextBox.Enabled = flag;
            groundAttackOrgDamageTextBox.Enabled = flag;
            groundAttackStrDamageTextBox.Enabled = flag;
            interdictionMissionComboBox.Enabled = flag;
            interdictionStartingEfficiencyTextBox.Enabled = flag;
            interdictionOrgDamageTextBox.Enabled = flag;
            interdictionStrDamageTextBox.Enabled = flag;
            strategicBombardmentMissionComboBox.Enabled = flag;
            strategicBombardmentStartingEfficiencyTextBox.Enabled = flag;
            logisticalStrikeMissionComboBox.Enabled = flag;
            logisticalStrikeStartingEfficiencyTextBox.Enabled = flag;
            runwayCrateringMissionComboBox.Enabled = flag;
            runwayCrateringStartingEfficiencyTextBox.Enabled = flag;
            installationStrikeMissionComboBox.Enabled = flag;
            installationStrikeStartingEfficiencyTextBox.Enabled = flag;
            navalStrikeMissionComboBox.Enabled = flag;
            navalStrikeStartingEfficiencyTextBox.Enabled = flag;
            portStrikeMissionComboBox.Enabled = flag;
            portStrikeStartingEfficiencyTextBox.Enabled = flag;
            convoyAirRaidingMissionComboBox.Enabled = flag;
            convoyAirRaidingStartingEfficiencyTextBox.Enabled = flag;
            airSupplyMissionComboBox.Enabled = flag;
            airSupplyStartingEfficiencyTextBox.Enabled = flag;
            airborneAssaultMissionComboBox.Enabled = flag;
            airborneAssaultStartingEfficiencyTextBox.Enabled = flag;
            nukeMissionComboBox.Enabled = flag;
            nukeStartingEfficiencyTextBox.Enabled = flag;
            airScrambleMissionComboBox.Enabled = flag;
            airScrambleStartingEfficiencyTextBox.Enabled = flag;
            airScrambleDetectionTextBox.Enabled = flag;
            airScrambleMinRequiredTextBox.Enabled = flag;
            convoyRadingMissionComboBox.Enabled = flag;
            convoyRadingStartingEfficiencyTextBox.Enabled = flag;
            convoyRadingRangeModifierTextBox.Enabled = flag;
            convoyRadingChanceDetectedTextBox.Enabled = flag;
            aswMissionComboBox.Enabled = flag;
            aswStartingEfficiencyTextBox.Enabled = flag;
            navalInterdictionMissionComboBox.Enabled = flag;
            navalInterdictionStartingEfficiencyTextBox.Enabled = flag;
            shoreBombardmentMissionComboBox.Enabled = flag;
            shoreBombardmentStartingEfficiencyTextBox.Enabled = flag;
            shoreBombardmentModifierDhTextBox.Enabled = flag;
        }

        /// <summary>
        ///     任務2タブの編集項目を初期化する
        /// </summary>
        private void InitMission2EditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            amphibousAssaultMissionLabel.Enabled = flag;
            amphibousAssaultStartingEfficiencyLabel.Enabled = flag;
            seaTransportMissionLabel.Enabled = flag;
            seaTransportStartingEfficiencyLabel.Enabled = flag;
            seaTransportRangeModifierLabel.Enabled = flag;
            seaTransportChanceDetectedLabel.Enabled = flag;
            NavalCombatPatrolMissionLabel.Enabled = flag;
            NavalCombatPatrolStartingEfficiencyLabel.Enabled = flag;
            navalPortStrikeMissionLabel.Enabled = flag;
            navalPortStrikeStartingEfficiencyLabel.Enabled = flag;
            navalAirbaseStrikeMissionLabel.Enabled = flag;
            navalAirbaseStrikeStartingEfficiencyLabel.Enabled = flag;
            sneakMoveMissionLabel.Enabled = flag;
            sneakMoveStartingEfficiencyLabel.Enabled = flag;
            sneakMoveRangeModifierLabel.Enabled = flag;
            sneakMoveChanceDetectedLabel.Enabled = flag;
            navalScrambleMissionLabel.Enabled = flag;
            navalScrambleStartingEfficiencyLabel.Enabled = flag;
            navalScrambleSpeedBonusLabel.Enabled = flag;

            amphibousAssaultMissionComboBox.Enabled = flag;
            amphibousAssaultStartingEfficiencyTextBox.Enabled = flag;
            seaTransportMissionComboBox.Enabled = flag;
            seaTransportStartingEfficiencyTextBox.Enabled = flag;
            seaTransportRangeModifierTextBox.Enabled = flag;
            seaTransportChanceDetectedTextBox.Enabled = flag;
            NavalCombatPatrolMissionComboBox.Enabled = flag;
            NavalCombatPatrolStartingEfficiencyTextBox.Enabled = flag;
            navalPortStrikeMissionComboBox.Enabled = flag;
            navalPortStrikeStartingEfficiencyTextBox.Enabled = flag;
            navalAirbaseStrikeMissionComboBox.Enabled = flag;
            navalAirbaseStrikeStartingEfficiencyTextBox.Enabled = flag;
            sneakMoveMissionComboBox.Enabled = flag;
            sneakMoveStartingEfficiencyTextBox.Enabled = flag;
            sneakMoveRangeModifierTextBox.Enabled = flag;
            sneakMoveChanceDetectedTextBox.Enabled = flag;
            navalScrambleMissionComboBox.Enabled = flag;
            navalScrambleStartingEfficiencyTextBox.Enabled = flag;
            navalScrambleSpeedBonusTextBox.Enabled = flag;

            // DH1.03以降固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            useAttackEfficiencyCombatModifierLabel.Enabled = flag;

            useAttackEfficiencyCombatModifierComboBox.Enabled = flag;
        }

        /// <summary>
        ///     国家タブの編集項目を初期化する
        /// </summary>
        private void InitCountryEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            landFortEfficiencyLabel.Enabled = flag;
            coastalFortEfficiencyLabel.Enabled = flag;
            groundDefenseEfficiencyLabel.Enabled = flag;
            convoyDefenseEfficiencyLabel.Enabled = flag;
            manpowerBoostLabel.Enabled = flag;
            transportCapacityModifierLabel.Enabled = flag;
            occupiedTransportCapacityModifierLabel.Enabled = flag;
            attritionModifierLabel.Enabled = flag;
            manpowerTrickleBackModifierLabel.Enabled = flag;
            supplyDistanceModifierLabel.Enabled = flag;
            repairModifierLabel.Enabled = flag;
            researchModifierLabel.Enabled = flag;
            radarEfficiencyLabel.Enabled = flag;
            hqSupplyEfficiencyBonusLabel.Enabled = flag;
            hqCombatEventsBonusLabel.Enabled = flag;
            combatEventChancesLabel.Enabled = flag;
            friendlyArmyDetectionChanceLabel.Enabled = flag;
            enemyArmyDetectionChanceLabel.Enabled = flag;
            friendlyIntelligenceChanceLabel.Enabled = flag;
            enemyIntelligenceChanceLabel.Enabled = flag;
            maxAmphibiousArmySizeLabel.Enabled = flag;
            energyToOilLabel.Enabled = flag;
            totalProductionEfficiencyLabel.Enabled = flag;
            supplyProductionEfficiencyLabel.Enabled = flag;
            aaPowerLabel.Enabled = flag;
            airSurpriseChanceLabel.Enabled = flag;
            landSurpriseChanceLabel.Enabled = flag;
            navalSurpriseChanceLabel.Enabled = flag;
            peacetimeIcModifierLabel.Enabled = flag;
            wartimeIcModifierLabel.Enabled = flag;
            buildingsProductionModifierLabel.Enabled = flag;
            convoysProductionModifierLabel.Enabled = flag;
            minShipsPositioningBattleLabel.Enabled = flag;
            maxShipsPositioningBattleLabel.Enabled = flag;
            peacetimeStockpilesResourcesLabel.Enabled = flag;
            wartimeStockpilesResourcesLabel.Enabled = flag;
            peacetimeStockpilesOilSuppliesLabel.Enabled = flag;
            wartimeStockpilesOilSuppliesLabel.Enabled = flag;

            landFortEfficiencyTextBox.Enabled = flag;
            coastalFortEfficiencyTextBox.Enabled = flag;
            groundDefenseEfficiencyTextBox.Enabled = flag;
            convoyDefenseEfficiencyTextBox.Enabled = flag;
            manpowerBoostTextBox.Enabled = flag;
            transportCapacityModifierTextBox.Enabled = flag;
            occupiedTransportCapacityModifierTextBox.Enabled = flag;
            attritionModifierTextBox.Enabled = flag;
            manpowerTrickleBackModifierTextBox.Enabled = flag;
            supplyDistanceModifierTextBox.Enabled = flag;
            repairModifierTextBox.Enabled = flag;
            researchModifierTextBox.Enabled = flag;
            radarEfficiencyTextBox.Enabled = flag;
            hqSupplyEfficiencyBonusTextBox.Enabled = flag;
            hqCombatEventsBonusTextBox.Enabled = flag;
            combatEventChancesTextBox.Enabled = flag;
            friendlyArmyDetectionChanceTextBox.Enabled = flag;
            enemyArmyDetectionChanceTextBox.Enabled = flag;
            friendlyIntelligenceChanceTextBox.Enabled = flag;
            enemyIntelligenceChanceTextBox.Enabled = flag;
            maxAmphibiousArmySizeTextBox.Enabled = flag;
            energyToOilTextBox.Enabled = flag;
            totalProductionEfficiencyTextBox.Enabled = flag;
            supplyProductionEfficiencyTextBox.Enabled = flag;
            aaPowerTextBox.Enabled = flag;
            airSurpriseChanceTextBox.Enabled = flag;
            landSurpriseChanceTextBox.Enabled = flag;
            navalSurpriseChanceTextBox.Enabled = flag;
            peacetimeIcModifierTextBox.Enabled = flag;
            wartimeIcModifierTextBox.Enabled = flag;
            buildingsProductionModifierTextBox.Enabled = flag;
            convoysProductionModifierTextBox.Enabled = flag;
            minShipsPositioningBattleTextBox.Enabled = flag;
            maxShipsPositioningBattleTextBox.Enabled = flag;
            peacetimeStockpilesResourcesTextBox.Enabled = flag;
            wartimeStockpilesResourcesTextBox.Enabled = flag;
            peacetimeStockpilesOilSuppliesTextBox.Enabled = flag;
            wartimeStockpilesOilSuppliesTextBox.Enabled = flag;
        }

        /// <summary>
        ///     研究タブの編集項目を初期化する
        /// </summary>
        private void InitResearchEditableItems()
        {
            // AoD固有項目
            bool flag = (Game.Type == GameType.ArsenalOfDemocracy);

            postHistoricalDateModifierAoDLabel.Enabled = flag;
            techSpeedModifierLabel.Enabled = flag;
            preHistoricalPenaltyLimitLabel.Enabled = flag;
            postHistoricalBonusLimitLabel.Enabled = flag;

            postHistoricalDateModifierAoDTextBox.Enabled = flag;
            techSpeedModifierTextBox.Enabled = flag;
            preHistoricalPenaltyLimitTextBox.Enabled = flag;
            postHistoricalBonusLimitTextBox.Enabled = flag;

            // AoD1.07以降固有項目
            flag = (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107);

            maxActiveTechTeamsAoDLabel.Enabled = flag;
            requiredIcEachTechTeamAoDLabel.Enabled = flag;

            maxActiveTechTeamsAoDTextBox.Enabled = flag;
            requiredIcEachTechTeamAoDTextBox.Enabled = flag;

            // AoD1.08以降固有項目
            flag = (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108);

            maximumRandomModifierLabel.Enabled = flag;

            maximumRandomModifierTextBox.Enabled = flag;

            // DH固有項目
            flag = (Game.Type == GameType.DarkestHour);

            postHistoricalDateModifierDhLabel.Enabled = flag;
            useNewTechnologyPageLayoutLabel.Enabled = flag;
            maxActiveTechTeamsDhLabel.Enabled = flag;
            minActiveTechTeamsLabel.Enabled = flag;
            requiredIcEachTechTeamDhLabel.Enabled = flag;

            postHistoricalDateModifierDhTextBox.Enabled = flag;
            useNewTechnologyPageLayoutComboBox.Enabled = flag;
            maxActiveTechTeamsDhTextBox.Enabled = flag;
            minActiveTechTeamsTextBox.Enabled = flag;
            requiredIcEachTechTeamDhTextBox.Enabled = flag;

            // DH1.03以降固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            techOverviewPanelStyleLabel.Enabled = flag;
            newCountryRocketryComponentLabel.Enabled = flag;
            newCountryNuclearPhysicsComponentLabel.Enabled = flag;
            newCountryNuclearEngineeringComponentLabel.Enabled = flag;
            newCountrySecretTechsLabel.Enabled = flag;
            maxTechTeamSkillLabel.Enabled = flag;

            techOverviewPanelStyleComboBox.Enabled = flag;
            newCountryRocketryComponentComboBox.Enabled = flag;
            newCountryNuclearPhysicsComponentComboBox.Enabled = flag;
            newCountryNuclearEngineeringComponentComboBox.Enabled = flag;
            newCountrySecretTechsComboBox.Enabled = flag;
            maxTechTeamSkillTextBox.Enabled = flag;
        }

        /// <summary>
        ///     貿易タブの編集項目を初期化する
        /// </summary>
        private void InitTradeEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            daysTradeOffersLabel.Enabled = flag;
            delayGameStartNewTradesLabel.Enabled = flag;
            limitAiNewTradesGameStartLabel.Enabled = flag;
            desiredOilStockpileLabel.Enabled = flag;
            criticalOilStockpileLabel.Enabled = flag;
            desiredSuppliesStockpileLabel.Enabled = flag;
            criticalSuppliesStockpileLabel.Enabled = flag;
            desiredResourcesStockpileLabel.Enabled = flag;
            criticalResourceStockpileLabel.Enabled = flag;
            wartimeDesiredStockpileMultiplierLabel.Enabled = flag;
            peacetimeExtraOilImportLabel.Enabled = flag;
            wartimeExtraOilImportLabel.Enabled = flag;
            extraImportBelowDesiredLabel.Enabled = flag;
            percentageProducedSuppliesLabel.Enabled = flag;
            percentageProducedMoneyLabel.Enabled = flag;
            extraImportStockpileSelectedLabel.Enabled = flag;
            daysDeliverResourcesTradesLabel.Enabled = flag;
            mergeTradeDealsLabel.Enabled = flag;
            manualTradeDealsLabel.Enabled = flag;
            puppetsSendSuppliesMoneyLabel.Enabled = flag;
            puppetsCriticalSupplyStockpileLabel.Enabled = flag;
            puppetsMaxPoolResourcesLabel.Enabled = flag;
            newTradeDealsMinEffectivenessLabel.Enabled = flag;
            cancelTradeDealsEffectivenessLabel.Enabled = flag;
            autoTradeAiTradeDealsLabel.Enabled = flag;

            daysTradeOffersTextBox.Enabled = flag;
            delayGameStartNewTradesTextBox.Enabled = flag;
            limitAiNewTradesGameStartTextBox.Enabled = flag;
            desiredOilStockpileTextBox.Enabled = flag;
            criticalOilStockpileTextBox.Enabled = flag;
            desiredSuppliesStockpileTextBox.Enabled = flag;
            criticalSuppliesStockpileTextBox.Enabled = flag;
            desiredResourcesStockpileTextBox.Enabled = flag;
            criticalResourceStockpileTextBox.Enabled = flag;
            wartimeDesiredStockpileMultiplierTextBox.Enabled = flag;
            peacetimeExtraOilImportTextBox.Enabled = flag;
            wartimeExtraOilImportTextBox.Enabled = flag;
            extraImportBelowDesiredTextBox.Enabled = flag;
            percentageProducedSuppliesTextBox.Enabled = flag;
            percentageProducedMoneyTextBox.Enabled = flag;
            extraImportStockpileSelectedTextBox.Enabled = flag;
            daysDeliverResourcesTradesTextBox.Enabled = flag;
            mergeTradeDealsComboBox.Enabled = flag;
            manualTradeDealsComboBox.Enabled = flag;
            puppetsSendSuppliesMoneyTextBox.Enabled = flag;
            puppetsCriticalSupplyStockpileTextBox.Enabled = flag;
            puppetsMaxPoolResourcesTextBox.Enabled = flag;
            newTradeDealsMinEffectivenessTextBox.Enabled = flag;
            cancelTradeDealsEffectivenessTextBox.Enabled = flag;
            autoTradeAiTradeDealsTextBox.Enabled = flag;
        }

        /// <summary>
        ///     AIタブの編集項目を初期化する
        /// </summary>
        private void InitAiEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            overproduceSuppliesBelowDesiredLabel.Enabled = flag;
            multiplierOverproduceSuppliesWarLabel.Enabled = flag;
            notProduceSuppliesStockpileOverLabel.Enabled = flag;
            maxSerialLineProductionGarrisonMilitiaLabel.Enabled = flag;
            minIcSerialProductionNavalAirLabel.Enabled = flag;
            notProduceNewUnitsManpowerRatioLabel.Enabled = flag;
            notProduceNewUnitsManpowerValueLabel.Enabled = flag;
            notProduceNewUnitsSupplyLabel.Enabled = flag;
            militaryStrengthTotalIcRatioPeacetimeLabel.Enabled = flag;
            militaryStrengthTotalIcRatioWartimeLabel.Enabled = flag;
            militaryStrengthTotalIcRatioMajorLabel.Enabled = flag;
            notUseOffensiveSupplyStockpileLabel.Enabled = flag;
            notUseOffensiveOilStockpileLabel.Enabled = flag;
            notUseOffensiveEseLabel.Enabled = flag;
            notUseOffensiveOrgStrDamageLabel.Enabled = flag;
            aiPeacetimeSpyMissionsDhLabel.Enabled = flag;
            aiSpyMissionsCostModifierDhLabel.Enabled = flag;
            aiDiplomacyCostModifierDhLabel.Enabled = flag;
            aiInfluenceModifierDhLabel.Enabled = flag;
            forcePuppetsJoinMastersAllianceNeutralityLabel.Enabled = flag;
            newAiReleaseRulesLabel.Enabled = flag;
            aiEventsActionSelectionRulesLabel.Enabled = flag;
            forceStrategicRedeploymentHourLabel.Enabled = flag;
            maxRedeploymentDaysAiLabel.Enabled = flag;
            useQuickAreaCheckGarrisonAiLabel.Enabled = flag;
            aiMastersGetProvincesConquredPuppetsLabel.Enabled = flag;
            minDaysRequiredAiReleaseCountryLabel.Enabled = flag;
            minDaysRequiredAiAlliedLabel.Enabled = flag;
            minDaysRequiredAiAlliedSupplyBaseLabel.Enabled = flag;
            minRequiredRelationsAlliedClaimedLabel.Enabled = flag;

            overproduceSuppliesBelowDesiredTextBox.Enabled = flag;
            multiplierOverproduceSuppliesWarTextBox.Enabled = flag;
            notProduceSuppliesStockpileOverTextBox.Enabled = flag;
            maxSerialLineProductionGarrisonMilitiaTextBox.Enabled = flag;
            minIcSerialProductionNavalAirTextBox.Enabled = flag;
            notProduceNewUnitsManpowerRatioTextBox.Enabled = flag;
            notProduceNewUnitsManpowerValueTextBox.Enabled = flag;
            notProduceNewUnitsSupplyTextBox.Enabled = flag;
            militaryStrengthTotalIcRatioPeacetimeTextBox.Enabled = flag;
            militaryStrengthTotalIcRatioWartimeTextBox.Enabled = flag;
            militaryStrengthTotalIcRatioMajorTextBox.Enabled = flag;
            notUseOffensiveSupplyStockpileTextBox.Enabled = flag;
            notUseOffensiveOilStockpileTextBox.Enabled = flag;
            notUseOffensiveEseTextBox.Enabled = flag;
            notUseOffensiveOrgStrDamageTextBox.Enabled = flag;
            aiPeacetimeSpyMissionsDhComboBox.Enabled = flag;
            aiSpyMissionsCostModifierDhTextBox.Enabled = flag;
            aiDiplomacyCostModifierDhTextBox.Enabled = flag;
            aiInfluenceModifierDhTextBox.Enabled = flag;
            forcePuppetsJoinMastersAllianceNeutralityTextBox.Enabled = flag;
            newAiReleaseRulesComboBox.Enabled = flag;
            aiEventsActionSelectionRulesTextBox.Enabled = flag;
            forceStrategicRedeploymentHourTextBox.Enabled = flag;
            maxRedeploymentDaysAiTextBox.Enabled = flag;
            useQuickAreaCheckGarrisonAiComboBox.Enabled = flag;
            aiMastersGetProvincesConquredPuppetsComboBox.Enabled = flag;
            minDaysRequiredAiReleaseCountryTextBox.Enabled = flag;
            minDaysRequiredAiAlliedTextBox.Enabled = flag;
            minDaysRequiredAiAlliedSupplyBaseTextBox.Enabled = flag;
            minRequiredRelationsAlliedClaimedTextBox.Enabled = flag;

            // DH1.02以前固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version <= 102);

            newDowRulesLabel.Enabled = flag;

            newDowRulesComboBox.Enabled = flag;

            // DH1.03以降固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            newDowRules2Label.Enabled = flag;

            newDowRules2ComboBox.Enabled = flag;
        }

        /// <summary>
        ///     MODタブの編集項目を初期化する
        /// </summary>
        private void InitModEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            aiSpyDiplomaticMissionLoggerLabel.Enabled = flag;
            countryLoggerLabel.Enabled = flag;
            switchedAiFilesLoggerLabel.Enabled = flag;
            useNewAutoSaveFileFormatLabel.Enabled = flag;
            loadNewAiSwitchingAllClientsLabel.Enabled = flag;
            tradeEfficiencyCalculationSystemLabel.Enabled = flag;
            mergeRelocateProvincialDepotsLabel.Enabled = flag;
            allowBrigadeAttachingInSupplyLabel.Enabled = flag;
            multipleDeploymentSizeArmiesLabel.Enabled = flag;
            multipleDeploymentSizeFleetsLabel.Enabled = flag;
            multipleDeploymentSizeAirLabel.Enabled = flag;
            allowUniquePicturesAllLandProvincesLabel.Enabled = flag;
            autoReplyEventsLabel.Enabled = flag;
            forceActionsShowLabel.Enabled = flag;
            enableDicisionsPlayersLabel.Enabled = flag;
            rebelsArmyCompositionLabel.Enabled = flag;
            rebelsArmyTechLevelLabel.Enabled = flag;
            rebelsArmyMinStrLabel.Enabled = flag;
            rebelsArmyMaxStrLabel.Enabled = flag;
            rebelsOrgRegainLabel.Enabled = flag;
            extraRebelBonusNeighboringProvinceLabel.Enabled = flag;
            extraRebelBonusOccupiedLabel.Enabled = flag;
            extraRebelBonusMountainLabel.Enabled = flag;
            extraRebelBonusHillLabel.Enabled = flag;
            extraRebelBonusForestLabel.Enabled = flag;
            extraRebelBonusJungleLabel.Enabled = flag;
            extraRebelBonusSwampLabel.Enabled = flag;
            extraRebelBonusDesertLabel.Enabled = flag;
            extraRebelBonusPlainLabel.Enabled = flag;
            extraRebelBonusUrbanLabel.Enabled = flag;
            extraRebelBonusAirNavalBasesLabel.Enabled = flag;
            returnRebelliousProvinceLabel.Enabled = flag;
            useNewMinisterFilesFormatLabel.Enabled = flag;
            loadSpritesModdirOnlyLabel.Enabled = flag;
            loadUnitIconsModdirOnlyLabel.Enabled = flag;
            loadUnitPicturesModdirOnlyLabel.Enabled = flag;
            loadAiFilesModdirOnlyLabel.Enabled = flag;
            useSpeedSetGarrisonStatusLabel.Enabled = flag;
            useOldSaveGameFormatLabel.Enabled = flag;

            aiSpyDiplomaticMissionLoggerComboBox.Enabled = flag;
            countryLoggerTextBox.Enabled = flag;
            switchedAiFilesLoggerComboBox.Enabled = flag;
            useNewAutoSaveFileFormatComboBox.Enabled = flag;
            loadNewAiSwitchingAllClientsComboBox.Enabled = flag;
            tradeEfficiencyCalculationSystemTextBox.Enabled = flag;
            mergeRelocateProvincialDepotsTextBox.Enabled = flag;
            allowBrigadeAttachingInSupplyComboBox.Enabled = flag;
            multipleDeploymentSizeArmiesTextBox.Enabled = flag;
            multipleDeploymentSizeFleetsTextBox.Enabled = flag;
            multipleDeploymentSizeAirTextBox.Enabled = flag;
            allowUniquePicturesAllLandProvincesComboBox.Enabled = flag;
            autoReplyEventsComboBox.Enabled = flag;
            forceActionsShowComboBox.Enabled = flag;
            enableDicisionsPlayersComboBox.Enabled = flag;
            rebelsArmyCompositionTextBox.Enabled = flag;
            rebelsArmyTechLevelTextBox.Enabled = flag;
            rebelsArmyMinStrTextBox.Enabled = flag;
            rebelsArmyMaxStrTextBox.Enabled = flag;
            rebelsOrgRegainTextBox.Enabled = flag;
            extraRebelBonusNeighboringProvinceTextBox.Enabled = flag;
            extraRebelBonusOccupiedTextBox.Enabled = flag;
            extraRebelBonusMountainTextBox.Enabled = flag;
            extraRebelBonusHillTextBox.Enabled = flag;
            extraRebelBonusForestTextBox.Enabled = flag;
            extraRebelBonusJungleTextBox.Enabled = flag;
            extraRebelBonusSwampTextBox.Enabled = flag;
            extraRebelBonusDesertTextBox.Enabled = flag;
            extraRebelBonusPlainTextBox.Enabled = flag;
            extraRebelBonusUrbanTextBox.Enabled = flag;
            extraRebelBonusAirNavalBasesTextBox.Enabled = flag;
            returnRebelliousProvinceTextBox.Enabled = flag;
            useNewMinisterFilesFormatComboBox.Enabled = flag;
            loadSpritesModdirOnlyComboBox.Enabled = flag;
            loadUnitIconsModdirOnlyComboBox.Enabled = flag;
            loadUnitPicturesModdirOnlyComboBox.Enabled = flag;
            loadAiFilesModdirOnlyComboBox.Enabled = flag;
            useSpeedSetGarrisonStatusComboBox.Enabled = flag;
            useOldSaveGameFormatComboBox.Enabled = flag;

            // DH1.02以前固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version <= 102);

            inGameLossesLoggingLabel.Enabled = flag;

            inGameLossesLoggingComboBox.Enabled = flag;

            // DH1.03以降固有項目
            flag = (Game.Type == GameType.DarkestHour && Game.Version >= 103);

            inGameLossLogging2Label.Enabled = flag;
            enableRetirementYearMinistersLabel.Enabled = flag;
            enableRetirementYearLeadersLabel.Enabled = flag;
            productionPanelUiStyleLabel.Enabled = flag;
            unitPicturesSizeLabel.Enabled = flag;
            enablePicturesNavalBrigadesLabel.Enabled = flag;
            buildingsBuildableOnlyProvincesLabel.Enabled = flag;
            unitModifiersStatisticsPagesLabel.Enabled = flag;

            inGameLossLogging2ComboBox.Enabled = flag;
            enableRetirementYearMinistersComboBox.Enabled = flag;
            enableRetirementYearLeadersComboBox.Enabled = flag;
            productionPanelUiStyleComboBox.Enabled = flag;
            unitPicturesSizeComboBox.Enabled = flag;
            enablePicturesNavalBrigadesComboBox.Enabled = flag;
            buildingsBuildableOnlyProvincesComboBox.Enabled = flag;
            unitModifiersStatisticsPagesTextBox.Enabled = flag;
        }

        /// <summary>
        ///     マップタブの編集項目を初期化する
        /// </summary>
        private void InitMapEditableItems()
        {
            // DH固有項目
            bool flag = (Game.Type == GameType.DarkestHour);

            mapNumberLabel.Enabled = flag;
            totalProvincesLabel.Enabled = flag;
            distanceCalculationModelLabel.Enabled = flag;
            MapWidthLabel.Enabled = flag;
            MapHeightLabel.Enabled = flag;

            mapNumberTextBox.Enabled = flag;
            totalProvincesTextBox.Enabled = flag;
            distanceCalculationModelComboBox.Enabled = flag;
            MapWidthTextBox.Enabled = flag;
            MapHeightTextBox.Enabled = flag;
        }

        /// <summary>
        ///     編集項目の値を更新する
        /// </summary>
        private void UpdateEditableItemsValue()
        {
            UpdateEconomy1ItemValue();
            UpdateEconomy2ItemValue();
            UpdateEconomy3ItemValue();
            UpdateIntelligenceItemValue();
            UpdateDiplomacyItemValue();
            UpdateCombat1ItemValue();
            UpdateCombat2ItemValue();
            UpdateCombat3ItemValue();
            UpdateCombat4ItemValue();
            UpdateCombat5ItemValue();
            UpdateMission1ItemValue();
            UpdateMission2ItemValue();
            UpdateCountryItemValue();
            UpdateResearchItemValue();
            UpdateTradeItemValue();
            UpdateAiItemValue();
            UpdateModItemValue();
            UpdateMapItemValue();
        }

        /// <summary>
        ///     経済1タブの編集項目の値を更新する
        /// </summary>
        private void UpdateEconomy1ItemValue()
        {
            icToTcRatioTextBox.Text = Misc.GetItem(MiscItemId.IcToTcRatio).ToString();
            icToSuppliesRatioTextBox.Text = Misc.GetItem(MiscItemId.IcToSuppliesRatio).ToString();
            icToConsumerGoodsRatioTextBox.Text = Misc.GetItem(MiscItemId.IcToConsumerGoodsRatio).ToString();
            icToMoneyRatioTextBox.Text = Misc.GetItem(MiscItemId.IcToMoneyRatio).ToString();
            maxGearingBonusTextBox.Text = Misc.GetItem(MiscItemId.MaxGearingBonus).ToString();
            gearingBonusIncrementTextBox.Text = Misc.GetItem(MiscItemId.GearingBonusIncrement).ToString();
            icMultiplierNonNationalTextBox.Text = Misc.GetItem(MiscItemId.IcMultiplierNonNational).ToString();
            icMultiplierNonOwnedTextBox.Text = Misc.GetItem(MiscItemId.IcMultiplierNonOwned).ToString();
            tcLoadUndeployedDivisionTextBox.Text = Misc.GetItem(MiscItemId.TcLoadUndeployedDivision).ToString();
            tcLoadOccupiedTextBox.Text = Misc.GetItem(MiscItemId.TcLoadOccupied).ToString();
            tcLoadMultiplierLandTextBox.Text = Misc.GetItem(MiscItemId.TcLoadMultiplierLand).ToString();
            tcLoadMultiplierAirTextBox.Text = Misc.GetItem(MiscItemId.TcLoadMultiplierAir).ToString();
            tcLoadMultiplierNavalTextBox.Text = Misc.GetItem(MiscItemId.TcLoadMultiplierNaval).ToString();
            tcLoadPartisanTextBox.Text = Misc.GetItem(MiscItemId.TcLoadPartisan).ToString();
            tcLoadFactorOffensiveTextBox.Text = Misc.GetItem(MiscItemId.TcLoadFactorOffensive).ToString();
            tcLoadProvinceDevelopmentTextBox.Text = Misc.GetItem(MiscItemId.TcLoadProvinceDevelopment).ToString();
            tcLoadBaseTextBox.Text = Misc.GetItem(MiscItemId.TcLoadBase).ToString();
            manpowerMultiplierNationalTextBox.Text = Misc.GetItem(MiscItemId.ManpowerMultiplierNational).ToString();
            manpowerMultiplierNonNationalTextBox.Text =
                Misc.GetItem(MiscItemId.ManpowerMultiplierNonNational).ToString();
            manpowerMultiplierColonyTextBox.Text = Misc.GetItem(MiscItemId.ManpowerMultiplierColony).ToString();
            trickleBackFactorManpowerTextBox.Text = Misc.GetItem(MiscItemId.TrickleBackFactorManpower).ToString();
            reinforceManpowerTextBox.Text = Misc.GetItem(MiscItemId.ReinforceManpower).ToString();
            reinforceCostTextBox.Text = Misc.GetItem(MiscItemId.ReinforceCost).ToString();
            reinforceTimeTextBox.Text = Misc.GetItem(MiscItemId.ReinforceTime).ToString();
            upgradeCostTextBox.Text = Misc.GetItem(MiscItemId.UpgradeCost).ToString();
            upgradeTimeTextBox.Text = Misc.GetItem(MiscItemId.UpgradeTime).ToString();
            nationalismStartingValueTextBox.Text = Misc.GetItem(MiscItemId.NationalismStartingValue).ToString();
            monthlyNationalismReductionTextBox.Text = Misc.GetItem(MiscItemId.MonthlyNationalismReduction).ToString();
            sendDivisionDaysTextBox.Text = Misc.GetItem(MiscItemId.SendDivisionDays).ToString();
            tcLoadUndeployedBrigadeTextBox.Text = Misc.GetItem(MiscItemId.TcLoadUndeployedBrigade).ToString();

            // DDA1.3以降固有項目
            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                spyMissionDaysTextBox.Text = Misc.GetItem(MiscItemId.SpyMissionDays).ToString();
                increateIntelligenceLevelDaysTextBox.Text =
                    Misc.GetItem(MiscItemId.IncreateIntelligenceLevelDays).ToString();
                chanceDetectSpyMissionTextBox.Text = Misc.GetItem(MiscItemId.ChanceDetectSpyMission).ToString();
                relationshipsHitDetectedMissionsTextBox.Text =
                    Misc.GetItem(MiscItemId.RelationshipsHitDetectedMissions).ToString();
                showThirdCountrySpyReportsComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.ShowThirdCountrySpyReports);
                distanceModifierNeighboursTextBox.Text = Misc.GetItem(MiscItemId.DistanceModifierNeighbours).ToString();
                spyInformationAccuracyModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.SpyInformationAccuracyModifier).ToString();
                aiPeacetimeSpyMissionsComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.AiPeacetimeSpyMissions);
                maxIcCostModifierTextBox.Text = Misc.GetItem(MiscItemId.MaxIcCostModifier).ToString();
                aiSpyMissionsCostModifierTextBox.Text = Misc.GetItem(MiscItemId.AiSpyMissionsCostModifier).ToString();
                aiDiplomacyCostModifierTextBox.Text = Misc.GetItem(MiscItemId.AiDiplomacyCostModifier).ToString();
                aiInfluenceModifierTextBox.Text = Misc.GetItem(MiscItemId.AiInfluenceModifier).ToString();
            }

            // AoD1.07以上固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                nationalismPerManpowerAoDTextBox.Text = Misc.GetItem(MiscItemId.NationalismPerManpowerAoD).ToString();
                coreProvinceEfficiencyRiseTimeTextBox.Text =
                    Misc.GetItem(MiscItemId.CoreProvinceEfficiencyRiseTime).ToString();
                restockSpeedLandTextBox.Text = Misc.GetItem(MiscItemId.RestockSpeedLand).ToString();
                restockSpeedAirTextBox.Text = Misc.GetItem(MiscItemId.RestockSpeedAir).ToString();
                restockSpeedNavalTextBox.Text = Misc.GetItem(MiscItemId.RestockSpeedNaval).ToString();
                spyCoupDissentModifierTextBox.Text = Misc.GetItem(MiscItemId.SpyCoupDissentModifier).ToString();
                convoyDutyConversionTextBox.Text = Misc.GetItem(MiscItemId.ConvoyDutyConversion).ToString();
                escortDutyConversionTextBox.Text = Misc.GetItem(MiscItemId.EscortDutyConversion).ToString();
                tpMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.TpMaxAttach).ToString();
                ssMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.SsMaxAttach).ToString();
                ssnMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.SsnMaxAttach).ToString();
                ddMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.DdMaxAttach).ToString();
                clMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.ClMaxAttach).ToString();
                caMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.CaMaxAttach).ToString();
                bcMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.BcMaxAttach).ToString();
                bbMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.BbMaxAttach).ToString();
                cvlMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.CvlMaxAttach).ToString();
                cvMaxAttachTextBox.Text = Misc.GetItem(MiscItemId.CvMaxAttach).ToString();
                canChangeIdeasComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.CanChangeIdeas) ? 1 : 0;
            }

            // DDA/AoD固有項目
            if (Game.Type == GameType.HeartsOfIron2 || Game.Type == GameType.ArsenalOfDemocracy)
            {
                requirementAffectSliderTextBox.Text = Misc.GetItem(MiscItemId.RequirementAffectSlider).ToString();
                canUnitSendNonAlliedComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.CanUnitSendNonAllied)
                                                                 ? 1
                                                                 : 0;
            }
        }

        /// <summary>
        ///     経済2タブの編集項目の値を更新する
        /// </summary>
        private void UpdateEconomy2ItemValue()
        {
            // Aod固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                dissentChangeSpeedTextBox.Text = Misc.GetItem(MiscItemId.DissentChangeSpeed).ToString();
                gearingResourceIncrementTextBox.Text = Misc.GetItem(MiscItemId.GearingResourceIncrement).ToString();
                gearingLossNoIcTextBox.Text = Misc.GetItem(MiscItemId.GearingLossNoIc).ToString();
                costRepairBuildingsTextBox.Text = Misc.GetItem(MiscItemId.CostRepairBuildings).ToString();
                timeRepairBuildingTextBox.Text = Misc.GetItem(MiscItemId.TimeRepairBuilding).ToString();
                provinceEfficiencyRiseTimeTextBox.Text = Misc.GetItem(MiscItemId.ProvinceEfficiencyRiseTime).ToString();
                lineUpkeepTextBox.Text = Misc.GetItem(MiscItemId.LineUpkeep).ToString();
                lineStartupTimeTextBox.Text = Misc.GetItem(MiscItemId.LineStartupTime).ToString();
                lineUpgradeTimeTextBox.Text = Misc.GetItem(MiscItemId.LineUpgradeTime).ToString();
                retoolingCostTextBox.Text = Misc.GetItem(MiscItemId.RetoolingCost).ToString();
                retoolingResourceTextBox.Text = Misc.GetItem(MiscItemId.RetoolingResource).ToString();
                dailyAgingManpowerTextBox.Text = Misc.GetItem(MiscItemId.DailyAgingManpower).ToString();
                supplyConvoyHuntTextBox.Text = Misc.GetItem(MiscItemId.SupplyConvoyHunt).ToString();
                supplyNavalStaticAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyNavalStaticAoD).ToString();
                supplyNavalMovingTextBox.Text = Misc.GetItem(MiscItemId.SupplyNavalMoving).ToString();
                supplyNavalBattleAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyNavalBattleAoD).ToString();
                supplyAirStaticAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirStaticAoD).ToString();
                supplyAirMovingTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirMoving).ToString();
                supplyAirBattleAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirBattleAoD).ToString();
                supplyAirBombingTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirBombing).ToString();
                supplyLandStaticAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyLandStaticAoD).ToString();
                supplyLandMovingTextBox.Text = Misc.GetItem(MiscItemId.SupplyLandMoving).ToString();
                supplyLandBattleAoDTextBox.Text = Misc.GetItem(MiscItemId.SupplyLandBattleAoD).ToString();
                supplyLandBombingTextBox.Text = Misc.GetItem(MiscItemId.SupplyLandBombing).ToString();
                supplyStockLandTextBox.Text = Misc.GetItem(MiscItemId.SupplyStockLand).ToString();
                supplyStockAirTextBox.Text = Misc.GetItem(MiscItemId.SupplyStockAir).ToString();
                supplyStockNavalTextBox.Text = Misc.GetItem(MiscItemId.SupplyStockNaval).ToString();
                syntheticOilConversionMultiplierTextBox.Text =
                    Misc.GetItem(MiscItemId.SyntheticOilConversionMultiplier).ToString();
                syntheticRaresConversionMultiplierTextBox.Text =
                    Misc.GetItem(MiscItemId.SyntheticRaresConversionMultiplier).ToString();
                militarySalaryTextBox.Text = Misc.GetItem(MiscItemId.MilitarySalary).ToString();
                maxIntelligenceExpenditureTextBox.Text = Misc.GetItem(MiscItemId.MaxIntelligenceExpenditure).ToString();
                maxResearchExpenditureTextBox.Text = Misc.GetItem(MiscItemId.MaxResearchExpenditure).ToString();
                militarySalaryAttrictionModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitarySalaryAttrictionModifier).ToString();
                militarySalaryDissentModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitarySalaryDissentModifier).ToString();
                nuclearSiteUpkeepCostTextBox.Text = Misc.GetItem(MiscItemId.NuclearSiteUpkeepCost).ToString();
                nuclearPowerUpkeepCostTextBox.Text = Misc.GetItem(MiscItemId.NuclearPowerUpkeepCost).ToString();
                syntheticOilSiteUpkeepCostTextBox.Text = Misc.GetItem(MiscItemId.SyntheticOilSiteUpkeepCost).ToString();
                syntheticRaresSiteUpkeepCostTextBox.Text =
                    Misc.GetItem(MiscItemId.SyntheticRaresSiteUpkeepCost).ToString();
                durationDetectionTextBox.Text = Misc.GetItem(MiscItemId.DurationDetection).ToString();
                convoyProvinceHostileTimeTextBox.Text = Misc.GetItem(MiscItemId.ConvoyProvinceHostileTime).ToString();
                convoyProvinceBlockedTimeTextBox.Text = Misc.GetItem(MiscItemId.ConvoyProvinceBlockedTime).ToString();
                autoTradeConvoyTextBox.Text = Misc.GetItem(MiscItemId.AutoTradeConvoy).ToString();
                spyUpkeepCostTextBox.Text = Misc.GetItem(MiscItemId.SpyUpkeepCost).ToString();
                spyDetectionChanceTextBox.Text = Misc.GetItem(MiscItemId.SpyDetectionChance).ToString();
                infraEfficiencyModifierTextBox.Text = Misc.GetItem(MiscItemId.InfraEfficiencyModifier).ToString();
                manpowerToConsumerGoodsTextBox.Text = Misc.GetItem(MiscItemId.ManpowerToConsumerGoods).ToString();
                timeBetweenSliderChangesAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.TimeBetweenSliderChangesAoD).ToString();
                minimalPlacementIcTextBox.Text = Misc.GetItem(MiscItemId.MinimalPlacementIc).ToString();
                nuclearPowerTextBox.Text = Misc.GetItem(MiscItemId.NuclearPower).ToString();
                freeInfraRepairTextBox.Text = Misc.GetItem(MiscItemId.FreeInfraRepair).ToString();
                maxSliderDissentTextBox.Text = Misc.GetItem(MiscItemId.MaxSliderDissent).ToString();
                minSliderDissentTextBox.Text = Misc.GetItem(MiscItemId.MinSliderDissent).ToString();
                maxDissentSliderMoveTextBox.Text = Misc.GetItem(MiscItemId.MaxDissentSliderMove).ToString();
                icConcentrationBonusTextBox.Text = Misc.GetItem(MiscItemId.IcConcentrationBonus).ToString();
                ministerChangeDelayTextBox.Text = Misc.GetItem(MiscItemId.MinisterChangeDelay).ToString();
                ministerChangeEventDelayTextBox.Text = Misc.GetItem(MiscItemId.MinisterChangeEventDelay).ToString();
                ideaChangeDelayTextBox.Text = Misc.GetItem(MiscItemId.IdeaChangeDelay).ToString();
                ideaChangeEventDelayTextBox.Text = Misc.GetItem(MiscItemId.IdeaChangeEventDelay).ToString();
                leaderChangeDelayTextBox.Text = Misc.GetItem(MiscItemId.LeaderChangeDelay).ToString();
                changeIdeaDissentTextBox.Text = Misc.GetItem(MiscItemId.ChangeIdeaDissent).ToString();
                changeMinisterDissentTextBox.Text = Misc.GetItem(MiscItemId.ChangeMinisterDissent).ToString();
                minDissentRevoltTextBox.Text = Misc.GetItem(MiscItemId.MinDissentRevolt).ToString();
                dissentRevoltMultiplierTextBox.Text = Misc.GetItem(MiscItemId.DissentRevoltMultiplier).ToString();
            }

            // AoD1.04以前固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version <= 104)
            {
                transportConversionTextBox.Text = Misc.GetItem(MiscItemId.TransportConversion).ToString();
            }
        }

        /// <summary>
        ///     経済3タブの編集項目の値を更新する
        /// </summary>
        private void UpdateEconomy3ItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                minAvailableIcTextBox.Text = Misc.GetItem(MiscItemId.MinAvailableIc).ToString();
                minFinalIcTextBox.Text = Misc.GetItem(MiscItemId.MinFinalIc).ToString();
                dissentReductionTextBox.Text = Misc.GetItem(MiscItemId.DissentReduction).ToString();
                icMultiplierPuppetTextBox.Text = Misc.GetItem(MiscItemId.IcMultiplierPuppet).ToString();
                resourceMultiplierNonNationalTextBox.Text =
                    Misc.GetItem(MiscItemId.ResourceMultiplierNonNational).ToString();
                resourceMultiplierNonOwnedTextBox.Text = Misc.GetItem(MiscItemId.ResourceMultiplierNonOwned).ToString();
                resourceMultiplierNonNationalAiTextBox.Text =
                    Misc.GetItem(MiscItemId.ResourceMultiplierNonNationalAi).ToString();
                resourceMultiplierPuppetTextBox.Text = Misc.GetItem(MiscItemId.ResourceMultiplierPuppet).ToString();
                manpowerMultiplierPuppetTextBox.Text = Misc.GetItem(MiscItemId.ManpowerMultiplierPuppet).ToString();
                manpowerMultiplierWartimeOverseaTextBox.Text =
                    Misc.GetItem(MiscItemId.ManpowerMultiplierWartimeOversea).ToString();
                manpowerMultiplierPeacetimeTextBox.Text =
                    Misc.GetItem(MiscItemId.ManpowerMultiplierPeacetime).ToString();
                manpowerMultiplierWartimeTextBox.Text = Misc.GetItem(MiscItemId.ManpowerMultiplierWartime).ToString();
                dailyRetiredManpowerTextBox.Text = Misc.GetItem(MiscItemId.DailyRetiredManpower).ToString();
                reinforceToUpdateModifierTextBox.Text = Misc.GetItem(MiscItemId.ReinforceToUpdateModifier).ToString();
                nationalismPerManpowerDhTextBox.Text = Misc.GetItem(MiscItemId.NationalismPerManpowerDh).ToString();
                maxNationalismTextBox.Text = Misc.GetItem(MiscItemId.MaxNationalism).ToString();
                maxRevoltRiskTextBox.Text = Misc.GetItem(MiscItemId.MaxRevoltRisk).ToString();
                canUnitSendNonAlliedDhComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.CanUnitSendNonAlliedDh);
                bluePrintsCanSoldNonAlliedComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.BluePrintsCanSoldNonAllied);
                provinceCanSoldNonAlliedComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.ProvinceCanSoldNonAllied);
                transferAlliedCoreProvincesComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.TransferAlliedCoreProvinces) ? 1 : 0;
                provinceBuildingsRepairModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.ProvinceBuildingsRepairModifier).ToString();
                provinceResourceRepairModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.ProvinceResourceRepairModifier).ToString();
                stockpileLimitMultiplierResourceTextBox.Text =
                    Misc.GetItem(MiscItemId.StockpileLimitMultiplierResource).ToString();
                stockpileLimitMultiplierSuppliesOilTextBox.Text =
                    Misc.GetItem(MiscItemId.StockpileLimitMultiplierSuppliesOil).ToString();
                overStockpileLimitDailyLossTextBox.Text =
                    Misc.GetItem(MiscItemId.OverStockpileLimitDailyLoss).ToString();
                maxResourceDepotSizeTextBox.Text = Misc.GetItem(MiscItemId.MaxResourceDepotSize).ToString();
                maxSuppliesOilDepotSizeTextBox.Text = Misc.GetItem(MiscItemId.MaxSuppliesOilDepotSize).ToString();
                desiredStockPilesSuppliesOilTextBox.Text =
                    Misc.GetItem(MiscItemId.DesiredStockPilesSuppliesOil).ToString();
                maxManpowerTextBox.Text = Misc.GetItem(MiscItemId.MaxManpower).ToString();
                convoyTransportsCapacityTextBox.Text = Misc.GetItem(MiscItemId.ConvoyTransportsCapacity).ToString();
                suppyLandStaticDhTextBox.Text = Misc.GetItem(MiscItemId.SuppyLandStaticDh).ToString();
                supplyLandBattleDhTextBox.Text = Misc.GetItem(MiscItemId.SupplyLandBattleDh).ToString();
                fuelLandStaticTextBox.Text = Misc.GetItem(MiscItemId.FuelLandStatic).ToString();
                fuelLandBattleTextBox.Text = Misc.GetItem(MiscItemId.FuelLandBattle).ToString();
                supplyAirStaticDhTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirStaticDh).ToString();
                supplyAirBattleDhTextBox.Text = Misc.GetItem(MiscItemId.SupplyAirBattleDh).ToString();
                fuelAirNavalStaticTextBox.Text = Misc.GetItem(MiscItemId.FuelAirNavalStatic).ToString();
                fuelAirBattleTextBox.Text = Misc.GetItem(MiscItemId.FuelAirBattle).ToString();
                supplyNavalStaticDhTextBox.Text = Misc.GetItem(MiscItemId.SupplyNavalStaticDh).ToString();
                supplyNavalBattleDhTextBox.Text = Misc.GetItem(MiscItemId.SupplyNavalBattleDh).ToString();
                fuelNavalNotMovingTextBox.Text = Misc.GetItem(MiscItemId.FuelNavalNotMoving).ToString();
                fuelNavalBattleTextBox.Text = Misc.GetItem(MiscItemId.FuelNavalBattle).ToString();
                tpTransportsConversionRatioTextBox.Text =
                    Misc.GetItem(MiscItemId.TpTransportsConversionRatio).ToString();
                ddEscortsConversionRatioTextBox.Text = Misc.GetItem(MiscItemId.DdEscortsConversionRatio).ToString();
                clEscortsConversionRatioTextBox.Text = Misc.GetItem(MiscItemId.ClEscortsConversionRatio).ToString();
                cvlEscortsConversionRatioTextBox.Text = Misc.GetItem(MiscItemId.CvlEscortsConversionRatio).ToString();
                productionLineEditComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ProductionLineEdit) ? 1 : 0;
                gearingBonusLossUpgradeUnitTextBox.Text =
                    Misc.GetItem(MiscItemId.GearingBonusLossUpgradeUnit).ToString();
                gearingBonusLossUpgradeBrigadeTextBox.Text =
                    Misc.GetItem(MiscItemId.GearingBonusLossUpgradeBrigade).ToString();
                dissentNukesTextBox.Text = Misc.GetItem(MiscItemId.DissentNukes).ToString();
                maxDailyDissentTextBox.Text = Misc.GetItem(MiscItemId.MaxDailyDissent).ToString();
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                nukesProductionModifierTextBox.Text = Misc.GetItem(MiscItemId.NukesProductionModifier).ToString();
                convoySystemOptionsAlliedComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.ConvoySystemOptionsAllied);
                resourceConvoysBackUnneededTextBox.Text =
                    Misc.GetItem(MiscItemId.ResourceConvoysBackUnneeded).ToString();
            }
        }

        /// <summary>
        ///     諜報タブの編集項目の値を更新する
        /// </summary>
        private void UpdateIntelligenceItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                spyMissionDaysDhTextBox.Text = Misc.GetItem(MiscItemId.SpyMissionDaysDh).ToString();
                increateIntelligenceLevelDaysDhTextBox.Text =
                    Misc.GetItem(MiscItemId.IncreateIntelligenceLevelDaysDh).ToString();
                chanceDetectSpyMissionDhTextBox.Text = Misc.GetItem(MiscItemId.ChanceDetectSpyMissionDh).ToString();
                relationshipsHitDetectedMissionsDhTextBox.Text =
                    Misc.GetItem(MiscItemId.RelationshipsHitDetectedMissionsDh).ToString();
                distanceModifierTextBox.Text = Misc.GetItem(MiscItemId.DistanceModifier).ToString();
                distanceModifierNeighboursDhTextBox.Text =
                    Misc.GetItem(MiscItemId.DistanceModifierNeighboursDh).ToString();
                spyLevelBonusDistanceModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.SpyLevelBonusDistanceModifier).ToString();
                spyLevelBonusDistanceModifierAboveTenTextBox.Text =
                    Misc.GetItem(MiscItemId.SpyLevelBonusDistanceModifierAboveTen).ToString();
                spyInformationAccuracyModifierDhTextBox.Text =
                    Misc.GetItem(MiscItemId.SpyInformationAccuracyModifierDh).ToString();
                icModifierCostTextBox.Text = Misc.GetItem(MiscItemId.IcModifierCost).ToString();
                minIcCostModifierTextBox.Text = Misc.GetItem(MiscItemId.MinIcCostModifier).ToString();
                maxIcCostModifierDhTextBox.Text = Misc.GetItem(MiscItemId.MaxIcCostModifierDh).ToString();
                extraMaintenanceCostAboveTenTextBox.Text =
                    Misc.GetItem(MiscItemId.ExtraMaintenanceCostAboveTen).ToString();
                extraCostIncreasingAboveTenTextBox.Text =
                    Misc.GetItem(MiscItemId.ExtraCostIncreasingAboveTen).ToString();
                showThirdCountrySpyReportsDhComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.ShowThirdCountrySpyReportsDh);
                spiesMoneyModifierTextBox.Text = Misc.GetItem(MiscItemId.SpiesMoneyModifier).ToString();
            }
        }

        /// <summary>
        ///     外交タブの編集項目の値を更新する
        /// </summary>
        private void UpdateDiplomacyItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                daysBetweenDiplomaticMissionsTextBox.Text =
                    Misc.GetItem(MiscItemId.DaysBetweenDiplomaticMissions).ToString();
                timeBetweenSliderChangesDhTextBox.Text = Misc.GetItem(MiscItemId.TimeBetweenSliderChangesDh).ToString();
                requirementAffectSliderDhTextBox.Text = Misc.GetItem(MiscItemId.RequirementAffectSliderDh).ToString();
                useMinisterPersonalityReplacingComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.UseMinisterPersonalityReplacing) ? 1 : 0;
                relationshipHitCancelTradeTextBox.Text = Misc.GetItem(MiscItemId.RelationshipHitCancelTrade).ToString();
                relationshipHitCancelPermanentTradeTextBox.Text =
                    Misc.GetItem(MiscItemId.RelationshipHitCancelPermanentTrade).ToString();
                puppetsJoinMastersAllianceComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.PuppetsJoinMastersAlliance) ? 1 : 0;
                mastersBecomePuppetsPuppetsComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.MastersBecomePuppetsPuppets) ? 1 : 0;
                allowManualClaimsChangeComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AllowManualClaimsChange)
                                                                    ? 1
                                                                    : 0;
                belligerenceClaimedProvinceTextBox.Text =
                    Misc.GetItem(MiscItemId.BelligerenceClaimedProvince).ToString();
                belligerenceClaimsRemovalTextBox.Text = Misc.GetItem(MiscItemId.BelligerenceClaimsRemoval).ToString();
                joinAutomaticallyAllesAxisComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.JoinAutomaticallyAllesAxis) ? 1 : 0;
                allowChangeHosHogComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.AllowChangeHosHog);
                changeTagCoupComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ChangeTagCoup) ? 1 : 0;
                filterReleaseCountriesComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.FilterReleaseCountries);
            }
        }

        /// <summary>
        ///     戦闘1タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCombat1ItemValue()
        {
            landXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.LandXpGainFactor).ToString();
            navalXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.NavalXpGainFactor).ToString();
            airXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.AirXpGainFactor).ToString();
            divisionXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.DivisionXpGainFactor).ToString();
            leaderXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.LeaderXpGainFactor).ToString();
            attritionSeverityModifierTextBox.Text = Misc.GetItem(MiscItemId.AttritionSeverityModifier).ToString();
            baseProximityTextBox.Text = Misc.GetItem(MiscItemId.BaseProximity).ToString();
            invasionModifierTextBox.Text = Misc.GetItem(MiscItemId.InvasionModifier).ToString();
            multipleCombatModifierTextBox.Text = Misc.GetItem(MiscItemId.MultipleCombatModifier).ToString();
            offensiveCombinedArmsBonusTextBox.Text = Misc.GetItem(MiscItemId.OffensiveCombinedArmsBonus).ToString();
            defensiveCombinedArmsBonusTextBox.Text = Misc.GetItem(MiscItemId.DefensiveCombinedArmsBonus).ToString();
            surpriseModifierTextBox.Text = Misc.GetItem(MiscItemId.SurpriseModifier).ToString();
            landCommandLimitModifierTextBox.Text = Misc.GetItem(MiscItemId.LandCommandLimitModifier).ToString();
            airCommandLimitModifierTextBox.Text = Misc.GetItem(MiscItemId.AirCommandLimitModifier).ToString();
            navalCommandLimitModifierTextBox.Text = Misc.GetItem(MiscItemId.NavalCommandLimitModifier).ToString();
            envelopmentModifierTextBox.Text = Misc.GetItem(MiscItemId.EnvelopmentModifier).ToString();
            encircledModifierTextBox.Text = Misc.GetItem(MiscItemId.EncircledModifier).ToString();
            landFortMultiplierTextBox.Text = Misc.GetItem(MiscItemId.LandFortMultiplier).ToString();
            coastalFortMultiplierTextBox.Text = Misc.GetItem(MiscItemId.CoastalFortMultiplier).ToString();
            dissentMultiplierTextBox.Text = Misc.GetItem(MiscItemId.DissentMultiplier).ToString();
            raderStationMultiplierTextBox.Text = Misc.GetItem(MiscItemId.RaderStationMultiplier).ToString();
            interceptorBomberModifierTextBox.Text = Misc.GetItem(MiscItemId.InterceptorBomberModifier).ToString();
            navalOverstackingModifierTextBox.Text = Misc.GetItem(MiscItemId.NavalOverstackingModifier).ToString();
            landLeaderCommandLimitRank0TextBox.Text = Misc.GetItem(MiscItemId.LandLeaderCommandLimitRank0).ToString();
            landLeaderCommandLimitRank1TextBox.Text = Misc.GetItem(MiscItemId.LandLeaderCommandLimitRank1).ToString();
            landLeaderCommandLimitRank2TextBox.Text = Misc.GetItem(MiscItemId.LandLeaderCommandLimitRank2).ToString();
            landLeaderCommandLimitRank3TextBox.Text = Misc.GetItem(MiscItemId.LandLeaderCommandLimitRank3).ToString();
            airLeaderCommandLimitRank0TextBox.Text = Misc.GetItem(MiscItemId.AirLeaderCommandLimitRank0).ToString();
            airLeaderCommandLimitRank1TextBox.Text = Misc.GetItem(MiscItemId.AirLeaderCommandLimitRank1).ToString();
            airLeaderCommandLimitRank2TextBox.Text = Misc.GetItem(MiscItemId.AirLeaderCommandLimitRank2).ToString();
            airLeaderCommandLimitRank3TextBox.Text = Misc.GetItem(MiscItemId.AirLeaderCommandLimitRank3).ToString();
            navalLeaderCommandLimitRank0TextBox.Text = Misc.GetItem(MiscItemId.NavalLeaderCommandLimitRank0).ToString();
            navalLeaderCommandLimitRank1TextBox.Text = Misc.GetItem(MiscItemId.NavalLeaderCommandLimitRank1).ToString();
            navalLeaderCommandLimitRank2TextBox.Text = Misc.GetItem(MiscItemId.NavalLeaderCommandLimitRank2).ToString();
            navalLeaderCommandLimitRank3TextBox.Text = Misc.GetItem(MiscItemId.NavalLeaderCommandLimitRank3).ToString();
            hqCommandLimitFactorTextBox.Text = Misc.GetItem(MiscItemId.HqCommandLimitFactor).ToString();
            convoyProtectionFactorTextBox.Text = Misc.GetItem(MiscItemId.ConvoyProtectionFactor).ToString();
            delayAfterCombatEndsTextBox.Text = Misc.GetItem(MiscItemId.DelayAfterCombatEnds).ToString();
            maximumSizesAirStacksTextBox.Text = Misc.GetItem(MiscItemId.MaximumSizesAirStacks).ToString();
            effectExperienceCombatTextBox.Text = Misc.GetItem(MiscItemId.EffectExperienceCombat).ToString();
            damageNavalBasesBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageNavalBasesBombing).ToString();
            damageAirBaseBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageAirBaseBombing).ToString();
            damageAaBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageAaBombing).ToString();
            damageRocketBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageRocketBombing).ToString();
            damageNukeBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageNukeBombing).ToString();
            damageRadarBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageRadarBombing).ToString();
            damageInfraBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageInfraBombing).ToString();
            damageIcBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageIcBombing).ToString();
            damageResourcesBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageResourcesBombing).ToString();
            chanceGetTerrainTraitTextBox.Text = Misc.GetItem(MiscItemId.ChanceGetTerrainTrait).ToString();
            chanceGetEventTraitTextBox.Text = Misc.GetItem(MiscItemId.ChanceGetEventTrait).ToString();
            bonusTerrainTraitTextBox.Text = Misc.GetItem(MiscItemId.BonusTerrainTrait).ToString();
            bonusEventTraitTextBox.Text = Misc.GetItem(MiscItemId.BonusEventTrait).ToString();
            chanceLeaderDyingTextBox.Text = Misc.GetItem(MiscItemId.ChanceLeaderDying).ToString();

            // AoDに存在しない項目
            if (Game.Type != GameType.ArsenalOfDemocracy)
            {
                airOverstackingModifierTextBox.Text = Misc.GetItem(MiscItemId.AirOverstackingModifier).ToString();
            }

            // DHに存在しない項目
            if (Game.Type != GameType.DarkestHour)
            {
                shoreBombardmentModifierTextBox.Text = Misc.GetItem(MiscItemId.ShoreBombardmentModifier).ToString();
                supplyProblemsModifierTextBox.Text = Misc.GetItem(MiscItemId.SupplyProblemsModifier).ToString();
                airOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamage).ToString();
            }

            // DH1.03以降に存在しない項目
            if (Game.Type != GameType.DarkestHour || Game.Version < 103)
            {
                howEffectiveGroundDefTextBox.Text = Misc.GetItem(MiscItemId.HowEffectiveGroundDef).ToString();
                chanceAvoidDefencesLeftTextBox.Text = Misc.GetItem(MiscItemId.ChanceAvoidDefencesLeft).ToString();
                chanceAvoidNoDefencesTextBox.Text = Misc.GetItem(MiscItemId.ChanceAvoidNoDefences).ToString();
            }

            // AODにもDHにも存在しない項目
            if (Game.Type != GameType.ArsenalOfDemocracy && Game.Type != GameType.DarkestHour)
            {
                airStrDamageOrgTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageOrg).ToString();
                airStrDamageTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamage).ToString();
            }

            // DDA1.3以降固有項目
            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                subsOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.SubsOrgDamage).ToString();
                subsStrDamageTextBox.Text = Misc.GetItem(MiscItemId.SubsStrDamage).ToString();
            }

            // DDA1.3以降またはDH固有項目
            if ((Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130) || Game.Type == GameType.DarkestHour)
            {
                subStacksDetectionModifierTextBox.Text = Misc.GetItem(MiscItemId.SubStacksDetectionModifier).ToString();
            }
        }

        /// <summary>
        ///     戦闘2タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCombat2ItemValue()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                noSupplyAttritionSeverityTextBox.Text = Misc.GetItem(MiscItemId.NoSupplyAttritionSeverity).ToString();
                noSupplyMinimunAttritionTextBox.Text = Misc.GetItem(MiscItemId.NoSupplyMinimunAttrition).ToString();
                raderStationAaMultiplierTextBox.Text = Misc.GetItem(MiscItemId.RaderStationAaMultiplier).ToString();
                airOverstackingModifierAoDTextBox.Text = Misc.GetItem(MiscItemId.AirOverstackingModifierAoD).ToString();
                landDelayBeforeOrdersTextBox.Text = Misc.GetItem(MiscItemId.LandDelayBeforeOrders).ToString();
                navalDelayBeforeOrdersTextBox.Text = Misc.GetItem(MiscItemId.NavalDelayBeforeOrders).ToString();
                airDelayBeforeOrdersTextBox.Text = Misc.GetItem(MiscItemId.AirDelayBeforeOrders).ToString();
                damageSyntheticOilBombingTextBox.Text = Misc.GetItem(MiscItemId.DamageSyntheticOilBombing).ToString();
                airOrgDamageLandAoDTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageLandAoD).ToString();
                airStrDamageLandAoDTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageLandAoD).ToString();
                landDamageArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.LandDamageArtilleryBombardment).ToString();
                infraDamageArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.InfraDamageArtilleryBombardment).ToString();
                icDamageArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.IcDamageArtilleryBombardment).ToString();
                resourcesDamageArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.ResourcesDamageArtilleryBombardment).ToString();
                penaltyArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.PenaltyArtilleryBombardment).ToString();
                artilleryStrDamageTextBox.Text = Misc.GetItem(MiscItemId.ArtilleryStrDamage).ToString();
                artilleryOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.ArtilleryOrgDamage).ToString();
                landStrDamageLandAoDTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageLandAoD).ToString();
                landOrgDamageLandTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageLand).ToString();
                landStrDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageAirAoD).ToString();
                landOrgDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageAirAoD).ToString();
                navalStrDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.NavalStrDamageAirAoD).ToString();
                navalOrgDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgDamageAirAoD).ToString();
                airStrDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageAirAoD).ToString();
                airOrgDamageAirAoDTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageAirAoD).ToString();
                navalStrDamageNavyAoDTextBox.Text = Misc.GetItem(MiscItemId.NavalStrDamageNavyAoD).ToString();
                navalOrgDamageNavyAoDTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgDamageNavyAoD).ToString();
                airStrDamageNavyAoDTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageNavyAoD).ToString();
                airOrgDamageNavyAoDTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageNavyAoD).ToString();
                militaryExpenseAttritionModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitaryExpenseAttritionModifier).ToString();
                navalMinCombatTimeTextBox.Text = Misc.GetItem(MiscItemId.NavalMinCombatTime).ToString();
                landMinCombatTimeTextBox.Text = Misc.GetItem(MiscItemId.LandMinCombatTime).ToString();
                airMinCombatTimeTextBox.Text = Misc.GetItem(MiscItemId.AirMinCombatTime).ToString();
                landOverstackingModifierTextBox.Text = Misc.GetItem(MiscItemId.LandOverstackingModifier).ToString();
                landOrgLossMovingTextBox.Text = Misc.GetItem(MiscItemId.LandOrgLossMoving).ToString();
                airOrgLossMovingTextBox.Text = Misc.GetItem(MiscItemId.AirOrgLossMoving).ToString();
                navalOrgLossMovingTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgLossMoving).ToString();
                supplyDistanceSeverityTextBox.Text = Misc.GetItem(MiscItemId.SupplyDistanceSeverity).ToString();
                supplyBaseTextBox.Text = Misc.GetItem(MiscItemId.SupplyBase).ToString();
                landOrgGainTextBox.Text = Misc.GetItem(MiscItemId.LandOrgGain).ToString();
                airOrgGainTextBox.Text = Misc.GetItem(MiscItemId.AirOrgGain).ToString();
                navalOrgGainTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgGain).ToString();
                nukeManpowerDissentTextBox.Text = Misc.GetItem(MiscItemId.NukeManpowerDissent).ToString();
                nukeIcDissentTextBox.Text = Misc.GetItem(MiscItemId.NukeIcDissent).ToString();
                nukeTotalDissentTextBox.Text = Misc.GetItem(MiscItemId.NukeTotalDissent).ToString();
                landFriendlyOrgGainTextBox.Text = Misc.GetItem(MiscItemId.LandFriendlyOrgGain).ToString();
                airLandStockModifierTextBox.Text = Misc.GetItem(MiscItemId.AirLandStockModifier).ToString();
                scorchDamageTextBox.Text = Misc.GetItem(MiscItemId.ScorchDamage).ToString();
                standGroundDissentTextBox.Text = Misc.GetItem(MiscItemId.StandGroundDissent).ToString();
                scorchGroundBelligerenceTextBox.Text = Misc.GetItem(MiscItemId.ScorchGroundBelligerence).ToString();
                defaultLandStackTextBox.Text = Misc.GetItem(MiscItemId.DefaultLandStack).ToString();
                defaultNavalStackTextBox.Text = Misc.GetItem(MiscItemId.DefaultNavalStack).ToString();
                defaultAirStackTextBox.Text = Misc.GetItem(MiscItemId.DefaultAirStack).ToString();
                defaultRocketStackTextBox.Text = Misc.GetItem(MiscItemId.DefaultRocketStack).ToString();
                fortDamageArtilleryBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.FortDamageArtilleryBombardment).ToString();
                artilleryBombardmentOrgCostTextBox.Text =
                    Misc.GetItem(MiscItemId.ArtilleryBombardmentOrgCost).ToString();
                landDamageFortTextBox.Text = Misc.GetItem(MiscItemId.LandDamageFort).ToString();
                airRebaseFactorTextBox.Text = Misc.GetItem(MiscItemId.AirRebaseFactor).ToString();
                airMaxDisorganizedTextBox.Text = Misc.GetItem(MiscItemId.AirMaxDisorganized).ToString();
                aaInflictedStrDamageTextBox.Text = Misc.GetItem(MiscItemId.AaInflictedStrDamage).ToString();
                aaInflictedOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.AaInflictedOrgDamage).ToString();
                aaInflictedFlyingDamageTextBox.Text = Misc.GetItem(MiscItemId.AaInflictedFlyingDamage).ToString();
                aaInflictedBombingDamageTextBox.Text = Misc.GetItem(MiscItemId.AaInflictedBombingDamage).ToString();
                hardAttackStrDamageTextBox.Text = Misc.GetItem(MiscItemId.HardAttackStrDamage).ToString();
                hardAttackOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.HardAttackOrgDamage).ToString();
                armorSoftBreakthroughMinTextBox.Text = Misc.GetItem(MiscItemId.ArmorSoftBreakthroughMin).ToString();
                armorSoftBreakthroughMaxTextBox.Text = Misc.GetItem(MiscItemId.ArmorSoftBreakthroughMax).ToString();
                navalCriticalHitChanceTextBox.Text = Misc.GetItem(MiscItemId.NavalCriticalHitChance).ToString();
                navalCriticalHitEffectTextBox.Text = Misc.GetItem(MiscItemId.NavalCriticalHitEffect).ToString();
                landFortDamageTextBox.Text = Misc.GetItem(MiscItemId.LandFortDamage).ToString();
                portAttackSurpriseChanceDayTextBox.Text =
                    Misc.GetItem(MiscItemId.PortAttackSurpriseChanceDay).ToString();
                portAttackSurpriseChanceNightTextBox.Text =
                    Misc.GetItem(MiscItemId.PortAttackSurpriseChanceNight).ToString();
            }
        }

        /// <summary>
        ///     戦闘3タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCombat3ItemValue()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                portAttackSurpriseModifierTextBox.Text = Misc.GetItem(MiscItemId.PortAttackSurpriseModifier).ToString();
                radarAntiSurpriseChanceTextBox.Text = Misc.GetItem(MiscItemId.RadarAntiSurpriseChance).ToString();
                radarAntiSurpriseModifierTextBox.Text = Misc.GetItem(MiscItemId.RadarAntiSurpriseModifier).ToString();
            }

            // AoD1.08以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108)
            {
                shoreBombardmentCapTextBox.Text = Misc.GetItem(MiscItemId.ShoreBombardmentCap).ToString();
                counterAttackStrDefenderAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.CounterAttackStrDefenderAoD).ToString();
                counterAttackOrgDefenderAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.CounterAttackOrgDefenderAoD).ToString();
                counterAttackStrAttackerAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.CounterAttackStrAttackerAoD).ToString();
                counterAttackOrgAttackerAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.CounterAttackOrgAttackerAoD).ToString();
                assaultStrDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.AssaultStrDefenderAoD).ToString();
                assaultOrgDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.AssaultOrgDefenderAoD).ToString();
                assaultStrAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.AssaultStrAttackerAoD).ToString();
                assaultOrgAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.AssaultOrgAttackerAoD).ToString();
                encirclementStrDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.EncirclementStrDefenderAoD).ToString();
                encirclementOrgDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.EncirclementOrgDefenderAoD).ToString();
                encirclementStrAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.EncirclementStrAttackerAoD).ToString();
                encirclementOrgAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.EncirclementOrgAttackerAoD).ToString();
                ambushStrDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.AmbushStrDefenderAoD).ToString();
                ambushOrgDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.AmbushOrgDefenderAoD).ToString();
                ambushStrAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.AmbushStrAttackerAoD).ToString();
                ambushOrgAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.AmbushOrgAttackerAoD).ToString();
                delayStrDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.DelayStrDefenderAoD).ToString();
                delayOrgDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.DelayOrgDefenderAoD).ToString();
                delayStrAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.DelayStrAttackerAoD).ToString();
                delayOrgAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.DelayOrgAttackerAoD).ToString();
                tacticalWithdrawStrDefenderAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawStrDefenderAoD).ToString();
                tacticalWithdrawOrgDefenderAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawOrgDefenderAoD).ToString();
                tacticalWithdrawStrAttackerAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawStrAttackerAoD).ToString();
                tacticalWithdrawOrgAttackerAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawOrgAttackerAoD).ToString();
                breakthroughStrDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughStrDefenderAoD).ToString();
                breakthroughOrgDefenderAoDTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughOrgDefenderAoD).ToString();
                breakthroughStrAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughStrAttackerAoD).ToString();
                breakthroughOrgAttackerAoDTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughOrgAttackerAoD).ToString();
            }
        }

        /// <summary>
        ///     戦闘4タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCombat4ItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                airDogfightXpGainFactorTextBox.Text = Misc.GetItem(MiscItemId.AirDogfightXpGainFactor).ToString();
                hardUnitsAttackingUrbanPenaltyTextBox.Text =
                    Misc.GetItem(MiscItemId.HardUnitsAttackingUrbanPenalty).ToString();
                supplyProblemsModifierLandTextBox.Text = Misc.GetItem(MiscItemId.SupplyProblemsModifierLand).ToString();
                supplyProblemsModifierAirTextBox.Text = Misc.GetItem(MiscItemId.SupplyProblemsModifierAir).ToString();
                supplyProblemsModifierNavalTextBox.Text =
                    Misc.GetItem(MiscItemId.SupplyProblemsModifierNaval).ToString();
                fuelProblemsModifierLandTextBox.Text = Misc.GetItem(MiscItemId.FuelProblemsModifierLand).ToString();
                fuelProblemsModifierAirTextBox.Text = Misc.GetItem(MiscItemId.FuelProblemsModifierAir).ToString();
                fuelProblemsModifierNavalTextBox.Text = Misc.GetItem(MiscItemId.FuelProblemsModifierNaval).ToString();
                convoyEscortsModelTextBox.Text = Misc.GetItem(MiscItemId.ConvoyEscortsModel).ToString();
                durationAirToAirBattlesTextBox.Text = Misc.GetItem(MiscItemId.DurationAirToAirBattles).ToString();
                durationNavalPortBombingTextBox.Text = Misc.GetItem(MiscItemId.DurationNavalPortBombing).ToString();
                durationStrategicBombingTextBox.Text = Misc.GetItem(MiscItemId.DurationStrategicBombing).ToString();
                durationGroundAttackBombingTextBox.Text =
                    Misc.GetItem(MiscItemId.DurationGroundAttackBombing).ToString();
                bonusSimilarTerrainTraitTextBox.Text = Misc.GetItem(MiscItemId.BonusSimilarTerrainTrait).ToString();
                airStrDamageLandOrgTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageLandOrg).ToString();
                airOrgDamageLandDhTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageLandDh).ToString();
                airStrDamageLandDhTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageLandDh).ToString();
                landOrgDamageLandOrgTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageLandOrg).ToString();
                landStrDamageLandDhTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageLandDh).ToString();
                airOrgDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageAirDh).ToString();
                airStrDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageAirDh).ToString();
                landOrgDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageAirDh).ToString();
                landStrDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageAirDh).ToString();
                navalOrgDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgDamageAirDh).ToString();
                navalStrDamageAirDhTextBox.Text = Misc.GetItem(MiscItemId.NavalStrDamageAirDh).ToString();
                subsOrgDamageAirTextBox.Text = Misc.GetItem(MiscItemId.SubsOrgDamageAir).ToString();
                subsStrDamageAirTextBox.Text = Misc.GetItem(MiscItemId.SubsStrDamageAir).ToString();
                airOrgDamageNavyDhTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageNavyDh).ToString();
                airStrDamageNavyDhTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageNavyDh).ToString();
                navalOrgDamageNavyDhTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgDamageNavyDh).ToString();
                navalStrDamageNavyDhTextBox.Text = Misc.GetItem(MiscItemId.NavalStrDamageNavyDh).ToString();
                subsOrgDamageNavyTextBox.Text = Misc.GetItem(MiscItemId.SubsOrgDamageNavy).ToString();
                subsStrDamageNavyTextBox.Text = Misc.GetItem(MiscItemId.SubsStrDamageNavy).ToString();
                navalOrgDamageAaTextBox.Text = Misc.GetItem(MiscItemId.NavalOrgDamageAa).ToString();
                airOrgDamageAaTextBox.Text = Misc.GetItem(MiscItemId.AirOrgDamageAa).ToString();
                airStrDamageAaTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageAa).ToString();
                aaAirFiringRulesComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.AaAirFiringRules);
                aaAirNightModifierTextBox.Text = Misc.GetItem(MiscItemId.AaAirNightModifier).ToString();
                aaAirBonusRadarsTextBox.Text = Misc.GetItem(MiscItemId.AaAirBonusRadars).ToString();
                movementBonusTerrainTraitTextBox.Text = Misc.GetItem(MiscItemId.MovementBonusTerrainTrait).ToString();
                movementBonusSimilarTerrainTraitTextBox.Text =
                    Misc.GetItem(MiscItemId.MovementBonusSimilarTerrainTrait).ToString();
                logisticsWizardEseBonusTextBox.Text = Misc.GetItem(MiscItemId.LogisticsWizardEseBonus).ToString();
                daysOffensiveSupplyTextBox.Text = Misc.GetItem(MiscItemId.DaysOffensiveSupply).ToString();
                ministerBonusesComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.MinisterBonuses) - 1;
                orgRegainBonusFriendlyTextBox.Text = Misc.GetItem(MiscItemId.OrgRegainBonusFriendly).ToString();
                orgRegainBonusFriendlyCapTextBox.Text = Misc.GetItem(MiscItemId.OrgRegainBonusFriendlyCap).ToString();
                convoyInterceptionMissionsComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.ConvoyInterceptionMissions);
                autoReturnTransportFleetsComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.AutoReturnTransportFleets);
                allowProvinceRegionTargetingComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.AllowProvinceRegionTargeting) ? 1 : 0;
                nightHoursWinterTextBox.Text = Misc.GetItem(MiscItemId.NightHoursWinter).ToString();
                nightHoursSpringFallTextBox.Text = Misc.GetItem(MiscItemId.NightHoursSpringFall).ToString();
                nightHoursSummerTextBox.Text = Misc.GetItem(MiscItemId.NightHoursSummer).ToString();
                recalculateLandArrivalTimesTextBox.Text =
                    Misc.GetItem(MiscItemId.RecalculateLandArrivalTimes).ToString();
                synchronizeArrivalTimePlayerTextBox.Text =
                    Misc.GetItem(MiscItemId.SynchronizeArrivalTimePlayer).ToString();
                synchronizeArrivalTimeAiTextBox.Text = Misc.GetItem(MiscItemId.SynchronizeArrivalTimeAi).ToString();
                recalculateArrivalTimesCombatComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.RecalculateArrivalTimesCombat);
                landSpeedModifierCombatTextBox.Text = Misc.GetItem(MiscItemId.LandSpeedModifierCombat).ToString();
                landSpeedModifierBombardmentTextBox.Text =
                    Misc.GetItem(MiscItemId.LandSpeedModifierBombardment).ToString();
                landSpeedModifierSupplyTextBox.Text = Misc.GetItem(MiscItemId.LandSpeedModifierSupply).ToString();
                landSpeedModifierOrgTextBox.Text = Misc.GetItem(MiscItemId.LandSpeedModifierOrg).ToString();
                landAirSpeedModifierFuelTextBox.Text = Misc.GetItem(MiscItemId.LandAirSpeedModifierFuel).ToString();
                defaultSpeedFuelTextBox.Text = Misc.GetItem(MiscItemId.DefaultSpeedFuel).ToString();
                fleetSizeRangePenaltyRatioTextBox.Text = Misc.GetItem(MiscItemId.FleetSizeRangePenaltyRatio).ToString();
                fleetSizeRangePenaltyThretholdTextBox.Text =
                    Misc.GetItem(MiscItemId.FleetSizeRangePenaltyThrethold).ToString();
                fleetSizeRangePenaltyMaxTextBox.Text = Misc.GetItem(MiscItemId.FleetSizeRangePenaltyMax).ToString();
                applyRangeLimitsAreasRegionsComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.ApplyRangeLimitsAreasRegions);
                radarBonusDetectionTextBox.Text = Misc.GetItem(MiscItemId.RadarBonusDetection).ToString();
                bonusDetectionFriendlyTextBox.Text = Misc.GetItem(MiscItemId.BonusDetectionFriendly).ToString();
                screensCapitalRatioModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.ScreensCapitalRatioModifier).ToString();
                chanceTargetNoOrgLandTextBox.Text = Misc.GetItem(MiscItemId.ChanceTargetNoOrgLand).ToString();
                screenCapitalShipsTargetingTextBox.Text =
                    Misc.GetItem(MiscItemId.ScreenCapitalShipsTargeting).ToString();
            }
        }

        /// <summary>
        ///     戦闘5タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCombat5ItemValue()
        {
            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                landChanceAvoidDefencesLeftTextBox.Text =
                    Misc.GetItem(MiscItemId.LandChanceAvoidDefencesLeft).ToString();
                airChanceAvoidDefencesLeftTextBox.Text = Misc.GetItem(MiscItemId.AirChanceAvoidDefencesLeft).ToString();
                navalChanceAvoidDefencesLeftTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalChanceAvoidDefencesLeft).ToString();
                landChanceAvoidNoDefencesTextBox.Text = Misc.GetItem(MiscItemId.LandChanceAvoidNoDefences).ToString();
                airChanceAvoidNoDefencesTextBox.Text = Misc.GetItem(MiscItemId.AirChanceAvoidNoDefences).ToString();
                navalChanceAvoidNoDefencesTextBox.Text = Misc.GetItem(MiscItemId.NavalChanceAvoidNoDefences).ToString();
                bonusLeaderSkillPointLandTextBox.Text = Misc.GetItem(MiscItemId.BonusLeaderSkillPointLand).ToString();
                bonusLeaderSkillPointAirTextBox.Text = Misc.GetItem(MiscItemId.BonusLeaderSkillPointAir).ToString();
                bonusLeaderSkillPointNavalTextBox.Text = Misc.GetItem(MiscItemId.BonusLeaderSkillPointNaval).ToString();
                landMinOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.LandMinOrgDamage).ToString();
                landOrgDamageHardSoftEachTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageHardSoftEach).ToString();
                landOrgDamageHardVsSoftTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageHardVsSoft).ToString();
                landMinStrDamageTextBox.Text = Misc.GetItem(MiscItemId.LandMinStrDamage).ToString();
                landStrDamageHardSoftEachTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageHardSoftEach).ToString();
                landStrDamageHardVsSoftTextBox.Text = Misc.GetItem(MiscItemId.LandStrDamageHardVsSoft).ToString();
                airMinOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.AirMinOrgDamage).ToString();
                airAdditionalOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.AirAdditionalOrgDamage).ToString();
                airMinStrDamageTextBox.Text = Misc.GetItem(MiscItemId.AirMinStrDamage).ToString();
                airAdditionalStrDamageTextBox.Text = Misc.GetItem(MiscItemId.AirAdditionalStrDamage).ToString();
                airStrDamageEntrencedTextBox.Text = Misc.GetItem(MiscItemId.AirStrDamageEntrenced).ToString();
                navalMinOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.NavalMinOrgDamage).ToString();
                navalAdditionalOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.NavalAdditionalOrgDamage).ToString();
                navalMinStrDamageTextBox.Text = Misc.GetItem(MiscItemId.NavalMinStrDamage).ToString();
                navalAdditionalStrDamageTextBox.Text = Misc.GetItem(MiscItemId.NavalAdditionalStrDamage).ToString();
                landOrgDamageLandUrbanTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageLandUrban).ToString();
                landOrgDamageLandFortTextBox.Text = Misc.GetItem(MiscItemId.LandOrgDamageLandFort).ToString();
                requiredLandFortSizeTextBox.Text = Misc.GetItem(MiscItemId.RequiredLandFortSize).ToString();
                fleetPositioningDaytimeTextBox.Text = Misc.GetItem(MiscItemId.FleetPositioningDaytime).ToString();
                fleetPositioningLeaderSkillTextBox.Text =
                    Misc.GetItem(MiscItemId.FleetPositioningLeaderSkill).ToString();
                fleetPositioningFleetSizeTextBox.Text = Misc.GetItem(MiscItemId.FleetPositioningFleetSize).ToString();
                fleetPositioningFleetCompositionTextBox.Text =
                    Misc.GetItem(MiscItemId.FleetPositioningFleetComposition).ToString();
                landCoastalFortsDamageTextBox.Text = Misc.GetItem(MiscItemId.LandCoastalFortsDamage).ToString();
                landCoastalFortsMaxDamageTextBox.Text = Misc.GetItem(MiscItemId.LandCoastalFortsMaxDamage).ToString();
                minSoftnessBrigadesTextBox.Text = Misc.GetItem(MiscItemId.MinSoftnessBrigades).ToString();
                autoRetreatOrgTextBox.Text = Misc.GetItem(MiscItemId.AutoRetreatOrg).ToString();
                landOrgNavalTransportationTextBox.Text = Misc.GetItem(MiscItemId.LandOrgNavalTransportation).ToString();
                maxLandDigTextBox.Text = Misc.GetItem(MiscItemId.MaxLandDig).ToString();
                digIncreaseDayTextBox.Text = Misc.GetItem(MiscItemId.DigIncreaseDay).ToString();
                breakthroughEncirclementMinSpeedTextBox.Text =
                    Misc.GetItem(MiscItemId.BreakthroughEncirclementMinSpeed).ToString();
                breakthroughEncirclementMaxChanceTextBox.Text =
                    Misc.GetItem(MiscItemId.BreakthroughEncirclementMaxChance).ToString();
                breakthroughEncirclementChanceModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.BreakthroughEncirclementChanceModifier).ToString();
                combatEventDurationTextBox.Text = Misc.GetItem(MiscItemId.CombatEventDuration).ToString();
                counterAttackOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.CounterAttackOrgAttackerDh).ToString();
                counterAttackStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.CounterAttackStrAttackerDh).ToString();
                counterAttackOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.CounterAttackOrgDefenderDh).ToString();
                counterAttackStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.CounterAttackStrDefenderDh).ToString();
                assaultOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.AssaultOrgAttackerDh).ToString();
                assaultStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.AssaultStrAttackerDh).ToString();
                assaultOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.AssaultOrgDefenderDh).ToString();
                assaultStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.AssaultStrDefenderDh).ToString();
                encirclementOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.EncirclementOrgAttackerDh).ToString();
                encirclementStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.EncirclementStrAttackerDh).ToString();
                encirclementOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.EncirclementOrgDefenderDh).ToString();
                encirclementStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.EncirclementStrDefenderDh).ToString();
                ambushOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.AmbushOrgAttackerDh).ToString();
                ambushStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.AmbushStrAttackerDh).ToString();
                ambushOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.AmbushOrgDefenderDh).ToString();
                ambushStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.AmbushStrDefenderDh).ToString();
                delayOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.DelayOrgAttackerDh).ToString();
                delayStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.DelayStrAttackerDh).ToString();
                delayOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.DelayOrgDefenderDh).ToString();
                delayStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.DelayStrDefenderDh).ToString();
                tacticalWithdrawOrgAttackerDhTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawOrgAttackerDh).ToString();
                tacticalWithdrawStrAttackerDhTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawStrAttackerDh).ToString();
                tacticalWithdrawOrgDefenderDhTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawOrgDefenderDh).ToString();
                tacticalWithdrawStrDefenderDhTextBox.Text =
                    Misc.GetItem(MiscItemId.TacticalWithdrawStrDefenderDh).ToString();
                breakthroughOrgAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughOrgAttackerDh).ToString();
                breakthroughStrAttackerDhTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughStrAttackerDh).ToString();
                breakthroughOrgDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughOrgDefenderDh).ToString();
                breakthroughStrDefenderDhTextBox.Text = Misc.GetItem(MiscItemId.BreakthroughStrDefenderDh).ToString();
                hqStrDamageBreakthroughComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.HqStrDamageBreakthrough)
                                                                    ? 1
                                                                    : 0;
                combatModeComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.CombatMode);
            }
        }

        /// <summary>
        ///     任務1タブの編集項目の値を更新する
        /// </summary>
        private void UpdateMission1ItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                attackMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AttackMission) ? 1 : 0;
                attackStartingEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.AttackStartingEfficiency).ToString();
                attackSpeedBonusTextBox.Text = Misc.GetItem(MiscItemId.AttackSpeedBonus).ToString();
                rebaseMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.RebaseMission) ? 1 : 0;
                rebaseStartingEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.RebaseStartingEfficiency).ToString();
                rebaseChanceDetectedTextBox.Text = Misc.GetItem(MiscItemId.RebaseChanceDetected).ToString();
                stratRedeployMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.StratRedeployMission)
                                                                 ? 1
                                                                 : 0;
                stratRedeployStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.StratRedeployStartingEfficiency).ToString();
                stratRedeployAddedValueTextBox.Text = Misc.GetItem(MiscItemId.StratRedeployAddedValue).ToString();
                stratRedeployDistanceMultiplierTextBox.Text =
                    Misc.GetItem(MiscItemId.StratRedeployDistanceMultiplier).ToString();
                supportAttackMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.SupportAttackMission)
                                                                 ? 1
                                                                 : 0;
                supportAttackStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.SupportAttackStartingEfficiency).ToString();
                supportAttackSpeedBonusTextBox.Text = Misc.GetItem(MiscItemId.SupportAttackSpeedBonus).ToString();
                supportDefenseMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.SupportDefenseMission)
                                                                  ? 1
                                                                  : 0;
                supportDefenseStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.SupportDefenseStartingEfficiency).ToString();
                supportDefenseSpeedBonusTextBox.Text = Misc.GetItem(MiscItemId.SupportDefenseSpeedBonus).ToString();
                reservesMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ReservesMission) ? 1 : 0;
                reservesStartingEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.ReservesStartingEfficiency).ToString();
                reservesSpeedBonusTextBox.Text = Misc.GetItem(MiscItemId.ReservesSpeedBonus).ToString();
                antiPartisanDutyMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AntiPartisanDutyMission)
                                                                    ? 1
                                                                    : 0;
                antiPartisanDutyStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AntiPartisanDutyStartingEfficiency).ToString();
                antiPartisanDutySuppressionTextBox.Text =
                    Misc.GetItem(MiscItemId.AntiPartisanDutySuppression).ToString();
                plannedDefenseMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.PlannedDefenseMission)
                                                                  ? 1
                                                                  : 0;
                plannedDefenseStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.PlannedDefenseStartingEfficiency).ToString();
                airSuperiorityMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AirSuperiorityMission)
                                                                  ? 1
                                                                  : 0;
                airSuperiorityStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AirSuperiorityStartingEfficiency).ToString();
                airSuperiorityDetectionTextBox.Text = Misc.GetItem(MiscItemId.AirSuperiorityDetection).ToString();
                airSuperiorityMinRequiredTextBox.Text = Misc.GetItem(MiscItemId.AirSuperiorityMinRequired).ToString();
                groundAttackMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.GroundAttackMission) ? 1 : 0;
                groundAttackStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.GroundAttackStartingEfficiency).ToString();
                groundAttackOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.GroundAttackOrgDamage).ToString();
                groundAttackStrDamageTextBox.Text = Misc.GetItem(MiscItemId.GroundAttackStrDamage).ToString();
                interdictionMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.InterdictionMission) ? 1 : 0;
                interdictionStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.InterdictionStartingEfficiency).ToString();
                interdictionOrgDamageTextBox.Text = Misc.GetItem(MiscItemId.InterdictionOrgDamage).ToString();
                interdictionStrDamageTextBox.Text = Misc.GetItem(MiscItemId.InterdictionStrDamage).ToString();
                strategicBombardmentMissionComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.StrategicBombardmentMission) ? 1 : 0;
                strategicBombardmentStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.StrategicBombardmentStartingEfficiency).ToString();
                logisticalStrikeMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.LogisticalStrikeMission)
                                                                    ? 1
                                                                    : 0;
                logisticalStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.LogisticalStrikeStartingEfficiency).ToString();
                runwayCrateringMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.RunwayCrateringMission)
                                                                   ? 1
                                                                   : 0;
                runwayCrateringStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.RunwayCrateringStartingEfficiency).ToString();
                installationStrikeMissionComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.InstallationStrikeMission) ? 1 : 0;
                installationStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.InstallationStrikeStartingEfficiency).ToString();
                navalStrikeMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NavalStrikeMission) ? 1 : 0;
                navalStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalStrikeStartingEfficiency).ToString();
                portStrikeMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.PortStrikeMission) ? 1 : 0;
                portStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.PortStrikeStartingEfficiency).ToString();
                convoyAirRaidingMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ConvoyAirRaidingMission)
                                                                    ? 1
                                                                    : 0;
                convoyAirRaidingStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.ConvoyAirRaidingStartingEfficiency).ToString();
                airSupplyMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AirSupplyMission) ? 1 : 0;
                airSupplyStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AirSupplyStartingEfficiency).ToString();
                airborneAssaultMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AirborneAssaultMission)
                                                                   ? 1
                                                                   : 0;
                airborneAssaultStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AirborneAssaultStartingEfficiency).ToString();
                nukeMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NukeMission) ? 1 : 0;
                nukeStartingEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.NukeStartingEfficiency).ToString();
                airScrambleMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AirScrambleMission) ? 1 : 0;
                airScrambleStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AirScrambleStartingEfficiency).ToString();
                airScrambleDetectionTextBox.Text = Misc.GetItem(MiscItemId.AirScrambleDetection).ToString();
                airScrambleMinRequiredTextBox.Text = Misc.GetItem(MiscItemId.AirScrambleMinRequired).ToString();
                convoyRadingMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ConvoyRadingMission) ? 1 : 0;
                convoyRadingStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.ConvoyRadingStartingEfficiency).ToString();
                convoyRadingRangeModifierTextBox.Text = Misc.GetItem(MiscItemId.ConvoyRadingRangeModifier).ToString();
                convoyRadingChanceDetectedTextBox.Text = Misc.GetItem(MiscItemId.ConvoyRadingChanceDetected).ToString();
                aswMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AswMission) ? 1 : 0;
                aswStartingEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.AswStartingEfficiency).ToString();
                navalInterdictionMissionComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NavalInterdictionMission) ? 1 : 0;
                navalInterdictionStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalInterdictionStartingEfficiency).ToString();
                shoreBombardmentMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ShoreBombardmentMission)
                                                                    ? 1
                                                                    : 0;
                shoreBombardmentStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.ShoreBombardmentStartingEfficiency).ToString();
                shoreBombardmentModifierDhTextBox.Text = Misc.GetItem(MiscItemId.ShoreBombardmentModifierDh).ToString();
            }
        }

        /// <summary>
        ///     任務2タブの編集項目の値を更新する
        /// </summary>
        private void UpdateMission2ItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                amphibousAssaultMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AmphibousAssaultMission)
                                                                    ? 1
                                                                    : 0;
                amphibousAssaultStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.AmphibousAssaultStartingEfficiency).ToString();
                seaTransportMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.SeaTransportMission) ? 1 : 0;
                seaTransportStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.SeaTransportStartingEfficiency).ToString();
                seaTransportRangeModifierTextBox.Text = Misc.GetItem(MiscItemId.SeaTransportRangeModifier).ToString();
                seaTransportChanceDetectedTextBox.Text = Misc.GetItem(MiscItemId.SeaTransportChanceDetected).ToString();
                NavalCombatPatrolMissionComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NavalCombatPatrolMission) ? 1 : 0;
                NavalCombatPatrolStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalCombatPatrolStartingEfficiency).ToString();
                navalPortStrikeMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NavalPortStrikeMission)
                                                                   ? 1
                                                                   : 0;
                navalPortStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalPortStrikeStartingEfficiency).ToString();
                navalAirbaseStrikeMissionComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NavalAirbaseStrikeMission) ? 1 : 0;
                navalAirbaseStrikeStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalAirbaseStrikeStartingEfficiency).ToString();
                sneakMoveMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.SneakMoveMission) ? 1 : 0;
                sneakMoveStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.SneakMoveStartingEfficiency).ToString();
                sneakMoveRangeModifierTextBox.Text = Misc.GetItem(MiscItemId.SneakMoveRangeModifier).ToString();
                sneakMoveChanceDetectedTextBox.Text = Misc.GetItem(MiscItemId.SneakMoveChanceDetected).ToString();
                navalScrambleMissionComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NavalScrambleMission)
                                                                 ? 1
                                                                 : 0;
                navalScrambleStartingEfficiencyTextBox.Text =
                    Misc.GetItem(MiscItemId.NavalScrambleStartingEfficiency).ToString();
                navalScrambleSpeedBonusTextBox.Text = Misc.GetItem(MiscItemId.NavalScrambleSpeedBonus).ToString();
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                useAttackEfficiencyCombatModifierComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.UseAttackEfficiencyCombatModifier) ? 1 : 0;
            }
        }

        /// <summary>
        ///     国家タブの編集項目の値を更新する
        /// </summary>
        private void UpdateCountryItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                landFortEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.LandFortEfficiency).ToString();
                coastalFortEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.CoastalFortEfficiency).ToString();
                groundDefenseEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.GroundDefenseEfficiency).ToString();
                convoyDefenseEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.ConvoyDefenseEfficiency).ToString();
                manpowerBoostTextBox.Text = Misc.GetItem(MiscItemId.ManpowerBoost).ToString();
                transportCapacityModifierTextBox.Text = Misc.GetItem(MiscItemId.TransportCapacityModifier).ToString();
                occupiedTransportCapacityModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.OccupiedTransportCapacityModifier).ToString();
                attritionModifierTextBox.Text = Misc.GetItem(MiscItemId.AttritionModifier).ToString();
                manpowerTrickleBackModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.ManpowerTrickleBackModifier).ToString();
                supplyDistanceModifierTextBox.Text = Misc.GetItem(MiscItemId.SupplyDistanceModifier).ToString();
                repairModifierTextBox.Text = Misc.GetItem(MiscItemId.RepairModifier).ToString();
                researchModifierTextBox.Text = Misc.GetItem(MiscItemId.ResearchModifier).ToString();
                radarEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.RadarEfficiency).ToString();
                hqSupplyEfficiencyBonusTextBox.Text = Misc.GetItem(MiscItemId.HqSupplyEfficiencyBonus).ToString();
                hqCombatEventsBonusTextBox.Text = Misc.GetItem(MiscItemId.HqCombatEventsBonus).ToString();
                combatEventChancesTextBox.Text = Misc.GetItem(MiscItemId.CombatEventChances).ToString();
                friendlyArmyDetectionChanceTextBox.Text =
                    Misc.GetItem(MiscItemId.FriendlyArmyDetectionChance).ToString();
                enemyArmyDetectionChanceTextBox.Text = Misc.GetItem(MiscItemId.EnemyArmyDetectionChance).ToString();
                friendlyIntelligenceChanceTextBox.Text = Misc.GetItem(MiscItemId.FriendlyIntelligenceChance).ToString();
                enemyIntelligenceChanceTextBox.Text = Misc.GetItem(MiscItemId.EnemyIntelligenceChance).ToString();
                maxAmphibiousArmySizeTextBox.Text = Misc.GetItem(MiscItemId.MaxAmphibiousArmySize).ToString();
                energyToOilTextBox.Text = Misc.GetItem(MiscItemId.EnergyToOil).ToString();
                totalProductionEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.TotalProductionEfficiency).ToString();
                supplyProductionEfficiencyTextBox.Text = Misc.GetItem(MiscItemId.SupplyProductionEfficiency).ToString();
                aaPowerTextBox.Text = Misc.GetItem(MiscItemId.AaPower).ToString();
                airSurpriseChanceTextBox.Text = Misc.GetItem(MiscItemId.AirSurpriseChance).ToString();
                landSurpriseChanceTextBox.Text = Misc.GetItem(MiscItemId.LandSurpriseChance).ToString();
                navalSurpriseChanceTextBox.Text = Misc.GetItem(MiscItemId.NavalSurpriseChance).ToString();
                peacetimeIcModifierTextBox.Text = Misc.GetItem(MiscItemId.PeacetimeIcModifier).ToString();
                wartimeIcModifierTextBox.Text = Misc.GetItem(MiscItemId.WartimeIcModifier).ToString();
                buildingsProductionModifierTextBox.Text =
                    Misc.GetItem(MiscItemId.BuildingsProductionModifier).ToString();
                convoysProductionModifierTextBox.Text = Misc.GetItem(MiscItemId.ConvoysProductionModifier).ToString();
                minShipsPositioningBattleTextBox.Text = Misc.GetItem(MiscItemId.MinShipsPositioningBattle).ToString();
                maxShipsPositioningBattleTextBox.Text = Misc.GetItem(MiscItemId.MaxShipsPositioningBattle).ToString();
                peacetimeStockpilesResourcesTextBox.Text =
                    Misc.GetItem(MiscItemId.PeacetimeStockpilesResources).ToString();
                wartimeStockpilesResourcesTextBox.Text = Misc.GetItem(MiscItemId.WartimeStockpilesResources).ToString();
                peacetimeStockpilesOilSuppliesTextBox.Text =
                    Misc.GetItem(MiscItemId.PeacetimeStockpilesOilSupplies).ToString();
                wartimeStockpilesOilSuppliesTextBox.Text =
                    Misc.GetItem(MiscItemId.WartimeStockpilesOilSupplies).ToString();
            }
        }

        /// <summary>
        ///     研究タブの編集項目の値を更新する
        /// </summary>
        private void UpdateResearchItemValue()
        {
            blueprintBonusTextBox.Text = Misc.GetItem(MiscItemId.BlueprintBonus).ToString();
            preHistoricalDateModifierTextBox.Text = Misc.GetItem(MiscItemId.PreHistoricalDateModifier).ToString();
            costSkillLevelTextBox.Text = Misc.GetItem(MiscItemId.CostSkillLevel).ToString();
            meanNumberInventionEventsYearTextBox.Text =
                Misc.GetItem(MiscItemId.MeanNumberInventionEventsYear).ToString();

            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                postHistoricalDateModifierAoDTextBox.Text =
                    Misc.GetItem(MiscItemId.PostHistoricalDateModifierAoD).ToString();
                techSpeedModifierTextBox.Text = Misc.GetItem(MiscItemId.TechSpeedModifier).ToString();
                preHistoricalPenaltyLimitTextBox.Text = Misc.GetItem(MiscItemId.PreHistoricalPenaltyLimit).ToString();
                postHistoricalBonusLimitTextBox.Text = Misc.GetItem(MiscItemId.PostHistoricalBonusLimit).ToString();
            }

            // AoD1.07以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                maxActiveTechTeamsAoDTextBox.Text = Misc.GetItem(MiscItemId.MaxActiveTechTeamsAoD).ToString();
                requiredIcEachTechTeamAoDTextBox.Text = Misc.GetItem(MiscItemId.RequiredIcEachTechTeamAoD).ToString();
            }

            // AoD1.08以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108)
            {
                maximumRandomModifierTextBox.Text = Misc.GetItem(MiscItemId.MaximumRandomModifier).ToString();
            }

            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                postHistoricalDateModifierDhTextBox.Text =
                    Misc.GetItem(MiscItemId.PostHistoricalDateModifierDh).ToString();
                useNewTechnologyPageLayoutComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.UseNewTechnologyPageLayout);
                maxActiveTechTeamsDhTextBox.Text = Misc.GetItem(MiscItemId.MaxActiveTechTeamsDh).ToString();
                minActiveTechTeamsTextBox.Text = Misc.GetItem(MiscItemId.MinActiveTechTeams).ToString();
                requiredIcEachTechTeamDhTextBox.Text = Misc.GetItem(MiscItemId.RequiredIcEachTechTeamDh).ToString();
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                techOverviewPanelStyleComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.TechOverviewPanelStyle);
                newCountryRocketryComponentComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NewCountryRocketryComponent) ? 1 : 0;
                newCountryNuclearPhysicsComponentComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NewCountryNuclearPhysicsComponent) ? 1 : 0;
                newCountryNuclearEngineeringComponentComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.NewCountryNuclearEngineeringComponent) ? 1 : 0;
                newCountrySecretTechsComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NewCountrySecretTechs)
                                                                  ? 1
                                                                  : 0;
                maxTechTeamSkillTextBox.Text = Misc.GetItem(MiscItemId.MaxTechTeamSkill).ToString();
            }
        }

        /// <summary>
        ///     貿易タブの編集項目の値を更新する
        /// </summary>
        private void UpdateTradeItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                daysTradeOffersTextBox.Text = Misc.GetItem(MiscItemId.DaysTradeOffers).ToString();
                delayGameStartNewTradesTextBox.Text = Misc.GetItem(MiscItemId.DelayGameStartNewTrades).ToString();
                limitAiNewTradesGameStartTextBox.Text = Misc.GetItem(MiscItemId.LimitAiNewTradesGameStart).ToString();
                desiredOilStockpileTextBox.Text = Misc.GetItem(MiscItemId.DesiredOilStockpile).ToString();
                criticalOilStockpileTextBox.Text = Misc.GetItem(MiscItemId.CriticalOilStockpile).ToString();
                desiredSuppliesStockpileTextBox.Text = Misc.GetItem(MiscItemId.DesiredSuppliesStockpile).ToString();
                criticalSuppliesStockpileTextBox.Text = Misc.GetItem(MiscItemId.CriticalSuppliesStockpile).ToString();
                desiredResourcesStockpileTextBox.Text = Misc.GetItem(MiscItemId.DesiredResourcesStockpile).ToString();
                criticalResourceStockpileTextBox.Text = Misc.GetItem(MiscItemId.CriticalResourceStockpile).ToString();
                wartimeDesiredStockpileMultiplierTextBox.Text =
                    Misc.GetItem(MiscItemId.WartimeDesiredStockpileMultiplier).ToString();
                peacetimeExtraOilImportTextBox.Text = Misc.GetItem(MiscItemId.PeacetimeExtraOilImport).ToString();
                wartimeExtraOilImportTextBox.Text = Misc.GetItem(MiscItemId.WartimeExtraOilImport).ToString();
                extraImportBelowDesiredTextBox.Text = Misc.GetItem(MiscItemId.ExtraImportBelowDesired).ToString();
                percentageProducedSuppliesTextBox.Text = Misc.GetItem(MiscItemId.PercentageProducedSupplies).ToString();
                percentageProducedMoneyTextBox.Text = Misc.GetItem(MiscItemId.PercentageProducedMoney).ToString();
                extraImportStockpileSelectedTextBox.Text =
                    Misc.GetItem(MiscItemId.ExtraImportStockpileSelected).ToString();
                daysDeliverResourcesTradesTextBox.Text = Misc.GetItem(MiscItemId.DaysDeliverResourcesTrades).ToString();
                mergeTradeDealsComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.MergeTradeDeals) ? 1 : 0;
                manualTradeDealsComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.ManualTradeDeals) ? 1 : 0;
                puppetsSendSuppliesMoneyTextBox.Text = Misc.GetItem(MiscItemId.PuppetsSendSuppliesMoney).ToString();
                puppetsCriticalSupplyStockpileTextBox.Text =
                    Misc.GetItem(MiscItemId.PuppetsCriticalSupplyStockpile).ToString();
                puppetsMaxPoolResourcesTextBox.Text = Misc.GetItem(MiscItemId.PuppetsMaxPoolResources).ToString();
                newTradeDealsMinEffectivenessTextBox.Text =
                    Misc.GetItem(MiscItemId.NewTradeDealsMinEffectiveness).ToString();
                cancelTradeDealsEffectivenessTextBox.Text =
                    Misc.GetItem(MiscItemId.CancelTradeDealsEffectiveness).ToString();
                autoTradeAiTradeDealsTextBox.Text = Misc.GetItem(MiscItemId.AutoTradeAiTradeDeals).ToString();
            }
        }

        /// <summary>
        ///     AIタブの編集項目の値を更新する
        /// </summary>
        private void UpdateAiItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                overproduceSuppliesBelowDesiredTextBox.Text =
                    Misc.GetItem(MiscItemId.OverproduceSuppliesBelowDesired).ToString();
                multiplierOverproduceSuppliesWarTextBox.Text =
                    Misc.GetItem(MiscItemId.MultiplierOverproduceSuppliesWar).ToString();
                notProduceSuppliesStockpileOverTextBox.Text =
                    Misc.GetItem(MiscItemId.NotProduceSuppliesStockpileOver).ToString();
                maxSerialLineProductionGarrisonMilitiaTextBox.Text =
                    Misc.GetItem(MiscItemId.MaxSerialLineProductionGarrisonMilitia).ToString();
                minIcSerialProductionNavalAirTextBox.Text =
                    Misc.GetItem(MiscItemId.MinIcSerialProductionNavalAir).ToString();
                notProduceNewUnitsManpowerRatioTextBox.Text =
                    Misc.GetItem(MiscItemId.NotProduceNewUnitsManpowerRatio).ToString();
                notProduceNewUnitsManpowerValueTextBox.Text =
                    Misc.GetItem(MiscItemId.NotProduceNewUnitsManpowerValue).ToString();
                notProduceNewUnitsSupplyTextBox.Text = Misc.GetItem(MiscItemId.NotProduceNewUnitsSupply).ToString();
                militaryStrengthTotalIcRatioPeacetimeTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitaryStrengthTotalIcRatioPeacetime).ToString();
                militaryStrengthTotalIcRatioWartimeTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitaryStrengthTotalIcRatioWartime).ToString();
                militaryStrengthTotalIcRatioMajorTextBox.Text =
                    Misc.GetItem(MiscItemId.MilitaryStrengthTotalIcRatioMajor).ToString();
                notUseOffensiveSupplyStockpileTextBox.Text =
                    Misc.GetItem(MiscItemId.NotUseOffensiveSupplyStockpile).ToString();
                notUseOffensiveOilStockpileTextBox.Text =
                    Misc.GetItem(MiscItemId.NotUseOffensiveOilStockpile).ToString();
                notUseOffensiveEseTextBox.Text = Misc.GetItem(MiscItemId.NotUseOffensiveEse).ToString();
                notUseOffensiveOrgStrDamageTextBox.Text =
                    Misc.GetItem(MiscItemId.NotUseOffensiveOrgStrDamage).ToString();
                aiPeacetimeSpyMissionsDhComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.AiPeacetimeSpyMissionsDh);
                aiSpyMissionsCostModifierDhTextBox.Text =
                    Misc.GetItem(MiscItemId.AiSpyMissionsCostModifierDh).ToString();
                aiDiplomacyCostModifierDhTextBox.Text = Misc.GetItem(MiscItemId.AiDiplomacyCostModifierDh).ToString();
                aiInfluenceModifierDhTextBox.Text = Misc.GetItem(MiscItemId.AiInfluenceModifierDh).ToString();
                forcePuppetsJoinMastersAllianceNeutralityTextBox.Text =
                    Misc.GetItem(MiscItemId.ForcePuppetsJoinMastersAllianceNeutrality).ToString();
                newAiReleaseRulesComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.NewAiReleaseRules) ? 1 : 0;
                aiEventsActionSelectionRulesTextBox.Text =
                    Misc.GetItem(MiscItemId.AiEventsActionSelectionRules).ToString();
                forceStrategicRedeploymentHourTextBox.Text =
                    Misc.GetItem(MiscItemId.ForceStrategicRedeploymentHour).ToString();
                maxRedeploymentDaysAiTextBox.Text = Misc.GetItem(MiscItemId.MaxRedeploymentDaysAi).ToString();
                useQuickAreaCheckGarrisonAiComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.UseQuickAreaCheckGarrisonAi) ? 1 : 0;
                aiMastersGetProvincesConquredPuppetsComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.AiMastersGetProvincesConquredPuppets) ? 1 : 0;
                minDaysRequiredAiReleaseCountryTextBox.Text =
                    Misc.GetItem(MiscItemId.MinDaysRequiredAiReleaseCountry).ToString();
                minDaysRequiredAiAlliedTextBox.Text = Misc.GetItem(MiscItemId.MinDaysRequiredAiAllied).ToString();
                minDaysRequiredAiAlliedSupplyBaseTextBox.Text =
                    Misc.GetItem(MiscItemId.MinDaysRequiredAiAlliedSupplyBase).ToString();
                minRequiredRelationsAlliedClaimedTextBox.Text =
                    Misc.GetItem(MiscItemId.MinRequiredRelationsAlliedClaimed).ToString();
            }

            // DH1.02以前固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version <= 102)
            {
                newDowRulesComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.NewDowRules);
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                newDowRules2ComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.NewDowRules2);
            }
        }

        /// <summary>
        ///     MODタブの編集項目の値を更新する
        /// </summary>
        private void UpdateModItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                aiSpyDiplomaticMissionLoggerComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.AiSpyDiplomaticMissionLogger) ? 1 : 0;
                countryLoggerTextBox.Text = Misc.GetItem(MiscItemId.CountryLogger).ToString();
                switchedAiFilesLoggerComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.SwitchedAiFilesLogger)
                                                                  ? 1
                                                                  : 0;
                useNewAutoSaveFileFormatComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.UseNewAutoSaveFileFormat) ? 1 : 0;
                loadNewAiSwitchingAllClientsComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.LoadNewAiSwitchingAllClients) ? 1 : 0;
                tradeEfficiencyCalculationSystemTextBox.Text =
                    Misc.GetItem(MiscItemId.TradeEfficiencyCalculationSystem).ToString();
                mergeRelocateProvincialDepotsTextBox.Text =
                    Misc.GetItem(MiscItemId.MergeRelocateProvincialDepots).ToString();
                allowBrigadeAttachingInSupplyComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.AllowBrigadeAttachingInSupply) ? 1 : 0;
                multipleDeploymentSizeArmiesTextBox.Text =
                    Misc.GetItem(MiscItemId.MultipleDeploymentSizeArmies).ToString();
                multipleDeploymentSizeFleetsTextBox.Text =
                    Misc.GetItem(MiscItemId.MultipleDeploymentSizeFleets).ToString();
                multipleDeploymentSizeAirTextBox.Text = Misc.GetItem(MiscItemId.MultipleDeploymentSizeAir).ToString();
                allowUniquePicturesAllLandProvincesComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.AllowUniquePicturesAllLandProvinces) ? 1 : 0;
                autoReplyEventsComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.AutoReplyEvents) ? 1 : 0;
                forceActionsShowComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.ForceActionsShow);
                enableDicisionsPlayersComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.EnableDicisionsPlayers)
                                                                   ? 1
                                                                   : 0;
                rebelsArmyCompositionTextBox.Text = Misc.GetItem(MiscItemId.RebelsArmyComposition).ToString();
                rebelsArmyTechLevelTextBox.Text = Misc.GetItem(MiscItemId.RebelsArmyTechLevel).ToString();
                rebelsArmyMinStrTextBox.Text = Misc.GetItem(MiscItemId.RebelsArmyMinStr).ToString();
                rebelsArmyMaxStrTextBox.Text = Misc.GetItem(MiscItemId.RebelsArmyMaxStr).ToString();
                rebelsOrgRegainTextBox.Text = Misc.GetItem(MiscItemId.RebelsOrgRegain).ToString();
                extraRebelBonusNeighboringProvinceTextBox.Text =
                    Misc.GetItem(MiscItemId.ExtraRebelBonusNeighboringProvince).ToString();
                extraRebelBonusOccupiedTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusOccupied).ToString();
                extraRebelBonusMountainTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusMountain).ToString();
                extraRebelBonusHillTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusHill).ToString();
                extraRebelBonusForestTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusForest).ToString();
                extraRebelBonusJungleTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusJungle).ToString();
                extraRebelBonusSwampTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusSwamp).ToString();
                extraRebelBonusDesertTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusDesert).ToString();
                extraRebelBonusPlainTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusPlain).ToString();
                extraRebelBonusUrbanTextBox.Text = Misc.GetItem(MiscItemId.ExtraRebelBonusUrban).ToString();
                extraRebelBonusAirNavalBasesTextBox.Text =
                    Misc.GetItem(MiscItemId.ExtraRebelBonusAirNavalBases).ToString();
                returnRebelliousProvinceTextBox.Text = Misc.GetItem(MiscItemId.ReturnRebelliousProvince).ToString();
                useNewMinisterFilesFormatComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.UseNewMinisterFilesFormat) ? 1 : 0;
                loadSpritesModdirOnlyComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.LoadSpritesModdirOnly)
                                                                  ? 1
                                                                  : 0;
                loadUnitIconsModdirOnlyComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.LoadUnitIconsModdirOnly)
                                                                    ? 1
                                                                    : 0;
                loadUnitPicturesModdirOnlyComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.LoadUnitPicturesModdirOnly) ? 1 : 0;
                loadAiFilesModdirOnlyComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.LoadAiFilesModdirOnly)
                                                                  ? 1
                                                                  : 0;
                useSpeedSetGarrisonStatusComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.UseSpeedSetGarrisonStatus);
                useOldSaveGameFormatComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.UseOldSaveGameFormat)
                                                                 ? 1
                                                                 : 0;
            }

            // DH1.02以前固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version <= 102)
            {
                inGameLossesLoggingComboBox.SelectedIndex = (bool) Misc.GetItem(MiscItemId.InGameLossesLogging) ? 1 : 0;
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                inGameLossLogging2ComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.InGameLossLogging2);
                enableRetirementYearMinistersComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.EnableRetirementYearMinisters) ? 1 : 0;
                enableRetirementYearLeadersComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.EnableRetirementYearLeaders) ? 1 : 0;
                productionPanelUiStyleComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.ProductionPanelUiStyle);
                unitPicturesSizeComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.UnitPicturesSize);
                enablePicturesNavalBrigadesComboBox.SelectedIndex =
                    (bool) Misc.GetItem(MiscItemId.EnablePicturesNavalBrigades) ? 1 : 0;
                buildingsBuildableOnlyProvincesComboBox.SelectedIndex =
                    (int) Misc.GetItem(MiscItemId.BuildingsBuildableOnlyProvinces);
                unitModifiersStatisticsPagesTextBox.Text =
                    Misc.GetItem(MiscItemId.UnitModifiersStatisticsPages).ToString();
            }
        }

        /// <summary>
        ///     マップタブの編集項目の値を更新する
        /// </summary>
        private void UpdateMapItemValue()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                mapNumberTextBox.Text = Misc.GetItem(MiscItemId.MapNumber).ToString();
                totalProvincesTextBox.Text = Misc.GetItem(MiscItemId.TotalProvinces).ToString();
                distanceCalculationModelComboBox.SelectedIndex = (int) Misc.GetItem(MiscItemId.DistanceCalculationModel);
                MapWidthTextBox.Text = Misc.GetItem(MiscItemId.MapWidth).ToString();
                MapHeightTextBox.Text = Misc.GetItem(MiscItemId.MapHeight).ToString();
            }
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        private void UpdateEditableItemsColor()
        {
            UpdateEconomy1ItemColor();
            UpdateEconomy2ItemColor();
            UpdateEconomy3ItemColor();
            UpdateIntelligenceItemColor();
            UpdateDiplomacyItemColor();
            UpdateCombat1ItemColor();
            UpdateCombat2ItemColor();
            UpdateCombat3ItemColor();
            UpdateCombat4ItemColor();
            UpdateCombat5ItemColor();
            UpdateMission1ItemColor();
            UpdateMission2ItemColor();
            UpdateCountryItemColor();
            UpdateResearchItemColor();
            UpdateTradeItemColor();
            UpdateAiItemColor();
            UpdateModItemColor();
            UpdateMapItemColor();
        }

        /// <summary>
        ///     経済1タブの編集項目の色を更新する
        /// </summary>
        private void UpdateEconomy1ItemColor()
        {
            icToTcRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToTcRatio) ? Color.Red : SystemColors.WindowText;
            icToSuppliesRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToSuppliesRatio)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            icToConsumerGoodsRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToConsumerGoodsRatio)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            icToMoneyRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcToMoneyRatio)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            maxGearingBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxGearingBonus)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            gearingBonusIncrementTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusIncrement)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            icMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierNonNational)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            icMultiplierNonOwnedTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierNonOwned)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            tcLoadUndeployedDivisionTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadUndeployedDivision)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            tcLoadOccupiedTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadOccupied)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            tcLoadMultiplierLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierLand)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            tcLoadMultiplierAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierAir)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            tcLoadMultiplierNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadMultiplierNaval)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            tcLoadPartisanTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadPartisan)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            tcLoadFactorOffensiveTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadFactorOffensive)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            tcLoadProvinceDevelopmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadProvinceDevelopment)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            tcLoadBaseTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadBase) ? Color.Red : SystemColors.WindowText;
            manpowerMultiplierNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierNational)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            manpowerMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierNonNational)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            manpowerMultiplierColonyTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierColony)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            trickleBackFactorManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.TrickleBackFactorManpower)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            reinforceManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceManpower)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            reinforceCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceCost)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            reinforceTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceTime)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            upgradeCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.UpgradeCost) ? Color.Red : SystemColors.WindowText;
            upgradeTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.UpgradeTime) ? Color.Red : SystemColors.WindowText;
            nationalismStartingValueTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismStartingValue)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            monthlyNationalismReductionTextBox.ForeColor = Misc.IsDirty(MiscItemId.MonthlyNationalismReduction)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            sendDivisionDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.SendDivisionDays)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            tcLoadUndeployedBrigadeTextBox.ForeColor = Misc.IsDirty(MiscItemId.TcLoadUndeployedBrigade)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;

            // DDA1.3以降固有項目
            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                spyMissionDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyMissionDays)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                increateIntelligenceLevelDaysTextBox.ForeColor = Misc.IsDirty(MiscItemId.IncreateIntelligenceLevelDays)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                chanceDetectSpyMissionTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceDetectSpyMission)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                relationshipsHitDetectedMissionsTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.RelationshipsHitDetectedMissions) ? Color.Red : SystemColors.WindowText;
                distanceModifierNeighboursTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifierNeighbours)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                spyInformationAccuracyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyInformationAccuracyModifier)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                maxIcCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIcCostModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                aiSpyMissionsCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiSpyMissionsCostModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                aiDiplomacyCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiDiplomacyCostModifier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                aiInfluenceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiInfluenceModifier)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            }

            // AoD1.07以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                nationalismPerManpowerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismPerManpowerAoD)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                coreProvinceEfficiencyRiseTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.CoreProvinceEfficiencyRiseTime)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                restockSpeedLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedLand)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                restockSpeedAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedAir)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                restockSpeedNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.RestockSpeedNaval)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                spyCoupDissentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyCoupDissentModifier)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                convoyDutyConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyDutyConversion)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                escortDutyConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.EscortDutyConversion)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                tpMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.TpMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                ssMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.SsMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                ssnMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.SsnMaxAttach)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                ddMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.DdMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                clMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.ClMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                caMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CaMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                bcMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.BcMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                bbMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.BbMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                cvlMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvlMaxAttach)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                cvMaxAttachTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvMaxAttach)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            }

            // DDA/AoD固有項目
            if (Game.Type == GameType.HeartsOfIron2 || Game.Type == GameType.ArsenalOfDemocracy)
            {
                requirementAffectSliderTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequirementAffectSlider)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     経済2タブの編集項目の色を更新する
        /// </summary>
        private void UpdateEconomy2ItemColor()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                dissentChangeSpeedTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentChangeSpeed)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                gearingResourceIncrementTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingResourceIncrement)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                gearingLossNoIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingLossNoIc)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                costRepairBuildingsTextBox.ForeColor = Misc.IsDirty(MiscItemId.CostRepairBuildings)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                timeRepairBuildingTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeRepairBuilding)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                provinceEfficiencyRiseTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ProvinceEfficiencyRiseTime)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                lineUpkeepTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineUpkeep) ? Color.Red : SystemColors.WindowText;
                lineStartupTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineStartupTime)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                lineUpgradeTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.LineUpgradeTime)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                retoolingCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.RetoolingCost)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                retoolingResourceTextBox.ForeColor = Misc.IsDirty(MiscItemId.RetoolingResource)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                dailyAgingManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.DailyAgingManpower)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                supplyConvoyHuntTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyConvoyHunt)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                supplyNavalStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalStaticAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                supplyNavalMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalMoving)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                supplyNavalBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalBattleAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                supplyAirStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirStaticAoD)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                supplyAirMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirMoving)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                supplyAirBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBattleAoD)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                supplyAirBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBombing)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                supplyLandStaticAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandStaticAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                supplyLandMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandMoving)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                supplyLandBattleAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBattleAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                supplyLandBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBombing)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                supplyStockLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockLand)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                supplyStockAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockAir)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                supplyStockNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyStockNaval)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                syntheticOilConversionMultiplierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SyntheticOilConversionMultiplier) ? Color.Red : SystemColors.WindowText;
                syntheticRaresConversionMultiplierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SyntheticRaresConversionMultiplier) ? Color.Red : SystemColors.WindowText;
                militarySalaryTextBox.ForeColor = Misc.IsDirty(MiscItemId.MilitarySalary)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                maxIntelligenceExpenditureTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIntelligenceExpenditure)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                maxResearchExpenditureTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxResearchExpenditure)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                militarySalaryAttrictionModifierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MilitarySalaryAttrictionModifier) ? Color.Red : SystemColors.WindowText;
                militarySalaryDissentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MilitarySalaryDissentModifier)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                nuclearSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearSiteUpkeepCost)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                nuclearPowerUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearPowerUpkeepCost)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                syntheticOilSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticOilSiteUpkeepCost)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                syntheticRaresSiteUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SyntheticRaresSiteUpkeepCost)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                durationDetectionTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationDetection)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                convoyProvinceHostileTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyProvinceHostileTime)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                convoyProvinceBlockedTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyProvinceBlockedTime)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                autoTradeConvoyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AutoTradeConvoy)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                spyUpkeepCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyUpkeepCost)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                spyDetectionChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyDetectionChance)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                infraEfficiencyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.InfraEfficiencyModifier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                manpowerToConsumerGoodsTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerToConsumerGoods)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                timeBetweenSliderChangesAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeBetweenSliderChangesAoD)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                minimalPlacementIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinimalPlacementIc)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                nuclearPowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.NuclearPower)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                freeInfraRepairTextBox.ForeColor = Misc.IsDirty(MiscItemId.FreeInfraRepair)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                maxSliderDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxSliderDissent)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                minSliderDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinSliderDissent)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                maxDissentSliderMoveTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxDissentSliderMove)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                icConcentrationBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcConcentrationBonus)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                ministerChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinisterChangeDelay)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                ministerChangeEventDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinisterChangeEventDelay)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                ideaChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.IdeaChangeDelay)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                ideaChangeEventDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.IdeaChangeEventDelay)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                leaderChangeDelayTextBox.ForeColor = Misc.IsDirty(MiscItemId.LeaderChangeDelay)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                changeIdeaDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChangeIdeaDissent)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                changeMinisterDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChangeMinisterDissent)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                minDissentRevoltTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinDissentRevolt)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                dissentRevoltMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentRevoltMultiplier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            }

            // AoD1.04以前固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version <= 104)
            {
                transportConversionTextBox.ForeColor = Misc.IsDirty(MiscItemId.TransportConversion)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     経済3タブの編集項目の色を更新する
        /// </summary>
        private void UpdateEconomy3ItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                minAvailableIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinAvailableIc)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                minFinalIcTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinFinalIc) ? Color.Red : SystemColors.WindowText;
                dissentReductionTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentReduction)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                icMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcMultiplierPuppet)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                resourceMultiplierNonNationalTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierNonNational)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                resourceMultiplierNonOwnedTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierNonOwned)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                resourceMultiplierNonNationalAiTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ResourceMultiplierNonNationalAi) ? Color.Red : SystemColors.WindowText;
                resourceMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceMultiplierPuppet)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                manpowerMultiplierPuppetTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierPuppet)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                manpowerMultiplierWartimeOverseaTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ManpowerMultiplierWartimeOversea) ? Color.Red : SystemColors.WindowText;
                manpowerMultiplierPeacetimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierPeacetime)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                manpowerMultiplierWartimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerMultiplierWartime)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                dailyRetiredManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.DailyRetiredManpower)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                reinforceToUpdateModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReinforceToUpdateModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                nationalismPerManpowerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NationalismPerManpowerDh)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                maxNationalismTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxNationalism)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                maxRevoltRiskTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxRevoltRisk)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                provinceBuildingsRepairModifierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ProvinceBuildingsRepairModifier) ? Color.Red : SystemColors.WindowText;
                provinceResourceRepairModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ProvinceResourceRepairModifier)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                stockpileLimitMultiplierResourceTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.StockpileLimitMultiplierResource) ? Color.Red : SystemColors.WindowText;
                stockpileLimitMultiplierSuppliesOilTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.StockpileLimitMultiplierSuppliesOil) ? Color.Red : SystemColors.WindowText;
                overStockpileLimitDailyLossTextBox.ForeColor = Misc.IsDirty(MiscItemId.OverStockpileLimitDailyLoss)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                maxResourceDepotSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxResourceDepotSize)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                maxSuppliesOilDepotSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxSuppliesOilDepotSize)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                desiredStockPilesSuppliesOilTextBox.ForeColor = Misc.IsDirty(MiscItemId.DesiredStockPilesSuppliesOil)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                maxManpowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxManpower)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                convoyTransportsCapacityTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyTransportsCapacity)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                suppyLandStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SuppyLandStaticDh)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                supplyLandBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyLandBattleDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                fuelLandStaticTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelLandStatic)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                fuelLandBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelLandBattle)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                supplyAirStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirStaticDh)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                supplyAirBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyAirBattleDh)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                fuelAirNavalStaticTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelAirNavalStatic)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                fuelAirBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelAirBattle)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                supplyNavalStaticDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalStaticDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                supplyNavalBattleDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyNavalBattleDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                fuelNavalNotMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelNavalNotMoving)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                fuelNavalBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelNavalBattle)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                tpTransportsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.TpTransportsConversionRatio)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                ddEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.DdEscortsConversionRatio)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                clEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.ClEscortsConversionRatio)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                cvlEscortsConversionRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.CvlEscortsConversionRatio)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                gearingBonusLossUpgradeUnitTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusLossUpgradeUnit)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                gearingBonusLossUpgradeBrigadeTextBox.ForeColor = Misc.IsDirty(MiscItemId.GearingBonusLossUpgradeBrigade)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                dissentNukesTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentNukes)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                maxDailyDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxDailyDissent)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                nukesProductionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukesProductionModifier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                resourceConvoysBackUnneededTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResourceConvoysBackUnneeded)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     諜報タブの項目の色を更新する
        /// </summary>
        private void UpdateIntelligenceItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                spyMissionDaysDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyMissionDaysDh)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                increateIntelligenceLevelDaysDhTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.IncreateIntelligenceLevelDaysDh) ? Color.Red : SystemColors.WindowText;
                chanceDetectSpyMissionDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceDetectSpyMissionDh)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                relationshipsHitDetectedMissionsDhTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.RelationshipsHitDetectedMissionsDh) ? Color.Red : SystemColors.WindowText;
                distanceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifier)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                distanceModifierNeighboursDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DistanceModifierNeighboursDh)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                spyLevelBonusDistanceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpyLevelBonusDistanceModifier)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                spyLevelBonusDistanceModifierAboveTenTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SpyLevelBonusDistanceModifierAboveTen) ? Color.Red : SystemColors.WindowText;
                spyInformationAccuracyModifierDhTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SpyInformationAccuracyModifierDh) ? Color.Red : SystemColors.WindowText;
                icModifierCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcModifierCost)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                minIcCostModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinIcCostModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                maxIcCostModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxIcCostModifierDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                extraMaintenanceCostAboveTenTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraMaintenanceCostAboveTen)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                extraCostIncreasingAboveTenTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraCostIncreasingAboveTen)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                spiesMoneyModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SpiesMoneyModifier)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     外交タブの編集項目の色を更新する
        /// </summary>
        private void UpdateDiplomacyItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                daysBetweenDiplomaticMissionsTextBox.ForeColor = Misc.IsDirty(MiscItemId.DaysBetweenDiplomaticMissions)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                timeBetweenSliderChangesDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TimeBetweenSliderChangesDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                requirementAffectSliderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequirementAffectSliderDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                relationshipHitCancelTradeTextBox.ForeColor = Misc.IsDirty(MiscItemId.RelationshipHitCancelTrade)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                relationshipHitCancelPermanentTradeTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.RelationshipHitCancelPermanentTrade) ? Color.Red : SystemColors.WindowText;
                belligerenceClaimedProvinceTextBox.ForeColor = Misc.IsDirty(MiscItemId.BelligerenceClaimedProvince)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                belligerenceClaimsRemovalTextBox.ForeColor = Misc.IsDirty(MiscItemId.BelligerenceClaimsRemoval)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     戦闘1タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCombat1ItemColor()
        {
            landXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandXpGainFactor)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            navalXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalXpGainFactor)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            airXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirXpGainFactor)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            divisionXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.DivisionXpGainFactor)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            leaderXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.LeaderXpGainFactor)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
            attritionSeverityModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AttritionSeverityModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            baseProximityTextBox.ForeColor = Misc.IsDirty(MiscItemId.BaseProximity)
                                                 ? Color.Red
                                                 : SystemColors.WindowText;
            invasionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.InvasionModifier)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            multipleCombatModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MultipleCombatModifier)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            offensiveCombinedArmsBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.OffensiveCombinedArmsBonus)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            defensiveCombinedArmsBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefensiveCombinedArmsBonus)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            surpriseModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SurpriseModifier)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            landCommandLimitModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandCommandLimitModifier)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
            airCommandLimitModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirCommandLimitModifier)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            navalCommandLimitModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalCommandLimitModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            envelopmentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.EnvelopmentModifier)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            encircledModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncircledModifier)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            landFortMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandFortMultiplier)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
            coastalFortMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.CoastalFortMultiplier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            dissentMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.DissentMultiplier)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            raderStationMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.RaderStationMultiplier)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            interceptorBomberModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.InterceptorBomberModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            navalOverstackingModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOverstackingModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            landLeaderCommandLimitRank0TextBox.ForeColor = Misc.IsDirty(MiscItemId.LandLeaderCommandLimitRank0)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            landLeaderCommandLimitRank1TextBox.ForeColor = Misc.IsDirty(MiscItemId.LandLeaderCommandLimitRank1)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            landLeaderCommandLimitRank2TextBox.ForeColor = Misc.IsDirty(MiscItemId.LandLeaderCommandLimitRank2)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            landLeaderCommandLimitRank3TextBox.ForeColor = Misc.IsDirty(MiscItemId.LandLeaderCommandLimitRank3)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            airLeaderCommandLimitRank0TextBox.ForeColor = Misc.IsDirty(MiscItemId.AirLeaderCommandLimitRank0)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            airLeaderCommandLimitRank1TextBox.ForeColor = Misc.IsDirty(MiscItemId.AirLeaderCommandLimitRank1)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            airLeaderCommandLimitRank2TextBox.ForeColor = Misc.IsDirty(MiscItemId.AirLeaderCommandLimitRank2)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            airLeaderCommandLimitRank3TextBox.ForeColor = Misc.IsDirty(MiscItemId.AirLeaderCommandLimitRank3)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
            navalLeaderCommandLimitRank0TextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalLeaderCommandLimitRank0)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            navalLeaderCommandLimitRank1TextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalLeaderCommandLimitRank1)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            navalLeaderCommandLimitRank2TextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalLeaderCommandLimitRank2)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            navalLeaderCommandLimitRank3TextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalLeaderCommandLimitRank3)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            hqCommandLimitFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.HqCommandLimitFactor)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            convoyProtectionFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyProtectionFactor)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            delayAfterCombatEndsTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayAfterCombatEnds)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            maximumSizesAirStacksTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaximumSizesAirStacks)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            effectExperienceCombatTextBox.ForeColor = Misc.IsDirty(MiscItemId.EffectExperienceCombat)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            damageNavalBasesBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageNavalBasesBombing)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
            damageAirBaseBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageAirBaseBombing)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            damageAaBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageAaBombing)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            damageRocketBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageRocketBombing)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            damageNukeBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageNukeBombing)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            damageRadarBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageRadarBombing)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
            damageInfraBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageInfraBombing)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
            damageIcBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageIcBombing)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            damageResourcesBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageResourcesBombing)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
            chanceGetTerrainTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceGetTerrainTrait)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
            chanceGetEventTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceGetEventTrait)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
            bonusTerrainTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusTerrainTrait)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            bonusEventTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusEventTrait)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
            chanceLeaderDyingTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceLeaderDying)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;

            // AoDに存在しない項目
            if (Game.Type != GameType.ArsenalOfDemocracy)
            {
                airOverstackingModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOverstackingModifier)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            }

            // DHに存在しない項目
            if (Game.Type != GameType.DarkestHour)
            {
                shoreBombardmentModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ShoreBombardmentModifier)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                supplyProblemsModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyProblemsModifier)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                airOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamage)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            }

            // DH1.03以降に存在しない項目
            if (Game.Type != GameType.DarkestHour || Game.Version < 103)
            {
                howEffectiveGroundDefTextBox.ForeColor = Misc.IsDirty(MiscItemId.HowEffectiveGroundDef)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                chanceAvoidDefencesLeftTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceAvoidDefencesLeft)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                chanceAvoidNoDefencesTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceAvoidNoDefences)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            }

            // AODにもDHにも存在しない項目
            if (Game.Type != GameType.ArsenalOfDemocracy && Game.Type != GameType.DarkestHour)
            {
                airStrDamageOrgTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageOrg)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                airStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamage)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
            }

            // DDA1.3以降固有項目
            if (Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130)
            {
                subsOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsOrgDamage)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                subsStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsStrDamage)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
            }

            // DDA1.3以降またはDH固有項目
            if ((Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130) || Game.Type == GameType.DarkestHour)
            {
                subStacksDetectionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubStacksDetectionModifier)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     戦闘2タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCombat2ItemColor()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                noSupplyAttritionSeverityTextBox.ForeColor = Misc.IsDirty(MiscItemId.NoSupplyAttritionSeverity)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                noSupplyMinimunAttritionTextBox.ForeColor = Misc.IsDirty(MiscItemId.NoSupplyMinimunAttrition)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                raderStationAaMultiplierTextBox.ForeColor = Misc.IsDirty(MiscItemId.RaderStationAaMultiplier)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                airOverstackingModifierAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOverstackingModifierAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                landDelayBeforeOrdersTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandDelayBeforeOrders)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                navalDelayBeforeOrdersTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalDelayBeforeOrders)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                airDelayBeforeOrdersTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirDelayBeforeOrders)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                damageSyntheticOilBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DamageSyntheticOilBombing)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                airOrgDamageLandAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageLandAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                airStrDamageLandAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageLandAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                landDamageArtilleryBombardmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandDamageArtilleryBombardment)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                infraDamageArtilleryBombardmentTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.InfraDamageArtilleryBombardment) ? Color.Red : SystemColors.WindowText;
                icDamageArtilleryBombardmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.IcDamageArtilleryBombardment)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                resourcesDamageArtilleryBombardmentTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ResourcesDamageArtilleryBombardment) ? Color.Red : SystemColors.WindowText;
                penaltyArtilleryBombardmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.PenaltyArtilleryBombardment)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                artilleryStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.ArtilleryStrDamage)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                artilleryOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.ArtilleryOrgDamage)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                landStrDamageLandAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageLandAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                landOrgDamageLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageLand)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                landStrDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageAirAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                landOrgDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageAirAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                navalStrDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalStrDamageAirAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                navalOrgDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgDamageAirAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                airStrDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageAirAoD)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                airOrgDamageAirAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageAirAoD)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                navalStrDamageNavyAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalStrDamageNavyAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                navalOrgDamageNavyAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgDamageNavyAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                airStrDamageNavyAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageNavyAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                airOrgDamageNavyAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageNavyAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                militaryExpenseAttritionModifierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MilitaryExpenseAttritionModifier) ? Color.Red : SystemColors.WindowText;
                navalMinCombatTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalMinCombatTime)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                landMinCombatTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandMinCombatTime)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                airMinCombatTimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirMinCombatTime)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                landOverstackingModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOverstackingModifier)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                landOrgLossMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgLossMoving)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                airOrgLossMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgLossMoving)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                navalOrgLossMovingTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgLossMoving)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                supplyDistanceSeverityTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyDistanceSeverity)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                supplyBaseTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyBase) ? Color.Red : SystemColors.WindowText;
                landOrgGainTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgGain)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                airOrgGainTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgGain) ? Color.Red : SystemColors.WindowText;
                navalOrgGainTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgGain)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                nukeManpowerDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukeManpowerDissent)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                nukeIcDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukeIcDissent)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                nukeTotalDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukeTotalDissent)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                landFriendlyOrgGainTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandFriendlyOrgGain)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                airLandStockModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirLandStockModifier)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                scorchDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.ScorchDamage)
                                                    ? Color.Red
                                                    : SystemColors.WindowText;
                standGroundDissentTextBox.ForeColor = Misc.IsDirty(MiscItemId.StandGroundDissent)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                scorchGroundBelligerenceTextBox.ForeColor = Misc.IsDirty(MiscItemId.ScorchGroundBelligerence)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                defaultLandStackTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefaultLandStack)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                defaultNavalStackTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefaultNavalStack)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                defaultAirStackTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefaultAirStack)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                defaultRocketStackTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefaultRocketStack)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                fortDamageArtilleryBombardmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.FortDamageArtilleryBombardment)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                artilleryBombardmentOrgCostTextBox.ForeColor = Misc.IsDirty(MiscItemId.ArtilleryBombardmentOrgCost)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                landDamageFortTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandDamageFort)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                airRebaseFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirRebaseFactor)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                airMaxDisorganizedTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirMaxDisorganized)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                aaInflictedStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaInflictedStrDamage)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                aaInflictedOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaInflictedOrgDamage)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                aaInflictedFlyingDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaInflictedFlyingDamage)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                aaInflictedBombingDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaInflictedBombingDamage)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                hardAttackStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.HardAttackStrDamage)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                hardAttackOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.HardAttackOrgDamage)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                armorSoftBreakthroughMinTextBox.ForeColor = Misc.IsDirty(MiscItemId.ArmorSoftBreakthroughMin)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                armorSoftBreakthroughMaxTextBox.ForeColor = Misc.IsDirty(MiscItemId.ArmorSoftBreakthroughMax)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                navalCriticalHitChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalCriticalHitChance)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                navalCriticalHitEffectTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalCriticalHitEffect)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                landFortDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandFortDamage)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                portAttackSurpriseChanceDayTextBox.ForeColor = Misc.IsDirty(MiscItemId.PortAttackSurpriseChanceDay)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                portAttackSurpriseChanceNightTextBox.ForeColor = Misc.IsDirty(MiscItemId.PortAttackSurpriseChanceNight)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     戦闘3タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCombat3ItemColor()
        {
            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                portAttackSurpriseModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.PortAttackSurpriseModifier)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                radarAntiSurpriseChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.RadarAntiSurpriseChance)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                radarAntiSurpriseModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.RadarAntiSurpriseModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            }

            // AoD1.08以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108)
            {
                shoreBombardmentCapTextBox.ForeColor = Misc.IsDirty(MiscItemId.ShoreBombardmentCap)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                counterAttackStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackStrDefenderAoD)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                counterAttackOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackOrgDefenderAoD)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                counterAttackStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackStrAttackerAoD)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                counterAttackOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackOrgAttackerAoD)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                assaultStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultStrDefenderAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                assaultOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultOrgDefenderAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                assaultStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultStrAttackerAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                assaultOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultOrgAttackerAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                encirclementStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementStrDefenderAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                encirclementOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementOrgDefenderAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                encirclementStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementStrAttackerAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                encirclementOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementOrgAttackerAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                ambushStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushStrDefenderAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                ambushOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushOrgDefenderAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                ambushStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushStrAttackerAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                ambushOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushOrgAttackerAoD)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                delayStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayStrDefenderAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                delayOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayOrgDefenderAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                delayStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayStrAttackerAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                delayOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayOrgAttackerAoD)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                tacticalWithdrawStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawStrDefenderAoD)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                tacticalWithdrawOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawOrgDefenderAoD)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                tacticalWithdrawStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawStrAttackerAoD)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                tacticalWithdrawOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawOrgAttackerAoD)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                breakthroughStrDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughStrDefenderAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                breakthroughOrgDefenderAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughOrgDefenderAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                breakthroughStrAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughStrAttackerAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                breakthroughOrgAttackerAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughOrgAttackerAoD)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     戦闘4タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCombat4ItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                airDogfightXpGainFactorTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirDogfightXpGainFactor)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                hardUnitsAttackingUrbanPenaltyTextBox.ForeColor = Misc.IsDirty(MiscItemId.HardUnitsAttackingUrbanPenalty)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                supplyProblemsModifierLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyProblemsModifierLand)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                supplyProblemsModifierAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyProblemsModifierAir)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                supplyProblemsModifierNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyProblemsModifierNaval)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                fuelProblemsModifierLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelProblemsModifierLand)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                fuelProblemsModifierAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelProblemsModifierAir)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                fuelProblemsModifierNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.FuelProblemsModifierNaval)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                convoyEscortsModelTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyEscortsModel)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                durationAirToAirBattlesTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationAirToAirBattles)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                durationNavalPortBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationNavalPortBombing)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                durationStrategicBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationStrategicBombing)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                durationGroundAttackBombingTextBox.ForeColor = Misc.IsDirty(MiscItemId.DurationGroundAttackBombing)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                bonusSimilarTerrainTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusSimilarTerrainTrait)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                airStrDamageLandOrgTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageLandOrg)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                airOrgDamageLandDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageLandDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                airStrDamageLandDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageLandDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                landOrgDamageLandOrgTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageLandOrg)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                landStrDamageLandDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageLandDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                airOrgDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageAirDh)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                airStrDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageAirDh)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                landOrgDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageAirDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                landStrDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageAirDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                navalOrgDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgDamageAirDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                navalStrDamageAirDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalStrDamageAirDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                subsOrgDamageAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsOrgDamageAir)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                subsStrDamageAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsStrDamageAir)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                airOrgDamageNavyDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageNavyDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                airStrDamageNavyDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageNavyDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                navalOrgDamageNavyDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgDamageNavyDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                navalStrDamageNavyDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalStrDamageNavyDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                subsOrgDamageNavyTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsOrgDamageNavy)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                subsStrDamageNavyTextBox.ForeColor = Misc.IsDirty(MiscItemId.SubsStrDamageNavy)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                navalOrgDamageAaTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalOrgDamageAa)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                airOrgDamageAaTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirOrgDamageAa)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                airStrDamageAaTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageAa)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                aaAirNightModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaAirNightModifier)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                aaAirBonusRadarsTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaAirBonusRadars)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                movementBonusTerrainTraitTextBox.ForeColor = Misc.IsDirty(MiscItemId.MovementBonusTerrainTrait)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                movementBonusSimilarTerrainTraitTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MovementBonusSimilarTerrainTrait) ? Color.Red : SystemColors.WindowText;
                logisticsWizardEseBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.LogisticsWizardEseBonus)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                daysOffensiveSupplyTextBox.ForeColor = Misc.IsDirty(MiscItemId.DaysOffensiveSupply)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                orgRegainBonusFriendlyTextBox.ForeColor = Misc.IsDirty(MiscItemId.OrgRegainBonusFriendly)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                orgRegainBonusFriendlyCapTextBox.ForeColor = Misc.IsDirty(MiscItemId.OrgRegainBonusFriendlyCap)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                nightHoursWinterTextBox.ForeColor = Misc.IsDirty(MiscItemId.NightHoursWinter)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                nightHoursSpringFallTextBox.ForeColor = Misc.IsDirty(MiscItemId.NightHoursSpringFall)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                nightHoursSummerTextBox.ForeColor = Misc.IsDirty(MiscItemId.NightHoursSummer)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                recalculateLandArrivalTimesTextBox.ForeColor = Misc.IsDirty(MiscItemId.RecalculateLandArrivalTimes)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                synchronizeArrivalTimePlayerTextBox.ForeColor = Misc.IsDirty(MiscItemId.SynchronizeArrivalTimePlayer)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                synchronizeArrivalTimeAiTextBox.ForeColor = Misc.IsDirty(MiscItemId.SynchronizeArrivalTimeAi)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                landSpeedModifierCombatTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandSpeedModifierCombat)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                landSpeedModifierBombardmentTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandSpeedModifierBombardment)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                landSpeedModifierSupplyTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandSpeedModifierSupply)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                landSpeedModifierOrgTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandSpeedModifierOrg)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                landAirSpeedModifierFuelTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandAirSpeedModifierFuel)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                defaultSpeedFuelTextBox.ForeColor = Misc.IsDirty(MiscItemId.DefaultSpeedFuel)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                fleetSizeRangePenaltyRatioTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetSizeRangePenaltyRatio)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                fleetSizeRangePenaltyThretholdTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetSizeRangePenaltyThrethold)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                fleetSizeRangePenaltyMaxTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetSizeRangePenaltyMax)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                radarBonusDetectionTextBox.ForeColor = Misc.IsDirty(MiscItemId.RadarBonusDetection)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                bonusDetectionFriendlyTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusDetectionFriendly)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                screensCapitalRatioModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ScreensCapitalRatioModifier)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                chanceTargetNoOrgLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.ChanceTargetNoOrgLand)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                screenCapitalShipsTargetingTextBox.ForeColor = Misc.IsDirty(MiscItemId.ScreenCapitalShipsTargeting)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     戦闘5タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCombat5ItemColor()
        {
            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                landChanceAvoidDefencesLeftTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandChanceAvoidDefencesLeft)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                airChanceAvoidDefencesLeftTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirChanceAvoidDefencesLeft)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                navalChanceAvoidDefencesLeftTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalChanceAvoidDefencesLeft)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                landChanceAvoidNoDefencesTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandChanceAvoidNoDefences)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                airChanceAvoidNoDefencesTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirChanceAvoidNoDefences)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                navalChanceAvoidNoDefencesTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalChanceAvoidNoDefences)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                bonusLeaderSkillPointLandTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusLeaderSkillPointLand)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                bonusLeaderSkillPointAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusLeaderSkillPointAir)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                bonusLeaderSkillPointNavalTextBox.ForeColor = Misc.IsDirty(MiscItemId.BonusLeaderSkillPointNaval)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                landMinOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandMinOrgDamage)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                landOrgDamageHardSoftEachTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageHardSoftEach)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                landOrgDamageHardVsSoftTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageHardVsSoft)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                landMinStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandMinStrDamage)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                landStrDamageHardSoftEachTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageHardSoftEach)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                landStrDamageHardVsSoftTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandStrDamageHardVsSoft)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                airMinOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirMinOrgDamage)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                airAdditionalOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirAdditionalOrgDamage)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                airMinStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirMinStrDamage)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                airAdditionalStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirAdditionalStrDamage)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                airStrDamageEntrencedTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirStrDamageEntrenced)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                navalMinOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalMinOrgDamage)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                navalAdditionalOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalAdditionalOrgDamage)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                navalMinStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalMinStrDamage)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                navalAdditionalStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalAdditionalStrDamage)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                landOrgDamageLandUrbanTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageLandUrban)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                landOrgDamageLandFortTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgDamageLandFort)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                requiredLandFortSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequiredLandFortSize)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                fleetPositioningDaytimeTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetPositioningDaytime)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                fleetPositioningLeaderSkillTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetPositioningLeaderSkill)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                fleetPositioningFleetSizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.FleetPositioningFleetSize)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                fleetPositioningFleetCompositionTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.FleetPositioningFleetComposition) ? Color.Red : SystemColors.WindowText;
                landCoastalFortsDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandCoastalFortsDamage)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                landCoastalFortsMaxDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandCoastalFortsMaxDamage)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                minSoftnessBrigadesTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinSoftnessBrigades)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                autoRetreatOrgTextBox.ForeColor = Misc.IsDirty(MiscItemId.AutoRetreatOrg)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                landOrgNavalTransportationTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandOrgNavalTransportation)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                maxLandDigTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxLandDig) ? Color.Red : SystemColors.WindowText;
                digIncreaseDayTextBox.ForeColor = Misc.IsDirty(MiscItemId.DigIncreaseDay)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                breakthroughEncirclementMinSpeedTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.BreakthroughEncirclementMinSpeed) ? Color.Red : SystemColors.WindowText;
                breakthroughEncirclementMaxChanceTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.BreakthroughEncirclementMaxChance) ? Color.Red : SystemColors.WindowText;
                breakthroughEncirclementChanceModifierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.BreakthroughEncirclementChanceModifier)
                        ? Color.Red
                        : SystemColors.WindowText;
                combatEventDurationTextBox.ForeColor = Misc.IsDirty(MiscItemId.CombatEventDuration)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                counterAttackOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackOrgAttackerDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                counterAttackStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackStrAttackerDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                counterAttackOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackOrgDefenderDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                counterAttackStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.CounterAttackStrDefenderDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                assaultOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultOrgAttackerDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                assaultStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultStrAttackerDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                assaultOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultOrgDefenderDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                assaultStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AssaultStrDefenderDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                encirclementOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementOrgAttackerDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                encirclementStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementStrAttackerDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                encirclementOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementOrgDefenderDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                encirclementStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.EncirclementStrDefenderDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                ambushOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushOrgAttackerDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                ambushStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushStrAttackerDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                ambushOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushOrgDefenderDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                ambushStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AmbushStrDefenderDh)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                delayOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayOrgAttackerDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                delayStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayStrAttackerDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                delayOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayOrgDefenderDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                delayStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayStrDefenderDh)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                tacticalWithdrawOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawOrgAttackerDh)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                tacticalWithdrawStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawStrAttackerDh)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                tacticalWithdrawOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawOrgDefenderDh)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                tacticalWithdrawStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.TacticalWithdrawStrDefenderDh)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                breakthroughOrgAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughOrgAttackerDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                breakthroughStrAttackerDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughStrAttackerDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                breakthroughOrgDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughOrgDefenderDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                breakthroughStrDefenderDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.BreakthroughStrDefenderDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     任務1タブの編集項目の色を更新する
        /// </summary>
        private void UpdateMission1ItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                attackStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AttackStartingEfficiency)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                attackSpeedBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.AttackSpeedBonus)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                rebaseStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebaseStartingEfficiency)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                rebaseChanceDetectedTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebaseChanceDetected)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                stratRedeployStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.StratRedeployStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                stratRedeployAddedValueTextBox.ForeColor = Misc.IsDirty(MiscItemId.StratRedeployAddedValue)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                stratRedeployDistanceMultiplierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.StratRedeployDistanceMultiplier) ? Color.Red : SystemColors.WindowText;
                supportAttackStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SupportAttackStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                supportAttackSpeedBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupportAttackSpeedBonus)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                supportDefenseStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.SupportDefenseStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                supportDefenseSpeedBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupportDefenseSpeedBonus)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                reservesStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReservesStartingEfficiency)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                reservesSpeedBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReservesSpeedBonus)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                antiPartisanDutyStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.AntiPartisanDutyStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                antiPartisanDutySuppressionTextBox.ForeColor = Misc.IsDirty(MiscItemId.AntiPartisanDutySuppression)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                plannedDefenseStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.PlannedDefenseStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                airSuperiorityStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.AirSuperiorityStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                airSuperiorityDetectionTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirSuperiorityDetection)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                airSuperiorityMinRequiredTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirSuperiorityMinRequired)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                groundAttackStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.GroundAttackStartingEfficiency)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                groundAttackOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.GroundAttackOrgDamage)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                groundAttackStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.GroundAttackStrDamage)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                interdictionStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.InterdictionStartingEfficiency)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                interdictionOrgDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.InterdictionOrgDamage)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                interdictionStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.InterdictionStrDamage)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                strategicBombardmentStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.StrategicBombardmentStartingEfficiency)
                        ? Color.Red
                        : SystemColors.WindowText;
                logisticalStrikeStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.LogisticalStrikeStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                runwayCrateringStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.RunwayCrateringStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                installationStrikeStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.InstallationStrikeStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                navalStrikeStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalStrikeStartingEfficiency)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                portStrikeStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.PortStrikeStartingEfficiency)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                convoyAirRaidingStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ConvoyAirRaidingStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                airSupplyStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirSupplyStartingEfficiency)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                airborneAssaultStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.AirborneAssaultStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                nukeStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.NukeStartingEfficiency)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                airScrambleStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirScrambleStartingEfficiency)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                airScrambleDetectionTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirScrambleDetection)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                airScrambleMinRequiredTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirScrambleMinRequired)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                convoyRadingStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyRadingStartingEfficiency)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                convoyRadingRangeModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyRadingRangeModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                convoyRadingChanceDetectedTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyRadingChanceDetected)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                aswStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.AswStartingEfficiency)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                navalInterdictionStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NavalInterdictionStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                shoreBombardmentStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ShoreBombardmentStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                shoreBombardmentModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.ShoreBombardmentModifierDh)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     任務2タブの編集項目の色を更新する
        /// </summary>
        private void UpdateMission2ItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                amphibousAssaultStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.AmphibousAssaultStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                seaTransportStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.SeaTransportStartingEfficiency)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                seaTransportRangeModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SeaTransportRangeModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                seaTransportChanceDetectedTextBox.ForeColor = Misc.IsDirty(MiscItemId.SeaTransportChanceDetected)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                NavalCombatPatrolStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NavalCombatPatrolStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                navalPortStrikeStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NavalPortStrikeStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                navalAirbaseStrikeStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NavalAirbaseStrikeStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                sneakMoveStartingEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.SneakMoveStartingEfficiency)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                sneakMoveRangeModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SneakMoveRangeModifier)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                sneakMoveChanceDetectedTextBox.ForeColor = Misc.IsDirty(MiscItemId.SneakMoveChanceDetected)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                navalScrambleStartingEfficiencyTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NavalScrambleStartingEfficiency) ? Color.Red : SystemColors.WindowText;
                navalScrambleSpeedBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalScrambleSpeedBonus)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     国家タブの編集項目の色を更新する
        /// </summary>
        private void UpdateCountryItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                landFortEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandFortEfficiency)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                coastalFortEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.CoastalFortEfficiency)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                groundDefenseEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.GroundDefenseEfficiency)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                convoyDefenseEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoyDefenseEfficiency)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                manpowerBoostTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerBoost)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                transportCapacityModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.TransportCapacityModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                occupiedTransportCapacityModifierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.OccupiedTransportCapacityModifier) ? Color.Red : SystemColors.WindowText;
                attritionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.AttritionModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                manpowerTrickleBackModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ManpowerTrickleBackModifier)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                supplyDistanceModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyDistanceModifier)
                                                              ? Color.Red
                                                              : SystemColors.WindowText;
                repairModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.RepairModifier)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                researchModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ResearchModifier)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                radarEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.RadarEfficiency)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                hqSupplyEfficiencyBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.HqSupplyEfficiencyBonus)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                hqCombatEventsBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.HqCombatEventsBonus)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                combatEventChancesTextBox.ForeColor = Misc.IsDirty(MiscItemId.CombatEventChances)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                friendlyArmyDetectionChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.FriendlyArmyDetectionChance)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                enemyArmyDetectionChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.EnemyArmyDetectionChance)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                friendlyIntelligenceChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.FriendlyIntelligenceChance)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                enemyIntelligenceChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.EnemyIntelligenceChance)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                maxAmphibiousArmySizeTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxAmphibiousArmySize)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                energyToOilTextBox.ForeColor = Misc.IsDirty(MiscItemId.EnergyToOil)
                                                   ? Color.Red
                                                   : SystemColors.WindowText;
                totalProductionEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.TotalProductionEfficiency)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                supplyProductionEfficiencyTextBox.ForeColor = Misc.IsDirty(MiscItemId.SupplyProductionEfficiency)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                aaPowerTextBox.ForeColor = Misc.IsDirty(MiscItemId.AaPower) ? Color.Red : SystemColors.WindowText;
                airSurpriseChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.AirSurpriseChance)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                landSurpriseChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.LandSurpriseChance)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                navalSurpriseChanceTextBox.ForeColor = Misc.IsDirty(MiscItemId.NavalSurpriseChance)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                peacetimeIcModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.PeacetimeIcModifier)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                wartimeIcModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.WartimeIcModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                buildingsProductionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.BuildingsProductionModifier)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                convoysProductionModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.ConvoysProductionModifier)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                minShipsPositioningBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinShipsPositioningBattle)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                maxShipsPositioningBattleTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxShipsPositioningBattle)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                peacetimeStockpilesResourcesTextBox.ForeColor = Misc.IsDirty(MiscItemId.PeacetimeStockpilesResources)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                wartimeStockpilesResourcesTextBox.ForeColor = Misc.IsDirty(MiscItemId.WartimeStockpilesResources)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                peacetimeStockpilesOilSuppliesTextBox.ForeColor = Misc.IsDirty(MiscItemId.PeacetimeStockpilesOilSupplies)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                wartimeStockpilesOilSuppliesTextBox.ForeColor = Misc.IsDirty(MiscItemId.WartimeStockpilesOilSupplies)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     研究タブの編集項目の色を更新する
        /// </summary>
        private void UpdateResearchItemColor()
        {
            blueprintBonusTextBox.ForeColor = Misc.IsDirty(MiscItemId.BlueprintBonus)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            preHistoricalDateModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.PreHistoricalDateModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            costSkillLevelTextBox.ForeColor = Misc.IsDirty(MiscItemId.CostSkillLevel)
                                                  ? Color.Red
                                                  : SystemColors.WindowText;
            meanNumberInventionEventsYearTextBox.ForeColor = Misc.IsDirty(MiscItemId.MeanNumberInventionEventsYear)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;

            // AoD固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                postHistoricalDateModifierAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.PostHistoricalDateModifierAoD)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                techSpeedModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.TechSpeedModifier)
                                                         ? Color.Red
                                                         : SystemColors.WindowText;
                preHistoricalPenaltyLimitTextBox.ForeColor = Misc.IsDirty(MiscItemId.PreHistoricalPenaltyLimit)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                postHistoricalBonusLimitTextBox.ForeColor = Misc.IsDirty(MiscItemId.PostHistoricalBonusLimit)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            }

            // AoD1.07以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 107)
            {
                maxActiveTechTeamsAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxActiveTechTeamsAoD)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                requiredIcEachTechTeamAoDTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequiredIcEachTechTeamAoD)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
            }

            // AoD1.08以降固有項目
            if (Game.Type == GameType.ArsenalOfDemocracy && Game.Version >= 108)
            {
                maximumRandomModifierTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaximumRandomModifier)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            }

            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                postHistoricalDateModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.PostHistoricalDateModifierDh)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                maxActiveTechTeamsDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxActiveTechTeamsDh)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                minActiveTechTeamsTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinActiveTechTeams)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                requiredIcEachTechTeamDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.RequiredIcEachTechTeamDh)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                maxTechTeamSkillTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxTechTeamSkill)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     貿易タブの編集項目の色を更新する
        /// </summary>
        private void UpdateTradeItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                daysTradeOffersTextBox.ForeColor = Misc.IsDirty(MiscItemId.DaysTradeOffers)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                delayGameStartNewTradesTextBox.ForeColor = Misc.IsDirty(MiscItemId.DelayGameStartNewTrades)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                limitAiNewTradesGameStartTextBox.ForeColor = Misc.IsDirty(MiscItemId.LimitAiNewTradesGameStart)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                desiredOilStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.DesiredOilStockpile)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                criticalOilStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.CriticalOilStockpile)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                desiredSuppliesStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.DesiredSuppliesStockpile)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                criticalSuppliesStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.CriticalSuppliesStockpile)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                desiredResourcesStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.DesiredResourcesStockpile)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                criticalResourceStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.CriticalResourceStockpile)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                wartimeDesiredStockpileMultiplierTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.WartimeDesiredStockpileMultiplier) ? Color.Red : SystemColors.WindowText;
                peacetimeExtraOilImportTextBox.ForeColor = Misc.IsDirty(MiscItemId.PeacetimeExtraOilImport)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                wartimeExtraOilImportTextBox.ForeColor = Misc.IsDirty(MiscItemId.WartimeExtraOilImport)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                extraImportBelowDesiredTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraImportBelowDesired)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                percentageProducedSuppliesTextBox.ForeColor = Misc.IsDirty(MiscItemId.PercentageProducedSupplies)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                percentageProducedMoneyTextBox.ForeColor = Misc.IsDirty(MiscItemId.PercentageProducedMoney)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                extraImportStockpileSelectedTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraImportStockpileSelected)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                daysDeliverResourcesTradesTextBox.ForeColor = Misc.IsDirty(MiscItemId.DaysDeliverResourcesTrades)
                                                                  ? Color.Red
                                                                  : SystemColors.WindowText;
                puppetsSendSuppliesMoneyTextBox.ForeColor = Misc.IsDirty(MiscItemId.PuppetsSendSuppliesMoney)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                puppetsCriticalSupplyStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.PuppetsCriticalSupplyStockpile)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                puppetsMaxPoolResourcesTextBox.ForeColor = Misc.IsDirty(MiscItemId.PuppetsMaxPoolResources)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                newTradeDealsMinEffectivenessTextBox.ForeColor = Misc.IsDirty(MiscItemId.NewTradeDealsMinEffectiveness)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                cancelTradeDealsEffectivenessTextBox.ForeColor = Misc.IsDirty(MiscItemId.CancelTradeDealsEffectiveness)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                autoTradeAiTradeDealsTextBox.ForeColor = Misc.IsDirty(MiscItemId.AutoTradeAiTradeDeals)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     AIタブの編集項目の色を更新する
        /// </summary>
        private void UpdateAiItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                overproduceSuppliesBelowDesiredTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.OverproduceSuppliesBelowDesired) ? Color.Red : SystemColors.WindowText;
                multiplierOverproduceSuppliesWarTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MultiplierOverproduceSuppliesWar) ? Color.Red : SystemColors.WindowText;
                notProduceSuppliesStockpileOverTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NotProduceSuppliesStockpileOver) ? Color.Red : SystemColors.WindowText;
                maxSerialLineProductionGarrisonMilitiaTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MaxSerialLineProductionGarrisonMilitia)
                        ? Color.Red
                        : SystemColors.WindowText;
                minIcSerialProductionNavalAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinIcSerialProductionNavalAir)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                notProduceNewUnitsManpowerRatioTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NotProduceNewUnitsManpowerRatio) ? Color.Red : SystemColors.WindowText;
                notProduceNewUnitsManpowerValueTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.NotProduceNewUnitsManpowerValue) ? Color.Red : SystemColors.WindowText;
                notProduceNewUnitsSupplyTextBox.ForeColor = Misc.IsDirty(MiscItemId.NotProduceNewUnitsSupply)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
                militaryStrengthTotalIcRatioPeacetimeTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MilitaryStrengthTotalIcRatioPeacetime) ? Color.Red : SystemColors.WindowText;
                militaryStrengthTotalIcRatioWartimeTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MilitaryStrengthTotalIcRatioWartime) ? Color.Red : SystemColors.WindowText;
                militaryStrengthTotalIcRatioMajorTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MilitaryStrengthTotalIcRatioMajor) ? Color.Red : SystemColors.WindowText;
                notUseOffensiveSupplyStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.NotUseOffensiveSupplyStockpile)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                notUseOffensiveOilStockpileTextBox.ForeColor = Misc.IsDirty(MiscItemId.NotUseOffensiveOilStockpile)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                notUseOffensiveEseTextBox.ForeColor = Misc.IsDirty(MiscItemId.NotUseOffensiveEse)
                                                          ? Color.Red
                                                          : SystemColors.WindowText;
                notUseOffensiveOrgStrDamageTextBox.ForeColor = Misc.IsDirty(MiscItemId.NotUseOffensiveOrgStrDamage)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                aiSpyMissionsCostModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiSpyMissionsCostModifierDh)
                                                                   ? Color.Red
                                                                   : SystemColors.WindowText;
                aiDiplomacyCostModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiDiplomacyCostModifierDh)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                aiInfluenceModifierDhTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiInfluenceModifierDh)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                forcePuppetsJoinMastersAllianceNeutralityTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ForcePuppetsJoinMastersAllianceNeutrality)
                        ? Color.Red
                        : SystemColors.WindowText;
                aiEventsActionSelectionRulesTextBox.ForeColor = Misc.IsDirty(MiscItemId.AiEventsActionSelectionRules)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                forceStrategicRedeploymentHourTextBox.ForeColor = Misc.IsDirty(MiscItemId.ForceStrategicRedeploymentHour)
                                                                      ? Color.Red
                                                                      : SystemColors.WindowText;
                maxRedeploymentDaysAiTextBox.ForeColor = Misc.IsDirty(MiscItemId.MaxRedeploymentDaysAi)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                minDaysRequiredAiReleaseCountryTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MinDaysRequiredAiReleaseCountry) ? Color.Red : SystemColors.WindowText;
                minDaysRequiredAiAlliedTextBox.ForeColor = Misc.IsDirty(MiscItemId.MinDaysRequiredAiAllied)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                minDaysRequiredAiAlliedSupplyBaseTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MinDaysRequiredAiAlliedSupplyBase) ? Color.Red : SystemColors.WindowText;
                minRequiredRelationsAlliedClaimedTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.MinRequiredRelationsAlliedClaimed) ? Color.Red : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     MODタブの編集項目の色を更新する
        /// </summary>
        private void UpdateModItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                countryLoggerTextBox.ForeColor = Misc.IsDirty(MiscItemId.CountryLogger)
                                                     ? Color.Red
                                                     : SystemColors.WindowText;
                tradeEfficiencyCalculationSystemTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.TradeEfficiencyCalculationSystem) ? Color.Red : SystemColors.WindowText;
                mergeRelocateProvincialDepotsTextBox.ForeColor = Misc.IsDirty(MiscItemId.MergeRelocateProvincialDepots)
                                                                     ? Color.Red
                                                                     : SystemColors.WindowText;
                multipleDeploymentSizeArmiesTextBox.ForeColor = Misc.IsDirty(MiscItemId.MultipleDeploymentSizeArmies)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                multipleDeploymentSizeFleetsTextBox.ForeColor = Misc.IsDirty(MiscItemId.MultipleDeploymentSizeFleets)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                multipleDeploymentSizeAirTextBox.ForeColor = Misc.IsDirty(MiscItemId.MultipleDeploymentSizeAir)
                                                                 ? Color.Red
                                                                 : SystemColors.WindowText;
                rebelsArmyCompositionTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebelsArmyComposition)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                rebelsArmyTechLevelTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebelsArmyTechLevel)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                rebelsArmyMinStrTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebelsArmyMinStr)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                rebelsArmyMaxStrTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebelsArmyMaxStr)
                                                        ? Color.Red
                                                        : SystemColors.WindowText;
                rebelsOrgRegainTextBox.ForeColor = Misc.IsDirty(MiscItemId.RebelsOrgRegain)
                                                       ? Color.Red
                                                       : SystemColors.WindowText;
                extraRebelBonusNeighboringProvinceTextBox.ForeColor =
                    Misc.IsDirty(MiscItemId.ExtraRebelBonusNeighboringProvince) ? Color.Red : SystemColors.WindowText;
                extraRebelBonusOccupiedTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusOccupied)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                extraRebelBonusMountainTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusMountain)
                                                               ? Color.Red
                                                               : SystemColors.WindowText;
                extraRebelBonusHillTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusHill)
                                                           ? Color.Red
                                                           : SystemColors.WindowText;
                extraRebelBonusForestTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusForest)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                extraRebelBonusJungleTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusJungle)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                extraRebelBonusSwampTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusSwamp)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                extraRebelBonusDesertTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusDesert)
                                                             ? Color.Red
                                                             : SystemColors.WindowText;
                extraRebelBonusPlainTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusPlain)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                extraRebelBonusUrbanTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusUrban)
                                                            ? Color.Red
                                                            : SystemColors.WindowText;
                extraRebelBonusAirNavalBasesTextBox.ForeColor = Misc.IsDirty(MiscItemId.ExtraRebelBonusAirNavalBases)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
                returnRebelliousProvinceTextBox.ForeColor = Misc.IsDirty(MiscItemId.ReturnRebelliousProvince)
                                                                ? Color.Red
                                                                : SystemColors.WindowText;
            }

            // DH1.03以降固有項目
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                unitModifiersStatisticsPagesTextBox.ForeColor = Misc.IsDirty(MiscItemId.UnitModifiersStatisticsPages)
                                                                    ? Color.Red
                                                                    : SystemColors.WindowText;
            }
        }

        /// <summary>
        ///     マップタブの編集項目の色を更新する
        /// </summary>
        private void UpdateMapItemColor()
        {
            // DH固有項目
            if (Game.Type == GameType.DarkestHour)
            {
                mapNumberTextBox.ForeColor = Misc.IsDirty(MiscItemId.MapNumber) ? Color.Red : SystemColors.WindowText;
                totalProvincesTextBox.ForeColor = Misc.IsDirty(MiscItemId.TotalProvinces)
                                                      ? Color.Red
                                                      : SystemColors.WindowText;
                MapWidthTextBox.ForeColor = Misc.IsDirty(MiscItemId.MapWidth) ? Color.Red : SystemColors.WindowText;
                MapHeightTextBox.ForeColor = Misc.IsDirty(MiscItemId.MapHeight) ? Color.Red : SystemColors.WindowText;
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
            if (!Misc.IsDirty())
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
                    SaveFiles();
                    break;
            }
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
            // miscファイルの再読み込みを要求する
            Misc.RequireReload();

            // miscファイルを読み込む
            LoadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFiles();
        }

        /// <summary>
        ///     基本データファイルを読み込む
        /// </summary>
        private void LoadFiles()
        {
            // 基本データファイルを読み込む
            Misc.Load();

            // 編集項目を初期化する
            InitEditableItems();

            // 編集項目の値を更新する
            UpdateEditableItemsValue();

            // 編集項目の色を更新する
            UpdateEditableItemsColor();
        }

        /// <summary>
        ///     基本データファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 基本データファイルを保存する
            Misc.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            UpdateEditableItemsColor();
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
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
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
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
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
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.NonNegInt:
                case MiscItemType.NonNegInt1:
                    if (i < 0)
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.NonPosInt:
                    if (i > 0)
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.NonNegIntMinusOne:
                    if (i < 0 && i != -1)
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedInt:
                    if (i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id])
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedPosInt:
                    if (i < Misc.ItemMinValues[(int) id])
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusOne:
                    if ((i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id]) && i != -1)
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusThree:
                    if ((i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id]) && i != -1 && i != -2 &&
                        i != -3)
                    {
                        textBox.Text = ((int) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.PosDbl:
                    if (d <= 0)
                    {
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
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
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
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
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                    if (d < 0 && Math.Abs(d - (-1)) > 0.00005)
                    {
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDbl0:
                    if (d < Misc.ItemMinValues[(int) id] || d > Misc.ItemMaxValues[(int) id])
                    {
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    break;

                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                    if ((d < Misc.ItemMinValues[(int) id] || d > Misc.ItemMaxValues[(int) id]) &&
                        Math.Abs(d - (-1)) > 0.00005)
                    {
                        textBox.Text = ((double) Misc.GetItem(id)).ToString(CultureInfo.InvariantCulture);
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
            if ((e.Index + (int) Misc.ItemMinValues[(int) id] == index) && Misc.IsDirty(id))
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
                    i = comboBox.SelectedIndex + (int) Misc.ItemMinValues[(int) id];
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