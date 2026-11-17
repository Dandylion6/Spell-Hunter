using UnityEngine;

[CreateAssetMenu(fileName = "New Burn", menuName = "Objects/Status Effects/Burn")]
public class BurnDefinition : StatusDefinition<Burn>
{
    [Header("Burn")] 
    [SerializeField] [Min(0)] private int maxPhysicalResistMelt = 30;
    [SerializeField] [Min(1)] private int maxStacks = 2;
    [SerializeField] [Min(1)] private int stacksOnProc = 1;

    [Header("Per Stack")] 
    [SerializeField] [Min(0)] private int damage = 3;
    [SerializeField] [Min(0)] private int physicalResistMelt = 6;
    [SerializeField] [Min(1)] private int stackDuration = 3;

    public override Burn CreateEffect(Combatant target)
    {
        return new Burn(target, displayName, damage, physicalResistMelt, maxPhysicalResistMelt, stackDuration, maxStacks,
            stacksOnProc);
    }
}