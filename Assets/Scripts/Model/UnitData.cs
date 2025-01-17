using System;
using System.Collections.Generic;
[Serializable]
public class UnitData
{
    public int UnitHealth;
    public int BarrierValue;
    public List<AbilityData> Abilities = new();
    public List<EffectData> Effects = new();
}
