using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RTI.DataSet;

namespace RTI
{
    class ScreenEnsembleViewModel : Caliburn.Micro.Screen, IScreenEnsLayer
    {
        #region Variables

        /// <summary>
        /// Options for screening.
        /// </summary>
        private ScreenEnsembleOptions _Options;

        /// <summary>
        /// Previous Good Bottom Track East velocity.
        /// </summary>
        private float _prevBtEast = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Good Bottom Track North velocity.
        /// </summary>
        private float _prevBtNorth = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Good Bottom Track Vertical velocity.
        /// </summary>
        private float _prevBtVert = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed X.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedX = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed Y.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedY = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed Z.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedZ = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed Transverse.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedTransverse = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed Longitundial.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedLongitudinal = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous Ship Speed Normal.  Used to remove ship speed from Velocity data.
        /// </summary>
        private float _prevShipSpeedNormal = DataSet.Ensemble.BAD_VELOCITY;

        /// <summary>
        /// Previous good Bottom Track range.
        /// </summary>
        private float _prevBtRange = DataSet.Ensemble.BAD_RANGE;

        /// <summary>
        /// Previous heading value to use as a backup.
        /// </summary>
        private float _prevHeading;

        #endregion

        #region Properties

        /// <summary>
        /// Flag to remove the ship speed.
        /// </summary>
        public bool IsRemoveShipSpeed
        {
            get { return _Options.IsRemoveShipSpeed; }
            set
            {
                _Options.IsRemoveShipSpeed = value;
                NotifyOfPropertyChange(() => IsRemoveShipSpeed);
            }
        }

        /// <summary>
        /// Use Bottom Track in remove ship speed.
        /// </summary>
        public bool IsRemoveShipSpeedBottomTrack
        {
            get { return _Options.IsRemoveShipSpeedBottomTrack; }
            set
            {
                _Options.IsRemoveShipSpeedBottomTrack = value;
                NotifyOfPropertyChange(() => IsRemoveShipSpeedBottomTrack);
            }
        }

        /// <summary>
        /// Use GPS in remove ship speed.
        /// </summary>
        public bool IsRemoveShipSpeedGps
        {
            get { return _Options.IsRemoveShipSpeedGps; }
            set
            {
                _Options.IsRemoveShipSpeedGps = value;
                NotifyOfPropertyChange(() => IsRemoveShipSpeedGps);
            }
        }

        /// <summary>
        /// Heading offset for removing the ship speed.
        /// </summary>
        public double RemoveShipSpeedHeadingOffset
        {
            get { return _Options.RemoveShipSpeedHeadingOffset; }
            set
            {
                _Options.RemoveShipSpeedHeadingOffset = value;
                NotifyOfPropertyChange(() => RemoveShipSpeedHeadingOffset);
            }
        }

        /// <summary>
        /// Flag to Mark Bad Below Bottom.
        /// </summary>
        public bool IsMarkBadBelowBottom
        {
            get { return _Options.IsMarkBadBelowBottom; }
            set
            {
                _Options.IsMarkBadBelowBottom = value;
                NotifyOfPropertyChange(() => IsMarkBadBelowBottom);
            }
        }

        /// <summary>
        /// Force a 3 Beam solution.
        /// </summary>
        public bool IsForce3BeamSolution
        {
            get { return _Options.IsForce3BeamSolution; }
            set
            {
                _Options.IsForce3BeamSolution = value;
                NotifyOfPropertyChange(() => IsForce3BeamSolution);
            }
        }

        /// <summary>
        /// Beam number to force 3 Beam solution bad.
        /// </summary>
        public int Force3BeamSolutionBeam
        {
            get { return _Options.Force3BeamSolutionBeam; }
            set
            {
                _Options.Force3BeamSolutionBeam = value;
                NotifyOfPropertyChange(() => Force3BeamSolutionBeam);
            }
        }

        /// <summary>
        /// Force a 3 beam solution in Bottom Track data.
        /// </summary>
        public bool IsForceBt3BeamSolution
        {
            get { return _Options.IsForceBt3BeamSolution; }
            set
            {
                _Options.IsForceBt3BeamSolution = value;
                NotifyOfPropertyChange(() => IsForceBt3BeamSolution);
            }
        }

        /// <summary>
        /// Beam number to force 3 beam solution bad in Bottom Track.
        /// </summary>
        public int ForceBt3BeamSolutionBeam
        {
            get { return _Options.ForceBt3BeamSolutionBeam; }
            set
            {
                _Options.ForceBt3BeamSolutionBeam = value;
                NotifyOfPropertyChange(() => ForceBt3BeamSolutionBeam);
            }
        }

        /// <summary>
        /// Retransform Data the data.
        /// </summary>
        public bool IsRetransformData
        {
            get { return _Options.IsRetransformData; }
            set
            {
                _Options.IsRetransformData = value;
                NotifyOfPropertyChange(() => IsRetransformData);
            }
        }

