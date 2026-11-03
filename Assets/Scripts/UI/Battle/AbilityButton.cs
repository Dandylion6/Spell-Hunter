using UnityEngine;

public class AbilityButton : ActionSessionView
{
    [SerializeField] private BattleLauncher launcher;

    private Ability ability;

    private void Start()
    {
        ability = new HexAbility("File Ball", null, new HexBaseStats());
        Bind(launcher.Player.Controller as PlayerActionController);
    }

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
}