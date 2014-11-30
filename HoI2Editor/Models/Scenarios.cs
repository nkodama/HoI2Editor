using System;
using System.IO;
using System.Windows.Forms;
using HoI2Editor.Parsers;
using HoI2Editor.Properties;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     シナリオデータ群
    /// </summary>
    public static class Scenarios
    {
        #region 公開プロパティ

        /// <summary>
        ///     シナリオデータ
        /// </summary>
        public static Scenario Data { get; private set; }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     シナリオファイル名
        /// </summary>
        private static string _fileName;

        /// <summary>
        ///     読み込み済みフラグ
        /// </summary>
        private static bool _loaded;

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private static bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     月名文字列
        /// </summary>
        public static readonly string[] MonthStrings =
        {
            "january",
            "february",
            "march",
            "april",
            "may",
            "june",
            "july",
            "august",
            "september",
            "october",
            "november",
            "december"
        };

        /// <summary>
        ///     天候文字列
        /// </summary>
        public static readonly string[] WeatherStrings =
        {
            "",
            "clear",
            "frozen",
            "raining",
            "snowing",
            "storm",
            "blizzard",
            "muddy"
        };

        /// <summary>
        ///     政体文字列
        /// </summary>
        public static readonly string[] GovernmentStrings =
        {
            "",
            "nazi",
            "fascist",
            "paternal_autocrat",
            "social_conservative",
            "market_liberal",
            "social_liberal",
            "social_democrat",
            "left_wing_radical",
            "leninist",
            "stalinist"
        };

        /// <summary>
        ///     建物文字列
        /// </summary>
        public static readonly string[] BuildingStrings =
        {
            "",
            "ic",
            "infrastructure",
            "coastal_fort",
            "land_fort",
            "flak",
            "air_base",
            "naval_base",
            "radar_station",
            "nuclear_reactor",
            "rocket_test",
            "synthetic_oil",
            "synthetic_rares",
            "nuclear_power"
        };

        /// <summary>
        ///     陸軍任務文字列
        /// </summary>
        public static readonly string[] LandMissionStrings =
        {
            "",
            "attack",
            "strat_redeploy",
            "support_attack",
            "support_defense",
            "reserves",
            "anti_partisan_duty"
        };

        /// <summary>
        ///     海軍任務文字列
        /// </summary>
        public static readonly string[] NavalMissionStrings =
        {
            "",
            "rebase",
            "convoy_raiding",
            "asw",
            "naval_interdiction",
            "shore_bombardment",
            "amphibious_assault",
            "sea_transport",
            "naval_combat_patrol",
            "naval_port_strike",
            "naval_airbase_strike"
        };

        /// <summary>
        ///     空軍任務文字列
        /// </summary>
        public static readonly string[] AirMissionStrings =
        {
            "",
            "air_superiority",
            "ground_attack",
            "runway_cratering",
            "installation_strike",
            "interdiction",
            "naval_strike",
            "port_strike",
            "logistical_strike",
            "strategic_bombardment",
            "air_supply",
            "airborne_assault",
            "convoy_air_raiding",
            "nuke"
        };

        #endregion

        #region ファイル読み込み

        /// <summary>
        ///     ファイルを読み込み済みかを取得する
        /// </summary>
        /// <returns>ファイルを読み込みならばtrueを返す</returns>
        public static bool IsLoaded()
        {
            return _loaded;
        }

        /// <summary>
        ///     ファイルの再読み込みを要求する
        /// </summary>
        public static void RequestReload()
        {
            _loaded = false;
        }

        /// <summary>
        ///     ユニット定義ファイル群を再読み込みする
        /// </summary>
        public static void Reload()
        {
            // 読み込み前なら何もしない
            if (!_loaded)
            {
                return;
            }

            _loaded = false;

            LoadFiles();
        }

        /// <summary>
        ///     シナリオファイル群を読み込む
        /// </summary>
        public static void Load(string fileName)
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            _fileName = fileName;

            LoadFiles();
        }

        /// <summary>
        ///     シナリオファイルを読み込む
        /// </summary>
        private static void LoadFiles()
        {
            // 読み込み済みならば戻る
            if (_loaded)
            {
                return;
            }

            // シナリオファイルを解釈する
            Log.Verbose("[Scenario] Load: {0}", Path.GetFileName(_fileName));
            try
            {
                Data = new Scenario();
                ScenarioParser.Parse(_fileName, Data);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Read error: {0}", _fileName);
                MessageBox.Show(String.Format("{0}: {1}", Resources.FileReadError, _fileName),
                    Resources.EditorScenario, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 編集済みフラグを全て解除する
            ResetDirtyAll();

            // 読み込み済みフラグを設定する
            _loaded = true;
        }

        #endregion

        #region ファイル書き込み

        /// <summary>
        ///     ユニット定義ファイル群を保存する
        /// </summary>
        /// <returns>保存に失敗すればfalseを返す</returns>
        public static bool Save()
        {
            return true;
        }

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     編集済みかどうかを取得する
        /// </summary>
        /// <returns>編集済みならばtrueを返す</returns>
        public static bool IsDirty()
        {
            return _dirtyFlag;
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        public static void SetDirty()
        {
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        private static void ResetDirtyAll()
        {
            _dirtyFlag = false;
        }

        #endregion
    }
}