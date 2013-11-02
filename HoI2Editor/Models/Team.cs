using System;

namespace HoI2Editor.Models
{
    /// <summary>
    ///     研究機関データ
    /// </summary>
    public class Team
    {
        #region 公開プロパティ

        /// <summary>
        ///     国タグ
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     研究機関ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     画像ファイル名
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        ///     スキル
        /// </summary>
        public int Skill { get; set; }

        /// <summary>
        ///     開始年
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        ///     終了年
        /// </summary>
        public int EndYear { get; set; }

        /// <summary>
        ///     研究特性
        /// </summary>
        public TechSpeciality[] Specialities
        {
            get { return _specialities; }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     項目の編集済みフラグ
        /// </summary>
        private readonly bool[] _dirtyFlags = new bool[Enum.GetValues(typeof (TeamItemId)).Length];

        /// <summary>
        ///     研究特性
        /// </summary>
        private readonly TechSpeciality[] _specialities = new TechSpeciality[SpecialityLength];

        /// <summary>
        ///     編集済みフラグ
        /// </summary>
        private bool _dirtyFlag;

        #endregion

        #region 公開定数

        /// <summary>
        ///     研究特性定義の数
        /// </summary>
        public const int SpecialityLength = 32;

        #endregion

        #region 編集済みフラグ操作

        /// <summary>
        ///     研究機関データが編集済みかどうかを取得する
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
        public bool IsDirty(TeamItemId id)
        {
            return _dirtyFlags[(int) id];
        }

        /// <summary>
        ///     編集済みフラグを設定する
        /// </summary>
        /// <param name="id">項目ID</param>
        public void SetDirty(TeamItemId id)
        {
            _dirtyFlags[(int) id] = true;
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て設定する
        /// </summary>
        public void SetDirtyAll()
        {
            foreach (TeamItemId id in Enum.GetValues(typeof (TeamItemId)))
            {
                _dirtyFlags[(int) id] = true;
            }
            _dirtyFlag = true;
        }

        /// <summary>
        ///     編集済みフラグを全て解除する
        /// </summary>
        public void ResetDirtyAll()
        {
            foreach (TeamItemId id in Enum.GetValues(typeof (TeamItemId)))
            {
                _dirtyFlags[(int) id] = false;
            }
            _dirtyFlag = false;
        }

        #endregion
    }

    /// <summary>
    ///     研究機関項目ID
    /// </summary>
    public enum TeamItemId
    {
        Country, // 国家
        Id, // ID
        Name, // 名前
        Skill, // スキル
        StartYear, // 開始年
        EndYear, // 終了年
        PictureName, // 画像ファイル名
        Speciality1, // 研究特性1
        Speciality2, // 研究特性2
        Speciality3, // 研究特性3
        Speciality4, // 研究特性4
        Speciality5, // 研究特性5
        Speciality6, // 研究特性6
        Speciality7, // 研究特性7
        Speciality8, // 研究特性8
        Speciality9, // 研究特性9
        Speciality10, // 研究特性10
        Speciality11, // 研究特性11
        Speciality12, // 研究特性12
        Speciality13, // 研究特性13
        Speciality14, // 研究特性14
        Speciality15, // 研究特性15
        Speciality16, // 研究特性16
        Speciality17, // 研究特性17
        Speciality18, // 研究特性18
        Speciality19, // 研究特性19
        Speciality20, // 研究特性20
        Speciality21, // 研究特性21
        Speciality22, // 研究特性22
        Speciality23, // 研究特性23
        Speciality24, // 研究特性24
        Speciality25, // 研究特性25
        Speciality26, // 研究特性26
        Speciality27, // 研究特性27
        Speciality28, // 研究特性28
        Speciality29, // 研究特性29
        Speciality30, // 研究特性30
        Speciality31, // 研究特性31
        Speciality32, // 研究特性32
    }
}