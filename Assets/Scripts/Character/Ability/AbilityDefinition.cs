using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Objects/Ability")]
public class AbilityDefinition : ScriptableObject
{
    [SerializeField] private string displayName = "New Ability";
    [SerializeField] private AbilityType type = AbilityType.Shot;
    [SerializeField] private List<AugmentDefinition> augments = new();
    
    public string DisplayName => displayName;
    public AbilityType Type => type;
    public IReadOnlyList<AugmentDefinition> Augments => augments;
}
