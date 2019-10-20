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
            set
            {
                if (ReferenceEquals(value, null))                
                    throw new NullReferenceException(nameof(value));              
                _stream = value;
            }
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

        protected async Task<string> ReadAllStream(StreamReader reader)
        {
            var content = await reader.ReadToEndAsync().ConfigureAwait(false);
            return content;
        }

        protected async Task WriteToStream(StreamWriter writer, string data)
        {
            Stream.Position = 0;
            await writer.WriteAsync(data).ConfigureAwait(false);
            await Stream.FlushAsync().ConfigureAwait(false);
        }
    }
}
