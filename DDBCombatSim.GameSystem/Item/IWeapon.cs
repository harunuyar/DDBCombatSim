namespace DDBCombatSim.GameSystem.Item;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Utils;

public interface IWeapon : IItem, IActionProvider
{
    EWeaponCategory WeaponCategory { get; }
    EDamageType DamageType { get; }
    EWeaponProperty WeaponProperty { get; }
    ERange Range { get; }
    EMagicNature MagicNature { get; }
    EWeaponHand WeaponHand { get; }
    int? MeleeRange { get; }
    int? RangedNormalRange { get; }
    int? RangedLongRange { get; }
    int? ConstantDamage { get; }
    Dice? DamageDice { get; }
    Dice? TwoHandedDamageDice { get; }
    int? Bonus { get; }
}
