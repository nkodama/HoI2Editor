using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    /// ユニット名エディタのフォーム
    /// </summary>
    public partial class UnitNameEditorForm : Form
    {
        #region 初期化

        /// <summary>
        /// コンストラクタ
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
            Country.Init();

            // ユニット名データを初期化する
            UnitNames.Init();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // 国家リストボックスを初期化する
            InitCountryListBox();

            // ユニット種類リストボックスを初期化する
            InitTypeListBox();

            // ユニット名定義ファイルを読み込む
            LoadFile();
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
        private void OnMinisterEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!UnitNames.IsDirty())
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
                    SaveFile();
                    break;
            }
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
            // ユニット名定義ファイルの再読み込みを要求する
            UnitNames.RequireReload();

            // ユニット名定義ファイルを読み込む
            LoadFile();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveFile();
        }

        /// <summary>
        ///     ユニット名定義ファイルを読み込む
        /// </summary>
        private void LoadFile()
        {
            // ユニット名定義ファイルを読み込む
            UnitNames.Load();

            // ユニット名リストの表示を更新する
            UpdateNameList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     ユニット名定義ファイルを保存する
        /// </summary>
        private void SaveFile()
        {
            // ユニット名定義ファイルを保存する
            UnitNames.Save();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Refresh();
        }

        #endregion

        #region 国家リストボックス

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string name in Country.Tags.Select(country => Country.Strings[(int)country]))
            {
                countryListBox.Items.Add(Config.GetText(name));
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
                CountryTag country = Country.Tags[e.Index];
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
        /// ユニット名リストを更新する
        /// </summary>
        void UpdateNameList()
        {
            nameTextBox.Clear();

            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndices.Count == 0)
            {
                return;
            }
            CountryTag country = Country.Tags[countryListBox.SelectedIndices[0]];

            // 選択中のユニット名種類がなければ戻る
            if (typeListBox.SelectedIndices.Count == 0)
            {
                return;
            }
            UnitNameType type = UnitNames.Types[typeListBox.SelectedIndices[0]];

            // ユニット名を順に追加する
            var sb = new StringBuilder();
            foreach (string name in UnitNames.GetNames(country, type))
            {
                sb.AppendLine(name);
            }

            nameTextBox.Text = sb.ToString();

            // 編集済みフラグを設定する
            UnitNames.SetDirty(country);
        }

        /// <summary>
        /// ユニット名リスト変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxValidated(object sender, EventArgs e)
        {
            // 選択中の国家がなければ戻る
            if (countryListBox.SelectedIndices.Count == 0)
            {
                return;
            }
            CountryTag country = Country.Tags[countryListBox.SelectedIndices[0]];

            // 選択中のユニット名種類がなければ戻る
            if (typeListBox.SelectedIndices.Count == 0)
            {
                return;
            }
            UnitNameType type = UnitNames.Types[typeListBox.SelectedIndices[0]];

            // ユニット名リストを更新する
            UnitNames.SetNames(country, type, nameTextBox.Lines.ToList());
        }

        #endregion

        #region 編集機能

        /// <summary>
        /// 元に戻すボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUndoButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Undo();
        }

        /// <summary>
        /// 切り取りボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Cut();
        }

        /// <summary>
        /// コピーボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Copy();
        }

        /// <summary>
        /// 貼り付けボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteButtonClick(object sender, EventArgs e)
        {
            nameTextBox.Paste();
        }

        /// <summary>
        /// 置換ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceButtonClick(object sender, EventArgs e)
        {
            if (regexcheckBox.Checked)
            {
                // 正規表現置換
                nameTextBox.Text = Regex.Replace(nameTextBox.Text, findComboBox.Text, replaceComboBox.Text,
                                                 RegexOptions.Multiline);
            }
            else
            {
                // 通常文字列置換
                nameTextBox.Text = Text.Replace(findComboBox.Text, replaceComboBox.Text);
            }
        }

        /// <summary>
        /// ユニット名を置換する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="type">ユニット名種類</param>
        void ReplaceUnitName(CountryTag country, UnitNameType type, string s, string t)
        {
            // ユニット名リストの文字列を順に置換する
            var names = UnitNames.GetNames(country, type).Select(name => name.Replace(s, t)).ToList();
            UnitNames.SetNames(country, type, names);

            // 編集済みフラグを設定する
            UnitNames.SetDirty(country);
        }

        #endregion
    }
}
