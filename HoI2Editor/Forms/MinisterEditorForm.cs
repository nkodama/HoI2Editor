using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Forms
{
    /// <summary>
    /// 閣僚エディタのフォーム
    /// </summary>
    public partial class MinisterEditorForm : Form
    {
        /// <summary>
        /// 絞り込み後の閣僚リスト
        /// </summary>
        private readonly List<Minister> _narrowedMinisterList = new List<Minister>();

        /// <summary>
        /// マスター閣僚リスト
        /// </summary>
        private List<Minister> _masterMinisterList = new List<Minister>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MinisterEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterEditorFormLoad(object sender, EventArgs e)
        {
            Config.LoadConfigFiles("D:\\Games\\CYBERFRONT\\AoD_Original_107\\config");

            foreach (string positionText in Minister.PositionTextTable)
            {
                if (string.IsNullOrEmpty(positionText))
                {
                    continue;
                }
                positionComboBox.Items.Add(Config.Text[positionText]);
            }

            foreach (string personalityText in Minister.PersonalityTextTable)
            {
                if (string.IsNullOrEmpty(personalityText))
                {
                    continue;
                }
                personalityComboBox.Items.Add(Config.Text[personalityText]);
            }

            foreach (string ideologyText in Minister.IdeologyTextTable)
            {
                if (string.IsNullOrEmpty(ideologyText))
                {
                    continue;
                }
                ideologyComboBox.Items.Add(Config.Text[ideologyText]);
            }

            foreach (string loyaltyText in Minister.LoyaltyTextTable)
            {
                if (string.IsNullOrEmpty(loyaltyText))
                {
                    continue;
                }
                loyaltyComboBox.Items.Add(loyaltyText);
            }

            foreach (string countryText in Country.CountryTextTable)
            {
                if (string.IsNullOrEmpty(countryText))
                {
                    continue;
                }
                countryListBox.Items.Add(countryText);
                countryComboBox.Items.Add(countryText);
            }
            countryListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 閣僚リストを国タグで絞り込む
        /// </summary>
        private void NarrowMinisterList()
        {
            _narrowedMinisterList.Clear();
            if (countryListBox.SelectedItems.Count == 0)
            {
                return;
            }
            List<CountryTag> selectedTagList =
                (from string countryText in countryListBox.SelectedItems select Country.CountryTextMap[countryText]).
                    ToList();
            foreach (Minister minister in _masterMinisterList)
            {
                if (selectedTagList.Contains(minister.CountryTag))
                {
                    _narrowedMinisterList.Add(minister);
                }
            }
        }

        /// <summary>
        /// 閣僚リストの表示を更新する
        /// </summary>
        private void UpdateMinisterList()
        {
            ministerListView.BeginUpdate();
            ministerListView.Items.Clear();

            foreach (Minister minister in _narrowedMinisterList)
            {
                var item = new ListViewItem {Text = Country.CountryTextTable[(int) minister.CountryTag], Tag = minister};
                item.SubItems.Add(minister.Id.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(minister.Name);
                item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
                //item.SubItems.Add(minister.StartYear.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add("");
                item.SubItems.Add(Config.Text[Minister.PositionTextTable[(int) minister.Position]]);
                item.SubItems.Add(Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]]);
                item.SubItems.Add(Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]]);

                ministerListView.Items.Add(item);
            }

            if (ministerListView.Items.Count > 0)
            {
                ministerListView.Items[0].Focused = true;
                ministerListView.Items[0].Selected = true;
            }

            ministerListView.EndUpdate();
        }

        /// <summary>
        /// 開くボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenButtonClick(object sender, EventArgs e)
        {
            _masterMinisterList = Minister.LoadMinisterFiles("D:\\Games\\CYBERFRONT\\AoD_Original_107\\db\\ministers");

            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        /// 終了ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            NarrowMinisterList();
            UpdateMinisterList();
        }

        /// <summary>
        /// 閣僚リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinisterListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var minister = e.Item.Tag as Minister;
            if (minister != null)
            {
                countryComboBox.Text = Country.CountryTextTable[(int) minister.CountryTag];
                idNumericUpDown.Value = minister.Id;
                nameTextBox.Text = minister.Name;
                startYearNumericUpDown.Value = minister.StartYear;
                endYearNumericUpDown.Value = minister.EndYear;
                positionComboBox.Text = Config.Text[Minister.PositionTextTable[(int) minister.Position]];
                personalityComboBox.Text = Config.Text[Minister.PersonalityTextTable[(int) minister.Personality]];
                ideologyComboBox.Text = Config.Text[Minister.IdeologyTextTable[(int) minister.Ideology]];
                loyaltyComboBox.Text = Minister.LoyaltyTextTable[(int) minister.Loyalty];
                pictureNameTextBox.Text = minister.PictureName;
                ministerPictureBox.ImageLocation =
                    Path.Combine("D:\\Games\\CYBERFRONT\\AoD_Original_107\\gfx\\interface\\pics",
                                 Path.ChangeExtension(minister.PictureName, ".bmp"));
            }
        }

        /// <summary>
        /// ID変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Id = (int) idNumericUpDown.Value;
            ministerListView.SelectedItems[0].SubItems[1].Text = minister.Id.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 名前文字列変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNameTextBoxTextChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Name = nameTextBox.Text;
            ministerListView.SelectedItems[0].SubItems[2].Text = minister.Name;
        }

        /// <summary>
        /// 開始年変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.StartYear = (int) startYearNumericUpDown.Value;
            ministerListView.SelectedItems[0].SubItems[3].Text =
                minister.StartYear.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 閣僚地位変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Position = Minister.PositionTextMap[positionComboBox.Text];
            ministerListView.SelectedItems[0].SubItems[5].Text = positionComboBox.Text;
        }

        /// <summary>
        /// 閣僚特性変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPersonalityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Personality = Minister.PersonalityTextMap[personalityComboBox.Text];
            ministerListView.SelectedItems[0].SubItems[6].Text = personalityComboBox.Text;
        }

        /// <summary>
        /// イデオロギー変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdeologyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Ideology = Minister.IdeologyTextMap[ideologyComboBox.Text];
            ministerListView.SelectedItems[0].SubItems[7].Text = ideologyComboBox.Text;
        }

        /// <summary>
        /// 忠誠度変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.Loyalty = Minister.LoyaltyTextMap[loyaltyComboBox.Text];
        }

        /// <summary>
        /// 画像ファイル名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameTextBoxTextChanged(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            minister.PictureName = pictureNameTextBox.Text;
            ministerPictureBox.ImageLocation =
                Path.Combine("D:\\Games\\CYBERFRONT\\AoD_Original_107\\gfx\\interface\\pics",
                             Path.ChangeExtension(minister.PictureName, ".bmp"));
        }

        /// <summary>
        /// 画像ファイル名参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPictureNameReferButtonClick(object sender, EventArgs e)
        {
            var minister = ministerListView.SelectedItems[0].Tag as Minister;
            if (minister == null)
            {
                return;
            }
            var dialog = new OpenFileDialog
                             {
                                 InitialDirectory = "D:\\Games\\CYBERFRONT\\AoD_Original_107\\gfx\\interface\\pics",
                                 FileName = minister.PictureName,
                                 Filter = Resources.OpenBitmapFileDialogFilter
                             };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureNameTextBox.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }
    }
}