namespace DDBCombatSim.GameSystem.Request.RequestModels;

public class TargetRequest
{
    public int Range { get; set; }
    public int Count { get; set; }
    public bool IncludeSelf { get; set; }
}
