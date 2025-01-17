public interface IBattleAdapter
{
    void OnAction(AbilityData ability, bool isPlayer);
    BattleState GetBattleState();
    void StartBattle();
}
