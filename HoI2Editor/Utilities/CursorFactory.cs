using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     カスタムカーソル作成用のヘルパークラス
    /// </summary>
    public static class CursorFactory
    {
        /// <summary>
        ///     GetIconInfo Win32API
        /// </summary>
        /// <param name="hIcon"></param>
        /// <param name="pIconInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        /// <summary>
        ///     CreateIconIndirect Win32API
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        /// <summary>
        ///     カーソルを作成する
        /// </summary>
        /// <param name="bmp">カーソル画像</param>
        /// <param name="xHotSpot">ホットスポットのX座標</param>
        /// <param name="yHotSpot">ホットスポットのY座標</param>
        /// <returns>作成したカーソル</returns>
        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr hIcon = bmp.GetHicon();
            var info = new IconInfo();
            GetIconInfo(hIcon, ref info);
            info.xHotspot = xHotSpot;
            info.yHotspot = yHotSpot;
            info.fIcon = false;
            hIcon = CreateIconIndirect(ref info);
            return new Cursor(hIcon);
        }
    }

    /// <summary>
    ///     IconInfo Win32 struct
    /// </summary>
    public struct IconInfo
    {
        #region

        // ReSharper disable InconsistentNaming
        public bool fIcon;
        // ReSharper restore InconsistentNaming

        #endregion

        #region

        // ReSharper disable InconsistentNaming
        public int xHotspot;
        // ReSharper restore InconsistentNaming

        #endregion

        #region

        // ReSharper disable InconsistentNaming
        public int yHotspot;
        // ReSharper restore InconsistentNaming

        #endregion

        #region

        // ReSharper disable InconsistentNaming
        public IntPtr hbmMask;
        // ReSharper restore InconsistentNaming

        #endregion

        #region

        // ReSharper disable InconsistentNaming
        public IntPtr hbmColor;
        // ReSharper restore InconsistentNaming

        #endregion
    }
}