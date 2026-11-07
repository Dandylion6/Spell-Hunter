using System.Collections.Generic;
using UnityEngine;

public class ExecutionPhase : BattlePhase
{
    public ExecutionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        BattleAction action = Context.CurrentTurn.Action;
        List<BattleEvent> events = action.Execute(Battle);

        foreach (BattleEvent battleEvent in events)
        {
            switch (battleEvent)
            {
                case DamageEvent damageEvent:
                    Debug.Log($"{action.Actor} has dealt {damageEvent.Damage} {damageEvent.Type} damage to {damageEvent.Target}: HP {damageEvent.HealthBeforeDamage} -> {damageEvent.HealthAfterDamage}");
                    break;
                case ActionEndEvent endEvent:
                    Debug.Log("Action has ended");
                    break;
            }
        }
        
        Context.Presenter.Play(Battle, events);
    }

    public override void Stop()
    {
        BattleAction action = Context.CurrentTurn.Action;
        int turnCost = Mathf.Max(55, action.TurnCost);
        Context.CurrentTurn.Actor.Clock.SpendTurn(turnCost);
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (Context.Presenter.IsPlaying) return false;

        // Start the cycle again.
        nextPhase = Context.ClockPhase;
        return true;
    }

    public override void Tick()
    {
    }
}