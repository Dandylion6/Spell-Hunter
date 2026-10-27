public class ExecutionPhase : BattlePhase
{
    public ExecutionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
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