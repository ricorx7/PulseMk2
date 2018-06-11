using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Source of the data for the ADCP.
    /// </summary>
    public enum EnsembleSource
    {
        /// <summary>
        /// Playback from a file.
        /// </summary>
        Playback,

        /// <summary>
        /// Data from the serial port.
        /// </summary>
        Serial,

        /// <summary>
        /// Data from the Ethernet port.
        /// </summary>
        Ethernet,

        /// <summary>
        /// Data from the UDP port.
        /// </summary>
        UDP,

        /// <summary>
        /// Long term average.
        /// </summary>
        LTA,

        /// <summary>
        /// Short term average.
        /// </summary>
        STA
    }

    /// <summary>
    /// Codec Layer interface.
    /// </summary>
    public interface ICodecLayer
    {
        /// <summary>
        /// Add Raw ensemble data from the ADCP.
        /// This data will be passed to the codecs for processing of ensembles.
        /// </summary>
        /// <param name="data">Data from the ADCP.</param>
        /// <param name="source">Source of the data.</param>
        /// <returns>Error code.</returns>
        int AddData(byte[] data, EnsembleSource source);

        /// <summary>
        /// Add Raw NMEA data from a GPS.  
        /// This data will be combined with the ADCP data.
        /// </summary>
        /// <param name="data">GPS NMEA data.</param>
        /// <param name="source">Source of the data.</param>
        /// <returns>Error code.</returns>
        int AddNmeaData(byte[] data, EnsembleSource source);

    }
}
