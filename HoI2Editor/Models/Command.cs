using System;
using System.Collections.Generic;
using System.Text;
using HoI2Editor.Utilities;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     イベントコマンド
    /// </summary>
    internal class Command
    {
        #region 公開プロパティ

        /// <summary>
        ///     コマンド種類
        /// </summary>
        internal CommandType Type { get; set; }

        /// <summary>
        ///     パラメータ - which
        /// </summary>
        internal object Which { get; set; }

        /// <summary>
        ///     パラメータ - value
        /// </summary>
        internal object Value { get; set; }

        /// <summary>
        ///     パラメータ - when
        /// </summary>
        internal object When { get; set; }

        /// <summary>
        ///     パラメータ - where
        /// </summary>
        internal object Where { get; set; }

        /// <summary>
        ///     コマンドトリガー
        /// </summary>
        internal List<Trigger> Triggers { get; } = new List<Trigger>();

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof(CommandItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal Command()
        {
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元のイベントコマンド</param>
        internal Command(Command original)
        {
            Type = original.Type;

            // 参照型のオブジェクトを格納する場合はここをdeep copyに変える
            Which = original.Which;
            Value = original.Value;
            When = original.When;
            Where = original.Where;

            foreach (Trigger trigger in original.Triggers)
            {
                Triggers.Add(new Trigger(trigger));
            }
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列に変換する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Game.Type == GameType.DarkestHour && Triggers != null && Triggers.Count > 0)
            {
                sb.Append("trigger = {");
                foreach (Trigger trigger in Triggers)
                {
                    sb.AppendFormat(" {0}", trigger);
                }
                sb.Append(" } ");
            }
            sb.AppendFormat("type = {0}", Commands.Strings[(int) Type]);
            if (Which != null)
            {
                sb.AppendFormat(" which = {0}", ObjectHelper.ToString(Which));
            }
            if (When != null)
            {
                sb.AppendFormat(" when = {0}", ObjectHelper.ToString(When));
            }
            if (Where != null)
            {
                sb.AppendFormat(" where = {0}", ObjectHelper.ToString(Where));
            }
            if (Value != null)
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
        internal bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal bool IsDirty(CommandItemId id)
        {
            return _dirtyFlags[(int) id];
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
        internal void SetDirty(CommandItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        internal void SetDirtyAll()
        {
            foreach (CommandItemId id in Enum.GetValues(typeof(CommandItemId)))
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
            foreach (CommandItemId id in Enum.GetValues(typeof(CommandItemId)))
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
    internal enum CommandType
    {
        None,
        Endgame,
        SetFlag,
        ClrFlag,
        LocalSetFlag,
        LocalClrFlag,
        RegimeFalls,
        Inherit,
        Country,
        AddCore,
        RemoveCore,
        SecedeProvince,
        Control,
        Capital,
        CivilWar,
        ChangeIdea,
        Belligerence,
        Dissent,
        ProvinceRevoltRisk,
        Domestic,
        SetDomestic,
        ChangePolicy,
        ChangePartisanActivity,
        SetPartisanActivity,
        ChangeUnitXp,
        SetUnitXp,
        ChangeLeaderXp,
        SetLeaderXp,
        ChangeRetoolTime,
        SetRetoolTime,
        Independence,
        Alliance,
        LeaveAlliance,
        Relation,
        SetRelation,
        Peace,
        PeaceWithAll,
        War,
        EndPuppet,
        EndMastery,
        MakePuppet,
        CoupNation,
        Access,
        EndAccess,
        AccessToAlliance,
        EndNonAggression,
        NonAggression,
        EndTrades,
        EndGuarantee,
        Guarantee,
        GrantMilitaryControl,
        EndMilitaryControl,
        AddTeamSkill,
        SetTeamSkill,
        AddTeamResearchType,
        RemoveTeamResearchType,
        SleepTeam,
        WakeTeam,
        GiveTeam,
        SleepMinister,
        SleepLeader,
        WakeLeader,
        GiveLeader,
        SetLeaderSkill,
        AddLeaderSkill,
        AddLeaderTrait,
        RemoveLeaderTrait,
        AllowDigIn,
        BuildDivision,
        AddCorps,
        ActivateDivision,
        AddDivision,
        RemoveDivision,
        DamageDivision,
        DisorgDivision,
        DeleteUnit,
        SwitchAllegiance,
        ScrapModel,
        LockDivision,
        UnlockDivision,
        NewModel,
        ActivateUnitType,
        DeactivateUnitType,
        CarrierLevel,
        ResearchSabotaged,
        Deactivate,
        Activate,
        InfoMayCause,
        StealTech,
        GainTech,
        Resource,
        Supplies,
        OilPool,
        MetalPool,
        EnergyPool,
        RareMaterialsPool,
        Money,
        ManPowerPool,
        RelativeManPower,
        ProvinceManPower,
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
        AllowBuilding,
        Construct,
        AllowConvoyEscorts,
        TransportPool,
        EscortPool,
        Convoy,
        PeaceTimeIcMod,
        TcMod,
        TcOccupiedMod,
        AttritionMod,
        SupplyDistMod,
        RepairMod,
        ResearchMod,
        BuildingProdMod,
        ConvoyProdMod,
        RadarEff,
        EnableTask,
        TaskEfficiency,
        MaxPositioning,
        MinPositioning,
        ProvinceKeyPoints,
        Ai,
        ExtraTc,
        Vp,
        Songs,
        Trigger,
        SleepEvent,
        MaxReactorSize,
        AbombProduction,
        DoubleNukeProd,
        NukeDamage,
        GasAttack,
        GasProtection,
        Revolt,
        AiPrepareWar,
        StartPattern,
        AddToPattern,
        EndPattern,
        SetGround,
        CounterAttack,
        Assault,
        Encirclement,
        Ambush,
        Delay,
        TacticalWithdrawal,
        Breakthrough,
        HqSupplyEff,
        SceFrequency,
        NuclearCarrier,
        MissileCarrier,
        OutOfFuelSpeed,
        NoFuelCombatMod,
        NoSuppliesCombatMod,
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
        Morale,
        TransportWeight,
        SupplyConsumption,
        FuelConsumption,
        SpeedCapArt,
        SpeedCapEng,
        SpeedCapAt,
        SpeedCapAa,
        ArtilleryBombardment,
        Suppression,
        Softness,
        Toughness,
        StrategicAttack,
        TacticalAttack,
        NavalAttack,
        SurfaceDetection,
        AirDetection,
        SubDetection,
        TransportCapacity,
        Range,
        ShoreAttack,
        NavalDefense,
        Visibility,
        SubAttack,
        ConvoyAttack,
        PlainAttack,
        PlainDefense,
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
        JungleAttack,
        JungleDefense,
        UrbanAttack,
        UrbanDefense,
        RiverAttack,
        ParaDropAttack,
        FortAttack,
        PlainMove,
        DesertMove,
        MountainMove,
        HillMove,
        ForestMove,
        SwampMove,
        JungleMove,
        UrbanMove,
        RiverCrossing,
        ClearAttack,
        ClearDefense,
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
        NightMove,
        NightAttack,
        NightDefense,
        MinisubBonus,
        Surprise,
        Intelligence,
        ArmyDetection,
        AaBatteries,
        IndustrialMultiplier,
        IndustrialModifier,
        TrickleBackMod,
        MaxAmphibMod,
        BuildingEffMod,
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
        AllianceLeader,
        AllianceName,
        Stockpile,
        AutoTrade,
        AutoTradeReset,
        MilitaryControl,
        FlagExt,
        Embargo,
        Name,
        SecedeArea,
        SecedeRegion,
        Trade,
        WarTimeIcMod,
        AddClaim,
        RemoveClaim,
        LandFortEff,
        CoastFortEff,
        ConvoyDefEff,
        GroundDefEff,
        Strength,
        Event,
        WakeMinister,
        Demobilize,
        StrengthCap,
        RemoveUnits
    }

    /// <summary>
    ///     コマンド項目ID
    /// </summary>
    internal enum CommandItemId
    {
        Type, // コマンドの種類
        Which, // whichパラメータ
        Value, // valueパラメータ
        When, // whenパラメータ
        Where // whereパラメータ
    }
}