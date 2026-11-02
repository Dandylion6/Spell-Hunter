using System.Collections.Generic;

/// <summary>
///     An action used in combat. Contains the information required to execute a specific action.
/// </summary>
public abstract class BattleAction
{
    public string Name { get; private set; }
    public Combatant Actor { get; private set; }

    public BattleAction(string name, Combatant actor)
    {
        Name = name;
        Actor = actor;
    }

    public abstract List<BattleEvent> Execute();
}
