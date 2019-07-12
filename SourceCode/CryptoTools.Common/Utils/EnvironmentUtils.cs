using System.IO;
using System.Reflection;

namespace CryptoTools.Common.Utils
{
	/// <summary>
	/// Environment related helper and utility methods to make client code more readable.
	/// </summary>
	public class EnvironmentUtils
    {
        /// <summary>
        /// Returns the Folder Path of the executing assembly.
        /// </summary>
        public static string ExecutingFolderPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }


        /// <summary>
        /// Returns the physical application root for a web application or the executing folder for a windows app. 
        /// 
        /// </summary>
        public static string ApplicationBaseFolder
        {
            get
            {
                // This needs testing
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                return path;
            }
        }
    }
}
