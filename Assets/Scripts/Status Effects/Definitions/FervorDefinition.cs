using UnityEngine;

[CreateAssetMenu(fileName = "New Fervor", menuName = "Objects/Status Effects/Fervor")]
public class FervorDefinition : SimpleStatusDefinition<Fervor>
{
    [Header("Fervor")] 
    [SerializeField] [Min(0)] private int outgoingPhysicalDamagePercent;
    [SerializeField] [Min(0)] private int incomingDamagePercent;

    public override Fervor CreateEffect(Combatant target)
    {
        return new Fervor(target, displayName, duration, outgoingPhysicalDamagePercent, incomingDamagePercent);
    }
}