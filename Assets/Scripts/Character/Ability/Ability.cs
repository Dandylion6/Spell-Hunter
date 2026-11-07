using System.Collections.Generic;
using System.Linq;

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
    public const int AugmentSlots = 4;
    
    public string DisplayName { get; private set; }
    public Combatant User { get; }
    public Stat ManaCost { get; protected set; }
    public Stat TurnCost { get; protected set; }
    public AugmentTags BaseTags { get; protected set; }
    
    public IReadOnlyList<Augment> Augments => augments;
    private readonly List<Augment> augments = new(AugmentSlots);

    public Ability(string displayName, Combatant user, AbilityBaseStats baseStats)
    {
        DisplayName = displayName;
        User = user;
        TurnCost = new Stat(baseStats.TurnCost);
        ManaCost = new Stat(baseStats.ManaCost);
    }
    
    public AbilityStatus CheckStatus(Battle battle)
    {
        if (User.Stats.CurrentMana < ManaCost.Value) return AbilityStatus.InsufficientMana;

        IReadOnlyList<Combatant> validTargets = GetValidTargets(battle);
        return validTargets.Count == 0 ? AbilityStatus.NoValidTarget : AbilityStatus.Ready;
    }

    public AugmentTags GetTags()
    {
        return Augments.Aggregate(BaseTags, (current, augment) => current | augment.Definition.Additions);
    }

    public void ApplyAugment(Augment newAugment)
    {
        if (newAugment == null) return;
        if (!newAugment.Definition.IsCompatibleWith(this)) return;
        
        if (augments.Contains(newAugment)) return;
        if (augments.Count == AugmentSlots) return;
        
        newAugment.AttachToAbility(this);
        augments.Add(newAugment);
    }

    public void RemoveAugment(Augment augment)
    {
        if (!augments.Contains(augment)) return;
        
        augment.DetachFromAbility();
        augments.Remove(augment);
    }

    public abstract List<Combatant> GetValidTargets(Battle battle);

    public bool IsValidTarget(Battle battle, Combatant target)
    {
        if (target == null) return false;
        List<Combatant> validTargets = GetValidTargets(battle);
        return validTargets.Contains(target);
    }

    public void Execute(Battle battle, List<BattleEvent> events, Combatant target)
    {
        CastContext context = new(this, battle, target);
        
        int oldMana = User.Stats.CurrentMana;
        int newMana = User.Stats.ConsumeMana(ManaCost.Value);
        events.Add(new ManaEvent(ManaCost.Value, newMana, oldMana, User));

        try
        {
            BuildScaffold(context);

            foreach (Augment augment in augments) augment.OnCast(context);
            foreach (StatusEffect effect in User.StatusEffects) effect.OnCast(context);
            
            ApplyCastRules(context);
            
            foreach (Augment augment in augments) augment.OnModify(context);
            foreach (StatusEffect effect in User.StatusEffects) effect.OnModify(context);

            if (!CanHit(context)) return;

            Resolve(context, events);
            foreach (Augment augment in augments) augment.OnHit(context);
        }
        finally
        {
            context.Cleanup();
        }
    }

    public void OnBattleStart(Battle battle)
    {
        foreach (Augment augment in Augments) augment.OnBattleStart(battle);
    }

    public void OnBattleEnd(Battle battle)
    {
        foreach (Augment augment in Augments) augment.OnBattleEnd(battle);
    }
    
    protected virtual void BuildScaffold(CastContext context) { }
    protected virtual void ApplyCastRules(CastContext context) { }
    protected virtual bool CanHit(CastContext context) => true;

    protected virtual void Resolve(CastContext context, List<BattleEvent> events)
    {
        foreach (Healing heal in context.Healing)
        {
            context.Ability.User.Stats.Heal(heal.Amount.Value);
        }
        
        foreach (Damage damage in context.Damage.Where(damage => damage.Tags.HasFlag(DamageTags.OnHit)))
        {
            int healthBefore = context.Target.Stats.CurrentHealth;
            int damageTaken = context.Target.Stats.TakeDamage(damage);
            int healthAfter = context.Target.Stats.CurrentHealth;
            
            events.Add(new DamageEvent(damage.Tags, damage.Type,  damageTaken, healthBefore, healthAfter, context.Target));
        }

        foreach (StatusEffect effect in context.StatusEffects)
        {
            effect.Target.AddStatusEffect(effect); // Will try to apply the status effect (or refresh it)
        }
    }
}
