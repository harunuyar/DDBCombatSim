namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class EffectExpiryEvent : IActionEvent
{
    public EffectExpiryEvent(EffectManager effectManager, EffectInstance effect)
    {
        EffectManager = effectManager;
        Effect = effect;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Effect Expiry: " + Effect.Effect.Name;

    public EffectManager EffectManager { get; }

    public EffectInstance Effect { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        EffectManager.ActiveEffects.Remove(Effect);

        Effect.Source.ActiveEffects.Remove(Effect);
        Effect.Owner.CausedEffects.Remove(Effect);

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
