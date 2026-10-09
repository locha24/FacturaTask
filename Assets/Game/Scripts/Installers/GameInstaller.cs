using Game.Controls;
using Game.Core;
using Zenject;

namespace Game.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PointerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}
