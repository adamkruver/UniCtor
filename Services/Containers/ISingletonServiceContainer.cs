using System;
using UniCtor.Services.Registers;

namespace UniCtor.Services.Containers
{
	internal interface ISingletonServiceContainer : ISingletonRegister
	{
		bool HasSingleton(Type type);

		Type GetSingletonType<T>();

		Type GetSingletonType(Type type);

		object GetSingleton<T>();

		object GetSingleton(Type type);

		void RegisterAsSingleton(Type serviceType, object implementation);
	}
}