using System;
[Serializable]
public class EffectData
{
    public string EffectName;
    public int EffectDamageValue;
    public int EffectDuration;
    public int HealPerTurn;
    public bool IsDebuff;
    public int CurrentDuration = 0;
}
