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
            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
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
                    Log.Verbose("[UnitName] Changed unit name");
                    // ユニット種類リストボックスの表示項目を更新する
                    UpdateTypeListBox();
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
            Location = HoI2Editor.Settings.UnitNameEditor.Location;
            Size = HoI2Editor.Settings.UnitNameEditor.Size;
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

            // ユニット名データを初期化する
            UnitNames.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ユニット種類リストボックスを初期化する
            InitTypeListBox();

            // 履歴を初期化する
            InitHistory();

            // オプション設定を初期化する
            InitOption();

            // ユニット名定義ファイルを読み込む
            UnitNames.Load();

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
                    HoI2Editor.Save();
                    break;
                case DialogResult.No:
                    HoI2Editor.SaveCanceled = true;
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
            HoI2Editor.OnUnitNameEditorFormClosed();
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
                HoI2Editor.Settings.UnitNameEditor.Location = Location;
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
                HoI2Editor.Settings.UnitNameEditor.Size = Size;
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
            if (HoI2Editor.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2Editor.Save();
                        break;
                }
            }

            HoI2Editor.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2Editor.Save();
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
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryListBox.Items.Add(s);
            }

            // 選択中の国家を反映する
            int index = HoI2Editor.Settings.UnitNameEditor.Country;
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

            // 編集済みフラグが変化するのでユニット種類リストボックスの表示を更新する
            typeListBox.Refresh();

            // 選択中の国家を保存する
            HoI2Editor.Settings.UnitNameEditor.Country = countryListBox.SelectedIndex;
        }

        #endregion

        #region ユニット種類リストボックス

        /// <summary>
        ///     ユニット種類リストボックスを初期化する
        /// </summary>
        private void InitTypeListBox()
        {
            typeListBox.BeginUpdate();
            typeListBox.Items.Clear();
            foreach (UnitNameType type in UnitNames.Types)
            {
                typeListBox.Items.Add(Config.GetText(UnitNames.TypeNames[(int) type]));
            }

            // 選択中のユニット種類を反映する
            int index = HoI2Editor.Settings.UnitNameEditor.UnitType;
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
            foreach (UnitNameType type in UnitNames.Types)
            {
                typeListBox.Items[i] = Config.GetText(UnitNames.TypeNames[(int) type]);
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

            // 選択中のユニット種類を保存する
            HoI2Editor.Settings.UnitNameEditor.UnitType = typeListBox.SelectedIndex;
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

            Log.Info("[UnitName] Replace: {0} -> {1}", to, with);

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
            _withHistory.Add(with);

            HoI2Editor.Settings.UnitNameEditor.ToHistory = _toHistory.Get().ToList();
            HoI2Editor.Settings.UnitNameEditor.WithHistory = _withHistory.Get().ToList();

            // 履歴コンボボックスを更新する
            UpdateReplaceHistory();
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
            var start = (int) startNumericUpDown.Value;
            var end = (int) endNumericUpDown.Value;

            Log.Info("[UnitName] Add: {0}-{1} {2} {3} [{4}] <{5}>", start, end, prefix, suffix,
                Config.GetText(UnitNames.TypeNames[(int) type]), Countries.Strings[(int) country]);

            // ユニット名を一括追加する
            UnitNames.AddSequential(prefix, suffix, start, end, country, type);

            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため国家リストボックスの表示を更新する
            countryListBox.Refresh();
            typeListBox.Refresh();

            // 履歴を更新する
            _prefixHistory.Add(prefix);
            _suffixHistory.Add(suffix);

            HoI2Editor.Settings.UnitNameEditor.PrefixHistory = _prefixHistory.Get().ToList();
            HoI2Editor.Settings.UnitNameEditor.SuffixHistory = _suffixHistory.Get().ToList();

            // 履歴コンボボックスを更新する
            UpdateAddHistory();
        }

        /// <summary>
        ///     補間ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInterpolateButtonClick(object sender, EventArgs e)
        {
            Log.Info("[UnitName] Interpolate");

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

        /// <summary>
        ///     履歴の初期化
        /// </summary>
        private void InitHistory()
        {
            _toHistory.Set(HoI2Editor.Settings.UnitNameEditor.ToHistory.ToArray());
            _withHistory.Set(HoI2Editor.Settings.UnitNameEditor.WithHistory.ToArray());
            _prefixHistory.Set(HoI2Editor.Settings.UnitNameEditor.PrefixHistory.ToArray());
            _suffixHistory.Set(HoI2Editor.Settings.UnitNameEditor.SuffixHistory.ToArray());

            UpdateReplaceHistory();
            UpdateAddHistory();

            if (toComboBox.Items.Count > 0)
            {
                toComboBox.SelectedIndex = 0;
            }

            if (withComboBox.Items.Count > 0)
            {
                withComboBox.SelectedIndex = 0;
            }

            if (prefixComboBox.Items.Count > 0)
            {
                prefixComboBox.SelectedIndex = 0;
            }

            if (suffixComboBox.Items.Count > 0)
            {
                suffixComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///     置換履歴コンボボックスを更新する
        /// </summary>
        private void UpdateReplaceHistory()
        {
            toComboBox.Items.Clear();
            foreach (string s in _toHistory.Get())
            {
                toComboBox.Items.Add(s);
            }

            withComboBox.Items.Clear();
            foreach (string s in _withHistory.Get())
            {
                withComboBox.Items.Add(s);
            }
        }

        /// <summary>
        ///     追加履歴コンボボックスを更新する
        /// </summary>
        private void UpdateAddHistory()
        {
            prefixComboBox.Items.Clear();
            foreach (string s in _prefixHistory.Get())
            {
                prefixComboBox.Items.Add(s);
            }

            suffixComboBox.Items.Clear();
            foreach (string s in _suffixHistory.Get())
            {
                suffixComboBox.Items.Add(s);
            }
        }

        /// <summary>
        ///     オプション設定を初期化する
        /// </summary>
        private void InitOption()
        {
            allCountryCheckBox.Checked = HoI2Editor.Settings.UnitNameEditor.ApplyAllCountires;
            allUnitTypeCheckBox.Checked = HoI2Editor.Settings.UnitNameEditor.ApplyAllUnitTypes;
            regexCheckBox.Checked = HoI2Editor.Settings.UnitNameEditor.RegularExpression;
        }

        /// <summary>
        ///     全ての国家に適用チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllCountryCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.UnitNameEditor.ApplyAllCountires = allCountryCheckBox.Checked;
        }

        /// <summary>
        ///     全てのユニット種に適用チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllUnitTypeCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.UnitNameEditor.ApplyAllUnitTypes = allUnitTypeCheckBox.Checked;
        }

        /// <summary>
        ///     正規表現チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegexCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.UnitNameEditor.RegularExpression = regexCheckBox.Checked;
        }

        #endregion
    }
}