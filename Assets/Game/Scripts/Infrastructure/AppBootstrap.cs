using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class AppBootstrap : IInitializable
    {
        private const int TargetFrameRate = 60;

        public void Initialize()
        {
            // Mobile defaults to 30 fps otherwise.
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = TargetFrameRate;
        }
    }
}
