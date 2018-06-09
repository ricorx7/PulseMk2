using Caliburn.Micro;

namespace RTI {
    public class ShellViewModel : Conductor<object>, IShell
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
    }
}