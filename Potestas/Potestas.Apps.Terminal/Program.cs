﻿using System;
using Potestas.Analizers;
using Potestas.ConcreteFactories;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Sources;
using Potestas.Storages;

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
            _testRegistration.AttachProcessingGroup(new SaveToFileProcessorFactory());
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
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource CreateSource()
        {
            return new RandomEnergySource();
        }
    }

    internal class ConsoleProcessingFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage = null;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalyzer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new ConsoleProcessor();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new ListStorage<IEnergyObservation>();
            return _storage;
        }
    }
}
