using UniCtor.Builders;

namespace UniCtor.Contexts
{
    public interface ISceneContext
    {
        IDependencyResolver DependencyResolver { get; }
    }
}