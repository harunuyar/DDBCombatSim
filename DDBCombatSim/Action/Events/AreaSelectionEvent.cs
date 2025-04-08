namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Battlefield.Area;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class AreaSelectionEvent : IActionEvent
{
    public AreaSelectionEvent(InputRequestManager inputRequestManager, ICombatant actor, AreaSelectionContext ctx)
    {
        InputRequestManager = inputRequestManager;
        Actor = actor;
        AreaSelectionContext = ctx;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Area Selection";

    public InputRequestManager InputRequestManager { get; }

    public ICombatant Actor { get; }

    public AreaSelectionContext AreaSelectionContext { get; }

    public IArea? Area { get; private set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        Area = await InputRequestManager.SendAreaSelectionRequest(Actor.Id, AreaSelectionContext, cancellationToken);

        if (Area == null)
        {
            Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "No Input", ECancellation.UserCancelled));
            return;
        }

        IsCompleted = true;
    }
}
