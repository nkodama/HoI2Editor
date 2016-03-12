using System.Linq;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     兵科データ
    /// </summary>
    internal static class Branches
    {
        #region 内部定数

        /// <summary>
        ///     兵科名
        /// </summary>
        private static readonly TextId[] Names =
        {
            TextId.Empty,
            TextId.BranchArmy,
            TextId.BranchNavy,
            TextId.BranchAirForce
        };

        #endregion

        #region 公開メソッド

        /// <summary>
        ///     兵科名を取得する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <returns>兵科名</returns>
        internal static string GetName(Branch branch)
        {
            return Config.GetText(Names[(int) branch]);
        }

        /// <summary>
        ///     兵科名の集合を取得する
        /// </summary>
        /// <returns>兵科名の集合</returns>
        internal static string[] GetNames()
        {
            return Names.Where(id => id != TextId.Empty).Select(Config.GetText).ToArray();
        }

        #endregion
    }

    /// <summary>
    ///     兵科
    /// </summary>
    internal enum Branch
    {
        None,
        Army, // 陸軍
        Navy, //海軍
        Airforce //空軍
    }
}