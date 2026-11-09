using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;

    private Combatant target;
    private ActionSelectionSession session;

    public void OnClick()
    {
        if (target == null) return;
        session?.SelectTarget(target);
    }

    public void Initialize(Combatant newTarget, ActionSelectionSession newSession)
    {
        target = newTarget;
        session = newSession;
        
        text.text = newTarget.DisplayName;
    }
}