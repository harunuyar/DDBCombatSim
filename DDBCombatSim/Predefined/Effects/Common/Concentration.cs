namespace DDBCombatSim.Predefined.Effects.Common;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Context;
using DDBCombatSim.Action.Events;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Effect;
using DDBCombatSim.Predefined.Actions;
using DDBCombatSim.Spell;
using DDBCombatSim.Utils;
using System.Threading;
using System.Threading.Tasks;

public class Concentration : BaseEffect, IActionProvider
{
    private readonly CombatContext combatContext;

    public Concentration(CombatContext combatContext, ISpell concentratedSpell, IEnumerable<EffectInstance> effects) : base("Concentration")
    {
        this.combatContext = combatContext;
        ConcentratedSpell = concentratedSpell;
        Effects = effects;
    }

    public ISpell ConcentratedSpell { get; }

    public IEnumerable<EffectInstance> Effects { get; }

    public override Task ExecutePreEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is SpellCastEvent)
        {
            effectInstance.Duration.IsFinished = true;

            foreach (var effect in Effects)
            {
                effect.Duration.IsFinished = true;
            }
        }

        return Task.CompletedTask;
    }

    public override async Task ExecutePostEventAsync(EffectInstance effectInstance, IActionEvent actionEvent, CancellationToken cancellationToken)
    {
        if (actionEvent is ApplyDamageEvent applyDamageEvent)
        {
            var dcStat = new Stats.IntStat("Base", 10);
            int extraDc = applyDamageEvent.Amount.Value / 2 - 10;
            if (extraDc > 0)
            {
                dcStat.Modifiers.Add(new Stats.Modifier<int>(applyDamageEvent, "Damage", extraDc));
            }

            var modifierStat = new Stats.IntStat("Base", 0);
            modifierStat.AddOtherAsModifier(effectInstance.Owner.SavingThrowModifiers[(int)EAbility.Constitution]);

            var advantageStat = new Stats.EnumStat<EAdvantage>("Concentration Save Advantage", EAdvantage.None);

            var savingThrowContext = new SavingThrowContext()
            {
                DC = dcStat,
                Modifier = modifierStat,
                Advantage = advantageStat,
                Ability = EAbility.Constitution
            };

            var savingThrowEvent = new ConcentrationSavingThrowEvent(combatContext, applyDamageEvent.Source, effectInstance.Owner, savingThrowContext);
            await savingThrowEvent.ExecuteAsync(cancellationToken);

            if (savingThrowEvent.Result.HasValue && savingThrowEvent.Result.Value.IsFailure())
            {
                effectInstance.Duration.IsFinished = true;

                foreach (var effect in Effects)
                {
                    effect.Duration.IsFinished = true;
                }
            }
        }
        else if (actionEvent is EffectExpiryEvent effectExpiryEvent)
        {
            if (Effects.Contains(effectExpiryEvent.Effect))
            {
                effectInstance.Duration.IsFinished = true;

                foreach (var effect in Effects)
                {
                    effect.Duration.IsFinished = true;
                }
            }
        }
    }

    public override bool IsApplicablePostEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is ApplyDamageEvent applyDamageEvent && applyDamageEvent.Target == effectInstance.Owner ||
            actionEvent is EffectExpiryEvent effectExpiryEvent && effectExpiryEvent.Effect == effectInstance;
    }

    public override bool IsApplicablePreEvent(EffectInstance effectInstance, IActionEvent actionEvent)
    {
        return actionEvent is SpellCastEvent spellCastEvent && spellCastEvent.Actor == effectInstance.Owner && spellCastEvent.Spell.RequiresConcentration;
    }

    public IEnumerable<IAction> GetActions(CombatContext combatContext, ICombatant owner)
    {
        return
        [
            new EndConcentration(combatContext, owner)
        ];
    }
}
