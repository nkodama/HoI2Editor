using System;
using System.Globalization;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     実数型のヘルパークラス
    /// </summary>
    public static class DoubleHelper
    {
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
                return val.ToString("F6");
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5");
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4");
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3");
            }
            // 小数点以下2桁
            if (Math.Abs(val - Math.Round(val, 1)) > 0.005)
            {
                return val.ToString("F2");
            }
            // 小数点以下1桁
            if (Math.Abs(val - Math.Round(val)) > 0.05)
            {
                return val.ToString("F1");
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
                return val.ToString("F6");
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5");
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4");
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3");
            }
            // 小数点以下2桁
            if (Math.Abs(val - Math.Round(val, 1)) > 0.005)
            {
                return val.ToString("F2");
            }
            // 小数点以下1桁を保証する
            return val.ToString("F1");
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
                return val.ToString("F6");
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5");
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4");
            }
            // 小数点以下3桁
            if (Math.Abs(val - Math.Round(val, 2)) > 0.0005)
            {
                return val.ToString("F3");
            }
            // 小数点以下2桁を保証する
            return val.ToString("F2");
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
                return val.ToString("F6");
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5");
            }
            // 小数点以下4桁
            if (Math.Abs(val - Math.Round(val, 3)) > 0.00005)
            {
                return val.ToString("F4");
            }
            // 小数点以下3桁を保証する
            return val.ToString("F3");
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
                return val.ToString("F6");
            }
            // 小数点以下5桁
            if (Math.Abs(val - Math.Round(val, 4)) > 0.000005)
            {
                return val.ToString("F5");
            }
            // 小数点以下4桁を保証する
            return val.ToString("F4");
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
                return val.ToString("F6");
            }
            // 小数点以下5桁を保証する
            return val.ToString("F5");
        }
    }
}