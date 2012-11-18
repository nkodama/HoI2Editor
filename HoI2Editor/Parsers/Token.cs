namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     字句解析トークン
    /// </summary>
    public class Token
    {
        /// <summary>
        ///     トークンの種類
        /// </summary>
        public TokenType Type;

        /// <summary>
        ///     トークンの値
        /// </summary>
        public object Value;
    }

    /// <summary>
    ///     トークンの種類
    /// </summary>
    public enum TokenType
    {
        Invalid, // 不正な値
        Identifier, // 識別子
        Number, // 数字
        String, // 文字列
        Equal, // =
        OpenBrace, // {
        CloseBrace, // }
    }
}