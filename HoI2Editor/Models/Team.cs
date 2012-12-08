namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ
    /// </summary>
    public class Team
    {
        /// <summary>
        ///     研究特性定義の数
        /// </summary>
        public const int SpecialityLength = 32;

        /// <summary>
        ///     研究特性
        /// </summary>
        private readonly TechSpeciality[] _specialities = new TechSpeciality[SpecialityLength];

        /// <summary>
        ///     国タグ
        /// </summary>
        public CountryTag CountryTag { get; set; }

        /// <summary>
        ///     研究機関ID
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
        ///     スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     研究特性
        /// </summary>
        public TechSpeciality[] Specialities
        {
            get { return _specialities; }
        }
    }
}