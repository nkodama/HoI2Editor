using System.Collections.Generic;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     技術データの構文解析クラス
    /// </summary>
    public static class TechParser
    {
        #region 内部定数

        /// <summary>
        ///     ログ出力時のカテゴリ名
        /// </summary>
        private const string LogCategory = "Tech";

        #endregion

        #region 構文解析

        /// <summary>
        ///     技術ファイルを構文解析する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>技術グループデータ</returns>
        public static TechGroup Parse(string fileName)
        {
            using (TextLexer lexer = new TextLexer(fileName, true))
            {
                Token token = lexer.GetToken();
                // 無効なトークン
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

                // technology
                if (s.Equals("technology"))
                {
                    TechGroup group = ParseTechnology(lexer);
                    if (group == null)
                    {
                        Log.InvalidSection(LogCategory, "technology", lexer);
                    }
                    return group;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                return null;
            }
        }

        /// <summary>
        ///     technologyセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>技術グループデータ</returns>
        private static TechGroup ParseTechnology(TextLexer lexer)
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

            TechGroup group = new TechGroup();
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
                    // =
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

                    // 技術グループID
                    group.Id = (int) (double) token.Value;
                    continue;
                }

                // category
                if (keyword.Equals("category"))
                {
                    // =
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

                    // 無効なカテゴリ文字列
                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    if (!Techs.CategoryMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 技術カテゴリ
                    group.Category = Techs.CategoryMap[s];
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
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 技術グループ名
                    group.Name = token.Value as string;
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
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 技術グループ説明
                    group.Desc = token.Value as string;
                    continue;
                }

                // label
                if (keyword.Equals("label"))
                {
                    TechLabel label = ParseLabel(lexer);
                    if (label == null)
                    {
                        Log.InvalidSection(LogCategory, "label", lexer);
                        continue;
                    }

                    // ラベル
                    group.Items.Add(label);
                    continue;
                }

                // event
                if (keyword.Equals("event"))
                {
                    TechEvent ev = ParseEvent(lexer);
                    if (ev == null)
                    {
                        Log.InvalidSection(LogCategory, "event", lexer);
                        continue;
                    }

                    // 技術イベント
                    group.Items.Add(ev);
                    continue;
                }

                // application
                if (keyword.Equals("application"))
                {
                    TechItem application = ParseApplication(lexer);
                    if (application == null)
                    {
                        Log.InvalidSection(LogCategory, "application", lexer);
                        continue;
                    }

                    // 技術
                    group.Items.Add(application);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return group;
        }

        /// <summary>
        ///     applicationセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>技術データ</returns>
        private static TechItem ParseApplication(TextLexer lexer)
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

            TechItem application = new TechItem();
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
                    // =
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

                    // 技術ID
                    application.Id = (int) (double) token.Value;
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
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 技術名
                    application.Name = token.Value as string;

                    // 短縮名
                    application.ShortName = "SHORT_" + application.Name;
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
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 技術説明
                    application.Desc = token.Value as string;
                    continue;
                }

                // position
                if (keyword.Equals("position"))
                {
                    TechPosition position = ParsePosition(lexer);
                    if (position == null)
                    {
                        Log.InvalidSection(LogCategory, "position", lexer);
                        continue;
                    }

                    // 座標リスト
                    application.Positions.Add(position);
                    continue;
                }

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

                    // 画像ファイル名
                    application.PictureName = token.Value as string;
                    continue;
                }

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

                    // 史実年
                    application.Year = (int) (double) token.Value;
                    continue;
                }

                // component
                if (keyword.Equals("component"))
                {
                    TechComponent component = ParseComponent(lexer);
                    if (component == null)
                    {
                        Log.InvalidSection(LogCategory, "component", lexer);
                        continue;
                    }

                    // 小研究
                    application.Components.Add(component);
                    continue;
                }

                // required
                if (keyword.Equals("required"))
                {
                    IEnumerable<int> ids = ParseRequired(lexer);
                    if (ids == null)
                    {
                        Log.InvalidSection(LogCategory, "required", lexer);
                        continue;
                    }

                    // 必要とする技術群(AND)
                    foreach (int id in ids)
                    {
                        RequiredTech tech = new RequiredTech { Id = id };
                        application.AndRequiredTechs.Add(tech);
                    }
                    continue;
                }

                // or_required
                if (keyword.Equals("or_required"))
                {
                    IEnumerable<int> ids = ParseRequired(lexer);
                    if (ids == null)
                    {
                        Log.InvalidSection(LogCategory, "or_required", lexer);
                        continue;
                    }

                    // 必要とする技術群(OR)
                    foreach (int id in ids)
                    {
                        RequiredTech tech = new RequiredTech { Id = id };
                        application.OrRequiredTechs.Add(tech);
                    }
                    continue;
                }

                // effects
                if (keyword.Equals("effects"))
                {
                    IEnumerable<Command> commands = ParseEffects(lexer);
                    if (commands == null)
                    {
                        Log.InvalidSection(LogCategory, "effects", lexer);
                        continue;
                    }

                    // 技術効果
                    application.Effects.AddRange(commands);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return application;
        }

        /// <summary>
        ///     labelセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>技術ラベルデータ</returns>
        private static TechLabel ParseLabel(TextLexer lexer)
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

            TechLabel label = new TechLabel();
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
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 無効なトークン
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // タグ名
                    label.Name = token.Value as string;
                    continue;
                }

                // position
                if (keyword.Equals("position"))
                {
                    TechPosition position = ParsePosition(lexer);
                    if (position == null)
                    {
                        Log.InvalidSection(LogCategory, "position", lexer);
                        continue;
                    }

                    // 座標リスト
                    label.Positions.Add(position);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return label;
        }

        /// <summary>
        ///     eventセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>技術イベントデータ</returns>
        private static TechEvent ParseEvent(TextLexer lexer)
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

            TechEvent ev = new TechEvent();
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
                    // =
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

                    // 技術イベントID
                    ev.Id = (int) (double) token.Value;
                    continue;
                }

                // position
                if (keyword.Equals("position"))
                {
                    TechPosition position = ParsePosition(lexer);
                    if (position == null)
                    {
                        Log.InvalidSection(LogCategory, "position", lexer);
                        continue;
                    }

                    // 座標リスト
                    ev.Positions.Add(position);
                    continue;
                }

                // technology
                if (keyword.Equals("technology"))
                {
                    // =
                    token = lexer.GetToken();
                    if (token.Type != TokenType.Equal)
                    {
                        Log.InvalidSection(LogCategory, "technology", lexer);
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

                    // 技術ID
                    ev.TechId = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return ev;
        }

        /// <summary>
        ///     posotionセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>座標データ</returns>
        private static TechPosition ParsePosition(TextLexer lexer)
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

            TechPosition position = new TechPosition();
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
                    // =
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

                    // X座標
                    position.X = (int) (double) token.Value;
                    continue;
                }

                // y
                if (keyword.Equals("y"))
                {
                    // =
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

                    // Y座標
                    position.Y = (int) (double) token.Value;
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return position;
        }

        /// <summary>
        ///     componentセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>小研究データ</returns>
        private static TechComponent ParseComponent(TextLexer lexer)
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

            TechComponent component = new TechComponent();
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
                    // =
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

                    // 小研究ID
                    component.Id = (int) (double) token.Value;
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
                    if (token.Type != TokenType.Identifier && token.Type != TokenType.String)
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 小研究名
                    component.Name = token.Value as string;
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

                    // 無効な研究特性文字列
                    string s = token.Value as string;
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    s = s.ToLower();
                    if (!Techs.SpecialityStringMap.ContainsKey(s))
                    {
                        Log.InvalidToken(LogCategory, token, lexer);
                        lexer.SkipLine();
                        continue;
                    }

                    // 小研究特性
                    component.Speciality = Techs.SpecialityStringMap[s];
                    continue;
                }

                // difficulty
                if (keyword.Equals("difficulty"))
                {
                    // =
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

                    // 難易度
                    component.Difficulty = (int) (double) token.Value;
                    continue;
                }

                // double_time
                if (keyword.Equals("double_time"))
                {
                    // =
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
                        // 倍の時間を要するかどうか
                        component.DoubleTime = true;
                        continue;
                    }

                    if (s.Equals("no"))
                    {
                        // 倍の時間を要するかどうか
                        component.DoubleTime = false;
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

            return component;
        }

        /// <summary>
        ///     required/or_requiredセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>技術IDリスト</returns>
        private static IEnumerable<int> ParseRequired(TextLexer lexer)
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
                    continue;
                }

                list.Add((int) (double) token.Value);
            }

            return list;
        }

        /// <summary>
        ///     effectsセクションを構文解析する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>コマンドリスト</returns>
        private static IEnumerable<Command> ParseEffects(TextLexer lexer)
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

            List<Command> list = new List<Command>();
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

                // command
                if (keyword.Equals("command"))
                {
                    Command command = CommandParser.Parse(lexer);
                    if (command == null)
                    {
                        Log.InvalidSection(LogCategory, "command", lexer);
                        continue;
                    }
                    if (command.Type == CommandType.None)
                    {
                        continue;
                    }

                    // コマンド
                    list.Add(command);
                    continue;
                }

                // 無効なトークン
                Log.InvalidToken(LogCategory, token, lexer);
                lexer.SkipLine();
            }

            return list;
        }

        #endregion
    }
}