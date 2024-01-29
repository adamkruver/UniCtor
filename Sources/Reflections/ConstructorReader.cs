using System;
using System.Linq;

namespace UniCtor.Reflections
{
    public class ConstructorReader
    {
        public Type[] GetParameterTypes(Type serviceType)
        {
            var constructor = serviceType.GetConstructors().SingleOrDefault() ??
                              throw new InvalidOperationException(
                                  $"Resolving type {serviceType} must have a single constructor"
                              );

            return constructor
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();
        }

        public bool HasNonInterfaceOrClassParameters(Type serviceType) =>
            GetParameterTypes(serviceType)
                .Count(type => type.IsInterface == false && type.IsClass == false) != 0;
    }
}