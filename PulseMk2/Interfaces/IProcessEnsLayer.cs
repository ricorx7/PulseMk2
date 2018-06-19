using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Type of ensemble.
    /// Single ensemble or averaged
    /// ensemble.
    /// </summary>
    public enum EnsembleType
    {
        /// <summary>
        /// A single ensemble.
        /// </summary>
        Single,

        /// <summary>
        /// Short term averaged ensemble.  This ensemble has
        /// multiple ensembles averaged together.
        /// </summary>
        STA,

        /// <summary>
        /// Long term averaged ensemble.  This ensemble has
        /// multiple ensembles averaged together.
        /// </summary>
        LTA
    }

    /// <summary>
    /// Pass Processed Ensemble data.
    /// </summary>
    interface IProcessEnsLayer
    {

        /// <summary>
        /// The ensemble has been processed for Screening and averaging the data.  Now 
        /// any plots or displays that need the processed data, will use this method.
        /// </summary>
        /// <param name="ensemble">Ensemble data.</param>
        /// <param name="source">Source of the ensemble.</param>
        /// <param name="type">Type of ensemble.</param>
        /// <param name="dataFormat">Data Format.</param>
        /// <returns>Negative number indicates an error.</returns>
        ViewSubsystemConfig ProcessEnsemble(DataSet.Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat);

    }
}
