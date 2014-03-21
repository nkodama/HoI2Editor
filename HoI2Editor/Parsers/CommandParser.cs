using System;
using System.Collections.Generic;
using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     コマンドの構文解析クラス
    /// </summary>
    public static class CommandParser
    {
        #region 内部フィールド

        /// <summary>
        ///     コマンド種類文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, CommandType> TypeMap = new Dictionary<string, CommandType>();

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static CommandParser()
        {
            foreach (CommandType type in Enum.GetValues(typeof (CommandType)))
            {
                TypeMap.Add(Command.TypeStringTable[(int) type], type);
            }
        }

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
                Log.Warning("[Command] Invalid token: {0}", token.Value);
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                    Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number && token.Type != TokenType.Identifier &&
                        token.Type != TokenType.String)
                    {
                        Log.Warning("[Command] Invalid token: {0}", token.Value);
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
                        command.Triggers.AddRange(triggers);
                    }
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Command] Invalid token: {0}", token.Value);
                lexer.SkipLine();
            }

            return command;
        }

        #endregion
    }
}