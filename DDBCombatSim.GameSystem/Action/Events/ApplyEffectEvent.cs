namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class ApplyEffectEvent : IActionEvent
{
    public ApplyEffectEvent(EffectManager effectManager, EffectInstance effect)
    {
        EffectManager = effectManager;
        Effect = effect;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Apply Effect: " + Effect.Effect.Name;

    public EffectManager EffectManager { get; }

    public EffectInstance Effect { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        EffectManager.ActiveEffects.Add(Effect);

        Effect.Source.ActiveEffects.Add(Effect);
        Effect.Owner.CausedEffects.Add(Effect);

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
