using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     ユニット名エディタのフォーム
    /// </summary>
    public partial class UnitNameEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     接頭辞の履歴
        /// </summary>
        private readonly History _prefixHistory = new History(HistorySize);

        /// <summary>
        ///     接尾辞の履歴
        /// </summary>
        private readonly History _suffixHistory = new History(HistorySize);

        /// <summary>
        ///     置換元の履歴
        /// </summary>
        private readonly History _toHistory = new History(HistorySize);

        /// <summary>
        ///     置換先の履歴
        /// </summary>
        private readonly History _withHistory = new History(HistorySize);

        #endregion

        #region 内部定数

        /// <summary>
        ///     履歴の最大数
        /// </summary>
        private const int HistorySize = 10;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public UnitNameEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitNameEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // ユニット名データを初期化する
            UnitNames.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ユニット種類リストボックスを初期化する
            InitTypeListBox();

            // ユニット名定義ファイルを読み込む
            UnitNames.Load();

            // データ読み込み後の処理
            OnUnitNamesLoaded();
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
        private void OnUnitNameEditorFormClosing(object sender, FormClosingEventArgs e)
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
        private void OnUnitNameEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnUnitNameEditorFormClosed();
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
        public void OnUnitNamesLoaded()
        {
            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnUnitNamesSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryListBox.Items.Add(s);
            }
            countryListBox.SelectedIndex = 0;
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
                brush = UnitNames.IsDirty(country) ? new SolidBrush(Color.Red) : new SolidBrush(SystemColors.WindowText);
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
            // ユニット名リストの表示を更新する
            UpdateNameList();
        }

        #endregion

        #region ユニット種類リストボックス

        /// <summary>
        ///     ユニット種類リストボックスを初期化する
        /// </summary>
        private void InitTypeListBox()
        {
            foreach (UnitNameType type in UnitNames.Types)
            {
                typeListBox.Items.Add(Config.GetText(UnitNames.TypeNames[(int) type]));
            }
            typeListBox.SelectedIndex = 0;
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
                UnitNameType type = UnitNames.Types[e.Index];
                brush = UnitNames.IsDirty(country, type)
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
            // ユニット名リストの表示を更新する
            UpdateNameList();
        }

        #endregion

        #region ユニット名リスト

        /// <summary>
        ///     ユニット名リストを更新する
        /// </summary>
        private void UpdateNameList()
        {
            nameTextBox.Clear();

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
            UnitNameType type = UnitNames.Types[typeListBox.SelectedIndex];

            // ユニット名を順に追加する
            var sb = new StringBuilder();
            foreach (string name in UnitNames.GetNames(country, type))
            {
                sb.AppendLine(name);
            }

            nameTextBox.Text = sb.ToString();
        }

        /// <summary>
        ///     ユニット名リスト変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxValidated(object sender, EventArgs e)
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
            UnitNameType type = UnitNames.Types[typeListBox.SelectedIndex];

            // ユニット名リストを更新する
            UnitNames.SetNames(nameTextBox.Lines.Where(line => !string.IsNullOrEmpty(line)).ToList(), country, type);

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        #endregion

        #region 編集機能

        /// <summary>
        ///     元に戻すボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUndoButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Undo();
        }

        /// <summary>
        ///     切り取りボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Cut();
        }

        /// <summary>
        ///     コピーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Copy();
        }

        /// <summary>
        ///     貼り付けボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Paste();
        }

        /// <summary>
        ///     置換ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceButtonClick(object sender, EventArgs e)
        {
            string to = toComboBox.Text;
            string with = withComboBox.Text;

            if (allCountryCheckBox.Checked)
            {
                if (allUnitTypeCheckBox.Checked)
                {
                    // 全てのユニット名を置換する
                    UnitNames.ReplaceAll(to, with, regexCheckBox.Checked);
                }
                else
                {
                    // ユニット名種類リストボックスの選択項目がなければ戻る
                    if (typeListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    // 全ての国のユニット名を置換する
                    UnitNames.ReplaceAllCountries(to, with,
                        UnitNames.Types[typeListBox.SelectedIndex], regexCheckBox.Checked);
                }
            }
            else
            {
                // 国家リストボックスの選択項目がなければ戻る
                if (countryListBox.SelectedIndex < 0)
                {
                    return;
                }
                if (allUnitTypeCheckBox.Checked)
                {
                    // 全てのユニット名種類のユニット名を置換する
                    UnitNames.ReplaceAllTypes(to, with,
                        Countries.Tags[countryListBox.SelectedIndex], regexCheckBox.Checked);
                }
                else
                {
                    // ユニット名種類リストボックスの選択項目がなければ戻る
                    if (typeListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    // ユニット名を置換する
                    UnitNames.Replace(to, with, Countries.Tags[countryListBox.SelectedIndex],
                        UnitNames.Types[typeListBox.SelectedIndex], regexCheckBox.Checked);
                }
            }

            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();

            // 履歴を更新する
            _toHistory.Add(to);
            toComboBox.Items.Clear();
            foreach (string s in _toHistory.Get())
            {
                toComboBox.Items.Add(s);
            }
            _withHistory.Add(with);
            withComboBox.Items.Clear();
            foreach (string s in _withHistory.Get())
            {
                withComboBox.Items.Add(s);
            }
        }

        /// <summary>
        ///     追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddButtonClick(object sender, EventArgs e)
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
            UnitNameType type = UnitNames.Types[typeListBox.SelectedIndex];

            string prefix = prefixComboBox.Text;
            string suffix = suffixComboBox.Text;

            // ユニット名を一括追加する
            UnitNames.AddSequential(prefix, suffix, (int) startNumericUpDown.Value, (int) endNumericUpDown.Value,
                country, type);

            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();

            // 履歴を更新する
            _prefixHistory.Add(prefix);
            prefixComboBox.Items.Clear();
            foreach (string s in _prefixHistory.Get())
            {
                prefixComboBox.Items.Add(s);
            }
            _suffixHistory.Add(suffix);
            suffixComboBox.Items.Clear();
            foreach (string s in _suffixHistory.Get())
            {
                suffixComboBox.Items.Add(s);
            }
        }

        /// <summary>
        ///     補間ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInterpolateButtonClick(object sender, EventArgs e)
        {
            if (allCountryCheckBox.Checked)
            {
                if (allUnitTypeCheckBox.Checked)
                {
                    // 全てのユニット名を補間する
                    UnitNames.InterpolateAll();
                }
                else
                {
                    // ユニット名種類リストボックスの選択項目がなければ戻る
                    if (typeListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    // 全ての国のユニット名を補間する
                    UnitNames.InterpolateAllCountries(UnitNames.Types[typeListBox.SelectedIndex]);
                }
            }
            else
            {
                // 国家リストボックスの選択項目がなければ戻る
                if (countryListBox.SelectedIndex < 0)
                {
                    return;
                }
                if (allUnitTypeCheckBox.Checked)
                {
                    // 全てのユニット名種類のユニット名を補間する
                    UnitNames.InterpolateAllTypes(Countries.Tags[countryListBox.SelectedIndex]);
                }
                else
                {
                    // ユニット名種類リストボックスの選択項目がなければ戻る
                    if (typeListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    // ユニット名を補間する
                    UnitNames.Interpolate(Countries.Tags[countryListBox.SelectedIndex],
                        UnitNames.Types[typeListBox.SelectedIndex]);
                }
            }

            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        #endregion
    }
}