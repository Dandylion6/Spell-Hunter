using System.Collections.Generic;

public enum CombatantType
{
    Ally,
    Opponent
}

public class Combatant
{
    public string DisplayName { get; private set; }
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get; private set; }
    public CombatantType CombatantType { get; private set; }
    public bool IsDead { get; private set; }

    public IReadOnlyList<Ability> Abilities { get; }

    public Combatant(CharacterStats stats, IActionProvider brain, CombatantType combatantType,
        IReadOnlyList<Ability> abilities)
    {
        DisplayName = stats.BaseName;
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
        CombatantType = combatantType;
        Abilities = abilities;
    }
    
    public void SetName(string displayName) => DisplayName = displayName;

    public void MarkAsDead() => IsDead = true;
    public void Revive() => IsDead = false;
}