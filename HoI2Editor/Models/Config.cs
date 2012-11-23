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
        public static readonly Dictionary<string, string> Text = new Dictionary<string, string>();

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        public static bool Loaded { get; set; }

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
            return Text.ContainsKey(key) ? Text[key] : key;
        }

        /// <summary>
        ///     文字列ファイル群を読み込む
        /// </summary>
        public static void LoadConfigFiles()
        {
            // 読み込み済みならば戻る
            if (Loaded)
            {
                return;
            }

            Text.Clear();
            var fileList = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, "config");
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

            folderName = Path.Combine(Game.FolderName, "config");
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

            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                fileList.Clear();

                folderName = Path.Combine(Game.ModFolderName, "config\\Additional");
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

                folderName = Path.Combine(Game.FolderName, "config\\Additional");
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
            }

            ModifyDuplicatedStrings();

            AddInsufficientStrings();

            Loaded = true;
        }

        /// <summary>
        ///     文字列ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadConfigFile(string fileName)
        {
            var reader = new StreamReader(fileName, Encoding.Default);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string[] tokens = line.Split(CsvSeparator);
                // 空行、コメント行を読み飛ばす
                if (tokens.Length <= 1 || string.IsNullOrEmpty(tokens[0]) || tokens[0][0] == '#')
                {
                    continue;
                }

                // 変換テーブルに登録する
                Text[tokens[0]] = tokens[1];
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
                Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"] += Resources.Army;
                Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"] += Resources.Navy;
            }

            // 偏執的誇大妄想家: ヒトラー/スターリン
            if (Text.ContainsKey("NPERSONALITY_HITLER") &&
                Text.ContainsKey("NPERSONALITY_STALIN") &&
                Text["NPERSONALITY_HITLER"].Equals(Text["NPERSONALITY_STALIN"]))
            {
                Text["NPERSONALITY_HITLER"] += Resources.Hitler;
                Text["NPERSONALITY_STALIN"] += Resources.Stalin;
            }
        }

        /// <summary>
        ///     不足している文字列を追加する
        /// </summary>
        private static void AddInsufficientStrings()
        {
            // DH固有の研究特性
            if (!Text.ContainsKey("RT_AVIONICS"))
            {
                Text["RT_AVIONICS"] = "航空電子工学";
            }
            if (!Text.ContainsKey("RT_MUNITIONS"))
            {
                Text["RT_MUNITIONS"] = "弾薬";
            }
            if (!Text.ContainsKey("RT_VEHICLE_ENGINEERING"))
            {
                Text["RT_VEHICLE_ENGINEERING"] = "車両工学";
            }
            if (!Text.ContainsKey("RT_CARRIER_DESIGN"))
            {
                Text["RT_CARRIER_DESIGN"] = "空母設計";
            }
            if (!Text.ContainsKey("RT_SUBMARINE_DESIGN"))
            {
                Text["RT_SUBMARINE_DESIGN"] = "潜水艦設計";
            }
            if (!Text.ContainsKey("RT_FIGHTER_DESIGN"))
            {
                Text["RT_FIGHTER_DESIGN"] = "戦闘機設計";
            }
            if (!Text.ContainsKey("RT_BOMBER_DESIGN"))
            {
                Text["RT_BOMBER_DESIGN"] = "爆撃機設計";
            }
            if (!Text.ContainsKey("RT_MOUNTAIN_TRAINING"))
            {
                Text["RT_MOUNTAIN_TRAINING"] = "山岳訓練";
            }
            if (!Text.ContainsKey("RT_AIRBORNE_TRAINING"))
            {
                Text["RT_AIRBORNE_TRAINING"] = "空挺訓練";
            }
            if (!Text.ContainsKey("RT_MARINE_TRAINING"))
            {
                Text["RT_MARINE_TRAINING"] = "海兵訓練";
            }
            if (!Text.ContainsKey("RT_MANEUVER_TACTICS"))
            {
                Text["RT_MANEUVER_TACTICS"] = "機動戦術";
            }
            if (!Text.ContainsKey("RT_BLITZKRIEG_TACTICS"))
            {
                Text["RT_BLITZKRIEG_TACTICS"] = "電撃戦戦術";
            }
            if (!Text.ContainsKey("RT_STATIC_DEFENSE_TACTICS"))
            {
                Text["RT_STATIC_DEFENSE_TACTICS"] = "静的防衛戦術";
            }
            if (!Text.ContainsKey("RT_MEDICINE"))
            {
                Text["RT_MEDICINE"] = "医療科学";
            }
            if (!Text.ContainsKey("RT_CAVALRY_TACTICS"))
            {
                Text["RT_CAVALRY_TACTICS"] = "騎兵戦術";
            }
            // ユーザー定義
            for (int i = 1; i <= 60; i++)
            {
                string key = string.Format("RT_USER{0}", i);
                if (!Text.ContainsKey(key))
                {
                    Text[key] = string.Format("ユーザー定義{0}", i);
                }
            }
        }
    }
}