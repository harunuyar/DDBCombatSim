namespace DDBCombatSim.GameSystem.Utils;

public class Dice(int dieCount, int dieSize)
{
    public int DieCount { get; } = dieCount;
    public int DieSize { get; } = dieSize;
}
