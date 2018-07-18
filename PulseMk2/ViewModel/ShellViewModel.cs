using Caliburn.Micro;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Windows.Forms;

namespace RTI {
    public class ShellViewModel : Conductor<object>, IShell, IDeactivate, IHandle<PlaybackTotalEnsemblesEvent>
    {
        #region Variables

        /// <summary>
        ///  Setup logger
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int MIN_ENS_INDEX = 1;

        /// <summary>
        /// Error log.
        /// </summary>
        public const string ERRORLOG_PATH = @"C:\RTI_Capture\PulseMk2ErrorLog.log";

        /// <summary>
        /// Event manager.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Playback timer.
        /// </summary>
        private Timer _playbackTimer;

        #endregion

        #region Properties

        /// <summary>
        /// Flag to set the application to ADMIN mode.
        /// </summary>
        private bool _IsAdmin;
        /// <summary>
        /// Flag to set the application to ADMIN mode.
        /// </summary>
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set
            {
                _IsAdmin = value;
                NotifyOfPropertyChange(() => IsAdmin);
            }
        }

        #region Playback 

        /// <summary>
        /// Turn on or off playback.
        /// </summary>
        private bool _IsPlayback;
        /// <summary>
        /// Turn on or off playback.
        /// </summary>
        public bool IsPlayback
        {
            get { return _IsPlayback; }
            set
            {
                _IsPlayback = value;
                NotifyOfPropertyChange(() => IsPlayback);

                if(_IsPlayback)
                {
                    StartPlayback();
                }
                else
                {
                    StopPlayback();
                }
            }
        }

        /// <summary>
        /// Number of ensembles in playback.
        /// </summary>
        private int _NumEnsembles;
        /// <summary>
        /// Number of ensembles in playback.
        /// </summary>
        public int NumEnsembles
        {
            get { return _NumEnsembles; }
            set
            {
                _NumEnsembles = value;
                NotifyOfPropertyChange(() => NumEnsembles);
            }
        }

        /// <summary>
        /// Lower Ensemble index to playback.
        /// </summary>
        private int _LowerSelectedEnsemble;
        /// <summary>
        /// Lower Ensemble index to playback.
        /// </summary>
        public int LowerSelectedEnsemble
        {
            get { return _LowerSelectedEnsemble; }
            set
            {
                _LowerSelectedEnsemble = value;
                NotifyOfPropertyChange(() => LowerSelectedEnsemble);
            }
        }

        /// <summary>
        /// Upper Ensemble index to playback.
        /// </summary>
        private int _UpperSelectedEnsemble;
        /// <summary>
        /// Upper Ensemble index to playback.
        /// </summary>
        public int UpperSelectedEnsemble
        {
            get { return _UpperSelectedEnsemble; }
            set
            {
                _UpperSelectedEnsemble = value;
                NotifyOfPropertyChange(() => UpperSelectedEnsemble);
            }
        }

        #endregion

        /// <summary>
        /// Set the status bar ViewModel.
        /// </summary>
        public StatusBarViewModel StatusBarVM { get; private set; }

        #endregion

        /// <summary>
        /// Initialize the application.
        /// </summary>
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            // Playback timer
            _playbackTimer = new Timer();
            _playbackTimer.Interval = 500;      // 0.5 seconds, 2 Hz
            _playbackTimer.Tick += _playbackTimer_Tick;

            // Set the mode
            IsAdmin = false;

            // Initialize the values for Playback
            IsPlayback = false;
            NumEnsembles = MIN_ENS_INDEX;
            LowerSelectedEnsemble = MIN_ENS_INDEX;
            UpperSelectedEnsemble = MIN_ENS_INDEX;

            // Setup the logger
            SetupErrorLog();

            // Initialize the status bar
            StatusBarVM = IoC.Get<StatusBarViewModel>();

