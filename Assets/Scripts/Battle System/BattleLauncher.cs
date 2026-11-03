using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    [SerializeField] private AbilityBaseStats abilityBaseStats;
    [SerializeField] private BattleContext context;
    [SerializeField] private CharacterDefinition playerDefinition;
    [SerializeField] private PlayerActionController controller;

    public Player Player { get; private set; }
    private AbilityFactory abilityFactory;
    
    public void Initialize(AbilityFactory factory) => abilityFactory = factory; 

    public void RequestBattle(Encounter encounter)
    {
        // TEMP!
        if (Player == null) GenerateDummyPlayer();
        if (abilityFactory == null) Initialize(new AbilityFactory(abilityBaseStats));

        List<Combatant> combatants = new()
        {
            new Combatant(Player?.Stats, Player?.Controller, CombatantType.Ally, new List<AbilityBlueprint>(), abilityFactory),
        };

        // PS: I hate the LINQ-expression for this code; it's unreadable.
        foreach (CharacterDefinition definition in encounter.Enemies)
        {
            CharacterStats character = new(definition);
            List<AbilityBlueprint> abilityBlueprints = definition.Abilities.Select(preset => preset.ToBlueprint()).ToList();
            combatants.Add(new Combatant(character, definition.GetBrain(), CombatantType.Opponent, abilityBlueprints, abilityFactory));
        }

        Battle newBattle = new(combatants);
        context.BeginCombatLoop(newBattle);
    }

    private void GenerateDummyPlayer()
    {
        // TODO: Replace dummy player.
        CharacterStats character = new(playerDefinition);
        Player = new Player(character, controller);
    }
}