using Game.Combat;
using Game.Configs;
using NTC.Pool;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class Bullet : MonoBehaviour
    {
        private TurretConfig _config;
        private TrailRenderer _trail;
        private float _travelled;

        private void Awake()
        {
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        public void Launch(TurretConfig config)
        {
            _config = config;
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
