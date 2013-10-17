using System;
using System.IO;
using System.Linq;
using System.Text;
using HoI2Editor.Models;

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
            writer.WriteLine("{0} = {");
        }
    }
}