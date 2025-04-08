namespace DDBCombatSim.Predefined5E2024.Effects.Conditions;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using DDBCombatSim.Predefined5E2024.Actions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Prone : BaseEffect, IActionProvider
{
    public Prone() : base("Prone")
    {
    }

    public override int Priority => 1;

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        if (actionEvent is AttackRollEvent attackRollEvent)
        {
            return attackRollEvent.Attacker == effectInstance.Owner || attackRollEvent.Target == effectInstance.Owner;
        }

        return false;
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
                if (attackRollEvent.Attacker.IsWithinRange(effectInstance.Owner, 5))
                {
                    attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Advantage));
                }
                else
                {
                    attackRollEvent.Advantage.Modifiers.Add(new Modifier<EAdvantage>(this, Name, EAdvantage.Disadvantage));
                }
            }
        }

        return Task.CompletedTask;
    }

    public IEnumerable<IAction> GetActions(CombatContext combatContext, ICombatant owner)
    {
        return
        [
            new StandUpAction(combatContext, owner)
        ];
    }
}
