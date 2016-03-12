using System.Collections.Generic;
using System.IO;
using System.Text;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Writers
{
    /// <summary>
    ///     ユニットデータの書き込みを担当するクラス
    /// </summary>
    internal static class UnitWriter
    {
        #region ファイル書き込み

        /// <summary>
        ///     ユニットデータをファイルへ書き込む
        /// </summary>
        /// <param name="unit">ユニットデータ</param>
        /// <param name="fileName">ファイル名</param>
        internal static void Write(UnitClass unit, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // unit_type
                if (Game.Type == GameType.ArsenalOfDemocracy)
                {
                    switch (unit.Branch)
                    {
                        case Branch.Army:
                            writer.WriteLine("land_unit_type = 1");
                            break;

                        case Branch.Navy:
                            writer.WriteLine("naval_unit_type = 1");
                            break;

                        case Branch.Airforce:
                            writer.WriteLine("air_unit_type = 1");
                            break;
                    }
                }

                // max_speed_step
                if ((Game.Type == GameType.ArsenalOfDemocracy) &&
                    (unit.Organization == UnitOrganization.Division) &&
                    (unit.MaxSpeedStep < 2))
                {
                    writer.WriteLine("max_speed_step = {0}", unit.MaxSpeedStep);
                }

                // detachable
                if (Game.Type == GameType.DarkestHour && unit.Detachable)
                {
                    writer.WriteLine("detachable = yes");
                    writer.WriteLine();
                }

                // locked
                if ((Game.Type == GameType.ArsenalOfDemocracy) &&
                    (unit.Organization == UnitOrganization.Brigade) &&
                    !unit.Detachable)
                {
                    writer.WriteLine("locked = 1");
                }

                // allowed_brigades
                if (unit.AllowedBrigades.Count > 0)
                {
                    foreach (UnitType brigade in unit.AllowedBrigades)
                    {
                        writer.WriteLine("allowed_brigades = {0}", Units.Strings[(int) brigade]);
                    }
                    // max_allowed_brigades
                    if ((Game.Type == GameType.DarkestHour) && (unit.MaxAllowedBrigades >= 0))
                    {
                        writer.WriteLine("max_allowed_brigades = {0}", unit.MaxAllowedBrigades);
                    }
                    writer.WriteLine();
                }

                // upgrade
                if ((Game.Type == GameType.DarkestHour) && (unit.Upgrades.Count > 0))
                {
                    foreach (UnitUpgrade upgrade in unit.Upgrades)
                    {
                        writer.WriteLine(
                            "upgrade = {{ type = {0} upgrade_time_factor = {1} upgrade_cost_factor = {2} }}",
                            Units.Strings[(int) upgrade.Type],
                            DoubleHelper.ToString(upgrade.UpgradeTimeFactor),
                            DoubleHelper.ToString(upgrade.UpgradeCostFactor));
                    }
                    writer.WriteLine();
                }

                // model
                int index = 0;
                foreach (UnitModel model in unit.Models)
                {
                    WriteModel(model, unit, index, writer);
                    index++;
                }
            }
        }

        /// <summary>
        ///     modelセクションを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="unit">ユニットデータ</param>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteModel(UnitModel model, UnitClass unit, int index, StreamWriter writer)
        {
            writer.WriteLine("# {0} - {1}", index, unit.GetModelName(index));
            writer.WriteLine("model = {");

            // 兵科固有部分
            switch (unit.Branch)
            {
                case Branch.Army:
                    if (unit.Organization == UnitOrganization.Division)
                    {
                        WriteLandDivision(model, writer);
                    }
                    else
                    {
                        WriteLandBrigade(model, writer);
                    }
                    break;

                case Branch.Navy:
                    if (unit.Organization == UnitOrganization.Division)
                    {
                        WriteNavalDivision(model, writer);
                    }
                    else
                    {
                        WriteNavalBrigade(model, writer);
                    }
                    break;

                case Branch.Airforce:
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

            if (Game.Type == GameType.DarkestHour)
            {
                // 自動改良
                if (model.AutoUpgrade)
                {
                    writer.WriteLine("\t{0}\t\t\t\t= {1}", Units.Strings[(int) model.UpgradeClass], model.UpgradeModel);
                }
                // upgrade_time_boost
                if (!model.UpgradeTimeBoost)
                {
                    writer.WriteLine("\tupgrade_time_boost \t= {0}", model.UpgradeTimeBoost ? "yes" : "no");
                }
            }

            // equipment (DH1.03以降)
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103) && (model.Equipments.Count > 0))
            {
                writer.Write("\tequipment = {");
                foreach (UnitEquipment equipment in model.Equipments)
                {
                    writer.Write(" {0} = {1}", Units.EquipmentStrings[(int) equipment.Resource], equipment.Quantity);
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
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            writer.WriteLine("\tmanpower \t\t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            writer.WriteLine("\tdefaultorganisation \t= {0}", DoubleHelper.ToString(model.DefaultOrganization));
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            writer.WriteLine("\tdefensiveness \t\t\t= {0}", DoubleHelper.ToString(model.Defensiveness));
            writer.WriteLine("\ttoughness\t\t\t\t= {0}", DoubleHelper.ToString(model.Toughness));
            writer.WriteLine("\tsoftness\t\t\t\t= {0}", DoubleHelper.ToString(model.Softness));
            writer.WriteLine("\tsuppression\t\t\t\t= {0}", DoubleHelper.ToString(model.Suppression));
            writer.WriteLine("\tairdefence\t\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            writer.WriteLine("\tsoftattack\t\t\t\t= {0}", DoubleHelper.ToString(model.SoftAttack));
            writer.WriteLine("\thardattack\t\t\t\t= {0}", DoubleHelper.ToString(model.HardAttack));
            writer.WriteLine("\tairattack\t\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            writer.WriteLine("\ttransportweight\t\t\t= {0}", DoubleHelper.ToString(model.TransportWeight));
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.NoFuelCombatMod))
            {
                writer.WriteLine("\tno_fuel_combat_mod \t\t= {0}", DoubleHelper.ToString(model.NoFuelCombatMod));
            }
            if (!DoubleHelper.IsZero(model.SpeedCapArt))
            {
                writer.WriteLine("\tspeed_cap_art\t\t\t= {0}", DoubleHelper.ToString(model.SpeedCapArt));
            }
            if (!DoubleHelper.IsZero(model.SpeedCapEng))
            {
                writer.WriteLine("\tspeed_cap_eng\t\t\t= {0}", DoubleHelper.ToString(model.SpeedCapEng));
            }
            if (!DoubleHelper.IsZero(model.SpeedCapAt))
            {
                writer.WriteLine("\tspeed_cap_at\t\t\t= {0}", DoubleHelper.ToString(model.SpeedCapAt));
            }
            if (!DoubleHelper.IsZero(model.SpeedCapAa))
            {
                writer.WriteLine("\tspeed_cap_aa\t\t\t= {0}", DoubleHelper.ToString(model.SpeedCapAa));
            }
            writer.WriteLine("\tupgrade_time_factor = {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            writer.WriteLine("\tupgrade_cost_factor = {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
            if ((Game.Type == GameType.ArsenalOfDemocracy) && !DoubleHelper.IsZero(model.MaxSupplyStock))
            {
                writer.WriteLine("\tmax_supply_stock = {0}", DoubleHelper.ToString(model.MaxSupplyStock));
            }
            if ((Game.Type == GameType.ArsenalOfDemocracy) && !DoubleHelper.IsZero(model.MaxOilStock))
            {
                writer.WriteLine("\tmax_oil_stock = {0}", DoubleHelper.ToString(model.MaxOilStock));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceTimeFactor))
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", DoubleHelper.ToString(model.ReinforceTimeFactor));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceCostFactor))
            {
                writer.WriteLine("\treinforce_cost \t\t= {0}", DoubleHelper.ToString(model.ReinforceCostFactor));
            }
        }

        /// <summary>
        ///     海軍師団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalDivision(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost\t\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime \t\t\t\t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            writer.WriteLine("\tdefaultorganisation \t\t= {0}", DoubleHelper.ToString(model.DefaultOrganization));
            writer.WriteLine("\tmorale\t\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            writer.WriteLine("\tmanpower\t\t\t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            writer.WriteLine("\tmaxspeed\t\t\t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}",
                DoubleHelper.ToString(model.SurfaceDetectionCapability));
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.AirDetectionCapability));
            writer.WriteLine("\tsubdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.SubDetectionCapability));
            writer.WriteLine("\tvisibility\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Visibility));
            writer.WriteLine("\tseadefence\t\t\t\t\t= {0}", DoubleHelper.ToString(model.SeaDefense));
            writer.WriteLine("\tairdefence\t\t\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            writer.WriteLine("\tseaattack\t\t\t\t\t= {0}", DoubleHelper.ToString(model.SeaAttack));
            writer.WriteLine("\tairattack\t\t\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            writer.WriteLine("\tsubattack\t\t\t\t\t= {0}", DoubleHelper.ToString(model.SubAttack));
            writer.WriteLine("\tconvoyattack\t\t\t\t\t= {0}", DoubleHelper.ToString(model.ConvoyAttack));
            writer.WriteLine("\tshorebombardment\t\t\t= {0}", DoubleHelper.ToString(model.ShoreBombardment));
            writer.WriteLine("\ttransportcapability\t\t\t= {0}", DoubleHelper.ToString(model.TransportCapability));
            writer.WriteLine("\trange\t\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Range));
            writer.WriteLine("\tsupplyconsumption\t\t\t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            writer.WriteLine("\tfuelconsumption\t\t\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            writer.WriteLine("\tdistance\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Distance));
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.UpgradeTimeFactor))
            {
                writer.WriteLine("\tupgrade_time_factor = {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.UpgradeCostFactor))
            {
                writer.WriteLine("\tupgrade_cost_factor = {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceTimeFactor))
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", DoubleHelper.ToString(model.ReinforceTimeFactor));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceCostFactor))
            {
                writer.WriteLine("\treinforce_cost \t\t= {0}", DoubleHelper.ToString(model.ReinforceCostFactor));
            }
        }

        /// <summary>
        ///     空軍師団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirDivision(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            writer.WriteLine("\tdefaultorganisation \t\t= {0}", DoubleHelper.ToString(model.DefaultOrganization));
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            writer.WriteLine("\tmanpower\t\t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            writer.WriteLine("\tmaxspeed\t\t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}",
                DoubleHelper.ToString(model.SurfaceDetectionCapability));
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.AirDetectionCapability));
            writer.WriteLine("\tsurfacedefence\t\t\t\t= {0}", DoubleHelper.ToString(model.SurfaceDefence));
            writer.WriteLine("\tairdefence\t\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            writer.WriteLine("\tairattack\t\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            writer.WriteLine("\tstrategicattack\t\t\t\t= {0}", DoubleHelper.ToString(model.StrategicAttack));
            writer.WriteLine("\tsoftattack\t\t\t\t= {0}", DoubleHelper.ToString(model.SoftAttack));
            writer.WriteLine("\thardattack\t\t\t\t= {0}", DoubleHelper.ToString(model.HardAttack));
            writer.WriteLine("\tnavalattack\t\t\t\t= {0}", DoubleHelper.ToString(model.NavalAttack));
            if (!DoubleHelper.IsZero(model.TransportCapability))
            {
                writer.WriteLine("\ttransportcapability\t\t\t= {0}", DoubleHelper.ToString(model.TransportCapability));
            }
            writer.WriteLine("\trange\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Range));
            writer.WriteLine("\tsupplyconsumption \t\t\t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            writer.WriteLine("\tfuelconsumption\t\t\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            writer.WriteLine("\tupgrade_time_factor = {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            writer.WriteLine("\tupgrade_cost_factor = {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceTimeFactor))
            {
                writer.WriteLine("\treinforce_time\t\t= {0}", DoubleHelper.ToString(model.ReinforceTimeFactor));
            }
            if ((Game.Type == GameType.DarkestHour) && !DoubleHelper.IsZero(model.ReinforceCostFactor))
            {
                writer.WriteLine("\treinforce_cost \t\t= {0}", DoubleHelper.ToString(model.ReinforceCostFactor));
            }
        }

        /// <summary>
        ///     陸軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLandBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime\t \t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            writer.WriteLine("\tmanpower \t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            if (!DoubleHelper.IsZero(model.MaxSpeed))
            {
                writer.WriteLine("\tmaxspeed \t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            }
            if (!DoubleHelper.IsZero(model.DefaultOrganization))
            {
                writer.WriteLine("\tdefaultorganisation = {0}", DoubleHelper.ToString(model.DefaultOrganization));
            }
            if (!DoubleHelper.IsZero(model.Morale))
            {
                writer.WriteLine("\tmorale\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            }
            if (!DoubleHelper.IsZero(model.Defensiveness))
            {
                writer.WriteLine("\tdefensiveness \t\t= {0}", DoubleHelper.ToString(model.Defensiveness));
            }
            if (!DoubleHelper.IsZero(model.Toughness))
            {
                writer.WriteLine("\ttoughness \t\t\t= {0}", DoubleHelper.ToString(model.Toughness));
            }
            if (!DoubleHelper.IsZero(model.Softness))
            {
                writer.WriteLine("\tsoftness\t\t\t= {0}", DoubleHelper.ToString(model.Softness));
            }
            if (!DoubleHelper.IsZero(model.Suppression))
            {
                writer.WriteLine("\tsuppression\t\t\t= {0}", DoubleHelper.ToString(model.Suppression));
            }
            if (!DoubleHelper.IsZero(model.AirDefence))
            {
                writer.WriteLine("\tairdefence\t\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            }
            if (!DoubleHelper.IsZero(model.SoftAttack))
            {
                writer.WriteLine("\tsoftattack\t\t\t= {0}", DoubleHelper.ToString(model.SoftAttack));
            }
            if (!DoubleHelper.IsZero(model.HardAttack))
            {
                writer.WriteLine("\thardattack\t\t\t= {0}", DoubleHelper.ToString(model.HardAttack));
            }
            if (!DoubleHelper.IsZero(model.AirAttack))
            {
                writer.WriteLine("\tairattack\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            }
            if ((Game.Type == GameType.ArsenalOfDemocracy) && !DoubleHelper.IsZero(model.ArtilleryBombardment))
            {
                writer.WriteLine("\tartillery_bombardment\t\t= {0}", DoubleHelper.ToString(model.ArtilleryBombardment));
            }
            if (!DoubleHelper.IsZero(model.TransportWeight))
            {
                writer.WriteLine("\ttransportweight\t\t\t= {0}", DoubleHelper.ToString(model.TransportWeight));
            }
            writer.WriteLine("\tsupplyconsumption \t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            if (!DoubleHelper.IsZero(model.FuelConsumption))
            {
                writer.WriteLine("\tfuelconsumption\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            }
            writer.WriteLine("\tupgrade_time_factor = {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            writer.WriteLine("\tupgrade_cost_factor = {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103) && !DoubleHelper.IsZero(model.SpeedCap))
            {
                writer.WriteLine("\tspeed_cap\t\t\t= {0}", DoubleHelper.ToString(model.SpeedCap));
            }
        }

        /// <summary>
        ///     海軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteNavalBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime\t\t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            if (!DoubleHelper.IsZero(model.DefaultOrganization))
            {
                writer.WriteLine("\tdefaultorganisation \t= {0}", DoubleHelper.ToString(model.DefaultOrganization));
            }
            if (!DoubleHelper.IsZero(model.Morale))
            {
                writer.WriteLine("\tmorale\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            }
            writer.WriteLine("\tmanpower \t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            if (!DoubleHelper.IsZero(model.MaxSpeed))
            {
                writer.WriteLine("\tmaxspeed \t\t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            }
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}",
                DoubleHelper.ToString(model.SurfaceDetectionCapability));
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.AirDetectionCapability));
            writer.WriteLine("\tsubdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.SubDetectionCapability));
            if (!DoubleHelper.IsZero(model.Visibility))
            {
                writer.WriteLine("\tvisibility\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Visibility));
            }
            writer.WriteLine("\tairdefence\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            if (!DoubleHelper.IsZero(model.SeaDefense))
            {
                writer.WriteLine("\tseadefence\t\t\t= {0}", DoubleHelper.ToString(model.SeaDefense));
            }
            writer.WriteLine("\tseaattack\t\t\t= {0}", DoubleHelper.ToString(model.SeaAttack));
            writer.WriteLine("\tconvoyattack\t\t\t= {0}", DoubleHelper.ToString(model.ConvoyAttack));
            writer.WriteLine("\tsubattack\t\t\t= {0}", DoubleHelper.ToString(model.SubAttack));
            writer.WriteLine("\tairattack\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            writer.WriteLine("\tshorebombardment\t\t= {0}", DoubleHelper.ToString(model.ShoreBombardment));
            if (!DoubleHelper.IsZero(model.TransportCapability))
            {
                writer.WriteLine("\ttransportcapability\t\t\t= {0}", DoubleHelper.ToString(model.TransportCapability));
            }
            if (!DoubleHelper.IsZero(model.Range))
            {
                writer.WriteLine("\trange\t\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Range));
            }
            writer.WriteLine("\tdistance\t\t\t= {0}", DoubleHelper.ToString(model.Distance));
            writer.WriteLine("\tsupplyconsumption \t\t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            writer.WriteLine("\tfuelconsumption\t\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            writer.WriteLine("\tupgrade_time_factor \t\t= {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            writer.WriteLine("\tupgrade_cost_factor \t\t= {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
        }

        /// <summary>
        ///     空軍旅団のモデルデータを書き出す
        /// </summary>
        /// <param name="model">ユニットモデルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteAirBrigade(UnitModel model, StreamWriter writer)
        {
            writer.WriteLine("\tcost \t\t\t\t\t= {0}", DoubleHelper.ToString(model.Cost));
            // 必要ICが0のモデルは未定義と扱う
            if (DoubleHelper.IsZero(model.Cost))
            {
                return;
            }
            writer.WriteLine("\tbuildtime\t \t\t\t= {0}", DoubleHelper.ToString(model.BuildTime));
            writer.WriteLine("\tdefaultorganisation \t\t= {0}", DoubleHelper.ToString(model.DefaultOrganization));
            writer.WriteLine("\tmorale\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Morale));
            writer.WriteLine("\tmanpower\t\t\t\t= {0}", DoubleHelper.ToString(model.ManPower));
            writer.WriteLine("\tmaxspeed\t\t\t\t= {0}", DoubleHelper.ToString(model.MaxSpeed));
            writer.WriteLine("\tsurfacedetectioncapability\t= {0}",
                DoubleHelper.ToString(model.SurfaceDetectionCapability));
            writer.WriteLine("\tairdetectioncapability\t\t= {0}", DoubleHelper.ToString(model.AirDetectionCapability));
            writer.WriteLine("\tsurfacedefence\t\t\t\t= {0}", DoubleHelper.ToString(model.SurfaceDefence));
            writer.WriteLine("\tairdefence\t\t\t\t= {0}", DoubleHelper.ToString(model.AirDefence));
            writer.WriteLine("\tairattack\t\t\t\t= {0}", DoubleHelper.ToString(model.AirAttack));
            writer.WriteLine("\tstrategicattack\t\t\t\t= {0}", DoubleHelper.ToString(model.StrategicAttack));
            writer.WriteLine("\tsoftattack\t\t\t\t= {0}", DoubleHelper.ToString(model.SoftAttack));
            writer.WriteLine("\thardattack\t\t\t\t= {0}", DoubleHelper.ToString(model.HardAttack));
            writer.WriteLine("\tnavalattack\t\t\t\t= {0}", DoubleHelper.ToString(model.NavalAttack));
            if (!DoubleHelper.IsZero(model.TransportCapability))
            {
                writer.WriteLine("\ttransportcapability\t\t\t= {0}", DoubleHelper.ToString(model.TransportCapability));
            }
            writer.WriteLine("\trange\t\t\t\t\t= {0}", DoubleHelper.ToString(model.Range));
            writer.WriteLine("\tsupplyconsumption \t\t\t= {0}", DoubleHelper.ToString(model.SupplyConsumption));
            writer.WriteLine("\tfuelconsumption\t\t\t\t= {0}", DoubleHelper.ToString(model.FuelConsumption));
            writer.WriteLine("\tupgrade_time_factor = {0}", DoubleHelper.ToString(model.UpgradeTimeFactor));
            writer.WriteLine("\tupgrade_cost_factor = {0}", DoubleHelper.ToString(model.UpgradeCostFactor));
        }

        /// <summary>
        ///     師団ユニットクラス定義データをファイルへ書き込む
        /// </summary>
        /// <param name="units">ユニットクラス一覧</param>
        /// <param name="fileName">ファイル名</param>
        internal static void WriteDivisionTypes(List<UnitClass> units, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // ヘッダを出力する
                WriteDivisionTypesHeader(writer);

                // ユニットクラス定義データを順に書き込む
                foreach (UnitType type in Units.DivisionTypes)
                {
                    UnitClass unit = units[(int) type];

                    // ユーザー定義師団に定義内容がなければ出力しない
                    if ((unit.Type >= UnitType.Division01) && !unit.ExistsEntity() && (unit.Models.Count == 0))
                    {
                        continue;
                    }

                    // ユニットクラス定義データを書き込む
                    WriteDivisionType(unit, writer);
                }
            }
        }

        /// <summary>
        ///     師団ユニットクラス定義を書き込む
        /// </summary>
        /// <param name="unit">ユニットクラスデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDivisionType(UnitClass unit, StreamWriter writer)
        {
            writer.WriteLine();
            if (unit.Type < UnitType.ReserveDivision33 || unit.Type > UnitType.ReserveDivision40)
            {
                writer.WriteLine("{0} = {{", Units.Strings[(int) unit.Type]);
            }
            else
            {
                writer.WriteLine("{0} = {{\t# Reserved for use by Darkest Hour Full", Units.Strings[(int) unit.Type]);
            }
            writer.WriteLine("\t#ID\t\t\t{0}", Units.UnitNumbers[(int) unit.Type]);
            if (unit.ExistsEntity() || (unit.Models.Count > 0))
            {
                writer.WriteLine("\ttype\t\t= {0}", Units.RealStrings[(int) unit.RealType]);
                writer.WriteLine("\tname\t\t= {0}", unit.Name);
                writer.WriteLine("\tshort_name\t= {0}", unit.ShortName);
                writer.WriteLine("\tdesc\t\t= {0}", unit.Desc);
                writer.WriteLine("\tshort_desc\t= {0}", unit.ShortDesc);
                writer.WriteLine("\teyr\t\t\t= {0}", unit.Eyr);
                writer.WriteLine("\tsprite\t\t= {0}", Units.SpriteStrings[(int) unit.Sprite]);
                writer.WriteLine("\ttransmute\t= {0}", Units.Strings[(int) unit.Transmute]);
                writer.WriteLine("\tgfx_prio\t= {0}", unit.GfxPrio);
                if (unit.DefaultType)
                {
                    string s = Units.RealStrings[(int) unit.RealType];
                    int len = s.Length;
                    if (len < 4)
                    {
                        writer.WriteLine("\t{0}\t\t\t= yes", s);
                    }
                    else if (len < 8)
                    {
                        writer.WriteLine("\t{0}\t\t= yes", s);
                    }
                    else if (len < 12)
                    {
                        writer.WriteLine("\t{0}\t= yes", s);
                    }
                    else
                    {
                        writer.WriteLine("\t{0} = yes", s);
                    }
                }
                writer.WriteLine("\tvalue\t\t= {0}", DoubleHelper.ToString(unit.Value));
                if (unit.Productable)
                {
                    writer.WriteLine("\tproduction\t= yes");
                }
            }
            writer.WriteLine("\tlist_prio\t= {0}", unit.ListPrio);
            if (unit.UiPrio != 0)
            {
                writer.WriteLine("\tui_prio\t\t= {0}", unit.UiPrio);
            }
            writer.WriteLine("}");
        }

        /// <summary>
        ///     師団ユニットクラス定義ファイルのヘッダを書き込む
        /// </summary>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteDivisionTypesHeader(StreamWriter writer)
        {
            writer.WriteLine("# UNIT TYPES DEFINITION FILE");
            writer.WriteLine("########");
            writer.WriteLine("# ARMY/NAVY/AIR COMPARISONS STATISTIC PAGES");
            writer.WriteLine("# eyr = {");
            writer.WriteLine("#	land = HeaderStringID1 # 1st column on Army page");
            writer.WriteLine("#	land = HeaderStringID2 # 2nd column on Army page");
            writer.WriteLine("#	[...list the rest of the land columns you want here]");
            writer.WriteLine("#	navy = HeaderStringID3 # 1st column on Naval page");
            writer.WriteLine("#	navy = HeaderStringID4 # 2nd column on Naval page");
            writer.WriteLine("#	[...list the rest of the naval columns you want here]");
            writer.WriteLine("#	air  = HeaderStringID5 # 1st column on Air page");
            writer.WriteLine("#	air  = HeaderStringID6 # 2nd column on Air page");
            writer.WriteLine("#	[...list the rest of the air columns you want here]");
            writer.WriteLine("# }");
            writer.WriteLine("########");
            writer.WriteLine("# FORMAT");
            writer.WriteLine("#");
            writer.WriteLine("# UnitTypeName = { 						# Replace UnitTypeName with one of the predefined unit types. ");
            writer.WriteLine("#	### REQUIRED PARAMETERS ###");
            writer.WriteLine("#	type		= RealUnitType			# Defines the real unit type (infantry, cavalry etc.)");
            writer.WriteLine("#	name		= UnitNameID			# Full unit name string (NAME_INFANTRY, NAME_CAVALRY etc.)");
            writer.WriteLine(
                "#	short_name	= ShortUnitNameID		# Short unit name string (SNAME_INFANTRY, SNAME_CAVALRY etc.)");
            writer.WriteLine(
                "#	desc		= LongDescriptionID		# Long unit description string (LDESC_INFANTRY, LDESC_CAVALRY etc.)");
            writer.WriteLine(
                "#	short_desc	= ShortDescriptionID	# Short unit description string (SDESC_INFANTRY, SDESC_CAVALRY etc.)");
            writer.WriteLine(
                "#	eyr			= Number				# Defines the group to which this unit type belongs on army/navy/air statistics page");
            writer.WriteLine(
                "#	sprite		= SpriteTypeName		# Replace SpriteTypeName with one of predefined sprite name types");
            writer.WriteLine(
                "#	transmute	= UnitTypeName			# Used by production AI. Defines what unit to build if the current one is not available ATM. ");
            writer.WriteLine(
                "#	gfx_prio	= Number				# Used to define which image to use on counter and sprite for mixed units ");
            writer.WriteLine(
                "#	value		= X.X(Number)			# Unit military value. Used in military power and other calculations.");
            writer.WriteLine("#	list_prio	= X(Number)				# Priority in lists (tech overview and production pages).");
            writer.WriteLine("#										# Units are sorted by groups (land/air/naval).");
            writer.WriteLine("#										# -1 - do not list that type, 0+ - priority in lists (lowest first)");
            writer.WriteLine("#	### OPTIONAL PARAMETERS ###");
            writer.WriteLine("#	ui_prio		= Number				# Sort priority for divisions in unit UI (highest first)");
            writer.WriteLine(
                "#	RealUnitType= yes					# Sets this UnitType as default RealUnitType. There can be only one definition for every RealUnitType. ");
            writer.WriteLine("#										# 	Used by production AI (militia, infantry), rebels (militia, infantry) ");
            writer.WriteLine(
                "#										#	and aliens (infantry, armor, strategic_bomber, interceptor, destroyer, carrier)");
            writer.WriteLine(
                "#										#	By default (when not specified) the first \"type = RealUnitType\" will be set.");
            writer.WriteLine(
                "#	production	= yes/no(default)		# Defines if this unit type is allowed(yes) or not(no) for production by default. ");
            writer.WriteLine(
                "#										#	When set to [no] (or not defined) the unit type must be unlocked by tech or event for every country.");
            writer.WriteLine("# }");
            writer.WriteLine("#");
            writer.WriteLine("########");
            writer.WriteLine("# The UnitTypeName is predefined (130 types in total) and must be used in this order:");
            writer.WriteLine(
                "#	infantry, cavalry, motorized, mechanized, light_armor, armor, garrison, hq, paratrooper, marine, ");
            writer.WriteLine(
                "#	bergsjaeger, cas, multi_role, interceptor, strategic_bomber, tactical_bomber, naval_bomber, ");
            writer.WriteLine(
                "#	transport_plane, battleship, light_cruiser, heavy_cruiser, battlecruiser, destroyer, carrier, ");
            writer.WriteLine(
                "#	submarine, transport, flying_bomb, flying_rocket, militia, escort_carrier, nuclear_submarine,");
            writer.WriteLine("#	light_carrier, rocket_interceptor, ");
            writer.WriteLine(
                "#	d_rsv_33, d_rsv_34, d_rsv_35, d_rsv_36, d_rsv_37, d_rsv_38, d_rsv_39, d_rsv_40,	# Reserved for use by Darkest Hour Full");
            writer.WriteLine(
                "#	d_01, d_02, d_03, d_04, d_05, d_06, d_07, d_08, d_09, d_10, d_11, d_12, d_13, d_14, d_15, d_16, ");
            writer.WriteLine(
                "#	d_17, d_18, d_19, d_20, d_21, d_22, d_23, d_24, d_25, d_26, d_27, d_28, d_29, d_30, d_31, d_32,");
            writer.WriteLine(
                "#	d_33, d_34, d_35, d_36, d_37, d_38, d_39, d_40, d_41, d_42, d_43, d_44, d_45, d_46, d_47, d_48,");
            writer.WriteLine(
                "#	d_49, d_50, d_51, d_52, d_53, d_54, d_55, d_56, d_57, d_58, d_59, d_60, d_61, d_62, d_63, d_64,");
            writer.WriteLine(
                "#	d_65, d_66, d_67, d_68, d_69, d_70, d_71, d_72, d_73, d_74, d_75, d_76, d_77, d_78, d_79, d_80,");
            writer.WriteLine(
                "#	d_81, d_82, d_83, d_84, d_85, d_86, d_87, d_88, d_89, d_90, d_91, d_92, d_93, d_94, d_95, d_96,");
            writer.WriteLine("#	d_97, d_98, d_99");
            writer.WriteLine("# There should be only one instance of each UnitTypeName!!!");
            writer.WriteLine("########");
            writer.WriteLine("# The RealUnitType is predefined and can be one of those:");
            writer.WriteLine(
                "#	infantry, cavalry, motorized, mechanized, light_armor, armor, garrison, hq, paratrooper,");
            writer.WriteLine("#	marine, bergsjaeger, cas, multi_role, interceptor, strategic_bomber, tactical_bomber,");
            writer.WriteLine("#	naval_bomber, transport_plane, battleship, light_cruiser, heavy_cruiser, battlecruiser,");
            writer.WriteLine(
                "#	destroyer, carrier, submarine, transport, flying_bomb, flying_rocket, militia, escort_carrier");
            writer.WriteLine("#	nuclear_submarine");
            writer.WriteLine("########");
            writer.WriteLine("# The SpriteTypeName is predefined and can be one of those:");
            writer.WriteLine("#	infantry, cavalry, motorized, mechanized, l_panzer, panzer, paratrooper, marine, ");
            writer.WriteLine(
                "#	bergsjaeger, fighter, escort, interceptor, bomber, tactical, cas, naval, transportplane, ");
            writer.WriteLine(
                "#	battleship, battlecruiser, heavy_cruiser, light_cruiser, destroyer, carrier, submarine, ");
            writer.WriteLine(
                "#	transport, militia, garrison, hq, flying_bomb, rocket, nuclear_submarine, escort_carrier,");
            writer.WriteLine("#	light_carrier, rocket_interceptor, ");
            writer.WriteLine("#	d_rsv_33, d_rsv_34, d_rsv_35, d_rsv_36, d_rsv_37, d_rsv_38, d_rsv_39, d_rsv_40,");
            writer.WriteLine(
                "#	d_01, d_02, d_03, d_04, d_05, d_06, d_07, d_08, d_09, d_10, d_11, d_12, d_13, d_14, d_15, d_16, ");
            writer.WriteLine(
                "#	d_17, d_18, d_19, d_20, d_21, d_22, d_23, d_24, d_25, d_26, d_27, d_28, d_29, d_30, d_31, d_32,");
            writer.WriteLine(
                "#	d_33, d_34, d_35, d_36, d_37, d_38, d_39, d_40, d_41, d_42, d_43, d_44, d_45, d_46, d_47, d_48,");
            writer.WriteLine(
                "#	d_49, d_50, d_51, d_52, d_53, d_54, d_55, d_56, d_57, d_58, d_59, d_60, d_61, d_62, d_63, d_64,");
            writer.WriteLine(
                "#	d_65, d_66, d_67, d_68, d_69, d_70, d_71, d_72, d_73, d_74, d_75, d_76, d_77, d_78, d_79, d_80,");
            writer.WriteLine(
                "#	d_81, d_82, d_83, d_84, d_85, d_86, d_87, d_88, d_89, d_90, d_91, d_92, d_93, d_94, d_95, d_96,");
            writer.WriteLine("#	d_97, d_98, d_99");
            writer.WriteLine("#");
            writer.WriteLine("########");
            writer.WriteLine("# NOTES:");
            writer.WriteLine(
                "#	1. It is possible to redefine the type of named unit types (like cavalry = { type = infantry }).");
            writer.WriteLine("#	2. No gaps (like defining d_02 without defining d_01) in UnitTypeName are allowed!");
            writer.WriteLine(
                "#	3. On add or remove of unit type, gfx\\map\\hoi_counter_strip.bmp must be updated to match the number of currently defined unit types.");
            writer.WriteLine("########");
            writer.WriteLine();
            writer.WriteLine("eyr = {");
            writer.WriteLine("	army = EYR_INF	#1");
            writer.WriteLine("	army = EYR_CAV	#2");
            writer.WriteLine("	army = EYR_MOT	#3");
            writer.WriteLine("	army = EYR_MEC	#4");
            writer.WriteLine("	army = EYR_LARM	#5");
            writer.WriteLine("	army = EYR_ARM	#6");
            writer.WriteLine("	army = EYR_PAR	#7");
            writer.WriteLine("	army = EYR_MAR	#8");
            writer.WriteLine("	army = EYR_BER	#9");
            writer.WriteLine("	army = EYR_GAR	#10");
            writer.WriteLine("	army = EYR_HQ	#11");
            writer.WriteLine("	army = EYR_MIL	#12");
            writer.WriteLine("	navy = EYR_CAR	#1");
            writer.WriteLine("	navy = EYR_ECAR	#2");
            writer.WriteLine("	navy = EYR_BAT	#3");
            writer.WriteLine("	navy = EYR_BCRU #4");
            writer.WriteLine("	navy = EYR_HCRU #5");
            writer.WriteLine("	navy = EYR_LCRU #6");
            writer.WriteLine("	navy = EYR_DES	#7");
            writer.WriteLine("	navy = EYR_SUB	#8");
            writer.WriteLine("	navy = EYR_NSUB	#9");
            writer.WriteLine("	navy = EYR_TRA	#10");
            writer.WriteLine("	air  = EYR_MUL	#1");
            writer.WriteLine("	air  = EYR_INT	#2");
            writer.WriteLine("	air  = EYR_CAS	#3");
            writer.WriteLine("	air  = EYR_STR	#4");
            writer.WriteLine("	air  = EYR_TAC	#5");
            writer.WriteLine("	air  = EYR_NAV	#6");
            writer.WriteLine("	air  = EYR_TPL	#7");
            writer.WriteLine("	air  = EYR_FBO	#8");
            writer.WriteLine("	air  = EYR_ROC	#9");
            writer.WriteLine("}");
        }

        /// <summary>
        ///     旅団ユニットクラス定義データをファイルへ書き込む
        /// </summary>
        /// <param name="units">ユニットクラス一覧</param>
        /// <param name="fileName">ファイル名</param>
        internal static void WriteBrigadeTypes(List<UnitClass> units, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                // ヘッダを出力する
                WriteBrigadeTypesHeader(writer);

                // ユニットクラス定義データを順に書き込む
                foreach (UnitType type in Units.BrigadeTypes)
                {
                    UnitClass unit = units[(int) type];

                    // ユーザー定義旅団に定義内容がなければ出力しない
                    if ((unit.Type >= UnitType.Brigade01) && !unit.ExistsEntity() && (unit.Models.Count == 0))
                    {
                        continue;
                    }

                    // ユニットクラス定義データを書き込む
                    WriteBrigadeType(unit, writer);
                }
            }
        }

        /// <summary>
        ///     旅団ユニットクラス定義を書き込む
        /// </summary>
        /// <param name="unit">ユニットクラスデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBrigadeType(UnitClass unit, StreamWriter writer)
        {
            writer.WriteLine();
            if (unit.Type == UnitType.None)
            {
                writer.WriteLine("{0} = {{\t#DO NOT CHANGE!!!", Units.Strings[(int) unit.Type]);
            }
            else if (unit.Type >= UnitType.ReserveBrigade36 && unit.Type <= UnitType.ReserveBrigade40)
            {
                writer.WriteLine("{0} = {{\t# Reserved for use by Darkest Hour Full", Units.Strings[(int) unit.Type]);
            }
            else
            {
                writer.WriteLine("{0} = {{", Units.Strings[(int) unit.Type]);
            }
            writer.WriteLine("\t#ID\t\t\t{0}", Units.UnitNumbers[(int) unit.Type]);
            if (unit.ExistsEntity() || (unit.Models.Count > 0))
            {
                switch (unit.Branch)
                {
                    case Branch.Army:
                        writer.WriteLine("\ttype\t\t= land");
                        break;

                    case Branch.Navy:
                        writer.WriteLine("\ttype\t\t= naval");
                        break;

                    case Branch.Airforce:
                        writer.WriteLine("\ttype\t\t= air");
                        break;
                }
                writer.WriteLine("\tname\t\t= {0}", unit.Name);
                writer.WriteLine("\tshort_name\t= {0}", unit.ShortName);
                writer.WriteLine("\tdesc\t\t= {0}", unit.Desc);
                writer.WriteLine("\tshort_desc\t= {0}", unit.ShortDesc);
                writer.WriteLine("\tvalue\t\t= {0}", DoubleHelper.ToString(unit.Value));
                if (unit.Cag)
                {
                    writer.WriteLine("\tcag\t\t\t= yes");
                }
                if (unit.Escort)
                {
                    writer.WriteLine("\tescort\t\t= yes");
                }
                if (unit.Engineer)
                {
                    writer.WriteLine("\tengineer\t= yes");
                }
            }
            if ((unit.Type != UnitType.None) || (unit.ListPrio != -1))
            {
                writer.WriteLine("\tlist_prio\t= {0}", unit.ListPrio);
            }
            writer.WriteLine("}");
        }

        /// <summary>
        ///     旅団ユニットクラス定義ファイルのヘッダを書き込む
        /// </summary>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteBrigadeTypesHeader(StreamWriter writer)
        {
            writer.WriteLine("# BRIGADE TYPES DEFINITION FILE");
            writer.WriteLine("########");
            writer.WriteLine("# FORMAT");
            writer.WriteLine("#");
            writer.WriteLine(
                "# BrigadeTypeName = { 					# Replace BrigadeTypeName with one of the predefined brigade types. ");
            writer.WriteLine("#	### REQUIRED PARAMETERS ###");
            writer.WriteLine("#	type		= land/air/naval		# Defines the brigade type");
            writer.WriteLine(
                "#	name		= UnitNameID			# Full brigade name string (NAME_ARTILLERY, NAME_ROCKET_ARTILLERY etc.)");
            writer.WriteLine(
                "#	short_name	= ShortUnitNameID		# Short brigade name string (SNAME_ARTILLERY, SNAME_ROCKET_ARTILLERY etc.)");
            writer.WriteLine(
                "#	desc		= LongDescriptionID		# Long brigade description string (LDESC_ARTILLERY, LDESC_ROCKET_ARTILLERY etc.)");
            writer.WriteLine(
                "#	short_desc	= ShortDescriptionID	# Short brigade description string (SDESC_ARTILLERY, SDESC_ROCKET_ARTILLERY etc.)");
            writer.WriteLine(
                "#	value		= X.X(Number)			# Unit military value. Used in military power and other calculations.");
            writer.WriteLine("#	list_prio	= X(Number)				# Priority in lists (tech overview and production pages).");
            writer.WriteLine("#										# Brigades are sorted by groups (land/air/naval). ");
            writer.WriteLine("#										# -1 - do not list that type, 0+ - priority in lists (lowest first)");
            writer.WriteLine("#	### OPTIONAL ###");
            writer.WriteLine("#	cag			= yes					# This brigade type is a CAG. Valid for Naval brigades only");
            writer.WriteLine("#	escort		= yes					# This brigade type is an escort. Valid for Air brigades only");
            writer.WriteLine("#	engineer	= yes					# This brigade type is an engineer. Valid for Land brigades only");
            writer.WriteLine("# }");
            writer.WriteLine("#");
            writer.WriteLine("########");
            writer.WriteLine("# The BrigadeTypeName is predefined (130 types in total) and must be used in this order:");
            writer.WriteLine(
                "#	none, artillery, sp_artillery, rocket_artillery, sp_rct_artillery, anti_tank, tank_destroyer,");
            writer.WriteLine(
                "#	light_armor_brigade, heavy_armor, super_heavy_armor, armored_car, anti_air, police, engineer,");
            writer.WriteLine(
                "#	cag, escort, naval_asw, naval_anti_air_s, naval_radar_s, naval_fire_controll_s, naval_improved_hull_s,");
            writer.WriteLine(
                "#	naval_torpedoes_s, naval_anti_air_l, naval_radar_l, naval_fire_controll_l, naval_improved_hull_l,");
            writer.WriteLine("#	naval_torpedoes_l, cavalry_brigade, sp_anti_air, medium_armor, floatplane,");
            writer.WriteLine("#	light_cag, amph_armor, glider_armor, glider_artillery, super_heavy_artillery, ");
            writer.WriteLine(
                "#	b_rsv_36, b_rsv_37, b_rsv_38, b_rsv_39, b_rsv_40 # Reserved for use by Darkest Hour Full");
            writer.WriteLine(
                "#	b_01, b_02, b_03, b_04, b_05, b_06, b_07, b_08, b_09, b_10, b_11, b_12, b_13, b_14, b_15, b_16, ");
            writer.WriteLine(
                "#	b_17, b_18, b_19, b_20, b_21, b_22, b_23, b_24, b_25, b_26, b_27, b_28, b_29, b_30, b_31, b_32,");
            writer.WriteLine(
                "#	b_33, b_34, b_35, b_36, b_37, b_38, b_39, b_40, b_41, b_42, b_43, b_44, b_45, b_46, b_47, b_48,");
            writer.WriteLine(
                "#	b_49, b_50, b_51, b_52, b_53, b_54, b_55, b_56, b_57, b_58, b_59, b_60, b_61, b_62, b_63, b_64,");
            writer.WriteLine(
                "#	b_65, b_66, b_67, b_68, b_69, b_70, b_71, b_72, b_73, b_74, b_75, b_76, b_77, b_78, b_79, b_80,");
            writer.WriteLine(
                "#	b_81, b_82, b_83, b_84, b_85, b_86, b_87, b_88, b_89, b_90, b_91, b_92, b_93, b_94, b_95, b_96,");
            writer.WriteLine("#	b_97, b_98, b_99");
            writer.WriteLine("########");
            writer.WriteLine("# NOTES:");
            writer.WriteLine("#	1. There should be only one instance of each BrigadeTypeName!!!");
            writer.WriteLine("#	2. No gaps (like defining b_02 without defining b_01) in BrigadeTypeName are allowed!");
            writer.WriteLine(
                "#	3. On add or remove of brigade type, gfx\\interface\\auxiliary.bmp and gfx\\interface\\auxiliarybig.bmp");
            writer.WriteLine("#	   must be updated to match the number of currently defined brigade types.");
            writer.WriteLine("########");
        }

        #endregion
    }
}