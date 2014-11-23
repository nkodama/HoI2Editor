using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoI2Editor.Models;

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

        /// <summary>
        ///     シナリオファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <returns>構文解析の成否</returns>
        public static bool Parse(string fileName, Scenario scenario)
        {
            _fileName = Path.GetFileName(fileName);
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

                        // シナリオ名
                        scenario.Name = token.Value as string;
                        continue;
                    }

                    // panel
                    if (keyword.Equals("panel"))
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

                        // パネル画像名
                        scenario.PanelName = token.Value as string;
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
                        scenario.History.AddRange(list);
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
                        scenario.History.AddRange(list);
                        continue;
                    }

                    // event
                    if (keyword.Equals("event"))
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

                        // イベントファイル
                        var name = token.Value as string;
                        scenario.Events.Add(name);
                        continue;
                    }

                    // include
                    if (keyword.Equals("include"))
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

                        // インクルードファイル
                        var name = token.Value as string;
                        scenario.Includes.Add(name);

                        // インクルードファイルを解釈する
                        string pathName = Game.GetReadFileName(name);
                        if (!File.Exists(pathName))
                        {
                            Log.Warning("[Scenario] Not exist include file: {0}", name);
                            continue;
                        }
                        FileNameStack.Push(pathName);
                        Parse(pathName, scenario);
                        _fileName = FileNameStack.Pop();
                        continue;
                    }

                    // province
                    if (keyword.Equals("province"))
                    {
                        ScenarioProvince province = ParseProvince(lexer);
                        if (province == null)
                        {
                            Log.InvalidSection(LogCategory, "province");
                            continue;
                        }

                        // プロヴィンス情報
                        switch (GetScenarioFileKind())
                        {
                            case ScenarioFileKind.BasesInc:
                                scenario.BasesProvinces.Add(province);
                                break;

                            case ScenarioFileKind.BasesDodInc:
                                scenario.BasesDodProvinces.Add(province);
                                break;

                            case ScenarioFileKind.VpInc:
                                scenario.VpProvinces.Add(province);
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
                        ScenarioCountry country = ParseCountry(lexer);
                        if (country == null)
                        {
                            Log.InvalidSection(LogCategory, "country");
                            continue;
                        }

                        // 国家情報
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

                    // シナリオヘッダ名
                    header.Name = token.Value as string;
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

                    // ヘッダ開始日時
                    header.StartDate = date;
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
                    header.Selectable.AddRange(list);
                    continue;
                }

                // 国タグ
                string tagName = keyword.ToUpper();
                if (Countries.StringMap.ContainsKey(tagName))
                {
                    Country tag = Countries.StringMap[tagName];
                    if (Countries.Tags.Contains(tag))
                    {
                        MajorCountry info = ParseMajorCountry(lexer);
                        if (info == null)
                        {
                            Log.InvalidSection(LogCategory, tagName);
                            continue;
                        }

                        // 主要国情報
                        info.Country = tag;
                        header.Majors.Add(info);
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

                    // 同盟国情報
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

                    // 戦争情報
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

                    // 外交協定情報
                    data.Treaties.Add(treaty);
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
                    data.DormantLeaders.AddRange(list);
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

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return data;
        }

        /// <summary>
        ///     主要国情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>主要国情報</returns>
        private static MajorCountry ParseMajorCountry(TextLexer lexer)
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

            var major = new MajorCountry();
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

                // picture
                if (keyword.Equals("picture"))
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

                    // プロパガンダ画像名
                    major.PictureName = token.Value as string;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return major;
        }

        /// <summary>
        ///     同盟国情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>同盟国情報</returns>
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

            var info = new Alliance();
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
                    info.Id = id;
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
                    info.Participant.AddRange(list);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     戦争情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>戦争情報</returns>
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

            var info = new War();
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
                    info.Id = id;
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
                    info.StartDate = date;
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
                    info.EndDate = date;
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
                    info.Attackers = alliance;
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
                    info.Defenders = alliance;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     外交協定情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交協定情報</returns>
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

                    var typeName = token.Value as string;
                    if (string.IsNullOrEmpty(typeName))
                    {
                        continue;
                    }
                    typeName = typeName.ToLower();

                    // non_aggression
                    if (typeName.Equals("non_aggression"))
                    {
                        treaty.Type = TreatyType.NonAggression;
                        continue;
                    }

                    // peace
                    if (typeName.Equals("peace"))
                    {
                        treaty.Type = TreatyType.Peace;
                        continue;
                    }

                    // trade
                    if (typeName.Equals("trade"))
                    {
                        treaty.Type = TreatyType.Trade;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // country
                if (keyword.Equals("country"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 対象国
                            if (treaty.Country1 == Country.None)
                            {
                                treaty.Country1 = tag;
                            }
                            else if (treaty.Country2 == Country.None)
                            {
                                treaty.Country2 = tag;
                            }
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // 資金
                    treaty.Money = (double) token.Value;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
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

                    // 物資
                    treaty.Supplies = (double) token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
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

                    // エネルギー
                    treaty.Energy = (double) token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
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

                    // 金属
                    treaty.Metal = (double) token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
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

                    // 希少資源
                    treaty.RareMaterials = (double) token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
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

                    // 石油
                    treaty.Oil = (double) token.Value;
                    continue;
                }

                // cancel
                if (keyword.Equals("cancel"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        treaty.Cancel = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        treaty.Cancel = false;
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

            return treaty;
        }

        /// <summary>
        ///     プロヴィンス情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>プロヴィンス情報</returns>
        private static ScenarioProvince ParseProvince(TextLexer lexer)
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

            var province = new ScenarioProvince();
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

                    // プロヴィンスID
                    province.Id = (int) (double) token.Value;
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

                    // 物資の備蓄量
                    province.SupplyPool = (double) token.Value;
                    continue;
                }

                // oilpool
                if (keyword.Equals("oilpool"))
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

                    // 石油の備蓄量
                    province.OilPool = (double) token.Value;
                    continue;
                }

                // energypool
                if (keyword.Equals("energypool"))
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

                    // エネルギーの備蓄量
                    province.EnergyPool = (double) token.Value;
                    continue;
                }

                // metalpool
                if (keyword.Equals("metalpool"))
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

                    // 金属の備蓄量
                    province.MetalPool = (double) token.Value;
                    continue;
                }

                // rarematerialspool
                if (keyword.Equals("rarematerialspool"))
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

                    // 希少資源の備蓄量
                    province.RareMaterialsPool = (double) token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
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

                    // エネルギー産出量
                    province.Energy = (double) token.Value;
                    continue;
                }

                // max_energy
                if (keyword.Equals("max_energy"))
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

                    // 最大エネルギー産出量
                    province.MaxEnergy = (double) token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
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

                    // 金属産出量
                    province.Metal = (double) token.Value;
                    continue;
                }

                // max_metal
                if (keyword.Equals("max_metal"))
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

                    // 最大金属産出量
                    province.MaxMetal = (double) token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
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

                    // 希少資源産出量
                    province.RareMaterials = (double) token.Value;
                    continue;
                }

                // max_rare_materials
                if (keyword.Equals("max_rare_materials"))
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

                    // 最大希少資源産出量
                    province.MaxRareMaterials = (double) token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
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

                    // 石油産出量
                    province.Oil = (double) token.Value;
                    continue;
                }

                // max_oil
                if (keyword.Equals("max_oil"))
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

                    // 最大石油産出量
                    province.MaxOil = (double) token.Value;
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

                    // 人的資源
                    province.Manpower = (double) token.Value;
                    continue;
                }

                // max_manpower
                if (keyword.Equals("max_manpower"))
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

                    // 最大人的資源
                    province.MaxManpower = (double) token.Value;
                    continue;
                }

                // points
                if (keyword.Equals("points"))
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

                    // 勝利ポイント
                    province.Vp = (int) (double) token.Value;
                    continue;
                }

                // province_revoltrisk
                if (keyword.Equals("province_revoltrisk"))
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

                    // 反乱率
                    province.RevoltRisk = (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return province;
        }

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

                    var s = (double) token.Value;
                    if (s < 0)
                    {
                        Log.OutOfRange(LogCategory, "size", token, lexer);
                        continue;
                    }

                    // 最大サイズ
                    size.MaxSize = (double) token.Value;
                    continue;
                }

                // current_size
                if (keyword.Equals("current_size"))
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

                    var s = (double) token.Value;
                    if (s < 0)
                    {
                        Log.OutOfRange(LogCategory, "current_size", token, lexer);
                        continue;
                    }

                    // 現在サイズ
                    size.CurrentSize = s;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return size;
        }

        /// <summary>
        ///     国家情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国家情報</returns>
        private static ScenarioCountry ParseCountry(TextLexer lexer)
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

            var country = new ScenarioCountry();
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 国タグ
                            country.Country = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // regular_id
                if (keyword.Equals("regular_id"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 兄弟国
                            country.RegularId = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // intrinsic_gov_type
                if (keyword.Equals("intrinsic_gov_type"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    if (Scenarios.GovernmentStrings.Contains(s))
                    {
                        // 独立可能政体
                        country.IntrinsicGovType = (GovernmentType) Array.IndexOf(Scenarios.GovernmentStrings, s);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // puppet
                if (keyword.Equals("puppet"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 宗主国
                            country.Master = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // control
                if (keyword.Equals("control"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 統帥権取得国
                            country.Control = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // belligerence
                if (keyword.Equals("belligerence"))
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

                    // 好戦性
                    country.Belligerence = (int) (double) token.Value;
                    continue;
                }

                // extra_tc
                if (keyword.Equals("extra_tc"))
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

                    // 追加輸送能力
                    country.ExtraTc = (double) token.Value;
                    continue;
                }

                // dissent
                if (keyword.Equals("dissent"))
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

                    // 国民不満度
                    country.Dissent = (double) token.Value;
                    continue;
                }

                // capital
                if (keyword.Equals("capital"))
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

                    // 首都のプロヴィンスID
                    country.Capital = (int) (double) token.Value;
                    continue;
                }

                // peacetime_ic_mod
                if (keyword.Equals("peacetime_ic_mod"))
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

                    // 平時のIC補正
                    country.PeaceTimeIcMod = (double) token.Value;
                    continue;
                }

                // ground_def_eff
                if (keyword.Equals("ground_def_eff"))
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

                    // 対地防御補正
                    country.GroundDefEff = (double) token.Value;
                    continue;
                }

                // ai
                if (keyword.Equals("ai"))
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

                    // AIファイル名
                    country.Ai = token.Value as string;
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

                    // 人的資源
                    country.Manpower = (double) token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
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

                    // エネルギー
                    country.Energy = (double) token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
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

                    // 金属
                    country.Metal = (double) token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
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

                    // 希少資源
                    country.RareMaterials = (double) token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
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

                    // 石油
                    country.Oil = (double) token.Value;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
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

                    // 物資
                    country.Supplies = (double) token.Value;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
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

                    // 資金
                    country.Money = (double) token.Value;
                    continue;
                }

                // transports
                if (keyword.Equals("transports"))
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

                    // 輸送船団
                    country.Transports = (int) (double) token.Value;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
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

                    // 護衛艦
                    country.Escorts = (int) (double) token.Value;
                    continue;
                }

                // free
                if (keyword.Equals("free"))
                {
                    FreeResources free = ParseFree(lexer);
                    if (free == null)
                    {
                        Log.InvalidSection(LogCategory, "free");
                        continue;
                    }

                    // マップ外資源
                    country.Free = free;
                    continue;
                }

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    IEnumerable<CountryRelation> list = ParseDiplomacy(lexer);
                    if (list == null)
                    {
                        Log.InvalidSection(LogCategory, "diplomacy");
                        continue;
                    }

                    // 外交情報
                    country.Diplomacy.AddRange(list);
                    continue;
                }

                // spyinfo
                if (keyword.Equals("spyinfo"))
                {
                    SpyInfo spyInfo = ParseSpyInfo(lexer);
                    if (spyInfo == null)
                    {
                        Log.InvalidSection(LogCategory, "spyinfo");
                        continue;
                    }

                    // 諜報情報
                    country.Intelligence.Add(spyInfo);
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

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return country;
        }

        /// <summary>
        ///     マップ外資源を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>マップ外資源</returns>
        private static FreeResources ParseFree(TextLexer lexer)
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

            var free = new FreeResources();
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

                    // 工業力
                    free.Ic = (double) token.Value;
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

                    // 人的資源
                    free.Manpower = (double) token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
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

                    // エネルギー
                    free.Energy = (double) token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
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

                    // 金属
                    free.Metal = (double) token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
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

                    // 希少資源
                    free.RareMaterials = (double) token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
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

                    // 石油
                    free.Oil = (double) token.Value;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
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

                    // 物資
                    free.Supplies = (double) token.Value;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
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

                    // 資金
                    free.Money = (double) token.Value;
                    continue;
                }

                // transport
                if (keyword.Equals("transport"))
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

                    // 輸送船団
                    free.Transports = (int) (double) token.Value;
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
                    if (token.Type != TokenType.Number)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 護衛艦
                    free.Escorts = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return free;
        }

        /// <summary>
        ///     外交情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交情報</returns>
        private static IEnumerable<CountryRelation> ParseDiplomacy(TextLexer lexer)
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

            var list = new List<CountryRelation>();
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
                    CountryRelation relation = ParseRelation(lexer);
                    if (relation == null)
                    {
                        Log.InvalidSection(LogCategory, "relation");
                        continue;
                    }

                    // 外交関係情報
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
        ///     外交関係情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交関係情報</returns>
        private static CountryRelation ParseRelation(TextLexer lexer)
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

            var relation = new CountryRelation();
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 国タグ
                            relation.Country = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // 関係値
                    relation.Value = (double) token.Value;
                    continue;
                }

                // access
                if (keyword.Equals("access"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        relation.Access = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        relation.Access = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

        /// <summary>
        ///     諜報情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>諜報情報</returns>
        private static SpyInfo ParseSpyInfo(TextLexer lexer)
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

            var spyInfo = new SpyInfo();
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 国タグ
                            spyInfo.Country = tag;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // numberofspies
                if (keyword.Equals("numberofspies"))
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

                    // スパイの数
                    spyInfo.Spies = (int) (double) token.Value;
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "democratic", token, lexer);
                        continue;
                    }

                    // 民主的 - 独裁的
                    policy.Democratic = value;
                    continue;
                }

                // political_left
                if (keyword.Equals("political_left"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "political_left", token, lexer);
                        continue;
                    }

                    // 政治的左派 - 政治的右派
                    policy.PoliticalLeft = value;
                    continue;
                }

                // freedom
                if (keyword.Equals("freedom"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "freedom", token, lexer);
                        continue;
                    }

                    // 開放社会 - 閉鎖社会
                    policy.Freedom = value;
                    continue;
                }

                // free_market
                if (keyword.Equals("free_market"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "free_market", token, lexer);
                        continue;
                    }

                    // 自由経済 - 中央計画経済
                    policy.FreeMarket = value;
                    continue;
                }

                // professional_army
                if (keyword.Equals("professional_army"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "professional_army", token, lexer);
                        continue;
                    }

                    // 常備軍 - 徴兵軍 (DH Fullでは動員 - 復員)
                    policy.ProfessionalArmy = value;
                    continue;
                }

                // defense_lobby
                if (keyword.Equals("defense_lobby"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "defense_lobby", token, lexer);
                        continue;
                    }

                    // タカ派 - ハト派
                    policy.DefenseLobby = value;
                    continue;
                }

                // interventionism
                if (keyword.Equals("interventionism"))
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

                    var value = (int) (double) token.Value;
                    if (value < 1 || value > 10)
                    {
                        Log.OutOfRange(LogCategory, "interventionism", token, lexer);
                        continue;
                    }

                    // 介入主義 - 孤立主義
                    policy.Interventionism = value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return policy;
        }

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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.IsTrade = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.IsTrade = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // transports
                if (keyword.Equals("transports"))
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

                    // 輸送船の数
                    convoy.Transports = (int) (double) token.Value;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
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

                    // 護衛艦の数
                    convoy.Escorts = (int) (double) token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.Energy = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.Energy = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.Metal = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.Metal = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.RareMaterials = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.RareMaterials = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.Oil = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.Oil = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        convoy.Supplies = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        convoy.Supplies = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // ユニット名
                    unit.Name = token.Value as string;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 統帥国
                            unit.Control = tag;
                            continue;
                        }
                    }
                }

                // leader
                if (keyword.Equals("leader"))
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

                    // 指揮官
                    unit.Leader = (int) (double) token.Value;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    unit.Location = (int) (double) token.Value;
                    continue;
                }

                // dig_in
                if (keyword.Equals("dig_in"))
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

                    // 塹壕レベル
                    unit.DigIn = (double) token.Value;
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

                    // ユニット名
                    unit.Name = token.Value as string;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 統帥国
                            unit.Control = tag;
                            continue;
                        }
                    }
                }

                // leader
                if (keyword.Equals("leader"))
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

                    // 指揮官
                    unit.Leader = (int) (double) token.Value;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    unit.Location = (int) (double) token.Value;
                    continue;
                }

                // base
                if (keyword.Equals("base"))
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

                    // 所属基地 (海軍/空軍)
                    unit.Base = (int) (double) token.Value;
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

                    // ユニット名
                    unit.Name = token.Value as string;
                    continue;
                }

                // leader
                if (keyword.Equals("leader"))
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

                    // 指揮官
                    unit.Leader = (int) (double) token.Value;
                    continue;
                }

                // control
                if (keyword.Equals("control"))
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

                    var tagName = token.Value as string;
                    if (string.IsNullOrEmpty(tagName))
                    {
                        continue;
                    }
                    tagName = tagName.ToUpper();

                    if (Countries.StringMap.ContainsKey(tagName))
                    {
                        Country tag = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(tag))
                        {
                            // 統帥国
                            unit.Control = tag;
                            continue;
                        }
                    }
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    unit.Location = (int) (double) token.Value;
                    continue;
                }

                // base
                if (keyword.Equals("base"))
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

                    // 所属基地 (海軍/空軍)
                    unit.Base = (int) (double) token.Value;
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

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return unit;
        }

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

                    // 師団名
                    division.Name = token.Value as string;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
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
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
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

                    // モデル番号
                    division.Model = (int) (double) token.Value;
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) (double) token.Value;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
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

                    // 最大戦力
                    division.MaxStrength = (int) (double) token.Value;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
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

                    // 戦力
                    division.Strength = (int) (double) token.Value;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
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

                    // 指揮統制率
                    division.Organisation = (int) (double) token.Value;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
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

                    // 経験値
                    division.Experience = (int) (double) token.Value;
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
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        division.Locked = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        division.Locked = false;
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

                    // 師団名
                    division.Name = token.Value as string;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
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
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
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

                    // モデル番号
                    division.Model = (int) (double) token.Value;
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // extra1
                if (keyword.Equals("extra1"))
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra1 = type;
                    continue;
                }

                // extra2
                if (keyword.Equals("extra2"))
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra2 = type;
                    continue;
                }

                // extra3
                if (keyword.Equals("extra3"))
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra3 = type;
                    continue;
                }

                // extra4
                if (keyword.Equals("extra4"))
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra4 = type;
                    continue;
                }

                // extra5
                if (keyword.Equals("extra5"))
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra5 = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) (double) token.Value;
                    continue;
                }

                // brigade_model1
                if (keyword.Equals("brigade_model1"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel1 = (int) (double) token.Value;
                    continue;
                }

                // brigade_model2
                if (keyword.Equals("brigade_model2"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel2 = (int) (double) token.Value;
                    continue;
                }

                // brigade_model3
                if (keyword.Equals("brigade_model3"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel3 = (int) (double) token.Value;
                    continue;
                }

                // brigade_model4
                if (keyword.Equals("brigade_model4"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel4 = (int) (double) token.Value;
                    continue;
                }

                // brigade_model5
                if (keyword.Equals("brigade_model5"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel5 = (int) (double) token.Value;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
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

                    // 最大戦力
                    division.MaxStrength = (int) (double) token.Value;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
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

                    // 戦力
                    division.Strength = (int) (double) token.Value;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
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

                    // 指揮統制率
                    division.Organisation = (int) (double) token.Value;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
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

                    // 経験値
                    division.Experience = (int) (double) token.Value;
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
                    division.MaxSpeed = (double) token.Value;
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
                    division.SeaDefense = (double) token.Value;
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
                    division.AirDefence = (double) token.Value;
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
                    division.SeaAttack = (double) token.Value;
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
                    division.SubAttack = (double) token.Value;
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
                    division.ConvoyAttack = (double) token.Value;
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
                    division.ShoreBombardment = (double) token.Value;
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
                    division.AirAttack = (double) token.Value;
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
                    division.Distance = (double) token.Value;
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
                    division.Visibility = (double) token.Value;
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
                    division.SurfaceDetectionCapability = (double) token.Value;
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
                    division.SubDetectionCapability = (double) token.Value;
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
                    division.AirDetectionCapability = (double) token.Value;
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

                    // 師団名
                    division.Name = token.Value as string;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
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
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
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

                    // モデル番号
                    division.Model = (int) (double) token.Value;
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) (double) token.Value;
                    continue;
                }

                // max_strength
                if (keyword.Equals("max_strength"))
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

                    // 最大戦力
                    division.MaxStrength = (int) (double) token.Value;
                    continue;
                }

                // strength
                if (keyword.Equals("strength"))
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

                    // 戦力
                    division.Strength = (int) (double) token.Value;
                    continue;
                }

                // organisation
                if (keyword.Equals("organisation"))
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

                    // 指揮統制率
                    division.Organisation = (int) (double) token.Value;
                    continue;
                }

                // experience
                if (keyword.Equals("experience"))
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

                    // 経験値
                    division.Experience = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (Scenarios.LandMissionStrings.Contains(s))
                    {
                        // 任務の種類
                        mission.Type = (LandMissionType) Array.IndexOf(Scenarios.LandMissionStrings, s);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // target
                if (keyword.Equals("target"))
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

                    // 対象プロヴィンス
                    mission.Target = (int) (double) token.Value;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
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

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) token.Value;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Night = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Night = false;
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Day = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Day = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // 任務
                    mission.Task = (int) (double) token.Value;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    mission.Location = (int) (double) token.Value;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (Scenarios.LandMissionStrings.Contains(s))
                    {
                        // 任務の種類
                        mission.Type = (NavalMissionType) Array.IndexOf(Scenarios.NavalMissionStrings, s);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // target
                if (keyword.Equals("target"))
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

                    // 対象プロヴィンス
                    mission.Target = (int) (double) token.Value;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
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

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) token.Value;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Night = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Night = false;
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Day = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Day = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // 任務
                    mission.Task = (int) (double) token.Value;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    mission.Location = (int) (double) token.Value;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (Scenarios.LandMissionStrings.Contains(s))
                    {
                        // 任務の種類
                        mission.Type = (AirMissionType) Array.IndexOf(Scenarios.AirMissionStrings, s);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // target
                if (keyword.Equals("target"))
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

                    // 対象プロヴィンス
                    mission.Target = (int) (double) token.Value;
                    continue;
                }

                // percentage
                if (keyword.Equals("percentage"))
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

                    // 戦力/指揮統制率下限
                    mission.Percentage = (double) token.Value;
                    continue;
                }

                // night
                if (keyword.Equals("night"))
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Night = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Night = false;
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

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();

                    // yes
                    if (s.Equals("yes"))
                    {
                        mission.Day = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        mission.Day = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
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

                    // 任務
                    mission.Task = (int) (double) token.Value;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    mission.Location = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return mission;
        }

        /// <summary>
        ///     生産中師団情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中師団情報</returns>
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

                    // 師団名
                    division.Name = token.Value as string;
                    continue;
                }

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
                    division.Cost = (double) token.Value;
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
                    division.Manpower = (double) token.Value;
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

                    // 進捗率増分
                    division.Progress = (double) token.Value;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
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

                    // 総進捗率
                    division.TotalProgress = (double) token.Value;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
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

                    // 連続生産ボーナス
                    division.GearingBonus = (double) token.Value;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
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

                    // 総生産数
                    division.Size = (int) (double) token.Value;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
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

                    // 生産完了数
                    division.Done = (int) (double) token.Value;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
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

                    // 完了日数
                    division.Days = (int) (double) token.Value;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
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

                    // 1単位の完了日数
                    division.DaysForFirst = (int) (double) token.Value;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
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
                    division.Type = type;
                    continue;
                }

                // model
                if (keyword.Equals("model"))
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

                    // モデル番号
                    division.Model = (int) (double) token.Value;
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

                    var s = token.Value as string;
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

                    // 付属旅団のユニット種類
                    division.Extra = type;
                    continue;
                }

                // brigade_model
                if (keyword.Equals("brigade_model"))
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

                    // 付属旅団のモデル番号
                    division.BrigadeModel = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return division;
        }

        /// <summary>
        ///     生産中輸送船団情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中輸送船団情報</returns>
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
                    if (token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 師団名
                    convoy.Name = token.Value as string;
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    convoy.Location = (int) (double) token.Value;
                    continue;
                }

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
                    convoy.Cost = (double) token.Value;
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
                    convoy.Manpower = (double) token.Value;
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

                    // 進捗率増分
                    convoy.Progress = (double) token.Value;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
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

                    // 総進捗率
                    convoy.TotalProgress = (double) token.Value;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
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

                    // 連続生産ボーナス
                    convoy.GearingBonus = (double) token.Value;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
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

                    // 総生産数
                    convoy.Size = (int) (double) token.Value;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
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

                    // 生産完了数
                    convoy.Done = (int) (double) token.Value;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
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

                    // 完了日数
                    convoy.Days = (int) (double) token.Value;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
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

                    // 1単位の完了日数
                    convoy.DaysForFirst = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return convoy;
        }

        /// <summary>
        ///     生産中建物情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>生産中建物情報</returns>
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
                    if (token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 師団名
                    building.Name = token.Value as string;
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
                        lexer.SkipLine();
                        continue;
                    }

                    var s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    s = s.ToLower();

                    if (Scenarios.BuildingStrings.Contains(s))
                    {
                        // 建物の種類
                        building.Type = (BuildingType) Array.IndexOf(Scenarios.BuildingStrings, s);
                        continue;
                    }

                    // 無効なトークン
                    Log.InvalidToken(LogCategory, token, lexer);
                    lexer.SkipLine();
                    continue;
                }

                // location
                if (keyword.Equals("location"))
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

                    // 位置
                    building.Location = (int) (double) token.Value;
                    continue;
                }

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
                    building.Cost = (double) token.Value;
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
                    building.Manpower = (double) token.Value;
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

                    // 進捗率増分
                    building.Progress = (double) token.Value;
                    continue;
                }

                // total_progress
                if (keyword.Equals("total_progress"))
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

                    // 総進捗率
                    building.TotalProgress = (double) token.Value;
                    continue;
                }

                // gearing_bonus
                if (keyword.Equals("gearing_bonus"))
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

                    // 連続生産ボーナス
                    building.GearingBonus = (double) token.Value;
                    continue;
                }

                // size
                if (keyword.Equals("size"))
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

                    // 総生産数
                    building.Size = (int) (double) token.Value;
                    continue;
                }

                // done
                if (keyword.Equals("done"))
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

                    // 生産完了数
                    building.Done = (int) (double) token.Value;
                    continue;
                }

                // days
                if (keyword.Equals("days"))
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

                    // 完了日数
                    building.Days = (int) (double) token.Value;
                    continue;
                }

                // days_for_first
                if (keyword.Equals("days_for_first"))
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

                    // 1単位の完了日数
                    building.DaysForFirst = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return building;
        }

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
                    lexer.SkipLine();
                    continue;
                }

                var tagName = token.Value as string;
                if (string.IsNullOrEmpty(tagName))
                {
                    continue;
                }
                tagName = tagName.ToUpper();

                if (Countries.StringMap.ContainsKey(tagName))
                {
                    Country tag = Countries.StringMap[tagName];
                    if (Countries.Tags.Contains(tag))
                    {
                        // 国タグ
                        list.Add(tag);
                        continue;
                    }
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
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

                    // 年
                    date.Year = (int) (double) token.Value;
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
                            Log.OutOfRange(LogCategory, "month", token, lexer);
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

                        int month = Array.IndexOf(Scenarios.MonthStrings, name);
                        if (month < 0)
                        {
                            Log.InvalidToken(LogCategory, token, lexer);
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

                    var day = (int) (double) token.Value;
                    if (day < 0 || day >= 30)
                    {
                        Log.OutOfRange(LogCategory, "day", token, lexer);
                    }

                    // 日
                    date.Day = day + 1;
                    continue;
                }

                // hour
                if (keyword.Equals("hour"))
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

                    var hour = (int) (double) token.Value;
                    if (hour < 0 || hour >= 24)
                    {
                        Log.OutOfRange(LogCategory, "hour", token, lexer);
                    }

                    // 時
                    date.Hour = hour;
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

                    // type
                    id.Type = (int) (double) token.Value;
                    continue;
                }

                // id
                if (keyword.Equals("id"))
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

                    // id
                    id.Id = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return id;
        }

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
            // ディレクトリ名がscenariosの場合は最上位のファイルのみなす
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