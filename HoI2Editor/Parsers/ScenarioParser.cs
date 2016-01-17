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

                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseString(lexer);
                        if (s == null)
                        {
                            Log.InvalidClause(LogCategory, "name", lexer);
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
                        if (s == null)
                        {
                            Log.InvalidClause(LogCategory, "panel", lexer);
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
                            Log.InvalidSection(LogCategory, "header", lexer);
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
                            Log.InvalidSection(LogCategory, "globaldata", lexer);
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
                            Log.InvalidSection(LogCategory, "history", lexer);
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
                            Log.InvalidSection(LogCategory, "sleepevent", lexer);
                            continue;
                        }

                        // 休止イベント
                        scenario.SleepEvents.AddRange(list);
                        continue;
                    }

                    // save_date
                    if (keyword.Equals("save_date"))
                    {
                        // シナリオ開始日時が設定されていなければ1936/1/1とみなす
                        GameDate startDate = scenario.GlobalData?.StartDate ?? new GameDate();

                        Dictionary<int, GameDate> dates = ParseSaveDate(lexer, startDate);
                        if (dates == null)
                        {
                            Log.InvalidSection(LogCategory, "save_date", lexer);
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
                            Log.InvalidSection(LogCategory, "map", lexer);
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
                        if (s == null)
                        {
                            Log.InvalidClause(LogCategory, "event", lexer);
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
                        if (s == null)
                        {
                            Log.InvalidClause(LogCategory, "include", lexer);
                            continue;
                        }

                        // インクルードファイル
                        if (GetScenarioFileKind() == ScenarioFileKind.Top)
                        {
                            scenario.IncludeFiles.Add(s);

                            // インクルードフォルダを設定する
                            string folderName = Path.GetDirectoryName(s);
                            scenario.IncludeFolder = Path.GetFileName(folderName);
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
                            Log.InvalidSection(LogCategory, "province", lexer);
                            continue;
                        }

                        // プロヴィンス設定
                        Scenarios.AddProvinceSettings(province);
                        switch (GetScenarioFileKind())
                        {
                            case ScenarioFileKind.BasesInc: // bases.inc
                                scenario.IsBaseProvinceSettings = true;
                                break;

                            case ScenarioFileKind.BasesDodInc: // bases_DOD.inc
                                scenario.IsBaseDodProvinceSettings = true;
                                break;

                            case ScenarioFileKind.DepotsInc: // depots.inc
                                scenario.IsDepotsProvinceSettings = true;
                                break;

                            case ScenarioFileKind.VpInc: // vp.inc
                                scenario.IsVpProvinceSettings = true;
                                break;

                            case ScenarioFileKind.Top: // シナリオ.eug
                                break;

                            default:
                                scenario.IsCountryProvinceSettings = true;
                                break;
                        }
                        continue;
                    }

                    // country
                    if (keyword.Equals("country"))
                    {
                        CountrySettings country = ParseCountry(lexer, scenario);
                        if (country == null)
                        {
                            Log.InvalidSection(LogCategory, "country", lexer);
                            continue;
                        }

                        if (!scenario.Countries.Contains(country))
                        {
                            // ファイル名を関連付ける
                            country.FileName = Path.GetFileName(_fileName);

                            // 国家設定
                            scenario.Countries.Add(country);
                        }
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

            Dictionary<int, GameDate> dates = new Dictionary<int, GameDate>();
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

                int id = (int) (double) token.Value;

                int? n = ParseInt(lexer);
                if (!n.HasValue)
                {
                    Log.InvalidClause(LogCategory, IntHelper.ToString(id), lexer);
                    continue;
                }

                // 保存日時
                GameDate date = startDate.Minus((int) n);
                dates[id] = date;
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

            ScenarioHeader header = new ScenarioHeader();
            while (true)
            {
                token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    Log.MissingCloseBrace(LogCategory, "header", lexer);
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

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
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
                        Log.InvalidSection(LogCategory, "startdate", lexer);
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
                        Log.InvalidClause(LogCategory, "startyear", lexer);
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
                        Log.InvalidClause(LogCategory, "endyear", lexer);
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
                        Log.InvalidClause(LogCategory, "free", lexer);
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
                        Log.InvalidClause(LogCategory, "combat", lexer);
                        continue;
                    }

                    // ショートシナリオ
                    header.IsBattleScenario = (bool) b;
                    continue;
                }

                // selectable
                if (keyword.Equals("selectable"))
                {
                    IEnumerable<Country> list = ParseCountryList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "selectable", lexer);
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
                        Log.InvalidClause(LogCategory, "set_ai_aggresive", lexer);
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
                        Log.InvalidClause(LogCategory, "set_difficulty", lexer);
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
                        Log.InvalidClause(LogCategory, "set_gamespeed", lexer);
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
                        MajorCountrySettings major = ParseMajorCountry(lexer);
                        if (major == null)
                        {
                            Log.InvalidSection(LogCategory, tagName, lexer);
                            continue;
                        }

                        // 主要国設定
                        major.Country = tag;
                        MajorCountrySettings prev = header.MajorCountries.FirstOrDefault(m => m.Country == tag);
                        if (prev != null)
                        {
                            header.MajorCountries.Remove(prev);
                        }
                        header.MajorCountries.Add(major);
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

            MajorCountrySettings major = new MajorCountrySettings();
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

                // desc
                if (keyword.Equals("desc"))
                {
                    string s = ParseStringOrIdentifier(lexer);
                    if (string.IsNullOrEmpty(s))
                    {
                        Log.InvalidClause(LogCategory, "desc", lexer);
                        continue;
                    }

                    // 説明文
                    major.Desc = s;
                    continue;
                }

                // picture
                if (keyword.Equals("picture"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "picture", lexer);
                        continue;
                    }

                    // プロパガンダ画像名
                    major.PictureName = s;
                    continue;
                }

                // songs
                if (keyword.Equals("songs"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "songs", lexer);
                        continue;
                    }

                    // 音楽ファイル名
                    major.Songs = s;
                    continue;
                }

                // bottom
                if (keyword.Equals("bottom"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "bottom", lexer);
                        continue;
                    }

                    // 右端に配置
                    major.Bottom = (bool) b;
                    continue;
                }

                if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 104))
                {
                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseStringOrIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name", lexer);
                            continue;
                        }

                        // 国名
                        major.Name = s;
                        continue;
                    }

                    // flag_ext
                    if (keyword.Equals("flag_ext"))
                    {
                        string s = ParseStringOrIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "flag_ext", lexer);
                            continue;
                        }

                        // 国旗接尾辞
                        major.FlagExt = s;
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return major;
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

            ScenarioGlobalData data = new ScenarioGlobalData();
            while (true)
            {
                token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    Log.MissingCloseBrace(LogCategory, "globaldata", lexer);
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

                // rules
                if (keyword.Equals("rules"))
                {
                    ScenarioRules rules = ParseRules(lexer);
                    if (rules == null)
                    {
                        Log.InvalidSection(LogCategory, "rules", lexer);
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
                        Log.InvalidSection(LogCategory, "startdate", lexer);
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
                        Log.InvalidSection(LogCategory, "enddate", lexer);
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
                        Log.InvalidSection(LogCategory, "axis", lexer);
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
                        Log.InvalidSection(LogCategory, "allies", lexer);
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
                        Log.InvalidSection(LogCategory, "comintern", lexer);
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
                        Log.InvalidSection(LogCategory, "alliance", lexer);
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
                        Log.InvalidSection(LogCategory, "war", lexer);
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
                        Log.InvalidSection(LogCategory, "treaty", lexer);
                        continue;
                    }

                    // 外交協定
                    switch (treaty.Type)
                    {
                        case TreatyType.NonAggression:
                            data.NonAggressions.Add(treaty);
                            break;

                        case TreatyType.Peace:
                            data.Peaces.Add(treaty);
                            break;

                        case TreatyType.Trade:
                            data.Trades.Add(treaty);
                            break;
                    }
                    continue;
                }

                // flags
                if (keyword.Equals("flags"))
                {
                    Dictionary<string, string> flags = ParseFlags(lexer);
                    if (flags == null)
                    {
                        Log.InvalidSection(LogCategory, "flags", lexer);
                        continue;
                    }

                    // グローバルフラグリスト
                    data.Flags = flags;
                    continue;
                }

                // queued_events
                if (keyword.Equals("queued_events"))
                {
                    List<QueuedEvent> events = ParseQueuedEvents(lexer);
                    if (events == null)
                    {
                        Log.InvalidSection(LogCategory, "queued_events", lexer);
                        continue;
                    }

                    // 処理待ちイベントリスト
                    data.QueuedEvents.AddRange(events);
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
                        string s = token.Value as string;
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

                    List<int> list = new List<int>();
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
                    continue;
                }

                // dormant_ministers
                if (keyword.Equals("dormant_ministers"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_ministers", lexer);
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
                        Log.InvalidSection(LogCategory, "dormant_teams", lexer);
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
                        Log.InvalidSection(LogCategory, "weather", lexer);
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
        ///     処理待ちイベントリストを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>処理待ちイベントリスト</returns>
        private static List<QueuedEvent> ParseQueuedEvents(TextLexer lexer)
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

            List<QueuedEvent> list = new List<QueuedEvent>();
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

                // event
                if (keyword.Equals("event"))
                {
                    QueuedEvent qe = ParseQueuedEvent(lexer);
                    if (qe == null)
                    {
                        Log.InvalidSection(LogCategory, "event", lexer);
                        continue;
                    }

                    // 処理待ちイベント
                    list.Add(qe);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return list;
        }

        /// <summary>
        ///     処理待ちイベントを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>処理待ちイベント</returns>
        private static QueuedEvent ParseQueuedEvent(TextLexer lexer)
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

            QueuedEvent qe = new QueuedEvent();
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

                // tag
                if (keyword.Equals("tag"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tag", lexer);
                        continue;
                    }

                    // イベント発生国
                    qe.Country = (Country) tag;
                    continue;
                }

                // id
                if (keyword.Equals("id"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "id", lexer);
                        continue;
                    }

                    // イベントID
                    qe.Id = (int) n;
                    continue;
                }

                // hour
                if (keyword.Equals("hour"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "hour", lexer);
                        continue;
                    }

                    // イベント発生待ち時間
                    qe.Hour = (int) n;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return qe;
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

            ScenarioRules rules = new ScenarioRules();
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

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "diplomacy", lexer);
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
                        Log.InvalidClause(LogCategory, "production", lexer);
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
                        Log.InvalidClause(LogCategory, "technology", lexer);
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

            Weather weather = new Weather();
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

                // static
                if (keyword.Equals("static"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "static", lexer);
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
                        Log.InvalidSection(LogCategory, "pattern", lexer);
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

            WeatherPattern pattern = new WeatherPattern();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
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
                        Log.InvalidSection(LogCategory, "provinces", lexer);
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
                        Log.InvalidClause(LogCategory, "centre", lexer);
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
                        Log.InvalidClause(LogCategory, "speed", lexer);
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
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "heading", lexer);
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

            MapSettings map = new MapSettings();
            while (true)
            {
                token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    Log.MissingCloseBrace(LogCategory, "map", lexer);
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
                        string s = token.Value as string;
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
                        string s = token.Value as string;
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
                        Log.InvalidSection(LogCategory, "top", lexer);
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
                        Log.InvalidSection(LogCategory, "bottom", lexer);
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

            MapPoint point = new MapPoint();
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

                // x
                if (keyword.Equals("x"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "x", lexer);
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
                        Log.InvalidClause(LogCategory, "y", lexer);
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

            ProvinceSettings province = new ProvinceSettings();
            while (true)
            {
                token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    Log.MissingCloseBrace(LogCategory, "province", lexer);
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

                // id
                if (keyword.Equals("id"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "id", lexer);
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
                        Log.InvalidSection(LogCategory, "ic", lexer);
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
                        Log.InvalidSection(LogCategory, "infra", lexer);
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
                        Log.InvalidSection(LogCategory, "landfort", lexer);
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
                        Log.InvalidSection(LogCategory, "coastalfort", lexer);
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
                        Log.InvalidSection(LogCategory, "anti_air", lexer);
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
                        Log.InvalidSection(LogCategory, "air_base", lexer);
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
                        Log.InvalidSection(LogCategory, "naval_base", lexer);
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
                        Log.InvalidSection(LogCategory, "radar_station", lexer);
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
                        Log.InvalidSection(LogCategory, "nuclear_reactor", lexer);
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
                        Log.InvalidSection(LogCategory, "rocket_test", lexer);
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
                        Log.InvalidSection(LogCategory, "synthetic_oil", lexer);
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
                        Log.InvalidSection(LogCategory, "synthetic_rares", lexer);
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
                        Log.InvalidSection(LogCategory, "nuclear_power", lexer);
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
                        Log.InvalidClause(LogCategory, "supplypool", lexer);
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
                        Log.InvalidClause(LogCategory, "oilpool", lexer);
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
                        Log.InvalidClause(LogCategory, "energypool", lexer);
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
                        Log.InvalidClause(LogCategory, "metalpool", lexer);
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
                        Log.InvalidClause(LogCategory, "rarematerialspool", lexer);
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
                        Log.InvalidClause(LogCategory, "energy", lexer);
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
                        Log.InvalidClause(LogCategory, "max_energy", lexer);
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
                        Log.InvalidClause(LogCategory, "metal", lexer);
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
                        Log.InvalidClause(LogCategory, "max_metal", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "max_rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "oil", lexer);
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
                        Log.InvalidClause(LogCategory, "max_oil", lexer);
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
                        Log.InvalidClause(LogCategory, "manpower", lexer);
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
                        Log.InvalidClause(LogCategory, "max_manpower", lexer);
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
                        Log.InvalidClause(LogCategory, "points", lexer);
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
                        Log.InvalidClause(LogCategory, "province_revoltrisk", lexer);
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
                        string s = ParseStringOrIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name", lexer);
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
                return new BuildingSize { Size = (double) token.Value };
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            BuildingSize size = new BuildingSize();
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

                // size
                if (keyword.Equals("size"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size", lexer);
                        continue;
                    }

                    if (d < 0)
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
                        Log.InvalidClause(LogCategory, "current_size", lexer);
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

            BuildingDevelopment building = new BuildingDevelopment();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
                        continue;
                    }

                    // typeとidの組
                    building.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
                        continue;
                    }

                    // 名前
                    building.Name = s;
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

                    string s = token.Value as string;
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
                        Log.InvalidClause(LogCategory, "location", lexer);
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
                        Log.InvalidClause(LogCategory, "cost", lexer);
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
                        Log.InvalidClause(LogCategory, "manpower", lexer);
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
                        Log.InvalidSection(LogCategory, "date", lexer);
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
                        Log.InvalidClause(LogCategory, "progress", lexer);
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
                        Log.InvalidClause(LogCategory, "total_progress", lexer);
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
                        Log.InvalidClause(LogCategory, "gearing_bonus", lexer);
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
                        Log.InvalidClause(LogCategory, "size", lexer);
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
                        Log.InvalidClause(LogCategory, "done", lexer);
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
                        Log.InvalidClause(LogCategory, "days", lexer);
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
                        Log.InvalidClause(LogCategory, "days_for_first", lexer);
                        continue;
                    }

                    // 1単位の完了日数
                    building.DaysForFirst = (int) n;
                    continue;
                }

                // halted
                if (keyword.Equals("halted"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "halted", lexer);
                        continue;
                    }

                    // 停止中
                    building.Halted = (bool) b;
                    continue;
                }

                // close_when_finished
                if (keyword.Equals("close_when_finished"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "close_when_finished", lexer);
                        continue;
                    }

                    // 完了時にキューを削除するかどうか
                    building.CloseWhenFinished = (bool) b;
                    continue;
                }

                // waitingforclosure
                if (keyword.Equals("waitingforclosure"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "waitingforclosure", lexer);
                        continue;
                    }

                    // 詳細不明
                    building.WaitingForClosure = (bool) b;
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

            Alliance alliance = new Alliance();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
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
                        Log.InvalidSection(LogCategory, "participant", lexer);
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
                        if (s == null)
                        {
                            Log.InvalidClause(LogCategory, "name", lexer);
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

            War war = new War();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
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
                        Log.InvalidSection(LogCategory, "date", lexer);
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
                        Log.InvalidSection(LogCategory, "enddate", lexer);
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
                        Log.InvalidSection(LogCategory, "attackers", lexer);
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
                        Log.InvalidSection(LogCategory, "defenders", lexer);
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

            Treaty treaty = new Treaty();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
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
                        Log.InvalidClause(LogCategory, "type", lexer);
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
                        Log.InvalidClause(LogCategory, "country", lexer);
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
                        Log.InvalidSection(LogCategory, "startdate", lexer);
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
                        Log.InvalidSection(LogCategory, "expirydate", lexer);
                        continue;
                    }

                    // 失効日時
                    treaty.EndDate = date;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "money", lexer);
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
                        Log.InvalidClause(LogCategory, "supplies", lexer);
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
                        Log.InvalidClause(LogCategory, "energy", lexer);
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
                        Log.InvalidClause(LogCategory, "metal", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "oil", lexer);
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
                        Log.InvalidClause(LogCategory, "cancel", lexer);
                        continue;
                    }

                    // 取り消し可能かどうか
                    treaty.Cancel = (bool) b;
                    continue;
                }

                if (Game.Type == GameType.ArsenalOfDemocracy)
                {
                    // isoversea
                    if (keyword.Equals("isoversea"))
                    {
                        bool? b = ParseBool(lexer);
                        if (!b.HasValue)
                        {
                            Log.InvalidClause(LogCategory, "isoversea", lexer);
                            continue;
                        }

                        // 海外貿易かどうか
                        treaty.IsOverSea = (bool) b;
                        continue;
                    }
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
        private static IEnumerable<Relation> ParseDiplomacy(TextLexer lexer)
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

            List<Relation> list = new List<Relation>();
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

                // relation
                if (keyword.Equals("relation"))
                {
                    Relation relation = ParseRelation(lexer);
                    if (relation == null)
                    {
                        Log.InvalidSection(LogCategory, "relation", lexer);
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
        private static Relation ParseRelation(TextLexer lexer)
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

            Relation relation = new Relation();
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

                // tag
                if (keyword.Equals("tag"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tag", lexer);
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
                        Log.InvalidClause(LogCategory, "value", lexer);
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
                        Log.InvalidClause(LogCategory, "access", lexer);
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
                        Log.InvalidSection(LogCategory, "guaranteed", lexer);
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
        /// <param name="scenario">シナリオデータ</param>
        /// <returns>国家設定</returns>
        private static CountrySettings ParseCountry(TextLexer lexer, Scenario scenario)
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

            CountrySettings settings = new CountrySettings();
            while (true)
            {
                token = lexer.GetToken();

                // ファイルの終端
                if (token == null)
                {
                    Log.MissingCloseBrace(LogCategory, "country", lexer);
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

                // tag
                if (keyword.Equals("tag"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "tag", lexer);
                        continue;
                    }

                    CountrySettings prev = scenario.Countries.FirstOrDefault(s => s.Country == tag.Value);
                    if (prev != null)
                    {
                        settings = prev;
                    }

                    // 国タグ
                    settings.Country = (Country) tag;
                    continue;
                }

                // regular_id
                if (keyword.Equals("regular_id"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "regular_id", lexer);
                        continue;
                    }

                    // 兄弟国
                    settings.RegularId = (Country) tag;
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
                    settings.IntrinsicGovType = (GovernmentType) Array.IndexOf(Scenarios.GovernmentStrings, s);
                    continue;
                }

                // puppet
                if (keyword.Equals("puppet"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "puppet", lexer);
                        continue;
                    }

                    // 宗主国
                    settings.Master = (Country) tag;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (tag == null)
                    {
                        Log.InvalidClause(LogCategory, "control", lexer);
                        continue;
                    }

                    // 統帥権取得国
                    settings.Control = (Country) tag;
                    continue;
                }

                // belligerence
                if (keyword.Equals("belligerence"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "belligerence", lexer);
                        continue;
                    }

                    // 好戦性
                    settings.Belligerence = (int) n;
                    continue;
                }

                // extra_tc
                if (keyword.Equals("extra_tc"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "extra_tc", lexer);
                        continue;
                    }

                    // 追加輸送能力
                    settings.ExtraTc = (double) d;
                    continue;
                }

                // dissent
                if (keyword.Equals("dissent"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dissent", lexer);
                        continue;
                    }

                    // 国民不満度
                    settings.Dissent = (double) d;
                    continue;
                }

                // capital
                if (keyword.Equals("capital"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "capital", lexer);
                        continue;
                    }

                    // 首都のプロヴィンスID
                    settings.Capital = (int) (double) n;
                    continue;
                }

                // tc_mod
                if (keyword.Equals("tc_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tc_mod", lexer);
                        continue;
                    }

                    // TC補正
                    settings.TcModifier = (double) d;
                    continue;
                }

                // tc_occupied_mod
                if (keyword.Equals("tc_occupied_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tc_occupied_mod", lexer);
                        continue;
                    }

                    // 占領地TC補正
                    settings.TcOccupiedModifier = (double) d;
                    continue;
                }

                // attrition_mod
                if (keyword.Equals("attrition_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "attrition_mod", lexer);
                        continue;
                    }

                    // 消耗補正
                    settings.AttritionModifier = (double) d;
                    continue;
                }

                // trickleback_mod
                if (keyword.Equals("trickleback_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "trickleback_mod", lexer);
                        continue;
                    }

                    // 漸次撤退補正
                    settings.TricklebackModifier = (double) d;
                    continue;
                }

                // max_amphib_mod
                if (keyword.Equals("max_amphib_mod"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_amphib_mod", lexer);
                        continue;
                    }

                    // 最大強襲上陸補正
                    settings.MaxAmphibModifier = (int) n;
                    continue;
                }

                // supply_dist_mod
                if (keyword.Equals("supply_dist_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supply_dist_mod", lexer);
                        continue;
                    }

                    // 補給補正
                    settings.SupplyDistModifier = (double) d;
                    continue;
                }

                // repair_mod
                if (keyword.Equals("repair_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "repair_mod", lexer);
                        continue;
                    }

                    // 修理補正
                    settings.RepairModifier = (double) d;
                    continue;
                }

                // research_mod
                if (keyword.Equals("research_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "research_mod", lexer);
                        continue;
                    }

                    // 研究補正
                    settings.ResearchModifier = (double) d;
                    continue;
                }

                // peacetime_ic_mod
                if (keyword.Equals("peacetime_ic_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "peacetime_ic_mod", lexer);
                        continue;
                    }

                    // 平時IC補正
                    settings.PeacetimeIcModifier = (double) d;
                    continue;
                }

                // wartime_ic_mod
                if (keyword.Equals("wartime_ic_mod"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "wartime_ic_mod", lexer);
                        continue;
                    }

                    // 戦時IC補正
                    settings.WartimeIcModifier = (double) d;
                    continue;
                }

                // industrial_modifier
                if (keyword.Equals("industrial_modifier"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "industrial_modifier", lexer);
                        continue;
                    }

                    // 工業力補正
                    settings.IndustrialModifier = (double) d;
                    continue;
                }

                // ground_def_eff
                if (keyword.Equals("ground_def_eff"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "ground_def_eff", lexer);
                        continue;
                    }

                    // 対地防御補正
                    settings.GroundDefEff = (double) d;
                    continue;
                }

                // ai
                if (keyword.Equals("ai"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "ai", lexer);
                        continue;
                    }

                    // AIファイル名
                    settings.AiFileName = s;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower", lexer);
                        continue;
                    }

                    // 人的資源
                    settings.Manpower = (double) d;
                    continue;
                }

                // relative_manpower
                if (keyword.Equals("relative_manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "relative_manpower", lexer);
                        continue;
                    }

                    // 人的資源補正値
                    settings.RelativeManpower = (double) d;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "energy", lexer);
                        continue;
                    }

                    // エネルギー
                    settings.Energy = (double) d;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "metal", lexer);
                        continue;
                    }

                    // 金属
                    settings.Metal = (double) d;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
                        continue;
                    }

                    // 希少資源
                    settings.RareMaterials = (double) d;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil", lexer);
                        continue;
                    }

                    // 石油
                    settings.Oil = (double) d;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplies", lexer);
                        continue;
                    }

                    // 物資
                    settings.Supplies = (double) d;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "money", lexer);
                        continue;
                    }

                    // 資金
                    settings.Money = (double) d;
                    continue;
                }

                // transports
                if (keyword.Equals("transports"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transports", lexer);
                        continue;
                    }

                    // 輸送船団
                    settings.Transports = (int) n;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "escorts", lexer);
                        continue;
                    }

                    // 護衛艦
                    settings.Escorts = (int) n;
                    continue;
                }

                // nuke
                if (keyword.Equals("nuke"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "nuke", lexer);
                        continue;
                    }

                    // 核兵器
                    settings.Nuke = (int) n;
                    continue;
                }

                // free
                if (keyword.Equals("free"))
                {
                    ResourceSettings free = ParseFree(lexer);
                    if (free == null)
                    {
                        Log.InvalidSection(LogCategory, "free", lexer);
                        continue;
                    }

                    // マップ外資源
                    settings.Offmap = free;
                    continue;
                }

                // consumer
                if (keyword.Equals("consumer"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "consumer", lexer);
                        continue;
                    }

                    // 消費財IC比率
                    settings.ConsumerSlider = (double) d;
                    continue;
                }

                // supply
                if (keyword.Equals("supply"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supply", lexer);
                        continue;
                    }

                    // 物資IC比率
                    settings.ConsumerSlider = (double) d;
                    continue;
                }

                // production
                if (keyword.Equals("production"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "production", lexer);
                        continue;
                    }

                    // 生産IC比率
                    settings.ConsumerSlider = (double) d;
                    continue;
                }

                // reinforcement
                if (keyword.Equals("reinforcement"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "reinforcement", lexer);
                        continue;
                    }

                    // 補充IC比率
                    settings.ConsumerSlider = (double) d;
                    continue;
                }

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    IEnumerable<Relation> list = ParseDiplomacy(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "diplomacy", lexer);
                        continue;
                    }

                    // 外交設定
                    settings.Relations.AddRange(list);
                    continue;
                }

                // spyinfo
                if (keyword.Equals("spyinfo"))
                {
                    SpySettings spy = ParseSpyInfo(lexer);
                    if (spy == null)
                    {
                        Log.InvalidSection(LogCategory, "spyinfo", lexer);
                        continue;
                    }

                    // 諜報設定
                    settings.Intelligence.Add(spy);
                    continue;
                }

                // nationalprovinces
                if (keyword.Equals("nationalprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "nationalprovinces", lexer);
                        continue;
                    }

                    // 中核プロヴィンス
                    settings.NationalProvinces.AddRange(list);
                    continue;
                }

                // ownedprovinces
                if (keyword.Equals("ownedprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "ownedprovinces", lexer);
                        continue;
                    }

                    // 保有プロヴィンス
                    settings.OwnedProvinces.AddRange(list);
                    continue;
                }

                // controlledprovinces
                if (keyword.Equals("controlledprovinces"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "controlledprovinces", lexer);
                        continue;
                    }

                    // 支配プロヴィンス
                    settings.ControlledProvinces.AddRange(list);
                    continue;
                }

                // techapps
                if (keyword.Equals("techapps"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "techapps", lexer);
                        continue;
                    }

                    // 保有技術
                    settings.TechApps.AddRange(list);
                    continue;
                }

                // blueprints
                if (keyword.Equals("blueprints"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "blueprints", lexer);
                        continue;
                    }

                    // 青写真
                    settings.BluePrints.AddRange(list);
                    continue;
                }

                // inventions
                if (keyword.Equals("inventions"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "inventions", lexer);
                        continue;
                    }

                    // 発明イベント
                    settings.Inventions.AddRange(list);
                    continue;
                }

                // policy
                if (keyword.Equals("policy"))
                {
                    CountryPolicy policy = ParsePolicy(lexer);
                    if (policy == null)
                    {
                        Log.InvalidSection(LogCategory, "policy", lexer);
                        continue;
                    }

                    // 政策スライダー
                    settings.Policy = policy;
                    continue;
                }

                // nukedate
                if (keyword.Equals("nukedate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "nukedate", lexer);
                        continue;
                    }

                    // 核兵器完成日時
                    settings.NukeDate = date;
                    continue;
                }

                // headofstate
                if (keyword.Equals("headofstate"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "headofstate", lexer);
                        continue;
                    }

                    // 国家元首
                    settings.HeadOfState = id;
                    continue;
                }

                // headofgovernment
                if (keyword.Equals("headofgovernment"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "headofgovernment", lexer);
                        continue;
                    }

                    // 政府首班
                    settings.HeadOfGovernment = id;
                    continue;
                }

                // foreignminister
                if (keyword.Equals("foreignminister"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "foreignminister", lexer);
                        continue;
                    }

                    // 外務大臣
                    settings.ForeignMinister = id;
                    continue;
                }

                // armamentminister
                if (keyword.Equals("armamentminister"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "armamentminister", lexer);
                        continue;
                    }

                    // 軍需大臣
                    settings.ArmamentMinister = id;
                    continue;
                }

                // ministerofsecurity
                if (keyword.Equals("ministerofsecurity"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "ministerofsecurity", lexer);
                        continue;
                    }

                    // 内務大臣
                    settings.MinisterOfSecurity = id;
                    continue;
                }

                // ministerofintelligence
                if (keyword.Equals("ministerofintelligence"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "ministerofintelligence", lexer);
                        continue;
                    }

                    // 情報大臣
                    settings.MinisterOfIntelligence = id;
                    continue;
                }

                // chiefofstaff
                if (keyword.Equals("chiefofstaff"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofstaff", lexer);
                        continue;
                    }

                    // 統合参謀総長
                    settings.ChiefOfStaff = id;
                    continue;
                }

                // chiefofarmy
                if (keyword.Equals("chiefofarmy"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofarmy", lexer);
                        continue;
                    }

                    // 陸軍総司令官
                    settings.ChiefOfArmy = id;
                    continue;
                }

                // chiefofnavy
                if (keyword.Equals("chiefofnavy"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofnavy", lexer);
                        continue;
                    }

                    // 海軍総司令官
                    settings.ChiefOfNavy = id;
                    continue;
                }

                // chiefofair
                if (keyword.Equals("chiefofair"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "chiefofair", lexer);
                        continue;
                    }

                    // 空軍総司令官
                    settings.ChiefOfAir = id;
                    continue;
                }

                // nationalidentity
                if (keyword.Equals("nationalidentity"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "nationalidentity", lexer);
                        continue;
                    }

                    // 国民の意識
                    settings.NationalIdentity = s;
                    continue;
                }

                // socialpolicy
                if (keyword.Equals("socialpolicy"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "socialpolicy", lexer);
                        continue;
                    }

                    // 社会政策
                    settings.SocialPolicy = s;
                    continue;
                }

                // nationalculture
                if (keyword.Equals("nationalculture"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "nationalculture", lexer);
                        continue;
                    }

                    // 国家の文化
                    settings.NationalCulture = s;
                    continue;
                }

                // dormant_leaders
                if (keyword.Equals("dormant_leaders"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_leaders", lexer);
                        continue;
                    }

                    // 休止指揮官
                    settings.DormantLeaders.AddRange(list);
                    continue;
                }

                // dormant_ministers
                if (keyword.Equals("dormant_ministers"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_ministers", lexer);
                        continue;
                    }

                    // 休止閣僚
                    settings.DormantMinisters.AddRange(list);
                    continue;
                }

                // dormant_teams
                if (keyword.Equals("dormant_teams"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "dormant_teams", lexer);
                        continue;
                    }

                    // 休止研究機関
                    settings.DormantTeams.AddRange(list);
                    continue;
                }

                // steal_leader
                if (keyword.Equals("steal_leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "steal_leader", lexer);
                        continue;
                    }

                    // 抽出指揮官
                    settings.StealLeaders.Add((int) n);
                    continue;
                }

                // allowed_divisions
                if (keyword.Equals("allowed_divisions"))
                {
                    Dictionary<UnitType, bool> divisions = ParseAllowedDivisions(lexer);
                    if (divisions == null)
                    {
                        Log.InvalidClause(LogCategory, "allowed_divisions", lexer);
                        continue;
                    }

                    // 生産可能師団
                    foreach (KeyValuePair<UnitType, bool> pair in divisions)
                    {
                        settings.AllowedDivisions[pair.Key] = pair.Value;
                    }
                    continue;
                }

                // allowed_brigades
                if (keyword.Equals("allowed_brigades"))
                {
                    Dictionary<UnitType, bool> brigades = ParseAllowedBrigades(lexer);
                    if (brigades == null)
                    {
                        Log.InvalidClause(LogCategory, "allowed_brigades", lexer);
                        continue;
                    }

                    // 生産可能旅団
                    foreach (KeyValuePair<UnitType, bool> pair in brigades)
                    {
                        settings.AllowedDivisions[pair.Key] = pair.Value;
                    }
                    continue;
                }

                // convoy
                if (keyword.Equals("convoy"))
                {
                    Convoy convoy = ParseConvoy(lexer);
                    if (convoy == null)
                    {
                        Log.InvalidSection(LogCategory, "convoy", lexer);
                        continue;
                    }

                    // 輸送船団
                    settings.Convoys.Add(convoy);
                    continue;
                }

                // landunit
                if (keyword.Equals("landunit"))
                {
                    Unit unit = ParseUnit(lexer, Branch.Army);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "landunit", lexer);
                        continue;
                    }

                    // 陸軍ユニット
                    settings.LandUnits.Add(unit);
                    continue;
                }

                // navalunit
                if (keyword.Equals("navalunit"))
                {
                    Unit unit = ParseUnit(lexer, Branch.Navy);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "navalunit", lexer);
                        continue;
                    }

                    // 海軍ユニット
                    settings.NavalUnits.Add(unit);
                    continue;
                }

                // airunit
                if (keyword.Equals("airunit"))
                {
                    Unit unit = ParseUnit(lexer, Branch.Airforce);
                    if (unit == null)
                    {
                        Log.InvalidSection(LogCategory, "airunit", lexer);
                        continue;
                    }

                    // 空軍ユニット
                    settings.AirUnits.Add(unit);
                    continue;
                }

                // division_development
                if (keyword.Equals("division_development"))
                {
                    DivisionDevelopment division = ParseDivisionDevelopment(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division_development", lexer);
                        continue;
                    }

                    // 生産中師団
                    settings.DivisionDevelopments.Add(division);
                    continue;
                }

                // convoy_development
                if (keyword.Equals("convoy_development"))
                {
                    ConvoyDevelopment convoy = ParseConvoyDevelopment(lexer);
                    if (convoy == null)
                    {
                        Log.InvalidSection(LogCategory, "convoy_development", lexer);
                        continue;
                    }

                    // 生産中輸送船団
                    settings.ConvoyDevelopments.Add(convoy);
                    continue;
                }

                // province_development
                if (keyword.Equals("province_development"))
                {
                    BuildingDevelopment building = ParseBuildingDevelopment(lexer);
                    if (building == null)
                    {
                        Log.InvalidSection(LogCategory, "province_development", lexer);
                        continue;
                    }

                    // 生産中建物
                    settings.BuildingDevelopments.Add(building);
                    continue;
                }

                // landdivision
                if (keyword.Equals("landdivision"))
                {
                    Division division = ParseDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "landdivision", lexer);
                        continue;
                    }

                    // 陸軍師団
                    division.Branch = Branch.Army;
                    settings.LandDivisions.Add(division);
                    continue;
                }

                // navaldivision
                if (keyword.Equals("navaldivision"))
                {
                    Division division = ParseDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "navaldivision", lexer);
                        continue;
                    }

                    // 海軍師団
                    division.Branch = Branch.Navy;
                    settings.NavalDivisions.Add(division);
                    continue;
                }

                // airdivision
                if (keyword.Equals("airdivision"))
                {
                    Division division = ParseDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "airdivision", lexer);
                        continue;
                    }

                    // 空軍師団
                    division.Branch = Branch.Airforce;
                    settings.AirDivisions.Add(division);
                    continue;
                }

                if (Game.Type == GameType.DarkestHour)
                {
                    // name
                    if (keyword.Equals("name"))
                    {
                        string s = ParseStringOrIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "name", lexer);
                            continue;
                        }

                        // 国名
                        settings.Name = s;
                        continue;
                    }

                    // flag_ext
                    if (keyword.Equals("flag_ext"))
                    {
                        string s = ParseIdentifier(lexer);
                        if (string.IsNullOrEmpty(s))
                        {
                            Log.InvalidClause(LogCategory, "flag_ext", lexer);
                            continue;
                        }

                        // 国旗の接尾辞
                        settings.FlagExt = s;
                        continue;
                    }

                    // ai_settings
                    if (keyword.Equals("ai_settings"))
                    {
                        AiSettings ai = ParseAiSettings(lexer);
                        if (settings == null)
                        {
                            Log.InvalidSection(LogCategory, "ai_settings", lexer);
                            continue;
                        }

                        // AI設定
                        settings.AiSettings = ai;
                        continue;
                    }

                    // claimedprovinces
                    if (keyword.Equals("claimedprovinces"))
                    {
                        IEnumerable<int> list = ParseIdList(lexer);
                        if (list == null)
                        {
                            Log.InvalidSection(LogCategory, "claimedprovinces", lexer);
                            continue;
                        }

                        // 領有権主張プロヴィンス
                        settings.ClaimedProvinces.AddRange(list);
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return settings;
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

            AiSettings settings = new AiSettings();
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

                // flags
                if (keyword.Equals("flags"))
                {
                    Dictionary<string, string> flags = ParseFlags(lexer);
                    if (flags == null)
                    {
                        Log.InvalidSection(LogCategory, "flags", lexer);
                        continue;
                    }

                    // ローカルフラグリスト
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

            ResourceSettings free = new ResourceSettings();
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

                // ic
                if (keyword.Equals("ic"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "ic", lexer);
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
                        Log.InvalidClause(LogCategory, "manpower", lexer);
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
                        Log.InvalidClause(LogCategory, "energy", lexer);
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
                        Log.InvalidClause(LogCategory, "metal", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "oil", lexer);
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
                        Log.InvalidClause(LogCategory, "supplies", lexer);
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
                        Log.InvalidClause(LogCategory, "money", lexer);
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
                        Log.InvalidClause(LogCategory, "transport", lexer);
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
                        Log.InvalidClause(LogCategory, "escort", lexer);
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

            SpySettings spy = new SpySettings();
            int lastLineNo = -1;
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

                // country
                if (keyword.Equals("country"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "country", lexer);
                        continue;
                    }

                    // 国タグ
                    spy.Country = (Country) tag;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // numberofspies
                if (keyword.Equals("numberofspies"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "numberofspies", lexer);
                        continue;
                    }

                    // スパイの数
                    spy.Spies = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                if (lexer.LineNo != lastLineNo)
                {
                    // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                    lexer.ReserveToken(token);
                    break;
                }
                lexer.SkipLine();
            }

            return spy;
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

            CountryPolicy policy = new CountryPolicy();
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

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date", lexer);
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
                        Log.InvalidClause(LogCategory, "democratic", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "political_left", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "freedom", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "freedom", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "professional_army", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "defense_lobby", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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
                        Log.InvalidClause(LogCategory, "interventionism", lexer);
                        continue;
                    }

                    if (n < 1 || n > 10)
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

        /// <summary>
        ///     生産可能師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産可能師団</returns>
        private static Dictionary<UnitType, bool> ParseAllowedDivisions(TextLexer lexer)
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

            Dictionary<UnitType, bool> divisions = new Dictionary<UnitType, bool>();
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

                bool? b = ParseBool(lexer);
                if (!b.HasValue)
                {
                    Log.InvalidClause(LogCategory, "allowed_divisions", lexer);
                    lexer.SkipLine();
                    continue;
                }

                divisions[type] = (bool) b;
            }

            return divisions;
        }

        /// <summary>
        ///     生産可能旅団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産可能旅団</returns>
        private static Dictionary<UnitType, bool> ParseAllowedBrigades(TextLexer lexer)
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

            Dictionary<UnitType, bool> brigades = new Dictionary<UnitType, bool>();
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
                if (!Units.BrigadeTypes.Contains(type))
                {
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                bool? b = ParseBool(lexer);
                if (!b.HasValue)
                {
                    Log.InvalidClause(LogCategory, Units.Strings[(int) type], lexer);
                    lexer.SkipLine();
                    continue;
                }

                brigades[type] = (bool) b;
            }

            return brigades;
        }

        #endregion

        #region ユニット

        /// <summary>
        ///     ユニットを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="branch">兵科</param>
        /// <returns>ユニット</returns>
        private static Unit ParseUnit(TextLexer lexer, Branch branch)
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

            Unit unit = new Unit { Branch = branch };
            int lastLineNo = -1;
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
                        continue;
                    }

                    // typeとidの組
                    unit.Id = id;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
                        continue;
                    }

                    // ユニット名
                    unit.Name = s;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
                {
                    Country? tag = ParseTag(lexer);
                    if (!tag.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "control", lexer);
                        continue;
                    }

                    // 統帥国
                    unit.Control = (Country) tag;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // leader
                if (keyword.Equals("leader"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "leader", lexer);
                        continue;
                    }

                    // 指揮官
                    unit.Leader = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "location", lexer);
                        continue;
                    }

                    // 現在位置
                    unit.Location = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // prevprov
                if (keyword.Equals("prevprov"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "prevprov", lexer);
                        continue;
                    }

                    // 直前の位置
                    unit.PrevProv = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // home
                if (keyword.Equals("home"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "home", lexer);
                        continue;
                    }

                    // 基準位置
                    unit.Home = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // base
                if (keyword.Equals("base"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "base", lexer);
                        continue;
                    }

                    // 所属基地
                    unit.Base = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // dig_in
                if (keyword.Equals("dig_in"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dig_in", lexer);
                        continue;
                    }

                    // 塹壕レベル
                    unit.DigIn = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "morale", lexer);
                        continue;
                    }

                    // 士気
                    unit.Morale = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // mission
                if (keyword.Equals("mission"))
                {
                    Mission mission = ParseMission(lexer);
                    if (mission == null)
                    {
                        Log.InvalidSection(LogCategory, "mission", lexer);
                        continue;
                    }

                    // 任務
                    unit.Mission = mission;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date", lexer);
                        continue;
                    }

                    // 指定日時
                    unit.Date = date;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // development
                if (keyword.Equals("development"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "development", lexer);
                        continue;
                    }

                    // development (詳細不明)
                    unit.Development = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // movetime
                if (keyword.Equals("movetime"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "movetime", lexer);
                        continue;
                    }

                    // 移動完了日時
                    unit.MoveTime = date;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // movement
                if (keyword.Equals("movement"))
                {
                    IEnumerable<int> list = ParseIdList(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "movement", lexer);
                        continue;
                    }

                    // 移動経路
                    unit.Movement.AddRange(list);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // attack
                if (keyword.Equals("attack"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "attack", lexer);
                        continue;
                    }

                    // 攻撃開始日時
                    unit.AttackDate = date;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // invasion
                if (keyword.Equals("invasion"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "invasion", lexer);
                        continue;
                    }

                    // 上陸中
                    unit.Invasion = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target", lexer);
                        continue;
                    }

                    // 上陸先
                    unit.Target = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // stand_ground
                if (keyword.Equals("stand_ground"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "stand_ground", lexer);
                        continue;
                    }

                    // 死守命令
                    unit.StandGround = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // scorch_ground
                if (keyword.Equals("scorch_ground"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "scorch_ground", lexer);
                        continue;
                    }

                    // 焦土作戦
                    unit.ScorchGround = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // prioritized
                if (keyword.Equals("prioritized"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "prioritized", lexer);
                        continue;
                    }

                    // 優先
                    unit.Prioritized = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // can_upgrade
                if (keyword.Equals("can_upgrade"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "can_upgrade", lexer);
                        continue;
                    }

                    // 改良可能
                    unit.CanUpgrade = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // can_reinforce
                if (keyword.Equals("can_reinforce"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "can_reinforce", lexer);
                        continue;
                    }

                    // 補充可能
                    unit.CanReinforcement = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // division
                if (keyword.Equals("division"))
                {
                    Division division = ParseDivision(lexer);
                    if (division == null)
                    {
                        Log.InvalidSection(LogCategory, "division", lexer);
                        continue;
                    }

                    // 兵科を設定
                    division.Branch = unit.Branch;

                    // 師団
                    unit.Divisions.Add(division);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // landunit
                if (keyword.Equals("landunit") && unit.Branch != Branch.Army)
                {
                    Unit landUnit = ParseUnit(lexer, Branch.Army);
                    if (landUnit == null)
                    {
                        Log.InvalidSection(LogCategory, "landunit", lexer);
                        continue;
                    }

                    // 搭載ユニット
                    unit.LandUnits.Add(landUnit);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
                {
                    Log.InvalidToken(LogCategory, token, lexer);

                    // strengthは師団対象であるがその後のエラー回避のため読み捨てる
                    ParseDouble(lexer);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // locked
                if (keyword.Equals("locked"))
                {
                    Log.InvalidToken(LogCategory, token, lexer);

                    // lockedは師団対象であるがその後のエラー回避のため読み捨てる
                    ParseBool(lexer);

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                if (lexer.LineNo != lastLineNo)
                {
                    // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                    lexer.ReserveToken(token);
                    break;
                }
                lexer.SkipLine();
            }

            return unit;
        }

        #endregion

        #region 師団

        /// <summary>
        ///     師団を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>師団</returns>
        private static Division ParseDivision(TextLexer lexer)
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

            Division division = new Division();
            int lastLineNo = -1;
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
                        continue;
                    }

                    // 師団名
                    division.Name = s;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType? type = ParseDivisionType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "type", lexer);
                        continue;
                    }

                    // ユニット種類
                    division.Type = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model", lexer);
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // nuke
                if (keyword.Equals("nuke"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "nuke", lexer);
                        continue;
                    }

                    // 核兵器搭載
                    division.Nuke = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra1", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra2", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra3", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra4", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra5", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_strength", lexer);
                        continue;
                    }

                    // 最大戦力
                    division.MaxStrength = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "strength", lexer);
                        continue;
                    }

                    // 戦力
                    division.Strength = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // defaultorganisation
                if (keyword.Equals("defaultorganisation"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "defaultorganisation", lexer);
                        continue;
                    }

                    // 最大組織率
                    division.MaxOrganisation = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "organisation", lexer);
                        continue;
                    }

                    // 組織率
                    division.Organisation = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // morale
                if (keyword.Equals("morale"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "morale", lexer);
                        continue;
                    }

                    // 士気
                    division.Morale = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "experience", lexer);
                        continue;
                    }

                    // 経験値
                    division.Experience = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // div_upgr_progress
                if (keyword.Equals("div_upgr_progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "div_upgr_progress", lexer);
                        continue;
                    }

                    // 改良進捗率
                    division.UpgradeProgress = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // redep_target
                if (keyword.Equals("redep_target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "redep_target", lexer);
                        continue;
                    }

                    // 再配置先プロヴィンス
                    division.RedeployTarget = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // redep_unit_name
                if (keyword.Equals("redep_unit_name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "redep_unit_name", lexer);
                        continue;
                    }

                    // 再配置先ユニット名
                    division.RedeployUnitName = s;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // redep_unit_id
                if (keyword.Equals("redep_unit_id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "redep_unit_id", lexer);
                        continue;
                    }

                    // 再配置先ユニットID
                    division.RedeployUnitId = id;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // offensive
                if (keyword.Equals("offensive"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "offensive", lexer);
                        continue;
                    }

                    // 攻勢開始日時
                    division.Offensive = date;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplies", lexer);
                        continue;
                    }

                    // 物資
                    division.Supplies = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "oil", lexer);
                        continue;
                    }

                    // 燃料
                    division.Fuel = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // max_supply_stock
                if (keyword.Equals("max_supply_stock"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_supply_stock", lexer);
                        continue;
                    }

                    // 最大物資
                    division.MaxSupplies = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // max_oil_stock
                if (keyword.Equals("max_oil_stock"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "max_oil_stock", lexer);
                        continue;
                    }

                    // 最大燃料
                    division.MaxFuel = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // supplyconsumption
                if (keyword.Equals("supplyconsumption"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "supplyconsumption", lexer);
                        continue;
                    }

                    // 物資消費量
                    division.SupplyConsumption = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // fuelconsumption
                if (keyword.Equals("fuelconsumption"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "fuelconsumption", lexer);
                        continue;
                    }

                    // 燃料消費量
                    division.FuelConsumption = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // maxspeed
                if (keyword.Equals("maxspeed"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "maxspeed", lexer);
                        continue;
                    }

                    // 最大速度
                    division.MaxSpeed = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // speed_cap_art
                if (keyword.Equals("speed_cap_art"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "speed_cap_art", lexer);
                        continue;
                    }

                    // 砲兵速度キャップ
                    division.SpeedCapArt = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // speed_cap_eng
                if (keyword.Equals("speed_cap_eng"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "speed_cap_eng", lexer);
                        continue;
                    }

                    // 工兵速度キャップ
                    division.SpeedCapEng = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // speed_cap_aa
                if (keyword.Equals("speed_cap_aa"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "speed_cap_aa", lexer);
                        continue;
                    }

                    // 対空速度キャップ
                    division.SpeedCapAa = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // speed_cap_at
                if (keyword.Equals("speed_cap_at"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "speed_cap_at", lexer);
                        continue;
                    }

                    // 対戦車速度キャップ
                    division.SpeedCapAt = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // transportweight
                if (keyword.Equals("transportweight"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transportweight", lexer);
                        continue;
                    }

                    // 輸送負荷
                    division.TransportWeight = (double) d;
                    continue;
                }

                // transportcapability
                if (keyword.Equals("transportcapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "transportcapability", lexer);
                        continue;
                    }

                    // 輸送能力
                    division.TransportCapability = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // defensiveness
                if (keyword.Equals("defensiveness"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "defensiveness", lexer);
                        continue;
                    }

                    // 防御力
                    division.Defensiveness = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // toughness
                if (keyword.Equals("toughness"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "toughness", lexer);
                        continue;
                    }

                    // 耐久力
                    division.Toughness = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // softness
                if (keyword.Equals("softness"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "softness", lexer);
                        continue;
                    }

                    // 脆弱性
                    division.Softness = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // suppression
                if (keyword.Equals("suppression"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "suppression", lexer);
                        continue;
                    }

                    // 制圧力
                    division.Suppression = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // seadefence
                if (keyword.Equals("seadefence"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "seadefence", lexer);
                        continue;
                    }

                    // 対艦/対潜防御力
                    division.SeaDefense = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // surfacedefence
                if (keyword.Equals("surfacedefence"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "surfacedefence", lexer);
                        continue;
                    }

                    // 対地防御力
                    division.SurfaceDefence = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // airdefence
                if (keyword.Equals("airdefence"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airdefence", lexer);
                        continue;
                    }

                    // 対空防御力
                    division.AirDefence = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // softattack
                if (keyword.Equals("softattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "softattack", lexer);
                        continue;
                    }

                    // 対人攻撃力
                    division.SoftAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // hardattack
                if (keyword.Equals("hardattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "hardattack", lexer);
                        continue;
                    }

                    // 対甲攻撃力
                    division.HardAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // seaattack
                if (keyword.Equals("seaattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "seaattack", lexer);
                        continue;
                    }

                    // 対艦攻撃力(海軍)
                    division.SeaAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // subattack
                if (keyword.Equals("subattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "subattack", lexer);
                        continue;
                    }

                    // 対潜攻撃力
                    division.SubAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // convoyattack
                if (keyword.Equals("convoyattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "convoyattack", lexer);
                        continue;
                    }

                    // 通商破壊力
                    division.ConvoyAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // shorebombardment
                if (keyword.Equals("shorebombardment"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "shorebombardment", lexer);
                        continue;
                    }

                    // 沿岸砲撃能力
                    division.ShoreBombardment = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // airattack
                if (keyword.Equals("airattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airattack", lexer);
                        continue;
                    }

                    // 対空攻撃力
                    division.AirAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // strategicattack
                if (keyword.Equals("strategicattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "strategicattack", lexer);
                        continue;
                    }

                    // 戦略爆撃攻撃力
                    division.StrategicAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // navalattack
                if (keyword.Equals("navalattack"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "navalattack", lexer);
                        continue;
                    }

                    // 空対艦攻撃力
                    division.NavalAttack = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // artillery_bombardment
                if (keyword.Equals("artillery_bombardment"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "artillery_bombardment", lexer);
                        continue;
                    }

                    // 砲撃能力
                    division.ArtilleryBombardment = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // surfacedetectioncapability
                if (keyword.Equals("surfacedetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "surfacedetectioncapability", lexer);
                        continue;
                    }

                    // 対艦索敵能力
                    division.SurfaceDetection = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // airdetectioncapability
                if (keyword.Equals("airdetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "airdetectioncapability", lexer);
                        continue;
                    }

                    // 対空索敵能力
                    division.AirDetection = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // subdetectioncapability
                if (keyword.Equals("subdetectioncapability"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "subdetectioncapability", lexer);
                        continue;
                    }

                    // 対潜索敵能力
                    division.SubDetection = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // visibility
                if (keyword.Equals("visibility"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "visibility", lexer);
                        continue;
                    }

                    // 可視性
                    division.Visibility = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // range
                if (keyword.Equals("range"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "range", lexer);
                        continue;
                    }

                    // 航続距離
                    division.Range = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // distance
                if (keyword.Equals("distance"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "distance", lexer);
                        continue;
                    }

                    // 射程距離
                    division.Distance = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // travelled
                if (keyword.Equals("travelled"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "travelled", lexer);
                        continue;
                    }

                    // 移動距離
                    division.Travelled = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // locked
                if (keyword.Equals("locked"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant", lexer);
                        continue;
                    }

                    // 移動不可
                    division.Locked = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // dormant
                if (keyword.Equals("dormant"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "dormant", lexer);
                        continue;
                    }

                    // 休止状態
                    division.Dormant = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                if (lexer.LineNo != lastLineNo)
                {
                    // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                    lexer.ReserveToken(token);
                    break;
                }
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

            DivisionDevelopment division = new DivisionDevelopment();
            int lastLineNo = -1;
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
                        continue;
                    }

                    // typeとidの組
                    division.Id = id;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
                        continue;
                    }

                    // 師団名
                    division.Name = s;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // cost
                if (keyword.Equals("cost"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "cost", lexer);
                        continue;
                    }

                    // 必要IC
                    division.Cost = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "manpower", lexer);
                        continue;
                    }

                    // 必要人的資源
                    division.Manpower = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // unitcost
                if (keyword.Equals("unitcost"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "unitcost", lexer);
                        continue;
                    }

                    // unitcost
                    division.UnitCost = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // new_model
                if (keyword.Equals("new_model"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "unitcost", lexer);
                        continue;
                    }

                    // new_model
                    division.NewModel = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // date
                if (keyword.Equals("date"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "date", lexer);
                        continue;
                    }

                    // 完了予定日
                    division.Date = date;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // progress
                if (keyword.Equals("progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "progress", lexer);
                        continue;
                    }

                    // 進捗率増分
                    division.Progress = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "total_progress", lexer);
                        continue;
                    }

                    // 総進捗率
                    division.TotalProgress = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "gearing_bonus", lexer);
                        continue;
                    }

                    // 連続生産ボーナス
                    division.GearingBonus = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "size", lexer);
                        continue;
                    }

                    // 総生産数
                    division.Size = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "done", lexer);
                        continue;
                    }

                    // 生産完了数
                    division.Done = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days", lexer);
                        continue;
                    }

                    // 完了日数
                    division.Days = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "days_for_first", lexer);
                        continue;
                    }

                    // 1単位の完了日数
                    division.DaysForFirst = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // halted
                if (keyword.Equals("halted"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "halted", lexer);
                        continue;
                    }

                    // 停止中
                    division.Halted = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // close_when_finished
                if (keyword.Equals("close_when_finished"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "close_when_finished", lexer);
                        continue;
                    }

                    // 完了時にキューを削除するかどうか
                    division.CloseWhenFinished = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // waitingforclosure
                if (keyword.Equals("waitingforclosure"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "waitingforclosure", lexer);
                        continue;
                    }

                    // waitingforclosure (詳細不明)
                    division.WaitingForClosure = (bool) b;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // retooling_time
                if (keyword.Equals("retooling_time"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "retooling_time", lexer);
                        continue;
                    }

                    // 生産ライン準備時間
                    division.RetoolingTime = (double) d;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    UnitType? type = ParseDivisionType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "type", lexer);
                        continue;
                    }

                    // ユニット種類
                    division.Type = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "model", lexer);
                        continue;
                    }

                    // モデル番号
                    division.Model = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra
                if (keyword.Equals("extra"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra1", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra1 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra2", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra2 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra3", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra3 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra4", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra4 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
                {
                    UnitType? type = ParseBrigadeType(lexer);
                    if (type == null)
                    {
                        Log.InvalidClause(LogCategory, "extra5", lexer);
                        continue;
                    }

                    // 付属旅団のユニット種類
                    division.Extra5 = (UnitType) type;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model1", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model2", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model3", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model4", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "brigade_model5", lexer);
                        continue;
                    }

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) n;

                    // 最終解釈行を覚えておく
                    lastLineNo = lexer.LineNo;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                if (lexer.LineNo != lastLineNo)
                {
                    // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                    lexer.ReserveToken(token);
                    break;
                }
                lexer.SkipLine();
            }

            return division;
        }

        #endregion

        #region 任務

        /// <summary>
        ///     任務を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>任務</returns>
        private static Mission ParseMission(TextLexer lexer)
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

            Mission mission = new Mission();
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

                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (!Scenarios.MissionStrings.Contains(s))
                    {
                        // 無効なトークン
                        Log.InvalidToken(LogCategory, token, lexer);
                        continue;
                    }

                    // 任務の種類
                    mission.Type = (MissionType) Array.IndexOf(Scenarios.MissionStrings, s);
                    continue;
                }

                // target
                if (keyword.Equals("target"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "target", lexer);
                        continue;
                    }

                    // 対象プロヴィンス
                    mission.Target = (int) n;
                    continue;
                }

                // missionscope
                if (keyword.Equals("missionscope"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "missionscope", lexer);
                        continue;
                    }

                    // 対象範囲
                    mission.MissionScope = (int) n;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "percentage", lexer);
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
                        Log.InvalidClause(LogCategory, "night", lexer);
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
                        Log.InvalidClause(LogCategory, "day", lexer);
                        continue;
                    }

                    // 昼間遂行
                    mission.Day = (bool) b;
                    continue;
                }

                // tz
                if (keyword.Equals("tz"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "tz", lexer);
                        continue;
                    }

                    // 対象範囲
                    mission.TargetZone = (int) n;
                    continue;
                }

                // ac
                if (keyword.Equals("ac"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "ac", lexer);
                        continue;
                    }

                    // 船団攻撃
                    mission.AttackConvoy = (bool) b;
                    continue;
                }

                // org
                if (keyword.Equals("org"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "org", lexer);
                        continue;
                    }

                    // 組織率下限
                    mission.OrgLimit = (double) d;
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.InvalidSection(LogCategory, "startdate", lexer);
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
                        Log.InvalidSection(LogCategory, "enddate", lexer);
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
                        Log.InvalidClause(LogCategory, "task", lexer);
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
                        Log.InvalidClause(LogCategory, "location", lexer);
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

            Convoy convoy = new Convoy();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
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
                        Log.InvalidSection(LogCategory, "trade_id", lexer);
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
                        Log.InvalidClause(LogCategory, "istradeconvoy", lexer);
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
                        Log.InvalidClause(LogCategory, "transports", lexer);
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
                        Log.InvalidClause(LogCategory, "escorts", lexer);
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
                        Log.InvalidClause(LogCategory, "energy", lexer);
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
                        Log.InvalidClause(LogCategory, "metal", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidClause(LogCategory, "rare_materials", lexer);
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
                        Log.InvalidSection(LogCategory, "list", lexer);
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

            ConvoyDevelopment convoy = new ConvoyDevelopment();
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

                // id
                if (keyword.Equals("id"))
                {
                    TypeId id = ParseTypeId(lexer);
                    if (id == null)
                    {
                        Log.InvalidSection(LogCategory, "id", lexer);
                        continue;
                    }

                    // typeとidの組
                    convoy.Id = id;
                    continue;
                }

                // name
                if (keyword.Equals("name"))
                {
                    string s = ParseString(lexer);
                    if (s == null)
                    {
                        Log.InvalidClause(LogCategory, "name", lexer);
                        continue;
                    }

                    // 名前
                    convoy.Name = s;
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
                        string s = token.Value as string;
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
                        Log.InvalidClause(LogCategory, "location", lexer);
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
                        Log.InvalidClause(LogCategory, "cost", lexer);
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
                        Log.InvalidClause(LogCategory, "manpower", lexer);
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
                        Log.InvalidSection(LogCategory, "date", lexer);
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
                        Log.InvalidClause(LogCategory, "progress", lexer);
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
                        Log.InvalidClause(LogCategory, "total_progress", lexer);
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
                        Log.InvalidClause(LogCategory, "gearing_bonus", lexer);
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
                        Log.InvalidClause(LogCategory, "size", lexer);
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
                        Log.InvalidClause(LogCategory, "done", lexer);
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
                        Log.InvalidClause(LogCategory, "days", lexer);
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
                        Log.InvalidClause(LogCategory, "days_for_first", lexer);
                        continue;
                    }

                    // 1単位の完了日数
                    convoy.DaysForFirst = (int) n;
                    continue;
                }

                // halted
                if (keyword.Equals("halted"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "halted", lexer);
                        continue;
                    }

                    // 停止中
                    convoy.Halted = (bool) b;
                    continue;
                }

                // close_when_finished
                if (keyword.Equals("close_when_finished"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "close_when_finished", lexer);
                        continue;
                    }

                    // 完了時にキューを削除するかどうか
                    convoy.CloseWhenFinished = (bool) b;
                    continue;
                }

                // waitingforclosure
                if (keyword.Equals("waitingforclosure"))
                {
                    bool? b = ParseBool(lexer);
                    if (!b.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "waitingforclosure", lexer);
                        continue;
                    }

                    // waitingforclosure (詳細不明)
                    convoy.WaitingForClosure = (bool) b;
                    continue;
                }

                // retooling_time
                if (keyword.Equals("retooling_time"))
                {
                    double? d = ParseDouble(lexer);
                    if (!d.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "retooling_time", lexer);
                        continue;
                    }

                    // 生産ライン準備時間
                    convoy.RetoolingTime = (double) d;
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

            List<int> list = new List<int>();
            int lastLineNo = -1;
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
                    if (lexer.LineNo != lastLineNo)
                    {
                        // 現在行が最終解釈行と異なる場合、閉じ括弧が不足しているものと見なす
                        lexer.ReserveToken(token);
                        break;
                    }
                    lexer.SkipLine();
                    continue;
                }

                // ID
                list.Add((int) (double) token.Value);

                // 最終解釈行を覚えておく
                lastLineNo = lexer.LineNo;
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

            List<Country> list = new List<Country>();
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

                string name = token.Value as string;
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

            GameDate date = new GameDate();
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

                // year
                if (keyword.Equals("year"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "year", lexer);
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
                        int month = (int) (double) token.Value;
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
                        string name = token.Value as string;
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
                        Log.InvalidClause(LogCategory, "day", lexer);
                        continue;
                    }

                    // 30日の記載が数多くあるので[情報]レベルでエラー出力する
                    if (n == 30)
                    {
                        Log.Info("[Scenario] Out of range: {0} at day ({1} L{2})", n, lexer.FileName, lexer.LineNo);
                    }
                    else if (n < 0 || n > 30)
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
                        Log.InvalidClause(LogCategory, "day", lexer);
                        continue;
                    }

                    if (n < 0 || n >= 24)
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

            TypeId id = new TypeId();
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

                // type
                if (keyword.Equals("type"))
                {
                    int? n = ParseInt(lexer);
                    if (!n.HasValue)
                    {
                        Log.InvalidClause(LogCategory, "type", lexer);
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
                        Log.InvalidClause(LogCategory, "id", lexer);
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

        /// <summary>
        ///     フラグリストを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>フラグリスト</returns>
        private static Dictionary<string, string> ParseFlags(TextLexer lexer)
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

            Dictionary<string, string> flags = new Dictionary<string, string>();
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
                    int n = (int) (double) token.Value;
                    if (n == 0 || n == 1)
                    {
                        flags[keyword] = ObjectHelper.ToString(token.Value);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    continue;
                }

                if (token.Type == TokenType.Identifier)
                {
                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (s.Equals("yes") || s.Equals("no"))
                    {
                        flags[keyword] = ObjectHelper.ToString(token.Value);
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
                string s = token.Value as string;
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
                int n = (int) (double) token.Value;

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

            string name = token.Value as string;
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
        /// <returns>ユニット種類</returns>
        private static UnitType? ParseDivisionType(TextLexer lexer)
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
                return null;
            }

            // サポート外のユニット種類
            UnitType type = Units.StringMap[s];
            if (!Units.DivisionTypes.Contains(type))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }

            return type;
        }

        /// <summary>
        ///     旅団のユニット種類を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>ユニット種類</returns>
        private static UnitType? ParseBrigadeType(TextLexer lexer)
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
                return null;
            }

            // サポート外のユニット種類
            UnitType type = Units.StringMap[s];
            if (!Units.BrigadeTypes.Contains(type))
            {
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
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
            DepotsInc, // depots.inc
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

            // depots.inc
            if (name.Equals("depots.inc"))
            {
                return ScenarioFileKind.DepotsInc;
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