using System;
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
    public class SaveToFileProcessor<T> : BaseProcessor<T> where T: IEnergyObservation
    {
        protected BaseProcessor<T> BaseProcessor;
        private Stream _stream;
        public string FileName { get; set; }

        public override void OnCompleted()
        {
            Console.WriteLine("SaveToFileProcessor is completed");
        }

        public override void OnError(Exception error)
        {
            Console.WriteLine($"Error appeared: {error}");
        }

        public override async void OnNext(T value)
        {
            using (_stream = new FileStream(FileName, FileMode.Append))
            {
                if (ReferenceEquals(BaseProcessor, null))
                {
                    var data = value.ToString();
                    var bytes = Encoding.Default.GetBytes(data);
                    await _stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                }
                else
                {
                    if (BaseProcessor is SerializeProcessor<T> processor)
                        processor.Stream = _stream;

                    BaseProcessor.OnNext(value);
                }

                _stream.Close();
            }
        }

        public override string Description => "Save to file processor";
    }
}
