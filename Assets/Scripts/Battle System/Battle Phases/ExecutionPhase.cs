using UnityEngine;

public class ExecutionPhase : BattlePhase
{
    private readonly StopWatch phaseDuration = new(1.0f); // A little delay in the phase.

    public ExecutionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        Debug.Log("[ACTION]\n" + CurrentTurn.Actor.Stats.DisplayName + "\nUsed Action: " + CurrentTurn.Action.Name + "!");
        phaseDuration.Start(); // Shouls start running after the action is done.
    }

    public override void Stop()
    {
        phaseDuration.Stop();
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (!phaseDuration.IsCompleted) return false;

        // Start the cycle again.
        nextPhase = Context.ClockPhase;
        return true;
    }

    public override void Tick()
    {
        phaseDuration.Tick();
    }
}