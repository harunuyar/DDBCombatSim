namespace DDBCombatSim.Predefined.Effects.Common;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Immunity : BaseEffect
{
    private readonly EDamageType damageTypes;

    public Immunity(EDamageType damageTypes) : base("Immunity")
    {
        this.damageTypes = damageTypes;
    }

    public override int Priority => 6;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is ApplyDamageEvent;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is ApplyDamageEvent damageEvent)
        {
            damageEvent.Immunities.Modifiers.Add(new Modifier<EDamageType>(this, Name, damageTypes));
        }

        return Task.CompletedTask;
    }
}
