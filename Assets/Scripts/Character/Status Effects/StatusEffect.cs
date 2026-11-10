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
    
    public virtual void OnApply() { }
    public virtual void OnRefresh(StatusEffect incomingEffect) { }
    public virtual void OnRemove() { }
    
    public virtual void OnBeforeAction(Battle battle, List<BattleEvent> events) { }
    public virtual void OnAfterAction(Battle battle, List<BattleEvent> events) { }
    
    public virtual void OnCast(CastContext context) { }
    public virtual void OnModify(CastContext context) { }
}

[System.Serializable]
public class BasicBuffBaseStats
{
    [SerializeField] private int buffTurns;
    
    public int BuffTurns => buffTurns;
}

public abstract class BasicBuff : StatusEffect
{
    public override bool IsExpired => TurnsRemaining <= 0;
    
    public int MaxTurns { get; }
    public int TurnsRemaining { get; private set; }
    
    protected BasicBuff(Combatant target, string displayName, BasicBuffBaseStats stats) : base(target, displayName)
    {
        MaxTurns = stats.BuffTurns;
        TurnsRemaining = stats.BuffTurns;
    }

    public override void OnRefresh(StatusEffect incomingEffect) => TurnsRemaining = MaxTurns;

    public override void OnBeforeAction(Battle battle, List<BattleEvent> events) => --TurnsRemaining;
}