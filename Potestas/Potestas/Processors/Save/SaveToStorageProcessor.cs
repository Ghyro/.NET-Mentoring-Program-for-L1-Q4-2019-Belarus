using System;
using Potestas.Interfaces;

namespace Potestas.Processors.Save
{
    public class SaveToStorageProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<IEnergyObservation> _storage;

        public SaveToStorageProcessor(IEnergyObservationStorage<IEnergyObservation> storage)
        {
            _storage = storage;
        }

        public string Description => "Saves observations to provided storage";

        public void OnCompleted()
        {
            Console.WriteLine("SaveToStorageProcessor is completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Error appeared: {error}");
        }

        public void OnNext(T value)
        {
            _storage.Add(value);
        }
    }
}
