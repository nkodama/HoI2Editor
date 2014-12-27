using System;
using System.Collections.Generic;
using System.Text;
using HoI2Editor.Utilities;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     イベントコマンド
    /// </summary>
    public class Command
    {
        #region 公開プロパティ

        /// <summary>
        ///     コマンド種類
        /// </summary>
        public CommandType Type { get; set; }

        /// <summary>
        ///     パラメータ - which
        /// </summary>
        public object Which { get; set; }

        /// <summary>
        ///     パラメータ - value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     パラメータ - when
        /// </summary>
        public object When { get; set; }

        /// <summary>
        ///     パラメータ - where
        /// </summary>
        public object Where { get; set; }

        /// <summary>
        ///     コマンドトリガー
        /// </summary>
        public List<Trigger> Triggers { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (CommandItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     コマンド種類文字列
        /// </summary>
        public static readonly string[] TypeStringTable =
        {
            "",
            "trigger",
            "sleepevent",
            "setflag",
            "clrflag",
            "local_setflag",
            "local_clrflag",
            "headofstate",
            "headofgovernment",
            "foreignminister",
            "armamentminister",
            "ministerofsecurity",
            "ministerofintelligence",
            "chiefofstaff",
            "chiefofarmy",
            "chiefofnavy",
            "chiefofair",
            "sleepminister",
            "alliance",
            "leave_alliance",
            "inherit",
            "country",
            "capital",
            "addcore",
            "removecore",
            "secedeprovince",
            "control",
            "province_revoltrisk",
            "regime_falls",
            "belligerence",
            "relation",
            "set_relation",
            "civil_war",
            "independence",
            "peace",
            "war",
            "make_puppet",
            "coup_nation",
            "access",
            "end_access",
            "end_non_aggression",
            "non_aggression",
            "end_trades",
            "end_guarantee",
            "guarantee",
            "end_puppet",
            "end_mastery",
            "steal_tech",
            "gain_tech",
            "research_sabotaged",
            "waketeam",
            "sleepteam",
            "deactivate",
            "scrap_model",
            "research_mod",
            "info_may_cause",
            "activate",
            "max_reactor_size",
            "industrial_modifier",
            "attrition_mod",
            "supply_dist_mod",
            "repair_mod",
            "radar_eff",
            "abomb_production",
            "double_nuke_prod",
            "resource",
            "supplies",
            "oilpool",
            "metalpool",
            "energypool",
            "rarematerialspool",
            "money",
            "manpowerpool",
            "relative_manpower",
            "province_keypoints",
            "vp",
            "allow_dig_in",
            "allow_convoy_escorts",
            "transport_pool",
            "escort_pool",
            "dissent",
            "peacetime_ic_mod",
            "construct",
            "province_manpower",
            "convoy",
            "domestic",
            "set_domestic",
            "change_policy",
            "allow_building",
            "building_prod_mod",
            "convoy_prod_mod",
            "tc_mod",
            "tc_occupied_mod",
            "free_ic",
            "free_oil",
            "free_supplies",
            "free_money",
            "free_metal",
            "free_energy",
            "free_rare_materials",
            "free_transport",
            "free_escort",
            "free_manpower",
            "add_prov_resource",
            "extra_tc",
            "set_leader_skill",
            "sleepleader",
            "wakeleader",
            "switch_allegiance",
            "delete_unit",
            "build_division",
            "add_corps",
            "add_division",
            "remove_division",
            "gas_attack",
            "gas_protection",
            "max_positioning",
            "min_positioning",
            "task_efficiency",
            "ground_def_eff",
            "trickleback_mod",
            "endgame",
            "activate_division",
            "damage_division",
            "disorg_division",
            "lock_division",
            "unlock_division",
            "revolt",
            "songs",
            "start_pattern",
            "add_to_pattern",
            "end_pattern",
            "set_ground",
            "hq_supply_eff",
            "sce_frequency",
            "counterattack",
            "assault",
            "encirclement",
            "ambush",
            "delay",
            "tactical_withdrawal",
            "breakthrough",
            "enable_task",
            "new_model",
            "activate_unit_type",
            "deactivate_unit_type",
            "nuclear_carrier",
            "missile_carrier",
            "soft_attack",
            "hard_attack",
            "defensiveness",
            "air_attack",
            "air_defense",
            "build_cost",
            "build_time",
            "manpower",
            "speed",
            "max_organization",
            "transport_weight",
            "supply_consumption",
            "fuel_consumption",
            "speed_cap_art",
            "speed_cap_eng",
            "speed_cap_at",
            "speed_cap_aa",
            "strategic_attack",
            "tactical_attack",
            "naval_attack",
            "surface_detection",
            "air_detection",
            "transport_capacity",
            "range",
            "shore_attack",
            "naval_defense",
            "visibility",
            "night_move",
            "night_attack",
            "night_defense",
            "desert_attack",
            "desert_defense",
            "mountain_attack",
            "mountain_defense",
            "hill_attack",
            "hill_defense",
            "forest_attack",
            "forest_defense",
            "swamp_attack",
            "swamp_defense",
            "urban_attack",
            "urban_defense",
            "river_attack",
            "paradrop_attack",
            "desert_move",
            "mountain_move",
            "hill_move",
            "forest_move",
            "swamp_move",
            "urban_move",
            "river_crossing",
            "frozen_attack",
            "frozen_defense",
            "snow_attack",
            "snow_defense",
            "blizzard_attack",
            "blizzard_defense",
            "rain_attack",
            "rain_defense",
            "storm_attack",
            "storm_defense",
            "muddy_attack",
            "muddy_defense",
            "frozen_move",
            "snow_move",
            "blizzard_move",
            "rain_move",
            "storm_move",
            "muddy_move",
            "minisub_bonus",
            "surprise",
            "intelligence",
            "army_detection",
            "aa_batteries",
            "industrial_multiplier",
            "convoy_def_eff",
            "morale",
            "nuke_damage",
            "max_amphib_mod",
            "sub_detection",
            "add_leader_skill",
            "add_leader_trait",
            "remove_leader_trait",
            "giveleader",
            "add_team_skill",
            "set_team_skill",
            "add_team_research_type",
            "remove_team_research_type",
            "giveteam",
            "artillery_bombardment",
            "change_idea",
            "change_partisan_activity",
            "set_partisan_activity",
            "change_unit_xp",
            "set_unit_xp",
            "change_leader_xp",
            "set_leader_xp",
            "change_retool_time",
            "set_retool_time",
            "grant_military_control",
            "end_military_control",
            "building_eff_mod",
            "jungle_attack",
            "jungle_defense",
            "jungle_move",
            "fort_attack",
            "carrier_level",
            "event",
            "wakeminister",
            "name",
            "flag_ext",
            "auto_trade",
            "auto_trade_reset",
            "trade",
            "wartime_ic_mod",
            "alliance_leader",
            "alliance_name",
            "military_control",
            "embargo",
            "secedearea",
            "secederegion",
            "addclaim",
            "removeclaim",
            "stockpile",
            "land_fort_eff",
            "coast_fort_eff",
            "strength",
            "demobilize",
            "convoy_attack",
            "sub_attack",
            "suppression",
            "softness",
            "toughness",
            "plain_attack",
            "plain_defense"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Command()
        {
            Triggers = new List<Trigger>();
        }

        /// <summary>
        ///     コマンドを複製する
        /// </summary>
        /// <returns>複製するコマンド</returns>
        public Command Clone()
        {
            var command = new Command { Type = Type, Which = Which, Value = Value, When = When, Where = Where };

            return command;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列に変換する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Game.Type == GameType.DarkestHour && Triggers != null && Triggers.Count > 0)
            {
                sb.Append("trigger = {");
                foreach (Trigger trigger in Triggers)
                {
                    sb.AppendFormat(" {0}", trigger);
                }
                sb.Append(" } ");
            }
            sb.AppendFormat("type = {0}", TypeStringTable[(int) Type]);
            if (!ObjectHelper.IsNullOrEmpty(Which))
            {
                sb.AppendFormat(" which = {0}", ObjectHelper.ToString(Which));
            }
            if (!ObjectHelper.IsNullOrEmpty(When))
            {
                sb.AppendFormat(" when = {0}", ObjectHelper.ToString(When));
            }
            if (!ObjectHelper.IsNullOrEmpty(Where))
            {
                sb.AppendFormat(" where = {0}", ObjectHelper.ToString(Where));
            }
            if (!ObjectHelper.IsNullOrEmpty(Value))
            {
                sb.AppendFormat(" value = {0}", ObjectHelper.ToString(Value));
            }
            return sb.ToString();
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     技術項目データが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(CommandItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(CommandItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
        {
            foreach (CommandItemId id in Enum.GetValues(typeof (CommandItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (CommandItemId id in Enum.GetValues(typeof (CommandItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     コマンド種類
    /// </summary>
    public enum CommandType
    {
        None,
        Trigger,
        SleepEvent,
        SetFlag,
        ClrFlag,
        LocalSetFlag,
        LocalClrFlag,
        HeadOfState,
        HeadOfGovernment,
        ForeignMinister,
        ArmamentMinister,
        MinisterOfSecurity,
        MinisterOfIntelligence,
        ChiefOfStaff,
        ChiefOfArmy,
        ChiefOfNavy,
        ChiefOfAir,
        SleepMinister,
        Alliance,
        LeaveAlliance,
        Inherit,
        Country,
        Capital,
        AddCore,
        RemoveCore,
        SecedeProvince,
        Control,
        ProvinceRevoltRisk,
        RegimeFalls,
        Belligerence,
        Relation,
        SetRelation,
        CivilWar,
        Independence,
        Peace,
        War,
        MakePuppet,
        CoupNation,
        Access,
        EndAccess,
        EndNonAggression,
        NonAggression,
        EndTrades,
        EndGuarantee,
        Guarantee,
        EndPuppet,
        EndMastery,
        StealTech,
        GainTech,
        ResearchSabotaged,
        WakeTeam,
        SleepTeam,
        Deactivate,
        ScrapModel,
        ResearchMod,
        InfoMayCause,
        Activate,
        MaxReactorSize,
        IndustrialModifier,
        AttritionMod,
        SupplyDistMod,
        RepairMod,
        RadarEff,
        AbombProduction,
        DoubleNukeProd,
        Resource,
        Supplies,
        OilPool,
        MetalPool,
        EnergyPool,
        RareMaterialsPool,
        Money,
        ManPowerPool,
        RelativeManPower,
        ProvinceKeyPoints,
        Vp,
        AllowDigIn,
        AllowConvoyEscorts,
        TransportPool,
        EscortPool,
        Dissent,
        PeaceTimeIcMod,
        Construct,
        ProvinceManPower,
        Convoy,
        Domestic,
        SetDomestic,
        ChangePolicy,
        AllowBuilding,
        BuildingProdMod,
        ConvoyProdMod,
        TcMod,
        TcOccupiedMod,
        FreeIc,
        FreeOil,
        FreeSupplies,
        FreeMoney,
        FreeMetal,
        FreeEnergy,
        FreeRareMaterials,
        FreeTransport,
        FreeEscort,
        FreeManPower,
        AddProvResource,
        ExtraTc,
        SetLeaderSkill,
        SleepLeader,
        WakeLeader,
        SwitchAllegiance,
        DeleteUnit,
        BuildDivision,
        AddCorps,
        AddDivision,
        RemoveDivision,
        GasAttack,
        GasProtection,
        MaxPositioning,
        MinPositioning,
        TaskEfficiency,
        GroundDefEff,
        TrickleBackMod,
        Endgame,
        ActivateDivision,
        DamageDivision,
        DisorgDivision,
        LockDivision,
        UnlockDivision,
        Revolt,
        Songs,
        StartPattern,
        AddToPattern,
        EndPattern,
        SetGround,
        HqSupplyEff,
        SceFrequency,
        CounterAttack,
        Assault,
        Encirclement,
        Ambush,
        Delay,
        TacticalWithdrawal,
        Breakthrough,
        EnableTask,
        NewModel,
        ActivateUnitType,
        DeactivateUnitType,
        NuclearCarrier,
        MissileCarrier,
        SoftAttack,
        HardAttack,
        Defensiveness,
        AirAttack,
        AirDefense,
        BuildCost,
        BuildTime,
        ManPower,
        Speed,
        MaxOrganization,
        TransportWeight,
        SupplyConsumption,
        FuelConsumption,
        SpeedCapArt,
        SpeedCapEng,
        SpeedCapAt,
        SpeedCapAa,
        StrategicAttack,
        TacticalAttack,
        NavalAttack,
        SurfaceDetection,
        AirDetection,
        TransportCapacity,
        Range,
        ShoreAttack,
        NavalDefense,
        Visibility,
        NightMove,
        NightAttack,
        NightDefense,
        DesertAttack,
        DesertDefense,
        MountainAttack,
        MountainDefense,
        HillAttack,
        HillDefense,
        ForestAttack,
        ForestDefense,
        SwampAttack,
        SwampDefense,
        UrbanAttack,
        UrbanDefense,
        RiverAttack,
        ParaDropAttack,
        DesertMove,
        MountainMove,
        HillMove,
        ForestMove,
        SwampMove,
        UrbanMove,
        RiverCrossing,
        FrozenAttack,
        FrozenDefense,
        SnowAttack,
        SnowDefense,
        BlizzardAttack,
        BlizzardDefense,
        RainAttack,
        RainDefense,
        StormAttack,
        StormDefense,
        MuddyAttack,
        MuddyDefense,
        FrozenMove,
        SnowMove,
        BlizzardMove,
        RainMove,
        StormMove,
        MuddyMove,
        MinisubBonus,
        Surprise,
        Intelligence,
        ArmyDetection,
        AaBatteries,
        IndustrialMultiplier,
        // DDA1.2から存在するがDataWikiに記載がないもの
        ConvoyDefEff,
        Morale,
        NukeDamage,
        MaxAmphibMod,
        SubDetection,
        // AoDで追加
        AddLeaderSkill,
        AddLeaderTrait,
        RemoveLeaderTrait,
        GiveLeader,
        AddTeamSkill,
        SetTeamSkill,
        AddTeamResearchType,
        RemoveTeamResearchType,
        GiveTeam,
        ArtilleryBombardment,
        ChangeIdea,
        ChangePartisanActivity,
        SetPartisanActivity,
        ChangeUnitXp,
        SetUnitXp,
        ChangeLeaderXp,
        SetLeaderXp,
        ChangeRetoolTime,
        SetRetoolTime,
        GrantMilitaryControl,
        EndMilitaryControl,
        // AoDで存在するがDataWikiに記載がないもの
        BuildingEffMod,
        // ICで存在するがDataWikiに記載がないもの
        JungleAttack,
        JungleDefense,
        JungleMove,
        FortAttack,
        CarrierLevel,
        // DHで追加
        Event,
        WakeMinister,
        Name,
        FlagExt,
        AutoTrade,
        AutoTradeReset,
        Trade,
        WarTimeIcMod,
        AllianceLeader,
        AllianceName,
        MilitaryControl,
        Embargo,
        SecedeArea,
        SecedeRegion,
        AddClaim,
        RemoveClaim,
        Stockpile,
        LandFortEff, // ICで存在するがDataWikiに記載がないもの
        CoastFortEff, // ICで存在するがDataWikiに記載がないもの
        Strength,
        Demobilize,
        // DHで存在するがDataWikiに記載がないもの
        ConvoyAttack,
        SubAttack,
        Suppression,
        Softness,
        Toughness,
        PlainAttack,
        PlainDefense
    }

    /// <summary>
    ///     コマンド項目ID
    /// </summary>
    public enum CommandItemId
    {
        Type, // コマンドの種類
        Which, // whichパラメータ
        Value, // valueパラメータ
        When, // whenパラメータ
        Where // whereパラメータ
    }
}