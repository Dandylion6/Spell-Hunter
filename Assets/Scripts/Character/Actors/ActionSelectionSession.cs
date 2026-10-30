using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionSession
{
    public enum SelectionState
    {
        ChoosingAction,
        ChoosingTarget,
        Completed,
    }

    public IReadOnlyList<Combatant> ValidTargets => validTargets;
    public Battle Battle { get; }
    public SelectionState State { get; private set; }
    public Combatant User { get; private set; }

    private Action onChange;
    private Action<BattleAction> onCompletion;
    
    private List<Combatant> validTargets;
    private Ability selectedAbility;
    private Combatant selectedTarget;
    
    public ActionSelectionSession(Battle battle,  Combatant user)
    {
        Battle = battle;
        User = user;
        State = SelectionState.ChoosingAction;
    }

    public void SubscribeToOnChange(Action callback) => onChange += callback;
    public void UnsubscribeFromOnChange(Action callback) => onChange -= callback;
    
    public void SubscribeToOnCompletion(Action<BattleAction> callback) => onCompletion += callback;
    public void UnsubscribeFromOnCompletion(Action<BattleAction> callback) => onCompletion -= callback;
    
    public void SelectAbility(Ability ability)
    {
        selectedTarget = null;
        selectedAbility = ability;

        validTargets = Battle.GetValidTargetsForAbility(User, selectedAbility);

        bool canAutoSelect = validTargets.Count == 1;
        if (canAutoSelect)
        {
            selectedTarget = validTargets[0];
            CompleteSelection();
            return;
        }
        
        State = SelectionState.ChoosingTarget;
        onChange?.Invoke();
    }

    public void SelectTarget(Combatant target)
    {
        if (selectedAbility == null) return;
        if (!Battle.IsValidTargetForAbility(User, target, selectedAbility)) return;
        
        selectedTarget = target;
        CompleteSelection();
    }

    private void CompleteSelection()
    {
        State = SelectionState.Completed;
        BattleAction action = new(User, selectedAbility, selectedTarget);
        onCompletion?.Invoke(action);
    }
}
