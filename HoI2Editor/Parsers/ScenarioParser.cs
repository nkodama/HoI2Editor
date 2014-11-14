using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    /// シナリオの構文解析クラス
    /// </summary>
    public static class ScenarioParser
    {
        #region 内部フィールド

        /// <summary>
        ///     解析中のファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        /// ファイル名のスタック
        /// </summary>
        private static readonly Stack<string> FileNameStack = new Stack<string>(); 

        #endregion

        #region 内部定数

        /// <summary>
        /// 月名文字列
        /// </summary>
        private static readonly string[] MonthNames =
        {
            "january",
            "february",
            "march",
            "april",
            "may",
            "june",
            "july",
            "august",
            "september",
            "october",
            "november",
            "december"
        };

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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.String)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.String)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                            Log.Warning("[Scenario] Parse failed: header section");
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
                            Log.Warning("[Scenario] Parse failed: globaldata section");
                            continue;
                        }

                        // シナリオグローバルデータ
                        scenario.GlobalData = data;
                        continue;
                    }

                    // event
                    if (keyword.Equals("event"))
                    {
                        // =
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Equal)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.String)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // イベントファイル
                        scenario.Events.Add(token.Value as string);
                        continue;
                    }

                    // include
                    if (keyword.Equals("include"))
                    {
                        // =
                        token = lexer.GetToken();
                        if (token.Type != TokenType.Equal)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 無効なトークン
                        token = lexer.GetToken();
                        if (token.Type != TokenType.String)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                            Log.Warning("[Scenario] Not exist include file: {0}", pathName);
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
                        ScenarioProvinceInfo info = ParseProvince(lexer);
                        if (info == null)
                        {
                            Log.Warning("[Scenario] Parse failed: province section");
                            continue;
                        }

                        // プロヴィンス情報
                        switch (GetScenarioFileKind())
                        {
                            case ScenarioFileKind.BasesInc:
                                scenario.BasesProvinces.Add(info);
                                break;

                            case ScenarioFileKind.BasesDodInc:
                                scenario.BasesDodProvinces.Add(info);
                                break;

                            case ScenarioFileKind.VpInc:
                                scenario.VpProvinces.Add(info);
                                break;

                            default:
                                scenario.CountryProvinces.Add(info);
                                break;

                        }
                        continue;
                    }

                    // country
                    if (keyword.Equals("country"))
                    {
                        ScenarioCountryInfo info = ParseCountry(lexer);
                        if (info == null)
                        {
                            Log.Warning("[Scenario] Parse failed: country section");
                            continue;
                        }

                        // 国家情報
                        scenario.Countries.Add(info);
                        continue;
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.String)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Parse failed: startdate section");
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
                        Log.Warning("[Scenario] Parse failed: selectable section");
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
                    Country country = Countries.StringMap[tagName];
                    if (Countries.Tags.Contains(country))
                    {
                        MajorCountryInfo info = ParseMajorCountry(lexer);
                        if (info == null)
                        {
                            Log.Warning("[Scenario] Parse failed: {0} section", tagName);
                            continue;
                        }
                    }
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Parse failed: startdate section");
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
                        Log.Warning("[Scenario] Parse failed: enddate section");
                        continue;
                    }

                    // 終了日時
                    data.EndDate = date;
                    continue;
                }

                // axis
                if (keyword.Equals("axis"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: axis section");
                        continue;
                    }

                    // 枢軸国
                    data.Axis = alliance;
                    continue;
                }

                // allies
                if (keyword.Equals("allies"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: allies section");
                        continue;
                    }

                    // 連合国
                    data.Allies = alliance;
                    continue;
                }

                // comintern
                if (keyword.Equals("comintern"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: comintern section");
                        continue;
                    }

                    // 共産国
                    data.Comintern = alliance;
                    continue;
                }

                // alliance
                if (keyword.Equals("alliance"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: alliance section");
                        continue;
                    }

                    // 同盟国情報
                    data.Alliances.Add(alliance);
                    continue;
                }

                // war
                if (keyword.Equals("war"))
                {
                    WarInfo war = ParseWar(lexer);
                    if (war == null)
                    {
                        Log.Warning("[Scenario] Parse failed: war section");
                        continue;
                    }

                    // 戦争情報
                    data.Wars.Add(war);
                    continue;
                }

                // treaty
                if (keyword.Equals("treaty"))
                {
                    TreatyInfo treaty = ParseTreaty(lexer);
                    if (treaty == null)
                    {
                        Log.Warning("[Scenario] Parse failed: treaty section");
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
                        Log.Warning("[Scenario] Parse failed: dormant_leaders section");
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
                        Log.Warning("[Scenario] Parse failed: dormant_ministers section");
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
                        Log.Warning("[Scenario] Parse failed: dormant_teams section");
                        continue;
                    }

                    // 休止研究機関
                    data.DormantTeams.AddRange(list);
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return data;
        }

        /// <summary>
        ///     主要国情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>主要国情報</returns>
        private static MajorCountryInfo ParseMajorCountry(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new MajorCountryInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.String)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // プロパガンダ画像名
                    info.PictureName = token.Value as string;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     同盟国情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>同盟国情報</returns>
        private static AllianceInfo ParseAlliance(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new AllianceInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Parse failed: id section");
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
                        Log.Warning("[Scenario] Parse failed: participant section");
                        continue;
                    }

                    // 参加国
                    info.Participant.AddRange(list);
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     戦争情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>戦争情報</returns>
        private static WarInfo ParseWar(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new WarInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Parse failed: id section");
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
                        Log.Warning("[Scenario] Parse failed: date section");
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
                        Log.Warning("[Scenario] Parse failed: enddate section");
                        continue;
                    }

                    // 終了日時
                    info.EndDate = date;
                    continue;
                }

                // attackers
                if (keyword.Equals("attackers"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: attackers section");
                        continue;
                    }

                    // 攻撃側参加国
                    info.Attackers = alliance;
                    continue;
                }

                // defenders
                if (keyword.Equals("defenders"))
                {
                    AllianceInfo alliance = ParseAlliance(lexer);
                    if (alliance == null)
                    {
                        Log.Warning("[Scenario] Parse failed: defenders section");
                        continue;
                    }

                    // 防御側参加国
                    info.Defenders = alliance;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     外交協定情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交協定情報</returns>
        private static TreatyInfo ParseTreaty(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new TreatyInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Parse failed: id section");
                        continue;
                    }

                    // typeとidの組
                    info.Id = id;
                    continue;
                }

                // type
                if (keyword.Equals("type"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        info.Type = TreatyType.NonAggression;
                        continue;
                    }

                    // peace
                    if (typeName.Equals("peace"))
                    {
                        info.Type = TreatyType.Peace;
                        continue;
                    }

                    // trade
                    if (typeName.Equals("trade"))
                    {
                        info.Type = TreatyType.Trade;
                        continue;
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Country country = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(country))
                        {
                            // 対象国
                            if (info.Country1 == Country.None)
                            {
                                info.Country1 = country;
                            }
                            else if (info.Country2 == Country.None)
                            {
                                info.Country2 = country;
                            }
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                    lexer.SkipLine();
                    continue;
                }

                // startdate
                if (keyword.Equals("startdate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 開始日時
                    info.StartDate = date;
                    continue;
                }

                // expirydate
                if (keyword.Equals("expirydate"))
                {
                    GameDate date = ParseDate(lexer);
                    if (date == null)
                    {
                        Log.Warning("[Scenario] Parse failed: expirydate section");
                        continue;
                    }

                    // 失効日時
                    info.ExpiryDate = date;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 資金
                    info.Money = (double) token.Value;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 物資
                    info.Supplies = (double)token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // エネルギー
                    info.Energy = (double)token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 金属
                    info.Metal = (double)token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 希少資源
                    info.RareMaterials = (double)token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 石油
                    info.Oil = (double)token.Value;
                    continue;
                }

                // cancel
                if (keyword.Equals("cancel"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        info.Cancel = true;
                        continue;
                    }

                    // no
                    if (s.Equals("no"))
                    {
                        info.Cancel = false;
                        continue;
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                    lexer.SkipLine();
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     プロヴィンス情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>プロヴィンス情報</returns>
        private static ScenarioProvinceInfo ParseProvince(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new ScenarioProvinceInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // プロヴィンスID
                    info.Id = (int) token.Value;
                    continue;
                }

                // ic
                if (keyword.Equals("ic"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // IC
                    info.Ic = size;
                    continue;
                }

                // infra
                if (keyword.Equals("infra"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // インフラ
                    info.Infrastructure = size;
                    continue;
                }

                // landfort
                if (keyword.Equals("landfort"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 陸上要塞
                    info.LandFort = size;
                    continue;
                }

                // coastalfort
                if (keyword.Equals("coastalfort"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 沿岸要塞
                    info.CoastalFort = size;
                    continue;
                }

                // anti_air
                if (keyword.Equals("anti_air"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 対空砲
                    info.AntiAir = size;
                    continue;
                }

                // air_base
                if (keyword.Equals("air_base"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 空軍基地
                    info.AirBase = size;
                    continue;
                }

                // naval_base
                if (keyword.Equals("naval_base"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 海軍基地
                    info.NavalBase = size;
                    continue;
                }

                // radar_station
                if (keyword.Equals("radar_station"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // レーダー基地
                    info.RadarStation = size;
                    continue;
                }

                // nuclear_reactor
                if (keyword.Equals("nuclear_reactor"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 原子炉
                    info.NuclearReactor = size;
                    continue;
                }

                // rocket_test
                if (keyword.Equals("rocket_test"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // ロケット試験場
                    info.RocketTest = size;
                    continue;
                }

                // synthetic_oil
                if (keyword.Equals("synthetic_oil"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 合成石油工場
                    info.SyntheticOil = size;
                    continue;
                }

                // synthetic_rares
                if (keyword.Equals("synthetic_rares"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 合成素材工場
                    info.SyntheticRares = size;
                    continue;
                }

                // nuclear_power
                if (keyword.Equals("nuclear_power"))
                {
                    BuildingSize size = ParseBuildingSize(lexer);
                    if (size == null)
                    {
                        Log.Warning("[Scenario] Parse failed: startdate section");
                        continue;
                    }

                    // 原子力発電所
                    info.NuclearPower = size;
                    continue;
                }

                // supplypool
                if (keyword.Equals("supplypool"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 物資の備蓄量
                    info.SupplyPool = (double)token.Value;
                    continue;
                }

                // oilpool
                if (keyword.Equals("oilpool"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 石油の備蓄量
                    info.OilPool = (double)token.Value;
                    continue;
                }

                // points
                if (keyword.Equals("points"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 勝利ポイント
                    info.Vp = (int)token.Value;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     国家情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>国家情報</returns>
        private static ScenarioCountryInfo ParseCountry(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var info = new ScenarioCountryInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Country country = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(country))
                        {
                            // 国タグ
                            info.Country = country;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Country country = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(country))
                        {
                            // 宗主国
                            info.Master = country;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Country country = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(country))
                        {
                            // 統帥権取得国
                            info.Control = country;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 好戦性
                    info.Belligerence = (int) token.Value;
                    continue;
                }

                // capital
                if (keyword.Equals("capital"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 首都のプロヴィンスID
                    info.Capital = (int)token.Value;
                    continue;
                }

                // manpower
                if (keyword.Equals("manpower"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 人的資源
                    info.Manpower = (double)token.Value;
                    continue;
                }

                // energy
                if (keyword.Equals("energy"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // エネルギー
                    info.Energy = (double)token.Value;
                    continue;
                }

                // metal
                if (keyword.Equals("metal"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 金属
                    info.Metal = (double)token.Value;
                    continue;
                }

                // rare_materials
                if (keyword.Equals("rare_materials"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 希少資源
                    info.RareMaterials = (double)token.Value;
                    continue;
                }

                // oil
                if (keyword.Equals("oil"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 石油
                    info.Oil = (double)token.Value;
                    continue;
                }

                // supplies
                if (keyword.Equals("supplies"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 物資
                    info.Supplies = (double)token.Value;
                    continue;
                }

                // money
                if (keyword.Equals("money"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 資金
                    info.Money = (double)token.Value;
                    continue;
                }

                // transports
                if (keyword.Equals("transports"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 輸送船団
                    info.Transports = (int)token.Value;
                    continue;
                }

                // escorts
                if (keyword.Equals("escorts"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 護衛艦
                    info.Escorts = (int)token.Value;
                    continue;
                }

                // diplomacy
                if (keyword.Equals("diplomacy"))
                {
                    IEnumerable<CountryRelationInfo> relations = ParseDiplomacy(lexer);
                    if (relations == null)
                    {
                        Log.Warning("[Scenario] Parse failed: diplomacy section");
                        continue;
                    }

                    // 外交情報
                    info.Diplomacy.AddRange(relations);
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return info;
        }

        /// <summary>
        ///     外交情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交情報</returns>
        private static IEnumerable<CountryRelationInfo> ParseDiplomacy(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var relations = new List<CountryRelationInfo>();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    CountryRelationInfo relation = ParseRelation(lexer);
                    if (relation == null)
                    {
                        Log.Warning("[Scenario] Parse failed: relation section");
                        continue;
                    }

                    // 外交関係情報
                    relations.Add(relation);
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return relations;
        }

        /// <summary>
        ///     外交関係情報を構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>外交関係情報</returns>
        private static CountryRelationInfo ParseRelation(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            var relation = new CountryRelationInfo();
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Country country = Countries.StringMap[tagName];
                        if (Countries.Tags.Contains(country))
                        {
                            // 国タグ
                            relation.Country = country;
                            continue;
                        }
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                    lexer.SkipLine();
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return relation;
        }

        /// <summary>
        ///     建物のサイズを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>建物のサイズ</returns>
        private static BuildingSize ParseBuildingSize(TextLexer lexer)
        {
            // =
            Token token = lexer.GetToken();
            if (token.Type != TokenType.Equal)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // 相対サイズ指定
            token = lexer.GetToken();
            if (token.Type == TokenType.Number)
            {
                return new BuildingSize { Size = (double)token.Value };
            }

            // {
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 現在サイズ
                    size.CurrentSize = (double)token.Value;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return size;
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                    lexer.SkipLine();
                    continue;
                }

                // ID
                list.Add((int) token.Value);
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Country country = Countries.StringMap[tagName];
                    if (Countries.Tags.Contains(country))
                    {
                        // 国タグ
                        list.Add(country);
                        continue;
                    }
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 年
                    date.Year = (int) token.Value;
                    continue;
                }

                // month
                if (keyword.Equals("month"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    token = lexer.GetToken();
                    if (token.Type == TokenType.Number)
                    {
                        // 無効なトークン
                        var month = (int)token.Value;
                        if (month < 0 || month > 11)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 月
                        date.Month = month;
                        continue;
                    }

                    if (token.Type == TokenType.Identifier)
                    {
                        // 無効なトークン
                        var name = token.Value as string;
                        if (string.IsNullOrEmpty(name))
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }
                        name = name.ToLower();
                        int month = Array.IndexOf(MonthNames, name);
                        if (month < 0 || month >= 12)
                        {
                            Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                            lexer.SkipLine();
                            continue;
                        }

                        // 月
                        date.Month = month;
                        continue;
                    }

                    // 無効なトークン
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }
                    var day = (int) token.Value;
                    if (day < 0 || day >= 30)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 日
                    date.Day = day;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                return null;
            }

            // {
            token = lexer.GetToken();
            if (token.Type != TokenType.OpenBrace)
            {
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                    Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
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
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // type
                    id.Type = (int)token.Value;
                    continue;
                }

                // id
                if (keyword.Equals("id"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Number)
                    {
                        Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                        lexer.SkipLine();
                        continue;
                    }

                    // id
                    id.Id = (int)token.Value;
                    continue;
                }

                // 無効なトークン
                Log.Warning("[Scenario] Invalid token: {0}", ObjectHelper.ToString(token.Value));
                lexer.SkipLine();
            }

            return id;
        }

        #endregion

        #region ファイル判別

        /// <summary>
        /// 解析中のシナリオファイルの種類を取得する
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

        /// <summary>
        /// シナリオファイルの種類
        /// </summary>
        public enum ScenarioFileKind
        {
            Normal, // その他
            Top, // 最上位のファイル
            BasesInc, // bases.inc
            BasesDodInc, // bases_DOD.inc
            VpInc // vp.inc
        }

        #endregion
    }
}
