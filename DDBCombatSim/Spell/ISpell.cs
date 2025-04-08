namespace DDBCombatSim.Spell;

using DDBCombatSim.Utils;

public interface ISpell : IDndObject
{
    string Description { get; }
    int Level { get; }
    ESpellSchool School { get; }
    ECostType CastingTime { get; }
    bool IsRitual { get; }
    bool RequiresConcentration { get; }
    Duration Duration { get; }
    ESpellComponent Components { get; }
}
