using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Potestas.Interfaces;
using Potestas.Observations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Potestas.WebHTTP
{
    public class WebStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly List<FlashObservation> _observations;
        private readonly string _url = "http://localhost:6836/api/flashobservation";
        private readonly IMapper mapper = MapperConfig.CreateMapper();


        public WebStorage()
        {
            _observations = new List<FlashObservation>();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _observations.GetEnumerator();
        }

        public void Add(T item)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/Create"))
            {
                var json = JsonConvert.SerializeObject(item);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                }
            }
        }

        public void Clear()
        {
            _observations.Clear();
            ClearDatabaseTable();
        }

        public bool Contains(T item)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/get"))
            {
                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<Models.FlashObservation>>(response.Content.ReadAsStringAsync().Result);

                    var searchItem = mapper.Map<Models.FlashObservation>(item);

                    return list.FindAll(x => x.DurationMs == searchItem.DurationMs
                        && x.EstimatedValue == searchItem.EstimatedValue
                        && x.Intensity == searchItem.Intensity
                        && x.ObservationPoint.X == searchItem.ObservationPoint.X
                        && x.ObservationPoint.Y == searchItem.ObservationPoint.Y
                        && x.ObservationTime == searchItem.ObservationTime).Any();
                }
                else
                {
                    return false;
                }
            }

        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex > _observations.Count)
            {
                _observations.AddRange((FlashObservation[])(object)array);
            }
            else
            {
                if (arrayIndex < 0)
                    arrayIndex = 0;
                _observations.InsertRange(arrayIndex, (FlashObservation[])(object)array);
            }
        }

        public bool Remove(T item)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{_url}/deletebyid"))
            {
                var json = JsonConvert.SerializeObject(item);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        public int Count => _observations.Count;

        public bool IsReadOnly => false;

        public string Description => "WebStorage";

        public IEnumerable<T> GetAll()
        {
            return (IEnumerable<T>)_observations;
        }

        public T GetByHash(int hashCode)
        {
            return (T)(object)_observations.SingleOrDefault(item => item.GetHashCode() == hashCode);
        }

        #region private
        
        private void ClearDatabaseTable()
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{_url}/deleteall"))
            {
                var response = client.SendAsync(request).GetAwaiter().GetResult();
            }
        }

        #endregion
    }
}
