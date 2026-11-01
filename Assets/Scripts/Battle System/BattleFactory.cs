using System.Collections.Generic;
using UnityEngine;

public class EncounterDefinition
{
    public IReadOnlyList<CharacterDefinition> PossibleEnemies { get; }
    public int MinEnemies { get; }
    public int MaxEnemies { get; }

    public EncounterDefinition(IReadOnlyList<CharacterDefinition> possibleEnemies, int minEnemies, int maxEnemies)
    {
        if (possibleEnemies.Count == 0)
            throw new System.ArgumentException("Battle must have at least one possible enemy!");
        
        if (minEnemies < 1)
            throw new System.ArgumentOutOfRangeException(nameof(minEnemies), "Must at least be 1!");
        
        if (minEnemies > maxEnemies)
            throw new System.ArgumentOutOfRangeException(nameof(minEnemies), "Is less than max enemies!");
        
        PossibleEnemies = possibleEnemies;
        MinEnemies = minEnemies;
        MaxEnemies = maxEnemies;
    }
}

public class BattleFactory
{
    private readonly CharacterStats player;
    private readonly IActionProvider playerActions;

    public BattleFactory(CharacterStats player, IActionProvider playerActions)
    {
        this.player = player;
        this.playerActions = playerActions;
    }
    
    public Battle Create(EncounterDefinition encounter)
    {
        List<Combatant> combatants = new();
        int enemyCount = Random.Range(encounter.MinEnemies, encounter.MaxEnemies + 1);

        Combatant playerCombatant = new(player, playerActions, CombatantType.Ally, combatants.Count);
        combatants.Add(playerCombatant);

        List<CharacterDefinition> picks = new(enemyCount);
        Dictionary<CharacterDefinition, int> totals = new();

        for (int i = 0; i < enemyCount; ++i)
        {
            CharacterDefinition definition = encounter.PossibleEnemies[Random.Range(0, encounter.PossibleEnemies.Count)];
            picks.Add(definition);
            totals[definition] = totals.GetValueOrDefault(definition) + 1;
        }
        
        Dictionary<CharacterDefinition, int> seen = new();

        foreach (CharacterDefinition definition in picks)
        {
            int index = seen.GetValueOrDefault(definition);
            seen[definition] = index + 1;
            
            string displayName = definition.DisplayName;
            bool hasSimilarCombatant = totals[definition] > 1;
            if (hasSimilarCombatant)
            {
                displayName += $" {(char)('A' + index)}";
            }

            // TODO: Make a proper AI brain.
            TestBrain testBrain = new();
            
            CharacterStats newCharacter = new(definition, displayName);
            combatants.Add(new Combatant(newCharacter, testBrain, CombatantType.Opponent, combatants.Count));
        }
        
        return new Battle(combatants);
    }
}
