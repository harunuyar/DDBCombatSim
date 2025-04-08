namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Stunned : Incapacitated
{
    public Stunned() : base("Stunned")
    {
    }

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePostEvent(effectInstance, actionEvent);
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner ||
               actionEvent is AttackSavingThrowEvent savingThrowEvent && savingThrowEvent.Target == effectInstance.Owner;
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner)
        {
            attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Advantage));
        }

        if (actionEvent is AttackSavingThrowEvent savingThrowEvent && 
            savingThrowEvent.Target == effectInstance.Owner &&
            (savingThrowEvent.Context.Ability == EAbility.Strength || savingThrowEvent.Context.Ability == EAbility.Dexterity))
        {
            savingThrowEvent.OverridingResult = new Modifier<ETestResult>(this, Name, ETestResult.Failure);
        }

        return Task.CompletedTask;
    }

    public override Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        return base.ExecutePostEventAsync(effectInstance, actionEvent, cancellationToken);
    }
}
