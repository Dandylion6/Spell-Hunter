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
        Debug.Log("Starting ActionPhase");
        action = CurrentTurn.Actor.Brain.RequestAction(Battle, CurrentTurn.Actor);
    }

    public override void Stop()
    {
        CurrentTurn.SetAction(action.Result);
    }

    public override bool ShouldSwitchTo(out BattlePhase nextPhase)
    {
        nextPhase = null;
        if (!action.IsCompletedSuccessfully) return false;
        
        // Ready to execute the action.
        nextPhase = Context.ExecutionPhase;                
        return true;
    }

    public override void Tick()
    {
    }
}