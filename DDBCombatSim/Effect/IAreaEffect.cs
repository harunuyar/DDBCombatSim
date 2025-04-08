namespace DDBCombatSim.Effect;

using DDBCombatSim.Battlefield.Area;

public interface IAreaEffect : IEffect
{
    public IArea Area { get; }
}
