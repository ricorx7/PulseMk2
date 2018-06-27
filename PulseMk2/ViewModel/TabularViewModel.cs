using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public class TabularViewModel : PlotViewModel, IDisposable
    {
        #region Variables

        /// <summary>
        /// Store the latest ensemble.
        /// </summary>
        private DataSet.Ensemble _ensemble;

        #endregion

        #region Properties

        #region Tabular Data

        #region HPR

        /// <summary>
        /// Heading value.
        /// </summary>
        public string Heading
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Heading.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Heading.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Pitch value.
        /// </summary>
        public string Pitch
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Pitch.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Pitch.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Roll value.
        /// </summary>
        public string Roll
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Roll.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Roll.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Orientation of the ADCP value.
        /// </summary>
        public string Orientation
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    if (_ensemble.AncillaryData.IsUpwardFacing())
                    {
                        return "Upward Facing";
                    }
                    else
                    {
                        return "Downward Facing";
                    }
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    if (_ensemble.BottomTrackData.IsUpwardFacing())
                    {
                        return "Upward Facing";
                    }
                    else
                    {
                        return "Downward Facing";
                    }
                }

                return "";
            }
        }

        #endregion

        #region Environmental

        /// <summary>
        /// Water Temperature value.
        /// </summary>
        public string WaterTemp
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.WaterTemp.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.WaterTemp.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// System Temperature value.
        /// </summary>
        public string SystemTemp
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.SystemTemp.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.SystemTemp.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Pressure value.
        /// </summary>
        public string Pressure
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Pressure.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Pressure.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Transducer Depth value.
        /// </summary>
        public string TransducerDepth
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.TransducerDepth.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.TransducerDepth.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Salinity value.
        /// </summary>
        public string Salinity
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Salinity.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Salinity.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Speed of Sound value.
        /// </summary>
        public string SpeedOfSound
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.SpeedOfSound.ToString("0.00");
                }
                else if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.SpeedOfSound.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Voltage value.
        /// </summary>
        public string Voltage
        {
            get
            {
                if (_ensemble != null && _ensemble.IsSystemSetupAvail)
                {
                    return _ensemble.SystemSetupData.Voltage.ToString("0.00");
                }

                return "";
            }
        }

        #endregion

        #region Status

        /// <summary>
        /// System Status value.
        /// </summary>
        public string SystemStatus
        {
            get
            {
                if (_ensemble != null && _ensemble.IsEnsembleAvail)
                {
                    return _ensemble.EnsembleData.Status.ToString();
                }

                return "";
            }
        }

        /// <summary>
        /// Bottom Track Status value.
        /// </summary>
        public string BtStatus
        {
            get
            {
                if (_ensemble != null && _ensemble.IsBottomTrackAvail)
                {
                    return _ensemble.BottomTrackData.Status.ToString();
                }

                return "";
            }
        }

        #endregion

        #region Bins

        /// <summary>
        /// Blank value.
        /// </summary>
        public string FirstBinRange
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.FirstBinRange.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Bin Size value.
        /// </summary>
        public string BinSize
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.BinSize.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// Number of bins value.
        /// </summary>
        public string NumBins
        {
            get
            {
                if (_ensemble != null && _ensemble.IsEnsembleAvail)
                {
                    return _ensemble.EnsembleData.NumBins.ToString("0");
                }

                return "";
            }
        }

        /// <summary>
        /// Number of beams value.
        /// </summary>
        public string NumBeams
        {
            get
            {
                if (_ensemble != null && _ensemble.IsEnsembleAvail)
                {
                    return _ensemble.EnsembleData.NumBeams.ToString("0");
                }

                return "";
            }
        }

        #endregion

        #region Pings

        /// <summary>
        /// Desired Pings value.
        /// </summary>
        public string DesiredPings
        {
            get
            {
                if (_ensemble != null && _ensemble.IsEnsembleAvail)
                {
                    return _ensemble.EnsembleData.DesiredPingCount.ToString("0");
                }

                return "";
            }
        }

        /// <summary>
        /// Actual Pings value.
        /// </summary>
        public string ActualPings
        {
            get
            {
                if (_ensemble != null && _ensemble.IsEnsembleAvail)
                {
                    return _ensemble.EnsembleData.ActualPingCount.ToString("0");
                }

                return "";
            }
        }

        /// <summary>
        /// First Ping Time value.
        /// </summary>
        public string FirstPingTime
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.FirstPingTime.ToString("0.00");
                }

                return "";
            }
        }

        /// <summary>
        /// First Ping Time value.
        /// </summary>
        public string LastPingTime
        {
            get
            {
                if (_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.LastPingTime.ToString("0.00");
                }

                return "";
            }
        }

        #endregion

        #endregion

        #region Plots

        /// <summary>
        /// Compass Rose Plot.
        /// </summary>
        public CompassRoseViewModel CompassPlot { get; set; }

        /// <summary>
        /// Compass Rose Plot.
        /// </summary>
        public CompassRoseViewModel CompassPlotExpanded { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// View the tabular data.
        /// </summary>
        public TabularViewModel()
            : base("Tabular")
        {
            CompassPlot = IoC.Get<CompassRoseViewModel>();
        }

        /// <summary>
        /// Shutdown the view model
        /// </summary>
        public void Dispose()
        {
            CompassPlot.Dispose();
        }

        /// <summary>
        /// Process the latest data.
        /// </summary>
        /// <param name="ens"></param>
        public void ProcessData(DataSet.Ensemble ens)
        {
            // Set the latest ensemble
            _ensemble = ens;

            // Update the compass rose
            AddCompassData();

            // Update the display
            NotifyOfPropertyChange(null);
        }


        #region Compass Data

        /// <summary>
        /// Add compass data to the compass rose.
        /// </summary>
        private void AddCompassData()
        {
            if (_ensemble != null)
            {
                if (_ensemble.IsAncillaryAvail)
                {
                    CompassPlot.AddIncomingData(_ensemble.AncillaryData.Heading, _ensemble.AncillaryData.Pitch, _ensemble.AncillaryData.Roll);
                }
                else if (_ensemble.IsBottomTrackAvail)
                {
                    CompassPlot.AddIncomingData(_ensemble.BottomTrackData.Heading, _ensemble.BottomTrackData.Pitch, _ensemble.AncillaryData.Roll);
                }
            }
        }

        #endregion
    }
}