            // Initialize to terminal view
            ActivateItem(IoC.Get<TerminalAdcpViewModel>());
        }

        /// <summary>
        /// Select the terminal view.
        /// </summary>
        public void TerminalView()
        {
            ActivateItem(IoC.Get<TerminalAdcpViewModel>());
        }

        /// <summary>
        /// Select the terminal view.
        /// </summary>
        public void DataFormatView()
        {
            ActivateItem(IoC.Get<DataFormatViewModel>());
        }

        /// <summary>
        /// Select the Dashboard view.
        /// </summary>
        public void DashboardView()
        {
            ActivateItem(IoC.Get<DashboardViewModel>());
        }

        /// <summary>
        /// Select the terminal view.
        /// </summary>
        public void GraphicalView()
        {
            ActivateItem(IoC.Get<HeatmapPlotViewModel>());
        }

        /// <summary>
        /// Call if closing the screen.
        /// </summary>
        /// <param name="close"></param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            foreach (var vm in IoC.GetAllInstances(typeof(IDisposable)))
            {
                ((IDisposable)vm).Dispose();
            }
        }

        #region Open Files

        /// <summary>
        /// Have the user select a file to playback.  Then set the 
        /// playback to the playback base in AdcpConnection.
        /// </summary>
        public void OpenFiles()
        {
            try
            {
                // Show the FolderBrowserDialog.
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Ensemble files (*.bin, *.ens)|*.bin; *.ens|BIN files (*.bin)|*.bin|ENS files (*.ens)|*.ens|All files (*.*)|*.*";
                dialog.Multiselect = true;
                //dialog.InitialDirectory = Pulse.Commons.DEFAULT_RECORD_DIR;

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Pass the files to all Playback layers
                    foreach (var vm in IoC.GetAllInstances(typeof(ILoadFilesLayer)))
                    {
                        ((ILoadFilesLayer)vm).LoadFiles(dialog.FileNames);
                    }
                }

                // Go to the dashboard
                ActivateItem(IoC.Get<DashboardViewModel>());
            }
            catch (AccessViolationException ae)
            {
                log.Error("Error trying to open file", ae);
            }
            catch (Exception e)
            {
                log.Error("Error trying to open file", e);
            }
        }

        #endregion

        #region Playback

        /// <summary>
        /// Start the playback.
        /// </summary>
        private void StartPlayback()
        {
            _playbackTimer.Start();
        }

        /// <summary>
        /// Stop the playback.
        /// </summary>
        private void StopPlayback()
        {
            _playbackTimer.Stop();
        }

        /// <summary>
        /// Timer to move to the next ensemble in the playback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _playbackTimer_Tick(object sender, EventArgs e)
        {
            foreach (var vm in IoC.GetAllInstances(typeof(IPlaybackLayer)))
            {
                // Playback the data
                ((IPlaybackLayer)vm).Playback(LowerSelectedEnsemble, UpperSelectedEnsemble);

                // Move the range
                LowerSelectedEnsemble++;
                UpperSelectedEnsemble++;
            }
        }

        #endregion

        #region Clear Plot

        /// <summary>
        /// Clear all the plots.
        /// </summary>
        public void ClearPlots()
        {
            // Clear all the plots in the dashboards
            IoC.Get<DashboardViewModel>().ClearPlots();
        }

        #endregion


        #region Error Logger

        /// <summary>
        /// Setup the error log.
        /// </summary>
        private void SetupErrorLog()
        {
            Hierarchy hierarchy = (Hierarchy)log4net.LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders(); /*Remove any other appenders*/

            FileAppender fileAppender = new FileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File = ERRORLOG_PATH;
            PatternLayout pl = new PatternLayout();
            string pulseVer = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            string rtiVer = System.Reflection.Assembly.LoadFrom("RTI.dll").GetName().Version.ToString();
            pl.ConversionPattern = "%d [%2%t] %-5p [%-10c] Pulse:" + pulseVer + " RTI:" + rtiVer + "   %m%n%n";
            pl.ActivateOptions();

            // If not Admin
            // Only log Error and Fatal errors
            if (IsAdmin)
            {
                fileAppender.AddFilter(new log4net.Filter.LevelMatchFilter() { LevelToMatch = log4net.Core.Level.Error });          // Log Error
                fileAppender.AddFilter(new log4net.Filter.LevelMatchFilter() { LevelToMatch = log4net.Core.Level.Fatal });          // Log Fatal
                fileAppender.AddFilter(new log4net.Filter.DenyAllFilter());                                                         // Reject all other errors
            }

            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(fileAppender);
        }

        /// <summary>
        /// Clear the Error Log.
        /// </summary>
        public void ClearErrorLog()
        {
            using (FileStream stream = new FileStream(ERRORLOG_PATH, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("");
                }
            }
        }

        #endregion

        #region Event Handler

        /// <summary>
        /// Handle the Playback Total ensemble event to get the number 
        /// of ensembles to playback.
        /// </summary>
        /// <param name="message">Event for number of esembles.</param>
        public void Handle(PlaybackTotalEnsemblesEvent message)
        {
            NumEnsembles = message.NumEnsembles;
            UpperSelectedEnsemble = message.NumEnsembles;
            LowerSelectedEnsemble = MIN_ENS_INDEX;
        }

        #endregion

    }
}