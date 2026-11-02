using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerActionController : MonoBehaviour, IActionProvider
{
    private TaskCompletionSource<BattleAction> pendingActionRequest;
    public ActionSelectionSession ActiveSession { get; private set; }

    private void OnDestroy()
    {
        pendingActionRequest?.TrySetCanceled();
    }

    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        if (ActiveSession != null)
            throw new InvalidOperationException("Cannot create a new session while one is already active!");

        pendingActionRequest = new TaskCompletionSource<BattleAction>();
        ActiveSession = new ActionSelectionSession(battle, combatant);

        ActiveSession.ActionChosen += OnActionChosen;
        SessionStarted?.Invoke(ActiveSession);

        return pendingActionRequest.Task;
    }

    public event Action<ActionSelectionSession> SessionStarted;
    public event Action<ActionSelectionSession> SessionEnded;

    private void OnActionChosen(BattleAction battleAction)
    {
        ActiveSession.ActionChosen -= OnActionChosen;
        pendingActionRequest?.TrySetResult(battleAction);

        SessionEnded?.Invoke(ActiveSession);
        ActiveSession = null;
    }
}