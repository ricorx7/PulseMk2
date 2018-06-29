using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region Class

        #region Data Grid Data

        /// <summary>
        /// Object to stor the data for the data grid.
        /// </summary>
        public class DataGridData
        {
            /// <summary>
            /// Bin number.
            /// </summary>
            public int Bin { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Beam0 { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Beam1 { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Beam2 { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Beam3 { get; set; }

            /// <summary>
            /// Set the data.
            /// </summary>
            /// <param name="bin">Bin number.</param>
            /// <param name="beam0">Beam 0 data.</param>
            /// <param name="beam1">Beam 1 data.</param>
            /// <param name="beam2">Beam 2 data.</param>
            /// <param name="beam3">Beam 3 data.</param>
            public DataGridData(int bin, double beam0, double beam1, double beam2, double beam3)
            {
                Bin = bin;
                Beam0 = beam0;
                Beam1 = beam1;
                Beam2 = beam2;
                Beam3 = beam3;
            }
        }

        #endregion

        #region VelocityVector Data Grid Data

        /// <summary>
        /// Object to stor the data for the data grid.
        /// </summary>
        public class VvDataGridData
        {
            /// <summary>
            /// Bin number.
            /// </summary>
            public int Bin { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Magnitude { get; set; }

            /// <summary>
            /// Beam 0 Data
            /// </summary>
            public double Direction { get; set; }

            /// <summary>
            /// Set the data.
            /// </summary>
            /// <param name="bin">Bin number.</param>
            /// <param name="mag">Magnitude data.</param>
            /// <param name="dir">Direction data.</param>
            public VvDataGridData(int bin, double mag, double dir)
            {
                Bin = bin;
                Magnitude = mag;
                Direction = dir;
            }
        }

        #endregion

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

        #region Velocity Data

        /// <summary>
        /// Earth velocity data.
        /// </summary>
        public ObservableCollection<DataGridData> EarthVelocity { get; set; }

        /// <summary>
        /// Instrument velocity data.
        /// </summary>
        public ObservableCollection<DataGridData> InstrumentVelocity { get; set; }

        /// <summary>
        /// Beam velocity data.
        /// </summary>
        public ObservableCollection<DataGridData> BeamVelocity { get; set; }

        /// <summary>
        /// Velocity vector data.
        /// </summary>
        public ObservableCollection<VvDataGridData> VelocityVector { get; set; }

        /// <summary>
        /// Amplitude data.
        /// </summary>
        public ObservableCollection<DataGridData> Amplitude { get; set; }

        /// <summary>
        /// Correlation data.
        /// </summary>
        public ObservableCollection<DataGridData> Correlation { get; set; }


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

            EarthVelocity = new ObservableCollection<DataGridData>();
            InstrumentVelocity = new ObservableCollection<DataGridData>();
            BeamVelocity = new ObservableCollection<DataGridData>();
            VelocityVector = new ObservableCollection<VvDataGridData>();
            Amplitude = new ObservableCollection<DataGridData>();
            Correlation = new ObservableCollection<DataGridData>();
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
        /// <param name="ens">Ensemble data</param>
        public void ProcessData(DataSet.Ensemble ens)
        {
            // Set the latest ensemble
            _ensemble = ens;

            // Update the compass rose
            AddCompassData();

            // Process the Earth velocity data
            if (ens.IsEarthVelocityAvail)
            {
                EarthVelocity = ProcessVelocityData(ens.EarthVelocityData.EarthVelocityData);
            }

            // Process the Instrument velocity data
            if (ens.IsInstrumentVelocityAvail)
            {
                InstrumentVelocity = ProcessVelocityData(ens.InstrumentVelocityData.InstrumentVelocityData);
            }

            // Process the Beam velocity data
            if (ens.IsBeamVelocityAvail)
            {
                BeamVelocity = ProcessVelocityData(ens.BeamVelocityData.BeamVelocityData);
            }

            // Process the Amplitude data
            if (ens.IsAmplitudeAvail)
            {
                Amplitude = ProcessVelocityData(ens.AmplitudeData.AmplitudeData);
            }

            // Process the Correlation data
            if (ens.IsCorrelationAvail)
            {
                Correlation = ProcessVelocityData(ens.CorrelationData.CorrelationData);
            }

            // Process the Velocity Vectors data
            if (ens.IsEarthVelocityAvail && ens.EarthVelocityData.IsVelocityVectorAvail)
            {
                VelocityVector = ProcessVelocityVectorData(ens.EarthVelocityData.VelocityVectors);
            }

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

        #region Process Velocity data

        /// <summary>
        /// Process the velocity vector data.
        /// </summary>
        /// <param name="data">Datat to process.</param>
        /// <returns>List of velocity vectors.</returns>
        private ObservableCollection<DataGridData> ProcessVelocityData(float[,] data)
        {
            int beams = data.GetLength(1);
            int bins = data.GetLength(0);

            ObservableCollection<DataGridData> list = new ObservableCollection<DataGridData>();

            for (int bin = 0; bin < bins; bin++)
            {
                double beam0 = 0.0f;
                double beam1 = 0.0f;
                double beam2 = 0.0f;
                double beam3 = 0.0f;

                for (int beam = 0; beam < beams; beam++)
                {
                    // Beam 0 Data
                    if (beam == 0)
                    {
                        beam0 = Math.Round(data[bin, beam], 3);
                    }

                    // Beam 1 Data
                    if (beam == 1)
                    {
                        beam1 = Math.Round(data[bin, beam], 3);
                    }

                    // Beam 2 Data
                    if (beam == 2)
                    {
                        beam2 = Math.Round(data[bin, beam], 3);
                    }

                    // Beam 3 Data
                    if (beam == 3)
                    {
                        beam3 = Math.Round(data[bin, beam], 3);
                    }
                }

                // Create the data
                DataGridData binData = new DataGridData(bin, beam0, beam1, beam2, beam3);

                // Add the data to the list
                list.Add(binData);
            }

            return list;
        }

        /// <summary>
        /// Process the velocity vector data.
        /// </summary>
        /// <param name="data">Datat to process.</param>
        /// <returns>List of velocity vectors.</returns>
        private ObservableCollection<VvDataGridData> ProcessVelocityVectorData(DataSet.VelocityVector[] data)
        {
            ObservableCollection<VvDataGridData> list = new ObservableCollection<VvDataGridData>();

            for (int bin = 0; bin < data.Length; bin++)
            {
                // Add the data to the list
                list.Add(new VvDataGridData(bin,
                                            Math.Round(data[bin].Magnitude, 3),
                                            Math.Round(data[bin].DirectionYNorth, 3)));
            }

            return list;
        }

        #endregion
    }
}
