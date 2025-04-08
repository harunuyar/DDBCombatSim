namespace DDBCombatSim.Predefined.Spells;

using DDBCombatSim.Action;
using DDBCombatSim.Action.Context;
using DDBCombatSim.Combat;
using DDBCombatSim.Combatant;
using DDBCombatSim.Spell;
using DDBCombatSim.Utils;

public class Firebolt : CommonAction, ISpell
{
    public Firebolt(CombatContext combatContext, ICombatant owner, EAbility spellcastingAbility) : base(combatContext, "Firebolt", owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
    {
        TargettingContext = new TargettingContext()
        {
            Range = 120,
            Count = 1,
            IncludeSelf = true,
            Intent = ETargetIntent.Harmful
        };

        AttackRollContext = new AttackRollContext()
        {
            Modifier = owner.GetAbilityModifier(spellcastingAbility)
        };

        DamageContext = new DamageContext()
        {
            DamageType = EDamageType.Fire
        };

        DamageRollContext = new RollContext()
        {
            Dice = new Dice(1, 10)
        };

        SpellContext = new SpellContext()
        {
            Spell = this
        };
    }

    public string Description => "You hurl a mote of fire at a creature or object within range. Make a ranged spell attack against the target. On a hit, the target takes 1d10 fire damage. A flammable object hit by this spell ignites if it isn't being worn or carried.";

    public int Level => 0;

    public ESpellSchool School => ESpellSchool.Evocation;

    public ECostType CastingTime => Cost.Type;

    public bool IsRitual => false;

    public bool RequiresConcentration => false;

    public Duration Duration => Duration.Instantaneous;

    public ESpellComponent Components => ESpellComponent.Verbal | ESpellComponent.Somatic;
}
