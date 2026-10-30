using UnityEngine;

public class Turn
{
    public int Index { get; private set; }
    
    /// <summary>
    ///     The combatant whose turn it currently is.
    /// </summary>
    public Combatant Actor { get; private set; }
    
    /// <summary>
    ///     The action currently assigned for execution, or null if none is pending.
    /// </summary>
    public BattleAction Action { get; private set; }

    public Turn(Combatant actor, int index)
    {
        Actor = actor;
        Index = index;
    }
    
    public void SetAction(BattleAction action) => Action ??= action;
}
