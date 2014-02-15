using System.Globalization;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     整数型のヘルパークラス
    /// </summary>
    public static class IntHelper
    {
        /// <summary>
        ///     文字列に変換する
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString0(int val)
        {
            return val.ToString("D", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下1桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString1(int val)
        {
            return val.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}