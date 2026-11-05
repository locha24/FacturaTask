using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
    public sealed class LevelConfig : ScriptableObject
    {
        [Header("Level")]
        [SerializeField, Min(1f)] private float _length = 160f;

        [Header("Car")]
        [SerializeField, Min(0.1f)] private float _carSpeed = 8f;
        [SerializeField, Min(0.1f)] private float _carAcceleration = 6f;
        [SerializeField, Min(0.1f)] private float _carBraking = 12f;
        [SerializeField, Min(1f)] private float _carMaxHealth = 100f;

        [Header("Enemies")]
        [SerializeField, Min(1)] private int _enemyCount = 12;
        [Tooltip("No enemies closer than this to the start")]
        [SerializeField, Min(0f)] private float _enemySpawnStartOffset = 25f;
        [Tooltip("No enemies closer than this to the finish")]
        [SerializeField, Min(0f)] private float _enemySpawnEndMargin = 15f;
        [Tooltip("Max sideways distance from the car's lane")]
        [SerializeField, Min(0f)] private float _enemyLateralRange = 10f;

        public float Length => _length;
        public float CarSpeed => _carSpeed;
        public float CarAcceleration => _carAcceleration;
        public float CarBraking => _carBraking;
        public float CarMaxHealth => _carMaxHealth;
        public int EnemyCount => _enemyCount;
        public float EnemySpawnStartOffset => _enemySpawnStartOffset;
        public float EnemySpawnEndMargin => _enemySpawnEndMargin;
        public float EnemyLateralRange => _enemyLateralRange;
    }
}
