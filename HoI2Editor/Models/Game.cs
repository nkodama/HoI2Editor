using System;
using System.Collections.Generic;
using System.IO;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ゲーム関連データ
    /// </summary>
    public static class Game
    {
        #region 公開プロパティ

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

                // ファイルの再読み込みを要求する
                RequireReload();
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

                // 言語モードを判別する
                DistinguishLanguageMode();

                // MODフォルダ名を更新する
                UpdateModFolderName();

                // ファイルの再読み込みを要求する
                RequireReload();
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

                // ファイルの再読み込みを要求する
                RequireReload();
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

        #endregion

        #region 内部フィールド

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

        #endregion

        #region 公開定数

        /// <summary>
        ///     文字列定義フォルダ
        /// </summary>
        public const string ConfigPathName = "config";

        /// <summary>
        ///     追加文字列定義フォルダ(AoD)
        /// </summary>
        public const string ConfigAdditionalPathName = "config\\Additional";

        /// <summary>
        ///     データベースフォルダ
        /// </summary>
        public const string DatabasePathName = "db";

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
        ///     師団ユニットフォルダ
        /// </summary>
        public const string DivisionPathName = "db\\units\\divisions";

        /// <summary>
        ///     旅団ユニットフォルダ
        /// </summary>
        public const string BrigadePathName = "db\\units\\brigades";

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
        ///     マップフォルダ名
        /// </summary>
        public const string MapPathName = "map";

        /// <summary>
        ///     マップフォルダ内の画像フォルダ名
        /// </summary>
        public const string MapImagePathName = "gfx";

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
        ///     プロヴィンス定義ファイル名
        /// </summary>
        public const string ProvinceFileName = "province.csv";

        /// <summary>
        ///     師団ユニットクラス定義ファイル名(DH)
        /// </summary>
        public const string DhDivisionTypePathName = "db\\units\\division_types.txt";

        /// <summary>
        ///     旅団ユニットクラス定義ファイル名(DH)
        /// </summary>
        public const string DhBrigadeTypePathName = "db\\units\\brigade_types.txt";

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

        /// <summary>
        ///     プロヴィンス名定義ファイル名
        /// </summary>
        public const string ProvinceTextFileName = "province_names.csv";

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Game()
        {
            FolderName = Environment.CurrentDirectory;
        }

        /// <summary>
        ///     ファイルの再読み込みを要求する
        /// </summary>
        private static void RequireReload()
        {
            Misc.RequireReload();
            Config.RequireReload();
            Leaders.RequireReload();
            Ministers.RequireReload();
            Teams.RequireReload();
            Techs.RequireReload();
            Units.RequireReload();
            Provinces.RequireReload();
        }

        #endregion

        #region パス操作

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
        ///     MODフォルダを考慮して読み込み用のファイル名を取得する
        /// </summary>
        /// <param name="folderName">フォルダ名</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ファイル名</returns>
        public static string GetReadFileName(string folderName, string fileName)
        {
            string pathName = Path.Combine(folderName, fileName);
            return GetReadFileName(pathName);
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
        ///     MODフォルダを考慮して書き込み用のファイル名を取得する
        /// </summary>
        /// <param name="folderName">フォルダ名</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ファイル名</returns>
        public static string GetWriteFileName(string folderName, string fileName)
        {
            string pathName = Path.Combine(folderName, fileName);
            return GetWriteFileName(pathName);
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
                       : string.Format("leaders{0}.csv", Country.Strings[(int) countryTag].ToUpper());
        }

        /// <summary>
        ///     閣僚ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>閣僚ファイル名</returns>
        public static string GetMinisterFileName(CountryTag countryTag)
        {
            return string.Format("ministers_{0}.csv", Country.Strings[(int) countryTag].ToLower());
        }

        /// <summary>
        ///     研究機関ファイル名を取得する
        /// </summary>
        /// <param name="countryTag">国タグ</param>
        /// <returns>研究機関ファイル名</returns>
        public static string GetTeamFileName(CountryTag countryTag)
        {
            return string.Format("teams_{0}.csv", Country.Strings[(int) countryTag].ToLower());
        }

        /// <summary>
        ///     マップフォルダ名を取得する
        /// </summary>
        /// <returns>マップフォルダ名</returns>
        public static string GetMapFolderName()
        {
            // バニラのマップ
            if (Type != GameType.DarkestHour || Misc.Map.MapNo == 0)
            {
                return DatabasePathName;
            }

            // DHのマップ拡張
            return Path.Combine(MapPathName, string.Format("Map_{0}", Misc.Map.MapNo));
        }

        /// <summary>
        ///     プロヴィンス名フォルダ名を取得する
        /// </summary>
        /// <returns>プロヴィンス名フォルダ名</returns>
        public static string GetProvinceNameFolderName()
        {
            // バニラのマップ
            if (Type != GameType.DarkestHour || Misc.Map.MapNo == 0)
            {
                return ConfigPathName;
            }

            // DHのマップ拡張
            return Path.Combine(MapPathName, string.Format("Map_{0}", Misc.Map.MapNo));
        }

        /// <summary>
        ///     プロヴィンス画像フォルダ名を取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>プロヴィンス画像フォルダ名</returns>
        public static string GetProvinceImageFileName(int id)
        {
            string folderName;
            if (Type != GameType.DarkestHour || Misc.Map.MapNo == 0)
            {
                // バニラのプロヴィンス画像フォルダ
                folderName = PicturePathName;
            }
            else
            {
                // DHのマップ拡張
                folderName = Path.Combine(MapPathName, string.Format("Map_{0}", Misc.Map.MapNo));
                folderName = Path.Combine(folderName, MapImagePathName);
            }

            return Path.Combine(folderName, string.Format("ill_prov_{0}.bmp", id));
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

        #endregion

        #region ゲームの種類/バージョン

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
            if (Type == GameType.None)
            {
                Version = 100;
                return;
            }

            // 実行ファイルのバイナリ列を読み込む
            var info = new FileInfo(_exeFileName);
            long size = info.Length;
            var data = new byte[size];

            FileStream s = info.OpenRead();
            s.Read(data, 0, (int) size);
            s.Close();

            // バージョン文字列を検索する
            byte[] pattern;
            List<uint> l;
            uint offset;
            switch (Type)
            {
                case GameType.HeartsOfIron2:
                    // Doomsday Armageddon v X.X
                    pattern = new byte[]
                        {
                            0x44, 0x6F, 0x6F, 0x6D, 0x73, 0x64, 0x61, 0x79,
                            0x20, 0x41, 0x72, 0x6D, 0x61, 0x67, 0x65, 0x64,
                            0x64, 0x6F, 0x6E, 0x20, 0x76, 0x20
                        };
                    l = BinaryScan(data, pattern, 0, (uint) size);
                    if (l.Count == 0)
                    {
                        // Iron Cross Armageddon X.XX
                        pattern = new byte[]
                            {
                                0x49, 0x72, 0x6F, 0x6E, 0x20, 0x43, 0x72, 0x6F,
                                0x73, 0x73, 0x20, 0x41, 0x72, 0x6D, 0x61, 0x67,
                                0x65, 0x64, 0x64, 0x6F, 0x6E, 0x20
                            };
                        l = BinaryScan(data, pattern, 0, (uint) size);
                        if (l.Count == 0)
                        {
                            Version = 100;
                            return;
                        }
                        offset = l[0] + (uint) pattern.Length;
                        Version = (data[offset] - '0')*100 + (data[offset + 2] - '0')*10 +
                                  (data[offset + 3] - '0');
                    }
                    else
                    {
                        offset = l[0] + (uint) pattern.Length;
                        Version = (data[offset] - '0')*100 + (data[offset + 2] - '0')*10;
                    }
                    break;

                case GameType.ArsenalOfDemocracy:
                    // Arsenal of Democracy X.XX
                    pattern = new byte[]
                        {
                            0x41, 0x72, 0x73, 0x65, 0x6E, 0x61, 0x6C, 0x20,
                            0x6F, 0x66, 0x20, 0x44, 0x65, 0x6D, 0x6F, 0x63,
                            0x72, 0x61, 0x63, 0x79, 0x20
                        };
                    l = BinaryScan(data, pattern, 0, (uint) size);
                    if (l.Count == 0)
                    {
                        // Arsenal Of Democracy v X.XX
                        pattern = new byte[]
                            {
                                0x41, 0x72, 0x73, 0x65, 0x6E, 0x61, 0x6C, 0x20,
                                0x4F, 0x66, 0x20, 0x44, 0x65, 0x6D, 0x6F, 0x63,
                                0x72, 0x61, 0x63, 0x79, 0x20, 0x76, 0x20
                            };
                        l = BinaryScan(data, pattern, 0, (uint) size);
                        if (l.Count == 0)
                        {
                            Version = 100;
                            return;
                        }
                    }
                    offset = l[0] + (uint) pattern.Length;
                    Version = (data[offset] - '0')*100 + (data[offset + 2] - '0')*10 + (data[offset + 3] - '0');
                    break;

                case GameType.DarkestHour:
                    // Darkest Hour v X.XX
                    pattern = new byte[]
                        {
                            0x44, 0x61, 0x72, 0x6B, 0x65, 0x73, 0x74, 0x20,
                            0x48, 0x6F, 0x75, 0x72, 0x20, 0x76, 0x20
                        };
                    l = BinaryScan(data, pattern, 0, (uint) size);
                    if (l.Count == 0)
                    {
                        Version = 100;
                        return;
                    }
                    offset = l[0] + (uint) pattern.Length;
                    Version = (data[offset] - '0')*100 + (data[offset + 2] - '0')*10 + (data[offset + 3] - '0');
                    break;

                default:
                    // Doomsday Armageddon v X.X
                    pattern = new byte[]
                        {
                            0x44, 0x6F, 0x6F, 0x6D, 0x73, 0x64, 0x61, 0x79,
                            0x20, 0x41, 0x72, 0x6D, 0x61, 0x67, 0x65, 0x64,
                            0x64, 0x6F, 0x6E, 0x20, 0x76, 0x20
                        };
                    l = BinaryScan(data, pattern, 0, (uint) size);
                    if (l.Count == 0)
                    {
                        Version = 100;
                        return;
                    }
                    offset = l[0] + (uint) pattern.Length;
                    Version = (data[offset] - '0')*100 + (data[offset + 2] - '0')*10;
                    break;
            }
        }

        /// <summary>
        ///     バイナリ列を探索する
        /// </summary>
        /// <param name="target">探索対象のデータ</param>
        /// <param name="pattern">探索するバイトパターン</param>
        /// <param name="start">開始位置</param>
        /// <param name="size">探索するバイトサイズ</param>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static List<uint> BinaryScan(byte[] target, byte[] pattern, uint start, uint size)
        {
            var result = new List<uint>();
            for (uint offset = start; offset <= start + size - pattern.Length; offset++)
            {
                if (IsBinaryMatch(target, pattern, offset))
                {
                    result.Add(offset);
                }
            }
            return result;
        }

        /// <summary>
        ///     バイナリ列が一致しているかを判定する
        /// </summary>
        /// <param name="target">探索対象のデータ</param>
        /// <param name="pattern">探索するバイトパターン</param>
        /// <param name="offset">判定する位置</param>
        /// <returns>バイナリ列が一致していればtrueを返す</returns>
        private static bool IsBinaryMatch(byte[] target, byte[] pattern, uint offset)
        {
            int i;
            for (i = 0; i < pattern.Length; i++)
            {
                if (target[offset + i] != pattern[i])
                    return false;
            }
            return true;
        }

        #endregion

        #region 言語

        /// <summary>
        ///     言語モードを自動判別する
        /// </summary>
        private static void DistinguishLanguageMode()
        {
            // ゲームフォルダ名が設定されていなければ判別しない
            if (string.IsNullOrEmpty(FolderName))
            {
                return;
            }

            // ゲームフォルダが存在しなければ判別しない
            if (!Directory.Exists(FolderName))
            {
                return;
            }

            // ゲームの種類が不明ならば判別しない
            if (Type == GameType.None)
            {
                return;
            }

            // _inmm.dllが存在すれば英語版日本語化
            if (File.Exists(Path.Combine(FolderName, "_inmm.dll")))
            {
                Config.LangMode = LanguageMode.PatchedJapanese;
                return;
            }

            // DoomsdayJP.exe(HoI2)/cyberfront.url(AoD)が存在すれば日本語版
            if (File.Exists(Path.Combine(FolderName, "DoomsdayJP.exe")) ||
                File.Exists(Path.Combine(FolderName, "cyberfront.url")))
            {
                Config.LangMode = LanguageMode.Japanese;
                return;
            }

            // それ以外は英語版
            Config.LangMode = LanguageMode.English;
        }

        #endregion
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