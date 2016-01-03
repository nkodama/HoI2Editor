using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

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
        public static List<Leader> Items { get; }

        /// <summary>
        ///     国タグと指揮官ファイル名の対応付け
        /// </summary>
        public static Dictionary<Country, string> FileNameMap { get; }

        /// <summary>
        ///     使用済みIDリスト
        /// </summary>
        public static HashSet<int> IdSet { get; }

        /// <summary>
        ///     階級名
        /// </summary>
        public static string[] RankNames { get; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     遅延読み込み用
        /// </summary>
        private static readonly BackgroundWorker Worker = new BackgroundWorker();

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     国家ごとの編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     指揮官リストファイルの編集済みフラグ
        /// </summary>
        private static bool _dirtyListFlag;

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

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Leaders()
        {
            // マスター指揮官リスト
            Items = new List<Leader>();

            // 国タグと指揮官ファイル名の対応付け
            FileNameMap = new Dictionary<Country, string>();

            // 使用済みIDリスト
            IdSet = new HashSet<int>();

            // 階級
            RankNames = new[] { "", Resources.Rank3, Resources.Rank2, Resources.Rank1, Resources.Rank0 };
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     指揮官ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     指揮官ファイル群を再読み込みする
        /// </summary>
        public static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            Load();
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 既に読み込み済みならば何もしない
            if (_loaded)
            {
                return;
            }

            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
                return;
            }

            LoadFiles();
        }

        /// <summary>
        ///     指揮官ファイル群を遅延読み込みする
        /// </summary>
        /// <param name="handler">読み込み完了イベントハンドラ</param>
        public static void LoadAsync(RunWorkerCompletedEventHandler handler)
        {
            // 既に読み込み済みならば完了イベントハンドラを呼び出す
            if (_loaded)
            {
                handler?.Invoke(null, new RunWorkerCompletedEventArgs(null, null, false));
                return;
            }

            // 読み込み完了イベントハンドラを登録する
            if (handler != null)
            {
                Worker.RunWorkerCompleted += handler;
                Worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
            }

            // 読み込み途中ならば戻る
            if (Worker.IsBusy)
            {
                return;
            }

            // ここで読み込み済みならば既に完了イベントハンドラを呼び出しているので何もせずに戻る
            if (_loaded)
            {
                return;
            }

            // 遅延読み込みを開始する
            Worker.DoWork += OnWorkerDoWork;
            Worker.RunWorkerAsync();
        }

        /// <summary>
        ///     読み込み完了まで待機する
        /// </summary>
        public static void WaitLoading()
        {
            while (Worker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     遅延読み込み中かどうかを判定する
        /// </summary>
        /// <returns>遅延読み込み中ならばtrueを返す</returns>
        public static bool IsLoading()
        {
            return Worker.IsBusy;
        }

        /// <summary>
        ///     遅延読み込み処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            LoadFiles();
        }

        /// <summary>
        ///     遅延読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 遅延読み込み完了時の処理
            HoI2EditorController.OnLoadingCompleted();
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む
        /// </summary>
        private static void LoadFiles()
        {
            Items.Clear();
            IdSet.Clear();
            FileNameMap.Clear();

            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    if (!LoadHoI2())
                    {
                        return;
                    }
                    break;

                case GameType.DarkestHour:
                    if (!LoadDh())
                    {
                        return;
                    }
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
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadHoI2()
        {
            List<string> filelist = new List<string>();
            string folderName;
            bool error = false;

            // 保存フォルダ内の指揮官ファイルを読み込む
            if (Game.IsExportFolderActive)
            {
                folderName = Game.GetExportFileName(Game.LeaderPathName);
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
                            if (!string.IsNullOrEmpty(name))
                            {
                                filelist.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Leader] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorLeader, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // MODフォルダ内の指揮官ファイルを読み込む
            if (Game.IsModActive)
            {
                folderName = Game.GetModFileName(Game.LeaderPathName);
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
                            if (!string.IsNullOrEmpty(name))
                            {
                                filelist.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Leader] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorLeader, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
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
                        error = true;
                        Log.Error("[Leader] Read error: {0}", fileName);
                        if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                            Resources.EditorLeader, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            == DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            return !error;
        }

        /// <summary>
        ///     指揮官ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadDh()
        {
            // 指揮官リストファイルが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetReadFileName(Game.DhLeaderListPathName);
            if (!File.Exists(listFileName))
            {
                return LoadHoI2();
            }

            // 指揮官リストファイルを読み込む
            IEnumerable<string> fileList;
            try
            {
                fileList = LoadList(listFileName);
            }
            catch (Exception)
            {
                Log.Error("[Leader] Read error: {0}", listFileName);
                MessageBox.Show($"{Resources.FileReadError}: {listFileName}",
                    Resources.EditorLeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool error = false;
            foreach (string fileName in fileList.Select(name => Game.GetReadFileName(Game.LeaderPathName, name)))
            {
                try
                {
                    // 指揮官ファイルを読み込む
                    LoadFile(fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Log.Error("[Leader] Read error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorLeader, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return !error;
        }

        /// <summary>
        ///     指揮官リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadList(string fileName)
        {
            Log.Verbose("[Leader] Load: {0}", Path.GetFileName(fileName));

            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // 空行
                    if (string.IsNullOrEmpty(line))
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
            }
            return list;
        }

        /// <summary>
        ///     指揮官ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadFile(string fileName)
        {
            Log.Verbose("[Leader] Load: {0}", Path.GetFileName(fileName));

            using (CsvLexer lexer = new CsvLexer(fileName))
            {
                // 空ファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                // ヘッダ行読み込み
                lexer.SkipLine();

                // ヘッダ行のみのファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                // 1行ずつ順に読み込む
                Country country = Country.None;
                while (!lexer.EndOfStream)
                {
                    Leader leader = ParseLine(lexer);

                    // 空行を読み飛ばす
                    if (leader == null)
                    {
                        continue;
                    }

                    Items.Add(leader);

                    if (country == Country.None)
                    {
                        country = leader.Country;
                        if (country != Country.None && !FileNameMap.ContainsKey(country))
                        {
                            FileNameMap.Add(country, lexer.FileName);
                        }
                    }
                }

                ResetDirty(country);
            }
        }

        /// <summary>
        ///     指揮官定義行を解釈する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <returns>指揮官データ</returns>
        private static Leader ParseLine(CsvLexer lexer)
        {
            string[] tokens = lexer.GetTokens();

            // 空行を読み飛ばす
            if (tokens == null)
            {
                return null;
            }

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != (Misc.EnableRetirementYearLeaders ? 19 : 18))
            {
                Log.Warning("[Leader] Invalid token count: {0} ({1} L{2})", tokens.Length, lexer.FileName, lexer.LineNo);
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

            Leader leader = new Leader();
            int index = 0;

            // 名前
            leader.Name = tokens[index];
            index++;

            // ID
            int id;
            if (!int.TryParse(tokens[index], out id))
            {
                Log.Warning("[Leader] Invalid id: {0} [{1}] ({1} L{2})", tokens[index], leader.Name, lexer.FileName,
                    lexer.LineNo);
                return null;
            }
            leader.Id = id;
            index++;

            // 国家
            if (string.IsNullOrEmpty(tokens[index]) || !Countries.StringMap.ContainsKey(tokens[index].ToUpper()))
            {
                Log.Warning("[Leader] Invalid country: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id, leader.Name,
                    lexer.FileName, lexer.LineNo);
                return null;
            }
            leader.Country = Countries.StringMap[tokens[index].ToUpper()];
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
                    Log.Warning("[Leader] Invalid rank{0} year: {1} [{2}: {3}] ({4} L{5})", i, tokens[index], leader.Id,
                        leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid ideal rank: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid max skill: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid trait: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid skill: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id, leader.Name,
                    lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid experience: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid loyalty: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id, leader.Name,
                    lexer.FileName, lexer.LineNo);
            }
            index++;

            // 兵科
            int branch;
            if (int.TryParse(tokens[index], out branch))
            {
                leader.Branch = (Branch) (branch + 1);
            }
            else
            {
                leader.Branch = Branch.None;
                Log.Warning("[Leader] Invalid branch: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id, leader.Name,
                    lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid start year: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                Log.Warning("[Leader] Invalid end year: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                    leader.Name, lexer.FileName, lexer.LineNo);
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
                    Log.Warning("[Leader] Invalid retirement year: {0} [{1}: {2}] ({3} L{4})", tokens[index], leader.Id,
                        leader.Name, lexer.FileName, lexer.LineNo);
                }
            }
            else
            {
                leader.RetirementYear = 1999;
            }

            return leader;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     指揮官ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return true;
            }

            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
            }

            // 指揮官リストファイルを保存する
            if ((Game.Type == GameType.DarkestHour) && IsDirtyList())
            {
                try
                {
                    SaveList();
                }
                catch (Exception)
                {
                    string fileName = Game.GetWriteFileName(Game.DhLeaderListPathName);
                    Log.Error("[Leader] Write error: {0}", fileName);
                    MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorLeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            bool error = false;
            foreach (Country country in Countries.Tags
                .Where(country => DirtyFlags[(int) country] && country != Country.None))
            {
                try
                {
                    // 指揮官ファイルを保存する
                    SaveFile(country);
                }
                catch (Exception)
                {
                    error = true;
                    string fileName = Game.GetWriteFileName(Game.LeaderPathName, Game.GetLeaderFileName(country));
                    Log.Error("[Leader] Write error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileWriteError}: {fileName}",
                        Resources.EditorLeader, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            // 保存に失敗していれば戻る
            if (error)
            {
                return false;
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            return true;
        }

        /// <summary>
        ///     指揮官リストファイルを保存する (DH)
        /// </summary>
        private static void SaveList()
        {
            // データベースフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.DatabasePathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Game.GetWriteFileName(Game.DhLeaderListPathName);
            Log.Info("[Leader] Save: {0}", Path.GetFileName(fileName));

            // 登録された指揮官ファイル名を順に書き込む
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                foreach (string name in FileNameMap.Select(pair => pair.Value))
                {
                    writer.WriteLine(name);
                }
            }

            // 編集済みフラグを解除する
            ResetDirtyList();
        }

        /// <summary>
        ///     指揮官ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveFile(Country country)
        {
            // 指揮官フォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.LeaderPathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string name = Game.GetLeaderFileName(country);
            string fileName = Path.Combine(folderName, name);
            Log.Info("[Leader] Save: {0}", name);

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                int lineNo = 2;

                // ヘッダ行を書き込む
                writer.WriteLine(
                    Misc.EnableRetirementYearLeaders
                        ? "Name;ID;Country;Rank 3 Year;Rank 2 Year;Rank 1 Year;Rank 0 Year;Ideal Rank;Max Skill;Traits;Skill;Experience;Loyalty;Type;Picture;Start Year;End Year;Retirement Year;x"
                        : "Name;ID;Country;Rank 3 Year;Rank 2 Year;Rank 1 Year;Rank 0 Year;Ideal Rank;Max Skill;Traits;Skill;Experience;Loyalty;Type;Picture;Start Year;End Year;x");

                // 指揮官定義行を順に書き込む
                foreach (Leader leader in Items.Where(leader => leader.Country == country))
                {
                    // 不正な値が設定されている場合は警告をログに出力する
                    if (leader.Branch == Branch.None)
                    {
                        Log.Warning("[Leader] Invalid branch: {0} {1} ({2} L{3})", leader.Id, leader.Name, name, lineNo);
                    }
                    if (leader.IdealRank == LeaderRank.None)
                    {
                        Log.Warning("[Leader] Invalid ideal rank: {0} {1} ({2} L{3})", leader.Id, leader.Name, name,
                            lineNo);
                    }

                    // 指揮官定義行を書き込む
                    if (Misc.EnableRetirementYearLeaders)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};x",
                            leader.Name,
                            leader.Id,
                            Countries.Strings[(int) leader.Country],
                            leader.RankYear[0],
                            leader.RankYear[1],
                            leader.RankYear[2],
                            leader.RankYear[3],
                            leader.IdealRank != LeaderRank.None ? IntHelper.ToString(4 - (int) leader.IdealRank) : "",
                            leader.MaxSkill,
                            leader.Traits,
                            leader.Skill,
                            leader.Experience,
                            leader.Loyalty,
                            leader.Branch != Branch.None ? IntHelper.ToString((int) (leader.Branch - 1)) : "",
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
                            Countries.Strings[(int) leader.Country],
                            leader.RankYear[0],
                            leader.RankYear[1],
                            leader.RankYear[2],
                            leader.RankYear[3],
                            leader.IdealRank != LeaderRank.None ? IntHelper.ToString(4 - (int) leader.IdealRank) : "",
                            leader.MaxSkill,
                            leader.Traits,
                            leader.Skill,
                            leader.Experience,
                            leader.Loyalty,
                            leader.Branch != Branch.None ? IntHelper.ToString((int) (leader.Branch - 1)) : "",
                            leader.PictureName,
                            leader.StartYear,
                            leader.EndYear);
                    }

                    // 編集済みフラグを解除する
                    leader.ResetDirtyAll();

                    lineNo++;
                }
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
            Log.Info("[Leader] Add leader: ({0}: {1}) <{2}>", leader.Id, leader.Name,
                Countries.Strings[(int) leader.Country]);

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

            Log.Info("[Leader] Insert leader: {0} ({1}: {2}) <{3}>", index, leader.Id, leader.Name,
                Countries.Strings[(int) leader.Country]);

            Items.Insert(index, leader);
        }

        /// <summary>
        ///     指揮官リストから項目を削除する
        /// </summary>
        /// <param name="leader">削除対象の項目</param>
        public static void RemoveItem(Leader leader)
        {
            Log.Info("[Leader] Remove leader: ({0}: {1}) <{2}>", leader.Id, leader.Name,
                Countries.Strings[(int) leader.Country]);

            Items.Remove(leader);

            // 使用済みIDリストから削除する
            IdSet.Remove(leader.Id);
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

            Log.Info("[Leader] Move leader: {0} -> {1} ({2}: {3}) <{4}>", srcIndex, destIndex, src.Id, src.Name,
                Countries.Strings[(int) src.Country]);

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

        #region 一括編集

        /// <summary>
        ///     一括編集
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        public static void BatchEdit(LeaderBatchEditArgs args)
        {
            LogBatchEdit(args);

            IEnumerable<Leader> leaders = GetBatchEditLeaders(args);
            if (args.CopyCountry == Country.None)
            {
                foreach (Leader leader in leaders)
                {
                    BatchEditLeader(leader, args);
                }
            }
            else
            {
                Country newCountry = args.CopyCountry;
                int id = args.Id;
                foreach (Leader leader in leaders)
                {
                    id = GetNewId(id);
                    Leader newLeader = new Leader(leader)
                    {
                        Country = newCountry,
                        Id = id
                    };
                    newLeader.SetDirtyAll();
                    BatchEditLeader(newLeader, args);
                    Items.Add(newLeader);
                }
            }
        }

        /// <summary>
        ///     一括編集の個別処理
        /// </summary>
        /// <param name="leader">対象指揮官</param>
        /// <param name="args">一括編集のパラメータ</param>
        private static void BatchEditLeader(Leader leader, LeaderBatchEditArgs args)
        {
            // 理想階級
            if (args.Items[(int) LeaderBatchItemId.IdealRank])
            {
                if (leader.IdealRank != args.IdealRank)
                {
                    leader.IdealRank = args.IdealRank;
                    leader.SetDirty(LeaderItemId.IdealRank);
                    SetDirty(leader.Country);
                }
            }

            // スキル
            if (args.Items[(int) LeaderBatchItemId.Skill])
            {
                if (leader.Skill != args.Skill)
                {
                    leader.Skill = args.Skill;
                    leader.SetDirty(LeaderItemId.Skill);
                    SetDirty(leader.Country);
                }
            }

            // 最大スキル
            if (args.Items[(int) LeaderBatchItemId.MaxSkill])
            {
                if (leader.MaxSkill != args.MaxSkill)
                {
                    leader.MaxSkill = args.MaxSkill;
                    leader.SetDirty(LeaderItemId.MaxSkill);
                    SetDirty(leader.Country);
                }
            }

            // 経験値
            if (args.Items[(int) LeaderBatchItemId.Experience])
            {
                if (leader.Experience != args.Experience)
                {
                    leader.Experience = args.Experience;
                    leader.SetDirty(LeaderItemId.Experience);
                    SetDirty(leader.Country);
                }
            }

            // 忠誠度
            if (args.Items[(int) LeaderBatchItemId.Loyalty])
            {
                if (leader.Loyalty != args.Loyalty)
                {
                    leader.Loyalty = args.Loyalty;
                    leader.SetDirty(LeaderItemId.Loyalty);
                    SetDirty(leader.Country);
                }
            }

            // 開始年
            if (args.Items[(int) LeaderBatchItemId.StartYear])
            {
                if (leader.StartYear != args.StartYear)
                {
                    leader.StartYear = args.StartYear;
                    leader.SetDirty(LeaderItemId.StartYear);
                    SetDirty(leader.Country);
                }
            }

            // 終了年
            if (args.Items[(int) LeaderBatchItemId.EndYear])
            {
                if (leader.EndYear != args.EndYear)
                {
                    leader.EndYear = args.EndYear;
                    leader.SetDirty(LeaderItemId.EndYear);
                    SetDirty(leader.Country);
                }
            }

            // 引退年
            if (args.Items[(int) LeaderBatchItemId.RetirementYear])
            {
                if (leader.RetirementYear != args.RetirementYear)
                {
                    leader.RetirementYear = args.RetirementYear;
                    leader.SetDirty(LeaderItemId.RetirementYear);
                    SetDirty(leader.Country);
                }
            }

            // 少将任官年
            if (args.Items[(int) LeaderBatchItemId.Rank3Year])
            {
                if (leader.RankYear[0] != args.RankYear[0])
                {
                    leader.RankYear[0] = args.RankYear[0];
                    leader.SetDirty(LeaderItemId.Rank3Year);
                    SetDirty(leader.Country);
                }
            }

            // 中将任官年
            if (args.Items[(int) LeaderBatchItemId.Rank2Year])
            {
                if (leader.RankYear[1] != args.RankYear[1])
                {
                    leader.RankYear[1] = args.RankYear[1];
                    leader.SetDirty(LeaderItemId.Rank2Year);
                    SetDirty(leader.Country);
                }
            }

            // 大将任官年
            if (args.Items[(int) LeaderBatchItemId.Rank1Year])
            {
                if (leader.RankYear[2] != args.RankYear[2])
                {
                    leader.RankYear[2] = args.RankYear[2];
                    leader.SetDirty(LeaderItemId.Rank1Year);
                    SetDirty(leader.Country);
                }
            }

            // 元帥任官年
            if (args.Items[(int) LeaderBatchItemId.Rank0Year])
            {
                if (leader.RankYear[3] != args.RankYear[3])
                {
                    leader.RankYear[3] = args.RankYear[3];
                    leader.SetDirty(LeaderItemId.Rank0Year);
                    SetDirty(leader.Country);
                }
            }
        }

        /// <summary>
        ///     一括編集対象の指揮官リストを取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>一括編集対象の指揮官リスト</returns>
        private static IEnumerable<Leader> GetBatchEditLeaders(LeaderBatchEditArgs args)
        {
            return args.Mode == BatchMode.All
                ? Items
                    .Where(leader =>
                        (leader.Branch == Branch.Army && args.Army) ||
                        (leader.Branch == Branch.Navy && args.Navy) ||
                        (leader.Branch == Branch.Airforce && args.Airforce))
                    .ToList()
                : Items
                    .Where(leader => args.TargetCountries.Contains(leader.Country))
                    .Where(leader =>
                        (leader.Branch == Branch.Army && args.Army) ||
                        (leader.Branch == Branch.Navy && args.Navy) ||
                        (leader.Branch == Branch.Airforce && args.Airforce))
                    .ToList();
        }

        /// <summary>
        ///     一括編集処理のログ出力
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        private static void LogBatchEdit(LeaderBatchEditArgs args)
        {
            Log.Verbose("[Leader] Batch {0} ({1})", GetBatchEditItemLog(args), GetBatchEditModeLog(args));
        }

        /// <summary>
        ///     一括編集項目のログ文字列を取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>ログ文字列</returns>
        private static string GetBatchEditItemLog(LeaderBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();
            if (args.Items[(int) LeaderBatchItemId.IdealRank])
            {
                sb.AppendFormat(" ideal rank: {0}", Config.GetText(RankNames[(int) args.IdealRank]));
            }
            if (args.Items[(int) LeaderBatchItemId.Skill])
            {
                sb.AppendFormat(" skill: {0}", args.Skill);
            }
            if (args.Items[(int) LeaderBatchItemId.MaxSkill])
            {
                sb.AppendFormat(" max skill: {0}", args.MaxSkill);
            }
            if (args.Items[(int) LeaderBatchItemId.Experience])
            {
                sb.AppendFormat(" experience: {0}", args.Experience);
            }
            if (args.Items[(int) LeaderBatchItemId.Loyalty])
            {
                sb.AppendFormat(" loyalty: {0}", args.Loyalty);
            }
            if (args.Items[(int) LeaderBatchItemId.StartYear])
            {
                sb.AppendFormat(" start year: {0}", args.StartYear);
            }
            if (args.Items[(int) LeaderBatchItemId.EndYear])
            {
                sb.AppendFormat(" end year: {0}", args.EndYear);
            }
            if (args.Items[(int) LeaderBatchItemId.RetirementYear])
            {
                sb.AppendFormat(" retirement year: {0}", args.RetirementYear);
            }
            if (args.Items[(int) LeaderBatchItemId.Rank3Year])
            {
                sb.AppendFormat(" rank3 year: {0}", args.RankYear[0]);
            }
            if (args.Items[(int) LeaderBatchItemId.Rank2Year])
            {
                sb.AppendFormat(" rank2 year: {0}", args.RankYear[1]);
            }
            if (args.Items[(int) LeaderBatchItemId.Rank1Year])
            {
                sb.AppendFormat(" rank1 year: {0}", args.RankYear[2]);
            }
            if (args.Items[(int) LeaderBatchItemId.Rank0Year])
            {
                sb.AppendFormat(" rank0 year: {0}", args.RankYear[3]);
            }
            if (args.CopyCountry != Country.None)
            {
                sb.AppendFormat(" Copy: {0} id: {1}", Countries.Strings[(int) args.CopyCountry], args.Id);
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     一括編集対象モードのログ文字列を取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>ログ文字列</returns>
        private static string GetBatchEditModeLog(LeaderBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();
            if (args.Mode == BatchMode.All)
            {
                sb.Append("ALL");
            }
            else
            {
                foreach (Country country in args.TargetCountries)
                {
                    sb.AppendFormat(" {0}", Countries.Strings[(int) country]);
                }
                if (sb.Length > 0)
                {
                    sb.Remove(0, 1);
                }
            }
            if (!args.Army || !args.Navy || !args.Airforce)
            {
                sb.Append(":");
                if (args.Army)
                {
                    sb.Append("L");
                }
                if (args.Navy)
                {
                    sb.Append("N");
                }
                if (args.Airforce)
                {
                    sb.Append("A");
                }
            }
            return sb.ToString();
        }

        #endregion

        #region ID操作

        /// <summary>
        ///     未使用の指揮官IDを取得する
        /// </summary>
        /// <param name="country">対象の国タグ</param>
        /// <returns>指揮官ID</returns>
        public static int GetNewId(Country country)
        {
            // 対象国の指揮官IDの最大値+1から検索を始める
            int id = GetMaxId(country);
            // 未使用IDが見つかるまでIDを1ずつ増やす
            return GetNewId(id);
        }

        /// <summary>
        ///     未使用の指揮官IDを取得する
        /// </summary>
        /// <param name="id">開始ID</param>
        /// <returns>指揮官ID</returns>
        public static int GetNewId(int id)
        {
            while (IdSet.Contains(id))
            {
                id++;
            }
            return id;
        }

        /// <summary>
        ///     対象国の指揮官IDの最大値を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>指揮官ID</returns>
        private static int GetMaxId(Country country)
        {
            if (country == Country.None)
            {
                return 1;
            }
            List<int> ids = Items.Where(leader => leader.Country == country).Select(leader => leader.Id).ToList();
            if (!ids.Any())
            {
                return 1;
            }
            return ids.Max() + 1;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty()
        {
            return (_dirtyFlag || _dirtyListFlag);
        }

        /// <summary>
        ///     指揮官リストファイルが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        private static bool IsDirtyList()
        {
            return _dirtyListFlag;
        }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(Country country)
        {
            return DirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirty(Country country)
        {
            DirtyFlags[(int) country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     指揮官リストファイルの編集済みフラグを設定する
        /// </summary>
        public static void SetDirtyList()
        {
            _dirtyListFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを解除する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void ResetDirty(Country country)
        {
            DirtyFlags[(int) country] = false;
        }

        /// <summary>
        ///     指揮官リストファイルの編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyList()
        {
            _dirtyListFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     指揮官一括編集のパラメータ
    /// </summary>
    public class LeaderBatchEditArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     一括編集対象モード
        /// </summary>
        public BatchMode Mode { get; set; }

        /// <summary>
        ///     対象国リスト
        /// </summary>
        public List<Country> TargetCountries { get; } = new List<Country>();

        /// <summary>
        ///     陸軍指揮官を対象とするかどうか
        /// </summary>
        public bool Army { get; set; }

        /// <summary>
        ///     海軍指揮官を対象とするかどうか
        /// </summary>
        public bool Navy { get; set; }

        /// <summary>
        ///     空軍指揮官を対象とするかどうか
        /// </summary>
        public bool Airforce { get; set; }

        /// <summary>
        ///     コピー先指定国
        /// </summary>
        public Country CopyCountry { get; set; }

        /// <summary>
        ///     開始ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     一括編集項目
        /// </summary>
        public bool[] Items { get; } = new bool[Enum.GetValues(typeof (LeaderBatchItemId)).Length];

        /// <summary>
        ///     理想階級
        /// </summary>
        public LeaderRank IdealRank { get; set; }

        /// <summary>
        ///     スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     最大スキル
        /// </summary>
        public int MaxSkill { get; set; }

        /// <summary>
        ///     経験値
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        ///     忠誠度
        /// </summary>
        public int Loyalty { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     引退年
        /// </summary>
        public int RetirementYear { get; set; }

        /// <summary>
        ///     任官年
        /// </summary>
        public int[] RankYear { get; } = new int[Leader.RankLength];

        #endregion
    }

    /// <summary>
    ///     一括編集対象モード
    /// </summary>
    public enum BatchMode
    {
        All, // 全て
        Selected, // 選択国
        Specified // 指定国
    }

    /// <summary>
    ///     一括編集項目ID
    /// </summary>
    public enum LeaderBatchItemId
    {
        IdealRank, // 理想階級
        Skill, // スキル
        MaxSkill, // 最大スキル
        Experience, // 経験値
        Loyalty, // 忠誠度
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Rank3Year, // 少将任官年
        Rank2Year, // 中将任官年
        Rank1Year, // 大将任官年
        Rank0Year // 元帥任官年
    }
}