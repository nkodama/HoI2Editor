using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;
using HoI2Editor.Writers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     シナリオデータ群
    /// </summary>
    public static class Scenarios
    {
        #region 公開プロパティ

        /// <summary>
        ///     シナリオデータ
        /// </summary>
        public static Scenario Data { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     シナリオファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     国タグと主要国設定の対応付け
        /// </summary>
        private static readonly Dictionary<Country, MajorCountrySettings> MajorTable =
            new Dictionary<Country, MajorCountrySettings>();

        /// <summary>
        ///     国タグと国家設定の対応付け
        /// </summary>
        private static readonly Dictionary<Country, CountrySettings> CountryTable =
            new Dictionary<Country, CountrySettings>();

        /// <summary>
        ///     国タグと国家関係の対応付け
        /// </summary>
        private static readonly Dictionary<Country, Dictionary<Country, Relation>> RelationTable =
            new Dictionary<Country, Dictionary<Country, Relation>>();

        /// <summary>
        ///     国タグと不可侵条約の対応付け
        /// </summary>
        private static readonly Dictionary<Country, Dictionary<Country, Treaty>> NonAggressionTable =
            new Dictionary<Country, Dictionary<Country, Treaty>>();

        /// <summary>
        ///     国タグと講和条約の対応付け
        /// </summary>
        private static readonly Dictionary<Country, Dictionary<Country, Treaty>> PeaceTable =
            new Dictionary<Country, Dictionary<Country, Treaty>>();

        /// <summary>
        ///     国タグと諜報設定の対応付け
        /// </summary>
        private static readonly Dictionary<Country, Dictionary<Country, SpySettings>> SpyTable =
            new Dictionary<Country, Dictionary<Country, SpySettings>>();

        /// <summary>
        ///     プロヴィンスIDとプロヴィンス設定の対応付け
        /// </summary>
        private static readonly Dictionary<int, ProvinceSettings> ProvinceTable =
            new Dictionary<int, ProvinceSettings>();

        /// <summary>
        ///     プロヴィンスIDとプロヴィンス保有国の対応付け
        /// </summary>
        private static readonly Dictionary<int, Country> OwnedCountries = new Dictionary<int, Country>();

        /// <summary>
        ///     プロヴィンスIDとプロヴィンス支配国の対応付け
        /// </summary>
        private static readonly Dictionary<int, Country> ControlledCountries = new Dictionary<int, Country>();

        /// <summary>
        ///     使用済みのtypeとidの組
        /// </summary>
        private static Dictionary<int, HashSet<int>> _usedTypeIds;

        #endregion

        #region 公開定数

        /// <summary>
        ///     同盟の標準type
        /// </summary>
        public const int DefaultAllianceType = 15000;

        /// <summary>
        ///     戦争の標準type
        /// </summary>
        public const int DefaultWarType = 9430;

        /// <summary>
        ///     外交協定の標準type
        /// </summary>
        public const int DefaultTreatyType = 16384;

        /// <summary>
        ///     指揮官の標準type
        /// </summary>
        public const int DefaultLeaderType = 6;

        /// <summary>
        ///     閣僚の標準type
        /// </summary>
        public const int DefaultMinisterType = 9;

        /// <summary>
        ///     研究機関の標準type
        /// </summary>
        public const int DefaultTeamType = 10;

        /// <summary>
        ///     月名文字列
        /// </summary>
        public static readonly string[] MonthStrings =
        {
            "january",
            "february",
            "march",
            "april",
            "may",
            "june",
            "july",
            "august",
            "september",
            "october",
            "november",
            "december"
        };

        /// <summary>
        ///     外交協定文字列
        /// </summary>
        public static readonly string[] TreatyStrings =
        {
            "non_aggression",
            "peace",
            "trade"
        };

        /// <summary>
        ///     天候文字列
        /// </summary>
        public static readonly string[] WeatherStrings =
        {
            "",
            "clear",
            "frozen",
            "raining",
            "snowing",
            "storm",
            "blizzard",
            "muddy"
        };

        /// <summary>
        ///     政体文字列
        /// </summary>
        public static readonly string[] GovernmentStrings =
        {
            "",
            "nazi",
            "fascist",
            "paternal_autocrat",
            "social_conservative",
            "market_liberal",
            "social_liberal",
            "social_democrat",
            "left_wing_radical",
            "leninist",
            "stalinist"
        };

        /// <summary>
        ///     建物文字列
        /// </summary>
        public static readonly string[] BuildingStrings =
        {
            "",
            "ic",
            "infrastructure",
            "coastal_fort",
            "land_fort",
            "anti_air",
            "air_base",
            "naval_base",
            "radar_station",
            "nuclear_reactor",
            "rocket_test",
            "synthetic_oil",
            "synthetic_rares",
            "nuclear_power"
        };

        /// <summary>
        ///     任務文字列
        /// </summary>
        public static readonly string[] MissionStrings =
        {
            "",
            "attack",
            "rebase",
            "strat_redeploy",
            "support_attack",
            "support_defense",
            "reserves",
            "anti_partisan_duty",
            "artillery_bombardment",
            "planned_defense",
            "air_superiority",
            "ground_attack",
            "runway_cratering",
            "installation_strike",
            "interdiction",
            "naval_strike",
            "port_strike",
            "logistical_strike",
            "strategic_bombardment",
            "air_supply",
            "airborne_assault",
            "air_scramble",
            "convoy_raiding",
            "asw",
            "naval_interdiction",
            "shore_bombardment",
            "amphibious_assault",
            "sea_transport",
            "naval_combat_patrol",
            "sneak_move",
            "naval_scramble",
            "convoy_air_raiding",
            "naval_port_strike",
            "naval_airbase_strike",
            "nuke",
            "retreat"
        };

        /// <summary>
        ///     輸送船団文字列
        /// </summary>
        public static readonly string[] ConvoyStrings =
        {
            "",
            "transports",
            "escorts"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     初期化処理
        /// </summary>
        public static void Init()
        {
            // 主要国設定を初期化する
            InitMajorCountries();

            // 国家情報を初期化する
            InitCountries();

            // 不可侵条約を初期化する
            InitNonAggressions();

            // 講和条約を初期化する
            InitPeaces();

            // 使用済みのtypeとidの組を初期化する
            InitTypeIds();

            // プロヴィンス情報を初期化する
            InitProvinces();
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     ファイルを読み込み済みかを取得する
        /// </summary>
        /// <returns>ファイルを読み込みならばtrueを返す</returns>
        public static bool IsLoaded()
        {
            return _loaded;
        }

        /// <summary>
        ///     ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     シナリオファイル群を再読み込みする
        /// </summary>
        public static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            LoadFiles();
        }

        /// <summary>
        ///     シナリオファイル群を読み込む
        /// </summary>
        public static void Load(string fileName)
        {
            // 読み込み済みのファイル名と一致すれば何もしない
            if (_loaded && fileName.Equals(_fileName))
            {
                return;
            }

            _fileName = fileName;

            LoadFiles();
        }

        /// <summary>
        ///     シナリオファイルを読み込む
        /// </summary>
        private static void LoadFiles()
        {
            // シナリオファイルを解釈する
            Log.Verbose("[Scenario] Load: {0}", Path.GetFileName(_fileName));
            try
            {
                Data = new Scenario();
                ScenarioParser.Parse(_fileName, Data);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Read error: {0}", _fileName);
                MessageBox.Show($"{Resources.FileReadError}: {_fileName}",
                    Resources.EditorScenario, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     シナリオファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            // 読み込み前ならば何もしない
            if (!_loaded)
            {
                return true;
            }

            // プロヴィンス設定をID順にソートする
            SortProvinceSettings();

            Scenario scenario = Data;
            if (scenario.IsDirtyProvinces())
            {
                // bases.inc
                if (scenario.IsBaseProvinceSettings && !SaveBasesIncFile())
                {
                    return false;
                }

                // bases_DOD.inc
                if (scenario.IsBaseDodProvinceSettings && !SaveBasesDodIncFile())
                {
                    return false;
                }

                // depots.inc
                if (scenario.IsDepotsProvinceSettings && !SaveDepotsIncFile())
                {
                    return false;
                }
            }

            // vp.inc
            if (scenario.IsVpProvinceSettings && scenario.IsDirtyVpInc() && !SaveVpIncFile())
            {
                return false;
            }

            // 国別inc
            if (scenario.IsDirtyProvinces())
            {
                if (scenario.Countries.Any(settings => !SaveCountryFiles(settings)))
                {
                    return false;
                }
            }
            else
            {
                if (scenario.Countries.Where(settings => settings.IsDirty())
                    .Any(settings => !SaveCountryFiles(settings)))
                {
                    return false;
                }
            }

            // シナリオファイル
            if (scenario.IsDirty() && !SaveScenarioFile())
            {
                return false;
            }

            // 編集済みフラグを解除する
            ResetDirtyAll();

            return true;
        }

        /// <summary>
        ///     シナリオファイルを保存する
        /// </summary>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SaveScenarioFile()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                return false;
            }

            // シナリオフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.ScenarioPathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, Path.GetFileName(_fileName));
            try
            {
                // シナリオファイルを保存する
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.Write(Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     基地定義ファイルを保存する
        /// </summary>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SaveBasesIncFile()
        {
            // シナリオインクルードフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Path.Combine(Game.ScenarioPathName, Data.IncludeFolder));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, Game.BasesIncFileName);
            try
            {
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.WriteBasesInc(Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     基地定義ファイルを保存する (DH Full 33年シナリオ)
        /// </summary>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SaveBasesDodIncFile()
        {
            // シナリオインクルードフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Path.Combine(Game.ScenarioPathName, Data.IncludeFolder));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, Game.BasesIncDodFileName);
            try
            {
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.WriteBasesDodInc(Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     資源備蓄定義ファイルを保存する
        /// </summary>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SaveDepotsIncFile()
        {
            // シナリオインクルードフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Path.Combine(Game.ScenarioPathName, Data.IncludeFolder));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, Game.DepotsIncFileName);
            try
            {
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.WriteDepotsInc(Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     VP定義ファイルを保存する
        /// </summary>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SaveVpIncFile()
        {
            // シナリオインクルードフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Path.Combine(Game.ScenarioPathName, Data.IncludeFolder));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, Game.VpIncFileName);
            try
            {
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.WriteVpInc(Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     国別incファイルを保存する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private static bool SaveCountryFiles(CountrySettings settings)
        {
            // シナリオインクルードフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Path.Combine(Game.ScenarioPathName, Data.IncludeFolder));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Path.Combine(folderName, string.IsNullOrEmpty(settings.FileName)
                ? $"{Countries.Strings[(int) settings.Country].ToLower()}.inc"
                : settings.FileName);
            try
            {
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
                ScenarioWriter.WriteCountrySettings(settings, Data, fileName);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Write error: {0}", fileName);
                MessageBox.Show($"{Resources.FileWriteError}: {fileName}", Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion

        #region 国家

        /// <summary>
        ///     主要国設定を初期化する
        /// </summary>
        private static void InitMajorCountries()
        {
            MajorTable.Clear();
            foreach (MajorCountrySettings major in Data.Header.MajorCountries)
            {
                MajorTable[major.Country] = major;
            }
        }

        /// <summary>
        ///     国家情報を初期化する
        /// </summary>
        private static void InitCountries()
        {
            CountryTable.Clear();
            RelationTable.Clear();
            SpyTable.Clear();
            foreach (CountrySettings settings in Data.Countries)
            {
                Country country = settings.Country;

                // 国タグと国家設定の対応付け
                CountryTable[country] = settings;

                // 国タグと国家関係の対応付け
                if (!RelationTable.ContainsKey(country))
                {
                    RelationTable.Add(country, new Dictionary<Country, Relation>());
                }
                foreach (Relation relation in settings.Relations)
                {
                    RelationTable[country][relation.Country] = relation;
                }

                // 国タグと諜報設定の対応付け
                if (!SpyTable.ContainsKey(country))
                {
                    SpyTable.Add(country, new Dictionary<Country, SpySettings>());
                }
                foreach (SpySettings spy in settings.Intelligence)
                {
                    SpyTable[country][spy.Country] = spy;
                }
            }
        }

        /// <summary>
        ///     不可侵条約を初期化する
        /// </summary>
        private static void InitNonAggressions()
        {
            NonAggressionTable.Clear();
            foreach (Treaty nonAggression in Data.GlobalData.NonAggressions)
            {
                if (!NonAggressionTable.ContainsKey(nonAggression.Country1))
                {
                    NonAggressionTable.Add(nonAggression.Country1, new Dictionary<Country, Treaty>());
                }
                NonAggressionTable[nonAggression.Country1][nonAggression.Country2] = nonAggression;
                if (!NonAggressionTable.ContainsKey(nonAggression.Country2))
                {
                    NonAggressionTable.Add(nonAggression.Country2, new Dictionary<Country, Treaty>());
                }
                NonAggressionTable[nonAggression.Country2][nonAggression.Country1] = nonAggression;
            }
        }

        /// <summary>
        ///     講和条約を初期化する
        /// </summary>
        private static void InitPeaces()
        {
            PeaceTable.Clear();
            foreach (Treaty peace in Data.GlobalData.Peaces)
            {
                if (!PeaceTable.ContainsKey(peace.Country1))
                {
                    PeaceTable.Add(peace.Country1, new Dictionary<Country, Treaty>());
                }
                PeaceTable[peace.Country1][peace.Country2] = peace;
                if (!PeaceTable.ContainsKey(peace.Country2))
                {
                    PeaceTable.Add(peace.Country2, new Dictionary<Country, Treaty>());
                }
                PeaceTable[peace.Country2][peace.Country1] = peace;
            }
        }

        /// <summary>
        ///     主要国設定を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>主要国設定</returns>
        public static MajorCountrySettings GetMajorCountrySettings(Country country)
        {
            return MajorTable.ContainsKey(country) ? MajorTable[country] : null;
        }

        /// <summary>
        ///     主要国設定を設定する
        /// </summary>
        /// <param name="major">主要国設定</param>
        public static void SetMajorCountrySettings(MajorCountrySettings major)
        {
            MajorTable[major.Country] = major;
        }

        /// <summary>
        ///     主要国設定を削除する
        /// </summary>
        /// <param name="major">主要国設定</param>
        public static void RemoveMajorCountrySettings(MajorCountrySettings major)
        {
            if (MajorTable.ContainsKey(major.Country))
            {
                MajorTable.Remove(major.Country);
            }
        }

        /// <summary>
        ///     国家設定を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>国家設定</returns>
        public static CountrySettings GetCountrySettings(Country country)
        {
            return CountryTable.ContainsKey(country) ? CountryTable[country] : null;
        }

        /// <summary>
        ///     国家設定を作成する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>国家設定</returns>
        public static CountrySettings CreateCountrySettings(Country country)
        {
            CountrySettings settings = new CountrySettings
            {
                Country = country,
                FileName = country != Country.CON ? $"{Countries.Strings[(int) country].ToLower()}.inc" : "congo.inc"
            };

            // 国家設定テーブルに登録する
            CountryTable[country] = settings;

            // 国家設定リストに追加する
            Data.Countries.Add(settings);

            // インクルードファイルを追加する
            Data.IncludeFiles.Add($"scenarios\\{Data.IncludeFolder}\\{settings.FileName}");

            // シナリオファイルの編集済みフラグを設定する
            Data.SetDirty();

            return settings;
        }

        /// <summary>
        ///     国家関係を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>国家関係</returns>
        public static Relation GetCountryRelation(Country country1, Country country2)
        {
            return RelationTable.ContainsKey(country1) && RelationTable[country1].ContainsKey(country2)
                ? RelationTable[country1][country2]
                : null;
        }

        /// <summary>
        ///     国家関係を設定する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <param name="relation">国家関係</param>
        public static void SetCountryRelation(Country country, Relation relation)
        {
            if (!RelationTable.ContainsKey(country))
            {
                RelationTable.Add(country, new Dictionary<Country, Relation>());
            }
            RelationTable[country][relation.Country] = relation;
        }

        /// <summary>
        ///     不可侵条約を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>不可侵条約</returns>
        public static Treaty GetNonAggression(Country country1, Country country2)
        {
            return NonAggressionTable.ContainsKey(country1) && NonAggressionTable[country1].ContainsKey(country2)
                ? NonAggressionTable[country1][country2]
                : null;
        }

        /// <summary>
        ///     不可侵条約を設定する
        /// </summary>
        /// <param name="treaty">不可侵条約</param>
        public static void SetNonAggression(Treaty treaty)
        {
            if (!NonAggressionTable.ContainsKey(treaty.Country1))
            {
                NonAggressionTable.Add(treaty.Country1, new Dictionary<Country, Treaty>());
            }
            NonAggressionTable[treaty.Country1][treaty.Country2] = treaty;

            if (!NonAggressionTable.ContainsKey(treaty.Country2))
            {
                NonAggressionTable.Add(treaty.Country2, new Dictionary<Country, Treaty>());
            }
            NonAggressionTable[treaty.Country2][treaty.Country1] = treaty;
        }

        /// <summary>
        ///     不可侵条約を削除する
        /// </summary>
        /// <param name="treaty">不可侵条約</param>
        public static void RemoveNonAggression(Treaty treaty)
        {
            if (NonAggressionTable.ContainsKey(treaty.Country1) &&
                NonAggressionTable[treaty.Country1].ContainsKey(treaty.Country2))
            {
                NonAggressionTable[treaty.Country1].Remove(treaty.Country2);
            }

            if (NonAggressionTable.ContainsKey(treaty.Country2) &&
                NonAggressionTable[treaty.Country2].ContainsKey(treaty.Country1))
            {
                NonAggressionTable[treaty.Country2].Remove(treaty.Country1);
            }
        }

        /// <summary>
        ///     講和条約を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>講和条約</returns>
        public static Treaty GetPeace(Country country1, Country country2)
        {
            return PeaceTable.ContainsKey(country1) && PeaceTable[country1].ContainsKey(country2)
                ? PeaceTable[country1][country2]
                : null;
        }

        /// <summary>
        ///     講和条約を設定する
        /// </summary>
        /// <param name="peace">講和条約</param>
        public static void SetPeace(Treaty peace)
        {
            if (!PeaceTable.ContainsKey(peace.Country1))
            {
                PeaceTable.Add(peace.Country1, new Dictionary<Country, Treaty>());
            }
            PeaceTable[peace.Country1][peace.Country2] = peace;

            if (!PeaceTable.ContainsKey(peace.Country2))
            {
                PeaceTable.Add(peace.Country2, new Dictionary<Country, Treaty>());
            }
            PeaceTable[peace.Country2][peace.Country1] = peace;
        }

        /// <summary>
        ///     講和条約を削除する
        /// </summary>
        /// <param name="peace">講和条約</param>
        public static void RemovePeace(Treaty peace)
        {
            if (PeaceTable.ContainsKey(peace.Country1) &&
                PeaceTable[peace.Country1].ContainsKey(peace.Country2))
            {
                PeaceTable[peace.Country1].Remove(peace.Country2);
            }

            if (PeaceTable.ContainsKey(peace.Country2) &&
                PeaceTable[peace.Country2].ContainsKey(peace.Country1))
            {
                PeaceTable[peace.Country2].Remove(peace.Country1);
            }
        }

        /// <summary>
        ///     諜報設定を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>諜報設定</returns>
        public static SpySettings GetCountryIntelligence(Country country1, Country country2)
        {
            return SpyTable.ContainsKey(country1) && SpyTable[country1].ContainsKey(country2)
                ? SpyTable[country1][country2]
                : null;
        }

        /// <summary>
        ///     諜報設定を設定する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <param name="spy">諜報設定</param>
        public static void SetCountryIntelligence(Country country, SpySettings spy)
        {
            if (!SpyTable.ContainsKey(country))
            {
                SpyTable.Add(country, new Dictionary<Country, SpySettings>());
            }
            SpyTable[country][spy.Country] = spy;
        }

        /// <summary>
        ///     国タグと国名の文字列を取得する
        /// </summary>
        /// <param name="country">国家</param>
        /// <returns>国タグと国名の文字列</returns>
        public static string GetCountryTagName(Country country)
        {
            return $"{Countries.Strings[(int) country]} {GetCountryName(country)}";
        }

        /// <summary>
        ///     国名文字列を取得する
        /// </summary>
        /// <param name="country">国家</param>
        /// <returns>国名文字列</returns>
        public static string GetCountryName(Country country)
        {
            // 主要国設定の国名
            MajorCountrySettings major = GetMajorCountrySettings(country);
            if (!string.IsNullOrEmpty(major?.Name))
            {
                return Config.ExistsKey(major.Name) ? Config.GetText(major.Name) : "";
            }

            // 国家設定の国名
            CountrySettings settings = GetCountrySettings(country);
            if (!string.IsNullOrEmpty(settings?.Name))
            {
                return Config.ExistsKey(settings.Name) ? Config.GetText(settings.Name) : settings.Name;
            }

            // 標準の国名
            return Countries.GetName(country);
        }

        #endregion

        #region プロヴィンス

        /// <summary>
        ///     プロヴィンス名を取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>プロヴィンス名</returns>
        public static string GetProvinceName(int id)
        {
            Province province = Provinces.Items[id];
            ProvinceSettings settings = GetProvinceSettings(id);
            if (!string.IsNullOrEmpty(settings?.Name))
            {
                return Config.ExistsKey(settings.Name) ? Config.GetText(settings.Name) : "";
            }
            return province.GetName();
        }

        /// <summary>
        ///     プロヴィンス名を取得する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>プロヴィンス名</returns>
        public static string GetProvinceName(Province province, ProvinceSettings settings)
        {
            if (!string.IsNullOrEmpty(settings?.Name))
            {
                return Config.ExistsKey(settings.Name) ? Config.GetText(settings.Name) : "";
            }
            return province.GetName();
        }

        /// <summary>
        ///     プロヴィンス名を設定する
        /// </summary>
        /// <param name="province">プロヴィンス</param>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="s">プロヴィンス名</param>
        public static void SetProvinceName(Province province, ProvinceSettings settings, string s)
        {
            if (!string.IsNullOrEmpty(settings?.Name))
            {
                Config.SetText(settings.Name, s, Game.ScenarioTextFileName);
            }
            else
            {
                province.SetName(s);
            }
        }

        /// <summary>
        ///     プロヴィンス情報を初期化する
        /// </summary>
        private static void InitProvinces()
        {
            // プロヴィンスIDとプロヴィンス設定の対応付け
            ProvinceTable.Clear();
            foreach (ProvinceSettings settings in Data.Provinces)
            {
                ProvinceTable[settings.Id] = settings;
            }

            // プロヴィンスIDとプロヴィンス保有国の対応付け
            OwnedCountries.Clear();
            foreach (CountrySettings settings in Data.Countries)
            {
                foreach (int id in settings.OwnedProvinces)
                {
                    if (OwnedCountries.ContainsKey(id))
                    {
                        Log.Warning("[Scenario] duplicated owned province: {0} <{1}> <{2}>", id,
                            Countries.Strings[(int) settings.Country], Countries.Strings[(int) OwnedCountries[id]]);
                    }
                    OwnedCountries[id] = settings.Country;
                }
            }

            // プロヴィンスIDとプロヴィンス支配国の対応付け
            ControlledCountries.Clear();
            foreach (CountrySettings settings in Data.Countries)
            {
                foreach (int id in settings.ControlledProvinces)
                {
                    if (ControlledCountries.ContainsKey(id))
                    {
                        Log.Warning("[Scenario] duplicated controlled province: {0} <{1}> <{2}>", id,
                            Countries.Strings[(int) settings.Country], Countries.Strings[(int) ControlledCountries[id]]);
                    }
                    ControlledCountries[id] = settings.Country;
                }
            }
        }

        /// <summary>
        ///     プロヴィンス設定を取得する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <returns>プロヴィンス設定</returns>
        public static ProvinceSettings GetProvinceSettings(int id)
        {
            if (!ProvinceTable.ContainsKey(id))
            {
                return null;
            }
            return ProvinceTable[id];
        }

        /// <summary>
        ///     プロヴィンス設定を追加する
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        public static void AddProvinceSettings(ProvinceSettings settings)
        {
            ProvinceSettings prev = Data.Provinces.Find(ps => ps.Id == settings.Id);
            if (prev == null)
            {
                Data.Provinces.Add(settings);
                ProvinceTable[settings.Id] = settings;
            }
            else
            {
                MergeProvinceSettings(prev, settings);
            }
        }

        /// <summary>
        ///     プロヴィンス設定をマージする
        /// </summary>
        /// <param name="prev">プロヴィンス設定1</param>
        /// <param name="settings">プロヴィンス設定2</param>
        private static void MergeProvinceSettings(ProvinceSettings prev, ProvinceSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Name))
            {
                prev.Name = settings.Name;
            }
            if (settings.Ic != null)
            {
                prev.Ic = settings.Ic;
            }
            if (settings.Infrastructure != null)
            {
                prev.Infrastructure = settings.Infrastructure;
            }
            if (settings.LandFort != null)
            {
                prev.LandFort = settings.LandFort;
            }
            if (settings.CoastalFort != null)
            {
                prev.CoastalFort = settings.CoastalFort;
            }
            if (settings.AntiAir != null)
            {
                prev.AntiAir = settings.AntiAir;
            }
            if (settings.AirBase != null)
            {
                prev.AirBase = settings.AirBase;
            }
            if (settings.NavalBase != null)
            {
                prev.NavalBase = settings.NavalBase;
            }
            if (settings.RadarStation != null)
            {
                prev.RadarStation = settings.RadarStation;
            }
            if (settings.NuclearReactor != null)
            {
                prev.NuclearReactor = settings.NuclearReactor;
            }
            if (settings.RocketTest != null)
            {
                prev.RocketTest = settings.RocketTest;
            }
            if (settings.SyntheticOil != null)
            {
                prev.SyntheticOil = settings.SyntheticOil;
            }
            if (settings.SyntheticRares != null)
            {
                prev.SyntheticRares = settings.SyntheticRares;
            }
            if (settings.NuclearPower != null)
            {
                prev.NuclearPower = settings.NuclearPower;
            }
            if (!DoubleHelper.IsZero(settings.Manpower))
            {
                prev.Manpower = settings.Manpower;
            }
            if (!DoubleHelper.IsZero(settings.MaxManpower))
            {
                prev.MaxManpower = settings.MaxManpower;
            }
            if (!DoubleHelper.IsZero(settings.EnergyPool))
            {
                prev.EnergyPool = settings.EnergyPool;
            }
            if (!DoubleHelper.IsZero(settings.Energy))
            {
                prev.Energy = settings.Energy;
            }
            if (!DoubleHelper.IsZero(settings.MaxEnergy))
            {
                prev.MaxEnergy = settings.MaxEnergy;
            }
            if (!DoubleHelper.IsZero(settings.MetalPool))
            {
                prev.MetalPool = settings.MetalPool;
            }
            if (!DoubleHelper.IsZero(settings.Metal))
            {
                prev.Metal = settings.Metal;
            }
            if (!DoubleHelper.IsZero(settings.MaxMetal))
            {
                prev.MaxMetal = settings.MaxMetal;
            }
            if (!DoubleHelper.IsZero(settings.RareMaterialsPool))
            {
                prev.RareMaterialsPool = settings.RareMaterialsPool;
            }
            if (!DoubleHelper.IsZero(settings.RareMaterials))
            {
                prev.RareMaterials = settings.RareMaterials;
            }
            if (!DoubleHelper.IsZero(settings.MaxRareMaterials))
            {
                prev.MaxRareMaterials = settings.MaxRareMaterials;
            }
            if (!DoubleHelper.IsZero(settings.OilPool))
            {
                prev.OilPool = settings.OilPool;
            }
            if (!DoubleHelper.IsZero(settings.Oil))
            {
                prev.Oil = settings.Oil;
            }
            if (!DoubleHelper.IsZero(settings.MaxOil))
            {
                prev.MaxOil = settings.MaxOil;
            }
            if (!DoubleHelper.IsZero(settings.SupplyPool))
            {
                prev.SupplyPool = settings.SupplyPool;
            }
            if (settings.Vp != 0)
            {
                prev.Vp = settings.Vp;
            }
            if (!DoubleHelper.IsZero(settings.RevoltRisk))
            {
                prev.RevoltRisk = settings.RevoltRisk;
            }
        }

        /// <summary>
        ///     プロヴィンス設定をID順にソートする
        /// </summary>
        private static void SortProvinceSettings()
        {
            Data.Provinces.Sort((x, y) => x.Id - y.Id);
        }

        /// <summary>
        ///     中核プロヴィンスを追加する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void AddCoreProvince(int id, CountrySettings settings)
        {
            // 中核プロヴィンスを追加する
            if (!settings.NationalProvinces.Contains(id))
            {
                settings.NationalProvinces.Add(id);
            }
        }

        /// <summary>
        ///     中核プロヴィンスを削除する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void RemoveCoreProvince(int id, CountrySettings settings)
        {
            // 中核プロヴィンスを削除する
            if (settings.NationalProvinces.Contains(id))
            {
                settings.NationalProvinces.Remove(id);
            }
        }

        /// <summary>
        ///     保有プロヴィンスを追加する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void AddOwnedProvince(int id, CountrySettings settings)
        {
            // 保有プロヴィンスを追加する
            if (!settings.OwnedProvinces.Contains(id))
            {
                settings.OwnedProvinces.Add(id);
            }

            // 元の保有国の保有プロヴィンスを削除する
            if (OwnedCountries.ContainsKey(id))
            {
                GetCountrySettings(OwnedCountries[id]).OwnedProvinces.Remove(id);
            }

            // プロヴィンスIDとプロヴィンス保有国の対応付けを更新する
            OwnedCountries[id] = settings.Country;
        }

        /// <summary>
        ///     保有プロヴィンスを削除する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void RemoveOwnedProvince(int id, CountrySettings settings)
        {
            // 保有プロヴィンスを削除する
            if (settings.OwnedProvinces.Contains(id))
            {
                settings.OwnedProvinces.Remove(id);
            }

            // プロヴィンスIDとプロヴィンス保有国の対応付けを削除する
            OwnedCountries.Remove(id);
        }

        /// <summary>
        ///     支配プロヴィンスを追加する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void AddControlledProvince(int id, CountrySettings settings)
        {
            // 支配プロヴィンスを追加する
            if (!settings.ControlledProvinces.Contains(id))
            {
                settings.ControlledProvinces.Add(id);
            }

            // 元の支配国の支配プロヴィンスを削除する
            if (ControlledCountries.ContainsKey(id))
            {
                GetCountrySettings(ControlledCountries[id]).ControlledProvinces.Remove(id);
            }

            // プロヴィンスIDとプロヴィンス支配国の対応付けを更新する
            ControlledCountries[id] = settings.Country;
        }

        /// <summary>
        ///     支配プロヴィンスを削除する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void RemoveControlledProvince(int id, CountrySettings settings)
        {
            // 支配プロヴィンスを削除する
            if (settings.ControlledProvinces.Contains(id))
            {
                settings.ControlledProvinces.Remove(id);
            }

            // プロヴィンスIDとプロヴィンス支配国の対応付けを削除する
            ControlledCountries.Remove(id);
        }

        /// <summary>
        ///     領有権主張プロヴィンスを追加する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void AddClaimedProvince(int id, CountrySettings settings)
        {
            // 領有権主張プロヴィンスを追加する
            if (!settings.ClaimedProvinces.Contains(id))
            {
                settings.ClaimedProvinces.Add(id);
            }
        }

        /// <summary>
        ///     領有権主張プロヴィンスを削除する
        /// </summary>
        /// <param name="id">プロヴィンスID</param>
        /// <param name="settings">国家設定</param>
        public static void RemoveClaimedProvince(int id, CountrySettings settings)
        {
            // 領有権主張プロヴィンスを削除する
            if (settings.ClaimedProvinces.Contains(id))
            {
                settings.ClaimedProvinces.Remove(id);
            }
        }

        #endregion

        #region typeとidの組

        /// <summary>
        ///     typeとidの組を初期化する
        /// </summary>
        private static void InitTypeIds()
        {
            _usedTypeIds = new Dictionary<int, HashSet<int>>
            {
                { DefaultAllianceType, new HashSet<int>() },
                { DefaultWarType, new HashSet<int>() },
                { DefaultTreatyType, new HashSet<int>() },
                { DefaultLeaderType, new HashSet<int>() },
                { DefaultMinisterType, new HashSet<int>() },
                { DefaultTeamType, new HashSet<int>() }
            };

            Scenario scenario = Data;
            ScenarioGlobalData data = scenario.GlobalData;

            if (data.Axis != null)
            {
                AddTypeId(data.Axis.Id);
            }
            if (data.Allies != null)
            {
                AddTypeId(data.Allies.Id);
            }
            if (data.Comintern != null)
            {
                AddTypeId(data.Comintern.Id);
            }
            foreach (Alliance alliance in data.Alliances)
            {
                AddTypeId(alliance.Id);
            }
            foreach (War war in data.Wars)
            {
                if (war.Attackers != null)
                {
                    AddTypeId(war.Attackers.Id);
                }
                if (war.Defenders != null)
                {
                    AddTypeId(war.Defenders.Id);
                }
            }

            foreach (Treaty nonAggression in data.NonAggressions)
            {
                AddTypeId(nonAggression.Id);
            }
            foreach (Treaty peace in data.Peaces)
            {
                AddTypeId(peace.Id);
            }
            foreach (Treaty trade in data.Trades)
            {
                AddTypeId(trade.Id);
            }

            if (data.Weather != null)
            {
                foreach (WeatherPattern pattern in data.Weather.Patterns)
                {
                    AddTypeId(pattern.Id);
                }
            }

            foreach (CountrySettings settings in scenario.Countries)
            {
                AddTypeId(settings.HeadOfState);
                AddTypeId(settings.HeadOfGovernment);
                AddTypeId(settings.ForeignMinister);
                AddTypeId(settings.ArmamentMinister);
                AddTypeId(settings.MinisterOfSecurity);
                AddTypeId(settings.MinisterOfIntelligence);
                AddTypeId(settings.ChiefOfStaff);
                AddTypeId(settings.ChiefOfArmy);
                AddTypeId(settings.ChiefOfNavy);
                AddTypeId(settings.ChiefOfAir);

                foreach (Unit unit in settings.LandUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (Division division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                }
                foreach (Unit unit in settings.NavalUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (Division division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                    foreach (Unit landUnit in unit.LandUnits)
                    {
                        AddTypeId(landUnit.Id);
                        foreach (Division landDivision in landUnit.Divisions)
                        {
                            AddTypeId(landDivision.Id);
                        }
                    }
                }
                foreach (Unit unit in settings.AirUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (Division division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                    foreach (Unit landUnit in unit.LandUnits)
                    {
                        AddTypeId(landUnit.Id);
                        foreach (Division landDivision in landUnit.Divisions)
                        {
                            AddTypeId(landDivision.Id);
                        }
                    }
                }
                foreach (DivisionDevelopment division in settings.DivisionDevelopments)
                {
                    AddTypeId(division.Id);
                }

                foreach (BuildingDevelopment building in settings.BuildingDevelopments)
                {
                    AddTypeId(building.Id);
                }

                foreach (Convoy convoy in settings.Convoys)
                {
                    AddTypeId(convoy.Id);
                    AddTypeId(convoy.TradeId);
                }
                foreach (ConvoyDevelopment convoy in settings.ConvoyDevelopments)
                {
                    AddTypeId(convoy.Id);
                }
            }
        }

        /// <summary>
        ///     typeとidの組が存在するかを返す
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="id">id</param>
        /// <returns>typeとidの組が存在すればtrueを返す</returns>
        public static bool ExistsTypeId(int type, int id)
        {
            return _usedTypeIds.ContainsKey(type) && _usedTypeIds[type].Contains(id);
        }

        /// <summary>
        ///     typeとidの組を登録する
        /// </summary>
        /// <param name="id">typeとidの組</param>
        /// <returns>登録に成功すればtrueを返す</returns>
        public static bool AddTypeId(TypeId id)
        {
            if (id == null)
            {
                return false;
            }

            if (!_usedTypeIds.ContainsKey(id.Type))
            {
                _usedTypeIds.Add(id.Type, new HashSet<int>());
            }

            if (_usedTypeIds[id.Type].Contains(id.Id))
            {
                return false;
            }
            _usedTypeIds[id.Type].Add(id.Id);

            return true;
        }

        /// <summary>
        ///     typeとidの組を削除する
        /// </summary>
        /// <param name="id">typeとidの組</param>
        /// <returns>削除に成功すればtrueを返す</returns>
        public static bool RemoveTypeId(TypeId id)
        {
            if (id == null)
            {
                return false;
            }

            if (!_usedTypeIds.ContainsKey(id.Type))
            {
                return false;
            }

            if (!_usedTypeIds[id.Type].Contains(id.Id))
            {
                return false;
            }
            _usedTypeIds[id.Type].Remove(id.Id);

            return true;
        }

        /// <summary>
        ///     新規typeを取得する
        /// </summary>
        /// <param name="startType">探索開始type</param>
        /// <returns>新規type</returns>
        public static int GetNewType(int startType)
        {
            int type = startType;
            while (!_usedTypeIds.ContainsKey(type))
            {
                type++;
            }
            return type;
        }

        /// <summary>
        ///     新規typeを取得する
        /// </summary>
        /// <param name="startType">探索開始type</param>
        /// <param name="id">id</param>
        /// <returns>新規type</returns>
        public static int GetNewType(int startType, int id)
        {
            int type = startType;
            while (ExistsTypeId(type, id))
            {
                type++;
            }
            return type;
        }

        /// <summary>
        ///     新規idを取得する
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="startId">探索開始id</param>
        /// <returns>新規id</returns>
        public static int GetNewId(int type, int startId)
        {
            int id = startId;
            if (!_usedTypeIds.ContainsKey(type))
            {
                return id;
            }
            HashSet<int> ids = _usedTypeIds[type];
            while (ids.Contains(id))
            {
                id++;
            }
            return id;
        }

        /// <summary>
        ///     typeを設定する
        /// </summary>
        /// <param name="typeId">typeとidの組</param>
        /// <param name="type">typeの値</param>
        public static void SetType(TypeId typeId, int type)
        {
            RemoveTypeId(typeId);
            typeId.Type = type;
            AddTypeId(typeId);
        }

        /// <summary>
        ///     idを設定する
        /// </summary>
        /// <param name="typeId">typeとidの組</param>
        /// <param name="id">idの値</param>
        public static void SetId(TypeId typeId, int id)
        {
            RemoveTypeId(typeId);
            typeId.Id = id;
            AddTypeId(typeId);
        }

        /// <summary>
        ///     新規idを取得する
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="startId">探索開始id</param>
        /// <returns>新規id</returns>
        public static TypeId GetNewTypeId(int type, int startId)
        {
            if (!_usedTypeIds.ContainsKey(type))
            {
                _usedTypeIds.Add(type, new HashSet<int>());
            }
            HashSet<int> ids = _usedTypeIds[type];
            int id = startId;
            while (ids.Contains(id))
            {
                id++;
            }
            _usedTypeIds[type].Add(id);
            return new TypeId { Type = type, Id = id };
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
        ///     編集済みフラグを設定する
        /// </summary>
        public static void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        private static void ResetDirtyAll()
        {
            Data.ResetDirtyAll();

            _dirtyFlag = false;
        }

        #endregion
    }
}