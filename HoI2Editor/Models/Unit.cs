using System;
using System.Collections.Generic;
using System.Linq;
using HoI2Editor.Utilities;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ユニットクラス
    /// </summary>
    internal class UnitClass
    {
        #region 公開プロパティ

        /// <summary>
        ///     ユニットの種類
        /// </summary>
        internal UnitType Type { get; }

        /// <summary>
        ///     ユニットの兵科
        /// </summary>
        internal Branch Branch { get; set; }

        /// <summary>
        ///     ユニットの編成
        /// </summary>
        internal UnitOrganization Organization { get; }

        /// <summary>
        ///     名前
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        ///     短縮名
        /// </summary>
        internal string ShortName { get; set; }

        /// <summary>
        ///     説明
        /// </summary>
        internal string Desc { get; set; }

        /// <summary>
        ///     簡易説明
        /// </summary>
        internal string ShortDesc { get; set; }

        /// <summary>
        ///     統計グループ
        /// </summary>
        internal int Eyr { get; set; }

        /// <summary>
        ///     スプライトの種類
        /// </summary>
        internal SpriteType Sprite { get; set; }

        /// <summary>
        ///     生産不可能な時に使用するクラス
        /// </summary>
        internal UnitType Transmute { get; set; }

        /// <summary>
        ///     画像の優先度
        /// </summary>
        internal int GfxPrio { get; set; }

        /// <summary>
        ///     軍事力
        /// </summary>
        internal double Value { get; set; }

        /// <summary>
        ///     リストの優先度
        /// </summary>
        internal int ListPrio { get; set; }

        /// <summary>
        ///     UI優先度
        /// </summary>
        internal int UiPrio { get; set; }

        /// <summary>
        ///     実ユニット種類
        /// </summary>
        internal RealUnitType RealType { get; set; }

        /// <summary>
        ///     最大生産速度
        /// </summary>
        internal int MaxSpeedStep { get; set; }

        /// <summary>
        ///     初期状態で生産可能かどうか
        /// </summary>
        internal bool Productable { get; set; }

        /// <summary>
        ///     空母航空隊かどうか
        /// </summary>
        internal bool Cag { get; set; }

        /// <summary>
        ///     護衛戦闘機かどうか
        /// </summary>
        internal bool Escort { get; set; }

        /// <summary>
        ///     工兵かどうか
        /// </summary>
        internal bool Engineer { get; set; }

        /// <summary>
        ///     標準の生産タイプかどうか
        /// </summary>
        internal bool DefaultType { get; set; }

        /// <summary>
        ///     旅団が着脱可能か
        /// </summary>
        internal bool Detachable { get; set; }

        /// <summary>
        ///     最大旅団数 (-1の場合未定義)
        /// </summary>
        internal int MaxAllowedBrigades { get; set; }

        /// <summary>
        ///     付属可能旅団
        /// </summary>
        internal List<UnitType> AllowedBrigades { get; private set; }

        /// <summary>
        ///     モデルリスト
        /// </summary>
        internal List<UnitModel> Models { get; }

        /// <summary>
        ///     ユニット更新情報
        /// </summary>
        internal List<UnitUpgrade> Upgrades { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     付属可能旅団の編集済みフラグ
        /// </summary>
        private readonly List<UnitType> _dirtyBrigades = new List<UnitType>();

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (UnitClassItemId)).Length];

        /// <summary>
        ///     ユニット定義ファイルの編集済みフラグ
        /// </summary>
        private bool _dirtyFileFlag;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        /// <summary>
        ///     実体存在フラグ
        /// </summary>
        /// <remarks>
        ///     d_rsv_33～d_rsv_40、b_rsv_36～b_rsv_40はこの値がfalseならばlist_prioのみ保存される
        ///     d_01～d_99、b_01～b_99はこの値がfalseならば保存されない
        /// </remarks>
        private bool _entityFlag;

        #endregion

        #region 内部定数

        /// <summary>
        ///     兵科の初期設定値
        /// </summary>
        private static readonly Branch[] DefaultBranches =
        {
            Branch.None,
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
            Branch.Army,
            Branch.Airforce,
            Branch.Airforce,
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
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Airforce,
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
            Branch.Navy,
            Branch.Airforce,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
            Branch.Navy,
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
            Branch.Army,
            Branch.Navy,
            Branch.Navy,
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
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army,
            Branch.Army
        };

        /// <summary>
        ///     編成の初期設定値
        /// </summary>
        private static readonly UnitOrganization[] DefaultOrganizations =
        {
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Division,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade,
            UnitOrganization.Brigade
        };

        /// <summary>
        ///     ユニット名の初期設定値
        /// </summary>
        private static readonly string[] DefaultNames =
        {
            "",
            "INFANTRY",
            "CAVALRY",
            "MOTORIZED",
            "MECHANIZED",
            "LIGHT_ARMOR",
            "ARMOR",
            "PARATROOPER",
            "MARINE",
            "BERGSJAEGER",
            "GARRISON",
            "HQ",
            "MILITIA",
            "MULTI_ROLE",
            "INTERCEPTOR",
            "STRATEGIC_BOMBER",
            "TACTICAL_BOMBER",
            "NAVAL_BOMBER",
            "CAS",
            "TRANSPORT_PLANE",
            "FLYING_BOMB",
            "FLYING_ROCKET",
            "BATTLESHIP",
            "LIGHT_CRUISER",
            "HEAVY_CRUISER",
            "BATTLECRUISER",
            "DESTROYER",
            "CARRIER",
            "ESCORT_CARRIER",
            "SUBMARINE",
            "NUCLEAR_SUBMARINE",
            "TRANSPORT",
            "LIGHT_CARRIER",
            "ROCKET_INTERCEPTOR",
            "D_RSV_33",
            "D_RSV_34",
            "D_RSV_35",
            "D_RSV_36",
            "D_RSV_37",
            "D_RSV_38",
            "D_RSV_39",
            "D_RSV_40",
            "D_01",
            "D_02",
            "D_03",
            "D_04",
            "D_05",
            "D_06",
            "D_07",
            "D_08",
            "D_09",
            "D_10",
            "D_11",
            "D_12",
            "D_13",
            "D_14",
            "D_15",
            "D_16",
            "D_17",
            "D_18",
            "D_19",
            "D_20",
            "D_21",
            "D_22",
            "D_23",
            "D_24",
            "D_25",
            "D_26",
            "D_27",
            "D_28",
            "D_29",
            "D_30",
            "D_31",
            "D_32",
            "D_33",
            "D_34",
            "D_35",
            "D_36",
            "D_37",
            "D_38",
            "D_39",
            "D_40",
            "D_41",
            "D_42",
            "D_43",
            "D_44",
            "D_45",
            "D_46",
            "D_47",
            "D_48",
            "D_49",
            "D_50",
            "D_51",
            "D_52",
            "D_53",
            "D_54",
            "D_55",
            "D_56",
            "D_57",
            "D_58",
            "D_59",
            "D_60",
            "D_61",
            "D_62",
            "D_63",
            "D_64",
            "D_65",
            "D_66",
            "D_67",
            "D_68",
            "D_69",
            "D_70",
            "D_71",
            "D_72",
            "D_73",
            "D_74",
            "D_75",
            "D_76",
            "D_77",
            "D_78",
            "D_79",
            "D_80",
            "D_81",
            "D_82",
            "D_83",
            "D_84",
            "D_85",
            "D_86",
            "D_87",
            "D_88",
            "D_89",
            "D_90",
            "D_91",
            "D_92",
            "D_93",
            "D_94",
            "D_95",
            "D_96",
            "D_97",
            "D_98",
            "D_99",
            "NONE",
            "ARTILLERY",
            "SP_ARTILLERY",
            "ROCKET_ARTILLERY",
            "SP_ROCKET_ARTILLERY",
            "ANTITANK",
            "TANK_DESTROYER",
            "LIGHT_ARMOR",
            "HEAVY_ARMOR",
            "SUPER_HEAVY_ARMOR",
            "ARMORED_CAR",
            "ANTIAIR",
            "POLICE",
            "ENGINEER",
            "CAG",
            "ESCORT",
            "NAVAL_ASW",
            "NAVAL_ANTI_AIR_S",
            "NAVAL_RADAR_S",
            "NAVAL_FIRE_CONTROLL_S",
            "NAVAL_IMPROVED_HULL_S",
            "NAVAL_TORPEDOES_S",
            "NAVAL_ANTI_AIR_L",
            "NAVAL_RADAR_L",
            "NAVAL_FIRE_CONTROLL_L",
            "NAVAL_IMPROVED_HULL_L",
            "NAVAL_TORPEDOES_L",
            "NAVAL_MINES",
            "NAVAL_SA_L",
            "NAVAL_SPOTTER_L",
            "NAVAL_SPOTTER_S",
            "B_U1",
            "B_U2",
            "B_U3",
            "B_U4",
            "B_U5",
            "B_U6",
            "B_U7",
            "B_U8",
            "B_U9",
            "B_U10",
            "B_U11",
            "B_U12",
            "B_U13",
            "B_U14",
            "B_U15",
            "B_U16",
            "B_U17",
            "B_U18",
            "B_U19",
            "B_U20",
            "CAVALRY_BRIGADE",
            "SP_ANTI_AIR",
            "MEDIUM_ARMOR",
            "FLOATPLANE",
            "LCAG",
            "AMPH_LIGHT_ARMOR_BRIGADE",
            "GLI_LIGHT_ARMOR_BRIGADE",
            "GLI_LIGHT_ARTILLERY",
            "SH_ARTILLERY",
            "B_RSV_36",
            "B_RSV_37",
            "B_RSV_38",
            "B_RSV_39",
            "B_RSV_40",
            "B_01",
            "B_02",
            "B_03",
            "B_04",
            "B_05",
            "B_06",
            "B_07",
            "B_08",
            "B_09",
            "B_10",
            "B_11",
            "B_12",
            "B_13",
            "B_14",
            "B_15",
            "B_16",
            "B_17",
            "B_18",
            "B_19",
            "B_20",
            "B_21",
            "B_22",
            "B_23",
            "B_24",
            "B_25",
            "B_26",
            "B_27",
            "B_28",
            "B_29",
            "B_30",
            "B_31",
            "B_32",
            "B_33",
            "B_34",
            "B_35",
            "B_36",
            "B_37",
            "B_38",
            "B_39",
            "B_40",
            "B_41",
            "B_42",
            "B_43",
            "B_44",
            "B_45",
            "B_46",
            "B_47",
            "B_48",
            "B_49",
            "B_50",
            "B_51",
            "B_52",
            "B_53",
            "B_54",
            "B_55",
            "B_56",
            "B_57",
            "B_58",
            "B_59",
            "B_60",
            "B_61",
            "B_62",
            "B_63",
            "B_64",
            "B_65",
            "B_66",
            "B_67",
            "B_68",
            "B_69",
            "B_70",
            "B_71",
            "B_72",
            "B_73",
            "B_74",
            "B_75",
            "B_76",
            "B_77",
            "B_78",
            "B_79",
            "B_80",
            "B_81",
            "B_82",
            "B_83",
            "B_84",
            "B_85",
            "B_86",
            "B_87",
            "B_88",
            "B_89",
            "B_90",
            "B_91",
            "B_92",
            "B_93",
            "B_94",
            "B_95",
            "B_96",
            "B_97",
            "B_98",
            "B_99"
        };

        /// <summary>
        ///     最大付属旅団数の初期値
        /// </summary>
        private static readonly int[] DefaultMaxBrigades =
        {
            0,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            0,
            0,
            1,
            1,
            1,
            0,
            0,
            0,
            0,
            5,
            2,
            3,
            4,
            1,
            1,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal UnitClass(UnitType type)
        {
            Type = type;
            Branch = DefaultBranches[(int) type];
            Organization = DefaultOrganizations[(int) type];
            ListPrio = -1;
            MaxAllowedBrigades = -1;
            AllowedBrigades = new List<UnitType>();
            Models = new List<UnitModel>();
            Upgrades = new List<UnitUpgrade>();

            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                // 最大生産速度を初期設定
                if (Organization == UnitOrganization.Division)
                {
                    MaxSpeedStep = 2;
                }

                // 旅団の着脱可能を初期設定
                if (Organization == UnitOrganization.Brigade)
                {
                    Detachable = true;
                }
            }

            string s = DefaultNames[(int) Type];
            Name = "NAME_" + s;
            ShortName = "SNAME_" + s;
            Desc = "LDESC_" + s;
            ShortDesc = "SDESC_" + s;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     ユニットクラス名を取得する
        /// </summary>
        /// <returns>ユニットクラス名</returns>
        public override string ToString()
        {
            return Config.ExistsKey(Name) ? Config.GetText(Name) : Units.Strings[(int) Type];
        }

        /// <summary>
        ///     ユニット短縮名を取得する
        /// </summary>
        /// <returns>短縮名</returns>
        internal string GetShortName()
        {
            return Config.ExistsKey(ShortName) ? Config.GetText(ShortName) : "";
        }

        /// <summary>
        ///     ユニット説明を取得する
        /// </summary>
        /// <returns>ユニット説明</returns>
        internal string GetDesc()
        {
            return Config.ExistsKey(Desc) ? Config.GetText(Desc) : "";
        }

        /// <summary>
        ///     ユニット短縮説明を取得する
        /// </summary>
        /// <returns>ユニット短縮説明</returns>
        internal string GetShortDesc()
        {
            return Config.ExistsKey(ShortDesc) ? Config.GetText(ShortDesc) : "";
        }

        #endregion

        #region データアクセス

        /// <summary>
        ///     最大付属旅団数が編集可能かどうかを取得する
        /// </summary>
        /// <returns>編集可能ならばtrueを返す</returns>
        internal bool CanModifyMaxAllowedBrigades()
        {
            // 旅団の場合は編集不可
            if (Organization == UnitOrganization.Brigade)
            {
                return false;
            }

            // DHならば編集可能
            if (Game.Type == GameType.DarkestHour)
            {
                return true;
            }

            // AoD1.07以降ならば艦船ユニットのみ編集可能
            if ((Game.Type == GameType.ArsenalOfDemocracy) && (Game.Version >= 107))
            {
                switch (Type)
                {
                    case UnitType.Transport:
                    case UnitType.Submarine:
                    case UnitType.NuclearSubmarine:
                    case UnitType.Destroyer:
                    case UnitType.LightCruiser:
                    case UnitType.HeavyCruiser:
                    case UnitType.BattleCruiser:
                    case UnitType.BattleShip:
                    case UnitType.EscortCarrier:
                    case UnitType.Carrier:
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     最大付属旅団数を取得する
        /// </summary>
        /// <returns>最大付属旅団数</returns>
        internal int GetMaxAllowedBrigades()
        {
            // 値が設定済みならば設定された値を返す
            if ((Game.Type == GameType.DarkestHour) && (MaxAllowedBrigades >= 0))
            {
                return MaxAllowedBrigades;
            }

            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                // DDAとAoDで最大付属旅団数が異なる箇所を再設定
                if (Type == UnitType.EscortCarrier || Type == UnitType.Cas)
                {
                    return 1;
                }

                // AoD1.07以降の場合はmiscに艦船の最大付属旅団数が定義されている
                if (Game.Version >= 107)
                {
                    switch (Type)
                    {
                        case UnitType.Transport:
                            return Misc.TpMaxAttach;

                        case UnitType.Submarine:
                            return Misc.SsMaxAttach;

                        case UnitType.NuclearSubmarine:
                            return Misc.SsnMaxAttach;

                        case UnitType.Destroyer:
                            return Misc.DdMaxAttach;

                        case UnitType.LightCruiser:
                            return Misc.ClMaxAttach;

                        case UnitType.HeavyCruiser:
                            return Misc.CaMaxAttach;

                        case UnitType.BattleCruiser:
                            return Misc.BcMaxAttach;

                        case UnitType.BattleShip:
                            return Misc.BbMaxAttach;

                        case UnitType.LightCarrier:
                            return Misc.CvlMaxAttach;

                        case UnitType.Carrier:
                            return Misc.CvMaxAttach;
                    }
                }
            }

            // デフォルト設定値を返す
            return DefaultMaxBrigades[(int) Type];
        }

        /// <summary>
        ///     最大付属旅団数を設定する
        /// </summary>
        /// <param name="brigades"></param>
        internal void SetMaxAllowedBrigades(int brigades)
        {
            // 旅団の場合は何もしない
            if (Organization == UnitOrganization.Brigade)
            {
                return;
            }

            // HoI2またはAoD1.07より前の場合は何もしない
            if ((Game.Type == GameType.HeartsOfIron2) ||
                ((Game.Type == GameType.ArsenalOfDemocracy) && (Game.Version < 107)))
            {
                return;
            }

            if (Game.Type == GameType.DarkestHour)
            {
                // DHの場合はユニットの設定値を更新する
                MaxAllowedBrigades = brigades;
            }
            else
            {
                // AoD1.07以降の場合は艦船ユニットのみ値を更新する
                switch (Type)
                {
                    case UnitType.Transport:
                        Misc.TpMaxAttach = brigades;
                        break;

                    case UnitType.Submarine:
                        Misc.SsMaxAttach = brigades;
                        break;

                    case UnitType.NuclearSubmarine:
                        Misc.SsnMaxAttach = brigades;
                        break;

                    case UnitType.Destroyer:
                        Misc.DdMaxAttach = brigades;
                        break;

                    case UnitType.LightCruiser:
                        Misc.ClMaxAttach = brigades;
                        break;

                    case UnitType.HeavyCruiser:
                        Misc.CaMaxAttach = brigades;
                        break;

                    case UnitType.BattleCruiser:
                        Misc.BcMaxAttach = brigades;
                        break;

                    case UnitType.BattleShip:
                        Misc.BbMaxAttach = brigades;
                        break;

                    case UnitType.LightCarrier:
                        Misc.CvlMaxAttach = brigades;
                        break;

                    case UnitType.Carrier:
                        Misc.CvMaxAttach = brigades;
                        break;

                    default:
                        return;
                }
            }

            // 編集済みフラグを設定する
            SetDirty(UnitClassItemId.MaxAllowedBrigades);
        }

        #endregion

        #region ユニットモデルリスト

        /// <summary>
        ///     ユニットモデルを挿入する
        /// </summary>
        /// <param name="model">挿入対象のユニットモデル</param>
        /// <param name="index">挿入する位置</param>
        /// <param name="name">ユニットモデル名</param>
        internal void InsertModel(UnitModel model, int index, string name)
        {
            Log.Info("[Unit] Insert model: {0} ({1})", index, this);

            // 挿入位置以降のユニットモデル名を変更する
            SlideModelNamesDown(index, Models.Count - 1);

            // 挿入位置の国別ユニットモデル名を削除する
            foreach (Country country in Countries.Tags)
            {
                RemoveModelName(index, country);
            }

            // 挿入位置のユニットモデル名を変更する
            SetModelName(index, name);

            // ユニットモデルリストに項目を挿入する
            Models.Insert(index, model);

            // 編集済みフラグを設定する
            model.SetDirtyAll();
            SetDirtyFile();
        }

        /// <summary>
        ///     ユニットモデルを削除する
        /// </summary>
        /// <param name="index">削除する位置</param>
        internal void RemoveModel(int index)
        {
            Log.Info("[Unit] Remove model: {0} ({1})", index, this);

            // 削除位置以降のユニットモデル名を変更する
            SlideModelNamesUp(index + 1, Models.Count - 1);

            // 末尾のユニットモデル名を削除する
            RemoveModelName(Models.Count - 1);
            foreach (Country country in Countries.Tags)
            {
                RemoveModelName(Models.Count - 1, country);
            }

            // ユニットモデルリストから項目を削除する
            Models.RemoveAt(index);

            // 編集済みフラグを設定する
            SetDirtyFile();
        }

        /// <summary>
        ///     ユニットモデルを移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        internal void MoveModel(int src, int dest)
        {
            Log.Info("[Unit] Move model: {0} -> {1} ({2})", src, dest, this);

            UnitModel model = Models[src];

            // ユニットモデルリストの項目を移動する
            if (src > dest)
            {
                // 上へ移動する場合
                Models.Insert(dest, model);
                Models.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                Models.Insert(dest + 1, model);
                Models.RemoveAt(src);
            }

            // 移動元のユニットモデル名を退避する
            string name = GetModelName(src);
            Dictionary<Country, string> names = Countries.Tags.Where(country => ExistsModelName(src, country))
                .ToDictionary(country => country, country => GetCountryModelName(src, country));

            // 移動元と移動先の間のユニットモデル名を変更する
            if (src > dest)
            {
                // 上へ移動する場合
                SlideModelNamesDown(dest, src - 1);
            }
            else
            {
                // 下へ移動する場合
                SlideModelNamesUp(src + 1, dest);
            }

            // 移動先のユニットモデル名を変更する
            SetModelName(dest, name);
            foreach (KeyValuePair<Country, string> pair in names)
            {
                SetModelName(dest, pair.Key, pair.Value);
            }

            // 編集済みフラグを設定する
            SetDirtyFile();
        }

        #endregion

        #region ユニットモデル名

        /// <summary>
        ///     共通のユニットモデル名を取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデル名</returns>
        internal string GetModelName(int index)
        {
            string key = GetModelNameKey(index);
            return Config.ExistsKey(key) ? Config.GetText(key) : "";
        }

        /// <summary>
        ///     国別のユニットモデル名を取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル名</returns>
        internal string GetCountryModelName(int index, Country country)
        {
            string key = GetModelNameKey(index, country);
            return Config.ExistsKey(key) ? Config.GetText(key) : "";
        }

        /// <summary>
        ///     共通のユニットモデル名を設定する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="s">ユニットモデル名</param>
        private void SetModelName(int index, string s)
        {
            Log.Info("[Unit] Set model name: {0} - {1} ({2})", index, s, this);

            Config.SetText(GetModelNameKey(index), s, Game.UnitTextFileName);
        }

        /// <summary>
        ///     国別のユニットモデル名を設定する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <param name="s">ユニットモデル名</param>
        internal void SetModelName(int index, Country country, string s)
        {
            if (country == Country.None)
            {
                SetModelName(index, s);
                return;
            }

            Log.Info("[Unit] Set country model name: {0} - {1} <{2}> ({3})", index, s, Countries.Strings[(int) country],
                this);

            Config.SetText(GetModelNameKey(index, country), s, Game.ModelTextFileName);
        }

        /// <summary>
        ///     共通のユニットモデル名をコピーする
        /// </summary>
        /// <param name="src">コピー元ユニットモデルのインデックス</param>
        /// <param name="dest">コピー元ユニットモデルのインデックス</param>
        private void CopyModelName(int src, int dest)
        {
            Log.Info("[Unit] Copy model name: {0} -> {1} ({2})", src, dest, this);

            SetModelName(dest, GetModelName(src));
        }

        /// <summary>
        ///     国別のユニットモデル名をコピーする
        /// </summary>
        /// <param name="src">コピー元ユニットモデルのインデックス</param>
        /// <param name="dest">コピー元ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        private void CopyModelName(int src, int dest, Country country)
        {
            if (country == Country.None)
            {
                SetModelName(dest, GetModelName(src));
                return;
            }

            Log.Info("[Unit] Copy country model name: {0} -> {1} <{2}> ({3})", src, dest,
                Countries.Strings[(int) country], this);

            SetModelName(dest, country, GetCountryModelName(src, country));
        }

        /// <summary>
        ///     共通のユニットモデル名を削除する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        private void RemoveModelName(int index)
        {
            // 共通のユニットモデル名が存在しなければ何もしない
            string key = GetModelNameKey(index);
            if (!Config.ExistsKey(key))
            {
                return;
            }

            Log.Info("[Unit] Remove model name: {0} ({1})", index, this);

            Config.RemoveText(key, Game.UnitTextFileName);
        }

        /// <summary>
        ///     国別のユニットモデル名を削除する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        internal void RemoveModelName(int index, Country country)
        {
            if (country == Country.None)
            {
                RemoveModelName(index);
                return;
            }

            // 国別のユニットモデル名が存在しなければ何もしない
            string key = GetModelNameKey(index, country);
            if (!Config.ExistsKey(key))
            {
                return;
            }

            Log.Info("[Unit] Remove country model name: {0} <{1}> ({2})", index, Countries.Strings[(int) country], this);

            Config.RemoveText(key, Game.ModelTextFileName);
        }

        /// <summary>
        ///     ユニットモデル名を1つ上へずらす
        /// </summary>
        /// <param name="start">開始位置</param>
        /// <param name="end">終了位置</param>
        private void SlideModelNamesUp(int start, int end)
        {
            // 開始位置が終了位置よりも後ろならば入れ替える
            if (start > end)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }

            Log.Info("[Unit] Slide model names up: {0} - {1} ({2})", start, end, this);

            // 共通のモデル名を順に上へ移動する
            for (int i = start; i <= end; i++)
            {
                if (ExistsModelName(i))
                {
                    CopyModelName(i, i - 1);
                }
                else
                {
                    if (ExistsModelName(i - 1))
                    {
                        RemoveModelName(i - 1);
                    }
                }
            }

            // 国別のモデル名を順に上へ移動する
            foreach (Country country in Countries.Tags)
            {
                for (int i = start; i <= end; i++)
                {
                    if (ExistsModelName(i, country))
                    {
                        CopyModelName(i, i - 1, country);
                    }
                    else
                    {
                        if (ExistsModelName(i - 1, country))
                        {
                            RemoveModelName(i - 1, country);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     ユニットモデル名を1つ下へずらす
        /// </summary>
        /// <param name="start">開始位置</param>
        /// <param name="end">終了位置</param>
        private void SlideModelNamesDown(int start, int end)
        {
            // 開始位置が終了位置よりも後ろならば入れ替える
            if (start > end)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }

            Log.Info("[Unit] Slide model names down: {0} - {1} ({2})", start, end, this);

            // 共通のモデル名を順に下へ移動する
            for (int i = end; i >= start; i--)
            {
                if (ExistsModelName(i))
                {
                    CopyModelName(i, i + 1);
                }
                else
                {
                    if (ExistsModelName(i + 1))
                    {
                        RemoveModelName(i + 1);
                    }
                }
            }

            // 国別のモデル名を順に下へ移動する
            foreach (Country country in Countries.Tags)
            {
                for (int i = end; i >= start; i--)
                {
                    if (ExistsModelName(i, country))
                    {
                        CopyModelName(i, i + 1, country);
                    }
                    else
                    {
                        if (ExistsModelName(i + 1, country))
                        {
                            RemoveModelName(i + 1, country);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     共通のモデル名が存在するかを取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>モデル名が存在すればtrueを返す</returns>
        private bool ExistsModelName(int index)
        {
            string key = GetModelNameKey(index);
            return !string.IsNullOrEmpty(key) && Config.ExistsKey(key);
        }

        /// <summary>
        ///     国別のモデル名が存在するかを取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <returns>モデル名が存在すればtrueを返す</returns>
        internal bool ExistsModelName(int index, Country country)
        {
            if (country == Country.None)
            {
                return ExistsModelName(index);
            }
            string key = GetModelNameKey(index, country);
            return !string.IsNullOrEmpty(key) && Config.ExistsKey(key);
        }

        /// <summary>
        ///     共通のユニットモデル名のキーを取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデル名のキー</returns>
        private string GetModelNameKey(int index)
        {
            string format = Organization == UnitOrganization.Division ? "MODEL_{0}_{1}" : "BRIG_MODEL_{0}_{1}";
            return string.Format(format, Units.UnitNumbers[(int) Type], index);
        }

        /// <summary>
        ///     国別のユニットモデル名のキーを取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル名のキー</returns>
        private string GetModelNameKey(int index, Country country)
        {
            if (country == Country.None)
            {
                return GetModelNameKey(index);
            }
            string format = Organization == UnitOrganization.Division ? "MODEL_{0}_{1}_{2}" : "BRIG_MODEL_{0}_{1}_{2}";
            return string.Format(format, Countries.Strings[(int) country], Units.UnitNumbers[(int) Type], index);
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニットクラスが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(UnitClassItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        internal void SetDirty(UnitClassItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
            SetEntity();

            switch (id)
            {
                case UnitClassItemId.MaxSpeedStep: // 最大生産速度
                case UnitClassItemId.Detachable: // 旅団が着脱可能か
                    _dirtyFileFlag = true;
                    Units.SetDirty();
                    break;

                case UnitClassItemId.Name:
                case UnitClassItemId.ShortName:
                case UnitClassItemId.Desc:
                case UnitClassItemId.ShortDesc:
                    // 文字列のみの更新となるのでユニット定義ファイルの更新は必要ない
                    break;

                case UnitClassItemId.Eyr: // 統計グループ
                case UnitClassItemId.Sprite: // スプライトの種類
                case UnitClassItemId.Transmute: // 生産不可能な時に使用するクラス
                case UnitClassItemId.GfxPrio: // 画像の優先度
                case UnitClassItemId.Vaule: // 軍事力
                case UnitClassItemId.ListPrio: // リストの優先度
                case UnitClassItemId.UiPrio: // UI優先度
                case UnitClassItemId.RealType: // 実ユニット種類
                case UnitClassItemId.Productable: // 初期状態で生産可能かどうか
                case UnitClassItemId.Cag: // 空母航空隊かどうか
                case UnitClassItemId.Escort: // 護衛戦闘機かどうか
                case UnitClassItemId.Engineer: // 工兵かどうか
                case UnitClassItemId.DefaultType: // 標準の生産タイプかどうか
                    if (Organization == UnitOrganization.Division)
                    {
                        Units.SetDirtyDivisionTypes();
                    }
                    else
                    {
                        Units.SetDirtyBrigadeTypes();
                    }
                    break;

                case UnitClassItemId.Branch: // ユニットの兵科
                    switch (Game.Type)
                    {
                        case GameType.ArsenalOfDemocracy:
                            // AoDの場合ユニット定義ファイルで師団/旅団ともに編集可能
                            _dirtyFileFlag = true;
                            Units.SetDirty();
                            break;

                        case GameType.DarkestHour:
                            // DHの場合師団/旅団定義ファイルで編集可能
                            if (Organization == UnitOrganization.Division)
                            {
                                Units.SetDirtyDivisionTypes();
                            }
                            else
                            {
                                Units.SetDirtyBrigadeTypes();
                            }
                            break;
                    }
                    break;

                case UnitClassItemId.MaxAllowedBrigades: // 最大旅団数
                    switch (Game.Type)
                    {
                        case GameType.ArsenalOfDemocracy:
                            // AoDの場合艦船ユニットのみMisc側で設定するためここでは何もしない
                            break;

                        case GameType.DarkestHour:
                            // DHの場合はユニット定義ファイルで編集可能
                            _dirtyFileFlag = true;
                            Units.SetDirty();
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        internal void SetDirty()
        {
            _dirtyFlag = true;
            SetEntity();
        }

        /// <summary>
        ///     ユニット定義ファイルが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirtyFile()
        {
            return _dirtyFileFlag;
        }

        /// <summary>
        ///     ユニット定義ファイルの編集済みフラグを設定する
        /// </summary>
        /// <returns></returns>
        internal void SetDirtyFile()
        {
            _dirtyFileFlag = true;
            _dirtyFlag = true;
            SetEntity();
            Units.SetDirty();
        }

        /// <summary>
        ///     付属可能旅団が編集済みかどうかを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool IsDirtyAllowedBrigades(UnitType type)
        {
            return _dirtyBrigades.Contains(type);
        }

        /// <summary>
        ///     付属可能旅団の編集済みフラグを設定する
        /// </summary>
        /// <param name="type">旅団の種類</param>
        internal void SetDirtyAllowedBrigades(UnitType type)
        {
            if (!_dirtyBrigades.Contains(type))
            {
                _dirtyBrigades.Add(type);
            }
            _dirtyFileFlag = true;
            _dirtyFlag = true;
            SetEntity();
            Units.SetDirty();
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        internal void ResetDirtyAll()
        {
            foreach (UnitClassItemId id in Enum.GetValues(typeof (UnitClassItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyBrigades.Clear();
            foreach (UnitModel model in Models)
            {
                model.ResetDirtyAll();
            }
            _dirtyFileFlag = false;
            _dirtyFlag = false;
        }

        /// <summary>
        ///     項目の実体が存在するかどうかを取得する
        /// </summary>
        /// <returns>実体が存在すればtrueを返す</returns>
        internal bool ExistsEntity()
        {
            return _entityFlag;
        }

        /// <summary>
        ///     実体存在フラグを設定する
        /// </summary>
        internal void SetEntity()
        {
            // DH1.03以降以外ならば何もしない
            if ((Game.Type != GameType.DarkestHour) || (Game.Version < 103))
            {
                return;
            }

            // 設定済みならば何もしない
            if (_entityFlag)
            {
                return;
            }

            _entityFlag = true;
        }

        #endregion
    }

    /// <summary>
    ///     ユニットモデル
    /// </summary>
    internal class UnitModel
    {
        #region 公開プロパティ

        /// <summary>
        ///     必要IC
        /// </summary>
        internal double Cost { get; set; }

        /// <summary>
        ///     生産に要する時間
        /// </summary>
        internal double BuildTime { get; set; }

        /// <summary>
        ///     必要人的資源
        /// </summary>
        internal double ManPower { get; set; }

        /// <summary>
        ///     移動速度
        /// </summary>
        internal double MaxSpeed { get; set; }

        /// <summary>
        ///     砲兵旅団付随時の速度キャップ
        /// </summary>
        internal double SpeedCapArt { get; set; }

        /// <summary>
        ///     工兵旅団付随時の速度キャップ
        /// </summary>
        internal double SpeedCapEng { get; set; }

        /// <summary>
        ///     対戦車旅団付随時の速度キャップ
        /// </summary>
        internal double SpeedCapAt { get; set; }

        /// <summary>
        ///     対空旅団付随時の速度キャップ
        /// </summary>
        internal double SpeedCapAa { get; set; }

        /// <summary>
        ///     航続距離
        /// </summary>
        internal double Range { get; set; }

        /// <summary>
        ///     組織率
        /// </summary>
        internal double DefaultOrganization { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        internal double Morale { get; set; }

        /// <summary>
        ///     防御力
        /// </summary>
        internal double Defensiveness { get; set; }

        /// <summary>
        ///     対艦/対潜防御力
        /// </summary>
        internal double SeaDefense { get; set; }

        /// <summary>
        ///     対空防御力
        /// </summary>
        internal double AirDefence { get; set; }

        /// <summary>
        ///     対地/対艦防御力
        /// </summary>
        internal double SurfaceDefence { get; set; }

        /// <summary>
        ///     耐久力
        /// </summary>
        internal double Toughness { get; set; }

        /// <summary>
        ///     脆弱性
        /// </summary>
        internal double Softness { get; set; }

        /// <summary>
        ///     制圧力
        /// </summary>
        internal double Suppression { get; set; }

        /// <summary>
        ///     対人攻撃力
        /// </summary>
        internal double SoftAttack { get; set; }

        /// <summary>
        ///     対甲攻撃力
        /// </summary>
        internal double HardAttack { get; set; }

        /// <summary>
        ///     対艦攻撃力(海軍)
        /// </summary>
        internal double SeaAttack { get; set; }

        /// <summary>
        ///     対潜攻撃力
        /// </summary>
        internal double SubAttack { get; set; }

        /// <summary>
        ///     通商破壊力
        /// </summary>
        internal double ConvoyAttack { get; set; }

        /// <summary>
        ///     湾岸攻撃力
        /// </summary>
        internal double ShoreBombardment { get; set; }

        /// <summary>
        ///     対空攻撃力
        /// </summary>
        internal double AirAttack { get; set; }

        /// <summary>
        ///     対艦攻撃力(空軍)
        /// </summary>
        internal double NavalAttack { get; set; }

        /// <summary>
        ///     戦略爆撃力
        /// </summary>
        internal double StrategicAttack { get; set; }

        /// <summary>
        ///     射程距離
        /// </summary>
        internal double Distance { get; set; }

        /// <summary>
        ///     対艦索敵能力
        /// </summary>
        internal double SurfaceDetectionCapability { get; set; }

        /// <summary>
        ///     対潜索敵能力
        /// </summary>
        internal double SubDetectionCapability { get; set; }

        /// <summary>
        ///     対空索敵能力
        /// </summary>
        internal double AirDetectionCapability { get; set; }

        /// <summary>
        ///     可視性
        /// </summary>
        internal double Visibility { get; set; }

        /// <summary>
        ///     所要TC
        /// </summary>
        internal double TransportWeight { get; set; }

        /// <summary>
        ///     輸送能力
        /// </summary>
        internal double TransportCapability { get; set; }

        /// <summary>
        ///     消費物資
        /// </summary>
        internal double SupplyConsumption { get; set; }

        /// <summary>
        ///     消費燃料
        /// </summary>
        internal double FuelConsumption { get; set; }

        /// <summary>
        ///     改良時間補正
        /// </summary>
        internal double UpgradeTimeFactor { get; set; }

        /// <summary>
        ///     改良IC補正
        /// </summary>
        internal double UpgradeCostFactor { get; set; }

        /// <summary>
        ///     砲撃攻撃力 (AoD)
        /// </summary>
        internal double ArtilleryBombardment { get; set; }

        /// <summary>
        ///     最大携行物資 (AoD)
        /// </summary>
        internal double MaxSupplyStock { get; set; }

        /// <summary>
        ///     最大携行燃料 (AoD)
        /// </summary>
        internal double MaxOilStock { get; set; }

        /// <summary>
        ///     燃料切れ時の戦闘補正 (DH)
        /// </summary>
        internal double NoFuelCombatMod { get; set; }

        /// <summary>
        ///     補充時間補正 (DH)
        /// </summary>
        internal double ReinforceTimeFactor { get; set; }

        /// <summary>
        ///     補充IC補正 (DH)
        /// </summary>
        internal double ReinforceCostFactor { get; set; }

        /// <summary>
        ///     改良時間の補正をするか (DH)
        /// </summary>
        internal bool UpgradeTimeBoost { get; set; } = true;

        /// <summary>
        ///     他師団への自動改良を許可するか (DH)
        /// </summary>
        internal bool AutoUpgrade { get; set; }

        /// <summary>
        ///     自動改良先のユニットクラス (DH)
        /// </summary>
        internal UnitType UpgradeClass { get; set; }

        /// <summary>
        ///     自動改良先モデル番号 (DH)
        /// </summary>
        internal int UpgradeModel { get; set; }

        /// <summary>
        ///     速度キャップ (DH1.03以降)
        /// </summary>
        internal double SpeedCap { get; set; }

        /// <summary>
        ///     装備 (DH1.03以降)
        /// </summary>
        internal List<UnitEquipment> Equipments { get; } = new List<UnitEquipment>();

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (UnitModelItemId)).Length];

        /// <summary>
        ///     国別モデル名の編集済みフラグ
        /// </summary>
        private readonly bool[] _nameDirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal UnitModel()
        {
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元のユニットモデル</param>
        internal UnitModel(UnitModel original)
        {
            Cost = original.Cost;
            BuildTime = original.BuildTime;
            ManPower = original.ManPower;
            MaxSpeed = original.MaxSpeed;
            SpeedCapArt = original.SpeedCapArt;
            SpeedCapEng = original.SpeedCapEng;
            SpeedCapAt = original.SpeedCapAt;
            SpeedCapAa = original.SpeedCapAa;
            Range = original.Range;
            DefaultOrganization = original.DefaultOrganization;
            Morale = original.Morale;
            Defensiveness = original.Defensiveness;
            SeaDefense = original.SeaDefense;
            AirDefence = original.AirDefence;
            SurfaceDefence = original.SurfaceDefence;
            Toughness = original.Toughness;
            Softness = original.Softness;
            Suppression = original.Suppression;
            SoftAttack = original.SoftAttack;
            HardAttack = original.HardAttack;
            SeaAttack = original.SeaAttack;
            SubAttack = original.SubAttack;
            ConvoyAttack = original.ConvoyAttack;
            ShoreBombardment = original.ShoreBombardment;
            AirAttack = original.AirAttack;
            NavalAttack = original.NavalAttack;
            StrategicAttack = original.StrategicAttack;
            Distance = original.Distance;
            SurfaceDetectionCapability = original.SurfaceDetectionCapability;
            SubDetectionCapability = original.SubDetectionCapability;
            AirDetectionCapability = original.AirDetectionCapability;
            Visibility = original.Visibility;
            TransportWeight = original.TransportWeight;
            TransportCapability = original.TransportCapability;
            SupplyConsumption = original.SupplyConsumption;
            FuelConsumption = original.FuelConsumption;
            UpgradeTimeFactor = original.UpgradeTimeFactor;
            UpgradeCostFactor = original.UpgradeCostFactor;
            ArtilleryBombardment = original.ArtilleryBombardment;
            MaxSupplyStock = original.MaxSupplyStock;
            MaxOilStock = original.MaxOilStock;
            NoFuelCombatMod = original.NoFuelCombatMod;
            ReinforceTimeFactor = original.ReinforceTimeFactor;
            ReinforceCostFactor = original.ReinforceCostFactor;
            UpgradeTimeBoost = original.UpgradeTimeBoost;
            AutoUpgrade = original.AutoUpgrade;
            UpgradeClass = original.UpgradeClass;
            UpgradeModel = original.UpgradeModel;
            SpeedCap = original.SpeedCap;
            foreach (UnitEquipment equipment in original.Equipments)
            {
                Equipments.Add(new UnitEquipment(equipment));
            }
        }

        #endregion

        #region 装備リスト

        /// <summary>
        ///     装備の項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        internal void MoveEquipment(int src, int dest)
        {
            UnitEquipment equipment = Equipments[src];

            if (src > dest)
            {
                // 上へ移動する場合
                Equipments.Insert(dest, equipment);
                Equipments.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                Equipments.Insert(dest + 1, equipment);
                Equipments.RemoveAt(src);
            }
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニットモデルが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(UnitModelItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     国別モデル名が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirtyName(Country country)
        {
            return country == Country.None ? _dirtyFlags[(int) UnitModelItemId.Name] : _nameDirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        internal void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        internal void SetDirty(UnitModelItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     国別モデル名の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">項目ID</param>
        internal void SetDirtyName(Country country)
        {
            if (country == Country.None)
            {
                _dirtyFlags[(int) UnitModelItemId.Name] = true;
            }
            else
            {
                _nameDirtyFlags[(int) country] = true;
            }
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        internal void SetDirtyAll()
        {
            foreach (UnitModelItemId id in Enum.GetValues(typeof (UnitModelItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        internal void ResetDirtyAll()
        {
            foreach (UnitModelItemId id in Enum.GetValues(typeof (UnitModelItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (Country country in Countries.Tags)
            {
                _nameDirtyFlags[(int) country] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     ユニット装備情報
    /// </summary>
    internal class UnitEquipment
    {
        #region 公開プロパティ

        /// <summary>
        ///     資源
        /// </summary>
        internal EquipmentType Resource { get; set; }

        /// <summary>
        ///     量
        /// </summary>
        internal double Quantity { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (UnitEquipmentItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal UnitEquipment()
        {
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元のユニット装備情報</param>
        internal UnitEquipment(UnitEquipment original)
        {
            Resource = original.Resource;
            Quantity = original.Quantity;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニット装備情報が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(UnitEquipmentItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        internal void SetDirty(UnitEquipmentItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        internal void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        internal void SetDirtyAll()
        {
            foreach (UnitEquipmentItemId id in Enum.GetValues(typeof (UnitEquipmentItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        internal void ResetDirtyAll()
        {
            foreach (UnitEquipmentItemId id in Enum.GetValues(typeof (UnitEquipmentItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     ユニット更新情報
    /// </summary>
    internal class UnitUpgrade
    {
        #region 公開プロパティ

        /// <summary>
        ///     ユニットの種類
        /// </summary>
        internal UnitType Type { get; set; }

        /// <summary>
        ///     改良時間補正
        /// </summary>
        internal double UpgradeTimeFactor { get; set; }

        /// <summary>
        ///     改良IC補正
        /// </summary>
        internal double UpgradeCostFactor { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (UnitUpgradeItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニット更新情報が編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(UnitUpgradeItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        internal void SetDirty(UnitUpgradeItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        internal void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        internal void SetDirtyAll()
        {
            foreach (UnitUpgradeItemId id in Enum.GetValues(typeof (UnitUpgradeItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        internal void ResetDirtyAll()
        {
            foreach (UnitUpgradeItemId id in Enum.GetValues(typeof (UnitUpgradeItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     ユニットの編成
    /// </summary>
    internal enum UnitOrganization
    {
        Division, // 師団
        Brigade // 旅団
    }

    /// <summary>
    ///     ユニットの種類
    /// </summary>
    internal enum UnitType
    {
        Undefined, // 未定義

        // 師団
        Infantry,
        Cavalry,
        Motorized,
        Mechanized,
        LightArmor,
        Armor,
        Paratrooper,
        Marine,
        Bergsjaeger,
        Garrison,
        Hq,
        Militia,
        MultiRole,
        Interceptor,
        StrategicBomber,
        TacticalBomber,
        NavalBomber,
        Cas,
        TransportPlane,
        FlyingBomb,
        FlyingRocket,
        BattleShip,
        LightCruiser,
        HeavyCruiser,
        BattleCruiser,
        Destroyer,
        Carrier,
        EscortCarrier,
        Submarine,
        NuclearSubmarine,
        Transport,
        // DH1.03のみ
        LightCarrier,
        RocketInterceptor,
        ReserveDivision33,
        ReserveDivision34,
        ReserveDivision35,
        ReserveDivision36,
        ReserveDivision37,
        ReserveDivision38,
        ReserveDivision39,
        ReserveDivision40,
        Division01,
        Division02,
        Division03,
        Division04,
        Division05,
        Division06,
        Division07,
        Division08,
        Division09,
        Division10,
        Division11,
        Division12,
        Division13,
        Division14,
        Division15,
        Division16,
        Division17,
        Division18,
        Division19,
        Division20,
        Division21,
        Division22,
        Division23,
        Division24,
        Division25,
        Division26,
        Division27,
        Division28,
        Division29,
        Division30,
        Division31,
        Division32,
        Division33,
        Division34,
        Division35,
        Division36,
        Division37,
        Division38,
        Division39,
        Division40,
        Division41,
        Division42,
        Division43,
        Division44,
        Division45,
        Division46,
        Division47,
        Division48,
        Division49,
        Division50,
        Division51,
        Division52,
        Division53,
        Division54,
        Division55,
        Division56,
        Division57,
        Division58,
        Division59,
        Division60,
        Division61,
        Division62,
        Division63,
        Division64,
        Division65,
        Division66,
        Division67,
        Division68,
        Division69,
        Division70,
        Division71,
        Division72,
        Division73,
        Division74,
        Division75,
        Division76,
        Division77,
        Division78,
        Division79,
        Division80,
        Division81,
        Division82,
        Division83,
        Division84,
        Division85,
        Division86,
        Division87,
        Division88,
        Division89,
        Division90,
        Division91,
        Division92,
        Division93,
        Division94,
        Division95,
        Division96,
        Division97,
        Division98,
        Division99,

        // 旅団
        None,
        Artillery,
        SpArtillery,
        RocketArtillery,
        SpRctArtillery,
        AntiTank,
        TankDestroyer,
        LightArmorBrigade,
        HeavyArmor,
        SuperHeavyArmor,
        ArmoredCar,
        AntiAir,
        Police,
        Engineer,
        Cag,
        Escort,
        NavalAsw,
        NavalAntiAirS,
        NavalRadarS,
        NavalFireControllS,
        NavalImprovedHullS,
        NavalTorpedoesS,
        NavalAntiAirL,
        NavalRadarL,
        NavalFireControllL,
        NavalImprovedHullL,
        NavalTorpedoesL,
        // AoDのみ
        NavalMines,
        NavalSaL,
        NavalSpotterL,
        NavalSpotterS,
        ExtraBrigade1,
        ExtraBrigade2,
        ExtraBrigade3,
        ExtraBrigade4,
        ExtraBrigade5,
        ExtraBrigade6,
        ExtraBrigade7,
        ExtraBrigade8,
        ExtraBrigade9,
        ExtraBrigade10,
        ExtraBrigade11,
        ExtraBrigade12,
        ExtraBrigade13,
        ExtraBrigade14,
        ExtraBrigade15,
        ExtraBrigade16,
        ExtraBrigade17,
        ExtraBrigade18,
        ExtraBrigade19,
        ExtraBrigade20,
        // DH1.03のみ
        CavalryBrigade,
        SpAntiAir,
        MediumArmor,
        FloatPlane,
        LightCag,
        AmphArmor,
        GliderArmor,
        GliderArtillery,
        SuperHeavyArtillery,
        ReserveBrigade36,
        ReserveBrigade37,
        ReserveBrigade38,
        ReserveBrigade39,
        ReserveBrigade40,
        Brigade01,
        Brigade02,
        Brigade03,
        Brigade04,
        Brigade05,
        Brigade06,
        Brigade07,
        Brigade08,
        Brigade09,
        Brigade10,
        Brigade11,
        Brigade12,
        Brigade13,
        Brigade14,
        Brigade15,
        Brigade16,
        Brigade17,
        Brigade18,
        Brigade19,
        Brigade20,
        Brigade21,
        Brigade22,
        Brigade23,
        Brigade24,
        Brigade25,
        Brigade26,
        Brigade27,
        Brigade28,
        Brigade29,
        Brigade30,
        Brigade31,
        Brigade32,
        Brigade33,
        Brigade34,
        Brigade35,
        Brigade36,
        Brigade37,
        Brigade38,
        Brigade39,
        Brigade40,
        Brigade41,
        Brigade42,
        Brigade43,
        Brigade44,
        Brigade45,
        Brigade46,
        Brigade47,
        Brigade48,
        Brigade49,
        Brigade50,
        Brigade51,
        Brigade52,
        Brigade53,
        Brigade54,
        Brigade55,
        Brigade56,
        Brigade57,
        Brigade58,
        Brigade59,
        Brigade60,
        Brigade61,
        Brigade62,
        Brigade63,
        Brigade64,
        Brigade65,
        Brigade66,
        Brigade67,
        Brigade68,
        Brigade69,
        Brigade70,
        Brigade71,
        Brigade72,
        Brigade73,
        Brigade74,
        Brigade75,
        Brigade76,
        Brigade77,
        Brigade78,
        Brigade79,
        Brigade80,
        Brigade81,
        Brigade82,
        Brigade83,
        Brigade84,
        Brigade85,
        Brigade86,
        Brigade87,
        Brigade88,
        Brigade89,
        Brigade90,
        Brigade91,
        Brigade92,
        Brigade93,
        Brigade94,
        Brigade95,
        Brigade96,
        Brigade97,
        Brigade98,
        Brigade99
    }

    /// <summary>
    ///     実ユニット種類 (DH1.03以降用)
    /// </summary>
    /// <remarks>
    ///     AIの生産に制限をかけるために使用する
    ///     生産AI: Militia/Infantry
    ///     パルチザン: Militia/Infantry
    ///     エイリアン: Infantry/Armor/StrategicBomber/Interceptor/Destroyer/Carrier
    /// </remarks>
    internal enum RealUnitType
    {
        Infantry,
        Cavalry,
        Motorized,
        Mechanized,
        LightArmor,
        Armor,
        Garrison,
        Hq,
        Paratrooper,
        Marine,
        Bergsjaeger,
        Cas,
        MultiRole,
        Interceptor,
        StrategicBomber,
        TacticalBomber,
        NavalBomber,
        TransportPlane,
        BattleShip,
        LightCruiser,
        HeavyCruiser,
        BattleCruiser,
        Destroyer,
        Carrier,
        Submarine,
        Transport,
        FlyingBomb,
        FlyingRocket,
        Militia,
        EscortCarrier,
        NuclearSubmarine
    }

    /// <summary>
    ///     スプライトの種類 (DH1.03以降用)
    /// </summary>
    internal enum SpriteType
    {
        Infantry,
        Cavalry,
        Motorized,
        Mechanized,
        LPanzer,
        Panzer,
        Paratrooper,
        Marine,
        Bergsjaeger,
        Fighter,
        Escort,
        Interceptor,
        Bomber,
        Tactical,
        Cas,
        Naval,
        TransportPlane,
        BattleShip,
        BattleCruiser,
        HeavyCruiser,
        LightCruiser,
        Destroyer,
        Carrier,
        Submarine,
        Transport,
        Militia,
        Garrison,
        Hq,
        FlyingBomb,
        Rocket,
        NuclearSubmarine,
        EscortCarrier,
        LightCarrier,
        RocketInterceptor,
        ReserveDivision33,
        ReserveDivision34,
        ReserveDivision35,
        ReserveDivision36,
        ReserveDivision37,
        ReserveDivision38,
        ReserveDivision39,
        ReserveDivision40,
        Division01,
        Division02,
        Division03,
        Division04,
        Division05,
        Division06,
        Division07,
        Division08,
        Division09,
        Division10,
        Division11,
        Division12,
        Division13,
        Division14,
        Division15,
        Division16,
        Division17,
        Division18,
        Division19,
        Division20,
        Division21,
        Division22,
        Division23,
        Division24,
        Division25,
        Division26,
        Division27,
        Division28,
        Division29,
        Division30,
        Division31,
        Division32,
        Division33,
        Division34,
        Division35,
        Division36,
        Division37,
        Division38,
        Division39,
        Division40,
        Division41,
        Division42,
        Division43,
        Division44,
        Division45,
        Division46,
        Division47,
        Division48,
        Division49,
        Division50,
        Division51,
        Division52,
        Division53,
        Division54,
        Division55,
        Division56,
        Division57,
        Division58,
        Division59,
        Division60,
        Division61,
        Division62,
        Division63,
        Division64,
        Division65,
        Division66,
        Division67,
        Division68,
        Division69,
        Division70,
        Division71,
        Division72,
        Division73,
        Division74,
        Division75,
        Division76,
        Division77,
        Division78,
        Division79,
        Division80,
        Division81,
        Division82,
        Division83,
        Division84,
        Division85,
        Division86,
        Division87,
        Division88,
        Division89,
        Division90,
        Division91,
        Division92,
        Division93,
        Division94,
        Division95,
        Division96,
        Division97,
        Division98,
        Division99
    }

    /// <summary>
    ///     装備の種類 (DH1.03以降用)
    /// </summary>
    internal enum EquipmentType
    {
        Manpower,
        Equipment,
        Artillery,
        HeavyArtillery,
        AntiAir,
        AntiTank,
        Horses,
        Trucks,
        Halftracks,
        ArmoredCar,
        LightArmor,
        MediumArmor,
        HeavyArmor,
        TankDestroyer,
        SpArtillery,
        Fighter,
        HeavyFighter,
        RocketInterceptor,
        Bomber,
        HeavyBomber,
        TransportPlane,
        Floatplane,
        Helicopter,
        Rocket,
        Balloon,
        Transports,
        Escorts,
        Transport,
        Battleship,
        BattleCruiser,
        HeavyCruiser,
        Carrier,
        EscortCarrier,
        LightCruiser,
        Destroyer,
        Submarine,
        NuclearSubmarine
    }

    /// <summary>
    ///     ユニットクラス項目ID
    /// </summary>
    internal enum UnitClassItemId
    {
        Type, // ユニットの種類
        Branch, // ユニットの兵科
        Organization, // ユニットの編成
        Name, // 名前
        ShortName, // 短縮名
        Desc, // 説明
        ShortDesc, // 簡易説明
        Eyr, // 統計グループ
        Sprite, // スプライトの種類
        Transmute, // 生産不可能な時に使用するクラス
        GfxPrio, // 画像の優先度
        Vaule, // 軍事力
        ListPrio, // リストの優先度
        UiPrio, // UI優先度
        RealType, // 実ユニット種類
        MaxSpeedStep, // 最大生産速度
        Productable, // 初期状態で生産可能かどうか
        Cag, // 空母航空隊かどうか
        Escort, // 護衛戦闘機かどうか
        Engineer, // 工兵かどうか
        DefaultType, // 標準の生産タイプかどうか
        Detachable, // 旅団が着脱可能か
        MaxAllowedBrigades // 最大旅団数
    }

    /// <summary>
    ///     ユニットモデル項目ID
    /// </summary>
    internal enum UnitModelItemId
    {
        Name, // 名前
        Cost, // 必要IC
        BuildTime, // 生産に要する時間
        ManPower, // 必要人的資源
        MaxSpeed, // 移動速度
        SpeedCapArt, // 砲兵旅団付随時の速度キャップ
        SpeedCapEng, // 工兵旅団付随時の速度キャップ
        SpeedCapAt, // 対戦車旅団付随時の速度キャップ
        SpeedCapAa, // 対空旅団付随時の速度キャップ
        Range, // 航続距離
        DefaultOrganization, // 組織率
        Morale, // 士気
        Defensiveness, // 防御力
        SeaDefense, // 対艦/対潜防御力
        AirDefense, // 対空防御力
        SurfaceDefense, // 対地/対艦防御力
        Toughness, // 耐久力
        Softness, // 脆弱性
        Suppression, // 制圧力
        SoftAttack, // 対人攻撃力
        HardAttack, // 対甲攻撃力
        SeaAttack, // 対艦攻撃力(海軍)
        SubAttack, // 対潜攻撃力
        ConvoyAttack, // 通商破壊力
        ShoreBombardment, // 湾岸攻撃力
        AirAttack, // 対空攻撃力
        NavalAttack, // 対艦攻撃力(空軍)
        StrategicAttack, // 戦略爆撃力
        Distance, // 射程距離
        SurfaceDetectionCapability, // 対艦索敵能力
        SubDetectionCapability, // 対潜索敵能力
        AirDetectionCapability, // 対空索敵能力
        Visibility, // 可視性
        TransportWeight, // 所要TC
        TransportCapability, // 輸送能力
        SupplyConsumption, // 消費物資
        FuelConsumption, // 消費燃料
        UpgradeTimeFactor, // 改良時間補正
        UpgradeCostFactor, // 改良IC補正
        ArtilleryBombardment, // 砲撃攻撃力 (AoD)
        MaxSupplyStock, // 最大携行物資 (AoD)
        MaxOilStock, // 最大携行燃料 (AoD)
        NoFuelCombatMod, // 燃料切れ時の戦闘補正 (DH)
        ReinforceTimeFactor, // 補充時間補正 (DH)
        ReinforceCostFactor, // 補充IC補正 (DH)
        UpgradeTimeBoost, // 改良時間の補正をするか (DH)
        AutoUpgrade, // 他師団への自動改良を許可するか (DH)
        UpgradeClass, // 自動改良先のユニットクラス (DH)
        UpgradeModel, // 自動改良先モデル番号 (DH)
        SpeedCap // 速度キャップ (DH1.03以降)
    }

    /// <summary>
    ///     ユニット装備項目ID
    /// </summary>
    internal enum UnitEquipmentItemId
    {
        Resource, // 資源
        Quantity // 量
    }

    /// <summary>
    ///     ユニット更新項目ID
    /// </summary>
    internal enum UnitUpgradeItemId
    {
        Type, // ユニットの種類
        UpgradeTimeFactor, // 改良時間補正
        UpgradeCostFactor // 改良IC補正
    }
}