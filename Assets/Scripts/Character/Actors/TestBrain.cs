using System.Linq;
using System.Threading.Tasks;

public class TestBrain : IActionProvider
{
    public Task<BattleAction> RequestAction(Battle battle, Combatant combatant)
    {
        Ability ability = combatant.Abilities.FirstOrDefault();
        if (ability == null)
            throw new System.NullReferenceException("Ability not found!");
        
        BattleAction testAction = new AbilityAction(combatant, ability, ability.GetValidTargets(battle).FirstOrDefault());
        Task<BattleAction> action = Task.FromResult(testAction);
        return action;
    }
}