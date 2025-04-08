namespace DDBCombatSim.Predefined.Effects.Common;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Resistance : BaseEffect
{
    private readonly EDamageType resistedTypes;

    public Resistance(EDamageType resistedTypes) : base("Resistance")
    {
        this.resistedTypes = resistedTypes;
    }

    public override int Priority => 5;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is ApplyDamageEvent;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is ApplyDamageEvent damageEvent)
        {
            damageEvent.Resistances.Modifiers.Add(new Modifier<EDamageType>(this, Name, resistedTypes));
        }

        return Task.CompletedTask;
    }
}
