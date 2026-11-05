using Game.Configs;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class CarController : MonoBehaviour
    {
        private GameFlow _flow;
        private LevelConfig _config;

        private Vector3 _startPosition;
        private float _speed;

        private float DistanceTravelled => Vector3.Dot(transform.position - _startPosition, transform.forward);

        [Inject]
        private void Construct(GameFlow flow, LevelConfig config)
        {
            _flow = flow;
            _config = config;
        }

        private void Start()
        {
            _startPosition = transform.position;
            _flow.LevelReset += ResetCar;
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.LevelReset -= ResetCar;
        }

        private void Update()
        {
            bool isDriving = _flow.State == GameState.Playing;

            float targetSpeed = isDriving ? _config.CarSpeed : 0f;
            float rate = isDriving ? _config.CarAcceleration : _config.CarBraking;
            _speed = Mathf.MoveTowards(_speed, targetSpeed, rate * Time.deltaTime);

            transform.position += transform.forward * (_speed * Time.deltaTime);

            if (isDriving && DistanceTravelled >= _config.Length)
                _flow.Win();
        }

        private void ResetCar()
        {
            _speed = 0f;
            transform.position = _startPosition;
        }
    }
}
