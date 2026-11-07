using UnityEngine;

public class AbilityFactory
{
    public AbilityBaseStatsConfig BaseStatsConfig { get; }

    public AbilityFactory(AbilityBaseStatsConfig baseStatsConfig) => BaseStatsConfig = baseStatsConfig;

    public Ability CreateAbility(AbilityBlueprint blueprint, Combatant user)
    {
        Ability ability = blueprint.AbilityType switch
        {
            AbilityType.Shot => new ShotAbility(blueprint.DisplayName, user, BaseStatsConfig.Shot),
            AbilityType.Hex => new HexAbility(blueprint.DisplayName, user, BaseStatsConfig.Hex),
            AbilityType.Boon => new BoonAbility(blueprint.DisplayName, user, BaseStatsConfig.Boon),
            _ => throw new System.ArgumentOutOfRangeException(nameof(blueprint.AbilityType))
        };

        foreach (AugmentDefinition definition in blueprint.Augments)
        {
            ability.ApplyAugment(new Augment(definition, ability));
        }
        
        return ability;
    }
}
