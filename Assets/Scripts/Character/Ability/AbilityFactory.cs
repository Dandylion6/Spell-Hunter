using UnityEngine;

public class AbilityFactory
{
    public AbilityBaseStats BaseStats { get; }

    public AbilityFactory(AbilityBaseStats baseStats) => BaseStats = baseStats;

    public Ability CreateAbility(AbilityBlueprint blueprint, Combatant user)
    {
        Ability ability = blueprint.AbilityType switch
        {
            AbilityType.Shot => new ShotAbility(blueprint.DisplayName, user, BaseStats.Shot),
            AbilityType.Hex => new HexAbility(blueprint.DisplayName, user, BaseStats.Hex),
            AbilityType.Boon => new BoonAbility(blueprint.DisplayName, user, BaseStats.Boon),
            _ => throw new System.ArgumentOutOfRangeException(nameof(blueprint.AbilityType))
        };

        foreach (AugmentDefinition definition in blueprint.Augments)
        {
            ability.ApplyAugment(new Augment(definition, ability));
        }
        
        return ability;
    }
}
