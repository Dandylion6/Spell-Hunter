using System.Collections.Generic;

public class CastContext
{
    public Ability Ability { get; }
    public Battle Battle { get; }
    public Combatant Target { get; }
    public List<Damage> Damage { get; }
    public List<StatusEffect> StatusEffects { get; }
    public List<Healing> Healing { get; }

    private readonly List<Stat> touchedStats = new();

    public CastContext(Ability ability, Battle battle, Combatant target)
    {
        Ability = ability;
        Battle = battle;
        Target = target;
        Damage = new List<Damage>();
        StatusEffects = new List<StatusEffect>();
        Healing = new List<Healing>();
    }

    public void AddTemporaryModifier(Stat stat, StatModifierType type, float value)
    {
        stat.AssignModifier(new StatModifier(this, type, value));
        if (!touchedStats.Contains(stat)) touchedStats.Add(stat);
    }

    public void Cleanup()
    {
        foreach (Stat stat in touchedStats) stat.RemoveModifiersFromSource(this);
        touchedStats.Clear();
    }
}
