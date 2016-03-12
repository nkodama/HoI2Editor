using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     カスタムカーソル作成用のヘルパークラス
    /// </summary>
    internal static class CursorFactory
    {
        /// <summary>
        ///     カーソルを作成する
        /// </summary>
        /// <param name="bitmap">カーソル画像</param>
        /// <param name="xHotSpot">ホットスポットのX座標</param>
        /// <param name="yHotSpot">ホットスポットのY座標</param>
        /// <returns>作成したカーソル</returns>
        internal static Cursor CreateCursor(Bitmap bitmap, int xHotSpot, int yHotSpot)
        {
            Bitmap andMask = new Bitmap(bitmap.Width, bitmap.Height);
            Bitmap xorMask = new Bitmap(bitmap.Width, bitmap.Height);
            Color transparent = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if (pixel == transparent)
                    {
                        andMask.SetPixel(x, y, Color.White);
                        xorMask.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        andMask.SetPixel(x, y, Color.Black);
                        xorMask.SetPixel(x, y, pixel);
                    }
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            IconInfo info = new IconInfo();
            NativeMethods.GetIconInfo(hIcon, ref info);
            info.xHotspot = xHotSpot;
            info.yHotspot = yHotSpot;
            info.hbmMask = andMask.GetHbitmap();
            info.hbmColor = xorMask.GetHbitmap();
            info.fIcon = false;
            hIcon = NativeMethods.CreateIconIndirect(ref info);
            return new Cursor(hIcon);
        }

        /// <summary>
        ///     カーソルを作成する
        /// </summary>
        /// <param name="bitmap">カーソル画像</param>
        /// <param name="andMask">ANDマスク画像</param>
        /// <param name="xHotSpot">ホットスポットのX座標</param>
        /// <param name="yHotSpot">ホットスポットのY座標</param>
        /// <returns>作成したカーソル</returns>
        internal static Cursor CreateCursor(Bitmap bitmap, Bitmap andMask, int xHotSpot, int yHotSpot)
        {
            Bitmap xorMask = new Bitmap(bitmap.Width, bitmap.Height);
            Color transparent = andMask.GetPixel(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    xorMask.SetPixel(x, y,
                        andMask.GetPixel(x, y) == transparent ? Color.Transparent : bitmap.GetPixel(x, y));
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            IconInfo info = new IconInfo();
            NativeMethods.GetIconInfo(hIcon, ref info);
            info.xHotspot = xHotSpot;
            info.yHotspot = yHotSpot;
            info.hbmMask = andMask.GetHbitmap();
            info.hbmColor = xorMask.GetHbitmap();
            info.fIcon = false;
            hIcon = NativeMethods.CreateIconIndirect(ref info);
            return new Cursor(hIcon);
        }

        /// <summary>
        ///     P/Invokeメソッド定義用クラス
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            ///     GetIconInfo Win32API
            /// </summary>
            /// <param name="hIcon"></param>
            /// <param name="pIconInfo"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

            /// <summary>
            ///     CreateIconIndirect Win32API
            /// </summary>
            /// <param name="icon"></param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            internal static extern IntPtr CreateIconIndirect(ref IconInfo icon);
        }
    }

    /// <summary>
    ///     IconInfo Win32 struct
    /// </summary>
    internal struct IconInfo
    {
        #region fIcon

        // ReSharper disable InconsistentNaming
        internal bool fIcon;
        // ReSharper restore InconsistentNaming

        #endregion

        #region xHotspot

        // ReSharper disable InconsistentNaming
        internal int xHotspot;
        // ReSharper restore InconsistentNaming

        #endregion

        #region yHotspot

        // ReSharper disable InconsistentNaming
        internal int yHotspot;
        // ReSharper restore InconsistentNaming

        #endregion

        #region hbmMask

        // ReSharper disable InconsistentNaming
        internal IntPtr hbmMask;
        // ReSharper restore InconsistentNaming

        #endregion

        #region hbmColor

        // ReSharper disable InconsistentNaming
        internal IntPtr hbmColor;
        // ReSharper restore InconsistentNaming

        #endregion
    }
}