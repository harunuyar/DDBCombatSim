namespace DDBCombatSim.GameSystem.Effect;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Utils;

public class EffectInstance
{
    public EffectInstance(IEffect effect, ICombatant owner, ICombatant source, Duration duration)
    {
        Effect = effect;
        Owner = owner;
        Source = source;
        Duration = duration;
    }

    public IEffect Effect { get; }

    public ICombatant Owner { get; }

    public ICombatant Source { get; }

    public Duration Duration { get; }

    public bool IsApplicablePreEvent(IActionEvent actionEvent)
    {
        return Effect.IsApplicablePreEvent(this, actionEvent);
    }

    public bool IsApplicablePostEvent(IActionEvent actionEvent)
    {
        return Effect.IsApplicablePostEvent(this, actionEvent);
    }

    public async Task ExecutePreEventAsync(IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        Duration.Trigger();
        await Effect.ExecutePreEventAsync(this, actionEvent, cancellationToken);
    }

    public async Task ExecutePostEventAsync(IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        Duration.Trigger();
        await Effect.ExecutePostEventAsync(this, actionEvent, cancellationToken);
    }
}
