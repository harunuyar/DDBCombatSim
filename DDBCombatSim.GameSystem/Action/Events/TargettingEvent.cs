namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class TargettingEvent : IActionEvent
{
    public TargettingEvent(ICombatant actor, ICombatant target, TargettingContext ctx)
    {
        Actor = actor;
        Target = target;
        Context = ctx;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Targetting";

    public TargettingContext Context { get; set; }

    public ICombatant Actor { get; }

    public ICombatant Target { get; }

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
