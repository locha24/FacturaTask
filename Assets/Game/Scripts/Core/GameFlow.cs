using System;
using Game.Controls;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public sealed class GameFlow : IInitializable, IDisposable
    {
        private const float RestartInputDelay = 0.4f;

        private readonly IPlayerInput _input;
        private float _finishedAt;

        public GameState State { get; private set; } = GameState.WaitingToStart;

        public event Action<GameState> StateChanged;
        public event Action LevelReset;

        public GameFlow(IPlayerInput input)
        {
            _input = input;
        }

        public void Initialize() => _input.Tapped += OnTapped;

        public void Dispose() => _input.Tapped -= OnTapped;

        public void Win() => Finish(GameState.Win);

        public void Lose() => Finish(GameState.Lose);

        private void OnTapped()
        {
            switch (State)
            {
                case GameState.WaitingToStart:
                    SetState(GameState.Playing);
                    break;

                case GameState.Win:
                case GameState.Lose:
                    if (Time.unscaledTime - _finishedAt >= RestartInputDelay)
                        RestartLevel();
                    break;
            }
        }

        private void Finish(GameState result)
        {
            if (State != GameState.Playing)
                return;

            _finishedAt = Time.unscaledTime;
            SetState(result);
        }

        private void RestartLevel()
        {
            LevelReset?.Invoke();
            SetState(GameState.WaitingToStart);
        }

        private void SetState(GameState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
