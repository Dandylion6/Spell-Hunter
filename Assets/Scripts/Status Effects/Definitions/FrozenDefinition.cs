using UnityEngine;

[CreateAssetMenu(fileName = "New Frozen", menuName = "Objects/Status Effects/Frozen")]
public class FrozenDefinition : SimpleStatusDefinition<Frozen>
{
    [Header("Frozen")] 
    [SerializeField] [Min(0)] private int shatterDamageThreshold = 12;
    [SerializeField] [Min(0)] private int thawDuration = 2;
    [SerializeField] private bool ignorePhysicalResistanceOnShatter = true;
    [SerializeField] private bool isImmuneToShock = true;

    public override Frozen CreateEffect(Combatant target)
    {
        return new Frozen(target, displayName, duration, shatterDamageThreshold, thawDuration,
            ignorePhysicalResistanceOnShatter, isImmuneToShock);
    }
}