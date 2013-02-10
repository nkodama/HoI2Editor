﻿using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ユニットデータ
    /// </summary>
    public class Unit
    {
        /// <summary>
        ///     付属可能旅団
        /// </summary>
        public List<UnitType> AllowedBrigades = new List<UnitType>();

        /// <summary>
        ///     ユニットの兵科
        /// </summary>
        public UnitBranch Branch;

        /// <summary>
        ///     空母航空隊かどうか
        /// </summary>
        public bool Cag;

        /// <summary>
        ///     標準の生産タイプかどうか
        /// </summary>
        public bool DefaultType;

        /// <summary>
        ///     説明
        /// </summary>
        public string Desc;

        /// <summary>
        ///     旅団が着脱可能か
        /// </summary>
        public bool Detachable;

        /// <summary>
        ///     工兵かどうか
        /// </summary>
        public bool Engineer;

        /// <summary>
        ///     護衛戦闘機かどうか
        /// </summary>
        public bool Escort;

        /// <summary>
        ///     統計グループ
        /// </summary>
        public int Eyr;

        /// <summary>
        ///     画像の優先度
        /// </summary>
        public int GfxPrio;

        /// <summary>
        ///     リストの優先度
        /// </summary>
        public int ListPrio;

        /// <summary>
        ///     最大旅団数
        /// </summary>
        public int MaxAllowedBrigades;

        /// <summary>
        ///     モデルリスト
        /// </summary>
        public List<UnitModel> Models = new List<UnitModel>();

        /// <summary>
        ///     名前
        /// </summary>
        public string Name;

        /// <summary>
        ///     ユニットの編成
        /// </summary>
        public UnitOrganization Organization;

        /// <summary>
        ///     初期状態で生産可能かどうか
        /// </summary>
        public bool Productable;

        /// <summary>
        ///     実ユニット種類
        /// </summary>
        public RealUnitType RealType;

        /// <summary>
        ///     簡易説明
        /// </summary>
        public string ShortDesc;

        /// <summary>
        ///     短縮名
        /// </summary>
        public string ShortName;

        /// <summary>
        ///     スプライト名
        /// </summary>
        public SpriteType Sprite;

        /// <summary>
        ///     生産不可能な時に使用するクラス
        /// </summary>
        public UnitType Transmute;

        /// <summary>
        ///     ユニットの種類
        /// </summary>
        public UnitType Type;

        /// <summary>
        ///     ユニット更新情報
        /// </summary>
        public List<UnitUpgrade> Upgrades = new List<UnitUpgrade>();

        /// <summary>
        ///     軍事力
        /// </summary>
        public double Value;

        /// <summary>
        ///     ユニットモデルを挿入する
        /// </summary>
        /// <param name="model"></param>
        /// <param name="index"></param>
        public void InsertModel(UnitModel model, int index)
        {
            Models.Insert(index, model);
        }

        /// <summary>
        ///     ユニットモデルを削除する
        /// </summary>
        /// <param name="index"></param>
        public void RemoveModel(int index)
        {
            Models.RemoveAt(index);
        }

        /// <summary>
        ///     ユニットモデルを移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        public void MoveModel(int src, int dest)
        {
            UnitModel model = Models[src];

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
        }
    }

    /// <summary>
    ///     ユニットモデルデータ
    /// </summary>
    public class UnitModel
    {
        /// <summary>
        ///     対空攻撃力
        /// </summary>
        public double AirAttack;

        /// <summary>
        ///     対空防御力
        /// </summary>
        public double AirDefense;

        /// <summary>
        ///     対空索敵能力
        /// </summary>
        public double AirDetectionCapability;

        /// <summary>
        ///     砲撃攻撃力 (AoD)
        /// </summary>
        public double ArtilleryBombardment;

        /// <summary>
        ///     他師団への自動改良を許可するか
        /// </summary>
        public bool AutoUpgrade;

        /// <summary>
        ///     生産に要する時間
        /// </summary>
        public double BuildTime;

        /// <summary>
        ///     通商破壊力
        /// </summary>
        public double ConvoyAttack;

        /// <summary>
        ///     必要IC
        /// </summary>
        public double Cost;

        /// <summary>
        ///     組織率
        /// </summary>
        public double DefaultOrganization;

        /// <summary>
        ///     防御力
        /// </summary>
        public double Defensiveness;

        /// <summary>
        ///     射程距離
        /// </summary>
        public double Distance;

        /// <summary>
        ///     装備 (DH1.03以降)
        /// </summary>
        public List<UnitEquipment> Equipments = new List<UnitEquipment>();

        /// <summary>
        ///     消費燃料
        /// </summary>
        public double FuelConsumption;

        /// <summary>
        ///     対甲攻撃力
        /// </summary>
        public double HardAttack;

        /// <summary>
        ///     必要人的資源
        /// </summary>
        public double ManPower;

        /// <summary>
        ///     最大携行燃料 (AoD)
        /// </summary>
        public double MaxOilStock;

        /// <summary>
        ///     移動速度
        /// </summary>
        public double MaxSpeed;

        /// <summary>
        ///     最大携行物資 (AoD)
        /// </summary>
        public double MaxSupplyStock;

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale;

        /// <summary>
        ///     対艦攻撃力(空軍)
        /// </summary>
        public double NavalAttack;

        /// <summary>
        ///     燃料切れ時の戦闘補正 (DH)
        /// </summary>
        public double NoFuelCombatMod;

        /// <summary>
        ///     航続距離
        /// </summary>
        public double Range;

        /// <summary>
        ///     補充IC補正 (DH)
        /// </summary>
        public double ReinforceCostFactor;

        /// <summary>
        ///     補充時間補正 (DH)
        /// </summary>
        public double ReinforceTimeFactor;

        /// <summary>
        ///     対艦攻撃力(海軍)
        /// </summary>
        public double SeaAttack;

        /// <summary>
        ///     対艦/対潜防御力
        /// </summary>
        public double SeaDefense;

        /// <summary>
        ///     湾岸攻撃力
        /// </summary>
        public double ShoreBombardment;

        /// <summary>
        ///     対人攻撃力
        /// </summary>
        public double SoftAttack;

        /// <summary>
        ///     脆弱性
        /// </summary>
        public double Softness;

        /// <summary>
        ///     速度キャップ (DH1.03以降)
        /// </summary>
        public double SpeedCap;

        /// <summary>
        ///     対空旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapAa;

        /// <summary>
        ///     砲兵旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapArt;

        /// <summary>
        ///     対戦車旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapAt;

        /// <summary>
        ///     工兵旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapEng;

        /// <summary>
        ///     戦略爆撃力
        /// </summary>
        public double StrategicAttack;

        /// <summary>
        ///     対潜攻撃力
        /// </summary>
        public double SubAttack;

        /// <summary>
        ///     対潜索敵能力
        /// </summary>
        public double SubDetectionCapability;

        /// <summary>
        ///     消費物資
        /// </summary>
        public double SupplyConsumption;

        /// <summary>
        ///     制圧力
        /// </summary>
        public double Suppression;

        /// <summary>
        ///     対地/対艦防御力
        /// </summary>
        public double SurfaceDefense;

        /// <summary>
        ///     対艦索敵能力
        /// </summary>
        public double SurfaceDetectionCapability;

        /// <summary>
        ///     耐久力
        /// </summary>
        public double Toughness;

        /// <summary>
        ///     輸送能力
        /// </summary>
        public double TransportCapability;

        /// <summary>
        ///     所要TC
        /// </summary>
        public double TransportWeight;

        /// <summary>
        ///     自動改良先のユニットクラス
        /// </summary>
        public UnitType UpgradeClass;

        /// <summary>
        ///     改良IC補正
        /// </summary>
        public double UpgradeCostFactor;

        /// <summary>
        ///     自動改良先モデル番号
        /// </summary>
        public int UpgradeModel;

        /// <summary>
        ///     改良時間の補正をするか
        /// </summary>
        public bool UpgradeTimeBoost;

        /// <summary>
        ///     改良時間補正
        /// </summary>
        public double UpgradeTimeFactor;

        /// <summary>
        ///     可視性
        /// </summary>
        public double Visibility;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public UnitModel()
        {
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元のユニットモデル</param>
        public UnitModel(UnitModel original)
        {
            DefaultOrganization = original.DefaultOrganization;
            Morale = original.Morale;
            Range = original.Range;
            TransportWeight = original.TransportWeight;
            TransportCapability = original.TransportCapability;
            Suppression = original.Suppression;
            SupplyConsumption = original.SupplyConsumption;
            FuelConsumption = original.FuelConsumption;
            MaxSupplyStock = original.MaxSupplyStock;
            MaxOilStock = original.MaxOilStock;
            Cost = original.Cost;
            BuildTime = original.BuildTime;
            ManPower = original.ManPower;
            UpgradeCostFactor = original.UpgradeCostFactor;
            UpgradeTimeFactor = original.UpgradeTimeFactor;
            ReinforceCostFactor = original.ReinforceCostFactor;
            ReinforceTimeFactor = original.ReinforceTimeFactor;
            MaxSpeed = original.MaxSpeed;
            SpeedCap = original.SpeedCap;
            SpeedCapArt = original.SpeedCapArt;
            SpeedCapEng = original.SpeedCapEng;
            SpeedCapAt = original.SpeedCapAt;
            SpeedCapAa = original.SpeedCapAa;
            Defensiveness = original.Defensiveness;
            SeaDefense = original.SeaDefense;
            AirDefense = original.AirDefense;
            SurfaceDefense = original.SurfaceDefense;
            Toughness = original.Toughness;
            Softness = original.Softness;
            SoftAttack = original.SoftAttack;
            HardAttack = original.HardAttack;
            SeaAttack = original.SeaAttack;
            SubAttack = original.SubAttack;
            ConvoyAttack = original.ConvoyAttack;
            ShoreBombardment = original.ShoreBombardment;
            AirAttack = original.AirAttack;
            NavalAttack = original.NavalAttack;
            StrategicAttack = original.StrategicAttack;
            ArtilleryBombardment = original.ArtilleryBombardment;
            Distance = original.Distance;
            Visibility = original.Visibility;
            SurfaceDetectionCapability = original.SurfaceDetectionCapability;
            SubDetectionCapability = original.SubDetectionCapability;
            AirDetectionCapability = original.AirDetectionCapability;
            NoFuelCombatMod = original.NoFuelCombatMod;
            Equipments.AddRange(original.Equipments);
        }

        /// <summary>
        ///     ユニットモデル名を取得する
        /// </summary>
        /// <param name="unit">ユニットクラス</param>
        /// <param name="no">ユニットモデル番号</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル名</returns>
        public static string GetName(Unit unit, int no, CountryTag country)
        {
            string name;
            int unitNo = Units.UnitNumbers[(int) unit.Type];
            if (country != CountryTag.None)
            {
                string countryText = Country.CountryTextTable[(int) country];
                name = string.Format(
                    unit.Organization == UnitOrganization.Division ? "MODEL_{0}_{1}_{2}" : "BRIG_MODEL_{0}_{1}_{2}",
                    countryText, unitNo, no);
                if (Config.ExistsKey(name))
                {
                    return name;
                }
            }
            name = string.Format(
                unit.Organization == UnitOrganization.Division ? "MODEL_{0}_{1}" : "BRIG_MODEL_{0}_{1}", unitNo, no);
            return name;
        }

        /// <summary>
        ///     装備の項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        public void MoveEquipment(int src, int dest)
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
    }

    /// <summary>
    ///     ユニット装備情報
    /// </summary>
    public class UnitEquipment
    {
        /// <summary>
        ///     量
        /// </summary>
        public double Quantity;

        /// <summary>
        ///     資源
        /// </summary>
        public string Resource;
    }

    /// <summary>
    ///     ユニット更新情報
    /// </summary>
    public class UnitUpgrade
    {
        /// <summary>
        ///     ユニットの種類
        /// </summary>
        public UnitType Type;

        /// <summary>
        ///     改良IC補正
        /// </summary>
        public double UpgradeCostFactor;

        /// <summary>
        ///     改良時間補正
        /// </summary>
        public double UpgradeTimeFactor;
    }

    /// <summary>
    ///     ユニットの兵科
    /// </summary>
    public enum UnitBranch
    {
        Army, // 陸軍
        Navy, // 海軍
        AirForce, // 空軍
    }

    /// <summary>
    ///     ユニットの編成
    /// </summary>
    public enum UnitOrganization
    {
        Division, // 師団
        Brigade, // 旅団
    }

    /// <summary>
    ///     ユニットの種類
    /// </summary>
    public enum UnitType
    {
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
        Brigade99,
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
    public enum RealUnitType
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
        NuclearSubmarine,
    }

    /// <summary>
    ///     スプライトの種類
    /// </summary>
    public enum SpriteType
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
        Division99,
    }
}