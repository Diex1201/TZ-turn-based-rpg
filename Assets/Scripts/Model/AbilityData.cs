using System;
using System.Collections.Generic;
[Serializable]
public class AbilityData 
{
    public string AbilityName;
    public int BasicAttackDamage = 8;
    public int FireballDamage = 5;
    public int BurningEffectDamage = 1;
    public int HealPerTurn = 2; 
    public int Duration; 
    public int Cooldown; 
    public bool IsOnCooldown;
    public List<EffectData> Effects = new();
    public EffectData Burning;
    public int CurrentCooldown;
}
