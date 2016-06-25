using System;
using System.Collections.Generic;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     トリガーの構文解析クラス
    /// </summary>
    public static class TriggerParser
    {
        #region 内部フィールド

        /// <summary>
        ///     トリガー種類文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, TriggerType> TypeMap = new Dictionary<string, TriggerType>();

        #endregion

        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Command";

        #endregion

        #region 初期化

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

        #endregion

        #region 構文解析

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
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
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
            List<Trigger> list = new List<Trigger>();
            int lastLineNo = lexer.LineNo;
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
                    Log.InvalidToken(LogCategory, token, lexer);
                    if (lexer.LineNo != lastLineNo)
                    {
                        // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                        lexer.ReserveToken(token);
                        break;
                    }
                    continue;
                }
                string keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // 無効なキーワード
                if (!TypeMap.ContainsKey(keyword))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    if (lexer.LineNo != lastLineNo)
                    {
                        // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                        lexer.ReserveToken(token);
                        break;
                    }
                    continue;
                }

                Trigger trigger = new Trigger { Type = TypeMap[keyword] };

                // =
                token = lexer.GetToken();
                if (token.Type != TokenType.Equal)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                token = lexer.GetToken();
                if (token.Type == TokenType.OpenBrace)
                {
                    List<Trigger> triggers = ParseContainerTrigger(lexer);
                    if (triggers == null)
                    {
                        continue;
                    }
                    trigger.Value = triggers;
                    list.Add(trigger);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                if (token.Type != TokenType.Number &&
                    token.Type != TokenType.Identifier &&
                    token.Type != TokenType.String)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    if (lexer.LineNo != lastLineNo)
                    {
                        // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                        lexer.ReserveToken(token);
                        break;
                    }
                    continue;
                }

                trigger.Value = token.Value;

                list.Add(trigger);

                // 最終解釈行を覚えておく
                lastLineNo = lexer.LineNo;
            }

            return list.Count > 0 ? list : null;
        }

        #endregion
    }
}