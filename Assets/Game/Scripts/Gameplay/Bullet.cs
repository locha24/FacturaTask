using Game.Combat;
using Game.Configs;
using Game.Infrastructure;
using NTC.Pool;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class Bullet : MonoBehaviour, IPooled
    {
        private TurretConfig _config;
        private TrailRenderer _trail;
        private float _travelled;

        [Inject]
        private void Construct(TurretConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        public void OnSpawned()
        {
            _travelled = 0f;

            if (_trail != null)
                _trail.Clear();
        }

        private void Update()
        {
            float step = _config.BulletSpeed * Time.deltaTime;
            Vector3 direction = transform.forward;

            if (Physics.SphereCast(transform.position, _config.BulletRadius, direction, out RaycastHit hit, step,
                    _config.BulletHitMask, QueryTriggerInteraction.Ignore))
            {
                IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
                target?.TakeDamage(_config.BulletDamage);

                NightPool.Despawn(this);
                return;
            }

            transform.position += direction * step;
            _travelled += step;

            if (_travelled >= _config.BulletRange)
                NightPool.Despawn(this);
        }
    }
}
