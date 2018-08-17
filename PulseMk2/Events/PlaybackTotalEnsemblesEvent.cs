using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Status message event.
    /// </summary>
    public class PlaybackTotalEnsemblesEvent
    {
        /// <summary>
        /// Total number of ensembles.
        /// </summary>
        public int NumEnsembles { get; set; }

        /// <summary>
        /// Initialize the event.
        /// </summary>
        /// <param name="num">Total number of ensembles.</param>
        public PlaybackTotalEnsemblesEvent(int num)
        {
            NumEnsembles = num;
        }

    }
}
