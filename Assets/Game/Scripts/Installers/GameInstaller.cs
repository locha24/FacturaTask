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
        [SerializeField] private TurretConfig _turretConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelConfig);
            Container.BindInstance(_turretConfig);

            Container.BindInterfacesTo<PointerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}
