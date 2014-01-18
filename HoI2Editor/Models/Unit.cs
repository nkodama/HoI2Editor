using System;
using System.Collections.Generic;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ユニットクラス
    /// </summary>
    public class Unit
    {
        #region 公開プロパティ

        /// <summary>
        ///     ユニットの種類
        /// </summary>
        public UnitType Type { get; set; }

        /// <summary>
        ///     ユニットの兵科
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        ///     ユニットの編成
        /// </summary>
        public UnitOrganization Organization { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     短縮名
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///     説明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     簡易説明
        /// </summary>
        public string ShortDesc { get; set; }

        /// <summary>
        ///     統計グループ
        /// </summary>
        public int Eyr { get; set; }

        /// <summary>
        ///     スプライトの種類
        /// </summary>
        public SpriteType Sprite { get; set; }

        /// <summary>
        ///     生産不可能な時に使用するクラス
        /// </summary>
        public UnitType Transmute { get; set; }

        /// <summary>
        ///     画像の優先度
        /// </summary>
        public int GfxPrio { get; set; }

        /// <summary>
        ///     軍事力
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        ///     リストの優先度
        /// </summary>
        public int ListPrio { get; set; }

        /// <summary>
        ///     UI優先度
        /// </summary>
        public int UiPrio { get; set; }

        /// <summary>
        ///     実ユニット種類
        /// </summary>
        public RealUnitType RealType { get; set; }

        /// <summary>
        ///     最大生産速度
        /// </summary>
        public int MaxSpeedStep { get; set; }

        /// <summary>
        ///     初期状態で生産可能かどうか
        /// </summary>
        public bool Productable { get; set; }

        /// <summary>
        ///     空母航空隊かどうか
        /// </summary>
        public bool Cag { get; set; }

        /// <summary>
        ///     護衛戦闘機かどうか
        /// </summary>
        public bool Escort { get; set; }

        /// <summary>
        ///     工兵かどうか
        /// </summary>
        public bool Engineer { get; set; }

        /// <summary>
        ///     標準の生産タイプかどうか
        /// </summary>
        public bool DefaultType { get; set; }

        /// <summary>
        ///     旅団が着脱可能か
        /// </summary>
        public bool Detachable { get; set; }

        /// <summary>
        ///     最大旅団数
        /// </summary>
        public int MaxAllowedBrigades { get; set; }

        /// <summary>
        ///     付属可能旅団
        /// </summary>
        public List<UnitType> AllowedBrigades { get; private set; }

        /// <summary>
        ///     モデルリスト
        /// </summary>
        public List<UnitModel> Models { get; private set; }

        /// <summary>
        ///     ユニット更新情報
        /// </summary>
        public List<UnitUpgrade> Upgrades { get; private set; }

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
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Unit()
        {
            AllowedBrigades = new List<UnitType>();
            Models = new List<UnitModel>();
            Upgrades = new List<UnitUpgrade>();
        }

        #endregion

        #region ユニットモデルリスト

        /// <summary>
        ///     ユニットモデルを挿入する
        /// </summary>
        /// <param name="model">挿入対象のユニットモデル</param>
        /// <param name="index">挿入する位置</param>
        /// <param name="name">ユニットモデル名</param>
        public void InsertModel(UnitModel model, int index, string name)
        {
            // ユニットモデルリストに項目を挿入する
            Models.Insert(index, model);

            // TODO: 国別ユニットモデル名の対応
            // TODO: 外部関数に逃す
            // TODO: 国別のモデル名と共通のモデル名を取得する関数を分ける
            // 挿入位置以降のユニットモデル名を変更する
            for (int i = Models.Count - 1; i > index; i--)
            {
                CopyModelName(i, i - 1);
            }

            // 挿入位置のユニットモデル名を変更する
            SetModelName(index, name);

            // 編集済みフラグを設定する
            model.SetDirtyAll();
            SetDirty();
        }

        /// <summary>
        ///     ユニットモデルを削除する
        /// </summary>
        /// <param name="index">削除する位置</param>
        public void RemoveModel(int index)
        {
            // ユニットモデルリストから項目を削除する
            Models.RemoveAt(index);

            // TODO: 国別ユニットモデル名の対応
            // 削除位置以降のユニットモデル名を変更する
            if (index < Models.Count)
            {
                for (int i = index; i < Models.Count; i++)
                {
                    CopyModelName(i, i + 1);
                }
            }

            // 末尾のユニットモデル名を削除する
            RemoveModelName(Models.Count);

            // 編集済みフラグを設定する
            SetDirty();
        }

        /// <summary>
        ///     ユニットモデルを移動する
        /// </summary>
        /// <param name="src">移動元の位置</param>
        /// <param name="dest">移動先の位置</param>
        public void MoveModel(int src, int dest)
        {
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

            // TODO: 国別ユニットモデル名の対応
            // TODO: 外部関数に逃がす
            // 移動元と移動先の間のユニットモデル名を変更する
            string name = GetModelName(src);
            if (src > dest)
            {
                // 上へ移動する場合
                for (int i = src; i > dest; i--)
                {
                    CopyModelName(i, i - 1);
                }
            }
            else
            {
                // 下へ移動する場合
                for (int i = src; i < dest; i++)
                {
                    CopyModelName(i, i + 1);
                }
            }

            // 移動先のユニットモデル名を変更する
            SetModelName(dest, name);

            // 編集済みフラグを設定する
            SetDirty();
        }

        #endregion

        #region ユニットモデル名

        /// <summary>
        ///     共通のユニットモデル名を取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデル名</returns>
        public string GetModelName(int index)
        {
            return Config.GetText(GetModelNameKey(index));
        }

        /// <summary>
        ///     国別のユニットモデル名を取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <returns>ユニットモデル名</returns>
        public string GetModelName(int index, Country country)
        {
            if (country == Country.None)
            {
                return GetModelName(index);
            }
            string key = GetModelNameKey(index, country);
            if (!Config.ExistsKey(key))
            {
                return GetModelName(index);
            }
            return Config.GetText(key);
        }

        /// <summary>
        ///     共通のユニットモデル名を設定する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="s">ユニットモデル名</param>
        public void SetModelName(int index, string s)
        {
            Config.SetText(GetModelNameKey(index), s, Game.UnitTextFileName);
        }

        /// <summary>
        ///     国別のユニットモデル名を設定する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        /// <param name="s">ユニットモデル名</param>
        public void SetModelName(int index, Country country, string s)
        {
            if (country == Country.None)
            {
                SetModelName(index, s);
                return;
            }

            Config.SetText(GetModelNameKey(index, country), s, Game.ModelTextFileName);
        }

        /// <summary>
        ///     共通のユニットモデル名をコピーする
        /// </summary>
        /// <param name="src">コピー元ユニットモデルのインデックス</param>
        /// <param name="dest">コピー元ユニットモデルのインデックス</param>
        public void CopyModelName(int src, int dest)
        {
            SetModelName(src, GetModelName(dest));
        }

        /// <summary>
        ///     国別のユニットモデル名をコピーする
        /// </summary>
        /// <param name="src">コピー元ユニットモデルのインデックス</param>
        /// <param name="dest">コピー元ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        public void CopyModelName(int src, int dest, Country country)
        {
            if (country == Country.None)
            {
                SetModelName(src, GetModelName(dest));
                return;
            }

            SetModelName(src, country, GetModelName(dest, country));
        }

        /// <summary>
        ///     共通のユニットモデル名を削除する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        public void RemoveModelName(int index)
        {
            Config.RemoveText(GetModelNameKey(index), Game.UnitTextFileName);
        }

        /// <summary>
        ///     国別のユニットモデル名を削除する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <param name="country">国タグ</param>
        public void RemoveModelName(int index, Country country)
        {
            if (country == Country.None)
            {
                RemoveModelName(index);
                return;
            }

            Config.RemoveText(GetModelNameKey(index, country), Game.ModelTextFileName);
        }

        /// <summary>
        ///     共通のユニットモデル名のキーを取得する
        /// </summary>
        /// <param name="index">ユニットモデルのインデックス</param>
        /// <returns>ユニットモデル名のキー</returns>
        private string GetModelNameKey(int index)
        {
            string format = (Organization == UnitOrganization.Division) ? "MODEL_{0}_{1}" : "BRIG_MODEL_{0}_{1}";
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
            string format = (Organization == UnitOrganization.Division) ? "MODEL_{0}_{1}_{2}" : "BRIG_MODEL_{0}_{1}_{2}";
            return string.Format(format, Countries.Strings[(int) country], Units.UnitNumbers[(int) Type], index);
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニットクラスが編集済みかどうかを取得する
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
        public bool IsDirty(UnitClassItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(UnitClassItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
            Units.SetDirty();
        }

        /// <summary>
        ///     付属可能旅団が編集済みかどうかを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsDirtyAllowedBrigades(UnitType type)
        {
            return _dirtyBrigades.Contains(type);
        }

        /// <summary>
        ///     付属可能旅団の編集済みフラグを設定する
        /// </summary>
        /// <param name="type">旅団の種類</param>
        public void SetDirtyAllowedBrigades(UnitType type)
        {
            if (!_dirtyBrigades.Contains(type))
            {
                _dirtyBrigades.Add(type);
            }
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
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
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     ユニットモデル
    /// </summary>
    public class UnitModel
    {
        #region 公開プロパティ

        /// <summary>
        ///     必要IC
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        ///     生産に要する時間
        /// </summary>
        public double BuildTime { get; set; }

        /// <summary>
        ///     必要人的資源
        /// </summary>
        public double ManPower { get; set; }

        /// <summary>
        ///     移動速度
        /// </summary>
        public double MaxSpeed { get; set; }

        /// <summary>
        ///     砲兵旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapArt { get; set; }

        /// <summary>
        ///     工兵旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapEng { get; set; }

        /// <summary>
        ///     対戦車旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapAt { get; set; }

        /// <summary>
        ///     対空旅団付随時の速度キャップ
        /// </summary>
        public double SpeedCapAa { get; set; }

        /// <summary>
        ///     航続距離
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        ///     組織率
        /// </summary>
        public double DefaultOrganization { get; set; }

        /// <summary>
        ///     士気
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        ///     防御力
        /// </summary>
        public double Defensiveness { get; set; }

        /// <summary>
        ///     対艦/対潜防御力
        /// </summary>
        public double SeaDefense { get; set; }

        /// <summary>
        ///     対空防御力
        /// </summary>
        public double AirDefense { get; set; }

        /// <summary>
        ///     対地/対艦防御力
        /// </summary>
        public double SurfaceDefense { get; set; }

        /// <summary>
        ///     耐久力
        /// </summary>
        public double Toughness { get; set; }

        /// <summary>
        ///     脆弱性
        /// </summary>
        public double Softness { get; set; }

        /// <summary>
        ///     制圧力
        /// </summary>
        public double Suppression { get; set; }

        /// <summary>
        ///     対人攻撃力
        /// </summary>
        public double SoftAttack { get; set; }

        /// <summary>
        ///     対甲攻撃力
        /// </summary>
        public double HardAttack { get; set; }

        /// <summary>
        ///     対艦攻撃力(海軍)
        /// </summary>
        public double SeaAttack { get; set; }

        /// <summary>
        ///     対潜攻撃力
        /// </summary>
        public double SubAttack { get; set; }

        /// <summary>
        ///     通商破壊力
        /// </summary>
        public double ConvoyAttack { get; set; }

        /// <summary>
        ///     湾岸攻撃力
        /// </summary>
        public double ShoreBombardment { get; set; }

        /// <summary>
        ///     対空攻撃力
        /// </summary>
        public double AirAttack { get; set; }

        /// <summary>
        ///     対艦攻撃力(空軍)
        /// </summary>
        public double NavalAttack { get; set; }

        /// <summary>
        ///     戦略爆撃力
        /// </summary>
        public double StrategicAttack { get; set; }

        /// <summary>
        ///     射程距離
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        ///     対艦索敵能力
        /// </summary>
        public double SurfaceDetectionCapability { get; set; }

        /// <summary>
        ///     対潜索敵能力
        /// </summary>
        public double SubDetectionCapability { get; set; }

        /// <summary>
        ///     対空索敵能力
        /// </summary>
        public double AirDetectionCapability { get; set; }

        /// <summary>
        ///     可視性
        /// </summary>
        public double Visibility { get; set; }

        /// <summary>
        ///     所要TC
        /// </summary>
        public double TransportWeight { get; set; }

        /// <summary>
        ///     輸送能力
        /// </summary>
        public double TransportCapability { get; set; }

        /// <summary>
        ///     消費物資
        /// </summary>
        public double SupplyConsumption { get; set; }

        /// <summary>
        ///     消費燃料
        /// </summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        ///     改良時間補正
        /// </summary>
        public double UpgradeTimeFactor { get; set; }

        /// <summary>
        ///     改良IC補正
        /// </summary>
        public double UpgradeCostFactor { get; set; }

        /// <summary>
        ///     砲撃攻撃力 (AoD)
        /// </summary>
        public double ArtilleryBombardment { get; set; }

        /// <summary>
        ///     最大携行物資 (AoD)
        /// </summary>
        public double MaxSupplyStock { get; set; }

        /// <summary>
        ///     最大携行燃料 (AoD)
        /// </summary>
        public double MaxOilStock { get; set; }

        /// <summary>
        ///     燃料切れ時の戦闘補正 (DH)
        /// </summary>
        public double NoFuelCombatMod { get; set; }

        /// <summary>
        ///     補充時間補正 (DH)
        /// </summary>
        public double ReinforceTimeFactor { get; set; }

        /// <summary>
        ///     補充IC補正 (DH)
        /// </summary>
        public double ReinforceCostFactor { get; set; }

        /// <summary>
        ///     改良時間の補正をするか (DH)
        /// </summary>
        public bool UpgradeTimeBoost { get; set; }

        /// <summary>
        ///     他師団への自動改良を許可するか (DH)
        /// </summary>
        public bool AutoUpgrade { get; set; }

        /// <summary>
        ///     自動改良先のユニットクラス (DH)
        /// </summary>
        public UnitType UpgradeClass { get; set; }

        /// <summary>
        ///     自動改良先モデル番号 (DH)
        /// </summary>
        public int UpgradeModel { get; set; }

        /// <summary>
        ///     速度キャップ (DH1.03以降)
        /// </summary>
        public double SpeedCap { get; set; }

        /// <summary>
        ///     装備 (DH1.03以降)
        /// </summary>
        public List<UnitEquipment> Equipments { get; private set; }

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
        public UnitModel()
        {
            Equipments = new List<UnitEquipment>();
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
            Equipments = new List<UnitEquipment>(original.Equipments);
        }

        #endregion

        #region 装備リスト

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

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニットモデルが編集済みかどうかを取得する
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
        public bool IsDirty(UnitModelItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     国別モデル名が編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirtyName(Country country)
        {
            return (country == Country.None) ? _dirtyFlags[(int) UnitModelItemId.Name] : _nameDirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(UnitModelItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     国別モデル名の編集済みフラグを設定する
        /// </summary>
        /// <param name="country">項目ID</param>
        public void SetDirtyName(Country country)
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
        public void SetDirtyAll()
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
        public void ResetDirtyAll()
        {
            foreach (UnitModelItemId id in Enum.GetValues(typeof (UnitModelItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (Country country in Enum.GetValues(typeof (Country)))
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
    public class UnitEquipment
    {
        #region 公開プロパティ

        /// <summary>
        ///     資源
        /// </summary>
        public EquipmentType Resource { get; set; }

        /// <summary>
        ///     量
        /// </summary>
        public double Quantity { get; set; }

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

        #region 編集済みフラグ操作

        /// <summary>
        ///     ユニット装備情報が編集済みかどうかを取得する
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
        public bool IsDirty(UnitEquipmentItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(UnitEquipmentItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
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
        public void ResetDirtyAll()
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
    public class UnitUpgrade
    {
        #region 公開プロパティ

        /// <summary>
        ///     ユニットの種類
        /// </summary>
        public UnitType Type { get; set; }

        /// <summary>
        ///     改良時間補正
        /// </summary>
        public double UpgradeTimeFactor { get; set; }

        /// <summary>
        ///     改良IC補正
        /// </summary>
        public double UpgradeCostFactor { get; set; }

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
        ///     ユニット装備情報が編集済みかどうかを取得する
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
        public bool IsDirty(UnitUpgradeItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(UnitUpgradeItemId id)
        {
            _dirtyFlags[(int) id] = true;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
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
        public void ResetDirtyAll()
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
    ///     スプライトの種類 (DH1.03以降用)
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

    /// <summary>
    ///     装備の種類 (DH1.03以降用)
    /// </summary>
    public enum EquipmentType
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
        NuclearSubmarine,
    }

    /// <summary>
    ///     ユニットクラス項目ID
    /// </summary>
    public enum UnitClassItemId
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
        MaxAllowedBrigades, // 最大旅団数
    }

    /// <summary>
    ///     ユニットモデル項目ID
    /// </summary>
    public enum UnitModelItemId
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
        SpeedCap, // 速度キャップ (DH1.03以降)
    }

    /// <summary>
    ///     ユニット装備項目ID
    /// </summary>
    public enum UnitEquipmentItemId
    {
        Resource, // 資源
        Quantity, // 量
    }

    /// <summary>
    ///     ユニット更新項目ID
    /// </summary>
    public enum UnitUpgradeItemId
    {
        Type, // ユニットの種類
        UpgradeTimeFactor, // 改良時間補正
        UpgradeCostFactor, // 改良IC補正
    }
}