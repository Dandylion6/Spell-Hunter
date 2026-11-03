using System.Collections.Generic;
using System.Linq;

public class HexAbility : Ability
{
    public Stat OnHitProcDamagePercentageIncrease { get; }
    
    public HexAbility(string displayName, Combatant user, HexBaseStats baseStats) : base(displayName, user)
    {
        ManaCost = new Stat(baseStats.ManaCost);
        OnHitProcDamagePercentageIncrease = new Stat(baseStats.OnHitProcDamagePercentageIncrease);
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType != User.CombatantType).ToList();
    }

    public override void Execute(Battle battle, List<BattleEvent> events, Combatant target)
    {
    }
}
