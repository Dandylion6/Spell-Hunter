using System.Collections.Generic;
using System.Linq;

public class Fervor : SimpleStatus
{
    public Stat OutgoingPhysicalDamagePercent { get; }
    public Stat IncomingDamagePercent { get; }

    public Fervor(Combatant target, string display, int duration, int outgoingPhysicalDamagePercent,
        int incomingDamagePercent) : base(target, display, duration)
    {
        OutgoingPhysicalDamagePercent = new Stat(outgoingPhysicalDamagePercent);
        IncomingDamagePercent = new Stat(incomingDamagePercent);
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

    public override string GetTooltip()
    {
        throw new System.NotImplementedException();
    }
}