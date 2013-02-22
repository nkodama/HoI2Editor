using System;
using System.Collections.Generic;
using System.Globalization;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     技術グループ
    /// </summary>
    public class TechGroup
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechGroup()
        {
            Items = new List<object>();
        }

        /// <summary>
        ///     技術グループID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     技術カテゴリ
        /// </summary>
        public TechCategory Category { get; set; }

        /// <summary>
        ///     技術グループ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     技術グループ説明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     項目リスト
        /// </summary>
        public List<object> Items { get; private set; }
    }

    /// <summary>
    ///     技術ラベル
    /// </summary>
    public class TechLabel
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechLabel()
        {
            Positions = new List<TechPosition>();
        }

        /// <summary>
        ///     タグ名
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        #region 生成

        /// <summary>
        ///     技術ラベルを作成する
        /// </summary>
        /// <returns>作成した技術ラベル</returns>
        public TechLabel Create()
        {
            var label = new TechLabel {Tag = Config.GetTempKey()};

            Config.SetText(label.Tag, "", Game.TechTextFileName);

            label.Positions.Add(new TechPosition());

            return label;
        }

        /// <summary>
        ///     技術ラベルを複製する
        /// </summary>
        /// <returns>複製した技術ラベル</returns>
        public TechLabel Clone()
        {
            var label = new TechLabel {Tag = Config.GetTempKey()};

            Config.SetText(label.Tag, Config.GetText(Tag), Game.TechTextFileName);

            foreach (TechPosition position in Positions)
            {
                label.Positions.Add(position.Clone());
            }

            return label;
        }

        /// <summary>
        ///     文字列の一時キーをIDに沿った値に変更する
        /// </summary>
        /// <param name="name">ラベル名</param>
        public void RenameTempKey(string name)
        {
            // ラベル名
            if (Config.IsReservedKey(Tag, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_CAT_{0}", name);
                Config.RenameText(Tag, newKey, Game.TechTextFileName);
                Tag = newKey;
            }
        }

        /// <summary>
        ///     文字列の一時キーを削除する
        /// </summary>
        public void RemoveTempKey()
        {
            // ラベル名
            if (Config.IsReservedKey(Tag, Game.TechTextFileName))
            {
                Config.RemoveText(Tag, Game.TechTextFileName);
            }
        }

        #endregion

        /// <summary>
        ///     文字列を取得する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            string s = Config.GetText(Tag);

            if (!string.IsNullOrEmpty(s) &&
                (s[0] == '%' || s[0] == 'ｧ' || s[0] == '§') &&
                s.Length > 4 &&
                s[1] >= '0' && s[1] <= '9' &&
                s[2] >= '0' && s[2] <= '9' &&
                s[3] >= '0' && s[3] <= '9')
            {
                s = s.Substring(4);
            }

            return s ?? "";
        }
    }

    /// <summary>
    ///     技術イベント
    /// </summary>
    public class TechEvent
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechEvent()
        {
            Positions = new List<TechPosition>();
        }

        /// <summary>
        ///     技術イベントID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        /// <summary>
        ///     技術ID
        /// </summary>
        public int Technology { get; set; }

        #region 生成

        /// <summary>
        ///     技術イベントを作成する
        /// </summary>
        /// <returns>作成した技術イベント</returns>
        public TechEvent Create()
        {
            var ev = new TechEvent();
            ev.Positions.Add(new TechPosition());

            return ev;
        }

        /// <summary>
        ///     技術イベントを複製する
        /// </summary>
        /// <returns>複製した技術イベント</returns>
        public TechEvent Clone()
        {
            var ev = new TechEvent {Id = Id, Technology = Technology};
            foreach (TechPosition position in Positions)
            {
                ev.Positions.Add(position.Clone());
            }

            return ev;
        }

        #endregion

        /// <summary>
        ///     文字列を取得する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return Id.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    ///     技術
    /// </summary>
    public class Tech
    {
        #region 定数

        /// <summary>
        ///     技術カテゴリキー名テーブル
        /// </summary>
        private static readonly string[] TechCategoryKeyNames =
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

        #endregion

        /// <summary>
        ///     研究特性文字列とIDの対応付け
        /// </summary>
        public static Dictionary<string, TechSpeciality> SpecialityStringMap = new Dictionary<string, TechSpeciality>();

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Tech()
        {
            // 研究特性文字列とIDの対応付け
            foreach (TechSpeciality speciality in Enum.GetValues(typeof (TechSpeciality)))
            {
                SpecialityStringMap.Add(Techs.SpecialityStrings[(int) speciality], speciality);
            }
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Tech()
        {
            Positions = new List<TechPosition>();
            Components = new List<TechComponent>();
            Required = new List<int>();
            OrRequired = new List<int>();
            Effects = new List<Command>();
        }

        /// <summary>
        ///     技術ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     技術名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     技術短縮名
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///     技術説明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     史実年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///     小研究
        /// </summary>
        public List<TechComponent> Components { get; private set; }

        /// <summary>
        ///     必要とする技術群(AND条件)
        /// </summary>
        public List<int> Required { get; private set; }

        /// <summary>
        ///     必要とする技術群(OR条件)
        /// </summary>
        public List<int> OrRequired { get; private set; }

        /// <summary>
        ///     技術効果
        /// </summary>
        public List<Command> Effects { get; private set; }

        #region 生成

        /// <summary>
        ///     技術を作成する
        /// </summary>
        /// <returns>作成した技術</returns>
        public static Tech Create()
        {
            var tech = new Tech
                           {
                               Name = Config.GetTempKey(),
                               ShortName = Config.GetTempKey(),
                               Desc = Config.GetTempKey(),
                               Year = 1936,
                           };

            Config.SetText(tech.Name, "", Game.TechTextFileName);
            Config.SetText(tech.ShortName, "", Game.TechTextFileName);
            Config.SetText(tech.Desc, "", Game.TechTextFileName);

            tech.Positions.Add(new TechPosition());

            return tech;
        }

        /// <summary>
        ///     技術を複製する
        /// </summary>
        /// <returns>複製した技術</returns>
        public Tech Clone()
        {
            var tech = new Tech
                           {
                               Id = Id + 10,
                               Name = Config.GetTempKey(),
                               ShortName = Config.GetTempKey(),
                               Desc = Config.GetTempKey(),
                               PictureName = PictureName,
                               Year = Year,
                           };

            Config.SetText(tech.Name, Config.GetText(Name), Game.TechTextFileName);
            Config.SetText(tech.ShortName, Config.GetText(ShortName), Game.TechTextFileName);
            Config.SetText(tech.Desc, Config.GetText(Desc), Game.TechTextFileName);

            foreach (TechPosition position in Positions)
            {
                tech.Positions.Add(position.Clone());
            }

            foreach (TechComponent component in Components)
            {
                tech.Components.Add(component.Clone());
            }

            tech.Required.AddRange(Required);
            tech.OrRequired.AddRange(OrRequired);

            foreach (Command command in Effects)
            {
                tech.Effects.Add(command.Clone());
            }

            return tech;
        }

        /// <summary>
        ///     文字列の一時キーをIDに沿った値に変更する
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        public void RenameTempKey(TechCategory category)
        {
            // 技術名
            if (Config.IsReservedKey(Name, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_APP_{0}_{1}_NAME", TechCategoryKeyNames[(int) category], Id);
                Config.RenameText(Name, newKey, Game.TechTextFileName);
                Name = newKey;
            }
            // 技術短縮名
            if (Config.IsReservedKey(ShortName, Game.TechTextFileName))
            {
                string newKey = String.Format("SHORT_TECH_APP_{0}_{1}_NAME", TechCategoryKeyNames[(int) category], Id);
                Config.RenameText(ShortName, newKey, Game.TechTextFileName);
                ShortName = newKey;
            }
            // 技術説明
            if (Config.IsReservedKey(Desc, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_APP_{0}_{1}_DESC", TechCategoryKeyNames[(int) category], Id);
                Config.RenameText(Desc, newKey, Game.TechTextFileName);
                Desc = newKey;
            }
            // 小研究名
            int componentId = 1;
            foreach (TechComponent component in Components)
            {
                if (Config.IsReservedKey(component.Name, Game.TechTextFileName))
                {
                    string newKey = String.Format("TECH_CMP_{0}_{1}_{2}_NAME", TechCategoryKeyNames[(int) category], Id,
                                                  componentId);
                    Config.RenameText(component.Name, newKey, Game.TechTextFileName);
                    component.Name = newKey;
                }
                componentId++;
            }
        }

        /// <summary>
        ///     文字列の一時キーを削除する
        /// </summary>
        public void RemoveTempKey()
        {
            // 技術名
            if (Config.IsReservedKey(Name, Game.TechTextFileName))
            {
                Config.RemoveText(Name, Game.TechTextFileName);
            }
            // 技術短縮名
            if (Config.IsReservedKey(ShortName, Game.TechTextFileName))
            {
                Config.RemoveText(ShortName, Game.TechTextFileName);
            }
            // 技術説明
            if (Config.IsReservedKey(Desc, Game.TechTextFileName))
            {
                Config.RemoveText(Desc, Game.TechTextFileName);
            }

            // 小研究名
            foreach (TechComponent component in Components)
            {
                if (Config.IsReservedKey(component.Name, Game.TechTextFileName))
                {
                    Config.RemoveText(component.Name, Game.TechTextFileName);
                }
            }
        }

        #endregion

        #region 小研究リスト

        /// <summary>
        ///     小研究リストに項目を追加する
        /// </summary>
        /// <param name="component">追加対象の項目</param>
        public void AddComponent(TechComponent component)
        {
            Components.Add(component);
        }

        /// <summary>
        ///     小研究リストに項目を挿入する
        /// </summary>
        /// <param name="component">挿入対象の項目</param>
        /// <param name="index">挿入する位置</param>
        public void InsertComponent(TechComponent component, int index)
        {
            Components.Insert(index, component);
        }

        /// <summary>
        ///     小研究リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        public void MoveComponent(int src, int dest)
        {
            TechComponent component = Components[src];

            if (src > dest)
            {
                // 上へ移動する場合
                Components.Insert(dest, component);
                Components.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                Components.Insert(dest + 1, component);
                Components.RemoveAt(src);
            }
        }

        /// <summary>
        ///     小研究リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目の位置</param>
        public void RemoveComponent(int index)
        {
            Components.RemoveAt(index);
        }

        #endregion

        #region 技術効果

        /// <summary>
        ///     技術効果リストに項目を追加する
        /// </summary>
        /// <param name="command">追加対象の項目</param>
        public void AddCommand(Command command)
        {
            Effects.Add(command);
        }

        /// <summary>
        ///     技術効果リストに項目を挿入する
        /// </summary>
        /// <param name="command">挿入対象の項目</param>
        /// <param name="index">挿入する位置</param>
        public void InsertCommand(Command command, int index)
        {
            Effects.Insert(index, command);
        }

        /// <summary>
        ///     技術効果リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        public void MoveCommand(int src, int dest)
        {
            Command command = Effects[src];

            if (src > dest)
            {
                // 上へ移動する場合
                Effects.Insert(dest, command);
                Effects.RemoveAt(src + 1);
            }
            else
            {
                // 下へ移動する場合
                Effects.Insert(dest + 1, command);
                Effects.RemoveAt(src);
            }
        }

        /// <summary>
        ///     技術効果リストの項目を削除する
        /// </summary>
        /// <param name="index">削除対象の項目の位置</param>
        public void RemoveCommand(int index)
        {
            Effects.RemoveAt(index);
        }

        #endregion

        /// <summary>
        ///     文字列を取得する
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return Config.GetText(Name);
        }
    }

    /// <summary>
    ///     技術ツリー内の座標
    /// </summary>
    public class TechPosition
    {
        /// <summary>
        ///     X座標
        /// </summary>
        public int X;

        /// <summary>
        ///     Y座標
        /// </summary>
        public int Y;

        #region 生成

        /// <summary>
        ///     座標を複製する
        /// </summary>
        /// <returns>複製した座標</returns>
        public TechPosition Clone()
        {
            var position = new TechPosition {X = X, Y = Y};

            return position;
        }

        #endregion
    }

    /// <summary>
    ///     小研究
    /// </summary>
    public class TechComponent
    {
        /// <summary>
        ///     小研究ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     小研究名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     小研究特性
        /// </summary>
        public TechSpeciality Speciality { get; set; }

        /// <summary>
        ///     難易度
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        ///     倍の時間を要するかどうか
        /// </summary>
        public bool DoubleTime { get; set; }

        #region 生成

        /// <summary>
        ///     小研究を作成する
        /// </summary>
        /// <returns>作成した小研究</returns>
        public static TechComponent Create()
        {
            var component = new TechComponent
                                {
                                    Name = Config.GetTempKey(),
                                    Difficulty = 1,
                                };

            Config.SetText(component.Name, "", Game.TechTextFileName);

            return component;
        }

        /// <summary>
        ///     小研究を複製する
        /// </summary>
        /// <returns>複製した小研究</returns>
        public TechComponent Clone()
        {
            var component = new TechComponent
                                {
                                    Id = Id + 1,
                                    Name = Config.GetTempKey(),
                                    Speciality = Speciality,
                                    Difficulty = Difficulty,
                                    DoubleTime = DoubleTime,
                                };

            Config.SetText(component.Name, Config.GetText(Name), Game.TechTextFileName);

            return component;
        }

        #endregion
    }

    /// <summary>
    ///     技術カテゴリ
    /// </summary>
    public enum TechCategory
    {
        Infantry,
        Armor,
        Naval,
        Aircraft,
        Industry,
        LandDoctrines,
        SecretWeapons,
        NavalDoctrines,
        AirDoctrines,
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