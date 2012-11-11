using System;
using System.IO;

namespace HoI2Editor.Models
{
    /// <summary>
    /// ゲーム関連データ
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static Game()
        {
            FolderName = Environment.CurrentDirectory;
        }

        /// <summary>
        /// ゲームの種類
        /// </summary>
        public static GameType Type { get; set; }

        /// <summary>
        /// ゲームフォルダ名
        /// </summary>
        public static string FolderName { get; set; }

        /// <summary>
        /// MOD名
        /// </summary>
        public static string ModName { get; set; }

        /// <summary>
        /// 文字列フォルダ名
        /// </summary>
        public static string ConfigFolderName
        {
            get { return Path.Combine(FolderName, "config"); }
        }

        /// <summary>
        /// 指揮官フォルダ名
        /// </summary>
        public static string LeaderFolderName
        {
            get { return Path.Combine(FolderName, "db\\leaders"); }
        }

        /// <summary>
        /// 閣僚フォルダ名
        /// </summary>
        public static string MinisterFolderName
        {
            get { return Path.Combine(FolderName, "db\\ministers"); }
        }

        /// <summary>
        /// 研究機関フォルダ名
        /// </summary>
        public static string TeamFolderName
        {
            get { return Path.Combine(FolderName, "db\\tech\\teams"); }
        }

        /// <summary>
        /// 画像フォルダ名
        /// </summary>
        public static string PictureFolderName
        {
            get { return Path.Combine(FolderName, "gfx\\interface\\pics"); }
        }

        /// <summary>
        /// 指揮官ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>指揮官ファイル名</returns>
        public static string GetLeaderFileName(CountryTag countryTag)
        {
            return countryTag != CountryTag.None ? Path.Combine(LeaderFolderName, Leader.FileNameMap[countryTag]) : "";
        }

        /// <summary>
        /// 閣僚ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>閣僚ファイル名</returns>
        public static string GetMinisterFileName(CountryTag countryTag)
        {
            return countryTag != CountryTag.None
                       ? string.Format("{0}\\ministers_{1}.csv", MinisterFolderName,
                                       Country.CountryTextTable[(int) countryTag].ToLower())
                       : "";
        }

        /// <summary>
        /// 研究機関ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>研究機関ファイル名</returns>
        public static string GetTeamFileName(CountryTag countryTag)
        {
            return countryTag != CountryTag.None
                       ? string.Format("{0}\\teams_{1}.csv", TeamFolderName,
                                       Country.CountryTextTable[(int) countryTag].ToLower())
                       : "";
        }

        /// <summary>
        /// 画像ファイル名を取得する
        /// </summary>
        /// <param name="pictureName">画像名</param>
        /// <returns>画像ファイル名</returns>
        public static string GetPictureFileName(string pictureName)
        {
            return !string.IsNullOrEmpty(pictureName)
                       ? Path.Combine(PictureFolderName, Path.ChangeExtension(pictureName, ".bmp"))
                       : "";
        }

        /// <summary>
        /// ゲームフォルダ名が有効かどうかを判定する
        /// </summary>
        /// <returns>ゲームフォルダ名が有効ならtrueを返す</returns>
        public static bool IsValidFolderName()
        {
            if (!Directory.Exists(ConfigFolderName))
            {
                return false;
            }

            if (!Directory.Exists(MinisterFolderName))
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// ゲームの種類
    /// </summary>
    public enum GameType
    {
        HeartsOfIron2, // Hearts of Iron 2 (Doomsday Armageddon)
        ArsenalOfDemocracy, // Arsenal of Democracy
        DarkestHour // Darkest Hour
    };
}