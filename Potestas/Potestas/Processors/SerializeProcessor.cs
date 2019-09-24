using System;

namespace Potestas.Processors
{
    /* TASK. Implement processor which serializes IEnergyObservation to the provided stream.
     * 1. Use serialization mechanism here. 
     * 2. Some IEnergyObservation could not be serializable.
     */
    public class SerializeProcessor : IEnergyObservationProcessor
    {
        public string Description => throw new NotImplementedException();

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IEnergyObservation value)
        {
            throw new NotImplementedException();
        }
    }
}
