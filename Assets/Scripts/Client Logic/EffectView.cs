using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EffectView : MonoBehaviour
{
    [SerializeField] private Image effectIcon;
    [SerializeField] private TextMeshProUGUI durationText;

    public void UpdateEffect(EffectData data)
    {
        SetEffectIcon(data.EffectName);

        if (data.EffectName == "Burning")
        {
            effectIcon.color = Color.red;
        }
        if (data.EffectName == "Regeneration")
        {
            effectIcon.color = Color.green;
        }
        if (data.EffectName == "Barrier")
        {
            effectIcon.color = Color.blue;
        }

        durationText.text = data.CurrentDuration.ToString();
    }
    private void SetEffectIcon(string effectName)
    {
        Sprite sprite = Resources.Load<Sprite>(effectName);
        effectIcon.sprite = sprite;
    }
}
