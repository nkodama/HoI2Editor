using HoI2Editor.Forms;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     モデル名エディタコントローラ
    /// </summary>
    internal class ModelNameEditorController
    {
        #region 内部フィールド

        /// <summary>
        ///     フォーム
        /// </summary>
        private ModelNameEditorForm _form;

        #endregion

        #region フォーム管理

        /// <summary>
        ///     フォームを開く
        /// </summary>
        internal void OpenForm()
        {
            if (_form == null)
            {
                _form = new ModelNameEditorForm(this);
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
        ///     フォームクローズ時の処理
        /// </summary>
        internal void OnFormClosed()
        {
            _form = null;
            HoI2EditorController.OnEditorStatusUpdate();
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
        ///     データ保存後の処理
        /// </summary>
        internal void OnFileSaved()
        {
            _form?.OnFileSaved();
        }

        /// <summary>
        ///     編集項目変更後の処理
        /// </summary>
        /// <param name="id">編集項目ID</param>
        internal void OnItemChanged(EditorItemId id)
        {
            _form?.OnItemChanged(id);
        }

        #endregion
    }
}