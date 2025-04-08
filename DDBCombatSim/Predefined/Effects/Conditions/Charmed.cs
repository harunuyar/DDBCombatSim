namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Context;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Charmed : BaseEffect
{
    public Charmed() : base("Charmed")
    {
    }

    public override int Priority => 10;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        if (actionEvent is TargettingEvent targetingEvent)
        {
            if (targetingEvent.Actor == effectInstance.Owner &&
                targetingEvent.Context.Intent == ETargetIntent.Harmful &&
                targetingEvent.Target == effectInstance.Source)
            {
                return true;
            }
        }

        return false;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is TargettingEvent targetingEvent &&
            targetingEvent.Actor == effectInstance.Owner &&
            targetingEvent.Context.Intent == ETargetIntent.Harmful &&
            targetingEvent.Target == effectInstance.Source)
        {
            targetingEvent.Cancellation.Modifiers.Add(
                new Modifier<ECancellation>(
                    this, 
                    "Charmed creature cannot target the source of its charm with harmful effects", 
                    ECancellation.SystemCancelled));
        }

        return Task.CompletedTask;
    }
}
