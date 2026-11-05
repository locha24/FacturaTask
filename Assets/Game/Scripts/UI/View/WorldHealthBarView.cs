using Game.Combat;
using UnityEngine;

namespace Game.UI
{
    public sealed class WorldHealthBarView : MonoBehaviour
    {
        [SerializeField] private GameObject _visual;
        [SerializeField] private Transform _fill;
        [SerializeField] private bool _showOnlyWhenDamaged = true;

        private Health _health;
        private Transform _camera;

        private void Awake()
        {
            _health = GetComponentInParent<Health>();
            _camera = Camera.main.transform;

            _health.Changed += OnHealthChanged;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.Changed -= OnHealthChanged;
        }

        private void LateUpdate()
        {
            if (_visual.activeSelf)
                transform.rotation = _camera.rotation;
        }

        private void OnHealthChanged(float current, float max)
        {
            float ratio = current / max;
            bool isDamaged = ratio > 0f && ratio < 1f;

            _visual.SetActive(!_showOnlyWhenDamaged || isDamaged);
            _fill.localScale = new Vector3(ratio, 1f, 1f);
        }
    }
}
