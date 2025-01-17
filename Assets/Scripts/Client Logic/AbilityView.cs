using UnityEngine;
using TMPro;
public class AbilityView : MonoBehaviour
{
    private AbilityData abilityData;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI cooldownText;

    public void SetAbilityData(AbilityData abilityData)
    {
        this.abilityData = abilityData;
        nameText.text = abilityData.AbilityName;
    }
    public void UpdateUI()
    {
        if (abilityData == null) return;
        cooldownText.text = abilityData.IsOnCooldown ? abilityData.Cooldown.ToString() : "";
    }
}
