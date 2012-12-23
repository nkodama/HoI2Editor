using System;
using System.Collections.Generic;
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
    public static class Ministers
    {
        /// <summary>
        ///     マスター閣僚リスト
        /// </summary>
        public static List<Minister> List = new List<Minister>();

        /// <summary>
        ///     閣僚編集フラグ
        /// </summary>
        public static readonly bool[] DirtyFlags = new bool[Enum.GetValues(typeof (CountryTag)).Length];

        /// <summary>
        ///     閣僚地位テーブル
        /// </summary>
        public static MinisterPositionInfo[] PositionTable;

        /// <summary>
        ///     閣僚特性テーブル
        /// </summary>
        public static MinisterPersonalityInfo[] PersonalityTable;

        /// <summary>
        ///     閣僚忠誠度テーブル
        /// </summary>
        public static MinisterLoyaltyInfo[] LoyaltyTable;

        /// <summary>
        ///     閣僚イデオロギーテーブル
        /// </summary>
        public static MinisterIdeologyInfo[] IdeologyTable;

        /// <summary>
        ///     閣僚地位と特性の対応付け
        /// </summary>
        public static List<int>[] PositionPersonalityTable =
            new List<int>[Enum.GetValues(typeof (MinisterPosition)).Length];

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
        ///     閣僚地位文字列
        /// </summary>
        private static readonly string[] PositionStringTable =
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
        ///     閣僚地位名
        /// </summary>
        private static readonly string[] PositionNameTable =
            {
                "",
                "HOIG_HEAD_OF_STATE",
                "HOIG_HEAD_OF_GOVERNMENT",
                "HOIG_FOREIGN_MINISTER",
                "HOIG_ARMAMENT_MINISTER",
                "HOIG_MINISTER_OF_SECURITY",
                "HOIG_MINISTER_OF_INTELLIGENCE",
                "HOIG_CHIEF_OF_STAFF",
                "HOIG_CHIEF_OF_ARMY",
                "HOIG_CHIEF_OF_NAVY",
                "HOIG_CHIEF_OF_AIR"
            };

        /// <summary>
        ///     閣僚特性文字列(HoI2)
        /// </summary>
        private static readonly string[] PersonalityStringTableHoI2 =
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
                "Laissez Faires Capitalist",
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
        public static readonly string[] PersonalityNameTableHoI2 =
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
                (int) MinisterPersonalityHoI2.LaissezFairesCapitalist,
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
        private static readonly string[] IdeologyStringTable =
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
        ///     イデオロギー名
        /// </summary>
        private static readonly string[] IdeologyNameTable =
            {
                "",
                "CATEGORY_NATIONAL_SOCIALIST",
                "CATEGORY_FASCIST",
                "CATEGORY_PATERNAL_AUTOCRAT",
                "CATEGORY_SOCIAL_CONSERVATIVE",
                "CATEGORY_MARKET_LIBERAL",
                "CATEGORY_SOCIAL_LIBERAL",
                "CATEGORY_SOCIAL_DEMOCRAT",
                "CATEGORY_LEFT_WING_RADICAL",
                "CATEGORY_LENINIST",
                "CATEGORY_STALINIST"
            };

        /// <summary>
        ///     閣僚特性文字列の先頭大文字変換の特例
        /// </summary>
        private static readonly Dictionary<string, string> PersonalityTitleCaseMap
            = new Dictionary<string, string>
                  {
                      {"die-hard reformer", "Die-hard Reformer"},
                      {"pig-headed isolationist", "Pig-headed Isolationist"},
                      {"air to ground proponent", "Air to Ground Proponent"},
                      {"air to sea proponent", "Air to Sea Proponent"},
                      {"man of the people", "Man of the People"},
                      {"prince of terror", "Prince of Terror"},
                      {"school of defence", "School of Defence"},
                      {"school of fire support", "School of Fire Support"},
                      {"school of mass combat", "School of Mass Combat"},
                      {"school of manoeuvre", "School of Manoeuvre"},
                      {"school of psychology", "School of Psychology"},
                      {"guns and butter doctrine", "Guns and Butter Doctrine"},
                      {"health and safety", "Health and Safety"},
                      {"doctrine of autonomy", "Doctrine of Autonomy"},
                      {"ger_mil_m1", "ger_mil_m1"},
                      {"ger_mil_m2", "ger_mil_m2"},
                      {"ger_mil_m3", "ger_mil_m3"},
                      {"ger_mil_m4", "ger_mil_m4"},
                      {"ger_mil_m5", "ger_mil_m5"},
                      {"ger_mil_m6", "ger_mil_m6"},
                      {"ger_mil_m7", "ger_mil_m7"},
                      {"ger_mil_m8", "ger_mil_m8"},
                      {"ger_mil_m9", "ger_mil_m9"},
                      {"ger_mil_m10", "ger_mil_m10"},
                      {"ger_mil_m11", "ger_mil_m11"},
                      {"brit_nav_mis", "brit_nav_mis"},
                      {"ss reichsfuhrer", "SS Reichsfuhrer"},
                      {"salesman of deception", "Salesman of Deception"},
                      {"master of propaganda", "Master of Propaganda"},
                      {"undersecretary of war", "Undersecretary of War"},
                      {"persuader of democracies", "Persuader of Democracies"},
                      {"father of united nations", "Father of United Nations"},
                      {"director of fbi", "Director of FBI"},
                      {"secretary of war", "Secretary of War"},
                      {"ambassador to un", "Ambassador to UN"},
                      {"secretary of the interior", "Secretary of the Interior"},
                      {"supporter of devaluation", "Supporter of Devaluation"},
                      {"opposer of the far right", "Opposer of the Far Right"},
                      {"supporter of friendly relations", "Supporter of Friendly Relations"},
                      {"opposer to military spending", "Opposer to Military Spending"}
                  };

        /// <summary>
        ///     閣僚特性文字列のよくある綴り間違いと正しい値の関連付け
        /// </summary>
        private static readonly Dictionary<string, string> PersonalityTypoMap
            = new Dictionary<string, string>
                  {
                      {"barking buffon", "barking buffoon"},
                      {"iron-fisted brute", "iron fisted brute"},
                      {"the cloak-n-dagger schemer", "the cloak n dagger schemer"},
                      {"cloak-n-dagger schemer", "the cloak n dagger schemer"},
                      {"cloak n dagger schemer", "the cloak n dagger schemer"},
                      {"laissez-faires capitalist", "laissez faires capitalist"},
                      {"laissez faire capitalist", "laissez faires capitalist"},
                      {"laissez-faire capitalist", "laissez faires capitalist"},
                      {"military entrepeneur", "military entrepreneur"},
                      {"crooked plutocrat", "crooked kleptocrat"},
                      {"school of defense", "school of defence"},
                      {"school of maneouvre", "school of manoeuvre"},
                      {"elastic defense doctrine", "elastic defence doctrine"},
                      {"static defense doctrine", "static defence doctrine"},
                      {"vertical envelopement doctrine", "vertical envelopment doctrine"},
                  };

        /// <summary>
        ///     現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        ///     現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

        /// <summary>
        ///     CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static Ministers()
        {
            // 閣僚地位
            var positionList = new List<MinisterPositionInfo>();
            foreach (MinisterPosition position in Enum.GetValues(typeof (MinisterPosition)))
            {
                var info = new MinisterPositionInfo
                               {
                                   String = PositionStringTable[(int) position],
                                   Name = PositionNameTable[(int) position]
                               };
                positionList.Add(info);
                PositionStringMap.Add(info.String.ToLower(), position);
            }
            PositionTable = positionList.ToArray();

            // 閣僚忠誠度
            LoyaltyTable = new[]
                               {
                                   new MinisterLoyaltyInfo {String = "", Name = ""},
                                   new MinisterLoyaltyInfo {String = "Very High", Name = Resources.LoyaltyVeryHigh},
                                   new MinisterLoyaltyInfo {String = "High", Name = Resources.LoyaltyHigh},
                                   new MinisterLoyaltyInfo {String = "Medium", Name = Resources.LoyaltyMedium},
                                   new MinisterLoyaltyInfo {String = "Low", Name = Resources.LoyaltyLow},
                                   new MinisterLoyaltyInfo {String = "Very Low", Name = Resources.LoyaltyVeryLow},
                                   new MinisterLoyaltyInfo {String = "Undying", Name = Resources.LoyaltyUndying},
                                   new MinisterLoyaltyInfo {String = "NA", Name = Resources.LoyaltyNA}
                               };
            for (int i = 0; i < LoyaltyTable.Length; i++)
            {
                LoyaltyStringMap.Add(LoyaltyTable[i].String.ToLower(), (MinisterLoyalty) i);
            }

            // 閣僚イデオロギー
            var ideologyList = new List<MinisterIdeologyInfo>();
            foreach (MinisterIdeology ideology in Enum.GetValues(typeof (MinisterIdeology)))
            {
                var info = new MinisterIdeologyInfo
                               {
                                   String = IdeologyStringTable[(int) ideology],
                                   Name = IdeologyNameTable[(int) ideology]
                               };
                ideologyList.Add(info);
                IdeologyStringMap.Add(info.String.ToLower(), ideology);
            }
            IdeologyTable = ideologyList.ToArray();
        }

        /// <summary>
        ///     閣僚特性を読み込む
        /// </summary>
        public static void LoadMinisterPersonality()
        {
            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                    LoadMinisterPeronalityHoI2();
                    break;

                case GameType.ArsenalOfDemocracy:
                    LoadMinisterPersonalityAoD();
                    break;

                case GameType.DarkestHour:
                    LoadMinisterPersonalityDh();
                    break;
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む(HoI2)
        /// </summary>
        private static void LoadMinisterPeronalityHoI2()
        {
            int positionCount = Enum.GetValues(typeof (MinisterPosition)).Length;
            int personalityCount = Enum.GetValues(typeof (MinisterPersonalityHoI2)).Length;

            PersonalityTable = new MinisterPersonalityInfo[personalityCount];
            PersonalityStringMap.Clear();
            for (int i = 0; i < personalityCount; i++)
            {
                var info = new MinisterPersonalityInfo
                               {
                                   String = PersonalityStringTableHoI2[i],
                                   Name = PersonalityNameTableHoI2[i]
                               };
                PersonalityTable[i] = info;
                PersonalityStringMap.Add(info.String.ToLower(), i);
            }

            for (int i = 0; i < positionCount; i++)
            {
                if (PositionPersonalityTableHoI2[i] != null)
                {
                    PositionPersonalityTable[i] = PositionPersonalityTableHoI2[i].ToList();
                    foreach (int j in PositionPersonalityTable[i])
                    {
                        PersonalityTable[j].Position[i] = true;
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む(AoD)
        /// </summary>
        private static void LoadMinisterPersonalityAoD()
        {
            PersonalityStringMap.Clear();
            for (int i = 0; i < Enum.GetValues(typeof (MinisterPosition)).Length; i++)
            {
                PositionPersonalityTable[i] = new List<int>();
            }

            List<MinisterPersonalityInfo> list =
                MinisterModifierParser.Parse(Game.GetFileName(Game.MinisterPersonalityPathNameAoD));
            PersonalityTable = list.ToArray();

            for (int i = 0; i < PersonalityTable.Length; i++)
            {
                PersonalityStringMap.Add(PersonalityTable[i].String.ToLower(), i);
                PersonalityTable[i].String = GetTitleCasePersonality(PersonalityTable[i].String.ToLower());
                for (int j = 0; j < Enum.GetValues(typeof (MinisterPosition)).Length; j++)
                {
                    if (PersonalityTable[i].Position[j])
                    {
                        PositionPersonalityTable[j].Add(i);
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚特性を読み込む(DH)
        /// </summary>
        private static void LoadMinisterPersonalityDh()
        {
            PersonalityStringMap.Clear();
            for (int i = 0; i < Enum.GetValues(typeof (MinisterPosition)).Length; i++)
            {
                PositionPersonalityTable[i] = new List<int>();
            }

            List<MinisterPersonalityInfo> list =
                MinisterPersonalityParser.Parse(Game.GetFileName(Game.MinisterPersonalityPathNameDh));
            PersonalityTable = list.ToArray();

            for (int i = 0; i < PersonalityTable.Length; i++)
            {
                PersonalityStringMap.Add(PersonalityTable[i].String.ToLower(), i);
                PersonalityTable[i].String = GetTitleCasePersonality(PersonalityTable[i].String.ToLower());
                for (int j = 0; j < Enum.GetValues(typeof (MinisterPosition)).Length; j++)
                {
                    if (PersonalityTable[i].Position[j])
                    {
                        PositionPersonalityTable[j].Add(i);
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む
        /// </summary>
        public static void LoadMinisterFiles()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            List.Clear();

            switch (Game.Type)
            {
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    LoadMinisterFilesHoI2();
                    break;

                case GameType.DarkestHour:
                    if (Game.IsModActive)
                    {
                        LoadMinisterFilesDh();
                    }
                    else
                    {
                        LoadMinisterFilesHoI2();
                    }
                    break;
            }

            _loaded = true;
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む(HoI2/AoD/DH-MOD未使用時)
        /// </summary>
        private static void LoadMinisterFilesHoI2()
        {
            var fileList = new List<string>();
            string folderName;

            if (Game.IsModActive)
            {
                folderName = Path.Combine(Game.ModFolderName, Game.MinisterPathName);
                if (Directory.Exists(folderName))
                {
                    foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                    {
                        try
                        {
                            LoadMinisterFile(fileName);
                            string name = Path.GetFileName(fileName);
                            if (!String.IsNullOrEmpty(name))
                            {
                                fileList.Add(name.ToLower());
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                            Resources.Error);
                        }
                    }
                }
            }

            folderName = Path.Combine(Game.FolderName, Game.MinisterPathName);
            if (Directory.Exists(folderName))
            {
                foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
                {
                    string name = Path.GetFileName(fileName);
                    if (!String.IsNullOrEmpty(name) && !fileList.Contains(name.ToLower()))
                    {
                        try
                        {
                            LoadMinisterFile(fileName);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                                            Resources.Error);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     閣僚ファイル群を読み込む(DH-MOD使用時)
        /// </summary>
        private static void LoadMinisterFilesDh()
        {
            // ministers.txtが存在しなければ従来通りの読み込み方法を使用する
            string listFileName = Game.GetFileName(Game.DhMinisterListPathName);
            if (!File.Exists(listFileName))
            {
                LoadMinisterFilesHoI2();
                return;
            }

            IEnumerable<string> fileList;
            try
            {
                fileList = LoadMinisterListFileDh(listFileName);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, listFileName), Resources.Error);
                return;
            }

            foreach (
                string fileName in fileList.Select(name => Game.GetFileName(Path.Combine(Game.MinisterPathName, name))))
            {
                try
                {
                    LoadMinisterFile(fileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName), Resources.Error);
                }
            }
        }

        /// <summary>
        ///     閣僚リストファイルを読み込む(DH)
        /// </summary>
        private static IEnumerable<string> LoadMinisterListFileDh(string fileName)
        {
            var list = new List<string>();
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // 空行
                    if (String.IsNullOrEmpty(line))
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
                reader.Close();
            }
            return list;
        }

        /// <summary>
        ///     閣僚ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        private static void LoadMinisterFile(string fileName)
        {
            using (var reader = new StreamReader(fileName, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = Path.GetFileName(fileName);
                _currentLineNo = 1;

                // 空ファイルを読み飛ばす
                if (reader.EndOfStream)
                {
                    return;
                }

                // 国タグ読み込み
                string line = reader.ReadLine();
                if (String.IsNullOrEmpty(line))
                {
                    return;
                }
                string[] token = line.Split(CsvSeparator);
                if (token.Length == 0 || String.IsNullOrEmpty(token[0]))
                {
                    return;
                }
                CountryTag country = Country.CountryStringMap[token[0].ToUpper()];

                _currentLineNo++;

                while (!reader.EndOfStream)
                {
                    ParseMinisterLine(reader.ReadLine(), country);
                    _currentLineNo++;
                }
                reader.Close();

                ClearDirtyFlag(country);
            }
        }

        /// <summary>
        ///     閣僚定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="country">国家タグ</param>
        private static void ParseMinisterLine(string line, CountryTag country)
        {
            // 空行を読み飛ばす
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            string[] tokens = line.Split(CsvSeparator);

            // ID指定のない行は読み飛ばす
            if (String.IsNullOrEmpty(tokens[0]))
            {
                return;
            }

            // トークン数が足りない行は読み飛ばす
            if (tokens.Length != (Misc.Mod.RetirementYearMinister ? 11 : (Misc.Mod.NewMinisterFormat ? 10 : 9)))
            {
                Log.Write(string.Format("{0}: {1} L{2}\n", Resources.InvalidTokenCount, _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (tokens.Length < (Misc.Mod.RetirementYearMinister ? 10 : (Misc.Mod.NewMinisterFormat ? 9 : 8)))
                {
                    return;
                }
            }

            var minister = new Minister {CountryTag = country};
            int index = 0;

            // ID
            int id;
            if (!Int32.TryParse(tokens[index], out id))
            {
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidID, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1}\n", tokens[index], tokens[index + 2]));
                return;
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
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidPosition, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, tokens[index + 1], tokens[index]));
            }
            index++;

            // 名前
            minister.Name = tokens[index];
            index++;

            // 開始年
            int startYear;
            if (Int32.TryParse(tokens[index], out startYear))
            {
                minister.StartYear = startYear + (Misc.Mod.NewMinisterFormat ? 0 : 1900);
            }
            else
            {
                minister.StartYear = 1936;
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidStartYear, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
            }
            index++;

            // 終了年
            if (Misc.Mod.NewMinisterFormat)
            {
                int endYear;
                if (Int32.TryParse(tokens[index], out endYear))
                {
                    minister.EndYear = endYear;
                }
                else
                {
                    minister.EndYear = 1970;
                    Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidEndYear, _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
                }
                index++;
            }
            else
            {
                minister.EndYear = 1970;
            }

            // 引退年
            if (Misc.Mod.RetirementYearMinister)
            {
                int retirementYear;
                if (Int32.TryParse(tokens[index], out retirementYear))
                {
                    minister.RetirementYear = retirementYear;
                }
                else
                {
                    minister.RetirementYear = 1999;
                    Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidRetirementYear, _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
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
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidIdeology, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
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
                if (PersonalityTypoMap.ContainsKey(personalityName) &&
                    PersonalityStringMap.ContainsKey(PersonalityTypoMap[personalityName]))
                {
                    minister.Personality = PersonalityStringMap[PersonalityTypoMap[personalityName]];
                    Log.Write(String.Format("{0}: {1} L{2}\n", Resources.ModifiedPersonality, _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2} -> {3}\n", minister.Id, minister.Name, tokens[index],
                                            PersonalityTable[minister.Personality].String));
                }
                else
                {
                    minister.Personality = 0;
                    Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidPersonality, _currentFileName,
                                            _currentLineNo));
                    Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
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
                Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidLoyalty, _currentFileName, _currentLineNo));
                Log.Write(String.Format("  {0}: {1} => {2}\n", minister.Id, minister.Name, tokens[index]));
            }
            index++;

            // 画像ファイル名
            minister.PictureName = tokens[index];

            List.Add(minister);
        }

        /// <summary>
        ///     閣僚ファイル群を保存する
        /// </summary>
        public static void SaveMinisterFiles()
        {
            foreach (
                CountryTag country in
                    Enum.GetValues(typeof (CountryTag))
                        .Cast<CountryTag>()
                        .Where(country => DirtyFlags[(int) country] && country != CountryTag.None))
            {
                try
                {
                    SaveMinisterFile(country);
                }
                catch (Exception)
                {
                    string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                                     Game.MinisterPathName);
                    string fileName = Path.Combine(folderName, Game.GetMinisterFileName(country));
                    MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName), Resources.Error);
                }
            }
        }

        /// <summary>
        ///     閣僚ファイルを保存する
        /// </summary>
        /// <param name="country">国タグ</param>
        private static void SaveMinisterFile(CountryTag country)
        {
            string folderName = Path.Combine(Game.IsModActive ? Game.ModFolderName : Game.FolderName,
                                             Game.MinisterPathName);
            // 閣僚フォルダが存在しなければ作成する
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string fileName = Path.Combine(folderName, Game.GetMinisterFileName(country));

            using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(Game.CodePage)))
            {
                _currentFileName = fileName;
                _currentLineNo = 3;

                if (Misc.Mod.RetirementYearMinister)
                {
                    writer.WriteLine(
                        "{0};Ruling Cabinet - Start;Name;Start Year;End Year;Retirement Year;Ideology;Personality;Loyalty;Picturename;X",
                        Country.CountryTextTable[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;;;X");
                }
                else if (Misc.Mod.NewMinisterFormat)
                {
                    writer.WriteLine(
                        "{0};Ruling Cabinet - Start;Name;Start Year;End Year;Ideology;Personality;Loyalty;Picturename;X",
                        Country.CountryTextTable[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;;X");
                }
                else
                {
                    writer.WriteLine("{0};Ruling Cabinet - Start;Name;Pool;Ideology;Personality;Loyalty;Picturename;x",
                                     Country.CountryTextTable[(int) country]);
                    writer.WriteLine(";Replacements;;;;;;;x");
                }

                foreach (Minister minister in List.Where(minister => minister.CountryTag == country))
                {
                    // 不正な値が設定されている場合は警告をログに出力する
                    if (minister.Position == MinisterPosition.None)
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidPosition, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1}\n", minister.Id, minister.Name));
                    }
                    if (minister.Ideology == MinisterIdeology.None)
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidIdeology, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1}\n", minister.Id, minister.Name));
                    }
                    if (minister.Loyalty == MinisterLoyalty.None)
                    {
                        Log.Write(String.Format("{0}: {1} L{2}\n", Resources.InvalidLoyalty, _currentFileName,
                                                _currentLineNo));
                        Log.Write(String.Format("  {0}: {1}\n", minister.Id, minister.Name));
                    }
                    if (Misc.Mod.RetirementYearMinister)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};X",
                            minister.Id,
                            PositionTable[(int) minister.Position].String,
                            minister.Name,
                            minister.StartYear,
                            minister.EndYear,
                            minister.RetirementYear,
                            IdeologyTable[(int) minister.Ideology].String,
                            PersonalityTable[minister.Personality].String,
                            LoyaltyTable[(int) minister.Loyalty].String,
                            minister.PictureName);
                    }
                    else if (Misc.Mod.NewMinisterFormat)
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};X",
                            minister.Id,
                            PositionTable[(int) minister.Position].String,
                            minister.Name,
                            minister.StartYear,
                            minister.EndYear,
                            IdeologyTable[(int) minister.Ideology].String,
                            PersonalityTable[minister.Personality].String,
                            LoyaltyTable[(int) minister.Loyalty].String,
                            minister.PictureName);
                    }
                    else
                    {
                        writer.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};x",
                            minister.Id,
                            PositionTable[(int) minister.Position].String,
                            minister.Name,
                            minister.StartYear - 1900,
                            IdeologyTable[(int) minister.Ideology].String,
                            PersonalityTable[minister.Personality].String,
                            LoyaltyTable[(int) minister.Loyalty].String,
                            minister.PictureName);
                    }
                    _currentLineNo++;
                }
                writer.Close();
            }

            ClearDirtyFlag(country);
        }

        /// <summary>
        ///     閣僚リストに項目を追加する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        public static void AddItem(Minister target)
        {
            List.Add(target);
        }

        /// <summary>
        ///     閣僚リストに項目を挿入する
        /// </summary>
        /// <param name="target">挿入対象の項目</param>
        /// <param name="position">挿入位置の直前の項目</param>
        public static void InsertItemNext(Minister target, Minister position)
        {
            List.Insert(List.IndexOf(position) + 1, target);
        }

        /// <summary>
        ///     閣僚リストから項目を削除する
        /// </summary>
        /// <param name="target"></param>
        public static void RemoveItem(Minister target)
        {
            List.Remove(target);
        }

        /// <summary>
        ///     閣僚リストの項目を移動する
        /// </summary>
        /// <param name="target">移動対象の項目</param>
        /// <param name="position">移動先位置の項目</param>
        public static void MoveItem(Minister target, Minister position)
        {
            int targetIndex = List.IndexOf(target);
            int positionIndex = List.IndexOf(position);

            if (targetIndex > positionIndex)
            {
                // 上へ移動する場合
                List.Insert(positionIndex, target);
                List.RemoveAt(targetIndex + 1);
            }
            else
            {
                // 下へ移動する場合
                List.Insert(positionIndex + 1, target);
                List.RemoveAt(targetIndex);
            }
        }

        /// <summary>
        ///     閣僚ファイルの再読み込みを要求する
        /// </summary>
        public static void RequireReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     編集フラグをセットする
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void SetDirtyFlag(CountryTag country)
        {
            DirtyFlags[(int) country] = true;
        }

        /// <summary>
        ///     編集フラグをクリアする
        /// </summary>
        /// <param name="country">国タグ</param>
        public static void ClearDirtyFlag(CountryTag country)
        {
            DirtyFlags[(int) country] = false;
        }

        /// <summary>
        ///     閣僚特性文字列を単語の先頭だけを大文字に変換する
        /// </summary>
        /// <param name="s">閣僚特性文字列</param>
        /// <returns>変換後の文字列</returns>
        private static string GetTitleCasePersonality(string s)
        {
            // 特別な変換規則のある場合
            if (PersonalityTitleCaseMap.ContainsKey(s))
            {
                return PersonalityTitleCaseMap[s];
            }

            // 単語の文字列だけを大文字に変換する
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(s);
        }

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
            LaissezFairesCapitalist, // 自由放任主義者
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
            VerticalEnvelopmentDoctrine, // 立体集中攻撃ドクトリン
        }
    }

    /// <summary>
    ///     閣僚地位データ
    /// </summary>
    public class MinisterPositionInfo
    {
        /// <summary>
        ///     閣僚地位名
        /// </summary>
        public string Name;

        /// <summary>
        ///     閣僚地位文字列
        /// </summary>
        public string String;
    }

    /// <summary>
    ///     閣僚特性データ
    /// </summary>
    public class MinisterPersonalityInfo
    {
        /// <summary>
        ///     閣僚特性名
        /// </summary>
        public string Name;

        /// <summary>
        ///     閣僚地位と閣僚特性の対応付け
        /// </summary>
        public bool[] Position = new bool[Enum.GetValues(typeof (MinisterPosition)).Length];

        /// <summary>
        ///     閣僚特性文字列
        /// </summary>
        public string String;
    }

    /// <summary>
    ///     閣僚忠誠度データ
    /// </summary>
    public class MinisterLoyaltyInfo
    {
        /// <summary>
        ///     閣僚忠誠度名
        /// </summary>
        public string Name;

        /// <summary>
        ///     閣僚忠誠度文字列
        /// </summary>
        public string String;
    }

    /// <summary>
    ///     閣僚イデオロギーデータ
    /// </summary>
    public class MinisterIdeologyInfo
    {
        /// <summary>
        ///     閣僚イデオロギー名
        /// </summary>
        public string Name;

        /// <summary>
        ///     閣僚イデオロギー文字列
        /// </summary>
        public string String;
    }
}