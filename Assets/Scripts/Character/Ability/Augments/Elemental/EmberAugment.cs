using UnityEngine;

[CreateAssetMenu(fileName = "Ember Augment", menuName = "Objects/Augments/Elemental/Ember")]
public class EmberAugment : ElementalAugment
{
    [SerializeField] private BurnBaseStats burn;
    [SerializeField] private FervorBuffBaseStats buff;

    protected override void CollectStatusEffect(CastContext context)
    {
        context.StatusEffects.Add(new Burn(context.Target, burn));
    }

    protected override void OnBoon(CastContext context)
    {
        context.StatusEffects.Add(new FervorBuff(context.Target, buff));
    }
}