using System.Collections.Generic;

[System.Serializable]
public class AbilityBlueprint
{
    public string DisplayName { get; }
    public AbilityType AbilityType { get; }
    public IReadOnlyList<AugmentDefinition> Augments { get; }

    public AbilityBlueprint(string displayName, AbilityType abilityType, IReadOnlyList<AugmentDefinition> augments)
    {
        DisplayName = displayName;
        AbilityType = abilityType;
        Augments = new List<AugmentDefinition>(augments); // It should copy the list, not reference it.
    }
}
