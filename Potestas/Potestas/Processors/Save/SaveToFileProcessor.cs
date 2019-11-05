using System;
using System.Configuration;
using System.IO;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;

namespace Potestas.Processors.Save
{
    /* TASK. Implement Processor which saves IEnergyObservation to the provided file.
     * 1. Try to decorate SerializeProcessor.
     * QUESTIONS:
     * Which bonuses does decoration have?
     * TEST: Which kind of tests should be written for this class?
     */
    public class SaveToFileProcessor<T> : IEnergyObservationProcessor<T> where T: IEnergyObservation
    {
        private string _filePath;
        private IEnergyObservation _observation;
        private FileStream _fstream;
        private IDisposable cancellation;
        private SerializeProcessor<IEnergyObservation> _serializeProcessor;
        private Stream _stream;

        public string FilePath
        {
            get => _filePath ?? ConfigurationManager.AppSettings["xmlProcessorPath"];
            set => _filePath = value;
        }

        public SaveToFileProcessor(SerializeProcessor<IEnergyObservation> serializeProcessor)
        {
            _serializeProcessor = serializeProcessor;
            FilePath = ConfigurationManager.AppSettings["xmlProcessorPath"];
        }    

        public void OnCompleted()
        {
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            _fstream?.Close();
        }

        public async void OnNext(T value)
        {
            if (ReferenceEquals(_serializeProcessor, null))
            {
                using (_stream = new FileStream(FilePath, FileMode.Append))
                {
                    using (var writer = new StreamWriter(_stream))
                    {
                        await writer.WriteLineAsync(value.ToString()).ConfigureAwait(false);

                        await writer.FlushAsync().ConfigureAwait(false);
                    }
                }
            }
            else
            {
                using (_stream = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    if (_serializeProcessor is SerializeProcessor<T>)                    
                        _serializeProcessor.Stream = _stream;                    

                    _serializeProcessor.OnNext(value);

                    _stream.Close();
                }
            }
        }

        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            _observation = null;
            _fstream?.Close();
            _serializeProcessor = null;
        }
        
        public string Description => "SaveToFileProcessor";
    }
}