        /// <summary>
        /// Heading source for retransforming the data.
        /// </summary>
        public Transform.HeadingSource RetransformHeadingSource
        {
            get { return _Options.RetransformHeadingSource; }
            set
            {
                _Options.RetransformHeadingSource = value;
                NotifyOfPropertyChange(() => RetransformHeadingSource);
            }
        }

        /// <summary>
        /// Retransform the data using this heading offset.
        /// </summary>
        public float RetransformHeadingOffset
        {
            get { return _Options.RetransformHeadingOffset; }
            set
            {
                _Options.RetransformHeadingOffset = value;
                NotifyOfPropertyChange(() => RetransformHeadingOffset);
            }
        }

        /// <summary>
        /// Retransform the data using this Water Profile Correlation Threshold.
        /// </summary>
        public float RetransformWpCorrThresh
        {
            get { return _Options.RetransformWpCorrThresh; }
            set
            {
                _Options.RetransformWpCorrThresh = value;
                NotifyOfPropertyChange(() => RetransformWpCorrThresh);
            }
        }

        /// <summary>
        /// Retransform the data using this Bottom Track Correlation Threshold.
        /// </summary>
        public float RetransformBtCorrThresh
        {
            get { return _Options.RetransformBtCorrThresh; }
            set
            {
                _Options.RetransformBtCorrThresh = value;
                NotifyOfPropertyChange(() => RetransformBtCorrThresh);
            }
        }

        /// <summary>
        /// Retransform the data using this Bottom Track SNR Threshold.
        /// </summary>
        public float RetransformBtSnrThresh
        {
            get { return _Options.RetransformBtSnrThresh; }
            set
            {
                _Options.RetransformBtSnrThresh = value;
                NotifyOfPropertyChange(() => RetransformBtSnrThresh);
            }
        }

        #endregion

        /// <summary>
        /// Initialize.
        /// </summary>
        public ScreenEnsembleViewModel()
        {
            _Options = new ScreenEnsembleOptions();
        }

        /// <summary>
        /// Initialize the options.
        /// </summary>
        /// <param name="options">Screening options.</param>
        public ScreenEnsembleViewModel(ScreenEnsembleOptions options)
        {
            _Options = options;
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
            // Fill in for missing data if only Bottom Track is turned on
            FillInMissingWpData(ref ensemble);

            // Screen for bad heading
            ScreenData.ScreenBadHeading.Screen(ref ensemble, _prevHeading);
            SetPreviousHeading(ensemble);

            // Force 3 Beam Solution
            if (_Options.IsForce3BeamSolution)
            {
                ScreenData.ScreenForce3BeamSolution.Force3BeamSolution(ref ensemble, _Options.Force3BeamSolutionBeam, dataFormat);
            }

            // Force 3 Beam Bottom Track solution
            if (_Options.IsForceBt3BeamSolution)
            {
                ScreenData.ScreenForce3BeamSolution.Force3BottomTrackBeamSolution(ref ensemble, _Options.ForceBt3BeamSolutionBeam, dataFormat);
            }

            // Retransform the data
            if (_Options.IsRetransformData)
            {
                // PD0 has a different cooridiate matrix
                // And the beams are in different positions
                Transform.ProfileTransform(ref ensemble, dataFormat, _Options.RetransformWpCorrThresh, _Options.RetransformHeadingSource, _Options.RetransformHeadingOffset);
                Transform.BottomTrackTransform(ref ensemble, dataFormat, _Options.RetransformBtCorrThresh, _Options.RetransformBtSnrThresh, _Options.RetransformHeadingSource, _Options.RetransformHeadingOffset);

                // WaterMass transform data
                // This will also create the ship data
                if (ensemble.IsInstrumentWaterMassAvail)
                {
                    Transform.WaterMassTransform(ref ensemble, dataFormat, _Options.RetransformBtCorrThresh, _Options.RetransformBtSnrThresh, _Options.RetransformHeadingSource, _Options.RetransformHeadingOffset, 0.0f);
                }
            }

            // Mark Bad Below Bottom
            if (_Options.IsMarkBadBelowBottom)
            {
                ScreenData.ScreenMarkBadBelowBottom.Screen(ref ensemble, _prevBtRange);
            }

            // Remove Ship Speed
            if (_Options.IsRemoveShipSpeed)
            {
                ScreenData.RemoveShipSpeed.RemoveVelocity(ref ensemble, _prevBtEast, _prevBtNorth, _prevBtVert, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps, _Options.RemoveShipSpeedHeadingOffset);
                ScreenData.RemoveShipSpeed.RemoveVelocityInstrument(ref ensemble, _prevShipSpeedX, _prevShipSpeedY, _prevShipSpeedZ, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps, _Options.RemoveShipSpeedHeadingOffset);
                ScreenData.RemoveShipSpeed.RemoveVelocityShip(ref ensemble, _prevShipSpeedTransverse, _prevShipSpeedLongitudinal, _prevShipSpeedNormal, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps, _Options.RemoveShipSpeedHeadingOffset);

                // Create the new velocity vectors based off the new data
                DataSet.VelocityVectorHelper.CreateVelocityVector(ref ensemble);
            }

            // Record the previous ship speed values
            SetPreviousShipSpeed(ensemble);

            // Pass the ensemble to the Processed Ensemble Layer
            foreach (var vm in IoC.GetAllInstances(typeof(IProcessEnsLayer)))
            {
                ((IRecordEnsLayer)vm).RecordEnsemble(data, ensemble, EnsembleSource.Serial, dataFormat);
            }

            return 0;
        }

