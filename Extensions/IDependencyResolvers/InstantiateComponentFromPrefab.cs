using UniCtor.Builders;
using UnityEngine;

namespace UniCtor.Sources.Di.Extensions.IDependencyResolvers
{
    public static partial class IDependencyResolverExtentions
    {
        public static T InstantiateComponentFromPrefab<T>(
            this IDependencyResolver dependencyResolver,
            T prefab,
            Vector3 position,
            Quaternion rotation
        ) where T : MonoBehaviour
        {
            var gameObject = Object.Instantiate(prefab,position,rotation);
            dependencyResolver.Resolve(gameObject.gameObject);
            return gameObject;
        }

        public static T InstantiateComponentFromPrefab<T>(this IDependencyResolver dependencyResolver, T prefab)
            where T : MonoBehaviour =>
            dependencyResolver.InstantiateComponentFromPrefab(prefab, Vector3.zero, Quaternion.identity);
    }
}