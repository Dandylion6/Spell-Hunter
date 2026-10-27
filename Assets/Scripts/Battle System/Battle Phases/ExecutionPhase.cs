using UnityEngine;

public class ExecutionPhase : BattlePhase
{
    private readonly StopWatch phaseDuration = new(1.0f);
    
    public ExecutionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        Debug.Log("Combatant: " + BattleData.ActiveCombatant.ID + "\nUsed Action: " + BattleData.Action.Name + "!");
        phaseDuration.Start();
    }

    public override void Stop()
    {
        BattleData.ClearAction();
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

    public override void Tick() => phaseDuration.Tick();
}