        #region Remove Ship Speed

        /// <summary>
        /// Store the previous Ship speed for the different velocity coordinate transforms.
        /// </summary>
        /// <param name="ens">Ensembles.</param>
        private void SetPreviousShipSpeed(DataSet.Ensemble ens)
        {
            // EARTH
            // Record the Bottom for previous values
            float[] prevShipSpeed = ScreenData.RemoveShipSpeed.GetPreviousShipSpeed(ens, (float)_Options.RemoveShipSpeedHeadingOffset, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps);
            _prevBtEast = prevShipSpeed[0];
            _prevBtNorth = prevShipSpeed[1];
            _prevBtVert = prevShipSpeed[2];

            // Instrument
            // Record the Bottom for previous values
            float[] prevShipSpeedInstrument = ScreenData.RemoveShipSpeed.GetPreviousShipSpeedInstrument(ens, (float)_Options.RemoveShipSpeedHeadingOffset, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps);
            _prevShipSpeedX = prevShipSpeedInstrument[0];
            _prevShipSpeedY = prevShipSpeedInstrument[1];
            _prevShipSpeedZ = prevShipSpeedInstrument[2];

            // Ship
            // Record the Bottom for previous values
            float[] prevShipSpeedShip = ScreenData.RemoveShipSpeed.GetPreviousShipSpeedShip(ens, (float)_Options.RemoveShipSpeedHeadingOffset, _Options.IsRemoveShipSpeedBottomTrack, _Options.IsRemoveShipSpeedGps);
            _prevShipSpeedTransverse = prevShipSpeedShip[0];
            _prevShipSpeedLongitudinal = prevShipSpeedShip[1];
            _prevShipSpeedNormal = prevShipSpeedShip[2];

            if (ens.IsBottomTrackAvail)
            {
                float range = ens.BottomTrackData.GetAverageRange();
                if (range != DataSet.Ensemble.BAD_RANGE)
                {
                    _prevBtRange = range;
                }
            }

        }

        #endregion

        #region Previous Heading

        /// <summary>
        /// Set the previous heading so we can always have the last good heading.
        /// </summary>
        /// <param name="ensemble">Ensemble to get last good heading.</param>
        private void SetPreviousHeading(DataSet.Ensemble ensemble)
        {
            if (ensemble.IsAncillaryAvail && ensemble.AncillaryData.Heading != 0.0f)
            {
                _prevHeading = ensemble.AncillaryData.Heading;
            }
        }

        #endregion

        #region Missing Water Profile

        /// <summary>
        /// Fill in missing values if water profile is turned off.  If it is turned off, it sets these values to 0 but
        /// Bottom Track has the values.
        /// </summary>
        /// <param name="ensemble">Ensemble to get the data.</param>
        public void FillInMissingWpData(ref DataSet.Ensemble ensemble)
        {
            if (ensemble.IsBottomTrackAvail && ensemble.IsEnsembleAvail)
            {
                // If these values are zero, then most likely WP is turned off
                // Fill in the data
                if (ensemble.EnsembleData.NumBeams == 0 && ensemble.EnsembleData.NumBins == 0 && ensemble.BottomTrackData.NumBeams != ensemble.EnsembleData.NumBeams)
                {
                    ensemble.EnsembleData.NumBeams = (int)ensemble.BottomTrackData.NumBeams;

                    if (ensemble.IsAncillaryAvail)
                    {
                        ensemble.AncillaryData.Heading = ensemble.BottomTrackData.Heading;
                        ensemble.AncillaryData.Pitch = ensemble.BottomTrackData.Pitch;
                        ensemble.AncillaryData.Roll = ensemble.BottomTrackData.Roll;
                        ensemble.AncillaryData.WaterTemp = ensemble.BottomTrackData.WaterTemp;
                        ensemble.AncillaryData.SystemTemp = ensemble.BottomTrackData.SystemTemp;
                        ensemble.AncillaryData.Salinity = ensemble.BottomTrackData.Salinity;
                        ensemble.AncillaryData.Pressure = ensemble.BottomTrackData.Pressure;
                        ensemble.AncillaryData.TransducerDepth = ensemble.BottomTrackData.TransducerDepth;
                        ensemble.AncillaryData.SpeedOfSound = ensemble.BottomTrackData.SpeedOfSound;
                        ensemble.AncillaryData.FirstPingTime = ensemble.BottomTrackData.FirstPingTime;
                        ensemble.AncillaryData.LastPingTime = ensemble.BottomTrackData.LastPingTime;
                    }
                }
            }
        }

        #endregion
    }
}
