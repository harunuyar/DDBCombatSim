namespace DDBCombatSim.Action;

using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public interface IActionEvent : IDndObject
{
    bool IsCompleted { get; }
    EnumStat<ECancellation> Cancellation { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
