namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Blinded : BaseEffect
{
    public Blinded() : base("Blinded")
    {
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is AttackRollEvent attackRollEvent && (attackRollEvent.Attacker == effectInstance.Owner || attackRollEvent.Target == effectInstance.Owner);
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            if (attackRollEvent.Attacker == effectInstance.Owner)
            {
                attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
            }

            if (attackRollEvent.Target == effectInstance.Owner)
            {
                attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Advantage));
            }
        }

        return Task.CompletedTask;
    }
}
