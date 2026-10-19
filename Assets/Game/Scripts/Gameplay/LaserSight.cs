using Game.Configs;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class LaserSight : MonoBehaviour
    {
        private LineRenderer _line;
        private GameFlow _flow;
        private TurretConfig _config;

        [Inject]
        private void Construct(GameFlow flow, TurretConfig config)
        {
            _flow = flow;
            _config = config;
        }

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
        }

        private void Start()
        {
            _flow.StateChanged += OnStateChanged;
            OnStateChanged(_flow.State);
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.StateChanged -= OnStateChanged;
        }
        
        private void LateUpdate()
        {
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            float length = _config.LaserLength;
            if (Physics.Raycast(origin, direction, out RaycastHit hit, length, _config.BulletHitMask, QueryTriggerInteraction.Ignore))
                length = hit.distance;

            _line.SetPosition(0, origin);
            _line.SetPosition(1, origin + direction * length);
        }

        private void OnStateChanged(GameState state)
        {
            bool isVisible = state == GameState.Playing;

            _line.enabled = isVisible;
            enabled = isVisible;
        }
    }
}
