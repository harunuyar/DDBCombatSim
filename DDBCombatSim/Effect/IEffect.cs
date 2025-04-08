namespace DDBCombatSim.Effect;

using DDBCombatSim.Action;
using DDBCombatSim.Utils;

public interface IEffect : IDndObject
{
    int Priority { get; }
    bool IsOptional { get; }
    Cost Cost { get; }
    Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken);
    Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken);
    bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent);
    bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent);
}