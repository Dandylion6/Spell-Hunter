using UnityEngine;

[CreateAssetMenu(fileName = "Frost Augment", menuName = "Objects/Augments/Elemental/Frost")]
public class FrostAugment : ElementalAugment
{
    [SerializeField] private ChilledBaseStats chilled;
    [SerializeField] private FrozenBaseStats frozen;
    
    protected override void CollectStatusEffect(CastContext context)
    {
        context.StatusEffects.Add(new Chilled(context.Target, chilled, frozen));
    }

    protected override void OnBoon(CastContext context)
    {
        // TODO: add Frostguard
    }
}
