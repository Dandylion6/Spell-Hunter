using System.Linq;
using System.Threading.Tasks;

public class TestBrain : IActionProvider
{
    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        Ability testAbility = new("Fire Ball", AbilityType.Shot);
        BattleAction testAction = new AbilityAction(combatant, testAbility, battle.GetValidTargetsForAbility(combatant, testAbility).FirstOrDefault());

        Task<BattleAction> action = Task.FromResult(testAction);
        return action;
    }
}