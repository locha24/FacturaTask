using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class AppBootstrap : IInitializable
    {
        private const int TargetFrameRate = 60;

        public void Initialize()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = TargetFrameRate;
        }
    }
}
