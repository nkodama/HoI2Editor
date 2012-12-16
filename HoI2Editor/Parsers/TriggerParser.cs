using System;
using System.Collections.Generic;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     トリガーの構文解析クラス
    /// </summary>
    public static class TriggerParser
    {
        /// <summary>
        ///     トリガー種類文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, TriggerType> TypeMap = new Dictionary<string, TriggerType>();

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static TriggerParser()
        {
            foreach (TriggerType type in Enum.GetValues(typeof (TriggerType)))
            {
                TypeMap.Add(Trigger.TypeStringTable[(int) type], type);
            }
        }

        /// <summary>
        ///     triggerセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>トリガーリスト</returns>
        public static List<Trigger> Parse(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                return null;
            }

            return ParseContainerTrigger(lexer);
        }

        /// <summary>
        ///     トリガーのコンテナを構文解析する
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        private static List<Trigger> ParseContainerTrigger(TextLexer lexer)
        {
            var list = new List<Trigger>();
            while (true)
            {
                Token token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    break;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                if (token.Type != TokenType.Identifier)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    lexer.SkipLine();
                    continue;
                }
                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // 無効なキーワード
                if (!TypeMap.ContainsKey(keyword))
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    lexer.SkipLine();
                    continue;
                }

                var trigger = new Trigger {Type = TypeMap[keyword]};

                // =
                token = lexer.GetToken();
                if (token.Type != TokenType.Equal)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    return null;
                }

                token = lexer.GetToken();
                if (token.Type == TokenType.OpenBrace)
                {
                    trigger.Value = ParseContainerTrigger(lexer);
                    if (trigger.Value != null)
                    {
                        list.Add(trigger);
                    }
                    continue;
                }

                if (token.Type != TokenType.Number &&
                    token.Type != TokenType.Identifier &&
                    token.Type != TokenType.String)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    continue;
                }

                trigger.Value = token.Value;

                list.Add(trigger);
            }

            return list.Count > 0 ? list : null;
        }
    }
}