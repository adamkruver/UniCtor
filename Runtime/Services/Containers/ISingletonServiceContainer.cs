using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface ISingletonServiceContainer : ISingletonRegister
	{
		bool HasSingleton(Type type);

		Type GetType<T>();

		Type GetType(Type type);

		object GetImplementation<T>();

		object GetImplementation(Type type);

		Func<IServiceProvider, object> GetFactory<T>();

		Func<IServiceProvider, object> GetFactory(Type serviceType);

		void RegisterAsSingleton(Type serviceType, object implementation);
	}
}