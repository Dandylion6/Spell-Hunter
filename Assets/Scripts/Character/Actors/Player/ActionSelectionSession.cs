using System;
using System.Collections.Generic;

public class ActionSelectionSession
{
    public enum SelectionState
    {
        ChoosingAction,
        ChoosingTarget,
        Completed
    }

    private Ability selectedAbility;
    private Combatant selectedTarget;

    private List<Combatant> validTargets;

    public ActionSelectionSession(Battle battle, Combatant user)
    {
        Battle = battle;
        User = user;
        State = SelectionState.ChoosingAction;
    }

    public IReadOnlyList<Combatant> ValidTargets => validTargets;
    public Battle Battle { get; }
    public SelectionState State { get; private set; }
    public Combatant User { get; }
    public event Action Changed;
    public event Action<BattleAction> ActionChosen;
    
    // TODO: Make the system decoupled from Ability if possible.

    /// <summary>
    ///     Selects an action that isn't an ability and completes the session.
    /// </summary>
    /// <param name="action">
    ///     Is a pre-defined action that doesn't require a target.
    /// </param>
    public void SelectAction(BattleAction action) => CompleteSelection(action);

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
            CompleteSelection(new AbilityAction(User, selectedAbility, selectedTarget));
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
        CompleteSelection(new AbilityAction(User, selectedAbility, selectedTarget));
    }

    private void CompleteSelection(BattleAction action)
    {
        State = SelectionState.Completed;
        Changed?.Invoke();
        ActionChosen?.Invoke(action);
    }
}