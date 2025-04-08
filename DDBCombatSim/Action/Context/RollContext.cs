namespace DDBCombatSim.Action.Context;

using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public class RollContext
{
    public IntStat Modifier { get; set; } = new IntStat("Default", 0);
    public EnumStat<EAdvantage> Advantage { get; set; } = new EnumStat<EAdvantage>("Default", EAdvantage.None);
    public Dice Dice { get; set; } = null!;
}