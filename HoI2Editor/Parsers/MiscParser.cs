using System.Linq;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     miscファイルの構文解析クラス
    /// </summary>
    public static class MiscParser
    {
        #region 構文解析

        /// <summary>
        ///     miscファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>構文解析の成否</returns>
        public static bool Parse(string fileName)
        {
            // ゲームの種類を設定する
            MiscGameType gameType;
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    gameType = (Game.Version >= 130) ? MiscGameType.Dda13 : MiscGameType.Dda12;
                    break;

                case GameType.ArsenalOfDemocracy:
                    gameType = (Game.Version >= 108)
                                   ? MiscGameType.Aod108
                                   : ((Game.Version <= 104) ? MiscGameType.Aod104 : MiscGameType.Aod107);
                    break;

                case GameType.DarkestHour:
                    gameType = (Game.Version >= 103) ? MiscGameType.Dh103 : MiscGameType.Dh102;
                    break;

                default:
                    gameType = MiscGameType.Dda12;
                    break;
            }

            using (var lexer = new TextLexer(fileName, false))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        return true;
                    }

                    // 空白文字/コメントを読み飛ばす
                    if (token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment)
                    {
                        continue;
                    }

                    // 無効なトークン
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        continue;
                    }

                    var keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        continue;
                    }

                    // economyセクション
                    if (keyword.Equals("economy"))
                    {
                        if (!ParseSection(MiscSectionId.Economy, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "economy",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // intelligenceセクション
                    if (keyword.Equals("intelligence"))
                    {
                        if (!ParseSection(MiscSectionId.Intelligence, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "intelligence",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // diplomacyセクション
                    if (keyword.Equals("diplomacy"))
                    {
                        if (!ParseSection(MiscSectionId.Diplomacy, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "diplomacy",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // combatセクション
                    if (keyword.Equals("combat"))
                    {
                        if (!ParseSection(MiscSectionId.Combat, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "combat",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // missionセクション
                    if (keyword.Equals("mission"))
                    {
                        if (!ParseSection(MiscSectionId.Mission, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "mission",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // countryセクション
                    if (keyword.Equals("country"))
                    {
                        if (!ParseSection(MiscSectionId.Country, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "country",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // researchセクション
                    if (keyword.Equals("research"))
                    {
                        if (!ParseSection(MiscSectionId.Research, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "research",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // tradeセクション
                    if (keyword.Equals("trade"))
                    {
                        if (!ParseSection(MiscSectionId.Trade, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "trade",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // aiセクション
                    if (keyword.Equals("ai"))
                    {
                        if (!ParseSection(MiscSectionId.Ai, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "ai",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // modセクション
                    if (keyword.Equals("mod"))
                    {
                        if (!ParseSection(MiscSectionId.Mod, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "mod",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // mapセクション
                    if (keyword.Equals("map"))
                    {
                        if (!ParseSection(MiscSectionId.Map, gameType, lexer))
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "map",
                                                    Resources.Section, "misc.txt"));
                        }
                        continue;
                    }

                    // 無効なトークン
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                }
            }
        }

        /// <summary>
        ///     セクションを構文解析する
        /// </summary>
        /// <param name="sectionId">セクションID</param>
        /// <param name="gameType">ゲームの種類</param>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseSection(MiscSectionId sectionId, MiscGameType gameType, TextLexer lexer)
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
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                return false;
            }

            MiscItemId[] itemIds
                = Misc.SectionItems[(int) sectionId].Where(id => Misc.ItemTable[(int) id, (int) gameType]).ToArray();
            int index = 0;

            while (index < itemIds.Length - 1)
            {
                // 空白文字/コメントを保存する
                token = lexer.GetToken();
                if (token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment)
                {
                    Misc.AppendComment(itemIds[index], token.Value as string);
                    continue;
                }

                // 設定値
                if (token.Type != TokenType.Number)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    return false;
                }
                MiscItemId itemId = itemIds[index];
                switch (Misc.ItemTypes[(int) itemId])
                {
                    case MiscItemType.Bool:
                        Misc.SetItem(itemId, (int) (double) token.Value != 0);
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
                        Misc.SetItem(itemId, (int) (double) token.Value);
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
                        Misc.SetItem(itemId, (double) token.Value);
                        break;
                }
                //Log.Write(string.Format("{0}: {1}\n", itemId, token.Value));
                index++;
            }

            // 空白文字/コメントを保存する
            while (true)
            {
                token = lexer.GetToken();
                if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.Comment)
                {
                    break;
                }
                Misc.AppendComment(itemIds[index], token.Value as string);
            }

            // } (セクション終端)
            if (token.Type != TokenType.CloseBrace)
            {
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                return false;
            }

            return true;
        }

        #endregion
    }
}