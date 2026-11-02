using System.Linq;
using UnityEngine;

public class TargetButton : ActionSessionView
{
    [SerializeField] private BattleContext context;
    [SerializeField] private PlayerActionController controller;

    private Combatant target;
    
    private void Start()
    {
        Bind(controller);
    }

    public void OnClick()
    {
        ActiveSession?.SelectTarget(target);
    }

    protected override void Refresh()
    {
    }

    protected override void OnSessionAttached(ActionSelectionSession newSession)
    {
        target = context.Battle.GetEnemiesOfUser(newSession.User).FirstOrDefault();
    }

    protected override void OnSessionDetached(ActionSelectionSession oldSession)
    {
    }
}