using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public class DashboardSubsystemConfigViewModel : Caliburn.Micro.Screen
    {
        #region Variables

        /// <summary>
        /// Latest ensemble.
        /// </summary>
        private DataSet.Ensemble _ensemble;

        #endregion

        #region Properties

        /// <summary>
        /// Subsystem configuration.
        /// </summary>
        public ViewSubsystemConfig Config { get; set; }

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
        /// Average depth.
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

        /// <summary>
        /// Heatmap plot.
        /// </summary>
        public HeatmapPlotViewModel HeatmapPlot { get; set; }

        #endregion

        /// <summary>
        /// Initialize the dashboard subsystem VM.
        /// </summary>
        /// <param name="config"></param>
        public DashboardSubsystemConfigViewModel(ViewSubsystemConfig config)
        {
            Config = config;

            HeatmapPlot = new HeatmapPlotViewModel();
        }


        #region Process Data

        /// <summary>
        /// Process the new data.
        /// </summary>
        private void ProcessData()
        {
            // Get the average velocity and direction
            CalcAvgVelDir();
        }

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

            // Process the information
            ProcessData();

            // Update the display
            NotifyOfPropertyChange(null);
        }

        #endregion
    }
}
