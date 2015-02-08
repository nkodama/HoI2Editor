using System.Globalization;
using HoI2Editor.Models;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     オブジェクト型のヘルパークラス
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        ///     オブジェクト型を文字列に変換する
        /// </summary>
        /// <param name="o">変換対象</param>
        /// <returns>文字列</returns>
        public static string ToString(object o)
        {
            if (o == null)
            {
                return string.Empty;
            }
            if (o is double)
            {
                return ((double) o).ToString(CultureInfo.InvariantCulture);
            }
            if (o is int)
            {
                return ((int) o).ToString(CultureInfo.InvariantCulture);
            }
            if (o is bool)
            {
                return ((bool) o) ? "yes" : "no";
            }
            if (o is Country)
            {
                return Countries.Strings[(int) (Country) o];
            }
            return o.ToString();
        }

        /// <summary>
        ///     2つのオブジェクトが等しい値かどうかを返す
        /// </summary>
        /// <param name="x">比較対象1</param>
        /// <param name="y">比較対象2</param>
        /// <returns>2つのオブジェクトが等しい値ならばtrueを返す</returns>
        public static bool IsEqual(object x, object y)
        {
            if ((x is double) && (y is double))
            {
                return DoubleHelper.IsEqual((double) x, (double) y);
            }
            return x.Equals(y);
        }

        /// <summary>
        ///     オブジェクトがnullか空文字列のどちらかかを返す
        /// </summary>
        /// <param name="o">判定対象</param>
        /// <returns>nullまたは空文字列ならばtrueを返す</returns>
        public static bool IsNullOrEmpty(object o)
        {
            return string.IsNullOrEmpty(o as string);
        }
    }
}