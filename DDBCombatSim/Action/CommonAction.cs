namespace DDBCombatSim.Action;

using DDBCombatSim.Action.Context;
using DDBCombatSim.Battlefield.Area;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Effect;
using DDBCombatSim.Predefined.Effects.Common;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class CommonAction : IAction
{
    public CommonAction(CombatContext combatContext, string name, ICombatant owner, Cost cost, bool isMagicAction)
    {
        CombatContext = combatContext;
        Name = name;
        Cost = cost;
        Actor = owner;
        IsMagicAction = isMagicAction;
    }

    public string Name { get; }

    public Cost Cost { get; }

    public ECancellation Cancellation { get; set; }

    public CombatContext CombatContext { get; }

    public ICombatant Actor { get; }

    public bool SelfTarget { get; set; }

    public TargettingContext? TargettingContext { get; set; }

    public AreaSelectionContext? AreaSelectionContext { get; set; }

    public AttackRollContext? AttackRollContext { get; set; }

    public SavingThrowContext? SavingThrowContext { get; set; }

    public SpellContext? SpellContext { get; set; }

    public EffectContext? EffectContext { get; set; }

    public DamageContext? DamageContext { get; set; }

    public HealContext? HealContext { get; set; }

    public RollContext? DamageRollContext { get; set; }

    public RollContext? HealRollContext { get; set; }

    public List<ICombatant> Targets { get; set; } = new();

    public IArea? Area { get; set; }

    public bool IsMagicAction { get; set; }

    public virtual async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Cancellation.ShouldStopAction())
        {
            return;
        }

        if (SelfTarget)
        {
            Targets.Add(Actor);
        }

        if (TargettingContext != null)
        {
            var newTargets = await this.GetTargetsAsync(TargettingContext, cancellationToken);

            if (Cancellation.ShouldStopAction())
            {
                return;
            }

            Targets.AddRange(newTargets!);
        }

        if (AreaSelectionContext != null)
        {
            Area ??= await this.GetAreaAsync(AreaSelectionContext, cancellationToken);

            if (Cancellation.ShouldStopAction())
            {
                return;
            }

            var newTargets = await this.GetCombatantsInAreaAsync(Area!, AreaSelectionContext, cancellationToken);

            if (Cancellation.ShouldStopAction())
            {
                return;
            }

            Targets.AddRange(newTargets!);
        }

        if (Cancellation.ShouldStopAction())
        {
            return;
        }

        if (SpellContext != null)
        {
            var spellCast = await this.CastSpellAsync(SpellContext, cancellationToken);

            if (Cancellation.ShouldStopAction())
            {
                return;
            }
        }

        List<(ICombatant Target, ETestResult TestResult)> targetResults = [.. Targets.Select(t => (t, ETestResult.Success))];

        if (AttackRollContext != null)
        {
            for (int i = 0; i < targetResults.Count; i++)
            {
                if (targetResults[i].TestResult.IsSuccess())
                {
                    var attackRollResult = await this.MakeAttackRollAsync(targetResults[i].Target, AttackRollContext, cancellationToken);

                    if (Cancellation.ShouldStopAction())
                    {
                        return;
                    }

                    targetResults[i] = (targetResults[i].Target, attackRollResult.Value);
                }
            }
        }

        if (SavingThrowContext != null)
        {
            for (int i = 0; i < targetResults.Count; i++)
            {
                if (targetResults[i].TestResult.IsSuccess())
                {
                    var savingThrowResult = await this.MakeSavingThrowAsync(targetResults[i].Target, SavingThrowContext, cancellationToken);

                    if (Cancellation.ShouldStopAction())
                    {
                        return;
                    }

                    targetResults[i] = (targetResults[i].Target, savingThrowResult.Value.IsSuccess() ? ETestResult.Failure : ETestResult.Success);
                }
            }
        }

        if (DamageContext != null)
        {
            foreach (var (target, testResult) in targetResults.Where(x => x.TestResult.IsSuccess() || DamageContext.HalfDamageOnSave))
            {
                int damage;
                if (DamageContext.ConstantDamage.HasValue)
                {
                    damage = DamageContext.ConstantDamage.Value;
                }
                else if (DamageRollContext != null)
                {
                    var damageRoll = await this.MakeDamageRollAsync(target, DamageContext, DamageRollContext, testResult == ETestResult.CriticalSuccess, cancellationToken);

                    if (Cancellation.ShouldStopAction())
                    {
                        return;
                    }

                    damage = damageRoll!.Value;
                }
                else
                {
                    throw new Exception("Damage roll context not found.");
                }

                bool savedHalfDamage = DamageContext.HalfDamageOnSave && testResult.IsFailure();

                await this.ApplyDamageAsync(target, DamageContext, damage, savedHalfDamage, cancellationToken);

                if (Cancellation.ShouldStopAction())
                {
                    return;
                }
            }
        }

        if (HealContext != null)
        {
            foreach (var (target, _) in targetResults.Where(x => x.TestResult.IsSuccess()))
            {
                int heal;
                if (HealContext.ConstantHeal.HasValue)
                {
                    heal = HealContext.ConstantHeal.Value;
                }
                else if (HealRollContext != null)
                {
                    var healRoll = await this.MakeHealRollAsync(target, HealRollContext, cancellationToken);

                    if (Cancellation.ShouldStopAction())
                    {
                        return;
                    }

                    heal = healRoll!.Value;
                }
                else
                {
                    throw new Exception("Heal context not found.");
                }

                await this.ApplyHealingAsync(target, heal, cancellationToken);

                if (Cancellation.ShouldStopAction())
                {
                    return;
                }
            }
        }

        List<EffectInstance> effects = new();

        if (EffectContext != null)
        {
            foreach (var (target, _) in targetResults.Where(x => x.TestResult.IsSuccess()))
            {
                var effect = await this.ApplyEffectAsync(target, EffectContext, cancellationToken);

                if (Cancellation.ShouldStopAction())
                {
                    return;
                }

                effects.Add(effect!);
            }
        }

        if (SpellContext != null && SpellContext.Spell.RequiresConcentration)
        {
            var concentrationEffect = new Concentration(CombatContext, SpellContext.Spell, effects);
            var concentrationInstance = new EffectInstance(concentrationEffect, Actor, Actor, SpellContext.Spell.Duration.Copy());
            await CombatContext.EffectManager.ApplyEffectAsync(concentrationInstance, cancellationToken);
        }
    }
}