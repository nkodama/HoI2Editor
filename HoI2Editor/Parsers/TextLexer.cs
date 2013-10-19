using System;
using System.IO;
using System.Text;
using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     テキストファイルの字句解析クラス
    /// </summary>
    public class TextLexer : IDisposable
    {
        #region 内部フィールド

        /// <summary>
        ///     テキストファイルの読み込み用
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        ///     保留中のトークン
        /// </summary>
        private Token _token;

        /// <summary>
        /// 空白文字をスキップするかどうか
        /// </summary>
        private bool _skipWhiteSpace;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="fileName">解析対象のファイル名</param>
        /// <param name="skipWhiteSpace">空白文字をスキップするかどうか</param>
        public TextLexer(string fileName, bool skipWhiteSpace)
        {
            _skipWhiteSpace = skipWhiteSpace;

            Open(fileName);
        }

        /// <summary>
        ///     オブジェクト破棄時の処理
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        ///     ファイルを開く
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public void Open(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            _reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage));
        }

        /// <summary>
        ///     ファイルを閉じる
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        #endregion

        #region 字句解析

        /// <summary>
        ///     字句解析
        /// </summary>
        /// <returns>トークン</returns>
        public Token GetToken()
        {
            return Read();
        }

        /// <summary>
        ///     指定の種類のトークンを要求する
        /// </summary>
        /// <param name="type">要求するトークンの種類</param>
        /// <returns>次のトークンが要求する種類ならばそのトークンを、異なればnullを返す</returns>
        public bool WantToken(TokenType type)
        {
            Token token = Peek();

            if (token != null && token.Type == type)
            {
                Read();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     指定の種類の識別子トークンを要求する
        /// </summary>
        /// <param name="keyword">要求するキーワード名</param>
        /// <returns>要求する識別子trueを返す</returns>
        public bool WantIdentifier(string keyword)
        {
            Token token = Peek();

            if (token != null && token.Type == TokenType.Identifier && ((string) token.Value).Equals(keyword))
            {
                Read();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     先頭のトークンを解析し、読み込みポインタを移動する
        /// </summary>
        /// <returns></returns>
        private Token Read()
        {
            // 既に解析済みのトークンがあれば返す
            if (_token != null)
            {
                Token result = _token;
                _token = null;
                return result;
            }

            return Parse();
        }

        /// <summary>
        ///     先頭のトークンを解析し、読み込みポインタを移動しない
        /// </summary>
        /// <returns></returns>
        private Token Peek()
        {
            // 既に解析済みのトークンがあれば返す
            if (_token != null)
            {
                return _token;
            }

            _token = Parse();
            return _token;
        }

        /// <summary>
        ///     字句解析
        /// </summary>
        /// <returns>トークン</returns>
        private Token Parse()
        {
            int c = _reader.Peek();

            // ファイルの末尾ならばnullを返す
            if (c == -1)
            {
                return null;
            }

            // 空白文字とコメントを読み飛ばす
            if (_skipWhiteSpace)
            {
                while (true)
                {
                    // ファイルの末尾ならばnullを返す
                    if (c == -1)
                    {
                        return null;
                    }

                    // 空白文字を読み飛ばす
                    if (char.IsWhiteSpace((char)c))
                    {
                        _reader.Read();
                        c = _reader.Peek();
                        continue;
                    }

                    // #の後は行コメントとみなす
                    if (c == '#')
                    {
                        _reader.ReadLine();
                        c = _reader.Peek();
                        continue;
                    }

                    // 空白文字とコメント以外ならばトークンの先頭と解釈する
                    break;
                }
            }

            // 数字
            if (char.IsDigit((char) c) || c == '-')
            {
                return ParseNumber();
            }

            // 識別子
            if (char.IsLetter((char) c) || c == '_')
            {
                return ParseIdentifier();
            }

            // 文字列
            if (c == '"')
            {
                _reader.Read();
                return ParseString();
            }

            // 等号
            if (c == '=')
            {
                _reader.Read();
                return new Token {Type = TokenType.Equal};
            }

            // 開き波括弧
            if (c == '{')
            {
                _reader.Read();
                return new Token {Type = TokenType.OpenBrace};
            }

            // 閉じ波括弧
            if (c == '}')
            {
                _reader.Read();
                return new Token {Type = TokenType.CloseBrace};
            }

            if (!_skipWhiteSpace)
            {
                // 空白文字
                if (char.IsWhiteSpace((char)c))
                {
                    return ParseWhiteSpace();
                }

                // コメント開始
                if (c == '#')
                {
                    return ParseComment();
                }
            }

            // 無効な文字列
            return ParseInvalid();
        }

        /// <summary>
        ///     数字を解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseNumber()
        {
            var sb = new StringBuilder();
            bool point = false;

            int c = _reader.Peek();
            if (c == '-')
            {
                _reader.Read();
                sb.Append((char) c);
            }

            while (true)
            {
                c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 数字ならば読み進める
                if (char.IsDigit((char) c))
                {
                    _reader.Read();
                    sb.Append((char) c);
                    continue;
                }

                // 小数点
                if (!point && c == '.')
                {
                    point = true;
                    _reader.Read();
                    sb.Append((char) c);
                    continue;
                }

                // 対象外の文字ならば抜ける
                break;
            }

            // 数字トークンを返す
            double d;
            if (double.TryParse(sb.ToString(), out d))
            {
                return new Token {Type = TokenType.Number, Value = d};
            }

            return new Token {Type = TokenType.Invalid, Value = sb.ToString()};
        }

        /// <summary>
        ///     識別子を解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseIdentifier()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 英文字または数字ならば読み進める
                if (char.IsLetter((char) c) || char.IsNumber((char) c) || c == '_')
                {
                    _reader.Read();
                    sb.Append((char) c);
                    continue;
                }

                // 対象外の文字ならば抜ける
                break;
            }

            return new Token {Type = TokenType.Identifier, Value = sb.ToString()};
        }

        /// <summary>
        ///     文字列を解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseString()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 引用符閉じ忘れのフールプループ
                // 改行を含む制御文字が現れれば抜ける
                if (char.IsControl((char) c))
                {
                    break;
                }

                // 引用符で文字列終了
                if (c == '"')
                {
                    _reader.Read();
                    break;
                }

                _reader.Read();
                sb.Append((char) c);
            }

            return new Token {Type = TokenType.String, Value = sb.ToString()};
        }

        /// <summary>
        ///     空白文字を解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseWhiteSpace()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 空白ならば読み進める
                if (char.IsWhiteSpace((char)c))
                {
                    _reader.Read();
                    sb.Append((char)c);
                    continue;
                }

                // 対象外の文字ならば抜ける
                break;
            }

            return new Token { Type = TokenType.WhiteSpace, Value = sb.ToString() };
        }

        /// <summary>
        ///     コメントを解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseComment()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 改行ならば読み込み終了
                if (c == '\r' || c == '\n')
                {
                    break;
                }

                _reader.Read();
                sb.Append((char)c);
            }

            return new Token { Type = TokenType.Comment, Value = sb.ToString() };
        }

        /// <summary>
        ///     無効トークンを解析する
        /// </summary>
        /// <returns>トークン</returns>
        private Token ParseInvalid()
        {
            var sb = new StringBuilder();

            while (true)
            {
                int c = _reader.Peek();

                // ファイルの末尾ならば読み込み終了
                if (c == -1)
                {
                    break;
                }

                // 他のトークンになり得る文字ならば読み込み終了
                if (char.IsWhiteSpace((char) c) ||
                    char.IsLetter((char) c) ||
                    char.IsDigit((char) c) ||
                    c == '"' ||
                    c == '=' ||
                    c == '{' ||
                    c == '}' ||
                    c == '_')
                {
                    break;
                }

                _reader.Read();
                sb.Append((char) c);
            }

            return new Token {Type = TokenType.Invalid, Value = sb.ToString()};
        }

        /// <summary>
        ///     行末まで読み飛ばす
        /// </summary>
        public void SkipLine()
        {
            _reader.ReadLine();
        }

        /// <summary>
        ///     指定種類のトークンまで読み飛ばす
        /// </summary>
        /// <param name="type"></param>
        public void SkipToToken(TokenType type)
        {
            while (true)
            {
                Token token = GetToken();
                if (token == null)
                {
                    return;
                }

                if (token.Type == type)
                {
                    return;
                }
            }
        }

        #endregion
    }
}