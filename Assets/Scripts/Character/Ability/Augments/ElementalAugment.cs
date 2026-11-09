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

        if (context.Ability is ShotAbility shotAbility)
        {
            bool proc = Random.Range(0, 100) < shotAbility.StatusProcChance.Value;
            if (!proc) return;
        }
        
        CollectStatusEffect(context);
    }
    
    protected abstract void CollectStatusEffect(CastContext context);

    protected abstract void OnBoon(CastContext context);
}
