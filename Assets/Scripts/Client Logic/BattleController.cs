using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
public class BattleController : MonoBehaviour
{
    [Inject(Id = "Player")] private UnitView playerUnitView;
    [Inject(Id = "Enemy")] private UnitView enemyUnitView;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private Transform playerAbilityParent;
    [SerializeField] private Transform enemyAbilityParent;
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private GameObject effectViewPrefab;
    private IBattleAdapter battleAdapter;
    private BattleState _battleState;
    private bool isBattleStateDirty = false;
    private Dictionary<string, AbilityView> playerAbilityButtons = new();
    private Dictionary<string, AbilityView> enemyAbilityButtons = new();
    private Dictionary<string, EffectView> playerEffectView = new();
    private Dictionary<string, EffectView> enemyEffectView = new();

    [Inject]
    public void Construct(IBattleAdapter battleAdapter)
    {
        this.battleAdapter = battleAdapter;
    }
    private void Start()
    {
        restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        battleAdapter.StartBattle();
        CreateButtons(battleAdapter.GetBattleState().PlayerUnit.Abilities, playerAbilityParent, true);
        CreateButtons(battleAdapter.GetBattleState().EnemyUnit.Abilities, enemyAbilityParent, false);
        UpdateUI();
    }
    private void CreateButtons(List<AbilityData> abilities, Transform abilityParent, bool isPlayer)
    {
        foreach (var ability in abilities)
        {
            GameObject button = Instantiate(abilityButtonPrefab, abilityParent);
            var abilityView = button.GetComponent<AbilityView>();
            abilityView.SetAbilityData(ability);
            if (isPlayer)
                button.GetComponent<Button>().onClick.AddListener(() => OnAbilityClick(ability));

            if (isPlayer)
                playerAbilityButtons.Add(ability.AbilityName, abilityView);
            else
                enemyAbilityButtons.Add(ability.AbilityName, abilityView);
        }
    }
    private void Update()
    {
        if (battleAdapter != null && isBattleStateDirty)
        {
            UpdateBattleState();
            isBattleStateDirty = false;
        }
    }
    private void UpdateBattleState()
    {
        _battleState = battleAdapter.GetBattleState();
        if (_battleState != null)
        {
            UpdateUI();
        }
        if (_battleState != null && _battleState.IsBattleOver)
        {
            restartButton.SetActive(true);
        }
        else
        {
            restartButton.SetActive(false);
        }
    }
    private void OnAbilityClick(AbilityData ability)
    {
        battleAdapter.OnAction(ability, true);
        isBattleStateDirty = true;
    }
    private void RestartGame()
    {
        battleAdapter.StartBattle();
        isBattleStateDirty = true;
    }
    private void UpdateButtons(UnitData unitData, Dictionary<string, AbilityView> buttons)
    {
        foreach (var ability in unitData.Abilities)
        {
            if (buttons.ContainsKey(ability.AbilityName))
            {
                buttons[ability.AbilityName].UpdateUI();
            }
        }
    }

    private void UpdateEffects(UnitData unitData, Transform parent, Dictionary<string, EffectView> effectViews)
    {
        List<string> currentEffects = new();
        foreach (var effect in unitData.Effects)
        {
            currentEffects.Add(effect.EffectName);

            if (!effectViews.ContainsKey(effect.EffectName))
            {
                if (effectViewPrefab == null)
                {
                    return;
                }
                GameObject effectViewObject = Instantiate(effectViewPrefab, parent);
                var effectView = effectViewObject.GetComponent<EffectView>();
                if (effectView == null)  return;
                effectViews.Add(effect.EffectName, effectView);
                effectView.UpdateEffect(effect);
            }
            else effectViews[effect.EffectName].UpdateEffect(effect);
        }
        var toRemove = new List<string>();
        foreach (var key in effectViews.Keys)
        {
            if (!currentEffects.Contains(key))
            {
                toRemove.Add(key);
            }
        }
        foreach (var key in toRemove)
        {
            Destroy(effectViews[key].gameObject);
            effectViews.Remove(key);
        }
    }
    public void UpdateUI()
    {
        if (_battleState == null) return;

        playerUnitView.UpdateUI(_battleState.PlayerUnit);
        enemyUnitView.UpdateUI(_battleState.EnemyUnit);
        UpdateButtons(_battleState.PlayerUnit, playerAbilityButtons);
        UpdateButtons(_battleState.EnemyUnit, enemyAbilityButtons);
        UpdateEffects(_battleState.PlayerUnit, playerUnitView.effectParent, playerEffectView);
        UpdateEffects(_battleState.EnemyUnit, enemyUnitView.effectParent, enemyEffectView);
    }
}
