using UnityEngine;

public class Ability
{
    public enum CastType
    {
        Shot,
        Hex,
        Boon,
    }
    
    public string Name { get; private set; } = "Ability";
    public int ManaCost { get; private set; }
    public CastType Cast { get; private set; }
}
