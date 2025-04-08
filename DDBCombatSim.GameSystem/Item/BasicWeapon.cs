namespace DDBCombatSim.GameSystem.Item;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;
using System.Collections.Generic;

public class BasicWeapon : IWeapon
{
    public BasicWeapon(string name)
    {
        Name = name;
    }

    public EWeaponCategory WeaponCategory { get; init; } = EWeaponCategory.None;

    public EDamageType DamageType { get; init; } = EDamageType.None;

    public EWeaponProperty WeaponProperty { get; init; } = EWeaponProperty.None;

    public ERange Range { get; init; } = ERange.Melee;

    public EMagicNature MagicNature { get; init; } = EMagicNature.NonMagical;

    public EWeaponHand WeaponHand { get; init; } = EWeaponHand.None;

    public int? MeleeRange { get; init; } = null;

    public int? RangedNormalRange { get; init; } = null;

    public int? RangedLongRange { get; init; } = null;

    public int? ConstantDamage { get; init; } = null;

    public Dice? DamageDice { get; init; } = null;

    public Dice? TwoHandedDamageDice { get; init; }

    public int? Bonus { get; init; } = null;

    public string Name { get; }

    public IEnumerable<IAction> GetActions(CombatContext combatContext, ICombatant owner)
    {
        var attackRollModifier = owner.GetWeaponAttackRollModifier(WeaponProperty, Range);
        var damageRollModifier = owner.GetWeaponDamageRollModifier(WeaponProperty, Range, WeaponHand);

        if (Bonus.HasValue)
        {
            attackRollModifier.Modifiers.Add(new Modifier<int>(this, "Magic Weapon", Bonus.Value));
            damageRollModifier.Modifiers.Add(new Modifier<int>(this, "Magic Weapon", Bonus.Value));
        }

        var actions = new List<IAction>();

        if (MeleeRange.HasValue)
        {
            if (WeaponHand == EWeaponHand.Primary && DamageDice != null)
            {
                var action = new CommonAction(combatContext, Name, owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
                {
                    TargettingContext = new TargettingContext()
                    {
                        Count = 1,
                        Range = MeleeRange.Value,
                        IncludeSelf = true,
                        Intent = ETargetIntent.Harmful
                    },
                    AttackRollContext = new AttackRollContext()
                    {
                        Modifier = attackRollModifier,
                        SourceType = WeaponCategory == EWeaponCategory.Unarmed ? EAttackSourceType.Unarmed : EAttackSourceType.Weapon,
                        Range = Range
                    },
                    DamageContext = new DamageContext()
                    {
                        ConstantDamage = ConstantDamage,
                        DamageType = DamageType,
                        MagicNature = MagicNature
                    },
                    DamageRollContext = new RollContext()
                    {
                        Dice = DamageDice,
                        Modifier = damageRollModifier
                    }
                };

                actions.Add(action);
            }
            else if (WeaponHand == EWeaponHand.Offhand && DamageDice != null)
            {
                var action = new CommonAction(combatContext, Name, owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
                {
                    TargettingContext = new TargettingContext()
                    {
                        Count = 1,
                        Range = MeleeRange.Value,
                        IncludeSelf = true,
                        Intent = ETargetIntent.Harmful
                    },
                    AttackRollContext = new AttackRollContext()
                    {
                        Modifier = attackRollModifier,
                        SourceType = WeaponCategory == EWeaponCategory.Unarmed ? EAttackSourceType.Unarmed : EAttackSourceType.Weapon,
                        Range = Range
                    },
                    DamageContext = new DamageContext()
                    {
                        ConstantDamage = ConstantDamage,
                        DamageType = DamageType,
                        MagicNature = MagicNature
                    },
                    DamageRollContext = new RollContext()
                    {
                        Dice = DamageDice,
                        Modifier = damageRollModifier
                    }
                };
                actions.Add(action);
            }
            else if (WeaponHand == EWeaponHand.TwoHand && (DamageDice != null || TwoHandedDamageDice != null))
            {
                var action = new CommonAction(combatContext, Name, owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
                {
                    TargettingContext = new TargettingContext()
                    {
                        Count = 1,
                        Range = MeleeRange.Value,
                        IncludeSelf = true,
                        Intent = ETargetIntent.Harmful
                    },
                    AttackRollContext = new AttackRollContext()
                    {
                        Modifier = attackRollModifier,
                        SourceType = WeaponCategory == EWeaponCategory.Unarmed ? EAttackSourceType.Unarmed : EAttackSourceType.Weapon,
                        Range = Range
                    },
                    DamageContext = new DamageContext()
                    {
                        ConstantDamage = ConstantDamage,
                        DamageType = DamageType,
                        MagicNature = MagicNature
                    },
                    DamageRollContext = new RollContext()
                    {
                        Dice = (TwoHandedDamageDice ?? DamageDice)!,
                        Modifier = damageRollModifier
                    }
                };

                actions.Add(action);
            }
        }

        if (RangedNormalRange.HasValue)
        {
            if (DamageDice != null)
            {
                var action = new CommonAction(combatContext, Name, owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
                {
                    TargettingContext = new TargettingContext()
                    {
                        Count = 1,
                        Range = RangedNormalRange.Value,
                        IncludeSelf = true,
                        Intent = ETargetIntent.Harmful
                    },
                    AttackRollContext = new AttackRollContext()
                    {
                        Modifier = attackRollModifier,
                        SourceType = EAttackSourceType.Weapon,
                        Range = Range
                    },
                    DamageContext = new DamageContext()
                    {
                        ConstantDamage = ConstantDamage,
                        DamageType = DamageType,
                        MagicNature = MagicNature
                    },
                    DamageRollContext = new RollContext()
                    {
                        Dice = DamageDice,
                        Modifier = damageRollModifier
                    }
                };

                actions.Add(action);
            }
        }

        if (RangedLongRange.HasValue)
        {
            if (DamageDice != null)
            {
                var action = new CommonAction(combatContext, Name, owner, new Cost() { Type = ECostType.Action, Amount = 1 }, false)
                {
                    TargettingContext = new TargettingContext()
                    {
                        Count = 1,
                        Range = RangedLongRange.Value,
                        IncludeSelf = true,
                        Intent = ETargetIntent.Harmful
                    },
                    AttackRollContext = new AttackRollContext()
                    {
                        Modifier = attackRollModifier,
                        SourceType = EAttackSourceType.Weapon,
                        Range = Range,
                        Advantage = new EnumStat<EAdvantage>("Long Range Attack", EAdvantage.Disadvantage)
                    },
                    DamageContext = new DamageContext()
                    {
                        ConstantDamage = ConstantDamage,
                        DamageType = DamageType,
                        MagicNature = MagicNature
                    },
                    DamageRollContext = new RollContext()
                    {
                        Dice = DamageDice,
                        Modifier = damageRollModifier
                    }
                };
                actions.Add(action);
            }
        }

        return actions;
    }
}
