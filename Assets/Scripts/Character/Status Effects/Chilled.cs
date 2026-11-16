using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChilledBaseStats : BasicBuffBaseStats
{
    [SerializeField] private int speedReductionPercent = 20;
    [SerializeField] private int freezeBuildThreshold = 2;

    public int SpeedReductionPercent => speedReductionPercent;
    public int FreezeBuildThreshold => freezeBuildThreshold;
}

public class Chilled : BasicStatus
{
    public Stat SpeedReductionPercent { get; private set; }
    public Stat FreezeBuildThreshold { get; private set; }
    public int FreezeBuildUp { get; private set; }
    public FrozenBaseStats FrozenStats { get; private set; }

    public Chilled(Combatant target, ChilledBaseStats stats, FrozenBaseStats frozenStats) : base(target, "Chilled",
        stats)
    {
        SpeedReductionPercent = new Stat(stats.SpeedReductionPercent);
        FreezeBuildThreshold = new Stat(stats.FreezeBuildThreshold);
        FrozenStats = frozenStats;
    }

    private void UpdateSpeedReduction()
    {
        Target.Stats.Speed.RemoveModifiersFromSource(this);
        Target.Stats.Speed.AssignModifier(new StatModifier(this, StatModifierType.Percentage,
            -SpeedReductionPercent.Value));
    }

    private void UpdateFreeze(List<BattleEvent> events)
    {
        Frozen frozen = new(Target, FrozenStats);
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
        TurnsRemaining = 0; // This will expire the chilled status.
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
            FrozenStats = newChilled.FrozenStats;
        }

        base.OnRefresh(incomingEffect, events);
        UpdateFreeze(events);
    }
}