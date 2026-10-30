using System.Collections.Generic;
using NTC.Pool;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    // Spawns from NightPool and makes the clones normal Zenject citizens: each clone is injected once,
    // the first time it comes out of the pool, and then gets OnSpawned with its dependencies in place.
    // (NightPool's own spawn callbacks run before that, and its instantiate event isn't raised for
    // clones created beyond a pool's capacity, so neither can be used for injection.)
    public sealed class PooledSpawner
    {
        private readonly DiContainer _container;
        private readonly HashSet<int> _injected = new();

        public PooledSpawner(DiContainer container)
        {
            _container = container;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject clone = NightPool.Spawn(prefab, position, rotation);

            if (_injected.Add(clone.GetInstanceID()))
                _container.InjectGameObject(clone);

            if (clone.TryGetComponent(out IPooled pooled))
                pooled.OnSpawned();

            return clone;
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, position, rotation).GetComponent<T>();
        }

        public void DespawnAll(GameObject prefab)
        {
            if (NightPool.TryGetPoolByPrefab(prefab, out NightGameObjectPool pool))
                pool.DespawnAllClones();
        }
    }
}
