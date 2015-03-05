using System;
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
        ///     選択ノードのラベルを更新する
        /// </summary>
        /// <param name="name">ラベル名</param>
        public void UpdateNodeLabel(string name)
        {
            if (_treeView.SelectedNode == null)
            {
                return;
            }

            _treeView.SelectedNode.Text = name;
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
            Unit unit = new Unit();
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            CountrySettings settings;

            // 陸軍/海軍/空軍ユニットのルートノード
            if (parent == null)
            {
                switch ((NodeType) node.Tag)
                {
                    case NodeType.Land:
                        unit.Branch = Branch.Army;
                        node.Nodes.Add(CreateLandUnitNode(unit));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.LandUnits.Add(unit);
                        break;

                    case NodeType.Naval:
                        unit.Branch = Branch.Navy;
                        node.Nodes.Add(CreateNavalUnitNode(unit));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.NavalUnits.Add(unit);
                        break;

                    case NodeType.Air:
                        unit.Branch = Branch.Airforce;
                        node.Nodes.Add(CreateAirUnitNode(unit));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.AirUnits.Add(unit);
                        break;
                }
                return;
            }

            // 搭載ユニットのルートノード
            Unit transport = parent.Tag as Unit;
            if (transport != null)
            {
                unit.Branch = Branch.Army;
                node.Nodes.Add(CreateLandUnitNode(unit));
                transport.LandUnits.Add(unit);
                return;
            }

            // 陸軍/海軍/空軍/搭載ユニット
            int index = parent.Nodes.IndexOf(node) + 1;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    unit.Branch = Branch.Army;
                    parent.Nodes.Insert(index, CreateLandUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandUnits.Insert(index, unit);
                    break;

                case NodeType.Naval:
                    unit.Branch = Branch.Navy;
                    parent.Nodes.Insert(index, CreateNavalUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalUnits.Insert(index, unit);
                    break;

                case NodeType.Air:
                    unit.Branch = Branch.Airforce;
                    parent.Nodes.Insert(index, CreateAirUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirUnits.Insert(index, unit);
                    break;

                case NodeType.Boarding:
                    unit.Branch = Branch.Army;
                    parent.Nodes.Insert(index, CreateLandUnitNode(unit));
                    transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Insert(index, unit);
                    break;
            }
        }

        /// <summary>
        ///     師団を追加する
        /// </summary>
        public void AddDivision()
        {
            Division division = new Division();
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            CountrySettings settings;

            // 未配備陸軍/海軍/空軍師団のルートノード
            if (parent == null)
            {
                switch ((NodeType) node.Tag)
                {
                    case NodeType.UndeployedLand:
                        division.Branch = Branch.Army;
                        node.Nodes.Add(CreateLandDivisionNode(division));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.LandDivisions.Add(division);
                        break;

                    case NodeType.UndeployedNaval:
                        division.Branch = Branch.Navy;
                        node.Nodes.Add(CreateNavalDivisionNode(division));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.NavalDivisions.Add(division);
                        break;

                    case NodeType.UndeployedAir:
                        division.Branch = Branch.Airforce;
                        node.Nodes.Add(CreateAirDivisionNode(division));
                        settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                        settings.AirDivisions.Add(division);
                        break;
                }
                return;
            }

            // 陸軍/海軍/空軍/搭載ユニット
            Unit unit = node.Tag as Unit;
            if (unit != null)
            {
                division.Branch = unit.Branch;
                switch ((NodeType) parent.Tag)
                {
                    case NodeType.Land:
                    case NodeType.Boarding:
                        node.Nodes.Add(CreateLandDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;

                    case NodeType.Naval:
                        node.Nodes.Add(CreateNavalDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;

                    case NodeType.Air:
                        node.Nodes.Add(CreateAirDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;
                }
                return;
            }

            // 陸軍/海軍/空軍/搭載師団
            unit = parent.Tag as Unit;
            if (unit != null)
            {
                division.Branch = unit.Branch;
                switch ((NodeType) parent.Parent.Tag)
                {
                    case NodeType.Land:
                    case NodeType.Boarding:
                        parent.Nodes.Add(CreateLandDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;

                    case NodeType.Naval:
                        parent.Nodes.Add(CreateNavalDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;

                    case NodeType.Air:
                        parent.Nodes.Add(CreateAirDivisionNode(division));
                        unit.Divisions.Add(division);
                        break;
                }
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    division.Branch = Branch.Army;
                    parent.Nodes.Add(CreateLandDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandDivisions.Add(division);
                    break;

                case NodeType.UndeployedNaval:
                    division.Branch = Branch.Navy;
                    parent.Nodes.Add(CreateNavalDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalDivisions.Add(division);
                    break;

                case NodeType.UndeployedAir:
                    division.Branch = Branch.Airforce;
                    parent.Nodes.Add(CreateAirDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirDivisions.Add(division);
                    break;
            }
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
            Unit unit = original.Clone();
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            int index = parent.Nodes.IndexOf(node) + 1;

            // 陸軍/海軍/空軍/搭載ユニット
            CountrySettings settings;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    parent.Nodes.Insert(index, CreateLandUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandUnits.Insert(index, unit);
                    break;

                case NodeType.Naval:
                    parent.Nodes.Insert(index, CreateNavalUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalUnits.Insert(index, unit);
                    break;

                case NodeType.Air:
                    parent.Nodes.Insert(index, CreateAirUnitNode(unit));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirUnits.Insert(index, unit);
                    break;

                case NodeType.Boarding:
                    parent.Nodes.Insert(index, CreateLandUnitNode(unit));
                    Unit transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Insert(index, unit);
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
            TreeNode node = _treeView.SelectedNode;
            TreeNode parent = node.Parent;
            int index = parent.Nodes.IndexOf(node) + 1;

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                switch ((NodeType) parent.Parent.Tag)
                {
                    case NodeType.Land:
                    case NodeType.Boarding:
                        parent.Nodes.Insert(index, CreateLandDivisionNode(division));
                        unit.Divisions.Insert(index, division);
                        break;

                    case NodeType.Naval:
                        parent.Nodes.Insert(index, CreateNavalDivisionNode(division));
                        unit.Divisions.Insert(index, division);
                        break;

                    case NodeType.Air:
                        parent.Nodes.Insert(index, CreateAirDivisionNode(division));
                        unit.Divisions.Insert(index, division);
                        break;
                }
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            CountrySettings settings;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    parent.Nodes.Insert(index, CreateLandDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedNaval:
                    parent.Nodes.Insert(index, CreateNavalDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedAir:
                    parent.Nodes.Insert(index, CreateAirDivisionNode(division));
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirDivisions.Insert(index, division);
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
            CountrySettings settings;

            parent.Nodes.Remove(node);

            // 陸軍/海軍/空軍/搭載ユニット
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandUnits.Remove(unit);
                    break;

                case NodeType.Naval:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalUnits.Remove(unit);
                    break;

                case NodeType.Air:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
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

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                unit.Divisions.Remove(division);
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            CountrySettings settings;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandDivisions.Remove(division);
                    break;

                case NodeType.UndeployedNaval:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalDivisions.Remove(division);
                    break;

                case NodeType.UndeployedAir:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
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
            CountrySettings settings;

            parent.Nodes.Remove(node);
            parent.Nodes.Insert(index, node);

            // 陸軍/海軍/空軍/搭載ユニット
            switch ((NodeType) parent.Tag)
            {
                case NodeType.Land:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandUnits.Remove(unit);
                    settings.LandUnits.Insert(index, unit);
                    break;

                case NodeType.Naval:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalUnits.Remove(unit);
                    settings.NavalUnits.Insert(index, unit);
                    break;

                case NodeType.Air:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirUnits.Remove(unit);
                    settings.AirUnits.Insert(index, unit);
                    break;

                case NodeType.Boarding:
                    Unit transport = (Unit) parent.Parent.Tag;
                    transport.LandUnits.Remove(unit);
                    transport.LandUnits.Insert(index, unit);
                    break;
            }
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

            // 陸軍/海軍/空軍/搭載師団
            Unit unit = parent.Tag as Unit;
            if (unit != null)
            {
                unit.Divisions.Remove(division);
                unit.Divisions.Insert(index, division);
                return;
            }

            // 未配備陸軍/海軍/空軍師団
            CountrySettings settings;
            switch ((NodeType) parent.Tag)
            {
                case NodeType.UndeployedLand:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.LandDivisions.Remove(division);
                    settings.LandDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedNaval:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.NavalDivisions.Remove(division);
                    settings.NavalDivisions.Insert(index, division);
                    break;

                case NodeType.UndeployedAir:
                    settings = Scenarios.GetCountrySettings(_country) ?? Scenarios.CreateCountrySettings(_country);
                    settings.AirDivisions.Remove(division);
                    settings.AirDivisions.Insert(index, division);
                    break;
            }
        }

        #endregion

        #region ノード選択

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
                case NodeType.Boarding:
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