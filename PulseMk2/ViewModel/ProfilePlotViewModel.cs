using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public class ProfilePlotViewModel : PlotViewModel
    {

        #region Variables

        /// <summary>
        /// Bin size of current ensemble.
        /// </summary>
        private float _BinSize;

        /// <summary>
        /// Blank of current ensemble.
        /// </summary>
        private float _Blank;

        #endregion

        #region Class and Enum

        /// <summary>
        /// Plot types.
        /// </summary>
        public enum PlotType
        {
            PLOT_CORRELATION,

            PLOT_AMPLITUDE,

            PLOT_VELOCITY
        }


        #endregion

        #region Properties

        /// <summary>
        /// Type of plot.
        /// </summary>
        private PlotType _SeriesType;
        /// <summary>
        /// Type of plot.
        /// </summary>
        public PlotType SeriesType
        {
            get { return _SeriesType; }
            set
            {
                _SeriesType = value;
                this.NotifyOfPropertyChange(() => this.SeriesType);
            }
        }

        /// <summary>
        /// Flag if the axis label should be depth or bins.
        /// </summary>
        private bool _IsDepthAxisLabel;
        /// <summary>
        /// Flag if the axis label should be depth or bins.
        /// </summary>
        public bool IsDepthAxisLabel
        {
            get { return _IsDepthAxisLabel; }
            set
            {
                _IsDepthAxisLabel = value;
                this.NotifyOfPropertyChange(() => this.IsDepthAxisLabel);
            }
        }


        #endregion

        /// <summary>
        /// Profile Plot View model.
        /// </summary>
        public ProfilePlotViewModel()
            : base("Profile Plot")
        {
            _Blank = 0.0f;
            _BinSize = 0.0f;
            IsDepthAxisLabel = false;

            // Set the series type
            SeriesType = PlotType.PLOT_CORRELATION;

            // Create the plot
            Plot = CreatePlot();

            // Set the axis
            SetAxis(SeriesType, _IsDepthAxisLabel);
        }

        #region Create Plot

        /// <summary>
        /// Create the plot.  Set the settings for the plot.
        /// </summary>
        /// <returns>Plot created.</returns>
        private ViewResolvingPlotModel CreatePlot()
        {
            ViewResolvingPlotModel temp = new ViewResolvingPlotModel();

            // Set the legend position
            temp.IsLegendVisible = true;
            temp.LegendPosition = LegendPosition.BottomCenter;
            temp.LegendPlacement = LegendPlacement.Inside;
            temp.LegendOrientation = LegendOrientation.Horizontal;
            temp.LegendFontSize = 10;
            temp.LegendItemSpacing = 8;

            return temp;
        }

        /// <summary>
        ///  Set the axis based off the plot type.
        /// </summary>
        /// <param name="type">Plot type.</param>
        /// <param name="isDepthAxis">Should the left axis be bins or depth.</param>
        public void SetAxis(PlotType type, bool isDepthAxis)
        {
            IsDepthAxisLabel = isDepthAxis;
            SeriesType = type;

            // Clear the current axes
            Plot.Axes.Clear();

            // For the correlation plot, display the depth for the axis
            // For the rest of the plots use the bin.
            if (type == PlotType.PLOT_CORRELATION)
            {
                // Setup the axis
                LinearAxis series = new LinearAxis()
                {
                    Position = AxisPosition.Left,
                    StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20
                };

                if(_IsDepthAxisLabel)
                {
                    series.Unit = "m";
                }
                else
                {
                    series.Unit = "bin";
                }

                Plot.Axes.Add(series);
                // Setup the axis
                Plot.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    //StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    //EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20,
                    MajorStep = 20,
                    MinimumPadding = 0,                                                 // Start at axis edge   
                    MaximumPadding = 0,
                    Unit = "%",
                    Minimum = 0,
                    Maximum = 100
                });

                Plot.Title = "Correlation";
            }
            else if(type == PlotType.PLOT_AMPLITUDE)
            {
                // Setup the axis
                LinearAxis series = new LinearAxis()
                {
                    Position = AxisPosition.Left,
                    StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20
                };

                if (_IsDepthAxisLabel)
                {
                    series.Unit = "m";
                }
                else
                {
                    series.Unit = "bin";
                }

                Plot.Axes.Add(series);
                // Setup the axis
                Plot.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    //StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    //EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20,
                    MajorStep = 20,
                    MinimumPadding = 0,                                                 // Start at axis edge   
                    MaximumPadding = 0,
                    Unit = "dB",
                    Minimum = 0,
                    Maximum = 120
                });

                Plot.Title = "Amplitude";
            }
            else if (type == PlotType.PLOT_VELOCITY)
            {
                // Setup the axis
                LinearAxis series = new LinearAxis()
                {
                    Position = AxisPosition.Left,
                    StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20
                };

                if (_IsDepthAxisLabel)
                {
                    series.Unit = "m";
                }
                else
                {
                    series.Unit = "bin";
                }

                Plot.Axes.Add(series);
                // Setup the axis
                Plot.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    //StartPosition = 1,                                                  // This will invert the axis to start at the top with minimum value
                    //EndPosition = 0,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Inside,                               // Put tick lines inside the plot
                    IntervalLength = 20,
                    MajorStep = 20,
                    MinimumPadding = 0,                                                 // Start at axis edge   
                    MaximumPadding = 0,
                    Unit = "m/s",
                    Minimum = 0,
                    Maximum = 2
                });

                Plot.Title = "Beam Velocity";
            }
        }

        #endregion

        #region Get Ensemble

        /// <summary>
        /// Add the latest ensemble to the plot.
        /// </summary>
        /// <param name="ens">Ensemble data.</param>
        public override void AddEnsemble(DataSet.Ensemble ens)
        {
            // Get the data based off the series type
            float[,] data = GetData(ens);

            // Set the bin size and blank

            if (ens.IsAncillaryAvail)
            {
                _BinSize = ens.AncillaryData.BinSize;
                _Blank = ens.AncillaryData.FirstBinRange;
            }

            // Draw the plot
            DrawPlot(data);
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get the data from the ensemble based off the series type.
        /// </summary>
        /// <param name="ens">Ensemble to get the data.</param>
        /// <returns>The data from the ensemble or null.</returns>
        private float[,] GetData(DataSet.Ensemble ens)
        {
            switch(_SeriesType)
            {
                case PlotType.PLOT_AMPLITUDE:
                    return GetAmpData(ens);
                case PlotType.PLOT_CORRELATION:
                    return GetCorrData(ens);
                case PlotType.PLOT_VELOCITY:
                    return GetVelData(ens);
                default:
                    return GetAmpData(ens);
            }
        }

        /// <summary>
        /// Get the Amplitude data.
        /// [bin, beam]
        /// GetLength(0) = Number of bins
        /// GetLength(1) = Number of beams
        /// </summary>
        /// <param name="ens">Ensemble.</param>
        /// <returns>Return the Amplitude data or null.</returns>
        private float[,] GetAmpData(DataSet.Ensemble ens)
        {
            if(ens.IsAmplitudeAvail)
            {
                return ens.AmplitudeData.AmplitudeData;
            }

            return null;
        }

        /// <summary>
        /// Get the Correlation data.
        /// [bin, beam]
        /// GetLength(0) = Number of bins
        /// GetLength(1) = Number of beams
        /// </summary>
        /// <param name="ens">Ensemble.</param>
        /// <returns>Return the Correlation data or null.</returns>
        private float[,] GetCorrData(DataSet.Ensemble ens)
        {
            if (ens.IsCorrelationAvail)
            {
                float[,] result = new float[ens.CorrelationData.CorrelationData.GetLength(0), ens.CorrelationData.CorrelationData.GetLength(1)];

                for(int bin = 0; bin < ens.CorrelationData.CorrelationData.GetLength(0); bin++)
                {
                    for(int beam = 0; beam < ens.CorrelationData.CorrelationData.GetLength(1); beam++)
                    {
                        result[bin, beam] = ens.CorrelationData.CorrelationData[bin, beam] * 100;   // Convert to percent
                    }
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Get the Beam Velocity data.
        /// [bin, beam]
        /// GetLength(0) = Number of bins
        /// GetLength(1) = Number of beams
        /// </summary>
        /// <param name="ens">Ensemble.</param>
        /// <returns>Return the Beam Velocity data or null.</returns>
        private float[,] GetVelData(DataSet.Ensemble ens)
        {
            if (ens.IsBeamVelocityAvail)
            {
                return ens.BeamVelocityData.BeamVelocityData;
            }

            return null;
        }

        #endregion

        #region Draw Plot

        /// <summary>
        /// Draw the plot based off the data given.
        /// </summary>
        /// <param name="data">Data to plot.</param>
        private void DrawPlot(float[,] data)
        {
            if (data != null)
            {
                // Clear all the current data
                ClearSeries();

                int numBins = data.GetLength(0);
                int numBeams = data.GetLength(1);

                // Go through each beam
                for(int beam = 0; beam < numBeams; beam++)
                {
                    // Create the line series by adding the points for each beam and bin
                    LineSeries series = new LineSeries();
                    series.Title = "Beam " + beam.ToString("0");

                    for(int bin = 0; bin < numBins; bin++)
                    {
                        if (_IsDepthAxisLabel)
                        {
                            // Get bin depth
                            float binDepth = _Blank + (bin * _BinSize);

                            series.Points.Add(new DataPoint(data[bin, beam], binDepth));
                        }
                        else
                        {
                            series.Points.Add(new DataPoint(data[bin, beam], bin));
                        }
                    }

                    // Draw the series
                    DrawSeries(series);
                }

            }
        }

        /// <summary>
        /// Add the series to the plot.
        /// </summary>
        /// <param name="series">Series to add.</param>
        private void DrawSeries(LineSeries series)
        {
            lock(Plot.SyncRoot)
            {
                Plot.Series.Add(series);
            }

            // After the line series have been updated
            // Refresh the plot with the latest data.
            Plot.InvalidatePlot(true);
        }

        /// <summary>
        /// Clear all the series from the plot.
        /// </summary>
        private void ClearSeries()
        {
            lock (Plot.SyncRoot)
            {
                Plot.Series.Clear();
            }

            // After the line series have been updated
            // Refresh the plot with the latest data.
            Plot.InvalidatePlot(true);
        }

        #endregion

    }
}
