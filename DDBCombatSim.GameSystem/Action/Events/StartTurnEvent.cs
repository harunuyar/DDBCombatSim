namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class StartTurnEvent : IActionEvent
{
    public StartTurnEvent(ICombatant actor)
    {
        Actor = actor;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Take Turn";

    public ICombatant Actor { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        Actor.Actions.Reset();
        Actor.BonusActions.Reset();
        Actor.Reactions.Reset();
        Actor.Speed.Reset();
        Actor.MagicActions.Reset();

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
