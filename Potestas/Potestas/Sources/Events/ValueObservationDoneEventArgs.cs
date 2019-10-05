using System;
using System.Collections.Generic;
using System.Text;

namespace Potestas.Sources.Events
{
    public class ValueObservationDoneEventArgs : EventArgs
    {
        public string Message { get; }

        public ValueObservationDoneEventArgs(string message)
        {
            Message = message;
        }
    }
}
