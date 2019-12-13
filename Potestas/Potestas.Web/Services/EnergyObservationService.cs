using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Storages;
using Potestas.Web.Interfaces;
using Potestas.Web.Models;

namespace Potestas.Web.Services
{
    public class EnergyObservationService : IEnergyObservationService
    {
        private readonly IEnergyObservationStorage<IEnergyObservation> _storage;
        private readonly IMapper _mapper;

        public EnergyObservationService(IEnergyObservationStorage<IEnergyObservation> storage, IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlashObservationViewModel>> GetAllObservationsAsync()
        {
            if (_storage is BsonStorage<IEnergyObservation> bsonStorage)
            {
                var observations = await Task.Run(() => bsonStorage.GetObservations());

                return observations.Select(obs => _mapper.Map<FlashObservationViewModel>(obs)).ToList();
            }

            return null;
        }

        public async Task AddObservationAsync(FlashObservationViewModel flashObservation)
        {
            if (_storage is BsonStorage<IEnergyObservation> bsonStorage)
            {
                var observartion = _mapper.Map<FlashObservation>(flashObservation);

                await Task.Run(() => bsonStorage.Add(observartion));
            }
        }

        public async Task DeleteObservationAsync(FlashObservationViewModel flashObservation)
        {
            if (_storage is BsonStorage<IEnergyObservation> bsonStorage)
            {
                var observartion = _mapper.Map<FlashObservation>(flashObservation);

                await Task.Run(() => bsonStorage.Remove(observartion));
            }
        }

        public async Task ClearObservationsAsync()
        {
            if (_storage is BsonStorage<IEnergyObservation> bsonStorage)
                await Task.Run(() => bsonStorage.Clear());
        }
    }
}
