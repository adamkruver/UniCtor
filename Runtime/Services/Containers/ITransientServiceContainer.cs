using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface ITransientServiceContainer : ITransientRegister
	{
		bool HasTransient(Type serviceType);

		Type GetType<T>();

		Type GetType(Type serviceType);

		Func<IServiceProvider, object> GetFactory<T>();

		Func<IServiceProvider, object> GetFactory(Type serviceType);
	}
}