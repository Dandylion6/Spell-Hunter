using System.Threading.Tasks;
using UnityEngine;

public class TestBrain : IActionProvider
{
    public Task<BattleAction> RequestAction(BattleData battle)
    {
        BattleAction testAction = new("Attack", battle.ActiveCombatant);

        Task<BattleAction> action = Task.FromResult(testAction);
        return action;
    }
}
