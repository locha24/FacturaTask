using DG.Tweening;
using Game.Combat;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health))]
    public sealed class CarWreck : MonoBehaviour
    {
        private const float FirstLanding = 0.5f;
        private const float SecondLanding = 0.78f;
        private const float RollDelay = 0.04f;

        [Tooltip("Parent of the body and the turret. Its pivot is the center of the body, so it rolls around it")]
        [SerializeField] private Transform _visual;
        [SerializeField] private MaterialSwitcher _materials;
        [SerializeField] private Material _wreckMaterial;
        [SerializeField] private ParticleDespawner _explosionPrefab;
        [SerializeField, Min(0.1f)] private float _tumbleDuration = 1.8f;
        [SerializeField, Min(0f)] private float _jumpHeight = 3f;
        [SerializeField, Min(0f)] private float _slideDistance = 3f;

        private Health _health;
        private GameFlow _flow;
        private PooledSpawner _spawner;

        private Vector3 _restPosition;
        private Quaternion _restRotation;
        private bool _landingBlastPlayed;

        [Inject]
        private void Construct(GameFlow flow, PooledSpawner spawner)
        {
            _flow = flow;
            _spawner = spawner;
        }

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _visual.GetLocalPositionAndRotation(out _restPosition, out _restRotation);

            _health.Died += OnDied;
            _flow.LevelReset += ResetWreck;
        }

        private void OnDestroy()
        {
            _health.Died -= OnDied;

            if (_flow != null)
                _flow.LevelReset -= ResetWreck;

            _visual.DOKill();
        }

        private void OnDied()
        {
            _visual.DOKill();
            _visual.SetLocalPositionAndRotation(_restPosition, _restRotation);

            _materials.Override(_wreckMaterial);
            Explode(1f);

            PlayTumble();
        }

        private void PlayTumble()
        {
            float rollSign = RandomSign();
            float pitchPeak = Random.Range(15f, 30f) * RandomSign();
            float yawTotal = Random.Range(25f, 60f) * RandomSign();

            _landingBlastPlayed = false;

            DOVirtual.Float(0f, 1f, _tumbleDuration, progress => ApplyTumble(progress, rollSign, pitchPeak, yawTotal))
                .SetEase(Ease.Linear)
                .SetTarget(_visual);
        }

        private void ApplyTumble(float t, float rollSign, float pitchPeak, float yawTotal)
        {
            float air = Mathf.Clamp01(t / FirstLanding);
            float spin = Mathf.Clamp01((t - RollDelay) / (FirstLanding - RollDelay));
            float landed = t > FirstLanding ? 1f : 0f;
            float settle = (1f - t) * (1f - t);

            float roll = rollSign * (540f * (1f - Mathf.Pow(1f - spin, 3f)) + 7f * Mathf.Sin(t * 60f) * settle * landed);
            float pitch = pitchPeak * Mathf.Sin(Mathf.PI * air) + 4f * Mathf.Sin(t * 45f) * settle * landed;
            float yaw = yawTotal * EaseOutQuad(t);

            Vector3 position = _restPosition
                + Vector3.up * Height(t)
                + Vector3.forward * (_slideDistance * EaseOutQuad(t));

            _visual.SetLocalPositionAndRotation(position, _restRotation * Quaternion.Euler(pitch, yaw, roll));

            if (!_landingBlastPlayed && t >= FirstLanding)
            {
                _landingBlastPlayed = true;
                Explode(0.6f);
            }
        }

        private float Height(float t)
        {
            if (t < FirstLanding)
                return _jumpHeight * Parabola(t / FirstLanding);

            if (t < SecondLanding)
                return _jumpHeight * 0.18f * Parabola((t - FirstLanding) / (SecondLanding - FirstLanding));

            return _jumpHeight * 0.04f * Parabola((t - SecondLanding) / (1f - SecondLanding));
        }

        private void Explode(float scale)
        {
            ParticleDespawner explosion = _spawner.Spawn(_explosionPrefab, _visual.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * scale;
        }

        private void ResetWreck()
        {
            _visual.DOKill();
            _visual.SetLocalPositionAndRotation(_restPosition, _restRotation);
            _materials.Restore();
        }

        private static float Parabola(float x) => 4f * x * (1f - x);

        private static float EaseOutQuad(float x) => 1f - (1f - x) * (1f - x);

        private static float RandomSign() => Random.value < 0.5f ? 1f : -1f;
    }
}
