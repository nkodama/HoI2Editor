using System;
using System.Globalization;

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
        /// <param name="o"></param>
        /// <returns></returns>
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
                return (Math.Abs((double) y - (double) x) <= 0.00005);
            }
            return x.Equals(y);
        }
    }
}