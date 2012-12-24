using System.Collections.Generic;
using System.IO;
using HoI2Editor.Models;

namespace HoI2Editor.Writers
{
    /// <summary>
    ///     技術データのファイル書き込みを担当するクラス
    /// </summary>
    public static class TechWriter
    {
        /// <summary>
        ///     カテゴリ文字列
        /// </summary>
        private static readonly string[] CategoryStringTable = new[]
                                                                   {
                                                                       "infantry",
                                                                       "armor",
                                                                       "naval",
                                                                       "aircraft",
                                                                       "industry",
                                                                       "land_doctrines",
                                                                       "secret_weapons",
                                                                       "naval_doctrines",
                                                                       "air_doctrines"
                                                                   };

        /// <summary>
        ///     技術グループをファイルへ書き込む
        /// </summary>
        /// <param name="group">技術グループデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void Write(TechGroup group, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("technology =");
                writer.WriteLine("{{ id          = {0}", group.Id);
                writer.WriteLine("  category    = {0}", CategoryStringTable[(int) group.Category]);
                writer.WriteLine("  name        = {0}", group.Name);
                writer.WriteLine("  desc        = {0}", group.Desc);
                foreach (object item in group.Items)
                {
                    WriteTechItem(item, writer);
                }
                writer.WriteLine("}");
            }
        }

        /// <summary>
        ///     項目を書き出す
        /// </summary>
        /// <param name="item">項目</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteTechItem(object item, StreamWriter writer)
        {
            if (item is Tech)
            {
                WriteApplication(item as Tech, writer);
            }
            else if (item is TechLabel)
            {
                WriteLabel(item as TechLabel, writer);
            }
            else if (item is TechEvent)
            {
                WriteEvent(item as TechEvent, writer);
            }
        }

        /// <summary>
        ///     labelセクションを書き出す
        /// </summary>
        /// <param name="label">ラベルデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLabel(TechLabel label, StreamWriter writer)
        {
            writer.WriteLine("  label =");
            writer.WriteLine("  {{ tag      = {0}", label.Tag);
            foreach (TechPosition position in label.Positions)
            {
                writer.WriteLine("    position = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     eventセクションを書き出す
        /// </summary>
        /// <param name="ev">技術イベントデータ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteEvent(TechEvent ev, StreamWriter writer)
        {
            writer.WriteLine("  event =");
            writer.WriteLine("  {{ id         = {0}", ev.Id);
            foreach (TechPosition position in ev.Positions)
            {
                writer.WriteLine("    position   = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            writer.WriteLine("    technology = {0}", ev.Technology);
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     applicationセクションを書き出す
        /// </summary>
        /// <param name="application">技術データ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteApplication(Tech application, StreamWriter writer)
        {
            writer.WriteLine("  # ");
            writer.WriteLine("  application =");
            writer.WriteLine("  {{ id        = {0}", application.Id);
            writer.WriteLine("    name      = {0}", application.Name);
            if (!string.IsNullOrEmpty(application.Desc))
            {
                writer.WriteLine("    desc      = {0}", application.Desc);
            }
            foreach (TechPosition position in application.Positions)
            {
                writer.WriteLine("    position  = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            if (!string.IsNullOrEmpty(application.PictureName))
            {
                writer.WriteLine("    picture   = \"{0}\"", application.PictureName);
            }
            writer.WriteLine("    year      = {0}", application.Year);
            foreach (TechComponent component in application.Components)
            {
                WriteComponent(component, writer);
            }
            WriteRequired(application.Required, writer);
            if (application.OrRequired.Count > 0)
            {
                WriteOrRequired(application.OrRequired, writer);
            }
            WriteEffects(application.Effects, writer);
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     componentセクションを書き出す
        /// </summary>
        /// <param name="component">小研究データ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteComponent(TechComponent component, StreamWriter writer)
        {
            writer.WriteLine("    # ");
            writer.Write(
                "    component = {{ id = {0} name = {1} type = {2} difficulty = {3}",
                component.Id,
                component.Name,
                Tech.SpecialityStringTable[(int) component.Speciality],
                component.Difficulty);
            if (component.DoubleTime)
            {
                writer.Write(" double_time = yes");
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     requiredセクションを書き出す
        /// </summary>
        /// <param name="required">要求技術IDリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteRequired(IEnumerable<int> required, StreamWriter writer)
        {
            writer.Write("    required  = {");
            foreach (int id in required)
            {
                writer.Write(" {0}", id);
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     or_requiredセクションを書き出す
        /// </summary>
        /// <param name="required">要求技術IDリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteOrRequired(IEnumerable<int> required, StreamWriter writer)
        {
            writer.Write("    or_required = {");
            foreach (int id in required)
            {
                writer.Write(" {0}", id);
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     effectsセクションを書き出す
        /// </summary>
        /// <param name="effects">技術効果データ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteEffects(List<Command> effects, StreamWriter writer)
        {
            if (effects.Count == 0)
            {
                writer.WriteLine("    effects =");
                writer.WriteLine("    { command = { }");
                writer.WriteLine("    }");
                return;
            }

            writer.WriteLine("    effects =");
            bool first = true;
            foreach (Command command in effects)
            {
                if (first)
                {
                    writer.Write("    { command = {");
                    first = false;
                }
                else
                {
                    writer.Write("      command = {");
                }
                if (command.Triggers != null)
                {
                    writer.Write(" trigger = {");
                    foreach (Trigger trigger in command.Triggers)
                    {
                        writer.Write(" {0} = {1}", Trigger.TypeStringTable[(int) trigger.Type], trigger.Value);
                    }
                    writer.Write(" }");
                }
                writer.Write(" type = {0}", Command.TypeStringTable[(int) command.Type]);
                if (command.Which != null)
                {
                    writer.Write(" which = {0}", command.Which);
                }
                if (command.When != null)
                {
                    writer.Write(" when = {0}", command.When);
                }
                if (command.Where != null)
                {
                    writer.Write(" where = {0}", command.Where);
                }
                if (command.Value != null)
                {
                    writer.Write(" value = {0}", command.Value);
                }
                writer.WriteLine(" }");
            }
            writer.WriteLine("    }");
        }
    }
}