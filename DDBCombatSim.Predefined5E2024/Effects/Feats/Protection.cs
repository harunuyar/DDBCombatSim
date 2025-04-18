﻿namespace DDBCombatSim.Predefined5E2024.Effects.Feats;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Protection : BaseEffect
{
    public Protection() : base("Protection")
    {
    }

    public override bool IsOptional => true;

    public override Cost Cost => new() { Type = ECostType.Reaction, Amount = 1 };

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is AttackRollEvent attackRollEvent && attackRollEvent.Target != effectInstance.Owner && attackRollEvent.Target.IsWithinRange(effectInstance.Owner, 5);
    }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
        }

        return Task.CompletedTask;
    }
}
