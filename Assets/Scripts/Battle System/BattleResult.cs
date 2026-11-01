using System.Collections.Generic;
using UnityEngine;

public enum BattleOutcome
{
    Victory,
    Defeat,
}

public readonly struct BattleResult
{
    public BattleOutcome Outcome { get; }
    public IReadOnlyList<Combatant> Defeated { get; }

    public BattleResult(BattleOutcome outcome, IReadOnlyList<Combatant> defeated)
    {
        Outcome = outcome;
        Defeated = defeated;
    }
}
