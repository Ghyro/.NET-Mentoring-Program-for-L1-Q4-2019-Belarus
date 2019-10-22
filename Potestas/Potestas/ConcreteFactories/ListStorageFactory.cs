using System;
using System.Collections.Generic;
using System.Text;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{

    public class ListStorageFactory<T> : IStorageFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new ListStorage<T>();
        }
    }
}
