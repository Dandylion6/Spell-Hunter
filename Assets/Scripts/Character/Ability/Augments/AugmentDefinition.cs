using System.Collections.Generic;
using UnityEngine;

public abstract class AugmentDefinition : ScriptableObject
{
    [SerializeField] private string displayName = "New Augment";
    [SerializeField] private int manaCost = 4;
    [SerializeField] private AugmentTags required = AugmentTags.Shot;
    [SerializeField] private AugmentTags additions = AugmentTags.Effect;
    
    public string DisplayName => displayName;
    public int ManaCost => manaCost;
    public AugmentTags Required => required;
    public AugmentTags Additions => additions;

    public virtual bool IsCompatibleWith(Ability ability) => true;

    public virtual void OnApply(Augment augment)
    {
        Ability ability = augment.ImbuedAbility;
        if (ability == null)
            throw new System.NullReferenceException("Ability is null");
            
        ability.ManaCost.AssignModifier(new StatModifier(augment, StatModifierType.Flat, manaCost));
    }

    public virtual void OnRemove(Augment augment)
    {
        Ability ability = augment.ImbuedAbility;
        if (ability == null)
            throw new System.NullReferenceException("Ability is null");
        
        ability.ManaCost.RemoveModifiersFromSource(augment);
    }
    
    public virtual void OnBattleStart(Augment augment, Battle battle) { }
    public virtual void OnBattleEnd(Augment augment, Battle battle) { }
    public virtual void OnCast(CastContext context) { }
    public virtual void OnModify(CastContext context) { }
    public virtual void OnHit(CastContext context) { }
    // TODO: Add OnStatusEffect
}
