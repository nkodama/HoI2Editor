using System;
using System.Collections.Generic;
using System.Text;
using HoI2Editor.Utilities;

namespace HoI2Editor.Models
{

    #region シナリオデータ

    /// <summary>
    ///     シナリオデータ
    /// </summary>
    public class Scenario
    {
        #region 公開プロパティ

        /// <summary>
        ///     保存ゲームかどうか
        /// </summary>
        public bool IsSaveGame { get; set; }

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
        ///     発生済みイベント
        /// </summary>
        public List<int> HistoryEvents { get; private set; }

        /// <summary>
        ///     休止イベント
        /// </summary>
        public List<int> SleepEvents { get; private set; }

        /// <summary>
        ///     イベント発生日時
        /// </summary>
        public Dictionary<int, GameDate> SaveDates { get; set; }

        /// <summary>
        ///     マップ設定
        /// </summary>
        public MapSettings Map { get; set; }

        /// <summary>
        ///     イベントファイル
        /// </summary>
        public List<string> EventFiles { get; private set; }

        /// <summary>
        ///     インクルードファイル
        /// </summary>
        public List<string> IncludeFiles { get; private set; }

        /// <summary>
        ///     インクルードフォルダ
        /// </summary>
        public string IncludeFolder { get; set; }

        /// <summary>
        ///     プロヴィンス設定
        /// </summary>
        public List<ProvinceSettings> Provinces { get; private set; }

        /// <summary>
        ///     国別incにプロヴィンス設定を定義するかどうか
        /// </summary>
        public bool IsCountryProvinceSettings { get; set; }

        /// <summary>
        ///     bases.incにプロヴィンス設定を定義するかどうか
        /// </summary>
        public bool IsBaseProvinceSettings { get; set; }

        /// <summary>
        ///     bases_DOD.incにプロヴィンス設定を定義するかどうか
        /// </summary>
        public bool IsBaseDodProvinceSettings { get; set; }

        /// <summary>
        ///     depots.incにプロヴィンス設定を定義するかどうか
        /// </summary>
        public bool IsDepotsProvinceSettings { get; set; }

        /// <summary>
        ///     vp.incにプロヴィンス設定を定義するかどうか
        /// </summary>
        public bool IsVpProvinceSettings { get; set; }

        /// <summary>
        ///     国家情報
        /// </summary>
        public List<CountrySettings> Countries { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        /// <summary>
        ///     選択可能国の編集済みフラグ
        /// </summary>
        private readonly HashSet<Country> _dirtySelectableCountries = new HashSet<Country>();

        /// <summary>
        ///     プロヴィンス設定の編集済みフラグ
        /// </summary>
        private bool _dirtyProvinces;

        /// <summary>
        ///     vp.incの編集済みフラグ
        /// </summary>
        private bool _dirtyVpInc;

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Name, // シナリオ名
            PanelName, // パネル画像名
            IncludeFolder, // インクルードフォルダ
            FreeSelection, // 国家の自由選択
            BattleScenario, // ショートシナリオ
            AiAggressive, // AIの攻撃性
            Difficulty, // 難易度
            GameSpeed, // ゲームスピード
            AllowDiplomacy, // 外交を許可
            AllowProduction, // 生産を許可
            AllowTechnology, // 技術開発を許可
            StartYear, // 開始年
            StartMonth, // 開始月
            StartDay, // 開始日
            EndYear, // 終了年
            EndMonth, // 終了月
            EndDay // 終了日
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Scenario()
        {
            HistoryEvents = new List<int>();
            SleepEvents = new List<int>();
            EventFiles = new List<string>();
            IncludeFiles = new List<string>();
            Provinces = new List<ProvinceSettings>();
            Countries = new List<CountrySettings>();
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の選択可能国が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtySelectableCountry(Country country)
        {
            return _dirtySelectableCountries.Contains(country);
        }

        /// <summary>
        ///     選択可能国の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">対象国</param>
        public void SetDirtySelectableCountry(Country country)
        {
            _dirtySelectableCountries.Add(country);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     プロヴィンスデータが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyProvinces()
        {
            return _dirtyProvinces;
        }

        /// <summary>
        ///     プロヴィンスデータの編集済みフラグを設定する
        /// </summary>
        public void SetDirtyProvinces()
        {
            _dirtyProvinces = true;
        }

        /// <summary>
        ///     vp.incが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyVpInc()
        {
            return _dirtyVpInc;
        }

        /// <summary>
        ///     vp.incの編集済みフラグを設定する
        /// </summary>
        public void SetDirtyVpInc()
        {
            _dirtyVpInc = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }

            _dirtySelectableCountries.Clear();

            if ((Header != null) && (Header.MajorCountries != null))
            {
                foreach (MajorCountrySettings major in Header.MajorCountries)
                {
                    major.ResetDirtyAll();
                }
            }

            if (GlobalData != null)
            {
                if (GlobalData.Axis != null)
                {
                    GlobalData.Axis.ResetDirtyAll();
                }
                if (GlobalData.Allies != null)
                {
                    GlobalData.Allies.ResetDirtyAll();
                }
                if (GlobalData.Comintern != null)
                {
                    GlobalData.Comintern.ResetDirtyAll();
                }
                foreach (Alliance alliance in GlobalData.Alliances)
                {
                    alliance.ResetDirtyAll();
                }
                foreach (War war in GlobalData.Wars)
                {
                    war.ResetDirtyAll();
                }
                foreach (Treaty nonAggression in GlobalData.NonAggressions)
                {
                    nonAggression.ResetDirtyAll();
                }
                foreach (Treaty peace in GlobalData.Peaces)
                {
                    peace.ResetDirtyAll();
                }
                foreach (Treaty trade in GlobalData.Trades)
                {
                    trade.ResetDirtyAll();
                }
            }

            foreach (CountrySettings settings in Countries)
            {
                settings.ResetDirtyAll();
            }

            foreach (ProvinceSettings settings in Provinces)
            {
                settings.ResetDirtyAll();
            }

            _dirtyProvinces = false;
            _dirtyVpInc = false;

            _dirtyFlag = false;
        }

        #endregion
    }

    #endregion

    #region シナリオヘッダ

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
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     国家の自由選択
        /// </summary>
        public bool IsFreeSelection { get; set; }

        /// <summary>
        ///     ショートシナリオ
        /// </summary>
        public bool IsBattleScenario { get; set; }

        /// <summary>
        ///     選択可能国家
        /// </summary>
        public List<Country> SelectableCountries { get; private set; }

        /// <summary>
        ///     主要国設定
        /// </summary>
        public List<MajorCountrySettings> MajorCountries { get; private set; }

        /// <summary>
        ///     AIの攻撃性
        /// </summary>
        public int AiAggressive { get; set; }

        /// <summary>
        ///     難易度
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        ///     ゲームスピード
        /// </summary>
        public int GameSpeed { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     AIの攻撃性の初期値
        /// </summary>
        public const int AiAggressiveDefault = 2;

        /// <summary>
        ///     難易度の初期値
        /// </summary>
        public const int DifficultyDefault = 2;

        /// <summary>
        ///     ゲームスピードの初期値
        /// </summary>
        public const int GameSpeedDefault = 3;

        /// <summary>
        ///     AIの攻撃性の選択肢数
        /// </summary>
        public const int AiAggressiveCount = 5;

        /// <summary>
        ///     難易度の選択肢数
        /// </summary>
        public const int DifficultyCount = 5;

        /// <summary>
        ///     ゲームスピードの選択肢数
        /// </summary>
        public const int GameSpeedCount = 8;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioHeader()
        {
            SelectableCountries = new List<Country>();
            MajorCountries = new List<MajorCountrySettings>();

            IsFreeSelection = true;

            AiAggressive = AiAggressiveDefault;
            Difficulty = DifficultyDefault;
            GameSpeed = GameSpeedDefault;
        }

        #endregion
    }

    /// <summary>
    ///     主要国設定
    /// </summary>
    public class MajorCountrySettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     国名定義
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     国旗の拡張子
        /// </summary>
        public string FlagExt { get; set; }

        /// <summary>
        ///     説明文
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     プロパガンダ画像名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     右端に配置
        /// </summary>
        public bool Bottom { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            NameKey, // 国名定義
            NameString, // 国名文字列
            FlagExt, // 国旗の接尾辞
            DescKey, // 説明文定義
            DescString, // 説明文文字列
            PictureName, // プロパガンダ画像名
            Bottom // 右端に配置
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
        }

        #endregion
    }

