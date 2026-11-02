using System.Collections.Generic;
using Mono.Cecil.Cil;

public class Ability
{
    public const int AugmentSlots = 3;
    public const int BaseShotCost = 1;
    public const int BaseHexCost = 6;
    public const int BaseBoonCost = 4;
    
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
    public Stat ManaCost { get; }
    public CastType Cast { get; }
    
    public IReadOnlyList<IAugment> Augments => augments;
    private readonly List<IAugment> augments = new(AugmentSlots);
    
    public Ability(string name, CastType cast)
    {
        Name = name;
        Cast = cast;
        ManaCost = Cast switch
        {
            CastType.Shot => new Stat(BaseShotCost),
            CastType.Hex => new Stat(BaseHexCost),
            CastType.Boon => new Stat(BaseBoonCost),
            _ => throw new System.ArgumentOutOfRangeException(nameof(cast))
        };
    }

    public Ability(string name, CastType cast, IReadOnlyList<IAugment> augments) : this(name, cast)
    {
        if (augments == null)
            throw new System.ArgumentNullException(nameof(augments));
            
        if (augments.Count > AugmentSlots)
            throw new System.ArgumentOutOfRangeException(nameof(augments));

        foreach (IAugment augment in augments) ApplyAugment(augment);
    }

    public AvailabilityResult CheckAvailability(Combatant user, Battle battle)
    {
        if (user.Stats.CurrentMana < ManaCost.Value) return AvailabilityResult.InsufficientMana;

        IReadOnlyList<Combatant> validTargets = battle.GetValidTargetsForAbility(user, this);
        return validTargets.Count == 0 ? AvailabilityResult.NoValidTarget : AvailabilityResult.Available;
    }

    public void ApplyAugment(IAugment newAugment)
    {
        if (newAugment == null) return;
        if (augments.Contains(newAugment)) return;
        if (augments.Count == AugmentSlots) return;
        
        augments.Add(newAugment);
        newAugment.ApplyPassive(this);
    }

    public void RemoveAugment(IAugment augment)
    {
        if (!augments.Contains(augment)) return;
        
        augments.Remove(augment);
        augment.RemovePassive(this);
    }
}
