using UnityEngine;

public abstract class ElementalAugment : AugmentDefinition
{
    [SerializeField] private int onHitDamage;
    [SerializeField] private DamageType damageType;

    public override void OnCast(CastContext context)
    {
        if (context.Ability is BoonAbility)
        {
            OnBoon(context);
            return;
        }
        
        context.Damage.Add(new Damage(damageType, onHitDamage, DamageTags.OnHit | DamageTags.Elemental));
        CollectStatusEffect(context);
    }
    
    protected abstract void CollectStatusEffect(CastContext context);

    protected abstract void OnBoon(CastContext context);
}
