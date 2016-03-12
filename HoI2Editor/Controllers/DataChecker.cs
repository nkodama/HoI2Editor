using System.Collections.Generic;
using System.Linq;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     データチェック処理
    /// </summary>
    internal static class DataChecker
    {
        #region 内部フィールド

        /// <summary>
        ///     チェック結果出力フォーム
        /// </summary>
        private static DataCheckerForm _form;

        #endregion

        #region シナリオ

        /// <summary>
        ///     シナリオデータをチェックする
        /// </summary>
        internal static void CheckScenario()
        {
            // シナリオ読み込み前ならば何もしない
            if (!Scenarios.IsLoaded())
            {
                return;
            }

            WriteLine(Resources.CheckMessageScenario);

            List<Province> provinces = Provinces.Items.Where(province => province.IsLand && province.Id > 0).ToList();
            if (Scenarios.Data.Map != null)
            {
                provinces = Scenarios.Data.Map.All
                    ? provinces.Where(province => !Scenarios.Data.Map.No.Contains(province.Id)).ToList()
                    : provinces.Where(province => Scenarios.Data.Map.Yes.Contains(province.Id)).ToList();
            }

            // 保有プロヴィンス
            CheckOwnedProvinces(provinces);
            // 支配プロヴィンス
            CheckControlledProvinces(provinces);

            WriteLine(Resources.CheckMessageDone);
            WriteLine();
        }

        /// <summary>
        ///     保有プロヴィンスのチェック
        /// </summary>
        /// <param name="provinces">プロヴィンスリスト</param>
        private static void CheckOwnedProvinces(IEnumerable<Province> provinces)
        {
            foreach (Province province in provinces)
            {
                int id = province.Id;
                ProvinceSettings ps = Scenarios.GetProvinceSettings(id);
                IEnumerable<Country> countries = Scenarios.Data.Countries
                    .Where(settings => settings.OwnedProvinces.Contains(id))
                    .Select(settings => settings.Country).ToList();
                if (!countries.Any())
                {
                    string name = Scenarios.GetProvinceName(province, ps);
                    WriteLine("{0}: {1} [{2}]", Resources.CheckResultNoProvinceOwner, id, name);
                    Log.Error("[Scenario] no province owner: {0} [{1}]", id, name);
                }
                else if (countries.Count() > 1)
                {
                    string name = Scenarios.GetProvinceName(province, ps);
                    string tagList = Countries.GetTagList(countries);
                    WriteLine("{0}: {1} [{2}] - {3}", Resources.CheckResultDuplicatedProvinceOwner, id, name, tagList);
                    Log.Error("[Scenario] duplicated province owner: {0} [{1}] - {2}", id, name, tagList);
                }
            }
        }

        /// <summary>
        ///     支配プロヴィンスのチェック
        /// </summary>
        /// <param name="provinces">プロヴィンスリスト</param>
        private static void CheckControlledProvinces(IEnumerable<Province> provinces)
        {
            foreach (Province province in provinces)
            {
                int id = province.Id;
                ProvinceSettings ps = Scenarios.GetProvinceSettings(id);
                IEnumerable<Country> countries = Scenarios.Data.Countries
                    .Where(settings => settings.OwnedProvinces.Contains(id))
                    .Select(settings => settings.Country).ToList();
                if (!countries.Any())
                {
                    string name = Scenarios.GetProvinceName(province, ps);
                    WriteLine("{0}: {1} [{2}]", Resources.CheckResultNoProvinceController, id, name);
                    Log.Error("[Scenario] no province controller: {0} [{1}]", id, name);
                }
                else if (countries.Count() > 1)
                {
                    string name = Scenarios.GetProvinceName(province, ps);
                    string tagList = Countries.GetTagList(countries);
                    WriteLine("{0}: {1} [{2}] - {3}", Resources.CheckResultDuplicatedProvinceController, id, name,
                        tagList);
                    Log.Error("[Scenario] duplicated province controller: {0} [{1}] - {2}", id, name, tagList);
                }
            }
        }

        #endregion

        #region チェック結果出力

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        /// <param name="args">パラメータ</param>
        internal static void Write(string s, params object[] args)
        {
            if (_form == null)
            {
                _form = new DataCheckerForm();
            }
            if (!_form.Visible)
            {
                _form.Show();
            }
            _form.Write(s, args);
        }

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        /// <param name="s">対象文字列</param>
        /// <param name="args">パラメータ</param>
        internal static void WriteLine(string s, params object[] args)
        {
            if (_form == null)
            {
                _form = new DataCheckerForm();
            }
            if (!_form.Visible)
            {
                _form.Show();
            }
            _form.WriteLine(s, args);
        }

        /// <summary>
        ///     チェック結果を出力する
        /// </summary>
        internal static void WriteLine()
        {
            if (_form == null)
            {
                _form = new DataCheckerForm();
            }
            if (!_form.Visible)
            {
                _form.Show();
            }
            _form.WriteLine();
        }

        #endregion
    }
}