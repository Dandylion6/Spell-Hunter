using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleContext : MonoBehaviour
{
    [SerializeField] private List<CharacterDefinition> testCharacters = new();
    
    public BattleData Data { get; private set; }
    public bool IsInCombat => currentPhase != null;
    
    public ClockPhase ClockPhase { get; private set; }
    public ActionPhase ActionPhase { get; private set; }
    public ExecutionPhase ExecutionPhase { get; private set; }
    
    private BattlePhase currentPhase;

    public void BeginCombat(BattleData data)
    {
        if (IsInCombat) return;
        Data = data;
        currentPhase = ClockPhase;
        currentPhase.Start();
    }

    private void EndCombat()
    {
        currentPhase = null;
        Data = null;
    }

    private void Awake()
    {
        ClockPhase = new ClockPhase(this);
        ActionPhase = new ActionPhase(this);
        ExecutionPhase = new ExecutionPhase(this);
        
        // TODO: Create a system to generate encounters.
        List<CharacterStats> characters = testCharacters.Select(character => new CharacterStats(character)).ToList();
        for (int i = 0; i < characters.Count; ++i)
        {
            CharacterStats character = characters[i];
            Debug.Log(i + ". " + character + " (" + character.ID + ")");
        }
        
        List<Combatant> combatants = characters.Select(testCharacter => new Combatant(testCharacter)).ToList();
        var data =  new BattleData(combatants);
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