using Potestas.Interfaces;
using Potestas.Storages;
using System.Configuration;


namespace Potestas.ConcreteFactories
{
    public class FileStorageFactory<T> : IStorageFactory<T> where T : IEnergyObservation
    {    
        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new FileStorage<T>(ConfigurationManager.AppSettings.Get("storagePath"));
        }
    }
}