    #endregion

    #region シナリオグローバルデータ

    /// <summary>
    ///     シナリオグローバルデータ
    /// </summary>
    public class ScenarioGlobalData
    {
        #region 公開プロパティ

        /// <summary>
        ///     ルール設定
        /// </summary>
        public ScenarioRules Rules { get; set; }

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
        public Alliance Axis { get; set; }

        /// <summary>
        ///     連合国
        /// </summary>
        public Alliance Allies { get; set; }

        /// <summary>
        ///     共産国
        /// </summary>
        public Alliance Comintern { get; set; }

        /// <summary>
        ///     同盟リスト
        /// </summary>
        public List<Alliance> Alliances { get; private set; }

        /// <summary>
        ///     戦争リスト
        /// </summary>
        public List<War> Wars { get; private set; }

        /// <summary>
        ///     不可侵条約リスト
        /// </summary>
        public List<Treaty> NonAggressions { get; private set; }

        /// <summary>
        ///     講和条約リスト
        /// </summary>
        public List<Treaty> Peaces { get; private set; }

        /// <summary>
        ///     貿易リスト
        /// </summary>
        public List<Treaty> Trades { get; private set; }

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

        /// <summary>
        ///     全指揮官を休止
        /// </summary>
        public bool DormantLeadersAll { get; set; }

        /// <summary>
        ///     天候設定
        /// </summary>
        public Weather Weather { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioGlobalData()
        {
            Rules = new ScenarioRules();
            Alliances = new List<Alliance>();
            Wars = new List<War>();
            NonAggressions = new List<Treaty>();
            Peaces = new List<Treaty>();
            Trades = new List<Treaty>();
            DormantLeaders = new List<int>();
            DormantMinisters = new List<int>();
            DormantTeams = new List<int>();
        }

        #endregion
    }

    /// <summary>
    ///     ルール設定
    /// </summary>
    public class ScenarioRules
    {
        #region 公開プロパティ

        /// <summary>
        ///     外交を許可
        /// </summary>
        public bool AllowDiplomacy { get; set; }

        /// <summary>
        ///     生産を許可
        /// </summary>
        public bool AllowProduction { get; set; }

        /// <summary>
        ///     技術開発を許可
        /// </summary>
        public bool AllowTechnology { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ScenarioRules()
        {
            AllowDiplomacy = true;
            AllowProduction = true;
            AllowTechnology = true;
        }

        #endregion
    }

    #endregion

    #region 天候

    /// <summary>
    ///     天候設定
    /// </summary>
    public class Weather
    {
        #region 公開プロパティ

        /// <summary>
        ///     固定設定
        /// </summary>
        public bool Static { get; set; }

        /// <summary>
        ///     天候パターン
        /// </summary>
        public List<WeatherPattern> Patterns { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Weather()
        {
            Patterns = new List<WeatherPattern>();
        }

        #endregion
    }

    /// <summary>
    ///     天候パターン
    /// </summary>
    public class WeatherPattern
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     プロヴィンスリスト
        /// </summary>
        public List<int> Provinces { get; private set; }

        /// <summary>
        ///     中央プロヴィンス
        /// </summary>
        public int Centre { get; set; }

        /// <summary>
        ///     速度
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        ///     方向
        /// </summary>
        public string Heading { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public WeatherPattern()
        {
            Provinces = new List<int>();
        }

        #endregion
    }

    /// <summary>
    ///     天候の種類
    /// </summary>
    public enum WeatherType
    {
        None,
        Clear, // 快晴
        Frozen, // 氷点下
        Raining, // 降雨
        Snowing, // 降雪
        Storm, // 暴風雨
        Blizzard, // 吹雪
        Muddy // 泥濘地
    }

    #endregion

    #region マップ

    /// <summary>
    ///     マップ設定
    /// </summary>
    public class MapSettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     全プロヴィンスが有効かどうか
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        ///     有効プロヴィンス
        /// </summary>
        public List<int> Yes { get; private set; }

        /// <summary>
        ///     無効プロヴィンス
        /// </summary>
        public List<int> No { get; private set; }

        /// <summary>
        ///     マップの範囲(左上)
        /// </summary>
        public MapPoint Top { get; set; }

        /// <summary>
        ///     マップの範囲(右下)
        /// </summary>
        public MapPoint Bottom { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MapSettings()
        {
            All = true;
            Yes = new List<int>();
            No = new List<int>();
        }

        #endregion
    }

    /// <summary>
    ///     マップの座標
    /// </summary>
    public class MapPoint
    {
        #region 公開プロパティ

        /// <summary>
        ///     X座標
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     Y座標
        /// </summary>
        public int Y { get; set; }

        #endregion
    }

    #endregion

    #region プロヴィンス

    /// <summary>
    ///     プロヴィンス設定
    /// </summary>
    public class ProvinceSettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     プロヴィンスID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     プロヴィンス名
        /// </summary>
        public string Name { get; set; }

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
        ///     エネルギーの備蓄量
        /// </summary>
        public double EnergyPool { get; set; }

        /// <summary>
        ///     金属の備蓄量
        /// </summary>
        public double MetalPool { get; set; }

        /// <summary>
        ///     希少資源の備蓄量
        /// </summary>
        public double RareMaterialsPool { get; set; }

        /// <summary>
        ///     エネルギー産出量
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        ///     最大エネルギー産出量
        /// </summary>
        public double MaxEnergy { get; set; }

        /// <summary>
        ///     金属産出量
        /// </summary>
        public double Metal { get; set; }

        /// <summary>
        ///     最大金属産出量
        /// </summary>
        public double MaxMetal { get; set; }

        /// <summary>
        ///     希少資源産出量
        /// </summary>
        public double RareMaterials { get; set; }

        /// <summary>
        ///     最大希少資源産出量
        /// </summary>
        public double MaxRareMaterials { get; set; }

        /// <summary>
        ///     石油産出量
        /// </summary>
        public double Oil { get; set; }

        /// <summary>
        ///     最大石油産出量
        /// </summary>
        public double MaxOil { get; set; }

        /// <summary>
        ///     人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     最大人的資源
        /// </summary>
        public double MaxManpower { get; set; }

        /// <summary>
        ///     勝利ポイント
        /// </summary>
        public int Vp { get; set; }

        /// <summary>
        ///     反乱率
        /// </summary>
        public double RevoltRisk { get; set; }

