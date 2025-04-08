namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Effect;
using DDBCombatSim.Predefined.Effects.Common;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Petrified : Incapacitated
{
    private static readonly Resistance resistanceEffect = new(EDamageTypeExtensions.All);

    public Petrified() : base("Petrified")
    {
    }

    public override int Priority => 100;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePreEvent(effectInstance, actionEvent) ||
               resistanceEffect.IsApplicablePreEvent(effectInstance, actionEvent) ||
               actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target == effectInstance.Owner ||
               actionEvent is AttackSavingThrowEvent savingThrowEvent &&
                   savingThrowEvent.Target == effectInstance.Owner &&
                   (savingThrowEvent.Context.Ability == EAbility.Strength || savingThrowEvent.Context.Ability == EAbility.Dexterity);
    }

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return base.IsApplicablePostEvent(effectInstance, actionEvent) ||
               actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner;
    }

    public override async Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
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

        await base.ExecutePreEventAsync(effectInstance, actionEvent, cancellationToken);
        await resistanceEffect.ExecutePreEventAsync(effectInstance, actionEvent, cancellationToken);
    }

    public override async Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner)
        {
            effectInstance.Owner.Speed.Value = 0;
        }

        await base.ExecutePostEventAsync(effectInstance, actionEvent, cancellationToken);
    }
}
