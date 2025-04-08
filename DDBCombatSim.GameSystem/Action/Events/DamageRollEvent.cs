namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Request.RequestModels;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class DamageRollEvent : IActionEvent
{
    private readonly CombatContext combatContext;

    public DamageRollEvent(CombatContext combatContext, ICombatant attacker, ICombatant target, DamageContext damageContext, RollContext rollContext, bool critical)
    {
        this.combatContext = combatContext;
        Attacker = attacker;
        Target = target;
        DamageContext = damageContext;
        RollContext = rollContext;
        Modifier = rollContext.Modifier.Clone();
        Advantage = rollContext.Advantage.Clone();
        Critical = new Modifier<bool>(this, "Default", critical);
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public string Name => "Damage Roll";

    public bool IsCompleted => Result != null;

    public EnumStat<ECancellation> Cancellation { get; }

    public Modifier<bool> Critical { get; }

    public ICombatant Attacker { get; }

    public ICombatant Target { get; }

    public DamageContext DamageContext { get; }

    public RollContext RollContext { get; }

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
            var rollResponse = await combatContext.InputRequestManager.GetHealRollResult(Attacker.Id, new RollRequest()
            {
                Name = Name,
                Description = "Roll to calculate damage.",
                Modifier = Modifier,
                Advantage = Advantage,
                DieCount = RollContext.Dice.DieCount,
                DieSize = RollContext.Dice.DieSize,
                Critical = Critical
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
