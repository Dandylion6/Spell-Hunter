using UnityEngine;

/// <summary>
///     Base class for views that display <see cref="ActionSelectionSession"/>. Handles the subscription and provides
///     methods used for managing the view such as <see cref="Refresh"/>. 
/// </summary>
/// <remarks>
///     Derived classes must <see cref="Bind"/> to a <see cref="PlayerActionController"/> and handle the build up and
///     tear down of the view. 
/// </remarks>
public abstract class ActionSessionView : MonoBehaviour
{
    protected PlayerActionController Controller { get; private set; }
    protected ActionSelectionSession ActiveSession { get; private set; }

    /// <summary>
    ///     Binds this view to <paramref name="controller"/> and releases the previous bind (if there is one) first.
    ///     If the controller already has a session running, the view attaches to it immediately.
    /// </summary>
    /// <param name="controller">
    ///     is the controller whose sessions this view should display.
    /// </param>
    protected void Bind(PlayerActionController controller)
    {
        Unbind();
        Controller = controller;
        
        Controller.SessionStarted += HandleSessionStarted;
        Controller.SessionEnded += HandleSessionEnded;

        bool alreadyInSession = controller.ActiveSession != null;
        if (!alreadyInSession) return;
        
        HandleSessionStarted(controller.ActiveSession);
    }

    /// <summary>
    ///     Releases the controller from the view and detaches the session if one is active.
    /// </summary>
    protected void Unbind()
    {
        if (Controller == null) return;
        
        Controller.SessionStarted -= HandleSessionStarted;
        Controller.SessionEnded -= HandleSessionEnded;
        
        Controller = null;
        DetachSession();
    }

    protected void OnDestroy() => Unbind();

    /// <summary>
    ///     Updates the view whenever the session data changes.
    /// </summary>
    protected abstract void Refresh();
    protected abstract void OnSessionAttached(ActionSelectionSession newSession);
    protected abstract void OnSessionDetached(ActionSelectionSession oldSession);
    
    /// <summary>
    ///     Stops listening to the current session and clears the <see cref="ActiveSession"/>.
    /// </summary>
    private void DetachSession()
    {
        if (ActiveSession == null) return;
        
        ActionSelectionSession oldSession = ActiveSession;
        
        ActiveSession.Changed -= Refresh;
        ActiveSession = null;
        
        OnSessionDetached(oldSession);
    }
    
    protected virtual void HandleSessionStarted(ActionSelectionSession newSession)
    {
        if (ActiveSession != null) DetachSession();
        
        ActiveSession = newSession;
        ActiveSession.Changed += Refresh;
        
        OnSessionAttached(newSession);
        Refresh();
    }

    protected virtual void HandleSessionEnded(ActionSelectionSession oldSession)
    {
        if (oldSession == ActiveSession) DetachSession();
    }
}
