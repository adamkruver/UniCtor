using System;
using System.Collections.Generic;
using UniCtor.Services;

namespace UniCtor.Strategy
{
    internal interface IResolveStrategy
    {
        object Resolve(Type serviceType, ServiceProvider serviceProvider, HashSet<Type> resolvingTypes);
    }
}