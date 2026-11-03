using System.Collections.Generic;

public static class CombatantNaming
{
    public static void Assign(IReadOnlyList<Combatant> combatants)
    {
        Dictionary<string, int> totals = new();
        foreach (Combatant combatant in combatants)
        {
            string baseName = combatant.Stats.BaseName;
            totals[baseName] += 1;
        }

        Dictionary<string, int> seen = new();
        foreach (Combatant combatant in combatants)
        {
            string baseName = combatant.Stats.BaseName;
            int index = seen.GetValueOrDefault(baseName);
            int total = totals.GetValueOrDefault(baseName);
            
            if (total > 1) AssignPrefix(combatant, index);
            else combatant.SetName(combatant.Stats.BaseName);
            
            seen.Add(combatant.Stats.BaseName, total);
        }
    }

    private static void AssignPrefix(Combatant combatant, int index)
    {
        if (index < 0 ) return;

        char suffix = (char)('A' + index);
        if (suffix > 'Z') return;
        
        combatant.SetName($"{combatant.Stats.BaseName} {suffix}");
    }
}