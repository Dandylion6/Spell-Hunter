using System.Collections.Generic;

public enum AbilityType
{
    Shot,
    Hex,
    Boon,
}

public enum AbilityStatus
{
    Ready,
    Unavailable,
    InsufficientMana,
    OnCooldown,
    NoValidTarget,
}

public class Ability
{
    public const int AugmentSlots = 3;
    public const int BaseShotCost = 1;
    public const int BaseHexCost = 6;
    public const int BaseBoonCost = 4;
    
    public string DisplayName { get; private set; }
    public CharacterStats User { get; private set; }
    public Stat ManaCost { get; }
    public AbilityType Type { get; }
    
    public IReadOnlyList<Augment> Augments => augments;
    private readonly List<Augment> augments = new(AugmentSlots);
    
    public Ability(string displayName, AbilityType type)
    {
        DisplayName = displayName;
        Type = type;
        ManaCost = Type switch
        {
            AbilityType.Shot => new Stat(BaseShotCost),
            AbilityType.Hex => new Stat(BaseHexCost),
            AbilityType.Boon => new Stat(BaseBoonCost),
            _ => throw new System.ArgumentOutOfRangeException(nameof(type))
        };
    }

    public Ability(string displayName, AbilityType type, IReadOnlyList<Augment> augments) : this(displayName, type)
    {
        if (augments == null)
            throw new System.ArgumentNullException(nameof(augments));
            
        if (augments.Count > AugmentSlots)
            throw new System.ArgumentOutOfRangeException(nameof(augments));

        foreach (Augment augment in augments) ApplyAugment(augment);
    }
    
    public AbilityStatus CheckStatus(Combatant user, Battle battle)
    {
        if (user.Stats.CurrentMana < ManaCost.Value) return AbilityStatus.InsufficientMana;

        IReadOnlyList<Combatant> validTargets = battle.GetValidTargetsForAbility(user, this);
        return validTargets.Count == 0 ? AbilityStatus.NoValidTarget : AbilityStatus.Ready;
    }
    
    public void SetUser(CharacterStats user) => User = user;
    public void ClearUser() => User = null;

    public void ApplyAugment(Augment newAugment)
    {
        if (newAugment == null) return;
        if (!newAugment.Definition.IsCompatibleWith(this)) return;
        
        if (augments.Contains(newAugment)) return;
        if (augments.Count == AugmentSlots) return;
        
        augments.Add(newAugment);
        newAugment.AttachToAbility(this);
    }

    public void RemoveAugment(Augment augment)
    {
        if (!augments.Contains(augment)) return;
        
        augments.Remove(augment);
        augment.DetachFromAbility();
    }

    public void OnBattleStart(Battle battle)
    {
        foreach (Augment augment in Augments) augment.OnBattleStart(battle);
    }

    public void OnBattleEnd(Battle battle)
    {
        foreach (Augment augment in Augments) augment.OnBattleEnd(battle);
    }

    public void OnCast(Battle battle, Combatant target)
    {
        foreach (Augment augment in Augments) augment.OnCast(battle, target);
    }

    public void OnHit(Battle battle, Combatant target)
    {
        foreach (Augment augment in Augments) augment.OnHit(battle, target);
    }
}
