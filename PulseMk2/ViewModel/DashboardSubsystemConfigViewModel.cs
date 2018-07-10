using Caliburn.Micro;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
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
        /// Expanded Ship Track plot.
        /// </summary>
        public ShipTrackPlotViewModel ShipTrackPlotExpanded { get; set; }

        /// <summary>
        /// Time series plot.
        /// </summary>
        public TimeSeriesViewModel TimeSeriesPlot { get; set; }

        /// <summary>
        /// Expanded Time series plot.
        /// </summary>
        public TimeSeriesViewModel TimeSeriesPlotExpanded { get; set; }

        /// <summary>
        /// 3D Profile Plot.
        /// </summary>
        public ProfilePlot3dViewModel Profile3dPlot { get; set; }

        /// <summary>
        /// Tabular data and compass rose.
        /// </summary>
        public TabularViewModel TabularData { get; set; }

        /// <summary>
        /// Tabular data and compass rose.
        /// </summary>
        public TabularViewModel TabularDataExpanded { get; set; }

        /// <summary>
        /// Correlation plot.
        /// </summary>
        public ProfilePlotViewModel CorrelationPlot { get; set; }

        /// <summary>
        /// Amplitude plot.
        /// </summary>
        public ProfilePlotViewModel AmplitudePlot { get; set; }

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

            HeatmapPlot = IoC.Get<HeatmapPlotViewModel>();
            HeatmapPlot.IsShowMenu = false;
            HeatmapPlot.IsShowStatusbar = false;
            HeatmapPlotExapnded = null;

            ShipTrackPlot = IoC.Get<ShipTrackPlotViewModel>();
            ShipTrackPlot.IsShowMenu = false;
            ShipTrackPlot.IsShowStatusbar = false;
            ShipTrackPlotExpanded = null;

            TimeSeriesPlot = IoC.Get<TimeSeriesViewModel>();
            TimeSeriesPlot.IsShowMenu = false;
            TimeSeriesPlot.IsShowStatusbar = false;
            TimeSeriesPlotExpanded = null;

            TabularData = IoC.Get<TabularViewModel>();
            TabularDataExpanded = null;

            Application.Current.Dispatcher.Invoke((System.Action)delegate
            {
                Profile3dPlot = IoC.Get<ProfilePlot3dViewModel>();
            });

            CorrelationPlot = IoC.Get<ProfilePlotViewModel>();
            CorrelationPlot.SetAxis(ProfilePlotViewModel.PlotType.PLOT_CORRELATION, false);

            AmplitudePlot = IoC.Get<ProfilePlotViewModel>();
            AmplitudePlot.SetAxis(ProfilePlotViewModel.PlotType.PLOT_AMPLITUDE, true);
        }

        /// <summary>
        /// Shutdown the view model.
        /// </summary>
        public void Dispose()
        {
            TabularData.Dispose();

            if(TabularDataExpanded != null)
            {
                TabularDataExpanded.Dispose();
            }
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

            // Pass the data to tabular
            TabularData.ProcessData(_ensemble);
            if(TabularDataExpanded != null)
            {
                TabularDataExpanded.ProcessData(_ensemble);
            }

            TimeSeriesPlot.AddEnsemble(_ensemble);

            // Plot 3D Velocity Plot
            Plot3dVelocityPlot();

            // Update the Heatmap
            HeatmapPlot.AddEnsemble(_ensemble);
            if(HeatmapPlotExapnded != null)
            {
                HeatmapPlotExapnded.AddEnsemble(_ensemble);
            }

            // Plot the amplitude data
            AmplitudePlot.AddEnsemble(_ensemble);

            // Plot the correlation data
            CorrelationPlot.AddEnsemble(_ensemble);
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

        #region Heatmap

        /// <summary>
        /// Flag if the Expanded Heatmap is already displayed.
        /// </summary>
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

        /// <summary>
        /// Expand the Heatmap plot to a new window.
        /// Pass all the data from the old plot to the new plot.
        /// </summary>
        public void ExpandHeatmapPlot()
        {
            // Create the Heatmap plot and attach to deactivate
            HeatmapPlotExapnded = IoC.Get<HeatmapPlotViewModel>();
            HeatmapPlotExapnded.Deactivated += HeatmapPlotExapnded_Deactivated;

            // Show the window and update the button
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.MinWidth = 600;
            settings.MinHeight = 300;
            _windowMgr.ShowWindow(HeatmapPlotExapnded, null, settings);
            NotifyOfPropertyChange(() => this.CanExpandHeatmapPlot);

            // Duplicate the plot
            HeatmapPlotExapnded.DuplicatePlot(HeatmapPlot);
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

        #region Ship Track

        /// <summary>
        /// Flag if the Expanded Heatmap is already displayed.
        /// </summary>
        public bool CanExpandShipTrackPlot
        {
            get
            {
                if (ShipTrackPlotExpanded == null)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Expand the Heatmap plot to a new window.
        /// Pass all the data from the old plot to the new plot.
        /// </summary>
        public void ExpandShipTrackPlot()
        {
            // Create the Heatmap plot and attach to deactivate
            ShipTrackPlotExpanded = IoC.Get<ShipTrackPlotViewModel>();
            ShipTrackPlot.IsShowMenu = false;
            ShipTrackPlot.IsShowStatusbar = false;
            ShipTrackPlotExpanded.Deactivated += ExpandShipTrackPlot_Deactivated;

            // Show the window and update the button
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.MinWidth = 600;
            settings.MinHeight = 300;
            _windowMgr.ShowWindow(ShipTrackPlotExpanded, null, settings);
            //_windowMgr.ShowWindow(ShipTrackPlotExpanded);
            NotifyOfPropertyChange(() => this.CanExpandShipTrackPlot);

            // Duplicate the plot
            ShipTrackPlotExpanded.DuplicatePlot(ShipTrackPlot);
        }

        /// <summary>
        /// The Heatmap Plot Expanded is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandShipTrackPlot_Deactivated(object sender, DeactivationEventArgs e)
        {
            ShipTrackPlotExpanded = null;
            NotifyOfPropertyChange(() => this.CanExpandShipTrackPlot);
        }

        #endregion

        #region Time Series

        /// <summary>
        /// Flag if the Expanded Time Series is already displayed.
        /// </summary>
        public bool CanExpandTimeSeriesPlot
        {
            get
            {
                if (TimeSeriesPlotExpanded == null)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Expand the Time Series plot to a new window.
        /// Pass all the data from the old plot to the new plot.
        /// </summary>
        public void ExpandTimeSeriesPlot()
        {
            // Create the Heatmap plot and attach to deactivate
            TimeSeriesPlotExpanded = IoC.Get<TimeSeriesViewModel>();
            TimeSeriesPlotExpanded.Deactivated += TimeSeriesPlotExpanded_Deactivated;

            // Show the window and update the button
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.MinWidth = 600;
            settings.MinHeight = 300;
            _windowMgr.ShowWindow(TimeSeriesPlotExpanded, null, settings);
            NotifyOfPropertyChange(() => this.CanExpandTimeSeriesPlot);

            // Duplicate the plot
            TimeSeriesPlotExpanded.DuplicatePlot(TimeSeriesPlot);
        }

        /// <summary>
        /// The Time Series Plot Expanded is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSeriesPlotExpanded_Deactivated(object sender, DeactivationEventArgs e)
        {
            TimeSeriesPlotExpanded = null;
            NotifyOfPropertyChange(() => this.CanExpandTimeSeriesPlot);
        }

        #endregion

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

            //Application.Current.Dispatcher.Invoke((System.Action)delegate
            //{
                // Process the information
                ProcessData();

                // Update the display
                NotifyOfPropertyChange(null);
            //});
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
