using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Potestas.Observations;
using Potestas.Storages;
using System.Collections.Generic;

namespace Potestas.WebHTTP.Controllers
{
    public class FlashObservationController : Controller
    {
        private readonly BsonStorage<FlashObservation> _storage;
        private readonly IMapper _mapper;

        public FlashObservationController()
        {
            _storage = new BsonStorage<FlashObservation>();
            _mapper = MapperConfig.CreateMapper();
        }

        [HttpGet]
        public ActionResult<List<Models.FlashObservation>> GetAll()
        {
            var enumerator = _storage.GetEnumerator();
            var flashObservations = new List<Models.FlashObservation>();

            while (enumerator.MoveNext())
            {
                flashObservations.Add(_mapper.Map<Models.FlashObservation>(enumerator.Current));
            }

            return flashObservations;
        }

        [HttpPost]
        public void Create(Models.FlashObservation item)
        {
            _storage.Add(_mapper.Map<FlashObservation>(item));
            RedirectToAction(nameof(GetAll));
        }

        [HttpDelete]
        public void DeleteById(Models.FlashObservation item)
        {
            _storage.Remove(_mapper.Map<FlashObservation>(item));
            RedirectToAction(nameof(GetAll));
        }

        [HttpDelete]
        public void DeleteAll()
        {
            _storage.Clear();
            RedirectToAction(nameof(GetAll));
        }
    }
}