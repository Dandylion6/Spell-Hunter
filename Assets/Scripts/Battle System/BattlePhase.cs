public abstract class BattlePhase
{
    protected BattleContext Context { get; private set; }

    protected BattlePhase(BattleContext context) => Context = context;
    
    /// <summary>
    /// Called once the phase becomes active. 
    /// </summary>
    public abstract void Start();
    
    /// <summary>
    /// Called once after the phase has ended.
    /// </summary>
    public abstract void Stop();
    
    /// <summary>
    /// Checks for phase switch after <see cref="Tick"/> has been called.
    /// </summary>
    /// <param name="nextPhase">is the <see cref="BattlePhase"/> to switch to.</param>
    /// <returns><c>True</c> if a switch should occur; <c>False</c> if logic should continue.</returns>
    public abstract bool ShouldSwitchTo(out BattlePhase nextPhase);
    
    /// <summary>
    /// Called at every frame while this is the current phase.
    /// </summary>
    public abstract void Tick();
}