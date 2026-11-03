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
    public const int MaxOpponents = 4;

    public List<Combatant> Combatants { get; }
    public event Action<BattleResult> Ended;

    public Battle(IReadOnlyList<Combatant> combatants)
    {
        BattleValidityCheck(combatants);
        Combatants = combatants.ToList();

        foreach (Combatant combatant in Combatants) ApplyAbilityPassives(combatant);
        CombatantNaming.Assign(Combatants);
    }

    private static void BattleValidityCheck(IReadOnlyList<Combatant> combatants)
    {
        if (combatants == null)
            throw new System.ArgumentNullException(nameof(combatants));

        if (combatants.Any(combatant => combatant == null))
            throw new System.ArgumentException("Null combatant!", nameof(combatants));

        int allyCount = combatants.Count(combatant => combatant.CombatantType == CombatantType.Ally);
        if (allyCount < 1)
            throw new System.ArgumentException("There must be at least one ally in battle!", nameof(combatants));

        int opponentCount = combatants.Count(combatant => combatant.CombatantType == CombatantType.Opponent);
        if (opponentCount is < 1 or > MaxOpponents)
            throw new System.ArgumentException($"Battle needs 1 - {MaxOpponents} opponents!", nameof(combatants));
    }

    /// <summary>
    /// </summary>
    /// <param name="combatant"></param>
    public void AddCombatant(Combatant combatant)
    {
        if (combatant == null) return;
        if (Combatants.Contains(combatant)) return;

        Combatants.Add(combatant);
        ApplyAbilityPassives(combatant);
        
        CombatantNaming.Assign(Combatants);
    }

    public void Kill(Combatant combatant)
    {
        if (combatant == null) return;
        if (!Combatants.Contains(combatant)) return;
        
        combatant.MarkAsDead();
        
        if (HasLivingMembers(combatant.CombatantType)) return;

        foreach (Combatant toRemove in Combatants) RemoveAbilityPassives(toRemove);
        Ended?.Invoke(BuildResult());
    }

    public List<Combatant> GetValidTargetsForAbility(Combatant user, Ability ability)
    {
        var validTargets = new List<Combatant>();
        return ability.Type switch
        {
            AbilityType.Shot or AbilityType.Hex => GetEnemiesOfUser(user),
            AbilityType.Boon => GetTeammatesOfUser(user),
            _ => validTargets
        };
    }

    public bool IsValidTargetForAbility(Combatant user, Combatant target, Ability ability)
    {
        List<Combatant> validTargets = GetValidTargetsForAbility(user, ability);
        return validTargets.Contains(target);
    }

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
    
    private void ApplyAbilityPassives(Combatant combatant)
    {
        foreach (Ability ability in combatant.Abilities) ability.OnBattleStart(this);
    }

    private void RemoveAbilityPassives(Combatant combatant)
    {
        foreach (Ability ability in combatant.Abilities) ability.OnBattleEnd(this);
    }

    private BattleResult BuildResult()
    {
        if (!HasLivingMembers(CombatantType.Opponent))
        {
            return new BattleResult(BattleOutcome.Victory, GetDeadMembers(CombatantType.Opponent));
        }

        return !HasLivingMembers(CombatantType.Ally)
            ? new BattleResult(BattleOutcome.Defeat)
            : new BattleResult(BattleOutcome.Unknown);
    }
}