using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    /// <summary>
    /// Status message event.
    /// </summary>
    public class StatusMessageEvent
    {
        /// <summary>
        /// Status message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initialize the message.
        /// </summary>
        /// <param name="msg"></param>
        public StatusMessageEvent(string msg)
        {
            Message = msg;
        }

    }
}
