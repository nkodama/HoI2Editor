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
            MiscGameType type = Misc.GetGameType();

            // ファイルへ書き込む
            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                writer.WriteLine("# NOTE: Obviously, the order of these variables cannot be changed.");

                foreach (MiscSectionId section in Enum.GetValues(typeof (MiscSectionId))
                    .Cast<MiscSectionId>()
                    .Where(section => Misc.SectionTable[(int) section, (int) type]))
                {
                    WriteSection(section, type, writer);
                }
            }
        }

        /// <summary>
        ///     セクションを書き出す
        /// </summary>
        /// <param name="section">セクションID</param>
        /// <param name="type">ゲームの種類</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteSection(MiscSectionId section, MiscGameType type, StreamWriter writer)
        {
            writer.WriteLine();
            writer.Write("{0} = {{", Misc.SectionStrings[(int) section]);

            // 項目のコメントと値を順に書き出す
            foreach (MiscItemId id in Misc.SectionItems[(int) section]
                .Where(id => Misc.ItemTable[(int) id, (int) type]))
            {
                writer.Write(Misc.GetComment(id));
                writer.Write(Misc.GetString(id));
            }

            // セクション末尾の空白文字/コメントを書き出す
            writer.Write(Misc.GetSuffix(section));

            writer.WriteLine("}");
        }
    }
}