using System;
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

        public event Action Tapped;

        public void Initialize()
        {
            _press.performed += OnPressPerformed;
            _press.Enable();
        }

        public void Dispose()
        {
            _press.performed -= OnPressPerformed;
            _press.Disable();
            _press.Dispose();
        }

        private void OnPressPerformed(InputAction.CallbackContext context) => Tapped?.Invoke();
    }
}
