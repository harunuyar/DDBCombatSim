namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class AttackRollEvent : IActionEvent
{
    private readonly CombatContext combatContext;

    public AttackRollEvent(CombatContext combatContext, ICombatant attacker, ICombatant target, AttackRollContext ctx)
    {
        this.combatContext = combatContext;
        Attacker = attacker;
        Target = target;
        Context = ctx;
        Modifier = Context.Modifier.Clone();
        Advantage = Context.Advantage.Clone();
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public string Name => "Attack Roll";

    public bool IsCompleted => Result.HasValue;

    public EnumStat<ECancellation> Cancellation { get; }

    public ICombatant Attacker { get; }

    public ICombatant Target { get; }

    public AttackRollContext Context { get; }

    public IntStat Modifier { get; }

    public EnumStat<EAdvantage> Advantage { get; }

    public int? RollResult { get; private set; }

    public Modifier<ETestResult>? OverridingResult { get; set; }

    public ETestResult? Result { get; private set; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return;
        }

        if (OverridingResult == null)
        {
            var rollResponse = await combatContext.InputRequestManager.GetAttackRollResult(Attacker.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll to hit " + Target.Name,
                Target = Target.ArmorClass,
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
                Result = ETestResult.CriticalFailure;
            }
            else if (RollResult == 20)
            {
                Result = ETestResult.CriticalSuccess;
            }
            else if (RollResult.Value + Modifier.Value >= Target.ArmorClass.Value)
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
