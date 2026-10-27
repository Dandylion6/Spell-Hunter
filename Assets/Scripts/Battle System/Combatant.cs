using UnityEngine;

public class Combatant
{
    public int ID { get; private set; }
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get;  private set; }

    public Combatant(CharacterStats stats, IActionProvider brain, int id = -1)
    {
        ID = id;
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
    }
}
