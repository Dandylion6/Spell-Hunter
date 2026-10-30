using System.Threading.Tasks;
using UnityEngine;

public class TestBrain : IActionProvider
{
    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        BattleAction testAction = new("Attack", combatant);

        Task<BattleAction> action = Task.FromResult(testAction);
        return action;
    }
}
