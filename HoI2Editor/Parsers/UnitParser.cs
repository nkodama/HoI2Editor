using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     ユニットデータの構文解析クラス
    /// </summary>
    public class UnitParser
    {
        /// <summary>
        ///     解析中のファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        ///     ユニットファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="unit">ユニットデータ</param>
        /// <returns>構文解析の成否</returns>
        public static bool Parse(string fileName, Unit unit)
        {
            _fileName = Path.GetFileName(fileName);
            using (var lexer = new TextLexer(fileName))
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    var keyword = token.Value as string;
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

                        var s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            return false;
                        }
                        s = s.ToLower();

                        // ユニットクラス名以外
                        if (!Units.StringMap.ContainsKey(s))
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // サポート外のユニットクラス
                        UnitType brigade = Units.StringMap[s];
                        if (!Units.Types.Contains(brigade))
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 旅団名
                        unit.AllowedBrigades.Add(brigade);
                        continue;
                    }

                    // model
                    if (keyword.Equals("model"))
                    {
                        UnitModel model = ParseModel(lexer);
                        if (model == null)
                        {
                            Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "model",
                                                    Resources.Section, _fileName));
                            continue;
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
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 陸軍師団/旅団
                            unit.Branch = UnitBranch.Army;
                            continue;
                        }

                        // naval_unit_type
                        if (keyword.Equals("naval_unit_type"))
                        {
                            // =
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Equal)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 海軍師団/旅団
                            unit.Branch = UnitBranch.Navy;
                            continue;
                        }

                        // air_unit_type
                        if (keyword.Equals("air_unit_type"))
                        {
                            // =
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Equal)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 1
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number || (int) (double) token.Value != 1)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 空軍師団/旅団
                            unit.Branch = UnitBranch.AirForce;
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

                            var s = token.Value as string;
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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

            var model = new UnitModel();
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

                // cost
                if (keyword.Equals("cost"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 対空防御力
                    model.AirDefense = (double) token.Value;
                    continue;
                }

                // surfacedefence
                if (keyword.Equals("surfacedefence"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 対地/対艦防御力
                    model.SurfaceDefense = (double) token.Value;
                    continue;
                }

                // toughness
                if (keyword.Equals("toughness"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 消費燃料
                    model.SupplyConsumption = (double) token.Value;
                    continue;
                }

                // upgrade_time_factor
                if (keyword.Equals("upgrade_time_factor"))
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Number)
                        {
                            Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 補充IC補正
                        model.Cost = (double) token.Value;
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
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                                lexer.SkipLine();
                                continue;
                            }

                            // 無効なトークン
                            token = lexer.GetToken();
                            if (token.Type != TokenType.Number)
                            {
                                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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
                                Log.Write(string.Format("{0}: {1} {2} / {3}\n", Resources.ParseFailed, "equipment",
                                                        Resources.Section, _fileName));
                                continue;
                            }

                            // 装備
                            model.Equipments.AddRange(equipments);
                            continue;
                        }
                    }
                }

                // 無効なトークン
                Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
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

            var equipments = new List<UnitEquipment>();
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
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    continue;
                }

                var resource = token.Value as string;
                if (string.IsNullOrEmpty(resource))
                {
                    continue;
                }
                resource = resource.ToLower();

                // =
                token = lexer.GetToken();
                if (token.Type != TokenType.Equal)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    return null;
                }

                // 値
                token = lexer.GetToken();
                if (token.Type != TokenType.Number)
                {
                    Log.Write(string.Format("{0}: {1}\n", Resources.InvalidToken, token.Value));
                    continue;
                }

                var equipment = new UnitEquipment {Resource = resource, Quantity = (double) token.Value};
                equipments.Add(equipment);
            }

            return equipments;
        }
    }
}