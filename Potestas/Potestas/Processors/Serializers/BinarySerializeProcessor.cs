using System.Runtime.Serialization.Formatters.Binary;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    public class BinarySerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        private readonly BinaryFormatter _binaryFormatter;

        public BinarySerializeProcessor()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public override void OnNext(T value)
        {
            using (Stream)
            {
                if (Stream.CanWrite)
                    _binaryFormatter.Serialize(Stream, value);
            }
        }

        public override string Description => "Binary formatter processor";
    }
}
