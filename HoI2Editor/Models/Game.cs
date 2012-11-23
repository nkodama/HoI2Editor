using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ゲーム関連データ
    /// </summary>
    public static class Game
    {
        #region パス定義

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
        ///     画像フォルダ
        /// </summary>
        public const string PicturePathName = "gfx\\interface\\pics";

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
        ///     閣僚一覧ファイル名(DH)
        /// </summary>
        public const string DhMinisterListPathName = "db\\ministers.txt";

        /// <summary>
        ///     研究特性アイコンのファイル名
        /// </summary>
        public const string TechIconPathName = "gfx\\interface\\tc_icons.bmp";

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
        /// 実行ファイル名
        /// </summary>
        private static string _exeFileName;

        /// <summary>
        ///     ゲームの種類
        /// </summary>
        public static GameType Type { get; private set; }

        /// <summary>
        /// ゲームバージョン
        /// </summary>
        public static int Version { get; private set; }

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
                Config.Loaded = false;
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
                Config.Loaded = false;
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
        ///     静的コンストラクタ
        /// </summary>
        static Game()
        {
            FolderName = Environment.CurrentDirectory;
        }

        /// <summary>
        ///     MODフォルダを考慮してファイル名を取得する
        /// </summary>
        /// <param name="pathName">パス名</param>
        /// <returns>ファイル名</returns>
        public static string GetFileName(string pathName)
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
        ///     指揮官ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>指揮官ファイル名</returns>
        public static string GetLeaderFileName(CountryTag countryTag)
        {
            return Leaders.FileNameMap[countryTag];
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
        /// ゲームのバージョンを自動判別する
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
            Version = (info.ProductVersion[0] - '0') * 100 + (info.ProductVersion[2] - '0') * 10 +
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