using System.Collections.Generic;
using UnityEngine;

public class AbilityUIList : ActionSessionView
{
    [SerializeField] private BattleContext context;
    [SerializeField] private PlayerActionController controller;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private AbilityButton buttonPrefab;

    private List<AbilityButton> buttons;
    
    protected override void Refresh()
    {
        
    }

    protected override void OnSessionAttached(ActionSelectionSession newSession)
    {
        group.interactable = true;
        group.blocksRaycasts = true;
        group.alpha = 1.0f;

        buttons = new List<AbilityButton>(newSession.User.Abilities.Count);
        foreach (Ability ability in newSession.User.Abilities)
        {
            AbilityButton button = Instantiate(buttonPrefab, transform);
            buttons.Add(button);
            button.Initialize(ability, newSession);
        }
    }

    protected override void OnSessionDetached(ActionSelectionSession oldSession)
    {
        group.interactable = false;
        group.blocksRaycasts = false;
        group.alpha = 0.0f;

        if (buttons == null) return;
        foreach (AbilityButton button in buttons) Destroy(button.gameObject);
        buttons = null;
    }

    private void Start()
    {
        Bind(controller);
        group.interactable = false;
        group.blocksRaycasts = false;
        group.alpha = 0;
    }
}
