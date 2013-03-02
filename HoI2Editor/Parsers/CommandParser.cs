using System;
using System.Collections.Generic;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     コマンドの構文解析クラス
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        ///     コマンド種類文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, CommandType> TypeMap = new Dictionary<string, CommandType>();

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static CommandParser()
        {
            foreach (CommandType type in Enum.GetValues(typeof (CommandType)))
            {
                TypeMap.Add(Command.TypeStrings[(int) type], type);
            }
        }

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

            var command = new Command();
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

                // type
                if (keyword.Equals("type"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なコマンド種類文字列
                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();
                    if (!TypeMap.ContainsKey(s))
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // コマンド種類
                    command.Type = TypeMap[s];
                    continue;
                }

                // which
                if (keyword.Equals("which"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // パラメータ - which
                    command.Which = token.Value;
                    continue;
                }

                // value
                if (keyword.Equals("value"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // パラメータ - value
                    command.Value = token.Value;
                    continue;
                }

                // when
                if (keyword.Equals("when"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // パラメータ - when
                    command.When = token.Value;
                    continue;
                }

                // where
                if (keyword.Equals("where"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // パラメータ - where
                    command.Where = token.Value;
                    continue;
                }

                // trigger
                if (keyword.Equals("trigger"))
                {
                    // トリガー
                    List<Trigger> triggers = TriggerParser.Parse(lexer);
                    if (triggers != null)
                    {
                        command.Triggers = triggers;
                    }
                    continue;
                }

                // 無効なトークン
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                lexer.SkipLine();
            }

            return command;
        }
    }
}