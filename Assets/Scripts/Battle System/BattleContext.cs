using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleContext : MonoBehaviour
{
    [SerializeField] private List<CharacterDefinition> testCharacters = new();
    
    public Battle Battle { get; private set; }
    public Turn CurrentTurn { get; private set; }
    public bool IsInCombat => Battle != null;
    
    public ClockPhase ClockPhase { get; private set; }
    public ActionPhase ActionPhase { get; private set; }
    public ExecutionPhase ExecutionPhase { get; private set; }
    
    private BattlePhase currentPhase;

    public void BeginCombat(Battle battle)
    {
        if (IsInCombat) return;
        Battle = battle;
        currentPhase = ClockPhase;
        currentPhase.Start();
    }

    public void BeginTurn(Combatant combatant)
    {
        int nextIndex = CurrentTurn == null ? 0 : CurrentTurn.Index + 1;
        CurrentTurn = new Turn(combatant, nextIndex);
    }

    private void EndCombat()
    {
        currentPhase.Stop();
        currentPhase = null;
        Battle = null;
    }

    private void Awake()
    {
        ClockPhase = new ClockPhase(this);
        ActionPhase = new ActionPhase(this);
        ExecutionPhase = new ExecutionPhase(this);
        
        // TODO: Create a system to generate encounters.

        TestBrain testBrain = new();
        
        List<CharacterStats> characters = testCharacters.Select(character => new CharacterStats(character)).ToList();
        List<Combatant> combatants = characters.Select((characterStats, i) => new Combatant(characterStats, testBrain, Combatant.Type.Opponent, i)).ToList();
        var data =  new Battle(combatants);
        BeginCombat(data);
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