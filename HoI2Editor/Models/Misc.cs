using HoI2Editor.Parsers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     miscファイルの設定項目
    /// </summary>
    public static class Misc
    {
        /// <summary>
        ///     economyセクション
        /// </summary>
        public static MiscEconomy Economy;

        /// <summary>
        ///     intelligenceセクション
        /// </summary>
        public static MiscIntelligence Intelligence;

        /// <summary>
        ///     diplomacyセクション
        /// </summary>
        public static MiscDiplomacy Diplomacy;

        /// <summary>
        ///     combatセクション
        /// </summary>
        public static MiscCombat Combat;

        /// <summary>
        ///     missionセクション
        /// </summary>
        public static MiscMission Mission;

        /// <summary>
        ///     countryセクション
        /// </summary>
        public static MiscCountry Country;

        /// <summary>
        ///     researchセクション
        /// </summary>
        public static MiscResearch Research;

        /// <summary>
        ///     tradeセクション
        /// </summary>
        public static MiscTrade Trade;

        /// <summary>
        ///     aiセクション
        /// </summary>
        public static MiscAi Ai;

        /// <summary>
        ///     modセクション
        /// </summary>
        public static MiscMod Mod;

        /// <summary>
        ///     mapセクション
        /// </summary>
        public static MiscMap Map;

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

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
        /// <summary>
        ///     AIのスパイ/外交活動をログに記録する [0:無効/1:有効]
        /// </summary>
        public int AiSpyDiplomacyLogger;

        /// <summary>
        ///     自国領土外でも補給が届いていれば旅団の配備を可能にする [0:無効/1:有効]
        /// </summary>
        public int AllowBrigadeAttachingInSupply;

        /// <summary>
        ///     全ての陸地プロヴィンスで固有の画像を許可する [0:無効/1:有効]
        /// </summary>
        public int AllowUniquePictureAllLandProvinces;

        /// <summary>
        ///     プレイヤー国のイベントに自動応答する [0:無効/1:有効]
        /// </summary>
        public int AutoReplyEvents;

        /// <summary>
        ///     建物をプロヴィンスからのみ生産可能にする
        ///     [0:無効/1:有効(港/飛行場/原子炉/ロケット試験所)/2:1に加えて対空砲/レーダーも]
        /// </summary>
        public int BuildableOnlyProvince;

        /// <summary>
        ///     国家の状態をログに記録する [0:無効/>0:指定日数毎に記録/-1:プレイヤー国のみ]
        /// </summary>
        public int CountryLogger;

        /// <summary>
        ///     備蓄庫の統合/再配置の計算間隔 [0:DDAのシステムを使用/>0:指定日数毎に計算]
        /// </summary>
        public int DepotCalculationInterval;

        /// <summary>
        ///     ディシジョンを有効にする [0:無効/1:有効]
        /// </summary>
        public int EnableDecisionsPlayer;

        /// <summary>
        ///     無効なアクションを表示する [0:無効/1:有効なトリガーがあれば表示/2:常に表示]
        /// </summary>
        public int ForceActionsToShowNoValidCommands;

        /// <summary>
        ///     AIをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public int LoadAiModdirOnly;

        /// <summary>
        ///     マルチプレイで新しいAI設定/切り替えを使用する [0:無効/1:有効]
        /// </summary>
        public int LoadNewAiSettingMultiPlayer;

        /// <summary>
        ///     スプライトをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public int LoadSpriteModdirOnly;

        /// <summary>
        ///     ユニットアイコンをMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public int LoadUnitIconModdirOnly;

        /// <summary>
        ///     ユニット画像をMODDIRからのみ読み込む [0:無効/有効]
        /// </summary>
        public int LoadUnitPictureModdirOnly;

        /// <summary>
        ///     損失をログに記録する
        ///     [0:無効/1:有効(艦船/輸送船団)/2:装備の損失も含む]
        ///     [3:捕獲されたユニットの装備の損失も含む/4:消耗も含む]
        /// </summary>
        public int LossesLogger;

        /// <summary>
        ///     一括配備数(空軍)
        /// </summary>
        public int MultipleDeploymentSizeAir;

        /// <summary>
        ///     一括配備数(陸軍)
        /// </summary>
        public int MultipleDeploymentSizeArmy;

        /// <summary>
        ///     一括配備数(海軍)
        /// </summary>
        public int MultipleDeploymentSizeFleet;

        /// <summary>
        ///     新しい自動セーブファイル名フォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public int NewAutoSaveFileFormat;

        /// <summary>
        ///     新閣僚フォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public bool NewMinisterFormat;

        /// <summary>
        ///     生産画面のUIスタイル [0:従来スタイル/1:新スタイル]
        /// </summary>
        public int ProductionPanelStyle;

        /// <summary>
        ///     反乱軍の構成 [0:民兵のみ/100:歩兵のみ]
        /// </summary>
        public int RebelArmyComposition;

        /// <summary>
        ///     反乱軍の最大戦力 [1-100]
        /// </summary>
        public double RebelArmyMaxStr;

        /// <summary>
        ///     反乱軍の最小戦力 [1-100]
        /// </summary>
        public double RebelArmyMinStr;

        /// <summary>
        ///     反乱軍の指揮統制率回復
        /// </summary>
        public double RebelArmyOrgRegain;

        /// <summary>
        ///     反乱軍の技術レベル [-1:モデル1/0:最新モデル/>0:最新モデルから指定数幅のランダム]
        /// </summary>
        public int RebelArmyTechLevel;

        /// <summary>
        ///     海軍/空軍基地のあるプロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusBase;

        /// <summary>
        ///     隣接プロヴィンスが反乱軍に支配されている時の反乱危険率増加値
        /// </summary>
        public double RebelBonusControlledRebel;

        /// <summary>
        ///     砂漠プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusDesert;

        /// <summary>
        ///     森林プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusForest;

        /// <summary>
        ///     丘陵プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusHill;

        /// <summary>
        ///     密林プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusJungle;

        /// <summary>
        ///     山岳プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusMountain;

        /// <summary>
        ///     隣接プロヴィンスが敵軍に占領されている時の反乱危険率増加値
        /// </summary>
        public double RebelBonusOccupiedEnemy;

        /// <summary>
        ///     平地プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusPlain;

        /// <summary>
        ///     湿地プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusSwamp;

        /// <summary>
        ///     都市プロヴィンスの反乱危険率増加値
        /// </summary>
        public double RebelBonusUrban;

        /// <summary>
        ///     指揮官の引退年を有効にする [0:無効/1:有効]
        /// </summary>
        public bool RetirementYearLeader;

        /// <summary>
        ///     閣僚の引退年を有効にする [0:無効/1:有効]
        /// </summary>
        public bool RetirementYearMinister;

        /// <summary>
        ///     反乱軍消滅後にプロヴィンスが元の所有国に復帰するまでの月数 [100000:復帰しない]
        /// </summary>
        public int ReturnMonthNoRebelArmy;

        /// <summary>
        ///     AI切り替えをログに記録する [0:無効/1:有効]
        /// </summary>
        public int SwitchedAiLogger;

        /// <summary>
        ///     新しい貿易システムの計算間隔 [0:DDAのシステムを使用/>0:指定日数毎に計算]
        /// </summary>
        public int TradeEfficiencyCalculationInterval;

        /// <summary>
        ///     ユニット補正の統計ページを新スタイルにする閾値
        /// </summary>
        public int UnitModifierStatisticsPage;

        /// <summary>
        ///     ユニット画像のサイズ [0:従来サイズ(192x104)/1:新サイズ(360x160)]
        /// </summary>
        public int UnitPictureSize;

        /// <summary>
        ///     DH1.02より前のセーブデータフォーマットを使用する [0:無効/1:有効]
        /// </summary>
        public int UseOldSaveGameFormat;

        /// <summary>
        ///     艦船装備に画像を使用する [0:アイコンを使用/1:画像を使用]
        /// </summary>
        public int UsePictureNavalBrigade;

        /// <summary>
        ///     移動不可ユニットの判定に速度の値を使用する [0:守備隊のみ移動不可/1:速度0ならば移動不可]
        /// </summary>
        public int UseSpeedGarrisonStatus;
    }

    /// <summary>
    ///     mapセクション
    /// </summary>
    public class MiscMap
    {
        /// <summary>
        ///     距離計算モデル [0:従来モデル/1:新モデル]
        /// </summary>
        public int DistanceCalculationModel;

        /// <summary>
        ///     マップの高さ
        /// </summary>
        public int MapHeight = 11520;

        /// <summary>
        ///     マップの番号 [0:デフォルトマップを使用/その他:map\map_Xを使用]
        /// </summary>
        public int MapNo;

        /// <summary>
        ///     マップの幅
        /// </summary>
        public int MapWidth = 29952;

        /// <summary>
        ///     総プロヴィンス数
        /// </summary>
        public int TotalProvinces = 2608;
    }
}