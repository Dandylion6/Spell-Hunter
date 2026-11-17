using System.Collections.Generic;
using System.Linq;

public class Frozen : SimpleStatus
{
    public int ShatterDamageThreshold { get; }
    public bool IgnorePhysicalResistanceOnShatter { get; }
    public int ThawDuration { get; }
    public bool IsImmuneToShock { get; } // TODO: Add immunity to all elements (on combatant.)

    public Frozen(Combatant target, string displayName, int duration, int shatterDamageThreshold, int thawDuration,
        bool ignorePhysicalResistanceOnShatter, bool isImmuneToShock) : base(target, displayName, duration)
    {
        ShatterDamageThreshold = shatterDamageThreshold;
        ThawDuration = thawDuration;
        IgnorePhysicalResistanceOnShatter = ignorePhysicalResistanceOnShatter;
        IsImmuneToShock = isImmuneToShock;
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

        base.OnBeforeAction(battle, currentTurn, events);

        if (DurationRemaining == 0) ProcThaw(events);
    }

    public override void OnTargeted(CastContext context, List<BattleEvent> events)
    {
        if (IsExpired) return;

        Damage firstPhysicalDamage = context.Damage.FirstOrDefault(damage =>
            damage.Type == DamageType.Physical && damage.Amount.Value >= ShatterDamageThreshold);

        if (firstPhysicalDamage == null) return;

        DurationRemaining = 0;
        if (IgnorePhysicalResistanceOnShatter) firstPhysicalDamage.SetIgnoreResistance(true);
        ProcThaw(events);
    }

    public override string GetTooltip()
    {
        throw new System.NotImplementedException();
    }

    private void ProcThaw(List<BattleEvent> events)
    {
        Thaw thaw = new(Target, ThawDuration);
        Target.AddStatusEffect(thaw, events);
        events.Add(new StatusEffectAddedEvent(thaw, Target, Target));
    }
}