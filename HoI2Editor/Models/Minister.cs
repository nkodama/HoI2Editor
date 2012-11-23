namespace HoI2Editor.Models
{
    /// <summary>
    ///     閣僚データ
    /// </summary>
    public class Minister
    {
        /// <summary>
        ///     国タグ
        /// </summary>
        public CountryTag CountryTag { get; set; }

        /// <summary>
        ///     閣僚ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     閣僚ポスト
        /// </summary>
        public MinisterPosition Position { get; set; }

        /// <summary>
        ///     閣僚特性
        /// </summary>
        public int Personality { get; set; }

        /// <summary>
        ///     閣僚忠誠度
        /// </summary>
        public MinisterLoyalty Loyalty { get; set; }

        /// <summary>
        ///     イデオロギー
        /// </summary>
        public MinisterIdeology Ideology { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }
    }

    /// <summary>
    ///     閣僚ポスト
    /// </summary>
    public enum MinisterPosition
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
        ChiefOfAirForce, // 空軍総司令官
    }

    /// <summary>
    ///     閣僚忠誠度
    /// </summary>
    public enum MinisterLoyalty
    {
        None,
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Undying,
        Na,
    }

    /// <summary>
    ///     イデオロギー
    /// </summary>
    public enum MinisterIdeology
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
        Stalinist, // ST スターリン主義者
    }
}