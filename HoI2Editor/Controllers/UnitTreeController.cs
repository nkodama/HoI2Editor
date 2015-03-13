using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HoI2Editor.Models;
using HoI2Editor.Properties;

namespace HoI2Editor.Controllers
{
    /// <summary>
    ///     ユニットツリーのコントローラクラス
    /// </summary>
    public class UnitTreeController
    {
        #region 公開プロパティ

        /// <summary>
        ///     選択国
        /// </summary>
        public Country Country
        {
            get { return _country; }
            set
            {
                _country = value;
                Update();
            }
        }

        #endregion

        #region 内部フィールド

        /// <summary>
        ///     ユニットツリービュー
        /// </summary>
        private readonly TreeView _treeView;

        /// <summary>
        ///     選択国
        /// </summary>
        private Country _country;

        #endregion

        #region 内部定数

        /// <summary>
        ///     特殊ノードの種類
        /// </summary>
        public enum NodeType
        {
            Land,
            Naval,
            Air,
            Boarding,
            UndeployedLand,
            UndeployedNaval,
            UndeployedAir,
            DivisionDevelopment,
            ConvoyDevelopment,
            ProvinceDevelopment
        }

        #endregion

        #region 公開イベント

        /// <summary>
        ///     ノード選択時の処理
        /// </summary>
        public event EventHandler<UnitTreeViewEventArgs> AfterSelect;

        #endregion

        #region 初期化

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="treeView">ユニットツリービュー</param>
        public UnitTreeController(TreeView treeView)
        {
            _treeView = treeView;

            Init();
        }

        /// <summary>
        ///     初期化処理
        /// </summary>
        private void Init()
        {
            _treeView.AfterSelect += OnUnitTreeViewAfterSelect;
        }

        #endregion

        #region ツリー表示

        /// <summary>
        ///     ユニットツリーを更新する
        /// </summary>
        public void Update()
        {
            _treeView.BeginUpdate();
            _treeView.Nodes.Clear();

            CountrySettings settings = Scenarios.GetCountrySettings(_country);

            // 陸軍ユニット
            TreeNode node = new TreeNode(Resources.UnitTreeLand) { Tag = NodeType.Land };
            if (settings != null)
            {
                foreach (Unit unit in settings.LandUnits)
                {
                    node.Nodes.Add(CreateLandUnitNode(unit));
                }
            }
            _treeView.Nodes.Add(node);

            // 海軍ユニット
            node = new TreeNode(Resources.UnitTreeNaval) { Tag = NodeType.Naval };
            if (settings != null)
            {
                foreach (Unit unit in settings.NavalUnits)
                {
                    node.Nodes.Add(CreateNavalUnitNode(unit));
                }
            }
            _treeView.Nodes.Add(node);

            // 空軍ユニット
            node = new TreeNode(Resources.UnitTreeAir) { Tag = NodeType.Air };
            if (settings != null)
            {
                foreach (Unit unit in settings.AirUnits)
                {
                    node.Nodes.Add(CreateAirUnitNode(unit));
                }
            }
            _treeView.Nodes.Add(node);

            // 未配備陸軍師団
            node = new TreeNode(Resources.UnitTreeUndeployedLand) { Tag = NodeType.UndeployedLand };
            if (settings != null)
            {
                foreach (Division division in settings.LandDivisions)
                {
                    node.Nodes.Add(CreateLandDivisionNode(division));
                }
            }
            _treeView.Nodes.Add(node);

            // 未配備海軍師団
            node = new TreeNode(Resources.UnitTreeUndeployedNaval) { Tag = NodeType.UndeployedNaval };
            if (settings != null)
            {
                foreach (Division division in settings.NavalDivisions)
                {
                    node.Nodes.Add(CreateNavalDivisionNode(division));
                }
            }
            _treeView.Nodes.Add(node);

            // 未配備空軍師団
            node = new TreeNode(Resources.UnitTreeUndeployedAir) { Tag = NodeType.UndeployedAir };
            if (settings != null)
            {
                foreach (Division division in settings.AirDivisions)
                {
                    node.Nodes.Add(CreateAirDivisionNode(division));
                }
            }
            _treeView.Nodes.Add(node);

            _treeView.EndUpdate();
        }

