using UnityEngine;

public class Combatant
{
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }

    public Combatant(CharacterStats stats)
    {
        Stats = stats;
        Clock = new TurnClock();
    }
}
