using System;
using System.Diagnostics;
using System.IO;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ゲーム関連データ
    /// </summary>
    public static class Game
    {
        #region パス定義

        /// <summary>
        ///     文字列定義フォルダ
        /// </summary>
        public const string ConfigPathName = "config";

        /// <summary>
        ///     追加文字列定義フォルダ(AoD)
        /// </summary>
        public const string ConfigAdditionalPathName = "config\\Additional";

        /// <summary>
        ///     指揮官フォルダ
        /// </summary>
        public const string LeaderPathName = "db\\leaders";

        /// <summary>
        ///     閣僚フォルダ
        /// </summary>
        public const string MinisterPathName = "db\\ministers";

        /// <summary>
        ///     研究機関フォルダ
        /// </summary>
        public const string TeamPathName = "db\\tech\\teams";

        /// <summary>
        ///     技術フォルダ
        /// </summary>
        public const string TechPathName = "db\\tech";

        /// <summary>
        ///     ユニットフォルダ
        /// </summary>
        public const string UnitPathName = "db\\units";

        /// <summary>
        ///     一般画像フォルダ
        /// </summary>
        public const string PicturePathName = "gfx\\interface";

        /// <summary>
        ///     指揮官/閣僚/研究機関画像フォルダ
        /// </summary>
        public const string PersonPicturePathName = "gfx\\interface\\pics";

        /// <summary>
        ///     技術画像フォルダ
        /// </summary>
        public const string TechPicturePathName = "gfx\\interface\\tech";

        /// <summary>
        ///     ユニットモデル画像フォルダ
        /// </summary>
        public const string ModelPicturePathName = "gfx\\interface\\models";

        /// <summary>
        ///     MODフォルダ名(DH)
        /// </summary>
        public const string ModPathNameDh = "Mods";

        /// <summary>
        ///     miscのファイル名
        /// </summary>
        public const string MiscPathName = "db\\misc.txt";

        /// <summary>
        ///     閣僚特性ファイル名(AoD)
        /// </summary>
        public const string MinisterPersonalityPathNameAoD = "db\\ministers\\minister_modifiers.txt";

        /// <summary>
        ///     閣僚特性ファイル名(DH)
        /// </summary>
        public const string MinisterPersonalityPathNameDh = "db\\ministers\\minister_personalities.txt";

        /// <summary>
        ///     指揮官一覧ファイル名(DH)
        /// </summary>
        public const string DhLeaderListPathName = "db\\leaders.txt";

        /// <summary>
        ///     閣僚一覧ファイル名(DH)
        /// </summary>
        public const string DhMinisterListPathName = "db\\ministers.txt";

        /// <summary>
        ///     研究機関一覧ファイル名(DH)
        /// </summary>
        public const string DhTeamListPathName = "db\\teams.txt";

        /// <summary>
        ///     研究特性アイコンのファイル名
        /// </summary>
        public const string TechIconPathName = "gfx\\interface\\tc_icons.bmp";

        /// <summary>
        ///     技術ラベルのファイル名
        /// </summary>
        public const string TechLabelPathName = "gfx\\interface\\button_tech_normal.bmp";

        /// <summary>
        ///     イベントラベルのファイル名
        /// </summary>
        public const string SecretLabelPathName = "gfx\\interface\\button_tech_secret.bmp";

        /// <summary>
        ///     技術文字列定義のファイル名
        /// </summary>
        public const string TechTextFileName = "tech_names.csv";

        /// <summary>
        ///     ユニット文字列定義のファイル名
        /// </summary>
        public const string UnitTextFileName = "unit_names.csv";

        /// <summary>
        ///     国別モデル文字列定義のファイル名
        /// </summary>
        public const string ModelTextFileName = "models.csv";

        #endregion

        /// <summary>
        ///     ゲームフォルダ名
        /// </summary>
        private static string _folderName;

        /// <summary>
        ///     MOD名
        /// </summary>
        private static string _modName;

        /// <summary>
        ///     MODフォルダ名
        /// </summary>
        private static string _modFolderName;

        /// <summary>
        ///     実行ファイル名
        /// </summary>
        private static string _exeFileName;

        /// <summary>
        ///     ファイル読み書き時のコードページ
        /// </summary>
        private static int _codePage;

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Game()
        {
            FolderName = Environment.CurrentDirectory;
        }

        /// <summary>
        ///     ゲームの種類
        /// </summary>
        public static GameType Type { get; private set; }

        /// <summary>
        ///     ゲームバージョン
        /// </summary>
        public static int Version { get; private set; }

        /// <summary>
        ///     ファイル読み書き時のコードページ
        /// </summary>
        public static int CodePage
        {
            get { return _codePage; }
            set
            {
                _codePage = value;

                // 共通リソースの再読み込み必要
                Misc.Loaded = false;
                Config.RequireReload();

                // 編集対象ファイルの再読み込み必要
                Leaders.RequireReload();
                Ministers.RequireReload();
                Teams.RequireReload();
                Techs.RequireReload();
            }
        }

        /// <summary>
        ///     ゲームフォルダ名
        /// </summary>
        public static string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;

                // ゲームの種類を判別する
                DistinguishGameType();

                // ゲームのバージョンを判別する
                DistinguishGameVersion();

                // MODフォルダ名を更新する
                UpdateModFolderName();

                // 共通リソースの再読み込み必要
                Misc.Loaded = false;
                Config.RequireReload();

                // 編集対象ファイルの再読み込み必要
                Leaders.RequireReload();
                Ministers.RequireReload();
                Teams.RequireReload();
                Techs.RequireReload();
            }
        }

        /// <summary>
        ///     ゲームフォルダが有効かどうか
        /// </summary>
        public static bool IsGameFolderActive
        {
            get { return Type != GameType.None; }
        }

        /// <summary>
        ///     MOD名
        /// </summary>
        public static string ModName
        {
            get { return _modName; }
            set
            {
                _modName = value;

                // MODフォルダ名を更新する
                UpdateModFolderName();

                // 共通リソースの再読み込み必要
                Misc.Loaded = false;
                Config.RequireReload();

                // 編集対象ファイルの再読み込み必要
                Leaders.RequireReload();
                Ministers.RequireReload();
                Teams.RequireReload();
                Techs.RequireReload();
            }
        }

        /// <summary>
        ///     MODが有効かどうか
        /// </summary>
        public static bool IsModActive { get; private set; }

        /// <summary>
        ///     MODフォルダ名
        /// </summary>
        public static string ModFolderName
        {
            get { return _modFolderName; }
        }

        /// <summary>
        ///     MODフォルダを考慮して読み込み用のファイル名を取得する
        /// </summary>
        /// <param name="pathName">パス名</param>
        /// <returns>ファイル名</returns>
        public static string GetReadFileName(string pathName)
        {
            if (IsModActive)
            {
                string fileName = Path.Combine(ModFolderName, pathName);
                if (File.Exists(fileName))
                {
                    return fileName;
                }
            }
            return Path.Combine(FolderName, pathName);
        }

        /// <summary>
        ///     MODフォルダを考慮して書き込み用のファイル名を取得する
        /// </summary>
        /// <param name="pathName">パス名</param>
        /// <returns>ファイル名</returns>
        public static string GetWriteFileName(string pathName)
        {
            return Path.Combine(IsModActive ? ModFolderName : FolderName, pathName);
        }

        /// <summary>
        ///     指揮官ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>指揮官ファイル名</returns>
        public static string GetLeaderFileName(CountryTag countryTag)
        {
            return Leaders.FileNameMap.ContainsKey(countryTag)
                       ? Leaders.FileNameMap[countryTag]
                       : string.Format("leaders{0}.csv", Country.CountryTextTable[(int) countryTag].ToUpper());
        }

        /// <summary>
        ///     閣僚ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>閣僚ファイル名</returns>
        public static string GetMinisterFileName(CountryTag countryTag)
        {
            return string.Format("ministers_{0}.csv", Country.CountryTextTable[(int) countryTag].ToLower());
        }

        /// <summary>
        ///     研究機関ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>研究機関ファイル名</returns>
        public static string GetTeamFileName(CountryTag countryTag)
        {
            return string.Format("teams_{0}.csv", Country.CountryTextTable[(int) countryTag].ToLower());
        }

        /// <summary>
        ///     ゲームの種類を自動判別する
        /// </summary>
        private static void DistinguishGameType()
        {
            if (string.IsNullOrEmpty(FolderName))
            {
                Type = GameType.None;
                return;
            }

            // HoI2
            string fileName = Path.Combine(FolderName, "Hoi2.exe");
            if (File.Exists(fileName))
            {
                Type = GameType.HeartsOfIron2;
                _exeFileName = fileName;
                return;
            }
            fileName = Path.Combine(FolderName, "DoomsdayJP.exe");
            if (File.Exists(fileName))
            {
                Type = GameType.HeartsOfIron2;
                _exeFileName = fileName;
                return;
            }

            // AoD
            fileName = Path.Combine(FolderName, "AODGame.exe");
            if (File.Exists(fileName))
            {
                Type = GameType.ArsenalOfDemocracy;
                _exeFileName = fileName;
                return;
            }

            // DH
            fileName = Path.Combine(FolderName, "Darkest Hour.exe");
            if (File.Exists(fileName))
            {
                Type = GameType.DarkestHour;
                _exeFileName = fileName;
                return;
            }

            Type = GameType.None;
        }

        /// <summary>
        ///     ゲームのバージョンを自動判別する
        /// </summary>
        private static void DistinguishGameVersion()
        {
            // DH以外では必要がないので判別しない
            if (Type != GameType.DarkestHour)
            {
                Version = 100;
                return;
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(_exeFileName);

            if (info.ProductVersion.Length < 4)
            {
                Version = 100;
                return;
            }
            Version = (info.ProductVersion[0] - '0')*100 + (info.ProductVersion[2] - '0')*10 +
                      (info.ProductVersion[3] - '0');
        }

        /// <summary>
        ///     MODフォルダ名を更新する
        /// </summary>
        private static void UpdateModFolderName()
        {
            if (!IsGameFolderActive)
            {
                IsModActive = false;
                _modFolderName = "";
                return;
            }
            if (string.IsNullOrEmpty(_modName))
            {
                IsModActive = false;
                _modFolderName = FolderName;
                return;
            }
            IsModActive = true;
            switch (Type)
            {
                case GameType.DarkestHour:
                    _modFolderName = Path.Combine(Path.Combine(FolderName, ModPathNameDh), ModName);
                    break;

                default:
                    _modFolderName = Path.Combine(FolderName, ModName);
                    break;
            }
        }
    }

    /// <summary>
    ///     ゲームの種類
    /// </summary>
    public enum GameType
    {
        None,
        HeartsOfIron2, // Hearts of Iron 2 (Doomsday Armageddon)
        ArsenalOfDemocracy, // Arsenal of Democracy
        DarkestHour // Darkest Hour
    }
}