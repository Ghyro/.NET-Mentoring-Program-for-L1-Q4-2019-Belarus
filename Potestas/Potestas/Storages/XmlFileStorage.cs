using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Storages
{
    public class XmlFileStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly string _filePath;
        private readonly List<FlashObservation> _observations;
        private readonly XmlSerializer _xmlSerializer;

        public XmlFileStorage()
        {
            _xmlSerializer = new XmlSerializer(typeof(List<FlashObservation>));
            _filePath = ConfigurationManager.AppSettings["xmlStoragePath"];
            _observations = new List<FlashObservation>();
            ReadFromFile();
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
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _observations.Add((FlashObservation)(object)item);
            WriteToFile();
        }

        public void Clear()
        {
            _observations.Clear();
            ClearFile();
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _observations.Contains((FlashObservation)(object)item);
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

            WriteToFile();
        }

        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                var removeItem = GetByHash(item.GetHashCode());

                if (removeItem == null)
                    throw new ArgumentNullException(nameof(item));

                _observations.Remove((FlashObservation)(object)removeItem);

                WriteToFile();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Count => _observations.Count;

        public bool IsReadOnly => false;

        public string Description => "XmlFileStorage";

        public IEnumerable<T> GetAll()
        {
            return (IEnumerable<T>)_observations;
        }

        public T GetByHash(int hashCode)
        {
            return (T)(object)_observations.SingleOrDefault(item => item.GetHashCode() == hashCode);
        }

        #region private

        private void ReadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            using (var stream = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                if (stream.Length <= 0)
                    return;

                using (var streamReader = new StreamReader(stream))
                {
                    _observations.Clear();

                    var flashObservations = (List<FlashObservation>)_xmlSerializer.Deserialize(streamReader);

                    _observations.AddRange(flashObservations);
                }
            }
        }

        private void WriteToFile()
        {
            var fileMode = File.Exists(_filePath) ? FileMode.Truncate : FileMode.Create;

            using (var stream = new FileStream(_filePath, fileMode))
            using (var streamWriter = new StreamWriter(stream))
                _xmlSerializer.Serialize(streamWriter, _observations);            
        }

        private void ClearFile()
        {
            if (!File.Exists(_filePath))
                return;
            using (new FileStream(_filePath, FileMode.Truncate)) { }
        }

        #endregion
    }
}
