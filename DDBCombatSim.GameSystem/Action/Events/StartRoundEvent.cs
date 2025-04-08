namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class StartRoundEvent : IActionEvent
{
    public StartRoundEvent(int round)
    {
        Round = round;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Round " + Round;

    public int Round { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
