namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Battlefield;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Frightened : BaseEffect
{
    public Frightened() : base("Frightened")
    {
    }

    public override int Priority => 10; // high to override other effects

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            if (attackRollEvent.Attacker == effectInstance.Owner && CanSeeSource(effectInstance))
            {
                return true;
            }
        }

        if (actionEvent is MovementEvent movementEvent)
        {
            if (movementEvent.Actor == effectInstance.Owner && IsMovingCloser(movementEvent, effectInstance))
            {
                return true;
            }
        }

        return false;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent &&
            attackRollEvent.Attacker == effectInstance.Owner &&
            CanSeeSource(effectInstance))
        {
            attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
        }

        if (actionEvent is MovementEvent movementEvent &&
            movementEvent.Actor == effectInstance.Owner &&
            IsMovingCloser(movementEvent, effectInstance))
        {
            movementEvent.Cancellation.Modifiers.Add(
                new Modifier<ECancellation>(
                    this, 
                    "Frightened creature cannot move closer to the source of its fear", 
                    ECancellation.SystemCancelled));
        }

        return Task.CompletedTask;
    }

    private static bool CanSeeSource(EffectInstance effectInstance)
    {
        // TODO: Replace with actual vision/line-of-sight logic later
        return effectInstance.Source != null;
    }

    private static bool IsMovingCloser(MovementEvent movementEvent, EffectInstance effectInstance)
    {
        if (effectInstance.Source == null)
        {
            return false;
        }

        Position current = movementEvent.Actor.Position;
        Position next = movementEvent.NewPosition;
        Position source = effectInstance.Source.Position;

        int currentDistance = ManhattanDistance(current, source);
        int nextDistance = ManhattanDistance(next, source);

        return nextDistance < currentDistance;
    }

    private static int ManhattanDistance(Position a, Position b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
