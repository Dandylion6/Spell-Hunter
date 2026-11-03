using System.Collections.Generic;

public enum AbilityStatus
{
    Ready,
    Unavailable,
    InsufficientMana,
    OnCooldown,
    NoValidTarget,
}

public abstract class Ability
{
    public const int AugmentSlots = 3;
    
    public string DisplayName { get; private set; }
    public Combatant User { get; }
    public Stat ManaCost { get; protected set; }
    
    public IReadOnlyList<Augment> Augments => augments;
    private readonly List<Augment> augments = new(AugmentSlots);

    public Ability(string displayName, Combatant user)
    {
        DisplayName = displayName;
        User = user;
        ManaCost = new Stat();
    }
    
    public AbilityStatus CheckStatus(Battle battle)
    {
        if (User.Stats.CurrentMana < ManaCost.Value) return AbilityStatus.InsufficientMana;

        IReadOnlyList<Combatant> validTargets = GetValidTargets(battle);
        return validTargets.Count == 0 ? AbilityStatus.NoValidTarget : AbilityStatus.Ready;
    }

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

    public abstract List<Combatant> GetValidTargets(Battle battle);

    public bool IsValidTarget(Battle battle, Combatant target)
    {
        if (target == null) return false;
        List<Combatant> validTargets = GetValidTargets(battle);
        return validTargets.Contains(target);
    }

    public abstract void Execute(Battle battle, List<BattleEvent> events, Combatant target);

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
