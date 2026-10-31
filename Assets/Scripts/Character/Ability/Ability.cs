using System.Collections.Generic;

public class Ability
{
    public enum CastType
    {
        Shot,
        Hex,
        Boon,
    }
    
    public enum AvailabilityResult
    {
        Available,
        Unavailable,
        InsufficientMana,
        OnCooldown,
        NoValidTarget,
    }
    
    public string Name { get; private set; }
    public int ManaCost { get; private set; }
    public CastType Cast { get; private set; }

    public Ability(string name, int manaCost, CastType cast)
    {
        Name = name;
        ManaCost = manaCost;
        Cast = cast;
    }

    public AvailabilityResult CheckAvailability(Combatant user, Battle battle)
    {
        if (user.Stats.CurrentMana < ManaCost) return AvailabilityResult.InsufficientMana;

        IReadOnlyList<Combatant> validTargets = battle.GetValidTargetsForAbility(user, this);
        return validTargets.Count == 0 ? AvailabilityResult.NoValidTarget : AvailabilityResult.Available;
    }
}
