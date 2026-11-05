using System.Collections.Generic;
using NTC.Pool;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
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
