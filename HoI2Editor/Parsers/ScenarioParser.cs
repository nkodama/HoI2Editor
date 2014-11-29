using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     シナリオの構文解析クラス
    /// </summary>
    public static class ScenarioParser
    {
        #region 内部フィールド

        /// <summary>
        ///     解析中のファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        ///     ファイル名のスタック
        /// </summary>
        private static readonly Stack<string> FileNameStack = new Stack<string>();

        #endregion

        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Scenario";

        #endregion

        #region 構文解析

        #region シナリオデータ

        /// <summary>
        ///     シナリオファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <returns>構文解析の成否</returns>
        public static bool Parse(string fileName, Scenario scenario)
        {
            _fileName = fileName;
            using (var lexer = new TextLexer(fileName, true))
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

                    var keyword = token.Value as string;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return false;
                    }
                    keyword = keyword.ToLower();

                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name");
                            continue;
                        }

                        // シナリオ名
                        scenario.Name = s;
                        continue;
                    }

                    // panel
                    if (keyword.Equals("panel"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "panel");
                            continue;
                        }

                        // パネル画像名
                        scenario.PanelName = s;
                        continue;
                    }

                    // header
                    if (keyword.Equals("header"))
                    {
                        ScenarioHeader header = ParseHeader(lexer);
                        if (header == null)
                        {
                            Log.InvalidSection(LogCategory, "header");
                            continue;
                        }

                        // シナリオヘッダ
                        scenario.Header = header;
                        continue;
                    }

                    // globaldata
                    if (keyword.Equals("globaldata"))
                    {
                        ScenarioGlobalData data = ParseGlobalData(lexer);
                        if (data == null)
                        {
                            Log.InvalidSection(LogCategory, "globaldata");
                            continue;
                        }

                        // シナリオグローバルデータ
                        scenario.GlobalData = data;
                        continue;
                    }

                    // history
                    if (keyword.Equals("history"))
                    {
                        IEnumerable<int> list = ParseIdList(lexer);
                        if (list == null)
                        {
                            Log.InvalidSection(LogCategory, "history");
                            continue;
                        }

                        // 発生済みイベント
                        scenario.HistoryEvents.AddRange(list);
                        continue;
                    }

                    // sleepevent
                    if (keyword.Equals("sleepevent"))
                    {
                        IEnumerable<int> list = ParseIdList(lexer);
                        if (list == null)
                        {
                            Log.InvalidSection(LogCategory, "sleepevent");
                            continue;
                        }

                        // 休止イベント
                        scenario.SleepEvents.AddRange(list);
                        continue;
                    }

                    // save_date
                    if (keyword.Equals("save_date"))
                    {
                        GameDate startDate;
                        // シナリオ開始日時が設定されていなければ1936/1/1とみなす
                        if (scenario.GlobalData == null || scenario.GlobalData.StartDate == null)
                        {
                            startDate = new GameDate();
                        }
                        else
                        {
                            startDate = scenario.GlobalData.StartDate;
                        }

                        Dictionary<int, GameDate> dates = ParseSaveDate(lexer, startDate);
                        if (dates == null)
                        {
                            Log.InvalidSection(LogCategory, "save_date");
                            continue;
                        }

                        // 保存日時
                        scenario.SaveDates = dates;
                        continue;
                    }

                    // map
                    if (keyword.Equals("map"))
                    {
                        MapSettings map = ParseMap(lexer);
                        if (map == null)
                        {
                            Log.InvalidSection(LogCategory, "map");
                            continue;
                        }

                        // マップ設定
                        scenario.Map = map;
                        continue;
                    }

                    // event
                    if (keyword.Equals("event"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "event");
                            continue;
                        }

                        // イベントファイル
                        if (GetScenarioFileKind() == ScenarioFileKind.Top)
                        {
                            scenario.EventFiles.Add(s);
                        }
                        continue;
                    }

                    // include
                    if (keyword.Equals("include"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "include");
                            continue;
                        }

                        // インクルードファイル
                        if (GetScenarioFileKind() == ScenarioFileKind.Top)
                        {
                            scenario.IncludeFiles.Add(s);

                            // インクルードフォルダを設定する
                            scenario.IncludeFolder = Path.GetDirectoryName(s);
                        }

                        string pathName = Game.GetReadFileName(s);
                        if (!File.Exists(pathName))
                        {
                            Log.Warning("[Scenario] Not exist include file: {0}", s);
                            continue;
                        }

                        // インクルードファイルを解釈する
                        FileNameStack.Push(_fileName);
                        Log.Verbose("[Scenario] Include: {0}", s);
                        Parse(pathName, scenario);
                        _fileName = FileNameStack.Pop();
                        continue;
                    }

                    // province
                    if (keyword.Equals("province"))
                    {
                        ProvinceSettings province = ParseProvince(lexer);
                        if (province == null)
                        {
                            Log.InvalidSection(LogCategory, "province");
                            continue;
                        }

                        // プロヴィンス設定
                        switch (GetScenarioFileKind())
                        {
                            case ScenarioFileKind.BasesInc: // bases.inc
                                scenario.BasesProvinces.Add(province);
                                break;

                            case ScenarioFileKind.BasesDodInc: // bases_DOD.inc
                                scenario.BasesDodProvinces.Add(province);
                                break;

                            case ScenarioFileKind.VpInc: // vp.inc
                                scenario.VpProvinces.Add(province);
                                break;

                            case ScenarioFileKind.Top: // シナリオ.eug
                                scenario.TopProvinces.Add(province);
                                break;

                            default:
                                scenario.CountryProvinces.Add(province);
                                break;
                        }
                        continue;
                    }

                    // country
                    if (keyword.Equals("country"))
                    {
                        CountrySettings country = ParseCountry(lexer);
                        if (country == null)
                        {
                            Log.InvalidSection(LogCategory, "country");
                            continue;
                        }

                        // 国家設定
                        scenario.Countries.Add(country);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                }
            }

            return true;
        }

        /// <summary>
        ///     保存日時を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="startDate">開始日時</param>
        /// <returns>保存日時</returns>
        private static Dictionary<int, GameDate> ParseSaveDate(TextLexer lexer, GameDate startDate)
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

            var dates = new Dictionary<int, GameDate>();
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
                if (token.Type != TokenType.Number)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                var id = (int) (double) token.Value;

                int? n = ParseInt(lexer);
                if (!n.HasValue)
                {
                    Log.InvalidClause(LogCategory, ObjectHelper.ToString(id));
                    continue;
                }

                // 保存日時
                GameDate date = startDate.Minus((int) n);
                dates.Add(id, date);
            }

            return dates;
        }

        #endregion

        #region シナリオヘッダ

        /// <summary>
        ///     シナリオヘッダを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>シナリオヘッダ</returns>
        private static ScenarioHeader ParseHeader(TextLexer lexer)
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

            var header = new ScenarioHeader();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // シナリオヘッダ名
                    header.Name = s;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    header.StartDate = date;
                    continue;
                }

                // startyear
                if (keyword.Equals("startyear"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "startyear");
                        continue;
                    }

                    // 開始年
                    header.StartYear = (int) n;
                    continue;
                }

                // endyear
                if (keyword.Equals("endyear"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "endyear");
                        continue;
                    }

                    // 終了年
                    header.EndYear = (int) n;
                    continue;
                }

                // free
                if (keyword.Equals("free"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "free");
                        continue;
                    }

                    // 国家の自由選択
                    header.IsFreeSelection = (bool) b;
                    continue;
                }

                // combat
                if (keyword.Equals("combat"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "combat");
                        continue;
                    }

                    // ショートシナリオ
                    header.IsCombatScenario = (bool) b;
                    continue;
                }

                // selectable
                if (keyword.Equals("selectable"))
                {
                    IEnumerable<Country> list = ParseCountryList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "selectable");
                        continue;
                    }

                    // 選択可能国
                    header.SelectableCountries.AddRange(list);
                    continue;
                }

                // set_ai_aggresive
                if (keyword.Equals("set_ai_aggresive"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "set_ai_aggresive");
                        continue;
                    }

                    // AIの攻撃性
                    header.AiAggressive = (int) n;
                    continue;
                }

                // set_difficulty
                if (keyword.Equals("set_difficulty"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "set_difficulty");
                        continue;
                    }

                    // 難易度
                    header.Difficulty = (int) n;
                    continue;
                }

                // set_gamespeed
                if (keyword.Equals("set_gamespeed"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "set_gamespeed");
                        continue;
                    }

                    // ゲームスピード
                    header.GameSpeed = (int) n;
                    continue;
                }

                // 国タグ
                string tagName = keyword.ToUpper();
                if (Countries.StringMap.ContainsKey(tagName))
                {
                    Country tag = Countries.StringMap[tagName];
                    if (Countries.Tags.Contains(tag))
                    {
                        MajorCountrySettings country = ParseMajorCountry(lexer);
                        if (country == null)
                        {
                            Log.InvalidSection(LogCategory, tagName);
                            continue;
                        }

                        // 主要国設定
                        country.Country = tag;
                        header.MajorCountries.Add(country);
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return header;
        }

        /// <summary>
        ///     主要国情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>主要国情報</returns>
        private static MajorCountrySettings ParseMajorCountry(TextLexer lexer)
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

            var country = new MajorCountrySettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // desc
                if (keyword.Equals("desc"))
                {
                    string s = ParseStringOrIdentifier(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "desc");
                        continue;
                    }

                    // 説明文
                    country.Desc = s;
                    continue;
                }

                // picture
                if (keyword.Equals("picture"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "picture");
                        continue;
                    }

                    // プロパガンダ画像名
                    country.PictureName = s;
                    continue;
                }

                // bottom
                if (keyword.Equals("bottom"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "bottom");
                        continue;
                    }

                    // 右端に配置
                    country.Bottom = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return country;
        }

        #endregion

        #region シナリオグローバルデータ

        /// <summary>
        ///     シナリオグローバルデータを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>シナリオグローバルデータ</returns>
        private static ScenarioGlobalData ParseGlobalData(TextLexer lexer)
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

            var data = new ScenarioGlobalData();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // rules
                if (keyword.Equals("rules"))
                {
                    ScenarioRules rules = ParseRules(lexer);
                    if (rules == null)
                    {
                        Log.InvalidSection(LogCategory, "rules");
                        continue;
                    }

                    // ルール設定
                    data.Rules = rules;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    data.StartDate = date;
                    continue;
                }

                // enddate
                if (keyword.Equals("enddate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "enddate");
                        continue;
                    }

                    // 終了日時
                    data.EndDate = date;
                    continue;
                }

                // axis
                if (keyword.Equals("axis"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "axis");
                        continue;
                    }

                    // 枢軸国
                    data.Axis = alliance;
                    continue;
                }

                // allies
                if (keyword.Equals("allies"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "allies");
                        continue;
                    }

                    // 連合国
                    data.Allies = alliance;
                    continue;
                }

                // comintern
                if (keyword.Equals("comintern"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "comintern");
                        continue;
                    }

                    // 共産国
                    data.Comintern = alliance;
                    continue;
                }

                // alliance
                if (keyword.Equals("alliance"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "alliance");
                        continue;
                    }

                    // 同盟国
                    data.Alliances.Add(alliance);
                    continue;
                }

                // war
                if (keyword.Equals("war"))
                {
                    War war = ParseWar(lexer);
                    if (war == null)
                    {
                        Log.InvalidSection(LogCategory, "war");
                        continue;
                    }

                    // 戦争
                    data.Wars.Add(war);
                    continue;
                }

                // treaty
                if (keyword.Equals("treaty"))
                {
                    Treaty treaty = ParseTreaty(lexer);
                    if (treaty == null)
                    {
                        Log.InvalidSection(LogCategory, "treaty");
                        continue;
                    }

                    // 外交協定
                    data.Treaties.Add(treaty);
                    continue;
                }

                // dormant_leaders
                if (keyword.Equals("dormant_leaders"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    token = lexer.GetToken();
                    if (token.Type == TokenType.Identifier)
                    {
                        var s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            continue;
                        }
                        s = s.ToLower();

                        // yes
                        if (s.Equals("yes"))
                        {
                            data.DormantLeadersAll = true;
                            continue;
                        }

                        // no
                        if (s.Equals("no"))
                        {
                            data.DormantLeadersAll = false;
                            continue;
                        }

                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // {
                    if (token.Type != TokenType.OpenBrace)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        return null;
                    }

                    var list = new List<int>();
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
                        if (token.Type != TokenType.Number)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            lexer.SkipLine();
                            continue;
                        }

                        // ID
                        list.Add((int) (double) token.Value);
                    }

                    // 休止指揮官
                    data.DormantLeaders.AddRange(list);
                }

                // dormant_ministers
                if (keyword.Equals("dormant_ministers"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_ministers");
                        continue;
                    }

                    // 休止閣僚
                    data.DormantMinisters.AddRange(list);
                    continue;
                }

                // dormant_teams
                if (keyword.Equals("dormant_teams"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_teams");
                        continue;
                    }

                    // 休止研究機関
                    data.DormantTeams.AddRange(list);
                    continue;
                }

                // weather
                if (keyword.Equals("weather"))
                {
                    Weather weather = ParseWeather(lexer);
                    if (weather == null)
                    {
                        Log.InvalidSection(LogCategory, "weather");
                        continue;
                    }

                    // 天候設定
                    data.Weather = weather;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return data;
        }

        /// <summary>
        ///     ルール設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>ルール設定</returns>
        private static ScenarioRules ParseRules(TextLexer lexer)
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

            var rules = new ScenarioRules();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "diplomacy");
                        continue;
                    }

                    // 外交
                    rules.AllowDiplomacy = (bool) b;
                    continue;
                }

                // production
                if (keyword.Equals("production"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "production");
                        continue;
                    }

                    // 生産
                    rules.AllowProduction = (bool) b;
                    continue;
                }

                // technology
                if (keyword.Equals("technology"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "technology");
                        continue;
                    }

                    // 技術
                    rules.AllowTechnology = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return rules;
        }

        #endregion

        #region 天候

        /// <summary>
        ///     天候設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>天候設定</returns>
        private static Weather ParseWeather(TextLexer lexer)
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

            var weather = new Weather();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // static
                if (keyword.Equals("static"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "static");
                        continue;
                    }

                    // 固定設定
                    weather.Static = (bool) b;
                    continue;
                }

                // pattern
                if (keyword.Equals("pattern"))
                {
                    WeatherPattern pattern = ParseWeatherPattern(lexer);
                    if (pattern == null)
                    {
                        Log.InvalidSection(LogCategory, "pattern");
                        continue;
                    }

                    // 天候パターン
                    weather.Patterns.Add(pattern);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return weather;
        }

        /// <summary>
        ///     天候パターンを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>天候パターン</returns>
        private static WeatherPattern ParseWeatherPattern(TextLexer lexer)
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

            var pattern = new WeatherPattern();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    pattern.Id = id;
                    continue;
                }

                // provinces
                if (keyword.Equals("provinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "provinces");
                        continue;
                    }

                    // プロヴィンスリスト
                    pattern.Provinces.AddRange(list);
                    continue;
                }

                // centre
                if (keyword.Equals("centre"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "centre");
                        continue;
                    }

                    // 中央プロヴィンス
                    pattern.Centre = (int) n;
                    continue;
                }

                // speed
                if (keyword.Equals("speed"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "speed");
                        continue;
                    }

                    // 速度
                    pattern.Speed = (int) n;
                    continue;
                }

                // heading
                if (keyword.Equals("heading"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "heading");
                        continue;
                    }

                    // 方向
                    pattern.Heading = s;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return pattern;
        }

        #endregion

        #region マップ

        /// <summary>
        ///     マップ設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>マップ設定</returns>
        private static MapSettings ParseMap(TextLexer lexer)
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

            var map = new MapSettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // yes
                if (keyword.Equals("yes"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // プロヴィンスID
                    token = lexer.GetToken();
                    if (token.Type == TokenType.Number)
                    {
                        map.Yes.Add((int) (double) token.Value);
                        continue;
                    }

                    if (token.Type == TokenType.Identifier)
                    {
                        var s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            continue;
                        }
                        s = s.ToLower();

                        // all
                        if (s.Equals("all"))
                        {
                            map.All = true;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // no
                if (keyword.Equals("no"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // プロヴィンスID
                    token = lexer.GetToken();
                    if (token.Type == TokenType.Number)
                    {
                        map.No.Add((int) (double) token.Value);
                        continue;
                    }

                    if (token.Type == TokenType.Identifier)
                    {
                        var s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            continue;
                        }
                        s = s.ToLower();

                        // all
                        if (s.Equals("all"))
                        {
                            map.All = false;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // top
                if (keyword.Equals("top"))
                {
                    MapPoint point = ParsePoint(lexer);
                    if (point == null)
                    {
                        Log.InvalidSection(LogCategory, "top");
                        continue;
                    }

                    // マップの範囲(左上)
                    map.Top = point;
                    continue;
                }

                // bottom
                if (keyword.Equals("bottom"))
                {
                    MapPoint point = ParsePoint(lexer);
                    if (point == null)
                    {
                        Log.InvalidSection(LogCategory, "bottom");
                        continue;
                    }

                    // マップの範囲(右下)
                    map.Bottom = point;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return map;
        }

        /// <summary>
        ///     マップの座標を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>マップの座標</returns>
        private static MapPoint ParsePoint(TextLexer lexer)
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

            var point = new MapPoint();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // x
                if (keyword.Equals("x"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "x");
                        continue;
                    }

                    // X座標
                    point.X = (int) n;
                    continue;
                }

                // y
                if (keyword.Equals("y"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "y");
                        continue;
                    }

                    // Y座標
                    point.Y = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return point;
        }

        #endregion

        #region プロヴィンス

        /// <summary>
        ///     プロヴィンス設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>プロヴィンス設定</returns>
        private static ProvinceSettings ParseProvince(TextLexer lexer)
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

            var province = new ProvinceSettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "id");
                        continue;
                    }

                    // プロヴィンスID
                    province.Id = (int) n;
                    continue;
                }

                // ic
                if (keyword.Equals("ic"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "ic");
                        continue;
                    }

                    // IC
                    province.Ic = size;
                    continue;
                }

                // infra
                if (keyword.Equals("infra"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "infra");
                        continue;
                    }

                    // インフラ
                    province.Infrastructure = size;
                    continue;
                }

                // landfort
                if (keyword.Equals("landfort"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "landfort");
                        continue;
                    }

                    // 陸上要塞
                    province.LandFort = size;
                    continue;
                }

                // coastalfort
                if (keyword.Equals("coastalfort"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "coastalfort");
                        continue;
                    }

                    // 沿岸要塞
                    province.CoastalFort = size;
                    continue;
                }

                // anti_air
                if (keyword.Equals("anti_air"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "anti_air");
                        continue;
                    }

                    // 対空砲
                    province.AntiAir = size;
                    continue;
                }

                // air_base
                if (keyword.Equals("air_base"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "air_base");
                        continue;
                    }

                    // 空軍基地
                    province.AirBase = size;
                    continue;
                }

                // naval_base
                if (keyword.Equals("naval_base"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "naval_base");
                        continue;
                    }

                    // 海軍基地
                    province.NavalBase = size;
                    continue;
                }

                // radar_station
                if (keyword.Equals("radar_station"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "radar_station");
                        continue;
                    }

                    // レーダー基地
                    province.RadarStation = size;
                    continue;
                }

                // nuclear_reactor
                if (keyword.Equals("nuclear_reactor"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "nuclear_reactor");
                        continue;
                    }

                    // 原子炉
                    province.NuclearReactor = size;
                    continue;
                }

                // rocket_test
                if (keyword.Equals("rocket_test"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "rocket_test");
                        continue;
                    }

                    // ロケット試験場
                    province.RocketTest = size;
                    continue;
                }

                // synthetic_oil
                if (keyword.Equals("synthetic_oil"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "synthetic_oil");
                        continue;
                    }

                    // 合成石油工場
                    province.SyntheticOil = size;
                    continue;
                }

                // synthetic_rares
                if (keyword.Equals("synthetic_rares"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "synthetic_rares");
                        continue;
                    }

                    // 合成素材工場
                    province.SyntheticRares = size;
                    continue;
                }

                // nuclear_power
                if (keyword.Equals("nuclear_power"))
                {
                    BuildingSize size = ParseSize(lexer);
                    if (size == null)
                    {
                        Log.InvalidSection(LogCategory, "nuclear_power");
                        continue;
                    }

                    // 原子力発電所
                    province.NuclearPower = size;
                    continue;
                }

                // supplypool
                if (keyword.Equals("supplypool"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplypool");
                        continue;
                    }

                    // 物資の備蓄量
                    province.SupplyPool = (double) d;
                    continue;
                }

                // oilpool
                if (keyword.Equals("oilpool"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oilpool");
                        continue;
                    }

                    // 石油の備蓄量
                    province.OilPool = (double) d;
                    continue;
                }

                // energypool
                if (keyword.Equals("energypool"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energypool");
                        continue;
                    }

                    // エネルギーの備蓄量
                    province.EnergyPool = (double) d;
                    continue;
                }

                // metalpool
                if (keyword.Equals("metalpool"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metalpool");
                        continue;
                    }

                    // 金属の備蓄量
                    province.MetalPool = (double) d;
                    continue;
                }

                // rarematerialspool
                if (keyword.Equals("rarematerialspool"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rarematerialspool");
                        continue;
                    }

                    // 希少資源の備蓄量
                    province.RareMaterialsPool = (double) d;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy");
                        continue;
                    }

                    // エネルギー産出量
                    province.Energy = (double) d;
                    continue;
                }

                // max_energy
                if (keyword.Equals("max_energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_energy");
                        continue;
                    }

                    // 最大エネルギー産出量
                    province.MaxEnergy = (double) d;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal");
                        continue;
                    }

                    // 金属産出量
                    province.Metal = (double) d;
                    continue;
                }

                // max_metal
                if (keyword.Equals("max_metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_metal");
                        continue;
                    }

                    // 最大金属産出量
                    province.MaxMetal = (double) d;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 希少資源産出量
                    province.RareMaterials = (double) d;
                    continue;
                }

                // max_rare_materials
                if (keyword.Equals("max_rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_rare_materials");
                        continue;
                    }

                    // 最大希少資源産出量
                    province.MaxRareMaterials = (double) d;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil");
                        continue;
                    }

                    // 石油産出量
                    province.Oil = (double) d;
                    continue;
                }

                // max_oil
                if (keyword.Equals("max_oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_oil");
                        continue;
                    }

                    // 最大石油産出量
                    province.MaxOil = (double) d;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 人的資源
                    province.Manpower = (double) d;
                    continue;
                }

                // max_manpower
                if (keyword.Equals("max_manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_manpower");
                        continue;
                    }

                    // 最大人的資源
                    province.MaxManpower = (double) d;
                    continue;
                }

                // points
                if (keyword.Equals("points"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "points");
                        continue;
                    }

                    // 勝利ポイント
                    province.Vp = (int) n;
                    continue;
                }

                // province_revoltrisk
                if (keyword.Equals("province_revoltrisk"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "province_revoltrisk");
                        continue;
                    }

                    // 反乱率
                    province.RevoltRisk = (double) d;
                    continue;
                }

                // weather
                if (keyword.Equals("weather"))
                {
                    string s = ParseIdentifier(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (!Scenarios.WeatherStrings.Contains(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 天候
                    province.Weather = (WeatherType) Array.IndexOf(Scenarios.WeatherStrings, s);
                    continue;
                }

                if (Game.Type == GameType.DarkestHour)
                {
                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name");
                            continue;
                        }

                        // プロヴィンス名
                        province.Name = s;
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return province;
        }

        #endregion

        #region 建物

        /// <summary>
        ///     建物のサイズを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>建物のサイズ</returns>
        private static BuildingSize ParseSize(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            // 相対サイズ指定
            token = lexer.GetToken();
            if (token.Type == TokenType.Number)
            {
                return new BuildingSize {Size = (double) token.Value};
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            var size = new BuildingSize();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // size
                if (keyword.Equals("size"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size");
                        continue;
                    }

                    if ((double) d < 0)
                    {
                        Log.OutOfRange(LogCategory, "size", d, lexer);
                        continue;
                    }

                    // 最大サイズ
                    size.MaxSize = (double) d;
                    continue;
                }

                // current_size
                if (keyword.Equals("current_size"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "current_size");
                        continue;
                    }

                    if (d < 0)
                    {
                        Log.OutOfRange(LogCategory, "current_size", d, lexer);
                        continue;
                    }

                    // 現在サイズ
                    size.CurrentSize = (double) d;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return size;
        }

        /// <summary>
        ///     生産中建物を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中建物</returns>
        private static BuildingDevelopment ParseBuildingDevelopment(TextLexer lexer)
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

            var building = new BuildingDevelopment();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    building.Id = id;
                    continue;
                }

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
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (!Scenarios.BuildingStrings.Contains(s))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 建物の種類
                    building.Type = (BuildingType) Array.IndexOf(Scenarios.BuildingStrings, s);
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    building.Location = (int) n;
                    continue;
                }

                // cost
                if (keyword.Equals("cost"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "cost");
                        continue;
                    }

                    // 必要IC
                    building.Cost = (double) d;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 必要人的資源
                    building.Manpower = (double) d;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 完了予定日
                    building.Date = date;
                    continue;
                }

                // progress
                if (keyword.Equals("progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "progress");
                        continue;
                    }

                    // 進捗率増分
                    building.Progress = (double) d;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "total_progress");
                        continue;
                    }

                    // 総進捗率
                    building.TotalProgress = (double) d;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "gearing_bonus");
                        continue;
                    }

                    // 連続生産ボーナス
                    building.GearingBonus = (double) d;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size");
                        continue;
                    }

                    // 総生産数
                    building.Size = (int) n;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "done");
                        continue;
                    }

                    // 生産完了数
                    building.Done = (int) n;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days");
                        continue;
                    }

                    // 完了日数
                    building.Days = (int) n;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days");
                        continue;
                    }

                    // 1単位の完了日数
                    building.DaysForFirst = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return building;
        }

        #endregion

        #region 外交

        /// <summary>
        ///     同盟国設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>同盟国設定</returns>
        private static Alliance ParseAlliance(TextLexer lexer)
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

            var alliance = new Alliance();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    alliance.Id = id;
                    continue;
                }

                // participant
                if (keyword.Equals("participant"))
                {
                    IEnumerable<Country> list = ParseCountryList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "participant");
                        continue;
                    }

                    // 参加国
                    alliance.Participant.AddRange(list);
                    continue;
                }

                if (Game.Type == GameType.DarkestHour)
                {
                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseString(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name");
                            continue;
                        }

                        // 同盟名
                        alliance.Name = s;
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return alliance;
        }

        /// <summary>
        ///     戦争設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>戦争設定</returns>
        private static War ParseWar(TextLexer lexer)
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

            var war = new War();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    war.Id = id;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 開始日時
                    war.StartDate = date;
                    continue;
                }

                // enddate
                if (keyword.Equals("enddate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "enddate");
                        continue;
                    }

                    // 終了日時
                    war.EndDate = date;
                    continue;
                }

                // attackers
                if (keyword.Equals("attackers"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "attackers");
                        continue;
                    }

                    // 攻撃側参加国
                    war.Attackers = alliance;
                    continue;
                }

                // defenders
                if (keyword.Equals("defenders"))
                {
                    Alliance alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.InvalidSection(LogCategory, "defenders");
                        continue;
                    }

                    // 防御側参加国
                    war.Defenders = alliance;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return war;
        }

        /// <summary>
        ///     外交協定設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交協定設定</returns>
        private static Treaty ParseTreaty(TextLexer lexer)
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

            var treaty = new Treaty();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    treaty.Id = id;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    string s = ParseIdentifier(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }
                    s = s.ToLower();

                    // non_aggression
                    if (s.Equals("non_aggression"))
                    {
                        treaty.Type = TreatyType.NonAggression;
                        continue;
                    }

                    // peace
                    if (s.Equals("peace"))
                    {
                        treaty.Type = TreatyType.Peace;
                        continue;
                    }

                    // trade
                    if (s.Equals("trade"))
                    {
                        treaty.Type = TreatyType.Trade;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // country
                if (keyword.Equals("country"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "country");
                        continue;
                    }

                    // 対象国
                    if (treaty.Country1 == Country.None)
                    {
                        treaty.Country1 = (Country) tag;
                    }
                    else if (treaty.Country2 == Country.None)
                    {
                        treaty.Country2 = (Country) tag;
                    }
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    treaty.StartDate = date;
                    continue;
                }

                // expirydate
                if (keyword.Equals("expirydate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "expirydate");
                        continue;
                    }

                    // 失効日時
                    treaty.ExpiryDate = date;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "money");
                        continue;
                    }

                    // 資金
                    treaty.Money = (double) d;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplies");
                        continue;
                    }

                    // 物資
                    treaty.Supplies = (double) d;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy");
                        continue;
                    }

                    // エネルギー
                    treaty.Energy = (double) d;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal");
                        continue;
                    }

                    // 金属
                    treaty.Metal = (double) d;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 希少資源
                    treaty.RareMaterials = (double) d;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil");
                        continue;
                    }

                    // 石油
                    treaty.Oil = (double) d;
                    continue;
                }

                // cancel
                if (keyword.Equals("cancel"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "cancel");
                        continue;
                    }

                    // 取り消し可能かどうか
                    treaty.Cancel = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return treaty;
        }

        /// <summary>
        ///     外交設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交設定</returns>
        private static IEnumerable<RelationSettings> ParseDiplomacy(TextLexer lexer)
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

            var list = new List<RelationSettings>();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // relation
                if (keyword.Equals("relation"))
                {
                    RelationSettings relation = ParseRelation(lexer);
                    if (relation == null)
                    {
                        Log.InvalidSection(LogCategory, "relation");
                        continue;
                    }

                    // 外交関係設定
                    list.Add(relation);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return list;
        }

        /// <summary>
        ///     外交関係設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交関係情報</returns>
        private static RelationSettings ParseRelation(TextLexer lexer)
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

            var relation = new RelationSettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // tag
                if (keyword.Equals("tag"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tag");
                        continue;
                    }

                    // 国タグ
                    relation.Country = (Country) tag;
                    continue;
                }

                // value
                if (keyword.Equals("value"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "value");
                        continue;
                    }

                    // 関係値
                    relation.Value = (double) d;
                    continue;
                }

                // access
                if (keyword.Equals("access"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "access");
                        continue;
                    }

                    // 通行許可
                    relation.Access = (bool) b;
                    continue;
                }

                // guaranteed
                if (keyword.Equals("guaranteed"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "guaranteed");
                        continue;
                    }

                    // 独立保障期限
                    relation.Guaranteed = date;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return relation;
        }

        #endregion

        #region 国家

        /// <summary>
        ///     国家設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国家設定</returns>
        private static CountrySettings ParseCountry(TextLexer lexer)
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

            var country = new CountrySettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // tag
                if (keyword.Equals("tag"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "tag");
                        continue;
                    }

                    // 国タグ
                    country.Country = (Country) tag;
                    continue;
                }

                // regular_id
                if (keyword.Equals("regular_id"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "regular_id");
                        continue;
                    }

                    // 兄弟国
                    country.RegularId = (Country) tag;
                    continue;
                }

                // intrinsic_gov_type
                if (keyword.Equals("intrinsic_gov_type"))
                {
                    string s = ParseIdentifier(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (!Scenarios.GovernmentStrings.Contains(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 独立可能政体
                    country.IntrinsicGovType = (GovernmentType) Array.IndexOf(Scenarios.GovernmentStrings, s);
                    continue;
                }

                // puppet
                if (keyword.Equals("puppet"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "puppet");
                        continue;
                    }

                    // 宗主国
                    country.Master = (Country) tag;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "control");
                        continue;
                    }

                    // 統帥権取得国
                    country.Control = (Country) tag;
                    continue;
                }

                // belligerence
                if (keyword.Equals("belligerence"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "belligerence");
                        continue;
                    }

                    // 好戦性
                    country.Belligerence = (int) n;
                    continue;
                }

                // extra_tc
                if (keyword.Equals("extra_tc"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "extra_tc");
                        continue;
                    }

                    // 追加輸送能力
                    country.ExtraTc = (double) d;
                    continue;
                }

                // dissent
                if (keyword.Equals("dissent"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dissent");
                        continue;
                    }

                    // 国民不満度
                    country.Dissent = (double) d;
                    continue;
                }

                // capital
                if (keyword.Equals("capital"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "capital");
                        continue;
                    }

                    // 首都のプロヴィンスID
                    country.Capital = (int) (double) n;
                    continue;
                }

                // peacetime_ic_mod
                if (keyword.Equals("peacetime_ic_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "peacetime_ic_mod");
                        continue;
                    }

                    // 平時IC補正
                    country.PeacetimeIcMod = (double) d;
                    continue;
                }

                // wartime_ic_mod
                if (keyword.Equals("wartime_ic_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "wartime_ic_mod");
                        continue;
                    }

                    // 戦時IC補正
                    country.WartimeIcMod = (double) d;
                    continue;
                }

                // industrial_modifier
                if (keyword.Equals("industrial_modifier"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "industrial_modifier");
                        continue;
                    }

                    // 工業力補正
                    country.IndustrialModifier = (double) d;
                    continue;
                }

                // ground_def_eff
                if (keyword.Equals("ground_def_eff"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "ground_def_eff");
                        continue;
                    }

                    // 対地防御補正
                    country.GroundDefEff = (double) d;
                    continue;
                }

                // ai
                if (keyword.Equals("ai"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "ai");
                        continue;
                    }

                    // AIファイル名
                    country.Ai = s;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 人的資源
                    country.Manpower = (double) d;
                    continue;
                }

                // relative_manpower
                if (keyword.Equals("relative_manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "relative_manpower");
                        continue;
                    }

                    // 人的資源補正値
                    country.RelativeManpower = (double) d;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy");
                        continue;
                    }

                    // エネルギー
                    country.Energy = (double) d;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal");
                        continue;
                    }

                    // 金属
                    country.Metal = (double) d;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 希少資源
                    country.RareMaterials = (double) d;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil");
                        continue;
                    }

                    // 石油
                    country.Oil = (double) d;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplies");
                        continue;
                    }

                    // 物資
                    country.Supplies = (double) d;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "money");
                        continue;
                    }

                    // 資金
                    country.Money = (double) d;
                    continue;
                }

                // transports
                if (keyword.Equals("transports"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transports");
                        continue;
                    }

                    // 輸送船団
                    country.Transports = (int) n;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "escorts");
                        continue;
                    }

                    // 護衛艦
                    country.Escorts = (int) n;
                    continue;
                }

                // nuke
                if (keyword.Equals("nuke"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "nuke");
                        continue;
                    }

                    // 核兵器
                    country.Nuke = (int) n;
                    continue;
                }

                // free
                if (keyword.Equals("free"))
                {
                    ResourceSettings free = ParseFree(lexer);
                    if (free == null)
                    {
                        Log.InvalidSection(LogCategory, "free");
                        continue;
                    }

                    // マップ外資源
                    country.Free = free;
                    continue;
                }

                // consumer
                if (keyword.Equals("consumer"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "consumer");
                        continue;
                    }

                    // 消費財IC比率
                    country.Consumer = (double) d;
                    continue;
                }

                // supply
                if (keyword.Equals("supply"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supply");
                        continue;
                    }

                    // 物資IC比率
                    country.Consumer = (double) d;
                    continue;
                }

                // production
                if (keyword.Equals("production"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "production");
                        continue;
                    }

                    // 生産IC比率
                    country.Consumer = (double) d;
                    continue;
                }

                // reinforcement
                if (keyword.Equals("reinforcement"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "reinforcement");
                        continue;
                    }

                    // 補充IC比率
                    country.Consumer = (double) d;
                    continue;
                }

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    IEnumerable<RelationSettings> list = ParseDiplomacy(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "diplomacy");
                        continue;
                    }

                    // 外交設定
                    country.Diplomacy.AddRange(list);
                    continue;
                }

                // spyinfo
                if (keyword.Equals("spyinfo"))
                {
                    SpySettings spy = ParseSpyInfo(lexer);
                    if (spy == null)
                    {
                        Log.InvalidSection(LogCategory, "spyinfo");
                        continue;
                    }

                    // 諜報設定
                    country.Intelligence.Add(spy);
                    continue;
                }

                // nationalprovinces
                if (keyword.Equals("nationalprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "nationalprovinces");
                        continue;
                    }

                    // 中核プロヴィンス
                    country.NationalProvinces.AddRange(list);
                    continue;
                }

                // ownedprovinces
                if (keyword.Equals("ownedprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "ownedprovinces");
                        continue;
                    }

                    // 保有プロヴィンス
                    country.OwnedProvinces.AddRange(list);
                    continue;
                }

                // controlledprovinces
                if (keyword.Equals("controlledprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "controlledprovinces");
                        continue;
                    }

                    // 支配プロヴィンス
                    country.ControlledProvinces.AddRange(list);
                    continue;
                }

                // techapps
                if (keyword.Equals("techapps"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "techapps");
                        continue;
                    }

                    // 保有技術
                    country.TechApps.AddRange(list);
                    continue;
                }

                // blueprints
                if (keyword.Equals("blueprints"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "blueprints");
                        continue;
                    }

                    // 青写真
                    country.BluePrints.AddRange(list);
                    continue;
                }

                // inventions
                if (keyword.Equals("inventions"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "inventions");
                        continue;
                    }

                    // 発明イベント
                    country.Inventions.AddRange(list);
                    continue;
                }

                // policy
                if (keyword.Equals("policy"))
                {
                    CountryPolicy policy = ParsePolicy(lexer);
                    if (policy == null)
                    {
                        Log.InvalidSection(LogCategory, "policy");
                        continue;
                    }

                    // 政策スライダー
                    country.Policy = policy;
                    continue;
                }

                // nukedate
                if (keyword.Equals("nukedate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "nukedate");
                        continue;
                    }

                    // 核兵器完成日時
                    country.NukeDate = date;
                    continue;
                }

                // headofstate
                if (keyword.Equals("headofstate"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "headofstate");
                        continue;
                    }

                    // 国家元首
                    country.HeadOfState = id;
                    continue;
                }

                // headofgovernment
                if (keyword.Equals("headofgovernment"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "headofgovernment");
                        continue;
                    }

                    // 政府首班
                    country.HeadOfGovernment = id;
                    continue;
                }

                // foreignminister
                if (keyword.Equals("foreignminister"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "foreignminister");
                        continue;
                    }

                    // 外務大臣
                    country.ForeignMinister = id;
                    continue;
                }

                // armamentminister
                if (keyword.Equals("armamentminister"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "armamentminister");
                        continue;
                    }

                    // 軍需大臣
                    country.ArmamentMinister = id;
                    continue;
                }

                // ministerofsecurity
                if (keyword.Equals("ministerofsecurity"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "ministerofsecurity");
                        continue;
                    }

                    // 内務大臣
                    country.MinisterOfSecurity = id;
                    continue;
                }

                // ministerofintelligence
                if (keyword.Equals("ministerofintelligence"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "ministerofintelligence");
                        continue;
                    }

                    // 情報大臣
                    country.MinisterOfIntelligence = id;
                    continue;
                }

                // chiefofstaff
                if (keyword.Equals("chiefofstaff"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofstaff");
                        continue;
                    }

                    // 統合参謀総長
                    country.ChiefOfStaff = id;
                    continue;
                }

                // chiefofarmy
                if (keyword.Equals("chiefofarmy"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofarmy");
                        continue;
                    }

                    // 陸軍総司令官
                    country.ChiefOfArmy = id;
                    continue;
                }

                // chiefofnavy
                if (keyword.Equals("chiefofnavy"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofnavy");
                        continue;
                    }

                    // 海軍総司令官
                    country.ChiefOfNavy = id;
                    continue;
                }

                // chiefofair
                if (keyword.Equals("chiefofair"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofair");
                        continue;
                    }

                    // 空軍総司令官
                    country.ChiefOfAir = id;
                    continue;
                }

                // dormant_leaders
                if (keyword.Equals("dormant_leaders"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_leaders");
                        continue;
                    }

                    // 休止指揮官
                    country.DormantLeaders.AddRange(list);
                    continue;
                }

                // dormant_ministers
                if (keyword.Equals("dormant_ministers"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_ministers");
                        continue;
                    }

                    // 休止閣僚
                    country.DormantMinisters.AddRange(list);
                    continue;
                }

                // dormant_teams
                if (keyword.Equals("dormant_teams"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_teams");
                        continue;
                    }

                    // 休止研究機関
                    country.DormantTeams.AddRange(list);
                    continue;
                }

                // steal_leader
                if (keyword.Equals("steal_leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "steal_leader");
                        continue;
                    }

                    // 抽出指揮官
                    country.StealLeaders.Add((int) n);
                    continue;
                }

                // convoy
                if (keyword.Equals("convoy"))
                {
                    Convoy convoy = ParseConvoy(lexer);
                    if (convoy == null)
                    {
                        Log.InvalidSection(LogCategory, "convoy");
                        continue;
                    }

                    // 輸送船団
                    country.Convoys.Add(convoy);
                    continue;
                }

                // landunit
                if (keyword.Equals("landunit"))
                {
                    LandUnit unit = ParseLandUnit(lexer);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "landunit");
                        continue;
                    }

                    // 陸軍ユニット
                    country.LandUnits.Add(unit);
                    continue;
                }

                // navalunit
                if (keyword.Equals("navalunit"))
                {
                    NavalUnit unit = ParseNavalUnit(lexer);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "navalunit");
                        continue;
                    }

                    // 海軍ユニット
                    country.NavalUnits.Add(unit);
                    continue;
                }

                // airunit
                if (keyword.Equals("airunit"))
                {
                    AirUnit unit = ParseAirUnit(lexer);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "airunit");
                        continue;
                    }

                    // 空軍ユニット
                    country.AirUnits.Add(unit);
                    continue;
                }

                // division_development
                if (keyword.Equals("division_development"))
                {
                    DivisionDevelopment division = ParseDivisionDevelopment(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division_development");
                        continue;
                    }

                    // 生産中師団
                    country.DivisionDevelopments.Add(division);
                    continue;
                }

                // convoy_development
                if (keyword.Equals("convoy_development"))
                {
                    ConvoyDevelopment convoy = ParseConvoyDevelopment(lexer);
                    if (convoy == null)
                    {
                        Log.InvalidSection(LogCategory, "convoy_development");
                        continue;
                    }

                    // 生産中輸送船団
                    country.ConvoyDevelopments.Add(convoy);
                    continue;
                }

                // province_development
                if (keyword.Equals("province_development"))
                {
                    BuildingDevelopment building = ParseBuildingDevelopment(lexer);
                    if (building == null)
                    {
                        Log.InvalidSection(LogCategory, "province_development");
                        continue;
                    }

                    // 生産中建物
                    country.BuildingDevelopments.Add(building);
                    continue;
                }

                // landdivision
                if (keyword.Equals("landdivision"))
                {
                    LandDivision division = ParseLandDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "landdivision");
                        continue;
                    }

                    // 陸軍師団
                    country.LandDivisions.Add(division);
                    continue;
                }

                // navaldivision
                if (keyword.Equals("navaldivision"))
                {
                    NavalDivision division = ParseNavalDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "navaldivision");
                        continue;
                    }

                    // 海軍師団
                    country.NavalDivisions.Add(division);
                    continue;
                }

                // airdivision
                if (keyword.Equals("airdivision"))
                {
                    AirDivision division = ParseAirDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "airdivision");
                        continue;
                    }

                    // 空軍師団
                    country.AirDivisions.Add(division);
                    continue;
                }

                if (Game.Type == GameType.DarkestHour)
                {
                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name");
                            continue;
                        }

                        // 国名
                        country.Name = s;
                        continue;
                    }

                    // flag_ext
                    if (keyword.Equals("flag_ext"))
                    {
                        string s = ParseIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "flag_ext");
                            continue;
                        }

                        // 国旗の接尾辞
                        country.FlagExt = s;
                        continue;
                    }

                    // ai_settings
                    if (keyword.Equals("ai_settings"))
                    {
                        AiSettings settings = ParseAiSettings(lexer);
                        if (settings == null)
                        {
                            Log.InvalidSection(LogCategory, "ai_settings");
                            continue;
                        }

                        // AI設定
                        country.AiSettings = settings;
                        continue;
                    }

                    // claimedprovinces
                    if (keyword.Equals("claimedprovinces"))
                    {
                        IEnumerable<int> list = ParseIdList(lexer);
                        if (list == null)
                        {
                            Log.InvalidSection(LogCategory, "claimedprovinces");
                            continue;
                        }

                        // 領有権主張プロヴィンス
                        country.ClaimedProvinces.AddRange(list);
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return country;
        }

        /// <summary>
        ///     AI設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>AI設定</returns>
        private static AiSettings ParseAiSettings(TextLexer lexer)
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

            var settings = new AiSettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // flags
                if (keyword.Equals("flags"))
                {
                    Dictionary<string, string> flags = ParseAiFlags(lexer);
                    if (flags == null)
                    {
                        Log.InvalidSection(LogCategory, "flags");
                        continue;
                    }

                    // AIフラグ
                    settings.Flags = flags;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return settings;
        }

        /// <summary>
        ///     AIフラグを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>AIフラグ</returns>
        private static Dictionary<string, string> ParseAiFlags(TextLexer lexer)
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

            var flags = new Dictionary<string, string>();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }

                // =
                token = lexer.GetToken();
                if (token.Type != TokenType.Equal)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                token = lexer.GetToken();
                if (token.Type == TokenType.Number)
                {
                    var n = (int) (double) token.Value;
                    if (n == 0 || n == 1)
                    {
                        flags.Add(keyword, ObjectHelper.ToString(token.Value));
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                if (token.Type == TokenType.Identifier)
                {
                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes") || s.Equals("no"))
                    {
                        flags.Add(keyword, ObjectHelper.ToString(token.Value));
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return flags;
        }

        /// <summary>
        ///     資源設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>資源設定</returns>
        private static ResourceSettings ParseFree(TextLexer lexer)
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

            var free = new ResourceSettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // ic
                if (keyword.Equals("ic"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "ic");
                        continue;
                    }

                    // 工業力
                    free.Ic = (double) d;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 人的資源
                    free.Manpower = (double) d;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy");
                        continue;
                    }

                    // エネルギー
                    free.Energy = (double) d;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal");
                        continue;
                    }

                    // 金属
                    free.Metal = (double) d;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 希少資源
                    free.RareMaterials = (double) d;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil");
                        continue;
                    }

                    // 石油
                    free.Oil = (double) d;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplies");
                        continue;
                    }

                    // 物資
                    free.Supplies = (double) d;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "money");
                        continue;
                    }

                    // 資金
                    free.Money = (double) d;
                    continue;
                }

                // transport
                if (keyword.Equals("transport"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transport");
                        continue;
                    }

                    // 輸送船団
                    free.Transports = (int) n;
                    continue;
                }

                // escort
                if (keyword.Equals("escort"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "escort");
                        continue;
                    }

                    // 護衛艦
                    free.Escorts = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return free;
        }

        /// <summary>
        ///     諜報設定を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>諜報設定</returns>
        private static SpySettings ParseSpyInfo(TextLexer lexer)
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

            var spyInfo = new SpySettings();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // country
                if (keyword.Equals("country"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "country");
                        continue;
                    }

                    // 国タグ
                    spyInfo.Country = (Country) tag;
                    continue;
                }

                // numberofspies
                if (keyword.Equals("numberofspies"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "numberofspies");
                        continue;
                    }

                    // スパイの数
                    spyInfo.Spies = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return spyInfo;
        }

        /// <summary>
        ///     政策スライダーを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>政策スライダー</returns>
        private static CountryPolicy ParsePolicy(TextLexer lexer)
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

            var policy = new CountryPolicy();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // スライダー移動可能日時
                    policy.Date = date;
                    continue;
                }

                // democratic
                if (keyword.Equals("democratic"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "democratic");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "democratic", n, lexer);
                        continue;
                    }

                    // 民主的 - 独裁的
                    policy.Democratic = (int) n;
                    continue;
                }

                // political_left
                if (keyword.Equals("political_left"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "political_left");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "political_left", n, lexer);
                        continue;
                    }

                    // 政治的左派 - 政治的右派
                    policy.PoliticalLeft = (int) n;
                    continue;
                }

                // freedom
                if (keyword.Equals("freedom"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "freedom");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "freedom", n, lexer);
                        continue;
                    }

                    // 開放社会 - 閉鎖社会
                    policy.Freedom = (int) n;
                    continue;
                }

                // free_market
                if (keyword.Equals("free_market"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "freedom");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "freedom", n, lexer);
                        continue;
                    }

                    // 自由経済 - 中央計画経済
                    policy.FreeMarket = (int) n;
                    continue;
                }

                // professional_army
                if (keyword.Equals("professional_army"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "professional_army");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "professional_army", n, lexer);
                        continue;
                    }

                    // 常備軍 - 徴兵軍 (DH Fullでは動員 - 復員)
                    policy.ProfessionalArmy = (int) n;
                    continue;
                }

                // defense_lobby
                if (keyword.Equals("defense_lobby"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "defense_lobby");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "defense_lobby", n, lexer);
                        continue;
                    }

                    // タカ派 - ハト派
                    policy.DefenseLobby = (int) n;
                    continue;
                }

                // interventionism
                if (keyword.Equals("interventionism"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "interventionism");
                        continue;
                    }

                    if ((int) n < 1 || (int) n > 10)
                    {
                        Log.OutOfRange(LogCategory, "interventionism", n, lexer);
                        continue;
                    }

                    // 介入主義 - 孤立主義
                    policy.Interventionism = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return policy;
        }

        #endregion

        #region ユニット

        /// <summary>
        ///     陸軍ユニットを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>陸軍ユニット</returns>
        private static LandUnit ParseLandUnit(TextLexer lexer)
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

            var unit = new LandUnit();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    unit.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // ユニット名
                    unit.Name = s;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "control");
                        continue;
                    }

                    // 統帥国
                    unit.Control = (Country) tag;
                    continue;
                }

                // leader
                if (keyword.Equals("leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "leader");
                        continue;
                    }

                    // 指揮官
                    unit.Leader = (int) n;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 現在位置
                    unit.Location = (int) n;
                    continue;
                }

                // prevprov
                if (keyword.Equals("prevprov"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "prevprov");
                        continue;
                    }

                    // 直前の位置
                    unit.PrevProv = (int) n;
                    continue;
                }

                // home
                if (keyword.Equals("home"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "home");
                        continue;
                    }

                    // 基準位置
                    unit.Home = (int) n;
                    continue;
                }

                // dig_in
                if (keyword.Equals("dig_in"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dig_in");
                        continue;
                    }

                    // 塹壕レベル
                    unit.DigIn = (double) d;
                    continue;
                }

                // mission
                if (keyword.Equals("mission"))
                {
                    LandMission mission = ParseLandMission(lexer);
                    if (mission == null)
                    {
                        Log.InvalidSection(LogCategory, "mission");
                        continue;
                    }

                    // 任務
                    unit.Mission = mission;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 指定日時
                    unit.Date = date;
                    continue;
                }

                // movetime
                if (keyword.Equals("movetime"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "movetime");
                        continue;
                    }

                    // 移動完了日時
                    unit.MoveTime = date;
                    continue;
                }

                // movement
                if (keyword.Equals("movement"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "movement");
                        continue;
                    }

                    // 移動経路
                    unit.Movement.AddRange(list);
                    continue;
                }

                // division
                if (keyword.Equals("division"))
                {
                    LandDivision division = ParseLandDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division");
                        continue;
                    }

                    // 師団
                    unit.Divisions.Add(division);
                    continue;
                }

                // invasion
                if (keyword.Equals("invasion"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "invasion");
                        continue;
                    }

                    // 上陸中
                    unit.Invasion = (bool) b;
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target");
                        continue;
                    }

                    // 上陸先
                    unit.Target = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return unit;
        }

        /// <summary>
        ///     海軍ユニットを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>海軍ユニット</returns>
        private static NavalUnit ParseNavalUnit(TextLexer lexer)
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

            var unit = new NavalUnit();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    unit.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // ユニット名
                    unit.Name = s;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "control");
                        continue;
                    }

                    // 統帥国
                    unit.Control = (Country) tag;
                    continue;
                }

                // leader
                if (keyword.Equals("leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "leader");
                        continue;
                    }

                    // 指揮官
                    unit.Leader = (int) n;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 現在位置
                    unit.Location = (int) n;
                    continue;
                }

                // prevprov
                if (keyword.Equals("prevprov"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "prevprov");
                        continue;
                    }

                    // 直前の位置
                    unit.PrevProv = (int) n;
                    continue;
                }

                // home
                if (keyword.Equals("home"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "home");
                        continue;
                    }

                    // 基準位置
                    unit.Home = (int) n;
                    continue;
                }

                // base
                if (keyword.Equals("base"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "base");
                        continue;
                    }

                    // 所属基地
                    unit.Base = (int) n;
                    continue;
                }

                // mission
                if (keyword.Equals("mission"))
                {
                    NavalMission mission = ParseNavalMission(lexer);
                    if (mission == null)
                    {
                        Log.InvalidSection(LogCategory, "mission");
                        continue;
                    }

                    // 任務
                    unit.Mission = mission;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 指定日時
                    unit.Date = date;
                    continue;
                }

                // movetime
                if (keyword.Equals("movetime"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "movetime");
                        continue;
                    }

                    // 移動完了日時
                    unit.MoveTime = date;
                    continue;
                }

                // movement
                if (keyword.Equals("movement"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "movement");
                        continue;
                    }

                    // 移動経路
                    unit.Movement.AddRange(list);
                    continue;
                }

                // division
                if (keyword.Equals("division"))
                {
                    NavalDivision division = ParseNavalDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division");
                        continue;
                    }

                    // 師団
                    unit.Divisions.Add(division);
                    continue;
                }

                // landunit
                if (keyword.Equals("landunit"))
                {
                    LandUnit landUnit = ParseLandUnit(lexer);
                    if (landUnit == null)
                    {
                        Log.InvalidSection(LogCategory, "landunit");
                        continue;
                    }

                    // 搭載ユニット
                    unit.LandUnits.Add(landUnit);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return unit;
        }

        /// <summary>
        ///     空軍ユニットを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>空軍ユニット</returns>
        private static AirUnit ParseAirUnit(TextLexer lexer)
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

            var unit = new AirUnit();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    unit.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // ユニット名
                    unit.Name = s;
                    continue;
                }

                // leader
                if (keyword.Equals("leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "leader");
                        continue;
                    }

                    // 指揮官
                    unit.Leader = (int) n;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "control");
                        continue;
                    }

                    // 統帥国
                    unit.Control = (Country) tag;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    unit.Location = (int) n;
                    continue;
                }

                // prevprov
                if (keyword.Equals("prevprov"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "prevprov");
                        continue;
                    }

                    // 直前の位置
                    unit.PrevProv = (int) n;
                    continue;
                }

                // home
                if (keyword.Equals("home"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "home");
                        continue;
                    }

                    // 基準位置
                    unit.Home = (int) n;
                    continue;
                }

                // base
                if (keyword.Equals("base"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "base");
                        continue;
                    }

                    // 所属基地
                    unit.Base = (int) n;
                    continue;
                }

                // mission
                if (keyword.Equals("mission"))
                {
                    AirMission mission = ParseAirMission(lexer);
                    if (mission == null)
                    {
                        Log.InvalidSection(LogCategory, "mission");
                        continue;
                    }

                    // 任務
                    unit.Mission = mission;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 指定日時
                    unit.Date = date;
                    continue;
                }

                // movetime
                if (keyword.Equals("movetime"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "movetime");
                        continue;
                    }

                    // 移動完了日時
                    unit.MoveTime = date;
                    continue;
                }

                // movement
                if (keyword.Equals("movement"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "movement");
                        continue;
                    }

                    // 移動経路
                    unit.Movement.AddRange(list);
                    continue;
                }

                // division
                if (keyword.Equals("division"))
                {
                    AirDivision division = ParseAirDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division");
                        continue;
                    }

                    // 師団
                    unit.Divisions.Add(division);
                    continue;
                }

                // landunit
                if (keyword.Equals("landunit"))
                {
                    LandUnit landUnit = ParseLandUnit(lexer);
                    if (landUnit == null)
                    {
                        Log.InvalidSection(LogCategory, "landunit");
                        continue;
                    }

                    // 搭載ユニット
                    unit.LandUnits.Add(landUnit);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return unit;
        }

        #endregion

        #region 師団

        /// <summary>
        ///     陸軍師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>陸軍師団</returns>
        private static LandDivision ParseLandDivision(TextLexer lexer)
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

            var division = new LandDivision();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // 師団名
                    division.Name = s;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType type = ParseDivisionType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }

                    // ユニット種類
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model");
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra1");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = type;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra2");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = type;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra3");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = type;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra4");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = type;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra5");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) n;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_strength");
                        continue;
                    }

                    // 最大戦力
                    division.MaxStrength = (int) n;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "strength");
                        continue;
                    }

                    // 戦力
                    division.Strength = (int) n;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "organisation");
                        continue;
                    }

                    // 指揮統制率
                    division.Organisation = (int) n;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "morale");
                        continue;
                    }

                    // 士気
                    division.Morale = (int) n;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "experience");
                        continue;
                    }

                    // 経験値
                    division.Experience = (int) n;
                    continue;
                }

                // offensive
                if (keyword.Equals("offensive"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "offensive");
                        continue;
                    }

                    // 攻勢開始日時
                    division.Offensive = date;
                    continue;
                }

                // dormant
                if (keyword.Equals("dormant"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant");
                        continue;
                    }

                    // 休止状態
                    division.Dormant = (bool) b;
                    continue;
                }

                // locked
                if (keyword.Equals("locked"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant");
                        continue;
                    }

                    // 移動不可
                    division.Locked = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

        /// <summary>
        ///     海軍師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>海軍師団</returns>
        private static NavalDivision ParseNavalDivision(TextLexer lexer)
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

            var division = new NavalDivision();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // 師団名
                    division.Name = s;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType type = ParseDivisionType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }

                    // ユニット種類
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model");
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;
                    continue;
                }

                // nuke
                if (keyword.Equals("nuke"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "nuke");
                        continue;
                    }

                    // 核兵器搭載
                    division.Nuke = (bool) b;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra1");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = type;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra2");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = type;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra3");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = type;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra4");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = type;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra5");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) n;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_strength");
                        continue;
                    }

                    // 最大戦力
                    division.MaxStrength = (int) n;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "strength");
                        continue;
                    }

                    // 戦力
                    division.Strength = (int) n;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "organisation");
                        continue;
                    }

                    // 指揮統制率
                    division.Organisation = (int) n;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "morale");
                        continue;
                    }

                    // 士気
                    division.Morale = (int) n;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "experience");
                        continue;
                    }

                    // 経験値
                    division.Experience = (int) n;
                    continue;
                }

                // maxspeed
                if (keyword.Equals("maxspeed"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "maxspeed");
                        continue;
                    }

                    // 移動速度
                    division.MaxSpeed = (double) d;
                    continue;
                }

                // seadefence
                if (keyword.Equals("seadefence"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "seadefence");
                        continue;
                    }

                    // 対艦/対潜防御力
                    division.SeaDefense = (double) d;
                    continue;
                }

                // airdefence
                if (keyword.Equals("airdefence"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airdefence");
                        continue;
                    }

                    // 対空防御力
                    division.AirDefence = (double) d;
                    continue;
                }

                // seaattack
                if (keyword.Equals("seaattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "seaattack");
                        continue;
                    }

                    // 対艦攻撃力(海軍)
                    division.SeaAttack = (double) d;
                    continue;
                }

                // subattack
                if (keyword.Equals("subattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "subattack");
                        continue;
                    }

                    // 対潜攻撃力
                    division.SubAttack = (double) d;
                    continue;
                }

                // convoyattack
                if (keyword.Equals("convoyattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "convoyattack");
                        continue;
                    }

                    // 通商破壊力
                    division.ConvoyAttack = (double) d;
                    continue;
                }

                // shorebombardment
                if (keyword.Equals("shorebombardment"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "shorebombardment");
                        continue;
                    }

                    // 湾岸攻撃力
                    division.ShoreBombardment = (double) d;
                    continue;
                }

                // airattack
                if (keyword.Equals("airattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airattack");
                        continue;
                    }

                    // 対空攻撃力
                    division.AirAttack = (double) d;
                    continue;
                }

                // distance
                if (keyword.Equals("distance"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "distance");
                        continue;
                    }

                    // 射程距離
                    division.Distance = (double) d;
                    continue;
                }

                // visibility
                if (keyword.Equals("visibility"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "visibility");
                        continue;
                    }

                    // 可視性
                    division.Visibility = (double) d;
                    continue;
                }

                // surfacedetectioncapability
                if (keyword.Equals("surfacedetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "surfacedetectioncapability");
                        continue;
                    }

                    // 対艦索敵能力
                    division.SurfaceDetectionCapability = (double) d;
                    continue;
                }

                // subdetectioncapability
                if (keyword.Equals("subdetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "subdetectioncapability");
                        continue;
                    }

                    // 対潜索敵能力
                    division.SubDetectionCapability = (double) d;
                    continue;
                }

                // airdetectioncapability
                if (keyword.Equals("airdetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airdetectioncapability");
                        continue;
                    }

                    // 対空索敵能力
                    division.AirDetectionCapability = (double) d;
                    continue;
                }

                // dormant
                if (keyword.Equals("dormant"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant");
                        continue;
                    }

                    // 休止状態
                    division.Dormant = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

        /// <summary>
        ///     空軍師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>空軍師団</returns>
        private static AirDivision ParseAirDivision(TextLexer lexer)
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

            var division = new AirDivision();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // 師団名
                    division.Name = s;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType type = ParseDivisionType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }

                    // ユニット種類
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model");
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;
                    continue;
                }

                // nuke
                if (keyword.Equals("nuke"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "nuke");
                        continue;
                    }

                    // 核兵器搭載
                    division.Nuke = (bool) b;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra1");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = type;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra2");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = type;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra3");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = type;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra4");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = type;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra5");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) n;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_strength");
                        continue;
                    }

                    // 最大戦力
                    division.MaxStrength = (int) n;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "strength");
                        continue;
                    }

                    // 戦力
                    division.Strength = (int) n;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "organisation");
                        continue;
                    }

                    // 指揮統制率
                    division.Organisation = (int) n;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "morale");
                        continue;
                    }

                    // 士気
                    division.Morale = (int) n;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "experience");
                        continue;
                    }

                    // 経験値
                    division.Experience = (int) n;
                    continue;
                }

                // dormant
                if (keyword.Equals("dormant"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant");
                        continue;
                    }

                    // 休止状態
                    division.Dormant = (bool) b;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

        /// <summary>
        ///     生産中師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中師団</returns>
        private static DivisionDevelopment ParseDivisionDevelopment(TextLexer lexer)
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

            var division = new DivisionDevelopment();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "name");
                        continue;
                    }

                    // 師団名
                    division.Name = s;
                    continue;
                }

                // cost
                if (keyword.Equals("cost"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "cost");
                        continue;
                    }

                    // 必要IC
                    division.Cost = (double) d;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 必要人的資源
                    division.Manpower = (double) d;
                    continue;
                }

                // unitcost
                if (keyword.Equals("unitcost"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "unitcost");
                        continue;
                    }

                    // unitcost
                    division.UnitCost = (bool) b;
                    continue;
                }

                // new_model
                if (keyword.Equals("new_model"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "unitcost");
                        continue;
                    }

                    // new_model
                    division.NewModel = (bool) b;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 完了予定日
                    division.Date = date;
                    continue;
                }

                // progress
                if (keyword.Equals("progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "progress");
                        continue;
                    }

                    // 進捗率増分
                    division.Progress = (double) d;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "total_progress");
                        continue;
                    }

                    // 総進捗率
                    division.TotalProgress = (double) d;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "gearing_bonus");
                        continue;
                    }

                    // 連続生産ボーナス
                    division.GearingBonus = (double) d;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size");
                        continue;
                    }

                    // 総生産数
                    division.Size = (int) n;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "done");
                        continue;
                    }

                    // 生産完了数
                    division.Done = (int) n;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days");
                        continue;
                    }

                    // 完了日数
                    division.Days = (int) n;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days_for_first");
                        continue;
                    }

                    // 1単位の完了日数
                    division.DaysForFirst = (int) n;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType type = ParseDivisionType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }

                    // ユニット種類
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model");
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra1");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = type;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra2");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = type;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra3");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = type;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra4");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = type;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType type = ParseBrigadeType(lexer);
                    if (type == UnitType.None)
                    {
                        Log.InvalidClause(LogCategory, "extra5");
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) n;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5");
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

        #endregion

        #region 任務

        /// <summary>
        ///     陸軍任務を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>陸軍任務</returns>
        private static LandMission ParseLandMission(TextLexer lexer)
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

            var mission = new LandMission();
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
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (!Scenarios.LandMissionStrings.Contains(s))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 任務の種類
                    mission.Type = (LandMissionType) Array.IndexOf(Scenarios.LandMissionStrings, s);
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target");
                        continue;
                    }

                    // 対象プロヴィンス
                    mission.Target = (int) n;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "percentage");
                        continue;
                    }

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) d;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "night");
                        continue;
                    }

                    // 夜間遂行
                    mission.Night = (bool) b;
                    continue;
                }

                // day
                if (keyword.Equals("day"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "day");
                        continue;
                    }

                    // 昼間遂行
                    mission.Day = (bool) b;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    mission.StartDate = date;
                    continue;
                }

                // task
                if (keyword.Equals("task"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "task");
                        continue;
                    }

                    // 任務
                    mission.Task = (int) n;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    mission.Location = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return mission;
        }

        /// <summary>
        ///     海軍任務を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>海軍任務</returns>
        private static NavalMission ParseNavalMission(TextLexer lexer)
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

            var mission = new NavalMission();
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
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (!Scenarios.NavalMissionStrings.Contains(s))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 任務の種類
                    mission.Type = (NavalMissionType) Array.IndexOf(Scenarios.NavalMissionStrings, s);
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target");
                        continue;
                    }

                    // 対象プロヴィンス
                    mission.Target = (int) n;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "percentage");
                        continue;
                    }

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) d;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "night");
                        continue;
                    }

                    // 夜間遂行
                    mission.Night = (bool) b;
                    continue;
                }

                // day
                if (keyword.Equals("day"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "day");
                        continue;
                    }

                    // 昼間遂行
                    mission.Day = (bool) b;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    mission.StartDate = date;
                    continue;
                }

                // enddate
                if (keyword.Equals("enddate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "enddate");
                        continue;
                    }

                    // 終了日時
                    mission.EndDate = date;
                    continue;
                }

                // task
                if (keyword.Equals("task"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "task");
                        continue;
                    }

                    // 任務
                    mission.Task = (int) n;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    mission.Location = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return mission;
        }

        /// <summary>
        ///     空軍任務を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>空軍任務</returns>
        private static AirMission ParseAirMission(TextLexer lexer)
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

            var mission = new AirMission();
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
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (!Scenarios.AirMissionStrings.Contains(s))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 任務の種類
                    mission.Type = (AirMissionType) Array.IndexOf(Scenarios.AirMissionStrings, s);
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target");
                        continue;
                    }

                    // 対象プロヴィンス
                    mission.Target = (int) n;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "percentage");
                        continue;
                    }

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) d;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "night");
                        continue;
                    }

                    // 夜間遂行
                    mission.Night = (bool) b;
                    continue;
                }

                // day
                if (keyword.Equals("day"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "day");
                        continue;
                    }

                    // 昼間遂行
                    mission.Day = (bool) b;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate");
                        continue;
                    }

                    // 開始日時
                    mission.StartDate = date;
                    continue;
                }

                // enddate
                if (keyword.Equals("enddate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "enddate");
                        continue;
                    }

                    // 終了日時
                    mission.EndDate = date;
                    continue;
                }

                // task
                if (keyword.Equals("task"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "task");
                        continue;
                    }

                    // 任務
                    mission.Task = (int) n;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    mission.Location = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return mission;
        }

        #endregion

        #region 輸送船団

        /// <summary>
        ///     輸送船団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>輸送船団</returns>
        private static Convoy ParseConvoy(TextLexer lexer)
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

            var convoy = new Convoy();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    convoy.Id = id;
                    continue;
                }

                // trade_id
                if (keyword.Equals("trade_id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "trade_id");
                        continue;
                    }

                    // 貿易ID
                    convoy.TradeId = id;
                    continue;
                }

                // istradeconvoy
                if (keyword.Equals("istradeconvoy"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "istradeconvoy");
                        continue;
                    }

                    // 貿易用の輸送船団かどうか
                    convoy.IsTrade = (bool) b;
                }

                // transports
                if (keyword.Equals("transports"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transports");
                        continue;
                    }

                    // 輸送船の数
                    convoy.Transports = (int) n;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "escorts");
                        continue;
                    }

                    // 護衛艦の数
                    convoy.Escorts = (int) n;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy");
                        continue;
                    }

                    // エネルギーの輸送有無
                    convoy.Energy = (bool) b;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal");
                        continue;
                    }

                    // 金属の輸送有無
                    convoy.Metal = (bool) b;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 希少資源の輸送有無
                    convoy.RareMaterials = (bool) b;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 石油の輸送有無
                    convoy.Oil = (bool) b;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials");
                        continue;
                    }

                    // 物資の輸送有無
                    convoy.Supplies = (bool) b;
                    continue;
                }

                // path
                if (keyword.Equals("path"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "list");
                        continue;
                    }

                    // 航路
                    convoy.Path.AddRange(list);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return convoy;
        }

        /// <summary>
        ///     生産中輸送船団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中輸送船団</returns>
        private static ConvoyDevelopment ParseConvoyDevelopment(TextLexer lexer)
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

            var convoy = new ConvoyDevelopment();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id");
                        continue;
                    }

                    // typeとidの組
                    convoy.Id = id;
                    continue;
                }

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

                    token = lexer.GetToken();
                    if (token.Type == TokenType.Identifier)
                    {
                        // 無効なトークン
                        var s = token.Value as string;
                        if (string.IsNullOrEmpty(s))
                        {
                            continue;
                        }
                        s = s.ToLower();

                        // transports
                        if (s.Equals("transports"))
                        {
                            // 輸送船
                            convoy.Type = ConvoyType.Transports;
                            continue;
                        }

                        if (s.Equals("escorts"))
                        {
                            // 護衛艦
                            convoy.Type = ConvoyType.Escorts;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location");
                        continue;
                    }

                    // 位置
                    convoy.Location = (int) n;
                    continue;
                }

                // cost
                if (keyword.Equals("cost"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "cost");
                        continue;
                    }

                    // 必要IC
                    convoy.Cost = (double) d;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower");
                        continue;
                    }

                    // 必要人的資源
                    convoy.Manpower = (double) d;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date");
                        continue;
                    }

                    // 完了予定日
                    convoy.Date = date;
                    continue;
                }

                // progress
                if (keyword.Equals("progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "progress");
                        continue;
                    }

                    // 進捗率増分
                    convoy.Progress = (double) d;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "total_progress");
                        continue;
                    }

                    // 総進捗率
                    convoy.TotalProgress = (double) d;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "gearing_bonus");
                        continue;
                    }

                    // 連続生産ボーナス
                    convoy.GearingBonus = (double) d;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size");
                        continue;
                    }

                    // 総生産数
                    convoy.Size = (int) n;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "done");
                        continue;
                    }

                    // 生産完了数
                    convoy.Done = (int) n;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days");
                        continue;
                    }

                    // 完了日数
                    convoy.Days = (int) n;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days_for_first");
                        continue;
                    }

                    // 1単位の完了日数
                    convoy.DaysForFirst = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return convoy;
        }

        #endregion

        #region 汎用

        /// <summary>
        ///     IDリストを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>IDリスト</returns>
        private static IEnumerable<int> ParseIdList(TextLexer lexer)
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

            var list = new List<int>();
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
                if (token.Type != TokenType.Number)
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // ID
                list.Add((int) (double) token.Value);
            }

            return list;
        }

        /// <summary>
        ///     国家リストを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国家リスト</returns>
        private static IEnumerable<Country> ParseCountryList(TextLexer lexer)
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

            var list = new List<Country>();
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

                var name = token.Value as string;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                name = name.ToUpper();

                if (!Countries.StringMap.ContainsKey(name))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                Country tag = Countries.StringMap[name];
                if (!Countries.Tags.Contains(tag))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                // 国タグ
                list.Add(tag);
            }

            return list;
        }

        /// <summary>
        ///     日時を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>日時</returns>
        private static GameDate ParseDate(TextLexer lexer)
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

            var date = new GameDate();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // year
                if (keyword.Equals("year"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "year");
                        continue;
                    }

                    // 年
                    date.Year = (int) n;
                    continue;
                }

                // month
                if (keyword.Equals("month"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    token = lexer.GetToken();
                    if (token.Type == TokenType.Number)
                    {
                        // 無効なトークン
                        var month = (int) (double) token.Value;
                        if (month < 0 || month >= 12)
                        {
                            Log.OutOfRange(LogCategory, "month", month, lexer);
                        }

                        // 月
                        date.Month = month + 1;
                        continue;
                    }

                    if (token.Type == TokenType.Identifier)
                    {
                        // 無効なトークン
                        var name = token.Value as string;
                        if (string.IsNullOrEmpty(name))
                        {
                            continue;
                        }
                        name = name.ToLower();

                        if (!Scenarios.MonthStrings.Contains(name))
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
                            continue;
                        }

                        int month = Array.IndexOf(Scenarios.MonthStrings, name);
                        if (month < 0 || month >= 12)
                        {
                            Log.OutOfRange(LogCategory, "month", month, lexer);
                            continue;
                        }

                        // 月
                        date.Month = month + 1;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // day
                if (keyword.Equals("day"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "day");
                        continue;
                    }

                    // 30日の記載が数多くあるので[情報]レベルでエラー出力する
                    if ((int) n == 30)
                    {
                        Log.Info("[Scenario] Out of range: {0} at day ({1} L{2})",
                            ObjectHelper.ToString(n), lexer.FileName, lexer.LineNo);
                    }
                    else if ((int) n < 0 || (int) n > 30)
                    {
                        Log.OutOfRange(LogCategory, "day", n, lexer);
                    }

                    // 日
                    date.Day = (int) n + 1;
                    continue;
                }

                // hour
                if (keyword.Equals("hour"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "day");
                        continue;
                    }

                    if ((int) n < 0 || (int) n >= 24)
                    {
                        Log.OutOfRange(LogCategory, "hour", n, lexer);
                    }

                    // 時
                    date.Hour = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return date;
        }

        /// <summary>
        ///     typeとidの組を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>typeとidの組</returns>
        private static TypeId ParseTypeId(TextLexer lexer)
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

            var id = new TypeId();
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

                var keyword = token.Value as string;
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }
                keyword = keyword.ToLower();

                // type
                if (keyword.Equals("type"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "type");
                        continue;
                    }

                    // type
                    id.Type = (int) n;
                    continue;
                }

                // id
                if (keyword.Equals("id"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "id");
                        continue;
                    }

                    // id
                    id.Id = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return id;
        }

        #endregion

        #region 値

        /// <summary>
        ///     整数値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>整数値</returns>
        private static int? ParseInt(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return (int) (double) token.Value;
        }

        /// <summary>
        ///     実数値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>実数値</returns>
        private static double? ParseDouble(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.Number)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return (double) token.Value;
        }

        /// <summary>
        ///     文字列値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>文字列値</returns>
        private static string ParseString(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.String)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return token.Value as string;
        }

        /// <summary>
        ///     識別子値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>文字列値</returns>
        private static string ParseIdentifier(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.Identifier)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return token.Value as string;
        }

        /// <summary>
        ///     文字列値または識別子値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>文字列値</returns>
        private static string ParseStringOrIdentifier(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.String && token.Type != TokenType.Identifier)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return token.Value as string;
        }

        /// <summary>
        ///     ブール値を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>ブール値</returns>
        private static bool? ParseBool(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type == TokenType.Identifier)
            {
                var s = token.Value as string;
                if (string.IsNullOrEmpty(s))
                {
                    return null;
                }
                s = s.ToLower();

                // yes
                if (s.Equals("yes"))
                {
                    return true;
                }

                // no
                if (s.Equals("no"))
                {
                    return false;
                }
            }

            else if (token.Type == TokenType.Number)
            {
                var n = (int) (double) token.Value;

                if (n == 1)
                {
                    return true;
                }

                if (n == 0)
                {
                    return false;
                }
            }

            // 無効なトークン
            Log.InvalidToken(LogCategory, token, lexer);
            return null;
        }

        /// <summary>
        ///     国タグを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国タグ</returns>
        private static Country? ParseTag(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return null;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.String && token.Type != TokenType.Identifier)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            var name = token.Value as string;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            name = name.ToUpper();

            if (!Countries.StringMap.ContainsKey(name))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            Country tag = Countries.StringMap[name];
            if (!Countries.Tags.Contains(tag))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return tag;
        }

        /// <summary>
        ///     師団のユニット種類を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国タグ</returns>
        private static UnitType ParseDivisionType(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return UnitType.None;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.Identifier)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            var s = token.Value as string;
            if (string.IsNullOrEmpty(s))
            {
                return UnitType.None;
            }
            s = s.ToLower();

            // ユニットクラス名以外
            if (!Units.StringMap.ContainsKey(s))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            // サポート外のユニット種類
            UnitType type = Units.StringMap[s];
            if (!Units.DivisionTypes.Contains(type))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            return type;
        }

        /// <summary>
        ///     旅団のユニット種類を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国タグ</returns>
        private static UnitType ParseBrigadeType(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
                return UnitType.None;
            }

            // 無効なトークン
            token = lexer.GetToken();
            if (token.Type != TokenType.Identifier)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            var s = token.Value as string;
            if (string.IsNullOrEmpty(s))
            {
                return UnitType.None;
            }
            s = s.ToLower();

            // ユニットクラス名以外
            if (!Units.StringMap.ContainsKey(s))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            // サポート外のユニット種類
            UnitType type = Units.StringMap[s];
            if (!Units.BrigadeTypes.Contains(type))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return UnitType.None;
            }

            return type;
        }

        #endregion

        #endregion

        #region ファイル判別

        /// <summary>
        ///     シナリオファイルの種類
        /// </summary>
        public enum ScenarioFileKind
        {
            Normal, // その他
            Top, // 最上位のファイル
            BasesInc, // bases.inc
            BasesDodInc, // bases_DOD.inc
            VpInc // vp.inc
        }

        /// <summary>
        ///     解析中のシナリオファイルの種類を取得する
        /// </summary>
        /// <returns>シナリオファイルの種類</returns>
        private static ScenarioFileKind GetScenarioFileKind()
        {
            // ディレクトリ名がscenariosの場合は最上位のファイルとみなす
            string dirName = Path.GetDirectoryName(_fileName);
            string parentName = Path.GetFileName(dirName);
            if (string.IsNullOrEmpty(parentName))
            {
                return ScenarioFileKind.Normal;
            }
            if (parentName.Equals("scenarios"))
            {
                return ScenarioFileKind.Top;
            }

            string name = Path.GetFileName(_fileName);
            if (string.IsNullOrEmpty(name))
            {
                return ScenarioFileKind.Normal;
            }
            name = name.ToLower();

            // bases.inc
            if (name.Equals("bases.inc"))
            {
                return ScenarioFileKind.BasesInc;
            }

            // bases_DOD.inc
            if (name.Equals("bases_dod.inc"))
            {
                return ScenarioFileKind.BasesDodInc;
            }

            // vp.inc
            if (name.Equals("vp.inc"))
            {
                return ScenarioFileKind.VpInc;
            }

            return ScenarioFileKind.Normal;
        }

        #endregion
    }
}