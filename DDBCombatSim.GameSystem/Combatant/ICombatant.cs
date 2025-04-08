namespace DDBCombatSim.GameSystem.Combatant;

using DDBCombatSim.GameSystem.Battlefield;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;

public interface ICombatant : IBattlefieldObject
{
    string Id { get; }
    string Name { get; }
    ECreatureType CreatureType { get; }
    EDamageType Resistances { get; }
    EDamageType Immunities { get; }
    EDamageType Vulnurabilities { get; }
    DeathStatus DeathStatus { get; }
    ConsumableStat HitPoints { get; }
    IntStat MaxHp => HitPoints.BaseStat;
    ConsumableStat TempHp { get; }
    IntStat ArmorClass { get; }
    IntStat[] AbilityScores { get; }
    IntStat[] SavingThrowModifiers { get; }
    IntStat[] AbilityModifiers { get; }
    ConsumableStat Actions { get; }
    ConsumableStat BonusActions { get; }
    ConsumableStat Reactions { get; }
    ConsumableStat MagicActions { get; }
    ConsumableStat Speed { get; }
    ConsumableStat[] SpellSlots { get; }
    List<EffectInstance> ActiveEffects { get; }
    List<EffectInstance> CausedEffects { get; }
    int ProficiencyBonus { get; }
    IntStat GetAbilityModifier(EAbility ability);
    IntStat GetWeaponAttackRollModifier(EWeaponProperty weaponProperty, ERange range);
    IntStat GetWeaponDamageRollModifier(EWeaponProperty weaponProperty, ERange range, EWeaponHand weaponHand);
}
