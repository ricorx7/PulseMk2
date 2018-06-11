using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTI.DataSet;

namespace RTI
{
    class ScreenEnsembleViewModel : IScreenEnsLayer
    {
        public class ScreenEnsembleOptions
        {
            /// <summary>
            /// Flag to remove the ship speed.
            /// </summary>
            public bool IsRemoveShipSpeed { get; set; }

            /// <summary>
            /// Use Bottom Track in remove ship speed.
            /// </summary>
            public bool IsRemoveShipSpeedBottomTrack { get; set; }

            /// <summary>
            /// Use GPS in remove ship speed.
            /// </summary>
            public bool IsRemoveShipSpeedGps { get; set; }

            /// <summary>
            /// Heading offset for removing the ship speed.
            /// </summary>
            public double RemoveShipSpeedHeadingOffset { get; set; }

            /// <summary>
            /// Flag to Mark Bad Below Bottom.
            /// </summary>
            public bool IsMarkBadBelowBottom { get; set; }

            /// <summary>
            /// Force a 3 Beam solution.
            /// </summary>
            public bool IsForce3BeamSolution { get; set; }

            /// <summary>
            /// Beam number to force 3 Beam solution bad.
            /// </summary>
            public int Force3BeamSolutionBeam { get; set; }

            /// <summary>
            /// Force a 3 beam solution in Bottom Track data.
            /// </summary>
            public bool IsForceBt3BeamSolution { get; set; }

            /// <summary>
            /// Beam number to force 3 beam solution bad in Bottom Track.
            /// </summary>
            public int ForceBt3BeamSolutionBeam { get; set; }

            /// <summary>
            /// Retransform Data the data.
            /// </summary>
            public bool IsRetransformData { get; set; }

            /// <summary>
            /// Heading source for retransforming the data.
            /// </summary>
            public Transform.HeadingSource RetransformHeadingSource { get; set; }

            /// <summary>
            /// Retransform the data using this heading offset.
            /// </summary>
            public double RetransformHeadingOffset { get; set; }

            /// <summary>
            /// Retransform the data using this Water Profile Correlation Threshold.
            /// </summary>
            public double RetransformWpCorrThresh { get; set; }

            /// <summary>
            /// Retransform the data using this Bottom Track Correlation Threshold.
            /// </summary>
            public double RetransformBtCorrThresh { get; set; }

            /// <summary>
            /// Retransform the data using this Bottom Track SNR Threshold.
            /// </summary>
            public double RetransformBtSnrThresh { get; set; }

            /// <summary>
            /// Initialize the data.
            /// </summary>
            public ScreenEnsembleOptions()
            {
                IsRemoveShipSpeed = true;
                IsRemoveShipSpeedBottomTrack = true;
                IsRemoveShipSpeedGps = true;
                RemoveShipSpeedHeadingOffset = 0.0;
                IsMarkBadBelowBottom = true;
                IsForce3BeamSolution = false;
                Force3BeamSolutionBeam = 0;
                IsForceBt3BeamSolution = false;
                ForceBt3BeamSolutionBeam = 0;
                IsRetransformData = false;
                RetransformHeadingSource = Transform.HeadingSource.ADCP;
                RetransformHeadingOffset = 0.0;
                RetransformWpCorrThresh = 0.250;
                RetransformBtCorrThresh = 0.90;
                RetransformBtSnrThresh = 10.0;
            }
        }


        /// <summary>
        /// Screen the ensemble.  Then pass it along to the average layer.
        /// </summary>
        /// <param name="data">Raw binary Ensemble.</param>
        /// <param name="ensemble">Ensemble object.</param>
        /// <param name="source">Source of the ensemble.</param>
        /// <param name="dataFormat">Format of the ensemble.</param>
        /// <returns>Negative number indicates an error.</returns>
        public int ScreenEnsemble(byte[] data, Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat)
        {
            throw new NotImplementedException();
        }
    }
}
