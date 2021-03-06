﻿namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     論理型のヘルパークラス
    /// </summary>
    public static class BoolHelper
    {
        /// <summary>
        ///     Yes/Noの文字列に変換する
        /// </summary>
        /// <param name="b">変換対象の値</param>
        /// <returns>変換後の文字列</returns>
        public static string ToString(bool b)
        {
            return b ? "yes" : "no";
        }
    }
}