using System.Collections.Generic;
using UnityEngine;

public class Chilled : SimpleStatus
{
    public Stat SpeedReductionPercent { get; private set; }
    public Stat FreezeBuildThreshold { get; private set; }
    public int FreezeBuildUp { get; private set; }
    public FrozenDefinition Frozen { get; private set; }

    public Chilled(Combatant target, string displayName, int duration, int speedReductionPercent,
        int freezeBuildThreshold, FrozenDefinition frozen) : base(target, displayName, duration)
    {
        SpeedReductionPercent = new Stat(speedReductionPercent);
        FreezeBuildThreshold = new Stat(freezeBuildThreshold);
        Frozen = frozen;
    }

    private void UpdateSpeedReduction()
    {
        Target.Stats.Speed.RemoveModifiersFromSource(this);
        Target.Stats.Speed.AssignModifier(new StatModifier(this, StatModifierType.Percentage,
            -SpeedReductionPercent.Value));
    }

    private void UpdateFreeze(List<BattleEvent> events)
    {
        Frozen frozen = Frozen.CreateEffect(Target);
        if (!Target.Stats.IsImmuneTo(frozen))
        {
            ++FreezeBuildUp;
        }

        if (FreezeBuildUp < FreezeBuildThreshold.Value)
        {
            UpdateSpeedReduction();
            return;
        }

        if (!Target.AddStatusEffect(frozen, events)) return;
        DurationRemaining = 0; // This will expire the chilled status.
        events.Add(new StatusEffectAddedEvent(frozen, Target, Target));
    }

    public override void OnApply(List<BattleEvent> events) => UpdateFreeze(events);

    public override void OnRemove()
    {
        Target.Stats.Speed.RemoveModifiersFromSource(this);
    }

    public override void OnRefresh(StatusEffect incomingEffect, List<BattleEvent> events)
    {
        if (incomingEffect is Chilled newChilled)
        {
            SpeedReductionPercent = newChilled.SpeedReductionPercent;
            FreezeBuildThreshold = newChilled.FreezeBuildThreshold;
            Frozen = newChilled.Frozen;
        }

        base.OnRefresh(incomingEffect, events);
        UpdateFreeze(events);
    }

    public override string GetTooltip()
    {
        throw new System.NotImplementedException();
    }
}