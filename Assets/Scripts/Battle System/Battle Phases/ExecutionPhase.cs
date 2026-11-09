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
        
        BattleResult result = Context.Battle.EvaluateBattleResult();
        if (result.Outcome != BattleOutcome.NotFinished)
        {
            events.Add(new BattleEndedEvent(result));
            Context.EndCombatLoop(result);
        }
        
        Context.Presenter?.Play(Battle, events);
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
        if (Context.Presenter is { IsPlaying: true }) return false;

        // Start the cycle again.
        nextPhase = Context.ClockPhase;
        return true;
    }

    public override void Tick()
    {
    }
}