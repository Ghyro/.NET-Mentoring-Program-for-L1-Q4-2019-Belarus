using System;

namespace Potestas.Sources.Events
{
    public class ValueObservationErrorEventArgs : EventArgs     
    {
        public Exception Exception { get; }

        public ValueObservationErrorEventArgs(Exception e)
        {
            Exception = e;
        }
    }
}
