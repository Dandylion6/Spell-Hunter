using UnityEngine;

public class ExecutionPhase : BattlePhase
{
    public const float PhaseDuration = 1.0f;
    private float durationSeconds;
    
    public ExecutionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        Debug.Log("Combatant: " + BattleData.ActiveCombatant.ID + "\nUsed Action: " + BattleData.Action.Name + "!");
    }

    public override void Stop()
    {
        BattleData.ClearAction();
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (durationSeconds < PhaseDuration) return false;

        // Start the cycle again.
        nextPhase = Context.ClockPhase;
        return true;
    }

    public override void Tick() => durationSeconds += Time.deltaTime;
}