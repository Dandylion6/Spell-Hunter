using System.Collections.Generic;
using System.Threading.Tasks;

public class ActionPhase : BattlePhase
{
    private Task<BattleAction> action;

    public ActionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        List<BattleEvent> events = new();
        Combatant actor = CurrentTurn.Actor;

        int manaRecovered = CurrentTurn.TicksSinceLastTurn; // TODO: Improve mana recovery.
        int oldMana = actor.Stats.CurrentMana;

        int newMana = actor.Stats.RegenerateMana(manaRecovered);
        if (newMana != oldMana)
        {
            events.Add(new ManaEvent(manaRecovered, oldMana, newMana, actor));
        }

        foreach (StatusEffect effect in actor.StatusEffects) effect.OnBeforeAction(Battle, CurrentTurn, events);

        events.Add(new PreActionEndEvent());
        Context.Presenter?.Play(Battle, events);

        action = CurrentTurn.Action != null
            ? Task.FromResult(CurrentTurn.Action) // Will create a finished task from the existing action.
            : CurrentTurn.Actor.Brain.RequestAction(Battle, actor);
    }

    public override void Stop()
    {
        CurrentTurn.SetAction(action.Result);
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (!action.IsCompletedSuccessfully) return false;

        // Ready to execute the action.
        nextPhase = Context.ExecutionPhase;
        return true;
    }

    public override void Tick()
    {
    }
}