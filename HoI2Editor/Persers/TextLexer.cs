using System;
using System.IO;
using System.Text;

namespace HoI2Editor.Persers
{
    /// <summary>
    ///     テキストファイルの字句解析クラス
    /// </summary>
    public class TextLexer : IDisposable
    {
        /// <summary>
        ///     テキストファイルの読み込み用
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="fileName">解析対象のファイル名</param>
        public TextLexer(string fileName)
        {
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

            _reader = new StreamReader(fileName);
        }

        /// <summary>
        ///     ファイルを閉じる
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        /// <summary>
        ///     字句解析
        /// </summary>
        /// <returns>トークン</returns>
        public Token Parse()
        {
            int c;

            // 空白文字とコメントを読み飛ばす
            while (true)
            {
                c = _reader.Peek();

                // ファイルの末尾ならばnullを返す
                if (c == -1)
                {
                    return null;
                }

                // 空白文字を読み飛ばす
                if (char.IsWhiteSpace((char) c))
                {
                    _reader.Read();
                    continue;
                }

                // #の後は行コメントとみなす
                if (c == '#')
                {
                    _reader.ReadLine();
                    continue;
                }

                // 空白文字とコメント以外ならばトークンの先頭と解釈する
                break;
            }

            // 数字
            if (char.IsDigit((char) c) || c == '-')
            {
                return ParseNumber();
            }

            // 識別子
            if (char.IsLetter((char) c))
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

                // 英文字ならば読み進める
                if (char.IsLetter((char) c))
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
                    c == '}')
                {
                    break;
                }

                _reader.Read();
                sb.Append((char) c);
            }

            return new Token {Type = TokenType.Invalid, Value = sb.ToString()};
        }
    }
}