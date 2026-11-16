using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FervorBuffBaseStats : BasicBuffBaseStats
{
    [SerializeField] private int outgoingPhysicalDamagePercent;
    [SerializeField] private int incomingDamagePercent;

    public int OutgoingPhysicalDamagePercent => outgoingPhysicalDamagePercent;
    public int IncomingDamagePercent => incomingDamagePercent;
}

public class FervorStatus : BasicStatus
{
    public Stat OutgoingPhysicalDamagePercent { get; }
    public Stat IncomingDamagePercent { get; }

    public FervorStatus(Combatant target, FervorBuffBaseStats stats) :
        base(target, "Fervor", stats)
    {
        OutgoingPhysicalDamagePercent = new Stat(stats.OutgoingPhysicalDamagePercent);
        IncomingDamagePercent = new Stat(stats.IncomingDamagePercent);
    }

    public override void OnApply(List<BattleEvent> events)
    {
        Target.Stats.IncomingDamagePercentage.AssignModifier(new StatModifier(this, StatModifierType.Flat,
            IncomingDamagePercent.Value));
    }

    public override void OnRemove()
    {
        Target.Stats.IncomingDamagePercentage.RemoveModifiersFromSource(this);
    }

    public override void OnModify(CastContext context)
    {
        StatModifier damageAmp = new(this, StatModifierType.Percentage, OutgoingPhysicalDamagePercent.Value);
        foreach (Damage damage in context.Damage.Where(damage => damage.Type == DamageType.Physical))
        {
            damage.Amount.AssignModifier(damageAmp);
        }
    }
}