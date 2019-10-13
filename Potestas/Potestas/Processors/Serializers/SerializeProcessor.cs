using System;
using System.IO;
using System.Threading.Tasks;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    /* TASK. Implement processor which serializes IEnergyObservation to the provided stream.
     * 1. Use serialization mechanism here. 
     * 2. Some IEnergyObservation could not be serializable.
     */
    public abstract class SerializeProcessor<T> : IEnergyObservationProcessor<T> where T: IEnergyObservation
    {
        private Stream _stream;

        public Stream Stream
        {
            get => _stream;
            set => _stream = value ?? throw new ArgumentNullException(nameof(value));
        }

        public abstract string Description { get; }

        public void OnCompleted()
        {
            Console.WriteLine($"Serialize process {typeof(T)} completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Error appeared: {error}");
        }

        public virtual void OnNext(T value)
        {
            if (Stream == null)
                throw new ArgumentNullException(nameof(Stream));      
            if (!(Stream.CanRead && Stream.CanWrite))
                throw new ArgumentException(nameof(Stream));
        }

        protected async Task<string> ReadAllStream()
        {
            var reader = new StreamReader(Stream);
            var content = await reader.ReadToEndAsync().ConfigureAwait(false);
            reader.Dispose();
            return content;
        }

        protected async Task WriteToStream(string data)
        {
            var writer = new StreamWriter(Stream);
            await writer.WriteAsync(data).ConfigureAwait(false);
            writer.Dispose();
        }

    }
}
