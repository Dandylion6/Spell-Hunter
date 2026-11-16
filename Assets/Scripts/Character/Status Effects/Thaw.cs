using System.Collections.Generic;

public class Thaw : BasicStatus
{
    public Thaw(Combatant target, int turns) : base(target, "Thaw", turns)
    {
    }

    public override void OnApply(List<BattleEvent> events)
    {
        Target.Stats.FrozenImmunity.AssignModifier(new StatModifier(this, StatModifierType.Flat, 1.0f));
    }

    public override void OnRemove()
    {
        Target.Stats.FrozenImmunity.RemoveModifiersFromSource(this);
    }
}
