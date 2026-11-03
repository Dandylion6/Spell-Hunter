using System.Linq;
using System.Threading.Tasks;

public class TestBrain : IActionProvider
{
    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        ShotAbility testAbility = new("Fire Ball", combatant, new ShotBaseStats());
        BattleAction testAction = new AbilityAction(combatant, testAbility, testAbility.GetValidTargets(battle).FirstOrDefault());

        Task<BattleAction> action = Task.FromResult(testAction);
        return action;
    }
}