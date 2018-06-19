using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    class Commons
    {
        /// <summary>
        /// Default recording directory.
        /// </summary>
        public const string DEFAULT_RECORD_DIR = @"C:\RTI_Capture";

        /// <summary>
        /// Max file to record.
        /// </summary>
        public const int MAX_FILE_SIZE = 16777216;      // 16mbs 

        /// <summary>
        /// Maximum number of configurations in CEPO.
        /// </summary>
        public const int MAX_CONFIGURATIONS = 12;

        /// <summary>
        /// Create a default folder path for the projects.
        /// This will be a folder in MyDocuments in a folder
        /// named RTI.
        /// </summary>
        /// <returns>Default folder path for projects.</returns>
        public static string GetProjectDefaultFolderPath()
        {
            string myDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return string.Format(@"{0}\RTI", myDoc);
        }
    }
}
