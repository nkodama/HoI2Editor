using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Models;
using HoI2Editor.Pages;
using HoI2Editor.Utilities;

namespace HoI2Editor.Forms
{
    /// <summary>
    ///     シナリオエディタフォーム
    /// </summary>
    internal partial class ScenarioEditorForm : Form
    {
        #region 内部フィールド

        /// <summary>
        ///     シナリオエディタのコントローラ
        /// </summary>
        private readonly ScenarioEditorController _controller;

        /// <summary>
        ///     タブページ番号
        /// </summary>
        private TabPageNo _tabPageNo;

        /// <summary>
        ///     タブページの初期化フラグ
        /// </summary>
        private readonly bool[] _tabPageInitialized = new bool[Enum.GetValues(typeof (TabPageNo)).Length];

        /// <summary>
        ///     メインタブ
        /// </summary>
        private ScenarioEditorMainPage _mainPage;

        /// <summary>
        ///     同盟タブ
        /// </summary>
        private ScenarioEditorAlliancePage _alliancePage;

        /// <summary>
        ///     関係ページ
        /// </summary>
        private ScenarioEditorRelationPage _relationPage;

        /// <summary>
        ///     国家タブ
        /// </summary>
        private ScenarioEditorCountryPage _countryPage;

        /// <summary>
        ///     政府タブ
        /// </summary>
        private ScenarioEditorGovernmentPage _governmentPage;

        /// <summary>
        ///     プロヴィンスタブ
        /// </summary>
        private ScenarioEditorProvincePage _provincePage;

        /// <summary>
        ///     初期部隊タブ
        /// </summary>
        private ScenarioEditorOobPage _oobPage;

        /// <summary>
        ///     編集項目IDとコントロールの関連付け
        /// </summary>
        internal readonly Dictionary<ScenarioEditorItemId, Control> _itemControls =
            new Dictionary<ScenarioEditorItemId, Control>();

        /// <summary>
        ///     技術項目リスト
        /// </summary>
        private List<TechItem> _techs;

        /// <summary>
        ///     発明イベントリスト
        /// </summary>
        private List<TechEvent> _inventions;

        /// <summary>
        ///     技術ツリーパネルのコントローラ
        /// </summary>
        private TechTreePanelController _techTreePanelController;

        #endregion

        #region 内部定数

        /// <summary>
        ///     タブページ番号
        /// </summary>
        private enum TabPageNo
        {
            Main, // メイン
            Alliance, // 同盟
            Relation, // 関係
            Trade, // 貿易
            Country, // 国家
            Government, // 政府
            Technology, // 技術
            Province, // プロヴィンス
            Oob // 初期部隊
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="controller">シナリオエディタコントローラ</param>
        internal ScenarioEditorForm(ScenarioEditorController controller)
        {
            InitializeComponent();

            _controller = controller;

            // フォームの初期化
            InitForm();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        internal void OnFileLoaded()
        {
            // 読み込み前ならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // シナリオ関連情報を初期化する
            Scenarios.Init();

            // 各タブページの初期化済み状態をクリアする
            foreach (TabPageNo page in Enum.GetValues(typeof (TabPageNo)))
            {
                _tabPageInitialized[(int) page] = false;
            }

            // 編集項目を更新する
            OnMainTabPageFileLoad();
            OnAllianceTabPageFileLoad();
            OnRelationTabPageFileLoad();
            OnTradeTabPageFileLoad();
            OnCountryTabPageFileLoad();
            OnGovernmentTabPageFileLoad();
            OnTechTabPageFileLoad();
            OnProvinceTabPageFileLoad();
            OnOobTabPageFileLoad();
        }

        /// <summary>
        ///     データ保存後の処理
        /// </summary>
        internal void OnFileSaved()
        {
            // 各タブページの初期化済み状態をクリアする
            foreach (TabPageNo page in Enum.GetValues(typeof (TabPageNo)))
            {
                _tabPageInitialized[(int) page] = false;
            }

            // 強制的に選択タブの表示を更新する
            OnScenarioTabControlSelectedIndexChanged(null, null);
        }

        /// <summary>
        ///     編集項目更新時の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal void OnItemChanged(EditorItemId id)
        {
            // 何もしない
        }

        /// <summary>
        ///     マップ読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapFileLoad(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // プロヴィンスタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Province)
            {
                return;
            }

            // マップパネルを更新する
            _provincePage.UpdateMapPanel();
        }

        #endregion

        #region フォーム

        /// <summary>
        ///     フォームの初期化
        /// </summary>
        private void InitForm()
        {
            // ウィンドウの位置
            Location = HoI2EditorController.Settings.ScenarioEditor.Location;
            Size = HoI2EditorController.Settings.ScenarioEditor.Size;

            // 技術ツリーパネル
            _techTreePanelController = new TechTreePanelController(techTreePictureBox) { ApplyItemStatus = true };
            _techTreePanelController.ItemMouseClick += OnTechTreeItemMouseClick;
            _techTreePanelController.QueryItemStatus += OnQueryTechTreeItemStatus;
        }

        /// <summary>
        ///     フォーム読み込み時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, EventArgs e)
        {
            // 国家データを初期化する
            Countries.Init();

            // 閣僚特性を初期化する
            Ministers.InitPersonality();

            // ユニットデータを初期化する
            Units.Init();

            // プロヴィンスデータを初期化する
            Provinces.Init();

            // ゲーム設定ファイルを読み込む
            Misc.Load();

            // 文字列定義ファイルを読み込む
            Config.Load();

            // マップを遅延読み込みする
            Maps.LoadAsync(MapLevel.Level2, OnMapFileLoad);

            // 指揮官データを遅延読み込みする
            Leaders.LoadAsync(null);

            // 閣僚データを遅延読み込みする
            Ministers.LoadAsync(null);

            // 技術データを遅延読み込みする
            Techs.LoadAsync(null);

            // プロヴィンスデータを遅延読み込みする
            Provinces.LoadAsync(null);

            // ユニットデータを遅延読み込みする
            Units.LoadAsync(null);

            // メインタブを初期化する
            OnMainTabPageSelected();

            // 表示項目を初期化する
            OnTradeTabPageFormLoad();
            OnTechTabPageFormLoad();

            // シナリオファイル読み込み済みなら編集項目を更新する
            if (Scenarios.IsLoaded())
            {
                OnFileLoaded();
            }
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _controller.OnFormClosing();
        }

        /// <summary>
        ///     フォームクローズ後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.OnFormClosed();
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
                HoI2EditorController.Settings.ScenarioEditor.Location = Location;
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
                HoI2EditorController.Settings.ScenarioEditor.Size = Size;
            }
        }

