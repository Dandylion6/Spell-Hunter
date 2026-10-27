using System.Threading.Tasks;
using UnityEngine;

public class ActionPhase : BattlePhase
{
    private Task<BattleAction> action;
    
    public ActionPhase(BattleContext context) : base(context)
    {
    }

    public override void Start()
    {
        BattleData.ClearAction(); // Make sure action isn't dirty.
        action = BattleData.ActiveCombatant.Brain.RequestAction(BattleData);
    }

    public override void Stop()
    {
        BattleData.AssignAction(action.Result);
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (!action.IsCompleted) return false;
        
        // Ready to execute the action.
        nextPhase = Context.ExecutionPhase;                
        return true;
    }

    public override void Tick()
    {
    }
}