using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private CharacterDefinition debugPlayerDefinition;
    [SerializeField] private PlayerActionController playerActions;
    [SerializeField] private BattleContext context;
    
    private CharacterStats debugPlayer;
    private BattleFactory factory;

    private void Awake()
    {
        debugPlayer = new CharacterStats(debugPlayerDefinition);
        factory = new BattleFactory(debugPlayer, playerActions);
    }

    public void StartBattle(EncounterDefinition encounter)
    {
        Battle newBattle = factory.Create(encounter);

        string battleLog = "[BATTLE STARTED]\n";

        foreach (Combatant combatant in newBattle.Combatants)
        {
            battleLog += $"{combatant.ID}. {combatant.Stats.DisplayName} Joined!\n";
        }
        
        Debug.Log(battleLog);
        
        context.BeginCombat(newBattle);
    }
}
