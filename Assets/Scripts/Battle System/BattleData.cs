using System.Collections.Generic;
using UnityEngine;

public class BattleData
{
    public IReadOnlyList<Combatant> Combatants => combatants;
    private readonly List<Combatant> combatants;
    
    public Combatant PreviousCombatant { get; private set; }
    public Combatant ActiveCombatant { get; private set; }
    public BattleAction Action { get; private set; }

    public BattleData(List<Combatant> combatants)
    {
        this.combatants = combatants;
    }

    public void AddCombatant(Combatant combatant)
    {
        if (combatant == null) return;
        if (combatants.Contains(combatant)) return;
        combatants.Add(combatant);
    }

    public void RemoveCombatant(Combatant combatant)
    {
        if (combatant == null) return;
        if (!combatants.Contains(combatant)) return;
        combatants.Remove(combatant);
    }
    
    public void SetActiveCombatant(Combatant activeCombatant)
    {
        PreviousCombatant = ActiveCombatant;
        ActiveCombatant = activeCombatant;
    }

    public void AssignAction(BattleAction action)
    {
        if (action == null) return;
        if (Action != null) return;
        Action = action;
    }
    
    public void ClearAction() => Action = null;
}
