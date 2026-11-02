using UnityEngine;

public class Player
{
    public CharacterStats Stats { get; }
    public IActionProvider Controller { get; }

    public Player(CharacterStats stats, IActionProvider controller)
    {
        Stats = stats;
        Controller = controller;
    }
}
