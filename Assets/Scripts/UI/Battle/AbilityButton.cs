using UnityEngine;

public class AbilityButton : ActionSessionView
{
    [SerializeField] private PlayerActionController controller;

    private Ability ability;
    
    public void OnClick()
    {
        ActiveSession?.SelectAbility(ability);
    }
    
    protected override void Refresh()
    {
    }

    protected override void OnSessionAttached(ActionSelectionSession newSession)
    {
        Debug.Log("Ready for action");
    }

    protected override void OnSessionDetached(ActionSelectionSession oldSession)
    {
    }

    private void Start()
    {
        ability = new Ability("Shoot", 10, Ability.CastType.Shot);
        Bind(controller);
    }
}
