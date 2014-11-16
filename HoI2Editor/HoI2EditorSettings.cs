using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using HoI2Editor.Forms;
using HoI2Editor.Models;
using HoI2Editor.Utilities;

namespace HoI2Editor
{
    /// <summary>
    ///     エディタの設定を管理するクラス
    /// </summary>
    public class HoI2EditorSettings
    {
        #region 公開プロパティ

        #region メインフォーム

        /// <summary>
        ///     メインフォームの設定
        /// </summary>
        public MainFormSettings Main = new MainFormSettings();

        #endregion

        #region 指揮官エディタ

        /// <summary>
        ///     指揮官エディタの設定
        /// </summary>
        public LeaderEditorSettings LeaderEditor = new LeaderEditorSettings();

        #endregion

        #region 閣僚エディタ

        /// <summary>
        ///     閣僚エディタの設定
        /// </summary>
        public MinisterEditorSettings MinisterEditor = new MinisterEditorSettings();

        #endregion

        #region 研究機関エディタ

        /// <summary>
        ///     研究機関エディタの設定
        /// </summary>
        public TeamEditorSettings TeamEditor = new TeamEditorSettings();

        #endregion

        #region プロヴィンスエディタ

        /// <summary>
        ///     プロヴィンスエディタの設定
        /// </summary>
        public ProvinceEditorSettings ProvinceEditor = new ProvinceEditorSettings();

        #endregion

        #region 技術ツリーエディタ

        /// <summary>
        ///     技術ツリーエディタの設定
        /// </summary>
        public TechEditorSettings TechEditor = new TechEditorSettings();

        #endregion

        #region ユニットモデルエディタ

        /// <summary>
        ///     ユニットモデルエディタの設定
        /// </summary>
        public UnitEditorSettings UnitEditor = new UnitEditorSettings();

        #endregion

        #region 基礎データエディタ

        /// <summary>
        ///     基礎データエディタの設定
        /// </summary>
        public MiscEditorSettings MiscEditor = new MiscEditorSettings();

        #endregion

        #region 軍団名エディタ

        /// <summary>
        ///     軍団名エディタの設定
        /// </summary>
        public CorpsNameEditorSettings CorpsNameEditor = new CorpsNameEditorSettings();

        #endregion

        #region ユニット名エディタ

        /// <summary>
        ///     ユニット名エディタの設定
        /// </summary>
        public UnitNameEditorSettings UnitNameEditor = new UnitNameEditorSettings();

        #endregion

        #region ユニットモデル名エディタ

        /// <summary>
        ///     ユニットモデル名エディタの設定
        /// </summary>
        public ModelNameEditorSettings ModelNameEditor = new ModelNameEditorSettings();

        #endregion

        #region ランダム指揮官名エディタ

        /// <summary>
        ///     ランダム指揮官名エディタの設定
        /// </summary>
        public RandomLeaderEditorSettings RandomLeaderEditor = new RandomLeaderEditorSettings();

        #endregion

        #region 研究速度ビューア

        /// <summary>
        ///     研究速度ビューアの設定
        /// </summary>
        public ResearchViewerSettings ResearchViewer = new ResearchViewerSettings();

        #endregion

        #region シナリオエディタ

        /// <summary>
        ///     シナリオエディタの設定
        /// </summary>
        public ScenarioEditorSettings ScenarioEditor = new ScenarioEditorSettings();

        #endregion

        #endregion

        #region 初期化

        /// <summary>
        ///     設定値を丸める
        /// </summary>
        public void Round()
        {
            Main.Round();
            LeaderEditor.Round();
            MinisterEditor.Round();
            TeamEditor.Round();
            ProvinceEditor.Round();
            TechEditor.Round();
            UnitEditor.Round();
            MiscEditor.Round();
            CorpsNameEditor.Round();
            UnitNameEditor.Round();
            ModelNameEditor.Round();
            RandomLeaderEditor.Round();
            ResearchViewer.Round();
            ScenarioEditor.Round();
        }