        /// <summary>
        ///     選択ユニットノードのラベルを更新する
        /// </summary>
        public void UpdateUnitNodeLabel(string name)
        {
            TreeNode node = GetSelectedUnitNode();
            if (node == null)
            {
                return;
            }

            node.Text = name;
        }

        /// <summary>
        ///     選択師団ノードのラベルを更新する
        /// </summary>
        public void UpdateDivisionNodeLabel(string name)
        {
            TreeNode node = GetSelectedDivisionNode();
            if (node == null)
            {
                return;
            }

            node.Text = name;
        }

        /// <summary>
        ///     ユニットのノードを作成する
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateUnitNode(Unit unit)
        {
            TreeNode node = new TreeNode(unit.Name) { Tag = unit };

            // 師団
            foreach (Division division in unit.Divisions)
            {
                node.Nodes.Add(CreateDivisionNode(division));
            }

            // 搭載ユニット
            if (unit.Branch == Branch.Navy || unit.Branch == Branch.Airforce)
            {
                TreeNode boarding = new TreeNode(Resources.UnitTreeBoarding) { Tag = NodeType.Boarding };
                foreach (Unit landUnit in unit.LandUnits)
                {
                    boarding.Nodes.Add(CreateUnitNode(landUnit));
                }
                node.Nodes.Add(boarding);
            }

            return node;
        }

        /// <summary>
        ///     師団のノードを作成する
        /// </summary>
        /// <param name="division">師団</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateDivisionNode(Division division)
        {
            return new TreeNode(division.Name) { Tag = division };
        }

        /// <summary>
        ///     陸軍ユニットのノードを作成する
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateLandUnitNode(Unit unit)
        {
            TreeNode node = new TreeNode(unit.Name) { Tag = unit };

            // 陸軍師団
            foreach (Division division in unit.Divisions)
            {
                node.Nodes.Add(CreateLandDivisionNode(division));
            }

            return node;
        }

        /// <summary>
        ///     海軍ユニットのノードを作成する
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateNavalUnitNode(Unit unit)
        {
            TreeNode node = new TreeNode(unit.Name) { Tag = unit };

            // 海軍師団
            foreach (Division division in unit.Divisions)
            {
                node.Nodes.Add(CreateNavalDivisionNode(division));
            }

            // 搭載ユニット
            TreeNode boarding = new TreeNode(Resources.UnitTreeBoarding) { Tag = NodeType.Boarding };
            foreach (Unit landUnit in unit.LandUnits)
            {
                boarding.Nodes.Add(CreateLandUnitNode(landUnit));
            }
            node.Nodes.Add(boarding);

            return node;
        }

        /// <summary>
        ///     空軍ユニットのノードを作成する
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateAirUnitNode(Unit unit)
        {
            TreeNode node = new TreeNode(unit.Name) { Tag = unit };

            // 空軍師団
            foreach (Division division in unit.Divisions)
            {
                node.Nodes.Add(CreateAirDivisionNode(division));
            }

            // 搭載ユニット
            TreeNode boarding = new TreeNode(Resources.UnitTreeBoarding) { Tag = NodeType.Boarding };
            foreach (Unit landUnit in unit.LandUnits)
            {
                boarding.Nodes.Add(CreateLandUnitNode(landUnit));
            }
            node.Nodes.Add(boarding);

            return node;
        }

        /// <summary>
        ///     陸軍師団のノードを作成する
        /// </summary>
        /// <param name="division">師団</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateLandDivisionNode(Division division)
        {
            return new TreeNode(division.Name) { Tag = division };
        }

        /// <summary>
        ///     海軍師団のノードを作成する
        /// </summary>
        /// <param name="division">師団</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateNavalDivisionNode(Division division)
        {
            return new TreeNode(division.Name) { Tag = division };
        }

