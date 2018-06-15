using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Layer to record the ensemble data.
    /// </summary>
    public interface IRecordEnsLayer
    {
        /// <summary>
        /// Receive the ensemble to be recorded.
        /// </summary>
        /// <param name="data">Binary data for the ensemble.</param>
        /// <param name="ensemble">Ensemble object.</param>
        /// <param name="source">Source of the ensemble.</param>
        /// <param name="dataFormat">Original data format.</param>
        /// <returns>Error code.</returns>
        int RecordEnsemble(byte[] data, DataSet.Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat);

    }
}
