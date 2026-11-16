using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public string DisplayName { get; }
    public Combatant Target { get; }
    public abstract bool IsExpired { get; }

    public StatusEffect(Combatant target, string displayName)
    {
        Target = target;
        DisplayName = displayName;
    }
    
    public virtual void OnApply(List<BattleEvent> events) { }
    public virtual void OnRefresh(StatusEffect incomingEffect, List<BattleEvent> events) { }
    public virtual void OnRemove() { }
    
    public virtual void OnBeforeAction(Battle battle, Turn currentTurn, List<BattleEvent> events) { }
    public virtual void OnAfterAction(Battle battle, List<BattleEvent> events) { }
    public virtual void OnTargeted(CastContext context, List<BattleEvent> events) { }
    
    public virtual void OnCast(CastContext context) { }
    public virtual bool CanBeApplied() => !Target.Stats.IsImmuneTo(this);
    public virtual void OnModify(CastContext context) { }
}

[System.Serializable]
public class BasicBuffBaseStats
{
    [SerializeField] private int buffTurns;
    
    public int BuffTurns => buffTurns;
}

public class BasicStatus : StatusEffect
{
    public override bool IsExpired => TurnsRemaining <= 0;
    
    public int MaxTurns { get; }
    public int TurnsRemaining { get; protected set; }
    
    protected BasicStatus(Combatant target, string displayName, BasicBuffBaseStats stats) : base(target, displayName)
    {
        MaxTurns = stats.BuffTurns;
        TurnsRemaining = stats.BuffTurns;
    }

    public BasicStatus(Combatant target, string displayName, int buffTurns) : base(target, displayName)
    {
        MaxTurns = buffTurns;
        TurnsRemaining = buffTurns;
    }

    public override void OnRefresh(StatusEffect incomingEffect, List<BattleEvent> events) => TurnsRemaining = MaxTurns;

    public override void OnBeforeAction(Battle battle, Turn currentTurn, List<BattleEvent> events) => --TurnsRemaining;
}