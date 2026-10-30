using System.Collections.Generic;

/// <summary>
///     An action used in combat. Contains the information required to execute a specific action.
/// </summary>
public class BattleAction
{
    public string Name { get; private set; }
    public Combatant Actor { get; private set; }
    
    /// <summary>
    ///     In the case an action is not an ability cast, this will be null.
    /// </summary>
    public Ability Ability { get; private set; }

    public Combatant Target { get; private set; }
    public bool UseTurn { get; private set; }
    

    public BattleAction(string name, Combatant actor, bool useTurn = true)
    {
        Name = name;
        Actor = actor;
        UseTurn = useTurn;
    }

    public BattleAction(Combatant actor, Ability ability, Combatant target, bool useTurn = true)
    {
        Name = ability.Name;
        Actor = actor;
        Ability = ability;
        UseTurn = useTurn;
        Target = target;
    }
}
