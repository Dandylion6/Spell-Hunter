using System.Collections.Generic;
using System.Linq;

/// <summary>
///     Holds the state of a battle: who is fighting, whose turn it is, the action being resolved, and battle-related queries. 
/// </summary>
/// <remarks>
///     Teams are determined by <see cref="Combatant.CombatantType"/>.
/// </remarks>
public class Battle
{
    /// <summary>
    ///     A read-only view of every combatant in the battle.
    /// </summary>
    public IReadOnlyList<Combatant> Combatants => combatants;
    private readonly List<Combatant> combatants;
    
    public Battle(List<Combatant> combatants)
    {
        this.combatants = combatants;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="combatant"></param>
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
    
    public List<Combatant> GetValidTargetsForAbility(Combatant user, Ability ability)
    {
        var validTargets = new List<Combatant>();
        return ability.Cast switch
        {
            Ability.CastType.Shot or Ability.CastType.Hex => GetEnemiesOfUser(user),
            Ability.CastType.Boon => GetTeammatesOfUser(user),
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
        return combatants.Where(target => target.CombatantType == user.CombatantType).ToList();
    }

    public List<Combatant> GetEnemiesOfUser(Combatant user)
    {
        return combatants.Where(target => target.CombatantType != user.CombatantType).ToList();
    }
}
