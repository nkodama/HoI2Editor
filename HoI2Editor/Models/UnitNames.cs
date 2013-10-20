using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoI2Editor.Models
{
    /// <summary>
    /// 新規ユニット名を保持するクラス
    /// </summary>
    public static class UnitNames
    {
        #region 公開プロパティ

        /// <summary>
        /// ユニット名
        /// </summary>
        public static List<List<string>> Names = new List<List<string>>();

        #endregion

        #region 公開定数

        /// <summary>
        /// ユニット種類名
        /// </summary>
        public static readonly string[] UnitTypeNames =
            {
                "HQ",
                "Inf",
                "Gar",
                "Cav",
                "Mot",
                "Mec",
                "L ARM",
                "Arm",
                "Par",
                "Mar",
                "Mtn",
                "Mil",
                "Fig",
                "Int F",
                "Esc F",
                "Str",
                "Tac",
                "CAS",
                "Nav",
                "Trp",
                "V1",
                "V2",
                "BB",
                "BC",
                "CV",
                "27",
                "31",
                "CA",
                "CL",
                "DD",
                "SS",
                "NS",
                "TP"
            };

        #endregion
    }

    /// <summary>
    /// ユニット種類
    /// </summary>
    public enum UnitNameType
    {
        Hq, // 司令部
        Infantry, // 歩兵
        Garrison, // 守備師団
        Cavalry, // 騎兵
        Motorized, // 自動車化歩兵
        Mechanized, // 機械化歩兵
        LightArmor, // 軽戦車
        Armor, // 戦車
        Paratrooper, // 空挺兵
        Marine, // 海兵
        Bergsjaeger, // 山岳兵
        Militia, // 民兵
        Fighter, // 戦闘機
        Interceptor, // 迎撃機
        EscortFighter, // 護衛戦闘機
        StrategicBomber, // 戦略爆撃機
        TacticalBomber, // 戦術爆撃機
        Cas, // 近接航空支援機
        NavalBomber, // 海軍爆撃機
        TransportPlane, // 輸送機
        FlyingBomb, // 飛行爆弾
        FlyingRocket, // 戦略ロケット
        Battleship, // 戦艦
        BattleCruiser, // 巡洋戦艦
        Carrier, // 空母
        LightCarrier, // 軽空母
        EscortCarrier, // 護衛空母
        HeavyCruiser, // 重巡洋艦
        LightCruiser, // 軽巡洋艦
        Destroyer, // 駆逐艦
        Submarine, // 潜水艦
        NuclearSubmarine, // 原子力潜水艦
        Transport, // 輸送艦
    }
}
