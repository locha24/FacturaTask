using Game.Configs;
using Game.Controls;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class TurretController : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Bullet _bulletPrefab;

        private IPlayerInput _input;
        private GameFlow _flow;
        private TurretConfig _config;
        private PooledSpawner _spawner;

        private float _targetYaw;
        private float _currentYaw;
        private float _yawVelocity;
        private float _nextShotTime;

        [Inject]
        private void Construct(IPlayerInput input, GameFlow flow, TurretConfig config, PooledSpawner spawner)
        {
            _input = input;
            _flow = flow;
            _config = config;
            _spawner = spawner;
        }

        private void Start()
        {
            _flow.LevelReset += ResetTurret;
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.LevelReset -= ResetTurret;
        }

        private void Update()
        {
            if (_flow.State != GameState.Playing)
                return;

            Aim();
            TryFire();
        }

        private void Aim()
        {
            _targetYaw = Mathf.Clamp(
                _targetYaw + _input.AimDelta * _config.AimSensitivity,
                -_config.YawLimit,
                _config.YawLimit);

            _currentYaw = Mathf.SmoothDampAngle(_currentYaw, _targetYaw, ref _yawVelocity, _config.AimSmoothTime);
            transform.localRotation = Quaternion.Euler(0f, _currentYaw, 0f);
        }

        private void TryFire()
        {
            if (Time.time < _nextShotTime)
                return;

            _nextShotTime = Time.time + _config.FireInterval;

            _spawner.Spawn(_bulletPrefab, _muzzle.position, _muzzle.rotation);
        }

        private void ResetTurret()
        {
            _targetYaw = 0f;
            _currentYaw = 0f;
            _yawVelocity = 0f;
            _nextShotTime = 0f;
            transform.localRotation = Quaternion.identity;
            
            _spawner.DespawnAll(_bulletPrefab.gameObject);
        }
    }
}
