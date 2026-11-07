using System.Collections.Generic;

public class BoonAbility : Ability
{
    public BoonAbility(string displayName, Combatant user, BoonBaseStats baseStats) : base(displayName, user, baseStats)
    {
        BaseTags = AugmentTags.Self;
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return new List<Combatant> { User };
    }
}
