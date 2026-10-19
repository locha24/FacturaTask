using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField, Min(1f)] private float _maxHealth = 3f;
        [SerializeField, Min(0.1f)] private float _moveSpeed = 11f;
        [Tooltip("Degrees per second")]
        [SerializeField, Min(1f)] private float _turnSpeed = 540f;

        [Header("Behaviour")]
        [Tooltip("Distance to the car at which an idle enemy starts running")]
        [SerializeField, Min(1f)] private float _aggroRadius = 22f;
        [Tooltip("Damage dealt to the car on touch, the enemy dies right after")]
        [SerializeField, Min(0.1f)] private float _contactDamage = 10f;
        [Tooltip("How close to the car's body the enemy must get to count as touching")]
        [SerializeField, Min(0f)] private float _touchDistance = 0.3f;

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float TurnSpeed => _turnSpeed;
        public float AggroRadius => _aggroRadius;
        public float ContactDamage => _contactDamage;
        public float TouchDistance => _touchDistance;
    }
}
