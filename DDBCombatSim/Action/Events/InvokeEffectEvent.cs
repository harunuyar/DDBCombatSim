namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Effect;
using DDBCombatSim.Request;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class InvokeEffectEvent : IActionEvent
{
    public InvokeEffectEvent(InputRequestManager inputRequestManager, IActionEvent actionEvent, EffectInstance effect, bool preEvent)
    {
        InputRequestManager = inputRequestManager;
        ActionEvent = actionEvent;
        Effect = effect;
        IsPreEvent = preEvent;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Invoke Effect";

    public InputRequestManager InputRequestManager { get; }

    public IActionEvent ActionEvent { get; }

    public EffectInstance Effect { get; }

    public bool IsPreEvent { get; }

    public Modifier<Cost>? OverridingCost { get; set; }

    public bool Invoked { get; private set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        bool canInvoke = false;

        var cost = OverridingCost?.Value ?? Effect.Effect.Cost;

        if (cost.IsFreeAction() || cost.IsNonCombat())
        {
            canInvoke = true;
        }
        else if (cost.IsAction())
        {
            canInvoke = Effect.Owner.Actions.CanUse(cost.Amount);
        }
        else if (cost.IsBonusAction())
        {
            canInvoke = Effect.Owner.BonusActions.CanUse(cost.Amount);
        }
        else if (cost.IsReaction())
        {
            canInvoke = Effect.Owner.Reactions.CanUse(cost.Amount);
        }

        bool shouldInvoke = false;

        if (canInvoke)
        {
            if (Effect.Effect.IsOptional)
            {
                var approvalRequest = new ApprovalRequest()
                {
                    Name = Name,
                    Description = $"Would you like to activate {Effect.Effect.Name} with the cost of 1 {Effect.Effect.Cost}?"
                };

                var response = await InputRequestManager.SendApprovalRequestAsync(Effect.Owner.Id, approvalRequest, cancellationToken);

                if (response == null)
                {
                    Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "No Input", ECancellation.UserCancelled));
                    return;
                }

                shouldInvoke = response.IsApproved;
            }
            else
            {
                shouldInvoke = true;
            }
        }
        
        if (shouldInvoke)
        {
            if (IsPreEvent)
            {
                await Effect.ExecutePreEventAsync(ActionEvent, cancellationToken);
            }
            else
            {
                await Effect.ExecutePostEventAsync(ActionEvent, cancellationToken);
            }

            if (cost.IsAction())
            {
                Effect.Owner.Actions.Use(cost.Amount);
            }
            else if (cost.IsBonusAction())
            {
                Effect.Owner.BonusActions.Use(cost.Amount);
            }
            else if (cost.IsReaction())
            {
                Effect.Owner.Reactions.Use(cost.Amount);
            }

            Invoked = true;
        }

        IsCompleted = true;
    }
}
