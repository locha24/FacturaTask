using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class GameOverlayView : MonoBehaviour
    {
        [SerializeField] private GameObject _tapToStartHint;
        [SerializeField] private GameObject _winOverlay;
        [SerializeField] private GameObject _loseOverlay;

        private GameFlow _flow;

        [Inject]
        private void Construct(GameFlow flow)
        {
            _flow = flow;
        }

        private void Start()
        {
            _flow.StateChanged += Refresh;
            Refresh(_flow.State);
        }

        private void OnDestroy()
        {
            if (_flow != null)
                _flow.StateChanged -= Refresh;
        }

        private void Refresh(GameState state)
        {
            _tapToStartHint.SetActive(state == GameState.WaitingToStart);
            _winOverlay.SetActive(state == GameState.Win);
            _loseOverlay.SetActive(state == GameState.Lose);
        }
    }
}
