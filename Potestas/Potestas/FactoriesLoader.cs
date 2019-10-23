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
    internal class FactoriesLoader
    {
        public (ISourceFactory[], IProcessingFactory[]) Load(Assembly assembly)
        {
            if (assembly == null)            
                throw new ArgumentNullException(nameof(assembly));            

            var types = assembly.GetTypes();

            var sourceFactoryClasses = new List<ISourceFactory>();
            var processingFactoryClasses = new List<IProcessingFactory>();

            foreach (var type in types)
            {
                var isAppropriateType = !type.IsAbstract && !type.IsInterface && type.IsPublic;               

                IsApprociateType(isAppropriateType, type,sourceFactoryClasses, processingFactoryClasses);
            }

            return (sourceFactoryClasses.ToArray(), processingFactoryClasses.ToArray());
        }

        private static bool IsImplementInterface<U>(Type type)
        {
            var found = type.FindInterfaces(new TypeFilter(InterfaceFilter), typeof(U).ToString());

            return found != null && found.Any();
        }

        private static T GetFactoryInstance<T>(Type type)
        {
            if (type.IsGenericType)            
                type = type.MakeGenericType(new Type[] { typeof(IEnergyObservation) });

            var instance = (T)Activator.CreateInstance(type);

            return instance;
        }

        private static bool InterfaceFilter(Type typeObj, object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        private static void IsApprociateType(bool isAppropriateType, Type type, List<ISourceFactory> source, List<IProcessingFactory> processing)
        {
            object instance = null;

            if (isAppropriateType && IsImplementInterface<ISourceFactory>(type))
                instance = GetFactoryInstance<ISourceFactory>(type);
            source.Add((ISourceFactory)instance);

            if (isAppropriateType && IsImplementInterface<IProcessingFactory>(type))
                if (instance == null)
                    instance = GetFactoryInstance<IProcessingFactory>(type);
            processing.Add((IProcessingFactory)instance);
        }
    }
}
