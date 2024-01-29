using System;
using UniCtor.Services.Containers;
using UniCtor.Strategy;

namespace UniCtor.Services
{
	internal interface IServiceContainer : IServiceCollection
	{
		public ISingletonServiceContainer Singleton { get; }

		public ITransientServiceContainer Transient { get; }

		public IScopedServiceContainer Scoped { get; }

		IResolveStrategy GetResolveStrategy(Type serviceType);
	}
}