using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    /// 新規ユニット名を保持するクラス
    /// </summary>
    public static class UnitNames
    {
        #region 公開プロパティ

        /// <summary>
        /// 利用可能なユニット名種類
        /// </summary>
        public static UnitNameType[] Types { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        /// ユニット名
        /// </summary>
        private static readonly Dictionary<CountryTag, Dictionary<UnitNameType, List<string>>> Items =
            new Dictionary<CountryTag, Dictionary<UnitNameType, List<string>>>();

        /// <summary>
        /// ユニット名種類文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, UnitNameType> TypeStringMap = new Dictionary<string, UnitNameType>();

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
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof(CountryTag)).Length];

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
        /// ユニット種類名
        /// </summary>
        public static readonly string[] TypeNames =
            {
                "NAME_HQ",
                "NAME_INFANTRY",
                "NAME_GARRISON",
                "NAME_CAVALRY",
                "NAME_MOTORIZED",
                "NAME_MECHANIZED",
                "NAME_LIGHT_ARMOR",
                "NAME_ARMOR",
                "NAME_PARATROOPER",
                "NAME_MARINE",
                "NAME_BERGSJAEGER",
                "NAME_MILITIA",
                "NAME_MULTI_ROLE",
                "NAME_INTERCEPTOR",
                "NAME_ROCKET_INTERCEPTOR",
                "NAME_ESCORT",
                "NAME_STRATEGIC_BOMBER",
                "NAME_TACTICAL_BOMBER",
                "NAME_CAS",
                "NAME_NAVAL_BOMBER",
                "NAME_TRANSPORT_PLANE",
                "NAME_FLYING_BOMB",
                "NAME_FLYING_ROCKET",
                "NAME_BATTLESHIP",
                "NAME_BATTLECRUISER",
                "NAME_CARRIER",
                "NAME_ESCORT_CARRIER",
                "NAME_LIGHT_CARRIER",
                "NAME_HEAVY_CRUISER",
                "NAME_LIGHT_CRUISER",
                "NAME_DESTROYER",
                "NAME_SUBMARINE",
                "NAME_NUCLEAR_SUBMARINE",
                "NAME_TRANSPORT"
            };

        /// <summary>
        /// ユニット種類文字列
        /// </summary>
        public static readonly string[] TypeStrings =
            {
                "HQ",
                "Inf",
                "Gar",
                "Cav",
                "Mot",
                "Mec",
                "L ARM",
                "Arm",
                "Par",
                "Mar",
                "Mtn",
                "Mil",
                "Fig",
                "Int F",
                "32",
                "Esc F",
                "Str",
                "Tac",
                "CAS",
                "Nav",
                "Trp",
                "V1",
                "V2",
                "BB",
                "BC",
                "CV",
                "27",
                "31",
                "CA",
                "CL",
                "DD",
                "SS",
                "NS",
                "TP"
            };

        #endregion

        #region 内部定数

        /// <summary>
        /// 利用可能なユニット名種類 (DDA/AoD/DH1.02)
        /// </summary>
        private static readonly UnitNameType[] TypesHoI2 =
            {
                UnitNameType.Hq,
                UnitNameType.Infantry,
                UnitNameType.Garrison,
                UnitNameType.Cavalry,
                UnitNameType.Motorized,
                UnitNameType.Mechanized,
                UnitNameType.LightArmor,
                UnitNameType.Armor,
                UnitNameType.Paratrooper,
                UnitNameType.Marine,
                UnitNameType.Bergsjaeger,
                UnitNameType.Militia,
                UnitNameType.Fighter,
                UnitNameType.Interceptor,
                UnitNameType.EscortFighter,
                UnitNameType.StrategicBomber,
                UnitNameType.TacticalBomber,
                UnitNameType.Cas,
                UnitNameType.NavalBomber,
                UnitNameType.TransportPlane,
                UnitNameType.FlyingBomb,
                UnitNameType.FlyingRocket,
                UnitNameType.Battleship,
                UnitNameType.BattleCruiser,
                UnitNameType.Carrier,
                UnitNameType.EscortCarrier,
                UnitNameType.HeavyCruiser,
                UnitNameType.LightCruiser,
                UnitNameType.Destroyer,
                UnitNameType.Submarine,
                UnitNameType.NuclearSubmarine,
                UnitNameType.Transport,
            };

        /// <summary>
        /// 利用可能なユニット名種類 (DH1.03)
        /// </summary>
        private static readonly UnitNameType[] TypesDh103 =
            {
                UnitNameType.Hq,
                UnitNameType.Infantry,
                UnitNameType.Garrison,
                UnitNameType.Cavalry,
                UnitNameType.Motorized,
                UnitNameType.Mechanized,
                UnitNameType.LightArmor,
                UnitNameType.Armor,
                UnitNameType.Paratrooper,
                UnitNameType.Marine,
                UnitNameType.Bergsjaeger,
                UnitNameType.Militia,
                UnitNameType.Fighter,
                UnitNameType.Interceptor,
                UnitNameType.RocketInterceptor,
                UnitNameType.EscortFighter,
                UnitNameType.StrategicBomber,
                UnitNameType.TacticalBomber,
                UnitNameType.Cas,
                UnitNameType.NavalBomber,
                UnitNameType.TransportPlane,
                UnitNameType.FlyingBomb,
                UnitNameType.FlyingRocket,
                UnitNameType.Battleship,
                UnitNameType.BattleCruiser,
                UnitNameType.Carrier,
                UnitNameType.EscortCarrier,
                UnitNameType.LightCarrier,
                UnitNameType.HeavyCruiser,
                UnitNameType.LightCruiser,
                UnitNameType.Destroyer,
                UnitNameType.Submarine,
                UnitNameType.NuclearSubmarine,
                UnitNameType.Transport,
            };

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = { ';' };

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static UnitNames()
        {
            // ユニット名種類
            foreach (UnitNameType type in Enum.GetValues(typeof (UnitNameType)))
            {
                TypeStringMap.Add(TypeStrings[(int) type].ToUpper(), type);
            }
        }

        /// <summary>
        /// ユニット名データを初期化する
        /// </summary>
        public static void Init()
        {
            // 利用可能なユニット名種類
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                Types = TypesDh103;
            }
            else
            {
                Types = TypesHoI2;
            }
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     ユニット名定義ファイルの再読み込みを要求する
        /// </summary>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     ユニット名定義ファイルを読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            Items.Clear();

            // ユニット名定義ファイルが存在しなければ戻る
            string fileName = Game.GetReadFileName(Game.UnitNamesPathName);
            if (!File.Exists(fileName))
            {
                return;
            }

            // ユニット名定義ファイルを読み込む
            LoadFile(fileName);

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     ユニット名定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        private static void LoadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

                while (!reader.EndOfStream)
                {
                    ParseLine(reader.ReadLine());
                    _currentLineNo++;
                }
            }
        }

        /// <summary>
        /// ユニット名定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        private static void ParseLine(string line)
        {
            // 空行を読み飛ばす
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string[] tokens = line.Split(CsvSeparator);

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != 3)
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                // 余分な項目がある場合は解析を続ける
                if (tokens.Length < 3)
                {
                    return;
                }
            }

            // 国タグ
            string countryName = tokens[0].ToUpper();
            if (!Country.StringMap.ContainsKey(countryName))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidCountryTag, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                return;
            }
            CountryTag country = Country.StringMap[countryName];

            // 兵科
            string typeName = tokens[1].ToUpper();
            if (!TypeStringMap.ContainsKey(typeName))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidBranch, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                return;
            }
            UnitNameType type = TypeStringMap[typeName];
            if (!Types.Contains(type))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidBranch, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                return;
            }

            // ユニット名
            string name = tokens[2];
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            // ユニット名を追加する
            AddName(country, type, name);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     ユニット名定義ファイルを保存する
        /// </summary>
        public static void Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return;
            }

            string fileName = Game.GetWriteFileName(Game.UnitNamesPathName);
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 3;


                foreach (CountryTag country in Items.Select(pair => pair.Key).Where(country => ExistsCountry(country) && Items[country].Count > 0))
                {
                    CountryTag c = country;
                    foreach (UnitNameType type in Items[country].Select(pair => pair.Key).Where(type => ExistsType(c, type) && Items[c][type].Count > 0))
                    {
                        try
                        {
                            foreach (string name in Items[country][type])
                            {
                                writer.WriteLine(
                                    "{0};{1};{2}", Country.Strings[(int) country], TypeStrings[(int) type], name);
                            }
                        }
                        catch (Exception)
                        {
                            Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                        }
                    }
                }
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;
        }

        #endregion

        #region ユニット名操作

        /// <summary>
        /// ユニット名リストを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名の種類</param>
        /// <returns>ユニット名リスト</returns>
        public static List<string> GetNames(CountryTag country, UnitNameType type)
        {
            // 未登録の場合は空のリストを返す
            if (!ExistsType(country, type))
            {
                return new List<string>();
            }

            return Items[country][type];
        }

        /// <summary>
        /// ユニット名を追加する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名の種類</param>
        /// <param name="name">ユニット名</param>
        public static void AddName(CountryTag country, UnitNameType type, string name)
        {
            // 未登録の場合は項目を作成する
            if (!ExistsCountry(country))
            {
                Items.Add(country, new Dictionary<UnitNameType, List<string>>());
            }
            if (!ExistsType(country, type))
            {
                Items[country].Add(type, new List<string>());
            }

            // ユニット名を追加する
            Items[country][type].Add(name);
        }

        /// <summary>
        /// ユニット名リストを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名の種類</param>
        /// <param name="names">ユニット名リスト</param>
        public static void SetNames(CountryTag country, UnitNameType type, List<string> names)
        {
            // 未登録の場合は項目を作成する
            if (!ExistsCountry(country))
            {
                Items.Add(country, new Dictionary<UnitNameType, List<string>>());
            }

            // ユニット名リストに変更がなければ戻る
            if (names.SequenceEqual(Items[country][type]))
            {
                return;
            }

            // ユニット名リストを設定する
            Items[country][type] = names;

            // 編集済みフラグを設定する
            SetDirty(country);
        }

        /// <summary>
        /// ユニット名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名種類</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void Replace(string s, string t, CountryTag country, UnitNameType type, bool regex)
        {
            var names = new List<string>();
            foreach (string name in Items[country][type])
            {
                string r = regex ? Regex.Replace(name, s, t) : name.Replace(s, t);
                if (!r.Equals(name))
                {
                    SetDirty(country);
                }
                names.Add(r);
            }
            SetNames(country, type, names);
        }

        /// <summary>
        /// 全てのユニット名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAll(string s, string t, bool regex)
        {
            List<KeyValuePair<CountryTag, UnitNameType>> pairs
                = (from country in Items.Select(pair => pair.Key)
                   from type in Items[country].Select(pair => pair.Key)
                   select new KeyValuePair<CountryTag, UnitNameType>(country, type)).ToList();
            foreach (KeyValuePair<CountryTag, UnitNameType> pair in pairs)
            {
                Replace(s, t, pair.Key, pair.Value, regex);
            }
        }

        /// <summary>
        /// 全ての国のユニット名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="type">ユニット名種類</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAllCountries(string s, string t, UnitNameType type, bool regex)
        {
            List<CountryTag> countries = Items.Select(pair => pair.Key).Where(country => Items[country].ContainsKey(type)).ToList();
            foreach (CountryTag country in countries)
            {
                Replace(s, t, country, type, regex);
            }
        }

        /// <summary>
        /// 全てのユニット名種類のユニット名を置換する
        /// </summary>
        /// <param name="s">置換元文字列</param>
        /// <param name="t">置換先文字列</param>
        /// <param name="country">国タグ</param>
        /// <param name="regex">正規表現を使用するか</param>
        public static void ReplaceAllTypes(string s, string t, CountryTag country, bool regex)
        {
            var types = new List<UnitNameType>();
            if (Items.ContainsKey(country))
            {
                types.AddRange(Items[country].Select(pair => pair.Key));
            }
            foreach (UnitNameType type in types)
            {
                Replace(s, t, country, type, regex);
            }
        }

        /// <summary>
        /// 指定した国の項目が存在するかを返す
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>項目が存在すればtrueを返す</returns>
        private static bool ExistsCountry(CountryTag country)
        {
            return Items.ContainsKey(country);
        }

        /// <summary>
        /// 指定したユニット種類の項目が存在するかを返す
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名の種類</param>
        private static bool ExistsType(CountryTag country, UnitNameType type)
        {
            if (!ExistsCountry(country))
            {
                return false;
            }

            return Items[country].ContainsKey(type);
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
            return DirtyFlags[(int)country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirty(CountryTag country)
        {
            DirtyFlags[(int)country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを解除する
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void ResetDirty(CountryTag country)
        {
            DirtyFlags[(int)country] = false;
        }

        #endregion
    }

    /// <summary>
    /// ユニット名種類
    /// </summary>
    public enum UnitNameType
    {
        Hq, // 司令部
        Infantry, // 歩兵
        Garrison, // 守備師団
        Cavalry, // 騎兵
        Motorized, // 自動車化歩兵
        Mechanized, // 機械化歩兵
        LightArmor, // 軽戦車
        Armor, // 戦車
        Paratrooper, // 空挺兵
        Marine, // 海兵
        Bergsjaeger, // 山岳兵
        Militia, // 民兵
        Fighter, // 戦闘機
        Interceptor, // 迎撃機
        RocketInterceptor, // ロケット迎撃機
        EscortFighter, // 護衛戦闘機
        StrategicBomber, // 戦略爆撃機
        TacticalBomber, // 戦術爆撃機
        Cas, // 近接航空支援機
        NavalBomber, // 海軍爆撃機
        TransportPlane, // 輸送機
        FlyingBomb, // 飛行爆弾
        FlyingRocket, // 戦略ロケット
        Battleship, // 戦艦
        BattleCruiser, // 巡洋戦艦
        Carrier, // 空母
        EscortCarrier, // 護衛空母
        LightCarrier, // 軽空母
        HeavyCruiser, // 重巡洋艦
        LightCruiser, // 軽巡洋艦
        Destroyer, // 駆逐艦
        Submarine, // 潜水艦
        NuclearSubmarine, // 原子力潜水艦
        Transport, // 輸送艦
    }
}
