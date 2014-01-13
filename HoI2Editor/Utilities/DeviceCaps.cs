using System;
using System.Runtime.InteropServices;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     デバイス情報のラッパークラス
    /// </summary>
    public static class DeviceCaps
    {
        /// <summary>
        ///     標準のX方向解像度
        /// </summary>
        private const int DefaultDpiX = 96;

        /// <summary>
        ///     標準のY方向解像度
        /// </summary>
        private const int DefaultDpiY = 96;

        /// <summary>
        ///     X方向解像度
        /// </summary>
        private static readonly int DpiX;

        /// <summary>
        ///     Y方向解像度
        /// </summary>
        private static readonly int DpiY;

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static DeviceCaps()
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            DpiX = GetDeviceCaps(hDc, (int) DeviceCapsIndices.LogPixelsX);
            DpiY = GetDeviceCaps(hDc, (int) DeviceCapsIndices.LogPixelsY);
        }

        /// <summary>
        ///     GetDeviceCaps Win32API
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        /// <summary>
        ///     GetDC Win32API
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        ///     MulDiv Win32API
        /// </summary>
        /// <param name="nNumber"></param>
        /// <param name="nNumerator"></param>
        /// <param name="nDenominator"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

        /// <summary>
        ///     スケーリング後の幅を取得する
        /// </summary>
        /// <param name="x">スケーリング前の幅</param>
        /// <returns>スケーリング後の幅</returns>
        public static int GetScaledWidth(int x)
        {
            return MulDiv(x, DpiX, DefaultDpiX);
        }

        /// <summary>
        ///     スケーリング後の高さを取得する
        /// </summary>
        /// <param name="y">スケーリング前の高さ</param>
        /// <returns>スケーリング後の高さ</returns>
        public static int GetScaledHeight(int y)
        {
            return MulDiv(y, DpiY, DefaultDpiY);
        }

        /// <summary>
        ///     スケーリング前の幅を取得する
        /// </summary>
        /// <param name="x">スケーリング後の幅</param>
        /// <returns>スケーリング前の幅</returns>
        public static int GetUnscaledWidth(int x)
        {
            return MulDiv(x, DefaultDpiX, DpiX);
        }

        /// <summary>
        ///     スケーリング前の高さを取得する
        /// </summary>
        /// <param name="y">スケーリング後の高さ</param>
        /// <returns>スケーリング前の高さ</returns>
        public static int GetUncaledHeight(int y)
        {
            return MulDiv(y, DefaultDpiY, DpiY);
        }

        /// <summary>
        ///     GetDeviceCapsの取得対象項目
        /// </summary>
        private enum DeviceCapsIndices
        {
            LogPixelsX = 88,
            LogPixelsY = 90,
        }
    }
}