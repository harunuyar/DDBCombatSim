namespace DDBCombatSim.GameSystem.Action.Context;

using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Utils;

public class EffectContext
{
    public IEffect Effect { get; set; } = null!;
    public Duration Duration { get; set; } = null!;
}