        /// <summary>
        ///     フォームの位置を丸める
        /// </summary>
        /// <param name="location">現在の位置</param>
        /// <param name="size">現在のサイズ</param>
        /// <param name="defaultWidth">デフォルト幅</param>
        /// <param name="defaultHeight">デフォルト高さ</param>
        /// <returns>丸めた後の位置</returns>
        private static Rectangle RoundFormPosition(Point location, Size size, int defaultWidth, int defaultHeight)
        {
            // デスクトップのサイズを取得する
            Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

            // フォームのサイズを丸める
            int width = size.Width;
            int scaledWidth = DeviceCaps.GetScaledWidth(defaultWidth);
            if ((width > screenRect.Width) || (width < scaledWidth))
            {
                width = scaledWidth;
            }
            int height = size.Height;
            int scaledHeight = DeviceCaps.GetScaledHeight(defaultHeight);
            if ((height > screenRect.Height) || (height < scaledHeight))
            {
                height = scaledHeight;
            }

            // フォームの位置を丸める
            int x = location.X;
            if (x < screenRect.Left)
            {
                x = screenRect.Left;
            }
            else if (x >= screenRect.Right)
            {
                x = screenRect.Right - 1;
            }
            int y = location.Y;
            if (y < screenRect.Top)
            {
                y = screenRect.Top;
            }
            else if (y >= screenRect.Bottom)
            {
                y = screenRect.Bottom - 1;
            }

            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        ///     フォームの位置を丸める (低解像度と高解像度でデフォルトの高さが異なる場合)
        /// </summary>
        /// <param name="location">現在の位置</param>
        /// <param name="size">現在のサイズ</param>
        /// <param name="defaultWidth">デフォルト幅</param>
        /// <param name="defaultHeightShort">デフォルト高さ(低解像度)</param>
        /// <param name="defaultHeightLong">デフォルト高さ(高解像度)</param>
        /// <returns>丸めた後の位置</returns>
        private static Rectangle RoundFormPosition(Point location, Size size,
            int defaultWidth, int defaultHeightShort, int defaultHeightLong)
        {
            // デスクトップのサイズを取得する
            Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

            // フォームのサイズを丸める
            int width = size.Width;
            int scaledWidth = DeviceCaps.GetScaledWidth(defaultWidth);
            if ((width > screenRect.Width) || (width < scaledWidth))
            {
                width = scaledWidth;
            }
            int height = size.Height;
            int scaledHeightShort = DeviceCaps.GetScaledHeight(defaultHeightShort);
            if ((height > screenRect.Height) || (height < scaledHeightShort))
            {
                int scaledHeightLong = DeviceCaps.GetScaledHeight(defaultHeightLong);
                height = (screenRect.Height >= scaledHeightLong) ? scaledHeightLong : scaledHeightShort;
            }

            // フォームの位置を丸める
            int x = location.X;
            if (x < screenRect.Left)
            {
                x = screenRect.Left;
            }
            else if (x >= screenRect.Right)
            {
                x = screenRect.Right - 1;
            }
            int y = location.Y;
            if (y < screenRect.Top)
            {
                y = screenRect.Top;
            }
            else if (y >= screenRect.Bottom)
            {
                y = screenRect.Bottom - 1;
            }

            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        ///     フォームの位置を丸める (低解像度/中解像度/高解像度でデフォルトの高さが異なる場合)
        /// </summary>
        /// <param name="location">現在の位置</param>
        /// <param name="size">現在のサイズ</param>
        /// <param name="defaultWidth">デフォルト幅</param>
        /// <param name="defaultHeightShort">デフォルト高さ(低解像度)</param>
        /// <param name="defaultHeightMiddle">デフォルト高さ(中解像度)</param>
        /// <param name="defaultHeightLong">デフォルト高さ(高解像度)</param>
        /// <returns>丸めた後の位置</returns>
        private static Rectangle RoundFormPosition(Point location, Size size, int defaultWidth, int defaultHeightShort,
            int defaultHeightMiddle, int defaultHeightLong)
        {
            // デスクトップのサイズを取得する
            Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

            // フォームのサイズを丸める
            int width = size.Width;
            int scaledWidth = DeviceCaps.GetScaledWidth(defaultWidth);
            if ((width > screenRect.Width) || (width < scaledWidth))
            {
                width = scaledWidth;
            }
            int height = size.Height;
            int scaledHeightShort = DeviceCaps.GetScaledHeight(defaultHeightShort);
            if ((height > screenRect.Height) || (height < scaledHeightShort))
            {
                int scaledHeightMiddle = DeviceCaps.GetScaledHeight(defaultHeightMiddle);
                int scaledHeightLong = DeviceCaps.GetScaledHeight(defaultHeightLong);
                height = (screenRect.Height >= scaledHeightLong)
                    ? scaledHeightLong
                    : (screenRect.Height >= scaledHeightMiddle) ? scaledHeightMiddle : scaledHeightShort;
            }

            // フォームの位置を丸める
            int x = location.X;
            if (x < screenRect.Left)
            {
                x = screenRect.Left;
            }
            else if (x >= screenRect.Right)
            {
                x = screenRect.Right - 1;
            }
            int y = location.Y;
            if (y < screenRect.Top)
            {
                y = screenRect.Top;
            }
            else if (y >= screenRect.Bottom)
            {
                y = screenRect.Bottom - 1;
            }

            return new Rectangle(x, y, width, height);
        }

        #endregion

        #region メインフォーム

        /// <summary>
        ///     メインフォームの設定
        /// </summary>
        public class MainFormSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ゲームフォルダ名
            /// </summary>
            public string GameFolder
            {
                get { return Game.FolderName; }
                set { Game.FolderName = value; }
            }

            /// <summary>
            ///     MODフォルダ名
            /// </summary>
            public string ModFolder
            {
                get { return Game.ModName; }
                set { Game.ModName = value; }
            }

            /// <summary>
            ///     保存フォルダ名
            /// </summary>
            public string ExportFolder
            {
                get { return Game.ExportName; }
                set { Game.ExportName = value; }
            }

            /// <summary>
            ///     ログ出力レベル
            /// </summary>
            public int LogLevel
            {
                get { return Log.Level; }
                set { Log.Level = value; }
            }

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 480;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 350;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public MainFormSettings()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region 指揮官エディタ

        /// <summary>
        ///     指揮官エディタの設定
        /// </summary>
        public class LeaderEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     指揮官リストビューの列の幅
            /// </summary>
            public int[] ListColumnWidth
            {
                get { return _listColumnWidth; }
                set { _listColumnWidth = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public List<Country> Countries
            {
                get { return _countries; }
                set { _countries = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     選択中の国家
            /// </summary>
            private List<Country> _countries = new List<Country>();

            /// <summary>
            ///     指揮官リストビューの列の幅
            /// </summary>
            private int[] _listColumnWidth = new int[LeaderEditorForm.LeaderListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 1000;

            /// <summary>
            ///     デフォルト高さ(低解像度)
            /// </summary>
            private const int DefaultHeightShort = 670;

            /// <summary>
            ///     デフォルト高さ(高解像度)
            /// </summary>
            private const int DefaultHeightLong = 720;

            /// <summary>
            ///     指揮官リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultListColumnWidth = {40, 60, 250, 55, 70, 70, 50, 50, 290};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public LeaderEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int longHeight = DeviceCaps.GetScaledHeight(DefaultHeightLong);
                int shortHeight = DeviceCaps.GetScaledHeight(DefaultHeightShort);
                int height = (screenRect.Height >= longHeight) ? longHeight : shortHeight;
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // 指揮官リストビューの列の幅を設定する
                for (int i = 0; i < LeaderEditorForm.LeaderListColumnCount; i++)
                {
                    ListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeightShort, DefaultHeightLong);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);

                // 選択中の国家がない場合、先頭になるAFGを選択する
                if (Countries.Count == 0)
                {
                    Countries.Add(Country.AFG);
                }
            }

            #endregion
        }

        #endregion

        #region 閣僚エディタ

        /// <summary>
        ///     閣僚エディタの設定
        /// </summary>
        public class MinisterEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     閣僚リストビューの列の幅
            /// </summary>
            public int[] ListColumnWidth
            {
                get { return _listColumnWidth; }
                set { _listColumnWidth = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public List<Country> Countries
            {
                get { return _countries; }
                set { _countries = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     選択中の国家
            /// </summary>
            private List<Country> _countries = new List<Country>();

            /// <summary>
            ///     閣僚リストビューの列の幅
            /// </summary>
            private int[] _listColumnWidth = new int[MinisterEditorForm.MinisterListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 800;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 600;

            /// <summary>
            ///     閣僚リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultListColumnWidth = {40, 60, 180, 50, 50, 95, 160, 100};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public MinisterEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // 閣僚リストビューの列の幅を設定する
                for (int i = 0; i < MinisterEditorForm.MinisterListColumnCount; i++)
                {
                    ListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);

                // 選択中の国家がない場合、先頭になるAFGを選択する
                if (Countries.Count == 0)
                {
                    Countries.Add(Country.AFG);
                }
            }

            #endregion
        }

        #endregion

        #region 研究機関エディタ

        /// <summary>
        ///     研究機関エディタの設定
        /// </summary>
        public class TeamEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     研究機関リストビューの列の幅
            /// </summary>
            public int[] ListColumnWidth
            {
                get { return _listColumnWidth; }
                set { _listColumnWidth = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public List<Country> Countries
            {
                get { return _countries; }
                set { _countries = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     選択中の国家
            /// </summary>
            private List<Country> _countries = new List<Country>();

            /// <summary>
            ///     研究機関リストビューの列の幅
            /// </summary>
            private int[] _listColumnWidth = new int[TeamEditorForm.TeamListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 800;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 600;

            /// <summary>
            ///     研究機関リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultListColumnWidth = {40, 60, 300, 50, 50, 50, 185};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public TeamEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // 研究機関リストビューの列の幅を設定する
                for (int i = 0; i < TeamEditorForm.TeamListColumnCount; i++)
                {
                    ListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);

                // 選択中の国家がない場合、先頭になるAFGを選択する
                if (Countries.Count == 0)
                {
                    Countries.Add(Country.AFG);
                }
            }

            #endregion
        }

        #endregion

        #region プロヴィンスエディタ

        /// <summary>
        ///     プロヴィンスエディタの設定
        /// </summary>
        public class ProvinceEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     プロヴィンスリストビューの列の幅
            /// </summary>
            public int[] ListColumnWidth
            {
                get { return _listColumnWidth; }
                set { _listColumnWidth = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     プロヴィンスリストビューの列の幅
            /// </summary>
            private int[] _listColumnWidth = new int[ProvinceEditorForm.ProvinceListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 800;

            /// <summary>
            ///     デフォルト高さ(低解像度)
            /// </summary>
            private const int DefaultHeightShort = 670;

            /// <summary>
            ///     デフォルト高さ(高解像度)
            /// </summary>
            private const int DefaultHeightLong = 720;

            /// <summary>
            ///     プロヴィンスリストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultListColumnWidth = {185, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public ProvinceEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int longHeight = DeviceCaps.GetScaledHeight(DefaultHeightLong);
                int shortHeight = DeviceCaps.GetScaledHeight(DefaultHeightShort);
                int height = (screenRect.Height >= longHeight) ? longHeight : shortHeight;
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // プロヴィンスリストビューの列の幅を設定する
                for (int i = 0; i < ProvinceEditorForm.ProvinceListColumnCount; i++)
                {
                    ListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeightShort, DefaultHeightLong);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region 技術ツリーエディタ

        /// <summary>
        ///     技術ツリーエディタの設定
        /// </summary>
        public class TechEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     AND必要技術リストビューの列の幅
            /// </summary>
            public int[] AndRequiredListColumnWidth
            {
                get { return _andRequiredListColumnWidth; }
                set { _andRequiredListColumnWidth = value; }
            }

            /// <summary>
            ///     OR必要技術リストビューの列の幅
            /// </summary>
            public int[] OrRequiredListColumnWidth
            {
                get { return _orRequiredListColumnWidth; }
                set { _orRequiredListColumnWidth = value; }
            }

            /// <summary>
            ///     小研究リストビューの列の幅
            /// </summary>
            public int[] ComponentListColumnWidth
            {
                get { return _componentListColumnWidth; }
                set { _componentListColumnWidth = value; }
            }

            /// <summary>
            ///     技術効果リストビューの列の幅
            /// </summary>
            public int[] EffectListColumnWidth
            {
                get { return _effectListColumnWidth; }
                set { _effectListColumnWidth = value; }
            }

            /// <summary>
            ///     技術座標リストビューの列の幅
            /// </summary>
            public int[] TechPositionListColumnWidth
            {
                get { return _techPositionListColumnWidth; }
                set { _techPositionListColumnWidth = value; }
            }

            /// <summary>
            ///     ラベル座標リストビューの列の幅
            /// </summary>
            public int[] LabelPositionListColumnWidth
            {
                get { return _labelPositionListColumnWidth; }
                set { _labelPositionListColumnWidth = value; }
            }

            /// <summary>
            ///     イベント座標リストビューの列の幅
            /// </summary>
            public int[] EventPositionListColumnWidth
            {
                get { return _eventPositionListColumnWidth; }
                set { _eventPositionListColumnWidth = value; }
            }

            /// <summary>
            ///     技術カテゴリリストボックスの選択項目
            /// </summary>
            public int Category { get; set; }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     AND必要技術リストビューの列の幅
            /// </summary>
            private int[] _andRequiredListColumnWidth = new int[TechEditorForm.RequiredListColumnCount];

            /// <summary>
            ///     小研究リストビューの列の幅
            /// </summary>
            private int[] _componentListColumnWidth = new int[TechEditorForm.ComponentListColumnCount];

            /// <summary>
            ///     技術効果リストビューの列の幅
            /// </summary>
            private int[] _effectListColumnWidth = new int[TechEditorForm.EffectListColumnCount];

            /// <summary>
            ///     イベント座標リストビューの列の幅
            /// </summary>
            private int[] _eventPositionListColumnWidth = new int[TechEditorForm.PositionListColumnCount];

            /// <summary>
            ///     ラベル座標リストビューの列の幅
            /// </summary>
            private int[] _labelPositionListColumnWidth = new int[TechEditorForm.PositionListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     OR必要技術リストビューの列の幅
            /// </summary>
            private int[] _orRequiredListColumnWidth = new int[TechEditorForm.RequiredListColumnCount];

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     技術座標リストビューの列の幅
            /// </summary>
            private int[] _techPositionListColumnWidth = new int[TechEditorForm.PositionListColumnCount];

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 1000;

            /// <summary>
            ///     デフォルト高さ(低解像度)
            /// </summary>
            private const int DefaultHeightShort = 670;

            /// <summary>
            ///     デフォルト高さ(中解像度)
            /// </summary>
            private const int DefaultHeightMiddle = 720;

            /// <summary>
            ///     デフォルト高さ(高解像度)
            /// </summary>
            private const int DefaultHeightLong = 876;

            /// <summary>
            ///     必要技術リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultRequiredListColumnWidth = {60, 235};

            /// <summary>
            ///     小研究リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultComponentListColumnWidth = {60, 250, 180, 60, 60};

            /// <summary>
            ///     技術効果リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultEffectListColumnWidth = {120, 120, 120, 120, 120};

            /// <summary>
            ///     座標リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultPositionListColumnWidth = {50, 50};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public TechEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int longHeight = DeviceCaps.GetScaledHeight(DefaultHeightLong);
                int middleHeight = DeviceCaps.GetScaledHeight(DefaultHeightMiddle);
                int shortHeight = DeviceCaps.GetScaledHeight(DefaultHeightShort);
                int height = (screenRect.Height >= longHeight)
                    ? longHeight
                    : (screenRect.Height >= middleHeight) ? middleHeight : shortHeight;
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // 必要技術リストビューの列の幅を設定する
                for (int i = 0; i < TechEditorForm.RequiredListColumnCount; i++)
                {
                    AndRequiredListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultRequiredListColumnWidth[i]);
                    OrRequiredListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultRequiredListColumnWidth[i]);
                }

                // 小研究リストビューの列の幅を設定する
                for (int i = 0; i < TechEditorForm.ComponentListColumnCount; i++)
                {
                    ComponentListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultComponentListColumnWidth[i]);
                }

                // 技術効果リストビューの列の幅を設定する
                for (int i = 0; i < TechEditorForm.EffectListColumnCount; i++)
                {
                    EffectListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultEffectListColumnWidth[i]);
                }

                // 座標リストビューの列の幅を設定する
                for (int i = 0; i < TechEditorForm.PositionListColumnCount; i++)
                {
                    TechPositionListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultPositionListColumnWidth[i]);
                    LabelPositionListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultPositionListColumnWidth[i]);
                    EventPositionListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultPositionListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeightShort, DefaultHeightLong);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region ユニットモデルエディタ

        /// <summary>
        ///     ユニットモデルエディタの設定
        /// </summary>
        public class UnitEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     ユニットモデルリストビューの列の幅
            /// </summary>
            public int[] ModelListColumnWidth
            {
                get { return _modelListColumnWidth; }
                set { _modelListColumnWidth = value; }
            }

            /// <summary>
            ///     改良リストビューの列の幅
            /// </summary>
            public int[] UpgradeListColumnWidth
            {
                get { return _upgradeListColumnWidth; }
                set { _upgradeListColumnWidth = value; }
            }

            /// <summary>
            ///     装備リストビューの列の幅
            /// </summary>
            public int[] EquipmentListColumnWidth
            {
                get { return _equipmentListColumnWidth; }
                set { _equipmentListColumnWidth = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     装備リストビューの列の幅
            /// </summary>
            private int[] _equipmentListColumnWidth = new int[UnitEditorForm.EquipmentListColumnCount];

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ユニットモデルリストビューの列の幅
            /// </summary>
            private int[] _modelListColumnWidth = new int[UnitEditorForm.ModelListColumnCount];

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     改良リストビューの列の幅
            /// </summary>
            private int[] _upgradeListColumnWidth = new int[UnitEditorForm.UpgradeListColumnCount];

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 1000;

            /// <summary>
            ///     デフォルト高さ(低解像度)
            /// </summary>
            private const int DefaultHeightShort = 670;

            /// <summary>
            ///     デフォルト高さ(高解像度)
            /// </summary>
            private const int DefaultHeightLong = 720;

            /// <summary>
            ///     ユニットモデルリストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultModelListColumnWidth = {40, 310, 50, 50, 50, 50, 50, 50, 50, 50};

            /// <summary>
            ///     改良リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultUpgradeListColumnWidth = {225, 40, 40};

            /// <summary>
            ///     装備リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultEquipmentListColumnWidth = {100, 60};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public UnitEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int longHeight = DeviceCaps.GetScaledHeight(DefaultHeightLong);
                int shortHeight = DeviceCaps.GetScaledHeight(DefaultHeightShort);
                int height = (screenRect.Height >= longHeight) ? longHeight : shortHeight;
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // ユニットモデルリストビューの列の幅を設定する
                for (int i = 0; i < UnitEditorForm.ModelListColumnCount; i++)
                {
                    ModelListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultModelListColumnWidth[i]);
                }

                // 改良リストビューの列の幅を設定する
                for (int i = 0; i < UnitEditorForm.UpgradeListColumnCount; i++)
                {
                    UpgradeListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultUpgradeListColumnWidth[i]);
                }

                // 装備リストビューの列の幅を設定する
                for (int i = 0; i < UnitEditorForm.EquipmentListColumnCount; i++)
                {
                    EquipmentListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultEquipmentListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeightShort, DefaultHeightLong);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region 基礎データエディタ

        /// <summary>
        ///     基礎データエディタの設定
        /// </summary>
        public class MiscEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     選択中のタブページ
            /// </summary>
            public int SelectedTab { get; set; }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 1000;

            /// <summary>
            ///     デフォルト高さ(低解像度)
            /// </summary>
            private const int DefaultHeightShort = 670;

            /// <summary>
            ///     デフォルト高さ(高解像度)
            /// </summary>
            private const int DefaultHeightLong = 720;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public MiscEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int longHeight = DeviceCaps.GetScaledHeight(DefaultHeightLong);
                int shortHeight = DeviceCaps.GetScaledHeight(DefaultHeightShort);
                int height = (screenRect.Height >= longHeight) ? longHeight : shortHeight;
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeightShort, DefaultHeightLong);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region 軍団名エディタ

        /// <summary>
        ///     軍団名エディタの設定
        /// </summary>
        public class CorpsNameEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     選択中の兵科
            /// </summary>
            public int Branch { get; set; }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public int Country { get; set; }

            /// <summary>
            ///     全ての兵科に適用するかどうか
            /// </summary>
            public bool ApplyAllBranches { get; set; }

            /// <summary>
            ///     全ての国家に適用するかどうか
            /// </summary>
            public bool ApplyAllCountires { get; set; }

            /// <summary>
            ///     正規表現を使用するかどうか
            /// </summary>
            public bool RegularExpression { get; set; }

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            public List<string> ToHistory
            {
                get { return _toHistory; }
                set { _toHistory = value; }
            }

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            public List<string> WithHistory
            {
                get { return _withHistory; }
                set { _withHistory = value; }
            }

            /// <summary>
            ///     接頭辞の履歴
            /// </summary>
            public List<string> PrefixHistory
            {
                get { return _prefixHistory; }
                set { _prefixHistory = value; }
            }

            /// <summary>
            ///     接尾辞の履歴
            /// </summary>
            public List<string> SuffixHistory
            {
                get { return _suffixHistory; }
                set { _suffixHistory = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     接頭辞の履歴
            /// </summary>
            private List<string> _prefixHistory = new List<string>();

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     接尾辞の履歴
            /// </summary>
            private List<string> _suffixHistory = new List<string>();

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            private List<string> _toHistory = new List<string>();

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            private List<string> _withHistory = new List<string>();

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 640;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 480;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public CorpsNameEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region ユニット名エディタ

        /// <summary>
        ///     ユニットモデル名エディタの設定
        /// </summary>
        public class ModelNameEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public int Country { get; set; }

            /// <summary>
            ///     選択中のユニット種類
            /// </summary>
            public int UnitType { get; set; }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 640;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 480;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public ModelNameEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region ユニットモデル名エディタ

        /// <summary>
        ///     ユニット名エディタの設定
        /// </summary>
        public class UnitNameEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public int Country { get; set; }

            /// <summary>
            ///     選択中のユニット種類
            /// </summary>
            public int UnitType { get; set; }

            /// <summary>
            ///     全ての国家に適用するかどうか
            /// </summary>
            public bool ApplyAllCountires { get; set; }

            /// <summary>
            ///     全てのユニット種類に適用するかどうか
            /// </summary>
            public bool ApplyAllUnitTypes { get; set; }

            /// <summary>
            ///     正規表現を使用するかどうか
            /// </summary>
            public bool RegularExpression { get; set; }

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            public List<string> ToHistory
            {
                get { return _toHistory; }
                set { _toHistory = value; }
            }

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            public List<string> WithHistory
            {
                get { return _withHistory; }
                set { _withHistory = value; }
            }

            /// <summary>
            ///     接頭辞の履歴
            /// </summary>
            public List<string> PrefixHistory
            {
                get { return _prefixHistory; }
                set { _prefixHistory = value; }
            }

            /// <summary>
            ///     接尾辞の履歴
            /// </summary>
            public List<string> SuffixHistory
            {
                get { return _suffixHistory; }
                set { _suffixHistory = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     接頭辞の履歴
            /// </summary>
            private List<string> _prefixHistory = new List<string>();

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     接尾辞の履歴
            /// </summary>
            private List<string> _suffixHistory = new List<string>();

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            private List<string> _toHistory = new List<string>();

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            private List<string> _withHistory = new List<string>();

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 640;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 480;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public UnitNameEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region ランダム指揮官名エディタ

        /// <summary>
        ///     ランダム指揮官名エディタの設定
        /// </summary>
        public class RandomLeaderEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public int Country { get; set; }

            /// <summary>
            ///     全ての国家に適用するかどうか
            /// </summary>
            public bool ApplyAllCountires { get; set; }

            /// <summary>
            ///     正規表現を使用するかどうか
            /// </summary>
            public bool RegularExpression { get; set; }

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            public List<string> ToHistory
            {
                get { return _toHistory; }
                set { _toHistory = value; }
            }

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            public List<string> WithHistory
            {
                get { return _withHistory; }
                set { _withHistory = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     置換元の履歴
            /// </summary>
            private List<string> _toHistory = new List<string>();

            /// <summary>
            ///     置換先の履歴
            /// </summary>
            private List<string> _withHistory = new List<string>();

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 640;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 480;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public RandomLeaderEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion

        #region 研究速度ビューア

        /// <summary>
        ///     研究速度ビューアの設定
        /// </summary>
        public class ResearchViewerSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            /// <summary>
            ///     技術リストビューの列の幅
            /// </summary>
            public int[] TechListColumnWidth
            {
                get { return _techListColumnWidth; }
                set { _techListColumnWidth = value; }
            }

            /// <summary>
            ///     研究機関リストビューの列の幅
            /// </summary>
            public int[] TeamListColumnWidth
            {
                get { return _teamListColumnWidth; }
                set { _teamListColumnWidth = value; }
            }

            /// <summary>
            ///     技術カテゴリリストボックスの選択項目
            /// </summary>
            public int Category { get; set; }

            /// <summary>
            ///     選択中の国家
            /// </summary>
            public List<Country> Countries
            {
                get { return _countries; }
                set { _countries = value; }
            }

            /// <summary>
            ///     指定日付を使用するかどうか
            /// </summary>
            public bool UseSpecifiedDate
            {
                get { return (Researches.DateMode == ResearchDateMode.Specified); }
                set { Researches.DateMode = value ? ResearchDateMode.Specified : ResearchDateMode.Historical; }
            }

            /// <summary>
            ///     指定日付
            /// </summary>
            public GameDate SpecifiedDate
            {
                get { return Researches.SpecifiedDate; }
                set { Researches.SpecifiedDate = value; }
            }

            /// <summary>
            ///     ロケット試験場の規模
            /// </summary>
            public int RocketTestingSites
            {
                get { return Researches.RocketTestingSites; }
                set { Researches.RocketTestingSites = value; }
            }

            /// <summary>
            ///     原子炉の規模
            /// </summary>
            public int NuclearReactors
            {
                get { return Researches.NuclearReactors; }
                set { Researches.NuclearReactors = value; }
            }

            /// <summary>
            ///     青写真の有無
            /// </summary>
            public bool Blueprint
            {
                get { return Researches.Blueprint; }
                set { Researches.Blueprint = value; }
            }

            /// <summary>
            ///     研究速度補正
            /// </summary>
            public string Modifier
            {
                get { return Researches.Modifier.ToString(CultureInfo.InvariantCulture); }
                set
                {
                    double d;
                    double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out d);
                    Researches.Modifier = d;

                    // 0以下の値だとまともに計算できなくなるので保険
                    if (Researches.Modifier <= 0)
                    {
                        Researches.Modifier = 1;
                    }
                }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     選択中の国家
            /// </summary>
            private List<Country> _countries = new List<Country>();

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            /// <summary>
            ///     研究機関リストビューの列の幅
            /// </summary>
            private int[] _teamListColumnWidth = new int[LeaderEditorForm.LeaderListColumnCount];

            /// <summary>
            ///     技術リストビューの列の幅
            /// </summary>
            private int[] _techListColumnWidth = new int[LeaderEditorForm.LeaderListColumnCount];

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 800;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 600;

            /// <summary>
            ///     技術リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultTechListColumnWidth = {310, 50, 50, 200};

            /// <summary>
            ///     研究機関リストビューの列のデフォルト幅
            /// </summary>
            private static readonly int[] DefaultTeamListColumnWidth = {0, 40, 50, 85, 200, 50, 45, 120};

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public ResearchViewerSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);

                // 技術リストビューの列の幅を設定する
                for (int i = 0; i < ResearchViewerForm.TechListColumnCount; i++)
                {
                    TechListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultTechListColumnWidth[i]);
                }

                // 研究機関リストビューの列の幅を設定する
                for (int i = 0; i < ResearchViewerForm.TeamListColumnCount; i++)
                {
                    TeamListColumnWidth[i] = DeviceCaps.GetScaledWidth(DefaultTeamListColumnWidth[i]);
                }
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);

                // 選択中の国家がない場合、先頭になるAFGを選択する
                if (_countries.Count == 0)
                {
                    _countries.Add(Country.AFG);
                }
            }

            #endregion
        }

        #endregion

        #region シナリオエディタ

        /// <summary>
        ///     シナリオエディタの設定
        /// </summary>
        public class ScenarioEditorSettings
        {
            #region 公開プロパティ

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            public Point Location
            {
                get { return _location; }
                set { _location = value; }
            }

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            public Size Size
            {
                get { return _size; }
                set { _size = value; }
            }

            #endregion

            #region 内部フィールド

            /// <summary>
            ///     ウィンドウ位置
            /// </summary>
            private Point _location;

            /// <summary>
            ///     ウィンドウサイズ
            /// </summary>
            private Size _size;

            #endregion

            #region 内部定数

            /// <summary>
            ///     デフォルト幅
            /// </summary>
            private const int DefaultWidth = 1000;

            /// <summary>
            ///     デフォルト高さ
            /// </summary>
            private const int DefaultHeight = 670;

            #endregion

            #region 初期化

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            public ScenarioEditorSettings()
            {
                Init();
            }

            /// <summary>
            ///     設定値を初期化する
            /// </summary>
            private void Init()
            {
                // デスクトップのサイズを取得する
                Rectangle screenRect = Screen.GetWorkingArea(new Point(200, 200));

                // ウィンドウ位置を設定する
                int width = DeviceCaps.GetScaledWidth(DefaultWidth);
                int height = DeviceCaps.GetScaledHeight(DefaultHeight);
                int x = screenRect.X + (screenRect.Width - width) / 2;
                int y = screenRect.Y + (screenRect.Height - height) / 2;
                Location = new Point(x, y);
                Size = new Size(width, height);
            }

            /// <summary>
            ///     設定値を丸める
            /// </summary>
            public void Round()
            {
                // ウィンドウ位置を丸める
                Rectangle rect = RoundFormPosition(Location, Size, DefaultWidth, DefaultHeight);
                Location = new Point(rect.X, rect.Y);
                Size = new Size(rect.Width, rect.Height);
            }

            #endregion
        }

        #endregion
    }
}