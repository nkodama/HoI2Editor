using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;
using HoI2Editor.Writers;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     技術データ群
    /// </summary>
    public static class Techs
    {
        /// <summary>
        ///     技術グループリスト
        /// </summary>
        public static List<TechGroup> List = new List<TechGroup>();

        /// <summary>
        ///     研究機関編集フラグ
        /// </summary>
        public static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (TechCategory)).Length];

        /// <summary>
        ///     技術定義ファイル名テーブル
        /// </summary>
        private static readonly string[] TechFileNames =
            {
                "infantry_tech.txt",
                "armor_tech.txt",
                "naval_tech.txt",
                "aircraft_tech.txt",
                "industry_tech.txt",
                "land_doctrines_tech.txt",
                "secret_weapons_tech.txt",
                "naval_doctrines_tech.txt",
                "air_doctrines_tech.txt"
            };

        /// <summary>
        ///     研究特性画像リスト
        /// </summary>
        public static ImageList SpecialityImages;

        /// <summary>
        ///     研究特性リスト
        /// </summary>
        public static TechSpeciality[] SpecialityTable;

        /// <summary>
        ///     研究特性リスト(HoI2)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableHoI2 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting
            };

        /// <summary>
        ///     研究特性リスト(DH1.02)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableDh102 =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16
            };

        /// <summary>
        ///     研究特性リスト(DH1.03)
        /// </summary>
        private static readonly TechSpeciality[] SpecialityTableDh =
            {
                TechSpeciality.None,
                TechSpeciality.Artillery,
                TechSpeciality.Mechanics,
                TechSpeciality.Electronics,
                TechSpeciality.Chemistry,
                TechSpeciality.Training,
                TechSpeciality.GeneralEquipment,
                TechSpeciality.Rocketry,
                TechSpeciality.NavalEngineering,
                TechSpeciality.Aeronautics,
                TechSpeciality.NuclearPhysics,
                TechSpeciality.NuclearEngineering,
                TechSpeciality.Management,
                TechSpeciality.IndustrialEngineering,
                TechSpeciality.Mathematics,
                TechSpeciality.SmallUnitTactics,
                TechSpeciality.LargeUnitTactics,
                TechSpeciality.CentralizedExecution,
                TechSpeciality.DecentralizedExecution,
                TechSpeciality.TechnicalEfficiency,
                TechSpeciality.IndividualCourage,
                TechSpeciality.InfantryFocus,
                TechSpeciality.CombinedArmsFocus,
                TechSpeciality.LargeUnitFocus,
                TechSpeciality.NavalArtillery,
                TechSpeciality.NavalTraining,
                TechSpeciality.AircraftTesting,
                TechSpeciality.FighterTactics,
                TechSpeciality.BomberTactics,
                TechSpeciality.CarrierTactics,
                TechSpeciality.SubmarineTactics,
                TechSpeciality.LargeTaskforceTactics,
                TechSpeciality.SmallTaskforceTactics,
                TechSpeciality.Seamanship,
                TechSpeciality.Piloting,
                TechSpeciality.Avionics,
                TechSpeciality.Munitions,
                TechSpeciality.VehicleEngineering,
                TechSpeciality.CarrierDesign,
                TechSpeciality.SubmarineDesign,
                TechSpeciality.FighterDesign,
                TechSpeciality.BomberDesign,
                TechSpeciality.MountainTraining,
                TechSpeciality.AirborneTraining,
                TechSpeciality.MarineTraining,
                TechSpeciality.ManeuverTactics,
                TechSpeciality.BlitzkriegTactics,
                TechSpeciality.StaticDefenseTactics,
                TechSpeciality.Medicine,
                TechSpeciality.CavalryTactics,
                TechSpeciality.RtUser1,
                TechSpeciality.RtUser2,
                TechSpeciality.RtUser3,
                TechSpeciality.RtUser4,
                TechSpeciality.RtUser5,
                TechSpeciality.RtUser6,
                TechSpeciality.RtUser7,
                TechSpeciality.RtUser8,
                TechSpeciality.RtUser9,
                TechSpeciality.RtUser10,
                TechSpeciality.RtUser11,
                TechSpeciality.RtUser12,
                TechSpeciality.RtUser13,
                TechSpeciality.RtUser14,
                TechSpeciality.RtUser15,
                TechSpeciality.RtUser16,
                TechSpeciality.RtUser17,
                TechSpeciality.RtUser18,
                TechSpeciality.RtUser19,
                TechSpeciality.RtUser20,
                TechSpeciality.RtUser21,
                TechSpeciality.RtUser22,
                TechSpeciality.RtUser23,
                TechSpeciality.RtUser24,
                TechSpeciality.RtUser25,
                TechSpeciality.RtUser26,
                TechSpeciality.RtUser27,
                TechSpeciality.RtUser28,
                TechSpeciality.RtUser29,
                TechSpeciality.RtUser30,
                TechSpeciality.RtUser31,
                TechSpeciality.RtUser32,
                TechSpeciality.RtUser33,
                TechSpeciality.RtUser34,
                TechSpeciality.RtUser35,
                TechSpeciality.RtUser36,
                TechSpeciality.RtUser37,
                TechSpeciality.RtUser38,
                TechSpeciality.RtUser39,
                TechSpeciality.RtUser40,
                TechSpeciality.RtUser41,
                TechSpeciality.RtUser42,
                TechSpeciality.RtUser43,
                TechSpeciality.RtUser44,
                TechSpeciality.RtUser45,
                TechSpeciality.RtUser46,
                TechSpeciality.RtUser47,
                TechSpeciality.RtUser48,
                TechSpeciality.RtUser49,
                TechSpeciality.RtUser50,
                TechSpeciality.RtUser51,
                TechSpeciality.RtUser52,
                TechSpeciality.RtUser53,
                TechSpeciality.RtUser54,
                TechSpeciality.RtUser55,
                TechSpeciality.RtUser56,
                TechSpeciality.RtUser57,
                TechSpeciality.RtUser58,
                TechSpeciality.RtUser59,
                TechSpeciality.RtUser60
            };

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     技術定義ファイル群を読み込む
        /// </summary>
        public static void LoadTechFiles()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            List.Clear();

            foreach (TechCategory category in Enum.GetValues(typeof (TechCategory)))
            {
                string fileName = Game.GetReadFileName(Path.Combine(Game.TechPathName, TechFileNames[(int) category]));
                try
                {
                    LoadTechFile(fileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName), Resources.Error);
                }
            }

            _loaded = true;
        }

        /// <summary>
        ///     技術定義ファイルを読み込む
        /// </summary>
        /// <param name="fileName">技術定義ファイル名</param>
        private static void LoadTechFile(string fileName)
        {
            TechGroup grp = TechParser.Parse(fileName);
            List.Add(grp);
            ClearDirtyFlag(grp.Category);
        }

        /// <summary>
        ///     技術定義ファイル群を保存する
        /// </summary>
        public static void SaveTechFiles()
        {
            string folderName = Game.GetWriteFileName(Game.TechPathName);
            // 技術定義フォルダがなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            foreach (TechGroup grp in List)
            {
                if (DirtyFlags[(int) grp.Category])
                {
                    string fileName = Path.Combine(folderName, TechFileNames[(int) grp.Category]);
                    try
                    {
                        TechWriter.Write(grp, fileName);
                        ClearDirtyFlag(grp.Category);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName), Resources.Error);
                    }
                }
            }
        }

        /// <summary>
        ///     項目リストに項目を挿入する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="target">追加対象の項目</param>
        public static void AddItem(TechCategory category, object target)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Add(target);
        }

        /// <summary>
        ///     項目リストに項目を追加する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItemNext(TechCategory category, object target, object position)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Insert(grp.Items.IndexOf(position) + 1, target);
        }

        /// <summary>
        ///     項目リストから項目を削除する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="item">削除対象の項目</param>
        public static void RemoveItem(TechCategory category, object item)
        {
            TechGroup grp = List[(int) category];
            grp.Items.Remove(item);

            if (item is Tech)
            {
                var techItem = item as Tech;
                techItem.RemoveTempKey();
            }
            else if (item is TechLabel)
            {
                var labelItem = item as TechLabel;
                labelItem.RemoveTempKey();
            }
        }

        /// <summary>
        ///     項目リストの項目を移動する
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <param name="target">移動対象の項目</param>
        /// <param name="position">移動先位置の項目</param>
        public static void MoveItem(TechCategory category, object target, object position)
        {
            TechGroup grp = List[(int) category];
            int targetIndex = grp.Items.IndexOf(target);
            int positionIndex = grp.Items.IndexOf(position);

            if (targetIndex > positionIndex)
            {
                // 上へ移動する場合
                grp.Items.Insert(positionIndex, target);
                grp.Items.RemoveAt(targetIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                grp.Items.Insert(positionIndex + 1, target);
                grp.Items.RemoveAt(targetIndex);
            }
        }

        /// <summary>
        ///     研究特性を初期化する
        /// </summary>
        public static void InitSpecialities()
        {
            // 研究特性リストを設定する
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    SpecialityTable = SpecialityTableHoI2;
                    break;

                case GameType.DarkestHour:
                    SpecialityTable = (Game.Version >= 103 ? SpecialityTableDh : SpecialityTableDh102);
                    break;
            }
        }

        /// <summary>
        ///     研究特性画像リストを初期化する
        /// </summary>
        public static void InitSpecialityImages()
        {
            // 研究特性画像リストを作成する
            var bitmap = new Bitmap(Game.GetReadFileName(Game.TechIconPathName));
            SpecialityImages = new ImageList
                                   {
                                       ImageSize = new Size(24, 24),
                                       TransparentColor = bitmap.GetPixel(0, 0)
                                   };
            SpecialityImages.Images.AddStrip(bitmap);
        }

        /// <summary>
        ///     技術ファイルの再読み込みを要求する
        /// </summary>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        public static void SetDirtyFlag(TechCategory category)
        {
            DirtyFlags[(int) category] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="category">技術カテゴリ</param>
        public static void ClearDirtyFlag(TechCategory category)
        {
            DirtyFlags[(int) category] = false;
        }
    }
}