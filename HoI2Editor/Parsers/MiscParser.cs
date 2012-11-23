using HoI2Editor.Models;
using HoI2Editor.Properties;

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
                Token token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    return true;
                }

                // 無効なトークン
                if (token.Type != TokenType.Identifier)
                {
                    Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
                    if (!ParseEconomy(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "economy", "misc.txt"));
                    }
                    continue;
                }

                // intelligenceセクション
                if (keyword.Equals("intelligence"))
                {
                    if (!ParseIntelligence(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "intelligence", "misc.txt"));
                    }
                    continue;
                }

                // diplomacyセクション
                if (keyword.Equals("diplomacy"))
                {
                    if (!ParseDiplomacy(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "diplomacy", "misc.txt"));
                    }
                    continue;
                }

                // combatセクション
                if (keyword.Equals("combat"))
                {
                    if (!ParseCombat(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "combat", "misc.txt"));
                    }
                    continue;
                }

                // missionセクション
                if (keyword.Equals("mission"))
                {
                    if (!ParseMission(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "mission", "misc.txt"));
                    }
                    continue;
                }

                // countryセクション
                if (keyword.Equals("country"))
                {
                    if (!ParseCountry(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "country", "misc.txt"));
                    }
                    continue;
                }

                // researchセクション
                if (keyword.Equals("research"))
                {
                    if (!ParseResearch(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "research", "misc.txt"));
                    }
                    continue;
                }

                // tradeセクション
                if (keyword.Equals("trade"))
                {
                    if (!ParseTrade(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "trade", "misc.txt"));
                    }
                    continue;
                }

                // aiセクション
                if (keyword.Equals("ai"))
                {
                    if (!ParseAi(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "ai", "misc.txt"));
                    }
                    continue;
                }

                // modセクション
                if (keyword.Equals("mod"))
                {
                    if (!ParseMod(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "mod", "misc.txt"));
                    }
                    continue;
                }

                // mapセクション
                if (keyword.Equals("map"))
                {
                    if (!ParseMap(lexer))
                    {
                        Log.Write(string.Format(Resources.ParseFailedSection, "map", "misc.txt"));
                    }
                    continue;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
            }
        }

        /// <summary>
        ///     economyセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseEconomy(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // AIのスパイ/外交活動をログに記録する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.AiSpyDiplomacyLogger = (int) (double) token.Value;

            // 国家の状態をログに記録する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.CountryLogger = (int) (double) token.Value;

            // AI切り替えをログに記録する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.SwitchedAiLogger = (int) (double) token.Value;

            // 新しい自動セーブファイル名フォーマットを使用する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.NewAutoSaveFileFormat = (int) (double) token.Value;

            // マルチプレイで新しいAI設定/切り替えを使用する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LoadNewAiSettingMultiPlayer = (int) (double) token.Value;

            // 新しい貿易システムの計算間隔
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.TradeEfficiencyCalculationInterval = (int) (double) token.Value;

            // 備蓄庫の統合/再配置の計算間隔
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.DepotCalculationInterval = (int) (double) token.Value;

            // 損失をログに記録する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LossesLogger = (int) (double) token.Value;

            // 自国領土外でも補給が届いていれば旅団の配備を可能にする
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.AllowBrigadeAttachingInSupply = (int) (double) token.Value;

            // 一括配備数(陸軍)
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeArmy = (int) (double) token.Value;

            // 一括配備数(海軍)
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeFleet = (int) (double) token.Value;

            // 一括配備数(空軍)
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.MultipleDeploymentSizeAir = (int) (double) token.Value;

            // 全ての陸地プロヴィンスで固有の画像を許可する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.AllowUniquePictureAllLandProvinces = (int) (double) token.Value;

            // プレイヤー国のイベントに自動応答する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.AutoReplyEvents = (int) (double) token.Value;

            // 無効なアクションを表示する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.ForceActionsToShowNoValidCommands = (int) (double) token.Value;

            // ディシジョンを有効にする
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.EnableDecisionsPlayer = (int) (double) token.Value;

            // 反乱軍の構成
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelArmyComposition = (int) (double) token.Value;

            // 反乱軍の技術レベル
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelArmyTechLevel = (int) (double) token.Value;

            // 反乱軍の最小戦力
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelArmyMinStr = (double) token.Value;

            // 反乱軍の最大戦力
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelArmyMaxStr = (double) token.Value;

            // 反乱軍の指揮統制率回復
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelArmyOrgRegain = (double) token.Value;

            // 隣接プロヴィンスが反乱軍に支配されている時の反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusControlledRebel = (double) token.Value;

            // 隣接プロヴィンスが敵軍に占領されている時の反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusOccupiedEnemy = (double) token.Value;

            // 山岳プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusMountain = (double) token.Value;

            // 丘陵プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusHill = (double) token.Value;

            // 森林プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusForest = (double) token.Value;

            // 密林プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusJungle = (double) token.Value;

            // 湿地プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusSwamp = (double) token.Value;

            // 砂漠プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusDesert = (double) token.Value;

            // 平地プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusPlain = (double) token.Value;

            // 都市プロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusUrban = (double) token.Value;

            // 海軍/空軍基地のあるプロヴィンスの反乱危険率増加値
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.RebelBonusBase = (double) token.Value;

            // 反乱軍消滅後にプロヴィンスが元の所有国に復帰するまでの月数
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.ReturnMonthNoRebelArmy = (int) (double) token.Value;

            // 新閣僚フォーマットを使用する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.NewMinisterFormat = ((int) (double) token.Value == 1);

            // スプライトをMODDIRからのみ読み込む
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LoadSpriteModdirOnly = (int) (double) token.Value;

            // ユニットアイコンをMODDIRからのみ読み込む
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LoadUnitIconModdirOnly = (int) (double) token.Value;

            // ユニット画像をMODDIRからのみ読み込む
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LoadUnitPictureModdirOnly = (int) (double) token.Value;

            // AIをMODDIRからのみ読み込む
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.LoadAiModdirOnly = (int) (double) token.Value;

            // 移動不可ユニットの判定に速度の値を使用する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.UseSpeedGarrisonStatus = (int) (double) token.Value;

            // DH1.02より前のセーブデータフォーマットを使用する
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }
            Misc.Mod.UseOldSaveGameFormat = (int) (double) token.Value;
            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
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
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            while (true)
            {
                // 暫定: 数字を読み飛ばす
                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    continue;
                }

                // } (セクション終端)
                if (token.Type == TokenType.CloseBrace)
                {
                    break;
                }

                // 無効なトークン
                Log.Write(string.Format(Resources.InvalidToken, token.Value));
                return false;
            }

            return true;
        }
    }
}