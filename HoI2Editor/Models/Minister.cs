using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoI2Editor.Models
{
    /// <summary>
    /// 閣僚データ
    /// </summary>
    internal class Minister
    {
        /// <summary>
        /// CSVファイルの区切り文字
        /// </summary>
        private static readonly char[] CsvSeparator = {';'};

        /// <summary>
        /// 閣僚地位名
        /// </summary>
        public static readonly string[] PositionNameTable =
            {
                "",
                "head of state",
                "head of government",
                "foreign minister",
                "minister of armament",
                "minister of security",
                "head of military intelligence",
                "chief of staff",
                "chief of army",
                "chief of navy",
                "chief of air force"
            };

        /// <summary>
        /// 閣僚地位文字列
        /// </summary>
        public static readonly string[] PositionTextTable =
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
        /// 閣僚特性名
        /// </summary>
        public static readonly string[] PersonalityNameTable =
            {
                "undistinguished suit",
                "autocratic charmer",
                "barking buffoon",
                "benevolent gentleman",
                "die-hard reformer",
                "insignificant layman",
                "pig-headed isolationist",
                "popular figurehead",
                "powerhungry demagogue",
                "resigned generalissimo",
                "ruthless powermonger",
                "stern imperialist",
                "weary stiffneck",
                "ambitious union boss",
                "backroom backstabber",
                "corporate suit",
                "flamboyant tough guy",
                "happy amateur",
                "naive optimist",
                "old admiral",
                "old airmarshal",
                "old general",
                "political protege",
                "silent workhorse",
                "smiling oilman",
                "apologetic clerk",
                "biased intellectual",
                "ideological crusader",
                "iron fisted brute",
                "general staffer",
                "great compromiser",
                "the cloak n dagger schemer",
                "administrative genius",
                "air superiority proponent",
                "battle fleet proponent",
                "resource industrialist",
                "laissez faires capitalist",
                "theoretical scientist",
                "military entrepreneur",
                "submarine proponent",
                "tank proponent",
                "infantry proponent",
                "corrupt kleptocrat",
                "air to ground proponent",
                "air to sea proponent",
                "strategic air proponent",
                "back stabber",
                "compassionate gentleman",
                "crime fighter",
                "crooked kleptocrat",
                "efficient sociopath",
                "man of the people",
                "prince of terror",
                "silent lawyer",
                "dismal enigma",
                "industrial specialist",
                "logistics specialist",
                "naval intelligence specialist",
                "political specialist",
                "technical specialist",
                "school of defence",
                "school of fire support",
                "school of mass combat",
                "school of manoeuvre",
                "school of psychology",
                "armoured spearhead doctrine",
                "decisive battle doctrine",
                "elastic defence doctrine",
                "guns and butter doctrine",
                "static defence doctrine",
                "base control doctrine",
                "decisive naval battle doctrine",
                "indirect approach doctrine",
                "open seas doctrine",
                "power projection doctrine",
                "air superiority doctrine",
                "army aviation doctrine",
                "carpet bombing doctrine",
                "naval aviation doctrine",
                "vertical envelopment doctrine"
            };

        /// <summary>
        /// 閣僚特性文字列
        /// </summary>
        public static readonly string[] PersonalityTextTable =
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
        /// イデオロギー名
        /// </summary>
        public static readonly string[] IdeologyNameTable =
            {
                "",
                "ns",
                "fa",
                "pa",
                "sc",
                "ml",
                "sl",
                "sd",
                "lwr",
                "le",
                "st"
            };

        /// <summary>
        /// イデオロギー文字列
        /// </summary>
        public static readonly string[] IdeologyTextTable =
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
        /// 忠誠度名
        /// </summary>
        public static readonly string[] LoyaltyNameTable =
            {
                "",
                "very high",
                "high",
                "medium",
                "low",
                "very low"
            };

        /// <summary>
        /// 忠誠度文字列
        /// </summary>
        public static readonly string[] LoyaltyTextTable =
            {
                "",
                "最高",
                "高",
                "中",
                "低",
                "最低"
            };

        /// <summary>
        /// 閣僚地位名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPosition> PositionNameMap =
            new Dictionary<string, MinisterPosition>();

        /// <summary>
        /// 閣僚地位文字列とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPosition> PositionTextMap =
            new Dictionary<string, MinisterPosition>();

        /// <summary>
        /// 閣僚特性名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPersonality> PersonalityNameMap =
            new Dictionary<string, MinisterPersonality>();

        /// <summary>
        /// 閣僚特性文字列とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPersonality> PersonalityTextMap =
            new Dictionary<string, MinisterPersonality>();

        /// <summary>
        /// イデオロギー名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterIdeology> IdeologyNameMap =
            new Dictionary<string, MinisterIdeology>();

        /// <summary>
        /// イデオロギー文字列とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterIdeology> IdeologyTextMap =
            new Dictionary<string, MinisterIdeology>();

        /// <summary>
        /// 忠誠度名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterLoyalty> LoyaltyNameMap =
            new Dictionary<string, MinisterLoyalty>();

        /// <summary>
        /// 忠誠度文字列とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterLoyalty> LoyaltyTextMap =
            new Dictionary<string, MinisterLoyalty>();

        /// <summary>
        /// 国家元首の特性
        /// </summary>
        private static readonly MinisterPersonality[] HeadOfStatePersonalities =
            {
                MinisterPersonality.AutocraticCharmer,
                MinisterPersonality.BarkingBuffoon,
                MinisterPersonality.BenevolentGentleman,
                MinisterPersonality.DieHardReformer,
                MinisterPersonality.InsignificantLayman,
                MinisterPersonality.PigHeadedIsolationist,
                MinisterPersonality.PopularFigurehead,
                MinisterPersonality.PowerHungryDemagogue,
                MinisterPersonality.ResignedGeneralissimo,
                MinisterPersonality.RuthlessPowermonger,
                MinisterPersonality.SternImperalist,
                MinisterPersonality.WearyStiffNeck,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 政府首班の特性
        /// </summary>
        private static readonly MinisterPersonality[] HeadOfGovernmentPersonalities =
            {
                MinisterPersonality.AmbitiousUnionBoss,
                MinisterPersonality.BackroomBackstabber,
                MinisterPersonality.CorporateSuit,
                MinisterPersonality.FlamboyantToughGuy,
                MinisterPersonality.HappyAmateur,
                MinisterPersonality.NaiveOptimist,
                MinisterPersonality.OldAdmiral,
                MinisterPersonality.OldAirMarshal,
                MinisterPersonality.OldGeneral,
                MinisterPersonality.PoliticalProtege,
                MinisterPersonality.SilentWorkhorse,
                MinisterPersonality.SmilingOilman,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 外務大臣の特性
        /// </summary>
        private static readonly MinisterPersonality[] ForeignMinisterPersonalities =
            {
                MinisterPersonality.ApologeticClerk,
                MinisterPersonality.BiasedIntellectual,
                MinisterPersonality.IdeologyCrusader,
                MinisterPersonality.IronFistedBrute,
                MinisterPersonality.GeneralStaffer,
                MinisterPersonality.GreatCompromiser,
                MinisterPersonality.TheCloakNDaggerSchemer,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 軍需大臣の特性
        /// </summary>
        private static readonly MinisterPersonality[] MinisterOfArmamentPersonalities =
            {
                MinisterPersonality.AdministrativeGenius,
                MinisterPersonality.AirSuperiorityProponent,
                MinisterPersonality.BattleFleetProponent,
                MinisterPersonality.ResourceIndustrialist,
                MinisterPersonality.LaissezFairesCapitalist,
                MinisterPersonality.TheoreticalScientist,
                MinisterPersonality.MilitaryEnterpreneur,
                MinisterPersonality.SubmarineProponent,
                MinisterPersonality.TankProponent,
                MinisterPersonality.InfantryProponent,
                MinisterPersonality.CorruptKleptocrat,
                MinisterPersonality.AirToGroundProponent,
                MinisterPersonality.AirToSeaProponent,
                MinisterPersonality.StrategicAirProponent,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 内務大臣の特性
        /// </summary>
        private static readonly MinisterPersonality[] MinisterOfSecurityPersonalities =
            {
                MinisterPersonality.BackStabber,
                MinisterPersonality.CompassionateGentleman,
                MinisterPersonality.CrimeFighter,
                MinisterPersonality.CrookedKleptocrat,
                MinisterPersonality.EfficientSociopath,
                MinisterPersonality.ManOfThePeople,
                MinisterPersonality.PrinceOfTerror,
                MinisterPersonality.SilentLawyer,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 情報大臣の特性
        /// </summary>
        private static readonly MinisterPersonality[] HeadOfMilitaryIntelligencePersonalities =
            {
                MinisterPersonality.DismalEnigma,
                MinisterPersonality.IndustrialSpecialist,
                MinisterPersonality.LogisticsSpecialist,
                MinisterPersonality.NavalIntelligenceSpecialist,
                MinisterPersonality.PoliticalSpecialist,
                MinisterPersonality.TechnicalSpecialist,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 統合参謀総長の特性
        /// </summary>
        private static readonly MinisterPersonality[] ChiefOfStaffPersonalities =
            {
                MinisterPersonality.SchoolOfDefence,
                MinisterPersonality.SchoolOfFireSupport,
                MinisterPersonality.SchoolOfMassCombat,
                MinisterPersonality.SchoolOfManeuvre,
                MinisterPersonality.SchoolOfPsychology,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 陸軍総司令官の特性
        /// </summary>
        private static readonly MinisterPersonality[] ChiefOfArmyPersonalities =
            {
                MinisterPersonality.ArmouredSpearheadDoctrine,
                MinisterPersonality.DecisiveBattleDoctrine,
                MinisterPersonality.ElasticDefenceDoctrine,
                MinisterPersonality.GunsAndButterDoctrine,
                MinisterPersonality.StaticDefenceDoctrine,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 海軍総司令官の特性
        /// </summary>
        private static readonly MinisterPersonality[] ChiefOfNavyPersonalities =
            {
                MinisterPersonality.BaseControlDoctrine,
                MinisterPersonality.DecisiveNavalBattleDoctrine,
                MinisterPersonality.IndirectApproachDoctrine,
                MinisterPersonality.OpenSeasDoctrine,
                MinisterPersonality.PowerProjectionDoctrine,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 空軍総司令官の特性
        /// </summary>
        private static readonly MinisterPersonality[] ChiefOfAirForcePersonalities =
            {
                MinisterPersonality.AirSuperiorityDoctrine,
                MinisterPersonality.ArmyAviationDoctrine,
                MinisterPersonality.CarpetBombingDoctrine,
                MinisterPersonality.NavalAviationDoctrine,
                MinisterPersonality.VerticalEnvelopmentDoctrine,
                MinisterPersonality.UndistinguishedSuit
            };

        /// <summary>
        /// 閣僚地位と特性の対応付け
        /// </summary>
        public static MinisterPersonality[][] PositionPersonalityTable =
            {
                null,
                HeadOfStatePersonalities,
                HeadOfGovernmentPersonalities,
                ForeignMinisterPersonalities,
                MinisterOfArmamentPersonalities,
                MinisterOfSecurityPersonalities,
                HeadOfMilitaryIntelligencePersonalities,
                ChiefOfStaffPersonalities,
                ChiefOfArmyPersonalities,
                ChiefOfNavyPersonalities,
                ChiefOfAirForcePersonalities
            };

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static Minister()
        {
            foreach (MinisterPosition position in Enum.GetValues(typeof (MinisterPosition)))
            {
                if (position == MinisterPosition.None)
                {
                    continue;
                }
                PositionNameMap.Add(PositionNameTable[(int) position], position);
                PositionTextMap.Add(Config.Text[PositionTextTable[(int) position]], position);
            }

            foreach (MinisterPersonality personality in Enum.GetValues(typeof (MinisterPersonality)))
            {
                PersonalityNameMap.Add(PersonalityNameTable[(int) personality], personality);
                PersonalityTextMap.Add(Config.Text[PersonalityTextTable[(int) personality]], personality);
            }

            foreach (MinisterLoyalty loyalty in Enum.GetValues(typeof (MinisterLoyalty)))
            {
                if (loyalty == MinisterLoyalty.None)
                {
                    continue;
                }
                LoyaltyNameMap.Add(LoyaltyNameTable[(int) loyalty], loyalty);
                LoyaltyTextMap.Add(LoyaltyTextTable[(int) loyalty], loyalty);
            }

            foreach (MinisterIdeology ideology in Enum.GetValues(typeof (MinisterIdeology)))
            {
                if (ideology == MinisterIdeology.None)
                {
                    continue;
                }
                IdeologyNameMap.Add(IdeologyNameTable[(int) ideology], ideology);
                IdeologyTextMap.Add(Config.Text[IdeologyTextTable[(int) ideology]], ideology);
            }
        }

        /// <summary>
        /// 国タグ
        /// </summary>
        public CountryTag CountryTag { get; set; }

        /// <summary>
        /// 閣僚ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        /// 閣僚ポスト
        /// </summary>
        public MinisterPosition Position { get; set; }

        /// <summary>
        /// 閣僚特性
        /// </summary>
        public MinisterPersonality Personality { get; set; }

        /// <summary>
        /// 閣僚忠誠度
        /// </summary>
        public MinisterLoyalty Loyalty { get; set; }

        /// <summary>
        /// イデオロギー
        /// </summary>
        public MinisterIdeology Ideology { get; set; }

        /// <summary>
        /// 開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        /// 終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        /// 閣僚ファイル群を読み込む
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        /// <returns>閣僚リスト</returns>
        public static List<Minister> LoadMinisterFiles(string folderName)
        {
            var ministers = new List<Minister>();

            foreach (string fileName in Directory.GetFiles(folderName, "*.csv"))
            {
                LoadMinisterFile(fileName, ministers);
            }

            return ministers;
        }

        /// <summary>
        /// 閣僚ファイルを読み込む
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        /// <param name="ministers">閣僚リスト</param>
        private static void LoadMinisterFile(string fileName, List<Minister> ministers)
        {
            var reader = new StreamReader(fileName, Encoding.Default);
            if (reader.EndOfStream)
            {
                return;
            }

            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return;
            }
            string[] token = line.Split(CsvSeparator);
            if (token.Length == 0 || string.IsNullOrEmpty(token[0]))
            {
                return;
            }
            CountryTag countryTag = Country.CountryTextMap[token[0]];

            while (!reader.EndOfStream)
            {
                ParseMinisterLine(reader.ReadLine(), ministers, countryTag);
            }
        }

        /// <summary>
        /// 閣僚定義行を解釈する
        /// </summary>
        /// <param name="line">対象文字列</param>
        /// <param name="ministers">閣僚リスト</param>
        /// <param name="countryTag">国家タグ</param>
        private static void ParseMinisterLine(string line, List<Minister> ministers, CountryTag countryTag)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string[] token = line.Split(CsvSeparator);
            if (token.Length != 9)
            {
                return;
            }

            var minister = new Minister();
            try
            {
                minister.CountryTag = countryTag;
                minister.Id = int.Parse(token[0]);
                MinisterPosition position = PositionNameMap[token[1].ToLower()];
                minister.Position = position;
                minister.Name = token[2];
                minister.StartYear = int.Parse(token[3]) + 1900;
                minister.EndYear = 1970;
                MinisterIdeology ideology = IdeologyNameMap[token[4].ToLower()];
                minister.Ideology = ideology;
                MinisterPersonality personality = PersonalityNameMap[token[5].ToLower()];
                minister.Personality = personality;
                MinisterLoyalty loyalty = LoyaltyNameMap[token[6].ToLower()];
                minister.Loyalty = loyalty;
                minister.PictureName = token[7];
            }
            catch (Exception)
            {
                return;
            }
            ministers.Add(minister);
        }
    }

    /// <summary>
    /// 閣僚ポスト
    /// </summary>
    public enum MinisterPosition
    {
        None,
        HeadOfState, // 国家元首
        HeadOfGovernment, // 政府首班
        ForeignMinister, // 外務大臣
        MinisterOfArmament, // 軍需大臣
        MinisterOfSecurity, // 内務大臣
        HeadOfMilitaryIntelligence, // 情報大臣
        ChiefOfStaff, // 統合参謀総長
        ChiefOfArmy, // 陸軍総司令官
        ChiefOfNavy, // 海軍総司令官
        ChiefOfAirForce, // 空軍総司令官
    }

    /// <summary>
    /// 閣僚特性
    /// </summary>
    public enum MinisterPersonality
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

    /// <summary>
    /// 閣僚忠誠度
    /// </summary>
    public enum MinisterLoyalty
    {
        None,
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
    }

    /// <summary>
    /// イデオロギー
    /// </summary>
    public enum MinisterIdeology
    {
        None,
        NationalSocialist, // NS 国家社会主義
        Fascist, // FA ファシスト
        PaternalAutocrat, // PA 権威主義者
        SocialConservative, // SC 社会保守派
        MarketLiberal, // ML 自由経済派
        SocialLiberal, // SL 社会自由派
        SocialDemocrat, // SD 社会民主派
        LeftWingRadical, // LWR 急進的左翼
        Leninist, // LE レーニン主義者
        Stalinist, // ST スターリン主義者
    }
}