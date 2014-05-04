using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Dialogs
{
    /// <summary>
    ///     閣僚一括編集ダイアログ
    /// </summary>
    public partial class MinisterBatchDialog : Form
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MinisterBatchDialog(Country country)
        {
            InitializeComponent();

            SelectedCountry = country;
        }

        /// <summary>
        ///     一括編集対象モード
        /// </summary>
        public MinisterBatchMode Mode { get; private set; }

        /// <summary>
        ///     一括編集項目
        /// </summary>
        public bool[] BatchItems { get; private set; }

        /// <summary>
        ///     選択国
        /// </summary>
        public Country SelectedCountry { get; private set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; private set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; private set; }

        /// <summary>
        ///     引退年
        /// </summary>
        public int RetirementYear { get; private set; }

        /// <summary>
        ///     イデオロギー
        /// </summary>
        public MinisterIdeology Ideology { get; private set; }

        /// <summary>
        ///     忠誠度
        /// </summary>
        public MinisterLoyalty Loyalty { get; private set; }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // メンバ変数の初期化
            BatchItems = new bool[Enum.GetValues(typeof (MinisterBatchItemId)).Length];
            StartYear = 1936;
            EndYear = 1964;
            RetirementYear = 1999;

            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 選択国コンボボックス
            countryComboBox.BeginUpdate();
            countryComboBox.Items.Clear();
            int width = countryComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, countryComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            countryComboBox.DropDownWidth = width;
            countryComboBox.EndUpdate();
            if (countryComboBox.Items.Count > 0)
            {
                countryComboBox.SelectedIndex = Countries.Tags.ToList().IndexOf(SelectedCountry);
            }
            countryComboBox.SelectedIndexChanged += OnCountryComboBoxSelectedIndexChanged;

            // イデオロギーコンボボックス
            ideologyComboBox.BeginUpdate();
            ideologyComboBox.Items.Clear();
            width = ideologyComboBox.Width;
            foreach (
                string s in Ministers.IdeologyNames.Where(name => !string.IsNullOrEmpty(name)).Select(Config.GetText))
            {
                ideologyComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, ideologyComboBox.Font).Width + margin);
            }
            ideologyComboBox.DropDownWidth = width;
            ideologyComboBox.EndUpdate();
            if (ideologyComboBox.Items.Count > 0)
            {
                ideologyComboBox.SelectedIndex = 0;
            }
            ideologyComboBox.SelectedIndexChanged += OnIdeologyComboBoxSelectedIndexChanged;

            // 忠誠度コンボボックス
            loyaltyComboBox.BeginUpdate();
            loyaltyComboBox.Items.Clear();
            width = loyaltyComboBox.Width;
            foreach (string s in Ministers.LoyaltyNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                loyaltyComboBox.Items.Add(s);
                width = Math.Max(width, (int) g.MeasureString(s, loyaltyComboBox.Font).Width + margin);
            }
            loyaltyComboBox.DropDownWidth = width;
            loyaltyComboBox.EndUpdate();
            if (loyaltyComboBox.Items.Count > 0)
            {
                loyaltyComboBox.SelectedIndex = 0;
            }
            loyaltyComboBox.SelectedIndexChanged += OnLoyaltyComboBoxSelectedIndexChanged;
        }

        /// <summary>
        ///     国家コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (countryComboBox.SelectedIndex < 0)
            {
                return;
            }
            if (!specifiedRadioButton.Checked)
            {
                specifiedRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     開始年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (!startYearCheckBox.Checked)
            {
                startYearCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     終了年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (!endYearCheckBox.Checked)
            {
                endYearCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     引退年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (!retirementYearCheckBox.Checked)
            {
                retirementYearCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     イデオロギーコンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeologyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ideologyComboBox.SelectedIndex < 0)
            {
                return;
            }
            if (!ideologyCheckBox.Checked)
            {
                ideologyCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     忠誠度コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (loyaltyComboBox.SelectedIndex < 0)
            {
                return;
            }
            if (!loyaltyCheckBox.Checked)
            {
                loyaltyCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     OKキー押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (allRadioButton.Checked)
            {
                Mode = MinisterBatchMode.All;
            }
            else if (selectedRadioButton.Checked)
            {
                Mode = MinisterBatchMode.Selected;
            }
            else
            {
                Mode = MinisterBatchMode.Specified;
            }
            if (countryComboBox.SelectedIndex >= 0)
            {
                SelectedCountry = Countries.Tags[countryComboBox.SelectedIndex];
            }

            BatchItems[(int) MinisterBatchItemId.StartYear] = startYearCheckBox.Checked;
            BatchItems[(int) MinisterBatchItemId.EndYear] = endYearCheckBox.Checked;
            BatchItems[(int) MinisterBatchItemId.RetirementYear] = retirementYearCheckBox.Checked;
            BatchItems[(int) MinisterBatchItemId.Ideology] = ideologyCheckBox.Checked;
            BatchItems[(int) MinisterBatchItemId.Loyalty] = loyaltyCheckBox.Checked;

            StartYear = (int) startYearNumericUpDown.Value;
            EndYear = (int) endYearNumericUpDown.Value;
            RetirementYear = (int) retirementYearNumericUpDown.Value;
            Ideology = (MinisterIdeology) (ideologyComboBox.SelectedIndex + 1);
            Loyalty = (MinisterLoyalty) (loyaltyComboBox.SelectedIndex + 1);
        }
    }

    /// <summary>
    ///     一括編集対象モード
    /// </summary>
    public enum MinisterBatchMode
    {
        All, // 全て
        Selected, // 選択国
        Specified, // 指定国
    }

    /// <summary>
    ///     一括編集項目ID
    /// </summary>
    public enum MinisterBatchItemId
    {
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Ideology, // イデオロギー
        Loyalty, // 忠誠度
    }
}