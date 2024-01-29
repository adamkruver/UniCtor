using UnityEngine;

namespace UniCtor.Builders
{
    public interface IGameObjectResolver
    {
        void Resolve(GameObject gameObject);
    }
}