        /// <summary>
        ///     空軍師団のノードを作成する
        /// </summary>
        /// <param name="division">師団</param>
        /// <returns>ツリーノード</returns>
        private static TreeNode CreateAirDivisionNode(Division division)
        {
            return new TreeNode(division.Name) { Tag = division };
        }

        #endregion

        #region ツリー操作

        /// <summary>
        ///     ユニットを追加する
        /// </summary>
        public void AddUnit()
        {
            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;

            // 陸軍/海軍/空軍ユニットのルートノード
            if (parent == null)
            {
                switch ((NodeType) node.Tag)
                {
                    case NodeType.Land:
                        AddUnit(Branch.Army, node, settings.LandUnits, settings);
                        break;

                    case NodeType.Naval:
                        AddUnit(Branch.Navy, node, settings.NavalUnits, settings);
                        break;

                    case NodeType.Air:
                        AddUnit(Branch.Airforce, node, settings.AirUnits, settings);
                        break;
                }
                return;
            }

            // 搭載ユニットのルートノード
            Unit transport = parent.Tag as Unit;
            if (transport != null)
            {
                AddUnit(Branch.Army, node, transport.LandUnits, settings);
                return;
            }

            // 陸軍/海軍/空軍/搭載ユニット
            int index = parent.Nodes.IndexOf(node) + 1;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    AddUnit(index, Branch.Army, parent, settings.LandUnits, settings);
                    break;

                case NodeType.Naval:
                    AddUnit(index, Branch.Navy, parent, settings.NavalUnits, settings);
                    break;

                case NodeType.Air:
                    AddUnit(index, Branch.Airforce, parent, settings.AirUnits, settings);
                    break;

                case NodeType.Boarding:
                    transport = (Unit) parent.Parent.Tag;
                    AddUnit(index, Branch.Army, parent, transport.LandUnits, settings);
                    break;
            }
        }

        /// <summary>
        ///     ユニットを追加する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="parent">親ノード</param>
        /// <param name="units">ユニットリスト</param>
        /// <param name="settings">国家設定</param>
        private void AddUnit(Branch branch, TreeNode parent, List<Unit> units, CountrySettings settings)
        {
            // ユニットを作成する
            Unit unit = new Unit { Id = settings.GetNewUnitTypeId(), Branch = branch };
            unit.SetDirtyAll();

            // ユニットの位置を初期化する
            InitUnitLocation(unit, settings);

            // ツリーノードを追加する
            TreeNode node = CreateUnitNode(unit);
            node.Text = Resources.UnitTreeNewUnit;
            parent.Nodes.Add(node);

            // ユニットリストに追加する
            units.Add(unit);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 追加したノードを選択する
            _treeView.SelectedNode = node;
        }

        /// <summary>
        ///     ユニットを追加する
        /// </summary>
        /// <param name="index">追加先のインデックス</param>
        /// <param name="branch">兵科</param>
        /// <param name="parent">親ノード</param>
        /// <param name="units">ユニットリスト</param>
        /// <param name="settings">国家設定</param>
        private void AddUnit(int index, Branch branch, TreeNode parent, List<Unit> units, CountrySettings settings)
        {
            // ユニットを作成する
            Unit unit = new Unit { Id = settings.GetNewUnitTypeId(), Branch = branch };
            unit.SetDirtyAll();

            // ユニットの位置を初期化する
            InitUnitLocation(unit, settings);

            // ツリーノードを追加する
            TreeNode node = CreateUnitNode(unit);
            node.Text = Resources.UnitTreeNewUnit;
            parent.Nodes.Insert(index, node);

            // ユニットリストに追加する
            units.Insert(index, unit);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 追加したノードを選択する
            _treeView.SelectedNode = node;
        }

