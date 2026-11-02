using System.Collections.Generic;

public class AbilityAction : BattleAction
{
    /// <summary>
    ///     In the case an action is not an ability cast, this will be null.
    /// </summary>
    public Ability Ability { get; private set; }
    public Combatant Target { get; private set; }
    
    public AbilityAction(Combatant actor, Ability ability, Combatant target) : base(ability.Name, actor)
    {
        Ability = ability;
        Target = target;
    }

    public override List<BattleEvent> Execute()
    {
        return new List<BattleEvent>();
    }
}
