using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Controls
{
    public sealed class PointerInput : IPlayerInput, IInitializable, IDisposable
    {
        private readonly InputAction _press = new(
            name: "Press",
            type: InputActionType.Button,
            binding: "<Pointer>/press");

        private readonly InputAction _delta = new(
            name: "Delta",
            type: InputActionType.Value,
            binding: "<Pointer>/delta");

        public event Action Tapped;

        public float AimDelta => _press.IsPressed() ? _delta.ReadValue<Vector2>().x / Screen.width : 0f;

        public void Initialize()
        {
            _press.performed += OnPressPerformed;
            _press.Enable();
            _delta.Enable();
        }

        public void Dispose()
        {
            _press.performed -= OnPressPerformed;
            _press.Dispose();
            _delta.Dispose();
        }

        private void OnPressPerformed(InputAction.CallbackContext context) => Tapped?.Invoke();
    }
}
