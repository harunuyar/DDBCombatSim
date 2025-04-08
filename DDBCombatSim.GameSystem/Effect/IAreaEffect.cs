namespace DDBCombatSim.GameSystem.Effect;

using DDBCombatSim.GameSystem.Battlefield.Area;

public interface IAreaEffect : IEffect
{
    public IArea Area { get; }
}
