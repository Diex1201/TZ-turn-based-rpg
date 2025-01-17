using Zenject;
public class LocalBattleAdapter : IBattleAdapter
{
    private BattleService battleService;
    [Inject] public LocalBattleAdapter(BattleService battleService) =>  this.battleService = battleService;
    public void OnAction(AbilityData ability, bool isPlayer) =>  battleService.OnAction(ability, isPlayer);
    public BattleState GetBattleState() => battleService.GetBattleState();
    public void StartBattle() =>  battleService.StartBattle();
}
