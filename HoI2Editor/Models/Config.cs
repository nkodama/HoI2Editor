using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    internal static class Config
    {
        /// <summary>
        ///     言語の最大数
        /// </summary>
        private const int MaxLanguages = 10;

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
                new[] {Resources.LanguageJapanese, Resources.LanguageEnglish}
            };

        /// <summary>
        ///     言語モード
        /// </summary>
        public static LanguageMode LangMode;

        /// <summary>
        ///     言語インデックス
        /// </summary>
        /// <remarks>
        ///     日本語環境ならば先頭言語が日本語、その次が英語(英語版日本語化の場合)で残りは空
        ///     日本語環境でなければ、英仏伊西独波葡露Extra1/2の順
        /// </remarks>
        public static int LangIndex = 0;

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
            }

            // 文字列変換テーブルの文字列を変更する
            Text[key][LangIndex] = text;
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

            if (!Text.ContainsKey(oldKey) || Text.ContainsKey(newKey))
            {
                return;
            }

            // 文字列変換テーブルに登録し直す
            Text.Add(newKey, Text[oldKey]);
            Text.Remove(oldKey);

            // 予約リストに登録し直す
            if (ReservedListTable[fileName].Contains(oldKey) && !ReservedListTable[fileName].Contains(newKey))
            {
                ReservedListTable[fileName].Remove(oldKey);
                ReservedListTable[fileName].Add(newKey);
            }
        }

        /// <summary>
        ///     文字列を削除する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        public static void RemoveText(string key, string fileName)
        {
            Text.Remove(key);
            ReservedListTable[fileName].Remove(key);
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
        ///     予約キーかどうかを判定する
        /// </summary>
        /// <param name="key">文字列の定義名</param>
        /// <param name="fileName">文字列定義ファイル名</param>
        /// <returns>予約キーかどうか</returns>
        public static bool IsReservedKey(string key, string fileName)
        {
            return (ReservedListTable.ContainsKey(fileName) && ReservedListTable[fileName].Contains(key));
        }

        /// <summary>
        ///     一時キーを取得する
        /// </summary>
        /// <returns>一時キー名</returns>
        public static string GetTempKey()
        {
            string key = string.Format("_EDITOR_TEMP_{0}", _tempNo);
            _tempNo++;

            return key;
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
                        try
                        {
                            LoadFile(fileName);
                            string name = Path.GetFileName(fileName);
                            if (!string.IsNullOrEmpty(name))
                            {
                                fileList.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
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
                        try
                        {
                            LoadFile(fileName);
                        }
                        catch (Exception)
                        {
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                        }
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
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
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
                                Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                            }
                        }
                    }
                }
            }

            // 重複文字列を置き換える
            ModifyDuplicatedStrings();

            // 不足している文字列を補完する
            AddInsufficientStrings();

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

                    // 変換テーブルに登録する
                    var t = new string[MaxLanguages];
                    for (int i = 0; i < MaxLanguages; i++)
                    {
                        t[i] = tokens[i + 1];
                    }
                    Text[tokens[0].ToUpper()] = t;
                }
            }

            // 定義順リストテーブルに登録する
            OrderListTable.Add(name, orderList);
        }

        /// <summary>
        ///     文字列ファイル群を保存する
        /// </summary>
        public static void Save()
        {
            var list = new List<string>(DirtyFiles);
            foreach (string fileName in list)
            {
                try
                {
                    SaveFile(fileName);
                    SetDirty(fileName, false);
                }
                catch (Exception)
                {
                    string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                                     Game.ConfigPathName);
                    string pathName = Path.Combine(folderName, fileName);
                    Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, pathName));
                }
            }
        }

        /// <summary>
        ///     文字列ファイルを保存する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void SaveFile(string fileName)
        {
            string folderName = Game.GetWriteFileName(Game.ConfigPathName);

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
                    if (Text.ContainsKey(k))
                    {
                        string[] t = Text[k];
                        writer.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};X",
                                         key, t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7], t[8], t[9]);
                    }
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
                                string.Format("{0}({1})", GetText("NPERSONALITY_HITLER"), Resources.Hitler));
                AddReplacedText("NPERSONALITY_STALIN",
                                string.Format("{0}({1})", GetText("NPERSONALITY_STALIN"), Resources.Stalin));
            }
        }

        /// <summary>
        ///     不足している文字列を追加する
        /// </summary>
        private static void AddInsufficientStrings()
        {
            // 旅団なし
            AddComplementedText("NAME_NONE", Resources.BrigadeNone);

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
        ///     編集済みフラグを更新する
        /// </summary>
        /// <param name="fileName">文字列定義ファイル名</param>
        /// <param name="flag">フラグ状態</param>
        public static void SetDirty(string fileName, bool flag)
        {
            if (flag)
            {
                if (!DirtyFiles.Contains(fileName))
                {
                    DirtyFiles.Add(fileName);
                }
            }
            else
            {
                if (DirtyFiles.Contains(fileName))
                {
                    DirtyFiles.Remove(fileName);
                }
            }
        }
    }

    /// <summary>
    ///     言語モード
    /// </summary>
    public enum LanguageMode
    {
        Japanese, // 日本語版
        English, // 英語版
        PatchedJapanese, // 英語版日本語化
    }

    /// <summary>
    ///     言語コード
    /// </summary>
    public enum LanguageCode
    {
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
        Japanese,
    }
}