        /// <summary>
        ///     天候
        /// </summary>
        public WeatherType Weather { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            NameKey, // プロヴィンス名キー
            NameString, // プロヴィンス名文字列
            Ic, // IC
            MaxIc, // 最大IC
            RelativeIc, // 相対IC
            Infrastructure, // インフラ
            MaxInfrastructure, // 最大インフラ
            RelativeInfrastructure, // 相対インフラ
            LandFort, // 陸上要塞
            MaxLandFort, // 最大陸上要塞
            RelativeLandFort, // 相対陸上要塞
            CoastalFort, // 沿岸要塞
            MaxCoastalFort, // 最大沿岸要塞
            RelativeCoastalFort, // 相対沿岸要塞
            AntiAir, // 対空砲
            MaxAntiAir, // 最大対空砲
            RelativeAntiAir, // 相対対空砲
            AirBase, // 空軍基地
            MaxAirBase, // 最大空軍基地
            RelativeAirBase, // 相対空軍基地
            NavalBase, // 海軍基地
            MaxNavalBase, // 最大海軍基地
            RelativeNavalBase, // 相対海軍基地
            RadarStation, // レーダー基地
            MaxRadarStation, // 最大レーダー基地
            RelativeRadarStation, // 相対レーダー基地
            NuclearReactor, // 原子炉
            MaxNuclearReactor, // 最大原子炉
            RelativeNuclearReactor, // 相対原子炉
            RocketTest, // ロケット試験場
            MaxRocketTest, // 最大ロケット試験場
            RelativeRocketTest, // 相対ロケット試験場
            SyntheticOil, // 合成石油工場
            MaxSyntheticOil, // 最大合成石油工場
            RelativeSyntheticOil, // 相対合成石油工場
            SyntheticRares, // 合成素材工場
            MaxSyntheticRares, // 最大合成素材工場
            RelativeSyntheticRares, // 相対合成素材工場
            NuclearPower, // 原子力発電所
            MaxNuclearPower, // 最大原子力発電所
            RelativeNuclearPower, // 相対原子力発電所
            SupplyPool, // 物資備蓄量
            OilPool, // 石油備蓄量
            EnergyPool, // エネルギー備蓄量
            MetalPool, // 金属備蓄量
            RareMaterialsPool, // 希少資源備蓄量
            Energy, // エネルギー産出量
            MaxEnergy, // 最大エネルギー産出量
            Metal, // 金属産出量
            MaxMetal, // 最大金属産出量
            RareMaterials, // 希少資源産出量
            MaxRareMaterials, // 最大希少資源産出量
            Oil, // 石油産出量
            MaxOil, // 最大石油産出量
            Manpower, // 人的資源
            MaxManpower, // 最大人的資源
            Vp, // 勝利ポイント
            RevoltRisk // 反乱率
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     プロヴィンス設定が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    #endregion

    #region 建物

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
    ///     生産中建物情報
    /// </summary>
    public class BuildingDevelopment
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     建物の種類
        /// </summary>
        public BuildingType Type { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     必要IC
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        ///     必要人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     完了予定日
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     進捗率増分
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        ///     総進捗率
        /// </summary>
        public double TotalProgress { get; set; }

        /// <summary>
        ///     連続生産ボーナス
        /// </summary>
        public double GearingBonus { get; set; }

        /// <summary>
        ///     連続生産数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     生産完了数
        /// </summary>
        public int Done { get; set; }

        /// <summary>
        ///     完了日数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        ///     最初の1単位の完了日数
        /// </summary>
        public int DaysForFirst { get; set; }

        /// <summary>
        ///     停止中
        /// </summary>
        public bool Halted { get; set; }

        /// <summary>
        ///     完了時にキューを削除するかどうか
        /// </summary>
        public bool CloseWhenFinished { get; set; }

        /// <summary>
        ///     詳細不明
        /// </summary>
        public bool WaitingForClosure { get; set; }

        #endregion
    }

    /// <summary>
    ///     建物の種類
    /// </summary>
    public enum BuildingType
    {
        None,
        Ic, // 工場
        Infrastructure, // インフラ
        CoastalFort, // 沿岸要塞
        LandFort, // 陸上要塞
        AntiAir, // 対空砲
        AirBase, // 航空基地
        NavalBase, // 海軍基地
        RadarStation, // レーダー基地
        NuclearReactor, // 原子炉
        RocketTest, // ロケット試験場
        SyntheticOil, // 合成石油工場
        SyntheticRares, // 合成素材工場
        NuclearPower // 原子力発電所
    }

    #endregion

    #region 外交

    /// <summary>
    ///     同盟設定
    /// </summary>
    public class Alliance
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

        /// <summary>
        ///     同盟名
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     参加国の編集済みフラグ
        /// </summary>
        private readonly HashSet<Country> _dirtyCountries = new HashSet<Country>();

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Type, // type
            Id, // id
            Name // 同盟名
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Alliance()
        {
            Participant = new List<Country>();
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     対象国が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyCountry(Country country)
        {
            return _dirtyCountries.Contains(country);
        }

        /// <summary>
        ///     参加国の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">対象国</param>
        public void SetDirtyCountry(Country country)
        {
            _dirtyCountries.Add(country);
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }

            _dirtyCountries.Clear();
        }

        #endregion
    }

    /// <summary>
    ///     戦争設定
    /// </summary>
    public class War
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
        public Alliance Attackers { get; set; }

        /// <summary>
        ///     防御側参加国
        /// </summary>
        public Alliance Defenders { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     参加国の編集済みフラグ
        /// </summary>
        private readonly HashSet<Country> _dirtyCountries = new HashSet<Country>();

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Type, // type
            Id, // id
            StartYear, // 開始年
            StartMonth, // 開始月
            StartDay, // 開始日
            EndYear, // 終了年
            EndMonth, // 終了月
            EndDay, // 終了日
            AttackerType, // 攻撃側type
            AttackerId, // 攻撃側id
            DefenderType, // 防御側type
            DefenderId // 防御側id
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     対象国が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyCountry(Country country)
        {
            return _dirtyCountries.Contains(country);
        }

        /// <summary>
        ///     参加国の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">対象国</param>
        public void SetDirtyCountry(Country country)
        {
            _dirtyCountries.Add(country);
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }

            _dirtyCountries.Clear();
        }

        #endregion
    }

