﻿namespace DDBCombatSim.Predefined5E2024.Actions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Utils;
using DDBCombatSim.Predefined5E2024.Effects.Conditions;

public class StandUpAction : IAction
{
    public StandUpAction(CombatContext combatContext, ICombatant owner)
    {
        CombatContext = combatContext;
        Actor = owner;
        Cost = new Cost() { Type = ECostType.Action, Amount = owner.Speed.MaxValue / 2 };
    }

    public string Name => "Stand Up";

    public Cost Cost { get; }

    public CombatContext CombatContext { get; }

    public ICombatant Actor { get; }

    public ECancellation Cancellation { get; set; }

    public bool IsMagicAction => false;

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var effectInstance = Actor.ActiveEffects.FirstOrDefault(e => e.Effect is Prone);
        if (effectInstance == null)
        {
            Cancellation |= ECancellation.Error;
            return;
        }

        await CombatContext.EffectManager.RemoveEffectAsync(effectInstance, cancellationToken);
    }
}
