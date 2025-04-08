namespace DDBCombatSim.Action.Events;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Combatant;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class ApplyDamageEvent : IActionEvent
{
    public ApplyDamageEvent(ICombatant source, ICombatant target, DamageContext ctx, int amount)
    {
        Source = source;
        Target = target;
        Context = ctx;
        Amount = new IntStat("Base", amount);
        Resistances = new EnumStat<EDamageType>("Resistances", target.Resistances);
        Immunities = new EnumStat<EDamageType>("Immunities", target.Immunities);
        Vulnurabilities = new EnumStat<EDamageType>("Vulnurabilities", target.Vulnurabilities);
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Apply Damage";

    public ICombatant Source { get; }

    public ICombatant Target { get; }

    public DamageContext Context { get; }

    public IntStat Amount { get; }

    public EnumStat<EDamageType> Resistances { get; set; }

    public EnumStat<EDamageType> Immunities { get; set; }

    public EnumStat<EDamageType> Vulnurabilities { get; set; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        int damage = Amount.Value;

        if (Vulnurabilities.Value.HasFlag(Context.DamageType))
        {
            damage *= 2;
        }
        else if (Immunities.Value.HasFlag(Context.DamageType))
        {
            damage = 0;
        }
        else if (Resistances.Value.HasFlag(Context.DamageType))
        {
            damage /= 2;
        }

        int tempHp = Target.TempHp.Value;
        Target.TempHp.Use(Math.Min(damage, tempHp));
        Target.HitPoints.Use(Math.Max(0, damage - tempHp));

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
