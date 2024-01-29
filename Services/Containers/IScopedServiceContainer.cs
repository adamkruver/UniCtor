using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface IScopedServiceContainer : IScopedRegister
	{
		bool HasScoped(Type serviceType);

		bool HasScopedType(Type serviceType);

		bool HasScopedFactory(Type serviceType);

		Type GetScopedType<T>();

		Type GetScopedType(Type serviceType);

		object GetScoped<T>();

		object GetScoped(Type serviceType);

		Func<IServiceProvider, object> GetScopedFactory<T>();

		Func<IServiceProvider, object> GetScopedFactory(Type serviceType);

		void RegisterAsScoped(Type serviceType, object implementation);

		object GetScopedFromParent(Type serviceType);
		Type GetScopedTypeFromParent(Type serviceType);
		Func<IServiceProvider, object> GetScopedFactoryFromParent(Type serviceType);
	}
}