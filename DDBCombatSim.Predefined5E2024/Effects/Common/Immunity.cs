namespace DDBCombatSim.Predefined5E2024.Effects.Common;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
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
