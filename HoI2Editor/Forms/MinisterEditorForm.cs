using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     閣僚エディタのフォーム
    /// </summary>
    public partial class MinisterEditorForm : Form
    {
        /// <summary>
        ///     絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _narrowedList = new List<Minister>();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MinisterEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     閣僚ファイルを読み込む
        /// </summary>
        private void LoadMinisterFiles()
        {
            // 閣僚ファイルを読み込む
            Ministers.LoadMinisterFiles();

            // 閣僚リストを絞り込む
            NarrowMinisterList();

            // 閣僚リストの表示を更新する
            UpdateMinisterList();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     閣僚ファイルを保存する
        /// </summary>
        private void SaveMinisterFiles()
        {
            // 閣僚ファイルを保存する
            Ministers.SaveMinisterFiles();

            // 編集済みフラグがクリアされるため表示を更新する
            countryListBox.Update();
        }

        /// <summary>
        ///     閣僚リストを国タグで絞り込む
        /// </summary>
        private void NarrowMinisterList()
        {
            _narrowedList.Clear();
            List<CountryTag> selectedTagList = countryListBox.SelectedItems.Count == 0
                                                   ? new List<CountryTag>()
                                                   : (from string country in countryListBox.SelectedItems
                                                      select Country.CountryStringMap[country]).ToList();

            foreach (
                Minister minister in Ministers.List.Where(minister => selectedTagList.Contains(minister.CountryTag)))
            {
                _narrowedList.Add(minister);
            }
        }

        /// <summary>
        ///     閣僚リストの表示を更新する
        /// </summary>
        private void UpdateMinisterList()
        {
            ministerListView.BeginUpdate();
            ministerListView.Items.Clear();

            foreach (Minister minister in _narrowedList)
            {
                ministerListView.Items.Add(CreateMinisterListViewItem(minister));
            }

            if (ministerListView.Items.Count > 0)
            {
                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;
                EnableEditableItems();
            }
            else
            {
                DisableEditableItems();
            }

            ministerListView.EndUpdate();
        }

        /// <summary>
        ///     国家コンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateCountryComboBox(Minister minister)
        {
            countryComboBox.BeginUpdate();

            if (minister.CountryTag != CountryTag.None)
            {
                if (string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.RemoveAt(0);
                }
                countryComboBox.SelectedIndex = (int) (minister.CountryTag - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(countryComboBox.Items[0].ToString()))
                {
                    countryComboBox.Items.Insert(0, "");
                }
                countryComboBox.SelectedIndex = 0;
            }

            countryComboBox.EndUpdate();
        }

        /// <summary>
        ///     閣僚地位コンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdatePositionComboBox(Minister minister)
        {
            positionComboBox.BeginUpdate();

            if (minister.Position != MinisterPosition.None)
            {
                if (string.IsNullOrEmpty(positionComboBox.Items[0].ToString()))
                {
                    positionComboBox.Items.RemoveAt(0);
                }
                positionComboBox.SelectedIndex = (int) (minister.Position - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(positionComboBox.Items[0].ToString()))
                {
                    positionComboBox.Items.Insert(0, "");
                }
                positionComboBox.SelectedIndex = 0;
            }

            positionComboBox.EndUpdate();
        }

        /// <summary>
        ///     閣僚特性コンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdatePersonalityComboBox(Minister minister)
        {
            personalityComboBox.BeginUpdate();
            personalityComboBox.Items.Clear();

            // 閣僚地位の値が不正な場合は、現在の閣僚特性のみ登録する
            if (minister.Position == MinisterPosition.None)
            {
                personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
                personalityComboBox.SelectedIndex = 0;
            }
                // 閣僚地位とマッチしない閣僚特性の場合、ワンショットで候補に登録する
            else if (!Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
            {
                personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[personality].Name));
                }
                personalityComboBox.SelectedIndex = 0;
            }
            else
            {
                foreach (int personality in Ministers.PositionPersonalityTable[(int) minister.Position])
                {
                    personalityComboBox.Items.Add(Config.GetText(Ministers.PersonalityTable[personality].Name));
                    if (personality == minister.Personality)
                    {
                        personalityComboBox.SelectedIndex = personalityComboBox.Items.Count - 1;
                    }
                }
            }
            personalityComboBox.EndUpdate();
        }

        /// <summary>
        ///     閣僚イデオロギーコンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateIdeologyComboBox(Minister minister)
        {
            ideologyComboBox.BeginUpdate();

            if (minister.Ideology != MinisterIdeology.None)
            {
                if (string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString()))
                {
                    ideologyComboBox.Items.RemoveAt(0);
                }
                ideologyComboBox.SelectedIndex = (int) (minister.Ideology - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString()))
                {
                    ideologyComboBox.Items.Insert(0, "");
                }
                ideologyComboBox.SelectedIndex = 0;
            }

            ideologyComboBox.EndUpdate();
        }

        /// <summary>
        ///     閣僚忠誠度コンボボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateLoyaltyComboBox(Minister minister)
        {
            loyaltyComboBox.BeginUpdate();

            if (minister.Loyalty != MinisterLoyalty.None)
            {
                if (string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString()))
                {
                    loyaltyComboBox.Items.RemoveAt(0);
                }
                loyaltyComboBox.SelectedIndex = (int) (minister.Loyalty - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString()))
                {
                    loyaltyComboBox.Items.Insert(0, "");
                }
                loyaltyComboBox.SelectedIndex = 0;
            }

            loyaltyComboBox.EndUpdate();
        }

        /// <summary>
        ///     閣僚画像ピクチャーボックスの項目を更新する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        private void UpdateMinisterPicture(Minister minister)
        {
            if (!string.IsNullOrEmpty(minister.PictureName))
            {
                string fileName =
                    Game.GetFileName(Path.Combine(Game.PersonPicturePathName,
                                                  Path.ChangeExtension(minister.PictureName, ".bmp")));
                ministerPictureBox.ImageLocation = File.Exists(fileName) ? fileName : "";
            }
            else
            {
                ministerPictureBox.ImageLocation = "";
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む
        /// </summary>
        private void LoadPersonality()
        {
            // 閣僚特性を読み込む
            Ministers.LoadMinisterPersonality();
        }

        /// <summary>
        ///     編集項目を初期化する
        /// </summary>
        private void InitEditableItems()
        {
            // 国タグ
            int maxSize = countryComboBox.DropDownWidth;
            foreach (string s in Country.CountryTextTable.Select(
                country =>
                Config.ExistsKey(country) ? string.Format("{0} {1}", country, Config.GetText(country)) : country))
            {
                countryComboBox.Items.Add(s);
                maxSize = Math.Max(maxSize,
                                   TextRenderer.MeasureText(s, countryComboBox.Font).Width +
                                   SystemInformation.VerticalScrollBarWidth);
            }
            countryComboBox.DropDownWidth = maxSize;

            // 地位
            maxSize = positionComboBox.DropDownWidth;
            foreach (string name in Ministers.PositionTable.Select(info => Config.GetText(info.Name)))
            {
                positionComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, positionComboBox.Font).Width);
            }
            positionComboBox.DropDownWidth = maxSize;

            // 特性
            personalityComboBox.DropDownWidth =
                Ministers.PersonalityTable.Select(info => Config.GetText(info.Name))
                         .Select(name => TextRenderer.MeasureText(name, personalityComboBox.Font).Width)
                         .Concat(new[] {personalityComboBox.DropDownWidth})
                         .Max();

            // イデオロギー
            maxSize = ideologyComboBox.DropDownWidth;
            foreach (string name in Ministers.IdeologyTable.Select(info => Config.GetText(info.Name)))
            {
                ideologyComboBox.Items.Add(Config.GetText(name));
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, ideologyComboBox.Font).Width);
            }
            ideologyComboBox.DropDownWidth = maxSize;

            // 忠誠度
            maxSize = loyaltyComboBox.DropDownWidth;
            foreach (string name in Ministers.LoyaltyTable.Select(info => info.Name))
            {
                loyaltyComboBox.Items.Add(name);
                maxSize = Math.Max(maxSize, TextRenderer.MeasureText(name, loyaltyComboBox.Font).Width);
            }
            loyaltyComboBox.DropDownWidth = maxSize;
        }

        /// <summary>
        ///     国家リストボックスを初期化する
        /// </summary>
        private void InitCountryListBox()
        {
            foreach (string country in Country.CountryTextTable.Where(country => !string.IsNullOrEmpty(country)))
            {
                countryListBox.Items.Add(country);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     閣僚リストに項目を追加する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        private void AddListItem(Minister target)
        {
            _narrowedList.Add(target);

            ministerListView.Items.Add(CreateMinisterListViewItem(target));

            ministerListView.Items[0].Focused = true;
            ministerListView.Items[0].Selected = true;
        }

        /// <summary>
        ///     閣僚リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="index">挿入先の位置</param>
        private void InsertListItem(Minister target, int index)
        {
            _narrowedList.Insert(index, target);

            ministerListView.Items.Insert(index, CreateMinisterListViewItem(target));

            ministerListView.Items[index].Focused = true;
            ministerListView.Items[index].Selected = true;
            ministerListView.Items[index].EnsureVisible();
        }

        /// <summary>
        ///     閣僚リストから項目を削除する
        /// </summary>
        /// <param name="index">削除対象の位置</param>
        private void RemoveItem(int index)
        {
            _narrowedList.RemoveAt(index);

            ministerListView.Items.RemoveAt(index);

            if (index < ministerListView.Items.Count)
            {
                ministerListView.Items[index].Focused = true;
                ministerListView.Items[index].Selected = true;
            }
            else if (index - 1 >= 0)
            {
                ministerListView.Items[index - 1].Focused = true;
                ministerListView.Items[index - 1].Selected = true;
            }
        }

        /// <summary>
        ///     閣僚リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の位置</param>
        /// <param name="position">移動先の位置</param>
        private void MoveListItem(int target, int position)
        {
            Minister minister = _narrowedList[target];

            if (target > position)
            {
                // 上へ移動する場合
                _narrowedList.Insert(position, minister);
                _narrowedList.RemoveAt(target + 1);

                ministerListView.Items.Insert(position, CreateMinisterListViewItem(minister));
                ministerListView.Items.RemoveAt(target + 1);
            }
            else
            {
                // 下へ移動する場合
                _narrowedList.Insert(position + 1, minister);
                _narrowedList.RemoveAt(target);

                ministerListView.Items.Insert(position + 1, CreateMinisterListViewItem(minister));
                ministerListView.Items.RemoveAt(target);
            }

            ministerListView.Items[position].Focused = true;
            ministerListView.Items[position].Selected = true;
            ministerListView.Items[position].EnsureVisible();
        }

        /// <summary>
        ///     閣僚リストビューの項目を作成する
        /// </summary>
        /// <param name="minister">閣僚データ</param>
        /// <returns>閣僚リストビューの項目</returns>
        private ListViewItem CreateMinisterListViewItem(Minister minister)
        {
            if (minister == null)
            {
                return null;
            }

            var item = new ListViewItem
                           {
                               Text = Country.CountryTextTable[(int) minister.CountryTag],
                               Tag = minister
                           };
            item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(minister.Name);
            item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(Misc.Mod.NewMinisterFormat ? minister.EndYear.ToString(CultureInfo.InvariantCulture) : "");
            item.SubItems.Add(Config.GetText(Ministers.PositionTable[(int) minister.Position].Name));
            item.SubItems.Add(Config.GetText(Ministers.PersonalityTable[minister.Personality].Name));
            item.SubItems.Add(Config.GetText(Ministers.IdeologyTable[(int) minister.Ideology].Name));

            return item;
        }

        /// <summary>
        ///     編集可能な項目を有効化する
        /// </summary>
        private void EnableEditableItems()
        {
            countryComboBox.Enabled = true;
            idNumericUpDown.Enabled = true;
            nameTextBox.Enabled = true;
            startYearNumericUpDown.Enabled = true;
            endYearNumericUpDown.Enabled = Misc.Mod.NewMinisterFormat;
            retirementYearNumericUpDown.Enabled = Misc.Mod.RetirementYearMinister;
            positionComboBox.Enabled = true;
            personalityComboBox.Enabled = true;
            ideologyComboBox.Enabled = true;
            loyaltyComboBox.Enabled = true;
            pictureNameTextBox.Enabled = true;
            pictureNameReferButton.Enabled = true;

            cloneButton.Enabled = true;
            removeButton.Enabled = true;
            topButton.Enabled = true;
            upButton.Enabled = true;
            downButton.Enabled = true;
            bottomButton.Enabled = true;
        }

        /// <summary>
        ///     編集可能な項目を無効化する
        /// </summary>
        private void DisableEditableItems()
        {
            countryComboBox.Text = "";
            idNumericUpDown.Value = 0;
            nameTextBox.Text = "";
            startYearNumericUpDown.Value = 1936;
            endYearNumericUpDown.Value = 1970;
            retirementYearNumericUpDown.Value = 1999;
            pictureNameTextBox.Text = "";
            ministerPictureBox.ImageLocation = "";

            countryComboBox.Enabled = false;
            idNumericUpDown.Enabled = false;
            nameTextBox.Enabled = false;
            startYearNumericUpDown.Enabled = false;
            endYearNumericUpDown.Enabled = false;
            retirementYearNumericUpDown.Enabled = false;
            positionComboBox.Enabled = false;
            personalityComboBox.Enabled = false;
            ideologyComboBox.Enabled = false;
            loyaltyComboBox.Enabled = false;
            pictureNameTextBox.Enabled = false;
            pictureNameReferButton.Enabled = false;

            cloneButton.Enabled = false;
            removeButton.Enabled = false;
            topButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            bottomButton.Enabled = false;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterEditorFormLoad(object sender, EventArgs e)
        {
            LoadPersonality();
            InitEditableItems();
            InitCountryListBox();
            LoadMinisterFiles();
        }

        /// <summary>
        ///     新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewButtonClick(object sender, EventArgs e)
        {
            Minister minister;
            if (ministerListView.SelectedItems.Count > 0)
            {
                var selected = ministerListView.SelectedItems[0].Tag as Minister;
                if (selected == null)
                {
                    return;
                }

                minister = new Minister
                               {
                                   CountryTag = selected.CountryTag,
                                   Id = selected.Id + 1,
                                   StartYear = 1936,
                                   EndYear = 1970,
                                   RetirementYear = 1999,
                                   Position = MinisterPosition.None,
                                   Personality = 0,
                                   Ideology = MinisterIdeology.None,
                                   Loyalty = MinisterLoyalty.None,
                               };

                Ministers.InsertItemNext(minister, selected);

                InsertListItem(minister, ministerListView.SelectedIndices[0] + 1);
            }
            else
            {
                minister = new Minister
                               {
                                   CountryTag =
                                       countryListBox.SelectedItems.Count > 0
                                           ? (CountryTag) (countryListBox.SelectedIndex + 1)
                                           : CountryTag.None,
                                   Id = 0,
                                   StartYear = 1930,
                                   EndYear = 1970,
                                   RetirementYear = 1999,
                                   Position = MinisterPosition.None,
                                   Personality = 0,
                                   Ideology = MinisterIdeology.None,
                                   Loyalty = MinisterLoyalty.None,
                               };

                Ministers.AddItem(minister);

                AddListItem(minister);

                EnableEditableItems();
            }

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     複製ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloneButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = ministerListView.SelectedItems[0].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            var minister = new Minister
                               {
                                   CountryTag = selected.CountryTag,
                                   Id = selected.Id + 1,
                                   Name = selected.Name,
                                   StartYear = selected.StartYear,
                                   EndYear = selected.EndYear,
                                   RetirementYear = selected.RetirementYear,
                                   Position = selected.Position,
                                   Personality = selected.Personality,
                                   Ideology = selected.Ideology,
                                   Loyalty = selected.Loyalty,
                                   PictureName = selected.PictureName
                               };

            Ministers.InsertItemNext(minister, selected);

            InsertListItem(minister, ministerListView.SelectedIndices[0] + 1);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selected = ministerListView.SelectedItems[0].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            Ministers.RemoveItem(selected);

            RemoveItem(ministerListView.SelectedIndices[0]);

            if (ministerListView.Items.Count == 0)
            {
                DisableEditableItems();
            }

            Ministers.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     先頭へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTopButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            int index = ministerListView.SelectedIndices[0];
            // 選択項目がリストの先頭ならば何もしない
            if (ministerListView.SelectedIndices[0] == 0)
            {
                return;
            }

            var selected = ministerListView.SelectedItems[0].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            var top = ministerListView.Items[0].Tag as Minister;
            if (top == null)
            {
                return;
            }

            Ministers.MoveItem(selected, top);

            MoveListItem(index, 0);

            Ministers.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの先頭ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            if (index == 0)
            {
                return;
            }

            var selected = ministerListView.SelectedItems[0].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            var upper = ministerListView.Items[index - 1].Tag as Minister;
            if (upper == null)
            {
                return;
            }

            Ministers.MoveItem(selected, upper);

            MoveListItem(index, index - 1);

            Ministers.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            if (index == ministerListView.Items.Count - 1)
            {
                return;
            }

            var selected = ministerListView.SelectedItems[0].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            var lower = ministerListView.Items[index + 1].Tag as Minister;
            if (lower == null)
            {
                return;
            }

            Ministers.MoveItem(selected, lower);

            MoveListItem(index, index + 1);

            Ministers.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     末尾へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBottomButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            // 選択項目がリストの末尾ならば何もしない
            int index = ministerListView.SelectedIndices[0];
            int bottomIndex = ministerListView.Items.Count - 1;
            if (ministerListView.SelectedIndices[0] == bottomIndex)
            {
                return;
            }

            var selected = ministerListView.Items[index].Tag as Minister;
            if (selected == null)
            {
                return;
            }

            var bottom = ministerListView.Items[ministerListView.Items.Count - 1].Tag as Minister;
            if (bottom == null)
            {
                return;
            }

            Ministers.MoveItem(selected, bottom);

            MoveListItem(index, ministerListView.Items.Count - 1);

            Ministers.SetDirtyFlag(selected.CountryTag);
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 背景を描画する
            e.DrawBackground();

            // 選択項目がない場合はスキップ
            if (e.Index != -1)
            {
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
                {
                    // 変更ありの項目は文字色を変更する
                    brush = Ministers.DirtyFlags[e.Index + 1]
                                ? new SolidBrush(Color.Red)
                                : new SolidBrush(SystemColors.WindowText);
                }
                else
                {
                    brush = new SolidBrush(SystemColors.HighlightText);
                }
                var listbox = sender as ListBox;
                if (listbox != null)
                {
                    string s = listbox.Items[e.Index].ToString();
                    e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
                }
                brush.Dispose();
            }

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
            countryAllButton.Text = countryListBox.SelectedItems.Count <= 1
                                        ? Resources.KeySelectAll
                                        : Resources.KeyUnselectAll;
            newButton.Enabled = countryListBox.SelectedItems.Count > 0;

            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        ///     閣僚リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            UpdateCountryComboBox(minister);
            idNumericUpDown.Value = minister.Id;
            nameTextBox.Text = minister.Name;
            startYearNumericUpDown.Value = minister.StartYear;
            endYearNumericUpDown.Value = minister.EndYear;
            retirementYearNumericUpDown.Value = minister.RetirementYear;
            UpdatePositionComboBox(minister);
            UpdatePersonalityComboBox(minister);
            UpdateIdeologyComboBox(minister);
            UpdateLoyaltyComboBox(minister);
            pictureNameTextBox.Text = minister.PictureName;
            UpdateMinisterPicture(minister);
        }

        /// <summary>
        ///     国タグ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            CountryTag newCountryTag = !string.IsNullOrEmpty(countryComboBox.Items[0].ToString())
                                           ? (CountryTag) (countryComboBox.SelectedIndex + 1)
                                           : (CountryTag) countryComboBox.SelectedIndex;
            if (newCountryTag == minister.CountryTag)
            {
                return;
            }

            Ministers.SetDirtyFlag(minister.CountryTag);

            minister.CountryTag = newCountryTag;
            ministerListView.SelectedItems[0].Text = Country.CountryTextTable[(int) minister.CountryTag];

            UpdateCountryComboBox(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);

            // 国家リストボックスの項目色を変更するため描画更新する
            countryListBox.Refresh();
        }

        /// <summary>
        ///     ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newId = (int) idNumericUpDown.Value;
            if (newId == minister.Id)
            {
                return;
            }

            minister.Id = newId;
            ministerListView.SelectedItems[0].SubItems[1].Text = minister.Id.ToString(CultureInfo.InvariantCulture);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newName = nameTextBox.Text;
            if (newName.Equals(minister.Name))
            {
                return;
            }

            minister.Name = newName;
            ministerListView.SelectedItems[0].SubItems[2].Text = minister.Name;

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newStartYear = (int) startYearNumericUpDown.Value;
            if (newStartYear == minister.StartYear)
            {
                return;
            }

            minister.StartYear = newStartYear;
            ministerListView.SelectedItems[0].SubItems[3].Text =
                minister.StartYear.ToString(CultureInfo.InvariantCulture);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     終了年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newEndYear = (int) endYearNumericUpDown.Value;
            if (newEndYear == minister.EndYear)
            {
                return;
            }

            minister.EndYear = newEndYear;
            ministerListView.SelectedItems[0].SubItems[4].Text =
                minister.EndYear.ToString(CultureInfo.InvariantCulture);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     引退年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            var newRetirementYear = (int) retirementYearNumericUpDown.Value;
            if (newRetirementYear == minister.RetirementYear)
            {
                return;
            }

            minister.RetirementYear = newRetirementYear;

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     閣僚地位変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterPosition newPosition = !string.IsNullOrEmpty(positionComboBox.Items[0].ToString())
                                               ? (MinisterPosition) (positionComboBox.SelectedIndex + 1)
                                               : (MinisterPosition) positionComboBox.SelectedIndex;
            if (newPosition == minister.Position)
            {
                return;
            }

            minister.Position = newPosition;
            ministerListView.SelectedItems[0].SubItems[5].Text =
                Config.GetText(Ministers.PositionTable[(int) minister.Position].Name);

            UpdatePositionComboBox(minister);
            // 地位に連動して特性の選択肢も変更する
            UpdatePersonalityComboBox(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     閣僚特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPersonalityComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 閣僚地位が不定値の時には変更不可
            if (minister.Position == MinisterPosition.None)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            int newPersonality;
            if (Ministers.PositionPersonalityTable[(int) minister.Position].Contains(minister.Personality))
            {
                newPersonality =
                    Ministers.PositionPersonalityTable[(int) minister.Position][personalityComboBox.SelectedIndex];
            }
            else
            {
                if (personalityComboBox.SelectedIndex == 0)
                {
                    return;
                }
                newPersonality =
                    Ministers.PositionPersonalityTable[(int) minister.Position][personalityComboBox.SelectedIndex - 1];
            }
            if (newPersonality == minister.Personality)
            {
                return;
            }

            minister.Personality = newPersonality;
            ministerListView.SelectedItems[0].SubItems[6].Text =
                Config.GetText(Ministers.PersonalityTable[minister.Personality].Name);

            UpdatePersonalityComboBox(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     イデオロギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdeologyComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterIdeology newIdeology = !string.IsNullOrEmpty(ideologyComboBox.Items[0].ToString())
                                               ? (MinisterIdeology) (ideologyComboBox.SelectedIndex + 1)
                                               : (MinisterIdeology) ideologyComboBox.SelectedIndex;
            if (newIdeology == minister.Ideology)
            {
                return;
            }

            minister.Ideology = newIdeology;
            ministerListView.SelectedItems[0].SubItems[7].Text =
                Config.GetText(Ministers.IdeologyTable[(int) minister.Ideology].Name);

            UpdateIdeologyComboBox(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            MinisterLoyalty newLoyalty = !string.IsNullOrEmpty(loyaltyComboBox.Items[0].ToString())
                                             ? (MinisterLoyalty) (loyaltyComboBox.SelectedIndex + 1)
                                             : (MinisterLoyalty) loyaltyComboBox.SelectedIndex;
            if (newLoyalty == minister.Loyalty)
            {
                return;
            }

            minister.Loyalty = newLoyalty;

            UpdateLoyaltyComboBox(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            // 値に変化がなければ何もせずに戻る
            string newPictureName = pictureNameTextBox.Text;
            if (newPictureName.Equals(minister.PictureName))
            {
                return;
            }

            minister.PictureName = newPictureName;
            UpdateMinisterPicture(minister);

            Ministers.SetDirtyFlag(minister.CountryTag);
        }

        /// <summary>
        ///     画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            if (ministerListView.SelectedItems.Count == 0)
            {
                return;
            }

            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }

            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = Path.Combine(Game.FolderName, Game.PersonPicturePathName),
                                 FileName = minister.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        /// <summary>
        ///     国家リストボックスの全選択/全解除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryAllButtonClick(object sender, EventArgs e)
        {
            countryListBox.BeginUpdate();

            // 選択イベントを処理すると時間がかかるので、一時的に無効化する
            countryListBox.SelectedIndexChanged -= OnCountryListBoxSelectedIndexChanged;

            if (countryListBox.SelectedItems.Count <= 1)
            {
                // スクロール位置を先頭に設定するため、逆順で選択する
                for (int i = countryListBox.Items.Count - 1; i >= 0; i--)
                {
                    countryListBox.SetSelected(i, true);
                }
            }
            else
            {
                for (int i = 0; i < countryListBox.Items.Count; i++)
                {
                    countryListBox.SetSelected(i, false);
                }
            }

            // 選択イベントを元に戻す
            countryListBox.SelectedIndexChanged += OnCountryListBoxSelectedIndexChanged;

            // 閣僚リスト絞り込みのため、ダミーでイベント発行する
            OnCountryListBoxSelectedIndexChanged(sender, e);

            countryListBox.EndUpdate();
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            LoadMinisterFiles();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            SaveMinisterFiles();
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
    }
}