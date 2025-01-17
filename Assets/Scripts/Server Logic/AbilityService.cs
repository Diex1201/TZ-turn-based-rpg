using System.Collections.Generic;
using UnityEngine;
public class AbilityService
{
    private List<AbilityData> playerAbilities = new();
    private List<AbilityData> enemyAbilities = new();

    public AbilityService()
    {
        playerAbilities.Add(new AbilityData { AbilityName = "Attack", Cooldown = 0 });
        playerAbilities.Add(new AbilityData { AbilityName = "Barrier", Duration = 2, Cooldown = 4 });
        playerAbilities.Add(new AbilityData { AbilityName = "Regeneration", Duration = 3, Cooldown = 5 });
        playerAbilities.Add(new AbilityData { AbilityName = "Fireball", Duration = 5, Cooldown = 6, });
        playerAbilities.Add(new AbilityData { AbilityName = "Cleanse", Cooldown = 5 });

        enemyAbilities.Add(new AbilityData { AbilityName = "Attack", Cooldown = 0 });
        enemyAbilities.Add(new AbilityData { AbilityName = "Barrier", Duration = 2, Cooldown = 4 });
        enemyAbilities.Add(new AbilityData { AbilityName = "Regeneration", Duration = 3, Cooldown = 5 });
        enemyAbilities.Add(new AbilityData { AbilityName = "Fireball", Duration = 5, Cooldown = 6, });
        enemyAbilities.Add(new AbilityData { AbilityName = "Cleanse", Cooldown = 5 });
    }
    public List<AbilityData> GetPlayerAbilities() => playerAbilities;
    public List<AbilityData> GetEnemyAbilities() => enemyAbilities;
    public void ApplyAbility(AbilityData ability, UnitData target, UnitData self)
    {
        switch (ability.AbilityName)
        {
            case "Attack":
                ApplyDamage(target, ability.BasicAttackDamage);
                break;
            case "Barrier":
                EffectData barrierEffect = new EffectData { EffectName = "Barrier", EffectDuration = ability.Duration, CurrentDuration = ability.Duration };
                self.Effects.Add(barrierEffect);
                self.BarrierValue = 5;
                break;
            case "Regeneration":
                EffectData regenEffect = new EffectData { EffectName = "Regeneration", HealPerTurn = 2, EffectDuration = ability.Duration, CurrentDuration = ability.Duration };
                self.Effects.Add(regenEffect);
                break;
            case "Fireball":
                ApplyDamage(target, ability.FireballDamage);
                EffectData burningEffect = new EffectData { EffectName = "Burning", EffectDamageValue = 1, EffectDuration = ability.Duration, IsDebuff = true, CurrentDuration = ability.Duration };
                target.Effects.Add(burningEffect);
                break;
            case "Cleanse":
                RemoveEffect(self, "Burning");
                break;
        }
        ability.IsOnCooldown = ability.AbilityName != "Attack";
    }
    private void RemoveEffect(UnitData unitData, string effectName)
    {
        var effect = unitData.Effects.Find(x => x.EffectName == effectName);
        if (effect != null)
        {
            unitData.Effects.Remove(effect);
        }
    }
    private void ApplyDamage(UnitData target, int damage)
    {
        if (target.BarrierValue > 0)
        {
            int totalDamage = damage - target.BarrierValue;
            target.BarrierValue = Mathf.Max(0, target.BarrierValue - damage);
            if (totalDamage > 0)
            {
                target.UnitHealth -= totalDamage;
                target.UnitHealth = Mathf.Max(0, target.UnitHealth); 
            }
        }
        else
        {
            target.UnitHealth -= damage;
            target.UnitHealth = Mathf.Max(0, target.UnitHealth);
        }
    }
    public void DecreaseCooldown(UnitData unitData)
    {
        foreach (var ability in unitData.Abilities)
        {
            if (ability.IsOnCooldown)
            {
                ability.Cooldown--;
                if (ability.Cooldown <= 0)
                {
                    ability.IsOnCooldown = false;
                    ability.Cooldown = 0;
                }
            }
        }
    }
}
