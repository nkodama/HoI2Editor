using System.Globalization;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     整数型のヘルパークラス
    /// </summary>
    public static class IntHelper
    {
        #region 文字列変換

        /// <summary>
        ///     文字列に変換する
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString(int val)
        {
            return ToString0(val);
        }

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

        #endregion

        #region 数値変換

        /// <summary>
        ///     文字列を数値に変換する
        /// </summary>
        /// <param name="s">変換対象の文字列</param>
        /// <param name="val">変換後の値</param>
        /// <returns>変換が成功すればtrueを返す</returns>
        public static bool TryParse(string s, out int val)
        {
            return int.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
        }

        #endregion
    }
}