namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Restrained : BaseEffect
{
    public Restrained() : base("Restrained")
    {
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return (actionEvent is AttackRollEvent attackRollEvent && (attackRollEvent.Attacker == effectInstance.Owner || attackRollEvent.Target == effectInstance.Owner)) ||
               (actionEvent is AttackSavingThrowEvent savingThrowEvent && savingThrowEvent.Target == effectInstance.Owner && savingThrowEvent.Context.Ability == EAbility.Dexterity);
    }

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner;
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

        if (actionEvent is AttackSavingThrowEvent savingThrowEvent)
        {
            savingThrowEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
        }

        return Task.CompletedTask;
    }

    public override Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is StartTurnEvent startTurnEvent)
        {
            effectInstance.Owner.Speed.Value = 0;
        }

        return Task.CompletedTask;
    }
}
