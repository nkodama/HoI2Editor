using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Writers
{
    public static class ScenarioWriter
    {
        #region シナリオデータ

        /// <summary>
        /// シナリオデータをファイルへ書き込む
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
                writer.WriteLine("# ###################");
                foreach (string name in scenario.EventFiles)
                {
                    writer.WriteLine("event      = \"{0}\"", name);
                }
                writer.WriteLine();
                foreach (string name in scenario.IncludeFiles)
                {
                    writer.WriteLine("include = \"{0}\"", name);
                }
            }
        }

        #endregion

        #region シナリオヘッダ

        /// <summary>
        /// シナリオヘッダを書き出す
        /// </summary>
        /// <param name="header">シナリオヘッダ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteHeader(ScenarioHeader header, TextWriter writer)
        {
            writer.WriteLine("header =");
            writer.WriteLine("{{ name       = \"{0}\"", header.Name);
            if (header.StartDate != null)
            {
                writer.Write("  startdate  = {{ year = {0} }}", header.StartDate.Year);
            }
            writer.WriteLine("  selectable = {");
            WriteCountryList(header.SelectableCountries, writer);
            writer.WriteLine("   ");
            writer.WriteLine("               }");
            foreach (MajorCountrySettings major in header.MajorCountries)
            {
                WriteMajorCountry(major, writer);
            }
            writer.WriteLine("}");
        }

        /// <summary>
        /// 主要国設定を書き出す
        /// </summary>
        /// <param name="major">主要国設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteMajorCountry(MajorCountrySettings major, TextWriter writer)
        {
            writer.Write("  {0}        = {{", Countries.Strings[(int)major.Country]);
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
            writer.WriteLine(" }");
        }

        #endregion

        #region シナリオグローバルデータ

        /// <summary>
        /// シナリオグローバルデータを書き出す
        /// </summary>
        /// <param name="data">シナリオグローバルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteGlobalData(ScenarioGlobalData data, TextWriter writer)
        {
            writer.WriteLine("globaldata =");
            writer.Write("{");
            if (data.StartDate != null)
            {
                writer.Write(" startdate = ");
                WriteDate(data.StartDate, writer);
            }
            writer.WriteLine();
            if (data.Axis != null)
            {
                WriteAlliance(data.Axis, "axis", writer);
            }
            if (data.Allies != null)
            {
                WriteAlliance(data.Axis, "allies", writer);
            }
            if (data.Comintern != null)
            {
                WriteAlliance(data.Axis, "comintern", writer);
            }
            foreach (Alliance alliance in data.Alliances)
            {
                WriteAlliance(alliance, "alliance", writer);
            }
            foreach (War war in data.Wars)
            {
                WriteWar(war, writer);
            }
            foreach (Treaty treaty in data.NonAggressions)
            {
                WriteTreaty(treaty, writer);
            }
            foreach (Treaty treaty in data.Peaces)
            {
                WriteTreaty(treaty, writer);
            }
            foreach (Treaty treaty in data.Trades)
            {
                WriteTreaty(treaty, writer);
            }
            if (data.EndDate != null)
            {
                writer.Write("  enddate   = ");
                WriteDate(data.EndDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("}");
        }

        /// <summary>
        /// 同盟情報を書き出す
        /// </summary>
        /// <param name="alliance">同盟情報</param>
        /// <param name="type">同盟の種類</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAlliance(Alliance alliance, string type, TextWriter writer)
        {
            writer.WriteLine("  {0} =", type);
            writer.Write("  {");
            if (alliance.Id != null)
            {
                writer.Write(" id        = ");
                WriteTypeId(alliance.Id, writer);
            }
            writer.WriteLine();
            writer.Write("    participant = {");
            WriteCountryList(alliance.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("  }");
        }

        /// <summary>
        /// 戦争情報を書き出す
        /// </summary>
        /// <param name="war">戦争情報</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteWar(War war, TextWriter writer)
        {
            writer.WriteLine("  war =");
            writer.Write("  {");
            if (war.Id != null)
            {
                writer.Write(" id        = ");
                WriteTypeId(war.Id, writer);
            }
            writer.WriteLine();
            if (war.StartDate != null)
            {
                writer.Write("    date      = ");
                WriteDate(war.StartDate, writer);
                writer.WriteLine();
            }
            if (war.EndDate != null)
            {
                writer.Write("    enddate   = ");
                WriteDate(war.EndDate, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    attackers =");
            writer.Write("    {");
            if (war.Attackers.Id != null)
            {
                writer.Write(" id          = ");
                WriteTypeId(war.Attackers.Id, writer);
            }
            writer.WriteLine();
            writer.Write("      participant = {");
            WriteCountryList(war.Attackers.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("    }");
            writer.WriteLine("    defenders =");
            writer.Write("    {");
            if (war.Defenders.Id != null)
            {
                writer.Write(" id          = ");
                WriteTypeId(war.Defenders.Id, writer);
            }
            writer.WriteLine();
            writer.Write("      participant = {");
            WriteCountryList(war.Defenders.Participant, writer);
            writer.WriteLine(" }");
            writer.WriteLine("    }");
            writer.WriteLine("  }");
        }

        /// <summary>
        /// 外交協定情報を書き出す
        /// </summary>
        /// <param name="treaty">外交協定情報</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTreaty(Treaty treaty, TextWriter writer)
        {
            writer.Write("  treaty =");
            writer.Write("  {");
            if (treaty.Id != null)
            {
                writer.Write(" id        = ");
                WriteTypeId(treaty.Id, writer);
            }
            writer.WriteLine();
            writer.WriteLine("    type       = {0}", Scenarios.TreatyStrings[(int) treaty.Type]);
            writer.WriteLine("    country    = \"{0}\"", treaty.Country1);
            writer.WriteLine("    country    = \"{0}\"", treaty.Country2);
            if (treaty.StartDate != null)
            {
                writer.Write("    startdate  = ");
                WriteDate(treaty.StartDate, writer);
                writer.WriteLine();
            }
            if (treaty.EndDate != null)
            {
                writer.Write("    expirydate = ");
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

        #endregion

        #region プロヴィンス設定

        /// <summary>
        /// プロヴィンス設定をbases.incに書き込む
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteBasesInc(List<ProvinceSettings> provinces, string fileName)
        {
            // bases.incに保存しないならば何もしない
            if (Scenarios.Type == ScenarioType.BattleScenario ||
                Scenarios.Type == ScenarioType.SaveGame)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                if (Scenarios.Type == ScenarioType.HoI2)
                {
                    WriteBasesIncHoI2(provinces, writer);
                }
                else
                {
                    WriteBasesIncDh(provinces, writer);
                }
            }
        }

        /// <summary>
        /// プロヴィンス設定をbases_DOD.incに書き込む
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteBasesDodInc(List<ProvinceSettings> provinces, string fileName)
        {
            // bases_DOD.incに保存しないならば何もしない
            if (Scenarios.Type != ScenarioType.Full33)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                foreach (ProvinceSettings settings in provinces.Where(ExistsDataBasesDodInc))
                {
                    writer.Write(" province = {{ id = {0}", settings.Id);
                    if (settings.Ic != null)
                    {
                        writer.Write(" ");
                        WriteBuilding(BuildingType.Ic, settings.Ic, writer);
                    }
                    if (settings.Infrastructure != null)
                    {
                        writer.Write(" ");
                        WriteBuilding(BuildingType.Infrastructure, settings.Infrastructure, writer);
                    }
                    writer.WriteLine(" } ");
                }
            }
        }

        /// <summary>
        /// プロヴィンス設定をvp.incに書き込む
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteVpInc(List<ProvinceSettings> provinces, string fileName)
        {
            // vp.incに保存しないならば何もしない
            if (Scenarios.Type == ScenarioType.SaveGame)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                if (Scenarios.Type == ScenarioType.HoI2)
                {
                    WriteVpIncHoI2(provinces, writer);
                }
                else
                {
                    WriteVpIncDh(provinces, writer);
                }
            }
        }

        /// <summary>
        /// プロヴィンス設定をbases.incに書き込む (HoI2/AoD)
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBasesIncHoI2(List<ProvinceSettings> provinces, TextWriter writer)
        {
            writer.WriteLine("#naval");
            writer.WriteLine();
            foreach (ProvinceSettings settings in provinces.Where(settings => settings.NavalBase != null))
            {
                writer.Write("province = {{ id =\t{0}\t", settings.Id);
                WriteBuilding(BuildingType.NavalBase, settings.NavalBase, writer);
                writer.WriteLine(" }");
            }
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("###airbases");
            writer.WriteLine();
            writer.WriteLine();
            foreach (ProvinceSettings settings in provinces.Where(settings => settings.AirBase != null))
            {
                writer.Write("province = {{ id =\t{0}\t", settings.Id);
                WriteBuilding(BuildingType.AirBase, settings.AirBase, writer);
                writer.WriteLine(" }");
            }
            writer.WriteLine();
        }

        /// <summary>
        /// プロヴィンス設定をbases.incに書き込む (DH)
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBasesIncDh(IEnumerable<ProvinceSettings> provinces, TextWriter writer)
        {
            foreach (ProvinceSettings settings in provinces.Where(ExistsDataBasesIncDh))
            {
                Province province = Provinces.Items.Find(p => p.Id == settings.Id);
                writer.Write(" province = {{ id = {0}   #  {1} - {2} - {3}", settings.Id,
                    Scenarios.GetProvinceName(province, settings), Provinces.GetAreaName(province.Area),
                    Provinces.GetRegionName(province.Region));
                if (Scenarios.Type != ScenarioType.Full33)
                {
                    if (settings.Ic != null)
                    {
                        writer.Write("   ");
                        WriteBuilding(BuildingType.Ic, settings.Ic, writer);
                        writer.WriteLine(" ");
                    }
                    if (settings.Infrastructure != null)
                    {
                        writer.Write("   ");
                        WriteBuilding(BuildingType.Infrastructure, settings.Infrastructure, writer);
                        writer.WriteLine(" ");
                    }
                }
                if (settings.LandFort != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.LandFort, settings.LandFort, writer);
                    writer.WriteLine(" ");
                }
                if (settings.CoastalFort != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.CoastalFort, settings.CoastalFort, writer);
                    writer.WriteLine(" ");
                }
                if (settings.AntiAir != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.AntiAir, settings.AntiAir, writer);
                    writer.WriteLine(" ");
                }
                if (settings.AirBase != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.AirBase, settings.AirBase, writer);
                    writer.WriteLine(" ");
                }
                if (settings.NavalBase != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.NavalBase, settings.NavalBase, writer);
                    writer.WriteLine(" ");
                }
                if (settings.RadarStation != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.RadarStation, settings.RadarStation, writer);
                    writer.WriteLine(" ");
                }
                if (settings.NuclearReactor != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.NuclearReactor, settings.NuclearReactor, writer);
                    writer.WriteLine(" ");
                }
                if (settings.RocketTest != null)
                {
                    writer.Write("   ");
                    WriteBuilding(BuildingType.RocketTest, settings.RocketTest, writer);
                    writer.WriteLine(" ");
                }
                writer.WriteLine(" } ");
                writer.WriteLine(" ");
            }
        }

        /// <summary>
        /// プロヴィンス設定をvp.incに書き込む (HoI2/AoD)
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteVpIncHoI2(IEnumerable<ProvinceSettings> provinces, TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("###############################");
            writer.WriteLine("# Victory points distribution #");
            writer.WriteLine("###############################");
            writer.WriteLine();
            foreach (ProvinceSettings settings in provinces.Where(settings => settings.Vp != 0))
            {
                Province province = Provinces.Items.Find(p => p.Id == settings.Id);
                writer.WriteLine("province = {{ id = {0} points = {1} }} # {2}", settings.Id, settings.Vp,
                    Scenarios.GetProvinceName(province, settings));
            }
        }

        /// <summary>
        /// プロヴィンス設定をvp.incに書き込む (DH)
        /// </summary>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteVpIncDh(IEnumerable<ProvinceSettings> provinces, TextWriter writer)
        {
            foreach (ProvinceSettings settings in provinces.Where(settings => settings.Vp != 0))
            {
                writer.WriteLine(" ");
                writer.WriteLine("###############################");
                writer.WriteLine("# Victory points distribution #");
                writer.WriteLine("###############################");
                writer.WriteLine("  ");
                Province province = Provinces.Items.Find(p => p.Id == settings.Id);
                writer.Write(" province = {{  id = {0} points = {1} }} # {2} ", settings.Id, settings.Vp,
                    Scenarios.GetProvinceName(province, settings));
            }
        }

        /// <summary>
        /// プロヴィンス設定を国別incに書き込む (国別inc)
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryProvince(ProvinceSettings settings, TextWriter writer)
        {
            Province province = Provinces.Items.Find(p => p.Id == settings.Id);
            if (IsSingleLineProvince(settings))
            {
                WriteSingleLineProvince(settings, writer);
                writer.WriteLine(" # {0}", Scenarios.GetProvinceName(province, settings));
            }
            else
            {
                writer.WriteLine("province =");
                writer.WriteLine("{{ id         = {0}", settings.Id);
            }
        }

        /// <summary>
        /// 単一行のプロヴィンス設定を書き出す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSingleLineProvince(ProvinceSettings settings, TextWriter writer)
        {
            writer.Write("province = {{ id = {0}", settings.Id);
            WriteBuilding(BuildingType.Ic, settings.Ic, writer);
            WriteBuilding(BuildingType.Infrastructure, settings.Infrastructure, writer);
            WriteBuilding(BuildingType.LandFort, settings.LandFort, writer);
            WriteBuilding(BuildingType.CoastalFort, settings.CoastalFort, writer);
            WriteBuilding(BuildingType.AntiAir, settings.AntiAir, writer);
            WriteBuilding(BuildingType.AirBase, settings.AirBase, writer);
            WriteBuilding(BuildingType.NavalBase, settings.NavalBase, writer);
            WriteBuilding(BuildingType.RadarStation, settings.RadarStation, writer);
            WriteBuilding(BuildingType.NuclearReactor, settings.NuclearReactor, writer);
            WriteBuilding(BuildingType.RocketTest, settings.RocketTest, writer);
            WriteBuilding(BuildingType.SyntheticOil, settings.SyntheticOil, writer);
            WriteBuilding(BuildingType.SyntheticRares, settings.SyntheticRares, writer);
            WriteBuilding(BuildingType.NuclearPower, settings.NuclearPower, writer);
            writer.Write(" }");
        }

        /// <summary>
        /// 複数行のプロヴィンス設定を書き出す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WritMultiLineProvince(ProvinceSettings settings, TextWriter writer)
        {
            writer.WriteLine("province =");
            writer.WriteLine("{{ id         = {0}", settings.Id);
            WriteBuilding(BuildingType.Ic, settings.Ic, writer);
            WriteBuilding(BuildingType.Infrastructure, settings.Infrastructure, writer);
            WriteBuilding(BuildingType.LandFort, settings.LandFort, writer);
            WriteBuilding(BuildingType.CoastalFort, settings.CoastalFort, writer);
            WriteBuilding(BuildingType.AntiAir, settings.AntiAir, writer);
            WriteBuilding(BuildingType.AirBase, settings.AirBase, writer);
            WriteBuilding(BuildingType.NavalBase, settings.NavalBase, writer);
            WriteBuilding(BuildingType.RadarStation, settings.RadarStation, writer);
            WriteBuilding(BuildingType.NuclearReactor, settings.NuclearReactor, writer);
            WriteBuilding(BuildingType.RocketTest, settings.RocketTest, writer);
            WriteBuilding(BuildingType.SyntheticOil, settings.SyntheticOil, writer);
            WriteBuilding(BuildingType.SyntheticRares, settings.SyntheticRares, writer);
            WriteBuilding(BuildingType.NuclearPower, settings.NuclearPower, writer);
            writer.Write(" }");
        }

        /// <summary>
        /// プロヴィンス設定が単一行で記載できるかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>単一行で記載できればtrueを返す</returns>
        private static bool IsSingleLineProvince(ProvinceSettings settings)
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

            return true;
        }

        /// <summary>
        /// bases.incに保存するデータが存在するかどうかを返す (DH)
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>bases.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsDataBasesIncDh(ProvinceSettings settings)
        {
            if (Scenarios.Type != ScenarioType.Full33)
            {
                if (settings.Ic != null || settings.Infrastructure != null)
                {
                    return true;
                }
            }

            if ((settings.LandFort != null) ||
                (settings.CoastalFort != null) ||
                (settings.AntiAir != null) ||
                (settings.AirBase != null) ||
                (settings.NavalBase != null) ||
                (settings.RadarStation != null) ||
                (settings.NuclearReactor != null) ||
                (settings.RocketTest != null) ||
                (settings.SyntheticOil != null) ||
                (settings.SyntheticRares != null) ||
                (settings.NuclearPower != null))
            {
                return true;
            }

            if (!DoubleHelper.IsZero(settings.RevoltRisk) ||
                !DoubleHelper.IsZero(settings.Manpower) ||
                !DoubleHelper.IsZero(settings.MaxManpower) ||
                !DoubleHelper.IsZero(settings.EnergyPool) ||
                !DoubleHelper.IsZero(settings.Energy) ||
                !DoubleHelper.IsZero(settings.MaxEnergy) ||
                !DoubleHelper.IsZero(settings.MetalPool) ||
                !DoubleHelper.IsZero(settings.Metal) ||
                !DoubleHelper.IsZero(settings.MaxMetal) ||
                !DoubleHelper.IsZero(settings.RareMaterialsPool) ||
                !DoubleHelper.IsZero(settings.RareMaterials) ||
                !DoubleHelper.IsZero(settings.MaxRareMaterials) ||
                !DoubleHelper.IsZero(settings.OilPool) ||
                !DoubleHelper.IsZero(settings.Oil) ||
                !DoubleHelper.IsZero(settings.MaxOil) ||
                !DoubleHelper.IsZero(settings.SupplyPool))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(settings.Name))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// bases_DOD.incに保存するデータが存在するかどうかを返す
        /// </summary>
        /// <param name="settings">プロヴィンス設定</param>
        /// <returns>bases_DOD.incに保存するデータが存在すればtrueを返す</returns>
        private static bool ExistsDataBasesDodInc(ProvinceSettings settings)
        {
            if (settings.Ic != null ||
                settings.Infrastructure != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 国家設定

        /// <summary>
        /// 国家設定をファイルへ書き込む
        /// </summary>
        /// <param name="settings">国家設定</param>
        /// <param name="scenario">シナリオデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void WriteCountrySettings(CountrySettings settings, Scenario scenario, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                writer.WriteLine();
                writer.WriteLine("##############################");
                writer.WriteLine("# Country definition for {0} #", Countries.Strings[(int) settings.Country]);
                writer.WriteLine("##############################");
                writer.WriteLine();
                WriteCountryProvinces(settings, scenario.Provinces, writer);
                writer.WriteLine("{{ tag                    = {0}", Countries.Strings[(int)settings.Country]);
                if (settings.Master != Country.None)
                {
                    writer.WriteLine("  puppet                 = {0}", Countries.Strings[(int)settings.Master]);
                }
                if (settings.Control != Country.None)
                {
                    writer.WriteLine("  control                = {0}", Countries.Strings[(int) settings.Control]);
                }
                writer.WriteLine("  # Resource Reserves");
                writer.WriteLine("  energy                 = {0}", ObjectHelper.ToString(settings.Energy));
                writer.WriteLine("  metal                  = {0}", ObjectHelper.ToString(settings.Metal));
                writer.WriteLine("  rare_materials         = {0}", ObjectHelper.ToString(settings.RareMaterials));
                writer.WriteLine("  oil                    = {0}", ObjectHelper.ToString(settings.Oil));
                writer.WriteLine("  supplies               = {0}", ObjectHelper.ToString(settings.Supplies));
                writer.WriteLine("  money                  = {0}", ObjectHelper.ToString(settings.Money));
                writer.WriteLine("  manpower               = {0}", ObjectHelper.ToString(settings.Manpower));
                writer.WriteLine("  capital                = {0}", settings.Capital);
                if (settings.Policy != null)
                {
                    WritePolicy(settings.Policy, writer);
                }
                writer.Write("  nationalprovinces      = {");
                WriteIdList(settings.NationalProvinces, writer);
                writer.WriteLine(" }");
                writer.Write("  ownedprovinces         = {");
                WriteIdList(settings.OwnedProvinces, writer);
                writer.WriteLine(" }");
                writer.Write("  controlledprovinces    = {");
                WriteIdList(settings.ControlledProvinces, writer);
                writer.WriteLine(" }");
                writer.Write("  techapps               = {");
                WriteIdList(settings.TechApps, writer);
                writer.WriteLine(" }");
                if (settings.Relations.Count > 0)
                {
                    
                }
            }
        }

        /// <summary>
        /// 支配プロヴィンス設定のリストを書き出す
        /// </summary>
        /// <param name="controlled">支配プロヴィンスのIDリスト</param>
        /// <param name="provinces">プロヴィンス設定のリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteCountryProvinces(ICollection<int> controlled, IEnumerable<ProvinceSettings> provinces,
            TextWriter writer)
        {
            foreach (ProvinceSettings settings in provinces.Where(p => controlled.Contains(p.Id)))
            {
                WriteCountryProvince(settings, writer);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 政策スライダーを書き出す
        /// </summary>
        /// <param name="policy">政策スライダー</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WritePolicy(CountryPolicy policy, TextWriter writer)
        {
            writer.WriteLine("  policy =");
            writer.Write("  {");
            if (policy.Date != null)
            {
                writer.Write(" date              = ");
                WriteDate(policy.Date, writer);
                writer.WriteLine();
            }
            writer.WriteLine("    democratic        = {0}", policy.Democratic);
            writer.WriteLine("    political_left    = {0}", policy.PoliticalLeft);
            writer.WriteLine("    free_market       = {0}", policy.FreeMarket);
            writer.WriteLine("    freedom           = {0}", policy.Freedom);
            writer.WriteLine("    professional_army = {0}", policy.ProfessionalArmy);
            writer.WriteLine("    defense_lobby     = {0}", policy.DefenseLobby);
            writer.WriteLine("    interventionism   = {0}", policy.Interventionism);
            writer.WriteLine("  }");
        }

        /// <summary>
        /// 外交設定を書き出す
        /// </summary>
        /// <param name="relations">国家関係リスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDiplomacy(List<Relation> relations, TextWriter writer)
        {
            writer.WriteLine("  diplomacy =");
            bool first = true;
            foreach (Relation relation in relations)
            {
                if (first)
                {
                    writer.Write("  { ");
                    first = false;
                }
                else
                {
                    writer.Write("    ");
                }
                WriteRelation(relation, writer);
            }
        }

        /// <summary>
        /// 国家関係を書き出す
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="writer"></param>
        private static void WriteRelation(Relation relation, TextWriter writer)
        {
            if (relation.Guaranteed == null)
            {
                writer.Write("relation = {{ tag = {0} value = {1}", Countries.Strings[(int)relation.Country],
                    ObjectHelper.ToString(relation.Value));
                if (relation.Access)
                {
                    writer.Write(" access = yes");
                }
            }
        }

        #endregion

        #region 汎用

        /// <summary>
        /// 国家リストを書き出す
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
        /// IDリストを書き出す
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
        /// 建物のサイズを書き出す
        /// </summary>
        /// <param name="type">建物の種類</param>
        /// <param name="building">建物のサイズ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBuilding(BuildingType type, BuildingSize building, TextWriter writer)
        {
            // サイズ指定がなければ何もしない
            if (building == null)
            {
                return;
            }

            writer.Write(" {0} = ", Scenarios.BuildingStrings[(int)type]);
            if (DoubleHelper.IsZero(building.Size))
            {
                writer.Write("{{ size = {0} current_size = {1} }}",
                    building.MaxSize, building.CurrentSize);
            }
            else
            {
                writer.Write("{0}", building.Size);
            }
        }

        /// <summary>
        /// 日時を書き出す
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
        /// typeとidの組を書き出す
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
