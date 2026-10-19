using DG.Tweening;
using Game.Combat;
using Game.Core;
using NTC.Pool;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health))]
    public sealed class CarWreck : MonoBehaviour
    {
        [Tooltip("Parent of the body and the turret. Its pivot is the center of the body, so it flips around it")]
        [SerializeField] private Transform _visual;
        [SerializeField] private MaterialSwitcher _materials;
        [SerializeField] private Material _wreckMaterial;
        [SerializeField] private ParticleDespawner _explosionPrefab;
        [SerializeField, Min(0.1f)] private float _flipDuration = 0.9f;
        [SerializeField, Min(0f)] private float _jumpHeight = 1.6f;

        private Health _health;
        private GameFlow _flow;

        private Vector3 _restPosition;
        private Quaternion _restRotation;

        [Inject]
        private void Construct(GameFlow flow)
        {
            _flow = flow;
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

            NightPool.Spawn(_explosionPrefab, _visual.position, Quaternion.identity);
            _materials.Override(_wreckMaterial);

            PlayFlip();
        }
        
        private void PlayFlip()
        {
            float roll = Random.value < 0.5f ? 180f : -180f;

            DOVirtual.Float(0f, 1f, _flipDuration, progress =>
            {
                float jump = _jumpHeight * 4f * progress * (1f - progress);
                float turn = roll * (1f - (1f - progress) * (1f - progress));

                _visual.SetLocalPositionAndRotation(
                    _restPosition + Vector3.up * jump,
                    _restRotation * Quaternion.Euler(0f, 0f, turn));
            }).SetTarget(_visual);
        }

        private void ResetWreck()
        {
            _visual.DOKill();
            _visual.SetLocalPositionAndRotation(_restPosition, _restRotation);
            _materials.Restore();
        }
    }
}
