using System;
using System.Collections.Generic;

[Flags]
public enum AugmentTags
{
    Shot = 1,
    Offensive = 2,
    Self = 4,
    Heal = 8,
    Damage = 16,
    Effect = 32,
    DoT = 64,
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
    public void OnCast(CastContext context) => Definition.OnCast(context);
    public void OnModify(CastContext context) => Definition.OnModify(context);
    public void OnHit(CastContext context) => Definition.OnHit(context);
    
    public void AttachToAbility(Ability ability)
    {
        if (ImbuedAbility != null) return;
        ImbuedAbility = ability;
        Definition.OnApply(this);
    }

    public void DetachFromAbility()
    {
        Definition.OnRemove(this);
        ImbuedAbility = null;
    }
}
