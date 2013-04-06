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
}