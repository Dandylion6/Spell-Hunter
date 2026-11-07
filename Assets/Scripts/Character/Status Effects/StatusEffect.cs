using System.Collections.Generic;

public abstract class StatusEffect
{
    public Combatant Target { get; }
    public abstract bool IsExpired { get; }

    public StatusEffect(Combatant target)
    {
        Target = target;
    }
    
    public virtual void OnApply() { }
    public virtual void OnRefresh(StatusEffect incomingEffect = null) { }
    public virtual void OnRemove() { }
    
    public virtual void OnBeforeAction(Battle battle, List<BattleEvent> events) { }
    public virtual void OnAfterAction(Battle battle, List<BattleEvent> events) { }
    
    public virtual void OnCast(CastContext context) { }
    public virtual void OnModify(CastContext context) { }
}

public abstract class BasicBuff : StatusEffect
{
    public override bool IsExpired => TurnsRemaining <= 0;
    
    public int MaxTurns { get; }
    public int TurnsRemaining { get; private set; }
    
    protected BasicBuff(Combatant target, int turns) : base(target)
    {
        MaxTurns = turns;
        TurnsRemaining = turns;
    }

    public override void OnRefresh(StatusEffect incomingEffect = null) => TurnsRemaining = MaxTurns;

    public override void OnBeforeAction(Battle battle, List<BattleEvent> events) => --TurnsRemaining;
}