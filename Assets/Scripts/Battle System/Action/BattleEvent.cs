public abstract class BattleEvent { }
public class InvalidActionEvent :  BattleEvent { }
public class ActionEndEvent : BattleEvent { }
public class PreActionEndEvent : BattleEvent { }

public class AbilityUsedEvent : BattleEvent
{
    public Combatant Source { get; }
    public Combatant Target { get; }
    public AbilityAction Action { get; }
    
    public AbilityUsedEvent(Combatant source, Combatant target, AbilityAction action)
    {
        Source = source;
        Target = target;
        Action = action;
    }
}

public class BattleEndedEvent : BattleEvent
{
    public BattleResult Result { get; }
    
    public BattleEndedEvent(BattleResult result) => Result = result;
}

public class ManaEvent : BattleEvent
{
    public int ManaDelta { get; }
    public int ManaBefore { get; }
    public int ManaAfter { get; }
    public Combatant Target { get; }

    public ManaEvent(int manaDelta, int manaBefore, int manaAfter, Combatant target)
    {
        ManaDelta = manaDelta;
        ManaBefore = manaBefore;
        ManaAfter = manaAfter;
        Target = target;
    }
}

public class DamageEvent : BattleEvent
{
    public DamageTags Tags { get; }
    public DamageType Type { get; }
    public int Damage { get; }
    public int HealthBeforeDamage { get; }
    public int HealthAfterDamage { get; }
    public Combatant Source { get; }
    public Combatant Target { get; }
    
    public DamageEvent(DamageTags tags, DamageType type, int damage, int healthBefore, int healthAfter, Combatant source, Combatant target)
    {
        Tags = tags;
        Type = type;
        Damage = damage;
        HealthBeforeDamage = healthBefore;
        HealthAfterDamage = healthAfter;
        Source = source;
        Target = target;
    }
}

public class StatusEffectAddedEvent : BattleEvent
{
    public StatusEffect StatusEffect { get; }
    public Combatant Source { get; }
    public Combatant Target { get; }

    public StatusEffectAddedEvent(StatusEffect statusEffect, Combatant source, Combatant target)
    {
        StatusEffect = statusEffect;
        Source = source;
        Target = target;
    }
}

public class StatusEffectRemovedEvent : BattleEvent
{
    public StatusEffect StatusEffect { get; }
    public Combatant Source { get; }

    public StatusEffectRemovedEvent(StatusEffect statusEffect, Combatant source)
    {
        StatusEffect = statusEffect;
        Source = source;
    }
}