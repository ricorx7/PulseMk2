using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Plaback a selected amount of ensembles.
    /// </summary>
    interface IPlaybackLayer
    {

        void Playback(int minIndex, int maxIndex);
    }
}
