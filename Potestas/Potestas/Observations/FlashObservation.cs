using System;

namespace Potestas.Observations
{
    /* TASK: Implement this structure by following requirements:
    * 1. EstimatedValue is the intensity multiple by duration
    * 2. Observations are equal if they made at the same time, 
    * the same observation point and EstimatedValue 
    * is the same by decimal presicion
    * 3. Implement custom constructors with ability to set ObservationTime by moment of creation or from constructor parameter.
    * 4. Implement == and != operators for the structure.
    * 6. Negative Intensity is a sign of invalid observation. Figure out how to process such errors. Remember you are writing a library.
    * 7. Intensity more than 2 000 000 000 is imposible and could be a sign of the invalid observation.
    * 8. Implement nice string representation of this observation.
    * QUESTIONS: 
    * How implementation of interface impacts boxing and unboxing operation for the structure?
    * Why overriding of Equals method is not enough?
    * What kind of pollymorhism does this struct contain?
    * Why immutable structure is used here?
    * TESTS: Cover this structure with unit tests
    */
    public struct FlashObservation : IEnergyObservation
    {
        public Coordinates ObservationPoint { get; }

        public double Intensity { get; }

        public int DurationMs { get; }

        public DateTime ObservationTime { get; }

        public double EstimatedValue => throw new NotImplementedException();
    }
}
