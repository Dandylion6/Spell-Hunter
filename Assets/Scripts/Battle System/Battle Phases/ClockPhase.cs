using System.Data;
using System.Linq;

public class ClockPhase : BattlePhase
{
    private readonly StopWatch phaseDuration = new(2.0f);

    public ClockPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        Combatant nextActiveCombatant = Battle.Combatants
            .OrderBy(combatant => combatant.Clock.TicksTillReady(combatant.Stats.Speed.Value))
            .ThenByDescending(combatant => combatant.Stats.Speed.Value)
            .FirstOrDefault();

        if (nextActiveCombatant == null) throw new NoNullAllowedException();

        int ticks = nextActiveCombatant.Clock.TicksTillReady(nextActiveCombatant.Stats.Speed.Value);
        foreach (Combatant combatant in Battle.Combatants)
            combatant.Clock.AddProgress(ticks, combatant.Stats.Speed.Value);
        
        Context.BeginTurn(nextActiveCombatant, ticks);
        phaseDuration.Start();
    }

    public override void Stop()
    {
        phaseDuration.Stop();
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (!phaseDuration.IsCompleted) return false;

        // Ready for combatant to act.
        nextPhase = Context.ActionPhase;
        return true;
    }

    public override void Tick()
    {
        phaseDuration.Tick();
    }
}