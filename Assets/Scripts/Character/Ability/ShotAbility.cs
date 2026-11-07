using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotAbility : Ability
{
    public Stat PhysicalDamage { get; }
    public Stat Accuracy { get; }
    public Stat StatusProcChance { get; }
    public Stat OnHitBleedChance { get; }
    
    public ShotAbility(string displayName, Combatant user, ShotBaseStats baseStats) : base(displayName, user, baseStats)
    {
        PhysicalDamage = new Stat(baseStats.PhysicalDamage);
        Accuracy = new Stat(baseStats.Accuracy);
        StatusProcChance = new Stat(baseStats.StatusProcChance);
        OnHitBleedChance =  new Stat(baseStats.OnHitBleedChance);
        BaseTags = AugmentTags.Shot | AugmentTags.Offensive | AugmentTags.Damage | AugmentTags.Effect;
    }

    public override List<Combatant> GetValidTargets(Battle battle)
    {
        return battle.Combatants.Where(combatant => combatant.CombatantType != User.CombatantType && !combatant.IsDead).ToList();
    }

    protected override void BuildScaffold(CastContext context)
    {
        context.Damage.Add(new Damage(DamageType.Physical, PhysicalDamage.Value, DamageTags.OnHit));
    }

    protected override bool CanHit(CastContext context) => Random.Range(0, 100) < Accuracy.Value;

    protected override void ApplyCastRules(CastContext context)
    {
        bool proc = Random.Range(0, 100) < OnHitBleedChance.Value;
        if (!proc) return;
        // TODO: Add bleed.
    }
}
