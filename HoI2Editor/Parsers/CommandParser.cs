using System.Collections.Generic;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     コマンドの構文解析クラス
    /// </summary>
    public static class CommandParser
    {
        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Command";

        #endregion

        #region 構文解析

        /// <summary>
        ///     commandセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>コマンド</returns>
        public static Command Parse(TextLexer lexer)
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

            Command command = new Command();
            int lastLineNo = lexer.LineNo;
            while (true)
            {
                token = lexer.GetToken();

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

                // type
                if (keyword.Equals("type"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なコマンド種類文字列
                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();
                    if (!Commands.StringMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // コマンド種類
                    command.Type = Commands.StringMap[s];

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // which
                if (keyword.Equals("which"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // パラメータ - which
                    command.Which = token.Value;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // value
                if (keyword.Equals("value"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // パラメータ - value
                    command.Value = token.Value;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // when
                if (keyword.Equals("when"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // パラメータ - when
                    command.When = token.Value;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // where
                if (keyword.Equals("where"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // パラメータ - where
                    command.Where = token.Value;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // trigger
                if (keyword.Equals("trigger"))
                {
                    List<Trigger> triggers = TriggerParser.Parse(lexer);
                    if (triggers == null)
                    {
                        continue;
                    }

                    // トリガー
                    command.Triggers.AddRange(triggers);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                if (lexer.LineNo != lastLineNo)
                {
                    // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                    lexer.ReserveToken(token);
                    break;
                }
            }

            return command;
        }

        #endregion
    }
}