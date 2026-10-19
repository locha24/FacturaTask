using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class CarTrail : MonoBehaviour
    {
        [SerializeField] private TrailRenderer[] _trails;

        private GameFlow _flow;

        [Inject]
        private void Construct(GameFlow flow)
        {
            _flow = flow;
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

        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.WaitingToStart:
                    Clear();
                    SetEmitting(false);
                    break;

                case GameState.Playing:
                    SetEmitting(true);
                    break;
            }
        }

        private void SetEmitting(bool value)
        {
            foreach (TrailRenderer trail in _trails)
                trail.emitting = value;
        }

        private void Clear()
        {
            foreach (TrailRenderer trail in _trails)
                trail.Clear();
        }
    }
}