        /// <summary>
        ///     チェックボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckButtonClick(object sender, EventArgs e)
        {
            // プロヴィンスデータ読み込み完了まで待つ
            Provinces.WaitLoading();

            DataChecker.CheckScenario();
        }

        /// <summary>
        ///     再読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReloadButtonClick(object sender, EventArgs e)
        {
            _controller.QueryReload();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            _controller.Save();
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
        ///     選択タブ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            _tabPageNo = (TabPageNo) scenarioTabControl.SelectedIndex;

            switch (_tabPageNo)
            {
                case TabPageNo.Main:
                    OnMainTabPageSelected();
                    break;

                case TabPageNo.Alliance:
                    OnAllianceTabPageSelected();
                    break;

                case TabPageNo.Relation:
                    OnRelationTabPageSelected();
                    break;

                case TabPageNo.Trade:
                    OnTradeTabPageSelected();
                    break;

                case TabPageNo.Country:
                    OnCountryTabPageSelected();
                    break;

                case TabPageNo.Government:
                    OnGovernmentTabPageSelected();
                    break;

                case TabPageNo.Technology:
                    OnTechTabPageSelected();
                    break;

                case TabPageNo.Province:
                    OnProvinceTabPageSelected();
                    break;

                case TabPageNo.Oob:
                    OnOobTabPageSelected();
                    break;
            }
        }

        #endregion

        #region メインタブ

