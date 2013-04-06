﻿using System;
using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     国家データ
    /// </summary>
    public class Country
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ一覧
        /// </summary>
        public static CountryTag[] Tags { get; private set; }

        /// <summary>
        ///     国タグ文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, CountryTag> StringMap { get; private set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     国タグ文字列
        /// </summary>
        public static string[] Strings =
            {
                "",
                "AFG",
                "ALB",
                "ALG",
                "ALI",
                "ALS",
                "ANG",
                "ARA",
                "ARG",
                "ARM",
                "AST",
                "AUS",
                "AXI",
                "AZB",
                "BEL",
                "BEN",
                "BHU",
                "BLR",
                "BOL",
                "BOS",
                "BRA",
                "BRU",
                "BUL",
                "BUR",
                "CAL",
                "CAM",
                "CAN",
                "CGX",
                "CHC",
                "CHI",
                "CHL",
                "CMB",
                "COL",
                "CON",
                "COS",
                "CRO",
                "CSA",
                "CSX",
                "CUB",
                "CXB",
                "CYN",
                "CYP",
                "CZE",
                "DDR",
                "DEN",
                "DFR",
                "DOM",
                "EAF",
                "ECU",
                "EGY",
                "ENG",
                "EQA",
                "EST",
                "ETH",
                "EUS",
                "FIN",
                "FLA",
                "FRA",
                "GAB",
                "GEO",
                "GER",
                "GLD",
                "GRE",
                "GUA",
                "GUI",
                "GUY",
                "HAI",
                "HOL",
                "HON",
                "HUN",
                "ICL",
                "IDC",
                "IND",
                "INO",
                "IRE",
                "IRQ",
                "ISR",
                "ITA",
                "JAP",
                "JOR",
                "KAZ",
                "KOR",
                "KUR",
                "KYG",
                "LAO",
                "LAT",
                "LBY",
                "LEB",
                "LIB",
                "LIT",
                "LUX",
                "MAD",
                "MAL",
                "MAN",
                "MEN",
                "MEX",
                "MLY",
                "MON",
                "MOR",
                "MOZ",
                "MTN",
                "NAM",
                "NEP",
                "NIC",
                "NIG",
                "NOR",
                "NZL",
                "OMN",
                "OTT",
                "PAK",
                "PAL",
                "PAN",
                "PAR",
                "PER",
                "PHI",
                "POL",
                "POR",
                "PRI",
                "PRK",
                "PRU",
                "QUE",
                "RHO",
                "ROM",
                "RSI",
                "RUS",
                "SAF",
                "SAL",
                "SAR",
                "SAU",
                "SCA",
                "SCH",
                "SCO",
                "SER",
                "SIA",
                "SIB",
                "SIE",
                "SIK",
                "SLO",
                "SLV",
                "SOM",
                "SOV",
                "SPA",
                "SPR",
                "SUD",
                "SWE",
                "SYR",
                "TAJ",
                "TAN",
                "TEX",
                "TIB",
                "TRA",
                "TRK",
                "TUN",
                "TUR",
                "UAP",
                "UAU",
                "UBO",
                "UCH",
                "UCS",
                "UER",
                "UES",
                "UGS",
                "UIC",
                "UIR",
                "UKR",
                "UPE",
                "UPR",
                "UPS",
                "URO",
                "URU",
                "USA",
                "USN",
                "UTC",
                "UTL",
                "UTO",
                "UZB",
                "VEN",
                "VIC",
                "VIE",
                "WLL",
                "YEM",
                "YUG",
                "U00",
                "U01",
                "U02",
                "U03",
                "U04",
                "U05",
                "U06",
                "U07",
                "U08",
                "U09",
                "U10",
                "U11",
                "U12",
                "U13",
                "U14",
                "U15",
                "U16",
                "U17",
                "U18",
                "U19",
                "U20",
                "U21",
                "U22",
                "U23",
                "U24",
                "U25",
                "U26",
                "U27",
                "U28",
                "U29",
                "U30",
                "U31",
                "U32",
                "U33",
                "U34",
                "U35",
                "U36",
                "U37",
                "U38",
                "U39",
                "U40",
                "U41",
                "U42",
                "U43",
                "U44",
                "U45",
                "U46",
                "U47",
                "U48",
                "U49",
                "U50",
                "U51",
                "U52",
                "U53",
                "U54",
                "U55",
                "U56",
                "U57",
                "U58",
                "U59",
                "U60",
                "U61",
                "U62",
                "U63",
                "U64",
                "U65",
                "U66",
                "U67",
                "U68",
                "U69",
                "U70",
                "U71",
                "U72",
                "U73",
                "U74",
                "U75",
                "U76",
                "U77",
                "U78",
                "U79",
                "U80",
                "U81",
                "U82",
                "U83",
                "U84",
                "U85",
                "U86",
                "U87",
                "U88",
                "U89",
                "U90",
                "U91",
                "U92",
                "U93",
                "U94",
                "U95",
                "U96",
                "U97",
                "U98",
                "U99",
                "UA0",
                "UA1",
                "UA2",
                "UA3",
                "UA4",
                "UA5",
                "UA6",
                "UA7",
                "UA8",
                "UA9",
                "UB0",
                "UB1",
                "UB2",
                "UB3",
                "UB4",
                "UB5",
                "UB6",
                "UB7",
                "UB8",
                "UB9",
                "UC0",
                "UC1",
                "UC2",
                "UC3",
                "UC4",
                "UC5",
                "UC6",
                "UC7",
                "UC8",
                "UC9",
                "UD0",
                "UD1",
                "UD2",
                "UD3",
                "UD4",
                "UD5",
                "UD6",
                "UD7",
                "UD8",
                "UD9",
                "UE0",
                "UE1",
                "UE2",
                "UE3",
                "UE4",
                "UE5",
                "UE6",
                "UE7",
                "UE8",
                "UE9",
                "UF0",
                "UF1",
                "UF2",
                "UF3",
                "UF4",
                "UF5",
                "UF6",
                "UF7",
                "UF8",
                "UF9"
            };

        #endregion

        #region 内部定数

        /// <summary>
        ///     国タグ一覧 (HoI2)
        /// </summary>
        private static readonly CountryTag[] TagsHoI2 =
            {
                CountryTag.AFG,
                CountryTag.ALB,
                CountryTag.ALG,
                CountryTag.ALI,
                CountryTag.ALS,
                CountryTag.ANG,
                CountryTag.ARA,
                CountryTag.ARG,
                CountryTag.ARM,
                CountryTag.AST,
                CountryTag.AUS,
                CountryTag.AXI,
                CountryTag.AZB,
                CountryTag.BEL,
                CountryTag.BEN,
                CountryTag.BHU,
                CountryTag.BLR,
                CountryTag.BOL,
                CountryTag.BOS,
                CountryTag.BRA,
                CountryTag.BRU,
                CountryTag.BUL,
                CountryTag.BUR,
                CountryTag.CAL,
                CountryTag.CAM,
                CountryTag.CAN,
                CountryTag.CGX,
                CountryTag.CHC,
                CountryTag.CHI,
                CountryTag.CHL,
                CountryTag.CMB,
                CountryTag.COL,
                CountryTag.CON,
                CountryTag.COS,
                CountryTag.CRO,
                CountryTag.CSA,
                CountryTag.CSX,
                CountryTag.CUB,
                CountryTag.CXB,
                CountryTag.CYN,
                CountryTag.CYP,
                CountryTag.CZE,
                CountryTag.DDR,
                CountryTag.DEN,
                CountryTag.DFR,
                CountryTag.DOM,
                CountryTag.EAF,
                CountryTag.ECU,
                CountryTag.EGY,
                CountryTag.ENG,
                CountryTag.EQA,
                CountryTag.EST,
                CountryTag.ETH,
                CountryTag.EUS,
                CountryTag.FIN,
                CountryTag.FLA,
                CountryTag.FRA,
                CountryTag.GAB,
                CountryTag.GEO,
                CountryTag.GER,
                CountryTag.GLD,
                CountryTag.GRE,
                CountryTag.GUA,
                CountryTag.GUI,
                CountryTag.GUY,
                CountryTag.HAI,
                CountryTag.HOL,
                CountryTag.HON,
                CountryTag.HUN,
                CountryTag.ICL,
                CountryTag.IDC,
                CountryTag.IND,
                CountryTag.INO,
                CountryTag.IRE,
                CountryTag.IRQ,
                CountryTag.ISR,
                CountryTag.ITA,
                CountryTag.JAP,
                CountryTag.JOR,
                CountryTag.KAZ,
                CountryTag.KOR,
                CountryTag.KUR,
                CountryTag.KYG,
                CountryTag.LAO,
                CountryTag.LAT,
                CountryTag.LBY,
                CountryTag.LEB,
                CountryTag.LIB,
                CountryTag.LIT,
                CountryTag.LUX,
                CountryTag.MAD,
                CountryTag.MAL,
                CountryTag.MAN,
                CountryTag.MEN,
                CountryTag.MEX,
                CountryTag.MLY,
                CountryTag.MON,
                CountryTag.MOR,
                CountryTag.MOZ,
                CountryTag.MTN,
                CountryTag.NAM,
                CountryTag.NEP,
                CountryTag.NIC,
                CountryTag.NIG,
                CountryTag.NOR,
                CountryTag.NZL,
                CountryTag.OMN,
                CountryTag.OTT,
                CountryTag.PAK,
                CountryTag.PAL,
                CountryTag.PAN,
                CountryTag.PAR,
                CountryTag.PER,
                CountryTag.PHI,
                CountryTag.POL,
                CountryTag.POR,
                CountryTag.PRI,
                CountryTag.PRK,
                CountryTag.PRU,
                CountryTag.QUE,
                CountryTag.RHO,
                CountryTag.ROM,
                CountryTag.RSI,
                CountryTag.RUS,
                CountryTag.SAF,
                CountryTag.SAL,
                CountryTag.SAR,
                CountryTag.SAU,
                CountryTag.SCA,
                CountryTag.SCH,
                CountryTag.SCO,
                CountryTag.SER,
                CountryTag.SIA,
                CountryTag.SIB,
                CountryTag.SIE,
                CountryTag.SIK,
                CountryTag.SLO,
                CountryTag.SLV,
                CountryTag.SOM,
                CountryTag.SOV,
                CountryTag.SPA,
                CountryTag.SPR,
                CountryTag.SUD,
                CountryTag.SWE,
                CountryTag.SYR,
                CountryTag.TAJ,
                CountryTag.TAN,
                CountryTag.TEX,
                CountryTag.TIB,
                CountryTag.TRA,
                CountryTag.TRK,
                CountryTag.TUN,
                CountryTag.TUR,
                CountryTag.UAP,
                CountryTag.UAU,
                CountryTag.UBO,
                CountryTag.UCH,
                CountryTag.UCS,
                CountryTag.UER,
                CountryTag.UES,
                CountryTag.UGS,
                CountryTag.UIC,
                CountryTag.UIR,
                CountryTag.UKR,
                CountryTag.UPE,
                CountryTag.UPR,
                CountryTag.UPS,
                CountryTag.URO,
                CountryTag.URU,
                CountryTag.USA,
                CountryTag.USN,
                CountryTag.UTC,
                CountryTag.UTL,
                CountryTag.UTO,
                CountryTag.UZB,
                CountryTag.VEN,
                CountryTag.VIC,
                CountryTag.VIE,
                CountryTag.WLL,
                CountryTag.YEM,
                CountryTag.YUG,
                CountryTag.U00,
                CountryTag.U01,
                CountryTag.U02,
                CountryTag.U03,
                CountryTag.U04,
                CountryTag.U05,
                CountryTag.U06,
                CountryTag.U07,
                CountryTag.U08,
                CountryTag.U09,
                CountryTag.U10,
                CountryTag.U11,
                CountryTag.U12,
                CountryTag.U13,
                CountryTag.U14,
                CountryTag.U15,
                CountryTag.U16,
                CountryTag.U17,
                CountryTag.U18,
                CountryTag.U19,
                CountryTag.U20,
                CountryTag.U21,
                CountryTag.U22,
                CountryTag.U23,
                CountryTag.U24,
                CountryTag.U25,
                CountryTag.U26,
                CountryTag.U27,
                CountryTag.U28,
                CountryTag.U29,
                CountryTag.U30,
                CountryTag.U31,
                CountryTag.U32,
                CountryTag.U33,
                CountryTag.U34,
                CountryTag.U35,
                CountryTag.U36,
                CountryTag.U37,
                CountryTag.U38,
                CountryTag.U39,
                CountryTag.U40,
                CountryTag.U41,
                CountryTag.U42,
                CountryTag.U43,
                CountryTag.U44,
                CountryTag.U45,
                CountryTag.U46,
                CountryTag.U47,
                CountryTag.U48,
                CountryTag.U49,
                CountryTag.U50,
                CountryTag.U51,
                CountryTag.U52,
                CountryTag.U53,
                CountryTag.U54,
                CountryTag.U55,
                CountryTag.U56,
                CountryTag.U57,
                CountryTag.U58,
                CountryTag.U59,
                CountryTag.U60,
                CountryTag.U61,
                CountryTag.U62,
                CountryTag.U63,
                CountryTag.U64,
                CountryTag.U65,
                CountryTag.U66,
                CountryTag.U67,
                CountryTag.U68,
                CountryTag.U69,
                CountryTag.U70,
                CountryTag.U71,
                CountryTag.U72,
                CountryTag.U73,
                CountryTag.U74,
                CountryTag.U75,
                CountryTag.U76,
                CountryTag.U77,
                CountryTag.U78,
                CountryTag.U79,
                CountryTag.U80,
                CountryTag.U81,
                CountryTag.U82,
                CountryTag.U83,
                CountryTag.U84,
                CountryTag.U85,
                CountryTag.U86,
                CountryTag.U87,
                CountryTag.U88,
                CountryTag.U89,
                CountryTag.U90,
                CountryTag.U91,
                CountryTag.U92,
                CountryTag.U93,
                CountryTag.U94,
                CountryTag.U95,
                CountryTag.U96,
                CountryTag.U97,
                CountryTag.U98,
                CountryTag.U99
            };

        /// <summary>
        ///     国タグ一覧 (AoD)
        /// </summary>
        private static readonly CountryTag[] TagsAoD =
            {
                CountryTag.AFG,
                CountryTag.ALB,
                CountryTag.ALG,
                CountryTag.ALI,
                CountryTag.ALS,
                CountryTag.ANG,
                CountryTag.ARA,
                CountryTag.ARG,
                CountryTag.ARM,
                CountryTag.AST,
                CountryTag.AUS,
                CountryTag.AXI,
                CountryTag.AZB,
                CountryTag.BEL,
                CountryTag.BEN,
                CountryTag.BHU,
                CountryTag.BLR,
                CountryTag.BOL,
                CountryTag.BOS,
                CountryTag.BRA,
                CountryTag.BRU,
                CountryTag.BUL,
                CountryTag.BUR,
                CountryTag.CAL,
                CountryTag.CAM,
                CountryTag.CAN,
                CountryTag.CGX,
                CountryTag.CHC,
                CountryTag.CHI,
                CountryTag.CHL,
                CountryTag.CMB,
                CountryTag.COL,
                CountryTag.CON,
                CountryTag.COS,
                CountryTag.CRO,
                CountryTag.CSA,
                CountryTag.CSX,
                CountryTag.CUB,
                CountryTag.CXB,
                CountryTag.CYN,
                CountryTag.CYP,
                CountryTag.CZE,
                CountryTag.DDR,
                CountryTag.DEN,
                CountryTag.DFR,
                CountryTag.DOM,
                CountryTag.EAF,
                CountryTag.ECU,
                CountryTag.EGY,
                CountryTag.ENG,
                CountryTag.EQA,
                CountryTag.EST,
                CountryTag.ETH,
                CountryTag.EUS,
                CountryTag.FIN,
                CountryTag.FLA,
                CountryTag.FRA,
                CountryTag.GAB,
                CountryTag.GEO,
                CountryTag.GER,
                CountryTag.GLD,
                CountryTag.GRE,
                CountryTag.GUA,
                CountryTag.GUI,
                CountryTag.GUY,
                CountryTag.HAI,
                CountryTag.HOL,
                CountryTag.HON,
                CountryTag.HUN,
                CountryTag.ICL,
                CountryTag.IDC,
                CountryTag.IND,
                CountryTag.INO,
                CountryTag.IRE,
                CountryTag.IRQ,
                CountryTag.ISR,
                CountryTag.ITA,
                CountryTag.JAP,
                CountryTag.JOR,
                CountryTag.KAZ,
                CountryTag.KOR,
                CountryTag.KUR,
                CountryTag.KYG,
                CountryTag.LAO,
                CountryTag.LAT,
                CountryTag.LBY,
                CountryTag.LEB,
                CountryTag.LIB,
                CountryTag.LIT,
                CountryTag.LUX,
                CountryTag.MAD,
                CountryTag.MAL,
                CountryTag.MAN,
                CountryTag.MEN,
                CountryTag.MEX,
                CountryTag.MLY,
                CountryTag.MON,
                CountryTag.MOR,
                CountryTag.MOZ,
                CountryTag.MTN,
                CountryTag.NAM,
                CountryTag.NEP,
                CountryTag.NIC,
                CountryTag.NIG,
                CountryTag.NOR,
                CountryTag.NZL,
                CountryTag.OMN,
                CountryTag.OTT,
                CountryTag.PAK,
                CountryTag.PAL,
                CountryTag.PAN,
                CountryTag.PAR,
                CountryTag.PER,
                CountryTag.PHI,
                CountryTag.POL,
                CountryTag.POR,
                CountryTag.PRI,
                CountryTag.PRK,
                CountryTag.PRU,
                CountryTag.QUE,
                CountryTag.RHO,
                CountryTag.ROM,
                CountryTag.RSI,
                CountryTag.RUS,
                CountryTag.SAF,
                CountryTag.SAL,
                CountryTag.SAR,
                CountryTag.SAU,
                CountryTag.SCA,
                CountryTag.SCH,
                CountryTag.SCO,
                CountryTag.SER,
                CountryTag.SIA,
                CountryTag.SIB,
                CountryTag.SIE,
                CountryTag.SIK,
                CountryTag.SLO,
                CountryTag.SLV,
                CountryTag.SOM,
                CountryTag.SOV,
                CountryTag.SPA,
                CountryTag.SPR,
                CountryTag.SUD,
                CountryTag.SWE,
                CountryTag.SYR,
                CountryTag.TAJ,
                CountryTag.TAN,
                CountryTag.TEX,
                CountryTag.TIB,
                CountryTag.TRA,
                CountryTag.TRK,
                CountryTag.TUN,
                CountryTag.TUR,
                CountryTag.UAP,
                CountryTag.UAU,
                CountryTag.UBO,
                CountryTag.UCH,
                CountryTag.UCS,
                CountryTag.UER,
                CountryTag.UES,
                CountryTag.UGS,
                CountryTag.UIC,
                CountryTag.UIR,
                CountryTag.UKR,
                CountryTag.UPE,
                CountryTag.UPR,
                CountryTag.UPS,
                CountryTag.URO,
                CountryTag.URU,
                CountryTag.USA,
                CountryTag.USN,
                CountryTag.UTC,
                CountryTag.UTL,
                CountryTag.UTO,
                CountryTag.UZB,
                CountryTag.VEN,
                CountryTag.VIC,
                CountryTag.VIE,
                CountryTag.WLL,
                CountryTag.YEM,
                CountryTag.YUG,
                CountryTag.U00,
                CountryTag.U01,
                CountryTag.U02,
                CountryTag.U03,
                CountryTag.U04,
                CountryTag.U05,
                CountryTag.U06,
                CountryTag.U07,
                CountryTag.U08,
                CountryTag.U09,
                CountryTag.U10,
                CountryTag.U11,
                CountryTag.U12,
                CountryTag.U13,
                CountryTag.U14,
                CountryTag.U15,
                CountryTag.U16,
                CountryTag.U17,
                CountryTag.U18,
                CountryTag.U19,
                CountryTag.U20,
                CountryTag.U21,
                CountryTag.U22,
                CountryTag.U23,
                CountryTag.U24,
                CountryTag.U25,
                CountryTag.U26,
                CountryTag.U27,
                CountryTag.U28,
                CountryTag.U29,
                CountryTag.U30,
                CountryTag.U31,
                CountryTag.U32,
                CountryTag.U33,
                CountryTag.U34,
                CountryTag.U35,
                CountryTag.U36,
                CountryTag.U37,
                CountryTag.U38,
                CountryTag.U39,
                CountryTag.U40,
                CountryTag.U41,
                CountryTag.U42,
                CountryTag.U43,
                CountryTag.U44,
                CountryTag.U45,
                CountryTag.U46,
                CountryTag.U47,
                CountryTag.U48,
                CountryTag.U49,
                CountryTag.U50,
                CountryTag.U51,
                CountryTag.U52,
                CountryTag.U53,
                CountryTag.U54,
                CountryTag.U55,
                CountryTag.U56,
                CountryTag.U57,
                CountryTag.U58,
                CountryTag.U59,
                CountryTag.U60,
                CountryTag.U61,
                CountryTag.U62,
                CountryTag.U63,
                CountryTag.U64,
                CountryTag.U65,
                CountryTag.U66,
                CountryTag.U67,
                CountryTag.U68,
                CountryTag.U69,
                CountryTag.U70,
                CountryTag.U71,
                CountryTag.U72,
                CountryTag.U73,
                CountryTag.U74,
                CountryTag.U75,
                CountryTag.U76,
                CountryTag.U77,
                CountryTag.U78,
                CountryTag.U79,
                CountryTag.U80,
                CountryTag.U81,
                CountryTag.U82,
                CountryTag.U83,
                CountryTag.U84,
                CountryTag.U85,
                CountryTag.U86,
                CountryTag.U87,
                CountryTag.U88,
                CountryTag.U89,
                CountryTag.U90,
                CountryTag.U91,
                CountryTag.U92,
                CountryTag.U93,
                CountryTag.U94,
                CountryTag.U95,
                CountryTag.U96,
                CountryTag.U97,
                CountryTag.U98,
                CountryTag.U99,
                CountryTag.UA0,
                CountryTag.UA1,
                CountryTag.UA2,
                CountryTag.UA3,
                CountryTag.UA4,
                CountryTag.UA5,
                CountryTag.UA6,
                CountryTag.UA7,
                CountryTag.UA8,
                CountryTag.UA9,
                CountryTag.UB0,
                CountryTag.UB1,
                CountryTag.UB2,
                CountryTag.UB3,
                CountryTag.UB4,
                CountryTag.UB5,
                CountryTag.UB6,
                CountryTag.UB7,
                CountryTag.UB8,
                CountryTag.UB9,
                CountryTag.UC0,
                CountryTag.UC1,
                CountryTag.UC2,
                CountryTag.UC3,
                CountryTag.UC4,
                CountryTag.UC5,
                CountryTag.UC6,
                CountryTag.UC7,
                CountryTag.UC8,
                CountryTag.UC9,
                CountryTag.UD0,
                CountryTag.UD1,
                CountryTag.UD2,
                CountryTag.UD3,
                CountryTag.UD4,
                CountryTag.UD5,
                CountryTag.UD6,
                CountryTag.UD7,
                CountryTag.UD8,
                CountryTag.UD9,
                CountryTag.UE0,
                CountryTag.UE1,
                CountryTag.UE2,
                CountryTag.UE3,
                CountryTag.UE4,
                CountryTag.UE5,
                CountryTag.UE6,
                CountryTag.UE7,
                CountryTag.UE8,
                CountryTag.UE9,
                CountryTag.UF0,
                CountryTag.UF1,
                CountryTag.UF2,
                CountryTag.UF3,
                CountryTag.UF4,
                CountryTag.UF5,
                CountryTag.UF6,
                CountryTag.UF7,
                CountryTag.UF8,
                CountryTag.UF9
            };

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Country()
        {
            StringMap = new Dictionary<string, CountryTag>();
            foreach (CountryTag country in Enum.GetValues(typeof (CountryTag)))
            {
                StringMap.Add(Strings[(int) country], country);
            }
        }

        /// <summary>
        ///     国タグ一覧を初期化する
        /// </summary>
        public static void Init()
        {
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.DarkestHour:
                    Tags = TagsHoI2;
                    break;

                case GameType.ArsenalOfDemocracy:
                    Tags = TagsAoD;
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    ///     国タグ
    /// </summary>
    public enum CountryTag
    {
        None, // 定義なし

        // ReSharper disable InconsistentNaming
        AFG,
        ALB,
        ALG,
        ALI,
        ALS,
        ANG,
        ARA,
        ARG,
        ARM,
        AST,
        AUS,
        AXI,
        AZB,
        BEL,
        BEN,
        BHU,
        BLR,
        BOL,
        BOS,
        BRA,
        BRU,
        BUL,
        BUR,
        CAL,
        CAM,
        CAN,
        CGX,
        CHC,
        CHI,
        CHL,
        CMB,
        COL,
        CON,
        COS,
        CRO,
        CSA,
        CSX,
        CUB,
        CXB,
        CYN,
        CYP,
        CZE,
        DDR,
        DEN,
        DFR,
        DOM,
        EAF,
        ECU,
        EGY,
        ENG,
        EQA,
        EST,
        ETH,
        EUS,
        FIN,
        FLA,
        FRA,
        GAB,
        GEO,
        GER,
        GLD,
        GRE,
        GUA,
        GUI,
        GUY,
        HAI,
        HOL,
        HON,
        HUN,
        ICL,
        IDC,
        IND,
        INO,
        IRE,
        IRQ,
        ISR,
        ITA,
        JAP,
        JOR,
        KAZ,
        KOR,
        KUR,
        KYG,
        LAO,
        LAT,
        LBY,
        LEB,
        LIB,
        LIT,
        LUX,
        MAD,
        MAL,
        MAN,
        MEN,
        MEX,
        MLY,
        MON,
        MOR,
        MOZ,
        MTN,
        NAM,
        NEP,
        NIC,
        NIG,
        NOR,
        NZL,
        OMN,
        OTT,
        PAK,
        PAL,
        PAN,
        PAR,
        PER,
        PHI,
        POL,
        POR,
        PRI,
        PRK,
        PRU,
        QUE,
        RHO,
        ROM,
        RSI,
        RUS,
        SAF,
        SAL,
        SAR,
        SAU,
        SCA,
        SCH,
        SCO,
        SER,
        SIA,
        SIB,
        SIE,
        SIK,
        SLO,
        SLV,
        SOM,
        SOV,
        SPA,
        SPR,
        SUD,
        SWE,
        SYR,
        TAJ,
        TAN,
        TEX,
        TIB,
        TRA,
        TRK,
        TUN,
        TUR,
        UAP,
        UAU,
        UBO,
        UCH,
        UCS,
        UER,
        UES,
        UGS,
        UIC,
        UIR,
        UKR,
        UPE,
        UPR,
        UPS,
        URO,
        URU,
        USA,
        USN,
        UTC,
        UTL,
        UTO,
        UZB,
        VEN,
        VIC,
        VIE,
        WLL,
        YEM,
        YUG,
        U00,
        U01,
        U02,
        U03,
        U04,
        U05,
        U06,
        U07,
        U08,
        U09,
        U10,
        U11,
        U12,
        U13,
        U14,
        U15,
        U16,
        U17,
        U18,
        U19,
        U20,
        U21,
        U22,
        U23,
        U24,
        U25,
        U26,
        U27,
        U28,
        U29,
        U30,
        U31,
        U32,
        U33,
        U34,
        U35,
        U36,
        U37,
        U38,
        U39,
        U40,
        U41,
        U42,
        U43,
        U44,
        U45,
        U46,
        U47,
        U48,
        U49,
        U50,
        U51,
        U52,
        U53,
        U54,
        U55,
        U56,
        U57,
        U58,
        U59,
        U60,
        U61,
        U62,
        U63,
        U64,
        U65,
        U66,
        U67,
        U68,
        U69,
        U70,
        U71,
        U72,
        U73,
        U74,
        U75,
        U76,
        U77,
        U78,
        U79,
        U80,
        U81,
        U82,
        U83,
        U84,
        U85,
        U86,
        U87,
        U88,
        U89,
        U90,
        U91,
        U92,
        U93,
        U94,
        U95,
        U96,
        U97,
        U98,
        U99,

        // AoDのみ
        UA0,
        UA1,
        UA2,
        UA3,
        UA4,
        UA5,
        UA6,
        UA7,
        UA8,
        UA9,
        UB0,
        UB1,
        UB2,
        UB3,
        UB4,
        UB5,
        UB6,
        UB7,
        UB8,
        UB9,
        UC0,
        UC1,
        UC2,
        UC3,
        UC4,
        UC5,
        UC6,
        UC7,
        UC8,
        UC9,
        UD0,
        UD1,
        UD2,
        UD3,
        UD4,
        UD5,
        UD6,
        UD7,
        UD8,
        UD9,
        UE0,
        UE1,
        UE2,
        UE3,
        UE4,
        UE5,
        UE6,
        UE7,
        UE8,
        UE9,
        UF0,
        UF1,
        UF2,
        UF3,
        UF4,
        UF5,
        UF6,
        UF7,
        UF8,
        UF9
        // ReSharper restore InconsistentNaming
    };
}