using Caliburn.Micro;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RTI
{
    public class DashboardSubsystemConfigViewModel : Caliburn.Micro.Screen, IDisposable
    {
        #region Variables

        /// <summary>
        /// Latest ensemble.
        /// </summary>
        private DataSet.Ensemble _ensemble;

        /// <summary>
        /// Windows manager.
        /// </summary>
        private IWindowManager _windowMgr;

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

        /// <summary>
        /// Subsystem configuration.
        /// </summary>
        public ViewSubsystemConfig Config { get; set; }

        #region Ensemble Info

        /// <summary>
        /// Header for the Subsystem.
        /// </summary>
        public string Header
        {
            get
            {
                return Config.Config.IndexCodeString();
            }
        }

        /// <summary>
        /// Subsystem Description.
        /// </summary>
        public string SubsystemDesc
        {
            get
            {
                return Config.Config.SubSystem.DescString();
            }
        }

        /// <summary>
        /// Source of the ensemble
        /// </summary>
        public string EnsembleSource
        {
            get
            {
                switch(Config.Source)
                {
                    case RTI.EnsembleSource.Playback:
                        return "Playback file";
                    case RTI.EnsembleSource.Ethernet:
                        return "Ethernet";
                    case RTI.EnsembleSource.UDP:
                        return "UDP";
                    case RTI.EnsembleSource.STA:
                        return "STA";
                    case RTI.EnsembleSource.LTA:
                        return "LTA";
                    case RTI.EnsembleSource.Serial:
                    default:
                        return "Serial";
                }
            }
        }

        /// <summary>
        /// Ensemble number.
        /// </summary>
        public string EnsembleNumber
        {
            get
            {
                if (_ensemble != null)
                {
                    if (_ensemble.IsEnsembleAvail)
                    {
                        return string.Format("{0}", _ensemble.EnsembleData.EnsembleNumber.ToString("0"));
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Ensemble Date and Time.
        /// </summary>
        public string DateAndTime
        {
            get
            {
                if (_ensemble != null)
                {
                    if (_ensemble.IsEnsembleAvail)
                    {
                        return string.Format("{0}", _ensemble.EnsembleData.EnsDateTime.ToString());
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Serial Number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                if (_ensemble != null)
                {
                    if (_ensemble.IsEnsembleAvail)
                    {
                        return string.Format("{0}", _ensemble.EnsembleData.SysSerialNumber.ToString());
                    }
                }

                return "";
            }
        }

        #endregion

        #region Average Data

        /// <summary>
        /// Average depth.
        /// </summary>
        public string AverageDepth
        {
            get
            {
                if(_ensemble != null)
                {
                    if(_ensemble.IsBottomTrackAvail)
                    {
                        double depth = _ensemble.BottomTrackData.GetAverageRange();

                        return string.Format("{0} m  ", depth.ToString("0.00"));
                    }
                    else
                    {
                        return "No Bottom Track";
                    }
                }

                return "";
            }
        }


        /// <summary>
        /// Average water direction.
        /// </summary>
        private string _AvgWaterDir;
        /// <summary>
        /// Average water direction.
        /// </summary>
        public string AvgWaterDir
        {
            get { return _AvgWaterDir; }
            set
            {
                _AvgWaterDir = value;
                NotifyOfPropertyChange(() => AvgWaterDir);
            }
        }

        /// <summary>
        /// Average water velocity.
        /// </summary>
        private string _AvgWaterVel;
        /// <summary>
        /// Average water velocity.
        /// </summary>
        public string AvgWaterVel
        {
            get { return _AvgWaterVel; }
            set
            {
                _AvgWaterVel = value;
                NotifyOfPropertyChange(() => AvgWaterVel);
            }
        }

        /// <summary>
        /// Average Bottom Track boat speed.
        /// </summary>
        private string _AvgBtSpeed;
        /// <summary>
        /// Average Bottom Track boat speed.
        /// </summary>
        public string AvgBtSpeed
        {
            get { return _AvgBtSpeed; }
            set
            {
                _AvgBtSpeed = value;
                NotifyOfPropertyChange(() => AvgBtSpeed);
            }
        }

        /// <summary>
        /// Average GPS speed.
        /// </summary>
        private string _GpsSpeed;
        /// <summary>
        /// Average GPS speed.
        /// </summary>
        public string GpsSpeed
        {
            get { return _GpsSpeed; }
            set
            {
                _GpsSpeed = value;
                NotifyOfPropertyChange(() => GpsSpeed);
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

        #region Tabular Data

        #region HPR

        /// <summary>
        /// Heading value.
        /// </summary>
        public string Heading
        {
            get
            {
                if(_ensemble != null && _ensemble.IsAncillaryAvail)
                {
                    return _ensemble.AncillaryData.Heading.ToString("0.00");
                }
                else if(_ensemble != null && _ensemble.IsBottomTrackAvail)
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
        /// Heatmap plot.
        /// </summary>
        public HeatmapPlotViewModel HeatmapPlot { get; set; }

        /// <summary>
        /// Expanded Heatmap Plot.
        /// </summary>
        public HeatmapPlotViewModel HeatmapPlotExapnded { get; set; }

        /// <summary>
        /// Ship Track plot.
        /// </summary>
        public ShipTrackPlotViewModel ShipTrackPlot { get; set; }

        /// <summary>
        /// Time series plot.
        /// </summary>
        public TimeSeriesViewModel TimeSeriesPlot { get; set; }

        /// <summary>
        /// Compass Rose Plot.
        /// </summary>
        public CompassRoseViewModel CompassPlot { get; set; }

        /// <summary>
        /// 3D Profile Plot.
        /// </summary>
        public ProfilePlot3dViewModel Profile3dPlot { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Initialize the dashboard subsystem VM.
        /// </summary>
        /// <param name="config"></param>
        public DashboardSubsystemConfigViewModel(ViewSubsystemConfig config)
        {
            Config = config;

            _windowMgr = IoC.Get<IWindowManager>();

            //HeatmapPlot = new HeatmapPlotViewModel();
            //IoC.BuildUp(HeatmapPlot);
            HeatmapPlot = IoC.Get<HeatmapPlotViewModel>();
            HeatmapPlot.IsShowMenu = false;
            HeatmapPlot.IsShowStatusbar = false;

            ShipTrackPlot = IoC.Get<ShipTrackPlotViewModel>();
            ShipTrackPlot.IsShowMenu = false;
            ShipTrackPlot.IsShowStatusbar = false;

            TimeSeriesPlot = IoC.Get<TimeSeriesViewModel>();
            TimeSeriesPlot.IsShowMenu = false;
            TimeSeriesPlot.IsShowStatusbar = false;

            CompassPlot = IoC.Get<CompassRoseViewModel>();

            Application.Current.Dispatcher.Invoke((System.Action)delegate
            {
                Profile3dPlot = IoC.Get<ProfilePlot3dViewModel>();
            });

            EarthVelocity = new ObservableCollection<DataGridData>();
            InstrumentVelocity = new ObservableCollection<DataGridData>();
            BeamVelocity = new ObservableCollection<DataGridData>();
            VelocityVector = new ObservableCollection<VvDataGridData>();
            Amplitude = new ObservableCollection<DataGridData>();
            Correlation = new ObservableCollection<DataGridData>();
        }

        public void Dispose()
        {
            CompassPlot.Dispose();
        }

        #region Process Data

        /// <summary>
        /// Process the new data.
        /// </summary>
        private void ProcessData()
        {
            // Get the average velocity and direction
            CalcAvgVelDir();

            // Get the boat speed
            AverageBoatSpeed();

            // Add Comapss data to compass rose
            AddCompassData();

            // Plot 3D Velocity Plot
            Plot3dVelocityPlot();

            // Update the Heatmap
            HeatmapPlot.AddEnsemble(_ensemble);
            if(HeatmapPlotExapnded != null)
            {
                HeatmapPlotExapnded.AddEnsemble(_ensemble);
            }

            // Process the Earth velocity data
            EarthVelocity.Clear();
            if (_ensemble.IsEarthVelocityAvail)
            {
                ProcessVelocityData(EarthVelocity, _ensemble.EarthVelocityData.EarthVelocityData);
            }

            // Process the Instrument velocity data
            InstrumentVelocity.Clear();
            if (_ensemble.IsInstrumentVelocityAvail)
            {
                ProcessVelocityData(InstrumentVelocity, _ensemble.InstrumentVelocityData.InstrumentVelocityData);
            }

            // Process the Beam velocity data
            BeamVelocity.Clear();
            if (_ensemble.IsBeamVelocityAvail)
            {
                ProcessVelocityData(BeamVelocity, _ensemble.BeamVelocityData.BeamVelocityData);
            }

            // Process the Amplitude data
            Amplitude.Clear();
            if (_ensemble.IsAmplitudeAvail)
            {
                ProcessVelocityData(Amplitude, _ensemble.AmplitudeData.AmplitudeData);
            }

            // Process the Correlation data
            Correlation.Clear();
            if (_ensemble.IsCorrelationAvail)
            {
                ProcessVelocityData(Correlation, _ensemble.CorrelationData.CorrelationData);
            }

            // Process the Velocity Vectors data
            VelocityVector.Clear();
            if (_ensemble.IsEarthVelocityAvail && _ensemble.EarthVelocityData.IsVelocityVectorAvail)
            {
                ProcessVelocityVectorData(VelocityVector, _ensemble.EarthVelocityData.VelocityVectors);
            }
        }

        #region Calculate Average 

        /// <summary>
        /// Calculate the average velocity and direction.
        /// </summary>
        private void CalcAvgVelDir()
        {
            if(_ensemble != null && _ensemble.IsEarthVelocityAvail)
            {
                if(_ensemble.EarthVelocityData.IsVelocityVectorAvail)
                {
                    // Accumulate the data
                    int count = 0;
                    double vvMag = 0.0;
                    double vvDir = 0.0;
                    foreach(var vv in _ensemble.EarthVelocityData.VelocityVectors)
                    {
                        if(vv.DirectionYNorth != DataSet.Ensemble.BAD_VELOCITY && vv.Magnitude != DataSet.Ensemble.BAD_VELOCITY )
                        {
                            count++;
                            vvMag += vv.Magnitude;
                            vvDir += vv.DirectionYNorth;
                        }
                    }

                    // Calculate the average
                    double avgMag = 0.0;
                    double avgDir = 0.0;
                    if (count > 0)
                    {
                        avgMag = vvMag / count;
                        avgDir = vvDir / count;
                    }

                    AvgWaterDir = string.Format("{0} deg", avgDir.ToString("0.00"));
                    AvgWaterVel = string.Format("{0} m/s", avgMag.ToString("0.00"));
                }
            }
        }

        /// <summary>
        /// Get the Average Bottom Track Boat speed.
        /// </summary>
        private void AverageBoatSpeed()
        {
            if (_ensemble.IsBottomTrackAvail)
            {
                AvgBtSpeed = _ensemble.BottomTrackData.GetVelocityMagnitude().ToString("0.00") + " m/s";
            }
            else
            {
                AvgBtSpeed = "No BT Speed";
            }

            if(_ensemble.IsNmeaAvail && _ensemble.NmeaData.IsGpvtgAvail())
            {
                GpsSpeed = _ensemble.NmeaData.GPVTG.Speed.ToMetersPerSecond().ToString("0.00") + " m/s";
            }
            else
            {
                GpsSpeed = "No GPS Speed";
            }


        }

        #endregion

        #region Process Velocity data

        /// <summary>
        /// Process the velocity data.
        /// </summary>
        private void ProcessVelocityData(ObservableCollection<DataGridData> list, float[,] data)
        {
            int beams = data.GetLength(1);
            int bins = data.GetLength(0);

            //List<DataGridData> list = new List<DataGridData>();

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

            //return list;
        }

        /// <summary>
        /// Process the velocity vector data.
        /// </summary>
        private void ProcessVelocityVectorData(ObservableCollection<VvDataGridData> list, DataSet.VelocityVector[] data)
        {
            for(int bin = 0; bin < data.Length; bin++)
            {
                // Add the data to the list
                list.Add(new VvDataGridData(bin, 
                                            Math.Round(data[bin].Magnitude, 3), 
                                            Math.Round(data[bin].DirectionYNorth, 3)));
            }
        }

        #endregion

        #region Compass Data

        /// <summary>
        /// Add compass data to the compass rose.
        /// </summary>
        private void AddCompassData()
        {
            if(_ensemble != null)
            {
                if(_ensemble.IsAncillaryAvail)
                {
                    CompassPlot.AddIncomingData(_ensemble.AncillaryData.Heading, _ensemble.AncillaryData.Pitch, _ensemble.AncillaryData.Roll);
                }
                else if(_ensemble.IsBottomTrackAvail)
                {
                    CompassPlot.AddIncomingData(_ensemble.BottomTrackData.Heading, _ensemble.BottomTrackData.Pitch, _ensemble.AncillaryData.Roll);
                }
            }
        }

        #endregion

        #region 3D Velocity Plot

        /// <summary>
        /// Plot the 3D velocity plot.
        /// Get the velocity vectors from the ensemble.
        /// </summary>
        private void Plot3dVelocityPlot()
        {
            if (_ensemble != null && _ensemble.IsEnsembleAvail && _ensemble.IsEarthVelocityAvail && _ensemble.EarthVelocityData.IsVelocityVectorAvail)
            {
                // Create struct to hold the data
                DataSet.EnsembleVelocityVectors ensVec = new DataSet.EnsembleVelocityVectors();
                ensVec.Id = _ensemble.EnsembleData.UniqueId;
                ensVec.Vectors = _ensemble.EarthVelocityData.VelocityVectors;

                Profile3dPlot.AddData(ensVec);
            }
        }

        #endregion

        #endregion

        #region Expand Plot

        public bool CanExpandHeatmapPlot
        {
            get
            {
                if (HeatmapPlotExapnded == null)
                {
                    return true;
                }

                return false;
            }
        }

        public void ExpandHeatmapPlot()
        {
            // Create the Heatmap plot and attach to deactivate
            HeatmapPlotExapnded = IoC.Get<HeatmapPlotViewModel>();
            HeatmapPlotExapnded.Deactivated += HeatmapPlotExapnded_Deactivated;

            // Show the window and update the button
            _windowMgr.ShowWindow(HeatmapPlotExapnded);
            NotifyOfPropertyChange(() => this.CanExpandHeatmapPlot);

            Application.Current.Dispatcher.Invoke((System.Action)delegate
            {

                // Lock the plot
                lock (HeatmapPlotExapnded.Plot.SyncRoot)
                {
                    // Move all the data over
                    HeatmapPlotExapnded.Plot.Series.Clear();

                    // All all the data from the original Plot
                    foreach (var series in HeatmapPlot.Plot.Series)
                    {
                        // Profile series
                        if (series.GetType() == typeof(HeatMapSeries))
                        {
                            HeatMapSeries hmSeries = new HeatMapSeries();
                            hmSeries.X0 = 0;                                                  // Left starts 0
                            hmSeries.X1 = ((HeatMapSeries)series).Data.GetLength(0);          // Right (num ensembles)
                            hmSeries.Y0 = 0;                                                  // Top starts 0
                            hmSeries.Y1 = ((HeatMapSeries)series).Data.GetLength(1);          // Bottom end (num bins)
                            hmSeries.Data = ((HeatMapSeries)series).Data;
                            hmSeries.Interpolate = false;

                            // Add series
                            HeatmapPlotExapnded.Plot.Series.Add(hmSeries);
                        }

                        // Bottom Track series
                        if (series.GetType() == typeof(AreaSeries))
                        {
                            AreaSeries btSeries = new AreaSeries();
                            btSeries.Color = ((AreaSeries)series).Color;
                            btSeries.Color2 = ((AreaSeries)series).Color2;
                            btSeries.Fill = ((AreaSeries)series).Fill;
                            btSeries.Tag = ((AreaSeries)series).Tag;

                            foreach (var pt in ((AreaSeries)series).Points)
                            {
                                btSeries.Points.Add(pt);
                            }
                            foreach (var pt in ((AreaSeries)series).Points2)
                            {
                                btSeries.Points2.Add(pt);
                            }

                            // Add series
                            HeatmapPlotExapnded.Plot.Series.Add(btSeries);
                        }
                    }
                }

                // Then refresh the plot
                HeatmapPlotExapnded.Plot.InvalidatePlot(true);
            });
        }

        /// <summary>
        /// The Heatmap Plot Expanded is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeatmapPlotExapnded_Deactivated(object sender, DeactivationEventArgs e)
        {
            HeatmapPlotExapnded = null;
            NotifyOfPropertyChange(() => this.CanExpandHeatmapPlot);
        }

        #endregion

        #region Eventhandler

        /// <summary>
        /// Process the ensemble to display on the dashboard.
        /// </summary>
        /// <param name="ensemble">Latest ensemble.</param>
        /// <param name="source">Source of the data.</param>
        public void ProcessEnsemble(DataSet.Ensemble ensemble, EnsembleSource source)
        {
            // Set the latest ensemble
            _ensemble = ensemble;

            Application.Current.Dispatcher.Invoke((System.Action)delegate
            {
                // Process the information
                ProcessData();

                // Update the display
                NotifyOfPropertyChange(null);
            });
        }

        /// <summary>
        /// Load the project to all the plots.
        /// </summary>
        /// <param name="project"></param>
        public void LoadProject(Project project)
        {
            HeatmapPlot.LoadProject(project.GetProjectFullPath());
            ShipTrackPlot.LoadProject(project.GetProjectFullPath());
            TimeSeriesPlot.LoadProject(project.GetProjectFullPath());
        }

        #endregion
    }
}
