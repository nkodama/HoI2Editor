using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoI2Editor.Models
{
    internal static class Config
    {
        /// <summary>
        /// 文字列変換テーブル
        /// </summary>
        public static readonly Dictionary<string, string> Text = new Dictionary<string, string>();

        /// <summary>
        /// CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        /// 文字列ファイル群を読み込む
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        public static void LoadConfigFiles(string folderName)
        {
            Text.Clear();
            LoadConfigFilesRecursive(folderName);
            ModifyDuplicatedStrings();
        }

        /// <summary>
        /// 文字列ファイル群を再帰的に読み込む
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        private static void LoadConfigFilesRecursive(string folderName)
        {
            // 対象フォルダ内のCSVファイルを順に解析する
            foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
            {
                LoadConfigFile(fileName);
            }

            // 参照フォルダ内のサブフォルダを順に解析する
            foreach (string subFolderName in Directory.GetDirectories(folderName))
            {
                LoadConfigFilesRecursive(subFolderName);
            }
        }

        /// <summary>
        /// 文字列ファイルを読み込む
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
        /// 重複する文字列を修正する
        /// </summary>
        private static void ModifyDuplicatedStrings()
        {
            // 決戦ドクトリン: 陸軍総司令官/海軍総司令官
            if (Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"].Equals(Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"]))
            {
                Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE"] += "(陸軍)";
                Text["NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2"] += "(海軍)";
            }
        }
    }
}