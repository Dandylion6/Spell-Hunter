using System.Collections.Generic;
using System.Linq;

public class HexAbility : Ability
{
    public Stat ElementalDamageIncrease { get; }
    
    public HexAbility(string displayName, Combatant user, HexBaseStats baseStats) : base(displayName, user, baseStats)
    {
        ElementalDamageIncrease = new Stat(baseStats.ElementalDamageIncrease);
        BaseTags = AugmentTags.Offensive;
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType != User.CombatantType).ToList();
    }

    protected override void ApplyCastRules(CastContext context)
    {
        StatModifier damageAmp = new(this, StatModifierType.Percentage, ElementalDamageIncrease.Value);
        foreach (Damage damage in context.Damage.Where(damage => damage.Type != DamageType.Physical))
        {
            damage.Amount.AssignModifier(damageAmp);
        }
    }
}
