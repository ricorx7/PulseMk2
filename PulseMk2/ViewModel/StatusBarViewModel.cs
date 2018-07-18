using Caliburn.Micro;
using RTI.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public class StatusBarViewModel : Screen, IProcessEnsLayer, IProjectLayer, IDisposable, IHandle<StatusProgressBarEvent>, IHandle<StatusMessageEvent>
    {
        #region Variable

        /// <summary>
        /// Event manager.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Properties

        #region Project

        /// <summary>
        /// Selected project name.
        /// </summary>
        private string _ProjectName;
        /// <summary>
        /// Selected project name.
        /// </summary>
        public string ProjectName
        {
            get { return _ProjectName; }
            set
            {
                _ProjectName = value;
                NotifyOfPropertyChange(() => ProjectName);
            }
        }

        #endregion

        #region Status Progress Bar

        /// <summary>
        /// Minimum Progress bar value.
        /// </summary>
        private double _MinProgressBar;
        /// <summary>
        /// Minimum Progress bar value.
        /// </summary>
        public double MinProgressBar
        {
            get { return _MinProgressBar; }
            set
            {
                _MinProgressBar = value;
                NotifyOfPropertyChange(() => MinProgressBar);
            }
        }

        /// <summary>
        /// Maximum Progress bar value.
        /// </summary>
        private double _MaxProgressBar;
        /// <summary>
        /// Maximum Progress bar value.
        /// </summary>
        public double MaxProgressBar
        {
            get { return _MaxProgressBar; }
            set
            {
                _MaxProgressBar = value;
                NotifyOfPropertyChange(() => MaxProgressBar);
            }
        }

        /// <summary>
        /// Progress bar value.
        /// </summary>
        private double _ValueProgressBar;
        /// <summary>
        /// Progress bar value.
        /// </summary>
        public double ValueProgressBar
        {
            get { return _ValueProgressBar; }
            set
            {
                _ValueProgressBar = value;
                NotifyOfPropertyChange(() => ValueProgressBar);
            }
        }

        #endregion

        #region Status

        /// <summary>
        /// Status message.
        /// </summary>
        private string _Status;
        /// <summary>
        /// Status message.
        /// </summary>
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Initialize the view model.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            ProjectName = "";
            MinProgressBar = 0.0;
            MaxProgressBar = 100.0;
            ValueProgressBar = 0.0;
        }

        public void Dispose()
        {

        }

        #region Event Handler

        /// <summary>
        /// Load Project event handler.
        /// </summary>
        /// <param name="project"></param>
        public void LoadProject(Project project)
        {
            ProjectName = project.GetProjectFullPath();
        }

        /// <summary>
        /// Process the ensemble.
        /// Cause a blink.
        /// </summary>
        /// <param name="ensemble"></param>
        /// <param name="source"></param>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public void ProcessEnsemble(Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat)
        {
            // Cause a blink

        }

        /// <summary>
        /// Handle the progress bar.
        /// </summary>
        /// <param name="msg">Progress bar.</param>
        public void Handle(StatusProgressBarEvent msg)
        {
            // Progress bar values
            MinProgressBar = msg.Min;
            MaxProgressBar = msg.Max;
            ValueProgressBar = msg.Value;
        }

        /// <summary>
        /// Status message.
        /// </summary>
        /// <param name="message">Status message.</param>
        public void Handle(StatusMessageEvent message)
        {
            Status = message.Message;
        }

        #endregion

    }
}
