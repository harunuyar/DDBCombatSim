namespace DDBCombatSim.Combatant;

using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public class PlayerCharacter : CombatantBase
{
    public PlayerCharacter(string id, string name, int level, IntStat armorClass, IntStat maxHp, int currentHp, int tempHp, IntStat[] abilityScores, IntStat[] savingThrows)
        : base(id, name, ECreatureType.Humanoid, armorClass, maxHp, currentHp, tempHp, abilityScores, savingThrows)
    {
        Level = level;
    }

    public int Level { get; set; }

    public int ProficiencyBonus => 2 + (Level - 1) / 4;

    public override IntStat GetAbilityModifier(EAbility ability)
    {
        var result = new IntStat("Base", 0);
        result.AddOtherAsModifier(AbilityScores[(int)ability]);
        result.Modifiers.Add(new Modifier<int>(this, "Proficiency Bonus", ProficiencyBonus));
        return result;
    }
}
