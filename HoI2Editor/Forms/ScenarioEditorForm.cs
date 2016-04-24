using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HoI2Editor.Controllers;

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
        ///     編集項目IDとコントロールの関連付け
        /// </summary>
        internal readonly Dictionary<ScenarioEditorItemId, Control> _itemControls =
            new Dictionary<ScenarioEditorItemId, Control>();

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
            _controller.OnFormLoad();
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
            _controller.Check();
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
        ///     タブページを関連付ける
        /// </summary>
        /// <param name="page">タブページ</param>
        /// <param name="index">タブページ番号</param>
        internal void AttachTabPage(UserControl page, int index)
        {
            scenarioTabControl.TabPages[index].Controls.Add(page);
        }

        /// <summary>
        ///     選択タブ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScenarioTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.OnSelectedTabPageChanged(scenarioTabControl.SelectedIndex);
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