    /// <summary>
    ///     外交協定設定
    /// </summary>
    public class Treaty
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
        ///     終了日時
        /// </summary>
        public GameDate EndDate { get; set; }

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

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Type, // type
            Id, // id
            Country1, // 対象国1
            Country2, // 対象国2
            StartYear, // 開始年
            StartMonth, // 開始月
            StartDay, // 開始日
            EndYear, // 終了年
            EndMonth, // 終了月
            EndDay, // 終了日
            Money, // 資金
            Supplies, // 物資
            Energy, // エネルギー
            Metal, // 金属
            RareMaterials, // 希少資源
            Oil, // 石油
            Cancel // 取り消し可能かどうか
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Treaty()
        {
            Cancel = true;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     貿易内容の文字列を取得する
        /// </summary>
        /// <returns>貿易内容の文字列</returns>
        public string GetTradeString()
        {
            StringBuilder sb = new StringBuilder();
            if (!DoubleHelper.IsZero(Energy))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceEnergy), DoubleHelper.ToString1(Energy));
            }
            if (!DoubleHelper.IsZero(Metal))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceMetal), DoubleHelper.ToString1(Metal));
            }
            if (!DoubleHelper.IsZero(RareMaterials))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceRareMaterials),
                    DoubleHelper.ToString1(RareMaterials));
            }
            if (!DoubleHelper.IsZero(Oil))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceOil), DoubleHelper.ToString1(Oil));
            }
            if (!DoubleHelper.IsZero(Supplies))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceSupplies), DoubleHelper.ToString1(Supplies));
            }
            if (!DoubleHelper.IsZero(Money))
            {
                sb.AppendFormat("{0}:{1}, ", Config.GetText(TextId.ResourceMoney), DoubleHelper.ToString1(Money));
            }
            int len = sb.Length;
            return (len > 0) ? sb.ToString(0, len - 2) : "";
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     外交協定設定が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     国家関係設定
    /// </summary>
    public class Relation
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
        ///     通行許可
        /// </summary>
        public bool Access { get; set; }

        /// <summary>
        ///     独立保障期限
        /// </summary>
        public GameDate Guaranteed { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Value, // 関係値
            Access, // 通行許可
            Guaranteed, // 独立保証
            GuaranteedYear, // 独立保障期限年
            GuaranteedMonth, // 独立保障期限月
            GuaranteedDay // 独立保障期限日
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
        }

        #endregion
    }

    /// <summary>
    ///     外交協定の種類
    /// </summary>
    public enum TreatyType
    {
        NonAggression, // 不可侵条約
        Peace, // 休戦協定
        Trade // 貿易
    }

    #endregion

    #region 国家

    /// <summary>
    ///     国家設定
    /// </summary>
    public class CountrySettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     ファイル名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     国名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     国旗の接尾辞
        /// </summary>
        public string FlagExt { get; set; }

        /// <summary>
        ///     兄弟国
        /// </summary>
        public Country RegularId { get; set; }

        /// <summary>
        ///     独立可能政体
        /// </summary>
        public GovernmentType IntrinsicGovType { get; set; }

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
        ///     追加輸送能力
        /// </summary>
        public double ExtraTc { get; set; }

        /// <summary>
        ///     国民不満度
        /// </summary>
        public double Dissent { get; set; }

        /// <summary>
        ///     首都
        /// </summary>
        public int Capital { get; set; }

        /// <summary>
        ///     平時IC補正
        /// </summary>
        public double PeacetimeIcModifier { get; set; }

        /// <summary>
        ///     戦時IC補正
        /// </summary>
        public double WartimeIcModifier { get; set; }

        /// <summary>
        ///     工業力補正
        /// </summary>
        public double IndustrialModifier { get; set; }

        /// <summary>
        ///     対地防御補正
        /// </summary>
        public double GroundDefEff { get; set; }

        /// <summary>
        ///     AIファイル名
        /// </summary>
        public string AiFileName { get; set; }

        /// <summary>
        ///     AI設定
        /// </summary>
        public AiSettings AiSettings { get; set; }

        /// <summary>
        ///     人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     人的資源補正値
        /// </summary>
        public double RelativeManpower { get; set; }

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
        ///     核兵器
        /// </summary>
        public int Nuke { get; set; }

        /// <summary>
        ///     マップ外資源
        /// </summary>
        public ResourceSettings Offmap { get; set; }

        /// <summary>
        ///     消費財IC比率
        /// </summary>
        public double ConsumerSlider { get; set; }

        /// <summary>
        ///     物資IC比率
        /// </summary>
        public double SupplySlider { get; set; }

        /// <summary>
        ///     生産IC比率
        /// </summary>
        public double ProductionSlider { get; set; }

        /// <summary>
        ///     補充IC比率
        /// </summary>
        public double ReinforcementSlider { get; set; }

        /// <summary>
        ///     外交関係
        /// </summary>
        public List<Relation> Relations { get; private set; }

        /// <summary>
        ///     諜報情報
        /// </summary>
        public List<SpySettings> Intelligence { get; private set; }

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
        ///     領有権主張プロヴィンス
        /// </summary>
        public List<int> ClaimedProvinces { get; private set; }

        /// <summary>
        ///     保有技術
        /// </summary>
        public List<int> TechApps { get; private set; }

        /// <summary>
        ///     青写真
        /// </summary>
        public List<int> BluePrints { get; private set; }

        /// <summary>
        ///     発明イベント
        /// </summary>
        public List<int> Inventions { get; private set; }

        /// <summary>
        ///     政策スライダー
        /// </summary>
        public CountryPolicy Policy { get; set; }

        /// <summary>
        ///     核兵器完成日時
        /// </summary>
        public GameDate NukeDate { get; set; }

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
        ///     国民の意識
        /// </summary>
        public string NationalIdentity { get; set; }

        /// <summary>
        ///     社会政策
        /// </summary>
        public string SocialPolicy { get; set; }

        /// <summary>
        ///     国家の文化
        /// </summary>
        public string NationalCulture { get; set; }

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

        /// <summary>
        ///     抽出指揮官
        /// </summary>
        public List<int> StealLeaders { get; private set; }

        /// <summary>
        ///     輸送船団
        /// </summary>
        public List<Convoy> Convoys { get; private set; }

        /// <summary>
        ///     陸軍ユニット
        /// </summary>
        public List<LandUnit> LandUnits { get; private set; }

        /// <summary>
        ///     海軍ユニット
        /// </summary>
        public List<NavalUnit> NavalUnits { get; private set; }

        /// <summary>
        ///     空軍ユニット
        /// </summary>
        public List<AirUnit> AirUnits { get; private set; }

        /// <summary>
        ///     生産中師団
        /// </summary>
        public List<DivisionDevelopment> DivisionDevelopments { get; private set; }

        /// <summary>
        ///     生産中輸送船団
        /// </summary>
        public List<ConvoyDevelopment> ConvoyDevelopments { get; private set; }

        /// <summary>
        ///     生産中建物
        /// </summary>
        public List<BuildingDevelopment> BuildingDevelopments { get; private set; }

        /// <summary>
        ///     陸軍師団
        /// </summary>
        public List<LandDivision> LandDivisions { get; private set; }

        /// <summary>
        ///     海軍師団
        /// </summary>
        public List<NavalDivision> NavalDivisions { get; private set; }

        /// <summary>
        ///     空軍師団
        /// </summary>
        public List<AirDivision> AirDivisions { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        /// <summary>
        ///     保有技術の編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyOwnedTechs = new HashSet<int>();

        /// <summary>
        ///     青写真の編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyBlueprints = new HashSet<int>();

        /// <summary>
        ///     発明イベントの編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyInventions = new HashSet<int>();

        /// <summary>
        ///     中核プロヴィンスの編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyCoreProvinces = new HashSet<int>();

        /// <summary>
        ///     保有プロヴィンスの編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyOwnedProvinces = new HashSet<int>();

        /// <summary>
        ///     支配プロヴィンスの編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyControlledProvinces = new HashSet<int>();

        /// <summary>
        ///     領有権主張プロヴィンスの編集済みフラグ
        /// </summary>
        private readonly HashSet<int> _dirtyClaimedProvinces = new HashSet<int>();

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            NameKey, // 国名定義
            NameString, // 国名文字列
            FlagExt, // 国旗の接尾辞
            RegularId, // 兄弟国
            IntrinsicGovType, // 独立可能政体
            Master, // 宗主国
            Control, // 統帥権取得国
            Belligerence, // 好戦性
            ExtraTc, // 追加輸送能力
            Dissent, // 国民不満度
            Capital, // 首都
            PeacetimeIcModifier, // 平時IC補正
            WartimeIcModifier, // 戦時IC補正
            IndustrialModifier, // 工業力補正
            GroundDefEff, // 対地防御補正
            AiFileName, // AIファイル名
            Manpower, // 人的資源
            RelativeManpower, // 人的資源補正値
            Energy, // エネルギー
            Metal, // 金属
            RareMaterials, // 希少資源
            Oil, // 石油
            Supplies, // 物資
            Money, // 資金
            Transports, // 輸送船団
            Escorts, // 護衛艦
            OffmapIc, // マップ外工業力
            OffmapManpower, // マップ外人的資源
            OffmapEnergy, // マップ外エネルギー
            OffmapMetal, // マップ外金属
            OffmapRareMaterials, // マップ外希少資源
            OffmapOil, // マップ外石油
            OffmapSupplies, // マップ外物資
            OffmapMoney, // マップ外資金
            OffmapTransports, // マップ外輸送船団
            OffmapEscorts, // マップ外護衛艦
            ConsumerSlider, // 消費財IC比率
            SupplySlider, // 物資IC比率
            ProductionSlider, // 生産IC比率
            ReinforcementSlider, // 補充IC比率
            SliderYear, // スライダー移動可能年
            SliderMonth, // スライダー移動可能月
            SliderDay, // スライダー移動可能日
            Democratic, // 民主的 - 独裁的
            PoliticalLeft, // 政治的左派 - 政治的右派
            Freedom, // 開放社会 - 閉鎖社会
            FreeMarket, // 自由経済 - 中央計画経済
            ProfessionalArmy, // 常備軍 - 徴兵軍
            DefenseLobby, // タカ派 - ハト派
            Interventionism, // 介入主義 - 孤立主義
            Nuke, // 核兵器
            NukeYear, // 核兵器生産年
            NukeMonth, // 核兵器生産月
            NukeDay, // 核兵器生産日
            HeadOfStateType, // 国家元首のtype
            HeadOfGovernmentType, // 政府首班のtype
            ForeignMinisterType, // 外務大臣のtype
            ArmamentMinisterType, // 軍需大臣のtype
            MinisterOfSecurityType, // 内務大臣のtype
            MinisterOfIntelligenceType, // 情報大臣のtype
            ChiefOfStaffType, // 統合参謀総長のtype
            ChiefOfArmyType, // 陸軍総司令官のtype
            ChiefOfNavyType, // 海軍総司令官のtype
            ChiefOfAirType, // 空軍総司令官のtype
            HeadOfStateId, // 国家元首のid
            HeadOfGovernmentId, // 政府首班のid
            ForeignMinisterId, // 外務大臣のid
            ArmamentMinisterId, // 軍需大臣のid
            MinisterOfSecurityId, // 内務大臣のid
            MinisterOfIntelligenceId, // 情報大臣のid
            ChiefOfStaffId, // 統合参謀総長のid
            ChiefOfArmyId, // 陸軍総司令官のid
            ChiefOfNavyId, // 海軍総司令官のid
            ChiefOfAirId // 空軍総司令官のid
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public CountrySettings()
        {
            Relations = new List<Relation>();
            Intelligence = new List<SpySettings>();
            NationalProvinces = new List<int>();
            OwnedProvinces = new List<int>();
            ControlledProvinces = new List<int>();
            ClaimedProvinces = new List<int>();
            TechApps = new List<int>();
            BluePrints = new List<int>();
            Inventions = new List<int>();
            DormantLeaders = new List<int>();
            DormantMinisters = new List<int>();
            DormantTeams = new List<int>();
            StealLeaders = new List<int>();
            Convoys = new List<Convoy>();
            LandUnits = new List<LandUnit>();
            NavalUnits = new List<NavalUnit>();
            AirUnits = new List<AirUnit>();
            DivisionDevelopments = new List<DivisionDevelopment>();
            ConvoyDevelopments = new List<ConvoyDevelopment>();
            BuildingDevelopments = new List<BuildingDevelopment>();
            LandDivisions = new List<LandDivision>();
            NavalDivisions = new List<NavalDivision>();
            AirDivisions = new List<AirDivision>();
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     国家設定が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の保有技術が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">技術ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyOwnedTech(int id)
        {
            return _dirtyOwnedTechs.Contains(id);
        }

        /// <summary>
        ///     保有技術の編集済みフラグを設定する
        /// </summary>
        /// <param name="id">技術ID</param>
        public void SetDirtyOwnedTech(int id)
        {
            _dirtyOwnedTechs.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の青写真が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">技術ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyBlueprint(int id)
        {
            return _dirtyBlueprints.Contains(id);
        }

        /// <summary>
        ///     青写真の編集済みフラグを設定する
        /// </summary>
        /// <param name="id">技術ID</param>
        public void SetDirtyBlueprint(int id)
        {
            _dirtyBlueprints.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の発明イベントが編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">イベントID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyInvention(int id)
        {
            return _dirtyInventions.Contains(id);
        }

        /// <summary>
        ///     発明イベントの編集済みフラグを設定する
        /// </summary>
        /// <param name="id">イベントID</param>
        public void SetDirtyInvention(int id)
        {
            _dirtyInventions.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の中核プロヴィンスが編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyCoreProvinces(int id)
        {
            return _dirtyCoreProvinces.Contains(id);
        }

        /// <summary>
        ///     中核プロヴィンスの編集済みフラグを設定する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        public void SetDirtyCoreProvinces(int id)
        {
            _dirtyCoreProvinces.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の保有プロヴィンスが編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyOwnedProvinces(int id)
        {
            return _dirtyOwnedProvinces.Contains(id);
        }

        /// <summary>
        ///     保有プロヴィンスの編集済みフラグを設定する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        public void SetDirtyOwnedProvinces(int id)
        {
            _dirtyOwnedProvinces.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の支配プロヴィンスが編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyControlledProvinces(int id)
        {
            return _dirtyControlledProvinces.Contains(id);
        }

        /// <summary>
        ///     支配プロヴィンスの編集済みフラグを設定する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        public void SetDirtyControlledProvinces(int id)
        {
            _dirtyControlledProvinces.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     対象の領有権主張プロヴィンスが編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyClaimedProvinces(int id)
        {
            return _dirtyClaimedProvinces.Contains(id);
        }

        /// <summary>
        ///     領有権主張プロヴィンスの編集済みフラグを設定する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        public void SetDirtyClaimedProvinces(int id)
        {
            _dirtyClaimedProvinces.Add(id);
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;

            _dirtyOwnedTechs.Clear();
            _dirtyBlueprints.Clear();
            _dirtyInventions.Clear();
            _dirtyCoreProvinces.Clear();
            _dirtyOwnedProvinces.Clear();
            _dirtyControlledProvinces.Clear();
            _dirtyClaimedProvinces.Clear();

            if (Relations != null)
            {
                foreach (Relation relation in Relations)
                {
                    relation.ResetDirtyAll();
                }
            }

            if (Intelligence != null)
            {
                foreach (SpySettings spy in Intelligence)
                {
                    spy.ResetDirtyAll();
                }
            }
        }

        #endregion
    }

    /// <summary>
    ///     AI設定
    /// </summary>
    public class AiSettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     フラグ
        /// </summary>
        public Dictionary<string, string> Flags { get; set; }

        #endregion
    }

    /// <summary>
    ///     資源設定
    /// </summary>
    public class ResourceSettings
    {
        #region 公開プロパティ

        /// <summary>
        ///     工業力
        /// </summary>
        public double Ic { get; set; }

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

        #endregion
    }

    /// <summary>
    ///     諜報設定
    /// </summary>
    public class SpySettings
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

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ItemId)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     項目ID
        /// </summary>
        public enum ItemId
        {
            Spies // スパイの数
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (ItemId id in Enum.GetValues(typeof (ItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
        }

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

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public CountryPolicy()
        {
            Democratic = 5;
            PoliticalLeft = 5;
            Freedom = 5;
            FreeMarket = 5;
            ProfessionalArmy = 5;
            DefenseLobby = 5;
            Interventionism = 5;
        }

        #endregion
    }

    /// <summary>
    ///     政体
    /// </summary>
    public enum GovernmentType
    {
        None,
        Nazi, // 国家社会主義
        Fascist, // ファシズム
        PaternalAutocrat, // 専制独裁
        SocialConservative, // 社会保守派
        MarketLiberal, // 自由経済派
        SocialLiberal, // 社会自由派
        SocialDemocrat, // 社会民主派
        LeftWingRadical, // 急進的左翼
        Leninist, // レーニン主義
        Stalinist // スターリン主義
    }

    #endregion

    #region ユニット

    /// <summary>
    ///     陸軍ユニット
    /// </summary>
    public class LandUnit
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
        ///     統帥国
        /// </summary>
        public Country Control { get; set; }

        /// <summary>
        ///     指揮官
        /// </summary>
        public int Leader { get; set; }

        /// <summary>
        ///     現在位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     直前の位置
        /// </summary>
        public int PrevProv { get; set; }

        /// <summary>
        ///     基準位置
        /// </summary>
        public int Home { get; set; }

        /// <summary>
        ///     塹壕レベル
        /// </summary>
        public double DigIn { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public LandMission Mission { get; set; }

        /// <summary>
        ///     指定日時
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     development (詳細不明)
        /// </summary>
        public bool Development { get; set; }

        /// <summary>
        ///     移動完了日時
        /// </summary>
        public GameDate MoveTime { get; set; }

        /// <summary>
        ///     移動経路
        /// </summary>
        public List<int> Movement { get; private set; }

        /// <summary>
        ///     攻撃日時
        /// </summary>
        public GameDate AttackDate { get; set; }

        /// <summary>
        ///     上陸中
        /// </summary>
        public bool Invasion { get; set; }

        /// <summary>
        ///     上陸先
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        ///     優先
        /// </summary>
        public bool Prioritized { get; set; }

        /// <summary>
        ///     改良可能
        /// </summary>
        public bool CanUpgrade { get; set; }

        /// <summary>
        ///     補充可能
        /// </summary>
        public bool CanReinforcement { get; set; }

        /// <summary>
        ///     構成師団
        /// </summary>
        public List<LandDivision> Divisions { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public LandUnit()
        {
            Movement = new List<int>();
            Divisions = new List<LandDivision>();
        }

        #endregion
    }

    /// <summary>
    ///     海軍ユニット
    /// </summary>
    public class NavalUnit
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
        ///     統帥国
        /// </summary>
        public Country Control { get; set; }

        /// <summary>
        ///     指揮官
        /// </summary>
        public int Leader { get; set; }

        /// <summary>
        ///     現在位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     直前の位置
        /// </summary>
        public int PrevProv { get; set; }

        /// <summary>
        ///     基準位置
        /// </summary>
        public int Home { get; set; }

        /// <summary>
        ///     所属基地
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public NavalMission Mission { get; set; }

        /// <summary>
        ///     指定日時
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     development (詳細不明)
        /// </summary>
        public bool Development { get; set; }

        /// <summary>
        ///     移動完了日時
        /// </summary>
        public GameDate MoveTime { get; set; }

        /// <summary>
        ///     移動経路
        /// </summary>
        public List<int> Movement { get; private set; }

        /// <summary>
        ///     攻撃日時
        /// </summary>
        public GameDate AttackDate { get; set; }

        /// <summary>
        ///     優先
        /// </summary>
        public bool Prioritized { get; set; }

        /// <summary>
        ///     改良可能
        /// </summary>
        public bool CanUpgrade { get; set; }

        /// <summary>
        ///     補充可能
        /// </summary>
        public bool CanReinforcement { get; set; }

        /// <summary>
        ///     構成師団
        /// </summary>
        public List<NavalDivision> Divisions { get; private set; }

        /// <summary>
        ///     搭載ユニット
        /// </summary>
        public List<LandUnit> LandUnits { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public NavalUnit()
        {
            Movement = new List<int>();
            Divisions = new List<NavalDivision>();
            LandUnits = new List<LandUnit>();
        }

        #endregion
    }

    /// <summary>
    ///     空軍ユニット
    /// </summary>
    public class AirUnit
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
        ///     統帥国
        /// </summary>
        public Country Control { get; set; }

        /// <summary>
        ///     指揮官
        /// </summary>
        public int Leader { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     直前の位置
        /// </summary>
        public int PrevProv { get; set; }

        /// <summary>
        ///     基準位置
        /// </summary>
        public int Home { get; set; }

        /// <summary>
        ///     所属基地
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public AirMission Mission { get; set; }

        /// <summary>
        ///     指定日時
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     development (詳細不明)
        /// </summary>
        public bool Development { get; set; }

        /// <summary>
        ///     移動完了日時
        /// </summary>
        public GameDate MoveTime { get; set; }

        /// <summary>
        ///     移動経路
        /// </summary>
        public List<int> Movement { get; private set; }

        /// <summary>
        ///     攻撃日時
        /// </summary>
        public GameDate AttackDate { get; set; }

        /// <summary>
        ///     優先
        /// </summary>
        public bool Prioritized { get; set; }

        /// <summary>
        ///     改良可能
        /// </summary>
        public bool CanUpgrade { get; set; }

        /// <summary>
        ///     補充可能
        /// </summary>
        public bool CanReinforcement { get; set; }

        /// <summary>
        ///     構成師団
        /// </summary>
        public List<AirDivision> Divisions { get; private set; }

        /// <summary>
        ///     搭載ユニット
        /// </summary>
        public List<LandUnit> LandUnits { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public AirUnit()
        {
            Movement = new List<int>();
            Divisions = new List<AirDivision>();
            LandUnits = new List<LandUnit>();
        }

        #endregion
    }

    #endregion

    #region 師団

    /// <summary>
    ///     陸軍師団
    /// </summary>
    public class LandDivision
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
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra1 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra2 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra3 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra4 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra5 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel1 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel2 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel3 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel4 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel5 { get; set; }

        /// <summary>
        ///     最大戦力
        /// </summary>
        public double MaxStrength { get; set; }

        /// <summary>
        ///     戦力
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        ///     最大組織率
        /// </summary>
        public double MaxOrganisation { get; set; }

        /// <summary>
        ///     組織率
        /// </summary>
        public double Organisation { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public double Experience { get; set; }

        /// <summary>
        ///     改良進捗率
        /// </summary>
        public double UpgradeProgress { get; set; }

        /// <summary>
        ///     再配置先プロヴィンス
        /// </summary>
        public int RedeployTarget { get; set; }

        /// <summary>
        ///     再配置先ユニット名
        /// </summary>
        public string RedeployUnitName { get; set; }

        /// <summary>
        ///     再配置先ユニットID
        /// </summary>
        public TypeId RedeployUnitId { get; set; }

        /// <summary>
        ///     攻勢開始日時
        /// </summary>
        public GameDate Offensive { get; set; }

        /// <summary>
        ///     物資
        /// </summary>
        public double Supplies { get; set; }

        /// <summary>
        ///     燃料
        /// </summary>
        public double Fuel { get; set; }

        /// <summary>
        ///     最大物資
        /// </summary>
        public double MaxSupplies { get; set; }

        /// <summary>
        ///     最大燃料
        /// </summary>
        public double MaxFuel { get; set; }

        /// <summary>
        ///     物資消費量
        /// </summary>
        public double SupplyConsumption { get; set; }

        /// <summary>
        ///     燃料消費量
        /// </summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        ///     最大速度
        /// </summary>
        public double MaxSpeed { get; set; }

        /// <summary>
        ///     砲兵速度キャップ
        /// </summary>
        public double SpeedCapArt { get; set; }

        /// <summary>
        ///     工兵速度キャップ
        /// </summary>
        public double SpeedCapEng { get; set; }

        /// <summary>
        ///     対空速度キャップ
        /// </summary>
        public double SpeedCapAa { get; set; }

        /// <summary>
        ///     対戦車速度キャップ
        /// </summary>
        public double SpeedCapAt { get; set; }

        /// <summary>
        ///     輸送負荷
        /// </summary>
        public double TransportWeight { get; set; }

        /// <summary>
        ///     防御力
        /// </summary>
        public double Defensiveness { get; set; }

        /// <summary>
        ///     耐久力
        /// </summary>
        public double Toughness { get; set; }

        /// <summary>
        ///     脆弱性
        /// </summary>
        public double Softness { get; set; }

        /// <summary>
        ///     制圧力
        /// </summary>
        public double Suppression { get; set; }

        /// <summary>
        ///     対空防御力
        /// </summary>
        public double AirDefence { get; set; }

        /// <summary>
        ///     対人攻撃力
        /// </summary>
        public double SoftAttack { get; set; }

        /// <summary>
        ///     対甲攻撃力
        /// </summary>
        public double HardAttack { get; set; }

        /// <summary>
        ///     対空攻撃力
        /// </summary>
        public double AirAttack { get; set; }

        /// <summary>
        ///     砲撃能力
        /// </summary>
        public double ArtilleryBombardment { get; set; }

        /// <summary>
        ///     休止状態
        /// </summary>
        public bool Dormant { get; set; }

        /// <summary>
        ///     移動不可
        /// </summary>
        public bool Locked { get; set; }

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
        public LandDivision()
        {
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
            BrigadeModel1 = UndefinedModelNo;
            BrigadeModel2 = UndefinedModelNo;
            BrigadeModel3 = UndefinedModelNo;
            BrigadeModel4 = UndefinedModelNo;
            BrigadeModel5 = UndefinedModelNo;
        }

        #endregion
    }

    /// <summary>
    ///     海軍師団
    /// </summary>
    public class NavalDivision
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
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     核兵器搭載
        /// </summary>
        public bool Nuke { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra1 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra2 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra3 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra4 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra5 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel1 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel2 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel3 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel4 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel5 { get; set; }

        /// <summary>
        ///     最大戦力
        /// </summary>
        public double MaxStrength { get; set; }

        /// <summary>
        ///     戦力
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        ///     最大組織率
        /// </summary>
        public double MaxOrganisation { get; set; }

        /// <summary>
        ///     組織率
        /// </summary>
        public double Organisation { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public double Experience { get; set; }

        /// <summary>
        ///     改良進捗率
        /// </summary>
        public double UpgradeProgress { get; set; }

        /// <summary>
        ///     物資
        /// </summary>
        public double Supplies { get; set; }

        /// <summary>
        ///     燃料
        /// </summary>
        public double Fuel { get; set; }

        /// <summary>
        ///     最大物資
        /// </summary>
        public double MaxSupplies { get; set; }

        /// <summary>
        ///     最大燃料
        /// </summary>
        public double MaxFuel { get; set; }

        /// <summary>
        ///     物資消費量
        /// </summary>
        public double SupplyConsumption { get; set; }

        /// <summary>
        ///     燃料消費量
        /// </summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        ///     最大速度
        /// </summary>
        public double MaxSpeed { get; set; }

        /// <summary>
        ///     輸送能力
        /// </summary>
        public double TransportCapability { get; set; }

        /// <summary>
        ///     対艦/対潜防御力
        /// </summary>
        public double SeaDefense { get; set; }

        /// <summary>
        ///     対空防御力
        /// </summary>
        public double AirDefence { get; set; }

        /// <summary>
        ///     対艦攻撃力(海軍)
        /// </summary>
        public double SeaAttack { get; set; }

        /// <summary>
        ///     対潜攻撃力
        /// </summary>
        public double SubAttack { get; set; }

        /// <summary>
        ///     通商破壊力
        /// </summary>
        public double ConvoyAttack { get; set; }

        /// <summary>
        ///     湾岸攻撃力
        /// </summary>
        public double ShoreBombardment { get; set; }

        /// <summary>
        ///     対空攻撃力
        /// </summary>
        public double AirAttack { get; set; }

        /// <summary>
        ///     対艦索敵能力
        /// </summary>
        public double SurfaceDetection { get; set; }

        /// <summary>
        ///     対空索敵能力
        /// </summary>
        public double AirDetection { get; set; }

        /// <summary>
        ///     対潜索敵能力
        /// </summary>
        public double SubDetection { get; set; }

        /// <summary>
        ///     可視性
        /// </summary>
        public double Visibility { get; set; }

        /// <summary>
        ///     航続距離
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        ///     射程距離
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        ///     移動距離
        /// </summary>
        public double Travelled { get; set; }

        /// <summary>
        ///     休止状態
        /// </summary>
        public bool Dormant { get; set; }

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
        public NavalDivision()
        {
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
            BrigadeModel1 = UndefinedModelNo;
            BrigadeModel2 = UndefinedModelNo;
            BrigadeModel3 = UndefinedModelNo;
            BrigadeModel4 = UndefinedModelNo;
            BrigadeModel5 = UndefinedModelNo;
        }

        #endregion
    }

    /// <summary>
    ///     空軍師団
    /// </summary>
    public class AirDivision
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
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     核兵器搭載
        /// </summary>
        public bool Nuke { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra1 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra2 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra3 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra4 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra5 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel1 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel2 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel3 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel4 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel5 { get; set; }

        /// <summary>
        ///     最大戦力
        /// </summary>
        public double MaxStrength { get; set; }

        /// <summary>
        ///     戦力
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        ///     最大組織率
        /// </summary>
        public double MaxOrganisation { get; set; }

        /// <summary>
        ///     組織率
        /// </summary>
        public double Organisation { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public double Experience { get; set; }

        /// <summary>
        ///     改良進捗率
        /// </summary>
        public double UpgradeProgress { get; set; }

        /// <summary>
        ///     物資
        /// </summary>
        public double Supplies { get; set; }

        /// <summary>
        ///     燃料
        /// </summary>
        public double Fuel { get; set; }

        /// <summary>
        ///     最大物資
        /// </summary>
        public double MaxSupplies { get; set; }

        /// <summary>
        ///     最大燃料
        /// </summary>
        public double MaxFuel { get; set; }

        /// <summary>
        ///     物資消費量
        /// </summary>
        public double SupplyConsumption { get; set; }

        /// <summary>
        ///     燃料消費量
        /// </summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        ///     最大速度
        /// </summary>
        public double MaxSpeed { get; set; }

        /// <summary>
        ///     輸送能力
        /// </summary>
        public double TransportCapability { get; set; }

        /// <summary>
        ///     対地防御力
        /// </summary>
        public double SurfaceDefence { get; set; }

        /// <summary>
        ///     対空防御力
        /// </summary>
        public double AirDefence { get; set; }

        /// <summary>
        ///     対人攻撃力
        /// </summary>
        public double SoftAttack { get; set; }

        /// <summary>
        ///     対甲攻撃力
        /// </summary>
        public double HardAttack { get; set; }

        /// <summary>
        ///     対空攻撃力
        /// </summary>
        public double AirAttack { get; set; }

        /// <summary>
        ///     戦略爆撃攻撃力
        /// </summary>
        public double StrategicAttack { get; set; }

        /// <summary>
        ///     対艦攻撃力
        /// </summary>
        public double NavalAttack { get; set; }

        /// <summary>
        ///     対艦索敵能力
        /// </summary>
        public double SurfaceDetection { get; set; }

        /// <summary>
        ///     対空索敵能力
        /// </summary>
        public double AirDetection { get; set; }

        /// <summary>
        ///     航続距離
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        ///     休止状態
        /// </summary>
        public bool Dormant { get; set; }

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
        public AirDivision()
        {
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
            BrigadeModel1 = UndefinedModelNo;
            BrigadeModel2 = UndefinedModelNo;
            BrigadeModel3 = UndefinedModelNo;
            BrigadeModel4 = UndefinedModelNo;
            BrigadeModel5 = UndefinedModelNo;
        }

        #endregion
    }

    /// <summary>
    ///     生産中師団
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
        ///     必要人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     unitcost (詳細不明)
        /// </summary>
        public bool UnitCost { get; set; }

        /// <summary>
        ///     new_model (詳細不明)
        /// </summary>
        public bool NewModel { get; set; }

        /// <summary>
        ///     完了予定日
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     進捗率増分
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        ///     総進捗率
        /// </summary>
        public double TotalProgress { get; set; }

        /// <summary>
        ///     連続生産ボーナス
        /// </summary>
        public double GearingBonus { get; set; }

        /// <summary>
        ///     総生産数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     生産完了数
        /// </summary>
        public int Done { get; set; }

        /// <summary>
        ///     完了日数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        ///     1単位の完了日数
        /// </summary>
        public int DaysForFirst { get; set; }

        /// <summary>
        ///     停止中
        /// </summary>
        public bool Halted { get; set; }

        /// <summary>
        ///     完了時にキューを削除するかどうか
        /// </summary>
        public bool CloseWhenFinished { get; set; }

        /// <summary>
        ///     waitingforclosure (詳細不明)
        /// </summary>
        public bool WaitingForClosure { get; set; }

        /// <summary>
        ///     ユニット種類
        /// </summary>
        public UnitType Type { get; set; }

        /// <summary>
        ///     モデル番号
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra1 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra2 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra3 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra4 { get; set; }

        /// <summary>
        ///     付属旅団のユニット種類
        /// </summary>
        public UnitType Extra5 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel1 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel2 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel3 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel4 { get; set; }

        /// <summary>
        ///     付属旅団のモデル番号
        /// </summary>
        public int BrigadeModel5 { get; set; }

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
            UnitCost = true;
            NewModel = true;
            Model = UndefinedModelNo;
            BrigadeModel = UndefinedModelNo;
            BrigadeModel1 = UndefinedModelNo;
            BrigadeModel2 = UndefinedModelNo;
            BrigadeModel3 = UndefinedModelNo;
            BrigadeModel4 = UndefinedModelNo;
            BrigadeModel5 = UndefinedModelNo;
        }

        #endregion
    }

    #endregion

    #region 任務

    /// <summary>
    ///     陸軍任務
    /// </summary>
    public class LandMission
    {
        #region 公開プロパティ

        /// <summary>
        ///     任務の種類
        /// </summary>
        public LandMissionType Type { get; set; }

        /// <summary>
        ///     対象プロヴィンス
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        ///     戦力/指揮統制率下限
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        ///     夜間遂行
        /// </summary>
        public bool Night { get; set; }

        /// <summary>
        ///     昼間遂行
        /// </summary>
        public bool Day { get; set; }

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public int Task { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        #endregion
    }

    /// <summary>
    ///     海軍任務
    /// </summary>
    public class NavalMission
    {
        #region 公開プロパティ

        /// <summary>
        ///     任務の種類
        /// </summary>
        public NavalMissionType Type { get; set; }

        /// <summary>
        ///     対象プロヴィンス
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        ///     戦力/指揮統制率下限
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        ///     夜間遂行
        /// </summary>
        public bool Night { get; set; }

        /// <summary>
        ///     昼間遂行
        /// </summary>
        public bool Day { get; set; }

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     終了日時
        /// </summary>
        public GameDate EndDate { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public int Task { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        #endregion
    }

    /// <summary>
    ///     空軍任務
    /// </summary>
    public class AirMission
    {
        #region 公開プロパティ

        /// <summary>
        ///     任務の種類
        /// </summary>
        public AirMissionType Type { get; set; }

        /// <summary>
        ///     対象プロヴィンス
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        ///     戦力/指揮統制率下限
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        ///     夜間遂行
        /// </summary>
        public bool Night { get; set; }

        /// <summary>
        ///     昼間遂行
        /// </summary>
        public bool Day { get; set; }

        /// <summary>
        ///     開始日時
        /// </summary>
        public GameDate StartDate { get; set; }

        /// <summary>
        ///     終了日時
        /// </summary>
        public GameDate EndDate { get; set; }

        /// <summary>
        ///     任務
        /// </summary>
        public int Task { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        #endregion
    }

    /// <summary>
    ///     陸軍任務の種類
    /// </summary>
    public enum LandMissionType
    {
        None,
        Attack, // 攻撃
        StratRedeploy, // 戦略的再配備
        SupportAttack, // 支援攻撃
        SupportDefense, // 防戦支援
        Reserves, // 待機
        AntiPartisanDuty // パルチザン掃討
    }

    /// <summary>
    ///     海軍任務の種類
    /// </summary>
    public enum NavalMissionType
    {
        None,
        ConvoyRaiding, // 船団襲撃
        Asw, // 対潜作戦
        NavalInterdiction, // 海上阻止
        ShoreBombardment, // 沿岸砲撃
        AmphibiousAssault, // 強襲上陸
        SeaTransport, // 海上輸送
        NavalCombatPatrol, // 海上戦闘哨戒
        NavalPortStrike, // 空母による港湾攻撃
        NavalAirbaseStrike // 空母による航空基地攻撃
    }

    /// <summary>
    ///     空軍任務の種類
    /// </summary>
    public enum AirMissionType
    {
        None,
        AirSuperiority, // 制空権
        GroundAttack, // 地上攻撃
        RunwayCratering, // 空港空爆
        InstallationStrike, // 軍事施設攻撃
        Interdiction, // 地上支援
        NavalStrike, // 艦船攻撃
        PortStrike, // 港湾攻撃
        LogisticalStrike, // 兵站攻撃
        StrategicBombardment, // 戦略爆撃
        AirSupply, // 空輸補給
        AirborneAssault, // 空挺強襲
        ConvoyAirRaiding, // 船団爆撃
        Nuke // 核攻撃
    }

    #endregion

    #region 輸送船団

    /// <summary>
    ///     輸送船団
    /// </summary>
    public class Convoy
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     貿易ID
        /// </summary>
        public TypeId TradeId { get; set; }

        /// <summary>
        ///     貿易用の輸送船団かどうか
        /// </summary>
        public bool IsTrade { get; set; }

        /// <summary>
        ///     輸送船の数
        /// </summary>
        public int Transports { get; set; }

        /// <summary>
        ///     護衛艦の数
        /// </summary>
        public int Escorts { get; set; }

        /// <summary>
        ///     エネルギーの輸送有無
        /// </summary>
        public bool Energy { get; set; }

        /// <summary>
        ///     金属の輸送有無
        /// </summary>
        public bool Metal { get; set; }

        /// <summary>
        ///     希少資源の輸送有無
        /// </summary>
        public bool RareMaterials { get; set; }

        /// <summary>
        ///     石油の輸送有無
        /// </summary>
        public bool Oil { get; set; }

        /// <summary>
        ///     物資の輸送有無
        /// </summary>
        public bool Supplies { get; set; }

        /// <summary>
        ///     航路
        /// </summary>
        public List<int> Path { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Convoy()
        {
            Path = new List<int>();
        }

        #endregion
    }

    /// <summary>
    ///     生産中輸送船団
    /// </summary>
    public class ConvoyDevelopment
    {
        #region 公開プロパティ

        /// <summary>
        ///     typeとidの組
        /// </summary>
        public TypeId Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     輸送船団の種類
        /// </summary>
        public ConvoyType Type { get; set; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        ///     必要IC
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        ///     必要人的資源
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     完了予定日
        /// </summary>
        public GameDate Date { get; set; }

        /// <summary>
        ///     進捗率増分
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        ///     総進捗率
        /// </summary>
        public double TotalProgress { get; set; }

        /// <summary>
        ///     連続生産ボーナス
        /// </summary>
        public double GearingBonus { get; set; }

        /// <summary>
        ///     連続生産数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     生産完了数
        /// </summary>
        public int Done { get; set; }

        /// <summary>
        ///     完了日数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        ///     最初の1単位の完了日数
        /// </summary>
        public int DaysForFirst { get; set; }

        /// <summary>
        ///     停止中
        /// </summary>
        public bool Halted { get; set; }

        /// <summary>
        ///     完了時にキューを削除するかどうか
        /// </summary>
        public bool CloseWhenFinished { get; set; }

        /// <summary>
        ///     詳細不明
        /// </summary>
        public bool WaitingForClosure { get; set; }

        #endregion
    }

    /// <summary>
    ///     輸送船団の種類
    /// </summary>
    public enum ConvoyType
    {
        None,
        Transports, // 輸送船
        Escorts // 護衛艦
    }

    #endregion

    #region 汎用

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

    #endregion
}