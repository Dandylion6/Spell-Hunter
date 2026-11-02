public enum CombatantType
{
    Ally,
    Opponent
}

public class Combatant
{
    public Combatant(CharacterStats stats, IActionProvider brain, CombatantType combatantType)
    {
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
        CombatantType = combatantType;
    }

    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get; private set; }
    public CombatantType CombatantType { get; private set; }
    public bool IsDead { get; private set; }

    public void MarkAsDead() => IsDead = true;
    public void Revive() => IsDead = false;
}