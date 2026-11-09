using System.Collections.Generic;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    [SerializeField] private AbilityBaseStatsConfig abilityBaseStatsConfig;
    [SerializeField] private BattleContext context;
    [SerializeField] private CharacterDefinition playerDefinition;
    [SerializeField] private PlayerActionController controller;

    public Player Player { get; private set; }
    private AbilityFactory abilityFactory;
    
    public void Initialize(AbilityFactory factory) => abilityFactory = factory; 

    public void RequestBattle(Encounter encounter)
    {
        List<Combatant> combatants = new()
        {
            new Combatant(Player?.Stats, Player?.Controller, CombatantType.Ally, playerDefinition.ToAbilityBlueprints(),
                abilityFactory),
        };

        // PS: I hate the LINQ-expression for this code; it's unreadable.
        foreach (CharacterDefinition definition in encounter.Enemies)
        {
            CharacterStats character = new(definition);
            List<AbilityBlueprint> blueprints = definition.ToAbilityBlueprints();
            Combatant combatant = new(character, definition.GetBrain(), CombatantType.Opponent, blueprints, abilityFactory);
            combatants.Add(combatant);
        }

        Battle newBattle = new(combatants);
        context.BeginCombatLoop(newBattle);
    }

    private void Awake()
    {
        if (Player == null) GenerateDummyPlayer();
        if (abilityFactory == null) Initialize(new AbilityFactory(abilityBaseStatsConfig));
    }

    private void GenerateDummyPlayer()
    {
        // TODO: Replace dummy player.
        CharacterStats character = new(playerDefinition);
        Player = new Player(character, controller);
    }
}