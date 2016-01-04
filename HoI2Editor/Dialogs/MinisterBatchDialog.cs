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
        #region 内部フィールド

        /// <summary>
        ///     一括編集のパラメータ
        /// </summary>
        private readonly MinisterBatchEditArgs _args;

        /// <summary>
        ///     開始IDが変更されたかどうか
        /// </summary>
        private bool _idChanged;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        public MinisterBatchDialog(MinisterBatchEditArgs args)
        {
            InitializeComponent();

            _args = args;
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
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            // 対象国コンボボックス
            srcComboBox.BeginUpdate();
            srcComboBox.Items.Clear();
            int width = srcComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? $"{name} {Config.GetText(name)}"
                    : name))
            {
                srcComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, srcComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            srcComboBox.DropDownWidth = width;
            srcComboBox.EndUpdate();
            if (srcComboBox.Items.Count > 0)
            {
                srcComboBox.SelectedIndex = Countries.Tags.ToList().IndexOf(_args.TargetCountries[0]);
            }
            srcComboBox.SelectedIndexChanged += OnSrcComboBoxSelectedIndexChanged;

            // 閣僚地位
            hosCheckBox.Text = Config.GetText(TextId.MinisterHeadOfState);
            hogCheckBox.Text = Config.GetText(TextId.MinisterHeadOfGovernment);
            mofCheckBox.Text = Config.GetText(TextId.MinisterForeignMinister);
            moaCheckBox.Text = Config.GetText(TextId.MinisterArmamentMinister);
            mosCheckBox.Text = Config.GetText(TextId.MinisterMinisterOfSecurity);
            moiCheckBox.Text = Config.GetText(TextId.MinisterMinisterOfIntelligence);
            cosCheckBox.Text = Config.GetText(TextId.MinisterChiefOfStaff);
            coaCheckBox.Text = Config.GetText(TextId.MinisterChiefOfArmy);
            conCheckBox.Text = Config.GetText(TextId.MinisterChiefOfNavy);
            coafCheckBox.Text = Config.GetText(TextId.MinisterChiefOfAir);

            // コピー/移動先コンボボックス
            destComboBox.BeginUpdate();
            destComboBox.Items.Clear();
            width = destComboBox.Width;
            foreach (string s in Countries.Tags
                .Select(country => Countries.Strings[(int) country])
                .Select(name => Config.ExistsKey(name)
                    ? $"{name} {Config.GetText(name)}"
                    : name))
            {
                destComboBox.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, destComboBox.Font).Width + SystemInformation.VerticalScrollBarWidth +
                    margin);
            }
            destComboBox.DropDownWidth = width;
            destComboBox.EndUpdate();
            if (destComboBox.Items.Count > 0)
            {
                destComboBox.SelectedIndex = Countries.Tags.ToList().IndexOf(_args.TargetCountries[0]);
            }
            destComboBox.SelectedIndexChanged += OnDestComboBoxSelectedIndexChanged;

            // 開始ID
            if (_args.TargetCountries.Count > 0)
            {
                idNumericUpDown.Value = Ministers.GetNewId(_args.TargetCountries[0]);
            }
            idNumericUpDown.ValueChanged += OnIdNumericUpDownValueChanged;

            // 終了年
            if (Game.Type != GameType.DarkestHour)
            {
                endYearCheckBox.Enabled = false;
                endYearNumericUpDown.Enabled = false;
                endYearNumericUpDown.ResetText();
            }

            // 引退年
            if ((Game.Type != GameType.DarkestHour) || (Game.Version < 103))
            {
                retirementYearCheckBox.Enabled = false;
                retirementYearNumericUpDown.Enabled = false;
                retirementYearNumericUpDown.ResetText();
            }

            // イデオロギーコンボボックス
            ideologyComboBox.BeginUpdate();
            ideologyComboBox.Items.Clear();
            width = ideologyComboBox.Width;
            foreach (string s in Ministers.IdeologyNames.Where(id => id != TextId.Empty).Select(Config.GetText))
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
        ///     OKキー押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkButtonClick(object sender, EventArgs e)
        {
            // 対象国モード
            if (allRadioButton.Checked)
            {
                _args.CountryMode = BatchCountryMode.All;
            }
            else if (selectedRadioButton.Checked)
            {
                _args.CountryMode = BatchCountryMode.Selected;
            }
            else
            {
                _args.CountryMode = BatchCountryMode.Specified;
                _args.TargetCountries.Clear();
                _args.TargetCountries.Add(Countries.Tags[srcComboBox.SelectedIndex]);
            }

            // 対象地位モード
            _args.PositionMode[(int) MinisterPosition.HeadOfState] = hosCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.HeadOfGovernment] = hogCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.ForeignMinister] = mofCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.MinisterOfArmament] = moaCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.MinisterOfSecurity] = mosCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.HeadOfMilitaryIntelligence] = moiCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.ChiefOfStaff] = cosCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.ChiefOfArmy] = coaCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.ChiefOfNavy] = conCheckBox.Checked;
            _args.PositionMode[(int) MinisterPosition.ChiefOfAirForce] = coafCheckBox.Checked;

            // 動作モード
            if (copyRadioButton.Checked)
            {
                _args.ActionMode = BatchActionMode.Copy;
            }
            else if (moveRadioButton.Checked)
            {
                _args.ActionMode = BatchActionMode.Move;
            }
            else
            {
                _args.ActionMode = BatchActionMode.Modify;
            }

            _args.Destination = Countries.Tags[destComboBox.SelectedIndex];
            _args.Id = (int) idNumericUpDown.Value;

            // 編集項目
            _args.Items[(int) MinisterBatchItemId.StartYear] = startYearCheckBox.Checked;
            _args.Items[(int) MinisterBatchItemId.EndYear] = endYearCheckBox.Checked;
            _args.Items[(int) MinisterBatchItemId.RetirementYear] = retirementYearCheckBox.Checked;
            _args.Items[(int) MinisterBatchItemId.Ideology] = ideologyCheckBox.Checked;
            _args.Items[(int) MinisterBatchItemId.Loyalty] = loyaltyCheckBox.Checked;

            _args.StartYear = (int) startYearNumericUpDown.Value;
            _args.EndYear = (int) endYearNumericUpDown.Value;
            _args.RetirementYear = (int) retirementYearNumericUpDown.Value;
            _args.Ideology = (MinisterIdeology) (ideologyComboBox.SelectedIndex + 1);
            _args.Loyalty = (MinisterLoyalty) (loyaltyComboBox.SelectedIndex + 1);
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     対象国コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSrcComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (srcComboBox.SelectedIndex < 0)
            {
                return;
            }

            if (!specifiedRadioButton.Checked)
            {
                specifiedRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     コピーラジオボタンのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!copyRadioButton.Checked)
            {
                return;
            }

            if (startYearCheckBox.Checked)
            {
                startYearCheckBox.Checked = false;
            }
            if (endYearCheckBox.Checked)
            {
                endYearCheckBox.Checked = false;
            }
            if (retirementYearCheckBox.Checked)
            {
                retirementYearCheckBox.Checked = false;
            }
            if (ideologyCheckBox.Checked)
            {
                ideologyCheckBox.Checked = false;
            }
            if (loyaltyCheckBox.Checked)
            {
                loyaltyCheckBox.Checked = false;
            }
        }

        /// <summary>
        ///     移動ラジオボタンのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!moveRadioButton.Checked)
            {
                return;
            }

            if (startYearCheckBox.Checked)
            {
                startYearCheckBox.Checked = false;
            }
            if (endYearCheckBox.Checked)
            {
                endYearCheckBox.Checked = false;
            }
            if (retirementYearCheckBox.Checked)
            {
                retirementYearCheckBox.Checked = false;
            }
            if (ideologyCheckBox.Checked)
            {
                ideologyCheckBox.Checked = false;
            }
            if (loyaltyCheckBox.Checked)
            {
                loyaltyCheckBox.Checked = false;
            }
        }

        /// <summary>
        ///     コピー/移動先コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDestComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (destComboBox.SelectedIndex < 0)
            {
                return;
            }

            if (!copyRadioButton.Checked && !moveRadioButton.Checked)
            {
                copyRadioButton.Checked = true;
            }

            // 開始ID数値アップダウンの数値が変更されていなければ変更する
            if (!_idChanged)
            {
                idNumericUpDown.ValueChanged -= OnIdNumericUpDownValueChanged;
                idNumericUpDown.Value = Ministers.GetNewId(Countries.Tags[destComboBox.SelectedIndex]);
                idNumericUpDown.ValueChanged += OnIdNumericUpDownValueChanged;
            }
        }

        /// <summary>
        ///     開始ID数値アップダウンの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdNumericUpDownValueChanged(object sender, EventArgs e)
        {
            _idChanged = true;

            if (!copyRadioButton.Checked && !moveRadioButton.Checked)
            {
                copyRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     開始年チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartYearCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (startYearCheckBox.Checked && !modifyRadioButton.Checked)
            {
                modifyRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     終了年チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndYearCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (endYearCheckBox.Checked && !modifyRadioButton.Checked)
            {
                modifyRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     引退年チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetirementYearCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (retirementYearCheckBox.Checked && !modifyRadioButton.Checked)
            {
                modifyRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     イデオロギーチェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIdeologyCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (ideologyCheckBox.Checked && !modifyRadioButton.Checked)
            {
                modifyRadioButton.Checked = true;
            }
        }

        /// <summary>
        ///     忠誠度チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoyaltyCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (loyaltyCheckBox.Checked && !modifyRadioButton.Checked)
            {
                modifyRadioButton.Checked = true;
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

        #endregion
    }
}