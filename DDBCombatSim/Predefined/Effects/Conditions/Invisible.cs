namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Invisible : BaseEffect
{
    public Invisible() : base("Invisible")
    {
    }

    public override int Priority => 5;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            if (attackRollEvent.Attacker == effectInstance.Owner || attackRollEvent.Target == effectInstance.Owner)
            {
                return true;
            }
        }

        return false;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            if (attackRollEvent.Attacker == effectInstance.Owner)
            {
                attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Advantage));
            }

            if (attackRollEvent.Target == effectInstance.Owner)
            {
                attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
            }
        }

        return Task.CompletedTask;
    }
}
