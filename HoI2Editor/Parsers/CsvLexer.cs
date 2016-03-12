using System;
using System.IO;
using System.Text;
using HoI2Editor.Models;

namespace HoI2Editor.Parsers
{
    /// <summary>
    ///     CSVファイルの字句解析クラス
    /// </summary>
    internal class CsvLexer : IDisposable
    {
        #region 公開プロパティ

        /// <summary>
        ///     解析中のファイル名
        /// </summary>
        internal string PathName { get; private set; }

        /// <summary>
        ///     解析中のファイル名 (ディレクトリ除く)
        /// </summary>
        internal string FileName => Path.GetFileName(PathName);

        /// <summary>
        ///     解析中の行番号
        /// </summary>
        internal int LineNo { get; private set; }

        /// <summary>
        ///     ファイルの末尾に到達したかどうかを返す
        /// </summary>
        internal bool EndOfStream => _reader.EndOfStream;

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     テキストファイルの読み込み用
        /// </summary>
        private StreamReader _reader;

        #endregion

        #region 内部定数

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] Separator = { ';' };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="fileName">解析対象のファイル名</param>
        internal CsvLexer(string fileName)
        {
            Open(fileName);
        }

        /// <summary>
        ///     オブジェクト破棄時の処理
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     デストラクタ
        /// </summary>
        ~CsvLexer()
        {
            Dispose(false);
        }

        /// <summary>
        ///     オブジェクト破棄時の処理
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_reader != null)
            {
                Close();
            }
        }

        /// <summary>
        ///     ファイルを開く
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        internal void Open(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            // 既に開いているファイルがあれば閉じる
            if (_reader != null)
            {
                Close();
            }

            _reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage));

            PathName = fileName;
            LineNo = 0;
        }

        /// <summary>
        ///     ファイルを閉じる
        /// </summary>
        internal void Close()
        {
            _reader.Close();
            _reader = null;
        }

        #endregion

        #region 字句解析

        /// <summary>
        ///     字句解析
        /// </summary>
        /// <returns>トークン列</returns>
        internal string[] GetTokens()
        {
            // ファイルの末尾に到達したらnullを返す
            if (_reader.EndOfStream)
            {
                return null;
            }

            LineNo++;
            string line = _reader.ReadLine();

            // 空白行ならばnullを返す
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            // コメント行ならばnullを返す
            if (line[0] == '#')
            {
                return null;
            }

            return line.Split(Separator);
        }

        /// <summary>
        ///     1行読み飛ばす
        /// </summary>
        internal void SkipLine()
        {
            // ファイルの末尾に到達したら何もしない
            if (_reader.EndOfStream)
            {
                return;
            }

            LineNo++;
            _reader.ReadLine();
        }

        #endregion
    }
}