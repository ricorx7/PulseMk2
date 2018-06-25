using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public class ProfilePlot3dViewModel : PlotViewModel, IDisposable
    {
        #region Properties

        /// <summary>
        /// 3D Velocity plot.
        /// </summary>
        public BinPlot3D VelPlot { get; set; }

        #endregion

        /// <summary>
        /// Initialize the plot.
        /// </summary>
        public ProfilePlot3dViewModel()
            : base("Profile Plot 3D")
        {
            VelPlot = new BinPlot3D();
            VelPlot.CylinderRadius = 0;
            VelPlot.ColormapBrushSelection = ColormapBrush.ColormapBrushEnum.Jet;
            VelPlot.MinVelocity = 0;
            VelPlot.MaxVelocity = 2;
        }

        public void Dispose()
        {

        }

        public void AddData(DataSet.EnsembleVelocityVectors ensVec)
        {

            Task.Run(() => VelPlot.AddIncomingData(ensVec));
        }

    }
}
