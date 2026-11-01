using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private BattleStarter battleStarter;
    [SerializeField] private List<CharacterDefinition> possibleEnemies = new();

    private void Start()
    {
        EncounterDefinition definition = new(possibleEnemies, 1, Battle.MaxEnemies);
        battleStarter.StartBattle(definition);
    }
}
