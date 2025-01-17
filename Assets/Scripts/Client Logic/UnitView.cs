using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UnitView : MonoBehaviour
{
    [SerializeField] public Transform effectParent;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image hpBar;
    public void UpdateUI(UnitData unitData)
    {
        if (unitData == null) return;

        hpText.text = unitData.UnitHealth.ToString();
        hpBar.fillAmount = (float)unitData.UnitHealth / 100f;
    }
}
