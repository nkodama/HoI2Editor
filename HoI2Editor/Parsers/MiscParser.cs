using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     miscファイルの構文解析クラス
    /// </summary>
    public static class MiscParser
    {
        /// <summary>
        ///     miscファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>構文解析の成否</returns>
        public static bool Parse(string fileName)
        {
            var lexer = new TextLexer(fileName);

            while (true)
            {
                Token token = lexer.Parse();

                if (token == null)
                {
                    return true;
                }

                if (token.Type != TokenType.Identifier)
                {
                    Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                    continue;
                }

                var identifier = token.Value as string;
                if (string.IsNullOrEmpty(identifier))
                {
                    return false;
                }

                if (identifier.Equals("economy"))
                {
                    if (!ParseEconomy(lexer))
                    {
                        Log.Write("Failed parsing economy section in misc.txt.\n\n");
                    }
                    continue;
                }

                if (identifier.Equals("intelligence"))
                {
                    if (!ParseIntelligence(lexer))
                    {
                        Log.Write("Failed parsing intelligence section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("diplomacy"))
                {
                    if (!ParseDiplomacy(lexer))
                    {
                        Log.Write("Failed parsing diplomacy section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("combat"))
                {
                    if (!ParseCombat(lexer))
                    {
                        Log.Write("Failed parsing combat section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("mission"))
                {
                    if (!ParseMission(lexer))
                    {
                        Log.Write("Failed parsing mission section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("country"))
                {
                    if (!ParseCountry(lexer))
                    {
                        Log.Write("Failed parsing country section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("research"))
                {
                    if (!ParseResearch(lexer))
                    {
                        Log.Write("Failed parsing research section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("trade"))
                {
                    if (!ParseTrade(lexer))
                    {
                        Log.Write("Failed parsing trade section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("ai"))
                {
                    if (!ParseAi(lexer))
                    {
                        Log.Write("Failed parsing ai section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("mod"))
                {
                    if (!ParseMod(lexer))
                    {
                        Log.Write("Failed parsing mod section in misc.txt.\n");
                    }
                    continue;
                }

                if (identifier.Equals("map"))
                {
                    if (!ParseMap(lexer))
                    {
                        Log.Write("Failed parsing map section in misc.txt.\n");
                    }
                    continue;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
            }
        }

        /// <summary>
        ///     economyセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseEconomy(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     intelligenceセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseIntelligence(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     diplomacyセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseDiplomacy(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     combatセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseCombat(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     missionセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseMission(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     countryセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseCountry(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     researchセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseResearch(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     tradeセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseTrade(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     aiセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseAi(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     modセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseMod(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.AiSpyDiplomacyLogger = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.CountryLogger = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.SwitchedAiLogger = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.NewAutoSaveFileFormat = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LoadNewAiSettingMultiPlayer = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.TradeEfficiencyCalculationInterval = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.DepotCalculationInterval = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LossesLogger = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.AllowBrigadeAttachingInSupply = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeArmy = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeFleet = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeAir = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.AllowUniquePictureAllLandProvinces = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.AutoReplyEvents = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.ForceActionsToShowNoValidCommands = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.EnableDecisionsPlayer = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelArmyComposition = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelArmyTechLevel = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelArmyMinStr = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelArmyMaxStr = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelArmyOrgRegain = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusControlledRebel = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusOccupiedEnemy = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusMountain = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusHill = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusForest = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusJungle = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusSwamp = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusDesert = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusPlain = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusUrban = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.RebelBonusBase = (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.ReturnMonthNoRebelArmy = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.NewMinisterFormat = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LoadSpriteModdirOnly = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LoadUnitIconModdirOnly = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LoadUnitPictureModdirOnly = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.LoadAiModdirOnly = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.UseSpeedGarrisonStatus = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }
            Misc.Mod.UseOldSaveGameFormat = (int) (double) token.Value;

            token = lexer.Parse();
            if (token.Type != TokenType.CloseBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     mapセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseMap(TextLexer lexer)
        {
            Token token = lexer.Parse();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            token = lexer.Parse();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            while (true)
            {
                token = lexer.Parse();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                Log.Write(string.Format("無効なトークン: {0}\n\n", token.Value));
                return false;
            }

            return true;
        }
    }
}