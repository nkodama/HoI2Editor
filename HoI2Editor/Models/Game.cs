using System;
using System.IO;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ゲーム関連データ
    /// </summary>
    public static class Game
    {
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
        ///     Darkest HourのMODフォルダ名
        /// </summary>
        private const string DhModPathName = "Mods";

        /// <summary>
        ///     ゲームフォルダ名
        /// </summary>
        private static string _folderName;

        /// <summary>
        ///     MOD名
        /// </summary>
        private static string _modName;

        /// <summary>
        ///     ゲームの種類
        /// </summary>
        private static GameType _type = GameType.HeartsOfIron2;

        /// <summary>
        ///     MODフォルダ名
        /// </summary>
        private static string _modFolderName;

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
        public static GameType Type
        {
            get { return _type; }
            set
            {
                _type = value;

                SetModFolderName();

                // 再読み込み必要
                Misc.Loaded = false;
                Config.Loaded = false;
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
                IsGameFolderActive = (!string.IsNullOrEmpty(_folderName) && Directory.Exists(_folderName));

                SetModFolderName();

                // 再読み込み必要
                Misc.Loaded = false;
                Config.Loaded = false;
            }
        }

        /// <summary>
        ///     ゲームフォルダが有効かどうか
        /// </summary>
        public static bool IsGameFolderActive { get; private set; }

        /// <summary>
        ///     MOD名
        /// </summary>
        public static string ModName
        {
            get { return _modName; }
            set
            {
                _modName = value;

                SetModFolderName();

                // 再読み込み必要
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
        public static void DistinguishGameType()
        {
            if (string.IsNullOrEmpty(FolderName))
            {
                return;
            }
            if (File.Exists(Path.Combine(FolderName, "Hoi2.exe")) ||
                File.Exists(Path.Combine(FolderName, "DoomsdayJP.exe")))
            {
                Type = GameType.HeartsOfIron2;
            }
            else if (File.Exists(Path.Combine(FolderName, "AODGame.exe")))
            {
                Type = GameType.ArsenalOfDemocracy;
            }
            else if (File.Exists(Path.Combine(FolderName, "Darkest Hour.exe")))
            {
                Type = GameType.DarkestHour;
            }
        }

        /// <summary>
        ///     MODフォルダ名を設定する
        /// </summary>
        private static void SetModFolderName()
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
                    _modFolderName = Path.Combine(Path.Combine(FolderName, DhModPathName), ModName);
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
        HeartsOfIron2, // Hearts of Iron 2 (Doomsday Armageddon)
        ArsenalOfDemocracy, // Arsenal of Democracy
        DarkestHour // Darkest Hour
    }
}