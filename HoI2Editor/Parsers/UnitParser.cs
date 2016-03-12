using System.Collections.Generic;
using System.Linq;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     ユニットデータの構文解析クラス
    /// </summary>
    internal class UnitParser
    {
        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Unit";

        #endregion

        #region 構文解析

        /// <summary>
        ///     ユニットファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="unit">ユニットデータ</param>
        /// <returns>構文解析の成否</returns>
        internal static bool Parse(string fileName, UnitClass unit)
        {
            using (TextLexer lexer = new TextLexer(fileName, true))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        break;
                    }

                    // 無効なトークン
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    string keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return false;
                    }
                    keyword = keyword.ToLower();

                    // allowed_brigades
                    if (keyword.Equals("allowed_brigades"))
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

                        string s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            return false;
                        }
                        s = s.ToLower();

                        // ユニットクラス名以外
                        if (!Units.StringMap.ContainsKey(s))
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // サポート外のユニットクラス
                        UnitType brigade = Units.StringMap[s];
                        if (!Units.BrigadeTypes.Contains(brigade))
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 旅団名
                        unit.AllowedBrigades.Add(brigade);
                        continue;
                    }

                    // max_allowed_brigades
                    if (keyword.Equals("max_allowed_brigades"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 最大旅団数
                        unit.MaxAllowedBrigades = (int) (double) token.Value;
                        continue;
                    }

                    // upgrade
                    if (keyword.Equals("upgrade"))
                    {
                        UnitUpgrade upgrade = ParseUpgrade(lexer);
                        if (upgrade == null)
                        {
                            Log.InvalidSection(LogCategory, "upgrade", lexer);
                            continue;
                        }

                        // 改良情報
                        unit.Upgrades.Add(upgrade);
                        continue;
                    }

                    // model
                    if (keyword.Equals("model"))
                    {
                        UnitModel model = ParseModel(lexer);
                        if (model == null)
                        {
                            Log.InvalidSection(LogCategory, "model", lexer);
                            continue;
                        }

                        // 自動改良先の初期設定
                        if (!model.AutoUpgrade)
                        {
                            model.UpgradeClass = unit.Type;
                            model.UpgradeModel = unit.Models.Count + 1;
                        }

                        // ユニットモデル
                        unit.Models.Add(model);
                        continue;
                    }

                    if (Game.Type == GameType.ArsenalOfDemocracy)
                    {
                        // land_unit_type
                        if (keyword.Equals("land_unit_type"))
                        {
                            // =
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Equal)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 陸軍師団/旅団
                            unit.Branch = Branch.Army;
                            continue;
                        }

                        // naval_unit_type
                        if (keyword.Equals("naval_unit_type"))
                        {
                            // =
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Equal)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 海軍師団/旅団
                            unit.Branch = Branch.Navy;
                            continue;
                        }

                        // air_unit_type
                        if (keyword.Equals("air_unit_type"))
                        {
                            // =
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Equal)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 空軍師団/旅団
                            unit.Branch = Branch.Airforce;
                            continue;
                        }

                        // max_speed_step
                        if (keyword.Equals("max_speed_step"))
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
                            if (token.Type != TokenType.Number)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }
                            int step = (int) (double) token.Value;
                            if (step < 0 || step > 2)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 最大生産速度
                            unit.MaxSpeedStep = step;
                            continue;
                        }

                        // locked
                        if (keyword.Equals("locked"))
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
                            if (token.Type != TokenType.Number)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 着脱可能
                            unit.Detachable = false;
                            continue;
                        }
                    }

                    else if (Game.Type == GameType.DarkestHour)
                    {
                        // detachable
                        if (keyword.Equals("detachable"))
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

                            string s = token.Value as string;
                            if (string.IsNullOrEmpty(s))
                            {
                                return false;
                            }
                            s = s.ToLower();

                            // yes
                            if (s.Equals("yes"))
                            {
                                unit.Detachable = true;
                                continue;
                            }

                            // no
                            if (s.Equals("no"))
                            {
                                unit.Detachable = false;
                                continue;
                            }

                            // 無効なトークン
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                }
            }

            return true;
        }

        /// <summary>
        ///     ユニットモデルを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>ユニットモデル</returns>
        private static UnitModel ParseModel(TextLexer lexer)
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

            UnitModel model = new UnitModel();
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
                    lexer.SkipLine();
                    continue;
                }

                string keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // cost
                if (keyword.Equals("cost"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 必要IC
                    model.Cost = (double) token.Value;
                    continue;
                }

                // buildtime
                if (keyword.Equals("buildtime"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 生産に要する時間
                    model.BuildTime = (double) token.Value;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 必要人的資源
                    model.ManPower = (double) token.Value;
                    continue;
                }

                // maxspeed
                if (keyword.Equals("maxspeed"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 移動速度
                    model.MaxSpeed = (double) token.Value;
                    continue;
                }

                // speed_cap_art
                if (keyword.Equals("speed_cap_art"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 砲兵旅団付随時の速度キャップ
                    model.SpeedCapArt = (double) token.Value;
                    continue;
                }

                // speed_cap_eng
                if (keyword.Equals("speed_cap_eng"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 工兵旅団付随時の速度キャップ
                    model.SpeedCapEng = (double) token.Value;
                    continue;
                }

                // speed_cap_at
                if (keyword.Equals("speed_cap_at"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対戦車旅団付随時の速度キャップ
                    model.SpeedCapAt = (double) token.Value;
                    continue;
                }

                // speed_cap_aa
                if (keyword.Equals("speed_cap_aa"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対空旅団付随時の速度キャップ
                    model.SpeedCapAa = (double) token.Value;
                    continue;
                }

                // range
                if (keyword.Equals("range"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 航続距離
                    model.Range = (double) token.Value;
                    continue;
                }

                // defaultorganisation
                if (keyword.Equals("defaultorganisation"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 組織率
                    model.DefaultOrganization = (double) token.Value;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 士気
                    model.Morale = (double) token.Value;
                    continue;
                }

                // defensiveness
                if (keyword.Equals("defensiveness"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 防御力
                    model.Defensiveness = (double) token.Value;
                    continue;
                }

                // seadefence
                if (keyword.Equals("seadefence"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対艦/対潜防御力
                    model.SeaDefense = (double) token.Value;
                    continue;
                }

                // airdefence
                if (keyword.Equals("airdefence"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対空防御力
                    model.AirDefence = (double) token.Value;
                    continue;
                }

                // surfacedefence
                if (keyword.Equals("surfacedefence"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対地/対艦防御力
                    model.SurfaceDefence = (double) token.Value;
                    continue;
                }

                // toughness
                if (keyword.Equals("toughness"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 耐久力
                    model.Toughness = (double) token.Value;
                    continue;
                }

                // softness
                if (keyword.Equals("softness"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 脆弱性
                    model.Softness = (double) token.Value;
                    continue;
                }

                // suppression
                if (keyword.Equals("suppression"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 制圧力
                    model.Suppression = (double) token.Value;
                    continue;
                }

                // softattack
                if (keyword.Equals("softattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対人攻撃力
                    model.SoftAttack = (double) token.Value;
                    continue;
                }

                // hardattack
                if (keyword.Equals("hardattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対甲攻撃力
                    model.HardAttack = (double) token.Value;
                    continue;
                }

                // seaattack
                if (keyword.Equals("seaattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対艦攻撃力(海軍)
                    model.SeaAttack = (double) token.Value;
                    continue;
                }

                // subattack
                if (keyword.Equals("subattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対潜攻撃力
                    model.SubAttack = (double) token.Value;
                    continue;
                }

                // convoyattack
                if (keyword.Equals("convoyattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 通商破壊力
                    model.ConvoyAttack = (double) token.Value;
                    continue;
                }

                // shorebombardment
                if (keyword.Equals("shorebombardment"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 湾岸攻撃力
                    model.ShoreBombardment = (double) token.Value;
                    continue;
                }

                // airattack
                if (keyword.Equals("airattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対空攻撃力
                    model.AirAttack = (double) token.Value;
                    continue;
                }

                // navalattack
                if (keyword.Equals("navalattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対艦攻撃力(空軍)
                    model.NavalAttack = (double) token.Value;
                    continue;
                }

                // strategicattack
                if (keyword.Equals("strategicattack"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 戦略爆撃力
                    model.StrategicAttack = (double) token.Value;
                    continue;
                }

                // distance
                if (keyword.Equals("distance"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 射程距離
                    model.Distance = (double) token.Value;
                    continue;
                }

                // surfacedetectioncapability
                if (keyword.Equals("surfacedetectioncapability"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対艦索敵能力
                    model.SurfaceDetectionCapability = (double) token.Value;
                    continue;
                }

                // subdetectioncapability
                if (keyword.Equals("subdetectioncapability"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対潜索敵能力
                    model.SubDetectionCapability = (double) token.Value;
                    continue;
                }

                // airdetectioncapability
                if (keyword.Equals("airdetectioncapability"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 対空索敵能力
                    model.AirDetectionCapability = (double) token.Value;
                    continue;
                }

                // visibility
                if (keyword.Equals("visibility"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 可視性
                    model.Visibility = (double) token.Value;
                    continue;
                }

                // transportweight
                if (keyword.Equals("transportweight"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 所要TC
                    model.TransportWeight = (double) token.Value;
                    continue;
                }

                // transportcapability
                if (keyword.Equals("transportcapability"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 輸送能力
                    model.TransportCapability = (double) token.Value;
                    continue;
                }

                // supplyconsumption
                if (keyword.Equals("supplyconsumption"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 消費物資
                    model.SupplyConsumption = (double) token.Value;
                    continue;
                }

                // fuelconsumption
                if (keyword.Equals("fuelconsumption"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 消費燃料
                    model.FuelConsumption = (double) token.Value;
                    continue;
                }

                // upgrade_time_factor
                if (keyword.Equals("upgrade_time_factor"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 改良時間補正
                    model.UpgradeTimeFactor = (double) token.Value;
                    continue;
                }

                // upgrade_cost_factor
                if (keyword.Equals("upgrade_cost_factor"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 改良IC補正
                    model.UpgradeCostFactor = (double) token.Value;
                    continue;
                }

                // AoD固有
                if (Game.Type == GameType.ArsenalOfDemocracy)
                {
                    // artillery_bombardment
                    if (keyword.Equals("artillery_bombardment"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 砲撃攻撃力
                        model.ArtilleryBombardment = (double) token.Value;
                        continue;
                    }

                    // max_supply_stock
                    if (keyword.Equals("max_supply_stock"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 最大携行物資
                        model.MaxSupplyStock = (double) token.Value;
                        continue;
                    }

                    // max_oil_stock
                    if (keyword.Equals("max_oil_stock"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 最大携行燃料
                        model.MaxOilStock = (double) token.Value;
                        continue;
                    }
                }

                // DH固有
                else if (Game.Type == GameType.DarkestHour)
                {
                    // no_fuel_combat_mod
                    if (keyword.Equals("no_fuel_combat_mod"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 燃料切れ時の戦闘補正
                        model.NoFuelCombatMod = (double) token.Value;
                        continue;
                    }

                    // reinforce_time
                    if (keyword.Equals("reinforce_time"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 補充時間補正
                        model.ReinforceTimeFactor = (double) token.Value;
                        continue;
                    }

                    // reinforce_cost
                    if (keyword.Equals("reinforce_cost"))
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 補充IC補正
                        model.ReinforceCostFactor = (double) token.Value;
                        continue;
                    }

                    // upgrade_time_boost
                    if (keyword.Equals("upgrade_time_boost"))
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

                        string s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            continue;
                        }
                        s = s.ToLower();

                        if (s.Equals("yes"))
                        {
                            // 改良時間の補正をするか
                            model.UpgradeTimeBoost = true;
                            continue;
                        }

                        if (s.Equals("no"))
                        {
                            // 改良時間の補正をするか
                            model.UpgradeTimeBoost = false;
                            continue;
                        }

                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 自動改良先ユニットクラス
                    if (Units.StringMap.ContainsKey(keyword))
                    {
                        // サポート外のユニット種類
                        UnitType type = Units.StringMap[keyword];
                        if (!Units.UnitTypes.Contains(type))
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // 他師団への自動改良
                        model.AutoUpgrade = true;
                        model.UpgradeClass = type;
                        model.UpgradeModel = (int) (double) token.Value;
                        continue;
                    }

                    // DH1.03以降固有
                    if (Game.Version >= 103)
                    {
                        // speed_cap
                        if (keyword.Equals("speed_cap"))
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
                            if (token.Type != TokenType.Number)
                            {
                                Log.InvalidToken(LogCategory, token, lexer);
                                lexer.SkipLine();
                                continue;
                            }

                            // 速度キャップ
                            model.SpeedCap = (double) token.Value;
                            continue;
                        }

                        // equipment
                        if (keyword.Equals("equipment"))
                        {
                            IEnumerable<UnitEquipment> equipments = ParseEquipment(lexer);
                            if (equipments == null)
                            {
                                Log.InvalidSection(LogCategory, "equipment", lexer);
                                continue;
                            }

                            // 装備
                            model.Equipments.AddRange(equipments);
                            continue;
                        }
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return model;
        }

        /// <summary>
        ///     equipmentセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>装備データ</returns>
        private static IEnumerable<UnitEquipment> ParseEquipment(TextLexer lexer)
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

            List<UnitEquipment> equipments = new List<UnitEquipment>();
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

                // 資源
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
                keyword = keyword.ToLower();

                if (!Units.EquipmentStringMap.ContainsKey(keyword))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }
                EquipmentType resource = Units.EquipmentStringMap[keyword];

                // =
                token = lexer.GetToken();
                if (token.Type != TokenType.Equal)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    return null;
                }

                // 値
                token = lexer.GetToken();
                if (token.Type != TokenType.Number)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                UnitEquipment equipment = new UnitEquipment { Resource = resource, Quantity = (double) token.Value };
                equipments.Add(equipment);
            }

            return equipments;
        }

        /// <summary>
        ///     upgradeセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>改良情報</returns>
        private static UnitUpgrade ParseUpgrade(TextLexer lexer)
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

            UnitUpgrade upgrade = new UnitUpgrade();
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
                    continue;
                }
                string keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    return null;
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    // ユニットクラス名以外
                    if (!Units.StringMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // サポート外のユニット種類
                    UnitType type = Units.StringMap[s];
                    if (!Units.DivisionTypes.Contains(type))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // ユニット種類
                    upgrade.Type = type;
                    continue;
                }

                // upgrade_cost_factor
                if (keyword.Equals("upgrade_cost_factor"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 改良コスト
                    upgrade.UpgradeCostFactor = (double) token.Value;
                    continue;
                }

                // upgrade_time_factor
                if (keyword.Equals("upgrade_time_factor"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 改良時間
                    upgrade.UpgradeTimeFactor = (double) token.Value;
                    continue;
                }


                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return upgrade;
        }

        /// <summary>
        ///     師団ユニットファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="units">ユニットクラス一覧</param>
        /// <returns>構文解析の成否</returns>
        internal static bool ParseDivisionTypes(string fileName, List<UnitClass> units)
        {
            using (TextLexer lexer = new TextLexer(fileName, true))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        break;
                    }

                    // 無効なトークン
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    string keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return false;
                    }
                    keyword = keyword.ToLower();

                    // eyr
                    if (keyword.Equals("eyr"))
                    {
                        if (!ParseEyr(lexer))
                        {
                            Log.InvalidSection(LogCategory, "eyr", lexer);
                            Log.Warning("[Unit] Parse failed: eyr section");
                            // 実装したらcontinueを追加すること
                            // continue;
                        }
                        // 実装したら解析結果を格納
                        continue;
                    }

                    // ユニットクラス名以外
                    if (!Units.StringMap.ContainsKey(keyword))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // サポート外のユニット種類
                    UnitType type = Units.StringMap[keyword];
                    if (!Units.DivisionTypes.Contains(type))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // ユニット種類
                    if (!ParseUnitClass(lexer, units[(int) type]))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     旅団ユニットファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="units">ユニットクラス一覧</param>
        /// <returns>構文解析の成否</returns>
        internal static bool ParseBrigadeTypes(string fileName, List<UnitClass> units)
        {
            using (TextLexer lexer = new TextLexer(fileName, true))
            {
                while (true)
                {
                    Token token = lexer.GetToken();

                    // ファイルの終端
                    if (token == null)
                    {
                        break;
                    }

                    // 無効なトークン
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    string keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return false;
                    }
                    keyword = keyword.ToLower();

                    // ユニットクラス名以外
                    if (!Units.StringMap.ContainsKey(keyword))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // サポート外のユニット種類
                    UnitType type = Units.StringMap[keyword];
                    if (!Units.BrigadeTypes.Contains(type))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // ユニット種類
                    if (!ParseUnitClass(lexer, units[(int) type]))
                    {
                        Log.InvalidSection(LogCategory, keyword, lexer);
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     eyrセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseEyr(TextLexer lexer)
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
                    continue;
                }
                string keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    return false;
                }
                keyword = keyword.ToLower();

                // army/navy/air
                if (keyword.Equals("army") || keyword.Equals("navy") || keyword.Equals("air"))
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

                        // 実装したらcontinueを追加すること
                        //continue;
                    }

                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return true;
        }

        /// <summary>
        ///     ユニットクラスセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="unit">ユニットデータ</param>
        /// <returns>構文解析の成否</returns>
        private static bool ParseUnitClass(TextLexer lexer, UnitClass unit)
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
                    continue;
                }
                string keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    return false;
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return false;
                    }
                    s = s.ToLower();

                    if (unit.Organization == UnitOrganization.Division)
                    {
                        // 実ユニット種類名
                        if (Units.RealStringMap.ContainsKey(s))
                        {
                            // 実ユニット種類
                            unit.RealType = Units.RealStringMap[s];
                            // 実ユニット種類に対応する兵科を設定する
                            unit.Branch = Units.RealBranchTable[(int) unit.RealType];
                            // 実体存在フラグを設定する
                            unit.SetEntity();
                            continue;
                        }
                    }
                    else
                    {
                        // land
                        if (s.Equals("land"))
                        {
                            // 陸軍旅団
                            unit.Branch = Branch.Army;
                            // 実体存在フラグを設定する
                            unit.SetEntity();
                            continue;
                        }
                        // naval
                        if (s.Equals("naval"))
                        {
                            // 海軍旅団
                            unit.Branch = Branch.Navy;
                            // 実体存在フラグを設定する
                            unit.SetEntity();
                            continue;
                        }
                        // air
                        if (s.Equals("air"))
                        {
                            // 空軍旅団
                            unit.Branch = Branch.Airforce;
                            // 実体存在フラグを設定する
                            unit.SetEntity();
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // ユニットクラス名
                    unit.Name = token.Value as string;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // short_name
                if (keyword.Equals("short_name"))
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

                    // ユニットクラス短縮名
                    unit.ShortName = token.Value as string;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // desc
                if (keyword.Equals("desc"))
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

                    // ユニットクラス説明
                    unit.Desc = token.Value as string;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // short_desc
                if (keyword.Equals("short_desc"))
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

                    // ユニットクラス短縮説明
                    unit.ShortDesc = token.Value as string;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // eyr
                if (keyword.Equals("eyr"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 統計グループ
                    unit.Eyr = (int) (double) token.Value;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // sprite
                if (keyword.Equals("sprite"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return false;
                    }
                    s = s.ToLower();

                    // スプライト種類名以外
                    if (!Units.SpriteStringMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // スプライト種類
                    unit.Sprite = Units.SpriteStringMap[s];
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // transmute
                if (keyword.Equals("transmute"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return false;
                    }
                    s = s.ToLower();

                    // ユニットクラス名以外
                    if (!Units.StringMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // サポート外のユニット種類
                    UnitType type = Units.StringMap[s];
                    if (!Units.DivisionTypes.Contains(type))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 代替ユニット種類
                    unit.Transmute = type;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // gfx_prio
                if (keyword.Equals("gfx_prio"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 画像の優先度
                    unit.GfxPrio = (int) (double) token.Value;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
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
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 軍事力
                    unit.Value = (double) token.Value;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // list_prio
                if (keyword.Equals("list_prio"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // リストの優先度
                    unit.ListPrio = (int) (double) token.Value;
                    if (unit.ListPrio != -1)
                    {
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                    }
                    continue;
                }

                // ui_prio
                if (keyword.Equals("ui_prio"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // UI優先度
                    unit.UiPrio = (int) (double) token.Value;
                    // 実体存在フラグを設定する
                    unit.SetEntity();
                    continue;
                }

                // production
                if (keyword.Equals("production"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes"))
                    {
                        // 初期状態で生産可能
                        unit.Productable = true;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 初期状態で生産不可能
                        unit.Productable = false;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // cag
                if (keyword.Equals("cag"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes"))
                    {
                        // 空母航空隊である
                        unit.Cag = true;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 空母航空隊ではない
                        unit.Cag = false;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // escort
                if (keyword.Equals("escort"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes"))
                    {
                        // 護衛戦闘機である
                        unit.Escort = true;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 護衛戦闘機である
                        unit.Escort = false;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // engineer
                if (keyword.Equals("engineer"))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes"))
                    {
                        // 工兵である
                        unit.Engineer = true;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 工兵ではない
                        unit.Engineer = false;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // RealUnitType
                if (Units.RealStringMap.ContainsKey(keyword))
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes"))
                    {
                        // 実ユニット種類のデフォルトである
                        unit.DefaultType = true;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 実ユニット種類のデフォルトではない
                        unit.DefaultType = false;
                        // 実体存在フラグを設定する
                        unit.SetEntity();
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return true;
        }

        #endregion
    }
}