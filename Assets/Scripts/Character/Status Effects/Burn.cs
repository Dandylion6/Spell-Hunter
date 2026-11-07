using UnityEngine;

[System.Serializable]
public struct BurnSettings
{
    
}

public class Burn : StatusEffect
{
    public class Stack
    {
        private int TurnsRemaining { get; set; }
    }
    
    public override bool IsExpired { get; }
    
    public Burn(Combatant target) : base(target)
    {
    }

    
}
