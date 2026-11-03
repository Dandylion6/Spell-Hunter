using System.Collections.Generic;
using System.Linq;

public class ShotAbility : Ability
{
    public Stat PhysicalDamage { get; }
    public Stat OnHitProcChance { get; }
    
    public ShotAbility(string displayName, Combatant user, ShotBaseStats baseStats) : base(displayName, user)
    {
        ManaCost = new Stat(baseStats.ManaCost);
        PhysicalDamage = new Stat(baseStats.PhysicalDamage);
        OnHitProcChance = new Stat(baseStats.OnHitProcChance);
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType == User.CombatantType).ToList();
    }

    public override void Execute(Battle battle, List<BattleEvent> events, Combatant target)
    {
        
    }
}
