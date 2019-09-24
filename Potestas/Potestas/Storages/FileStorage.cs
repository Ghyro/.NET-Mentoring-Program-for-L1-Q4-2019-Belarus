using System;
using System.Collections;
using System.Collections.Generic;

namespace Potestas.Storages
{
    /* TASK. Implement file storage
     */
    class FileStorage : IEnergyObservationStorage
    {
        public string Description => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(IEnergyObservation item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IEnergyObservation item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IEnergyObservation[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEnergyObservation> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(IEnergyObservation item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
