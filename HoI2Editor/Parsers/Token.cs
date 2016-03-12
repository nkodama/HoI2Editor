namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     字句解析トークン
    /// </summary>
    internal class Token
    {
        #region 公開プロパティ

        /// <summary>
        ///     トークンの種類
        /// </summary>
        internal TokenType Type { get; set; }

        /// <summary>
        ///     トークンの値
        /// </summary>
        internal object Value { get; set; }

        #endregion
    }

    /// <summary>
    ///     トークンの種類
    /// </summary>
    internal enum TokenType
    {
        Invalid, // 不正な値
        Identifier, // 識別子
        Number, // 数字
        String, // 文字列
        Equal, // =
        OpenBrace, // {
        CloseBrace, // }
        WhiteSpace, // 空白文字
        Comment // コメント
    }
}