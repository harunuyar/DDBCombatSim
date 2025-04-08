﻿namespace DDBCombatSim.Predefined.Spells;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Predefined.Effects.Common;
using DDBCombatSim.Spell;
using DDBCombatSim.Utils;

public class Stoneskin : CommonAction, ISpell
{
    public Stoneskin(CombatContext combatContext, ICombatant owner)
        : base(combatContext, "Stoneskin", owner, new Cost() { Type = ECostType.Action, Amount = 1 }, true)
    {
        TargettingContext = new TargettingContext()
        {
            Range = 1,
            Count = 1,
            IncludeSelf = true,
            Intent = ETargetIntent.Helpful
        };

        SpellContext = new SpellContext()
        {
            Spell = this
        };

        EffectContext = new EffectContext()
        {
            Effect = new NonMagicalResistance(
                EDamageType.Bludgeoning | EDamageType.Piercing | EDamageType.Slashing),
            Duration = Duration
        };
    }

    public string Description =>
        "The target gains resistance to nonmagical bludgeoning, piercing, and slashing damage for the duration.";

    public int Level => 4;
    public ESpellSchool School => ESpellSchool.Abjuration;
    public bool IsRitual => false;
    public bool RequiresConcentration => true;
    public Duration Duration => Duration.Forever;
    public ESpellComponent Components => ESpellComponent.Verbal | ESpellComponent.Somatic | ESpellComponent.Material;
    public ECostType CastingTime => ECostType.Action;
}
