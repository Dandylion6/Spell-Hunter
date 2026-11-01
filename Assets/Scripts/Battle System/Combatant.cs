public enum CombatantType
{
    Ally,
    Opponent,
}

public class Combatant
{
    public int ID { get; private set; }
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get;  private set; }
    public CombatantType CombatantType { get; private set; } 

    public Combatant(CharacterStats stats, IActionProvider brain, CombatantType combatantType, int id)
    {
        ID = id;
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
        CombatantType = combatantType;
    }
}
