using System;
using System.Collections.Generic;
using UniCtor.Reflections;
using UniCtor.Services;

namespace UniCtor.Strategy
{
    internal sealed class ClassResolveStrategy : IResolveStrategy
    {
        private readonly ConstructorReader _constructorReader;

        public ClassResolveStrategy(ConstructorReader constructorReader)
        {
            _constructorReader = constructorReader ?? throw new ArgumentNullException(nameof(constructorReader));
        }

        public object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes)
        {
            if (_constructorReader.HasNonInterfaceOrClassParameters(serviceType))
                throw new InvalidOperationException($"Type {serviceType} must have only interface or class parameter");

            var parameterTypes = _constructorReader.GetParameterTypes(serviceType);

            if (parameterTypes.Length == 0)
                return Activator.CreateInstance(serviceType);

            List<object> parameters = new List<object>();

            foreach (Type parameterType in parameterTypes)
            {
                object implementation = serviceProvider.Resolve(parameterType, resolvingTypes) ??
                                        throw new InvalidOperationException("Service not found");

                parameters.Add(implementation);
            }

            return Activator.CreateInstance(serviceType, parameters.ToArray());
        }
    }
}