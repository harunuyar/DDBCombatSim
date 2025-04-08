namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using System.Threading;
using System.Threading.Tasks;

public class Incapacitated : BaseEffect
{
    public Incapacitated(string? name = null) : base(name ?? "Incapacitated")
    {
    }

    public override int Priority => 10;

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner;
    }

    public override Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner)
        {
            effectInstance.Owner.Speed.Value = 0;
            effectInstance.Owner.Actions.Value = 0;
            effectInstance.Owner.BonusActions.Value = 0;
            effectInstance.Owner.Reactions.Value = 0;
        }

        return Task.CompletedTask;
    }
}
