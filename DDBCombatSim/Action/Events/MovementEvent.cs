namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Battlefield;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class MovementEvent : IActionEvent
{
    public MovementEvent(CombatContext combatContext, ICombatant actor, Position newPosition)
    {
        CombatContext = combatContext;
        Actor = actor;
        NewPosition = newPosition;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Movement";

    public CombatContext CombatContext { get; }

    public ICombatant Actor { get; }

    public Position NewPosition { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Actor.Position.DistanceTo(NewPosition) > 5)
        {
            throw new InvalidOperationException("Cannot move more than 5 feet in one movement event.");
        }

        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        CombatContext.Battlefield.SetObjectPosition(Actor, NewPosition);
        IsCompleted = true;
        return Task.CompletedTask;
    }
}
