namespace DDBCombatSim.GameSystem.Utils;

public enum EAdvantage
{
    None = 0,
    Advantage = 1,
    Disadvantage = 2
}

public static class EAdvantageExtensions
{
    public static bool HasAdvantage(this EAdvantage advantage)
    {
        return advantage == EAdvantage.Advantage;
    }
    public static bool HasDisadvantage(this EAdvantage advantage)
    {
        return advantage == EAdvantage.Disadvantage;
    }
}
