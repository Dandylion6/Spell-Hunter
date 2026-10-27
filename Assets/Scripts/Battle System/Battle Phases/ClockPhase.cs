using System.Data;
using System.Linq;
using UnityEngine;

public class ClockPhase : BattlePhase
{
    public const float PhaseDuration = 2.0f;
    private float secondsWaiting;
    
    public ClockPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        Combatant nextActiveCombatant = BattleData.Combatants
            .OrderBy(combatant => combatant.Clock.TicksTillReady(combatant.Stats.Speed))
            .FirstOrDefault();

        if (nextActiveCombatant == null) throw new NoNullAllowedException();
        
        int ticks = nextActiveCombatant.Clock.TicksTillReady(nextActiveCombatant.Stats.Speed);
        foreach (Combatant combatant in BattleData.Combatants)
        {
            combatant.Clock.AddProgress(ticks, combatant.Stats.Speed);
        }
        
        nextActiveCombatant.Clock.LoopClock();
        BattleData.SetActiveCombatant(nextActiveCombatant);
        secondsWaiting = 0.0f;
    }

    public override void Stop()
    {
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (secondsWaiting < PhaseDuration)  return false;

        // Ready for combatant to act.
        nextPhase = Context.ActionPhase;
        return true;
    }

    public override void Tick() => secondsWaiting += Time.deltaTime;
}