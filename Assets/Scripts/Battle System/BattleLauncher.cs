using System.Collections.Generic;
using System.Linq;
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
            new Combatant(player.Stats, player.Controller, CombatantType.Ally, new List<Ability>()),
        };

        // PS: I hate the LINQ-expression for this code; it's unreadable.
        foreach (CharacterDefinition definition in encounter.Enemies)
        {
            List<Ability> abilities = BuildAbilities(definition.Abilities);
            CharacterStats character = new(definition);
            combatants.Add(new Combatant(character, definition.GetBrain(), CombatantType.Opponent, abilities));
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

    private static List<Ability> BuildAbilities(IReadOnlyList<AbilityDefinition> definitions)
    {
        List<Ability> abilities = new();
        if (definitions == null || definitions.Count == 0) return abilities;

        abilities.AddRange(definitions.Select(definition =>
            new Ability(definition.DisplayName, definition.Type, BuildAugments(definition.Augments))));

        return abilities;
    }

    private static List<Augment> BuildAugments(IReadOnlyList<AugmentDefinition> definitions)
    {
        List<Augment> augments = new();
        if (definitions == null || definitions.Count == 0) return augments;

        augments.AddRange(definitions.Select(definition => new Augment(definition)));
        return augments;
    }
}