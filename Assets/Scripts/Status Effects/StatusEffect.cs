using System.Collections.Generic;
using UnityEngine;

public abstract class StatusDefinition<T> : ScriptableObject where T : StatusEffect
{
    [SerializeField] protected string displayName = "New Status";

    public abstract T CreateEffect(Combatant target);
}

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
    
    public abstract string GetTooltip();
}