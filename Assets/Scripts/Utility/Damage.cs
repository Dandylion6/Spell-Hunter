public enum DamageType
{
    Physical,
    Fire,
    Cold,
    Shock,
    Toxin
}

[System.Flags]
public enum DamageTags
{
    None = 0,
    OnHit = 1,
    Elemental = 2,
    Status = 4,
}

public class Damage
{
    public DamageType Type { get; }
    public DamageTags Tags { get; }
    public Stat Amount { get; }
    public bool IgnoresResistance { get; }

    public Damage(DamageType type, int amount, DamageTags tags, bool ignoresResistance = false)
    {
        Type = type;
        Tags = tags;
        Amount = new Stat(amount);
        IgnoresResistance = ignoresResistance;
    }
}