        /// <summary>
        ///     メインタブのファイル読み込み時の処理
        /// </summary>
        private void OnMainTabPageFileLoad()
        {
            // メインタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Main)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Main])
            {
                return;
            }

            // 編集項目を初期化する
            _mainPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Main] = true;
        }

        /// <summary>
        ///     メインタブ選択時の処理
        /// </summary>
        private void OnMainTabPageSelected()
        {
            // タブページを作成する
            if (_mainPage == null)
            {
                _mainPage = new ScenarioEditorMainPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Main].Controls.Add(_mainPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Main])
            {
                return;
            }

            // 編集項目を初期化する
            _mainPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Main] = true;
        }

        /// <summary>
        ///     パネル画像を更新する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        internal void UpdatePanelImage(string fileName)
        {
            _mainPage.UpdatePanelImage(fileName);
        }

        /// <summary>
        ///     プロパガンダ画像を更新する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <param name="fileName">プロパガンダ画像名</param>
        internal void UpdatePropagandaImage(Country country, string fileName)
        {
            _mainPage.UpdatePropagandaImage(country, fileName);
        }

        #endregion

        #region 同盟タブ

        /// <summary>
        ///     同盟タブのファイル読み込み時の処理
        /// </summary>
        private void OnAllianceTabPageFileLoad()
        {
            // 同盟タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Alliance)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Alliance])
            {
                return;
            }

            // 編集項目を初期化する
            _alliancePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Alliance] = true;
        }

        /// <summary>
        ///     同盟タブ選択時の処理
        /// </summary>
        private void OnAllianceTabPageSelected()
        {
            // タブページを作成する
            if (_alliancePage == null)
            {
                _alliancePage = new ScenarioEditorAlliancePage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Alliance].Controls.Add(_alliancePage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Alliance])
            {
                return;
            }

            // 編集項目を初期化する
            _alliancePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Alliance] = true;
        }

        /// <summary>
        ///     同盟リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetAllianceListItemText(int no, string s)
        {
            _alliancePage.SetAllianceListItemText(no, s);
        }

        #endregion

        #region 関係タブ

        /// <summary>
        ///     関係タブのファイル読み込み時の処理
        /// </summary>
        private void OnRelationTabPageFileLoad()
        {
            // 同盟タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Relation)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Relation])
            {
                return;
            }

            // 編集項目を初期化する
            _relationPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Relation] = true;
        }

        /// <summary>
        ///     関係タブ選択時の処理
        /// </summary>
        private void OnRelationTabPageSelected()
        {
            // タブページを作成する
            if (_relationPage == null)
            {
                _relationPage = new ScenarioEditorRelationPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Relation].Controls.Add(_relationPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Relation])
            {
                return;
            }

            // 編集項目を初期化する
            _relationPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Relation] = true;
        }

        /// <summary>
        ///     貿易リストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">項目のインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetRelationListItemText(int index, int no, string s)
        {
            _relationPage.SetRelationListItemText(index, no, s);
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetRelationListItemText(int no, string s)
        {
            _relationPage.SetRelationListItemText(no, s);
        }

        #endregion

        #region 貿易タブ

        #region 貿易タブ - 共通

        /// <summary>
        ///     貿易タブの編集項目を初期化する
        /// </summary>
        private void InitTradeTab()
        {
            InitTradeInfoItems();
            InitTradeDealsItems();
        }

        /// <summary>
        ///     貿易タブの編集項目を更新する
        /// </summary>
        private void UpdateTradeTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Trade])
            {
                return;
            }

            // 貿易国コンボボックスを更新する
            UpdateCountryComboBox(tradeCountryComboBox1, false);
            UpdateCountryComboBox(tradeCountryComboBox2, false);

            // 貿易リストを更新する
            UpdateTradeList();

            // 貿易リストを有効化する
            EnableTradeList();

            // 新規ボタンを有効化する
            EnableTradeNewButton();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Trade] = true;
        }

        /// <summary>
        ///     貿易タブのフォーム読み込み時の処理
        /// </summary>
        private void OnTradeTabPageFormLoad()
        {
            // 貿易タブを初期化する
            InitTradeTab();
        }

        /// <summary>
        ///     貿易タブのファイル読み込み時の処理
        /// </summary>
        private void OnTradeTabPageFileLoad()
        {
            // 貿易タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Trade)
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateTradeTab();
        }

        /// <summary>
        ///     貿易タブ選択時の処理
        /// </summary>
        private void OnTradeTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初回遷移時には表示を更新する
            UpdateTradeTab();
        }

        #endregion

        #region 貿易タブ - 貿易リスト

        /// <summary>
        ///     貿易リストの表示を更新する
        /// </summary>
        private void UpdateTradeList()
        {
            List<Treaty> trades = Scenarios.Data.GlobalData.Trades;
            tradeListView.BeginUpdate();
            tradeListView.Items.Clear();
            foreach (Treaty treaty in trades)
            {
                tradeListView.Items.Add(CreateTradeListViewItem(treaty));
            }
            tradeListView.EndUpdate();

            // 編集項目を無効化する
            DisableTradeInfoItems();
            DisableTradeDealsItems();
            DisableTradeButtons();

            // 編集項目をクリアする
            ClearTradeInfoItems();
            ClearTradeDealsItems();
        }

        /// <summary>
        ///     貿易リストを有効化する
        /// </summary>
        private void EnableTradeList()
        {
            tradeListView.Enabled = true;
        }

        /// <summary>
        ///     新規ボタンを有効化する
        /// </summary>
        private void EnableTradeNewButton()
        {
            tradeNewButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを有効化する
        /// </summary>
        private void EnableTradeButtons()
        {
            int index = tradeListView.SelectedIndices[0];
            int count = tradeListView.Items.Count;
            tradeUpButton.Enabled = index > 0;
            tradeDownButton.Enabled = index < count - 1;
            tradeRemoveButton.Enabled = true;
        }

        /// <summary>
        ///     削除/上へ/下へボタンを無効化する
        /// </summary>
        private void DisableTradeButtons()
        {
            tradeUpButton.Enabled = false;
            tradeDownButton.Enabled = false;
            tradeRemoveButton.Enabled = false;
        }

        /// <summary>
        ///     貿易リストビューの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (tradeListView.SelectedIndices.Count == 0)
            {
                // 編集項目を無効化する
                DisableTradeInfoItems();
                DisableTradeDealsItems();
                DisableTradeButtons();

                // 編集項目をクリアする
                ClearTradeInfoItems();
                ClearTradeDealsItems();
                return;
            }

            Treaty treaty = GetSelectedTrade();

            // 編集項目を更新する
            UpdateTradeInfoItems(treaty);
            UpdateTradeDealsItems(treaty);

            // 編集項目を有効化する
            EnableTradeInfoItems();
            EnableTradeDealsItems();
            EnableTradeButtons();
        }

        /// <summary>
        ///     貿易の上へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeUpButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index - 1, item);
            tradeListView.Items[index - 1].Focused = true;
            tradeListView.Items[index - 1].Selected = true;
            tradeListView.EnsureVisible(index - 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index - 1, trade);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の下へボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDownButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストビューの項目を移動する
            int index = tradeListView.SelectedIndices[0];
            ListViewItem item = tradeListView.Items[index];
            tradeListView.Items.RemoveAt(index);
            tradeListView.Items.Insert(index + 1, item);
            tradeListView.Items[index + 1].Focused = true;
            tradeListView.Items[index + 1].Selected = true;
            tradeListView.EnsureVisible(index + 1);

            // 貿易リストの項目を移動する
            Treaty trade = trades[index];
            trades.RemoveAt(index);
            trades.Insert(index + 1, trade);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();
        }

        /// <summary>
        ///     貿易の新規ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeNewButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            // 貿易リストに項目を追加する
            Treaty trade = new Treaty
            {
                StartDate = new GameDate(),
                EndDate = new GameDate(),
                Id = Scenarios.GetNewTypeId(Scenarios.DefaultTreatyType, 1)
            };
            trades.Add(trade);

            // 貿易リストビューに項目を追加する
            ListViewItem item = new ListViewItem { Tag = trade };
            item.SubItems.Add("");
            item.SubItems.Add("");
            tradeListView.Items.Add(item);

            // 編集済みフラグを設定する
            trade.SetDirty(Treaty.ItemId.StartYear);
            trade.SetDirty(Treaty.ItemId.StartMonth);
            trade.SetDirty(Treaty.ItemId.StartDay);
            trade.SetDirty(Treaty.ItemId.EndYear);
            trade.SetDirty(Treaty.ItemId.EndMonth);
            trade.SetDirty(Treaty.ItemId.EndDay);
            trade.SetDirty(Treaty.ItemId.Type);
            trade.SetDirty(Treaty.ItemId.Id);
            trade.SetDirty(Treaty.ItemId.Cancel);
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();

            // 追加した項目を選択する
            if (tradeListView.SelectedIndices.Count > 0)
            {
                ListViewItem prev = tradeListView.SelectedItems[0];
                prev.Focused = false;
                prev.Selected = false;
            }
            item.Focused = true;
            item.Selected = true;
        }

        /// <summary>
        ///     貿易の削除ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeRemoveButtonClick(object sender, EventArgs e)
        {
            Scenario scenario = Scenarios.Data;
            List<Treaty> trades = scenario.GlobalData.Trades;

            int index = tradeListView.SelectedIndices[0];
            Treaty trade = trades[index];

            // typeとidの組を削除する
            Scenarios.RemoveTypeId(trade.Id);

            // 貿易リストから項目を削除する
            trades.RemoveAt(index);

            // 貿易リストビューから項目を削除する
            tradeListView.Items.RemoveAt(index);

            // 編集済みフラグを設定する
            Scenarios.Data.SetDirty();
            Scenarios.SetDirty();

            // 削除した項目の次を選択する
            if (index == trades.Count)
            {
                index--;
            }
            if (index >= 0)
            {
                tradeListView.Items[index].Focused = true;
                tradeListView.Items[index].Selected = true;
            }
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetTradeListItemText(int no, string s)
        {
            tradeListView.SelectedItems[0].SubItems[no].Text = s;
        }

        /// <summary>
        ///     貿易リストの項目を作成する
        /// </summary>
        /// <param name="treaty">貿易情報</param>
        /// <returns>貿易リストの項目</returns>
        private static ListViewItem CreateTradeListViewItem(Treaty treaty)
        {
            ListViewItem item = new ListViewItem
            {
                Text = Countries.GetName(treaty.Country1),
                Tag = treaty
            };
            item.SubItems.Add(Countries.GetName(treaty.Country2));
            item.SubItems.Add(treaty.GetTradeString());

            return item;
        }

        /// <summary>
        ///     選択中の貿易情報を取得する
        /// </summary>
        /// <returns>選択中の貿易情報</returns>
        private Treaty GetSelectedTrade()
        {
            return tradeListView.SelectedItems.Count > 0 ? tradeListView.SelectedItems[0].Tag as Treaty : null;
        }

        #endregion

        #region 貿易タブ - 貿易情報

        /// <summary>
        ///     貿易情報の編集項目を初期化する
        /// </summary>
        private void InitTradeInfoItems()
        {
            _itemControls.Add(ScenarioEditorItemId.TradeStartYear, tradeStartYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeStartMonth, tradeStartMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeStartDay, tradeStartDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndYear, tradeEndYearTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndMonth, tradeEndMonthTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeEndDay, tradeEndDayTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeType, tradeTypeTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeId, tradeIdTextBox);
            _itemControls.Add(ScenarioEditorItemId.TradeCancel, tradeCancelCheckBox);

            tradeStartYearTextBox.Tag = ScenarioEditorItemId.TradeStartYear;
            tradeStartMonthTextBox.Tag = ScenarioEditorItemId.TradeStartMonth;
            tradeStartDayTextBox.Tag = ScenarioEditorItemId.TradeStartDay;
            tradeEndYearTextBox.Tag = ScenarioEditorItemId.TradeEndYear;
            tradeEndMonthTextBox.Tag = ScenarioEditorItemId.TradeEndMonth;
            tradeEndDayTextBox.Tag = ScenarioEditorItemId.TradeEndDay;
            tradeTypeTextBox.Tag = ScenarioEditorItemId.TradeType;
            tradeIdTextBox.Tag = ScenarioEditorItemId.TradeId;
            tradeCancelCheckBox.Tag = ScenarioEditorItemId.TradeCancel;
        }

        /// <summary>
        ///     貿易情報の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeInfoItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeStartYearTextBox, treaty);
            _controller.UpdateItemValue(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeStartDayTextBox, treaty);
            _controller.UpdateItemValue(tradeEndYearTextBox, treaty);
            _controller.UpdateItemValue(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemValue(tradeEndDayTextBox, treaty);
            _controller.UpdateItemValue(tradeTypeTextBox, treaty);
            _controller.UpdateItemValue(tradeIdTextBox, treaty);
            _controller.UpdateItemValue(tradeCancelCheckBox, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeStartYearTextBox, treaty);
            _controller.UpdateItemColor(tradeStartMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeStartDayTextBox, treaty);
            _controller.UpdateItemColor(tradeEndYearTextBox, treaty);
            _controller.UpdateItemColor(tradeEndMonthTextBox, treaty);
            _controller.UpdateItemColor(tradeEndDayTextBox, treaty);
            _controller.UpdateItemColor(tradeTypeTextBox, treaty);
            _controller.UpdateItemColor(tradeIdTextBox, treaty);
            _controller.UpdateItemColor(tradeCancelCheckBox, treaty);
        }

        /// <summary>
        ///     貿易情報の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeInfoItems()
        {
            tradeStartYearTextBox.Text = "";
            tradeStartMonthTextBox.Text = "";
            tradeStartDayTextBox.Text = "";
            tradeEndYearTextBox.Text = "";
            tradeEndMonthTextBox.Text = "";
            tradeEndDayTextBox.Text = "";
            tradeTypeTextBox.Text = "";
            tradeIdTextBox.Text = "";
            tradeCancelCheckBox.Checked = false;
        }

        /// <summary>
        ///     貿易情報の編集項目を有効化する
        /// </summary>
        private void EnableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易情報の編集項目を無効化する
        /// </summary>
        private void DisableTradeInfoItems()
        {
            tradeInfoGroupBox.Enabled = false;
        }

        #endregion

        #region 貿易タブ - 貿易内容

        /// <summary>
        ///     貿易内容の編集項目を初期化する
        /// </summary>
        private void InitTradeDealsItems()
        {
            _itemControls.Add(ScenarioEditorItemId.TradeCountry1, tradeCountryComboBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeCountry2, tradeCountryComboBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeEnergy1, tradeEnergyTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeEnergy2, tradeEnergyTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeMetal1, tradeMetalTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeMetal2, tradeMetalTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeRareMaterials1, tradeRareMaterialsTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeRareMaterials2, tradeRareMaterialsTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeOil1, tradeOilTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeOil2, tradeOilTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeSupplies1, tradeSuppliesTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeSupplies2, tradeSuppliesTextBox2);
            _itemControls.Add(ScenarioEditorItemId.TradeMoney1, tradeMoneyTextBox1);
            _itemControls.Add(ScenarioEditorItemId.TradeMoney2, tradeMoneyTextBox2);

            tradeCountryComboBox1.Tag = ScenarioEditorItemId.TradeCountry1;
            tradeCountryComboBox2.Tag = ScenarioEditorItemId.TradeCountry2;
            tradeEnergyTextBox1.Tag = ScenarioEditorItemId.TradeEnergy1;
            tradeEnergyTextBox2.Tag = ScenarioEditorItemId.TradeEnergy2;
            tradeMetalTextBox1.Tag = ScenarioEditorItemId.TradeMetal1;
            tradeMetalTextBox2.Tag = ScenarioEditorItemId.TradeMetal2;
            tradeRareMaterialsTextBox1.Tag = ScenarioEditorItemId.TradeRareMaterials1;
            tradeRareMaterialsTextBox2.Tag = ScenarioEditorItemId.TradeRareMaterials2;
            tradeOilTextBox1.Tag = ScenarioEditorItemId.TradeOil1;
            tradeOilTextBox2.Tag = ScenarioEditorItemId.TradeOil2;
            tradeSuppliesTextBox1.Tag = ScenarioEditorItemId.TradeSupplies1;
            tradeSuppliesTextBox2.Tag = ScenarioEditorItemId.TradeSupplies2;
            tradeMoneyTextBox1.Tag = ScenarioEditorItemId.TradeMoney1;
            tradeMoneyTextBox2.Tag = ScenarioEditorItemId.TradeMoney2;

            // 貿易資源ラベル
            tradeEnergyLabel.Text = Config.GetText(TextId.ResourceEnergy);
            tradeMetalLabel.Text = Config.GetText(TextId.ResourceMetal);
            tradeRareMaterialsLabel.Text = Config.GetText(TextId.ResourceRareMaterials);
            tradeOilLabel.Text = Config.GetText(TextId.ResourceOil);
            tradeSuppliesLabel.Text = Config.GetText(TextId.ResourceSupplies);
            tradeMoneyLabel.Text = Config.GetText(TextId.ResourceMoney);
        }

        /// <summary>
        ///     貿易内容の編集項目を更新する
        /// </summary>
        /// <param name="treaty">協定</param>
        private void UpdateTradeDealsItems(Treaty treaty)
        {
            // 編集項目の表示を更新する
            _controller.UpdateItemValue(tradeCountryComboBox1, treaty);
            _controller.UpdateItemValue(tradeCountryComboBox2, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemValue(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox1, treaty);
            _controller.UpdateItemValue(tradeMetalTextBox2, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemValue(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemValue(tradeOilTextBox1, treaty);
            _controller.UpdateItemValue(tradeOilTextBox2, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemValue(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemValue(tradeMoneyTextBox2, treaty);

            // 編集項目の色を更新する
            _controller.UpdateItemColor(tradeEnergyTextBox1, treaty);
            _controller.UpdateItemColor(tradeEnergyTextBox2, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox1, treaty);
            _controller.UpdateItemColor(tradeMetalTextBox2, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox1, treaty);
            _controller.UpdateItemColor(tradeRareMaterialsTextBox2, treaty);
            _controller.UpdateItemColor(tradeOilTextBox1, treaty);
            _controller.UpdateItemColor(tradeOilTextBox2, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox1, treaty);
            _controller.UpdateItemColor(tradeSuppliesTextBox2, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox1, treaty);
            _controller.UpdateItemColor(tradeMoneyTextBox2, treaty);
        }

        /// <summary>
        ///     貿易内容の編集項目の表示をクリアする
        /// </summary>
        private void ClearTradeDealsItems()
        {
            tradeCountryComboBox1.SelectedIndex = -1;
            tradeCountryComboBox2.SelectedIndex = -1;
            tradeEnergyTextBox1.Text = "";
            tradeEnergyTextBox2.Text = "";
            tradeMetalTextBox1.Text = "";
            tradeMetalTextBox2.Text = "";
            tradeRareMaterialsTextBox1.Text = "";
            tradeRareMaterialsTextBox2.Text = "";
            tradeOilTextBox1.Text = "";
            tradeOilTextBox2.Text = "";
            tradeSuppliesTextBox1.Text = "";
            tradeSuppliesTextBox2.Text = "";
            tradeMoneyTextBox1.Text = "";
            tradeMoneyTextBox2.Text = "";
        }

        /// <summary>
        ///     貿易内容の編集項目を有効化する
        /// </summary>
        private void EnableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = true;
        }

        /// <summary>
        ///     貿易内容の編集項目を無効化する
        /// </summary>
        private void DisableTradeDealsItems()
        {
            tradeDealsGroupBox.Enabled = false;
        }

        /// <summary>
        ///     貿易国入れ替えボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeSwapButtonClick(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 値を入れ替える
            Country country = treaty.Country1;
            treaty.Country1 = treaty.Country2;
            treaty.Country2 = country;

            treaty.Energy = -treaty.Energy;
            treaty.Metal = -treaty.Metal;
            treaty.RareMaterials = -treaty.RareMaterials;
            treaty.Oil = -treaty.Oil;
            treaty.Supplies = -treaty.Supplies;
            treaty.Money = -treaty.Money;

            // 編集済みフラグを設定する
            treaty.SetDirty(Treaty.ItemId.Country1);
            treaty.SetDirty(Treaty.ItemId.Country2);
            treaty.SetDirty(Treaty.ItemId.Energy);
            treaty.SetDirty(Treaty.ItemId.Metal);
            treaty.SetDirty(Treaty.ItemId.RareMaterials);
            treaty.SetDirty(Treaty.ItemId.Oil);
            treaty.SetDirty(Treaty.ItemId.Supplies);
            treaty.SetDirty(Treaty.ItemId.Money);
            Scenarios.SetDirty();

            // 貿易リストビューの項目を更新する
            ListViewItem item = tradeListView.SelectedItems[0];
            item.Text = Countries.GetName(treaty.Country1);
            item.SubItems[1].Text = Countries.GetName(treaty.Country2);
            item.SubItems[2].Text = treaty.GetTradeString();

            // 編集項目を更新する
            UpdateTradeDealsItems(treaty);
        }

        #endregion

        #region 貿易タブ - 編集項目

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeIntItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
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
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
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
            if (!_controller.IsItemValueValid(itemId, val, treaty))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     テキストボックスのフォーカス移動後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeDoubleItemTextBoxValidated(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
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
            double val;
            if (!DoubleHelper.TryParse(control.Text, out val))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            // 初期値から変更されていなければ何もしない
            object prev = _controller.GetItemValue(itemId, treaty);
            if ((prev == null) && DoubleHelper.IsZero(val))
            {
                return;
            }

            // 値に変化がなければ何もしない
            if ((prev != null) && DoubleHelper.IsEqual(val, (double) prev))
            {
                return;
            }

            // 無効な値ならば値を戻す
            if (!_controller.IsItemValueValid(itemId, val, treaty))
            {
                _controller.UpdateItemValue(control, treaty);
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     チェックボックスのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeItemCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ何もしない
            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            CheckBox control = sender as CheckBox;
            if (control == null)
            {
                return;
            }
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            bool val = control.Checked;
            if (val == (bool) _controller.GetItemValue(itemId, treaty))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 文字色を変更する
            control.ForeColor = Color.Red;

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        /// <summary>
        ///     コンボボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryItemComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            // 項目がなければ何もしない
            if (e.Index < 0)
            {
                return;
            }

            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            // 背景を描画する
            e.DrawBackground();

            // 項目の文字列を描画する
            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;
            Country val = (Country) _controller.GetItemValue(itemId, treaty);
            Country sel = (Country) _controller.GetListItemValue(itemId, e.Index);
            Brush brush = (val == sel) && _controller.IsItemDirty(itemId, treaty)
                ? new SolidBrush(Color.Red)
                : new SolidBrush(SystemColors.WindowText);
            string s = control.Items[e.Index].ToString();
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds);
            brush.Dispose();

            // フォーカスを描画する
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     コンボボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTradeCountryItemComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = sender as ComboBox;
            if (control == null)
            {
                return;
            }

            // 選択項目がなければ何もしない
            if (control.SelectedIndex < 0)
            {
                return;
            }

            Treaty treaty = GetSelectedTrade();
            if (treaty == null)
            {
                return;
            }

            ScenarioEditorItemId itemId = (ScenarioEditorItemId) control.Tag;

            // 値に変化がなければ何もしない
            Country val = Countries.Tags[control.SelectedIndex];
            if (val == (Country) _controller.GetItemValue(itemId, treaty))
            {
                return;
            }

            _controller.OutputItemValueChangedLog(itemId, val, treaty);

            // 項目値変更前の処理
            _controller.PreItemChanged(itemId, val, treaty);

            // 値を更新する
            _controller.SetItemValue(itemId, val, treaty);

            // 編集済みフラグを設定する
            _controller.SetItemDirty(itemId, treaty);

            // 項目色を変更するため描画更新する
            control.Refresh();

            // 項目値変更後の処理
            _controller.PostItemChanged(itemId, val, treaty);
        }

        #endregion

        #endregion

        #region 国家タブ

        /// <summary>
        ///     国家タブのファイル読み込み時の処理
        /// </summary>
        private void OnCountryTabPageFileLoad()
        {
            // 国家タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Country)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Country])
            {
                return;
            }

            // 編集項目を初期化する
            _countryPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Country] = true;
        }

        /// <summary>
        ///     国家タブ選択時の処理
        /// </summary>
        private void OnCountryTabPageSelected()
        {
            // タブページを作成する
            if (_countryPage == null)
            {
                _countryPage = new ScenarioEditorCountryPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Country].Controls.Add(_countryPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Country])
            {
                return;
            }

            // 編集項目を初期化する
            _countryPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Country] = true;
        }

        #endregion

        #region 政府タブ

        /// <summary>
        ///     政府タブのファイル読み込み時の処理
        /// </summary>
        private void OnGovernmentTabPageFileLoad()
        {
            // 政府タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Government)
            {
                return;
            }

            // 閣僚データの読み込み完了まで待機する
            Ministers.WaitLoading();

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Government])
            {
                return;
            }

            // 編集項目を初期化する
            _governmentPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Government] = true;
        }

        /// <summary>
        ///     政府タブ選択時の処理
        /// </summary>
        private void OnGovernmentTabPageSelected()
        {
            // タブページを作成する
            if (_governmentPage == null)
            {
                _governmentPage = new ScenarioEditorGovernmentPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Government].Controls.Add(_governmentPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 閣僚データの読み込み完了まで待機する
            Ministers.WaitLoading();

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Government])
            {
                return;
            }

            // 編集項目を初期化する
            _governmentPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Government] = true;
        }

        #endregion

        #region 技術タブ

        #region 技術タブ - 共通

        /// <summary>
        ///     技術タブを初期化する
        /// </summary>
        private static void InitTechTab()
        {
            // 何もしない
        }

        /// <summary>
        ///     技術タブを更新する
        /// </summary>
        private void UpdateTechTab()
        {
            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Technology])
            {
                return;
            }

            // 技術カテゴリリストボックスを初期化する
            InitTechCategoryListBox();

            // 技術カテゴリリストボックスを有効化する
            EnableTechCategoryListBox();

            // 国家リストボックスを更新する
            UpdateCountryListBox(techCountryListBox);

            // 国家リストボックスを有効化する
            EnableTechCountryListBox();

            // 編集項目を無効化する
            DisableTechItems();

            // 編集項目をクリアする
            ClearTechItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Technology] = true;
        }

        /// <summary>
        ///     技術タブのフォーム読み込み時の処理
        /// </summary>
        private static void OnTechTabPageFormLoad()
        {
            // 技術タブを初期化する
            InitTechTab();
        }

        /// <summary>
        ///     技術タブのファイル読み込み時の処理
        /// </summary>
        private void OnTechTabPageFileLoad()
        {
            // 政府タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Technology)
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            Techs.WaitLoading();

            // 初回遷移時には表示を更新する
            UpdateTechTab();
        }

        /// <summary>
        ///     技術タブ選択時の処理
        /// </summary>
        private void OnTechTabPageSelected()
        {
            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            Techs.WaitLoading();

            // 初回遷移時には表示を更新する
            UpdateTechTab();
        }

        #endregion

        #region 技術タブ - 技術カテゴリ

        /// <summary>
        ///     技術カテゴリリストボックスを初期化する
        /// </summary>
        private void InitTechCategoryListBox()
        {
            techCategoryListBox.BeginUpdate();
            techCategoryListBox.Items.Clear();
            foreach (TechGroup grp in Techs.Groups)
            {
                techCategoryListBox.Items.Add(grp);
            }
            techCategoryListBox.SelectedIndex = 0;
            techCategoryListBox.EndUpdate();
        }

        /// <summary>
        ///     技術カテゴリリストボックスを有効化する
        /// </summary>
        private void EnableTechCategoryListBox()
        {
            techCategoryListBox.Enabled = true;
        }

        /// <summary>
        ///     技術カテゴリリストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechCategoryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択中の国家がなければ編集項目を無効化する
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                // 編集項目を無効化する
                DisableTechItems();

                // 編集項目をクリアする
                ClearTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();

            // 編集項目を有効化する
            EnableTechItems();
        }

        /// <summary>
        ///     選択中の技術グループを取得する
        /// </summary>
        /// <returns>選択中の技術グループ</returns>
        private TechGroup GetSelectedTechGroup()
        {
            if (techCategoryListBox.SelectedIndex < 0)
            {
                return null;
            }
            return Techs.Groups[techCategoryListBox.SelectedIndex];
        }

        #endregion

        #region 技術タブ - 国家

        /// <summary>
        ///     国家リストボックスを有効化する
        /// </summary>
        private void EnableTechCountryListBox()
        {
            techCountryListBox.Enabled = true;
        }

        /// <summary>
        ///     国家リストボックスの選択項目変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechCountryListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択項目がなければ編集項目を無効化する
            if (techCountryListBox.SelectedIndex < 0)
            {
                // 編集項目を無効化する
                DisableTechItems();

                // 編集項目をクリアする
                ClearTechItems();
                return;
            }

            // 編集項目を更新する
            UpdateTechItems();

            // 編集項目を有効化する
            EnableTechItems();
        }

        /// <summary>
        ///     選択中の国家を取得する
        /// </summary>
        /// <returns>選択中の国家</returns>
        private Country GetSelectedTechCountry()
        {
            if (techCountryListBox.SelectedIndex < 0)
            {
                return Country.None;
            }
            return Countries.Tags[techCountryListBox.SelectedIndex];
        }

        #endregion

        #region 技術タブ - 編集項目

        /// <summary>
        ///     技術の編集項目を更新する
        /// </summary>
        private void UpdateTechItems()
        {
            Country country = GetSelectedTechCountry();
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            TechGroup grp = GetSelectedTechGroup();

            // 保有技術リスト
            _techs = grp.Items.OfType<TechItem>().ToList();
            UpdateOwnedTechList(settings);

            // 青写真リスト
            UpdateBlueprintList(settings);

            // 発明イベントリスト
            _inventions = Techs.Groups.SelectMany(g => g.Items.OfType<TechEvent>()).ToList();
            UpdateInventionList(settings);

            // 技術ツリーを更新する
            _techTreePanelController.Category = grp.Category;
            _techTreePanelController.Update();
        }

        /// <summary>
        ///     技術の編集項目の表示をクリアする
        /// </summary>
        private void ClearTechItems()
        {
            // 技術ツリーをクリアする
            _techTreePanelController.Clear();

            // 編集項目をクリアする
            ownedTechsListView.Items.Clear();
            blueprintsListView.Items.Clear();
            inventionsListView.Items.Clear();
        }

        /// <summary>
        ///     技術の編集項目を有効化する
        /// </summary>
        private void EnableTechItems()
        {
            ownedTechsLabel.Enabled = true;
            ownedTechsListView.Enabled = true;
            blueprintsLabel.Enabled = true;
            blueprintsListView.Enabled = true;
            inventionsLabel.Enabled = true;
            inventionsListView.Enabled = true;
        }

        /// <summary>
        ///     技術の編集項目を無効化する
        /// </summary>
        private void DisableTechItems()
        {
            ownedTechsLabel.Enabled = false;
            ownedTechsListView.Enabled = false;
            blueprintsLabel.Enabled = false;
            blueprintsListView.Enabled = false;
            inventionsLabel.Enabled = false;
            inventionsListView.Enabled = false;
        }

        /// <summary>
        ///     保有技術リストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateOwnedTechList(CountrySettings settings)
        {
            ownedTechsListView.ItemChecked -= OnOwnedTechsListViewItemChecked;
            ownedTechsListView.BeginUpdate();
            ownedTechsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    ownedTechsListView.Items.Add(new ListViewItem
                    {
                        Text = name,
                        Checked = settings.TechApps.Contains(item.Id),
                        ForeColor = settings.IsDirtyOwnedTech(item.Id) ? Color.Red : ownedTechsListView.ForeColor,
                        Tag = item
                    });
                }
            }
            else
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    ownedTechsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                }
            }
            ownedTechsListView.EndUpdate();
            ownedTechsListView.ItemChecked += OnOwnedTechsListViewItemChecked;
        }

        /// <summary>
        ///     青写真リストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateBlueprintList(CountrySettings settings)
        {
            blueprintsListView.ItemChecked -= OnBlueprintsListViewItemChecked;
            blueprintsListView.BeginUpdate();
            blueprintsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    blueprintsListView.Items.Add(new ListViewItem
                    {
                        Text = name,
                        Checked = settings.BluePrints.Contains(item.Id),
                        ForeColor = settings.IsDirtyBlueprint(item.Id) ? Color.Red : ownedTechsListView.ForeColor,
                        Tag = item
                    });
                }
            }
            else
            {
                foreach (TechItem item in _techs)
                {
                    string name = item.ToString();
                    blueprintsListView.Items.Add(new ListViewItem { Text = name, Tag = item });
                }
            }
            blueprintsListView.EndUpdate();
            blueprintsListView.ItemChecked += OnBlueprintsListViewItemChecked;
        }

        /// <summary>
        ///     発明イベントリストの表示を更新する
        /// </summary>
        /// <param name="settings">国家設定</param>
        private void UpdateInventionList(CountrySettings settings)
        {
            inventionsListView.ItemChecked -= OnInveitionsListViewItemChecked;
            inventionsListView.BeginUpdate();
            inventionsListView.Items.Clear();
            if (settings != null)
            {
                foreach (TechEvent ev in _inventions)
                {
                    inventionsListView.Items.Add(new ListViewItem
                    {
                        Text = ev.ToString(),
                        Checked = settings.Inventions.Contains(ev.Id),
                        ForeColor = settings.IsDirtyInvention(ev.Id) ? Color.Red : inventionsListView.ForeColor,
                        Tag = ev
                    });
                }
            }
            else
            {
                foreach (TechEvent ev in _inventions)
                {
                    inventionsListView.Items.Add(new ListViewItem { Text = ev.ToString(), Tag = ev });
                }
            }
            inventionsListView.EndUpdate();
            inventionsListView.ItemChecked += OnInveitionsListViewItemChecked;
        }

        /// <summary>
        ///     保有技術リストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOwnedTechsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem item = e.Item.Tag as TechItem;
            if (item == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == settings.TechApps.Contains(item.Id)))
            {
                return;
            }

            Log.Info("[Scenario] owned techs: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            // 値を更新する
            if (val)
            {
                settings.TechApps.Add(item.Id);
            }
            else
            {
                settings.TechApps.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyOwnedTech(item.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(item);
        }

        /// <summary>
        ///     青写真リストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlueprintsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem item = e.Item.Tag as TechItem;
            if (item == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == settings.BluePrints.Contains(item.Id)))
            {
                return;
            }

            Log.Info("[Scenario] blurprints: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            // 値を更新する
            if (val)
            {
                settings.BluePrints.Add(item.Id);
            }
            else
            {
                settings.BluePrints.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyBlueprint(item.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(item);
        }

        /// <summary>
        ///     発明イベントリストビューのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInveitionsListViewItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechEvent ev = e.Item.Tag as TechEvent;
            if (ev == null)
            {
                return;
            }
            CountrySettings settings = Scenarios.GetCountrySettings(country);

            // 値に変化がなければ何もしない
            bool val = e.Item.Checked;
            if ((settings != null) && (val == settings.Inventions.Contains(ev.Id)))
            {
                return;
            }

            Log.Info("[Scenario] inventions: {0}{1} ({2})", val ? '+' : '-', ev.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            // 値を更新する
            if (val)
            {
                settings.Inventions.Add(ev.Id);
            }
            else
            {
                settings.Inventions.Remove(ev.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyInvention(ev.Id);
            Scenarios.SetDirty();

            // 文字色を変更する
            e.Item.ForeColor = Color.Red;

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(ev);
        }

        #endregion

        #region 技術タブ - 技術ツリー

        /// <summary>
        ///     項目ラベルマウスクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTechTreeItemMouseClick(object sender, TechTreePanelController.ItemMouseEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            TechItem tech = e.Item as TechItem;
            if (tech != null)
            {
                // 左クリックで保有技術の有無を切り替える
                if (e.Button == MouseButtons.Left)
                {
                    ToggleOwnedTech(tech, country);
                }
                // 右クリックで青写真の有無を切り替える
                else if (e.Button == MouseButtons.Right)
                {
                    ToggleBlueprint(tech, country);
                }
                return;
            }

            TechEvent ev = e.Item as TechEvent;
            if (ev != null)
            {
                // 左クリックで保有技術の有無を切り替える
                if (e.Button == MouseButtons.Left)
                {
                    ToggleInvention(ev, country);
                }
            }
        }

        /// <summary>
        ///     保有技術の有無を切り替える
        /// </summary>
        /// <param name="item">対象技術</param>
        /// <param name="country">対象国</param>
        private void ToggleOwnedTech(TechItem item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.TechApps.Contains(item.Id);

            Log.Info("[Scenario] owned techs: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            // 値を更新する
            if (val)
            {
                settings.TechApps.Add(item.Id);
            }
            else
            {
                settings.TechApps.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyOwnedTech(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _techs.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = ownedTechsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     保有技術の有無を切り替える
        /// </summary>
        /// <param name="item">対象技術</param>
        /// <param name="country">対象国</param>
        private void ToggleBlueprint(TechItem item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.BluePrints.Contains(item.Id);

            Log.Info("[Scenario] blueprints: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            if (val)
            {
                settings.BluePrints.Add(item.Id);
            }
            else
            {
                settings.BluePrints.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyBlueprint(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _techs.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = blueprintsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     発明イベントの有無を切り替える
        /// </summary>
        /// <param name="item">対象発明イベント</param>
        /// <param name="country">対象国</param>
        private void ToggleInvention(TechEvent item, Country country)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(country);
            bool val = (settings == null) || !settings.Inventions.Contains(item.Id);

            Log.Info("[Scenario] inventions: {0}{1} ({2})", val ? '+' : '-', item.Id, Countries.Strings[(int) country]);

            if (settings == null)
            {
                settings = Scenarios.CreateCountrySettings(country);
            }

            if (val)
            {
                settings.Inventions.Add(item.Id);
            }
            else
            {
                settings.Inventions.Remove(item.Id);
            }

            // 編集済みフラグを設定する
            settings.SetDirtyInvention(item.Id);
            Scenarios.SetDirty();

            // 技術ツリーの項目ラベルを更新する
            _techTreePanelController.UpdateItem(item);

            // 保有技術リストビューの表示を更新する
            int index = _inventions.IndexOf(item);
            if (index >= 0)
            {
                ListViewItem li = inventionsListView.Items[index];
                li.Checked = val;
                li.ForeColor = Color.Red;
                li.EnsureVisible();
            }
        }

        /// <summary>
        ///     技術項目の状態を返す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQueryTechTreeItemStatus(object sender, TechTreePanelController.QueryItemStatusEventArgs e)
        {
            // 選択中の国家がなければ何もしない
            Country country = GetSelectedTechCountry();
            if (country == Country.None)
            {
                return;
            }

            CountrySettings settings = Scenarios.GetCountrySettings(country);
            if (settings == null)
            {
                return;
            }

            TechItem tech = e.Item as TechItem;
            if (tech != null)
            {
                e.Done = settings.TechApps.Contains(tech.Id);
                e.Blueprint = settings.BluePrints.Contains(tech.Id);
                return;
            }

            TechEvent ev = e.Item as TechEvent;
            if (ev != null)
            {
                e.Done = settings.Inventions.Contains(ev.Id);
            }
        }

        #endregion

        #endregion

        #region プロヴィンスタブ

        /// <summary>
        ///     プロヴィンスタブのファイル読み込み時の処理
        /// </summary>
        private void OnProvinceTabPageFileLoad()
        {
            // プロヴィンスタブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Province)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Province])
            {
                return;
            }

            // 編集項目を初期化する
            _provincePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Province] = true;
        }

        /// <summary>
        ///     プロヴィンスタブ選択時の処理
        /// </summary>
        private void OnProvinceTabPageSelected()
        {
            // タブページを作成する
            if (_provincePage == null)
            {
                _provincePage = new ScenarioEditorProvincePage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Province].Controls.Add(_provincePage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // プロヴィンスデータの読み込み完了まで待機する
            Provinces.WaitLoading();

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Province])
            {
                return;
            }

            // 編集項目を初期化する
            _provincePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Province] = true;
        }

        /// <summary>
        ///     プロヴィンスリストビューの項目文字列を設定する
        /// </summary>
        /// <param name="index">プロヴィンスリストビューのインデックス</param>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetProvinceListItemText(int index, int no, string s)
        {
            _provincePage.SetProvinceListItemText(index, no, s);
        }

        /// <summary>
        ///     プロヴィンス単位でマップ画像を更新する
        /// </summary>
        /// <param name="id">更新対象のプロヴィンスID</param>
        /// <param name="highlighted">強調表示の有無</param>
        /// <param name="mode">フィルターモード</param>
        internal void UpdateProvince(ushort id, bool highlighted, MapPanelController.MapFilterMode mode)
        {
            _provincePage.UpdateProvince(id, highlighted, mode);
        }

        #endregion

        #region 初期部隊タブ

        /// <summary>
        ///     初期部隊タブのファイル読み込み時の処理
        /// </summary>
        private void OnOobTabPageFileLoad()
        {
            // 初期部隊タブ選択中でなければ何もしない
            if (_tabPageNo != TabPageNo.Oob)
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Oob])
            {
                return;
            }

            // 指揮官データの読み込み完了まで待機する
            Leaders.WaitLoading();

            // プロヴィンスデータの読み込み完了まで待機する
            Provinces.WaitLoading();

            // ユニットデータの読み込み完了まで待機する
            Units.WaitLoading();

            // 編集項目を初期化する
            _oobPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Oob] = true;
        }

        /// <summary>
        ///     初期部隊タブ選択時の処理
        /// </summary>
        private void OnOobTabPageSelected()
        {
            // タブページを作成する
            if (_oobPage == null)
            {
                _oobPage = new ScenarioEditorOobPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Oob].Controls.Add(_oobPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 指揮官データの読み込み完了まで待機する
            Leaders.WaitLoading();

            // プロヴィンスデータの読み込み完了まで待機する
            Provinces.WaitLoading();

            // ユニットデータの読み込み完了まで待機する
            Units.WaitLoading();

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Oob])
            {
                return;
            }

            // 編集項目を初期化する
            _oobPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Oob] = true;
        }

        #endregion

        #region 共通

        #region 共通 - 国家

        /// <summary>
        ///     国家リストボックスを更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        private static void UpdateCountryListBox(ListBox control)
        {
            control.BeginUpdate();
            control.Items.Clear();
            foreach (Country country in Countries.Tags)
            {
                control.Items.Add(Scenarios.GetCountryTagName(country));
            }
            control.EndUpdate();
        }

        /// <summary>
        ///     国家コンボボックスを更新する
        /// </summary>
        /// <param name="control">コントロール</param>
        /// <param name="allowEmpty">空項目を許可するかどうか</param>
        private void UpdateCountryComboBox(ComboBox control, bool allowEmpty)
        {
            Graphics g = Graphics.FromHwnd(Handle);
            int margin = DeviceCaps.GetScaledWidth(2) + 1;

            int width = control.Width;
            control.BeginUpdate();
            control.Items.Clear();
            if (allowEmpty)
            {
                control.Items.Add("");
            }
            foreach (Country country in Countries.Tags)
            {
                string s = Countries.GetTagName(country);
                control.Items.Add(s);
                width = Math.Max(width,
                    (int) g.MeasureString(s, control.Font).Width + SystemInformation.VerticalScrollBarWidth + margin);
            }
            control.DropDownWidth = width;
            control.EndUpdate();
        }

        /// <summary>
        ///     国家リストボックスの項目描画処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCountryListBoxDrawItem(object sender, DrawItemEventArgs e)
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

            // 背景を描画する
            e.DrawBackground();

            // 項目を描画する
            Brush brush;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                // 変更ありの項目は文字色を変更する
                CountrySettings settings = Scenarios.GetCountrySettings(Countries.Tags[e.Index]);
                brush = new SolidBrush(settings != null
                    ? (settings.IsDirty() ? Color.Red : control.ForeColor)
                    : Color.LightGray);
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

        #endregion

        #region 共通 - 編集項目

        /// <summary>
        ///     編集項目IDに関連付けられたコントロールを取得する
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        internal Control GetItemControl(ScenarioEditorItemId itemId)
        {
            return _itemControls.ContainsKey(itemId) ? _itemControls[itemId] : null;
        }

        #endregion

        #endregion
    }
}