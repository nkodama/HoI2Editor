using System.Collections.Generic;
using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     閣僚特性定義ファイルの構文解析(AoD)
    /// </summary>
    internal class MinisterModifierParser
    {
        #region 内部定数

        /// <summary>
        ///     閣僚特性定義ファイル内の閣僚地位名とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, int> PositionMap
            = new Dictionary<string, int>
            {
                { "", (int) MinisterPosition.None },
                { "headofstate", (int) MinisterPosition.HeadOfState },
                { "headofgovernment", (int) MinisterPosition.HeadOfGovernment },
                { "foreignminister", (int) MinisterPosition.ForeignMinister },
                { "armamentminister", (int) MinisterPosition.MinisterOfArmament },
                { "ministerofsecurity", (int) MinisterPosition.MinisterOfSecurity },
                { "ministerofintelligence", (int) MinisterPosition.HeadOfMilitaryIntelligence },
                { "chiefofstaff", (int) MinisterPosition.ChiefOfStaff },
                { "chiefofarmy", (int) MinisterPosition.ChiefOfArmy },
                { "chiefofnavy", (int) MinisterPosition.ChiefOfNavy },
                { "chiefofair", (int) MinisterPosition.ChiefOfAirForce }
            };

        #endregion

        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Minister";

        #endregion

        #region 構文解析

        /// <summary>
        ///     閣僚特性定義ファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>閣僚特性リスト</returns>
        public static List<MinisterPersonalityInfo> Parse(string fileName)
        {
            List<MinisterPersonalityInfo> list = null;

            using (TextLexer lexer = new TextLexer(fileName, true))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        return list;
                    }

                    // 無効なトークン
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    string keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        continue;
                    }

                    // minister_modifiersセクション
                    if (keyword.Equals("minister_modifiers"))
                    {
                        if (!ParseMinisterModifiers(lexer))
                        {
                            Log.InvalidSection(LogCategory, "minister_modifiers", lexer);
                        }
                        continue;
                    }

                    // minister_personalitiesセクション
                    if (keyword.Equals("minister_personalities"))
                    {
                        list = ParseMinisterPersonalities(lexer);
                        continue;
                    }

                    Log.InvalidToken(LogCategory, token, lexer);
                }
            }
        }

        /// <summary>
        ///     minister_modifiersセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseMinisterModifiers(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            while (true)
            {
                // 暫定: 識別子を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Identifier)
                {
                    lexer.SkipLine();
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     minister_personalitiesセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>閣僚特性リスト</returns>
        private static List<MinisterPersonalityInfo> ParseMinisterPersonalities(TextLexer lexer)
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

            List<MinisterPersonalityInfo> list = new List<MinisterPersonalityInfo>();
            while (true)
            {
                // ファイル終端
                token = lexer.GetToken();
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
                if (token.Type != TokenType.Identifier || !((string) token.Value).Equals("personality"))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // personalityセクション
                MinisterPersonalityInfo info = ParseMinisterPersonality(lexer);
                if (info == null)
                {
                    Log.InvalidSection(LogCategory, "personality", lexer);
                    continue;
                }

                // 閣僚特性リストへ登録
                list.Add(info);
            }

            return list;
        }

        /// <summary>
        ///     personalityセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>閣僚特性データ</returns>
        private static MinisterPersonalityInfo ParseMinisterPersonality(TextLexer lexer)
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

            MinisterPersonalityInfo info = new MinisterPersonalityInfo();
            while (true)
            {
                // ファイル終端
                token = lexer.GetToken();
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
                    return null;
                }

                string keyword = token.Value as string;
                if (keyword == null)
                {
                    return null;
                }

                // personality_string
                if (keyword.Equals("personality_string"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 閣僚特性文字列
                    info.String = token.Value as string;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 閣僚特性名
                    info.Name = token.Value as string;
                    continue;
                }

                // desc
                if (keyword.Equals("desc"))
                {
                    // 暫定: 1行単位で読み飛ばす
                    lexer.SkipLine();
                    continue;
                }

                // minister_position
                if (keyword.Equals("minister_position"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    string position = token.Value as string;
                    if (string.IsNullOrEmpty(position))
                    {
                        continue;
                    }
                    position = position.ToLower();

                    // 閣僚地位
                    if (PositionMap.ContainsKey(position))
                    {
                        // いずれか1つ
                        info.Position[PositionMap[position]] = true;
                    }
                    else if (position.Equals("all"))
                    {
                        // 全て
                        for (int i = 0; i < info.Position.Length; i++)
                        {
                            info.Position[i] = true;
                        }
                    }
                    else if (Game.Type != GameType.DarkestHour || !position.Equals("generic"))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                    }
                    continue;
                }

                // modifier
                if (keyword.Equals("modifier"))
                {
                    if (!ParseMinisterPersonalityModifier(lexer))
                    {
                        Log.InvalidSection(LogCategory, "modifier", lexer);
                    }
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
            }

            return info;
        }

        /// <summary>
        ///     modifierセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseMinisterPersonalityModifier(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            while (true)
            {
                // ファイル終端
                token = lexer.GetToken();
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
                    return false;
                }

                string keyword = token.Value as string;
                if (keyword == null)
                {
                    return false;
                }

                // type
                if (keyword.Equals("type"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 識別子
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
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
                        return false;
                    }

                    // 識別子/数字
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // option1
                if (keyword.Equals("option1"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 数字
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // option2
                if (keyword.Equals("option2"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 数字
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // modifier_effect
                if (keyword.Equals("modifier_effect"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 数字
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // division
                if (keyword.Equals("division"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 識別子
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }

                    // 識別子
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return false;
                    }
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            return true;
        }

        #endregion
    }
}