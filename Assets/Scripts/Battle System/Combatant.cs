using UnityEngine;

public class Combatant
{
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get;  private set; }

    public Combatant(CharacterStats stats, IActionProvider brain)
    {
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
    }
}
