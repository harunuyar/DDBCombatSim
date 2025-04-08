namespace DDBCombatSim.GameSystem.Action.Events;

using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Spell;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class SpellCastEvent : IActionEvent
{
    public SpellCastEvent(CombatContext combatContext, ICombatant actor, ISpell spell)
    {
        CombatContext = combatContext;
        Actor = actor;
        Spell = spell;
        Cancellation = new EnumStat<ECancellation>("Cancellation", ECancellation.None);
    }

    public bool IsCompleted { get; private set; }

    public EnumStat<ECancellation> Cancellation { get; }

    public string Name => "Spell Cast: " + Spell.Name;

    public CombatContext CombatContext { get; }

    public ICombatant Actor { get; }

    public ISpell Spell { get; }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.Value.ShouldStopActionEvent())
        {
            return Task.CompletedTask;
        }

        IsCompleted = true;
        return Task.CompletedTask;
    }
}
