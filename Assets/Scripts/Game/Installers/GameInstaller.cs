using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public InGameUIScript InGameUIScript;
    public InGameUIManager InGameUIManager;
    public GameMaster GameMaster;
    public GameMoleMaster GameMoleMaster;
    public OutGameUIManager OutGameUIManager;
    public override void InstallBindings()
    {
        Container.Bind<InGameUIScript>().FromInstance(InGameUIScript).AsSingle();
        Container.Bind<InGameUIManager>().FromInstance(InGameUIManager).AsSingle();
        Container.Bind<GameMaster>().FromInstance(GameMaster).AsSingle();
        Container.Bind<GameMoleMaster>().FromInstance(GameMoleMaster).AsSingle();
        Container.Bind<OutGameUIManager>().FromInstance(OutGameUIManager).AsSingle();
    }
}