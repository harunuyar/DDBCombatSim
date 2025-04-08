namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Unconscious : Incapacitated
{
    private readonly Prone prone = new();

    public Unconscious() : base("Unconscious")
    {
    }

    public override int Priority => 20;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePreEvent(effectInstance, actionEvent) ||
               prone.IsApplicablePreEvent(effectInstance, actionEvent) ||
               actionEvent is AttackSavingThrowEvent savingThrowEvent &&
                   savingThrowEvent.Target == effectInstance.Owner &&
                   (savingThrowEvent.Context.Ability == EAbility.Strength || savingThrowEvent.Context.Ability == EAbility.Dexterity);
    }

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePostEvent(effectInstance, actionEvent) ||
               prone.IsApplicablePostEvent(effectInstance, actionEvent) ||
               actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner;
    }

    public override async Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackSavingThrowEvent savingThrowEvent &&
            savingThrowEvent.Target == effectInstance.Owner &&
            (savingThrowEvent.Context.Ability == EAbility.Strength || savingThrowEvent.Context.Ability == EAbility.Dexterity))
        {
            savingThrowEvent.OverridingResult = new Modifier<ETestResult>(this, Name, ETestResult.Failure);
        }

        await base.ExecutePreEventAsync(effectInstance, actionEvent, cancellationToken);
        await prone.ExecutePreEventAsync(effectInstance, actionEvent, cancellationToken);
    }

    public override async Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent &&
            attackRollEvent.Target == effectInstance.Owner &&
            attackRollEvent.Result.HasValue &&
            attackRollEvent.Result.Value.IsSuccess() &&
            attackRollEvent.Context.Range == ERange.Melee &&
            attackRollEvent.Attacker.IsWithinRange(effectInstance.Owner, 5))
        {
            attackRollEvent.OverridingResult = new Modifier<ETestResult>(this, Name, ETestResult.CriticalSuccess);
        }

        await base.ExecutePostEventAsync(effectInstance, actionEvent, cancellationToken);
        await prone.ExecutePostEventAsync(effectInstance, actionEvent, cancellationToken);
    }
}
