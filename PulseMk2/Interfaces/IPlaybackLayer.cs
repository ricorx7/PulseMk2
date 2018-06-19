using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    public interface IPlaybackLayer
    {
        /// <summary>
        /// Load the file.
        /// </summary>
        /// <param name="filePath"></param>
        Project LoadFiles(string[] files);
    }
}
