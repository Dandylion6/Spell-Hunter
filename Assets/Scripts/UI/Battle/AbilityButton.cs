using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;

    private Ability ability;
    private ActionSelectionSession session;

    public void OnClick()
    {
        if (ability == null) return;
        session?.SelectAbility(ability);
    }

    public void Initialize(Ability newAbility, ActionSelectionSession newSession)
    {
        ability = newAbility;
        session = newSession;
        
        button.interactable = ability.CheckStatus(newSession.Battle) == AbilityStatus.Ready;
        text.text = ability.DisplayName;
    }
}