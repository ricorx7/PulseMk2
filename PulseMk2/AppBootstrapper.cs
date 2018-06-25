namespace RTI {
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IShell, ShellViewModel>();
            container.Singleton<HomeViewModel, HomeViewModel>();

            container.Singleton<TerminalAdcpViewModel, TerminalAdcpViewModel>();
            var termAdcpVM = container.GetInstance<TerminalAdcpViewModel>();
            container.Instance<IDisposable>(termAdcpVM);                                // Dispose Interface

            // DataFormat VM with ICodecLayer
            container.Singleton<DataFormatViewModel, DataFormatViewModel>();
            var dataFormatVM = container.GetInstance<DataFormatViewModel>();
            container.Instance<ICodecLayer>(dataFormatVM);                              // Codec Layer Interface
            container.Instance<IPlaybackLayer>(dataFormatVM);                              // File Playback Layer Interface
            container.Instance<IDisposable>(dataFormatVM);                              // Dispose Interface

            container.Singleton<DashboardViewModel, DashboardViewModel>();
            var dashVM = container.GetInstance<DashboardViewModel>();
            container.Instance<IProcessEnsLayer>(dashVM);                               // Process Ensemble Layer interface
            container.Instance<IProjectLayer>(dashVM);                                  // Project Layer interface
            container.Instance<IDisposable>(dashVM);                                    // Disposable interface

            container.PerRequest<HeatmapPlotViewModel, HeatmapPlotViewModel>();
            var heatmapVM = container.GetInstance<HeatmapPlotViewModel>();
            container.Instance<IPlotLayer>(heatmapVM);                                  // Plot Layer interface

            container.PerRequest<TimeSeriesViewModel, TimeSeriesViewModel>();
            var timeseriesVM = container.GetInstance<TimeSeriesViewModel>();
            container.Instance<IPlotLayer>(timeseriesVM);                               // Plot Layer interface

            container.PerRequest<ShipTrackPlotViewModel, ShipTrackPlotViewModel>();
            var shiptrackVM = container.GetInstance<ShipTrackPlotViewModel>();
            container.Instance<IPlotLayer>(shiptrackVM);                                // Plot Layer interface

            container.PerRequest<CompassRoseViewModel, CompassRoseViewModel>();
            //var compassVM = container.GetInstance<CompassRoseViewModel>();
            ////container.Instance<IPlotLayer>(compassVM);                                // Plot Layer interface
            //container.Instance<IDisposable>(compassVM);                                 // Dispose Interface

            container.PerRequest<ProfilePlot3dViewModel, ProfilePlot3dViewModel>();
        }

        protected override object GetInstance(Type service, string key) {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}