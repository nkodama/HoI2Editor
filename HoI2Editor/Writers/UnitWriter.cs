using System;
using System.IO;
using System.Text;
using HoI2Editor.Models;

namespace HoI2Editor.Writers
{
    /// <summary>
    ///     ユニットデータの書き込みを担当するクラス
    /// </summary>
    public static class UnitWriter
    {
        /// <summary>
        ///     ユニットデータをファイルへ書き込む
        /// </summary>
        /// <param name="unit">ユニットデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void Write(Unit unit, string fileName)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // unit_type
                if (Game.Type == GameType.ArsenalOfDemocracy)
                {
                    switch (unit.Branch)
                    {
                        case UnitBranch.Army:
                            writer.WriteLine("land_unit_type = 1");
                            break;

                        case UnitBranch.Navy:
                            writer.WriteLine("naval_unit_type = 1");
                            break;

                        case UnitBranch.AirForce:
                            writer.WriteLine("air_unit_type = 1");
                            break;
                    }
                }

                // detachable
                if (Game.Type == GameType.DarkestHour && unit.Detachable)
                {
                    writer.WriteLine("detachable = yes");
                    writer.WriteLine();
                }

                // allowed_brigades
                if (unit.AllowedBrigades.Count > 0)
                {
                    foreach (UnitType brigade in unit.AllowedBrigades)
                    {
                        writer.WriteLine("allowed_brigades = {0}", Units.StringTable[(int) brigade]);
                    }
                    writer.WriteLine();
                }

                // upgrade
                if (Game.Type == GameType.DarkestHour && unit.Upgrades.Count > 0)
                {
                    foreach (UnitUpgrade upgrade in unit.Upgrades)
                    {
                        writer.WriteLine(
                            "upgrade = {{ type = {0} upgrade_time_factor = {1} upgrade_cost_factor = {2}}}",
                            Units.StringTable[(int) upgrade.Type],
                            upgrade.UpgradeTimeFactor,
                            upgrade.UpgradeCostFactor);
                    }
                    writer.WriteLine();
                }

