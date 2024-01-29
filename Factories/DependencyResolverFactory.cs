using UniCtor.Builders;
using UniCtor.Resolvers;

namespace UniCtor.Factories
{
    public class DependencyResolverFactory
    {
        public IDependencyResolver Create() => 
            new DependencyResolver(null, null);
    }
}