using Potestas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Potestas
{
    /* TASK. Implement method Load to load factories interfaces from assembly provided.
     * 1. Consider some classes could be private.
     * 2. Consider using special attribute to exclude some factories from creation.
     * 3. Consider refactoring of factory interfaces.
     * 4. Consider making an extension for Assembly class.
     */
    class FactoriesLoader
    {
        public (ISourceFactory<IEnergyObservation>[], IProcessingFactory<IEnergyObservation>[],
                IStorageFactory<IEnergyObservation>[], IAnalizerFactory<IEnergyObservation>[]) Load(Assembly assembly)
        {
            if (assembly == null)            
                throw new ArgumentNullException(nameof(assembly));            

            var types = assembly.GetTypes();

            var sourceFactoryClasses = new List<ISourceFactory<IEnergyObservation>>();
            var processingFactoryClasses = new List<IProcessingFactory<IEnergyObservation>>();
            var storageFactoryClasses = new List<IStorageFactory<IEnergyObservation>>();
            var analizerFactoryClasses = new List<IAnalizerFactory<IEnergyObservation>>();

            foreach (var type in types)
            {
                var isAppropriateType = !type.IsAbstract && !type.IsInterface && type.IsPublic;               

                IsApprociateType(isAppropriateType, type,sourceFactoryClasses, processingFactoryClasses,
                                 storageFactoryClasses, analizerFactoryClasses);
            }

            return (sourceFactoryClasses.ToArray(), processingFactoryClasses.ToArray(),
                    storageFactoryClasses.ToArray(), analizerFactoryClasses.ToArray());
        }

        private bool IsImplementInterface<U>(Type type)
        {
            var found = type.FindInterfaces(new TypeFilter(InterfaceFilter), typeof(U).ToString());

            return found != null && found.Any();
        }

        private T GetFactoryInstance<T>(Type type)
        {
            if (type.IsGenericType)            
                type = type.MakeGenericType(new Type[] { typeof(IEnergyObservation) });

            T instance = (T)Activator.CreateInstance(type);

            return instance;
        }

        private bool InterfaceFilter(Type typeObj, object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        private void IsApprociateType(bool isAppropriateType, Type type, List<ISourceFactory<IEnergyObservation>> source,
            List<IProcessingFactory<IEnergyObservation>> processing, List<IStorageFactory<IEnergyObservation>> storage,
            List<IAnalizerFactory<IEnergyObservation>> analizer)
        {
            object instance = null;

            if (isAppropriateType && IsImplementInterface<ISourceFactory<IEnergyObservation>>(type))
                instance = GetFactoryInstance<ISourceFactory<IEnergyObservation>>(type);
            source.Add((ISourceFactory<IEnergyObservation>)instance);

            if (isAppropriateType && IsImplementInterface<IProcessingFactory<IEnergyObservation>>(type))
                if (instance == null)
                    instance = GetFactoryInstance<IProcessingFactory<IEnergyObservation>>(type);
            processing.Add((IProcessingFactory<IEnergyObservation>)instance);

            if (isAppropriateType && IsImplementInterface<IStorageFactory<IEnergyObservation>>(type))
                if (instance == null)
                    instance = GetFactoryInstance<IStorageFactory<IEnergyObservation>>(type);
            storage.Add((IStorageFactory<IEnergyObservation>)instance);

            if (isAppropriateType && IsImplementInterface<IAnalizerFactory<IEnergyObservation>>(type))
                if (instance == null)
                    instance = GetFactoryInstance<IAnalizerFactory<IEnergyObservation>>(type);
            analizer.Add((IAnalizerFactory<IEnergyObservation>)instance);
        }
    }
}
