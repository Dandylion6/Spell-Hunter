using System.Collections.Generic;
using UnityEngine;

public class RandomEncounterTrigger : MonoBehaviour
{
    [SerializeField] private BattleLauncher launcher;
    [SerializeField] private List<CharacterDefinition> enemies;
    
    private CharacterStats player;
    
    private void Start()
    {
        launcher.RequestBattle(GenerateRandomEncounter());
    }

    private Encounter GenerateRandomEncounter()
    {
        int enemyCount = Random.Range(1, Battle.MaxOpponents + 1);
        List<CharacterDefinition> picks = new(enemyCount);

        for (int i = 0; i < enemyCount; ++i)
        {
            int randomEnemy = Random.Range(0, enemies.Count);
            picks.Add(enemies[randomEnemy]);
        }

        return new Encounter(picks);
    } 
}
