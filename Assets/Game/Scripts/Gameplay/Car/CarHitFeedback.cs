using DG.Tweening;
using Game.Combat;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health))]
    public sealed class CarHitFeedback : MonoBehaviour
    {
        [SerializeField] private Transform _visual;
        [SerializeField] private MaterialSwitcher _materials;
        [SerializeField, Min(0f)] private float _shakeDuration = 0.25f;
        [SerializeField, Min(0f)] private float _shakeStrength = 0.12f;
        [SerializeField, Min(1)] private int _shakeVibrato = 20;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _health.Damaged += OnDamaged;
        }

        private void OnDestroy()
        {
            _health.Damaged -= OnDamaged;
            _visual.DOKill();
        }

        private void OnDamaged()
        {
            if (_health.IsDead)
                return;

            _materials.Flash();

            _visual.DOComplete();
            _visual.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
        }
    }
}
