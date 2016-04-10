using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HoI2Editor.Controllers;
using HoI2Editor.Models;
using HoI2Editor.Pages;

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
        ///     貿易ページ
        /// </summary>
        private ScenarioEditorTradePage _tradePage;

        /// <summary>
        ///     国家タブ
        /// </summary>
        private ScenarioEditorCountryPage _countryPage;

        /// <summary>
        ///     政府タブ
        /// </summary>
        private ScenarioEditorGovernmentPage _governmentPage;

        /// <summary>
        ///     技術タブ
        /// </summary>
        private ScenarioEditorTechnologyPage _technologyPage;

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

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Trade])
            {
                return;
            }

            // 編集項目を初期化する
            _tradePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Trade] = true;
        }

        /// <summary>
        ///     貿易タブ選択時の処理
        /// </summary>
        private void OnTradeTabPageSelected()
        {
            // タブページを作成する
            if (_tradePage == null)
            {
                _tradePage = new ScenarioEditorTradePage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Trade].Controls.Add(_tradePage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Trade])
            {
                return;
            }

            // 編集項目を初期化する
            _tradePage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Trade] = true;
        }

        /// <summary>
        ///     貿易リストビューの選択項目の文字列を設定する
        /// </summary>
        /// <param name="no">項目番号</param>
        /// <param name="s">文字列</param>
        internal void SetTradeListItemText(int no, string s)
        {
            _tradePage.SetTradeListItemText(no, s);
        }

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

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Technology])
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            Techs.WaitLoading();

            // 編集項目を初期化する
            _technologyPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Technology] = true;
        }

        /// <summary>
        ///     技術タブ選択時の処理
        /// </summary>
        private void OnTechTabPageSelected()
        {
            // タブページを作成する
            if (_technologyPage == null)
            {
                _technologyPage = new ScenarioEditorTechnologyPage(_controller, this);
                scenarioTabControl.TabPages[(int) TabPageNo.Technology].Controls.Add(_technologyPage);
            }

            // シナリオ未読み込みならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            // 技術データの読み込み完了まで待機する
            Techs.WaitLoading();

            // 初期化済みであれば何もしない
            if (_tabPageInitialized[(int) TabPageNo.Technology])
            {
                return;
            }

            // 編集項目を初期化する
            _technologyPage.UpdateItems();

            // 初期化済みフラグをセットする
            _tabPageInitialized[(int) TabPageNo.Technology] = true;
        }

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
    }
}