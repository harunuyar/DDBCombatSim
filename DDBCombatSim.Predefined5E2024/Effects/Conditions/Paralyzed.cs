namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Paralyzed : Incapacitated
{
    public Paralyzed() : base("Paralyzed")
    {
    }

    public override int Priority => 10; // high to override other effects

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePostEvent(effectInstance, actionEvent) || 
            actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner;
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePreEvent(effectInstance, actionEvent) ||
            actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner ||
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

        return base.ExecutePreEventAsync(effectInstance, actionEvent, cancellationToken);
    }

    public override Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner)
        {
            if (attackRollEvent.Result.HasValue && attackRollEvent.Result.Value.IsSuccess() && attackRollEvent.Context.Range == ERange.Melee && effectInstance.Source.IsWithinRange(effectInstance.Owner, 5))
            {
                attackRollEvent.OverridingResult = new Modifier<ETestResult>(this, Name, ETestResult.CriticalSuccess);
            }
        }

        return base.ExecutePostEventAsync(effectInstance, actionEvent, cancellationToken);
    }
}
