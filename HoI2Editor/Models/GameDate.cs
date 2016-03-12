using System.Globalization;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     ゲーム内の日付
    /// </summary>
    public class GameDate
    {
        #region 公開プロパティ

        /// <summary>
        ///     年
        /// </summary>
        internal int Year { get; set; }

        /// <summary>
        ///     月
        /// </summary>
        internal int Month { get; set; }

        /// <summary>
        ///     日
        /// </summary>
        internal int Day { get; set; }

        /// <summary>
        ///     時
        /// </summary>
        internal int Hour { get; set; }

        #endregion

        #region 公開定数

        /// <summary>
        ///     年の最小値
        /// </summary>
        internal const int MinYear = 0;

        /// <summary>
        ///     年の最大値
        /// </summary>
        internal const int MaxYear = 9999;

        /// <summary>
        ///     月の最小値
        /// </summary>
        internal const int MinMonth = 1;

        /// <summary>
        ///     月の最大値
        /// </summary>
        internal const int MaxMonth = 12;

        /// <summary>
        ///     日の最小値
        /// </summary>
        internal const int MinDay = 1;

        /// <summary>
        ///     日の最大値
        /// </summary>
        internal const int MaxDay = 30;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        internal GameDate(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="year">年</param>
        internal GameDate(int year)
        {
            Year = year;
            Month = 1;
            Day = 1;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        internal GameDate()
        {
            Year = 1936;
            Month = 1;
            Day = 1;
        }

        /// <summary>
        ///     コピーコンストラクタ
        /// </summary>
        /// <param name="original">複製元のゲーム日時</param>
        internal GameDate(GameDate original)
        {
            Year = original.Year;
            Month = original.Month;
            Day = original.Day;
            Hour = original.Hour;
        }

        #endregion

        #region 日数計算

        /// <summary>
        ///     日数を加算する
        /// </summary>
        /// <param name="days">加算する日数</param>
        /// <returns>加算後の日付</returns>
        internal GameDate Plus(int days)
        {
            int offset = Year * 360 + (Month - 1) * 30 + (Day - 1) + days;

            int year = offset / 360;
            offset -= year * 360;
            int month = offset / 30 + 1;
            int day = offset % 30 + 1;

            return new GameDate(year, month, day);
        }

        /// <summary>
        ///     日数を減算する
        /// </summary>
        /// <param name="days">減算する日数</param>
        /// <returns>加算後の日付</returns>
        internal GameDate Minus(int days)
        {
            int offset = Year * 360 + (Month - 1) * 30 + (Day - 1) - days;

            int year = offset / 360;
            offset -= year * 360;
            int month = offset / 30 + 1;
            int day = offset % 30 + 1;

            return new GameDate(year, month, day);
        }

        /// <summary>
        ///     差分日数を取得する
        /// </summary>
        /// <param name="date">比較対象の日付</param>
        /// <returns>差分日数</returns>
        internal int Difference(GameDate date)
        {
            return (Year - date.Year) * 360 + (Month - date.Month) * 30 + (Day - date.Day);
        }

        /// <summary>
        ///     差分日数を取得する
        /// </summary>
        /// <param name="year">比較対象の年</param>
        /// <returns>差分日数</returns>
        internal int Difference(int year)
        {
            return (Year - year) * 360 + (Month - 1) * 30 + (Day - 1);
        }

        #endregion

        #region 文字列操作

        /// <summary>
        ///     日付文字列を取得する
        /// </summary>
        /// <returns>日付文字列</returns>
        public override string ToString()
        {
            string format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            format = format.Replace("yyyy", "{0}");
            format = format.Replace("MM", "{1:D2}");
            format = format.Replace("M", "{1}");
            format = format.Replace("dd", "{2:D2}");
            format = format.Replace("d", "{2}");
            return string.Format(format, Year, Month, Day);
        }

        #endregion
    }
}