namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Combatant;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class DeathEvent : IActionEvent
{
    public DeathEvent(ICombatant combatant)
    {
        Combatant = combatant;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Death";

    public ICombatant Combatant { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        Combatant.DeathStatus.IsDying = true;

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
