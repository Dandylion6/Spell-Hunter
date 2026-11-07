using System.Collections.Generic;

public class AbilityAction : BattleAction
{
    /// <summary>
    ///     In the case an action is not an ability cast, this will be null.
    /// </summary>
    public Ability Ability { get; }
    public Combatant Target { get; }
    
    public AbilityAction(Combatant actor, Ability ability, Combatant target) : base(ability.DisplayName, actor)
    {
        Ability = ability;
        Target = target;
    }

    protected override void Execute(Battle battle, List<BattleEvent> events)
    {
        Ability.Execute(battle, events, Target);
        TurnCost = Ability.TurnCost.Value;
    }
}
