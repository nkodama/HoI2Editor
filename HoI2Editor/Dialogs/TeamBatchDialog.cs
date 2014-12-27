using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Dialogs
{
    /// <summary>
    ///     研究機関一括編集ダイアログ
    /// </summary>
    public partial class TeamBatchDialog : Form
    {
        #region 公開プロパティ

        /// <summary>
        ///     一括編集対象モード
        /// </summary>
        public TeamBatchMode Mode { get; private set; }

        /// <summary>
        ///     一括編集項目
        /// </summary>
        public bool[] BatchItems { get; private set; }

        /// <summary>
        ///     選択国
        /// </summary>
        public Country SelectedCountry { get; private set; }

        /// <summary>
        ///     スキル
        /// </summary>
        public int Skill { get; private set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; private set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="country">研究機関エディタフォームでの選択国</param>
        public TeamBatchDialog(Country country)
        {
            InitializeComponent();

            SelectedCountry = country;
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // メンバ変数の初期化
            BatchItems = new bool[Enum.GetValues(typeof (MinisterBatchItemId)).Length];

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
                Mode = TeamBatchMode.All;
            }
            else if (selectedRadioButton.Checked)
            {
                Mode = TeamBatchMode.Selected;
            }
            else
            {
                Mode = TeamBatchMode.Specified;
            }
            if (countryComboBox.SelectedIndex >= 0)
            {
                SelectedCountry = Countries.Tags[countryComboBox.SelectedIndex];
            }

            BatchItems[(int) TeamBatchItemId.Skill] = skillCheckBox.Checked;
            BatchItems[(int) TeamBatchItemId.StartYear] = startYearCheckBox.Checked;
            BatchItems[(int) TeamBatchItemId.EndYear] = endYearCheckBox.Checked;

            Skill = (int) skillNumericUpDown.Value;
            StartYear = (int) startYearNumericUpDown.Value;
            EndYear = (int) endYearNumericUpDown.Value;
        }

        #endregion

        #region 編集項目

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
        ///     スキル数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (!skillCheckBox.Checked)
            {
                skillCheckBox.Checked = true;
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

        #endregion
    }

    /// <summary>
    ///     一括編集対象モード
    /// </summary>
    public enum TeamBatchMode
    {
        All, // 全て
        Selected, // 選択国
        Specified // 指定国
    }

    /// <summary>
    ///     一括編集項目ID
    /// </summary>
    public enum TeamBatchItemId
    {
        Skill, // スキル
        StartYear, // 開始年
        EndYear // 終了年
    }
}