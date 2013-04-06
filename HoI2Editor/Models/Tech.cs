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
        #region 公開プロパティ

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
        public List<ITechItem> Items { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechGroupItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechGroup()
        {
            Items = new List<ITechItem>();
        }

        #endregion

        #region 技術項目リスト操作

        /// <summary>
        ///     技術項目リストに項目を挿入する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        public void AddItem(ITechItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        ///     技術項目リストに項目を挿入する
        /// </summary>
        /// <param name="item">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public void InsertItem(ITechItem item, ITechItem position)
        {
            Items.Insert(Items.IndexOf(position) + 1, item);
        }

        /// <summary>
        ///     技術項目リストから項目を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        public void RemoveItem(ITechItem item)
        {
            Items.Remove(item);

            // 一時キーを削除する
            item.RemoveTempKey();
        }

        /// <summary>
        ///     技術項目リストの項目を移動する
        /// </summary>
        /// <param name="src">移動対象の項目</param>
        /// <param name="dest">移動先位置の項目</param>
        public void MoveItem(ITechItem src, ITechItem dest)
        {
            int srcIndex = Items.IndexOf(src);
            int destIndex = Items.IndexOf(dest);

            if (srcIndex > destIndex)
            {
                // 上へ移動する場合
                Items.Insert(destIndex, src);
                Items.RemoveAt(srcIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                Items.Insert(destIndex + 1, src);
                Items.RemoveAt(srcIndex);
            }
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     技術グループ名を取得する
        /// </summary>
        /// <returns>技術グループ名</returns>
        public override string ToString()
        {
            return Config.GetText(Name);
        }

        /// <summary>
        ///     技術グループ説明を取得する
        /// </summary>
        /// <returns>技術グループ説明</returns>
        public string GetDesc()
        {
            return Config.GetText(Desc);
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     技術グループが編集済みかどうかを取得する
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
        public bool IsDirty(TechGroupItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechGroupItemId id)
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
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TechGroupItemId id in Enum.GetValues(typeof (TechGroupItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (ITechItem item in Items)
            {
                item.ResetDirtyAll();
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     技術項目の共通インターフェース
    /// </summary>
    public interface ITechItem
    {
        /// <summary>
        ///     座標リスト
        /// </summary>
        List<TechPosition> Positions { get; }

        /// <summary>
        ///     技術項目を複製する
        /// </summary>
        /// <returns>複製した技術項目</returns>
        ITechItem Clone();

        /// <summary>
        ///     文字列の一時キーを保存形式に変更する
        /// </summary>
        /// <param name="name">キー文字列</param>
        void RenameTempKey(string name);

        /// <summary>
        ///     文字列の一時キーを削除する
        /// </summary>
        void RemoveTempKey();

        /// <summary>
        ///     技術項目データが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        bool IsDirty();

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        bool IsDirty(TechItemId id);

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        void SetDirty(TechItemId id);

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        void SetDirty();

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        void SetDirtyAll();

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        void ResetDirtyAll();
    }

    /// <summary>
    ///     技術項目
    /// </summary>
    public class TechItem : ITechItem
    {
        #region 公開プロパティ

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
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     史実年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///     小研究リスト
        /// </summary>
        public List<TechComponent> Components { get; private set; }

        /// <summary>
        ///     必要技術リスト(AND条件)
        /// </summary>
        public List<RequiredTech> AndRequiredTechs { get; private set; }

        /// <summary>
        ///     必要技術リスト(OR条件)
        /// </summary>
        public List<RequiredTech> OrRequiredTechs { get; private set; }

        /// <summary>
        ///     技術効果リスト
        /// </summary>
        public List<Command> Effects { get; private set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechItem()
        {
            Positions = new List<TechPosition>();
            AndRequiredTechs = new List<RequiredTech>();
            OrRequiredTechs = new List<RequiredTech>();
            Components = new List<TechComponent>();
            Effects = new List<Command>();
        }

        /// <summary>
        ///     技術アプリケーションを複製する
        /// </summary>
        /// <returns>複製した技術アプリケーション</returns>
        public ITechItem Clone()
        {
            var item = new TechItem
                           {
                               Id = Id + 10,
                               Name = Config.GetTempKey(),
                               ShortName = Config.GetTempKey(),
                               Desc = Config.GetTempKey(),
                               PictureName = PictureName,
                               Year = Year,
                           };

            // 文字列設定
            Config.SetText(item.Name, Config.GetText(Name), Game.TechTextFileName);
            Config.SetText(item.ShortName, Config.GetText(ShortName), Game.TechTextFileName);
            Config.SetText(item.Desc, Config.GetText(Desc), Game.TechTextFileName);

            // 座標リスト
            foreach (TechPosition position in Positions)
            {
                item.Positions.Add(position.Clone());
            }

            // 小研究リスト
            foreach (TechComponent component in Components)
            {
                item.Components.Add(component.Clone());
            }

            // 必要技術リスト
            item.AndRequiredTechs.AddRange(AndRequiredTechs);
            item.OrRequiredTechs.AddRange(OrRequiredTechs);

            // 技術効果リスト
            foreach (Command command in Effects)
            {
                item.Effects.Add(command.Clone());
            }

            return item;
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

        #region 文字列操作

        /// <summary>
        ///     文字列の一時キーを保存形式に変更する
        /// </summary>
        /// <param name="name">キー文字列</param>
        public void RenameTempKey(string name)
        {
            // 技術名
            if (Config.IsReservedKey(Name, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_APP_{0}_{1}_NAME", name, Id);
                Config.RenameText(Name, newKey, Game.TechTextFileName);
                Name = newKey;
            }
            // 技術短縮名
            if (Config.IsReservedKey(ShortName, Game.TechTextFileName))
            {
                string newKey = String.Format("SHORT_TECH_APP_{0}_{1}_NAME", name, Id);
                Config.RenameText(ShortName, newKey, Game.TechTextFileName);
                ShortName = newKey;
            }
            // 技術説明
            if (Config.IsReservedKey(Desc, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_APP_{0}_{1}_DESC", name, Id);
                Config.RenameText(Desc, newKey, Game.TechTextFileName);
                Desc = newKey;
            }
            // 小研究名
            int componentId = 1;
            foreach (TechComponent component in Components)
            {
                if (Config.IsReservedKey(component.Name, Game.TechTextFileName))
                {
                    string newKey = String.Format("TECH_CMP_{0}_{1}_{2}_NAME", name, Id, componentId);
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

        /// <summary>
        ///     技術名を取得する
        /// </summary>
        /// <returns>技術名</returns>
        public override string ToString()
        {
            return Config.GetText(Name);
        }

        /// <summary>
        ///     技術短縮名を取得する
        /// </summary>
        /// <returns>技術短縮名</returns>
        public string GetShortName()
        {
            return Config.GetText(ShortName);
        }

        /// <summary>
        ///     技術説明を取得する
        /// </summary>
        /// <returns>技術説明</returns>
        public string GetDesc()
        {
            return Config.GetText(Desc);
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
        public bool IsDirty(TechItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
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
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            foreach (TechPosition position in Positions)
            {
                position.SetDirtyAll();
            }
            foreach (RequiredTech tech in AndRequiredTechs)
            {
                tech.SetDirty();
            }
            foreach (RequiredTech tech in OrRequiredTechs)
            {
                tech.SetDirty();
            }
            foreach (TechComponent component in Components)
            {
                component.SetDirtyAll();
            }
            foreach (Command command in Effects)
            {
                command.SetDirtyAll();
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (TechPosition position in Positions)
            {
                position.ResetDirtyAll();
            }
            foreach (RequiredTech tech in AndRequiredTechs)
            {
                tech.ResetDirty();
            }
            foreach (RequiredTech tech in OrRequiredTechs)
            {
                tech.ResetDirty();
            }
            foreach (TechComponent component in Components)
            {
                component.ResetDirtyAll();
            }
            foreach (Command command in Effects)
            {
                command.ResetDirtyAll();
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     技術ラベル
    /// </summary>
    public class TechLabel : ITechItem
    {
        #region 公開プロパティ

        /// <summary>
        ///     ラベル名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechLabel()
        {
            Positions = new List<TechPosition>();
        }

        /// <summary>
        ///     技術ラベルを複製する
        /// </summary>
        /// <returns>複製した技術ラベル</returns>
        public ITechItem Clone()
        {
            var item = new TechLabel {Name = Config.GetTempKey()};

            // 文字列設定
            Config.SetText(item.Name, Config.GetText(Name), Game.TechTextFileName);

            // 座標リスト
            foreach (TechPosition position in Positions)
            {
                item.Positions.Add(position.Clone());
            }

            return item;
        }

        /// <summary>
        ///     技術ラベルを作成する
        /// </summary>
        /// <returns>作成した技術ラベル</returns>
        public static TechLabel Create()
        {
            var item = new TechLabel {Name = Config.GetTempKey()};

            // 文字列設定
            Config.SetText(item.Name, "", Game.TechTextFileName);

            return item;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列の一時キーを保存形式に変更する
        /// </summary>
        /// <param name="name">キー名</param>
        public void RenameTempKey(string name)
        {
            // ラベル名
            if (Config.IsReservedKey(Name, Game.TechTextFileName))
            {
                string newKey = String.Format("TECH_CAT_{0}", name);
                Config.RenameText(Name, newKey, Game.TechTextFileName);
                Name = newKey;
            }
        }

        /// <summary>
        ///     文字列の一時キーを削除する
        /// </summary>
        public void RemoveTempKey()
        {
            // ラベル名
            if (Config.IsReservedKey(Name, Game.TechTextFileName))
            {
                Config.RemoveText(Name, Game.TechTextFileName);
            }
        }

        /// <summary>
        ///     技術ラベル名を取得する
        /// </summary>
        /// <returns>技術ラベル名</returns>
        public override string ToString()
        {
            string s = Config.GetText(Name);

            // 色指定文字列を読み飛ばす
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
        public bool IsDirty(TechItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
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
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            foreach (TechPosition position in Positions)
            {
                position.SetDirtyAll();
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (TechPosition position in Positions)
            {
                position.ResetDirtyAll();
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     技術イベント
    /// </summary>
    public class TechEvent : ITechItem
    {
        #region 公開プロパティ

        /// <summary>
        ///     技術イベントID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     技術ID
        /// </summary>
        public int TechId { get; set; }

        /// <summary>
        ///     座標リスト
        /// </summary>
        public List<TechPosition> Positions { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TechEvent()
        {
            Positions = new List<TechPosition>();
        }

        /// <summary>
        ///     技術イベントを複製する
        /// </summary>
        /// <returns>複製した技術イベント</returns>
        public ITechItem Clone()
        {
            var item = new TechEvent {Id = Id, TechId = TechId};

            // 座標リスト
            foreach (TechPosition position in Positions)
            {
                item.Positions.Add(position.Clone());
            }

            return item;
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     文字列の一時キーをIDに沿った値に変更する
        /// </summary>
        /// <param name="name">キー名</param>
        public void RenameTempKey(string name)
        {
            // 何もしない
        }

        /// <summary>
        ///     文字列の一時キーを削除する
        /// </summary>
        public void RemoveTempKey()
        {
            // 何もしない
        }

        /// <summary>
        ///     技術イベント文字列を取得する
        /// </summary>
        /// <returns>技術イベント文字列</returns>
        public override string ToString()
        {
            // 名前が存在しないので代わりにIDを返す
            return Id.ToString(CultureInfo.InvariantCulture);
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
        public bool IsDirty(TechItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
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
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            foreach (TechPosition position in Positions)
            {
                position.SetDirtyAll();
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TechItemId id in Enum.GetValues(typeof (TechItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            foreach (TechPosition position in Positions)
            {
                position.ResetDirtyAll();
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     技術ツリー内の座標
    /// </summary>
    public class TechPosition
    {
        #region 公開プロパティ

        /// <summary>
        ///     X座標
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     Y座標
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechPositionItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

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
        public bool IsDirty(TechPositionItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechPositionItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
        {
            foreach (TechPositionItemId id in Enum.GetValues(typeof (TechPositionItemId)))
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
            foreach (TechPositionItemId id in Enum.GetValues(typeof (TechPositionItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     必要技術
    /// </summary>
    public class RequiredTech
    {
        #region 公開プロパティ

        /// <summary>
        ///     技術ID
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを解除する
        /// </summary>
        public void ResetDirty()
        {
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     小研究
    /// </summary>
    public class TechComponent
    {
        #region 公開プロパティ

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

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TechComponentItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 初期化

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

            // 文字列設定
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

        #region 文字列操作

        /// <summary>
        ///     小研究名を取得する
        /// </summary>
        /// <returns>小研究名</returns>
        public override string ToString()
        {
            return Config.GetText(Name);
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
        public bool IsDirty(TechComponentItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TechComponentItemId id)
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
            foreach (TechComponentItemId id in Enum.GetValues(typeof (TechComponentItemId)))
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
            foreach (TechComponentItemId id in Enum.GetValues(typeof (TechComponentItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     技術カテゴリ
    /// </summary>
    public enum TechCategory
    {
        Infantry, // 歩兵
        Armor, // 装甲と火砲
        Naval, // 艦船
        Aircraft, // 航空機
        Industry, // 産業
        LandDoctrines, // 陸戦ドクトリン
        SecretWeapons, // 秘密兵器
        NavalDoctrines, // 海戦ドクトリン
        AirDoctrines, // 空戦ドクトリン
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

    /// <summary>
    ///     技術グループ項目ID
    /// </summary>
    public enum TechGroupItemId
    {
        Name, // 名前
        Desc, // 説明
    }

    /// <summary>
    ///     技術項目ID
    /// </summary>
    public enum TechItemId
    {
        Id, // ID
        Name, // 名前
        ShortName, // 短縮名
        Desc, // 説明
        PictureName, // 画像ファイル名
        Year, // 史実年
        TechId, // 技術ID
    }

    /// <summary>
    ///     技術座標項目ID
    /// </summary>
    public enum TechPositionItemId
    {
        X, // X座標
        Y, // Y座標
    }

    /// <summary>
    ///     小研究項目ID
    /// </summary>
    public enum TechComponentItemId
    {
        Id, // 小研究ID
        Name, // 小研究名
        Specilaity, // 研究特性
        Difficulty, // 難易度
        DoubleTime, // 2倍の時間を要するかどうか
    }
}