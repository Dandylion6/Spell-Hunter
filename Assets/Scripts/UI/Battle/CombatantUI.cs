using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatantUI : MonoBehaviour
{
    [SerializeField] private GameObject turnIcon;
    [SerializeField] private TMP_Text combatantName;
    [SerializeField] private Slider health;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider mana;
    [SerializeField] private TMP_Text manaText;
    
    private Combatant combatant;
    
    public void Initialize(Combatant newCombatant)
    {
        combatant = newCombatant;
        combatantName.text = combatant.DisplayName;
    }

    public void UpdateTurn(Turn currentTurn)
    {
        bool isMyTurn = currentTurn.Actor == combatant;
        if (turnIcon.activeSelf == isMyTurn) return;
        
        turnIcon.SetActive(isMyTurn);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void Update()
    {
        health.value = (float)combatant.Stats.CurrentHealth / combatant.Stats.MaxHealth.Value;
        mana.value = (float)combatant.Stats.CurrentMana / combatant.Stats.MaxMana.Value;

        healthText.text = $"{combatant.Stats.CurrentHealth}/{combatant.Stats.MaxHealth}";
        manaText.text = $"{combatant.Stats.CurrentMana}/{combatant.Stats.MaxMana}";
    }
}
