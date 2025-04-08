namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Combatant;
using DDBCombatSim.Spell;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class InvokeActionEvent : IActionEvent
{
    public InvokeActionEvent(ICombatant actor, IAction action)
    {
        Actor = actor;
        Action = action;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Invoke Action";

    public ICombatant Actor { get; }

    public IAction Action { get; }

    public Modifier<Cost>? OverridingCost { get; set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        var spell = Action as ISpell;

        bool canInvoke = false;

        var cost = OverridingCost?.Value ?? Action.Cost;

        if (cost.IsFreeAction() || cost.IsNonCombat())
        {
            canInvoke = true;
        }
        else if (cost.IsMovement())
        {
            canInvoke = Actor.Speed.CanUse(cost.Amount);
        }
        else if (cost.IsAction())
        {
            canInvoke = Actor.Actions.CanUse(cost.Amount);
        }
        else if (cost.IsBonusAction())
        {
            canInvoke = Actor.BonusActions.CanUse(cost.Amount);
        }
        else if (cost.IsReaction())
        {
            canInvoke = Actor.Reactions.CanUse(cost.Amount);
        }

        if (Action.IsMagicAction)
        {
            canInvoke &= Actor.MagicActions.CanUse(1);
        }

        if (spell != null)
        {
            canInvoke &= Actor.SpellSlots[spell.Level - 1].CanUse(1);
        }

        if (canInvoke)
        {
            if (cost.IsMovement())
            {
                Actor.Speed.Use(cost.Amount);
            }
            else if (cost.IsAction())
            {
                Actor.Actions.Use();
            }
            else if (cost.IsBonusAction())
            {
                Actor.BonusActions.Use();
            }   
            else if (cost.IsReaction())
            {
                Actor.Reactions.Use();
            }

            if (Action.IsMagicAction)
            {
                Actor.MagicActions.Use(1);
            }

            if (spell != null)
            {
                Actor.SpellSlots[spell.Level - 1].Use(1);
            }

            await Action.ExecuteAsync(cancellationToken);

            Cancellation.Modifiers.Add(new Modifier<ECancellation>(Action, "Action", Action.Cancellation));
            IsCompleted = !Cancellation.Value.ShouldStopAction();

            if (!IsCompleted && Cancellation.Value.ShouldRefund())
            {
                if (cost.IsMovement())
                {
                    Actor.Speed.Restore(cost.Amount);
                }
                else if (cost.IsAction())
                {
                    Actor.Actions.Restore(cost.Amount);
                }
                else if (cost.IsBonusAction())
                {
                    Actor.BonusActions.Restore(cost.Amount);
                }
                else if (cost.IsReaction())
                {
                    Actor.Reactions.Restore(cost.Amount);
                }

                if (Action.IsMagicAction)
                {
                    Actor.MagicActions.Restore(1);
                }

                if (spell != null)
                {
                    Actor.SpellSlots[spell.Level - 1].Restore(1);
                }
            }
        }
        else
        {
           Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "Can't afford", ECancellation.UserCancelled));
        } 
    }
}
