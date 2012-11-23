namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ
    /// </summary>
    public class Team
    {
        /// <summary>
        ///     研究特性
        /// </summary>
        public const int SpecialityLength = 32;

        /// <summary>
        ///     研究特性文字列
        /// </summary>
        public static readonly string[] SpecialityStringTable =
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
        public static readonly string[] SpecialityNameTable =
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
        ///     研究特性
        /// </summary>
        private readonly TechSpeciality[] _specialities = new TechSpeciality[SpecialityLength];

        /// <summary>
        ///     国タグ
        /// </summary>
        public CountryTag CountryTag { get; set; }

        /// <summary>
        ///     研究機関ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     研究特性
        /// </summary>
        public TechSpeciality[] Specialities
        {
            get { return _specialities; }
        }
    }

    /// <summary>
    ///     研究特性
    /// </summary>
    public enum TechSpeciality
    {
        None,

        // 共通
        Artillery, // 火砲
        Mechanics, // 機械工学
        Electronics, // 電子工学
        Chemistry, // 化学
        Training, // 訓練
        GeneralEquipment, // 一般装備
        Rocketry, // ロケット工学
        NavalEngineering, // 海軍工学
        Aeronautics, // 航空学
        NuclearPhysics, // 核物理学
        NuclearEngineering, // 核工学
        Management, // 管理
        IndustrialEngineering, // 産業工学
        Mathematics, // 数学
        SmallUnitTactics, // 小規模部隊戦術
        LargeUnitTactics, // 大規模部隊戦術
        CentralizedExecution, // 集中実行
        DecentralizedExecution, // 分散実行
        TechnicalEfficiency, // 技術効率
        IndividualCourage, // 各自の勇気
        InfantryFocus, // 歩兵重視
        CombinedArmsFocus, // 諸兵科連合部隊重視
        LargeUnitFocus, // 大規模部隊重視
        NavalArtillery, // 艦砲
        NavalTraining, // 海軍訓練
        AircraftTesting, // 航空機試験
        FighterTactics, // 戦闘機戦術
        BomberTactics, // 爆撃機戦術
        CarrierTactics, // 空母戦術
        SubmarineTactics, // 潜水艦戦術
        LargeTaskforceTactics, // 大規模機動部隊戦術
        SmallTaskforceTactics, // 小規模機動部隊戦術
        Seamanship, // 操船術
        Piloting, // 沿岸航法

        // DHのみ
        Avionics, // 航空電子工学
        Munitions, // 弾薬
        VehicleEngineering, // 車両工学
        CarrierDesign, // 空母設計
        SubmarineDesign, // 潜水艦設計
        FighterDesign, // 戦闘機設計
        BomberDesign, // 爆撃機設計
        MountainTraining, // 山岳訓練
        AirborneTraining, // 空挺訓練
        MarineTraining, // 海兵訓練
        ManeuverTactics, // 機動戦術
        BlitzkriegTactics, // 電撃戦戦術
        StaticDefenseTactics, // 静的防衛戦術
        Medicine, // 医療科学
        CavalryTactics, // 騎兵戦術(DH1.03以降のみ)
        RtUser1,
        RtUser2,
        RtUser3,
        RtUser4,
        RtUser5,
        RtUser6,
        RtUser7,
        RtUser8,
        RtUser9,
        RtUser10,
        RtUser11,
        RtUser12,
        RtUser13,
        RtUser14,
        RtUser15,
        RtUser16,
        RtUser17, // 以降DH1.03以降のみ
        RtUser18,
        RtUser19,
        RtUser20,
        RtUser21,
        RtUser22,
        RtUser23,
        RtUser24,
        RtUser25,
        RtUser26,
        RtUser27,
        RtUser28,
        RtUser29,
        RtUser30,
        RtUser31,
        RtUser32,
        RtUser33,
        RtUser34,
        RtUser35,
        RtUser36,
        RtUser37,
        RtUser38,
        RtUser39,
        RtUser40,
        RtUser41,
        RtUser42,
        RtUser43,
        RtUser44,
        RtUser45,
        RtUser46,
        RtUser47,
        RtUser48,
        RtUser49,
        RtUser50,
        RtUser51,
        RtUser52,
        RtUser53,
        RtUser54,
        RtUser55,
        RtUser56,
        RtUser57,
        RtUser58,
        RtUser59,
        RtUser60,
    }
}