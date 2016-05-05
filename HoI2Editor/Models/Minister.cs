using System;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     閣僚データ
    /// </summary>
    internal class Minister
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        internal Country Country { get; set; }

        /// <summary>
        ///     閣僚ID
        /// </summary>
        internal int Id
        {
            get { return _id; }
            set
            {
                Ministers.IdSet.Remove(_id);
                _id = value;
                Ministers.IdSet.Add(_id);
            }
        }

        /// <summary>
        ///     名前
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        internal string PictureName { get; set; }

        /// <summary>
        ///     閣僚地位
        /// </summary>
        internal MinisterPosition Position { get; set; }

        /// <summary>
        ///     閣僚特性
        /// </summary>
        internal int Personality { get; set; }

        /// <summary>
        ///     忠誠度
        /// </summary>
        internal MinisterLoyalty Loyalty { get; set; }

        /// <summary>
        ///     イデオロギー
        /// </summary>
        internal MinisterIdeology Ideology { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        internal int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        internal int EndYear { get; set; }

        /// <summary>
        ///     引退年
        /// </summary>
        internal int RetirementYear { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof(MinisterItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        /// <summary>
        ///     閣僚ID
        /// </summary>
        private int _id;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal Minister()
        {
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元の閣僚データ</param>
        internal Minister(Minister original)
        {
            Country = original.Country;
            Id = original.Id;
            Name = original.Name;
            PictureName = original.PictureName;
            Position = original.Position;
            Personality = original.Personality;
            Loyalty = original.Loyalty;
            Ideology = original.Ideology;
            StartYear = original.StartYear;
            EndYear = original.EndYear;
            RetirementYear = original.RetirementYear;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     閣僚データが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(MinisterItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        internal void SetDirty(MinisterItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        internal void SetDirtyAll()
        {
            foreach (MinisterItemId id in Enum.GetValues(typeof(MinisterItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        internal void ResetDirtyAll()
        {
            foreach (MinisterItemId id in Enum.GetValues(typeof(MinisterItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     閣僚地位
    /// </summary>
    internal enum MinisterPosition
    {
        None,
        HeadOfState, // 国家元首
        HeadOfGovernment, // 政府首班
        ForeignMinister, // 外務大臣
        MinisterOfArmament, // 軍需大臣
        MinisterOfSecurity, // 内務大臣
        HeadOfMilitaryIntelligence, // 情報大臣
        ChiefOfStaff, // 統合参謀総長
        ChiefOfArmy, // 陸軍総司令官
        ChiefOfNavy, // 海軍総司令官
        ChiefOfAirForce // 空軍総司令官
    }

    /// <summary>
    ///     閣僚忠誠度
    /// </summary>
    internal enum MinisterLoyalty
    {
        None,
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Undying,
        Na
    }

    /// <summary>
    ///     イデオロギー
    /// </summary>
    internal enum MinisterIdeology
    {
        None,
        NationalSocialist, // NS 国家社会主義
        Fascist, // FA ファシスト
        PaternalAutocrat, // PA 権威主義者
        SocialConservative, // SC 社会保守派
        MarketLiberal, // ML 自由経済派
        SocialLiberal, // SL 社会自由派
        SocialDemocrat, // SD 社会民主派
        LeftWingRadical, // LWR 急進的左翼
        Leninist, // LE レーニン主義者
        Stalinist // ST スターリン主義者
    }

    /// <summary>
    ///     閣僚項目ID
    /// </summary>
    internal enum MinisterItemId
    {
        Country, // 国家
        Id, // ID
        Name, // 名前
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Position, // 閣僚地位
        Personality, // 閣僚特性
        Ideology, // イデオロギー
        Loyalty, // 忠誠度
        PictureName // 画像ファイル名
    }
}