﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;

namespace Potestas
{
    /* TASK. Try to refactor this code and add proper exception handling
     * 
     */

    internal class RegisteredSourceProcessingGroup : IProcessingGroup<IEnergyObservation>
    {
        private readonly RegisteredEnergyObservationSourceWrapper _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        public IEnergyObservationStorage<IEnergyObservation> Storage { get; }

        public IEnergyObservationAnalizer<IEnergyObservation> Analizer { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper sourceRegistration,
            IProcessingFactory<IEnergyObservation> factory,
            IStorageFactory<IEnergyObservation> storageFactory,
            IAnalizerFactory<IEnergyObservation> analizerFactory)
        {
            _sourceRegistration = sourceRegistration;            
            Storage = storageFactory.CreateListStorage();
            Processor = factory.CreateSaveToStorageProcessor(Storage);
            Analizer = analizerFactory.CreateAnalizer(Storage);
        }

        public void Detach()
        {
            _processorSubscription.Dispose();
            _sourceRegistration.RemoveProcessingGroup(this);
        }
    }

    internal class RegisteredEnergyObservationSourceWrapper : ISourceRegistration, IEnergyObservationProcessor<IEnergyObservation>
    {
        private readonly ApplicationFrame _app;
        private readonly IEnergyObservationSource _inner;
        private readonly IDisposable _internalSubscription;
        private readonly List<IProcessingGroup<IEnergyObservation>> _processingGroups;
        private CancellationTokenSource _cts;

        public RegisteredEnergyObservationSourceWrapper(ApplicationFrame app, IEnergyObservationSource inner)
        {
            _app = app;
            _inner = inner;
            _processingGroups = new List<IProcessingGroup<IEnergyObservation>>();
            Subscribe(this);
        }

        public SourceStatus Status { get; private set; }

        public IReadOnlyCollection<IProcessingGroup<IEnergyObservation>> ProcessingUnits => _processingGroups.AsReadOnly();

        public string Description => "Internal application listener to track Sources State";

        internal IDisposable Subscribe(IEnergyObservationProcessor<IEnergyObservation> processor)
        {
            return _inner.Subscribe(processor);
        }

        public IProcessingGroup<IEnergyObservation> AttachProcessingGroup(IProcessingFactory<IEnergyObservation> factory,
            IStorageFactory<IEnergyObservation> storageFactory,
            IAnalizerFactory<IEnergyObservation> analizerFactory
)
        {                                                                                                           
            var processingGroup = new RegisteredSourceProcessingGroup(this, factory, storageFactory, analizerFactory);
            _processingGroups.Add(processingGroup);
            return processingGroup;
        }

        internal void RemoveProcessingGroup(IProcessingGroup<IEnergyObservation> processingGroup)
        {
            _processingGroups.Remove(processingGroup);
        }

        public void OnCompleted() => Status = SourceStatus.Completed;

        public void OnError(Exception error) => Status = SourceStatus.Failed;

        public void OnNext(IEnergyObservation value) => Status = SourceStatus.Running;

        public Task Start()
        {
            // TODO: add SemaphoreSlim to prevent multiple runs
            _cts = new CancellationTokenSource();
            return _inner.Run(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void Unregister()
        {
            _internalSubscription.Dispose();
            _app.RemoveRegistration(this);
        }
    }

    public sealed class ApplicationFrame : IEnergyObservationApplicationModel
    {
        private static readonly FactoriesLoader _factoriesLoader = new FactoriesLoader();

        private readonly List<ISourceFactory<IEnergyObservation>> _sourceFactories;
        private readonly List<IProcessingFactory<IEnergyObservation>> _processingFactories;
        private readonly List<RegisteredEnergyObservationSourceWrapper> _registeredSources;
        private readonly List<IAnalizerFactory<IEnergyObservation>> _analizerFactories;
        private readonly List<IStorageFactory<IEnergyObservation>> _storageFactories;

        public IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories => _sourceFactories.AsReadOnly();
        public IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories => _processingFactories.AsReadOnly();
        public IReadOnlyCollection<ISourceRegistration> RegisteredSources => _registeredSources.AsReadOnly();
        public IReadOnlyCollection<IStorageFactory<IEnergyObservation>> StorageFactories => _storageFactories.AsReadOnly();
        public IReadOnlyCollection<IAnalizerFactory<IEnergyObservation>> AnalizerFactories => _analizerFactories.AsReadOnly();

        public ApplicationFrame()
        {
            _registeredSources = new List<RegisteredEnergyObservationSourceWrapper>();
            _processingFactories = new List<IProcessingFactory<IEnergyObservation>>();
            _sourceFactories = new List<ISourceFactory<IEnergyObservation>>();
            _storageFactories = new List<IStorageFactory<IEnergyObservation>>();
            _analizerFactories = new List<IAnalizerFactory<IEnergyObservation>>();
        }

        public void LoadPlugin(Assembly assembly)
        {
            var (sourceFactories, processingFactories, storageFactories, analizerFactories) = _factoriesLoader.Load(assembly);
            _processingFactories.AddRange(processingFactories);
            _sourceFactories.AddRange(sourceFactories);
            _storageFactories.AddRange(storageFactories);
            _analizerFactories.AddRange(analizerFactories);
        }

        public ISourceRegistration CreateAndRegisterSource(ISourceFactory<IEnergyObservation> factory)
        {
            var source = factory.CreateSource();
            var registration = new RegisteredEnergyObservationSourceWrapper(this, source);
            _registeredSources.Add(registration);
            return registration;
        }

        internal void RemoveRegistration(RegisteredEnergyObservationSourceWrapper registration)
        {
            _registeredSources.Remove(registration);
        }
    }
}
