using System;
using System.Configuration;
using System.IO;
using System.Text;
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
        protected IEnergyObservationProcessor<T> _processor;
        private Stream _stream;
        public string _fileName { get; set; }

        public string FileName
        {
            get => _fileName ?? ConfigurationManager.AppSettings.Get("processorPath");
            set => _fileName = value;
        }

        public SaveToFileProcessor(IEnergyObservationProcessor<T> processor)
        {
            _processor = processor;
        }

        public void OnCompleted()
        {
            Console.WriteLine("SaveToFileProcessor is completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Error appeared: {error}");
        }

        public async void OnNext(T value)
        {
            using (_stream = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                if (ReferenceEquals(_processor, null))
                {
                    var data = value.ToString();
                    var bytes = Encoding.Default.GetBytes(data);
                    await _stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                }
                else
                {
                    if (_processor is SerializeProcessor<T> processor)
                        processor.Stream = _stream;

                    _processor.OnNext(value);
                }

                _stream.Close();
            }
        }

        public string Description => "Save to file processor";
    }
}
