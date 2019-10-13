using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Potestas.Interfaces;

namespace Potestas.Storages
{
    /* TASK. Implement file storage
     */
    public class FileStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private string _filePath;
        private readonly List<T> _observation;

        public FileStorage(string filePath, List<T> collection)
        {
            _filePath = filePath;
            _observation = collection;
            ReadFromFile();
        }
        public T this[int index]
        {
            get => _observation[index];
            set => _observation[index] = value;
        }

        public string Description => "File storage";

        public int Count => _observation.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _observation.Add(item);
            WriteToFile();
        }

        public void Clear()
        {
            _observation.Clear();
            ClearFile();
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _observation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex > _observation.Count)
            {
                _observation.AddRange(array);
            }
            else
            {
                if (arrayIndex < 0)                
                    arrayIndex = 0;              
                _observation.InsertRange(arrayIndex, array);
            }

            WriteToFile();
        }

        public T GetByHash(int hashCode)
        {
            return _observation.SingleOrDefault(item => item.GetHashCode() == hashCode);
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

                _observation.Remove(removeItem);

                WriteToFile();

                return true;
            }
            catch
            {
                return false;
            }            
        }

        public IEnumerable<T> GetAll()
        {
            return _observation;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _observation.GetEnumerator();
        }

        #region private
        private bool ReadFromFile()
        {
            if (File.Exists(_filePath))
                using (var stream = new FileStream(_filePath, FileMode.OpenOrCreate))
                {
                    if (stream.Length <= 0)
                        return false;

                    var content = default(string);

                    using (var streamReader = new StreamReader(stream))
                    {
                        content = streamReader.ReadToEnd();
                    }

                    _observation.Clear();
                    _observation.AddRange(JsonConvert.DeserializeObject<List<T>>(content));
                    return true;
                }

            return false;
        }

        private void WriteToFile()
        {
            var fileMode = File.Exists(_filePath) ? FileMode.Truncate : FileMode.Create;

            using (var stream = new FileStream(_filePath, fileMode))
            {
                var content = JsonConvert.SerializeObject(_observation);
                using (var streamWriter = new StreamWriter(stream))
                    streamWriter.Write(content);
            }
        }

        private void ClearFile()
        {
            if (File.Exists(_filePath))
                using (var stream = new FileStream(_filePath, FileMode.Truncate)) { }
        }
        #endregion
    }
}
