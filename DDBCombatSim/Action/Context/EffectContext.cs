namespace DDBCombatSim.Action.Context;

using DDBCombatSim.Effect;
using DDBCombatSim.Utils;

public class EffectContext
{
    public IEffect Effect { get; set; } = null!;
    public Duration Duration { get; set; } = null!;
}
