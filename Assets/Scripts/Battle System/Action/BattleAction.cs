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

    public IReadOnlyList<Combatant> Targets => targets;
    public bool UseTurn { get; private set; }
    
    private readonly List<Combatant> targets = null;

    public BattleAction(string name, Combatant actor, bool useTurn = true)
    {
        Name = name;
        Actor = actor;
        UseTurn = useTurn;
    }

    public BattleAction(Combatant actor, Ability ability, List<Combatant> targets, bool useTurn = true)
    {
        Name = ability.Name;
        Actor = actor;
        Ability = ability;
        UseTurn = useTurn;
        this.targets = targets;
    }
}
