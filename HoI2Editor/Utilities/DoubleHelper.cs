using System;
using System.Globalization;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     実数型のヘルパークラス
    /// </summary>
    public static class DoubleHelper
    {
        #region 数値比較

        /// <summary>
        ///     数値が等しいかを判定する
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual(double val1, double val2)
        {
            // 小数点以下6桁まで考慮する
            return Math.Abs(val1 - val2) < 0.0000005;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下なし)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual0(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.5;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下1桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual1(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.05;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下2桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual2(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.005;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下3桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual3(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.0005;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下4桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual4(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.00005;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下5桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual5(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.000005;
        }

        /// <summary>
        ///     数値が等しいかを判定する (小数点以下6桁)
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値が等しければtrueを返す</returns>
        public static bool IsEqual6(double val1, double val2)
        {
            return Math.Abs(val1 - val2) < 0.0000005;
        }

        /// <summary>
        ///     数値が0に等しいかを判定する
        /// </summary>
        /// <param name="val">数値</param>
        /// <returns>数値が0に等しければtrueを返す</returns>
        public static bool IsZero(double val)
        {
            return Math.Abs(val) < 0.0000005;
        }

        /// <summary>
        ///     数値が指定値以下であるかを判定する
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値1が数値2以下であればtrueを返す</returns>
        public static bool IsLessOrEqual(double val1, double val2)
        {
            return (val1 - val2) < 0.0000005;
        }

        /// <summary>
        ///     数値が指定値以上であるかを判定する
        /// </summary>
        /// <param name="val1">数値1</param>
        /// <param name="val2">数値2</param>
        /// <returns>数値1が数値2以上であればtrueを返す</returns>
        public static bool IsGreaterOrEqual(double val1, double val2)
        {
            return (val1 - val2) > -0.0000005;
        }

        /// <summary>
        ///     数値が負であるかを判定する
        /// </summary>
        /// <param name="val">数値</param>
        /// <returns>数値が負であればtrueを返す</returns>
        public static bool IsNegative(double val)
        {
            return val < -0.0000005;
        }

        #endregion

        #region 文字列変換

        /// <summary>
        ///     文字列に変換する
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString(double val)
        {
            return val.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下なし)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString0(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5", CultureInfo.InvariantCulture);
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4", CultureInfo.InvariantCulture);
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3", CultureInfo.InvariantCulture);
            }
            // 小数点以下2桁
            if (Math.Abs(val - Math.Round(val, 1)) > 0.005)
            {
                return val.ToString("F2", CultureInfo.InvariantCulture);
            }
            // 小数点以下1桁
            if (Math.Abs(val - Math.Round(val)) > 0.05)
            {
                return val.ToString("F1", CultureInfo.InvariantCulture);
            }
            // 小数点以下なし
            return val.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下1桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString1(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5", CultureInfo.InvariantCulture);
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4", CultureInfo.InvariantCulture);
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3", CultureInfo.InvariantCulture);
            }
            // 小数点以下2桁
            if (Math.Abs(val - Math.Round(val, 1)) > 0.005)
            {
                return val.ToString("F2", CultureInfo.InvariantCulture);
            }
            // 小数点以下1桁を保証する
            return val.ToString("F1", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下2桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString2(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5", CultureInfo.InvariantCulture);
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4", CultureInfo.InvariantCulture);
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3", CultureInfo.InvariantCulture);
            }
            // 小数点以下2桁を保証する
            return val.ToString("F2", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下3桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString3(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5", CultureInfo.InvariantCulture);
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4", CultureInfo.InvariantCulture);
            }
            // 小数点以下3桁を保証する
            return val.ToString("F3", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (小数点以下4桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString4(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5", CultureInfo.InvariantCulture);
            }
            // 小数点以下4桁を保証する
            return val.ToString("F4", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     文字列に変換する (実数/小数点以下5桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString5(double val)
        {
            // 小数点以下6桁
            if (Math.Abs(val - Math.Round(val, 5)) > 0.0000005)
            {
                return val.ToString("F6", CultureInfo.InvariantCulture);
            }
            // 小数点以下5桁を保証する
            return val.ToString("F5", CultureInfo.InvariantCulture);
        }

        #endregion

        #region 数値変換

        /// <summary>
        ///     文字列を数値に変換する
        /// </summary>
        /// <param name="s">変換対象の文字列</param>
        /// <param name="val">変換後の値</param>
        /// <returns>変換が成功すればtrueを返す</returns>
        public static bool TryParse(string s, out double val)
        {
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
        }

        #endregion
    }
}