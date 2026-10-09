using System;

namespace Game.Controls
{
    public interface IPlayerInput
    {
        /// <summary>Raised when the pointer (mouse or touch) is pressed.</summary>
        event Action Tapped;
    }
}
