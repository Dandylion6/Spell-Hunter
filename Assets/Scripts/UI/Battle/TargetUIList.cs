using System.Collections.Generic;
using UnityEngine;

public class TargetUIList : ActionSessionView
{
    [SerializeField] private BattleContext context;
    [SerializeField] private PlayerActionController controller;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TargetButton buttonPrefab;

    private List<TargetButton> buttons;
    
    protected override void Refresh()
    {
        if (ActiveSession.State != ActionSelectionSession.SelectionState.ChoosingTarget)
        {
            Toggle(false);
            return;
        }

        buttons = new List<TargetButton>(ActiveSession.ValidTargets.Count);
        foreach (Combatant target in ActiveSession.ValidTargets)
        {
            TargetButton button = Instantiate(buttonPrefab, group.transform);
            buttons.Add(button);
            button.Initialize(target, ActiveSession);
        }
        
        Toggle(true);
    }

    protected override void OnSessionAttached(ActionSelectionSession newSession)
    {
        Toggle(false);
    }

    protected override void OnSessionDetached(ActionSelectionSession oldSession)
    {
        Toggle(false);
    }

    private void Toggle(bool active)
    {
        group.interactable = active;
        group.blocksRaycasts = active;
        group.alpha = active ? 1.0f : 0.0f;

        if (active || buttons == null) return;
        
        foreach (TargetButton button in buttons) Destroy(button.gameObject);
        buttons = null;
    }

    private void Start()
    {
        Bind(controller);
        Toggle(false);
    }
}