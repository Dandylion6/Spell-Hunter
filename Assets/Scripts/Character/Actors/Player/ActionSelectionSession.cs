using System;
using System.Collections.Generic;

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
    public event Action Changed;
    public event Action<BattleAction> ActionChosen;
    
    private List<Combatant> validTargets;
    private Ability selectedAbility;
    private Combatant selectedTarget;
    
    public ActionSelectionSession(Battle battle,  Combatant user)
    {
        Battle = battle;
        User = user;
        State = SelectionState.ChoosingAction;
    }
    
    public void SelectAbility(Ability ability)
    {
        selectedTarget = null;

        bool isDeselecting = selectedAbility == ability;
        if (isDeselecting)
        {
            selectedAbility = null;
            State = SelectionState.ChoosingAction;
            Changed?.Invoke();
            return;
        }
        
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
        Changed?.Invoke();
    }

    public void SelectTarget(Combatant target)
    {
        if (selectedAbility == null) return;
        if (State == SelectionState.Completed) return;
        
        if (!Battle.IsValidTargetForAbility(User, target, selectedAbility)) return;
        
        selectedTarget = target;
        CompleteSelection();
    }
    
    private void CompleteSelection()
    {
        State = SelectionState.Completed;
        BattleAction action = new(User, selectedAbility, selectedTarget);
        Changed?.Invoke();
        ActionChosen?.Invoke(action);
    }
}
