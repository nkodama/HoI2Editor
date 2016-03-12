using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究速度データ群
    /// </summary>
    internal static class Researches
    {
        #region 公開プロパティ

        /// <summary>
        ///     研究速度リスト
        /// </summary>
        internal static readonly List<Research> Items = new List<Research>();

        /// <summary>
        ///     研究速度計算時の基準年度
        /// </summary>
        internal static ResearchDateMode DateMode { get; set; }

        /// <summary>
        ///     指定日付
        /// </summary>
        internal static GameDate SpecifiedDate { get; set; }

        /// <summary>
        ///     ロケット試験場の規模
        /// </summary>
        internal static int RocketTestingSites { get; set; }

        /// <summary>
        ///     原子炉の規模
        /// </summary>
        internal static int NuclearReactors { get; set; }

        /// <summary>
        ///     青写真の有無
        /// </summary>
        internal static bool Blueprint { get; set; }

        /// <summary>
        ///     研究速度補正
        /// </summary>
        internal static double Modifier { get; set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Researches()
        {
            SpecifiedDate = new GameDate();
            Modifier = 1;
        }

        #endregion

        #region 研究速度リスト

        /// <summary>
        ///     研究速度リストを更新する
        /// </summary>
        /// <param name="tech">対象技術</param>
        /// <param name="teams">研究機関</param>
        internal static void UpdateResearchList(TechItem tech, IEnumerable<Team> teams)
        {
            Items.Clear();

            // 研究速度を順に登録する
            foreach (Team team in teams)
            {
                Research research = new Research(tech, team);
                Items.Add(research);
            }

            // 研究日数の順にソートする
            Items.Sort((research1, research2) => research1.Days - research2.Days);
        }

        #endregion
    }

    /// <summary>
    ///     研究速度計算時の基準日付モード
    /// </summary>
    internal enum ResearchDateMode
    {
        Historical, // 史実年度を使用する
        Specified // 指定日付を使用する
    }
}