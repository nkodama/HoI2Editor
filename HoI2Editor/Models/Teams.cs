using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ群
    /// </summary>
    public static class Teams
    {
        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        public static List<Team> List = new List<Team>();

        /// <summary>
        ///     研究機関編集フラグ
        /// </summary>
        public static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     研究特性文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, TechSpeciality> SpecialityStringMap =
            new Dictionary<string, TechSpeciality>();

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
        ///     研究機関ファイル群を読み込む
        /// </summary>
        public static void LoadTeamFiles()
        {
            // 編集済みフラグを全クリアする
            ClearDirtyFlags();

            List.Clear();

            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    LoadTeamFilesHoI2();
                    break;

                case GameType.DarkestHour:
                    if (Game.IsModActive)
                    {
                        LoadTeamFilesDh();
                    }
                    else
                    {
                        LoadTeamFilesHoI2();
                    }
                    break;
            }
        }

        /// <summary>
        ///     研究機関ファイル群を読み込む(HoI2/AoD/DH-MOD未使用時)
        /// </summary>
        private static void LoadTeamFilesHoI2()
        {
            var list = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.TeamPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadTeamFile(fileName);
                        string name = Path.GetFileName(fileName);
                        if (!String.IsNullOrEmpty(name))
                        {
                            list.Add(name.ToLower());
                        }
                    }
                }
            }

            folderName = Path.Combine(Game.FolderName, Game.TeamPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!String.IsNullOrEmpty(name) && !list.Contains(name.ToLower()))
                    {
                        LoadTeamFile(fileName);
                    }
                }
            }
        }

        /// <summary>
        ///     研究機関ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        private static void LoadTeamFilesDh()
        {
            // teams.txtが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetFileName(Game.DhTeamListPathName);
            if (!File.Exists(listFileName))
            {
                LoadTeamFilesHoI2();
                return;
            }

            IEnumerable<string> fileList = LoadTeamListFileDh(listFileName);
            foreach (string fileName in fileList)
            {
                LoadTeamFile(Game.GetFileName(Path.Combine(Game.TeamPathName, fileName)));
            }
        }

        /// <summary>
        ///     研究機関リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadTeamListFileDh(string fileName)
        {
            var list = new List<string>();
            var reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                // 空行
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }

                // コメント行
                if (line[0] == '#')
                {
                    continue;
                }

                list.Add(line);
            }
            reader.Close();

            return list;
        }

        /// <summary>
        ///     研究機関ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadTeamFile(string fileName)
        {
            _currentFileName = Path.GetFileName(fileName);
            _currentLineNo = 1;

            var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage));
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
            string[] tokens = line.Split(CsvSeparator);
            if (tokens.Length == 0 || String.IsNullOrEmpty(tokens[0]))
            {
                return;
            }
            // サポート外の国タグの場合は何もしない
            if (!Country.CountryStringMap.ContainsKey(tokens[0].ToUpper()))
            {
                return;
            }
            CountryTag country = Country.CountryStringMap[tokens[0].ToUpper()];

            _currentLineNo++;

            while (!reader.EndOfStream)
            {
                ParseTeamLine(reader.ReadLine(), country);
                _currentLineNo++;
            }

            reader.Close();
        }

        /// <summary>
        ///     研究機関定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="country">国家タグ</param>
        private static void ParseTeamLine(string line, CountryTag country)
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
            if (token.Length != 39)
            {
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (token.Length < 38)
                {
                    return;
                }
            }

            var team = new Team {CountryTag = country};
            int id;
            if (!Int32.TryParse(token[0], out id))
            {
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidID, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}\n", token[0]));
                return;
            }
            team.Id = id;
            team.Name = token[1];
            team.PictureName = token[2];
            int skill;
            if (Int32.TryParse(token[3], out skill))
            {
                team.Skill = skill;
            }
            else
            {
                team.Skill = 1;
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidSkill, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", team.Id, team.Name, token[3]));
            }
            int startYear;
            if (Int32.TryParse(token[4], out startYear))
            {
                team.StartYear = startYear;
            }
            else
            {
                team.StartYear = 1930;
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidStartYear, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", team.Id, team.Name, token[4]));
            }
            int endYear;
            if (Int32.TryParse(token[5], out endYear))
            {
                team.EndYear = endYear;
            }
            else
            {
                team.EndYear = 1970;
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidEndYear, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", team.Id, team.Name, token[5]));
            }
            for (int i = 0; i < Team.SpecialityLength; i++)
            {
                string s = token[6 + i].ToLower();
                if (String.IsNullOrEmpty(s))
                {
                    team.Specialities[i] = TechSpeciality.None;
                }
                else if (Tech.SpecialityStringMap.ContainsKey(s))
                {
                    TechSpeciality speciality = Tech.SpecialityStringMap[s];
                    team.Specialities[i] = speciality;
                    if (!Techs.SpecialityTable.Contains(speciality))
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidSpeciality, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1} => {2}\n", team.Id, team.Name, token[6 + i]));
                    }
                }
                else
                {
                    team.Specialities[i] = TechSpeciality.None;
                    Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidSpeciality, _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n", team.Id, team.Name, token[6 + i]));
                }
            }

            List.Add(team);
        }

        /// <summary>
        ///     研究機関ファイル群を保存する
        /// </summary>
        public static void SaveTeamFiles()
        {
            foreach (
                CountryTag country in
                    Enum.GetValues(typeof (CountryTag))
                        .Cast<CountryTag>()
                        .Where(country => DirtyFlags[(int) country] && country != CountryTag.None))
            {
                SaveTeamFile(country);
            }

            // 編集済みフラグを全クリアする
            ClearDirtyFlags();
        }

        /// <summary>
        ///     研究機関ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveTeamFile(CountryTag country)
        {
            string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName, Game.TeamPathName);
            // 研究機関フォルダが存在しなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetTeamFileName(country));

            _currentFileName = fileName;
            _currentLineNo = 2;

            var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage));
            writer.WriteLine(
                "{0};Name;Pic Name;Skill;Start Year;End Year;Speciality1;Speciality2;Speciality3;Speciality4;Speciality5;Speciality6;Speciality7;Speciality8;Speciality9;Speciality10;Speciality11;Speciality12;Speciality13;Speciality14;Speciality15;Speciality16;Speciality17;Speciality18;Speciality19;Speciality20;Speciality21;Speciality22;Speciality23;Speciality24;Speciality25;Speciality26;Speciality27;Speciality28;Speciality29;Speciality30;Speciality31;Speciality32;x",
                Country.CountryTextTable[(int) country]);

            foreach (Team team in List.Where(team => team.CountryTag == country).Where(team => team != null))
            {
                writer.Write(
                    "{0};{1};{2};{3};{4};{5}",
                    team.Id,
                    team.Name,
                    team.PictureName,
                    team.Skill,
                    team.StartYear,
                    team.EndYear);
                for (int i = 0; i < Team.SpecialityLength; i++)
                {
                    writer.Write(";{0}",
                                 team.Specialities[i] != TechSpeciality.None
                                     ? Tech.SpecialityStringTable[(int) team.Specialities[i]]
                                     : "");
                }
                writer.WriteLine(";x");
                _currentLineNo++;
            }

            writer.Close();
        }

        /// <summary>
        ///     研究機関リストに項目を追加する
        /// </summary>
        /// <param name="target">追加対象の項目</param>
        public static void AddItem(Team target)
        {
            List.Add(target);
        }

        /// <summary>
        ///     研究機関リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItemNext(Team target, Team position)
        {
            List.Insert(List.IndexOf(position) + 1, target);
        }

        /// <summary>
        ///     研究機関リストから項目を削除する
        /// </summary>
        /// <param name="target">削除対象の項目</param>
        public static void RemoveItem(Team target)
        {
            List.Remove(target);
        }

        /// <summary>
        ///     研究機関リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の項目</param>
        /// <param name="position">移動先位置の項目</param>
        public static void MoveItem(Team target, Team position)
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