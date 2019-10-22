using System;
using System.IO;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Processors.Serializers;
using Potestas.Sources;

namespace Potestas.Apps.Terminal
{
    internal static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;

        static Program()
        {
            _app = new ApplicationFrame();
        }

        private static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory(), null, null);
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }
    }

    internal class ConsoleSourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource CreateSource()
        {
            return new RandomEnergySource();
        }
    }

    internal class ConsoleProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToStorageProcessor(IEnergyObservationStorage<IEnergyObservation> observationStorage)
        {
            return new SaveToStorageProcessor<IEnergyObservation>(observationStorage);
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateSerializeProcessor(Stream stream)
        {
            return new SerializeProcessor<IEnergyObservation>(stream);
        }
    }
}
