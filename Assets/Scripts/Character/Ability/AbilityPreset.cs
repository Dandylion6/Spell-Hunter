using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Shot,
    Hex,
    Boon,
}

[CreateAssetMenu(fileName = "New Preset Ability", menuName = "Objects/Preset Ability")]
public class AbilityPreset : ScriptableObject
{
    [SerializeField] private string displayName = "New Ability";
    [SerializeField] private AbilityType abilityType = AbilityType.Shot;
    [SerializeField] private List<AugmentDefinition> augments = new();
    
    public string DisplayName => displayName;
    public AbilityType AbilityType => abilityType;
    public IReadOnlyList<AugmentDefinition> Augments => augments;

    public AbilityBlueprint ToBlueprint() => new AbilityBlueprint(displayName, abilityType, augments);
}