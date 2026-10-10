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

        public float Length => _length;
        public float CarSpeed => _carSpeed;
        public float CarAcceleration => _carAcceleration;
        public float CarBraking => _carBraking;
    }
}
