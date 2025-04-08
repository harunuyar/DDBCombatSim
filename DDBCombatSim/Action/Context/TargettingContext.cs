namespace DDBCombatSim.Action.Context;

public class TargettingContext
{
    public int Range { get; set; }
    public int Count { get; set; }
    public bool IncludeSelf { get; set; }
    public ETargetIntent Intent { get; set; }
}

public enum ETargetIntent
{
    Unknown,
    Harmful,
    Helpful,
    Neutral
}
