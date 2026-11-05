using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "TurretConfig", menuName = "Game/Turret Config")]
    public sealed class TurretConfig : ScriptableObject
    {
        [Header("Aim")]
        [SerializeField, Range(10f, 90f)] private float _yawLimit = 70f;
        [Tooltip("Degrees of rotation per full screen-width drag")]
        [SerializeField, Min(1f)] private float _aimSensitivity = 150f;
        [SerializeField, Min(0.01f)] private float _aimSmoothTime = 0.08f;
        [Tooltip("Length of the red aiming laser")]
        [SerializeField, Min(1f)] private float _laserLength = 20f;

        [Header("Fire")]
        [SerializeField, Min(0.02f)] private float _fireInterval = 0.15f;

        [Header("Bullet")]
        [SerializeField, Min(1f)] private float _bulletSpeed = 50f;
        [SerializeField, Min(1f)] private float _bulletRange = 60f;
        [SerializeField, Min(0.1f)] private float _bulletDamage = 1f;
        [Tooltip("Hit radius of the bullet, keep it equal to the visual radius")]
        [SerializeField, Min(0.05f)] private float _bulletRadius = 0.25f;
        [SerializeField] private LayerMask _bulletHitMask = ~0;

        public float YawLimit => _yawLimit;
        public float AimSensitivity => _aimSensitivity;
        public float AimSmoothTime => _aimSmoothTime;
        public float LaserLength => _laserLength;
        public float FireInterval => _fireInterval;
        public float BulletSpeed => _bulletSpeed;
        public float BulletRange => _bulletRange;
        public float BulletDamage => _bulletDamage;
        public float BulletRadius => _bulletRadius;
        public LayerMask BulletHitMask => _bulletHitMask;
    }
}
