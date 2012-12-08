using System.Collections.Generic;
using System.IO;
using System.Text;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    internal static class Config
    {
        /// <summary>
        ///     文字列変換テーブル
        /// </summary>
        private static readonly Dictionary<string, string> Text = new Dictionary<string, string>();

        /// <summary>
        ///     置き換え文字列変換テーブル
        /// </summary>
        /// <remarks>
        ///     登録した文字列はTextよりも優先して参照される。
        ///     ファイルに書き出される時には無視される。
        ///     エディタ内部で文字列を修正/補完したい時に使用する。
        /// </remarks>
        private static readonly Dictionary<string, string> ReplacedText = new Dictionary<string, string>();

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
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

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
            string text;
            if (ReplacedText.TryGetValue(key, out text))
            {
                return text;
            }

            // 文字列変換テーブルに登録されていれば参照する
            if (Text.TryGetValue(key, out text))
            {
                return text;
            }

            // テーブルに登録されていなければ定義名を返す
            return key;
        }

        /// <summary>
        ///     文字列を設定する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="text">文字列</param>
        /// <remarks>
        ///     文字列が登録されていなければ新規追加、登録されていれば値を変更する
        /// </remarks>
        public static void SetText(string key, string text)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Text[key.ToUpper()] = text;
        }

        /// <summary>
        ///     文字列定義が登録されているかを返す
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <returns>文字列定義が登録されていればtrueを返す</returns>
        public static bool ExistsKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            key = key.ToUpper();

            return Text.ContainsKey(key) || ReplacedText.ContainsKey(key);
        }

        /// <summary>
        ///     一時キーを取得する
        /// </summary>
        /// <param name="fileName">文字列定義ファイル名</param>
        /// <returns>一時キー</returns>
        public static string GetTempKey(string fileName)
        {
            string key = string.Format("_EDITOR_TEMP_{0}", _tempNo);
            _tempNo++;

            if (!ReservedListTable.ContainsKey(fileName))
            {
                ReservedListTable[fileName] = new List<string>();
            }
            ReservedListTable[fileName].Add(key);

            return key;
        }

        /// <summary>
        ///     一時キーを削除する
        /// </summary>
        /// <param name="key">一時キー</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void RemoveTempKey(string key, string fileName)
        {
            Text.Remove(key);
            ReservedListTable[fileName].Remove(key);
        }

        /// <summary>
        ///     一時キーをリネームする
        /// </summary>
        /// <param name="oldKey">リネーム対象の一時キー</param>
        /// <param name="newKey">リネーム後の一時キー</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void RenameTempKey(string oldKey, string newKey, string fileName)
        {
            oldKey = oldKey.ToUpper();
            newKey = newKey.ToUpper();

            string text = ReplacedText[oldKey];
            ReplacedText.Remove(oldKey);
            ReplacedText.Add(newKey, text);

            ReservedListTable[fileName].Remove(oldKey);
            ReservedListTable[fileName].Add(newKey);
        }

        /// <summary>
        ///     一時キーかどうかを判定する
        /// </summary>
        /// <param name="key">一時キー</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        /// <returns>一時キーかどうか</returns>
        public static bool IsTempKey(string key, string fileName)
        {
            return ReservedListTable.ContainsKey(fileName) && ReservedListTable[fileName].Contains(key);
        }

        /// <summary>
        ///     文字列ファイル群を読み込む
        /// </summary>
        public static void LoadConfigFiles()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            Text.Clear();
            ReplacedText.Clear();
            OrderListTable.Clear();
            DirtyFiles.Clear();

            var fileList = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.ConfigPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadConfigFile(fileName);
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name))
                        {
                            fileList.Add(name.ToLower());
                        }
                    }
                }
            }

            folderName = Path.Combine(Game.FolderName, Game.ConfigPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!string.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                    {
                        LoadConfigFile(fileName);
                    }
                }
            }

            // AoDではconfig\Additional以下のファイルを読み込む
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                fileList.Clear();

                folderName = Path.Combine(Game.ModFolderName, Game.ConfigAdditionalPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadConfigFile(fileName);
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name))
                        {
                            fileList.Add(Path.Combine("Additional", name.ToLower()));
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
                            LoadConfigFile(fileName);
                        }
                    }
                }
            }

            ModifyDuplicatedStrings();
            AddInsufficientStrings();

            _loaded = true;
        }

        /// <summary>
        ///     文字列ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadConfigFile(string fileName)
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
                    if (tokens.Length != 12)
                    {
                        Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount,
                                                Path.Combine(Game.ConfigPathName, name), lineNo));
                        Log.Write(string.Format("  {0}\n", line));

                        // 末尾のxがない/余分な項目がある場合は解析を続ける
                        if (tokens.Length < 11)
                        {
                            continue;
                        }
                    }

                    // 空行、コメント行を読み飛ばす
                    if (tokens.Length <= 1 || string.IsNullOrEmpty(tokens[0]) || tokens[0][0] == '#')
                    {
                        continue;
                    }

                    // 変換テーブルに登録する
                    Text[tokens[0].ToUpper()] = tokens[1];
                }
            }

            // 定義順リストテーブルに登録する
            OrderListTable.Add(name, orderList);
        }

        /// <summary>
        ///     文字列ファイル群を保存する
        /// </summary>
        public static void SaveConfigFiles()
        {
            foreach (string fileName in DirtyFiles)
            {
                SaveConfigFile(fileName);
            }

            // 編集フラグを全てクリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     文字列ファイルを保存する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void SaveConfigFile(string fileName)
        {
            using (var writer = new StreamWriter(Game.GetFileName(Path.Combine(Game.ConfigPathName, fileName))))
            {
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
                        // ファイル末尾は追加文字列の後で
                        if (key.Equals("#EOF"))
                        {
                            break;
                        }
                        writer.WriteLine("{0};;;;;;;;;;;X", key);
                        continue;
                    }
                    // 文字列定義
                    string k = key.ToUpper();
                    if (Text.ContainsKey(k))
                    {
                        writer.WriteLine("{0};{1};;;;;;;;;;X", key, Text[k]);
                    }
                    else
                    {
                        writer.WriteLine("{0};;;;;;;;;;;X", key);
                    }
                }

                // 追加の文字列定義
                if (ReservedListTable.ContainsKey(fileName))
                {
                    foreach (string key in ReservedListTable[fileName])
                    {
                        string k = key.ToUpper();
                        if (Text.ContainsKey(k))
                        {
                            writer.WriteLine("{0};{1};;;;;;;;;;X", key, Text[k]);
                        }
                        else
                        {
                            writer.WriteLine("{0};;;;;;;;;;;X", key);
                        }
                    }
                }

                // 末尾行
                writer.WriteLine("#EOF;;;;;;;;;;;X");
            }
        }

        /// <summary>
        ///     重複する文字列を修正する
        /// </summary>
        private static void ModifyDuplicatedStrings()
        {
            // 決戦ドクトリン: 陸軍総司令官/海軍総司令官
            if (Text.ContainsKey("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE") &&
                Text.ContainsKey("NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2") &&
                Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"].Equals(Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"]))
            {
                ReplacedText["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"] =
                    string.Format("{0}({1})", Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"], Resources.BranchArmy);
                ReplacedText["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"] =
                    string.Format("{0}({1})", Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"], Resources.BranchNavy);
            }

            // 偏執的誇大妄想家: ヒトラー/スターリン
            if (Text.ContainsKey("NPERSONALITY_HITLER") &&
                Text.ContainsKey("NPERSONALITY_STALIN") &&
                Text["NPERSONALITY_HITLER"].Equals(Text["NPERSONALITY_STALIN"]))
            {
                ReplacedText["NPERSONALITY_HITLER"] =
                    string.Format("{0}({1})", Text["NPERSONALITY_HITLER"], Resources.Hitler);
                ReplacedText["NPERSONALITY_STALIN"] =
                    string.Format("{0}({1})", Text["NPERSONALITY_STALIN"], Resources.Stalin);
            }
        }

        /// <summary>
        ///     不足している文字列を追加する
        /// </summary>
        private static void AddInsufficientStrings()
        {
            // DH固有の研究特性
            if (Game.Type == GameType.DarkestHour)
            {
                if (!Text.ContainsKey("RT_AVIONICS"))
                {
                    ReplacedText["RT_AVIONICS"] = Resources.SpecialityAvionics;
                }
                if (!Text.ContainsKey("RT_MUNITIONS"))
                {
                    ReplacedText["RT_MUNITIONS"] = Resources.SpecialityMunitions;
                }
                if (!Text.ContainsKey("RT_VEHICLE_ENGINEERING"))
                {
                    ReplacedText["RT_VEHICLE_ENGINEERING"] = Resources.SpecialityVehicleEngineering;
                }
                if (!Text.ContainsKey("RT_CARRIER_DESIGN"))
                {
                    ReplacedText["RT_CARRIER_DESIGN"] = Resources.SpecialityCarrierDesign;
                }
                if (!Text.ContainsKey("RT_SUBMARINE_DESIGN"))
                {
                    ReplacedText["RT_SUBMARINE_DESIGN"] = Resources.SpecialitySubmarineDesign;
                }
                if (!Text.ContainsKey("RT_FIGHTER_DESIGN"))
                {
                    ReplacedText["RT_FIGHTER_DESIGN"] = Resources.SpecialityFighterDesign;
                }
                if (!Text.ContainsKey("RT_BOMBER_DESIGN"))
                {
                    ReplacedText["RT_BOMBER_DESIGN"] = Resources.SpecialityBomberDesign;
                }
                if (!Text.ContainsKey("RT_MOUNTAIN_TRAINING"))
                {
                    ReplacedText["RT_MOUNTAIN_TRAINING"] = Resources.SpecialityMountainTraining;
                }
                if (!Text.ContainsKey("RT_AIRBORNE_TRAINING"))
                {
                    ReplacedText["RT_AIRBORNE_TRAINING"] = Resources.SpecialityAirborneTraining;
                }
                if (!Text.ContainsKey("RT_MARINE_TRAINING"))
                {
                    ReplacedText["RT_MARINE_TRAINING"] = Resources.SpecialityMarineTraining;
                }
                if (!Text.ContainsKey("RT_MANEUVER_TACTICS"))
                {
                    ReplacedText["RT_MANEUVER_TACTICS"] = Resources.SpecialityManeuverTactics;
                }
                if (!Text.ContainsKey("RT_BLITZKRIEG_TACTICS"))
                {
                    ReplacedText["RT_BLITZKRIEG_TACTICS"] = Resources.SpecialityBlitzkriegTactics;
                }
                if (!Text.ContainsKey("RT_STATIC_DEFENSE_TACTICS"))
                {
                    ReplacedText["RT_STATIC_DEFENSE_TACTICS"] = Resources.SpecialityStaticDefenseTactics;
                }
                if (!Text.ContainsKey("RT_MEDICINE"))
                {
                    ReplacedText["RT_MEDICINE"] = Resources.SpecialityMedicine;
                }
                if (!Text.ContainsKey("RT_CAVALRY_TACTICS"))
                {
                    ReplacedText["RT_CAVALRY_TACTICS"] = Resources.SpecialityCavalryTactics;
                }
                // ユーザー定義
                for (int i = 1; i <= 60; i++)
                {
                    string key = string.Format("RT_USER{0}", i);
                    if (!Text.ContainsKey(key))
                    {
                        ReplacedText[key] = string.Format("{0}{1}", Resources.SpecialityUser, i);
                    }
                }
            }
        }

        /// <summary>
        ///     文字列定義ファイルの再読み込みを要求する
        /// </summary>
        /// <remarks>
        ///     ゲームフォルダ、MOD名、ゲーム種類、言語の変更があった場合に呼び出す
        /// </remarks>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     編集フラグを設定する
        /// </summary>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void SetDirtyFlag(string fileName)
        {
            if (!DirtyFiles.Contains(fileName))
            {
                DirtyFiles.Add(fileName);
            }
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void ClearDirtyFlag(string fileName)
        {
            if (DirtyFiles.Contains(fileName))
            {
                DirtyFiles.Remove(fileName);
            }
        }

        /// <summary>
        ///     編集フラグを全てクリアする
        /// </summary>
        private static void ClearDirtyFlags()
        {
            DirtyFiles.Clear();
        }
    }

    /// <summary>
    ///     言語コード
    /// </summary>
    public enum Languages
    {
        Japanese,
        English,
        French,
        Italian,
        Spanish,
        German,
        Polish,
        Portuguese,
        Russian,
        Extra1,
        Extra2,
    }
}