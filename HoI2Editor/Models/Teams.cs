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
    ///     研究機関データ群
    /// </summary>
    internal static class Teams
    {
        #region 公開プロパティ

        /// <summary>
        ///     マスター研究機関リスト
        /// </summary>
        internal static List<Team> Items { get; }

        /// <summary>
        ///     国タグと研究機関ファイル名の対応付け
        /// </summary>
        internal static Dictionary<Country, string> FileNameMap { get; }

        /// <summary>
        ///     使用済みIDリスト
        /// </summary>
        internal static HashSet<int> IdSet { get; }

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
        ///     編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     研究機関リストファイルの編集済みフラグ
        /// </summary>
        private static bool _dirtyListFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Teams()
        {
            // マスター研究機関リスト
            Items = new List<Team>();

            // 国タグと研究機関ファイル名の対応付け
            FileNameMap = new Dictionary<Country, string>();

            // 使用済みIDリスト
            IdSet = new HashSet<int>();
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     研究機関ファイルの再読み込みを要求する
        /// </summary>
        internal static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     研究機関ファイル群を再読み込みする
        /// </summary>
        internal static void Reload()
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
        ///     研究機関ファイル群を読み込む
        /// </summary>
        internal static void Load()
        {
            // 読み込み済みならば戻る
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
        ///     研究機関ファイル群を遅延読み込みする
        /// </summary>
        /// <param name="handler">読み込み完了イベントハンドラ</param>
        internal static void LoadAsync(RunWorkerCompletedEventHandler handler)
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
        internal static void WaitLoading()
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
        internal static bool IsLoading()
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
        ///     研究機関ファイル群を読み込む
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
        ///     研究機関ファイル群を読み込む(HoI2/AoD/DH-MOD未使用時)
        /// </summary>
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadHoI2()
        {
            List<string> list = new List<string>();
            string folderName;
            bool error = false;

            // 保存フォルダ内の研究機関ファイルを読み込む
            if (Game.IsExportFolderActive)
            {
                folderName = Game.GetExportFileName(Game.TeamPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            // 研究機関ファイルを読み込む
                            LoadFile(fileName);

                            // 研究機関ファイル一覧に読み込んだファイル名を登録する
                            string name = Path.GetFileName(fileName);
                            if (!string.IsNullOrEmpty(name))
                            {
                                list.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Team] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorTeam, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // MODフォルダ内の研究機関ファイルを読み込む
            if (Game.IsModActive)
            {
                folderName = Game.GetModFileName(Game.TeamPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            // 研究機関ファイルを読み込む
                            LoadFile(fileName);

                            // 研究機関ファイル一覧に読み込んだファイル名を登録する
                            string name = Path.GetFileName(fileName);
                            if (!string.IsNullOrEmpty(name))
                            {
                                list.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Team] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorTeam, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // バニラフォルダ内の研究機関ファイルを読み込む
            folderName = Path.Combine(Game.FolderName, Game.TeamPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    // MODフォルダ内で読み込んだファイルは無視する
                    string name = Path.GetFileName(fileName);
                    if (string.IsNullOrEmpty(name) || list.Contains(name.ToLower()))
                    {
                        continue;
                    }

                    try
                    {
                        // 研究機関ファイルを読み込む
                        LoadFile(fileName);
                    }
                    catch (Exception)
                    {
                        error = true;
                        Log.Error("[Team] Read error: {0}", fileName);
                        if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                            Resources.EditorTeam, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
        ///     研究機関ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadDh()
        {
            // 研究機関リストファイルが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetReadFileName(Game.DhTeamListPathName);
            if (!File.Exists(listFileName))
            {
                return LoadHoI2();
            }

            // 研究機関リストファイルを読み込む
            IEnumerable<string> fileList;
            try
            {
                fileList = LoadList(listFileName);
            }
            catch (Exception)
            {
                Log.Error("[Team] Read error: {0}", listFileName);
                MessageBox.Show($"{Resources.FileReadError}: {listFileName}",
                    Resources.EditorTeam, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool error = false;
            foreach (string fileName in fileList.Select(name => Game.GetReadFileName(Game.TeamPathName, name)))
            {
                try
                {
                    // 研究機関ファイルを読み込む
                    LoadFile(fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Log.Error("[Team] Read error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorTeam, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return !error;
        }

        /// <summary>
        ///     研究機関リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadList(string fileName)
        {
            Log.Verbose("[Team] Load: {0}", Path.GetFileName(fileName));

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
        ///     研究機関ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadFile(string fileName)
        {
            Log.Verbose("[Team] Load: {0}", Path.GetFileName(fileName));

            using (CsvLexer lexer = new CsvLexer(fileName))
            {
                // 空ファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                // 国タグ読み込み
                string[] tokens = lexer.GetTokens();
                if (tokens == null || tokens.Length == 0 || string.IsNullOrEmpty(tokens[0]))
                {
                    return;
                }
                // サポート外の国タグの場合は何もしない
                if (!Countries.StringMap.ContainsKey(tokens[0].ToUpper()))
                {
                    return;
                }
                Country country = Countries.StringMap[tokens[0].ToUpper()];

                // ヘッダ行のみのファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                while (!lexer.EndOfStream)
                {
                    Team team = ParseLine(lexer, country);

                    // 空行を読み飛ばす
                    if (team == null)
                    {
                        continue;
                    }

                    Items.Add(team);
                }

                ResetDirty(country);

                if (country != Country.None && !FileNameMap.ContainsKey(country))
                {
                    FileNameMap.Add(country, lexer.FileName);
                }
            }
        }

        /// <summary>
        ///     研究機関定義行を解釈する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="country">国家タグ</param>
        /// <returns>研究機関データ</returns>
        private static Team ParseLine(CsvLexer lexer, Country country)
        {
            string[] tokens = lexer.GetTokens();

            // ID指定のない行は読み飛ばす
            if (string.IsNullOrEmpty(tokens?[0]))
            {
                return null;
            }

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != 39)
            {
                Log.Warning("[Team] Invalid token count: {0} ({1} L{2})", tokens.Length, lexer.FileName, lexer.LineNo);
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (tokens.Length < 38)
                {
                    return null;
                }
            }

            Team team = new Team { Country = country };
            int index = 0;

            // ID
            int id;
            if (!int.TryParse(tokens[index], out id))
            {
                Log.Warning("[Team] Invalid id: {0} ({1} L{2})", tokens[index], lexer.FileName, lexer.LineNo);
                return null;
            }
            team.Id = id;
            index++;

            // 名前
            team.Name = tokens[index];
            index++;

            // 画像ファイル名
            team.PictureName = tokens[index];
            index++;

            // スキル
            int skill;
            if (int.TryParse(tokens[index], out skill))
            {
                team.Skill = skill;
            }
            else
            {
                team.Skill = 1;
                Log.Warning("[Team] Invalid skill: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    lexer.FileName, lexer.LineNo);
            }
            index++;

            // 開始年
            int startYear;
            if (int.TryParse(tokens[index], out startYear))
            {
                team.StartYear = startYear;
            }
            else
            {
                team.StartYear = 1930;
                Log.Warning("[Team] Invalid start year: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    lexer.FileName, lexer.LineNo);
            }
            index++;

            // 終了年
            int endYear;
            if (int.TryParse(tokens[index], out endYear))
            {
                team.EndYear = endYear;
            }
            else
            {
                team.EndYear = 1970;
                Log.Warning("[Team] Invalid end year: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    lexer.FileName, lexer.LineNo);
            }
            index++;

            // 研究特性
            for (int i = 0; i < Team.SpecialityLength; i++, index++)
            {
                string s = tokens[index].ToLower();

                // 空文字列
                if (string.IsNullOrEmpty(s))
                {
                    team.Specialities[i] = TechSpeciality.None;
                    continue;
                }

                // 無効な研究特性文字列
                if (!Techs.SpecialityStringMap.ContainsKey(s))
                {
                    team.Specialities[i] = TechSpeciality.None;
                    Log.Warning("[Team] Invalid speciality: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id,
                        team.Name, lexer.FileName, lexer.LineNo);
                    continue;
                }

                // サポート外の研究特性
                TechSpeciality speciality = Techs.SpecialityStringMap[s];
                if (!Techs.Specialities.Contains(speciality))
                {
                    Log.Warning("[Team] Invalid speciality: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id,
                        team.Name, lexer.FileName, lexer.LineNo);
                    continue;
                }

                team.Specialities[i] = speciality;
            }

            return team;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     研究機関ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        internal static bool Save()
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

            // 研究機関リストファイルを保存する
            if ((Game.Type == GameType.DarkestHour) && IsDirtyList())
            {
                try
                {
                    SaveList();
                }
                catch (Exception)
                {
                    string fileName = Game.GetWriteFileName(Game.DhTeamListPathName);
                    Log.Error("[Team] Write error: {0}", fileName);
                    MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorTeam, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            bool error = false;
            foreach (Country country in Countries.Tags
                .Where(country => DirtyFlags[(int) country] && country != Country.None))
            {
                try
                {
                    // 研究機関ファイルを保存する
                    SaveFile(country);
                }
                catch (Exception)
                {
                    error = true;
                    string fileName = Game.GetWriteFileName(Game.MinisterPathName, Game.GetMinisterFileName(country));
                    Log.Error("[Team] Write error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileWriteError}: {fileName}",
                        Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                        return false;
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
        ///     研究機関リストファイルを保存する (DH)
        /// </summary>
        private static void SaveList()
        {
            // データベースフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.DatabasePathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Game.GetWriteFileName(Game.DhTeamListPathName);
            Log.Info("[Team] Save: {0}", Path.GetFileName(fileName));

            // 登録された研究機関ファイル名を順に書き込む
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
        ///     研究機関ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveFile(Country country)
        {
            // 研究機関フォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.TeamPathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string name = Game.GetTeamFileName(country);
            string fileName = Path.Combine(folderName, name);
            Log.Info("[Team] Save: {0}", name);

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // ヘッダ行を書き込む
                writer.WriteLine(
                    "{0};Name;Pic Name;Skill;Start Year;End Year;Speciality1;Speciality2;Speciality3;Speciality4;Speciality5;Speciality6;Speciality7;Speciality8;Speciality9;Speciality10;Speciality11;Speciality12;Speciality13;Speciality14;Speciality15;Speciality16;Speciality17;Speciality18;Speciality19;Speciality20;Speciality21;Speciality22;Speciality23;Speciality24;Speciality25;Speciality26;Speciality27;Speciality28;Speciality29;Speciality30;Speciality31;Speciality32;x",
                    Countries.Strings[(int) country]);

                // 研究機関定義行を順に書き込む
                foreach (Team team in Items.Where(team => team.Country == country))
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
                                ? Techs.SpecialityStrings[(int) team.Specialities[i]]
                                : "");
                    }
                    writer.WriteLine(";x");

                    // 編集済みフラグを解除する
                    team.ResetDirtyAll();
                }
            }

            ResetDirty(country);
        }

        #endregion

        #region 研究機関リスト操作

        /// <summary>
        ///     研究機関リストに項目を追加する
        /// </summary>
        /// <param name="team">追加対象の項目</param>
        internal static void AddItem(Team team)
        {
            Log.Info("[Team] Add team: ({0}: {1}) <{2}>", team.Id, team.Name, Countries.Strings[(int) team.Country]);

            Items.Add(team);
        }

        /// <summary>
        ///     研究機関リストに項目を挿入する
        /// </summary>
        /// <param name="team">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        internal static void InsertItem(Team team, Team position)
        {
            int index = Items.IndexOf(position) + 1;

            Log.Info("[Team] Insert team: {0} ({1}: {2}) <{3}>", index, team.Id, team.Name,
                Countries.Strings[(int) team.Country]);

            Items.Insert(index, team);
        }

        /// <summary>
        ///     研究機関リストから項目を削除する
        /// </summary>
        /// <param name="team">削除対象の項目</param>
        internal static void RemoveItem(Team team)
        {
            Log.Info("[Team] Remove team: ({0}: {1}) <{2}>", team.Id, team.Name,
                Countries.Strings[(int) team.Country]);

            Items.Remove(team);

            // 使用済みIDリストから削除する
            IdSet.Remove(team.Id);
        }

        /// <summary>
        ///     研究機関リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の項目</param>
        /// <param name="dest">移動先の項目</param>
        internal static void MoveItem(Team src, Team dest)
        {
            int srcIndex = Items.IndexOf(src);
            int destIndex = Items.IndexOf(dest);

            Log.Info("[Team] Move team: {0} -> {1} ({2}: {3}) <{4}>", srcIndex, destIndex, src.Id, src.Name,
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
        internal static void BatchEdit(TeamBatchEditArgs args)
        {
            LogBatchEdit(args);

            IEnumerable<Team> teams = GetBatchEditTeams(args);
            Country newCountry;
            switch (args.ActionMode)
            {
                case BatchActionMode.Modify:
                    // 研究機関を一括編集する
                    foreach (Team team in teams)
                    {
                        BatchEditTeam(team, args);
                    }
                    break;

                case BatchActionMode.Copy:
                    // 研究機関をコピーする
                    newCountry = args.Destination;
                    int id = args.Id;
                    foreach (Team team in teams)
                    {
                        id = GetNewId(id);
                        Team newTeam = new Team(team)
                        {
                            Country = newCountry,
                            Id = id
                        };
                        newTeam.SetDirtyAll();
                        Items.Add(newTeam);
                    }

                    // コピー先の国の編集済みフラグを設定する
                    SetDirty(newCountry);

                    // コピー先の国がファイル一覧に存在しなければ追加する
                    if (!FileNameMap.ContainsKey(newCountry))
                    {
                        FileNameMap.Add(newCountry, Game.GetTeamFileName(newCountry));
                        SetDirtyList();
                    }
                    break;

                case BatchActionMode.Move:
                    // 研究機関を移動する
                    newCountry = args.Destination;
                    foreach (Team team in teams)
                    {
                        // 移動前の国の編集済みフラグを設定する
                        SetDirty(team.Country);

                        team.Country = newCountry;
                        team.SetDirty(TeamItemId.Country);
                    }

                    // 移動先の国の編集済みフラグを設定する
                    SetDirty(newCountry);

                    // 移動先の国がファイル一覧に存在しなければ追加する
                    if (!FileNameMap.ContainsKey(newCountry))
                    {
                        FileNameMap.Add(newCountry, Game.GetTeamFileName(newCountry));
                        SetDirtyList();
                    }
                    break;
            }
        }

        /// <summary>
        ///     一括編集の個別処理
        /// </summary>
        /// <param name="team">対象研究機関</param>
        /// <param name="args">一括編集のパラメータ</param>
        private static void BatchEditTeam(Team team, TeamBatchEditArgs args)
        {
            // スキル
            if (args.Items[(int) TeamBatchItemId.Skill])
            {
                if (team.Skill != args.Skill)
                {
                    team.Skill = args.Skill;
                    team.SetDirty(TeamItemId.Skill);
                    SetDirty(team.Country);
                }
            }

            // 開始年
            if (args.Items[(int) TeamBatchItemId.StartYear])
            {
                if (team.StartYear != args.StartYear)
                {
                    team.StartYear = args.StartYear;
                    team.SetDirty(TeamItemId.StartYear);
                    SetDirty(team.Country);
                }
            }

            // 終了年
            if (args.Items[(int) TeamBatchItemId.EndYear])
            {
                if (team.EndYear != args.EndYear)
                {
                    team.EndYear = args.EndYear;
                    team.SetDirty(TeamItemId.EndYear);
                    SetDirty(team.Country);
                }
            }
        }

        /// <summary>
        ///     一括編集対象の研究機関リストを取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>一括編集対象の研究機関リスト</returns>
        private static IEnumerable<Team> GetBatchEditTeams(TeamBatchEditArgs args)
        {
            return args.CountryMode == BatchCountryMode.All
                ? Items.ToList()
                : Items.Where(team => args.TargetCountries.Contains(team.Country)).ToList();
        }

        /// <summary>
        ///     一括編集処理のログ出力
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        private static void LogBatchEdit(TeamBatchEditArgs args)
        {
            Log.Verbose($"[Team] Batch {GetBatchEditItemLog(args)} ({GetBatchEditModeLog(args)})");
        }

        /// <summary>
        ///     一括編集項目のログ文字列を取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>ログ文字列</returns>
        private static string GetBatchEditItemLog(TeamBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();
            if (args.Items[(int) TeamBatchItemId.Skill])
            {
                sb.AppendFormat($" skill: {args.Skill}");
            }
            if (args.Items[(int) TeamBatchItemId.StartYear])
            {
                sb.AppendFormat($" start year: {args.StartYear}");
            }
            if (args.Items[(int) TeamBatchItemId.EndYear])
            {
                sb.AppendFormat($" end year: {args.EndYear}");
            }
            if (args.ActionMode == BatchActionMode.Copy)
            {
                sb.Append($" Copy: {Countries.Strings[(int) args.Destination]} id: {args.Id}");
            }
            else if (args.ActionMode == BatchActionMode.Move)
            {
                sb.Append($" Move: {Countries.Strings[(int) args.Destination]} id: {args.Id}");
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
        private static string GetBatchEditModeLog(TeamBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();

            // 一括編集対象国
            if (args.CountryMode == BatchCountryMode.All)
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

            return sb.ToString();
        }

        #endregion

        #region ID操作

        /// <summary>
        ///     未使用の研究機関IDを取得する
        /// </summary>
        /// <param name="country">対象の国タグ</param>
        /// <returns>研究機関ID</returns>
        internal static int GetNewId(Country country)
        {
            // 対象国の研究機関IDの最大値+1から検索を始める
            int id = GetMaxId(country);
            // 未使用IDが見つかるまでIDを1ずつ増やす
            return GetNewId(id);
        }

        /// <summary>
        ///     未使用の研究機関IDを取得する
        /// </summary>
        /// <param name="id">開始ID</param>
        /// <returns>研究機関ID</returns>
        internal static int GetNewId(int id)
        {
            while (IdSet.Contains(id))
            {
                id++;
            }
            return id;
        }

        /// <summary>
        ///     対象国の研究機関IDの最大値を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>研究機関ID</returns>
        private static int GetMaxId(Country country)
        {
            if (country == Country.None)
            {
                return 1;
            }
            List<int> ids = Items.Where(team => team.Country == country).Select(team => team.Id).ToList();
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
        internal static bool IsDirty()
        {
            return _dirtyFlag || _dirtyListFlag;
        }

        /// <summary>
        ///     研究機関リストファイルが編集済みかどうかを取得する
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
        internal static bool IsDirty(Country country)
        {
            return DirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        internal static void SetDirty(Country country)
        {
            DirtyFlags[(int) country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     研究機関リストファイルの編集済みフラグを設定する
        /// </summary>
        internal static void SetDirtyList()
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
        ///     研究機関リストファイルの編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyList()
        {
            _dirtyListFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     研究機関一括編集のパラメータ
    /// </summary>
    internal class TeamBatchEditArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     一括編集対象国モード
        /// </summary>
        internal BatchCountryMode CountryMode { get; set; }

        /// <summary>
        ///     対象国リスト
        /// </summary>
        internal List<Country> TargetCountries { get; } = new List<Country>();

        /// <summary>
        ///     一括編集動作モード
        /// </summary>
        internal BatchActionMode ActionMode { get; set; }

        /// <summary>
        ///     コピー/移動先指定国
        /// </summary>
        internal Country Destination { get; set; }

        /// <summary>
        ///     開始ID
        /// </summary>
        internal int Id { get; set; }

        /// <summary>
        ///     一括編集項目
        /// </summary>
        internal bool[] Items { get; } = new bool[Enum.GetValues(typeof (TeamBatchItemId)).Length];

        /// <summary>
        ///     スキル
        /// </summary>
        internal int Skill { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        internal int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        internal int EndYear { get; set; }

        #endregion
    }

    /// <summary>
    ///     一括編集項目ID
    /// </summary>
    internal enum TeamBatchItemId
    {
        Skill, // スキル
        StartYear, // 開始年
        EndYear // 終了年
    }
}