namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class DeathSavingThrowEvent : BaseSavingThrowEvent
{
    public DeathSavingThrowEvent(CombatContext combatContext, ICombatant actor)
        : base(combatContext, actor, new SavingThrowContext() { DC = new IntStat("Default", 10) })
    {
    }

    public override string Name => "Death Save";

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        if (OverridingResult == null)
        {
            var rollResponse = await CombatContext.InputRequestManager.GetDeathSavingThrowResult(Target.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll to save " + Target.Name + " from death.",
                Target = SaveDc,
                Modifier = Modifier,
                Advantage = Advantage,
                DieCount = 1,
                DieSize = 20
            }, cancellationToken);

            if (rollResponse == null)
            {
                Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "No Input", ECancellation.UserCancelled));
                return;
            }

            RollResult = rollResponse.Roll;

            if (RollResult == 1)
            {
                Result = ETestResult.CriticalFailure;
            }
            else if (RollResult == 20)
            {
                Result = ETestResult.CriticalSuccess;
            }
            else if (RollResult.Value + Modifier.Value >= SaveDc.Value)
            {
                Result = ETestResult.Success;
            }
            else
            {
                Result = ETestResult.Failure;
            }
        }
        else
        {
            Result = OverridingResult.Value;
        }

        switch (Result)
        {
            case ETestResult.CriticalFailure:
                Target.DeathStatus.AddFailure(2);
                break;
            case ETestResult.CriticalSuccess:
                Target.DeathStatus.Reset();
                Target.HitPoints.Restore(1);
                break;
            case ETestResult.Success:
                Target.DeathStatus.AddSuccess(1);
                break;
            case ETestResult.Failure:
                Target.DeathStatus.AddFailure(1);
                break;
        }
    }
}
