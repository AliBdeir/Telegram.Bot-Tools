using System;
using System.Collections.Generic;
using System.Text;

namespace Interactivity.Types
{
    public class InteractivityResult<T>
    {
        public T Value { get; set; }
        public bool TimedOut { get; set; }
        public InteractivityResult(T value, bool timedOut)
        {
            this.Value = value;
            this.TimedOut = timedOut;
        }

    }
}
