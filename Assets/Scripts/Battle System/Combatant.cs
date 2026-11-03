using System.Collections.Generic;

public enum CombatantType
{
    Ally,
    Opponent
}

public class Combatant
{
    public string DisplayName { get; private set; }
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get; private set; }
    public CombatantType CombatantType { get; private set; }
    public bool IsDead { get; private set; }

    public IReadOnlyList<Ability> Abilities { get; }

    public Combatant(CharacterStats stats, IActionProvider brain, CombatantType combatantType,
        IReadOnlyList<AbilityBlueprint> abilityBlueprints, AbilityFactory abilityFactory)
    {
        DisplayName = stats.BaseName;
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
        CombatantType = combatantType;
        Abilities = BuildAbilities(abilityBlueprints, abilityFactory);
    }

    public void SetName(string displayName) => DisplayName = displayName;

    public void MarkAsDead() => IsDead = true;
    public void Revive() => IsDead = false;

    private List<Ability> BuildAbilities(IReadOnlyList<AbilityBlueprint> abilityBlueprints,
        AbilityFactory abilityFactory)
    {
        List<Ability> abilities = new(abilityBlueprints.Count);
        foreach (AbilityBlueprint abilityBlueprint in abilityBlueprints)
        {
            abilityFactory.CreateAbility(abilityBlueprint, this);
        }

        return abilities;
    }
}