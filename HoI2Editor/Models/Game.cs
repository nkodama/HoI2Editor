using System;
using System.IO;

namespace HoI2Editor
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
        /// 閣僚フォルダ名
        /// </summary>
        public static string MinisterFolderName
        {
            get { return Path.Combine(FolderName, "db\\ministers"); }
        }

        /// <summary>
        /// 画像フォルダ名
        /// </summary>
        public static string PictureFolderName
        {
            get { return Path.Combine(FolderName, "gfx\\interface\\pics"); }
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