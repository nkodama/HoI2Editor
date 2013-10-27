using System;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor.Writers
{
    /// <summary>
    ///     miscファイル書き込みを担当するクラス
    /// </summary>
    public static class MiscWriter
    {
        /// <summary>
        ///     miscファイルへ書き込む
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public static void Write(string fileName)
        {
            // miscファイルの種類を設定する
            MiscGameType gameType;
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    gameType = (Game.Version >= 130) ? MiscGameType.Dda13 : MiscGameType.Dda12;
                    break;

                case GameType.ArsenalOfDemocracy:
                    gameType = (Game.Version >= 108)
                                   ? MiscGameType.Aod108
                                   : ((Game.Version <= 104) ? MiscGameType.Aod104 : MiscGameType.Aod107);
                    break;

                case GameType.DarkestHour:
                    gameType = (Game.Version >= 103) ? MiscGameType.Dh103 : MiscGameType.Dh102;
                    break;

                default:
                    gameType = MiscGameType.Dda12;
                    break;
            }

            // ファイルへ書き込む
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                writer.WriteLine("# NOTE: Obviously, the order of these variables cannot be changed.");

                foreach (MiscSectionId section in Enum.GetValues(typeof (MiscSectionId)))
                {
                    if (Misc.SectionTable[(int) section, (int) gameType])
                    {
                        WriteSection(section, gameType, writer);
                    }
                }
            }
        }

        /// <summary>
        ///     セクションを書き出す
        /// </summary>
        /// <param name="sectionId">セクションID</param>
        /// <param name="gameType">ゲームの種類</param>
        /// <param name="writer">ファイル書き込み用</param>
        public static void WriteSection(MiscSectionId sectionId, MiscGameType gameType, StreamWriter writer)
        {
            MiscItemId[] itemIds
                = Misc.SectionItems[(int) sectionId].Where(id => Misc.ItemTable[(int) id, (int) gameType]).ToArray();

            writer.WriteLine();
            writer.Write("{0} = {{", Misc.SectionNames[(int) sectionId]);

            // 項目のコメントと値を順に書き出す
            int index;
            for (index = 0; index < itemIds.Length - 1; index++)
            {
                MiscItemId id = itemIds[index];
                writer.Write(Misc.GetComment(id));
                switch (Misc.ItemTypes[(int) id])
                {
                    case MiscItemType.Bool:
                        writer.Write((bool) Misc.GetItem(id) ? 1 : 0);
                        break;

                    case MiscItemType.Enum:
                    case MiscItemType.Int:
                    case MiscItemType.PosInt:
                    case MiscItemType.NonNegInt:
                    case MiscItemType.NonPosInt:
                    case MiscItemType.NonNegIntMinusOne:
                    case MiscItemType.RangedInt:
                    case MiscItemType.RangedPosInt:
                    case MiscItemType.RangedIntMinusOne:
                    case MiscItemType.RangedIntMinusThree:
                        writer.Write(IntHelper.ToString0((int) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegInt1:
                        writer.Write(IntHelper.ToString1((int) Misc.GetItem(id)));
                        break;

                    case MiscItemType.Dbl:
                    case MiscItemType.PosDbl:
                    case MiscItemType.NonNegDbl:
                    case MiscItemType.NonPosDbl:
                    case MiscItemType.NonNegDblMinusOne1:
                    case MiscItemType.RangedDbl:
                    case MiscItemType.RangedDblMinusOne1:
                        writer.Write(DoubleHelper.ToString1((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl0:
                    case MiscItemType.NonPosDbl0:
                    case MiscItemType.RangedDbl0:
                        writer.Write(DoubleHelper.ToString0((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl2:
                    case MiscItemType.NonPosDbl2:
                        writer.Write(DoubleHelper.ToString2((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl5:
                        writer.Write(DoubleHelper.ToString5((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDblMinusOne:
                    case MiscItemType.RangedDblMinusOne:
                        writer.Write(GetDbl1MinusOneString((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl2AoD:
                        writer.Write(GetDbl1AoD2String((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl4Dda13:
                        writer.Write(GetDbl1Dda134String((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonNegDbl2Dh103Full:
                        writer.Write(GetDbl1Range2String((double) Misc.GetItem(id), 0, 0.1000005));
                        break;

                    case MiscItemType.NonNegDbl2Dh103Full1:
                        writer.Write(GetDbl2Range1String((double) Misc.GetItem(id), 0, 0.2000005));
                        break;

                    case MiscItemType.NonNegDbl2Dh103Full2:
                        writer.Write(GetDbl1Range2String((double) Misc.GetItem(id), 0, 1));
                        break;
                    case MiscItemType.NonPosDbl5AoD:
                        writer.Write(GetDbl1AoD5String((double) Misc.GetItem(id)));
                        break;

                    case MiscItemType.NonPosDbl2Dh103Full:
                        writer.Write(GetDbl1Range2String((double) Misc.GetItem(id), -0.1000005, 0));
                        break;

                    case MiscItemType.NonNegIntNegDbl:
                        writer.Write(GetNonNegIntNegDblString((double) Misc.GetItem(id)));
                        break;
                }
            }
            // 最終項目の後のコメントを書き出す
            writer.Write(Misc.GetComment(itemIds[index]));

            writer.WriteLine("}");
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下1桁 or -1)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        private static string GetDbl1MinusOneString(double val)
        {
            return Math.Abs(val - (-1)) < 0.0000005 ? "-1" : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下1桁/DDA1.3 or DHのみ小数点以下4桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        private static string GetDbl1Dda134String(double val)
        {
            return ((Game.Type == GameType.HeartsOfIron2 && Game.Version >= 130) || Game.Type == GameType.DarkestHour)
                       ? DoubleHelper.ToString4(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下1桁/AoDのみ小数点以下2桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        private static string GetDbl1AoD2String(double val)
        {
            return (Game.Type == GameType.ArsenalOfDemocracy)
                       ? DoubleHelper.ToString2(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下1桁/AoDのみ小数点以下5桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        private static string GetDbl1AoD5String(double val)
        {
            return (Game.Type == GameType.ArsenalOfDemocracy)
                       ? DoubleHelper.ToString5(val)
                       : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下1桁/指定範囲内ならば小数点以下2桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <param name="min">範囲内の最小値</param>
        /// <param name="max">範囲内の最大値</param>
        /// <returns>出力文字列</returns>
        private static string GetDbl1Range2String(double val, double min, double max)
        {
            return (val > min && val < max) ? DoubleHelper.ToString2(val) : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (実数/小数点以下2桁/指定範囲外ならば小数点以下1桁)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        /// <param name="min">範囲内の最小値</param>
        /// <param name="max">範囲内の最大値</param>
        private static string GetDbl2Range1String(double val, double min, double max)
        {
            return (val > min && val < max) ? DoubleHelper.ToString2(val) : DoubleHelper.ToString1(val);
        }

        /// <summary>
        ///     出力文字列を取得する (非負の整数 or 負の実数)
        /// </summary>
        /// <param name="val">変換対象の値</param>
        /// <returns>出力文字列</returns>
        private static string GetNonNegIntNegDblString(double val)
        {
            return (val < 0) ? DoubleHelper.ToString1(val) : IntHelper.ToString0((int) val);
        }
    }
}