using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Dialogs
{
    /// <summary>
    /// 指揮官一括編集ダイアログ
    /// </summary>
    public partial class LeaderBatchDialog : Form
    {
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
        /// 経験値
        /// </summary>
        public int Experience { get; private set; }

        /// <summary>
        /// 忠誠度
        /// </summary>
        public int Loyalty { get; private set; }

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
        /// 任官年
        /// </summary>
        public int[] RankYear { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="country">指揮官エディタフォームでの選択国</param>
        public LeaderBatchDialog(Country country)
        {
            InitializeComponent();

            SelectedCountry = country;
        }

        /// <summary>
        /// フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // メンバ変数の初期化
            BatchItems = new bool[Enum.GetValues(typeof(LeaderBatchItemId)).Length];
            RankYear = new int[Enum.GetValues(typeof (LeaderRank)).Length - 1];

            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 選択国コンボボックス
            countryComboBox.BeginUpdate();
            countryComboBox.Items.Clear();
            int width = countryComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int)country])
                .Select(name => Config.ExistsKey(name)
                    ? string.Format("{0} {1}", name, Config.GetText(name))
                    : name))
            {
                countryComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int)g.MeasureString(s, countryComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            countryComboBox.DropDownWidth = width;
            countryComboBox.EndUpdate();
            if (countryComboBox.Items.Count > 0)
            {
                countryComboBox.SelectedIndex = Countries.Tags.ToList().IndexOf(SelectedCountry);
            }
            countryComboBox.SelectedIndexChanged += OnCountryComboBoxSelectedIndexChanged;

            // 理想階級コンボボックス
            idealRankComboBox.BeginUpdate();
            idealRankComboBox.Items.Clear();
            width = idealRankComboBox.Width;
            foreach (string s in Leaders.RankNames.Where(name => !string.IsNullOrEmpty(name)))
            {
                idealRankComboBox.Items.Add(s);
                width = Math.Max(width, (int)g.MeasureString(s, idealRankComboBox.Font).Width + margin);
            }
            idealRankComboBox.DropDownWidth = width;
            idealRankComboBox.EndUpdate();
            if (idealRankComboBox.Items.Count > 0)
            {
                idealRankComboBox.SelectedIndex = 0;
            }
            idealRankComboBox.SelectedIndexChanged += OnIdealRankComboBoxSelectedIndexChanged;
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
            specifiedRadioButton.Checked = true;
        }

        /// <summary>
        ///     理想階級コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdealRankComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (idealRankComboBox.SelectedIndex < 0)
            {
                return;
            }
            idealRankCheckBox.Checked = true;
        }

        /// <summary>
        ///     スキル数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            skillCheckBox.Checked = true;
        }

        /// <summary>
        ///     最大スキル数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMaxSkillNumericUpDownValueChanged(object sender, EventArgs e)
        {
            maxSkillCheckBox.Checked = true;
        }

        /// <summary>
        ///     経験値数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExperienceNumericUpDownValueChanged(object sender, EventArgs e)
        {
            experienceCheckBox.Checked = true;
        }

        /// <summary>
        ///     忠誠度数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyNumericUpDownValueChanged(object sender, EventArgs e)
        {
            loyaltyCheckBox.Checked = true;
        }

        /// <summary>
        ///     開始年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            startYearCheckBox.Checked = true;
        }

        /// <summary>
        ///     終了年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            endYearCheckBox.Checked = true;
        }

        /// <summary>
        ///     引退年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearNumericUpDownValueChanged(object sender, EventArgs e)
        {
            retirementYearCheckBox.Checked = true;
        }

        /// <summary>
        ///     少将任官年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            rankYearCheckBox1.Checked = true;
        }

        /// <summary>
        ///      中将任官年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            rankYearCheckBox2.Checked = true;
        }

        /// <summary>
        ///     大将任官年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown3ValueChanged(object sender, EventArgs e)
        {
            rankYearCheckBox3.Checked = true;
        }

        /// <summary>
        ///     元帥任官年数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRankYearNumericUpDown4ValueChanged(object sender, EventArgs e)
        {
            rankYearCheckBox4.Checked = true;
        }
    }

    /// <summary>
    ///     一括編集対象モード
    /// </summary>
    public enum LeaderBatchMode
    {
        All, // 全て
        Selected, // 選択国
        Specified, // 指定国
    }

    /// <summary>
    ///     一括編集項目ID
    /// </summary>
    public enum LeaderBatchItemId
    {
        IdealRank, // 理想階級
        Skill, // スキル
        MaxSkill, // 最大スキル
        Experience, // 経験値
        Loyalty, // 忠誠度
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Rank3Year, // 少将任官年
        Rank2Year, // 中将任官年
        Rank1Year, // 大将任官年
        Rank0Year, // 元帥任官年
    }
}
