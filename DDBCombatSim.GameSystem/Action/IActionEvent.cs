namespace DDBCombatSim.GameSystem.Action;

using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;

public interface IActionEvent : IDndObject
{
    bool IsCompleted { get; }
    EnumStat<ECancellation> Cancellation { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
