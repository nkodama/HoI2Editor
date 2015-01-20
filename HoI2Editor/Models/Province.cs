using System;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     プロヴィンスデータ
    /// </summary>
    public class Province
    {
        #region 公開プロパティ

        /// <summary>
        ///     プロヴィンスID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     プロヴィンス名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     地域ID
        /// </summary>
        public AreaId Area { get; set; }

        /// <summary>
        ///     地方ID
        /// </summary>
        public RegionId Region { get; set; }

        /// <summary>
        ///     大陸ID
        /// </summary>
        public ContinentId Continent { get; set; }

        /// <summary>
        ///     気候ID
        /// </summary>
        public ClimateId Climate { get; set; }

        /// <summary>
        ///     地形ID
        /// </summary>
        public TerrainId Terrain { get; set; }

        /// <summary>
        ///     サイズ補正(不明)
        /// </summary>
        public int SizeModifier { get; set; }

        /// <summary>
        ///     航空機許容量(不明)
        /// </summary>
        public int AirCapacity { get; set; }

        /// <summary>
        ///     インフラ
        /// </summary>
        public double Infrastructure { get; set; }

        /// <summary>
        ///     都市(不明)
        /// </summary>
        public int City { get; set; }

        /// <summary>
        ///     砂浜の有無
        /// </summary>
        public bool Beaches { get; set; }

        /// <summary>
        ///     港の有無
        /// </summary>
        public bool PortAllowed { get; set; }

        /// <summary>
        ///     港の海域
        /// </summary>
        public int PortSeaZone { get; set; }

        /// <summary>
        ///     IC
        /// </summary>
        public double Ic { get; set; }

        /// <summary>
        ///     労働力
        /// </summary>
        public double Manpower { get; set; }

        /// <summary>
        ///     石油
        /// </summary>
        public double Oil { get; set; }

        /// <summary>
        ///     金属
        /// </summary>
        public double Metal { get; set; }

        /// <summary>
        ///     エネルギー
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        ///     希少資源
        /// </summary>
        public double RareMaterials { get; set; }

        /// <summary>
        ///     都市のX座標
        /// </summary>
        public int CityXPos { get; set; }

        /// <summary>
        ///     都市のY座標
        /// </summary>
        public int CityYPos { get; set; }

        /// <summary>
        ///     軍隊のX座標
        /// </summary>
        public int ArmyXPos { get; set; }

        /// <summary>
        ///     軍隊のY座標
        /// </summary>
        public int ArmyYPos { get; set; }

        /// <summary>
        ///     港のX座標
        /// </summary>
        public int PortXPos { get; set; }

        /// <summary>
        ///     港のY座標
        /// </summary>
        public int PortYPos { get; set; }

        /// <summary>
        ///     砂浜のX座標
        /// </summary>
        public int BeachXPos { get; set; }

        /// <summary>
        ///     砂浜のY座標
        /// </summary>
        public int BeachYPos { get; set; }

        /// <summary>
        ///     砂浜のアイコン
        /// </summary>
        public int BeachIcon { get; set; }

        /// <summary>
        ///     要塞のX座標
        /// </summary>
        public int FortXPos { get; set; }

        /// <summary>
        ///     要塞のY座標
        /// </summary>
        public int FortYPos { get; set; }

        /// <summary>
        ///     対空砲のX座標
        /// </summary>
        public int AaXPos { get; set; }

        /// <summary>
        ///     対空砲のY座標
        /// </summary>
        public int AaYPos { get; set; }

        /// <summary>
        ///     カウンターのX座標
        /// </summary>
        public int CounterXPos { get; set; }

        /// <summary>
        ///     カウンターのY座標
        /// </summary>
        public int CounterYPos { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainVariant1 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainXPos1 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainYPos1 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainVariant2 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainXPos2 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainYPos2 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainVariant3 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainXPos3 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainYPos3 { get; set; }

        /// <summary>
        ///     不明
        /// </summary>
        public int TerrainVariant4 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標1
        /// </summary>
        public int FillCoordX1 { get; set; }

        /// <summary>
        ///     塗りつぶしY座標1
        /// </summary>
        public int FillCoordY1 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標2
        /// </summary>
        public int FillCoordX2 { get; set; }

        /// <summary>
        ///     塗りつぶしY座標2
        /// </summary>
        public int FillCoordY2 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標3
        /// </summary>
        public int FillCoordX3 { get; set; }

        /// <summary>
        ///     塗りつぶしY座標3
        /// </summary>
        public int FillCoordY3 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標4
        /// </summary>
        public int FillCoordX4 { get; set; }

        /// <summary>
        ///     塗りつぶしY座標4
        /// </summary>
        public int FillCoordY4 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標5
        /// </summary>
        public int FillCoordX5 { get; set; }

        /// <summary>
        ///     塗りつぶしY座標5
        /// </summary>
        public int FillCoordY5 { get; set; }

        /// <summary>
        ///     塗りつぶしX座標6
        /// </summary>
        public int FillCoordX6 { get; set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (ProvinceItemId)).Length];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 文字列操作

        /// <summary>
        ///     プロヴィンス名を取得する
        /// </summary>
        /// <returns>プロヴィンス名</returns>
        public string GetName()
        {
            return Config.GetText(Name);
        }

        /// <summary>
        ///     プロヴィンス名を設定する
        /// </summary>
        /// <param name="s">プロヴィンス名</param>
        public void SetName(string s)
        {
            Config.SetText(Name, s, Game.ProvinceTextFileName);
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     プロヴィンスデータが編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     項目が編集済みかどうかを取得する
        /// </summary>
        /// <param name="id">項目ID</param>
        /// <returns>編集済みならばtrueを返す</returns>
        public bool IsDirty(ProvinceItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(ProvinceItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TeamItemId id in Enum.GetValues(typeof (ProvinceItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     地域ID
    /// </summary>
    public enum AreaId
    {
        None,

        // 陸地用
        Adelaide,
        Afghanistan,
        Agats,
        Alabama,
        Alaska,
        AlaskanArchipelago,
        Albania,
        Alberta,
        AliceSprings,
        AlpesProvence,
        Amur,
        AnatolianNorthCoast,
        AnatolianSouthCoast,
        AngolanCoast,
        AngolanPlains,
        Anhui,
        Antilles,
        AppennineRidge,
        Aquitaine,
        ArabianDesert,
        Arizona,
        Arkansas,
        Arkhangelsk,
        ArmeniaAzerbaijan,
        Astrakhan,
        Asuncion,
        Attica,
        Austria,
        Babo,
        BadenWurttemberg,
        BahamasIslands,
        Baleares,
        Belgorod, // DHのみ
        Bavaria,
        Bechuanaland,
        Bengal,
        Bengazi,
        Bermuda,
        Bessarabia,
        Bohemia,
        Bombay,
        Bonin,
        Bosnia,
        Bosporus,
        Bougainville,
        BourgogneChampagne,
        Brandenburg,
        Brasilia,
        BrestLitovsk,
        Brisbane,
        BritishColumbia,
        Brittany,
        Bulgaria,
        Burma,
        Cairns,
        California,
        CameroonianJungle,
        Canarias,
        CantabricChain,
        CapVerdeIslands,
        Cape,
        Caracas,
        Carolinese,
        Catalonia,
        Celebes,
        CentralAfricaDesert,
        CentralAfricaPlains,
        CentralFinland,
        CentralItaly,
        CentralNorway,
        CentralRainforests,
        CentralSerbia,
        CentralSpain,
        CentralTurkey,
        Chahar,
        Chihuahua,
        Chile,
        ChileanArchipelago,
        Colorado,
        ConnecticutRhodeIsland,
        ContinentalSpain,
        Croatia,
        Cuba,
        DanakilPlateau,
        Darwin,
        Deccar,
        Delhi,
        Delta,
        Denmark,
        DiegoGarciaIsland,
        Dnepropretovsk,
        EastAndalucia,
        EastAtlas,
        EastEngland,
        EastJava,
        EastPersia,
        EastPrussia,
        EastSerbia,
        EasternAnatolia,
        EasternGhat,
        EasternHungary,
        Eire,
        Engels, // DHのみ
        ElAlamein,
        ElRif,
        EspirtuSanto,
        Estonia,
        EthiopianHighland,
        Fiji,
        Flanders,
        FloresTimor,
        Florida,
        Fujian,
        Gabes,
        Gansu,
        Georgia,
        Georgien,
        Goetaland,
        GoldCoast,
        GreekIsland,
        Grodno,
        Groznyi, // DHのみ
        Guadalcanal,
        Guangdong,
        Guangxi,
        Guayana,
        GuineanCoast,
        Guizhou,
        HannoverMagdeburg,
        Hawaii,
        Hebei,
        Hedjaz,
        Heilongjiang,
        Henan,
        Hessen,
        Himalayas,
        Hispaniola,
        HolsteinMecklemburg,
        Honshu,
        Hubei,
        Hunan,
        Iceland,
        Idaho,
        Illinois,
        Indiana,
        Indochine,
        InteriorAlgeria,
        InteriorLibya,
        Iowa,
        Iquitos,
        Iraq,
        Irkutsk,
        IvoryCoast,
        Jiangsu,
        Jiangxi,
        Jilin,
        JohnsonIsland,
        Kamchatka,
        Kansas,
        Karelia,
        Kassarine,
        Kazakstan,
        Kazan,
        Kentucky,
        Kenya,
        Khabarovsk,
        Kharkov,
        Kiev,
        KirgizSteppe,
        Kirgizistan,
        Kirov,
        Kola, // DHのみ
        KongoleseJungle,
        Kostroma,
        Krakow,
        Krasnodar,
        Krim,
        Kurdufan,
        Kursk,
        Kuybyshev,
        KyushuShikoku,
        LaPaz,
        LaPlata,
        Lae,
        Lakes,
        Lapland,
        Latvia,
        LebanonSyria,
        Leningrad,
        Levant,
        LeyteIslandGroup,
        Liaoning,
        LigurianIslands,
        Lima,
        LimousinAuvergne,
        Lithuania,
        Loire,
        LorraineAlsace,
        Louisiana,
        LowerArgentine,
        LowerCalifornia,
        Luzon,
        Lvov,
        Madagascar,
        Magadan,
        Magdalena,
        MaghrebCoast,
        Maine,
        Malacka,
        MalianDesert,
        MalianValleys,
        Manaus,
        Manitoba,
        Maracaibo,
        MarcusWake,
        Marshalls,
        MarylandDelaware,
        Massachussets,
        MatoGrosso,
        Mauretania,
        Melbourne,
        Melkosopotjnik,
        Memel,
        MexicoCity,
        Michigan,
        Midway,
        Mindanao,
        Minnesota,
        Minsk,
        Mississippi,
        Missouri,
        Mocambique,
        Molucks,
        Mongolia,
        Montana,
        Morocco,
        Moscow,
        Mozyr, // DHのみ
        Nagpur,
        Nalchik, // DHのみ
        Nambia,
        Nauru,
        Nebraska,
        NedreNorrland,
        Netherlands,
        NewBrunswick,
        NewFoundland,
        NewHampshire,
        NewJersey,
        NewMexico,
        NewYork,
        NewZealand,
        Nevada,
        Niassa,
        Nicaragua,
        Niger,
        NileValley,
        NizhnijNovgorod,
        Normandy,
        NorthBorneo,
        NorthCarolina,
        NorthDakota,
        NorthEngland,
        NorthEthiopia,
        NorthGilberts,
        NorthIreland,
        NorthItaly,
        NorthKrasnoyarsk,
        NorthMarianes,
        NorthNigeria,
        NorthPersia,
        NorthRhodesia,
        NorthRomania,
        NorthScotland,
        NorthernNorway,
        NorthwestTerritories,
        NovaScotia,
        Novgorod,
        Novosibirsk,
        NubianDesert,
        Odessa,
        Oesterbotten,
        OestraSvealand,
        OevreNorrland,
        Ohio,
        Oklahoma,
        Omsk,
        Ontario,
        Oran,
        Oregon,
        Orenburg, // DHのみ
        Orissa,
        Pakistan,
        PalawanMindoro,
        Palestine,
        PanamanRidge,
        Paris,
        PasDeCalais,
        Pennsylvania,
        Penza,
        Perm,
        PersianGulfCoast,
        Perth,
        Petrozavodsk, // DHのみ
        Phoenix,
        Polotsk,
        Poltava, // DHのみ
        Polynesia,
        PortMoresby,
        Portugal,
        Poznan,
        Primorski,
        Pskov,
        Pyongyang,
        PyreneesLanguedoc,
        Qattara,
        Qinghai,
        Quebec,
        Quito,
        Rabaul,
        Rajastan,
        RedSeaCoast,
        Rehe,
        ReservoireRybinsk,
        Rhineland, // DHのみ
        RioDeJaneiro,
        RioDeOro,
        Rogachev,
        Rostov,
        Ryazan,
        Sachsen,
        SakhalinKuriles,
        Samoa,
        SaoPaulo,
        Saransk,
        Sarmi,
        Saskatchewan,
        Senegambia,
        Senjavin,
        Seoul,
        Shaanxi,
        Shaba,
        Shandong,
        Shanxi,
        Siam,
        Sichuan,
        SicilianGap,
        SidiBarrani,
        Silesia, // DHのみ
        Sirte,
        SlaveCoast,
        Slovakia,
        Smolensk,
        Somalia,
        Somaliland,
        SouthBorneo,
        SouthCarolina,
        SouthDakota,
        SouthEthiopia,
        SouthFinland,
        SouthGilberts,
        SouthItaly,
        SouthKrasnoyarsk,
        SouthNigeria,
        SouthPersia,
        SouthRhodesia,
        SouthRomania,
        SouthScotland,
        SouthcentralNorway,
        SouthernNorway,
        StHelena,
        Stalingrad,
        SuezChannelArea,
        SuiyuanNingxia,
        Sumatra,
        SundaIslands,
        Sverdlovsk,
        Switzerland,
        Sydney,
        Syzran,
        Tadzjikistan,
        Taiwan,
        TajmyrEvenk,
        Tambov,
        Tanganyika,
        Tennessee,
        Texas,
        TheAzores,
        TheFalklands,
        Thrace,
        Tibet,
        TierraDelFuego,
        Tobruk,
        Tohoku,
        Tomsk,
        Transjordan,
        Transnistria,
        TransvaalNatal,
        Transylvania,
        Tunis,
        Turkestan,
        Turkmenistan,
        Tyumen,
        Ufa,
        Uganda,
        UpperArgentine,
        UpperNileValley,
        Utah,
        Uzbekistan,
        VaestraSvealand,
        Wallonia,
        Warsaw,
        Washington,
        VelikiyeLuki,
        VeraCruz,
        Vermont,
        Vorkuta, // DHのみ
        WestAndalucia,
        WestAtlas,
        WestEngland,
        WestJava,
        WestVirginia,
        WesternDesert,
        WesternGhat,
        WesternHungary,
        Westphalen,
        Virginia,
        Wisconsin,
        Vladimir,
        Volta,
        Wyoming,
        Xikang,
        Xinjiang,
        Yakutsk,
        YemeniteMountains,
        Yukon,
        Yunnan,
        Zabaikalye,
        Zhejiang,
        Ryukyusland,
        NorthBismarckArchipelago,
        NorthNewHebrides,
        SouthNewHebrides,
        CentralSolomons,
        WestAleutians,
        Ceylon,
        Hokkaido,
        NorthBurma,
        EastBengal,
        KraPeninsula,
        Tripoli,
        Greenland,

        // 海洋用
        Lake,
        AdriaticSea,
        AegeanSea,
        BlackSea,
        RedSea,
        TyrrhenianSea,
        BothnianBay,
        NorthernBaltic,
        SouthernBaltic,
        Kattegat,
        BarentsSea,
        LuzonStrait,
        PhilipineTrench,
        SuluSea,
        CelebesSea,
        CoastOfBrunei,
        FloresSea,
        MoluccaSea,
        BandaSea,
        WestCoralSea,
        ArafuraSea,
        JavaRidge,
        MalaccaStrait,
        GulfOfSiam,
        SpratlySea,
        CoastOfIndochina,
        TaiwanStrait,
        IrishSea,
        EnglishChannel,
        DenmarkStrait,
        SoutheastNorthsea,
        WestNorthsea,
        CentralNorthsea,
        NorwegianSea,
        CostaDelSol,
        AlgerianCoast,
        EgyptianCoast,
        GulfOfLyon,
        SeaOfJapan,
        NorthBismarckSea,
        SouthBismarckSea,
        Marianas,
        MarshallsSea,
        WesternSolomons,
        EasternSolomons,
        EastCoralSea,
        CoastOfJapan,
        WesternApproaches,
        GreenlandCoast,
        FaroesGap,
        NorthAtlantic,
        BayOfBiscay,
        Azores,
        PortugeseCoast,
        CapStVincent,
        CoastOfBrazil,
        CapVerde,
        GulfOfGuinea,
        CentralAtlantic,
        CoastOfAfrica,
        CoastOfBissao,
        SolomonSea,
        NorthTasmanSea,
        SouthTasmanSea,
        EastGreatAustralianBight,
        WestGreatAustralianBight,
        HudsonBay,
        PersianGulf,
        YucatanStrait,
        CentralCarribean,
        WindwardIslands,
        WestGulfOfMexico,
        EastGulfOfMexico,
        FloridaStrait,
        BermudaTriangle,
        NorthernSeaOfLabrador,
        SouthernSeaOfLabrador,
        GrandBanks,
        TheSeamounts,
        CanadianMaritimes,
        UsMaritimes,
        GuianaBasin,
        SouthCentralMidAtlanticRidge,
        CentralMidAtlanticRidge,
        Aleutians,
        Carolines,
        CentralPhilippineSea,
        CoastOfCeylon,
        CoastOfKamchatka,
        EastBayOfBengal,
        EastBeringSea,
        EastMarianaBasin,
        EastSeaOfOkhotsk,
        GulfOfAlaska,
        HawaiianRise,
        JavaTrench,
        MarianaTrench,
        MidPacificMountains,
        NinetyeastRidge,
        NorthNortheastPacificBasin,
        NorthwestPacificBasin,
        Ryukyus,
        SouthSeaOfOkhotsk,
        TimorSea,
        WestBayOfBengal,
        WestBeringSea,
        WestCoastOfAustralia,
        WestCoastOfMexico,
        WestCoastOfTheUnitedStates,
        WestSeaOfOkhotsk,
        YellowSea,
        AtlanticIndianRidge,
        CoastOfSouthAfrica,
        FijiBasin,
        Gilberts,
        LineIslands,
        MascarenePlateau,
        MidIndianRidge,
        NorthArabianSea,
        NorthEastPacificOcean,
        NorthMozambiqueChannel,
        NortheastCoastOfMadagascar,
        SouthArabianSea,
        SouthEastPacificOcean,
        SouthMozambiqueChannel,
        SoutheastCoastOfMadagascar,
        SoutheastIndianOcean,
        SouthwestIndianOcean,
        EastCostOfNewZealand,
        NorthSoutheastPacificOcean,
        SouthSoutheastPacificOcean,
        SoutheastPacificBasin,
        SouthwestPacificBasin,
        WestCoastOfCentralAmerica,
        WestCoastOfChile,
        WestCoastOfPeru,
        HornOfAfrica,
        CoastOfAngolaNamibia,
        AngolaPlain,
        ArgentinePlain,
        CoastOfArgentina,
        CoastOfUruguay,
        CoastOfNewGuinea,
        CoastOfGuyana,
        CoastOfRecife,
        CapeFinisterre,
        ArcticOcean,
        TheHebreides,
        IrishWestCoast,
        PernambucoPlain,
        AscensionFractureZone,
        EastNorthSea,
        CaspianSea // DHのみ
    }

    /// <summary>
    ///     地方ID
    /// </summary>
    public enum RegionId
    {
        None,

        // 陸地用
        Afghanistan,
        Alaska,
        Algeria,
        Amazonas,
        AmericanPacific,
        Anatolia,
        Andes,
        AsianPacific,
        Australia,
        Austria,
        Balkan,
        BalticStates,
        Benelux,
        Belarus,
        BismarckArchipelago,
        Brazil,
        BrazilianHighlands,
        Canada,
        Caribbean,
        CentralAfrica,
        CentralAmerica,
        CentralAsia,
        China,
        Czechoslovakia,
        Denmark,
        DiegoGarciaIsland,
        EastAfrica,
        EasternRussia,
        Egypt,
        England,
        Ethiopia,
        FarEastSiberia,
        Finland,
        France,
        Germany,
        GranChaco,
        GrandColombia,
        Greenland,
        HornofAfrica,
        Hungary,
        Iceland,
        India,
        Indochine,
        Indonesia,
        Ireland,
        Irkutsk,
        Italy,
        Japan,
        Kaukasus,
        Kazakstan,
        Korea,
        Krasnoyarsk,
        Libya,
        Madagascar,
        Mexico,
        MiddleEast,
        MidwestUs,
        Morocco,
        NewGuinea,
        NorthSolomonIslands,
        NortheastUs,
        NorthernPermafrost,
        NorthernRussia,
        NorthwestUs,
        Norway,
        Novosibirsk,
        Patagonia,
        Persia,
        Philippines,
        Poland,
        Portugal,
        RioDeLaPlata,
        Romania,
        Russia,
        Sahara,
        Scotland,
        SouthAfrica,
        SouthSolomonIslands,
        SouthcentralUs,
        SoutheastUs,
        SouthernRussia,
        SouthwestAfrica,
        SouthwestUs,
        Spain,
        Sudan,
        Sweden,
        Switzerland,
        Tomsk,
        Tunisia,
        Ukraine,
        Urals,
        WestAfrica,
        WhiteSeaTundra,
        EasternCanada,
        WesternCanada,
        WesternRussia,
        NorthernCanada,

        // 海洋用
        Lake,
        BlackSea,
        BalticSea,
        WesternMediterranean,
        CentralMediterranean,
        EasternMediterranean,
        SouthChinaSea,
        PhilippineSea,
        Moluccas,
        JavaSea,
        CoralSea,
        EastChinaSea,
        NorthSea,
        NortheastAtlantic,
        NorthAtlatic,
        NorthwestAtlantic,
        IceSea,
        TasmanSea,
        Carribean,
        GreatAustralianBight,
        Sargassos,
        MexicanGulf,
        CanadianArctic,
        GrandBanksShelf,
        EasternSeaboard,
        CentralMidAtlanticRidge,
        BismarckSea,
        BayOfBengal,
        BeringSea,
        CentralPacificOcean,
        EastIndianOcean,
        HomeIslands,
        NorthPacificOcean,
        SeaofOkhotsk,
        WestCoastOfNorthAmerica,
        ArabianSea,
        CapeOfGoodHope,
        CoastOfMadagascar,
        EastPacificOcean,
        MozambiqueChannel,
        SouthPacificOcean,
        SouthIndianOcean,
        WestIndianOcean,
        PacificAntarcticRidge,
        SoutheastPacificOcean,
        WestCoastOfSouthAmerica,
        SoutheastAtlanticOcean,
        SouthwestAtlanticOcean,
        WesternIceSea,
        BrazilianCoast,
        SouthcentralAtlantic,
        EasternAtlantic,
        CelticSea,
        IberianWestCoast
    }

    /// <summary>
    ///     大陸ID
    /// </summary>
    public enum ContinentId
    {
        None,
        NorthAmerica,
        SouthAmerica,
        Europe,
        Asia,
        Oceania,
        Africa,
        Lake,
        AtlanticOcean,
        PacificOcean,
        IndianOcean
    }

    /// <summary>
    ///     気候ID
    /// </summary>
    public enum ClimateId
    {
        None,
        Arctic,
        Subarctic,
        Temperate,
        Muddy,
        Mediterranean,
        Subtropical,
        Tropical,
        Arid
    }

    /// <summary>
    ///     地形ID
    /// </summary>
    public enum TerrainId
    {
        Plains,
        Forest,
        Mountain,
        Desert,
        Marsh,
        Hills,
        Jungle,
        Urban,
        Ocean,
        River,
        TerraIncognito,
        Unknown,
        Clear
    }

    /// <summary>
    ///     プロヴィンス項目ID
    /// </summary>
    public enum ProvinceItemId
    {
        Id, // プロヴィンスID
        Name, // プロヴィンス名
        Area, // 地域
        Region, // 地方
        Continent, // 大陸
        Climate, // 気候
        Terrain, // 地形
        SizeModifier, // サイズ補正
        AirCapacity, // 航空機許容量
        Infrastructure, // インフラ
        City, // 都市
        Beaches, // 砂浜の有無
        PortAllowed, // 港の有無
        PortSeaZone, // 港の海域
        Ic, // IC
        Manpower, // 労働力
        Oil, // 石油
        Metal, // 金属
        Energy, // エネルギー
        RareMaterials, // 希少資源
        CityXPos, // 都市のX座標
        CityYPos, // 都市のY座標
        ArmyXPos, // 軍隊のX座標
        ArmyYPos, // 軍隊のY座標
        PortXPos, // 港のX座標
        PortYPos, // 港のY座標
        BeachXPos, // 砂浜のX座標
        BeachYPos, // 砂浜のY座標
        BeachIcon, // 砂浜のアイコン
        FortXPos, // 要塞のX座標
        FortYPos, // 要塞のY座標
        AaXPos, // 対空砲のX座標
        AaYPos, // 対空砲のY座標
        CounterXPos, // カウンターのX座標
        CounterYPos, // カウンターのY座標
        TerrainVariant1,
        TerrainXPos1,
        TerrainYPos1,
        TerrainVariant2,
        TerrainXPos2,
        TerrainYPos2,
        TerrainVariant3,
        TerrainXPos3,
        TerrainYPos3,
        TerrainVariant4,
        FillCoordX1, // 塗りつぶしX座標1
        FillCoordY1, // 塗りつぶしY座標1
        FillCoordX2, // 塗りつぶしX座標2
        FillCoordY2, // 塗りつぶしY座標2
        FillCoordX3, // 塗りつぶしX座標3
        FillCoordY3, // 塗りつぶしY座標3
        FillCoordX4, // 塗りつぶしX座標4
        FillCoordY4, // 塗りつぶしY座標4
        FillCoordX5, // 塗りつぶしX座標5
        FillCoordY5, // 塗りつぶしY座標5
        FillCoordX6 // 塗りつぶしX座標6
    }
}