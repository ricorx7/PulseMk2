using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RTI
{
    public class ShipTrackPlotViewModel : PlotViewModel
    {

        #region Class

        public class ShipTrackData
        {

            /// <summary>
            /// Date and time of the ensemble.
            /// </summary>
            public string DateTime { get; set; }

            /// <summary>
            /// Ensemble number.
            /// </summary>
            public string EnsNum { get; set; }

            /// <summary>
            /// Latitude and Longitude
            /// </summary>
            public string LatLon { get; set; }

            /// <summary>
            /// GPS data.
            /// </summary>
            public List<DataPoint> ShipData { get; set; }

            /// <summary>
            /// Water Speed and Direction data.
            /// </summary>
            public Annotation WaterData { get; set; }

            public ShipTrackData()
            {
                ShipData = new List<DataPoint>();
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// Minimum Index.
        /// </summary>
        private int _minIndex;

        /// <summary>
        /// Maximum index.
        /// </summary>
        private int _maxIndex;

        #endregion

        #region Properties

        #region Plot Scale

        /// <summary>
        /// Magnitude scale.  This value is mulitplied to the magnitude
        /// value to increase or decrease the line length for visual representation.
        /// </summary>
        private int _MagScale;
        /// <summary>
        /// Magnitude scale.  This value is mulitplied to the magnitude
        /// value to increase or decrease the line length for visual representation.
        /// </summary>
        public int MagScale
        {
            get { return _MagScale; }
            set
            {
                _MagScale = value;
                NotifyOfPropertyChange(() => MagScale);

                ReplotData();
            }
        }

        /// <summary>
        /// Set the minimum value.
        /// </summary>
        private double _MinValue;
        /// <summary>
        /// Set the minimum value.
        /// </summary>
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                _MinValue = value;
                NotifyOfPropertyChange(() => MinValue);

                if (_MinValue > _MaxValue)
                {
                    MaxValue += 1;
                }

                // Replot the data
                ReplotData();

                // Set the color map canvas
                ColorMapCanvas = ColorHM.GetColorMapLegend(_MinValue, _MaxValue);
                NotifyOfPropertyChange(() => ColorMapCanvas);
            }
        }

        /// <summary>
        /// Set the maximum value.
        /// </summary>
        private double _MaxValue;
        /// <summary>
        /// Set the maximum value.
        /// </summary>
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                NotifyOfPropertyChange(() => MaxValue);

                if (_MaxValue < _MinValue)
                {
                    MinValue -= 1;
                }

                // Replot the data
                ReplotData();

                // Set the color map canvas
                ColorMapCanvas = ColorHM.GetColorMapLegend(_MinValue, _MaxValue);
                NotifyOfPropertyChange(() => ColorMapCanvas);
            }
        }

        #endregion

        #region Color Legend

        /// <summary>
        /// Convert the values to a color based off a color map.
        /// </summary>
        public ColorHeatMap ColorHM { get; set; }

        /// <summary>
        /// Color map canvas to display the options.
        /// </summary>
        public System.Windows.Controls.Canvas ColorMapCanvas { get; set; }

        #endregion

        #region Plot Options

        /// <summary>
        /// Use GPS speed as a backup value.
        /// </summary>
        private bool _IsUseGpsSpeedBackup;
        /// <summary>
        /// Use GPS speed as a backup value.
        /// </summary>
        public bool IsUseGpsSpeedBackup
        {
            get { return _IsUseGpsSpeedBackup; }
            set
            {
                _IsUseGpsSpeedBackup = value;
                NotifyOfPropertyChange(() => IsUseGpsSpeedBackup);

                ReplotData();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Initialize the Plot.
        /// </summary>
        public ShipTrackPlotViewModel() 
            : base("Ship Track Plot")
        {
            IsUseGpsSpeedBackup = true;

            // Create the plot
            Plot = CreatePlot();

            // Color map
            ColorHM = new ColorHeatMap(0x80);      // 50% alpha

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ColorMapCanvas = ColorHM.GetColorMapLegend(_MinValue, _MaxValue);       // Set the color map canvas
            });
        }

        #region Create Plot

        /// <summary>
        /// Create the plot.
        /// </summary>
        /// <returns></returns>
        private ViewResolvingPlotModel CreatePlot()
        {
            ViewResolvingPlotModel temp = new ViewResolvingPlotModel();

            temp.IsLegendVisible = true;

            //temp.Background = OxyColors.Black;
            //temp.TextColor = OxyColors.White;
            //temp.PlotAreaBorderColor = OxyColors.White;

            temp.Title = "Ship Track";

            // Setup the axis
            //var c = OxyColors.White;
            temp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                //Minimum = 0,
                //StartPosition = 1,                                              // This will invert the axis to start at the top with minimum value
                //EndPosition = 0
                //TicklineColor = OxyColors.White,
                //MajorGridlineStyle = LineStyle.Solid,
                //MinorGridlineStyle = LineStyle.Solid,
                //MajorGridlineColor = OxyColor.FromAColor(40, c),
                //MinorGridlineColor = OxyColor.FromAColor(20, c),
                //IntervalLength = 5,
                MinimumPadding = 0.1,                                               // Pad the top and bottom of the plot so min/max lines can be seen
                MaximumPadding = 0.1,                                               // Pad the top and bottom of the plot so min/max lines can be seen
                Unit = "m"
            });
            temp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                //MajorStep = 1
                //Minimum = 0,
                //Maximum = _maxDataSets,
                //TicklineColor = OxyColors.White,
                //MajorGridlineStyle = LineStyle.Solid,
                //MinorGridlineStyle = LineStyle.Solid,
                //MajorGridlineColor = OxyColor.FromAColor(40, c),
                //MinorGridlineColor = OxyColor.FromAColor(20, c),
                //IntervalLength = 5,
                //TickStyle = OxyPlot.Axes.TickStyle.None,
                //IsAxisVisible = false,
                Unit = "m"
            });

            temp.Series.Add(new LineSeries() { Color = OxyColors.Chartreuse, StrokeThickness = 1, Title = "Ship Track" });

            return temp;
        }

        #endregion

        #region Load Project

        /// <summary>
        /// Load the project.  Use the selected min and max index to select the ensemble range to display.
        /// </summary>
        /// <param name="fileName">Project file path.</param>
        /// <param name="minIndex">Minimum Ensemble index.</param>
        /// <param name="maxIndex">Maximum Ensemble index.</param>
        public override void LoadProject(string fileName, SubsystemConfiguration ssConfig, int minIndex = 0, int maxIndex = 0)
        {
            // Load the base calls
            base.LoadProject(fileName, ssConfig, minIndex, maxIndex);

            // Plot the data
            ReplotData(minIndex, maxIndex);
        }

        #endregion

        #region Replot Data

        /// <summary>
        /// Implement reploting the data.
        /// </summary>
        /// <param name="minIndex">Minimum Index.</param>
        /// <param name="maxIndex">Maximum Index.</param>
        public override void ReplotData(int minIndex, int maxIndex)
        {
            Task.Run(() =>
            {
                _minIndex = minIndex;
                _maxIndex = maxIndex;
                DrawPlot(minIndex, maxIndex);

            });
        }

        /// <summary>
        /// Implement replotting the data.
        /// </summary>
        public override void ReplotData()
        {
            // Replot the data
            ReplotData(_minIndex, _maxIndex);
        }

        #endregion

        #region Draw Plot

        /// <summary>
        /// Get the data from the database.  Then draw the plot.
        /// </summary>
        /// <param name="minIndex">Minimum index in the database.</param>
        /// <param name="maxIndex">Maximum index in the database.</param>
        private async void DrawPlot(int minIndex, int maxIndex)
        {
            // Clear the current markers
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Plot.Series.Count > 1)
                {
                    // Clear the data
                    ((LineSeries)Plot.Series[0]).Points.Clear();
                }
                else
                {
                    // Add the line series back if cleared
                    Plot.Series.Add(new LineSeries() { Color = OxyColors.Chartreuse, StrokeThickness = 1, Title = "Ship Track" });
                }

            });

            // Init the list of data
            List<ShipTrackData> data = new List<ShipTrackData>();

            // Verify a file was given
            if (!string.IsNullOrEmpty(_ProjectFilePath))
            {
                // Verify the file exist
                if (File.Exists(_ProjectFilePath))
                {
                    // Create data Source string
                    string dataSource = string.Format("Data Source={0};Version=3;", _ProjectFilePath);

                    try
                    {
                        // Create a new database connection:
                        using (SQLiteConnection sqlite_conn = new SQLiteConnection(dataSource))
                        {
                            // Open the connection:
                            sqlite_conn.Open();

                            // Get total number of ensembles in the project
                            await Task.Run(() => TotalNumEnsembles = GetNumEnsembles(sqlite_conn));

                            // If this is the first time loading
                            // show the entire plot
                            if (_firstLoad)
                            {
                                _firstLoad = false;
                                minIndex = 1;
                                maxIndex = TotalNumEnsembles;
                            }

                            // Get the data from the project
                            await Task.Run(() => data = GetData(sqlite_conn, _MagScale, minIndex, maxIndex));

                            // Close connection
                            sqlite_conn.Close();
                        }

                        // If there is no data, do not plot
                        if (data != null && data.Count > 0)
                        {
                            // Update status
                            StatusMsg = "Drawing Plot";

                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                // Plot the data
                                PlotData(data);
                            });
                        }
                        else
                        {
                            StatusMsg = "No GPS data to plot";
                        }

                    }
                    catch (SQLiteException e)
                    {
                        Debug.WriteLine("Error using database", e);
                        return;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error using database", e);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get the data based off the selected data type.
        /// </summary>
        /// <param name="cnn">SQLite connection.</param>
        /// <param name="magScale">Magnitude scale.</param>
        /// <param name="minIndex">Minimum Ensemble index.</param>
        /// <param name="maxIndex">Maximum Ensemble index.</param>
        /// <returns>The selected for each ensemble and bin.</returns>
        private List<ShipTrackData> GetData(SQLiteConnection cnn, double magScale, int minIndex = 0, int maxIndex = 0)
        {
            //StatusProgressMax = TotalNumEnsembles;
            StatusProgress = 0;

            string datasetColumnName = "BottomTrackDS";

            // Get the number of ensembles
            string query = string.Format("SELECT COUNT(*) FROM tblEnsemble WHERE ({0} IS NOT NULL) {1} {2};",
                                                                    datasetColumnName,
                                                                    GenerateQueryFileList(),
                                                                    GenerateQuerySubsystemList());
            int numEnsembles = GetNumEnsembles(cnn, query);
            // Update the progress bar
            StatusProgressMax = numEnsembles;

            // If min and max are used, set the limit and offset
            LimitOffset lo = CalcLimitOffset(numEnsembles, minIndex, maxIndex);
            numEnsembles = lo.Limit;

            // Get data
            query = string.Format("SELECT ID,EnsembleNum,DateTime,EnsembleDS,AncillaryDS,BottomTrackDS,EarthVelocityDS,NmeaDS,{0} FROM tblEnsemble WHERE ({1} IS NOT NULL) {2} {3} LIMIT {4} OFFSET {5};",
                                            datasetColumnName,
                                            datasetColumnName,
                                            GenerateQueryFileList(),
                                            GenerateQuerySubsystemList(),
                                            lo.Limit,
                                            lo.Offset);

            // Get the data to plot
            return QueryDataFromDb(cnn, query, magScale, minIndex, maxIndex);
        }

        /// <summary>
        /// Query the data from the database.
        /// </summary>
        /// <param name="cnn">SQLite connection.</param>
        /// <param name="query">Query for the data.</param>
        /// <param name="magScale">Magnitude scale.</param>
        /// <param name="minIndex">Minimum index.</param>
        /// <param name="maxIndex">Maximum index.</param>
        /// <returns></returns>
        private List<ShipTrackData> QueryDataFromDb(SQLiteConnection cnn, string query, double magScale, int minIndex = 0, int maxIndex = 0)
        {
            // Init list
            double prevBtEast = DbDataHelper.BAD_VELOCITY;
            double prevBtNorth = DbDataHelper.BAD_VELOCITY;
            double prevBtTime = 0.0;

            // Init the new series data
            List<ShipTrackData> stDataList = new List<ShipTrackData>();
            //stData.MagScale = magScale;

            // Ensure a connection was made
            if (cnn == null)
            {
                return null;
            }

            using (DbCommand cmd = cnn.CreateCommand())
            {
                cmd.CommandText = query;

                // Get Result
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ShipTrackData stData = new ShipTrackData();

                    // Update the status
                    StatusProgress++;
                    StatusMsg = reader["EnsembleNum"].ToString();

                    // Get the Ensemble number and Date and time
                    stData.DateTime = reader["DateTime"].ToString();
                    stData.EnsNum = reader["EnsembleNum"].ToString();

                    // Plot the lat/lon
                    //stData.LatLon = reader["Position"].ToString();

                    // Heading
                    //DbDataHelper.HPR hpr = DbDataHelper.GetHPR(reader);
                    //stData.Heading = hpr.Heading;

                    //// Get the range bin
                    //int rangeBin = DbDataHelper.GetRangeBin(reader);

                    //// Get the magnitude data
                    //string jsonEarth = reader["EarthVelocityDS"].ToString();
                    //if (!string.IsNullOrEmpty(jsonEarth))
                    //{
                    //    // Convert to a JSON object
                    //    JObject ensEarth = JObject.Parse(jsonEarth);

                    //    // Average the data
                    //    avgMag = DbDataHelper.GetAvgMag(ensEarth, IsMarkBadBelowBottom, rangeBin, DbDataHelper.BAD_VELOCITY);
                    //    avgDir = DbDataHelper.GetAvgDir(ensEarth, IsMarkBadBelowBottom, rangeBin, DbDataHelper.BAD_VELOCITY);

                    //    //Debug.WriteLine(string.Format("Avg Dir: {0} Avg Mag: {1}", avgDir, avgMag));
                    //}

                    if (IsUseGpsSpeedBackup)
                    {
                        // Get the GPS data from the database
                        DbDataHelper.GpsData gpsData = DbDataHelper.GetGpsData(reader);

                        // Check for a backup value for BT East and North speed from the GPS if a Bottom Track value is never found
                        if (Math.Round(prevBtEast, 4) == BAD_VELOCITY && gpsData != null && gpsData.IsBackShipSpeedGood)
                        {
                            prevBtEast = gpsData.BackupShipEast;
                            prevBtNorth = gpsData.BackupShipNorth;
                        }
                    }

                    // Get the velocity
                    DbDataHelper.VelocityMagDir velMagDir = DbDataHelper.CreateVelocityVectors(reader, prevBtEast, prevBtNorth, true, true);

                    if (velMagDir.IsBtVelGood)
                    {
                        // Get the average range
                        //stData.AvgRange = DbDataHelper.GetAverageRange(reader);
                        DataPoint pt = CalculateDistanceTraveledPoint(velMagDir, prevBtTime, prevBtEast, prevBtNorth);
                        stData.ShipData.Add(pt);

                        // Store the backup value
                        prevBtEast = velMagDir.BtEastVel;
                        prevBtNorth = velMagDir.BtNorthVel;
                        prevBtTime = velMagDir.BtFirstPingTime;
                    }

                    // Add the data to the list
                    stDataList.Add(stData);
                }
            }

            return stDataList;
        }

        /// <summary>
        /// Create a data point based off the Bottom Track data.
        /// </summary>
        /// <param name="data">Data from the ensemble.</param>
        /// <param name="prevBtTime">Previous Bottom Track time.</param>
        /// <param name="prevBtEast">Previous Bottom Track East velocity.</param>
        /// <param name="prevBtNorth">Previous Bottom Track North velocity.</param>
        /// <param name="declination">Declination or heading offset.</param>
        /// <param name="xOffset">X offset.</param>
        /// <param name="yOffset">Y offset.</param>
        /// <returns></returns>
        private DataPoint CalculateDistanceTraveledPoint(DbDataHelper.VelocityMagDir data, double prevBtTime, double prevBtEast, double prevBtNorth, double declination = 0.0, double xOffset = 0.0, double yOffset = 0.0)
        {
            double dT = data.BtFirstPingTime - prevBtTime;

            if(prevBtEast == BAD_VELOCITY || prevBtNorth == BAD_VELOCITY)
            {
                prevBtEast = 0.0;
                prevBtNorth = 0.0;
            }

            double BtE = 0.5 * dT * (data.BtEastVel + prevBtEast);
            double BtN = 0.5 * dT * (data.BtNorthVel + prevBtNorth);

            double BtEarthMag = Math.Sqrt((BtE * BtE) + (BtN * BtN));
            double BtEarthDir = (Math.Atan2(BtE, BtN) * (180.0 / Math.PI)) + declination;
            if (BtEarthDir < 0.0)
            {
                BtEarthDir = 360.0 + BtEarthDir;
            }

            // Generate X,Y point
            double x = xOffset + (BtEarthMag * Math.Sin(MathHelper.DegreeToRadian(BtEarthDir)));
            double y = yOffset + (BtEarthMag * Math.Cos(MathHelper.DegreeToRadian(BtEarthDir)));

            return new DataPoint(x, y);
        }

        #endregion

        #region Plot Data

        /// <summary>
        /// Plot the data given.
        /// </summary>
        /// <param name="data">Data to plot.</param>
        private void PlotData(List<ShipTrackData> data)
        {
            lock(Plot.SyncRoot)
            {
                foreach (ShipTrackData std in data)
                {
                    ((LineSeries)Plot.Series[0]).Points.AddRange(std.ShipData);
                }
            }

            Plot.InvalidatePlot(true);
        }

        #endregion
    }
}
