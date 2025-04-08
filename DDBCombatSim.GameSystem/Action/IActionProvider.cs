namespace DDBCombatSim.GameSystem.Action;

using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;

public interface IActionProvider
{
    IEnumerable<IAction> GetActions(CombatContext combatContext, ICombatant owner);
}
