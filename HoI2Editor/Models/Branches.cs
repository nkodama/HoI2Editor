using System.Linq;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     兵科データ
    /// </summary>
    public static class Branches
    {
        /// <summary>
        ///     兵科名
        /// </summary>
        private static readonly string[] Names = { "", "EYR_ARMY", "EYR_NAVY", "EYR_AIRFORCE" };

        /// <summary>
        ///     兵科名を取得する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <returns>兵科名</returns>
        public static string GetName(Branch branch)
        {
            return Config.GetText(Names[(int) branch]);
        }

        /// <summary>
        ///     兵科名の集合を取得する
        /// </summary>
        /// <returns>兵科名の集合</returns>
        public static string[] GetNames()
        {
            return Names.Where(s => !string.IsNullOrEmpty(s)).Select(Config.GetText).ToArray();
        }
    }

    /// <summary>
    ///     兵科
    /// </summary>
    public enum Branch
    {
        None,
        Army, // 陸軍
        Navy, //海軍
        Airforce //空軍
    }
}