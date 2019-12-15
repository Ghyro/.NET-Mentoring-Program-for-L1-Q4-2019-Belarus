using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public EnergyObservationService(IEnergyObservationStorage<IEnergyObservation> storage, IMapper mapper, IMemoryCache cache)
        {
            _storage = storage;
            _mapper = mapper;
            _cache = cache;
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

                _cache.Set(flashObservation.Id, flashObservation, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
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
