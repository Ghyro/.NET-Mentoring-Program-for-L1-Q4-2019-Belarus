using Potestas.Analizers;
using Potestas.Storages;
using System;
using Potestas.Interfaces;
using Potestas.Sources;
using Potestas.Processors.Save;

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
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory());
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }
    }

    internal class ConsoleSourceFactory : ISourceFactory
    {
        public IEnergyObservationEventSource CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource()
        {
            return new RandomEnergySource();
        }
    }

    internal class ConsoleProcessingFactory : IProcessingFactory
    {
        public IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage)
        {
            return new LINQAnalizer(observationStorage);
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new ConsoleProcessor();
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToFileProcessor()
        {
            return new SaveToFileProcessor<IEnergyObservation>(@"C:\observaions.txt");           
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return new ListStorage();
        }
    }
}
