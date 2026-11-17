using UnityEngine;

[CreateAssetMenu(fileName = "Frost Augment", menuName = "Objects/Augments/Elemental/Frost")]
public class FrostAugment : ElementalAugment
{
    [SerializeField] private ChilledDefinition chilled;
    
    protected override void CollectStatusEffect(CastContext context)
    {
        context.StatusEffects.Add(chilled.CreateEffect(context.Target));
    }

    protected override void OnBoon(CastContext context)
    {
        // TODO: add Frostguard
    }
}
