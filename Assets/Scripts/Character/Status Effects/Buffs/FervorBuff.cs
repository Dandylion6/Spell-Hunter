using System.Linq;
using UnityEngine;

public class FervorBuffBaseStats : BasicBuffBaseStats
{
    [SerializeField] private int outgoingPhysicalDamagePercent;
    [SerializeField] private int incomingDamagePercent;

    public int OutgoingPhysicalDamagePercent => outgoingPhysicalDamagePercent;
    public int IncomingDamagePercent => incomingDamagePercent;
}

public class FervorBuff : BasicBuff
{
    public int OutgoingPhysicalDamagePercent { get; }
    public int IncomingDamagePercent { get; }

    public FervorBuff(Combatant target, FervorBuffBaseStats stats) :
        base(target, "Fervor", stats)
    {
        OutgoingPhysicalDamagePercent = stats.OutgoingPhysicalDamagePercent;
        IncomingDamagePercent = stats.IncomingDamagePercent;
    }

    public override void OnApply()
    {
        Target.Stats.IncomingDamagePercentage.AssignModifier(new StatModifier(this, StatModifierType.Flat,
            IncomingDamagePercent));
    }

    public override void OnRemove()
    {
        Target.Stats.IncomingDamagePercentage.RemoveModifiersFromSource(this);
    }

    public override void OnModify(CastContext context)
    {
        StatModifier damageAmp = new(this, StatModifierType.Percentage, OutgoingPhysicalDamagePercent);
        foreach (Damage damage in context.Damage.Where(damage => damage.Type == DamageType.Physical))
        {
            damage.Amount.AssignModifier(damageAmp);
        }
    }
}