using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTI.DataSet;

namespace RTI
{
    public class DashboardViewModel : Caliburn.Micro.Screen, IProcessEnsLayer
    {
        #region Variable

        /// <summary>
        /// Dictionary to hold the subsystem config and the view model.
        /// </summary>
        private Dictionary<ViewSubsystemConfig, DashboardSubsystemConfigViewModel> _dictSsConfig;

        /// <summary>
        /// Collection of all the subsytem configurations.
        /// </summary>
        public ObservableCollection<DashboardSubsystemConfigViewModel> SsConfigList { get; set; }

        #endregion


        /// <summary>
        /// Initialize.
        /// </summary>
        public DashboardViewModel()
        {
            // Create the dict
            _dictSsConfig = new Dictionary<ViewSubsystemConfig, DashboardSubsystemConfigViewModel>();

            // Create the list to display all the views
            SsConfigList = new ObservableCollection<DashboardSubsystemConfigViewModel>();
        }


        public int ProcessEnsemble(Ensemble ensemble, EnsembleSource source, AdcpCodec.CodecEnum dataFormat)
        {
            if (ensemble != null && ensemble.IsEnsembleAvail)
            {
                // Create subsystem config
                ViewSubsystemConfig config = new ViewSubsystemConfig(ensemble.EnsembleData.SubsystemConfig, source);

                // Check if the config exist already
                if (!_dictSsConfig.ContainsKey(config))
                {
                    // Create the viewmodel for each subsystem config/source found
                    DashboardSubsystemConfigViewModel vm = new DashboardSubsystemConfigViewModel(config);

                    // Add the vm to the list
                    _dictSsConfig.Add(config, vm);
                    SsConfigList.Add(vm);

                    // Pass the ensemble to the viewmodel
                    vm.ProcessEnsemble(ensemble, source);

                    return 0;
                }
                else
                {
                    // Viewmodel already exist, so send the ensemble
                    DashboardSubsystemConfigViewModel vm = null;
                    if(_dictSsConfig.TryGetValue(config, out vm))
                    {
                        vm.ProcessEnsemble(ensemble, source);
                    }

                    return 0;
                }
            }

            // Not a valid ensemble
            return -1;
        }
    }
}
