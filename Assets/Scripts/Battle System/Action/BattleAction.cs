using System.Collections.Generic;

/// <summary>
///     An action used in combat. Contains the information required to execute a specific action.
/// </summary>
public abstract class BattleAction
{
    public string Name { get; private set; }
    public Combatant Actor { get; private set; }
    public int TurnCost { get; protected set; } = TurnClock.ClockThreshold;

    public BattleAction(string name, Combatant actor)
    {
        Name = name;
        Actor = actor;
    }

    public List<BattleEvent> Execute(Battle battle)
    {
        List<BattleEvent> events = new();
        if (battle == null)
        {
            events.Add(new InvalidActionEvent());
            return events;
        }
        
        Execute(battle, events);
        
        foreach (StatusEffect effect in Actor.StatusEffects)
        {
            if (effect.IsExpired)
            {
                Actor.StatusEffects.Remove(effect);
                events.Add(new StatusEffectRemovedEvent(effect, Actor));
                break;
            }
            effect.OnAfterAction(battle, events);
        }
        
        events.Add(new ActionEndEvent());
        return events;
    }

    protected abstract void Execute(Battle battle, List<BattleEvent> events);
}
