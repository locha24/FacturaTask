using System;

namespace Game.Controls
{
    public interface IPlayerInput
    {
        event Action Tapped;
        
        float AimDelta { get; }
    }
}
