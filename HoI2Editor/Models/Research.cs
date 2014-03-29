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

        /// <summary>
        ///     研究が完了する日付
        /// </summary>
        public GameDate EndDate { get; private set; }

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
            GameDate date = (Researches.DateMode == ResearchDateMode.Specified)
                ? Researches.SpecifiedDate
                : new GameDate(tech.Year);
            Days = GetTechDays(tech, team, date);
            EndDate = date.Plus(Days);
        }

        #endregion

        #region 研究速度計算

        /// <summary>
        ///     技術の研究に要する日数を取得する
        /// </summary>
        /// <param name="tech">技術項目</param>
        /// <param name="team">研究機関</param>
        /// <param name="date">開始日</param>
        /// <returns>研究に要する日数</returns>
        private static int GetTechDays(TechItem tech, Team team, GameDate date)
        {
            int offset = date.Difference(new GameDate(tech.Year));
            int days = 0;

            foreach (TechComponent component in tech.Components)
            {
                int day = GetComponentDays(component, offset, team);
                offset += day;
                days += day;
            }

            return days;
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
            int s = team.Skill;

            // 特性が一致する場合はスキルを倍にして6を加える
            if (team.Specialities.Contains(component.Speciality))
            {
                s += team.Skill + 6;
            }

            // 研究施設がある場合は施設の規模を設定する
            int t = 0;
            switch (component.Speciality)
            {
                case TechSpeciality.Rocketry:
                    t = Researches.RocketTestingSites;
                    break;

                case TechSpeciality.NuclearPhysics:
                case TechSpeciality.NuclearEngineering:
                    t = Researches.NuclearReactors;
                    break;
            }

            // ゲームごとの基本進捗率を取得する
            double progress;
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    progress = (9.3 + 1.5*s + 10*t)/d;
                    break;

                case GameType.ArsenalOfDemocracy:
                    progress = (3.0 + 0.5*(s + 10*Math.Sqrt(t)))/d;
                    break;

                case GameType.DarkestHour:
                    progress = (9.0 + 1.5*(s + 5.62*t))/d;
                    break;

                default:
                    // ゲームの種類が不明な場合はHoI2として扱う
                    progress = (9.3 + 1.5*s + 10*t)/d;
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
        ///     小研究に必要な日数を取得する
        /// </summary>
        /// <param name="component">小研究</param>
        /// <param name="offset">史実年度との差分日数</param>
        /// <param name="team">研究機関</param>
        /// <returns>日数</returns>
        private static int GetComponentDays(TechComponent component, int offset, Team team)
        {
            int totalDays = 0;
            double totalProgress = 0;

            // 基準となる進捗率を求める
            double baseProgress = GetBaseProgress(component, team);

            // STEP1: 史実年度前ペナルティ下限値で研究する日数を求める
            if ((offset < 0) && (Misc.PreHistoricalDateModifier < 0))
            {
                // 史実年度前ペナルティを求める
                double preHistoricalModifier = Misc.PreHistoricalDateModifier;

                // 史実年度前ペナルティの下限値を求める
                double preHistoricalLimit = (Game.Type == GameType.ArsenalOfDemocracy)
                    ? Misc.PreHistoricalPenaltyLimit
                    : 0.1;

                // 史実年度前ペナルティが下限値になる日を求める
                var preHistoricalLimitOffset = (int) Math.Floor((1 - preHistoricalLimit)/preHistoricalModifier);

                // 差分日数が下限値日数を超える場合
                if (offset <= preHistoricalLimitOffset)
                {
                    // 下限値で研究完了する日数を求める
                    var preHistoricalLimitDays = (int) Math.Ceiling(100/(baseProgress*preHistoricalLimit));

                    // 下限値で研究完了する場合は日数を返す
                    if (offset + preHistoricalLimitDays <= preHistoricalLimitOffset)
                    {
                        return preHistoricalLimitDays;
                    }

                    // 下限値で研究する日数と進捗を加算する
                    preHistoricalLimitDays = preHistoricalLimitOffset - offset;
                    totalDays = preHistoricalLimitDays;
                    totalProgress = baseProgress*preHistoricalLimit*preHistoricalLimitDays;
                    offset += preHistoricalLimitDays;
                }
            }

            // STEP2: 下限未達の史実年度前ペナルティで研究する日数を求める
            if (offset < 0)
            {
                // 史実年度前ペナルティを求める
                double preHistoricalModifier = Misc.PreHistoricalDateModifier;

                // 史実年度前ペナルティありで研究する日数を求める
                int preHistricalDays = GetPreHistoricalDays(baseProgress, 100 - totalProgress, offset,
                    preHistoricalModifier);

                // 史実年度前に研究完了する場合は日数を返す
                if (offset + preHistricalDays <= 0)
                {
                    totalDays += preHistricalDays;
                    return totalDays;
                }

                // 史実年度前に研究する日数と進捗を加算する
                preHistricalDays = -offset;
                totalDays += preHistricalDays;
                totalProgress += GetPreHistoricalProgress(baseProgress, preHistricalDays, offset, preHistoricalModifier);
                offset = 0;
            }

            // STEP3: 史実年度前後の補正がない状態で研究する日数を求める

            // HoI2の場合史実年度後補正がない
            if (Game.Type == GameType.HeartsOfIron2)
            {
                totalDays += (int) Math.Ceiling((100 - totalProgress)/baseProgress);
                return totalDays;
            }

            // 史実年度後ボーナスを求める
            double postHistoricalModifier = (Game.Type == GameType.ArsenalOfDemocracy)
                ? Misc.PostHistoricalDateModifierAoD
                : Misc.PostHistoricalDateModifierDh;

            // 史実年度後ボーナスがない場合
            if (postHistoricalModifier <= 0)
            {
                totalDays += (int) Math.Ceiling((100 - totalProgress)/baseProgress);
                return totalDays;
            }

            // DHの場合、1年経過までは補正なし
            if (Game.Type == GameType.DarkestHour)
            {
                // ロケット技術/核技術には史実年度後ボーナスが適用されない
                switch (component.Speciality)
                {
                    case TechSpeciality.Rocketry:
                    case TechSpeciality.NuclearPhysics:
                    case TechSpeciality.NuclearEngineering:
                        totalDays += (int) Math.Ceiling((100 - totalProgress)/baseProgress);
                        return totalDays;
                }

                offset -= 360;
                if (offset < 0)
                {
                    // 史実年度後ボーナスなしで研究する日数を求める
                    var historicalDays = (int) Math.Ceiling((100 - totalProgress)/baseProgress);

                    // 史実年度後ボーナスなしの機関に研究が完了する場合
                    if (offset + historicalDays < 0)
                    {
                        totalDays += historicalDays;
                        return totalDays;
                    }

                    // 史実年度後ボーナスなしで研究する日数と進捗を加算する
                    historicalDays = -offset;
                    totalDays += historicalDays;
                    totalProgress += baseProgress*historicalDays;
                    offset = 0;
                }
            }

            // STEP4: 上限未達の史実年度後ボーナスで研究する日数を求める

            // 史実年度後ボーナスの上限値を求める
            double postHistoricalLimit = (Game.Type == GameType.ArsenalOfDemocracy)
                ? Misc.PostHistoricalBonusLimit
                : Misc.BlueprintBonus;

            // 史実年度後ボーナスが上限値になる日を求める
            var postHistoricalLimitOffset =
                (int) Math.Ceiling(Math.Abs((postHistoricalLimit - 1)/postHistoricalModifier));

            if (offset < postHistoricalLimitOffset)
            {
                // 史実年度後ボーナスありで研究する日数を求める
                int postHistoricalDays = GetPostHistoricalDays(baseProgress, 100 - totalProgress, offset,
                    postHistoricalModifier);

                // 史実年度後ボーナスが上限値に到達する前に研究が完了する場合
                if (offset + postHistoricalDays < postHistoricalLimitOffset)
                {
                    totalDays += postHistoricalDays;
                    return totalDays;
                }

                // 史実年度後に研究する日数と進捗を加算する
                postHistoricalDays = postHistoricalLimitOffset - offset - 1;
                totalDays += postHistoricalDays;
                totalProgress += GetPostHistoricalProgress(baseProgress, postHistoricalDays, offset,
                    postHistoricalModifier);
            }

            // STEP5: 史実年度後ボーナス上限値で研究する日数を求める
            totalDays += (int) Math.Ceiling((100 - totalProgress)/(baseProgress*postHistoricalLimit));
            return totalDays;
        }

        /// <summary>
        ///     史実年度前の研究に必要な日数を取得する
        /// </summary>
        /// <param name="progress">基本進捗率</param>
        /// <param name="target">目標進捗率</param>
        /// <param name="offset">史実年度との差分日数</param>
        /// <param name="modifier">1日ごとの進捗率補正</param>
        /// <returns>日数</returns>
        private static int GetPreHistoricalDays(double progress, double target, int offset, double modifier)
        {
            return (int) Math.Ceiling(GetPositiveSolutionQuadraticEquation(
                -progress*modifier/2,
                progress/2*(2 - (2*offset + 1)*modifier),
                -target));
        }

        /// <summary>
        ///     史実年度前補正を考慮した進捗率を取得する
        /// </summary>
        /// <param name="progress">基本進捗率</param>
        /// <param name="offset">史実年度との差分日数</param>
        /// <param name="days">対象日数</param>
        /// <param name="modifier">1日ごとの進捗率補正</param>
        /// <returns>進捗率</returns>
        private static double GetPreHistoricalProgress(double progress, int days, int offset, double modifier)
        {
            return progress*days*(2 - (2*offset + days + 1)*modifier)/2;
        }

        /// <summary>
        ///     史実年度後の研究に必要な日数を取得する
        /// </summary>
        /// <param name="progress">基本進捗率</param>
        /// <param name="target">目標進捗率</param>
        /// <param name="offset">史実年度との差分日数</param>
        /// <param name="modifier">1日ごとの進捗率補正</param>
        /// <returns>日数</returns>
        private static int GetPostHistoricalDays(double progress, double target, int offset, double modifier)
        {
            return (int) Math.Ceiling(GetPositiveSolutionQuadraticEquation(
                progress*modifier/2,
                progress/2*(2 + (2*offset + 1)*modifier),
                -target));
        }

        /// <summary>
        ///     史実年度後補正を考慮した進捗率を取得する
        /// </summary>
        /// <param name="progress">基本進捗率</param>
        /// <param name="offset">史実年度との差分日数</param>
        /// <param name="days">対象日数</param>
        /// <param name="modifier">1日ごとの進捗率補正</param>
        /// <returns>進捗率</returns>
        private static double GetPostHistoricalProgress(double progress, int days, int offset, double modifier)
        {
            return progress*days*(2 + (2*offset + days + 1)*modifier)/2;
        }

        /// <summary>
        ///     2次方程式の正の解を求める
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>正の解のみを返す</returns>
        /// <remarks>
        ///     a * x^2 + b * x + c = 0
        ///     b' = b/2a, c'=c/aとして
        ///     x = -b' +- sqrt(b'^2 - c)
        ///     このうち+の方が正の解となる
        /// </remarks>
        private static double GetPositiveSolutionQuadraticEquation(double a, double b, double c)
        {
            double bb = b/a/2;
            double cc = c/a;
            return -bb + Math.Sqrt(bb*bb - cc);
        }

        #endregion
    }
}