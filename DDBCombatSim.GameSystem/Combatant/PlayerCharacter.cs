namespace DDBCombatSim.GameSystem.Combatant;

using DDBCombatSim.GameSystem.Stats;

public class PlayerCharacter : CombatantBase
{
    public PlayerCharacter(string id, string name, int level, IntStat armorClass, IntStat maxHp, int currentHp, int tempHp, IntStat[] abilityScores, IntStat[] savingThrows)
        : base(id, name, ECreatureType.Humanoid, armorClass, maxHp, currentHp, tempHp, abilityScores, savingThrows)
    {
        Level = level;
        ProficiencyBonus = 2 + (Level - 1) / 4;
    }

    public int Level { get; set; }
}
