namespace DDBCombatSim.Predefined5E2024.Spells;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Battlefield.Area;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Spell;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Threading;
using System.Threading.Tasks;

public class IceKnife : IAction, ISpell
{
    public IceKnife(CombatContext combatContext, ICombatant owner, EAbility spellcastingAbility)
    {
        CombatContext = combatContext;
        Actor = owner;
        SpellcastingAbility = spellcastingAbility;
    }

    public string Name => "Ice Knife";

    public string Description => "You create a shard of ice and fling it at one creature within range. Make a ranged spell attack against the target. On a hit, the target takes 1d10 piercing damage. Hit or miss, the shard then explodes. The target and each creature within 5 feet of it must succeed on a Dexterity saving throw or take 2d6 cold damage.";

    public int Level => 1;

    public ESpellSchool School => ESpellSchool.Evocation;

    public ECostType CastingTime => ECostType.Action;

    public bool IsRitual => false;

    public bool RequiresConcentration => false;

    public Duration Duration => Duration.Instantaneous;

    public ESpellComponent Components => ESpellComponent.Verbal | ESpellComponent.Somatic;

    public CombatContext CombatContext { get; }

    public ICombatant Actor { get; }

    public Cost Cost => new() { Type = ECostType.Action, Amount = 1 };

    public EAbility SpellcastingAbility { get; }

    public ECancellation Cancellation { get; set; }

    public bool IsMagicAction => true;

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var modifier = Actor.GetAbilityModifier(SpellcastingAbility);

        var action1 = new CommonAction(CombatContext, "Ice Knife Attack", Actor, new Cost { Type = ECostType.NonCombat, Amount = 1 }, false)
        {
            SpellContext = new SpellContext()
            {
                Spell = this
            },
            TargettingContext = new TargettingContext()
            {
                Count = 1,
                Range = 60,
                IncludeSelf = true,
                Intent = ETargetIntent.Harmful
            },
            AttackRollContext = new AttackRollContext()
            {
                Modifier = modifier
            },
            DamageContext = new DamageContext()
            {
                DamageType = EDamageType.Piercing
            },
            DamageRollContext = new RollContext()
            {
                Dice = new Dice(1, 10)
            }
        };

        await action1.ExecuteAsync(cancellationToken);

        Cancellation |= action1.Cancellation;

        if (Cancellation.ShouldStopAction())
        {
            return;
        }

        if (action1.Targets.Count != 1)
        {
            Cancellation |= ECancellation.Error;
            throw new Exception("Ice Knife Attack did not target exactly 1 creature.");
        }

        var saveDc = new IntStat("Base", 8);
        saveDc.AddOtherAsModifier(modifier);

        var areaDefinition = new SphereAreaDefinition(5);
        var area = areaDefinition.Create(action1.Targets[0].Position.GetCenterPoint());

        var action2 = new CommonAction(CombatContext, "Ice Knife Explosion", Actor, new Cost { Type = ECostType.NonCombat, Amount = 1 }, false)
        {
            Area = area,
            AreaSelectionContext = new AreaSelectionContext()
            {
                AreaDefinition = areaDefinition
            },
            SavingThrowContext = new SavingThrowContext()
            {
                Ability = EAbility.Dexterity,
                DC = saveDc
            },
            DamageContext = new DamageContext()
            {
                DamageType = EDamageType.Cold
            },
            DamageRollContext = new RollContext()
            {
                Dice = new Dice(2, 6)
            }
        };

        await action2.ExecuteAsync(cancellationToken);

        Cancellation |= action2.Cancellation;
    }
}
