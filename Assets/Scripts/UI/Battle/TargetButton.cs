using UnityEngine;

public class TargetButton : ActionSessionView
{
    [SerializeField] private BattleContext context;
    [SerializeField] private PlayerActionController controller;

    private Combatant target;
    
    public void OnClick()
    {
        ActiveSession?.SelectTarget(target);
    }
    
    protected override void Refresh()
    {
    }

    protected override void OnSessionAttached(ActionSelectionSession newSession)
    {
    }

    protected override void OnSessionDetached(ActionSelectionSession oldSession)
    {
    }
    
    private void Start()
    {
        target = context.Battle.Combatants[1];
        Bind(controller);
    }
}
