using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                "",
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
        /// 閣僚特性文字列
        /// </summary>
        public static readonly string[] PersonalityTextTable =
            {
                "",
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
                "Very High",
                "High",
                "Medium",
                "Low",
                "Very Low",
                "Undying",
                "NA"
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
                "最低",
                "不死",
                "NA"
            };

        /// <summary>
        /// 閣僚地位名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPosition> PositionNameMap =
            new Dictionary<string, MinisterPosition>();

        /// <summary>
        /// 閣僚特性名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterPersonality> PersonalityNameMap =
            new Dictionary<string, MinisterPersonality>();

        /// <summary>
        /// イデオロギー名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterIdeology> IdeologyNameMap =
            new Dictionary<string, MinisterIdeology>();

        /// <summary>
        /// 忠誠度名とIDの対応付け
        /// </summary>
        public static readonly Dictionary<string, MinisterLoyalty> LoyaltyNameMap =
            new Dictionary<string, MinisterLoyalty>();

        /// <summary>
        /// 国家元首の特性
        /// </summary>
        private static readonly MinisterPersonality[] HeadOfStatePersonalities =
            {
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
                MinisterPersonality.None,
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
        /// 現在解析中のファイル名
        /// </summary>
        private static string _currentFileName = "";

        /// <summary>
        /// 現在解析中の行番号
        /// </summary>
        private static int _currentLineNo;

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
                PositionNameMap.Add(PositionNameTable[(int) position].ToLower(), position);
            }

            foreach (MinisterPersonality personality in Enum.GetValues(typeof (MinisterPersonality)))
            {
                PersonalityNameMap.Add(PersonalityNameTable[(int) personality].ToLower(), personality);
            }

            foreach (MinisterLoyalty loyalty in Enum.GetValues(typeof (MinisterLoyalty)))
            {
                if (loyalty == MinisterLoyalty.None)
                {
                    continue;
                }
                LoyaltyNameMap.Add(LoyaltyNameTable[(int) loyalty].ToLower(), loyalty);
            }

            foreach (MinisterIdeology ideology in Enum.GetValues(typeof (MinisterIdeology)))
            {
                if (ideology == MinisterIdeology.None)
                {
                    continue;
                }
                IdeologyNameMap.Add(IdeologyNameTable[(int) ideology].ToLower(), ideology);
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
        /// <returns>閣僚リスト</returns>
        public static List<Minister> LoadMinisterFiles()
        {
            var ministers = new List<Minister>();

            foreach (string fileName in Directory.GetFiles(Game.MinisterFolderName, "*.csv"))
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
            _currentFileName = Path.GetFileName(fileName);
            _currentLineNo = 1;

            var reader = new StreamReader(fileName, Encoding.Default);
            // 空ファイルを読み飛ばす
            if (reader.EndOfStream)
            {
                return;
            }

            // 国タグ読み込み
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
            CountryTag countryTag = Country.CountryTextMap[token[0].ToUpper()];

            _currentLineNo++;

            while (!reader.EndOfStream)
            {
                ParseMinisterLine(reader.ReadLine(), ministers, countryTag);
                _currentLineNo++;
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
            // 空行を読み飛ばす
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string[] token = line.Split(CsvSeparator);

            // ID指定のない行は読み飛ばす
            if (string.IsNullOrEmpty(token[0]))
            {
                return;
            }

            // トークン数が足りない行は読み飛ばす
            if (token.Length != 9)
            {
                Log.Write(string.Format("項目数の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n\n", line));
                // 末尾のxがない/余分な項目がある場合は解析を続ける
                if (token.Length < 8)
                {
                    return;
                }
            }

            var minister = new Minister();
            int id;
            if (!int.TryParse(token[0], out id))
            {
                Log.Write(string.Format("IDの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}\n\n", token[0]));
                return;
            }
            minister.Id = id;
            minister.Name = token[2];
            int startYear;
            if (!int.TryParse(token[3], out startYear))
            {
                Log.Write(string.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[3]));
                return;
            }
            minister.StartYear = startYear + 1900;
            minister.EndYear = 1970;
            string positionName = token[1].ToLower();
            if (PositionNameMap.ContainsKey(positionName))
            {
                minister.Position = PositionNameMap[positionName];
            }
            else
            {
                Log.Write(string.Format("閣僚地位の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[1]));
                minister.Position = MinisterPosition.None;
            }
            string ideologyName = token[4].ToLower();
            if (IdeologyNameMap.ContainsKey(ideologyName))
            {
                minister.Ideology = IdeologyNameMap[ideologyName];
            }
            else
            {
                Log.Write(string.Format("イデオロギーの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[4]));
                minister.Ideology = MinisterIdeology.None;
            }
            string personalityName = token[5].ToLower();
            if (PersonalityNameMap.ContainsKey(personalityName))
            {
                minister.Personality = PersonalityNameMap[personalityName];
            }
            else
            {
                Log.Write(string.Format("閣僚特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[5]));
                minister.Personality = MinisterPersonality.None;
            }
            string loyaltyName = token[6].ToLower();
            if (LoyaltyNameMap.ContainsKey(loyaltyName))
            {
                minister.Loyalty = LoyaltyNameMap[loyaltyName];
            }
            else
            {
                Log.Write(string.Format("忠誠度の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, token[6]));
                minister.Loyalty = MinisterLoyalty.None;
            }
            minister.PictureName = token[7];
            minister.CountryTag = countryTag;
            ministers.Add(minister);
        }

        /// <summary>
        /// 閣僚ファイル群を保存する
        /// </summary>
        /// <param name="ministers">閣僚リスト</param>
        /// <param name="dirtyFlags">編集フラグ </param>
        public static void SaveMinisterFiles(List<Minister> ministers, bool[] dirtyFlags)
        {
            foreach (
                CountryTag countryTag in
                    Enum.GetValues(typeof (CountryTag)).Cast<CountryTag>().Where(
                        countryTag => dirtyFlags[(int) countryTag]).Where(countryTag => countryTag != CountryTag.None))
            {
                SaveMinisterFile(ministers, countryTag);
            }
        }

        /// <summary>
        /// 閣僚ファイルを保存する
        /// </summary>
        /// <param name="ministers">閣僚リスト</param>
        /// <param name="countryTag">国タグ</param>
        private static void SaveMinisterFile(IEnumerable<Minister> ministers, CountryTag countryTag)
        {
            if (countryTag == CountryTag.None)
            {
                return;
            }

            _currentFileName = Path.GetFileName(Game.GetMinisterFileName(countryTag));
            _currentLineNo = 3;

            var writer = new StreamWriter(Game.GetMinisterFileName(countryTag), false, Encoding.Default);
            writer.WriteLine("{0};Ruling Cabinet - Start;Name;Pool;Ideology;Personality;Loyalty;Picturename;x",
                             Country.CountryTextTable[(int) countryTag]);
            writer.WriteLine(";Replacements;;;;;;;x");

            foreach (
                Minister minister in
                    ministers.Where(minister => minister.CountryTag == countryTag).Where(minister => minister != null))
            {
                // 不正な値が設定されている場合は警告をログに出力する
                if (string.IsNullOrEmpty(minister.Name))
                {
                    Log.Write(string.Format("閣僚名の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.StartYear < 1900 || minister.StartYear > 1999)
                {
                    Log.Write(string.Format("開始年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, minister.StartYear));
                }
                if (minister.EndYear < 1900 || minister.EndYear > 1999)
                {
                    Log.Write(string.Format("終了年の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1} => {2}\n\n", minister.Id, minister.Name, minister.EndYear));
                }
                if (minister.Position == MinisterPosition.None)
                {
                    Log.Write(string.Format("閣僚地位の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Personality == MinisterPersonality.None)
                {
                    Log.Write(string.Format("閣僚特性の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Ideology == MinisterIdeology.None)
                {
                    Log.Write(string.Format("イデオロギーの異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }
                if (minister.Loyalty == MinisterLoyalty.None)
                {
                    Log.Write(string.Format("忠誠度の異常: {0} L{1} \n", _currentFileName, _currentLineNo));
                    Log.Write(string.Format("  {0}: {1}\n\n", minister.Id, minister.Name));
                }

                writer.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};x", minister.Id,
                                 PositionNameTable[(int) minister.Position], minister.Name, minister.StartYear - 1900,
                                 IdeologyNameTable[(int) minister.Ideology],
                                 PersonalityNameTable[(int) minister.Personality],
                                 LoyaltyNameTable[(int) minister.Loyalty], minister.PictureName);
                _currentLineNo++;
            }

            writer.Close();
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
        None,

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
        Undying,
        Na,
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