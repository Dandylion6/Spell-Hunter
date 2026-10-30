using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
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
    
    public string Name { get; private set; } = "Ability";
    public int ManaCost { get; private set; }
    public CastType Cast { get; private set; }

    public abstract AvailabilityResult CheckAvailability(Combatant user, Battle battle);
}
