﻿namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using System.Threading;
using System.Threading.Tasks;

public class Grappled : BaseEffect
{
    public Grappled() : base("Grappled")
    {
    }

    public override int Priority => 5;

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner;
    }

    public override Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is StartTurnEvent startTurnEvent && startTurnEvent.Actor == effectInstance.Owner)
        {
            effectInstance.Owner.Speed.Value = 0;
        }

        return Task.CompletedTask;
    }
}
