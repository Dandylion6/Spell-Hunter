using UnityEngine;

public class BattleContext : MonoBehaviour
{
    public bool IsInCombat => currentPhase != null;
    
    public ClockPhase ClockPhase { get; private set; }
    public ActionPhase ActionPhase { get; private set; }
    public ExecutionPhase ExecutionPhase { get; private set; }
    
    private BattlePhase currentPhase;

    public void BeginCombat()
    {
        if (IsInCombat) return;
        currentPhase = ClockPhase;
    }

    private void EndCombat() => currentPhase = null;

    private void Awake()
    {
        ClockPhase = new ClockPhase(this);
        ActionPhase = new ActionPhase(this);
        ExecutionPhase = new ExecutionPhase(this);
        
        // TODO: Create a system to generate encounters.
        BeginCombat();
    }

    private void Update()
    {
        if (!IsInCombat) return; // There is no combat currently.

        currentPhase.Tick();
        
        if (currentPhase.ShouldSwitchTo(out BattlePhase nextPhase))
        {
            SwitchTo(nextPhase);
        }
    }

    /// <summary>
    ///     Handles switching to a different <see cref="BattlePhase"/>.
    /// </summary>
    /// <param name="nextPhase">
    ///     is the <see cref="BattlePhase"/> to switch to.
    /// </param>
    private void SwitchTo(BattlePhase nextPhase)
    {
        // This keeps the change clear.
        BattlePhase oldPhase = currentPhase;
        currentPhase = nextPhase;
        
        oldPhase.Stop();
        nextPhase.Start();
    }
}