using Game.Combat;
using Game.Configs;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private Health _car;

        private GameFlow _flow;
        private LevelConfig _levelConfig;
        private PooledSpawner _spawner;

        private Vector3 _carStart;

        [Inject]
        private void Construct(GameFlow flow, LevelConfig levelConfig, PooledSpawner spawner)
        {
            _flow = flow;
            _levelConfig = levelConfig;
            _spawner = spawner;
        }

        private void Start()
        {
            _carStart = _car.transform.position;

            _flow.LevelReset += Respawn;
            Spawn();
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.LevelReset -= Respawn;
        }

        private void Respawn()
        {
            _spawner.DespawnAll(_enemyPrefab.gameObject);

            Spawn();
        }

        private void Spawn()
        {
            int count = _levelConfig.EnemyCount;
            float zStart = _carStart.z + _levelConfig.EnemySpawnStartOffset;
            float zEnd = _carStart.z + _levelConfig.Length - _levelConfig.EnemySpawnEndMargin;
            float slotLength = (zEnd - zStart) / count;

            for (int i = 0; i < count; i++)
            {
                float z = zStart + slotLength * (i + Random.Range(0.15f, 0.85f));
                float x = _carStart.x + Random.Range(-_levelConfig.EnemyLateralRange, _levelConfig.EnemyLateralRange);
                
                Quaternion rotation = Quaternion.Euler(0f, 180f + Random.Range(-30f, 30f), 0f);

                Enemy enemy = _spawner.Spawn(_enemyPrefab, new Vector3(x, _carStart.y, z), rotation);
                enemy.Init(_enemyConfig, _flow, _car);
            }
        }
    }
}
