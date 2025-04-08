namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public abstract class BaseSavingThrowEvent : IActionEvent
{
    public BaseSavingThrowEvent(CombatContext combatContext, ICombatant target, SavingThrowContext ctx)
    {
        CombatContext = combatContext;
        Target = target;
        Context = ctx;
        SaveDc = Context.DC.Clone();
        Modifier = Context.Modifier.Clone();
        Advantage = Context.Advantage.Clone();
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public abstract string Name { get; }

    public EnumStat<ECancellation> Cancellation { get; }

    public bool IsCompleted => RollResult != null;

    public CombatContext CombatContext { get; }

    public ICombatant Target { get; }

    public SavingThrowContext Context { get; }

    public IntStat SaveDc { get; }

    public IntStat Modifier { get; }

    public EnumStat<EAdvantage> Advantage { get; }

    public int? RollResult { get; protected set; }

    public Modifier<ETestResult>? OverridingResult { get; set; }

    public ETestResult? Result { get; protected set; }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken);
}
