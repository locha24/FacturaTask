using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField, Min(0.01f)] private float _smoothTime = 0.15f;

        private GameFlow _flow;
        private Vector3 _offset;
        private Vector3 _velocity;
        private bool _snapToTarget;

        [Inject]
        private void Construct(GameFlow flow)
        {
            _flow = flow;
        }

        private void Start()
        {
            _offset = transform.position - _target.position;
            _flow.LevelReset += RequestSnap;
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.LevelReset -= RequestSnap;
        }

        private void LateUpdate()
        {
            Vector3 desired = _target.position + _offset;

            if (_snapToTarget)
            {
                transform.position = desired;
                _velocity = Vector3.zero;
                _snapToTarget = false;
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, _smoothTime);
        }
        
        private void RequestSnap() => _snapToTarget = true;
    }
}
