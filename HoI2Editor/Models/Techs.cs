using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
using HoI2Editor.Writers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     技術データ群
    /// </summary>
    public static class Techs
    {
        #region フィールド

        /// <summary>
        ///     技術グループリスト
        /// </summary>
        public static List<TechGroup> List = new List<TechGroup>();

        /// <summary>
        ///     研究特性文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, TechSpeciality> SpecialityStringMap = new Dictionary<string, TechSpeciality>();

        /// <summary>
        ///     研究特性画像リスト
        /// </summary>
        public static ImageList SpecialityImages;

        /// <summary>
        ///     研究特性リスト
        /// </summary>
        public static TechSpeciality[] Specialities;

        /// <summary>
        ///     研究機関編集フラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (TechCategory)).Length];

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        #endregion

        #region 定数

        /// <summary>
        ///     技術カテゴリ文字列
        /// </summary>
        public static readonly string[] CategoryStrings =
            {
                "infantry",
                "armor",
                "naval",
                "aircraft",
                "industry",
                "land_doctrines",
                "secret_weapons",
                "naval_doctrines",
                "air_doctrines"
            };

        /// <summary>
        ///     技術カテゴリ名
        /// </summary>
        public static readonly string[] TechCategoryNames =
            {
                "INFANTRY",
                "ARMOR",
                "NAVAL",
                "AIRCRAFT",
                "INDUSTRY",
                "LD",
                "SW",
                "ND",
                "AD"
            };

        /// <summary>
        ///     技術定義ファイル名
        /// </summary>
        private static readonly string[] TechFileNames =
            {
                "infantry_tech.txt",
                "armor_tech.txt",
                "naval_tech.txt",
                "aircraft_tech.txt",
                "industry_tech.txt",
                "land_doctrines_tech.txt",
                "secret_weapons_tech.txt",
                "naval_doctrines_tech.txt",
                "air_doctrines_tech.txt"
            };

        /// <summary>
        ///     研究特性リスト(HoI2)
        /// </summary>
        private static readonly TechSpeciality[] SpecialitiesHoI2 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting
            };

        /// <summary>
        ///     研究特性リスト(DH1.02)
        /// </summary>
        private static readonly TechSpeciality[] SpecialitiesDh102 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16
            };

        /// <summary>
        ///     研究特性リスト(DH1.03)
        /// </summary>
        private static readonly TechSpeciality[] SpecialitiesDh =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.CavalryTactics,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16,
                TechSpeciality.RtUser17,
                TechSpeciality.RtUser18,
                TechSpeciality.RtUser19,
                TechSpeciality.RtUser20,
                TechSpeciality.RtUser21,
                TechSpeciality.RtUser22,
                TechSpeciality.RtUser23,
                TechSpeciality.RtUser24,
                TechSpeciality.RtUser25,
                TechSpeciality.RtUser26,
                TechSpeciality.RtUser27,
                TechSpeciality.RtUser28,
                TechSpeciality.RtUser29,
                TechSpeciality.RtUser30,
                TechSpeciality.RtUser31,
                TechSpeciality.RtUser32,
                TechSpeciality.RtUser33,
                TechSpeciality.RtUser34,
                TechSpeciality.RtUser35,
                TechSpeciality.RtUser36,
                TechSpeciality.RtUser37,
                TechSpeciality.RtUser38,
                TechSpeciality.RtUser39,
                TechSpeciality.RtUser40,
                TechSpeciality.RtUser41,
                TechSpeciality.RtUser42,
                TechSpeciality.RtUser43,
                TechSpeciality.RtUser44,
                TechSpeciality.RtUser45,
                TechSpeciality.RtUser46,
                TechSpeciality.RtUser47,
                TechSpeciality.RtUser48,
                TechSpeciality.RtUser49,
                TechSpeciality.RtUser50,
                TechSpeciality.RtUser51,
                TechSpeciality.RtUser52,
                TechSpeciality.RtUser53,
                TechSpeciality.RtUser54,
                TechSpeciality.RtUser55,
                TechSpeciality.RtUser56,
                TechSpeciality.RtUser57,
                TechSpeciality.RtUser58,
                TechSpeciality.RtUser59,
                TechSpeciality.RtUser60
            };

        /// <summary>
        ///     研究特性文字列
        /// </summary>
        public static readonly string[] SpecialityStrings =
            {
                "",
                "artillery",
                "mechanics",
                "electronics",
                "chemistry",
                "training",
                "general_equipment",
                "rocketry",
                "naval_engineering",
                "aeronautics",
                "nuclear_physics",
                "nuclear_engineering",
                "management",
                "industrial_engineering",
                "mathematics",
                "small_unit_tactics",
                "large_unit_tactics",
                "centralized_execution",
                "decentralized_execution",
                "technical_efficiency",
                "individual_courage",
                "infantry_focus",
                "combined_arms_focus",
                "large_unit_focus",
                "naval_artillery",
                "naval_training",
                "aircraft_testing",
                "fighter_tactics",
                "bomber_tactics",
                "carrier_tactics",
                "submarine_tactics",
                "large_taskforce_tactics",
                "small_taskforce_tactics",
                "seamanship",
                "piloting",
                "avionics",
                "munitions",
                "vehicle_engineering",
                "carrier_design",
                "submarine_design",
                "fighter_design",
                "bomber_design",
                "mountain_training",
                "airborne_training",
                "marine_training",
                "maneuver_tactics",
                "blitzkrieg_tactics",
                "static_defense_tactics",
                "medicine",
                "cavalry_tactics",
                "rt_user_1",
                "rt_user_2",
                "rt_user_3",
                "rt_user_4",
                "rt_user_5",
                "rt_user_6",
                "rt_user_7",
                "rt_user_8",
                "rt_user_9",
                "rt_user_10",
                "rt_user_11",
                "rt_user_12",
                "rt_user_13",
                "rt_user_14",
                "rt_user_15",
                "rt_user_16",
                "rt_user_17",
                "rt_user_18",
                "rt_user_19",
                "rt_user_20",
                "rt_user_21",
                "rt_user_22",
                "rt_user_23",
                "rt_user_24",
                "rt_user_25",
                "rt_user_26",
                "rt_user_27",
                "rt_user_28",
                "rt_user_29",
                "rt_user_30",
                "rt_user_31",
                "rt_user_32",
                "rt_user_33",
                "rt_user_34",
                "rt_user_35",
                "rt_user_36",
                "rt_user_37",
                "rt_user_38",
                "rt_user_39",
                "rt_user_40",
                "rt_user_41",
                "rt_user_42",
                "rt_user_43",
                "rt_user_44",
                "rt_user_45",
                "rt_user_46",
                "rt_user_47",
                "rt_user_48",
                "rt_user_49",
                "rt_user_50",
                "rt_user_51",
                "rt_user_52",
                "rt_user_53",
                "rt_user_54",
                "rt_user_55",
                "rt_user_56",
                "rt_user_57",
                "rt_user_58",
                "rt_user_59",
                "rt_user_60"
            };

        /// <summary>
        ///     研究特性名
        /// </summary>
        public static readonly string[] SpecialityNames =
            {
                "",
                "RT_ARTILLERY",
                "RT_MECHANICS",
                "RT_ELECTRONICS",
                "RT_CHEMISTRY",
                "RT_TRAINING",
                "RT_GENERAL_EQUIPMENT",
                "RT_ROCKETRY",
                "RT_NAVAL_ENGINEERING",
                "RT_AERONAUTICS",
                "RT_NUCLEAR_PHYSICS",
                "RT_NUCLEAR_ENGINEERING",
                "RT_MANAGEMENT",
                "RT_INDUSTRIAL_ENGINEERING",
                "RT_MATHEMATICS",
                "RT_SMALL_UNIT_TACTICS",
                "RT_LARGE_UNIT_TACTICS",
                "RT_CENTRALIZED_EXECUTION",
                "RT_DECENTRALIZED_EXECUTION",
                "RT_TECHNICAL_EFFICIENCY",
                "RT_INDIVIDUAL_COURAGE",
                "RT_INFANTRY_FOCUS",
                "RT_COMBINED_ARMS_FOCUS",
                "RT_LARGE_UNIT_FOCUS",
                "RT_NAVAL_ARTILLERY",
                "RT_NAVAL_TRAINING",
                "RT_AIRCRAFT_TESTING",
                "RT_FIGHTER_TACTICS",
                "RT_BOMBER_TACTICS",
                "RT_CARRIER_TACTICS",
                "RT_SUBMARINE_TACTICS",
                "RT_LARGE_TASKFORCE_TACTICS",
                "RT_SMALL_TASKFORCE_TACTICS",
                "RT_SEAMANSHIP",
                "RT_PILOTING",
                "RT_AVIONICS",
                "RT_MUNITIONS",
                "RT_VEHICLE_ENGINEERING",
                "RT_CARRIER_DESIGN",
                "RT_SUBMARINE_DESIGN",
                "RT_FIGHTER_DESIGN",
                "RT_BOMBER_DESIGN",
                "RT_MOUNTAIN_TRAINING",
                "RT_AIRBORNE_TRAINING",
                "RT_MARINE_TRAINING",
                "RT_MANEUVER_TACTICS",
                "RT_BLITZKRIEG_TACTICS",
                "RT_STATIC_DEFENSE_TACTICS",
                "RT_MEDICINE",
                "RT_CAVALRY_TACTICS",
                "RT_USER_1",
                "RT_USER_2",
                "RT_USER_3",
                "RT_USER_4",
                "RT_USER_5",
                "RT_USER_6",
                "RT_USER_7",
                "RT_USER_8",
                "RT_USER_9",
                "RT_USER_10",
                "RT_USER_11",
                "RT_USER_12",
                "RT_USER_13",
                "RT_USER_14",
                "RT_USER_15",
                "RT_USER_16",
                "RT_USER_17",
                "RT_USER_18",
                "RT_USER_19",
                "RT_USER_20",
                "RT_USER_21",
                "RT_USER_22",
                "RT_USER_23",
                "RT_USER_24",
                "RT_USER_25",
                "RT_USER_26",
                "RT_USER_27",
                "RT_USER_28",
                "RT_USER_29",
                "RT_USER_30",
                "RT_USER_31",
                "RT_USER_32",
                "RT_USER_33",
                "RT_USER_34",
                "RT_USER_35",
                "RT_USER_36",
                "RT_USER_37",
                "RT_USER_38",
                "RT_USER_39",
                "RT_USER_40",
                "RT_USER_41",
                "RT_USER_42",
                "RT_USER_43",
                "RT_USER_44",
                "RT_USER_45",
                "RT_USER_46",
                "RT_USER_47",
                "RT_USER_48",
                "RT_USER_49",
                "RT_USER_50",
                "RT_USER_51",
                "RT_USER_52",
                "RT_USER_53",
                "RT_USER_54",
                "RT_USER_55",
                "RT_USER_56",
                "RT_USER_57",
                "RT_USER_58",
                "RT_USER_59",
                "RT_USER_60"
            };

        /// <summary>
        ///     カテゴリ文字列とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, TechCategory> CategoryMap
            = new Dictionary<string, TechCategory>
                  {
                      {"infantry", TechCategory.Infantry},
                      {"armor", TechCategory.Armor},
                      {"naval", TechCategory.Naval},
                      {"aircraft", TechCategory.Aircraft},
                      {"industry", TechCategory.Industry},
                      {"land_doctrines", TechCategory.LandDoctrines},
                      {"secret_weapons", TechCategory.SecretWeapons},
                      {"naval_doctrines", TechCategory.NavalDoctrines},
                      {"air_doctrines", TechCategory.AirDoctrines},
                  };

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Techs()
        {
            // 研究特性文字列とIDの対応付け
            foreach (TechSpeciality speciality in Enum.GetValues(typeof (TechSpeciality)))
            {
                SpecialityStringMap.Add(SpecialityStrings[(int) speciality], speciality);
            }
        }

        /// <summary>
        ///     研究特性を初期化する
        /// </summary>
        public static void InitSpecialities()
        {
            // 研究特性リストを設定する
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    Specialities = SpecialitiesHoI2;
                    break;

                case GameType.DarkestHour:
                    Specialities = (Game.Version >= 103 ? SpecialitiesDh : SpecialitiesDh102);
                    break;
            }

            // 研究特性画像リストを作成する
            var bitmap = new Bitmap(Game.GetReadFileName(Game.TechIconPathName));
            SpecialityImages = new ImageList
                                   {
                                       ImageSize = new Size(24, 24),
                                       TransparentColor = bitmap.GetPixel(0, 0)
                                   };
            SpecialityImages.Images.AddStrip(bitmap);
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     技術ファイルの再読み込みを要求する
        /// </summary>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     技術定義ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            List.Clear();

            foreach (TechCategory category in Enum.GetValues(typeof (TechCategory)))
            {
                string fileName = Game.GetReadFileName(Game.TechPathName, TechFileNames[(int) category]);
                try
                {
                    // 技術定義ファイルを読み込む
                    LoadFile(fileName);
                }
                catch (Exception)
                {
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                }
            }

            _loaded = true;
        }

        /// <summary>
        ///     技術定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">技術定義ファイル名</param>
        private static void LoadFile(string fileName)
        {
            TechGroup grp = TechParser.Parse(fileName);
            List.Add(grp);
            ResetDirty(grp.Category);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     技術定義ファイル群を保存する
        /// </summary>
        public static void Save()
        {
            string folderName = Game.GetWriteFileName(Game.TechPathName);
            // 技術定義フォルダがなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            foreach (TechGroup grp in List)
            {
                if (DirtyFlags[(int) grp.Category])
                {
                    string fileName = Path.Combine(folderName, TechFileNames[(int) grp.Category]);
                    try
                    {
                        // 技術定義ファイルを保存する
                        TechWriter.Write(grp, fileName);

                        // 編集済みフラグを解除する
                        ResetDirty(grp.Category);
                    }
                    catch (Exception)
                    {
                        Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                    }
                }
            }
        }

        #endregion

        #region 技術項目リスト操作

        /// <summary>
        ///     技術項目リストに項目を挿入する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="item">追加対象の項目</param>
        public static void AddItem(TechCategory category, ITechItem item)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Add(item);
        }

        /// <summary>
        ///     技術項目リストに項目を追加する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="item">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItem(TechCategory category, ITechItem item, ITechItem position)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Insert(grp.Items.IndexOf(position) + 1, item);
        }

        /// <summary>
        ///     技術項目リストから項目を削除する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="item">削除対象の項目</param>
        public static void RemoveItem(TechCategory category, ITechItem item)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Remove(item);

            // 一時キーを削除する
            item.RemoveTempKey();
        }

        /// <summary>
        ///     技術項目リストの項目を移動する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="src">移動対象の項目</param>
        /// <param name="dest">移動先位置の項目</param>
        public static void MoveItem(TechCategory category, ITechItem src, ITechItem dest)
        {
            TechGroup grp = List[(int) category];
            int srcIndex = grp.Items.IndexOf(src);
            int destIndex = grp.Items.IndexOf(dest);

            if (srcIndex > destIndex)
            {
                // 上へ移動する場合
                grp.Items.Insert(destIndex, src);
                grp.Items.RemoveAt(srcIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                grp.Items.Insert(destIndex + 1, src);
                grp.Items.RemoveAt(srcIndex);
            }
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty(TechCategory category)
        {
            return DirtyFlags[(int) category];
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        public static void SetDirty(TechCategory category)
        {
            DirtyFlags[(int) category] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        public static void ResetDirty(TechCategory category)
        {
            DirtyFlags[(int) category] = false;
        }

        #endregion
    }
}