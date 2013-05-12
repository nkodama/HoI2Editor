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
        #region ファイル書き込み

        /// <summary>
        ///     技術グループをファイルへ書き込む
        /// </summary>
        /// <param name="grp">技術グループデータ</param>
        /// <param name="fileName">ファイル名</param>
        public static void Write(TechGroup grp, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("technology =");
                writer.WriteLine("{{ id          = {0}", grp.Id);
                writer.WriteLine("  category    = {0}", Techs.CategoryStrings[(int) grp.Category]);
                writer.WriteLine("  name        = {0} # Localized name", grp.Name);
                writer.WriteLine("  desc        = {0} # Localized description", grp.Desc);
                foreach (ITechItem item in grp.Items)
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
        private static void WriteTechItem(ITechItem item, StreamWriter writer)
        {
            if (item is TechItem)
            {
                WriteApplication(item as TechItem, writer);
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
        /// <param name="item">技術ラベル項目</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteLabel(TechLabel item, StreamWriter writer)
        {
            writer.WriteLine("  label =");
            writer.WriteLine("  {{ tag      = {0}", item.Name);
            foreach (TechPosition position in item.Positions)
            {
                writer.WriteLine("    position = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     eventセクションを書き出す
        /// </summary>
        /// <param name="item">技術イベント項目</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteEvent(TechEvent item, StreamWriter writer)
        {
            writer.WriteLine("  event =");
            writer.WriteLine("  {{ id         = {0}", item.Id);
            foreach (TechPosition position in item.Positions)
            {
                writer.WriteLine("    position   = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            writer.WriteLine("    technology = {0}", item.TechId);
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     applicationセクションを書き出す
        /// </summary>
        /// <param name="item">技術項目</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteApplication(TechItem item, StreamWriter writer)
        {
            writer.WriteLine("  # {0}", Config.GetText(item.Name));
            writer.WriteLine("  application =");
            writer.WriteLine("  {{ id        = {0}", item.Id);
            writer.WriteLine("    name      = {0}", item.Name);
            if (!string.IsNullOrEmpty(item.Desc))
            {
                writer.WriteLine("    desc      = {0}", item.Desc);
            }
            foreach (TechPosition position in item.Positions)
            {
                writer.WriteLine("    position  = {{ x = {0} y = {1} }}", position.X, position.Y);
            }
            if (!string.IsNullOrEmpty(item.PictureName))
            {
                writer.WriteLine("    picture   = \"{0}\"", item.PictureName);
            }
            writer.WriteLine("    year      = {0}", item.Year);
            foreach (TechComponent component in item.Components)
            {
                WriteComponent(component, writer);
            }
            WriteRequired(item.AndRequiredTechs, writer);
            if (item.OrRequiredTechs.Count > 0)
            {
                WriteOrRequired(item.OrRequiredTechs, writer);
            }
            WriteEffects(item.Effects, writer);
            writer.WriteLine("  }");
        }

        /// <summary>
        ///     componentセクションを書き出す
        /// </summary>
        /// <param name="component">小研究データ</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteComponent(TechComponent component, StreamWriter writer)
        {
            writer.WriteLine("    # {0}", Config.GetText(component.Name));
            writer.Write(
                "    component = {{ id = {0} name = {1} type = {2} difficulty = {3}",
                component.Id,
                component.Name,
                Techs.SpecialityStrings[(int) component.Speciality],
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
        /// <param name="techs">要求技術IDリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteRequired(IEnumerable<RequiredTech> techs, StreamWriter writer)
        {
            writer.Write("    required  = {");
            foreach (RequiredTech tech in techs)
            {
                writer.Write(" {0}", tech.Id);
            }
            writer.WriteLine(" }");
        }

        /// <summary>
        ///     or_requiredセクションを書き出す
        /// </summary>
        /// <param name="techs">要求技術IDリスト</param>
        /// <param name="writer">ファイル書き込み用</param>
        private static void WriteOrRequired(IEnumerable<RequiredTech> techs, StreamWriter writer)
        {
            writer.Write("    or_required = {");
            foreach (RequiredTech tech in techs)
            {
                writer.Write(" {0}", tech.Id);
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
                if (Game.Type == GameType.DarkestHour && command.Triggers != null && command.Triggers.Count > 0)
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

        #endregion
    }
}