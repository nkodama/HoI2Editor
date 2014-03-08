using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     文字列定義クラス
    /// </summary>
    internal static class Config
    {
        #region 公開プロパティ

        /// <summary>
        ///     言語モード
        /// </summary>
        public static LanguageMode LangMode
        {
            get { return _langMode; }
            set
            {
                _langMode = value;
                Debug.WriteLine(string.Format("Language Mode: {0}", LanguageModeStrings[(int) _langMode]));
            }
        }

        /// <summary>
        ///     言語インデックス
        /// </summary>
        /// <remarks>
        ///     日本語環境ならば先頭言語が日本語、その次が英語(英語版日本語化の場合)で残りは空
        ///     日本語環境でなければ、英仏伊西独波葡露Extra1/2の順
        /// </remarks>
        public static int LangIndex
        {
            private get { return _langIndex; }
            set
            {
                _langIndex = value;
                Debug.WriteLine(string.Format("Language Index: {0} ({1})", _langIndex,
                    LanguageStrings[(int) _langMode][_langIndex]));
            }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     言語モード
        /// </summary>
        private static LanguageMode _langMode;

        /// <summary>
        ///     言語インデックス
        /// </summary>
        private static int _langIndex;

        /// <summary>
        ///     文字列変換テーブル
        /// </summary>
        private static readonly Dictionary<string, string[]> Text = new Dictionary<string, string[]>();

        /// <summary>
        ///     置き換え文字列変換テーブル
        /// </summary>
        /// <remarks>
        ///     登録した文字列はTextよりも優先して参照される。
        ///     ファイルに書き出される時には無視される。
        ///     エディタ内部で重複文字列を修正したい時に使用する。
        /// </remarks>
        private static readonly Dictionary<string, string[]> ReplacedText = new Dictionary<string, string[]>();

        /// <summary>
        ///     補完文字列変換テーブル
        /// </summary>
        /// <remarks>
        ///     登録した文字列はTextに定義が存在しない時に参照される。
        ///     ファイルに書き出される時には無視される。
        ///     エディタ内部で文字列を補完したい時に使用する。
        /// </remarks>
        private static readonly Dictionary<string, string[]> ComplementedText = new Dictionary<string, string[]>();

        /// <summary>
        ///     文字列定義順リストテーブル
        /// </summary>
        /// <remarks>
        ///     文字列定義ファイルごとの並び順を保持する。
        ///     変更を保存した時に、元の順番を維持するために使用する。
        /// </remarks>
        private static readonly Dictionary<string, List<string>> OrderListTable = new Dictionary<string, List<string>>();

        /// <summary>
        ///     文字列予約リストテーブル
        /// </summary>
        /// <remarks>
        ///     編集途中で追加が必要になった文字列定義を保持する。
        ///     保存時には各ファイルの末尾に追記される。
        /// </remarks>
        private static readonly Dictionary<string, List<string>> ReservedListTable =
            new Dictionary<string, List<string>>();

        /// <summary>
        ///     文字列定義ファイルテーブル
        /// </summary>
        private static readonly Dictionary<string, string> TextFileTable = new Dictionary<string, string>();

        /// <summary>
        ///     一時キーリスト
        /// </summary>
        /// <remarks>
        ///     一時キーが発行された時、一時キーのまま保存された定義を読み込んだ時に登録される。
        ///     一時キーをリネームした時にリストから削除される。
        ///     一時キーのままの定義は文字列保存時にスキップされる
        /// </remarks>
        private static readonly List<string> TempKeyList = new List<string>();

        /// <summary>
        ///     編集済みファイルのリスト
        /// </summary>
        /// <remarks>
        ///     ファイル名はconfigファイルからの相対パスで保存される
        /// </remarks>
        private static readonly List<string> DirtyFiles = new List<string>();

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     一時キー作成のための番号
        /// </summary>
        private static int _tempNo = 1;

        /// <summary>
        ///     一時キーかどうかの判定のための正規表現
        /// </summary>
        private static readonly Regex RegexTempKey = new Regex("_EDITOR_TEMP_\\d+");

        #endregion

        #region 公開定数

        /// <summary>
        ///     言語名文字列
        /// </summary>
        public static readonly string[][] LanguageStrings =
        {
            new[] {Resources.LanguageJapanese},
            new[]
            {
                Resources.LanguageEnglish, Resources.LanguageFrench, Resources.LanguageItalian,
                Resources.LanguageSpanish, Resources.LanguageGerman, Resources.LanguagePolish,
                Resources.LanguagePortuguese, Resources.LanguageRussian, Resources.LanguageExtra1,
                Resources.LanguageExtra2
            },
            new[] {Resources.LanguageJapanese, Resources.LanguageEnglish},
            new[] {Resources.LanguageKorean},
            new[] {Resources.LanguageChinese},
            new[] {Resources.LanguageChinese}
        };

        #endregion

        #region 内部定数

        /// <summary>
        ///     言語の最大数
        /// </summary>
        private const int MaxLanguages = 10;

        /// <summary>
        ///     言語モード文字列
        /// </summary>
        private static readonly string[] LanguageModeStrings =
        {
            "Japanese",
            "English",
            "Patched Japanese",
            "Patched Korean",
            "Patched Traditional Chinese",
            "Patched Simplified Chinese"
        };

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     文字列定義ファイルの再読み込みを要求する
        /// </summary>
        /// <remarks>
        ///     ゲームフォルダ、MOD名、ゲーム種類、言語の変更があった場合に呼び出す
        /// </remarks>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     文字列定義ファイル群を再読み込みする
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
        ///     文字列ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            Text.Clear();
            ReplacedText.Clear();
            ComplementedText.Clear();
            OrderListTable.Clear();
            TextFileTable.Clear();
            TempKeyList.Clear();
            DirtyFiles.Clear();

            var fileList = new List<string>();
            string folderName;
            bool error = false;

            // DHでデフォルト以外のマップを使用する場合、マップフォルダからprovince_names.csvを読み込む
            if ((Game.Type == GameType.DarkestHour) && (Misc.MapNumber != 0))
            {
                folderName = Path.Combine(Game.MapPathName, string.Format("Map_{0}", Misc.MapNumber));
                string fileName = Game.GetReadFileName(folderName, Game.ProvinceTextFileName);
                if (File.Exists(fileName))
                {
                    string name = Path.GetFileName(fileName);
                    try
                    {
                        LoadFile(fileName);
                        if (!string.IsNullOrEmpty(name))
                        {
                            fileList.Add(name.ToLower());
                        }
                    }
                    catch (Exception)
                    {
                        error = true;
                        Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                        Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                        if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                            Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }

            // 保存フォルダ内の文字列ファイル群を読み込む
            if (Game.IsExportFolderActive)
            {
                folderName = Path.Combine(Game.ExportFolderName, Game.ConfigPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                        {
                            try
                            {
                                LoadFile(fileName);
                                fileList.Add(name.ToLower());
                            }
                            catch (Exception)
                            {
                                error = true;
                                Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                                if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                    Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            // MODフォルダ内の文字列ファイル群を読み込む
            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.ConfigPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                        {
                            try
                            {
                                LoadFile(fileName);
                                fileList.Add(name.ToLower());
                            }
                            catch (Exception)
                            {
                                error = true;
                                Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                                if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                    Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            // バニラフォルダ内の文字列ファイル群を読み込む
            folderName = Path.Combine(Game.FolderName, Game.ConfigPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!string.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                    {
                        try
                        {
                            LoadFile(fileName);
                        }
                        catch (Exception)
                        {
                            error = true;
                            Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                            if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            // AoDではconfig\Additional以下のファイルを読み込む
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                fileList.Clear();

                if (Game.IsExportFolderActive)
                {
                    folderName = Path.Combine(Game.ExportFolderName, Game.ConfigAdditionalPathName);
                    if (Directory.Exists(folderName))
                    {
                        foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                        {
                            try
                            {
                                LoadFile(fileName);
                                string name = Path.GetFileName(fileName);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    fileList.Add(Path.Combine("Additional", name.ToLower()));
                                }
                            }
                            catch (Exception)
                            {
                                error = true;
                                Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                                if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                    Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                if (Game.IsModActive)
                {
                    folderName = Path.Combine(Game.ModFolderName, Game.ConfigAdditionalPathName);
                    if (Directory.Exists(folderName))
                    {
                        foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                        {
                            try
                            {
                                LoadFile(fileName);
                                string name = Path.GetFileName(fileName);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    fileList.Add(Path.Combine("Additional", name.ToLower()));
                                }
                            }
                            catch (Exception)
                            {
                                error = true;
                                Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                                if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                    Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                folderName = Path.Combine(Game.FolderName, Game.ConfigAdditionalPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name) &&
                            !fileList.Contains(Path.Combine("Additional", name.ToLower())))
                        {
                            try
                            {
                                LoadFile(fileName);
                            }
                            catch (Exception)
                            {
                                error = true;
                                Debug.WriteLine(string.Format("[Config] Read error: {0}", fileName));
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                                if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                    Resources.EditorConfig, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            // 重複文字列を置き換える
            ModifyDuplicatedStrings();

            // 不足している文字列を補完する
            AddInsufficientStrings();

            // 読み込みに失敗していれば戻る
            if (error)
            {
                return;
            }

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     文字列ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string name = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            // ゲーム中に使用しないファイルを無視する
            if (name.Equals("editor.csv") || name.Equals("launcher.csv"))
            {
                return;
            }

            string dirName = Path.GetFileName(Path.GetDirectoryName(fileName));
            if (!string.IsNullOrEmpty(dirName) && dirName.ToLower().Equals("additional"))
            {
                name = Path.Combine("Addtional", name);
            }

            Debug.WriteLine(string.Format("[Config] Load: {0}", name));

            // トークン数の設定
            int expectedCount;
            int effectiveCount;
            if (name.Equals("editor.csv"))
            {
                expectedCount = 11;
                effectiveCount = 10;
            }
            else if (name.Equals("famous_quotes.csv"))
            {
                expectedCount = 16;
                effectiveCount = 16;
            }
            else if (name.Equals("launcher.csv"))
            {
                expectedCount = 10;
                effectiveCount = 10;
            }
            else
            {
                expectedCount = 12;
                effectiveCount = 11;
            }

            int lineNo = 0;
            var orderList = new List<string>();

            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lineNo++;

                    // 空行を読み飛ばす
                    if (string.IsNullOrEmpty(line))
                    {
                        orderList.Add("");
                        continue;
                    }

                    string[] tokens = line.Split(CsvSeparator);

                    // 先頭トークンを定義順リストに登録する
                    orderList.Add(tokens[0]);

                    // トークン数が足りない行は読み飛ばす
                    if (tokens.Length != expectedCount)
                    {
                        Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount,
                            Path.Combine(Game.ConfigPathName, name), lineNo));
                        Log.Write(string.Format("  {0}\n\n", line));
                        Debug.WriteLine(string.Format("[Config] Invalid token count ({0} L{1})", name, lineNo));

                        // 末尾のxがない/余分な項目がある場合は解析を続ける
                        if (tokens.Length < effectiveCount)
                        {
                            continue;
                        }
                    }

                    // 空行、コメント行を読み飛ばす
                    if (tokens.Length <= 1 || string.IsNullOrEmpty(tokens[0]) || tokens[0][0] == '#')
                    {
                        continue;
                    }

                    string key = tokens[0].ToUpper();
                    // 何らかの理由で一時キーがファイルに残っていれば一時キーリストに登録する
                    if (RegexTempKey.IsMatch(key))
                    {
                        TempKeyList.Add(key);
                        Log.Write(string.Format("{0}: {1} {2} L{3}\n", Resources.WarningUnexpectedTempKey, key,
                            Path.Combine(Game.ConfigPathName, name), lineNo));
                        Debug.WriteLine(string.Format("[Config] Unexpected temp key: {0} ({1} L{2})", key, name, lineNo));
                    }

                    // 変換テーブルに登録する
                    var t = new string[MaxLanguages];
                    for (int i = 0; i < MaxLanguages; i++)
                    {
                        t[i] = tokens[i + 1];
                    }
                    Text[key] = t;

                    // 文字列定義ファイルテーブルに登録する
                    TextFileTable[key] = name;
                }
            }

            // 定義順リストテーブルに登録する
            OrderListTable.Add(name, orderList);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     文字列ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return true;
            }

            bool error = false;
            foreach (string fileName in DirtyFiles)
            {
                try
                {
                    SaveFile(fileName);
                }
                catch (Exception)
                {
                    error = true;
                    string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                        Game.ConfigPathName);
                    string pathName = Path.Combine(folderName, fileName);
                    Debug.WriteLine(string.Format("[Config] Write error: {0}", pathName));
                    Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, pathName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, pathName),
                        Resources.EditorUnit, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
        ///     文字列ファイルを保存する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void SaveFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string name = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            string dirName = Path.GetFileName(Path.GetDirectoryName(fileName));
            if (!string.IsNullOrEmpty(dirName) && dirName.ToLower().Equals("additional"))
            {
                name = Path.Combine("Addtional", name);
            }

            Debug.WriteLine(string.Format("[Config] Save: {0}", name));

            // 保存フォルダ名を取得する
            string folderName = Game.GetWriteFileName(fileName.Equals(Game.ProvinceTextFileName)
                ? Game.GetProvinceNameFolderName()
                : Game.ConfigPathName);

            // 文字列フォルダがなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string pathName = Path.Combine(folderName, fileName);

            using (var writer = new StreamWriter(pathName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // 最初のEOF定義で追加文字列を書き込むためのフラグ
                bool firsteof = true;

                // 既存の文字列定義
                foreach (string key in OrderListTable[fileName])
                {
                    // 空行
                    if (string.IsNullOrEmpty(key))
                    {
                        writer.WriteLine(";;;;;;;;;;;X");
                        continue;
                    }

                    // コメント行
                    if (key[0] == '#')
                    {
                        // 先頭行
                        if (key.Equals("#  STRING NAME (do not change!)"))
                        {
                            writer.WriteLine(
                                "#  STRING NAME (do not change!);English;French;Italian;Spanish;German;Polish;Portuguese;Russian;;Extra2;X");
                            continue;
                        }
                        // ファイル末尾のEOFの直前に追加文字列を出力する
                        if (key.Equals("#EOF") && firsteof)
                        {
                            // 追加文字列
                            WriteAdditionalStrings(fileName, writer);
                            firsteof = false;
                        }
                        writer.WriteLine("{0};;;;;;;;;;;X", key);
                        continue;
                    }

                    // 文字列定義
                    string k = key.ToUpper();
                    // 一時キーは保存しない
                    if (TempKeyList.Contains(k))
                    {
                        TempKeyList.Remove(k);
                        Log.Write(string.Format("{0}: {1}", Resources.WarningRemovedUnusedTempKey, key));
                        Debug.WriteLine(string.Format("[Config] Removed unused temp key: {0}", key));
                        continue;
                    }
                    // 登録されていないキーは保存しない
                    if (!Text.ContainsKey(k))
                    {
                        Log.Write(string.Format("{0}: {1} ({2})\n", Resources.WarningSkippedUnexisitingKey, key, name));
                        Debug.WriteLine(string.Format("[Config] Skipped unexisting key: {0} ({1})", key, name));
                        continue;
                    }
                    string[] t = Text[k];
                    writer.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};X",
                        key, t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7], t[8], t[9]);
                }

                // ファイル末尾のEOFがない場合の保険
                if (firsteof)
                {
                    // 追加文字列
                    WriteAdditionalStrings(fileName, writer);
                    // 末尾行
                    writer.WriteLine("#EOF;;;;;;;;;;;X");
                }
            }
        }

        /// <summary>
        ///     追加の文字列定義を出力する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAdditionalStrings(string fileName, StreamWriter writer)
        {
            // 追加の文字列定義がなければ戻る
            if (!ReservedListTable.ContainsKey(fileName))
            {
                return;
            }

            // 追加の文字列定義を順に出力する
            foreach (string key in ReservedListTable[fileName])
            {
                string k = key.ToUpper();
                // 一時キーは保存しない
                if (TempKeyList.Contains(k))
                {
                    Log.Write(string.Format("{0}: {1} {2}", Resources.WarningSkippedUnusedTempKey, key, fileName));
                    Debug.WriteLine(string.Format("[Config] Skipped temp key: {0} ({1})", key, fileName));
                    TempKeyList.Remove(k);
                    continue;
                }
                if (Text.ContainsKey(k))
                {
                    string[] t = Text[k];
                    writer.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};X",
                        key, t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7], t[8], t[9]);
                }
                else
                {
                    writer.WriteLine("{0};;;;;;;;;;;X", key);
                }
            }

            // 予約リストを全て削除する
            ReservedListTable[fileName].Clear();
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列を取得する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <returns>取得した文字列</returns>
        public static string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            key = key.ToUpper();

            // 置き換え文字列変換テーブルに登録されていれば優先して参照する
            if (ReplacedText.ContainsKey(key))
            {
                return ReplacedText[key][LangIndex];
            }

            // 文字列変換テーブルに登録されていれば参照する
            if (Text.ContainsKey(key))
            {
                return Text[key][LangIndex];
            }

            // 補完文字列変換テーブルに登録されていれば参照する
            if (ComplementedText.ContainsKey(key))
            {
                return ComplementedText[key][LangIndex];
            }

            // テーブルに登録されていなければ定義名を返す
            Debug.WriteLine(string.Format("[Config] GetText failed: {0}", key));
            return key;
        }

        /// <summary>
        ///     文字列を設定する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="text">登録する文字列</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        /// <remarks>
        ///     文字列が登録されていなければ新規追加、登録されていれば値を変更する
        ///     ファイル名の指定は既存の定義が存在しない場合のみ有効
        /// </remarks>
        public static void SetText(string key, string text, string fileName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            key = key.ToUpper();

            // 文字列変換テーブルに登録されていなければ登録する
            if (!Text.ContainsKey(key))
            {
                // 予約リストがなければ作成する
                if (!ReservedListTable.ContainsKey(fileName))
                {
                    ReservedListTable.Add(fileName, new List<string>());
                }

                // 予約リストに登録する
                ReservedListTable[fileName].Add(key);

                // 文字列変換テーブルに登録する
                Text[key] = new string[MaxLanguages];

                // 文字列定義ファイルテーブルに登録する
                TextFileTable[key] = fileName;

                Debug.WriteLine(string.Format("[Config] Added {0} ({1})", key, fileName));
            }
            else if (TextFileTable.ContainsKey(key))
            {
                // 既存の定義ならばファイル名を検索して置き換える
                fileName = TextFileTable[key];
            }

            // 文字列変換テーブルの文字列を変更する
            Text[key][LangIndex] = text;
            Debug.WriteLine(string.Format("[Config] Set {0}: {1}", key, text));

            // 編集済みフラグを設定する
            SetDirty(fileName);
        }

        /// <summary>
        ///     文字列定義名を変更する
        /// </summary>
        /// <param name="oldKey">変更対象の文字列定義名</param>
        /// <param name="newKey">変更後の文字列定義名</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void RenameText(string oldKey, string newKey, string fileName)
        {
            if (string.IsNullOrEmpty(oldKey) || string.IsNullOrEmpty(newKey))
            {
                return;
            }
            oldKey = oldKey.ToUpper();
            newKey = newKey.ToUpper();

            // 文字列変換テーブルに登録し直す
            if (Text.ContainsKey(oldKey))
            {
                if (!Text.ContainsKey(newKey))
                {
                    Text.Add(newKey, Text[oldKey]);
                    Debug.WriteLine(string.Format("[Config] Rename: {0} - {1}", oldKey, newKey));
                }
                else
                {
                    // 変換後のキーあり: 一時キーがリネームされずに保存された場合
                    Text[newKey] = Text[oldKey];
                    Log.Write(string.Format("{0}: {1} - {2}\n", Resources.WarningRenameTargetAlreadyExists, oldKey,
                        newKey));
                    Debug.WriteLine(string.Format("[Config] Rename target already exists in text table: {0} - {1}",
                        oldKey, newKey));
                }
                Text.Remove(oldKey);
            }
            else
            {
                if (!Text.ContainsKey(newKey))
                {
                    // 文字列変換テーブルに登録する
                    Text[newKey] = new string[MaxLanguages];
                    Text[newKey][LangIndex] = "";
                }
                // 変換前のキーなし: 一時キーが重複していて既にリネームされた場合
                Log.Write(string.Format("{0}: {1} - {2}\n", Resources.WarningRenameSourceDoesNotExist, oldKey, newKey));
                Debug.WriteLine(string.Format("[Config] Rename source does not exist in text table: {0} - {1}", oldKey,
                    newKey));
            }

            // 予約リストに登録し直す
            if (ReservedListTable.ContainsKey(fileName))
            {
                if (ReservedListTable[fileName].Contains(oldKey))
                {
                    if (!ReservedListTable[fileName].Contains(newKey))
                    {
                        ReservedListTable[fileName].Add(newKey);
                        Debug.WriteLine(string.Format("[Config] Replaced reserved list: {0} - {1} ({2})", oldKey, newKey,
                            fileName));
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("[Config] Already exists in reserved list: {0} - {1} ({2})",
                            oldKey, newKey, fileName));
                    }
                    ReservedListTable[fileName].Remove(oldKey);
                }
            }

            // 文字列定義順リストを書き換える
            if (OrderListTable.ContainsKey(fileName) && OrderListTable[fileName].Contains(oldKey))
            {
                int index = OrderListTable[fileName].LastIndexOf(oldKey);
                OrderListTable[fileName][index] = newKey;
            }

            // 文字列定義ファイルテーブルに登録し直す
            if (TextFileTable.ContainsKey(oldKey))
            {
                TextFileTable.Remove(fileName);
            }
            if (!TextFileTable.ContainsKey(newKey))
            {
                TextFileTable.Add(newKey, fileName);
            }

            // 一時キーリストから削除する
            if (TempKeyList.Contains(oldKey))
            {
                TempKeyList.Remove(oldKey);
                Debug.WriteLine(string.Format("[Config] Removed temp list: {0}", oldKey));
            }

            // 編集済みフラグを設定する
            SetDirty(fileName);
        }

        /// <summary>
        ///     文字列を削除する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void RemoveText(string key, string fileName)
        {
            // 文字列変換テーブルから削除する
            if (Text.ContainsKey(key))
            {
                Text.Remove(key);
                Debug.WriteLine(string.Format("[Config] Removed text: {0} ({1})", key, Path.GetFileName(fileName)));
            }

            // 予約リストから削除する
            if (ReservedListTable.ContainsKey(fileName) && ReservedListTable[fileName].Contains(key))
            {
                ReservedListTable[fileName].Remove(key);
                Debug.WriteLine(string.Format("[Config] Removed reserved list: {0} ({1})", key,
                    Path.GetFileName(fileName)));
            }

            // 文字列定義順リストから削除する
            if (OrderListTable.ContainsKey(fileName) && OrderListTable[fileName].Contains(key))
            {
                OrderListTable[fileName].Remove(key);
            }

            // 文字列定義ファイルテーブルから削除する
            if (TextFileTable.ContainsKey(key))
            {
                TextFileTable.Remove(key);
            }

            // 一時キーリストから削除する
            if (TempKeyList.Contains(key))
            {
                TempKeyList.Remove(key);
                Debug.WriteLine(string.Format("[Config] Removed temp list: {0}", key));
            }

            // 編集済みフラグを設定する
            SetDirty(fileName);
        }

        /// <summary>
        ///     文字列が登録されているかを返す
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <returns>文字列が登録されていればtrueを返す</returns>
        public static bool ExistsKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            key = key.ToUpper();

            return Text.ContainsKey(key);
        }

        /// <summary>
        ///     一時キーかどうかを判定する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <returns>一時キーかどうか</returns>
        public static bool IsTempKey(string key)
        {
            return !string.IsNullOrEmpty(key) && RegexTempKey.IsMatch(key);
        }

        /// <summary>
        ///     一時キーを取得する
        /// </summary>
        /// <returns>一時キー名</returns>
        public static string GetTempKey()
        {
            string key;
            do
            {
                key = string.Format("_EDITOR_TEMP_{0}", _tempNo);
                _tempNo++;
            } while (TempKeyList.Contains(key) || ExistsKey(key));

            // 一時キーリストに登録する
            TempKeyList.Add(key);
            Debug.WriteLine(string.Format("[Config] New temp key: {0}", key));

            return key;
        }

        /// <summary>
        ///     一時キーリストに登録する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        public static void AddTempKey(string key)
        {
            if (!TempKeyList.Contains(key))
            {
                TempKeyList.Add(key);
                Debug.WriteLine(string.Format("[Config] Added temp key: {0}", key));
            }
        }

        /// <summary>
        ///     置き換え文字列変換テーブルに登録する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="text">登録する文字列</param>
        private static void AddReplacedText(string key, string text)
        {
            // 置き換え文字列変換テーブルに登録する
            ReplacedText[key] = new string[MaxLanguages];
            ReplacedText[key][LangIndex] = text;
        }

        /// <summary>
        ///     補完文字列変換テーブルに登録する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="text">登録する文字列</param>
        private static void AddComplementedText(string key, string text)
        {
            // 登録文字列があれば何もしない
            if (Text.ContainsKey(key))
            {
                return;
            }

            // 補完文字列変換テーブルに登録する
            ComplementedText[key] = new string[MaxLanguages];
            ComplementedText[key][LangIndex] = text;
        }

        /// <summary>
        ///     重複する文字列を修正する
        /// </summary>
        private static void ModifyDuplicatedStrings()
        {
            // 決戦ドクトリン: 陸軍総司令官/海軍総司令官
            if (ExistsKey("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE") &&
                ExistsKey("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2") &&
                GetText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE")
                    .Equals(GetText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2")))
            {
                AddReplacedText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE",
                    string.Format("{0}({1})", GetText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"),
                        Resources.BranchArmy));
                AddReplacedText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2",
                    string.Format("{0}({1})", GetText("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"),
                        Resources.BranchNavy));
            }

            // 偏執的誇大妄想家: ヒトラー/スターリン
            if (ExistsKey("NPERSONALITY_HITLER") &&
                ExistsKey("NPERSONALITY_STALIN") &&
                GetText("NPERSONALITY_HITLER").Equals(GetText("NPERSONALITY_STALIN")))
            {
                AddReplacedText("NPERSONALITY_HITLER",
                    string.Format("{0}({1})", GetText("NPERSONALITY_HITLER"), Resources.MinisterHitler));
                AddReplacedText("NPERSONALITY_STALIN",
                    string.Format("{0}({1})", GetText("NPERSONALITY_STALIN"), Resources.MinisterStalin));
            }

            // ドイツ軍事顧問: パイパー/マイスナー/ブロンザルト/ゼークト/ゾウヒョン/パシヴィッツ/ゼルノ/ゴルツ/ジーフェルト/トヴネ/ウーゼドム
            if (ExistsKey("NPERSONALITY_GER_MIL_M1") &&
                ExistsKey("NPERSONALITY_GER_MIL_M2") &&
                ExistsKey("NPERSONALITY_GER_MIL_M3") &&
                ExistsKey("NPERSONALITY_GER_MIL_M4") &&
                ExistsKey("NPERSONALITY_GER_MIL_M5") &&
                ExistsKey("NPERSONALITY_GER_MIL_M6") &&
                ExistsKey("NPERSONALITY_GER_MIL_M7") &&
                ExistsKey("NPERSONALITY_GER_MIL_M8") &&
                ExistsKey("NPERSONALITY_GER_MIL_M9") &&
                ExistsKey("NPERSONALITY_GER_MIL_M10") &&
                ExistsKey("NPERSONALITY_GER_MIL_M11") &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M2")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M3")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M4")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M5")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M6")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M7")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M8")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M9")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M10")) &&
                GetText("NPERSONALITY_GER_MIL_M1").Equals(GetText("NPERSONALITY_GER_MIL_M11")))
            {
                AddReplacedText("NPERSONALITY_GER_MIL_M1",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M1"), Resources.MinisterPeiper));
                AddReplacedText("NPERSONALITY_GER_MIL_M2",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M2"), Resources.MinisterMeissner));
                AddReplacedText("NPERSONALITY_GER_MIL_M3",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M3"), Resources.MinisterBronsart));
                AddReplacedText("NPERSONALITY_GER_MIL_M4",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M4"), Resources.MinisterSeeckt));
                AddReplacedText("NPERSONALITY_GER_MIL_M5",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M5"), Resources.MinisterSouchon));
                AddReplacedText("NPERSONALITY_GER_MIL_M6",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M6"), Resources.MinisterPaschwitz));
                AddReplacedText("NPERSONALITY_GER_MIL_M7",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M7"), Resources.MinisterSerno));
                AddReplacedText("NPERSONALITY_GER_MIL_M8",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M8"), Resources.MinisterGoltz));
                AddReplacedText("NPERSONALITY_GER_MIL_M9",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M9"), Resources.MinisterSievert));
                AddReplacedText("NPERSONALITY_GER_MIL_M10",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M10"), Resources.MinisterThauvenay));
                AddReplacedText("NPERSONALITY_GER_MIL_M11",
                    string.Format("{0}({1})", GetText("NPERSONALITY_GER_MIL_M11"), Resources.MinisterUsedom));
            }

            // 暗号解析の専門家: シンクレア/フリードマン
            if (ExistsKey("NPERSONALITY_SINCLAIR") &&
                ExistsKey("NPERSONALITY_FRIEDMAN") &&
                GetText("NPERSONALITY_SINCLAIR").Equals(GetText("NPERSONALITY_FRIEDMAN")))
            {
                AddReplacedText("NPERSONALITY_SINCLAIR",
                    string.Format("{0}({1})", GetText("NPERSONALITY_SINCLAIR"), Resources.MinisterSinclair));
                AddReplacedText("NPERSONALITY_FRIEDMAN",
                    string.Format("{0}({1})", GetText("NPERSONALITY_FRIEDMAN"), Resources.MinisterFriedman));
            }
        }

        /// <summary>
        ///     不足している文字列を追加する
        /// </summary>
        private static void AddInsufficientStrings()
        {
            // 旅団なし
            AddComplementedText("NAME_NONE", Resources.BrigadeNone);

            // 大陸名
            AddComplementedText("CON_LAKE", Resources.ContinentLake);
            AddComplementedText("CON_ATLANTICOCEAN", Resources.ContinentAtlanticOcean);
            AddComplementedText("CON_PACIFICOCEAN", Resources.ContinentPacificOcean);
            AddComplementedText("CON_INDIANOCEAN", Resources.ContinentIndianOcean);

            // 地方名
            AddComplementedText("REG_-", "-");

            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                // ユーザー定義のユニットクラス名
                for (int i = 1; i <= 20; i++)
                {
                    AddComplementedText(string.Format("NAME_B_U{0}", i),
                        string.Format("{0}{1}", Resources.BrigadeUser, i));
                }
            }

            if (Game.Type == GameType.DarkestHour)
            {
                // DH固有の研究特性
                AddComplementedText("RT_AVIONICS", Resources.SpecialityAvionics);
                AddComplementedText("RT_MUNITIONS", Resources.SpecialityMunitions);
                AddComplementedText("RT_VEHICLE_ENGINEERING", Resources.SpecialityVehicleEngineering);
                AddComplementedText("RT_CARRIER_DESIGN", Resources.SpecialityCarrierDesign);
                AddComplementedText("RT_SUBMARINE_DESIGN", Resources.SpecialitySubmarineDesign);
                AddComplementedText("RT_FIGHTER_DESIGN", Resources.SpecialityFighterDesign);
                AddComplementedText("RT_BOMBER_DESIGN", Resources.SpecialityBomberDesign);
                AddComplementedText("RT_MOUNTAIN_TRAINING", Resources.SpecialityMountainTraining);
                AddComplementedText("RT_AIRBORNE_TRAINING", Resources.SpecialityAirborneTraining);
                AddComplementedText("RT_MARINE_TRAINING", Resources.SpecialityMarineTraining);
                AddComplementedText("RT_MANEUVER_TACTICS", Resources.SpecialityManeuverTactics);
                AddComplementedText("RT_BLITZKRIEG_TACTICS", Resources.SpecialityBlitzkriegTactics);
                AddComplementedText("RT_STATIC_DEFENSE_TACTICS", Resources.SpecialityStaticDefenseTactics);
                AddComplementedText("RT_MEDICINE", Resources.SpecialityMedicine);
                AddComplementedText("RT_CAVALRY_TACTICS", Resources.SpecialityCavalryTactics);

                // ユーザー定義の研究特性
                for (int i = 1; i <= 60; i++)
                {
                    AddComplementedText(string.Format("RT_USER{0}", i),
                        string.Format("{0}{1}", Resources.SpecialityUser, i));
                }

                // DH固有のユニットクラス名
                AddComplementedText("NAME_LIGHT_CARRIER", Resources.DivisionLightCarrier);
                AddComplementedText("NAME_ROCKET_INTERCEPTOR", Resources.DivisionRocketInterceptor);
                AddComplementedText("NAME_CAVALRY_BRIGADE", Resources.BrigadeCavalry);
                AddComplementedText("NAME_SP_ANTI_AIR", Resources.BrigadeSpAntiAir);
                AddComplementedText("NAME_MEDIUM_ARMOR", Resources.BrigadeMediumTank);
                AddComplementedText("NAME_FLOATPLANE", Resources.BrigadeFloatPlane);
                AddComplementedText("NAME_LCAG", Resources.BrigadeLightCarrierAirGroup);
                AddComplementedText("NAME_AMPH_LIGHT_ARMOR_BRIGADE", Resources.BrigadeAmphibiousLightArmor);
                AddComplementedText("NAME_GLI_LIGHT_ARMOR_BRIGADE", Resources.BrigadeGliderLightArmor);
                AddComplementedText("NAME_GLI_LIGHT_ARTILLERY", Resources.BrigadeGliderLightArtillery);
                AddComplementedText("NAME_SH_ARTILLERY", Resources.BrigadeSuperHeavyArtillery);

                // ユーザー定義のユニットクラス名
                for (int i = 33; i <= 40; i++)
                {
                    AddComplementedText(string.Format("NAME_D_RSV_{0}", i),
                        string.Format("{0}{1}", Resources.DivisionReserved, i));
                }
                for (int i = 36; i <= 40; i++)
                {
                    AddComplementedText(string.Format("NAME_B_RSV_{0}", i),
                        string.Format("{0}{1}", Resources.BrigadeReserved, i));
                }
                for (int i = 1; i <= 99; i++)
                {
                    AddComplementedText(string.Format("NAME_D_{0:D2}", i),
                        string.Format("{0}{1}", Resources.DivisionUser, i));
                    AddComplementedText(string.Format("NAME_B_{0:D2}", i),
                        string.Format("{0}{1}", Resources.BrigadeUser, i));
                }

                // DH Fullで定義されていない旅団のユニットクラス名
                AddComplementedText("NAME_ROCKET_ARTILLERY", Resources.BrigadeRocketArtillery);
                AddComplementedText("NAME_SP_ROCKET_ARTILLERY", Resources.BrigadeSpRocketArtillery);
                AddComplementedText("NAME_ANTITANK", Resources.BrigadeAntiTank);
                AddComplementedText("NAME_NAVAL_TORPEDOES_L", Resources.BrigadeNavalTorpedoesL);

                // DH None/Lightで定義されていない閣僚特性
                AddComplementedText("GENERIC MINISTER", Resources.MinisterPersonalityGenericMinister);
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
            return (DirtyFiles.Count > 0);
        }

        /// <summary>
        ///     編集済みフラグを更新する
        /// </summary>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void SetDirty(string fileName)
        {
            if (!DirtyFiles.Contains(fileName))
            {
                DirtyFiles.Add(fileName);
            }
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        private static void ResetDirtyAll()
        {
            DirtyFiles.Clear();
        }

        #endregion
    }

    /// <summary>
    ///     言語モード
    /// </summary>
    public enum LanguageMode
    {
        Japanese, // 日本語版
        English, // 英語版
        PatchedJapanese, // 英語版日本語化
        PatchedKorean, // 英語版韓国語化
        PatchedTraditionalChinese, // 英語版繁体字中国語化
        PatchedSimplifiedChinese, // 英語版簡体字中国語化
    }
}