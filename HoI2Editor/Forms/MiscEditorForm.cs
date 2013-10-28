using System;
using System.Drawing;
using System.Linq;
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
        private Control[] _controls;

        /// <summary>
        ///     項目IDと編集ラベルとの対応付け
        /// </summary>
        private Label[] _labels;

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
            // 編集項目を初期化する
            InitEditableItems();

            // miscファイルを読み込む
            LoadFiles();

            // フォームを前面に表示する
            Activate();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 項目IDと編集ラベルを対応付ける
            _labels = new[]
                {
                    icToTcRatioLabel,
                    icToSuppliesRatioLabel,
                    icToConsumerGoodsRatioLabel,
                    icToMoneyRatioLabel,
                    dissentChangeSpeedLabel,
                    minAvailableIcLabel,
                    minFinalIcLabel,
                    dissentReductionLabel,
                    maxGearingBonusLabel,
                    gearingBonusIncrementLabel,
                    gearingResourceIncrementLabel,
                    gearingLossNoIcLabel,
                    icMultiplierNonNationalLabel,
                    icMultiplierNonOwnedLabel,
                    icMultiplierPuppetLabel,
                    resourceMultiplierNonNationalLabel,
                    resourceMultiplierNonOwnedLabel,
                    resourceMultiplierNonNationalAiLabel,
                    resourceMultiplierPuppetLabel,
                    tcLoadUndeployedDivisionLabel,
                    tcLoadOccupiedLabel,
                    tcLoadMultiplierLandLabel,
                    tcLoadMultiplierAirLabel,
                    tcLoadMultiplierNavalLabel,
                    tcLoadPartisanLabel,
                    tcLoadFactorOffensiveLabel,
                    tcLoadProvinceDevelopmentLabel,
                    tcLoadBaseLabel,
                    manpowerMultiplierNationalLabel,
                    manpowerMultiplierNonNationalLabel,
                    manpowerMultiplierColonyLabel,
                    manpowerMultiplierPuppetLabel,
                    manpowerMultiplierWartimeOverseaLabel,
                    manpowerMultiplierPeacetimeLabel,
                    manpowerMultiplierWartimeLabel,
                    dailyRetiredManpowerLabel,
                    requirementAffectSliderLabel,
                    trickleBackFactorManpowerLabel,
                    reinforceManpowerLabel,
                    reinforceCostLabel,
                    reinforceTimeLabel,
                    upgradeCostLabel,
                    upgradeTimeLabel,
                    reinforceToUpdateModifierLabel,
                    nationalismStartingValueLabel,
                    nationalismPerManpowerAoDLabel,
                    nationalismPerManpowerDhLabel,
                    maxNationalismLabel,
                    maxRevoltRiskLabel,
                    monthlyNationalismReductionLabel,
                    sendDivisionDaysLabel,
                    tcLoadUndeployedBrigadeLabel,
                    canUnitSendNonAlliedLabel,
                    spyMissionDaysLabel,
                    increateIntelligenceLevelDaysLabel,
                    chanceDetectSpyMissionLabel,
                    relationshipsHitDetectedMissionsLabel,
                    showThirdCountrySpyReportsLabel,
                    distanceModifierNeighboursLabel,
                    spyInformationAccuracyModifierLabel,
                    aiPeacetimeSpyMissionsLabel,
                    maxIcCostModifierLabel,
                    aiSpyMissionsCostModifierLabel,
                    aiDiplomacyCostModifierLabel,
                    aiInfluenceModifierLabel,
                    costRepairBuildingsLabel,
                    timeRepairBuildingLabel,
                    provinceEfficiencyRiseTimeLabel,
                    coreProvinceEfficiencyRiseTimeLabel,
                    lineUpkeepLabel,
                    lineStartupTimeLabel,
                    lineUpgradeTimeLabel,
                    retoolingCostLabel,
                    retoolingResourceLabel,
                    dailyAgingManpowerLabel,
                    supplyConvoyHuntLabel,
                    supplyNavalStaticAoDLabel,
                    supplyNavalMovingLabel,
                    supplyNavalBattleAoDLabel,
                    supplyAirStaticAoDLabel,
                    supplyAirMovingLabel,
                    supplyAirBattleAoDLabel,
                    supplyAirBombingLabel,
                    supplyLandStaticAoDLabel,
                    supplyLandMovingLabel,
                    supplyLandBattleAoDLabel,
                    supplyLandBombingLabel,
                    supplyStockLandLabel,
                    supplyStockAirLabel,
                    supplyStockNavalLabel,
                    restockSpeedLandLabel,
                    restockSpeedAirLabel,
                    restockSpeedNavalLabel,
                    syntheticOilConversionMultiplierLabel,
                    syntheticRaresConversionMultiplierLabel,
                    militarySalaryLabel,
                    maxIntelligenceExpenditureLabel,
                    maxResearchExpenditureLabel,
                    militarySalaryAttrictionModifierLabel,
                    militarySalaryDissentModifierLabel,
                    nuclearSiteUpkeepCostLabel,
                    nuclearPowerUpkeepCostLabel,
                    syntheticOilSiteUpkeepCostLabel,
                    syntheticRaresSiteUpkeepCostLabel,
                    durationDetectionLabel,
                    convoyProvinceHostileTimeLabel,
                    convoyProvinceBlockedTimeLabel,
                    autoTradeConvoyLabel,
                    spyUpkeepCostLabel,
                    spyDetectionChanceLabel,
                    spyCoupDissentModifierLabel,
                    infraEfficiencyModifierLabel,
                    manpowerToConsumerGoodsLabel,
                    timeBetweenSliderChangesAoDLabel,
                    minimalPlacementIcLabel,
                    nuclearPowerLabel,
                    freeInfraRepairLabel,
                    maxSliderDissentLabel,
                    minSliderDissentLabel,
                    maxDissentSliderMoveLabel,
                    icConcentrationBonusLabel,
                    transportConversionLabel,
                    convoyDutyConversionLabel,
                    escortDutyConversionLabel,
                    ministerChangeDelayLabel,
                    ministerChangeEventDelayLabel,
                    ideaChangeDelayLabel,
                    ideaChangeEventDelayLabel,
                    leaderChangeDelayLabel,
                    changeIdeaDissentLabel,
                    changeMinisterDissentLabel,
                    minDissentRevoltLabel,
                    dissentRevoltMultiplierLabel,
                    tpMaxAttachLabel,
                    ssMaxAttachLabel,
                    ssnMaxAttachLabel,
                    ddMaxAttachLabel,
                    clMaxAttachLabel,
                    caMaxAttachLabel,
                    bcMaxAttachLabel,
                    bbMaxAttachLabel,
                    cvlMaxAttachLabel,
                    cvMaxAttachLabel,
                    canChangeIdeasLabel,
                    canUnitSendNonAlliedDhLabel,
                    bluePrintsCanSoldNonAlliedLabel,
                    provinceCanSoldNonAlliedLabel,
                    transferAlliedCoreProvincesLabel,
                    provinceBuildingsRepairModifierLabel,
                    provinceResourceRepairModifierLabel,
                    stockpileLimitMultiplierResourceLabel,
                    stockpileLimitMultiplierSuppliesOilLabel,
                    overStockpileLimitDailyLossLabel,
                    maxResourceDepotSizeLabel,
                    maxSuppliesOilDepotSizeLabel,
                    desiredStockPilesSuppliesOilLabel,
                    maxManpowerLabel,
                    convoyTransportsCapacityLabel,
                    suppyLandStaticDhLabel,
                    supplyLandBattleDhLabel,
                    fuelLandStaticLabel,
                    fuelLandBattleLabel,
                    supplyAirStaticDhLabel,
                    supplyAirBattleDhLabel,
                    fuelAirNavalStaticLabel,
                    fuelAirBattleLabel,
                    supplyNavalStaticDhLabel,
                    supplyNavalBattleDhLabel,
                    fuelNavalNotMovingLabel,
                    fuelNavalBattleLabel,
                    tpTransportsConversionRatioLabel,
                    ddEscortsConversionRatioLabel,
                    clEscortsConversionRatioLabel,
                    cvlEscortsConversionRatioLabel,
                    productionLineEditLabel,
                    gearingBonusLossUpgradeUnitLabel,
                    gearingBonusLossUpgradeBrigadeLabel,
                    dissentNukesLabel,
                    maxDailyDissentLabel,
                    nukesProductionModifierLabel,
                    convoySystemOptionsAlliedLabel,
                    resourceConvoysBackUnneededLabel,
                    null,
                    spyMissionDaysDhLabel,
                    increateIntelligenceLevelDaysDhLabel,
                    chanceDetectSpyMissionDhLabel,
                    relationshipsHitDetectedMissionsDhLabel,
                    distanceModifierLabel,
                    distanceModifierNeighboursDhLabel,
                    spyLevelBonusDistanceModifierLabel,
                    spyLevelBonusDistanceModifierAboveTenLabel,
                    spyInformationAccuracyModifierDhLabel,
                    icModifierCostLabel,
                    minIcCostModifierLabel,
                    maxIcCostModifierDhLabel,
                    extraMaintenanceCostAboveTenLabel,
                    extraCostIncreasingAboveTenLabel,
                    showThirdCountrySpyReportsDhLabel,
                    spiesMoneyModifierLabel,
                    null,
                    daysBetweenDiplomaticMissionsLabel,
                    timeBetweenSliderChangesDhLabel,
                    requirementAffectSliderDhLabel,
                    useMinisterPersonalityReplacingLabel,
                    relationshipHitCancelTradeLabel,
                    relationshipHitCancelPermanentTradeLabel,
                    puppetsJoinMastersAllianceLabel,
                    mastersBecomePuppetsPuppetsLabel,
                    allowManualClaimsChangeLabel,
                    belligerenceClaimedProvinceLabel,
                    belligerenceClaimsRemovalLabel,
                    joinAutomaticallyAllesAxisLabel,
                    allowChangeHosHogLabel,
                    changeTagCoupLabel,
                    filterReleaseCountriesLabel,
                    null,
                    landXpGainFactorLabel,
                    navalXpGainFactorLabel,
                    airXpGainFactorLabel,
                    airDogfightXpGainFactorLabel,
                    divisionXpGainFactorLabel,
                    leaderXpGainFactorLabel,
                    attritionSeverityModifierLabel,
                    noSupplyAttritionSeverityLabel,
                    noSupplyMinimunAttritionLabel,
                    baseProximityLabel,
                    shoreBombardmentModifierLabel,
                    shoreBombardmentCapLabel,
                    invasionModifierLabel,
                    multipleCombatModifierLabel,
                    offensiveCombinedArmsBonusLabel,
                    defensiveCombinedArmsBonusLabel,
                    surpriseModifierLabel,
                    landCommandLimitModifierLabel,
                    airCommandLimitModifierLabel,
                    navalCommandLimitModifierLabel,
                    envelopmentModifierLabel,
                    encircledModifierLabel,
                    landFortMultiplierLabel,
                    coastalFortMultiplierLabel,
                    hardUnitsAttackingUrbanPenaltyLabel,
                    dissentMultiplierLabel,
                    supplyProblemsModifierLabel,
                    supplyProblemsModifierLandLabel,
                    supplyProblemsModifierAirLabel,
                    supplyProblemsModifierNavalLabel,
                    fuelProblemsModifierLandLabel,
                    fuelProblemsModifierAirLabel,
                    fuelProblemsModifierNavalLabel,
                    raderStationMultiplierLabel,
                    raderStationAaMultiplierLabel,
                    interceptorBomberModifierLabel,
                    airOverstackingModifierLabel,
                    airOverstackingModifierAoDLabel,
                    navalOverstackingModifierLabel,
                    landLeaderCommandLimitRank0Label,
                    landLeaderCommandLimitRank1Label,
                    landLeaderCommandLimitRank2Label,
                    landLeaderCommandLimitRank3Label,
                    airLeaderCommandLimitRank0Label,
                    airLeaderCommandLimitRank1Label,
                    airLeaderCommandLimitRank2Label,
                    airLeaderCommandLimitRank3Label,
                    navalLeaderCommandLimitRank0Label,
                    navalLeaderCommandLimitRank1Label,
                    navalLeaderCommandLimitRank2Label,
                    navalLeaderCommandLimitRank3Label,
                    hqCommandLimitFactorLabel,
                    convoyProtectionFactorLabel,
                    convoyEscortsModelLabel,
                    delayAfterCombatEndsLabel,
                    landDelayBeforeOrdersLabel,
                    navalDelayBeforeOrdersLabel,
                    airDelayBeforeOrdersLabel,
                    maximumSizesAirStacksLabel,
                    durationAirToAirBattlesLabel,
                    durationNavalPortBombingLabel,
                    durationStrategicBombingLabel,
                    durationGroundAttackBombingLabel,
                    effectExperienceCombatLabel,
                    damageNavalBasesBombingLabel,
                    damageAirBaseBombingLabel,
                    damageAaBombingLabel,
                    damageRocketBombingLabel,
                    damageNukeBombingLabel,
                    damageRadarBombingLabel,
                    damageInfraBombingLabel,
                    damageIcBombingLabel,
                    damageResourcesBombingLabel,
                    damageSyntheticOilBombingLabel,
                    howEffectiveGroundDefLabel,
                    chanceAvoidDefencesLeftLabel,
                    chanceAvoidNoDefencesLabel,
                    landChanceAvoidDefencesLeftLabel,
                    airChanceAvoidDefencesLeftLabel,
                    navalChanceAvoidDefencesLeftLabel,
                    landChanceAvoidNoDefencesLabel,
                    airChanceAvoidNoDefencesLabel,
                    navalChanceAvoidNoDefencesLabel,
                    chanceGetTerrainTraitLabel,
                    chanceGetEventTraitLabel,
                    bonusTerrainTraitLabel,
                    bonusSimilarTerrainTraitLabel,
                    bonusEventTraitLabel,
                    bonusLeaderSkillPointLandLabel,
                    bonusLeaderSkillPointAirLabel,
                    bonusLeaderSkillPointNavalLabel,
                    chanceLeaderDyingLabel,
                    airOrgDamageLabel,
                    airStrDamageOrgLabel,
                    airStrDamageLabel,
                    landMinOrgDamageLabel,
                    landOrgDamageHardSoftEachLabel,
                    landOrgDamageHardVsSoftLabel,
                    landMinStrDamageLabel,
                    landStrDamageHardSoftEachLabel,
                    landStrDamageHardVsSoftLabel,
                    airMinOrgDamageLabel,
                    airAdditionalOrgDamageLabel,
                    airMinStrDamageLabel,
                    airAdditionalStrDamageLabel,
                    airStrDamageEntrencedLabel,
                    navalMinOrgDamageLabel,
                    navalAdditionalOrgDamageLabel,
                    navalMinStrDamageLabel,
                    navalAdditionalStrDamageLabel,
                    airStrDamageLandOrgLabel,
                    airOrgDamageLandDhLabel,
                    airStrDamageLandDhLabel,
                    landOrgDamageLandOrgLabel,
                    landOrgDamageLandUrbanLabel,
                    landOrgDamageLandFortLabel,
                    requiredLandFortSizeLabel,
                    landStrDamageLandDhLabel,
                    airOrgDamageAirDhLabel,
                    airStrDamageAirDhLabel,
                    landOrgDamageAirDhLabel,
                    landStrDamageAirDhLabel,
                    navalOrgDamageAirDhLabel,
                    navalStrDamageAirDhLabel,
                    subsOrgDamageAirLabel,
                    subsStrDamageAirLabel,
                    airOrgDamageNavyDhLabel,
                    airStrDamageNavyDhLabel,
                    navalOrgDamageNavyDhLabel,
                    navalStrDamageNavyDhLabel,
                    subsOrgDamageNavyLabel,
                    subsStrDamageNavyLabel,
                    subsOrgDamageLabel,
                    subsStrDamageLabel,
                    subStacksDetectionModifierLabel,
                    airOrgDamageLandAoDLabel,
                    airStrDamageLandAoDLabel,
                    landDamageArtilleryBombardmentLabel,
                    infraDamageArtilleryBombardmentLabel,
                    icDamageArtilleryBombardmentLabel,
                    resourcesDamageArtilleryBombardmentLabel,
                    penaltyArtilleryBombardmentLabel,
                    artilleryStrDamageLabel,
                    artilleryOrgDamageLabel,
                    landStrDamageLandAoDLabel,
                    landOrgDamageLandLabel,
                    landStrDamageAirAoDLabel,
                    landOrgDamageAirAoDLabel,
                    navalStrDamageAirAoDLabel,
                    navalOrgDamageAirAoDLabel,
                    airStrDamageAirAoDLabel,
                    airOrgDamageAirAoDLabel,
                    navalStrDamageNavyAoDLabel,
                    navalOrgDamageNavyAoDLabel,
                    airStrDamageNavyAoDLabel,
                    airOrgDamageNavyAoDLabel,
                    militaryExpenseAttritionModifierLabel,
                    navalMinCombatTimeLabel,
                    landMinCombatTimeLabel,
                    airMinCombatTimeLabel,
                    landOverstackingModifierLabel,
                    landOrgLossMovingLabel,
                    airOrgLossMovingLabel,
                    navalOrgLossMovingLabel,
                    supplyDistanceSeverityLabel,
                    supplyBaseLabel,
                    landOrgGainLabel,
                    airOrgGainLabel,
                    navalOrgGainLabel,
                    nukeManpowerDissentLabel,
                    nukeIcDissentLabel,
                    nukeTotalDissentLabel,
                    landFriendlyOrgGainLabel,
                    airLandStockModifierLabel,
                    scorchDamageLabel,
                    standGroundDissentLabel,
                    scorchGroundBelligerenceLabel,
                    defaultLandStackLabel,
                    defaultNavalStackLabel,
                    defaultAirStackLabel,
                    defaultRocketStackLabel,
                    fortDamageArtilleryBombardmentLabel,
                    artilleryBombardmentOrgCostLabel,
                    landDamageFortLabel,
                    airRebaseFactorLabel,
                    airMaxDisorganizedLabel,
                    aaInflictedStrDamageLabel,
                    aaInflictedOrgDamageLabel,
                    aaInflictedFlyingDamageLabel,
                    aaInflictedBombingDamageLabel,
                    hardAttackStrDamageLabel,
                    hardAttackOrgDamageLabel,
                    armorSoftBreakthroughMinLabel,
                    armorSoftBreakthroughMaxLabel,
                    navalCriticalHitChanceLabel,
                    navalCriticalHitEffectLabel,
                    landFortDamageLabel,
                    portAttackSurpriseChanceDayLabel,
                    portAttackSurpriseChanceNightLabel,
                    portAttackSurpriseModifierLabel,
                    radarAntiSurpriseChanceLabel,
                    radarAntiSurpriseModifierLabel,
                    counterAttackStrDefenderAoDLabel,
                    counterAttackOrgDefenderAoDLabel,
                    counterAttackStrAttackerAoDLabel,
                    counterAttackOrgAttackerAoDLabel,
                    assaultStrDefenderAoDLabel,
                    assaultOrgDefenderAoDLabel,
                    assaultStrAttackerAoDLabel,
                    assaultOrgAttackerAoDLabel,
                    encirclementStrDefenderAoDLabel,
                    encirclementOrgDefenderAoDLabel,
                    encirclementStrAttackerAoDLabel,
                    encirclementOrgAttackerAoDLabel,
                    ambushStrDefenderAoDLabel,
                    ambushOrgDefenderAoDLabel,
                    ambushStrAttackerAoDLabel,
                    ambushOrgAttackerAoDLabel,
                    delayStrDefenderAoDLabel,
                    delayOrgDefenderAoDLabel,
                    delayStrAttackerAoDLabel,
                    delayOrgAttackerAoDLabel,
                    tacticalWithdrawStrDefenderAoDLabel,
                    tacticalWithdrawOrgDefenderAoDLabel,
                    tacticalWithdrawStrAttackerAoDLabel,
                    tacticalWithdrawOrgAttackerAoDLabel,
                    breakthroughStrDefenderAoDLabel,
                    breakthroughOrgDefenderAoDLabel,
                    breakthroughStrAttackerAoDLabel,
                    breakthroughOrgAttackerAoDLabel,
                    navalOrgDamageAaLabel,
                    airOrgDamageAaLabel,
                    airStrDamageAaLabel,
                    aaAirFiringRulesLabel,
                    aaAirNightModifierLabel,
                    aaAirBonusRadarsLabel,
                    movementBonusTerrainTraitLabel,
                    movementBonusSimilarTerrainTraitLabel,
                    logisticsWizardEseBonusLabel,
                    daysOffensiveSupplyLabel,
                    ministerBonusesLabel,
                    orgRegainBonusFriendlyLabel,
                    orgRegainBonusFriendlyCapLabel,
                    convoyInterceptionMissionsLabel,
                    autoReturnTransportFleetsLabel,
                    allowProvinceRegionTargetingLabel,
                    nightHoursWinterLabel,
                    nightHoursSpringFallLabel,
                    nightHoursSummerLabel,
                    recalculateLandArrivalTimesLabel,
                    synchronizeArrivalTimePlayerLabel,
                    synchronizeArrivalTimeAiLabel,
                    recalculateArrivalTimesCombatLabel,
                    landSpeedModifierCombatLabel,
                    landSpeedModifierBombardmentLabel,
                    landSpeedModifierSupplyLabel,
                    landSpeedModifierOrgLabel,
                    landAirSpeedModifierFuelLabel,
                    defaultSpeedFuelLabel,
                    fleetSizeRangePenaltyRatioLabel,
                    fleetSizeRangePenaltyThretholdLabel,
                    fleetSizeRangePenaltyMaxLabel,
                    applyRangeLimitsAreasRegionsLabel,
                    radarBonusDetectionLabel,
                    bonusDetectionFriendlyLabel,
                    screensCapitalRatioModifierLabel,
                    chanceTargetNoOrgLandLabel,
                    screenCapitalShipsTargetingLabel,
                    fleetPositioningDaytimeLabel,
                    fleetPositioningLeaderSkillLabel,
                    fleetPositioningFleetSizeLabel,
                    fleetPositioningFleetCompositionLabel,
                    landCoastalFortsDamageLabel,
                    landCoastalFortsMaxDamageLabel,
                    minSoftnessBrigadesLabel,
                    autoRetreatOrgLabel,
                    landOrgNavalTransportationLabel,
                    maxLandDigLabel,
                    digIncreaseDayLabel,
                    breakthroughEncirclementMinSpeedLabel,
                    breakthroughEncirclementMaxChanceLabel,
                    breakthroughEncirclementChanceModifierLabel,
                    combatEventDurationLabel,
                    counterAttackOrgAttackerDhLabel,
                    counterAttackStrAttackerDhLabel,
                    counterAttackOrgDefenderDhLabel,
                    counterAttackStrDefenderDhLabel,
                    assaultOrgAttackerDhLabel,
                    assaultStrAttackerDhLabel,
                    assaultOrgDefenderDhLabel,
                    assaultStrDefenderDhLabel,
                    encirclementOrgAttackerDhLabel,
                    encirclementStrAttackerDhLabel,
                    encirclementOrgDefenderDhLabel,
                    encirclementStrDefenderDhLabel,
                    ambushOrgAttackerDhLabel,
                    ambushStrAttackerDhLabel,
                    ambushOrgDefenderDhLabel,
                    ambushStrDefenderDhLabel,
                    delayOrgAttackerDhLabel,
                    delayStrAttackerDhLabel,
                    delayOrgDefenderDhLabel,
                    delayStrDefenderDhLabel,
                    tacticalWithdrawOrgAttackerDhLabel,
                    tacticalWithdrawStrAttackerDhLabel,
                    tacticalWithdrawOrgDefenderDhLabel,
                    tacticalWithdrawStrDefenderDhLabel,
                    breakthroughOrgAttackerDhLabel,
                    breakthroughStrAttackerDhLabel,
                    breakthroughOrgDefenderDhLabel,
                    breakthroughStrDefenderDhLabel,
                    hqStrDamageBreakthroughLabel,
                    combatModeLabel,
                    null,
                    attackMissionLabel,
                    attackStartingEfficiencyLabel,
                    attackSpeedBonusLabel,
                    rebaseMissionLabel,
                    rebaseStartingEfficiencyLabel,
                    rebaseChanceDetectedLabel,
                    stratRedeployMissionLabel,
                    stratRedeployStartingEfficiencyLabel,
                    stratRedeployAddedValueLabel,
                    stratRedeployDistanceMultiplierLabel,
                    supportAttackMissionLabel,
                    supportAttackStartingEfficiencyLabel,
                    supportAttackSpeedBonusLabel,
                    supportDefenseMissionLabel,
                    supportDefenseStartingEfficiencyLabel,
                    supportDefenseSpeedBonusLabel,
                    reservesMissionLabel,
                    reservesStartingEfficiencyLabel,
                    reservesSpeedBonusLabel,
                    antiPartisanDutyMissionLabel,
                    antiPartisanDutyStartingEfficiencyLabel,
                    antiPartisanDutySuppressionLabel,
                    plannedDefenseMissionLabel,
                    plannedDefenseStartingEfficiencyLabel,
                    airSuperiorityMissionLabel,
                    airSuperiorityStartingEfficiencyLabel,
                    airSuperiorityDetectionLabel,
                    airSuperiorityMinRequiredLabel,
                    groundAttackMissionLabel,
                    groundAttackStartingEfficiencyLabel,
                    groundAttackOrgDamageLabel,
                    groundAttackStrDamageLabel,
                    interdictionMissionLabel,
                    interdictionStartingEfficiencyLabel,
                    interdictionOrgDamageLabel,
                    interdictionStrDamageLabel,
                    strategicBombardmentMissionLabel,
                    strategicBombardmentStartingEfficiencyLabel,
                    logisticalStrikeMissionLabel,
                    logisticalStrikeStartingEfficiencyLabel,
                    runwayCrateringMissionLabel,
                    runwayCrateringStartingEfficiencyLabel,
                    installationStrikeMissionLabel,
                    installationStrikeStartingEfficiencyLabel,
                    navalStrikeMissionLabel,
                    navalStrikeStartingEfficiencyLabel,
                    portStrikeMissionLabel,
                    portStrikeStartingEfficiencyLabel,
                    convoyAirRaidingMissionLabel,
                    convoyAirRaidingStartingEfficiencyLabel,
                    airSupplyMissionLabel,
                    airSupplyStartingEfficiencyLabel,
                    airborneAssaultMissionLabel,
                    airborneAssaultStartingEfficiencyLabel,
                    nukeMissionLabel,
                    nukeStartingEfficiencyLabel,
                    airScrambleMissionLabel,
                    airScrambleStartingEfficiencyLabel,
                    airScrambleDetectionLabel,
                    airScrambleMinRequiredLabel,
                    convoyRadingMissionLabel,
                    convoyRadingStartingEfficiencyLabel,
                    convoyRadingRangeModifierLabel,
                    convoyRadingChanceDetectedLabel,
                    aswMissionLabel,
                    aswStartingEfficiencyLabel,
                    navalInterdictionMissionLabel,
                    navalInterdictionStartingEfficiencyLabel,
                    shoreBombardmentMissionLabel,
                    shoreBombardmentStartingEfficiencyLabel,
                    shoreBombardmentModifierDhLabel,
                    amphibousAssaultMissionLabel,
                    amphibousAssaultStartingEfficiencyLabel,
                    seaTransportMissionLabel,
                    seaTransportStartingEfficiencyLabel,
                    seaTransportRangeModifierLabel,
                    seaTransportChanceDetectedLabel,
                    NavalCombatPatrolMissionLabel,
                    NavalCombatPatrolStartingEfficiencyLabel,
                    navalPortStrikeMissionLabel,
                    navalPortStrikeStartingEfficiencyLabel,
                    navalAirbaseStrikeMissionLabel,
                    navalAirbaseStrikeStartingEfficiencyLabel,
                    sneakMoveMissionLabel,
                    sneakMoveStartingEfficiencyLabel,
                    sneakMoveRangeModifierLabel,
                    sneakMoveChanceDetectedLabel,
                    navalScrambleMissionLabel,
                    navalScrambleStartingEfficiencyLabel,
                    navalScrambleSpeedBonusLabel,
                    useAttackEfficiencyCombatModifierLabel,
                    null,
                    landFortEfficiencyLabel,
                    coastalFortEfficiencyLabel,
                    groundDefenseEfficiencyLabel,
                    convoyDefenseEfficiencyLabel,
                    manpowerBoostLabel,
                    transportCapacityModifierLabel,
                    occupiedTransportCapacityModifierLabel,
                    attritionModifierLabel,
                    manpowerTrickleBackModifierLabel,
                    supplyDistanceModifierLabel,
                    repairModifierLabel,
                    researchModifierLabel,
                    radarEfficiencyLabel,
                    hqSupplyEfficiencyBonusLabel,
                    hqCombatEventsBonusLabel,
                    combatEventChancesLabel,
                    friendlyArmyDetectionChanceLabel,
                    enemyArmyDetectionChanceLabel,
                    friendlyIntelligenceChanceLabel,
                    enemyIntelligenceChanceLabel,
                    maxAmphibiousArmySizeLabel,
                    energyToOilLabel,
                    totalProductionEfficiencyLabel,
                    supplyProductionEfficiencyLabel,
                    aaPowerLabel,
                    airSurpriseChanceLabel,
                    landSurpriseChanceLabel,
                    navalSurpriseChanceLabel,
                    peacetimeIcModifierLabel,
                    wartimeIcModifierLabel,
                    buildingsProductionModifierLabel,
                    convoysProductionModifierLabel,
                    minShipsPositioningBattleLabel,
                    maxShipsPositioningBattleLabel,
                    peacetimeStockpilesResourcesLabel,
                    wartimeStockpilesResourcesLabel,
                    peacetimeStockpilesOilSuppliesLabel,
                    wartimeStockpilesOilSuppliesLabel,
                    null,
                    blueprintBonusLabel,
                    preHistoricalDateModifierLabel,
                    postHistoricalDateModifierDhLabel,
                    costSkillLevelLabel,
                    meanNumberInventionEventsYearLabel,
                    postHistoricalDateModifierAoDLabel,
                    techSpeedModifierLabel,
                    preHistoricalPenaltyLimitLabel,
                    postHistoricalBonusLimitLabel,
                    maxActiveTechTeamsAoDLabel,
                    requiredIcEachTechTeamAoDLabel,
                    maximumRandomModifierLabel,
                    useNewTechnologyPageLayoutLabel,
                    techOverviewPanelStyleLabel,
                    maxActiveTechTeamsDhLabel,
                    minActiveTechTeamsLabel,
                    requiredIcEachTechTeamDhLabel,
                    newCountryRocketryComponentLabel,
                    newCountryNuclearPhysicsComponentLabel,
                    newCountryNuclearEngineeringComponentLabel,
                    newCountrySecretTechsLabel,
                    maxTechTeamSkillLabel,
                    null,
                    daysTradeOffersLabel,
                    delayGameStartNewTradesLabel,
                    limitAiNewTradesGameStartLabel,
                    desiredOilStockpileLabel,
                    criticalOilStockpileLabel,
                    desiredSuppliesStockpileLabel,
                    criticalSuppliesStockpileLabel,
                    desiredResourcesStockpileLabel,
                    criticalResourceStockpileLabel,
                    wartimeDesiredStockpileMultiplierLabel,
                    peacetimeExtraOilImportLabel,
                    wartimeExtraOilImportLabel,
                    extraImportBelowDesiredLabel,
                    percentageProducedSuppliesLabel,
                    percentageProducedMoneyLabel,
                    extraImportStockpileSelectedLabel,
                    daysDeliverResourcesTradesLabel,
                    mergeTradeDealsLabel,
                    manualTradeDealsLabel,
                    puppetsSendSuppliesMoneyLabel,
                    puppetsCriticalSupplyStockpileLabel,
                    puppetsMaxPoolResourcesLabel,
                    newTradeDealsMinEffectivenessLabel,
                    cancelTradeDealsEffectivenessLabel,
                    autoTradeAiTradeDealsLabel,
                    null,
                    overproduceSuppliesBelowDesiredLabel,
                    multiplierOverproduceSuppliesWarLabel,
                    notProduceSuppliesStockpileOverLabel,
                    maxSerialLineProductionGarrisonMilitiaLabel,
                    minIcSerialProductionNavalAirLabel,
                    notProduceNewUnitsManpowerRatioLabel,
                    notProduceNewUnitsManpowerValueLabel,
                    notProduceNewUnitsSupplyLabel,
                    militaryStrengthTotalIcRatioPeacetimeLabel,
                    militaryStrengthTotalIcRatioWartimeLabel,
                    militaryStrengthTotalIcRatioMajorLabel,
                    notUseOffensiveSupplyStockpileLabel,
                    notUseOffensiveOilStockpileLabel,
                    notUseOffensiveEseLabel,
                    notUseOffensiveOrgStrDamageLabel,
                    aiPeacetimeSpyMissionsDhLabel,
                    aiSpyMissionsCostModifierDhLabel,
                    aiDiplomacyCostModifierDhLabel,
                    aiInfluenceModifierDhLabel,
                    newDowRulesLabel,
                    newDowRules2Label,
                    forcePuppetsJoinMastersAllianceNeutralityLabel,
                    newAiReleaseRulesLabel,
                    aiEventsActionSelectionRulesLabel,
                    forceStrategicRedeploymentHourLabel,
                    maxRedeploymentDaysAiLabel,
                    useQuickAreaCheckGarrisonAiLabel,
                    aiMastersGetProvincesConquredPuppetsLabel,
                    minDaysRequiredAiReleaseCountryLabel,
                    minDaysRequiredAiAlliedLabel,
                    minDaysRequiredAiAlliedSupplyBaseLabel,
                    minRequiredRelationsAlliedClaimedLabel,
                    null,
                    aiSpyDiplomaticMissionLoggerLabel,
                    countryLoggerLabel,
                    switchedAiFilesLoggerLabel,
                    useNewAutoSaveFileFormatLabel,
                    loadNewAiSwitchingAllClientsLabel,
                    tradeEfficiencyCalculationSystemLabel,
                    mergeRelocateProvincialDepotsLabel,
                    inGameLossesLoggingLabel,
                    inGameLossLogging2Label,
                    allowBrigadeAttachingInSupplyLabel,
                    multipleDeploymentSizeArmiesLabel,
                    multipleDeploymentSizeFleetsLabel,
                    multipleDeploymentSizeAirLabel,
                    allowUniquePicturesAllLandProvincesLabel,
                    autoReplyEventsLabel,
                    forceActionsShowLabel,
                    enableDicisionsPlayersLabel,
                    rebelsArmyCompositionLabel,
                    rebelsArmyTechLevelLabel,
                    rebelsArmyMinStrLabel,
                    rebelsArmyMaxStrLabel,
                    rebelsOrgRegainLabel,
                    extraRebelBonusNeighboringProvinceLabel,
                    extraRebelBonusOccupiedLabel,
                    extraRebelBonusMountainLabel,
                    extraRebelBonusHillLabel,
                    extraRebelBonusForestLabel,
                    extraRebelBonusJungleLabel,
                    extraRebelBonusSwampLabel,
                    extraRebelBonusDesertLabel,
                    extraRebelBonusPlainLabel,
                    extraRebelBonusUrbanLabel,
                    extraRebelBonusAirNavalBasesLabel,
                    returnRebelliousProvinceLabel,
                    useNewMinisterFilesFormatLabel,
                    enableRetirementYearMinistersLabel,
                    enableRetirementYearLeadersLabel,
                    loadSpritesModdirOnlyLabel,
                    loadUnitIconsModdirOnlyLabel,
                    loadUnitPicturesModdirOnlyLabel,
                    loadAiFilesModdirOnlyLabel,
                    useSpeedSetGarrisonStatusLabel,
                    useOldSaveGameFormatLabel,
                    productionPanelUiStyleLabel,
                    unitPicturesSizeLabel,
                    enablePicturesNavalBrigadesLabel,
                    buildingsBuildableOnlyProvincesLabel,
                    unitModifiersStatisticsPagesLabel,
                    null,
                    mapNumberLabel,
                    totalProvincesLabel,
                    distanceCalculationModelLabel,
                    MapWidthLabel,
                    MapHeightLabel,
                    null
                };

            // 項目IDと編集コントロールを対応付ける
            _controls = new Control[]
                {
                    icToTcRatioTextBox,
                    icToSuppliesRatioTextBox,
                    icToConsumerGoodsRatioTextBox,
                    icToMoneyRatioTextBox,
                    dissentChangeSpeedTextBox,
                    minAvailableIcTextBox,
                    minFinalIcTextBox,
                    dissentReductionTextBox,
                    maxGearingBonusTextBox,
                    gearingBonusIncrementTextBox,
                    gearingResourceIncrementTextBox,
                    gearingLossNoIcTextBox,
                    icMultiplierNonNationalTextBox,
                    icMultiplierNonOwnedTextBox,
                    icMultiplierPuppetTextBox,
                    resourceMultiplierNonNationalTextBox,
                    resourceMultiplierNonOwnedTextBox,
                    resourceMultiplierNonNationalAiTextBox,
                    resourceMultiplierPuppetTextBox,
                    tcLoadUndeployedDivisionTextBox,
                    tcLoadOccupiedTextBox,
                    tcLoadMultiplierLandTextBox,
                    tcLoadMultiplierAirTextBox,
                    tcLoadMultiplierNavalTextBox,
                    tcLoadPartisanTextBox,
                    tcLoadFactorOffensiveTextBox,
                    tcLoadProvinceDevelopmentTextBox,
                    tcLoadBaseTextBox,
                    manpowerMultiplierNationalTextBox,
                    manpowerMultiplierNonNationalTextBox,
                    manpowerMultiplierColonyTextBox,
                    manpowerMultiplierPuppetTextBox,
                    manpowerMultiplierWartimeOverseaTextBox,
                    manpowerMultiplierPeacetimeTextBox,
                    manpowerMultiplierWartimeTextBox,
                    dailyRetiredManpowerTextBox,
                    requirementAffectSliderTextBox,
                    trickleBackFactorManpowerTextBox,
                    reinforceManpowerTextBox,
                    reinforceCostTextBox,
                    reinforceTimeTextBox,
                    upgradeCostTextBox,
                    upgradeTimeTextBox,
                    reinforceToUpdateModifierTextBox,
                    nationalismStartingValueTextBox,
                    nationalismPerManpowerAoDTextBox,
                    nationalismPerManpowerDhTextBox,
                    maxNationalismTextBox,
                    maxRevoltRiskTextBox,
                    monthlyNationalismReductionTextBox,
                    sendDivisionDaysTextBox,
                    tcLoadUndeployedBrigadeTextBox,
                    canUnitSendNonAlliedComboBox,
                    spyMissionDaysTextBox,
                    increateIntelligenceLevelDaysTextBox,
                    chanceDetectSpyMissionTextBox,
                    relationshipsHitDetectedMissionsTextBox,
                    showThirdCountrySpyReportsComboBox,
                    distanceModifierNeighboursTextBox,
                    spyInformationAccuracyModifierTextBox,
                    aiPeacetimeSpyMissionsComboBox,
                    maxIcCostModifierTextBox,
                    aiSpyMissionsCostModifierTextBox,
                    aiDiplomacyCostModifierTextBox,
                    aiInfluenceModifierTextBox,
                    costRepairBuildingsTextBox,
                    timeRepairBuildingTextBox,
                    provinceEfficiencyRiseTimeTextBox,
                    coreProvinceEfficiencyRiseTimeTextBox,
                    lineUpkeepTextBox,
                    lineStartupTimeTextBox,
                    lineUpgradeTimeTextBox,
                    retoolingCostTextBox,
                    retoolingResourceTextBox,
                    dailyAgingManpowerTextBox,
                    supplyConvoyHuntTextBox,
                    supplyNavalStaticAoDTextBox,
                    supplyNavalMovingTextBox,
                    supplyNavalBattleAoDTextBox,
                    supplyAirStaticAoDTextBox,
                    supplyAirMovingTextBox,
                    supplyAirBattleAoDTextBox,
                    supplyAirBombingTextBox,
                    supplyLandStaticAoDTextBox,
                    supplyLandMovingTextBox,
                    supplyLandBattleAoDTextBox,
                    supplyLandBombingTextBox,
                    supplyStockLandTextBox,
                    supplyStockAirTextBox,
                    supplyStockNavalTextBox,
                    restockSpeedLandTextBox,
                    restockSpeedAirTextBox,
                    restockSpeedNavalTextBox,
                    syntheticOilConversionMultiplierTextBox,
                    syntheticRaresConversionMultiplierTextBox,
                    militarySalaryTextBox,
                    maxIntelligenceExpenditureTextBox,
                    maxResearchExpenditureTextBox,
                    militarySalaryAttrictionModifierTextBox,
                    militarySalaryDissentModifierTextBox,
                    nuclearSiteUpkeepCostTextBox,
                    nuclearPowerUpkeepCostTextBox,
                    syntheticOilSiteUpkeepCostTextBox,
                    syntheticRaresSiteUpkeepCostTextBox,
                    durationDetectionTextBox,
                    convoyProvinceHostileTimeTextBox,
                    convoyProvinceBlockedTimeTextBox,
                    autoTradeConvoyTextBox,
                    spyUpkeepCostTextBox,
                    spyDetectionChanceTextBox,
                    spyCoupDissentModifierTextBox,
                    infraEfficiencyModifierTextBox,
                    manpowerToConsumerGoodsTextBox,
                    timeBetweenSliderChangesAoDTextBox,
                    minimalPlacementIcTextBox,
                    nuclearPowerTextBox,
                    freeInfraRepairTextBox,
                    maxSliderDissentTextBox,
                    minSliderDissentTextBox,
                    maxDissentSliderMoveTextBox,
                    icConcentrationBonusTextBox,
                    transportConversionTextBox,
                    convoyDutyConversionTextBox,
                    escortDutyConversionTextBox,
                    ministerChangeDelayTextBox,
                    ministerChangeEventDelayTextBox,
                    ideaChangeDelayTextBox,
                    ideaChangeEventDelayTextBox,
                    leaderChangeDelayTextBox,
                    changeIdeaDissentTextBox,
                    changeMinisterDissentTextBox,
                    minDissentRevoltTextBox,
                    dissentRevoltMultiplierTextBox,
                    tpMaxAttachTextBox,
                    ssMaxAttachTextBox,
                    ssnMaxAttachTextBox,
                    ddMaxAttachTextBox,
                    clMaxAttachTextBox,
                    caMaxAttachTextBox,
                    bcMaxAttachTextBox,
                    bbMaxAttachTextBox,
                    cvlMaxAttachTextBox,
                    cvMaxAttachTextBox,
                    canChangeIdeasComboBox,
                    canUnitSendNonAlliedDhComboBox,
                    bluePrintsCanSoldNonAlliedComboBox,
                    provinceCanSoldNonAlliedComboBox,
                    transferAlliedCoreProvincesComboBox,
                    provinceBuildingsRepairModifierTextBox,
                    provinceResourceRepairModifierTextBox,
                    stockpileLimitMultiplierResourceTextBox,
                    stockpileLimitMultiplierSuppliesOilTextBox,
                    overStockpileLimitDailyLossTextBox,
                    maxResourceDepotSizeTextBox,
                    maxSuppliesOilDepotSizeTextBox,
                    desiredStockPilesSuppliesOilTextBox,
                    maxManpowerTextBox,
                    convoyTransportsCapacityTextBox,
                    suppyLandStaticDhTextBox,
                    supplyLandBattleDhTextBox,
                    fuelLandStaticTextBox,
                    fuelLandBattleTextBox,
                    supplyAirStaticDhTextBox,
                    supplyAirBattleDhTextBox,
                    fuelAirNavalStaticTextBox,
                    fuelAirBattleTextBox,
                    supplyNavalStaticDhTextBox,
                    supplyNavalBattleDhTextBox,
                    fuelNavalNotMovingTextBox,
                    fuelNavalBattleTextBox,
                    tpTransportsConversionRatioTextBox,
                    ddEscortsConversionRatioTextBox,
                    clEscortsConversionRatioTextBox,
                    cvlEscortsConversionRatioTextBox,
                    productionLineEditComboBox,
                    gearingBonusLossUpgradeUnitTextBox,
                    gearingBonusLossUpgradeBrigadeTextBox,
                    dissentNukesTextBox,
                    maxDailyDissentTextBox,
                    nukesProductionModifierTextBox,
                    convoySystemOptionsAlliedComboBox,
                    resourceConvoysBackUnneededTextBox,
                    null,
                    spyMissionDaysDhTextBox,
                    increateIntelligenceLevelDaysDhTextBox,
                    chanceDetectSpyMissionDhTextBox,
                    relationshipsHitDetectedMissionsDhTextBox,
                    distanceModifierTextBox,
                    distanceModifierNeighboursDhTextBox,
                    spyLevelBonusDistanceModifierTextBox,
                    spyLevelBonusDistanceModifierAboveTenTextBox,
                    spyInformationAccuracyModifierDhTextBox,
                    icModifierCostTextBox,
                    minIcCostModifierTextBox,
                    maxIcCostModifierDhTextBox,
                    extraMaintenanceCostAboveTenTextBox,
                    extraCostIncreasingAboveTenTextBox,
                    showThirdCountrySpyReportsDhComboBox,
                    spiesMoneyModifierTextBox,
                    null,
                    daysBetweenDiplomaticMissionsTextBox,
                    timeBetweenSliderChangesDhTextBox,
                    requirementAffectSliderDhTextBox,
                    useMinisterPersonalityReplacingComboBox,
                    relationshipHitCancelTradeTextBox,
                    relationshipHitCancelPermanentTradeTextBox,
                    puppetsJoinMastersAllianceComboBox,
                    mastersBecomePuppetsPuppetsComboBox,
                    allowManualClaimsChangeComboBox,
                    belligerenceClaimedProvinceTextBox,
                    belligerenceClaimsRemovalTextBox,
                    joinAutomaticallyAllesAxisComboBox,
                    allowChangeHosHogComboBox,
                    changeTagCoupComboBox,
                    filterReleaseCountriesComboBox,
                    null,
                    landXpGainFactorTextBox,
                    navalXpGainFactorTextBox,
                    airXpGainFactorTextBox,
                    airDogfightXpGainFactorTextBox,
                    divisionXpGainFactorTextBox,
                    leaderXpGainFactorTextBox,
                    attritionSeverityModifierTextBox,
                    noSupplyAttritionSeverityTextBox,
                    noSupplyMinimunAttritionTextBox,
                    baseProximityTextBox,
                    shoreBombardmentModifierTextBox,
                    shoreBombardmentCapTextBox,
                    invasionModifierTextBox,
                    multipleCombatModifierTextBox,
                    offensiveCombinedArmsBonusTextBox,
                    defensiveCombinedArmsBonusTextBox,
                    surpriseModifierTextBox,
                    landCommandLimitModifierTextBox,
                    airCommandLimitModifierTextBox,
                    navalCommandLimitModifierTextBox,
                    envelopmentModifierTextBox,
                    encircledModifierTextBox,
                    landFortMultiplierTextBox,
                    coastalFortMultiplierTextBox,
                    hardUnitsAttackingUrbanPenaltyTextBox,
                    dissentMultiplierTextBox,
                    supplyProblemsModifierTextBox,
                    supplyProblemsModifierLandTextBox,
                    supplyProblemsModifierAirTextBox,
                    supplyProblemsModifierNavalTextBox,
                    fuelProblemsModifierLandTextBox,
                    fuelProblemsModifierAirTextBox,
                    fuelProblemsModifierNavalTextBox,
                    raderStationMultiplierTextBox,
                    raderStationAaMultiplierTextBox,
                    interceptorBomberModifierTextBox,
                    airOverstackingModifierTextBox,
                    airOverstackingModifierAoDTextBox,
                    navalOverstackingModifierTextBox,
                    landLeaderCommandLimitRank0TextBox,
                    landLeaderCommandLimitRank1TextBox,
                    landLeaderCommandLimitRank2TextBox,
                    landLeaderCommandLimitRank3TextBox,
                    airLeaderCommandLimitRank0TextBox,
                    airLeaderCommandLimitRank1TextBox,
                    airLeaderCommandLimitRank2TextBox,
                    airLeaderCommandLimitRank3TextBox,
                    navalLeaderCommandLimitRank0TextBox,
                    navalLeaderCommandLimitRank1TextBox,
                    navalLeaderCommandLimitRank2TextBox,
                    navalLeaderCommandLimitRank3TextBox,
                    hqCommandLimitFactorTextBox,
                    convoyProtectionFactorTextBox,
                    convoyEscortsModelTextBox,
                    delayAfterCombatEndsTextBox,
                    landDelayBeforeOrdersTextBox,
                    navalDelayBeforeOrdersTextBox,
                    airDelayBeforeOrdersTextBox,
                    maximumSizesAirStacksTextBox,
                    durationAirToAirBattlesTextBox,
                    durationNavalPortBombingTextBox,
                    durationStrategicBombingTextBox,
                    durationGroundAttackBombingTextBox,
                    effectExperienceCombatTextBox,
                    damageNavalBasesBombingTextBox,
                    damageAirBaseBombingTextBox,
                    damageAaBombingTextBox,
                    damageRocketBombingTextBox,
                    damageNukeBombingTextBox,
                    damageRadarBombingTextBox,
                    damageInfraBombingTextBox,
                    damageIcBombingTextBox,
                    damageResourcesBombingTextBox,
                    damageSyntheticOilBombingTextBox,
                    howEffectiveGroundDefTextBox,
                    chanceAvoidDefencesLeftTextBox,
                    chanceAvoidNoDefencesTextBox,
                    landChanceAvoidDefencesLeftTextBox,
                    airChanceAvoidDefencesLeftTextBox,
                    navalChanceAvoidDefencesLeftTextBox,
                    landChanceAvoidNoDefencesTextBox,
                    airChanceAvoidNoDefencesTextBox,
                    navalChanceAvoidNoDefencesTextBox,
                    chanceGetTerrainTraitTextBox,
                    chanceGetEventTraitTextBox,
                    bonusTerrainTraitTextBox,
                    bonusSimilarTerrainTraitTextBox,
                    bonusEventTraitTextBox,
                    bonusLeaderSkillPointLandTextBox,
                    bonusLeaderSkillPointAirTextBox,
                    bonusLeaderSkillPointNavalTextBox,
                    chanceLeaderDyingTextBox,
                    airOrgDamageTextBox,
                    airStrDamageOrgTextBox,
                    airStrDamageTextBox,
                    landMinOrgDamageTextBox,
                    landOrgDamageHardSoftEachTextBox,
                    landOrgDamageHardVsSoftTextBox,
                    landMinStrDamageTextBox,
                    landStrDamageHardSoftEachTextBox,
                    landStrDamageHardVsSoftTextBox,
                    airMinOrgDamageTextBox,
                    airAdditionalOrgDamageTextBox,
                    airMinStrDamageTextBox,
                    airAdditionalStrDamageTextBox,
                    airStrDamageEntrencedTextBox,
                    navalMinOrgDamageTextBox,
                    navalAdditionalOrgDamageTextBox,
                    navalMinStrDamageTextBox,
                    navalAdditionalStrDamageTextBox,
                    airStrDamageLandOrgTextBox,
                    airOrgDamageLandDhTextBox,
                    airStrDamageLandDhTextBox,
                    landOrgDamageLandOrgTextBox,
                    landOrgDamageLandUrbanTextBox,
                    landOrgDamageLandFortTextBox,
                    requiredLandFortSizeTextBox,
                    landStrDamageLandDhTextBox,
                    airOrgDamageAirDhTextBox,
                    airStrDamageAirDhTextBox,
                    landOrgDamageAirDhTextBox,
                    landStrDamageAirDhTextBox,
                    navalOrgDamageAirDhTextBox,
                    navalStrDamageAirDhTextBox,
                    subsOrgDamageAirTextBox,
                    subsStrDamageAirTextBox,
                    airOrgDamageNavyDhTextBox,
                    airStrDamageNavyDhTextBox,
                    navalOrgDamageNavyDhTextBox,
                    navalStrDamageNavyDhTextBox,
                    subsOrgDamageNavyTextBox,
                    subsStrDamageNavyTextBox,
                    subsOrgDamageTextBox,
                    subsStrDamageTextBox,
                    subStacksDetectionModifierTextBox,
                    airOrgDamageLandAoDTextBox,
                    airStrDamageLandAoDTextBox,
                    landDamageArtilleryBombardmentTextBox,
                    infraDamageArtilleryBombardmentTextBox,
                    icDamageArtilleryBombardmentTextBox,
                    resourcesDamageArtilleryBombardmentTextBox,
                    penaltyArtilleryBombardmentTextBox,
                    artilleryStrDamageTextBox,
                    artilleryOrgDamageTextBox,
                    landStrDamageLandAoDTextBox,
                    landOrgDamageLandTextBox,
                    landStrDamageAirAoDTextBox,
                    landOrgDamageAirAoDTextBox,
                    navalStrDamageAirAoDTextBox,
                    navalOrgDamageAirAoDTextBox,
                    airStrDamageAirAoDTextBox,
                    airOrgDamageAirAoDTextBox,
                    navalStrDamageNavyAoDTextBox,
                    navalOrgDamageNavyAoDTextBox,
                    airStrDamageNavyAoDTextBox,
                    airOrgDamageNavyAoDTextBox,
                    militaryExpenseAttritionModifierTextBox,
                    navalMinCombatTimeTextBox,
                    landMinCombatTimeTextBox,
                    airMinCombatTimeTextBox,
                    landOverstackingModifierTextBox,
                    landOrgLossMovingTextBox,
                    airOrgLossMovingTextBox,
                    navalOrgLossMovingTextBox,
                    supplyDistanceSeverityTextBox,
                    supplyBaseTextBox,
                    landOrgGainTextBox,
                    airOrgGainTextBox,
                    navalOrgGainTextBox,
                    nukeManpowerDissentTextBox,
                    nukeIcDissentTextBox,
                    nukeTotalDissentTextBox,
                    landFriendlyOrgGainTextBox,
                    airLandStockModifierTextBox,
                    scorchDamageTextBox,
                    standGroundDissentTextBox,
                    scorchGroundBelligerenceTextBox,
                    defaultLandStackTextBox,
                    defaultNavalStackTextBox,
                    defaultAirStackTextBox,
                    defaultRocketStackTextBox,
                    fortDamageArtilleryBombardmentTextBox,
                    artilleryBombardmentOrgCostTextBox,
                    landDamageFortTextBox,
                    airRebaseFactorTextBox,
                    airMaxDisorganizedTextBox,
                    aaInflictedStrDamageTextBox,
                    aaInflictedOrgDamageTextBox,
                    aaInflictedFlyingDamageTextBox,
                    aaInflictedBombingDamageTextBox,
                    hardAttackStrDamageTextBox,
                    hardAttackOrgDamageTextBox,
                    armorSoftBreakthroughMinTextBox,
                    armorSoftBreakthroughMaxTextBox,
                    navalCriticalHitChanceTextBox,
                    navalCriticalHitEffectTextBox,
                    landFortDamageTextBox,
                    portAttackSurpriseChanceDayTextBox,
                    portAttackSurpriseChanceNightTextBox,
                    portAttackSurpriseModifierTextBox,
                    radarAntiSurpriseChanceTextBox,
                    radarAntiSurpriseModifierTextBox,
                    counterAttackStrDefenderAoDTextBox,
                    counterAttackOrgDefenderAoDTextBox,
                    counterAttackStrAttackerAoDTextBox,
                    counterAttackOrgAttackerAoDTextBox,
                    assaultStrDefenderAoDTextBox,
                    assaultOrgDefenderAoDTextBox,
                    assaultStrAttackerAoDTextBox,
                    assaultOrgAttackerAoDTextBox,
                    encirclementStrDefenderAoDTextBox,
                    encirclementOrgDefenderAoDTextBox,
                    encirclementStrAttackerAoDTextBox,
                    encirclementOrgAttackerAoDTextBox,
                    ambushStrDefenderAoDTextBox,
                    ambushOrgDefenderAoDTextBox,
                    ambushStrAttackerAoDTextBox,
                    ambushOrgAttackerAoDTextBox,
                    delayStrDefenderAoDTextBox,
                    delayOrgDefenderAoDTextBox,
                    delayStrAttackerAoDTextBox,
                    delayOrgAttackerAoDTextBox,
                    tacticalWithdrawStrDefenderAoDTextBox,
                    tacticalWithdrawOrgDefenderAoDTextBox,
                    tacticalWithdrawStrAttackerAoDTextBox,
                    tacticalWithdrawOrgAttackerAoDTextBox,
                    breakthroughStrDefenderAoDTextBox,
                    breakthroughOrgDefenderAoDTextBox,
                    breakthroughStrAttackerAoDTextBox,
                    breakthroughOrgAttackerAoDTextBox,
                    navalOrgDamageAaTextBox,
                    airOrgDamageAaTextBox,
                    airStrDamageAaTextBox,
                    aaAirFiringRulesComboBox,
                    aaAirNightModifierTextBox,
                    aaAirBonusRadarsTextBox,
                    movementBonusTerrainTraitTextBox,
                    movementBonusSimilarTerrainTraitTextBox,
                    logisticsWizardEseBonusTextBox,
                    daysOffensiveSupplyTextBox,
                    ministerBonusesComboBox,
                    orgRegainBonusFriendlyTextBox,
                    orgRegainBonusFriendlyCapTextBox,
                    convoyInterceptionMissionsComboBox,
                    autoReturnTransportFleetsComboBox,
                    allowProvinceRegionTargetingComboBox,
                    nightHoursWinterTextBox,
                    nightHoursSpringFallTextBox,
                    nightHoursSummerTextBox,
                    recalculateLandArrivalTimesTextBox,
                    synchronizeArrivalTimePlayerTextBox,
                    synchronizeArrivalTimeAiTextBox,
                    recalculateArrivalTimesCombatComboBox,
                    landSpeedModifierCombatTextBox,
                    landSpeedModifierBombardmentTextBox,
                    landSpeedModifierSupplyTextBox,
                    landSpeedModifierOrgTextBox,
                    landAirSpeedModifierFuelTextBox,
                    defaultSpeedFuelTextBox,
                    fleetSizeRangePenaltyRatioTextBox,
                    fleetSizeRangePenaltyThretholdTextBox,
                    fleetSizeRangePenaltyMaxTextBox,
                    applyRangeLimitsAreasRegionsComboBox,
                    radarBonusDetectionTextBox,
                    bonusDetectionFriendlyTextBox,
                    screensCapitalRatioModifierTextBox,
                    chanceTargetNoOrgLandTextBox,
                    screenCapitalShipsTargetingTextBox,
                    fleetPositioningDaytimeTextBox,
                    fleetPositioningLeaderSkillTextBox,
                    fleetPositioningFleetSizeTextBox,
                    fleetPositioningFleetCompositionTextBox,
                    landCoastalFortsDamageTextBox,
                    landCoastalFortsMaxDamageTextBox,
                    minSoftnessBrigadesTextBox,
                    autoRetreatOrgTextBox,
                    landOrgNavalTransportationTextBox,
                    maxLandDigTextBox,
                    digIncreaseDayTextBox,
                    breakthroughEncirclementMinSpeedTextBox,
                    breakthroughEncirclementMaxChanceTextBox,
                    breakthroughEncirclementChanceModifierTextBox,
                    combatEventDurationTextBox,
                    counterAttackOrgAttackerDhTextBox,
                    counterAttackStrAttackerDhTextBox,
                    counterAttackOrgDefenderDhTextBox,
                    counterAttackStrDefenderDhTextBox,
                    assaultOrgAttackerDhTextBox,
                    assaultStrAttackerDhTextBox,
                    assaultOrgDefenderDhTextBox,
                    assaultStrDefenderDhTextBox,
                    encirclementOrgAttackerDhTextBox,
                    encirclementStrAttackerDhTextBox,
                    encirclementOrgDefenderDhTextBox,
                    encirclementStrDefenderDhTextBox,
                    ambushOrgAttackerDhTextBox,
                    ambushStrAttackerDhTextBox,
                    ambushOrgDefenderDhTextBox,
                    ambushStrDefenderDhTextBox,
                    delayOrgAttackerDhTextBox,
                    delayStrAttackerDhTextBox,
                    delayOrgDefenderDhTextBox,
                    delayStrDefenderDhTextBox,
                    tacticalWithdrawOrgAttackerDhTextBox,
                    tacticalWithdrawStrAttackerDhTextBox,
                    tacticalWithdrawOrgDefenderDhTextBox,
                    tacticalWithdrawStrDefenderDhTextBox,
                    breakthroughOrgAttackerDhTextBox,
                    breakthroughStrAttackerDhTextBox,
                    breakthroughOrgDefenderDhTextBox,
                    breakthroughStrDefenderDhTextBox,
                    hqStrDamageBreakthroughComboBox,
                    combatModeComboBox,
                    null,
                    attackMissionComboBox,
                    attackStartingEfficiencyTextBox,
                    attackSpeedBonusTextBox,
                    rebaseMissionComboBox,
                    rebaseStartingEfficiencyTextBox,
                    rebaseChanceDetectedTextBox,
                    stratRedeployMissionComboBox,
                    stratRedeployStartingEfficiencyTextBox,
                    stratRedeployAddedValueTextBox,
                    stratRedeployDistanceMultiplierTextBox,
                    supportAttackMissionComboBox,
                    supportAttackStartingEfficiencyTextBox,
                    supportAttackSpeedBonusTextBox,
                    supportDefenseMissionComboBox,
                    supportDefenseStartingEfficiencyTextBox,
                    supportDefenseSpeedBonusTextBox,
                    reservesMissionComboBox,
                    reservesStartingEfficiencyTextBox,
                    reservesSpeedBonusTextBox,
                    antiPartisanDutyMissionComboBox,
                    antiPartisanDutyStartingEfficiencyTextBox,
                    antiPartisanDutySuppressionTextBox,
                    plannedDefenseMissionComboBox,
                    plannedDefenseStartingEfficiencyTextBox,
                    airSuperiorityMissionComboBox,
                    airSuperiorityStartingEfficiencyTextBox,
                    airSuperiorityDetectionTextBox,
                    airSuperiorityMinRequiredTextBox,
                    groundAttackMissionComboBox,
                    groundAttackStartingEfficiencyTextBox,
                    groundAttackOrgDamageTextBox,
                    groundAttackStrDamageTextBox,
                    interdictionMissionComboBox,
                    interdictionStartingEfficiencyTextBox,
                    interdictionOrgDamageTextBox,
                    interdictionStrDamageTextBox,
                    strategicBombardmentMissionComboBox,
                    strategicBombardmentStartingEfficiencyTextBox,
                    logisticalStrikeMissionComboBox,
                    logisticalStrikeStartingEfficiencyTextBox,
                    runwayCrateringMissionComboBox,
                    runwayCrateringStartingEfficiencyTextBox,
                    installationStrikeMissionComboBox,
                    installationStrikeStartingEfficiencyTextBox,
                    navalStrikeMissionComboBox,
                    navalStrikeStartingEfficiencyTextBox,
                    portStrikeMissionComboBox,
                    portStrikeStartingEfficiencyTextBox,
                    convoyAirRaidingMissionComboBox,
                    convoyAirRaidingStartingEfficiencyTextBox,
                    airSupplyMissionComboBox,
                    airSupplyStartingEfficiencyTextBox,
                    airborneAssaultMissionComboBox,
                    airborneAssaultStartingEfficiencyTextBox,
                    nukeMissionComboBox,
                    nukeStartingEfficiencyTextBox,
                    airScrambleMissionComboBox,
                    airScrambleStartingEfficiencyTextBox,
                    airScrambleDetectionTextBox,
                    airScrambleMinRequiredTextBox,
                    convoyRadingMissionComboBox,
                    convoyRadingStartingEfficiencyTextBox,
                    convoyRadingRangeModifierTextBox,
                    convoyRadingChanceDetectedTextBox,
                    aswMissionComboBox,
                    aswStartingEfficiencyTextBox,
                    navalInterdictionMissionComboBox,
                    navalInterdictionStartingEfficiencyTextBox,
                    shoreBombardmentMissionComboBox,
                    shoreBombardmentStartingEfficiencyTextBox,
                    shoreBombardmentModifierDhTextBox,
                    amphibousAssaultMissionComboBox,
                    amphibousAssaultStartingEfficiencyTextBox,
                    seaTransportMissionComboBox,
                    seaTransportStartingEfficiencyTextBox,
                    seaTransportRangeModifierTextBox,
                    seaTransportChanceDetectedTextBox,
                    NavalCombatPatrolMissionComboBox,
                    NavalCombatPatrolStartingEfficiencyTextBox,
                    navalPortStrikeMissionComboBox,
                    navalPortStrikeStartingEfficiencyTextBox,
                    navalAirbaseStrikeMissionComboBox,
                    navalAirbaseStrikeStartingEfficiencyTextBox,
                    sneakMoveMissionComboBox,
                    sneakMoveStartingEfficiencyTextBox,
                    sneakMoveRangeModifierTextBox,
                    sneakMoveChanceDetectedTextBox,
                    navalScrambleMissionComboBox,
                    navalScrambleStartingEfficiencyTextBox,
                    navalScrambleSpeedBonusTextBox,
                    useAttackEfficiencyCombatModifierComboBox,
                    null,
                    landFortEfficiencyTextBox,
                    coastalFortEfficiencyTextBox,
                    groundDefenseEfficiencyTextBox,
                    convoyDefenseEfficiencyTextBox,
                    manpowerBoostTextBox,
                    transportCapacityModifierTextBox,
                    occupiedTransportCapacityModifierTextBox,
                    attritionModifierTextBox,
                    manpowerTrickleBackModifierTextBox,
                    supplyDistanceModifierTextBox,
                    repairModifierTextBox,
                    researchModifierTextBox,
                    radarEfficiencyTextBox,
                    hqSupplyEfficiencyBonusTextBox,
                    hqCombatEventsBonusTextBox,
                    combatEventChancesTextBox,
                    friendlyArmyDetectionChanceTextBox,
                    enemyArmyDetectionChanceTextBox,
                    friendlyIntelligenceChanceTextBox,
                    enemyIntelligenceChanceTextBox,
                    maxAmphibiousArmySizeTextBox,
                    energyToOilTextBox,
                    totalProductionEfficiencyTextBox,
                    supplyProductionEfficiencyTextBox,
                    aaPowerTextBox,
                    airSurpriseChanceTextBox,
                    landSurpriseChanceTextBox,
                    navalSurpriseChanceTextBox,
                    peacetimeIcModifierTextBox,
                    wartimeIcModifierTextBox,
                    buildingsProductionModifierTextBox,
                    convoysProductionModifierTextBox,
                    minShipsPositioningBattleTextBox,
                    maxShipsPositioningBattleTextBox,
                    peacetimeStockpilesResourcesTextBox,
                    wartimeStockpilesResourcesTextBox,
                    peacetimeStockpilesOilSuppliesTextBox,
                    wartimeStockpilesOilSuppliesTextBox,
                    null,
                    blueprintBonusTextBox,
                    preHistoricalDateModifierTextBox,
                    postHistoricalDateModifierDhTextBox,
                    costSkillLevelTextBox,
                    meanNumberInventionEventsYearTextBox,
                    postHistoricalDateModifierAoDTextBox,
                    techSpeedModifierTextBox,
                    preHistoricalPenaltyLimitTextBox,
                    postHistoricalBonusLimitTextBox,
                    maxActiveTechTeamsAoDTextBox,
                    requiredIcEachTechTeamAoDTextBox,
                    maximumRandomModifierTextBox,
                    useNewTechnologyPageLayoutComboBox,
                    techOverviewPanelStyleComboBox,
                    maxActiveTechTeamsDhTextBox,
                    minActiveTechTeamsTextBox,
                    requiredIcEachTechTeamDhTextBox,
                    newCountryRocketryComponentComboBox,
                    newCountryNuclearPhysicsComponentComboBox,
                    newCountryNuclearEngineeringComponentComboBox,
                    newCountrySecretTechsComboBox,
                    maxTechTeamSkillTextBox,
                    null,
                    daysTradeOffersTextBox,
                    delayGameStartNewTradesTextBox,
                    limitAiNewTradesGameStartTextBox,
                    desiredOilStockpileTextBox,
                    criticalOilStockpileTextBox,
                    desiredSuppliesStockpileTextBox,
                    criticalSuppliesStockpileTextBox,
                    desiredResourcesStockpileTextBox,
                    criticalResourceStockpileTextBox,
                    wartimeDesiredStockpileMultiplierTextBox,
                    peacetimeExtraOilImportTextBox,
                    wartimeExtraOilImportTextBox,
                    extraImportBelowDesiredTextBox,
                    percentageProducedSuppliesTextBox,
                    percentageProducedMoneyTextBox,
                    extraImportStockpileSelectedTextBox,
                    daysDeliverResourcesTradesTextBox,
                    mergeTradeDealsComboBox,
                    manualTradeDealsComboBox,
                    puppetsSendSuppliesMoneyTextBox,
                    puppetsCriticalSupplyStockpileTextBox,
                    puppetsMaxPoolResourcesTextBox,
                    newTradeDealsMinEffectivenessTextBox,
                    cancelTradeDealsEffectivenessTextBox,
                    autoTradeAiTradeDealsTextBox,
                    null,
                    overproduceSuppliesBelowDesiredTextBox,
                    multiplierOverproduceSuppliesWarTextBox,
                    notProduceSuppliesStockpileOverTextBox,
                    maxSerialLineProductionGarrisonMilitiaTextBox,
                    minIcSerialProductionNavalAirTextBox,
                    notProduceNewUnitsManpowerRatioTextBox,
                    notProduceNewUnitsManpowerValueTextBox,
                    notProduceNewUnitsSupplyTextBox,
                    militaryStrengthTotalIcRatioPeacetimeTextBox,
                    militaryStrengthTotalIcRatioWartimeTextBox,
                    militaryStrengthTotalIcRatioMajorTextBox,
                    notUseOffensiveSupplyStockpileTextBox,
                    notUseOffensiveOilStockpileTextBox,
                    notUseOffensiveEseTextBox,
                    notUseOffensiveOrgStrDamageTextBox,
                    aiPeacetimeSpyMissionsDhComboBox,
                    aiSpyMissionsCostModifierDhTextBox,
                    aiDiplomacyCostModifierDhTextBox,
                    aiInfluenceModifierDhTextBox,
                    newDowRulesComboBox,
                    newDowRules2ComboBox,
                    forcePuppetsJoinMastersAllianceNeutralityTextBox,
                    newAiReleaseRulesComboBox,
                    aiEventsActionSelectionRulesTextBox,
                    forceStrategicRedeploymentHourTextBox,
                    maxRedeploymentDaysAiTextBox,
                    useQuickAreaCheckGarrisonAiComboBox,
                    aiMastersGetProvincesConquredPuppetsComboBox,
                    minDaysRequiredAiReleaseCountryTextBox,
                    minDaysRequiredAiAlliedTextBox,
                    minDaysRequiredAiAlliedSupplyBaseTextBox,
                    minRequiredRelationsAlliedClaimedTextBox,
                    null,
                    aiSpyDiplomaticMissionLoggerComboBox,
                    countryLoggerTextBox,
                    switchedAiFilesLoggerComboBox,
                    useNewAutoSaveFileFormatComboBox,
                    loadNewAiSwitchingAllClientsComboBox,
                    tradeEfficiencyCalculationSystemTextBox,
                    mergeRelocateProvincialDepotsTextBox,
                    inGameLossesLoggingComboBox,
                    inGameLossLogging2ComboBox,
                    allowBrigadeAttachingInSupplyComboBox,
                    multipleDeploymentSizeArmiesTextBox,
                    multipleDeploymentSizeFleetsTextBox,
                    multipleDeploymentSizeAirTextBox,
                    allowUniquePicturesAllLandProvincesComboBox,
                    autoReplyEventsComboBox,
                    forceActionsShowComboBox,
                    enableDicisionsPlayersComboBox,
                    rebelsArmyCompositionTextBox,
                    rebelsArmyTechLevelTextBox,
                    rebelsArmyMinStrTextBox,
                    rebelsArmyMaxStrTextBox,
                    rebelsOrgRegainTextBox,
                    extraRebelBonusNeighboringProvinceTextBox,
                    extraRebelBonusOccupiedTextBox,
                    extraRebelBonusMountainTextBox,
                    extraRebelBonusHillTextBox,
                    extraRebelBonusForestTextBox,
                    extraRebelBonusJungleTextBox,
                    extraRebelBonusSwampTextBox,
                    extraRebelBonusDesertTextBox,
                    extraRebelBonusPlainTextBox,
                    extraRebelBonusUrbanTextBox,
                    extraRebelBonusAirNavalBasesTextBox,
                    returnRebelliousProvinceTextBox,
                    useNewMinisterFilesFormatComboBox,
                    enableRetirementYearMinistersComboBox,
                    enableRetirementYearLeadersComboBox,
                    loadSpritesModdirOnlyComboBox,
                    loadUnitIconsModdirOnlyComboBox,
                    loadUnitPicturesModdirOnlyComboBox,
                    loadAiFilesModdirOnlyComboBox,
                    useSpeedSetGarrisonStatusComboBox,
                    useOldSaveGameFormatComboBox,
                    productionPanelUiStyleComboBox,
                    unitPicturesSizeComboBox,
                    enablePicturesNavalBrigadesComboBox,
                    buildingsBuildableOnlyProvincesComboBox,
                    unitModifiersStatisticsPagesTextBox,
                    null,
                    mapNumberTextBox,
                    totalProvincesTextBox,
                    distanceCalculationModelComboBox,
                    MapWidthTextBox,
                    MapHeightTextBox,
                    null
                };

            // コントロールにタグを設定する
            foreach (
                MiscItemId id in
                    Enum.GetValues(typeof (MiscItemId)).Cast<MiscItemId>().Where(id => _controls[(int) id] != null))
            {
                _controls[(int) id].Tag = id;
            }
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            MiscGameType gameType = Misc.GetGameType();
            foreach (MiscItemId id in Enum.GetValues(typeof (MiscItemId))
                                          .Cast<MiscItemId>()
                                          .Where(id => _labels[(int) id] != null))
            {
                if (Misc.ItemTable[(int) id, (int) gameType])
                {
                    _labels[(int) id].Enabled = true;
                    _controls[(int) id].Enabled = true;
                    ComboBox comboBox;
                    switch (Misc.ItemTypes[(int) id])
                    {
                        case MiscItemType.None:
                            break;

                        case MiscItemType.Bool:
                            comboBox = _controls[(int) id] as ComboBox;
                            if (comboBox != null)
                            {
                                comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                            }
                            break;

                        case MiscItemType.Enum:
                            comboBox = _controls[(int) id] as ComboBox;
                            if (comboBox != null)
                            {
                                comboBox.SelectedIndex = (int) Misc.GetItem(id) - (int) Misc.ItemMinValues[(int) id];
                            }
                            break;

                        default:
                            var textBox = _controls[(int) id] as TextBox;
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
                    _controls[(int) id].Enabled = false;
                }
            }
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        private void UpdateItemColor()
        {
            MiscGameType gameType = Misc.GetGameType();
            foreach (MiscItemId id in Enum.GetValues(typeof (MiscItemId))
                                          .Cast<MiscItemId>()
                                          .Where(id => Misc.ItemTable[(int) id, (int) gameType]))
            {
                switch (Misc.ItemTypes[(int) id])
                {
                    case MiscItemType.None:
                    case MiscItemType.Bool:
                    case MiscItemType.Enum:
                        break;

                    default:
                        var textBox = _controls[(int) id] as TextBox;
                        if (textBox != null)
                        {
                            textBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                        }
                        break;
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

            // 編集項目を更新する
            UpdateEditableItems();

            // 編集項目の色を更新する
            UpdateItemColor();
        }

        /// <summary>
        ///     基本データファイルを保存する
        /// </summary>
        private void SaveFiles()
        {
            // 基本データファイルを保存する
            Misc.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            UpdateItemColor();
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
                    if (i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedPosInt:
                    if (i < Misc.ItemMinValues[(int) id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusOne:
                    if ((i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id]) && i != -1)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusThree:
                    if ((i < Misc.ItemMinValues[(int) id] || i > Misc.ItemMaxValues[(int) id]) && i != -1 && i != -2 &&
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
                    if (d < Misc.ItemMinValues[(int) id] || d > Misc.ItemMaxValues[(int) id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                    if ((d < Misc.ItemMinValues[(int) id] || d > Misc.ItemMaxValues[(int) id]) &&
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