using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public interface IProjectLayer
    {

        /// <summary>
        /// Load a project.
        /// </summary>
        /// <param name="project">Project to load.</param>
        void LoadProject(Project project);
    }
}
