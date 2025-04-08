namespace DDBCombatSim.Action;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Battlefield.Area;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Effect;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public static class ActionExtensions
{
    public static async Task<IEnumerable<ICombatant>?> GetTargetsAsync(this IAction action, TargettingContext ctx, CancellationToken cancellationToken)
    {
        var targetRequest = new TargetRequest()
        {
            Range = ctx.Range,
            Count = ctx.Count,
            IncludeSelf = ctx.IncludeSelf
        };

        var targetResponse = await action.CombatContext.InputRequestManager.GetTargets(action.Actor.Id, targetRequest, cancellationToken);

        if (targetResponse == null)
        {
            action.Cancellation |= ECancellation.UserCancelled;
            return null;
        }

        var targets = new List<ICombatant>();

        foreach (var target in targetResponse.Targets)
        {
            var targetCombatant = action.CombatContext.Combatants.Find(c => c.Id == target);

            if (targetCombatant == null)
            {
                action.Cancellation |= ECancellation.Error;
                throw new Exception("Target not found.");
            }

            if (targetCombatant == action.Actor && !ctx.IncludeSelf)
            {
                action.Cancellation |= ECancellation.Error;
                throw new Exception("Self targetting is not allowed.");
            }

            var targettingEvent = new TargettingEvent(action.Actor, targetCombatant, ctx);
            await action.ExecuteEventAsync(targettingEvent, action.CombatContext, cancellationToken);

            if (action.Cancellation.ShouldStopAction())
            {
                return null;
            }

            targets.Add(targetCombatant);
        }

        return targets;
    }

    public static async Task<IArea?> GetAreaAsync(this IAction action, AreaSelectionContext ctx, CancellationToken cancellationToken)
    {
        var areaSelectionEvent = new AreaSelectionEvent(action.CombatContext.InputRequestManager, action.Actor, ctx);
        await action.ExecuteEventAsync(areaSelectionEvent, action.CombatContext, cancellationToken);

        if (action.Cancellation.ShouldStopAction())
        {
            return null;
        }

        if (areaSelectionEvent.Area == null)
        {
            action.Cancellation |= ECancellation.Error;
            throw new Exception("Area not found.");
        }

        return areaSelectionEvent.Area;
    }

    public static async Task<IEnumerable<ICombatant>?> GetCombatantsInAreaAsync(this IAction action, IArea area, AreaSelectionContext ctx, CancellationToken cancellationToken)
    {
        var affectedTiles = action.CombatContext.Battlefield.Tiles.Where(t => area.Intersects(t));
        var combatantsInArea = affectedTiles.Select(t => t.Objects).OfType<ICombatant>();

        var targets = new List<ICombatant>();

        if (ctx.ControlledArea)
        {
            var targetSelectionRequest = new TargetSelectionRequest()
            {
                Options = combatantsInArea.Select(c => c.Id),
                Limit = ctx.TargetLimit
            };

            var targetResponse = await action.CombatContext.InputRequestManager.GetTargetSelection(action.Actor.Id, targetSelectionRequest, cancellationToken);

            if (targetResponse == null)
            {
                action.Cancellation |= ECancellation.UserCancelled;
                return null;
            }

            foreach (var target in targetResponse.Targets)
            {
                var targetCombatant = action.CombatContext.Combatants.Find(c => c.Id == target);

                if (targetCombatant == null)
                {
                    action.Cancellation |= ECancellation.Error;
                    throw new Exception("Target not found.");
                }

                targets.Add(targetCombatant);
            }
        }
        else
        {
            targets.AddRange(combatantsInArea);
        }

        return targets;
    }

    public static async Task<ETestResult?> MakeAttackRollAsync(this IAction action, ICombatant target, AttackRollContext ctx, CancellationToken cancellationToken)
    {
        var attackRollEvent = new AttackRollEvent(action.CombatContext, action.Actor, target, ctx);
        await action.ExecuteEventAsync(attackRollEvent, action.CombatContext, cancellationToken);
        
        if (action.Cancellation.ShouldStopAction())
        {
            return null;
        }

        if (!attackRollEvent.Result.HasValue)
        {
            action.Cancellation |= ECancellation.Error;
            throw new Exception("Attack roll result not found.");
        }

        return attackRollEvent.Result.Value;
    }

    public static async Task<ETestResult?> MakeSavingThrowAsync(this IAction action, ICombatant target, SavingThrowContext ctx, CancellationToken cancellationToken)
    {
        var savingThrowEvent = new AttackSavingThrowEvent(action.CombatContext, action.Actor, target, ctx);
        await action.ExecuteEventAsync(savingThrowEvent, action.CombatContext, cancellationToken);

        if (action.Cancellation.ShouldStopAction())
        {
            return null;
        }

        if (!savingThrowEvent.Result.HasValue)
        {
            action.Cancellation |= ECancellation.Error;
            throw new Exception("Saving throw result not found.");
        }

        return savingThrowEvent.Result.Value;
    }

    public static async Task<IntStat?> MakeDamageRollAsync(this IAction action, ICombatant target, DamageContext damageCtx, RollContext rollCtx, bool critical, CancellationToken cancellationToken)
    {
        var damageRollEvent = new DamageRollEvent(action.CombatContext, action.Actor, target, damageCtx, rollCtx, critical);
        await action.ExecuteEventAsync(damageRollEvent, action.CombatContext, cancellationToken);

        if (action.Cancellation.ShouldStopAction())
        {
            return null;
        }

        if (damageRollEvent.Result == null)
        {
            action.Cancellation |= ECancellation.Error;
            throw new Exception("Damage roll result not found.");
        }

        return damageRollEvent.Result;
    }

    public static async Task<IntStat?> MakeHealRollAsync(this IAction action, ICombatant target, RollContext ctx, CancellationToken cancellationToken)
    {
        var healRollEvent = new HealRollEvent(action.CombatContext, action.Actor, target, ctx);
        await action.ExecuteEventAsync(healRollEvent, action.CombatContext, cancellationToken);
        
        if (action.Cancellation.ShouldStopAction())
        {
            return null;
        }

        if (healRollEvent.Result == null)
        {
            action.Cancellation |= ECancellation.Error;
            throw new Exception("Heal roll result not found.");
        }

        return healRollEvent.Result;
    }

    public static async Task<bool> CastSpellAsync(this IAction action, SpellContext ctx, CancellationToken cancellationToken)
    {
        var spellEvent = new SpellCastEvent(action.CombatContext, action.Actor, ctx.Spell);
        await action.ExecuteEventAsync(spellEvent, action.CombatContext, cancellationToken);
        return spellEvent.IsCompleted;
    }

    public static async Task<bool> ApplyDamageAsync(this IAction action, ICombatant target, DamageContext ctx, int amount, bool savedHalfDamage, CancellationToken cancellationToken)
    {
        var damageEvent = new ApplyDamageEvent(action.Actor, target, ctx, amount);

        if (savedHalfDamage)
        {
            int halfDamage = (int)Math.Ceiling(amount / 2.0);
            damageEvent.Amount.Modifiers.Add(new Modifier<int>(action, "Half Damage on Save", -halfDamage));
        }

        await action.ExecuteEventAsync(damageEvent, action.CombatContext, cancellationToken);
        return damageEvent.IsCompleted;
    }

    public static async Task<bool> ApplyHealingAsync(this IAction action, ICombatant target, int amount, CancellationToken cancellationToken)
    {
        var healEvent = new ApplyHealingEvent(action.Actor, target, amount);
        await action.ExecuteEventAsync(healEvent, action.CombatContext, cancellationToken);
        return healEvent.IsCompleted;
    }

    public static async Task<EffectInstance?> ApplyEffectAsync(this IAction action, ICombatant target, EffectContext ctx, CancellationToken cancellationToken)
    {
        var effectInstance = new EffectInstance(ctx.Effect, action.Actor, target, ctx.Duration);
        var effectEvent = new ApplyEffectEvent(action.CombatContext.EffectManager, effectInstance);
        await action.ExecuteEventAsync(effectEvent, action.CombatContext, cancellationToken);
        return effectEvent.IsCompleted ? effectInstance : null;
    }

    public static async Task ExecuteEventAsync(this IAction action, IActionEvent actionEvent, CombatContext combatContext, CancellationToken cancellationToken)
    {
        await combatContext.EffectManager.HandlePreEventEffectsAsync(actionEvent, cancellationToken);

        action.Cancellation |= actionEvent.Cancellation.Value;

        if (action.Cancellation.ShouldStopAction())
        {
            return;
        }

        await actionEvent.ExecuteAsync(cancellationToken);

        action.Cancellation |= actionEvent.Cancellation.Value;

        if (action.Cancellation.ShouldStopAction())
        {
            return;
        }

        await combatContext.EffectManager.HandlePostEventEffectsAsync(actionEvent, cancellationToken);

        action.Cancellation |= actionEvent.Cancellation.Value;

        if (action.Cancellation.ShouldStopAction())
        {
            return;
        }
    }
}
