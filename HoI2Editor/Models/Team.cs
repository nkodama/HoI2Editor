using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ
    /// </summary>
    internal class Team
    {
        /// <summary>
        ///     研究特性
        /// </summary>
        public const int SpecialityLength = 32;

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        ///     研究特性名
        /// </summary>
        public static readonly string[] SpecialityNameTable =
            {
                "",
                "artillery",
                "mechanics",
                "electronics",
                "chemistry",
                "training",
                "general_equipment",
                "rocketry",
                "naval_engineering",
                "aeronautics",
                "nuclear_physics",
                "nuclear_engineering",
                "management",
                "industrial_engineering",
                "mathematics",
                "small_unit_tactics",
                "large_unit_tactics",
                "centralized_execution",
                "decentralized_execution",
                "technical_efficiency",
                "individual_courage",
                "infantry_focus",
                "combined_arms_focus",
                "large_unit_focus",
                "naval_artillery",
                "naval_training",
                "aircraft_testing",
                "fighter_tactics",
                "bomber_tactics",
                "large_taskforce_tactics",
                "small_taskforce_tactics",
                "seamanship",
                "piloting",
                "submarine_tactics",
                "carrier_tactics"
            };

        /// <summary>
        ///     研究特性文字列
        /// </summary>
        public static readonly string[] SpecialityTextTable =
            {
                "",
                "RT_ARTILLERY",
                "RT_MECHANICS",
                "RT_ELECTRONICS",
                "RT_CHEMISTRY",
                "RT_TRAINING",
                "RT_GENERAL_EQUIPMENT",
                "RT_ROCKETRY",
                "RT_NAVAL_ENGINEERING",
                "RT_AERONAUTICS",
                "RT_NUCLEAR_PHYSICS",
                "RT_NUCLEAR_ENGINEERING",
                "RT_MANAGEMENT",
                "RT_INDUSTRIAL_ENGINEERING",
                "RT_MATHEMATICS",
                "RT_SMALL_UNIT_TACTICS",
                "RT_LARGE_UNIT_TACTICS",
                "RT_CENTRALIZED_EXECUTION",
                "RT_DECENTRALIZED_EXECUTION",
                "RT_TECHNICAL_EFFICIENCY",
                "RT_INDIVIDUAL_COURAGE",
                "RT_INFANTRY_FOCUS",
                "RT_COMBINED_ARMS_FOCUS",
                "RT_LARGE_UNIT_FOCUS",
                "RT_NAVAL_ARTILLERY",
                "RT_NAVAL_TRAINING",
                "RT_AIRCRAFT_TESTING",
                "RT_FIGHTER_TACTICS",
                "RT_BOMBER_TACTICS",
                "RT_LARGE_TASKFORCE_TACTICS",
                "RT_SMALL_TASKFORCE_TACTICS",
                "RT_SEAMANSHIP",
                "RT_PILOTING",
                "RT_SUBMARINE_TACTICS",
                "RT_CARRIER_TACTICS"
            };

        /// <summary>
        ///     研究特性名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, TechSpeciality> SpecialityNameMap =
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
        ///     研究特性
        /// </summary>
        private readonly TechSpeciality[] _specialities = new TechSpeciality[SpecialityLength];

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Team()
        {
            foreach (
                TechSpeciality speciality in
                    Enum.GetValues(typeof (TechSpeciality))
                        .Cast<TechSpeciality>()
                        .Where(speciality => speciality != TechSpeciality.None))
            {
                SpecialityNameMap.Add(SpecialityNameTable[(int) speciality].ToLower(), speciality);
            }
        }

        /// <summary>
        ///     国タグ
        /// </summary>
        public CountryTag? CountryTag { get; set; }

        /// <summary>
        ///     研究機関ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     研究特性
        /// </summary>
        public TechSpeciality[] Specialities
        {
            get { return _specialities; }
        }

        /// <summary>
        ///     研究機関ファイル群を読み込む
        /// </summary>
        /// <returns>研究機関リスト</returns>
        public static List<Team> LoadTeamFiles()
        {
            var teams = new List<Team>();
            var list = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.TeamPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        LoadTeamFile(fileName, teams);
                        string name = Path.GetFileName(fileName);
                        if (!string.IsNullOrEmpty(name))
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
                    if (!string.IsNullOrEmpty(name) && !list.Contains(name.ToLower()))
                    {
                        LoadTeamFile(fileName, teams);
                    }
                }
            }

            return teams;
        }

        /// <summary>
        ///     研究機関ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        /// <param name="teams">研究機関リスト</param>
        private static void LoadTeamFile(string fileName, List<Team> teams)
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
            if (string.IsNullOrEmpty(line))
            {
                return;
            }
            string[] token = line.Split(CsvSeparator);
            if (token.Length == 0 || string.IsNullOrEmpty(token[0]))
            {
                return;
            }
            // サポート外の国タグの場合は何もしない
            if (!Country.CountryTextMap.ContainsKey(token[0].ToUpper()))
            {
                return;
            }
            CountryTag countryTag = Country.CountryTextMap[token[0].ToUpper()];

            _currentLineNo++;

            while (!reader.EndOfStream)
            {
                ParseTeamLine(reader.ReadLine(), teams, countryTag);
                _currentLineNo++;
            }
        }

        /// <summary>
        ///     研究機関定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="teams">研究機関リスト</param>
        /// <param name="countryTag">国家タグ</param>
        private static void ParseTeamLine(string line, List<Team> teams, CountryTag countryTag)
        {
            // 空行を読み飛ばす
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string[] token = line.Split(CsvSeparator);

            // ID指定のない行は読み飛ばす
            if (string.IsNullOrEmpty(token[0]))
            {
                return;
            }

            // トークン数が足りない行は読み飛ばす
            if (token.Length != 39)
            {
                Log.Write(string.Format("項目数の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (token.Length < 38)
                {
                    return;
                }
            }

            var team = new Team();
            int id;
            if (!int.TryParse(token[0], out id))
            {
                Log.Write(string.Format("IDの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n\n", token[0]));
                return;
            }
            team.Id = id;
            team.Name = token[1];
            team.PictureName = token[2];
            int skill;
            if (int.TryParse(token[3], out skill))
            {
                team.Skill = skill;
            }
            else
            {
                team.Skill = 1;
                Log.Write(string.Format("スキルの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", team.Id, team.Name, token[3]));
            }
            int startYear;
            if (int.TryParse(token[4], out startYear))
            {
                team.StartYear = startYear;
            }
            else
            {
                team.StartYear = 1930;
                Log.Write(string.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", team.Id, team.Name, token[4]));
            }
            int endYear;
            if (int.TryParse(token[5], out endYear))
            {
                team.EndYear = endYear;
            }
            else
            {
                team.EndYear = 1970;
                Log.Write(string.Format("終了年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", team.Id, team.Name, token[5]));
            }
            for (int i = 0; i < SpecialityLength; i++)
            {
                string specialityName = token[6 + i].ToLower();
                if (string.IsNullOrEmpty(specialityName))
                {
                    team.Specialities[i] = TechSpeciality.None;
                    continue;
                }
                if (SpecialityNameMap.ContainsKey(specialityName))
                {
                    team.Specialities[i] = SpecialityNameMap[specialityName];
                }
                else
                {
                    team.Specialities[i] = TechSpeciality.None;
                    Log.Write(string.Format("研究特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1} => {2}\n\n", team.Id, team.Name, token[6 + i]));
                }
            }
            team.CountryTag = countryTag;

            teams.Add(team);
        }

        /// <summary>
        ///     研究機関ファイル群を保存する
        /// </summary>
        /// <param name="teams">研究機関リスト</param>
        /// <param name="dirtyFlags">編集フラグ </param>
        public static void SaveTeamFiles(List<Team> teams, bool[] dirtyFlags)
        {
            foreach (
                CountryTag countryTag in
                    Enum.GetValues(typeof (CountryTag))
                        .Cast<CountryTag>()
                        .Where(countryTag => dirtyFlags[(int) countryTag]))
            {
                SaveTeamFile(teams, countryTag);
            }
        }

        /// <summary>
        ///     研究機関ファイルを保存する
        /// </summary>
        /// <param name="teams">研究機関リスト</param>
        /// <param name="countryTag">国タグ</param>
        private static void SaveTeamFile(IEnumerable<Team> teams, CountryTag countryTag)
        {
            string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName, Game.TeamPathName);
            // 研究機関フォルダが存在しなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetTeamFileName(countryTag));

            _currentFileName = fileName;
            _currentLineNo = 2;

            var writer = new StreamWriter(fileName, false, Encoding.Default);
            writer.WriteLine(
                "{0};Name;Pic Name;Skill;Start Year;End Year;Speciality1;Speciality2;Speciality3;Speciality4;Speciality5;Speciality6;Speciality7;Speciality8;Speciality9;Speciality10;Speciality11;Speciality12;Speciality13;Speciality14;Speciality15;Speciality16;Speciality17;Speciality18;Speciality19;Speciality20;Speciality21;Speciality22;Speciality23;Speciality24;Speciality25;Speciality26;Speciality27;Speciality28;Speciality29;Speciality30;Speciality31;Speciality32;x",
                Country.CountryTextTable[(int) countryTag]);

            foreach (
                Team team in
                    teams.Where(team => team.CountryTag == countryTag).Where(team => team != null))
            {
                writer.Write(
                    "{0};{1};{2};{3};{4};{5}",
                    team.Id,
                    team.Name,
                    team.PictureName,
                    team.Skill,
                    team.StartYear,
                    team.EndYear);
                for (int i = 0; i < SpecialityLength; i++)
                {
                    writer.Write(";{0}",
                                 team.Specialities[i] != TechSpeciality.None
                                     ? SpecialityNameTable[(int) team.Specialities[i]]
                                     : "");
                }
                writer.WriteLine(";x");
                _currentLineNo++;
            }

            writer.Close();
        }
    }

    /// <summary>
    ///     研究特性
    /// </summary>
    public enum TechSpeciality
    {
        None,

        // 共通
        Artillery, // 火砲
        Mechanics, // 機械工学
        Electronics, // 電子工学
        Chemistry, // 化学
        Training, // 訓練
        GeneralEquipment, // 一般装備
        Rocketry, // ロケット工学
        NavalEngineering, // 海軍工学
        Aeronautics, // 航空学
        NuclearPhysics, // 核物理学
        NuclearEngineering, // 核工学
        Management, // 管理
        IndustrialEngineering, // 産業工学
        Mathematics, // 数学
        SmallUnitTactics, // 小規模部隊戦術
        LargeUnitTactics, // 大規模部隊戦術
        CentralizedExecution, // 集中実行
        DecentralizedExecution, // 分散実行
        TechnicalEfficiency, // 技術効率
        IndividualCourage, // 各自の勇気
        InfantryFocus, // 歩兵重視
        CombinedArmsFocus, // 諸兵科連合部隊重視
        LargeUnitFocus, // 大規模部隊重視
        NavalArtillery, // 艦砲
        NavalTraining, // 海軍訓練
        AircraftTesting, // 航空機試験
        FighterTactics, // 戦闘機戦術
        BomberTactics, // 爆撃機戦術
        LargeTaskforceTactics, // 大規模機動部隊戦術
        SmallTaskforceTactics, // 小規模機動部隊戦術
        Seamanship, // 操船術
        Piloting, // 沿岸航法
        SubmarineTactics, // 潜水艦戦術
        CarrierTactics, // 空母戦術
    }
}