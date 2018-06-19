using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    /// <summary>
    /// Create a unique value for the subsystem configuration.
    /// </summary>
    public class ViewSubsystemConfig
    {
        /// <summary>
        /// Subsystem Configuration.
        /// </summary>
        public SubsystemConfiguration Config { get; set; }

        /// <summary>
        /// Ensemble source.
        /// </summary>
        public EnsembleSource Source { get; set; }

        /// <summary>
        /// Initalize.
        /// </summary>
        public ViewSubsystemConfig()
        {
            Config = new SubsystemConfiguration();
            Source = EnsembleSource.Serial;
        }

        /// <summary>
        /// Initialize the values.
        /// </summary>
        /// <param name="config">Subystem configuration.</param>
        /// <param name="source">Ensemble source.</param>
        public ViewSubsystemConfig(SubsystemConfiguration config, EnsembleSource source)
        {
            Config = config;
            Source = source;
        }

        /// <summary>
        /// Check if they are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (Config.CepoIndex == ((ViewSubsystemConfig)obj).Config.CepoIndex &&
                Config.SubSystem == ((ViewSubsystemConfig)obj).Config.SubSystem &&
                Source == ((ViewSubsystemConfig)obj).Source);
        }

        public static bool operator ==(ViewSubsystemConfig obj1, ViewSubsystemConfig obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ViewSubsystemConfig obj1, ViewSubsystemConfig obj2)
        {
            return !Equals(obj1, obj2);
        }


        public override int GetHashCode()
        {
            return Config.GetHashCode() + Source.GetHashCode();
        }

    }
}
