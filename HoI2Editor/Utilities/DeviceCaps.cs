﻿using System;
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
            IntPtr hDc = NativeMethods.GetDC(IntPtr.Zero);
            DpiX = NativeMethods.GetDeviceCaps(hDc, (int) DeviceCapsIndices.LogPixelsX);
            DpiY = NativeMethods.GetDeviceCaps(hDc, (int) DeviceCapsIndices.LogPixelsY);
        }

        /// <summary>
        ///     スケーリング後の幅を取得する
        /// </summary>
        /// <param name="x">スケーリング前の幅</param>
        /// <returns>スケーリング後の幅</returns>
        public static int GetScaledWidth(int x)
        {
            return NativeMethods.MulDiv(x, DpiX, DefaultDpiX);
        }

        /// <summary>
        ///     スケーリング後の高さを取得する
        /// </summary>
        /// <param name="y">スケーリング前の高さ</param>
        /// <returns>スケーリング後の高さ</returns>
        public static int GetScaledHeight(int y)
        {
            return NativeMethods.MulDiv(y, DpiY, DefaultDpiY);
        }

        /// <summary>
        ///     スケーリング前の幅を取得する
        /// </summary>
        /// <param name="x">スケーリング後の幅</param>
        /// <returns>スケーリング前の幅</returns>
        public static int GetUnscaledWidth(int x)
        {
            return NativeMethods.MulDiv(x, DefaultDpiX, DpiX);
        }

        /// <summary>
        ///     スケーリング前の高さを取得する
        /// </summary>
        /// <param name="y">スケーリング後の高さ</param>
        /// <returns>スケーリング前の高さ</returns>
        public static int GetUnscaledHeight(int y)
        {
            return NativeMethods.MulDiv(y, DefaultDpiY, DpiY);
        }

        /// <summary>
        ///     GetDeviceCapsの取得対象項目
        /// </summary>
        private enum DeviceCapsIndices
        {
            LogPixelsX = 88,
            LogPixelsY = 90
        }

        /// <summary>
        ///     P/Invokeメソッド定義用クラス
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            ///     GetDeviceCaps Win32API
            /// </summary>
            /// <param name="hdc"></param>
            /// <param name="nIndex"></param>
            /// <returns></returns>
            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

            /// <summary>
            ///     GetDC Win32API
            /// </summary>
            /// <param name="hWnd"></param>
            /// <returns></returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            /// <summary>
            ///     MulDiv Win32API
            /// </summary>
            /// <param name="nNumber"></param>
            /// <param name="nNumerator"></param>
            /// <param name="nDenominator"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);
        }
    }
}