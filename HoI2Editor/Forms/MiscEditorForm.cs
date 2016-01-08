using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     基礎データエディタのフォーム
    /// </summary>
    public partial class MiscEditorForm : Form
    {
        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MiscEditorForm()
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
            foreach (TabPage page in miscTabControl.TabPages)
            {
                // 編集項目を更新する
                UpdateEditableItems(page);
                // 編集項目の色を更新する
                UpdateItemColor(page);
            }
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        public void OnFileSaved()
        {
            // 編集済みフラグがクリアされるため表示を更新する
            foreach (TabPage page in miscTabControl.TabPages)
            {
                UpdateItemColor(page);
            }
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        public void OnItemChanged(EditorItemId id)
        {
            switch (id)
            {
                case EditorItemId.LeaderRetirementYear:
                    Log.Verbose("[Misc] Changed leader retirement year");
                    UpdateLeaderRetirementYear();
                    break;

                case EditorItemId.MinisterEndYear:
                    Log.Verbose("[Misc] Changed minister end year");
                    UpdateMinisterEndYear();
                    break;

                case EditorItemId.MinisterRetirementYear:
                    Log.Verbose("[Misc] Changed minister retirement year");
                    UpdateMinisterRetirementYear();
                    break;

                case EditorItemId.MaxAllowedBrigades:
                    Log.Verbose("[Misc] Changed max allowed brigades");
                    UpdateMaxAllowedBrigades();
                    break;
            }
        }

        /// <summary>
        ///     指揮官引退年の有無を更新する
        /// </summary>
        private void UpdateLeaderRetirementYear()
        {
            // DH1.03以降でなければ何もしない
            if ((Game.Type != GameType.DarkestHour) || (Game.Version < 103))
            {
                return;
            }
            foreach (TabPage tabPage in miscTabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    // タグの設定されていないラベルをスキップする
                    if (control.Tag == null)
                    {
                        continue;
                    }

                    MiscItemId id = (MiscItemId) control.Tag;
                    if ((MiscItemId) control.Tag == MiscItemId.EnableRetirementYearLeaders)
                    {
                        ComboBox comboBox = control as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                            comboBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚終了年の有無を更新する
        /// </summary>
        private void UpdateMinisterEndYear()
        {
            // DHでなければ何もしない
            if (Game.Type != GameType.DarkestHour)
            {
                return;
            }
            foreach (TabPage tabPage in miscTabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    // タグの設定されていないラベルをスキップする
                    if (control.Tag == null)
                    {
                        continue;
                    }

                    MiscItemId id = (MiscItemId) control.Tag;
                    if ((MiscItemId) control.Tag == MiscItemId.UseNewMinisterFilesFormat)
                    {
                        ComboBox comboBox = control as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                            comboBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚引退年の有無を更新する
        /// </summary>
        private void UpdateMinisterRetirementYear()
        {
            // DH1.03以降でなければ何もしない
            if ((Game.Type != GameType.DarkestHour) || (Game.Version < 103))
            {
                return;
            }
            foreach (TabPage tabPage in miscTabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    // タグの設定されていないラベルをスキップする
                    if (control.Tag == null)
                    {
                        continue;
                    }

                    MiscItemId id = (MiscItemId) control.Tag;
                    if ((MiscItemId) control.Tag == MiscItemId.EnableRetirementYearMinisters)
                    {
                        ComboBox comboBox = control as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                            comboBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     最大付属可能旅団数を更新する
        /// </summary>
        private void UpdateMaxAllowedBrigades()
        {
            // AoD1.07以降でなければ何もしない
            if ((Game.Type != GameType.ArsenalOfDemocracy) || (Game.Version < 107))
            {
                return;
            }
            foreach (TabPage tabPage in miscTabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    // タグの設定されていないラベルをスキップする
                    if (control.Tag == null)
                    {
                        continue;
                    }

                    MiscItemId id = (MiscItemId) control.Tag;
                    switch (id)
                    {
                        case MiscItemId.TpMaxAttach: // 輸送艦最大付属装備数
                        case MiscItemId.SsMaxAttach: // 潜水艦最大付属装備数
                        case MiscItemId.SsnMaxAttach: // 原子力潜水艦最大付属装備数
                        case MiscItemId.DdMaxAttach: // 駆逐艦最大付属装備数
                        case MiscItemId.ClMaxAttach: // 軽巡洋艦最大付属装備数
                        case MiscItemId.CaMaxAttach: // 重巡洋艦最大付属装備数
                        case MiscItemId.BcMaxAttach: // 巡洋戦艦最大付属装備数
                        case MiscItemId.BbMaxAttach: // 戦艦最大付属装備数
                        case MiscItemId.CvlMaxAttach: // 軽空母最大付属装備数
                        case MiscItemId.CvMaxAttach: // 空母最大付属装備数
                            TextBox textBox = control as TextBox;
                            if (textBox != null)
                            {
                                textBox.Text = Misc.GetString(id);
                                textBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                            }
                            break;
                    }
                }
            }
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // ウィンドウの位置
            Location = HoI2EditorController.Settings.MiscEditor.Location;
            //Size = HoI2Editor.Settings.MiscEditor.Size;

            // 画面解像度が十分に広い場合はタブページが広く表示できるようにする
            int longHeight = DeviceCaps.GetScaledHeight(720);
            if (Screen.GetWorkingArea(this).Height >= longHeight)
            {
                Height = longHeight;
            }
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 基本データファイルを読み込む
            Misc.Load();

            // タブページ群を初期化する
            InitTabPages();

            // データ読み込み後の処理
            OnFileLoaded();

            // 選択中のタブページを初期化する
            if (miscTabControl.TabCount > 0)
            {
                int index = HoI2EditorController.Settings.MiscEditor.SelectedTab;
                if ((index < 0) || (index >= miscTabControl.TabCount))
                {
                    index = 0;
                }
                InitTabPage(miscTabControl.TabPages[index]);
                miscTabControl.SelectTab(index);
            }
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // 編集済みでなければフォームを閉じる
            if (!HoI2EditorController.IsDirty())
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
                    HoI2EditorController.Save();
                    break;
                case DialogResult.No:
                    HoI2EditorController.SaveCanceled = true;
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
            HoI2EditorController.OnMiscEditorFormClosed();
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
                HoI2EditorController.Settings.MiscEditor.Location = Location;
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
                HoI2EditorController.Settings.MiscEditor.Size = Size;
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
            if (HoI2EditorController.IsDirty())
            {
                DialogResult result = MessageBox.Show(Resources.ConfirmSaveMessage, Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        HoI2EditorController.Save();
                        break;
                }
            }

            HoI2EditorController.Reload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            HoI2EditorController.Save();
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

        #region タブページ処理

        /// <summary>
        ///     タブページ群を初期化する
        /// </summary>
        private void InitTabPages()
        {
            miscTabControl.TabPages.Clear();

            MiscGameType type = Misc.GetGameType();
            int itemHeight = DeviceCaps.GetScaledHeight(25);
            int itemMargin = DeviceCaps.GetScaledWidth(20);
            int itemsPerColumn = (miscTabControl.ClientSize.Height - miscTabControl.ItemSize.Height - itemMargin) /
                                 itemHeight;
            const int columnsPerPage = 3;

            foreach (MiscSectionId section in Enum.GetValues(typeof (MiscSectionId))
                .Cast<MiscSectionId>()
                .Where(section => Misc.SectionTable[(int) section, (int) type]))
            {
                TabPage tabPage = new TabPage
                {
                    Text = Misc.GetSectionName(section),
                    BackColor = SystemColors.Control
                };
                List<List<MiscItemId>> table = new List<List<MiscItemId>>();
                List<MiscItemId> list = new List<MiscItemId>();
                int row = 0;
                int column = 0;
                int page = 1;
                foreach (MiscItemId id in Misc.SectionItems[(int) section]
                    .Where(id => Misc.ItemTable[(int) id, (int) type]))
                {
                    if (row >= itemsPerColumn)
                    {
                        table.Add(list);

                        list = new List<MiscItemId>();
                        column++;
                        row = 0;

                        if (column >= columnsPerPage)
                        {
                            if (page == 1)
                            {
                                tabPage.Text += IntHelper.ToString(page);
                            }
                            tabPage.Tag = table;
                            miscTabControl.TabPages.Add(tabPage);

                            page++;
                            tabPage = new TabPage
                            {
                                Text = Misc.GetSectionName(section) + IntHelper.ToString(page),
                                BackColor = SystemColors.Control
                            };
                            table = new List<List<MiscItemId>>();
                            column = 0;
                        }
                    }

                    list.Add(id);
                    row++;
                }

                table.Add(list);
                tabPage.Tag = table;
                miscTabControl.TabPages.Add(tabPage);
            }
        }

        /// <summary>
        ///     タブページを初期化する
        /// </summary>
        /// <param name="tabPage">対象のタブページ</param>
        private void InitTabPage(TabPage tabPage)
        {
            // 編集項目を作成する
            CreateEditableItems(tabPage);

            // 編集項目を更新する
            UpdateEditableItems(tabPage);

            // 編集項目の色を更新する
            UpdateItemColor(tabPage);
        }

        /// <summary>
        ///     編集項目を作成する
        /// </summary>
        /// <param name="tabPage">対象のタブページ</param>
        private void CreateEditableItems(TabPage tabPage)
        {
            List<List<MiscItemId>> table = tabPage.Tag as List<List<MiscItemId>>;
            if (table == null)
            {
                return;
            }

            Graphics g = Graphics.FromHwnd(Handle);
            int itemHeight = DeviceCaps.GetScaledHeight(25);
            int labelStartX = DeviceCaps.GetScaledWidth(10);
            int labelStartY = DeviceCaps.GetScaledHeight(13);
            int editStartY = DeviceCaps.GetScaledHeight(10);
            int labelEditMargin = DeviceCaps.GetScaledWidth(8);
            int columnMargin = DeviceCaps.GetScaledWidth(10);
            int textBoxWidth = DeviceCaps.GetScaledWidth(50);
            int textBoxHeight = DeviceCaps.GetScaledHeight(19);
            int comboBoxWidthUnit = DeviceCaps.GetScaledWidth(15);
            int comboBoxWidthBase = DeviceCaps.GetScaledWidth(50);
            int comboBoxWidthMargin = DeviceCaps.GetScaledWidth(8);
            int comboBoxHeight = DeviceCaps.GetScaledHeight(20);

            int labelX = labelStartX;
            foreach (List<MiscItemId> list in table)
            {
                int labelY = labelStartY;
                int editX = labelX; // 編集コントロールの右端X座標(左端ではない)
                foreach (MiscItemId id in list)
                {
                    // ラベルを作成する
                    Label label = new Label
                    {
                        Text = Misc.GetItemName(id),
                        AutoSize = true,
                        Location = new Point(labelX, labelY)
                    };
                    string t = Misc.GetItemToolTip(id);
                    if (!string.IsNullOrEmpty(t))
                    {
                        miscToolTip.SetToolTip(label, t);
                    }
                    tabPage.Controls.Add(label);

                    // 編集コントロールの幅のみ求める
                    int x = labelX + label.Width + labelEditMargin;
                    MiscItemType type = Misc.ItemTypes[(int) id];
                    switch (type)
                    {
                        case MiscItemType.Bool:
                        case MiscItemType.Enum:
                            int maxWidth = comboBoxWidthBase;
                            for (int i = Misc.IntMinValues[id]; i <= Misc.IntMaxValues[id]; i++)
                            {
                                string s = Misc.GetItemChoice(id, i);
                                if (string.IsNullOrEmpty(s))
                                {
                                    continue;
                                }
                                maxWidth = Math.Max(maxWidth,
                                    (int) g.MeasureString(s, Font).Width + SystemInformation.VerticalScrollBarWidth
                                    + comboBoxWidthMargin);
                                maxWidth = comboBoxWidthBase
                                           + (maxWidth - comboBoxWidthBase + (comboBoxWidthUnit - 1))
                                           / comboBoxWidthUnit * comboBoxWidthUnit;
                            }
                            x += maxWidth;
                            break;

                        default:
                            // テキストボックスの項目は固定
                            x += textBoxWidth;
                            break;
                    }
                    if (x > editX)
                    {
                        editX = x;
                    }
                    labelY += itemHeight;
                }
                int editY = editStartY;
                foreach (MiscItemId id in list)
                {
                    // 編集コントロールを作成する
                    MiscItemType type = Misc.ItemTypes[(int) id];
                    switch (type)
                    {
                        case MiscItemType.Bool:
                        case MiscItemType.Enum:
                            ComboBox comboBox = new ComboBox
                            {
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                DrawMode = DrawMode.OwnerDrawFixed,
                                Tag = id
                            };
                            // コンボボックスの選択項目を登録し、最大幅を求める
                            int maxWidth = comboBoxWidthBase;
                            for (int i = Misc.IntMinValues[id]; i <= Misc.IntMaxValues[id]; i++)
                            {
                                string s = Misc.GetItemChoice(id, i);
                                if (string.IsNullOrEmpty(s))
                                {
                                    continue;
                                }
                                comboBox.Items.Add(s);
                                maxWidth = Math.Max(maxWidth,
                                    (int) g.MeasureString(s, Font).Width + SystemInformation.VerticalScrollBarWidth
                                    + comboBoxWidthMargin);
                                maxWidth = comboBoxWidthBase
                                           + (maxWidth - comboBoxWidthBase + (comboBoxWidthUnit - 1))
                                           / comboBoxWidthUnit * comboBoxWidthUnit;
                            }
                            comboBox.Size = new Size(maxWidth, comboBoxHeight);
                            comboBox.Location = new Point(editX - maxWidth, editY);
                            comboBox.DrawItem += OnItemComboBoxDrawItem;
                            comboBox.SelectedIndexChanged += OnItemComboBoxSelectedIndexChanged;
                            tabPage.Controls.Add(comboBox);
                            break;

                        default:
                            TextBox textBox = new TextBox
                            {
                                Size = new Size(textBoxWidth, textBoxHeight),
                                Location = new Point(editX - textBoxWidth, editY),
                                TextAlign = HorizontalAlignment.Right,
                                Tag = id
                            };
                            textBox.Validated += OnItemTextBoxValidated;
                            tabPage.Controls.Add(textBox);
                            break;
                    }
                    editY += itemHeight;
                }
                // 次の列との間を空ける
                labelX = editX + columnMargin;
            }
        }

        /// <summary>
        ///     編集項目を更新する
        /// </summary>
        /// <param name="tabPage">対象のタブページ</param>
        private static void UpdateEditableItems(TabPage tabPage)
        {
            foreach (Control control in tabPage.Controls)
            {
                // タグの設定されていないラベルをスキップする
                if (control.Tag == null)
                {
                    continue;
                }

                MiscItemId id = (MiscItemId) control.Tag;
                ComboBox comboBox;
                switch (Misc.ItemTypes[(int) id])
                {
                    case MiscItemType.None:
                        break;

                    case MiscItemType.Bool:
                        comboBox = control as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SelectedIndex = (bool) Misc.GetItem(id) ? 1 : 0;
                        }
                        break;

                    case MiscItemType.Enum:
                        comboBox = control as ComboBox;
                        if (comboBox != null)
                        {
                            comboBox.SelectedIndex = (int) Misc.GetItem(id) - Misc.IntMinValues[id];
                        }
                        break;

                    default:
                        TextBox textBox = control as TextBox;
                        if (textBox != null)
                        {
                            textBox.Text = Misc.GetString(id);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     編集項目の色を更新する
        /// </summary>
        /// <param name="tabPage">対象のタブページ</param>
        private static void UpdateItemColor(TabPage tabPage)
        {
            foreach (Control control in tabPage.Controls)
            {
                // タグの設定されていないラベルをスキップする
                if (control.Tag == null)
                {
                    continue;
                }

                MiscItemId id = (MiscItemId) control.Tag;
                switch (Misc.ItemTypes[(int) id])
                {
                    case MiscItemType.None:
                    case MiscItemType.Bool:
                    case MiscItemType.Enum:
                        break;

                    default:
                        TextBox textBox = control as TextBox;
                        if (textBox != null)
                        {
                            textBox.ForeColor = Misc.IsDirty(id) ? Color.Red : SystemColors.WindowText;
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     タブページ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiscTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tabPage = miscTabControl.SelectedTab;
            if (tabPage.Controls.Count == 0)
            {
                InitTabPage(tabPage);
            }

            // 選択中のタブページを保存する
            HoI2EditorController.Settings.MiscEditor.SelectedTab = miscTabControl.SelectedIndex;
        }

        #endregion

        #region 編集項目操作

        /// <summary>
        ///     編集項目テキストボックスフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTextBoxValidated(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            MiscItemId id = (MiscItemId) textBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            double d = 0;
            int i = 0;

            // 変更後の文字列を数値に変換できなければ値を戻す
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    if (!int.TryParse(textBox.Text, out i))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    if (!DoubleHelper.TryParse(textBox.Text, out d))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.None:
                case MiscItemType.Bool:
                case MiscItemType.Enum:
                    break;
            }

            // 設定範囲外の値ならば戻す
            switch (type)
            {
                case MiscItemType.PosInt:
                    if (i <= 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegInt:
                case MiscItemType.NonNegInt1:
                    if (i < 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonPosInt:
                    if (i > 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegIntMinusOne:
                    if ((i < 0) && (i != -1))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedInt:
                    if ((i < Misc.IntMinValues[id]) || (i > Misc.IntMaxValues[id]))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedPosInt:
                    if (i < Misc.IntMinValues[id])
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusOne:
                    if (((i < Misc.IntMinValues[id]) || (i > Misc.IntMaxValues[id])) && (i != -1))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedIntMinusThree:
                    if (((i < Misc.IntMinValues[id]) || (i > Misc.IntMaxValues[id])) && (i != -1) && (i != -2) &&
                        (i != -3))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.PosDbl:
                    if (d <= 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                    if (d < 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonPosDbl:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                    if (d > 0)
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                    if ((d < 0) && !DoubleHelper.IsEqual(d, -1))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDbl0:
                    if ((d < Misc.DblMinValues[id]) || (d > Misc.DblMaxValues[id]))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;

                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                    if (((d < Misc.DblMinValues[id]) || (d > Misc.DblMaxValues[id])) && !DoubleHelper.IsEqual(d, -1))
                    {
                        textBox.Text = Misc.GetString(id);
                        return;
                    }
                    break;
            }

            // 値に変化がなければ何もしない
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    if (i == (int) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    if (DoubleHelper.IsEqual(d, (double) Misc.GetItem(id)))
                    {
                        return;
                    }
                    break;
            }

            string old = Misc.GetString(id);

            // 値を更新する
            switch (type)
            {
                case MiscItemType.Int:
                case MiscItemType.PosInt:
                case MiscItemType.NonNegInt:
                case MiscItemType.NonPosInt:
                case MiscItemType.NonNegIntMinusOne:
                case MiscItemType.NonNegInt1:
                case MiscItemType.RangedInt:
                case MiscItemType.RangedPosInt:
                case MiscItemType.RangedIntMinusOne:
                case MiscItemType.RangedIntMinusThree:
                    Misc.SetItem(id, i);
                    break;

                case MiscItemType.Dbl:
                case MiscItemType.PosDbl:
                case MiscItemType.NonNegDbl:
                case MiscItemType.NonPosDbl:
                case MiscItemType.NonNegDbl0:
                case MiscItemType.NonNegDbl2:
                case MiscItemType.NonNegDbl5:
                case MiscItemType.NonPosDbl0:
                case MiscItemType.NonPosDbl2:
                case MiscItemType.NonNegDblMinusOne:
                case MiscItemType.NonNegDblMinusOne1:
                case MiscItemType.NonNegDbl2AoD:
                case MiscItemType.NonNegDbl4Dda13:
                case MiscItemType.NonNegDbl2Dh103Full:
                case MiscItemType.NonNegDbl2Dh103Full1:
                case MiscItemType.NonNegDbl2Dh103Full2:
                case MiscItemType.NonPosDbl5AoD:
                case MiscItemType.NonPosDbl2Dh103Full:
                case MiscItemType.RangedDbl:
                case MiscItemType.RangedDblMinusOne:
                case MiscItemType.RangedDblMinusOne1:
                case MiscItemType.RangedDbl0:
                case MiscItemType.NonNegIntNegDbl:
                    Misc.SetItem(id, d);
                    break;
            }

            Log.Info("[Misc] {0}: {1} -> {2}", Misc.GetItemName(id), old, Misc.GetString(id));

            // 編集済みフラグを設定する
            Misc.SetDirty(id);

            // 文字色を変更する
            textBox.ForeColor = Color.Red;

            // 他のフォームに更新を通知する
            NotifyItemChange(id);
        }

        /// <summary>
        ///     他のフォームに更新を通知する
        /// </summary>
        /// <param name="id">項目ID</param>
        private void NotifyItemChange(MiscItemId id)
        {
            switch (id)
            {
                case MiscItemId.TpMaxAttach: // 輸送艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.Transport].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.SsMaxAttach: // 潜水艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.Submarine].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.SsnMaxAttach: // 原子力潜水艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.NuclearSubmarine].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.DdMaxAttach: // 駆逐艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.Destroyer].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.ClMaxAttach: // 軽巡洋艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.LightCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.CaMaxAttach: // 重巡洋艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.HeavyCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.BcMaxAttach: // 巡洋戦艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.BattleCruiser].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.BbMaxAttach: // 戦艦最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.BattleShip].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.CvlMaxAttach: // 軽空母最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.EscortCarrier].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;

                case MiscItemId.CvMaxAttach: // 空母最大付属装備数
                    if (Units.IsLoaded())
                    {
                        Units.Items[(int) UnitType.Carrier].SetDirty(UnitClassItemId.MaxAllowedBrigades);
                    }
                    HoI2EditorController.OnItemChanged(EditorItemId.MaxAllowedBrigades, this);
                    break;
            }
        }

        /// <summary>
        ///     編集項目コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            MiscItemId id = (MiscItemId) comboBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            // 項目がなければ何もしない
            if (e.Index == -1)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            Brush brush;
            int index = 0;
            switch (type)
            {
                case MiscItemType.Bool:
                    index = (bool) Misc.GetItem(id) ? 1 : 0;
                    break;

                case MiscItemType.Enum:
                    index = (int) Misc.GetItem(id);
                    break;
            }
            if ((e.Index + Misc.IntMinValues[id] == index) && Misc.IsDirty(id))
            {
                brush = new SolidBrush(Color.Red);
            }
            else
            {
                brush = new SolidBrush(SystemColors.WindowText);
            }
            string s = comboBox.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     編集項目コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }
            MiscItemId id = (MiscItemId) comboBox.Tag;
            MiscItemType type = Misc.ItemTypes[(int) id];

            if (comboBox.SelectedIndex == -1)
            {
                return;
            }

            bool b = false;
            int i = 0;

            // 値に変化がなければ何もしない
            switch (type)
            {
                case MiscItemType.Bool:
                    b = comboBox.SelectedIndex == 1;
                    if (b == (bool) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;

                case MiscItemType.Enum:
                    i = comboBox.SelectedIndex + Misc.IntMinValues[id];
                    if (i == (int) Misc.GetItem(id))
                    {
                        return;
                    }
                    break;
            }

            // 値を更新する
            switch (type)
            {
                case MiscItemType.Bool:
                    Log.Info("[Misc] {0}: {1} -> {2}", Misc.GetItemName(id),
                        Misc.GetItemChoice(id, (bool) Misc.GetItem(id) ? 1 : 0), Misc.GetItemChoice(id, b ? 1 : 0));
                    Misc.SetItem(id, b);
                    break;

                case MiscItemType.Enum:
                    Log.Info("[Misc] {0}: {1} -> {2}", Misc.GetItemName(id),
                        Misc.GetItemChoice(id, (int) Misc.GetItem(id)), Misc.GetItemChoice(id, i));
                    Misc.SetItem(id, i);
                    break;
            }

            // 編集済みフラグを設定する
            Misc.SetDirty(id);

            // 項目色を変更するために描画更新する
            comboBox.Refresh();
        }

        #endregion
    }
}