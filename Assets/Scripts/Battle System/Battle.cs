using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///     Holds the state of a battle: who is fighting, whose turn it is, the action being resolved, and battle-related
///     queries.
/// </summary>
/// <remarks>
///     Teams are determined by <see cref="Combatant.CombatantType" />.
/// </remarks>
public class Battle
{
    public const int MaxOpponents = 3;

    public List<Combatant> Combatants { get; }

    public Battle(IReadOnlyList<Combatant> combatants)
    {
        BattleValidityCheck(combatants);
        Combatants = combatants.ToList();

        foreach (Ability ability in Combatants.SelectMany(combatant => combatant.Abilities))
        {
            ability.OnBattleStart(this);
        }
        CombatantNaming.Assign(Combatants);
    }

    private static void BattleValidityCheck(IReadOnlyList<Combatant> combatants)
    {
        if (combatants == null)
            throw new ArgumentNullException(nameof(combatants));

        if (combatants.Any(combatant => combatant == null))
            throw new ArgumentException("Null combatant!", nameof(combatants));

        int allyCount = combatants.Count(combatant => combatant.CombatantType == CombatantType.Ally);
        if (allyCount < 1)
            throw new ArgumentException("There must be at least one ally in battle!", nameof(combatants));

        int opponentCount = combatants.Count(combatant => combatant.CombatantType == CombatantType.Opponent);
        if (opponentCount is < 1 or > MaxOpponents)
            throw new ArgumentException($"Battle needs 1 - {MaxOpponents} opponents!", nameof(combatants));
    }

    /// <summary>
    /// </summary>
    /// <param name="combatant"></param>
    public void AddCombatant(Combatant combatant)
    {
        if (combatant == null) return;
        if (Combatants.Contains(combatant)) return;

        Combatants.Add(combatant);
        foreach (Ability ability in combatant.Abilities) ability.OnBattleStart(this);
        
        CombatantNaming.Assign(Combatants);
    }

    public BattleResult EvaluateBattleResult() => BuildResult();

    public List<Combatant> GetTeammatesOfUser(Combatant user)
    {
        return Combatants.Where(target => target.CombatantType == user.CombatantType).ToList();
    }

    public List<Combatant> GetEnemiesOfUser(Combatant user)
    {
        return Combatants.Where(target => target.CombatantType != user.CombatantType).ToList();
    }

    public List<Combatant> GetLivingMembers(CombatantType type)
    {
        return Combatants.Where(combatant => combatant.CombatantType == type && !combatant.IsDead).ToList();
    }

    public List<Combatant> GetDeadMembers(CombatantType type)
    {
        return Combatants.Where(combatant => combatant.CombatantType == type && combatant.IsDead).ToList();
    }

    public bool HasLivingMembers(CombatantType type)
    {
        return Combatants.Where(combatant => combatant.CombatantType == type).Any(combatant => !combatant.IsDead);
    }

    private BattleResult BuildResult()
    {
        if (!HasLivingMembers(CombatantType.Opponent))
        {
            return new BattleResult(BattleOutcome.Victory, GetDeadMembers(CombatantType.Opponent));
        }

        return !HasLivingMembers(CombatantType.Ally)
            ? new BattleResult(BattleOutcome.Defeat)
            : new BattleResult(BattleOutcome.NotFinished);
    }
}