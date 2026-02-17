using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameMode gameMode;
    [SerializeField] private DiceManager diceManager;
    public override void InstallBindings()
    {
        Container.Bind<GameMode>().FromInstance(gameMode).AsSingle();
        Container.Bind<DiceManager>().FromInstance(diceManager).AsSingle();
    }
}
