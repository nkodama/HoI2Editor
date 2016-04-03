using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;
using HoI2Editor.Utilities;

namespace HoI2Editor.Pages
{
    /// <summary>
    ///     シナリオエディタの同盟タブ
    /// </summary>
    internal partial class ScenarioEditorAlliancePage : UserControl
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタコントローラ
        /// </summary>
        private readonly ScenarioEditorController _controller;

        /// <summary>
        ///     シナリオエディタのフォーム
        /// </summary>
        private readonly ScenarioEditorForm _form;

        /// <summary>
        ///     同盟国以外の国家リスト
        /// </summary>
        private List<Country> _allianceFreeCountries;

        /// <summary>
        ///     戦争国以外の国家リスト
        /// </summary>
        private List<Country> _warFreeCountries;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタコントローラ</param>
        /// <param name="form">シナリオエディタのフォーム</param>
        internal ScenarioEditorAlliancePage(ScenarioEditorController controller, ScenarioEditorForm form)
        {
            InitializeComponent();

            _controller = controller;
            _form = form;

            // 編集項目を初期化する
            InitAllianceItems();
            InitWarItems();
        }

        /// <summary>
        ///     同盟タブを初期化する
        /// </summary>
        internal void Init()
        {
            // 同盟リストを更新する
            UpdateAllianceList();

            // 戦争リストを更新する
            UpdateWarList();

            // 同盟リストを有効化する
            EnableAllianceList();

            // 戦争リストを有効化する
            EnableWarList();
        }

        #endregion

        #region 同盟

