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
        Context.Presenter.Play(Battle, events);
    }

    public override void Stop() => Context.CurrentTurn.Actor.Clock.LoopClock();

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