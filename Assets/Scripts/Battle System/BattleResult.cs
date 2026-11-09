using System.Collections.Generic;

public enum BattleOutcome
{
    NotFinished,
    Victory,
    Defeat,
}

public readonly struct BattleResult
{
    public BattleOutcome Outcome { get; }
    public IReadOnlyList<Combatant> Defeated { get; }

    public BattleResult(BattleOutcome outcome, IReadOnlyList<Combatant> defeated = null)
    {
        Outcome = outcome;
        Defeated = defeated;
    }
}