        /// <summary>
        ///     ユニットの位置を初期化する
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <param name="settings">国家設定</param>
        private static void InitUnitLocation(Unit unit, CountrySettings settings)
        {
            ProvinceSettings capitalSettings;

            switch (unit.Branch)
            {
                case Branch.Army:
                    unit.Location = settings.Capital;
                    break;

                case Branch.Navy:
                    capitalSettings = Scenarios.GetProvinceSettings(settings.Capital);
                    if (capitalSettings != null && capitalSettings.NavalBase != null)
                    {
                        unit.Location = settings.Capital;
                        unit.Base = settings.Capital;
                    }
                    else
                    {
                        foreach (ProvinceSettings ps in settings.ControlledProvinces
                            .Select(Scenarios.GetProvinceSettings)
                            .Where(ps => ps != null && ps.NavalBase != null))
                        {
                            unit.Location = ps.Id;
                            unit.Base = ps.Id;
                            break;
                        }
                    }
                    break;

                case Branch.Airforce:
                    capitalSettings = Scenarios.GetProvinceSettings(settings.Capital);
                    if (capitalSettings != null && capitalSettings.AirBase != null)
                    {
                        unit.Location = settings.Capital;
                        unit.Base = settings.Capital;
                    }
                    else
                    {
                        foreach (ProvinceSettings ps in settings.ControlledProvinces
                            .Select(Scenarios.GetProvinceSettings)
                            .Where(ps => ps != null && ps.AirBase != null))
                        {
                            unit.Location = ps.Id;
                            unit.Base = ps.Id;
                            break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        ///     師団を追加する
        /// </summary>
        public void AddDivision()
        {
            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 未配備陸軍/海軍/空軍師団のルートノード
            if (parent == null)
            {
                switch ((NodeType) node.Tag)
                {
                    case NodeType.UndeployedLand:
                        AddDivision(Branch.Army, node, settings.LandDivisions, settings);
                        break;

                    case NodeType.UndeployedNaval:
                        AddDivision(Branch.Navy, node, settings.NavalDivisions, settings);
                        break;

                    case NodeType.UndeployedAir:
                        AddDivision(Branch.Airforce, node, settings.AirDivisions, settings);
                        break;
                }
                return;
            }

            // 陸軍/海軍/空軍/搭載ユニット
            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                AddDivision(unit.Branch, node, unit.Divisions, settings);
                return;
            }

            int index = parent.Nodes.IndexOf(node) + 1;

            // 陸軍/海軍/空軍/搭載師団
            unit = parent.Tag as Unit;
            if (unit != null)
            {
                AddDivision(index, unit.Branch, parent, unit.Divisions, settings);
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    AddDivision(index, Branch.Army, parent, settings.LandDivisions, settings);
                    break;

                case NodeType.UndeployedNaval:
                    AddDivision(index, Branch.Navy, parent, settings.NavalDivisions, settings);
                    break;

                case NodeType.UndeployedAir:
                    AddDivision(index, Branch.Airforce, parent, settings.AirDivisions, settings);
                    break;
            }
        }

        /// <summary>
        ///     師団を追加する
        /// </summary>
        /// <param name="branch">兵科</param>
        /// <param name="parent">親ノード</param>
        /// <param name="divisions">師団リスト</param>
        /// <param name="settings">国家設定</param>
        private void AddDivision(Branch branch, TreeNode parent, List<Division> divisions, CountrySettings settings)
        {
            // 師団を作成する
            Division division = new Division { Id = settings.GetNewUnitTypeId(), Branch = branch };
            division.SetDirtyAll();
            switch (branch)
            {
                case Branch.Army:
                    division.Type = UnitType.Infantry;
                    break;

                case Branch.Navy:
                    division.Type = UnitType.BattleShip;
                    break;

                case Branch.Airforce:
                    division.Type = UnitType.MultiRole;
                    break;
            }

            // ツリーノードを追加する
            TreeNode node = CreateDivisionNode(division);
            node.Text = Resources.UnitTreeNewDivision;
            int index = parent.Nodes.Count;
            if ((parent.Tag is Unit) && (branch == Branch.Navy || branch == Branch.Airforce))
            {
                index--;
            }
            parent.Nodes.Insert(index, node);

            // 師団リストに追加する
            divisions.Add(division);

            // 追加したノードを選択する
            _treeView.SelectedNode = node;
        }

        /// <summary>
        ///     ユニットを追加する
        /// </summary>
        /// <param name="index">追加先のインデックス</param>
        /// <param name="branch">兵科</param>
        /// <param name="parent">親ノード</param>
        /// <param name="divisions">師団リスト</param>
        /// <param name="settings">国家設定</param>
        private void AddDivision(int index, Branch branch, TreeNode parent, List<Division> divisions,
            CountrySettings settings)
        {
            // 師団を作成する
            Division division = new Division { Id = settings.GetNewUnitTypeId(), Branch = branch };
            division.SetDirtyAll();
            switch (branch)
            {
                case Branch.Army:
                    division.Type = UnitType.Infantry;
                    break;

                case Branch.Navy:
                    division.Type = UnitType.BattleShip;
                    break;

                case Branch.Airforce:
                    division.Type = UnitType.MultiRole;
                    break;
            }

            // ツリーノードを追加する
            TreeNode node = CreateDivisionNode(division);
            node.Text = Resources.UnitTreeNewDivision;
            parent.Nodes.Insert(index, node);

            // 師団リストに追加する
            divisions.Insert(index, division);

            // 追加したノードを選択する
            _treeView.SelectedNode = node;
        }

        /// <summary>
        ///     ユニット/師団を複製する
        /// </summary>
        public void Clone()
        {
            TreeNode node = _treeView.SelectedNode;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                CloneUnit(unit);
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                CloneDivision(division);
            }
        }

        /// <summary>
        ///     ユニットを複製する
        /// </summary>
        /// <param name="original">複製対象のユニット</param>
        private void CloneUnit(Unit original)
        {
            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);
            Unit unit = original.Clone();
            unit.SetDirtyAll();
            TreeNode selected = _treeView.SelectedNode;
            TreeNode parent = selected.Parent;
            TreeNode node;
            int index = parent.Nodes.IndexOf(selected) + 1;

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載ユニット
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    node = CreateLandUnitNode(unit);
                    if (string.IsNullOrEmpty(unit.Name))
                    {
                        node.Text = Resources.UnitTreeNewUnit;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.LandUnits.Insert(index, unit);
                    _treeView.SelectedNode = node;
                    break;

                case NodeType.Naval:
                    node = CreateNavalUnitNode(unit);
                    if (string.IsNullOrEmpty(unit.Name))
                    {
                        node.Text = Resources.UnitTreeNewUnit;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.NavalUnits.Insert(index, unit);
                    _treeView.SelectedNode = node;
                    break;

                case NodeType.Air:
                    node = CreateAirUnitNode(unit);
                    if (string.IsNullOrEmpty(unit.Name))
                    {
                        node.Text = Resources.UnitTreeNewUnit;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.AirUnits.Insert(index, unit);
                    _treeView.SelectedNode = node;
                    break;

                case NodeType.Boarding:
                    node = CreateLandUnitNode(unit);
                    if (string.IsNullOrEmpty(unit.Name))
                    {
                        node.Text = Resources.UnitTreeNewUnit;
                    }
                    parent.Nodes.Insert(index, node);
                    Unit transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Insert(index, unit);
                    _treeView.SelectedNode = node;
                    break;
            }
        }

        /// <summary>
        ///     師団を複製する
        /// </summary>
        /// <param name="original">複製対象の師団</param>
        private void CloneDivision(Division original)
        {
            Division division = original.Clone();
            division.SetDirtyAll();
            TreeNode selected = _treeView.SelectedNode;
            TreeNode parent = selected.Parent;
            TreeNode node;
            int index = parent.Nodes.IndexOf(selected) + 1;

            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                switch ((NodeType) parent.Parent.Tag)
                {
                    case NodeType.Land:
                    case NodeType.Boarding:
                        node = CreateLandDivisionNode(division);
                        if (string.IsNullOrEmpty(division.Name))
                        {
                            node.Text = Resources.UnitTreeNewDivision;
                        }
                        parent.Nodes.Insert(index, node);
                        unit.Divisions.Insert(index, division);
                        _treeView.SelectedNode = node;
                        break;

                    case NodeType.Naval:
                        node = CreateNavalDivisionNode(division);
                        if (string.IsNullOrEmpty(division.Name))
                        {
                            node.Text = Resources.UnitTreeNewDivision;
                        }
                        parent.Nodes.Insert(index, node);
                        unit.Divisions.Insert(index, division);
                        _treeView.SelectedNode = node;
                        break;

                    case NodeType.Air:
                        node = CreateAirDivisionNode(division);
                        if (string.IsNullOrEmpty(division.Name))
                        {
                            node.Text = Resources.UnitTreeNewDivision;
                        }
                        parent.Nodes.Insert(index, node);
                        unit.Divisions.Insert(index, division);
                        _treeView.SelectedNode = node;
                        break;
                }
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    node = CreateLandDivisionNode(division);
                    if (string.IsNullOrEmpty(division.Name))
                    {
                        node.Text = Resources.UnitTreeNewDivision;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.LandDivisions.Insert(index, division);
                    _treeView.SelectedNode = node;
                    break;

                case NodeType.UndeployedNaval:
                    node = CreateNavalDivisionNode(division);
                    if (string.IsNullOrEmpty(division.Name))
                    {
                        node.Text = Resources.UnitTreeNewDivision;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.NavalDivisions.Insert(index, division);
                    _treeView.SelectedNode = node;
                    break;

                case NodeType.UndeployedAir:
                    node = CreateAirDivisionNode(division);
                    if (string.IsNullOrEmpty(division.Name))
                    {
                        node.Text = Resources.UnitTreeNewDivision;
                    }
                    parent.Nodes.Insert(index, node);
                    settings.AirDivisions.Insert(index, division);
                    _treeView.SelectedNode = node;
                    break;
            }
        }

        /// <summary>
        ///     ユニット/師団を削除する
        /// </summary>
        public void Remove()
        {
            TreeNode node = _treeView.SelectedNode;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                // ユニットを削除する
                RemoveUnit(unit);

                // typeとidの組を削除する
                unit.RemoveTypeId();
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                // 師団を削除する
                RemoveDivision(division);

                // typeとidの組を削除する
                division.RemoveTypeId();
            }
        }

        /// <summary>
        ///     ユニットを削除する
        /// </summary>
        /// <param name="unit">削除対象のユニット</param>
        private void RemoveUnit(Unit unit)
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;

            parent.Nodes.Remove(node);

            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載ユニット
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    settings.LandUnits.Remove(unit);
                    break;

                case NodeType.Naval:
                    settings.NavalUnits.Remove(unit);
                    break;

                case NodeType.Air:
                    settings.AirUnits.Remove(unit);
                    break;

                case NodeType.Boarding:
                    Unit transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Remove(unit);
                    break;
            }
        }

        /// <summary>
        ///     師団を削除する
        /// </summary>
        /// <param name="division">削除対象の師団</param>
        private void RemoveDivision(Division division)
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;

            parent.Nodes.Remove(node);

            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                unit.Divisions.Remove(division);
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    settings.LandDivisions.Remove(division);
                    break;

                case NodeType.UndeployedNaval:
                    settings.NavalDivisions.Remove(division);
                    break;

                case NodeType.UndeployedAir:
                    settings.AirDivisions.Remove(division);
                    break;
            }
        }

        /// <summary>
        ///     ユニット/師団を先頭へ移動する
        /// </summary>
        public void MoveTop()
        {
            TreeNode node = _treeView.SelectedNode;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                MoveUnit(unit, 0);
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                MoveDivision(division, 0);
            }
        }

        /// <summary>
        ///     ユニット/師団を1つ上へ移動する
        /// </summary>
        public void MoveUp()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            int index = parent.Nodes.IndexOf(node) - 1;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                MoveUnit(unit, index);
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                MoveDivision(division, index);
            }
        }

        /// <summary>
        ///     ユニット/師団を1つ下へ移動する
        /// </summary>
        public void MoveDown()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            int index = parent.Nodes.IndexOf(node) + 1;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                MoveUnit(unit, index);
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                MoveDivision(division, index);
            }
        }

        /// <summary>
        ///     ユニット/師団を末尾へ移動する
        /// </summary>
        public void MoveBottom()
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            int index = parent.Nodes.Count - 1;

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                MoveUnit(unit, index);
            }

            Division division = node.Tag as Division;
            if (division != null)
            {
                if ((NodeType) parent.Nodes[parent.Nodes.Count - 1].Tag == NodeType.Boarding)
                {
                    index--;
                }
                MoveDivision(division, index);
            }
        }

        /// <summary>
        ///     ユニットを移動する
        /// </summary>
        /// <param name="unit">移動対象のユニット</param>
        /// <param name="index">移動先のインデックス</param>
        private void MoveUnit(Unit unit, int index)
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;

            parent.Nodes.Remove(node);
            parent.Nodes.Insert(index, node);

            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載ユニット
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    settings.LandUnits.Remove(unit);
                    settings.LandUnits.Insert(index, unit);
                    break;

                case NodeType.Naval:
                    settings.NavalUnits.Remove(unit);
                    settings.NavalUnits.Insert(index, unit);
                    break;

                case NodeType.Air:
                    settings.AirUnits.Remove(unit);
                    settings.AirUnits.Insert(index, unit);
                    break;

                case NodeType.Boarding:
                    Unit transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Remove(unit);
                    transport.LandUnits.Insert(index, unit);
                    break;
            }

            // 移動対象のノードを選択する
            _treeView.SelectedNode = node;
        }

        /// <summary>
        ///     師団を移動する
        /// </summary>
        /// <param name="division">移動対象の師団</param>
        /// <param name="index">移動先のインデックス</param>
        private void MoveDivision(Division division, int index)
        {
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            parent.Nodes.Remove(node);
            parent.Nodes.Insert(index, node);

            CountrySettings settings = Scenarios.GetCountrySettings(_country) ??
                                       Scenarios.CreateCountrySettings(_country);

            // 編集済みフラグを設定する
            settings.SetDirty();
            Scenarios.SetDirty();

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                unit.Divisions.Remove(division);
                unit.Divisions.Insert(index, division);

                // 移動対象のノードを選択する
                _treeView.SelectedNode = node;
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    settings.LandDivisions.Remove(division);
                    settings.LandDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedNaval:
                    settings.NavalDivisions.Remove(division);
                    settings.NavalDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedAir:
                    settings.AirDivisions.Remove(division);
                    settings.AirDivisions.Insert(index, division);
                    break;
            }

            // 移動対象のノードを選択する
            _treeView.SelectedNode = node;
        }

        #endregion

        #region ノード選択

        /// <summary>
        ///     選択中のユニットを取得する
        /// </summary>
        /// <returns>選択中のユニット</returns>
        public Unit GetSelectedUnit()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node == null)
            {
                return null;
            }

            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                return unit;
            }

            node = node.Parent;
            if (node == null)
            {
                return null;
            }

            return node.Tag as Unit;
        }

        /// <summary>
        ///     選択中の師団を取得する
        /// </summary>
        /// <returns>選択中の師団</returns>
        public Division GetSelectedDivision()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node == null)
            {
                return null;
            }

            return node.Tag as Division;
        }

        /// <summary>
        ///     選択中のユニットノードを取得する
        /// </summary>
        /// <returns>選択中のユニットノード</returns>
        private TreeNode GetSelectedUnitNode()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node == null)
            {
                return null;
            }

            if (node.Tag is Unit)
            {
                return node;
            }

            node = node.Parent;
            if (node != null && node.Tag is Unit)
            {
                return node;
            }

            return null;
        }

        /// <summary>
        ///     選択中の師団ノードを取得する
        /// </summary>
        /// <returns>選択中の師団ノード</returns>
        private TreeNode GetSelectedDivisionNode()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node != null && node.Tag is Division)
            {
                return node;
            }

            return null;
        }

        /// <summary>
        ///     ユニットツリービューのノード選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            Unit unit = e.Node.Tag as Unit;
            if (unit != null)
            {
                OnUnitAfterSelect(unit, sender, e);
                return;
            }

            Division division = e.Node.Tag as Division;
            if (division != null)
            {
                OnDivisionAfterSelect(division, sender, e);
                return;
            }

            OnOtherAfterSelect(sender, e);
        }

        /// <summary>
        ///     ユニットツリービューのユニットノード選択時の処理
        /// </summary>
        /// <param name="unit">ユニット</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitAfterSelect(Unit unit, object sender, TreeViewEventArgs e)
        {
            int index = e.Node.Parent.Nodes.IndexOf(e.Node);
            int bottom = e.Node.Parent.Nodes.Count - 1;

            UnitTreeViewEventArgs args = new UnitTreeViewEventArgs(e)
            {
                Unit = unit,
                CanAddUnit = true,
                CanAddDivision = true,
                IsTop = (index == 0),
                IsBottom = (index == bottom)
            };

            if (AfterSelect != null)
            {
                AfterSelect(sender, args);
            }
        }

        /// <summary>
        ///     ユニットツリービューの師団ノード選択時の処理
        /// </summary>
        /// <param name="division">師団</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDivisionAfterSelect(Division division, object sender, TreeViewEventArgs e)
        {
            int index = e.Node.Parent.Nodes.IndexOf(e.Node);
            int bottom = e.Node.Parent.Nodes.Count - 1;
            // 搭載ユニットの分-1する
            if (division.Branch == Branch.Navy || division.Branch == Branch.Airforce)
            {
                bottom--;
            }

            UnitTreeViewEventArgs args = new UnitTreeViewEventArgs(e)
            {
                Division = division,
                Unit = e.Node.Parent.Tag as Unit,
                CanAddDivision = true,
                IsTop = (index <= 0),
                IsBottom = (index >= bottom)
            };

            if (AfterSelect != null)
            {
                AfterSelect(sender, args);
            }
        }

        /// <summary>
        ///     ユニットツリービューのユニット/師団以外のノード選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOtherAfterSelect(object sender, TreeViewEventArgs e)
        {
            UnitTreeViewEventArgs args = new UnitTreeViewEventArgs(e);
            switch ((NodeType) e.Node.Tag)
            {
                case NodeType.Land:
                case NodeType.Naval:
                case NodeType.Air:
                    args.CanAddUnit = true;
                    break;

                case NodeType.Boarding:
                    args.Unit = e.Node.Parent.Tag as Unit;
                    args.CanAddUnit = true;
                    break;

                case NodeType.UndeployedLand:
                case NodeType.UndeployedNaval:
                case NodeType.UndeployedAir:
                    args.CanAddDivision = true;
                    break;
            }

            if (AfterSelect != null)
            {
                AfterSelect(sender, args);
            }
        }

        #endregion

        #region 内部クラス

        /// <summary>
        ///     ユニットツリーイベントのパラメータ
        /// </summary>
        public class UnitTreeViewEventArgs : TreeViewEventArgs
        {
            /// <summary>
            ///     対象ユニット
            /// </summary>
            public Unit Unit { get; set; }

            /// <summary>
            ///     対象師団
            /// </summary>
            public Division Division { get; set; }

            /// <summary>
            ///     ユニット追加可能かどうか
            /// </summary>
            public bool CanAddUnit { get; set; }

            /// <summary>
            ///     師団追加可能かどうか
            /// </summary>
            public bool CanAddDivision { get; set; }

            /// <summary>
            ///     先頭ノードかどうか
            /// </summary>
            public bool IsTop { get; set; }

            /// <summary>
            ///     末尾ノードかどうか
            /// </summary>
            public bool IsBottom { get; set; }

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <param name="e">ツリーイベントのパラメータ</param>
            public UnitTreeViewEventArgs(TreeViewEventArgs e) : base(e.Node, e.Action)
            {
            }
        }

        #endregion
    }
}