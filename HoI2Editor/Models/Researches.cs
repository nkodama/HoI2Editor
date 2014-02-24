using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究速度データ群
    /// </summary>
    public static class Researches
    {
        #region 公開プロパティ

        /// <summary>
        ///     研究速度リスト
        /// </summary>
        public static readonly List<Research> Items = new List<Research>();

        /// <summary>
        ///     研究速度計算時の基準年度
        /// </summary>
        public static ResearchYearMode YearMode { get; set; }

        /// <summary>
        ///     現在年度
        /// </summary>
        public static int CurrentYear { get; set; }

        /// <summary>
        ///     指定年度
        /// </summary>
        public static int SpecifiedYear { get; set; }

        /// <summary>
        ///     ロケット試験場の規模
        /// </summary>
        public static int RocketTestingSites { get; set; }

        /// <summary>
        ///     原子炉の規模
        /// </summary>
        public static int NuclearReactors { get; set; }

        /// <summary>
        ///     青写真の有無
        /// </summary>
        public static bool Blueprint { get; set; }

        /// <summary>
        ///     研究速度補正
        /// </summary>
        public static double Modifier { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Researches()
        {
            SpecifiedYear = 1936;
            Modifier = 1;
        }

        #endregion

        #region 研究速度リスト

        /// <summary>
        ///     研究速度リストを更新する
        /// </summary>
        /// <param name="tech">対象技術</param>
        /// <param name="teams">研究機関</param>
        public static void UpdateResearchList(TechItem tech, IEnumerable<Team> teams)
        {
            Items.Clear();

            // 研究速度を順に登録する
            foreach (Team team in teams)
            {
                var research = new Research(tech, team);
                Items.Add(research);
            }

            // 研究日数の順にソートする
            Items.Sort((research1, research2) => research1.Days - research2.Days);
        }

        #endregion
    }

    /// <summary>
    ///     研究速度計算時の基準年度
    /// </summary>
    public enum ResearchYearMode
    {
        Historical, // 史実年度を使用する
        Specified, // 指定年度を使用する
    }
}