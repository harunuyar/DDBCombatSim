namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class ApplyHealingEvent : IActionEvent
{
    public ApplyHealingEvent(ICombatant healer, ICombatant target, int amount)
    {
        Healer = healer;
        Target = target;
        Amount = new IntStat("Base", amount);
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Apply Healing";

    public ICombatant Healer { get; }

    public ICombatant Target { get; }

    public IntStat Amount { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        Target.HitPoints.Restore(Amount.Value);

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
