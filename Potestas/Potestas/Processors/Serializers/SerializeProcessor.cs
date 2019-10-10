using System;
using System.IO;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    /* TASK. Implement processor which serializes IEnergyObservation to the provided stream.
     * 1. Use serialization mechanism here. 
     * 2. Some IEnergyObservation could not be serializable.
     */
    public abstract class SerializeProcessor<T> : BaseProcessor<T> where T: IEnergyObservation
    {
        private Stream _stream;

        public Stream Stream
        {
            get => _stream;
            set => _stream = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override void OnCompleted()
        {
            Console.WriteLine($"Serialize process {typeof(T)} completed");
        }

        public override void OnError(Exception error)
        {
            Console.WriteLine($"Error appeared: {error}");
        }
    }
}
