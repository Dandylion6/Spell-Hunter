using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleStatusDefinition<T> : StatusDefinition<T> where T : SimpleStatus
{
    [SerializeField] [Range(1, 10)] protected int duration = 2;
}

public abstract class SimpleStatus : StatusEffect
{
    public override bool IsExpired => DurationRemaining <= 0;

    public int MaxDuration { get; }
    public int DurationRemaining { get; protected set; }

    protected SimpleStatus(Combatant target, string displayName, int duration) : base(target, displayName)
    {
        MaxDuration = duration;
        DurationRemaining = duration;
    }

    public override void OnRefresh(StatusEffect incomingEffect, List<BattleEvent> events) =>
        DurationRemaining = MaxDuration;

    public override void OnBeforeAction(Battle battle, Turn currentTurn, List<BattleEvent> events) =>
        --DurationRemaining;
}