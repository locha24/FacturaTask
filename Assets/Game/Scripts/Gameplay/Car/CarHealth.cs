using Game.Combat;
using Game.Configs;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health), typeof(BoxCollider))]
    public sealed class CarHealth : MonoBehaviour
    {
        private Health _health;
        private GameFlow _flow;
        private LevelConfig _config;

        [Inject]
        private void Construct(GameFlow flow, LevelConfig config)
        {
            _flow = flow;
            _config = config;
        }

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _health.Initialize(_config.CarMaxHealth);
            _health.Died += _flow.Lose;
            _flow.LevelReset += ResetHealth;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.Died -= _flow.Lose;

            if (_flow != null)
                _flow.LevelReset -= ResetHealth;
        }

        private void ResetHealth()
        {
            _health.Initialize(_config.CarMaxHealth);
        }
    }
}
