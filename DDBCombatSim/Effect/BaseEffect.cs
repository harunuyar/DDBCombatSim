namespace DDBCombatSim.Effect;

using DDBCombatSim.Action;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public abstract class BaseEffect : IEffect
{
    public BaseEffect(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public virtual int Priority => 1;
    public virtual bool IsOptional => false;
    public virtual Cost Cost => new Cost { Type = ECostType.NonCombat, Amount = 1 };

    public virtual Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return false;
    }

    public virtual bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return false;
    }
}
