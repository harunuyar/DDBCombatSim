﻿namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class ConcentrationSavingThrowEvent : BaseSavingThrowEvent
{
    public ConcentrationSavingThrowEvent(CombatContext combatContext, ICombatant attacker, ICombatant target, SavingThrowContext ctx)
        : base(combatContext, target, ctx)
    {
        Attacker = attacker;
    }

    public override string Name => "Attack Saving Throw";

    public ICombatant Attacker { get; }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        if (OverridingResult == null)
        {
            var rollResponse = await CombatContext.InputRequestManager.GetSavingThrowResult(Target.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll to save " + Target.Name,
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
                Result = ETestResult.Failure;
            }
            else if (RollResult == 20)
            {
                Result = ETestResult.Success;
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
    }
}
