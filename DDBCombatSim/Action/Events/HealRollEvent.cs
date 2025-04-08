namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class HealRollEvent : IActionEvent
{
    private readonly CombatContext combatContext;

    public HealRollEvent(CombatContext combatContext, ICombatant healer, ICombatant target, RollContext rollContext)
    {
        this.combatContext = combatContext;
        Healer = healer;
        Target = target;
        RollContext = rollContext;
        Modifier = rollContext.Modifier.Clone();
        Advantage = rollContext.Advantage.Clone();
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public IAction? Action { get; }

    public string Name => "Heal Roll";

    public bool IsCompleted => Result != null;

    public EnumStat<ECancellation> Cancellation { get; }

    public RollContext RollContext { get; }

    public ICombatant Healer { get; }

    public ICombatant Target { get; }

    public IntStat Modifier { get; }

    public EnumStat<EAdvantage> Advantage { get; }

    public int? RollResult { get; private set; }

    public IntStat? OverridingResult { get; set; }

    public IntStat? Result { get; private set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        if (OverridingResult == null)
        {
            var rollResponse = await combatContext.InputRequestManager.GetHealRollResult(Healer.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll to calculate healing.",
                Modifier = Modifier,
                Advantage = Advantage,
                DieCount = RollContext.Dice.DieCount,
                DieSize = RollContext.Dice.DieSize
            }, cancellationToken);

            if (rollResponse == null)
            {
                Cancellation.Modifiers.Add(new Modifier<ECancellation>(this, "No Input", ECancellation.UserCancelled));
                return;
            }

            RollResult = rollResponse.Roll;

            Result = new IntStat("Roll Result", RollResult.Value);
            Result.AddOtherAsModifier(Modifier);
        }
        else
        {
            Result = OverridingResult;
        }
    }
}
