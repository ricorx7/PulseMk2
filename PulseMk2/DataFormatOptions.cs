using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    /// <summary>
    /// Store the Data Format options.
    /// </summary>
    public class DataFormatOptions
    {
        /// <summary>
        /// Rowe Tech Binary.
        /// </summary>
        public bool IsRTB { get; set; }

        /// <summary>
        /// Rowe Tech DVL.
        /// </summary>
        public bool IsRTD { get; set; }

        /// <summary>
        /// PD0.
        /// </summary>
        public bool IsPD0 { get; set; }

        /// <summary>
        /// PD6_13
        /// </summary>
        public bool IsPD6_13 { get; set; }

        /// <summary>
        /// PD4_5
        /// </summary>
        public bool IsPD4_5 { get; set; }

        /// <summary>
        /// Initialize.
        /// </summary>
        public DataFormatOptions()
        {
            IsRTB = true;
            IsRTD = true;
            IsPD0 = true;
            IsPD6_13 = true;
            IsPD4_5 = true;
        }
    }
}
