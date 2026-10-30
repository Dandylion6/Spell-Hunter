using System.Threading.Tasks;

/// <summary>
///     Handles logic to return a <see cref="BattleAction"/>.
/// </summary>
public interface IActionProvider
{
    Task<BattleAction> RequestAction(Battle battle, Combatant combatant);
}