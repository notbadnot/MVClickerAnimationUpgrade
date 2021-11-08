using Zenject;


namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public SoundManager SoundManager;

        public override void InstallBindings()
        {
            Container.Bind<PrefabFactory>().AsSingle();

            Container.Bind<SoundManager>().FromInstance(SoundManager).AsSingle().NonLazy();
        }
    }
}