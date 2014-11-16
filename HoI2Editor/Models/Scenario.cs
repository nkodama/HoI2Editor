using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     シナリオデータ
    /// </summary>
    public class Scenario
    {
        #region 公開プロパティ

        /// <summary>
        ///     シナリオ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     パネル画像名
        /// </summary>
        public string PanelName { get; set; }

        /// <summary>
        ///     シナリオヘッダ
        /// </summary>
        public ScenarioHeader Header { get; set; }

        /// <summary>
        ///     シナリオグローバルデータ
        /// </summary>
        public ScenarioGlobalData GlobalData { get; set; }

        /// <summary>
        ///     イベントファイル
        /// </summary>
        public List<string> Events { get; private set; }

        /// <summary>
        ///     インクルードファイル
        /// </summary>
        public List<string> Includes { get; private set; }

        /// <summary>
        ///     プロヴィンス情報 (国別incに記載)
        /// </summary>
        public List<ScenarioProvinceInfo> CountryProvinces { get; private set; }

        /// <summary>
        ///     プロヴィンス情報 (bases.incに記載)
        /// </summary>
        public List<ScenarioProvinceInfo> BasesProvinces { get; private set; }

        /// <summary>
        ///     プロヴィンス情報 (bases_DOD.incに記載)
        /// </summary>
        public List<ScenarioProvinceInfo> BasesDodProvinces { get; private set; }

        /// <summary>
        ///     プロヴィンス情報 (vp.incに記載)
        /// </summary>
        public List<ScenarioProvinceInfo> VpProvinces { get; private set; }

        /// <summary>
        ///     国家情報
        /// </summary>
        public List<ScenarioCountryInfo> Countries { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Scenario()
        {
            Events = new List<string>();
            Includes = new List<string>();
            CountryProvinces = new List<ScenarioProvinceInfo>();
            BasesProvinces = new List<ScenarioProvinceInfo>();
            BasesDodProvinces = new List<ScenarioProvinceInfo>();
            VpProvinces = new List<ScenarioProvinceInfo>();
            Countries = new List<ScenarioCountryInfo>();
        }

        #endregion
    }

    /// <summary>
    ///     シナリオヘッダ
    /// </summary>
    public class ScenarioHeader
    {
        #region 公開プロパティ

        /// <summary>
        ///     シナリオヘッダ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     ヘッダ開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     選択可能国家
        /// </summary>
        public List<Country> Selectable { get; private set; }

        /// <summary>
        ///     主要国情報
        /// </summary>
        public List<MajorCountryInfo> Majors { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioHeader()
        {
            StartDate = new GameDate();
            Selectable = new List<Country>();
            Majors = new List<MajorCountryInfo>();
        }

        #endregion
    }

    /// <summary>
    ///     シナリオグローバルデータ
    /// </summary>
    public class ScenarioGlobalData
    {
        #region 公開プロパティ

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     終了日時
        /// </summary>
        public GameDate EndDate { get; set; }

        /// <summary>
        ///     枢軸国
        /// </summary>
        public AllianceInfo Axis { get; set; }

        /// <summary>
        ///     連合国
        /// </summary>
        public AllianceInfo Allies { get; set; }

        /// <summary>
        ///     共産国
        /// </summary>
        public AllianceInfo Comintern { get; set; }

        /// <summary>
        ///     同盟国情報
        /// </summary>
        public List<AllianceInfo> Alliances { get; private set; }

        /// <summary>
        ///     戦争情報
        /// </summary>
        public List<WarInfo> Wars { get; private set; }

        /// <summary>
        ///     外交協定情報
        /// </summary>
        public List<TreatyInfo> Treaties { get; private set; }

        /// <summary>
        ///     休止指揮官
        /// </summary>
        public List<int> DormantLeaders { get; private set; }

        /// <summary>
        ///     休止閣僚
        /// </summary>
        public List<int> DormantMinisters { get; private set; }

        /// <summary>
        ///     休止研究機関
        /// </summary>
        public List<int> DormantTeams { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioGlobalData()
        {
            Alliances = new List<AllianceInfo>();
            Wars = new List<WarInfo>();
            Treaties = new List<TreatyInfo>();
            DormantLeaders = new List<int>();
            DormantMinisters = new List<int>();
            DormantTeams = new List<int>();
        }

        #endregion
    }

    /// <summary>
    ///     主要国情報
    /// </summary>
    public class MajorCountryInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     プロパガンダ画像名
        /// </summary>
        public string PictureName { get; set; }

        #endregion
    }

    /// <summary>
    ///     同盟情報
    /// </summary>
    public class AllianceInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     参加国
        /// </summary>
        public List<Country> Participant { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public AllianceInfo()
        {
            Participant = new List<Country>();
        }

        #endregion
    }

    /// <summary>
    ///     戦争情報
    /// </summary>
    public class WarInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     終了日時
        /// </summary>
        public GameDate EndDate { get; set; }

        /// <summary>
        ///     攻撃側参加国
        /// </summary>
        public AllianceInfo Attackers { get; set; }

        /// <summary>
        ///     防御側参加国
        /// </summary>
        public AllianceInfo Defenders { get; set; }

        #endregion
    }

    /// <summary>
    ///     外交協定情報
    /// </summary>
    public class TreatyInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     外交協定の種類
        /// </summary>
        public TreatyType Type { get; set; }

        /// <summary>
        ///     対象国1
        /// </summary>
        public Country Country1 { get; set; }

        /// <summary>
        ///     対象国2
        /// </summary>
        public Country Country2 { get; set; }

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     失効日時
        /// </summary>
        public GameDate ExpiryDate { get; set; }

        /// <summary>
        ///     資金
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        ///     物資
        /// </summary>
        public double Supplies { get; set; }

        /// <summary>
        ///     エネルギー
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        ///     金属
        /// </summary>
        public double Metal { get; set; }

        /// <summary>
        ///     希少資源
        /// </summary>
        public double RareMaterials { get; set; }

        /// <summary>
        ///     石油
        /// </summary>
        public double Oil { get; set; }

        /// <summary>
        ///     取り消し可能かどうか
        /// </summary>
        public bool Cancel { get; set; }

        #endregion
    }

    /// <summary>
    ///     シナリオプロヴィンス情報
    /// </summary>
    public class ScenarioProvinceInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     プロヴィンスID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     工場のサイズ
        /// </summary>
        public BuildingSize Ic { get; set; }

        /// <summary>
        ///     インフラのサイズ
        /// </summary>
        public BuildingSize Infrastructure { get; set; }

        /// <summary>
        ///     陸上要塞のサイズ
        /// </summary>
        public BuildingSize LandFort { get; set; }

        /// <summary>
        ///     沿岸要塞のサイズ
        /// </summary>
        public BuildingSize CoastalFort { get; set; }

        /// <summary>
        ///     対空砲のサイズ
        /// </summary>
        public BuildingSize AntiAir { get; set; }

        /// <summary>
        ///     空軍基地のサイズ
        /// </summary>
        public BuildingSize AirBase { get; set; }

        /// <summary>
        ///     海軍基地のサイズ
        /// </summary>
        public BuildingSize NavalBase { get; set; }

        /// <summary>
        ///     レーダー基地のサイズ
        /// </summary>
        public BuildingSize RadarStation { get; set; }

        /// <summary>
        ///     原子炉のサイズ
        /// </summary>
        public BuildingSize NuclearReactor { get; set; }

        /// <summary>
        ///     ロケット試験場のサイズ
        /// </summary>
        public BuildingSize RocketTest { get; set; }

        /// <summary>
        ///     合成石油工場のサイズ
        /// </summary>
        public BuildingSize SyntheticOil { get; set; }

        /// <summary>
        ///     合成素材工場のサイズ
        /// </summary>
        public BuildingSize SyntheticRares { get; set; }

        /// <summary>
        ///     原子力発電所のサイズ
        /// </summary>
        public BuildingSize NuclearPower { get; set; }

        /// <summary>
        ///     物資の備蓄量
        /// </summary>
        public double SupplyPool { get; set; }

        /// <summary>
        ///     石油の備蓄量
        /// </summary>
        public double OilPool { get; set; }

        /// <summary>
        ///     勝利ポイント
        /// </summary>
        public int Vp { get; set; }

        #endregion
    }

    /// <summary>
    ///     建物のサイズ
    /// </summary>
    public class BuildingSize
    {
        #region 公開プロパティ

        /// <summary>
        ///     相対サイズ
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        ///     最大サイズ
        /// </summary>
        public double MaxSize { get; set; }

        /// <summary>
        ///     現在のサイズ
        /// </summary>
        public double CurrentSize { get; set; }

        #endregion
    }

    /// <summary>
    ///     シナリオ国家情報
    /// </summary>
    public class ScenarioCountryInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     宗主国
        /// </summary>
        public Country Master { get; set; }

        /// <summary>
        ///     統帥権取得国
        /// </summary>
        public Country Control { get; set; }

        /// <summary>
        ///     好戦性
        /// </summary>
        public int Belligerence { get; set; }

        /// <summary>
        ///     首都のプロヴィンスID
        /// </summary>
        public int Capital { get; set; }

        /// <summary>
        ///     人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     エネルギー
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        ///     金属
        /// </summary>
        public double Metal { get; set; }

        /// <summary>
        ///     希少資源
        /// </summary>
        public double RareMaterials { get; set; }

        /// <summary>
        ///     石油
        /// </summary>
        public double Oil { get; set; }

        /// <summary>
        ///     物資
        /// </summary>
        public double Supplies { get; set; }

        /// <summary>
        ///     資金
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        ///     輸送船団
        /// </summary>
        public int Transports { get; set; }

        /// <summary>
        ///     護衛艦
        /// </summary>
        public int Escorts { get; set; }

        /// <summary>
        ///     外交情報
        /// </summary>
        public List<RelationInfo> Diplomacy { get; private set; }

        /// <summary>
        ///     諜報情報
        /// </summary>
        public List<SpyInfo> Intelligence { get; private set; }

        /// <summary>
        ///     中核プロヴィンス
        /// </summary>
        public List<int> NationalProvinces { get; private set; }

        /// <summary>
        ///     保有プロヴィンス
        /// </summary>
        public List<int> OwnedProvinces { get; private set; }

        /// <summary>
        ///     支配プロヴィンス
        /// </summary>
        public List<int> ControlledProvinces { get; private set; }

        /// <summary>
        ///     保有技術
        /// </summary>
        public List<int> TechApps { get; private set; }

        /// <summary>
        ///     青写真
        /// </summary>
        public List<int> BluePrints { get; private set; }

        /// <summary>
        ///     政策スライダー
        /// </summary>
        public CountryPolicy Policy { get; set; }

        /// <summary>
        ///     国家元首
        /// </summary>
        public TypeId HeadOfState { get; set; }

        /// <summary>
        ///     政府首班
        /// </summary>
        public TypeId HeadOfGovernment { get; set; }

        /// <summary>
        ///     外務大臣
        /// </summary>
        public TypeId ForeignMinister { get; set; }

        /// <summary>
        ///     軍需大臣
        /// </summary>
        public TypeId ArmamentMinister { get; set; }

        /// <summary>
        ///     内務大臣
        /// </summary>
        public TypeId MinisterOfSecurity { get; set; }

        /// <summary>
        ///     情報大臣
        /// </summary>
        public TypeId MinisterOfIntelligence { get; set; }

        /// <summary>
        ///     統合参謀総長
        /// </summary>
        public TypeId ChiefOfStaff { get; set; }

        /// <summary>
        ///     陸軍総司令官
        /// </summary>
        public TypeId ChiefOfArmy { get; set; }

        /// <summary>
        ///     海軍総司令官
        /// </summary>
        public TypeId ChiefOfNavy { get; set; }

        /// <summary>
        ///     空軍総司令官
        /// </summary>
        public TypeId ChiefOfAir { get; set; }

        /// <summary>
        ///     陸軍ユニット
        /// </summary>
        public List<ScenarioUnit> LandUnits { get; private set; }

        /// <summary>
        ///     海軍ユニット
        /// </summary>
        public List<ScenarioUnit> NavalUnits { get; private set; }

        /// <summary>
        ///     空軍ユニット
        /// </summary>
        public List<ScenarioUnit> AirUnits { get; private set; }

        /// <summary>
        ///     生産中の師団
        /// </summary>
        public List<DivisionDevelopment> DivisionDevelopments { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioCountryInfo()
        {
            Diplomacy = new List<RelationInfo>();
            Intelligence = new List<SpyInfo>();
            NationalProvinces = new List<int>();
            OwnedProvinces = new List<int>();
            ControlledProvinces = new List<int>();
            TechApps = new List<int>();
            BluePrints = new List<int>();
            LandUnits = new List<ScenarioUnit>();
            NavalUnits = new List<ScenarioUnit>();
            AirUnits = new List<ScenarioUnit>();
            DivisionDevelopments = new List<DivisionDevelopment>();
        }

        #endregion
    }

    /// <summary>
    ///     国家関係情報
    /// </summary>
    public class RelationInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     相手国
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     関係値
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        ///     通行許可の有無
        /// </summary>
        public bool Access { get; set; }

        #endregion
    }

    /// <summary>
    ///     国家諜報情報
    /// </summary>
    public class SpyInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     相手国
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     スパイの数
        /// </summary>
        public int Spies { get; set; }

        #endregion
    }

    /// <summary>
    ///     政策スライダー
    /// </summary>
    public class CountryPolicy
    {
        #region 公開プロパティ

        /// <summary>
        ///     スライダー移動可能日時
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     民主的 - 独裁的
        /// </summary>
        public int Democratic { get; set; }

        /// <summary>
        ///     政治的左派 - 政治的右派
        /// </summary>
        public int PoliticalLeft { get; set; }

        /// <summary>
        ///     開放社会 - 閉鎖社会
        /// </summary>
        public int Freedom { get; set; }

        /// <summary>
        ///     自由経済 - 中央計画経済
        /// </summary>
        public int FreeMarket { get; set; }

        /// <summary>
        ///     常備軍 - 徴兵軍 (DH Fullでは動員 - 復員)
        /// </summary>
        public int ProfessionalArmy { get; set; }

        /// <summary>
        ///     タカ派 - ハト派
        /// </summary>
        public int DefenseLobby { get; set; }

        /// <summary>
        ///     介入主義 - 孤立主義
        /// </summary>
        public int Interventionism { get; set; }

        #endregion
    }

    /// <summary>
    ///     ユニット情報
    /// </summary>
    public class ScenarioUnit
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     ユニット名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     所属基地 (海軍/空軍)
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        ///     師団
        /// </summary>
        public List<ScenarioDivision> Divisions { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioUnit()
        {
            Divisions = new List<ScenarioDivision>();
        }

        #endregion
    }

    /// <summary>
    ///     師団情報
    /// </summary>
    public class ScenarioDivision
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     師団名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     ユニット種類
        /// </summary>
        public UnitType Type { get; set; }

        /// <summary>
        ///     充足率
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     未定義のモデル番号
        /// </summary>
        public const int UndefinedModelNo = -1;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioDivision()
        {
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
        }

        #endregion
    }

    /// <summary>
    ///     生産中師団情報
    /// </summary>
    public class DivisionDevelopment
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     師団名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     必要IC
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        ///     完了予定日
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     ユニット種類
        /// </summary>
        public UnitType Type { get; set; }

        /// <summary>
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     未定義のモデル番号
        /// </summary>
        public const int UndefinedModelNo = -1;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DivisionDevelopment()
        {
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
        }

        #endregion
    }

    /// <summary>
    ///     typeとidの組
    /// </summary>
    public class TypeId
    {
        #region 公開プロパティ

        /// <summary>
        ///     id
        /// </summary>
        public int Id;

        /// <summary>
        ///     type
        /// </summary>
        public int Type;

        #endregion
    }

    /// <summary>
    ///     外交協定の種類
    /// </summary>
    public enum TreatyType
    {
        None,
        NonAggression, // 不可侵条約
        Peace, // 休戦協定
        Trade, // 貿易
    }
}