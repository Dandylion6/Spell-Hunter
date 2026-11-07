using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Objects/Status Effect")]
public abstract class StatusEffectDefinition : ScriptableObject
{
    [Header("Status")]
    [SerializeField] private string displayName = "New Status";
    
    public string DisplayName => displayName;

    public abstract StatusEffect OnProc(Augment augment, List<BattleEvent> events, Battle battle, Combatant target);
}