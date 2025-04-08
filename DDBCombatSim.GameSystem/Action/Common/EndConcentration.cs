namespace DDBCombatSim.GameSystem.Action.Common;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Effect.Common;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class EndConcentration : IAction
{
    public EndConcentration(CombatContext combatContext, ICombatant owner)
    {
        CombatContext = combatContext;
        Actor = owner;
    }

    public CombatContext CombatContext { get; }
    public ICombatant Actor { get; }
    public Cost Cost => new() { Type = ECostType.NonCombat, Amount = 1 };
    public string Name => "End Concentration";
    public ECancellation Cancellation { get; set; } = ECancellation.None;
    public bool IsMagicAction => false;

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var effectInstance = Actor.ActiveEffects.FirstOrDefault(e => e.Effect is Concentration);
        if (effectInstance == null)
        {
            Cancellation = ECancellation.Error;
            return;
        }

        await CombatContext.EffectManager.RemoveEffectAsync(effectInstance, cancellationToken);
    }
}
