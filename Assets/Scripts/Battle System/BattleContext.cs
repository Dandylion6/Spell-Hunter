using System;
using UnityEngine;

public class BattleContext : MonoBehaviour
{
    public Battle Battle { get; private set; }
    public Turn CurrentTurn { get; private set; }
    public IBattlePresenter Presenter { get; private set; } 
    public bool IsInCombat => Battle != null;

    public ClockPhase ClockPhase { get; private set; }
    public ActionPhase ActionPhase { get; private set; }
    public ExecutionPhase ExecutionPhase { get; private set; }

    public event Action<Battle> LoopStarted;
    public event Action<Turn> TurnChanged;
    public event Action<BattleResult> LoopEnded;
    
    private BattlePhase currentPhase;
    
    public void SetPresenter(IBattlePresenter presenter) => Presenter = presenter;
    
    public void BeginCombatLoop(Battle battle)
    {
        if (IsInCombat)
            throw new InvalidOperationException("A Battle is already in session!");

        Battle = battle;
        currentPhase = ClockPhase;
        currentPhase.Start();

        LoopStarted?.Invoke(battle);
    }
    
    public void EndCombatLoop(BattleResult result)
    {
        if (!IsInCombat) return;

        currentPhase.Stop();
        currentPhase = null;
        
        CurrentTurn = null;
        Battle = null;
        
        LoopEnded?.Invoke(result);
    }

    public void BeginTurn(Combatant combatant, int ticks)
    {
        int nextIndex = CurrentTurn == null ? 0 : CurrentTurn.Index + 1;
        CurrentTurn = new Turn(combatant, nextIndex, ticks);
        TurnChanged?.Invoke(CurrentTurn);
    }
    
    private void Awake()
    {
        ClockPhase = new ClockPhase(this);
        ActionPhase = new ActionPhase(this);
        ExecutionPhase = new ExecutionPhase(this);
    }

    private void Update()
    {
        if (!IsInCombat) return; // There is no combat currently.

        currentPhase.Tick();

        if (currentPhase.ShouldSwitchTo(out BattlePhase nextPhase)) SwitchTo(nextPhase);
    }

    /// <summary>
    ///     Handles switching to a different <see cref="BattlePhase" />.
    /// </summary>
    /// <param name="nextPhase">
    ///     is the <see cref="BattlePhase" /> to switch to.
    /// </param>
    private void SwitchTo(BattlePhase nextPhase)
    {
        if (!IsInCombat) return; // Can't allow switching if there is no battle.
        
        // This keeps the change clear.
        BattlePhase oldPhase = currentPhase;
        currentPhase = nextPhase;

        oldPhase.Stop();
        nextPhase.Start();
    }
}