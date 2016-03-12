using System.Collections.Generic;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     文字列の履歴を管理するクラス
    /// </summary>
    internal class History
    {
        /// <summary>
        ///     履歴の実体
        /// </summary>
        private readonly List<string> _items;

        /// <summary>
        ///     履歴の最大数
        /// </summary>
        private readonly int _size;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="size">履歴の最大数</param>
        internal History(int size)
        {
            _size = size;
            _items = new List<string>();
        }

        /// <summary>
        ///     履歴の項目をクリアする
        /// </summary>
        internal void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        ///     履歴の項目を取得する
        /// </summary>
        /// <returns>履歴の項目</returns>
        internal string[] Get()
        {
            return _items.ToArray();
        }

        /// <summary>
        ///     履歴の項目を設定する
        /// </summary>
        /// <param name="items">履歴の項目</param>
        internal void Set(string[] items)
        {
            _items.Clear();
            _items.AddRange(items);
        }

        /// <summary>
        ///     履歴の項目を追加する
        /// </summary>
        /// <param name="item">履歴の項目</param>
        internal void Add(string item)
        {
            if (!_items.Contains(item))
            {
                // 先頭に項目を追加する
                _items.Insert(0, item);
                // 最大数を超えていれば末尾の項目を削除する
                if (_items.Count > _size)
                {
                    _items.RemoveAt(_size);
                }
            }
            else
            {
                // 重複する要素を削除する
                int index = _items.IndexOf(item);
                _items.RemoveAt(index);
                // 先頭に項目を追加する
                _items.Insert(0, item);
            }
        }
    }
}