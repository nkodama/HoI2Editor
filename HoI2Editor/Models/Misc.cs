using System;
using HoI2Editor.Parsers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     miscファイルの設定項目
    /// </summary>
    public static class Misc
    {
        #region 公開プロパティ

        /// <summary>
        ///     economyセクション
        /// </summary>
        public static MiscEconomy Economy { get; private set; }

        /// <summary>
        ///     intelligenceセクション
        /// </summary>
        public static MiscIntelligence Intelligence { get; private set; }

        /// <summary>
        ///     diplomacyセクション
        /// </summary>
        public static MiscDiplomacy Diplomacy { get; private set; }

        /// <summary>
        ///     combatセクション
        /// </summary>
        public static MiscCombat Combat { get; private set; }

        /// <summary>
        ///     missionセクション
        /// </summary>
        public static MiscMission Mission { get; private set; }

        /// <summary>
        ///     countryセクション
        /// </summary>
        public static MiscCountry Country { get; private set; }

        /// <summary>
        ///     researchセクション
        /// </summary>
        public static MiscResearch Research { get; private set; }

        /// <summary>
        ///     tradeセクション
        /// </summary>
        public static MiscTrade Trade { get; private set; }

        /// <summary>
        ///     aiセクション
        /// </summary>
        public static MiscAi Ai { get; private set; }

        /// <summary>
        ///     modセクション
        /// </summary>
        public static MiscMod Mod { get; private set; }

        /// <summary>
        ///     mapセクション
        /// </summary>
        public static MiscMap Map { get; private set; }

        #endregion

        #region 公開プロパティ - economy

        /// <summary>
        ///     ICからTCへの変換効率
        /// </summary>
        public static double IcToTcRatio;

        /// <summary>
        ///     ICから物資への変換効率
        /// </summary>
        public static double IcToSuppliesRatio;

        /// <summary>
        ///     ICから消費財への変換効率
        /// </summary>
        public static double IcToConsumerGoodsRatio;

        /// <summary>
        ///     ICから資金への変換効率
        /// </summary>
        public static double IcToMoneyRatio;

        /// <summary>
        ///     不満度変化速度
        /// </summary>
        public static double DissentChangeSpeed;

        /// <summary>
        ///     最小実効ICの比率
        /// </summary>
        public static double MinAvailableIc;

        /// <summary>
        ///     最小実効IC
        /// </summary>
        public static double MinFinalIc;

        /// <summary>
        ///     不満度低下補正
        /// </summary>
        public static double DissentReduction;

        /// <summary>
        ///     最大ギアリングボーナス
        /// </summary>
        public static double MaxGearingBonus;

        /// <summary>
        ///     ギアリングボーナスの増加値
        /// </summary>
        public static double GearingBonusIncrement;

        /// <summary>
        ///     連続生産時の資源消費増加
        /// </summary>
        public static double GearingResourceIncrement;

        /// <summary>
        ///     IC不足時のギアリングボーナス減少値
        /// </summary>
        public static double GearingLossNoIc;

        /// <summary>
        ///     非中核州のIC補正
        /// </summary>
        public static double IcMultiplierNonNational;

        /// <summary>
        ///     占領地のIC補正
        /// </summary>
        public static double IcMultiplierNonOwned;

        /// <summary>
        ///     属国のIC補正
        /// </summary>
        public static double IcMultiplierPuppet;

        /// <summary>
        ///     非中核州の資源補正
        /// </summary>
        public static double ResourceMultiplierNonNational;

        /// <summary>
        ///     占領地の資源補正
        /// </summary>
        public static double ResourceMultiplierNonOwned;

        /// <summary>
        ///     非中核州の資源補正(AI)
        /// </summary>
        public static double ResourceMultiplierNonNationalAi;

        /// <summary>
        ///     属国の資源補正
        /// </summary>
        public static double ResourceMultiplierPuppet;

        /// <summary>
        ///     未配備師団のTC負荷
        /// </summary>
        public static double TcLoadUndeployedDivision;

        /// <summary>
        ///     占領地のTC負荷
        /// </summary>
        public static double TcLoadOccupied;

        /// <summary>
        ///     陸軍師団のTC負荷補正
        /// </summary>
        public static double TcLoadMultiplierLand;

        /// <summary>
        ///     空軍師団のTC負荷補正
        /// </summary>
        public static double TcLoadMultiplierAir;

        /// <summary>
        ///     海軍師団のTC負荷補正
        /// </summary>
        public static double TcLoadMultiplierNaval;

        /// <summary>
        ///     パルチザンのTC負荷
        /// </summary>
        public static double TcLoadPartisan;

        /// <summary>
        ///     攻勢時のTC負荷係数
        /// </summary>
        public static double TcLoadFactorOffensive;

        /// <summary>
        ///     プロヴィンス開発のTC負荷
        /// </summary>
        public static double TcLoadProvinceDevelopment;

        /// <summary>
        ///     未配備の基地のTC負荷
        /// </summary>
        public static double TcLoadBase;

        /// <summary>
        ///     中核州の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierNational;

        /// <summary>
        ///     非中核州の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierNonNational;

        /// <summary>
        ///     海外州の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierColony;

        /// <summary>
        ///     属国の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierPuppet;

        /// <summary>
        ///     戦時の海外州の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierWartimeOversea;

        /// <summary>
        ///     平時の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierPeacetime;

        /// <summary>
        ///     戦時の人的資源補正
        /// </summary>
        public static double ManpowerMultiplierWartime;

        /// <summary>
        ///     人的資源の老化率
        /// </summary>
        public static double DailyRetiredManpower;

        /// <summary>
        ///     政策スライダーに影響を与えるためのIC比率
        /// </summary>
        public static double RequirementAffectSlider;

        /// <summary>
        ///     戦闘による損失からの復帰係数
        /// </summary>
        public static double TrickleBackFactorManpower;

        /// <summary>
        ///     補充に必要な人的資源の比率
        /// </summary>
        public static double ReinforceManpower;

        /// <summary>
        ///     補充に必要なICの比率
        /// </summary>
        public static double ReinforceCost;

        /// <summary>
        ///     補充に必要な時間の比率
        /// </summary>
        public static double ReinforceTime;

        /// <summary>
        ///     改良に必要なICの比率
        /// </summary>
        public static double UpgradeCost;

        /// <summary>
        ///     改良に必要な時間の比率
        /// </summary>
        public static double UpgradeTime;

        /// <summary>
        ///     改良のための補充係数
        /// </summary>
        public static double ReinforceToUpdateModifier;

        /// <summary>
        ///     ナショナリズムの初期値
        /// </summary>
        public static double NationalismStartingValue;

        /// <summary>
        ///     人的資源によるナショナリズムの補正値
        /// </summary>
        public static double NationalismPerManpowerAoD;

        /// <summary>
        ///     人的資源によるナショナリズムの補正値
        /// </summary>
        public static double NationalismPerManpowerDh;

        /// <summary>
        ///     ナショナリズム最大値
        /// </summary>
        public static double MaxNationalism;

        /// <summary>
        ///     最大反乱率
        /// </summary>
        public static double MaxRevoltRisk;

        /// <summary>
        ///     月ごとのナショナリズムの減少値
        /// </summary>
        public static double MonthlyNationalismReduction;

        /// <summary>
        ///     師団譲渡後配備可能になるまでの時間
        /// </summary>
        public static int SendDivisionDays;

        /// <summary>
        ///     未配備旅団のTC負荷
        /// </summary>
        public static double TcLoadUndeployedBrigade;

        /// <summary>
        ///     非同盟国に師団を譲渡できるかどうか
        /// </summary>
        public static bool CanUnitSendNonAllied;

        /// <summary>
        ///     諜報任務の間隔
        /// </summary>
        public static int SpyMissionDays;

        /// <summary>
        ///     諜報レベルの増加間隔
        /// </summary>
        public static int IncreateIntelligenceLevelDays;

        /// <summary>
        ///     国内の諜報活動を発見する確率
        /// </summary>
        public static int ChanceDetectSpyMission;

        /// <summary>
        ///     諜報任務発覚時の友好度低下量
        /// </summary>
        public static double RelationshipsHitDetectedMissions;

        /// <summary>
        ///     第三国の諜報活動を報告するか
        /// </summary>
        public static int ShowThirdCountrySpyReports;

        /// <summary>
        ///     諜報任務の近隣国補正
        /// </summary>
        public static double DistanceModifierNeighbours;

        /// <summary>
        ///     情報の正確さ補正
        /// </summary>
        public static double SpyInformationAccuracyModifier;

        /// <summary>
        ///     AIの平時の攻撃的諜報活動
        /// </summary>
        public static int AiPeacetimeSpyMissions;

        /// <summary>
        ///     諜報コスト補正の最大IC
        /// </summary>
        public static double MaxIcCostModifier;

        /// <summary>
        ///     AIの諜報コスト補正
        /// </summary>
        public static double AiSpyMissionsCostModifier;

        /// <summary>
        ///     AIの外交コスト補正
        /// </summary>
        public static double AiDiplomacyCostModifier;

        /// <summary>
        ///     AIの外交影響度補正
        /// </summary>
        public static double AiInfluenceModifier;

        /// <summary>
        ///     建物修復コスト補正
        /// </summary>
        public static double CostRepairBuildings;

        /// <summary>
        ///     建物修復時間補正
        /// </summary>
        public static double TimeRepairBuilding;

        /// <summary>
        ///     プロヴィンス効率上昇時間
        /// </summary>
        public static int ProvinceEfficiencyRiseTime;

        /// <summary>
        ///     中核プロヴィンス効率上昇時間
        /// </summary>
        public static int CoreProvinceEfficiencyRiseTime;

        /// <summary>
        ///     ライン維持コスト補正
        /// </summary>
        public static double LineUpkeep;

        /// <summary>
        ///     ライン開始時間
        /// </summary>
        public static int LineStartupTime;

        /// <summary>
        ///     ライン改良時間
        /// </summary>
        public static int LineUpgradeTime;

        /// <summary>
        ///     ライン調整コスト補正
        /// </summary>
        public static double RetoolingCost;

        /// <summary>
        ///     ライン調整資源補正
        /// </summary>
        public static double RetoolingResource;

        /// <summary>
        ///     人的資源老化補正
        /// </summary>
        public static double DailyAgingManpower;

        /// <summary>
        ///     船団襲撃時物資使用量補正
        /// </summary>
        public static double SupplyConvoyHunt;

        /// <summary>
        ///     海軍の待機時物資使用量補正
        /// </summary>
        public static double SupplyNavalStaticAoD;

        /// <summary>
        ///     海軍の移動時物資使用量補正
        /// </summary>
        public static double SupplyNavalMoving;

        /// <summary>
        ///     海軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyNavalBattleAoD;

        /// <summary>
        ///     空軍の待機時物資使用量補正
        /// </summary>
        public static double SupplyAirStaticAoD;

        /// <summary>
        ///     空軍の移動時物資使用量補正
        /// </summary>
        public static double SupplyAirMoving;

        /// <summary>
        ///     空軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyAirBattleAoD;

        /// <summary>
        ///     空軍の爆撃時物資使用量補正
        /// </summary>
        public static double SupplyAirBombing;

        /// <summary>
        ///     陸軍の待機時物資使用量補正
        /// </summary>
        public static double SupplyLandStaticAoD;

        /// <summary>
        ///     陸軍の移動時物資使用量補正
        /// </summary>
        public static double SupplyLandMoving;

        /// <summary>
        ///     陸軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyLandBattleAoD;

        /// <summary>
        ///     陸軍の砲撃時物資使用量補正
        /// </summary>
        public static double SupplyLandBombing;

        /// <summary>
        ///     陸軍の物資備蓄量
        /// </summary>
        public static double SupplyStockLand;

        /// <summary>
        ///     空軍の物資備蓄量
        /// </summary>
        public static double SupplyStockAir;

        /// <summary>
        ///     海軍の物資備蓄量
        /// </summary>
        public static double SupplyStockNaval;

        /// <summary>
        ///     陸軍の物資再備蓄速度
        /// </summary>
        public static double RestockSpeedLand;

        /// <summary>
        ///     空軍の物資再備蓄速度
        /// </summary>
        public static double RestockSpeedAir;

        /// <summary>
        ///     海軍の物資再備蓄速度
        /// </summary>
        public static double RestockSpeedNaval;

        /// <summary>
        ///     合成石油変換係数
        /// </summary>
        public static double SyntheticOilConversionMultiplier;

        /// <summary>
        ///     合成希少資源変換係数
        /// </summary>
        public static double SyntheticRaresConversionMultiplier;

        /// <summary>
        ///     軍隊の給料
        /// </summary>
        public static double MilitarySalary;

        /// <summary>
        ///     最大諜報費比率
        /// </summary>
        public static double MaxIntelligenceExpenditure;

        /// <summary>
        ///     最大研究費比率
        /// </summary>
        public static double MaxResearchExpenditure;

        /// <summary>
        ///     軍隊の給料不足時の消耗補正
        /// </summary>
        public static double MilitarySalaryAttrictionModifier;

        /// <summary>
        ///     軍隊の給料不足時の不満度補正
        /// </summary>
        public static double MilitarySalaryDissentModifier;

        /// <summary>
        ///     原子炉維持コスト
        /// </summary>
        public static double NuclearSiteUpkeepCost;

        /// <summary>
        ///     原子力発電所維持コスト
        /// </summary>
        public static double NuclearPowerUpkeepCost;

        /// <summary>
        ///     合成石油工場維持コスト
        /// </summary>
        public static double SyntheticOilSiteUpkeepCost;

        /// <summary>
        ///     合成希少資源工場維持コスト
        /// </summary>
        public static double SyntheticRaresSiteUpkeepCost;

        /// <summary>
        ///     海軍情報の存続期間
        /// </summary>
        public static int DurationDetection;

        /// <summary>
        ///     船団攻撃回避時間
        /// </summary>
        public static int ConvoyProvinceHostileTime;

        /// <summary>
        ///     船団攻撃妨害時間
        /// </summary>
        public static int ConvoyProvinceBlockedTime;

        /// <summary>
        ///     自動貿易に必要な輸送船団割合
        /// </summary>
        public static double AutoTradeConvoy;

        /// <summary>
        ///     諜報維持コスト
        /// </summary>
        public static double SpyUpkeepCost;

        /// <summary>
        ///     スパイ発見確率
        /// </summary>
        public static double SpyDetectionChance;

        /// <summary>
        ///     不満度によるクーデター成功率修正
        /// </summary>
        public static double SpyCoupDissentModifier;

        /// <summary>
        ///     インフラによるプロヴィンス効率補正
        /// </summary>
        public static double InfraEfficiencyModifier;

        /// <summary>
        ///     人的資源の消費財生産補正
        /// </summary>
        public static double ManpowerToConsumerGoods;

        /// <summary>
        ///     スライダー移動の間隔
        /// </summary>
        public static int TimeBetweenSliderChangesAoD;

        /// <summary>
        ///     海外プロヴィンスへの配置の必要IC
        /// </summary>
        public static double MinimalPlacementIc;

        /// <summary>
        ///     原子力発電量
        /// </summary>
        public static double NuclearPower;

        /// <summary>
        ///     インフラの自然回復率
        /// </summary>
        public static double FreeInfraRepair;

        /// <summary>
        ///     スライダー移動時の最大不満度
        /// </summary>
        public static double MaxSliderDissent;

        /// <summary>
        ///     スライダー移動時の最小不満度
        /// </summary>
        public static double MinSliderDissent;

        /// <summary>
        ///     スライダー移動可能な最大不満度
        /// </summary>
        public static double MaxDissentSliderMove;

        /// <summary>
        ///     工場集中ボーナス
        /// </summary>
        public static double IcConcentrationBonus;

        /// <summary>
        ///     輸送艦変換係数
        /// </summary>
        public static double TransportConversion;

        /// <summary>
        ///     輸送船団変換係数
        /// </summary>
        public static double ConvoyDutyConversion;

        /// <summary>
        ///     護衛船団変換係数
        /// </summary>
        public static double EscortDutyConversion;

        /// <summary>
        ///     閣僚変更遅延日数
        /// </summary>
        public static int MinisterChangeDelay;

        /// <summary>
        ///     閣僚変更遅延日数(イベント)
        /// </summary>
        public static int MinisterChangeEventDelay;

        /// <summary>
        ///     国策変更遅延日数
        /// </summary>
        public static int IdeaChangeDelay;

        /// <summary>
        ///     国策変更遅延日数(イベント)
        /// </summary>
        public static int IdeaChangeEventDelay;

        /// <summary>
        ///     指揮官変更遅延日数
        /// </summary>
        public static int LeaderChangeDelay;

        /// <summary>
        ///     国策変更時の不満度上昇量
        /// </summary>
        public static double ChangeIdeaDissent;

        /// <summary>
        ///     閣僚変更時の不満度上昇量
        /// </summary>
        public static double ChangeMinisterDissent;

        /// <summary>
        ///     反乱が発生する最低不満度
        /// </summary>
        public static double MinDissentRevolt;

        /// <summary>
        ///     不満度による反乱軍発生率係数
        /// </summary>
        public static double DissentRevoltMultiplier;

        /// <summary>
        ///     輸送艦最大付属装備数
        /// </summary>
        public static int TpMaxAttach;

        /// <summary>
        ///     潜水艦最大付属装備数
        /// </summary>
        public static int SsMaxAttach;

        /// <summary>
        ///     原子力潜水艦最大付属装備数
        /// </summary>
        public static int SsnMaxAttach;

        /// <summary>
        ///     駆逐艦最大付属装備数
        /// </summary>
        public static int DdMaxAttach;

        /// <summary>
        ///     軽巡洋艦最大付属装備数
        /// </summary>
        public static int ClMaxAttach;

        /// <summary>
        ///     重巡洋艦最大付属装備数
        /// </summary>
        public static int CaMaxAttach;

        /// <summary>
        ///     巡洋戦艦最大付属装備数
        /// </summary>
        public static int BcMaxAttach;

        /// <summary>
        ///     戦艦最大付属装備数
        /// </summary>
        public static int BbMaxAttach;

        /// <summary>
        ///     軽空母最大付属装備数
        /// </summary>
        public static int CvlMaxAttach;

        /// <summary>
        ///     空母最大付属装備数
        /// </summary>
        public static int CvMaxAttach;

        /// <summary>
        ///     プレイヤーの国策変更を許可
        /// </summary>
        public static bool CanChangeIdeas;

        /// <summary>
        ///     非同盟国に師団を譲渡できるかどうか
        /// </summary>
        public static int CanUnitSendNonAlliedDh;

        /// <summary>
        ///     非同盟国に青写真の売却を許可
        /// </summary>
        public static int BluePrintsCanSoldNonAllied;

        /// <summary>
        ///     非同盟国にプロヴィンスの売却/譲渡を許可
        /// </summary>
        public static int ProvinceCanSoldNonAllied;

        /// <summary>
        ///     占領中の同盟国の中核州返還を許可
        /// </summary>
        public static bool TransferAlliedCoreProvinces;

        /// <summary>
        ///     建物修復速度補正
        /// </summary>
        public static double ProvinceBuildingsRepairModifier;

        /// <summary>
        ///     資源回復速度補正
        /// </summary>
        public static double ProvinceResourceRepairModifier;

        /// <summary>
        ///     資源備蓄上限補正
        /// </summary>
        public static double StockpileLimitMultiplierResource;

        /// <summary>
        ///     物資/燃料備蓄上限補正
        /// </summary>
        public static double StockpileLimitMultiplierSuppliesOil;

        /// <summary>
        ///     超過備蓄損失割合
        /// </summary>
        public static double OverStockpileLimitDailyLoss;

        /// <summary>
        ///     資源備蓄上限値
        /// </summary>
        public static double MaxResourceDepotSize;

        /// <summary>
        ///     物資/燃料備蓄上限値
        /// </summary>
        public static double MaxSuppliesOilDepotSize;

        /// <summary>
        ///     理想物資/燃料備蓄比率
        /// </summary>
        public static double DesiredStockPilesSuppliesOil;

        /// <summary>
        ///     最大人的資源
        /// </summary>
        public static double MaxManpower;

        /// <summary>
        ///     船団輸送能力
        /// </summary>
        public static double ConvoyTransportsCapacity;

        /// <summary>
        ///     陸軍の待機時物資使用量補正
        /// </summary>
        public static double SuppyLandStaticDh;

        /// <summary>
        ///     陸軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyLandBattleDh;

        /// <summary>
        ///     陸軍の待機時燃料使用量補正
        /// </summary>
        public static double FuelLandStatic;

        /// <summary>
        ///     陸軍の戦闘時燃料使用量補正
        /// </summary>
        public static double FuelLandBattle;

        /// <summary>
        ///     空軍の待機時物資使用量補正
        /// </summary>
        public static double SupplyAirStaticDh;

        /// <summary>
        ///     空軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyAirBattleDh;

        /// <summary>
        ///     空軍/海軍の待機時燃料使用量補正
        /// </summary>
        public static double FuelAirNavalStatic;

        /// <summary>
        ///     空軍の戦闘時燃料使用量補正
        /// </summary>
        public static double FuelAirBattle;

        /// <summary>
        ///     海軍の待機時物資使用量補正
        /// </summary>
        public static double SupplyNavalStaticDh;

        /// <summary>
        ///     海軍の戦闘時物資使用量補正
        /// </summary>
        public static double SupplyNavalBattleDh;

        /// <summary>
        ///     海軍の非移動時燃料使用量補正
        /// </summary>
        public static double FuelNavalNotMoving;

        /// <summary>
        ///     海軍の戦闘時燃料使用量補正
        /// </summary>
        public static double FuelNavalBattle;

        /// <summary>
        ///     輸送艦の輸送船団への変換比率
        /// </summary>
        public static double TpTransportsConversionRatio;

        /// <summary>
        ///     駆逐艦の護衛船団への変換比率
        /// </summary>
        public static double DdEscortsConversionRatio;

        /// <summary>
        ///     軽巡洋艦の護衛船団への変換比率
        /// </summary>
        public static double ClEscortsConversionRatio;

        /// <summary>
        ///     軽空母の護衛船団への変換比率
        /// </summary>
        public static double CvlEscortsConversionRatio;

        /// <summary>
        ///     生産ラインの編集
        /// </summary>
        public static bool ProductionLineEdit;

        /// <summary>
        ///     ユニット改良時のギアリングボーナス減少比率
        /// </summary>
        public static double GearingBonusLossUpgradeUnit;

        /// <summary>
        ///     旅団改良時のギアリングボーナス減少比率
        /// </summary>
        public static double GearingBonusLossUpgradeBrigade;

        /// <summary>
        ///     中核州核攻撃時の不満度上昇係数
        /// </summary>
        public static double DissentNukes;

        /// <summary>
        ///     物資/消費財不足時の最大不満度上昇値
        /// </summary>
        public static double MaxDailyDissent;

        /// <summary>
        ///     核兵器生産補正
        /// </summary>
        public static double NukesProductionModifier;

        /// <summary>
        ///     同盟国に対する船団システム
        /// </summary>
        public static int ConvoySystemOptionsAllied;

        /// <summary>
        ///     不要な資源/燃料の回収比率
        /// </summary>
        public static double ResourceConvoysBackUnneeded;

        #endregion

        #region 公開プロパティ - intelligence

        /// <summary>
        ///     諜報任務の間隔
        /// </summary>
        public static int SpyMissionDaysDh;

        /// <summary>
        ///     諜報レベルの増加間隔
        /// </summary>
        public static int IncreateIntelligenceLevelDaysDh;

        /// <summary>
        ///     国内の諜報活動を発見する確率
        /// </summary>
        public static double ChanceDetectSpyMissionDh;

        /// <summary>
        ///     諜報任務発覚時の友好度低下量
        /// </summary>
        public static double RelationshipsHitDetectedMissionsDh;

        /// <summary>
        ///     諜報任務の距離補正
        /// </summary>
        public static double DistanceModifier;

        /// <summary>
        ///     諜報任務の近隣国補正
        /// </summary>
        public static double DistanceModifierNeighboursDh;

        /// <summary>
        ///     諜報レベルの距離補正
        /// </summary>
        public static double SpyLevelBonusDistanceModifier;

        /// <summary>
        ///     諜報レベル10超過時の距離補正
        /// </summary>
        public static double SpyLevelBonusDistanceModifierAboveTen;

        /// <summary>
        ///     情報の正確さ補正
        /// </summary>
        public static double SpyInformationAccuracyModifierDh;

        /// <summary>
        ///     諜報コストのIC補正
        /// </summary>
        public static double IcModifierCost;

        /// <summary>
        ///     諜報コスト補正の最小IC
        /// </summary>
        public static double MinIcCostModifier;

        /// <summary>
        ///     諜報コスト補正の最大IC
        /// </summary>
        public static double MaxIcCostModifierDh;

        /// <summary>
        ///     諜報レベル10超過時追加維持コスト
        /// </summary>
        public static double ExtraMaintenanceCostAboveTen;

        /// <summary>
        ///     諜報レベル10超過時増加コスト
        /// </summary>
        public static double ExtraCostIncreasingAboveTen;

        /// <summary>
        ///     第三国の諜報活動を報告するか
        /// </summary>
        public static int ShowThirdCountrySpyReportsDh;

        /// <summary>
        ///     諜報資金割り当て補正
        /// </summary>
        public static double SpiesMoneyModifier;

        #endregion

        #region 公開プロパティ - diplomacy

        /// <summary>
        ///     外交官派遣間隔
        /// </summary>
        public static int DaysBetweenDiplomaticMissions;

        /// <summary>
        ///     スライダー移動の間隔
        /// </summary>
        public static int TimeBetweenSliderChangesDh;

        /// <summary>
        ///     政策スライダーに影響を与えるためのIC比率
        /// </summary>
        public static double RequirementAffectSliderDh;

        /// <summary>
        ///     閣僚交代時に閣僚特性を適用する
        /// </summary>
        public static bool UseMinisterPersonalityReplacing;

        /// <summary>
        ///     貿易キャンセル時の友好度低下
        /// </summary>
        public static double RelationshipHitCancelTrade;

        /// <summary>
        ///     永久貿易キャンセル時の友好度低下
        /// </summary>
        public static double RelationshipHitCancelPermanentTrade;

        /// <summary>
        ///     属国が宗主国の同盟に強制参加する
        /// </summary>
        public static bool PuppetsJoinMastersAlliance;

        /// <summary>
        ///     属国の属国が設立できるか
        /// </summary>
        public static bool MastersBecomePuppetsPuppets;

        /// <summary>
        ///     領有権主張の変更
        /// </summary>
        public static bool AllowManualClaimsChange;

        /// <summary>
        ///     領有権主張時の好戦性上昇値
        /// </summary>
        public static double BelligerenceClaimedProvince;

        /// <summary>
        ///     領有権撤回時の好戦性減少値
        /// </summary>
        public static double BelligerenceClaimsRemoval;

        /// <summary>
        ///     宣戦布告された時に対抗陣営へ自動加盟
        /// </summary>
        public static bool JoinAutomaticallyAllesAxis;

        /// <summary>
        ///     国家元首/政府首班の交代
        /// </summary>
        public static int AllowChangeHosHog;

        /// <summary>
        ///     クーデター発生時に兄弟国へ変更
        /// </summary>
        public static bool ChangeTagCoup;

        /// <summary>
        ///     独立可能国設定
        /// </summary>
        public static int FilterReleaseCountries;

        #endregion

        #region 公開プロパティ - combat

        /// <summary>
        ///     陸軍経験値入手係数
        /// </summary>
        public static double LandXpGainFactor;

        /// <summary>
        ///     海軍経験値入手係数
        /// </summary>
        public static double NavalXpGainFactor;

        /// <summary>
        ///     空軍経験値入手係数
        /// </summary>
        public static double AirXpGainFactor;

        /// <summary>
        ///     空軍空戦時経験値入手係数
        /// </summary>
        public static double AirDogfightXpGainFactor;

        /// <summary>
        ///     師団経験値入手係数
        /// </summary>
        public static double DivisionXpGainFactor;

        /// <summary>
        ///     指揮官経験値入手係数
        /// </summary>
        public static double LeaderXpGainFactor;

        /// <summary>
        ///     消耗係数
        /// </summary>
        public static double AttritionSeverityModifier;

        /// <summary>
        ///     無補給時の自然条件消耗係数
        /// </summary>
        public static double NoSupplyAttritionSeverity;

        /// <summary>
        ///     無補給時の消耗係数
        /// </summary>
        public static double NoSupplyMinimunAttrition;

        /// <summary>
        ///     基地戦闘補正
        /// </summary>
        public static double BaseProximity;

        /// <summary>
        ///     艦砲射撃戦闘補正
        /// </summary>
        public static double ShoreBombardmentModifier;

        /// <summary>
        ///     艦砲射撃戦闘効率上限
        /// </summary>
        public static double ShoreBombardmentCap;

        /// <summary>
        ///     強襲上陸ペナルティ
        /// </summary>
        public static double InvasionModifier;

        /// <summary>
        ///     側面攻撃ペナルティ
        /// </summary>
        public static double MultipleCombatModifier;

        /// <summary>
        ///     攻撃側諸兵科連合ボーナス
        /// </summary>
        public static double OffensiveCombinedArmsBonus;

        /// <summary>
        ///     防御側諸兵科連合ボーナス
        /// </summary>
        public static double DefensiveCombinedArmsBonus;

        /// <summary>
        ///     奇襲攻撃ペナルティ
        /// </summary>
        public static double SurpriseModifier;

        /// <summary>
        ///     陸軍指揮上限ペナルティ
        /// </summary>
        public static double LandCommandLimitModifier;

        /// <summary>
        ///     空軍指揮上限ペナルティ
        /// </summary>
        public static double AirCommandLimitModifier;

        /// <summary>
        ///     海軍指揮上限ペナルティ
        /// </summary>
        public static double NavalCommandLimitModifier;

        /// <summary>
        ///     多方面攻撃補正
        /// </summary>
        public static double EnvelopmentModifier;

        /// <summary>
        ///     包囲攻撃ペナルティ
        /// </summary>
        public static double EncircledModifier;

        /// <summary>
        ///     要塞攻撃ペナルティ
        /// </summary>
        public static double LandFortMultiplier;

        /// <summary>
        ///     沿岸要塞攻撃ペナルティ
        /// </summary>
        public static double CoastalFortMultiplier;

        /// <summary>
        ///     装甲ユニットの都市攻撃ペナルティ
        /// </summary>
        public static double HardUnitsAttackingUrbanPenalty;

        /// <summary>
        ///     国民不満度ペナルティ
        /// </summary>
        public static double DissentMultiplier;

        /// <summary>
        ///     補給不足ペナルティ
        /// </summary>
        public static double SupplyProblemsModifier;

        /// <summary>
        ///     陸軍物資不足ペナルティ
        /// </summary>
        public static double SupplyProblemsModifierLand;

        /// <summary>
        ///     空軍物資不足ペナルティ
        /// </summary>
        public static double SupplyProblemsModifierAir;

        /// <summary>
        ///     海軍物資不足ペナルティ
        /// </summary>
        public static double SupplyProblemsModifierNaval;

        /// <summary>
        ///     陸軍燃料不足ペナルティ
        /// </summary>
        public static double FuelProblemsModifierLand;

        /// <summary>
        ///     空軍燃料不足ペナルティ
        /// </summary>
        public static double FuelProblemsModifierAir;

        /// <summary>
        ///     海軍燃料不足ペナルティ
        /// </summary>
        public static double FuelProblemsModifierNaval;

        /// <summary>
        ///     レーダー補正
        /// </summary>
        public static double RaderStationMultiplier;

        /// <summary>
        ///     レーダー/対空砲複合補正
        /// </summary>
        public static double RaderStationAaMultiplier;

        /// <summary>
        ///     爆撃機迎撃ボーナス
        /// </summary>
        public static double InterceptorBomberModifier;

        /// <summary>
        ///     空軍スタックペナルティ
        /// </summary>
        public static double AirOverstackingModifier;

        /// <summary>
        ///     空軍スタックペナルティ
        /// </summary>
        public static double AirOverstackingModifierAoD;

        /// <summary>
        ///     海軍スタックペナルティ
        /// </summary>
        public static double NavalOverstackingModifier;

        /// <summary>
        ///     陸軍元帥指揮上限
        /// </summary>
        public static int LandLeaderCommandLimitRank0;

        /// <summary>
        ///     陸軍大将指揮上限
        /// </summary>
        public static int LandLeaderCommandLimitRank1;

        /// <summary>
        ///     陸軍中将指揮上限
        /// </summary>
        public static int LandLeaderCommandLimitRank2;

        /// <summary>
        ///     陸軍少将指揮上限
        /// </summary>
        public static int LandLeaderCommandLimitRank3;

        /// <summary>
        ///     空軍元帥指揮上限
        /// </summary>
        public static int AirLeaderCommandLimitRank0;

        /// <summary>
        ///     空軍大将指揮上限
        /// </summary>
        public static int AirLeaderCommandLimitRank1;

        /// <summary>
        ///     空軍中将指揮上限
        /// </summary>
        public static int AirLeaderCommandLimitRank2;

        /// <summary>
        ///     空軍少将指揮上限
        /// </summary>
        public static int AirLeaderCommandLimitRank3;

        /// <summary>
        ///     海軍元帥指揮上限
        /// </summary>
        public static int NavalLeaderCommandLimitRank0;

        /// <summary>
        ///     海軍大将指揮上限
        /// </summary>
        public static int NavalLeaderCommandLimitRank1;

        /// <summary>
        ///     海軍中将指揮上限
        /// </summary>
        public static int NavalLeaderCommandLimitRank2;

        /// <summary>
        ///     海軍少将指揮上限
        /// </summary>
        public static int NavalLeaderCommandLimitRank3;

        /// <summary>
        ///     司令部指揮上限係数
        /// </summary>
        public static double HqCommandLimitFactor;

        /// <summary>
        ///     輸送船団護衛係数
        /// </summary>
        public static double ConvoyProtectionFactor;

        /// <summary>
        ///     輸送船団護衛モデル
        /// </summary>
        public static int ConvoyEscortsModel;

        /// <summary>
        ///     戦闘後命令遅延時間
        /// </summary>
        public static int DelayAfterCombatEnds;

        /// <summary>
        ///     陸軍命令遅延時間
        /// </summary>
        public static int LandDelayBeforeOrders;

        /// <summary>
        ///     海軍命令遅延時間
        /// </summary>
        public static int NavalDelayBeforeOrders;

        /// <summary>
        ///     空軍命令遅延時間
        /// </summary>
        public static int AirDelayBeforeOrders;

        /// <summary>
        ///     空軍最大スタックサイズ
        /// </summary>
        public static int MaximumSizesAirStacks;

        /// <summary>
        ///     空戦最小戦闘時間
        /// </summary>
        public static int DurationAirToAirBattles;

        /// <summary>
        ///     港湾攻撃最小戦闘時間
        /// </summary>
        public static int DurationNavalPortBombing;

        /// <summary>
        ///     戦略爆撃最小戦闘時間
        /// </summary>
        public static int DurationStrategicBombing;

        /// <summary>
        ///     地上爆撃最小戦闘時間
        /// </summary>
        public static int DurationGroundAttackBombing;

        /// <summary>
        ///     経験値補正
        /// </summary>
        public static double EffectExperienceCombat;

        /// <summary>
        ///     海軍基地戦略爆撃係数
        /// </summary>
        public static double DamageNavalBasesBombing;

        /// <summary>
        ///     空軍基地戦略爆撃係数
        /// </summary>
        public static double DamageAirBaseBombing;

        /// <summary>
        ///     対空砲戦略爆撃係数
        /// </summary>
        public static double DamageAaBombing;

        /// <summary>
        ///     ロケット試験場戦略爆撃係数
        /// </summary>
        public static double DamageRocketBombing;

        /// <summary>
        ///     原子炉戦略爆撃係数
        /// </summary>
        public static double DamageNukeBombing;

        /// <summary>
        ///     レーダー戦略爆撃係数
        /// </summary>
        public static double DamageRadarBombing;

        /// <summary>
        ///     インフラ戦略爆撃係数
        /// </summary>
        public static double DamageInfraBombing;

        /// <summary>
        ///     IC戦略爆撃係数
        /// </summary>
        public static double DamageIcBombing;

        /// <summary>
        ///     資源戦略爆撃係数
        /// </summary>
        public static double DamageResourcesBombing;

        /// <summary>
        ///     合成石油工場戦略爆撃係数
        /// </summary>
        public static double DamageSyntheticOilBombing;

        /// <summary>
        ///     対地防御効率補正
        /// </summary>
        public static double HowEffectiveGroundDef;

        /// <summary>
        ///     基本回避率(防御回数あり)
        /// </summary>
        public static double ChanceAvoidDefencesLeft;

        /// <summary>
        ///     基本回避率(防御回数なし)
        /// </summary>
        public static double ChanceAvoidNoDefences;

        /// <summary>
        ///     陸軍基本回避率(防御回数あり)
        /// </summary>
        public static double LandChanceAvoidDefencesLeft;

        /// <summary>
        ///     空軍基本回避率(防御回数あり)
        /// </summary>
        public static double AirChanceAvoidDefencesLeft;

        /// <summary>
        ///     海軍基本回避率(防御回数あり)
        /// </summary>
        public static double NavalChanceAvoidDefencesLeft;

        /// <summary>
        ///     陸軍基本回避率(防御回数あり)
        /// </summary>
        public static double LandChanceAvoidNoDefences;

        /// <summary>
        ///     空軍基本回避率(防御回数あり)
        /// </summary>
        public static double AirChanceAvoidNoDefences;

        /// <summary>
        ///     海軍基本回避率(防御回数あり)
        /// </summary>
        public static double NavalChanceAvoidNoDefences;

        /// <summary>
        ///     地形特性獲得可能性
        /// </summary>
        public static double ChanceGetTerrainTrait;

        /// <summary>
        ///     戦闘特性獲得可能性
        /// </summary>
        public static double ChanceGetEventTrait;

        /// <summary>
        ///     地形特性補正
        /// </summary>
        public static double BonusTerrainTrait;

        /// <summary>
        ///     戦闘特性補正
        /// </summary>
        public static double BonusEventTrait;

        /// <summary>
        ///     陸軍指揮官スキル補正
        /// </summary>
        public static double BonusLeaderSkillPointLand;

        /// <summary>
        ///     空軍指揮官スキル補正
        /// </summary>
        public static double BonusLeaderSkillPointAir;

        /// <summary>
        ///     海軍指揮官スキル補正
        /// </summary>
        public static double BonusLeaderSkillPointNaval;

        /// <summary>
        ///     指揮官死亡確率
        /// </summary>
        public static double ChanceLeaderDying;

        /// <summary>
        ///     空軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamage;

        /// <summary>
        ///     空軍戦力被ダメージ(組織力)
        /// </summary>
        public static double AirStrDamageOrg;

        /// <summary>
        ///     空軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamage;

        /// <summary>
        ///     陸軍最小組織率被ダメージ
        /// </summary>
        public static double LandMinOrgDamage;

        /// <summary>
        ///     陸軍組織率被ダメージ(装甲/非装甲同士)
        /// </summary>
        public static int LandOrgDamageHardSoftEach;

        /// <summary>
        ///     陸軍組織率被ダメージ(装甲対非装甲)
        /// </summary>
        public static int LandOrgDamageHardVsSoft;

        /// <summary>
        ///     陸軍最小戦力被ダメージ
        /// </summary>
        public static double LandMinStrDamage;

        /// <summary>
        ///     陸軍戦力被ダメージ(装甲/非装甲同士)
        /// </summary>
        public static int LandStrDamageHardSoftEach;

        /// <summary>
        ///     陸軍戦力被ダメージ(装甲対非装甲)
        /// </summary>
        public static int LandStrDamageHardVsSoft;

        /// <summary>
        ///     空軍最小組織率被ダメージ
        /// </summary>
        public static double AirMinOrgDamage;

        /// <summary>
        ///     空軍追加組織率被ダメージ
        /// </summary>
        public static int AirAdditionalOrgDamage;

        /// <summary>
        ///     空軍最小戦力被ダメージ
        /// </summary>
        public static double AirMinStrDamage;

        /// <summary>
        ///     空軍追加戦力被ダメージ
        /// </summary>
        public static int AirAdditionalStrDamage;

        /// <summary>
        ///     空軍戦力被ダメージ(対塹壕)
        /// </summary>
        public static double AirStrDamageEntrenced;

        /// <summary>
        ///     海軍最小組織率被ダメージ
        /// </summary>
        public static double NavalMinOrgDamage;

        /// <summary>
        ///     海軍追加組織率被ダメージ
        /// </summary>
        public static int NavalAdditionalOrgDamage;

        /// <summary>
        ///     海軍最小戦力被ダメージ
        /// </summary>
        public static double NavalMinStrDamage;

        /// <summary>
        ///     海軍追加戦力被ダメージ
        /// </summary>
        public static int NavalAdditionalStrDamage;

        /// <summary>
        ///     空軍対陸軍戦力被ダメージ(組織率)
        /// </summary>
        public static double AirStrDamageLandOrg;

        /// <summary>
        ///     空軍対陸軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageLandDh;

        /// <summary>
        ///     空軍対陸軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageLandDh;

        /// <summary>
        ///     陸軍対陸軍組織率被ダメージ(組織率)
        /// </summary>
        public static double LandOrgDamageLandOrg;

        /// <summary>
        ///     陸軍対陸軍組織率被ダメージ(都市)
        /// </summary>
        public static double LandOrgDamageLandUrban;

        /// <summary>
        ///     陸軍対陸軍組織率被ダメージ(要塞)
        /// </summary>
        public static double LandOrgDamageLandFort;

        /// <summary>
        ///     必要要塞規模
        /// </summary>
        public static double RequiredLandFortSize;

        /// <summary>
        ///     陸軍対陸軍戦力被ダメージ
        /// </summary>
        public static double LandStrDamageLandDh;

        /// <summary>
        ///     空軍対空軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageAirDh;

        /// <summary>
        ///     空軍対空軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageAirDh;

        /// <summary>
        ///     陸軍対空軍組織率被ダメージ
        /// </summary>
        public static double LandOrgDamageAirDh;

        /// <summary>
        ///     陸軍対空軍戦力被ダメージ
        /// </summary>
        public static double LandStrDamageAirDh;

        /// <summary>
        ///     海軍対空軍組織率被ダメージ
        /// </summary>
        public static double NavalOrgDamageAirDh;

        /// <summary>
        ///     海軍対空軍戦力被ダメージ
        /// </summary>
        public static double NavalStrDamageAirDh;

        /// <summary>
        ///     潜水艦対空軍組織率被ダメージ
        /// </summary>
        public static int SubsOrgDamageAir;

        /// <summary>
        ///     潜水艦対空軍戦力被ダメージ
        /// </summary>
        public static int SubsStrDamageAir;

        /// <summary>
        ///     空軍対海軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageNavyDh;

        /// <summary>
        ///     空軍対海軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageNavyDh;

        /// <summary>
        ///     海軍対海軍組織率被ダメージ
        /// </summary>
        public static double NavalOrgDamageNavyDh;

        /// <summary>
        ///     海軍対海軍戦力被ダメージ
        /// </summary>
        public static double NavalStrDamageNavyDh;

        /// <summary>
        ///     潜水艦対海軍組織率被ダメージ
        /// </summary>
        public static double SubsOrgDamageNavy;

        /// <summary>
        ///     潜水艦対海軍戦力被ダメージ
        /// </summary>
        public static double SubsStrDamageNavy;

        /// <summary>
        ///     潜水艦組織率被ダメージ
        /// </summary>
        public static double SubsOrgDamage;

        /// <summary>
        ///     潜水艦戦力被ダメージ
        /// </summary>
        public static double SubsStrDamage;

        /// <summary>
        ///     潜水艦発見補正
        /// </summary>
        public static double SubStacksDetectionModifier;

        /// <summary>
        ///     空軍対陸軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageLandAoD;

        /// <summary>
        ///     空軍対陸軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageLandAoD;

        /// <summary>
        ///     砲撃ダメージ補正(陸上部隊)
        /// </summary>
        public static double LandDamageArtilleryBombardment;

        /// <summary>
        ///     砲撃ダメージ補正(インフラ)
        /// </summary>
        public static double InfraDamageArtilleryBombardment;

        /// <summary>
        ///     砲撃ダメージ補正(IC)
        /// </summary>
        public static double IcDamageArtilleryBombardment;

        /// <summary>
        ///     砲撃ダメージ補正(資源)
        /// </summary>
        public static double ResourcesDamageArtilleryBombardment;

        /// <summary>
        ///     砲撃中の被攻撃ペナルティ
        /// </summary>
        public static double PenaltyArtilleryBombardment;

        /// <summary>
        ///     砲撃戦力ダメージ
        /// </summary>
        public static double ArtilleryStrDamage;

        /// <summary>
        ///     砲撃組織率ダメージ
        /// </summary>
        public static double ArtilleryOrgDamage;

        /// <summary>
        ///     陸軍対陸軍戦力被ダメージ
        /// </summary>
        public static double LandStrDamageLandAoD;

        /// <summary>
        ///     陸軍対陸軍組織率被ダメージ
        /// </summary>
        public static double LandOrgDamageLand;

        /// <summary>
        ///     陸軍対空軍戦力被ダメージ
        /// </summary>
        public static double LandStrDamageAirAoD;

        /// <summary>
        ///     陸軍対空軍組織率被ダメージ
        /// </summary>
        public static double LandOrgDamageAirAoD;

        /// <summary>
        ///     海軍対空軍戦力被ダメージ
        /// </summary>
        public static double NavalStrDamageAirAoD;

        /// <summary>
        ///     海軍対空軍組織率被ダメージ
        /// </summary>
        public static double NavalOrgDamageAirAoD;

        /// <summary>
        ///     空軍対空軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageAirAoD;

        /// <summary>
        ///     空軍対空軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageAirAoD;

        /// <summary>
        ///     海軍対海軍戦力被ダメージ
        /// </summary>
        public static double NavalStrDamageNavyAoD;

        /// <summary>
        ///     海軍対海軍組織率被ダメージ
        /// </summary>
        public static double NavalOrgDamageNavyAoD;

        /// <summary>
        ///     空軍対海軍戦力被ダメージ
        /// </summary>
        public static double AirStrDamageNavyAoD;

        /// <summary>
        ///     空軍対海軍組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageNavyAoD;

        /// <summary>
        ///     給料不足時の戦闘補正
        /// </summary>
        public static double MilitaryExpenseAttritionModifier;

        /// <summary>
        ///     海軍最小戦闘時間
        /// </summary>
        public static double NavalMinCombatTime;

        /// <summary>
        ///     陸軍最小戦闘時間
        /// </summary>
        public static double LandMinCombatTime;

        /// <summary>
        ///     空軍最小戦闘時間
        /// </summary>
        public static double AirMinCombatTime;

        /// <summary>
        ///     陸軍スタックペナルティ
        /// </summary>
        public static double LandOverstackingModifier;

        /// <summary>
        ///     陸軍移動時組織率減少係数
        /// </summary>
        public static double LandOrgLossMoving;

        /// <summary>
        ///     空軍移動時組織率減少係数
        /// </summary>
        public static double AirOrgLossMoving;

        /// <summary>
        ///     海軍移動時組織率減少係数
        /// </summary>
        public static double NavalOrgLossMoving;

        /// <summary>
        ///     遠隔地補給係数
        /// </summary>
        public static double SupplyDistanceSeverity;

        /// <summary>
        ///     基礎補給効率
        /// </summary>
        public static double SupplyBase;

        /// <summary>
        ///     陸軍組織率補正
        /// </summary>
        public static double LandOrgGain;

        /// <summary>
        ///     空軍組織率補正
        /// </summary>
        public static double AirOrgGain;

        /// <summary>
        ///     海軍組織率補正
        /// </summary>
        public static double NavalOrgGain;

        /// <summary>
        ///     核攻撃不満度係数(人的資源)
        /// </summary>
        public static double NukeManpowerDissent;

        /// <summary>
        ///     核攻撃不満度係数(IC)
        /// </summary>
        public static double NukeIcDissent;

        /// <summary>
        ///     核攻撃不満度係数(トータル)
        /// </summary>
        public static double NukeTotalDissent;

        /// <summary>
        ///     陸軍友好地組織率補正
        /// </summary>
        public static double LandFriendlyOrgGain;

        /// <summary>
        ///     阻止攻撃備蓄補正
        /// </summary>
        public static double AirLandStockModifier;

        /// <summary>
        ///     焦土命令ダメージ
        /// </summary>
        public static double ScorchDamage;

        /// <summary>
        ///     死守命令不満度上昇
        /// </summary>
        public static double StandGroundDissent;

        /// <summary>
        ///     焦土命令好戦性上昇
        /// </summary>
        public static double ScorchGroundBelligerence;

        /// <summary>
        ///     陸軍デフォルトスタック数
        /// </summary>
        public static double DefaultLandStack;

        /// <summary>
        ///     海軍デフォルトスタック数
        /// </summary>
        public static double DefaultNavalStack;

        /// <summary>
        ///     空軍デフォルトスタック数
        /// </summary>
        public static double DefaultAirStack;

        /// <summary>
        ///     ロケットデフォルトスタック数
        /// </summary>
        public static double DefaultRocketStack;

        /// <summary>
        ///     要塞砲撃ダメージ補正
        /// </summary>
        public static double FortDamageArtilleryBombardment;

        /// <summary>
        ///     砲撃組織率減少
        /// </summary>
        public static double ArtilleryBombardmentOrgCost;

        /// <summary>
        ///     陸軍対要塞ダメージ係数
        /// </summary>
        public static double LandDamageFort;

        /// <summary>
        ///     空軍基地移動組織率減少係数
        /// </summary>
        public static double AirRebaseFactor;

        /// <summary>
        ///     空港占領時ペナルティ
        /// </summary>
        public static double AirMaxDisorganized;

        /// <summary>
        ///     対空砲戦力ダメージ補正
        /// </summary>
        public static double AaInflictedStrDamage;

        /// <summary>
        ///     対空砲組織率ダメージ補正
        /// </summary>
        public static double AaInflictedOrgDamage;

        /// <summary>
        ///     対空砲上空通過ダメージ補正
        /// </summary>
        public static double AaInflictedFlyingDamage;

        /// <summary>
        ///     対空砲爆撃中ダメージ補正
        /// </summary>
        public static double AaInflictedBombingDamage;

        /// <summary>
        ///     装甲ユニット戦力ダメージ補正
        /// </summary>
        public static double HardAttackStrDamage;

        /// <summary>
        ///     装甲ユニット組織率ダメージ補正
        /// </summary>
        public static double HardAttackOrgDamage;

        /// <summary>
        ///     戦車対人最小突破係数
        /// </summary>
        public static double ArmorSoftBreakthroughMin;

        /// <summary>
        ///     戦車対人最大突破係数
        /// </summary>
        public static double ArmorSoftBreakthroughMax;

        /// <summary>
        ///     海軍クリティカルヒット確率
        /// </summary>
        public static double NavalCriticalHitChance;

        /// <summary>
        ///     海軍クリティカルヒット効果
        /// </summary>
        public static double NavalCriticalHitEffect;

        /// <summary>
        ///     要塞ダメージ補正
        /// </summary>
        public static double LandFortDamage;

        /// <summary>
        ///     日中港湾攻撃奇襲確率
        /// </summary>
        public static double PortAttackSurpriseChanceDay;

        /// <summary>
        ///     夜間港湾攻撃奇襲確率
        /// </summary>
        public static double PortAttackSurpriseChanceNight;

        /// <summary>
        ///     港湾攻撃奇襲補正
        /// </summary>
        public static double PortAttackSurpriseModifier;

        /// <summary>
        ///     レーダー奇襲確率減少値
        /// </summary>
        public static double RadarAntiSurpriseChance;

        /// <summary>
        ///     レーダー奇襲効果減少値
        /// </summary>
        public static double RadarAntiSurpriseModifier;

        /// <summary>
        ///     反撃イベント防御側戦力補正
        /// </summary>
        public static double CounterAttackStrDefenderAoD;

        /// <summary>
        ///     反撃イベント防御側組織率補正
        /// </summary>
        public static double CounterAttackOrgDefenderAoD;

        /// <summary>
        ///     反撃イベント攻撃側戦力補正
        /// </summary>
        public static double CounterAttackStrAttackerAoD;

        /// <summary>
        ///     反撃イベント攻撃側組織率補正
        /// </summary>
        public static double CounterAttackOrgAttackerAoD;

        /// <summary>
        ///     強襲イベント防御側戦力補正
        /// </summary>
        public static double AssaultStrDefenderAoD;

        /// <summary>
        ///     強襲イベント防御側組織率補正
        /// </summary>
        public static double AssaultOrgDefenderAoD;

        /// <summary>
        ///     強襲イベント攻撃側戦力補正
        /// </summary>
        public static double AssaultStrAttackerAoD;

        /// <summary>
        ///     強襲イベント攻撃側組織率補正
        /// </summary>
        public static double AssaultOrgAttackerAoD;

        /// <summary>
        ///     包囲イベント防御側戦力補正
        /// </summary>
        public static double EncirclementStrDefenderAoD;

        /// <summary>
        ///     包囲イベント防御側組織率補正
        /// </summary>
        public static double EncirclementOrgDefenderAoD;

        /// <summary>
        ///     包囲イベント攻撃側戦力補正
        /// </summary>
        public static double EncirclementStrAttackerAoD;

        /// <summary>
        ///     包囲イベント攻撃側組織率補正
        /// </summary>
        public static double EncirclementOrgAttackerAoD;

        /// <summary>
        ///     待伏イベント防御側戦力補正
        /// </summary>
        public static double AmbushStrDefenderAoD;

        /// <summary>
        ///     待伏イベント防御側組織率補正
        /// </summary>
        public static double AmbushOrgDefenderAoD;

        /// <summary>
        ///     待伏イベント攻撃側戦力補正
        /// </summary>
        public static double AmbushStrAttackerAoD;

        /// <summary>
        ///     待伏イベント攻撃側組織率補正
        /// </summary>
        public static double AmbushOrgAttackerAoD;

        /// <summary>
        ///     遅延イベント防御側戦力補正
        /// </summary>
        public static double DelayStrDefenderAoD;

        /// <summary>
        ///     遅延イベント防御側組織率補正
        /// </summary>
        public static double DelayOrgDefenderAoD;

        /// <summary>
        ///     遅延イベント攻撃側戦力補正
        /// </summary>
        public static double DelayStrAttackerAoD;

        /// <summary>
        ///     遅延イベント攻撃側組織率補正
        /// </summary>
        public static double DelayOrgAttackerAoD;

        /// <summary>
        ///     後退イベント防御側戦力補正
        /// </summary>
        public static double TacticalWithdrawStrDefenderAoD;

        /// <summary>
        ///     後退イベント防御側組織率補正
        /// </summary>
        public static double TacticalWithdrawOrgDefenderAoD;

        /// <summary>
        ///     後退イベント攻撃側戦力補正
        /// </summary>
        public static double TacticalWithdrawStrAttackerAoD;

        /// <summary>
        ///     後退イベント攻撃側組織率補正
        /// </summary>
        public static double TacticalWithdrawOrgAttackerAoD;

        /// <summary>
        ///     突破イベント防御側戦力補正
        /// </summary>
        public static double BreakthroughStrDefenderAoD;

        /// <summary>
        ///     突破イベント防御側組織率補正
        /// </summary>
        public static double BreakthroughOrgDefenderAoD;

        /// <summary>
        ///     突破イベント攻撃側戦力補正
        /// </summary>
        public static double BreakthroughStrAttackerAoD;

        /// <summary>
        ///     突破イベント攻撃側組織率補正
        /// </summary>
        public static double BreakthroughOrgAttackerAoD;

        /// <summary>
        ///     海軍対空砲組織率被ダメージ
        /// </summary>
        public static double NavalOrgDamageAa;

        /// <summary>
        ///     空軍対空砲組織率被ダメージ
        /// </summary>
        public static double AirOrgDamageAa;

        /// <summary>
        ///     空軍対空砲戦力被ダメージ
        /// </summary>
        public static double AirStrDamageAa;

        /// <summary>
        ///     対空砲攻撃ルール
        /// </summary>
        public static int AaAirFiringRules;

        /// <summary>
        ///     対空砲夜間攻撃補正
        /// </summary>
        public static double AaAirNightModifier;

        /// <summary>
        ///     対空砲攻撃レーダーボーナス
        /// </summary>
        public static double AaAirBonusRadars;

        /// <summary>
        ///     地形適正移動ボーナス
        /// </summary>
        public static double MovementBonusTerrainTrait;

        /// <summary>
        ///     類似地形適正移動ボーナス
        /// </summary>
        public static double MovementBonusSimilarTerrainTrait;

        /// <summary>
        ///     兵站管理の補給効率ボーナス
        /// </summary>
        public static double LogisticsWizardEseBonus;

        /// <summary>
        ///     攻勢継続日数
        /// </summary>
        public static int DaysOffensiveSupply;

        /// <summary>
        ///     閣僚ボーナス適用方法
        /// </summary>
        public static double MinisterBonuses;

        /// <summary>
        ///     友好地組織率回復ボーナス
        /// </summary>
        public static double OrgRegainBonusFriendly;

        /// <summary>
        ///     友好地組織率回復ボーナス上限
        /// </summary>
        public static double OrgRegainBonusFriendlyCap;

        /// <summary>
        ///     海上任務中の船団妨害
        /// </summary>
        public static int ConvoyInterceptionMissions;

        /// <summary>
        ///     輸送艦隊の自動帰還
        /// </summary>
        public static int AutoReturnTransportFleets;

        /// <summary>
        ///     単一プロヴィンス/地域指定任務
        /// </summary>
        public static bool AllowProvinceRegionTargeting;

        /// <summary>
        ///     冬季夜間時間
        /// </summary>
        public static double NightHoursWinter;

        /// <summary>
        ///     春季/秋季夜間時間
        /// </summary>
        public static double NightHoursSpringFall;

        /// <summary>
        ///     夏季夜間時間
        /// </summary>
        public static double NightHoursSummer;

        /// <summary>
        ///     陸上部隊到着時刻再計算間隔
        /// </summary>
        public static int RecalculateLandArrivalTimes;

        /// <summary>
        ///     同時到着補正(プレイヤー)
        /// </summary>
        public static double SynchronizeArrivalTimePlayer;

        /// <summary>
        ///     同時到着補正(AI)
        /// </summary>
        public static double SynchronizeArrivalTimeAi;

        /// <summary>
        ///     戦闘後到着時刻再計算
        /// </summary>
        public static double RecalculateArrivalTimesCombat;

        /// <summary>
        ///     戦闘時陸軍移動速度補正
        /// </summary>
        public static double LandSpeedModifierCombat;

        /// <summary>
        ///     沿岸砲撃時陸軍移動速度補正
        /// </summary>
        public static double LandSpeedModifierBombardment;

        /// <summary>
        ///     物資切れ時陸軍移動速度補正
        /// </summary>
        public static double LandSpeedModifierSupply;

        /// <summary>
        ///     組織率低下時陸軍移動速度補正
        /// </summary>
        public static double LandSpeedModifierOrg;

        /// <summary>
        ///     燃料切れ時陸軍/空軍移動速度補正
        /// </summary>
        public static double LandAirSpeedModifierFuel;

        /// <summary>
        ///     燃料切れ時デフォルト移動速度
        /// </summary>
        public static double DefaultSpeedFuel;

        /// <summary>
        ///     艦隊規模航続距離ペナルティ割合
        /// </summary>
        public static double FleetSizeRangePenaltyRatio;

        /// <summary>
        ///     艦隊規模航続距離ペナルティ閾値
        /// </summary>
        public static double FleetSizeRangePenaltyThrethold;

        /// <summary>
        ///     艦隊規模航続距離ペナルティ上限
        /// </summary>
        public static double FleetSizeRangePenaltyMax;

        /// <summary>
        ///     地方/地域内での距離制限適用
        /// </summary>
        public static int ApplyRangeLimitsAreasRegions;

        /// <summary>
        ///     レーダー航空機発見ボーナス
        /// </summary>
        public static double RadarBonusDetection;

        /// <summary>
        ///     友好地航空機発見ボーナス
        /// </summary>
        public static double BonusDetectionFriendly;

        /// <summary>
        ///     主力艦/補助艦割合修正
        /// </summary>
        public static double ScreensCapitalRatioModifier;

        /// <summary>
        ///     陸軍組織率不足ユニット標的確率
        /// </summary>
        public static double ChanceTargetNoOrgLand;

        /// <summary>
        ///     主力艦/補助艦標的ポジション値
        /// </summary>
        public static double ScreenCapitalShipsTargeting;

        /// <summary>
        ///     海戦ポジション値日中ボーナス
        /// </summary>
        public static double FleetPositioningDaytime;

        /// <summary>
        ///     海戦ポジション値スキル補正
        /// </summary>
        public static double FleetPositioningLeaderSkill;

        /// <summary>
        ///     海戦ポジション値艦隊規模補正
        /// </summary>
        public static double FleetPositioningFleetSize;

        /// <summary>
        ///     海戦ポジション値艦隊構成補正
        /// </summary>
        public static double FleetPositioningFleetComposition;

        /// <summary>
        ///     要塞被ダメージ補正
        /// </summary>
        public static double LandCoastalFortsDamage;

        /// <summary>
        ///     要塞最大被ダメージ
        /// </summary>
        public static double LandCoastalFortsMaxDamage;

        /// <summary>
        ///     付属旅団による最小脆弱性
        /// </summary>
        public static double MinSoftnessBrigades;

        /// <summary>
        ///     自動撤退組織率
        /// </summary>
        public static double AutoRetreatOrg;

        /// <summary>
        ///     陸軍海上輸送後組織率補正
        /// </summary>
        public static double LandOrgNavalTransportation;

        /// <summary>
        ///     最大塹壕値
        /// </summary>
        public static int MaxLandDig;

        /// <summary>
        ///     1日の塹壕増加量
        /// </summary>
        public static double DigIncreaseDay;

        /// <summary>
        ///     突破/包囲最小速度
        /// </summary>
        public static double BreakthroughEncirclementMinSpeed;

        /// <summary>
        ///     突破/包囲最大確率
        /// </summary>
        public static double BreakthroughEncirclementMaxChance;

        /// <summary>
        ///     突破/包囲確率補正
        /// </summary>
        public static double BreakthroughEncirclementChanceModifier;

        /// <summary>
        ///     コンバットイベント継続時間
        /// </summary>
        public static int CombatEventDuration;

        /// <summary>
        ///     反撃イベント攻撃側組織率補正
        /// </summary>
        public static double CounterAttackOrgAttackerDh;

        /// <summary>
        ///     反撃イベント攻撃側戦力補正
        /// </summary>
        public static double CounterAttackStrAttackerDh;

        /// <summary>
        ///     反撃イベント防御側組織率補正
        /// </summary>
        public static double CounterAttackOrgDefenderDh;

        /// <summary>
        ///     反撃イベント防御側戦力補正
        /// </summary>
        public static double CounterAttackStrDefenderDh;

        /// <summary>
        ///     強襲イベント攻撃側組織率補正
        /// </summary>
        public static double AssaultOrgAttackerDh;

        /// <summary>
        ///     強襲イベント攻撃側戦力補正
        /// </summary>
        public static double AssaultStrAttackerDh;

        /// <summary>
        ///     強襲イベント防御側組織率補正
        /// </summary>
        public static double AssaultOrgDefenderDh;

        /// <summary>
        ///     強襲イベント防御側戦力補正
        /// </summary>
        public static double AssaultStrDefenderDh;

        /// <summary>
        ///     包囲イベント攻撃側組織率補正
        /// </summary>
        public static double EncirclementOrgAttackerDh;

        /// <summary>
        ///     包囲イベント攻撃側戦力補正
        /// </summary>
        public static double EncirclementStrAttackerDh;

        /// <summary>
        ///     包囲イベント防御側組織率補正
        /// </summary>
        public static double EncirclementOrgDefenderDh;

        /// <summary>
        ///     包囲イベント防御側戦力補正
        /// </summary>
        public static double EncirclementStrDefenderDh;

        /// <summary>
        ///     待伏イベント攻撃側組織率補正
        /// </summary>
        public static double AmbushOrgAttackerDh;

        /// <summary>
        ///     待伏イベント攻撃側戦力補正
        /// </summary>
        public static double AmbushStrAttackerDh;

        /// <summary>
        ///     待伏イベント防御側組織率補正
        /// </summary>
        public static double AmbushOrgDefenderDh;

        /// <summary>
        ///     待伏イベント防御側戦力補正
        /// </summary>
        public static double AmbushStrDefenderDh;

        /// <summary>
        ///     遅延イベント攻撃側組織率補正
        /// </summary>
        public static double DelayOrgAttackerDh;

        /// <summary>
        ///     遅延イベント攻撃側戦力補正
        /// </summary>
        public static double DelayStrAttackerDh;

        /// <summary>
        ///     遅延イベント防御側組織率補正
        /// </summary>
        public static double DelayOrgDefenderDh;

        /// <summary>
        ///     遅延イベント防御側戦力補正
        /// </summary>
        public static double DelayStrDefenderDh;

        /// <summary>
        ///     後退イベント攻撃側組織率補正
        /// </summary>
        public static double TacticalWithdrawOrgAttackerDh;

        /// <summary>
        ///     後退イベント攻撃側戦力補正
        /// </summary>
        public static double TacticalWithdrawStrAttackerDh;

        /// <summary>
        ///     後退イベント防御側組織率補正
        /// </summary>
        public static double TacticalWithdrawOrgDefenderDh;

        /// <summary>
        ///     後退イベント防御側戦力補正
        /// </summary>
        public static double TacticalWithdrawStrDefenderDh;

        /// <summary>
        ///     突破イベント攻撃側組織率補正
        /// </summary>
        public static double BreakthroughOrgAttackerDh;

        /// <summary>
        ///     突破イベント攻撃側戦力補正
        /// </summary>
        public static double BreakthroughStrAttackerDh;

        /// <summary>
        ///     突破イベント防御側組織率補正
        /// </summary>
        public static double BreakthroughOrgDefenderDh;

        /// <summary>
        ///     突破イベント防御側戦力補正
        /// </summary>
        public static double BreakthroughStrDefenderDh;

        /// <summary>
        ///     司令部は突破イベント時のみ戦力ダメージ
        /// </summary>
        public static bool HqStrDamageBreakthrough;

        /// <summary>
        ///     戦闘モード
        /// </summary>
        public static int CombatMode;

        #endregion

        #region 内部フィールド

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
            Economy = new MiscEconomy();
            Intelligence = new MiscIntelligence();
            Diplomacy = new MiscDiplomacy();
            Combat = new MiscCombat();
            Mission = new MiscMission();
            Country = new MiscCountry();
            Research = new MiscResearch();
            Trade = new MiscTrade();
            Ai = new MiscAi();
            Mod = new MiscMod();
            Map = new MiscMap();

            // miscファイルを解釈する
            if (!MiscParser.Parse(Game.GetReadFileName(Game.MiscPathName)))
            {
                return;
            }

            _loaded = true;
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
    }

    /// <summary>
    ///     economyセクション
    /// </summary>
    public class MiscEconomy
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     intelligenceセクション
    /// </summary>
    public class MiscIntelligence
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     diplomacyセクション
    /// </summary>
    public class MiscDiplomacy
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     combatセクション
    /// </summary>
    public class MiscCombat
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     missionセクション
    /// </summary>
    public class MiscMission
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     countryセクション
    /// </summary>
    public class MiscCountry
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     researchセクション
    /// </summary>
    public class MiscResearch
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     tradeセクション
    /// </summary>
    public class MiscTrade
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     aiセクション
    /// </summary>
    public class MiscAi
    {
        // NOT IMPLEMENTED
    }

    /// <summary>
    ///     modセクション
    /// </summary>
    public class MiscMod
    {
        #region 公開プロパティ

        #region DH

        /// <summary>
        ///     AIのスパイ/外交活動をログに記録する [0:無効/1:有効]
        /// </summary>
        public bool AiSpyDiplomacyLogger { get; set; }

        /// <summary>
        ///     国家の状態をログに記録する [0:無効/>0:指定日数毎に記録/-1:プレイヤー国のみ]
        /// </summary>
        public int CountryLogger { get; set; }

        /// <summary>
        ///     AI切り替えをログに記録する [0:無効/1:有効]
        /// </summary>
        public bool SwitchedAiLogger { get; set; }

        /// <summary>
        ///     新しい自動セーブファイル名フォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public bool NewAutoSaveFileFormat { get; set; }

        /// <summary>
        ///     マルチプレイで新しいAI設定/切り替えを使用する [0:無効/1:有効]
        /// </summary>
        public bool LoadNewAiSettingMultiPlayer { get; set; }

        /// <summary>
        ///     新しい貿易システムの計算間隔 [0:DDAのシステムを使用/>0:指定日数毎に計算]
        /// </summary>
        public int TradeEfficiencyCalculationInterval { get; set; }

        /// <summary>
        ///     備蓄庫の統合/再配置の計算間隔 [0:DDAのシステムを使用/>0:指定日数毎に計算]
        /// </summary>
        public int DepotCalculationInterval { get; set; }

        /// <summary>
        ///     損失をログに記録する
        ///     [0:無効/1:有効(艦船/輸送船団)/2:装備の損失も含む]
        ///     [3:捕獲されたユニットの装備の損失も含む/4:消耗も含む]
        /// </summary>
        public int LossesLogger { get; set; }

        /// <summary>
        ///     自国領土外でも補給が届いていれば旅団の配備を可能にする [0:無効/1:有効]
        /// </summary>
        public bool AllowBrigadeAttachingInSupply { get; set; }

        /// <summary>
        ///     一括配備数(陸軍)
        /// </summary>
        public int MultipleDeploymentSizeArmy { get; set; }

        /// <summary>
        ///     一括配備数(海軍)
        /// </summary>
        public int MultipleDeploymentSizeFleet { get; set; }

        /// <summary>
        ///     一括配備数(空軍)
        /// </summary>
        public int MultipleDeploymentSizeAir { get; set; }

        /// <summary>
        ///     全ての陸地プロヴィンスで固有の画像を許可する [0:無効/1:有効]
        /// </summary>
        public bool AllowUniquePictureAllLandProvinces { get; set; }

        /// <summary>
        ///     プレイヤー国のイベントに自動応答する [0:無効/1:有効]
        /// </summary>
        public bool AutoReplyEvents { get; set; }

        /// <summary>
        ///     無効なアクションを表示する [0:無効/1:有効なトリガーがあれば表示/2:常に表示]
        /// </summary>
        public int ForceActionsToShowNoValidCommands { get; set; }

        /// <summary>
        ///     ディシジョンを有効にする [0:無効/1:有効]
        /// </summary>
        public bool EnableDecisionsPlayer { get; set; }

        /// <summary>
        ///     反乱軍の構成 [0:民兵のみ/100:歩兵のみ]
        /// </summary>
        public int RebelArmyComposition { get; set; }

        /// <summary>
        ///     反乱軍の技術レベル [-1:モデル1/0:最新モデル/>0:最新モデルから指定数幅のランダム]
        /// </summary>
        public int RebelArmyTechLevel { get; set; }

        /// <summary>
        ///     反乱軍の最小戦力 [1-100]
        /// </summary>
        public double RebelArmyMinStr { get; set; }

        /// <summary>
        ///     反乱軍の最大戦力 [1-100]
        /// </summary>
        public double RebelArmyMaxStr { get; set; }

        /// <summary>
        ///     反乱軍の指揮統制率回復
        /// </summary>
        public double RebelArmyOrgRegain { get; set; }

        /// <summary>
        ///     隣接プロヴィンスが反乱軍に支配されている時の反乱危険率増加値
        /// </summary>
        public double RebelBonusControlledRebel { get; set; }

        /// <summary>
        ///     隣接プロヴィンスが敵軍に占領されている時の反乱危険率増加値
        /// </summary>
        public double RebelBonusOccupiedEnemy { get; set; }

        /// <summary>
        ///     山岳プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusMountain { get; set; }

        /// <summary>
        ///     丘陵プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusHill { get; set; }

        /// <summary>
        ///     森林プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusForest { get; set; }

        /// <summary>
        ///     密林プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusJungle { get; set; }

        /// <summary>
        ///     湿地プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusSwamp { get; set; }

        /// <summary>
        ///     砂漠プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusDesert { get; set; }

        /// <summary>
        ///     平地プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusPlain { get; set; }

        /// <summary>
        ///     都市プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusUrban { get; set; }

        /// <summary>
        ///     海軍/空軍基地のあるプロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusBase { get; set; }

        /// <summary>
        ///     反乱軍消滅後にプロヴィンスが元の所有国に復帰するまでの月数 [100000:復帰しない]
        /// </summary>
        public int ReturnMonthNoRebelArmy { get; set; }

        /// <summary>
        ///     新閣僚フォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public bool NewMinisterFormat { get; set; }

        #endregion

        #region DH1.03

        /// <summary>
        ///     閣僚の引退年を有効にする [0:無効/1:有効]
        /// </summary>
        public bool RetirementYearMinister { get; set; }

        /// <summary>
        ///     指揮官の引退年を有効にする [0:無効/1:有効]
        /// </summary>
        public bool RetirementYearLeader { get; set; }

        #endregion

        #region DH

        /// <summary>
        ///     スプライトをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public bool LoadSpriteModdirOnly { get; set; }

        /// <summary>
        ///     ユニットアイコンをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public bool LoadUnitIconModdirOnly { get; set; }

        /// <summary>
        ///     ユニット画像をMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public bool LoadUnitPictureModdirOnly { get; set; }

        /// <summary>
        ///     AIをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public bool LoadAiModdirOnly { get; set; }

        /// <summary>
        ///     移動不可ユニットの判定に速度の値を使用する [0:守備隊のみ移動不可/1:速度0ならば移動不可]
        /// </summary>
        public bool UseSpeedGarrisonStatus { get; set; }

        /// <summary>
        ///     DH1.02より前のセーブデータフォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public bool UseOldSaveGameFormat { get; set; }

        #endregion

        #region DH1.03以降

        /// <summary>
        ///     生産画面のUIスタイル [0:従来スタイル/1:新スタイル]
        /// </summary>
        public bool ProductionPanelStyle { get; set; }

        /// <summary>
        ///     ユニット画像のサイズ [0:従来サイズ(192x104)/1:新サイズ(360x160)]
        /// </summary>
        public bool UnitPictureSize { get; set; }

        /// <summary>
        ///     艦船装備に画像を使用する [0:アイコンを使用/1:画像を使用]
        /// </summary>
        public bool UsePictureNavalBrigade { get; set; }

        /// <summary>
        ///     建物をプロヴィンスからのみ生産可能にする
        ///     [0:無効/1:有効(港/飛行場/原子炉/ロケット試験所)/2:1に加えて対空砲/レーダーも]
        /// </summary>
        public int BuildableOnlyProvince { get; set; }

        /// <summary>
        ///     ユニット補正の統計ページを新スタイルにする閾値
        /// </summary>
        public int UnitModifierStatisticsPage { get; set; }

        #endregion

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MiscMod()
        {
            MultipleDeploymentSizeArmy = 3;
            MultipleDeploymentSizeFleet = 6;
            MultipleDeploymentSizeAir = 2;
            RebelArmyTechLevel = -1;
            RebelArmyMinStr = 100;
            RebelArmyMaxStr = 100;
            RebelArmyOrgRegain = 0.2;
            ReturnMonthNoRebelArmy = 100000;
            UnitModifierStatisticsPage = 15;
        }

        #endregion
    }

    /// <summary>
    ///     mapセクション
    /// </summary>
    public class MiscMap
    {
        #region 公開プロパティ

        #region DH

        /// <summary>
        ///     マップの番号 [0:デフォルトマップを使用/その他:map\map_Xを使用]
        /// </summary>
        public int MapNo { get; set; }

        /// <summary>
        ///     総プロヴィンス数
        /// </summary>
        public int TotalProvinces { get; set; }

        /// <summary>
        ///     距離計算モデル [0:従来モデル/1:新モデル]
        /// </summary>
        public bool DistanceCalculationModel { get; set; }

        /// <summary>
        ///     マップの幅
        /// </summary>
        public int MapWidth { get; set; }

        /// <summary>
        ///     マップの高さ
        /// </summary>
        public int MapHeight { get; set; }

        #endregion

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MiscMap()
        {
            TotalProvinces = 2608;
            MapWidth = 29952;
            MapHeight = 11520;
        }

        #endregion
    }

    /// <summary>
    ///     Miscの項目ID
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
        CanUnitSendNonAllied, // 非同盟国に師団を譲渡できるかどうか
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
        CanUnitSendNonAlliedDh, // 非同盟国に師団を譲渡できるかどうか
        BluePrintsCanSoldNonAllied, // 非同盟国に青写真の売却を許可
        ProvinceCanSoldNonAllied, // 非同盟国にプロヴィンスの売却/譲渡を許可
        TransferAlliedCoreProvinces, // 占領中の同盟国の中核州返還を許可
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
    }
}