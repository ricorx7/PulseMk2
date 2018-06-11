using Caliburn.Micro;
using System;

namespace RTI {
    public class ShellViewModel : Conductor<object>, IShell, IDeactivate
    {
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
    }
}