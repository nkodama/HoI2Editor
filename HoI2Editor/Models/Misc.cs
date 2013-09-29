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
        /// <summary>
        ///     AIの外交コスト補正
        /// </summary>
        public double AiDiplomacyCostModifier;

        /// <summary>
        ///     AIの外交影響度補正
        /// </summary>
        public double AiInfluenceModifier;

        /// <summary>
        ///     AIの諜報コスト補正
        /// </summary>
        public double AiSpyMissionsCostModifier;

        /// <summary>
        ///     自動貿易に必要な輸送船団割合
        /// </summary>
        public double AutoTradeConvoy;

        /// <summary>
        ///     戦艦最大付属装備数
        /// </summary>
        public int BbMaxAttach;

        /// <summary>
        ///     巡洋戦艦最大付属装備数
        /// </summary>
        public int BcMaxAttach;

        /// <summary>
        ///     非同盟国に青写真の売却を許可
        /// </summary>
        public int BluePrintsCanSoldNonAllied;

        /// <summary>
        ///     重巡洋艦最大付属装備数
        /// </summary>
        public int CaMaxAttach;

        /// <summary>
        ///     プレイヤーの国策変更を許可
        /// </summary>
        public bool CanChangeIdeas;

        /// <summary>
        ///     非同盟国に師団を譲渡できるかどうか
        /// </summary>
        public bool CanUnitSendNonAllied;

        /// <summary>
        ///     非同盟国に師団を譲渡できるかどうか
        /// </summary>
        public int CanUnitSendNonAlliedDh;

        /// <summary>
        ///     国内の諜報活動を発見する確率
        /// </summary>
        public int ChanceDetectSpyMission;

        /// <summary>
        ///     国策変更時の不満度上昇量
        /// </summary>
        public double ChangeIdeaDissent;

        /// <summary>
        ///     閣僚変更時の不満度上昇量
        /// </summary>
        public double ChangeMinisterDissent;

        /// <summary>
        ///     軽巡洋艦の護衛船団への変換比率
        /// </summary>
        public double ClEscortsConversionRatio;

        /// <summary>
        ///     軽巡洋艦最大付属装備数
        /// </summary>
        public int ClMaxAttach;

        /// <summary>
        ///     輸送船団変換係数
        /// </summary>
        public double ConvoyDutyConversion;

        /// <summary>
        ///     船団攻撃妨害時間
        /// </summary>
        public int ConvoyProvinceBlockedTime;

        /// <summary>
        ///     船団攻撃回避時間
        /// </summary>
        public int ConvoyProvinceHostileTime;

        /// <summary>
        ///     同盟国に対する船団システム
        /// </summary>
        public int ConvoySystemOptionsAllied;

        /// <summary>
        ///     船団輸送能力
        /// </summary>
        public double ConvoyTransportsCapacity;

        /// <summary>
        ///     中核プロヴィンス効率上昇時間
        /// </summary>
        public int CoreProvinceEfficiencyRiseTime;

        /// <summary>
        ///     建物修復コスト補正
        /// </summary>
        public double CostRepairBuildings;

        /// <summary>
        ///     空母最大付属装備数
        /// </summary>
        public int CvMaxAttach;

        /// <summary>
        ///     軽空母の護衛船団への変換比率
        /// </summary>
        public double CvlEscortsConversionRatio;

        /// <summary>
        ///     軽空母最大付属装備数
        /// </summary>
        public int CvlMaxAttach;

        /// <summary>
        ///     人的資源老化補正
        /// </summary>
        public double DailyAgingManpower;

        /// <summary>
        ///     人的資源の老化率
        /// </summary>
        public double DailyRetiredManpower;

        /// <summary>
        ///     駆逐艦の護衛船団への変換比率
        /// </summary>
        public double DdEscortsConversionRatio;

        /// <summary>
        ///     駆逐艦最大付属装備数
        /// </summary>
        public int DdMaxAttach;

        /// <summary>
        ///     理想物資/燃料備蓄比率
        /// </summary>
        public double DesiredStockPilesSuppliesOil;

        /// <summary>
        ///     AIの平時の攻撃的諜報活動
        /// </summary>
        public int AiPeacetimeSpyMissions;

        /// <summary>
        ///     不満度変化速度
        /// </summary>
        public double DissentChangeSpeed;

        /// <summary>
        ///     中核州核攻撃時の不満度上昇係数
        /// </summary>
        public double DissentNukes;

        /// <summary>
        ///     不満度低下補正
        /// </summary>
        public double DissentReduction;

        /// <summary>
        ///     不満度による反乱軍発生率係数
        /// </summary>
        public double DissentRevoltMultiplier;

        /// <summary>
        ///     諜報任務の近隣国補正
        /// </summary>
        public double DistanceModifierNeighbours;

        /// <summary>
        ///     海軍情報の存続期間
        /// </summary>
        public int DurationDetection;

        /// <summary>
        ///     護衛船団変換係数
        /// </summary>
        public double EscortDutyConversion;

        /// <summary>
        ///     インフラの自然回復率
        /// </summary>
        public double FreeInfraRepair;

        /// <summary>
        ///     空軍の戦闘時燃料使用量補正
        /// </summary>
        public double FuelAirBattle;

        /// <summary>
        ///     空軍/海軍の待機時燃料使用量補正
        /// </summary>
        public double FuelAirNavalStatic;

        /// <summary>
        ///     陸軍の戦闘時燃料使用量補正
        /// </summary>
        public double FuelLandBattle;

        /// <summary>
        ///     陸軍の待機時燃料使用量補正
        /// </summary>
        public double FuelLandStatic;

        /// <summary>
        ///     海軍の戦闘時燃料使用量補正
        /// </summary>
        public double FuelNavalBattle;

        /// <summary>
        ///     海軍の非移動時燃料使用量補正
        /// </summary>
        public double FuelNavalNotMoving;

        /// <summary>
        ///     ギアリングボーナスの増加値
        /// </summary>
        public double GearingBonusIncrement;

        /// <summary>
        ///     旅団改良時のギアリングボーナス減少比率
        /// </summary>
        public double GearingBonusLossUpgradeBrigade;

        /// <summary>
        ///     ユニット改良時のギアリングボーナス減少比率
        /// </summary>
        public double GearingBonusLossUpgradeUnit;

        /// <summary>
        ///     IC不足時のギアリングボーナス減少値
        /// </summary>
        public double GearingLossNoIc;

        /// <summary>
        ///     連続生産時の資源消費増加
        /// </summary>
        public double GearingResourceIncrement;

        /// <summary>
        ///     工場集中ボーナス
        /// </summary>
        public double IcConcentrationBonus;

        /// <summary>
        ///     非中核州のIC補正
        /// </summary>
        public double IcMultiplierNonNational;

        /// <summary>
        ///     占領地のIC補正
        /// </summary>
        public double IcMultiplierNonOwned;

        /// <summary>
        ///     属国のIC補正
        /// </summary>
        public double IcMultiplierPuppet;

        /// <summary>
        ///     ICから消費財への変換効率
        /// </summary>
        public double IcToConsumerGoodsRatio;

        /// <summary>
        ///     ICから資金への変換効率
        /// </summary>
        public double IcToMoneyRatio;

        /// <summary>
        ///     ICから物資への変換効率
        /// </summary>
        public double IcToSuppliesRatio;

        /// <summary>
        ///     ICからTCへの変換効率
        /// </summary>
        public double IcToTcRatio;

        /// <summary>
        ///     国策変更遅延日数
        /// </summary>
        public int IdeaChangeDelay;

        /// <summary>
        ///     国策変更遅延日数(イベント)
        /// </summary>
        public int IdeaChangeEventDelay;

        /// <summary>
        ///     諜報レベルの増加間隔
        /// </summary>
        public int IncreateIntelligenceLevelDays;

        /// <summary>
        ///     インフラによるプロヴィンス効率補正
        /// </summary>
        public double InfraEfficiencyModifier;

        /// <summary>
        ///     指揮官変更遅延日数
        /// </summary>
        public int LeaderChangeDelay;

        /// <summary>
        ///     ライン開始時間
        /// </summary>
        public int LineStartupTime;

        /// <summary>
        ///     ライン改良時間
        /// </summary>
        public int LineUpgradeTime;

        /// <summary>
        ///     ライン維持コスト補正
        /// </summary>
        public double LineUpkeep;

        /// <summary>
        ///     海外州の人的資源補正
        /// </summary>
        public double ManpowerMultiplierColony;

        /// <summary>
        ///     中核州の人的資源補正
        /// </summary>
        public double ManpowerMultiplierNational;

        /// <summary>
        ///     非中核州の人的資源補正
        /// </summary>
        public double ManpowerMultiplierNonNational;

        /// <summary>
        ///     平時の人的資源補正
        /// </summary>
        public double ManpowerMultiplierPeacetime;

        /// <summary>
        ///     属国の人的資源補正
        /// </summary>
        public double ManpowerMultiplierPuppet;

        /// <summary>
        ///     戦時の人的資源補正
        /// </summary>
        public double ManpowerMultiplierWartime;

        /// <summary>
        ///     戦時の海外州の人的資源補正
        /// </summary>
        public double ManpowerMultiplierWartimeOversea;

        /// <summary>
        ///     人的資源の消費財生産補正
        /// </summary>
        public double ManpowerToConsumerGoods;

        /// <summary>
        ///     物資/消費財不足時の最大不満度上昇値
        /// </summary>
        public double MaxDailyDissent;

        /// <summary>
        ///     スライダー移動可能な最大不満度
        /// </summary>
        public double MaxDissentSliderMove;

        /// <summary>
        ///     最大ギアリングボーナス
        /// </summary>
        public double MaxGearingBonus;

        /// <summary>
        ///     諜報コスト補正の最大IC
        /// </summary>
        public double MaxIcCostModifier;

        /// <summary>
        ///     最大諜報費比率
        /// </summary>
        public double MaxIntelligenceExpenditure;

        /// <summary>
        ///     最大人的資源
        /// </summary>
        public double MaxManpower;

        /// <summary>
        ///     ナショナリズム最大値
        /// </summary>
        public double MaxNationalism;

        /// <summary>
        ///     最大研究費比率
        /// </summary>
        public double MaxResearchExpenditure;

        /// <summary>
        ///     資源備蓄上限値
        /// </summary>
        public double MaxResourceDepotSize;

        /// <summary>
        ///     最大反乱率
        /// </summary>
        public double MaxRevoltRisk;

        /// <summary>
        ///     スライダー移動時の最大不満度
        /// </summary>
        public double MaxSliderDissent;

        /// <summary>
        ///     物資/燃料備蓄上限値
        /// </summary>
        public double MaxSuppliesOilDepotSize;

        /// <summary>
        ///     軍隊の給料
        /// </summary>
        public double MilitarySalary;

        /// <summary>
        ///     軍隊の給料不足時の消耗補正
        /// </summary>
        public double MilitarySalaryAttrictionModifier;

        /// <summary>
        ///     軍隊の給料不足時の不満度補正
        /// </summary>
        public double MilitarySalaryDissentModifier;

        /// <summary>
        ///     最小実効ICの比率
        /// </summary>
        public double MinAvailableIc;

        /// <summary>
        ///     反乱が発生する最低不満度
        /// </summary>
        public double MinDissentRevolt;

        /// <summary>
        ///     最小実効IC
        /// </summary>
        public double MinFinalIc;

        /// <summary>
        ///     スライダー移動時の最小不満度
        /// </summary>
        public double MinSliderDissent;

        /// <summary>
        ///     海外プロヴィンスへの配置の必要IC
        /// </summary>
        public double MinimalPlacementIc;

        /// <summary>
        ///     閣僚変更遅延日数
        /// </summary>
        public int MinisterChangeDelay;

        /// <summary>
        ///     閣僚変更遅延日数(イベント)
        /// </summary>
        public int MinisterChangeEventDelay;

        /// <summary>
        ///     月ごとのナショナリズムの減少値
        /// </summary>
        public double MonthlyNationalismReduction;

        /// <summary>
        ///     人的資源によるナショナリズムの補正値
        /// </summary>
        public double NationalismPerManpowerAoD;

        /// <summary>
        ///     人的資源によるナショナリズムの補正値
        /// </summary>
        public double NationalismPerManpowerDh;

        /// <summary>
        ///     ナショナリズムの初期値
        /// </summary>
        public double NationalismStartingValue;

        /// <summary>
        ///     原子力発電量
        /// </summary>
        public double NuclearPower;

        /// <summary>
        ///     原子力発電所維持コスト
        /// </summary>
        public double NuclearPowerUpkeepCost;

        /// <summary>
        ///     原子炉維持コスト
        /// </summary>
        public double NuclearSiteUpkeepCost;

        /// <summary>
        ///     核兵器生産補正
        /// </summary>
        public double NukesProductionModifier;

        /// <summary>
        ///     超過備蓄損失割合
        /// </summary>
        public double OverStockpileLimitDailyLoss;

        /// <summary>
        ///     生産ラインの編集
        /// </summary>
        public bool ProductionLineEdit;

        /// <summary>
        ///     建物修復速度補正
        /// </summary>
        public double ProvinceBuildingsRepairModifier;

        /// <summary>
        ///     非同盟国にプロヴィンスの売却/譲渡を許可
        /// </summary>
        public int ProvinceCanSoldNonAllied;

        /// <summary>
        ///     プロヴィンス効率上昇時間
        /// </summary>
        public int ProvinceEfficiencyRiseTime;

        /// <summary>
        ///     資源回復速度補正
        /// </summary>
        public double ProvinceResourceRepairModifier;

        /// <summary>
        ///     補充に必要なICの比率
        /// </summary>
        public double ReinforceCost;

        /// <summary>
        ///     補充に必要な人的資源の比率
        /// </summary>
        public double ReinforceManpower;

        /// <summary>
        ///     補充に必要な時間の比率
        /// </summary>
        public double ReinforceTime;

        /// <summary>
        ///     改良のための補充係数
        /// </summary>
        public double ReinforceToUpdateModifier;

        /// <summary>
        ///     諜報任務発覚時の友好度低下量
        /// </summary>
        public double RelationshipsHitDetectedMissions;

        /// <summary>
        ///     政策スライダーに影響を与えるためのIC比率
        /// </summary>
        public double RequirementAffectSlider;

        /// <summary>
        ///     不要な資源/燃料の回収比率
        /// </summary>
        public double ResourceConvoysBackUnneeded;

        /// <summary>
        ///     非中核州の資源補正
        /// </summary>
        public double ResourceMultiplierNonNational;

        /// <summary>
        ///     非中核州の資源補正(AI)
        /// </summary>
        public double ResourceMultiplierNonNationalAi;

        /// <summary>
        ///     占領地の資源補正
        /// </summary>
        public double ResourceMultiplierNonOwned;

        /// <summary>
        ///     属国の資源補正
        /// </summary>
        public double ResourceMultiplierPuppet;

        /// <summary>
        ///     空軍の物資再備蓄速度
        /// </summary>
        public double RestockSpeedAir;

        /// <summary>
        ///     陸軍の物資再備蓄速度
        /// </summary>
        public double RestockSpeedLand;

        /// <summary>
        ///     海軍の物資再備蓄速度
        /// </summary>
        public double RestockSpeedNaval;

        /// <summary>
        ///     ライン調整コスト補正
        /// </summary>
        public double RetoolingCost;

        /// <summary>
        ///     ライン調整資源補正
        /// </summary>
        public double RetoolingResource;

        /// <summary>
        ///     師団譲渡後配備可能になるまでの時間
        /// </summary>
        public int SendDivisionDays;

        /// <summary>
        ///     第三国の諜報活動を報告するか
        /// </summary>
        public int ShowThirdCountrySpyReports;

        /// <summary>
        ///     不満度によるクーデター成功率修正
        /// </summary>
        public double SpyCoupDissentModifier;

        /// <summary>
        ///     スパイ発見確率
        /// </summary>
        public double SpyDetectionChance;

        /// <summary>
        ///     情報の正確さ補正
        /// </summary>
        public double SpyInformationAccuracyModifier;

        /// <summary>
        ///     諜報任務の間隔
        /// </summary>
        public int SpyMissionDays;

        /// <summary>
        ///     諜報維持コスト
        /// </summary>
        public double SpyUpkeepCost;

        /// <summary>
        ///     潜水艦最大付属装備数
        /// </summary>
        public int SsMaxAttach;

        /// <summary>
        ///     原子力潜水艦最大付属装備数
        /// </summary>
        public int SsnMaxAttach;

        /// <summary>
        ///     資源備蓄上限補正
        /// </summary>
        public double StockpileLimitMultiplierResource;

        /// <summary>
        ///     物資/燃料備蓄上限補正
        /// </summary>
        public double StockpileLimitMultiplierSuppliesOil;

        /// <summary>
        ///     空軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyAirBattleAoD;

        /// <summary>
        ///     空軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyAirBattleDh;

        /// <summary>
        ///     空軍の爆撃時物資使用量補正
        /// </summary>
        public double SupplyAirBombing;

        /// <summary>
        ///     空軍の移動時物資使用量補正
        /// </summary>
        public double SupplyAirMoving;

        /// <summary>
        ///     空軍の待機時物資使用量補正
        /// </summary>
        public double SupplyAirStaticAoD;

        /// <summary>
        ///     空軍の待機時物資使用量補正
        /// </summary>
        public double SupplyAirStaticDh;

        /// <summary>
        ///     船団襲撃時物資使用量補正
        /// </summary>
        public double SupplyConvoyHunt;

        /// <summary>
        ///     陸軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyLandBattleAoD;

        /// <summary>
        ///     陸軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyLandBattleDh;

        /// <summary>
        ///     陸軍の砲撃時物資使用量補正
        /// </summary>
        public double SupplyLandBombing;

        /// <summary>
        ///     陸軍の移動時物資使用量補正
        /// </summary>
        public double SupplyLandMoving;

        /// <summary>
        ///     陸軍の待機時物資使用量補正
        /// </summary>
        public double SupplyLandStaticAoD;

        /// <summary>
        ///     海軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyNavalBattleAoD;

        /// <summary>
        ///     海軍の戦闘時物資使用量補正
        /// </summary>
        public double SupplyNavalBattleDh;

        /// <summary>
        ///     海軍の移動時物資使用量補正
        /// </summary>
        public double SupplyNavalMoving;

        /// <summary>
        ///     海軍の待機時物資使用量補正
        /// </summary>
        public double SupplyNavalStaticAoD;

        /// <summary>
        ///     海軍の待機時物資使用量補正
        /// </summary>
        public double SupplyNavalStaticDh;

        /// <summary>
        ///     空軍の物資備蓄量
        /// </summary>
        public double SupplyStockAir;

        /// <summary>
        ///     陸軍の物資備蓄量
        /// </summary>
        public double SupplyStockLand;

        /// <summary>
        ///     海軍の物資備蓄量
        /// </summary>
        public double SupplyStockNaval;

        /// <summary>
        ///     陸軍の待機時物資使用量補正
        /// </summary>
        public double SuppyLandStaticDh;

        /// <summary>
        ///     合成石油変換係数
        /// </summary>
        public double SyntheticOilConversionMultiplier;

        /// <summary>
        ///     合成石油工場維持コスト
        /// </summary>
        public double SyntheticOilSiteUpkeepCost;

        /// <summary>
        ///     合成希少資源変換係数
        /// </summary>
        public double SyntheticRaresConversionMultiplier;

        /// <summary>
        ///     合成希少資源工場維持コスト
        /// </summary>
        public double SyntheticRaresSiteUpkeepCost;

        /// <summary>
        ///     未配備の基地のTC負荷
        /// </summary>
        public double TcLoadBase;

        /// <summary>
        ///     攻勢時のTC負荷係数
        /// </summary>
        public double TcLoadFactorOffensive;

        /// <summary>
        ///     空軍師団のTC負荷補正
        /// </summary>
        public double TcLoadMultiplierAir;

        /// <summary>
        ///     陸軍師団のTC負荷補正
        /// </summary>
        public double TcLoadMultiplierLand;

        /// <summary>
        ///     海軍師団のTC負荷補正
        /// </summary>
        public double TcLoadMultiplierNaval;

        /// <summary>
        ///     占領地のTC負荷
        /// </summary>
        public double TcLoadOccupied;

        /// <summary>
        ///     パルチザンのTC負荷
        /// </summary>
        public double TcLoadPartisan;

        /// <summary>
        ///     プロヴィンス開発のTC負荷
        /// </summary>
        public double TcLoadProvinceDevelopment;

        /// <summary>
        ///     未配備旅団のTC負荷
        /// </summary>
        public double TcLoadUndeployedBrigade;

        /// <summary>
        ///     未配備師団のTC負荷
        /// </summary>
        public double TcLoadUndeployedDivision;

        /// <summary>
        ///     スライダー移動の間隔
        /// </summary>
        public int TimeBetweenSliderChangesAoD;

        /// <summary>
        ///     建物修復時間補正
        /// </summary>
        public double TimeRepairBuilding;

        /// <summary>
        ///     輸送艦最大付属装備数
        /// </summary>
        public int TpMaxAttach;

        /// <summary>
        ///     輸送艦の輸送船団への変換比率
        /// </summary>
        public double TpTransportsConversionRatio;

        /// <summary>
        ///     占領中の同盟国の中核州返還を許可
        /// </summary>
        public bool TransferAlliedCoreProvinces;

        /// <summary>
        ///     輸送艦変換係数
        /// </summary>
        public double TransportConversion;

        /// <summary>
        ///     戦闘による損失からの復帰係数
        /// </summary>
        public double TrickleBackFactorManpower;

        /// <summary>
        ///     改良に必要なICの比率
        /// </summary>
        public double UpgradeCost;

        /// <summary>
        ///     改良に必要な時間の比率
        /// </summary>
        public double UpgradeTime;
    }

    /// <summary>
    ///     intelligenceセクション
    /// </summary>
    public class MiscIntelligence
    {
        /// <summary>
        /// 諜報任務の間隔
        /// </summary>
        public int SpyMissionDaysDh;

        /// <summary>
        /// 諜報レベルの増加間隔
        /// </summary>
        public int IncreateIntelligenceLevelDaysDh;

        /// <summary>
        /// 国内の諜報活動を発見する確率
        /// </summary>
        public double ChanceDetectSpyMissionDh;

        /// <summary>
        /// 諜報任務発覚時の友好度低下量
        /// </summary>
        public double RelationshipsHitDetectedMissionsDh;

        /// <summary>
        /// 諜報任務の距離補正
        /// </summary>
        public double DistanceModifier;

        /// <summary>
        /// 諜報任務の近隣国補正
        /// </summary>
        public double DistanceModifierNeighboursDh;

        /// <summary>
        /// 諜報レベルの距離補正
        /// </summary>
        public double SpyLevelBonusDistanceModifier;

        /// <summary>
        /// 諜報レベル10超過時の距離補正
        /// </summary>
        public double SpyLevelBonusDistanceModifierAboveTen;

        /// <summary>
        /// 情報の正確さ補正
        /// </summary>
        public double SpyInformationAccuracyModifierDh;

        /// <summary>
        /// 諜報コストのIC補正
        /// </summary>
        public double IcModifierCost;

        /// <summary>
        /// 諜報コスト補正の最小IC
        /// </summary>
        public double MinIcCostModifier;

        /// <summary>
        /// 諜報コスト補正の最大IC
        /// </summary>
        public double MaxIcCostModifierDh;

        /// <summary>
        /// 諜報レベル10超過時追加維持コスト
        /// </summary>
        public double ExtraMaintenanceCostAboveTen;

        /// <summary>
        /// 諜報レベル10超過時増加コスト
        /// </summary>
        public double ExtraCostIncreasingAboveTen;

        /// <summary>
        /// 第三国の諜報活動を報告するか
        /// </summary>
        public int ShowThirdCountrySpyReportsDh;

        /// <summary>
        /// 諜報資金割り当て補正
        /// </summary>
        public double SpiesMoneyModifier;
    }

    /// <summary>
    ///     diplomacyセクション
    /// </summary>
    public class MiscDiplomacy
    {
        /// <summary>
        /// 外交官派遣間隔
        /// </summary>
        public int DaysBetweenDiplomaticMissions;

        /// <summary>
        /// スライダー移動の間隔
        /// </summary>
        public int TimeBetweenSliderChangesDh;

        /// <summary>
        /// 政策スライダーに影響を与えるためのIC比率
        /// </summary>
        public double RequirementAffectSliderDh;

        /// <summary>
        /// 閣僚交代時に閣僚特性を適用する
        /// </summary>
        public bool UseMinisterPersonalityReplacing;

        /// <summary>
        /// 貿易キャンセル時の友好度低下
        /// </summary>
        public double RelationshipHitCancelTrade;

        /// <summary>
        /// 永久貿易キャンセル次の友好度低下
        /// </summary>
        public double RelationshipHitCancelPermanentTrade;

        /// <summary>
        /// 属国が宗主国の同盟に強制参加する
        /// </summary>
        public bool PuppetsJoinMastersAlliance;

        /// <summary>
        /// 属国の属国が設立できるか
        /// </summary>
        public bool MastersBecomePuppetsPuppets;

        /// <summary>
        /// 領有権主張の変更
        /// </summary>
        public bool AllowManualClaimsChange;

        /// <summary>
        /// 領有権主張時の好戦性上昇値
        /// </summary>
        public double BelligerenceClaimedProvince;

        /// <summary>
        /// 領有権撤回時の好戦性減少値
        /// </summary>
        public double BelligerenceClaimsRemoval;

        /// <summary>
        /// 宣戦布告された時に対抗陣営へ自動加盟
        /// </summary>
        public bool JoinAutomaticallyAllesAxis;

        /// <summary>
        /// 国家元首/政府首班の交代
        /// </summary>
        public int AllowChangeHosHog;

        /// <summary>
        /// クーデター発生時に兄弟国へ変更
        /// </summary>
        public bool ChangeTagCoup;

        /// <summary>
        /// 独立可能国設定
        /// </summary>
        public int FilterReleaseCountries;
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
    }
}