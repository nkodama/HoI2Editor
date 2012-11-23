using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     指揮官データ群
    /// </summary>
    public static class Leaders
    {
        /// <summary>
        ///     マスター指揮官リスト
        /// </summary>
        public static List<Leader> List = new List<Leader>();

        /// <summary>
        ///     指揮官編集フラグ
        /// </summary>
        public static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     国タグと指揮官ファイル名の対応付け
        /// </summary>
        public static readonly Dictionary<CountryTag, string> FileNameMap = new Dictionary<CountryTag, string>();

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
        ///     指揮官ファイル群を読み込む
        /// </summary>
        public static void LoadLeaderFiles()
        {
            List.Clear();
            FileNameMap.Clear();

            var filelist = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.LeaderPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadLeaderFile(fileName);
                        string name = Path.GetFileName(fileName);
                        if (!String.IsNullOrEmpty(name))
                        {
                            filelist.Add(name.ToLower());
                        }
                    }
                }
            }

            folderName = Path.Combine(Game.FolderName, Game.LeaderPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!String.IsNullOrEmpty(name) && !filelist.Contains(name.ToLower()))
                    {
                        LoadLeaderFile(fileName);
                    }
                }
            }

            // 編集済みフラグを全クリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     指揮官ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadLeaderFile(string fileName)
        {
            _currentFileName = Path.GetFileName(fileName);
            _currentLineNo = 1;

            var reader = new StreamReader(fileName, Encoding.Default);
            // 空ファイルを読み飛ばす
            if (reader.EndOfStream)
            {
                return;
            }

            // ヘッダ行読み込み
            string line = reader.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            _currentLineNo++;
            var country = CountryTag.None;

            while (!reader.EndOfStream)
            {
                Leader leader = ParseLeaderLine(reader.ReadLine());

                if (country == CountryTag.None && leader != null)
                {
                    country = leader.CountryTag;
                    if (country != CountryTag.None && !FileNameMap.ContainsKey(country))
                    {
                        FileNameMap.Add(country, Path.GetFileName(fileName));
                    }
                }
                _currentLineNo++;
            }
        }

        /// <summary>
        ///     指揮官定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <returns>指揮官データ</returns>
        private static Leader ParseLeaderLine(string line)
        {
            // 空行を読み飛ばす
            if (String.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] token = line.Split(CsvSeparator);

            // トークン数が足りない行は読み飛ばす
            if (token.Length != 18)
            {
                Log.Write(String.Format("項目数の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}\n\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (token.Length < 17)
                {
                    return null;
                }
            }

            // 名前指定のない行は読み飛ばす
            if (String.IsNullOrEmpty(token[0]))
            {
                return null;
            }

            var leader = new Leader();
            int id;
            if (!Int32.TryParse(token[1], out id))
            {
                Log.Write(String.Format("IDの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1}\n\n", token[1], token[0]));
                return null;
            }
            leader.Id = id;
            leader.Name = token[0];
            if (String.IsNullOrEmpty(token[2]) || !Country.CountryTextMap.ContainsKey(token[2].ToUpper()))
            {
                Log.Write(String.Format("国タグの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[2]));
                return null;
            }
            leader.CountryTag = Country.CountryTextMap[token[2].ToUpper()];
            for (int i = 0; i < 4; i++)
            {
                int rankYear;
                if (Int32.TryParse(token[3 + i], out rankYear))
                {
                    leader.RankYear[i] = rankYear;
                }
                else
                {
                    leader.RankYear[i] = 1990;
                    Log.Write(String.Format("{0}任官年の異常: {1} L{2} \n", Leader.RankTextTable[i], _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[3 + i]));
                }
            }
            int idealRank;
            if (Int32.TryParse(token[7], out idealRank) && 0 <= idealRank && idealRank <= 3)
            {
                leader.IdealRank = (LeaderRank) (4 - idealRank);
            }
            else
            {
                leader.IdealRank = LeaderRank.None;
                Log.Write(String.Format("理想階級の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[7]));
            }
            int maxSkill;
            if (Int32.TryParse(token[8], out maxSkill))
            {
                leader.MaxSkill = maxSkill;
            }
            else
            {
                leader.MaxSkill = 0;
                Log.Write(String.Format("最大スキルの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[8]));
            }
            uint traits;
            if (UInt32.TryParse(token[9], out traits))
            {
                leader.Traits = traits;
            }
            else
            {
                leader.Traits = LeaderTraits.None;
                Log.Write(String.Format("特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[9]));
            }
            int skill;
            if (Int32.TryParse(token[10], out skill))
            {
                leader.Skill = skill;
            }
            else
            {
                leader.Skill = 0;
                Log.Write(String.Format("スキルの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[10]));
            }
            int experience;
            if (Int32.TryParse(token[11], out experience))
            {
                leader.Experience = experience;
            }
            else
            {
                leader.Experience = 0;
                Log.Write(String.Format("経験値の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[11]));
            }
            int loyalty;
            if (Int32.TryParse(token[12], out loyalty))
            {
                leader.Loyalty = loyalty;
            }
            else
            {
                leader.Loyalty = 0;
                Log.Write(String.Format("忠誠度の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[12]));
            }
            int branch;
            if (Int32.TryParse(token[13], out branch))
            {
                leader.Branch = (LeaderBranch) (branch + 1);
            }
            else
            {
                leader.Branch = LeaderBranch.None;
                Log.Write(String.Format("兵科の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[13]));
            }
            leader.PictureName = token[14];
            int startYear;
            if (Int32.TryParse(token[15], out startYear))
            {
                leader.StartYear = startYear;
            }
            else
            {
                leader.StartYear = 1930;
                Log.Write(String.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[15]));
            }
            int endYear;
            if (Int32.TryParse(token[16], out endYear))
            {
                leader.EndYear = endYear;
            }
            else
            {
                leader.EndYear = 1970;
                Log.Write(String.Format("終了年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n\n", leader.Id, leader.Name, token[16]));
            }

            List.Add(leader);

            return leader;
        }

        /// <summary>
        ///     指揮官ファイル群を保存する
        /// </summary>
        public static void SaveLeaderFiles()
        {
            foreach (
                CountryTag country in
                    Enum.GetValues(typeof (CountryTag)).Cast<CountryTag>().Where(country => DirtyFlags[(int) country]))
            {
                SaveLeaderFile(country);
            }

            // 編集済みフラグを全クリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     指揮官ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveLeaderFile(CountryTag country)
        {
            string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                             Game.LeaderPathName);
            // 指揮官フォルダが存在しなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetLeaderFileName(country));

            _currentFileName = fileName;
            _currentLineNo = 2;

            var writer = new StreamWriter(fileName, false, Encoding.Default);
            writer.WriteLine(
                "Name;ID;Country;Rank 3 Year;Rank 2 Year;Rank 1 Year;Rank 0 Year;Ideal Rank;Max Skill;Traits;Skill;Experience;Loyalty;Type;Picture;Start Year;End Year;x");

            foreach (Leader leader in List.Where(leader => leader.CountryTag == country))
            {
                writer.WriteLine(
                    "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};x",
                    leader.Name,
                    leader.Id,
                    Country.CountryTextTable[(int) leader.CountryTag],
                    leader.RankYear[0],
                    leader.RankYear[1],
                    leader.RankYear[2],
                    leader.RankYear[3],
                    leader.IdealRank != LeaderRank.None
                        ? (4 - (int) leader.IdealRank).ToString(CultureInfo.InvariantCulture)
                        : "",
                    leader.MaxSkill,
                    leader.Traits,
                    leader.Skill,
                    leader.Experience,
                    leader.Loyalty,
                    leader.Branch != LeaderBranch.None
                        ? ((int) (leader.Branch - 1)).ToString(CultureInfo.InvariantCulture)
                        : "",
                    leader.PictureName,
                    leader.StartYear,
                    leader.EndYear);
                _currentLineNo++;
            }

            writer.Close();
        }

        /// <summary>
        ///     指揮官リストに項目を追加する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        public static void AddItem(Leader target)
        {
            List.Add(target);
        }

        /// <summary>
        ///     指揮官リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItemNext(Leader target, Leader position)
        {
            List.Insert(List.IndexOf(position) + 1, target);
        }

        /// <summary>
        ///     指揮官リストから項目を削除する
        /// </summary>
        /// <param name="target"></param>
        public static void RemoveItem(Leader target)
        {
            List.Remove(target);
        }

        /// <summary>
        ///     指揮官リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の項目</param>
        /// <param name="position">移動先位置の項目</param>
        public static void MoveItem(Leader target, Leader position)
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