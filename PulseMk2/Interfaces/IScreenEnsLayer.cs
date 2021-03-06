﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{

    /// <summary>
    /// Layer to screen the ensemble data.
    /// </summary>
    public interface IScreenEnsLayer
    {
        /// <summary>
        /// Receive the ensemble data to be screened.
        /// </summary>
        /// <param name="data">Binary data for the ensemble.</param>
        /// <param name="ensemble">Ensemble object.</param>
        /// <param name="source">Source of the ensemble.</param>
        /// <param name="dataFormat">Original data format the codec parsed.</param>
        /// <returns>Error code.</returns>
        int ScreenEnsemble(byte[] data, DataSet.Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat);

    }
}