                // model
                int no = 0;
                foreach (UnitModel model in unit.Models)
                {
                    WriteModel(model, unit, no, writer);
                    no++;
                }
            }
        }

        /// <summary>
        ///     modelセクションを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="unit">ユニットデータ</param>
        /// <param name="no">モデル番号</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteModel(UnitModel model, Unit unit, int no, StreamWriter writer)
        {
            writer.WriteLine("# {0} - {1}", no, Config.GetText(UnitModel.GetName(unit, no, CountryTag.None)));
            writer.WriteLine("model = {");

            // 兵科固有部分
            switch (unit.Branch)
            {
                case UnitBranch.Army:
                    if (unit.Organization == UnitOrganization.Division)
                    {
                        WriteLandDivision(model, writer);
                    }
                    else
                    {
                        WriteLandBrigade(model, writer);
                    }
                    break;

                case UnitBranch.Navy:
                    if (unit.Organization == UnitOrganization.Division)
                    {
                        WriteNavalDivision(model, writer);
                    }
                    else
                    {
                        WriteNavalBrigade(model, writer);
                    }
                    break;

                case UnitBranch.AirForce:
                    if (unit.Organization == UnitOrganization.Division)
                    {
                        WriteAirDivision(model, writer);
                    }
                    else
                    {
                        WriteAirBrigade(model, writer);
                    }
                    break;
            }

            // equipment (DH1.03以降)
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103 && model.Equipments.Count > 0)
            {
                writer.Write("\tequipment = {");
                foreach (UnitEquipment equipment in model.Equipments)
                {
                    writer.Write(" {0} = {1}", equipment.Resource, equipment.Quantity);
                }
                writer.WriteLine(" }");
            }

            writer.WriteLine("}");
        }

        /// <summary>
        ///     陸軍師団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLandDivision(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            writer.WriteLine("\tdefensiveness \t\t\t= {0}", model.Defensiveness);
            writer.WriteLine("\ttoughness\t\t\t\t= {0}", model.Toughness);
            writer.WriteLine("\tsoftness\t\t\t\t= {0}", model.Softness);
            writer.WriteLine("\tsuppression\t\t\t\t= {0}", model.Suppression);
            writer.WriteLine("\tairdefence\t\t\t\t= {0}", model.AirDefense);
            writer.WriteLine("\tsoftattack\t\t\t\t= {0}", model.SoftAttack);
            writer.WriteLine("\thardattack\t\t\t\t= {0}", model.HardAttack);
            writer.WriteLine("\tairattack\t\t\t\t= {0}", model.AirAttack);
            writer.WriteLine("\ttransportweight\t\t\t= {0}", model.TransportWeight);
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.NoFuelCombatMod) > 0.00005)
            {
                writer.WriteLine("\tno_fuel_combat_mod \t\t= {0}", model.NoFuelCombatMod);
            }
            writer.WriteLine("\tspeed_cap_art\t\t\t= {0}", model.SpeedCapArt);
            writer.WriteLine("\tspeed_cap_eng\t\t\t= {0}", model.SpeedCapEng);
            writer.WriteLine("\tspeed_cap_at\t\t\t= {0}", model.SpeedCapAt);
            writer.WriteLine("\tspeed_cap_aa\t\t\t= {0}", model.SpeedCapAa);
            writer.WriteLine("\tupgrade_time_factor = {0}", model.UpgradeTimeFactor);
            writer.WriteLine("\tupgrade_cost_factor = {0}", model.UpgradeCostFactor);
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                writer.WriteLine("\tmax_supply_stock = {0}", model.MaxSupplyStock);
                writer.WriteLine("\tmax_oil_stock = {0}", model.MaxOilStock);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceTimeFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", model.ReinforceTimeFactor);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceCostFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_cost\t\t= {0}", model.ReinforceCostFactor);
            }
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103 && Math.Abs(model.SpeedCap) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap\t\t\t= {0}", model.SpeedCap);
            }
        }

        /// <summary>
        ///     海軍師団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalDivision(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}", model.SurfaceDetectionCapability);
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", model.AirDetectionCapability);
            writer.WriteLine("\tsubdetectioncapability\t\t= {0}", model.SubDetectionCapability);
            writer.WriteLine("\tvisibility\t\t\t\t\t= {0}", model.Visibility);
            writer.WriteLine("\tseadefence\t\t\t\t\t= {0}", model.SeaDefense);
            writer.WriteLine("\tairdefence\t\t\t\t\t= {0}", model.AirDefense);
            writer.WriteLine("\tseaattack\t\t\t\t\t= {0}", model.SeaAttack);
            writer.WriteLine("\tairattack\t\t\t\t\t= {0}", model.AirAttack);
            writer.WriteLine("\tsubattack\t\t\t\t\t= {0}", model.SubAttack);
            writer.WriteLine("\tconvoyattack\t\t\t\t\t= {0}", model.ConvoyAttack);
            writer.WriteLine("\tshorebombardment\t\t\t= {0}", model.ShoreBombardment);
            writer.WriteLine("\ttransportcapability\t\t\t= {0}", model.TransportCapability);
            writer.WriteLine("\t");
            writer.WriteLine("\trange\t\t\t\t\t\t= {0}", model.Range);
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            writer.WriteLine("\tdistance\t\t\t\t\t= {0}", model.Distance);
        }

        /// <summary>
        ///     空軍師団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirDivision(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}", model.SurfaceDetectionCapability);
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", model.AirDetectionCapability);
            writer.WriteLine("\tsurfacedefence\t\t\t= {0}", model.SurfaceDefense);
            writer.WriteLine("\tairdefence\t\t\t\t\t= {0}", model.AirDefense);
            writer.WriteLine("\tairattack\t\t\t\t\t= {0}", model.AirAttack);
            if (Math.Abs(model.StrategicAttack) > 0.00005)
            {
                writer.WriteLine("\tstrategicattack\t\t\t\t\t= {0}", model.StrategicAttack);
            }
            if (Math.Abs(model.SoftAttack) > 0.00005)
            {
                writer.WriteLine("\tsoftattack\t\t\t\t= {0}", model.SoftAttack);
            }
            if (Math.Abs(model.HardAttack) > 0.00005)
            {
                writer.WriteLine("\thardattack\t\t\t\t= {0}", model.HardAttack);
            }
            if (Math.Abs(model.NavalAttack) > 0.00005)
            {
                writer.WriteLine("\tnavalattack\t\t\t\t\t= {0}", model.NavalAttack);
            }
            writer.WriteLine("\trange\t\t\t\t\t\t= {0}", model.Range);
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            writer.WriteLine("\tupgrade_time_factor = {0}", model.UpgradeTimeFactor);
            writer.WriteLine("\tupgrade_cost_factor = {0}", model.UpgradeCostFactor);
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceTimeFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", model.ReinforceTimeFactor);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceCostFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_cost\t\t= {0}", model.ReinforceCostFactor);
            }
        }

        /// <summary>
        ///     陸軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLandBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            if (Math.Abs(model.MaxSpeed) > 0.00005)
            {
                writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            }
            if (Math.Abs(model.DefaultOrganization) > 0.00005)
            {
                writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            }
            if (Math.Abs(model.Morale) > 0.00005)
            {
                writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            }
            if (Math.Abs(model.Defensiveness) > 0.00005)
            {
                writer.WriteLine("\tdefensiveness \t\t\t= {0}", model.Defensiveness);
            }
            if (Math.Abs(model.Toughness) > 0.00005)
            {
                writer.WriteLine("\ttoughness\t\t\t\t= {0}", model.Toughness);
            }
            if (Math.Abs(model.Softness) > 0.00005)
            {
                writer.WriteLine("\tsoftness\t\t\t\t= {0}", model.Softness);
            }
            if (Math.Abs(model.Suppression) > 0.00005)
            {
                writer.WriteLine("\tsuppression\t\t\t\t= {0}", model.Suppression);
            }
            if (Math.Abs(model.AirDefense) > 0.00005)
            {
                writer.WriteLine("\tairdefence\t\t\t\t= {0}", model.AirDefense);
            }
            if (Math.Abs(model.SoftAttack) > 0.00005)
            {
                writer.WriteLine("\tsoftattack\t\t\t\t= {0}", model.SoftAttack);
            }
            if (Math.Abs(model.HardAttack) > 0.00005)
            {
                writer.WriteLine("\thardattack\t\t\t\t= {0}", model.HardAttack);
            }
            if (Math.Abs(model.AirAttack) > 0.00005)
            {
                writer.WriteLine("\tairattack\t\t\t\t= {0}", model.AirAttack);
            }
            if (Game.Type == GameType.ArsenalOfDemocracy && Math.Abs(model.ArtilleryBombardment) > 0.00005)
            {
                writer.WriteLine("\tartillery_bombardment\t\t= {0}", model.ArtilleryBombardment);
            }
            if (Math.Abs(model.TransportWeight) > 0.00005)
            {
                writer.WriteLine("\ttransportweight\t\t\t= {0}", model.TransportWeight);
            }
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            if (Math.Abs(model.FuelConsumption) > 0.00005)
            {
                writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.NoFuelCombatMod) > 0.00005)
            {
                writer.WriteLine("\tno_fuel_combat_mod \t\t= {0}", model.NoFuelCombatMod);
            }
            if (Math.Abs(model.SpeedCapArt) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap_art\t\t\t= {0}", model.SpeedCapArt);
            }
            if (Math.Abs(model.SpeedCapEng) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap_eng\t\t\t= {0}", model.SpeedCapEng);
            }
            if (Math.Abs(model.SpeedCapAt) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap_at\t\t\t= {0}", model.SpeedCapAt);
            }
            if (Math.Abs(model.SpeedCapAa) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap_aa\t\t\t= {0}", model.SpeedCapAa);
            }
            writer.WriteLine("\tupgrade_time_factor = {0}", model.UpgradeTimeFactor);
            writer.WriteLine("\tupgrade_cost_factor = {0}", model.UpgradeCostFactor);
            if (Game.Type == GameType.ArsenalOfDemocracy && Math.Abs(model.MaxSupplyStock) > 0.00005)
            {
                writer.WriteLine("\tmax_supply_stock = {0}", model.MaxSupplyStock);
            }
            if (Game.Type == GameType.ArsenalOfDemocracy && Math.Abs(model.MaxOilStock) > 0.00005)
            {
                writer.WriteLine("\tmax_oil_stock = {0}", model.MaxOilStock);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceTimeFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", model.ReinforceTimeFactor);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceCostFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_cost\t\t= {0}", model.ReinforceCostFactor);
            }
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103 && Math.Abs(model.SpeedCap) > 0.00005)
            {
                writer.WriteLine("\tspeed_cap\t\t\t= {0}", model.SpeedCap);
            }
        }

        /// <summary>
        ///     海軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            if (Math.Abs(model.DefaultOrganization) > 0.00005)
            {
                writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            }
            if (Math.Abs(model.Morale) > 0.00005)
            {
                writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            }
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            if (Math.Abs(model.MaxSpeed) > 0.00005)
            {
                writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            }
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}", model.SurfaceDetectionCapability);
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", model.AirDetectionCapability);
            writer.WriteLine("\tsubdetectioncapability\t\t= {0}", model.SubDetectionCapability);
            if (Math.Abs(model.Visibility) > 0.00005)
            {
                writer.WriteLine("\tvisibility\t\t\t\t\t= {0}", model.Visibility);
            }
            writer.WriteLine("\tairdefence\t\t\t\t\t= {0}", model.AirDefense);
            if (Math.Abs(model.SeaDefense) > 0.00005)
            {
                writer.WriteLine("\tseadefence\t\t\t\t\t= {0}", model.SeaDefense);
            }
            writer.WriteLine("\tseaattack\t\t\t\t\t= {0}", model.SeaAttack);
            writer.WriteLine("\tconvoyattack\t\t\t\t\t= {0}", model.ConvoyAttack);
            writer.WriteLine("\tsubattack\t\t\t\t\t= {0}", model.SubAttack);
            writer.WriteLine("\tairattack\t\t\t\t\t= {0}", model.AirAttack);
            writer.WriteLine("\tshorebombardment\t\t\t= {0}", model.ShoreBombardment);
            if (Math.Abs(model.TransportCapability) > 0.00005)
            {
                writer.WriteLine("\ttransportcapability\t\t\t= {0}", model.TransportCapability);
            }
            if (Math.Abs(model.Range) > 0.00005)
            {
                writer.WriteLine("\trange\t\t\t\t\t\t= {0}", model.Range);
            }
            writer.WriteLine("\tdistance\t\t\t\t\t= {0}", model.Distance);
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            writer.WriteLine("\tupgrade_time_factor = {0}", model.UpgradeTimeFactor);
            writer.WriteLine("\tupgrade_cost_factor = {0}", model.UpgradeCostFactor);
        }

        /// <summary>
        ///     空軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", model.Cost);
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", model.BuildTime);
            writer.WriteLine("\tdefaultorganisation \t= {0}", model.DefaultOrganization);
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", model.Morale);
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", model.ManPower);
            writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", model.MaxSpeed);
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}", model.SurfaceDetectionCapability);
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", model.AirDetectionCapability);
            writer.WriteLine("\tsurfacedefence\t\t\t= {0}", model.SurfaceDefense);
            writer.WriteLine("\tairdefence\t\t\t\t\t= {0}", model.AirDefense);
            writer.WriteLine("\tairattack\t\t\t\t\t= {0}", model.AirAttack);
            writer.WriteLine("\tstrategicattack\t\t\t\t\t= {0}", model.StrategicAttack);
            writer.WriteLine("\tsoftattack\t\t\t\t= {0}", model.SoftAttack);
            writer.WriteLine("\thardattack\t\t\t\t= {0}", model.HardAttack);
            writer.WriteLine("\tnavalattack\t\t\t\t\t= {0}", model.NavalAttack);
            writer.WriteLine("\trange\t\t\t\t\t\t= {0}", model.Range);
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", model.SupplyConsumption);
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", model.FuelConsumption);
            writer.WriteLine("\tupgrade_time_factor = {0}", model.UpgradeTimeFactor);
            writer.WriteLine("\tupgrade_cost_factor = {0}", model.UpgradeCostFactor);
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceTimeFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", model.ReinforceTimeFactor);
            }
            if (Game.Type == GameType.DarkestHour && Math.Abs(model.ReinforceCostFactor) > 0.00005)
            {
                writer.WriteLine("\treinforce_cost\t\t= {0}", model.ReinforceCostFactor);
            }
        }
    }
}