using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RTI
{
    public class DashboardSubsystemConfigViewModel : Caliburn.Micro.Screen
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

                        return string.Format("{0} m", depth.ToString("0.00"));
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

        #region Plots

        /// <summary>
        /// Heatmap plot.
        /// </summary>
        public HeatmapPlotViewModel HeatmapPlot { get; set; }

        /// <summary>
        /// Ship Track plot.
        /// </summary>
        public ShipTrackPlotViewModel ShipTrackPlot { get; set; }

        /// <summary>
        /// Time series plot.
        /// </summary>
        public TimeSeriesViewModel TimeSeriesPlot { get; set; }

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
            HeatmapPlot = IoC.Get<HeatmapPlotViewModel>();
            HeatmapPlot.IsShowMenu = false;
            HeatmapPlot.IsShowStatusbar = false;

            ShipTrackPlot = IoC.Get<ShipTrackPlotViewModel>();
            ShipTrackPlot.IsShowMenu = false;
            ShipTrackPlot.IsShowStatusbar = false;

            TimeSeriesPlot = IoC.Get<TimeSeriesViewModel>();
            TimeSeriesPlot.IsShowMenu = false;
            TimeSeriesPlot.IsShowStatusbar = false;

            EarthVelocity = new ObservableCollection<DataGridData>();
            InstrumentVelocity = new ObservableCollection<DataGridData>();
            BeamVelocity = new ObservableCollection<DataGridData>();
            VelocityVector = new ObservableCollection<VvDataGridData>();
            Amplitude = new ObservableCollection<DataGridData>();
            Correlation = new ObservableCollection<DataGridData>();
        }


        #region Process Data

        /// <summary>
        /// Process the new data.
        /// </summary>
        private void ProcessData()
        {
            // Get the average velocity and direction
            CalcAvgVelDir();

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

        #endregion

        #region Expand Plot

        public void ExpandHeatmapPlot()
        {
            //_windowMgr.ShowWindow(IoC.Get<HeatmapPlotViewModel>(), HeatmapPlot);
            //HeatmapPlotViewModel vm = IoC.Get<HeatmapPlotViewModel>(Config.Config.CepoIndex.ToString());
            //_windowMgr.ShowWindow(vm);
            //_windowMgr.ShowWindow(HeatmapPlot);

            _windowMgr.ShowWindow(IoC.Get<HeatmapPlotViewModel>());
            //_windowMgr.ShowDialog(IoC.Get<HeatmapPlotViewModel>());
            //_windowMgr.ShowWindow(IoC.GetInstance(typeof(HeatmapPlotViewModel), Config.Config.CepoIndex.ToString()));
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
