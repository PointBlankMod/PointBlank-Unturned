using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace PointBlank
{
    /// <summary>
    /// All information about the Unturned API
    /// </summary>
    public static class APIInfo
    {
        #region Private Info
        private static Assembly pbAssembly = Assembly.GetExecutingAssembly();
        private static FileVersionInfo pbFileVersionInfo = FileVersionInfo.GetVersionInfo(pbAssembly.Location);
        #endregion

        #region Public Info
        /// <summary>
        /// The name of the program
        /// </summary>
        public static readonly string Name = pbFileVersionInfo.ProductName;
        /// <summary>
        /// The creator of the program
        /// </summary>
        public static readonly string Creator = pbFileVersionInfo.CompanyName;
        /// <summary>
        /// The version of the program
        /// </summary>
        public static readonly string Version = pbFileVersionInfo.ProductVersion;
        /// <summary>
        /// The description of the program
        /// </summary>
        public static readonly string Description = pbFileVersionInfo.FileDescription;
        /// <summary>
        /// Is the program in debug release
        /// </summary>
        public static readonly bool IsDebug = pbFileVersionInfo.IsDebug;
        /// <summary>
        /// The directory of the program
        /// </summary>
        public static readonly string Location = Path.GetDirectoryName(Uri.UnescapeDataString((new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));
        #endregion
    }
}
