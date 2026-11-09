using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        
        int manaRecovered = CurrentTurn.TicksSinceLastTurn * 2;
        int oldMana = actor.Stats.CurrentMana;
        
        int newMana = actor.Stats.RegenerateMana(manaRecovered);
        if (newMana != oldMana)
        {
            events.Add(new ManaEvent(manaRecovered, oldMana, newMana, actor));
        }

        foreach (StatusEffect effect in actor.StatusEffects) effect.OnBeforeAction(Battle, events);
        
        events.Add(new PreActionEndEvent());
        Context.Presenter?.Play(Battle, events);
        
        action = CurrentTurn.Actor.Brain.RequestAction(Battle, actor);
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