using System;
using System.Collections.Generic;
using System.Text;

namespace Interactivity.Types
{
    public class InteractivityConfiguration
    {
        public TimeSpan DefaultTimeOutTime { get; set; }
        public string CommandPrefix { get; set; } = "/";
    }
}
