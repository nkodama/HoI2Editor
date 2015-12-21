using System;
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

            // フォームの初期化
            InitForm();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        public void OnFileLoaded()
        {
            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ユニット種類リストボックスを初期化する
            InitTypeListBox();
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
                    Log.Verbose("[ModelName] Changed unit name");
                    // ユニット種類リストボックスの表示項目を更新する
                    UpdateTypeListBox();
                    break;

                case EditorItemId.ModelList:
                    Log.Verbose("[ModelName] Changed model list");
                    // 編集項目の表示を更新する
                    UpdateEditableItems();
                    break;

                case EditorItemId.CommonModelName:
                    Log.Verbose("[ModelName] Changed common model name");
                    break;

                case EditorItemId.CountryModelName:
                    Log.Verbose("[ModelName] Changed country model name");
                    // 編集項目の表示を更新する
                    UpdateEditableItems();
                    break;
            }
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // 国家リストボックス
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);
            // ユニット種類リストボックス
            typeListBox.ItemHeight = DeviceCaps.GetScaledHeight(typeListBox.ItemHeight);

            // ウィンドウの位置
            Location = HoI2EditorController.Settings.ModelNameEditor.Location;
            Size = HoI2EditorController.Settings.ModelNameEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // ユニットデータを初期化する
            Units.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // ユニットデータを読み込む
            Units.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2EditorController.IsDirty())
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
                    HoI2EditorController.Save();
                    break;
                case DialogResult.No:
                    HoI2EditorController.SaveCanceled = true;
                    break;
            }
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2EditorController.OnModelNameEditorFormClosed();
        }

        /// <summary>
        ///     フォーム移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormMove(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2EditorController.Settings.ModelNameEditor.Location = Location;
            }
        }

        /// <summary>
        ///     フォームリサイズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                HoI2EditorController.Settings.ModelNameEditor.Size = Size;
            }
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            // 編集済みならば保存するかを問い合わせる
            if (HoI2EditorController.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2EditorController.Save();
                        break;
                }
            }

            HoI2EditorController.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2EditorController.Save();
        }

        /// <summary>
        ///     閉じるボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
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
                    ? $"{name} {Config.GetText(name)}"
                    : name))
            {
                countryListBox.Items.Add(s);
            }

            // 選択中の国家を反映する
            int index = HoI2EditorController.Settings.ModelNameEditor.Country;
            if ((index < 0) || (index >= countryListBox.Items.Count))
            {
                index = 0;
            }
            countryListBox.SelectedIndex = index;

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

            // 選択中の国家を保存する
            HoI2EditorController.Settings.ModelNameEditor.Country = countryListBox.SelectedIndex;
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
                UnitClass unit = Units.Items[(int) type];
                typeListBox.Items.Add(unit);
            }

            // 選択中のユニット種類を反映する
            int index = HoI2EditorController.Settings.ModelNameEditor.UnitType;
            if ((index < 0) || (index >= typeListBox.Items.Count))
            {
                index = 0;
            }
            typeListBox.SelectedIndex = index;

            typeListBox.EndUpdate();
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
                UnitClass unit = Units.Items[(int) type];
                typeListBox.Items[i] = unit;
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

            // 選択中のユニット種類を保存する
            HoI2EditorController.Settings.ModelNameEditor.UnitType = typeListBox.SelectedIndex;
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
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // 選択中のユニット名種類がなければ戻る
            UnitClass unit = typeListBox.SelectedItem as UnitClass;
            if (unit == null)
            {
                return;
            }

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
            int itemsPerColumn = (itemPanel.DisplayRectangle.Height - textBoxStartY * 2) / itemHeight;

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
                    Label label = new Label
                    {
                        Text = $"{i}: {unit.GetModelName(i)}",
                        AutoSize = true,
                        Location = new Point(labelX, labelY)
                    };
                    itemPanel.Controls.Add(label);
                    maxLabelWidth = Math.Max(maxLabelWidth, label.Width);
                    labelY += itemHeight;

                    // テキストボックスの最大幅を求める
                    if (unit.ExistsModelName(i, country))
                    {
                        string s = unit.GetCountryModelName(i, country);
                        maxEditWidth = Math.Max(maxEditWidth, (int) g.MeasureString(s, Font).Width + textBoxWidthMargin);
                    }
                }

                int textBoxX = labelX + maxLabelWidth + labelEditMargin;
                int textBoxY = textBoxStartY;
                for (int i = index; i < max; i++)
                {
                    // テキストボックスを作成する
                    TextBox textBox = new TextBox
                    {
                        Size = new Size(maxEditWidth, textBoxHeight),
                        Location = new Point(textBoxX, textBoxY),
                        ForeColor = unit.Models[i].IsDirtyName(country) ? Color.Red : SystemColors.WindowText,
                        Tag = i
                    };
                    if (unit.ExistsModelName(i, country))
                    {
                        textBox.Text = unit.GetCountryModelName(i, country);
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
            itemPanel.Controls.Add(new Label { Location = new Point(labelX, labelStartY), AutoSize = true });
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
            UnitClass unit = typeListBox.SelectedItem as UnitClass;
            if (unit == null)
            {
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            int index = (int) textBox.Tag;

            if (unit.ExistsModelName(index, country))
            {
                // 値に変化がなければ何もしない
                if (textBox.Text.Equals(unit.GetCountryModelName(index, country)))
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
            Units.SetDirtyModelName(country, unit.Type);

            // 文字色を変更する
            textBox.ForeColor = Color.Red;

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();

            // ユニットモデル名の更新を通知する
            HoI2EditorController.OnItemChanged(EditorItemId.CountryModelName, this);
        }

        #endregion
    }
}