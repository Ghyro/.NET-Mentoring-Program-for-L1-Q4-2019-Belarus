using Potestas.Observations;
using System;

namespace Potestas.Sources.Events
{
    public class NewValueObservedEventArgs : EventArgs
    {
        public FlashObservation FlashObservation { get; }

        public NewValueObservedEventArgs(FlashObservation flashObservation)
        {
            FlashObservation = flashObservation;
        }
    }
}
