using UnityEngine;
using Zenject;

public class BattleInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerUnitView;
    [SerializeField] private GameObject enemyUnitView;
    [SerializeField] private GameObject battleController;
    public override void InstallBindings()
    {
        Container.Bind<UnitService>().AsSingle();
        Container.Bind<AbilityService>().AsSingle();
        Container.Bind<BattleService>().AsSingle();
        Container.Bind<IBattleAdapter>().To<LocalBattleAdapter>().AsSingle();

        Container.Bind<UnitView>().WithId("Player").FromInstance(playerUnitView.GetComponent<UnitView>());
        Container.Bind<UnitView>().WithId("Enemy").FromInstance(enemyUnitView.GetComponent<UnitView>());
        Container.Bind<BattleController>().FromInstance(battleController.GetComponent<BattleController>()).AsSingle();
    }
}
