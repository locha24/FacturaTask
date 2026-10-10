using Game.Configs;
using Game.Controls;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelConfig);
            Container.BindInterfacesTo<PointerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}
