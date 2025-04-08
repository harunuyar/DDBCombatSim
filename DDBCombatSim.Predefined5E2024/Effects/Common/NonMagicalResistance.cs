namespace DDBCombatSim.Predefined5E2024.Effects.Common;

using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Utils;

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
