using System.Collections.Generic;

public class AbilityAction : BattleAction
{
    /// <summary>
    ///     In the case an action is not an ability cast, this will be null.
    /// </summary>
    public Ability Ability { get; }
    public Combatant Target { get; private set; }
    
    public AbilityAction(Combatant actor, Ability ability, Combatant target) : base(ability.DisplayName, actor)
    {
        Ability = ability;
        Target = target;
    }

    protected override void Execute(Battle battle, List<BattleEvent> events)
    {
        switch (Ability.Type)
        {
            case AbilityType.Shot: ExecuteShot(battle, events); break;
            case AbilityType.Hex: ExecuteHex(battle, events); break;
            case AbilityType.Boon: ExecuteBoon(battle, events); break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    private void ExecuteShot(Battle battle, List<BattleEvent> events)
    {
        
    }

    private void ExecuteHex(Battle battle, List<BattleEvent> events)
    {
        throw new System.NotImplementedException();
    }

    private void ExecuteBoon(Battle battle, List<BattleEvent> events)
    {
        throw new System.NotImplementedException();
    }
}
