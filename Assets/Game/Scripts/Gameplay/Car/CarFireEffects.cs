using Game.Combat;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health))]
    public sealed class CarFireEffects : MonoBehaviour
    {
        [Tooltip("Particle systems to scale. Their emission rate in the prefab is the rate at full damage")]
        [SerializeField] private ParticleSystem[] _systems;
        [Tooltip("Share of health below which smoke and fire appear")]
        [SerializeField, Range(0.05f, 1f)] private float _startRatio = 0.7f;

        private Health _health;
        private float[] _maxRates;

        private void Awake()
        {
            _health = GetComponent<Health>();

            _maxRates = new float[_systems.Length];
            for (int i = 0; i < _systems.Length; i++)
                _maxRates[i] = _systems[i].emission.rateOverTime.constant;
        }

        private void Start()
        {
            _health.Changed += OnHealthChanged;

            Apply(_health.Max > 0f ? _health.Current / _health.Max : 1f);
        }

        private void OnDestroy()
        {
            _health.Changed -= OnHealthChanged;
        }

        private void OnHealthChanged(float current, float max)
        {
            Apply(current / max);
        }

        private void Apply(float healthRatio)
        {
            float intensity = Mathf.InverseLerp(_startRatio, 0f, healthRatio);

            for (int i = 0; i < _systems.Length; i++)
            {
                ParticleSystem.EmissionModule emission = _systems[i].emission;
                emission.rateOverTime = _maxRates[i] * intensity;
            }
        }
    }
}
