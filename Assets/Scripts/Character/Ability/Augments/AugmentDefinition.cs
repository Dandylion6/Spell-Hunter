using UnityEngine;

public abstract class AugmentDefinition : ScriptableObject
{
    [SerializeField] private string displayName = "New Augment";
    [SerializeField] private int manaCost = 4;
    [SerializeField] private AugmentTags tags = AugmentTags.NegativeEffect;
    
    public string DisplayName => displayName;
    public int ManaCost => manaCost;
    public AugmentTags Tags => tags;

    public virtual bool IsCompatibleWith(Ability ability) => true;
    
    public virtual void OnBattleStart(Augment augment, Battle battle) { }
    public virtual void OnBattleEnd(Augment augment, Battle battle) { }
    public virtual void OnCast(Augment augment, Battle battle, Combatant target) { }
    public virtual void OnHit(Augment augment, Battle battle, Combatant target) { }
    // TODO: Add OnStatusEffect
}
