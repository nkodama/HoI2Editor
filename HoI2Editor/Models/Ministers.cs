using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     閣僚データ群
    /// </summary>
    public static class Ministers
    {
        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        public static List<Minister> List = new List<Minister>();

        /// <summary>
        ///     閣僚編集フラグ
        /// </summary>
        public static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     閣僚地位名とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterPosition> PositionNameMap =
            new Dictionary<string, MinisterPosition>();

        /// <summary>
        ///     閣僚特性名とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterPersonality> PersonalityNameMap =
            new Dictionary<string, MinisterPersonality>();

        /// <summary>
        ///     イデオロギー名とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterIdeology> IdeologyNameMap =
            new Dictionary<string, MinisterIdeology>();

        /// <summary>
        ///     忠誠度名とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterLoyalty> LoyaltyNameMap =
            new Dictionary<string, MinisterLoyalty>();

        /// <summary>
        ///     閣僚特性のよくある綴り間違いと特性値の関連付け
        /// </summary>
        private static readonly Dictionary<string, MinisterPersonality> PersonalityTypoMap
            = new Dictionary<string, MinisterPersonality>
                  {
                      {"barking buffon", MinisterPersonality.BarkingBuffoon},
                      {"iron-fisted brute", MinisterPersonality.IronFistedBrute},
                      {"the cloak-n-dagger schemer", MinisterPersonality.TheCloakNDaggerSchemer},
                      {"cloak-n-dagger schemer", MinisterPersonality.TheCloakNDaggerSchemer},
                      {"cloak n dagger schemer", MinisterPersonality.TheCloakNDaggerSchemer},
                      {"laissez-faires capitalist", MinisterPersonality.LaissezFairesCapitalist},
                      {"laissez faire capitalist", MinisterPersonality.LaissezFairesCapitalist},
                      {"laissez-faire capitalist", MinisterPersonality.LaissezFairesCapitalist},
                      {"military entrepeneur", MinisterPersonality.MilitaryEnterpreneur},
                      {"crooked plutocrat", MinisterPersonality.CrookedKleptocrat},
                      {"school of defense", MinisterPersonality.SchoolOfDefence},
                      {"school of maneouvre", MinisterPersonality.SchoolOfManeuvre},
                      {"elastic defense doctrine", MinisterPersonality.ElasticDefenceDoctrine},
                      {"static defense doctrine", MinisterPersonality.StaticDefenceDoctrine},
                      {"vertical envelopement doctrine", MinisterPersonality.VerticalEnvelopmentDoctrine},
                  };

        /// <summary>
        ///     現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        ///     現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Ministers()
        {
            foreach (MinisterPosition position in Enum.GetValues(typeof (MinisterPosition)))
            {
                PositionNameMap.Add(Minister.PositionNameTable[(int) position].ToLower(), position);
            }

            foreach (MinisterPersonality personality in Enum.GetValues(typeof (MinisterPersonality)))
            {
                PersonalityNameMap.Add(Minister.PersonalityNameTable[(int) personality].ToLower(), personality);
            }

            foreach (MinisterLoyalty loyalty in Enum.GetValues(typeof (MinisterLoyalty)))
            {
                LoyaltyNameMap.Add(Minister.LoyaltyNameTable[(int) loyalty].ToLower(), loyalty);
            }

            foreach (MinisterIdeology ideology in Enum.GetValues(typeof (MinisterIdeology)))
            {
                IdeologyNameMap.Add(Minister.IdeologyNameTable[(int) ideology].ToLower(), ideology);
            }
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む
        /// </summary>
        public static void LoadMinisterFiles()
        {
            var fileList = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.MinisterPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadMinisterFile(fileName);
                        string name = Path.GetFileName(fileName);
                        if (!String.IsNullOrEmpty(name))
                        {
                            fileList.Add(name.ToLower());
                        }
                    }
                }
            }

            folderName = Path.Combine(Game.FolderName, Game.MinisterPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!String.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                    {
                        LoadMinisterFile(fileName);
                    }
                }
            }

            // 編集済みフラグを全クリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     閣僚ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadMinisterFile(string fileName)
        {
            _currentFileName = Path.GetFileName(fileName);
            _currentLineNo = 1;

            var reader = new StreamReader(fileName, Encoding.Default);
            // 空ファイルを読み飛ばす
            if (reader.EndOfStream)
            {
                return;
            }

            // 国タグ読み込み
            string line = reader.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                return;
            }
            string[] token = line.Split(CsvSeparator);
            if (token.Length == 0 || String.IsNullOrEmpty(token[0]))
            {
                return;
            }
            CountryTag country = Country.CountryTextMap[token[0].ToUpper()];

            _currentLineNo++;

            while (!reader.EndOfStream)
            {
                ParseMinisterLine(reader.ReadLine(), country);
                _currentLineNo++;
            }
        }

        /// <summary>
        ///     閣僚定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="country">国家タグ</param>
        private static void ParseMinisterLine(string line, CountryTag country)
        {
            // 空行を読み飛ばす
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            string[] token = line.Split(CsvSeparator);

            // ID指定のない行は読み飛ばす
            if (String.IsNullOrEmpty(token[0]))
            {
                return;
            }

            // トークン数が足りない行は読み飛ばす
            if (token.Length != 9)
            {
                Log.Write(String.Format("項目数の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}\n\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (token.Length < 8)
                {
                    return;
                }
            }

            var minister = new Minister();
            int id;
            if (!Int32.TryParse(token[0], out id))
            {
                Log.Write(String.Format("IDの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1}\n\n", token[0], token[2]));
                return;
            }
            minister.Id = id;
            minister.Name = token[2];
            int startYear;
            if (Int32.TryParse(token[3], out startYear))
            {
                minister.StartYear = startYear + 1900;
            }
            else
            {
                minister.StartYear = 1936;
                Log.Write(String.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[3]));
            }
            minister.EndYear = 1970;
            string positionName = token[1].ToLower();
            if (PositionNameMap.ContainsKey(positionName))
            {
                minister.Position = PositionNameMap[positionName];
            }
            else
            {
                minister.Position = null;
                Log.Write(String.Format("閣僚地位の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[1]));
            }
            string ideologyName = token[4].ToLower();
            if (IdeologyNameMap.ContainsKey(ideologyName))
            {
                minister.Ideology = IdeologyNameMap[ideologyName];
            }
            else
            {
                minister.Ideology = null;
                Log.Write(String.Format("イデオロギーの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[4]));
            }
            string personalityName = token[5].ToLower();
            if (PersonalityNameMap.ContainsKey(personalityName))
            {
                minister.Personality = PersonalityNameMap[personalityName];
            }
            else
            {
                if (PersonalityTypoMap.ContainsKey(personalityName))
                {
                    minister.Personality = PersonalityTypoMap[personalityName];
                    Log.Write(String.Format("閣僚特性の修正: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2} -> {3}\n\n", minister.Id, minister.Name, token[5],
                                            Minister.PersonalityNameTable[(int) minister.Personality]));
                }
                else
                {
                    minister.Personality = null;
                    Log.Write(String.Format("閣僚特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[5]));
                }
            }
            string loyaltyName = token[6].ToLower();
            if (LoyaltyNameMap.ContainsKey(loyaltyName))
            {
                minister.Loyalty = LoyaltyNameMap[loyaltyName];
            }
            else
            {
                minister.Loyalty = null;
                Log.Write(String.Format("忠誠度の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[6]));
            }
            minister.PictureName = token[7];
            minister.CountryTag = country;

            List.Add(minister);
        }

        /// <summary>
        ///     閣僚ファイル群を保存する
        /// </summary>
        public static void SaveMinisterFiles()
        {
            foreach (
                CountryTag country in
                    Enum.GetValues(typeof (CountryTag))
                        .Cast<CountryTag>()
                        .Where(country => DirtyFlags[(int) country]))
            {
                SaveMinisterFile(country);
            }

            // 編集済みフラグを全クリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     閣僚ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveMinisterFile(CountryTag country)
        {
            string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                             Game.MinisterPathName);
            // 閣僚フォルダが存在しなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetMinisterFileName(country));

            _currentFileName = fileName;
            _currentLineNo = 3;

            var writer = new StreamWriter(fileName, false, Encoding.Default);
            writer.WriteLine("{0};Ruling Cabinet - Start;Name;Pool;Ideology;Personality;Loyalty;Picturename;x",
                             Country.CountryTextTable[(int) country]);
            writer.WriteLine(";Replacements;;;;;;;x");

            foreach (Minister minister in List.Where(minister => minister.CountryTag == country))
            {
                // 不正な値が設定されている場合は警告をログに出力する
                if (minister.StartYear < 1900 || minister.StartYear > 1999)
                {
                    Log.Write(String.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, minister.StartYear));
                }
                if (minister.EndYear < 1900 || minister.EndYear > 1999)
                {
                    Log.Write(String.Format("終了年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, minister.EndYear));
                }
                if (minister.Position == null)
                {
                    Log.Write(String.Format("閣僚地位の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Personality == null)
                {
                    Log.Write(String.Format("閣僚特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Ideology == null)
                {
                    Log.Write(String.Format("イデオロギーの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Loyalty == null)
                {
                    Log.Write(String.Format("忠誠度の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(String.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }

                writer.WriteLine(
                    "{0};{1};{2};{3};{4};{5};{6};{7};x",
                    minister.Id,
                    minister.Position != null ? Minister.PositionNameTable[(int) minister.Position] : "",
                    minister.Name,
                    minister.StartYear - 1900,
                    minister.Ideology != null ? Minister.IdeologyNameTable[(int) minister.Ideology] : "",
                    minister.Personality != null ? Minister.PersonalityNameTable[(int) minister.Personality] : "",
                    minister.Loyalty != null ? Minister.LoyaltyNameTable[(int) minister.Loyalty] : "",
                    minister.PictureName);
                _currentLineNo++;
            }

            writer.Close();
        }

        /// <summary>
        ///     閣僚リストに項目を追加する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        public static void AddItem(Minister target)
        {
            List.Add(target);
        }

        /// <summary>
        ///     閣僚リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItemNext(Minister target, Minister position)
        {
            List.Insert(List.IndexOf(position) + 1, target);
        }

        /// <summary>
        ///     閣僚リストから項目を削除する
        /// </summary>
        /// <param name="target"></param>
        public static void RemoveItem(Minister target)
        {
            List.Remove(target);
        }

        /// <summary>
        ///     閣僚リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の項目</param>
        /// <param name="position">移動先位置の項目</param>
        public static void MoveItem(Minister target, Minister position)
        {
            int targetIndex = List.IndexOf(target);
            int positionIndex = List.IndexOf(position);

            if (targetIndex > positionIndex)
            {
                // 上へ移動する場合
                List.Insert(positionIndex, target);
                List.RemoveAt(targetIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                List.Insert(positionIndex + 1, target);
                List.RemoveAt(targetIndex);
            }
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirtyFlag(CountryTag country)
        {
            DirtyFlags[(int) country] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void ClearDirtyFlag(CountryTag country)
        {
            DirtyFlags[(int) country] = false;
        }

        /// <summary>
        ///     編集フラグを全てクリアする
        /// </summary>
        private static void ClearDirtyFlags()
        {
            foreach (CountryTag country in Enum.GetValues(typeof (CountryTag)))
            {
                ClearDirtyFlag(country);
            }
        }
    }
}