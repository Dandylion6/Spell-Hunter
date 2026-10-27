using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ClockPhase : BattlePhase
{
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
    }

    public override void Stop()
    {
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        return false;
    }

    public override void Tick()
    {
    }
}