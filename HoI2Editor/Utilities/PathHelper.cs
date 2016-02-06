using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HoI2Editor.Utilities
{
    /// <summary>
    ///     パス操作のヘルパークラス
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        ///     パス名の最大サイズ
        /// </summary>
        private const int MaxPath = 260;

        /// <summary>
        ///     パスの区切り文字
        /// </summary>
        private static readonly char[] PathSeparator = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        /// <summary>
        ///     相対パス名を取得する
        /// </summary>
        /// <param name="pathName">対象パス名</param>
        /// <param name="baseDirName">基準ディレクトリ名</param>
        /// <returns>相対パス名</returns>
        public static string GetRelativePathName(string pathName, string baseDirName)
        {
            string[] targets = pathName.Split(PathSeparator);
            string[] bases = baseDirName.Split(PathSeparator);
            if ((bases.Length == 0) || (targets.Length < bases.Length))
            {
                return "";
            }
            if (bases.Where((t, i) => !targets[i].Equals(t)).Any())
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(MaxPath);
            bool result = NativeMethods.PathRelativePathTo(sb, baseDirName, FileAttributes.Directory, pathName,
                FileAttributes.Normal);
            if (!result)
            {
                return "";
            }
            string s = sb.ToString(2, sb.Length - 2);
            s = s.Replace('\\', '/');
            return s;
        }

        /// <summary>
        ///     P/Invokeメソッド定義用クラス
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            ///     PathRelativePathTo Win32API
            /// </summary>
            /// <param name="pszPath"></param>
            /// <param name="pszFrom"></param>
            /// <param name="dwAttrFrom"></param>
            /// <param name="pszTo"></param>
            /// <param name="dwAttrTo"></param>
            /// <returns></returns>
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern bool PathRelativePathTo(StringBuilder pszPath, string pszFrom,
                FileAttributes dwAttrFrom, string pszTo, FileAttributes dwAttrTo);
        }
    }
}