using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
using HoI2Editor.Writers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ユニットデータ群
    /// </summary>
    public static class Units
    {
        #region 公開プロパティ

        /// <summary>
        ///     ユニット一覧
        /// </summary>
        public static List<Unit> Items { get; private set; }

        /// <summary>
        ///     ユニット種類文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, UnitType> StringMap { get; private set; }

        /// <summary>
        ///     実ユニット種類文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, RealUnitType> RealStringMap { get; private set; }

        /// <summary>
        ///     スプライト種類文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, SpriteType> SpriteStringMap { get; private set; }

        /// <summary>
        ///     装備文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, EquipmentType> EquipmentStringMap { get; private set; }

        /// <summary>
        ///     利用可能なユニット種類
        /// </summary>
        public static List<UnitType> UnitTypes { get; private set; }

        /// <summary>
        ///     利用可能な師団ユニット種類
        /// </summary>
        public static UnitType[] DivisionTypes { get; private set; }

        /// <summary>
        ///     利用可能な旅団ユニット種類
        /// </summary>
        public static UnitType[] BrigadeTypes { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     師団ユニットクラス定義ファイルの編集済みフラグ
        /// </summary>
        private static bool _divisionTypesDirty;

        /// <summary>
        ///     旅団ユニットクラス定義ファイルの編集済みフラグ
        /// </summary>
        private static bool _brigadeTypesDirty;

        /// <summary>
        ///     国家ごとのモデル名編集済みフラグ
        /// </summary>
        private static readonly bool[] CountryNameDirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     ユニット名種類ごとのモデル名編集済みフラグ
        /// </summary>
        private static readonly bool[,] TypeNameDirtyFlags =
            new bool[Enum.GetValues(typeof (Country)).Length, Enum.GetValues(typeof (UnitType)).Length];

        #endregion

        #region 公開定数

        /// <summary>
        ///     ユニット種類文字列テーブル
        /// </summary>
        public static readonly string[] Strings =
        {
            "infantry",
            "cavalry",
            "motorized",
            "mechanized",
            "light_armor",
            "armor",
            "paratrooper",
            "marine",
            "bergsjaeger",
            "garrison",
            "hq",
            "militia",
            "multi_role",
            "interceptor",
            "strategic_bomber",
            "tactical_bomber",
            "naval_bomber",
            "cas",
            "transport_plane",
            "flying_bomb",
            "flying_rocket",
            "battleship",
            "light_cruiser",
            "heavy_cruiser",
            "battlecruiser",
            "destroyer",
            "carrier",
            "escort_carrier",
            "submarine",
            "nuclear_submarine",
            "transport",
            "light_carrier",
            "rocket_interceptor",
            "d_rsv_33",
            "d_rsv_34",
            "d_rsv_35",
            "d_rsv_36",
            "d_rsv_37",
            "d_rsv_38",
            "d_rsv_39",
            "d_rsv_40",
            "d_01",
            "d_02",
            "d_03",
            "d_04",
            "d_05",
            "d_06",
            "d_07",
            "d_08",
            "d_09",
            "d_10",
            "d_11",
            "d_12",
            "d_13",
            "d_14",
            "d_15",
            "d_16",
            "d_17",
            "d_18",
            "d_19",
            "d_20",
            "d_21",
            "d_22",
            "d_23",
            "d_24",
            "d_25",
            "d_26",
            "d_27",
            "d_28",
            "d_29",
            "d_30",
            "d_31",
            "d_32",
            "d_33",
            "d_34",
            "d_35",
            "d_36",
            "d_37",
            "d_38",
            "d_39",
            "d_40",
            "d_41",
            "d_42",
            "d_43",
            "d_44",
            "d_45",
            "d_46",
            "d_47",
            "d_48",
            "d_49",
            "d_50",
            "d_51",
            "d_52",
            "d_53",
            "d_54",
            "d_55",
            "d_56",
            "d_57",
            "d_58",
            "d_59",
            "d_60",
            "d_61",
            "d_62",
            "d_63",
            "d_64",
            "d_65",
            "d_66",
            "d_67",
            "d_68",
            "d_69",
            "d_70",
            "d_71",
            "d_72",
            "d_73",
            "d_74",
            "d_75",
            "d_76",
            "d_77",
            "d_78",
            "d_79",
            "d_80",
            "d_81",
            "d_82",
            "d_83",
            "d_84",
            "d_85",
            "d_86",
            "d_87",
            "d_88",
            "d_89",
            "d_90",
            "d_91",
            "d_92",
            "d_93",
            "d_94",
            "d_95",
            "d_96",
            "d_97",
            "d_98",
            "d_99",
            "none",
            "artillery",
            "sp_artillery",
            "rocket_artillery",
            "sp_rct_artillery",
            "anti_tank",
            "tank_destroyer",
            "light_armor_brigade",
            "heavy_armor",
            "super_heavy_armor",
            "armored_car",
            "anti_air",
            "police",
            "engineer",
            "cag",
            "escort",
            "naval_asw",
            "naval_anti_air_s",
            "naval_radar_s",
            "naval_fire_controll_s",
            "naval_improved_hull_s",
            "naval_torpedoes_s",
            "naval_anti_air_l",
            "naval_radar_l",
            "naval_fire_controll_l",
            "naval_improved_hull_l",
            "naval_torpedoes_l",
            "naval_mines",
            "naval_sa_l",
            "naval_spotter_l",
            "naval_spotter_s",
            "b_u1",
            "b_u2",
            "b_u3",
            "b_u4",
            "b_u5",
            "b_u6",
            "b_u7",
            "b_u8",
            "b_u9",
            "b_u10",
            "b_u11",
            "b_u12",
            "b_u13",
            "b_u14",
            "b_u15",
            "b_u16",
            "b_u17",
            "b_u18",
            "b_u19",
            "b_u20",
            "cavalry_brigade",
            "sp_anti_air",
            "medium_armor",
            "floatplane",
            "light_cag",
            "amph_armor",
            "glider_armor",
            "glider_artillery",
            "super_heavy_artillery",
            "b_rsv_36",
            "b_rsv_37",
            "b_rsv_38",
            "b_rsv_39",
            "b_rsv_40",
            "b_01",
            "b_02",
            "b_03",
            "b_04",
            "b_05",
            "b_06",
            "b_07",
            "b_08",
            "b_09",
            "b_10",
            "b_11",
            "b_12",
            "b_13",
            "b_14",
            "b_15",
            "b_16",
            "b_17",
            "b_18",
            "b_19",
            "b_20",
            "b_21",
            "b_22",
            "b_23",
            "b_24",
            "b_25",
            "b_26",
            "b_27",
            "b_28",
            "b_29",
            "b_30",
            "b_31",
            "b_32",
            "b_33",
            "b_34",
            "b_35",
            "b_36",
            "b_37",
            "b_38",
            "b_39",
            "b_40",
            "b_41",
            "b_42",
            "b_43",
            "b_44",
            "b_45",
            "b_46",
            "b_47",
            "b_48",
            "b_49",
            "b_50",
            "b_51",
            "b_52",
            "b_53",
            "b_54",
            "b_55",
            "b_56",
            "b_57",
            "b_58",
            "b_59",
            "b_60",
            "b_61",
            "b_62",
            "b_63",
            "b_64",
            "b_65",
            "b_66",
            "b_67",
            "b_68",
            "b_69",
            "b_70",
            "b_71",
            "b_72",
            "b_73",
            "b_74",
            "b_75",
            "b_76",
            "b_77",
            "b_78",
            "b_79",
            "b_80",
            "b_81",
            "b_82",
            "b_83",
            "b_84",
            "b_85",
            "b_86",
            "b_87",
            "b_88",
            "b_89",
            "b_90",
            "b_91",
            "b_92",
            "b_93",
            "b_94",
            "b_95",
            "b_96",
            "b_97",
            "b_98",
            "b_99"
        };

        /// <summary>
        ///     実ユニット文字列
        /// </summary>
        public static readonly string[] RealStrings =
        {
            "infantry",
            "cavalry",
            "motorized",
            "mechanized",
            "light_armor",
            "armor",
            "garrison",
            "hq",
            "paratrooper",
            "marine",
            "bergsjaeger",
            "cas",
            "multi_role",
            "interceptor",
            "strategic_bomber",
            "tactical_bomber",
            "naval_bomber",
            "transport_plane",
            "battleship",
            "light_cruiser",
            "heavy_cruiser",
            "battlecruiser",
            "destroyer",
            "carrier",
            "submarine",
            "transport",
            "flying_bomb",
            "flying_rocket",
            "militia",
            "escort_carrier",
            "nuclear_submarine"
        };

        /// <summary>
        ///     スプライト種類文字列
        /// </summary>
        public static readonly string[] SpriteStrings =
        {
            "infantry",
            "cavalry",
            "motorized",
            "mechanized",
            "l_panzer",
            "panzer",
            "paratrooper",
            "marine",
            "bergsjaeger",
            "fighter",
            "escort",
            "interceptor",
            "bomber",
            "tactical",
            "cas",
            "naval",
            "transportplane",
            "battleship",
            "battlecruiser",
            "heavy_cruiser",
            "light_cruiser",
            "destroyer",
            "carrier",
            "submarine",
            "transport",
            "militia",
            "garrison",
            "hq",
            "flying_bomb",
            "rocket",
            "nuclear_submarine",
            "escort_carrier",
            "light_carrier",
            "rocket_interceptor",
            "d_rsv_33",
            "d_rsv_34",
            "d_rsv_35",
            "d_rsv_36",
            "d_rsv_37",
            "d_rsv_38",
            "d_rsv_39",
            "d_rsv_40",
            "d_01",
            "d_02",
            "d_03",
            "d_04",
            "d_05",
            "d_06",
            "d_07",
            "d_08",
            "d_09",
            "d_10",
            "d_11",
            "d_12",
            "d_13",
            "d_14",
            "d_15",
            "d_16",
            "d_17",
            "d_18",
            "d_19",
            "d_20",
            "d_21",
            "d_22",
            "d_23",
            "d_24",
            "d_25",
            "d_26",
            "d_27",
            "d_28",
            "d_29",
            "d_30",
            "d_31",
            "d_32",
            "d_33",
            "d_34",
            "d_35",
            "d_36",
            "d_37",
            "d_38",
            "d_39",
            "d_40",
            "d_41",
            "d_42",
            "d_43",
            "d_44",
            "d_45",
            "d_46",
            "d_47",
            "d_48",
            "d_49",
            "d_50",
            "d_51",
            "d_52",
            "d_53",
            "d_54",
            "d_55",
            "d_56",
            "d_57",
            "d_58",
            "d_59",
            "d_60",
            "d_61",
            "d_62",
            "d_63",
            "d_64",
            "d_65",
            "d_66",
            "d_67",
            "d_68",
            "d_69",
            "d_70",
            "d_71",
            "d_72",
            "d_73",
            "d_74",
            "d_75",
            "d_76",
            "d_77",
            "d_78",
            "d_79",
            "d_80",
            "d_81",
            "d_82",
            "d_83",
            "d_84",
            "d_85",
            "d_86",
            "d_87",
            "d_88",
            "d_89",
            "d_90",
            "d_91",
            "d_92",
            "d_93",
            "d_94",
            "d_95",
            "d_96",
            "d_97",
            "d_98",
            "d_99"
        };

        /// <summary>
        ///     装備文字列
        /// </summary>
        public static readonly string[] EquipmentStrings =
        {
            "manpower",
            "equipment",
            "artillery",
            "heavy_artillery",
            "anti_air",
            "anti_tank",
            "horses",
            "trucks",
            "halftracks",
            "armored_car",
            "light_armor",
            "medium_armor",
            "heavy_armor",
            "tank_destroyer",
            "sp_artillery",
            "fighter",
            "heavy_fighter",
            "rocket_interceptor",
            "bomber",
            "heavy_bomber",
            "transport_plane",
            "floatplane",
            "helicopter",
            "rocket",
            "balloon",
            "transports",
            "escorts",
            "transport",
            "battleship",
            "battlecruiser",
            "heavy_cruiser",
            "carrier",
            "escort_carrier",
            "light_cruiser",
            "destroyer",
            "submarine",
            "nuclear_submarine"
        };

        /// <summary>
        ///     実ユニット名
        /// </summary>
        public static readonly string[] RealNames =
        {
            "NAME_INFANTRY",
            "NAME_CAVALRY",
            "NAME_MOTORIZED",
            "NAME_MECHANIZED",
            "NAME_LIGHT_ARMOR",
            "NAME_ARMOR",
            "NAME_GARRISON",
            "NAME_HQ",
            "NAME_PARATROOPER",
            "NAME_MARINE",
            "NAME_BERGSJAEGER",
            "NAME_CAS",
            "NAME_MULTI_ROLE",
            "NAME_INTERCEPTOR",
            "NAME_STRATEGIC_BOMBER",
            "NAME_TACTICAL_BOMBER",
            "NAME_NAVAL_BOMBER",
            "NAME_TRANSPORT_PLANE",
            "NAME_BATTLESHIP",
            "NAME_LIGHT_CRUISER",
            "NAME_HEAVY_CRUISER",
            "NAME_BATTLECRUISER",
            "NAME_DESTROYER",
            "NAME_CARRIER",
            "NAME_SUBMARINE",
            "NAME_TRANSPORT",
            "NAME_FLYING_BOMB",
            "NAME_FLYING_ROCKET",
            "NAME_MILITIA",
            "NAME_ESCORT_CARRIER",
            "NAME_NUCLEAR_SUBMARINE"
        };

        /// <summary>
        ///     スプライト種類名
        /// </summary>
        public static readonly string[] SpriteNames =
        {
            "NAME_INFANTRY",
            "NAME_CAVALRY",
            "NAME_MOTORIZED",
            "NAME_MECHANIZED",
            "NAME_LIGHT_ARMOR",
            "NAME_ARMOR",
            "NAME_PARATROOPER",
            "NAME_MARINE",
            "NAME_BERGSJAEGER",
            "NAME_MULTI_ROLE",
            "NAME_ESCORT",
            "NAME_INTERCEPTOR",
            "NAME_STRATEGIC_BOMBER",
            "NAME_TACTICAL_BOMBER",
            "NAME_CAS",
            "NAME_NAVAL_BOMBER",
            "NAME_TRANSPORT_PLANE",
            "NAME_BATTLESHIP",
            "NAME_BATTLECRUISER",
            "NAME_HEAVY_CRUISER",
            "NAME_LIGHT_CRUISER",
            "NAME_DESTROYER",
            "NAME_CARRIER",
            "NAME_SUBMARINE",
            "NAME_TRANSPORT",
            "NAME_MILITIA",
            "NAME_GARRISON",
            "NAME_HQ",
            "NAME_FLYING_BOMB",
            "NAME_FLYING_ROCKET",
            "NAME_NUCLEAR_SUBMARINE",
            "NAME_ESCORT_CARRIER",
            "NAME_LIGHT_CARRIER",
            "NAME_ROCKET_INTERCEPTOR",
            "NAME_D_RSV_33",
            "NAME_D_RSV_34",
            "NAME_D_RSV_35",
            "NAME_D_RSV_36",
            "NAME_D_RSV_37",
            "NAME_D_RSV_38",
            "NAME_D_RSV_39",
            "NAME_D_RSV_40",
            "NAME_D_01",
            "NAME_D_02",
            "NAME_D_03",
            "NAME_D_04",
            "NAME_D_05",
            "NAME_D_06",
            "NAME_D_07",
            "NAME_D_08",
            "NAME_D_09",
            "NAME_D_10",
            "NAME_D_11",
            "NAME_D_12",
            "NAME_D_13",
            "NAME_D_14",
            "NAME_D_15",
            "NAME_D_16",
            "NAME_D_17",
            "NAME_D_18",
            "NAME_D_19",
            "NAME_D_20",
            "NAME_D_21",
            "NAME_D_22",
            "NAME_D_23",
            "NAME_D_24",
            "NAME_D_25",
            "NAME_D_26",
            "NAME_D_27",
            "NAME_D_28",
            "NAME_D_29",
            "NAME_D_30",
            "NAME_D_31",
            "NAME_D_32",
            "NAME_D_33",
            "NAME_D_34",
            "NAME_D_35",
            "NAME_D_36",
            "NAME_D_37",
            "NAME_D_38",
            "NAME_D_39",
            "NAME_D_40",
            "NAME_D_41",
            "NAME_D_42",
            "NAME_D_43",
            "NAME_D_44",
            "NAME_D_45",
            "NAME_D_46",
            "NAME_D_47",
            "NAME_D_48",
            "NAME_D_49",
            "NAME_D_50",
            "NAME_D_51",
            "NAME_D_52",
            "NAME_D_53",
            "NAME_D_54",
            "NAME_D_55",
            "NAME_D_56",
            "NAME_D_57",
            "NAME_D_58",
            "NAME_D_59",
            "NAME_D_60",
            "NAME_D_61",
            "NAME_D_62",
            "NAME_D_63",
            "NAME_D_64",
            "NAME_D_65",
            "NAME_D_66",
            "NAME_D_67",
            "NAME_D_68",
            "NAME_D_69",
            "NAME_D_70",
            "NAME_D_71",
            "NAME_D_72",
            "NAME_D_73",
            "NAME_D_74",
            "NAME_D_75",
            "NAME_D_76",
            "NAME_D_77",
            "NAME_D_78",
            "NAME_D_79",
            "NAME_D_80",
            "NAME_D_81",
            "NAME_D_82",
            "NAME_D_83",
            "NAME_D_84",
            "NAME_D_85",
            "NAME_D_86",
            "NAME_D_87",
            "NAME_D_88",
            "NAME_D_89",
            "NAME_D_90",
            "NAME_D_91",
            "NAME_D_92",
            "NAME_D_93",
            "NAME_D_94",
            "NAME_D_95",
            "NAME_D_96",
            "NAME_D_97",
            "NAME_D_98",
            "NAME_D_99"
        };

        /// <summary>
        ///     装備名
        /// </summary>
        public static readonly string[] EquipmentNames =
        {
            "EQ_MANPOWER",
            "EQ_EQUIPMENT",
            "EQ_ARTILLERY",
            "EQ_ANTI_AIR",
            "EQ_ANTI_TANK",
            "EQ_HEAVY_ARTILLERY",
            "EQ_HORSES",
            "EQ_TRUCKS",
            "EQ_HALFTRACKS",
            "EQ_ARMORED_CARS",
            "EQ_LIGHT_ARMOR",
            "EQ_MEDIUM_ARMOR",
            "EQ_HEAVY_ARMOR",
            "EQ_TANK_DESTROYER",
            "EQ_SP_ARTILLERY",
            "EQ_FIGHTERS",
            "EQ_HEAVY_FIGHTERS",
            "EQ_ROCKET_INTERCEPTORS",
            "EQ_BOMBERS",
            "EQ_HEAVY_BOMBERS",
            "EQ_TRANSPORT_PLANES",
            "EQ_FLOATPLANE",
            "EQ_HELICOPTERS",
            "EQ_ROCKETS",
            "EQ_BALLOONS",
            "EQ_CONV_TRANS",
            "EQ_CONV_ESC",
            "EQ_TRANSPORT",
            "EQ_BATTLESHIP",
            "EQ_BATTLECRUISER",
            "EQ_HEAVY_CRUISER",
            "EQ_CARRIER",
            "EQ_ESCORT_CARRIER",
            "EQ_LIGHT_CRUISER",
            "EQ_DESTROYER",
            "EQ_SUBMARINE",
            "EQ_NUCLEAR_SUBMARINE"
        };

        /// <summary>
        ///     ユニット番号の初期設定値
        /// </summary>
        public static readonly int[] UnitNumbers =
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            55,
            56,
            57,
            58,
            59,
            60,
            61,
            62,
            63,
            64,
            65,
            66,
            67,
            68,
            69,
            70,
            71,
            72,
            73,
            74,
            75,
            76,
            77,
            78,
            79,
            80,
            81,
            82,
            83,
            84,
            85,
            86,
            87,
            88,
            89,
            90,
            91,
            92,
            93,
            94,
            95,
            96,
            97,
            98,
            99,
            100,
            101,
            102,
            103,
            104,
            105,
            106,
            107,
            108,
            109,
            110,
            111,
            112,
            113,
            114,
            115,
            116,
            117,
            118,
            119,
            120,
            121,
            122,
            123,
            124,
            125,
            126,
            127,
            128,
            129,
            130,
            131,
            132,
            133,
            134,
            135,
            136,
            137,
            138,
            139,
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            50,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            55,
            56,
            57,
            58,
            59,
            60,
            61,
            62,
            63,
            64,
            65,
            66,
            67,
            68,
            69,
            70,
            71,
            72,
            73,
            74,
            75,
            76,
            77,
            78,
            79,
            80,
            81,
            82,
            83,
            84,
            85,
            86,
            87,
            88,
            89,
            90,
            91,
            92,
            93,
            94,
            95,
            96,
            97,
            98,
            99,
            100,
            101,
            102,
            103,
            104,
            105,
            106,
            107,
            108,
            109,
            110,
            111,
            112,
            113,
            114,
            115,
            116,
            117,
            118,
            119,
            120,
            121,
            122,
            123,
            124,
            125,
            126,
            127,
            128,
            129,
            130,
            131,
            132,
            133,
            134,
            135,
            136,
            137,
            138,
            139
        };

        /// <summary>
        ///     実ユニット種類に対応する兵科
        /// </summary>
        public static readonly Branch[] RealBranchTable =
        {
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Airforce,
            Branch.Airforce,
            Branch.Army,
            Branch.Navy,
            Branch.Navy
        };

        #endregion

        #region 内部定数

        /// <summary>
        ///     利用可能な師団ユニット種類 (HoI2)
        /// </summary>
        private static readonly UnitType[] DivisionTypesHoI2 =
        {
            UnitType.Infantry,
            UnitType.Cavalry,
            UnitType.Motorized,
            UnitType.Mechanized,
            UnitType.LightArmor,
            UnitType.Armor,
            UnitType.Paratrooper,
            UnitType.Marine,
            UnitType.Bergsjaeger,
            UnitType.Garrison,
            UnitType.Hq,
            UnitType.Militia,
            UnitType.MultiRole,
            UnitType.Interceptor,
            UnitType.StrategicBomber,
            UnitType.TacticalBomber,
            UnitType.NavalBomber,
            UnitType.Cas,
            UnitType.TransportPlane,
            UnitType.FlyingBomb,
            UnitType.FlyingRocket,
            UnitType.BattleShip,
            UnitType.LightCruiser,
            UnitType.HeavyCruiser,
            UnitType.BattleCruiser,
            UnitType.Destroyer,
            UnitType.Carrier,
            UnitType.EscortCarrier,
            UnitType.Submarine,
            UnitType.NuclearSubmarine,
            UnitType.Transport
        };

        /// <summary>
        ///     利用可能な師団ユニット種類 (AoD)
        /// </summary>
        private static readonly UnitType[] DivisionTypesAoD =
        {
            UnitType.Infantry,
            UnitType.Cavalry,
            UnitType.Motorized,
            UnitType.Mechanized,
            UnitType.LightArmor,
            UnitType.Armor,
            UnitType.Paratrooper,
            UnitType.Marine,
            UnitType.Bergsjaeger,
            UnitType.Garrison,
            UnitType.Hq,
            UnitType.Militia,
            UnitType.MultiRole,
            UnitType.Interceptor,
            UnitType.StrategicBomber,
            UnitType.TacticalBomber,
            UnitType.NavalBomber,
            UnitType.Cas,
            UnitType.TransportPlane,
            UnitType.FlyingBomb,
            UnitType.FlyingRocket,
            UnitType.BattleShip,
            UnitType.LightCruiser,
            UnitType.HeavyCruiser,
            UnitType.BattleCruiser,
            UnitType.Destroyer,
            UnitType.Carrier,
            UnitType.EscortCarrier,
            UnitType.Submarine,
            UnitType.NuclearSubmarine,
            UnitType.Transport
        };

        /// <summary>
        ///     利用可能な師団ユニット種類 (DH1.03以降)
        /// </summary>
        private static readonly UnitType[] DivisionTypesDh =
        {
            UnitType.Infantry,
            UnitType.Cavalry,
            UnitType.Motorized,
            UnitType.Mechanized,
            UnitType.LightArmor,
            UnitType.Armor,
            UnitType.Paratrooper,
            UnitType.Marine,
            UnitType.Bergsjaeger,
            UnitType.Garrison,
            UnitType.Hq,
            UnitType.Militia,
            UnitType.MultiRole,
            UnitType.Interceptor,
            UnitType.StrategicBomber,
            UnitType.TacticalBomber,
            UnitType.NavalBomber,
            UnitType.Cas,
            UnitType.TransportPlane,
            UnitType.FlyingBomb,
            UnitType.FlyingRocket,
            UnitType.BattleShip,
            UnitType.LightCruiser,
            UnitType.HeavyCruiser,
            UnitType.BattleCruiser,
            UnitType.Destroyer,
            UnitType.Carrier,
            UnitType.EscortCarrier,
            UnitType.Submarine,
            UnitType.NuclearSubmarine,
            UnitType.Transport,
            UnitType.LightCarrier,
            UnitType.RocketInterceptor,
            UnitType.ReserveDivision33,
            UnitType.ReserveDivision34,
            UnitType.ReserveDivision35,
            UnitType.ReserveDivision36,
            UnitType.ReserveDivision37,
            UnitType.ReserveDivision38,
            UnitType.ReserveDivision39,
            UnitType.ReserveDivision40,
            UnitType.Division01,
            UnitType.Division02,
            UnitType.Division03,
            UnitType.Division04,
            UnitType.Division05,
            UnitType.Division06,
            UnitType.Division07,
            UnitType.Division08,
            UnitType.Division09,
            UnitType.Division10,
            UnitType.Division11,
            UnitType.Division12,
            UnitType.Division13,
            UnitType.Division14,
            UnitType.Division15,
            UnitType.Division16,
            UnitType.Division17,
            UnitType.Division18,
            UnitType.Division19,
            UnitType.Division20,
            UnitType.Division21,
            UnitType.Division22,
            UnitType.Division23,
            UnitType.Division24,
            UnitType.Division25,
            UnitType.Division26,
            UnitType.Division27,
            UnitType.Division28,
            UnitType.Division29,
            UnitType.Division30,
            UnitType.Division31,
            UnitType.Division32,
            UnitType.Division33,
            UnitType.Division34,
            UnitType.Division35,
            UnitType.Division36,
            UnitType.Division37,
            UnitType.Division38,
            UnitType.Division39,
            UnitType.Division40,
            UnitType.Division41,
            UnitType.Division42,
            UnitType.Division43,
            UnitType.Division44,
            UnitType.Division45,
            UnitType.Division46,
            UnitType.Division47,
            UnitType.Division48,
            UnitType.Division49,
            UnitType.Division50,
            UnitType.Division51,
            UnitType.Division52,
            UnitType.Division53,
            UnitType.Division54,
            UnitType.Division55,
            UnitType.Division56,
            UnitType.Division57,
            UnitType.Division58,
            UnitType.Division59,
            UnitType.Division60,
            UnitType.Division61,
            UnitType.Division62,
            UnitType.Division63,
            UnitType.Division64,
            UnitType.Division65,
            UnitType.Division66,
            UnitType.Division67,
            UnitType.Division68,
            UnitType.Division69,
            UnitType.Division70,
            UnitType.Division71,
            UnitType.Division72,
            UnitType.Division73,
            UnitType.Division74,
            UnitType.Division75,
            UnitType.Division76,
            UnitType.Division77,
            UnitType.Division78,
            UnitType.Division79,
            UnitType.Division80,
            UnitType.Division81,
            UnitType.Division82,
            UnitType.Division83,
            UnitType.Division84,
            UnitType.Division85,
            UnitType.Division86,
            UnitType.Division87,
            UnitType.Division88,
            UnitType.Division89,
            UnitType.Division90,
            UnitType.Division91,
            UnitType.Division92,
            UnitType.Division93,
            UnitType.Division94,
            UnitType.Division95,
            UnitType.Division96,
            UnitType.Division97,
            UnitType.Division98,
            UnitType.Division99
        };

        /// <summary>
        ///     利用可能な旅団ユニット種類 (HoI2)
        /// </summary>
        private static readonly UnitType[] BrigadeTypesHoI2 =
        {
            UnitType.None,
            UnitType.Artillery,
            UnitType.SpArtillery,
            UnitType.RocketArtillery,
            UnitType.SpRctArtillery,
            UnitType.AntiTank,
            UnitType.TankDestroyer,
            UnitType.LightArmorBrigade,
            UnitType.HeavyArmor,
            UnitType.SuperHeavyArmor,
            UnitType.ArmoredCar,
            UnitType.AntiAir,
            UnitType.Police,
            UnitType.Engineer,
            UnitType.Cag,
            UnitType.Escort,
            UnitType.NavalAsw,
            UnitType.NavalAntiAirS,
            UnitType.NavalRadarS,
            UnitType.NavalFireControllS,
            UnitType.NavalImprovedHullS,
            UnitType.NavalTorpedoesS,
            UnitType.NavalAntiAirL,
            UnitType.NavalRadarL,
            UnitType.NavalFireControllL,
            UnitType.NavalImprovedHullL,
            UnitType.NavalTorpedoesL
        };

        /// <summary>
        ///     利用可能なユニット種類 (AoD)
        /// </summary>
        private static readonly UnitType[] BrigadeTypesAoD =
        {
            UnitType.None,
            UnitType.Artillery,
            UnitType.SpArtillery,
            UnitType.RocketArtillery,
            UnitType.SpRctArtillery,
            UnitType.AntiTank,
            UnitType.TankDestroyer,
            UnitType.LightArmorBrigade,
            UnitType.HeavyArmor,
            UnitType.SuperHeavyArmor,
            UnitType.ArmoredCar,
            UnitType.AntiAir,
            UnitType.Police,
            UnitType.Engineer,
            UnitType.Cag,
            UnitType.Escort,
            UnitType.NavalAsw,
            UnitType.NavalAntiAirS,
            UnitType.NavalRadarS,
            UnitType.NavalFireControllS,
            UnitType.NavalImprovedHullS,
            UnitType.NavalTorpedoesS,
            UnitType.NavalAntiAirL,
            UnitType.NavalRadarL,
            UnitType.NavalFireControllL,
            UnitType.NavalImprovedHullL,
            UnitType.NavalTorpedoesL,
            UnitType.NavalMines,
            UnitType.NavalSaL,
            UnitType.NavalSpotterL,
            UnitType.NavalSpotterS,
            UnitType.ExtraBrigade1,
            UnitType.ExtraBrigade2,
            UnitType.ExtraBrigade3,
            UnitType.ExtraBrigade4,
            UnitType.ExtraBrigade5,
            UnitType.ExtraBrigade6,
            UnitType.ExtraBrigade7,
            UnitType.ExtraBrigade8,
            UnitType.ExtraBrigade9,
            UnitType.ExtraBrigade10,
            UnitType.ExtraBrigade11,
            UnitType.ExtraBrigade12,
            UnitType.ExtraBrigade13,
            UnitType.ExtraBrigade14,
            UnitType.ExtraBrigade15,
            UnitType.ExtraBrigade16,
            UnitType.ExtraBrigade17,
            UnitType.ExtraBrigade18,
            UnitType.ExtraBrigade19,
            UnitType.ExtraBrigade20
        };

        /// <summary>
        ///     利用可能なユニット種類 (DH1.03以降)
        /// </summary>
        private static readonly UnitType[] BrigadeTypesDh =
        {
            UnitType.None,
            UnitType.Artillery,
            UnitType.SpArtillery,
            UnitType.RocketArtillery,
            UnitType.SpRctArtillery,
            UnitType.AntiTank,
            UnitType.TankDestroyer,
            UnitType.LightArmorBrigade,
            UnitType.HeavyArmor,
            UnitType.SuperHeavyArmor,
            UnitType.ArmoredCar,
            UnitType.AntiAir,
            UnitType.Police,
            UnitType.Engineer,
            UnitType.Cag,
            UnitType.Escort,
            UnitType.NavalAsw,
            UnitType.NavalAntiAirS,
            UnitType.NavalRadarS,
            UnitType.NavalFireControllS,
            UnitType.NavalImprovedHullS,
            UnitType.NavalTorpedoesS,
            UnitType.NavalAntiAirL,
            UnitType.NavalRadarL,
            UnitType.NavalFireControllL,
            UnitType.NavalImprovedHullL,
            UnitType.NavalTorpedoesL,
            UnitType.CavalryBrigade,
            UnitType.SpAntiAir,
            UnitType.MediumArmor,
            UnitType.FloatPlane,
            UnitType.LightCag,
            UnitType.AmphArmor,
            UnitType.GliderArmor,
            UnitType.GliderArtillery,
            UnitType.SuperHeavyArtillery,
            UnitType.ReserveBrigade36,
            UnitType.ReserveBrigade37,
            UnitType.ReserveBrigade38,
            UnitType.ReserveBrigade39,
            UnitType.ReserveBrigade40,
            UnitType.Brigade01,
            UnitType.Brigade02,
            UnitType.Brigade03,
            UnitType.Brigade04,
            UnitType.Brigade05,
            UnitType.Brigade06,
            UnitType.Brigade07,
            UnitType.Brigade08,
            UnitType.Brigade09,
            UnitType.Brigade10,
            UnitType.Brigade11,
            UnitType.Brigade12,
            UnitType.Brigade13,
            UnitType.Brigade14,
            UnitType.Brigade15,
            UnitType.Brigade16,
            UnitType.Brigade17,
            UnitType.Brigade18,
            UnitType.Brigade19,
            UnitType.Brigade20,
            UnitType.Brigade21,
            UnitType.Brigade22,
            UnitType.Brigade23,
            UnitType.Brigade24,
            UnitType.Brigade25,
            UnitType.Brigade26,
            UnitType.Brigade27,
            UnitType.Brigade28,
            UnitType.Brigade29,
            UnitType.Brigade30,
            UnitType.Brigade31,
            UnitType.Brigade32,
            UnitType.Brigade33,
            UnitType.Brigade34,
            UnitType.Brigade35,
            UnitType.Brigade36,
            UnitType.Brigade37,
            UnitType.Brigade38,
            UnitType.Brigade39,
            UnitType.Brigade40,
            UnitType.Brigade41,
            UnitType.Brigade42,
            UnitType.Brigade43,
            UnitType.Brigade44,
            UnitType.Brigade45,
            UnitType.Brigade46,
            UnitType.Brigade47,
            UnitType.Brigade48,
            UnitType.Brigade49,
            UnitType.Brigade50,
            UnitType.Brigade51,
            UnitType.Brigade52,
            UnitType.Brigade53,
            UnitType.Brigade54,
            UnitType.Brigade55,
            UnitType.Brigade56,
            UnitType.Brigade57,
            UnitType.Brigade58,
            UnitType.Brigade59,
            UnitType.Brigade60,
            UnitType.Brigade61,
            UnitType.Brigade62,
            UnitType.Brigade63,
            UnitType.Brigade64,
            UnitType.Brigade65,
            UnitType.Brigade66,
            UnitType.Brigade67,
            UnitType.Brigade68,
            UnitType.Brigade69,
            UnitType.Brigade70,
            UnitType.Brigade71,
            UnitType.Brigade72,
            UnitType.Brigade73,
            UnitType.Brigade74,
            UnitType.Brigade75,
            UnitType.Brigade76,
            UnitType.Brigade77,
            UnitType.Brigade78,
            UnitType.Brigade79,
            UnitType.Brigade80,
            UnitType.Brigade81,
            UnitType.Brigade82,
            UnitType.Brigade83,
            UnitType.Brigade84,
            UnitType.Brigade85,
            UnitType.Brigade86,
            UnitType.Brigade87,
            UnitType.Brigade88,
            UnitType.Brigade89,
            UnitType.Brigade90,
            UnitType.Brigade91,
            UnitType.Brigade92,
            UnitType.Brigade93,
            UnitType.Brigade94,
            UnitType.Brigade95,
            UnitType.Brigade96,
            UnitType.Brigade97,
            UnitType.Brigade98,
            UnitType.Brigade99
        };

        /// <summary>
        ///     ユニット定義ファイル名の初期設定値
        /// </summary>
        private static readonly string[] DefaultFileNames =
        {
            "infantry.txt",
            "cavalry.txt",
            "motorized.txt",
            "mechanized.txt",
            "light_armor.txt",
            "armor.txt",
            "paratrooper.txt",
            "marine.txt",
            "bergsjaeger.txt",
            "garrison.txt",
            "hq.txt",
            "militia.txt",
            "multi_role.txt",
            "interceptor.txt",
            "strategic_bomber.txt",
            "tactical_bomber.txt",
            "naval_bomber.txt",
            "cas.txt",
            "transport_plane.txt",
            "flying_bomb.txt",
            "flying_rocket.txt",
            "battleship.txt",
            "light_cruiser.txt",
            "heavy_cruiser.txt",
            "battlecruiser.txt",
            "destroyer.txt",
            "carrier.txt",
            "escort_carrier.txt",
            "submarine.txt",
            "nuclear_submarine.txt",
            "transport.txt",
            "light_carrier.txt",
            "rocket_interceptor.txt",
            "d_rsv_33.txt",
            "d_rsv_34.txt",
            "d_rsv_35.txt",
            "d_rsv_36.txt",
            "d_rsv_37.txt",
            "d_rsv_38.txt",
            "d_rsv_39.txt",
            "d_rsv_40.txt",
            "d_01.txt",
            "d_02.txt",
            "d_03.txt",
            "d_04.txt",
            "d_05.txt",
            "d_06.txt",
            "d_07.txt",
            "d_08.txt",
            "d_09.txt",
            "d_10.txt",
            "d_11.txt",
            "d_12.txt",
            "d_13.txt",
            "d_14.txt",
            "d_15.txt",
            "d_16.txt",
            "d_17.txt",
            "d_18.txt",
            "d_19.txt",
            "d_20.txt",
            "d_21.txt",
            "d_22.txt",
            "d_23.txt",
            "d_24.txt",
            "d_25.txt",
            "d_26.txt",
            "d_27.txt",
            "d_28.txt",
            "d_29.txt",
            "d_30.txt",
            "d_31.txt",
            "d_32.txt",
            "d_33.txt",
            "d_34.txt",
            "d_35.txt",
            "d_36.txt",
            "d_37.txt",
            "d_38.txt",
            "d_39.txt",
            "d_40.txt",
            "d_41.txt",
            "d_42.txt",
            "d_43.txt",
            "d_44.txt",
            "d_45.txt",
            "d_46.txt",
            "d_47.txt",
            "d_48.txt",
            "d_49.txt",
            "d_50.txt",
            "d_51.txt",
            "d_52.txt",
            "d_53.txt",
            "d_54.txt",
            "d_55.txt",
            "d_56.txt",
            "d_57.txt",
            "d_58.txt",
            "d_59.txt",
            "d_60.txt",
            "d_61.txt",
            "d_62.txt",
            "d_63.txt",
            "d_64.txt",
            "d_65.txt",
            "d_66.txt",
            "d_67.txt",
            "d_68.txt",
            "d_69.txt",
            "d_70.txt",
            "d_71.txt",
            "d_72.txt",
            "d_73.txt",
            "d_74.txt",
            "d_75.txt",
            "d_76.txt",
            "d_77.txt",
            "d_78.txt",
            "d_79.txt",
            "d_80.txt",
            "d_81.txt",
            "d_82.txt",
            "d_83.txt",
            "d_84.txt",
            "d_85.txt",
            "d_86.txt",
            "d_87.txt",
            "d_88.txt",
            "d_89.txt",
            "d_90.txt",
            "d_91.txt",
            "d_92.txt",
            "d_93.txt",
            "d_94.txt",
            "d_95.txt",
            "d_96.txt",
            "d_97.txt",
            "d_98.txt",
            "d_99.txt",
            "none.txt",
            "artillery.txt",
            "sp_artillery.txt",
            "rocket_artillery.txt",
            "sp_rct_artillery.txt",
            "anti_tank.txt",
            "tank_destroyer.txt",
            "light_armor_brigade.txt",
            "heavy_armor.txt",
            "super_heavy_armor.txt",
            "armored_car.txt",
            "anti_air.txt",
            "police.txt",
            "engineer.txt",
            "cag.txt",
            "escort.txt",
            "naval_asw.txt",
            "naval_anti_air_s.txt",
            "naval_radar_s.txt",
            "naval_fire_controll_s.txt",
            "naval_improved_hull_s.txt",
            "naval_torpedoes_s.txt",
            "naval_anti_air_l.txt",
            "naval_radar_l.txt",
            "naval_fire_controll_l.txt",
            "naval_improved_hull_l.txt",
            "naval_torpedoes_l.txt",
            "naval_mines.txt",
            "naval_sa_l.txt",
            "naval_spotter_l.txt",
            "naval_spotter_s.txt",
            "b_u1.txt",
            "b_u2.txt",
            "b_u3.txt",
            "b_u4.txt",
            "b_u5.txt",
            "b_u6.txt",
            "b_u7.txt",
            "b_u8.txt",
            "b_u9.txt",
            "b_u10.txt",
            "b_u11.txt",
            "b_u12.txt",
            "b_u13.txt",
            "b_u14.txt",
            "b_u15.txt",
            "b_u16.txt",
            "b_u17.txt",
            "b_u18.txt",
            "b_u19.txt",
            "b_u20.txt",
            "cavalry_brigade.txt",
            "sp_anti_air.txt",
            "medium_armor.txt",
            "floatplane.txt",
            "light_cag.txt",
            "amph_armor.txt",
            "glider_armor.txt",
            "glider_artillery.txt",
            "super_heavy_artillery.txt",
            "b_rsv_36.txt",
            "b_rsv_37.txt",
            "b_rsv_38.txt",
            "b_rsv_39.txt",
            "b_rsv_40.txt",
            "b_01.txt",
            "b_02.txt",
            "b_03.txt",
            "b_04.txt",
            "b_05.txt",
            "b_06.txt",
            "b_07.txt",
            "b_08.txt",
            "b_09.txt",
            "b_10.txt",
            "b_11.txt",
            "b_12.txt",
            "b_13.txt",
            "b_14.txt",
            "b_15.txt",
            "b_16.txt",
            "b_17.txt",
            "b_18.txt",
            "b_19.txt",
            "b_20.txt",
            "b_21.txt",
            "b_22.txt",
            "b_23.txt",
            "b_24.txt",
            "b_25.txt",
            "b_26.txt",
            "b_27.txt",
            "b_28.txt",
            "b_29.txt",
            "b_30.txt",
            "b_31.txt",
            "b_32.txt",
            "b_33.txt",
            "b_34.txt",
            "b_35.txt",
            "b_36.txt",
            "b_37.txt",
            "b_38.txt",
            "b_39.txt",
            "b_40.txt",
            "b_41.txt",
            "b_42.txt",
            "b_43.txt",
            "b_44.txt",
            "b_45.txt",
            "b_46.txt",
            "b_47.txt",
            "b_48.txt",
            "b_49.txt",
            "b_50.txt",
            "b_51.txt",
            "b_52.txt",
            "b_53.txt",
            "b_54.txt",
            "b_55.txt",
            "b_56.txt",
            "b_57.txt",
            "b_58.txt",
            "b_59.txt",
            "b_60.txt",
            "b_61.txt",
            "b_62.txt",
            "b_63.txt",
            "b_64.txt",
            "b_65.txt",
            "b_66.txt",
            "b_67.txt",
            "b_68.txt",
            "b_69.txt",
            "b_70.txt",
            "b_71.txt",
            "b_72.txt",
            "b_73.txt",
            "b_74.txt",
            "b_75.txt",
            "b_76.txt",
            "b_77.txt",
            "b_78.txt",
            "b_79.txt",
            "b_80.txt",
            "b_81.txt",
            "b_82.txt",
            "b_83.txt",
            "b_84.txt",
            "b_85.txt",
            "b_86.txt",
            "b_87.txt",
            "b_88.txt",
            "b_89.txt",
            "b_90.txt",
            "b_91.txt",
            "b_92.txt",
            "b_93.txt",
            "b_94.txt",
            "b_95.txt",
            "b_96.txt",
            "b_97.txt",
            "b_98.txt",
            "b_99.txt"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Units()
        {
            // ユニット一覧
            Items = new List<Unit>();

            // ユニット種類文字列とIDの対応付けを初期化
            StringMap = new Dictionary<string, UnitType>();
            foreach (UnitType type in Enum.GetValues(typeof (UnitType)))
            {
                StringMap.Add(Strings[(int) type], type);
            }

            // 実ユニット種類文字列とIDの対応付けを初期化
            RealStringMap = new Dictionary<string, RealUnitType>();
            foreach (RealUnitType type in Enum.GetValues(typeof (RealUnitType)))
            {
                RealStringMap.Add(RealStrings[(int) type], type);
            }

            // スプライト種類文字列とIDの対応付けを初期化
            SpriteStringMap = new Dictionary<string, SpriteType>();
            foreach (SpriteType type in Enum.GetValues(typeof (SpriteType)))
            {
                SpriteStringMap.Add(SpriteStrings[(int) type], type);
            }

            // 装備文字列とIDの対応付けを初期化
            EquipmentStringMap = new Dictionary<string, EquipmentType>();
            foreach (EquipmentType type in Enum.GetValues(typeof (EquipmentType)))
            {
                EquipmentStringMap.Add(EquipmentStrings[(int) type], type);
            }
        }

        /// <summary>
        ///     ユニットデータを初期化する
        /// </summary>
        public static void Init()
        {
            InitTypes();
        }

        /// <summary>
        ///     利用可能なユニット種類を初期化する
        /// </summary>
        private static void InitTypes()
        {
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                DivisionTypes = DivisionTypesAoD;
                BrigadeTypes = BrigadeTypesAoD;
            }
            else if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                DivisionTypes = DivisionTypesDh;
                BrigadeTypes = BrigadeTypesDh;
            }
            else
            {
                DivisionTypes = DivisionTypesHoI2;
                BrigadeTypes = BrigadeTypesHoI2;
            }

            UnitTypes = new List<UnitType>();
            UnitTypes.AddRange(DivisionTypes);
            UnitTypes.AddRange(BrigadeTypes);
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

            Load();
        }

        /// <summary>
        ///     ユニット定義ファイル群を読み込む
        /// </summary>
        public static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            LoadFiles();

            // モデル名の編集済みフラグをクリアする
            ResetDirtyAllModelName();

            // 最大付属旅団数の編集済みフラグを反映する
            if ((Game.Type == GameType.ArsenalOfDemocracy) && (Game.Version >= 107))
            {
                UpdateDirtyMaxAllowedBrigades();
            }
        }

        /// <summary>
        ///     ユニット定義ファイル群を読み込む
        /// </summary>
        private static void LoadFiles()
        {
            Items.Clear();

            // ユニットクラスデータの初期値を設定する
            foreach (UnitType type in Enum.GetValues(typeof (UnitType)))
            {
                Items.Add(new Unit(type));
            }

            // ユニットクラス定義ファイルを読み込む(DH1.03以降)
            if (Game.Type == GameType.DarkestHour && Game.Version >= 103)
            {
                string fileName = "";
                try
                {
                    fileName = Game.GetReadFileName(Game.DhDivisionTypePathName);
                    LoadDivisionTypes(fileName);

                    fileName = Game.GetReadFileName(Game.DhBrigadeTypePathName);
                    LoadBrigadeTypes(fileName);
                }
                catch (Exception)
                {
                    Debug.WriteLine(string.Format("[Unit] Read error: {0}", fileName));
                    Log.Write(String.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                        Resources.EditorUnit, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ユニット定義ファイルを順に読み込む
            bool error = false;
            foreach (UnitType type in UnitTypes)
            {
                try
                {
                    LoadFile(type);
                }
                catch (Exception)
                {
                    error = true;
                    Unit unit = Items[(int) type];
                    string fileName =
                        Game.GetReadFileName(
                            unit.Organization == UnitOrganization.Division
                                ? Game.DivisionPathName
                                : Game.BrigadePathName, DefaultFileNames[(int) type]);
                    Debug.WriteLine(string.Format("[Unit] Read error: {0}", fileName));
                    Log.Write(string.Format("{0}: {1}\n\n", Resources.FileReadError, fileName));
                    if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                        Resources.EditorUnit, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

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
        ///     ユニット定義ファイルを読み込む
        /// </summary>
        /// <param name="type">ユニットの種類</param>
        private static void LoadFile(UnitType type)
        {
            // ユニット定義ファイルを解析する
            Unit unit = Items[(int) type];
            string fileName =
                Game.GetReadFileName(
                    unit.Organization == UnitOrganization.Division ? Game.DivisionPathName : Game.BrigadePathName,
                    DefaultFileNames[(int) type]);
            if (!File.Exists(fileName))
            {
                return;
            }

            Debug.WriteLine(string.Format("[Unit] Load: {0}", Path.GetFileName(fileName)));

            UnitParser.Parse(fileName, unit);
        }

        /// <summary>
        ///     師団ユニットクラス定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadDivisionTypes(string fileName)
        {
            // ファイルが存在しなければ戻る
            if (!File.Exists(fileName))
            {
                return;
            }

            Debug.WriteLine(string.Format("[Unit] Load: {0}", Path.GetFileName(fileName)));

            // ファイルを解析する
            UnitParser.ParseDivisionTypes(fileName, Items);

            // 編集済みフラグを解除する
            ResetDirtyDivisionTypes();
        }

        /// <summary>
        ///     旅団ユニットクラス定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadBrigadeTypes(string fileName)
        {
            // ファイルが存在しなければ戻る
            if (!File.Exists(fileName))
            {
                return;
            }

            Debug.WriteLine(string.Format("[Unit] Load: {0}", Path.GetFileName(fileName)));

            // ファイルを解析する
            UnitParser.ParseBrigadeTypes(fileName, Items);

            // 編集済みフラグを解除する
            ResetDirtyBrigadeTypes();
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     ユニット定義ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            if ((Game.Type == GameType.DarkestHour) && (Game.Version >= 103))
            {
                if (IsDirtyDivisionTypes())
                {
                    // 師団定義ファイルを保存する
                    string fileName = Game.GetWriteFileName(Game.DhDivisionTypePathName);
                    try
                    {
                        SaveDivisionTypes(fileName);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine(string.Format("[Unit] Write error: {0}", fileName));
                        Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                        MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                            Resources.EditorUnit, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                if (IsDirtyBrigadeTypes())
                {
                    // 旅団定義ファイルを保存する
                    string fileName = Game.GetWriteFileName(Game.DhBrigadeTypePathName);
                    try
                    {
                        SaveBrigadeTypes(fileName);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine(string.Format("[Unit] Write error: {0}", fileName));
                        Log.Write(String.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                        MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                            Resources.EditorUnit, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            if (IsDirty())
            {
                // ユニット定義ファイルへ順に保存する
                bool error = false;
                foreach (Unit unit in Items.Where(unit => unit.IsDirty()))
                {
                    string folderName = Game.GetWriteFileName((unit.Organization == UnitOrganization.Division)
                        ? Game.DivisionPathName
                        : Game.BrigadePathName);
                    string fileName = Path.Combine(folderName, DefaultFileNames[(int) unit.Type]);

                    try
                    {
                        // 師団/旅団定義フォルダがなければ作成する
                        if (!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }

                        Debug.WriteLine(string.Format("[Unit] Save: {0}", Path.GetFileName(fileName)));

                        // ユニット定義ファイルを保存する
                        UnitWriter.Write(unit, fileName);
                    }
                    catch (Exception)
                    {
                        error = true;
                        Debug.WriteLine(string.Format("[Unit] Write error: {0}", fileName));
                        Log.Write(string.Format("{0}: {1}\n\n", Resources.FileWriteError, fileName));
                        if (MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName),
                            Resources.EditorUnit, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
                // 文字列定義のみ保存の場合、ユニットクラス名などの編集済みフラグがクリアされないためここで全クリアする
                foreach (Unit unit in Items)
                {
                    unit.ResetDirtyAll();
                }

                // モデル名の編集済みフラグをクリアする
                ResetDirtyAllModelName();
            }

            return true;
        }

        /// <summary>
        ///     師団ユニットクラス定義ファイルを保存する
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void SaveDivisionTypes(string fileName)
        {
            // 変更がなければ何もしない
            if (!IsDirtyDivisionTypes())
            {
                return;
            }

            // ユニット定義フォルダがなければ作成する
            string folderName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            Debug.WriteLine(string.Format("[Unit] Save: {0}", Path.GetFileName(fileName)));

            // 師団ユニットクラス定義ファイルを保存する
            UnitWriter.WriteDivisionTypes(Items, fileName);

            // 編集済みフラグを解除する
            ResetDirtyDivisionTypes();
        }

        /// <summary>
        ///     旅団ユニットクラス定義ファイルを保存する
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void SaveBrigadeTypes(string fileName)
        {
            // 変更がなければ何もしない
            if (!IsDirtyBrigadeTypes())
            {
                return;
            }

            // ユニット定義フォルダがなければ作成する
            string folderName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            Debug.WriteLine(string.Format("[Unit] Save: {0}", Path.GetFileName(fileName)));

            // 旅団ユニットクラス定義ファイルを保存する
            UnitWriter.WriteBrigadeTypes(Items, fileName);

            // 編集済みフラグを解除する
            ResetDirtyBrigadeTypes();
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
        ///     師団ユニットクラス定義が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        private static bool IsDirtyDivisionTypes()
        {
            return _divisionTypesDirty;
        }

        /// <summary>
        ///     旅団ユニットクラス定義が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        private static bool IsDirtyBrigadeTypes()
        {
            return _brigadeTypesDirty;
        }

        /// <summary>
        ///     師団ユニットクラス定義の編集済みフラグを設定する
        /// </summary>
        public static void SetDirtyDivisionTypes()
        {
            _divisionTypesDirty = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     旅団ユニットクラス定義の編集済みフラグを設定する
        /// </summary>
        public static void SetDirtyBrigadeTypes()
        {
            _brigadeTypesDirty = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     師団ユニットクラス定義の編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyDivisionTypes()
        {
            _divisionTypesDirty = false;
        }

        /// <summary>
        ///     旅団ユニットクラス定義の編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyBrigadeTypes()
        {
            _brigadeTypesDirty = false;
        }

        /// <summary>
        ///     モデル名が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirtyModelName(Country country)
        {
            return CountryNameDirtyFlags[(int) country];
        }

        /// <summary>
        ///     モデル名が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名種類</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirtyModelName(Country country, UnitType type)
        {
            return TypeNameDirtyFlags[(int) country, (int) type];
        }

        /// <summary>
        ///     モデル名の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名種類</param>
        public static void SetDirtyModelName(Country country, UnitType type)
        {
            TypeNameDirtyFlags[(int) country, (int) type] = true;
            CountryNameDirtyFlags[(int) country] = true;
        }

        /// <summary>
        ///     モデル名の編集済みフラグを全て解除する
        /// </summary>
        private static void ResetDirtyAllModelName()
        {
            foreach (Country country in Enum.GetValues(typeof (Country)))
            {
                foreach (UnitType type in UnitTypes)
                {
                    TypeNameDirtyFlags[(int) country, (int) type] = false;
                }
                CountryNameDirtyFlags[(int) country] = false;
            }
        }

        /// <summary>
        ///     最大付属旅団数の編集済みフラグを更新する
        /// </summary>
        private static void UpdateDirtyMaxAllowedBrigades()
        {
            // 輸送艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.TpMaxAttach))
            {
                Items[(int) UnitType.Transport].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 潜水艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.SsMaxAttach))
            {
                Items[(int) UnitType.Submarine].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 原子力潜水艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.SsnMaxAttach))
            {
                Items[(int) UnitType.NuclearSubmarine].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 駆逐艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.DdMaxAttach))
            {
                Items[(int) UnitType.Destroyer].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 軽巡洋艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.ClMaxAttach))
            {
                Items[(int) UnitType.LightCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 重巡洋艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.CaMaxAttach))
            {
                Items[(int) UnitType.HeavyCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 巡洋戦艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.BcMaxAttach))
            {
                Items[(int) UnitType.BattleCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 戦艦最大付属装備数
            if (Misc.IsDirty(MiscItemId.BbMaxAttach))
            {
                Items[(int) UnitType.BattleShip].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 軽空母最大付属装備数
            if (Misc.IsDirty(MiscItemId.CvlMaxAttach))
            {
                Items[(int) UnitType.EscortCarrier].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
            // 空母最大付属装備数
            if (Misc.IsDirty(MiscItemId.CvMaxAttach))
            {
                Items[(int) UnitType.Carrier].SetDirty(UnitClassItemId.MaxAllowedBrigades);
            }
        }

        #endregion
    }
}