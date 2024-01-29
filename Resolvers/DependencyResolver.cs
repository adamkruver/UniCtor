using UniCtor.Builders;
using UniCtor.Extensions.IEnumerable;
using UniCtor.Services;
using UnityEngine;

namespace UniCtor.Resolvers
{
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceContainer _servicesContainer;

        public DependencyResolver(IServiceContainer serviceContainer, IServiceProvider serviceProvider)
        {
            _servicesContainer = new ServiceContainer(serviceContainer);
            _serviceProvider = new ServiceProvider(_servicesContainer, serviceProvider);

            _servicesContainer
                .RegisterAsTransient<IDependencyResolver, DependencyResolver>();
        }

        public IServiceCollection Services => _servicesContainer;

        public T Resolve<T>() where T : class =>
            _serviceProvider.GetService<T>();

        public void Resolve(GameObject gameObject) =>
            gameObject
                .GetComponentsInChildren<MonoBehaviour>()
                .ForEach(
                    monoBehaviour =>
                        MonoBehaviourResolver
                            .Create(monoBehaviour)?
                            .Resolve(_serviceProvider)
                );
    }
}