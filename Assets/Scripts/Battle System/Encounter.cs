using System.Collections.Generic;

public class Encounter
{
    public IReadOnlyList<CharacterDefinition> Enemies { get; }

    public Encounter(IReadOnlyList<CharacterDefinition> enemies)
    {
        Enemies = enemies;
    }
}
