using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BattleService
{
    private BattleState battleState;
    private AbilityService abilityService;
    private UnitService unitService;
    [Inject]
    public BattleService(UnitService unitService, AbilityService abilityService)
    {
        this.unitService = unitService;
        this.abilityService = abilityService;
        battleState = new BattleState();
    }

    public BattleState GetBattleState() => battleState;
    public void StartBattle()
    {
        battleState.PlayerUnit = new UnitData()
        {
            UnitHealth = 100,
            Abilities = abilityService.GetPlayerAbilities(),
            Effects = new List<EffectData>()
        };
        battleState.EnemyUnit = new UnitData()
        {
            UnitHealth = 100,
            Abilities = abilityService.GetEnemyAbilities(),
            Effects = new List<EffectData>()
        };
        battleState.CurrentTurn = 0;
        battleState.IsBattleOver = false;
    }
    public void OnAction(AbilityData ability, bool isPlayer)
    {
        UnitData target = isPlayer ? battleState.EnemyUnit : battleState.PlayerUnit;
        UnitData self = isPlayer ? battleState.PlayerUnit : battleState.EnemyUnit;

        if (ability.IsOnCooldown) return;
        unitService.HandleEffects(self, self.Effects);
        abilityService.DecreaseCooldown(battleState.PlayerUnit);
        abilityService.ApplyAbility(ability, target, self);

        if (isPlayer && !battleState.IsBattleOver)
        {
            UnitData enemyUnit = battleState.EnemyUnit;
            List<AbilityData> enemyAbilities = enemyUnit.Abilities.FindAll(x => !x.IsOnCooldown);
            if (enemyAbilities.Count > 0)
            {
                var enemyAbility = enemyAbilities[Random.Range(0, enemyAbilities.Count)];
                unitService.HandleEffects(enemyUnit, enemyUnit.Effects);
                abilityService.DecreaseCooldown(battleState.EnemyUnit);
                abilityService.ApplyAbility(enemyAbility, battleState.PlayerUnit, battleState.EnemyUnit);
                
            }
        }

        battleState.IsBattleOver = battleState.PlayerUnit.UnitHealth <= 0 || battleState.EnemyUnit.UnitHealth <= 0;
    }
}
