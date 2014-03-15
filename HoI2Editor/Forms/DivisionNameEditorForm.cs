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
    ///     師団名エディタのフォーム
    /// </summary>
    public partial class DivisionNameEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     接頭辞の履歴
        /// </summary>
        private readonly History _prefixHistory = new History(HistorySize);

        /// <summary>
        ///     設備時の履歴
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
        public DivisionNameEditorForm()
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
            // 師団名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグがクリアされるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            // 何もしない
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // 兵科リストボックス
            branchListBox.ItemHeight = DeviceCaps.GetScaledHeight(branchListBox.ItemHeight);
            // 国家リストボックス
            countryListBox.ItemHeight = DeviceCaps.GetScaledHeight(countryListBox.ItemHeight);

            // ウィンドウの位置
            Location = HoI2Editor.Settings.DivisionNameEditor.Location;
            Size = HoI2Editor.Settings.DivisionNameEditor.Size;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionNameEditorFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 兵科リストボックスを初期化する
            InitBranchListBox();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // 履歴を初期化する
            InitHistory();

            // オプション設定を初期化する
            InitOption();

            // 師団名定義ファイルを読み込む
            DivisionNames.Load();

            // データ読み込み後の処理
            OnFileLoaded();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionNameEditorFormClosing(object sender, FormClosingEventArgs e)
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
        private void OnDivisionNameEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            HoI2Editor.OnDivisionNameEditorFormClosed();
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
                HoI2Editor.Settings.DivisionNameEditor.Location = Location;
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
                HoI2Editor.Settings.DivisionNameEditor.Size = Size;
            }
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

        #endregion

        #region 兵科リストボックス

        /// <summary>
        ///     兵科リストボックスを初期化する
        /// </summary>
        private void InitBranchListBox()
        {
            branchListBox.BeginUpdate();
            branchListBox.Items.Clear();
            branchListBox.Items.Add(Config.GetText("EYR_ARMY"));
            branchListBox.Items.Add(Config.GetText("EYR_NAVY"));
            branchListBox.Items.Add(Config.GetText("EYR_AIRFORCE"));

            // 選択中の兵科を反映する
            int index = HoI2Editor.Settings.DivisionNameEditor.Branch;
            if ((index < 0) || (index >= branchListBox.Items.Count))
            {
                index = 0;
            }
            branchListBox.SelectedIndex = index;

            branchListBox.EndUpdate();
        }

        /// <summary>
        ///     兵科リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchListBoxDrawItem(object sender, DrawItemEventArgs e)
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
                var branch = (Branch) (e.Index + 1);
                brush = DivisionNames.IsDirty(branch)
                    ? new SolidBrush(Color.Red)
                    : new SolidBrush(SystemColors.WindowText);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = branchListBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     兵科リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBranchListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 師団名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが変化するので国家リストボックスの表示を更新する
            countryListBox.Refresh();

            // 選択中の兵科を保存する
            HoI2Editor.Settings.DivisionNameEditor.Branch = branchListBox.SelectedIndex;
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
            int index = HoI2Editor.Settings.DivisionNameEditor.Country;
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
                var branch = (Branch) (branchListBox.SelectedIndex + 1);
                Country country = Countries.Tags[e.Index];
                brush = DivisionNames.IsDirty(branch, country)
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
            // 師団名リストの表示を更新する
            UpdateNameList();

            // 選択中の国家を保存する
            HoI2Editor.Settings.DivisionNameEditor.Country = countryListBox.SelectedIndex;
        }

        #endregion

        #region 師団名リスト

        /// <summary>
        ///     師団名リストを更新する
        /// </summary>
        private void UpdateNameList()
        {
            nameTextBox.Clear();

            // 選択中の兵科がなければ戻る
            if (branchListBox.SelectedIndex < 0)
            {
                return;
            }
            var branch = (Branch) (branchListBox.SelectedIndex + 1);

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // 師団名を順に追加する
            var sb = new StringBuilder();
            foreach (string name in DivisionNames.GetNames(branch, country))
            {
                sb.AppendLine(name);
            }

            nameTextBox.Text = sb.ToString();
        }

        /// <summary>
        ///     師団名リスト変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の兵科がなければ戻る
            if (branchListBox.SelectedIndex < 0)
            {
                return;
            }
            var branch = (Branch) (branchListBox.SelectedIndex + 1);

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            // 師団名リストを更新する
            DivisionNames.SetNames(nameTextBox.Lines.Where(line => !string.IsNullOrEmpty(line)).ToList(), branch,
                country);

            // 編集済みフラグが更新されるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();
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

            if (allBranchCheckBox.Checked)
            {
                if (allCountryCheckBox.Checked)
                {
                    // 全ての師団名を置換する
                    DivisionNames.ReplaceAll(to, with, regexCheckBox.Checked);
                }
                else
                {
                    // 国家リストボックスの選択項目がなければ戻る
                    if (countryListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    Country country = Countries.Tags[countryListBox.SelectedIndex];

                    // 全ての兵科の師団名を置換する
                    DivisionNames.ReplaceAllBranches(to, with, country, regexCheckBox.Checked);
                }
            }
            else
            {
                // 兵科リストボックスの選択項目がなければ戻る
                if (branchListBox.SelectedIndex < 0)
                {
                    return;
                }
                var branch = (Branch) (branchListBox.SelectedIndex + 1);

                if (allCountryCheckBox.Checked)
                {
                    // 全ての国の師団名を置換する
                    DivisionNames.ReplaceAllCountries(to, with, branch, regexCheckBox.Checked);
                }
                else
                {
                    // 国家リストボックスの選択項目がなければ戻る
                    if (countryListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    Country country = Countries.Tags[countryListBox.SelectedIndex];

                    // 師団名を置換する
                    DivisionNames.Replace(to, with, branch, country, regexCheckBox.Checked);
                }
            }

            // 師団名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();

            // 履歴を更新する
            _toHistory.Add(to);
            _withHistory.Add(with);

            HoI2Editor.Settings.DivisionNameEditor.ToHistory = _toHistory.Get().ToList();
            HoI2Editor.Settings.DivisionNameEditor.WithHistory = _withHistory.Get().ToList();

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
            // 兵科リストボックスの選択項目がなければ戻る
            if (branchListBox.SelectedIndex < 0)
            {
                return;
            }
            var branch = (Branch) (branchListBox.SelectedIndex + 1);

            // 国家リストボックスの選択項目がなければ戻る
            if (countryListBox.SelectedIndex < 0)
            {
                return;
            }
            Country country = Countries.Tags[countryListBox.SelectedIndex];

            string prefix = prefixComboBox.Text;
            string suffix = suffixComboBox.Text;

            // 師団名を一括追加する
            DivisionNames.AddSequential(prefix, suffix, (int) startNumericUpDown.Value, (int) endNumericUpDown.Value,
                branch, country);

            // 師団名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();

            // 履歴を更新する
            _prefixHistory.Add(prefix);
            _suffixHistory.Add(suffix);

            HoI2Editor.Settings.DivisionNameEditor.PrefixHistory = _prefixHistory.Get().ToList();
            HoI2Editor.Settings.DivisionNameEditor.SuffixHistory = _suffixHistory.Get().ToList();

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
            if (allBranchCheckBox.Checked)
            {
                if (allCountryCheckBox.Checked)
                {
                    // 全ての師団名を補間する
                    DivisionNames.InterpolateAll();
                }
                else
                {
                    // 国家リストボックスの選択項目がなければ戻る
                    if (countryListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    Country country = Countries.Tags[countryListBox.SelectedIndex];

                    // 全ての兵科の師団名を補間する
                    DivisionNames.InterpolateAllBranches(country);
                }
            }
            else
            {
                // 兵科リストボックスの選択項目がなければ戻る
                if (branchListBox.SelectedIndex < 0)
                {
                    return;
                }
                var branch = (Branch) (branchListBox.SelectedIndex + 1);

                if (allCountryCheckBox.Checked)
                {
                    // 全ての国の師団名を補間する
                    DivisionNames.InterpolateAllCountries(branch);
                }
                else
                {
                    // 国家リストボックスの選択項目がなければ戻る
                    if (countryListBox.SelectedIndex < 0)
                    {
                        return;
                    }
                    Country country = Countries.Tags[countryListBox.SelectedIndex];

                    // 師団名を補間する
                    DivisionNames.Interpolate(branch, country);
                }
            }

            // 師団名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグが更新されるため表示を更新する
            branchListBox.Refresh();
            countryListBox.Refresh();
        }

        /// <summary>
        ///     履歴の初期化
        /// </summary>
        private void InitHistory()
        {
            _toHistory.Set(HoI2Editor.Settings.DivisionNameEditor.ToHistory.ToArray());
            _withHistory.Set(HoI2Editor.Settings.DivisionNameEditor.WithHistory.ToArray());
            _prefixHistory.Set(HoI2Editor.Settings.DivisionNameEditor.PrefixHistory.ToArray());
            _suffixHistory.Set(HoI2Editor.Settings.DivisionNameEditor.SuffixHistory.ToArray());

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
            allBranchCheckBox.Checked = HoI2Editor.Settings.DivisionNameEditor.ApplyAllBranches;
            allCountryCheckBox.Checked = HoI2Editor.Settings.DivisionNameEditor.ApplyAllCountires;
            regexCheckBox.Checked = HoI2Editor.Settings.DivisionNameEditor.RegularExpression;
        }

        /// <summary>
        ///     全ての兵科に適用チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllBranchCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.DivisionNameEditor.ApplyAllBranches = allBranchCheckBox.Checked;
        }

        /// <summary>
        ///     全ての国家に適用チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllCountryCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.DivisionNameEditor.ApplyAllCountires = allCountryCheckBox.Checked;
        }

        /// <summary>
        ///     正規表現チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRegexCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            HoI2Editor.Settings.DivisionNameEditor.RegularExpression = regexCheckBox.Checked;
        }

        #endregion
    }
}