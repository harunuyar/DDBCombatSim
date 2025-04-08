namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class EndTurnEvent : IActionEvent
{
    public EndTurnEvent(ICombatant actor)
    {
        Actor = actor;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "End Turn";

    public ICombatant Actor { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        Actor.Actions.Value = 0;
        Actor.BonusActions.Value = 0;
        Actor.MagicActions.Value = 0;
        Actor.Speed.Value = 0;

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
