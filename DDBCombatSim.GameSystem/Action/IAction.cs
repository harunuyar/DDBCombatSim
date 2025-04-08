namespace DDBCombatSim.GameSystem.Action;

using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Utils;

public interface IAction : IDndObject
{
    CombatContext CombatContext { get; }
    ICombatant Actor { get; }
    Cost Cost { get; }
    ECancellation Cancellation { get; set; }
    bool IsMagicAction { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
