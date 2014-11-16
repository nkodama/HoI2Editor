﻿using System;
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
            string fileName = Game.GetReadFileName(Game.MiscPathName);
            Log.Verbose("[Scenario] Load: {0}", Path.GetFileName(fileName));
            try
            {
                Data = new Scenario();
                ScenarioParser.Parse(fileName, Data);
            }
            catch (Exception)
            {
                Log.Error("[Scenario] Read error: {0}", fileName);
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileReadError, fileName),
                    Resources.EditorMisc, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string fileName = Game.GetWriteFileName(_fileName);
            string folderName = Path.GetDirectoryName(fileName);

            try
            {
                // シナリオフォルダがなければ作成する
                if (!string.IsNullOrEmpty(folderName) && !Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                Log.Info("[Scenario] Save: {0}", Path.GetFileName(fileName));
            }
            catch (Exception)
            {
                Log.Error("[Unit] Write error: {0}", fileName);
                MessageBox.Show(string.Format("{0}: {1}", Resources.FileWriteError, fileName), Resources.EditorScenario,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 編集済みフラグを解除する
            ResetDirtyAll();

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