using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;

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
        ///     国タグと国家設定の対応付け
        /// </summary>
        private static Dictionary<Country, CountrySettings> _countries;

        /// <summary>
        ///     国タグと国家関係の対応付け
        /// </summary>
        private static Dictionary<Country, Dictionary<Country, Relation>> _relations;

        /// <summary>
        ///     国タグと不可侵条約の対応付け
        /// </summary>
        private static Dictionary<Country, Dictionary<Country, Treaty>> _nonAggressions;

        /// <summary>
        ///     国タグと講和条約の対応付け
        /// </summary>
        private static Dictionary<Country, Dictionary<Country, Treaty>> _peaces;

        /// <summary>
        ///     国タグと諜報設定の対応付け
        /// </summary>
        private static Dictionary<Country, Dictionary<Country, SpySettings>> _spies;

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
            "flak",
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
        ///     陸軍任務文字列
        /// </summary>
        public static readonly string[] LandMissionStrings =
        {
            "",
            "attack",
            "strat_redeploy",
            "support_attack",
            "support_defense",
            "reserves",
            "anti_partisan_duty"
        };

        /// <summary>
        ///     海軍任務文字列
        /// </summary>
        public static readonly string[] NavalMissionStrings =
        {
            "",
            "rebase",
            "convoy_raiding",
            "asw",
            "naval_interdiction",
            "shore_bombardment",
            "amphibious_assault",
            "sea_transport",
            "naval_combat_patrol",
            "naval_port_strike",
            "naval_airbase_strike"
        };

        /// <summary>
        ///     空軍任務文字列
        /// </summary>
        public static readonly string[] AirMissionStrings =
        {
            "",
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
            "convoy_air_raiding",
            "nuke"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     初期化処理
        /// </summary>
        public static void Init()
        {
            // 国家関係を初期化する
            InitDiplomacy();

            // 使用済みのtypeとidの組を初期化する
            InitTypeIds();
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
        ///     ユニット定義ファイル群を再読み込みする
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
            // 読み込み済みならば戻る
            if (_loaded)
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
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

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
                MessageBox.Show(String.Format("{0}: {1}", Resources.FileReadError, _fileName),
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
        ///     ユニット定義ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            return true;
        }

        #endregion

        #region 国家関係

        /// <summary>
        ///     国家関係を初期化する
        /// </summary>
        private static void InitDiplomacy()
        {
            Scenario scenario = Data;
            ScenarioGlobalData data = scenario.GlobalData;

            _countries = new Dictionary<Country, CountrySettings>();
            _relations = new Dictionary<Country, Dictionary<Country, Relation>>();
            _spies = new Dictionary<Country, Dictionary<Country, SpySettings>>();
            foreach (CountrySettings settings in scenario.Countries)
            {
                Country country = settings.Country;

                // 国タグと国家設定の対応付け
                _countries.Add(country, settings);

                // 国タグと国家関係の対応付け
                _relations.Add(country, new Dictionary<Country, Relation>());
                foreach (Relation relation in settings.Relations)
                {
                    _relations[country][relation.Country] = relation;
                }

                // 国タグと諜報設定の対応付け
                _spies.Add(country, new Dictionary<Country, SpySettings>());
                foreach (SpySettings spy in settings.Intelligence)
                {
                    _spies[country][spy.Country] = spy;
                }
            }

            _nonAggressions = new Dictionary<Country, Dictionary<Country, Treaty>>();
            _peaces = new Dictionary<Country, Dictionary<Country, Treaty>>();
            foreach (Treaty treaty in data.Treaties)
            {
                switch (treaty.Type)
                {
                    case TreatyType.NonAggression:
                        // 国タグと不可侵条約の対応付け
                        if (!_nonAggressions.ContainsKey(treaty.Country1))
                        {
                            _nonAggressions.Add(treaty.Country1, new Dictionary<Country, Treaty>());
                        }
                        _nonAggressions[treaty.Country1][treaty.Country2] = treaty;
                        if (!_nonAggressions.ContainsKey(treaty.Country2))
                        {
                            _nonAggressions.Add(treaty.Country2, new Dictionary<Country, Treaty>());
                        }
                        _nonAggressions[treaty.Country2][treaty.Country1] = treaty;
                        break;

                    case TreatyType.Peace:
                        // 国タグと講和条約の対応付け
                        if (!_peaces.ContainsKey(treaty.Country1))
                        {
                            _peaces.Add(treaty.Country1, new Dictionary<Country, Treaty>());
                        }
                        _peaces[treaty.Country1][treaty.Country2] = treaty;
                        if (!_peaces.ContainsKey(treaty.Country2))
                        {
                            _peaces.Add(treaty.Country2, new Dictionary<Country, Treaty>());
                        }
                        _peaces[treaty.Country2][treaty.Country1] = treaty;
                        break;
                }
            }
        }

        /// <summary>
        ///     国家設定を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>国家設定</returns>
        public static CountrySettings GetCountrySettings(Country country)
        {
            if (!_countries.ContainsKey(country))
            {
                return null;
            }
            return _countries[country];
        }

        /// <summary>
        ///     国家設定を設定する
        /// </summary>
        /// <param name="settings">国家設定</param>
        public static void SetCountrySettings(CountrySettings settings)
        {
            Country country = settings.Country;
            _countries[country] = settings;
        }

        /// <summary>
        ///     国家関係を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>国家関係</returns>
        public static Relation GetCountryRelation(Country country1, Country country2)
        {
            if (!_relations.ContainsKey(country1))
            {
                return null;
            }
            if (!_relations[country1].ContainsKey(country2))
            {
                return null;
            }
            return _relations[country1][country2];
        }

        /// <summary>
        ///     国家関係を設定する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <param name="relation">国家関係</param>
        public static void SetCountryRelation(Country country, Relation relation)
        {
            Country target = relation.Country;
            if (!_relations.ContainsKey(country))
            {
                _relations.Add(country, new Dictionary<Country, Relation>());
            }
            _relations[country][target] = relation;
        }

        /// <summary>
        ///     不可侵条約を取得する
        /// </summary>
        /// <param name="country1">対象国1</param>
        /// <param name="country2">対象国2</param>
        /// <returns>不可侵条約</returns>
        public static Treaty GetNonAggression(Country country1, Country country2)
        {
            if (!_nonAggressions.ContainsKey(country1))
            {
                return null;
            }
            if (!_nonAggressions[country1].ContainsKey(country2))
            {
                return null;
            }
            return _nonAggressions[country1][country2];
        }

        /// <summary>
        ///     不可侵条約を設定する
        /// </summary>
        /// <param name="nonAggression">不可侵条約</param>
        public static void SetNonAggression(Treaty nonAggression)
        {
            if (!_nonAggressions.ContainsKey(nonAggression.Country1))
            {
                _nonAggressions.Add(nonAggression.Country1, new Dictionary<Country, Treaty>());
            }
            _nonAggressions[nonAggression.Country1][nonAggression.Country2] = nonAggression;
            if (!_nonAggressions.ContainsKey(nonAggression.Country2))
            {
                _nonAggressions.Add(nonAggression.Country2, new Dictionary<Country, Treaty>());
            }
            _nonAggressions[nonAggression.Country2][nonAggression.Country1] = nonAggression;
        }

        /// <summary>
        ///     不可侵条約を削除する
        /// </summary>
        /// <param name="nonAggression">不可侵条約</param>
        public static void RemoveNonAggression(Treaty nonAggression)
        {
            if (_nonAggressions.ContainsKey(nonAggression.Country1) &&
                _nonAggressions[nonAggression.Country1].ContainsKey(nonAggression.Country2))
            {
                _nonAggressions[nonAggression.Country1].Remove(nonAggression.Country2);
            }
            if (_nonAggressions.ContainsKey(nonAggression.Country2) &&
                _nonAggressions[nonAggression.Country2].ContainsKey(nonAggression.Country1))
            {
                _nonAggressions[nonAggression.Country2].Remove(nonAggression.Country1);
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
            if (!_peaces.ContainsKey(country1))
            {
                return null;
            }
            if (!_peaces[country1].ContainsKey(country2))
            {
                return null;
            }
            return _peaces[country1][country2];
        }

        /// <summary>
        ///     講和条約を設定する
        /// </summary>
        /// <param name="peace">講和条約</param>
        public static void SetPeace(Treaty peace)
        {
            if (!_peaces.ContainsKey(peace.Country1))
            {
                _peaces.Add(peace.Country1, new Dictionary<Country, Treaty>());
            }
            _peaces[peace.Country1][peace.Country2] = peace;
            if (!_peaces.ContainsKey(peace.Country2))
            {
                _peaces.Add(peace.Country2, new Dictionary<Country, Treaty>());
            }
            _peaces[peace.Country2][peace.Country1] = peace;
        }

        /// <summary>
        ///     講和条約を削除する
        /// </summary>
        /// <param name="peace">講和条約</param>
        public static void RemovePeace(Treaty peace)
        {
            if (_peaces.ContainsKey(peace.Country1) &&
                _peaces[peace.Country1].ContainsKey(peace.Country2))
            {
                _peaces[peace.Country1].Remove(peace.Country2);
            }
            if (_peaces.ContainsKey(peace.Country2) &&
                _peaces[peace.Country2].ContainsKey(peace.Country1))
            {
                _peaces[peace.Country2].Remove(peace.Country1);
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
            if (!_spies.ContainsKey(country1))
            {
                return null;
            }
            if (!_spies[country1].ContainsKey(country2))
            {
                return null;
            }
            return _spies[country1][country2];
        }

        /// <summary>
        ///     諜報設定を設定する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <param name="spy">諜報設定</param>
        public static void SetCountryIntelligence(Country country, SpySettings spy)
        {
            if (!_spies.ContainsKey(country))
            {
                _spies.Add(country, new Dictionary<Country, SpySettings>());
            }
            _spies[country][spy.Country] = spy;
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

            foreach (Treaty treaty in data.Treaties)
            {
                AddTypeId(treaty.Id);
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

                foreach (LandUnit unit in settings.LandUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (LandDivision division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                }
                foreach (NavalUnit unit in settings.NavalUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (NavalDivision division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                    foreach (LandUnit landUnit in unit.LandUnits)
                    {
                        AddTypeId(landUnit.Id);
                        foreach (LandDivision landDivision in landUnit.Divisions)
                        {
                            AddTypeId(landDivision.Id);
                        }
                    }
                }
                foreach (AirUnit unit in settings.AirUnits)
                {
                    AddTypeId(unit.Id);
                    foreach (AirDivision division in unit.Divisions)
                    {
                        AddTypeId(division.Id);
                    }
                    foreach (LandUnit landUnit in unit.LandUnits)
                    {
                        AddTypeId(landUnit.Id);
                        foreach (LandDivision landDivision in landUnit.Divisions)
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
            _dirtyFlag = false;
        }

        #endregion
    }
}