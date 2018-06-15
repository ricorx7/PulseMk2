using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    /// <summary>
    /// Screening options.
    /// </summary>
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
        public float RetransformHeadingOffset { get; set; }

        /// <summary>
        /// Retransform the data using this Water Profile Correlation Threshold.
        /// </summary>
        public float RetransformWpCorrThresh { get; set; }

        /// <summary>
        /// Retransform the data using this Bottom Track Correlation Threshold.
        /// </summary>
        public float RetransformBtCorrThresh { get; set; }

        /// <summary>
        /// Retransform the data using this Bottom Track SNR Threshold.
        /// </summary>
        public float RetransformBtSnrThresh { get; set; }

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
            RetransformHeadingOffset = 0.0f;
            RetransformWpCorrThresh = 0.250f;
            RetransformBtCorrThresh = 0.90f;
            RetransformBtSnrThresh = 10.0f;
        }
    }
}
