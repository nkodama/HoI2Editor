using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ群
    /// </summary>
    public static class Teams
    {
        #region 公開プロパティ

        /// <summary>
        ///     マスター研究機関リスト
        /// </summary>
        public static List<Team> Items { get; private set; }

        /// <summary>
        ///     国タグと研究機関ファイル名の対応付け
        /// </summary>
        public static Dictionary<Country, string> FileNameMap { get; private set; }

        /// <summary>
        ///     使用済みIDリスト
        /// </summary>
        public static HashSet<int> IdSet { get; private set; }

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
        ///     編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     研究機関リストファイルの編集済みフラグ
        /// </summary>
        private static bool _dirtyListFlag;

        /// <summary>
        ///     現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        ///     現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

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
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     研究機関ファイル群を再読み込みする
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
        ///     研究機関ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

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
            var list = new List<string>();
            string folderName;
            bool error = false;

            // 保存フォルダ内の研究機関ファイルを読み込む
            if (Game.IsExportFolderActive)
            {
                folderName = Path.Combine(Game.ExportFolderName, Game.TeamPathName);
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
                            if (!String.IsNullOrEmpty(name))
                            {
                                list.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Team] Read error: {0}", fileName);
                            if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
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
                folderName = Path.Combine(Game.ModFolderName, Game.TeamPathName);
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
                            if (!String.IsNullOrEmpty(name))
                            {
                                list.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Team] Read error: {0}", fileName);
                            if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
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
                    if (String.IsNullOrEmpty(name) || list.Contains(name.ToLower()))
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
                        if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
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
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, listFileName),
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
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
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

            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

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
                if (!Countries.StringMap.ContainsKey(tokens[0].ToUpper()))
                {
                    return;
                }
                Country country = Countries.StringMap[tokens[0].ToUpper()];

                _currentLineNo++;

                while (!reader.EndOfStream)
                {
                    ParseLine(reader.ReadLine(), country);
                    _currentLineNo++;
                }

                ResetDirty(country);

                FileNameMap.Add(country, Path.GetFileName(fileName));
            }
        }

        /// <summary>
        ///     研究機関定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="country">国家タグ</param>
        private static void ParseLine(string line, Country country)
        {
            // 空行を読み飛ばす
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            string[] tokens = line.Split(CsvSeparator);

            // ID指定のない行は読み飛ばす
            if (String.IsNullOrEmpty(tokens[0]))
            {
                return;
            }

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != 39)
            {
                Log.Warning("[Team] Invalid token count: {0} ({1} L{2})", tokens.Length, _currentFileName,
                    _currentLineNo);
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (tokens.Length < 38)
                {
                    return;
                }
            }

            var team = new Team {Country = country};
            int index = 0;

            // ID
            int id;
            if (!Int32.TryParse(tokens[index], out id))
            {
                Log.Warning("[Team] Invalid id: {0} ({1} L{2})", tokens[index], _currentFileName, _currentLineNo);
                return;
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
            if (Int32.TryParse(tokens[index], out skill))
            {
                team.Skill = skill;
            }
            else
            {
                team.Skill = 1;
                Log.Warning("[Team] Invalid skill: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    _currentFileName, _currentLineNo);
            }
            index++;

            // 開始年
            int startYear;
            if (Int32.TryParse(tokens[index], out startYear))
            {
                team.StartYear = startYear;
            }
            else
            {
                team.StartYear = 1930;
                Log.Warning("[Team] Invalid start year: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    _currentFileName, _currentLineNo);
            }
            index++;

            // 終了年
            int endYear;
            if (Int32.TryParse(tokens[index], out endYear))
            {
                team.EndYear = endYear;
            }
            else
            {
                team.EndYear = 1970;
                Log.Warning("[Team] Invalid end year: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id, team.Name,
                    _currentFileName, _currentLineNo);
            }
            index++;

            // 研究特性
            for (int i = 0; i < Team.SpecialityLength; i++, index++)
            {
                string s = tokens[index].ToLower();

                // 空文字列
                if (String.IsNullOrEmpty(s))
                {
                    team.Specialities[i] = TechSpeciality.None;
                    continue;
                }

                // 無効な研究特性文字列
                if (!Techs.SpecialityStringMap.ContainsKey(s))
                {
                    team.Specialities[i] = TechSpeciality.None;
                    Log.Warning("[Team] Invalid speciality: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id,
                        team.Name, _currentFileName, _currentLineNo);
                    continue;
                }

                // サポート外の研究特性
                TechSpeciality speciality = Techs.SpecialityStringMap[s];
                if (!Techs.Specialities.Contains(speciality))
                {
                    Log.Warning("[Team] Invalid speciality: {0} [{1}: {2}] ({3} L{4})", tokens[index], team.Id,
                        team.Name, _currentFileName, _currentLineNo);
                    continue;
                }

                team.Specialities[i] = speciality;
            }

            Items.Add(team);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     研究機関ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return true;
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
                    MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
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
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
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
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 1;

                foreach (string name in FileNameMap.Select(pair => pair.Value))
                {
                    writer.WriteLine(name);
                    _currentLineNo++;
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

            string fileName = Path.Combine(folderName, Game.GetTeamFileName(country));
            Log.Info("[Team] Save: {0}", Path.GetFileName(fileName));

            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 2;

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

                    _currentLineNo++;
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
        public static void AddItem(Team team)
        {
            Log.Info("[Team] Add team: ({0}: {1}) <{2}>", team.Id, team.Name, Countries.Strings[(int) team.Country]);

            Items.Add(team);
        }

        /// <summary>
        ///     研究機関リストに項目を挿入する
        /// </summary>
        /// <param name="team">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItem(Team team, Team position)
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
        public static void RemoveItem(Team team)
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
        public static void MoveItem(Team src, Team dest)
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

        #region ID操作

        /// <summary>
        ///     未使用の研究機関IDを取得する
        /// </summary>
        /// <param name="country">対象の国タグ</param>
        /// <returns>研究機関ID</returns>
        public static int GetNewId(Country country)
        {
            // 対象国の研究機関IDの最大値+1から検索を始める
            int id = 1;
            if (country != Country.None)
            {
                List<int> ids = Items.Where(team => team.Country == country).Select(team => team.Id).ToList();
                if (ids.Any())
                {
                    id = ids.Max() + 1;
                }
            }
            // 未使用IDが見つかるまでIDを1ずつ増やす
            while (IdSet.Contains(id))
            {
                id++;
            }
            return id;
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
        ///     研究機関リストファイルの編集済みフラグを設定する
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
        ///     研究機関リストファイルの編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyList()
        {
            _dirtyListFlag = false;
        }

        #endregion
    }
}