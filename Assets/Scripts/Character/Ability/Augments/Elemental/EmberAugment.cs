using UnityEngine;

[CreateAssetMenu(fileName = "Ember Augment", menuName = "Objects/Augments/Elemental/Ember")]
public class EmberAugment : ElementalAugment
{
    [SerializeField] private BurnDefinition burn;
    [SerializeField] private FervorDefinition fervor;

    protected override void CollectStatusEffect(CastContext context)
    {
        context.StatusEffects.Add(burn.CreateEffect(context.Target));
    }

    protected override void OnBoon(CastContext context)
    {
        context.StatusEffects.Add(fervor.CreateEffect(context.Target));
    }
}