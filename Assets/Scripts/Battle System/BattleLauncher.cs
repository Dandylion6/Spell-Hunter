using System.Collections.Generic;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    [SerializeField] private BattleContext context;
    [SerializeField] private CharacterDefinition playerDefinition;
    [SerializeField] private PlayerActionController controller;

    private Player player;
    
    public void RequestBattle(Encounter encounter)
    {
        if (player == null) GenerateDummyPlayer();
        
        List<Combatant> combatants = new()
        {
            new Combatant(player.Stats, player.Controller, CombatantType.Ally)
        };
        
        // PS: I hate the LINQ-expression for this code; it's unreadable.
        foreach (CharacterDefinition definition in encounter.Enemies)
        {
            CharacterStats character = new(definition);
            combatants.Add(new Combatant(character, definition.GetBrain(), CombatantType.Opponent));
        }

        Battle newBattle = new(combatants);
        context.BeginCombatLoop(newBattle);
    }

    private void GenerateDummyPlayer()
    {
        // TODO: Replace dummy player.
        CharacterStats character = new(playerDefinition);
        player = new Player(character, controller);
    }
}