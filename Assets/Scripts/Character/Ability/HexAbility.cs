using System.Collections.Generic;
using System.Linq;

public class HexAbility : Ability
{
    public Stat DurationPercentageIncrease { get; }
    
    public HexAbility(string displayName, Combatant user, HexBaseStats baseStats) : base(displayName, user, baseStats)
    {
        DurationPercentageIncrease = new Stat(baseStats.DurationPercentageIncrease);
        BaseTags = AugmentTags.Offensive;
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType != User.CombatantType).ToList();
    }
}
