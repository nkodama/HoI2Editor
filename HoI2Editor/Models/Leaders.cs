using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     指揮官データ群
    /// </summary>
    public static class Leaders
    {
        #region 公開プロパティ

        /// <summary>
        ///     マスター指揮官リスト
        /// </summary>
        public static List<Leader> Items { get; private set; }

        /// <summary>
        ///     国タグと指揮官ファイル名の対応付け
        /// </summary>
        public static Dictionary<CountryTag, string> FileNameMap { get; private set; }

        /// <summary>
        ///     兵科名
        /// </summary>
        public static string[] BranchNames { get; private set; }

        /// <summary>
        ///     階級名
        /// </summary>
        public static string[] RankNames { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     国家ごとの編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        ///     現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

        #endregion

        #region 公開定数

        /// <summary>
        ///     指揮官特性値
        /// </summary>
        public static readonly uint[] TraitsValues =
            {
                LeaderTraits.LogisticsWizard,
                LeaderTraits.DefensiveDoctrine,
                LeaderTraits.OffensiveDoctrine,
                LeaderTraits.WinterSpecialist,
                LeaderTraits.Trickster,
                LeaderTraits.Engineer,
                LeaderTraits.FortressBuster,
                LeaderTraits.PanzerLeader,
                LeaderTraits.Commando,
                LeaderTraits.OldGuard,
                LeaderTraits.SeaWolf,
                LeaderTraits.BlockadeRunner,
                LeaderTraits.SuperiorTactician,
                LeaderTraits.Spotter,
                LeaderTraits.TankBuster,
                LeaderTraits.CarpetBomber,
                LeaderTraits.NightFlyer,
                LeaderTraits.FleetDestroyer,
                LeaderTraits.DesertFox,
                LeaderTraits.JungleRat,
                LeaderTraits.UrbanWarfareSpecialist,
                LeaderTraits.Ranger,
                LeaderTraits.Mountaineer,
                LeaderTraits.HillsFighter,
                LeaderTraits.CounterAttacker,
                LeaderTraits.Assaulter,
                LeaderTraits.Encircler,
                LeaderTraits.Ambusher,
                LeaderTraits.Disciplined,
                LeaderTraits.ElasticDefenceSpecialist,
                LeaderTraits.Blitzer
            };

        /// <summary>
        ///     指揮官特性名
        /// </summary>
        public static readonly string[] TraitsNames =
            {
                "TRAIT_LOGWIZ",
                "TRAIT_DEFDOC",
                "TRAIT_OFFDOC",
                "TRAIT_WINSPE",
                "TRAIT_TRICKS",
                "TRAIT_ENGINE",
                "TRAIT_FORBUS",
                "TRAIT_PNZLED",
                "TRAIT_COMMAN",
                "TRAIT_OLDGRD",
                "TRAIT_SEAWOL",
                "TRAIT_BLKRUN",
                "TRAIT_SUPTAC",
                "TRAIT_SPOTTE",
                "TRAIT_TNKBUS",
                "TRAIT_CRPBOM",
                "TRAIT_NGHTFL",
                "TRAIT_FLTDES",
                "TRAIT_DSRFOX",
                "TRAIT_JUNGLE",
                "TRAIT_URBAN",
                "TRAIT_FOREST",
                "TRAIT_MOUNTAIN",
                "TRAIT_HILLS",
                "TRAIT_COUNTER",
                "TRAIT_ASSAULT",
                "TRAIT_ENCIRCL",
                "TRAIT_AMBUSH",
                "TRAIT_DELAY",
                "TRAIT_TATICAL",
                "TRAIT_BREAK"
            };

        #endregion

        #region 内部定数

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Leaders()
        {
            // マスター指揮官リスト
            Items = new List<Leader>();

            // 国タグと指揮官ファイル名の対応付け
            FileNameMap = new Dictionary<CountryTag, string>();

            // 兵科
            BranchNames = new[] {"", Resources.BranchArmy, Resources.BranchNavy, Resources.BranchAirforce};

            // 階級
            RankNames = new[] {"", Resources.Rank3, Resources.Rank2, Resources.Rank1, Resources.Rank0};
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     指揮官ファイルの再読み込みを要求する
        /// </summary>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            Items.Clear();
            FileNameMap.Clear();

            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    LoadHoI2();
                    break;

                case GameType.DarkestHour:
                    LoadDh();
                    break;
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む(HoI2/AoD/DH-MOD未使用時)
        /// </summary>
        private static void LoadHoI2()
        {
            var filelist = new List<string>();
            string folderName;

            // MODフォルダ内の指揮官ファイルを読み込む
            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.LeaderPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            // 指揮官ファイルを読み込む
                            LoadFile(fileName);

                            // 指揮官ファイル一覧に読み込んだファイル名を登録する
                            string name = Path.GetFileName(fileName);
                            if (!String.IsNullOrEmpty(name))
                            {
                                filelist.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                        }
                    }
                }
            }

            // バニラフォルダ内の指揮官ファイルを読み込む
            folderName = Path.Combine(Game.FolderName, Game.LeaderPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    // MODフォルダ内で読み込んだファイルは無視する
                    string name = Path.GetFileName(fileName);
                    if (string.IsNullOrEmpty(name) || filelist.Contains(name.ToLower()))
                    {
                        continue;
                    }

                    try
                    {
                        // 指揮官ファイルを読み込む
                        LoadFile(fileName);
                    }
                    catch (Exception)
                    {
                        Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    }
                }
            }
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        private static void LoadDh()
        {
            // 指揮官リストファイルが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetReadFileName(Game.DhLeaderListPathName);
            if (!File.Exists(listFileName))
            {
                LoadHoI2();
                return;
            }

            // 指揮官リストファイルを読み込む
            IEnumerable<string> fileList;
            try
            {
                fileList = LoadList(listFileName);
            }
            catch (Exception)
            {
                Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, listFileName));
                return;
            }

            foreach (string fileName in fileList.Select(name => Game.GetReadFileName(Game.LeaderPathName, name)))
            {
                try
                {
                    // 指揮官ファイルを読み込む
                    LoadFile(fileName);
                }
                catch (Exception)
                {
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                }
            }
        }

        /// <summary>
        ///     指揮官リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadList(string fileName)
        {
            var list = new List<string>();
            using (var reader = new StreamReader(fileName))
            {
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
            }
            return list;
        }

        /// <summary>
        ///     指揮官ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

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
                    Leader leader = ParseLine(reader.ReadLine());

                    if (country == CountryTag.None && leader != null)
                    {
                        country = leader.Country;
                        if (country != CountryTag.None && !FileNameMap.ContainsKey(country))
                        {
                            FileNameMap.Add(country, Path.GetFileName(fileName));
                        }
                    }
                    _currentLineNo++;
                }
                reader.Close();

                ResetDirty(country);
            }
        }

        /// <summary>
        ///     指揮官定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <returns>指揮官データ</returns>
        private static Leader ParseLine(string line)
        {
            // 空行を読み飛ばす
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] tokens = line.Split(CsvSeparator);

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != (Misc.EnableRetirementYearLeaders ? 19 : 18))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (tokens.Length < (Misc.EnableRetirementYearLeaders ? 18 : 17))
                {
                    return null;
                }
            }

            // 名前指定のない行は読み飛ばす
            if (string.IsNullOrEmpty(tokens[0]))
            {
                return null;
            }

            var leader = new Leader();
            int index = 0;

            // 名前
            leader.Name = tokens[index];
            index++;

            // ID
            int id;
            if (!int.TryParse(tokens[index], out id))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidID, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1}\n", tokens[index], leader.Name));
                return null;
            }
            leader.Id = id;
            index++;

            // 国家
            if (string.IsNullOrEmpty(tokens[index]) || !Country.StringMap.ContainsKey(tokens[index].ToUpper()))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidCountryTag, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
                return null;
            }
            leader.Country = Country.StringMap[tokens[index].ToUpper()];
            index++;

            // 任官年
            for (int i = 0; i < 4; i++)
            {
                int rankYear;
                if (int.TryParse(tokens[index], out rankYear))
                {
                    leader.RankYear[i] = rankYear;
                }
                else
                {
                    leader.RankYear[i] = 1990;
                    Log.Write(string.Format("{0}({1}): {2} L{3}\n", Resources.InvalidRankYear, RankNames[i],
                                            _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
                }
                index++;
            }

            // 理想階級
            int idealRank;
            if (int.TryParse(tokens[index], out idealRank) && 0 <= idealRank && idealRank <= 3)
            {
                leader.IdealRank = (LeaderRank) (4 - idealRank);
            }
            else
            {
                leader.IdealRank = LeaderRank.None;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidIdealRank, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 最大スキル
            int maxSkill;
            if (int.TryParse(tokens[index], out maxSkill))
            {
                leader.MaxSkill = maxSkill;
            }
            else
            {
                leader.MaxSkill = 0;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidMaxSkill, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 指揮官特性
            uint traits;
            if (uint.TryParse(tokens[index], out traits))
            {
                leader.Traits = traits;
            }
            else
            {
                leader.Traits = LeaderTraits.None;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTraits, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // スキル
            int skill;
            if (int.TryParse(tokens[index], out skill))
            {
                leader.Skill = skill;
            }
            else
            {
                leader.Skill = 0;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidSkill, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 経験値
            int experience;
            if (int.TryParse(tokens[index], out experience))
            {
                leader.Experience = experience;
            }
            else
            {
                leader.Experience = 0;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidExperience, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 忠誠度
            int loyalty;
            if (int.TryParse(tokens[index], out loyalty))
            {
                leader.Loyalty = loyalty;
            }
            else
            {
                leader.Loyalty = 0;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidLoyalty, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 兵科
            int branch;
            if (int.TryParse(tokens[index], out branch))
            {
                leader.Branch = (LeaderBranch) (branch + 1);
            }
            else
            {
                leader.Branch = LeaderBranch.None;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidBranch, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 画像ファイル名
            leader.PictureName = tokens[index];
            index++;

            // 開始年
            int startYear;
            if (int.TryParse(tokens[index], out startYear))
            {
                leader.StartYear = startYear;
            }
            else
            {
                leader.StartYear = 1930;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidStartYear, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 終了年
            int endYear;
            if (int.TryParse(tokens[index], out endYear))
            {
                leader.EndYear = endYear;
            }
            else
            {
                leader.EndYear = 1970;
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidEndYear, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
            }
            index++;

            // 引退年
            if (Misc.EnableRetirementYearLeaders)
            {
                int retirementYear;
                if (int.TryParse(tokens[index], out retirementYear))
                {
                    leader.RetirementYear = retirementYear;
                }
                else
                {
                    leader.RetirementYear = 1999;
                    Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidEndYear, _currentFileName,
                                            _currentLineNo));
                    Log.Write(string.Format("  {0}: {1} => {2}\n", leader.Id, leader.Name, tokens[index]));
                }
            }
            else
            {
                leader.RetirementYear = 1999;
            }

            Items.Add(leader);

            return leader;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     指揮官ファイル群を保存する
        /// </summary>
        public static void Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return;
            }

            foreach (
                CountryTag country in Enum.GetValues(typeof (CountryTag))
                                          .Cast<CountryTag>()
                                          .Where(country => DirtyFlags[(int) country] && country != CountryTag.None))
            {
                try
                {
                    // 指揮官ファイルを保存する
                    SaveFile(country);
                }
                catch (Exception)
                {
                    string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                                     Game.LeaderPathName);
                    string fileName = Path.Combine(folderName, Game.GetLeaderFileName(country));
                    Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                }
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;
        }

        /// <summary>
        ///     指揮官ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveFile(CountryTag country)
        {
            // 指揮官フォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.LeaderPathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetLeaderFileName(country));

            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 2;

                // ヘッダ行を書き込む
                writer.WriteLine(
                    Misc.EnableRetirementYearLeaders
                        ? "Name;ID;Country;Rank 3 Year;Rank 2 Year;Rank 1 Year;Rank 0 Year;Ideal Rank;Max Skill;Traits;Skill;Experience;Loyalty;Type;Picture;Start Year;End Year;Retirement Year;x"
                        : "Name;ID;Country;Rank 3 Year;Rank 2 Year;Rank 1 Year;Rank 0 Year;Ideal Rank;Max Skill;Traits;Skill;Experience;Loyalty;Type;Picture;Start Year;End Year;x");

                // 指揮官定義行を順に書き込む
                foreach (Leader leader in Items.Where(leader => leader.Country == country))
                {
                    // 不正な値が設定されている場合は警告をログに出力する
                    if (leader.Branch == LeaderBranch.None)
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidBranch, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1}\n", leader.Id, leader.Name));
                    }
                    if (leader.IdealRank == LeaderRank.None)
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidIdealRank, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1}\n", leader.Id, leader.Name));
                    }

                    // 指揮官定義行を書き込む
                    if (Misc.EnableRetirementYearLeaders)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};x",
                            leader.Name,
                            leader.Id,
                            Country.Strings[(int) leader.Country],
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
                            leader.EndYear,
                            leader.RetirementYear);
                    }
                    else
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};x",
                            leader.Name,
                            leader.Id,
                            Country.Strings[(int) leader.Country],
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
                    }

                    // 編集済みフラグを解除する
                    leader.ResetDirtyAll();

                    _currentLineNo++;
                }
                writer.Close();
            }

            ResetDirty(country);
        }

        #endregion

        #region 指揮官リスト操作

        /// <summary>
        ///     指揮官リストに項目を追加する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        public static void AddItem(Leader leader)
        {
            Items.Add(leader);
        }

        /// <summary>
        ///     指揮官リストに項目を挿入する
        /// </summary>
        /// <param name="leader">挿入対象の項目</param>
        /// <param name="position">挿入先の項目</param>
        public static void InsertItem(Leader leader, Leader position)
        {
            int index = Items.IndexOf(position) + 1;
            Items.Insert(index, leader);
        }

        /// <summary>
        ///     指揮官リストから項目を削除する
        /// </summary>
        /// <param name="leader">削除対象の項目</param>
        public static void RemoveItem(Leader leader)
        {
            Items.Remove(leader);
        }

        /// <summary>
        ///     指揮官リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の項目</param>
        /// <param name="dest">移動先の項目</param>
        public static void MoveItem(Leader src, Leader dest)
        {
            int srcIndex = Items.IndexOf(src);
            int destIndex = Items.IndexOf(dest);

            if (srcIndex > destIndex)
            {
                // 上へ移動する場合
                Items.Insert(destIndex, src);
                Items.RemoveAt(srcIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                Items.Insert(destIndex + 1, src);
                Items.RemoveAt(srcIndex);
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
            return _dirtyFlag;
        }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(CountryTag country)
        {
            return DirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirty(CountryTag country)
        {
            DirtyFlags[(int) country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを解除する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void ResetDirty(CountryTag country)
        {
            DirtyFlags[(int) country] = false;
        }

        #endregion
    }
}