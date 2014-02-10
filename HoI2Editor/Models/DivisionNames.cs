using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     新規師団名を保持するクラス
    /// </summary>
    public static class DivisionNames
    {
        #region 内部フィールド

        /// <summary>
        ///     師団名リスト
        /// </summary>
        private static readonly List<string>[,] Items =
            new List<string>[Enum.GetValues(typeof (Branch)).Length, Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     兵科ごとの編集済みフラグ
        /// </summary>
        private static readonly bool[] BranchDirtyFlags = new bool[Enum.GetValues(typeof (Branch)).Length];

        /// <summary>
        ///     国家ごとの編集済みフラグ
        /// </summary>
        private static readonly bool[,] CountryDirtyFlags =
            new bool[Enum.GetValues(typeof (Branch)).Length, Enum.GetValues(typeof (Country)).Length];

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
        ///     師団名定義ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     師団名定義ファイル群を再読み込みする
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
        ///     師団名定義ファイルを読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // 師団名リストをクリアする
            foreach (
                Branch branch in Enum.GetValues(typeof (Branch)).Cast<Branch>().Where(branch => branch != Branch.None))
            {
                foreach (Country country in Countries.Tags)
                {
                    Items[(int) branch, (int) country] = null;
                }
            }

            bool error = false;

            // 陸軍師団名定義ファイルを読み込む
            string fileName = Game.GetReadFileName(Game.ArmyNamesPathName);
            if (File.Exists(fileName))
            {
                try
                {
                    LoadFile(Branch.Army, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Read error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            else
            {
                error = true;
            }

            // 海軍師団名定義ファイルを読み込む
            fileName = Game.GetReadFileName(Game.NavyNamesPathName);
            if (File.Exists(fileName))
            {
                try
                {
                    LoadFile(Branch.Navy, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Read error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            else
            {
                error = true;
            }

            // 空軍師団名定義ファイルを読み込む
            fileName = Game.GetReadFileName(Game.AirNamesPathName);
            if (File.Exists(fileName))
            {
                try
                {
                    LoadFile(Branch.Airforce, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Read error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            else
            {
                error = true;
            }

            // 読み込みに失敗していれば戻る
            if (error)
            {
                return;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     師団名定義ファイルを読み込む
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="fileName">ファイル名</param>
        private static void LoadFile(Branch branch, string fileName)
        {
            Debug.WriteLine(string.Format("[DivisionName] Load: {0}", Path.GetFileName(fileName)));

            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

                while (!reader.EndOfStream)
                {
                    ParseLine(branch, reader.ReadLine());
                    _currentLineNo++;
                }
            }
        }

        /// <summary>
        ///     師団名定義行を解釈する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="line">対象文字列</param>
        private static void ParseLine(Branch branch, string line)
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

            // 師団名
            string name = tokens[1];
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            // 師団名を追加する
            AddName(name, branch, country);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     師団名定義ファイルを保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            bool error = false;

            // 陸軍師団名定義ファイルを保存する
            if (IsDirty(Branch.Army))
            {
                string fileName = Game.GetWriteFileName(Game.ArmyNamesPathName);
                try
                {
                    SaveFile(Branch.Army, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Write error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }


            // 海軍師団名定義ファイルを保存する
            if (IsDirty(Branch.Navy))
            {
                string fileName = Game.GetWriteFileName(Game.NavyNamesPathName);
                try
                {
                    SaveFile(Branch.Navy, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Write error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            // 空軍師団名定義ファイルを保存する
            if (IsDirty(Branch.Airforce))
            {
                string fileName = Game.GetWriteFileName(Game.AirNamesPathName);
                try
                {
                    SaveFile(Branch.Airforce, fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Debug.WriteLine(string.Format("[DivisionName] Write error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                        Resources.EditorDivisionName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            // 保存に失敗していれば戻る
            if (error)
            {
                return false;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            return true;
        }

        /// <summary>
        ///     師団名定義ファイルを保存する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="fileName">ファイル名</param>
        private static void SaveFile(Branch branch, string fileName)
        {
            // dbフォルダがなければ作成する
            string folderName = Game.GetWriteFileName(Game.DatabasePathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            Debug.WriteLine(string.Format("[DivisionName] Save: {0}", Path.GetFileName(fileName)));

            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 1;

                foreach (Country country in Countries.Tags.Where(country => Items[(int) branch, (int) country] != null))
                {
                    foreach (string name in Items[(int) branch, (int) country])
                    {
                        writer.WriteLine("{0};{1}", Countries.Strings[(int) country], name);
                        _currentLineNo++;
                    }
                }
            }
        }

        #endregion

        #region 師団名操作

        /// <summary>
        ///     師団名リストを取得する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        /// <returns>師団名リスト</returns>
        public static IEnumerable<string> GetNames(Branch branch, Country country)
        {
            return Items[(int) branch, (int) country] ?? new List<string>();
        }

        /// <summary>
        ///     師団名を追加する
        /// </summary>
        /// <param name="name">師団名</param>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        private static void AddName(string name, Branch branch, Country country)
        {
            // 未登録の場合はリストを作成する
            if (Items[(int) branch, (int) country] == null)
            {
                Items[(int) branch, (int) country] = new List<string>();
            }

            // 師団名を追加する
            Items[(int) branch, (int) country].Add(name);
        }

        /// <summary>
        ///     師団名リストを設定する
        /// </summary>
        /// <param name="names">師団名リスト</param>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        public static void SetNames(List<string> names, Branch branch, Country country)
        {
            // 師団名リストに変更がなければ戻る
            if (Items[(int) branch, (int) country] != null && names.SequenceEqual(Items[(int) branch, (int) country]))
            {
                return;
            }

            // 師団名リストを設定する
            Items[(int) branch, (int) country] = names;

            // 編集済みフラグを設定する
            SetDirty(branch, country);
        }

        /// <summary>
        ///     師団名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void Replace(string s, string t, Branch branch, Country country, bool regex)
        {
            // 未登録ならば何もしない
            if (Items[(int) branch, (int) country] == null)
            {
                return;
            }

            List<string> names = Items[(int) branch, (int) country]
                .Select(name => regex ? Regex.Replace(name, s, t) : name.Replace(s, t)).ToList();
            SetNames(names, branch, country);
        }

        /// <summary>
        ///     全ての師団名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAll(string s, string t, bool regex)
        {
            foreach (Branch branch in Enum.GetValues(typeof (Branch)))
            {
                foreach (Country country in Countries.Tags)
                {
                    Replace(s, t, branch, country, regex);
                }
            }
        }

        /// <summary>
        ///     全ての兵科の師団名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="country">国タグ</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAllBranches(string s, string t, Country country, bool regex)
        {
            foreach (Branch branch in Enum.GetValues(typeof (Branch)))
            {
                Replace(s, t, branch, country, regex);
            }
        }

        /// <summary>
        ///     全ての国の師団名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="branch">兵科</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAllCountries(string s, string t, Branch branch, bool regex)
        {
            foreach (Country country in Countries.Tags)
            {
                Replace(s, t, branch, country, regex);
            }
        }

        /// <summary>
        ///     師団名を連番追加する
        /// </summary>
        /// <param name="prefix">接頭辞</param>
        /// <param name="suffix">接尾辞</param>
        /// <param name="start">開始番号</param>
        /// <param name="end">終了番号</param>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        public static void AddSequential(string prefix, string suffix, int start, int end, Branch branch,
            Country country)
        {
            // 未登録の場合はリストを作成する
            if (Items[(int) branch, (int) country] == null)
            {
                Items[(int) branch, (int) country] = new List<string>();
            }

            for (int i = start; i <= end; i++)
            {
                string name = string.Format("{0}{1}{2}", prefix, i, suffix);
                if (!Items[(int) branch, (int) country].Contains(name))
                {
                    AddName(name, branch, country);
                    SetDirty(branch, country);
                }
            }
        }

        /// <summary>
        ///     師団名を連番補間する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        public static void Interpolate(Branch branch, Country country)
        {
            // 未登録ならば何もしない
            if (Items[(int) branch, (int) country] == null)
            {
                return;
            }

            var names = new List<string>();
            var r = new Regex("([^\\d]*)(\\d+)(.*)");
            string pattern = "";
            int prev = 0;
            bool found = false;
            foreach (string name in Items[(int) branch, (int) country])
            {
                if (r.IsMatch(name))
                {
                    int n;
                    if (int.TryParse(r.Replace(name, "$2"), out n))
                    {
                        if (!found)
                        {
                            // 出力パターンを設定する
                            pattern = r.Replace(name, "$1{0}$3");
                            found = true;
                        }
                        else
                        {
                            // 前の番号と現在の番号の間を補間する
                            if (prev + 1 < n)
                            {
                                for (int i = prev + 1; i < n; i++)
                                {
                                    string s = string.Format(pattern, i);
                                    if (!names.Contains(s))
                                    {
                                        names.Add(s);
                                    }
                                }
                            }
                        }
                        prev = n;
                    }
                }
                names.Add(name);
            }

            SetNames(names, branch, country);
        }

        /// <summary>
        ///     全ての師団名を連番補間する
        /// </summary>
        public static void InterpolateAll()
        {
            foreach (Branch branch in Enum.GetValues(typeof (Branch)))
            {
                foreach (Country country in Countries.Tags)
                {
                    Interpolate(branch, country);
                }
            }
        }

        /// <summary>
        ///     全ての兵科の師団名を連番補間する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void InterpolateAllBranches(Country country)
        {
            foreach (Branch branch in Enum.GetValues(typeof (Branch)))
            {
                Interpolate(branch, country);
            }
        }

        /// <summary>
        ///     全ての国の師団名を連番補間する
        /// </summary>
        /// <param name="branch">兵科</param>
        public static void InterpolateAllCountries(Branch branch)
        {
            foreach (Country country in Countries.Tags)
            {
                Interpolate(branch, country);
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
        /// <param name="branch">兵科</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(Branch branch)
        {
            return BranchDirtyFlags[(int) branch];
        }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(Branch branch, Country country)
        {
            return CountryDirtyFlags[(int) branch, (int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="country">国タグ</param>
        private static void SetDirty(Branch branch, Country country)
        {
            CountryDirtyFlags[(int) branch, (int) country] = true;
            BranchDirtyFlags[(int) branch] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        private static void ResetDirtyAll()
        {
            foreach (Branch branch in Enum.GetValues(typeof (Branch)))
            {
                foreach (Country country in Enum.GetValues(typeof (Country)))
                {
                    CountryDirtyFlags[(int) branch, (int) country] = false;
                }
                BranchDirtyFlags[(int) branch] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }
}