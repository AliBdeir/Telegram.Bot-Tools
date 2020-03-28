using System;
using System.Collections.Generic;
using System.Text;

namespace Interactivity.Models
{
    public class InteractivityResult<T>
    {
        public T Result { get; set; }
        public bool TimedOut { get; set; }
    }
}
