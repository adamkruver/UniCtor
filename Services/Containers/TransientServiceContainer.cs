using System;
using System.Collections.Generic;
using UniCtor.Reflections;

namespace UniCtor.Services.Containers
{
    internal class TransientServiceContainer : ITransientServiceContainer
    {
        private readonly Dictionary<Type, Type> _transientTypes = new();
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _transientFactories = new();

        private readonly IServiceContainer _container;
        private readonly ITransientServiceContainer _parentContainer;
        private readonly ConstructorReader _constructorReader;

        public TransientServiceContainer(
            IServiceContainer container,
            ConstructorReader constructorReader,
            ITransientServiceContainer parentContainer
        )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _parentContainer = parentContainer;
            _constructorReader = constructorReader ?? throw new ArgumentNullException(nameof(constructorReader));
        }

        public IServiceCollection RegisterAsTransient<TService, TImplementation>()
            where TImplementation : class, TService
        {
            if (_constructorReader.HasNonInterfaceOrClassParameters(typeof(TImplementation)))
                throw new InvalidOperationException(
                    $"Type {typeof(TImplementation)} must have only interface or class parameter"
                );

            _transientTypes[typeof(TService)] = typeof(TImplementation);

            return _container;
        }

        public IServiceCollection RegisterAsTransient<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            _transientFactories[typeof(TService)] = factory ?? throw new ArgumentNullException(nameof(factory));

            return _container;
        }

        public bool HasTransient(Type serviceType) =>
            _transientTypes.ContainsKey(serviceType) ||
            _transientFactories.ContainsKey(serviceType) ||
            (_parentContainer?.HasTransient(serviceType) ?? false);

        public Type GetTransientType<T>() =>
            GetTransientType(typeof(T));

        public Type GetTransientType(Type serviceType) =>
            (_transientTypes.ContainsKey(serviceType)
                ? _transientTypes[serviceType]
                : null)
            ?? _parentContainer?.GetTransientType(serviceType);

        public Func<IServiceProvider, object> GetTransientFactory<T>() =>
            GetTransientFactory(typeof(T));

        public Func<IServiceProvider, object> GetTransientFactory(Type serviceType) =>
            (_transientFactories.ContainsKey(serviceType)
                ? _transientFactories[serviceType]
                : null)
            ?? _parentContainer?.GetTransientFactory(serviceType);
    }
}