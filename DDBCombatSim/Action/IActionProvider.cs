namespace DDBCombatSim.Action;

using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;

public interface IActionProvider
{
    IEnumerable<IAction> GetActions(CombatContext combatContext, ICombatant owner);
}
