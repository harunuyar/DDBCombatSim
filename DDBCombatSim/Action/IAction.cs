namespace DDBCombatSim.Action;

using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Utils;

public interface IAction : IDndObject
{
    CombatContext CombatContext { get; }
    ICombatant Actor { get; }
    Cost Cost { get; }
    ECancellation Cancellation { get; set; }
    bool IsMagicAction { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
