using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ランダム指揮官名を保持するクラス
    /// </summary>
    public class RandomLeaders
    {
        #region 内部フィールド

        /// <summary>
        ///     ランダム指揮官名リスト
        /// </summary>
        private static readonly List<string>[] Items = new List<string>[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     国家ごとの編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        ///     現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

        #endregion

        #region 内部定数

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     ランダム指揮官名定義ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     ランダム指揮官名定義ファイル群を再読み込みする
        /// </summary>
        public static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            Load();
        }

        /// <summary>
        ///     ランダム指揮官名定義ファイルを読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // ランダム指揮官名リストをクリアする
            foreach (Country country in Countries.Tags)
            {
                Items[(int) country] = null;
            }

            // ランダム指揮官名定義ファイルが存在しなければ戻る
            string fileName = Game.GetReadFileName(Game.RandomLeadersPathName);
            if (!File.Exists(fileName))
            {
                return;
            }

            // ランダム指揮官名定義ファイルを読み込む
            LoadFile(fileName);

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     ランダム指揮官名定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void LoadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

                while (!reader.EndOfStream)
                {
                    ParseLine(reader.ReadLine());
                    _currentLineNo++;
                }
            }
        }

        /// <summary>
        ///     ランダム指揮官名定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        private static void ParseLine(string line)
        {
            // 空行を読み飛ばす
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string[] tokens = line.Split(CsvSeparator);

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != 2)
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                // 余分な項目がある場合は解析を続ける
                if (tokens.Length < 2)
                {
                    return;
                }
            }

            // 国タグ
            string countryName = tokens[0].ToUpper();
            if (!Countries.StringMap.ContainsKey(countryName))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidCountryTag, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                return;
            }
            Country country = Countries.StringMap[countryName];

            // ランダム指揮官名
            string name = tokens[1];
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            // ランダム指揮官名を追加する
            AddName(name, country);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     ランダム指揮官名定義ファイルを保存する
        /// </summary>
        public static void Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return;
            }

            // dbフォルダがなければ作成する
            string folderName = Game.GetWriteFileName(Game.DatabasePathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Game.GetWriteFileName(Game.RandomLeadersPathName);
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 1;

                foreach (Country country in Countries.Tags.Where(country => Items[(int) country] != null))
                {
                    foreach (string name in Items[(int) country])
                    {
                        try
                        {
                            writer.WriteLine("{0};{1}", Countries.Strings[(int) country], name);
                        }
                        catch (Exception)
                        {
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                        }
                        _currentLineNo++;
                    }
                }
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();
        }

        #endregion

        #region ランダム指揮官名操作

        /// <summary>
        ///     ランダム指揮官名リストを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>ランダム指揮官名リスト</returns>
        public static List<string> GetNames(Country country)
        {
            return Items[(int) country] ?? new List<string>();
        }

        /// <summary>
        ///     ランダム指揮官名を追加する
        /// </summary>
        /// <param name="name">ランダム指揮官名</param>
        /// <param name="country">国タグ</param>
        public static void AddName(string name, Country country)
        {
            // 未登録の場合はリストを作成する
            if (Items[(int) country] == null)
            {
                Items[(int) country] = new List<string>();
            }

            // ランダム指揮官名を追加する
            Items[(int) country].Add(name);
        }

        /// <summary>
        ///     ランダム指揮官名リストを設定する
        /// </summary>
        /// <param name="names">ランダム指揮官名リスト</param>
        /// <param name="country">国タグ</param>
        public static void SetNames(List<string> names, Country country)
        {
            // ランダム指揮官名リストに変更がなければ戻る
            if (Items[(int) country] != null && names.SequenceEqual(Items[(int) country]))
            {
                return;
            }

            // ランダム指揮官名リストを設定する
            Items[(int) country] = names;

            // 編集済みフラグを設定する
            SetDirty(country);
        }

        /// <summary>
        ///     ランダム指揮官名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="country">国タグ</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void Replace(string s, string t, Country country, bool regex)
        {
            // 未登録ならば何もしない
            if (Items[(int) country] == null)
            {
                return;
            }

            List<string> names =
                Items[(int) country].Select(name => regex ? Regex.Replace(name, s, t) : name.Replace(s, t)).ToList();
            SetNames(names, country);
        }

        /// <summary>
        ///     全てのランダム指揮官名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAll(string s, string t, bool regex)
        {
            foreach (Country country in Countries.Tags)
            {
                Replace(s, t, country, regex);
            }
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(Country country)
        {
            return DirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirty(Country country)
        {
            DirtyFlags[(int) country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public static void ResetDirtyAll()
        {
            foreach (Country country in Enum.GetValues(typeof (Country)))
            {
                DirtyFlags[(int) country] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }
}