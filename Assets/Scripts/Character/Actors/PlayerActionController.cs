using System.Threading.Tasks;
using UnityEngine;

public class PlayerActionController : MonoBehaviour, IActionProvider
{
    private TaskCompletionSource<BattleAction> pendingActionRequest;
    
    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        pendingActionRequest = new TaskCompletionSource<BattleAction>();
        
        ActionSelectionSession session = new(battle, combatant);
        session.SubscribeToOnCompletion(SubmitAction);
        
        // TODO: Provide the ability to choose actions here.
        
        return pendingActionRequest.Task;
    }
    
    public void SubmitAction(BattleAction battleAction) => pendingActionRequest?.TrySetResult(battleAction);
}
