using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoonAbility : Ability
{
    public BoonAbility(string displayName, Combatant user, BoonBaseStats baseStats) : base(displayName, user)
    {
        ManaCost = new Stat(baseStats.ManaCost);
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType == User.CombatantType).ToList();
    }

    public override void Execute(Battle battle, List<BattleEvent> events, Combatant target)
    {
        throw new System.NotImplementedException();
    }
}
