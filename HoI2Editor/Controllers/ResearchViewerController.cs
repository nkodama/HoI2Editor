using HoI2Editor.Forms;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     研究速度ビューアコントローラ
    /// </summary>
    internal class ResearchViewerController
    {
        #region 公開プロパティ

        /// <summary>
        ///     エディタインスタンス
        /// </summary>
        /// <remarks>リファクタリング後に削除すること</remarks>
        internal HoI2EditorInstance Instance => _instance;

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     エディタインスタンス
        /// </summary>
        private readonly HoI2EditorInstance _instance;

        /// <summary>
        ///     フォーム
        /// </summary>
        private ResearchViewerForm _form;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="instance">エディタインスタンス</param>
        internal ResearchViewerController(HoI2EditorInstance instance)
        {
            _instance = instance;
        }

        #endregion

        #region フォーム管理

        /// <summary>
        ///     フォームを開く
        /// </summary>
        internal void OpenForm()
        {
            if (_form == null)
            {
                _form = new ResearchViewerForm(this);
                _form.Show();
            }
            else
            {
                _form.Activate();
            }
        }

        /// <summary>
        ///     フォームが存在するかどうかを返す
        /// </summary>
        /// <returns>フォームが存在すればtrueを返す</returns>
        internal bool ExistsForm()
        {
            return _form != null;
        }

        /// <summary>
        ///     フォームクローズ前の処理
        /// </summary>
        /// <returns>キャンセルするならばtrueを返す</returns>
        internal bool OnFormClosing()
        {
            return _instance.QuerySave();
        }

        /// <summary>
        ///     フォームクローズ時の処理
        /// </summary>
        internal void OnFormClosed()
        {
            _form = null;
            _instance.OnEditorStatusUpdate();
        }

        #endregion

        #region データ処理

        /// <summary>
        ///     データ読み込み後の処理
        /// </summary>
        internal void OnFileLoaded()
        {
            _form?.OnFileLoaded();
        }

        /// <summary>
        ///     編集項目更新時の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal void OnItemChanged(EditorItemId id)
        {
            _form?.OnItemChanged(id);
        }

        #endregion
    }
}