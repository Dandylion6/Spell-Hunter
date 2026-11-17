using UnityEngine;

[CreateAssetMenu(fileName = "New Chilled", menuName = "Objects/Status Effects/Chilled")]
public class ChilledDefinition : SimpleStatusDefinition<Chilled>
{
    [Header("Chilled")] [SerializeField] private int speedReductionPercent = 20;
    [SerializeField] private int freezeBuildThreshold = 2;
    [SerializeField] private FrozenDefinition frozen;

    public override Chilled CreateEffect(Combatant target)
    {
        return new Chilled(target, displayName, duration, speedReductionPercent, freezeBuildThreshold, frozen);
    }
}