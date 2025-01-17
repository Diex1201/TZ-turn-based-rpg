using System;
using System.Collections.Generic;
using UnityEngine;
public class UnitService
{
    public void HandleEffects(UnitData unitData, List<EffectData> effects)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];

            if (effect.EffectName == "Regeneration")
            {
                unitData.UnitHealth += effect.HealPerTurn;
                unitData.UnitHealth = Math.Min(unitData.UnitHealth, 100);
            }
            if (effect.EffectName == "Burning")
            {
                unitData.UnitHealth -= effect.EffectDamageValue;
            }

            if (effect.EffectDuration > 0)
            {
                if (effect.CurrentDuration > 0)
                    effect.CurrentDuration--;

                if (effect.CurrentDuration <= 0)
                {
                    effects.RemoveAt(i);
                }

            }
        }
    }
}
