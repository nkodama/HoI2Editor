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
        ///     研究特性リスト
        /// </summary>
        public static TechSpeciality[] SpecialityTable;

        /// <summary>
        ///     研究特性リスト(HoI2)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableHoI2 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting
            };

        /// <summary>
        ///     研究特性リスト(DH1.02)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableDh102 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16
            };

        /// <summary>
        ///     研究特性リスト(DH1.03)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableDh =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.CavalryTactics,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16,
                TechSpeciality.RtUser17,
                TechSpeciality.RtUser18,
                TechSpeciality.RtUser19,
                TechSpeciality.RtUser20,
                TechSpeciality.RtUser21,
                TechSpeciality.RtUser22,
                TechSpeciality.RtUser23,
                TechSpeciality.RtUser24,
                TechSpeciality.RtUser25,
                TechSpeciality.RtUser26,
                TechSpeciality.RtUser27,
                TechSpeciality.RtUser28,
                TechSpeciality.RtUser29,
                TechSpeciality.RtUser30,
                TechSpeciality.RtUser31,
                TechSpeciality.RtUser32,
                TechSpeciality.RtUser33,
                TechSpeciality.RtUser34,
                TechSpeciality.RtUser35,
                TechSpeciality.RtUser36,
                TechSpeciality.RtUser37,
                TechSpeciality.RtUser38,
                TechSpeciality.RtUser39,
                TechSpeciality.RtUser40,
                TechSpeciality.RtUser41,
                TechSpeciality.RtUser42,
                TechSpeciality.RtUser43,
                TechSpeciality.RtUser44,
                TechSpeciality.RtUser45,
                TechSpeciality.RtUser46,
                TechSpeciality.RtUser47,
                TechSpeciality.RtUser48,
                TechSpeciality.RtUser49,
                TechSpeciality.RtUser50,
                TechSpeciality.RtUser51,
                TechSpeciality.RtUser52,
                TechSpeciality.RtUser53,
                TechSpeciality.RtUser54,
                TechSpeciality.RtUser55,
                TechSpeciality.RtUser56,
                TechSpeciality.RtUser57,
                TechSpeciality.RtUser58,
                TechSpeciality.RtUser59,
                TechSpeciality.RtUser60
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
        ///     研究特性を初期化する
        /// </summary>
        public static void InitSpecialities()
        {
            // 研究特性リストを設定する
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    SpecialityTable = SpecialityTableHoI2;
                    break;

                case GameType.DarkestHour:
                    SpecialityTable = SpecialityTableDh102;
                    break;
            }

            // 研究特性文字列とIDの対応付け
            SpecialityStringMap.Clear();
            foreach (
                TechSpeciality speciality in SpecialityTable.Where(speciality => speciality != TechSpeciality.None))
            {
                SpecialityStringMap.Add(Team.SpecialityStringTable[(int) speciality].ToLower(), speciality);
            }
        }

        /// <summary>
        ///     研究機関ファイル群を読み込む
        /// </summary>
        public static void LoadTeamFiles()
        {
            // 編集済みフラグを全クリアする
            ClearDirtyFlags();

            List.Clear();

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
        ///     研究機関ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadTeamFile(string fileName)
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
            // サポート外の国タグの場合は何もしない
            if (!Country.CountryStringMap.ContainsKey(token[0].ToUpper()))
            {
                return;
            }
            CountryTag country = Country.CountryStringMap[token[0].ToUpper()];

            _currentLineNo++;

            while (!reader.EndOfStream)
            {
                ParseTeamLine(reader.ReadLine(), country);
                _currentLineNo++;
            }
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
                string speciality = token[6 + i].ToLower();
                if (String.IsNullOrEmpty(speciality))
                {
                    team.Specialities[i] = TechSpeciality.None;
                }
                else if (SpecialityStringMap.ContainsKey(speciality))
                {
                    team.Specialities[i] = SpecialityStringMap[speciality];
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
                        .Where(country => DirtyFlags[(int) country]))
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

            var writer = new StreamWriter(fileName, false, Encoding.Default);
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
                                     ? Team.SpecialityStringTable[(int) team.Specialities[i]]
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
        /// <param name="target">挿入対象の項目</param>
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
        /// <param name="target"></param>
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