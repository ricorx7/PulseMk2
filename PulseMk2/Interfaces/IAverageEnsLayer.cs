using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Layer to Average the ensemble data.
    /// </summary>
    public interface IAverageEnsLayer
    {
        /// <summary>
        /// Receive the ensemble to be averaged.  This data should already be screened.
        /// </summary>
        /// <param name="data">Binary data for the ensemble.</param>
        /// <param name="ensemble">Ensemble object.</param>
        /// <param name="source">Source of the ensemble.</param>
        /// <returns>Error code.</returns>
        int AverageEnsemble(byte[] data, DataSet.Ensemble ensemble, EnsembleSource source);

    }
}
