using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    ///     技術データ群
    /// </summary>
    internal static class Techs
    {
        #region 公開プロパティ

        /// <summary>
        ///     技術グループリスト
        /// </summary>
        internal static List<TechGroup> Groups { get; }

        /// <summary>
        ///     技術IDリスト
        /// </summary>
        internal static List<int> TechIds { get; }

        /// <summary>
        ///     技術IDの対応付けテーブル
        /// </summary>
        internal static Dictionary<int, TechItem> TechIdMap { get; }

        /// <summary>
        ///     研究特性リスト
        /// </summary>
        internal static TechSpeciality[] Specialities { get; private set; }

        /// <summary>
        ///     研究特性文字列とIDの対応付け
        /// </summary>
        internal static Dictionary<string, TechSpeciality> SpecialityStringMap { get; }

        /// <summary>
        ///     研究特性画像リスト
        /// </summary>
        internal static ImageList SpecialityImages { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     重複文字列リスト
        /// </summary>
        private static readonly Dictionary<string, int> DuplicatedList = new Dictionary<string, int>();

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     遅延読み込み用
        /// </summary>
        private static readonly BackgroundWorker Worker = new BackgroundWorker();

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     技術カテゴリ文字列
        /// </summary>
        internal static readonly string[] CategoryStrings =
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
        private static readonly string[] CategoryNames =
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
        ///     研究特性文字列
        /// </summary>
        internal static readonly string[] SpecialityStrings =
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
        ///     カテゴリ文字列とIDの対応付け
        /// </summary>
        internal static readonly Dictionary<string, TechCategory> CategoryMap
            = new Dictionary<string, TechCategory>
            {
                { "infantry", TechCategory.Infantry },
                { "armor", TechCategory.Armor },
                { "naval", TechCategory.Naval },
                { "aircraft", TechCategory.Aircraft },
                { "industry", TechCategory.Industry },
                { "land_doctrines", TechCategory.LandDoctrines },
                { "secret_weapons", TechCategory.SecretWeapons },
                { "naval_doctrines", TechCategory.NavalDoctrines },
                { "air_doctrines", TechCategory.AirDoctrines }
            };

        #endregion

        #region 内部定数

        /// <summary>
        ///     技術定義ファイル名
        /// </summary>
        private static readonly string[] FileNames =
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
        ///     研究特性名
        /// </summary>
        private static readonly string[] SpecialityNames =
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

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Techs()
        {
            // 技術グループリスト
            Groups = new List<TechGroup>();

            // 技術IDリスト
            TechIds = new List<int>();

            // 技術IDの対応付け
            TechIdMap = new Dictionary<int, TechItem>();

            // 研究特性文字列とIDの対応付け
            SpecialityStringMap = new Dictionary<string, TechSpeciality>();
            foreach (TechSpeciality speciality in Enum.GetValues(typeof(TechSpeciality)))
            {
                SpecialityStringMap.Add(SpecialityStrings[(int) speciality], speciality);
            }
        }

        /// <summary>
        ///     研究特性を初期化する
        /// </summary>
        internal static void InitSpecialities()
        {
            // 研究特性リストを設定する
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    Specialities = SpecialitiesHoI2;
                    break;

                case GameType.DarkestHour:
                    Specialities = Game.Version >= 103 ? SpecialitiesDh : SpecialitiesDh102;
                    break;
            }

            // 研究特性画像リストを作成する
            Bitmap bitmap = new Bitmap(Game.GetReadFileName(Game.TechIconPathName));
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
        internal static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     技術ファイル群を再読み込みする
        /// </summary>
        internal static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            Load();
        }

        /// <summary>
        ///     技術定義ファイル群を読み込む
        /// </summary>
        internal static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
                return;
            }

            LoadFiles();
        }

        /// <summary>
        ///     技術定義ファイル群を遅延読み込みする
        /// </summary>
        /// <param name="handler">読み込み完了イベントハンドラ</param>
        internal static void LoadAsync(RunWorkerCompletedEventHandler handler)
        {
            // 既に読み込み済みならば完了イベントハンドラを呼び出す
            if (_loaded)
            {
                handler?.Invoke(null, new RunWorkerCompletedEventArgs(null, null, false));
                return;
            }

            // 読み込み完了イベントハンドラを登録する
            if (handler != null)
            {
                Worker.RunWorkerCompleted += handler;
                Worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
            }

            // 読み込み途中ならば戻る
            if (Worker.IsBusy)
            {
                return;
            }

            // ここで読み込み済みならば既に完了イベントハンドラを呼び出しているので何もせずに戻る
            if (_loaded)
            {
                return;
            }

            // 遅延読み込みを開始する
            Worker.DoWork += OnWorkerDoWork;
            Worker.RunWorkerAsync();
        }

        /// <summary>
        ///     読み込み完了まで待機する
        /// </summary>
        internal static void WaitLoading()
        {
            while (Worker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     遅延読み込み中かどうかを判定する
        /// </summary>
        /// <returns>遅延読み込み中ならばtrueを返す</returns>
        internal static bool IsLoading()
        {
            return Worker.IsBusy;
        }

        /// <summary>
        ///     遅延読み込み処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            LoadFiles();
        }

        /// <summary>
        ///     遅延読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 遅延読み込み完了時の処理
            HoI2EditorApplication.Instance.OnLoadingCompleted();
        }

        /// <summary>
        ///     技術定義ファイル群を読み込む
        /// </summary>
        private static void LoadFiles()
        {
            // コマンドの初期化
            Commands.Init();

            Groups.Clear();

            bool error = false;
            foreach (TechCategory category in Enum.GetValues(typeof(TechCategory)))
            {
                string fileName = FileNames[(int) category];
                string pathName = Game.GetReadFileName(Game.TechPathName, fileName);
                try
                {
                    // 技術定義ファイルを読み込む
                    LoadFile(pathName);
                }
                catch (Exception)
                {
                    error = true;
                    Log.Error("[Tech] Read error: {0}", pathName);
                    if (MessageBox.Show($"{Resources.FileReadError}: {pathName}",
                        Resources.EditorTech, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }

            // 技術IDの対応付けを更新する
            UpdateTechIdMap();

            // 重複文字列リストを更新する
            UpdateDuplicatedList();

            // リンクの切れた一時キーをリストに登録する
            AddUnlinkedTempKey();

            // 読み込みに失敗していれば戻る
            if (error)
            {
                return;
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     技術定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">技術定義ファイル名</param>
        private static void LoadFile(string fileName)
        {
            Log.Verbose("[Tech] Load: {0}", Path.GetFileName(fileName));

            TechGroup grp = TechParser.Parse(fileName);
            if (grp == null)
            {
                Log.Error("[Tech] Read error: {0}", Path.GetFileName(fileName));
                return;
            }
            Groups.Add(grp);
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     技術定義ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        internal static bool Save()
        {
            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
            }

            if (IsDirty())
            {
                string folderName = Game.GetWriteFileName(Game.TechPathName);
                try
                {
                    // 技術定義フォルダがなければ作成する
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                }
                catch (Exception)
                {
                    Log.Error("[Tech] Write error: {0}", folderName);
                    MessageBox.Show($"{Resources.FileWriteError}: {folderName}",
                        Resources.EditorTech, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return false;
                }

                bool error = false;
                foreach (TechGroup grp in Groups.Where(grp => grp.IsDirty()))
                {
                    string fileName = Path.Combine(folderName, FileNames[(int) grp.Category]);
                    try
                    {
                        // 技術定義ファイルを保存する
                        Log.Info("[Tech] Save: {0}", Path.GetFileName(fileName));
                        TechWriter.Write(grp, fileName);
                    }
                    catch (Exception)
                    {
                        error = true;
                        Log.Error("[Tech] Write error: {0}", fileName);
                        if (MessageBox.Show($"{Resources.FileWriteError}: {fileName}",
                            Resources.EditorTech, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            == DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                }

                // 保存に失敗していれば戻る
                if (error)
                {
                    return false;
                }

                // 編集済みフラグを解除する
                _dirtyFlag = false;
            }

            if (_loaded)
            {
                // 文字列定義のみ保存の場合、技術名などの編集済みフラグがクリアされないためここで全クリアする
                foreach (TechGroup grp in Groups)
                {
                    grp.ResetDirtyAll();
                }
            }

            return true;
        }

        /// <summary>
        ///     文字列キーを保存形式に変更する
        /// </summary>
        internal static void RenameKeys()
        {
            foreach (TechGroup grp in Groups)
            {
                string categoryName = CategoryNames[(int) grp.Category];
                bool dirty = false;

                // 技術
                List<int> list = new List<int>();
                foreach (TechItem item in grp.Items.OfType<TechItem>())
                {
                    item.AddKeyNumbers(list);
                }
                foreach (TechItem item in grp.Items.OfType<TechItem>())
                {
                    if (item.RenameKeys(categoryName, list))
                    {
                        dirty = true;
                    }
                }

                // ラベル
                list = new List<int>();
                foreach (TechLabel item in grp.Items.OfType<TechLabel>())
                {
                    item.AddKeyNumbers(list);
                }
                foreach (TechLabel item in grp.Items.OfType<TechLabel>())
                {
                    if (item.RenameKeys(categoryName, list))
                    {
                        dirty = true;
                    }
                }

                // 編集済みフラグを更新する
                if (dirty)
                {
                    grp.SetDirty();
                }
            }
        }

        #endregion

        #region 技術項目とIDの対応付け

        /// <summary>
        ///     技術IDを変更する
        /// </summary>
        /// <param name="item">技術項目</param>
        /// <param name="id">技術ID</param>
        internal static void ModifyTechId(TechItem item, int id)
        {
            // 値の変更前に技術項目とIDの対応付けを削除する
            TechIds.Remove(id);
            TechIdMap.Remove(id);

            // 値を更新する
            item.Id = id;

            // 技術項目とIDの対応付けを更新する
            UpdateTechIdMap();
        }

        /// <summary>
        ///     技術項目とIDの対応付けを更新する
        /// </summary>
        internal static void UpdateTechIdMap()
        {
            TechIds.Clear();
            TechIdMap.Clear();
            foreach (TechItem item in Groups.SelectMany(grp => grp.Items.OfType<TechItem>()))
            {
                if (!TechIds.Contains(item.Id))
                {
                    TechIds.Add(item.Id);
                    TechIdMap.Add(item.Id, item);
                }
            }
        }

        /// <summary>
        ///     未使用の技術IDを取得する
        /// </summary>
        /// <param name="startId">検索を開始するID</param>
        /// <returns>未使用の技術ID</returns>
        internal static int GetNewId(int startId)
        {
            int id = startId;
            while (TechIds.Contains(id))
            {
                id += 10;
            }
            return id;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列の定義名が重複しているかを取得する
        /// </summary>
        /// <param name="name">対象の文字列定義名</param>
        /// <returns>定義名が重複していればtrueを返す</returns>
        internal static bool IsDuplicatedName(string name)
        {
            return DuplicatedList.ContainsKey(name) && (DuplicatedList[name] > 1);
        }

        /// <summary>
        ///     重複文字列リストを更新する
        /// </summary>
        private static void UpdateDuplicatedList()
        {
            DuplicatedList.Clear();
            foreach (ITechItem item in Groups.SelectMany(grp => grp.Items))
            {
                AddDuplicatedListItem(item);
            }
        }

        /// <summary>
        ///     重複文字列リストに項目を追加する
        /// </summary>
        /// <param name="item">技術項目</param>
        internal static void AddDuplicatedListItem(ITechItem item)
        {
            TechItem techItem = item as TechItem;
            if (techItem != null)
            {
                IncrementDuplicatedListCount(techItem.Name);
                IncrementDuplicatedListCount(techItem.ShortName);
                IncrementDuplicatedListCount(techItem.Desc);
                foreach (TechComponent component in techItem.Components)
                {
                    IncrementDuplicatedListCount(component.Name);
                }
                return;
            }

            TechLabel labelItem = item as TechLabel;
            if (labelItem != null)
            {
                IncrementDuplicatedListCount(labelItem.Name);
            }
        }

        /// <summary>
        ///     重複文字列リストの項目を削除する
        /// </summary>
        /// <param name="item">技術項目</param>
        internal static void RemoveDuplicatedListItem(ITechItem item)
        {
            TechItem techItem = item as TechItem;
            if (techItem != null)
            {
                DecrementDuplicatedListCount(techItem.Name);
                DecrementDuplicatedListCount(techItem.ShortName);
                DecrementDuplicatedListCount(techItem.Desc);
                foreach (TechComponent component in techItem.Components)
                {
                    DecrementDuplicatedListCount(component.Name);
                }
                return;
            }

            TechLabel labelItem = item as TechLabel;
            if (labelItem != null)
            {
                DecrementDuplicatedListCount(labelItem.Name);
            }
        }

        /// <summary>
        ///     重複文字列リストのカウントをインクリメントする
        /// </summary>
        /// <param name="name">対象の文字列定義名</param>
        internal static void IncrementDuplicatedListCount(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (!DuplicatedList.ContainsKey(name))
            {
                DuplicatedList.Add(name, 1);
            }
            else
            {
                DuplicatedList[name]++;
                Log.Info("[Tech] Incremented duplicated list: {0} {1}", name, DuplicatedList[name]);
            }
        }

        /// <summary>
        ///     重複文字列リストのカウントをデクリメントする
        /// </summary>
        /// <param name="name">対象の文字列定義名</param>
        internal static void DecrementDuplicatedListCount(string name)
        {
            if (!string.IsNullOrEmpty(name) && DuplicatedList.ContainsKey(name))
            {
                DuplicatedList[name]--;
                if (DuplicatedList[name] == 0)
                {
                    DuplicatedList.Remove(name);
                }
                else
                {
                    Log.Info("[Tech] Decremented duplicated list: {0} {1}", name, DuplicatedList[name]);
                }
            }
        }

        /// <summary>
        ///     リンクの切れた一時キーをリストに登録する
        /// </summary>
        private static void AddUnlinkedTempKey()
        {
            foreach (ITechItem item in Groups.SelectMany(grp => grp.Items))
            {
                if (item is TechItem)
                {
                    TechItem techItem = item as TechItem;
                    if (Config.IsTempKey(techItem.Name))
                    {
                        Config.AddTempKey(techItem.Name);
                    }
                    if (Config.IsTempKey(techItem.ShortName))
                    {
                        Config.AddTempKey(techItem.ShortName);
                    }
                    if (Config.IsTempKey(techItem.Desc))
                    {
                        Config.AddTempKey(techItem.Desc);
                    }
                    foreach (TechComponent component in techItem.Components)
                    {
                        if (Config.IsTempKey(component.Name))
                        {
                            Config.AddTempKey(component.Name);
                        }
                    }
                }
                else if (item is TechLabel)
                {
                    TechLabel labelItem = item as TechLabel;
                    if (Config.IsTempKey(labelItem.Name))
                    {
                        Config.AddTempKey(labelItem.Name);
                    }
                }
            }
        }

        /// <summary>
        ///     研究特性名を取得する
        /// </summary>
        /// <param name="speciality">研究特性</param>
        /// <returns>研究特性名</returns>
        internal static string GetSpecialityName(TechSpeciality speciality)
        {
            string name = SpecialityNames[(int) speciality];
            return Config.ExistsKey(name) ? Config.GetText(name) : name;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal static bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        internal static void SetDirty()
        {
            _dirtyFlag = true;
        }

        #endregion
    }
}