        /// <summary>
        ///     同盟の編集項目を初期化する
        /// </summary>
        private void InitAllianceItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.AllianceName, allianceNameTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.AllianceType, allianceTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.AllianceId, allianceIdTextBox);

            allianceNameTextBox.Tag = ScenarioEditorItemId.AllianceName;
            allianceTypeTextBox.Tag = ScenarioEditorItemId.AllianceType;
            allianceIdTextBox.Tag = ScenarioEditorItemId.AllianceId;
        }

        /// <summary>
        ///     同盟の編集項目を更新する
        /// </summary>
        /// <param name="alliance">同盟</param>
        private void UpdateAllianceItems(Alliance alliance)
        {
            // 編集項目の値を更新する
            _controller.UpdateItemValue(allianceNameTextBox, alliance);
            _controller.UpdateItemValue(allianceTypeTextBox, alliance);
            _controller.UpdateItemValue(allianceIdTextBox, alliance);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(allianceNameTextBox, alliance);
            _controller.UpdateItemColor(allianceTypeTextBox, alliance);
            _controller.UpdateItemColor(allianceIdTextBox, alliance);

            // 同盟参加国リストを更新する
            UpdateAllianceParticipant(alliance);
        }

        /// <summary>
        ///     同盟の編集項目をクリアする
        /// </summary>
        private void ClearAllianceItems()
        {
            // 編集項目をクリアする
            allianceNameTextBox.Text = "";
            allianceTypeTextBox.Text = "";
            allianceIdTextBox.Text = "";

            // 同盟参加国リストをクリアする
            ClearAllianceParticipant();
        }

        /// <summary>
        ///     同盟の編集項目を有効化する
        /// </summary>
        private void EnableAllianceItems()
        {
            int index = allianceListView.SelectedIndices[0];

            // 枢軸国/連合国/共産国以外は名前変更できない
            allianceNameLabel.Enabled = index < 3;
            allianceNameTextBox.Enabled = index < 3;
            allianceIdLabel.Enabled = true;
            allianceTypeTextBox.Enabled = true;
            allianceIdTextBox.Enabled = true;

            // 同盟参加国リストを有効化する
            EnableAllianceParticipant();
        }

        /// <summary>
        ///     同盟の編集項目を無効化する
        /// </summary>
        private void DisableAllianceItems()
        {
            allianceNameLabel.Enabled = false;
            allianceNameTextBox.Enabled = false;
            allianceIdLabel.Enabled = false;
            allianceTypeTextBox.Enabled = false;
            allianceIdTextBox.Enabled = false;

            // 同盟参加国リストを無効化する
            DisableAllianceParticipant();
        }

        #endregion

        #region 同盟リスト

        /// <summary>
        ///     同盟リストを更新する
        /// </summary>
        private void UpdateAllianceList()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            allianceListView.BeginUpdate();
            allianceListView.Items.Clear();

            // 枢軸国
            ListViewItem item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Axis),
                Tag = data.Axis
            };
            item.SubItems.Add(data.Axis != null ? Countries.GetNameList(data.Axis.Participant) : "");
            allianceListView.Items.Add(item);

            // 連合国
            item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Allies),
                Tag = data.Allies
            };
            item.SubItems.Add(data.Allies != null ? Countries.GetNameList(data.Allies.Participant) : "");
            allianceListView.Items.Add(item);

            // 共産国
            item = new ListViewItem
            {
                Text = (string) _controller.GetItemValue(ScenarioEditorItemId.AllianceName, data.Comintern),
                Tag = data.Comintern
            };
            item.SubItems.Add(data.Comintern != null ? Countries.GetNameList(data.Comintern.Participant) : "");
            allianceListView.Items.Add(item);

            // その他の同盟
            foreach (Alliance alliance in data.Alliances)
            {
                item = new ListViewItem
                {
                    Text = Resources.Alliance,
                    Tag = alliance
                };
                item.SubItems.Add(Countries.GetNameList(alliance.Participant));
                allianceListView.Items.Add(item);
            }

            allianceListView.EndUpdate();

            // 同盟操作ボタンを無効化する
            DisableAllianceItemButtons();

            // 同盟参加国操作ボタンを無効化する
            DisableAllianceParticipantButtons();

            // 編集項目を無効化する
            DisableAllianceItems();

            // 編集項目をクリアする
            ClearAllianceItems();
        }

        /// <summary>
        ///     同盟リストを有効化する
        /// </summary>
        private void EnableAllianceList()
        {
            allianceGroupBox.Enabled = true;
        }

        /// <summary>
        ///     同盟操作ボタンを有効化する
        /// </summary>
        private void EnableAllianceItemButtons()
        {
            int count = allianceListView.Items.Count;
            int index = allianceListView.SelectedIndices[0];

            // 枢軸国/連合国/共産国は順番変更/削除できない
            allianceUpButton.Enabled = index > 3;
            allianceDownButton.Enabled = (index < count - 1) && (index >= 3);
            allianceRemoveButton.Enabled = index >= 3;
        }

        /// <summary>
        ///     同盟操作ボタンを無効化する
        /// </summary>
        private void DisableAllianceItemButtons()
        {
            allianceUpButton.Enabled = false;
            allianceDownButton.Enabled = false;
            allianceRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     同盟リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 同盟参加国操作ボタンを無効化する
            DisableAllianceParticipantButtons();

            // 選択項目がなければ編集項目を無効化する
            if (allianceListView.SelectedItems.Count == 0)
            {
                // 同盟操作ボタンを無効化する
                DisableAllianceItemButtons();

                // 編集項目を無効化する
                DisableAllianceItems();

                // 編集項目をクリアする
                ClearAllianceItems();
                return;
            }

            Alliance alliance = GetSelectedAlliance();

            // 編集項目を更新する
            UpdateAllianceItems(alliance);

            // 編集項目を有効化する
            EnableAllianceItems();

            // 同盟操作ボタンを有効化する
            EnableAllianceItemButtons();
        }

        /// <summary>
        ///     同盟の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストビューの項目を移動する
            int index = allianceListView.SelectedIndices[0];
            ListViewItem item = allianceListView.Items[index];
            allianceListView.Items.RemoveAt(index);
            allianceListView.Items.Insert(index - 1, item);
            allianceListView.Items[index - 1].Focused = true;
            allianceListView.Items[index - 1].Selected = true;
            allianceListView.EnsureVisible(index - 1);

            // 同盟リストの項目を移動する
            index -= 3; // -3は枢軸国/連合国/共産国の分
            Alliance alliance = alliances[index];
            alliances.RemoveAt(index);
            alliances.Insert(index - 1, alliance);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     同盟の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストビューの項目を移動する
            int index = allianceListView.SelectedIndices[0];
            ListViewItem item = allianceListView.Items[index];
            allianceListView.Items.RemoveAt(index);
            allianceListView.Items.Insert(index + 1, item);
            allianceListView.Items[index + 1].Focused = true;
            allianceListView.Items[index + 1].Selected = true;
            allianceListView.EnsureVisible(index + 1);

            // 同盟リストの項目を移動する
            index -= 3; // -3は枢軸国/連合国/共産国の分
            Alliance alliance = alliances[index];
            alliances.RemoveAt(index);
            alliances.Insert(index + 1, alliance);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     同盟の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 同盟リストに項目を追加する
            Alliance alliance = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultAllianceType, 1) };
            alliances.Add(alliance);

            // 同盟リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Text = Resources.Alliance, Tag = alliance };
            item.SubItems.Add("");
            allianceListView.Items.Add(item);

            Log.Info("[Scenario] alliance added ({0})", allianceListView.Items.Count - 4);

            // 編集済みフラグを設定する
            alliance.SetDirty(Alliance.ItemId.Type);
            alliance.SetDirty(Alliance.ItemId.Id);
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (allianceListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = allianceListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     同盟の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Alliance> alliances = scenario.GlobalData.Alliances;

            // 枢軸国/連合国/共産国は削除できない
            int index = allianceListView.SelectedIndices[0] - 3;
            if (index < 0)
            {
                return;
            }

            Alliance alliance = alliances[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(alliance.Id);

            // 同盟リストから項目を削除する
            alliances.RemoveAt(index);

            // 同盟リストビューから項目を削除する
            allianceListView.Items.RemoveAt(index + 3);

            Log.Info("[Scenario] alliance removed ({0})", index);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            index += index < alliances.Count ? 3 : 3 - 1;
            allianceListView.Items[index].Focused = true;
            allianceListView.Items[index].Selected = true;
        }


        /// <summary>
        ///     同盟リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetAllianceListItemText(int no, string s)
        {
            allianceListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     選択中の同盟情報を取得する
        /// </summary>
        /// <returns>選択中の同盟情報</returns>
        private Alliance GetSelectedAlliance()
        {
            return allianceListView.SelectedItems.Count > 0 ? allianceListView.SelectedItems[0].Tag as Alliance : null;
        }

        #endregion

        #region 同盟参加国

        /// <summary>
        ///     同盟参加国リストを更新する
        /// </summary>
        /// <param name="alliance">同盟</param>
        private void UpdateAllianceParticipant(Alliance alliance)
        {
            // 同盟参加国
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.Clear();
            foreach (Country country in alliance.Participant)
            {
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceParticipantListBox.EndUpdate();

            // 同盟非参加国
            _allianceFreeCountries = Countries.Tags.Where(country => !alliance.Participant.Contains(country)).ToList();
            allianceFreeCountryListBox.BeginUpdate();
            allianceFreeCountryListBox.Items.Clear();
            foreach (Country country in _allianceFreeCountries)
            {
                allianceFreeCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            allianceFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟参加国リストをクリアする
        /// </summary>
        private void ClearAllianceParticipant()
        {
            allianceParticipantListBox.Items.Clear();
            allianceFreeCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     同盟参加国リストを有効化する
        /// </summary>
        private void EnableAllianceParticipant()
        {
            allianceParticipantLabel.Enabled = true;
            allianceParticipantListBox.Enabled = true;
            allianceFreeCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     同盟参加国リストを無効化する
        /// </summary>
        private void DisableAllianceParticipant()
        {
            allianceParticipantLabel.Enabled = false;
            allianceParticipantListBox.Enabled = false;
            allianceFreeCountryListBox.Enabled = false;
        }

        /// <summary>
        ///     同盟参加国操作ボタンを無効化する
        /// </summary>
        private void DisableAllianceParticipantButtons()
        {
            allianceParticipantAddButton.Enabled = false;
            allianceParticipantRemoveButton.Enabled = false;
            allianceLeaderButton.Enabled = false;
        }

        /// <summary>
        ///     同盟参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ListBox control = sender as ListBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = alliance.IsDirtyCountry(alliance.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     同盟非参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ListBox control = sender as ListBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = alliance.IsDirtyCountry(_allianceFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     同盟参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceParticipantListBox.SelectedIndices.Count;
            int index = allianceParticipantListBox.SelectedIndex;

            allianceParticipantRemoveButton.Enabled = count > 0;
            allianceLeaderButton.Enabled = (count == 1) && (index > 0);
        }

        /// <summary>
        ///     同盟非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = allianceFreeCountryListBox.SelectedIndices.Count;

            allianceParticipantAddButton.Enabled = count > 0;
        }

        /// <summary>
        ///     同盟参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in allianceFreeCountryListBox.SelectedIndices select _allianceFreeCountries[index])
                    .ToList();
            allianceParticipantListBox.BeginUpdate();
            allianceFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟参加国リストボックスに追加する
                allianceParticipantListBox.Items.Add(Countries.GetTagName(country));

                // 同盟参加国リストに追加する
                alliance.Participant.Add(country);

                // 同盟非参加国リストボックスから削除する
                int index = _allianceFreeCountries.IndexOf(country);
                allianceFreeCountryListBox.Items.RemoveAt(index);
                _allianceFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                alliance.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 同盟リストビューの項目を更新する
                allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

                Log.Info("[Scenario] alliance participant: +{0} ({1})", Countries.Strings[(int) country],
                    allianceListView.SelectedIndices[0]);
            }
            allianceParticipantListBox.EndUpdate();
            allianceFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceParticipantRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in allianceParticipantListBox.SelectedIndices select alliance.Participant[index])
                    .ToList();
            allianceParticipantListBox.BeginUpdate();
            allianceFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 同盟非参加国リストボックスに追加する
                int index = _allianceFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _allianceFreeCountries.Count;
                }
                allianceFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _allianceFreeCountries.Insert(index, country);

                // 同盟参加国リストボックスから削除する
                index = alliance.Participant.IndexOf(country);
                allianceParticipantListBox.Items.RemoveAt(index);

                // 同盟参加国リストから削除する
                alliance.Participant.Remove(country);

                // 編集済みフラグを設定する
                alliance.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 同盟リストビューの項目を更新する
                allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

                Log.Info("[Scenario] alliance participant: -{0} ({1})", Countries.Strings[(int) country],
                    allianceListView.SelectedIndices[0]);
            }
            allianceParticipantListBox.EndUpdate();
            allianceFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     同盟のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            int index = allianceParticipantListBox.SelectedIndex;
            Country country = alliance.Participant[index];

            // 同盟参加国リストボックスの先頭に移動する
            allianceParticipantListBox.BeginUpdate();
            allianceParticipantListBox.Items.RemoveAt(index);
            allianceParticipantListBox.Items.Insert(0, Countries.GetTagName(country));
            allianceParticipantListBox.EndUpdate();

            // 同盟参加国リストの先頭に移動する
            alliance.Participant.RemoveAt(index);
            alliance.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            alliance.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 同盟リストビューの項目を更新する
            allianceListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(alliance.Participant);

            Log.Info("[Scenario] alliance leader: {0} ({1})", Countries.Strings[(int) country],
                allianceListView.SelectedIndices[0]);
        }

        #endregion

        #region 戦争

        /// <summary>
        ///     戦争の編集項目を初期化する
        /// </summary>
        private void InitWarItems()
        {
            _form._itemControls.Add(ScenarioEditorItemId.WarStartYear, warStartYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarStartMonth, warStartMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarStartDay, warStartDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarEndYear, warEndYearTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarEndMonth, warEndMonthTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarEndDay, warEndDayTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarType, warTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarId, warIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarAttackerType, warAttackerTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarAttackerId, warAttackerIdTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarDefenderType, warDefenderTypeTextBox);
            _form._itemControls.Add(ScenarioEditorItemId.WarDefenderId, warDefenderIdTextBox);

            warStartYearTextBox.Tag = ScenarioEditorItemId.WarStartYear;
            warStartMonthTextBox.Tag = ScenarioEditorItemId.WarStartMonth;
            warStartDayTextBox.Tag = ScenarioEditorItemId.WarStartDay;
            warEndYearTextBox.Tag = ScenarioEditorItemId.WarEndYear;
            warEndMonthTextBox.Tag = ScenarioEditorItemId.WarEndMonth;
            warEndDayTextBox.Tag = ScenarioEditorItemId.WarEndDay;
            warTypeTextBox.Tag = ScenarioEditorItemId.WarType;
            warIdTextBox.Tag = ScenarioEditorItemId.WarId;
            warAttackerTypeTextBox.Tag = ScenarioEditorItemId.WarAttackerType;
            warAttackerIdTextBox.Tag = ScenarioEditorItemId.WarAttackerId;
            warDefenderTypeTextBox.Tag = ScenarioEditorItemId.WarDefenderType;
            warDefenderIdTextBox.Tag = ScenarioEditorItemId.WarDefenderId;
        }

        /// <summary>
        ///     戦争の編集項目を更新する
        /// </summary>
        /// <param name="war">戦争</param>
        private void UpdateWarItems(War war)
        {
            // 編集項目の値を更新する
            _controller.UpdateItemValue(warStartYearTextBox, war);
            _controller.UpdateItemValue(warStartMonthTextBox, war);
            _controller.UpdateItemValue(warStartDayTextBox, war);
            _controller.UpdateItemValue(warEndYearTextBox, war);
            _controller.UpdateItemValue(warEndMonthTextBox, war);
            _controller.UpdateItemValue(warEndDayTextBox, war);
            _controller.UpdateItemValue(warTypeTextBox, war);
            _controller.UpdateItemValue(warIdTextBox, war);
            _controller.UpdateItemValue(warAttackerTypeTextBox, war);
            _controller.UpdateItemValue(warAttackerIdTextBox, war);
            _controller.UpdateItemValue(warDefenderTypeTextBox, war);
            _controller.UpdateItemValue(warDefenderIdTextBox, war);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(warStartYearTextBox, war);
            _controller.UpdateItemColor(warStartMonthTextBox, war);
            _controller.UpdateItemColor(warStartDayTextBox, war);
            _controller.UpdateItemColor(warEndYearTextBox, war);
            _controller.UpdateItemColor(warEndMonthTextBox, war);
            _controller.UpdateItemColor(warEndDayTextBox, war);
            _controller.UpdateItemColor(warTypeTextBox, war);
            _controller.UpdateItemColor(warIdTextBox, war);
            _controller.UpdateItemColor(warAttackerTypeTextBox, war);
            _controller.UpdateItemColor(warAttackerIdTextBox, war);
            _controller.UpdateItemColor(warDefenderTypeTextBox, war);
            _controller.UpdateItemColor(warDefenderIdTextBox, war);

            // 戦争参加国リストを更新する
            UpdateWarParticipant(war);
        }

        /// <summary>
        ///     戦争の編集項目をクリアする
        /// </summary>
        private void ClearWarItems()
        {
            // 編集項目をクリアする
            warStartYearTextBox.Text = "";
            warStartMonthTextBox.Text = "";
            warStartDayTextBox.Text = "";
            warEndYearTextBox.Text = "";
            warEndMonthTextBox.Text = "";
            warEndDayTextBox.Text = "";
            warTypeTextBox.Text = "";
            warIdTextBox.Text = "";
            warAttackerTypeTextBox.Text = "";
            warAttackerIdTextBox.Text = "";
            warDefenderTypeTextBox.Text = "";
            warDefenderIdTextBox.Text = "";

            // 戦争参加国リストをクリアする
            ClearWarParticipant();
        }

        /// <summary>
        ///     戦争の編集項目を有効化する
        /// </summary>
        private void EnableWarItems()
        {
            warStartDateLabel.Enabled = true;
            warStartYearTextBox.Enabled = true;
            warStartMonthTextBox.Enabled = true;
            warStartDayTextBox.Enabled = true;
            warEndDateLabel.Enabled = true;
            warEndYearTextBox.Enabled = true;
            warEndMonthTextBox.Enabled = true;
            warEndDayTextBox.Enabled = true;
            warIdLabel.Enabled = true;
            warTypeTextBox.Enabled = true;
            warIdTextBox.Enabled = true;
            warAttackerIdLabel.Enabled = true;
            warAttackerTypeTextBox.Enabled = true;
            warAttackerIdTextBox.Enabled = true;
            warDefenderIdLabel.Enabled = true;
            warDefenderTypeTextBox.Enabled = true;
            warDefenderIdTextBox.Enabled = true;

            // 戦争参加国リストを有効化する
            EnableWarParticipant();
        }

        /// <summary>
        ///     戦争の編集項目を無効化する
        /// </summary>
        private void DisableWarItems()
        {
            warStartDateLabel.Enabled = false;
            warStartYearTextBox.Enabled = false;
            warStartMonthTextBox.Enabled = false;
            warStartDayTextBox.Enabled = false;
            warEndDateLabel.Enabled = false;
            warEndYearTextBox.Enabled = false;
            warEndMonthTextBox.Enabled = false;
            warEndDayTextBox.Enabled = false;
            warIdLabel.Enabled = false;
            warTypeTextBox.Enabled = false;
            warIdTextBox.Enabled = false;
            warAttackerIdLabel.Enabled = false;
            warAttackerTypeTextBox.Enabled = false;
            warAttackerIdTextBox.Enabled = false;
            warDefenderIdLabel.Enabled = false;
            warDefenderTypeTextBox.Enabled = false;
            warDefenderIdTextBox.Enabled = false;

            // 戦争参加国リストを無効化する
            DisableWarParticipant();
        }

        #endregion

        #region 戦争リスト

        /// <summary>
        ///     戦争リストビューを更新する
        /// </summary>
        private void UpdateWarList()
        {
            ScenarioGlobalData data = Scenarios.Data.GlobalData;

            warListView.BeginUpdate();
            warListView.Items.Clear();
            foreach (War war in data.Wars)
            {
                ListViewItem item = new ListViewItem
                {
                    Text = Countries.GetNameList(war.Attackers.Participant),
                    Tag = war
                };
                item.SubItems.Add(Countries.GetNameList(war.Defenders.Participant));
                warListView.Items.Add(item);
            }
            warListView.EndUpdate();

            // 戦争操作ボタンを無効化する
            DisableWarItemButtons();

            // 戦争参加国操作ボタンを無効化する
            DisableWarParticipantButtons();

            // 編集項目を無効化する
            DisableWarItems();

            // 編集項目をクリアする
            ClearWarItems();
        }

        /// <summary>
        ///     戦争リストを有効化する
        /// </summary>
        private void EnableWarList()
        {
            warGroupBox.Enabled = true;
        }

        /// <summary>
        ///     戦争操作ボタンを有効化する
        /// </summary>
        private void EnableWarItemButtons()
        {
            int count = warListView.Items.Count;
            int index = warListView.SelectedIndices[0];

            warUpButton.Enabled = index > 0;
            warDownButton.Enabled = index < count - 1;
            warRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     戦争操作ボタンを有効化する
        /// </summary>
        private void DisableWarItemButtons()
        {
            warUpButton.Enabled = false;
            warDownButton.Enabled = false;
            warRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     戦争リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 戦争参加国操作ボタンを無効化する
            DisableWarParticipantButtons();

            // 選択項目がなければ編集項目を無効化する
            if (warListView.SelectedItems.Count == 0)
            {
                // 戦争操作ボタンを無効化する
                DisableWarItemButtons();

                // 編集項目を無効化する
                DisableWarItems();

                // 編集項目をクリアする
                ClearWarItems();
                return;
            }

            War war = GetSelectedWar();

            // 編集項目を更新する
            UpdateWarItems(war);

            // 編集項目を有効化する
            EnableWarItems();

            // 戦争操作ボタンを有効化する
            EnableWarItemButtons();
        }

        /// <summary>
        ///     戦争の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストビューの項目を移動する
            int index = warListView.SelectedIndices[0];
            ListViewItem item = warListView.Items[index];
            warListView.Items.RemoveAt(index);
            warListView.Items.Insert(index - 1, item);
            warListView.Items[index - 1].Focused = true;
            warListView.Items[index - 1].Selected = true;
            warListView.EnsureVisible(index - 1);

            // 戦争リストの項目を移動する
            War war = wars[index];
            wars.RemoveAt(index);
            wars.Insert(index - 1, war);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     戦争の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストビューの項目を移動する
            int index = warListView.SelectedIndices[0];
            ListViewItem item = warListView.Items[index];
            warListView.Items.RemoveAt(index);
            warListView.Items.Insert(index + 1, item);
            warListView.Items[index + 1].Focused = true;
            warListView.Items[index + 1].Selected = true;
            warListView.EnsureVisible(index + 1);

            // 戦争リストの項目を移動する
            War war = wars[index];
            wars.RemoveAt(index);
            wars.Insert(index + 1, war);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     戦争の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 戦争リストに項目を追加する
            War war = new War
            {
                StartDate = new GameDate(),
                EndDate = new GameDate(),
                Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1),
                Attackers = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1) },
                Defenders = new Alliance { Id = Scenarios.GetNewTypeId(Scenarios.DefaultWarType, 1) }
            };
            wars.Add(war);

            // 戦争リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Tag = war };
            item.SubItems.Add("");
            warListView.Items.Add(item);

            Log.Info("[Scenario] war added ({0})", warListView.Items.Count - 1);

            // 編集済みフラグを設定する
            war.SetDirty(War.ItemId.StartYear);
            war.SetDirty(War.ItemId.StartMonth);
            war.SetDirty(War.ItemId.StartDay);
            war.SetDirty(War.ItemId.EndYear);
            war.SetDirty(War.ItemId.EndMonth);
            war.SetDirty(War.ItemId.EndDay);
            war.SetDirty(War.ItemId.Type);
            war.SetDirty(War.ItemId.Id);
            war.SetDirty(War.ItemId.AttackerType);
            war.SetDirty(War.ItemId.AttackerId);
            war.SetDirty(War.ItemId.DefenderType);
            war.SetDirty(War.ItemId.DefenderId);
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (warListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = warListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     戦争の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<War> wars = scenario.GlobalData.Wars;

            // 枢軸国/連合国/共産国は削除できない
            int index = warListView.SelectedIndices[0];
            if (index < 0)
            {
                return;
            }

            War war = wars[index];

            Log.Info("[Scenario] war removed ({0})", index);

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(war.Id);

            // 戦争リストから項目を削除する
            wars.RemoveAt(index);

            // 戦争リストビューから項目を削除する
            warListView.Items.RemoveAt(index);

            // 編集済みフラグを設定する
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            if (index >= wars.Count)
            {
                index--;
            }
            if (index >= 0)
            {
                warListView.Items[index].Focused = true;
                warListView.Items[index].Selected = true;
            }
        }

        /// <summary>
        ///     選択中の戦争情報を取得する
        /// </summary>
        /// <returns>選択中の戦争情報</returns>
        private War GetSelectedWar()
        {
            return warListView.SelectedItems.Count > 0 ? warListView.SelectedItems[0].Tag as War : null;
        }

        #endregion

        #region 戦争参加国

        /// <summary>
        ///     戦争参加国リストを更新する
        /// </summary>
        /// <param name="war"></param>
        private void UpdateWarParticipant(War war)
        {
            IEnumerable<Country> countries = Countries.Tags;

            // 攻撃側参加国
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.Clear();
            if (war.Attackers?.Participant != null)
            {
                foreach (Country country in war.Attackers.Participant)
                {
                    warAttackerListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Attackers.Participant.Contains(country));
            }
            warAttackerListBox.EndUpdate();

            // 防御側参加国
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.Clear();
            if (war.Defenders?.Participant != null)
            {
                foreach (Country country in war.Defenders.Participant)
                {
                    warDefenderListBox.Items.Add(Countries.GetTagName(country));
                }
                countries = countries.Where(country => !war.Defenders.Participant.Contains(country));
            }
            warDefenderListBox.EndUpdate();

            // 戦争非参加国
            _warFreeCountries = countries.ToList();
            warFreeCountryListBox.BeginUpdate();
            warFreeCountryListBox.Items.Clear();
            foreach (Country country in _warFreeCountries)
            {
                warFreeCountryListBox.Items.Add(Countries.GetTagName(country));
            }
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争参加国リストをクリアする
        /// </summary>
        private void ClearWarParticipant()
        {
            warAttackerListBox.Items.Clear();
            warDefenderListBox.Items.Clear();
            warFreeCountryListBox.Items.Clear();
        }

        /// <summary>
        ///     戦争参加国リストを有効化する
        /// </summary>
        private void EnableWarParticipant()
        {
            warAttackerLabel.Enabled = true;
            warAttackerListBox.Enabled = true;
            warDefenderLabel.Enabled = true;
            warDefenderListBox.Enabled = true;
            warFreeCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     戦争参加国リストを無効化する
        /// </summary>
        private void DisableWarParticipant()
        {
            warAttackerLabel.Enabled = false;
            warAttackerListBox.Enabled = false;
            warDefenderLabel.Enabled = false;
            warDefenderListBox.Enabled = false;
            warFreeCountryListBox.Enabled = false;
        }

        /// <summary>
        ///     戦争参加国操作ボタンを無効化する
        /// </summary>
        private void DisableWarParticipantButtons()
        {
            warAttackerAddButton.Enabled = false;
            warAttackerRemoveButton.Enabled = false;
            warAttackerLeaderButton.Enabled = false;
            warDefenderAddButton.Enabled = false;
            warDefenderRemoveButton.Enabled = false;
            warDefenderLeaderButton.Enabled = false;
        }

        /// <summary>
        ///     戦争攻撃側参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ListBox control = sender as ListBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(war.Attackers.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争防御側参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }
            ListBox control = sender as ListBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(war.Defenders.Participant[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争非参加国リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ListBox control = sender as ListBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                bool dirty = war.IsDirtyCountry(_warFreeCountries[e.Index]);
                brush = new SolidBrush(dirty ? Color.Red : control.ForeColor);
            }
            else
            {
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     戦争攻撃側参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warAttackerListBox.SelectedIndices.Count;
            int index = warAttackerListBox.SelectedIndex;

            warAttackerRemoveButton.Enabled = count > 0;
            warAttackerLeaderButton.Enabled = (count == 1) && (index > 0);
        }

        /// <summary>
        ///     戦争防御側参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warDefenderListBox.SelectedIndices.Count;
            int index = warDefenderListBox.SelectedIndex;

            warDefenderRemoveButton.Enabled = count > 0;
            warDefenderLeaderButton.Enabled = (count == 1) && (index > 0);
        }

        /// <summary>
        ///     戦争非参加国リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int count = warFreeCountryListBox.SelectedIndices.Count;

            warAttackerAddButton.Enabled = count > 0;
            warDefenderAddButton.Enabled = count > 0;
        }

        /// <summary>
        ///     戦争攻撃側参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warFreeCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warAttackerListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争攻撃側参加国リストボックスに追加する
                warAttackerListBox.Items.Add(Countries.GetTagName(country));

                // 戦争攻撃側参加国リストに追加する
                war.Attackers.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warFreeCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争攻撃側参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warAttackerListBox.SelectedIndices select war.Attackers.Participant[index]).ToList();
            warAttackerListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _warFreeCountries.Insert(index, country);

                // 戦争攻撃側参加国リストボックスから削除する
                index = war.Attackers.Participant.IndexOf(country);
                warAttackerListBox.Items.RemoveAt(index);

                // 戦争攻撃側参加国リストから削除する
                war.Attackers.Participant.Remove(country);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

                Log.Info("[Scenario] war attacker: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warAttackerListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争防御側参加国の追加ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderAddButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warFreeCountryListBox.SelectedIndices select _warFreeCountries[index]).ToList();
            warDefenderListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争防御側参加国リストボックスに追加する
                warDefenderListBox.Items.Add(Countries.GetTagName(country));

                // 戦争防御側参加国リストに追加する
                war.Defenders.Participant.Add(country);

                // 戦争非参加国リストボックスから削除する
                int index = _warFreeCountries.IndexOf(country);
                warFreeCountryListBox.Items.RemoveAt(index);
                _warFreeCountries.RemoveAt(index);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: +{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争防御側参加国の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderRemoveButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            List<Country> countries =
                (from int index in warDefenderListBox.SelectedIndices select war.Defenders.Participant[index]).ToList();
            warDefenderListBox.BeginUpdate();
            warFreeCountryListBox.BeginUpdate();
            foreach (Country country in countries)
            {
                // 戦争非参加国リストボックスに追加する
                int index = _warFreeCountries.FindIndex(c => c > country);
                if (index < 0)
                {
                    index = _warFreeCountries.Count;
                }
                warFreeCountryListBox.Items.Insert(index, Countries.GetTagName(country));
                _warFreeCountries.Insert(index, country);

                // 戦争防御側参加国リストボックスから削除する
                index = war.Defenders.Participant.IndexOf(country);
                warDefenderListBox.Items.RemoveAt(index);

                // 戦争防御側参加国リストから削除する
                war.Defenders.Participant.Remove(country);

                // 編集済みフラグを設定する
                war.SetDirtyCountry(country);
                Scenarios.SetDirty();

                // 戦争リストビューの項目を更新する
                warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

                Log.Info("[Scenario] war defender: -{0} ({1})", Countries.Strings[(int) country],
                    warListView.SelectedIndices[0]);
            }
            warDefenderListBox.EndUpdate();
            warFreeCountryListBox.EndUpdate();
        }

        /// <summary>
        ///     戦争攻撃側のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarAttackerLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            int index = warAttackerListBox.SelectedIndex;
            Country country = war.Attackers.Participant[index];

            // 戦争攻撃側参加国リストボックスの先頭に移動する
            warAttackerListBox.BeginUpdate();
            warAttackerListBox.Items.RemoveAt(index);
            warAttackerListBox.Items.Insert(0, Countries.GetTagName(country));
            warAttackerListBox.EndUpdate();

            // 戦争攻撃側参加国リストの先頭に移動する
            war.Attackers.Participant.RemoveAt(index);
            war.Attackers.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            war.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 戦争リストビューの項目を更新する
            warListView.SelectedItems[0].SubItems[0].Text = Countries.GetNameList(war.Attackers.Participant);

            Log.Info("[Scenario] war attacker leader: {0} ({1})", Countries.Strings[(int) country],
                warListView.SelectedIndices[0]);
        }

        /// <summary>
        ///     戦争防御側のリーダーに設定ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarDefenderLeaderButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            int index = warDefenderListBox.SelectedIndex;
            Country country = war.Defenders.Participant[index];

            // 戦争防御側参加国リストボックスの先頭に移動する
            warDefenderListBox.BeginUpdate();
            warDefenderListBox.Items.RemoveAt(index);
            warDefenderListBox.Items.Insert(0, Countries.GetTagName(country));
            warDefenderListBox.EndUpdate();

            // 戦争防御側参加国リストの先頭に移動する
            war.Defenders.Participant.RemoveAt(index);
            war.Defenders.Participant.Insert(0, country);

            // 編集済みフラグを設定する
            war.SetDirtyCountry(country);
            Scenarios.SetDirty();

            // 戦争リストビューの項目を更新する
            warListView.SelectedItems[0].SubItems[1].Text = Countries.GetNameList(war.Defenders.Participant);

            Log.Info("[Scenario] war defender leader: {0} ({1})", Countries.Strings[(int) country],
                warListView.SelectedIndices[0]);
        }

        #endregion

        #region 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, alliance);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, alliance);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, alliance);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, alliance, allianceListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, alliance);

            // 値を更新する
            _controller.SetItemValue(itemId, val, alliance);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, alliance);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, alliance);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWarIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            War war = GetSelectedWar();
            if (war == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 文字列を数値に変換できなければ値を戻す
            int val;
            if (!IntHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, war);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, war);
            if ((prev == null) && (val == 0))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && (val == (int) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val))
            {
                _controller.UpdateItemValue(control, war);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, war, warListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, war);

            // 値を更新する
            _controller.SetItemValue(itemId, val, war);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, war);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, war);
        }

        /// <summary>
        ///     テキストボックスの値変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllianceStringItemTextBoxTextChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Alliance alliance = GetSelectedAlliance();
            if (alliance == null)
            {
                return;
            }

            TextBox control = sender as TextBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            string val = control.Text;
            if (val.Equals((string) _controller.GetItemValue(itemId, alliance)))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, alliance, allianceListView.SelectedIndices[0]);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, alliance);

            // 値を更新する
            _controller.SetItemValue(itemId, val, alliance);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, alliance);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, alliance);
        }

        #endregion
    }
}