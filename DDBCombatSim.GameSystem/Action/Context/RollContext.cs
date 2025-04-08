namespace DDBCombatSim.GameSystem.Action.Context;

using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;

public class RollContext
{
    public IntStat Modifier { get; set; } = new IntStat("Default", 0);
    public EnumStat<EAdvantage> Advantage { get; set; } = new EnumStat<EAdvantage>("Default", EAdvantage.None);
    public Dice Dice { get; set; } = null!;
}