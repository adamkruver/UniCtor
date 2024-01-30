using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface IScopedServiceContainer : IScopedRegister
	{
		bool HasScoped(Type serviceType);

		bool HasType(Type serviceType);

		bool HasFactory(Type serviceType);

		Type GetType<T>();

		Type GetType(Type serviceType);

		object GetImplementation<T>();

		object GetImplementation(Type serviceType);

		Func<IServiceProvider, object> GetFactory<T>();

		Func<IServiceProvider, object> GetFactory(Type serviceType);

		void RegisterAsScoped(Type serviceType, object implementation);

		object GetImplementationFromParent(Type serviceType);
		Type GetTypeFromParent(Type serviceType);
		Func<IServiceProvider, object> GetScopedFactoryFromParent(Type serviceType);
	}
}