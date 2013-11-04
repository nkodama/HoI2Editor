using System;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     指揮官データ
    /// </summary>
    public class Leader
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     指揮官ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                Leaders.IdSet.Remove(_id);
                _id = value;
                Leaders.IdSet.Add(_id);
            }
        }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     初期スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     最大スキル
        /// </summary>
        public int MaxSkill { get; set; }

        /// <summary>
        ///     任官年
        /// </summary>
        public int[] RankYear
        {
            get { return _rankYear; }
        }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     引退年
        /// </summary>
        public int RetirementYear { get; set; }

        /// <summary>
        ///     理想階級
        /// </summary>
        public LeaderRank IdealRank { get; set; }

        /// <summary>
        ///     指揮官特性
        /// </summary>
        public uint Traits { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        ///     忠誠度
        /// </summary>
        public int Loyalty { get; set; }

        /// <summary>
        ///     兵科
        /// </summary>
        public Branch Branch { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (LeaderItemId)).Length];

        /// <summary>
        ///     任官年
        /// </summary>
        private readonly int[] _rankYear = new int[RankLength];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        /// <summary>
        ///     指揮官ID
        /// </summary>
        private int _id;

        #endregion

        #region 内部定数

        /// <summary>
        ///     階級の数
        /// </summary>
        private const int RankLength = 4;

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     指揮官データが編集済みかどうかを取得する
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
        public bool IsDirty(LeaderItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(LeaderItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
        {
            foreach (LeaderItemId id in Enum.GetValues(typeof (LeaderItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (LeaderItemId id in Enum.GetValues(typeof (LeaderItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     指揮官特性値
    /// </summary>
    public static class LeaderTraits
    {
        /// <summary>
        ///     特性なし
        /// </summary>
        public const uint None = 0x00000000;

        /// <summary>
        ///     兵站管理
        /// </summary>
        public const uint LogisticsWizard = 0x00000001;

        /// <summary>
        ///     防勢ドクトリン
        /// </summary>
        public const uint DefensiveDoctrine = 0x00000002;

        /// <summary>
        ///     攻勢ドクトリン
        /// </summary>
        public const uint OffensiveDoctrine = 0x00000004;

        /// <summary>
        ///     冬期戦
        /// </summary>
        public const uint WinterSpecialist = 0x00000008;

        /// <summary>
        ///     伏撃
        /// </summary>
        public const uint Trickster = 0x00000010;

        /// <summary>
        ///     工兵
        /// </summary>
        public const uint Engineer = 0x00000020;

        /// <summary>
        ///     要塞攻撃
        /// </summary>
        public const uint FortressBuster = 0x00000040;

        /// <summary>
        ///     機甲戦
        /// </summary>
        public const uint PanzerLeader = 0x00000080;

        /// <summary>
        ///     特殊戦
        /// </summary>
        public const uint Commando = 0x00000100;

        /// <summary>
        ///     古典派
        /// </summary>
        public const uint OldGuard = 0x00000200;

        /// <summary>
        ///     海狼
        /// </summary>
        public const uint SeaWolf = 0x00000400;

        /// <summary>
        ///     封鎖線突破の達人
        /// </summary>
        public const uint BlockadeRunner = 0x00000800;

        /// <summary>
        ///     卓越した戦術家
        /// </summary>
        public const uint SuperiorTactician = 0x00001000;

        /// <summary>
        ///     索敵
        /// </summary>
        public const uint Spotter = 0x00002000;

        /// <summary>
        ///     対戦車攻撃
        /// </summary>
        public const uint TankBuster = 0x00004000;

        /// <summary>
        ///     絨毯爆撃
        /// </summary>
        public const uint CarpetBomber = 0x00008000;

        /// <summary>
        ///     夜間航空作戦
        /// </summary>
        public const uint NightFlyer = 0x00010000;

        /// <summary>
        ///     対艦攻撃
        /// </summary>
        public const uint FleetDestroyer = 0x00020000;

        /// <summary>
        ///     砂漠のキツネ
        /// </summary>
        public const uint DesertFox = 0x00040000;

        /// <summary>
        ///     密林のネズミ
        /// </summary>
        public const uint JungleRat = 0x00080000;

        /// <summary>
        ///     市街戦
        /// </summary>
        public const uint UrbanWarfareSpecialist = 0x00100000;

        /// <summary>
        ///     レンジャー
        /// </summary>
        public const uint Ranger = 0x00200000;

        /// <summary>
        ///     山岳戦
        /// </summary>
        public const uint Mountaineer = 0x00400000;

        /// <summary>
        ///     高地戦
        /// </summary>
        public const uint HillsFighter = 0x00800000;

        /// <summary>
        ///     反撃戦
        /// </summary>
        public const uint CounterAttacker = 0x01000000;

        /// <summary>
        ///     突撃戦
        /// </summary>
        public const uint Assaulter = 0x02000000;

        /// <summary>
        ///     包囲戦
        /// </summary>
        public const uint Encircler = 0x04000000;

        /// <summary>
        ///     奇襲戦
        /// </summary>
        public const uint Ambusher = 0x08000000;

        /// <summary>
        ///     規律
        /// </summary>
        public const uint Disciplined = 0x10000000;

        /// <summary>
        ///     戦術的退却
        /// </summary>
        public const uint ElasticDefenceSpecialist = 0x20000000;

        /// <summary>
        ///     電撃戦
        /// </summary>
        public const uint Blitzer = 0x40000000;

        /// <summary>
        ///     陸軍特性
        /// </summary>
        public const uint ArmyTraits =
            LogisticsWizard | DefensiveDoctrine | OffensiveDoctrine | WinterSpecialist | Trickster | Engineer |
            FortressBuster | PanzerLeader | Commando | OldGuard | DesertFox | JungleRat | UrbanWarfareSpecialist |
            Ranger | Mountaineer | HillsFighter | CounterAttacker | Assaulter | Encircler | Ambusher | Disciplined |
            ElasticDefenceSpecialist | Blitzer;

        /// <summary>
        ///     海軍特性
        /// </summary>
        public const uint NavyTraits = OldGuard | SeaWolf | BlockadeRunner | SuperiorTactician | Spotter | NightFlyer;

        /// <summary>
        ///     空軍特性
        /// </summary>
        public const uint AirforceTraits =
            OldGuard | SuperiorTactician | Spotter | TankBuster | CarpetBomber | NightFlyer | FleetDestroyer;
    }

    /// <summary>
    ///     指揮官特性
    /// </summary>
    public enum LeaderTraitsId
    {
        LogisticsWizard, // 兵站管理
        DefensiveDoctrine, // 防勢ドクトリン
        OffensiveDoctrine, // 攻勢ドクトリン
        WinterSpecialist, // 冬期戦
        Trickster, // 伏撃
        Engineer, // 工兵
        FortressBuster, // 要塞攻撃
        PanzerLeader, // 機甲戦
        Commando, // 特殊戦
        OldGuard, // 古典派
        SeaWolf, // 海狼
        BlockadeRunner, // 封鎖線突破の達人
        SuperiorTactician, // 卓越した戦術家
        Spotter, // 索敵
        TankBuster, // 対戦車攻撃
        CarpetBomber, // 絨毯爆撃
        NightFlyer, // 夜間航空作戦
        FleetDestroyer, // 対艦攻撃
        DesertFox, // 砂漠のキツネ
        JungleRat, // 密林のネズミ
        UrbanWarfareSpecialist, // 市街戦
        Ranger, // レンジャー
        Mountaineer, // 山岳戦
        HillsFighter, // 高地戦
        CounterAttacker, // 反撃戦
        Assaulter, // 突撃戦
        Encircler, // 包囲戦
        Ambusher, // 奇襲戦
        Disciplined, // 規律
        ElasticDefenceSpecialist, // 戦術的退却
        Blitzer, // 電撃戦
    }

    /// <summary>
    ///     指揮官階級
    /// </summary>
    public enum LeaderRank
    {
        None,
        MajorGeneral, // 少将
        LieutenantGeneral, // 中将
        General, // 大将
        Marshal, // 元帥
    }

    /// <summary>
    ///     指揮官項目ID
    /// </summary>
    public enum LeaderItemId
    {
        Country, // 国家
        Id, // ID
        Name, // 名前
        Branch, // 兵科
        IdealRank, // 理想階級
        Skill, // スキル
        MaxSkill, // 最大スキル
        Experience, // 経験値
        Loyalty, // 忠誠度
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Rank3Year, // 少将任官年
        Rank2Year, // 中将任官年
        Rank1Year, // 大将任官年
        Rank0Year, // 元帥任官年
        PictureName, // 画像ファイル名
        LogisticsWizard, // 特性: 兵站管理
        DefensiveDoctrine, // 特性: 防勢ドクトリン
        OffensiveDoctrine, // 特性: 攻勢ドクトリン
        WinterSpecialist, // 特性: 冬期戦
        Trickster, // 特性: 伏撃
        Engineer, // 特性: 工兵
        FortressBuster, // 特性: 要塞攻撃
        PanzerLeader, // 特性: 機甲戦
        Commando, // 特性: 特殊戦
        OldGuard, // 特性: 古典派
        SeaWolf, // 特性: 海狼
        BlockadeRunner, // 特性: 封鎖線突破の達人
        SuperiorTactician, // 特性: 卓越した戦術家
        Spotter, // 特性: 索敵
        TankBuster, // 特性: 対戦車攻撃
        CarpetBomber, // 特性: 絨毯爆撃
        NightFlyer, // 特性: 夜間航空作戦
        FleetDestroyer, // 特性: 対艦攻撃
        DesertFox, // 特性: 砂漠のキツネ
        JungleRat, // 特性: 密林のネズミ
        UrbanWarfareSpecialist, // 特性: 市街戦
        Ranger, // 特性: レンジャー
        Mountaineer, // 特性: 山岳戦
        HillsFighter, // 特性: 高地戦
        CounterAttacker, // 特性: 反撃戦
        Assaulter, // 特性: 突撃戦
        Encircler, // 特性: 包囲戦
        Ambusher, // 特性: 奇襲戦
        Disciplined, // 特性: 規律
        ElasticDefenceSpecialist, // 特性: 戦術的退却
        Blitzer, // 特性: 電撃戦
    }
}