using System;
using System.Linq;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究速度計算クラス
    /// </summary>
    public class Research
    {
        #region 公開プロパティ

        /// <summary>
        ///     対象技術
        /// </summary>
        public TechItem Tech { get; private set; }

        /// <summary>
        ///     対象研究機関
        /// </summary>
        public Team Team { get; private set; }

        /// <summary>
        ///     研究に要する日数
        /// </summary>
        public int Days { get; private set; }

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="tech">技術項目</param>
        /// <param name="team">研究機関</param>
        public Research(TechItem tech, Team team)
        {
            Tech = tech;
            Team = team;
            Days = GetTechDays(tech, team);
        }

        #endregion

        #region 研究速度計算

        /// <summary>
        ///     技術の研究に要する日数を取得する
        /// </summary>
        /// <param name="tech">技術項目</param>
        /// <param name="team">研究機関</param>
        /// <returns>研究に要する日数</returns>
        private static int GetTechDays(TechItem tech, Team team)
        {
            return tech.Components.Sum(component => GetComponentDays(component, tech, team));
        }

        /// <summary>
        ///     小研究の研究に要する日数を取得する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <param name="tech">技術項目</param>
        /// <param name="team">研究機関</param>
        /// <returns>研究に要する日数</returns>
        private static int GetComponentDays(TechComponent component, TechItem tech, Team team)
        {
            double progress = GetBaseProgress(component, team);
            if (Researches.YearMode == ResearchYearMode.Specified)
            {
                if (Researches.SpecifiedYear < tech.Year)
                {
                    progress *= GetPreHistoricalPenalty(tech);
                }
                else if (Researches.SpecifiedYear > tech.Year)
                {
                    progress *= GetPostHistoricalBonus(tech, component.Speciality);
                }
            }

            return (int) Math.Ceiling(100/progress);
        }

        /// <summary>
        ///     研究速度の基本進捗率を取得する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <param name="team">研究機関</param>
        /// <returns>研究速度の基本進捗率</returns>
        private static double GetBaseProgress(TechComponent component, Team team)
        {
            int d = component.Difficulty + 2;
            int s = team.Skill - 1;

            // 特性が一致する場合はスキルを倍にして6を加える
            if (team.Specialities.Contains(component.Speciality))
            {
                s += team.Skill + 6;
            }

            // 研究施設がある場合は施設の規模*10をスキルに加える
            switch (component.Speciality)
            {
                case TechSpeciality.Rocketry:
                    s += Researches.RocketTestingSites*10;
                    break;

                case TechSpeciality.NuclearPhysics:
                case TechSpeciality.NuclearEngineering:
                    s += Researches.NuclearReactors*10;
                    break;
            }

            // ゲームごとの基本進捗率を取得する
            double progress;
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    progress = (10.8 + 1.5*s)/d;
                    break;

                case GameType.ArsenalOfDemocracy:
                    progress = (3.5 + 0.5*s)/d;
                    break;

                case GameType.DarkestHour:
                    progress = (10.5 + 1.5*s)/d;
                    break;

                default:
                    // ゲームの種類が不明な場合はHoI2として扱う
                    progress = (10.8 + 1.5*s)/d;
                    break;
            }

            // 2倍時間設定の場合は進捗率半分として扱う
            if (component.DoubleTime)
            {
                progress /= 2;
            }

            // 青写真補正
            if (Researches.Blueprint)
            {
                progress *= Misc.BlueprintBonus;
            }

            // AoDの場合Miscの補正を考慮
            if (Game.Type == GameType.ArsenalOfDemocracy)
            {
                progress *= Misc.TechSpeedModifier;
            }

            // その他諸々の補正(計算機技術/閣僚/難易度/シナリオ設定など)
            progress *= Researches.Modifier;

            return progress;
        }

        /// <summary>
        ///     史実年度前ペナルティを取得する
        /// </summary>
        /// <param name="tech">技術項目</param>
        /// <returns>史実年度前ペナルティ</returns>
        private static double GetPreHistoricalPenalty(TechItem tech)
        {
            double penalty = (tech.Year - Researches.SpecifiedYear)*360*Misc.PreHistoricalDateModifier;
            double limit = (Game.Type == GameType.ArsenalOfDemocracy) ? Misc.PreHistoricalPenaltyLimit : 0.1;
            return Math.Max(1 + penalty, limit);
        }

        /// <summary>
        ///     史実年度後ボーナスを取得する
        /// </summary>
        /// <param name="tech">技術項目</param>
        /// <param name="speciality">研究特性</param>
        /// <returns>史実年度後ボーナス</returns>
        private static double GetPostHistoricalBonus(TechItem tech, TechSpeciality speciality)
        {
            double limit = 1.5;
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    return 1;

                case GameType.ArsenalOfDemocracy:
                    // 最大値はmisc.txtから取得
                    limit = Misc.PostHistoricalBonusLimit;
                    break;

                case GameType.DarkestHour:
                    // ロケット/核技術には史実年度後ボーナスが適用されない
                    switch (speciality)
                    {
                        case TechSpeciality.Rocketry:
                        case TechSpeciality.NuclearPhysics:
                        case TechSpeciality.NuclearEngineering:
                            return 1;
                    }
                    // 最大値は青写真ボーナスと同じ
                    limit = Misc.BlueprintBonus;
                    break;
            }
            double bonus = (Researches.SpecifiedYear - tech.Year)*360*Misc.PostHistoricalDateModifier;
            return Math.Min(1 + bonus, limit);
        }

        #endregion
    }
}