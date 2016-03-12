using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     閣僚データ群
    /// </summary>
    internal static class Ministers
    {
        #region 公開プロパティ

        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        internal static List<Minister> Items { get; }

        /// <summary>
        ///     国タグと閣僚ファイル名の対応付け
        /// </summary>
        internal static Dictionary<Country, string> FileNameMap { get; }

        /// <summary>
        ///     使用済みIDリスト
        /// </summary>
        internal static HashSet<int> IdSet { get; }

        /// <summary>
        ///     閣僚特性一覧
        /// </summary>
        internal static MinisterPersonalityInfo[] Personalities { get; private set; }

        /// <summary>
        ///     閣僚地位と特性の対応付け
        /// </summary>
        internal static List<int>[] PositionPersonalityTable { get; }

        /// <summary>
        ///     忠誠度名
        /// </summary>
        internal static string[] LoyaltyNames { get; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     閣僚地位文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterPosition> PositionStringMap =
            new Dictionary<string, MinisterPosition>();

        /// <summary>
        ///     閣僚特性文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, int> PersonalityStringMap = new Dictionary<string, int>();

        /// <summary>
        ///     イデオロギー文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterIdeology> IdeologyStringMap =
            new Dictionary<string, MinisterIdeology>();

        /// <summary>
        ///     忠誠度文字列とIDの対応付け
        /// </summary>
        private static readonly Dictionary<string, MinisterLoyalty> LoyaltyStringMap =
            new Dictionary<string, MinisterLoyalty>();

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     遅延読み込み用
        /// </summary>
        private static readonly BackgroundWorker Worker = new BackgroundWorker();

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (Country)).Length];

        /// <summary>
        ///     閣僚リストファイルの編集済みフラグ
        /// </summary>
        private static bool _dirtyListFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     閣僚地位名
        /// </summary>
        internal static readonly TextId[] PositionNames =
        {
            TextId.Empty,
            TextId.MinisterHeadOfState,
            TextId.MinisterHeadOfGovernment,
            TextId.MinisterForeignMinister,
            TextId.MinisterArmamentMinister,
            TextId.MinisterMinisterOfSecurity,
            TextId.MinisterMinisterOfIntelligence,
            TextId.MinisterChiefOfStaff,
            TextId.MinisterChiefOfArmy,
            TextId.MinisterChiefOfNavy,
            TextId.MinisterChiefOfAir
        };

        /// <summary>
        ///     イデオロギー名
        /// </summary>
        internal static readonly TextId[] IdeologyNames =
        {
            TextId.Empty,
            TextId.IdeologyNationalSocialist,
            TextId.IdeologyFascist,
            TextId.IdeologyPaternalAutocrat,
            TextId.IdeologySocialConservative,
            TextId.IdeologyMarketLiberal,
            TextId.IdeologySocialLiberal,
            TextId.IdeologySocialDemocrat,
            TextId.IdeologyLeftWingRadical,
            TextId.IdeologyLeninist,
            TextId.IdeologyStalinist
        };

        #endregion

        #region 内部定数

        /// <summary>
        ///     閣僚地位文字列
        /// </summary>
        private static readonly string[] PositionStrings =
        {
            "",
            "Head of State",
            "Head of Government",
            "Foreign Minister",
            "Minister of Armament",
            "Minister of Security",
            "Head of Military Intelligence",
            "Chief of Staff",
            "Chief of Army",
            "Chief of Navy",
            "Chief of Air Force"
        };

        /// <summary>
        ///     閣僚特性文字列(HoI2)
        /// </summary>
        private static readonly string[] PersonalityStringsHoI2 =
        {
            "Undistinguished Suit",
            "Autocratic Charmer",
            "Barking Buffoon",
            "Benevolent Gentleman",
            "Die-hard Reformer",
            "Insignificant Layman",
            "Pig-headed Isolationist",
            "Popular Figurehead",
            "Powerhungry Demagogue",
            "Resigned Generalissimo",
            "Ruthless Powermonger",
            "Stern Imperialist",
            "Weary Stiffneck",
            "Ambitious Union Boss",
            "Backroom Backstabber",
            "Corporate Suit",
            "Flamboyant Tough Guy",
            "Happy Amateur",
            "Naive Optimist",
            "Old Admiral",
            "Old Air Marshal",
            "Old General",
            "Political Protege",
            "Silent Workhorse",
            "Smiling Oilman",
            "Apologetic Clerk",
            "Biased Intellectual",
            "Ideological Crusader",
            "Iron Fisted Brute",
            "General Staffer",
            "Great Compromiser",
            "The Cloak N Dagger Schemer",
            "Administrative Genius",
            "Air Superiority Proponent",
            "Battle Fleet Proponent",
            "Resource Industrialist",
            "Laissez-Faire Capitalist",
            "Theoretical Scientist",
            "Military Entrepreneur",
            "Submarine Proponent",
            "Tank Proponent",
            "Infantry Proponent",
            "Corrupt Kleptocrat",
            "Air to Ground Proponent",
            "Air to Sea Proponent",
            "Strategic Air Proponent",
            "Back Stabber",
            "Compassionate Gentleman",
            "Crime Fighter",
            "Crooked Kleptocrat",
            "Efficient Sociopath",
            "Man of the People",
            "Prince of Terror",
            "Silent Lawyer",
            "Dismal Enigma",
            "Industrial Specialist",
            "Logistics Specialist",
            "Naval Intelligence Specialist",
            "Political Specialist",
            "Technical Specialist",
            "School of Defence",
            "School of Fire Support",
            "School of Mass Combat",
            "School of Manoeuvre",
            "School of Psychology",
            "Armoured Spearhead Doctrine",
            "Decisive Battle Doctrine",
            "Elastic Defence Doctrine",
            "Guns and Butter Doctrine",
            "Static Defence Doctrine",
            "Base Control Doctrine",
            "Decisive Naval Battle Doctrine",
            "Indirect Approach Doctrine",
            "Open Seas Doctrine",
            "Power Projection Doctrine",
            "Air Superiority Doctrine",
            "Army Aviation Doctrine",
            "Carpet Bombing Doctrine",
            "Naval Aviation Doctrine",
            "Vertical Envelopment Doctrine"
        };

        /// <summary>
        ///     閣僚特性名(HoI2)
        /// </summary>
        private static readonly string[] PersonalityNamesHoI2 =
        {
            "NPERSONALITY_UNDISTINGUISHED_SUIT",
            "NPERSONALITY_AUTOCRATIC_CHARMER",
            "NPERSONALITY_BARKING_BUFFOON",
            "NPERSONALITY_BENEVOLENT_GENTLEMAN",
            "NPERSONALITY_DIE_HARD_REFORMER",
            "NPERSONALITY_INSIGNIFICANT_LAYMAN",
            "NPERSONALITY_PIG_HEADED_ISOLATIONIST",
            "NPERSONALITY_POPULAR_FIGUREHEAD",
            "NPERSONALITY_POWER_HUNGRY_DEMAGOGUE",
            "NPERSONALITY_RESIGNED_GENERALISSIMO",
            "NPERSONALITY_RUTHLESS_POWERMONGER",
            "NPERSONALITY_STERN_IMPERIALIST",
            "NPERSONALITY_WEARY_STIFF_NECK",
            "NPERSONALITY_AMBITIOUS_UNION_BOSS",
            "NPERSONALITY_BACKROOM_BACKSTABBER",
            "NPERSONALITY_CORPORATE_SUIT",
            "NPERSONALITY_FLAMBOYANT_TOUGH_GUY",
            "NPERSONALITY_HAPPY_AMATEUR",
            "NPERSONALITY_NAIVE_OPTIMIST",
            "NPERSONALITY_OLD_ADMIRAL",
            "NPERSONALITY_OLD_AIR_MARSHAL",
            "NPERSONALITY_OLD_GENERAL",
            "NPERSONALITY_POLITICAL_PROTEGE",
            "NPERSONALITY_SILENT_WORKHORSE",
            "NPERSONALITY_SMILING_OILMAN",
            "NPERSONALITY_APOLOGETIC_CLERK",
            "NPERSONALITY_BIASED_INTELLECTUAL",
            "NPERSONALITY_IDEOLOGICAL_CRUSADER",
            "NPERSONALITY_IRON_FISTED_BRUTE",
            "NPERSONALITY_GENERAL_STAFFER",
            "NPERSONALITY_GREAT_COMPROMISER",
            "NPERSONALITY_THE_CLOAK_N_DAGGER_SCHEMER",
            "NPERSONALITY_ADMINISTRATIVE_GENIUS",
            "NPERSONALITY_AIR_SUPERIORITY_PROPONENT",
            "NPERSONALITY_BATTLE_FLEET_PROPONENT",
            "NPERSONALITY_RESOURCE_INDUSTRIALIST",
            "NPERSONALITY_LAISSEZ_FAIRES_CAPITALIST",
            "NPERSONALITY_THEORETICAL_SCIENTIST",
            "NPERSONALITY_MILITARY_ENTREPRENEUR",
            "NPERSONALITY_SUBMARINE_PROPONENT",
            "NPERSONALITY_TANK_PROPONENT",
            "NPERSONALITY_INFANTRY_PROPONENT",
            "NPERSONALITY_CORRUPT_KLEPTOCRAT",
            "NPERSONALITY_AIR_TO_GROUND_PROPONENT",
            "NPERSONALITY_AIR_TO_SEA_PROPONENT",
            "NPERSONALITY_STRATEGIC_AIR_PROPONENT",
            "NPERSONALITY_BACK_STABBER",
            "NPERSONALITY_COMPASSIONATE_GENTLEMAN",
            "NPERSONALITY_CRIME_FIGHTER",
            "NPERSONALITY_CROOKED_KLEPTOCRAT",
            "NPERSONALITY_EFFICIENT_SOCIOPATH",
            "NPERSONALITY_MAN_OF_THE_PEOPLE",
            "NPERSONALITY_PRINCE_OF_TERROR",
            "NPERSONALITY_SILENT_LAWYER",
            "NPERSONALITY_DISMAL_ENIGMA",
            "NPERSONALITY_INDUSTRIAL_SPECIALIST",
            "NPERSONALITY_LOGISTICS_SPECIALIST",
            "NPERSONALITY_NAVAL_INTELLIGENCE_SPECIALIST",
            "NPERSONALITY_POLITICAL_SPECIALIST",
            "NPERSONALITY_TECHNICAL_SPECIALIST",
            "NPERSONALITY_SCHOOL_OF_DEFENCE",
            "NPERSONALITY_SCHOOL_OF_FIRE_SUPPORT",
            "NPERSONALITY_SCHOOL_OF_MASS_COMBAT",
            "NPERSONALITY_SCHOOL_OF_MANOEUVRE",
            "NPERSONALITY_SCHOOL_OF_PSYCHOLOGY",
            "NPERSONALITY_ARMOURED_SPEARHEAD_DOCTRINE",
            "NPERSONALITY_DECISIVE_BATTLE_DOCTRINE",
            "NPERSONALITY_ELASTIC_DEFENCE_DOCTRINE",
            "NPERSONALITY_GUNS_AND_BUTTER_DOCTRINE",
            "NPERSONALITY_STATIC_DEFENCE_DOCTRINE",
            "NPERSONALITY_BASE_CONTROL_DOCTRINE",
            "NPERSONALITY_DECISIVE_BATTLE_DOCTRINE2",
            "NPERSONALITY_INDIRECT_APPROACH_DOCTRINE",
            "NPERSONALITY_OPEN_SEAS_DOCTRINE",
            "NPERSONALITY_POWER_PROJECTION_DOCTRINE",
            "NPERSONALITY_AIR_SUPERIORITY_DOCTRINE",
            "NPERSONALITY_ARMY_AVIATION_DOCTRINE",
            "NPERSONALITY_CARPET_BOMBING_DOCTRINE",
            "NPERSONALITY_NAVAL_AVIATION_DOCTRINE",
            "NPERSONALITY_VERTICAL_ENVELOPMENT_DOCTRINE"
        };

        /// <summary>
        ///     国家元首の特性(HoI2)
        /// </summary>
        private static readonly int[] HeadOfStatePersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.AutocraticCharmer,
            (int) MinisterPersonalityHoI2.BarkingBuffoon,
            (int) MinisterPersonalityHoI2.BenevolentGentleman,
            (int) MinisterPersonalityHoI2.DieHardReformer,
            (int) MinisterPersonalityHoI2.InsignificantLayman,
            (int) MinisterPersonalityHoI2.PigHeadedIsolationist,
            (int) MinisterPersonalityHoI2.PopularFigurehead,
            (int) MinisterPersonalityHoI2.PowerHungryDemagogue,
            (int) MinisterPersonalityHoI2.ResignedGeneralissimo,
            (int) MinisterPersonalityHoI2.RuthlessPowermonger,
            (int) MinisterPersonalityHoI2.SternImperalist,
            (int) MinisterPersonalityHoI2.WearyStiffNeck,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     政府首班の特性(HoI2)
        /// </summary>
        private static readonly int[] HeadOfGovernmentPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.AmbitiousUnionBoss,
            (int) MinisterPersonalityHoI2.BackroomBackstabber,
            (int) MinisterPersonalityHoI2.CorporateSuit,
            (int) MinisterPersonalityHoI2.FlamboyantToughGuy,
            (int) MinisterPersonalityHoI2.HappyAmateur,
            (int) MinisterPersonalityHoI2.NaiveOptimist,
            (int) MinisterPersonalityHoI2.OldAdmiral,
            (int) MinisterPersonalityHoI2.OldAirMarshal,
            (int) MinisterPersonalityHoI2.OldGeneral,
            (int) MinisterPersonalityHoI2.PoliticalProtege,
            (int) MinisterPersonalityHoI2.SilentWorkhorse,
            (int) MinisterPersonalityHoI2.SmilingOilman,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     外務大臣の特性(HoI2)
        /// </summary>
        private static readonly int[] ForeignMinisterPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.ApologeticClerk,
            (int) MinisterPersonalityHoI2.BiasedIntellectual,
            (int) MinisterPersonalityHoI2.IdeologyCrusader,
            (int) MinisterPersonalityHoI2.IronFistedBrute,
            (int) MinisterPersonalityHoI2.GeneralStaffer,
            (int) MinisterPersonalityHoI2.GreatCompromiser,
            (int) MinisterPersonalityHoI2.TheCloakNDaggerSchemer,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     軍需大臣の特性(HoI2)
        /// </summary>
        private static readonly int[] MinisterOfArmamentPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.AdministrativeGenius,
            (int) MinisterPersonalityHoI2.AirSuperiorityProponent,
            (int) MinisterPersonalityHoI2.BattleFleetProponent,
            (int) MinisterPersonalityHoI2.ResourceIndustrialist,
            (int) MinisterPersonalityHoI2.LaissezFaireCapitalist,
            (int) MinisterPersonalityHoI2.TheoreticalScientist,
            (int) MinisterPersonalityHoI2.MilitaryEnterpreneur,
            (int) MinisterPersonalityHoI2.SubmarineProponent,
            (int) MinisterPersonalityHoI2.TankProponent,
            (int) MinisterPersonalityHoI2.InfantryProponent,
            (int) MinisterPersonalityHoI2.CorruptKleptocrat,
            (int) MinisterPersonalityHoI2.AirToGroundProponent,
            (int) MinisterPersonalityHoI2.AirToSeaProponent,
            (int) MinisterPersonalityHoI2.StrategicAirProponent,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     内務大臣の特性(HoI2)
        /// </summary>
        private static readonly int[] MinisterOfSecurityPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.BackStabber,
            (int) MinisterPersonalityHoI2.CompassionateGentleman,
            (int) MinisterPersonalityHoI2.CrimeFighter,
            (int) MinisterPersonalityHoI2.CrookedKleptocrat,
            (int) MinisterPersonalityHoI2.EfficientSociopath,
            (int) MinisterPersonalityHoI2.ManOfThePeople,
            (int) MinisterPersonalityHoI2.PrinceOfTerror,
            (int) MinisterPersonalityHoI2.SilentLawyer,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     情報大臣の特性(HoI2)
        /// </summary>
        private static readonly int[] HeadOfMilitaryIntelligencePersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.DismalEnigma,
            (int) MinisterPersonalityHoI2.IndustrialSpecialist,
            (int) MinisterPersonalityHoI2.LogisticsSpecialist,
            (int) MinisterPersonalityHoI2.NavalIntelligenceSpecialist,
            (int) MinisterPersonalityHoI2.PoliticalSpecialist,
            (int) MinisterPersonalityHoI2.TechnicalSpecialist,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     統合参謀総長の特性(HoI2)
        /// </summary>
        private static readonly int[] ChiefOfStaffPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.SchoolOfDefence,
            (int) MinisterPersonalityHoI2.SchoolOfFireSupport,
            (int) MinisterPersonalityHoI2.SchoolOfMassCombat,
            (int) MinisterPersonalityHoI2.SchoolOfManeuvre,
            (int) MinisterPersonalityHoI2.SchoolOfPsychology,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     陸軍総司令官の特性(HoI2)
        /// </summary>
        private static readonly int[] ChiefOfArmyPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.ArmouredSpearheadDoctrine,
            (int) MinisterPersonalityHoI2.DecisiveBattleDoctrine,
            (int) MinisterPersonalityHoI2.ElasticDefenceDoctrine,
            (int) MinisterPersonalityHoI2.GunsAndButterDoctrine,
            (int) MinisterPersonalityHoI2.StaticDefenceDoctrine,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     海軍総司令官の特性(HoI2)
        /// </summary>
        private static readonly int[] ChiefOfNavyPersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.BaseControlDoctrine,
            (int) MinisterPersonalityHoI2.DecisiveNavalBattleDoctrine,
            (int) MinisterPersonalityHoI2.IndirectApproachDoctrine,
            (int) MinisterPersonalityHoI2.OpenSeasDoctrine,
            (int) MinisterPersonalityHoI2.PowerProjectionDoctrine,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     空軍総司令官の特性(HoI2)
        /// </summary>
        private static readonly int[] ChiefOfAirForcePersonalitiesHoI2 =
        {
            (int) MinisterPersonalityHoI2.AirSuperiorityDoctrine,
            (int) MinisterPersonalityHoI2.ArmyAviationDoctrine,
            (int) MinisterPersonalityHoI2.CarpetBombingDoctrine,
            (int) MinisterPersonalityHoI2.NavalAviationDoctrine,
            (int) MinisterPersonalityHoI2.VerticalEnvelopmentDoctrine,
            (int) MinisterPersonalityHoI2.UndistinguishedSuit
        };

        /// <summary>
        ///     閣僚地位と特性の対応付け(HoI2)
        /// </summary>
        private static readonly int[][] PositionPersonalityTableHoI2 =
        {
            null,
            HeadOfStatePersonalitiesHoI2,
            HeadOfGovernmentPersonalitiesHoI2,
            ForeignMinisterPersonalitiesHoI2,
            MinisterOfArmamentPersonalitiesHoI2,
            MinisterOfSecurityPersonalitiesHoI2,
            HeadOfMilitaryIntelligencePersonalitiesHoI2,
            ChiefOfStaffPersonalitiesHoI2,
            ChiefOfArmyPersonalitiesHoI2,
            ChiefOfNavyPersonalitiesHoI2,
            ChiefOfAirForcePersonalitiesHoI2
        };

        /// <summary>
        ///     イデオロギー文字列
        /// </summary>
        private static readonly string[] IdeologyStrings =
        {
            "",
            "NS",
            "FA",
            "PA",
            "SC",
            "ML",
            "SL",
            "SD",
            "LWR",
            "LE",
            "ST"
        };

        /// <summary>
        ///     忠誠度文字列
        /// </summary>
        private static readonly string[] LoyaltyStrings =
        {
            "",
            "Very High",
            "High",
            "Medium",
            "Low",
            "Very Low",
            "Undying",
            "NA"
        };

        /// <summary>
        ///     閣僚特性文字列の先頭大文字変換の特例
        /// </summary>
        private static readonly Dictionary<string, string> PersonalityStringCaseMap
            = new Dictionary<string, string>
            {
                { "die-hard reformer", "Die-hard Reformer" },
                { "pig-headed isolationist", "Pig-headed Isolationist" },
                { "air to ground proponent", "Air to Ground Proponent" },
                { "air to sea proponent", "Air to Sea Proponent" },
                { "man of the people", "Man of the People" },
                { "prince of terror", "Prince of Terror" },
                { "school of defence", "School of Defence" },
                { "school of fire support", "School of Fire Support" },
                { "school of mass combat", "School of Mass Combat" },
                { "school of manoeuvre", "School of Manoeuvre" },
                { "school of psychology", "School of Psychology" },
                { "guns and butter doctrine", "Guns and Butter Doctrine" },
                { "health and safety", "Health and Safety" },
                { "doctrine of autonomy", "Doctrine of Autonomy" },
                { "ger_mil_m1", "ger_mil_m1" },
                { "ger_mil_m2", "ger_mil_m2" },
                { "ger_mil_m3", "ger_mil_m3" },
                { "ger_mil_m4", "ger_mil_m4" },
                { "ger_mil_m5", "ger_mil_m5" },
                { "ger_mil_m6", "ger_mil_m6" },
                { "ger_mil_m7", "ger_mil_m7" },
                { "ger_mil_m8", "ger_mil_m8" },
                { "ger_mil_m9", "ger_mil_m9" },
                { "ger_mil_m10", "ger_mil_m10" },
                { "ger_mil_m11", "ger_mil_m11" },
                { "brit_nav_mis", "brit_nav_mis" },
                { "ss reichsfuhrer", "SS Reichsfuhrer" },
                { "salesman of deception", "Salesman of Deception" },
                { "master of propaganda", "Master of Propaganda" },
                { "undersecretary of war", "Undersecretary of War" },
                { "persuader of democracies", "Persuader of Democracies" },
                { "father of united nations", "Father of United Nations" },
                { "director of fbi", "Director of FBI" },
                { "secretary of war", "Secretary of War" },
                { "ambassador to un", "Ambassador to UN" },
                { "secretary of the interior", "Secretary of the Interior" },
                { "supporter of devaluation", "Supporter of Devaluation" },
                { "opposer of the far right", "Opposer of the Far Right" },
                { "supporter of friendly relations", "Supporter of Friendly Relations" },
                { "opposer to military spending", "Opposer to Military Spending" }
            };

        /// <summary>
        ///     閣僚特性文字列のよくある綴り間違いと正しい値の関連付け
        /// </summary>
        private static readonly Dictionary<string, string> PersonalityStringTypoMap
            = new Dictionary<string, string>
            {
                { "barking buffon", "barking buffoon" },
                { "iron-fisted brute", "iron fisted brute" },
                { "the cloak-n-dagger schemer", "the cloak n dagger schemer" },
                { "cloak-n-dagger schemer", "the cloak n dagger schemer" },
                { "cloak n dagger schemer", "the cloak n dagger schemer" },
                { "laissez-faires capitalist", "laissez-faire capitalist" },
                { "laissez faires capitalist", "laissez-faire capitalist" },
                { "laissez faire capitalist", "laissez-faire capitalist" },
                { "military entrepeneur", "military entrepreneur" },
                { "crooked plutocrat", "crooked kleptocrat" },
                { "school of defense", "school of defence" },
                { "school of maneouvre", "school of manoeuvre" },
                { "elastic defense doctrine", "elastic defence doctrine" },
                { "static defense doctrine", "static defence doctrine" },
                { "vertical envelopement doctrine", "vertical envelopment doctrine" }
            };

        /// <summary>
        ///     閣僚特性(HoI2)
        /// </summary>
        private enum MinisterPersonalityHoI2
        {
            // 汎用
            UndistinguishedSuit, // 平凡な政治家

            // 国家元首
            AutocraticCharmer, // ワンマン政治家
            BarkingBuffoon, // 口先だけの道化者
            BenevolentGentleman, // 高潔な紳士
            DieHardReformer, // 不屈の改革者
            InsignificantLayman, // 無能な凡人
            PigHeadedIsolationist, // 強硬な孤立主義者
            PopularFigurehead, // 形のみの指導者
            PowerHungryDemagogue, // 権力に餓えた扇動家
            ResignedGeneralissimo, // 引退した大元帥
            RuthlessPowermonger, // 権力の亡者
            SternImperalist, // 厳格な帝国主義者
            WearyStiffNeck, // 臆病な頑固者

            // 政府首班
            AmbitiousUnionBoss, // 野心に燃える元労働組合代表
            BackroomBackstabber, // 裏工作の達人
            CorporateSuit, // 元ビジネスマン
            FlamboyantToughGuy, // 派手好きなタフガイ
            HappyAmateur, // 幸運な素人
            NaiveOptimist, // ナイーブな楽天家
            OldAdmiral, // 元海軍大将
            OldAirMarshal, // 元空軍大将
            OldGeneral, // 元陸軍大将
            PoliticalProtege, // 権力者の腰ぎんちゃく
            SilentWorkhorse, // 寡黙な勤勉家
            SmilingOilman, // にこやかな石油王

            // 外務大臣
            ApologeticClerk, // 小心な官吏
            BiasedIntellectual, // 偏見を持った知識人
            IdeologyCrusader, // イデオロギーの闘士
            IronFistedBrute, // 冷酷非道
            GeneralStaffer, // 参謀タイプ
            GreatCompromiser, // 調停の達人
            TheCloakNDaggerSchemer, // 謀略家

            // 軍需大臣
            AdministrativeGenius, // 天才的実務家
            AirSuperiorityProponent, // 制空権重視
            BattleFleetProponent, // 艦隊重視
            ResourceIndustrialist, // 鉱山企業家タイプ
            LaissezFaireCapitalist, // 自由放任主義者
            TheoreticalScientist, // 理論科学者
            MilitaryEnterpreneur, // 歴戦の勇士
            SubmarineProponent, // 潜水艦重視
            TankProponent, // 戦車重視
            InfantryProponent, // 歩兵重視
            CorruptKleptocrat, // 泥棒政治家
            AirToGroundProponent, // 空対地戦闘重視
            AirToSeaProponent, // 空対海戦闘重視
            StrategicAirProponent, // 戦略爆撃重視

            // 内務大臣
            BackStabber, // 裏工作の名人
            CompassionateGentleman, // 情け深い紳士
            CrimeFighter, // 治安重視
            CrookedKleptocrat, // ならず者政治家
            EfficientSociopath, // 反社会的効率主義者
            ManOfThePeople, // 庶民の味方
            PrinceOfTerror, // 恐怖政治の推進者
            SilentLawyer, // 物静かな法律家

            // 情報大臣
            DismalEnigma, // 不気味な謎の人物
            IndustrialSpecialist, // 産業分析の専門家
            LogisticsSpecialist, // 兵站分析の専門家
            NavalIntelligenceSpecialist, // 海軍情報の専門家
            PoliticalSpecialist, // 政治分析の専門家
            TechnicalSpecialist, // 技術分析の専門家

            // 統合参謀総長
            SchoolOfDefence, // 防衛論者
            SchoolOfFireSupport, // 火力支援論者
            SchoolOfMassCombat, // 人海戦術論者
            SchoolOfManeuvre, // 機動論者
            SchoolOfPsychology, // 精神論者

            // 陸軍総司令官
            ArmouredSpearheadDoctrine, // 機甲突撃ドクトリン
            DecisiveBattleDoctrine, // 決戦ドクトリン(陸軍)
            ElasticDefenceDoctrine, // 弾力的防御ドクトリン
            GunsAndButterDoctrine, // 軍備優先ドクトリン
            StaticDefenceDoctrine, // 静的防御ドクトリン

            // 海軍総司令官
            BaseControlDoctrine, // 基地支配ドクトリン
            DecisiveNavalBattleDoctrine, // 決戦ドクトリン(海軍)
            IndirectApproachDoctrine, // 間接接近ドクトリン
            OpenSeasDoctrine, // 外海ドクトリン
            PowerProjectionDoctrine, // 戦力投入ドクトリン

            // 空軍総司令官
            AirSuperiorityDoctrine, // 制空権ドクトリン
            ArmyAviationDoctrine, // 陸軍支援ドクトリン
            CarpetBombingDoctrine, // 絨毯爆撃ドクトリン
            NavalAviationDoctrine, // 海軍支援ドクトリン
            VerticalEnvelopmentDoctrine // 立体集中攻撃ドクトリン
        }

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Ministers()
        {
            // マスター閣僚リスト
            Items = new List<Minister>();

            // 国タグと閣僚ファイル名の対応付け
            FileNameMap = new Dictionary<Country, string>();

            // 使用済みIDリスト
            IdSet = new HashSet<int>();

            // 閣僚地位と特性の対応付け
            PositionPersonalityTable = new List<int>[Enum.GetValues(typeof (MinisterPosition)).Length];

            // 閣僚地位
            foreach (MinisterPosition position in Enum.GetValues(typeof (MinisterPosition)))
            {
                PositionStringMap.Add(PositionStrings[(int) position].ToLower(), position);
            }

            // 忠誠度
            LoyaltyNames = new[]
            {
                "",
                Resources.LoyaltyVeryHigh,
                Resources.LoyaltyHigh,
                Resources.LoyaltyMedium,
                Resources.LoyaltyLow,
                Resources.LoyaltyVeryLow,
                Resources.LoyaltyUndying,
                Resources.LoyaltyNA
            };
            foreach (MinisterLoyalty loyalty in Enum.GetValues(typeof (MinisterLoyalty)))
            {
                LoyaltyStringMap.Add(LoyaltyStrings[(int) loyalty].ToLower(), loyalty);
            }

            // イデオロギー
            foreach (MinisterIdeology ideology in Enum.GetValues(typeof (MinisterIdeology)))
            {
                IdeologyStringMap.Add(IdeologyStrings[(int) ideology].ToLower(), ideology);
            }
        }

        #endregion

        #region 閣僚特性

        /// <summary>
        ///     閣僚特性を初期化する
        /// </summary>
        internal static void InitPersonality()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // 閣僚特性を初期化する
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    // 閣僚特性を初期化する
                    InitPeronalityHoI2();
                    break;

                case GameType.ArsenalOfDemocracy:
                    // 閣僚特性定義ファイルを読み込む
                    LoadPersonalityAoD();
                    break;

                case GameType.DarkestHour:
                    // 閣僚特性定義ファイルを読み込む
                    LoadPersonalityDh();
                    break;
            }
        }

        /// <summary>
        ///     閣僚特性を初期化する(HoI2)
        /// </summary>
        private static void InitPeronalityHoI2()
        {
            int positionCount = Enum.GetValues(typeof (MinisterPosition)).Length;
            int personalityCount = Enum.GetValues(typeof (MinisterPersonalityHoI2)).Length;

            // 閣僚特性情報を初期化する
            Personalities = new MinisterPersonalityInfo[personalityCount];
            PersonalityStringMap.Clear();
            for (int i = 0; i < personalityCount; i++)
            {
                MinisterPersonalityInfo info = new MinisterPersonalityInfo
                {
                    String = PersonalityStringsHoI2[i],
                    Name = PersonalityNamesHoI2[i]
                };
                Personalities[i] = info;
                PersonalityStringMap.Add(info.String.ToLower(), i);
            }

            // 閣僚地位と閣僚特性の対応付けを初期化する
            for (int i = 0; i < positionCount; i++)
            {
                // MinisterPosition.Noneに対しては何もしない
                if (PositionPersonalityTableHoI2[i] == null)
                {
                    continue;
                }

                PositionPersonalityTable[i] = PositionPersonalityTableHoI2[i].ToList();
                foreach (int j in PositionPersonalityTable[i])
                {
                    Personalities[j].Position[i] = true;
                }
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む(AoD)
        /// </summary>
        private static void LoadPersonalityAoD()
        {
            PersonalityStringMap.Clear();
            for (int i = 0; i < Enum.GetValues(typeof (MinisterPosition)).Length; i++)
            {
                PositionPersonalityTable[i] = new List<int>();
            }

            // 閣僚特性ファイルを読み込む
            List<MinisterPersonalityInfo> list =
                MinisterModifierParser.Parse(Game.GetReadFileName(Game.MinisterPersonalityPathNameAoD));
            Personalities = list.ToArray();

            // 関連テーブルを初期化する
            for (int i = 0; i < Personalities.Length; i++)
            {
                string s = Personalities[i].String.ToLower();
                if (PersonalityStringMap.ContainsKey(s))
                {
                    Log.Warning($"[Minister] Duplicated personality strings: {Personalities[i].String}");
                }
                PersonalityStringMap[s] = i;
                Personalities[i].String = GetCasePersonalityString(Personalities[i].String.ToLower());
                for (int j = 0; j < Enum.GetValues(typeof (MinisterPosition)).Length; j++)
                {
                    if (Personalities[i].Position[j])
                    {
                        PositionPersonalityTable[j].Add(i);
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む(DH)
        /// </summary>
        private static void LoadPersonalityDh()
        {
            PersonalityStringMap.Clear();
            for (int i = 0; i < Enum.GetValues(typeof (MinisterPosition)).Length; i++)
            {
                PositionPersonalityTable[i] = new List<int>();
            }

            // 閣僚特性ファイルを読み込む
            List<MinisterPersonalityInfo> list =
                MinisterPersonalityParser.Parse(Game.GetReadFileName(Game.MinisterPersonalityPathNameDh));
            Personalities = list.ToArray();

            // 関連テーブルを初期化する
            for (int i = 0; i < Personalities.Length; i++)
            {
                string s = Personalities[i].String.ToLower();
                if (PersonalityStringMap.ContainsKey(s))
                {
                    Log.Warning($"[Minister] Duplicated personality strings: {Personalities[i].String}");
                }
                PersonalityStringMap[s] = i;
                Personalities[i].String = GetCasePersonalityString(Personalities[i].String.ToLower());
                for (int j = 0; j < Enum.GetValues(typeof (MinisterPosition)).Length; j++)
                {
                    if (Personalities[i].Position[j])
                    {
                        PositionPersonalityTable[j].Add(i);
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚特性文字列を単語の先頭だけ大文字に変換する
        /// </summary>
        /// <param name="s">閣僚特性文字列</param>
        /// <returns>変換後の文字列</returns>
        private static string GetCasePersonalityString(string s)
        {
            // 特別な変換規則のある場合
            if (PersonalityStringCaseMap.ContainsKey(s))
            {
                return PersonalityStringCaseMap[s];
            }

            // 単語の文字列だけを大文字に変換する
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(s);
        }

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     閣僚ファイルの再読み込みを要求する
        /// </summary>
        internal static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     閣僚ファイル群を再読み込みする
        /// </summary>
        internal static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            Load();
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む
        /// </summary>
        internal static void Load()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
                return;
            }

            LoadFiles();
        }

        /// <summary>
        ///     閣僚ファイル群を遅延読み込みする
        /// </summary>
        /// <param name="handler">読み込み完了イベントハンドラ</param>
        internal static void LoadAsync(RunWorkerCompletedEventHandler handler)
        {
            // 既に読み込み済みならば完了イベントハンドラを呼び出す
            if (_loaded)
            {
                handler?.Invoke(null, new RunWorkerCompletedEventArgs(null, null, false));
                return;
            }

            // 読み込み完了イベントハンドラを登録する
            if (handler != null)
            {
                Worker.RunWorkerCompleted += handler;
                Worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
            }

            // 読み込み途中ならば戻る
            if (Worker.IsBusy)
            {
                return;
            }

            // ここで読み込み済みならば既に完了イベントハンドラを呼び出しているので何もせずに戻る
            if (_loaded)
            {
                return;
            }

            // 遅延読み込みを開始する
            Worker.DoWork += OnWorkerDoWork;
            Worker.RunWorkerAsync();
        }

        /// <summary>
        ///     読み込み完了まで待機する
        /// </summary>
        internal static void WaitLoading()
        {
            while (Worker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        ///     遅延読み込み中かどうかを判定する
        /// </summary>
        /// <returns>遅延読み込み中ならばtrueを返す</returns>
        internal static bool IsLoading()
        {
            return Worker.IsBusy;
        }

        /// <summary>
        ///     遅延読み込み処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            LoadFiles();
        }

        /// <summary>
        ///     遅延読み込み完了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 遅延読み込み完了時の処理
            HoI2EditorController.OnLoadingCompleted();
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む
        /// </summary>
        private static void LoadFiles()
        {
            Items.Clear();
            IdSet.Clear();
            FileNameMap.Clear();

            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    if (!LoadHoI2())
                    {
                        return;
                    }
                    break;

                case GameType.DarkestHour:
                    if (!LoadDh())
                    {
                        return;
                    }
                    break;
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む(HoI2/AoD/DH-MOD未使用時)
        /// </summary>
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadHoI2()
        {
            List<string> fileList = new List<string>();
            string folderName;
            bool error = false;

            // 保存フォルダ内の閣僚ファイルを読み込む
            if (Game.IsExportFolderActive)
            {
                folderName = Game.GetExportFileName(Game.MinisterPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            // 閣僚ファイルを読み込む
                            LoadFile(fileName);

                            // 閣僚ファイル一覧に読み込んだファイル名を登録する
                            string name = Path.GetFileName(fileName);
                            if (!string.IsNullOrEmpty(name))
                            {
                                fileList.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Minister] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // MODフォルダ内の閣僚ファイルを読み込む
            if (Game.IsModActive)
            {
                folderName = Game.GetModFileName(Game.MinisterPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            // 閣僚ファイルを読み込む
                            LoadFile(fileName);

                            // 閣僚ファイル一覧に読み込んだファイル名を登録する
                            string name = Path.GetFileName(fileName);
                            if (!string.IsNullOrEmpty(name))
                            {
                                fileList.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            error = true;
                            Log.Error("[Minister] Read error: {0}", fileName);
                            if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                                Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // バニラフォルダ内の閣僚ファイルを読み込む
            folderName = Path.Combine(Game.FolderName, Game.MinisterPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    // MODフォルダ内で読み込んだファイルは無視する
                    string name = Path.GetFileName(fileName);
                    if (string.IsNullOrEmpty(name) || fileList.Contains(name.ToLower()))
                    {
                        continue;
                    }

                    try
                    {
                        // 閣僚ファイルを読み込む
                        LoadFile(fileName);
                    }
                    catch (Exception)
                    {
                        error = true;
                        Log.Error("[Minister] Read error: {0}", fileName);
                        if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                            Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            == DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            return !error;
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        /// <returns>読み込みに失敗すればfalseを返す</returns>
        private static bool LoadDh()
        {
            // 閣僚リストファイルが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetReadFileName(Game.DhMinisterListPathName);
            if (!File.Exists(listFileName))
            {
                return LoadHoI2();
            }

            // 閣僚リストファイルを読み込む
            IEnumerable<string> fileList;
            try
            {
                fileList = LoadList(listFileName);
            }
            catch (Exception)
            {
                Log.Error("[Minister] Read error: {0}", listFileName);
                MessageBox.Show($"{Resources.FileReadError}: {listFileName}",
                    Resources.EditorMinister, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool error = false;
            foreach (string fileName in fileList.Select(name => Game.GetReadFileName(Game.MinisterPathName, name)))
            {
                try
                {
                    // 閣僚ファイルを読み込む
                    LoadFile(fileName);
                }
                catch (Exception)
                {
                    error = true;
                    Log.Error("[Minister] Read error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return !error;
        }

        /// <summary>
        ///     閣僚リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadList(string fileName)
        {
            Log.Verbose("[Minister] Load: {0}", Path.GetFileName(fileName));

            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // 空行
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    // コメント行
                    if (line[0] == '#')
                    {
                        continue;
                    }

                    list.Add(line);
                }
            }
            return list;
        }

        /// <summary>
        ///     閣僚ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadFile(string fileName)
        {
            Log.Verbose("[Minister] Load: {0}", Path.GetFileName(fileName));

            using (CsvLexer lexer = new CsvLexer(fileName))
            {
                // 空ファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                // 国タグ読み込み
                string[] tokens = lexer.GetTokens();
                if (tokens == null || tokens.Length == 0 || string.IsNullOrEmpty(tokens[0]))
                {
                    return;
                }
                // サポート外の国タグの場合は何もしない
                if (!Countries.StringMap.ContainsKey(tokens[0].ToUpper()))
                {
                    return;
                }
                Country country = Countries.StringMap[tokens[0].ToUpper()];

                // ヘッダ行のみのファイルを読み飛ばす
                if (lexer.EndOfStream)
                {
                    return;
                }

                while (!lexer.EndOfStream)
                {
                    Minister minister = ParseLine(lexer, country);

                    // 空行を読み飛ばす
                    if (minister == null)
                    {
                        continue;
                    }

                    Items.Add(minister);
                }

                ResetDirty(country);

                if (country != Country.None && !FileNameMap.ContainsKey(country))
                {
                    FileNameMap.Add(country, lexer.FileName);
                }
            }
        }

        /// <summary>
        ///     閣僚定義行を解釈する
        /// </summary>
        /// <param name="lexer">字句解析器</param>
        /// <param name="country">国家タグ</param>
        /// <returns>閣僚データ</returns>
        private static Minister ParseLine(CsvLexer lexer, Country country)
        {
            string[] tokens = lexer.GetTokens();

            // ID指定のない行は読み飛ばす
            if (string.IsNullOrEmpty(tokens?[0]))
            {
                return null;
            }

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != (Misc.EnableRetirementYearMinisters ? 11 : (Misc.UseNewMinisterFilesFormat ? 10 : 9)))
            {
                Log.Warning("[Minister] Invalid token count: {0} ({1} L{2})", tokens.Length, lexer.FileName,
                    lexer.LineNo);
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (tokens.Length < (Misc.EnableRetirementYearMinisters ? 10 : (Misc.UseNewMinisterFilesFormat ? 9 : 8)))
                {
                    return null;
                }
            }

            Minister minister = new Minister { Country = country };
            int index = 0;

            // ID
            int id;
            if (!int.TryParse(tokens[index], out id))
            {
                Log.Warning("[Minister] Invalid id: {0} ({1} L{2})", tokens[index], lexer.FileName, lexer.LineNo);
                return null;
            }
            minister.Id = id;
            index++;

            // 閣僚地位
            string positionName = tokens[index].ToLower();
            if (PositionStringMap.ContainsKey(positionName))
            {
                minister.Position = PositionStringMap[positionName];
            }
            else
            {
                minister.Position = MinisterPosition.None;
                Log.Warning("[Minister] Invalid position: {0} [{1}] ({2} L{3})", tokens[index], minister.Id,
                    lexer.FileName, lexer.LineNo);
            }
            index++;

            // 名前
            minister.Name = tokens[index];
            index++;

            // 開始年
            int startYear;
            if (int.TryParse(tokens[index], out startYear))
            {
                minister.StartYear = startYear + (Misc.UseNewMinisterFilesFormat ? 0 : 1900);
            }
            else
            {
                minister.StartYear = 1936;
                Log.Warning("[Minister] Invalid start year: {0} [{1}: {2}] ({3} L{4})", tokens[index], minister.Id,
                    minister.Name, lexer.FileName, lexer.LineNo);
            }
            index++;

            // 終了年
            if (Misc.UseNewMinisterFilesFormat)
            {
                int endYear;
                if (int.TryParse(tokens[index], out endYear))
                {
                    minister.EndYear = endYear;
                }
                else
                {
                    minister.EndYear = 1970;
                    Log.Warning("[Minister] Invalid end year: {0} [{1}: {2}] ({3} L{4})", tokens[index], minister.Id,
                        minister.Name, lexer.FileName, lexer.LineNo);
                }
                index++;
            }
            else
            {
                minister.EndYear = 1970;
            }

            // 引退年
            if (Misc.EnableRetirementYearMinisters)
            {
                int retirementYear;
                if (int.TryParse(tokens[index], out retirementYear))
                {
                    minister.RetirementYear = retirementYear;
                }
                else
                {
                    minister.RetirementYear = 1999;
                    Log.Warning("[Minister] Invalid retirement year: {0} [{1}: {2}] ({3} L{4})", tokens[index],
                        minister.Id, minister.Name, lexer.FileName, lexer.LineNo);
                }
                index++;
            }
            else
            {
                minister.RetirementYear = 1999;
            }

            // イデオロギー
            string ideologyName = tokens[index].ToLower();
            if (IdeologyStringMap.ContainsKey(ideologyName))
            {
                minister.Ideology = IdeologyStringMap[ideologyName];
            }
            else
            {
                minister.Ideology = MinisterIdeology.None;
                Log.Warning("[Minister] Invalid ideology: {0} [{1}: {2}] ({3} L{4})", tokens[index], minister.Id,
                    minister.Name, lexer.FileName, lexer.LineNo);
            }
            index++;

            // 閣僚特性
            string personalityName = tokens[index].ToLower();
            if (PersonalityStringMap.ContainsKey(personalityName))
            {
                minister.Personality = PersonalityStringMap[personalityName];
            }
            else
            {
                if (PersonalityStringTypoMap.ContainsKey(personalityName) &&
                    PersonalityStringMap.ContainsKey(PersonalityStringTypoMap[personalityName]))
                {
                    minister.Personality = PersonalityStringMap[PersonalityStringTypoMap[personalityName]];
                    Log.Warning("[Minister] Modified personality: {0} -> {1} [{2}: {3}] ({4} L{5})", tokens[index],
                        Personalities[minister.Personality].String, minister.Id, minister.Name, lexer.FileName,
                        lexer.LineNo);
                }
                else
                {
                    minister.Personality = 0;
                    Log.Warning("[Minister] Invalid personality: {0} [{1}: {2}] ({3} L{4})", tokens[index], minister.Id,
                        minister.Name, lexer.FileName, lexer.LineNo);
                }
            }
            index++;

            // 忠誠度
            string loyaltyName = tokens[index].ToLower();
            if (LoyaltyStringMap.ContainsKey(loyaltyName))
            {
                minister.Loyalty = LoyaltyStringMap[loyaltyName];
            }
            else
            {
                minister.Loyalty = MinisterLoyalty.None;
                Log.Warning("[Minister] Invalid loyalty: {0} [{1}: {2}] ({3} L{4})", tokens[index], minister.Id,
                    minister.Name, lexer.FileName, lexer.LineNo);
            }
            index++;

            // 画像ファイル名
            minister.PictureName = tokens[index];

            return minister;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     閣僚ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        internal static bool Save()
        {
            // 編集済みでなければ何もしない
            if (!IsDirty())
            {
                return true;
            }

            // 読み込み途中ならば完了を待つ
            if (Worker.IsBusy)
            {
                WaitLoading();
            }

            // 閣僚リストファイルを保存する
            if ((Game.Type == GameType.DarkestHour) && IsDirtyList())
            {
                try
                {
                    SaveList();
                }
                catch (Exception)
                {
                    string fileName = Game.GetWriteFileName(Game.DhMinisterListPathName);
                    Log.Error("[Minister] Write error: {0}", fileName);
                    MessageBox.Show($"{Resources.FileReadError}: {fileName}",
                        Resources.EditorMinister, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            bool error = false;
            foreach (Country country in Countries.Tags
                .Where(country => DirtyFlags[(int) country] && country != Country.None))
            {
                try
                {
                    // 閣僚ファイルを保存する
                    SaveFile(country);
                }
                catch (Exception)
                {
                    error = true;
                    string fileName = Game.GetWriteFileName(Game.MinisterPathName, Game.GetMinisterFileName(country));
                    Log.Error("[Minister] Write error: {0}", fileName);
                    if (MessageBox.Show($"{Resources.FileWriteError}: {fileName}",
                        Resources.EditorMinister, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        == DialogResult.Cancel)
                        return false;
                }
            }

            // 保存に失敗していれば戻る
            if (error)
            {
                return false;
            }

            // 編集済みフラグを解除する
            _dirtyFlag = false;

            return true;
        }

        /// <summary>
        ///     閣僚リストファイルを保存する (DH)
        /// </summary>
        private static void SaveList()
        {
            // データベースフォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.DatabasePathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string fileName = Game.GetWriteFileName(Game.DhMinisterListPathName);
            Log.Info("[Minister] Save: {0}", Path.GetFileName(fileName));

            // 登録された閣僚ファイル名を順に書き込む
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                foreach (string name in FileNameMap.Select(pair => pair.Value))
                {
                    writer.WriteLine(name);
                }
            }

            // 編集済みフラグを解除する
            ResetDirtyList();
        }

        /// <summary>
        ///     閣僚ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveFile(Country country)
        {
            // 閣僚フォルダが存在しなければ作成する
            string folderName = Game.GetWriteFileName(Game.MinisterPathName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string name = Game.GetMinisterFileName(country);
            string fileName = Path.Combine(folderName, name);
            Log.Info("[Minister] Save: {0}", name);

            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                int lineNo = 3;

                // ヘッダ行を書き込む
                if (Misc.EnableRetirementYearMinisters)
                {
                    writer.WriteLine(
                        "{0};Ruling Cabinet - Start;Name;Start Year;End Year;Retirement Year;Ideology;Personality;Loyalty;Picturename;X",
                        Countries.Strings[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;;;X");
                }
                else if (Misc.UseNewMinisterFilesFormat)
                {
                    writer.WriteLine(
                        "{0};Ruling Cabinet - Start;Name;Start Year;End Year;Ideology;Personality;Loyalty;Picturename;X",
                        Countries.Strings[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;;X");
                }
                else
                {
                    writer.WriteLine("{0};Ruling Cabinet - Start;Name;Pool;Ideology;Personality;Loyalty;Picturename;x",
                        Countries.Strings[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;x");
                }

                // 閣僚定義行を順に書き込む
                foreach (Minister minister in Items.Where(minister => minister.Country == country))
                {
                    // 不正な値が設定されている場合は警告をログに出力する
                    if (minister.Position == MinisterPosition.None)
                    {
                        Log.Warning("[Minister] Invalid position: {0} {1} ({2} L{3})", minister.Id, minister.Name, name,
                            lineNo);
                    }
                    if (minister.Ideology == MinisterIdeology.None)
                    {
                        Log.Warning("[Minister] Invalid ideology: {0} {1} ({2} L{3})", minister.Id, minister.Name, name,
                            lineNo);
                    }
                    if (minister.Loyalty == MinisterLoyalty.None)
                    {
                        Log.Warning("[Minister] Invalid loyalty: {0} {1} ({2} L{3})", minister.Id, minister.Name, name,
                            lineNo);
                    }

                    // 閣僚定義行を書き込む
                    if (Misc.EnableRetirementYearMinisters)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};X",
                            minister.Id,
                            PositionStrings[(int) minister.Position],
                            minister.Name,
                            minister.StartYear,
                            minister.EndYear,
                            minister.RetirementYear,
                            IdeologyStrings[(int) minister.Ideology],
                            Personalities[minister.Personality].String,
                            LoyaltyStrings[(int) minister.Loyalty],
                            minister.PictureName);
                    }
                    else if (Misc.UseNewMinisterFilesFormat)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};X",
                            minister.Id,
                            PositionStrings[(int) minister.Position],
                            minister.Name,
                            minister.StartYear,
                            minister.EndYear,
                            IdeologyStrings[(int) minister.Ideology],
                            Personalities[minister.Personality].String,
                            LoyaltyStrings[(int) minister.Loyalty],
                            minister.PictureName);
                    }
                    else
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};x",
                            minister.Id,
                            PositionStrings[(int) minister.Position],
                            minister.Name,
                            minister.StartYear - 1900,
                            IdeologyStrings[(int) minister.Ideology],
                            Personalities[minister.Personality].String,
                            LoyaltyStrings[(int) minister.Loyalty],
                            minister.PictureName);
                    }

                    // 編集済みフラグを解除する
                    minister.ResetDirtyAll();

                    lineNo++;
                }
            }

            ResetDirty(country);
        }

        #endregion

        #region 閣僚リスト操作

        /// <summary>
        ///     閣僚リストに項目を追加する
        /// </summary>
        /// <param name="minister">挿入対象の項目</param>
        internal static void AddItem(Minister minister)
        {
            Log.Info("[Minister] Add minister: ({0}: {1}) <{2}>", minister.Id, minister.Name,
                Countries.Strings[(int) minister.Country]);

            Items.Add(minister);
        }

        /// <summary>
        ///     閣僚リストに項目を挿入する
        /// </summary>
        /// <param name="minister">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        internal static void InsertItem(Minister minister, Minister position)
        {
            int index = Items.IndexOf(position) + 1;

            Log.Info("[Minister] Insert minister: {0} ({1}: {2}) <{3}>", index, minister.Id, minister.Name,
                Countries.Strings[(int) minister.Country]);

            Items.Insert(index, minister);
        }

        /// <summary>
        ///     閣僚リストから項目を削除する
        /// </summary>
        /// <param name="minister"></param>
        internal static void RemoveItem(Minister minister)
        {
            Log.Info("[Minister] Move minister: ({0}: {1}) <{2}>", minister.Id, minister.Name,
                Countries.Strings[(int) minister.Country]);

            Items.Remove(minister);

            // 使用済みIDリストから削除する
            IdSet.Remove(minister.Id);
        }

        /// <summary>
        ///     閣僚リストの項目を移動する
        /// </summary>
        /// <param name="src">移動元の項目</param>
        /// <param name="dest">移動先の項目</param>
        internal static void MoveItem(Minister src, Minister dest)
        {
            int srcIndex = Items.IndexOf(src);
            int destIndex = Items.IndexOf(dest);

            Log.Info("[Minister] Move minister: {0} -> {1} ({2}: {3}) <{4}>", srcIndex, destIndex, src.Id, src.Name,
                Countries.Strings[(int) src.Country]);

            if (srcIndex > destIndex)
            {
                // 上へ移動する場合
                Items.Insert(destIndex, src);
                Items.RemoveAt(srcIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                Items.Insert(destIndex + 1, src);
                Items.RemoveAt(srcIndex);
            }
        }

        #endregion

        #region 一括編集

        /// <summary>
        ///     一括編集
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        internal static void BatchEdit(MinisterBatchEditArgs args)
        {
            LogBatchEdit(args);

            IEnumerable<Minister> ministers = GetBatchEditMinisters(args);
            Country newCountry;
            switch (args.ActionMode)
            {
                case BatchActionMode.Modify:
                    // 閣僚を一括編集する
                    foreach (Minister minister in ministers)
                    {
                        BatchEditMinister(minister, args);
                    }
                    break;

                case BatchActionMode.Copy:
                    // 閣僚をコピーする
                    newCountry = args.Destination;
                    int id = args.Id;
                    foreach (Minister minister in ministers)
                    {
                        id = GetNewId(id);
                        Minister newMinister = new Minister(minister)
                        {
                            Country = newCountry,
                            Id = id
                        };
                        newMinister.SetDirtyAll();
                        Items.Add(newMinister);
                    }

                    // コピー先の国の編集済みフラグを設定する
                    SetDirty(newCountry);

                    // コピー先の国がファイル一覧に存在しなければ追加する
                    if (!FileNameMap.ContainsKey(newCountry))
                    {
                        FileNameMap.Add(newCountry, Game.GetMinisterFileName(newCountry));
                        SetDirtyList();
                    }
                    break;

                case BatchActionMode.Move:
                    // 閣僚を移動する
                    newCountry = args.Destination;
                    foreach (Minister minister in ministers)
                    {
                        // 移動前の国の編集済みフラグを設定する
                        SetDirty(minister.Country);

                        minister.Country = newCountry;
                        minister.SetDirty(MinisterItemId.Country);
                    }

                    // 移動先の国の編集済みフラグを設定する
                    SetDirty(newCountry);

                    // 移動先の国がファイル一覧に存在しなければ追加する
                    if (!FileNameMap.ContainsKey(newCountry))
                    {
                        FileNameMap.Add(newCountry, Game.GetMinisterFileName(newCountry));
                        SetDirtyList();
                    }
                    break;
            }
        }

        /// <summary>
        ///     一括編集の個別処理
        /// </summary>
        /// <param name="minister">対象閣僚</param>
        /// <param name="args">一括編集のパラメータ</param>
        private static void BatchEditMinister(Minister minister, MinisterBatchEditArgs args)
        {
            // 開始年
            if (args.Items[(int) MinisterBatchItemId.StartYear])
            {
                if (minister.StartYear != args.StartYear)
                {
                    minister.StartYear = args.StartYear;
                    minister.SetDirty(MinisterItemId.StartYear);
                    SetDirty(minister.Country);
                }
            }

            // 終了年
            if (args.Items[(int) MinisterBatchItemId.EndYear])
            {
                if (minister.EndYear != args.EndYear)
                {
                    minister.EndYear = args.EndYear;
                    minister.SetDirty(MinisterItemId.EndYear);
                    SetDirty(minister.Country);
                }
            }

            // 引退年
            if (args.Items[(int) MinisterBatchItemId.RetirementYear])
            {
                if (minister.RetirementYear != args.RetirementYear)
                {
                    minister.RetirementYear = args.RetirementYear;
                    minister.SetDirty(MinisterItemId.RetirementYear);
                    SetDirty(minister.Country);
                }
            }

            // イデオロギー
            if (args.Items[(int) MinisterBatchItemId.Ideology])
            {
                if (minister.Ideology != args.Ideology)
                {
                    minister.Ideology = args.Ideology;
                    minister.SetDirty(MinisterItemId.Ideology);
                    SetDirty(minister.Country);
                }
            }

            // 忠誠度
            if (args.Items[(int) MinisterBatchItemId.Loyalty])
            {
                if (minister.Loyalty != args.Loyalty)
                {
                    minister.Loyalty = args.Loyalty;
                    minister.SetDirty(MinisterItemId.Loyalty);
                    SetDirty(minister.Country);
                }
            }
        }

        /// <summary>
        ///     一括編集対象の閣僚リストを取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>一括編集対象の閣僚リスト</returns>
        private static IEnumerable<Minister> GetBatchEditMinisters(MinisterBatchEditArgs args)
        {
            return args.CountryMode == BatchCountryMode.All
                ? Items.Where(minister => args.PositionMode[(int) minister.Position]).ToList()
                : Items.Where(minister => args.TargetCountries.Contains(minister.Country))
                    .Where(minister => args.PositionMode[(int) minister.Position]).ToList();
        }

        /// <summary>
        ///     一括編集処理のログ出力
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        private static void LogBatchEdit(MinisterBatchEditArgs args)
        {
            Log.Verbose($"[Minister] Batch {GetBatchEditItemLog(args)} ({GetBatchEditModeLog(args)})");
        }

        /// <summary>
        ///     一括編集項目のログ文字列を取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>ログ文字列</returns>
        private static string GetBatchEditItemLog(MinisterBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();
            if (args.Items[(int) MinisterBatchItemId.StartYear])
            {
                sb.Append($" start year: {args.StartYear}");
            }
            if (args.Items[(int) MinisterBatchItemId.EndYear])
            {
                sb.Append($" end year: {args.EndYear}");
            }
            if (args.Items[(int) MinisterBatchItemId.RetirementYear])
            {
                sb.Append($" retirement year: {args.RetirementYear}");
            }
            if (args.Items[(int) MinisterBatchItemId.Ideology])
            {
                sb.Append($" ideology: {IdeologyNames[(int) args.Ideology]}");
            }
            if (args.Items[(int) MinisterBatchItemId.Loyalty])
            {
                sb.Append($" loyalty: {LoyaltyNames[(int) args.Loyalty]}");
            }
            if (args.ActionMode == BatchActionMode.Copy)
            {
                sb.Append($" Copy: {Countries.Strings[(int) args.Destination]} id: {args.Id}");
            }
            else if (args.ActionMode == BatchActionMode.Move)
            {
                sb.Append($" Move: {Countries.Strings[(int) args.Destination]} id: {args.Id}");
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     一括編集対象モードのログ文字列を取得する
        /// </summary>
        /// <param name="args">一括編集のパラメータ</param>
        /// <returns>ログ文字列</returns>
        private static string GetBatchEditModeLog(MinisterBatchEditArgs args)
        {
            StringBuilder sb = new StringBuilder();

            // 一括編集対象国
            if (args.CountryMode == BatchCountryMode.All)
            {
                sb.Append("ALL");
            }
            else
            {
                foreach (Country country in args.TargetCountries)
                {
                    sb.Append($" {Countries.Strings[(int) country]}");
                }
                if (sb.Length > 0)
                {
                    sb.Remove(0, 1);
                }
            }

            // 一括編集対象地位
            if (!args.PositionMode[(int) MinisterPosition.HeadOfState] ||
                !args.PositionMode[(int) MinisterPosition.HeadOfGovernment] ||
                !args.PositionMode[(int) MinisterPosition.ForeignMinister] ||
                !args.PositionMode[(int) MinisterPosition.MinisterOfArmament] ||
                !args.PositionMode[(int) MinisterPosition.MinisterOfSecurity] ||
                !args.PositionMode[(int) MinisterPosition.HeadOfMilitaryIntelligence] ||
                !args.PositionMode[(int) MinisterPosition.ChiefOfStaff] ||
                !args.PositionMode[(int) MinisterPosition.ChiefOfArmy] ||
                !args.PositionMode[(int) MinisterPosition.ChiefOfNavy] ||
                !args.PositionMode[(int) MinisterPosition.ChiefOfAirForce])
            {
                sb.Append(
                    $"|{(args.PositionMode[(int) MinisterPosition.HeadOfState] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.HeadOfGovernment] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.ForeignMinister] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.MinisterOfArmament] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.MinisterOfSecurity] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.HeadOfMilitaryIntelligence] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.ChiefOfStaff] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.ChiefOfArmy] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.ChiefOfNavy] ? 'o' : 'x')}" +
                    $"{(args.PositionMode[(int) MinisterPosition.ChiefOfAirForce] ? 'o' : 'x')}");
            }
            return sb.ToString();
        }

        #endregion

        #region ID操作

        /// <summary>
        ///     未使用の閣僚IDを取得する
        /// </summary>
        /// <param name="country">対象の国タグ</param>
        /// <returns>閣僚ID</returns>
        internal static int GetNewId(Country country)
        {
            // 対象国の閣僚IDの最大値+1から検索を始める
            int id = GetMaxId(country);
            // 未使用IDが見つかるまでIDを1ずつ増やす
            return GetNewId(id);
        }

        /// <summary>
        ///     未使用の閣僚IDを取得する
        /// </summary>
        /// <param name="id">開始ID</param>
        /// <returns>閣僚ID</returns>
        internal static int GetNewId(int id)
        {
            while (IdSet.Contains(id))
            {
                id++;
            }
            return id;
        }

        /// <summary>
        ///     対象国の閣僚IDの最大値を取得する
        /// </summary>
        /// <param name="country">対象国</param>
        /// <returns>閣僚ID</returns>
        private static int GetMaxId(Country country)
        {
            if (country == Country.None)
            {
                return 1;
            }
            List<int> ids =
                Items.Where(minister => minister.Country == country).Select(minister => minister.Id).ToList();
            if (!ids.Any())
            {
                return 1;
            }
            return ids.Max() + 1;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        internal static bool IsDirty()
        {
            return _dirtyFlag || _dirtyListFlag;
        }

        /// <summary>
        ///     閣僚リストファイルが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        private static bool IsDirtyList()
        {
            return _dirtyListFlag;
        }

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <param name="country">国タグ</param>
        /// <returns>編集済みならばtrueを返す</returns>
        internal static bool IsDirty(Country country)
        {
            return DirtyFlags[(int) country];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="country">国タグ</param>
        internal static void SetDirty(Country country)
        {
            DirtyFlags[(int) country] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     閣僚リストファイルの編集済みフラグを設定する
        /// </summary>
        internal static void SetDirtyList()
        {
            _dirtyListFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを解除する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void ResetDirty(Country country)
        {
            DirtyFlags[(int) country] = false;
        }

        /// <summary>
        ///     閣僚リストファイルの編集済みフラグを解除する
        /// </summary>
        private static void ResetDirtyList()
        {
            _dirtyListFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     閣僚特性情報
    /// </summary>
    internal class MinisterPersonalityInfo
    {
        #region 公開プロパティ

        /// <summary>
        ///     閣僚地位と閣僚特性の対応付け
        /// </summary>
        internal bool[] Position { get; } = new bool[Enum.GetValues(typeof (MinisterPosition)).Length];

        /// <summary>
        ///     閣僚特性名
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        ///     閣僚特性名の文字列
        /// </summary>
        internal string NameText => Config.ExistsKey(Name) ? Config.GetText(Name) : Name;

        /// <summary>
        ///     閣僚特性文字列
        /// </summary>
        internal string String { get; set; }

        #endregion
    }

    /// <summary>
    ///     閣僚一括編集のパラメータ
    /// </summary>
    internal class MinisterBatchEditArgs
    {
        #region 公開プロパティ

        /// <summary>
        ///     一括編集対象国モード
        /// </summary>
        internal BatchCountryMode CountryMode { get; set; }

        /// <summary>
        ///     対象国リスト
        /// </summary>
        internal List<Country> TargetCountries { get; } = new List<Country>();

        /// <summary>
        ///     一括編集対象地位モード
        /// </summary>
        internal bool[] PositionMode { get; } = new bool[Enum.GetValues(typeof (MinisterPosition)).Length];

        /// <summary>
        ///     一括編集動作モード
        /// </summary>
        internal BatchActionMode ActionMode { get; set; }

        /// <summary>
        ///     コピー/移動先指定国
        /// </summary>
        internal Country Destination { get; set; }

        /// <summary>
        ///     開始ID
        /// </summary>
        internal int Id { get; set; }

        /// <summary>
        ///     一括編集項目
        /// </summary>
        internal bool[] Items { get; } = new bool[Enum.GetValues(typeof (MinisterBatchItemId)).Length];

        /// <summary>
        ///     開始年
        /// </summary>
        internal int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        internal int EndYear { get; set; }

        /// <summary>
        ///     引退年
        /// </summary>
        internal int RetirementYear { get; set; }

        /// <summary>
        ///     イデオロギー
        /// </summary>
        internal MinisterIdeology Ideology { get; set; }

        /// <summary>
        ///     忠誠度
        /// </summary>
        internal MinisterLoyalty Loyalty { get; set; }

        #endregion
    }

    /// <summary>
    ///     閣僚一括編集項目ID
    /// </summary>
    internal enum MinisterBatchItemId
    {
        StartYear, // 開始年
        EndYear, // 終了年
        RetirementYear, // 引退年
        Ideology, // イデオロギー
        Loyalty // 忠誠度
    }
}