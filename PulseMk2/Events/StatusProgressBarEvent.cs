using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    /// <summary>
    /// Progress bar event in the status bar.
    /// </summary>
    public class StatusProgressBarEvent
    {
        /// <summary>
        /// Minimum status bar progress bar load.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Maximum progress status bar load.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Current value in the Progress bar.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Name of the file or status message.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initialize the progress bar value.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// <param name="name">File name or status message.</param>
        public StatusProgressBarEvent(double min, double max, double value, string name)
        {
            Min = min;
            Max = max;
            Value = value;
            Name = name;
        }

    }
}
