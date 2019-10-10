using System;
using Potestas.Interfaces;

namespace Potestas.Processors
{
    public class SaveToStorageProcessor : IEnergyObservationProcessor<IEnergyObservation>
    {
        private readonly IEnergyObservationStorage _storage;

        public SaveToStorageProcessor(IEnergyObservationStorage storage)
        {
            _storage = storage;
        }

        public string Description => "Saves observations to provided storage";

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
            _storage.Add(value);
        }
    }
}
