using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface ITransientServiceContainer : ITransientRegister
	{
		bool HasTransient(Type serviceType);

		Type GetTransientType<T>();

		Type GetTransientType(Type serviceType);

		Func<IServiceProvider, object> GetTransientFactory<T>();

		Func<IServiceProvider, object> GetTransientFactory(Type serviceType);
	}
}