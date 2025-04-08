namespace DDBCombatSim.Predefined.Effects.Common;

using DDBCombatSim.Action.Events;
using DDBCombatSim.Action;
using DDBCombatSim.Effect;
using DDBCombatSim.Utils;

public class NonMagicalResistance : Resistance
{
    public NonMagicalResistance(EDamageType resistedTypes) : base(resistedTypes)
    {
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is ApplyDamageEvent damageEvent &&
               damageEvent.Context.MagicNature == EMagicNature.NonMagical;
    }
}
