using System.Linq;
using System.Text;
using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     miscファイルの構文解析クラス
    /// </summary>
    internal static class MiscParser
    {
        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Misc";

        #endregion

        #region 構文解析

        /// <summary>
        ///     miscファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        internal static void Parse(string fileName)
        {
            // ゲームの種類を設定する
            MiscGameType type = Misc.GetGameType();

            using (TextLexer lexer = new TextLexer(fileName, false))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        return;
                    }

                    // 空白文字/コメントを読み飛ばす
                    if (token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment)
                    {
                        continue;
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

                    // economyセクション
                    if (keyword.Equals("economy"))
                    {
                        if (!ParseSection(MiscSectionId.Economy, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "economy", lexer);
                        }
                        continue;
                    }

                    // intelligenceセクション
                    if (keyword.Equals("intelligence"))
                    {
                        if (!ParseSection(MiscSectionId.Intelligence, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "intelligence", lexer);
                        }
                        continue;
                    }

                    // diplomacyセクション
                    if (keyword.Equals("diplomacy"))
                    {
                        if (!ParseSection(MiscSectionId.Diplomacy, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "diplomacy", lexer);
                        }
                        continue;
                    }

                    // combatセクション
                    if (keyword.Equals("combat"))
                    {
                        if (!ParseSection(MiscSectionId.Combat, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "combat", lexer);
                        }
                        continue;
                    }

                    // missionセクション
                    if (keyword.Equals("mission"))
                    {
                        if (!ParseSection(MiscSectionId.Mission, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "mission", lexer);
                        }
                        continue;
                    }

                    // countryセクション
                    if (keyword.Equals("country"))
                    {
                        if (!ParseSection(MiscSectionId.Country, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "country", lexer);
                        }
                        continue;
                    }

                    // researchセクション
                    if (keyword.Equals("research"))
                    {
                        if (!ParseSection(MiscSectionId.Research, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "research", lexer);
                        }
                        continue;
                    }

                    // tradeセクション
                    if (keyword.Equals("trade"))
                    {
                        if (!ParseSection(MiscSectionId.Trade, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "trade", lexer);
                        }
                        continue;
                    }

                    // aiセクション
                    if (keyword.Equals("ai"))
                    {
                        if (!ParseSection(MiscSectionId.Ai, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "ai", lexer);
                        }
                        continue;
                    }

                    // modセクション
                    if (keyword.Equals("mod"))
                    {
                        if (!ParseSection(MiscSectionId.Mod, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "mod", lexer);
                        }
                        continue;
                    }

                    // mapセクション
                    if (keyword.Equals("map"))
                    {
                        if (!ParseSection(MiscSectionId.Map, type, lexer))
                        {
                            Log.InvalidSection(LogCategory, "map", lexer);
                        }
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                }
            }
        }

        /// <summary>
        ///     セクションを構文解析する
        /// </summary>
        /// <param name="section">セクションID</param>
        /// <param name="type">ゲームの種類</param>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseSection(MiscSectionId section, MiscGameType type, TextLexer lexer)
        {
            // 空白文字/コメントを読み飛ばす
            Token token;
            while (true)
            {
                token = lexer.GetToken();
                if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.Comment)
                {
                    break;
                }
            }

            // =
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            // 空白文字/コメントを読み飛ばす
            while (true)
            {
                token = lexer.GetToken();
                if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.Comment)
                {
                    break;
                }
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            StringBuilder sb;
            foreach (MiscItemId id in Misc.SectionItems[(int) section]
                .Where(id => Misc.ItemTable[(int) id, (int) type]))
            {
                // 空白文字/コメントを保存する
                sb = new StringBuilder();
                while (true)
                {
                    token = lexer.GetToken();
                    if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.Comment)
                    {
                        break;
                    }
                    sb.Append(token.Value);
                }
                Misc.SetComment(id, sb.ToString());

                // 設定値
                if (token.Type != TokenType.Number)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    return false;
                }
                switch (Misc.ItemTypes[(int) id])
                {
                    case MiscItemType.Bool:
                        Misc.SetItem(id, (int) (double) token.Value != 0);
                        break;

                    case MiscItemType.Enum:
                    case MiscItemType.Int:
                    case MiscItemType.PosInt:
                    case MiscItemType.NonNegInt:
                    case MiscItemType.NonPosInt:
                    case MiscItemType.NonNegIntMinusOne:
                    case MiscItemType.NonNegInt1:
                    case MiscItemType.RangedInt:
                    case MiscItemType.RangedPosInt:
                    case MiscItemType.RangedIntMinusOne:
                    case MiscItemType.RangedIntMinusThree:
                        Misc.SetItem(id, (int) (double) token.Value);
                        break;

                    case MiscItemType.Dbl:
                    case MiscItemType.PosDbl:
                    case MiscItemType.NonNegDbl:
                    case MiscItemType.NonPosDbl:
                    case MiscItemType.NonNegDbl0:
                    case MiscItemType.NonNegDbl2:
                    case MiscItemType.NonNegDbl5:
                    case MiscItemType.NonPosDbl0:
                    case MiscItemType.NonPosDbl2:
                    case MiscItemType.NonNegDblMinusOne:
                    case MiscItemType.NonNegDblMinusOne1:
                    case MiscItemType.NonNegDbl2AoD:
                    case MiscItemType.NonNegDbl4Dda13:
                    case MiscItemType.NonNegDbl2Dh103Full:
                    case MiscItemType.NonNegDbl2Dh103Full1:
                    case MiscItemType.NonNegDbl2Dh103Full2:
                    case MiscItemType.NonPosDbl5AoD:
                    case MiscItemType.NonPosDbl2Dh103Full:
                    case MiscItemType.RangedDbl:
                    case MiscItemType.RangedDblMinusOne:
                    case MiscItemType.RangedDblMinusOne1:
                    case MiscItemType.RangedDbl0:
                    case MiscItemType.NonNegIntNegDbl:
                        Misc.SetItem(id, (double) token.Value);
                        break;
                }
            }

            // セクション末尾の空白文字/コメントを保存する
            sb = new StringBuilder();
            while (true)
            {
                token = lexer.GetToken();
                if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.Comment)
                {
                    break;
                }
                sb.Append(token.Value);
            }
            Misc.SetSuffix(section, sb.ToString());

            // } (セクション終端)
            if (token.Type != TokenType.CloseBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return false;
            }

            return true;
        }

        #endregion
    }
}