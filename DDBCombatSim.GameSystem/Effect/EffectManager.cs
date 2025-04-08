namespace DDBCombatSim.GameSystem.Effect;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Events;
using DDBCombatSim.GameSystem.Request;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class EffectManager
{
    public EffectManager(InputRequestManager inputRequestManager)
    {
        InputRequestManager = inputRequestManager;
        ActiveEffects = [];
    }

    public InputRequestManager InputRequestManager { get; }

    public HashSet<EffectInstance> ActiveEffects { get; }

    public async Task ApplyEffectAsync(EffectInstance effect, CancellationToken cancellationToken)
    {
        var applyEffectEvent = new ApplyEffectEvent(this, effect);

        await HandlePreEventEffectsAsync(applyEffectEvent, cancellationToken);
        await applyEffectEvent.ExecuteAsync(cancellationToken);
        await HandlePostEventEffectsAsync(applyEffectEvent, cancellationToken);
    }

    public async Task RemoveEffectAsync(EffectInstance effect, CancellationToken cancellationToken)
    {
        var removeEffectEvent = new EffectExpiryEvent(this, effect);

        await HandlePreEventEffectsAsync(removeEffectEvent, cancellationToken);
        await removeEffectEvent.ExecuteAsync(cancellationToken);
        await HandlePostEventEffectsAsync(removeEffectEvent, cancellationToken);
    }

    public async Task InvokeEffectAsync(IActionEvent actionEvent, EffectInstance effect, bool preEvent, CancellationToken cancellationToken)
    {
        var invokeEffectEvent = new InvokeEffectEvent(InputRequestManager, actionEvent, effect, preEvent);

        await HandlePreEventEffectsAsync(invokeEffectEvent, cancellationToken);
        await invokeEffectEvent.ExecuteAsync(cancellationToken);
        await HandlePostEventEffectsAsync(invokeEffectEvent, cancellationToken);
    }

    public async Task HandlePreEventEffectsAsync(IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        var effects = ActiveEffects.Where(effect => effect.IsApplicablePreEvent(actionEvent)).OrderByDescending(effect => effect.Effect.Priority).ToList();

        foreach (var effect in effects)
        {
            if (actionEvent is not InvokeEffectEvent)
            {
                await InvokeEffectAsync(actionEvent, effect, true, cancellationToken);
            }
            else
            {
                await effect.ExecutePreEventAsync(actionEvent, cancellationToken);
            }
        }

        await CheckExpiry(cancellationToken);
    }

    public async Task HandlePostEventEffectsAsync(IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        var effects = ActiveEffects.Where(effect => effect.IsApplicablePostEvent(actionEvent)).OrderByDescending(effect => effect.Effect.Priority).ToList();

        foreach (var effect in effects)
        {
            if (actionEvent is not InvokeEffectEvent)
            {
                await InvokeEffectAsync(actionEvent, effect, false, cancellationToken);
            }
            else
            {
                await effect.ExecutePostEventAsync(actionEvent, cancellationToken);
            }
        }

        await CheckExpiry(cancellationToken);
    }

    private async Task CheckExpiry(CancellationToken cancellationToken)
    {
        var effects = ActiveEffects.Where(effect => effect.Duration.IsFinished).ToList();

        foreach (var effect in effects)
        {
            await RemoveEffectAsync(effect, cancellationToken);
        }
    }
}
