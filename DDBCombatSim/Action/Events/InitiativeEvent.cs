namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class InitiativeEvent : IActionEvent
{
    public InitiativeEvent(CombatContext combatContext, ICombatant target, IntStat modifier, EnumStat<EAdvantage> advantage)
    {
        CombatContext = combatContext;
        Target = target;
        Modifier = modifier;
        Advantage = advantage;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted => Result != null;

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Initiative";

    public CombatContext CombatContext { get; }

    public ICombatant Target { get; }

    public IntStat Modifier { get; }

    public EnumStat<EAdvantage> Advantage { get; }

    public IntStat? Result { get; private set; }

    public IntStat? OverriddenResult { get; set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        if (OverriddenResult == null)
        {
            var rollResponse = await CombatContext.InputRequestManager.GetInitiativeRollResult(Target.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll for initiative.",
                DieCount = 1,
                DieSize = 20,
                Advantage = Advantage,
                Modifier = Modifier
            }, cancellationToken);

            if (rollResponse == null)
            {
                Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "No Input", ECancellation.UserCancelled));
                return;
            }

            Result = new IntStat("Initiative Roll", rollResponse.Roll);
        }
        else
        {
            Result = OverriddenResult;
        }
    }
}
