using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ember Augment", menuName = "Objects/Augments/Elemental/Ember")]
public class EmberAugment : ElementalAugment
{
    [Header("Ember Buff")] 
    [SerializeField] private int buffDuration;
    [SerializeField] private int outgoingPhysicalDamageIncrease;
    [SerializeField] private int incomingDamageIncrease;

    protected override void CollectStatusEffect(CastContext context)
    {
        // TODO: Add status effects.
    }

    protected override void OnBoon(CastContext context)
    {
        context.StatusEffects.Add(new EmberBuff(context.Target, buffDuration, outgoingPhysicalDamageIncrease, incomingDamageIncrease));
    }
}

public class EmberBuff : BasicBuff
{
    public int OutgoingPhysicalDamageIncrease { get; }
    public int IncomingDamageIncrease { get; }

    public EmberBuff(Combatant target, int duration, int outgoingPhysicalDamageIncrease, int incomingDamageIncrease) :
        base(target, duration)
    {
        OutgoingPhysicalDamageIncrease = outgoingPhysicalDamageIncrease;
        IncomingDamageIncrease = incomingDamageIncrease;
    }

    public override void OnApply()
    {
        Target.Stats.IncomingDamagePercentage.AssignModifier(new StatModifier(this, StatModifierType.Flat,
            IncomingDamageIncrease));
    }

    public override void OnRemove()
    {
        Target.Stats.IncomingDamagePercentage.RemoveModifiersFromSource(this);
    }

    public override void OnModify(CastContext context)
    {
        foreach (Damage damage in context.Damage.Where(damage => damage.Type == DamageType.Physical))
        {
            damage.Amount.AssignModifier(new StatModifier(this, StatModifierType.Percentage,
                OutgoingPhysicalDamageIncrease));
        }
    }
}