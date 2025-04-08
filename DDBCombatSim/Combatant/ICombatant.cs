namespace DDBCombatSim.Combatant;

using DDBCombatSim.Battlefield;
using DDBCombatSim.Effect;
using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

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
    IntStat GetAbilityModifier(EAbility ability);
}
