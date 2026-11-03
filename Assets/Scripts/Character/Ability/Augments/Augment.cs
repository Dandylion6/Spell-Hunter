using System;

[Flags]
public enum AugmentTags
{
    NegativeEffect = 1,
    PositiveEffect = 2,
    Healing = 4,
}

public class Augment
{
    public Ability ImbuedAbility { get; private set; }
    public AugmentDefinition Definition { get; private set; }

    public Augment(AugmentDefinition definition, Ability imbuedAbility = null)
    {
        ImbuedAbility = imbuedAbility;
        Definition = definition;
    }
    
    public bool IsCompatibleWith(Ability ability) => Definition.IsCompatibleWith(ability);
    public void OnBattleStart(Battle battle) => Definition.OnBattleStart(this, battle);
    public void OnBattleEnd(Battle battle) => Definition.OnBattleEnd(this, battle);
    public void OnCast(Battle battle, Combatant target) => Definition.OnCast(this, battle, target);
    public void OnHit(Battle battle, Combatant target) => Definition.OnHit(this, battle, target);
    
    public void AttachToAbility(Ability ability)
    {
        if (ImbuedAbility != null) return;
        ImbuedAbility = ability;
    }

    public void DetachFromAbility() => ImbuedAbility = null;
}
