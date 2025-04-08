namespace DDBCombatSim.Action.Context;

using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public class SavingThrowContext
{
    public IntStat DC { get; set; } = new IntStat("Default", 8);
    public EAbility Ability { get; set; }
    public IntStat Modifier { get; set; } = new IntStat("Default", 0);
    public EnumStat<EAdvantage> Advantage { get; set; } = new EnumStat<EAdvantage>("Default", EAdvantage.None);
}