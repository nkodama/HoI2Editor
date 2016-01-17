using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Writers
{
    /// <summary>
    ///     シナリオデータのファイル書き込みを担当するクラス
    /// </summary>
    public static class ScenarioWriter
    {
        #region シナリオデータ

        /// <summary>
        ///     シナリオデータをファイルへ書き込む
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void Write(Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                writer.WriteLine("name       = \"{0}\"", scenario.Name);
                writer.WriteLine("panel      = \"{0}\"", scenario.PanelName);
                WriteHeader(scenario.Header, writer);
                WriteGlobalData(scenario.GlobalData, writer);
                WriteHistoryEvents(scenario, writer);
                WriteSleepEvents(scenario, writer);
                WriteSaveDates(scenario, writer);
                WriteMap(scenario.Map, writer);
                WriteEvents(scenario, writer);
                WriteIncludes(scenario, writer);
            }
        }

        /// <summary>
        ///     イベント履歴リストを書き出す
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteHistoryEvents(Scenario scenario, TextWriter writer)
        {
            if (scenario.HistoryEvents.Count > 0)
            {
                writer.WriteLine();
                writer.Write("history = {");
                WriteIdList(scenario.HistoryEvents, writer);
                writer.WriteLine(" }");
            }
        }

        /// <summary>
        ///     休止イベントリストを書き出す
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSleepEvents(Scenario scenario, TextWriter writer)
        {
            if (scenario.SleepEvents.Count > 0)
            {
                writer.WriteLine();
                writer.Write("sleepevent = {");
                WriteIdList(scenario.SleepEvents, writer);
                writer.WriteLine(" }");
            }
        }

        /// <summary>
        ///     保存日時リストを書き出す
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSaveDates(Scenario scenario, TextWriter writer)
        {
            if (scenario.SaveDates == null || scenario.SaveDates.Count == 0)
            {
                return;
            }

            // シナリオ開始日時が設定されていなければ1936/1/1とみなす
            GameDate startDate = scenario.GlobalData?.StartDate ?? new GameDate();

            writer.WriteLine();
            writer.WriteLine("save_date = {");
            foreach (KeyValuePair<int, GameDate> pair in scenario.SaveDates)
            {
                writer.WriteLine("  {0} = {1}", pair.Key, startDate.Difference(pair.Value));
            }
            writer.WriteLine("}");
        }

        /// <summary>
        ///     マップ設定を書き出す
        /// </summary>
        /// <param name="map">マップ設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMap(MapSettings map, TextWriter writer)
        {
            if (map == null)
            {
                return;
            }
            writer.WriteLine();
            writer.WriteLine("map = {");
            writer.WriteLine("  {0} = all", map.All ? "yes" : "no");
            writer.WriteLine();
            foreach (int id in map.Yes)
            {
                writer.WriteLine("  yes = {0}", id);
            }
            foreach (int id in map.No)
            {
                writer.WriteLine("  no = {0}", id);
            }
            if (map.Top != null || map.Bottom != null)
            {
                writer.WriteLine();
            }
            if (map.Top != null)
            {
                writer.WriteLine("  top = {{ x = {0} y = {1} }}", map.Top.X, map.Top.Y);
            }
            if (map.Bottom != null)
            {
                writer.WriteLine("  bottom = {{ x = {0} y = {1} }}", map.Bottom.X, map.Bottom.Y);
            }
            writer.WriteLine("}");
        }

        /// <summary>
        ///     イベントファイルリストを書き出す
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteEvents(Scenario scenario, TextWriter writer)
        {
            if (scenario.EventFiles.Count == 0)
            {
                return;
            }
            writer.WriteLine("# ###################");
            foreach (string name in scenario.EventFiles)
            {
                writer.WriteLine("event      = \"{0}\"", name);
            }
        }

        /// <summary>
        ///     インクルードファイルリストを書き出す
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteIncludes(Scenario scenario, TextWriter writer)
        {
            if (scenario.IncludeFiles.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (string name in scenario.IncludeFiles)
            {
                writer.WriteLine("include = \"{0}\"", name);
            }
        }

        #endregion

        #region シナリオヘッダ

        /// <summary>
        ///     シナリオヘッダを書き出す
        /// </summary>
        /// <param name="header">シナリオヘッダ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteHeader(ScenarioHeader header, TextWriter writer)
        {
            writer.WriteLine("header = {");
            if (!string.IsNullOrEmpty(header.Name))
            {
                writer.WriteLine("  name       = \"{0}\"", header.Name);
            }
            if (header.StartDate != null)
            {
                writer.WriteLine("  startdate  = {{ year = {0} }}", header.StartDate.Year);
            }
            else
            {
                if (header.StartYear > 0)
                {
                    writer.WriteLine("  startyear  = {0}", header.StartYear);
                }
                if (header.EndYear > 0)
                {
                    writer.WriteLine("  endyear    = {0}", header.EndYear);
                }
            }
            if (!header.IsFreeSelection)
            {
                writer.WriteLine("  free       = no");
            }
            if (header.IsBattleScenario)
            {
                writer.WriteLine("  combat     = yes");
            }
            WriteSelectableCountries(header, writer);
            WriteMajorCountries(header, writer);
            writer.WriteLine("}");
        }

        /// <summary>
        ///     選択可能国リストを書き出す
        /// </summary>
        /// <param name="header">シナリオヘッダ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSelectableCountries(ScenarioHeader header, TextWriter writer)
        {
            if (header.SelectableCountries.Count == 0)
            {
                return;
            }
            writer.Write("  selectable = {");
            WriteCountryList(header.SelectableCountries, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     主要国設定リストを書き出す
        /// </summary>
        /// <param name="header">シナリオヘッダ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMajorCountries(ScenarioHeader header, TextWriter writer)
        {
            foreach (MajorCountrySettings major in header.MajorCountries)
            {
                WriteMajorCountry(major, writer);
            }
        }

        /// <summary>
        ///     主要国設定を書き出す
        /// </summary>
        /// <param name="major">主要国設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMajorCountry(MajorCountrySettings major, TextWriter writer)
        {
            writer.Write("  {0}        = {{", Countries.Strings[(int) major.Country]);
            if (!string.IsNullOrEmpty(major.Desc))
            {
                writer.Write(" desc = \"{0}\"", major.Desc);
            }
            if (major.Bottom)
            {
                writer.Write(" bottom = yes");
            }
            if (!string.IsNullOrEmpty(major.PictureName))
            {
                writer.Write(" picture = \"{0}\"", major.PictureName);
            }
            if (!string.IsNullOrEmpty(major.FlagExt))
            {
                writer.Write(" flag_ext = {0}", major.FlagExt);
            }
            if (!string.IsNullOrEmpty(major.Name))
            {
                writer.Write(" name = {0}", major.Name);
            }
            if (!string.IsNullOrEmpty(major.Songs))
            {
                writer.Write($" songs = \"{major.Songs}\"");
            }
            writer.WriteLine(" }");
        }

        #endregion

        #region シナリオグローバルデータ

        /// <summary>
        ///     シナリオグローバルデータを書き出す
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteGlobalData(ScenarioGlobalData data, TextWriter writer)
        {
            writer.WriteLine("globaldata = {");
            WriteRules(data.Rules, writer);
            WriteScenarioStartDate(data.StartDate, writer);
            WriteAlliances(data, writer);
            WriteWars(data, writer);
            WriteTreaties(data, writer);
            WriteScenarioDormantLeaders(data, writer);
            WriteScenarioDormantMinisters(data, writer);
            WriteScenarioDormantTeams(data, writer);
            WriteScenarioEndDate(data.EndDate, writer);
            WriteGlobalFlags(data.Flags, writer);
            writer.WriteLine("}");
        }

        /// <summary>
        ///     シナリオ開始日を書き出す
        /// </summary>
        /// <param name="date">シナリオ開始日</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteScenarioStartDate(GameDate date, TextWriter writer)
        {
            if (date == null)
            {
                return;
            }
            writer.Write("  startdate = ");
            WriteDate(date, writer);
            writer.WriteLine();
        }

        /// <summary>
        ///     シナリオ終了日を書き出す
        /// </summary>
        /// <param name="date">シナリオ終了日</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteScenarioEndDate(GameDate date, TextWriter writer)
        {
            if (date == null)
            {
                return;
            }
            writer.Write("  enddate   = ");
            WriteDate(date, writer);
            writer.WriteLine();
        }

        /// <summary>
        ///     ルール設定を書き出す
        /// </summary>
        /// <param name="rules">ルール設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteRules(ScenarioRules rules, TextWriter writer)
        {
            // 禁止項目がなければセクションを書き出さない
            if (rules.AllowDiplomacy && rules.AllowProduction && rules.AllowTechnology)
            {
                return;
            }

            writer.WriteLine("  rules = {");
            if (!rules.AllowDiplomacy)
            {
                writer.WriteLine("    diplomacy = no");
            }
            if (!rules.AllowProduction)
            {
                writer.WriteLine("    production = no");
            }
            if (!rules.AllowTechnology)
            {
                writer.WriteLine("    technology = no");
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     グローバルフラグリストを書き出す
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="writer"></param>
        private static void WriteGlobalFlags(Dictionary<string, string> flags, TextWriter writer)
        {
            if (flags.Count == 0)
            {
                return;
            }
            writer.WriteLine("  flags = {");
            foreach (KeyValuePair<string, string> pair in flags)
            {
                writer.WriteLine($"    {pair.Key} = {pair.Value}");
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     同盟リストを書き出す
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAlliances(ScenarioGlobalData data, TextWriter writer)
        {
            if (data.Axis?.Id != null)
            {
                WriteAlliance(data.Axis, "axis", writer);
            }
            if (data.Allies?.Id != null)
            {
                WriteAlliance(data.Allies, "allies", writer);
            }
            if (data.Comintern?.Id != null)
            {
                WriteAlliance(data.Comintern, "comintern", writer);
            }
            foreach (Alliance alliance in data.Alliances)
            {
                WriteAlliance(alliance, "alliance", writer);
            }
        }

        /// <summary>
        ///     戦争リストを書き出す
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteWars(ScenarioGlobalData data, TextWriter writer)
        {
            if (data.Wars.Count == 0)
            {
                return;
            }
            foreach (War war in data.Wars)
            {
                WriteWar(war, writer);
            }
        }

        /// <summary>
        ///     外交協定リストを書き出す
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTreaties(ScenarioGlobalData data, TextWriter writer)
        {
            // 不可侵条約
            foreach (Treaty treaty in data.NonAggressions)
            {
                WriteTreaty(treaty, writer);
            }

            // 講和条約
            foreach (Treaty treaty in data.Peaces)
            {
                WriteTreaty(treaty, writer);
            }

            // 貿易
            foreach (Treaty treaty in data.Trades)
            {
                WriteTreaty(treaty, writer);
            }
        }

        /// <summary>
        ///     同盟情報を書き出す
        /// </summary>
        /// <param name="alliance">同盟情報</param>
        /// <param name="type">同盟の種類</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAlliance(Alliance alliance, string type, TextWriter writer)
        {
            writer.WriteLine("  {0} = {{", type);
            if (alliance.Id != null)
            {
                writer.Write("    id          = ");
                WriteTypeId(alliance.Id, writer);
                writer.WriteLine();
            }
            if (!string.IsNullOrEmpty(alliance.Name))
            {
                writer.WriteLine("    name        = \"{0}\"", alliance.Name);
            }
            if (alliance.Defensive)
            {
                writer.WriteLine("    defensive   = yes");
            }
            writer.Write("    participant = {");
            WriteCountryList(alliance.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     戦争情報を書き出す
        /// </summary>
        /// <param name="war">戦争情報</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteWar(War war, TextWriter writer)
        {
            writer.WriteLine("  war = {");
            if (war.Id != null)
            {
                writer.Write("    id          = ");
                WriteTypeId(war.Id, writer);
                writer.WriteLine();
            }
            if (war.StartDate != null)
            {
                writer.Write("    date        = ");
                WriteDate(war.StartDate, writer);
                writer.WriteLine();
            }
            if (war.EndDate != null)
            {
                writer.Write("    enddate     = ");
                WriteDate(war.EndDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    attackers   = {");
            if (war.Attackers.Id != null)
            {
                writer.Write("      id          = ");
                WriteTypeId(war.Attackers.Id, writer);
                writer.WriteLine();
            }
            writer.Write("      participant = {");
            WriteCountryList(war.Attackers.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("    }");
            writer.WriteLine("    defenders   = {");
            if (war.Defenders.Id != null)
            {
                writer.Write("      id          = ");
                WriteTypeId(war.Defenders.Id, writer);
                writer.WriteLine();
            }
            writer.Write("      participant = {");
            WriteCountryList(war.Defenders.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("    }");
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     外交協定情報を書き出す
        /// </summary>
        /// <param name="treaty">外交協定情報</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTreaty(Treaty treaty, TextWriter writer)
        {
            writer.WriteLine("  treaty = {");
            if (treaty.Id != null)
            {
                writer.Write("    id             = ");
                WriteTypeId(treaty.Id, writer);
                writer.WriteLine();
            }
            if (treaty.IsOverSea && (Game.Type == GameType.ArsenalOfDemocracy))
            {
                writer.WriteLine("    isoversea      = yes");
            }
            writer.WriteLine("    type           = {0}", Scenarios.TreatyStrings[(int) treaty.Type]);
            writer.WriteLine("    country        = \"{0}\"", treaty.Country1);
            writer.WriteLine("    country        = \"{0}\"", treaty.Country2);
            if (treaty.StartDate != null)
            {
                writer.Write("    startdate      = ");
                WriteDate(treaty.StartDate, writer);
                writer.WriteLine();
            }
            if (treaty.EndDate != null)
            {
                writer.Write("    expirydate     = ");
                WriteDate(treaty.EndDate, writer);
                writer.WriteLine();
            }
            if (!DoubleHelper.IsZero(treaty.Energy))
            {
                writer.WriteLine("    energy         = {0}", DoubleHelper.ToString(treaty.Energy));
            }
            if (!DoubleHelper.IsZero(treaty.Metal))
            {
                writer.WriteLine("    metal          = {0}", DoubleHelper.ToString(treaty.Metal));
            }
            if (!DoubleHelper.IsZero(treaty.RareMaterials))
            {
                writer.WriteLine("    rare_materials = {0}", DoubleHelper.ToString(treaty.RareMaterials));
            }
            if (!DoubleHelper.IsZero(treaty.Oil))
            {
                writer.WriteLine("    oil            = {0}", DoubleHelper.ToString(treaty.Oil));
            }
            if (!DoubleHelper.IsZero(treaty.Supplies))
            {
                writer.WriteLine("    supplies       = {0}", DoubleHelper.ToString(treaty.Supplies));
            }
            if (!DoubleHelper.IsZero(treaty.Money))
            {
                writer.WriteLine("    money          = {0}", DoubleHelper.ToString(treaty.Money));
            }
            if (!treaty.Cancel)
            {
                writer.WriteLine("    cancel         = no");
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     休止指揮官リストを書き出す (シナリオグローバルデータ)
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteScenarioDormantLeaders(ScenarioGlobalData data, TextWriter writer)
        {
            if (data.DormantLeadersAll)
            {
                writer.WriteLine("  dormant_leaders   = yes");
                return;
            }

            // 休止指揮官がない場合は何も書き出さない
            if (data.DormantLeaders.Count == 0)
            {
                return;
            }

            writer.Write("  dormant_leaders   = {");
            WriteIdList(data.DormantLeaders, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     休止閣僚リストを書き出す (シナリオグローバルデータ)
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteScenarioDormantMinisters(ScenarioGlobalData data, TextWriter writer)
        {
            // 休止閣僚がない場合は何も書き出さない
            if (data.DormantMinisters.Count == 0)
            {
                return;
            }

            writer.Write("  dormant_ministers = {");
            WriteIdList(data.DormantMinisters, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     休止研究機関リストを書き出す (シナリオグローバルデータ)
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteScenarioDormantTeams(ScenarioGlobalData data, TextWriter writer)
        {
            // 休止研究機関がない場合は何も書き出さない
            if (data.DormantTeams.Count == 0)
            {
                return;
            }

            writer.Write("  dormant_teams     = {");
            WriteIdList(data.DormantTeams, writer);
            writer.WriteLine(" }");
        }

        #endregion

        #region プロヴィンス

        /// <summary>
        ///     プロヴィンス設定をbases.incに書き込む
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteBasesInc(Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                if (Game.IsDhFull())
                {
                    // DH Full
                    WriteBasesIncDhFull(scenario, writer);
                }
                else
                {
                    // HoI2/AoD/DH None
                    WriteBasesIncHoI2(scenario, writer);
                }
            }
        }

        /// <summary>
        ///     プロヴィンス設定をbases.incに書き込む (HoI2/AoD/DH None)
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBasesIncHoI2(Scenario scenario, TextWriter writer)
        {
            foreach (ProvinceSettings settings in scenario.Provinces.Where(settings => settings.NavalBase != null))
            {
                Province province = Provinces.Items[settings.Id];
                writer.Write("province = {{ id = {0} naval_base = ", settings.Id);
                WriteBuilding(settings.NavalBase, writer);
                writer.WriteLine(" }} # {0}", Scenarios.GetProvinceName(province, settings));
            }
            writer.WriteLine();
            foreach (ProvinceSettings settings in scenario.Provinces.Where(settings => settings.AirBase != null))
            {
                Province province = Provinces.Items[settings.Id];
                writer.Write("province = {{ id = {0} air_base = ", settings.Id);
                WriteBuilding(settings.AirBase, writer);
                writer.WriteLine(" }");
                writer.WriteLine(" }} # {0}", Scenarios.GetProvinceName(province, settings));
            }
        }

        /// <summary>
        ///     プロヴィンス設定をbases.incに書き込む (DH Full)
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBasesIncDhFull(Scenario scenario, TextWriter writer)
        {
            foreach (ProvinceSettings settings in scenario.Provinces.Where(p => ExistsBasesIncDataDhFull(p, scenario)))
            {
                Province province = Provinces.Items[settings.Id];
                writer.WriteLine("province = {");
                writer.WriteLine("  id = {0} # {1}", settings.Id, Scenarios.GetProvinceName(province, settings));
                if (!scenario.IsBaseDodProvinceSettings)
                {
                    if (settings.Ic != null)
                    {
                        writer.Write("  ic = ");
                        WriteBuilding(settings.Ic, writer);
                        writer.WriteLine();
                    }
                    if (settings.Infrastructure != null)
                    {
                        writer.Write("  infra = ");
                        WriteBuilding(settings.Infrastructure, writer);
                        writer.WriteLine();
                    }
                }
                if (settings.LandFort != null)
                {
                    writer.Write("  landfort = ");
                    WriteBuilding(settings.LandFort, writer);
                    writer.WriteLine();
                }
                if (settings.CoastalFort != null)
                {
                    writer.Write("  coastalfort = ");
                    WriteBuilding(settings.CoastalFort, writer);
                    writer.WriteLine();
                }
                if (settings.AntiAir != null)
                {
                    writer.Write("  anti_air = ");
                    WriteBuilding(settings.AntiAir, writer);
                    writer.WriteLine();
                }
                if (settings.AirBase != null)
                {
                    writer.Write("  air_base = ");
                    WriteBuilding(settings.AirBase, writer);
                    writer.WriteLine();
                }
                if (settings.NavalBase != null)
                {
                    writer.Write("  naval_base = ");
                    WriteBuilding(settings.NavalBase, writer);
                    writer.WriteLine();
                }
                if (settings.RadarStation != null)
                {
                    writer.Write("  radar_station = ");
                    WriteBuilding(settings.RadarStation, writer);
                    writer.WriteLine();
                }
                if (settings.NuclearReactor != null)
                {
                    writer.Write("  nuclear_reactor = ");
                    WriteBuilding(settings.NuclearReactor, writer);
                    writer.WriteLine();
                }
                if (settings.RocketTest != null)
                {
                    writer.Write("  rocket_test = ");
                    WriteBuilding(settings.RocketTest, writer);
                    writer.WriteLine();
                }
                if (!scenario.IsDepotsProvinceSettings)
                {
                    if (settings.SupplyPool > 0)
                    {
                        writer.WriteLine("  supplypool = {0}", DoubleHelper.ToString(settings.SupplyPool));
                    }
                    if (settings.OilPool > 0)
                    {
                        writer.WriteLine("  oilpool = {0}", DoubleHelper.ToString(settings.OilPool));
                    }
                    if (settings.EnergyPool > 0)
                    {
                        writer.WriteLine("  energypool = {0}", DoubleHelper.ToString(settings.EnergyPool));
                    }
                    if (settings.MetalPool > 0)
                    {
                        writer.WriteLine("  metalpool = {0}", DoubleHelper.ToString(settings.MetalPool));
                    }
                    if (settings.RareMaterialsPool > 0)
                    {
                        writer.WriteLine("  rarematerialspool = {0}", DoubleHelper.ToString(settings.RareMaterialsPool));
                    }
                }
                if (settings.Energy > 0)
                {
                    writer.WriteLine("  energy = {0}", DoubleHelper.ToString(settings.Energy));
                }
                if (settings.MaxEnergy > 0)
                {
                    writer.WriteLine("  max_energy = {0}", DoubleHelper.ToString(settings.MaxEnergy));
                }
                if (settings.Metal > 0)
                {
                    writer.WriteLine("  metal = {0}", DoubleHelper.ToString(settings.Metal));
                }
                if (settings.MaxMetal > 0)
                {
                    writer.WriteLine("  max_metal = {0}", DoubleHelper.ToString(settings.MaxMetal));
                }
                if (settings.RareMaterials > 0)
                {
                    writer.WriteLine("  rare_materials = {0}", DoubleHelper.ToString(settings.RareMaterials));
                }
                if (settings.MaxRareMaterials > 0)
                {
                    writer.WriteLine("  max_rare_materials = {0}", DoubleHelper.ToString(settings.MaxRareMaterials));
                }
                if (settings.Oil > 0)
                {
                    writer.WriteLine("  oil = {0}", DoubleHelper.ToString(settings.Oil));
                }
                if (settings.MaxOil > 0)
                {
                    writer.WriteLine("  max_oil = {0}", DoubleHelper.ToString(settings.MaxOil));
                }
                if (settings.Manpower > 0)
                {
                    writer.WriteLine("  manpower = {0}", DoubleHelper.ToString(settings.Manpower));
                }
                if (settings.MaxManpower > 0)
                {
                    writer.WriteLine("  max_manpower = {0}", DoubleHelper.ToString(settings.MaxManpower));
                }
                if (settings.RevoltRisk > 0)
                {
                    writer.WriteLine("  province_revoltrisk = {0}", DoubleHelper.ToString(settings.RevoltRisk));
                }
                if (!string.IsNullOrEmpty(settings.Name))
                {
                    writer.WriteLine("  name = {0}", settings.Name);
                }
                if (settings.Weather != WeatherType.None)
                {
                    writer.WriteLine("  weather = {0}", Scenarios.WeatherStrings[(int) settings.Weather]);
                }
                writer.WriteLine("}");
                writer.WriteLine();
            }
        }

        /// <summary>
        ///     プロヴィンス設定をbases_DOD.incに書き込む
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteBasesDodInc(Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                foreach (ProvinceSettings settings in scenario.Provinces.Where(ExistsBasesDodIncData))
                {
                    Province province = Provinces.Items[settings.Id];
                    writer.Write("province = {{ id = {0}", settings.Id);
                    if (settings.Ic != null)
                    {
                        writer.Write(" ic = ");
                        WriteBuilding(settings.Ic, writer);
                    }
                    if (settings.Infrastructure != null)
                    {
                        writer.Write(" infra = ");
                        WriteBuilding(settings.Infrastructure, writer);
                    }
                    writer.WriteLine(" }} # {0}", Scenarios.GetProvinceName(province, settings));
                }
            }
        }

        /// <summary>
        ///     プロヴィンス設定をdepots.incに書き込む
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteDepotsInc(Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                foreach (ProvinceSettings settings in scenario.Provinces.Where(ExistsDepotsIncData))
                {
                    Province province = Provinces.Items[settings.Id];
                    writer.WriteLine("province = {");
                    writer.WriteLine("  id = {0} # {1}", settings.Id, Scenarios.GetProvinceName(province, settings));
                    if (settings.SupplyPool > 0)
                    {
                        writer.WriteLine("  supplypool = {0}", DoubleHelper.ToString(settings.SupplyPool));
                    }
                    if (settings.OilPool > 0)
                    {
                        writer.WriteLine("  oilpool = {0}", DoubleHelper.ToString(settings.OilPool));
                    }
                    if (settings.EnergyPool > 0)
                    {
                        writer.WriteLine("  energypool = {0}", DoubleHelper.ToString(settings.EnergyPool));
                    }
                    if (settings.MetalPool > 0)
                    {
                        writer.WriteLine("  metalpool = {0}", DoubleHelper.ToString(settings.MetalPool));
                    }
                    if (settings.RareMaterialsPool > 0)
                    {
                        writer.WriteLine("  rarematerialspool = {0}", DoubleHelper.ToString(settings.RareMaterialsPool));
                    }
                    writer.WriteLine("}");
                }
            }
        }

        /// <summary>
        ///     プロヴィンス設定をvp.incに書き込む
        /// </summary>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteVpInc(Scenario scenario, string fileName)
        {
            // vp.incに保存しないならば何もしない
            if (!scenario.IsVpProvinceSettings)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                writer.WriteLine();
                writer.WriteLine("###############################");
                writer.WriteLine("# Victory points distribution #");
                writer.WriteLine("###############################");
                writer.WriteLine();
                foreach (ProvinceSettings settings in scenario.Provinces.Where(settings => settings.Vp != 0))
                {
                    Province province = Provinces.Items[settings.Id];
                    writer.WriteLine("province = {{ id = {0} points = {1} }} # {2}", settings.Id, settings.Vp,
                        Scenarios.GetProvinceName(province, settings));
                }
            }
        }

        /// <summary>
        ///     プロヴィンス設定のリストを国別incに書き込む
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        /// <returns>書き込むデータがあればtrueを返す</returns>
        private static bool WriteCountryProvinces(CountrySettings settings, Scenario scenario,
            TextWriter writer)
        {
            if (!scenario.IsCountryProvinceSettings)
            {
                return false;
            }
            bool exists = false;
            foreach (ProvinceSettings ps in scenario.Provinces.Where(
                ps => settings.ControlledProvinces.Contains(ps.Id) && ExistsCountryIncData(ps, scenario)))
            {
                WriteCountryProvince(ps, scenario, writer);
                exists = true;
            }
            return exists;
        }

        /// <summary>
        ///     プロヴィンス設定を国別incに書き込む
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryProvince(ProvinceSettings settings, Scenario scenario, TextWriter writer)
        {
            if (IsProvinceSingleLine(settings))
            {
                WriteCountryProvinceSingleLine(settings, scenario, writer);
            }
            else
            {
                WriteCountryProvinceMultiLine(settings, scenario, writer);
            }
        }

        /// <summary>
        ///     単一行のプロヴィンス設定を国別incに書き込む
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryProvinceSingleLine(ProvinceSettings settings, Scenario scenario,
            TextWriter writer)
        {
            Province province = Provinces.Items[settings.Id];
            writer.Write("province = {{ id = {0}", settings.Id);
            if (settings.Ic != null)
            {
                writer.Write(" ic = ");
                WriteBuilding(settings.Ic, writer);
            }
            if (settings.Infrastructure != null)
            {
                writer.Write(" infra = ");
                WriteBuilding(settings.Infrastructure, writer);
            }
            if (settings.LandFort != null)
            {
                writer.Write(" landfort = ");
                WriteBuilding(settings.LandFort, writer);
            }
            if (settings.CoastalFort != null)
            {
                writer.Write(" coastalfort = ");
                WriteBuilding(settings.CoastalFort, writer);
            }
            if (settings.AntiAir != null)
            {
                writer.Write(" anti_air = ");
                WriteBuilding(settings.AntiAir, writer);
            }
            if (!scenario.IsBaseProvinceSettings)
            {
                if (settings.AirBase != null)
                {
                    writer.Write(" air_base = ");
                    WriteBuilding(settings.AirBase, writer);
                }
                if (settings.NavalBase != null)
                {
                    writer.Write(" naval_base = ");
                    WriteBuilding(settings.NavalBase, writer);
                }
            }
            if (settings.RadarStation != null)
            {
                writer.Write(" radar_station = ");
                WriteBuilding(settings.RadarStation, writer);
            }
            if (settings.NuclearReactor != null)
            {
                writer.Write(" nuclear_reactor = ");
                WriteBuilding(settings.NuclearReactor, writer);
            }
            if (settings.RocketTest != null)
            {
                writer.Write(" rocket_test = ");
                WriteBuilding(settings.RocketTest, writer);
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                if (settings.SyntheticOil != null)
                {
                    writer.Write(" synthetic_oil = ");
                    WriteBuilding(settings.SyntheticOil, writer);
                }
                if (settings.SyntheticRares != null)
                {
                    writer.Write(" synthetic_rares = ");
                    WriteBuilding(settings.SyntheticRares, writer);
                }
                if (settings.NuclearPower != null)
                {
                    writer.Write(" nuclear_power = ");
                    WriteBuilding(settings.NuclearPower, writer);
                }
            }
            if (settings.SupplyPool > 0)
            {
                writer.Write(" supplypool = {0}", ObjectHelper.ToString(settings.SupplyPool));
            }
            if (settings.OilPool > 0)
            {
                writer.Write(" oilpool = {0}", ObjectHelper.ToString(settings.OilPool));
            }
            writer.WriteLine(" }} # {0}", Scenarios.GetProvinceName(province, settings));
        }

        /// <summary>
        ///     複数行のプロヴィンス設定を国別incに書き込む
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryProvinceMultiLine(ProvinceSettings settings, Scenario scenario,
            TextWriter writer)
        {
            Province province = Provinces.Items[settings.Id];
            writer.WriteLine("province = {");
            writer.WriteLine("  id = {0} # {1}", settings.Id, Scenarios.GetProvinceName(province, settings));
            if (settings.Ic != null)
            {
                writer.Write("  ic = ");
                WriteBuilding(settings.Ic, writer);
                writer.WriteLine();
            }
            if (settings.Infrastructure != null)
            {
                writer.Write("  infra = ");
                WriteBuilding(settings.Infrastructure, writer);
                writer.WriteLine();
            }
            if (settings.LandFort != null)
            {
                writer.Write("  landfort = ");
                WriteBuilding(settings.LandFort, writer);
                writer.WriteLine();
            }
            if (settings.CoastalFort != null)
            {
                writer.Write("  coastalfort = ");
                WriteBuilding(settings.CoastalFort, writer);
                writer.WriteLine();
            }
            if (settings.AntiAir != null)
            {
                writer.Write("  anti_air = ");
                WriteBuilding(settings.AntiAir, writer);
                writer.WriteLine();
            }
            if (!scenario.IsBaseProvinceSettings)
            {
                if (settings.AirBase != null)
                {
                    writer.Write("  air_base = ");
                    WriteBuilding(settings.AirBase, writer);
                    writer.WriteLine();
                }
                if (settings.NavalBase != null)
                {
                    writer.Write("  naval_base = ");
                    WriteBuilding(settings.NavalBase, writer);
                    writer.WriteLine();
                }
            }
            if (settings.RadarStation != null)
            {
                writer.Write("  radar_station = ");
                WriteBuilding(settings.RadarStation, writer);
                writer.WriteLine();
            }
            if (settings.NuclearReactor != null)
            {
                writer.Write("  nuclear_reactor = ");
                WriteBuilding(settings.NuclearReactor, writer);
                writer.WriteLine();
            }
            if (settings.RocketTest != null)
            {
                writer.Write("  rocket_test = ");
                WriteBuilding(settings.RocketTest, writer);
                writer.WriteLine();
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                if (settings.SyntheticOil != null)
                {
                    writer.Write("  synthetic_oil = ");
                    WriteBuilding(settings.SyntheticOil, writer);
                    writer.WriteLine();
                }
                if (settings.SyntheticRares != null)
                {
                    writer.Write("  synthetic_rares = ");
                    WriteBuilding(settings.SyntheticRares, writer);
                    writer.WriteLine();
                }
                if (settings.NuclearPower != null)
                {
                    writer.Write("  nuclear_power = ");
                    WriteBuilding(settings.NuclearPower, writer);
                    writer.WriteLine();
                }
            }
            if (settings.SupplyPool > 0)
            {
                writer.WriteLine("  supplypool = {0}", ObjectHelper.ToString(settings.SupplyPool));
            }
            if (settings.OilPool > 0)
            {
                writer.WriteLine("  oilpool = {0}", ObjectHelper.ToString(settings.OilPool));
            }
            if (settings.EnergyPool > 0)
            {
                writer.WriteLine("  energypool = {0}", ObjectHelper.ToString(settings.EnergyPool));
            }
            if (settings.MetalPool > 0)
            {
                writer.WriteLine("  metalpool = {0}", ObjectHelper.ToString(settings.MetalPool));
            }
            if (settings.RareMaterialsPool > 0)
            {
                writer.WriteLine("  rarematerialspool = {0}", ObjectHelper.ToString(settings.RareMaterialsPool));
            }
            if (settings.Energy > 0)
            {
                writer.WriteLine("  energy = {0}", ObjectHelper.ToString(settings.Energy));
            }
            if (settings.MaxEnergy > 0)
            {
                writer.WriteLine("  max_energy = {0}", ObjectHelper.ToString(settings.MaxEnergy));
            }
            if (settings.Metal > 0)
            {
                writer.WriteLine("  metal = {0}", ObjectHelper.ToString(settings.Metal));
            }
            if (settings.MaxMetal > 0)
            {
                writer.WriteLine("  max_metal = {0}", ObjectHelper.ToString(settings.MaxMetal));
            }
            if (settings.RareMaterials > 0)
            {
                writer.WriteLine("  rare_materials = {0}", ObjectHelper.ToString(settings.RareMaterials));
            }
            if (settings.MaxRareMaterials > 0)
            {
                writer.WriteLine("  max_rare_materials = {0}", ObjectHelper.ToString(settings.MaxRareMaterials));
            }
            if (settings.Oil > 0)
            {
                writer.WriteLine("  oil = {0}", ObjectHelper.ToString(settings.Oil));
            }
            if (settings.MaxOil > 0)
            {
                writer.WriteLine("  max_oil = {0}", ObjectHelper.ToString(settings.MaxOil));
            }
            if (settings.Manpower > 0)
            {
                writer.WriteLine("  manpower = {0}", ObjectHelper.ToString(settings.Manpower));
            }
            if (settings.MaxManpower > 0)
            {
                writer.WriteLine("  max_manpower = {0}", ObjectHelper.ToString(settings.MaxManpower));
            }
            if (settings.RevoltRisk > 0)
            {
                writer.WriteLine("  province_revoltrisk = {0}", ObjectHelper.ToString(settings.RevoltRisk));
            }
            if (!string.IsNullOrEmpty(settings.Name))
            {
                writer.WriteLine("  name = {0} ", settings.Name);
            }
            if (settings.Weather != WeatherType.None)
            {
                writer.WriteLine("  weather = {0}", Scenarios.WeatherStrings[(int) settings.Weather]);
            }
            writer.WriteLine("}");
        }

        /// <summary>
        ///     プロヴィンス設定が単一行で記載できるかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>単一行で記載できればtrueを返す</returns>
        private static bool IsProvinceSingleLine(ProvinceSettings settings)
        {
            if ((settings.Ic != null && settings.Ic.MaxSize > 0) ||
                (settings.Infrastructure != null && settings.Infrastructure.MaxSize > 0) ||
                (settings.LandFort != null && settings.LandFort.MaxSize > 0) ||
                (settings.CoastalFort != null && settings.CoastalFort.MaxSize > 0) ||
                (settings.AntiAir != null && settings.AntiAir.MaxSize > 0) ||
                (settings.AirBase != null && settings.AirBase.MaxSize > 0) ||
                (settings.NavalBase != null && settings.NavalBase.MaxSize > 0) ||
                (settings.RadarStation != null && settings.RadarStation.MaxSize > 0) ||
                (settings.NuclearReactor != null && settings.NuclearReactor.MaxSize > 0) ||
                (settings.RocketTest != null && settings.RocketTest.MaxSize > 0) ||
                (settings.SyntheticOil != null && settings.SyntheticOil.MaxSize > 0) ||
                (settings.SyntheticRares != null && settings.SyntheticRares.MaxSize > 0) ||
                (settings.NuclearPower != null && settings.NuclearPower.MaxSize > 0))
            {
                return false;
            }

            if (settings.EnergyPool > 0 ||
                settings.MetalPool > 0 ||
                settings.RareMaterialsPool > 0 ||
                settings.Energy > 0 ||
                settings.MaxEnergy > 0 ||
                settings.Metal > 0 ||
                settings.MaxMetal > 0 ||
                settings.RareMaterials > 0 ||
                settings.MaxRareMaterials > 0 ||
                settings.Oil > 0 ||
                settings.MaxOil > 0 ||
                settings.Manpower > 0 ||
                settings.MaxManpower > 0 ||
                settings.RevoltRisk > 0)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(settings.Name))
            {
                return false;
            }

            if (settings.Weather != WeatherType.None)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     bases.incに保存するプロヴィンスデータが存在するかどうかを返す (DH Full)
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <returns>bases.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsBasesIncDataDhFull(ProvinceSettings settings, Scenario scenario)
        {
            if (!scenario.IsBaseDodProvinceSettings)
            {
                if (settings.Ic != null || settings.Infrastructure != null)
                {
                    return true;
                }
            }

            if (settings.LandFort != null ||
                settings.CoastalFort != null ||
                settings.AntiAir != null ||
                settings.AirBase != null ||
                settings.NavalBase != null ||
                settings.RadarStation != null ||
                settings.NuclearReactor != null ||
                settings.RocketTest != null ||
                settings.SyntheticOil != null ||
                settings.SyntheticRares != null ||
                settings.NuclearPower != null)
            {
                return true;
            }

            if (!scenario.IsDepotsProvinceSettings)
            {
                if (settings.SupplyPool > 0 ||
                    settings.OilPool > 0 ||
                    settings.EnergyPool > 0 ||
                    settings.MetalPool > 0 ||
                    settings.RareMaterialsPool > 0)
                {
                    return true;
                }
            }

            if (settings.RevoltRisk > 0 ||
                settings.Manpower > 0 ||
                settings.MaxManpower > 0 ||
                settings.Energy > 0 ||
                settings.MaxEnergy > 0 ||
                settings.Metal > 0 ||
                settings.MaxMetal > 0 ||
                settings.RareMaterials > 0 ||
                settings.MaxRareMaterials > 0 ||
                settings.Oil > 0 ||
                settings.MaxOil > 0)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(settings.Name))
            {
                return true;
            }

            if (settings.Weather != WeatherType.None)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     bases_DOD.incに保存するプロヴィンスデータが存在するかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>bases_DOD.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsBasesDodIncData(ProvinceSettings settings)
        {
            return (settings.Ic != null) || (settings.Infrastructure != null);
        }

        /// <summary>
        ///     depots.incに保存するプロヴィンスデータが存在するかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>depots.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsDepotsIncData(ProvinceSettings settings)
        {
            if (settings.EnergyPool > 0 ||
                settings.MetalPool > 0 ||
                settings.RareMaterialsPool > 0 ||
                settings.OilPool > 0 ||
                settings.SupplyPool > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     国別.incに保存するプロヴィンスデータが存在するかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <returns>bases.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsCountryIncData(ProvinceSettings settings, Scenario scenario)
        {
            if (Game.IsDhFull() && scenario.IsBaseProvinceSettings)
            {
                return false;
            }

            if (!scenario.IsBaseDodProvinceSettings)
            {
                if (settings.Ic != null || settings.Infrastructure != null)
                {
                    return true;
                }
            }

            if (settings.LandFort != null ||
                settings.CoastalFort != null ||
                settings.AntiAir != null ||
                settings.AirBase != null ||
                settings.NavalBase != null ||
                settings.RadarStation != null ||
                settings.NuclearReactor != null ||
                settings.RocketTest != null ||
                settings.SyntheticOil != null ||
                settings.SyntheticRares != null ||
                settings.NuclearPower != null)
            {
                return true;
            }

            if (!scenario.IsDepotsProvinceSettings)
            {
                if (settings.SupplyPool > 0 ||
                    settings.OilPool > 0 ||
                    settings.EnergyPool > 0 ||
                    settings.MetalPool > 0 ||
                    settings.RareMaterialsPool > 0)
                {
                    return true;
                }
            }

            if (settings.RevoltRisk > 0 ||
                settings.Manpower > 0 ||
                settings.MaxManpower > 0 ||
                settings.Energy > 0 ||
                settings.MaxEnergy > 0 ||
                settings.Metal > 0 ||
                settings.MaxMetal > 0 ||
                settings.RareMaterials > 0 ||
                settings.MaxRareMaterials > 0 ||
                settings.Oil > 0 ||
                settings.MaxOil > 0)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(settings.Name))
            {
                return true;
            }

            if (settings.Weather != WeatherType.None)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 建物

        /// <summary>
        ///     生産中建物リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBuildingDevelopments(CountrySettings settings, TextWriter writer)
        {
            if (settings.BuildingDevelopments.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (BuildingDevelopment building in settings.BuildingDevelopments)
            {
                WriteBuildingDevelopment(building, writer);
            }
        }

        /// <summary>
        ///     生産中建物を書き出す
        /// </summary>
        /// <param name="building">生産中建物</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBuildingDevelopment(BuildingDevelopment building, TextWriter writer)
        {
            writer.WriteLine("  province_development = {");
            if (building.Id != null)
            {
                writer.Write("    id             = ");
                WriteTypeId(building.Id, writer);
                writer.WriteLine();
            }
            if (building.Name != null)
            {
                writer.WriteLine("    name           = \"{0}\"", building.Name);
            }
            if (building.Progress > 0)
            {
                writer.WriteLine("    progress       = {0}", DoubleHelper.ToString4(building.Progress));
            }
            if (building.Location > 0)
            {
                writer.WriteLine("    location       = {0}", building.Location);
            }
            if (building.Cost > 0)
            {
                writer.WriteLine("    cost           = {0}", DoubleHelper.ToString4(building.Cost));
            }
            if (building.Date != null)
            {
                writer.Write("    date           = ");
                WriteDate(building.Date, writer);
                writer.WriteLine();
            }
            if (building.Manpower > 0)
            {
                writer.WriteLine("    manpower       = {0}", DoubleHelper.ToString4(building.Manpower));
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                if (building.Halted)
                {
                    writer.WriteLine("    halted         = yes");
                }
                writer.WriteLine("    close_when_finished = {0}", BoolHelper.ToString(building.CloseWhenFinished));
                writer.WriteLine("    waitingforclosure = {0}", BoolHelper.ToString(building.WaitingForClosure));
            }
            if (building.TotalProgress > 0)
            {
                writer.WriteLine("    total_progress = {0}", DoubleHelper.ToString4(building.Progress));
            }
            if (building.Size > 0)
            {
                writer.WriteLine("    size           = {0}", building.Size);
            }
            if (building.Done > 0)
            {
                writer.WriteLine("    done           = {0}", building.Done);
            }
            if (building.Days > 0)
            {
                writer.WriteLine("    days           = {0}", building.Days);
            }
            if (building.DaysForFirst > 0)
            {
                writer.WriteLine("    days_for_first = {0}", building.DaysForFirst);
            }
            if (building.GearingBonus > 0)
            {
                writer.WriteLine("    gearing_bonus  = {0}", DoubleHelper.ToString4(building.GearingBonus));
            }
            writer.WriteLine("    type           = {0}", Scenarios.BuildingStrings[(int) building.Type]);
            writer.WriteLine("  }");
        }

        #endregion

        #region 国家設定

        /// <summary>
        ///     国家設定をファイルへ書き込む
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteCountrySettings(CountrySettings settings, Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                WriteCountryHeader(settings, writer);
                if (WriteCountryProvinces(settings, scenario, writer) || Game.IsDhFull())
                {
                    WriteMainHeader(writer);
                }
                writer.WriteLine();
                writer.WriteLine("country = {");
                WriteCountryInfo(settings, writer);
                WriteCountryModifiers(settings, writer);
                WriteCountryResources(settings, writer);
                WriteOffmapResources(settings, writer);
                WriteDiplomacy(settings, writer);
                WriteSpyInfos(settings, writer);
                WriteCountryTerritories(settings, writer);
                WriteTechApps(settings, writer);
                WriteBlueprints(settings, writer);
                WriteInventions(settings, writer);
                WriteDeactivate(settings, writer);
                WriteCountryPolicy(settings, writer);
                WriteCabinet(settings, writer);
                WriteIdea(settings, writer);
                WriteCountryDormantLeaders(settings, writer);
                WriteCountryDormantMinisters(settings, writer);
                WriteCountryDormantTeams(settings, writer);
                WriteStealLeaders(settings, writer);
                WriteAllowedDivisions(settings, writer);
                WriteAllowedBrigades(settings, writer);
                WriteLandUnits(settings, writer);
                WriteNavalUnits(settings, writer);
                WriteAirUnits(settings, writer);
                WriteDivisionDevelopments(settings, writer);
                WriteBuildingDevelopments(settings, writer);
                WriteConvoyDevelopments(settings, writer);
                WriteDormantLandDivisions(settings, writer);
                WriteDormantNavalDivisions(settings, writer);
                WriteDormantAirDivisions(settings, writer);
                writer.WriteLine("}");
            }
        }

        /// <summary>
        ///     国家ヘッダを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryHeader(CountrySettings settings, TextWriter writer)
        {
            writer.WriteLine("##############################");
            writer.WriteLine("# Country definition for {0} #", Countries.Strings[(int) settings.Country]);
            writer.WriteLine("##############################");
        }

        /// <summary>
        ///     メインヘッダを書き出す
        /// </summary>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMainHeader(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("#####################");
            writer.WriteLine("# Country main data #");
            writer.WriteLine("#####################");
        }

        /// <summary>
        ///     国家情報を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryInfo(CountrySettings settings, TextWriter writer)
        {
            writer.WriteLine("  tag                = {0}", Countries.Strings[(int) settings.Country]);
            if (settings.RegularId != Country.None)
            {
                writer.WriteLine("  regular_id         = {0}", Countries.Strings[(int) settings.RegularId]);
            }
            if (settings.Master != Country.None)
            {
                writer.WriteLine("  puppet             = {0}", Countries.Strings[(int) settings.Master]);
            }
            if (settings.Control != Country.None)
            {
                writer.WriteLine("  control            = {0}", Countries.Strings[(int) settings.Control]);
            }
            if (!string.IsNullOrEmpty(settings.Name))
            {
                writer.Write("  name               = ");
                writer.WriteLine(Config.ExistsKey(settings.Name) ? $"{settings.Name}" : $"\"{settings.Name}\"");
            }
            if (!string.IsNullOrEmpty(settings.FlagExt))
            {
                writer.WriteLine("  flag_ext           = {0}", settings.FlagExt);
            }
            if (!string.IsNullOrEmpty(settings.AiFileName))
            {
                writer.WriteLine("  ai                 = \"{0}\"", settings.AiFileName);
            }
            if (settings.AiSettings != null)
            {
                writer.Write("  ai_settings        = { flags = ");
                WriteAiSettings(settings, writer);
                writer.WriteLine(" }");
            }
            if (settings.IntrinsicGovType != GovernmentType.None)
            {
                writer.WriteLine("  intrinsic_gov_type = {0}",
                    Scenarios.GovernmentStrings[(int) settings.IntrinsicGovType]);
            }
            if (settings.Belligerence > 0)
            {
                writer.WriteLine("  belligerence       = {0}", settings.Belligerence);
            }
            writer.WriteLine("  capital            = {0} # {1}", settings.Capital,
                Scenarios.GetProvinceName(settings.Capital));
            if (settings.Dissent > 0)
            {
                writer.WriteLine("  dissent            = {0}", DoubleHelper.ToString(settings.Dissent));
            }
            if (settings.ExtraTc > 0)
            {
                writer.WriteLine("  extra_tc           = {0}", DoubleHelper.ToString(settings.ExtraTc));
            }
            writer.WriteLine("  manpower           = {0}", DoubleHelper.ToString(settings.Manpower));
            if (!DoubleHelper.IsZero(settings.RelativeManpower))
            {
                writer.WriteLine("  relative_manpower  = {0}", DoubleHelper.ToString(settings.RelativeManpower));
            }
            if (!DoubleHelper.IsZero(settings.GroundDefEff))
            {
                writer.WriteLine("  ground_def_eff     = {0}", DoubleHelper.ToString(settings.GroundDefEff));
            }
        }

        /// <summary>
        ///     AI設定を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAiSettings(CountrySettings settings, TextWriter writer)
        {
            writer.Write("{");
            foreach (KeyValuePair<string, string> pair in settings.AiSettings.Flags)
            {
                writer.Write(" {0} = {1}", pair.Key, pair.Value);
            }
            writer.Write(" }");
        }

        /// <summary>
        ///     資源設定を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryResources(CountrySettings settings, TextWriter writer)
        {
            writer.WriteLine("  # Resource Reserves");
            writer.WriteLine("  energy         = {0}", ObjectHelper.ToString(settings.Energy));
            writer.WriteLine("  metal          = {0}", ObjectHelper.ToString(settings.Metal));
            writer.WriteLine("  rare_materials = {0}", ObjectHelper.ToString(settings.RareMaterials));
            writer.WriteLine("  oil            = {0}", ObjectHelper.ToString(settings.Oil));
            writer.WriteLine("  supplies       = {0}", ObjectHelper.ToString(settings.Supplies));
            writer.WriteLine("  money          = {0}", ObjectHelper.ToString(settings.Money));
            if (settings.Transports != 0)
            {
                writer.WriteLine("  transports     = {0}", settings.Transports);
            }
            if (settings.Escorts != 0)
            {
                writer.WriteLine("  escorts        = {0}", settings.Escorts);
            }
        }

        /// <summary>
        ///     マップ外資源設定を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteOffmapResources(CountrySettings settings, TextWriter writer)
        {
            ResourceSettings offmap = settings.Offmap;
            if (offmap == null)
            {
                return;
            }
            writer.WriteLine("  free = {");
            if (!DoubleHelper.IsZero(offmap.Ic))
            {
                writer.WriteLine("    ic             = {0}", ObjectHelper.ToString(offmap.Ic));
            }
            if (!DoubleHelper.IsZero(offmap.Manpower))
            {
                writer.WriteLine("    manpower       = {0}", ObjectHelper.ToString(offmap.Manpower));
            }
            if (!DoubleHelper.IsZero(offmap.Energy))
            {
                writer.WriteLine("    energy         = {0}", ObjectHelper.ToString(offmap.Energy));
            }
            if (!DoubleHelper.IsZero(offmap.Metal))
            {
                writer.WriteLine("    metal          = {0}", ObjectHelper.ToString(offmap.Metal));
            }
            if (!DoubleHelper.IsZero(offmap.RareMaterials))
            {
                writer.WriteLine("    rare_materials = {0}", ObjectHelper.ToString(offmap.RareMaterials));
            }
            if (!DoubleHelper.IsZero(offmap.Oil))
            {
                writer.WriteLine("    oil            = {0}", ObjectHelper.ToString(offmap.Oil));
            }
            if (!DoubleHelper.IsZero(offmap.Supplies))
            {
                writer.WriteLine("    supplies       = {0}", ObjectHelper.ToString(offmap.Supplies));
            }
            if (!DoubleHelper.IsZero(offmap.Money))
            {
                writer.WriteLine("    money          = {0}", ObjectHelper.ToString(offmap.Money));
            }
            if (offmap.Transports != 0)
            {
                writer.WriteLine("    transports     = {0}", offmap.Transports);
            }
            if (offmap.Escorts != 0)
            {
                writer.WriteLine("    escorts        = {0}", offmap.Escorts);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     外交設定を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDiplomacy(CountrySettings settings, TextWriter writer)
        {
            if (settings.Relations.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            writer.WriteLine("  diplomacy = {");
            foreach (Relation relation in settings.Relations)
            {
                WriteRelation(relation, writer);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     国家関係を書き出す
        /// </summary>
        /// <param name="relation">国家関係</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteRelation(Relation relation, TextWriter writer)
        {
            if (relation.Guaranteed == null)
            {
                writer.Write("    relation = {{ tag = {0} value = {1}", Countries.Strings[(int) relation.Country],
                    ObjectHelper.ToString(relation.Value));
                if (relation.Access)
                {
                    writer.Write(" access = yes");
                }
                writer.WriteLine(" }");
            }
            else
            {
                writer.WriteLine("    relation = {");
                writer.WriteLine("      tag        = {0}", Countries.Strings[(int) relation.Country]);
                writer.WriteLine("      value      = {0}", ObjectHelper.ToString(relation.Value));
                if (relation.Access)
                {
                    writer.WriteLine("      access     = yes");
                }
                writer.Write("      guaranteed = ");
                WriteDate(relation.Guaranteed, writer);
                writer.WriteLine();
                writer.WriteLine("    }");
            }
        }

        /// <summary>
        ///     諜報設定リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSpyInfos(CountrySettings settings, TextWriter writer)
        {
            foreach (SpySettings spy in settings.Intelligence)
            {
                writer.WriteLine("  SpyInfo                = {{ country = \"{0}\" NumberOfSpies = {1} }}",
                    Countries.Strings[(int) spy.Country], spy.Spies);
            }
        }

        /// <summary>
        ///     領土設定を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryTerritories(CountrySettings settings, TextWriter writer)
        {
            writer.WriteLine();
            writer.Write("  nationalprovinces      = {");
            WriteIdList(settings.NationalProvinces, writer);
            writer.WriteLine(" }");
            writer.Write("  ownedprovinces         = {");
            WriteIdList(settings.OwnedProvinces, writer);
            writer.WriteLine(" }");
            writer.Write("  controlledprovinces    = {");
            WriteIdList(settings.ControlledProvinces, writer);
            writer.WriteLine(" }");
            if (settings.ClaimedProvinces.Count > 0)
            {
                writer.Write("  claimedprovinces       = {");
                WriteIdList(settings.ClaimedProvinces, writer);
                writer.WriteLine(" }");
            }
        }

        /// <summary>
        ///     保有技術リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTechApps(CountrySettings settings, TextWriter writer)
        {
            if (settings.TechApps.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            writer.Write("  techapps               = {");
            WriteIdList(settings.TechApps, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     青写真リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBlueprints(CountrySettings settings, TextWriter writer)
        {
            if (settings.BluePrints.Count == 0)
            {
                return;
            }
            writer.Write("  blueprints             = {");
            WriteIdList(settings.BluePrints, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     発明イベントリストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteInventions(CountrySettings settings, TextWriter writer)
        {
            if (settings.Inventions.Count == 0)
            {
                return;
            }
            writer.Write("  inventions             = {");
            WriteIdList(settings.Inventions, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     無効技術リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDeactivate(CountrySettings settings, TextWriter writer)
        {
            if (settings.Deactivate.Count == 0)
            {
                return;
            }
            writer.Write("  deactivate             = {");
            WriteIdList(settings.Deactivate, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     政策スライダーを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryPolicy(CountrySettings settings, TextWriter writer)
        {
            CountryPolicy policy = settings.Policy;
            if (policy == null)
            {
                return;
            }
            writer.WriteLine();
            writer.WriteLine("  policy = {");
            if (policy.Date != null)
            {
                writer.Write("    date              = ");
                WriteDate(policy.Date, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    democratic        = {0}", policy.Democratic);
            writer.WriteLine("    political_left    = {0}", policy.PoliticalLeft);
            writer.WriteLine("    freedom           = {0}", policy.Freedom);
            writer.WriteLine("    free_market       = {0}", policy.FreeMarket);
            writer.WriteLine("    professional_army = {0}", policy.ProfessionalArmy);
            writer.WriteLine("    defense_lobby     = {0}", policy.DefenseLobby);
            writer.WriteLine("    interventionism   = {0}", policy.Interventionism);
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     国家補正値を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryModifiers(CountrySettings settings, TextWriter writer)
        {
            if (!DoubleHelper.IsZero(settings.TcModifier))
            {
                writer.WriteLine($"  tc_mod                 = {DoubleHelper.ToString(settings.TcModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.TcOccupiedModifier))
            {
                writer.WriteLine($"  tc_occupied_mod        = {DoubleHelper.ToString(settings.TcOccupiedModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.AttritionModifier))
            {
                writer.WriteLine($"  attrition_mod          = {DoubleHelper.ToString(settings.AttritionModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.TricklebackModifier))
            {
                writer.WriteLine($"  trickleback_mod        = {DoubleHelper.ToString(settings.TricklebackModifier)}");
            }
            if (settings.MaxAmphibModifier > 0)
            {
                writer.WriteLine($"  max_amphib_mod         = {IntHelper.ToString(settings.MaxAmphibModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.SupplyDistModifier))
            {
                writer.WriteLine($"  supply_dist_mod        = {DoubleHelper.ToString(settings.SupplyDistModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.RepairModifier))
            {
                writer.WriteLine($"  repair_mod             = {DoubleHelper.ToString(settings.RepairModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.ResearchModifier))
            {
                writer.WriteLine($"  research_mod           = {DoubleHelper.ToString(settings.ResearchModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.PeacetimeIcModifier))
            {
                writer.WriteLine($"  peacetime_ic_mod       = {DoubleHelper.ToString(settings.PeacetimeIcModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.WartimeIcModifier))
            {
                writer.WriteLine($"  wartime_ic_mod         = {DoubleHelper.ToString(settings.WartimeIcModifier)}");
            }
            if (!DoubleHelper.IsZero(settings.WartimeIcModifier))
            {
                writer.WriteLine($"  industrial_modifier    = {DoubleHelper.ToString(settings.WartimeIcModifier)}");
            }
        }

        /// <summary>
        ///     閣僚リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCabinet(CountrySettings settings, TextWriter writer)
        {
            if (settings.HeadOfState != null)
            {
                writer.Write("  headofstate            = ");
                WriteMinister(settings.HeadOfState, writer);
            }
            if (settings.HeadOfGovernment != null)
            {
                writer.Write("  headofgovernment       = ");
                WriteMinister(settings.HeadOfGovernment, writer);
            }
            if (settings.ForeignMinister != null)
            {
                writer.Write("  foreignminister        = ");
                WriteMinister(settings.ForeignMinister, writer);
            }
            if (settings.ArmamentMinister != null)
            {
                writer.Write("  armamentminister       = ");
                WriteMinister(settings.ArmamentMinister, writer);
            }
            if (settings.MinisterOfSecurity != null)
            {
                writer.Write("  ministerofsecurity     = ");
                WriteMinister(settings.MinisterOfSecurity, writer);
            }
            if (settings.MinisterOfIntelligence != null)
            {
                writer.Write("  ministerofintelligence = ");
                WriteMinister(settings.MinisterOfIntelligence, writer);
            }
            if (settings.ChiefOfStaff != null)
            {
                writer.Write("  chiefofstaff           = ");
                WriteMinister(settings.ChiefOfStaff, writer);
            }
            if (settings.ChiefOfArmy != null)
            {
                writer.Write("  chiefofarmy            = ");
                WriteMinister(settings.ChiefOfArmy, writer);
            }
            if (settings.ChiefOfNavy != null)
            {
                writer.Write("  chiefofnavy            = ");
                WriteMinister(settings.ChiefOfNavy, writer);
            }
            if (settings.ChiefOfAir != null)
            {
                writer.Write("  chiefofair             = ");
                WriteMinister(settings.ChiefOfAir, writer);
            }
        }

        /// <summary>
        ///     閣僚を書き出す
        /// </summary>
        /// <param name="typeId">閣僚のtypeとid</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMinister(TypeId typeId, TextWriter writer)
        {
            WriteTypeId(typeId, writer);
            Minister minister = Ministers.Items.Find(m => m.Id == typeId.Id);
            if (minister != null)
            {
                writer.WriteLine(" # {0}", minister.Name);
            }
            else
            {
                writer.WriteLine();
            }
        }

        /// <summary>
        ///     国策を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteIdea(CountrySettings settings, TextWriter writer)
        {
            // 国策をサポートするのはAoDのみ
            if (Game.Type != GameType.ArsenalOfDemocracy)
            {
                return;
            }

            if (!string.IsNullOrEmpty(settings.NationalIdentity))
            {
                writer.WriteLine("  nationalidentity       = \"{0}\"", settings.NationalIdentity);
            }
            if (!string.IsNullOrEmpty(settings.SocialPolicy))
            {
                writer.WriteLine("  socialpolicy           = \"{0}\"", settings.SocialPolicy);
            }
            if (!string.IsNullOrEmpty(settings.NationalCulture))
            {
                writer.WriteLine("  nationalculture        = \"{0}\"", settings.NationalCulture);
            }
        }

        /// <summary>
        ///     休止指揮官リストを書き出す (国家設定)
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryDormantLeaders(CountrySettings settings, TextWriter writer)
        {
            if (settings.DormantLeaders.Count == 0)
            {
                return;
            }
            writer.Write("  dormant_leaders        = {");
            WriteIdList(settings.DormantLeaders, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     休止閣僚リストを書き出す (国家設定)
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryDormantMinisters(CountrySettings settings, TextWriter writer)
        {
            if (settings.DormantMinisters.Count == 0)
            {
                return;
            }
            writer.Write("  dormant_ministers      = {");
            WriteIdList(settings.DormantMinisters, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     休止研究機関リストを書き出す (国家設定)
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryDormantTeams(CountrySettings settings, TextWriter writer)
        {
            if (settings.DormantTeams.Count == 0)
            {
                return;
            }
            writer.Write("  dormant_teams          = {");
            WriteIdList(settings.DormantTeams, writer);
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     抽出指揮官リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteStealLeaders(CountrySettings settings, TextWriter writer)
        {
            if (settings.StealLeaders.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (int id in settings.StealLeaders)
            {
                writer.WriteLine("  steal_leader = {0}", id);
            }
        }

        #endregion

        #region ユニット

        /// <summary>
        ///     陸軍ユニットリストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLandUnits(CountrySettings settings, TextWriter writer)
        {
            if (settings.LandUnits.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Unit unit in settings.LandUnits)
            {
                WriteLandUnit(unit, writer, "  ");
            }
        }

        /// <summary>
        ///     海軍ユニットリストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalUnits(CountrySettings settings, TextWriter writer)
        {
            if (settings.NavalUnits.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Unit unit in settings.NavalUnits)
            {
                WriteNavalUnit(unit, writer);
            }
        }

        /// <summary>
        ///     空軍ユニットリストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirUnits(CountrySettings settings, TextWriter writer)
        {
            if (settings.AirUnits.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Unit unit in settings.AirUnits)
            {
                WriteAirUnit(unit, writer);
            }
        }

        /// <summary>
        ///     陸軍ユニットを書き出す
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <param name="writer">ファイル書き込み用</param>
        /// <param name="indent">インデント文字列</param>
        private static void WriteLandUnit(Unit unit, TextWriter writer, string indent)
        {
            writer.WriteLine("{0}landunit = {{", indent);
            if (unit.Id != null)
            {
                writer.Write("{0}  id       = ", indent);
                WriteTypeId(unit.Id, writer);
                writer.WriteLine();
            }
            if (!string.IsNullOrEmpty(unit.Name))
            {
                writer.WriteLine("{0}  name     = \"{1}\"", indent, unit.Name);
            }
            if (unit.Location > 0)
            {
                writer.WriteLine("{0}  location = {1}", indent, unit.Location);
            }
            if (unit.PrevProv > 0)
            {
                writer.WriteLine("{0}  prevprov = {1}", indent, unit.PrevProv);
            }
            if (unit.Home > 0)
            {
                writer.WriteLine("{0}  home     = {1}", indent, unit.Home);
            }
            if (unit.Mission != null)
            {
                WriteLandMission(unit.Mission, writer, indent + "  ");
            }
            if (unit.Date != null)
            {
                writer.Write("{0}  date     = ", indent);
                WriteDate(unit.Date, writer);
                writer.WriteLine();
            }
            if (Game.Type == GameType.ArsenalOfDemocracy && !unit.Development)
            {
                writer.WriteLine("{0}  development = no", indent);
            }
            if (unit.StandGround)
            {
                writer.WriteLine("{0}  stand_ground = yes", indent);
            }
            if (unit.ScorchGround)
            {
                writer.WriteLine("{0}  scorch_ground = yes", indent);
            }
            if (unit.Prioritized)
            {
                writer.WriteLine("{0}  prioritized = yes", indent);
            }
            if (unit.CanUpgrade)
            {
                writer.WriteLine("{0}  can_upgrade = no", indent);
            }
            if (unit.CanReinforcement)
            {
                writer.WriteLine("{0}  can_reinforce = no", indent);
            }
            if (unit.Control != Country.None)
            {
                writer.WriteLine("{0}  control  = {1}", indent, Countries.Strings[(int) unit.Control]);
            }
            if (unit.Morale > 0)
            {
                writer.WriteLine("{0}  morale   = {1}", indent, DoubleHelper.ToString(unit.Morale));
            }
            if (unit.Leader > 0)
            {
                writer.WriteLine("{0}  leader   = {1}", indent, unit.Leader);
            }
            if (unit.MoveTime != null)
            {
                writer.Write("{0}  movetime = ", indent);
                WriteDate(unit.MoveTime, writer);
                writer.WriteLine();
            }
            if (unit.Movement.Count > 0)
            {
                writer.Write("{0}  movement = {{", indent);
                WriteIdList(unit.Movement, writer);
                writer.WriteLine(" }");
            }
            foreach (Division division in unit.Divisions)
            {
                WriteLandDivision(division, writer, indent + "  ");
            }
            if (unit.DigIn > 0)
            {
                writer.WriteLine("{0}  dig_in   = {1}", indent, DoubleHelper.ToString3(unit.DigIn));
            }
            if (unit.AttackDate != null)
            {
                writer.Write("{0}  attack   = ", indent);
                WriteDate(unit.AttackDate, writer);
                writer.WriteLine();
            }
            if (unit.Invasion)
            {
                writer.WriteLine("{0}  invasion = yes", indent);
            }
            if (unit.Target > 0)
            {
                writer.WriteLine("{0}  target   = {1}", indent, unit.Target);
            }
            writer.WriteLine("{0}}}", indent);
        }

        /// <summary>
        ///     海軍ユニットを書き出す
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalUnit(Unit unit, TextWriter writer)
        {
            writer.WriteLine("  navalunit = {");
            if (unit.Id != null)
            {
                writer.Write("    id       = ");
                WriteTypeId(unit.Id, writer);
                writer.WriteLine();
            }
            if (!string.IsNullOrEmpty(unit.Name))
            {
                writer.WriteLine("    name     = \"{0}\"", unit.Name);
            }
            if (unit.Location > 0)
            {
                writer.WriteLine("    location = {0}", unit.Location);
            }
            if (unit.PrevProv > 0)
            {
                writer.WriteLine("    prevprov = {0}", unit.PrevProv);
            }
            if (unit.Home > 0)
            {
                writer.WriteLine("    home     = {0}", unit.Home);
            }
            if (unit.Base > 0)
            {
                writer.WriteLine("    base     = {0}", unit.Base);
            }
            if (unit.Mission != null)
            {
                WriteNavalMission(unit.Mission, writer);
            }
            if (unit.Date != null)
            {
                writer.Write("    date     = ");
                WriteDate(unit.Date, writer);
                writer.WriteLine();
            }
            if (Game.Type == GameType.ArsenalOfDemocracy && !unit.Development)
            {
                writer.WriteLine("    development = no");
            }
            if (unit.StandGround)
            {
                writer.WriteLine("    stand_ground = yes");
            }
            if (unit.Prioritized)
            {
                writer.WriteLine("    prioritized = yes");
            }
            if (unit.CanUpgrade)
            {
                writer.WriteLine("    can_upgrade = no");
            }
            if (unit.CanReinforcement)
            {
                writer.WriteLine("    can_reinforce = no");
            }
            if (unit.Control != Country.None)
            {
                writer.WriteLine("    control  = {0}", Countries.Strings[(int) unit.Control]);
            }
            if (unit.Morale > 0)
            {
                writer.WriteLine("    morale   = {0}", DoubleHelper.ToString(unit.Morale));
            }
            if (unit.Leader > 0)
            {
                writer.WriteLine("    leader   = {0}", unit.Leader);
            }
            if (unit.MoveTime != null)
            {
                writer.Write("    movetime = ");
                WriteDate(unit.MoveTime, writer);
                writer.WriteLine();
            }
            if (unit.Movement.Count > 0)
            {
                writer.Write("    movement = {");
                WriteIdList(unit.Movement, writer);
                writer.WriteLine(" }");
            }
            foreach (Division division in unit.Divisions)
            {
                WriteNavalDivision(division, writer);
            }
            foreach (Unit landUnit in unit.LandUnits)
            {
                WriteLandUnit(landUnit, writer, "    ");
            }
            if (unit.AttackDate != null)
            {
                writer.Write("    attack   = ");
                WriteDate(unit.AttackDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     空軍ユニットを書き出す
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirUnit(Unit unit, TextWriter writer)
        {
            writer.WriteLine("  airunit = { ");
            if (unit.Id != null)
            {
                writer.Write("    id       = ");
                WriteTypeId(unit.Id, writer);
                writer.WriteLine();
            }
            if (!string.IsNullOrEmpty(unit.Name))
            {
                writer.WriteLine("    name     = \"{0}\"", unit.Name);
            }
            if (unit.Location > 0)
            {
                writer.WriteLine("    location = {0}", unit.Location);
            }
            if (unit.PrevProv > 0)
            {
                writer.WriteLine("    prevprov = {0}", unit.PrevProv);
            }
            if (unit.Home > 0)
            {
                writer.WriteLine("    home     = {0}", unit.Home);
            }
            if (unit.Base > 0)
            {
                writer.WriteLine("    base     = {0}", unit.Base);
            }
            if (unit.Mission != null)
            {
                WriteAirMission(unit.Mission, writer);
            }
            if (unit.Date != null)
            {
                writer.Write("    date     = ");
                WriteDate(unit.Date, writer);
                writer.WriteLine();
            }
            if (Game.Type == GameType.ArsenalOfDemocracy && !unit.Development)
            {
                writer.WriteLine("    development = no");
            }
            if (unit.Prioritized)
            {
                writer.WriteLine("    prioritized = yes");
            }
            if (unit.CanUpgrade)
            {
                writer.WriteLine("    can_upgrade = no");
            }
            if (unit.CanReinforcement)
            {
                writer.WriteLine("    can_reinforce = no");
            }
            if (unit.Control != Country.None)
            {
                writer.WriteLine("    control  = {0}", Countries.Strings[(int) unit.Control]);
            }
            if (unit.Morale > 0)
            {
                writer.WriteLine("    morale   = {0}", DoubleHelper.ToString(unit.Morale));
            }
            if (unit.Leader > 0)
            {
                writer.WriteLine("    leader   = {0}", unit.Leader);
            }
            if (unit.MoveTime != null)
            {
                writer.Write("    movetime = ");
                WriteDate(unit.MoveTime, writer);
                writer.WriteLine();
            }
            if (unit.Movement.Count > 0)
            {
                writer.Write("    movement = {");
                WriteIdList(unit.Movement, writer);
                writer.WriteLine(" }");
            }
            foreach (Division division in unit.Divisions)
            {
                WriteAirDivision(division, writer);
            }
            foreach (Unit landUnit in unit.LandUnits)
            {
                WriteLandUnit(landUnit, writer, "    ");
            }
            if (unit.AttackDate != null)
            {
                writer.Write("    attack   = ");
                WriteDate(unit.AttackDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("  }");
        }

        #endregion

        #region 師団

        /// <summary>
        ///     陸軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        /// <param name="indent">インデント文字列</param>
        private static void WriteLandDivision(Division division, TextWriter writer, string indent)
        {
            writer.WriteLine("{0}division = {{", indent);
            writer.Write("{0}  id             = ", indent);
            WriteTypeId(division.Id, writer);
            writer.WriteLine();
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.WriteLine("{0}  name           = \"{1}\"", indent, division.Name);
            }
            writer.WriteLine("{0}  type           = {1}", indent, Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.WriteLine("{0}  model          = {1}", indent, division.Model);
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.WriteLine("{0}  extra{1}         = {2}", indent,
                    division.Extra2 == UnitType.Undefined ? " " : "1", Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.WriteLine("{0}  extra2         = {1}", indent, Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.WriteLine("{0}  extra3         = {1}", indent, Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.WriteLine("{0}  extra4         = {1}", indent, Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.WriteLine("{0}  extra5         = {1}", indent, Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.WriteLine("{0}  brigade_model{1} = {2}", indent,
                    division.Extra2 == UnitType.Undefined ? " " : "1", division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.WriteLine("{0}  brigade_model2 = {1}", indent, division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.WriteLine("{0}  brigade_model3 = {1}", indent, division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.WriteLine("{0}  brigade_model4 = {1}", indent, division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.WriteLine("{0}  brigade_model5 = {1}", indent, division.BrigadeModel5);
            }
            if (division.Strength > 0)
            {
                writer.WriteLine("{0}  strength       = {1}", indent, DoubleHelper.ToString(division.Strength));
            }
            if (division.MaxStrength > 0)
            {
                writer.WriteLine("{0}  max_strength   = {1}", indent, DoubleHelper.ToString(division.MaxStrength));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("{0}  organisation   = {1}", indent, DoubleHelper.ToString(division.Organisation));
            }
            if (division.Morale > 0)
            {
                writer.WriteLine("{0}  morale         = {1}", indent, DoubleHelper.ToString(division.Morale));
            }
            if (division.Experience > 0)
            {
                writer.WriteLine("{0}  experience     = {1}", indent, DoubleHelper.ToString(division.Experience));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("{0}  defaultorganisation = {1}", indent,
                    DoubleHelper.ToString(division.MaxOrganisation));
            }
            if (division.UpgradeProgress > 0)
            {
                writer.WriteLine("{0}  div_upgr_progress = {1}", indent, DoubleHelper.ToString(division.Experience));
            }
            if (division.RedeployTarget > 0)
            {
                writer.WriteLine("{0}  redep_target = {1}", indent, division.RedeployTarget);
            }
            if (division.RedeployUnitName != null)
            {
                writer.WriteLine("{0}  redep_unit_name = {1}", indent, division.RedeployUnitName);
            }
            if (division.RedeployUnitId != null)
            {
                writer.Write("{0}  redep_unit_id = ", indent);
                WriteTypeId(division.Id, writer);
                writer.WriteLine();
            }
            if (division.Offensive != null)
            {
                writer.Write("{0}  offensive = ", indent);
                WriteDate(division.Offensive, writer);
                writer.WriteLine();
            }
            if (division.Supplies > 0)
            {
                writer.WriteLine("{0}  supplies = {1}", indent, DoubleHelper.ToString(division.Supplies));
            }
            if (division.Fuel > 0)
            {
                writer.WriteLine("{0}  oil = {1}", indent, DoubleHelper.ToString(division.Fuel));
            }
            if (division.MaxSupplies > 0)
            {
                writer.WriteLine("{0}  max_supply_stock = {1}", indent, DoubleHelper.ToString(division.MaxSupplies));
            }
            if (division.MaxFuel > 0)
            {
                writer.WriteLine("{0}  max_oil_stock  = {1}", indent, DoubleHelper.ToString(division.MaxFuel));
            }
            if (division.MaxSpeed > 0)
            {
                writer.WriteLine("{0}  maxspeed = {1}", indent, DoubleHelper.ToString(division.MaxSpeed));
            }
            if (division.SupplyConsumption > 0)
            {
                writer.WriteLine("{0}  supplyconsumption = {1}", indent,
                    DoubleHelper.ToString(division.SupplyConsumption));
            }
            if (division.FuelConsumption > 0)
            {
                writer.WriteLine("{0}  fuelconsumption = {1}", indent, DoubleHelper.ToString(division.FuelConsumption));
            }
            if (division.Defensiveness > 0)
            {
                writer.WriteLine("{0}  defensiveness = {1}", indent, DoubleHelper.ToString(division.Defensiveness));
            }
            if (division.Toughness > 0)
            {
                writer.WriteLine("{0}  toughness = {1}", indent, DoubleHelper.ToString(division.Toughness));
            }
            if (division.Softness > 0)
            {
                writer.WriteLine("{0}  softness = {1}", indent, DoubleHelper.ToString(division.Softness));
            }
            if (division.Suppression > 0)
            {
                writer.WriteLine("{0}  suppression = {1}", indent, DoubleHelper.ToString(division.Suppression));
            }
            if (division.AirDefence > 0)
            {
                writer.WriteLine("{0}  airdefence = {1}", indent, DoubleHelper.ToString(division.AirDefence));
            }
            if (division.SoftAttack > 0)
            {
                writer.WriteLine("{0}  softattack = {1}", indent, DoubleHelper.ToString(division.SoftAttack));
            }
            if (division.HardAttack > 0)
            {
                writer.WriteLine("{0}  hardattack = {1}", indent, DoubleHelper.ToString(division.HardAttack));
            }
            if (division.TransportWeight > 0)
            {
                writer.WriteLine("{0}  transportweight = {1}", indent, DoubleHelper.ToString(division.TransportWeight));
            }
            if (division.AirAttack > 0)
            {
                writer.WriteLine("{0}  airattack = {1}", indent, DoubleHelper.ToString(division.AirAttack));
            }
            if (division.SpeedCapArt > 0)
            {
                writer.WriteLine("{0}  speed_cap_art = {1}", indent, DoubleHelper.ToString(division.SpeedCapArt));
            }
            if (division.SpeedCapEng > 0)
            {
                writer.WriteLine("{0}  speed_cap_eng = {1}", indent, DoubleHelper.ToString(division.SpeedCapEng));
            }
            if (division.SpeedCapAa > 0)
            {
                writer.WriteLine("{0}  speed_cap_aa = {1}", indent, DoubleHelper.ToString(division.SpeedCapAa));
            }
            if (division.SpeedCapAt > 0)
            {
                writer.WriteLine("{0}  speed_cap_at = {1}", indent, DoubleHelper.ToString(division.SpeedCapAt));
            }
            if (division.ArtilleryBombardment > 0)
            {
                writer.WriteLine("{0}  artillery_bombardment = {1}", indent,
                    DoubleHelper.ToString(division.ArtilleryBombardment));
            }
            if (division.Dormant)
            {
                writer.WriteLine("{0}  dormant        = yes", indent);
            }
            if (division.Locked)
            {
                writer.WriteLine("{0}  locked         = yes", indent);
            }
            writer.WriteLine("{0}}}", indent);
        }

        /// <summary>
        ///     海軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalDivision(Division division, TextWriter writer)
        {
            writer.WriteLine("    division = {");
            writer.Write("      id             = ");
            WriteTypeId(division.Id, writer);
            writer.WriteLine();
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.WriteLine("      name           = \"{0}\"", division.Name);
            }
            writer.WriteLine("      type           = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.WriteLine("      model          = {0}", division.Model);
            }
            if (division.Nuke)
            {
                writer.WriteLine("      nuke           = yes");
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.WriteLine("      extra1         = {0}", Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.WriteLine("      extra2         = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.WriteLine("      extra3         = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.WriteLine("      extra4         = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.WriteLine("      extra5         = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.WriteLine("      brigade_model1 = {0}", division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.WriteLine("      brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.WriteLine("      brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.WriteLine("      brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.WriteLine("      brigade_model5 = {0}", division.BrigadeModel5);
            }
            if (division.Strength > 0)
            {
                writer.WriteLine("      strength       = {0}", DoubleHelper.ToString(division.Strength));
            }
            if (division.MaxStrength > 0)
            {
                writer.WriteLine("      max_strength   = {0}", DoubleHelper.ToString(division.MaxStrength));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("      organisation   = {0}", DoubleHelper.ToString(division.Organisation));
            }
            if (division.Morale > 0)
            {
                writer.WriteLine("      morale         = {0}", DoubleHelper.ToString(division.Morale));
            }
            if (division.Experience > 0)
            {
                writer.WriteLine("      experience     = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("      defaultorganisation = {0}", DoubleHelper.ToString(division.MaxOrganisation));
            }
            if (division.UpgradeProgress > 0)
            {
                writer.WriteLine("      div_upgr_progress = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Supplies > 0)
            {
                writer.WriteLine("      supplies = {0}", DoubleHelper.ToString(division.Supplies));
            }
            if (division.Fuel > 0)
            {
                writer.WriteLine("      oil = {0}", DoubleHelper.ToString(division.Fuel));
            }
            if (division.MaxSupplies > 0)
            {
                writer.WriteLine("      max_supply_stock = {0}", DoubleHelper.ToString(division.MaxSupplies));
            }
            if (division.MaxFuel > 0)
            {
                writer.WriteLine("      max_oil_stock  = {0}", DoubleHelper.ToString(division.MaxFuel));
            }
            if (division.MaxSpeed > 0)
            {
                writer.WriteLine("      maxspeed = {0}", DoubleHelper.ToString(division.MaxSpeed));
            }
            if (division.SupplyConsumption > 0)
            {
                writer.WriteLine("      supplyconsumption = {0}", DoubleHelper.ToString(division.SupplyConsumption));
            }
            if (division.FuelConsumption > 0)
            {
                writer.WriteLine("      fuelconsumption = {0}", DoubleHelper.ToString(division.FuelConsumption));
            }
            if (division.SurfaceDetection > 0)
            {
                writer.WriteLine("      surfacedetectioncapability = {0}",
                    DoubleHelper.ToString(division.SurfaceDetection));
            }
            if (division.AirDetection > 0)
            {
                writer.WriteLine("      airdetectioncapability = {0}", DoubleHelper.ToString(division.AirDetection));
            }
            if (division.SubDetection > 0)
            {
                writer.WriteLine("      subdetectioncapability = {0}", DoubleHelper.ToString(division.SubDetection));
            }
            if (division.Visibility > 0)
            {
                writer.WriteLine("      visibility = {0}", DoubleHelper.ToString(division.Visibility));
            }
            if (division.SeaDefense > 0)
            {
                writer.WriteLine("      seadefence = {0}", DoubleHelper.ToString(division.SeaDefense));
            }
            if (division.AirDefence > 0)
            {
                writer.WriteLine("      airdefence = {0}", DoubleHelper.ToString(division.AirDefence));
            }
            if (division.SeaAttack > 0)
            {
                writer.WriteLine("      seaattack = {0}", DoubleHelper.ToString(division.SeaAttack));
            }
            if (division.SubAttack > 0)
            {
                writer.WriteLine("      subattack = {0}", DoubleHelper.ToString(division.SubAttack));
            }
            if (division.ConvoyAttack > 0)
            {
                writer.WriteLine("      convoyattack = {0}", DoubleHelper.ToString(division.ConvoyAttack));
            }
            if (division.ShoreBombardment > 0)
            {
                writer.WriteLine("      shorebombardment = {0}", DoubleHelper.ToString(division.ShoreBombardment));
            }
            if (division.AirAttack > 0)
            {
                writer.WriteLine("      airattack = {0}", DoubleHelper.ToString(division.AirAttack));
            }
            if (division.TransportCapability > 0)
            {
                writer.WriteLine("      transportcapability = {0}", DoubleHelper.ToString(division.TransportCapability));
            }
            if (division.Range > 0)
            {
                writer.WriteLine("      range = {0}", DoubleHelper.ToString(division.Range));
            }
            if (division.Distance > 0)
            {
                writer.WriteLine("      distance = {0}", DoubleHelper.ToString(division.Distance));
            }
            if (division.Travelled > 0)
            {
                writer.WriteLine("      travelled = {0}", DoubleHelper.ToString(division.Travelled));
            }
            if (division.Dormant)
            {
                writer.WriteLine("      dormant        = yes");
            }
            writer.WriteLine("    }");
        }

        /// <summary>
        ///     空軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirDivision(Division division, TextWriter writer)
        {
            writer.WriteLine("    division = {");
            writer.Write("      id             = ");
            WriteTypeId(division.Id, writer);
            writer.WriteLine();
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.WriteLine("      name           = \"{0}\"", division.Name);
            }
            writer.WriteLine("      type           = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.WriteLine("      model          = {0}", division.Model);
            }
            if (division.Nuke)
            {
                writer.WriteLine("      nuke           = yes");
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.WriteLine("      extra{0}         = {1}", division.Extra2 == UnitType.Undefined ? " " : "1",
                    Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.WriteLine("      extra2         = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.WriteLine("      extra3         = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.WriteLine("      extra4         = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.WriteLine("      extra5         = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.WriteLine("      brigade_model{0} = {1}", division.Extra2 == UnitType.Undefined ? " " : "1",
                    division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.WriteLine("      brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.WriteLine("      brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.WriteLine("      brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.WriteLine("      brigade_model5 = {0}", division.BrigadeModel5);
            }
            if (division.Strength > 0)
            {
                writer.WriteLine("      strength       = {0}", DoubleHelper.ToString(division.Strength));
            }
            if (division.MaxStrength > 0)
            {
                writer.WriteLine("      max_strength   = {0}", DoubleHelper.ToString(division.MaxStrength));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("      organisation   = {0}", DoubleHelper.ToString(division.Organisation));
            }
            if (division.Morale > 0)
            {
                writer.WriteLine("      morale         = {0}", DoubleHelper.ToString(division.Morale));
            }
            if (division.Experience > 0)
            {
                writer.WriteLine("      experience     = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Organisation > 0)
            {
                writer.WriteLine("      defaultorganisation = {0}", DoubleHelper.ToString(division.MaxOrganisation));
            }
            if (division.UpgradeProgress > 0)
            {
                writer.WriteLine("      div_upgr_progress = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Supplies > 0)
            {
                writer.WriteLine("      supplies = {0}", DoubleHelper.ToString(division.Supplies));
            }
            if (division.Fuel > 0)
            {
                writer.WriteLine("      oil = {0}", DoubleHelper.ToString(division.Fuel));
            }
            if (division.MaxSupplies > 0)
            {
                writer.WriteLine("      max_supply_stock = {0}", DoubleHelper.ToString(division.MaxSupplies));
            }
            if (division.MaxFuel > 0)
            {
                writer.WriteLine("      max_oil_stock  = {0}", DoubleHelper.ToString(division.MaxFuel));
            }
            if (division.MaxSpeed > 0)
            {
                writer.WriteLine("      maxspeed = {0}", DoubleHelper.ToString(division.MaxSpeed));
            }
            if (division.SupplyConsumption > 0)
            {
                writer.WriteLine("      supplyconsumption = {0}", DoubleHelper.ToString(division.SupplyConsumption));
            }
            if (division.FuelConsumption > 0)
            {
                writer.WriteLine("      fuelconsumption = {0}", DoubleHelper.ToString(division.FuelConsumption));
            }
            if (division.SurfaceDetection > 0)
            {
                writer.WriteLine("      surfacedetectioncapability = {0}",
                    DoubleHelper.ToString(division.SurfaceDetection));
            }
            if (division.AirDetection > 0)
            {
                writer.WriteLine("      airdetectioncapability = {0}", DoubleHelper.ToString(division.AirDetection));
            }
            if (division.SurfaceDefence > 0)
            {
                writer.WriteLine("      surfacedefence = {0}", DoubleHelper.ToString(division.SurfaceDefence));
            }
            if (division.AirDefence > 0)
            {
                writer.WriteLine("      airdefence = {0}", DoubleHelper.ToString(division.AirDefence));
            }
            if (division.AirAttack > 0)
            {
                writer.WriteLine("      airattack = {0}", DoubleHelper.ToString(division.AirAttack));
            }
            if (division.StrategicAttack > 0)
            {
                writer.WriteLine("      strategicattack = {0}", DoubleHelper.ToString(division.StrategicAttack));
            }
            if (division.SoftAttack > 0)
            {
                writer.WriteLine("      softattack = {0}", DoubleHelper.ToString(division.SoftAttack));
            }
            if (division.HardAttack > 0)
            {
                writer.WriteLine("      hardattack = {0}", DoubleHelper.ToString(division.HardAttack));
            }
            if (division.TransportCapability > 0)
            {
                writer.WriteLine("      transportcapability = {0}", DoubleHelper.ToString(division.TransportCapability));
            }
            if (division.NavalAttack > 0)
            {
                writer.WriteLine("      navalattack = {0}", DoubleHelper.ToString(division.NavalAttack));
            }
            if (division.Range > 0)
            {
                writer.WriteLine("      range = {0}", DoubleHelper.ToString(division.Range));
            }
            if (division.Dormant)
            {
                writer.WriteLine("      dormant        = yes");
            }
            writer.WriteLine("    }");
        }

        /// <summary>
        ///     生産中師団リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDivisionDevelopments(CountrySettings settings, TextWriter writer)
        {
            if (settings.DivisionDevelopments.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (DivisionDevelopment division in settings.DivisionDevelopments)
            {
                WriteDivisionDevelopment(division, writer);
            }
        }

        /// <summary>
        ///     生産中師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDivisionDevelopment(DivisionDevelopment division, TextWriter writer)
        {
            writer.WriteLine("  division_development = {");
            writer.Write("    id             = ");
            WriteTypeId(division.Id, writer);
            writer.WriteLine();
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.WriteLine("    name           = \"{0}\"", division.Name);
            }
            writer.WriteLine("    type           = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.WriteLine("    model          = {0}", division.Model);
            }
            if (division.Cost > 0)
            {
                writer.WriteLine("    cost           = {0}", DoubleHelper.ToString1(division.Cost));
            }
            if (!division.UnitCost)
            {
                writer.WriteLine("    unitcost       = no");
            }
            if (!division.NewModel)
            {
                writer.WriteLine("    new_model      = no");
            }
            if (division.Date != null)
            {
                writer.Write("    date           = ");
                WriteDate(division.Date, writer);
                writer.WriteLine();
            }
            if (division.Manpower > 0)
            {
                writer.WriteLine("    manpower       = {0}", DoubleHelper.ToString(division.Manpower));
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                if (division.Halted)
                {
                    writer.WriteLine("    halted         = yes");
                }
                writer.WriteLine("    close_when_finished = {0}", BoolHelper.ToString(division.CloseWhenFinished));
                writer.WriteLine("    waitingforclosure = {0}", BoolHelper.ToString(division.WaitingForClosure));
                if (division.RetoolingTime > 0)
                {
                    writer.WriteLine("    retooling_time = {0}", DoubleHelper.ToString4(division.RetoolingTime));
                }
            }
            if (division.TotalProgress > 0)
            {
                writer.WriteLine("    total_progress = {0}", DoubleHelper.ToString4(division.Progress));
            }
            if (division.Size > 0)
            {
                writer.WriteLine("    size           = {0}", division.Size);
            }
            if (division.Done > 0)
            {
                writer.WriteLine("    done           = {0}", division.Done);
            }
            if (division.Days > 0)
            {
                writer.WriteLine("    days           = {0}", division.Days);
            }
            if (division.DaysForFirst > 0)
            {
                writer.WriteLine("    days_for_first = {0}", division.DaysForFirst);
            }
            if (division.GearingBonus > 0)
            {
                writer.WriteLine("    gearing_bonus  = {0}", DoubleHelper.ToString4(division.GearingBonus));
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.WriteLine("    extra{0}         = {1}",
                    (division.Extra2 == UnitType.Undefined) && (Units.Items[(int) division.Type].Branch != Branch.Navy)
                        ? " "
                        : "1",
                    Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.WriteLine("    extra2         = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.WriteLine("    extra3         = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.WriteLine("    extra4         = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.WriteLine("    extra5         = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.WriteLine("    brigade_model{0} = {1}",
                    (division.Extra2 == UnitType.Undefined) && (Units.Items[(int) division.Type].Branch != Branch.Navy)
                        ? " "
                        : "1",
                    division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.WriteLine("    brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.WriteLine("    brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.WriteLine("    brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.WriteLine("    brigade_model5 = {0}", division.BrigadeModel5);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     休止中陸軍師団リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantLandDivisions(CountrySettings settings, TextWriter writer)
        {
            if (settings.LandDivisions.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Division division in settings.LandDivisions)
            {
                WriteDormantLandDivision(division, writer);
            }
        }

        /// <summary>
        ///     休止中海軍師団リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantNavalDivisions(CountrySettings settings, TextWriter writer)
        {
            if (settings.NavalDivisions.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Division division in settings.NavalDivisions)
            {
                WriteDormantNavalDivision(division, writer);
            }
        }

        /// <summary>
        ///     休止中空軍師団リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantAirDivisions(CountrySettings settings, TextWriter writer)
        {
            if (settings.AirDivisions.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (Division division in settings.AirDivisions)
            {
                WriteDormantAirDivision(division, writer);
            }
        }

        /// <summary>
        ///     休止中陸軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantLandDivision(Division division, TextWriter writer)
        {
            writer.Write("  landdivision = {");
            if (division.Dormant)
            {
                writer.Write(" dormant = yes");
            }
            if (division.Id != null)
            {
                writer.Write(" id = ");
                WriteTypeId(division.Id, writer);
            }
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.Write(" name = \"{0}\" ", division.Name);
            }
            writer.Write(" type = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.Write(" model = {0}", division.Model);
            }
            if (division.Strength > 0)
            {
                writer.Write(" strength = {0}", division.Strength);
            }
            if (division.MaxStrength > 0)
            {
                writer.Write(" max_strength = {0}", division.MaxStrength);
            }
            if (division.Organisation > 0)
            {
                writer.Write(" organisation = {0}", division.Organisation);
            }
            if (division.Morale > 0)
            {
                writer.Write(" morale = {0}", division.Morale);
            }
            if (division.Experience > 0)
            {
                writer.Write(" experience = {0}", division.Experience);
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.Write(" extra{0} = {1}", division.Extra2 == UnitType.Undefined ? "" : "1",
                    Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.Write(" extra2 = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.Write(" extra3 = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.Write(" extra4 = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.Write(" extra5 = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.Write(" brigade_model{0} = {1}", division.Extra2 == UnitType.Undefined ? "" : "1",
                    division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.Write(" brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.Write(" brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.Write(" brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.Write(" brigade_model5 = {0}", division.BrigadeModel5);
            }
            if (division.Locked)
            {
                writer.Write(" locked = yes");
            }
            writer.WriteLine(" } ");
        }

        /// <summary>
        ///     休止中海軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantNavalDivision(Division division, TextWriter writer)
        {
            writer.Write("  navaldivision = {");
            if (division.Dormant)
            {
                writer.Write(" dormant = yes");
            }
            if (division.Id != null)
            {
                writer.Write(" id = ");
                WriteTypeId(division.Id, writer);
            }
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.Write(" name = \"{0}\"", division.Name);
            }
            writer.Write(" type = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.Write(" model = {0}", division.Model);
            }
            if (division.Nuke)
            {
                writer.Write(" nuke = yes");
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.Write(" extra1 = {0}", Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.Write(" extra2 = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.Write(" extra3 = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.Write(" extra4 = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.Write(" extra5 = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.Write(" brigade_model1 = {0}", division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.Write(" brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.Write(" brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.Write(" brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.Write(" brigade_model5 = {0}", division.BrigadeModel5);
            }
            if (division.Strength > 0)
            {
                writer.Write(" strength = {0}", DoubleHelper.ToString(division.Strength));
            }
            if (division.MaxStrength > 0)
            {
                writer.Write(" max_strength = {0}", DoubleHelper.ToString(division.MaxStrength));
            }
            if (division.Organisation > 0)
            {
                writer.Write(" organisation = {0}", DoubleHelper.ToString(division.Organisation));
            }
            if (division.Morale > 0)
            {
                writer.Write(" morale = {0}", DoubleHelper.ToString(division.Morale));
            }
            if (division.Experience > 0)
            {
                writer.Write(" experience = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Organisation > 0)
            {
                writer.Write(" defaultorganisation = {0}", DoubleHelper.ToString(division.MaxOrganisation));
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     休止中空軍師団を書き出す
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDormantAirDivision(Division division, TextWriter writer)
        {
            writer.Write("  airdivision = {");
            if (division.Dormant)
            {
                writer.Write(" dormant = yes");
            }
            if (division.Id != null)
            {
                writer.Write(" id = ");
                WriteTypeId(division.Id, writer);
            }
            if (!string.IsNullOrEmpty(division.Name))
            {
                writer.Write(" name = \"{0}\"", division.Name);
            }
            writer.Write(" type = {0}", Units.Strings[(int) division.Type]);
            if (division.Model >= 0)
            {
                writer.Write(" model = {0}", division.Model);
            }
            if (division.Nuke)
            {
                writer.Write(" nuke = yes");
            }
            if (division.Extra1 != UnitType.Undefined)
            {
                writer.Write(" extra{0} = {1}", division.Extra2 == UnitType.Undefined ? "" : "1",
                    Units.Strings[(int) division.Extra1]);
            }
            if (division.Extra2 != UnitType.Undefined)
            {
                writer.Write(" extra2 = {0}", Units.Strings[(int) division.Extra2]);
            }
            if (division.Extra3 != UnitType.Undefined)
            {
                writer.Write(" extra3 = {0}", Units.Strings[(int) division.Extra3]);
            }
            if (division.Extra4 != UnitType.Undefined)
            {
                writer.Write(" extra4 = {0}", Units.Strings[(int) division.Extra4]);
            }
            if (division.Extra5 != UnitType.Undefined)
            {
                writer.Write(" extra5 = {0}", Units.Strings[(int) division.Extra5]);
            }
            if (division.Extra1 != UnitType.Undefined && division.BrigadeModel1 >= 0)
            {
                writer.Write(" brigade_model{0} = {1}", division.Extra2 == UnitType.Undefined ? "" : "1",
                    division.BrigadeModel1);
            }
            if (division.Extra2 != UnitType.Undefined && division.BrigadeModel2 >= 0)
            {
                writer.Write(" brigade_model2 = {0}", division.BrigadeModel2);
            }
            if (division.Extra3 != UnitType.Undefined && division.BrigadeModel3 >= 0)
            {
                writer.Write(" brigade_model3 = {0}", division.BrigadeModel3);
            }
            if (division.Extra4 != UnitType.Undefined && division.BrigadeModel4 >= 0)
            {
                writer.Write(" brigade_model4 = {0}", division.BrigadeModel4);
            }
            if (division.Extra5 != UnitType.Undefined && division.BrigadeModel5 >= 0)
            {
                writer.Write(" brigade_model5 = {0}", division.BrigadeModel5);
            }
            if (division.Strength > 0)
            {
                writer.Write(" strength = {0}", DoubleHelper.ToString(division.Strength));
            }
            if (division.MaxStrength > 0)
            {
                writer.Write(" max_strength = {0}", DoubleHelper.ToString(division.MaxStrength));
            }
            if (division.Organisation > 0)
            {
                writer.Write(" organisation = {0}", DoubleHelper.ToString(division.Organisation));
            }
            if (division.Morale > 0)
            {
                writer.Write(" morale = {0}", DoubleHelper.ToString(division.Morale));
            }
            if (division.Experience > 0)
            {
                writer.Write(" experience = {0}", DoubleHelper.ToString(division.Experience));
            }
            if (division.Organisation > 0)
            {
                writer.Write(" defaultorganisation = {0}", DoubleHelper.ToString(division.MaxOrganisation));
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     生産可能師団を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAllowedDivisions(CountrySettings settings, TextWriter writer)
        {
            if (settings.AllowedDivisions.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            writer.WriteLine("  allowed_divisions = {");
            foreach (KeyValuePair<UnitType, bool> pair in settings.AllowedDivisions)
            {
                WriteUnitBoolPair(pair.Key, pair.Value, writer);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     生産可能旅団を書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAllowedBrigades(CountrySettings settings, TextWriter writer)
        {
            if (settings.AllowedBrigades.Count == 0)
            {
                return;
            }
            writer.WriteLine("  allowed_brigades = {");
            foreach (KeyValuePair<UnitType, bool> pair in settings.AllowedBrigades)
            {
                WriteUnitBoolPair(pair.Key, pair.Value, writer);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     ユニット種類と論理値の組を出力する
        /// </summary>
        /// <param name="type">ユニット種類</param>
        /// <param name="b">論理値</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteUnitBoolPair(UnitType type, bool b, TextWriter writer)
        {
            writer.WriteLine("    {0} = {1}", Units.Strings[(int) type], b ? "yes" : "no");
        }

        #endregion

        #region 任務

        /// <summary>
        ///     陸軍任務を書き出す
        /// </summary>
        /// <param name="mission">任務</param>
        /// <param name="writer">ファイル書き込み用</param>
        /// <param name="indent">インデント文字列</param>
        private static void WriteLandMission(Mission mission, TextWriter writer, string indent)
        {
            writer.WriteLine("{0}mission = {{", indent);
            if (mission.Type != MissionType.None)
            {
                writer.WriteLine("{0}  type = {1}", indent, Scenarios.MissionStrings[(int) mission.Type]);
            }
            if (mission.Target > 0)
            {
                writer.WriteLine("{0}  target = {1}", indent, mission.Target);
            }
            writer.WriteLine("{0}  percentage = {1}", indent, DoubleHelper.ToString(mission.Percentage));
            if (mission.Night)
            {
                writer.WriteLine("{0}  night = yes", indent);
            }
            if (mission.Day)
            {
                writer.WriteLine("{0}  day = yes", indent);
            }
            if (mission.StartDate != null)
            {
                writer.Write("{0}  startdate = ", indent);
                WriteDate(mission.StartDate, writer);
                writer.WriteLine();
            }
            if (mission.EndDate != null)
            {
                writer.Write("{0}  enddate = ", indent);
                WriteDate(mission.EndDate, writer);
                writer.WriteLine();
            }
            if (mission.Task > 0)
            {
                writer.WriteLine("{0}  task = {1}", indent, mission.Task);
            }
            if (mission.Location > 0)
            {
                writer.WriteLine("{0}  location = {1}", indent, mission.Location);
            }
            writer.WriteLine("{0}}}", indent);
        }

        /// <summary>
        ///     海軍任務を書き出す
        /// </summary>
        /// <param name="mission">任務</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalMission(Mission mission, TextWriter writer)
        {
            writer.WriteLine("    mission = {");
            if (mission.Type != MissionType.None)
            {
                writer.WriteLine("      type = {0}", Scenarios.MissionStrings[(int) mission.Type]);
            }
            if (mission.Target > 0)
            {
                writer.WriteLine("      target = {0}", mission.Target);
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                writer.WriteLine("      missionscope = {0}", mission.MissionScope);
            }
            writer.WriteLine("      percentage = {0}", DoubleHelper.ToString(mission.Percentage));
            if (mission.Night)
            {
                writer.WriteLine("      night = yes");
            }
            if (mission.Day)
            {
                writer.WriteLine("      day = yes");
            }
            if (Game.Type == GameType.DarkestHour)
            {
                if (mission.TargetZone > 0)
                {
                    writer.WriteLine("      tz = {0}", mission.TargetZone);
                }
                if (mission.AttackConvoy)
                {
                    writer.WriteLine("      ac = yes");
                }
                if (!DoubleHelper.IsEqual(mission.OrgLimit, mission.Percentage))
                {
                    writer.WriteLine("      org = {0}", DoubleHelper.ToString(mission.OrgLimit));
                }
            }
            if (mission.StartDate != null)
            {
                writer.Write("      startdate = ");
                WriteDate(mission.StartDate, writer);
                writer.WriteLine();
            }
            if (mission.EndDate != null)
            {
                writer.Write("      enddate = ");
                WriteDate(mission.EndDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    }");
        }

        /// <summary>
        ///     空軍任務を書き出す
        /// </summary>
        /// <param name="mission">任務</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirMission(Mission mission, TextWriter writer)
        {
            writer.WriteLine("    mission = {");
            if (mission.Type != MissionType.None)
            {
                writer.WriteLine("      type = {0}", Scenarios.MissionStrings[(int) mission.Type]);
            }
            if (mission.Target > 0)
            {
                writer.WriteLine("      target = {0}", mission.Target);
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                writer.WriteLine("      missionscope = {0}", mission.MissionScope);
            }
            writer.WriteLine("      percentage = {0}", DoubleHelper.ToString(mission.Percentage));
            if (mission.Night)
            {
                writer.WriteLine("      night = yes");
            }
            if (mission.Day)
            {
                writer.WriteLine("      day = yes");
            }
            if (Game.Type == GameType.DarkestHour)
            {
                if (mission.TargetZone > 0)
                {
                    writer.WriteLine("      tz = {0}", mission.TargetZone);
                }
                if (!DoubleHelper.IsEqual(mission.OrgLimit, mission.Percentage))
                {
                    writer.WriteLine("      org = {0}", DoubleHelper.ToString(mission.OrgLimit));
                }
            }
            if (mission.StartDate != null)
            {
                writer.Write("      startdate = ");
                WriteDate(mission.StartDate, writer);
                writer.WriteLine();
            }
            if (mission.EndDate != null)
            {
                writer.Write("      enddate = ");
                WriteDate(mission.EndDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    }");
        }

        #endregion

        #region 輸送船団

        /// <summary>
        ///     生産中輸送船団リストを書き出す
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteConvoyDevelopments(CountrySettings settings, TextWriter writer)
        {
            if (settings.ConvoyDevelopments.Count == 0)
            {
                return;
            }
            writer.WriteLine();
            foreach (ConvoyDevelopment convoy in settings.ConvoyDevelopments)
            {
                WriteConvoyDevelopment(convoy, writer);
            }
        }

        /// <summary>
        ///     生産中輸送船団を書き出す
        /// </summary>
        /// <param name="convoy">生産中輸送船団</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteConvoyDevelopment(ConvoyDevelopment convoy, TextWriter writer)
        {
            writer.WriteLine("  convoy_development = {");
            if (convoy.Id != null)
            {
                writer.Write("    id             = ");
                WriteTypeId(convoy.Id, writer);
                writer.WriteLine();
            }
            if (convoy.Name != null)
            {
                writer.WriteLine("    name           = \"{0}\"", convoy.Name);
            }
            if (convoy.Progress > 0)
            {
                writer.WriteLine("    progress       = {0}", DoubleHelper.ToString4(convoy.Progress));
            }
            if (convoy.Location > 0)
            {
                writer.WriteLine("    location       = {0}", convoy.Location);
            }
            if (convoy.Cost > 0)
            {
                writer.WriteLine("    cost           = {0}", DoubleHelper.ToString4(convoy.Cost));
            }
            if (convoy.Date != null)
            {
                writer.Write("    date           = ");
                WriteDate(convoy.Date, writer);
                writer.WriteLine();
            }
            if (convoy.Manpower > 0)
            {
                writer.WriteLine("    manpower       = {0}", DoubleHelper.ToString4(convoy.Manpower));
            }
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                if (convoy.Halted)
                {
                    writer.WriteLine("    halted         = yes");
                }
                writer.WriteLine("    close_when_finished = {0}", BoolHelper.ToString(convoy.CloseWhenFinished));
                writer.WriteLine("    waitingforclosure = {0}", BoolHelper.ToString(convoy.WaitingForClosure));
                if (convoy.RetoolingTime > 0)
                {
                    writer.WriteLine("    retooling_time = {0}", DoubleHelper.ToString4(convoy.RetoolingTime));
                }
            }
            if (convoy.TotalProgress > 0)
            {
                writer.WriteLine("    total_progress = {0}", DoubleHelper.ToString4(convoy.Progress));
            }
            if (convoy.Size > 0)
            {
                writer.WriteLine("    size           = {0}", convoy.Size);
            }
            if (convoy.Done > 0)
            {
                writer.WriteLine("    done           = {0}", convoy.Done);
            }
            if (convoy.Days > 0)
            {
                writer.WriteLine("    days           = {0}", convoy.Days);
            }
            if (convoy.DaysForFirst > 0)
            {
                writer.WriteLine("    days_for_first = {0}", convoy.DaysForFirst);
            }
            if (convoy.GearingBonus > 0)
            {
                writer.WriteLine("    gearing_bonus  = {0}", DoubleHelper.ToString4(convoy.GearingBonus));
            }
            writer.WriteLine("    type           = {0}", Scenarios.ConvoyStrings[(int) convoy.Type]);
            writer.WriteLine("  }");
        }

        #endregion

        #region 汎用

        /// <summary>
        ///     国家リストを書き出す
        /// </summary>
        /// <param name="countries">国家リスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryList(IEnumerable<Country> countries, TextWriter writer)
        {
            foreach (Country country in countries)
            {
                writer.Write(" {0}", Countries.Strings[(int) country]);
            }
        }

        /// <summary>
        ///     IDリストを書き出す
        /// </summary>
        /// <param name="ids">IDリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteIdList(IEnumerable<int> ids, TextWriter writer)
        {
            foreach (int id in ids)
            {
                writer.Write(" {0}", id);
            }
        }

        /// <summary>
        ///     建物のサイズを書き出す
        /// </summary>
        /// <param name="building">建物のサイズ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBuilding(BuildingSize building, TextWriter writer)
        {
            if (DoubleHelper.IsZero(building.Size))
            {
                writer.Write("{{ size = {0} current_size = {1} }}", ObjectHelper.ToString(building.MaxSize),
                    ObjectHelper.ToString(building.CurrentSize));
            }
            else
            {
                writer.Write(ObjectHelper.ToString(building.Size));
            }
        }

        /// <summary>
        ///     日時を書き出す
        /// </summary>
        /// <param name="date">日時</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDate(GameDate date, TextWriter writer)
        {
            if (date.Hour == 0)
            {
                writer.Write("{{ year = {0} month = {1} day = {2} }}", date.Year, Scenarios.MonthStrings[date.Month - 1],
                    date.Day - 1);
            }
            else
            {
                writer.Write("{{ year = {0} month = {1} day = {2} hour = {3} }}", date.Year,
                    Scenarios.MonthStrings[date.Month - 1], date.Day - 1, date.Hour);
            }
        }

        /// <summary>
        ///     typeとidの組を書き出す
        /// </summary>
        /// <param name="id">typeとidの組</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTypeId(TypeId id, TextWriter writer)
        {
            writer.Write("{{ type = {0} id = {1} }}", id.Type, id.Id);
        }

        #endregion
    }
}