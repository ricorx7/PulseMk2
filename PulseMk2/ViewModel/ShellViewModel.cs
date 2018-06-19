using Caliburn.Micro;
using System;
using System.Windows.Forms;

namespace RTI {
    public class ShellViewModel : Conductor<object>, IShell, IDeactivate
    {
        #region Variables

        /// <summary>
        ///  Setup logger
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        /// <summary>
        /// Initialize the application.
        /// </summary>
        public ShellViewModel()
        {
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

        /// <summary>
        /// Have the user select a file to playback.  Then set the 
        /// playback to the playback base in AdcpConnection.
        /// </summary>
        public void PlaybackFile()
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
                    foreach (var vm in IoC.GetAllInstances(typeof(IPlaybackLayer)))
                    {
                        ((IPlaybackLayer)vm).LoadFiles(dialog.FileNames);
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
    }
}