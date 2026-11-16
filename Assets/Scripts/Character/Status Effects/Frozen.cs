using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FrozenBaseStats
{
    [SerializeField] [Min(0)] private int frozenTurns = 1;
    [SerializeField] [Min(0)] private int shatterDamageThreshold = 12;
    [SerializeField] [Min(0)] private int thawTurns = 2;
    [SerializeField] private bool ignorePhysicalResistanceOnShatter = true;
    [SerializeField] private bool isImmuneToShock = true;

    public int FrozenTurns => frozenTurns;
    public int ShatterDamageThreshold => shatterDamageThreshold;
    public int ThawTurns => thawTurns;
    public bool IgnorePhysicalResistanceOnShatter => ignorePhysicalResistanceOnShatter;
    public bool IsImmuneToShock => isImmuneToShock;
}

public class Frozen : StatusEffect
{
    public override bool IsExpired => TurnsRemaining <= 0;
    
    public int ShatterDamageThreshold { get; }
    public bool IgnorePhysicalResistanceOnShatter { get; }
    public int ThawTurns { get; }
    public bool IsImmuneToShock { get; } // TODO: Add immunity to all elements (on combatant.)
    public int TurnsRemaining { get; private set; }

    public Frozen(Combatant target, FrozenBaseStats stats) : base(target, "Frozen")
    {
        TurnsRemaining = stats.FrozenTurns;
        ShatterDamageThreshold = stats.ShatterDamageThreshold;
        ThawTurns = stats.ThawTurns;
        IgnorePhysicalResistanceOnShatter = stats.IgnorePhysicalResistanceOnShatter;
        IsImmuneToShock = stats.IsImmuneToShock;
    }

    public override void OnApply(List<BattleEvent> events)
    {
        if (!IsImmuneToShock) return;
        Target.Stats.ShockImmunity.AssignModifier(new StatModifier(this, StatModifierType.Flat, 1.0f));
    }

    public override void OnRemove() => Target.Stats.ShockImmunity.RemoveModifiersFromSource(this);

    public override void OnBeforeAction(Battle battle, Turn currentTurn, List<BattleEvent> events)
    {
        if (currentTurn.TrySetAction(new SkipAction("Frozen", Target, TurnClock.ClockThreshold)))
        {
            events.Add(new ActionSkippedEvent(Target, "Frozen"));
        }

        --TurnsRemaining;
        if (TurnsRemaining == 0) ProcThaw(events);
    }

    public override void OnTargeted(CastContext context, List<BattleEvent> events)
    {
        if (IsExpired) return;
        
        Damage firstPhysicalDamage = context.Damage.FirstOrDefault(damage =>
            damage.Type == DamageType.Physical && damage.Amount.Value >= ShatterDamageThreshold);
        
        if (firstPhysicalDamage == null) return;

        TurnsRemaining = 0;
        if (IgnorePhysicalResistanceOnShatter) firstPhysicalDamage.SetIgnoreResistance(true);
        ProcThaw(events);
    }

    private void ProcThaw(List<BattleEvent> events)
    {
        Thaw thaw = new(Target, ThawTurns);
        Target.AddStatusEffect(thaw, events);
        events.Add(new StatusEffectAddedEvent(thaw, Target, Target));
    }
}