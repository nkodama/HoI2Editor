using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     技術ツリーパネルのコントローラクラス
    /// </summary>
    internal class TechTreePanelController
    {
        #region 公開プロパティ

        /// <summary>
        ///     技術カテゴリ
        /// </summary>
        internal TechCategory Category { get; set; }

        /// <summary>
        ///     項目ラベルの表示に項目の状態を反映するかどうか
        /// </summary>
        internal bool ApplyItemStatus { get; set; }

        /// <summary>
        ///     項目ラベルのドラッグアンドドロップを許可するかどうか
        /// </summary>
        internal bool AllowDragDrop { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     技術ツリーパネルのピクチャーボックス
        /// </summary>
        private readonly PictureBox _pictureBox;

        /// <summary>
        ///     技術ラベルの幅
        /// </summary>
        private static int _techLabelWidth;

        /// <summary>
        ///     技術ラベルの高さ
        /// </summary>
        private static int _techLabelHeight;

        /// <summary>
        ///     イベントラベルの幅
        /// </summary>
        private static int _eventLabelWidth;

        /// <summary>
        ///     イベントラベルの高さ
        /// </summary>
        private static int _eventLabelHeight;

        /// <summary>
        ///     技術ラベルの画像
        /// </summary>
        private static Bitmap _techLabelBitmap;

        /// <summary>
        ///     完了技術ラベルの画像
        /// </summary>
        private static Bitmap _doneTechLabelBitmap;

        /// <summary>
        ///     青写真付き技術ラベルの画像
        /// </summary>
        private static Bitmap _blueprintTechLabelBitmap;

        /// <summary>
        ///     青写真付き完了技術ラベルの画像
        /// </summary>
        private static Bitmap _blueprintDoneTechLabelBitmap;

        /// <summary>
        ///     イベントラベルの画像
        /// </summary>
        private static Bitmap _eventLabelBitmap;

        /// <summary>
        ///     完了イベントラベルの画像
        /// </summary>
        private static Bitmap _doneEventLabelBitmap;

        /// <summary>
        ///     技術ラベルのマスク画像
        /// </summary>
        private static Bitmap _techLabelMask;

        /// <summary>
        ///     イベントラベルのマスク画像
        /// </summary>
        private static Bitmap _eventLabelMask;

        /// <summary>
        ///     技術ラベルの描画領域
        /// </summary>
        private static Region _techLabelRegion;

        /// <summary>
        ///     イベントラベルの描画領域
        /// </summary>
        private static Region _eventLabelRegion;

        /// <summary>
        ///     ラベル画像の読み込み済みフラグ
        /// </summary>
        private static bool _labelInitialized;

        /// <summary>
        ///     ドラッグアンドドロップの開始位置
        /// </summary>
        private static Point _dragPoint = Point.Empty;

        /// <summary>
        ///     ドラッグ中のカーソル
        /// </summary>
        private static Cursor _dragCursor;

        #endregion

        #region 内部定数

        /// <summary>
        ///     技術ラベルの幅の基準値
        /// </summary>
        private const int TechLabelWidthBase = 112;

        /// <summary>
        ///     技術ラベルの高さの基準値
        /// </summary>
        private const int TechLabelHeightBase = 16;

        /// <summary>
        ///     イベントラベルの幅の基準値
        /// </summary>
        private const int EventLabelWidthBase = 112;

        /// <summary>
        ///     イベントラベルの高さの基準値
        /// </summary>
        private const int EventLabelHeightBase = 24;

        /// <summary>
        ///     青写真アイコンの幅
        /// </summary>
        private const int BlueprintIconWidth = 16;

        /// <summary>
        ///     青写真アイコンの幅
        /// </summary>
        private const int BlueprintIconHeight = 16;

        /// <summary>
        ///     青写真アイコンのX座標
        /// </summary>
        private const int BlueprintIconX = 88;

        /// <summary>
        ///     青写真アイコンのY座標
        /// </summary>
        private const int BlueprintIconY = 0;

        /// <summary>
        ///     技術ツリー画像ファイル名
        /// </summary>
        private static readonly string[] TechTreeFileNames =
        {
            "techtree_infantry.bmp",
            "techtree_armor.bmp",
            "techtree_naval.bmp",
            "techtree_aircraft.bmp",
            "techtree_industry.bmp",
            "techtree_land_doctrine.bmp",
            "techtree_secret_weapons.bmp",
            "techtree_naval_doctrines.bmp",
            "techtree_air_doctrines.bmp"
        };

        #endregion

        #region 公開イベント

        /// <summary>
        ///     項目ラベルクリック時のイベント
        /// </summary>
        internal event EventHandler<ItemEventArgs> ItemClick;

        /// <summary>
        ///     項目ラベルマウスクリック時のイベント
        /// </summary>
        internal event EventHandler<ItemMouseEventArgs> ItemMouseClick;

        /// <summary>
        ///     項目ラベルマウスダウン時のイベント
        /// </summary>
        internal event EventHandler<ItemMouseEventArgs> ItemMouseDown;

        /// <summary>
        ///     項目ラベルドラッグアンドドロップ時のイベント
        /// </summary>
        internal event EventHandler<ItemDragEventArgs> ItemDragDrop;

        /// <summary>
        ///     項目状態問い合わせイベント
        /// </summary>
        internal event EventHandler<QueryItemStatusEventArgs> QueryItemStatus;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="pictureBox">技術ツリーピクチャーボックス</param>
        internal TechTreePanelController(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;

            // ラベル画像を初期化する
            InitLabelBitmap();

            // イベントハンドラを初期化する
            InitEventHandler();

            // 技術ツリーピクチャーボックスへのドラッグアンドドロップを許可する
            // 設計時プロパティに存在しないので初期化時に設定する
            _pictureBox.AllowDrop = true;
        }

        /// <summary>
        ///     イベントハンドラを初期化する
        /// </summary>
        private void InitEventHandler()
        {
            _pictureBox.DragOver += OnPictureBoxDragOver;
            _pictureBox.DragDrop += OnPictureBoxDragDrop;
        }

        #endregion

        #region 技術ツリー操作

        /// <summary>
        ///     技術ツリーを更新する
        /// </summary>
        internal void Update()
        {
            // 技術ツリー画像を更新する
            UpdateTechTreeImage();

            // 項目ラベルを更新する
            UpdateItems();
        }

        /// <summary>
        ///     技術ツリーをクリアする
        /// </summary>
        internal void Clear()
        {
            // 技術ツリー画像をクリアする
            ClearTechTreeImage();

            // 項目ラベルをクリアする
            ClearItems();
        }

        #endregion

        #region 技術ツリー画像

        /// <summary>
        ///     技術ツリー画像を更新する
        /// </summary>
        private void UpdateTechTreeImage()
        {
            Bitmap original = new Bitmap(Game.GetReadFileName(Game.PicturePathName, TechTreeFileNames[(int) Category]));
            original.MakeTransparent(Color.Lime);

            int width = DeviceCaps.GetScaledWidth(original.Width);
            int height = DeviceCaps.GetScaledHeight(original.Height);
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(original, 0, 0, width, height);
            g.Dispose();
            original.Dispose();

            Image prev = _pictureBox.Image;
            _pictureBox.Image = bitmap;
            prev?.Dispose();
        }

        /// <summary>
        ///     技術ツリー画像をクリアする
        /// </summary>
        private void ClearTechTreeImage()
        {
            Image prev = _pictureBox.Image;
            _pictureBox.Image = null;
            prev?.Dispose();
        }

        #endregion

        #region 項目ラベル

        /// <summary>
        ///     技術ツリーの項目ラベルを更新する
        /// </summary>
        private void UpdateItems()
        {
            _pictureBox.Controls.Clear();
            foreach (ITechItem item in Techs.Groups[(int) Category].Items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        ///     技術ツリーの項目ラベルをクリアする
        /// </summary>
        private void ClearItems()
        {
            _pictureBox.Controls.Clear();
        }

        /// <summary>
        ///     技術ツリーに項目ラベル群を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        internal void AddItem(ITechItem item)
        {
            foreach (TechPosition position in item.Positions)
            {
                AddItem(item, position);
            }
        }

        /// <summary>
        ///     技術ツリーに項目ラベルを追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        internal void AddItem(ITechItem item, TechPosition position)
        {
            TechItem tech = item as TechItem;
            if (tech != null)
            {
                AddTechItem(tech, position);
                return;
            }

            TechLabel label = item as TechLabel;
            if (label != null)
            {
                AddLabelItem(label, position);
                return;
            }

            TechEvent ev = item as TechEvent;
            if (ev != null)
            {
                AddEventItem(ev, position);
            }
        }

        /// <summary>
        ///     技術ツリーに技術項目を追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddTechItem(TechItem item, TechPosition position)
        {
            Label label = new Label
            {
                Location = new Point(DeviceCaps.GetScaledWidth(position.X), DeviceCaps.GetScaledHeight(position.Y)),
                BackColor = Color.Transparent,
                Tag = new TechLabelInfo { Item = item, Position = position },
                Size = new Size(_techLabelBitmap.Width, _techLabelBitmap.Height),
                Region = _techLabelRegion
            };

            // ラベル画像を設定する
            if (ApplyItemStatus && (QueryItemStatus != null))
            {
                QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                QueryItemStatus(this, e);
                label.Image = e.Done
                    ? (e.Blueprint ? _blueprintDoneTechLabelBitmap : _doneTechLabelBitmap)
                    : (e.Blueprint ? _blueprintTechLabelBitmap : _techLabelBitmap);
            }
            else
            {
                label.Image = _techLabelBitmap;
            }

            label.Click += OnItemLabelClick;
            label.MouseClick += OnItemLabelMouseClick;
            label.MouseDown += OnItemLabelMouseDown;
            label.MouseUp += OnItemLabelMouseUp;
            label.MouseMove += OnItemLabelMouseMove;
            label.GiveFeedback += OnItemLabelGiveFeedback;
            label.Paint += OnTechItemPaint;

            _pictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーに技術ラベルを追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddLabelItem(TechLabel item, TechPosition position)
        {
            Label label = new Label
            {
                Location = new Point(DeviceCaps.GetScaledWidth(position.X), DeviceCaps.GetScaledHeight(position.Y)),
                BackColor = Color.Transparent,
                Tag = new TechLabelInfo { Item = item, Position = position }
            };
            label.Size = Graphics.FromHwnd(label.Handle).MeasureString(item.ToString(), label.Font).ToSize();

            label.Click += OnItemLabelClick;
            label.MouseClick += OnItemLabelMouseClick;
            label.MouseDown += OnItemLabelMouseDown;
            label.MouseUp += OnItemLabelMouseUp;
            label.MouseMove += OnItemLabelMouseMove;
            label.GiveFeedback += OnItemLabelGiveFeedback;
            label.Paint += OnTechLabelPaint;

            _pictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーに発明イベントを追加する
        /// </summary>
        /// <param name="item">追加対象の項目</param>
        /// <param name="position">追加対象の位置</param>
        private void AddEventItem(TechEvent item, TechPosition position)
        {
            Label label = new Label
            {
                Location = new Point(DeviceCaps.GetScaledWidth(position.X), DeviceCaps.GetScaledHeight(position.Y)),
                BackColor = Color.Transparent,
                Tag = new TechLabelInfo { Item = item, Position = position },
                Size = new Size(_eventLabelBitmap.Width, _eventLabelBitmap.Height),
                Region = _eventLabelRegion
            };

            // ラベル画像を設定する
            if (ApplyItemStatus && (QueryItemStatus != null))
            {
                QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                QueryItemStatus(this, e);
                label.Image = e.Done ? _doneEventLabelBitmap : _eventLabelBitmap;
            }
            else
            {
                label.Image = _doneEventLabelBitmap;
            }

            label.Click += OnItemLabelClick;
            label.MouseClick += OnItemLabelMouseClick;
            label.MouseDown += OnItemLabelMouseDown;
            label.MouseUp += OnItemLabelMouseUp;
            label.MouseMove += OnItemLabelMouseMove;
            label.GiveFeedback += OnItemLabelGiveFeedback;

            _pictureBox.Controls.Add(label);
        }

        /// <summary>
        ///     技術ツリーの項目群を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        internal void RemoveItem(ITechItem item)
        {
            Control.ControlCollection labels = _pictureBox.Controls;
            foreach (Label label in labels)
            {
                TechLabelInfo info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }

                if (info.Item == item)
                {
                    _pictureBox.Controls.Remove(label);
                }
            }
        }

        /// <summary>
        ///     技術ツリーの項目を削除する
        /// </summary>
        /// <param name="item">削除対象の項目</param>
        /// <param name="position">削除対象の位置</param>
        internal void RemoveItem(ITechItem item, TechPosition position)
        {
            Control.ControlCollection labels = _pictureBox.Controls;
            foreach (Label label in labels)
            {
                TechLabelInfo info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }

                if (info.Item == item && info.Position == position)
                {
                    _pictureBox.Controls.Remove(label);
                }
            }
        }

        /// <summary>
        ///     技術ツリーの項目ラベルを更新する
        /// </summary>
        /// <param name="item">更新対象の項目</param>
        internal void UpdateItem(ITechItem item)
        {
            Control.ControlCollection labels = _pictureBox.Controls;
            foreach (Label label in labels)
            {
                TechLabelInfo info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }

                if (info.Item != item)
                {
                    continue;
                }

                // ラベル項目の場合はサイズを再計算する
                if (item is TechLabel)
                {
                    label.Size = Graphics.FromHwnd(label.Handle).MeasureString(item.ToString(), label.Font).ToSize();
                }

                // 項目ラベルの表示に項目の状態を反映しないならば再描画のみ
                if (!ApplyItemStatus || (QueryItemStatus == null))
                {
                    label.Refresh();
                    continue;
                }

                if (item is TechItem)
                {
                    // ラベル画像を設定する
                    QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                    QueryItemStatus(this, e);
                    label.Image = e.Done
                        ? (e.Blueprint ? _blueprintDoneTechLabelBitmap : _doneTechLabelBitmap)
                        : (e.Blueprint ? _blueprintTechLabelBitmap : _techLabelBitmap);
                }
                else if (item is TechEvent)
                {
                    // ラベル画像を設定する
                    QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                    QueryItemStatus(this, e);
                    label.Image = e.Done ? _doneEventLabelBitmap : _eventLabelBitmap;
                }
            }
        }

        /// <summary>
        ///     技術ツリーの項目を更新する
        /// </summary>
        /// <param name="item">更新対象の項目</param>
        /// <param name="position">更新対象の座標</param>
        internal void UpdateItem(ITechItem item, TechPosition position)
        {
            Control.ControlCollection labels = _pictureBox.Controls;
            foreach (Label label in labels)
            {
                TechLabelInfo info = label.Tag as TechLabelInfo;
                if (info == null)
                {
                    continue;
                }

                if (info.Item != item)
                {
                    continue;
                }

                // ラベルの位置を更新する
                if (info.Position == position)
                {
                    label.Location = new Point(DeviceCaps.GetScaledWidth(position.X),
                        DeviceCaps.GetScaledHeight(position.Y));
                }

                // 項目ラベルの表示に項目の状態を反映しないならば座標変更のみ
                if (!ApplyItemStatus || (QueryItemStatus == null))
                {
                    continue;
                }

                if (item is TechItem)
                {
                    // ラベル画像を設定する
                    QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                    QueryItemStatus(this, e);
                    label.Image = e.Done
                        ? (e.Blueprint ? _blueprintDoneTechLabelBitmap : _doneTechLabelBitmap)
                        : (e.Blueprint ? _blueprintTechLabelBitmap : _techLabelBitmap);
                }
                else if (item is TechEvent)
                {
                    // ラベル画像を設定する
                    QueryItemStatusEventArgs e = new QueryItemStatusEventArgs(item);
                    QueryItemStatus(this, e);
                    label.Image = e.Done ? _doneEventLabelBitmap : _eventLabelBitmap;
                }
            }
        }

        /// <summary>
        ///     技術項目描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTechItemPaint(object sender, PaintEventArgs e)
        {
            Label label = sender as Label;
            TechLabelInfo info = label?.Tag as TechLabelInfo;
            TechItem techItem = info?.Item as TechItem;

            string s = techItem?.GetShortName();
            if (string.IsNullOrEmpty(s))
            {
                return;
            }
            Brush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(s, label.Font, brush, 6, 2);
            brush.Dispose();
        }

        /// <summary>
        ///     ラベル項目描画時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTechLabelPaint(object sender, PaintEventArgs e)
        {
            Label label = sender as Label;
            TechLabelInfo info = label?.Tag as TechLabelInfo;
            TechLabel labelItem = info?.Item as TechLabel;

            string s = labelItem?.ToString();
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            // 色指定文字列を解釈する
            Brush brush;
            if ((s[0] == '%' || s[0] == 'ｧ' || s[0] == '§') &&
                s.Length > 4 &&
                s[1] >= '0' && s[1] <= '9' &&
                s[2] >= '0' && s[2] <= '9' &&
                s[3] >= '0' && s[3] <= '9')
            {
                brush = new SolidBrush(Color.FromArgb((s[3] - '0') << 5, (s[2] - '0') << 5, (s[1] - '0') << 5));
                s = s.Substring(4);
            }
            else
            {
                brush = new SolidBrush(Color.White);
            }
            e.Graphics.DrawString(s, label.Font, brush, -2, 0);
            brush.Dispose();
        }

        /// <summary>
        ///     項目ラベルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelClick(object sender, EventArgs e)
        {
            // イベントハンドラを呼び出す
            if (ItemClick != null)
            {
                Label label = sender as Label;
                TechLabelInfo info = label?.Tag as TechLabelInfo;
                if (info == null)
                {
                    return;
                }

                ItemClick(sender, new ItemEventArgs(info.Item, info.Position));
            }
        }

        /// <summary>
        ///     項目ラベルマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelMouseClick(object sender, MouseEventArgs e)
        {
            // イベントハンドラを呼び出す
            if (ItemMouseClick != null)
            {
                Label label = sender as Label;
                TechLabelInfo info = label?.Tag as TechLabelInfo;
                if (info == null)
                {
                    return;
                }

                ItemMouseClick(sender, new ItemMouseEventArgs(info.Item, info.Position, e));
            }
        }

        /// <summary>
        ///     項目ラベルマウスダウン時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelMouseDown(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            TechLabelInfo info = label?.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            // ドラッグアンドドロップの開始準備
            if (AllowDragDrop)
            {
                // 左ボタンダウンでなければドラッグ状態を解除する
                if (e.Button != MouseButtons.Left)
                {
                    _dragPoint = Point.Empty;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                // ドラッグ開始位置を設定する
                _dragPoint = new Point(label.Left + e.X, label.Top + e.Y);
            }

            // イベントハンドラを呼び出す
            ItemMouseDown?.Invoke(sender, new ItemMouseEventArgs(info.Item, info.Position, e));
        }

        /// <summary>
        ///     項目ラベルマウスアップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelMouseUp(object sender, MouseEventArgs e)
        {
            // ドラッグアンドドロップが無効ならば何もしない
            if (!AllowDragDrop)
            {
                return;
            }

            // ドラッグ状態を解除する
            _dragPoint = Point.Empty;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        ///     項目ラベルマウス移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelMouseMove(object sender, MouseEventArgs e)
        {
            // ドラッグアンドドロップが無効ならば何もしない
            if (!AllowDragDrop)
            {
                return;
            }

            // ドラッグ中でなければ何もしない
            if (_dragPoint == Point.Empty)
            {
                return;
            }

            Label label = sender as Label;
            if (label == null)
            {
                return;
            }

            // ドラッグ判定サイズを超えていなければ何もしない
            Size dragSize = SystemInformation.DragSize;
            Rectangle dragRect = new Rectangle(_dragPoint.X - dragSize.Width / 2, _dragPoint.Y - dragSize.Height / 2,
                dragSize.Width, dragSize.Height);
            if (dragRect.Contains(label.Left + e.X, label.Top + e.Y))
            {
                return;
            }

            TechLabelInfo info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }

            // カーソル画像を作成する
            Bitmap bitmap = new Bitmap(label.Width, label.Height);
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            label.DrawToBitmap(bitmap, new Rectangle(0, 0, label.Width, label.Height));
            if (info.Item is TechItem)
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _techLabelMask, _dragPoint.X - label.Left,
                    _dragPoint.Y - label.Top);
            }
            else if (info.Item is TechLabel)
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _dragPoint.X - label.Left,
                    _dragPoint.Y - label.Top);
            }
            else
            {
                _dragCursor = CursorFactory.CreateCursor(bitmap, _eventLabelMask, _dragPoint.X - label.Left,
                    _dragPoint.Y - label.Top);
            }

            // ドラッグアンドドロップを開始する
            label.DoDragDrop(sender, DragDropEffects.Move);

            // ドラッグ状態を解除する
            _dragPoint = Point.Empty;
            _dragCursor.Dispose();

            bitmap.Dispose();
        }

        /// <summary>
        ///     項目ラベルのカーソル更新処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemLabelGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // ドラッグアンドドロップが無効ならば何もしない
            if (!AllowDragDrop)
            {
                return;
            }

            if ((e.Effect & DragDropEffects.Move) != 0)
            {
                // カーソル画像を設定する
                e.UseDefaultCursors = false;
                Cursor.Current = _dragCursor;
            }
            else
            {
                e.UseDefaultCursors = true;
            }
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureBoxDragOver(object sender, DragEventArgs e)
        {
            // ドラッグアンドドロップが無効ならば何もしない
            if (!AllowDragDrop)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // ラベルでなければ何もしない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            Label label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null)
            {
                return;
            }

            // 技術ツリー画像の範囲外ならばドロップを禁止する
            Rectangle dragRect = new Rectangle(0, 0, _pictureBox.Image.Width, _pictureBox.Image.Height);
            Point p = _pictureBox.PointToClient(new Point(e.X, e.Y));
            Rectangle r = new Rectangle(label.Left + p.X - _dragPoint.X, label.Top + p.Y - _dragPoint.Y, label.Width,
                label.Height);
            e.Effect = dragRect.Contains(r) ? DragDropEffects.Move : DragDropEffects.None;
        }

        /// <summary>
        ///     技術ツリーピクチャーボックスにドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureBoxDragDrop(object sender, DragEventArgs e)
        {
            // ドラッグアンドドロップが無効ならば何もしない
            if (!AllowDragDrop)
            {
                return;
            }

            // ラベルでなければ何もしない
            if (!e.Data.GetDataPresent(typeof (Label)))
            {
                return;
            }

            Label label = e.Data.GetData(typeof (Label)) as Label;
            if (label == null)
            {
                return;
            }

            // 技術ツリー上のドロップ座標を計算する
            Point p = new Point(e.X, e.Y);
            p = _pictureBox.PointToClient(p);
            p.X = label.Left + p.X - _dragPoint.X;
            p.Y = label.Top + p.Y - _dragPoint.Y;

            // ラベル情報の座標を更新する
            TechLabelInfo info = label.Tag as TechLabelInfo;
            if (info == null)
            {
                return;
            }
            info.Position.X = DeviceCaps.GetUnscaledWidth(p.X);
            info.Position.Y = DeviceCaps.GetUnscaledHeight(p.Y);

            // ラベルの座標を更新する
            label.Location = p;

            // イベントハンドラを呼び出す
            ItemDragDrop?.Invoke(this, new ItemDragEventArgs(info.Item, info.Position, e));
        }

        /// <summary>
        ///     ラベル画像を初期化する
        /// </summary>
        private static void InitLabelBitmap()
        {
            // 既に初期化済みならば何もしない
            if (_labelInitialized)
            {
                return;
            }

            // 技術ラベル
            Bitmap bitmap = new Bitmap(Game.GetReadFileName(Game.TechLabelPathName));
            _techLabelWidth = DeviceCaps.GetScaledWidth(TechLabelWidthBase);
            _techLabelHeight = DeviceCaps.GetScaledHeight(TechLabelHeightBase);
            _techLabelBitmap = new Bitmap(_techLabelWidth, _techLabelHeight);
            Graphics g = Graphics.FromImage(_techLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _techLabelWidth, _techLabelHeight),
                new Rectangle(0, 0, TechLabelWidthBase, TechLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();
            Color transparent = _techLabelBitmap.GetPixel(0, 0);

            // 青写真付き技術ラベル
            Bitmap icon = new Bitmap(Game.GetReadFileName(Game.BlueprintIconPathName));
            icon.MakeTransparent(icon.GetPixel(0, 0));
            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(icon, new Rectangle(BlueprintIconX, BlueprintIconY, BlueprintIconWidth, BlueprintIconHeight),
                new Rectangle(0, 0, BlueprintIconWidth, BlueprintIconHeight), GraphicsUnit.Pixel);
            g.Dispose();
            _blueprintTechLabelBitmap = new Bitmap(_techLabelWidth, _techLabelHeight);
            g = Graphics.FromImage(_blueprintTechLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _techLabelWidth, _techLabelHeight),
                new Rectangle(0, 0, TechLabelWidthBase, TechLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();
            bitmap.Dispose();

            // 完了技術ラベル
            bitmap = new Bitmap(Game.GetReadFileName(Game.DoneTechLabelPathName));
            _doneTechLabelBitmap = new Bitmap(_techLabelWidth, _techLabelHeight);
            g = Graphics.FromImage(_doneTechLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _techLabelWidth, _techLabelHeight),
                new Rectangle(0, 0, TechLabelWidthBase, TechLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();

            // 青写真付き完了技術ラベル
            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(icon, new Rectangle(BlueprintIconX, BlueprintIconY, BlueprintIconWidth, BlueprintIconHeight),
                new Rectangle(0, 0, BlueprintIconWidth, BlueprintIconHeight), GraphicsUnit.Pixel);
            g.Dispose();
            icon.Dispose();
            _blueprintDoneTechLabelBitmap = new Bitmap(_techLabelWidth, _techLabelHeight);
            g = Graphics.FromImage(_blueprintDoneTechLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _techLabelWidth, _techLabelHeight),
                new Rectangle(0, 0, TechLabelWidthBase, TechLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();
            bitmap.Dispose();

            // 技術ラベルの領域
            _techLabelMask = new Bitmap(_techLabelWidth, _techLabelHeight);
            _techLabelRegion = new Region(new Rectangle(0, 0, _techLabelWidth, _techLabelHeight));
            for (int y = 0; y < _techLabelBitmap.Height; y++)
            {
                for (int x = 0; x < _techLabelBitmap.Width; x++)
                {
                    if (_techLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        _techLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                        _techLabelMask.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        _techLabelMask.SetPixel(x, y, Color.Black);
                    }
                }
            }

            // 技術ラベルの透明色設定
            _techLabelBitmap.MakeTransparent(transparent);
            _blueprintTechLabelBitmap.MakeTransparent(transparent);
            _doneTechLabelBitmap.MakeTransparent(transparent);
            _blueprintDoneTechLabelBitmap.MakeTransparent(transparent);

            // 発明イベントラベル
            bitmap = new Bitmap(Game.GetReadFileName(Game.SecretLabelPathName));
            _eventLabelWidth = DeviceCaps.GetScaledWidth(EventLabelWidthBase);
            _eventLabelHeight = DeviceCaps.GetScaledHeight(EventLabelHeightBase);
            _eventLabelBitmap = new Bitmap(_eventLabelWidth, _eventLabelHeight);
            g = Graphics.FromImage(_eventLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _eventLabelWidth, _eventLabelHeight),
                new Rectangle(EventLabelWidthBase, 0, EventLabelWidthBase, EventLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();
            transparent = _eventLabelBitmap.GetPixel(0, 0);

            // 完了発明イベントラベル
            _doneEventLabelBitmap = new Bitmap(_eventLabelWidth, _eventLabelHeight);
            g = Graphics.FromImage(_doneEventLabelBitmap);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, new Rectangle(0, 0, _eventLabelWidth, _eventLabelHeight),
                new Rectangle(0, 0, EventLabelWidthBase, EventLabelHeightBase), GraphicsUnit.Pixel);
            g.Dispose();
            bitmap.Dispose();

            // 発明イベントラベルの領域
            _eventLabelMask = new Bitmap(_eventLabelWidth, _eventLabelHeight);
            _eventLabelRegion = new Region(new Rectangle(0, 0, _eventLabelWidth, _eventLabelHeight));
            for (int y = 0; y < _eventLabelBitmap.Height; y++)
            {
                for (int x = 0; x < _eventLabelBitmap.Width; x++)
                {
                    if (_eventLabelBitmap.GetPixel(x, y) == transparent)
                    {
                        _eventLabelRegion.Exclude(new Rectangle(x, y, 1, 1));
                        _eventLabelMask.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        _eventLabelMask.SetPixel(x, y, Color.Black);
                    }
                }
            }

            // 発明イベントラベルの透明色設定
            _eventLabelBitmap.MakeTransparent(transparent);
            _doneEventLabelBitmap.MakeTransparent(transparent);

            // 初期化済みフラグを設定する
            _labelInitialized = true;
        }

        #endregion

        #region 内部クラス

        /// <summary>
        ///     技術ラベルに関連付けられる情報
        /// </summary>
        private class TechLabelInfo
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item;

            /// <summary>
            ///     位置
            /// </summary>
            internal TechPosition Position;
        }

        /// <summary>
        ///     項目ラベルイベントのパラメータ
        /// </summary>
        internal class ItemEventArgs : EventArgs
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item { get; private set; }

            /// <summary>
            ///     項目ラベルの位置
            /// </summary>
            internal TechPosition Position { get; private set; }

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="item">技術項目</param>
            /// <param name="position">項目ラベルの位置</param>
            internal ItemEventArgs(ITechItem item, TechPosition position)
            {
                Item = item;
                Position = position;
            }
        }

        /// <summary>
        ///     項目ラベルマウスイベントのパラメータ
        /// </summary>
        internal class ItemMouseEventArgs : MouseEventArgs
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item { get; private set; }

            /// <summary>
            ///     項目ラベルの位置
            /// </summary>
            internal TechPosition Position { get; private set; }

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="item">技術項目</param>
            /// <param name="position">項目ラベルの位置</param>
            /// <param name="e">マウスイベントのパラメータ</param>
            internal ItemMouseEventArgs(ITechItem item, TechPosition position, MouseEventArgs e)
                : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
            {
                Item = item;
                Position = position;
            }
        }

        /// <summary>
        ///     項目ラベルドラッグアンドドロップイベントのパラメータ
        /// </summary>
        internal class ItemDragEventArgs : DragEventArgs
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item { get; private set; }

            /// <summary>
            ///     項目ラベルの位置
            /// </summary>
            internal TechPosition Position { get; private set; }

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="item">技術項目</param>
            /// <param name="position">項目ラベルの位置</param>
            /// <param name="e">ドラッグアンドドロップイベントのパラメータ</param>
            internal ItemDragEventArgs(ITechItem item, TechPosition position, DragEventArgs e)
                : base(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect)
            {
                Item = item;
                Position = position;
            }
        }

        /// <summary>
        ///     項目状態問い合わせイベントのパラメータ
        /// </summary>
        internal class QueryItemStatusEventArgs : EventArgs
        {
            /// <summary>
            ///     技術項目
            /// </summary>
            internal ITechItem Item { get; private set; }

            /// <summary>
            ///     完了したかどうか
            /// </summary>
            internal bool Done;

            /// <summary>
            ///     青写真ありかどうか
            /// </summary>
            internal bool Blueprint;

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="item">技術項目</param>
            internal QueryItemStatusEventArgs(ITechItem item)
            {
                Item = item;
            }
        }

        #endregion
    }
}