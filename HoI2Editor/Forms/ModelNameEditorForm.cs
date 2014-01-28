using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     モデル名エディタのフォーム
    /// </summary>
    public partial class ModelNameEditorForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ModelNameEditorForm()
        {
            InitializeComponent();

            // 自動スケーリングを考慮した初期化
            InitScaling();
        }

        /// <summary>
        ///     自動スケーリングを考慮した初期化
        /// </summary>
        private void InitScaling()
        {
            // 国家リストボックス
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);
            // ユニット種類リストボックス
            typeListBox.ItemHeight = DeviceCaps.GetScaledHeight(typeListBox.ItemHeight);
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // ユニットデータを初期化する
            Units.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // ユニットデータを読み込む
            Units.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ユニット種類リストボックスを初期化する
            InitTypeListBox();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        #endregion

        #region 終了処理

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2Editor.IsDirty())
            {
                return;
            }

            // 保存するかを問い合わせる
            DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    HoI2Editor.SaveFiles();
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModelNameEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnModelNameEditorFormClosed();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.SaveFiles();
                        break;
                }
            }

            HoI2Editor.ReloadFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.SaveFiles();
        }

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // 編集項目の表示を更新する
            UpdateEditableItems();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集項目の表示を更新する
            UpdateEditableItems();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            switch (id)
            {
                case EditorItemId.UnitName:
                    Debug.WriteLine("[ModelName] Changed unit name");
                    // ユニット種類リストボックスの表示項目を更新する
                    UpdateTypeListBox();
                    break;

                case EditorItemId.ModelList:
                    Debug.WriteLine("[ModelName] Changed model list");
                    // 編集項目の表示を更新する
                    UpdateEditableItems();
                    break;

                case EditorItemId.CommonModelName:
                    Debug.WriteLine("[ModelName] Changed common model name");
                    break;

                case EditorItemId.CountryModelName:
                    Debug.WriteLine("[ModelName] Changed country model name");
                    // 編集項目の表示を更新する
                    UpdateEditableItems();
                    break;
            }
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            countryListBox.BeginUpdate();
            countryListBox.Items.Clear();
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryListBox.Items.Add(s);
            }
            countryListBox.SelectedIndex = 0;
            countryListBox.EndUpdate();
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                Country country = Countries.Tags[e.Index];
                brush = Units.IsDirtyModelName(country)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = countryListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目の表示を更新する
            UpdateEditableItems();

            // 編集済みフラグが変化するのでユニット種類リストボックスの表示を更新する
            typeListBox.Refresh();
        }

        #endregion

        #region ユニット種類リストボックス

        /// <summary>
        ///     ユニット種類リストボックスを初期化する
        /// </summary>
        private void InitTypeListBox()
        {
            // リストボックスに項目を登録する
            typeListBox.BeginUpdate();
            typeListBox.Items.Clear();
            foreach (UnitType type in Units.UnitTypes)
            {
                Unit unit = Units.Items[(int) type];
                typeListBox.Items.Add(Config.GetText(unit.Name));
            }
            typeListBox.EndUpdate();

            // 先頭の項目を選択する
            if (typeListBox.Items.Count > 0)
            {
                typeListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     ユニット種類リストボックスの表示項目を更新する
        /// </summary>
        private void UpdateTypeListBox()
        {
            typeListBox.BeginUpdate();
            int top = typeListBox.TopIndex;
            int i = 0;
            foreach (UnitType type in Units.UnitTypes)
            {
                Unit unit = Units.Items[(int) type];
                typeListBox.Items[i] = Config.GetText(unit.Name);
                i++;
            }
            typeListBox.TopIndex = top;
            typeListBox.EndUpdate();
        }

        /// <summary>
        ///     ユニット種類リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTypeListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
            {
                // 変更ありの項目は文字色を変更する
                Country country = Countries.Tags[countryListBox.SelectedIndex];
                UnitType type = Units.UnitTypes[e.Index];
                brush = Units.IsDirtyModelName(country, type)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = typeListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     ユニット種類リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTypeListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 編集項目の表示を更新する
            UpdateEditableItems();
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     編集項目の表示を更新する
        /// </summary>
        private void UpdateEditableItems()
        {
            itemPanel.Controls.Clear();

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }

            // 選択中のユニット名種類がなければ戻る
            Country country = Countries.Tags[countryListBox.SelectedIndex];
            if (typeListBox.SelectedIndex < 0)
            {
                return;
            }
            Unit unit = Units.Items[(int) Units.UnitTypes[typeListBox.SelectedIndex]];

            Graphics g = Graphics.FromHwnd(Handle);
            int itemHeight = DeviceCaps.GetScaledHeight(25);
            int labelStartX = DeviceCaps.GetScaledWidth(10);
            int labelStartY = DeviceCaps.GetScaledHeight(13);
            int textBoxStartY = DeviceCaps.GetScaledHeight(10);
            int labelEditMargin = DeviceCaps.GetScaledWidth(10);
            int columnMargin = DeviceCaps.GetScaledWidth(10);
            int textBoxWidthBase = DeviceCaps.GetScaledWidth(100);
            int textBoxWidthMargin = DeviceCaps.GetScaledWidth(8);
            int textBoxHeight = DeviceCaps.GetScaledHeight(19);
            int itemsPerColumn = (itemPanel.DisplayRectangle.Height - textBoxStartY*2)/itemHeight;

            int labelX = labelStartX;
            int index = 0;
            while (index < unit.Models.Count)
            {
                int max = Math.Min(unit.Models.Count - index, itemsPerColumn) + index;

                int labelY = labelStartY;
                int maxLabelWidth = 0;
                int maxEditWidth = textBoxWidthBase;
                for (int i = index; i < max; i++)
                {
                    // ラベルを作成する
                    var label = new Label
                    {
                        Text = string.Format("{0}: {1}", i, unit.GetModelName(i)),
                        AutoSize = true,
                        Location = new Point(labelX, labelY)
                    };
                    itemPanel.Controls.Add(label);
                    maxLabelWidth = Math.Max(maxLabelWidth, label.Width);
                    labelY += itemHeight;

                    // テキストボックスの最大幅を求める
                    if (unit.ExistsModelName(i, country))
                    {
                        string s = unit.GetModelName(i, country);
                        maxEditWidth = Math.Max(maxEditWidth, (int) g.MeasureString(s, Font).Width + textBoxWidthMargin);
                    }
                }

                int textBoxX = labelX + maxLabelWidth + labelEditMargin;
                int textBoxY = textBoxStartY;
                for (int i = index; i < max; i++)
                {
                    // テキストボックスを作成する
                    var textBox = new TextBox
                    {
                        Size = new Size(maxEditWidth, textBoxHeight),
                        Location = new Point(textBoxX, textBoxY),
                        ForeColor = unit.Models[i].IsDirtyName(country) ? Color.Red : SystemColors.WindowText,
                        Tag = i
                    };
                    if (unit.ExistsModelName(i, country))
                    {
                        textBox.Text = unit.GetModelName(i, country);
                    }
                    textBox.Validated += OnItemTextBoxValidated;
                    itemPanel.Controls.Add(textBox);
                    textBoxY += itemHeight;
                }

                // 次の列へ移動する
                index += itemsPerColumn;
                labelX = textBoxX + maxEditWidth + columnMargin;
            }

            // スクロールバーの位置を調整するため最終列の次にダミーラベルを作成する
            itemPanel.Controls.Add(new Label {Location = new Point(labelX, labelStartY), AutoSize = true});
        }

        /// <summary>
        ///     編集項目テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // 選択中のユニット名種類がなければ戻る
            if (typeListBox.SelectedIndex < 0)
            {
                return;
            }
            UnitType type = Units.UnitTypes[typeListBox.SelectedIndex];
            Unit unit = Units.Items[(int) type];

            if (!(sender is TextBox))
            {
                return;
            }
            var textBox = sender as TextBox;
            var index = (int) textBox.Tag;

            if (unit.ExistsModelName(index, country))
            {
                // 値に変化がなければ何もしない
                if (textBox.Text.Equals(unit.GetModelName(index, country)))
                {
                    return;
                }
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    // 変更後の文字列が空ならば国別のモデル名を削除する
                    unit.RemoveModelName(index, country);
                }
                else
                {
                    // 値を更新する
                    unit.SetModelName(index, country, textBox.Text);
                }
            }
            else
            {
                // 値に変化がなければ何もしない
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    return;
                }
                // 値を更新する
                unit.SetModelName(index, country, textBox.Text);
            }

            // 編集済みフラグを設定する
            UnitModel model = unit.Models[index];
            model.SetDirtyName(country);
            Units.SetDirtyModelName(country, type);

            // 文字色を変更する
            textBox.ForeColor = Color.Red;

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();

            // ユニットモデル名の更新を通知する
            HoI2Editor.OnItemChanged(EditorItemId.CountryModelName, this);
        }

        #endregion
    }
}